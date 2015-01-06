using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.AccessControl;
using System.Text;
using FileStream = System.IO.FileStream;
using StreamReader = System.IO.StreamReader;

namespace Alphaleonis.Win32.Filesystem
{
   public static partial class File
   {
      #region Open

      /// <summary>Opens a <see cref="FileStream"/> on the specified path with read/write access.</summary>
      /// <param name="path">The file to open.</param>
      /// <param name="mode">A <see cref="FileMode"/> value that specifies whether a file is created if one does not exist, and determines whether the contents of existing files are retained or overwritten.</param>
      /// <returns>A <see cref="FileStream"/> opened in the specified mode and path, with read/write access and not shared.</returns>
      [SecurityCritical]
      public static FileStream Open(string path, FileMode mode)
      {
         return OpenInternal(null, path, mode, 0, mode == FileMode.Append ? FileAccess.Write : FileAccess.ReadWrite, FileShare.None, ExtendedFileAttributes.Normal, PathFormat.RelativeOrFullPath);
      }

      /// <summary>Opens a <see cref="FileStream"/> on the specified path, with the specified mode and access.</summary>
      /// <param name="path">The file to open.</param>
      /// <param name="mode">A <see cref="FileMode"/> value that specifies whether a file is created if one does not exist, and determines whether the contents of existing files are retained or overwritten.</param>
      /// <param name="access">A <see cref="FileAccess"/> value that specifies the operations that can be performed on the file.</param>
      /// <returns>An unshared <see cref="FileStream"/> that provides access to the specified file, with the specified mode and access.</returns>
      [SecurityCritical]
      public static FileStream Open(string path, FileMode mode, FileAccess access)
      {
         return OpenInternal(null, path, mode, 0, access, FileShare.None, ExtendedFileAttributes.Normal, PathFormat.RelativeOrFullPath);
      }

      /// <summary>Opens a <see cref="FileStream"/> on the specified path, having the specified mode with read, write, or read/write access and the specified sharing option.</summary>
      /// <param name="path">The file to open.</param>
      /// <param name="mode">A <see cref="FileMode"/> value that specifies whether a file is created if one does not exist, and determines whether the contents of existing files are retained or overwritten.</param>
      /// <param name="access">A <see cref="FileAccess"/> value that specifies the operations that can be performed on the file.</param>
      /// <param name="share">A <see cref="FileShare"/> value specifying the type of access other threads have to the file.</param>
      /// <returns>A <see cref="FileStream"/> on the specified path, having the specified mode with read, write, or read/write access and the specified sharing option.</returns>
      [SecurityCritical]
      public static FileStream Open(string path, FileMode mode, FileAccess access, FileShare share)
      {
         return OpenInternal(null, path, mode, 0, access, share, ExtendedFileAttributes.Normal, PathFormat.RelativeOrFullPath);
      }

      /// <summary>[AlphaFS] Opens a <see cref="FileStream"/> on the specified path with read/write access.</summary>
      /// <param name="path">The file to open.</param>
      /// <param name="mode">
      ///   A <see cref="FileMode"/> value that specifies whether a file is created if one does not exist, and determines whether the contents
      ///   of existing files are retained or overwritten.
      /// </param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>A <see cref="FileStream"/> opened in the specified mode and path, with read/write access and not shared.</returns>
      [SecurityCritical]
      public static FileStream Open(string path, FileMode mode, PathFormat pathFormat)
      {
         return OpenInternal(null, path, mode, 0, mode == FileMode.Append ? FileAccess.Write : FileAccess.ReadWrite, FileShare.None, ExtendedFileAttributes.Normal, pathFormat);
      }

      /// <summary>[AlphaFS] Opens a <see cref="FileStream"/> on the specified path, with the specified mode and access.</summary>
      /// <param name="path">The file to open.</param>
      /// <param name="mode">
      ///   A <see cref="FileMode"/> value that specifies whether a file is created if one does not exist, and determines whether the contents
      ///   of existing files are retained or overwritten.
      /// </param>
      /// <param name="access">A <see cref="FileAccess"/> value that specifies the operations that can be performed on the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>
      ///   An unshared <see cref="FileStream"/> that provides access to the specified file, with the specified mode and access.
      /// </returns>
      [SecurityCritical]
      public static FileStream Open(string path, FileMode mode, FileAccess access, PathFormat pathFormat)
      {
         return OpenInternal(null, path, mode, 0, access, FileShare.None, ExtendedFileAttributes.Normal, pathFormat);
      }

