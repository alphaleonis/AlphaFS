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

namespace Alphaleonis.Win32.Filesystem
{
   public static partial class Device
   {
      /// <summary>[AlphaFS] Gets the physical drive information such as the serial number and Product ID.</summary>
      /// <returns>A <see cref="PhysicalDriveInfo"/> instance that represents the physical drive on the Computer or <c>null</c> on error.</returns>      
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="Exception"/>
      /// <param name="devicePath">
      ///   A drive path such as: "C:", "D:\".
      ///   A volume <see cref="Guid"/> path such as: "\\?\Volume{xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}\".
      ///   A <see cref="DeviceInfo.DevicePath"/> string (when <see cref="DeviceInfo.ClassGuid"/> is set to <see cref="DeviceGuid.Disk"/>).
      /// </param>
      [SecurityCritical]
      public static PhysicalDriveInfo GetPhysicalDriveInfo(string devicePath)
      {
         return GetPhysicalDriveInfoCore(devicePath, null);
      }




      ///  <summary>[AlphaFS] Gets the physical drive information such as the serial number and Product ID.</summary>
      ///  <returns>A <see cref="PhysicalDriveInfo"/> instance that represents the physical drive on the Computer or <c>null</c> on error.</returns>      
      ///  <exception cref="ArgumentException"/>
      ///  <exception cref="ArgumentNullException"/>
      ///  <exception cref="NotSupportedException"/>
      ///  <exception cref="Exception"/>
      /// <param name="devicePath">
      ///    A drive path such as: "C:", "D:\".
      ///    A volume <see cref="Guid"/> path such as: "\\?\Volume{xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}\".
      ///    A <see cref="DeviceInfo.DevicePath"/> string (when <see cref="DeviceInfo.ClassGuid"/> is set to <see cref="DeviceGuid.Disk"/>).
      /// </param>
      /// <param name="deviceInfo">a <see cref="DeviceInfo"/> instance.</param>
      /// <param name="getStorageData"></param>
      /// <remarks>Use either <paramref name="devicePath"/> or <paramref name="deviceInfo"/>, not both.</remarks>
      [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Object is disposed.")]
      [SecurityCritical]
      internal static PhysicalDriveInfo GetPhysicalDriveInfoCore(string devicePath, DeviceInfo deviceInfo, bool getStorageData = true)
      {
         var isDeviceInfo = null != deviceInfo && !Utils.IsNullOrWhiteSpace(deviceInfo.DevicePath);

         if (isDeviceInfo)
            devicePath = deviceInfo.DevicePath;


         string logicalDrive;
         var storageDevice = GetStorageDevice(devicePath, out logicalDrive);

         if (null == storageDevice)
            return null;


         var device = storageDevice.Value;
         var diskNumber = device.DeviceNumber;

         var physicalDriveInfo = new PhysicalDriveInfo
         {
            DevicePath = devicePath,
            DeviceNumber = diskNumber,
            PartitionNumber = device.PartitionNumber,

            // "FriendlyName" usually contains a more complete name, as seen in Windows Explorer.
            Name = isDeviceInfo ? deviceInfo.FriendlyName : null
         };


         if (getStorageData)
         {
            var physicalDrive = string.Format(CultureInfo.InvariantCulture, "{0}{1}", Path.PhysicalDrivePrefix, diskNumber.ToString(CultureInfo.InvariantCulture));

            SetStorageData(physicalDrive, physicalDriveInfo);
         }


         if (null != logicalDrive)
            GetVolumeDiskExtents(logicalDrive);


         return physicalDriveInfo;
      }


      private static void SetStorageData(string physicalDrive, PhysicalDriveInfo physicalDriveInfo)
      {
         // FileSystemRights desiredAccess: If this parameter is zero, the application can query certain metadata such as file, directory, or device attributes
         // without accessing that file or device, even if GENERIC_READ access would have been denied.
         // You cannot request an access mode that conflicts with the sharing mode that is specified by the dwShareMode parameter in an open request that already has an open handle.
         //const int desiredAccess = 0;

         // Requires elevation for.
         const FileSystemRights desiredAccess = FileSystemRights.Read | FileSystemRights.Write;

         //const bool elevatedAccess = (desiredAccess & FileSystemRights.Read) != 0 && (desiredAccess & FileSystemRights.Write) != 0;


         using (var safeHandle = OpenPhysicalDrive(physicalDrive, desiredAccess))
         {
            var storagePropertyQuery = new NativeMethods.STORAGE_PROPERTY_QUERY
            {
               PropertyId = 0, // StorageDeviceProperty, from STORAGE_PROPERTY_ID enum.
               QueryType = 0   // PropertyStandardQuery, from STORAGE_QUERY_TYPE enum
            };


            using (var safeBuffer = InvokeDeviceIoData(safeHandle, NativeMethods.IoControlCode.IOCTL_STORAGE_QUERY_PROPERTY, storagePropertyQuery, physicalDrive, NativeMethods.DefaultFileBufferSize / 4))
            {
               var storageDescriptor = safeBuffer.PtrToStructure<NativeMethods.STORAGE_DEVICE_DESCRIPTOR>(0);


               physicalDriveInfo.BusType = (StorageBusType) storageDescriptor.BusType;

               physicalDriveInfo.CommandQueueing = storageDescriptor.CommandQueueing;

               physicalDriveInfo.ProductRevision = safeBuffer.PtrToStringAnsi((int) storageDescriptor.ProductRevisionOffset).Trim();

               physicalDriveInfo.RemovableMedia = storageDescriptor.RemovableMedia;


               if (Utils.IsNullOrWhiteSpace(physicalDriveInfo.Name))

                  physicalDriveInfo.Name = safeBuffer.PtrToStringAnsi((int) storageDescriptor.ProductIdOffset).Trim();


               // Get the device hardware serial number.

               physicalDriveInfo.SerialNumber = safeBuffer.PtrToStringAnsi((int) storageDescriptor.SerialNumberOffset).Trim();
            }


            // Get the device size.

            using (var safeBuffer = new SafeGlobalMemoryBufferHandle(Marshal.SizeOf(typeof(long))))
            {
               uint bytesReturned;
               var success = NativeMethods.DeviceIoControl(safeHandle, NativeMethods.IoControlCode.IOCTL_DISK_GET_LENGTH_INFO, IntPtr.Zero, 0, safeBuffer, (uint) safeBuffer.Capacity, out bytesReturned, IntPtr.Zero);

               var lastError = Marshal.GetLastWin32Error();

               if (!success)
                  NativeError.ThrowException(lastError, string.Format(CultureInfo.InvariantCulture, "Drive: {0}", physicalDrive));


               physicalDriveInfo.TotalSize = safeBuffer.ReadInt64();
            }
         }
      }


      private static NativeMethods.STORAGE_DEVICE_NUMBER? GetStorageDevice(string devicePath, out string logicalDrive)
      {
         logicalDrive = null;

         var pathToDevice = ValidateAndGetPathToDevice(devicePath, out logicalDrive);

         if (Utils.IsNullOrWhiteSpace(pathToDevice))
            return null;


         // No elevation needed.

         using (var safeHandle = OpenPhysicalDrive(pathToDevice, 0))

         using (var safeBuffer = GetDeviceIoData<NativeMethods.STORAGE_DEVICE_NUMBER>(safeHandle, NativeMethods.IoControlCode.IOCTL_STORAGE_GET_DEVICE_NUMBER, pathToDevice))

            return null != safeBuffer ? safeBuffer.PtrToStructure<NativeMethods.STORAGE_DEVICE_NUMBER>(0) : (NativeMethods.STORAGE_DEVICE_NUMBER?)null;
      }


      private static string ValidateAndGetPathToDevice(string devicePath, out string logicalDrive)
      {
         if (Utils.IsNullOrWhiteSpace(devicePath))
            throw new ArgumentNullException("devicePath");


         // Resolve single drive letter or get root directory information.

         devicePath = devicePath.Length == 1 ? devicePath + Path.VolumeSeparatorChar : Path.GetPathRoot(devicePath, false);


         var isDrive = Path.IsLogicalDriveCore(devicePath, PathFormat.LongFullPath);

         var isVolume = devicePath.StartsWith(Path.VolumePrefix + "{", StringComparison.OrdinalIgnoreCase);

         var isDeviceInfo = !isDrive && !isVolume && devicePath.StartsWith(Path.LongPathPrefix, StringComparison.Ordinal);


         if (!isDrive && !isVolume && !isDeviceInfo)
            throw new ArgumentException(Resources.Argument_must_be_DriveLetter_or_VolumeGuid_or_DevicePath, "devicePath");


         devicePath = string.Format(CultureInfo.InvariantCulture, "{0}{1}", isDrive ? Path.LogicalDrivePrefix : string.Empty, Path.RemoveTrailingDirectorySeparator(devicePath));

         logicalDrive = isDrive ? devicePath : null;


         return devicePath;
      }
   }
}
