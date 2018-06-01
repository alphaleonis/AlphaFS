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
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;

namespace Alphaleonis.Win32.Filesystem
{
   public static partial class Directory
   {
      #region .NET

      /// <summary>Deletes an empty directory from a specified path.</summary>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="DirectoryReadOnlyException"/>
      /// <param name="path">The name of the empty directory to remove. This directory must be writable and empty.</param>
      [SecurityCritical]
      public static void Delete(string path)
      {
         DeleteDirectoryCore(null, null, path, false, false, false, PathFormat.RelativePath);
      }

      /// <summary>Deletes the specified directory and, if indicated, any subdirectories in the directory.</summary>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="DirectoryReadOnlyException"/>
      /// <param name="path">The name of the directory to remove.</param>
      /// <param name="recursive"><see langword="true"/> to remove directories, subdirectories, and files in <paramref name="path"/>. <see langword="false"/> otherwise.</param>
      [SecurityCritical]
      public static void Delete(string path, bool recursive)
      {
         DeleteDirectoryCore(null, null, path, recursive, false, false, PathFormat.RelativePath);
      }

      #endregion // .NET

      /// <summary>[AlphaFS] Deletes an empty directory from a specified path.</summary>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="DirectoryReadOnlyException"/>
      /// <param name="path">The name of the empty directory to remove. This directory must be writable and empty.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void Delete(string path, PathFormat pathFormat)
      {
         DeleteDirectoryCore(null, null, path, false, false, false, pathFormat);
      }


      /// <summary>[AlphaFS] Deletes the specified directory and, if indicated, any subdirectories in the directory.</summary>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="DirectoryReadOnlyException"/>
      /// <param name="path">The name of the directory to remove.</param>
      /// <param name="recursive"><see langword="true"/> to remove directories, subdirectories, and files in <paramref name="path"/>. <see langword="false"/> otherwise.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void Delete(string path, bool recursive, PathFormat pathFormat)
      {
         DeleteDirectoryCore(null, null, path, recursive, false, false, pathFormat);
      }


