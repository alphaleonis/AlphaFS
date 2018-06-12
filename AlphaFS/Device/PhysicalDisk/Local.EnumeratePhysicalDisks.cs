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
using Alphaleonis.Win32.Filesystem;
using Alphaleonis.Win32.Security;
using DriveInfo = Alphaleonis.Win32.Filesystem.DriveInfo;
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
         var driveTypes = new[] {DriveType.CDRom, DriveType.Fixed, DriveType.Removable};
         var physicalDisks = new PhysicalDiskInfo[1];
         var volumeGuids = new Collection<PhysicalDiskInfo>();
         var logicalDrives = new Collection<PhysicalDiskInfo>();
         
         
         // Devices / Physical disk.

         foreach (var deviceInfo in EnumerateDevicesCore(null, DeviceGuid.Disk, false))
         {
            var storageDeviceInfo = GetStorageDeviceInfoCore(false, deviceInfo.DevicePath);

            if (null == storageDeviceInfo || isDeviceNumber && storageDeviceInfo.DeviceNumber != deviceNumber)
               continue;
            
            physicalDisks[0] = PhysicalDiskInfo.InitializePhysicalDiskInfo(isElevated, null, deviceInfo);

            // There can only be one.
            break;
         }


         if (null == physicalDisks[0])
            yield break;


         // Volumes.

         foreach (var volumeGuid in Volume.EnumerateVolumes())
         {
            var storageDeviceInfo = GetStorageDeviceInfoCore(false, volumeGuid);

            if (null == storageDeviceInfo || isDeviceNumber && storageDeviceInfo.DeviceNumber != deviceNumber)
               continue;

            volumeGuids.Add(PhysicalDiskInfo.InitializePhysicalDiskInfo(isElevated, volumeGuid, null));
         }


         // Logical drives.

         foreach (var driveInfo in DriveInfo.GetDrives())
         {
            if (!driveTypes.Contains(driveInfo.DriveType))
               continue;

            var storageDeviceInfo = GetStorageDeviceInfoCore(false, driveInfo.Name);

            if (null == storageDeviceInfo || storageDeviceInfo.DeviceNumber != deviceNumber)
               continue;

            logicalDrives.Add(PhysicalDiskInfo.InitializePhysicalDiskInfo(isElevated, driveInfo.Name, null));
         }


         foreach (var pDisk in physicalDisks)

            yield return PopulatePhysicalDisk(pDisk, volumeGuids, logicalDrives);
      }
      

      private static PhysicalDiskInfo PopulatePhysicalDisk(PhysicalDiskInfo pDisk, IEnumerable<PhysicalDiskInfo> pVolumes, IEnumerable<PhysicalDiskInfo> pLogicalDrives)
      {
         var pDiskInfo = Utils.CopyFrom(pDisk);


         foreach (var pVolume in pVolumes.Where(pVol => pVol.StorageDeviceInfo.DeviceNumber == pDiskInfo.StorageDeviceInfo.DeviceNumber))
         {
            var driveNumber = pVolume.StorageDeviceInfo.DeviceNumber;

            var partitionNumber = pVolume.StorageDeviceInfo.PartitionNumber;


            PopulateVolumeDetails(pDiskInfo, partitionNumber, pVolume.DevicePath);


            // Get logical drive from volume matching DeviceNumber and PartitionNumber.

            foreach (var pLogicalDrive in pLogicalDrives.Where(pDriveLogical => pDriveLogical.StorageDeviceInfo.DeviceNumber == driveNumber && pDriveLogical.StorageDeviceInfo.PartitionNumber == partitionNumber))

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
