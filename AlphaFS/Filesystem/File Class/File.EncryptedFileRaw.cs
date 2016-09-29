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
using System.IO;
using System.Runtime.InteropServices;

namespace Alphaleonis.Win32.Filesystem
{
   partial class File
   {
      #region Export

      /// <summary>[AlphaFS] Backs up (export) encrypted files. This is one of a group of Encrypted File System (EFS) functions that is
      /// intended to implement backup and restore functionality, while maintaining files in their encrypted state.</summary>
      /// <remarks>
      ///   <para>
      ///      The file being backed up is not decrypted; it is backed up in its encrypted state.
      ///   </para>
      ///   <para>
      ///      If the caller does not have access to the key for the file, the caller needs
      ///      <see cref="Alphaleonis.Win32.Security.Privilege.Backup"/> to export encrypted files. See
      ///      <see cref="Alphaleonis.Win32.Security.PrivilegeEnabler"/>.
      ///   </para>
      ///   <para>
      ///      To backup an encrypted file call one of the
      ///      <see cref="O:Alphaleonis.Win32.Filesystem.File.ExportEncryptedFileRaw"/> overloads and specify the file to backup
      ///      along with the destination stream of the backup data.
      ///   </para>
      ///   <para>
      ///      This function is intended for the backup of only encrypted files; see <see cref="BackupFileStream"/> for backup
      ///      of unencrypted files.
      ///   </para>
      /// </remarks>
      /// <param name="fileName">The name of the file to be backed up.</param>
      /// <param name="outputStream">The destination stream to which the backup data will be written.</param>
      /// <seealso cref="O:Alphaleonis.Win32.Filesystem.File.ImportEncryptedFileRaw"/>      
      public static void ExportEncryptedFileRaw(string fileName, Stream outputStream)
      {
         ImportExportEncryptedFileDirectoryRawCore(true, false, outputStream, fileName, PathFormat.RelativePath, false);
      }

      /// <summary>[AlphaFS] Backs up (export) encrypted files. This is one of a group of Encrypted File System (EFS) functions that is
      ///   intended to implement backup and restore functionality, while maintaining files in their encrypted state.</summary>
      /// <remarks>
      ///   <para>
      ///      The file being backed up is not decrypted; it is backed up in its encrypted state.
      ///   </para>
      ///   <para>
      ///      If the caller does not have access to the key for the file, the caller needs
      ///      <see cref="Alphaleonis.Win32.Security.Privilege.Backup"/> to export encrypted files. See
      ///      <see cref="Alphaleonis.Win32.Security.PrivilegeEnabler"/>.
      ///   </para>
      ///   <para>
      ///      To backup an encrypted file call one of the
      ///      <see cref="O:Alphaleonis.Win32.Filesystem.File.ExportEncryptedFileRaw"/> overloads and specify the file to backup
      ///      along with the destination stream of the backup data.
      ///   </para>
      ///   <para>
      ///      This function is intended for the backup of only encrypted files; see <see cref="BackupFileStream"/> for backup
      ///      of unencrypted files.
      ///   </para>
      /// </remarks>
      /// <param name="fileName">The name of the file to be backed up.</param>
      /// <param name="outputStream">The destination stream to which the backup data will be written.</param>
      /// <param name="pathFormat">The path format of the <paramref name="fileName"/> parameter.</param>
      /// <seealso cref="O:Alphaleonis.Win32.Filesystem.File.ImportEncryptedFileRaw"/>
      public static void ExportEncryptedFileRaw(string fileName, Stream outputStream, PathFormat pathFormat)
      {
         ImportExportEncryptedFileDirectoryRawCore(true, false, outputStream, fileName, pathFormat, false);
      }

      #endregion // Export

      #region Import

