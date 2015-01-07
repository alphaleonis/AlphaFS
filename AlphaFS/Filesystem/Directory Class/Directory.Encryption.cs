using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;

namespace Alphaleonis.Win32.Filesystem
{
   public static partial class Directory
   {
      #region Decrypt

      /// <summary>[AlphaFS] Decrypts a directory that was encrypted by the current account using the Encrypt method.</summary>
      /// <param name="path">A path that describes a directory to decrypt.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void Decrypt(string path, PathFormat pathFormat)
      {
         EncryptDecryptDirectoryInternal(path, false, false, pathFormat);
      }

      /// <summary>[AlphaFS] Decrypts a directory that was encrypted by the current account using the Encrypt method.</summary>
      /// <param name="path">A path that describes a directory to decrypt.</param>
      /// <param name="recursive"><see langword="true"/> to decrypt the directory recursively. <see langword="false"/> only decrypt files and directories in the root of <paramref name="path"/>.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void Decrypt(string path, bool recursive, PathFormat pathFormat)
      {
         EncryptDecryptDirectoryInternal(path, false, recursive, pathFormat);
      }

      /// <summary>[AlphaFS] Decrypts a directory that was encrypted by the current account using the Encrypt method.</summary>
      /// <param name="path">A path that describes a directory to decrypt.</param>
      [SecurityCritical]
      public static void Decrypt(string path)
      {
         EncryptDecryptDirectoryInternal(path, false, false, PathFormat.Relative);
      }

      /// <summary>[AlphaFS] Decrypts a directory that was encrypted by the current account using the Encrypt method.</summary>
      /// <param name="path">A path that describes a directory to decrypt.</param>
      /// <param name="recursive"><see langword="true"/> to decrypt the directory recursively. <see langword="false"/> only decrypt files and directories in the root of <paramref name="path"/>.</param>
      [SecurityCritical]
      public static void Decrypt(string path, bool recursive)
      {
         EncryptDecryptDirectoryInternal(path, false, recursive, PathFormat.Relative);
      }

      #endregion // Decrypt

      #region Encrypt

      /// <summary>[AlphaFS] Encrypts a directory so that only the account used to encrypt the directory can decrypt it.</summary>
      /// <param name="path">A path that describes a directory to encrypt.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void Encrypt(string path, PathFormat pathFormat)
      {
         EncryptDecryptDirectoryInternal(path, true, false, pathFormat);
      }

      /// <summary>[AlphaFS] Encrypts a directory so that only the account used to encrypt the directory can decrypt it.</summary>
      /// <param name="path">A path that describes a directory to encrypt.</param>
      /// <param name="recursive"><see langword="true"/> to encrypt the directory recursively. <see langword="false"/> only encrypt files and directories in the root of <paramref name="path"/>.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void Encrypt(string path, bool recursive, PathFormat pathFormat)
      {
         EncryptDecryptDirectoryInternal(path, true, recursive, pathFormat);
      }


      /// <summary>[AlphaFS] Encrypts a directory so that only the account used to encrypt the directory can decrypt it.</summary>
      /// <param name="path">A path that describes a directory to encrypt.</param>
      [SecurityCritical]
      public static void Encrypt(string path)
      {
         EncryptDecryptDirectoryInternal(path, true, false, PathFormat.Relative);
      }

      /// <summary>[AlphaFS] Encrypts a directory so that only the account used to encrypt the directory can decrypt it.</summary>
      /// <param name="path">A path that describes a directory to encrypt.</param>
      /// <param name="recursive"><see langword="true"/> to encrypt the directory recursively. <see langword="false"/> only encrypt files and directories in the root of <paramref name="path"/>.</param>
      [SecurityCritical]
      public static void Encrypt(string path, bool recursive)
      {
         EncryptDecryptDirectoryInternal(path, true, recursive, PathFormat.Relative);
      }

      #endregion // Encrypt

      #region DisableEncryption

      /// <summary>[AlphaFS] Disables encryption of the specified directory and the files in it. It does not affect encryption of subdirectories below the indicated directory.</summary>
      /// <param name="path">The name of the directory for which to disable encryption.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <remarks>This method will create/change the file "Desktop.ini" and wil set Encryption value: "Disable=0"</remarks>
      [SecurityCritical]
      public static void DisableEncryption(string path, PathFormat pathFormat)
      {
         EnableDisableEncryptionInternal(path, false, pathFormat);
      }

