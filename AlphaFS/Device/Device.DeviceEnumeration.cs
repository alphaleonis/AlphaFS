/*  Copyright (C) 2008-2017 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
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
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security;

namespace Alphaleonis.Win32.Filesystem
{
   /// <summary>Provides static methods to retrieve device resource information from a local or remote host.</summary>
   public static partial class Device
   {
      /// <summary>[AlphaFS] Enumerates all available devices on the local host.</summary>
      /// <returns><see cref="IEnumerable{DeviceInfo}"/> instances of type <see cref="DeviceGuid"/> from the local host.</returns>
      /// <param name="deviceGuid">One of the <see cref="DeviceGuid"/> devices.</param>
      [SecurityCritical]
      public static IEnumerable<DeviceInfo> EnumerateDevices(DeviceGuid deviceGuid)
      {
         return EnumerateDevicesCore(null, null, deviceGuid);
      }


      /// <summary>[AlphaFS] Enumerates all available devices of type <see cref="DeviceGuid"/> on the local or remote host.</summary>
      /// <returns><see cref="IEnumerable{DeviceInfo}"/> instances of type <see cref="DeviceGuid"/> for the specified <paramref name="hostName"/>.</returns>
      /// <param name="hostName">The name of the local or remote host on which the device resides. <see langword="null"/> refers to the local host.</param>
      /// <param name="deviceGuid">One of the <see cref="DeviceGuid"/> devices.</param>
      [SecurityCritical]
      public static IEnumerable<DeviceInfo> EnumerateDevices(string hostName, DeviceGuid deviceGuid)
      {
         return EnumerateDevicesCore(null, hostName, deviceGuid);
      }
      

      
      
      /// <summary>[AlphaFS] Enumerates all available devices on the local or remote host.</summary>
      [SecurityCritical]
      internal static IEnumerable<DeviceInfo> EnumerateDevicesCore(SafeHandle safeHandle, string hostName, DeviceGuid deviceInterfaceGuid, bool getAllProperties = true)
      {
         SafeCmConnectMachineHandle safeMachineHandle;
         var callerHandle = safeHandle != null;
         var deviceGuid = new Guid(Utils.GetEnumDescription(deviceInterfaceGuid));


         // CM_Connect_Machine()
         // MSDN Note: Beginning in Windows 8 and Windows Server 2012 functionality to access remote machines has been removed.
         // You cannot access remote machines when running on these versions of Windows. 
         // http://msdn.microsoft.com/en-us/library/windows/hardware/ff537948%28v=vs.85%29.aspx

         var lastError = NativeMethods.CM_Connect_Machine(Host.GetUncName(hostName), out safeMachineHandle);

         NativeMethods.IsValidHandle(safeMachineHandle, lastError);


         using (safeMachineHandle)
         {
            // Start at the "Root" of the device tree of the specified machine.
            if (!callerHandle)
               safeHandle = NativeMethods.SetupDiGetClassDevsEx(ref deviceGuid, IntPtr.Zero, IntPtr.Zero, NativeMethods.SetupDiGetClassDevsExFlags.Present | NativeMethods.SetupDiGetClassDevsExFlags.DeviceInterface, IntPtr.Zero, hostName, IntPtr.Zero);

            NativeMethods.IsValidHandle(safeHandle, Marshal.GetLastWin32Error());
            

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
                  var success = NativeMethods.SetupDiGetDeviceInterfaceDetail(safeHandle, ref deviceInterfaceData, ref deviceInterfaceDetailData, NativeMethods.DefaultFileBufferSize, IntPtr.Zero, ref deviceInfoData);

                  lastError = Marshal.GetLastWin32Error();
                  if (!success)
                     NativeError.ThrowException(lastError, hostName);


                  // Create DeviceInfo instance.
                  // Set DevicePath property of DeviceInfo instance.
                  var deviceInfo = new DeviceInfo(hostName) {DevicePath = deviceInterfaceDetailData.DevicePath};


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

                     lastError = NativeMethods.CM_Get_Device_ID_Ex(deviceInfoData.DevInst, safeBuffer, (uint) safeBuffer.Capacity, 0, safeMachineHandle);

                     if (lastError != Win32Errors.CR_SUCCESS)
                        NativeError.ThrowException(lastError, hostName);

                     // Add to instance.
                     deviceInfo.InstanceId = safeBuffer.PtrToStringUni();
                  }


                  if (getAllProperties)
                     SetDeviceProperties(safeHandle, deviceInfo, deviceInfoData);

                  else if (deviceInterfaceGuid == DeviceGuid.Disk)
                     SetPhysicalDiskProperties(safeHandle, deviceInfo, deviceInfoData);
                     

                  
                  yield return deviceInfo;


                  // Get new structure instance.
                  deviceInterfaceData = CreateDeviceInterfaceDataInstance();
               }
            }
            finally
            {
               // Handle is ours, dispose.
               if (!callerHandle && null != safeHandle)
                  safeHandle.Close();
            }
         }
      }
   }
}
