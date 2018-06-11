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
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security;
using System.Security.AccessControl;
using Alphaleonis.Win32.Filesystem;
using Alphaleonis.Win32.Security;

namespace Alphaleonis.Win32.Device
{
   public static partial class Local
   {
      /// <summary>[AlphaFS] Retrieves the physical disk that is related to the logical drive name, volume <see cref="Guid"/> or <see cref="DeviceInfo.DevicePath"/>.</summary>
      /// <returns>Returns a <see cref="PhysicalDiskInfo"/> instance that represents the physical disk on the Computer or <c>null</c> on error/no data available.</returns>
      /// <remark>
      ///   Most properties of the returned <see cref="PhysicalDiskInfo.StorageAdapterInfo"/> and <see cref="PhysicalDiskInfo.StorageDeviceInfo"/> instances are meaningless unless this method is called from an elevated state.
      ///   Do not call this method for every volume or logical drive on the system as each call queries all physical disks, associated volumes and logical drives.
      ///   Instead, use method <see cref="EnumeratePhysicalDisks()"/> and property <see cref="PhysicalDiskInfo.VolumeGuids"/> and/or <see cref="PhysicalDiskInfo.LogicalDrives"/>.
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
         return GetPhysicalDiskInfo(ProcessContext.IsElevatedProcess, devicePath);
      }


      /// <summary>[AlphaFS] Retrieves the physical disk that is related to the logical drive name, volume <see cref="Guid"/> or <see cref="DeviceInfo.DevicePath"/>.</summary>
      /// <returns>Returns a <see cref="PhysicalDiskInfo"/> instance that represents the physical disk on the Computer or <c>null</c> on error/no data available.</returns>
      /// <remark>
      ///   Most properties of the returned <see cref="PhysicalDiskInfo.StorageAdapterInfo"/> and <see cref="PhysicalDiskInfo.StorageDeviceInfo"/> instances are meaningless unless this method is called from an elevated state.
      ///   Do not call this method for every volume or logical drive on the system as each call queries all physical disks, associated volumes and logical drives.
      ///   Instead, use method <see cref="EnumeratePhysicalDisks()"/> and property <see cref="PhysicalDiskInfo.VolumeGuids"/> and/or <see cref="PhysicalDiskInfo.LogicalDrives"/>.
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
         bool isDrive;
         bool isVolume;
         bool isDevice;

         var validatedDevicePath = FileSystemHelper.GetValidatedDevicePath(devicePath, out isDrive, out isVolume, out isDevice);

         if (isDrive)
            validatedDevicePath = FileSystemHelper.GetLocalDevicePath(validatedDevicePath);


         // We need the StorageDeviceInfo to locate our input path.

         var storageDeviceInfo = GetStorageDeviceInfo(isElevated, validatedDevicePath);

         // Get the PhysicalDiskInfo instance.

         var pDiskInfo = null == storageDeviceInfo ? null : isDevice

            ? EnumeratePhysicalDisksCore(isElevated).SingleOrDefault(pDisk => pDisk.StorageDeviceInfo.DeviceNumber == storageDeviceInfo.DeviceNumber && pDisk.StorageDeviceInfo.PartitionNumber == storageDeviceInfo.PartitionNumber)

            : isVolume
               ? EnumeratePhysicalDisksCore(isElevated).SingleOrDefault(pDisk => pDisk.ContainsVolume(validatedDevicePath))

               : isDrive
                  ? EnumeratePhysicalDisksCore(isElevated).SingleOrDefault(pDisk => pDisk.ContainsVolume(validatedDevicePath))
                  : null;


         if (null != pDiskInfo)
         {
            pDiskInfo.StorageDeviceInfo = storageDeviceInfo;

            if (null == pDiskInfo.StoragePartitionInfo)
               pDiskInfo.StoragePartitionInfo = GetStoragePartitionInfo(isElevated, validatedDevicePath);
         }


         return pDiskInfo;
      }


      /// <summary>[AlphaFS] Retrieves the physical disk that is related to the logical drive name, volume <see cref="Guid"/> or <see cref="DeviceInfo.DevicePath"/>.</summary>
      ///  <returns>A <see cref="PhysicalDiskInfo"/> instance that represents the physical disk on the Computer or <c>null</c> on error/no data available.</returns>
      /// <remark>
      ///   Most properties of the returned <see cref="PhysicalDiskInfo.StorageAdapterInfo"/> and <see cref="PhysicalDiskInfo.StorageDeviceInfo"/> instances are meaningless unless this method is called from an elevated state.
      ///   Do not call this method for every volume or logical drive on the system as each call queries all physical disks, associated volumes and logical drives.
      ///   Instead, use method <see cref="EnumeratePhysicalDisks()"/> and property <see cref="PhysicalDiskInfo.VolumeGuids"/> and/or <see cref="PhysicalDiskInfo.LogicalDrives"/>.
      /// </remark>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="Exception"/>
      /// <param name="isElevated"><c>true</c> indicates the current process is in an elevated state, allowing to retrieve more data.</param>
      /// <param name="devicePath">
      ///    <para>A disk path such as: <c>\\.\PhysicalDrive0</c></para>
      ///    <para>A drive path such as: <c>C</c>, <c>C:</c> or <c>C:\</c></para>
      ///    <para>A volume <see cref="Guid"/> such as: <c>\\?\Volume{xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}\</c></para>
      ///    <para>A <see cref="DeviceInfo.DevicePath"/> string such as: <c>\\?\scsi#disk...{xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}</c></para>
      /// </param>
      /// <param name="deviceInfo">A <see cref="DeviceInfo"/> instance.</param>
      /// <remarks>Use either <paramref name="devicePath"/> or <paramref name="deviceInfo"/>, not both.</remarks>
      [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Object is disposed.")]
      [SecurityCritical]
      private static PhysicalDiskInfo NewPhysicalDiskInfo(bool isElevated, string devicePath, DeviceInfo deviceInfo)
      {
         if (devicePath == null && deviceInfo == null)
            return null;

         var isDevice = null != deviceInfo && !Utils.IsNullOrWhiteSpace(deviceInfo.DevicePath);

         if (isDevice)
            devicePath = deviceInfo.DevicePath;

         
         var pDiskInfo = new PhysicalDiskInfo
         {
            StorageAdapterInfo = GetStorageAdapterInfo(isElevated, devicePath),

            StorageDeviceInfo = GetStorageDeviceInfo(isElevated, devicePath),

            StoragePartitionInfo = GetStoragePartitionInfo(isElevated, devicePath),


            DevicePath = devicePath,

            DeviceDescription = isDevice ? deviceInfo.DeviceDescription : null,

            Name = isDevice ? deviceInfo.FriendlyName : null,

            PhysicalDeviceObjectName = isDevice ? deviceInfo.PhysicalDeviceObjectName : null,
         };


         if (isDevice)
            pDiskInfo.StorageAdapterInfo.BusReportedDeviceDescription = deviceInfo.BusReportedDeviceDescription;


         return pDiskInfo;
      }
   }
}