      /// <summary>[AlphaFS] Disables encryption of the specified directory and the files in it. It does not affect encryption of subdirectories below the indicated directory.</summary>
      /// <param name="path">The name of the directory for which to disable encryption.</param>
      /// <remarks>This method will create/change the file "Desktop.ini" and wil set Encryption value: "Disable=0"</remarks>
      [SecurityCritical]
      public static void DisableEncryption(string path)
      {
         EnableDisableEncryptionInternal(path, false, PathFormat.Relative);
      }

      #endregion // DisableEncryption

      #region EnableEncryption

      /// <summary>[AlphaFS] Enables encryption of the specified directory and the files in it. It does not affect encryption of subdirectories below the indicated directory.</summary>
      /// <param name="path">The name of the directory for which to enable encryption.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <remarks>This method will create/change the file "Desktop.ini" and wil set Encryption value: "Disable=1"</remarks>
      [SecurityCritical]
      public static void EnableEncryption(string path, PathFormat pathFormat)
      {
         EnableDisableEncryptionInternal(path, true, pathFormat);
      }

      /// <summary>[AlphaFS] Enables encryption of the specified directory and the files in it. It does not affect encryption of subdirectories below the indicated directory.</summary>
      /// <param name="path">The name of the directory for which to enable encryption.</param>
      /// <remarks>This method will create/change the file "Desktop.ini" and wil set Encryption value: "Disable=1"</remarks>
      [SecurityCritical]
      public static void EnableEncryption(string path)
      {
         EnableDisableEncryptionInternal(path, true, PathFormat.Relative);
      }

      #endregion // EnableEncryption

      #region Internal Methods

      /// <summary>[AlphaFS] Enables encryption of the specified directory and the files in it. It does not affect encryption of subdirectories below the indicated directory.</summary>
      /// <param name="path">The name of the directory for which to enable encryption.</param>
      /// <param name="enable"><see langword="true"/> enabled encryption, <see langword="false"/> disables encryption.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <remarks>This method will create/change the file "Desktop.ini" and wil set Encryption value: "Disable=0 | 1"</remarks>
      [SecurityCritical]
      internal static void EnableDisableEncryptionInternal(string path, bool enable, PathFormat pathFormat)
      {
         if (Utils.IsNullOrWhiteSpace(path))
            throw new ArgumentNullException("path");

         string pathLp = Path.GetExtendedLengthPathInternal(null, path, pathFormat, GetFullPathOptions.RemoveTrailingDirectorySeparator | GetFullPathOptions.CheckInvalidPathChars | GetFullPathOptions.CheckAdditional);

         // EncryptionDisable()
         // In the ANSI version of this function, the name is limited to 248 characters.
         // To extend this limit to 32,767 wide characters, call the Unicode version of the function and prepend "\\?\" to the path.
         // 2013-01-13: MSDN does not confirm LongPath usage but a Unicode version of this function exists.

         if (!NativeMethods.EncryptionDisable(pathLp, !enable))
            NativeError.ThrowException(pathLp);
      }

      /// <summary>[AlphaFS] Unified method EncryptDecryptFileInternal() to decrypt/encrypt a directory recursively so that only the account used to encrypt the directory can decrypt it.</summary>
      /// <param name="path">A path that describes a directory to encrypt.</param>
      /// <param name="encrypt"><see langword="true"/> encrypt, <see langword="false"/> decrypt.</param>
      /// <param name="recursive"><see langword="true"/> to decrypt the directory recursively. <see langword="false"/> only decrypt files and directories in the root of <paramref name="path"/>.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      internal static void EncryptDecryptDirectoryInternal(string path, bool encrypt, bool recursive, PathFormat pathFormat)
      {
         string pathLp = Path.GetExtendedLengthPathInternal(null, path, pathFormat, GetFullPathOptions.RemoveTrailingDirectorySeparator | GetFullPathOptions.CheckInvalidPathChars | GetFullPathOptions.CheckAdditional);

         var directoryEnumerationOptions = DirectoryEnumerationOptions.FilesAndFolders | DirectoryEnumerationOptions.AsLongPath;

         if (recursive)
            directoryEnumerationOptions |= DirectoryEnumerationOptions.Recursive;


         // Process folders and files.
         foreach (string fso in EnumerateFileSystemEntryInfosInternal<string>(null, pathLp, Path.WildcardStarMatchAll, directoryEnumerationOptions, PathFormat.LongFullPath))
            File.EncryptDecryptFileInternal(true, fso, encrypt, PathFormat.LongFullPath);

         // Process the root folder, the given path.
         File.EncryptDecryptFileInternal(true, pathLp, encrypt, PathFormat.LongFullPath);
      }

      #endregion // EncryptDecryptDirectoryInternal
   }
}
