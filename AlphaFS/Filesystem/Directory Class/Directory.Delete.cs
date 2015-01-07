using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;

namespace Alphaleonis.Win32.Filesystem
{
   public static partial class Directory
   {
      #region Delete

      /// <summary>Deletes an empty directory from a specified path.</summary>
      /// <param name="path">The name of the empty directory to remove. This directory must be writable and empty.</param>
      /// <remarks>MSDN: .NET 4+ Trailing spaces are removed from the end of the <paramref name="path"/> parameter before deleting the directory.</remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException ">path is <see langword="null"/>.</exception>
      [SecurityCritical]
      public static void Delete(string path)
      {
         DeleteDirectoryInternal(null, null, path, false, false, true, false, PathFormat.Relative);
      }

      /// <summary>Deletes the specified directory and, if indicated, any subdirectories in the directory.</summary>
      /// <param name="path">The name of the directory to remove.</param>
      /// <param name="recursive"><see langword="true"/> to remove directories, subdirectories, and files in <paramref name="path"/>. <see langword="false"/> otherwise.</param>
      /// <remarks>MSDN: .NET 4+ Trailing spaces are removed from the end of the <paramref name="path"/> parameter before deleting the directory.</remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException ">path is <see langword="null"/>.</exception>
      [SecurityCritical]
      public static void Delete(string path, bool recursive)
      {
         DeleteDirectoryInternal(null, null, path, recursive, false, !recursive, false, PathFormat.Relative);
      }

      /// <summary>[AlphaFS] Deletes the specified directory and, if indicated, any subdirectories in the directory.</summary>
      /// <param name="path">The name of the directory to remove.</param>
      /// <param name="recursive"><see langword="true"/> to remove directories, subdirectories, and files in <paramref name="path"/>. <see langword="false"/> otherwise.</param>
      /// <param name="ignoreReadOnly"><see langword="true"/> overrides read only <see cref="FileAttributes"/> of files and directories.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <remarks>MSDN: .NET 4+ Trailing spaces are removed from the end of the <paramref name="path"/> parameter before deleting the directory.</remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException ">path is <see langword="null"/>.</exception>
      [SecurityCritical]
      public static void Delete(string path, bool recursive, bool ignoreReadOnly, PathFormat pathFormat)
      {
         DeleteDirectoryInternal(null, null, path, recursive, ignoreReadOnly, !recursive, false, pathFormat);
      }

      /// <summary>[AlphaFS] Deletes the specified directory and, if indicated, any subdirectories in the directory.</summary>
      /// <param name="path">The name of the directory to remove.</param>
      /// <param name="recursive"><see langword="true"/> to remove directories, subdirectories, and files in <paramref name="path"/>. <see langword="false"/> otherwise.</param>
      /// <param name="ignoreReadOnly"><see langword="true"/> overrides read only <see cref="FileAttributes"/> of files and directories.</param>
      /// <remarks>MSDN: .NET 4+ Trailing spaces are removed from the end of the <paramref name="path"/> parameter before deleting the directory.</remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException ">path is <see langword="null"/>.</exception>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException ">path is <see langword="null"/>.</exception>
      [SecurityCritical]
      public static void Delete(string path, bool recursive, bool ignoreReadOnly)
      {
         DeleteDirectoryInternal(null, null, path, recursive, ignoreReadOnly, !recursive, false, PathFormat.Relative);
      }

      #region Transacted

      /// <summary>[AlphaFS] Deletes the specified directory and, if indicated, any subdirectories in the directory.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The name of the directory to remove.</param>
      /// <param name="recursive"><see langword="true"/> to remove directories, subdirectories, and files in <paramref name="path"/>. <see langword="false"/> otherwise.</param>
      /// <param name="ignoreReadOnly"><see langword="true"/> overrides read only <see cref="FileAttributes"/> of files and directories.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <remarks>MSDN: .NET 4+ Trailing spaces are removed from the end of the <paramref name="path"/> parameter before deleting the directory.</remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException ">path is <see langword="null"/>.</exception>
      [SecurityCritical]
      public static void Delete(KernelTransaction transaction, string path, bool recursive, bool ignoreReadOnly, PathFormat pathFormat)
      {
         DeleteDirectoryInternal(null, transaction, path, recursive, ignoreReadOnly, !recursive, false, pathFormat);
      }

      /// <summary>[AlphaFS] Deletes an empty directory from a specified path.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The name of the empty directory to remove. This directory must be writable and empty.</param>
      /// <remarks>MSDN: .NET 4+ Trailing spaces are removed from the end of the <paramref name="path"/> parameter before deleting the directory.</remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException ">path is <see langword="null"/>.</exception>
      [SecurityCritical]
      public static void Delete(KernelTransaction transaction, string path)
      {
         DeleteDirectoryInternal(null, transaction, path, false, false, true, false, PathFormat.Relative);
      }

