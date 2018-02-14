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

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security;

namespace Alphaleonis.Win32.Filesystem
{
   public static partial class Device
   {
      /// <summary>[AlphaFS] Enumerates the physical drives on the Computer, populated with volume- and logical drive information.</summary>
      /// <returns>An <see cref="IEnumerable{PhysicalDriveInfo}"/> collection that represents the physical drives on the Computer.</returns>      
      [SecurityCritical]
      public static IEnumerable<PhysicalDriveInfo> EnumeratePhysicalDrives()
      {
         var physicalDrives = EnumerateDevicesCore(null, DeviceGuid.Disk, false).Select(deviceInfo => GetPhysicalDriveInfoCore(null, null, deviceInfo, true, true)).Where(physicalDrive => null != physicalDrive).ToArray();

         var pVolumes = Volume.EnumerateVolumes().Select(volumeGuid => GetPhysicalDriveInfoCore(null, volumeGuid, null, false, false)).Where(physicalDrive => null != physicalDrive).ToArray();

         var pLogicalDrives = DriveInfo.EnumerateLogicalDrivesCore(false, false).Select(driveName => GetPhysicalDriveInfoCore(null, driveName, null, false, false)).Where(physicalDrive => null != physicalDrive).ToArray();


         foreach (var pDrive in physicalDrives)
         {
            var pDriveInfo = new PhysicalDriveInfo(pDrive)
            {
               StorageDeviceInfo = {PartitionNumber = pDrive.StorageDeviceInfo.PartitionNumber}
            };

            
            // Get volume from physical drive matching DeviceNumber.

            foreach (var pDriveVolume in pVolumes.Where(pDriveVolume => pDriveVolume.StorageDeviceInfo.DeviceNumber == pDrive.StorageDeviceInfo.DeviceNumber))
            {
               if (null == pDriveInfo.PartitionNumbers)
                  pDriveInfo.PartitionNumbers = new Collection<int>();

               pDriveInfo.PartitionNumbers.Add(pDriveVolume.StorageDeviceInfo.PartitionNumber);


               if (null == pDriveInfo.VolumeGuids)
                  pDriveInfo.VolumeGuids = new Collection<string>();

               pDriveInfo.VolumeGuids.Add(pDriveVolume.DevicePath);


               // Get logical drive from volume matching DeviceNumber and PartitionNumber.

               foreach (var pDriveLogical in pLogicalDrives.Where(pDriveLogical => pDriveLogical.StorageDeviceInfo.DeviceNumber == pDriveVolume.StorageDeviceInfo.DeviceNumber && pDriveLogical.StorageDeviceInfo.PartitionNumber == pDriveVolume.StorageDeviceInfo.PartitionNumber))
               {
                  if (null == pDriveInfo.LogicalDrives)
                     pDriveInfo.LogicalDrives = new Collection<string>();

                  pDriveInfo.LogicalDrives.Add(Path.RemoveTrailingDirectorySeparator(pDriveLogical.DevicePath));
               }
            }


            yield return pDriveInfo;
         }
      }
   }
}
