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

using Alphaleonis.Win32.Security;
using Microsoft.Win32.SafeHandles;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.AccessControl;
using FileStream = System.IO.FileStream;

namespace Alphaleonis.Win32.Filesystem
{
   public static partial class File
   {
      #region Non-Transactional

      /// <summary>Creates or overwrites a file in the specified path.</summary>
      /// <param name="path">The path and name of the file to create.</param>
      /// <returns>A <see cref="FileStream"/> that provides read/write access to the file specified in <paramref name="path"/>.</returns>
      [SecurityCritical]
      public static FileStream Create(string path)
      {
         return CreateFileStreamCore(null, path, ExtendedFileAttributes.Normal, null, FileMode.Create, FileAccess.ReadWrite, FileShare.None, NativeMethods.DefaultFileBufferSize, PathFormat.RelativePath);
      }

      /// <summary>Creates or overwrites the specified file.</summary>
      /// <param name="path">The name of the file.</param>
      /// <param name="bufferSize">The number of bytes buffered for reads and writes to the file.</param>
      /// <returns>
      ///   A <see cref="FileStream"/> with the specified buffer size that provides read/write access to the file specified in
      ///   <paramref name="path"/>.
      /// </returns>
      [SecurityCritical]
      public static FileStream Create(string path, int bufferSize)
      {
         return CreateFileStreamCore(null, path, ExtendedFileAttributes.Normal, null, FileMode.Create, FileAccess.ReadWrite, FileShare.None, bufferSize, PathFormat.RelativePath);
      }

      /// <summary>
      ///   Creates or overwrites the specified file, specifying a buffer size and a
      ///   <see cref="FileOptions"/> value that describes how to create or overwrite the file.
      /// </summary>
      /// <param name="path">The name of the file.</param>
      /// <param name="bufferSize">The number of bytes buffered for reads and writes to the file.</param>
      /// <param name="options">One of the <see cref="FileOptions"/> values that describes how to create or overwrite the file.</param>
      /// <returns>A new file with the specified buffer size.</returns>
      [SecurityCritical]
      public static FileStream Create(string path, int bufferSize, FileOptions options)
      {
         return CreateFileStreamCore(null, path, (ExtendedFileAttributes) options, null, FileMode.Create, FileAccess.ReadWrite, FileShare.None, bufferSize, PathFormat.RelativePath);
      }

      /// <summary>
      ///   Creates or overwrites the specified file, specifying a buffer size and a
      ///   <see cref="FileOptions"/> value that describes how to create or overwrite the file.
      /// </summary>
      /// <param name="path">The name of the file.</param>
      /// <param name="bufferSize">The number of bytes buffered for reads and writes to the file.</param>
      /// <param name="options">One of the <see cref="FileOptions"/> values that describes how to create or overwrite the file.</param>
      /// <param name="fileSecurity">
      ///   One of the <see cref="FileSecurity"/> values that determines the access control and audit security for the file.
      /// </param>
      /// <returns>A new file with the specified buffer size, file options, and file security.</returns>
      [SecurityCritical]
      public static FileStream Create(string path, int bufferSize, FileOptions options, FileSecurity fileSecurity)
      {
         return CreateFileStreamCore(null, path, (ExtendedFileAttributes)options, fileSecurity, FileMode.Create, FileAccess.ReadWrite, FileShare.None, bufferSize, PathFormat.RelativePath);
      }

      /// <summary>Creates or overwrites a file in the specified path.</summary>
      /// <param name="path">The path and name of the file to create.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>
      ///   A <see cref="FileStream"/> that provides read/write access to the file specified in
      ///   <paramref name="path"/>.
      /// </returns>
      [SecurityCritical]
      public static FileStream Create(string path, PathFormat pathFormat)
      {
         return CreateFileStreamCore(null, path, ExtendedFileAttributes.Normal, null, FileMode.Create, FileAccess.ReadWrite, FileShare.None, NativeMethods.DefaultFileBufferSize, pathFormat);
      }

