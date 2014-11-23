/* Copyright (c) 2008-2014 Peter Palotas, Jeffrey Jangli, Normalex
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
      /// <param name="isFullPath">
      /// <para><c>true</c> <paramref name="fileName"/> is an absolute path. Unicode prefix is applied.</para>
      /// <para><c>false</c> <paramref name="fileName"/> will be checked and resolved to an absolute path. Unicode prefix is applied.</para>
      /// <para><c>null</c> <paramref name="fileName"/> is already an absolute path with Unicode prefix. Use as is.</para>
      /// </param>
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
      /// <param name="isFullPath">
      /// <para><c>true</c> <paramref name="fileName"/> is an absolute path. Unicode prefix is applied.</para>
      /// <para><c>false</c> <paramref name="fileName"/> will be checked and resolved to an absolute path. Unicode prefix is applied.</para>
      /// <para><c>null</c> <paramref name="fileName"/> is already an absolute path with Unicode prefix. Use as is.</para>
      /// </param>
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

      /// <summary>Copies an existing file to a new file, disallowing the overwriting of an existing file.
      /// <para>&#160;</para>
      /// <returns>Returns a new <see cref="T:FileInfo"/> instance with a fully qualified path.</returns>
      /// <para>&#160;</para>
      /// <remarks>
      /// <para>Use this method to prevent overwriting of an existing file by default.</para>
      /// <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      /// <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <exception cref="ArgumentException">destinationPath contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">destinationPath is <c>null</c>.</exception>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="NativeError.ThrowException()"/>
      /// <param name="destinationPath">The name of the new file to copy to.</param>
      [SecurityCritical]
      public FileInfo CopyTo(string destinationPath)
      {
         string destinationPathLp;
         CopyToMoveToInternal(destinationPath, false, CopyOptions.FailIfExists, null, null, null, out destinationPathLp, false);
         return new FileInfo(Transaction, destinationPathLp, null);
      }

      /// <summary>Copies an existing file to a new file, allowing the overwriting of an existing file.
      /// <para>&#160;</para>
      /// <returns>
      /// <para>Returns a new file, or an overwrite of an existing file if <paramref name="overwrite"/> is <c>true</c>.</para>
      /// <para>If the file exists and <paramref name="overwrite"/> is <c>false</c>, an <see cref="IOException"/> is thrown.</para>
      /// </returns>
      /// <para>&#160;</para>
      /// <remarks>
      /// <para>Use this method to allow or prevent overwriting of an existing file.</para>
      /// <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      /// <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <exception cref="ArgumentException">destinationPath contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">destinationPath is <c>null</c>.</exception>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="NativeError.ThrowException()"/>
      /// <param name="destinationPath">The name of the new file to copy to.</param>
      /// <param name="overwrite"><c>true</c> to allow an existing file to be overwritten; otherwise, <c>false</c>.</param>
      [SecurityCritical]
      public FileInfo CopyTo(string destinationPath, bool overwrite)
      {
         string destinationPathLp;
         CopyToMoveToInternal(destinationPath, false, overwrite ? CopyOptions.None : CopyOptions.FailIfExists, null, null, null, out destinationPathLp, false);
         return new FileInfo(Transaction, destinationPathLp, null);
      }

      #endregion // .NET

      #region AlphaFS

      #region IsFullPath

      /// <summary>[AlphaFS] Copies an existing file to a new file, disallowing the overwriting of an existing file.
      /// <para>&#160;</para>
      /// <returns>Returns a new <see cref="T:FileInfo"/> instance with a fully qualified path.</returns>
      /// <para>&#160;</para>
      /// <remarks>
      /// <para>Use this method to prevent overwriting of an existing file by default.</para>
      /// <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      /// <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <exception cref="ArgumentException">destinationPath contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">destinationPath is <c>null</c>.</exception>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="NativeError.ThrowException()"/>
      /// <param name="destinationPath">The name of the new file to copy to.</param>
      /// <param name="isFullPath">
      ///    <para><c>true</c> <paramref name="destinationPath"/> is an absolute path. Unicode prefix is applied.</para>
      ///    <para><c>false</c> <paramref name="destinationPath"/> will be checked and resolved to an absolute path. Unicode prefix is applied.</para>
      ///    <para><c>null</c> <paramref name="destinationPath"/> is already an absolute path with Unicode prefix. Use as is.</para>
      /// </param>
      [SecurityCritical]
      public FileInfo CopyTo(string destinationPath, bool? isFullPath)
      {
         string destinationPathLp;
         CopyToMoveToInternal(destinationPath, false, CopyOptions.FailIfExists, null, null, null, out destinationPathLp, isFullPath);
         return new FileInfo(Transaction, destinationPathLp, null);
      }

      /// <summary>[AlphaFS] Copies an existing file to a new file, allowing the overwriting of an existing file.
      /// <para>&#160;</para>
      /// <returns>
      /// <para>Returns a new file, or an overwrite of an existing file if <paramref name="overwrite"/> is <c>true</c>.</para>
      /// <para>If the file exists and <paramref name="overwrite"/> is <c>false</c>, an <see cref="IOException"/> is thrown.</para>
      /// </returns>
      /// <para>&#160;</para>
      /// <remarks>
      /// <para>Use this method to allow or prevent overwriting of an existing file.</para>
      /// <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      /// <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <exception cref="ArgumentException">destinationPath contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">destinationPath is <c>null</c>.</exception>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="NativeError.ThrowException()"/>
      /// <param name="destinationPath">The name of the new file to copy to.</param>
      /// <param name="overwrite"><c>true</c> to allow an existing file to be overwritten; otherwise, <c>false</c>.</param>
      /// <param name="isFullPath">
      ///    <para><c>true</c> <paramref name="destinationPath"/> is an absolute path. Unicode prefix is applied.</para>
      ///    <para><c>false</c> <paramref name="destinationPath"/> will be checked and resolved to an absolute path. Unicode prefix is applied.</para>
      ///    <para><c>null</c> <paramref name="destinationPath"/> is already an absolute path with Unicode prefix. Use as is.</para>
      /// </param>
      [SecurityCritical]
      public FileInfo CopyTo(string destinationPath, bool overwrite, bool? isFullPath)
      {
         string destinationPathLp;
         CopyToMoveToInternal(destinationPath, false, overwrite ? CopyOptions.None : CopyOptions.FailIfExists, null, null, null, out destinationPathLp, isFullPath);
         return new FileInfo(Transaction, destinationPathLp, null);
      }

      #endregion // IsFullPath

      #endregion // AlphaFS

      #endregion // CopyTo

      #region Create

      #region .NET

      /// <summary>Creates a file.</summary>
      /// <returns><see cref="T:FileStream"/>A new file.</returns>
      [SecurityCritical]
      public FileStream Create()
      {
         return File.CreateFileInternal(Transaction, LongFullName, NativeMethods.DefaultFileBufferSize, ExtendedFileAttributes.Normal, null, FileMode.Create, FileAccess.ReadWrite, FileShare.None, null);
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
         return new StreamWriter(File.CreateFileInternal(Transaction, LongFullName, NativeMethods.DefaultFileBufferSize, ExtendedFileAttributes.Normal, null, FileMode.Create, FileAccess.ReadWrite, FileShare.None, null), NativeMethods.DefaultFileEncoding);
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

      /// <summary>Permanently deletes a file.
      /// <para>&#160;</para>
      /// <remarks>If the file does not exist, this method does nothing.</remarks>
      /// </summary>
      /// <exception cref="IOException"></exception>
      public override void Delete()
      {
         File.DeleteFileInternal(Transaction, LongFullName, false, null);
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

      /// <summary>Moves a specified file to a new location, providing the option to specify a new file name.
      /// <para>&#160;</para>
      /// <remarks>
      /// <para>Use this method to prevent overwriting of an existing file by default.</para>
      /// <para>This method works across disk volumes.</para>
      /// <para>For example, the file c:\MyFile.txt can be moved to d:\public and renamed NewFile.txt.</para>
      /// <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      /// <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <exception cref="ArgumentException">destinationPath contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">destinationPath is <c>null</c>.</exception>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="NativeError.ThrowException()"/>
      /// <param name="destinationPathFileName">The path to move the file to, which can specify a different file name.</param>
      [SecurityCritical]
      public void MoveTo(string destinationPathFileName)
      {
         string destinationPathLp;
         CopyToMoveToInternal(destinationPathFileName, false, null, MoveOptions.CopyAllowed, null, null, out destinationPathLp, false);
         CopyToMoveToInternalRefresh(destinationPathFileName, destinationPathLp);
      }

      #endregion // .NET

      #region AlphaFS

      #region IsFullPath

      /// <summary>[AlphaFS] Moves a specified file to a new location, providing the option to specify a new file name.
      /// <para>&#160;</para>
      /// <remarks>
      /// <para>Use this method to prevent overwriting of an existing file by default.</para>
      /// <para>This method works across disk volumes.</para>
      /// <para>For example, the file c:\MyFile.txt can be moved to d:\public and renamed NewFile.txt.</para>
      /// <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      /// <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <exception cref="ArgumentException">destinationPath contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">destinationPath is <c>null</c>.</exception>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="NativeError.ThrowException()"/>
      /// <param name="destinationPathFileName">The path to move the file to, which can specify a different file name.</param>
      /// <param name="isFullPath">
      ///    <para><c>true</c> <paramref name="destinationPathFileName"/> is an absolute path. Unicode prefix is applied.</para>
      ///    <para><c>false</c> <paramref name="destinationPathFileName"/> will be checked and resolved to an absolute path. Unicode prefix is applied.</para>
      ///    <para><c>null</c> <paramref name="destinationPathFileName"/> is already an absolute path with Unicode prefix. Use as is.</para>
      /// </param>
      [SecurityCritical]
      public void MoveTo(string destinationPathFileName, bool? isFullPath)
      {
         string destinationPathLp;
         CopyToMoveToInternal(destinationPathFileName, false, null, MoveOptions.CopyAllowed, null, null, out destinationPathLp, isFullPath);
         CopyToMoveToInternalRefresh(destinationPathFileName, destinationPathLp);
      }

      #endregion // IsFullPath
      
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
         return File.OpenInternal(Transaction, LongFullName, mode, 0, FileAccess.Read, FileShare.None, ExtendedFileAttributes.Normal, null);
      }

      /// <summary>Opens a file in the specified mode with read, write, or read/write access.</summary>
      /// <param name="mode">A <see cref="T:FileMode"/> constant specifying the mode (for example, Open or Append) in which to open the file.</param>
      /// <param name="access">A <see cref="T:FileAccess"/> constant specifying whether to open the file with Read, Write, or ReadWrite file access.</param>
      /// <returns>A <see cref="T:FileStream"/> object opened in the specified mode and access, and unshared.</returns>
      [SecurityCritical]
      public FileStream Open(FileMode mode, FileAccess access)
      {
         return File.OpenInternal(Transaction, LongFullName, mode, 0, access, FileShare.None, ExtendedFileAttributes.Normal, null);
      }

      /// <summary>Opens a file in the specified mode with read, write, or read/write access and the specified sharing option.</summary>
      /// <param name="mode">A <see cref="T:FileMode"/> constant specifying the mode (for example, Open or Append) in which to open the file.</param>
      /// <param name="access">A <see cref="T:FileAccess"/> constant specifying whether to open the file with Read, Write, or ReadWrite file access.</param>
      /// <param name="share">A <see cref="T:FileShare"/> constant specifying the type of access other <see cref="T:FileStream"/> objects have to this file.</param>
      /// <returns>A <see cref="T:FileStream"/> object opened with the specified mode, access, and sharing options.</returns>
      [SecurityCritical]
      public FileStream Open(FileMode mode, FileAccess access, FileShare share)
      {
         return File.OpenInternal(Transaction, LongFullName, mode, 0, access, share, ExtendedFileAttributes.Normal, null);
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
         return File.OpenInternal(Transaction, LongFullName, mode, rights, 0, FileShare.None, ExtendedFileAttributes.Normal, null);
      }

      /// <summary>[AlphaFS] Opens a file in the specified mode with read, write, or read/write access and the specified sharing option.</summary>
      /// <param name="mode">A <see cref="T:FileMode"/> constant specifying the mode (for example, Open or Append) in which to open the file.</param>
      /// <param name="rights">A <see cref="T:FileSystemRights"/> value that specifies whether a file is created if one does not exist, and determines whether the contents of existing files are retained or overwritten along with additional options.</param>
      /// <param name="share">A <see cref="T:FileShare"/> constant specifying the type of access other <see cref="T:FileStream"/> objects have to this file.</param>
      /// <returns>A <see cref="T:FileStream"/> object opened with the specified mode, access, and sharing options.</returns>
      [SecurityCritical]
      public FileStream Open(FileMode mode, FileSystemRights rights, FileShare share)
      {
         return File.OpenInternal(Transaction, LongFullName, mode, rights, 0, share, ExtendedFileAttributes.Normal, null);
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
         return File.OpenInternal(Transaction, LongFullName, FileMode.Open, 0, FileAccess.Read, FileShare.Read, ExtendedFileAttributes.Normal, null);
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
         return new StreamReader(File.OpenInternal(Transaction, LongFullName, FileMode.Open, 0, FileAccess.Read, FileShare.None, ExtendedFileAttributes.Normal, null), NativeMethods.DefaultFileEncoding);
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
         return new StreamReader(File.OpenInternal(Transaction, LongFullName, FileMode.Open, 0, FileAccess.Read, FileShare.None, ExtendedFileAttributes.Normal, null), encoding);
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
         return File.OpenInternal(Transaction, LongFullName, FileMode.Open, 0, FileAccess.Write, FileShare.None, ExtendedFileAttributes.Normal, null);
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
      /// <param name="isFullPath">
      /// <para><c>true</c> <paramref name="destinationFileName"/> and <paramref name="destinationBackupFileName"/> is an absolute path. Unicode prefix is applied.</para>
      /// <para><c>false</c> <paramref name="destinationFileName"/> and <paramref name="destinationBackupFileName"/> will be checked and resolved to an absolute path. Unicode prefix is applied.</para>
      /// <para><c>null</c> <paramref name="destinationFileName"/> and <paramref name="destinationBackupFileName"/> is already an absolute path with Unicode prefix. Use as is.</para>
      /// </param>
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
               : Path.GetFullPathInternal(Transaction, destinationFileName, true, false, false, true, false, true, true);

         // Pass null to the destinationBackupFileName parameter if you do not want to create a backup of the file being replaced.
         string destinationBackupFileNameLp = isFullPath == null
            ? destinationBackupFileName
            : (bool) isFullPath
               ? Path.GetLongPathInternal(destinationBackupFileName, false, false, false, false)
               : Path.GetFullPathInternal(Transaction, destinationBackupFileName, true, false, false, true, false, true, true);

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

      #region CopyTo1

      #region IsFullPath

      #region FileInfo

      /// <summary>[AlphaFS] Copies an existing file to a new file, allowing the overwriting of an existing file, <see cref="CopyOptions"/> can be specified.
      /// <para>&#160;</para>
      /// <returns>
      /// <para>Returns a new file, or an overwrite of an existing file if <paramref name="copyOptions"/> is not <see cref="CopyOptions.FailIfExists"/>.</para>
      /// <para>If the file exists and <paramref name="copyOptions"/> contains <see cref="CopyOptions.FailIfExists"/>, an <see cref="IOException"/> is thrown.</para>
      /// </returns>
      /// <para>&#160;</para>
      /// <remarks>
      /// <para>Use this method to allow or prevent overwriting of an existing file.</para>
      /// <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      /// <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      /// <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <exception cref="ArgumentException">destinationPath contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">destinationPath is <c>null</c>.</exception>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="NativeError.ThrowException()"/>
      /// <param name="destinationPath">The name of the new file to copy to.</param>
      /// <param name="copyOptions"><see cref="T:CopyOptions"/> that specify how the file is to be copied.</param>
      /// <param name="isFullPath">
      ///    <para><c>true</c> <paramref name="destinationPath"/> is an absolute path. Unicode prefix is applied.</para>
      ///    <para><c>false</c> <paramref name="destinationPath"/> will be checked and resolved to an absolute path. Unicode prefix is applied.</para>
      ///    <para><c>null</c> <paramref name="destinationPath"/> is already an absolute path with Unicode prefix. Use as is.</para>
      /// </param>
      [SecurityCritical]
      public FileInfo CopyTo1(string destinationPath, CopyOptions copyOptions, bool? isFullPath)
      {
         string destinationPathLp;
         CopyToMoveToInternal(destinationPath, false, copyOptions, null, null, null, out destinationPathLp, isFullPath);
         return new FileInfo(Transaction, destinationPathLp, null);
      }

      /// <summary>[AlphaFS] Copies an existing file to a new file, allowing the overwriting of an existing file, <see cref="CopyOptions"/> can be specified.
      /// <para>&#160;</para>
      /// <returns>
      /// <para>Returns a new file, or an overwrite of an existing file if <paramref name="copyOptions"/> is not <see cref="CopyOptions.FailIfExists"/>.</para>
      /// <para>If the file exists and <paramref name="copyOptions"/> contains <see cref="CopyOptions.FailIfExists"/>, an <see cref="IOException"/> is thrown.</para>
      /// </returns>
      /// <para>&#160;</para>
      /// <remarks>
      /// <para>Use this method to allow or prevent overwriting of an existing file.</para>
      /// <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      /// <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      /// <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <exception cref="ArgumentException">destinationPath contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">destinationPath is <c>null</c>.</exception>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="NativeError.ThrowException()"/>
      /// <param name="destinationPath">The name of the new file to copy to.</param>
      /// <param name="copyOptions"><see cref="T:CopyOptions"/> that specify how the file is to be copied.</param>
      /// <param name="preserveDates"><c>true</c> if original Timestamps must be preserved, <c>false</c> otherwise.</param>
      /// <param name="isFullPath">
      ///    <para><c>true</c> <paramref name="destinationPath"/> is an absolute path. Unicode prefix is applied.</para>
      ///    <para><c>false</c> <paramref name="destinationPath"/> will be checked and resolved to an absolute path. Unicode prefix is applied.</para>
      ///    <para><c>null</c> <paramref name="destinationPath"/> is already an absolute path with Unicode prefix. Use as is.</para>
      /// </param>
      [SecurityCritical]
      public FileInfo CopyTo1(string destinationPath, CopyOptions copyOptions, bool preserveDates, bool? isFullPath)
      {
         string destinationPathLp;
         CopyToMoveToInternal(destinationPath, preserveDates, copyOptions, null, null, null, out destinationPathLp, isFullPath);
         return new FileInfo(Transaction, destinationPathLp, null);
      }

      #endregion // FileInfo

      #region CopyMoveResult

      /// <summary>[AlphaFS] Copies an existing file to a new file, allowing the overwriting of an existing file,
      /// <para>and the possibility of notifying the application of its progress through a callback function.</para>
      /// <para>&#160;</para>
      /// <returns>
      /// <para>Returns a <see cref="CopyMoveResult"/> class with the status of the Copy action.</para>
      /// <para>Returns a new <see cref="T:FileInfo"/> instance with a fully qualified path when successfully copied.</para>
      /// </returns>
      /// <para>&#160;</para>
      /// <remarks>
      /// <para>Use this method to prevent overwriting of an existing file by default.</para>
      /// <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      /// <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <exception cref="ArgumentException">destinationPath contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">destinationPath is <c>null</c>.</exception>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="NativeError.ThrowException()"/>
      /// <param name="destinationPath">The name of the new file to copy to.</param>
      /// <param name="progressHandler">A callback function that is called each time another portion of the file has been copied. This parameter can be <c>null</c>.</param>
      /// <param name="userProgressData">The argument to be passed to the callback function. This parameter can be <c>null</c>.</param>
      /// <param name="isFullPath">
      ///    <para><c>true</c> <paramref name="destinationPath"/> is an absolute path. Unicode prefix is applied.</para>
      ///    <para><c>false</c> <paramref name="destinationPath"/> will be checked and resolved to an absolute path. Unicode prefix is applied.</para>
      ///    <para><c>null</c> <paramref name="destinationPath"/> is already an absolute path with Unicode prefix. Use as is.</para>
      /// </param>
      [SecurityCritical]
      public CopyMoveResult CopyTo1(string destinationPath, CopyMoveProgressRoutine progressHandler, object userProgressData, bool? isFullPath)
      {
         string destinationPathLp;
         CopyMoveResult cmr = CopyToMoveToInternal(destinationPath, false, CopyOptions.FailIfExists, null, progressHandler, userProgressData, out destinationPathLp, isFullPath);
         CopyToMoveToInternalRefresh(destinationPath, destinationPathLp);
         return cmr;
      }

      /// <summary>[AlphaFS] Copies an existing file to a new file, allowing the overwriting of an existing file, <see cref="CopyOptions"/> can be specified.
      /// <para>and the possibility of notifying the application of its progress through a callback function.</para>
      /// <para>&#160;</para>
      /// <returns>
      /// <para>Returns a <see cref="CopyMoveResult"/> class with the status of the Copy action.</para>
      /// <para>Returns a new file, or an overwrite of an existing file if <paramref name="copyOptions"/> is not <see cref="CopyOptions.FailIfExists"/>.</para>
      /// <para>If the file exists and <paramref name="copyOptions"/> contains <see cref="CopyOptions.FailIfExists"/>, an <see cref="IOException"/> is thrown.</para>
      /// </returns>
      /// <para>&#160;</para>
      /// <remarks>
      /// <para>Use this method to allow or prevent overwriting of an existing file.</para>
      /// <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      /// <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      /// <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <exception cref="ArgumentException">destinationPath contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">destinationPath is <c>null</c>.</exception>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="NativeError.ThrowException()"/>
      /// <param name="destinationPath">The name of the new file to copy to.</param>
      /// <param name="copyOptions"><see cref="T:CopyOptions"/> that specify how the file is to be copied.</param>
      /// <param name="progressHandler">A callback function that is called each time another portion of the file has been copied. This parameter can be <c>null</c>.</param>
      /// <param name="userProgressData">The argument to be passed to the callback function. This parameter can be <c>null</c>.</param>
      /// <param name="isFullPath">
      ///    <para><c>true</c> <paramref name="destinationPath"/> is an absolute path. Unicode prefix is applied.</para>
      ///    <para><c>false</c> <paramref name="destinationPath"/> will be checked and resolved to an absolute path. Unicode prefix is applied.</para>
      ///    <para><c>null</c> <paramref name="destinationPath"/> is already an absolute path with Unicode prefix. Use as is.</para>
      /// </param>
      [SecurityCritical]
      public CopyMoveResult CopyTo1(string destinationPath, CopyOptions copyOptions, CopyMoveProgressRoutine progressHandler, object userProgressData, bool? isFullPath)
      {
         string destinationPathLp;
         CopyMoveResult cmr = CopyToMoveToInternal(destinationPath, false, copyOptions, null, progressHandler, userProgressData, out destinationPathLp, isFullPath);
         CopyToMoveToInternalRefresh(destinationPath, destinationPathLp);
         return cmr;
      }

      /// <summary>[AlphaFS] Copies an existing file to a new file, allowing the overwriting of an existing file, <see cref="CopyOptions"/> can be specified.
      /// <para>and the possibility of notifying the application of its progress through a callback function.</para>
      /// <para>&#160;</para>
      /// <returns>
      /// <para>Returns a <see cref="CopyMoveResult"/> class with the status of the Copy action.</para>
      /// <para>Returns a new file, or an overwrite of an existing file if <paramref name="copyOptions"/> is not <see cref="CopyOptions.FailIfExists"/>.</para>
      /// <para>If the file exists and <paramref name="copyOptions"/> contains <see cref="CopyOptions.FailIfExists"/>, an <see cref="IOException"/> is thrown.</para>
      /// </returns>
      /// <para>&#160;</para>
      /// <remarks>
      /// <para>Use this method to allow or prevent overwriting of an existing file.</para>
      /// <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      /// <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      /// <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <exception cref="ArgumentException">destinationPath contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">destinationPath is <c>null</c>.</exception>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="NativeError.ThrowException()"/>
      /// <param name="destinationPath">The name of the new file to copy to.</param>
      /// <param name="copyOptions"><see cref="T:CopyOptions"/> that specify how the file is to be copied.</param>
      /// <param name="preserveDates"><c>true</c> if original Timestamps must be preserved, <c>false</c> otherwise.</param>
      /// <param name="progressHandler">A callback function that is called each time another portion of the file has been copied. This parameter can be <c>null</c>.</param>
      /// <param name="userProgressData">The argument to be passed to the callback function. This parameter can be <c>null</c>.</param>
      /// <param name="isFullPath">
      ///    <para><c>true</c> <paramref name="destinationPath"/> is an absolute path. Unicode prefix is applied.</para>
      ///    <para><c>false</c> <paramref name="destinationPath"/> will be checked and resolved to an absolute path. Unicode prefix is applied.</para>
      ///    <para><c>null</c> <paramref name="destinationPath"/> is already an absolute path with Unicode prefix. Use as is.</para>
      /// </param>
      [SecurityCritical]
      public CopyMoveResult CopyTo1(string destinationPath, CopyOptions copyOptions, bool preserveDates, CopyMoveProgressRoutine progressHandler, object userProgressData, bool? isFullPath)
      {
         string destinationPathLp;
         CopyMoveResult cmr = CopyToMoveToInternal(destinationPath, preserveDates, copyOptions, null, progressHandler, userProgressData, out destinationPathLp, isFullPath);
         CopyToMoveToInternalRefresh(destinationPath, destinationPathLp);
         return cmr;
      }

      #endregion // CopyMoveResult

      #endregion // IsFullPath

      #region FileInfo

      /// <summary>[AlphaFS] Copies an existing file to a new file, allowing the overwriting of an existing file, <see cref="CopyOptions"/> can be specified.
      /// <para>&#160;</para>
      /// <returns>
      /// <para>Returns a new file, or an overwrite of an existing file if <paramref name="copyOptions"/> is not <see cref="CopyOptions.FailIfExists"/>.</para>
      /// <para>If the file exists and <paramref name="copyOptions"/> contains <see cref="CopyOptions.FailIfExists"/>, an <see cref="IOException"/> is thrown.</para>
      /// </returns>
      /// <para>&#160;</para>
      /// <remarks>
      /// <para>Use this method to allow or prevent overwriting of an existing file.</para>
      /// <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      /// <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      /// <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <exception cref="ArgumentException">destinationPath contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">destinationPath is <c>null</c>.</exception>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="NativeError.ThrowException()"/>
      /// <param name="destinationPath">The name of the new file to copy to.</param>
      /// <param name="copyOptions"><see cref="T:CopyOptions"/> that specify how the file is to be copied.</param>
      [SecurityCritical]
      public FileInfo CopyTo1(string destinationPath, CopyOptions copyOptions)
      {
         string destinationPathLp;
         CopyToMoveToInternal(destinationPath, false, copyOptions, null, null, null, out destinationPathLp, false);
         return new FileInfo(Transaction, destinationPathLp, null);
      }

      /// <summary>[AlphaFS] Copies an existing file to a new file, allowing the overwriting of an existing file, <see cref="CopyOptions"/> can be specified.
      /// <para>&#160;</para>
      /// <returns>
      /// <para>Returns a new file, or an overwrite of an existing file if <paramref name="copyOptions"/> is not <see cref="CopyOptions.FailIfExists"/>.</para>
      /// <para>If the file exists and <paramref name="copyOptions"/> contains <see cref="CopyOptions.FailIfExists"/>, an <see cref="IOException"/> is thrown.</para>
      /// </returns>
      /// <para>&#160;</para>
      /// <remarks>
      /// <para>Use this method to allow or prevent overwriting of an existing file.</para>
      /// <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      /// <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      /// <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <exception cref="ArgumentException">destinationPath contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">destinationPath is <c>null</c>.</exception>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="NativeError.ThrowException()"/>
      /// <param name="destinationPath">The name of the new file to copy to.</param>
      /// <param name="copyOptions"><see cref="T:CopyOptions"/> that specify how the file is to be copied.</param>
      /// <param name="preserveDates"><c>true</c> if original Timestamps must be preserved, <c>false</c> otherwise.</param>
      [SecurityCritical]
      public FileInfo CopyTo1(string destinationPath, CopyOptions copyOptions, bool preserveDates)
      {
         string destinationPathLp;
         CopyToMoveToInternal(destinationPath, preserveDates, copyOptions, null, null, null, out destinationPathLp, false);
         return new FileInfo(Transaction, destinationPathLp, null);
      }

      #endregion // FileInfo

      #region CopyMoveResult

      /// <summary>[AlphaFS] Copies an existing file to a new file, allowing the overwriting of an existing file,
      /// <para>and the possibility of notifying the application of its progress through a callback function.</para>
      /// <para>&#160;</para>
      /// <returns>
      /// <para>Returns a <see cref="CopyMoveResult"/> class with the status of the Copy action.</para>
      /// <para>Returns a new <see cref="T:FileInfo"/> instance with a fully qualified path when successfully copied.</para>
      /// </returns>
      /// <para>&#160;</para>
      /// <remarks>
      /// <para>Use this method to prevent overwriting of an existing file by default.</para>
      /// <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      /// <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <exception cref="ArgumentException">destinationPath contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">destinationPath is <c>null</c>.</exception>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="NativeError.ThrowException()"/>
      /// <param name="destinationPath">The name of the new file to copy to.</param>
      /// <param name="progressHandler">A callback function that is called each time another portion of the file has been copied. This parameter can be <c>null</c>.</param>
      /// <param name="userProgressData">The argument to be passed to the callback function. This parameter can be <c>null</c>.</param>
      [SecurityCritical]
      public CopyMoveResult CopyTo1(string destinationPath, CopyMoveProgressRoutine progressHandler, object userProgressData)
      {
         string destinationPathLp;
         CopyMoveResult cmr = CopyToMoveToInternal(destinationPath, false, CopyOptions.FailIfExists, null, progressHandler, userProgressData, out destinationPathLp, false);
         CopyToMoveToInternalRefresh(destinationPath, destinationPathLp);
         return cmr;
      }
      
      /// <summary>[AlphaFS] Copies an existing file to a new file, allowing the overwriting of an existing file, <see cref="CopyOptions"/> can be specified.
      /// <para>and the possibility of notifying the application of its progress through a callback function.</para>
      /// <para>&#160;</para>
      /// <returns>
      /// <para>Returns a <see cref="CopyMoveResult"/> class with the status of the Copy action.</para>
      /// <para>Returns a new file, or an overwrite of an existing file if <paramref name="copyOptions"/> is not <see cref="CopyOptions.FailIfExists"/>.</para>
      /// <para>If the file exists and <paramref name="copyOptions"/> contains <see cref="CopyOptions.FailIfExists"/>, an <see cref="IOException"/> is thrown.</para>
      /// </returns>
      /// <para>&#160;</para>
      /// <remarks>
      /// <para>Use this method to allow or prevent overwriting of an existing file.</para>
      /// <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      /// <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      /// <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <exception cref="ArgumentException">destinationPath contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">destinationPath is <c>null</c>.</exception>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="NativeError.ThrowException()"/>
      /// <param name="destinationPath">The name of the new file to copy to.</param>
      /// <param name="copyOptions"><see cref="T:CopyOptions"/> that specify how the file is to be copied.</param>
      /// <param name="progressHandler">A callback function that is called each time another portion of the file has been copied. This parameter can be <c>null</c>.</param>
      /// <param name="userProgressData">The argument to be passed to the callback function. This parameter can be <c>null</c>.</param>
      [SecurityCritical]
      public CopyMoveResult CopyTo1(string destinationPath, CopyOptions copyOptions, CopyMoveProgressRoutine progressHandler, object userProgressData)
      {
         string destinationPathLp;
         CopyMoveResult cmr = CopyToMoveToInternal(destinationPath, false, copyOptions, null, progressHandler, userProgressData, out destinationPathLp, false);
         CopyToMoveToInternalRefresh(destinationPath, destinationPathLp);
         return cmr;
      }
      
      /// <summary>[AlphaFS] Copies an existing file to a new file, allowing the overwriting of an existing file, <see cref="CopyOptions"/> can be specified.
      /// <para>and the possibility of notifying the application of its progress through a callback function.</para>
      /// <para>&#160;</para>
      /// <returns>
      /// <para>Returns a <see cref="CopyMoveResult"/> class with the status of the Copy action.</para>
      /// <para>Returns a new file, or an overwrite of an existing file if <paramref name="copyOptions"/> is not <see cref="CopyOptions.FailIfExists"/>.</para>
      /// <para>If the file exists and <paramref name="copyOptions"/> contains <see cref="CopyOptions.FailIfExists"/>, an <see cref="IOException"/> is thrown.</para>
      /// </returns>
      /// <para>&#160;</para>
      /// <remarks>
      /// <para>Use this method to allow or prevent overwriting of an existing file.</para>
      /// <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      /// <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      /// <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <exception cref="ArgumentException">destinationPath contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">destinationPath is <c>null</c>.</exception>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="NativeError.ThrowException()"/>
      /// <param name="destinationPath">The name of the new file to copy to.</param>
      /// <param name="copyOptions"><see cref="T:CopyOptions"/> that specify how the file is to be copied.</param>
      /// <param name="preserveDates"><c>true</c> if original Timestamps must be preserved, <c>false</c> otherwise.</param>
      /// <param name="progressHandler">A callback function that is called each time another portion of the file has been copied. This parameter can be <c>null</c>.</param>
      /// <param name="userProgressData">The argument to be passed to the callback function. This parameter can be <c>null</c>.</param>
      [SecurityCritical]
      public CopyMoveResult CopyTo1(string destinationPath, CopyOptions copyOptions, bool preserveDates, CopyMoveProgressRoutine progressHandler, object userProgressData)
      {
         string destinationPathLp;
         CopyMoveResult cmr = CopyToMoveToInternal(destinationPath, preserveDates, copyOptions, null, progressHandler, userProgressData, out destinationPathLp, false);
         CopyToMoveToInternalRefresh(destinationPath, destinationPathLp);
         return cmr;
      }

      #endregion // CopyMoveResult

      #endregion // CopyTo1

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

      /// <summary>[AlphaFS] Retrieves the actual number of bytes of disk storage used by all streams (NTFS ADS).</summary>
      /// <returns>The number of bytes used by all streams.</returns>
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

      #region MoveTo1

      #region IsFullPath

      #region FileInfo

      /// <summary>[AlphaFS] Moves a specified file to a new location, providing the option to specify a new file name, <see cref="MoveOptions"/> can be specified.
      /// <para>&#160;</para>
      /// <returns>
      /// <para>Returns a new <see cref="T:FileInfo"/> instance with a fully qualified path when successfully moved,</para>
      /// </returns>
      /// <para>&#160;</para>
      /// <remarks>
      /// <para>Use this method to allow or prevent overwriting of an existing file.</para>
      /// <para>This method works across disk volumes.</para>
      /// <para>For example, the file c:\MyFile.txt can be moved to d:\public and renamed NewFile.txt.</para>
      /// <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      /// <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <exception cref="ArgumentException">destinationPath contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">destinationPath is <c>null</c>.</exception>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="NativeError.ThrowException()"/>
      /// <param name="destinationPathFileName">The path to move the file to, which can specify a different file name.</param>
      /// <param name="moveOptions"><see cref="T:MoveOptions"/> that specify how the directory is to be moved. This parameter can be <c>null</c>.</param>
      /// <param name="isFullPath">
      ///    <para><c>true</c> <paramref name="destinationPathFileName"/> is an absolute path. Unicode prefix is applied.</para>
      ///    <para><c>false</c> <paramref name="destinationPathFileName"/> will be checked and resolved to an absolute path. Unicode prefix is applied.</para>
      ///    <para><c>null</c> <paramref name="destinationPathFileName"/> is already an absolute path with Unicode prefix. Use as is.</para>
      /// </param>
      [SecurityCritical]
      public FileInfo MoveTo1(string destinationPathFileName, MoveOptions moveOptions, bool? isFullPath)
      {
         string destinationPathLp;
         CopyToMoveToInternal(destinationPathFileName, false, null, moveOptions, null, null, out destinationPathLp, isFullPath);
         return new FileInfo(Transaction, destinationPathLp, null);
      }

      #endregion // FileInfo

      #region CopyMoveResult

      /// <summary>[AlphaFS] Moves a specified file to a new location, providing the option to specify a new file name,
      /// <para>and the possibility of notifying the application of its progress through a callback function.</para>
      /// <para>&#160;</para>
      /// <returns>Returns a <see cref="CopyMoveResult"/> class with the status of the Move action.</returns>
      /// <para>&#160;</para>
      /// <remarks>
      /// <para>Use this method to prevent overwriting of an existing file by default.</para>
      /// <para>This method works across disk volumes.</para>
      /// <para>For example, the file c:\MyFile.txt can be moved to d:\public and renamed NewFile.txt.</para>
      /// <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      /// <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <exception cref="ArgumentException">destinationPath contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">destinationPath is <c>null</c>.</exception>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="NativeError.ThrowException()"/>
      /// <param name="destinationPathFileName">The path to move the file to, which can specify a different file name.</param>
      /// <param name="progressHandler">A callback function that is called each time another portion of the directory has been moved. This parameter can be <c>null</c>.</param>
      /// <param name="userProgressData">The argument to be passed to the callback function. This parameter can be <c>null</c>.</param>
      /// <param name="isFullPath">
      ///    <para><c>true</c> <paramref name="destinationPathFileName"/> is an absolute path. Unicode prefix is applied.</para>
      ///    <para><c>false</c> <paramref name="destinationPathFileName"/> will be checked and resolved to an absolute path. Unicode prefix is applied.</para>
      ///    <para><c>null</c> <paramref name="destinationPathFileName"/> is already an absolute path with Unicode prefix. Use as is.</para>
      /// </param>
      [SecurityCritical]
      public CopyMoveResult MoveTo1(string destinationPathFileName, CopyMoveProgressRoutine progressHandler, object userProgressData, bool? isFullPath)
      {
         string destinationPathLp;
         CopyMoveResult cmr = CopyToMoveToInternal(destinationPathFileName, false, null, MoveOptions.CopyAllowed, progressHandler, userProgressData, out destinationPathLp, isFullPath);
         CopyToMoveToInternalRefresh(destinationPathFileName, destinationPathLp);
         return cmr;
      }
      
      /// <summary>[AlphaFS] Moves a specified file to a new location, providing the option to specify a new file name, <see cref="MoveOptions"/> can be specified,
      /// <para>and the possibility of notifying the application of its progress through a callback function.</para>
      /// <para>&#160;</para>
      /// <returns>Returns a <see cref="CopyMoveResult"/> class with the status of the Move action.</returns>
      /// <para>&#160;</para>
      /// <remarks>
      /// <para>Use this method to allow or prevent overwriting of an existing file.</para>
      /// <para>This method works across disk volumes.</para>
      /// <para>For example, the file c:\MyFile.txt can be moved to d:\public and renamed NewFile.txt.</para>
      /// <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      /// <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <exception cref="ArgumentException">destinationPath contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">destinationPath is <c>null</c>.</exception>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="NativeError.ThrowException()"/>
      /// <param name="destinationPathFileName">The path to move the file to, which can specify a different file name.</param>
      /// <param name="moveOptions"><see cref="T:MoveOptions"/> that specify how the directory is to be moved. This parameter can be <c>null</c>.</param>
      /// <param name="progressHandler">A callback function that is called each time another portion of the directory has been moved. This parameter can be <c>null</c>.</param>
      /// <param name="userProgressData">The argument to be passed to the callback function. This parameter can be <c>null</c>.</param>
      /// <param name="isFullPath">
      ///    <para><c>true</c> <paramref name="destinationPathFileName"/> is an absolute path. Unicode prefix is applied.</para>
      ///    <para><c>false</c> <paramref name="destinationPathFileName"/> will be checked and resolved to an absolute path. Unicode prefix is applied.</para>
      ///    <para><c>null</c> <paramref name="destinationPathFileName"/> is already an absolute path with Unicode prefix. Use as is.</para>
      /// </param>
      [SecurityCritical]
      public CopyMoveResult MoveTo1(string destinationPathFileName, MoveOptions moveOptions, CopyMoveProgressRoutine progressHandler, object userProgressData, bool? isFullPath)
      {
         string destinationPathLp;
         CopyMoveResult cmr = CopyToMoveToInternal(destinationPathFileName, false, null, moveOptions, progressHandler, userProgressData, out destinationPathLp, isFullPath);
         CopyToMoveToInternalRefresh(destinationPathFileName, destinationPathLp);
         return cmr;
      }

      #endregion // CopyMoveResult

      #endregion // IsFullPath

      #region FileInfo

      /// <summary>[AlphaFS] Moves a specified file to a new location, providing the option to specify a new file name, <see cref="MoveOptions"/> can be specified.
      /// <para>&#160;</para>
      /// <returns>
      /// <para>Returns a new <see cref="T:FileInfo"/> instance with a fully qualified path when successfully moved,</para>
      /// </returns>
      /// <para>&#160;</para>
      /// <remarks>
      /// <para>Use this method to allow or prevent overwriting of an existing file.</para>
      /// <para>This method works across disk volumes.</para>
      /// <para>For example, the file c:\MyFile.txt can be moved to d:\public and renamed NewFile.txt.</para>
      /// <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      /// <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <exception cref="ArgumentException">destinationPath contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">destinationPath is <c>null</c>.</exception>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="NativeError.ThrowException()"/>
      /// <param name="destinationPathFileName">The path to move the file to, which can specify a different file name.</param>
      /// <param name="moveOptions"><see cref="T:MoveOptions"/> that specify how the directory is to be moved. This parameter can be <c>null</c>.</param>
      [SecurityCritical]
      public FileInfo MoveTo1(string destinationPathFileName, MoveOptions moveOptions)
      {
         string destinationPathLp;
         CopyToMoveToInternal(destinationPathFileName, false, null, moveOptions, null, null, out destinationPathLp, false);
         return new FileInfo(Transaction, destinationPathLp, null);
      }

      #endregion // FileInfo

      #region CopyMoveResult

      /// <summary>[AlphaFS] Moves a specified file to a new location, providing the option to specify a new file name,
      /// <para>and the possibility of notifying the application of its progress through a callback function.</para>
      /// <para>&#160;</para>
      /// <returns>Returns a <see cref="CopyMoveResult"/> class with the status of the Move action.</returns>
      /// <para>&#160;</para>
      /// <remarks>
      /// <para>Use this method to prevent overwriting of an existing file by default.</para>
      /// <para>This method works across disk volumes.</para>
      /// <para>For example, the file c:\MyFile.txt can be moved to d:\public and renamed NewFile.txt.</para>
      /// <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      /// <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <exception cref="ArgumentException">destinationPath contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">destinationPath is <c>null</c>.</exception>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="NativeError.ThrowException()"/>
      /// <param name="destinationPathFileName">The path to move the file to, which can specify a different file name.</param>
      /// <param name="progressHandler">A callback function that is called each time another portion of the directory has been moved. This parameter can be <c>null</c>.</param>
      /// <param name="userProgressData">The argument to be passed to the callback function. This parameter can be <c>null</c>.</param>
      [SecurityCritical]
      public CopyMoveResult MoveTo1(string destinationPathFileName, CopyMoveProgressRoutine progressHandler, object userProgressData)
      {
         string destinationPathLp;
         CopyMoveResult cmr = CopyToMoveToInternal(destinationPathFileName, false, null, MoveOptions.CopyAllowed, progressHandler, userProgressData, out destinationPathLp, false);
         CopyToMoveToInternalRefresh(destinationPathFileName, destinationPathLp);
         return cmr;
      }
      
      /// <summary>[AlphaFS] Moves a specified file to a new location, providing the option to specify a new file name, <see cref="MoveOptions"/> can be specified,
      /// <para>and the possibility of notifying the application of its progress through a callback function.</para>
      /// <para>&#160;</para>
      /// <returns>Returns a <see cref="CopyMoveResult"/> class with the status of the Move action.</returns>
      /// <para>&#160;</para>
      /// <remarks>
      /// <para>Use this method to allow or prevent overwriting of an existing file.</para>
      /// <para>This method works across disk volumes.</para>
      /// <para>For example, the file c:\MyFile.txt can be moved to d:\public and renamed NewFile.txt.</para>
      /// <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      /// <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <exception cref="ArgumentException">destinationPath contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">destinationPath is <c>null</c>.</exception>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="NativeError.ThrowException()"/>
      /// <param name="destinationPathFileName">The path to move the file to, which can specify a different file name.</param>
      /// <param name="moveOptions"><see cref="T:MoveOptions"/> that specify how the directory is to be moved. This parameter can be <c>null</c>.</param>
      /// <param name="progressHandler">A callback function that is called each time another portion of the directory has been moved. This parameter can be <c>null</c>.</param>
      /// <param name="userProgressData">The argument to be passed to the callback function. This parameter can be <c>null</c>.</param>
      [SecurityCritical]
      public CopyMoveResult MoveTo1(string destinationPathFileName, MoveOptions moveOptions, CopyMoveProgressRoutine progressHandler, object userProgressData)
      {
         string destinationPathLp;
         CopyMoveResult cmr = CopyToMoveToInternal(destinationPathFileName, false, null, moveOptions, progressHandler, userProgressData, out destinationPathLp, false);
         CopyToMoveToInternalRefresh(destinationPathFileName, destinationPathLp);
         return cmr;
      }

      #endregion // CopyMoveResult

      #endregion // MoveTo1

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

      /// <summary>[AlphaFS] Unified method CopyToMoveToInternal() to copy/move an existing file to a new file, allowing the overwriting of an existing file.
      /// <para>&#160;</para>
      /// <returns>Returns a <see cref="CopyMoveResult"/> class with the status of the Copy or Move action.</returns>
      /// <para>&#160;</para>
      /// <remarks>
      /// <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      /// <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      /// <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <exception cref="ArgumentException">destinationPath contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">destinationPath is <c>null</c>.</exception>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="NativeError.ThrowException()"/>
      /// <param name="destinationPath"><para>A full path string to the destination directory</para></param>
      /// <param name="preserveDates"><c>true</c> if original Timestamps must be preserved, <c>false</c> otherwise.</param>
      /// <param name="copyOptions"><para>This parameter can be <c>null</c>. Use <see cref="T:CopyOptions"/> to specify how the file is to be copied.</para></param>
      /// <param name="moveOptions"><para>This parameter can be <c>null</c>. Use <see cref="T:MoveOptions"/> that specify how the file is to be moved.</para></param>
      /// <param name="progressHandler"><para>This parameter can be <c>null</c>. A callback function that is called each time another portion of the file has been copied.</para></param>
      /// <param name="userProgressData"><para>This parameter can be <c>null</c>. The argument to be passed to the callback function.</para></param>
      /// <param name="longFullPath">Returns the retrieved long full path.</param>
      /// <param name="isFullPath">
      ///    <para><c>true</c> <paramref name="destinationPath"/> is an absolute path. Unicode prefix is applied.</para>
      ///    <para><c>false</c> <paramref name="destinationPath"/> will be checked and resolved to an absolute path. Unicode prefix is applied.</para>
      ///    <para><c>null</c> <paramref name="destinationPath"/> is already an absolute path with Unicode prefix. Use as is.</para>
      /// </param>
      [SecurityCritical]
      private CopyMoveResult CopyToMoveToInternal(string destinationPath, bool preserveDates, CopyOptions? copyOptions, MoveOptions? moveOptions, CopyMoveProgressRoutine progressHandler, object userProgressData, out string longFullPath, bool? isFullPath)
      {
         string destinationPathLp = isFullPath == null
            ? destinationPath
            : (bool) isFullPath
               ? Path.GetLongPathInternal(destinationPath, false, false, false, false)
#if NET35
               : Path.GetFullPathInternal(Transaction, destinationPath, true, false, false, true, false, true, true);
#else
               : Path.GetFullPathInternal(Transaction, destinationPath, true, true, false, true, false, true, true);
#endif

         longFullPath = destinationPathLp;

         // Returns false when CopyMoveProgressResult is PROGRESS_CANCEL or PROGRESS_STOP.
         return File.CopyMoveInternal(false, Transaction, LongFullName, destinationPathLp, preserveDates, copyOptions, moveOptions, progressHandler, userProgressData, isFullPath);
      }

      private void CopyToMoveToInternalRefresh(string destinationPath, string destinationPathLp)
      {
         LongFullName = destinationPathLp;
         FullPath = Path.GetRegularPathInternal(destinationPathLp, false, false, false, false);

         OriginalPath = destinationPath;
         DisplayPath = OriginalPath;

         _name = Path.GetFileName(destinationPathLp, true);

         // Flush any cached information about the file.
         Reset();
      }

      #endregion // CopyToMoveToInternal

      #endregion // Unified Internals

      #endregion // AlphaFS

      #endregion // Methods

      #region Properties

      #region .NET

      #region Directory

      /// <summary>Gets an instance of the parent directory.
      /// <para>&#160;</para>
      /// <value>A <see cref="T:DirectoryInfo"/> object representing the parent directory of this file.</value>
      /// <para>&#160;</para>
      /// <remarks>To get the parent directory as a string, use the DirectoryName property.</remarks>
      /// </summary>
      /// <exception cref="DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive.</exception>
      public DirectoryInfo Directory
      {
         get
         {
            string dirName = DirectoryName;
            return dirName == null ? null : new DirectoryInfo(Transaction, dirName, true);
         }
      }

      #endregion // Directory

      #region DirectoryName

      /// <summary>Gets a string representing the directory's full path.
      /// <para>&#160;</para>
      /// <value>A string representing the directory's full path.</value>
      /// <para>&#160;</para>
      /// <remarks>
      /// <para>To get the parent directory as a DirectoryInfo object, use the Directory property.</para>
      /// <para>When first called, FileInfo calls Refresh and caches information about the file.</para>
      /// <para>On subsequent calls, you must call Refresh to get the latest copy of the information.</para>
      /// </remarks>
      /// </summary>
      /// <exception cref="ArgumentNullException">null was passed in for the directory name.</exception>
      public string DirectoryName
      {
         [SecurityCritical] get { return Path.GetDirectoryName(FullPath, false); }
      }

      #endregion // DirectoryName

      #region Exists

      /// <summary>Gets a value indicating whether the file exists.
      /// <para>&#160;</para>
      /// <value><c>true</c> if the file exists; otherwise, <c>false</c>.</value>
      /// <para>&#160;</para>
      /// <remarks>
      /// <para>The <see cref="T:Exists"/> property returns <c>false</c> if any error occurs while trying to determine if the specified file exists.</para>
      /// <para>This can occur in situations that raise exceptions such as passing a file name with invalid characters or too many characters,</para>
      /// <para>a failing or missing disk, or if the caller does not have permission to read the file.</para>
      /// </remarks>
      /// </summary>
      [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
      public override bool Exists
      {
         [SecurityCritical]
         get
         {
            try
            {
               if (DataInitialised == -1)
                  Refresh();

               return DataInitialised == 0 && (Win32AttributeData.FileAttributes & FileAttributes.Directory) == 0;
            }
            catch
            {
               return false;
            }
         }
      }

      #endregion // Exists

      #region IsReadOnly

      /// <summary>Gets or sets a value that determines if the current file is read only.
      /// <para>&#160;</para>
      /// <value><c>true</c> if the current file is read only; otherwise, <c>false</c>.</value>
      /// <para>&#160;</para>
      /// <remarks>
      /// <para>Use the IsReadOnly property to quickly determine or change whether the current file is read only.</para>
      /// <para>When first called, FileInfo calls Refresh and caches information about the file.</para>
      /// <para>On subsequent calls, you must call Refresh to get the latest copy of the information.</para>
      /// </remarks>
      /// </summary>
      /// <exception cref="FileNotFoundException">The file described by the current FileInfo object could not be found.</exception>
      /// <exception cref="IOException">An I/O error occurred while opening the file.</exception>
      public bool IsReadOnly
      {
         get { return (Attributes & FileAttributes.ReadOnly) != 0; }

         set
         {
            if (value)
               Attributes |= FileAttributes.ReadOnly;
            else
               Attributes &= ~FileAttributes.ReadOnly;
         }
      }

      #endregion // IsReadOnly

      #region Length

      /// <summary>Gets the size, in bytes, of the current file.
      /// <para>&#160;</para>
      /// <value>The size of the current file in bytes.</value>
      /// <para>&#160;</para>
      /// <remarks>
      /// <para>The value of the Length property is pre-cached</para>
      /// <para>To get the latest value, call the Refresh method.</para>
      /// </remarks>
      /// <exception cref="System.IO.FileNotFoundException">The file does not exist or the Length property is called for a directory.</exception>
      /// <exception cref="System.IO.IOException"/>
      /// </summary>
      [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations", Justification = ".NET also throws FileNotFoundException().")]
      public long Length
      {
         [SecurityCritical]
         get
         {
            if (DataInitialised == -1)
            {
               Win32AttributeData = new NativeMethods.Win32FileAttributeData();
               Refresh();
            }

            // MSDN: .NET 3.5+: IOException: Refresh cannot initialize the data. 
            if (DataInitialised != 0)
               NativeError.ThrowException(DataInitialised, DisplayPath, true);

            if ((Win32AttributeData.FileAttributes & FileAttributes.Directory) != 0)
               NativeError.ThrowException(Win32Errors.ERROR_FILE_NOT_FOUND, DisplayPath, true);

            return Win32AttributeData.FileSize;
         }
      }

      #endregion // Length

      #region Name

      private string _name;

      /// <summary>Gets the name of the file.
      /// <para>&#160;</para>
      /// <value>The name of the file.</value>
      /// <para>&#160;</para>
      /// <remarks>
      /// <para>The name of the file includes the file extension.</para>
      /// <para>When first called, <see cref="T:FileInfo"/> calls Refresh and caches information about the file.</para>
      /// <para>On subsequent calls, you must call Refresh to get the latest copy of the information.</para>
      /// <para>The name of the file includes the file extension.</para>
      /// </remarks>
      /// </summary>
      public override string Name
      {
         get { return _name; }
      }

      #endregion // Name

      #endregion // .NET

      #endregion // Properties
   }
}