      /// <summary>[AlphaFS] Restores (import) encrypted files. This is one of a group of Encrypted File System (EFS) functions that is
      ///   intended to implement backup and restore functionality, while maintaining files in their encrypted state.</summary>
      /// <remarks>
      ///   <para>
      ///     If the caller does not have access to the key for the file, the caller needs
      ///     <see cref="Alphaleonis.Win32.Security.Privilege.Backup"/> to restore encrypted files. See
      ///     <see cref="Alphaleonis.Win32.Security.PrivilegeEnabler"/>.
      ///   </para>
      ///   <para>
      ///     To restore an encrypted file call one of the
      ///     <see cref="O:Alphaleonis.Win32.Filesystem.File.ImportEncryptedFileRaw"/> overloads and specify the file to restore
      ///     along with the destination stream of the restored data.
      ///   </para>
      ///   <para>
      ///     This function is intended for the restoration of only encrypted files; see <see cref="BackupFileStream"/> for
      ///     backup of unencrypted files.
      ///   </para>
      /// </remarks>
      /// <param name="inputStream">The stream to read previously backed up data from.</param>
      /// <param name="destinationFilePath">The path of the destination file to restore to.</param>
      /// <seealso cref="O:Alphaleonis.Win32.Filesystem.File.ExportEncryptedFileRaw"/>
      public static void ImportEncryptedFileRaw(Stream inputStream, string destinationFilePath)
      {
         ImportExportEncryptedFileDirectoryRawCore(false, false, inputStream, destinationFilePath, PathFormat.RelativePath, false);
      }

      /// <summary>[AlphaFS] Restores (import) encrypted files. This is one of a group of Encrypted File System (EFS) functions that is
      ///   intended to implement backup and restore functionality, while maintaining files in their encrypted state.</summary>
      /// <remarks>
      ///   <para>
      ///     If the caller does not have access to the key for the file, the caller needs
      ///     <see cref="Alphaleonis.Win32.Security.Privilege.Backup"/> to restore encrypted files. See
      ///     <see cref="Alphaleonis.Win32.Security.PrivilegeEnabler"/>.
      ///   </para>
      ///   <para>
      ///     To restore an encrypted file call one of the
      ///     <see cref="O:Alphaleonis.Win32.Filesystem.File.ImportEncryptedFileRaw"/> overloads and specify the file to restore
      ///     along with the destination stream of the restored data.
      ///   </para>
      ///   <para>
      ///     This function is intended for the restoration of only encrypted files; see <see cref="BackupFileStream"/> for
      ///     backup of unencrypted files.
      ///   </para>
      /// </remarks>
      /// <param name="inputStream">The stream to read previously backed up data from.</param>
      /// <param name="destinationFilePath">The path of the destination file to restore to.</param>
      /// <param name="pathFormat">The path format of the <paramref name="destinationFilePath"/> parameter.</param>
      /// <seealso cref="O:Alphaleonis.Win32.Filesystem.File.ExportEncryptedFileRaw"/>
      public static void ImportEncryptedFileRaw(Stream inputStream, string destinationFilePath, PathFormat pathFormat)
      {
         ImportExportEncryptedFileDirectoryRawCore(false, false, inputStream, destinationFilePath, pathFormat, false);
      }


      /// <summary>[AlphaFS] Restores (import) encrypted files. This is one of a group of Encrypted File System (EFS) functions that is
      ///   intended to implement backup and restore functionality, while maintaining files in their encrypted state.</summary>
      /// <remarks>
      ///   <para>
      ///     If the caller does not have access to the key for the file, the caller needs
      ///     <see cref="Alphaleonis.Win32.Security.Privilege.Backup"/> to restore encrypted files. See
      ///     <see cref="Alphaleonis.Win32.Security.PrivilegeEnabler"/>.
      ///   </para>
      ///   <para>
      ///     To restore an encrypted file call one of the
      ///     <see cref="O:Alphaleonis.Win32.Filesystem.File.ImportEncryptedFileRaw"/> overloads and specify the file to restore
      ///     along with the destination stream of the restored data.
      ///   </para>
      ///   <para>
      ///     This function is intended for the restoration of only encrypted files; see <see cref="BackupFileStream"/> for
      ///     backup of unencrypted files.
      ///   </para>
      /// </remarks>
      /// <param name="inputStream">The stream to read previously backed up data from.</param>
      /// <param name="destinationFilePath">The path of the destination file to restore to.</param>
      /// <param name="overwriteHidden">If set to <see langword="true"/> a hidden file will be overwritten on import.</param>
      /// <seealso cref="O:Alphaleonis.Win32.Filesystem.File.ExportEncryptedFileRaw"/>
      public static void ImportEncryptedFileRaw(Stream inputStream, string destinationFilePath, bool overwriteHidden)
      {
         ImportExportEncryptedFileDirectoryRawCore(false, false, inputStream, destinationFilePath, PathFormat.RelativePath, overwriteHidden);
      }