      /// <summary>Creates or overwrites the specified file.</summary>
      /// <param name="path">The name of the file.</param>
      /// <param name="bufferSize">The number of bytes buffered for reads and writes to the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>
      ///   A <see cref="FileStream"/> with the specified buffer size that provides read/write access to the file specified in
      ///   <paramref name="path"/>.
      /// </returns>
      [SecurityCritical]
      public static FileStream Create(string path, int bufferSize, PathFormat pathFormat)
      {
         return CreateFileStreamCore(null, path, ExtendedFileAttributes.Normal, null, FileMode.Create, FileAccess.ReadWrite, FileShare.None, bufferSize, pathFormat);
      }

      /// <summary>
      ///   Creates or overwrites the specified file, specifying a buffer size and a
      ///   <see cref="FileOptions"/> value that describes how to create or overwrite the file.
      /// </summary>
      /// <param name="path">The name of the file.</param>
      /// <param name="bufferSize">
      ///   The number of bytes buffered for reads and writes to the file.
      /// </param>
      /// <param name="options">
      ///   One of the <see cref="FileOptions"/> values that describes how to create or overwrite the
      ///   file.
      /// </param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>A new file with the specified buffer size.</returns>
      [SecurityCritical]
      public static FileStream Create(string path, int bufferSize, FileOptions options, PathFormat pathFormat)
      {
         return CreateFileStreamCore(null, path, (ExtendedFileAttributes)options, null, FileMode.Create, FileAccess.ReadWrite, FileShare.None, bufferSize, pathFormat);
      }

      /// <summary>
      ///   Creates or overwrites the specified file, specifying a buffer size and a
      ///   <see cref="FileOptions"/> value that describes how to create or overwrite the file.
      /// </summary>
      /// <param name="path">The name of the file.</param>
      /// <param name="bufferSize">
      ///   The number of bytes buffered for reads and writes to the file.
      /// </param>
      /// <param name="options">
      ///   One of the <see cref="FileOptions"/> values that describes how to create or overwrite the
      ///   file.
      /// </param>
      /// <param name="fileSecurity">
      ///   One of the <see cref="FileSecurity"/> values that determines the access control and audit
      ///   security for the file.
      /// </param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>
      ///   A new file with the specified buffer size, file options, and file security.
      /// </returns>
      [SecurityCritical]
      public static FileStream Create(string path, int bufferSize, FileOptions options, FileSecurity fileSecurity, PathFormat pathFormat)
      {
         return CreateFileStreamCore(null, path, (ExtendedFileAttributes)options, fileSecurity, FileMode.Create, FileAccess.ReadWrite, FileShare.None, bufferSize, pathFormat);
      }

      #endregion // .NET
   
      #region Transactional

      /// <summary>Creates or overwrites a file in the specified path.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path and name of the file to create.</param>
      /// <returns>
      ///   A <see cref="FileStream"/> that provides read/write access to the file specified in
      ///   <paramref name="path"/>.
      /// </returns>
      [SecurityCritical]
      public static FileStream CreateTransacted(KernelTransaction transaction, string path)
      {
         return CreateFileStreamCore(transaction, path, ExtendedFileAttributes.Normal, null, FileMode.Create, FileAccess.ReadWrite, FileShare.None, NativeMethods.DefaultFileBufferSize, PathFormat.RelativePath);
      }

      /// <summary>Creates or overwrites the specified file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The name of the file.</param>
      /// <param name="bufferSize">
      ///   The number of bytes buffered for reads and writes to the file.
      /// </param>
      /// <returns>
      ///   A <see cref="FileStream"/> with the specified buffer size that provides read/write access
      ///   to the file specified in <paramref name="path"/>.
      /// </returns>
      [SecurityCritical]
      public static FileStream CreateTransacted(KernelTransaction transaction, string path, int bufferSize)
      {
         return CreateFileStreamCore(transaction, path, ExtendedFileAttributes.Normal, null, FileMode.Create, FileAccess.ReadWrite, FileShare.None, bufferSize, PathFormat.RelativePath);
      }

