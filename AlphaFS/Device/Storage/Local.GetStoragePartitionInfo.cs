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
using System.Runtime.InteropServices;
using System.Security;
using System.Security.AccessControl;
using Alphaleonis.Win32.Filesystem;
using Alphaleonis.Win32.Security;
using Microsoft.Win32.SafeHandles;

namespace Alphaleonis.Win32.Device
{
   public static partial class Local
   {
      /// <summary>[AlphaFS] Retrieves information about the partitions on a disk and the features of each partition.</summary>
      /// <returns>Returns a <see cref="StoragePartitionInfo"/> instance that represent the partition info that is related to <paramref name="devicePath"/>.</returns>
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
      public static StoragePartitionInfo GetStoragePartitionInfo(string devicePath)
      {
         return GetStoragePartitionInfoCore(ProcessContext.IsElevatedProcess, -1, devicePath);
      }


      /// <summary>[AlphaFS] Retrieves information about the partitions on a disk and the features of each partition.</summary>
      /// <returns>Returns a <see cref="StoragePartitionInfo"/> instance that represent the partition info that is related to <paramref name="devicePath"/>.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="Exception"/>
      /// <param name="isElevated"><c>true</c> indicates the current process is in an elevated state, allowing to retrieve more data.</param>
      /// <param name="devicePath">
      /// <para>A disk path such as: <c>\\.\PhysicalDrive0</c></para>
      /// <para>A drive path such as: <c>C</c>, <c>C:</c> or <c>C:\</c></para>
      /// <para>A volume <see cref="Guid"/> such as: <c>\\?\Volume{xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}\</c></para>
      /// <para>A <see cref="DeviceInfo.DevicePath"/> string such as: <c>\\?\scsi#disk...{xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}</c></para>
      /// </param>
      [SecurityCritical]
      public static StoragePartitionInfo GetStoragePartitionInfo(bool isElevated, string devicePath)
      {
         return GetStoragePartitionInfoCore(isElevated, -1, devicePath);
      }




      /// <summary>[AlphaFS] Retrieves information about the partitions on a disk and the features of each partition.</summary>
      /// <returns>Returns a <see cref="StoragePartitionInfo"/> instance that represent the partition info that is related to <paramref name="devicePath"/>.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="Exception"/>
      /// <param name="isElevated"><c>true</c> indicates the current process is in an elevated state, allowing to retrieve more data.</param>
      /// <param name="deviceNumber">Retrieve a <see cref="PhysicalDiskInfo"/> instance by device number.</param>
      /// <param name="devicePath">
      /// <para>A disk path such as: <c>\\.\PhysicalDrive0</c></para>
      /// <para>A drive path such as: <c>C</c>, <c>C:</c> or <c>C:\</c></para>
      /// <para>A volume <see cref="Guid"/> such as: <c>\\?\Volume{xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}\</c></para>
      /// <para>A <see cref="DeviceInfo.DevicePath"/> string such as: <c>\\?\scsi#disk...{xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}</c></para>
      /// </param>
      [SecurityCritical]
      internal static StoragePartitionInfo GetStoragePartitionInfoCore(bool isElevated, int deviceNumber, string devicePath)
      {
         var isDeviceNumber = deviceNumber > -1;
         var isDrive = false;
         bool isVolume;
         bool isDevice;

         var localDevicePath = isDeviceNumber ? Path.PhysicalDrivePrefix + deviceNumber.ToString(CultureInfo.InvariantCulture) : FileSystemHelper.GetValidatedDevicePath(devicePath, out isDrive, out isVolume, out isDevice);

         if (isDrive)
            localDevicePath = FileSystemHelper.GetLocalDevicePath(localDevicePath);


         using (var safeHandle = FileSystemHelper.OpenPhysicalDisk(localDevicePath, isElevated ? FileSystemRights.Read : NativeMethods.FILE_ANY_ACCESS))

            return GetStoragePartitionInfoNative(safeHandle, localDevicePath);
      }


