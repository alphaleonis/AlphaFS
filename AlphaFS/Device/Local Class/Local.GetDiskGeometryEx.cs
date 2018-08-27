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
      /// <summary>Returns information about the physical disk's geometry (media type, number of cylinders, tracks per cylinder, sectors per track, and bytes per sector).</summary>
      [SecurityCritical]
      private static NativeMethods.DISK_GEOMETRY_EX GetDiskGeometryEx(SafeFileHandle safeFileHandle, string pathForException)
      {
         var bufferSize = 128;

         while (true)
            using (var safeBuffer = new SafeGlobalMemoryBufferHandle(bufferSize))
            {
               var success = NativeMethods.DeviceIoControl(safeFileHandle, NativeMethods.IoControlCode.IOCTL_DISK_GET_DRIVE_GEOMETRY_EX, IntPtr.Zero, 0, safeBuffer, (uint) safeBuffer.Capacity, IntPtr.Zero, IntPtr.Zero);

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


                  var offset = (uint) sizeOf + sizeof(long); // 32

                  diskGeometryEx.PartitionInformation = safeBuffer.PtrToStructure<NativeMethods.DISK_PARTITION_INFO>((int) offset);


                  //// Intermittently throws: System.AccessViolationException: Attempted to read or write protected memory.
                  //// Observed when mounting an .iso file.

                  //offset += diskGeometryEx.PartitionInformation.SizeOfPartitionInfo;

                  //diskGeometryEx.DiskDetectionInfo = safeBuffer.PtrToStructure<NativeMethods.DISK_DETECTION_INFO>((int) offset);

                  return diskGeometryEx;
               }


               if (lastError == Win32Errors.ERROR_NOT_READY ||

                   // A logical drive path like "\\.\D:" fails on a dynamic disk.
                   lastError == Win32Errors.ERROR_INVALID_FUNCTION ||

                   // Request device number from a DeviceGuid.Image device.
                   lastError == Win32Errors.ERROR_NOT_SUPPORTED)

                  return new NativeMethods.DISK_GEOMETRY_EX();


               bufferSize = Utils.GetDoubledBufferSizeOrThrowException(safeBuffer, lastError, bufferSize, pathForException);
            }
      }
   }
}
