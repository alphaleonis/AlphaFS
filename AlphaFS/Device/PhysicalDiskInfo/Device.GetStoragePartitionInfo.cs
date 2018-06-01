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
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using Microsoft.Win32.SafeHandles;

namespace Alphaleonis.Win32.Filesystem
{
   public static partial class Device
   {

      public static StoragePartitionInfo GetStoragePartitionInfo(string devicePath)
      {
         return GetStoragePartitionInfoCore(devicePath);
      }




      internal static StoragePartitionInfo GetStoragePartitionInfoCore(string devicePath)
      {
         string logicalDrive;

         var pathToDevice = GetDevicePath(devicePath, out logicalDrive);

         if (Utils.IsNullOrWhiteSpace(pathToDevice))
            return null;


         using (var safeHandle = OpenPhysicalDisk(pathToDevice, NativeMethods.FILE_ANY_ACCESS))

            return GetStoragePartitionInfoNative(safeHandle, devicePath);
      }


      /// <summary>Retrieves information about the number of partitions on a disk and the features of each partition.</summary>
      [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
      private static StoragePartitionInfo GetStoragePartitionInfoNative(SafeFileHandle safeHandle, string pathToDevice)
      {
         SafeFileHandle safeHandleRetry = null;
         var isRetry = false;


      StartGetData:

         // Get storage partition info.

         using (var safeBuffer = InvokeDeviceIoData(isRetry ? safeHandleRetry : safeHandle, NativeMethods.IoControlCode.IOCTL_DISK_GET_DRIVE_LAYOUT_EX, 0, pathToDevice, NativeMethods.DefaultFileBufferSize / 8))
         {
            if (null == safeBuffer)
            {
               // Assumption through observation: devicePath is a logical drive that points to a Dynamic disk.


               var volDiskExtents = GetVolumeDiskExtents(isRetry ? safeHandleRetry : safeHandle, pathToDevice);

               if (volDiskExtents.HasValue)
               {
                  // Use the first disk extent.

                  pathToDevice = string.Format(CultureInfo.InvariantCulture, "{0}{1}", Path.PhysicalDrivePrefix, volDiskExtents.Value.Extents[0].DiskNumber.ToString(CultureInfo.InvariantCulture));


                  safeHandleRetry = OpenPhysicalDisk(pathToDevice, FileSystemRights.Read);

                  isRetry = null != safeHandleRetry && !safeHandleRetry.IsClosed && !safeHandleRetry.IsInvalid;
               }


               if (isRetry)
                  goto StartGetData;

               return null;
            }


            if (isRetry && !safeHandleRetry.IsClosed)
               safeHandleRetry.Close();


            var driveStructureSize = Marshal.SizeOf(typeof(NativeMethods.DRIVE_LAYOUT_INFORMATION_EX));  // 48
            var partitionStructureSize = Marshal.SizeOf(typeof(NativeMethods.PARTITION_INFORMATION_EX)); // 144

            var drive = safeBuffer.PtrToStructure<NativeMethods.DRIVE_LAYOUT_INFORMATION_EX>();

            var partitions = new NativeMethods.PARTITION_INFORMATION_EX[drive.PartitionCount];


            for (var i = 0; i <= drive.PartitionCount - 1; i++)

               partitions[i] = safeBuffer.PtrToStructure<NativeMethods.PARTITION_INFORMATION_EX>(driveStructureSize + i * partitionStructureSize);


            var disk = GetDiskGeometryExNative(safeHandle, pathToDevice);

            return new StoragePartitionInfo(disk, drive, partitions);
         }
      }
      


      private static StoragePartitionInfo GetStoragePartitionInfoNative0(SafeFileHandle safeHandle, string pathForException)
      {
         var driveStructureSize = Marshal.SizeOf(typeof(NativeMethods.DRIVE_LAYOUT_INFORMATION_EX));  // 48
         var partitionStructureSize = Marshal.SizeOf(typeof(NativeMethods.PARTITION_INFORMATION_EX)); // 144
         var partitionCount = 1;

         var bufferSize = driveStructureSize + partitionCount * partitionStructureSize;

         while (true)
            using (var safeBuffer = new SafeGlobalMemoryBufferHandle(bufferSize))
            {
               var success = NativeMethods.DeviceIoControl(safeHandle, NativeMethods.IoControlCode.IOCTL_DISK_GET_DRIVE_LAYOUT_EX, IntPtr.Zero, 0, safeBuffer, (uint) safeBuffer.Capacity, IntPtr.Zero, IntPtr.Zero);

               var lastError = Marshal.GetLastWin32Error();


               if (success)
               {
                  var drive = safeBuffer.PtrToStructure<NativeMethods.DRIVE_LAYOUT_INFORMATION_EX>();

                  var partitions = new NativeMethods.PARTITION_INFORMATION_EX[drive.PartitionCount];


                  for (partitionCount = 0; partitionCount <= drive.PartitionCount - 1; partitionCount++)

                     partitions[partitionCount] = safeBuffer.PtrToStructure<NativeMethods.PARTITION_INFORMATION_EX>(driveStructureSize + partitionCount * partitionStructureSize);


                  var disk = GetDiskGeometryExNative(safeHandle, pathForException);

                  return new StoragePartitionInfo(disk, drive, partitions);
               }


               if (lastError == Win32Errors.ERROR_NOT_READY ||

                   // Dynamic disk.
                   lastError == Win32Errors.ERROR_INVALID_FUNCTION ||

                   // Request device number from a DeviceGuid.Image device.
                   lastError == Win32Errors.ERROR_NOT_SUPPORTED)

                  return null;


               bufferSize = GetDoubledBufferSizeOrThrowException(safeBuffer, lastError, bufferSize, pathForException);
            }
      }


      /// <summary>Returns information about the physical disk's geometry (media type, number of cylinders, tracks per cylinder, sectors per track, and bytes per sector).</summary>
      private static NativeMethods.DISK_GEOMETRY_EX GetDiskGeometryExNative(SafeFileHandle safeHandle, string pathForException)
      {
         var bufferSize = 128;

         while (true)
            using (var safeBuffer = new SafeGlobalMemoryBufferHandle(bufferSize))
            {
               var success = NativeMethods.DeviceIoControl(safeHandle, NativeMethods.IoControlCode.IOCTL_DISK_GET_DRIVE_GEOMETRY_EX, IntPtr.Zero, 0, safeBuffer, (uint) safeBuffer.Capacity, IntPtr.Zero, IntPtr.Zero);

               var lastError = Marshal.GetLastWin32Error();


               if (success)
               {
                  var typeOf = typeof(NativeMethods.DISK_GEOMETRY);
                  var sizeOf = Marshal.SizeOf(typeOf);

                  var diskGeometryEx = new NativeMethods.DISK_GEOMETRY_EX
                  {
                     Geometry = safeBuffer.PtrToStructure<NativeMethods.DISK_GEOMETRY>(),

                     DiskSize = safeBuffer.ReadInt64(sizeOf)
                  };


                  var offset = (uint) sizeOf + sizeof(long); // 32

                  diskGeometryEx.PartitionInformation = safeBuffer.PtrToStructure<NativeMethods.DISK_PARTITION_INFO>((int) offset);


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


               bufferSize = GetDoubledBufferSizeOrThrowException(safeBuffer, lastError, bufferSize, pathForException);
            }
      }
   }
}