      /// <summary>
      ///   Creates or overwrites the specified file, specifying a buffer size and a
      ///   <see cref="FileOptions"/> value that describes how to create or overwrite the file.
      /// </summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The name of the file.</param>
      /// <param name="bufferSize">The number of bytes buffered for reads and writes to the file.</param>
      /// <param name="options">One of the <see cref="FileOptions"/> values that describes how to create or overwrite the file.</param>
      /// <returns>A new file with the specified buffer size.</returns>
      [SecurityCritical]
      public static FileStream CreateTransacted(KernelTransaction transaction, string path, int bufferSize, FileOptions options)
      {
         return CreateFileStreamCore(transaction, path, (ExtendedFileAttributes)options, null, FileMode.Create, FileAccess.ReadWrite, FileShare.None, bufferSize, PathFormat.RelativePath);
      }

      /// <summary>
      ///   Creates or overwrites the specified file, specifying a buffer size and a
      ///   <see cref="FileOptions"/> value that describes how to create or overwrite the file.
      /// </summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The name of the file.</param>
      /// <param name="bufferSize">The number of bytes buffered for reads and writes to the file.</param>
      /// <param name="options">One of the <see cref="FileOptions"/> values that describes how to create or overwrite the file.</param>
      /// <param name="fileSecurity">
      ///   One of the <see cref="FileSecurity"/> values that determines the access control and audit security for the file.
      /// </param>
      /// <returns>A new file with the specified buffer size, file options, and file security.</returns>
      [SecurityCritical]
      public static FileStream CreateTransacted(KernelTransaction transaction, string path, int bufferSize, FileOptions options, FileSecurity fileSecurity)
      {
         return CreateFileStreamCore(transaction, path, (ExtendedFileAttributes)options, fileSecurity, FileMode.Create, FileAccess.ReadWrite, FileShare.None, bufferSize, PathFormat.RelativePath);
      }

      /// <summary>Creates or overwrites a file in the specified path.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path and name of the file to create.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>
      ///   A <see cref="FileStream"/> that provides read/write access to the file specified in
      ///   <paramref name="path"/>.
      /// </returns>
      [SecurityCritical]
      public static FileStream CreateTransacted(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         return CreateFileStreamCore(transaction, path, ExtendedFileAttributes.Normal, null, FileMode.Create, FileAccess.ReadWrite, FileShare.None, NativeMethods.DefaultFileBufferSize, pathFormat);
      }

      /// <summary>Creates or overwrites the specified file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The name of the file.</param>
      /// <param name="bufferSize">The number of bytes buffered for reads and writes to the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>
      ///   A <see cref="FileStream"/> with the specified buffer size that provides read/write access to the file specified in
      ///   <paramref name="path"/>.
      /// </returns>
      [SecurityCritical]
      public static FileStream CreateTransacted(KernelTransaction transaction, string path, int bufferSize, PathFormat pathFormat)
      {
         return CreateFileStreamCore(transaction, path, ExtendedFileAttributes.Normal, null, FileMode.Create, FileAccess.ReadWrite, FileShare.None, bufferSize, pathFormat);
      }

      /// <summary>
      ///   Creates or overwrites the specified file, specifying a buffer size and a
      ///   <see cref="FileOptions"/> value that describes how to create or overwrite the file.
      /// </summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The name of the file.</param>
      /// <param name="bufferSize">The number of bytes buffered for reads and writes to the file.</param>
      /// <param name="options">One of the <see cref="FileOptions"/> values that describes how to create or overwrite the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>A new file with the specified buffer size.</returns>
      [SecurityCritical]
      public static FileStream CreateTransacted(KernelTransaction transaction, string path, int bufferSize, FileOptions options, PathFormat pathFormat)
      {
         return CreateFileStreamCore(transaction, path, (ExtendedFileAttributes)options, null, FileMode.Create, FileAccess.ReadWrite, FileShare.None, bufferSize, pathFormat);
      }

