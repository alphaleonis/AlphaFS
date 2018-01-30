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

      /// <summary>Opens a physical device, such as: "\\.\PhysicalDrive0" for access.</summary>
      /// <returns>A <see cref="SafeFileHandle"/> instance.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="Exception"/>
      /// <param name="path">The path to the physical device, such as: "\\.\PhysicalDrive0".</param>
      /// <param name="fileSystemRights">If no elevated access is needed to access the physical device, specify 0 for this parameter.</param>
      [SecurityCritical]
      public static SafeFileHandle OpenPhysicalDrive(string path, FileSystemRights fileSystemRights)
      {
         return File.CreateFileCore(null, path, ExtendedFileAttributes.Normal, null, FileMode.Open, fileSystemRights, FileShare.ReadWrite, false, false, PathFormat.LongFullPath);
      }
   }
}
