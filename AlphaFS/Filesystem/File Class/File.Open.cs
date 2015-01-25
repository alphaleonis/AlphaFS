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

using Microsoft.Win32.SafeHandles;
using System.Diagnostics.CodeAnalysis;
using System.IO;
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

      #region Non-Transactional

      /// <summary>Opens a <see cref="FileStream"/> on the specified path with read/write access.</summary>
      /// <param name="path">The file to open.</param>
      /// <param name="mode">A <see cref="FileMode"/> value that specifies whether a file is created if one does not exist, and determines whether the contents of existing files are retained or overwritten.</param>
      /// <returns>A <see cref="FileStream"/> opened in the specified mode and path, with read/write access and not shared.</returns>
      [SecurityCritical]
      public static FileStream Open(string path, FileMode mode)
      {
         return OpenInternal(null, path, mode, mode == FileMode.Append ? FileAccess.Write : FileAccess.ReadWrite, FileShare.None, ExtendedFileAttributes.Normal, null, null, PathFormat.RelativePath);
      }

      /// <summary>Opens a <see cref="FileStream"/> on the specified path, with the specified mode and access.</summary>
      /// <param name="path">The file to open.</param>
      /// <param name="mode">A <see cref="FileMode"/> value that specifies whether a file is created if one does not exist, and determines whether the contents of existing files are retained or overwritten.</param>
      /// <param name="access">A <see cref="FileAccess"/> value that specifies the operations that can be performed on the file.</param>
      /// <returns>An unshared <see cref="FileStream"/> that provides access to the specified file, with the specified mode and access.</returns>
      [SecurityCritical]
      public static FileStream Open(string path, FileMode mode, FileAccess access)
      {
         return OpenInternal(null, path, mode, access, FileShare.None, ExtendedFileAttributes.Normal, null, null, PathFormat.RelativePath);
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
         return OpenInternal(null, path, mode, access, share, ExtendedFileAttributes.Normal, null, null, PathFormat.RelativePath);
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
         return OpenInternal(null, path, mode, mode == FileMode.Append ? FileAccess.Write : FileAccess.ReadWrite, FileShare.None, ExtendedFileAttributes.Normal, null, null, pathFormat);
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
         return OpenInternal(null, path, mode, access, FileShare.None, ExtendedFileAttributes.Normal, null, null, pathFormat);
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
         return OpenInternal(null, path, mode, access, share, ExtendedFileAttributes.Normal, null, null, pathFormat);
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
      /// <param name="extendedAttributes">The extended attributes.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>
      ///   A <see cref="FileStream"/> on the specified path, having the specified mode with read, write, or read/write access and the
      ///   specified sharing option.
      /// </returns>
      [SecurityCritical]
      public static FileStream Open(string path, FileMode mode, FileAccess access, FileShare share, ExtendedFileAttributes extendedAttributes, PathFormat pathFormat)
      {
         return OpenInternal(null, path, mode, access, share, extendedAttributes, null, null, pathFormat);
      }

      // New below

      /// <summary>
      ///   Opens a <see cref="FileStream"/> on the specified path using the specified creation mode, read/write and
      ///   sharing permission, and buffer size.
      /// </summary>
      /// <param name="path">The file to open.</param>
      /// <param name="mode">A constant that determines how to open or create the file.</param>
      /// <param name="access">A <see cref="FileAccess"/> value that specifies the operations that can be performed on the
      /// file.</param>
      /// <param name="share">A constant that determines how the file will be shared by processes.</param>
      /// <param name="bufferSize">A positive <see cref="System.Int32"/> value greater than 0 indicating the buffer size. The default buffer size is 4096. </param>
      /// <returns>
      ///   A <see cref="FileStream"/> on the specified path, having the specified mode with read, write, or read/write
      ///   access and the specified sharing option.
      /// </returns>
      [SecurityCritical]
      public static FileStream Open(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize)
      {
         return OpenInternal(null, path, mode, access, share, ExtendedFileAttributes.Normal, bufferSize, null, PathFormat.RelativePath);
      }

      /// <summary>
      ///   Opens a <see cref="FileStream"/> on the specified path using the specified creation mode, read/write and
      ///   sharing permission, and buffer size.
      /// </summary>
      /// <param name="path">The file to open.</param>
      /// <param name="mode">A constant that determines how to open or create the file.</param>
      /// <param name="access">A <see cref="FileAccess"/> value that specifies the operations that can be performed on the
      /// file.</param>
      /// <param name="share">A constant that determines how the file will be shared by processes.</param>
      /// <param name="bufferSize">A positive <see cref="System.Int32"/> value greater than 0 indicating the buffer size. The
      /// default buffer size is 4096.</param>
      /// <param name="useAsync">Specifies whether to use asynchronous I/O or synchronous I/O. However, note that the
      /// underlying operating system might not support asynchronous I/O, so when specifying true, the handle might be
      /// opened synchronously depending on the platform. When opened asynchronously, the BeginRead and BeginWrite methods
      /// perform better on large reads or writes, but they might be much slower for small reads or writes. If the
      /// application is designed to take advantage of asynchronous I/O, set the useAsync parameter to true. Using
      /// asynchronous I/O correctly can speed up applications by as much as a factor of 10, but using it without
      /// redesigning the application for asynchronous I/O can decrease performance by as much as a factor of 10.</param>
      /// <returns>
      ///   A <see cref="FileStream"/> on the specified path, having the specified mode with read, write, or read/write
      ///   access and the specified sharing option.
      /// </returns>
      [SecurityCritical]
      public static FileStream Open(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize, bool useAsync)
      {
         return OpenInternal(null, path, mode, access, share, ExtendedFileAttributes.Normal | (useAsync ? ExtendedFileAttributes.Overlapped : 0), bufferSize, null, PathFormat.RelativePath);
      }

      /// <summary>
      ///   Opens a <see cref="FileStream"/> on the specified path using the specified creation mode, read/write and
      ///   sharing permission, and buffer size.
      /// </summary>
      /// <param name="path">The file to open.</param>
      /// <param name="mode">A constant that determines how to open or create the file.</param>
      /// <param name="access">A <see cref="FileAccess"/> value that specifies the operations that can be performed on the
      /// file.</param>
      /// <param name="share">A constant that determines how the file will be shared by processes.</param>
      /// <param name="bufferSize">A positive <see cref="System.Int32"/> value greater than 0 indicating the buffer size. The
      /// default buffer size is 4096.</param>
      /// <param name="options">A value that specifies additional file options.</param>
      /// <returns>
      ///   A <see cref="FileStream"/> on the specified path, having the specified mode with read, write, or read/write
      ///   access and the specified sharing option.
      /// </returns>
      [SecurityCritical]
      public static FileStream Open(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize, FileOptions options)
      {
         return OpenInternal(null, path, mode, access, share, (ExtendedFileAttributes)options, bufferSize, null, PathFormat.RelativePath);
      }

      /// <summary>
      ///   Opens a <see cref="FileStream"/> on the specified path using the specified creation mode, read/write and
      ///   sharing permission, and buffer size.
      /// </summary>
      /// <param name="path">The file to open.</param>
      /// <param name="mode">A constant that determines how to open or create the file.</param>
      /// <param name="access">A <see cref="FileAccess"/> value that specifies the operations that can be performed on the
      /// file.</param>
      /// <param name="share">A constant that determines how the file will be shared by processes.</param>
      /// <param name="bufferSize">A positive <see cref="System.Int32"/> value greater than 0 indicating the buffer size. The
      /// default buffer size is 4096.</param>
      /// <param name="extendedAttributes">The extended attributes specifying additional options.</param>
      /// <returns>
      ///   A <see cref="FileStream"/> on the specified path, having the specified mode with read, write, or read/write
      ///   access and the specified sharing option.
      /// </returns>      
      [SecurityCritical]
      public static FileStream Open(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize, ExtendedFileAttributes extendedAttributes)
      {
         return OpenInternal(null, path, mode, access, share, extendedAttributes, bufferSize, null, PathFormat.RelativePath);
      }

      /// <summary>
      ///   Opens a <see cref="FileStream"/> on the specified path using the specified creation mode, read/write and
      ///   sharing permission, and buffer size.
      /// </summary>
      /// <param name="path">The file to open.</param>
      /// <param name="mode">A constant that determines how to open or create the file.</param>
      /// <param name="rights">A <see cref="FileAccess"/> value that specifies the operations that can be performed on the
      /// file.</param>
      /// <param name="share">A constant that determines how the file will be shared by processes.</param>
      /// <param name="bufferSize">A positive <see cref="System.Int32"/> value greater than 0 indicating the buffer size. The
      /// default buffer size is 4096.</param>
      /// <param name="options">A value that specifies additional file options.</param>
      /// <returns>
      ///   A <see cref="FileStream"/> on the specified path, having the specified mode with read, write, or read/write
      ///   access and the specified sharing option.
      /// </returns>
      [SecurityCritical]
      public static FileStream Open(string path, FileMode mode, FileSystemRights rights, FileShare share, int bufferSize, FileOptions options)
      {
         return OpenInternal(null, path, mode, rights, share, (ExtendedFileAttributes)options, bufferSize, null, PathFormat.RelativePath);
      }

      /// <summary>
      ///   Opens a <see cref="FileStream"/> on the specified path using the specified creation mode, read/write and
      ///   sharing permission, and buffer size.
      /// </summary>
      /// <param name="path">The file to open.</param>
      /// <param name="mode">A constant that determines how to open or create the file.</param>
      /// <param name="rights">A <see cref="FileAccess"/> value that specifies the operations that can be performed on the
      /// file.</param>
      /// <param name="share">A constant that determines how the file will be shared by processes.</param>
      /// <param name="bufferSize">A positive <see cref="System.Int32"/> value greater than 0 indicating the buffer size. The
      /// default buffer size is 4096.</param>
      /// <param name="extendedAttributes">Extended attributes specifying additional options.</param>
      /// <returns>
      ///   A <see cref="FileStream"/> on the specified path, having the specified mode with read, write, or read/write
      ///   access and the specified sharing option.
      /// </returns>
      [SecurityCritical]
      public static FileStream Open(string path, FileMode mode, FileSystemRights rights, FileShare share, int bufferSize, ExtendedFileAttributes extendedAttributes)
      {
         return OpenInternal(null, path, mode, rights, share, extendedAttributes, bufferSize, null, PathFormat.RelativePath);
      }

      /// <summary>
      ///   Opens a <see cref="FileStream"/> on the specified path using the specified creation mode, read/write and
      ///   sharing permission, and buffer size.
      /// </summary>
      /// <param name="path">The file to open.</param>
      /// <param name="mode">A constant that determines how to open or create the file.</param>
      /// <param name="rights">A <see cref="FileAccess"/> value that specifies the operations that can be performed on the
      /// file.</param>
      /// <param name="share">A constant that determines how the file will be shared by processes.</param>
      /// <param name="bufferSize">A positive <see cref="System.Int32"/> value greater than 0 indicating the buffer size. The
      /// default buffer size is 4096.</param>
      /// <param name="options">A value that specifies additional file options.</param>
      /// <param name="security">A value that determines the access control and audit security for the file.</param>
      /// <returns>
      ///   A <see cref="FileStream"/> on the specified path, having the specified mode with read, write, or read/write
      ///   access and the specified sharing option.
      /// </returns>
      [SecurityCritical]
      public static FileStream Open(string path, FileMode mode, FileSystemRights rights, FileShare share, int bufferSize, FileOptions options, FileSecurity security)
      {
         return OpenInternal(null, path, mode, rights, share, (ExtendedFileAttributes)options, bufferSize, security, PathFormat.RelativePath);
      }

      /// <summary>
      ///   Opens a <see cref="FileStream"/> on the specified path using the specified creation mode, read/write and
      ///   sharing permission, and buffer size.
      /// </summary>
      /// <param name="path">The file to open.</param>
      /// <param name="mode">A constant that determines how to open or create the file.</param>
      /// <param name="rights">A <see cref="FileAccess"/> value that specifies the operations that can be performed on the
      /// file.</param>
      /// <param name="share">A constant that determines how the file will be shared by processes.</param>
      /// <param name="bufferSize">A positive <see cref="System.Int32"/> value greater than 0 indicating the buffer size. The
      /// default buffer size is 4096.</param>
      /// <param name="extendedAttributes">Extended attributes specifying additional options.</param>
      /// <param name="security">A value that determines the access control and audit security for the file.</param>
      /// <returns>
      ///   A <see cref="FileStream"/> on the specified path, having the specified mode with read, write, or read/write
      ///   access and the specified sharing option.
      /// </returns>
      [SecurityCritical]
      public static FileStream Open(string path, FileMode mode, FileSystemRights rights, FileShare share, int bufferSize, ExtendedFileAttributes extendedAttributes, FileSecurity security)
      {
         return OpenInternal(null, path, mode, rights, share, extendedAttributes, bufferSize, security, PathFormat.RelativePath);
      }

      /// <summary>
      ///   Opens a <see cref="FileStream"/> on the specified path using the specified creation mode, read/write and
      ///   sharing permission, and buffer size.
      /// </summary>
      /// <param name="path">The file to open.</param>
      /// <param name="mode">A constant that determines how to open or create the file.</param>
      /// <param name="access">A <see cref="FileAccess"/> value that specifies the operations that can be performed on the
      /// file.</param>
      /// <param name="share">A constant that determines how the file will be shared by processes.</param>
      /// <param name="bufferSize">A positive <see cref="System.Int32"/> value greater than 0 indicating the buffer size. The
      /// default buffer size is 4096.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter.</param>
      /// <returns>
      ///   A <see cref="FileStream"/> on the specified path, having the specified mode with read, write, or read/write
      ///   access and the specified sharing option.
      /// </returns>
      [SecurityCritical]
      public static FileStream Open(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize, PathFormat pathFormat)
      {
         return OpenInternal(null, path, mode, access, share, ExtendedFileAttributes.Normal, bufferSize, null, pathFormat);
      }

      /// <summary>
      ///   Opens a <see cref="FileStream"/> on the specified path using the specified creation mode, read/write and
      ///   sharing permission, and buffer size.
      /// </summary>
      /// <param name="path">The file to open.</param>
      /// <param name="mode">A constant that determines how to open or create the file.</param>
      /// <param name="access">A <see cref="FileAccess"/> value that specifies the operations that can be performed on the
      /// file.</param>
      /// <param name="share">A constant that determines how the file will be shared by processes.</param>
      /// <param name="bufferSize">A positive <see cref="System.Int32"/> value greater than 0 indicating the buffer size. The
      /// default buffer size is 4096.</param>
      /// <param name="useAsync">Specifies whether to use asynchronous I/O or synchronous I/O. However, note that the
      /// underlying operating system might not support asynchronous I/O, so when specifying true, the handle might be
      /// opened synchronously depending on the platform. When opened asynchronously, the BeginRead and BeginWrite methods
      /// perform better on large reads or writes, but they might be much slower for small reads or writes. If the
      /// application is designed to take advantage of asynchronous I/O, set the useAsync parameter to true. Using
      /// asynchronous I/O correctly can speed up applications by as much as a factor of 10, but using it without
      /// redesigning the application for asynchronous I/O can decrease performance by as much as a factor of 10.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter.</param>
      /// <returns>
      ///   A <see cref="FileStream"/> on the specified path, having the specified mode with read, write, or read/write
      ///   access and the specified sharing option.
      /// </returns>
      [SecurityCritical]
      public static FileStream Open(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize, bool useAsync, PathFormat pathFormat)
      {
         return OpenInternal(null, path, mode, access, share, ExtendedFileAttributes.Normal | (useAsync ? ExtendedFileAttributes.Overlapped : 0), bufferSize, null, pathFormat);
      }

      /// <summary>
      ///   Opens a <see cref="FileStream"/> on the specified path using the specified creation mode, read/write and
      ///   sharing permission, and buffer size.
      /// </summary>
      /// <param name="path">The file to open.</param>
      /// <param name="mode">A constant that determines how to open or create the file.</param>
      /// <param name="access">A <see cref="FileAccess"/> value that specifies the operations that can be performed on the
      /// file.</param>
      /// <param name="share">A constant that determines how the file will be shared by processes.</param>
      /// <param name="bufferSize">A positive <see cref="System.Int32"/> value greater than 0 indicating the buffer size. The
      /// default buffer size is 4096.</param>
      /// <param name="options">A value that specifies additional file options.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter.</param>
      /// <returns>
      ///   A <see cref="FileStream"/> on the specified path, having the specified mode with read, write, or read/write
      ///   access and the specified sharing option.
      /// </returns>
      [SecurityCritical]
      public static FileStream Open(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize, FileOptions options, PathFormat pathFormat)
      {
         return OpenInternal(null, path, mode, access, share, (ExtendedFileAttributes)options, bufferSize, null, pathFormat);
      }

      /// <summary>
      ///   Opens a <see cref="FileStream"/> on the specified path using the specified creation mode, read/write and
      ///   sharing permission, and buffer size.
      /// </summary>
      /// <param name="path">The file to open.</param>
      /// <param name="mode">A constant that determines how to open or create the file.</param>
      /// <param name="access">A <see cref="FileAccess"/> value that specifies the operations that can be performed on the
      /// file.</param>
      /// <param name="share">A constant that determines how the file will be shared by processes.</param>
      /// <param name="bufferSize">A positive <see cref="System.Int32"/> value greater than 0 indicating the buffer size. The
      /// default buffer size is 4096.</param>
      /// <param name="extendedAttributes">The extended attributes specifying additional options.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter.</param>
      /// <returns>
      ///   A <see cref="FileStream"/> on the specified path, having the specified mode with read, write, or read/write
      ///   access and the specified sharing option.
      /// </returns>
      [SecurityCritical]
      public static FileStream Open(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize, ExtendedFileAttributes extendedAttributes, PathFormat pathFormat)
      {
         return OpenInternal(null, path, mode, access, share, extendedAttributes, bufferSize, null, pathFormat);
      }

      /// <summary>
      ///   Opens a <see cref="FileStream"/> on the specified path using the specified creation mode, read/write and
      ///   sharing permission, and buffer size.
      /// </summary>
      /// <param name="path">The file to open.</param>
      /// <param name="mode">A constant that determines how to open or create the file.</param>
      /// <param name="rights">A <see cref="FileAccess"/> value that specifies the operations that can be performed on the
      /// file.</param>
      /// <param name="share">A constant that determines how the file will be shared by processes.</param>
      /// <param name="bufferSize">A positive <see cref="System.Int32"/> value greater than 0 indicating the buffer size. The
      /// default buffer size is 4096.</param>
      /// <param name="options">A value that specifies additional file options.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter.</param>
      /// <returns>
      ///   A <see cref="FileStream"/> on the specified path, having the specified mode with read, write, or read/write
      ///   access and the specified sharing option.
      /// </returns>
      [SecurityCritical]
      public static FileStream Open(string path, FileMode mode, FileSystemRights rights, FileShare share, int bufferSize, FileOptions options, PathFormat pathFormat)
      {
         return OpenInternal(null, path, mode, rights, share, (ExtendedFileAttributes)options, bufferSize, null, pathFormat);
      }

      /// <summary>
      ///   Opens a <see cref="FileStream"/> on the specified path using the specified creation mode, read/write and
      ///   sharing permission, and buffer size.
      /// </summary>
      /// <param name="path">The file to open.</param>
      /// <param name="mode">A constant that determines how to open or create the file.</param>
      /// <param name="rights">A <see cref="FileAccess"/> value that specifies the operations that can be performed on the
      /// file.</param>
      /// <param name="share">A constant that determines how the file will be shared by processes.</param>
      /// <param name="bufferSize">A positive <see cref="System.Int32"/> value greater than 0 indicating the buffer size. The
      /// default buffer size is 4096.</param>
      /// <param name="extendedAttributes">Extended attributes specifying additional options.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter.</param>
      /// <returns>
      ///   A <see cref="FileStream"/> on the specified path, having the specified mode with read, write, or read/write
      ///   access and the specified sharing option.
      /// </returns>
      [SecurityCritical]
      public static FileStream Open(string path, FileMode mode, FileSystemRights rights, FileShare share, int bufferSize, ExtendedFileAttributes extendedAttributes, PathFormat pathFormat)
      {
         return OpenInternal(null, path, mode, rights, share, extendedAttributes, bufferSize, null, pathFormat);
      }

      /// <summary>
      ///   Opens a <see cref="FileStream"/> on the specified path using the specified  creation mode, access rights and sharing permission, the buffer size, additional file options, access control and audit security.
      /// </summary>
      /// <param name="path">The file to open.</param>
      /// <param name="mode">A constant that determines how to open or create the file.</param>
      /// <param name="rights">A <see cref="FileAccess"/> value that specifies the operations that can be performed on the
      /// file.</param>
      /// <param name="share">A constant that determines how the file will be shared by processes.</param>
      /// <param name="bufferSize">A positive <see cref="System.Int32"/> value greater than 0 indicating the buffer size. The
      /// default buffer size is 4096.</param>
      /// <param name="options">A value that specifies additional file options.</param>
      /// <param name="security">A value that determines the access control and audit security for the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter.</param>
      /// <returns>
      ///   A <see cref="FileStream"/> on the specified path, having the specified mode with read, write, or read/write
      ///   access and the specified sharing option.
      /// </returns>
      [SecurityCritical]
      public static FileStream Open(string path, FileMode mode, FileSystemRights rights, FileShare share, int bufferSize, FileOptions options, FileSecurity security, PathFormat pathFormat)
      {
         return OpenInternal(null, path, mode, rights, share, (ExtendedFileAttributes)options, bufferSize, security, pathFormat);
      }

      /// <summary>
      ///   Opens a <see cref="FileStream"/> on the specified path using the specified  creation mode, access rights and
      ///   sharing permission, the buffer size, additional file options, access control and audit security.
      /// </summary>
      /// <param name="path">The file to open.</param>
      /// <param name="mode">A constant that determines how to open or create the file.</param>
      /// <param name="rights">A <see cref="FileAccess"/> value that specifies the operations that can be performed on the
      /// file.</param>
      /// <param name="share">A constant that determines how the file will be shared by processes.</param>
      /// <param name="bufferSize">A positive <see cref="System.Int32"/> value greater than 0 indicating the buffer size. The
      /// default buffer size is 4096.</param>
      /// <param name="extendedAttributes">Extended attributes specifying additional options.</param>
      /// <param name="security">A value that determines the access control and audit security for the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter.</param>
      /// <returns>
      ///   A <see cref="FileStream"/> on the specified path, having the specified mode with read, write, or read/write
      ///   access and the specified sharing option.
      /// </returns>
      [SecurityCritical]
      public static FileStream Open(string path, FileMode mode, FileSystemRights rights, FileShare share, int bufferSize, ExtendedFileAttributes extendedAttributes, FileSecurity security, PathFormat pathFormat)
      {
         return OpenInternal(null, path, mode, rights, share, extendedAttributes, bufferSize, security, pathFormat);
      }

      #endregion

      #region Transactional

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
         return OpenInternal(transaction, path, mode, mode == FileMode.Append ? FileAccess.Write : FileAccess.ReadWrite, FileShare.None, ExtendedFileAttributes.Normal, null, null, PathFormat.RelativePath);
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
         return OpenInternal(transaction, path, mode, access, FileShare.None, ExtendedFileAttributes.Normal, null, null, PathFormat.RelativePath);
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
         return OpenInternal(transaction, path, mode, access, share, ExtendedFileAttributes.Normal, null, null, PathFormat.RelativePath);
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
         return OpenInternal(transaction, path, mode, mode == FileMode.Append ? FileAccess.Write : FileAccess.ReadWrite, FileShare.None, ExtendedFileAttributes.Normal, null, null, pathFormat);
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
         return OpenInternal(transaction, path, mode, access, FileShare.None, ExtendedFileAttributes.Normal, null, null, pathFormat);
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
         return OpenInternal(transaction, path, mode, access, share, ExtendedFileAttributes.Normal, null, null, pathFormat);
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
      /// <param name="extendedAttributes">The extended attributes.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>
      ///   A <see cref="FileStream"/> on the specified path, having the specified mode with read, write, or read/write access and the
      ///   specified sharing option.
      /// </returns>
      [SecurityCritical]
      public static FileStream Open(KernelTransaction transaction, string path, FileMode mode, FileAccess access, FileShare share, ExtendedFileAttributes extendedAttributes, PathFormat pathFormat)
      {
         return OpenInternal(transaction, path, mode, access, share, extendedAttributes, null, null, pathFormat);
      }


      // New below

      /// <summary>
      ///   Opens a <see cref="FileStream"/> on the specified path using the specified creation mode, read/write and
      ///   sharing permission, and buffer size.
      /// </summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to open.</param>
      /// <param name="mode">A constant that determines how to open or create the file.</param>
      /// <param name="access">A <see cref="FileAccess"/> value that specifies the operations that can be performed on the
      /// file.</param>
      /// <param name="share">A constant that determines how the file will be shared by processes.</param>
      /// <param name="bufferSize">A positive <see cref="System.Int32"/> value greater than 0 indicating the buffer size. The
      /// default buffer size is 4096.</param>
      /// <returns>
      ///   A <see cref="FileStream"/> on the specified path, having the specified mode with read, write, or read/write
      ///   access and the specified sharing option.
      /// </returns>
      [SecurityCritical]
      public static FileStream Open(KernelTransaction transaction, string path, FileMode mode, FileAccess access, FileShare share, int bufferSize)
      {
         return OpenInternal(transaction, path, mode, access, share, ExtendedFileAttributes.Normal, bufferSize, null, PathFormat.RelativePath);
      }

      /// <summary>
      ///   Opens a <see cref="FileStream"/> on the specified path using the specified creation mode, read/write and
      ///   sharing permission, and buffer size.
      /// </summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to open.</param>
      /// <param name="mode">A constant that determines how to open or create the file.</param>
      /// <param name="access">A <see cref="FileAccess"/> value that specifies the operations that can be performed on the
      /// file.</param>
      /// <param name="share">A constant that determines how the file will be shared by processes.</param>
      /// <param name="bufferSize">A positive <see cref="System.Int32"/> value greater than 0 indicating the buffer size. The
      /// default buffer size is 4096.</param>
      /// <param name="useAsync">Specifies whether to use asynchronous I/O or synchronous I/O. However, note that the
      /// underlying operating system might not support asynchronous I/O, so when specifying true, the handle might be
      /// opened synchronously depending on the platform. When opened asynchronously, the BeginRead and BeginWrite methods
      /// perform better on large reads or writes, but they might be much slower for small reads or writes. If the
      /// application is designed to take advantage of asynchronous I/O, set the useAsync parameter to true. Using
      /// asynchronous I/O correctly can speed up applications by as much as a factor of 10, but using it without
      /// redesigning the application for asynchronous I/O can decrease performance by as much as a factor of 10.</param>
      /// <returns>
      ///   A <see cref="FileStream"/> on the specified path, having the specified mode with read, write, or read/write
      ///   access and the specified sharing option.
      /// </returns>
      [SecurityCritical]
      public static FileStream Open(KernelTransaction transaction, string path, FileMode mode, FileAccess access, FileShare share, int bufferSize, bool useAsync)
      {
         return OpenInternal(transaction, path, mode, access, share, ExtendedFileAttributes.Normal | (useAsync ? ExtendedFileAttributes.Overlapped : 0), bufferSize, null, PathFormat.RelativePath);
      }

      /// <summary>
      ///   Opens a <see cref="FileStream"/> on the specified path using the specified creation mode, read/write and
      ///   sharing permission, and buffer size.
      /// </summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to open.</param>
      /// <param name="mode">A constant that determines how to open or create the file.</param>
      /// <param name="access">A <see cref="FileAccess"/> value that specifies the operations that can be performed on the
      /// file.</param>
      /// <param name="share">A constant that determines how the file will be shared by processes.</param>
      /// <param name="bufferSize">A positive <see cref="System.Int32"/> value greater than 0 indicating the buffer size. The
      /// default buffer size is 4096.</param>
      /// <param name="options">A value that specifies additional file options.</param>
      /// <returns>
      ///   A <see cref="FileStream"/> on the specified path, having the specified mode with read, write, or read/write
      ///   access and the specified sharing option.
      /// </returns>
      [SecurityCritical]
      public static FileStream Open(KernelTransaction transaction, string path, FileMode mode, FileAccess access, FileShare share, int bufferSize, FileOptions options)
      {
         return OpenInternal(transaction, path, mode, access, share, (ExtendedFileAttributes)options, bufferSize, null, PathFormat.RelativePath);
      }

      /// <summary>
      ///   Opens a <see cref="FileStream"/> on the specified path using the specified creation mode, read/write and
      ///   sharing permission, and buffer size.
      /// </summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to open.</param>
      /// <param name="mode">A constant that determines how to open or create the file.</param>
      /// <param name="access">A <see cref="FileAccess"/> value that specifies the operations that can be performed on the
      /// file.</param>
      /// <param name="share">A constant that determines how the file will be shared by processes.</param>
      /// <param name="bufferSize">A positive <see cref="System.Int32"/> value greater than 0 indicating the buffer size. The
      /// default buffer size is 4096.</param>
      /// <param name="extendedAttributes">The extended attributes specifying additional options.</param>
      /// <returns>
      ///   A <see cref="FileStream"/> on the specified path, having the specified mode with read, write, or read/write
      ///   access and the specified sharing option.
      /// </returns>
      [SecurityCritical]
      public static FileStream Open(KernelTransaction transaction, string path, FileMode mode, FileAccess access, FileShare share, int bufferSize, ExtendedFileAttributes extendedAttributes)
      {
         return OpenInternal(transaction, path, mode, access, share, extendedAttributes, bufferSize, null, PathFormat.RelativePath);
      }

      /// <summary>
      ///   Opens a <see cref="FileStream"/> on the specified path using the specified creation mode, read/write and
      ///   sharing permission, and buffer size.
      /// </summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to open.</param>
      /// <param name="mode">A constant that determines how to open or create the file.</param>
      /// <param name="rights">A <see cref="FileAccess"/> value that specifies the operations that can be performed on the
      /// file.</param>
      /// <param name="share">A constant that determines how the file will be shared by processes.</param>
      /// <param name="bufferSize">A positive <see cref="System.Int32"/> value greater than 0 indicating the buffer size. The
      /// default buffer size is 4096.</param>
      /// <param name="options">A value that specifies additional file options.</param>
      /// <returns>
      ///   A <see cref="FileStream"/> on the specified path, having the specified mode with read, write, or read/write
      ///   access and the specified sharing option.
      /// </returns>
      [SecurityCritical]
      public static FileStream Open(KernelTransaction transaction, string path, FileMode mode, FileSystemRights rights, FileShare share, int bufferSize, FileOptions options)
      {
         return OpenInternal(transaction, path, mode, rights, share, (ExtendedFileAttributes)options, bufferSize, null, PathFormat.RelativePath);
      }

      /// <summary>
      ///   Opens a <see cref="FileStream"/> on the specified path using the specified creation mode, read/write and
      ///   sharing permission, and buffer size.
      /// </summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to open.</param>
      /// <param name="mode">A constant that determines how to open or create the file.</param>
      /// <param name="rights">A <see cref="FileAccess"/> value that specifies the operations that can be performed on the
      /// file.</param>
      /// <param name="share">A constant that determines how the file will be shared by processes.</param>
      /// <param name="bufferSize">A positive <see cref="System.Int32"/> value greater than 0 indicating the buffer size. The
      /// default buffer size is 4096.</param>
      /// <param name="extendedAttributes">Extended attributes specifying additional options.</param>
      /// <returns>
      ///   A <see cref="FileStream"/> on the specified path, having the specified mode with read, write, or read/write
      ///   access and the specified sharing option.
      /// </returns>
      [SecurityCritical]
      public static FileStream Open(KernelTransaction transaction, string path, FileMode mode, FileSystemRights rights, FileShare share, int bufferSize, ExtendedFileAttributes extendedAttributes)
      {
         return OpenInternal(transaction, path, mode, rights, share, extendedAttributes, bufferSize, null, PathFormat.RelativePath);
      }

      /// <summary>
      ///   Opens a <see cref="FileStream"/> on the specified path using the specified creation mode, read/write and
      ///   sharing permission, and buffer size.
      /// </summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to open.</param>
      /// <param name="mode">A constant that determines how to open or create the file.</param>
      /// <param name="rights">A <see cref="FileAccess"/> value that specifies the operations that can be performed on the
      /// file.</param>
      /// <param name="share">A constant that determines how the file will be shared by processes.</param>
      /// <param name="bufferSize">A positive <see cref="System.Int32"/> value greater than 0 indicating the buffer size. The
      /// default buffer size is 4096.</param>
      /// <param name="options">A value that specifies additional file options.</param>
      /// <param name="security">A value that determines the access control and audit security for the file.</param>
      /// <returns>
      ///   A <see cref="FileStream"/> on the specified path, having the specified mode with read, write, or read/write
      ///   access and the specified sharing option.
      /// </returns>
      [SecurityCritical]
      public static FileStream Open(KernelTransaction transaction, string path, FileMode mode, FileSystemRights rights, FileShare share, int bufferSize, FileOptions options, FileSecurity security)
      {
         return OpenInternal(transaction, path, mode, rights, share, (ExtendedFileAttributes)options, bufferSize, security, PathFormat.RelativePath);
      }

      /// <summary>
      ///   Opens a <see cref="FileStream"/> on the specified path using the specified creation mode, read/write and
      ///   sharing permission, and buffer size.
      /// </summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to open.</param>
      /// <param name="mode">A constant that determines how to open or create the file.</param>
      /// <param name="rights">A <see cref="FileAccess"/> value that specifies the operations that can be performed on the
      /// file.</param>
      /// <param name="share">A constant that determines how the file will be shared by processes.</param>
      /// <param name="bufferSize">A positive <see cref="System.Int32"/> value greater than 0 indicating the buffer size. The
      /// default buffer size is 4096.</param>
      /// <param name="extendedAttributes">Extended attributes specifying additional options.</param>
      /// <param name="security">A value that determines the access control and audit security for the file.</param>
      /// <returns>
      ///   A <see cref="FileStream"/> on the specified path, having the specified mode with read, write, or read/write
      ///   access and the specified sharing option.
      /// </returns>
      [SecurityCritical]
      public static FileStream Open(KernelTransaction transaction, string path, FileMode mode, FileSystemRights rights, FileShare share, int bufferSize, ExtendedFileAttributes extendedAttributes, FileSecurity security)
      {
         return OpenInternal(transaction, path, mode, rights, share, extendedAttributes, bufferSize, security, PathFormat.RelativePath);
      }

      /// <summary>
      ///   Opens a <see cref="FileStream"/> on the specified path using the specified creation mode, read/write and
      ///   sharing permission, and buffer size.
      /// </summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to open.</param>
      /// <param name="mode">A constant that determines how to open or create the file.</param>
      /// <param name="access">A <see cref="FileAccess"/> value that specifies the operations that can be performed on the
      /// file.</param>
      /// <param name="share">A constant that determines how the file will be shared by processes.</param>
      /// <param name="bufferSize">A positive <see cref="System.Int32"/> value greater than 0 indicating the buffer size. The
      /// default buffer size is 4096.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter.</param>
      /// <returns>
      ///   A <see cref="FileStream"/> on the specified path, having the specified mode with read, write, or read/write
      ///   access and the specified sharing option.
      /// </returns>
      [SecurityCritical]
      public static FileStream Open(KernelTransaction transaction, string path, FileMode mode, FileAccess access, FileShare share, int bufferSize, PathFormat pathFormat)
      {
         return OpenInternal(transaction, path, mode, access, share, ExtendedFileAttributes.Normal, bufferSize, null, pathFormat);
      }

      /// <summary>
      ///   Opens a <see cref="FileStream"/> on the specified path using the specified creation mode, read/write and
      ///   sharing permission, and buffer size.
      /// </summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to open.</param>
      /// <param name="mode">A constant that determines how to open or create the file.</param>
      /// <param name="access">A <see cref="FileAccess"/> value that specifies the operations that can be performed on the
      /// file.</param>
      /// <param name="share">A constant that determines how the file will be shared by processes.</param>
      /// <param name="bufferSize">A positive <see cref="System.Int32"/> value greater than 0 indicating the buffer size. The
      /// default buffer size is 4096.</param>
      /// <param name="useAsync">Specifies whether to use asynchronous I/O or synchronous I/O. However, note that the
      /// underlying operating system might not support asynchronous I/O, so when specifying true, the handle might be
      /// opened synchronously depending on the platform. When opened asynchronously, the BeginRead and BeginWrite methods
      /// perform better on large reads or writes, but they might be much slower for small reads or writes. If the
      /// application is designed to take advantage of asynchronous I/O, set the useAsync parameter to true. Using
      /// asynchronous I/O correctly can speed up applications by as much as a factor of 10, but using it without
      /// redesigning the application for asynchronous I/O can decrease performance by as much as a factor of 10.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter.</param>
      /// <returns>
      ///   A <see cref="FileStream"/> on the specified path, having the specified mode with read, write, or read/write
      ///   access and the specified sharing option.
      /// </returns>
      [SecurityCritical]
      public static FileStream Open(KernelTransaction transaction, string path, FileMode mode, FileAccess access, FileShare share, int bufferSize, bool useAsync, PathFormat pathFormat)
      {
         return OpenInternal(transaction, path, mode, access, share, ExtendedFileAttributes.Normal | (useAsync ? ExtendedFileAttributes.Overlapped : 0), bufferSize, null, pathFormat);
      }

      /// <summary>
      ///   Opens a <see cref="FileStream"/> on the specified path using the specified creation mode, read/write and
      ///   sharing permission, and buffer size.
      /// </summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to open.</param>
      /// <param name="mode">A constant that determines how to open or create the file.</param>
      /// <param name="access">A <see cref="FileAccess"/> value that specifies the operations that can be performed on the
      /// file.</param>
      /// <param name="share">A constant that determines how the file will be shared by processes.</param>
      /// <param name="bufferSize">A positive <see cref="System.Int32"/> value greater than 0 indicating the buffer size. The
      /// default buffer size is 4096.</param>
      /// <param name="options">A value that specifies additional file options.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter.</param>
      /// <returns>
      ///   A <see cref="FileStream"/> on the specified path, having the specified mode with read, write, or read/write
      ///   access and the specified sharing option.
      /// </returns>
      [SecurityCritical]
      public static FileStream Open(KernelTransaction transaction, string path, FileMode mode, FileAccess access, FileShare share, int bufferSize, FileOptions options, PathFormat pathFormat)
      {
         return OpenInternal(transaction, path, mode, access, share, (ExtendedFileAttributes)options, bufferSize, null, pathFormat);
      }

      /// <summary>
      ///   Opens a <see cref="FileStream"/> on the specified path using the specified creation mode, read/write and
      ///   sharing permission, and buffer size.
      /// </summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to open.</param>
      /// <param name="mode">A constant that determines how to open or create the file.</param>
      /// <param name="access">A <see cref="FileAccess"/> value that specifies the operations that can be performed on the
      /// file.</param>
      /// <param name="share">A constant that determines how the file will be shared by processes.</param>
      /// <param name="bufferSize">A positive <see cref="System.Int32"/> value greater than 0 indicating the buffer size. The
      /// default buffer size is 4096.</param>
      /// <param name="extendedAttributes">The extended attributes specifying additional options.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter.</param>
      /// <returns>
      ///   A <see cref="FileStream"/> on the specified path, having the specified mode with read, write, or read/write
      ///   access and the specified sharing option.
      /// </returns>
      [SecurityCritical]
      public static FileStream Open(KernelTransaction transaction, string path, FileMode mode, FileAccess access, FileShare share, int bufferSize, ExtendedFileAttributes extendedAttributes, PathFormat pathFormat)
      {
         return OpenInternal(transaction, path, mode, access, share, extendedAttributes, bufferSize, null, pathFormat);
      }

      /// <summary>
      ///   Opens a <see cref="FileStream"/> on the specified path using the specified creation mode, read/write and
      ///   sharing permission, and buffer size.
      /// </summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to open.</param>
      /// <param name="mode">A constant that determines how to open or create the file.</param>
      /// <param name="rights">A <see cref="FileAccess"/> value that specifies the operations that can be performed on the
      /// file.</param>
      /// <param name="share">A constant that determines how the file will be shared by processes.</param>
      /// <param name="bufferSize">A positive <see cref="System.Int32"/> value greater than 0 indicating the buffer size. The
      /// default buffer size is 4096.</param>
      /// <param name="options">A value that specifies additional file options.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter.</param>
      /// <returns>
      ///   A <see cref="FileStream"/> on the specified path, having the specified mode with read, write, or read/write
      ///   access and the specified sharing option.
      /// </returns>
      [SecurityCritical]
      public static FileStream Open(KernelTransaction transaction, string path, FileMode mode, FileSystemRights rights, FileShare share, int bufferSize, FileOptions options, PathFormat pathFormat)
      {
         return OpenInternal(transaction, path, mode, rights, share, (ExtendedFileAttributes)options, bufferSize, null, pathFormat);
      }

      /// <summary>
      ///   Opens a <see cref="FileStream"/> on the specified path using the specified creation mode, read/write and
      ///   sharing permission, and buffer size.
      /// </summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to open.</param>
      /// <param name="mode">A constant that determines how to open or create the file.</param>
      /// <param name="rights">A <see cref="FileAccess"/> value that specifies the operations that can be performed on the
      /// file.</param>
      /// <param name="share">A constant that determines how the file will be shared by processes.</param>
      /// <param name="bufferSize">A positive <see cref="System.Int32"/> value greater than 0 indicating the buffer size. The
      /// default buffer size is 4096.</param>
      /// <param name="extendedAttributes">Extended attributes specifying additional options.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter.</param>
      /// <returns>
      ///   A <see cref="FileStream"/> on the specified path, having the specified mode with read, write, or read/write
      ///   access and the specified sharing option.
      /// </returns>
      [SecurityCritical]
      public static FileStream Open(KernelTransaction transaction, string path, FileMode mode, FileSystemRights rights, FileShare share, int bufferSize, ExtendedFileAttributes extendedAttributes, PathFormat pathFormat)
      {
         return OpenInternal(transaction, path, mode, rights, share, extendedAttributes, bufferSize, null, pathFormat);
      }

      /// <summary>
      ///   Opens a <see cref="FileStream"/> on the specified path using the specified  creation mode, access rights and
      ///   sharing permission, the buffer size, additional file options, access control and audit security.
      /// </summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to open.</param>
      /// <param name="mode">A constant that determines how to open or create the file.</param>
      /// <param name="rights">A <see cref="FileAccess"/> value that specifies the operations that can be performed on the
      /// file.</param>
      /// <param name="share">A constant that determines how the file will be shared by processes.</param>
      /// <param name="bufferSize">A positive <see cref="System.Int32"/> value greater than 0 indicating the buffer size. The
      /// default buffer size is 4096.</param>
      /// <param name="options">A value that specifies additional file options.</param>
      /// <param name="security">A value that determines the access control and audit security for the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter.</param>
      /// <returns>
      ///   A <see cref="FileStream"/> on the specified path, having the specified mode with read, write, or read/write
      ///   access and the specified sharing option.
      /// </returns>
      [SecurityCritical]
      public static FileStream Open(KernelTransaction transaction, string path, FileMode mode, FileSystemRights rights, FileShare share, int bufferSize, FileOptions options, FileSecurity security, PathFormat pathFormat)
      {
         return OpenInternal(transaction, path, mode, rights, share, (ExtendedFileAttributes)options, bufferSize, security, pathFormat);
      }

      /// <summary>
      ///   Opens a <see cref="FileStream"/> on the specified path using the specified  creation mode, access rights and
      ///   sharing permission, the buffer size, additional file options, access control and audit security.
      /// </summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to open.</param>
      /// <param name="mode">A constant that determines how to open or create the file.</param>
      /// <param name="rights">A <see cref="FileAccess"/> value that specifies the operations that can be performed on the
      /// file.</param>
      /// <param name="share">A constant that determines how the file will be shared by processes.</param>
      /// <param name="bufferSize">A positive <see cref="System.Int32"/> value greater than 0 indicating the buffer size. The
      /// default buffer size is 4096.</param>
      /// <param name="extendedAttributes">Extended attributes specifying additional options.</param>
      /// <param name="security">A value that determines the access control and audit security for the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter.</param>
      /// <returns>
      ///   A <see cref="FileStream"/> on the specified path, having the specified mode with read, write, or read/write
      ///   access and the specified sharing option.
      /// </returns>
      [SecurityCritical]
      public static FileStream Open(KernelTransaction transaction, string path, FileMode mode, FileSystemRights rights, FileShare share, int bufferSize, ExtendedFileAttributes extendedAttributes, FileSecurity security, PathFormat pathFormat)
      {
         return OpenInternal(transaction, path, mode, rights, share, extendedAttributes, bufferSize, security, pathFormat);
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
         return Open(path, FileMode.Open, FileAccess.Read);            
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
         return Open(path, FileMode.Open, FileAccess.Read, pathFormat);
      }

      #region Transactional

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
         return Open(transaction, path, FileMode.Open, FileAccess.Read);
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
         return Open(transaction, path, FileMode.Open, FileAccess.Read, pathFormat);
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
         return new StreamReader(OpenRead(path), NativeMethods.DefaultFileEncoding);
      }

      /// <summary>[AlphaFS] Opens an existing NativeMethods.DefaultFileEncoding encoded text file for reading.</summary>
      /// <param name="path">The file to be opened for reading.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>A <see cref="StreamReader"/> on the specified path.</returns>
      [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
      [SecurityCritical]
      public static StreamReader OpenText(string path, PathFormat pathFormat)
      {
         return new StreamReader(OpenRead(path, pathFormat), NativeMethods.DefaultFileEncoding);
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
         return new StreamReader(OpenRead(path, pathFormat), encoding);
      }


      /// <summary>[AlphaFS] Opens an existing <see cref="Encoding"/> encoded text file for reading.</summary>
      /// <param name="path">The file to be opened for reading.</param>
      /// <param name="encoding">The <see cref="Encoding"/> applied to the contents of the file.</param>
      /// <returns>A <see cref="StreamReader"/> on the specified path.</returns>
      [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
      [SecurityCritical]
      public static StreamReader OpenText(string path, Encoding encoding)
      {
         return new StreamReader(OpenRead(path), encoding);
      }

      #region Transactional

      /// <summary>[AlphaFS] Opens an existing NativeMethods.DefaultFileEncoding encoded text file for reading.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to be opened for reading.</param>
      /// <returns>A <see cref="StreamReader"/> on the specified path.</returns>
      [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
      public static StreamReader OpenText(KernelTransaction transaction, string path)
      {
         return new StreamReader(OpenRead(transaction, path), NativeMethods.DefaultFileEncoding);
      }

      /// <summary>[AlphaFS] Opens an existing NativeMethods.DefaultFileEncoding encoded text file for reading.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to be opened for reading.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>A <see cref="StreamReader"/> on the specified path.</returns>
      [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
      public static StreamReader OpenText(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         return new StreamReader(OpenRead(transaction, path, pathFormat), NativeMethods.DefaultFileEncoding);
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
         return new StreamReader(OpenRead(transaction, path, pathFormat), encoding);
      }

      /// <summary>[AlphaFS] Opens an existing <see cref="Encoding"/> encoded text file for reading.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to be opened for reading.</param>
      /// <param name="encoding">The <see cref="Encoding"/> applied to the contents of the file.</param>
      /// <returns>A <see cref="StreamReader"/> on the specified path.</returns>
      [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
      public static StreamReader OpenText(KernelTransaction transaction, string path, Encoding encoding)
      {
         return new StreamReader(OpenRead(transaction, path), encoding);
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
         return Open(path, FileMode.OpenOrCreate, FileAccess.Write);            
      }

      /// <summary>[AlphaFS] Opens an existing file or creates a new file for writing.</summary>
      /// <param name="path">The file to be opened for writing.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>An unshared <see cref="FileStream"/> object on the specified path with <see cref="FileAccess.Write"/> access.</returns>
      [SecurityCritical]
      public static FileStream OpenWrite(string path, PathFormat pathFormat)
      {
         return Open(path, FileMode.OpenOrCreate, FileAccess.Write, pathFormat);
      }

      #region Transactional

      /// <summary>[AlphaFS] Opens an existing file or creates a new file for writing.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to be opened for writing.</param>
      /// <returns>An unshared <see cref="FileStream"/> object on the specified path with <see cref="FileAccess.Write"/> access.</returns>
      [SecurityCritical]
      public static FileStream OpenWrite(KernelTransaction transaction, string path)
      {
         return Open(transaction, path, FileMode.OpenOrCreate, FileAccess.Write);
      }

      /// <summary>[AlphaFS] Opens an existing file or creates a new file for writing.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to be opened for writing.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>An unshared <see cref="FileStream"/> object on the specified path with <see cref="FileAccess.Write"/> access.</returns>
      [SecurityCritical]
      public static FileStream OpenWrite(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         return Open(transaction, path, FileMode.OpenOrCreate, FileAccess.Write, pathFormat);
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
         return OpenInternal(null, path, FileMode.Open, FileSystemRights.ReadData, FileShare.None, ExtendedFileAttributes.BackupSemantics | ExtendedFileAttributes.SequentialScan | ExtendedFileAttributes.ReadOnly, null, null, pathFormat);
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
         return OpenInternal(null, path, FileMode.Open, FileSystemRights.ReadData, FileShare.None, ExtendedFileAttributes.BackupSemantics | ExtendedFileAttributes.SequentialScan | ExtendedFileAttributes.ReadOnly, null, null, PathFormat.RelativePath);
      }

      #region Transactional

      /// <summary>[AlphaFS] Opens the specified file for reading purposes bypassing security attributes.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file path to open.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>A <see cref="FileStream"/> on the specified path, having the read-only mode and sharing options.</returns>
      [SecurityCritical]
      public static FileStream OpenBackupRead(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         return OpenInternal(transaction, path, FileMode.Open, FileSystemRights.ReadData, FileShare.None, ExtendedFileAttributes.BackupSemantics | ExtendedFileAttributes.SequentialScan | ExtendedFileAttributes.ReadOnly, null, null, pathFormat);
      }

      /// <summary>[AlphaFS] Opens the specified file for reading purposes bypassing security attributes.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file path to open.</param>
      /// <returns>A <see cref="FileStream"/> on the specified path, having the read-only mode and sharing options.</returns>
      [SecurityCritical]
      public static FileStream OpenBackupRead(KernelTransaction transaction, string path)
      {
         return OpenInternal(transaction, path, FileMode.Open, FileSystemRights.ReadData, FileShare.None, ExtendedFileAttributes.BackupSemantics | ExtendedFileAttributes.SequentialScan | ExtendedFileAttributes.ReadOnly, null, null, PathFormat.RelativePath);
      }

      #endregion // Transacted

      #endregion // OpenBackupRead

      #region Internal Methods

      /// <summary>
      ///   [AlphaFS] Unified method OpenInternal() to open a <see cref="FileStream"/> on the specified path, having the
      ///   specified mode with
      ///   <para>read, write, or read/write access, the specified sharing option and additional options specified.</para>
      /// </summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to open.</param>
      /// <param name="mode">A <see cref="FileMode"/> value that specifies whether a file is created if one does not exist,
      /// and determines whether the contents of existing files are retained or overwritten.</param>
      /// <param name="access">A <see cref="FileAccess"/> value that specifies the operations that can be performed on the
      /// file.</param>
      /// <param name="share">A <see cref="FileShare"/> value specifying the type of access other threads have to the file.</param>
      /// <param name="attributes">Advanced <see cref="ExtendedFileAttributes"/> options for this file.</param>
      /// <param name="bufferSize">A positive <see cref="System.Int32"/> value greater than 0 indicating the buffer size. The
      /// default buffer size is 4096.</param>
      /// 
      /// <param name="security">The security.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>
      ///   <para>A <see cref="FileStream"/> instance on the specified path, having the specified mode with</para>
      ///   <para>read, write, or read/write access and the specified sharing option.</para>
      /// </returns>
      [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
      internal static FileStream OpenInternal(KernelTransaction transaction, string path, FileMode mode, FileAccess access, FileShare share, ExtendedFileAttributes attributes, int? bufferSize, FileSecurity security, PathFormat pathFormat)
      {
         FileSystemRights rights = access == FileAccess.Read ? FileSystemRights.Read : (access == FileAccess.Write ? FileSystemRights.Write : FileSystemRights.Read | FileSystemRights.Write);
         SafeFileHandle safeHandle = CreateFileInternal(transaction, path, attributes, security, mode, rights, share, true, pathFormat);
         return new FileStream(safeHandle, access, bufferSize ?? NativeMethods.DefaultFileBufferSize, (attributes & ExtendedFileAttributes.Overlapped) != 0);
      }

      /// <summary>
      ///   [AlphaFS] Unified method OpenInternal() to open a <see cref="FileStream"/> on the specified path, having the
      ///   specified mode with
      ///   <para>read, write, or read/write access, the specified sharing option and additional options specified.</para>
      /// </summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to open.</param>
      /// <param name="mode">A <see cref="FileMode"/> value that specifies whether a file is created if one does not exist,
      /// and determines whether the contents of existing files are retained or overwritten.</param>
      /// <param name="rights">A <see cref="FileSystemRights"/> value that specifies whether a file is created if one does
      /// not exist, and determines whether the contents of existing files are retained or overwritten along with additional
      /// options.</param>
      /// <param name="share">A <see cref="FileShare"/> value specifying the type of access other threads have to the file.</param>
      /// <param name="attributes">Advanced <see cref="ExtendedFileAttributes"/> options for this file.</param>
      /// <param name="bufferSize">A positive <see cref="System.Int32"/> value greater than 0 indicating the buffer size. The
      /// default buffer size is 4096.</param>
      /// 
      /// <param name="security">The security.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>
      ///   <para>A <see cref="FileStream"/> instance on the specified path, having the specified mode with</para>
      ///   <para>read, write, or read/write access and the specified sharing option.</para>
      /// </returns>
      [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
      internal static FileStream OpenInternal(KernelTransaction transaction, string path, FileMode mode, FileSystemRights rights, FileShare share, ExtendedFileAttributes attributes, int? bufferSize, FileSecurity security, PathFormat pathFormat)
      {
         FileAccess access = ((rights & FileSystemRights.ReadData) != 0 ? FileAccess.Read : 0) | (((rights & FileSystemRights.WriteData) != 0 || (rights & FileSystemRights.AppendData) != 0) ? FileAccess.Write : 0);
         SafeFileHandle safeHandle = CreateFileInternal(transaction, path, attributes, security, mode, rights, share, true, pathFormat);
         return new FileStream(safeHandle, access, bufferSize ?? NativeMethods.DefaultFileBufferSize, (attributes & ExtendedFileAttributes.Overlapped) != 0);
      }      

      #endregion // OpenInternal
   }
}
