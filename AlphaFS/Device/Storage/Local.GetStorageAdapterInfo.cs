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
using System.Security;
using Alphaleonis.Win32.Filesystem;

namespace Alphaleonis.Win32.Device
{
   public static partial class Local
   {
      /// <summary>[AlphaFS] Retrieves the storage adapter of the device that is related to the logical drive name, volume <see cref="Guid"/> or <see cref="DeviceInfo.DevicePath"/>.</summary>
      /// <returns>Returns a <see cref="StorageAdapterInfo"/> instance that represent the adapter information of the storage device that is related to <paramref name="devicePath"/>.</returns>
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
      public static StorageAdapterInfo GetStorageAdapterInfo(string devicePath)
      {
         return GetStorageAdapterInfoCore(devicePath);
      }


      /// <summary>[AlphaFS] Retrieves the storage adapter of the device that is related to the logical drive name, volume <see cref="Guid"/> or <see cref="DeviceInfo.DevicePath"/>.</summary>
      /// <returns>Returns a <see cref="StorageDeviceInfo"/> instance that represent the storage device that is related to <paramref name="devicePath"/>.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="Exception"/>
      /// <param name="devicePath">
      ///    <para>A disk path such as: <c>\\.\PhysicalDrive0</c></para>
      ///    <para>A drive path such as: <c>C</c>, <c>C:</c> or <c>C:\</c></para>
      ///    <para>A volume <see cref="Guid"/> such as: <c>\\?\Volume{xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}\</c></para>
      ///    <para>A <see cref="DeviceInfo.DevicePath"/> string such as: <c>\\?\scsi#disk...{xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}</c></para>
      /// </param>
      /// <param name="deviceInfo">An initialized <see cref="DeviceInfo"/> instance.</param>
      [SecurityCritical]
      private static StorageAdapterInfo GetStorageAdapterInfoCore(string devicePath, DeviceInfo deviceInfo = null)
      {
         bool isDevice;
         bool isDrive;
         bool isVolume;

         var localDevicePath = FileSystemHelper.GetValidatedDevicePath(devicePath, out isDrive, out isVolume, out isDevice);

         if (isDrive)
            localDevicePath = FileSystemHelper.GetLocalDevicePath(localDevicePath);


         // The StorageDeviceInfo is always needed because it contains the device- and partition number.

         var storageDeviceInfo = GetStorageDeviceInfoCore(false, -1, localDevicePath);

         //if (null == storageDeviceInfo)
         //   return null;


         var deviceNumber = null != storageDeviceInfo ? storageDeviceInfo.DeviceNumber : -1;
         
         var storagePropertyQuery = new NativeMethods.STORAGE_PROPERTY_QUERY
         {
            PropertyId = NativeMethods.STORAGE_PROPERTY_ID.StorageAdapterProperty,
            QueryType = NativeMethods.STORAGE_QUERY_TYPE.PropertyStandardQuery
         };

         var retry = false;


      Retry:

         using (var safeHandle = FileSystemHelper.OpenPhysicalDisk(localDevicePath, NativeMethods.FILE_ANY_ACCESS))
         {
            if (!retry && !isDevice)
            {
               var volDiskExtents = GetVolumeDiskExtents(safeHandle, localDevicePath);

               if (null == volDiskExtents || volDiskExtents.Value.NumberOfDiskExtents == 0)
                  return null;

               // Use the first disk extent.
               deviceNumber = (int) volDiskExtents.Value.Extents[0].DiskNumber;
            }


            int lastError;

            using (var safeBuffer = InvokeDeviceIoData(safeHandle, NativeMethods.IoControlCode.IOCTL_STORAGE_QUERY_PROPERTY, storagePropertyQuery, localDevicePath, out lastError, Filesystem.NativeMethods.DefaultFileBufferSize / 8))
            {
               if (null != safeBuffer)
               {
                  var storageAdapterInfo = new StorageAdapterInfo(safeBuffer.PtrToStructure<NativeMethods.STORAGE_ADAPTER_DESCRIPTOR>());

                  if (null != deviceInfo)
                     storageAdapterInfo.BusReportedDeviceDescription = deviceInfo.BusReportedDeviceDescription;

                  return storageAdapterInfo;
               }


               // A logical drive path like \\.\D: fails on a dynamic disk.

               if (!retry && lastError == Win32Errors.ERROR_INVALID_FUNCTION)
               {
                  localDevicePath = Path.PhysicalDrivePrefix + deviceNumber.ToString(CultureInfo.InvariantCulture);

                  retry = true;
                  goto Retry;
               }

               return null;
            }
         }
      }
   }
}