      /// <summary>
      ///   [AlphaFS] Opens a <see cref="FileStream"/> on the specified path, having the specified mode with read, write, or read/write access
      ///   and the specified sharing option.
      /// </summary>
      /// <param name="path">The file to open.</param>
      /// <param name="mode">
      ///   A <see cref="FileMode"/> value that specifies whether a file is created if one does not exist, and determines whether the contents
      ///   of existing files are retained or overwritten.
      /// </param>
      /// <param name="access">A <see cref="FileAccess"/> value that specifies the operations that can be performed on the file.</param>
      /// <param name="share">A <see cref="FileShare"/> value specifying the type of access other threads have to the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>
      ///   A <see cref="FileStream"/> on the specified path, having the specified mode with read, write, or read/write access and the
      ///   specified sharing option.
      /// </returns>
      [SecurityCritical]
      public static FileStream Open(string path, FileMode mode, FileAccess access, FileShare share, PathFormat pathFormat)
      {
         return OpenInternal(null, path, mode, 0, access, share, ExtendedFileAttributes.Normal, pathFormat);
      }


      #region Transacted

      /// <summary>[AlphaFS] (Transacted) Opens a <see cref="FileStream"/> on the specified path with read/write access.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to open.</param>
      /// <param name="mode">
      ///   A <see cref="FileMode"/> value that specifies whether a file is created if one does not exist, and determines whether the contents
      ///   of existing files are retained or overwritten.
      /// </param>
      /// <returns>A <see cref="FileStream"/> opened in the specified mode and path, with read/write access and not shared.</returns>
      [SecurityCritical]
      public static FileStream Open(KernelTransaction transaction, string path, FileMode mode)
      {
         return OpenInternal(transaction, path, mode, 0, mode == FileMode.Append ? FileAccess.Write : FileAccess.ReadWrite, FileShare.None, ExtendedFileAttributes.Normal, PathFormat.RelativeOrFullPath);
      }

      /// <summary>[AlphaFS] Opens a <see cref="FileStream"/> on the specified path, with the specified mode and access.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to open.</param>
      /// <param name="mode">
      ///   A <see cref="FileMode"/> value that specifies whether a file is created if one does not exist, and determines whether the contents
      ///   of existing files are retained or overwritten.
      /// </param>
      /// <param name="access">A <see cref="FileAccess"/> value that specifies the operations that can be performed on the file.</param>
      /// <returns>
      ///   An unshared <see cref="FileStream"/> that provides access to the specified file, with the specified mode and access.
      /// </returns>
      [SecurityCritical]
      public static FileStream Open(KernelTransaction transaction, string path, FileMode mode, FileAccess access)
      {
         return OpenInternal(transaction, path, mode, 0, access, FileShare.None, ExtendedFileAttributes.Normal, PathFormat.RelativeOrFullPath);
      }

      /// <summary>
      ///   [AlphaFS] Opens a <see cref="FileStream"/> on the specified path, having the specified mode with read, write, or read/write access
      ///   and the specified sharing option.
      /// </summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to open.</param>
      /// <param name="mode">
      ///   A <see cref="FileMode"/> value that specifies whether a file is created if one does not exist, and determines whether the contents
      ///   of existing files are retained or overwritten.
      /// </param>
      /// <param name="access">A <see cref="FileAccess"/> value that specifies the operations that can be performed on the file.</param>
      /// <param name="share">A <see cref="FileShare"/> value specifying the type of access other threads have to the file.</param>
      /// <returns>
      ///   A <see cref="FileStream"/> on the specified path, having the specified mode with read, write, or read/write access and the
      ///   specified sharing option.
      /// </returns>
      [SecurityCritical]
      public static FileStream Open(KernelTransaction transaction, string path, FileMode mode, FileAccess access, FileShare share)
      {
         return OpenInternal(transaction, path, mode, 0, access, share, ExtendedFileAttributes.Normal, PathFormat.RelativeOrFullPath);
      }

