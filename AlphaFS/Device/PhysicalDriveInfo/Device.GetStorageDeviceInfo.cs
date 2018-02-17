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
using System.Globalization;

namespace Alphaleonis.Win32.Filesystem
{
   public static partial class Device
   {
      /// <summary>[AlphaFS] Retrieves the type, device- and partition number for the storage device on the Computer that is related to the logical drive name, volume GUID or <see cref="DeviceInfo.DevicePath"/>.</summary>
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
         return GetStorageDeviceInfoCore(Security.ProcessContext.IsElevatedProcess, devicePath);
      }


      /// <summary>[AlphaFS] Retrieves the type, device- and partition number for the storage device on the Computer that is related to the logical drive name, volume GUID or <see cref="DeviceInfo.DevicePath"/>.</summary>
      /// <returns>A <see cref="StorageDeviceInfo"/> instance that represent the storage device on the Computer that is related to <paramref name="devicePath"/>.</returns>
      ///  <exception cref="ArgumentException"/>
      ///  <exception cref="ArgumentNullException"/>
      ///  <exception cref="NotSupportedException"/>
      ///  <exception cref="Exception"/>
      /// <param name="isElevated"><see langword="true"/> indicates the current process is in an elevated state, allowing to retrieve more data.</param>
      /// <param name="devicePath">
      ///    A drive path such as: "C", "C:" or "C:\".
      ///    A volume <see cref="Guid"/> such as: "\\?\Volume{xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}\".
      ///    A <see cref="DeviceInfo.DevicePath"/> string (when <see cref="DeviceInfo.ClassGuid"/> is set to <see cref="DeviceGuid.Disk"/>).
      /// </param>
      internal static StorageDeviceInfo GetStorageDeviceInfoCore(bool isElevated, string devicePath)
      {
         string logicalDrive;

         var pathToDevice = GetDevicePath(devicePath, out logicalDrive);

         if (Utils.IsNullOrWhiteSpace(pathToDevice))
            return null;


         StorageDeviceInfo storageDeviceInfo = null;


      StartGetData:

         // No elevation needed.

         using (var safeHandle = OpenPhysicalDrive(pathToDevice, 0))
         {
            var safeBuffer = GetDeviceIoData<NativeMethods.STORAGE_DEVICE_NUMBER>(safeHandle, NativeMethods.IoControlCode.IOCTL_STORAGE_GET_DEVICE_NUMBER, devicePath);
            
            if (null == safeBuffer)
            {
               // Assumption through observation: devicePath is a logical drive that points to a Dynamic disk.


               var volDiskExtents = GetVolumeDiskExtents(safeHandle);

               if (volDiskExtents.HasValue)
               {
                  // Use the first disk extent.

                  pathToDevice = string.Format(CultureInfo.InvariantCulture, "{0}{1}", Path.PhysicalDrivePrefix, volDiskExtents.Value.Extents[0].DiskNumber.ToString(CultureInfo.InvariantCulture));
                  
                  goto StartGetData;
               }
            }

            else
            {
               using (safeBuffer)
                  storageDeviceInfo = new StorageDeviceInfo(safeBuffer.PtrToStructure<NativeMethods.STORAGE_DEVICE_NUMBER>(0));
            }
         }


         if (isElevated)
         {
            var pDriveInfo = GetPhysicalDriveInfoCore(true, storageDeviceInfo, pathToDevice, null);

            if (null != pDriveInfo)
               storageDeviceInfo = pDriveInfo.StorageDeviceInfo;
         }


         return storageDeviceInfo;
      }
   }
}
