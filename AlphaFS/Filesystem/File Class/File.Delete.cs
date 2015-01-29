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
using System.ComponentModel;
using System.Globalization;
using System.IO;
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
         DeleteFileInternal(null, path, false, PathFormat.RelativePath);
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
         DeleteFileInternal(null, path, ignoreReadOnly, PathFormat.RelativePath);
      }

      #region Transactional

      /// <summary>[AlphaFS] Deletes the specified file.</summary>
      /// <remarks>If the file to be deleted does not exist, no exception is thrown.</remarks>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">
      ///   The name of the file to be deleted. Wildcard characters are not supported.
      /// </param>      
      [SecurityCritical]
      public static void Delete(KernelTransaction transaction, string path)
      {
         DeleteFileInternal(transaction, path, false, PathFormat.RelativePath);
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
         DeleteFileInternal(transaction, path, ignoreReadOnly, PathFormat.RelativePath);
      }

      #endregion // Transacted

      #endregion // Delete

      #region Internal Methods

      /// <summary>Unified method DeleteFileInternal() to delete a Non-/Transacted file.</summary>
      /// <remarks>If the file to be deleted does not exist, no exception is thrown.</remarks>
      /// <exception cref="ArgumentException">
      ///   <para>Passed when the path parameter contains invalid characters, is empty, or contains only white spaces.</para>
      ///   <para>Path is prefixed with, or contains, only a colon character (:).</para>
      /// </exception>
      /// <exception cref="NotSupportedException">Path contains a colon character (:) that is not part of a drive label ("C:\").</exception>
      /// <exception cref="UnauthorizedAccessException">Thrown when an Unauthorized Access error condition occurs.</exception>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The name of the file to be deleted.</param>
      /// <param name="ignoreReadOnly"><see langword="true"/> overrides the read only <see cref="FileAttributes"/> of the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      internal static void DeleteFileInternal(KernelTransaction transaction, string path, bool ignoreReadOnly, PathFormat pathFormat)
      {
         #region Setup

         if (pathFormat == PathFormat.RelativePath)
            Path.CheckSupportedPathFormat(path, true, true);

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
                  NativeError.ThrowException(lastError, pathLp);
                  break;

               case Win32Errors.ERROR_ACCESS_DENIED:
                  var data = new NativeMethods.WIN32_FILE_ATTRIBUTE_DATA();
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
                        throw new FileReadOnlyException(pathLp);
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

            NativeError.ThrowException(lastError, pathLp);
         }
      }

      #endregion // DeleteFileInternal
   }
}