      /// <summary>[AlphaFS] (Transacted) Opens a <see cref="FileStream"/> on the specified path with read/write access.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to open.</param>
      /// <param name="mode">
      ///   A <see cref="FileMode"/> value that specifies whether a file is created if one does not exist, and determines whether the contents
      ///   of existing files are retained or overwritten.
      /// </param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>A <see cref="FileStream"/> opened in the specified mode and path, with read/write access and not shared.</returns>
      [SecurityCritical]
      public static FileStream Open(KernelTransaction transaction, string path, FileMode mode, PathFormat pathFormat)
      {
         return OpenInternal(transaction, path, mode, 0, mode == FileMode.Append ? FileAccess.Write : FileAccess.ReadWrite, FileShare.None, ExtendedFileAttributes.Normal, pathFormat);
      }

      /// <summary>[AlphaFS] Opens a <see cref="FileStream"/> on the specified path, with the specified mode and access.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to open.</param>
      /// <param name="mode">
      ///   A <see cref="FileMode"/> value that specifies whether a file is created if one does not exist, and determines whether the contents
      ///   of existing files are retained or overwritten.
      /// </param>
      /// <param name="access">A <see cref="FileAccess"/> value that specifies the operations that can be performed on the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>
      ///   An unshared <see cref="FileStream"/> that provides access to the specified file, with the specified mode and access.
      /// </returns>
      [SecurityCritical]
      public static FileStream Open(KernelTransaction transaction, string path, FileMode mode, FileAccess access, PathFormat pathFormat)
      {
         return OpenInternal(transaction, path, mode, 0, access, FileShare.None, ExtendedFileAttributes.Normal, pathFormat);
      }

      /// <summary>[AlphaFS] Opens a <see cref="FileStream"/> on the specified path, having the specified mode with read, write, or read/write access and the specified sharing option.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to open.</param>
      /// <param name="mode">A <see cref="FileMode"/> value that specifies whether a file is created if one does not exist, and determines whether the contents of existing files are retained or overwritten.</param>
      /// <param name="access">A <see cref="FileAccess"/> value that specifies the operations that can be performed on the file.</param>
      /// <param name="share">A <see cref="FileShare"/> value specifying the type of access other threads have to the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>A <see cref="FileStream"/> on the specified path, having the specified mode with read, write, or read/write access and the specified sharing option.</returns>
      [SecurityCritical]
      public static FileStream Open(KernelTransaction transaction, string path, FileMode mode, FileAccess access, FileShare share, PathFormat pathFormat)
      {
         return OpenInternal(transaction, path, mode, 0, access, share, ExtendedFileAttributes.Normal, pathFormat);
      }


      #endregion // Transacted


      #endregion // Open

      #region OpenRead

      /// <summary>Opens an existing file for reading.</summary>
      /// <remarks>
      ///   This method is equivalent to the FileStream(string, FileMode, FileAccess, FileShare) constructor overload with a
      ///   <see cref="FileMode"/> value of Open, a <see cref="FileAccess"/> value of Read and a <see cref="FileShare"/> value of Read.
      /// </remarks>
      /// <param name="path">The file to be opened for reading.</param>
      /// <returns>A read-only <see cref="FileStream"/> on the specified path.</returns>
      [SecurityCritical]
      public static FileStream OpenRead(string path)
      {
         return OpenInternal(null, path, FileMode.Open, 0, FileAccess.Read, FileShare.Read, ExtendedFileAttributes.Normal, PathFormat.RelativeOrFullPath);
      }

      /// <summary>[AlphaFS] Opens an existing file for reading.</summary>
      /// <remarks>
      ///   This method is equivalent to the FileStream(string, FileMode, FileAccess, FileShare) constructor overload with a
      ///   <see cref="FileMode"/> value of Open, a <see cref="FileAccess"/> value of Read and a <see cref="FileShare"/> value of Read.
      /// </remarks>
      /// <param name="path">The file to be opened for reading.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>A read-only <see cref="FileStream"/> on the specified path.</returns>
      [SecurityCritical]
      public static FileStream OpenRead(string path, PathFormat pathFormat)
      {
         return OpenInternal(null, path, FileMode.Open, 0, FileAccess.Read, FileShare.Read, ExtendedFileAttributes.Normal, pathFormat);
      }

      #region Transacted

