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
using Microsoft.Win32.SafeHandles;

namespace Alphaleonis.Win32.Filesystem
{
   public static partial class Device
   {
      /// <summary>Retrieves the physical location of a specified volume on one or more disks.</summary>
      private static NativeMethods.VOLUME_DISK_EXTENTS? GetVolumeDiskExtents(SafeFileHandle safeHandle, string pathForException)
      {
         var structSize = Marshal.SizeOf(typeof(NativeMethods.VOLUME_DISK_EXTENTS));
         var numberOfExtents = 1;
         var bufferSize = structSize * numberOfExtents;


         while(true)
            using (var safeBuffer = new SafeGlobalMemoryBufferHandle(bufferSize))
            {
               var success = NativeMethods.DeviceIoControl(safeHandle, NativeMethods.IoControlCode.IOCTL_VOLUME_GET_VOLUME_DISK_EXTENTS, IntPtr.Zero, 0, safeBuffer, (uint) safeBuffer.Capacity, IntPtr.Zero, IntPtr.Zero);

               var lastError = Marshal.GetLastWin32Error();

               if (success)
               {
                  numberOfExtents = safeBuffer.ReadInt32();

                  var diskExtent = new NativeMethods.DISK_EXTENT[numberOfExtents];


                  for (int i = 0, itemOffset = 0; i < numberOfExtents - 1; i++, itemOffset = structSize * i)

                     diskExtent[i] = safeBuffer.PtrToStructure<NativeMethods.DISK_EXTENT>(itemOffset);


                  return new NativeMethods.VOLUME_DISK_EXTENTS {Extents = diskExtent};
               }

               
               bufferSize = GetDoubledBufferSizeOrThrowException(safeBuffer, lastError, bufferSize, pathForException);
            }
      }
   }
}
