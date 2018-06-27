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

using System.Collections.ObjectModel;
using System.Globalization;
using System.Security;
using System.Security.AccessControl;
using Alphaleonis.Win32.Filesystem;
using Alphaleonis.Win32.Security;

namespace Alphaleonis.Win32.Device
{
   public sealed partial class PhysicalDiskInfo
   {
      [SecurityCritical]
      private void CreatePhysicalDiskInfoInstance(int deviceNumber, string devicePath, StorageDeviceInfo storageDeviceInfo, DeviceInfo deviceInfo)
      {
         var isElevated = ProcessContext.IsElevatedProcess;
         var getByDeviceNumber = deviceNumber > -1;
         string physicalDriveNumberPath = null;

         bool isDrive;
         bool isVolume;
         bool isDevice;

         if (null != deviceInfo)
            devicePath = deviceInfo.DevicePath;

         var localDevicePath = FileSystemHelper.GetValidatedDevicePath(getByDeviceNumber ? Path.PhysicalDrivePrefix + deviceNumber.ToString(CultureInfo.InvariantCulture) : devicePath, out isDrive, out isVolume, out isDevice);

         localDevicePath = FileSystemHelper.GetLocalDevicePath(localDevicePath);


         // The StorageDeviceInfo is always needed as it contains the device- and partition number.

         StorageDeviceInfo = storageDeviceInfo ?? Local.GetStorageDeviceInfo(isElevated, isDevice, deviceNumber, localDevicePath, out physicalDriveNumberPath);
         
         if (null == StorageDeviceInfo)
            return;

         deviceNumber = getByDeviceNumber ? deviceNumber : StorageDeviceInfo.DeviceNumber;

         if (!SetDeviceInfoDataFromDeviceNumber(isElevated, deviceNumber, deviceInfo))
            return;


         // If physicalDriveNumberPath != null, the drive is opened using: "\\.\PhysicalDriveX" path format
         // which is the device, not the volume/logical drive.

         localDevicePath = FileSystemHelper.GetValidatedDevicePath(physicalDriveNumberPath ?? localDevicePath, out isDrive, out isVolume, out isDevice);

         
         AddDeviceInfoData(isElevated, isDevice, deviceNumber, localDevicePath);
      }


      private void AddDeviceInfoData(bool isElevated, bool isDevice, int deviceNumber, string localDevicePath)
      {
         DosDeviceName = Volume.QueryDosDevice(Path.GetRegularPathCore(localDevicePath, GetFullPathOptions.None, false));


         using (var safeFileHandle = Local.OpenDevice(localDevicePath, isElevated ? FileSystemRights.Read : NativeMethods.FILE_ANY_ACCESS))
         {
            StorageAdapterInfo = Local.GetStorageAdapterInfo(safeFileHandle, deviceNumber, localDevicePath, _deviceInfo.BusReportedDeviceDescription);

            StoragePartitionInfo = Local.GetStoragePartitionInfo(safeFileHandle, deviceNumber, localDevicePath);
         }


         UpdateDevicePartitionData(isElevated, isDevice, localDevicePath);

         PopulatePhysicalDisk(isElevated);
      }


      /// <summary>Retrieves volumes/logical drives that belong to the PhysicalDiskInfo instance.</summary>
      [SecurityCritical]
      private void PopulatePhysicalDisk(bool isElevated)
      {
         var deviceNumber = StorageDeviceInfo.DeviceNumber;

         _partitionIndexCollection = new Collection<int>();
         _volumeGuidCollection = new Collection<string>();
         _logicalDriveCollection = new Collection<string>();


         foreach (var volumeGuid in Volume.EnumerateVolumes())
         {
            string unusedLocalDevicePath;

            // The StorageDeviceInfo is always needed as it contains the device- and partition number.

            var storageDeviceInfo = Local.GetStorageDeviceInfo(isElevated, false, deviceNumber, volumeGuid, out unusedLocalDevicePath);

            if (null == storageDeviceInfo)
               continue;


            _partitionIndexCollection.Add(storageDeviceInfo.PartitionNumber);

            _volumeGuidCollection.Add(volumeGuid);


            // Resolve logical drive from volume matching DeviceNumber and PartitionNumber.

            var driveName = Volume.GetVolumeDisplayName(volumeGuid);
            
            if (!Utils.IsNullOrWhiteSpace(driveName))

               _logicalDriveCollection.Add(Path.RemoveTrailingDirectorySeparator(driveName));
         }


         PartitionIndexes = _partitionIndexCollection;

         VolumeGuids = _volumeGuidCollection;

         LogicalDrives = _logicalDriveCollection;
      }


