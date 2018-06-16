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
using System.Security;
using Alphaleonis.Win32.Filesystem;
using Alphaleonis.Win32.Security;
using Path = Alphaleonis.Win32.Filesystem.Path;

namespace Alphaleonis.Win32.Device
{
   public static partial class Local
   {
      /// <summary>[AlphaFS] Enumerates the physical disks (including CD/DVD devices) on the Computer, populated with volume-/logical drive information.</summary>
      /// <returns>Returns an <see cref="IEnumerable{PhysicalDiskInfo}"/> collection that represents the physical disks on the Computer.</returns>
      [SecurityCritical]
      public static IEnumerable<PhysicalDiskInfo> EnumeratePhysicalDisks()
      {
         return EnumeratePhysicalDisksCore(ProcessContext.IsElevatedProcess, -1);
      }


      /// <summary>[AlphaFS] Enumerates the physical disks (including CD/DVD devices) on the Computer, populated with volume-/logical drive information.</summary>
      /// <returns>Returns an <see cref="IEnumerable{PhysicalDiskInfo}"/> collection that represents the physical disks on the Computer.</returns>
      /// <param name="isElevated"><c>true</c> indicates the current process is in an elevated state, allowing to retrieve more data.</param>
      /// <param name="deviceNumber">Retrieve a <see cref="PhysicalDiskInfo"/> instance by device number.</param>
      [SecurityCritical]
      internal static IEnumerable<PhysicalDiskInfo> EnumeratePhysicalDisksCore(bool isElevated, int deviceNumber)
      {
         var getByDeviceNumber = deviceNumber > -1;

         foreach (var deviceInfo in EnumerateDevicesCore(null, new []{DeviceGuid.Disk, DeviceGuid.CDRom}, false))
         {
            string unusedLocalDevicePath;

            // The StorageDeviceInfo is always needed as it contains the device- and partition number.

            var storageDeviceInfo = GetStorageDeviceInfoCore(isElevated, deviceNumber, deviceInfo.DevicePath, out unusedLocalDevicePath);

            if (null == storageDeviceInfo)
               continue;


            var devicePath = deviceInfo.DevicePath;

            var pDiskInfo = new PhysicalDiskInfo
            {
               DevicePath = devicePath,

               DeviceDescription = deviceInfo.DeviceDescription,

               Name = deviceInfo.FriendlyName,

               PhysicalDeviceObjectName = deviceInfo.PhysicalDeviceObjectName,


               StorageAdapterInfo = GetStorageAdapterInfoCore(isElevated, devicePath, deviceInfo.BusReportedDeviceDescription),
               
               StorageDeviceInfo = storageDeviceInfo,

               StoragePartitionInfo = GetStoragePartitionInfoCore(isElevated, devicePath)
            };


            //if (pDiskInfo.StorageDeviceInfo.TotalSize == 0)
            //   pDiskInfo.StorageDeviceInfo.TotalSize = new DiskSpaceInfo(devicePath, false, true, true).TotalNumberOfBytes;

            yield return PopulatePhysicalDisk(isElevated, pDiskInfo);


            // There can only be one.
            if (getByDeviceNumber)
               break;
         }
      }


      /// <summary>Retrieves volumes and logical drives that belong to <paramref name="pDiskInfo"/> </summary>
      internal static PhysicalDiskInfo PopulatePhysicalDisk(bool isElevated, PhysicalDiskInfo pDiskInfo)
      {
         var newPDiskInfo = Utils.CopyFrom(pDiskInfo);

         var deviceNumber = newPDiskInfo.StorageDeviceInfo.DeviceNumber;


         foreach (var volumeGuid in Volume.EnumerateVolumes())
         {
            string unusedLocalDevicePath;

            // The StorageDeviceInfo is always needed as it contains the device- and partition number.

            var storageDeviceInfo = GetStorageDeviceInfoCore(isElevated, deviceNumber, volumeGuid, out unusedLocalDevicePath);

            if (null == storageDeviceInfo)
               continue;


            AddToPartitionIndex(newPDiskInfo, storageDeviceInfo.PartitionNumber);
            
            AddToVolumeGuids(newPDiskInfo, volumeGuid);
            
            // Resolve logical drive from volume matching DeviceNumber and PartitionNumber.

            var driveName = Volume.GetVolumeDisplayName(volumeGuid);

            if (!Utils.IsNullOrWhiteSpace(driveName))
               AddToLogicalDrives(newPDiskInfo, driveName);
         }


         return newPDiskInfo;
      }


      private static void AddToPartitionIndex(PhysicalDiskInfo pDiskInfo, int deviceNumber)
      {
         // Add device partition index numbers.

         if (null == pDiskInfo.PartitionIndexes)
            pDiskInfo.PartitionIndexes = new Collection<int>();

         pDiskInfo.PartitionIndexes.Add(deviceNumber);
      }


      private static void AddToVolumeGuids(PhysicalDiskInfo pDiskInfo, string volumeGuid)
      {
         //// Add device volume labels.

         //if (null == pDiskInfo.VolumeLabels)
         //   pDiskInfo.VolumeLabels = new Collection<string>();

         //pDiskInfo.VolumeLabels.Add(pVolume.Name);
         

         // Add device volume GUIDs.

         if (null == pDiskInfo.VolumeGuids)
            pDiskInfo.VolumeGuids = new Collection<string>();

         pDiskInfo.VolumeGuids.Add(volumeGuid);
      }


      private static void AddToLogicalDrives(PhysicalDiskInfo pDiskInfo, string drivePath)
      {
         // Add device logical drive.

         if (null == pDiskInfo.LogicalDrives)
            pDiskInfo.LogicalDrives = new Collection<string>();

         pDiskInfo.LogicalDrives.Add(Path.RemoveTrailingDirectorySeparator(drivePath));
      }
   }
}