      /// <summary>
      ///   Creates or overwrites the specified file, specifying a buffer size and a
      ///   <see cref="FileOptions"/> value that describes how to create or overwrite the file.
      /// </summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The name of the file.</param>
      /// <param name="bufferSize">The number of bytes buffered for reads and writes to the file.</param>
      /// <param name="options">One of the <see cref="FileOptions"/> values that describes how to create or overwrite the file.</param>
      /// <param name="fileSecurity">
      ///   One of the <see cref="FileSecurity"/> values that determines the access control and audit security for the file.
      /// </param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>A new file with the specified buffer size, file options, and file security.</returns>
      [SecurityCritical]
      public static FileStream CreateTransacted(KernelTransaction transaction, string path, int bufferSize, FileOptions options, FileSecurity fileSecurity, PathFormat pathFormat)
      {
         return CreateFileStreamCore(transaction, path, (ExtendedFileAttributes)options, fileSecurity, FileMode.Create, FileAccess.ReadWrite, FileShare.None, bufferSize, pathFormat);
      }

      #endregion

      #region Internal

      /// <summary>Creates or overwrites a file in the specified path.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The name of the file.</param>
      /// <param name="attributes">The <see cref="ExtendedFileAttributes"/> additional advanced options to create a file.</param>
      /// <param name="fileSecurity">
      ///   A <see cref="FileSecurity"/> instance that determines the access control and audit security for the file.
      /// </param>
      /// <param name="mode">The <see cref="FileMode"/> option gives you more precise control over how you want to create a file.</param>
      /// <param name="access">
      ///   The <see cref="FileAccess"/> allow you additionally specify to default read/write capability - just write, bypassing any cache.
      /// </param>
      /// <param name="share">
      ///   The <see cref="FileShare"/> option controls how you would like to share created file with other requesters.
      /// </param>
      ///  <param name="pathFormat">Indicates the format of the <paramref name="path"/> parameter.</param>
      /// <param name="bufferSize">The number of bytes buffered for reads and writes to the file.</param>
      /// <returns>A <see cref="FileStream"/> that provides read/write access to the file specified in path.</returns>      
      [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "False positive")]
      [SecurityCritical]
      internal static FileStream CreateFileStreamCore(KernelTransaction transaction, string path, ExtendedFileAttributes attributes, FileSecurity fileSecurity, FileMode mode, FileAccess access, FileShare share, int bufferSize, PathFormat pathFormat)
      {
         SafeFileHandle safeHandle = null;

         try
         {
            safeHandle = CreateFileCore(transaction, path, attributes, fileSecurity, mode, (FileSystemRights) access, share, true, pathFormat);

            return new FileStream(safeHandle, access, bufferSize, (attributes & ExtendedFileAttributes.Overlapped) != 0);
         }
         catch
         {
            if (safeHandle != null)
               safeHandle.Dispose();

            throw;
         }
      }

