/* Copyright (c) 2008-2015 Peter Palotas, Jeffrey Jangli, Normalex
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

using System.Collections.Generic;
using Alphaleonis.Win32.Security;
using Microsoft.Win32.SafeHandles;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.AccessControl;
using SecurityNativeMethods = Alphaleonis.Win32.Security.NativeMethods;

namespace Alphaleonis.Win32.Filesystem
{
   /// <summary>The <see cref="BackupFileStream"/> provides access to data associated with a specific file or directory, including security information and alternative data streams, for backup and restore operations.</summary>
   /// <remarks>This class uses the <see href="http://msdn.microsoft.com/en-us/library/aa362509(VS.85).aspx">BackupRead</see>, 
   /// <see href="http://msdn.microsoft.com/en-us/library/aa362510(VS.85).aspx">BackupSeek</see> and 
   /// <see href="http://msdn.microsoft.com/en-us/library/aa362511(VS.85).aspx">BackupWrite</see> functions from the Win32 API to provide access to the file or directory.
   /// </remarks>
   public class BackupFileStream : Stream
   {
      #region Construction and Destruction

      #region File

      /// <summary>Initializes a new instance of the <see cref="BackupFileStream"/> class with the specified path and creation mode.</summary>
      /// <param name="path">A relative or absolute path for the file that the current <see cref="BackupFileStream"/> object will encapsulate.</param>
      /// <param name="mode">A <see cref="FileMode"/> constant that determines how to open or create the file.</param>
      /// <remarks>The file will be opened for exclusive access for both reading and writing.</remarks>
      [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
      [SecurityCritical]
      public BackupFileStream(string path, FileMode mode)
         : this(File.CreateFileInternal(null, path, ExtendedFileAttributes.Normal, null, mode, FileSystemRights.Read | FileSystemRights.Write, FileShare.None, true, false), FileSystemRights.Read | FileSystemRights.Write)
      {
      }

      /// <summary>Initializes a new instance of the <see cref="BackupFileStream"/> class with the specified path, creation mode and access rights.</summary>
      /// <param name="path">A relative or absolute path for the file that the current <see cref="BackupFileStream"/> object will encapsulate.</param>
      /// <param name="mode">A <see cref="FileMode"/> constant that determines how to open or create the file.</param>
      /// <param name="access">A <see cref="FileSystemRights"/> constant that determines the access rights to use when creating access and audit rules for the file.</param>
      /// <remarks>The file will be opened for exclusive access.</remarks>
      [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
      [SecurityCritical]
      public BackupFileStream(string path, FileMode mode, FileSystemRights access)
         : this(File.CreateFileInternal(null, path, ExtendedFileAttributes.Normal, null, mode, access, FileShare.None, true, false), access)
      {
      }

      /// <summary>Initializes a new instance of the <see cref="BackupFileStream"/> class with the specified path, creation mode, access rights and sharing permission.</summary>
      /// <param name="path">A relative or absolute path for the file that the current <see cref="BackupFileStream"/> object will encapsulate.</param>
      /// <param name="mode">A <see cref="FileMode"/> constant that determines how to open or create the file.</param>
      /// <param name="access">A <see cref="FileSystemRights"/> constant that determines the access rights to use when creating access and audit rules for the file.</param>
      /// <param name="share">A <see cref="FileShare"/> constant that determines how the file will be shared by processes.</param>
      [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
      [SecurityCritical]
      public BackupFileStream(string path, FileMode mode, FileSystemRights access, FileShare share)
         : this(File.CreateFileInternal(null, path, ExtendedFileAttributes.Normal, null, mode, access, share, true, false), access)
      {
      }

      /// <summary>Initializes a new instance of the <see cref="BackupFileStream"/> class with the specified path, creation mode, access rights and sharing permission, and additional file attributes.</summary>
      /// <param name="path">A relative or absolute path for the file that the current <see cref="BackupFileStream"/> object will encapsulate.</param>
      /// <param name="mode">A <see cref="FileMode"/> constant that determines how to open or create the file.</param>
      /// <param name="access">A <see cref="FileSystemRights"/> constant that determines the access rights to use when creating access and audit rules for the file.</param>
      /// <param name="share">A <see cref="FileShare"/> constant that determines how the file will be shared by processes.</param>
      /// <param name="attributes">A <see cref="ExtendedFileAttributes"/> constant that specifies additional file attributes.</param>
      [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
      [SecurityCritical]
      public BackupFileStream(string path, FileMode mode, FileSystemRights access, FileShare share, ExtendedFileAttributes attributes)
         : this(File.CreateFileInternal(null, path, attributes, null, mode, access, share, true, false), access)
      {
      }

      /// <summary>Initializes a new instance of the <see cref="BackupFileStream"/> class with the specified path, creation mode, access rights and sharing permission, additional file attributes, access control and audit security.</summary>
      /// <param name="path">A relative or absolute path for the file that the current <see cref="BackupFileStream"/> object will encapsulate.</param>
      /// <param name="mode">A <see cref="FileMode"/> constant that determines how to open or create the file.</param>
      /// <param name="access">A <see cref="FileSystemRights"/> constant that determines the access rights to use when creating access and audit rules for the file.</param>
      /// <param name="share">A <see cref="FileShare"/> constant that determines how the file will be shared by processes.</param>
      /// <param name="attributes">A <see cref="ExtendedFileAttributes"/> constant that specifies additional file attributes.</param>
      /// <param name="security">A <see cref="FileSecurity"/> constant that determines the access control and audit security for the file. This parameter This parameter may be <see langword="null"/>.</param>
      [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
      [SecurityCritical]
      public BackupFileStream(string path, FileMode mode, FileSystemRights access, FileShare share, ExtendedFileAttributes attributes, FileSecurity security)
         : this(File.CreateFileInternal(null, path, attributes, security, mode, access, share, true, false), access)
      {
      }

      #region Transacted

      /// <summary>Initializes a new instance of the <see cref="BackupFileStream"/> class with the specified path and creation mode.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">A relative or absolute path for the file that the current <see cref="BackupFileStream"/> object will encapsulate.</param>
      /// <param name="mode">A <see cref="FileMode"/> constant that determines how to open or create the file.</param>
      /// <remarks>The file will be opened for exclusive access for both reading and writing.</remarks>
      [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
      [SecurityCritical]
      public BackupFileStream(KernelTransaction transaction, string path, FileMode mode)
         : this(File.CreateFileInternal(transaction, path, ExtendedFileAttributes.Normal, null, mode, FileSystemRights.Read | FileSystemRights.Write, FileShare.None, true, false), FileSystemRights.Read | FileSystemRights.Write)
      {
      }

      /// <summary>Initializes a new instance of the <see cref="BackupFileStream"/> class with the specified path, creation mode and access rights.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">A relative or absolute path for the file that the current <see cref="BackupFileStream"/> object will encapsulate.</param>
      /// <param name="mode">A <see cref="FileMode"/> constant that determines how to open or create the file.</param>
      /// <param name="access">A <see cref="FileSystemRights"/> constant that determines the access rights to use when creating access and audit rules for the file.</param>
      /// <remarks>The file will be opened for exclusive access.</remarks>
      [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
      [SecurityCritical]
      public BackupFileStream(KernelTransaction transaction, string path, FileMode mode, FileSystemRights access)
         : this(File.CreateFileInternal(transaction, path, ExtendedFileAttributes.Normal, null, mode, access, FileShare.None, true, false), access)
      {
      }

      /// <summary>Initializes a new instance of the <see cref="BackupFileStream"/> class with the specified path, creation mode, access rights and sharing permission.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">A relative or absolute path for the file that the current <see cref="BackupFileStream"/> object will encapsulate.</param>
      /// <param name="mode">A <see cref="FileMode"/> constant that determines how to open or create the file.</param>
      /// <param name="access">A <see cref="FileSystemRights"/> constant that determines the access rights to use when creating access and audit rules for the file.</param>
      /// <param name="share">A <see cref="FileShare"/> constant that determines how the file will be shared by processes.</param>
      [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
      [SecurityCritical]
      public BackupFileStream(KernelTransaction transaction, string path, FileMode mode, FileSystemRights access, FileShare share)
         : this(File.CreateFileInternal(transaction, path, ExtendedFileAttributes.Normal, null, mode, access, share, true, false), access)
      {
      }

      /// <summary>Initializes a new instance of the <see cref="BackupFileStream"/> class with the specified path, creation mode, access rights and sharing permission, and additional file attributes.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">A relative or absolute path for the file that the current <see cref="BackupFileStream"/> object will encapsulate.</param>
      /// <param name="mode">A <see cref="FileMode"/> constant that determines how to open or create the file.</param>
      /// <param name="access">A <see cref="FileSystemRights"/> constant that determines the access rights to use when creating access and audit rules for the file.</param>
      /// <param name="share">A <see cref="FileShare"/> constant that determines how the file will be shared by processes.</param>
      /// <param name="attributes">A <see cref="ExtendedFileAttributes"/> constant that specifies additional file attributes.</param>
      [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
      [SecurityCritical]
      public BackupFileStream(KernelTransaction transaction, string path, FileMode mode, FileSystemRights access, FileShare share, ExtendedFileAttributes attributes)
         : this(File.CreateFileInternal(transaction, path, attributes, null, mode, access, share, true, false), access)
      {
      }

      /// <summary>Initializes a new instance of the <see cref="BackupFileStream"/> class with the specified path, creation mode, access rights and sharing permission, additional file attributes, access control and audit security.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">A relative or absolute path for the file that the current <see cref="BackupFileStream"/> object will encapsulate.</param>
      /// <param name="mode">A <see cref="FileMode"/> constant that determines how to open or create the file.</param>
      /// <param name="access">A <see cref="FileSystemRights"/> constant that determines the access rights to use when creating access and audit rules for the file.</param>
      /// <param name="share">A <see cref="FileShare"/> constant that determines how the file will be shared by processes.</param>
      /// <param name="attributes">A <see cref="ExtendedFileAttributes"/> constant that specifies additional file attributes.</param>
      /// <param name="security">A <see cref="FileSecurity"/> constant that determines the access control and audit security for the file. This parameter This parameter may be <see langword="null"/>.</param>
      [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
      [SecurityCritical]
      public BackupFileStream(KernelTransaction transaction, string path, FileMode mode, FileSystemRights access, FileShare share, ExtendedFileAttributes attributes, FileSecurity security)
         : this(File.CreateFileInternal(transaction, path, attributes, security, mode, access, share, true, false), access)
      {
      }

      #endregion //Transacted

      #endregion // File

      #region Stream

      /// <summary>Initializes a new instance of the <see cref="BackupFileStream"/> class for the specified file handle, with the specified read/write permission.</summary>
      /// <param name="handle">A file handle for the file that this <see cref="BackupFileStream"/> object will encapsulate.</param>
      /// <param name="access">A <see cref="FileSystemRights"/> constant that gets the <see cref="CanRead"/> and <see cref="CanWrite"/> properties of the <see cref="BackupFileStream"/> object.</param>
      
      [SecurityCritical]
      public BackupFileStream(SafeFileHandle handle, FileSystemRights access)
      {
         _safeFileHandle = handle;
         _canRead = (access & FileSystemRights.ReadData) != 0;
         _canWrite = (access & FileSystemRights.WriteData) != 0;
      }

      #endregion // Stream

      #region Dispose

      /// <summary>Releases unmanaged resources and performs other cleanup operations before the <see cref="BackupFileStream"/> is reclaimed by garbage collection.</summary>
      ~BackupFileStream()
      {
         Dispose(false);
      }

      #endregion // Dispose

      #endregion // Construction and Destruction

      #region Methods

      #region Dispose

      /// <summary>Releases the unmanaged resources used by the <see cref="System.IO.Stream"/> and optionally releases the managed resources.</summary>
      /// <param name="disposing"><see langword="true"/> to release both managed and unmanaged resources; <see langword="false"/> to release only unmanaged resources.</param>
      [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
      protected override void Dispose(bool disposing)
      {
         // If one of the constructors previously threw an exception,
         // than the object hasn't been initialized properly and call from finalize will fail.

         if (!SafeFileHandle.IsInvalid)
         {
            if (_context != IntPtr.Zero)
            {
               uint temp;

               // MSDN: To release the memory used by the data structure, call BackupRead with the bAbort parameter set to TRUE when the backup operation is complete.
               if (!NativeMethods.BackupRead(SafeFileHandle, new SafeGlobalMemoryBufferHandle(), 0, out temp, true, false, out _context))
                  NativeError.ThrowException(Marshal.GetLastWin32Error());

               _context = IntPtr.Zero;
            }

            SafeFileHandle.Close();
         }

         base.Dispose(disposing);
      }

      #endregion // Dispose

      #region Read / Write

      #region Read

      /// <summary>Reads a sequence of bytes from the current stream and advances the position within the stream by the number of bytes read.</summary>
      /// <param name="buffer">An array of bytes. When this method returns, the buffer contains the specified byte array with the values
      /// between <paramref name="offset"/> and (<paramref name="offset"/> + <paramref name="count"/> - 1) replaced by the bytes read from the current source.</param>
      /// <param name="offset">The zero-based byte offset in <paramref name="buffer"/> at which to begin storing the data read from the current stream.</param>
      /// <param name="count">The maximum number of bytes to be read from the current stream.</param>
      /// <returns>
      /// The total number of bytes read into the buffer. This can be less than the number of bytes requested if that many bytes are not
      /// currently available, or zero (0) if the end of the stream has been reached.
      /// </returns>
      /// <exception cref="System.ArgumentException">The sum of <paramref name="offset"/> and <paramref name="count"/> is larger than the buffer length.</exception>
      /// <exception cref="System.ArgumentNullException">
      /// 	<paramref name="buffer"/> is <see langword="null"/>.</exception>
      /// <exception cref="System.ArgumentOutOfRangeException">
      /// 	<paramref name="offset"/> or <paramref name="count"/> is negative.</exception>
      /// <exception cref="System.NotSupportedException">The stream does not support reading.</exception>
      /// <exception cref="System.ObjectDisposedException">Methods were called after the stream was closed.</exception>
      /// <remarks>This method will not backup the access-control list (ACL) data for the file or directory.</remarks>      
      public override int Read(byte[] buffer, int offset, int count)
      {
         return Read(buffer, offset, count, false);
      }

      /// <summary>When overridden in a derived class, reads a sequence of bytes from the current stream and advances the position within the stream by the number of bytes read.</summary>
      /// <param name="buffer">An array of bytes. When this method returns, the buffer contains the specified byte array with the values
      /// between <paramref name="offset"/> and (<paramref name="offset"/> + <paramref name="count"/> - 1) replaced by the bytes read from the current source.</param>
      /// <param name="offset">The zero-based byte offset in <paramref name="buffer"/> at which to begin storing the data read from the current stream.</param>
      /// <param name="count">The maximum number of bytes to be read from the current stream.</param>
      /// <param name="processSecurity">Indicates whether the function will backup the access-control list (ACL) data for the file or directory.</param>
      /// <returns>
      /// The total number of bytes read into the buffer. This can be less than the number of bytes requested if that many bytes are not
      /// currently available, or zero (0) if the end of the stream has been reached.
      /// </returns>
      /// <exception cref="System.ArgumentException">The sum of <paramref name="offset"/> and <paramref name="count"/> is larger than the buffer length.</exception>
      /// <exception cref="System.ArgumentNullException">
      /// 	<paramref name="buffer"/> is <see langword="null"/>.</exception>
      /// <exception cref="System.ArgumentOutOfRangeException">
      /// 	<paramref name="offset"/> or <paramref name="count"/> is negative.</exception>
      /// <exception cref="System.NotSupportedException">The stream does not support reading.</exception>
      /// <exception cref="System.ObjectDisposedException">Methods were called after the stream was closed.</exception>
      [SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands")]
      [SecurityCritical]
      public int Read(byte[] buffer, int offset, int count, bool processSecurity)
      {
         if (!CanRead)
            throw new NotSupportedException("Stream does not support reading");

         if (buffer == null)
            throw new ArgumentNullException("buffer");

         if (offset + count > buffer.Length)
            throw new ArgumentException("The sum of offset and count is larger than the size of the buffer.");

         if (offset < 0)
            throw new ArgumentOutOfRangeException("offset", offset, Resources.OffsetMustNotBeNegative);

         if (count < 0)
            throw new ArgumentOutOfRangeException("count", count, Resources.CountMustNotBeNegative);

         using (SafeGlobalMemoryBufferHandle safeBuffer = new SafeGlobalMemoryBufferHandle(count))
         {
            uint numberOfBytesRead;

            if (!NativeMethods.BackupRead(SafeFileHandle, safeBuffer, (uint) safeBuffer.Capacity, out numberOfBytesRead, false, processSecurity, out _context))
               // Throws IOException.
               NativeError.ThrowException(Marshal.GetLastWin32Error(), true);

            // See File.GetAccessControlInternal(): .CopyTo() does not work there?
            safeBuffer.CopyTo(buffer, offset, count);

            return (int) numberOfBytesRead;
         }
      }

      #endregion // Read

      #region Write

      /// <summary>Writes a sequence of bytes to the current stream and advances the current position within this stream by the number of bytes written.</summary>
      /// <overloads>
      /// Writes a sequence of bytes to the current stream and advances the current position within this stream by the number of bytes written.
      /// </overloads>
      /// <param name="buffer">An array of bytes. This method copies <paramref name="count"/> bytes from <paramref name="buffer"/> to the current stream.</param>
      /// <param name="offset">The zero-based byte offset in <paramref name="buffer"/> at which to begin copying bytes to the current stream.</param>
      /// <param name="count">The number of bytes to be written to the current stream.</param>
      /// <exception cref="System.ArgumentException">The sum of <paramref name="offset"/> and <paramref name="count"/> is greater than the buffer length.</exception>
      /// <exception cref="System.ArgumentNullException">
      /// 	<paramref name="buffer"/> is <see langword="null"/>.</exception>
      /// <exception cref="System.ArgumentOutOfRangeException">
      /// 	<paramref name="offset"/> or <paramref name="count"/> is negative.</exception>
      /// <exception cref="System.NotSupportedException">The stream does not support writing.</exception>
      /// <exception cref="System.ObjectDisposedException">Methods were called after the stream was closed.</exception>
      /// <remarks>This method will not process the access-control list (ACL) data for the file or directory.</remarks>      
      public override void Write(byte[] buffer, int offset, int count)
      {
         Write(buffer, offset, count, false);
      }

      /// <summary>When overridden in a derived class, writes a sequence of bytes to the current stream and advances the current position within this stream by the number of bytes written.</summary>
      /// <param name="buffer">An array of bytes. This method copies <paramref name="count"/> bytes from <paramref name="buffer"/> to the current stream.</param>
      /// <param name="offset">The zero-based byte offset in <paramref name="buffer"/> at which to begin copying bytes to the current stream.</param>
      /// <param name="count">The number of bytes to be written to the current stream.</param>
      /// <param name="processSecurity">Specifies whether the function will restore the access-control list (ACL) data for the file or directory. 
      /// If this is <see langword="true"/> you need to specify <see cref="FileSystemRights.TakeOwnership"/> and <see cref="FileSystemRights.ChangePermissions"/> access when 
      /// opening the file or directory handle. If the handle does not have those access rights, the operating system denies 
      /// access to the ACL data, and ACL data restoration will not occur.</param>
      /// <exception cref="System.ArgumentException">The sum of <paramref name="offset"/> and <paramref name="count"/> is greater than the buffer length.</exception>
      /// <exception cref="System.ArgumentNullException">
      /// 	<paramref name="buffer"/> is <see langword="null"/>.</exception>
      /// <exception cref="System.ArgumentOutOfRangeException">
      /// 	<paramref name="offset"/> or <paramref name="count"/> is negative.</exception>
      /// <exception cref="System.NotSupportedException">The stream does not support writing.</exception>
      /// <exception cref="System.ObjectDisposedException">Methods were called after the stream was closed.</exception>
      [SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands")]
      [SecurityCritical]
      public void Write(byte[] buffer, int offset, int count, bool processSecurity)
      {
         if (buffer == null)
            throw new ArgumentNullException("buffer");

         if (offset < 0)
            throw new ArgumentOutOfRangeException("offset", offset, Resources.OffsetMustNotBeNegative);

         if (count < 0)
            throw new ArgumentOutOfRangeException("count", count, Resources.CountMustNotBeNegative);

         if (offset + count > buffer.Length)
            throw new ArgumentException(Resources.BufferIsNotLargeEnoughForTheRequestedOperation);

         using (SafeGlobalMemoryBufferHandle safeBuffer = new SafeGlobalMemoryBufferHandle(count))
         {
            safeBuffer.CopyFrom(buffer, offset, count);

            uint bytesWritten;

            if (!NativeMethods.BackupWrite(SafeFileHandle, safeBuffer, (uint) safeBuffer.Capacity, out bytesWritten, false, processSecurity, out _context))
               // Throws IOException.
               NativeError.ThrowException(Marshal.GetLastWin32Error(), true);
         }         
      }

      #endregion // Write

      #region Flush

      /// <summary>Clears all buffers for this stream and causes any buffered data to be written to the underlying device.</summary>
      public override void Flush()
      {
         if (!NativeMethods.FlushFileBuffers(SafeFileHandle))
            // Throws IOException.
            NativeError.ThrowException(Marshal.GetLastWin32Error(), true);
      }

      #endregion // Flush

      #endregion // Read / Write

      #region Navigate

      #region Seek

      /// <summary>When overridden in a derived class, sets the position within the current stream.</summary>
      /// <param name="offset">A byte offset relative to the <paramref name="origin"/> parameter.</param>
      /// <param name="origin">A value of type <see cref="System.IO.SeekOrigin"/> indicating the reference point used to obtain the new position.</param>
      /// <returns>The new position within the current stream.</returns>
      /// <remarks><para><note><para>This stream does not support seeking using this method, and calling this method will always throw <see cref="NotSupportedException"/>. See <see cref="Skip"/> for an alternative way of seeking forward.</para></note></para></remarks>
      /// <exception cref="System.NotSupportedException">The stream does not support seeking.</exception>
      public override long Seek(long offset, SeekOrigin origin)
      {
         throw new NotSupportedException();
      }

      #endregion // Seek

      #region SetLength

      /// <summary>When overridden in a derived class, sets the length of the current stream.</summary>
      /// <param name="value">The desired length of the current stream in bytes.</param>
      /// <remarks>This method is not supported by the <see cref="BackupFileStream"/> class, and calling it will always generate a <see cref="NotSupportedException"/>.</remarks>
      /// <exception cref="System.NotSupportedException">Always thrown by this class.</exception>
      public override void SetLength(long value)
      {
         throw new NotSupportedException(Resources.ThisStreamDoesNotSupportSeeking);
      }

      #endregion // SetLength

      #region Skip

      /// <summary>Skips ahead the specified number of bytes from the current stream.</summary>
      /// <remarks><para>This method represents the Win32 API implementation of <see href="http://msdn.microsoft.com/en-us/library/aa362509(VS.85).aspx">BackupSeek</see>.</para>
      /// <para>
      /// Applications use the <see cref="Skip"/> method to skip portions of a data stream that cause errors. This function does not 
      /// seek across stream headers. For example, this function cannot be used to skip the stream name. If an application 
      /// attempts to seek past the end of a substream, the function fails, the return value indicates the actual number of bytes 
      /// the function seeks, and the file position is placed at the start of the next stream header.
      /// </para>
      /// </remarks>
      /// <param name="bytes">The number of bytes to skip.</param>
      /// <returns>The number of bytes actually skipped.</returns>
      [SecurityCritical]
      public long Skip(long bytes)
      {
         uint lowSought, highSought;
         if (!NativeMethods.BackupSeek(SafeFileHandle, NativeMethods.GetLowOrderDword(bytes), NativeMethods.GetHighOrderDword(bytes), out lowSought, out highSought, out _context))
         {
            int lastError = Marshal.GetLastWin32Error();

            // Error Code 25 indicates a seek error, we just skip that here.
            if (lastError != Win32Errors.NO_ERROR && lastError != Win32Errors.ERROR_SEEK)
               // Throws IOException.
               NativeError.ThrowException(lastError, true);
         }

         return NativeMethods.ToLong(highSought, lowSought);
      }

      #endregion // Skip

      #endregion // Navigate

      #region Get/SetAccessControl

      #region GetAccessControl

      /// <summary>Gets a <see cref="FileSecurity"/> object that encapsulates the access control list (ACL) entries for the file described by the current <see cref="BackupFileStream"/> object.</summary>
      /// <returns>A <see cref="FileSecurity"/> object that encapsulates the access control list (ACL) entries for the file described by the current <see cref="BackupFileStream"/> object. </returns>
      
      [SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands")]
      [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
      [SecurityCritical]
      public FileSecurity GetAccessControl()
      {
         IntPtr pSidOwner, pSidGroup, pDacl, pSacl;
         SafeGlobalMemoryBufferHandle pSecurityDescriptor;

         uint lastError = SecurityNativeMethods.GetSecurityInfo(SafeFileHandle, ObjectType.FileObject,
            SecurityInformation.Group | SecurityInformation.Owner | SecurityInformation.Label | SecurityInformation.Dacl | SecurityInformation.Sacl,
            out pSidOwner, out pSidGroup, out pDacl, out pSacl, out pSecurityDescriptor);

         try
         {
            if (lastError != Win32Errors.ERROR_SUCCESS)
               // Throws IOException.
               NativeError.ThrowException((int) lastError, true);

            if (pSecurityDescriptor.IsInvalid)
               throw new IOException(Resources.InvalidSecurityDescriptorReturnedFromSystem);

            uint length = SecurityNativeMethods.GetSecurityDescriptorLength(pSecurityDescriptor);

            byte[] managedBuffer = new byte[length];

            // See File.GetAccessControlInternal(): .CopyTo() does not work there?
            pSecurityDescriptor.CopyTo(managedBuffer, 0, (int) length);

            FileSecurity fs = new FileSecurity();
            fs.SetSecurityDescriptorBinaryForm(managedBuffer);

            return fs;
         }
         finally
         {
            if (pSecurityDescriptor != null)
               pSecurityDescriptor.Close();
         }
      }

      #endregion // GetAccessControl

      #region SetAccessControl

      /// <summary>Applies access control list (ACL) entries described by a <see cref="FileSecurity"/> object to the file described by the  current <see cref="BackupFileStream"/> object.</summary>
      /// <param name="fileSecurity">A <see cref="FileSecurity"/> object that describes an ACL entry to apply to the current file.</param>
      
      [SecurityCritical]
      public void SetAccessControl(ObjectSecurity fileSecurity)
      {
         File.SetAccessControlInternal(null, SafeFileHandle, fileSecurity, AccessControlSections.All, false);
      }

      #endregion // SetAccessControl

      #endregion // Get/SetAccessControl

      #region Lock / Unlock

      #region Lock

      /// <summary>Prevents other processes from changing the <see cref="BackupFileStream"/> while permitting read access.</summary>
      /// <param name="position">The beginning of the range to lock. The value of this parameter must be equal to or greater than zero (0).</param>
      /// <param name="length">The range to be locked.</param>
      /// <exception cref="ArgumentOutOfRangeException"><paramref name="position"/> or <paramref name="length"/> is negative.</exception>
      /// <exception cref="ObjectDisposedException">The file is closed.</exception>
      [SecurityCritical]
      public virtual void Lock(long position, long length)
      {
         if (position < 0)
            throw new ArgumentOutOfRangeException("position", position, Resources.BackupFileStream_Unlock_Backup_FileStream_Unlock_Position_must_not_be_negative_);

         if (length < 0)
            throw new ArgumentOutOfRangeException("length", length, Resources.BackupFileStream_Unlock_Backup_FileStream_Lock_Length_must_not_be_negative_);

         if (!NativeMethods.LockFile(SafeFileHandle, NativeMethods.GetLowOrderDword(position), NativeMethods.GetHighOrderDword(position), NativeMethods.GetLowOrderDword(length), NativeMethods.GetHighOrderDword(length)))
            // Throws IOException.
            NativeError.ThrowException(Marshal.GetLastWin32Error(), true);
      }

      #endregion // Lock

      #region Unlock

      /// <summary>Allows access by other processes to all or part of a file that was previously locked.</summary>
      /// <param name="position">The beginning of the range to unlock.</param>
      /// <param name="length">The range to be unlocked.</param>
      /// <exception cref="ArgumentOutOfRangeException"></exception>
      /// <exception cref="ArgumentOutOfRangeException"><paramref name="position"/> or <paramref name="length"/> is negative.</exception>
      /// <exception cref="ObjectDisposedException">The file is closed.</exception>
      [SecurityCritical]
      public virtual void Unlock(long position, long length)
      {
         if (position < 0)
            throw new ArgumentOutOfRangeException("position", position, Resources.BackupFileStream_Unlock_Backup_FileStream_Unlock_Position_must_not_be_negative_);

         if (length < 0)
            throw new ArgumentOutOfRangeException("length", length, Resources.BackupFileStream_Unlock_Backup_FileStream_Lock_Length_must_not_be_negative_);

         if (!NativeMethods.UnlockFile(SafeFileHandle, NativeMethods.GetLowOrderDword(position), NativeMethods.GetHighOrderDword(position), NativeMethods.GetLowOrderDword(length), NativeMethods.GetHighOrderDword(length)))
            // Throws IOException.
            NativeError.ThrowException(Marshal.GetLastWin32Error(), true);
      }

      #endregion Unlock

      #endregion // Lock / Unlock

      #region EnumerateStreams
      /// <summary>
      ///   Returns <see cref="AlternateDataStreamInfo"/> instances, associated with the file.
      /// </summary>
      /// <returns>
      ///   An <see cref="IEnumerable{AlternateDataStreamInfo}"/> collection of streams for the file
      ///   specified by path.
      /// </returns>
      public IEnumerable<AlternateDataStreamInfo> EnumerateStreams()
      {
         return AlternateDataStreamInfo.EnumerateStreamsInternal(null, null, SafeFileHandle, null, null, null, null);
      }

      #endregion // EnumerateStreams

      #region ReadStreamInfo

      /// <summary>Reads a stream header from the current <see cref="AlternateDataStreamInfo"/>.</summary>
      /// <returns>The stream header read from the current <see cref="AlternateDataStreamInfo"/>, or <see langword="null"/> if the end-of-file 
      /// was reached before the required number of bytes of a header could be read.</returns>
      /// <remarks>The stream must be positioned at where an actual header starts for the returned object to represent valid information.</remarks>
      public AlternateDataStreamInfo ReadStreamInfo()
      {
         // Return the first entry.
         return AlternateDataStreamInfo.EnumerateStreamsInternal(null, null, SafeFileHandle, null, null, null, null).FirstOrDefault();
      }

      #endregion // ReadStreamInfo

      #endregion // Methods

      #region Properties

      #region CanRead

      private readonly bool _canRead;

      /// <summary>Gets a value indicating whether the current stream supports reading.</summary>
      /// <returns><see langword="true"/> if the stream supports reading, <see langword="false"/> otherwise.</returns>
      public override bool CanRead
      {
         get { return _canRead; }
      }

      #endregion // CanRead

      #region CanSeek

      /// <summary>Gets a value indicating whether the current stream supports seeking.</summary>        
      /// <returns>This method always returns <see langword="false"/>.</returns>
      public override bool CanSeek
      {
         get { return false; }
      }

      #endregion // CanSeek

      #region CanWrite

      private readonly bool _canWrite;

      /// <summary>Gets a value indicating whether the current stream supports writing.</summary>
      /// <returns><see langword="true"/> if the stream supports writing, <see langword="false"/> otherwise.</returns>
      public override bool CanWrite
      {
         get { return _canWrite; }
      }

      #endregion // CanWrite

      #region Length

      /// <summary>When overridden in a derived class, gets the length in bytes of the stream.</summary>
      /// <value>This method always throws an exception.</value>
      /// <exception cref="System.NotSupportedException">This exception is always thrown if this property is accessed on a <see cref="BackupFileStream"/>.</exception>
      public override long Length
      {
         get { throw new NotSupportedException(Resources.ThisStreamDoesNotSupportSeeking); }
      }

      #endregion // Length

      #region Position

      /// <summary>When overridden in a derived class, gets or sets the position within the current stream.</summary>
      /// <value>This method always throws an exception.</value>
      /// <exception cref="System.NotSupportedException">This exception is always thrown if this property is accessed on a <see cref="BackupFileStream"/>.</exception>
      public override long Position
      {
         get { throw new NotSupportedException(Resources.ThisStreamDoesNotSupportSeeking); }
         set { throw new NotSupportedException(Resources.ThisStreamDoesNotSupportSeeking); }
      }

      #endregion // Position

      #region SafeFileHandle

      private readonly SafeFileHandle _safeFileHandle;

      /// <summary>Gets a <see cref="SafeFileHandle"/> object that represents the operating system file handle for the file that the current <see cref="BackupFileStream"/> object encapsulates.</summary>
      /// <value>A <see cref="SafeFileHandle"/> object that represents the operating system file handle for the file that 
      /// the current <see cref="BackupFileStream"/> object encapsulates.</value>
      public SafeFileHandle SafeFileHandle
      {
         get { return _safeFileHandle; }
      }

      #endregion // SafeFileHandle

      #endregion // Properties

      #region Private Fields

      [SuppressMessage("Microsoft.Reliability", "CA2006:UseSafeHandleToEncapsulateNativeResources")]
      private IntPtr _context = IntPtr.Zero;

      #endregion // Private Fields
   }
}