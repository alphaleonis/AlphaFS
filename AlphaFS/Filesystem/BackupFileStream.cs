/* Copyright (c) 2008-2009 Peter Palotas
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
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Permissions;
using Microsoft.Win32.SafeHandles;
using SecInfo = Alphaleonis.Win32.Security.NativeMethods.SECURITY_INFORMATION;
using SecurityNativeMethods = Alphaleonis.Win32.Security.NativeMethods;

namespace Alphaleonis.Win32.Filesystem
{


    /// <summary>
    /// The <see cref="BackupFileStream"/> provides access to data associated with a specific file or directory, including security information
    /// and alternative data streams, for backup and restore operations.
    /// </summary>
    /// <remarks>This class uses the <see href="http://msdn.microsoft.com/en-us/library/aa362509(VS.85).aspx">BackupRead</see>, 
    /// <see href="http://msdn.microsoft.com/en-us/library/aa362510(VS.85).aspx">BackupSeek</see> and 
    /// <see href="http://msdn.microsoft.com/en-us/library/aa362511(VS.85).aspx">BackupWrite</see> functions from the Win32 API to provide
    /// access to the file or directory.
    /// </remarks>
    public class BackupFileStream : System.IO.Stream
    {
        #region Construction and Destruction

       /// <summary>
       /// Initializes a new instance of the <see cref="BackupFileStream"/> class with the specified path, creation mode, access rights
       /// and sharing permission, additional file options, access control and audit security.
       /// </summary>
       /// <param name="transaction">The transaction.</param>
       /// <param name="path">A relative or absolute path for the file that the current <see cref="BackupFileStream"/> object will encapsulate.</param>
       /// <param name="mode">A <see cref="FileMode"/> constant that determines how to open or create the file.</param>
       /// <param name="access">A <see cref="FileSystemRights"/> constant that determines the access rights to use when creating access and audit rules for the file.</param>
       /// <param name="share">A <see cref="FileShare"/> constant that determines how the file will be shared by processes.</param>
       /// <param name="options">A <see cref="FileOptions"/> constant that specifies additional file options.</param>
       /// <param name="security">A <see cref="FileSecurity"/> constant that determines the access control and audit security for the file. This parameter may be <see langword="null"/>.</param>
       /// <overloads>
       /// Initializes a new instance of the <see cref="BackupFileStream"/> class.
       /// </overloads>
       [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope"), SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
       public BackupFileStream(KernelTransaction transaction, string path, FileMode mode, FileSystemRights access, FileShare share, FileOptions options, FileSecurity security)
          : this(File.CreateInternal(transaction, path, mode, access, share, options, security), access)
       {
       }

       /// <summary>
       /// Initializes a new instance of the <see cref="BackupFileStream"/> class with the specified path, creation mode, access rights
       /// and sharing permission, and additional file options.
       /// </summary>
       /// <param name="transaction">The transaction.</param>
       /// <param name="path">A relative or absolute path for the file that the current <see cref="BackupFileStream"/> object will encapsulate.</param>
       /// <param name="mode">A <see cref="FileMode"/> constant that determines how to open or create the file.</param>
       /// <param name="access">A <see cref="FileSystemRights"/> constant that determines the access rights to use when creating access and audit rules for the file.</param>
       /// <param name="share">A <see cref="FileShare"/> constant that determines how the file will be shared by processes.</param>
       /// <param name="options">A <see cref="FileOptions"/> constant that specifies additional file options.</param>
       [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
       public BackupFileStream(KernelTransaction transaction, string path, FileMode mode, FileSystemRights access, FileShare share, FileOptions options)
          : this(transaction, path, mode, access, share, options, null)
       {
       }

       /// <summary>
       /// Initializes a new instance of the <see cref="BackupFileStream"/> class with the specified path, creation mode, access rights
       /// and sharing permission.
       /// </summary>
       /// <param name="transaction">The transaction.</param>
       /// <param name="path">A relative or absolute path for the file that the current <see cref="BackupFileStream"/> object will encapsulate.</param>
       /// <param name="mode">A <see cref="FileMode"/> constant that determines how to open or create the file.</param>
       /// <param name="access">A <see cref="FileSystemRights"/> constant that determines the access rights to use when creating access and audit rules for the file.</param>
       /// <param name="share">A <see cref="FileShare"/> constant that determines how the file will be shared by processes.</param>
       [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
       public BackupFileStream(KernelTransaction transaction, string path, FileMode mode, FileSystemRights access, FileShare share)
          : this(transaction, path, mode, access, share, FileOptions.None)
       {
       }

       /// <summary>
       /// Initializes a new instance of the <see cref="BackupFileStream"/> class with the specified path, creation mode and access rights.
       /// </summary>
       /// <param name="transaction">The transaction.</param>
       /// <param name="path">A relative or absolute path for the file that the current <see cref="BackupFileStream"/> object will encapsulate.</param>
       /// <param name="mode">A <see cref="FileMode"/> constant that determines how to open or create the file.</param>
       /// <param name="access">A <see cref="FileSystemRights"/> constant that determines the access rights to use when creating access and audit rules for the file.</param>
       /// <remarks>The file will be opened for exclusive access.</remarks>
       [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
       public BackupFileStream(KernelTransaction transaction, string path, FileMode mode, FileSystemRights access)
          : this(transaction, path, mode, access, FileShare.None)
       {
       }

       /// <summary>
       /// Initializes a new instance of the <see cref="BackupFileStream"/> class with the specified path and creation mode.
       /// </summary>
       /// <param name="transaction">The transaction.</param>
       /// <param name="path">A relative or absolute path for the file that the current <see cref="BackupFileStream"/> object will encapsulate.</param>
       /// <param name="mode">A <see cref="FileMode"/> constant that determines how to open or create the file.</param>
       /// <remarks>The file will be opened for exclusive access for both reading and writing.</remarks>
       [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
       public BackupFileStream(KernelTransaction transaction, string path, FileMode mode)
          : this(transaction, path, mode, FileSystemRights.Read | FileSystemRights.Write)
       {
       }

       /// <summary>
       /// Initializes a new instance of the <see cref="BackupFileStream"/> class with the specified path, creation mode, access rights
       /// and sharing permission, additional file options, access control and audit security.
       /// </summary>
       /// <param name="path">A relative or absolute path for the file that the current <see cref="BackupFileStream"/> object will encapsulate.</param>
       /// <param name="mode">A <see cref="FileMode"/> constant that determines how to open or create the file.</param>
       /// <param name="access">A <see cref="FileSystemRights"/> constant that determines the access rights to use when creating access and audit rules for the file.</param>
       /// <param name="share">A <see cref="FileShare"/> constant that determines how the file will be shared by processes.</param>
       /// <param name="options">A <see cref="FileOptions"/> constant that specifies additional file options.</param>
       /// <param name="security">A <see cref="FileSecurity"/> constant that determines the access control and audit security for the file. This parameter may be <see langword="null"/>.</param>
       /// <overloads>
       /// Initializes a new instance of the <see cref="BackupFileStream"/> class.
       /// </overloads>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope"), SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public BackupFileStream(string path, FileMode mode, FileSystemRights access, FileShare share, FileOptions options, FileSecurity security)
            : this(File.CreateInternal(path, mode, access, share, options, security), access)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BackupFileStream"/> class with the specified path, creation mode, access rights
        /// and sharing permission, and additional file options.
        /// </summary>
        /// <param name="path">A relative or absolute path for the file that the current <see cref="BackupFileStream"/> object will encapsulate.</param>
        /// <param name="mode">A <see cref="FileMode"/> constant that determines how to open or create the file.</param>
        /// <param name="access">A <see cref="FileSystemRights"/> constant that determines the access rights to use when creating access and audit rules for the file.</param>
        /// <param name="share">A <see cref="FileShare"/> constant that determines how the file will be shared by processes.</param>
        /// <param name="options">A <see cref="FileOptions"/> constant that specifies additional file options.</param>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public BackupFileStream(string path, FileMode mode, FileSystemRights access, FileShare share, FileOptions options)
            : this(path, mode, access, share, options, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BackupFileStream"/> class with the specified path, creation mode, access rights 
        /// and sharing permission.
        /// </summary>
        /// <param name="path">A relative or absolute path for the file that the current <see cref="BackupFileStream"/> object will encapsulate.</param>
        /// <param name="mode">A <see cref="FileMode"/> constant that determines how to open or create the file. </param>
        /// <param name="access">A <see cref="FileSystemRights"/> constant that determines the access rights to use when creating access and audit rules for the file.</param>
        /// <param name="share">A <see cref="FileShare"/> constant that determines how the file will be shared by processes. </param>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public BackupFileStream(string path, FileMode mode, FileSystemRights access, FileShare share)
            : this(path, mode, access, share, FileOptions.None)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BackupFileStream"/> class with the specified path, creation mode and access rights.
        /// </summary>
        /// <param name="path">A relative or absolute path for the file that the current <see cref="BackupFileStream"/> object will encapsulate.</param>
        /// <param name="mode">A <see cref="FileMode"/> constant that determines how to open or create the file. </param>
        /// <param name="access">A <see cref="FileSystemRights"/> constant that determines the access rights to use when creating access and audit rules for the file.</param>
        /// <remarks>The file will be opened for exclusive access.</remarks>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public BackupFileStream(string path, FileMode mode, FileSystemRights access)
            : this(path, mode, access, FileShare.None)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BackupFileStream"/> class with the specified path and creation mode.
        /// </summary>
        /// <param name="path">A relative or absolute path for the file that the current <see cref="BackupFileStream"/> object will encapsulate.</param>
        /// <param name="mode">A <see cref="FileMode"/> constant that determines how to open or create the file. </param>
        /// <remarks>The file will be opened for exclusive access for both reading and writing.</remarks>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public BackupFileStream(string path, FileMode mode)
            : this(path, mode, FileSystemRights.Read | FileSystemRights.Write)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BackupFileStream"/> class for the specified file handle, with the specified read/write permission.
        /// </summary>
        /// <param name="handle">A file handle for the file that this <see cref="BackupFileStream"/> object will encapsulate. </param>
        /// <param name="access">A <see cref="FileSystemRights"/> constant that gets the <see cref="CanRead"/> and <see cref="CanWrite"/> properties 
        /// of the <see cref="BackupFileStream"/> object. </param>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public BackupFileStream(SafeFileHandle handle, FileSystemRights access)
        {
            if (handle.IsInvalid)
                throw new ArgumentException(Alphaleonis.Win32.Resources.InvalidHandle);
            mFileHandle = handle;

            mCanRead = 0 != (access & FileSystemRights.ReadData);
            mCanWrite = 0 != (access & (FileSystemRights.WriteData));

            mProcessSecurity = true;
        }

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="BackupFileStream"/> is reclaimed by garbage collection.
        /// </summary>
        ~BackupFileStream()
        {
            Dispose(false);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Reads a sequence of bytes from the current stream and advances the position within
        /// the stream by the number of bytes read.
        /// </summary>
        /// <param name="buffer">An array of bytes. When this method returns, the buffer contains the specified byte array with the values
        /// between <paramref name="offset"/> and (<paramref name="offset"/> + <paramref name="count"/> - 1) replaced by the bytes read from the current source.</param>
        /// <param name="offset">The zero-based byte offset in <paramref name="buffer"/> at which to begin storing the data read from the current stream.</param>
        /// <param name="count">The maximum number of bytes to be read from the current stream.</param>
        /// <returns>
        /// The total number of bytes read into the buffer. This can be less than the number of bytes requested if that many bytes are not
        /// currently available, or zero (0) if the end of the stream has been reached.
        /// </returns>
        /// <exception cref="System.ArgumentException">The sum of <paramref name="offset"/> and <paramref name="count"/> is larger than the buffer length. </exception>
        /// <exception cref="System.ArgumentNullException">
        /// 	<paramref name="buffer"/> is null. </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// 	<paramref name="offset"/> or <paramref name="count"/> is negative. </exception>
        /// <exception cref="System.IO.IOException">An I/O error occurs. </exception>
        /// <exception cref="System.NotSupportedException">The stream does not support reading. </exception>
        /// <exception cref="System.ObjectDisposedException">Methods were called after the stream was closed. </exception>
        /// <remarks>This method will not backup the access-control list (ACL) data for the file or directory.</remarks>
        [SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
        public override int Read(byte[] buffer, int offset, int count)
        {
            return Read(buffer, offset, count, false);
        }

        /// <summary>
        /// When overridden in a derived class, reads a sequence of bytes from the current stream and advances the position within
        /// the stream by the number of bytes read.
        /// </summary>
        /// <param name="buffer">An array of bytes. When this method returns, the buffer contains the specified byte array with the values
        /// between <paramref name="offset"/> and (<paramref name="offset"/> + <paramref name="count"/> - 1) replaced by the bytes read from the current source.</param>
        /// <param name="offset">The zero-based byte offset in <paramref name="buffer"/> at which to begin storing the data read from the current stream.</param>
        /// <param name="count">The maximum number of bytes to be read from the current stream.</param>
        /// <param name="processSecurity">Indicates whether the function will backup the access-control list (ACL) data for the file or directory. </param>
        /// <returns>
        /// The total number of bytes read into the buffer. This can be less than the number of bytes requested if that many bytes are not
        /// currently available, or zero (0) if the end of the stream has been reached.
        /// </returns>
        /// <exception cref="System.ArgumentException">The sum of <paramref name="offset"/> and <paramref name="count"/> is larger than the buffer length. </exception>
        /// <exception cref="System.ArgumentNullException">
        /// 	<paramref name="buffer"/> is null. </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// 	<paramref name="offset"/> or <paramref name="count"/> is negative. </exception>
        /// <exception cref="System.IO.IOException">An I/O error occurs. </exception>
        /// <exception cref="System.NotSupportedException">The stream does not support reading. </exception>
        /// <exception cref="System.ObjectDisposedException">Methods were called after the stream was closed. </exception>
        [SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
        public int Read(byte[] buffer, int offset, int count, bool processSecurity)
        {
            if (!CanRead)
                throw new NotSupportedException("Stream does not support reading");

            if (buffer == null)
                throw new ArgumentNullException("buffer");

            if (offset + count > buffer.Length)
                throw new ArgumentException("The sum of offset and count is larger than the size of the buffer.");

            if (offset < 0)
                throw new ArgumentOutOfRangeException("offset", Alphaleonis.Win32.Resources.OffsetMustNotBeNegative);

            if (count < 0)
                throw new ArgumentOutOfRangeException("count", Alphaleonis.Win32.Resources.CountMustNotBeNegative);

            using (SafeGlobalMemoryBufferHandle hBuf = new SafeGlobalMemoryBufferHandle(count))
            {
                uint numberOfBytesRead;
                if (!NativeMethods.BackupRead(mFileHandle, hBuf, (uint)hBuf.Capacity, out numberOfBytesRead, false, processSecurity, ref mContext))
                    NativeError.ThrowException();

                hBuf.CopyTo(buffer, offset, count);

                return (int)numberOfBytesRead;
            }
        }

        /// <summary>
        /// Skips ahead the specified number of bytes from the current stream.
        /// </summary>
        /// <remarks>
        /// <para>
        ///     This method represents the Win32 API implementation of <see href="http://msdn.microsoft.com/en-us/library/aa362509(VS.85).aspx">BackupSeek</see>.
        /// </para>
        /// <para>
        /// Applications use the <see cref="Skip"/> method to skip portions of a data stream that cause errors. This function does not 
        /// seek across stream headers. For example, this function cannot be used to skip the stream name. If an application 
        /// attempts to seek past the end of a substream, the function fails, the return value indicates the actual number of bytes 
        /// the function seeks, and the file position is placed at the start of the next stream header.
        /// </para>
        /// </remarks>
        /// <param name="bytes">The number of bytes to skip.</param>
        /// <returns>The number of bytes actually skipped.</returns>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public long Skip(long bytes)
        {
            uint lowSought, highSought;

            if (!NativeMethods.BackupSeek(mFileHandle, NativeMethods.GetLowOrderDword(bytes), NativeMethods.GetHighOrderDword(bytes),
                out lowSought, out highSought, ref mContext))
            {
                int errorCode = Marshal.GetLastWin32Error();

                // Error Code 25 indicates a seek error, we just skip that here.
                if (errorCode != 0 && errorCode != 25)
                {
                    NativeError.ThrowException(errorCode);
                }
            }
            return ((long)lowSought & 0xFFFFFFFF) | ((long)highSought << 32);

        }

        /// <summary>
        /// When overridden in a derived class, sets the position within the current stream.
        /// </summary>
        /// <param name="offset">A byte offset relative to the <paramref name="origin"/> parameter.</param>
        /// <param name="origin">A value of type <see cref="System.IO.SeekOrigin"/> indicating the reference point used to obtain the new position.</param>
        /// <returns>
        /// The new position within the current stream.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         <note>
        ///             <para>
        ///                 This stream does not support seeking using this method, and calling this method will always throw 
        ///                 <see cref="NotSupportedException"/>. See <see cref="Skip"/> for an alternative way of seeking forward.
        ///             </para>
        ///         </note>
        ///     </para>
        /// </remarks>
        /// <exception cref="System.NotSupportedException">The stream does not support seeking.</exception>
        public override long Seek(long offset, System.IO.SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Applies access control list (ACL) entries described by a <see cref="FileSecurity"/> object to the file described by the 
        /// current <see cref="BackupFileStream"/> object. 
        /// </summary>
        /// <param name="fileSecurity">A <see cref="FileSecurity"/> object that describes an ACL entry to apply to the current file.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters"), SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public void SetAccessControl(FileSecurity fileSecurity)
        {
            Security.NativeMethods.SetSecurityInfo(mFileHandle, Security.NativeMethods.SE_OBJECT_TYPE.SE_FILE_OBJECT, fileSecurity);
        }

        /// <summary>
        /// When overridden in a derived class, sets the length of the current stream.
        /// </summary>
        /// <param name="value">The desired length of the current stream in bytes.</param>
        /// <remarks>This method is not supported by the <see cref="BackupFileStream"/> class, and calling it will always
        /// generate a <see cref="NotSupportedException"/>.</remarks>
        /// <exception cref="System.NotSupportedException">Always thrown by this class.</exception>
        public override void SetLength(long value)
        {
            throw new NotSupportedException(Resources.ThisStreamDoesNotSupportSeeking);
        }

        /// <summary>
        /// Writes a sequence of bytes to the current stream and advances the current position 
        /// within this stream by the number of bytes written.
        /// </summary>
        /// <overloads>
        /// Writes a sequence of bytes to the current stream and advances the current position within this stream by the number of bytes written.
        /// </overloads>
        /// <param name="buffer">An array of bytes. This method copies <paramref name="count"/> bytes from <paramref name="buffer"/> to the current stream.</param>
        /// <param name="offset">The zero-based byte offset in <paramref name="buffer"/> at which to begin copying bytes to the current stream.</param>
        /// <param name="count">The number of bytes to be written to the current stream.</param>
        /// <exception cref="System.ArgumentException">The sum of <paramref name="offset"/> and <paramref name="count"/> is greater than the buffer length. </exception>
        /// <exception cref="System.ArgumentNullException">
        /// 	<paramref name="buffer"/> is null. </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// 	<paramref name="offset"/> or <paramref name="count"/> is negative. </exception>
        /// <exception cref="System.IO.IOException">An I/O error occurs. </exception>
        /// <exception cref="System.NotSupportedException">The stream does not support writing. </exception>
        /// <exception cref="System.ObjectDisposedException">Methods were called after the stream was closed. </exception>
        /// <remarks>This method will not process the access-control list (ACL) data for the file or directory.</remarks>
        [SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
        public override void Write(byte[] buffer, int offset, int count)
        {
            Write(buffer, offset, count, false);
        }

        /// <summary>
        /// When overridden in a derived class, writes a sequence of bytes to the current stream and advances the current position
        /// within this stream by the number of bytes written.
        /// </summary>
        /// <param name="buffer">An array of bytes. This method copies <paramref name="count"/> bytes from <paramref name="buffer"/> to the current stream.</param>
        /// <param name="offset">The zero-based byte offset in <paramref name="buffer"/> at which to begin copying bytes to the current stream.</param>
        /// <param name="count">The number of bytes to be written to the current stream.</param>
        /// <param name="processSecurity">Specifies whether the function will restore the access-control list (ACL) data for the file or directory. 
        /// If this is <see langword="true"/>, you need to specify <see cref="FileSystemRights.TakeOwnership"/> and <see cref="FileSystemRights.ChangePermissions"/> access when 
        /// opening the file or directory handle. If the handle does not have those access rights, the operating system denies 
        /// access to the ACL data, and ACL data restoration will not occur.</param>
        /// <exception cref="System.ArgumentException">The sum of <paramref name="offset"/> and <paramref name="count"/> is greater than the buffer length. </exception>
        /// <exception cref="System.ArgumentNullException">
        /// 	<paramref name="buffer"/> is null. </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// 	<paramref name="offset"/> or <paramref name="count"/> is negative. </exception>
        /// <exception cref="System.IO.IOException">An I/O error occurs. </exception>
        /// <exception cref="System.NotSupportedException">The stream does not support writing. </exception>
        /// <exception cref="System.ObjectDisposedException">Methods were called after the stream was closed. </exception>
        [SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
        public void Write(byte[] buffer, int offset, int count, bool processSecurity)
        {
            if (buffer == null)
                throw new ArgumentNullException("buffer");

            if (offset < 0)
                throw new ArgumentOutOfRangeException("offset", Alphaleonis.Win32.Resources.OffsetMustNotBeNegative);

            if (count < 0)
                throw new ArgumentOutOfRangeException("count", Alphaleonis.Win32.Resources.CountMustNotBeNegative);

            if (offset + count > buffer.Length)
                throw new ArgumentException(Alphaleonis.Win32.Resources.BufferIsNotLargeEnoughForTheRequestedOperation);

            using (SafeGlobalMemoryBufferHandle hBuf = new SafeGlobalMemoryBufferHandle(count))
            {
                hBuf.CopyFrom(buffer, offset, count);
                uint bytesWritten;
                if (!NativeMethods.BackupWrite(mFileHandle, hBuf, (uint)hBuf.Capacity, out bytesWritten, false, processSecurity, ref mContext))
                    NativeError.ThrowException();
            }
        }
        

        /// <summary>
        /// Allows access by other processes to all or part of a file that was previously locked. 
        /// </summary>
        /// <param name="position">The beginning of the range to unlock.</param>
        /// <param name="length">The range to be unlocked.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="position"/> or <paramref name="length"/> is negative.</exception>
        /// <exception cref="ObjectDisposedException">The file is closed.</exception>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public virtual void Unlock(long position, long length)
        {
            if (position < 0)
                throw new ArgumentOutOfRangeException("position", position, "Position must not be negative.");

            if (length < 0)
                throw new ArgumentOutOfRangeException("length", length, "Length must not be negative.");

            if (!NativeMethods.UnlockFile(mFileHandle,
                    NativeMethods.GetLowOrderDword(position),
                    NativeMethods.GetHighOrderDword(position),
                    NativeMethods.GetLowOrderDword(length),
                    NativeMethods.GetHighOrderDword(length)))
            {
                NativeError.ThrowException();
            }
        }

        /// <summary>
        /// Clears all buffers for this stream and causes any buffered data to be written to the underlying device.
        /// </summary>
        /// <exception cref="System.IO.IOException">An I/O error occurs. </exception>
        [SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
        public override void Flush()
        {
            NativeMethods.FlushFileBuffers(mFileHandle);
        }

        /// <summary>
        /// Gets a <see cref="FileSecurity"/> object that encapsulates the access control list (ACL) entries for the file described by the 
        /// current <see cref="BackupFileStream"/> object. 
        /// </summary>
        /// <returns>A <see cref="FileSecurity"/> object that encapsulates the access control list (ACL) entries for the file described by the 
        /// current <see cref="BackupFileStream"/> object. </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public FileSecurity GetAccessControl()
        {
            IntPtr pSidOwner, pSidGroup, pDacl, pSacl;
            Alphaleonis.Win32.Security.SafeLocalMemoryBufferHandle pSecurityDescriptor;
            uint errorCode = SecurityNativeMethods.GetSecurityInfo(mFileHandle, SecurityNativeMethods.SE_OBJECT_TYPE.SE_FILE_OBJECT,
                SecInfo.DACL_SECURITY_INFORMATION | SecInfo.GROUP_SECURITY_INFORMATION | SecInfo.OWNER_SECURITY_INFORMATION |
                SecInfo.LABEL_SECURITY_INFORMATION | SecInfo.SACL_SECURITY_INFORMATION,
                out pSidOwner, out pSidGroup, out pDacl, out pSacl, out pSecurityDescriptor);

            try
            {
                if (errorCode != 0)
                    NativeError.ThrowException(errorCode);

                if (!SecurityNativeMethods.IsValidSecurityDescriptor(pSecurityDescriptor))
                    throw new System.IO.IOException(Alphaleonis.Win32.Resources.InvalidSecurityDescriptorReturnedFromSystem);

                uint length = SecurityNativeMethods.GetSecurityDescriptorLength(pSecurityDescriptor);

                byte[] managedBuffer = new byte[length];

                pSecurityDescriptor.CopyTo(managedBuffer, 0, (int)length);

                FileSecurity fs = new FileSecurity();
                fs.SetSecurityDescriptorBinaryForm(managedBuffer);
                return fs;
            }
            finally
            {
                pSecurityDescriptor.Dispose();
            }
        }


        /// <summary>
        /// Reads a stream header from the current <see cref="BackupFileStream"/>.
        /// </summary>
        /// <returns>The stream header read from the current <see cref="BackupFileStream"/>, or <see langword="null"/> if the end-of-file 
        /// was reached before the required number of bytes of a header could be read.</returns>
        /// <remarks>The stream must be positioned at where an actual header starts for the returned object to represent valid 
        /// information.</remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2001:AvoidCallingProblematicMethods", MessageId = "System.Runtime.InteropServices.SafeHandle.DangerousGetHandle"), SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public BackupStreamInfo ReadStreamInfo()
        {
            using (SafeGlobalMemoryBufferHandle hBuf = new SafeGlobalMemoryBufferHandle(Marshal.SizeOf(typeof(NativeMethods.WIN32_STREAM_ID))))
            {
                uint numberOfBytesRead;
                if (!NativeMethods.BackupRead(mFileHandle, hBuf, (uint)Marshal.SizeOf(typeof(NativeMethods.WIN32_STREAM_ID)), out numberOfBytesRead, false, mProcessSecurity, ref mContext))
                    NativeError.ThrowException();

                if (numberOfBytesRead == 0)
                    return null;

                if (numberOfBytesRead < Marshal.SizeOf(typeof(NativeMethods.WIN32_STREAM_ID)))
                    throw new System.IO.IOException(Alphaleonis.Win32.Resources.IncompleteHeaderRead);

                NativeMethods.WIN32_STREAM_ID streamID = (NativeMethods.WIN32_STREAM_ID)Marshal.PtrToStructure(hBuf.DangerousGetHandle(), typeof(NativeMethods.WIN32_STREAM_ID));

                uint nameLength = (uint)Math.Min(streamID.dwStreamNameSize, hBuf.Capacity);
                if (!NativeMethods.BackupRead(mFileHandle, hBuf, nameLength, out numberOfBytesRead, false, mProcessSecurity, ref mContext))
                    NativeError.ThrowException();


                string name = Marshal.PtrToStringUni(hBuf.DangerousGetHandle(), (int)nameLength / 2);

                return new BackupStreamInfo(streamID, name);

            }
        }

        /// <summary>
        /// Prevents other processes from changing the <see cref="BackupFileStream"/> while permitting read access. 
        /// </summary>
        /// <param name="position">The beginning of the range to lock. The value of this parameter must be equal to or greater than zero (0).</param>
        /// <param name="length">The range to be locked. </param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="position"/> or <paramref name="length"/> is negative.</exception>
        /// <exception cref="ObjectDisposedException">The file is closed.</exception>
        /// <exception cref="System.IO.IOException">The process cannot access the file because another process has locked a portion of the file. </exception>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public virtual void Lock(long position, long length)
        {
            if (position < 0)
                throw new ArgumentOutOfRangeException("position", position, "Position must not be negative.");

            if (length < 0)
                throw new ArgumentOutOfRangeException("length", length, "Length must not be negative.");

            if (!NativeMethods.LockFile(mFileHandle,
                    NativeMethods.GetLowOrderDword(position),
                    NativeMethods.GetHighOrderDword(position),
                    NativeMethods.GetLowOrderDword(length),
                    NativeMethods.GetHighOrderDword(length)))
            {
                NativeError.ThrowException();
            }
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets a value indicating whether the current stream supports reading.
        /// </summary>
        /// <value></value>
        /// <returns>true if the stream supports reading; otherwise, false.</returns>
        public override bool CanRead
        {
            get { return mCanRead; }
        }

        /// <summary>
        /// Gets a value indicating whether the current stream supports seeking.
        /// </summary>        
        /// <returns>This method always returns <see langword="false"/>.</returns>
        public override bool CanSeek
        {
            get { return false; }
        }

        /// <summary>
        /// Gets a value indicating whether the current stream supports writing.
        /// </summary>
        /// <value></value>
        /// <returns>true if the stream supports writing; otherwise, false.</returns>
        public override bool CanWrite
        {
            get { return mCanWrite; }
        }

        /// <summary>
        /// When overridden in a derived class, gets the length in bytes of the stream.
        /// </summary>
        /// <value>This method always throws an exception.</value>
        /// <exception cref="System.NotSupportedException">This exception is always thrown if this property is accessed on a <see cref="BackupFileStream"/>.</exception>
        public override long Length
        {
            get { throw new NotSupportedException(Resources.ThisStreamDoesNotSupportSeeking); }
        }

        /// <summary>
        /// When overridden in a derived class, gets or sets the position within the current stream.
        /// </summary>
        /// <value>This method always throws an exception.</value>
        /// <exception cref="System.NotSupportedException">This exception is always thrown if this property is accessed on a <see cref="BackupFileStream"/>.</exception>
        public override long Position
        {
            get
            {
                throw new NotSupportedException(Resources.ThisStreamDoesNotSupportSeeking);
            }

            set
            {
                throw new NotSupportedException(Resources.ThisStreamDoesNotSupportSeeking);
            }
        }

        /// <summary>
        /// Gets a <see cref="SafeFileHandle"/> object that represents the operating system file handle for the file that 
        /// the current <see cref="BackupFileStream"/> object encapsulates.
        /// </summary>
        /// <value>A <see cref="SafeFileHandle"/> object that represents the operating system file handle for the file that 
        /// the current <see cref="BackupFileStream"/> object encapsulates.</value>
        public SafeHandle SafeHandle { get { return mFileHandle; } }

        #endregion

        #region Protected methods

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="System.IO.Stream"/> and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope"), SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
        protected override void Dispose(bool disposing)
        {
            //if one of the constructors previously threw an exception, than the object hasn't been initialized properly
            //and call from finalize will fail
            if (mFileHandle != null)
            {
                if (!mFileHandle.IsClosed && !mFileHandle.IsInvalid)
                {
                    uint temp;
                    if (mContext != IntPtr.Zero)
                    {
                        NativeMethods.BackupRead(mFileHandle, new SafeGlobalMemoryBufferHandle(), 0, out temp, true, false, ref mContext);
                        mContext = IntPtr.Zero;
                    }
                }

                mFileHandle.Dispose();
            }

            base.Dispose(disposing);
        }

        #endregion

        #region Private fields

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2006:UseSafeHandleToEncapsulateNativeResources")]
        private IntPtr mContext = IntPtr.Zero;
        private SafeFileHandle mFileHandle;
        private bool mCanRead;
        private bool mCanWrite;
        private bool mProcessSecurity;

        #endregion 

    }
}
