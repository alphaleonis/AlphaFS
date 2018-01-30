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
using System.Security;
using System.Security.AccessControl;
using Microsoft.Win32.SafeHandles;

namespace Alphaleonis.Win32.Filesystem
{
   public static partial class Device
   {
      /// <summary>
      /// 
      /// </summary>
      /// <param name="physicalDrive"></param>
      /// <param name="desiredAccess"></param>
      /// <returns></returns>
      [SecurityCritical]
      public static void GetVolumeDiskExtents(string physicalDrive, FileSystemRights desiredAccess)
      {
         var structure = GetVolumeDiskExtentsCore(physicalDrive, desiredAccess);

         //Console.WriteLine("{0}: NumberOfDiskExtents: {1}", physicalDrive, structure.NumberOfDiskExtents);
      }


      /// <summary>
      /// 
      /// </summary>
      /// <param name="safeHandle"></param>
      /// <param name="physicalDrive"></param>
      /// <param name="desiredAccess"></param>
      /// <returns></returns>
      [SecurityCritical]
      public static void GetVolumeDiskExtents(SafeFileHandle safeHandle, string physicalDrive, FileSystemRights desiredAccess)
      {

      }


      /// <summary>
      /// 
      /// </summary>
      /// <param name="physicalDrive"></param>
      /// <param name="desiredAccess"></param>
      /// <returns></returns>
      [SecurityCritical]
      private static NativeMethods.VOLUME_DISK_EXTENTS GetVolumeDiskExtentsCore(string physicalDrive, FileSystemRights desiredAccess)
      {
         using (var safeHandle = File.OpenPhysicalDrive(physicalDrive, desiredAccess))

         using (var safeBuffer = GetDeviceIoData3<NativeMethods.VOLUME_DISK_EXTENTS>(safeHandle, NativeMethods.IoControlCode.IOCTL_STORAGE_GET_DEVICE_NUMBER, physicalDrive, 32))
         {
            var offset = 0;

            var structure = safeBuffer.PtrToStructure<NativeMethods.VOLUME_DISK_EXTENTS>(0);

            safeBuffer.PtrToStructure<NativeMethods.VOLUME_DISK_EXTENTS>(0);

            //for (var i = 0; i < structure.NumberOfDiskExtents; i += offset)
            //{
            //   structure.Extents[i] = new NativeMethods.DISK_EXTENT[structure.NumberOfDiskExtents];
            //}


            return structure;
         }
      }
   }
}