      /// <summary>[AlphaFS] Opens an existing file for reading.</summary>
      /// <remarks>
      ///   This method is equivalent to the FileStream(string, FileMode, FileAccess, FileShare) constructor overload with a
      ///   <see cref="FileMode"/> value of Open, a <see cref="FileAccess"/> value of Read and a <see cref="FileShare"/> value of Read.
      /// </remarks>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to be opened for reading.</param>
      /// <returns>A read-only <see cref="FileStream"/> on the specified path.</returns>
      [SecurityCritical]
      public static FileStream OpenRead(KernelTransaction transaction, string path)
      {
         return OpenInternal(transaction, path, FileMode.Open, 0, FileAccess.Read, FileShare.Read, ExtendedFileAttributes.Normal, PathFormat.RelativeOrFullPath);
      }

      /// <summary>[AlphaFS] Opens an existing file for reading.</summary>
      /// <remarks>
      ///   This method is equivalent to the FileStream(string, FileMode, FileAccess, FileShare) constructor overload with a
      ///   <see cref="FileMode"/> value of Open, a <see cref="FileAccess"/> value of Read and a <see cref="FileShare"/> value of Read.
      /// </remarks>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to be opened for reading.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>A read-only <see cref="FileStream"/> on the specified path.</returns>
      [SecurityCritical]
      public static FileStream OpenRead(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         return OpenInternal(transaction, path, FileMode.Open, 0, FileAccess.Read, FileShare.Read, ExtendedFileAttributes.Normal, pathFormat);
      }


      #endregion // Transacted

      #endregion // OpenRead

      #region OpenText

