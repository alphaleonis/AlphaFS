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
using System.Globalization;
using System.Security;
using System.Security.AccessControl;
using Alphaleonis.Win32.Filesystem;
using Microsoft.Win32.SafeHandles;

namespace Alphaleonis.Win32.Device
{
   public static partial class Local
   {
      /// <returns>Returns a <see cref="StorageDeviceInfo"/> instance that represent the storage device that is related to <paramref name="devicePath"/>.</returns>
      /// <exception cref="Exception"/>
      [SecurityCritical]
      internal static StorageDeviceInfo GetStorageDeviceInfo(bool isElevated, bool isDevice, int deviceNumber, string devicePath, out string localDevicePath)
      {
         localDevicePath = FilesystemHelper.GetLocalDevicePath(devicePath);
         var getByDeviceNumber = deviceNumber > -1;
         var retry = false;

      Retry:

         int lastError;

         // On retry, the drive is accessed using "\\.\PhysicalDriveX" path format which is the device, not the volume/logical drive.

         using (var safeFileHandle = OpenDevice(localDevicePath, isElevated ? FileSystemRights.Read : NativeMethods.FILE_ANY_ACCESS))

         using (var safeBuffer = GetDeviceIoData<NativeMethods.STORAGE_DEVICE_NUMBER>(safeFileHandle, NativeMethods.IoControlCode.IOCTL_STORAGE_GET_DEVICE_NUMBER, localDevicePath, out lastError))
         {
            if (null != safeBuffer)
            {
               var storageDeviceInfo = new StorageDeviceInfo(safeBuffer.PtrToStructure<NativeMethods.STORAGE_DEVICE_NUMBER>());

               if (getByDeviceNumber && deviceNumber != storageDeviceInfo.DeviceNumber)
                  return null;

               SetStorageDeviceInfoData(isElevated, safeFileHandle, localDevicePath, storageDeviceInfo);


               if (!localDevicePath.StartsWith(Path.PhysicalDrivePrefix, StringComparison.OrdinalIgnoreCase))
                  localDevicePath = null;

               return storageDeviceInfo;
            }


            // A logical drive path on a dynamic disk like "\\.\D:" fails.

            if (!retry && !isDevice && lastError == Win32Errors.ERROR_INVALID_FUNCTION)
               foreach (var physicalDeviceNumber in GetDeviceNumbersForVolume(safeFileHandle, localDevicePath))
               {
                  if (getByDeviceNumber && deviceNumber != physicalDeviceNumber)
                     continue;

                  // StorageDeviceInfo.PartitionNumber = 0 when opening the device as "\\.\PhysicalDriveX".

                  localDevicePath = Path.PhysicalDrivePrefix + physicalDeviceNumber.ToString(CultureInfo.InvariantCulture);

                  isDevice = true;
                  retry = true;

                  goto Retry;
               }
         }

         return null;
      }


      /// <returns>Returns an <see cref="IEnumerable{Int}"/> of physical drive device numbers used by the specified <paramref name="safeFileHandle"/> volume.</returns>
      [SecurityCritical]
      private static IEnumerable<int> GetDeviceNumbersForVolume(SafeFileHandle safeFileHandle, string pathForException)
      {
         var volDiskExtents = GetVolumeDiskExtents(safeFileHandle, pathForException);

         if (volDiskExtents.HasValue)

            foreach (var extent in volDiskExtents.Value.Extents)

               yield return (int) extent.DiskNumber;
      }
   }
}