      /// <summary>Creates or opens a file, directory or I/O device.</summary>
      /// <returns>A <see cref="SafeFileHandle"/> that provides read/write access to the file or directory specified by <paramref name="path"/>.</returns>
      /// <remarks>
      ///   <para>To obtain a directory handle using CreateFile, specify the FILE_FLAG_BACKUP_SEMANTICS flag as part of dwFlagsAndAttributes.</para>
      ///   <para>The most commonly used I/O devices are as follows: file, file stream, directory, physical disk, volume, console buffer, tape drive, communications resource, mailslot, and pipe.</para>
      /// </remarks>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="NotSupportedException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path and name of the file or directory to create.</param>
      /// <param name="attributes">One of the <see cref="ExtendedFileAttributes"/> values that describes how to create or overwrite the file or directory.</param>
      /// <param name="fileSecurity">A <see cref="FileSecurity"/> instance that determines the access control and audit security for the file or directory.</param>
      /// <param name="fileMode">A <see cref="FileMode"/> constant that determines how to open or create the file or directory.</param>
      /// <param name="fileSystemRights">A <see cref="FileSystemRights"/> constant that determines the access rights to use when creating access and audit rules for the file or directory.</param>
      /// <param name="fileShare">A <see cref="FileShare"/> constant that determines how the file or directory will be shared by processes.</param>
      /// <param name="checkPath">.</param>
      /// <param name="pathFormat">Indicates the format of the <paramref name="path"/> parameter.</param>      
      [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Object needs to be disposed by caller.")]
      [SecurityCritical]
      internal static SafeFileHandle CreateFileCore(KernelTransaction transaction, string path, ExtendedFileAttributes attributes, FileSecurity fileSecurity, FileMode fileMode, FileSystemRights fileSystemRights, FileShare fileShare, bool checkPath, PathFormat pathFormat)
      {
         if (checkPath && pathFormat == PathFormat.RelativePath)
            Path.CheckSupportedPathFormat(path, true, true);

         // When isFile == null, we're working with a device.
         // When opening a VOLUME or removable media drive (for example, a floppy disk drive or flash memory thumb drive),
         // the path string should be the following form: "\\.\X:"
         // Do not use a trailing backslash (\), which indicates the root.

         var pathLp = Path.GetExtendedLengthPathCore(transaction, path, pathFormat, GetFullPathOptions.TrimEnd | GetFullPathOptions.RemoveTrailingDirectorySeparator);

         PrivilegeEnabler privilegeEnabler = null;


         var isAppend = fileMode == FileMode.Append;


         // CreateFileXxx() does not support FileMode.Append mode.
         if (isAppend)
         {
            fileMode = FileMode.OpenOrCreate;
            fileSystemRights &= FileSystemRights.AppendData; // Add right.
         }


         if (fileSecurity != null)
            fileSystemRights |= (FileSystemRights) 0x1000000; // Set right.

         // AccessSystemSecurity = 0x1000000    AccessSystemAcl access type.
         // MaximumAllowed       = 0x2000000    MaximumAllowed  access type.

         if ((fileSystemRights & (FileSystemRights) 0x1000000) != 0 ||
             (fileSystemRights & (FileSystemRights) 0x2000000) != 0)
            privilegeEnabler = new PrivilegeEnabler(Privilege.Security);


         using (privilegeEnabler)
         using (var securityAttributes = new Security.NativeMethods.SecurityAttributes(fileSecurity))
         {
            var handle = transaction == null || !NativeMethods.IsAtLeastWindowsVista

               // CreateFile() / CreateFileTransacted()
               // In the ANSI version of this function, the name is limited to MAX_PATH characters.
               // To extend this limit to 32,767 wide characters, call the Unicode version of the function and prepend "\\?\" to the path.
               // 2013-01-13: MSDN confirms LongPath usage.

               ? NativeMethods.CreateFile(pathLp, fileSystemRights, fileShare, securityAttributes, fileMode, attributes, IntPtr.Zero)
               : NativeMethods.CreateFileTransacted(pathLp, fileSystemRights, fileShare, securityAttributes, fileMode, attributes, IntPtr.Zero, transaction.SafeHandle, IntPtr.Zero, IntPtr.Zero);

            var lastError = Marshal.GetLastWin32Error();

            if (handle.IsInvalid)
            {
               handle.Close();
               NativeError.ThrowException(lastError, pathLp);
            }


            if (isAppend)
            {
               var stream = new FileStream(handle, FileAccess.Write, NativeMethods.DefaultFileBufferSize, (attributes & ExtendedFileAttributes.Overlapped) != 0);
               stream.Seek(0, SeekOrigin.End);
            }

            return handle;
         }
      }

      #endregion // CreateFileCore
   }
}