      [SecurityCritical]
      private bool SetDeviceInfoDataFromDeviceNumber(bool isElevated, int deviceNumber, DeviceInfo deviceInfo)
      {
         if (null == deviceInfo)
            foreach (var device in Local.EnumerateDevicesCore(null, new[] {DeviceGuid.Disk, DeviceGuid.CDRom}, false))
            {
               string unusedDevicePath;

               var storageDeviceInfo = Local.GetStorageDeviceInfo(isElevated, true, deviceNumber, device.DevicePath, out unusedDevicePath);

               if (null != storageDeviceInfo)
               {
                  deviceInfo = device;
                  break;
               }
            }


         _deviceInfo = deviceInfo;

         return null != deviceInfo && !Utils.IsNullOrWhiteSpace(deviceInfo.DevicePath);
      }


      [SecurityCritical]
      private void UpdateDevicePartitionData(bool isElevated, bool isDevice, string localDevicePath)
      {
         if (StoragePartitionInfo.OnDynamicDisk)
         {
            // At this point, PartitionNumber = 0 which points to the device.
            // Get the user data partition; the partition that normally occupies most of the disk space.

            foreach (var partition in StoragePartitionInfo.GptPartitionInfo)
            {
               if (partition.PartitionType == PartitionType.LdmData)
               {
                  StorageDeviceInfo.PartitionNumber = partition.PartitionNumber;

                  StorageDeviceInfo.TotalSize = partition.PartitionLength;

                  break;
               }
            }
         }

         else if (!isElevated && StorageDeviceInfo.TotalSize == 0 && null != StoragePartitionInfo)

            StorageDeviceInfo.TotalSize = isDevice ? StoragePartitionInfo.TotalSize : new DiskSpaceInfo(localDevicePath, false, true, true).TotalNumberOfBytes;
      }


      
      
      /// <summary>Returns the "FriendlyName" of the physical disk.</summary>
      /// <returns>Returns a string that represents this instance.</returns>
      public override string ToString()
      {
         return Name ?? DevicePath;
      }


      /// <summary>Determines whether the specified Object is equal to the current Object.</summary>
      /// <param name="obj">Another object to compare to.</param>
      /// <returns><c>true</c> if the specified Object is equal to the current Object; otherwise, <c>false</c>.</returns>
      public override bool Equals(object obj)
      {
         if (null == obj || GetType() != obj.GetType())
            return false;

         var other = obj as PhysicalDiskInfo;

         return null != other &&
                other.Name == Name &&
                other.DevicePath == DevicePath &&
                other.DosDeviceName == DosDeviceName &&
                other.PhysicalDeviceObjectName == PhysicalDeviceObjectName &&
                other.PartitionIndexes == PartitionIndexes &&
                other.VolumeGuids == VolumeGuids &&
                other.LogicalDrives == LogicalDrives &&
                other.StorageAdapterInfo == StorageAdapterInfo &&
                other.StorageDeviceInfo == StorageDeviceInfo &&
                other.StoragePartitionInfo == StoragePartitionInfo;
      }


      /// <summary>Serves as a hash function for a particular type.</summary>
      /// <returns>Returns a hash code for the current Object.</returns>
      public override int GetHashCode()
      {
         return null != DevicePath ? DevicePath.GetHashCode() : 0;
      }


      /// <summary>Implements the operator ==</summary>
      /// <param name="left">A.</param>
      /// <param name="right">B.</param>
      /// <returns>The result of the operator.</returns>
      public static bool operator ==(PhysicalDiskInfo left, PhysicalDiskInfo right)
      {
         return ReferenceEquals(left, null) && ReferenceEquals(right, null) || !ReferenceEquals(left, null) && !ReferenceEquals(right, null) && left.Equals(right);
      }


      /// <summary>Implements the operator !=</summary>
      /// <param name="left">A.</param>
      /// <param name="right">B.</param>
      /// <returns>The result of the operator.</returns>
      public static bool operator !=(PhysicalDiskInfo left, PhysicalDiskInfo right)
      {
         return !(left == right);
      }
   }
}
