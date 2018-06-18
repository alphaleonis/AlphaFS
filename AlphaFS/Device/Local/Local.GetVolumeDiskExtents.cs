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
      /// <summary>Retrieves the physical location and disk number of a specified volume on one or more disks.</summary>
      [SecurityCritical]
      private static NativeMethods.VOLUME_DISK_EXTENTS? GetVolumeDiskExtents(SafeFileHandle safeFileHandle, string pathForException)
      {
         var structSize = Marshal.SizeOf(typeof(NativeMethods.DISK_EXTENT_SINGLE));
         var bufferSize = structSize;


         while(true)
            using (var safeBuffer = new SafeGlobalMemoryBufferHandle(bufferSize))
            {
               var success = NativeMethods.DeviceIoControl(safeFileHandle, NativeMethods.IoControlCode.IOCTL_VOLUME_GET_VOLUME_DISK_EXTENTS, IntPtr.Zero, 0, safeBuffer, (uint) safeBuffer.Capacity, IntPtr.Zero, IntPtr.Zero);

               var lastError = Marshal.GetLastWin32Error();

               if (success)
               {
                  var numberOfExtents = safeBuffer.ReadInt64();

                  var diskExtent = new NativeMethods.DISK_EXTENT[numberOfExtents];

                  for (int i = 0, itemOffset = 0; i < numberOfExtents; i++, itemOffset = structSize * i)
                  {
                     var single = safeBuffer.PtrToStructure<NativeMethods.DISK_EXTENT_SINGLE>(itemOffset);

                     diskExtent[i].DiskNumber = single.Extent.DiskNumber;
                     diskExtent[i].ExtentLength = single.Extent.ExtentLength;
                     diskExtent[i].StartingOffset = single.Extent.StartingOffset;
                  }

                  return new NativeMethods.VOLUME_DISK_EXTENTS
                  {
                     NumberOfDiskExtents = (uint) numberOfExtents,
                     Extents = diskExtent
                  };
               }


               // 2016-06-18 TODO: When lastError = ERROR_MORE_DATA, the drive is part of a mirror or volume, or the volume is on multiple disks.

               // Encountered a drive such as a CDRom/mounted .iso file.
               if (lastError == Win32Errors.ERROR_INVALID_FUNCTION)
                  return null;
               

               bufferSize = Utils.GetDoubledBufferSizeOrThrowException(safeBuffer, lastError, bufferSize, pathForException);
            }
      }
   }
}
