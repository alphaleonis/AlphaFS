using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;

namespace Alphaleonis.Win32.Filesystem
{
   public static partial class File
   {
      #region Public Methods

      /// <summary>Decrypts a file that was encrypted by the current account using the Encrypt method.</summary>
      /// <param name="path">A path that describes a file to decrypt.</param>
      [SecurityCritical]
      public static void Decrypt(string path)
      {
         EncryptDecryptFileInternal(false, path, false, PathFormat.Auto);
      }

      /// <summary>[AlphaFS] Decrypts a file that was encrypted by the current account using the Encrypt method.</summary>
      /// <param name="path">A path that describes a file to decrypt.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void Decrypt(string path, PathFormat pathFormat)
      {
         EncryptDecryptFileInternal(false, path, false, pathFormat);
      }

      /// <summary>Encrypts a file so that only the account used to encrypt the file can decrypt it.</summary>
      /// <param name="path">A path that describes a file to encrypt.</param>
      [SecurityCritical]
      public static void Encrypt(string path)
      {
         EncryptDecryptFileInternal(false, path, true, PathFormat.Auto);
      }

      /// <summary>[AlphaFS] Encrypts a file so that only the account used to encrypt the file can decrypt it.</summary>
      /// <param name="path">A path that describes a file to encrypt.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SecurityCritical]
      public static void Encrypt(string path, PathFormat pathFormat)
      {
         EncryptDecryptFileInternal(false, path, true, pathFormat);
      }

      #endregion




      #region Internal Methods

      /// <summary>
      ///   [AlphaFS] Unified method EncryptDecryptFileInternal() to decrypt/encrypt a file or directory so that only the account used to
      ///   encrypt the file can decrypt it.
      /// </summary>
      /// <exception cref="NotSupportedException">Thrown when the requested operation is not supported.</exception>
      /// <param name="isFolder">Specifies that <paramref name="path"/> is a file or directory.</param>
      /// <param name="path">A path that describes a file to encrypt.</param>
      /// <param name="encrypt"><see langword="true"/> encrypt, <see langword="false"/> decrypt.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SecurityCritical]
      internal static void EncryptDecryptFileInternal(bool isFolder, string path, bool encrypt, PathFormat pathFormat)
      {
         string pathLp = Path.GetExtendedLengthPathInternal(null, path, pathFormat, GetFullPathOptions.RemoveTrailingDirectorySeparator | GetFullPathOptions.CheckInvalidPathChars | GetFullPathOptions.CheckAdditional);

         // Reset file/directory attributes.
         // MSDN: If lpFileName specifies a read-only file, the function fails and GetLastError returns ERROR_FILE_READ_ONLY.
         SetAttributesInternal(isFolder, null, pathLp, FileAttributes.Normal, true, PathFormat.ExtendedLength);

         // EncryptFile() / DecryptFile()
         // In the ANSI version of this function, the name is limited to 248 characters.
         // To extend this limit to 32,767 wide characters, call the Unicode version of the function and prepend "\\?\" to the path.
         // 2013-01-13: MSDN does not confirm LongPath usage but a Unicode version of this function exists.

         if (!(encrypt
            ? NativeMethods.EncryptFile(pathLp)
            : NativeMethods.DecryptFile(pathLp, 0)))
         {
            int lastError = Marshal.GetLastWin32Error();
            switch ((uint)lastError)
            {
               case Win32Errors.ERROR_ACCESS_DENIED:
                  string root = Path.GetPathRoot(pathLp, false);
                  if (!string.Equals("NTFS", new DriveInfo(root).DriveFormat, StringComparison.OrdinalIgnoreCase))
                     throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, "The drive does not support NTFS encryption: [{0}]", root));
                  break;

               default:
                  if (lastError == Win32Errors.ERROR_FILE_NOT_FOUND && isFolder)
                     lastError = (int)Win32Errors.ERROR_PATH_NOT_FOUND;

                  NativeError.ThrowException(lastError, pathLp);
                  break;
            }
         }
      }

      #endregion // EncryptDecryptFileInternal
   }
}