      /// <summary>[AlphaFS] Deletes the specified directory and, if indicated, any subdirectories in the directory.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The name of the directory to remove.</param>
      /// <param name="recursive"><see langword="true"/> to remove directories, subdirectories, and files in <paramref name="path"/>. <see langword="false"/> otherwise.</param>
      /// <remarks>MSDN: .NET 4+ Trailing spaces are removed from the end of the <paramref name="path"/> parameter before deleting the directory.</remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException ">path is <see langword="null"/>.</exception>
      [SecurityCritical]
      public static void Delete(KernelTransaction transaction, string path, bool recursive)
      {
         DeleteDirectoryInternal(null, transaction, path, recursive, false, !recursive, false, PathFormat.Relative);
      }


      /// <summary>[AlphaFS] Deletes the specified directory and, if indicated, any subdirectories in the directory.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The name of the directory to remove.</param>
      /// <param name="recursive"><see langword="true"/> to remove directories, subdirectories, and files in <paramref name="path"/>. <see langword="false"/> otherwise.</param>
      /// <param name="ignoreReadOnly"><see langword="true"/> overrides read only <see cref="FileAttributes"/> of files and directories.</param>
      /// <remarks>MSDN: .NET 4+ Trailing spaces are removed from the end of the <paramref name="path"/> parameter before deleting the directory.</remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException ">path is <see langword="null"/>.</exception>
      [SecurityCritical]
      public static void Delete(KernelTransaction transaction, string path, bool recursive, bool ignoreReadOnly)
      {
         DeleteDirectoryInternal(null, transaction, path, recursive, ignoreReadOnly, !recursive, false, PathFormat.Relative);
      }

      #endregion // Transacted


      #endregion // Delete

      #region Internal Methods

