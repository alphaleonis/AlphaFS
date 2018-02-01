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

namespace Alphaleonis.Win32.Filesystem
{
   public static partial class Device
   {
      [SecurityCritical]
      public static void GetVolumeDiskExtents(string logicalDrive)
      {
         // FileSystemRights desiredAccess: If this parameter is zero, the application can query certain metadata such as file, directory, or device attributes
         // without accessing that file or device, even if GENERIC_READ access would have been denied.
         // You cannot request an access mode that conflicts with the sharing mode that is specified by the dwShareMode parameter in an open request that already has an open handle.
         //const int desiredAccess = 0;

         // Requires elevation.
         const FileSystemRights desiredAccess = FileSystemRights.Read | FileSystemRights.Write;

         //const bool elevatedAccess = (desiredAccess & FileSystemRights.Read) != 0 && (desiredAccess & FileSystemRights.Write) != 0;



         //var volumeDiskExtents = new NativeMethods.VOLUME_DISK_EXTENTS();

         logicalDrive = @"\\.\C:";
         using (var safeHandle = OpenPhysicalDrive(logicalDrive, desiredAccess))

         using (var safeBuffer = GetDeviceIoData<NativeMethods.VOLUME_DISK_EXTENTS>(safeHandle, NativeMethods.IoControlCode.IOCTL_VOLUME_GET_VOLUME_DISK_EXTENTS, logicalDrive))
         {
            if (null != safeBuffer)
            {
               var structure = safeBuffer.PtrToStructure<NativeMethods.VOLUME_DISK_EXTENTS>(0);

               for (var i = 0; i < structure.NumberOfDiskExtents; ++i)
               {
                  var pDiskExtent = structure.Extents[i];
               }
            }
         }
      }
   }
}
