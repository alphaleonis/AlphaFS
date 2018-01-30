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
using System.Runtime.InteropServices;
using System.Security;
using System.Security.AccessControl;
using Microsoft.Win32.SafeHandles;

namespace Alphaleonis.Win32.Filesystem
{
   public static partial class Device
   {
      /// <summary>[AlphaFS] Gets the hardware information such as the serial number and Product ID.</summary>
      /// <returns>A <see cref="PhysicalDriveInfo"/> instance that represents the physical drive on the Computer or <c>null</c> on error.</returns>      
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="Exception"/>
      /// <param name="driveName">
      ///   A valid drive path or drive letter suc as: "C:", "D:\".
      ///   A <see cref="DeviceInfo.DevicePath"/> string is also valid when <see cref="DeviceInfo.ClassGuid"/> is set to <see cref="DeviceGuid.Disk"/>.
      /// </param>
      [SecurityCritical]
      public static PhysicalDriveInfo GetPhysicalDriveInfo(string driveName)
      {
         return GetPhysicalDriveInfoCore(driveName, null);
      }


      /// <summary>[AlphaFS] Gets the hardware information such as the serial number and Product ID.</summary>
      /// <returns>A <see cref="PhysicalDriveInfo"/> instance that represents the physical drive on the Computer or <c>null</c> on error.</returns>      
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="Exception"/>
      /// <param name="driveName">
      ///   A valid drive path or drive letter suc as: "C:", "D:\".
      ///   A <see cref="DeviceInfo.DevicePath"/> string is also valid when <see cref="DeviceInfo.ClassGuid"/> is set to <see cref="DeviceGuid.Disk"/>.
      /// </param>
      /// <param name="deviceInfo">a <see cref="DeviceInfo"/> instance.</param>
      ///<remarks>Use either <paramref name="driveName"/> or <paramref name="deviceInfo"/>, not both.</remarks>
      [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Object is disposed.")]
      [SecurityCritical]
      internal static PhysicalDriveInfo GetPhysicalDriveInfoCore(string driveName, DeviceInfo deviceInfo)
      {
         // FileSystemRights desiredAccess: If this parameter is zero, the application can query certain metadata such as file, directory, or device attributes
         // without accessing that file or device, even if GENERIC_READ access would have been denied.
         // You cannot request an access mode that conflicts with the sharing mode that is specified by the dwShareMode parameter in an open request that already has an open handle.
         //const int desiredAccess = 0;

         // Requires elevation for: TotalNumberOfBytes
         const FileSystemRights desiredAccess = FileSystemRights.Read | FileSystemRights.Write;

         const bool elevatedAccess = (desiredAccess & FileSystemRights.Read) != 0 && (desiredAccess & FileSystemRights.Write) != 0;


         var pathToDevice = ValidateAndGetPathToDevice(driveName, deviceInfo);

         if (Utils.IsNullOrWhiteSpace(pathToDevice))
            return null;


         SafeFileHandle safeHandle;
         NativeMethods.STORAGE_DEVICE_NUMBER? storageDevice;


         // No elevation needed.

         using (safeHandle = OpenPhysicalDrive(pathToDevice, 0))
         {
            storageDevice = GetStorageDeviceDriveNumber(safeHandle, pathToDevice);

            if (null == storageDevice)
               return null;
         }


         var device = storageDevice.Value;
         var diskNumber = device.DeviceNumber;
         
         var physicalDrive = string.Format(CultureInfo.InvariantCulture, "{0}{1}", Path.PhysicalDrivePrefix, diskNumber.ToString(CultureInfo.InvariantCulture));
         
         var info = new PhysicalDriveInfo
         {
            DeviceNumber = diskNumber,
            PartitionNumber = device.PartitionNumber
         };



         //try
         //{
         //   GetVolumeDiskExtents(physicalDrive, desiredAccess);
         //}
         //catch (Exception ex)
         //{
         //   Console.WriteLine(ex.Message);
         //}


         using (safeHandle = OpenPhysicalDrive(physicalDrive, desiredAccess))
         {
            uint bytesReturned;

            var storagePropertyQuery = new NativeMethods.STORAGE_PROPERTY_QUERY
            {
               PropertyId = 0, // StorageDeviceProperty, from STORAGE_PROPERTY_ID enum.
               QueryType = 0   // PropertyStandardQuery, from STORAGE_QUERY_TYPE enum
            };


            using (var safeBuffer = InvokeDeviceIoData(safeHandle, NativeMethods.IoControlCode.IOCTL_STORAGE_QUERY_PROPERTY, storagePropertyQuery, physicalDrive, NativeMethods.DefaultFileBufferSize / 4))
            {
               var storageDescriptor = safeBuffer.PtrToStructure<NativeMethods.STORAGE_DEVICE_DESCRIPTOR>(0);


               info.BusType = (StorageBusType) storageDescriptor.BusType;

               info.IsRemovable = storageDescriptor.RemovableMedia;

               info.SupportsCommandQueueing = storageDescriptor.CommandQueueing;
               
               info.ProductRevision = safeBuffer.PtrToStringAnsi((int) storageDescriptor.ProductRevisionOffset).Trim();


               // "FriendlyName" usually contains a more complete name, as seen in Windows Explorer.

               info.Name = null != deviceInfo && !Utils.IsNullOrWhiteSpace(deviceInfo.FriendlyName)
                  ? deviceInfo.FriendlyName
                  : safeBuffer.PtrToStringAnsi((int) storageDescriptor.ProductIdOffset).Trim();


               //info.InstanceID = null != deviceInfo && !Utils.IsNullOrWhiteSpace(deviceInfo.InstanceID)
               //   ? deviceInfo.InstanceID
               //   : string.Empty;


               // Retrieve the device's hardware serial number.

               long serial;
               if (long.TryParse(safeBuffer.PtrToStringAnsi((int) storageDescriptor.SerialNumberOffset).Trim(), out serial))
                  info.SerialNumber = serial;
            }


            if (elevatedAccess)
            {
               long diskSize;

               var success = NativeMethods.DeviceIoControl5(safeHandle, NativeMethods.IoControlCode.IOCTL_DISK_GET_LENGTH_INFO, IntPtr.Zero, 0, out diskSize, (uint) Marshal.SizeOf(typeof(long)), out bytesReturned, IntPtr.Zero);

               var lastError = Marshal.GetLastWin32Error();
               if (!success)
                  NativeError.ThrowException(lastError, string.Format(CultureInfo.InvariantCulture, "Drive: {0}", physicalDrive));


               info.TotalSize = diskSize;
            }
         }


         // "FriendlyName" usually contains a more complete name, as seen in Windows Explorer.

         if (null == deviceInfo)
            SetDeviceFriendlyName(info);


         return info;
      }


      private static string ValidateAndGetPathToDevice(string driveName, DeviceInfo deviceInfo)
      {
         var hasDeviceInfo = null != deviceInfo;
         if (!hasDeviceInfo)
         {
            if (Utils.IsNullOrWhiteSpace(driveName))
               throw new ArgumentNullException("driveName");

            driveName = driveName.Length == 1 ? driveName + Path.VolumeSeparatorChar : Path.GetPathRoot(driveName, false);

            if (!Path.IsLogicalDriveCore(driveName, PathFormat.LongFullPath))
               throw new ArgumentException(Resources.Argument_must_be_a_drive_letter_from_a_z, "driveName");

            driveName = string.Format(CultureInfo.InvariantCulture, "{0}{1}", Path.LogicalDrivePrefix, driveName);
         }


         return Path.RemoveTrailingDirectorySeparator(hasDeviceInfo ? deviceInfo.DevicePath : driveName);
      }


      [SecurityCritical]
      private static void SetDeviceFriendlyName(PhysicalDriveInfo info)
      {
         //foreach (var deviceInfo in EnumerateDevicesCore(null, null, DeviceGuid.Disk))
         //{
         //   if (null != deviceInfo.InstanceID && deviceInfo.InstanceID.Equals(info.InstanceID, StringComparison.OrdinalIgnoreCase))
         //   {
         //      if (!Utils.IsNullOrWhiteSpace(deviceInfo.FriendlyName))
         //      {
         //         info.Name = deviceInfo.FriendlyName;
         //         return;
         //      }
         //   }
         //}
      }
   }
}
