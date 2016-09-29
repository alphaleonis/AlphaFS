/*  Copyright (C) 2008-2016 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
 *  
 *  Permission is hereby granted, free of charge, to any person obtaining a copy 
 *  of this software and associated documentation files (the "Software"), to deal 
 *  in the Software without restriction, including without limitation the rights 
 *  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell 
 *  copies of the Software, and to permit persons to whom the Software is 
 *  furnished to do so, subject to the following conditions:
 *  
 *  The above copyright notice and this permission notice shall be included in 
 *  all copies or substantial portions of the Software.
 *  
 *  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
 *  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
 *  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
 *  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
 *  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
 *  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN 
 *  THE SOFTWARE. 
 */

using Alphaleonis.Win32.Network;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.AccessControl;
using System.Text;

namespace Alphaleonis.Win32.Filesystem
{
   /// <summary>Provides static methods to retrieve device resource information from a local or remote host.</summary>
   public static class Device
   {
      #region EnumerateDevices

      /// <summary>Enumerates all available devices on the local host.</summary>
      /// <returns><see cref="IEnumerable{DeviceInfo}"/> instances of type <see cref="DeviceGuid"/> from the local host.</returns>
      /// <param name="deviceGuid">One of the <see cref="DeviceGuid"/> devices.</param>
      [SecurityCritical]
      public static IEnumerable<DeviceInfo> EnumerateDevices(DeviceGuid deviceGuid)
      {
         return EnumerateDevices(null, deviceGuid);
      }

      /// <summary>Enumerates all available devices of type <see cref="DeviceGuid"/> on the local or remote host.</summary>
      /// <returns><see cref="IEnumerable{DeviceInfo}"/> instances of type <see cref="DeviceGuid"/> for the specified <paramref name="hostName"/>.</returns>
      /// <param name="hostName">The name of the local or remote host on which the device resides. <see langword="null"/> refers to the local host.</param>
      /// <param name="deviceGuid">One of the <see cref="DeviceGuid"/> devices.</param>
      [SecurityCritical]
      public static IEnumerable<DeviceInfo> EnumerateDevices(string hostName, DeviceGuid deviceGuid)
      {
         return EnumerateDevicesCore(null, hostName, deviceGuid);
      }

      #endregion // EnumerateDevices

      #region Internal Methods

      #region EnumerateDevicesCore

