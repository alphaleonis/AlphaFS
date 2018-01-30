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
   public static partial class Device
   {
      /// <summary>[AlphaFS] Enumerates all available devices on the local host.</summary>
      /// <returns><see cref="IEnumerable{DeviceInfo}"/> instances of type <see cref="DeviceGuid"/> from the local host.</returns>
      /// <param name="deviceGuid">One of the <see cref="DeviceGuid"/> devices.</param>
      [SecurityCritical]
      public static IEnumerable<DeviceInfo> EnumerateDevices(DeviceGuid deviceGuid)
      {
         return EnumerateDevicesCore(null, deviceGuid);
      }


      /// <summary>[AlphaFS] Enumerates all available devices of type <see cref="DeviceGuid"/> on the local or remote host.</summary>
      /// <returns><see cref="IEnumerable{DeviceInfo}"/> instances of type <see cref="DeviceGuid"/> for the specified <paramref name="hostName"/>.</returns>
      /// <param name="hostName">The name of the local or remote host on which the device resides. <see langword="null"/> refers to the local host.</param>
      /// <param name="deviceGuid">One of the <see cref="DeviceGuid"/> devices.</param>
      [SecurityCritical]
      public static IEnumerable<DeviceInfo> EnumerateDevices(string hostName, DeviceGuid deviceGuid)
      {
         return EnumerateDevicesCore(hostName, deviceGuid);
      }


      
      
      /// <summary>[AlphaFS] Enumerates all available devices on the local or remote host.</summary>
      [SecurityCritical]
      internal static IEnumerable<DeviceInfo> EnumerateDevicesCore(string hostName, DeviceGuid interfaceGuid, bool getAllProperties = true)
      {
         // CM_Connect_Machine()
         // MSDN Note: Beginning in Windows 8 and Windows Server 2012 functionality to access remote machines has been removed.
         // You cannot access remote machines when running on these versions of Windows. 
         // http://msdn.microsoft.com/en-us/library/windows/hardware/ff537948%28v=vs.85%29.aspx


         SafeCmConnectMachineHandle safeMachineHandle;

         var lastError = NativeMethods.CM_Connect_Machine(Host.GetUncName(hostName), out safeMachineHandle);

         NativeMethods.IsValidHandle(safeMachineHandle, lastError);

         
         var deviceGuid = new Guid(Utils.GetEnumDescription(interfaceGuid));

         
         // Start at the "Root" of the device tree of the specified machine.

         using (safeMachineHandle)
         using (var safeHandle = NativeMethods.SetupDiGetClassDevsEx(ref deviceGuid, IntPtr.Zero, IntPtr.Zero, NativeMethods.SetupDiGetClassDevsExFlags.Present | NativeMethods.SetupDiGetClassDevsExFlags.DeviceInterface, IntPtr.Zero, hostName, IntPtr.Zero))
         {
            NativeMethods.IsValidHandle(safeHandle, Marshal.GetLastWin32Error());
            
            uint memberInterfaceIndex = 0;
            var interfaceStructSize = (uint) Marshal.SizeOf(typeof(NativeMethods.SP_DEVICE_INTERFACE_DATA));
            var dataStructSize = (uint) Marshal.SizeOf(typeof(NativeMethods.SP_DEVINFO_DATA));


            // Start enumerating device interfaces.

            do
            {
               var interfaceData = new NativeMethods.SP_DEVICE_INTERFACE_DATA {cbSize = interfaceStructSize};

               var success = NativeMethods.SetupDiEnumDeviceInterfaces(safeHandle, IntPtr.Zero, ref deviceGuid, memberInterfaceIndex++, ref interfaceData);

               lastError = Marshal.GetLastWin32Error();
               if (!success)
               {
                  if (lastError != Win32Errors.NO_ERROR && lastError != Win32Errors.ERROR_NO_MORE_ITEMS)
                     NativeError.ThrowException(lastError, hostName);

                  break;
               }


               // Create DeviceInfo instance.

               var diData = new NativeMethods.SP_DEVINFO_DATA {cbSize = dataStructSize};

               var deviceInfo = new DeviceInfo(hostName)
               {
                  ClassGuid = deviceGuid,
                  DevicePath = GetInterfaceDetails(safeHandle, ref interfaceData, ref diData).DevicePath
               };


               if (getAllProperties)
               {
                  SetDeviceProperties(safeHandle, deviceInfo, diData);

                  deviceInfo.InstanceID = GetDeviceInstanceID(safeMachineHandle, hostName, diData);
               }

               else
                  SetMinimalDeviceProperties(safeHandle, deviceInfo, diData);


               yield return deviceInfo;

            } while (true);
         }
      }


      [SecurityCritical]
      private static string GetDeviceInstanceID(SafeCmConnectMachineHandle safeMachineHandle, string hostName, NativeMethods.SP_DEVINFO_DATA diData)
      {
         // CM_Get_Parent_Ex()
         // Note: Using this function to access remote machines is not supported
         // beginning with Windows 8 and Windows Server 2012, as this functionality has been removed.
         // http://msdn.microsoft.com/en-us/library/windows/hardware/ff538615%28v=vs.85%29.aspx


         uint ptrPrevious;

         var lastError = NativeMethods.CM_Get_Parent_Ex(out ptrPrevious, diData.DevInst, 0, safeMachineHandle);

         if (lastError != Win32Errors.CR_SUCCESS)
            NativeError.ThrowException(lastError, hostName);


         // Now we get the InstanceID of the USB level device.
         using (var safeBuffer = new SafeGlobalMemoryBufferHandle(NativeMethods.DefaultFileBufferSize / 4))
         {
            // CM_Get_Device_ID_Ex()
            // Note: Using this function to access remote machines is not supported beginning with Windows 8 and Windows Server 2012,
            // as this functionality has been removed.
            // http://msdn.microsoft.com/en-us/library/windows/hardware/ff538411%28v=vs.85%29.aspx

            lastError = NativeMethods.CM_Get_Device_ID_Ex(diData.DevInst, safeBuffer, (uint) safeBuffer.Capacity, 0, safeMachineHandle);

            if (lastError != Win32Errors.CR_SUCCESS)
               NativeError.ThrowException(lastError, hostName);


            // Device InstanceID.

            return safeBuffer.PtrToStringUni();
         }
      }
   }
}
