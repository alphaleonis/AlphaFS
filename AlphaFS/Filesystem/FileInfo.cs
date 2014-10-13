/* Copyright (c) 2008-2014 Peter Palotas, Alexandr Normuradov, Jeffrey Jangli
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
using System.IO;
using System.Security;
using System.Security.AccessControl;
using System.Text;

namespace Alphaleonis.Win32.Filesystem
{
   /// <summary>Provides properties and instance methods for the creation, copying, deletion, moving, and opening of files, and aids in the creation of <see cref="T:FileStream"/> objects. This class cannot be inherited.</summary>
   [SerializableAttribute]
   public sealed class FileInfo : FileSystemInfo
   {
      #region Constructors

      #region FileInfo

      #region .NET

      /// <summary>Initializes a new instance of the <see cref="T:Alphaleonis.Win32.Filesystem.FileInfo"/> class, which acts as a wrapper for a file path.</summary>
      /// <param name="fileName">The fully qualified name of the new file, or the relative file name. Do not end the path with the directory separator character.</param>
      /// <remarks>This constructor does not check if a file exists. This constructor is a placeholder for a string that is used to access the file in subsequent operations.</remarks>
      public FileInfo(string fileName) : this(null, fileName, false)
      {
      }

      #endregion // .NET

      #region AlphaFS

      /// <summary>[AlphaFS] Initializes a new instance of the <see cref="T:Alphaleonis.Win32.Filesystem.FileInfo"/> class, which acts as a wrapper for a file path.</summary>
      /// <param name="fileName">The fully qualified name of the new file, or the relative file name. Do not end the path with the directory separator character.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="fileName"/> will be normalized and long path prefixed. <c>null</c> <paramref name="fileName"/> is already a full path with long path prefix, will be used as is.</param>
      /// <remarks>This constructor does not check if a file exists. This constructor is a placeholder for a string that is used to access the file in subsequent operations.</remarks>
      public FileInfo(string fileName, bool? isFullPath) : this(null, fileName, isFullPath)
      {
      }

      #region Transacted

      /// <summary>[AlphaFS] Initializes a new instance of the <see cref="T:Alphaleonis.Win32.Filesystem.FileInfo"/> class, which acts as a wrapper for a file path.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="fileName">The fully qualified name of the new file, or the relative file name. Do not end the path with the directory separator character.</param>
      /// <remarks>This constructor does not check if a file exists. This constructor is a placeholder for a string that is used to access the file in subsequent operations.</remarks>
      public FileInfo(KernelTransaction transaction, string fileName) : this(transaction, fileName, false)
      {
      }

      /// <summary>[AlphaFS] Initializes a new instance of the <see cref="T:Alphaleonis.Win32.Filesystem.FileInfo"/> class, which acts as a wrapper for a file path.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="fileName">The fully qualified name of the new file, or the relative file name. Do not end the path with the directory separator character.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="fileName"/> will be normalized and long path prefixed. <c>null</c> <paramref name="fileName"/> is already a full path with long path prefix, will be used as is.</param>
      /// <remarks>This constructor does not check if a file exists. This constructor is a placeholder for a string that is used to access the file in subsequent operations.</remarks>
      public FileInfo(KernelTransaction transaction, string fileName, bool? isFullPath)
      {
         InitializeInternal(false, transaction, fileName, isFullPath);

         _name = Path.GetFileName(Path.RemoveDirectorySeparator(fileName, false), isFullPath != null && (bool) isFullPath);
      }

      #endregion // Transacted

      #endregion // AlphaFS

      #endregion // FileInfo

      #endregion // Constructors
      
      #region Methods

      #region .NET

      #region AppendText

      #region .NET

      /// <summary>Creates a <see cref="T:System.IO.StreamWriter"/> that appends text to the file represented by this instance of the <see cref="T:FileInfo"/>.</summary>
      /// <returns>A new <see cref="T:StreamWriter"/></returns>
      [SecurityCritical]
      public StreamWriter AppendText()
      {
         return File.AppendTextInternal(Transaction, LongFullName, NativeMethods.DefaultFileEncoding, null);
      }

      /// <summary>Creates a <see cref="T:StreamWriter"/> that appends text to the file represented by this instance of the <see cref="T:FileInfo"/>.</summary>
      /// <param name="encoding">The character <see cref="T:Encoding"/> to use.</param>
      /// <returns>A new <see cref="T:StreamWriter"/></returns>
      [SecurityCritical]
      public StreamWriter AppendText(Encoding encoding)
      {
         return File.AppendTextInternal(Transaction, LongFullName, encoding, null);
      }

      #endregion // .NET

      #endregion // AppendText

      #region CopyTo

      #region .NET

      /// <summary>Copies an existing file to a new file, disallowing the overwriting of an existing file.</summary>
      /// <param name="destFileName">The name of the new file to copy to.</param>
      /// <returns>A new file with a fully qualified path.</returns>
      /// <remarks>Use the CopyTo method to allow overwriting of an existing file.</remarks>
      /// <remarks>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method. If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "dest")]
      [SecurityCritical]
      public FileInfo CopyTo(string destFileName)
      {
         return CopyToMoveToInternal(false, destFileName, NativeMethods.CopyOptionsFail, null, null, null, false);
      }

      /// <summary>Copies an existing file to a new file, allowing the overwriting of an existing file.</summary>
      /// <param name="destFileName">The name of the new file to copy to.</param>
      /// <param name="overwrite"><c>true</c> to allow an existing file to be overwritten; otherwise, <c>false</c>.</param>
      /// <returns>A new file, or an overwrite of an existing file if <paramref name="overwrite"/> is <c>true</c>. If the file exists and <paramref name="overwrite"/> is <c>false</c>, an <see cref="T:System.IO.IOException"/> is thrown.</returns>
      /// <remarks>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method. If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "dest")]
      [SecurityCritical]
      public FileInfo CopyTo(string destFileName, bool overwrite)
      {
         return CopyToMoveToInternal(false, destFileName, overwrite ? NativeMethods.CopyOptionsNone : NativeMethods.CopyOptionsFail, null, null, null, false);
      }

      #endregion // .NET

      #region AlphaFS

      /// <summary>[AlphaFS] Copies an existing file to a new file, allowing the overwriting of an existing file.</summary>
      /// <param name="destFileName">The name of the new file to copy to.</param>
      /// <param name="overwrite"><c>true</c> to allow an existing file to be overwritten; otherwise, <c>false</c>.</param>
      /// <param name="copyProgress"><para>This parameter can be <c>null</c>. A callback function that is called each time another portion of the file has been copied.</para></param>
      /// <param name="userProgressData"><para>This parameter can be <c>null</c>. The argument to be passed to the callback function.</para></param>
      /// <returns>A new file, or an overwrite of an existing file if <paramref name="overwrite"/> is <c>true</c>. If the file exists and <paramref name="overwrite"/> is <c>false</c>, an <see cref="T:System.IO.IOException"/> is thrown.</returns>
      /// <remarks>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method. If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "dest")]
      [SecurityCritical]
      public FileInfo CopyTo(string destFileName, bool overwrite, CopyMoveProgressCallback copyProgress, object userProgressData)
      {
         return CopyToMoveToInternal(false, destFileName, overwrite ? NativeMethods.CopyOptionsNone : NativeMethods.CopyOptionsFail, null, copyProgress, userProgressData, false);
      }

      #endregion // AlphaFS

      #endregion // CopyTo

      #region Create

      #region .NET

      /// <summary>Creates a file.</summary>
      /// <returns><see cref="T:FileStream"/>A new file.</returns>
      [SecurityCritical]
      public FileStream Create()
      {
         return File.CreateFileInternal(Transaction, LongFullName, NativeMethods.DefaultFileBufferSize, EFileAttributes.Normal, null, FileMode.Create, FileAccess.ReadWrite, FileShare.None, null);
      }

      #endregion // .NET

      #endregion // Create

      #region CreateText

      #region .NET

      /// <summary>Creates a <see crefe="StreamWriter"/> instance that writes a new text file.</summary>
      /// <returns>A new <see cref="T:StreamWriter"/></returns>
      [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
      [SecurityCritical]
      public StreamWriter CreateText()
      {
         return new StreamWriter(File.CreateFileInternal(Transaction, LongFullName, NativeMethods.DefaultFileBufferSize, EFileAttributes.Normal, null, FileMode.Create, FileAccess.ReadWrite, FileShare.None, null), NativeMethods.DefaultFileEncoding);
      }

      #endregion // .NET

      #endregion // CreateText

      #region Decrypt

      #region .NET

      /// <summary>Decrypts a file that was encrypted by the current account using the Encrypt method.</summary>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public void Decrypt()
      {
         File.EncryptDecryptFileInternal(false, LongFullName, false, null);
      }

      #endregion // .NET

      #endregion // Decrypt

      #region Delete

      #region .NET

      /// <summary>Permanently deletes a file.</summary>
      /// <remarks>If the file does not exist, this method does nothing.</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      public override void Delete()
      {
         File.DeleteFileInternal(Transaction, LongFullName, false, null);
         Reset();
      }

      #endregion // .NET

      #region AlphaFS

      /// <summary>[AlphaFS] Permanently deletes a file.</summary>
      /// <param name="ignoreReadOnly"><c>true</c> overrides the read only <see cref="T:FileAttributes"/> of the file.</param>
      /// <remarks>If the file does not exist, this method does nothing.</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      public void Delete(bool ignoreReadOnly)
      {
         File.DeleteFileInternal(Transaction, LongFullName, ignoreReadOnly, null);
         Reset();
      }

      #endregion // AlphaFS

      #endregion // Delete

      #region Encrypt

      #region .NET

      /// <summary>Encrypts a file so that only the account used to encrypt the file can decrypt it.</summary>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public void Encrypt()
      {
         File.EncryptDecryptFileInternal(false, LongFullName, true, null);
      }

      #endregion // .NET

      #endregion // Encrypt

      #region GetAccessControl

      #region .NET

      /// <summary>Gets a <see cref="T:System.Security.AccessControl.FileSecurity"/> object that encapsulates the access control list (ACL) entries for the file described by the current <see cref="T:FileInfo"/> object.</summary>
      /// <returns><see cref="T:System.Security.AccessControl.FileSecurity"/>A FileSecurity object that encapsulates the access control rules for the current file.</returns>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public FileSecurity GetAccessControl()
      {
         return File.GetAccessControlInternal<FileSecurity>(false, LongFullName, AccessControlSections.Access | AccessControlSections.Group | AccessControlSections.Owner, null);
      }

      /// <summary>Gets a <see cref="T:System.Security.AccessControl.FileSecurity"/> object that encapsulates the specified type of access control list (ACL) entries for the file described by the current FileInfo object.</summary>
      /// <param name="includeSections">One of the <see cref="T:System.Security"/> values that specifies which group of access control entries to retrieve.</param>
      /// <returns><see cref="T:System.Security.AccessControl.FileSecurity"/> object that encapsulates the specified type of access control list (ACL) entries for the file described by the current FileInfo object.</returns>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public FileSecurity GetAccessControl(AccessControlSections includeSections)
      {
         return File.GetAccessControlInternal<FileSecurity>(false, LongFullName, includeSections, null);
      }

      #endregion // .NET

      #endregion // GetAccessControl

      #region MoveTo

      #region .NET

      /// <summary>Moves a specified file to a new location, providing the option to specify a new file name.</summary>
      /// <param name="destFileName">The path to move the file to, which can specify a different file name.</param>
      /// <remarks>This method works across disk volumes.</remarks>
      /// <remarks>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method. If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "dest")]
      [SecurityCritical]
      public void MoveTo(string destFileName)
      {
         CopyToMoveToInternal(true, destFileName, null, NativeMethods.MoveOptionsReplace, null, null, false);
      }

      #endregion // .NET

      #region AlphaFS

      /// <summary>[AlphaFS] Moves a FileyInfo instance and its contents to a new path.</summary>
      /// <param name="destFileName">The path to the new location for sourceFileName.</param>
      /// <param name="moveProgress"><para>This parameter can be <c>null</c>. A callback function that is called each time another portion of the file has been moved.</para></param>
      /// <param name="userProgressData"><para>This parameter can be <c>null</c>. The argument to be passed to the callback function.</para></param>
      /// <remarks>This method works across disk volumes.</remarks>
      /// <remarks>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method. If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "dest")]
      [SecurityCritical]
      public void MoveTo(string destFileName, CopyMoveProgressCallback moveProgress, object userProgressData)
      {
         CopyToMoveToInternal(true, destFileName, null, NativeMethods.MoveOptionsReplace, moveProgress, userProgressData, false);
      }

      #endregion // AlphaFS

      #endregion // MoveTo

      #region Open

      #region .NET

      /// <summary>Opens a file in the specified mode.</summary>
      /// <param name="mode">A <see cref="T:FileMode"/> constant specifying the mode (for example, Open or Append) in which to open the file.</param>
      /// <returns>A <see cref="T:FileStream"/> file opened in the specified mode, with read/write access and unshared.</returns>
      [SecurityCritical]
      public FileStream Open(FileMode mode)
      {
         return File.OpenInternal(Transaction, LongFullName, mode, 0, FileAccess.Read, FileShare.None, EFileAttributes.Normal, null);
      }

      /// <summary>Opens a file in the specified mode with read, write, or read/write access.</summary>
      /// <param name="mode">A <see cref="T:FileMode"/> constant specifying the mode (for example, Open or Append) in which to open the file.</param>
      /// <param name="access">A <see cref="T:FileAccess"/> constant specifying whether to open the file with Read, Write, or ReadWrite file access.</param>
      /// <returns>A <see cref="T:FileStream"/> object opened in the specified mode and access, and unshared.</returns>
      [SecurityCritical]
      public FileStream Open(FileMode mode, FileAccess access)
      {
         return File.OpenInternal(Transaction, LongFullName, mode, 0, access, FileShare.None, EFileAttributes.Normal, null);
      }

      /// <summary>Opens a file in the specified mode with read, write, or read/write access and the specified sharing option.</summary>
      /// <param name="mode">A <see cref="T:FileMode"/> constant specifying the mode (for example, Open or Append) in which to open the file.</param>
      /// <param name="access">A <see cref="T:FileAccess"/> constant specifying whether to open the file with Read, Write, or ReadWrite file access.</param>
      /// <param name="share">A <see cref="T:FileShare"/> constant specifying the type of access other <see cref="T:FileStream"/> objects have to this file.</param>
      /// <returns>A <see cref="T:FileStream"/> object opened with the specified mode, access, and sharing options.</returns>
      [SecurityCritical]
      public FileStream Open(FileMode mode, FileAccess access, FileShare share)
      {
         return File.OpenInternal(Transaction, LongFullName, mode, 0, access, share, EFileAttributes.Normal, null);
      }

      #endregion // .NET

      #region AlphaFS

      /// <summary>[AlphaFS] Opens a file in the specified mode with read, write, or read/write access.</summary>
      /// <param name="mode">A <see cref="T:FileMode"/> constant specifying the mode (for example, Open or Append) in which to open the file.</param>
      /// <param name="rights">A <see cref="T:FileSystemRights"/> value that specifies whether a file is created if one does not exist, and determines whether the contents of existing files are retained or overwritten along with additional options.</param>
      /// <returns>A <see cref="T:FileStream"/> object opened in the specified mode and access, and unshared.</returns>
      [SecurityCritical]
      public FileStream Open(FileMode mode, FileSystemRights rights)
      {
         return File.OpenInternal(Transaction, LongFullName, mode, rights, 0, FileShare.None, EFileAttributes.Normal, null);
      }

      /// <summary>[AlphaFS] Opens a file in the specified mode with read, write, or read/write access and the specified sharing option.</summary>
      /// <param name="mode">A <see cref="T:FileMode"/> constant specifying the mode (for example, Open or Append) in which to open the file.</param>
      /// <param name="rights">A <see cref="T:FileSystemRights"/> value that specifies whether a file is created if one does not exist, and determines whether the contents of existing files are retained or overwritten along with additional options.</param>
      /// <param name="share">A <see cref="T:FileShare"/> constant specifying the type of access other <see cref="T:FileStream"/> objects have to this file.</param>
      /// <returns>A <see cref="T:FileStream"/> object opened with the specified mode, access, and sharing options.</returns>
      [SecurityCritical]
      public FileStream Open(FileMode mode, FileSystemRights rights, FileShare share)
      {
         return File.OpenInternal(Transaction, LongFullName, mode, rights, 0, share, EFileAttributes.Normal, null);
      }

      #endregion // AlphaFS

      #endregion // Open

      #region OpenRead

      #region .NET

      /// <summary>Creates a read-only <see cref="T:FileStream"/>.</summary>
      /// <returns>A new read-only <see cref="T:FileStream"/> object.</returns>
      /// <remarks>This method returns a read-only <see cref="T:FileStream"/> object with the <see cref="T:FileShare"/> mode set to Read.</remarks>
      [SecurityCritical]
      public FileStream OpenRead()
      {
         return File.OpenInternal(Transaction, LongFullName, FileMode.Open, 0, FileAccess.Read, FileShare.Read, EFileAttributes.Normal, null);
      }

      #endregion // .NET

      #endregion // OpenRead

      #region OpenText

      #region .NET

      /// <summary>Creates a <see cref="T:StreamReader"/> with <see cref="T:NativeMethods.DefaultFileEncoding"/> encoding that reads from an existing text file.</summary>
      /// <returns>A new <see cref="T:StreamReader"/> with <see cref="T:NativeMethods.DefaultFileEncoding"/> encoding.</returns>
      [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
      [SecurityCritical]
      public StreamReader OpenText()
      {
         return new StreamReader(File.OpenInternal(Transaction, LongFullName, FileMode.Open, 0, FileAccess.Read, FileShare.None, EFileAttributes.Normal, null), NativeMethods.DefaultFileEncoding);
      }

      #endregion // .NET

      #region AlphaFS

      /// <summary>[AlphaFS] Creates a <see cref="T:StreamReader"/> with <see cref="T:Encoding"/> that reads from an existing text file.</summary>
      /// <returns>A new <see cref="T:StreamReader"/> with the specified <see cref="T:Encoding"/>.</returns>
      /// <param name="encoding">The <see cref="T:Encoding"/> applied to the contents of the file.</param>
      [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
      [SecurityCritical]
      public StreamReader OpenText(Encoding encoding)
      {
         return new StreamReader(File.OpenInternal(Transaction, LongFullName, FileMode.Open, 0, FileAccess.Read, FileShare.None, EFileAttributes.Normal, null), encoding);
      }

      #endregion // AlphaFS

      #endregion // OpenText

      #region OpenWrite

      #region .NET

      /// <summary>Creates a write-only <see cref="T:FileStream"/>.</summary>
      /// <returns>A write-only unshared <see cref="T:FileStream"/> object for a new or existing file.</returns>
      [SecurityCritical]
      public FileStream OpenWrite()
      {
         return File.OpenInternal(Transaction, LongFullName, FileMode.Open, 0, FileAccess.Write, FileShare.None, EFileAttributes.Normal, null);
      }

      #endregion // .NET

      #endregion // OpenWrite

      #region Refresh

      #region .NET

      /// <summary>Refreshes the state of the object.</summary>
      [SecurityCritical]
      public new void Refresh()
      {
         base.Refresh();
      }

      #endregion // .NET

      #endregion // Refresh

      #region Replace

      #region .NET

      /// <summary>Replaces the contents of a specified file with the file described by the current <see cref="T:FileInfo"/> object, deleting the original file, and creating a backup of the replaced file.</summary>
      /// <param name="destinationFileName">The name of a file to replace with the current file.</param>
      /// <param name="destinationBackupFileName">The name of a file with which to create a backup of the file described by the <paramref name="destinationFileName"/> parameter.</param>
      /// <returns>A <see cref="T:FileInfo"/> object that encapsulates information about the file described by the <paramref name="destinationFileName"/> parameter.</returns>
      /// <remarks>The Replace method replaces the contents of a specified file with the contents of the file described by the current <see cref="T:FileInfo"/> object. It also creates a backup of the file that was replaced. Finally, it returns a new <see cref="T:FileInfo"/> object that describes the overwritten file.</remarks>
      /// <remarks>Pass null to the <paramref name="destinationBackupFileName"/> parameter if you do not want to create a backup of the file being replaced.</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public FileInfo Replace(string destinationFileName, string destinationBackupFileName)
      {
         return Replace(destinationFileName, destinationBackupFileName, false, false);
      }

      /// <summary>Replaces the contents of a specified file with the file described by the current <see cref="T:FileInfo"/> object, deleting the original file, and creating a backup of the replaced file. Also specifies whether to ignore merge errors.</summary>
      /// <param name="destinationFileName">The name of a file to replace with the current file.</param>
      /// <param name="destinationBackupFileName">The name of a file with which to create a backup of the file described by the <paramref name="destinationFileName"/> parameter.</param>
      /// <param name="ignoreMetadataErrors"><c>true</c> to ignore merge errors (such as attributes and ACLs) from the replaced file to the replacement file; otherwise, <c>false</c>.</param>
      /// <returns>A <see cref="T:FileInfo"/> object that encapsulates information about the file described by the <paramref name="destinationFileName"/> parameter.</returns>
      /// <remarks>The Replace method replaces the contents of a specified file with the contents of the file described by the current <see cref="T:FileInfo"/> object. It also creates a backup of the file that was replaced. Finally, it returns a new <see cref="T:FileInfo"/> object that describes the overwritten file.</remarks>
      /// <remarks>Pass null to the <paramref name="destinationBackupFileName"/> parameter if you do not want to create a backup of the file being replaced.</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public FileInfo Replace(string destinationFileName, string destinationBackupFileName, bool ignoreMetadataErrors)
      {
         return Replace(destinationFileName, destinationBackupFileName, ignoreMetadataErrors, false);
      }

      #endregion // .NET

      #region AlphaFS

      #region IsFullPath

      /// <summary>[AlphaFS] Replaces the contents of a specified file with the file described by the current <see cref="T:FileInfo"/> object, deleting the original file, and creating a backup of the replaced file. Also specifies whether to ignore merge errors.</summary>
      /// <param name="destinationFileName">The name of a file to replace with the current file.</param>
      /// <param name="destinationBackupFileName">The name of a file with which to create a backup of the file described by the <paramref name="destinationFileName"/> parameter.</param>
      /// <param name="ignoreMetadataErrors"><c>true</c> to ignore merge errors (such as attributes and ACLs) from the replaced file to the replacement file; otherwise, <c>false</c>.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="destinationFileName"/> and <paramref name="destinationBackupFileName"/> will be normalized and long path prefixed. <c>null</c> <paramref name="destinationFileName"/> and <paramref name="destinationBackupFileName"/> are already a full path with long path prefix, will be used as is.</param>
      /// <returns>A <see cref="T:FileInfo"/> object that encapsulates information about the file described by the <paramref name="destinationFileName"/> parameter.</returns>
      /// <remarks>The Replace method replaces the contents of a specified file with the contents of the file described by the current <see cref="T:FileInfo"/> object. It also creates a backup of the file that was replaced. Finally, it returns a new <see cref="T:FileInfo"/> object that describes the overwritten file.</remarks>
      /// <remarks>Pass null to the <paramref name="destinationBackupFileName"/> parameter if you do not want to create a backup of the file being replaced.</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public FileInfo Replace(string destinationFileName, string destinationBackupFileName, bool ignoreMetadataErrors, bool? isFullPath)
      {
         string destinationFileNameLp = isFullPath == null
            ? destinationFileName
            : (bool) isFullPath
               ? Path.GetLongPathInternal(destinationFileName, false, false, false, false)
               : Path.GetFullPathInternal(Transaction, destinationFileName, true, false, false, true);

         // Pass null to the destinationBackupFileName parameter if you do not want to create a backup of the file being replaced.
         string destinationBackupFileNameLp = isFullPath == null
            ? destinationBackupFileName
            : (bool) isFullPath
               ? Path.GetLongPathInternal(destinationBackupFileName, false, false, false, false)
               : Path.GetFullPathInternal(Transaction, destinationBackupFileName, true, false, false, true);

         File.ReplaceInternal(LongFullName, destinationFileNameLp, destinationBackupFileNameLp, ignoreMetadataErrors, null);

         return new FileInfo(Transaction, destinationFileNameLp, true);
      }

      #endregion // IsFullPath

      #endregion // AlphaFS

      #endregion // Replace

      #region SetAccessControl

      #region .NET

      /// <summary>Applies access control list (ACL) entries described by a FileSecurity object to the file described by the current FileInfo object.</summary>
      /// <param name="fileSecurity">A <see cref="T:FileSecurity"/> object that describes an access control list (ACL) entry to apply to the current file.</param>
      /// <remarks>The SetAccessControl method applies access control list (ACL) entries to the current file that represents the noninherited ACL list. 
      /// Use the SetAccessControl method whenever you need to add or remove ACL entries from a file.
      /// </remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
      [SecurityCritical]
      public void SetAccessControl(FileSecurity fileSecurity)
      {
         File.SetAccessControlInternal(LongFullName, null, fileSecurity, AccessControlSections.All, null);
      }

      /// <summary>Applies access control list (ACL) entries described by a FileSecurity object to the file described by the current FileInfo object.</summary>
      /// <param name="fileSecurity">A <see cref="T:FileSecurity"/> object that describes an access control list (ACL) entry to apply to the current file.</param>
      /// <param name="includeSections">One or more of the <see cref="T:AccessControlSections"/> values that specifies the type of access control list (ACL) information to set.</param>
      /// <remarks>The SetAccessControl method applies access control list (ACL) entries to the current file that represents the noninherited ACL list. 
      /// Use the SetAccessControl method whenever you need to add or remove ACL entries from a file.
      /// </remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
      [SecurityCritical]
      public void SetAccessControl(FileSecurity fileSecurity, AccessControlSections includeSections)
      {
         File.SetAccessControlInternal(LongFullName, null, fileSecurity, includeSections, null);
      }

      #endregion // .NET

      #endregion // SetAccessControl

      #region ToString

      #region .NET

      /// <summary>Returns the path as a string.</summary>
      /// <returns>The path.</returns>
      public override string ToString()
      {
         return DisplayPath;
      }

      #endregion // .NET

      #endregion // ToString

      #endregion // .NET

      #region AlphaFS

      #region AddStream

      /// <summary>[AlphaFS] Adds an alternate data stream (NTFS ADS) to the file.</summary>
      /// <param name="name">The name for the stream. If a stream with <paramref name="name"/> already exists, it will be overwritten.</param>
      /// <param name="contents">The lines to add to the stream.</param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public void AddStream(string name, string[] contents)
      {
         AlternateDataStreamInfo.AddStreamInternal(false, Transaction, LongFullName, name, contents, null);
      }

      #endregion // AddStream

      #region Compress

      /// <summary>[AlphaFS] Compresses a file using NTFS compression.</summary>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public void Compress()
      {
         Device.ToggleCompressionInternal(false, Transaction, LongFullName, true, null);
      }

      #endregion // Compress

      #region Decompress

      /// <summary>[AlphaFS] Decompresses an NTFS compressed file.</summary>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public void Decompress()
      {
         Device.ToggleCompressionInternal(false, Transaction, LongFullName, false, null);
      }

      #endregion // Decompress

      #region EnumerateStreams

      /// <summary>[AlphaFS] Returns an enumerable collection of <see cref="T:AlternateDataStreamInfo"/> instances for the file.</summary>
      /// <returns>An enumerable collection of <see cref="T:AlternateDataStreamInfo"/> instances for the file.</returns>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public IEnumerable<AlternateDataStreamInfo> EnumerateStreams()
      {
         return AlternateDataStreamInfo.EnumerateStreamsInternal(false, Transaction, null, LongFullName, null, null, null);
      }

      /// <summary>[AlphaFS] Returns an enumerable collection of <see cref="T:AlternateDataStreamInfo"/> instances for the file.</summary>
      /// <returns>An enumerable collection of <see cref="T:AlternateDataStreamInfo"/> of type <see cref="T:StreamType"/> instances for the file.</returns>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public IEnumerable<AlternateDataStreamInfo> EnumerateStreams(StreamType streamType)
      {
         return AlternateDataStreamInfo.EnumerateStreamsInternal(false, Transaction, null, LongFullName, null, streamType, null);
      }

      #endregion // EnumerateStreams

      #region GetStreamSize

      /// <summary>[AlphaFS] Retrieves the actual number of bytes of disk storage used by all data streams (NTFS ADS).</summary>
      /// <returns>The number of bytes used by all data streams.</returns>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public long GetStreamSize()
      {
         return AlternateDataStreamInfo.GetStreamSizeInternal(false, Transaction, null, LongFullName, null, null, null);
      }

      /// <summary>[AlphaFS] Retrieves the actual number of bytes of disk storage used by a named data streams (NTFS ADS).</summary>
      /// <param name="name">The name of the stream to retrieve.</param>
      /// <returns>The number of bytes used by a named stream.</returns>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public long GetStreamSize(string name)
      {
         return AlternateDataStreamInfo.GetStreamSizeInternal(false, Transaction, null, LongFullName, name, StreamType.Data, null);
      }

      /// <summary>[AlphaFS] Retrieves the actual number of bytes of disk storage used by a <see cref="T:StreamType"/> data streams (NTFS ADS).</summary>
      /// <param name="type">The <see cref="T:StreamType"/> of the stream to retrieve.</param>
      /// <returns>The number of bytes used by stream of type <see cref="T:StreamType"/>.</returns>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public long GetStreamSize(StreamType type)
      {
         return AlternateDataStreamInfo.GetStreamSizeInternal(false, Transaction, null, LongFullName, null, type, null);
      }

      #endregion GetStreamSize

      #region RemoveStream

      /// <summary>[AlphaFS] Removes all alternate data streams (NTFS ADS) from the file.</summary>
      /// <remarks>This method only removes streams of type <see cref="T:StreamType.AlternateData"/>.</remarks>
      /// <remarks>No Exception is thrown if the stream does not exist.</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public void RemoveStream()
      {
         AlternateDataStreamInfo.RemoveStreamInternal(false, Transaction, LongFullName, null, null);
      }

      /// <summary>[AlphaFS] Removes an alternate data stream (NTFS ADS) from the file.</summary>
      /// <param name="name">The name of the stream to remove.</param>
      /// <remarks>This method only removes streams of type <see cref="T:StreamType.AlternateData"/>.</remarks>
      /// <remarks>No Exception is thrown if the stream does not exist.</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public void RemoveStream(string name)
      {
         AlternateDataStreamInfo.RemoveStreamInternal(false, Transaction, LongFullName, name, null);
      }

      #endregion // RemoveStream


      #region Unified Internals

      #region CopyToMoveToInternal

      /// <summary>[AlphaFS] Unified method CopyToMoveToInternal() to copy an existing file to a new file, allowing the overwriting of an existing file.</summary>
      /// <param name="isMove"><c>true</c> indicates a file move, <c>false</c> indicates a file copy.</param>
      /// <param name="destFileName"><para>A full path string to the destination directory</para></param>
      /// <param name="copyOptions"><para>This parameter can be <c>null</c>. Use <see cref="T:CopyOptions"/> to specify how the file is to be copied.</para></param>
      /// <param name="moveOptions"><para>This parameter can be <c>null</c>. Use <see cref="T:MoveOptions"/> that specify how the file is to be moved.</para></param>
      /// <param name="copyProgress"><para>This parameter can be <c>null</c>. A callback function that is called each time another portion of the file has been copied.</para></param>
      /// <param name="userProgressData"><para>This parameter can be <c>null</c>. The argument to be passed to the callback function.</para></param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="destFileName"/> will be normalized and long path prefixed. <c>null</c> <paramref name="destFileName"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>When <paramref name="isMove"/> is <c>false</c> a new <see cref="T:FileInfo"/> instance with a fully qualified path. Otherwise <c>null</c> is returned.</returns>
      /// <remarks>The attributes of the original file are retained in the copied file.</remarks>
      /// <remarks>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method. If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior</remarks>
      /// <remarks>This Move method works across disk volumes, and it does not throw an exception if the source and destination are
      /// the same. Note that if you attempt to replace a file by moving a file of the same name into that directory, you
      /// get an IOException. You cannot use the Move method to overwrite an existing file.
      /// </remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      private FileInfo CopyToMoveToInternal(bool isMove, string destFileName, CopyOptions? copyOptions, MoveOptions? moveOptions, CopyMoveProgressCallback copyProgress, object userProgressData, bool? isFullPath)
      {
         string destFileNameLp = isFullPath == null
            ? destFileName
            : (bool) isFullPath
            ? Path.GetLongPathInternal(destFileName, false, false, false, false)
            : Path.GetFullPathInternal(Transaction, destFileName, true, false, false, true);

         File.CopyMoveInternal(isMove, Transaction, LongFullName, destFileNameLp, false, copyOptions, moveOptions, copyProgress, userProgressData, null);

         if (isMove)
         {
            LongFullName = destFileNameLp;
            FullPath = Path.GetRegularPathInternal(destFileNameLp, false, false, false, false);

            OriginalPath = destFileName;
            DisplayPath = OriginalPath;

            _name = Path.GetFileName(destFileNameLp, true);

            // Flush any cached information about the file.
            Reset();
         }

         return isMove ? null : new FileInfo(Transaction, destFileNameLp, true);
      }

      #endregion // CopyToMoveToInternal

      #endregion // Unified Internals

      #endregion // AlphaFS

      #endregion // Methods

      #region Properties

      #region .NET

      #region Directory

      /// <summary>Gets an instance of the parent directory.</summary>
      /// <returns>A <see cref="T:DirectoryInfo"/> object representing the parent directory of this file.</returns>
      /// <remarks>To get the parent directory as a string, use the DirectoryName property.</remarks>
      public DirectoryInfo Directory
      {
         get
         {
            string dirName = DirectoryName;
            return dirName == null ? null : new DirectoryInfo(Transaction, dirName, false);
         }
      }

      #endregion // Directory

      #region DirectoryName

      /// <summary>Gets the directory's full path.</summary>
      /// <returns>The directory's full path.</returns>
      public string DirectoryName
      {
         get { return Path.GetDirectoryName(FullPath); }
      }

      #endregion // DirectoryName

      #region Exists

      /// <summary>Gets a value indicating whether the file exists.</summary>
      /// <returns><c>true</c> on success, <c>false</c> otherwise.</returns>
      public override bool Exists
      {
         get { return (EntryInfo != null && !EntryInfo.IsDirectory); }
      }

      #endregion // Exists

      #region IsReadOnly

      /// <summary>Gets or sets a value that determines if the current file is read only.</summary>
      /// <returns><c>true</c> if the current file is read only, <c>false</c> otherwise.</returns>
      public bool IsReadOnly
      {
         get
         {
            return Attributes == (FileAttributes) (-1) || (Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly;
         }

         set
         {
            FileInfo fileInfo = this;
            fileInfo.Attributes = value
               ? (fileInfo.Attributes | FileAttributes.ReadOnly)
               : (fileInfo.Attributes & ~FileAttributes.ReadOnly);
         }
      }

      #endregion // IsReadOnly

      #region Length

      private long _length = -1;

      /// <summary>Gets the size, in bytes, of the current file.</summary>
      /// <returns>The size of the current file in bytes.</returns>
      public long Length
      {
         get
         {
            if (_length == -1)
               Refresh();

            _length = EntryInfo != null ? EntryInfo.FileSize : -1;

            return _length;
         }
      }

      #endregion // Length

      #region Name

      private string _name;

      /// <summary>Gets the name of the file.</summary>
      /// <returns>The name of the file.</returns>
      /// <remarks>
      /// The name of the file includes the file extension.
      /// When first called, FileInfo calls Refresh and caches information about the file. On subsequent calls, you must call Refresh to get the latest copy of the information.
      /// </remarks>
      public override string Name
      {
         get { return _name; }
      }

      #endregion // Name

      #endregion // .NET

      #region AlphaFS

      #region LengthStreams

      /// <summary>[AlphaFS] Retrieves the actual number of bytes of disk storage used by all data streams (NTFS ADS).</summary>
      /// <returns>The number of bytes used by all data streams.</returns>
      public long LengthStreams
      {
         get
         {
            return AlternateDataStreamInfo.GetStreamSizeInternal(false, Transaction, null, LongFullName, null, null, null);
         }
      }

      #endregion // LengthStreams

      #endregion // AlphaFS

      #endregion // Properties
   }
}