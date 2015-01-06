using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;

namespace Alphaleonis.Win32.Filesystem
{
   public static partial class File
   {
      #region GetCompressedSize

      /// <summary>[AlphaFS] Retrieves the actual number of bytes of disk storage used to store a specified file.</summary>
      /// <remarks>
      ///   If the file is located on a volume that supports compression and the file is compressed, the value obtained is the compressed size
      ///   of the specified file. If the file is located on a volume that supports sparse files and the file is a sparse file, the value
      ///   obtained is the sparse size of the specified file.
      /// </remarks>
      /// <param name="path"><para>The name of the file.</para></param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>The actual number of bytes of disk storage used to store the specified file.</returns>
      [SecurityCritical]
      public static long GetCompressedSize(string path, PathFormat pathFormat)
      {
         return GetCompressedSizeInternal(null, path, pathFormat);
      }

      /// <summary>[AlphaFS] Retrieves the actual number of bytes of disk storage used to store a specified file.</summary>
      /// <remarks>
      ///   If the file is located on a volume that supports compression and the file is compressed, the value obtained is the compressed size
      ///   of the specified file. If the file is located on a volume that supports sparse files and the file is a sparse file, the value
      ///   obtained is the sparse size of the specified file.
      /// </remarks>
      /// <param name="path"><para>The name of the file.</para></param>
      /// <returns>The actual number of bytes of disk storage used to store the specified file.</returns>
      [SecurityCritical]
      public static long GetCompressedSize(string path)
      {
         return GetCompressedSizeInternal(null, path, PathFormat.RelativeOrFullPath);
      }

      /// <summary>
      ///   [AlphaFS] Retrieves the actual number of bytes of disk storage used to store a specified file as part of a transaction. If the file
      ///   is located on a volume that supports compression and the file is compressed, the value obtained is the compressed size of the
      ///   specified file. If the file is located on a volume that supports sparse files and the file is a sparse file, the value obtained is
      ///   the sparse size of the specified file.
      /// </summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path"><para>The name of the file.</para></param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>The actual number of bytes of disk storage used to store the specified file.</returns>
      [SecurityCritical]
      public static long GetCompressedSize(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         return GetCompressedSizeInternal(transaction, path, pathFormat);
      }

      /// <summary>
      ///   [AlphaFS] Retrieves the actual number of bytes of disk storage used to store a specified file as part of a transaction. If the file
      ///   is located on a volume that supports compression and the file is compressed, the value obtained is the compressed size of the
      ///   specified file. If the file is located on a volume that supports sparse files and the file is a sparse file, the value obtained is
      ///   the sparse size of the specified file.
      /// </summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path"><para>The name of the file.</para></param>
      /// <returns>The actual number of bytes of disk storage used to store the specified file.</returns>
      [SecurityCritical]
      public static long GetCompressedSize(KernelTransaction transaction, string path)
      {
         return GetCompressedSizeInternal(transaction, path, PathFormat.RelativeOrFullPath);
      }

      #endregion // GetCompressedSize

      #region Internal Methods

      /// <summary>
      ///   [AlphaFS] Unified method GetCompressedSizeInternal() to retrieve the actual number of bytes of disk storage used to store a
      ///   specified file as part of a transaction. If the file is located on a volume that supports compression and the file is compressed,
      ///   the value obtained is the compressed size of the specified file. If the file is located on a volume that supports sparse files and
      ///   the file is a sparse file, the value obtained is the sparse size of the specified file.
      /// </summary>
      /// <exception cref="ArgumentNullException">Thrown when one or more required arguments are null.</exception>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path"><para>The name of the file.</para></param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>The actual number of bytes of disk storage used to store the specified file.</returns>      
      [SecurityCritical]
      internal static long GetCompressedSizeInternal(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         if (pathFormat != PathFormat.LongFullPath && Utils.IsNullOrWhiteSpace(path))
            throw new ArgumentNullException("path");

         string pathLp = Path.GetExtendedLengthPathInternal(transaction, path, pathFormat, GetFullPathOptions.RemoveTrailingDirectorySeparator | GetFullPathOptions.CheckInvalidPathChars | GetFullPathOptions.CheckAdditional);

         uint fileSizeHigh;
         uint fileSizeLow = transaction == null || !NativeMethods.IsAtLeastWindowsVista

            // GetCompressedFileSize() / GetCompressedFileSizeTransacted()
            // In the ANSI version of this function, the name is limited to 248 characters.
            // To extend this limit to 32,767 wide characters, call the Unicode version of the function and prepend "\\?\" to the path.
            // 2013-01-13: MSDN does not confirm LongPath usage but a Unicode version of this function exists.

            ? NativeMethods.GetCompressedFileSize(pathLp, out fileSizeHigh)
            : NativeMethods.GetCompressedFileSizeTransacted(pathLp, out fileSizeHigh, transaction.SafeHandle);

         // If the function fails, and lpFileSizeHigh is NULL, the return value is INVALID_FILE_SIZE.
         if (fileSizeLow == Win32Errors.ERROR_INVALID_FILE_SIZE && fileSizeHigh == 0)
            NativeError.ThrowException(Marshal.GetLastWin32Error(), pathLp);

         return NativeMethods.ToLong(fileSizeHigh, fileSizeLow);
      }

      #endregion // GetCompressedSizeInternal
   }
}
