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
      /// <exception cref="ArgumentException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="FileReadOnlyException"/>
      [SecurityCritical]
      public static void Delete(string path)
      {
         DeleteFileCore(null, path, false, PathFormat.RelativePath);
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
      /// <exception cref="ArgumentException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="FileReadOnlyException"/>
      [SecurityCritical]
      public static void Delete(string path, bool ignoreReadOnly, PathFormat pathFormat)
      {
         DeleteFileCore(null, path, ignoreReadOnly, pathFormat);
      }


      /// <summary>[AlphaFS] Deletes the specified file.</summary>
      /// <remarks>If the file to be deleted does not exist, no exception is thrown.</remarks>
      /// <param name="path">
      ///   The name of the file to be deleted. Wildcard characters are not supported.
      /// </param>
      /// <param name="ignoreReadOnly">
      ///   <see langword="true"/> overrides the read only <see cref="FileAttributes"/> of the file.
      /// </param>      
      /// <exception cref="ArgumentException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="FileReadOnlyException"/>
      [SecurityCritical]
      public static void Delete(string path, bool ignoreReadOnly)
      {
         DeleteFileCore(null, path, ignoreReadOnly, PathFormat.RelativePath);
      }

      #region Transactional

      /// <summary>[AlphaFS] Deletes the specified file.</summary>
      /// <remarks>If the file to be deleted does not exist, no exception is thrown.</remarks>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">
      ///   The name of the file to be deleted. Wildcard characters are not supported.
      /// </param>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="FileReadOnlyException"/>
      [SecurityCritical]
      public static void DeleteTransacted(KernelTransaction transaction, string path)
      {
         DeleteFileCore(transaction, path, false, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Deletes the specified file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The name of the file to be deleted. Wildcard characters are not supported.</param>
      /// <param name="ignoreReadOnly"><see langword="true"/> overrides the read only <see cref="FileAttributes"/> of the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <remarks>If the file to be deleted does not exist, no exception is thrown.</remarks>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="FileReadOnlyException"/>
      [SecurityCritical]
      public static void DeleteTransacted(KernelTransaction transaction, string path, bool ignoreReadOnly, PathFormat pathFormat)
      {
         DeleteFileCore(transaction, path, ignoreReadOnly, pathFormat);
      }

      /// <summary>[AlphaFS] Deletes the specified file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The name of the file to be deleted. Wildcard characters are not supported.</param>
      /// <param name="ignoreReadOnly"><see langword="true"/> overrides the read only <see cref="FileAttributes"/> of the file.</param>
      /// <remarks>If the file to be deleted does not exist, no exception is thrown.</remarks>      
      /// <exception cref="ArgumentException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="FileReadOnlyException"/>
      [SecurityCritical]
      public static void DeleteTransacted(KernelTransaction transaction, string path, bool ignoreReadOnly)
      {
         DeleteFileCore(transaction, path, ignoreReadOnly, PathFormat.RelativePath);
      }

      #endregion // Transacted

      #endregion // Delete

      #region Internal Methods

      /// <summary>Deletes a Non-/Transacted file.</summary>
      /// <remarks>If the file to be deleted does not exist, no exception is thrown.</remarks>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="FileReadOnlyException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The name of the file to be deleted.</param>
      /// <param name="ignoreReadOnly"><see langword="true"/> overrides the read only <see cref="FileAttributes"/> of the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      internal static void DeleteFileCore(KernelTransaction transaction, string path, bool ignoreReadOnly, PathFormat pathFormat)
      {
         #region Setup

         if (pathFormat == PathFormat.RelativePath)
            Path.CheckSupportedPathFormat(path, true, true);

         string pathLp = Path.GetExtendedLengthPathCore(transaction, path, pathFormat, GetFullPathOptions.TrimEnd | GetFullPathOptions.RemoveTrailingDirectorySeparator);

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
                  int dataInitialised = FillAttributeInfoCore(transaction, pathLp, ref data, false, true);

                  if (data.dwFileAttributes != (FileAttributes)(-1))
                  {
                     if ((data.dwFileAttributes & FileAttributes.Directory) != 0)
                        // MSDN: .NET 3.5+: UnauthorizedAccessException: Path is a directory.
                        throw new UnauthorizedAccessException(string.Format(CultureInfo.CurrentCulture, "({0}) {1}",
                           Win32Errors.ERROR_INVALID_PARAMETER, string.Format(CultureInfo.CurrentCulture, Resources.Target_File_Is_A_Directory, pathLp)));


                     if ((data.dwFileAttributes & FileAttributes.ReadOnly) != 0)
                     {
                        if (ignoreReadOnly)
                        {
                           // Reset file attributes.
                           SetAttributesCore(false, transaction, pathLp, FileAttributes.Normal, PathFormat.LongFullPath);
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

      #endregion // Internal Methods
   }
}
