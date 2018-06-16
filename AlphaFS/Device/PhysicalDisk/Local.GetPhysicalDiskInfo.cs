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
using System.Linq;
using System.Security;
using Alphaleonis.Win32.Filesystem;
using Alphaleonis.Win32.Security;

namespace Alphaleonis.Win32.Device
{
   public static partial class Local
   {
      /// <summary>[AlphaFS] Retrieves the physical disk that is related to the logical drive name, volume <see cref="Guid"/> or <see cref="DeviceInfo.DevicePath"/>.</summary>
      /// <returns>Returns a <see cref="PhysicalDiskInfo"/> instance that represents the physical disk on the Computer or <c>null</c> if the device can not be retrieved.</returns>
      /// <remark>
      ///   Most properties of the returned <see cref="PhysicalDiskInfo.StorageAdapterInfo"/> and <see cref="PhysicalDiskInfo.StorageDeviceInfo"/> instances are meaningless unless this method is called from an elevated state.
      ///   Do not call this method for every logical drive/volume on the Computer as each call queries all physical disks, associated volumes and logical drives.
      ///   Instead, use method <see cref="EnumeratePhysicalDisks()"/> and property <see cref="PhysicalDiskInfo.VolumeGuids"/> or <see cref="PhysicalDiskInfo.LogicalDrives"/>.
      /// </remark>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="NotSupportedException"/>
      /// <param name="devicePath">
      /// <para>A disk path such as: <c>\\.\PhysicalDrive0</c></para>
      /// <para>A drive path such as: <c>C</c>, <c>C:</c> or <c>C:\</c></para>
      /// <para>A volume <see cref="Guid"/> such as: <c>\\?\Volume{xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}\</c></para>
      /// <para>A <see cref="DeviceInfo.DevicePath"/> string such as: <c>\\?\scsi#disk...{xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}</c></para>
      /// </param>
      [SecurityCritical]
      public static PhysicalDiskInfo GetPhysicalDiskInfo(string devicePath)
      {
         return GetPhysicalDiskInfoCore(ProcessContext.IsElevatedProcess, -1, devicePath);
      }


      /// <summary>[AlphaFS] Retrieves the physical disk that is related to the logical drive name, volume <see cref="Guid"/> or <see cref="DeviceInfo.DevicePath"/>.</summary>
      /// <returns>Returns a <see cref="PhysicalDiskInfo"/> instance that represents the physical disk on the Computer or <c>null</c> if the device can not be retrieved.</returns>
      /// <remark>
      ///   Most properties of the returned <see cref="PhysicalDiskInfo.StorageAdapterInfo"/> and <see cref="PhysicalDiskInfo.StorageDeviceInfo"/> instances are meaningless unless this method is called from an elevated state.
      ///   Do not call this method for every logical drive/volume on the Computer as each call queries all physical disks, associated volumes and logical drives.
      ///   Instead, use method <see cref="EnumeratePhysicalDisks()"/> and property <see cref="PhysicalDiskInfo.VolumeGuids"/> or <see cref="PhysicalDiskInfo.LogicalDrives"/>.
      /// </remark>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="NotSupportedException"/>
      /// <param name="isElevated"><c>true</c> indicates the current process is in an elevated state, allowing to retrieve more data.</param>
      /// <param name="devicePath">
      /// <para>A disk path such as: <c>\\.\PhysicalDrive0</c></para>
      /// <para>A drive path such as: <c>C</c>, <c>C:</c> or <c>C:\</c></para>
      /// <para>A volume <see cref="Guid"/> such as: <c>\\?\Volume{xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}\</c></para>
      /// <para>A <see cref="DeviceInfo.DevicePath"/> string such as: <c>\\?\scsi#disk...{xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}</c></para>
      /// </param>
      [SecurityCritical]
      public static PhysicalDiskInfo GetPhysicalDiskInfo(bool isElevated, string devicePath)
      {
         return GetPhysicalDiskInfoCore(isElevated, -1, devicePath);
      }
      



      /// <summary>[AlphaFS] Retrieves the physical disk that is related to the logical drive name, volume <see cref="Guid"/> or <see cref="DeviceInfo.DevicePath"/>.</summary>
      /// <returns>Returns a <see cref="PhysicalDiskInfo"/> instance that represents the physical disk on the Computer or <c>null</c> if the device can not be retrieved.</returns>
      /// <remark>
      ///   Most properties of the returned <see cref="PhysicalDiskInfo.StorageAdapterInfo"/> and <see cref="PhysicalDiskInfo.StorageDeviceInfo"/> instances are meaningless unless this method is called from an elevated state.
      ///   Do not call this method for every logical drive/volume on the Computer as each call queries all physical disks, associated volumes and logical drives.
      ///   Instead, use method <see cref="EnumeratePhysicalDisks()"/> and property <see cref="PhysicalDiskInfo.VolumeGuids"/> or <see cref="PhysicalDiskInfo.LogicalDrives"/>.
      /// </remark>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="NotSupportedException"/>
      /// <param name="isElevated"><c>true</c> indicates the current process is in an elevated state, allowing to retrieve more data.</param>
      /// <param name="deviceNumber">Retrieve a <see cref="PhysicalDiskInfo"/> instance by device number.</param>
      /// <param name="devicePath">
      ///    <para>A disk path such as: <c>\\.\PhysicalDrive0</c></para>
      ///    <para>A drive path such as: <c>C</c>, <c>C:</c> or <c>C:\</c></para>
      ///    <para>A volume <see cref="Guid"/> such as: <c>\\?\Volume{xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}\</c></para>
      ///    <para>A <see cref="DeviceInfo.DevicePath"/> string such as: <c>\\?\scsi#disk...{xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}</c></para>
      /// </param>
      [SecurityCritical]
      internal static PhysicalDiskInfo GetPhysicalDiskInfoCore(bool isElevated, int deviceNumber, string devicePath)
      {
         var getByDeviceNumber = deviceNumber > -1;
         bool isDevice;
         var isDrive = false;
         var isVolume = false;

         var localDevicePath = getByDeviceNumber ? Path.PhysicalDrivePrefix + deviceNumber.ToString(CultureInfo.InvariantCulture) : FileSystemHelper.GetValidatedDevicePath(devicePath, out isDrive, out isVolume, out isDevice);

         if (isDrive)
            localDevicePath = FileSystemHelper.GetLocalDevicePath(localDevicePath);
         

         // The StorageDeviceInfo is always needed as it contains the device- and partition number.

         var storageDeviceInfo = GetStorageDeviceInfoCore(isElevated, deviceNumber, localDevicePath, out localDevicePath);

         if (null == storageDeviceInfo)
            return null;

         if (!getByDeviceNumber)
            deviceNumber = storageDeviceInfo.DeviceNumber;


         var physicalDiskInfo = EnumeratePhysicalDisksCore(isElevated, deviceNumber).FirstOrDefault();

         if (null != physicalDiskInfo)
         {
            if (isDrive || isVolume)
               physicalDiskInfo.StorageDeviceInfo = storageDeviceInfo;

            if (null == physicalDiskInfo.StoragePartitionInfo)
               physicalDiskInfo.StoragePartitionInfo = GetStoragePartitionInfoCore(isElevated, localDevicePath);
         }


         return physicalDiskInfo;
      }
   }
}
