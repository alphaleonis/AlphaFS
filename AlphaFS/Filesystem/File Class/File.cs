/* Copyright (C) 2008-2015 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
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

using Alphaleonis.Win32.Security;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.AccessControl;
using System.Text;
using FileStream = System.IO.FileStream;
using StreamReader = System.IO.StreamReader;
using StreamWriter = System.IO.StreamWriter;

namespace Alphaleonis.Win32.Filesystem
{

   /// <summary>Provides static methods for the creation, copying, deletion, moving, and opening of files, and aids in the creation of <see cref="FileStream"/> objects.
   /// <para>This class cannot be inherited.</para>
   /// </summary>
   [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
   public static partial class File
   {
      #region .NET

      #region GetLastWriteTime

      #region .NET

      /// <summary>Gets the date and time that the specified file was last written to.</summary>
      /// <param name="path">The file for which to obtain write date and time information.</param>
      /// <returns>
      ///   A <see cref="System.DateTime"/> structure set to the date and time that the specified file was last written to. This value is
      ///   expressed in local time.
      /// </returns>
      [SecurityCritical]
      public static DateTime GetLastWriteTime(string path)
      {
         return GetLastWriteTimeInternal(null, path, false, PathFormat.Auto).ToLocalTime();
      }

      #endregion //.NET

      #region AlphaFS

      #region IsFullPath

      /// <summary>[AlphaFS] Gets the date and time that the specified file was last written to.</summary>
      /// <param name="path">The file for which to obtain write date and time information.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>
      ///   A <see cref="System.DateTime"/> structure set to the date and time that the specified file was last written to. This value is
      ///   expressed in local time.
      /// </returns>
      [SecurityCritical]
      public static DateTime GetLastWriteTime(string path, PathFormat pathFormat)
      {
         return GetLastWriteTimeInternal(null, path, false, pathFormat).ToLocalTime();
      }

      #endregion // IsFullPath

      #region Transacted

      #region .NET

      /// <summary>[AlphaFS] Gets the date and time that the specified file was last written to.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file for which to obtain write date and time information.</param>
      /// <returns>
      ///   A <see cref="System.DateTime"/> structure set to the date and time that the specified file was last written to. This value is
      ///   expressed in local time.
      /// </returns>
      [SecurityCritical]
      public static DateTime GetLastWriteTime(KernelTransaction transaction, string path)
      {
         return GetLastWriteTimeInternal(transaction, path, false, PathFormat.Auto).ToLocalTime();
      }

      #endregion // .NET

      #region IsFullPath

      /// <summary>[AlphaFS] Gets the date and time that the specified file was last written to.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file for which to obtain write date and time information.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>
      ///   A <see cref="System.DateTime"/> structure set to the date and time that the specified file was last written to. This value is
      ///   expressed in local time.
      /// </returns>
      [SecurityCritical]
      public static DateTime GetLastWriteTime(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         return GetLastWriteTimeInternal(transaction, path, false, pathFormat).ToLocalTime();
      }

      #endregion // IsFullPath

      #endregion // Transacted

      #endregion // AlphaFS

      #endregion // GetLastWriteTime

      #region GetLastWriteTimeUtc

      #region .NET

      /// <summary>Gets the date and time, in coordinated universal time (UTC) time, that the specified file was last written to.</summary>
      /// <param name="path">The file for which to obtain write date and time information.</param>
      /// <returns>
      ///   A <see cref="System.DateTime"/> structure set to the date and time that the specified file was last written to. This value is
      ///   expressed in UTC time.
      /// </returns>
      [SecurityCritical]
      public static DateTime GetLastWriteTimeUtc(string path)
      {
         return GetLastWriteTimeInternal(null, path, true, PathFormat.Auto);
      }

      #endregion // .NET

      #region AlphaFS

      #region IsFullPath

      /// <summary>
      ///   [AlphaFS] Gets the date and time, in coordinated universal time (UTC) time, that the specified file was last written to.
      /// </summary>
      /// <param name="path">The file for which to obtain write date and time information.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>
      ///   A <see cref="System.DateTime"/> structure set to the date and time that the specified file was last written to. This value is
      ///   expressed in UTC time.
      /// </returns>
      [SecurityCritical]
      public static DateTime GetLastWriteTimeUtc(string path, PathFormat pathFormat)
      {
         return GetLastWriteTimeInternal(null, path, true, pathFormat);
      }

      #endregion // IsFullPath

      #region Transacted

      #region .NET

      /// <summary>
      ///   [AlphaFS] Gets the date and time, in coordinated universal time (UTC) time, that the specified file was last written to.
      /// </summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file for which to obtain write date and time information.</param>
      /// <returns>
      ///   A <see cref="System.DateTime"/> structure set to the date and time that the specified file was last written to. This value is
      ///   expressed in UTC time.
      /// </returns>
      [SecurityCritical]
      public static DateTime GetLastWriteTimeUtc(KernelTransaction transaction, string path)
      {
         return GetLastWriteTimeInternal(transaction, path, true, PathFormat.Auto);
      }

      #endregion // .NET

      #region IsFullPath

      /// <summary>
      ///   [AlphaFS] Gets the date and time, in coordinated universal time (UTC) time, that the specified file was last written to.
      /// </summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file for which to obtain write date and time information.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>
      ///   A <see cref="System.DateTime"/> structure set to the date and time that the specified file was last written to. This value is
      ///   expressed in UTC time.
      /// </returns>
      [SecurityCritical]
      public static DateTime GetLastWriteTimeUtc(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         return GetLastWriteTimeInternal(transaction, path, true, pathFormat);
      }

      #endregion // IsFullPath

      #endregion // Transacted

      #endregion // AlphaFS

      #endregion // GetLastWriteTimeUtc

      #region Open

      #region .NET

      /// <summary>Opens a <see cref="FileStream"/> on the specified path with read/write access.</summary>
      /// <param name="path">The file to open.</param>
      /// <param name="mode">A <see cref="FileMode"/> value that specifies whether a file is created if one does not exist, and determines whether the contents of existing files are retained or overwritten.</param>
      /// <returns>A <see cref="FileStream"/> opened in the specified mode and path, with read/write access and not shared.</returns>
      [SecurityCritical]
      public static FileStream Open(string path, FileMode mode)
      {
         return OpenInternal(null, path, mode, 0, mode == FileMode.Append ? FileAccess.Write : FileAccess.ReadWrite, FileShare.None, ExtendedFileAttributes.Normal, PathFormat.Auto);
      }

      /// <summary>Opens a <see cref="FileStream"/> on the specified path, with the specified mode and access.</summary>
      /// <param name="path">The file to open.</param>
      /// <param name="mode">A <see cref="FileMode"/> value that specifies whether a file is created if one does not exist, and determines whether the contents of existing files are retained or overwritten.</param>
      /// <param name="access">A <see cref="FileAccess"/> value that specifies the operations that can be performed on the file.</param>
      /// <returns>An unshared <see cref="FileStream"/> that provides access to the specified file, with the specified mode and access.</returns>
      [SecurityCritical]
      public static FileStream Open(string path, FileMode mode, FileAccess access)
      {
         return OpenInternal(null, path, mode, 0, access, FileShare.None, ExtendedFileAttributes.Normal, PathFormat.Auto);
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
         return OpenInternal(null, path, mode, 0, access, share, ExtendedFileAttributes.Normal, PathFormat.Auto);
      }

      #endregion // .NET

      #region AlphaFS

      #region IsFullPath

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

      #endregion // IsFullPath

      #region Transacted

      #region .NET

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
         return OpenInternal(transaction, path, mode, 0, mode == FileMode.Append ? FileAccess.Write : FileAccess.ReadWrite, FileShare.None, ExtendedFileAttributes.Normal, PathFormat.Auto);
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
         return OpenInternal(transaction, path, mode, 0, access, FileShare.None, ExtendedFileAttributes.Normal, PathFormat.Auto);
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
         return OpenInternal(transaction, path, mode, 0, access, share, ExtendedFileAttributes.Normal, PathFormat.Auto);
      }

      #endregion // .NET

      #region IsFullPath

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

      #endregion // IsFullPath

      #endregion // Transacted

      #endregion // AlphaFS

      #endregion // Open

      #region OpenRead

      #region .NET

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
         return OpenInternal(null, path, FileMode.Open, 0, FileAccess.Read, FileShare.Read, ExtendedFileAttributes.Normal, PathFormat.Auto);
      }

      #endregion // .NET

      #region AlphaFS

      #region IsFullPath

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

      #endregion // IsFullPath

      #region Transacted

      #region .NET

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
         return OpenInternal(transaction, path, FileMode.Open, 0, FileAccess.Read, FileShare.Read, ExtendedFileAttributes.Normal, PathFormat.Auto);
      }

      #endregion //.NET

      #region IsFullPath

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

      #endregion // IsFullPath

      #endregion // Transacted

      #endregion // AlphaFS

      #endregion // OpenRead

      #region OpenText

      #region .NET

      /// <summary>Opens an existing NativeMethods.DefaultFileEncoding encoded text file for reading.</summary>
      /// <param name="path">The file to be opened for reading.</param>
      /// <returns>A <see cref="StreamReader"/> on the specified path.</returns>
      [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
      [SecurityCritical]
      public static StreamReader OpenText(string path)
      {
         return new StreamReader(OpenInternal(null, path, FileMode.Open, 0, FileAccess.Read, FileShare.Read, ExtendedFileAttributes.Normal, PathFormat.Auto), NativeMethods.DefaultFileEncoding);
      }

      #endregion // .NET

      #region AlphaFS

      #region IsFullPath

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

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Opens an existing <see cref="Encoding"/> encoded text file for reading.</summary>
      /// <param name="path">The file to be opened for reading.</param>
      /// <param name="encoding">The <see cref="Encoding"/> applied to the contents of the file.</param>
      /// <returns>A <see cref="StreamReader"/> on the specified path.</returns>
      [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
      [SecurityCritical]
      public static StreamReader OpenText(string path, Encoding encoding)
      {
         return new StreamReader(OpenInternal(null, path, FileMode.Open, 0, FileAccess.Read, FileShare.Read, ExtendedFileAttributes.Normal, PathFormat.Auto), encoding);
      }

      #region Transacted

      #region .NET

      /// <summary>[AlphaFS] Opens an existing NativeMethods.DefaultFileEncoding encoded text file for reading.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to be opened for reading.</param>
      /// <returns>A <see cref="StreamReader"/> on the specified path.</returns>
      [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
      public static StreamReader OpenText(KernelTransaction transaction, string path)
      {
         return new StreamReader(OpenInternal(transaction, path, FileMode.Open, 0, FileAccess.Read, FileShare.Read, ExtendedFileAttributes.Normal, PathFormat.Auto), NativeMethods.DefaultFileEncoding);
      }

      #endregion // .NET

      #region IsFullPath

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

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Opens an existing <see cref="Encoding"/> encoded text file for reading.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to be opened for reading.</param>
      /// <param name="encoding">The <see cref="Encoding"/> applied to the contents of the file.</param>
      /// <returns>A <see cref="StreamReader"/> on the specified path.</returns>
      [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
      public static StreamReader OpenText(KernelTransaction transaction, string path, Encoding encoding)
      {
         return new StreamReader(OpenInternal(transaction, path, FileMode.Open, 0, FileAccess.Read, FileShare.Read, ExtendedFileAttributes.Normal, PathFormat.Auto), encoding);
      }

      #endregion // Transacted

      #endregion // AlphaFS

      #endregion // OpenText

      #region OpenWrite

      #region .NET

      /// <summary>Opens an existing file or creates a new file for writing.</summary>
      /// <param name="path">The file to be opened for writing.</param>
      /// <returns>An unshared <see cref="FileStream"/> object on the specified path with <see cref="FileAccess.Write"/> access.</returns>
      [SecurityCritical]
      public static FileStream OpenWrite(string path)
      {
         return OpenInternal(null, path, FileMode.OpenOrCreate, 0, FileAccess.Write, FileShare.None, ExtendedFileAttributes.Normal, PathFormat.Auto);
      }

      #endregion // .NET

      #region AlphaFS

      #region IsFullPath

      /// <summary>[AlphaFS] Opens an existing file or creates a new file for writing.</summary>
      /// <param name="path">The file to be opened for writing.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>An unshared <see cref="FileStream"/> object on the specified path with <see cref="FileAccess.Write"/> access.</returns>
      [SecurityCritical]
      public static FileStream OpenWrite(string path, PathFormat pathFormat)
      {
         return OpenInternal(null, path, FileMode.OpenOrCreate, 0, FileAccess.Write, FileShare.None, ExtendedFileAttributes.Normal, pathFormat);
      }

      #endregion // IsFullPath

      #region Transacted

      #region .NET

      /// <summary>[AlphaFS] Opens an existing file or creates a new file for writing.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to be opened for writing.</param>
      /// <returns>An unshared <see cref="FileStream"/> object on the specified path with <see cref="FileAccess.Write"/> access.</returns>
      [SecurityCritical]
      public static FileStream OpenWrite(KernelTransaction transaction, string path)
      {
         return OpenInternal(transaction, path, FileMode.OpenOrCreate, 0, FileAccess.Write, FileShare.None, ExtendedFileAttributes.Normal, PathFormat.Auto);
      }

      #endregion // .NET

      #region IsFullPath

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

      #endregion // IsFullPath

      #endregion // Transacted

      #endregion // AlphaFS

      #endregion // OpenWrite

      #region ReadAllBytes

      #region .NET

      /// <summary>Opens a binary file, reads the contents of the file into a byte array, and then closes the file.</summary>
      /// <param name="path">The file to open for reading.</param>
      /// <returns>A byte array containing the contents of the file.</returns>
      [SecurityCritical]
      public static byte[] ReadAllBytes(string path)
      {
         return ReadAllBytesInternal(null, path, PathFormat.Auto);
      }

      #endregion // .NET

      #region AlphaFS

      #region IsFullPath

      /// <summary>[AlphaFS] Opens a binary file, reads the contents of the file into a byte array, and then closes the file.</summary>
      /// <param name="path">The file to open for reading.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>A byte array containing the contents of the file.</returns>
      [SecurityCritical]
      public static byte[] ReadAllBytes(string path, PathFormat pathFormat)
      {
         return ReadAllBytesInternal(null, path, pathFormat);
      }

      #endregion // IsFullPath

      #region Transacted

      #region .NET

      /// <summary>[AlphaFS] Opens a binary file, reads the contents of the file into a byte array, and then closes the file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to open for reading.</param>
      /// <returns>A byte array containing the contents of the file.</returns>
      [SecurityCritical]
      public static byte[] ReadAllBytes(KernelTransaction transaction, string path)
      {
         return ReadAllBytesInternal(transaction, path, PathFormat.Auto);
      }

      #endregion // .NET

      #region IsFullPath

      /// <summary>[AlphaFS] Opens a binary file, reads the contents of the file into a byte array, and then closes the file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to open for reading.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>A byte array containing the contents of the file.</returns>
      [SecurityCritical]
      public static byte[] ReadAllBytes(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         return ReadAllBytesInternal(transaction, path, pathFormat);
      }

      #endregion // IsFullPath

      #endregion // Transacted

      #endregion // AlphaFS

      #endregion // ReadAllBytes

      #region ReadAllLines

      #region .NET

      /// <summary>Opens a text file, reads all lines of the file, and then closes the file.</summary>
      /// <param name="path">The file to open for reading.</param>
      /// <returns>All lines of the file.</returns>
      [SecurityCritical]
      public static string[] ReadAllLines(string path)
      {
         return ReadAllLinesInternal(null, path, NativeMethods.DefaultFileEncoding, PathFormat.Auto).ToArray();
      }

      /// <summary>Opens a file, reads all lines of the file with the specified encoding, and then closes the file.</summary>
      /// <param name="path">The file to open for reading.</param>
      /// <param name="encoding">The <see cref="Encoding"/> applied to the contents of the file.</param>
      /// <returns>All lines of the file.</returns>
      [SecurityCritical]
      public static string[] ReadAllLines(string path, Encoding encoding)
      {
         return ReadAllLinesInternal(null, path, encoding, PathFormat.Auto).ToArray();
      }

      #endregion // .NET

      #region AlphaFS

      #region IsFullPath

      /// <summary>[AlphaFS] Opens a text file, reads all lines of the file, and then closes the file.</summary>
      /// <param name="path">The file to open for reading.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>All lines of the file.</returns>
      [SecurityCritical]
      public static string[] ReadAllLines(string path, PathFormat pathFormat)
      {
         return ReadAllLinesInternal(null, path, NativeMethods.DefaultFileEncoding, pathFormat).ToArray();
      }

      /// <summary>[AlphaFS] Opens a file, reads all lines of the file with the specified encoding, and then closes the file.</summary>
      /// <param name="path">The file to open for reading.</param>
      /// <param name="encoding">The <see cref="Encoding"/> applied to the contents of the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>All lines of the file.</returns>
      [SecurityCritical]
      public static string[] ReadAllLines(string path, Encoding encoding, PathFormat pathFormat)
      {
         return ReadAllLinesInternal(null, path, encoding, pathFormat).ToArray();
      }

      #endregion // IsFullPath

      #region Transacted

      #region .NET

      /// <summary>[AlphaFS] Opens a text file, reads all lines of the file, and then closes the file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to open for reading.</param>
      /// <returns>All lines of the file.</returns>
      [SecurityCritical]
      public static string[] ReadAllLines(KernelTransaction transaction, string path)
      {
         return ReadAllLinesInternal(transaction, path, NativeMethods.DefaultFileEncoding, PathFormat.Auto).ToArray();
      }

      /// <summary>[AlphaFS] Opens a file, reads all lines of the file with the specified encoding, and then closes the file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to open for reading.</param>
      /// <param name="encoding">The <see cref="Encoding"/> applied to the contents of the file.</param>
      /// <returns>All lines of the file.</returns>
      [SecurityCritical]
      public static string[] ReadAllLines(KernelTransaction transaction, string path, Encoding encoding)
      {
         return ReadAllLinesInternal(transaction, path, encoding, PathFormat.Auto).ToArray();
      }

      #endregion // .NET

      #region IsFullPath

      /// <summary>[AlphaFS] Opens a text file, reads all lines of the file, and then closes the file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to open for reading.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>All lines of the file.</returns>
      [SecurityCritical]
      public static string[] ReadAllLines(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         return ReadAllLinesInternal(transaction, path, NativeMethods.DefaultFileEncoding, pathFormat).ToArray();
      }

      /// <summary>[AlphaFS] Opens a file, reads all lines of the file with the specified encoding, and then closes the file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to open for reading.</param>
      /// <param name="encoding">The <see cref="Encoding"/> applied to the contents of the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>All lines of the file.</returns>
      [SecurityCritical]
      public static string[] ReadAllLines(KernelTransaction transaction, string path, Encoding encoding, PathFormat pathFormat)
      {
         return ReadAllLinesInternal(transaction, path, encoding, pathFormat).ToArray();
      }

      #endregion // IsFullPath

      #endregion // Transacted

      #endregion // AlphaFS

      #endregion // ReadAllLines

      #region ReadAllText

      #region .NET

      /// <summary>Opens a text file, reads all lines of the file, and then closes the file.</summary>
      /// <param name="path">The file to open for reading.</param>
      /// <returns>All lines of the file.</returns>
      [SecurityCritical]
      public static string ReadAllText(string path)
      {
         return ReadAllTextInternal(null, path, NativeMethods.DefaultFileEncoding, PathFormat.Auto);
      }

      /// <summary>Opens a file, reads all lines of the file with the specified encoding, and then closes the file.</summary>
      /// <param name="path">The file to open for reading.</param>
      /// <param name="encoding">The <see cref="Encoding"/> applied to the contents of the file.</param>
      /// <returns>All lines of the file.</returns>
      [SecurityCritical]
      public static string ReadAllText(string path, Encoding encoding)
      {
         return ReadAllTextInternal(null, path, encoding, PathFormat.Auto);
      }

      #endregion // .NET

      #region AlphaFS

      #region IsFullPath

      /// <summary>[AlphaFS] Opens a text file, reads all lines of the file, and then closes the file.</summary>
      /// <param name="path">The file to open for reading.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>All lines of the file.</returns>
      [SecurityCritical]
      public static string ReadAllText(string path, PathFormat pathFormat)
      {
         return ReadAllTextInternal(null, path, NativeMethods.DefaultFileEncoding, pathFormat);
      }

      /// <summary>[AlphaFS] Opens a file, reads all lines of the file with the specified encoding, and then closes the file.</summary>
      /// <param name="path">The file to open for reading.</param>
      /// <param name="encoding">The <see cref="Encoding"/> applied to the contents of the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>All lines of the file.</returns>
      [SecurityCritical]
      public static string ReadAllText(string path, Encoding encoding, PathFormat pathFormat)
      {
         return ReadAllTextInternal(null, path, encoding, pathFormat);
      }

      #endregion // IsFullPath

      #region Transacted

      #region .NET

      /// <summary>[AlphaFS] Opens a text file, reads all lines of the file, and then closes the file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to open for reading.</param>
      /// <returns>All lines of the file.</returns>
      [SecurityCritical]
      public static string ReadAllText(KernelTransaction transaction, string path)
      {
         return ReadAllTextInternal(transaction, path, NativeMethods.DefaultFileEncoding, PathFormat.Auto);
      }

      /// <summary>[AlphaFS] Opens a file, reads all lines of the file with the specified encoding, and then closes the file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to open for reading.</param>
      /// <param name="encoding">The <see cref="Encoding"/> applied to the contents of the file.</param>
      /// <returns>All lines of the file.</returns>
      [SecurityCritical]
      public static string ReadAllText(KernelTransaction transaction, string path, Encoding encoding)
      {
         return ReadAllTextInternal(transaction, path, encoding, PathFormat.Auto);
      }

      #endregion // .NET

      #region IsFullPath

      /// <summary>[AlphaFS] Opens a text file, reads all lines of the file, and then closes the file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to open for reading.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>All lines of the file.</returns>
      [SecurityCritical]
      public static string ReadAllText(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         return ReadAllTextInternal(transaction, path, NativeMethods.DefaultFileEncoding, pathFormat);
      }

      /// <summary>[AlphaFS] Opens a file, reads all lines of the file with the specified encoding, and then closes the file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to open for reading.</param>
      /// <param name="encoding">The <see cref="Encoding"/> applied to the contents of the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>All lines of the file.</returns>
      [SecurityCritical]
      public static string ReadAllText(KernelTransaction transaction, string path, Encoding encoding, PathFormat pathFormat)
      {
         return ReadAllTextInternal(transaction, path, encoding, pathFormat);
      }

      #endregion // IsFullPath

      #endregion // Transacted

      #endregion // AlphaFS

      #endregion // ReadAllText

      #region ReadLines

      #region .NET

      /// <summary>Reads the lines of a file.</summary>
      /// <param name="path">The file to read.</param>
      /// <returns>All the lines of the file, or the lines that are the result of a query.</returns>
      [SecurityCritical]
      public static IEnumerable<string> ReadLines(string path)
      {
         return ReadLinesInternal(null, path, NativeMethods.DefaultFileEncoding, PathFormat.Auto);
      }

      /// <summary>Read the lines of a file that has a specified encoding.</summary>
      /// <param name="path">The file to read.</param>
      /// <param name="encoding">The encoding that is applied to the contents of the file.</param>
      /// <returns>All the lines of the file, or the lines that are the result of a query.</returns>
      [SecurityCritical]
      public static IEnumerable<string> ReadLines(string path, Encoding encoding)
      {
         return ReadLinesInternal(null, path, encoding, PathFormat.Auto);
      }

      #endregion // .NET

      #region AlphaFS

      #region IsFullPath

      /// <summary>[AlphaFS] Reads the lines of a file.</summary>
      /// <param name="path">The file to read.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>All the lines of the file, or the lines that are the result of a query.</returns>
      [SecurityCritical]
      public static IEnumerable<string> ReadLines(string path, PathFormat pathFormat)
      {
         return ReadLinesInternal(null, path, NativeMethods.DefaultFileEncoding, pathFormat);
      }

      /// <summary>[AlphaFS] Read the lines of a file that has a specified encoding.</summary>
      /// <param name="path">The file to read.</param>
      /// <param name="encoding">The encoding that is applied to the contents of the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>All the lines of the file, or the lines that are the result of a query.</returns>
      [SecurityCritical]
      public static IEnumerable<string> ReadLines(string path, Encoding encoding, PathFormat pathFormat)
      {
         return ReadLinesInternal(null, path, encoding, pathFormat);
      }

      #endregion // IsFullPath

      #region Transacted

      #region .NET

      /// <summary>[AlphaFS] Reads the lines of a file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to read.</param>
      /// <returns>All the lines of the file, or the lines that are the result of a query.</returns>
      [SecurityCritical]
      public static IEnumerable<string> ReadLines(KernelTransaction transaction, string path)
      {
         return ReadLinesInternal(transaction, path, NativeMethods.DefaultFileEncoding, PathFormat.Auto);
      }

      /// <summary>[AlphaFS] Read the lines of a file that has a specified encoding.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to read.</param>
      /// <param name="encoding">The encoding that is applied to the contents of the file.</param>
      /// <returns>All the lines of the file, or the lines that are the result of a query.</returns>
      [SecurityCritical]
      public static IEnumerable<string> ReadLines(KernelTransaction transaction, string path, Encoding encoding)
      {
         return ReadLinesInternal(transaction, path, encoding, PathFormat.Auto);
      }

      #endregion // .NET

      #region IsFullPath

      /// <summary>[AlphaFS] Reads the lines of a file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to read.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>All the lines of the file, or the lines that are the result of a query.</returns>
      [SecurityCritical]
      public static IEnumerable<string> ReadLines(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         return ReadLinesInternal(transaction, path, NativeMethods.DefaultFileEncoding, pathFormat);
      }

      /// <summary>[AlphaFS] Read the lines of a file that has a specified encoding.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to read.</param>
      /// <param name="encoding">The encoding that is applied to the contents of the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>All the lines of the file, or the lines that are the result of a query.</returns>
      [SecurityCritical]
      public static IEnumerable<string> ReadLines(KernelTransaction transaction, string path, Encoding encoding, PathFormat pathFormat)
      {
         return ReadLinesInternal(transaction, path, encoding, pathFormat);
      }

      #endregion // IsFullPath

      #endregion // Transacted

      #endregion // AlphaFS

      #endregion // ReadLines

      #region Replace

      #region .NET

      /// <summary>
      ///   Replaces the contents of a specified file with the contents of another file, deleting the original file, and creating a backup of
      ///   the replaced file.
      /// </summary>
      /// <remarks>
      ///   The Replace method replaces the contents of a specified file with the contents of another file. It also creates a backup of the
      ///   file that was replaced.
      /// </remarks>
      /// <remarks>
      ///   If the <paramref name="sourceFileName"/> and <paramref name="destinationFileName"/> are on different volumes, this method will
      ///   raise an exception. If the <paramref name="destinationBackupFileName"/> is on a different volume from the source file, the backup
      ///   file will be deleted.
      /// </remarks>
      /// <remarks>
      ///   Pass null to the <paramref name="destinationBackupFileName"/> parameter if you do not want to create a backup of the file being
      ///   replaced.
      /// </remarks>
      /// <param name="sourceFileName">The name of a file that replaces the file specified by <paramref name="destinationFileName"/>.</param>
      /// <param name="destinationFileName">The name of the file being replaced.</param>
      /// <param name="destinationBackupFileName">The name of the backup file.</param>      
      [SecurityCritical]
      public static void Replace(string sourceFileName, string destinationFileName, string destinationBackupFileName)
      {
         ReplaceInternal(sourceFileName, destinationFileName, destinationBackupFileName, false, PathFormat.Auto);
      }

      /// <summary>
      ///   Replaces the contents of a specified file with the contents of another file, deleting the original file, and creating a backup of
      ///   the replaced file and optionally ignores merge errors.
      /// </summary>
      /// <remarks>
      ///   The Replace method replaces the contents of a specified file with the contents of another file. It also creates a backup of the
      ///   file that was replaced.
      /// </remarks>
      /// <remarks>
      ///   If the <paramref name="sourceFileName"/> and <paramref name="destinationFileName"/> are on different volumes, this method will
      ///   raise an exception. If the <paramref name="destinationBackupFileName"/> is on a different volume from the source file, the backup
      ///   file will be deleted.
      /// </remarks>
      /// <remarks>
      ///   Pass null to the <paramref name="destinationBackupFileName"/> parameter if you do not want to create a backup of the file being
      ///   replaced.
      /// </remarks>
      /// <param name="sourceFileName">The name of a file that replaces the file specified by <paramref name="destinationFileName"/>.</param>
      /// <param name="destinationFileName">The name of the file being replaced.</param>
      /// <param name="destinationBackupFileName">The name of the backup file.</param>
      /// <param name="ignoreMetadataErrors">
      ///   <see langword="true"/> to ignore merge errors (such as attributes and access control lists (ACLs)) from the replaced file to the
      ///   replacement file; otherwise, <see langword="false"/>.
      /// </param>      
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "dest")]
      [SecurityCritical]
      public static void Replace(string sourceFileName, string destinationFileName, string destinationBackupFileName, bool ignoreMetadataErrors)
      {
         ReplaceInternal(sourceFileName, destinationFileName, destinationBackupFileName, ignoreMetadataErrors, PathFormat.Auto);
      }

      #endregion // .NET

      #region AlphaFS

      #region IsFullPath

      /// <summary>
      ///   [AlphaFS] Replaces the contents of a specified file with the contents of another file, deleting the original file, and creating a
      ///   backup of the replaced file and optionally ignores merge errors.
      /// </summary>
      /// <remarks>
      ///   The Replace method replaces the contents of a specified file with the contents of another file. It also creates a backup of the
      ///   file that was replaced.
      /// </remarks>
      /// <remarks>
      ///   If the <paramref name="sourceFileName"/> and <paramref name="destinationFileName"/> are on different volumes, this method will
      ///   raise an exception. If the <paramref name="destinationBackupFileName"/> is on a different volume from the source file, the backup
      ///   file will be deleted.
      /// </remarks>
      /// <remarks>
      ///   Pass null to the <paramref name="destinationBackupFileName"/> parameter if you do not want to create a backup of the file being
      ///   replaced.
      /// </remarks>
      /// <param name="sourceFileName">The name of a file that replaces the file specified by <paramref name="destinationFileName"/>.</param>
      /// <param name="destinationFileName">The name of the file being replaced.</param>
      /// <param name="destinationBackupFileName">The name of the backup file.</param>
      /// <param name="ignoreMetadataErrors">
      ///   <see langword="true"/> to ignore merge errors (such as attributes and access control lists (ACLs)) from the replaced file to the
      ///   replacement file; otherwise, <see langword="false"/>.
      /// </param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "dest")]
      [SecurityCritical]
      public static void Replace(string sourceFileName, string destinationFileName, string destinationBackupFileName, bool ignoreMetadataErrors, PathFormat pathFormat)
      {
         ReplaceInternal(sourceFileName, destinationFileName, destinationBackupFileName, ignoreMetadataErrors, pathFormat);
      }

      #endregion // IsFullPath

      #endregion // AlphaFS

      #endregion // Replace

      #region SetAccessControl

      #region .NET

      /// <summary>
      ///   Applies access control list (ACL) entries described by a <see cref="FileSecurity"/> FileSecurity object to the specified file.
      /// </summary>
      /// <param name="path">A file to add or remove access control list (ACL) entries from.</param>
      /// <param name="fileSecurity">
      ///   A  <see cref="FileSecurity"/> object that describes an ACL entry to apply to the file described by the <paramref name="path"/>
      ///   parameter.
      /// </param>      
      [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
      [SecurityCritical]
      public static void SetAccessControl(string path, FileSecurity fileSecurity)
      {
         SetAccessControlInternal(path, null, fileSecurity, AccessControlSections.All, PathFormat.Auto);
      }

      /// <summary>
      ///   Applies access control list (ACL) entries described by a <see cref="DirectorySecurity"/> object to the specified directory.
      /// </summary>
      /// <param name="path">A directory to add or remove access control list (ACL) entries from.</param>
      /// <param name="fileSecurity">
      ///   A <see cref="FileSecurity "/> object that describes an ACL entry to apply to the directory described by the path parameter.
      /// </param>
      /// <param name="includeSections">
      ///   One or more of the <see cref="AccessControlSections"/> values that specifies the type of access control list (ACL) information to
      ///   set.
      /// </param>      
      [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
      [SecurityCritical]
      public static void SetAccessControl(string path, FileSecurity fileSecurity, AccessControlSections includeSections)
      {
         SetAccessControlInternal(path, null, fileSecurity, includeSections, PathFormat.Auto);
      }

      #endregion // .NET

      #region AlphaFS

      #region IsFullPath

      /// <summary>
      ///   [AlphaFS] Applies access control list (ACL) entries described by a <see cref="FileSecurity"/> FileSecurity object to the specified
      ///   file.
      /// </summary>
      /// <param name="path">A file to add or remove access control list (ACL) entries from.</param>
      /// <param name="fileSecurity">
      ///   A  <see cref="FileSecurity"/> object that describes an ACL entry to apply to the file described by the <paramref name="path"/>
      ///   parameter.
      /// </param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
      [SecurityCritical]
      public static void SetAccessControl(string path, FileSecurity fileSecurity, PathFormat pathFormat)
      {
         SetAccessControlInternal(path, null, fileSecurity, AccessControlSections.All, pathFormat);
      }

      /// <summary>
      ///   [AlphaFS] Applies access control list (ACL) entries described by a <see cref="DirectorySecurity"/> object to the specified
      ///   directory.
      /// </summary>
      /// <param name="path">A directory to add or remove access control list (ACL) entries from.</param>
      /// <param name="fileSecurity">
      ///   A <see cref="FileSecurity "/> object that describes an ACL entry to apply to the directory described by the path parameter.
      /// </param>
      /// <param name="includeSections">
      ///   One or more of the <see cref="AccessControlSections"/> values that specifies the type of access control list (ACL) information to
      ///   set.
      /// </param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
      [SecurityCritical]
      public static void SetAccessControl(string path, FileSecurity fileSecurity, AccessControlSections includeSections, PathFormat pathFormat)
      {
         SetAccessControlInternal(path, null, fileSecurity, includeSections, pathFormat);
      }

      #endregion // IsFullPath

      #endregion // AlphaFS

      #endregion // SetAccessControl

      #region SetAttributes

      #region .NET

      /// <summary>Sets the specified <see cref="FileAttributes"/> of the file or directory on the specified path.</summary>
      /// <remarks>
      ///   Certain file attributes, such as <see cref="FileAttributes.Hidden"/> and <see cref="FileAttributes.ReadOnly"/>, can be combined.
      ///   Other attributes, such as <see cref="FileAttributes.Normal"/>, must be used alone.
      /// </remarks>
      /// <remarks>
      ///   It is not possible to change the <see cref="FileAttributes.Compressed"/> status of a File object using this method.
      /// </remarks>
      /// <param name="path">The path to the file or directory.</param>
      /// <param name="fileAttributes">A bitwise combination of the enumeration values.</param>
      /// <overloads>Sets the specified <see cref="FileAttributes"/> of the file or directory on the specified path.</overloads>
      [SecurityCritical]
      public static void SetAttributes(string path, FileAttributes fileAttributes)
      {
         SetAttributesInternal(false, null, path, fileAttributes, false, PathFormat.Auto);
      }

      #endregion // .NET

      #region AlphaFS

      #region IsFullPath

      /// <summary>[AlphaFS] Sets the specified <see cref="FileAttributes"/> of the file or directory on the specified path.</summary>
      /// <remarks>
      ///   Certain file attributes, such as <see cref="FileAttributes.Hidden"/> and <see cref="FileAttributes.ReadOnly"/>, can be combined.
      ///   Other attributes, such as <see cref="FileAttributes.Normal"/>, must be used alone.
      /// </remarks>
      /// <remarks>
      ///   It is not possible to change the <see cref="FileAttributes.Compressed"/> status of a File object using this method.
      /// </remarks>
      /// <param name="path">The path to the file or directory.</param>
      /// <param name="fileAttributes">A bitwise combination of the enumeration values.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SecurityCritical]
      public static void SetAttributes(string path, FileAttributes fileAttributes, PathFormat pathFormat)
      {
         SetAttributesInternal(false, null, path, fileAttributes, false, pathFormat);
      }

      #endregion // IsFullPath

      #region Transacted

      #region .NET

      /// <summary>[AlphaFS] Sets the specified <see cref="FileAttributes"/> of the file on the specified path.</summary>
      /// <remarks>
      ///   Certain file attributes, such as <see cref="FileAttributes.Hidden"/> and <see cref="FileAttributes.ReadOnly"/>, can be combined.
      ///   Other attributes, such as <see cref="FileAttributes.Normal"/>, must be used alone.
      /// </remarks>
      /// <remarks>
      ///   It is not possible to change the <see cref="FileAttributes.Compressed"/> status of a File object using this method.
      /// </remarks>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to the file.</param>
      /// <param name="fileAttributes">A bitwise combination of the enumeration values.</param>      
      [SecurityCritical]
      public static void SetAttributes(KernelTransaction transaction, string path, FileAttributes fileAttributes)
      {
         SetAttributesInternal(false, transaction, path, fileAttributes, false, PathFormat.Auto);
      }

      #endregion // .NET

      #region IsFullPath

      /// <summary>[AlphaFS] Sets the specified <see cref="FileAttributes"/> of the file on the specified path.</summary>
      /// <remarks>
      ///   Certain file attributes, such as <see cref="FileAttributes.Hidden"/> and <see cref="FileAttributes.ReadOnly"/>, can be combined.
      ///   Other attributes, such as <see cref="FileAttributes.Normal"/>, must be used alone.
      /// </remarks>
      /// <remarks>
      ///   It is not possible to change the <see cref="FileAttributes.Compressed"/> status of a File object using this method.
      /// </remarks>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to the file.</param>
      /// <param name="fileAttributes">A bitwise combination of the enumeration values.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SecurityCritical]
      public static void SetAttributes(KernelTransaction transaction, string path, FileAttributes fileAttributes, PathFormat pathFormat)
      {
         SetAttributesInternal(false, transaction, path, fileAttributes, false, pathFormat);
      }

      #endregion // IsFullPath

      #endregion // Transacted

      #endregion // AlphaFS

      #endregion // SetAttributes

      #region SetCreationTime

      #region .NET

      /// <summary>Sets the date and time the file was created.</summary>
      /// <param name="path">The file for which to set the creation date and time information.</param>
      /// <param name="creationTime">
      ///   A <see cref="System.DateTime"/> containing the value to set for the creation date and time of <paramref name="path"/>. This value
      ///   is expressed in local time.
      /// </param>      
      [SecurityCritical]
      public static void SetCreationTime(string path, DateTime creationTime)
      {
         SetFsoDateTimeInternal(false, null, path, creationTime.ToUniversalTime(), null, null, PathFormat.Auto);
      }

      #endregion // .NET

      #region AlphaFS

      #region IsFullPath

      /// <summary>[AlphaFS] Sets the date and time the file was created.</summary>
      /// <param name="path">The file for which to set the creation date and time information.</param>
      /// <param name="creationTime">
      ///   A <see cref="System.DateTime"/> containing the value to set for the creation date and time of <paramref name="path"/>. This value
      ///   is expressed in local time.
      /// </param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SecurityCritical]
      public static void SetCreationTime(string path, DateTime creationTime, PathFormat pathFormat)
      {
         SetFsoDateTimeInternal(false, null, path, creationTime.ToUniversalTime(), null, null, pathFormat);
      }

      #endregion // IsFullPath

      #region Transacted

      #region .NET

      /// <summary>[AlphaFS] Sets the date and time the file was created.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file for which to set the creation date and time information.</param>
      /// <param name="creationTime">
      ///   A <see cref="System.DateTime"/> containing the value to set for the creation date and time of <paramref name="path"/>. This value
      ///   is expressed in local time.
      /// </param>      
      [SecurityCritical]
      public static void SetCreationTime(KernelTransaction transaction, string path, DateTime creationTime)
      {
         SetFsoDateTimeInternal(false, transaction, path, creationTime.ToUniversalTime(), null, null, PathFormat.Auto);
      }

      #endregion // .NET

      #region IsFullPath

      /// <summary>[AlphaFS] Sets the date and time the file was created.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file for which to set the creation date and time information.</param>
      /// <param name="creationTime">
      ///   A <see cref="System.DateTime"/> containing the value to set for the creation date and time of <paramref name="path"/>. This value
      ///   is expressed in local time.
      /// </param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SecurityCritical]
      public static void SetCreationTime(KernelTransaction transaction, string path, DateTime creationTime, PathFormat pathFormat)
      {
         SetFsoDateTimeInternal(false, transaction, path, creationTime.ToUniversalTime(), null, null, pathFormat);
      }

      #endregion // IsFullPath

      #endregion // Transacted

      #endregion // AlphaFS

      #endregion // SetCreationTime

      #region SetCreationTimeUtc

      #region .NET

      /// <summary>Sets the date and time, in coordinated universal time (UTC), that the file was created.</summary>
      /// <param name="path">The file for which to set the creation date and time information.</param>
      /// <param name="creationTimeUtc">
      ///   A <see cref="System.DateTime"/> containing the value to set for the creation date and time of <paramref name="path"/>. This value
      ///   is expressed in UTC time.
      /// </param>      
      [SecurityCritical]
      public static void SetCreationTimeUtc(string path, DateTime creationTimeUtc)
      {
         SetFsoDateTimeInternal(false, null, path, creationTimeUtc, null, null, PathFormat.Auto);
      }

      #endregion // .NET

      #region AlphaFS

      #region IsFullPath

      /// <summary>[AlphaFS] Sets the date and time, in coordinated universal time (UTC), that the file was created.</summary>
      /// <param name="path">The file for which to set the creation date and time information.</param>
      /// <param name="creationTimeUtc">
      ///   A <see cref="System.DateTime"/> containing the value to set for the creation date and time of <paramref name="path"/>. This value
      ///   is expressed in UTC time.
      /// </param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SecurityCritical]
      public static void SetCreationTimeUtc(string path, DateTime creationTimeUtc, PathFormat pathFormat)
      {
         SetFsoDateTimeInternal(false, null, path, creationTimeUtc, null, null, pathFormat);
      }

      #endregion // IsFullPath

      #region Transacted

      #region .NET

      /// <summary>[AlphaFS] Sets the date and time, in coordinated universal time (UTC), that the file was created.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file for which to set the creation date and time information.</param>
      /// <param name="creationTimeUtc">
      ///   A <see cref="System.DateTime"/> containing the value to set for the creation date and time of <paramref name="path"/>. This value
      ///   is expressed in UTC time.
      /// </param>      
      [SecurityCritical]
      public static void SetCreationTimeUtc(KernelTransaction transaction, string path, DateTime creationTimeUtc)
      {
         SetFsoDateTimeInternal(false, transaction, path, creationTimeUtc, null, null, PathFormat.Auto);
      }

      #endregion // .NET

      #region IsFullPath

      /// <summary>[AlphaFS] Sets the date and time, in coordinated universal time (UTC), that the file was created.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file for which to set the creation date and time information.</param>
      /// <param name="creationTimeUtc">
      ///   A <see cref="System.DateTime"/> containing the value to set for the creation date and time of <paramref name="path"/>. This value
      ///   is expressed in UTC time.
      /// </param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SecurityCritical]
      public static void SetCreationTimeUtc(KernelTransaction transaction, string path, DateTime creationTimeUtc, PathFormat pathFormat)
      {
         SetFsoDateTimeInternal(false, transaction, path, creationTimeUtc, null, null, pathFormat);
      }

      #endregion // IsFullPath

      #endregion // Transacted

      #endregion // AlphaFS

      #endregion // SetCreationTimeUtc

      #region SetLastAccessTime

      #region .NET

      /// <summary>Sets the date and time that the specified file was last accessed.</summary>
      /// <param name="path">The file for which to set the access date and time information.</param>
      /// <param name="lastAccessTime">
      ///   A <see cref="System.DateTime"/> containing the value to set for the last access date and time of <paramref name="path"/>. This
      ///   value is expressed in local time.
      /// </param>      
      [SecurityCritical]
      public static void SetLastAccessTime(string path, DateTime lastAccessTime)
      {
         SetFsoDateTimeInternal(false, null, path, null, lastAccessTime.ToUniversalTime(), null, PathFormat.Auto);
      }

      #endregion // .NET

      #region AlphaFS

      #region IsFullPath

      /// <summary>[AlphaFS] Sets the date and time that the specified file was last accessed.</summary>
      /// <param name="path">The file for which to set the access date and time information.</param>
      /// <param name="lastAccessTime">
      ///   A <see cref="System.DateTime"/> containing the value to set for the last access date and time of <paramref name="path"/>. This
      ///   value is expressed in local time.
      /// </param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SecurityCritical]
      public static void SetLastAccessTime(string path, DateTime lastAccessTime, PathFormat pathFormat)
      {
         SetFsoDateTimeInternal(false, null, path, null, lastAccessTime.ToUniversalTime(), null, pathFormat);
      }

      #endregion // IsFullPath

      #region Transaction

      #region .NET

      /// <summary>[AlphaFS] Sets the date and time that the specified file was last accessed.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file for which to set the access date and time information.</param>
      /// <param name="lastAccessTime">
      ///   A <see cref="System.DateTime"/> containing the value to set for the last access date and time of <paramref name="path"/>. This
      ///   value is expressed in local time.
      /// </param>      
      [SecurityCritical]
      public static void SetLastAccessTime(KernelTransaction transaction, string path, DateTime lastAccessTime)
      {
         SetFsoDateTimeInternal(false, transaction, path, null, lastAccessTime.ToUniversalTime(), null, PathFormat.Auto);
      }

      #endregion // .NET

      #region IsFullPath

      /// <summary>[AlphaFS] Sets the date and time that the specified file was last accessed.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file for which to set the access date and time information.</param>
      /// <param name="lastAccessTime">
      ///   A <see cref="System.DateTime"/> containing the value to set for the last access date and time of <paramref name="path"/>. This
      ///   value is expressed in local time.
      /// </param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SecurityCritical]
      public static void SetLastAccessTime(KernelTransaction transaction, string path, DateTime lastAccessTime, PathFormat pathFormat)
      {
         SetFsoDateTimeInternal(false, transaction, path, null, lastAccessTime.ToUniversalTime(), null, pathFormat);
      }

      #endregion // IsFullPath

      #endregion // Transaction

      #endregion // AlphaFS

      #endregion // SetLastAccessTime

      #region SetLastAccessTimeUtc

      #region .NET

      /// <summary>Sets the date and time, in coordinated universal time (UTC), that the specified file was last accessed.</summary>
      /// <param name="path">The file for which to set the access date and time information.</param>
      /// <param name="lastAccessTimeUtc">
      ///   A <see cref="System.DateTime"/> containing the value to set for the last access date and time of <paramref name="path"/>. This
      ///   value is expressed in UTC time.
      /// </param>      
      [SecurityCritical]
      public static void SetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc)
      {
         SetFsoDateTimeInternal(false, null, path, null, lastAccessTimeUtc, null, PathFormat.Auto);
      }

      #endregion // .NET

      #region AlphaFS

      #region IsFullPath

      /// <summary>[AlphaFS] Sets the date and time, in coordinated universal time (UTC), that the specified file was last accessed.</summary>
      /// <param name="path">The file for which to set the access date and time information.</param>
      /// <param name="lastAccessTimeUtc">
      ///   A <see cref="System.DateTime"/> containing the value to set for the last access date and time of <paramref name="path"/>. This
      ///   value is expressed in UTC time.
      /// </param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SecurityCritical]
      public static void SetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc, PathFormat pathFormat)
      {
         SetFsoDateTimeInternal(false, null, path, null, lastAccessTimeUtc, null, pathFormat);
      }

      #endregion // IsFullPath

      #region Transacted

      #region .NET

      /// <summary>[AlphaFS] Sets the date and time, in coordinated universal time (UTC), that the specified file was last accessed.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file for which to set the access date and time information.</param>
      /// <param name="lastAccessTimeUtc">
      ///   A <see cref="System.DateTime"/> containing the value to set for the last access date and time of <paramref name="path"/>. This
      ///   value is expressed in UTC time.
      /// </param>      
      [SecurityCritical]
      public static void SetLastAccessTimeUtc(KernelTransaction transaction, string path, DateTime lastAccessTimeUtc)
      {
         SetFsoDateTimeInternal(false, transaction, path, null, lastAccessTimeUtc, null, PathFormat.Auto);
      }

      #endregion // .NET

      #region IsFullPath

      /// <summary>[AlphaFS] Sets the date and time, in coordinated universal time (UTC), that the specified file was last accessed.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file for which to set the access date and time information.</param>
      /// <param name="lastAccessTimeUtc">
      ///   A <see cref="System.DateTime"/> containing the value to set for the last access date and time of <paramref name="path"/>. This
      ///   value is expressed in UTC time.
      /// </param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SecurityCritical]
      public static void SetLastAccessTimeUtc(KernelTransaction transaction, string path, DateTime lastAccessTimeUtc, PathFormat pathFormat)
      {
         SetFsoDateTimeInternal(false, transaction, path, null, lastAccessTimeUtc, null, pathFormat);
      }

      #endregion // IsFullPath

      #endregion // Transacted

      #endregion // AlphaFS

      #endregion // SetLastAccessTimeUtc

      #region SetLastWriteTime

      #region .NET

      /// <summary>Sets the date and time that the specified file was last written to.</summary>
      /// <param name="path">The file for which to set the date and time information.</param>
      /// <param name="lastWriteTime">
      ///   A <see cref="System.DateTime"/> containing the value to set for the last write date and time of <paramref name="path"/>. This value
      ///   is expressed in local time.
      /// </param>      
      [SecurityCritical]
      public static void SetLastWriteTime(string path, DateTime lastWriteTime)
      {
         SetFsoDateTimeInternal(false, null, path, null, null, lastWriteTime.ToUniversalTime(), PathFormat.Auto);
      }

      #endregion // .NET

      #region AlphaFS

      #region IsFullPath

      /// <summary>[AlphaFS] Sets the date and time that the specified file was last written to.</summary>
      /// <param name="path">The file for which to set the date and time information.</param>
      /// <param name="lastWriteTime">
      ///   A <see cref="System.DateTime"/> containing the value to set for the last write date and time of <paramref name="path"/>. This value
      ///   is expressed in local time.
      /// </param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SecurityCritical]
      public static void SetLastWriteTime(string path, DateTime lastWriteTime, PathFormat pathFormat)
      {
         SetFsoDateTimeInternal(false, null, path, null, null, lastWriteTime.ToUniversalTime(), pathFormat);
      }

      #endregion // IsFullPath

      #region Transacted

      #region .NET

      /// <summary>[AlphaFS] Sets the date and time that the specified file was last written to.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file for which to set the date and time information.</param>
      /// <param name="lastWriteTime">
      ///   A <see cref="System.DateTime"/> containing the value to set for the last write date and time of <paramref name="path"/>. This value
      ///   is expressed in local time.
      /// </param>      
      [SecurityCritical]
      public static void SetLastWriteTime(KernelTransaction transaction, string path, DateTime lastWriteTime)
      {
         SetFsoDateTimeInternal(false, transaction, path, null, null, lastWriteTime.ToUniversalTime(), PathFormat.Auto);
      }

      #endregion // .NET

      #region IsFullPath

      /// <summary>[AlphaFS] Sets the date and time that the specified file was last written to.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file for which to set the date and time information.</param>
      /// <param name="lastWriteTime">
      ///   A <see cref="System.DateTime"/> containing the value to set for the last write date and time of <paramref name="path"/>. This value
      ///   is expressed in local time.
      /// </param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SecurityCritical]
      public static void SetLastWriteTime(KernelTransaction transaction, string path, DateTime lastWriteTime, PathFormat pathFormat)
      {
         SetFsoDateTimeInternal(false, transaction, path, null, null, lastWriteTime.ToUniversalTime(), pathFormat);
      }

      #endregion // IsFullPath

      #endregion // Transacted

      #endregion // AlphaFS

      #endregion // SetLastWriteTime

      #region SetLastWriteTimeUtc

      #region .NET

      /// <summary>Sets the date and time, in coordinated universal time (UTC), that the specified file was last written to.</summary>
      /// <param name="path">The file for which to set the date and time information.</param>
      /// <param name="lastWriteTimeUtc">
      ///   A <see cref="System.DateTime"/> containing the value to set for the last write date and time of <paramref name="path"/>. This value
      ///   is expressed in UTC time.
      /// </param>      
      [SecurityCritical]
      public static void SetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc)
      {
         SetFsoDateTimeInternal(false, null, path, null, null, lastWriteTimeUtc, PathFormat.Auto);
      }

      #endregion // .NET

      #region AlphaFS

      #region IsFullPath

      /// <summary>
      ///   [AlphaFS] Sets the date and time, in coordinated universal time (UTC), that the specified file was last written to.
      /// </summary>
      /// <param name="path">The file for which to set the date and time information.</param>
      /// <param name="lastWriteTimeUtc">
      ///   A <see cref="System.DateTime"/> containing the value to set for the last write date and time of <paramref name="path"/>. This value
      ///   is expressed in UTC time.
      /// </param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SecurityCritical]
      public static void SetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc, PathFormat pathFormat)
      {
         SetFsoDateTimeInternal(false, null, path, null, null, lastWriteTimeUtc, pathFormat);
      }

      #endregion // IsFullPath

      #region Transacted

      #region .NET

      /// <summary>
      ///   [AlphaFS] Sets the date and time, in coordinated universal time (UTC), that the specified file was last written to.
      /// </summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file for which to set the date and time information.</param>
      /// <param name="lastWriteTimeUtc">
      ///   A <see cref="System.DateTime"/> containing the value to set for the last write date and time of <paramref name="path"/>. This value
      ///   is expressed in UTC time.
      /// </param>      
      [SecurityCritical]
      public static void SetLastWriteTimeUtc(KernelTransaction transaction, string path, DateTime lastWriteTimeUtc)
      {
         SetFsoDateTimeInternal(false, transaction, path, null, null, lastWriteTimeUtc, PathFormat.Auto);
      }

      #endregion // .NET

      #region IsFullPath

      /// <summary>
      ///   [AlphaFS] Sets the date and time, in coordinated universal time (UTC), that the specified file was last written to.
      /// </summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file for which to set the date and time information.</param>
      /// <param name="lastWriteTimeUtc">
      ///   A <see cref="System.DateTime"/> containing the value to set for the last write date and time of <paramref name="path"/>. This value
      ///   is expressed in UTC time.
      /// </param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SecurityCritical]
      public static void SetLastWriteTimeUtc(KernelTransaction transaction, string path, DateTime lastWriteTimeUtc, PathFormat pathFormat)
      {
         SetFsoDateTimeInternal(false, transaction, path, null, null, lastWriteTimeUtc, pathFormat);
      }

      #endregion // IsFullPath

      #endregion // Transacted

      #endregion // AlphaFS

      #endregion // SetLastWriteTimeUtc

      #region WriteAllBytes

      #region .NET

      /// <summary>
      ///   Creates a new file, writes the specified byte array to the file, and then closes the file. If the target file already exists, it is
      ///   overwritten.
      /// </summary>
      /// <param name="path">The file to write to.</param>
      /// <param name="bytes">The bytes to write to the file.</param>
      [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "bytes")]
      [SecurityCritical]
      public static void WriteAllBytes(string path, byte[] bytes)
      {
         WriteAllBytesInternal(null, path, bytes, PathFormat.Auto);
      }

      #endregion // .NET

      #region AlphaFS

      #region IsFullPath

      /// <summary>
      ///   [AlphaFS] Creates a new file, writes the specified byte array to the file, and then closes the file. If the target file already
      ///   exists, it is overwritten.
      /// </summary>
      /// <param name="path">The file to write to.</param>
      /// <param name="bytes">The bytes to write to the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "bytes")]
      [SecurityCritical]
      public static void WriteAllBytes(string path, byte[] bytes, PathFormat pathFormat)
      {
         WriteAllBytesInternal(null, path, bytes, pathFormat);
      }

      #endregion // IsFullPath

      #region Transacted

      #region .NET

      /// <summary>
      ///   [AlphaFS] Creates a new file, writes the specified byte array to the file, and then closes the file. If the target file already
      ///   exists, it is overwritten.
      /// </summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to write to.</param>
      /// <param name="bytes">The bytes to write to the file.</param>
      [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "bytes")]
      [SecurityCritical]
      public static void WriteAllBytes(KernelTransaction transaction, string path, byte[] bytes)
      {
         WriteAllBytesInternal(transaction, path, bytes, PathFormat.Auto);
      }

      #endregion // .NET

      #region IsFullPath

      /// <summary>
      ///   [AlphaFS] Creates a new file, writes the specified byte array to the file, and then closes the file. If the target file already
      ///   exists, it is overwritten.
      /// </summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to write to.</param>
      /// <param name="bytes">The bytes to write to the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "bytes")]
      [SecurityCritical]
      public static void WriteAllBytes(KernelTransaction transaction, string path, byte[] bytes, PathFormat pathFormat)
      {
         WriteAllBytesInternal(transaction, path, bytes, pathFormat);
      }

      #endregion // IsFullPath

      #endregion // Transacted

      #endregion // AlphaFS

      #endregion // WriteAllBytes

      #region WriteAllLines

      #region .NET

      /// <summary>Creates a new file, writes a collection of strings to the file, and then closes the file.</summary>
      /// <remarks>The default behavior of the method is to write out data by using UTF-8 encoding without a byte order mark (BOM).</remarks>
      /// <param name="path">The file to write to.</param>
      /// <param name="contents">The lines to write to the file.</param>
      [SecurityCritical]
      public static void WriteAllLines(string path, IEnumerable<string> contents)
      {
         WriteAppendAllLinesInternal(null, path, contents, new UTF8Encoding(false, true), false, true, PathFormat.Auto);
      }

      /// <summary>Creates a new file by using the specified encoding, writes a collection of strings to the file, and then closes the file.</summary>
      /// <param name="path">The file to write to.</param>
      /// <param name="contents">The string array to write to the file.</param>
      [SecurityCritical]
      public static void WriteAllLines(string path, string[] contents)
      {
         WriteAppendAllLinesInternal(null, path, contents, new UTF8Encoding(false, true), false, true, PathFormat.Auto);
      }

      /// <summary>
      ///   Creates a new file by using the specified encoding, writes a collection of strings to the file, and then closes the file.
      /// </summary>
      /// <param name="path">The file to write to.</param>
      /// <param name="contents">The lines to write to the file.</param>
      /// <param name="encoding">The character <see cref="Encoding"/> to use.</param>
      [SecurityCritical]
      public static void WriteAllLines(string path, IEnumerable<string> contents, Encoding encoding)
      {
         WriteAppendAllLinesInternal(null, path, contents, encoding, false, true, PathFormat.Auto);
      }

      /// <summary>Creates a new file by using the specified encoding, writes a collection of strings to the file, and then closes the file.</summary>
      /// <param name="path">The file to write to.</param>
      /// <param name="contents">The string array to write to the file.</param>
      /// <param name="encoding">The character <see cref="Encoding"/> to use.</param>
      [SecurityCritical]
      public static void WriteAllLines(string path, string[] contents, Encoding encoding)
      {
         WriteAppendAllLinesInternal(null, path, contents, encoding, false, true, PathFormat.Auto);
      }

      #endregion // .NET

      #region AlphaFS

      #region IsFullPath

      /// <summary>[AlphaFS] Creates a new file, writes a collection of strings to the file, and then closes the file.</summary>
      /// <remarks>The default behavior of the method is to write out data by using UTF-8 encoding without a byte order mark (BOM).</remarks>
      /// <param name="path">The file to write to.</param>
      /// <param name="contents">The lines to write to the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void WriteAllLines(string path, IEnumerable<string> contents, PathFormat pathFormat)
      {
         WriteAppendAllLinesInternal(null, path, contents, new UTF8Encoding(false, true), false, true, pathFormat);
      }

      /// <summary>
      ///   [AlphaFS] Creates a new file by using the specified encoding, writes a collection of strings to the file, and then closes the file.
      /// </summary>
      /// <param name="path">The file to write to.</param>
      /// <param name="contents">The string array to write to the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void WriteAllLines(string path, string[] contents, PathFormat pathFormat)
      {
         WriteAppendAllLinesInternal(null, path, contents, new UTF8Encoding(false, true), false, true, pathFormat);
      }

      /// <summary>
      ///   [AlphaFS] Creates a new file by using the specified encoding, writes a collection of strings to the file, and then closes the file.
      /// </summary>
      /// <param name="path">The file to write to.</param>
      /// <param name="contents">The lines to write to the file.</param>
      /// <param name="encoding">The character <see cref="Encoding"/> to use.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void WriteAllLines(string path, IEnumerable<string> contents, Encoding encoding, PathFormat pathFormat)
      {
         WriteAppendAllLinesInternal(null, path, contents, encoding, false, true, pathFormat);
      }

      /// <summary>
      ///   [AlphaFS] Creates a new file by using the specified encoding, writes a collection of strings to the file, and then closes the file.
      /// </summary>
      /// <param name="path">The file to write to.</param>
      /// <param name="contents">The string array to write to the file.</param>
      /// <param name="encoding">The character <see cref="Encoding"/> to use.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void WriteAllLines(string path, string[] contents, Encoding encoding, PathFormat pathFormat)
      {
         WriteAppendAllLinesInternal(null, path, contents, encoding, false, true, pathFormat);
      }

      #endregion // IsFullPath

      #region Transacted

      #region .NET

      /// <summary>[AlphaFS] Creates a new file, writes a collection of strings to the file, and then closes the file.</summary>
      /// <remarks>The default behavior of the method is to write out data by using UTF-8 encoding without a byte order mark (BOM).</remarks>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to write to.</param>
      /// <param name="contents">The lines to write to the file.</param>
      [SecurityCritical]
      public static void WriteAllLines(KernelTransaction transaction, string path, IEnumerable<string> contents)
      {
         WriteAppendAllLinesInternal(transaction, path, contents, new UTF8Encoding(false, true), false, true, PathFormat.Auto);
      }

      /// <summary>[AlphaFS] Creates a new file, writes a collection of strings to the file, and then closes the file.</summary>
      /// <remarks>The default behavior of the method is to write out data by using UTF-8 encoding without a byte order mark (BOM).</remarks>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to write to.</param>
      /// <param name="contents">The string array to write to the file.</param>
      [SecurityCritical]
      public static void WriteAllLines(KernelTransaction transaction, string path, string[] contents)
      {
         WriteAppendAllLinesInternal(transaction, path, contents, new UTF8Encoding(false, true), false, true, PathFormat.Auto);
      }

      /// <summary>
      ///   [AlphaFS] Creates a new file by using the specified encoding, writes a collection of strings to the file, and then closes the file.
      /// </summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to write to.</param>
      /// <param name="contents">The lines to write to the file.</param>
      /// <param name="encoding">The character <see cref="Encoding"/> to use.</param>
      [SecurityCritical]
      public static void WriteAllLines(KernelTransaction transaction, string path, IEnumerable<string> contents, Encoding encoding)
      {
         WriteAppendAllLinesInternal(transaction, path, contents, encoding, false, true, PathFormat.Auto);
      }

      /// <summary>
      ///   Creates a new file by using the specified encoding, writes a collection of strings to the file, and then closes the file.
      /// </summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to write to.</param>
      /// <param name="contents">The string array to write to the file.</param>
      /// <param name="encoding">The character <see cref="Encoding"/> to use.</param>
      [SecurityCritical]
      public static void WriteAllLines(KernelTransaction transaction, string path, string[] contents, Encoding encoding)
      {
         WriteAppendAllLinesInternal(transaction, path, contents, encoding, false, true, PathFormat.Auto);
      }

      #endregion // .NET

      #region IsFullPath

      /// <summary>[AlphaFS] Creates a new file, writes a collection of strings to the file, and then closes the file.</summary>
      /// <remarks>The default behavior of the method is to write out data by using UTF-8 encoding without a byte order mark (BOM).</remarks>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to write to.</param>
      /// <param name="contents">The lines to write to the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void WriteAllLines(KernelTransaction transaction, string path, IEnumerable<string> contents, PathFormat pathFormat)
      {
         WriteAppendAllLinesInternal(transaction, path, contents, new UTF8Encoding(false, true), false, true, pathFormat);
      }

      /// <summary>[AlphaFS] Creates a new file, writes a collection of strings to the file, and then closes the file.</summary>
      /// <remarks>The default behavior of the method is to write out data by using UTF-8 encoding without a byte order mark (BOM).</remarks>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to write to.</param>
      /// <param name="contents">The string array to write to the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void WriteAllLines(KernelTransaction transaction, string path, string[] contents, PathFormat pathFormat)
      {
         WriteAppendAllLinesInternal(transaction, path, contents, new UTF8Encoding(false, true), false, true, pathFormat);
      }

      /// <summary>
      ///   [AlphaFS] Creates a new file by using the specified encoding, writes a collection of strings to the file, and then closes the file.
      /// </summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to write to.</param>
      /// <param name="contents">The lines to write to the file.</param>
      /// <param name="encoding">The character <see cref="Encoding"/> to use.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void WriteAllLines(KernelTransaction transaction, string path, IEnumerable<string> contents, Encoding encoding, PathFormat pathFormat)
      {
         WriteAppendAllLinesInternal(transaction, path, contents, encoding, false, true, pathFormat);
      }

      /// <summary>
      ///   [AlphaFS] Creates a new file by using the specified encoding, writes a collection of strings to the file, and then closes the file.
      /// </summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to write to.</param>
      /// <param name="contents">The string array to write to the file.</param>
      /// <param name="encoding">The character <see cref="Encoding"/> to use.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void WriteAllLines(KernelTransaction transaction, string path, string[] contents, Encoding encoding, PathFormat pathFormat)
      {
         WriteAppendAllLinesInternal(transaction, path, contents, encoding, false, true, pathFormat);
      }

      #endregion // IsFullPath

      #endregion // Transacted

      #endregion // AlphaFS

      #endregion // WriteAllLines

      #region WriteAllText

      #region .NET

      /// <summary>
      ///   Creates a new file, writes the specified string to the file, and then closes the file. If the target file already exists, it is
      ///   overwritten.
      /// </summary>
      /// <remarks>This method uses UTF-8 encoding without a Byte-Order Mark (BOM)</remarks>
      /// <param name="path">The file to write to.</param>
      /// <param name="contents">The string to write to the file.</param>
      [SecurityCritical]
      public static void WriteAllText(string path, string contents)
      {
         WriteAppendAllLinesInternal(null, path, new[] { contents }, new UTF8Encoding(false, true), false, false, PathFormat.Auto);
      }

      /// <summary>
      ///   Creates a new file, writes the specified string to the file using the specified encoding, and then closes the file. If the target
      ///   file already exists, it is overwritten.
      /// </summary>
      /// <param name="path">The file to write to.</param>
      /// <param name="contents">The string to write to the file.</param>
      /// <param name="encoding">The <see cref="Encoding"/> applied to the contents of the file.</param>
      [SecurityCritical]
      public static void WriteAllText(string path, string contents, Encoding encoding)
      {
         WriteAppendAllLinesInternal(null, path, new[] { contents }, encoding, false, false, PathFormat.Auto);
      }

      #endregion // .NET

      #region AlphaFS

      #region IsFullPath

      /// <summary>
      ///   [AlphaFS] Creates a new file, writes the specified string to the file, and then closes the file. If the target file already exists,
      ///   it is overwritten.
      /// </summary>
      /// <remarks>This method uses UTF-8 encoding without a Byte-Order Mark (BOM)</remarks>
      /// <param name="path">The file to write to.</param>
      /// <param name="contents">The string to write to the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void WriteAllText(string path, string contents, PathFormat pathFormat)
      {
         WriteAppendAllLinesInternal(null, path, new[] { contents }, new UTF8Encoding(false, true), false, false, pathFormat);
      }

      /// <summary>
      ///   [AlphaFS] Creates a new file, writes the specified string to the file using the specified encoding, and then closes the file. If
      ///   the target file already exists, it is overwritten.
      /// </summary>
      /// <param name="path">The file to write to.</param>
      /// <param name="contents">The string to write to the file.</param>
      /// <param name="encoding">The <see cref="Encoding"/> applied to the contents of the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void WriteAllText(string path, string contents, Encoding encoding, PathFormat pathFormat)
      {
         WriteAppendAllLinesInternal(null, path, new[] { contents }, encoding, false, false, pathFormat);
      }

      #endregion // IsFullPath

      #region Transacted

      #region .NET

      /// <summary>
      ///   [AlphaFS] Creates a new file as part of a transaction, write the contents to the file, and then closes the file. If the target file
      ///   already exists, it is overwritten.
      /// </summary>
      /// <remarks>This method uses UTF-8 encoding without a Byte-Order Mark (BOM)</remarks>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to write to.</param>
      /// <param name="contents">The string to write to the file.</param>
      [SecurityCritical]
      public static void WriteAllText(KernelTransaction transaction, string path, string contents)
      {
         WriteAppendAllLinesInternal(transaction, path, new[] { contents }, new UTF8Encoding(false, true), false, false, PathFormat.Auto);
      }

      /// <summary>[AlphaFS] Creates a new file as part of a transaction, writes the specified string to the file using the specified encoding, and then closes the file. If the target file already exists, it is overwritten.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to write to.</param>
      /// <param name="contents">The string to write to the file.</param>
      /// <param name="encoding">The <see cref="Encoding"/> applied to the contents of the file.</param>
      [SecurityCritical]
      public static void WriteAllText(KernelTransaction transaction, string path, string contents, Encoding encoding)
      {
         WriteAppendAllLinesInternal(transaction, path, new[] { contents }, encoding, false, false, PathFormat.Auto);
      }

      #endregion // .NET

      #region IsFullPath

      /// <summary>
      ///   [AlphaFS] Creates a new file as part of a transaction, write the contents to the file, and then closes the file. If the target file
      ///   already exists, it is overwritten.
      /// </summary>
      /// <remarks>This method uses UTF-8 encoding without a Byte-Order Mark (BOM)</remarks>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to write to.</param>
      /// <param name="contents">The string to write to the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void WriteAllText(KernelTransaction transaction, string path, string contents, PathFormat pathFormat)
      {
         WriteAppendAllLinesInternal(transaction, path, new[] { contents }, new UTF8Encoding(false, true), false, false, pathFormat);
      }

      /// <summary>
      ///   [AlphaFS] Creates a new file as part of a transaction, writes the specified string to the file using the specified encoding, and
      ///   then closes the file. If the target file already exists, it is overwritten.
      /// </summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to write to.</param>
      /// <param name="contents">The string to write to the file.</param>
      /// <param name="encoding">The <see cref="Encoding"/> applied to the contents of the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void WriteAllText(KernelTransaction transaction, string path, string contents, Encoding encoding, PathFormat pathFormat)
      {
         WriteAppendAllLinesInternal(transaction, path, new[] { contents }, encoding, false, false, pathFormat);
      }

      #endregion // IsFullPath

      #endregion // Transacted

      #endregion // AlphaFS

      #endregion // WriteAllText

      #endregion // .NET

      #region AlphaFS

      #region AddStream

      #region IsFullPath

      /// <summary>[AlphaFS] Adds an alternate data stream (NTFS ADS) to an existing file.</summary>
      /// <param name="path">The path to an existing file.</param>
      /// <param name="name">The name for the stream. If a stream with <paramref name="name"/> already exists, it will be overwritten.</param>
      /// <param name="contents">The lines to add to the stream.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SecurityCritical]
      public static void AddStream(string path, string name, string[] contents, PathFormat pathFormat)
      {
         AlternateDataStreamInfo.AddStreamInternal(false, null, path, name, contents, pathFormat);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Adds an alternate data stream (NTFS ADS) to an existing file.</summary>
      /// <param name="path">The path to an existing file.</param>
      /// <param name="name">The name for the stream. If a stream with <paramref name="name"/> already exists, it will be overwritten.</param>
      /// <param name="contents">The lines to add to the stream.</param>      
      [SecurityCritical]
      public static void AddStream(string path, string name, string[] contents)
      {
         AlternateDataStreamInfo.AddStreamInternal(false, null, path, name, contents, PathFormat.Auto);
      }

      #region Transacted

      #region IsFullPath

      /// <summary>[AlphaFS] Adds an alternate data stream (NTFS ADS) to an existing file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to an existing file.</param>
      /// <param name="name">The name for the stream. If a stream with <paramref name="name"/> already exists, it will be overwritten.</param>
      /// <param name="contents">The lines to add to the stream.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SecurityCritical]
      public static void AddStream(KernelTransaction transaction, string path, string name, string[] contents, PathFormat pathFormat)
      {
         AlternateDataStreamInfo.AddStreamInternal(false, transaction, path, name, contents, pathFormat);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Adds an alternate data stream (NTFS ADS) to an existing file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to an existing file.</param>
      /// <param name="name">The name for the stream. If a stream with <paramref name="name"/> already exists, it will be overwritten.</param>
      /// <param name="contents">The lines to add to the stream.</param>      
      [SecurityCritical]
      public static void AddStream(KernelTransaction transaction, string path, string name, string[] contents)
      {
         AlternateDataStreamInfo.AddStreamInternal(false, transaction, path, name, contents, PathFormat.Auto);
      }

      #endregion Transacted

      #endregion // AddStream

      #region Compress

      #region IsFullPath

      /// <summary>[AlphaFS] Compresses a file using NTFS compression.</summary>
      /// <param name="path">A path that describes a file to compress.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SecurityCritical]
      public static void Compress(string path, PathFormat pathFormat)
      {
         Device.ToggleCompressionInternal(false, null, path, true, pathFormat);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Compresses a file using NTFS compression.</summary>
      /// <param name="path">A path that describes a file to compress.</param>      
      [SecurityCritical]
      public static void Compress(string path)
      {
         Device.ToggleCompressionInternal(false, null, path, true, PathFormat.Auto);
      }

      #region Transacted

      #region IsFullPath

      /// <summary>[AlphaFS] Compresses a file using NTFS compression.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">A path that describes a file to compress.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SecurityCritical]
      public static void Compress(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         Device.ToggleCompressionInternal(false, transaction, path, true, pathFormat);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Compresses a file using NTFS compression.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">A path that describes a file to compress.</param>      
      [SecurityCritical]
      public static void Compress(KernelTransaction transaction, string path)
      {
         Device.ToggleCompressionInternal(false, transaction, path, true, PathFormat.Auto);
      }

      #endregion // Transacted

      #endregion // Compress


      #region CreateHardlink

      #region IsFullPath

      /// <summary>
      ///   [AlphaFS] Establishes a hard link between an existing file and a new file. This function is only supported on the NTFS file system,
      ///   and only for files, not directories.
      /// </summary>
      /// <param name="fileName">The name of the new file. This parameter cannot specify the name of a directory.</param>
      /// <param name="existingFileName">The name of the existing file. This parameter cannot specify the name of a directory.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Hardlink")]
      [SecurityCritical]
      public static void CreateHardlink(string fileName, string existingFileName, PathFormat pathFormat)
      {
         CreateHardlinkInternal(null, fileName, existingFileName, pathFormat);
      }

      #endregion // IsFullPath

      /// <summary>
      ///   [AlphaFS] Establishes a hard link between an existing file and a new file. This function is only supported on the NTFS file system,
      ///   and only for files, not directories.
      /// </summary>
      /// <param name="fileName">The name of the new file. This parameter cannot specify the name of a directory.</param>
      /// <param name="existingFileName">The name of the existing file. This parameter cannot specify the name of a directory.</param>      
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Hardlink")]
      [SecurityCritical]
      public static void CreateHardlink(string fileName, string existingFileName)
      {
         CreateHardlinkInternal(null, fileName, existingFileName, PathFormat.Auto);
      }

      #region Transacted

      #region IsFullPath

      /// <summary>
      ///   [AlphaFS] Establishes a hard link between an existing file and a new file. This function is only supported on the NTFS file system,
      ///   and only for files, not directories.
      /// </summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="fileName">The name of the new file. This parameter cannot specify the name of a directory.</param>
      /// <param name="existingFileName">The name of the existing file. This parameter cannot specify the name of a directory.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Hardlink")]
      [SecurityCritical]
      public static void CreateHardlink(KernelTransaction transaction, string fileName, string existingFileName, PathFormat pathFormat)
      {
         CreateHardlinkInternal(transaction, fileName, existingFileName, pathFormat);
      }

      #endregion // IsFullPath

      /// <summary>
      ///   [AlphaFS] Establishes a hard link between an existing file and a new file. This function is only supported on the NTFS file system,
      ///   and only for files, not directories.
      /// </summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="fileName">The name of the new file. This parameter cannot specify the name of a directory.</param>
      /// <param name="existingFileName">The name of the existing file. This parameter cannot specify the name of a directory.</param>      
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Hardlink")]
      [SecurityCritical]
      public static void CreateHardlink(KernelTransaction transaction, string fileName, string existingFileName)
      {
         CreateHardlinkInternal(transaction, fileName, existingFileName, PathFormat.Auto);
      }

      #endregion // Transacted

      #endregion // CreateHardlink

      #region CreateSymbolicLink

      #region IsFullPath

      /// <summary>[AlphaFS] Creates a symbolic link.</summary>
      /// <param name="symlinkFileName">The name of the target for the symbolic link to be created.</param>
      /// <param name="targetFileName">The symbolic link to be created.</param>
      /// <param name="targetType">Indicates whether the link target, <paramref name="targetFileName"/>, is a file or directory.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "symlink")]
      [SecurityCritical]
      public static void CreateSymbolicLink(string symlinkFileName, string targetFileName, SymbolicLinkTarget targetType, PathFormat pathFormat)
      {
         CreateSymbolicLinkInternal(null, symlinkFileName, targetFileName, targetType, pathFormat);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Creates a symbolic link.</summary>
      /// <param name="symlinkFileName">The name of the target for the symbolic link to be created.</param>
      /// <param name="targetFileName">The symbolic link to be created.</param>
      /// <param name="targetType">Indicates whether the link target, <paramref name="targetFileName"/>, is a file or directory.</param>      
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "symlink")]
      [SecurityCritical]
      public static void CreateSymbolicLink(string symlinkFileName, string targetFileName, SymbolicLinkTarget targetType)
      {
         CreateSymbolicLinkInternal(null, symlinkFileName, targetFileName, targetType, PathFormat.Auto);
      }

      #region Transacted

      #region IsFullPath

      /// <summary>[AlphaFS] Creates a symbolic link.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="symlinkFileName">The name of the target for the symbolic link to be created.</param>
      /// <param name="targetFileName">The symbolic link to be created.</param>
      /// <param name="targetType">Indicates whether the link target, <paramref name="targetFileName"/>, is a file or directory.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "symlink")]
      [SecurityCritical]
      public static void CreateSymbolicLink(KernelTransaction transaction, string symlinkFileName, string targetFileName, SymbolicLinkTarget targetType, PathFormat pathFormat)
      {
         CreateSymbolicLinkInternal(transaction, symlinkFileName, targetFileName, targetType, pathFormat);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Creates a symbolic link.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="symlinkFileName">The name of the target for the symbolic link to be created.</param>
      /// <param name="targetFileName">The symbolic link to be created.</param>
      /// <param name="targetType">Indicates whether the link target, <paramref name="targetFileName"/>, is a file or directory.</param>      
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "symlink")]
      [SecurityCritical]
      public static void CreateSymbolicLink(KernelTransaction transaction, string symlinkFileName, string targetFileName, SymbolicLinkTarget targetType)
      {
         CreateSymbolicLinkInternal(transaction, symlinkFileName, targetFileName, targetType, PathFormat.Auto);
      }

      #endregion // Transacted

      #endregion // CreateSymbolicLink

      #region Decompress

      #region IsFullPath

      /// <summary>[AlphaFS] Decompresses an NTFS compressed file.</summary>
      /// <param name="path">A path that describes a file to decompress.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SecurityCritical]
      public static void Decompress(string path, PathFormat pathFormat)
      {
         Device.ToggleCompressionInternal(false, null, path, false, pathFormat);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Decompresses an NTFS compressed file.</summary>
      /// <param name="path">A path that describes a file to decompress.</param>      
      [SecurityCritical]
      public static void Decompress(string path)
      {
         Device.ToggleCompressionInternal(false, null, path, false, PathFormat.Auto);
      }

      #region Transacted

      #region IsFullPath

      /// <summary>[AlphaFS] Decompresses an NTFS compressed file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">A path that describes a file to decompress.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SecurityCritical]
      public static void Decompress(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         Device.ToggleCompressionInternal(false, transaction, path, false, pathFormat);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Decompresses an NTFS compressed file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">A path that describes a file to decompress.</param>      
      [SecurityCritical]
      public static void Decompress(KernelTransaction transaction, string path)
      {
         Device.ToggleCompressionInternal(false, transaction, path, false, PathFormat.Auto);
      }

      #endregion // Transacted

      #endregion // Decompress

      #region EnumerateHardlinks

      #region IsFullPath

      /// <summary>[AlphaFS] Creates an enumeration of all the hard links to the specified <paramref name="path"/>.</summary>
      /// <param name="path">The name of the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>An enumerable collection of <see cref="string"/> of all the hard links to the specified <paramref name="path"/></returns>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Hardlinks")]
      [SecurityCritical]
      public static IEnumerable<string> EnumerateHardlinks(string path, PathFormat pathFormat)
      {
         return EnumerateHardlinksInternal(null, path, pathFormat);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Creates an enumeration of all the hard links to the specified <paramref name="path"/>.</summary>
      /// <param name="path">The name of the file.</param>
      /// <returns>An enumerable collection of <see cref="string"/> of all the hard links to the specified <paramref name="path"/></returns>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Hardlinks")]
      [SecurityCritical]
      public static IEnumerable<string> EnumerateHardlinks(string path)
      {
         return EnumerateHardlinksInternal(null, path, PathFormat.Auto);
      }

      #region Transacted

      #region IsFullPath

      /// <summary>[AlphaFS] Creates an enumeration of all the hard links to the specified <paramref name="path"/>.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The name of the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>An enumerable collection of <see cref="string"/> of all the hard links to the specified <paramref name="path"/></returns>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Hardlinks")]
      [SecurityCritical]
      public static IEnumerable<string> EnumerateHardlinks(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         return EnumerateHardlinksInternal(transaction, path, pathFormat);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Creates an enumeration of all the hard links to the specified <paramref name="path"/>.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The name of the file.</param>
      /// <returns>An enumerable collection of <see cref="string"/> of all the hard links to the specified <paramref name="path"/></returns>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Hardlinks")]
      [SecurityCritical]
      public static IEnumerable<string> EnumerateHardlinks(KernelTransaction transaction, string path)
      {
         return EnumerateHardlinksInternal(transaction, path, PathFormat.Auto);
      }

      #endregion // Transacted

      #endregion // EnumerateHardlinks

      #region EnumerateStreams

      #region IsFullPath

      /// <summary>
      ///   [AlphaFS] Returns an enumerable collection of <see cref="AlternateDataStreamInfo"/> instances for the file specified by
      ///   <paramref name="path"/>.
      /// </summary>
      /// <param name="path">The file to search.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>
      ///   An enumerable collection of <see cref="AlternateDataStreamInfo"/> instances for the file specified by <paramref name="path"/>.
      /// </returns>      
      [SecurityCritical]
      public static IEnumerable<AlternateDataStreamInfo> EnumerateStreams(string path, PathFormat pathFormat)
      {
         return AlternateDataStreamInfo.EnumerateStreamsInternal(null, null, null, path, null, null, pathFormat);
      }

      /// <summary>
      ///   [AlphaFS] Returns an enumerable collection of <see cref="AlternateDataStreamInfo"/> of type <see cref="StreamType"/> instances for
      ///   the file specified by <paramref name="path"/>.
      /// </summary>
      /// <param name="path">The file to search.</param>
      /// <param name="streamType">The type of stream to retrieve.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>
      ///   An enumerable collection of <see cref="AlternateDataStreamInfo"/> of type <see cref="StreamType"/> instances for the file specified
      ///   by <paramref name="path"/>.
      /// </returns>      
      [SecurityCritical]
      public static IEnumerable<AlternateDataStreamInfo> EnumerateStreams(string path, StreamType streamType, PathFormat pathFormat)
      {
         return AlternateDataStreamInfo.EnumerateStreamsInternal(null, null, null, path, null, streamType, pathFormat);
      }

      #endregion // IsFullPath

      /// <summary>
      ///   [AlphaFS] Returns an enumerable collection of <see cref="AlternateDataStreamInfo"/> instances for the file specified by
      ///   <paramref name="path"/>.
      /// </summary>
      /// <param name="path">The file to search.</param>
      /// <returns>
      ///   An enumerable collection of <see cref="AlternateDataStreamInfo"/> instances for the file specified by <paramref name="path"/>.
      /// </returns>      
      [SecurityCritical]
      public static IEnumerable<AlternateDataStreamInfo> EnumerateStreams(string path)
      {
         return AlternateDataStreamInfo.EnumerateStreamsInternal(null, null, null, path, null, null, PathFormat.Auto);
      }

      /// <summary>
      ///   [AlphaFS] Returns an enumerable collection of <see cref="AlternateDataStreamInfo"/> of type <see cref="StreamType"/> instances for
      ///   the file specified by <paramref name="path"/>.
      /// </summary>
      /// <param name="path">The file to search.</param>
      /// <param name="streamType">The type of stream to retrieve.</param>
      /// <returns>
      ///   An enumerable collection of <see cref="AlternateDataStreamInfo"/> of type <see cref="StreamType"/> instances for the file specified
      ///   by <paramref name="path"/>.
      /// </returns>      
      [SecurityCritical]
      public static IEnumerable<AlternateDataStreamInfo> EnumerateStreams(string path, StreamType streamType)
      {
         return AlternateDataStreamInfo.EnumerateStreamsInternal(null, null, null, path, null, streamType, PathFormat.Auto);
      }

      /// <summary>
      ///   [AlphaFS] Returns an enumerable collection of <see cref="AlternateDataStreamInfo"/> instances for the handle specified by
      ///   <paramref name="handle"/>.
      /// </summary>
      /// <param name="handle">A <see cref="SafeFileHandle"/> connected to the file from which to retrieve the information.</param>
      /// <returns>
      ///   An enumerable collection of <see cref="AlternateDataStreamInfo"/> instances for the handle specified by <paramref name="handle"/>.
      /// </returns>      
      [SecurityCritical]
      public static IEnumerable<AlternateDataStreamInfo> EnumerateStreams(SafeFileHandle handle)
      {
         return AlternateDataStreamInfo.EnumerateStreamsInternal(null, null, handle, null, null, null, PathFormat.ExtendedLength);
      }

      #region Transacted

      #region IsFullPath

      /// <summary>
      ///   [AlphaFS] Returns an enumerable collection of <see cref="AlternateDataStreamInfo"/> instances for the file specified by
      ///   <paramref name="path"/>.
      /// </summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to search.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>
      ///   An enumerable collection of <see cref="AlternateDataStreamInfo"/> instances for the file specified by <paramref name="path"/>.
      /// </returns>      
      [SecurityCritical]
      public static IEnumerable<AlternateDataStreamInfo> EnumerateStreams(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         return AlternateDataStreamInfo.EnumerateStreamsInternal(null, transaction, null, path, null, null, pathFormat);
      }

      /// <summary>
      ///   [AlphaFS] Returns an enumerable collection of <see cref="AlternateDataStreamInfo"/> of type <see cref="StreamType"/> instances for
      ///   the file specified by <paramref name="path"/>.
      /// </summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to search.</param>
      /// <param name="streamType">The type of stream to retrieve.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>
      ///   An enumerable collection of <see cref="AlternateDataStreamInfo"/> of type <see cref="StreamType"/> instances for the file specified
      ///   by <paramref name="path"/>.
      /// </returns>      
      [SecurityCritical]
      public static IEnumerable<AlternateDataStreamInfo> EnumerateStreams(KernelTransaction transaction, string path, StreamType streamType, PathFormat pathFormat)
      {
         return AlternateDataStreamInfo.EnumerateStreamsInternal(null, transaction, null, path, null, streamType, pathFormat);
      }

      #endregion // IsFullPath

      /// <summary>
      ///   [AlphaFS] Returns an enumerable collection of <see cref="AlternateDataStreamInfo"/> instances for the file specified by
      ///   <paramref name="path"/>.
      /// </summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to search.</param>
      /// <returns>
      ///   An enumerable collection of <see cref="AlternateDataStreamInfo"/> instances for the file specified by <paramref name="path"/>.
      /// </returns>      
      [SecurityCritical]
      public static IEnumerable<AlternateDataStreamInfo> EnumerateStreams(KernelTransaction transaction, string path)
      {
         return AlternateDataStreamInfo.EnumerateStreamsInternal(null, transaction, null, path, null, null, PathFormat.Auto);
      }

      /// <summary>
      ///   [AlphaFS] Returns an enumerable collection of <see cref="AlternateDataStreamInfo"/> of type <see cref="StreamType"/> instances for
      ///   the file specified by <paramref name="path"/>.
      /// </summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to search.</param>
      /// <param name="streamType">The type of stream to retrieve.</param>
      /// <returns>
      ///   An enumerable collection of <see cref="AlternateDataStreamInfo"/> of type <see cref="StreamType"/> instances for the file specified
      ///   by <paramref name="path"/>.
      /// </returns>      
      [SecurityCritical]
      public static IEnumerable<AlternateDataStreamInfo> EnumerateStreams(KernelTransaction transaction, string path, StreamType streamType)
      {
         return AlternateDataStreamInfo.EnumerateStreamsInternal(null, transaction, null, path, null, streamType, PathFormat.Auto);
      }

      #endregion // Transacted

      #endregion // EnumerateStreams

      #region GetChangeTime

      #region IsFullPath

      /// <summary>Gets the change date and time of the specified file.</summary>
      /// <param name="path">The file for which to obtain creation date and time information.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>
      ///   A <see cref="System.DateTime"/> structure set to the change date and time for the specified file. This value is expressed in local
      ///   time.
      /// </returns>
      [SecurityCritical]
      public static DateTime GetChangeTime(string path, PathFormat pathFormat)
      {
         return GetChangeTimeInternal(false, null, null, path, false, pathFormat);
      }

      #endregion // IsFullPath

      /// <summary>Gets the change date and time of the specified file.</summary>
      /// <param name="path">The file for which to obtain creation date and time information.</param>
      /// <returns>
      ///   A <see cref="System.DateTime"/> structure set to the change date and time for the specified file. This value is expressed in local
      ///   time.
      /// </returns>
      [SecurityCritical]
      public static DateTime GetChangeTime(string path)
      {
         return GetChangeTimeInternal(false, null, null, path, false, PathFormat.Auto);
      }

      /// <summary>Gets the change date and time of the specified file.</summary>
      /// <param name="safeHandle">An open handle to the file or directory from which to retrieve information.</param>
      /// <returns>
      ///   A <see cref="System.DateTime"/> structure set to the change date and time for the specified file. This value is expressed in local
      ///   time.
      /// </returns>
      [SecurityCritical]
      public static DateTime GetChangeTime(SafeFileHandle safeHandle)
      {
         return GetChangeTimeInternal(false, null, safeHandle, null, false, PathFormat.ExtendedLength);
      }

      #region Transacted

      #region IsFullPath

      /// <summary>Gets the change date and time of the specified file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file for which to obtain creation date and time information.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>
      ///   A <see cref="System.DateTime"/> structure set to the change date and time for the specified file. This value is expressed in local
      ///   time.
      /// </returns>
      [SecurityCritical]
      public static DateTime GetChangeTime(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         return GetChangeTimeInternal(false, transaction, null, path, false, pathFormat);
      }

      #endregion // IsFullPath

      /// <summary>Gets the change date and time of the specified file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file for which to obtain creation date and time information.</param>
      /// <returns>
      ///   A <see cref="System.DateTime"/> structure set to the change date and time for the specified file. This value is expressed in local
      ///   time.
      /// </returns>
      [SecurityCritical]
      public static DateTime GetChangeTime(KernelTransaction transaction, string path)
      {
         return GetChangeTimeInternal(false, transaction, null, path, false, PathFormat.Auto);
      }

      #endregion // Transacted

      #endregion // GetChangeTime

      #region GetChangeTimeUtc

      #region IsFullPath

      /// <summary>Gets the change date and time, in Coordinated Universal Time (UTC) format, of the specified file.</summary>
      /// <param name="path">The file for which to obtain change date and time information, in Coordinated Universal Time (UTC) format.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>
      ///   A <see cref="System.DateTime"/> structure set to the change date and time for the specified file. This value is expressed in UTC
      ///   time.
      /// </returns>
      [SecurityCritical]
      public static DateTime GetChangeTimeUtc(string path, PathFormat pathFormat)
      {
         return GetChangeTimeInternal(false, null, null, path, true, pathFormat);
      }

      #endregion // IsFullPath

      /// <summary>Gets the change date and time, in Coordinated Universal Time (UTC) format, of the specified file.</summary>
      /// <param name="path">The file for which to obtain change date and time information, in Coordinated Universal Time (UTC) format.</param>
      /// <returns>
      ///   A <see cref="System.DateTime"/> structure set to the change date and time for the specified file. This value is expressed in UTC
      ///   time.
      /// </returns>
      [SecurityCritical]
      public static DateTime GetChangeTimeUtc(string path)
      {
         return GetChangeTimeInternal(false, null, null, path, true, PathFormat.Auto);
      }

      /// <summary>Gets the change date and time, in Coordinated Universal Time (UTC) format, of the specified file.</summary>
      /// <param name="safeHandle">An open handle to the file or directory from which to retrieve information.</param>
      /// <returns>
      ///   A <see cref="System.DateTime"/> structure set to the change date and time for the specified file. This value is expressed in UTC
      ///   time.
      /// </returns>
      [SecurityCritical]
      public static DateTime GetChangeTimeUtc(SafeFileHandle safeHandle)
      {
         return GetChangeTimeInternal(false, null, safeHandle, null, true, PathFormat.ExtendedLength);
      }

      #region Transacted

      #region IsFullPath

      /// <summary>Gets the change date and time, in Coordinated Universal Time (UTC) format, of the specified file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file for which to obtain change date and time information, in Coordinated Universal Time (UTC) format.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>
      ///   A <see cref="System.DateTime"/> structure set to the change date and time for the specified file. This value is expressed in UTC
      ///   time.
      /// </returns>
      [SecurityCritical]
      public static DateTime GetChangeTimeUtc(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         return GetChangeTimeInternal(false, transaction, null, path, true, pathFormat);
      }

      #endregion // IsFullPath

      /// <summary>Gets the change date and time, in Coordinated Universal Time (UTC) format, of the specified file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file for which to obtain change date and time information, in Coordinated Universal Time (UTC) format.</param>
      /// <returns>
      ///   A <see cref="System.DateTime"/> structure set to the change date and time for the specified file. This value is expressed in UTC
      ///   time.
      /// </returns>
      [SecurityCritical]
      public static DateTime GetChangeTimeUtc(KernelTransaction transaction, string path)
      {
         return GetChangeTimeInternal(false, transaction, null, path, true, PathFormat.Auto);
      }

      #endregion // Transacted

      #endregion // GetChangeTimeUtc

      #region GetCompressedSize

      #region IsFullPath

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

      #endregion // IsFullPath

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
         return GetCompressedSizeInternal(null, path, PathFormat.Auto);
      }

      #region Transacted

      #region IsFullPath

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

      #endregion // IsFullPath

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
         return GetCompressedSizeInternal(transaction, path, PathFormat.Auto);
      }

      #endregion // Transacted

      #endregion // GetCompressedSize

      #region GetEncryptionStatus

      #region IsFullPath

      /// <summary>[AlphaFS] Retrieves the encryption status of the specified file.</summary>
      /// <param name="path">The name of the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>The <see cref="FileEncryptionStatus"/> of the specified <paramref name="path"/>.</returns>      
      [SecurityCritical]
      public static FileEncryptionStatus GetEncryptionStatus(string path, PathFormat pathFormat)
      {
         return GetEncryptionStatusInternal(path, pathFormat);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Retrieves the encryption status of the specified file.</summary>
      /// <param name="path">The name of the file.</param>
      /// <returns>The <see cref="FileEncryptionStatus"/> of the specified <paramref name="path"/>.</returns>      
      [SecurityCritical]
      public static FileEncryptionStatus GetEncryptionStatus(string path)
      {
         return GetEncryptionStatusInternal(path, PathFormat.Auto);
      }

      #endregion // GetEncryptionStatus

      #region GetFileInfoByHandle

      /// <summary>[AlphaFS] Retrieves file information for the specified <see cref="SafeFileHandle"/>.</summary>
      /// <param name="handle">A <see cref="SafeFileHandle"/> connected to the open file from which to retrieve the information.</param>
      /// <returns>A <see cref="ByHandleFileInfo"/> object containing the requested information.</returns>
      [SecurityCritical]
      public static ByHandleFileInfo GetFileInfoByHandle(SafeFileHandle handle)
      {
         NativeMethods.IsValidHandle(handle);

         NativeMethods.ByHandleFileInfo info;

         if (!NativeMethods.GetFileInformationByHandle(handle, out info))
            // Throws IOException.
            NativeError.ThrowException(Marshal.GetLastWin32Error(), true);

         return new ByHandleFileInfo(info);
      }

      #endregion // GetFileInfoByHandle

      #region GetFileSystemEntry

      #region IsFullPath

      /// <summary>[AlphaFS] Gets the <see cref="FileSystemEntryInfo"/> of the file on the path.</summary>
      /// <returns>The <see cref="FileSystemEntryInfo"/> instance of the file or directory.</returns>
      /// <param name="path">The path to the file or directory.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static FileSystemEntryInfo GetFileSystemEntryInfo(string path, PathFormat pathFormat)
      {
         return GetFileSystemEntryInfoInternal(null, path, false, pathFormat);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Gets the <see cref="FileSystemEntryInfo"/> of the file on the path.</summary>
      /// <returns>The <see cref="FileSystemEntryInfo"/> instance of the file or directory.</returns>
      /// <param name="path">The path to the file or directory.</param>
      [SecurityCritical]
      public static FileSystemEntryInfo GetFileSystemEntryInfo(string path)
      {
         return GetFileSystemEntryInfoInternal(null, path, false, PathFormat.Auto);
      }

      #region Transacted

      #region IsFullPath

      /// <summary>[AlphaFS] Gets the <see cref="FileSystemEntryInfo"/> of the file on the path.</summary>
      /// <returns>The <see cref="FileSystemEntryInfo"/> instance of the file or directory.</returns>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to the file or directory.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static FileSystemEntryInfo GetFileSystemEntryInfo(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         return GetFileSystemEntryInfoInternal(transaction, path, false, pathFormat);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Gets the <see cref="FileSystemEntryInfo"/> of the file on the path.</summary>
      /// <returns>The <see cref="FileSystemEntryInfo"/> instance of the file or directory.</returns>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to the file or directory.</param>
      [SecurityCritical]
      public static FileSystemEntryInfo GetFileSystemEntryInfo(KernelTransaction transaction, string path)
      {
         return GetFileSystemEntryInfoInternal(transaction, path, false, PathFormat.Auto);
      }

      #endregion // Transacted

      #endregion // GetFileSystemEntry

      #region GetLinkTargetInfo

      #region IsFullPath

      /// <summary>[AlphaFS] Gets information about the target of a mount point or symbolic link on an NTFS file system.</summary>
      /// <param name="path">The path to the reparse point.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>
      ///   An instance of <see cref="LinkTargetInfo"/> or <see cref="SymbolicLinkTargetInfo"/> containing information about the symbolic link
      ///   or mount point pointed to by <paramref name="path"/>.
      /// </returns>
      [SecurityCritical]
      public static LinkTargetInfo GetLinkTargetInfo(string path, PathFormat pathFormat)
      {
         return GetLinkTargetInfoInternal(null, path, pathFormat);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Gets information about the target of a mount point or symbolic link on an NTFS file system.</summary>
      /// <param name="path">The path to the reparse point.</param>
      /// <returns>
      ///   An instance of <see cref="LinkTargetInfo"/> or <see cref="SymbolicLinkTargetInfo"/> containing information about the symbolic link
      ///   or mount point pointed to by <paramref name="path"/>.
      /// </returns>
      [SecurityCritical]
      public static LinkTargetInfo GetLinkTargetInfo(string path)
      {
         return GetLinkTargetInfoInternal(null, path, PathFormat.Auto);
      }

      #region Transacted

      #region IsFullPath

      /// <summary>[AlphaFS] Gets information about the target of a mount point or symbolic link on an NTFS file system.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to the reparse point.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>
      ///   An instance of <see cref="LinkTargetInfo"/> or <see cref="SymbolicLinkTargetInfo"/> containing information about the symbolic link
      ///   or mount point pointed to by <paramref name="path"/>.
      /// </returns>
      [SecurityCritical]
      public static LinkTargetInfo GetLinkTargetInfo(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         return GetLinkTargetInfoInternal(transaction, path, pathFormat);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Gets information about the target of a mount point or symbolic link on an NTFS file system.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to the reparse point.</param>
      /// <returns>
      ///   An instance of <see cref="LinkTargetInfo"/> or <see cref="SymbolicLinkTargetInfo"/> containing information about the symbolic link
      ///   or mount point pointed to by <paramref name="path"/>.
      /// </returns>
      [SecurityCritical]
      public static LinkTargetInfo GetLinkTargetInfo(KernelTransaction transaction, string path)
      {
         return GetLinkTargetInfoInternal(transaction, path, PathFormat.Auto);
      }

      #endregion // Transacted

      #endregion // GetLinkTargetInfo

      #region GetSize

      #region IsFullPath

      /// <summary>[AlphaFS] Retrieves the file size, in bytes to store a specified file.</summary>
      /// <param name="path">The path to the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>The file size, in bytes.</returns>      
      [SecurityCritical]
      public static long GetSize(string path, PathFormat pathFormat)
      {
         return GetSizeInternal(null, null, path, pathFormat);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Retrieves the file size, in bytes to store a specified file.</summary>
      /// <param name="path">The path to the file.</param>
      /// <returns>The file size, in bytes.</returns>      
      [SecurityCritical]
      public static long GetSize(string path)
      {
         return GetSizeInternal(null, null, path, PathFormat.Auto);
      }

      /// <summary>[AlphaFS] Retrieves the file size, in bytes to store a specified file.</summary>
      /// <param name="handle">The <see cref="SafeFileHandle"/> to the file.</param>
      /// <returns>The file size, in bytes.</returns>      
      [SecurityCritical]
      public static long GetSize(SafeFileHandle handle)
      {
         return GetSizeInternal(null, handle, null, PathFormat.ExtendedLength);
      }

      #region Transacted

      #region IsFullPath

      /// <summary>[AlphaFS] Retrieves the file size, in bytes to store a specified file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>The number of bytes of disk storage used to store the specified file.</returns>
      [SecurityCritical]
      public static long GetSize(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         return GetSizeInternal(transaction, null, path, pathFormat);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Retrieves the file size, in bytes to store a specified file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to the file.</param>
      /// <returns>The number of bytes of disk storage used to store the specified file.</returns>
      [SecurityCritical]
      public static long GetSize(KernelTransaction transaction, string path)
      {
         return GetSizeInternal(transaction, null, path, PathFormat.Auto);
      }

      #endregion // Transacted

      #endregion // GetSize

      #region GetStreamSize

      #region IsFullPath

      /// <summary>[AlphaFS] Retrieves the actual number of bytes of disk storage used by all data streams (NTFS ADS).</summary>
      /// <param name="path">The path to an existing file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>The number of bytes used by all data streams.</returns>      
      [SecurityCritical]
      public static long GetStreamSize(string path, PathFormat pathFormat)
      {
         return AlternateDataStreamInfo.GetStreamSizeInternal(false, null, null, path, null, null, pathFormat);
      }

      /// <summary>[AlphaFS] Retrieves the actual number of bytes of disk storage used by a named data streams (NTFS ADS).</summary>
      /// <param name="path">The path to an existing file.</param>
      /// <param name="name">The name of the stream to retrieve.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>The number of bytes used by a named stream.</returns>      
      [SecurityCritical]
      public static long GetStreamSize(string path, string name, PathFormat pathFormat)
      {
         return AlternateDataStreamInfo.GetStreamSizeInternal(false, null, null, path, name, StreamType.Data, pathFormat);
      }

      /// <summary>
      ///   [AlphaFS] Retrieves the actual number of bytes of disk storage used by a <see cref="StreamType"/> data streams (NTFS ADS).
      /// </summary>
      /// <param name="path">The path to an existing file.</param>
      /// <param name="type">The <see cref="StreamType"/> of the stream to retrieve.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>The number of bytes used by stream of type <see cref="StreamType"/>.</returns>      
      [SecurityCritical]
      public static long GetStreamSize(string path, StreamType type, PathFormat pathFormat)
      {
         return AlternateDataStreamInfo.GetStreamSizeInternal(false, null, null, path, null, type, pathFormat);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Retrieves the actual number of bytes of disk storage used by all data streams (NTFS ADS).</summary>
      /// <param name="path">The path to an existing file.</param>
      /// <returns>The number of bytes used by all data streams.</returns>      
      [SecurityCritical]
      public static long GetStreamSize(string path)
      {
         return AlternateDataStreamInfo.GetStreamSizeInternal(false, null, null, path, null, null, PathFormat.Auto);
      }

      /// <summary>[AlphaFS] Retrieves the actual number of bytes of disk storage used by a named data streams (NTFS ADS).</summary>
      /// <param name="path">The path to an existing file.</param>
      /// <param name="name">The name of the stream to retrieve.</param>
      /// <returns>The number of bytes used by a named stream.</returns>      
      [SecurityCritical]
      public static long GetStreamSize(string path, string name)
      {
         return AlternateDataStreamInfo.GetStreamSizeInternal(false, null, null, path, name, StreamType.Data, PathFormat.Auto);
      }

      /// <summary>
      ///   [AlphaFS] Retrieves the actual number of bytes of disk storage used by a <see cref="StreamType"/> data streams (NTFS ADS).
      /// </summary>
      /// <param name="path">The path to an existing file.</param>
      /// <param name="type">The <see cref="StreamType"/> of the stream to retrieve.</param>
      /// <returns>The number of bytes used by stream of type <see cref="StreamType"/>.</returns>      
      [SecurityCritical]
      public static long GetStreamSize(string path, StreamType type)
      {
         return AlternateDataStreamInfo.GetStreamSizeInternal(false, null, null, path, null, type, PathFormat.Auto);
      }

      /// <summary>[AlphaFS] Retrieves the actual number of bytes of disk storage used by all data streams (NTFS ADS).</summary>
      /// <param name="handle">The <see cref="SafeFileHandle"/> to the file.</param>
      /// <param name="name">The name of the stream to retrieve.</param>
      /// <returns>The number of bytes used by a named stream.</returns>      
      [SecurityCritical]
      public static long GetStreamSize(SafeFileHandle handle, string name)
      {
         return AlternateDataStreamInfo.GetStreamSizeInternal(false, null, handle, null, name, StreamType.Data, PathFormat.ExtendedLength);
      }

      /// <summary>
      ///   [AlphaFS] Retrieves the actual number of bytes of disk storage used by a <see cref="StreamType"/> data streams (NTFS ADS).
      /// </summary>
      /// <param name="handle">The <see cref="SafeFileHandle"/> to the file.</param>
      /// <param name="type">The <see cref="StreamType"/> of the stream to retrieve.</param>
      /// <returns>The number of bytes used by stream of type <see cref="StreamType"/>.</returns>      
      [SecurityCritical]
      public static long GetStreamSize(SafeFileHandle handle, StreamType type)
      {
         return AlternateDataStreamInfo.GetStreamSizeInternal(false, null, handle, null, null, type, PathFormat.ExtendedLength);
      }

      #region Transacted

      #region IsFullPath

      /// <summary>[AlphaFS] Retrieves the actual number of bytes of disk storage used by all data streams (NTFS ADS).</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to an existing file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>The number of bytes used by all data streams.</returns>      
      [SecurityCritical]
      public static long GetStreamSize(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         return AlternateDataStreamInfo.GetStreamSizeInternal(false, transaction, null, path, null, null, pathFormat);
      }

      /// <summary>[AlphaFS] Retrieves the actual number of bytes of disk storage used by a named data streams (NTFS ADS).</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to an existing file.</param>
      /// <param name="name">The name of the stream to retrieve.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>The number of bytes used by a named stream.</returns>      
      [SecurityCritical]
      public static long GetStreamSize(KernelTransaction transaction, string path, string name, PathFormat pathFormat)
      {
         return AlternateDataStreamInfo.GetStreamSizeInternal(false, transaction, null, path, name, StreamType.Data, pathFormat);
      }

      /// <summary>
      ///   [AlphaFS] Retrieves the actual number of bytes of disk storage used by a <see cref="StreamType"/> data streams (NTFS ADS).
      /// </summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to an existing file.</param>
      /// <param name="type">The <see cref="StreamType"/> of the stream to retrieve.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>The number of bytes used by stream of type <see cref="StreamType"/>.</returns>      
      [SecurityCritical]
      public static long GetStreamSize(KernelTransaction transaction, string path, StreamType type, PathFormat pathFormat)
      {
         return AlternateDataStreamInfo.GetStreamSizeInternal(false, transaction, null, path, null, type, pathFormat);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Retrieves the actual number of bytes of disk storage used by all data streams (NTFS ADS).</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to an existing file.</param>
      /// <returns>The number of bytes used by all data streams.</returns>      
      [SecurityCritical]
      public static long GetStreamSize(KernelTransaction transaction, string path)
      {
         return AlternateDataStreamInfo.GetStreamSizeInternal(false, transaction, null, path, null, null, PathFormat.Auto);
      }

      /// <summary>[AlphaFS] Retrieves the actual number of bytes of disk storage used by a named data streams (NTFS ADS).</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to an existing file.</param>
      /// <param name="name">The name of the stream to retrieve.</param>
      /// <returns>The number of bytes used by a named stream.</returns>      
      [SecurityCritical]
      public static long GetStreamSize(KernelTransaction transaction, string path, string name)
      {
         return AlternateDataStreamInfo.GetStreamSizeInternal(false, transaction, null, path, name, null, PathFormat.Auto);
      }

      /// <summary>[AlphaFS] Retrieves the actual number of bytes of disk storage used by all data streams (NTFS ADS).</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to an existing file.</param>
      /// <param name="type">The <see cref="StreamType"/> of the stream to retrieve.</param>
      /// <returns>The number of bytes used by stream of type <see cref="StreamType"/>.</returns>      
      [SecurityCritical]
      public static long GetStreamSize(KernelTransaction transaction, string path, StreamType type)
      {
         return AlternateDataStreamInfo.GetStreamSizeInternal(false, transaction, null, path, null, type, PathFormat.Auto);
      }

      #endregion // Transacted

      #endregion GetStreamSize

      

      #region OpenBackupRead

      #region IsFullPath

      /// <summary>[AlphaFS] Opens the specified file for reading purposes bypassing security attributes.</summary>
      /// <param name="path">The file path to open.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>A <see cref="FileStream"/> on the specified path, having the read-only mode and sharing options.</returns>
      [SecurityCritical]
      public static FileStream OpenBackupRead(string path, PathFormat pathFormat)
      {
         return OpenInternal(null, path, FileMode.Open, FileSystemRights.ReadData, FileAccess.Read, FileShare.None, ExtendedFileAttributes.BackupSemantics | ExtendedFileAttributes.SequentialScan | ExtendedFileAttributes.ReadOnly, pathFormat);
      }

      #endregion // IsFullPath

      /// <summary>
      ///   [AlphaFS] Opens the specified file for reading purposes bypassing security attributes.
      ///   This method is simpler to use then BackupFileStream to read only file's data stream.
      /// </summary>
      /// <param name="path">The file path to open.</param>
      /// <returns>A <see cref="FileStream"/> on the specified path, having the read-only mode and sharing options.</returns>      
      [SecurityCritical]
      public static FileStream OpenBackupRead(string path)
      {
         return OpenInternal(null, path, FileMode.Open, FileSystemRights.ReadData, FileAccess.Read, FileShare.None, ExtendedFileAttributes.BackupSemantics | ExtendedFileAttributes.SequentialScan | ExtendedFileAttributes.ReadOnly, PathFormat.ExtendedLength);
      }

      #region Transacted

      #region IsFullPath

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

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Opens the specified file for reading purposes bypassing security attributes.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file path to open.</param>
      /// <returns>A <see cref="FileStream"/> on the specified path, having the read-only mode and sharing options.</returns>
      [SecurityCritical]
      public static FileStream OpenBackupRead(KernelTransaction transaction, string path)
      {
         return OpenInternal(transaction, path, FileMode.Open, FileSystemRights.ReadData, FileAccess.Read, FileShare.None, ExtendedFileAttributes.BackupSemantics | ExtendedFileAttributes.SequentialScan | ExtendedFileAttributes.ReadOnly, PathFormat.ExtendedLength);
      }

      #endregion // Transacted

      #endregion // OpenBackupRead

      #region RemoveStream

      #region IsFullPath

      /// <summary>[AlphaFS] Removes all alternate data streams (NTFS ADS) from an existing file.</summary>
      /// <remarks>This method only removes streams of type <see cref="StreamType.AlternateData"/>.</remarks>
      /// <remarks>No Exception is thrown if the stream does not exist.</remarks>
      /// <param name="path">The path to an existing file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SecurityCritical]
      public static void RemoveStream(string path, PathFormat pathFormat)
      {
         AlternateDataStreamInfo.RemoveStreamInternal(false, null, path, null, pathFormat);
      }

      /// <summary>[AlphaFS] Removes an alternate data stream (NTFS ADS) from an existing file.</summary>
      /// <remarks>This method only removes streams of type <see cref="StreamType.AlternateData"/>.</remarks>
      /// <remarks>No Exception is thrown if the stream does not exist.</remarks>
      /// <param name="path">The path to an existing file.</param>
      /// <param name="name">The name of the stream to remove.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SecurityCritical]
      public static void RemoveStream(string path, string name, PathFormat pathFormat)
      {
         AlternateDataStreamInfo.RemoveStreamInternal(false, null, path, name, pathFormat);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Removes all alternate data streams (NTFS ADS) from an existing file.</summary>
      /// <remarks>This method only removes streams of type <see cref="StreamType.AlternateData"/>.</remarks>
      /// <remarks>No Exception is thrown if the stream does not exist.</remarks>
      /// <param name="path">The path to an existing file.</param>      
      [SecurityCritical]
      public static void RemoveStream(string path)
      {
         AlternateDataStreamInfo.RemoveStreamInternal(false, null, path, null, PathFormat.Auto);
      }

      /// <summary>[AlphaFS] Removes an alternate data stream (NTFS ADS) from an existing file.</summary>
      /// <remarks>This method only removes streams of type <see cref="StreamType.AlternateData"/>.</remarks>
      /// <remarks>No Exception is thrown if the stream does not exist.</remarks>
      /// <param name="path">The path to an existing file.</param>
      /// <param name="name">The name of the stream to remove.</param>      
      [SecurityCritical]
      public static void RemoveStream(string path, string name)
      {
         AlternateDataStreamInfo.RemoveStreamInternal(false, null, path, name, PathFormat.Auto);
      }

      #region Transacted

      #region IsFullPath

      /// <summary>[AlphaFS] Removes all alternate data streams (NTFS ADS) from an existing file.</summary>
      /// <remarks>This method only removes streams of type <see cref="StreamType.AlternateData"/>.</remarks>
      /// <remarks>No Exception is thrown if the stream does not exist.</remarks>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to an existing file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SecurityCritical]
      public static void RemoveStream(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         AlternateDataStreamInfo.RemoveStreamInternal(false, transaction, path, null, pathFormat);
      }

      /// <summary>[AlphaFS] Removes an alternate data stream (NTFS ADS) from an existing file.</summary>
      /// <remarks>This method only removes streams of type <see cref="StreamType.AlternateData"/>.</remarks>
      /// <remarks>No Exception is thrown if the stream does not exist.</remarks>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to an existing file.</param>
      /// <param name="name">The name of the stream to remove.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SecurityCritical]
      public static void RemoveStream(KernelTransaction transaction, string path, string name, PathFormat pathFormat)
      {
         AlternateDataStreamInfo.RemoveStreamInternal(false, transaction, path, name, pathFormat);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Removes all alternate data streams (NTFS ADS) from an existing file.</summary>
      /// <remarks>This method only removes streams of type <see cref="StreamType.AlternateData"/>.</remarks>
      /// <remarks>No Exception is thrown if the stream does not exist.</remarks>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to an existing file.</param>      
      [SecurityCritical]
      public static void RemoveStream(KernelTransaction transaction, string path)
      {
         AlternateDataStreamInfo.RemoveStreamInternal(false, transaction, path, null, PathFormat.Auto);
      }

      /// <summary>[AlphaFS] Removes an alternate data stream (NTFS ADS) from an existing file.</summary>
      /// <remarks>This method only removes streams of type <see cref="StreamType.AlternateData"/>.</remarks>
      /// <remarks>No Exception is thrown if the stream does not exist.</remarks>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to an existing file.</param>
      /// <param name="name">The name of the stream to remove.</param>      
      [SecurityCritical]
      public static void RemoveStream(KernelTransaction transaction, string path, string name)
      {
         AlternateDataStreamInfo.RemoveStreamInternal(false, transaction, path, name, PathFormat.Auto);
      }

      #endregion Transacted

      #endregion // RemoveStream

      #region SetTimestamps

      #region IsFullPath

      /// <summary>[AlphaFS] Sets all the date and time stamps for the specified file, at once.</summary>
      /// <param name="path">The file for which to set the dates and times information.</param>
      /// <param name="creationTime">
      ///   A <see cref="System.DateTime"/> containing the value to set for the creation date and time of <paramref name="path"/>. This value
      ///   is expressed in local time.
      /// </param>
      /// <param name="lastAccessTime">
      ///   A <see cref="System.DateTime"/> containing the value to set for the last access date and time of <paramref name="path"/>. This
      ///   value is expressed in local time.
      /// </param>
      /// <param name="lastWriteTime">
      ///   A <see cref="System.DateTime"/> containing the value to set for the last write date and time of <paramref name="path"/>. This value
      ///   is expressed in local time.
      /// </param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SecurityCritical]
      public static void SetTimestamps(string path, DateTime creationTime, DateTime lastAccessTime, DateTime lastWriteTime, PathFormat pathFormat)
      {
         SetFsoDateTimeInternal(false, null, path, creationTime.ToUniversalTime(), lastAccessTime.ToUniversalTime(), lastWriteTime.ToUniversalTime(), pathFormat);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Sets all the date and time stamps for the specified file, at once.</summary>
      /// <param name="path">The file for which to set the dates and times information.</param>
      /// <param name="creationTime">
      ///   A <see cref="System.DateTime"/> containing the value to set for the creation date and time of <paramref name="path"/>. This value
      ///   is expressed in local time.
      /// </param>
      /// <param name="lastAccessTime">
      ///   A <see cref="System.DateTime"/> containing the value to set for the last access date and time of <paramref name="path"/>. This
      ///   value is expressed in local time.
      /// </param>
      /// <param name="lastWriteTime">
      ///   A <see cref="System.DateTime"/> containing the value to set for the last write date and time of <paramref name="path"/>. This value
      ///   is expressed in local time.
      /// </param>     
      [SecurityCritical]
      public static void SetTimestamps(string path, DateTime creationTime, DateTime lastAccessTime, DateTime lastWriteTime)
      {
         SetFsoDateTimeInternal(false, null, path, creationTime.ToUniversalTime(), lastAccessTime.ToUniversalTime(), lastWriteTime.ToUniversalTime(), PathFormat.Auto);
      }

      #region Transacted

      #region IsFullPath

      /// <summary>[AlphaFS] Sets all the date and time stamps for the specified file, at once.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file for which to set the dates and times information.</param>
      /// <param name="creationTime">
      ///   A <see cref="System.DateTime"/> containing the value to set for the creation date and time of <paramref name="path"/>. This value
      ///   is expressed in local time.
      /// </param>
      /// <param name="lastAccessTime">
      ///   A <see cref="System.DateTime"/> containing the value to set for the last access date and time of <paramref name="path"/>. This
      ///   value is expressed in local time.
      /// </param>
      /// <param name="lastWriteTime">
      ///   A <see cref="System.DateTime"/> containing the value to set for the last write date and time of <paramref name="path"/>. This value
      ///   is expressed in local time.
      /// </param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SecurityCritical]
      public static void SetTimestamps(KernelTransaction transaction, string path, DateTime creationTime, DateTime lastAccessTime, DateTime lastWriteTime, PathFormat pathFormat)
      {
         SetFsoDateTimeInternal(false, transaction, path, creationTime.ToUniversalTime(), lastAccessTime.ToUniversalTime(), lastWriteTime.ToUniversalTime(), pathFormat);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Sets all the date and time stamps for the specified file, at once.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file for which to set the dates and times information.</param>
      /// <param name="creationTime">
      ///   A <see cref="System.DateTime"/> containing the value to set for the creation date and time of <paramref name="path"/>. This value
      ///   is expressed in local time.
      /// </param>
      /// <param name="lastAccessTime">
      ///   A <see cref="System.DateTime"/> containing the value to set for the last access date and time of <paramref name="path"/>. This
      ///   value is expressed in local time.
      /// </param>
      /// <param name="lastWriteTime">
      ///   A <see cref="System.DateTime"/> containing the value to set for the last write date and time of <paramref name="path"/>. This value
      ///   is expressed in local time.
      /// </param>      
      [SecurityCritical]
      public static void SetTimestamps(KernelTransaction transaction, string path, DateTime creationTime, DateTime lastAccessTime, DateTime lastWriteTime)
      {
         SetFsoDateTimeInternal(false, transaction, path, creationTime.ToUniversalTime(), lastAccessTime.ToUniversalTime(), lastWriteTime.ToUniversalTime(), PathFormat.Auto);
      }

      #endregion // Transacted

      #endregion // SetTimestamps

      #region SetTimestampsUtc

      #region IsFullPath

      /// <summary>[AlphaFS] Sets all the date and time stamps, in coordinated universal time (UTC), for the specified file, at once.</summary>
      /// <param name="path">The file for which to set the dates and times information.</param>
      /// <param name="creationTimeUtc">
      ///   A <see cref="System.DateTime"/> containing the value to set for the creation date and time of <paramref name="path"/>. This value
      ///   is expressed in UTC time.
      /// </param>
      /// <param name="lastAccessTimeUtc">
      ///   A <see cref="System.DateTime"/> containing the value to set for the last access date and time of <paramref name="path"/>. This
      ///   value is expressed in UTC time.
      /// </param>
      /// <param name="lastWriteTimeUtc">
      ///   A <see cref="System.DateTime"/> containing the value to set for the last write date and time of <paramref name="path"/>. This value
      ///   is expressed in UTC time.
      /// </param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SecurityCritical]
      public static void SetTimestampsUtc(string path, DateTime creationTimeUtc, DateTime lastAccessTimeUtc, DateTime lastWriteTimeUtc, PathFormat pathFormat)
      {
         SetFsoDateTimeInternal(false, null, path, creationTimeUtc, lastAccessTimeUtc, lastWriteTimeUtc, pathFormat);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Sets all the date and time stamps, in coordinated universal time (UTC), for the specified file, at once.</summary>
      /// <param name="path">The file for which to set the dates and times information.</param>
      /// <param name="creationTimeUtc">
      ///   A <see cref="System.DateTime"/> containing the value to set for the creation date and time of <paramref name="path"/>. This value
      ///   is expressed in UTC time.
      /// </param>
      /// <param name="lastAccessTimeUtc">
      ///   A <see cref="System.DateTime"/> containing the value to set for the last access date and time of <paramref name="path"/>. This
      ///   value is expressed in UTC time.
      /// </param>
      /// <param name="lastWriteTimeUtc">
      ///   A <see cref="System.DateTime"/> containing the value to set for the last write date and time of <paramref name="path"/>. This value
      ///   is expressed in UTC time.
      /// </param>      
      [SecurityCritical]
      public static void SetTimestampsUtc(string path, DateTime creationTimeUtc, DateTime lastAccessTimeUtc, DateTime lastWriteTimeUtc)
      {
         SetFsoDateTimeInternal(false, null, path, creationTimeUtc, lastAccessTimeUtc, lastWriteTimeUtc, PathFormat.Auto);
      }

      #region Transacted

      #region IsFullPath

      /// <summary>[AlphaFS] Sets all the date and time stamps, in coordinated universal time (UTC), for the specified file, at once.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file for which to set the dates and times information.</param>
      /// <param name="creationTimeUtc">
      ///   A <see cref="System.DateTime"/> containing the value to set for the creation date and time of <paramref name="path"/>. This value
      ///   is expressed in UTC time.
      /// </param>
      /// <param name="lastAccessTimeUtc">
      ///   A <see cref="System.DateTime"/> containing the value to set for the last access date and time of <paramref name="path"/>. This
      ///   value is expressed in UTC time.
      /// </param>
      /// <param name="lastWriteTimeUtc">
      ///   A <see cref="System.DateTime"/> containing the value to set for the last write date and time of <paramref name="path"/>. This value
      ///   is expressed in UTC time.
      /// </param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SecurityCritical]
      public static void SetTimestampsUtc(KernelTransaction transaction, string path, DateTime creationTimeUtc, DateTime lastAccessTimeUtc, DateTime lastWriteTimeUtc, PathFormat pathFormat)
      {
         SetFsoDateTimeInternal(false, transaction, path, creationTimeUtc, lastAccessTimeUtc, lastWriteTimeUtc, pathFormat);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Sets all the date and time stamps, in coordinated universal time (UTC), for the specified file, at once.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file for which to set the dates and times information.</param>
      /// <param name="creationTimeUtc">
      ///   A <see cref="System.DateTime"/> containing the value to set for the creation date and time of <paramref name="path"/>. This value
      ///   is expressed in UTC time.
      /// </param>
      /// <param name="lastAccessTimeUtc">
      ///   A <see cref="System.DateTime"/> containing the value to set for the last access date and time of <paramref name="path"/>. This
      ///   value is expressed in UTC time.
      /// </param>
      /// <param name="lastWriteTimeUtc">
      ///   A <see cref="System.DateTime"/> containing the value to set for the last write date and time of <paramref name="path"/>. This value
      ///   is expressed in UTC time.
      /// </param>      
      [SecurityCritical]
      public static void SetTimestampsUtc(KernelTransaction transaction, string path, DateTime creationTimeUtc, DateTime lastAccessTimeUtc, DateTime lastWriteTimeUtc)
      {
         SetFsoDateTimeInternal(false, transaction, path, creationTimeUtc, lastAccessTimeUtc, lastWriteTimeUtc, PathFormat.Auto);
      }

      #endregion // Transacted

      #endregion // SetTimestampsUtc

      #region TransferTimestamps

      #region IsFullPath

      /// <summary>[AlphaFS] Transfers the date and time stamps for the specified files.</summary>
      /// <remarks>This method does not change last access time for the source file.</remarks>
      /// <param name="sourcePath">The source file to get the date and time stamps from.</param>
      /// <param name="destinationPath">The destination file to set the date and time stamps.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SecurityCritical]
      public static void TransferTimestamps(string sourcePath, string destinationPath, PathFormat pathFormat)
      {
         TransferTimestampsInternal(false, null, sourcePath, destinationPath, pathFormat);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Transfers the date and time stamps for the specified files.</summary>
      /// <remarks>This method does not change last access time for the source file.</remarks>
      /// <param name="sourcePath">The source file to get the date and time stamps from.</param>
      /// <param name="destinationPath">The destination file to set the date and time stamps.</param>      
      [SecurityCritical]
      public static void TransferTimestamps(string sourcePath, string destinationPath)
      {
         TransferTimestampsInternal(false, null, sourcePath, destinationPath, PathFormat.Auto);
      }

      #region Transacted

      #region IsFullPath

      /// <summary>[AlphaFS] Transfers the date and time stamps for the specified files.</summary>
      /// <remarks>This method does not change last access time for the source file.</remarks>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourcePath">The source file to get the date and time stamps from.</param>
      /// <param name="destinationPath">The destination file to set the date and time stamps.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SecurityCritical]
      public static void TransferTimestamps(KernelTransaction transaction, string sourcePath, string destinationPath, PathFormat pathFormat)
      {
         TransferTimestampsInternal(false, transaction, sourcePath, destinationPath, pathFormat);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Transfers the date and time stamps for the specified files.</summary>
      /// <remarks>This method does not change last access time for the source file.</remarks>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourcePath">The source file to get the date and time stamps from.</param>
      /// <param name="destinationPath">The destination file to set the date and time stamps.</param>      
      [SecurityCritical]
      public static void TransferTimestamps(KernelTransaction transaction, string sourcePath, string destinationPath)
      {
         TransferTimestampsInternal(false, transaction, sourcePath, destinationPath, PathFormat.Auto);
      }

      #endregion // Transacted

      #endregion // TransferTimestamps


      #region Unified Internals

      

      #region CreateHardlinkInternal

      /// <summary>
      ///   [AlphaFS] Unified method CreateHardlinkInternal() to establish a hard link between an existing file and a new file. This function
      ///   is only supported on the NTFS file system, and only for files, not directories.
      /// </summary>
      /// <exception cref="NotSupportedException">Thrown when the requested operation is not supported.</exception>
      /// <param name="transaction">The transaction.</param>
      /// <param name="fileName">The name of the new file. This parameter cannot specify the name of a directory.</param>
      /// <param name="existingFileName">The name of the existing file. This parameter cannot specify the name of a directory.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Hardlink")]
      [SecurityCritical]
      internal static void CreateHardlinkInternal(KernelTransaction transaction, string fileName, string existingFileName, PathFormat pathFormat)
      {
         string fileNameLp = Path.GetExtendedLengthPathInternal(transaction, fileName, pathFormat, GetFullPathOptions.RemoveTrailingDirectorySeparator | GetFullPathOptions.CheckInvalidPathChars | GetFullPathOptions.CheckAdditional);

         string existingFileNameLp = Path.GetExtendedLengthPathInternal(transaction, existingFileName, pathFormat, GetFullPathOptions.RemoveTrailingDirectorySeparator | GetFullPathOptions.CheckInvalidPathChars | GetFullPathOptions.CheckAdditional);

         if (!(transaction == null || !NativeMethods.IsAtLeastWindowsVista

            // CreateHardLink() / CreateHardLinkTransacted()
            // In the ANSI version of this function, the name is limited to MAX_PATH characters.
            // To extend this limit to 32,767 wide characters, call the Unicode version of the function and prepend "\\?\" to the path.
            // 2013-01-13: MSDN does not confirm LongPath usage but a Unicode version of this function exists.

            ? NativeMethods.CreateHardLink(fileNameLp, existingFileNameLp, IntPtr.Zero)
            : NativeMethods.CreateHardLinkTransacted(fileNameLp, existingFileNameLp, IntPtr.Zero, transaction.SafeHandle)))
         {
            int lastError = Marshal.GetLastWin32Error();
            switch ((uint)lastError)
            {
               case Win32Errors.ERROR_INVALID_FUNCTION:
                  throw new NotSupportedException(Resources.HardLinksOnNonNTFSPartitionsIsNotSupported);

               default:
                  // Throws IOException.
                  NativeError.ThrowException(lastError, fileNameLp, existingFileName, true);
                  break;
            }
         }
      }

      #endregion // CreateHardlinkInternal

      #region CreateSymbolicLinkInternal

      /// <summary>[AlphaFS] Unified method CreateSymbolicLinkInternal() to create a symbolic link.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="symlinkFileName">The name of the target for the symbolic link to be created.</param>
      /// <param name="targetFileName">The symbolic link to be created.</param>
      /// <param name="targetType">Indicates whether the link target, <paramref name="targetFileName"/>, is a file or directory.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SecurityCritical]
      internal static void CreateSymbolicLinkInternal(KernelTransaction transaction, string symlinkFileName, string targetFileName, SymbolicLinkTarget targetType, PathFormat pathFormat)
      {
         string symlinkFileNameLp = Path.GetExtendedLengthPathInternal(transaction, symlinkFileName, pathFormat, GetFullPathOptions.RemoveTrailingDirectorySeparator | GetFullPathOptions.CheckInvalidPathChars | GetFullPathOptions.CheckAdditional);

         string targetFileNameLp = Path.GetExtendedLengthPathInternal(transaction, targetFileName, pathFormat, GetFullPathOptions.RemoveTrailingDirectorySeparator | GetFullPathOptions.CheckInvalidPathChars | GetFullPathOptions.CheckAdditional);

         if (!(transaction == null || !NativeMethods.IsAtLeastWindowsVista

            // CreateSymbolicLink() / CreateSymbolicLinkTransacted()
            // In the ANSI version of this function, the name is limited to MAX_PATH characters.
            // To extend this limit to 32,767 wide characters, call the Unicode version of the function and prepend "\\?\" to the path.
            // 2014-02-14: MSDN does not confirm LongPath usage but a Unicode version of this function exists.

            ? NativeMethods.CreateSymbolicLink(symlinkFileNameLp, targetFileNameLp, targetType)
            : NativeMethods.CreateSymbolicLinkTransacted(symlinkFileNameLp, targetFileNameLp, targetType, transaction.SafeHandle)))
            NativeError.ThrowException(symlinkFileNameLp, targetFileNameLp);
      }

      #endregion // CreateSymbolicLinkInternal

      

      

      

      #region EnumerateHardlinksInternal

      /// <summary>[AlphaFS] Creates an enumeration of all the hard links to the specified <paramref name="path"/>.</summary>
      /// <exception cref="PlatformNotSupportedException">Thrown when a Platform Not Supported error condition occurs.</exception>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The name of the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>An enumerable collection of <see cref="string"/> of all the hard links to the specified <paramref name="path"/></returns>
      internal static IEnumerable<string> EnumerateHardlinksInternal(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         if (!NativeMethods.IsAtLeastWindowsVista)
            throw new PlatformNotSupportedException(Resources.RequiresWindowsVistaOrHigher);

         string pathLp = Path.GetExtendedLengthPathInternal(transaction, path, pathFormat, GetFullPathOptions.RemoveTrailingDirectorySeparator | GetFullPathOptions.CheckInvalidPathChars | GetFullPathOptions.CheckAdditional);

         // Default buffer length, will be extended if needed, although this should not happen.
         uint length = NativeMethods.MaxPathUnicode;
         StringBuilder builder = new StringBuilder((int)length);


      getFindFirstFileName:

         using (SafeFindFileHandle handle = transaction == null

            // FindFirstFileName() / FindFirstFileNameTransacted()
            // In the ANSI version of this function, the name is limited to MAX_PATH characters.
            // To extend this limit to 32,767 wide characters, call the Unicode version of the function and prepend "\\?\" to the path.
            // 2013-01-13: MSDN does not confirm LongPath usage but a Unicode version of this function exists.

            ? NativeMethods.FindFirstFileName(pathLp, 0, out length, builder)
            : NativeMethods.FindFirstFileNameTransacted(pathLp, 0, out length, builder, transaction.SafeHandle))
         {
            if (handle.IsInvalid)
            {
               int lastError = Marshal.GetLastWin32Error();
               switch ((uint)lastError)
               {
                  case Win32Errors.ERROR_MORE_DATA:
                     builder = new StringBuilder((int)length);
                     handle.Close();
                     goto getFindFirstFileName;

                  default:
                     // If the function fails, the return value is INVALID_HANDLE_VALUE.
                     NativeError.ThrowException(lastError, pathLp);
                     break;
               }
            }

            yield return builder.ToString();


            //length = NativeMethods.MaxPathUnicode;
            //builder = new StringBuilder((int)length);

            do
            {
               while (!NativeMethods.FindNextFileName(handle, out length, builder))
               {
                  int lastError = Marshal.GetLastWin32Error();
                  switch ((uint)lastError)
                  {
                     // We've reached the end of the enumeration.
                     case Win32Errors.ERROR_HANDLE_EOF:
                        yield break;

                     case Win32Errors.ERROR_MORE_DATA:
                        builder = new StringBuilder((int)length);
                        continue;

                     default:
                        //If the function fails, the return value is zero (0).
                        // Throws IOException.
                        NativeError.ThrowException(lastError, true);
                        break;
                  }
               }

               yield return builder.ToString();

            } while (true);
         }
      }

      #endregion // EnumerateHardlinksInternal

      

      #region FillAttributeInfoInternal

      /// <summary>
      ///   Calls NativeMethods.GetFileAttributesEx to retrieve Win32FileAttributeData.
      ///   <para>Note that classes should use -1 as the uninitialized state for dataInitialized when relying on this method.</para>
      /// </summary>
      /// <remarks>No path (null, empty string) checking or normalization is performed.</remarks>
      /// <param name="transaction">.</param>
      /// <param name="pathLp">.</param>
      /// <param name="win32AttrData">[in,out].</param>
      /// <param name="tryagain">.</param>
      /// <param name="returnErrorOnNotFound">.</param>
      /// <returns>Returns 0 on success, otherwise a Win32 error code.</returns>
      [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
      [SecurityCritical]
      internal static int FillAttributeInfoInternal(KernelTransaction transaction, string pathLp, ref NativeMethods.Win32FileAttributeData win32AttrData, bool tryagain, bool returnErrorOnNotFound)
      {
         int dataInitialised = (int)Win32Errors.ERROR_SUCCESS;

         #region Try Again

         // Someone has a handle to the file open, or other error.
         if (tryagain)
         {
            NativeMethods.Win32FindData findData;

            // ChangeErrorMode is for the Win32 SetThreadErrorMode() method, used to suppress possible pop-ups.
            using (new NativeMethods.ChangeErrorMode(NativeMethods.ErrorMode.FailCriticalErrors))
            {
               bool error = false;

               SafeFindFileHandle handle = transaction == null || !NativeMethods.IsAtLeastWindowsVista

                  // FindFirstFileEx() / FindFirstFileTransacted()
                  // In the ANSI version of this function, the name is limited to MAX_PATH characters.
                  // To extend this limit to 32,767 wide characters, call the Unicode version of the function and prepend "\\?\" to the path.
                  // 2013-01-13: MSDN confirms LongPath usage.

                  // A trailing backslash is not allowed.
                  ? NativeMethods.FindFirstFileEx(Path.RemoveDirectorySeparator(pathLp, false), NativeMethods.FindExInfoLevel, out findData, NativeMethods.FindExSearchOps.SearchNameMatch, IntPtr.Zero, NativeMethods.LargeCache)
                  : NativeMethods.FindFirstFileTransacted(Path.RemoveDirectorySeparator(pathLp, false), NativeMethods.FindExInfoLevel, out findData, NativeMethods.FindExSearchOps.SearchNameMatch, IntPtr.Zero, NativeMethods.LargeCache, transaction.SafeHandle);

               try
               {
                  if (handle.IsInvalid)
                  {
                     error = true;
                     dataInitialised = Marshal.GetLastWin32Error();

                     if (dataInitialised == Win32Errors.ERROR_FILE_NOT_FOUND ||
                         dataInitialised == Win32Errors.ERROR_PATH_NOT_FOUND ||
                         dataInitialised == Win32Errors.ERROR_NOT_READY) // Floppy device not ready.
                     {
                        if (!returnErrorOnNotFound)
                        {
                           // Return default value for backward compatibility
                           dataInitialised = (int)Win32Errors.ERROR_SUCCESS;
                           win32AttrData.FileAttributes = (FileAttributes)(-1);
                        }
                     }

                     return dataInitialised;
                  }
               }
               finally
               {
                  try
                  {
                     // Close the Win32 handle.
                     handle.Close();
                  }
                  catch
                  {
                     // If we're already returning an error, don't throw another one.
                     if (!error)
                        NativeError.ThrowException(dataInitialised, pathLp, true);
                  }
               }
            }

            // Copy the attribute information.
            win32AttrData = new NativeMethods.Win32FileAttributeData(findData);
         }

         #endregion // Try Again

         else
         {
            using (new NativeMethods.ChangeErrorMode(NativeMethods.ErrorMode.FailCriticalErrors))
            {
               if (!(transaction == null || !NativeMethods.IsAtLeastWindowsVista

                  // GetFileAttributesEx() / GetFileAttributesTransacted()
                  // In the ANSI version of this function, the name is limited to MAX_PATH characters.
                  // To extend this limit to 32,767 wide characters, call the Unicode version of the function and prepend "\\?\" to the path.
                  // 2013-01-13: MSDN confirms LongPath usage.

                  ? NativeMethods.GetFileAttributesEx(pathLp, NativeMethods.GetFileExInfoLevels.GetFileExInfoStandard, out win32AttrData)
                  : NativeMethods.GetFileAttributesTransacted(pathLp, NativeMethods.GetFileExInfoLevels.GetFileExInfoStandard, out win32AttrData, transaction.SafeHandle)))
               {
                  dataInitialised = Marshal.GetLastWin32Error();

                  if (dataInitialised != Win32Errors.ERROR_FILE_NOT_FOUND &&
                      dataInitialised != Win32Errors.ERROR_PATH_NOT_FOUND &&
                      dataInitialised != Win32Errors.ERROR_NOT_READY) // Floppy device not ready.
                  {
                     // In case someone latched onto the file. Take the perf hit only for failure.
                     return FillAttributeInfoInternal(transaction, pathLp, ref win32AttrData, true, returnErrorOnNotFound);
                  }

                  if (!returnErrorOnNotFound)
                  {
                     // Return default value for backward compbatibility.
                     dataInitialised = (int)Win32Errors.ERROR_SUCCESS;
                     win32AttrData.FileAttributes = (FileAttributes)(-1);
                  }
               }
            }
         }

         return dataInitialised;
      }

      #endregion //FillAttributeInfoInternal

      

      #region GetCompressedSizeInternal

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
         if (pathFormat != PathFormat.ExtendedLength && Utils.IsNullOrWhiteSpace(path))
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

      #region GetChangeTimeInternal

      /// <summary>[AlphaFS] Unified method GetChangeTimeInternal() to get the change date and time of the specified file.</summary>
      /// <remarks><para>Use either <paramref name="path"/> or <paramref name="safeHandle"/>, not both.</para></remarks>
      /// <exception cref="PlatformNotSupportedException">Thrown when a Platform Not Supported error condition occurs.</exception>
      /// <exception cref="ArgumentNullException">Thrown when one or more required arguments are null.</exception>
      /// <param name="isFolder">Specifies that <paramref name="path"/> is a file or directory.</param>
      /// <param name="transaction">The transaction.</param>
      /// <param name="safeHandle">An open handle to the file or directory from which to retrieve information.</param>
      /// <param name="path">The file or directory for which to obtain creation date and time information.</param>
      /// <param name="getUtc">
      ///   <see langword="true"/> gets the Coordinated Universal Time (UTC), <see langword="false"/> gets the local time.
      /// </param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>
      ///   <para>Returns a <see cref="System.DateTime"/> structure set to the change date and time for the specified file.</para>
      ///   <para>This value is expressed in local time.</para>
      /// </returns>
      [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
      [SuppressMessage("Microsoft.Reliability", "CA2001:AvoidCallingProblematicMethods", MessageId = "System.Runtime.InteropServices.SafeHandle.DangerousGetHandle", Justification = "DangerousAddRef() and DangerousRelease() are applied.")]
      [SecurityCritical]
      internal static DateTime GetChangeTimeInternal(bool isFolder, KernelTransaction transaction, SafeFileHandle safeHandle, string path, bool getUtc, PathFormat pathFormat)
      {
         if (!NativeMethods.IsAtLeastWindowsVista)
            throw new PlatformNotSupportedException(Resources.RequiresWindowsVistaOrHigher);

         bool callerHandle = safeHandle != null;
         if (!callerHandle)
         {
            if (pathFormat != PathFormat.ExtendedLength && Utils.IsNullOrWhiteSpace(path))
               throw new ArgumentNullException("path");

            string pathLp = Path.GetExtendedLengthPathInternal(transaction, path, pathFormat, GetFullPathOptions.RemoveTrailingDirectorySeparator | GetFullPathOptions.CheckInvalidPathChars | GetFullPathOptions.CheckAdditional);

            safeHandle = CreateFileInternal(transaction, pathLp, isFolder ? ExtendedFileAttributes.BackupSemantics : ExtendedFileAttributes.Normal, null, FileMode.Open, FileSystemRights.ReadData, FileShare.ReadWrite, true, PathFormat.ExtendedLength);
         }


         try
         {
            NativeMethods.IsValidHandle(safeHandle);

            using (SafeGlobalMemoryBufferHandle safeBuffer = new SafeGlobalMemoryBufferHandle(NativeMethods.DefaultFileBufferSize))
            {
               NativeMethods.IsValidHandle(safeBuffer);

               if (!NativeMethods.GetFileInformationByHandleEx(safeHandle, NativeMethods.FileInfoByHandleClass.FileBasicInfo, safeBuffer, NativeMethods.DefaultFileBufferSize))
                  // Throws IOException.
                  NativeError.ThrowException(Marshal.GetLastWin32Error(), true);


               // CA2001:AvoidCallingProblematicMethods

               IntPtr buffer = IntPtr.Zero;
               bool successRef = false;
               safeBuffer.DangerousAddRef(ref successRef);

               // MSDN: The DangerousGetHandle method poses a security risk because it can return a handle that is not valid.
               if (successRef)
                  buffer = safeBuffer.DangerousGetHandle();

               safeBuffer.DangerousRelease();

               if (buffer == IntPtr.Zero)
                  NativeError.ThrowException(Resources.HandleDangerousRef);

               // CA2001:AvoidCallingProblematicMethods


               NativeMethods.FileTime changeTime = Utils.MarshalPtrToStructure<NativeMethods.FileBasicInfo>(0, buffer).ChangeTime;

               return getUtc
                  ? DateTime.FromFileTimeUtc(changeTime)
                  : DateTime.FromFileTime(changeTime);
            }
         }
         finally
         {
            // Handle is ours, dispose.
            if (!callerHandle && safeHandle != null)
               safeHandle.Close();
         }
      }

      #endregion // GetChangeTimeInternal

      

      #region GetEncryptionStatusInternal

      /// <summary>[AlphaFS] Unified method GetEncryptionStatusInternal() to retrieve the encryption status of the specified file.</summary>
      /// <param name="path">The name of the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>The <see cref="FileEncryptionStatus"/> of the specified <paramref name="path"/>.</returns>
      [SecurityCritical]
      internal static FileEncryptionStatus GetEncryptionStatusInternal(string path, PathFormat pathFormat)
      {
         if (pathFormat != PathFormat.ExtendedLength && Utils.IsNullOrWhiteSpace(path))
            throw new ArgumentNullException("path");

         string pathLp = Path.GetExtendedLengthPathInternal(null, path, pathFormat, GetFullPathOptions.RemoveTrailingDirectorySeparator | GetFullPathOptions.CheckInvalidPathChars | GetFullPathOptions.CheckAdditional);

         FileEncryptionStatus status;

         // FileEncryptionStatus()
         // In the ANSI version of this function, the name is limited to 248 characters.
         // To extend this limit to 32,767 wide characters, call the Unicode version of the function and prepend "\\?\" to the path.
         // 2013-01-13: MSDN does not confirm LongPath usage but a Unicode version of this function exists.

         if (!NativeMethods.FileEncryptionStatus(pathLp, out status))
            NativeError.ThrowException(Marshal.GetLastWin32Error(), pathLp);

         return status;
      }

      #endregion // GetEncryptionStatusInternal

      #region GetFileSystemEntryInfoInternal

      /// <summary>[AlphaFS] Unified method GetFileSystemEntryInfoInternal() to get a FileSystemEntryInfo from a Non-/Transacted directory/file.</summary>
      /// <returns>The <see cref="FileSystemEntryInfo"/> instance of the file or directory, or <c>null</c> on Exception when <paramref name="continueOnException"/> is <c>true</c>.</returns>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <c>null</c>.</exception>
      /// <exception cref="NativeError.ThrowException()"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to the file or directory.</param>
      /// <param name="continueOnException">
      ///    <para><c>true</c> suppress any Exception that might be thrown a result from a failure,</para>
      ///    <para>such as ACLs protected directories or non-accessible reparse points.</para>
      /// </param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      internal static FileSystemEntryInfo GetFileSystemEntryInfoInternal(KernelTransaction transaction, string path, bool continueOnException, PathFormat pathFormat)
      {
         // // Enable BasicSearch and LargeCache by default.
         var directoryEnumerationOptions = DirectoryEnumerationOptions.BasicSearch | DirectoryEnumerationOptions.LargeCache;

         if (continueOnException)
            directoryEnumerationOptions |= DirectoryEnumerationOptions.ContinueOnException;

         return (new FindFileSystemEntryInfo(false, transaction, path, Path.WildcardQuestion, directoryEnumerationOptions, typeof(FileSystemEntryInfo), pathFormat)).Get<FileSystemEntryInfo>();
      }

      #endregion // GetFileSystemEntryInfoInternal

      

      #region GetLastWriteTimeUtcInternal

      /// <summary>
      ///   [AlphaFS] Gets the date and time, in coordinated universal time (UTC) or local time, that the specified file or directory was last
      ///   written to.
      /// </summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file or directory for which to obtain write date and time information.</param>
      /// <param name="getUtc">
      ///   <see langword="true"/> gets the Coordinated Universal Time (UTC), <see langword="false"/> gets the local time.
      /// </param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>
      ///   A <see cref="System.DateTime"/> structure set to the date and time that the specified file or directory was last written to.
      ///   Depending on <paramref name="getUtc"/> this value is expressed in UTC- or local time.
      /// </returns>
      [SecurityCritical]
      internal static DateTime GetLastWriteTimeInternal(KernelTransaction transaction, string path, bool getUtc, PathFormat pathFormat)
      {
         NativeMethods.FileTime lastWriteTime = GetAttributesExInternal<NativeMethods.Win32FileAttributeData>(transaction, path, pathFormat).LastWriteTime;

         return getUtc
            ? DateTime.FromFileTimeUtc(lastWriteTime)
            : DateTime.FromFileTime(lastWriteTime);
      }

      #endregion // GetLastWriteTimeUtcInternal

      #region GetLinkTargetInfoInternal

      /// <summary>
      ///   [AlphaFS] Unified method GetLinkTargetInfoInternal() to get information about the target of a mount point or symbolic link on an
      ///   NTFS file system.
      /// </summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to the reparse point.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>
      ///   An instance of <see cref="LinkTargetInfo"/> or <see cref="SymbolicLinkTargetInfo"/> containing information about the symbolic link
      ///   or mount point pointed to by <paramref name="path"/>.
      /// </returns>
      [SecurityCritical]
      internal static LinkTargetInfo GetLinkTargetInfoInternal(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         using (SafeFileHandle safeHandle = CreateFileInternal(transaction, path, ExtendedFileAttributes.OpenReparsePoint | ExtendedFileAttributes.BackupSemantics, null, FileMode.Open, 0, FileShare.ReadWrite, true, pathFormat))
            return Device.GetLinkTargetInfoInternal(safeHandle);
      }

      #endregion // GetLinkTargetInfoInternal

      #region GetSizeInternal

      /// <summary>[AlphaFS] Unified method GetSizeInternal() to retrieve the file size, in bytes to store a specified file.</summary>
      /// <remarks>Use either <paramref name="path"/> or <paramref name="safeHandle"/>, not both.</remarks>
      /// <param name="transaction">The transaction.</param>
      /// <param name="safeHandle">The <see cref="SafeFileHandle"/> to the file.</param>
      /// <param name="path">The path to the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>The number of bytes of disk storage used to store the specified file.</returns>
      [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
      [SecurityCritical]
      internal static long GetSizeInternal(KernelTransaction transaction, SafeFileHandle safeHandle, string path, PathFormat pathFormat)
      {
         bool callerHandle = safeHandle != null;
         if (!callerHandle)
         {
            string pathLp = Path.GetExtendedLengthPathInternal(transaction, path, pathFormat, GetFullPathOptions.RemoveTrailingDirectorySeparator | GetFullPathOptions.CheckInvalidPathChars | GetFullPathOptions.CheckAdditional);

            safeHandle = CreateFileInternal(transaction, pathLp, ExtendedFileAttributes.None, null, FileMode.Open, FileSystemRights.ReadData, FileShare.Read, true, PathFormat.ExtendedLength);
         }


         long fileSize;

         try
         {
            NativeMethods.GetFileSizeEx(safeHandle, out fileSize);
         }
         finally
         {
            // Handle is ours, dispose.
            if (!callerHandle && safeHandle != null)
               safeHandle.Close();
         }

         return fileSize;
      }

      #endregion // GetSizeInternal

      #region OpenInternal

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

      #region ReadAllBytesInternal

      /// <summary>
      ///   [AlphaFS] Unified method ReadAllBytesInternal() to open a binary file, reads the contents of the file into a byte array, and then
      ///   closes the file.
      /// </summary>
      /// <exception cref="IOException">Thrown when an IO failure occurred.</exception>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to open for reading.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>A byte array containing the contents of the file.</returns>
      [SecurityCritical]
      internal static byte[] ReadAllBytesInternal(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         byte[] buffer;

         using (FileStream fs = OpenInternal(transaction, path, FileMode.Open, 0, FileAccess.Read, FileShare.Read, ExtendedFileAttributes.None, pathFormat))
         {
            int offset = 0;
            long length = fs.Length;

            if (length > int.MaxValue)
               throw new IOException(string.Format(CultureInfo.CurrentCulture, "File larger than 2GB: [{0}]", path));

            int count = (int)length;
            buffer = new byte[count];
            while (count > 0)
            {
               int n = fs.Read(buffer, offset, count);
               if (n == 0)
                  throw new IOException("Unexpected end of file found");
               offset += n;
               count -= n;
            }
         }
         return buffer;
      }

      #endregion // ReadAllBytesInternal

      #region ReadAllLinesInternal

      /// <summary>
      ///   [AlphaFS] Unified method ReadAllLinesInternal() to open a file, read all lines of the file with the specified encoding, and then
      ///   close the file.
      /// </summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to open for reading.</param>
      /// <param name="encoding">The <see cref="Encoding"/> applied to the contents of the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>An IEnumerable string containing all lines of the file.</returns>
      [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
      [SecurityCritical]
      internal static IEnumerable<string> ReadAllLinesInternal(KernelTransaction transaction, string path, Encoding encoding, PathFormat pathFormat)
      {
         using (StreamReader sr = new StreamReader(OpenInternal(transaction, path, FileMode.Open, 0, FileAccess.Read, FileShare.Read, ExtendedFileAttributes.SequentialScan, pathFormat), encoding))
         {
            string line;
            while ((line = sr.ReadLine()) != null)
               yield return line;
         }
      }

      #endregion // ReadAllLinesInternal

      #region ReadAllTextInternal

      /// <summary>
      ///   [AlphaFS] Unified method ReadAllTextInternal() to open a file, read all lines of the file with the specified encoding, and then
      ///   close the file.
      /// </summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to open for reading.</param>
      /// <param name="encoding">The <see cref="Encoding"/> applied to the contents of the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>All lines of the file.</returns>
      [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
      [SecurityCritical]
      internal static string ReadAllTextInternal(KernelTransaction transaction, string path, Encoding encoding, PathFormat pathFormat)
      {
         using (StreamReader sr = new StreamReader(OpenInternal(transaction, path, FileMode.Open, 0, FileAccess.Read, FileShare.Read, ExtendedFileAttributes.SequentialScan, pathFormat), encoding))
            return sr.ReadToEnd();
      }

      #endregion // ReadAllTextInternal

      #region ReadLinesInternal

      /// <summary>[AlphaFS] Unified method ReadLinesInternal() to read the lines of a file that has a specified encoding.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to read.</param>
      /// <param name="encoding">The encoding that is applied to the contents of the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>All the lines of the file, or the lines that are the result of a query.</returns>
      [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
      [SecurityCritical]
      internal static IEnumerable<string> ReadLinesInternal(KernelTransaction transaction, string path, Encoding encoding, PathFormat pathFormat)
      {
         using (StreamReader sr = new StreamReader(OpenInternal(transaction, path, FileMode.Open, 0, FileAccess.Read, FileShare.Read, ExtendedFileAttributes.SequentialScan, pathFormat), encoding))
         {
            string line;
            while ((line = sr.ReadLine()) != null)
               yield return line;
         }
      }

      #endregion // ReadLinesInternal

      #region ReplaceInternal

      /// <summary>
      ///   [AlphaFS] Unified method ReplaceInternal() to replace the contents of a specified file with the contents of another file, deleting
      ///   the original file, and creating a backup of the replaced file and optionally ignores merge errors.
      /// </summary>
      /// <remarks>
      ///   The Replace method replaces the contents of a specified file with the contents of another file. It also creates a backup of the
      ///   file that was replaced.
      /// </remarks>
      /// <remarks>
      ///   If the <paramref name="sourceFileName"/> and <paramref name="destinationFileName"/> are on different volumes, this method will
      ///   raise an exception. If the <paramref name="destinationBackupFileName"/> is on a different volume from the source file, the backup
      ///   file will be deleted.
      /// </remarks>
      /// <remarks>
      ///   Pass null to the <paramref name="destinationBackupFileName"/> parameter if you do not want to create a backup of the file being
      ///   replaced.
      /// </remarks>
      /// <param name="sourceFileName">The name of a file that replaces the file specified by <paramref name="destinationFileName"/>.</param>
      /// <param name="destinationFileName">The name of the file being replaced.</param>
      /// <param name="destinationBackupFileName">The name of the backup file.</param>
      /// <param name="ignoreMetadataErrors">
      ///   <see langword="true"/> to ignore merge errors (such as attributes and access control lists (ACLs)) from the replaced file to the
      ///   replacement file; otherwise, <see langword="false"/>.
      /// </param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SecurityCritical]
      internal static void ReplaceInternal(string sourceFileName, string destinationFileName, string destinationBackupFileName, bool ignoreMetadataErrors, PathFormat pathFormat)
      {
         string sourceFileNameLp = Path.GetExtendedLengthPathInternal(null, sourceFileName, pathFormat, GetFullPathOptions.RemoveTrailingDirectorySeparator | GetFullPathOptions.CheckInvalidPathChars | GetFullPathOptions.CheckAdditional);
         string destinationFileNameLp = Path.GetExtendedLengthPathInternal(null, destinationFileName, pathFormat, GetFullPathOptions.RemoveTrailingDirectorySeparator | GetFullPathOptions.CheckInvalidPathChars | GetFullPathOptions.CheckAdditional);
         // Pass null to the destinationBackupFileName parameter if you do not want to create a backup of the file being replaced.
         string destinationBackupFileNameLp = Path.GetExtendedLengthPathInternal(null, destinationBackupFileName, pathFormat, GetFullPathOptions.RemoveTrailingDirectorySeparator | GetFullPathOptions.CheckInvalidPathChars | GetFullPathOptions.CheckAdditional);

         const int replacefileWriteThrough = 1;
         const int replacefileIgnoreMergeErrors = 2;

         FileSystemRights dwReplaceFlags = (FileSystemRights)replacefileWriteThrough;
         if (ignoreMetadataErrors)
            dwReplaceFlags |= (FileSystemRights)replacefileIgnoreMergeErrors;

         // ReplaceFile()
         // In the ANSI version of this function, the name is limited to MAX_PATH characters.
         // To extend this limit to 32,767 wide characters, call the Unicode version of the function and prepend "\\?\" to the path.
         // 2013-01-13: MSDN does not confirm LongPath usage but a Unicode version of this function exists.

         if (!NativeMethods.ReplaceFile(destinationFileNameLp, sourceFileNameLp, destinationBackupFileNameLp, dwReplaceFlags, IntPtr.Zero, IntPtr.Zero))
            NativeError.ThrowException(Marshal.GetLastWin32Error(), sourceFileNameLp, destinationFileNameLp);
      }

      #endregion // ReplaceInternal

      #region SetAccessControlInternal

      /// <summary>
      ///   [AlphaFS] Unified method SetAccessControlInternal() applies access control list (ACL) entries described by a
      ///   <see cref="FileSecurity"/> FileSecurity object to the specified file.
      /// </summary>
      /// <remarks>Use either <paramref name="path"/> or <paramref name="handle"/>, not both.</remarks>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <param name="path">
      ///   A file to add or remove access control list (ACL) entries from. This parameter This parameter may be <see langword="null"/>.
      /// </param>
      /// <param name="handle">
      ///   A handle to add or remove access control list (ACL) entries from. This parameter This parameter may be <see langword="null"/>.
      /// </param>
      /// <param name="objectSecurity">
      ///   A <see cref="DirectorySecurity"/> or <see cref="FileSecurity"/> object that describes an ACL entry to apply to the file described
      ///   by the <paramref name="path"/> parameter.
      /// </param>
      /// <param name="includeSections">
      ///   One or more of the <see cref="AccessControlSections"/> values that specifies the type of access control list (ACL) information to
      ///   set.
      /// </param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      ///
      /// <exception cref="ArgumentException">
      ///   The path parameter contains invalid characters, is empty, or contains only white spaces.
      /// </exception>
      [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
      [SecurityCritical]
      internal static void SetAccessControlInternal(string path, SafeHandle handle, ObjectSecurity objectSecurity, AccessControlSections includeSections, PathFormat pathFormat)
      {
         if (pathFormat == PathFormat.Auto)
            Path.CheckValidPath(path, true, true);

         if (objectSecurity == null)
            throw new ArgumentNullException("objectSecurity");

         byte[] managedDescriptor = objectSecurity.GetSecurityDescriptorBinaryForm();
         using (SafeGlobalMemoryBufferHandle hDescriptor = new SafeGlobalMemoryBufferHandle(managedDescriptor.Length))
         {
            string pathLp = Path.GetExtendedLengthPathInternal(null, path, pathFormat, GetFullPathOptions.RemoveTrailingDirectorySeparator | GetFullPathOptions.CheckInvalidPathChars);

            hDescriptor.CopyFrom(managedDescriptor, 0, managedDescriptor.Length);

            SecurityDescriptorControl control;
            uint revision;
            if (!Security.NativeMethods.GetSecurityDescriptorControl(hDescriptor, out control, out revision))
               NativeError.ThrowException(Marshal.GetLastWin32Error(), pathLp);

            PrivilegeEnabler privilegeEnabler = null;
            try
            {
               SecurityInformation securityInfo = SecurityInformation.None;

               IntPtr pDacl = IntPtr.Zero;
               if ((includeSections & AccessControlSections.Access) != 0)
               {
                  bool daclDefaulted, daclPresent;
                  if (!Security.NativeMethods.GetSecurityDescriptorDacl(hDescriptor, out daclPresent, out pDacl, out daclDefaulted))
                     NativeError.ThrowException(Marshal.GetLastWin32Error(), pathLp);

                  if (daclPresent)
                  {
                     securityInfo |= SecurityInformation.Dacl;
                     securityInfo |= (control & SecurityDescriptorControl.DaclProtected) != 0
                        ? SecurityInformation.ProtectedDacl
                        : SecurityInformation.UnprotectedDacl;
                  }
               }

               IntPtr pSacl = IntPtr.Zero;
               if ((includeSections & AccessControlSections.Audit) != 0)
               {
                  bool saclDefaulted, saclPresent;
                  if (!Security.NativeMethods.GetSecurityDescriptorSacl(hDescriptor, out saclPresent, out pSacl, out saclDefaulted))
                     NativeError.ThrowException(Marshal.GetLastWin32Error(), pathLp);

                  if (saclPresent)
                  {
                     securityInfo |= SecurityInformation.Sacl;
                     securityInfo |= (control & SecurityDescriptorControl.SaclProtected) != 0
                        ? SecurityInformation.ProtectedSacl
                        : SecurityInformation.UnprotectedSacl;

                     privilegeEnabler = new PrivilegeEnabler(Privilege.Security);
                  }
               }

               IntPtr pOwner = IntPtr.Zero;
               if ((includeSections & AccessControlSections.Owner) != 0)
               {
                  bool ownerDefaulted;
                  if (!Security.NativeMethods.GetSecurityDescriptorOwner(hDescriptor, out pOwner, out ownerDefaulted))
                     NativeError.ThrowException(Marshal.GetLastWin32Error(), pathLp);

                  if (pOwner != IntPtr.Zero)
                     securityInfo |= SecurityInformation.Owner;
               }

               IntPtr pGroup = IntPtr.Zero;
               if ((includeSections & AccessControlSections.Group) != 0)
               {
                  bool groupDefaulted;
                  if (!Security.NativeMethods.GetSecurityDescriptorGroup(hDescriptor, out pGroup, out groupDefaulted))
                     NativeError.ThrowException(Marshal.GetLastWin32Error(), pathLp);

                  if (pGroup != IntPtr.Zero)
                     securityInfo |= SecurityInformation.Group;
               }


               uint lastError;
               if (!Utils.IsNullOrWhiteSpace(pathLp))
               {
                  // SetNamedSecurityInfo()
                  // In the ANSI version of this function, the name is limited to MAX_PATH characters.
                  // To extend this limit to 32,767 wide characters, call the Unicode version of the function and prepend "\\?\" to the path.
                  // 2013-01-13: MSDN does not confirm LongPath usage but a Unicode version of this function exists.

                  lastError = Security.NativeMethods.SetNamedSecurityInfo(pathLp, ObjectType.FileObject, securityInfo, pOwner, pGroup, pDacl, pSacl);
                  if (lastError != Win32Errors.ERROR_SUCCESS)
                     NativeError.ThrowException(lastError, pathLp);
               }
               else if (NativeMethods.IsValidHandle(handle))
               {
                  lastError = Security.NativeMethods.SetSecurityInfo(handle, ObjectType.FileObject, securityInfo, pOwner, pGroup, pDacl, pSacl);
                  if (lastError != Win32Errors.ERROR_SUCCESS)
                     NativeError.ThrowException((int)lastError);
               }
            }
            finally
            {
               if (privilegeEnabler != null)
                  privilegeEnabler.Dispose();
            }
         }
      }

      #endregion // SetAccessControlInternal

      #region SetAttributesInternal

      /// <summary>[AlphaFS] Unified method SetAttributesInternal() to set the attributes for a Non-/Transacted file/directory.</summary>
      /// <remarks>
      ///   Certain file attributes, such as <see cref="FileAttributes.Hidden"/> and <see cref="FileAttributes.ReadOnly"/>, can be combined.
      ///   Other attributes, such as <see cref="FileAttributes.Normal"/>, must be used alone.
      /// </remarks>
      /// <remarks>
      ///   It is not possible to change the <see cref="FileAttributes.Compressed"/> status of a File object using the SetAttributes method.
      /// </remarks>
      /// <exception cref="ArgumentException">Thrown when one or more arguments have unsupported or illegal values.</exception>
      /// <param name="isFolder">Specifies that <paramref name="path"/> is a file or directory.</param>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The name of the file or directory whose attributes are to be set.</param>
      /// <param name="fileAttributes">
      ///   The attributes to set for the file or directory. Note that all other values override <see cref="FileAttributes.Normal"/>.
      /// </param>
      /// <param name="continueOnNotExist">
      ///   <see langword="true"/> does not throw an Exception when the file system object does not exist.
      /// </param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SecurityCritical]
      internal static void SetAttributesInternal(bool isFolder, KernelTransaction transaction, string path, FileAttributes fileAttributes, bool continueOnNotExist, PathFormat pathFormat)
      {
         string pathLp = Path.GetExtendedLengthPathInternal(transaction, path, pathFormat, GetFullPathOptions.RemoveTrailingDirectorySeparator | GetFullPathOptions.CheckInvalidPathChars | GetFullPathOptions.CheckAdditional);

         if (!(transaction == null || !NativeMethods.IsAtLeastWindowsVista

            // SetFileAttributes()
            // In the ANSI version of this function, the name is limited to MAX_PATH characters.
            // To extend this limit to 32,767 wide characters, call the Unicode version of the function and prepend "\\?\" to the path.
            // 2013-01-13: MSDN confirms LongPath usage.

            ? NativeMethods.SetFileAttributes(pathLp, fileAttributes)
            : NativeMethods.SetFileAttributesTransacted(pathLp, fileAttributes, transaction.SafeHandle)))
         {
            if (continueOnNotExist)
               return;

            uint lastError = (uint)Marshal.GetLastWin32Error();

            switch (lastError)
            {
               // MSDN: .NET 3.5+: ArgumentException: FileSystemInfo().Attributes
               case Win32Errors.ERROR_INVALID_PARAMETER:
                  throw new ArgumentException(Resources.InvalidFileAttribute);

               case Win32Errors.ERROR_FILE_NOT_FOUND:
                  if (isFolder)
                     lastError = (int)Win32Errors.ERROR_PATH_NOT_FOUND;

                  // MSDN: .NET 3.5+: DirectoryNotFoundException: The specified path is invalid, (for example, it is on an unmapped drive).
                  // MSDN: .NET 3.5+: FileNotFoundException: The file cannot be found.
                  NativeError.ThrowException(lastError, pathLp);
                  break;
            }

            NativeError.ThrowException(lastError, pathLp);
         }
      }

      #endregion // SetAttributesInternal

      #region SetFsoDateTimeInternal

      /// <summary>
      ///   [AlphaFS] Unified method SetFsoDateTimeInternal() to set the date and time, in coordinated universal time (UTC), that the file or
      ///   directory was created and/or last accessed and/or written to.
      /// </summary>
      /// <param name="isFolder">Specifies that <paramref name="path"/> is a file or directory.</param>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file or directory for which to set the date and time information.</param>
      /// <param name="creationTimeUtc">
      ///   A <see cref="System.DateTime"/> containing the value to set for the creation date and time of <paramref name="path"/>. This value
      ///   is expressed in UTC time.
      /// </param>
      /// <param name="lastAccessTimeUtc">
      ///   A <see cref="System.DateTime"/> containing the value to set for the last access date and time of <paramref name="path"/>. This
      ///   value is expressed in UTC time.
      /// </param>
      /// <param name="lastWriteTimeUtc">
      ///   A <see cref="System.DateTime"/> containing the value to set for the last write date and time of <paramref name="path"/>. This value
      ///   is expressed in UTC time.
      /// </param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SecurityCritical]
      internal static void SetFsoDateTimeInternal(bool isFolder, KernelTransaction transaction, string path, DateTime? creationTimeUtc, DateTime? lastAccessTimeUtc, DateTime? lastWriteTimeUtc, PathFormat pathFormat)
      {
         // Because we already check here, use false for CreateFileInternal() to prevent another check.
         if (pathFormat == PathFormat.Auto)
            Path.CheckValidPath(path, false, false);

         using (SafeGlobalMemoryBufferHandle creationTime = SafeGlobalMemoryBufferHandle.CreateFromLong(creationTimeUtc.HasValue ? creationTimeUtc.Value.ToFileTimeUtc() : (long?)null))
         using (SafeGlobalMemoryBufferHandle lastAccessTime = SafeGlobalMemoryBufferHandle.CreateFromLong(lastAccessTimeUtc.HasValue ? lastAccessTimeUtc.Value.ToFileTimeUtc() : (long?)null))
         using (SafeGlobalMemoryBufferHandle lastWriteTime = SafeGlobalMemoryBufferHandle.CreateFromLong(lastWriteTimeUtc.HasValue ? lastWriteTimeUtc.Value.ToFileTimeUtc() : (long?)null))
         using (SafeFileHandle safeHandle = CreateFileInternal(transaction, path, isFolder ? ExtendedFileAttributes.BackupSemantics : ExtendedFileAttributes.Normal, null, FileMode.Open, FileSystemRights.WriteAttributes, FileShare.Delete | FileShare.Write, false, pathFormat))
            if (!NativeMethods.SetFileTime(safeHandle, creationTime, lastAccessTime, lastWriteTime))
               NativeError.ThrowException(path);
      }

      #endregion // SetFsoDateTimeInternal

      #region TransferTimestampsInternal

      /// <summary>
      ///   [AlphaFS] Unified method TransferTimestampsInternal() to transfer the date and time stamps for the specified files and directories.
      /// </summary>
      /// <remarks>This method does not change last access time for the source file.</remarks>
      /// <remarks>This method uses BackupSemantics flag to get Timestamp changed for directories.</remarks>
      /// <param name="isFolder">
      ///   Specifies that <paramref name="sourcePath"/> and <paramref name="destinationPath"/> are a file or directory.
      /// </param>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourcePath">The source path.</param>
      /// <param name="destinationPath">The destination path.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SecurityCritical]
      internal static void TransferTimestampsInternal(bool isFolder, KernelTransaction transaction, string sourcePath, string destinationPath, PathFormat pathFormat)
      {
         NativeMethods.Win32FileAttributeData attrs = GetAttributesExInternal<NativeMethods.Win32FileAttributeData>(transaction, sourcePath, pathFormat);

         SetFsoDateTimeInternal(isFolder, transaction, destinationPath, DateTime.FromFileTimeUtc(attrs.CreationTime), DateTime.FromFileTimeUtc(attrs.LastAccessTime), DateTime.FromFileTimeUtc(attrs.LastWriteTime), pathFormat);
      }

      #endregion // TransferTimestampsInternal

      #region WriteAllBytesInternal

      /// <summary>
      ///   [AlphaFS] Unified method WriteAllBytesInternal() to create a new file as part of a transaction, writes the specified byte array to
      ///   the file, and then closes the file. If the target file already exists, it is overwritten.
      /// </summary>
      /// <exception cref="ArgumentNullException">Thrown when one or more required arguments are null.</exception>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to write to.</param>
      /// <param name="bytes">The bytes to write to the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "bytes")]
      [SecurityCritical]
      internal static void WriteAllBytesInternal(KernelTransaction transaction, string path, byte[] bytes, PathFormat pathFormat)
      {
         if (bytes == null)
            throw new ArgumentNullException("bytes");

         using (FileStream fs = OpenInternal(transaction, path, FileMode.Create, 0, FileAccess.Write, FileShare.Read, ExtendedFileAttributes.None, pathFormat))
            fs.Write(bytes, 0, bytes.Length);
      }

      #endregion // WriteAllBytesInternal

      

      #endregion // Unified Internals

      #endregion // AlphaFS
   }
}