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
using System.Globalization;
using System.Security;
using System.Security.AccessControl;
using Alphaleonis.Win32.Filesystem;

namespace Alphaleonis.Win32.Device
{
   public static partial class Local
   {
      // /// <remarks>When this method is called from a non-elevated state, the <see cref="StorageDeviceInfo.TotalSize"/> property always returns <c>0</c>.</remarks>


      /// <returns>Returns a <see cref="StorageDeviceInfo"/> instance that represent the storage device that is related to <paramref name="devicePath"/>.</returns>
      /// <exception cref="Exception"/>
      [SecurityCritical]
      internal static StorageDeviceInfo GetStorageDeviceInfo(bool isElevated, bool isDevice, int deviceNumber, string devicePath, out string localDevicePath)
      {
         localDevicePath = devicePath;
         var getByDeviceNumber = deviceNumber > -1;
         var retry = false;

      Retry:

         // Accessing a volume like: "\\.\D" on a dynamic disk fails with ERROR_INVALID_FUNCTION.
         // On retry, the drive is accessed using the: "\\.\PhysicalDriveX" path format which is the device, not the volume/logical drive.

         using (var safeFileHandle = OpenDevice(localDevicePath, isElevated ? FileSystemRights.Read : NativeMethods.FILE_ANY_ACCESS))
         {
            int lastError;

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


               // A logical drive path like \\.\D: fails on a dynamic disk.

               if (!retry && !isDevice && lastError == Win32Errors.ERROR_INVALID_FUNCTION)
               {
                  var volDiskExtents = GetVolumeDiskExtents(safeFileHandle, localDevicePath);

                  if (volDiskExtents.HasValue)
                     foreach (var extent in volDiskExtents.Value.Extents)
                     {
                        var newDeviceNumber = (int) extent.DiskNumber;

                        if (getByDeviceNumber && deviceNumber != newDeviceNumber)
                           continue;

                        localDevicePath = Path.PhysicalDrivePrefix + newDeviceNumber.ToString(CultureInfo.InvariantCulture);

                        isDevice = true;
                        retry = true;

                        goto Retry;
                     }
               }
            }

            return null;
         }
      }
   }
}
