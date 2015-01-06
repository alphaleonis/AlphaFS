using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;

namespace Alphaleonis.Win32.Filesystem
{
   public static partial class File
   {
      #region Delete

      /// <summary>Deletes the specified file.</summary>
      /// <remarks>If the file to be deleted does not exist, no exception is thrown.</remarks>
      /// <param name="path">
      ///   The name of the file to be deleted. Wildcard characters are not supported.
      /// </param>
      [SecurityCritical]
      public static void Delete(string path)
      {
         DeleteFileInternal(null, path, false, PathFormat.RelativeOrFullPath);
      }

      /// <summary>[AlphaFS] Deletes the specified file.</summary>
      /// <remarks>If the file to be deleted does not exist, no exception is thrown.</remarks>
      /// <param name="path">
      ///   The name of the file to be deleted. Wildcard characters are not supported.
      /// </param>
      /// <param name="ignoreReadOnly">
      ///   <see langword="true"/> overrides the read only <see cref="FileAttributes"/> of the file.
      /// </param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SecurityCritical]
      public static void Delete(string path, bool ignoreReadOnly, PathFormat pathFormat)
      {
         DeleteFileInternal(null, path, ignoreReadOnly, pathFormat);
      }


      /// <summary>[AlphaFS] Deletes the specified file.</summary>
      /// <remarks>If the file to be deleted does not exist, no exception is thrown.</remarks>
      /// <param name="path">
      ///   The name of the file to be deleted. Wildcard characters are not supported.
      /// </param>
      /// <param name="ignoreReadOnly">
      ///   <see langword="true"/> overrides the read only <see cref="FileAttributes"/> of the file.
      /// </param>      
      [SecurityCritical]
      public static void Delete(string path, bool ignoreReadOnly)
      {
         DeleteFileInternal(null, path, ignoreReadOnly, PathFormat.RelativeOrFullPath);
      }

      #region Transacted

      /// <summary>[AlphaFS] Deletes the specified file.</summary>
      /// <remarks>If the file to be deleted does not exist, no exception is thrown.</remarks>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">
      ///   The name of the file to be deleted. Wildcard characters are not supported.
      /// </param>      
      [SecurityCritical]
      public static void Delete(KernelTransaction transaction, string path)
      {
         DeleteFileInternal(transaction, path, false, PathFormat.RelativeOrFullPath);
      }

      /// <summary>[AlphaFS] Deletes the specified file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The name of the file to be deleted. Wildcard characters are not supported.</param>
      /// <param name="ignoreReadOnly"><see langword="true"/> overrides the read only <see cref="FileAttributes"/> of the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <remarks>If the file to be deleted does not exist, no exception is thrown.</remarks>
      [SecurityCritical]
      public static void Delete(KernelTransaction transaction, string path, bool ignoreReadOnly, PathFormat pathFormat)
      {
         DeleteFileInternal(transaction, path, ignoreReadOnly, pathFormat);
      }

      /// <summary>[AlphaFS] Deletes the specified file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The name of the file to be deleted. Wildcard characters are not supported.</param>
      /// <param name="ignoreReadOnly"><see langword="true"/> overrides the read only <see cref="FileAttributes"/> of the file.</param>
      /// <remarks>If the file to be deleted does not exist, no exception is thrown.</remarks>      
      [SecurityCritical]
      public static void Delete(KernelTransaction transaction, string path, bool ignoreReadOnly)
      {
         DeleteFileInternal(transaction, path, ignoreReadOnly, PathFormat.RelativeOrFullPath);
      }

      #endregion // Transacted

      #endregion // Delete

      #region Internal Methods

