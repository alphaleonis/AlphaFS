/*  Copyright (C) 2008-2018 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
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

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using Alphaleonis.Win32.Filesystem;
using Alphaleonis.Win32.Network;

namespace Alphaleonis.Win32.Device
{
   public static partial class Local
   {
      /// <summary>[AlphaFS] Enumerates all available devices on the local host.</summary>
      /// <returns>Returns an <see cref="IEnumerable{DeviceInfo}"/> collection from the local host.</returns>
      /// <param name="deviceGuid">One of the <see cref="DeviceGuid"/> device guids.</param>
      [SecurityCritical]
      public static IEnumerable<DeviceInfo> EnumerateDevices(DeviceGuid deviceGuid)
      {
         return EnumerateDevicesCore(null, new[] {deviceGuid}, true);
      }


      /// <summary>[AlphaFS] Enumerates all available devices of type <see cref="DeviceGuid"/> on the local host.</summary>
      /// <returns>Returns an <see cref="IEnumerable{DeviceInfo}"/> collection from the local host.</returns>
      /// <param name="deviceGuid">One or more <see cref="DeviceGuid"/> device guids.</param>
      [SecurityCritical]
      public static IEnumerable<DeviceInfo> EnumerateDevices(DeviceGuid[] deviceGuid)
      {
         return EnumerateDevicesCore(null, deviceGuid, true);
      }


      /// <summary>[AlphaFS] Enumerates all available devices of type <see cref="DeviceGuid"/> on the local or remote host.</summary>
      /// <returns>Returns an <see cref="IEnumerable{DeviceInfo}"/> collection for the specified <paramref name="hostName"/>.</returns>
      /// <remarks>
      ///   MSDN Note: Beginning in Windows 8 and Windows Server 2012 functionality to access remote machines has been removed.
      ///   You cannot access remote machines when running on these versions of Windows.
      ///   <para>http://msdn.microsoft.com/en-us/library/windows/hardware/ff537948%28v=vs.85%29.aspx</para>
      /// </remarks>
      /// <param name="hostName">The name of the local or remote host on which the device resides. <c>null</c> refers to the local host.</param>
      /// <param name="deviceGuid">One of the <see cref="DeviceGuid"/> device guids.</param>
      [SecurityCritical]
      public static IEnumerable<DeviceInfo> EnumerateDevices(string hostName, DeviceGuid deviceGuid)
      {
         return EnumerateDevicesCore(hostName, new[] {deviceGuid}, true);
      }


      /// <summary>[AlphaFS] Enumerates all available devices of type <see cref="DeviceGuid"/> on the local or remote host.</summary>
      /// <returns>Returns an <see cref="IEnumerable{DeviceInfo}"/> collection for the specified <paramref name="hostName"/>.</returns>
      /// <remarks>
      ///   MSDN Note: Beginning in Windows 8 and Windows Server 2012 functionality to access remote machines has been removed.
      ///   You cannot access remote machines when running on these versions of Windows.
      ///   <para>http://msdn.microsoft.com/en-us/library/windows/hardware/ff537948%28v=vs.85%29.aspx</para>
      /// </remarks>
      /// <param name="hostName">The name of the local or remote host on which the device resides. <c>null</c> refers to the local host.</param>
      /// <param name="deviceGuid">One or more <see cref="DeviceGuid"/> device guids.</param>
      [SecurityCritical]
      public static IEnumerable<DeviceInfo> EnumerateDevices(string hostName, DeviceGuid[] deviceGuid)
      {
         return EnumerateDevicesCore(hostName, deviceGuid, true);
      }


      /// <summary>[AlphaFS] Enumerates all available devices of type <see cref="DeviceGuid"/> on the local or remote host.</summary>
      /// <returns>Returns an <see cref="IEnumerable{DeviceInfo}"/> collection for the specified <paramref name="hostName"/>.</returns>
      /// <remarks>
      ///   MSDN Note: Beginning in Windows 8 and Windows Server 2012 functionality to access remote machines has been removed.
      ///   You cannot access remote machines when running on these versions of Windows.
      ///   <para>http://msdn.microsoft.com/en-us/library/windows/hardware/ff537948%28v=vs.85%29.aspx</para>
      /// </remarks>
      /// <param name="hostName">The name of the local or remote host on which the device resides. <c>null</c> refers to the local host.</param>
      /// <param name="deviceGuid">One or more <see cref="DeviceGuid"/> device guids.</param>
      /// <param name="getAllProperties"><c>true</c> to retrieve all device properties.</param>
      [SecurityCritical]
      internal static IEnumerable<DeviceInfo> EnumerateDevicesCore(string hostName, DeviceGuid[] deviceGuid, bool getAllProperties)
      {
         SafeCmConnectMachineHandle safeMachineHandle = null;

         var isRemote = !Utils.IsNullOrWhiteSpace(hostName);
         if (isRemote)
         {
            var lastError = NativeMethods.CM_Connect_Machine(Host.GetUncName(hostName), out safeMachineHandle);

            Utils.IsValidHandle(safeMachineHandle, lastError);
         }


         using (safeMachineHandle)
            foreach (var guid in deviceGuid)
            foreach (var device in EnumerateDevicesNative(safeMachineHandle, hostName, guid, getAllProperties))
               yield return device;
      }


      /// <summary>[AlphaFS] Enumerates all available devices of type <see cref="DeviceGuid"/> on the local or remote host.</summary>
      /// <returns>Returns an <see cref="IEnumerable{DeviceInfo}"/> collection for the specified <paramref name="safeMachineHandle"/>.</returns>
      /// <remarks>
      ///   MSDN Note: Beginning in Windows 8 and Windows Server 2012 functionality to access remote machines has been removed.
      ///   You cannot access remote machines when running on these versions of Windows.
      ///   <para>http://msdn.microsoft.com/en-us/library/windows/hardware/ff537948%28v=vs.85%29.aspx</para>
      /// </remarks>
      /// <param name="safeMachineHandle">An initialized <see cref="SafeCmConnectMachineHandle"/> instance.</param>
      /// <param name="hostName">The name of the local or remote host on which the device resides. <c>null</c> refers to the local host.</param>
      /// <param name="deviceGuid">One of the <see cref="DeviceGuid"/> devices.</param>
      /// <param name="getAllProperties"><c>true</c> to retrieve all device properties.</param>
      [SecurityCritical]
      internal static IEnumerable<DeviceInfo> EnumerateDevicesNative(SafeCmConnectMachineHandle safeMachineHandle, string hostName, DeviceGuid deviceGuid, bool getAllProperties)
      {
         var nonNullHostName = !Utils.IsNullOrWhiteSpace(hostName) ? hostName : Environment.MachineName;

         var classGuid = new Guid(Utils.GetEnumDescription(deviceGuid));
         

         // Start at the "Root" of the device tree.

         using (var safeHandle = NativeMethods.SetupDiGetClassDevsEx(ref classGuid, IntPtr.Zero, IntPtr.Zero, NativeMethods.DEVICE_INFORMATION_FLAGS.DIGCF_PRESENT | NativeMethods.DEVICE_INFORMATION_FLAGS.DIGCF_DEVICEINTERFACE, IntPtr.Zero, hostName, IntPtr.Zero))
         {
            var lastError = Marshal.GetLastWin32Error();

            Utils.IsValidHandle(safeHandle, lastError);
            
            uint memberInterfaceIndex = 0;
            var interfaceStructSize = (uint) Marshal.SizeOf(typeof(NativeMethods.SP_DEVICE_INTERFACE_DATA));
            var dataStructSize = (uint) Marshal.SizeOf(typeof(NativeMethods.SP_DEVINFO_DATA));


            // Start enumerating device interfaces.
            
            while (true)
            {
               var interfaceData = new NativeMethods.SP_DEVICE_INTERFACE_DATA {cbSize = interfaceStructSize};

               var success = NativeMethods.SetupDiEnumDeviceInterfaces(safeHandle, IntPtr.Zero, ref classGuid, memberInterfaceIndex++, ref interfaceData);

               lastError = Marshal.GetLastWin32Error();

               if (!success)
               {
                  if (lastError != Win32Errors.NO_ERROR && lastError != Win32Errors.ERROR_NO_MORE_ITEMS)
                     NativeError.ThrowException(lastError, nonNullHostName);

                  break;
               }


               // Create DeviceInfo instance.

               var diData = new NativeMethods.SP_DEVINFO_DATA {cbSize = dataStructSize};

               var deviceInfo = new DeviceInfo(nonNullHostName) {DevicePath = GetDeviceInterfaceDetail(safeHandle, ref interfaceData, ref diData).DevicePath};


               if (getAllProperties)
               {
                  deviceInfo.InstanceId = GetDeviceInstanceId(safeMachineHandle, nonNullHostName, diData);

                  SetDeviceProperties(safeHandle, deviceInfo, diData);
               }

               else
                  SetMinimalDeviceProperties(safeHandle, deviceInfo, diData);


               yield return deviceInfo;
            }
         }
      }




      [SecurityCritical]
      private static string GetDeviceInstanceId(SafeCmConnectMachineHandle safeMachineHandle, string hostNameForException, NativeMethods.SP_DEVINFO_DATA diData)
      {
         uint ptrPrevious;

         var lastError = NativeMethods.CM_Get_Parent_Ex(out ptrPrevious, diData.DevInst, 0, safeMachineHandle);

         if (lastError != Win32Errors.CR_SUCCESS)
            NativeError.ThrowException(lastError, hostNameForException);


         using (var safeBuffer = new SafeGlobalMemoryBufferHandle(Filesystem.NativeMethods.DefaultFileBufferSize / 8)) // 512
         {
            lastError = NativeMethods.CM_Get_Device_ID_Ex(diData.DevInst, safeBuffer, (uint) safeBuffer.Capacity, 0, safeMachineHandle);

            if (lastError != Win32Errors.CR_SUCCESS)
               NativeError.ThrowException(lastError, hostNameForException);


            // Device InstanceID, such as: "USB\VID_8087&PID_0A2B\5&2EDA7E1E&0&7", "SCSI\DISK&VEN_SANDISK&PROD_X400\4&288ED25&0&000200", ...

            return safeBuffer.PtrToStringUni();
         }
      }


      /// <summary>Builds a Device Interface Detail Data structure.</summary>
      /// <returns>Returns an initialized NativeMethods.SP_DEVICE_INTERFACE_DETAIL_DATA instance.</returns>
      [SecurityCritical]
      private static NativeMethods.SP_DEVICE_INTERFACE_DETAIL_DATA GetDeviceInterfaceDetail(SafeHandle safeHandle, ref NativeMethods.SP_DEVICE_INTERFACE_DATA interfaceData, ref NativeMethods.SP_DEVINFO_DATA infoData)
      {
         var didd = new NativeMethods.SP_DEVICE_INTERFACE_DETAIL_DATA {cbSize = (uint) (IntPtr.Size == 4 ? 6 : 8)};

         var success = NativeMethods.SetupDiGetDeviceInterfaceDetail(safeHandle, ref interfaceData, ref didd, (uint) Marshal.SizeOf(didd), IntPtr.Zero, ref infoData);

         var lastError = Marshal.GetLastWin32Error();

         if (!success)
            NativeError.ThrowException(lastError);

         return didd;
      }


      [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "lastError")]
      private static string GetDeviceBusReportedDeviceDescription(SafeHandle safeHandle, NativeMethods.SP_DEVINFO_DATA infoData)
      {
         if (!OperatingSystem.IsAtLeast(OperatingSystem.EnumOsName.Windows7))
            return string.Empty;


         const int bufferSize = Filesystem.NativeMethods.DefaultFileBufferSize / 32; // 128

         var descriptionBuffer = new byte[bufferSize];
         ulong propertyType = 0;
         var requiredSize = 0;


         var success = NativeMethods.SetupDiGetDeviceProperty(safeHandle, ref infoData, ref NativeMethods.DEVPROPKEYS.DeviceBusReportedDeviceDesc, ref propertyType, descriptionBuffer, descriptionBuffer.Length, ref requiredSize, 0);

         var lastError = Marshal.GetLastWin32Error();

         if (success)
         {
            var value = Encoding.Unicode.GetString(descriptionBuffer);

            var result = value.Remove(value.IndexOf((char) 0));

            return !Utils.IsNullOrWhiteSpace(result) ? result.Trim() : string.Empty;
         }

         return string.Empty;
      }


      [SecurityCritical]
      private static string GetDeviceRegistryProperty(SafeHandle safeHandle, NativeMethods.SP_DEVINFO_DATA infoData, NativeMethods.SPDRP property)
      {
         var bufferSize = Filesystem.NativeMethods.DefaultFileBufferSize / 8; // 512

         while (true)
            using (var safeBuffer = new SafeGlobalMemoryBufferHandle(bufferSize))
            {
               var success = NativeMethods.SetupDiGetDeviceRegistryProperty(safeHandle, ref infoData, property, IntPtr.Zero, safeBuffer, (uint) safeBuffer.Capacity, IntPtr.Zero);

               var lastError = Marshal.GetLastWin32Error();

               if (success)
               {
                  var value = safeBuffer.PtrToStringUni();

                  return !Utils.IsNullOrWhiteSpace(value) ? value.Trim() : string.Empty;
               }


               // MSDN: SetupDiGetDeviceRegistryProperty returns ERROR_INVALID_DATA error code if
               // the requested property does not exist for a device or if the property data is not valid.

               if (lastError == Win32Errors.ERROR_INVALID_DATA)
                  return string.Empty;


               bufferSize = Utils.GetDoubledBufferSizeOrThrowException(safeBuffer, lastError, bufferSize, property.ToString());
            }
      }


      [SecurityCritical]
      private static void SetDeviceProperties(SafeHandle safeHandle, DeviceInfo deviceInfo, NativeMethods.SP_DEVINFO_DATA infoData)
      {
         SetMinimalDeviceProperties(safeHandle, deviceInfo, infoData);


         deviceInfo.BaseContainerId = new Guid(GetDeviceRegistryProperty(safeHandle, infoData, NativeMethods.SPDRP.BaseContainerId));

         deviceInfo.ClassGuid = new Guid(GetDeviceRegistryProperty(safeHandle, infoData, NativeMethods.SPDRP.ClassGuid));
         
         deviceInfo.CompatibleIds = GetDeviceRegistryProperty(safeHandle, infoData, NativeMethods.SPDRP.CompatibleIds);

         deviceInfo.DeviceClass = GetDeviceRegistryProperty(safeHandle, infoData, NativeMethods.SPDRP.Class);

         deviceInfo.DeviceDriver = GetDeviceRegistryProperty(safeHandle, infoData, NativeMethods.SPDRP.Driver);

         deviceInfo.EnumeratorName = GetDeviceRegistryProperty(safeHandle, infoData, NativeMethods.SPDRP.EnumeratorName);

         deviceInfo.HardwareId = GetDeviceRegistryProperty(safeHandle, infoData, NativeMethods.SPDRP.HardwareId);

         deviceInfo.LocationInformation = GetDeviceRegistryProperty(safeHandle, infoData, NativeMethods.SPDRP.LocationInformation);

         deviceInfo.LocationPaths = GetDeviceRegistryProperty(safeHandle, infoData, NativeMethods.SPDRP.LocationPaths);
         
         deviceInfo.Service = GetDeviceRegistryProperty(safeHandle, infoData, NativeMethods.SPDRP.Service);
      }


      [SecurityCritical]
      private static void SetMinimalDeviceProperties(SafeHandle safeHandle, DeviceInfo deviceInfo, NativeMethods.SP_DEVINFO_DATA infoData)
      {
         deviceInfo.BusReportedDeviceDescription = GetDeviceBusReportedDeviceDescription(safeHandle, infoData);
         
         deviceInfo.DeviceDescription = GetDeviceRegistryProperty(safeHandle, infoData, NativeMethods.SPDRP.DeviceDescription);
         
         deviceInfo.FriendlyName = GetDeviceRegistryProperty(safeHandle, infoData, NativeMethods.SPDRP.FriendlyName);

         deviceInfo.Manufacturer = GetDeviceRegistryProperty(safeHandle, infoData, NativeMethods.SPDRP.Manufacturer);

         deviceInfo.PhysicalDeviceObjectName = GetDeviceRegistryProperty(safeHandle, infoData, NativeMethods.SPDRP.PhysicalDeviceObjectName);
      }
   }
}
