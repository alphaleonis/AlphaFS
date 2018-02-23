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
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security;
using System.Security.AccessControl;

namespace Alphaleonis.Win32.Filesystem
{
   public static partial class Device
   {
      /// <summary>[AlphaFS] Retrieves the physical drive on the Computer that is related to the logical drive name, volume GUID or <see cref="DeviceInfo.DevicePath"/>.
      /// <para>Most properties of the returned <see cref="PhysicalDriveInfo.StorageAdapterInfo"/> and <see cref="PhysicalDriveInfo.StorageDeviceInfo"/> instances are meaningless unless this method is called from an elevated state.</para>
      /// <para>Do not call this method for every volume or logical drive on the system as each call queries all physical drives and associated volumes and logical drives.</para>
      /// <para>Instead, use method <see cref="EnumeratePhysicalDrives()"/> and property <see cref="PhysicalDriveInfo.VolumeGuids"/> and/or <see cref="PhysicalDriveInfo.LogicalDrives"/>.</para>
      /// </summary>
      /// <returns>A <see cref="PhysicalDriveInfo"/> instance that represents the physical drive on the Computer or <c>null</c> on error/no data available.</returns>
      ///  <exception cref="ArgumentException"/>
      ///  <exception cref="ArgumentNullException"/>
      ///  <exception cref="NotSupportedException"/>
      /// <param name="devicePath">
      ///    A drive path such as: "C", "C:" or "C:\".
      ///    A volume <see cref="Guid"/> such as: "\\?\Volume{xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}\".
      ///    A <see cref="DeviceInfo.DevicePath"/> string (when <see cref="DeviceInfo.ClassGuid"/> is set to <see cref="DeviceGuid.Disk"/>).
      /// </param>
      [SecurityCritical]
      public static PhysicalDriveInfo GetPhysicalDriveInfo(string devicePath)
      {
         bool isDrive;
         bool isVolume;
         bool isDeviceInfo;
         
         var isElevated = Security.ProcessContext.IsElevatedProcess;

         devicePath = ValidateDevicePath(devicePath, out isDrive, out isVolume, out isDeviceInfo);
         
         
         var storageDeviceInfo = GetStorageDeviceInfoCore(isElevated, devicePath);

         if (null == storageDeviceInfo)
            return null;


         // Use logical drive path.

         if (isDrive)
            GetDevicePath(devicePath, out devicePath);


         var pDriveInfo = isDeviceInfo

            ? EnumeratePhysicalDrivesCore(isElevated).SingleOrDefault(pDrive => pDrive.StorageDeviceInfo.DeviceNumber == storageDeviceInfo.DeviceNumber && pDrive.StorageDeviceInfo.PartitionNumber == storageDeviceInfo.PartitionNumber)

            : isVolume
               ? EnumeratePhysicalDrivesCore(isElevated).SingleOrDefault(pDrive => null != pDrive.VolumeGuids && pDrive.VolumeGuids.Contains(devicePath, StringComparer.OrdinalIgnoreCase))

               : isDrive
                  ? EnumeratePhysicalDrivesCore(isElevated).SingleOrDefault(pDrive => null != pDrive.LogicalDrives && pDrive.LogicalDrives.Contains(devicePath, StringComparer.OrdinalIgnoreCase))
                  : null;


         if (null != pDriveInfo && null != pDriveInfo.StorageDeviceInfo)
            pDriveInfo.StorageDeviceInfo.PartitionNumber = storageDeviceInfo.PartitionNumber;


         return pDriveInfo;
      }


      /// <summary>[AlphaFS] Retrieves the physical drive on the Computer that is related to the logical drive name, volume GUID or <see cref="DeviceInfo.DevicePath"/>.
      /// <para>Most properties of the returned <see cref="PhysicalDriveInfo.StorageAdapterInfo"/> and <see cref="PhysicalDriveInfo.StorageDeviceInfo"/> instances are meaningless unless this method is called from an elevated state.</para>
      /// </summary>
      ///  <returns>A <see cref="PhysicalDriveInfo"/> instance that represents the physical drive on the Computer or <c>null</c> on error/no data available.</returns>
      ///  <exception cref="ArgumentException"/>
      ///  <exception cref="ArgumentNullException"/>
      ///  <exception cref="NotSupportedException"/>
      ///  <exception cref="Exception"/>
      /// <param name="isElevated"><see langword="true"/> indicates the current process is in an elevated state, allowing to retrieve more data.</param>
      /// <param name="devicePath">
      ///    A drive path such as: "C", "C:" or "C:\".
      ///    A volume <see cref="Guid"/> such as: "\\?\Volume{xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}\".
      ///    A <see cref="DeviceInfo.DevicePath"/> string (when <see cref="DeviceInfo.ClassGuid"/> is set to <see cref="DeviceGuid.Disk"/>).
      /// </param>
      /// <param name="storageDeviceInfo">A <see cref="StorageDeviceInfo"/> instance.</param>
      /// <param name="deviceInfo">A <see cref="DeviceInfo"/> instance.</param>
      /// <remarks>Use either <paramref name="devicePath"/> or <paramref name="deviceInfo"/>, not both.</remarks>
      [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Object is disposed.")]
      [SecurityCritical]
      internal static PhysicalDriveInfo GetPhysicalDriveInfoCore(bool isElevated, string devicePath, StorageDeviceInfo storageDeviceInfo, DeviceInfo deviceInfo)
      {
         var isDeviceInfo = null != deviceInfo && !Utils.IsNullOrWhiteSpace(deviceInfo.DevicePath);

         if (isDeviceInfo)
            devicePath = deviceInfo.DevicePath;


         if (null == storageDeviceInfo)
            storageDeviceInfo = GetStorageDeviceInfoCore(false, devicePath);

         if (null == storageDeviceInfo)
            return null;


         var pDriveInfo = new PhysicalDriveInfo
         {
            DevicePath = devicePath,

            DeviceDescription = isDeviceInfo ? deviceInfo.DeviceDescription : null,

            Name = isDeviceInfo ? deviceInfo.FriendlyName : null,

            PhysicalDeviceObjectName = isDeviceInfo ? deviceInfo.PhysicalDeviceObjectName : null
         };


         // When elevated, get populated StorageAdapterInfo and StorageDeviceInfo instances.

         if (isElevated)
         {
            GetDriveStuff(@"\\.\C:");


            using (var safeHandle = OpenPhysicalDrive(devicePath, FileSystemRights.Read))
            {
               pDriveInfo.StorageAdapterInfo = GetStorageAdapterInfoNative(safeHandle, devicePath);

               storageDeviceInfo = GetStorageDeviceInfoNative(safeHandle, devicePath, storageDeviceInfo);
            }
         }


         pDriveInfo.StorageDeviceInfo = storageDeviceInfo;


         if (isDeviceInfo)
         {
            if (null == pDriveInfo.StorageAdapterInfo)
               pDriveInfo.StorageAdapterInfo = new StorageAdapterInfo();

            pDriveInfo.StorageAdapterInfo.BusReportedDeviceDescription = deviceInfo.BusReportedDeviceDescription;
         }
         

         return pDriveInfo;
      }
   }
}