      /// <summary>[AlphaFS] Unified method DeleteFileInternal() to delete a Non-/Transacted file.</summary>
      /// <remarks>If the file to be deleted does not exist, no exception is thrown.</remarks>
      /// <exception cref="UnauthorizedAccessException">Thrown when an Unauthorized Access error condition occurs.</exception>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The name of the file to be deleted.</param>
      /// <param name="ignoreReadOnly"><see langword="true"/> overrides the read only <see cref="FileAttributes"/> of the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      internal static void DeleteFileInternal(KernelTransaction transaction, string path, bool ignoreReadOnly, PathFormat pathFormat)
      {
         #region Setup

         if (pathFormat == PathFormat.RelativeOrFullPath)
            Path.CheckValidPath(path, true, true);

         string pathLp = Path.GetExtendedLengthPathInternal(transaction, path, pathFormat, GetFullPathOptions.TrimEnd | GetFullPathOptions.RemoveTrailingDirectorySeparator);

         // If the path points to a symbolic link, the symbolic link is deleted, not the target.

         #endregion // Setup

      startDeleteFile:

         if (!(transaction == null || !NativeMethods.IsAtLeastWindowsVista

            // DeleteFile() / DeleteFileTransacted()
            // In the ANSI version of this function, the name is limited to MAX_PATH characters.
            // To extend this limit to 32,767 wide characters, call the Unicode version of the function and prepend "\\?\" to the path.
            // 2013-01-13: MSDN confirms LongPath usage.

            ? NativeMethods.DeleteFile(pathLp)
            : NativeMethods.DeleteFileTransacted(pathLp, transaction.SafeHandle)))
         {
            int lastError = Marshal.GetLastWin32Error();
            switch ((uint)lastError)
            {
               case Win32Errors.ERROR_FILE_NOT_FOUND:
                  // MSDN: .NET 3.5+: If the file to be deleted does not exist, no exception is thrown.
                  return;

               case Win32Errors.ERROR_PATH_NOT_FOUND:
                  // MSDN: .NET 3.5+: DirectoryNotFoundException: The specified path is invalid (for example, it is on an unmapped drive).
                  NativeError.ThrowException(lastError, pathLp);
                  return;

               case Win32Errors.ERROR_SHARING_VIOLATION:
                  // MSDN: .NET 3.5+: IOException: The specified file is in use or there is an open handle on the file.
                  NativeError.ThrowException(lastError, pathLp, true);
                  break;

               case Win32Errors.ERROR_ACCESS_DENIED:
                  var data = new NativeMethods.Win32FileAttributeData();
                  int dataInitialised = FillAttributeInfoInternal(transaction, pathLp, ref data, false, true);

                  if (data.FileAttributes != (FileAttributes)(-1))
                  {
                     if ((data.FileAttributes & FileAttributes.Directory) == FileAttributes.Directory)
                        // MSDN: .NET 3.5+: UnauthorizedAccessException: Path is a directory.
                        throw new UnauthorizedAccessException(string.Format(CultureInfo.CurrentCulture, "({0}) {1}",
                           Win32Errors.ERROR_INVALID_PARAMETER, string.Format(CultureInfo.CurrentCulture, Resources.DirectoryExistsWithSameNameSpecifiedByPath, pathLp)));


                     if ((data.FileAttributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                     {
                        if (ignoreReadOnly)
                        {
                           // Reset file attributes.
                           SetAttributesInternal(false, transaction, pathLp, FileAttributes.Normal, true, PathFormat.LongFullPath);
                           goto startDeleteFile;
                        }

                        // MSDN: .NET 3.5+: UnauthorizedAccessException: Path specified a read-only file.
                        throw new UnauthorizedAccessException(string.Format(CultureInfo.CurrentCulture, "({0}) {1}: [{2}]",
                           Win32Errors.ERROR_FILE_READ_ONLY, new Win32Exception((int)Win32Errors.ERROR_FILE_READ_ONLY).Message, pathLp));
                     }
                  }

                  if (dataInitialised == Win32Errors.ERROR_SUCCESS)
                     // MSDN: .NET 3.5+: UnauthorizedAccessException: The caller does not have the required permission.
                     NativeError.ThrowException(lastError, pathLp);

                  break;
            }

            // MSDN: .NET 3.5+: IOException:
            // The specified file is in use.
            // There is an open handle on the file, and the operating system is Windows XP or earlier.

            // Throws IOException.
            NativeError.ThrowException(lastError, pathLp, true);
         }
      }

      #endregion // DeleteFileInternal


   }
}
