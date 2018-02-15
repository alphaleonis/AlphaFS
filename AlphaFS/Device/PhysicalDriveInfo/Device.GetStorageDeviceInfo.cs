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

using System;

namespace Alphaleonis.Win32.Filesystem
{
   public static partial class Device
   {
      /// <summary>[AlphaFS] Get the type, device- and partition number for the storage device on the Computer that is related to the logical drive name, volume GUID or <see cref="DeviceInfo.DevicePath"/>.</summary>
      /// <returns>A <see cref="StorageDeviceInfo"/> instance that represent the storage device on the Computer that is related to <paramref name="devicePath"/>.</returns>
      ///  <exception cref="ArgumentException"/>
      ///  <exception cref="ArgumentNullException"/>
      ///  <exception cref="NotSupportedException"/>
      ///  <exception cref="Exception"/>
      /// <param name="devicePath">
      ///    A drive path such as: "C", "C:" or "C:\".
      ///    A volume <see cref="Guid"/> such as: "\\?\Volume{xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}\".
      ///    A <see cref="DeviceInfo.DevicePath"/> string (when <see cref="DeviceInfo.ClassGuid"/> is set to <see cref="DeviceGuid.Disk"/>).
      /// </param>
      public static StorageDeviceInfo GetStorageDeviceInfo(string devicePath)
      {
         return GetStorageDeviceInfoCore(devicePath, false);
      }


      /// <summary>[AlphaFS] Get the type, device- and partition number for the storage device on the Computer that is related to the logical drive name, volume GUID or <see cref="DeviceInfo.DevicePath"/>.</summary>
      /// <returns>A <see cref="StorageDeviceInfo"/> instance that represent the storage device on the Computer that is related to <paramref name="devicePath"/>.</returns>
      ///  <exception cref="ArgumentException"/>
      ///  <exception cref="ArgumentNullException"/>
      ///  <exception cref="NotSupportedException"/>
      ///  <exception cref="Exception"/>
      /// <param name="devicePath">
      ///    A drive path such as: "C", "C:" or "C:\".
      ///    A volume <see cref="Guid"/> such as: "\\?\Volume{xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}\".
      ///    A <see cref="DeviceInfo.DevicePath"/> string (when <see cref="DeviceInfo.ClassGuid"/> is set to <see cref="DeviceGuid.Disk"/>).
      /// </param>
      /// <param name="getBusType">When <c>true</c> also get the <see cref="StorageBusType"/> which requires elevated rights.</param>
      public static StorageDeviceInfo GetStorageDeviceInfo(string devicePath, bool getBusType)
      {
         return GetStorageDeviceInfoCore(devicePath, getBusType);
      }


      /// <summary>[AlphaFS] Get the type, device- and partition number for the storage device on the Computer that is related to the logical drive name, volume GUID or <see cref="DeviceInfo.DevicePath"/>.</summary>
      /// <returns>A <see cref="StorageDeviceInfo"/> instance that represent the storage device on the Computer that is related to <paramref name="devicePath"/>.</returns>
      ///  <exception cref="ArgumentException"/>
      ///  <exception cref="ArgumentNullException"/>
      ///  <exception cref="NotSupportedException"/>
      ///  <exception cref="Exception"/>
      /// <param name="devicePath">
      ///    A drive path such as: "C", "C:" or "C:\".
      ///    A volume <see cref="Guid"/> such as: "\\?\Volume{xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}\".
      ///    A <see cref="DeviceInfo.DevicePath"/> string (when <see cref="DeviceInfo.ClassGuid"/> is set to <see cref="DeviceGuid.Disk"/>).
      /// </param>
      /// <param name="getBusType">When <c>true</c> also get the <see cref="StorageBusType"/> which requires elevated rights.</param>
      internal static StorageDeviceInfo GetStorageDeviceInfoCore(string devicePath, bool getBusType)
      {
         string logicalDrive;

         var pathToDevice = GetDevicePath(devicePath, out logicalDrive);

         if (Utils.IsNullOrWhiteSpace(pathToDevice))
            return null;


         // No elevation needed.

         //try { 
            using (var safeHandle = OpenPhysicalDrive(pathToDevice, 0))

            using (var safeBuffer = GetDeviceIoData<NativeMethods.STORAGE_DEVICE_NUMBER>(safeHandle, NativeMethods.IoControlCode.IOCTL_STORAGE_GET_DEVICE_NUMBER, devicePath))
            {
               var storage = null != safeBuffer ? safeBuffer.PtrToStructure<NativeMethods.STORAGE_DEVICE_NUMBER>(0) : (NativeMethods.STORAGE_DEVICE_NUMBER?) null;

               if (null != storage)
               {
                  var storageInfo = new StorageDeviceInfo((NativeMethods.STORAGE_DEVICE_NUMBER) storage);

                  if (getBusType)
                  {
                     // Requires elevation.

                     var pDriveInfo = GetPhysicalDriveInfoCore(storageInfo, pathToDevice, null, false);

                     if (null != pDriveInfo)
                        storageInfo.BusType = pDriveInfo.StorageDeviceInfo.BusType;
                  }

                  return storageInfo;
               }
            }
         //}
         //catch {}

         return null;
      }
   }
}
