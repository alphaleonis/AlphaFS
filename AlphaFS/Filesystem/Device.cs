/* Copyright (c) 2008-2014 Peter Palotas, Jeffrey Jangli, Normalex
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

      /// <summary>Enumerates all available devices on the local.</summary>
      /// <param name="deviceGuid">One of the <see cref="T:DeviceGuid"/> devices.</param>
      /// <returns>Returns <see cref="T:IEnumerable{DeviceInfo}"/> instances of type <see cref="T:DeviceGuid"/> from the local host.</returns>
      [SecurityCritical]
      public static IEnumerable<DeviceInfo> EnumerateDevices(DeviceGuid deviceGuid)
      {
         return EnumerateDevices(null, deviceGuid);
      }

      /// <summary>Enumerates all available devices of type <see cref="T:DeviceGuid"/> on the local or remote host.</summary>
      /// <param name="hostName">The name of the local or remote host on which the device resides. <c>null</c> refers to the local host.</param>
      /// <param name="deviceGuid">One of the <see cref="T:DeviceGuid"/> devices.</param>
      /// <returns>Returns <see cref="T:IEnumerable{DeviceInfo}"/> instances of type <see cref="T:DeviceGuid"/> for the specified <paramref name="hostName"/>.</returns>
      [SecurityCritical]
      public static IEnumerable<DeviceInfo> EnumerateDevices(string hostName, DeviceGuid deviceGuid)
      {
         return EnumerateDevicesInternal(null, hostName, deviceGuid);
      }

      #endregion // EnumerateDevices


      #region Unified Internals

      #region EnumerateDevicesInternal

      /// <summary>Enumerates all available devices on the local or remote host.</summary>
      /// <exception cref="NativeError.ThrowException()"></exception>
      [SecurityCritical]
      internal static IEnumerable<DeviceInfo> EnumerateDevicesInternal(SafeHandle safeHandle, string hostName, DeviceGuid deviceInterfaceGuid)
      {
         bool callerHandle = safeHandle != null;
         Guid deviceGuid = new Guid(NativeMethods.GetEnumDescription(deviceInterfaceGuid));

         
         // CM_Connect_Machine()
         // MSDN Note: Beginning in Windows 8 and Windows Server 2012 functionality to access remote machines has been removed.
         // You cannot access remote machines when running on these versions of Windows. 
         // http://msdn.microsoft.com/en-us/library/windows/hardware/ff537948%28v=vs.85%29.aspx

         SafeCmConnectMachineHandle safeMachineHandle;
         int lastError = NativeMethods.CM_Connect_Machine(Path.LocalToUncInternal(Host.GetUncName(hostName), false, false, false, false), out safeMachineHandle);
         if (safeMachineHandle.IsInvalid)
         {
            safeMachineHandle.Close();
            NativeError.ThrowException(lastError, Resources.HandleInvalid);
         }

         using (safeMachineHandle)
         {
            // Start at the "Root" of the device tree of the specified machine.
            if (!callerHandle)
               safeHandle = NativeMethods.SetupDiGetClassDevsEx(ref deviceGuid, IntPtr.Zero, IntPtr.Zero,
                  NativeMethods.SetupDiGetClassDevsExFlags.Present |
                  NativeMethods.SetupDiGetClassDevsExFlags.DeviceInterface,
                  IntPtr.Zero, hostName, IntPtr.Zero);

            if (safeHandle.IsInvalid)
            {
               safeHandle.Close();
               NativeError.ThrowException(Marshal.GetLastWin32Error(), Resources.HandleInvalid);
            }
                 
            
            try
            {
               uint memberInterfaceIndex = 0;
               NativeMethods.SpDeviceInterfaceData deviceInterfaceData = CreateDeviceInterfaceDataInstance();

               // Start enumerating Device Interfaces.
               while (NativeMethods.SetupDiEnumDeviceInterfaces(safeHandle, IntPtr.Zero, ref deviceGuid, memberInterfaceIndex++, ref deviceInterfaceData))
               {
                  lastError = Marshal.GetLastWin32Error();
                  if (lastError != Win32Errors.NO_ERROR)
                     NativeError.ThrowException(lastError, hostName);


                  NativeMethods.SpDeviceInfoData deviceInfoData = CreateDeviceInfoDataInstance();
                  NativeMethods.SpDeviceInterfaceDetailData deviceInterfaceDetailData = GetDeviceInterfaceDetailDataInstance(safeHandle, deviceInterfaceData, deviceInfoData);

                  // Get device interace details.
                  if (!NativeMethods.SetupDiGetDeviceInterfaceDetail(safeHandle, ref deviceInterfaceData, ref deviceInterfaceDetailData, NativeMethods.DefaultFileBufferSize, IntPtr.Zero, ref deviceInfoData))
                  {
                     lastError = Marshal.GetLastWin32Error();
                     if (lastError != Win32Errors.NO_ERROR)
                        NativeError.ThrowException(lastError, hostName);
                  }

                  // Create DeviceInfo instance.
                  // Set DevicePath property of DeviceInfo instance.
                  DeviceInfo deviceInfo = new DeviceInfo(hostName) {DevicePath = deviceInterfaceDetailData.DevicePath};


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
                  using (SafeGlobalMemoryBufferHandle safeBuffer = new SafeGlobalMemoryBufferHandle(NativeMethods.DefaultFileBufferSize))
                  {
                     // CM_Get_Device_ID_Ex()
                     // Note: Using this function to access remote machines is not supported beginning with Windows 8 and Windows Server 2012,
                     // as this functionality has been removed.
                     // http://msdn.microsoft.com/en-us/library/windows/hardware/ff538411%28v=vs.85%29.aspx

                     lastError = NativeMethods.CM_Get_Device_ID_Ex(deviceInfoData.DevInst, safeBuffer, (uint)safeBuffer.Capacity, 0, safeMachineHandle);
                     if (lastError != Win32Errors.CR_SUCCESS)
                        NativeError.ThrowException(lastError, hostName);


                     // CA2001:AvoidCallingProblematicMethods

                     IntPtr buffer = IntPtr.Zero;
                     bool successRef = false;
                     safeBuffer.DangerousAddRef(ref successRef);

                     // MSDN: The DangerousGetHandle method poses a security risk because it can return a handle that is not valid.
                     if (successRef)
                        buffer = safeBuffer.DangerousGetHandle();

                     safeBuffer.DangerousRelease();

                     if (buffer == IntPtr.Zero)
                        NativeError.ThrowException(Resources.HandleDangerousRef);

                     // CA2001:AvoidCallingProblematicMethods


                     // Add to instance.
                     deviceInfo.InstanceId = Marshal.PtrToStringUni(buffer);
                  }

                  #region Get Registry Properties

                  using (SafeGlobalMemoryBufferHandle safeBuffer = new SafeGlobalMemoryBufferHandle(NativeMethods.DefaultFileBufferSize))
                  {
                     uint regType;
                     string dataString;


                     // CA2001:AvoidCallingProblematicMethods

                     IntPtr buffer = IntPtr.Zero;
                     bool successRef = false;
                     safeBuffer.DangerousAddRef(ref successRef);

                     // MSDN: The DangerousGetHandle method poses a security risk because it can return a handle that is not valid.
                     if (successRef)
                        buffer = safeBuffer.DangerousGetHandle();

                     safeBuffer.DangerousRelease();

                     if (buffer == IntPtr.Zero)
                        NativeError.ThrowException(Resources.HandleDangerousRef);

                     // CA2001:AvoidCallingProblematicMethods


                     uint safeBufferCapacity = (uint) safeBuffer.Capacity;


                     if (NativeMethods.SetupDiGetDeviceRegistryProperty(safeHandle, ref deviceInfoData, NativeMethods.SetupDiGetDeviceRegistryPropertyEnum.BaseContainerId, out regType, safeBuffer, safeBufferCapacity, IntPtr.Zero))
                     {
                        dataString = Marshal.PtrToStringUni(buffer);
                        if (!Utils.IsNullOrWhiteSpace(dataString))
                           deviceInfo.BaseContainerId = new Guid(dataString);
                     }

                     if (NativeMethods.SetupDiGetDeviceRegistryProperty(safeHandle, ref deviceInfoData, NativeMethods.SetupDiGetDeviceRegistryPropertyEnum.ClassGuid, out regType, safeBuffer, safeBufferCapacity, IntPtr.Zero))
                     {
                        dataString = Marshal.PtrToStringUni(buffer);
                        if (!Utils.IsNullOrWhiteSpace(dataString))
                           deviceInfo.ClassGuid = new Guid(dataString);
                     }


                     if (NativeMethods.SetupDiGetDeviceRegistryProperty(safeHandle, ref deviceInfoData, NativeMethods.SetupDiGetDeviceRegistryPropertyEnum.Class, out regType, safeBuffer, safeBufferCapacity, IntPtr.Zero))
                        deviceInfo.Class = Marshal.PtrToStringUni(buffer);
                     
                     if (NativeMethods.SetupDiGetDeviceRegistryProperty(safeHandle, ref deviceInfoData, NativeMethods.SetupDiGetDeviceRegistryPropertyEnum.CompatibleIds, out regType, safeBuffer, safeBufferCapacity, IntPtr.Zero))
                        deviceInfo.CompatibleIds = Marshal.PtrToStringUni(buffer);

                     if (NativeMethods.SetupDiGetDeviceRegistryProperty(safeHandle, ref deviceInfoData, NativeMethods.SetupDiGetDeviceRegistryPropertyEnum.DeviceDescription, out regType, safeBuffer, safeBufferCapacity, IntPtr.Zero))
                        deviceInfo.DeviceDescription = Marshal.PtrToStringUni(buffer);

                     if (NativeMethods.SetupDiGetDeviceRegistryProperty(safeHandle, ref deviceInfoData, NativeMethods.SetupDiGetDeviceRegistryPropertyEnum.Driver, out regType, safeBuffer, safeBufferCapacity, IntPtr.Zero))
                        deviceInfo.Driver = Marshal.PtrToStringUni(buffer);

                     if (NativeMethods.SetupDiGetDeviceRegistryProperty(safeHandle, ref deviceInfoData, NativeMethods.SetupDiGetDeviceRegistryPropertyEnum.EnumeratorName, out regType, safeBuffer, safeBufferCapacity, IntPtr.Zero))
                        deviceInfo.EnumeratorName = Marshal.PtrToStringUni(buffer);

                     if (NativeMethods.SetupDiGetDeviceRegistryProperty(safeHandle, ref deviceInfoData, NativeMethods.SetupDiGetDeviceRegistryPropertyEnum.FriendlyName, out regType, safeBuffer, safeBufferCapacity, IntPtr.Zero))
                        deviceInfo.FriendlyName = Marshal.PtrToStringUni(buffer);

                     if (NativeMethods.SetupDiGetDeviceRegistryProperty(safeHandle, ref deviceInfoData, NativeMethods.SetupDiGetDeviceRegistryPropertyEnum.HardwareId, out regType, safeBuffer, safeBufferCapacity, IntPtr.Zero))
                        deviceInfo.HardwareId = Marshal.PtrToStringUni(buffer);

                     if (NativeMethods.SetupDiGetDeviceRegistryProperty(safeHandle, ref deviceInfoData, NativeMethods.SetupDiGetDeviceRegistryPropertyEnum.LocationInformation, out regType, safeBuffer, safeBufferCapacity, IntPtr.Zero))
                        deviceInfo.LocationInformation = Marshal.PtrToStringUni(buffer);

                     if (NativeMethods.SetupDiGetDeviceRegistryProperty(safeHandle, ref deviceInfoData, NativeMethods.SetupDiGetDeviceRegistryPropertyEnum.LocationPaths, out regType, safeBuffer, safeBufferCapacity, IntPtr.Zero))
                        deviceInfo.LocationPaths = Marshal.PtrToStringUni(buffer);

                     if (NativeMethods.SetupDiGetDeviceRegistryProperty(safeHandle, ref deviceInfoData, NativeMethods.SetupDiGetDeviceRegistryPropertyEnum.Manufacturer, out regType, safeBuffer, safeBufferCapacity, IntPtr.Zero))
                        deviceInfo.Manufacturer = Marshal.PtrToStringUni(buffer);

                     if (NativeMethods.SetupDiGetDeviceRegistryProperty(safeHandle, ref deviceInfoData, NativeMethods.SetupDiGetDeviceRegistryPropertyEnum.PhysicalDeviceObjectName, out regType, safeBuffer, safeBufferCapacity, IntPtr.Zero))
                        deviceInfo.PhysicalDeviceObjectName = Marshal.PtrToStringUni(buffer);

                     if (NativeMethods.SetupDiGetDeviceRegistryProperty(safeHandle, ref deviceInfoData, NativeMethods.SetupDiGetDeviceRegistryPropertyEnum.Service, out regType, safeBuffer, safeBufferCapacity, IntPtr.Zero))
                        deviceInfo.Service = Marshal.PtrToStringUni(buffer);
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

      #endregion // EnumerateDevicesInternal

      #region GetLinkTargetInfoInternal

      /// <summary>Unified method GetLinkTargetInfoInternal() to get information about the target of a mount point or symbolic link on an NTFS file system.</summary>
      /// <exception cref="NativeError.ThrowException()"></exception>
      [SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
      [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
      [SuppressMessage("Microsoft.Reliability", "CA2001:AvoidCallingProblematicMethods", MessageId = "System.Runtime.InteropServices.SafeHandle.DangerousGetHandle", Justification = "DangerousAddRef() and DangerousRelease() are applied.")]
      [SecurityCritical]
      internal static LinkTargetInfo GetLinkTargetInfoInternal(SafeFileHandle safeHandle)
      {
         // Start with a large buffer to prevent a 2nd call.
         uint bytesReturned = NativeMethods.MaxPathUnicode;

         using (SafeGlobalMemoryBufferHandle safeBuffer = new SafeGlobalMemoryBufferHandle((int)bytesReturned))
         { 
            do
            {
               // Possible PInvoke signature bug: safeBuffer.Capacity and bytesReturned are always the same.
               // Since a large buffer is used, we are not affected.

               // DeviceIoControlMethod.Buffered = 0,
               // DeviceIoControlFileDevice.FileSystem = 9
               // FsctlGetReparsePoint = (DeviceIoControlFileDevice.FileSystem << 16) | (42 << 2) | DeviceIoControlMethod.Buffered | (0 << 14)

               if (!NativeMethods.DeviceIoControl(safeHandle, ((9 << 16) | (42 << 2) | 0 | (0 << 14)), IntPtr.Zero, 0, safeBuffer, (uint) safeBuffer.Capacity, out bytesReturned, IntPtr.Zero))
               {
                  int lastError = Marshal.GetLastWin32Error();
                  switch ((uint)lastError)
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
            } while (true);


            
            // CA2001:AvoidCallingProblematicMethods

            IntPtr buffer = IntPtr.Zero;
            bool successRef = false;
            safeBuffer.DangerousAddRef(ref successRef);

            // MSDN: The DangerousGetHandle method poses a security risk because it can return a handle that is not valid.
            if (successRef)
               buffer = safeBuffer.DangerousGetHandle();

            safeBuffer.DangerousRelease();

            if (buffer == IntPtr.Zero)
               NativeError.ThrowException(Resources.HandleDangerousRef);

            // CA2001:AvoidCallingProblematicMethods
 
            
            Type toMountPointReparseBuffer = typeof(NativeMethods.MountPointReparseBuffer);
            Type toReparseDataBufferHeader = typeof(NativeMethods.ReparseDataBufferHeader);
            Type toSymbolicLinkReparseBuffer = typeof(NativeMethods.SymbolicLinkReparseBuffer);
            IntPtr marshalReparseBuffer = Marshal.OffsetOf(toReparseDataBufferHeader, "data");

            NativeMethods.ReparseDataBufferHeader header = NativeMethods.GetStructure<NativeMethods.ReparseDataBufferHeader>(0, buffer);
               
            IntPtr dataPos;
            byte[] dataBuffer;

            switch (header.ReparseTag)
            {
               case ReparsePointTag.MountPoint:
                  NativeMethods.MountPointReparseBuffer mprb = NativeMethods.GetStructure<NativeMethods.MountPointReparseBuffer>(0, new IntPtr(buffer.ToInt64() + marshalReparseBuffer.ToInt64()));

                  dataPos = new IntPtr(marshalReparseBuffer.ToInt64() + Marshal.OffsetOf(toMountPointReparseBuffer, "data").ToInt64());
                  dataBuffer = new byte[bytesReturned - dataPos.ToInt64()];

                  Marshal.Copy(new IntPtr(buffer.ToInt64() + dataPos.ToInt64()), dataBuffer, 0, dataBuffer.Length);

                  return new LinkTargetInfo(
                     Encoding.Unicode.GetString(dataBuffer, mprb.SubstituteNameOffset, mprb.SubstituteNameLength),
                     Encoding.Unicode.GetString(dataBuffer, mprb.PrintNameOffset, mprb.PrintNameLength));

               
               case ReparsePointTag.SymLink:
                  NativeMethods.SymbolicLinkReparseBuffer slrb = NativeMethods.GetStructure<NativeMethods.SymbolicLinkReparseBuffer>(0, new IntPtr(buffer.ToInt64() + marshalReparseBuffer.ToInt64()));

                  dataPos = new IntPtr(marshalReparseBuffer.ToInt64() + Marshal.OffsetOf(toSymbolicLinkReparseBuffer, "data").ToInt64());
                  dataBuffer = new byte[bytesReturned - dataPos.ToInt64()];

                  Marshal.Copy(new IntPtr(buffer.ToInt64() + dataPos.ToInt64()), dataBuffer, 0, dataBuffer.Length);

                  return new SymbolicLinkTargetInfo(
                     Encoding.Unicode.GetString(dataBuffer, slrb.SubstituteNameOffset, slrb.SubstituteNameLength),
                     Encoding.Unicode.GetString(dataBuffer, slrb.PrintNameOffset, slrb.PrintNameLength), slrb.Flags);

               
               default:
                  throw new UnrecognizedReparsePointException();
            }
         }
      }

      #endregion // GetLinkTargetInfoInternal

      #region ToggleCompressionInternal

      /// <summary>Unified method ToggleCompressionInternal() to set the NTFS compression state of a file or directory on a volume whose file system supports per-file and per-directory compression.</summary>
      /// <param name="isFolder">Specifies that <paramref name="path"/> is a file or directory.</param>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">A path that describes a folder or file to compress or decompress.</param>
      /// <param name="compress"><c>true</c> = compress, <c>false</c> = decompress</param>
      /// <param name="isFullPath">
      /// <para><c>true</c> <paramref name="path"/> is an absolute path. Unicode prefix is applied.</para>
      /// <para><c>false</c> <paramref name="path"/> will be checked and resolved to an absolute path. Unicode prefix is applied.</para>
      /// <para><c>null</c> <paramref name="path"/> is already an absolute path with Unicode prefix. Use as is.</para>
      /// </param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      internal static void ToggleCompressionInternal(bool isFolder, KernelTransaction transaction, string path, bool compress, bool? isFullPath)
      {
         using (SafeFileHandle handle = File.CreateFileInternal(!isFolder, transaction, path, isFolder ? EFileAttributes.BackupSemantics : EFileAttributes.Normal, null, FileMode.Open, FileSystemRights.Modify, FileShare.None, isFullPath))
         {
            // DeviceIoControlMethod.Buffered = 0,
            // DeviceIoControlFileDevice.FileSystem = 9
            // FsctlSetCompression = (DeviceIoControlFileDevice.FileSystem << 16) | (16 << 2) | DeviceIoControlMethod.Buffered | ((FileAccess.Read | FileAccess.Write) << 14)

            // 0 = Decompress, 1 = Compress.
            InvokeIoControlUnknownSize(handle, ((9 << 16) | (16 << 2) | 0 | ((uint)(FileAccess.Read | FileAccess.Write) << 14)), (compress) ? 1 : 0);
         }
      }

      #endregion // ToggleCompressionInternal


      #region Private

      #region CreateDeviceInfoDataInstance

      /// <summary>Builds a DeviceInfo Data structure.</summary>
      /// <returns>An initialized <see cref="T:NativeMethods.SpDeviceInfoData"/> instance.</returns>
      [SecurityCritical]
      private static NativeMethods.SpDeviceInfoData CreateDeviceInfoDataInstance()
      {
         NativeMethods.SpDeviceInfoData did = new NativeMethods.SpDeviceInfoData();
         did.cbSize = (uint) Marshal.SizeOf(did);

         return did;
      }

      #endregion // CreateDeviceInfoDataInstance

      #region CreateDeviceInterfaceDataInstance

      /// <summary>Builds a Device Interface Data structure.</summary>
      /// <returns>An initialized <see cref="T:NativeMethods.SpDeviceInterfaceData"/> instance.</returns>
      [SecurityCritical]
      private static NativeMethods.SpDeviceInterfaceData CreateDeviceInterfaceDataInstance()
      {
         NativeMethods.SpDeviceInterfaceData did = new NativeMethods.SpDeviceInterfaceData();
         did.cbSize = (uint) Marshal.SizeOf(did);

         return did;
      }

      #endregion // CreateDeviceInterfaceDataInstance

      #region GetDeviceInterfaceDetailDataInstance

      /// <summary>Builds a Device Interface Detail Data structure.</summary>
      /// <returns>An initialized <see cref="T:NativeMethods.SpDeviceInterfaceDetailData"/> instance.</returns>
      [SecurityCritical]
      private static NativeMethods.SpDeviceInterfaceDetailData GetDeviceInterfaceDetailDataInstance(SafeHandle safeHandle, NativeMethods.SpDeviceInterfaceData deviceInterfaceData, NativeMethods.SpDeviceInfoData deviceInfoData)
      {
         // Build a Device Interface Detail Data structure.
         NativeMethods.SpDeviceInterfaceDetailData didd = new NativeMethods.SpDeviceInterfaceDetailData
         {
            cbSize = (IntPtr.Size == 4) ? (uint) (Marshal.SystemDefaultCharSize + 4) : 8
         };

         // Get device interace details.
         if (!NativeMethods.SetupDiGetDeviceInterfaceDetail(safeHandle, ref deviceInterfaceData, ref didd, NativeMethods.DefaultFileBufferSize, IntPtr.Zero, ref deviceInfoData))
         {
            int lastError = Marshal.GetLastWin32Error();
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

         uint inputSize = (uint)Marshal.SizeOf(input);
         uint outputLength = increment;

         do
         {
            output = new byte[outputLength];
            if (!NativeMethods.DeviceIoControl(handle, controlCode, input, inputSize, output, outputLength, out bytesReturned, IntPtr.Zero))
            {
               int lastError = Marshal.GetLastWin32Error();
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

         byte[] res = new byte[bytesReturned];
         Array.Copy(output, res, bytesReturned);

         return res;
      }

      #endregion // InvokeIoControlUnknownSize

      #endregion // Private

      #endregion // Unified Internals
   }
}