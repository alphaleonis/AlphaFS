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
using System.Runtime.InteropServices;
using System.Security;
using Microsoft.Win32.SafeHandles;

namespace Alphaleonis.Win32.Device
{
   public static partial class Local
   {
      /// <summary>[AlphaFS] Retrieves information about the partitions on a disk and the features of each partition.</summary>
      /// <returns>Returns a <see cref="StoragePartitionInfo"/> instance that represent the partition info that is related to <paramref name="safeFileHandle"/>.</returns>
      /// <exception cref="Exception"/>
      [SecurityCritical]
      internal static StoragePartitionInfo GetStoragePartitionInfo(SafeFileHandle safeFileHandle, int deviceNumber, string localDevicePath)
      {
         using (var safeBuffer = InvokeDeviceIoData(safeFileHandle, NativeMethods.IoControlCode.IOCTL_DISK_GET_DRIVE_LAYOUT_EX, 0, localDevicePath, Filesystem.NativeMethods.DefaultFileBufferSize / 4))

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


                  var disk = GetDiskGeometryEx(safeFileHandle, localDevicePath);

                  return new StoragePartitionInfo(deviceNumber, disk, layout, partitions);
               }
            }

         return null;
      }
   }
}
