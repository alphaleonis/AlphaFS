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
using System.Globalization;
using System.Security;
using System.Security.AccessControl;
using Alphaleonis.Win32.Filesystem;
using Alphaleonis.Win32.Security;
using Microsoft.Win32.SafeHandles;

namespace Alphaleonis.Win32.Device
{
   public static partial class Local
   {
      /// <summary>[AlphaFS] Retrieves the type, device- and partition number for the storage device that is related to the logical drive name, volume <see cref="Guid"/> or <see cref="DeviceInfo.DevicePath"/>.</summary>
      /// <returns>Returns a <see cref="StorageDeviceInfo"/> instance that represent the storage device that is related to <paramref name="devicePath"/>.</returns>
      /// <remarks>When this method is called from a non-elevated state, the <see cref="StorageDeviceInfo.TotalSize"/> property always returns <c>0</c>.</remarks>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="Exception"/>
      /// <param name="devicePath">
      /// <para>A disk path such as: <c>\\.\PhysicalDrive0</c></para>
      /// <para>A drive path such as: <c>C</c>, <c>C:</c> or <c>C:\</c></para>
      /// <para>A volume <see cref="Guid"/> such as: <c>\\?\Volume{xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}\</c></para>
      /// <para>A <see cref="DeviceInfo.DevicePath"/> string such as: <c>\\?\scsi#disk...{xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}</c></para>
      /// </param>
      [SecurityCritical]
      public static StorageDeviceInfo GetStorageDeviceInfo(string devicePath)
      {
         string unusedLocalDevicePath;

         return GetStorageDeviceInfoCore(ProcessContext.IsElevatedProcess, -1, devicePath, out unusedLocalDevicePath);
      }


      /// <returns>Returns a <see cref="StorageDeviceInfo"/> instance that represent the storage device that is related to <paramref name="devicePath"/>.</returns>
      /// <remarks>When this method is called from a non-elevated state, the <see cref="StorageDeviceInfo.TotalSize"/> property always returns <c>0</c>.</remarks>
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
      [SecurityCritical]
      public static StorageDeviceInfo GetStorageDeviceInfo(bool isElevated, string devicePath)
      {
         string unusedLocalDevicePath;

         return GetStorageDeviceInfoCore(isElevated, -1, devicePath, out unusedLocalDevicePath);
      }


      /// <returns>Returns a <see cref="StorageDeviceInfo"/> instance that represent the storage device that is related to <paramref name="devicePath"/>.</returns>
      /// <remarks>When this method is called from a non-elevated state, the <see cref="StorageDeviceInfo.TotalSize"/> property always returns <c>0</c>.</remarks>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="Exception"/>
      /// <param name="isElevated"><c>true</c> indicates the current process is in an elevated state, allowing to retrieve more data.</param>
      /// <param name="deviceNumber">A number that indicates a physical disk on the Computer.</param>
      /// <param name="devicePath">
      ///    <para>A disk path such as: <c>\\.\PhysicalDrive0</c></para>
      ///    <para>A drive path such as: <c>C</c>, <c>C:</c> or <c>C:\</c></para>
      ///    <para>A volume <see cref="Guid"/> such as: <c>\\?\Volume{xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}\</c></para>
      ///    <para>A <see cref="DeviceInfo.DevicePath"/> string such as: <c>\\?\scsi#disk...{xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}</c></para>
      /// </param>
      /// <param name="localDevicePath">The resolved local device path such as <c>\\.\C:</c> or <c>\\.\PhysicalDrive0</c></param>
      [SecurityCritical]
      private static StorageDeviceInfo GetStorageDeviceInfoCore(bool isElevated, int deviceNumber, string devicePath, out string localDevicePath)
      {
         bool isDrive;
         bool isVolume;
         bool isDevice;

         localDevicePath = FileSystemHelper.GetValidatedDevicePath(devicePath, out isDrive, out isVolume, out isDevice);

         if (isDrive)
            localDevicePath = FileSystemHelper.GetLocalDevicePath(localDevicePath);

         var retry = false;

      Retry:
         
         using (var safeHandle = FileSystemHelper.OpenPhysicalDisk(localDevicePath, isElevated ? FileSystemRights.Read : NativeMethods.FILE_ANY_ACCESS))
         {
            int lastError;

            using (var safeBuffer = GetDeviceIoData<NativeMethods.STORAGE_DEVICE_NUMBER>(safeHandle, NativeMethods.IoControlCode.IOCTL_STORAGE_GET_DEVICE_NUMBER, localDevicePath, out lastError))
            {
               if (null != safeBuffer)
               {
                  var storageDeviceInfo = new StorageDeviceInfo(safeBuffer.PtrToStructure<NativeMethods.STORAGE_DEVICE_NUMBER>());

                  if (!retry && deviceNumber > -1 && deviceNumber != storageDeviceInfo.DeviceNumber)
                     return null;

                  SetStorageDeviceInfoData(isElevated, isDevice, safeHandle, localDevicePath, storageDeviceInfo);

                  return storageDeviceInfo;
               }


               // A logical drive path like \\.\D: fails on a dynamic disk.

               if (!retry && !isDevice && lastError == Win32Errors.ERROR_INVALID_FUNCTION)
               {
                  var volDiskExtents = GetVolumeDiskExtents(safeHandle, localDevicePath);

                  if (null != volDiskExtents && volDiskExtents.Value.NumberOfDiskExtents > 0)
                  {
                     // Use the first disk extent.
                     deviceNumber = (int) volDiskExtents.Value.Extents[0].DiskNumber;

                     localDevicePath = Path.PhysicalDrivePrefix + deviceNumber.ToString(CultureInfo.InvariantCulture);

                     retry = true;
                     goto Retry;
                  }
               }
            }

            return null;
         }
      }