      /// <summary>Opens an existing NativeMethods.DefaultFileEncoding encoded text file for reading.</summary>
      /// <param name="path">The file to be opened for reading.</param>
      /// <returns>A <see cref="StreamReader"/> on the specified path.</returns>
      [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
      [SecurityCritical]
      public static StreamReader OpenText(string path)
      {
         return new StreamReader(OpenInternal(null, path, FileMode.Open, 0, FileAccess.Read, FileShare.Read, ExtendedFileAttributes.Normal, PathFormat.RelativeOrFullPath), NativeMethods.DefaultFileEncoding);
      }

      /// <summary>[AlphaFS] Opens an existing NativeMethods.DefaultFileEncoding encoded text file for reading.</summary>
      /// <param name="path">The file to be opened for reading.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>A <see cref="StreamReader"/> on the specified path.</returns>
      [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
      [SecurityCritical]
      public static StreamReader OpenText(string path, PathFormat pathFormat)
      {
         return new StreamReader(OpenInternal(null, path, FileMode.Open, 0, FileAccess.Read, FileShare.Read, ExtendedFileAttributes.Normal, pathFormat), NativeMethods.DefaultFileEncoding);
      }

      /// <summary>[AlphaFS] Opens an existing <see cref="Encoding"/> encoded text file for reading.</summary>
      /// <param name="path">The file to be opened for reading.</param>
      /// <param name="encoding">The <see cref="Encoding"/> applied to the contents of the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>A <see cref="StreamReader"/> on the specified path.</returns>
      [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
      [SecurityCritical]
      public static StreamReader OpenText(string path, Encoding encoding, PathFormat pathFormat)
      {
         return new StreamReader(OpenInternal(null, path, FileMode.Open, 0, FileAccess.Read, FileShare.Read, ExtendedFileAttributes.Normal, pathFormat), encoding);
      }


      /// <summary>[AlphaFS] Opens an existing <see cref="Encoding"/> encoded text file for reading.</summary>
      /// <param name="path">The file to be opened for reading.</param>
      /// <param name="encoding">The <see cref="Encoding"/> applied to the contents of the file.</param>
      /// <returns>A <see cref="StreamReader"/> on the specified path.</returns>
      [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
      [SecurityCritical]
      public static StreamReader OpenText(string path, Encoding encoding)
      {
         return new StreamReader(OpenInternal(null, path, FileMode.Open, 0, FileAccess.Read, FileShare.Read, ExtendedFileAttributes.Normal, PathFormat.RelativeOrFullPath), encoding);
      }

      #region Transacted

      /// <summary>[AlphaFS] Opens an existing NativeMethods.DefaultFileEncoding encoded text file for reading.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to be opened for reading.</param>
      /// <returns>A <see cref="StreamReader"/> on the specified path.</returns>
      [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
      public static StreamReader OpenText(KernelTransaction transaction, string path)
      {
         return new StreamReader(OpenInternal(transaction, path, FileMode.Open, 0, FileAccess.Read, FileShare.Read, ExtendedFileAttributes.Normal, PathFormat.RelativeOrFullPath), NativeMethods.DefaultFileEncoding);
      }

      /// <summary>[AlphaFS] Opens an existing NativeMethods.DefaultFileEncoding encoded text file for reading.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to be opened for reading.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>A <see cref="StreamReader"/> on the specified path.</returns>
      [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
      public static StreamReader OpenText(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         return new StreamReader(OpenInternal(transaction, path, FileMode.Open, 0, FileAccess.Read, FileShare.Read, ExtendedFileAttributes.Normal, pathFormat), NativeMethods.DefaultFileEncoding);
      }

      /// <summary>[AlphaFS] Opens an existing <see cref="Encoding"/> encoded text file for reading.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to be opened for reading.</param>
      /// <param name="encoding">The <see cref="Encoding"/> applied to the contents of the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>A <see cref="StreamReader"/> on the specified path.</returns>
      [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
      public static StreamReader OpenText(KernelTransaction transaction, string path, Encoding encoding, PathFormat pathFormat)
      {
         return new StreamReader(OpenInternal(transaction, path, FileMode.Open, 0, FileAccess.Read, FileShare.Read, ExtendedFileAttributes.Normal, pathFormat), encoding);
      }

      /// <summary>[AlphaFS] Opens an existing <see cref="Encoding"/> encoded text file for reading.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to be opened for reading.</param>
      /// <param name="encoding">The <see cref="Encoding"/> applied to the contents of the file.</param>
      /// <returns>A <see cref="StreamReader"/> on the specified path.</returns>
      [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
      public static StreamReader OpenText(KernelTransaction transaction, string path, Encoding encoding)
      {
         return new StreamReader(OpenInternal(transaction, path, FileMode.Open, 0, FileAccess.Read, FileShare.Read, ExtendedFileAttributes.Normal, PathFormat.RelativeOrFullPath), encoding);
      }

      #endregion // Transacted


      #endregion // OpenText

      #region OpenWrite

      /// <summary>Opens an existing file or creates a new file for writing.</summary>
      /// <param name="path">The file to be opened for writing.</param>
      /// <returns>An unshared <see cref="FileStream"/> object on the specified path with <see cref="FileAccess.Write"/> access.</returns>
      [SecurityCritical]
      public static FileStream OpenWrite(string path)
      {
         return OpenInternal(null, path, FileMode.OpenOrCreate, 0, FileAccess.Write, FileShare.None, ExtendedFileAttributes.Normal, PathFormat.RelativeOrFullPath);
      }

      /// <summary>[AlphaFS] Opens an existing file or creates a new file for writing.</summary>
      /// <param name="path">The file to be opened for writing.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>An unshared <see cref="FileStream"/> object on the specified path with <see cref="FileAccess.Write"/> access.</returns>
      [SecurityCritical]
      public static FileStream OpenWrite(string path, PathFormat pathFormat)
      {
         return OpenInternal(null, path, FileMode.OpenOrCreate, 0, FileAccess.Write, FileShare.None, ExtendedFileAttributes.Normal, pathFormat);
      }

      #region Transacted

      /// <summary>[AlphaFS] Opens an existing file or creates a new file for writing.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to be opened for writing.</param>
      /// <returns>An unshared <see cref="FileStream"/> object on the specified path with <see cref="FileAccess.Write"/> access.</returns>
      [SecurityCritical]
      public static FileStream OpenWrite(KernelTransaction transaction, string path)
      {
         return OpenInternal(transaction, path, FileMode.OpenOrCreate, 0, FileAccess.Write, FileShare.None, ExtendedFileAttributes.Normal, PathFormat.RelativeOrFullPath);
      }

      /// <summary>[AlphaFS] Opens an existing file or creates a new file for writing.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to be opened for writing.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>An unshared <see cref="FileStream"/> object on the specified path with <see cref="FileAccess.Write"/> access.</returns>
      [SecurityCritical]
      public static FileStream OpenWrite(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         return OpenInternal(transaction, path, FileMode.OpenOrCreate, 0, FileAccess.Write, FileShare.None, ExtendedFileAttributes.Normal, pathFormat);
      }

      #endregion // Transacted


      #endregion // OpenWrite

      #region OpenBackupRead

      /// <summary>[AlphaFS] Opens the specified file for reading purposes bypassing security attributes.</summary>
      /// <param name="path">The file path to open.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>A <see cref="FileStream"/> on the specified path, having the read-only mode and sharing options.</returns>
      [SecurityCritical]
      public static FileStream OpenBackupRead(string path, PathFormat pathFormat)
      {
         return OpenInternal(null, path, FileMode.Open, FileSystemRights.ReadData, FileAccess.Read, FileShare.None, ExtendedFileAttributes.BackupSemantics | ExtendedFileAttributes.SequentialScan | ExtendedFileAttributes.ReadOnly, pathFormat);
      }


      /// <summary>
      ///   [AlphaFS] Opens the specified file for reading purposes bypassing security attributes.
      ///   This method is simpler to use then BackupFileStream to read only file's data stream.
      /// </summary>
      /// <param name="path">The file path to open.</param>
      /// <returns>A <see cref="FileStream"/> on the specified path, having the read-only mode and sharing options.</returns>      
      [SecurityCritical]
      public static FileStream OpenBackupRead(string path)
      {
         return OpenInternal(null, path, FileMode.Open, FileSystemRights.ReadData, FileAccess.Read, FileShare.None, ExtendedFileAttributes.BackupSemantics | ExtendedFileAttributes.SequentialScan | ExtendedFileAttributes.ReadOnly, PathFormat.LongFullPath);
      }

      #region Transacted

      /// <summary>[AlphaFS] Opens the specified file for reading purposes bypassing security attributes.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file path to open.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>A <see cref="FileStream"/> on the specified path, having the read-only mode and sharing options.</returns>
      [SecurityCritical]
      public static FileStream OpenBackupRead(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         return OpenInternal(transaction, path, FileMode.Open, FileSystemRights.ReadData, FileAccess.Read, FileShare.None, ExtendedFileAttributes.BackupSemantics | ExtendedFileAttributes.SequentialScan | ExtendedFileAttributes.ReadOnly, pathFormat);
      }

      /// <summary>[AlphaFS] Opens the specified file for reading purposes bypassing security attributes.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file path to open.</param>
      /// <returns>A <see cref="FileStream"/> on the specified path, having the read-only mode and sharing options.</returns>
      [SecurityCritical]
      public static FileStream OpenBackupRead(KernelTransaction transaction, string path)
      {
         return OpenInternal(transaction, path, FileMode.Open, FileSystemRights.ReadData, FileAccess.Read, FileShare.None, ExtendedFileAttributes.BackupSemantics | ExtendedFileAttributes.SequentialScan | ExtendedFileAttributes.ReadOnly, PathFormat.LongFullPath);
      }

      #endregion // Transacted

      #endregion // OpenBackupRead

      #region Internal Methods

      /// <summary>
      ///   [AlphaFS] Unified method OpenInternal() to open a <see cref="FileStream"/> on the specified path, having the specified mode with
      ///   <para>read, write, or read/write access, the specified sharing option and additional options specified.</para>
      /// </summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to open.</param>
      /// <param name="mode">
      ///   A <see cref="FileMode"/> value that specifies whether a file is created if one does not exist, and determines whether the contents
      ///   of existing files are retained or overwritten.
      /// </param>
      /// <param name="rights">
      ///   A <see cref="FileSystemRights"/> value that specifies whether a file is created if one does not exist, and determines whether the
      ///   contents of existing files are retained or overwritten along with additional options.
      /// </param>
      /// <param name="access">A <see cref="FileAccess"/> value that specifies the operations that can be performed on the file.</param>
      /// <param name="share">A <see cref="FileShare"/> value specifying the type of access other threads have to the file.</param>
      /// <param name="attributes">Advanced <see cref="ExtendedFileAttributes"/> options for this file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>
      ///   <para>A <see cref="FileStream"/> instance on the specified path, having the specified mode with</para>
      ///   <para>read, write, or read/write access and the specified sharing option.</para>
      /// </returns>
      [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
      [SecurityCritical]
      internal static FileStream OpenInternal(KernelTransaction transaction, string path, FileMode mode, FileSystemRights rights, FileAccess access, FileShare share, ExtendedFileAttributes attributes, PathFormat pathFormat)
      {
         SafeFileHandle safeHandle = CreateFileInternal(transaction, path, attributes, null, mode, rights != 0 ? rights : (FileSystemRights)access, share, true, pathFormat);

         return rights != 0
            ? new FileStream(safeHandle, FileAccess.Write)
            : new FileStream(safeHandle, access);
      }

      #endregion // OpenInternal
   }
}