      /// <summary>Enumerates all available devices on the local or remote host.</summary>
      [SecurityCritical]
      internal static IEnumerable<DeviceInfo> EnumerateDevicesCore(SafeHandle safeHandle, string hostName, DeviceGuid deviceInterfaceGuid)
      {
         var callerHandle = safeHandle != null;
         var deviceGuid = new Guid(Utils.GetEnumDescription(deviceInterfaceGuid));


         // CM_Connect_Machine()
         // MSDN Note: Beginning in Windows 8 and Windows Server 2012 functionality to access remote machines has been removed.
         // You cannot access remote machines when running on these versions of Windows. 
         // http://msdn.microsoft.com/en-us/library/windows/hardware/ff537948%28v=vs.85%29.aspx

         SafeCmConnectMachineHandle safeMachineHandle;
         var lastError = NativeMethods.CM_Connect_Machine(Path.LocalToUncCore(Host.GetUncName(hostName), false, false, false), out safeMachineHandle);

         if (safeMachineHandle != null && safeMachineHandle.IsInvalid)
         {
            safeMachineHandle.Close();
            NativeError.ThrowException(lastError, Resources.Handle_Is_Invalid);
         }

         using (safeMachineHandle)
         {
            // Start at the "Root" of the device tree of the specified machine.
            if (!callerHandle)
               safeHandle = NativeMethods.SetupDiGetClassDevsEx(ref deviceGuid, IntPtr.Zero, IntPtr.Zero,
                  NativeMethods.SetupDiGetClassDevsExFlags.Present |
                  NativeMethods.SetupDiGetClassDevsExFlags.DeviceInterface,
                  IntPtr.Zero, hostName, IntPtr.Zero);

            if (safeHandle != null && safeHandle.IsInvalid)
            {
               safeHandle.Close();
               NativeError.ThrowException(Marshal.GetLastWin32Error(), Resources.Handle_Is_Invalid);
            }


            try
            {
               uint memberInterfaceIndex = 0;
               var deviceInterfaceData = CreateDeviceInterfaceDataInstance();

               // Start enumerating Device Interfaces.
               while (NativeMethods.SetupDiEnumDeviceInterfaces(safeHandle, IntPtr.Zero, ref deviceGuid, memberInterfaceIndex++, ref deviceInterfaceData))
               {
                  lastError = Marshal.GetLastWin32Error();
                  if (lastError != Win32Errors.NO_ERROR)
                     NativeError.ThrowException(lastError, hostName);


                  var deviceInfoData = CreateDeviceInfoDataInstance();
                  var deviceInterfaceDetailData = GetDeviceInterfaceDetailDataInstance(safeHandle, deviceInterfaceData, deviceInfoData);

                  // Get device interace details.
                  if (!NativeMethods.SetupDiGetDeviceInterfaceDetail(safeHandle, ref deviceInterfaceData, ref deviceInterfaceDetailData, NativeMethods.DefaultFileBufferSize, IntPtr.Zero, ref deviceInfoData))
                  {
                     lastError = Marshal.GetLastWin32Error();
                     if (lastError != Win32Errors.NO_ERROR)
                        NativeError.ThrowException(lastError, hostName);
                  }

                  // Create DeviceInfo instance.
                  // Set DevicePath property of DeviceInfo instance.
                  var deviceInfo = new DeviceInfo(hostName) { DevicePath = deviceInterfaceDetailData.DevicePath };


                  // Current InstanceId is at the "USBSTOR" level, so we
                  // need up "move up" one level to get to the "USB" level.
                  uint ptrPrevious;

                  // CM_Get_Parent_Ex()
                  // Note: Using this function to access remote machines is not supported
                  // beginning with Windows 8 and Windows Server 2012, as this functionality has been removed.
                  // http://msdn.microsoft.com/en-us/library/windows/hardware/ff538615%28v=vs.85%29.aspx

                  lastError = NativeMethods.CM_Get_Parent_Ex(out ptrPrevious, deviceInfoData.DevInst, 0, safeMachineHandle);
                  if (lastError != Win32Errors.CR_SUCCESS)
                     NativeError.ThrowException(lastError, hostName);


                  // Now we get the InstanceID of the USB level device.
                  using (var safeBuffer = new SafeGlobalMemoryBufferHandle(NativeMethods.DefaultFileBufferSize))
                  {
                     // CM_Get_Device_ID_Ex()
                     // Note: Using this function to access remote machines is not supported beginning with Windows 8 and Windows Server 2012,
                     // as this functionality has been removed.
                     // http://msdn.microsoft.com/en-us/library/windows/hardware/ff538411%28v=vs.85%29.aspx

                     lastError = NativeMethods.CM_Get_Device_ID_Ex(deviceInfoData.DevInst, safeBuffer, (uint)safeBuffer.Capacity, 0, safeMachineHandle);
                     if (lastError != Win32Errors.CR_SUCCESS)
                        NativeError.ThrowException(lastError, hostName);

                     // Add to instance.
                     deviceInfo.InstanceId = safeBuffer.PtrToStringUni();
                  }

                  #region Get Registry Properties

                  using (var safeBuffer = new SafeGlobalMemoryBufferHandle(NativeMethods.DefaultFileBufferSize))
                  {
                     uint regType;
                     string dataString;
                     var safeBufferCapacity = (uint) safeBuffer.Capacity;


                     if (NativeMethods.SetupDiGetDeviceRegistryProperty(safeHandle, ref deviceInfoData, NativeMethods.SetupDiGetDeviceRegistryPropertyEnum.BaseContainerId, out regType, safeBuffer, safeBufferCapacity, IntPtr.Zero))
                     {
                        dataString = safeBuffer.PtrToStringUni();
                        if (!Utils.IsNullOrWhiteSpace(dataString))
                           deviceInfo.BaseContainerId = new Guid(dataString);
                     }

                     if (NativeMethods.SetupDiGetDeviceRegistryProperty(safeHandle, ref deviceInfoData, NativeMethods.SetupDiGetDeviceRegistryPropertyEnum.ClassGuid, out regType, safeBuffer, safeBufferCapacity, IntPtr.Zero))
                     {
                        dataString = safeBuffer.PtrToStringUni();
                        if (!Utils.IsNullOrWhiteSpace(dataString))
                           deviceInfo.ClassGuid = new Guid(dataString);
                     }


                     if (NativeMethods.SetupDiGetDeviceRegistryProperty(safeHandle, ref deviceInfoData, NativeMethods.SetupDiGetDeviceRegistryPropertyEnum.Class, out regType, safeBuffer, safeBufferCapacity, IntPtr.Zero))
                        deviceInfo.Class = safeBuffer.PtrToStringUni();

                     if (NativeMethods.SetupDiGetDeviceRegistryProperty(safeHandle, ref deviceInfoData, NativeMethods.SetupDiGetDeviceRegistryPropertyEnum.CompatibleIds, out regType, safeBuffer, safeBufferCapacity, IntPtr.Zero))
                        deviceInfo.CompatibleIds = safeBuffer.PtrToStringUni();

                     if (NativeMethods.SetupDiGetDeviceRegistryProperty(safeHandle, ref deviceInfoData, NativeMethods.SetupDiGetDeviceRegistryPropertyEnum.DeviceDescription, out regType, safeBuffer, safeBufferCapacity, IntPtr.Zero))
                        deviceInfo.DeviceDescription = safeBuffer.PtrToStringUni();

                     if (NativeMethods.SetupDiGetDeviceRegistryProperty(safeHandle, ref deviceInfoData, NativeMethods.SetupDiGetDeviceRegistryPropertyEnum.Driver, out regType, safeBuffer, safeBufferCapacity, IntPtr.Zero))
                        deviceInfo.Driver = safeBuffer.PtrToStringUni();

                     if (NativeMethods.SetupDiGetDeviceRegistryProperty(safeHandle, ref deviceInfoData, NativeMethods.SetupDiGetDeviceRegistryPropertyEnum.EnumeratorName, out regType, safeBuffer, safeBufferCapacity, IntPtr.Zero))
                        deviceInfo.EnumeratorName = safeBuffer.PtrToStringUni();

                     if (NativeMethods.SetupDiGetDeviceRegistryProperty(safeHandle, ref deviceInfoData, NativeMethods.SetupDiGetDeviceRegistryPropertyEnum.FriendlyName, out regType, safeBuffer, safeBufferCapacity, IntPtr.Zero))
                        deviceInfo.FriendlyName = safeBuffer.PtrToStringUni();

                     if (NativeMethods.SetupDiGetDeviceRegistryProperty(safeHandle, ref deviceInfoData, NativeMethods.SetupDiGetDeviceRegistryPropertyEnum.HardwareId, out regType, safeBuffer, safeBufferCapacity, IntPtr.Zero))
                        deviceInfo.HardwareId = safeBuffer.PtrToStringUni();

                     if (NativeMethods.SetupDiGetDeviceRegistryProperty(safeHandle, ref deviceInfoData, NativeMethods.SetupDiGetDeviceRegistryPropertyEnum.LocationInformation, out regType, safeBuffer, safeBufferCapacity, IntPtr.Zero))
                        deviceInfo.LocationInformation = safeBuffer.PtrToStringUni();

                     if (NativeMethods.SetupDiGetDeviceRegistryProperty(safeHandle, ref deviceInfoData, NativeMethods.SetupDiGetDeviceRegistryPropertyEnum.LocationPaths, out regType, safeBuffer, safeBufferCapacity, IntPtr.Zero))
                        deviceInfo.LocationPaths = safeBuffer.PtrToStringUni();

                     if (NativeMethods.SetupDiGetDeviceRegistryProperty(safeHandle, ref deviceInfoData, NativeMethods.SetupDiGetDeviceRegistryPropertyEnum.Manufacturer, out regType, safeBuffer, safeBufferCapacity, IntPtr.Zero))
                        deviceInfo.Manufacturer = safeBuffer.PtrToStringUni();

                     if (NativeMethods.SetupDiGetDeviceRegistryProperty(safeHandle, ref deviceInfoData, NativeMethods.SetupDiGetDeviceRegistryPropertyEnum.PhysicalDeviceObjectName, out regType, safeBuffer, safeBufferCapacity, IntPtr.Zero))
                        deviceInfo.PhysicalDeviceObjectName = safeBuffer.PtrToStringUni();

                     if (NativeMethods.SetupDiGetDeviceRegistryProperty(safeHandle, ref deviceInfoData, NativeMethods.SetupDiGetDeviceRegistryPropertyEnum.Service, out regType, safeBuffer, safeBufferCapacity, IntPtr.Zero))
                        deviceInfo.Service = safeBuffer.PtrToStringUni();
                  }

                  #endregion // Get Registry Properties

                  yield return deviceInfo;

                  // Get new structure instance.
                  deviceInterfaceData = CreateDeviceInterfaceDataInstance();
               }
            }
            finally
            {
               // Handle is ours, dispose.
               if (!callerHandle && safeHandle != null)
                  safeHandle.Close();
            }
         }
      }

