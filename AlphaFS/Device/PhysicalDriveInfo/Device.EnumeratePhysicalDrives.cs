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
         var physicalDrives = EnumerateDevicesCore(null, DeviceGuid.Disk, false).Select(deviceInfo => GetPhysicalDriveInfoCore(null, null, deviceInfo, true)).Where(physicalDrive => null != physicalDrive).ToArray();
         
         var pVolumes = Volume.EnumerateVolumes().Select(volumeGuid => GetPhysicalDriveInfoCore(null, volumeGuid, null, null)).Where(physicalDrive => null != physicalDrive).ToArray();

         var pLogicalDrives = DriveInfo.EnumerateLogicalDrivesCore(false, false).Select(driveName => GetPhysicalDriveInfoCore(null, driveName, null, null)).Where(physicalDrive => null != physicalDrive).ToArray();


         foreach (var pDrive in physicalDrives)
         {
            var pDriveInfo = new PhysicalDriveInfo(pDrive)
            {
               StorageDeviceInfo =
               {
                  DeviceNumber = pDrive.StorageDeviceInfo.DeviceNumber,
                  PartitionNumber = pDrive.StorageDeviceInfo.PartitionNumber
               }
            };

            
            // Get volume from physical drive matching DeviceNumber.

            foreach (var pVolume in pVolumes.Where(pVol => pVol.StorageDeviceInfo.DeviceNumber == pDriveInfo.StorageDeviceInfo.DeviceNumber))
            {
               // Add device volume labels.

               if (null == pDriveInfo.VolumeLabels)
                  pDriveInfo.VolumeLabels = new Collection<string>();

               pDriveInfo.VolumeLabels.Add(pVolume.Name);


               // Add device partition index numbers.

               if (null == pDriveInfo.PartitionIndexes)
                  pDriveInfo.PartitionIndexes = new Collection<int>();

               pDriveInfo.PartitionIndexes.Add(pVolume.StorageDeviceInfo.PartitionNumber);


               // Add device volume GUIDs.

               if (null == pDriveInfo.VolumeGuids)
                  pDriveInfo.VolumeGuids = new Collection<string>();

               pDriveInfo.VolumeGuids.Add(pVolume.DevicePath);


               
               
               // Get logical drive from volume matching DeviceNumber and PartitionNumber.

               foreach (var pLogicalDrive in pLogicalDrives.Where(pDriveLogical => pDriveLogical.StorageDeviceInfo.DeviceNumber == pVolume.StorageDeviceInfo.DeviceNumber && pDriveLogical.StorageDeviceInfo.PartitionNumber == pVolume.StorageDeviceInfo.PartitionNumber))
               {
                  if (null == pDriveInfo.LogicalDrives)
                     pDriveInfo.LogicalDrives = new Collection<string>();

                  pDriveInfo.LogicalDrives.Add(Path.RemoveTrailingDirectorySeparator(pLogicalDrive.DevicePath));
               }
            }


            yield return pDriveInfo;
         }


         // Windows Disk Management shows CD-ROM so mimic that behaviour.
         var cdRoms = EnumerateDevicesCore(null, DeviceGuid.CDRom, false).Select(deviceInfo => GetPhysicalDriveInfoCore(null, null, deviceInfo, true)).Where(physicalDrive => null != physicalDrive).ToArray();

         foreach (var pCdRom in cdRoms)
         {
            var pDriveInfo = new PhysicalDriveInfo(pCdRom)
            {
               StorageDeviceInfo =
               {
                  DeviceNumber = pCdRom.StorageDeviceInfo.DeviceNumber,
                  PartitionNumber = pCdRom.StorageDeviceInfo.PartitionNumber
               }
            };


            // Get volume from CDRom matching DeviceNumber.

            var pVolume = pVolumes.FirstOrDefault(pVol => pVol.StorageDeviceInfo.DeviceNumber == pDriveInfo.StorageDeviceInfo.DeviceNumber && pVol.StorageDeviceInfo.PartitionNumber == pDriveInfo.StorageDeviceInfo.PartitionNumber);

            if (null != pVolume)
            {
               // Add device volume labels.

               if (null == pDriveInfo.VolumeLabels)
                  pDriveInfo.VolumeLabels = new Collection<string>();

               pDriveInfo.VolumeLabels.Add(pVolume.Name);


               // Add device partition index numbers.

               if (null == pDriveInfo.PartitionIndexes)
                  pDriveInfo.PartitionIndexes = new Collection<int>();

               pDriveInfo.PartitionIndexes.Add(pVolume.StorageDeviceInfo.PartitionNumber);


               // Add device volume GUIDs.

               if (null == pDriveInfo.VolumeGuids)
                  pDriveInfo.VolumeGuids = new Collection<string>();

               pDriveInfo.VolumeGuids.Add(pVolume.DevicePath);




               // Get logical drive from CDRom matching DeviceNumber and PartitionNumber.

               var logicalDrive = pLogicalDrives.FirstOrDefault(pDriveLogical => pDriveLogical.StorageDeviceInfo.DeviceNumber == pVolume.StorageDeviceInfo.DeviceNumber && pDriveLogical.StorageDeviceInfo.PartitionNumber == pVolume.StorageDeviceInfo.PartitionNumber);

               if (null != logicalDrive)
               {
                  if (null == pDriveInfo.LogicalDrives)
                     pDriveInfo.LogicalDrives = new Collection<string>();

                  pDriveInfo.LogicalDrives.Add(Path.RemoveTrailingDirectorySeparator(logicalDrive.DevicePath));
               }
            }
            

            yield return pDriveInfo;
         }
      }
   }
}