      /// <summary>[AlphaFS] Unified method DeleteDirectoryInternal() to delete a Non-/Transacted directory.</summary>
      /// <remarks>
      /// <para>The RemoveDirectory function marks a directory for deletion on close. Therefore, the directory is not removed until the last handle to the directory is closed.</para>
      /// <para>MSDN: .NET 4+ Trailing spaces are removed from the end of the <paramref name="path"/> parameter before deleting the directory.</para>
      /// </remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException ">path is <see langword="null"/>.</exception>
      /// <param name="fileSystemEntryInfo">A FileSystemEntryInfo instance. Use either <paramref name="fileSystemEntryInfo"/> or <paramref name="path"/>, not both.</param>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The name of the directory to remove. Use either <paramref name="path"/> or <paramref name="fileSystemEntryInfo"/>, not both.</param>
      /// <param name="recursive"><see langword="true"/> to remove all files and subdirectories recursively; <see langword="false"/> otherwise only the top level empty directory.</param>
      /// <param name="ignoreReadOnly"><see langword="true"/> overrides read only attribute of files and directories.</param>
      /// <param name="requireEmpty"><see langword="true"/> requires the the directory must be empty.</param>
      /// <param name="continueOnNotExist"><see langword="true"/> does not throw an Exception when the file system object does not exist.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
      [SecurityCritical]
      internal static void DeleteDirectoryInternal(FileSystemEntryInfo fileSystemEntryInfo, KernelTransaction transaction, string path, bool recursive, bool ignoreReadOnly, bool requireEmpty, bool continueOnNotExist, PathFormat pathFormat)
      {
         #region Setup

         if (pathFormat == PathFormat.Relative)
            Path.CheckValidPath(path, true, true);

         if (fileSystemEntryInfo == null)
         {
            // MSDN: .NET 3.5+: DirectoryNotFoundException:
            // Path does not exist or could not be found.
            // Path refers to a file instead of a directory.
            // The specified path is invalid (for example, it is on an unmapped drive). 

            fileSystemEntryInfo = File.GetFileSystemEntryInfoInternal(transaction,
               Path.GetExtendedLengthPathInternal(transaction, path, pathFormat, GetFullPathOptions.TrimEnd | GetFullPathOptions.RemoveTrailingDirectorySeparator),
                  continueOnNotExist, pathFormat);
         }

         if (fileSystemEntryInfo == null)
            return;

         string pathLp = fileSystemEntryInfo.LongFullPath;

         #endregion // Setup

         // Do not follow mount points nor symbolic links, but do delete the reparse point itself.

         // If directory is reparse point, disable recursion.
         if (recursive && fileSystemEntryInfo.IsReparsePoint)
            recursive = false;


         // Check to see if this is a mount point, and unmount it.
         if (fileSystemEntryInfo.IsMountPoint)
         {
            int lastError = Volume.DeleteVolumeMountPointInternal(pathLp, true);

            if (lastError != Win32Errors.ERROR_SUCCESS && lastError != Win32Errors.ERROR_PATH_NOT_FOUND)
               NativeError.ThrowException(lastError, pathLp);

            // Now it is safe to delete the actual directory.
         }


         if (recursive)
         {
            // Enumerate all file system objects.
            foreach (FileSystemEntryInfo fsei in EnumerateFileSystemEntryInfosInternal<FileSystemEntryInfo>(transaction, pathLp, Path.WildcardStarMatchAll, DirectoryEnumerationOptions.FilesAndFolders, PathFormat.LongFullPath))
            {
               if (fsei.IsDirectory)
                  DeleteDirectoryInternal(fsei, transaction, null, true, ignoreReadOnly, requireEmpty, true, PathFormat.LongFullPath);
               else
                  File.DeleteFileInternal(transaction, fsei.LongFullPath, ignoreReadOnly, PathFormat.LongFullPath);
            }
         }

         #region Remove

      startRemoveDirectory:

         if (!(transaction == null || !NativeMethods.IsAtLeastWindowsVista

            // RemoveDirectory() / RemoveDirectoryTransacted()
            // In the ANSI version of this function, the name is limited to MAX_PATH characters.
            // To extend this limit to 32,767 wide characters, call the Unicode version of the function and prepend "\\?\" to the path.
            // 2014-09-09: MSDN confirms LongPath usage.

            // RemoveDirectory on a symbolic link will remove the link itself.

            ? NativeMethods.RemoveDirectory(pathLp)
            : NativeMethods.RemoveDirectoryTransacted(pathLp, transaction.SafeHandle)))
         {
            int lastError = Marshal.GetLastWin32Error();
            switch ((uint)lastError)
            {
               case Win32Errors.ERROR_DIR_NOT_EMPTY:
                  if (requireEmpty)
                     // MSDN: .NET 3.5+: IOException: The directory specified by path is read-only, or recursive is false and path is not an empty directory. 
                     NativeError.ThrowException(lastError, pathLp, true);

                  goto startRemoveDirectory;


               case Win32Errors.ERROR_DIRECTORY:
                  // MSDN: .NET 3.5+: DirectoryNotFoundException: Path refers to a file instead of a directory.
                  if (File.ExistsInternal(false, transaction, pathLp, PathFormat.LongFullPath))
                     throw new DirectoryNotFoundException(String.Format(CultureInfo.CurrentCulture, "({0}) {1}",
                        Win32Errors.ERROR_INVALID_PARAMETER, String.Format(CultureInfo.CurrentCulture, Resources.FileExistsWithSameNameSpecifiedByPath, pathLp)));
                  break;


               case Win32Errors.ERROR_PATH_NOT_FOUND:
                  if (continueOnNotExist)
                     return;
                  break;

               case Win32Errors.ERROR_SHARING_VIOLATION:
                  // MSDN: .NET 3.5+: IOException: The directory is being used by another process or there is an open handle on the directory.
                  NativeError.ThrowException(lastError, pathLp, true);
                  break;

               case Win32Errors.ERROR_ACCESS_DENIED:
                  NativeMethods.Win32FileAttributeData data = new NativeMethods.Win32FileAttributeData();
                  int dataInitialised = File.FillAttributeInfoInternal(transaction, pathLp, ref data, false, true);

                  if (data.FileAttributes != (FileAttributes)(-1))
                  {
                     if ((data.FileAttributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                     {
                        // MSDN: .NET 3.5+: IOException: The directory specified by path is read-only, or recursive is false and path is not an empty directory.

                        if (ignoreReadOnly)
                        {
                           // Reset directory attributes.
                           File.SetAttributesInternal(true, transaction, pathLp, FileAttributes.Normal, true, PathFormat.LongFullPath);
                           goto startRemoveDirectory;
                        }

                        // MSDN: .NET 3.5+: IOException: The directory is read-only or contains a read-only file.
                        NativeError.ThrowException(Win32Errors.ERROR_FILE_READ_ONLY, pathLp, true);
                     }
                  }

                  if (dataInitialised == Win32Errors.ERROR_SUCCESS)
                     // MSDN: .NET 3.5+: UnauthorizedAccessException: The caller does not have the required permission.
                     NativeError.ThrowException(lastError, pathLp);

                  break;
            }

            // MSDN: .NET 3.5+: IOException:
            // A file with the same name and location specified by path exists.
            // The directory specified by path is read-only, or recursive is false and path is not an empty directory. 
            // The directory is the application's current working directory. 
            // The directory contains a read-only file.
            // The directory is being used by another process.

            // Throws IOException.
            NativeError.ThrowException(lastError, pathLp, true);
         }

         #endregion // Remove
      }

      #endregion // DeleteDirectoryInternal
   }
}