      /// <summary>[AlphaFS] Deletes the specified directory and, if indicated, any subdirectories in the directory.</summary>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="DirectoryReadOnlyException"/>
      /// <param name="path">The name of the directory to remove.</param>
      /// <param name="recursive"><see langword="true"/> to remove directories, subdirectories, and files in <paramref name="path"/>. <see langword="false"/> otherwise.</param>
      /// <param name="ignoreReadOnly"><see langword="true"/> overrides read only <see cref="FileAttributes"/> of files and directories.</param>
      [SecurityCritical]
      public static void Delete(string path, bool recursive, bool ignoreReadOnly)
      {
         DeleteDirectoryCore(null, null, path, recursive, ignoreReadOnly, false, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Deletes the specified directory and, if indicated, any subdirectories in the directory.</summary>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="DirectoryReadOnlyException"/>
      /// <param name="path">The name of the directory to remove.</param>
      /// <param name="recursive"><see langword="true"/> to remove directories, subdirectories, and files in <paramref name="path"/>. <see langword="false"/> otherwise.</param>
      /// <param name="ignoreReadOnly"><see langword="true"/> overrides read only <see cref="FileAttributes"/> of files and directories.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void Delete(string path, bool recursive, bool ignoreReadOnly, PathFormat pathFormat)
      {
         DeleteDirectoryCore(null, null, path, recursive, ignoreReadOnly, false, pathFormat);
      }


      #region Transactional

      /// <summary>[AlphaFS] Deletes an empty directory from a specified path.</summary>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="DirectoryReadOnlyException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The name of the empty directory to remove. This directory must be writable and empty.</param>
      [SecurityCritical]
      public static void DeleteTransacted(KernelTransaction transaction, string path)
      {
         DeleteDirectoryCore(transaction, null, path, false, false, false, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Deletes an empty directory from a specified path.</summary>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="DirectoryReadOnlyException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The name of the empty directory to remove. This directory must be writable and empty.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void DeleteTransacted(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         DeleteDirectoryCore(transaction, null, path, false, false, false, pathFormat);
      }


      /// <summary>[AlphaFS] Deletes the specified directory and, if indicated, any subdirectories in the directory.</summary>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="DirectoryReadOnlyException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The name of the directory to remove.</param>
      /// <param name="recursive"><see langword="true"/> to remove directories, subdirectories, and files in <paramref name="path"/>. <see langword="false"/> otherwise.</param>
      [SecurityCritical]
      public static void DeleteTransacted(KernelTransaction transaction, string path, bool recursive)
      {
         DeleteDirectoryCore(transaction, null, path, recursive, false, false, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Deletes the specified directory and, if indicated, any subdirectories in the directory.</summary>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="DirectoryReadOnlyException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The name of the directory to remove.</param>
      /// <param name="recursive"><see langword="true"/> to remove directories, subdirectories, and files in <paramref name="path"/>. <see langword="false"/> otherwise.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void DeleteTransacted(KernelTransaction transaction, string path, bool recursive, PathFormat pathFormat)
      {
         DeleteDirectoryCore(transaction, null, path, recursive, false, false, pathFormat);
      }


      /// <summary>[AlphaFS] Deletes the specified directory and, if indicated, any subdirectories in the directory.</summary>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="DirectoryReadOnlyException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The name of the directory to remove.</param>
      /// <param name="recursive"><see langword="true"/> to remove directories, subdirectories, and files in <paramref name="path"/>. <see langword="false"/> otherwise.</param>
      /// <param name="ignoreReadOnly"><see langword="true"/> overrides read only <see cref="FileAttributes"/> of files and directories.</param>
      [SecurityCritical]
      public static void DeleteTransacted(KernelTransaction transaction, string path, bool recursive, bool ignoreReadOnly)
      {
         DeleteDirectoryCore(transaction, null, path, recursive, ignoreReadOnly, false, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Deletes the specified directory and, if indicated, any subdirectories in the directory.</summary>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="DirectoryReadOnlyException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The name of the directory to remove.</param>
      /// <param name="recursive"><see langword="true"/> to remove directories, subdirectories, and files in <paramref name="path"/>. <see langword="false"/> otherwise.</param>
      /// <param name="ignoreReadOnly"><see langword="true"/> overrides read only <see cref="FileAttributes"/> of files and directories.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void DeleteTransacted(KernelTransaction transaction, string path, bool recursive, bool ignoreReadOnly, PathFormat pathFormat)
      {
         DeleteDirectoryCore(transaction, null, path, recursive, ignoreReadOnly, false, pathFormat);
      }

      #endregion // Transactional


      #region Internal Methods

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
      /// <param name="recursive"><see langword="true"/> to remove all files and subdirectories recursively; <see langword="false"/> otherwise only the top level empty directory.</param>
      /// <param name="ignoreReadOnly"><see langword="true"/> overrides read only attribute of files and directories.</param>
      /// <param name="continueOnNotFound">When <see langword="true"/> does not throw an <see cref="DirectoryNotFoundException"/> when the directory does not exist.</param>
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
                     //Volume.DeleteVolumeMountPointCore(transaction, fsei.LongFullPath, false, true, pathFormat);

                  dirs.Push(fsei.LongFullPath);
               }

               else
                  File.DeleteFileCore(transaction, fsei.LongFullPath, ignoreReadOnly, pathFormat);
            }


            while (dirs.Count > 0)
               DeleteDirectoryCore(transaction, dirs.Pop(), ignoreReadOnly, continueOnNotFound);
         }


         // Check to see if this is a mount point, and unmount it.
         // Now it is safe to delete the actual directory.
         if (fsEntryInfo.IsMountPoint)
            DeleteJunctionCore(transaction, fsEntryInfo, null, false, pathFormat);
            //Volume.DeleteVolumeMountPointCore(transaction, fsEntryInfo.LongFullPath, false, true, pathFormat);

         DeleteDirectoryCore(transaction, fsEntryInfo.LongFullPath, ignoreReadOnly, continueOnNotFound);
      }


      [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
      private static void DeleteDirectoryCore(KernelTransaction transaction, string pathLp, bool ignoreReadOnly, bool continueOnNotFound)
      {

      startRemoveDirectory:

         var success = transaction == null || !NativeMethods.IsAtLeastWindowsVista

            // RemoveDirectory() / RemoveDirectoryTransacted()
            // 2014-09-09: MSDN confirms LongPath usage.

            // RemoveDirectory on a symbolic link will remove the link itself.

            ? NativeMethods.RemoveDirectory(pathLp)
            : NativeMethods.RemoveDirectoryTransacted(pathLp, transaction.SafeHandle);


         var lastError = Marshal.GetLastWin32Error();
         if (!success)
         {
            switch ((uint) lastError)
            {
               case Win32Errors.ERROR_DIR_NOT_EMPTY:
                  // MSDN: .NET 3.5+: IOException: The directory specified by path is not an empty directory. 
                  throw new DirectoryNotEmptyException(pathLp, true);


               case Win32Errors.ERROR_DIRECTORY:
                  // MSDN: .NET 3.5+: DirectoryNotFoundException: Path refers to a file instead of a directory.
                  if (File.ExistsCore(transaction, false, pathLp, PathFormat.LongFullPath))
                     throw new DirectoryNotFoundException(string.Format(CultureInfo.InvariantCulture, "({0}) {1}", lastError, string.Format(CultureInfo.InvariantCulture, Resources.Target_Directory_Is_A_File, pathLp)));
                  break;


               case Win32Errors.ERROR_PATH_NOT_FOUND:
                  if (continueOnNotFound)
                     return;
                  break;


               case Win32Errors.ERROR_SHARING_VIOLATION:
                  // MSDN: .NET 3.5+: IOException: The directory is being used by another process or there is an open handle on the directory.
                  NativeError.ThrowException(lastError, pathLp);
                  break;


               case Win32Errors.ERROR_ACCESS_DENIED:
                  var attrs = new NativeMethods.WIN32_FILE_ATTRIBUTE_DATA();
                  var dataInitialised = File.FillAttributeInfoCore(transaction, pathLp, ref attrs, false, true);

                  
                  if (File.IsReadOnly(attrs.dwFileAttributes))
                  {
                     // MSDN: .NET 3.5+: IOException: The directory specified by path is read-only.

                     if (ignoreReadOnly)
                     {
                        // Reset directory attributes to Normal.
                        File.SetAttributesCore(transaction, true, pathLp, FileAttributes.Normal, PathFormat.LongFullPath);

                        goto startRemoveDirectory;
                     }


                     // MSDN: .NET 3.5+: IOException: The directory is read-only.
                     throw new DirectoryReadOnlyException(pathLp);
                  }


                  // MSDN: .NET 3.5+: UnauthorizedAccessException: The caller does not have the required permission.
                  if (dataInitialised == Win32Errors.ERROR_SUCCESS)
                     NativeError.ThrowException(lastError, pathLp);

                  break;
            }

            // MSDN: .NET 3.5+: IOException:
            // A file with the same name and location specified by path exists.
            // The directory specified by path is read-only, or recursive is false and path is not an empty directory. 
            // The directory is the application's current working directory. 
            // The directory contains a read-only file.
            // The directory is being used by another process.

            NativeError.ThrowException(lastError, pathLp);
         }
      }

      #endregion // Internal Methods
   }
}
