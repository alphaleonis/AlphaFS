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

namespace Alphaleonis.Win32.Device
{
   public static partial class Local
   {
      /// <summary>[AlphaFS] Retrieves the physical disk on the Computer that is related to the logical drive name, volume <see cref="Guid"/> or <see cref="DeviceInfo.DevicePath"/>.</summary>
      /// <returns>Returns a <see cref="PhysicalDiskInfo"/> instance that represents the physical disk on the Computer or <c>null</c> on error/no data available.</returns>
      /// <remark>
      ///   Most properties of the returned <see cref="PhysicalDiskInfo.StorageAdapterInfo"/> and <see cref="PhysicalDiskInfo.StorageDeviceInfo"/> instances are meaningless unless this method is called from an elevated state.
      ///   Do not call this method for every volume or logical drive on the system as each call queries all physical disks, associated volumes and logical drives.
      ///   Instead, use method <see cref="EnumeratePhysicalDisks"/> and property <see cref="PhysicalDiskInfo.VolumeGuids"/> and/or <see cref="PhysicalDiskInfo.LogicalDrives"/>.
      /// </remark>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="NotSupportedException"/>
      /// <param name="devicePath">
      /// <para>A disk path such as: <c>\\.\PhysicalDrive0</c></para>
      /// <para>A drive path such as: <c>C</c>, <c>C:</c> or <c>C:\</c></para>
      /// <para>A volume <see cref="Guid"/> such as: <c>\\?\Volume{xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}\</c></para>
      /// <para>A <see cref="DeviceInfo.DevicePath"/> string such as: <c>\\?\pcistor#disk...{xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}</c></para>
      /// </param>
      [SecurityCritical]
      public static PhysicalDiskInfo GetPhysicalDiskInfo(string devicePath)
      {
         bool isDrive;
         bool isVolume;
         bool isDeviceInfo;
         
         var isElevated = Security.ProcessContext.IsElevatedProcess;

         devicePath = FileSystemHelper.ValidateDevicePath(devicePath, out isDrive, out isVolume, out isDeviceInfo);
         
         
         var storageDeviceInfo = GetStorageDeviceInfoCore(isElevated, devicePath);

         if (null == storageDeviceInfo)
            return null;


         // Use logical drive path.

         if (isDrive)
            FileSystemHelper.GetDevicePath(devicePath, out devicePath);


         var pDiskInfo = isDeviceInfo

            ? EnumeratePhysicalDisksCore(isElevated).SingleOrDefault(pDisk => pDisk.StorageDeviceInfo.DeviceNumber == storageDeviceInfo.DeviceNumber && pDisk.StorageDeviceInfo.PartitionNumber == storageDeviceInfo.PartitionNumber)

            : isVolume
               ? EnumeratePhysicalDisksCore(isElevated).SingleOrDefault(pDisk => null != pDisk.VolumeGuids && pDisk.VolumeGuids.Contains(devicePath, StringComparer.OrdinalIgnoreCase))

               : isDrive
                  ? EnumeratePhysicalDisksCore(isElevated).SingleOrDefault(pDisk => null != pDisk.LogicalDrives && pDisk.LogicalDrives.Contains(devicePath, StringComparer.OrdinalIgnoreCase))
                  : null;


         if (null != pDiskInfo && null != pDiskInfo.StorageDeviceInfo)
            pDiskInfo.StorageDeviceInfo.PartitionNumber = storageDeviceInfo.PartitionNumber;


         return pDiskInfo;
      }


      /// <summary>[AlphaFS] Retrieves the physical disk on the Computer that is related to the logical drive name, volume <see cref="Guid"/> or <see cref="DeviceInfo.DevicePath"/>.</summary>
      ///  <returns>A <see cref="PhysicalDiskInfo"/> instance that represents the physical disk on the Computer or <c>null</c> on error/no data available.</returns>
      /// <remark>
      ///   Most properties of the returned <see cref="PhysicalDiskInfo.StorageAdapterInfo"/> and <see cref="PhysicalDiskInfo.StorageDeviceInfo"/> instances are meaningless unless this method is called from an elevated state.
      ///   Do not call this method for every volume or logical drive on the system as each call queries all physical disks, associated volumes and logical drives.
      ///   Instead, use method <see cref="EnumeratePhysicalDisks"/> and property <see cref="PhysicalDiskInfo.VolumeGuids"/> and/or <see cref="PhysicalDiskInfo.LogicalDrives"/>.
      /// </remark>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="Exception"/>
      /// <param name="isElevated"><c>true</c> indicates the current process is in an elevated state, allowing to retrieve more data.</param>
      /// <param name="devicePath">
      /// <para>A disk path such as: <c>\\.\PhysicalDrive0</c></para>
      /// <para>A drive path such as: <c>C</c>, <c>C:</c> or <c>C:\</c></para>
      /// <para>A volume <see cref="Guid"/> such as: <c>\\?\Volume{xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}\</c></para>
      /// <para>A <see cref="DeviceInfo.DevicePath"/> string such as: <c>\\?\pcistor#disk...{xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}</c></para>
      /// </param>
      /// <param name="storageDeviceInfo">A <see cref="StorageDeviceInfo"/> instance.</param>
      /// <param name="deviceInfo">A <see cref="DeviceInfo"/> instance.</param>
      /// <remarks>Use either <paramref name="devicePath"/> or <paramref name="deviceInfo"/>, not both.</remarks>
      [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Object is disposed.")]
      [SecurityCritical]
      internal static PhysicalDiskInfo GetPhysicalDiskInfoCore(bool isElevated, string devicePath, StorageDeviceInfo storageDeviceInfo, DeviceInfo deviceInfo)
      {
         var isDeviceInfo = null != deviceInfo && !Utils.IsNullOrWhiteSpace(deviceInfo.DevicePath);

         if (isDeviceInfo)
            devicePath = deviceInfo.DevicePath;


         if (null == storageDeviceInfo)
            storageDeviceInfo = GetStorageDeviceInfoCore(false, devicePath);

         if (null == storageDeviceInfo)
            return null;


         var pDiskInfo = new PhysicalDiskInfo
         {
            DevicePath = devicePath,

            DeviceDescription = isDeviceInfo ? deviceInfo.DeviceDescription : null,

            Name = isDeviceInfo ? deviceInfo.FriendlyName : null,

            PhysicalDeviceObjectName = isDeviceInfo ? deviceInfo.PhysicalDeviceObjectName : null
         };


         // When elevated, get populated StorageAdapterInfo and StorageDeviceInfo instances.

         if (isElevated)
         {
            using (var safeHandle = FileSystemHelper.OpenPhysicalDisk(devicePath, FileSystemRights.Read))
            {
               pDiskInfo.StorageAdapterInfo = GetStorageAdapterInfoNative(safeHandle, devicePath);

               storageDeviceInfo = GetStorageDeviceInfoNative(safeHandle, devicePath, storageDeviceInfo);

               pDiskInfo.StoragePartitionInfo = GetStoragePartitionInfoNative(safeHandle, devicePath);
            }
         }


         pDiskInfo.StorageDeviceInfo = storageDeviceInfo;


         if (isDeviceInfo)
         {
            if (null == pDiskInfo.StorageAdapterInfo)
               pDiskInfo.StorageAdapterInfo = new StorageAdapterInfo();

            pDiskInfo.StorageAdapterInfo.BusReportedDeviceDescription = deviceInfo.BusReportedDeviceDescription;
         }
         

         return pDiskInfo;
      }
   }
}
