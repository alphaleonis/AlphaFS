/*  Copyright (C) 2008-2015 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
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
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security;

namespace Alphaleonis.Win32.Filesystem
{
   partial class Directory
   {
      #region DeleteEmptySubdirectories

      #region Non-Transactional

      /// <summary>[AlphaFS] Deletes empty subdirectories from the specified directory.</summary>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <param name="path">The name of the directory to remove empty subdirectories from.</param>
      /// <param name="recursive"><see langword="true"/> deletes empty subdirectories from this directory and its subdirectories.</param>
      [SecurityCritical]
      public static void DeleteEmptySubdirectories(string path, bool recursive)
      {
         DeleteEmptySubdirectoriesInternal(null, null, path, recursive, false, true, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Deletes empty subdirectories from the specified directory.</summary>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <param name="path">The name of the directory to remove empty subdirectories from.</param>
      /// <param name="recursive"><see langword="true"/> deletes empty subdirectories from this directory and its subdirectories.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void DeleteEmptySubdirectories(string path, bool recursive, PathFormat pathFormat)
      {
         DeleteEmptySubdirectoriesInternal(null, null, path, recursive, false, true, pathFormat);
      }



      /// <summary>[AlphaFS] Deletes empty subdirectories from the specified directory.</summary>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <param name="path">The name of the directory to remove empty subdirectories from.</param>
      /// <param name="recursive"><see langword="true"/> deletes empty subdirectories from this directory and its subdirectories.</param>
      /// <param name="ignoreReadOnly"><see langword="true"/> overrides read only <see cref="FileAttributes"/> of empty directories.</param>
      [SecurityCritical]
      public static void DeleteEmptySubdirectories(string path, bool recursive, bool ignoreReadOnly)
      {
         DeleteEmptySubdirectoriesInternal(null, null, path, recursive, ignoreReadOnly, true, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Deletes empty subdirectories from the specified directory.</summary>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <param name="path">The name of the directory to remove empty subdirectories from.</param>
      /// <param name="recursive"><see langword="true"/> deletes empty subdirectories from this directory and its subdirectories.</param>
      /// <param name="ignoreReadOnly"><see langword="true"/> overrides read only <see cref="FileAttributes"/> of empty directories.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void DeleteEmptySubdirectories(string path, bool recursive, bool ignoreReadOnly, PathFormat pathFormat)
      {
         DeleteEmptySubdirectoriesInternal(null, null, path, recursive, ignoreReadOnly, true, pathFormat);
      }

      #endregion // Non-Transactional

      #region Transactional

      /// <summary>[AlphaFS] Deletes empty subdirectories from the specified directory.</summary>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The name of the directory to remove empty subdirectories from.</param>
      /// <param name="recursive"><see langword="true"/> deletes empty subdirectories from this directory and its subdirectories.</param>
      [SecurityCritical]
      public static void DeleteEmptySubdirectories(KernelTransaction transaction, string path, bool recursive)
      {
         DeleteEmptySubdirectoriesInternal(null, transaction, path, recursive, false, true, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Deletes empty subdirectories from the specified directory.</summary>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The name of the directory to remove empty subdirectories from.</param>
      /// <param name="recursive"><see langword="true"/> deletes empty subdirectories from this directory and its subdirectories.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void DeleteEmptySubdirectories(KernelTransaction transaction, string path, bool recursive, PathFormat pathFormat)
      {
         DeleteEmptySubdirectoriesInternal(null, transaction, path, recursive, false, true, pathFormat);
      }



      /// <summary>[AlphaFS] Deletes empty subdirectories from the specified directory.</summary>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The name of the directory to remove empty subdirectories from.</param>
      /// <param name="recursive"><see langword="true"/> deletes empty subdirectories from this directory and its subdirectories.</param>
      /// <param name="ignoreReadOnly"><see langword="true"/> overrides read only <see cref="FileAttributes"/> of empty directories.</param>
      [SecurityCritical]
      public static void DeleteEmptySubdirectories(KernelTransaction transaction, string path, bool recursive, bool ignoreReadOnly)
      {
         DeleteEmptySubdirectoriesInternal(null, transaction, path, recursive, ignoreReadOnly, true, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Deletes empty subdirectories from the specified directory.</summary>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The name of the directory to remove empty subdirectories from.</param>
      /// <param name="recursive"><see langword="true"/> deletes empty subdirectories from this directory and its subdirectories.</param>
      /// <param name="ignoreReadOnly"><see langword="true"/> overrides read only <see cref="FileAttributes"/> of empty directories.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void DeleteEmptySubdirectories(KernelTransaction transaction, string path, bool recursive, bool ignoreReadOnly, PathFormat pathFormat)
      {
         DeleteEmptySubdirectoriesInternal(null, transaction, path, recursive, ignoreReadOnly, true, pathFormat);
      }

      #endregion // Transactional

      #endregion // DeleteEmptySubdirectories

      #region Internal Methods

      /// <summary>[AlphaFS] Unified method DeleteEmptySubdirectoriesInternal() to delete empty subdirectories from the specified directory.</summary>
      /// <remarks>Trailing spaces are removed from the end of the <paramref name="path"/> parameter before deleting the empty directory.</remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="IOException">path is <see langword="null"/>.</exception>
      /// <param name="fileSystemEntryInfo">A FileSystemEntryInfo instance. Use either <paramref name="fileSystemEntryInfo"/> or <paramref name="path"/>, not both.</param>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The name of the directory to remove empty subdirectories from. Use either <paramref name="path"/> or <paramref name="fileSystemEntryInfo"/>, not both.</param>
      /// <param name="recursive"><see langword="true"/> deletes empty subdirectories from this directory and its subdirectories.</param>
      /// <param name="ignoreReadOnly"><see langword="true"/> overrides read only <see cref="FileAttributes"/> of empty directories.</param>
      /// <param name="initialize">When <see langword="true"/> indicates the method is called externally.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      internal static void DeleteEmptySubdirectoriesInternal(FileSystemEntryInfo fileSystemEntryInfo, KernelTransaction transaction, string path, bool recursive, bool ignoreReadOnly, bool initialize, PathFormat pathFormat)
      {
         #region Setup

         if (pathFormat == PathFormat.RelativePath)
            Path.CheckSupportedPathFormat(path, true, true);

         if (fileSystemEntryInfo == null)
         {
            if (!File.ExistsInternal(true, transaction, path, pathFormat))
               NativeError.ThrowException(Win32Errors.ERROR_PATH_NOT_FOUND, path);

            fileSystemEntryInfo = File.GetFileSystemEntryInfoInternal(transaction,
               Path.GetExtendedLengthPathInternal(transaction, path, pathFormat, GetFullPathOptions.TrimEnd | GetFullPathOptions.RemoveTrailingDirectorySeparator | GetFullPathOptions.FullCheck)
               , false, pathFormat);
         }

         if (fileSystemEntryInfo == null)
            throw new ArgumentNullException("path");

         string pathLp = fileSystemEntryInfo.LongFullPath;

         #endregion // Setup

         // Ensure path is a directory.
         if (!fileSystemEntryInfo.IsDirectory)
            throw new IOException(String.Format(CultureInfo.CurrentCulture, Resources.FileExistsWithSameNameSpecifiedByPath, pathLp));

         var dirEnumOptions = DirectoryEnumerationOptions.Folders;
         if (recursive)
            dirEnumOptions |= DirectoryEnumerationOptions.Recursive;

         foreach (var fsei in EnumerateFileSystemEntryInfosInternal<FileSystemEntryInfo>(transaction, pathLp, Path.WildcardStarMatchAll, dirEnumOptions, PathFormat.LongFullPath))
            DeleteEmptySubdirectoriesInternal(fsei, transaction, null, recursive, ignoreReadOnly, false, PathFormat.LongFullPath);


         if (!EnumerateFileSystemEntryInfosInternal<string>(transaction, pathLp, Path.WildcardStarMatchAll, DirectoryEnumerationOptions.FilesAndFolders, PathFormat.LongFullPath).Any())
         {
            // Prevent deleting path itself.
            if (!initialize)
               DeleteDirectoryInternal(fileSystemEntryInfo, transaction, null, false, ignoreReadOnly, true, true, PathFormat.LongFullPath);
         }
      }

      #endregion // DeleteEmptySubdirectoriesInternal
   }
}