      /// <summary>[AlphaFS] Restores (import) encrypted files. This is one of a group of Encrypted File System (EFS) functions that is
      ///   intended to implement backup and restore functionality, while maintaining files in their encrypted state.</summary>
      /// <remarks>
      ///   <para>
      ///     If the caller does not have access to the key for the file, the caller needs
      ///     <see cref="Alphaleonis.Win32.Security.Privilege.Backup"/> to restore encrypted files. See
      ///     <see cref="Alphaleonis.Win32.Security.PrivilegeEnabler"/>.
      ///   </para>
      ///   <para>
      ///     To restore an encrypted file call one of the
      ///     <see cref="O:Alphaleonis.Win32.Filesystem.File.ImportEncryptedFileRaw"/> overloads and specify the file to restore
      ///     along with the destination stream of the restored data.
      ///   </para>
      ///   <para>
      ///     This function is intended for the restoration of only encrypted files; see <see cref="BackupFileStream"/> for
      ///     backup of unencrypted files.
      ///   </para>
      /// </remarks>
      /// <param name="inputStream">The stream to read previously backed up data from.</param>
      /// <param name="destinationFilePath">The path of the destination file to restore to.</param>
      /// <param name="overwriteHidden">If set to <see langword="true"/> a hidden file will be overwritten on import.</param>
      /// <param name="pathFormat">The path format of the <paramref name="destinationFilePath"/> parameter.</param>
      /// <seealso cref="O:Alphaleonis.Win32.Filesystem.File.ExportEncryptedFileRaw"/>
      public static void ImportEncryptedFileRaw(Stream inputStream, string destinationFilePath, bool overwriteHidden, PathFormat pathFormat)
      {
         ImportExportEncryptedFileDirectoryRawCore(false, false, inputStream, destinationFilePath, pathFormat, overwriteHidden);
      }

      #endregion // Import




      internal static void ImportExportEncryptedFileDirectoryRawCore(bool isExport, bool isFolder, Stream stream, string destinationPath, PathFormat pathFormat, bool overwriteHidden)
      {
         string destinationPathLp = Path.GetExtendedLengthPathCore(null, destinationPath, pathFormat, GetFullPathOptions.FullCheck | GetFullPathOptions.TrimEnd);
         
         NativeMethods.EncryptedFileRawMode mode = isExport
            ? NativeMethods.EncryptedFileRawMode.CreateForExport
            : NativeMethods.EncryptedFileRawMode.CreateForImport;

         if (isFolder)
            mode = mode | NativeMethods.EncryptedFileRawMode.CreateForDir;

         if (overwriteHidden)
            mode = mode | NativeMethods.EncryptedFileRawMode.OverwriteHidden;


         // OpenEncryptedFileRaw()
         // In the ANSI version of this function, the name is limited to 248 characters.
         // To extend this limit to 32,767 wide characters, call the Unicode version of the function and prepend "\\?\" to the path.
         // 2015-08-02: MSDN does not confirm LongPath usage but a Unicode version of this function exists.

         SafeEncryptedFileRawHandle context;
         var lastError = NativeMethods.OpenEncryptedFileRaw(destinationPathLp, mode, out context);

         try
         {
            if (lastError != Win32Errors.ERROR_SUCCESS)
               NativeError.ThrowException(lastError, destinationPathLp);


            lastError = isExport
               ? NativeMethods.ReadEncryptedFileRaw((pbData, pvCallbackContext, length) =>
               {
                  try
                  {
                     var data = new byte[length];

                     Marshal.Copy(pbData, data, 0, (int) length);

                     stream.Write(data, 0, (int) length);
                  }
                  catch (Exception ex)
                  {
                     return Marshal.GetHRForException(ex) & NativeMethods.OverflowExceptionBitShift;
                  }

                  return (int) Win32Errors.ERROR_SUCCESS;

               }, IntPtr.Zero, context)


               : NativeMethods.WriteEncryptedFileRaw((IntPtr pbData, IntPtr pvCallbackContext, ref uint length) =>
               {
                  try
                  {
                     var data = new byte[length];

                     length = (uint) stream.Read(data, 0, (int) length);
                     if (length == 0)
                        return (int) Win32Errors.ERROR_SUCCESS;

                     Marshal.Copy(data, 0, pbData, (int) length);
                  }
                  catch (Exception ex)
                  {
                     return Marshal.GetHRForException(ex) & NativeMethods.OverflowExceptionBitShift;
                  }

                  return (int) Win32Errors.ERROR_SUCCESS;

               }, IntPtr.Zero, context);


            if (lastError != Win32Errors.ERROR_SUCCESS)
               NativeError.ThrowException(lastError, destinationPathLp);
         }
         finally
         {
            if (context != null)
               context.Dispose();
         }
      }
   }
}
