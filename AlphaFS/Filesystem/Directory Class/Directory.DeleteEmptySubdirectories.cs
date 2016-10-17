/*  Copyright (C) 2008-2016 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
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
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security;

namespace Alphaleonis.Win32.Filesystem
{
   partial class Directory
   {
      /// <summary>[AlphaFS] Deletes empty subdirectories from the specified directory.</summary>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="DirectoryReadOnlyException"/>
      /// <param name="path">The name of the directory to remove empty subdirectories from.</param>
      [SecurityCritical]
      public static void DeleteEmptySubdirectories(string path)
      {
         DeleteEmptySubdirectoriesCore(null, null, path, false, false, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Deletes empty subdirectories from the specified directory.</summary>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="DirectoryReadOnlyException"/>
      /// <param name="path">The name of the directory to remove empty subdirectories from.</param>
      /// <param name="recursive"><see langword="true"/> deletes empty subdirectories from this directory and its subdirectories.</param>
      [SecurityCritical]
      public static void DeleteEmptySubdirectories(string path, bool recursive)
      {
         DeleteEmptySubdirectoriesCore(null, null, path, recursive, false, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Deletes empty subdirectories from the specified directory.</summary>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="DirectoryReadOnlyException"/>
      /// <param name="path">The name of the directory to remove empty subdirectories from.</param>
      /// <param name="recursive"><see langword="true"/> deletes empty subdirectories from this directory and its subdirectories.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void DeleteEmptySubdirectories(string path, bool recursive, PathFormat pathFormat)
      {
         DeleteEmptySubdirectoriesCore(null, null, path, recursive, false, pathFormat);
      }
      
      /// <summary>[AlphaFS] Deletes empty subdirectories from the specified directory.</summary>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="DirectoryReadOnlyException"/>
      /// <param name="path">The name of the directory to remove empty subdirectories from.</param>
      /// <param name="recursive"><see langword="true"/> deletes empty subdirectories from this directory and its subdirectories.</param>
      /// <param name="ignoreReadOnly"><see langword="true"/> overrides read only <see cref="FileAttributes"/> of empty directories.</param>
      [SecurityCritical]
      public static void DeleteEmptySubdirectories(string path, bool recursive, bool ignoreReadOnly)
      {
         DeleteEmptySubdirectoriesCore(null, null, path, recursive, ignoreReadOnly, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Deletes empty subdirectories from the specified directory.</summary>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="DirectoryReadOnlyException"/>
      /// <param name="path">The name of the directory to remove empty subdirectories from.</param>
      /// <param name="recursive"><see langword="true"/> deletes empty subdirectories from this directory and its subdirectories.</param>
      /// <param name="ignoreReadOnly"><see langword="true"/> overrides read only <see cref="FileAttributes"/> of empty directories.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void DeleteEmptySubdirectories(string path, bool recursive, bool ignoreReadOnly, PathFormat pathFormat)
      {
         DeleteEmptySubdirectoriesCore(null, null, path, recursive, ignoreReadOnly, pathFormat);
      }


      #region Transactional

      /// <summary>[AlphaFS] Deletes empty subdirectories from the specified directory.</summary>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="DirectoryReadOnlyException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The name of the directory to remove empty subdirectories from.</param>
      [SecurityCritical]
      public static void DeleteEmptySubdirectoriesTransacted(KernelTransaction transaction, string path)
      {
         DeleteEmptySubdirectoriesCore(null, transaction, path, false, false, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Deletes empty subdirectories from the specified directory.</summary>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="DirectoryReadOnlyException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The name of the directory to remove empty subdirectories from.</param>
      /// <param name="recursive"><see langword="true"/> deletes empty subdirectories from this directory and its subdirectories.</param>
      [SecurityCritical]
      public static void DeleteEmptySubdirectoriesTransacted(KernelTransaction transaction, string path, bool recursive)
      {
         DeleteEmptySubdirectoriesCore(null, transaction, path, recursive, false, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Deletes empty subdirectories from the specified directory.</summary>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="DirectoryReadOnlyException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The name of the directory to remove empty subdirectories from.</param>
      /// <param name="recursive"><see langword="true"/> deletes empty subdirectories from this directory and its subdirectories.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void DeleteEmptySubdirectoriesTransacted(KernelTransaction transaction, string path, bool recursive, PathFormat pathFormat)
      {
         DeleteEmptySubdirectoriesCore(null, transaction, path, recursive, false, pathFormat);
      }
      
      /// <summary>[AlphaFS] Deletes empty subdirectories from the specified directory.</summary>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="DirectoryReadOnlyException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The name of the directory to remove empty subdirectories from.</param>
      /// <param name="recursive"><see langword="true"/> deletes empty subdirectories from this directory and its subdirectories.</param>
      /// <param name="ignoreReadOnly"><see langword="true"/> overrides read only <see cref="FileAttributes"/> of empty directories.</param>
      [SecurityCritical]
      public static void DeleteEmptySubdirectoriesTransacted(KernelTransaction transaction, string path, bool recursive, bool ignoreReadOnly)
      {
         DeleteEmptySubdirectoriesCore(null, transaction, path, recursive, ignoreReadOnly, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Deletes empty subdirectories from the specified directory.</summary>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="DirectoryReadOnlyException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The name of the directory to remove empty subdirectories from.</param>
      /// <param name="recursive"><see langword="true"/> deletes empty subdirectories from this directory and its subdirectories.</param>
      /// <param name="ignoreReadOnly"><see langword="true"/> overrides read only <see cref="FileAttributes"/> of empty directories.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void DeleteEmptySubdirectoriesTransacted(KernelTransaction transaction, string path, bool recursive, bool ignoreReadOnly, PathFormat pathFormat)
      {
         DeleteEmptySubdirectoriesCore(null, transaction, path, recursive, ignoreReadOnly, pathFormat);
      }

      #endregion // Transactional


      #region Internal Methods

      /// <summary>[AlphaFS] Delete empty subdirectories from the specified directory.</summary>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="DirectoryReadOnlyException"/>
      /// <param name="fsEntryInfo">A FileSystemEntryInfo instance. Use either <paramref name="fsEntryInfo"/> or <paramref name="path"/>, not both.</param>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The name of the directory to remove empty subdirectories from. Use either <paramref name="path"/> or <paramref name="fsEntryInfo"/>, not both.</param>
      /// <param name="recursive"><see langword="true"/> deletes empty subdirectories from this directory and its subdirectories.</param>
      /// <param name="ignoreReadOnly"><see langword="true"/> overrides read only <see cref="FileAttributes"/> of empty directories.</param>
      /// <param name="initialize">When <see langword="true"/> indicates the method is called externally.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      internal static void DeleteEmptySubdirectoriesCore0(FileSystemEntryInfo fsEntryInfo, KernelTransaction transaction, string path, bool recursive, bool ignoreReadOnly, bool initialize, PathFormat pathFormat)
      {
         #region Setup

         if (fsEntryInfo == null)
         {
            if (pathFormat == PathFormat.RelativePath)
               Path.CheckSupportedPathFormat(path, true, true);

            if (!File.ExistsCore(true, transaction, path, pathFormat))
               NativeError.ThrowException(Win32Errors.ERROR_PATH_NOT_FOUND, path);

            fsEntryInfo = File.GetFileSystemEntryInfoCore(true, transaction, Path.GetExtendedLengthPathCore(transaction, path, pathFormat, GetFullPathOptions.TrimEnd | GetFullPathOptions.RemoveTrailingDirectorySeparator | GetFullPathOptions.FullCheck) , false, pathFormat);

            if (fsEntryInfo == null)
               return;
         }
         
         #endregion // Setup


         // Ensure path is a directory.
         if (!fsEntryInfo.IsDirectory)
            throw new IOException(string.Format(CultureInfo.CurrentCulture, Resources.Target_Directory_Is_A_File, fsEntryInfo.LongFullPath));


         var dirEnumOptions = DirectoryEnumerationOptions.Folders | DirectoryEnumerationOptions.SkipReparsePoints;

         if (recursive)
            dirEnumOptions |= DirectoryEnumerationOptions.Recursive;


         foreach (var fsei in EnumerateFileSystemEntryInfosCore<FileSystemEntryInfo>(transaction, fsEntryInfo.LongFullPath, Path.WildcardStarMatchAll, dirEnumOptions, PathFormat.LongFullPath))
            DeleteEmptySubdirectoriesCore0(fsei, transaction, null, recursive, ignoreReadOnly, false, PathFormat.LongFullPath);


         if (!EnumerateFileSystemEntryInfosCore<string>(transaction, fsEntryInfo.LongFullPath, Path.WildcardStarMatchAll, DirectoryEnumerationOptions.FilesAndFolders, PathFormat.LongFullPath).Any())
         {
            // Prevent deleting path itself.
            if (!initialize)
               DeleteDirectoryCore(fsEntryInfo, transaction, null, false, ignoreReadOnly, true, PathFormat.LongFullPath);
         }
      }


      /// <summary>[AlphaFS] Delete empty subdirectories from the specified directory.</summary>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="DirectoryReadOnlyException"/>
      /// <param name="fsEntryInfo">A FileSystemEntryInfo instance. Use either <paramref name="fsEntryInfo"/> or <paramref name="path"/>, not both.</param>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The name of the directory to remove empty subdirectories from. Use either <paramref name="path"/> or <paramref name="fsEntryInfo"/>, not both.</param>
      /// <param name="recursive"><see langword="true"/> deletes empty subdirectories from this directory and its subdirectories.</param>
      /// <param name="ignoreReadOnly"><see langword="true"/> overrides read only <see cref="FileAttributes"/> of empty directories.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      internal static void DeleteEmptySubdirectoriesCore(FileSystemEntryInfo fsEntryInfo, KernelTransaction transaction, string path, bool recursive, bool ignoreReadOnly, PathFormat pathFormat)
      {
         #region Setup

         if (fsEntryInfo == null)
         {
            if (pathFormat == PathFormat.RelativePath)
               Path.CheckSupportedPathFormat(path, true, true);

            if (!File.ExistsCore(true, transaction, path, pathFormat))
               NativeError.ThrowException(Win32Errors.ERROR_PATH_NOT_FOUND, path);

            fsEntryInfo = File.GetFileSystemEntryInfoCore(true, transaction, Path.GetExtendedLengthPathCore(transaction, path, pathFormat, GetFullPathOptions.TrimEnd | GetFullPathOptions.RemoveTrailingDirectorySeparator | GetFullPathOptions.FullCheck), false, pathFormat);

            if (fsEntryInfo == null)
               return;
         }

         #endregion // Setup


         // Ensure path is a directory.
         if (!fsEntryInfo.IsDirectory)
            throw new IOException(string.Format(CultureInfo.CurrentCulture, Resources.Target_Directory_Is_A_File, fsEntryInfo.LongFullPath));


         var dirs = new Stack<string>(1000);
         dirs.Push(fsEntryInfo.LongFullPath);

         while (dirs.Count > 0)
         {
            foreach (var fsei in EnumerateFileSystemEntryInfosCore<FileSystemEntryInfo>(transaction, dirs.Pop(), Path.WildcardStarMatchAll, DirectoryEnumerationOptions.Folders | DirectoryEnumerationOptions.ContinueOnException, PathFormat.LongFullPath))
            {
               // Ensure the directory is empty.
               if (!EnumerateFileSystemEntryInfosCore<string>(transaction, fsei.LongFullPath, Path.WildcardStarMatchAll, DirectoryEnumerationOptions.FilesAndFolders, PathFormat.LongFullPath).Any())
                  DeleteDirectoryCore(fsei, transaction, null, false, ignoreReadOnly, true, PathFormat.LongFullPath);

               else if (recursive)
                  dirs.Push(fsei.LongFullPath);
            }
         }
      }

      #endregion // Internal Methods
   }
}