      #endregion // EnumerateDevicesCore

      #region GetLinkTargetInfoCore

      /// <summary>Get information about the target of a mount point or symbolic link on an NTFS file system.</summary>
      [SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times", Justification = "Disposing is controlled.")]
      [SecurityCritical]
      internal static LinkTargetInfo GetLinkTargetInfoCore(SafeFileHandle safeHandle)
      {
         // Start with a large buffer to prevent a 2nd call.
         // MAXIMUM_REPARSE_DATA_BUFFER_SIZE = 16384
         uint bytesReturned = 4*NativeMethods.DefaultFileBufferSize;
         
         using (var safeBuffer = new SafeGlobalMemoryBufferHandle((int) bytesReturned))
         {
            while (true)
            {
               // DeviceIoControlMethod.Buffered = 0,
               // DeviceIoControlFileDevice.FileSystem = 9
               // FsctlGetReparsePoint = (DeviceIoControlFileDevice.FileSystem << 16) | (42 << 2) | DeviceIoControlMethod.Buffered | (0 << 14)

               if (!NativeMethods.DeviceIoControl(safeHandle, ((9 << 16) | (42 << 2) | 0 | (0 << 14)), IntPtr.Zero, 0, safeBuffer, (uint) safeBuffer.Capacity, out bytesReturned, IntPtr.Zero))
               {
                  var lastError = Marshal.GetLastWin32Error();
                  switch ((uint) lastError)
                  {
                     case Win32Errors.ERROR_MORE_DATA:
                     case Win32Errors.ERROR_INSUFFICIENT_BUFFER:
                        if (safeBuffer.Capacity < bytesReturned)
                        {
                           safeBuffer.Close();
                           break;
                        }

                        NativeError.ThrowException(lastError);
                        break;
                  }
               }
               else
                  break;
            }


            var marshalReparseBuffer = (int) Marshal.OffsetOf(typeof(NativeMethods.ReparseDataBufferHeader), "data");

            var header = safeBuffer.PtrToStructure<NativeMethods.ReparseDataBufferHeader>(0);

            var dataOffset = (int) (marshalReparseBuffer + (header.ReparseTag == ReparsePointTag.MountPoint
               ? Marshal.OffsetOf(typeof (NativeMethods.MountPointReparseBuffer), "data")
               : Marshal.OffsetOf(typeof (NativeMethods.SymbolicLinkReparseBuffer), "data")).ToInt64());

            var dataBuffer = new byte[bytesReturned - dataOffset];


            switch (header.ReparseTag)
            {
               case ReparsePointTag.MountPoint:
                  var mountPoint = safeBuffer.PtrToStructure<NativeMethods.MountPointReparseBuffer>(marshalReparseBuffer);

                  safeBuffer.CopyTo(dataOffset, dataBuffer, 0, dataBuffer.Length);

                  return new LinkTargetInfo(
                     Encoding.Unicode.GetString(dataBuffer, mountPoint.SubstituteNameOffset, mountPoint.SubstituteNameLength),
                     Encoding.Unicode.GetString(dataBuffer, mountPoint.PrintNameOffset, mountPoint.PrintNameLength));


               case ReparsePointTag.SymLink:
                  var symLink = safeBuffer.PtrToStructure<NativeMethods.SymbolicLinkReparseBuffer>(marshalReparseBuffer);

                  safeBuffer.CopyTo(dataOffset, dataBuffer, 0, dataBuffer.Length);

                  return new SymbolicLinkTargetInfo(
                     Encoding.Unicode.GetString(dataBuffer, symLink.SubstituteNameOffset, symLink.SubstituteNameLength),
                     Encoding.Unicode.GetString(dataBuffer, symLink.PrintNameOffset, symLink.PrintNameLength), symLink.Flags);


               default:
                  throw new UnrecognizedReparsePointException();
            }
         }
      }

      #endregion // GetLinkTargetInfoCore

      #region ToggleCompressionCore

      /// <summary>Sets the NTFS compression state of a file or directory on a volume whose file system supports per-file and per-directory compression.</summary>
      /// <param name="isFolder">Specifies that <paramref name="path"/> is a file or directory.</param>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">A path that describes a folder or file to compress or decompress.</param>
      /// <param name="compress"><see langword="true"/> = compress, <see langword="false"/> = decompress</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>

      [SecurityCritical]
      internal static void ToggleCompressionCore(bool isFolder, KernelTransaction transaction, string path, bool compress, PathFormat pathFormat)
      {
         using (var handle = File.CreateFileCore(transaction, path, isFolder ? ExtendedFileAttributes.BackupSemantics : ExtendedFileAttributes.Normal, null, FileMode.Open, FileSystemRights.Modify, FileShare.None, true, pathFormat))
         {
            // DeviceIoControlMethod.Buffered = 0,
            // DeviceIoControlFileDevice.FileSystem = 9
            // FsctlSetCompression = (DeviceIoControlFileDevice.FileSystem << 16) | (16 << 2) | DeviceIoControlMethod.Buffered | ((FileAccess.Read | FileAccess.Write) << 14)

            // 0 = Decompress, 1 = Compress.
            InvokeIoControlUnknownSize(handle, ((9 << 16) | (16 << 2) | 0 | ((uint)(FileAccess.Read | FileAccess.Write) << 14)), (compress) ? 1 : 0);
         }
      }

      #endregion // ToggleCompressionCore


      #region Private

      #region CreateDeviceInfoDataInstance

      /// <summary>Builds a DeviceInfo Data structure.</summary>
      /// <returns>An initialized NativeMethods.SP_DEVINFO_DATA instance.</returns>
      [SecurityCritical]
      private static NativeMethods.SP_DEVINFO_DATA CreateDeviceInfoDataInstance()
      {
         var did = new NativeMethods.SP_DEVINFO_DATA();
         did.cbSize = (uint) Marshal.SizeOf(did);

         return did;
      }

      #endregion // CreateDeviceInfoDataInstance

      #region CreateDeviceInterfaceDataInstance

      /// <summary>Builds a Device Interface Data structure.</summary>
      /// <returns>An initialized NativeMethods.SP_DEVICE_INTERFACE_DATA instance.</returns>
      [SecurityCritical]
      private static NativeMethods.SP_DEVICE_INTERFACE_DATA CreateDeviceInterfaceDataInstance()
      {
         var did = new NativeMethods.SP_DEVICE_INTERFACE_DATA();
         did.cbSize = (uint) Marshal.SizeOf(did);

         return did;
      }

      #endregion // CreateDeviceInterfaceDataInstance

      #region GetDeviceInterfaceDetailDataInstance

      /// <summary>Builds a Device Interface Detail Data structure.</summary>
      /// <returns>An initialized NativeMethods.SP_DEVICE_INTERFACE_DETAIL_DATA instance.</returns>
      [SecurityCritical]
      private static NativeMethods.SP_DEVICE_INTERFACE_DETAIL_DATA GetDeviceInterfaceDetailDataInstance(SafeHandle safeHandle, NativeMethods.SP_DEVICE_INTERFACE_DATA deviceInterfaceData, NativeMethods.SP_DEVINFO_DATA deviceInfoData)
      {
         // Build a Device Interface Detail Data structure.
         var didd = new NativeMethods.SP_DEVICE_INTERFACE_DETAIL_DATA
         {
            cbSize = (IntPtr.Size == 4) ? (uint) (Marshal.SystemDefaultCharSize + 4) : 8
         };

         // Get device interace details.
         if (!NativeMethods.SetupDiGetDeviceInterfaceDetail(safeHandle, ref deviceInterfaceData, ref didd, NativeMethods.DefaultFileBufferSize, IntPtr.Zero, ref deviceInfoData))
         {
            var lastError = Marshal.GetLastWin32Error();
            if (lastError != Win32Errors.NO_ERROR)
               NativeError.ThrowException(lastError);
         }

         return didd;
      }

      #endregion // GetDeviceInterfaceDetailDataInstance

      #region InvokeIoControlUnknownSize

      /// <summary>Repeatedly invokes InvokeIoControl with the specified input until enough memory has been allocated.</summary>
      [SecurityCritical]
      private static byte[] InvokeIoControlUnknownSize<TV>(SafeFileHandle handle, uint controlCode, TV input, uint increment = 128)
      {
         byte[] output;
         uint bytesReturned;

         var inputSize = (uint) Marshal.SizeOf(input);
         var outputLength = increment;

         do
         {
            output = new byte[outputLength];
            if (!NativeMethods.DeviceIoControl(handle, controlCode, input, inputSize, output, outputLength, out bytesReturned, IntPtr.Zero))
            {
               var lastError = Marshal.GetLastWin32Error();
               switch ((uint)lastError)
               {
                  case Win32Errors.ERROR_MORE_DATA:
                  case Win32Errors.ERROR_INSUFFICIENT_BUFFER:
                     outputLength += increment;
                     break;

                  default:
                     NativeError.ThrowException(lastError);
                     break;
               }
            }
            else
               break;

         } while (true);

         // Return the result
         if (output.Length == bytesReturned)
            return output;

         var res = new byte[bytesReturned];
         Array.Copy(output, res, bytesReturned);

         return res;
      }

      #endregion // InvokeIoControlUnknownSize

      #endregion // Private

      #endregion // Internal Methods
   }
}