      [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
      [SecurityCritical]
      private static void SetStorageDeviceInfoData(bool isElevated, bool isDevice, SafeFileHandle safeHandle, string localDevicePath, StorageDeviceInfo storageDeviceInfo)
      {
         var storagePropertyQuery = new NativeMethods.STORAGE_PROPERTY_QUERY
         {
            PropertyId = NativeMethods.STORAGE_PROPERTY_ID.StorageDeviceProperty,
            QueryType = NativeMethods.STORAGE_QUERY_TYPE.PropertyStandardQuery
         };
         

         using (var safeBuffer = InvokeDeviceIoData(safeHandle, NativeMethods.IoControlCode.IOCTL_STORAGE_QUERY_PROPERTY, storagePropertyQuery, localDevicePath, Filesystem.NativeMethods.DefaultFileBufferSize / 2))
         {
            var deviceDescriptor = safeBuffer.PtrToStructure<NativeMethods.STORAGE_DEVICE_DESCRIPTOR>();


            storageDeviceInfo.BusType = (StorageBusType) deviceDescriptor.BusType;

            storageDeviceInfo.CommandQueueing = deviceDescriptor.CommandQueueing;

            storageDeviceInfo.ProductId = safeBuffer.PtrToStringAnsi((int) deviceDescriptor.ProductIdOffset).Trim();

            storageDeviceInfo.ProductRevision = safeBuffer.PtrToStringAnsi((int) deviceDescriptor.ProductRevisionOffset).Trim();

            storageDeviceInfo.RemovableMedia = deviceDescriptor.RemovableMedia;

            storageDeviceInfo.SerialNumber = safeBuffer.PtrToStringAnsi((int) deviceDescriptor.SerialNumberOffset).Trim();

            storageDeviceInfo.VendorId = safeBuffer.PtrToStringAnsi((int) deviceDescriptor.VendorIdOffset).Trim();


            if (Utils.IsNullOrWhiteSpace(storageDeviceInfo.ProductRevision) || storageDeviceInfo.ProductRevision.Length == 1)
               storageDeviceInfo.ProductRevision = null;

            if (Utils.IsNullOrWhiteSpace(storageDeviceInfo.SerialNumber) || storageDeviceInfo.SerialNumber.Length == 1)
               storageDeviceInfo.SerialNumber = null;
            
            if (Utils.IsNullOrWhiteSpace(storageDeviceInfo.VendorId) || storageDeviceInfo.VendorId.Length == 1)
               storageDeviceInfo.VendorId = null;
         }


         // Get disk total size.

         var lastError = 0;

         if (isElevated)
         {
            using (var safeBuffer = GetDeviceIoData<long>(safeHandle,NativeMethods.IoControlCode.IOCTL_DISK_GET_LENGTH_INFO, localDevicePath, out lastError))

               storageDeviceInfo.TotalSize = null != safeBuffer ? safeBuffer.ReadInt64() : 0;
         }


         // A logical drive path like \\.\D: fails on a dynamic disk.

         else if (!isDevice || lastError == Win32Errors.ERROR_INVALID_FUNCTION)

            storageDeviceInfo.TotalSize = new DiskSpaceInfo(localDevicePath, false, true, true).TotalNumberOfBytes;
      }
   }
}