      [SecurityCritical]
      private static StoragePartitionInfo GetStoragePartitionInfoNative(SafeFileHandle safeHandle, string pathForException)
      {
         var volDiskExtents = GetVolumeDiskExtents(safeHandle, pathForException);

         if (null == volDiskExtents || volDiskExtents.Value.NumberOfDiskExtents == 0)
            return null;


         using (var safeBuffer = InvokeDeviceIoData(safeHandle, NativeMethods.IoControlCode.IOCTL_DISK_GET_DRIVE_LAYOUT_EX, 0, pathForException, Filesystem.NativeMethods.DefaultFileBufferSize / 4))
            if (null != safeBuffer)
            {
               var layout = safeBuffer.PtrToStructure<NativeMethods.DRIVE_LAYOUT_INFORMATION_EX>();

               // Sanity check.
               if (layout.PartitionCount <= 256)
               {
                  var driveStructureSize = Marshal.SizeOf(typeof(NativeMethods.DRIVE_LAYOUT_INFORMATION_EX)); // 48

                  var partitionStructureSize = Marshal.SizeOf(typeof(NativeMethods.PARTITION_INFORMATION_EX)); // 144

                  var partitions = new NativeMethods.PARTITION_INFORMATION_EX[layout.PartitionCount];


                  for (var i = 0; i <= layout.PartitionCount - 1; i++)

                     partitions[i] = safeBuffer.PtrToStructure<NativeMethods.PARTITION_INFORMATION_EX>(driveStructureSize + i * partitionStructureSize);


                  var disk = GetDiskGeometryExNative(safeHandle, pathForException);


                  // Use the first disk extent.
                  var diskNumber = volDiskExtents.Value.Extents[0].DiskNumber;

                  return new StoragePartitionInfo((int) diskNumber, disk, layout, partitions);
               }
            }

         return null;
      }


      /// <summary>Returns information about the physical disk's geometry (media type, number of cylinders, tracks per cylinder, sectors per track, and bytes per sector).</summary>
      [SecurityCritical]
      private static NativeMethods.DISK_GEOMETRY_EX GetDiskGeometryExNative(SafeFileHandle safeHandle, string pathForException)
      {
         var bufferSize = 128;

         while (true)
            using (var safeBuffer = new SafeGlobalMemoryBufferHandle(bufferSize))
            {
               var success = NativeMethods.DeviceIoControl(safeHandle, NativeMethods.IoControlCode.IOCTL_DISK_GET_DRIVE_GEOMETRY_EX, IntPtr.Zero, 0, safeBuffer, (uint)safeBuffer.Capacity, IntPtr.Zero, IntPtr.Zero);

               var lastError = Marshal.GetLastWin32Error();


               if (success)
               {
                  var typeOf = typeof(NativeMethods.DISK_GEOMETRY);
                  var sizeOf = Marshal.SizeOf(typeOf); // 24

                  var diskGeometryEx = new NativeMethods.DISK_GEOMETRY_EX
                  {
                     Geometry = safeBuffer.PtrToStructure<NativeMethods.DISK_GEOMETRY>(),

                     DiskSize = safeBuffer.ReadInt64(sizeOf)
                  };


                  var offset = (uint)sizeOf + sizeof(long); // 32

                  diskGeometryEx.PartitionInformation = safeBuffer.PtrToStructure<NativeMethods.DISK_PARTITION_INFO>((int)offset);


                  //// Intermittently throws: System.AccessViolationException: Attempted to read or write protected memory.
                  //// Observed when mounting an .iso file.

                  //offset += diskGeometryEx.PartitionInformation.SizeOfPartitionInfo;

                  //diskGeometryEx.DiskDetectionInfo = safeBuffer.PtrToStructure<NativeMethods.DISK_DETECTION_INFO>((int) offset);

                  return diskGeometryEx;
               }


               if (lastError == Win32Errors.ERROR_NOT_READY ||

                   // Dynamic disk.
                   lastError == Win32Errors.ERROR_INVALID_FUNCTION ||

                   // Request device number from a DeviceGuid.Image device.
                   lastError == Win32Errors.ERROR_NOT_SUPPORTED)

                  return new NativeMethods.DISK_GEOMETRY_EX();


               bufferSize = Utils.GetDoubledBufferSizeOrThrowException(safeBuffer, lastError, bufferSize, pathForException);
            }
      }
   }
}
