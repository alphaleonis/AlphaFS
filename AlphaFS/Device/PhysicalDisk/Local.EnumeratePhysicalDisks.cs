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
using System.Linq;
using System.Security;
using Alphaleonis.Win32.Filesystem;
using Alphaleonis.Win32.Security;
using Path = Alphaleonis.Win32.Filesystem.Path;

namespace Alphaleonis.Win32.Device
{
   public static partial class Local
   {
      /// <summary>[AlphaFS] Enumerates the physical disks on the Computer, populated with volume-/logical drive information.</summary>
      /// <returns>Returns an <see cref="IEnumerable{PhysicalDiskInfo}"/> collection that represents the physical disks on the Computer.</returns>
      [SecurityCritical]
      public static IEnumerable<PhysicalDiskInfo> EnumeratePhysicalDisks()
      {
         return EnumeratePhysicalDisksCore(ProcessContext.IsElevatedProcess);
      }

      
      /// <summary>[AlphaFS] Enumerates the physical disks on the Computer, populated with volume-/logical drive information.</summary>
      /// <returns>Returns an <see cref="IEnumerable{PhysicalDiskInfo}"/> collection that represents the physical disks on the Computer.</returns>
      /// <param name="isElevated"><c>true</c> indicates the current process is in an elevated state, allowing to retrieve more data.</param>
      /// <param name="deviceNumber">Retrieve a <see cref="PhysicalDiskInfo"/> instance by device number.</param>
      [SecurityCritical]
      private static IEnumerable<PhysicalDiskInfo> EnumeratePhysicalDisksCore(bool isElevated, int deviceNumber = -1)
      {
         var isDeviceNumber = deviceNumber > -1;
         var physicalDisks = new Collection<PhysicalDiskInfo>();
         var volumeGuids = new Collection<PhysicalDiskInfo>();
         var logicalDrives = new Collection<PhysicalDiskInfo>();


         foreach (var deviceInfo in EnumerateDevicesCore(null, DeviceGuid.Disk, false))
         {
            var storageDeviceInfo = GetStorageDeviceInfoCore(false, deviceInfo.DevicePath);

            if (null == storageDeviceInfo || isDeviceNumber && storageDeviceInfo.DeviceNumber != deviceNumber)
               continue;

            physicalDisks.Add(PhysicalDiskInfo.InitializePhysicalDiskInfo(isElevated, null, deviceInfo));

            // There can only be one.
            if (isDeviceNumber)
               break;
         }


         if (physicalDisks.Count == 0)
            yield break;


         // Retrieve volumes belonging to deviceNumber.

         foreach (var volume in Volume.EnumerateVolumes())
         {
            var storageDeviceInfo = GetStorageDeviceInfoCore(false, volume);

            if (null == storageDeviceInfo || isDeviceNumber && deviceNumber != storageDeviceInfo.DeviceNumber)
               continue;

            volumeGuids.Add(PhysicalDiskInfo.InitializePhysicalDiskInfo(isElevated, volume, null));


            // Resolve logical drives.

            var driveName = Volume.GetVolumeDisplayName(volume);

            if (!Utils.IsNullOrWhiteSpace(driveName))
               logicalDrives.Add(PhysicalDiskInfo.InitializePhysicalDiskInfo(isElevated, driveName, null));
         }


         foreach (var pDisk in physicalDisks)

            yield return PopulatePhysicalDisk(pDisk, volumeGuids, logicalDrives);
      }
      

      private static PhysicalDiskInfo PopulatePhysicalDisk(PhysicalDiskInfo pDisk, ICollection<PhysicalDiskInfo> pVolumes, ICollection<PhysicalDiskInfo> pLogicalDrives)
      {
         var pDiskInfo = Utils.CopyFrom(pDisk);


         if (null != pVolumes && pVolumes.Count > 0)

            foreach (var pVolume in pVolumes.Where(pVol => pVol.StorageDeviceInfo.DeviceNumber == pDiskInfo.StorageDeviceInfo.DeviceNumber))
            {
               var driveNumber = pVolume.StorageDeviceInfo.DeviceNumber;

               var partitionNumber = pVolume.StorageDeviceInfo.PartitionNumber;
            

               PopulateVolumeDetails(pDiskInfo, partitionNumber, pVolume.DevicePath);


               // Get logical drive from volume matching DeviceNumber and PartitionNumber.

               if (null != pLogicalDrives && pLogicalDrives.Count > 0)

                  foreach (var pLogicalDrive in pLogicalDrives.Where(pDriveLogical => driveNumber == pDriveLogical.StorageDeviceInfo.DeviceNumber && partitionNumber == pDriveLogical.StorageDeviceInfo.PartitionNumber))

                     PopulateLogicalDriveDetails(pDiskInfo, pLogicalDrive.DevicePath);
            }


         return pDiskInfo;
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


      private static void PopulateLogicalDriveDetails(PhysicalDiskInfo pDiskInfo, string drivePath)
      {
         // Add device logical drive.

         if (null == pDiskInfo.LogicalDrives)
            pDiskInfo.LogicalDrives = new Collection<string>();

         pDiskInfo.LogicalDrives.Add(Path.RemoveTrailingDirectorySeparator(drivePath));
      }
   }
}
