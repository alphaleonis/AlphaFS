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
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Alphaleonis.Win32.Filesystem
{
   public static partial class File
   {
      /// <summary>
      ///   Backs up (export) encrypted files. This is one of a group of Encrypted File System (EFS) functions that is
      ///   intended to implement backup and restore functionality, while maintaining files in their encrypted state.
      /// </summary>
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
      /// <param name="output">The destination stream to which the backup data will be written.</param>
      /// <seealso cref="O:Alphaleonis.Win32.Filesystem.File.ImportEncryptedFileRaw"/>      
      public static void ExportEncryptedFileRaw(string fileName, System.IO.Stream output)
      {
         ExportEncryptedFileRaw(fileName, output, PathFormat.RelativePath);
      }

      /// <summary>
      ///   Backs up (export) encrypted files. This is one of a group of Encrypted File System (EFS) functions that is
      ///   intended to implement backup and restore functionality, while maintaining files in their encrypted state.
      /// </summary>
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
      /// <param name="output">The destination stream to which the backup data will be written.</param>
      /// <param name="pathFormat">The path format of the <paramref name="fileName"/> parameter.</param>
      /// <seealso cref="O:Alphaleonis.Win32.Filesystem.File.ImportEncryptedFileRaw"/>
      public static void ExportEncryptedFileRaw(string fileName, System.IO.Stream output, PathFormat pathFormat)
      {
         string lpPath = Path.GetExtendedLengthPathInternal(null, fileName, pathFormat, GetFullPathOptions.FullCheck | GetFullPathOptions.TrimEnd);
         SafeEncryptedFileRawHandle context = null;
         int errorCode = NativeMethods.OpenEncryptedFileRaw(lpPath, NativeMethods.EncryptedFileRawMode.CreateForExport, out context);
         try
         {
            if (errorCode != Win32Errors.ERROR_SUCCESS)
               NativeError.ThrowException(errorCode, fileName);

            errorCode = NativeMethods.ReadEncryptedFileRaw((IntPtr pbData, IntPtr pvCallbackContext, uint ulLength) =>
               {
                  try
                  {
                     byte[] data = new byte[ulLength];
                     Marshal.Copy(pbData, data, 0, (int)ulLength);
                     output.Write(data, 0, (int)ulLength);
                  }
                  catch (Exception ex)
                  {                     
                     return Marshal.GetHRForException(ex);
                  }

                  return (int)Win32Errors.ERROR_SUCCESS;
               }, IntPtr.Zero, context);

            if (errorCode != Win32Errors.ERROR_SUCCESS)
               NativeError.ThrowException(errorCode, fileName);
         }
         finally
         {
            if (context != null)
               context.Dispose();
         }
      }

      /// <summary>
      ///   Restores (import) encrypted files. This is one of a group of Encrypted File System (EFS) functions that is
      ///   intended to implement backup and restore functionality, while maintaining files in their encrypted state.
      /// </summary>
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
      /// <param name="stream">The stream to read previously backed up data from.</param>
      /// <param name="destinationFilePath">The path of the destination file to restore to.</param>
      /// <seealso cref="O:Alphaleonis.Win32.Filesystem.File.ExportEncryptedFileRaw"/>
      public static void ImportEncryptedFileRaw(System.IO.Stream stream, string destinationFilePath)
      {
         ImportEncryptedFileRaw(stream, destinationFilePath, PathFormat.RelativePath);
      }

      /// <summary>
      ///   Restores (import) encrypted files. This is one of a group of Encrypted File System (EFS) functions that is
      ///   intended to implement backup and restore functionality, while maintaining files in their encrypted state.
      /// </summary>
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
      public static void ImportEncryptedFileRaw(System.IO.Stream inputStream, string destinationFilePath, PathFormat pathFormat)
      {
         ImportEncryptedFileRaw(inputStream, destinationFilePath, false, pathFormat);
      }

      /// <summary>
      ///   Restores (import) encrypted files. This is one of a group of Encrypted File System (EFS) functions that is
      ///   intended to implement backup and restore functionality, while maintaining files in their encrypted state.
      /// </summary>
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
      public static void ImportEncryptedFileRaw(System.IO.Stream inputStream, string destinationFilePath, bool overwriteHidden)
      {
         ImportEncryptedFileRaw(inputStream, destinationFilePath, overwriteHidden, PathFormat.RelativePath);
      }

      /// <summary>
      ///   Restores (import) encrypted files. This is one of a group of Encrypted File System (EFS) functions that is
      ///   intended to implement backup and restore functionality, while maintaining files in their encrypted state.
      /// </summary>
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
      public static void ImportEncryptedFileRaw(System.IO.Stream inputStream, string destinationFilePath, bool overwriteHidden, PathFormat pathFormat)
      {
         ImportEncryptedFileRawCore(inputStream, destinationFilePath, pathFormat, NativeMethods.EncryptedFileRawMode.CreateForImport | (overwriteHidden ? NativeMethods.EncryptedFileRawMode.OverwriteHidden : 0));         
      }

      internal static void ImportEncryptedFileRawCore(System.IO.Stream inputStream, string destinationFilePath, PathFormat pathFormat, NativeMethods.EncryptedFileRawMode mode)
      {
         string lpPath = Path.GetExtendedLengthPathInternal(null, destinationFilePath, pathFormat, GetFullPathOptions.FullCheck | GetFullPathOptions.TrimEnd);
         SafeEncryptedFileRawHandle context = null;
         int errorCode = NativeMethods.OpenEncryptedFileRaw(lpPath, mode, out context);
         try
         {
            if (errorCode != Win32Errors.ERROR_SUCCESS)
               NativeError.ThrowException(errorCode, null, destinationFilePath);

            errorCode = NativeMethods.WriteEncryptedFileRaw((IntPtr pbData, IntPtr pvCallbackContext, ref uint ulLength) =>
            {
               try
               {
                  byte[] buffer = new byte[ulLength];
                  ulLength = (uint)inputStream.Read(buffer, 0, (int)ulLength);
                  if (ulLength == 0)
                  {
                     return (int)Win32Errors.ERROR_SUCCESS;
                  }

                  Marshal.Copy(buffer, 0, pbData, (int)ulLength);
               }
               catch (Exception ex)
               {
                  return Marshal.GetHRForException(ex);
               }

               return (int)Win32Errors.ERROR_SUCCESS;
            }, IntPtr.Zero, context);

            if (errorCode != Win32Errors.ERROR_SUCCESS)
               NativeError.ThrowException(errorCode, null, destinationFilePath);
         }
         finally
         {
            if (context != null)
               context.Dispose();
         }
      }
   }
}
