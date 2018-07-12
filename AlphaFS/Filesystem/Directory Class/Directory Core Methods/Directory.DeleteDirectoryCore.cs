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
using System.Collections.Generic;
using System.IO;
using System.Security;

namespace Alphaleonis.Win32.Filesystem
{
   public static partial class Directory
   {
      /// <summary>Deletes the specified directory and, if indicated, any subdirectories in the directory.</summary>
      /// <remarks>The RemoveDirectory function marks a directory for deletion on close. Therefore, the directory is not removed until the last handle to the directory is closed.</remarks>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="DirectoryReadOnlyException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="fsEntryInfo">A FileSystemEntryInfo instance. Use either <paramref name="fsEntryInfo"/> or <paramref name="path"/>, not both.</param>
      /// <param name="path">The name of the directory to remove. Use either <paramref name="path"/> or <paramref name="fsEntryInfo"/>, not both.</param>
      /// <param name="recursive"><c>true</c> to remove all files and subdirectories recursively; <c>false</c> otherwise only the top level empty directory.</param>
      /// <param name="ignoreReadOnly"><c>true</c> overrides read only attribute of files and directories.</param>
      /// <param name="continueOnNotFound">When <c>true</c> does not throw an <see cref="DirectoryNotFoundException"/> when the directory does not exist.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      internal static void DeleteDirectoryCore(KernelTransaction transaction, FileSystemEntryInfo fsEntryInfo, string path, bool recursive, bool ignoreReadOnly, bool continueOnNotFound, PathFormat pathFormat)
      {
         if (null == fsEntryInfo)
         {
            if (null == path)
               throw new ArgumentNullException("path");


            // MSDN: .NET 3.5+: DirectoryNotFoundException:
            // Path does not exist or could not be found.
            // Path refers to a file instead of a directory.
            // The specified path is invalid (for example, it is on an unmapped drive). 

            if (pathFormat == PathFormat.RelativePath)
               Path.CheckSupportedPathFormat(path, true, true);

            fsEntryInfo = File.GetFileSystemEntryInfoCore(transaction, true, Path.GetExtendedLengthPathCore(transaction, path, pathFormat, GetFullPathOptions.RemoveTrailingDirectorySeparator), continueOnNotFound, pathFormat);

            if (null == fsEntryInfo)
               return;
         }

         pathFormat = PathFormat.LongFullPath;


         // Reset attributes to Normal if we already know the facts.

         if (ignoreReadOnly && (fsEntryInfo.IsReadOnly || fsEntryInfo.IsHidden))

            File.SetAttributesCore(transaction, fsEntryInfo.IsDirectory, fsEntryInfo.LongFullPath, FileAttributes.Normal, PathFormat.LongFullPath);


         // Do not follow mount points nor symbolic links, but do delete the reparse point itself.
         // If directory is reparse point, disable recursion.

         if (recursive && !fsEntryInfo.IsReparsePoint)
         {
            var dirs = new Stack<string>(1000);

            foreach (var fsei in EnumerateFileSystemEntryInfosCore<FileSystemEntryInfo>(null, transaction, fsEntryInfo.LongFullPath, Path.WildcardStarMatchAll, SearchOption.AllDirectories, null, null, pathFormat))
            {
               if (fsei.IsDirectory)
               {
                  // Check to see if this is a mount point, and unmount it.
                  // Now it is safe to delete the actual directory.
                  if (fsei.IsMountPoint)
                     DeleteJunctionCore(transaction, fsei, null, false, pathFormat);

                  dirs.Push(fsei.LongFullPath);
               }

               else
                  File.DeleteFileCore(transaction, fsei.LongFullPath, ignoreReadOnly, fsei.Attributes, pathFormat);
            }


            while (dirs.Count > 0)
               DeleteDirectoryNative(transaction, dirs.Pop(), ignoreReadOnly, continueOnNotFound, 0);
         }


         // Check to see if this is a mount point, and unmount it.
         // Now it is safe to delete the actual directory.

         if (fsEntryInfo.IsMountPoint)
            DeleteJunctionCore(transaction, fsEntryInfo, null, false, pathFormat);

         DeleteDirectoryNative(transaction, fsEntryInfo.LongFullPath, ignoreReadOnly, continueOnNotFound, 0);
      }
   }
}
