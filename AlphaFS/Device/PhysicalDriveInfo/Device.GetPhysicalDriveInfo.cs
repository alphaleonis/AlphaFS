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
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.AccessControl;

namespace Alphaleonis.Win32.Filesystem
{
   public static partial class Device
   {
      /// <summary> [AlphaFS] Retrieves the physical drive on the Computer that is related to the logical drive name, volume GUID or <see cref="DeviceInfo.DevicePath"/>.
      /// <para>Do not call this method for every volume or logical drive on the system as each call queries all physical drives and associated volumes and logical drives.</para>
      /// <para>Instead, use method <see cref="EnumeratePhysicalDrives"/> and property <see cref="PhysicalDriveInfo.VolumeGuids"/> or <see cref="PhysicalDriveInfo.LogicalDrives"/>.</para>
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
         string logicalDrive;

         devicePath = ValidateDevicePath(devicePath, out isDrive, out isVolume, out isDeviceInfo);

         var storageInfo = GetStorageDeviceInfoCore(devicePath, false);

         if (null == storageInfo)
            return null;


         if (isDrive)
         {
            GetDevicePath(devicePath, out logicalDrive);
            devicePath = logicalDrive;
         }


         var pDriveInfo = isDeviceInfo

            ? EnumeratePhysicalDrives().FirstOrDefault(pDrive => pDrive.StorageDeviceInfo.DeviceNumber == storageInfo.DeviceNumber && pDrive.StorageDeviceInfo.PartitionNumber == storageInfo.PartitionNumber)

            : isVolume
               ? EnumeratePhysicalDrives().FirstOrDefault(pDrive => null != pDrive.VolumeGuids && pDrive.VolumeGuids.Contains(devicePath, StringComparer.OrdinalIgnoreCase))

               : isDrive
                  ? EnumeratePhysicalDrives().FirstOrDefault(pDrive => null != pDrive.LogicalDrives && pDrive.LogicalDrives.Contains(devicePath, StringComparer.OrdinalIgnoreCase))
                  : null;


         if (null != pDriveInfo)
            pDriveInfo.StorageDeviceInfo.PartitionNumber = storageInfo.PartitionNumber;


         return pDriveInfo;
      }


      ///  <summary>[AlphaFS] Gets the physical drive information such as DeviceType, DeviceNumber, PartitionNumber and the serial number and Product ID.</summary>
      ///  <returns>A <see cref="PhysicalDriveInfo"/> instance that represents the physical drive on the Computer or <c>null</c> on error/no data available.</returns>
      ///  <exception cref="ArgumentException"/>
      ///  <exception cref="ArgumentNullException"/>
      ///  <exception cref="NotSupportedException"/>
      ///  <exception cref="Exception"/>
      /// <param name="storageInfo">A <see cref="StorageDeviceInfo"/> instance.</param>
      /// <param name="devicePath">
      ///    A drive path such as: "C", "C:" or "C:\".
      ///    A volume <see cref="Guid"/> such as: "\\?\Volume{xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}\".
      ///    A <see cref="DeviceInfo.DevicePath"/> string (when <see cref="DeviceInfo.ClassGuid"/> is set to <see cref="DeviceGuid.Disk"/>).
      /// </param>
      /// <param name="deviceInfo">A <see cref="DeviceInfo"/> instance.</param>
      /// <param name="getDeviceData"></param>
      /// <param name="getAllData"></param>
      /// <remarks>Use either <paramref name="devicePath"/> or <paramref name="deviceInfo"/>, not both.</remarks>
      [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Object is disposed.")]
      [SecurityCritical]
      internal static PhysicalDriveInfo GetPhysicalDriveInfoCore(StorageDeviceInfo storageInfo, string devicePath, DeviceInfo deviceInfo, bool? getAllData)
      {
         var isDeviceInfo = null != deviceInfo && !Utils.IsNullOrWhiteSpace(deviceInfo.DevicePath);

         if (isDeviceInfo)
            devicePath = deviceInfo.DevicePath;


         if (null == storageInfo)
            storageInfo = GetStorageDeviceInfoCore(devicePath, false);

         if (null == storageInfo)
            return null;


         var pDriveInfo = new PhysicalDriveInfo(storageInfo)
         {
            DeviceDescription = isDeviceInfo ? deviceInfo.DeviceDescription : null,

            DevicePath = devicePath,

            // "FriendlyName" usually contains a more complete name, as seen in Windows Explorer.
            Name = isDeviceInfo ? deviceInfo.FriendlyName : null,
         };
         

         if (null != getAllData)
         {
            try
            {
               // Requires elevation.
               PopulatePhysicalDriveInfo((bool) getAllData, devicePath, pDriveInfo);
            }
            catch { }
         }


         return pDriveInfo;
      }
      

      /// <summary>Sets the physical drive properties such as FriendlyName, device size and serial number. Requires elevation.</summary>
      private static void PopulatePhysicalDriveInfo(bool getAllData, string devicePath, PhysicalDriveInfo physicalDriveInfo)
      {
         using (var safeHandle = OpenPhysicalDrive(devicePath, FileSystemRights.Read | FileSystemRights.Write))
         {
            var storagePropertyQuery = new NativeMethods.STORAGE_PROPERTY_QUERY
            {
               PropertyId = 0, // StorageDeviceProperty, from STORAGE_PROPERTY_ID enum.
               QueryType = 0   // PropertyStandardQuery, from STORAGE_QUERY_TYPE enum
            };


            using (var safeBuffer = InvokeDeviceIoData(safeHandle, NativeMethods.IoControlCode.IOCTL_STORAGE_QUERY_PROPERTY, storagePropertyQuery, devicePath, NativeMethods.DefaultFileBufferSize / 2))
            {
               var storageDescriptor = safeBuffer.PtrToStructure<NativeMethods.STORAGE_DEVICE_DESCRIPTOR>(0);


               physicalDriveInfo.StorageDeviceInfo.BusType = (StorageBusType) storageDescriptor.BusType;

               if (!getAllData)
                  return;


               physicalDriveInfo.CommandQueueing = storageDescriptor.CommandQueueing;

               physicalDriveInfo.ProductRevision = safeBuffer.PtrToStringAnsi((int) storageDescriptor.ProductRevisionOffset).Trim();

               physicalDriveInfo.RemovableMedia = storageDescriptor.RemovableMedia;


               if (Utils.IsNullOrWhiteSpace(physicalDriveInfo.Name))
                  physicalDriveInfo.Name = safeBuffer.PtrToStringAnsi((int) storageDescriptor.ProductIdOffset).Trim();
               
               
               // Get the device hardware serial number.

               physicalDriveInfo.SerialNumber = safeBuffer.PtrToStringAnsi((int) storageDescriptor.SerialNumberOffset).Trim();
            }


            // Get the device size.

            using (var safeBuffer = GetDeviceIoData<long>(safeHandle, NativeMethods.IoControlCode.IOCTL_DISK_GET_LENGTH_INFO, devicePath))

               physicalDriveInfo.TotalSize = null != safeBuffer ? safeBuffer.ReadInt64() : 0;
         }
      }
   }
}
