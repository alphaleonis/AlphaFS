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

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Security;

namespace Alphaleonis.Win32.Filesystem
{
   public static partial class Device
   {
      /// <summary>[AlphaFS] Enumerates the physical disks on the Computer, populated with volume- and logical drive information.</summary>
      /// <returns>An <see cref="IEnumerable{PhysicalDiskInfo}"/> collection that represents the physical disks on the Computer.</returns>      
      [SecurityCritical]
      public static IEnumerable<PhysicalDiskInfo> EnumeratePhysicalDisks()
      {
         return EnumeratePhysicalDisksCore(Security.ProcessContext.IsElevatedProcess);
      }


      
      
      /// <summary>[AlphaFS] Enumerates the physical disks on the Computer, populated with volume- and logical drive information.</summary>
      /// <returns>An <see cref="IEnumerable{PhysicalDiskInfo}"/> collection that represents the physical disks on the Computer.</returns>      
      [SecurityCritical]
      internal static IEnumerable<PhysicalDiskInfo> EnumeratePhysicalDisksCore(bool isElevated)
      {
         var physicalDisks = EnumerateDevicesCore(null, DeviceGuid.Disk, false).Select(deviceInfo => GetPhysicalDiskInfoCore(isElevated, null, null, deviceInfo)).Where(physicalDisk => null != physicalDisk).ToArray();

         var pVolumeGuids = Volume.EnumerateVolumes().Select(volumeGuid => GetPhysicalDiskInfoCore(false, volumeGuid, null, null)).Where(physicalDisk => null != physicalDisk).ToArray();

         var pLogicalDrives = DriveInfo.EnumerateLogicalDrivesCore(false, false)
            
            .Select(driveName => new DriveInfo(driveName)).Where(driveInfo => driveInfo.DriveType == DriveType.CDRom || driveInfo.DriveType == DriveType.Fixed || driveInfo.DriveType == DriveType.Removable)

            .Select(driveInfo => GetPhysicalDiskInfoCore(false, driveInfo.Name, null, null)).Where(physicalDisk => null != physicalDisk).ToArray();


         foreach (var pDisk in physicalDisks)

            yield return PopulatePhysicalDisk(pDisk, pVolumeGuids, pLogicalDrives);


         // Windows Disk Management shows CD-ROM so mimic that behaviour.

         var cdRoms = EnumerateDevicesCore(null, DeviceGuid.CDRom, false).Select(deviceInfo => GetPhysicalDiskInfoCore(isElevated, null, null, deviceInfo)).Where(physicalDisk => null != physicalDisk).ToArray();

         foreach (var pCdRom in cdRoms)

            yield return PopulatePhysicalCDRom(pCdRom, pVolumeGuids, pLogicalDrives);
      }


      private static void PopulateLogicalDriveDetails(PhysicalDiskInfo pDiskInfo, string drivePath)
      {
         // Add device logical drive.

         if (null == pDiskInfo.LogicalDrives)
            pDiskInfo.LogicalDrives = new Collection<string>();

         pDiskInfo.LogicalDrives.Add(Path.RemoveTrailingDirectorySeparator(drivePath));
      }


      private static void PopulateVolumeDetails(PhysicalDiskInfo pDiskInfo, int partitionNumber, string volumeGuid)
      {
         //// Add device volume labels.

         //if (null == pDiskInfo.VolumeLabels)
         //   pDiskInfo.VolumeLabels = new Collection<string>();

         //pDiskInfo.VolumeLabels.Add(pVolume.Name);


         // Add device partition index numbers.

         if (null == pDiskInfo.PartitionIndexes)
            pDiskInfo.PartitionIndexes = new Collection<int>();

         pDiskInfo.PartitionIndexes.Add(partitionNumber);


         // Add device volume GUIDs.

         if (null == pDiskInfo.VolumeGuids)
            pDiskInfo.VolumeGuids = new Collection<string>();

         pDiskInfo.VolumeGuids.Add(volumeGuid);
      }


      private static PhysicalDiskInfo PopulatePhysicalCDRom(PhysicalDiskInfo pCdRom, PhysicalDiskInfo[] pVolumes, PhysicalDiskInfo[] pLogicalDrives)
      {
         var pDiskInfo = new PhysicalDiskInfo(pCdRom) {StorageDeviceInfo = pCdRom.StorageDeviceInfo};


         // Get volume from CDRom matching DeviceNumber.

         var pVolume = pVolumes.SingleOrDefault(pVol => pVol.StorageDeviceInfo.DeviceNumber == pDiskInfo.StorageDeviceInfo.DeviceNumber && pVol.StorageDeviceInfo.PartitionNumber == pDiskInfo.StorageDeviceInfo.PartitionNumber);

         if (null != pVolume)
         {
            PopulateVolumeDetails(pDiskInfo, pVolume.StorageDeviceInfo.PartitionNumber, pVolume.DevicePath);


            // Get logical drive from CDRom matching DeviceNumber and PartitionNumber.

            var pLogicalDrive = pLogicalDrives.SingleOrDefault(pDriveLogical => pDriveLogical.StorageDeviceInfo.DeviceNumber == pVolume.StorageDeviceInfo.DeviceNumber && pDriveLogical.StorageDeviceInfo.PartitionNumber == pVolume.StorageDeviceInfo.PartitionNumber);

            if (null != pLogicalDrive)
               PopulateLogicalDriveDetails(pDiskInfo, pLogicalDrive.DevicePath);
         }


         return pDiskInfo;
      }


      private static PhysicalDiskInfo PopulatePhysicalDisk(PhysicalDiskInfo pDisk, PhysicalDiskInfo[] pVolumes, PhysicalDiskInfo[] pLogicalDrives)
      {
         var pDiskInfo = new PhysicalDiskInfo(pDisk) {StorageDeviceInfo = pDisk.StorageDeviceInfo};


         foreach (var pVolume in pVolumes.Where(pVol => pVol.StorageDeviceInfo.DeviceNumber == pDiskInfo.StorageDeviceInfo.DeviceNumber))
         {
            var volumeDriveNumber = pVolume.StorageDeviceInfo.DeviceNumber;
            var volumePartitionNumber = pVolume.StorageDeviceInfo.PartitionNumber;


            PopulateVolumeDetails(pDiskInfo, pVolume.StorageDeviceInfo.PartitionNumber, pVolume.DevicePath);


            // Get logical drive from volume matching DeviceNumber and PartitionNumber.

            foreach (var pLogicalDrive in pLogicalDrives.Where(pDriveLogical => pDriveLogical.StorageDeviceInfo.DeviceNumber == volumeDriveNumber && pDriveLogical.StorageDeviceInfo.PartitionNumber == volumePartitionNumber))

               PopulateLogicalDriveDetails(pDiskInfo, pLogicalDrive.DevicePath);
         }


         return pDiskInfo;
      }
   }
}
