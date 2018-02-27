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
using Microsoft.Win32.SafeHandles;
using System.IO;
using System.Security;
using System.Security.AccessControl;

namespace Alphaleonis.Win32.Filesystem
{
   public static partial class Device
   {
      /// <summary>Opens a physical disk, volume/logical drive for read access.</summary>
      /// <returns>A <see cref="SafeFileHandle"/> instance.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="Exception"/>
      /// <param name="devicePath">
      /// <para>A drive path such as: "C", "C:" or "C:\".</para>
      /// <para>A volume <see cref="Guid"/> such as: "\\?\Volume{xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}\".</para>
      /// <para>A <see cref="DeviceInfo.DevicePath"/> string such as: "\\?\pcistor#disk...{xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}".</para>
      /// </param>
      /// <param name="fileSystemRights">If no elevated access is needed to access the physical device, specify 0 for this parameter.</param>
      [SecurityCritical]
      public static SafeFileHandle OpenPhysicalDrive(string devicePath, FileSystemRights fileSystemRights)
      {
         // fileSystemRights: If this parameter is 0 (FILE_ANY_ACCESS), the application can query certain metadata such as file, directory, or device attributes
         // without accessing that file or device, even if GENERIC_READ access would have been denied.
         // You cannot request an access mode that conflicts with the sharing mode that is specified by the dwShareMode parameter in an open request that already has an open handle.
         //
         //
         // When opening a volume or removable media drive (for example, a floppy disk drive or flash memory thumb drive), the lpFileName string should be the following form: "\\.\X:".
         // Do not use a trailing backslash (\), which indicates the root directory of a drive.
         //
         // When opening a volume or floppy disk, the dwShareMode parameter must have the FILE_SHARE_WRITE flag.


         return File.CreateFileCore(null, devicePath, ExtendedFileAttributes.Normal | ExtendedFileAttributes.NoBuffering, null, FileMode.Open, fileSystemRights, FileShare.ReadWrite, false, false, PathFormat.LongFullPath);
      }
   }
}
