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
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Permissions;
using System.Text;
using Alphaleonis.Win32.Security;
using Microsoft.Win32.SafeHandles;
using FileStream = System.IO.FileStream;
using SecurityNativeMethods = Alphaleonis.Win32.Security.NativeMethods;
using StreamReader = System.IO.StreamReader;
using StreamWriter = System.IO.StreamWriter;

namespace Alphaleonis.Win32.Filesystem
{
    /// <summary>
    /// Provides static methods for the creation, copying, deletion, moving, and opening of files, and aids in the creation of <see cref="FileStream"/> objects.
    /// As opposed to the corresponding <see cref="System.IO.File"/> class, this class supports the use of extended length unicode paths, such as 
    /// <c>\\?\Volume{c00fa7c5-63eb-11dd-b6ed-806e6f6e6963}\autoexec.bat</c>. In addition, support for transacted file operation using the 
    /// kernel transaction manager is provided. (See also <see cref="KernelTransaction"/>).
    /// </summary>
    /// <remarks>Note that no methods in this class perform any validation of the supplied paths. They are passed as is to the corresponding
    /// native kernel functions, meaning that invalid paths may result in exceptions of a type other than the expected for a certain operation.</remarks>
    public static class File
    {
        #region AppendAllText

        /// <overloads>
        /// <summary>
        /// Opens a file, appends the specified string to the file, and then closes the file. 
        /// </summary>
        /// <remarks>
        /// If the file does not exist, this method creates a file, writes the specified string to the file, then closes the file.
        /// </remarks>
        /// </overloads>
        /// <summary>
        /// Opens a file, appends the specified string to the file, and then closes the file. If the file does not exist, this method creates a file, writes the specified string to the file, then closes the file.
        /// </summary>
        /// <param name="path">The file to append the specified string to.</param>
        /// <param name="contents">The string to append to the file.</param>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static void AppendAllText(string path, string contents)
        {
            AppendAllText(path, contents, Encoding.UTF8);
        }

        /// <summary>
        /// Appends the specified string to the file, creating the file if it does not already exist.
        /// </summary>
        /// <param name="path">The file to append the specified string to. </param>
        /// <param name="contents">The string to append to the file. </param>
        /// <param name="encoding">The character encoding to use.</param>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static void AppendAllText(string path, string contents, Encoding encoding)
        {
            using (FileStream fs = Open(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read))
            {
                fs.Seek(0, System.IO.SeekOrigin.End);
                StreamWriter writer = new StreamWriter(fs, encoding);
                writer.Write(contents);
            }
        }

        /// <summary>
        /// Opens a file as part of a transaction, appends the specified string to the file, and then closes the file. If the file does not exist, this method creates a file, writes the specified string to the file, then closes the file.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        /// <param name="path">The file to append the specified string to.</param>
        /// <param name="contents">The string to append to the file.</param>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static void AppendAllText(KernelTransaction transaction, string path, string contents)
        {
            AppendAllText(transaction, path, contents, Encoding.UTF8);
        }

        /// <summary>
        /// Appends the specified string to the file as part of a transaction, creating the file if it does not already exist.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        /// <param name="path">The file to append the specified string to.</param>
        /// <param name="contents">The string to append to the file.</param>
        /// <param name="encoding">The character encoding to use.</param>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static void AppendAllText(KernelTransaction transaction, string path, string contents, Encoding encoding)
        {
            using (FileStream fs = Open(transaction, path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read))
            {
                fs.Seek(0, System.IO.SeekOrigin.End);
                StreamWriter writer = new StreamWriter(fs, encoding);
                writer.Write(contents);
            }
        }

        #endregion

        #region AppendText

        /// <overloads>
        /// Creates a <see cref="System.IO.StreamWriter"/> that appends UTF-8 encoded text to an existing file.
        /// </overloads>
        /// <summary>
        /// Creates a <see cref="System.IO.StreamWriter"/> that appends UTF-8 encoded text to an existing file.
        /// </summary>
        /// <param name="path">The path to the file to append to.</param>
        /// <returns>A <see cref="System.IO.StreamWriter"/> that appends UTF-8 encoded text to an existing file.</returns>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static StreamWriter AppendText(string path)
        {
            FileStream fs = Open(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read);
            try
            {
                fs.Seek(0, System.IO.SeekOrigin.End);
                return new StreamWriter(fs, Encoding.UTF8);
            }
            catch
            {
                fs.Close();
                throw;
            }
        }

        /// <summary>
        /// Creates a <see cref="System.IO.StreamWriter"/>, that is part of a transaction, that appends UTF-8 encoded text to an existing file.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        /// <param name="path">The path to the file to append to.</param>
        /// <returns>
        /// A <see cref="System.IO.StreamWriter"/> that appends UTF-8 encoded text to an existing file.
        /// </returns>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static StreamWriter AppendText(KernelTransaction transaction, string path)
        {
            FileStream fs = Open(transaction, path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read);
            try
            {
                fs.Seek(0, System.IO.SeekOrigin.End);
                return new StreamWriter(fs, Encoding.UTF8);
            }
            catch
            {
                fs.Close();
                throw;
            }
        }

        #endregion

        #region Copy

        #region Non transacted

        /// <overloads>
        /// Copies an existing file to a new file.
        /// </overloads>
        /// <summary>
        /// Copies an existing file to a new file. Overwriting a file of the same name is not allowed.
        /// </summary>
        /// <param name="sourceFileName">The file to copy. </param>
        /// <param name="destinationFileName">The name of the destination file. This cannot be a directory or an existing file.</param>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static void Copy(string sourceFileName, string destinationFileName)
        {
            Copy(sourceFileName, destinationFileName, CopyOptions.FailIfExists, null, null, false);
        }

        /// <summary>
        /// Copies an existing file to a new file. 
        /// </summary>
        /// <param name="sourceFileName">The file to copy.</param>
        /// <param name="destinationFileName">The name of the destination file. This cannot be a directory or an existing file.</param>
        /// <param name="overwrite"><c>true</c> if the destination file can be overwritten; otherwise, <c>false</c>.</param>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static void Copy(string sourceFileName, string destinationFileName, bool overwrite)
        {
            Copy(sourceFileName, destinationFileName, overwrite ? CopyOptions.None : CopyOptions.FailIfExists, null, null, false);
        }

        /// <summary>
        /// Copies an existing file to a new file.
        /// </summary>
        /// <param name="existingFileName">The name of an existing file. </param>
        /// <param name="destinationFileName">The name of the new file. </param>
        /// <param name="copyMode">Flags that specify how the file is to be copied.</param>
        /// <returns><c>true</c> if the file was completely copied, or <c>false</c> if the copy operation was aborted.</returns>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static bool Copy(string existingFileName, string destinationFileName, CopyOptions copyMode)
        {
            return Copy(existingFileName, destinationFileName, copyMode, null, null, false);
        }

        /// <summary>
        /// Copies an existing file to a new file.
        /// </summary>
        /// <param name="existingFileName">The name of an existing file.</param>
        /// <param name="destinationFileName">The name of the new file.</param>
        /// <param name="copyMode">Flags that specify how the file is to be copied.</param>
        /// <param name="preserveDates"><c>true</c> if original Timestamps must be preserved, otherwise <c>false</c></param>
        /// <returns>
        /// 	<c>true</c> if the file was completely copied, or <c>false</c> if the copy operation was aborted.
        /// </returns>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static bool Copy(string existingFileName, string destinationFileName, CopyOptions copyMode, bool preserveDates)
        {
            return Copy(existingFileName, destinationFileName, copyMode, null, null, preserveDates);
        }

        /// <summary>
        /// Copies an existing file to a new file, notifying the application of its progress through a callback function.
        /// </summary>
        /// <param name="existingFileName">The name of an existing file.</param>
        /// <param name="destinationFileName">The name of the new file.</param>
        /// <param name="copyMode">Flags that specify how the file is to be copied.</param>
        /// <param name="progressRoutine">A callback function that is called each time another portion of the file has been copied. This parameter can be <see langword="null"/>.</param>
        /// <param name="userProgressData">The argument to be passed to the callback function. This parameter can be <see langword="null"/>.</param>
        /// <param name="preserveDates"><c>true</c> if original Timestamps must be preserved, otherwise <c>false</c></param>
        /// <returns>
        /// 	<c>true</c> if the file was completely copied, or <c>false</c> if the copy operation was aborted.
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "3#"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Flags"), SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static bool Copy(string existingFileName, string destinationFileName,
            CopyOptions copyMode, CopyProgressRoutine progressRoutine, object userProgressData, bool preserveDates)
        {
            if (existingFileName == null)
                throw new ArgumentNullException("existingFileName");

            if (destinationFileName == null)
                throw new ArgumentNullException("destinationFileName");

            FileSystemEntryInfo originalAttributes = null;
            // acquiring Timestamps before copy, to preserve original last access time.
            if (preserveDates)
            {
                originalAttributes = GetFileSystemEntryInfo(existingFileName);
            }

            NativeMethods.NativeCopyProgressRoutine routine = null;
            if (progressRoutine != null)
            {
                routine = delegate(Int64 TotalFileSize, Int64 TotalBytesTransferred, Int64 StreamSize,
                            Int64 StreamBytesTransferred, UInt32 dwStreamNumber, CopyProgressCallbackReason dwCallbackReason, IntPtr hSourceFile,
                            IntPtr hDestinationFile, IntPtr lpData)
                {
                    return (CopyProgressResult)progressRoutine(TotalFileSize, TotalBytesTransferred,
                        StreamSize, StreamBytesTransferred, dwStreamNumber,
                        (CopyProgressCallbackReason)dwCallbackReason,
                        userProgressData);
                };
            }

            int cancel = 0;
            if (!NativeMethods.CopyFileExW(existingFileName, destinationFileName, routine, new SafeGlobalMemoryBufferHandle(), ref cancel, copyMode))
            {
                uint result = (uint)Marshal.GetLastWin32Error();
                if (result == Win32Errors.ERROR_REQUEST_ABORTED)
                    return false;
                NativeError.ThrowException(result, existingFileName, destinationFileName);
            }

            // applying original Timestamps if it was requested
            if (preserveDates)
            {
                SetFileTimeInternal(destinationFileName, originalAttributes.Win32FindData.ftCreationTime.AsLong(), originalAttributes.Win32FindData.ftLastAccessTime.AsLong(), originalAttributes.Win32FindData.ftLastWriteTime.AsLong());
            }

            return true; // copy operation succeeded
        }

        #endregion

        #region Transacted

        /// <summary>
        /// Copies an existing file to a new file as a transacted operation. Overwriting a file of the same name is not allowed.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        /// <param name="sourceFileName">The file to copy.</param>
        /// <param name="destinationFileName">The name of the destination file. This cannot be a directory or an existing file.</param>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static void Copy(KernelTransaction transaction, string sourceFileName, string destinationFileName)
        {
            Copy(transaction, sourceFileName, destinationFileName, CopyOptions.FailIfExists);
        }

        /// <summary>
        /// Copies an existing file to a new file as a transacted operation.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        /// <param name="sourceFileName">The file to copy.</param>
        /// <param name="destinationFileName">The name of the destination file. This cannot be a directory or an existing file.</param>
        /// <param name="overwrite"><c>true</c> if the destination file can be overwritten; otherwise, <c>false</c>.</param>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static void Copy(KernelTransaction transaction, string sourceFileName, string destinationFileName, bool overwrite)
        {
            Copy(transaction, sourceFileName, destinationFileName, overwrite ? CopyOptions.None : CopyOptions.FailIfExists);
        }

        /// <summary>
        /// Copies an existing file to a new file as a transacted operation.
        /// </summary>
        /// <param name="existingFileName">The name of an existing file. </param>
        /// <param name="destinationFileName">The name of the new file. </param>
        /// <param name="copyMode">Flags that specify how the file is to be copied.</param>
        /// <param name="transaction">The transaction to use.</param>
        /// <returns><c>true</c> if the file was completely copied, or <c>false</c> if the copy operation was aborted.</returns>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static bool Copy(KernelTransaction transaction, string existingFileName, string destinationFileName, CopyOptions copyMode)
        {
            return Copy(transaction, existingFileName, destinationFileName, copyMode, null, null);
        }

        /// <summary>
        /// Copies an existing file to a new file as a transacted operation, notifying the application of its progress through a callback function.
        /// </summary>
        /// <param name="existingFileName">The name of an existing file. </param>
        /// <param name="destinationFileName">The name of the new file. </param>
        /// <param name="copyMode">Flags that specify how the file is to be copied.</param>
        /// <param name="progressRoutine">A callback function that is called each time another portion of the file has been copied. This parameter can be <see langword="null"/>.</param>
        /// <param name="userProgressData">The argument to be passed to the callback function. This parameter can be <see langword="null"/>.</param>
        /// <param name="transaction">The transaction to use.</param>
        /// <returns><c>true</c> if the file was completely copied, or <c>false</c> if the copy operation was aborted.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "4#"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "3#"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Flags"), SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static bool Copy(KernelTransaction transaction, string existingFileName, string destinationFileName,
            CopyOptions copyMode, CopyProgressRoutine progressRoutine, object userProgressData)
        {
            if (transaction == null)
                throw new ArgumentNullException("transaction");

            if (existingFileName == null)
                throw new ArgumentNullException("existingFileName");

            if (destinationFileName == null)
                throw new ArgumentNullException("destinationFileName");

            NativeMethods.NativeCopyProgressRoutine routine = null;
            if (progressRoutine != null)
            {
                routine = delegate(long TotalFileSize, long TotalBytesTransferred, long StreamSize,
                            long StreamBytesTransferred, uint dwStreamNumber, CopyProgressCallbackReason dwCallbackReason, IntPtr hSourceFile,
                            IntPtr hDestinationFile, IntPtr lpData)
                {
                    return progressRoutine(TotalFileSize, TotalBytesTransferred,
                        StreamSize, StreamBytesTransferred, dwStreamNumber,
                        (CopyProgressCallbackReason)dwCallbackReason,
                        userProgressData);
                };
            }

            int cancel = 0;
            if (!NativeMethods.CopyFileTransactedW(existingFileName, destinationFileName, routine, new SafeGlobalMemoryBufferHandle(), ref cancel, copyMode, transaction.SafeHandle))
            {
                uint result = (uint)Marshal.GetLastWin32Error();
                if (result == Win32Errors.ERROR_REQUEST_ABORTED)
                    return false;
                NativeError.ThrowException(result, existingFileName, destinationFileName);
            }
            return true;

        }

        #endregion

        #endregion

        #region Create

        #region Non transacted

        /// <overloads>
        /// Creates, overwrites or opens a file or directory in the specified path.
        /// </overloads>
        /// <summary>
        /// Creates or overwrites a file in the specified path.
        /// </summary>
        /// <param name="path">The path and name of the file to create.</param>
        /// <returns>A <see cref="FileStream"/> that provides read/write access to the file specified in path.</returns>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public static FileStream Create(string path)
        {
            // Using default buffer size of 4096 here, no idea what would be a good value though.
            return Create(path, 4096, FileOptions.None, null);
        }

        /// <summary>
        /// Creates or overwrites a file in the specified path.
        /// </summary>
        /// <param name="path">The path and name of the file to create.</param>
        /// <param name="bufferSize">The number of bytes buffered for reads and writes to the file.</param>
        /// <returns>
        /// A <see cref="FileStream"/> that provides read/write access to the file specified in path.
        /// </returns>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public static FileStream Create(string path, int bufferSize)
        {
            return Create(path, bufferSize, FileOptions.None, null);
        }

        /// <summary>
        /// Creates or overwrites the specified file, specifying a buffer size and a <see cref="FileOptions"/> value that describes how to create or overwrite the file.
        /// </summary>
        /// <param name="path">The name of the file.</param>
        /// <param name="bufferSize">The number of bytes buffered for reads and writes to the file.</param>
        /// <param name="options">One of the <see cref="FileOptions"/> values that describes how to create or overwrite the file.</param>
        /// <returns>
        /// A new file with the specified buffer size.
        /// </returns>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public static FileStream Create(string path, int bufferSize, FileOptions options)
        {
            return Create(path, bufferSize, options, null);
        }

        /// <summary>
        /// Creates or overwrites the specified file, specifying a buffer size, a <see cref="FileOptions"/> value that describes how to create or overwrite the file and a specified <see cref="FileSecurity"/>.
        /// </summary>
        /// <param name="path">The name of the file.</param>
        /// <param name="bufferSize">The number of bytes buffered for reads and writes to the file.</param>
        /// <param name="options">One of the <see cref="FileOptions"/> values that describes how to create or overwrite the file.</param>
        /// <param name="fileSecurity">A <see cref="FileSecurity"/> instance that determines the access control and audit security for the file.</param>
        /// <returns>
        /// A new file with the specified buffer size and security options.
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope"), SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public static FileStream Create(string path, int bufferSize, FileOptions options, FileSecurity fileSecurity)
        {
            SafeFileHandle handle = File.CreateInternal(path, FileMode.Create, (FileSystemRights)FileAccess.ReadWrite, FileShare.None, options, fileSecurity);
            return new System.IO.FileStream(handle, System.IO.FileAccess.ReadWrite, bufferSize, (options & FileOptions.Overlapped) != 0);
        }

        /// <summary>
        /// Creates or overwrites the specified file,
        /// specifying advanced options <see cref="FileMode"/>, <see cref="FileAccess"/>, <see cref="FileShare"/>, <see cref="FileOptions"/>, <see cref="FileSecurity"/> and a buffer size.
        /// </summary>
        /// <param name="path">The name of the file.</param>
        /// <param name="mode">The <see cref="FileMode"/> option gives you more precise control over how you want to create a file.</param>
        /// <param name="access">The <see cref="FileAccess"/> allow you additionaly specify to default redwrite capability - just write, bypassing any cache.</param>
        /// <param name="share">The <see cref="FileShare"/> option controls how you would like to share created file with other requesters.</param>
        /// <param name="options">The <see cref="FileOptions"/> additional advanced options to create a file.</param>
        /// <param name="fileSecurity">A <see cref="FileSecurity"/> instance that determines the access control and audit security for the file.</param>
        /// <param name="bufferSize">Size of the buffer.</param>
        /// <returns>A new file with the specified options and buffer size.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope"), SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static FileStream Create(string path, FileMode mode, FileAccess access, FileShare share, FileOptions options, FileSecurity fileSecurity, int bufferSize)
        {
            SafeFileHandle handle = File.CreateInternal(path, mode, (FileSystemRights)access, share, options, fileSecurity);
            return new System.IO.FileStream(handle, ToSystemFileAccess(access), bufferSize, (options & FileOptions.Overlapped) != 0);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope"), SecurityPermission(SecurityAction.LinkDemand)]
        internal static SafeFileHandle CreateInternal(string path, FileMode mode, FileSystemRights access, FileShare share, FileOptions options, FileSecurity fileSecurity)
        {
            if (path == null)
                throw new ArgumentNullException("path");

            SafeGlobalMemoryBufferHandle memory = null;
            PrivilegeEnabler privilegeEnabler = null;
            try
            {
                NativeMethods.SecurityAttributes attr = new NativeMethods.SecurityAttributes();
                attr.Initialize(out memory, fileSecurity);

                if ((access & FileSystemRights.SystemSecurity) != 0)
                    privilegeEnabler = new PrivilegeEnabler(Privilege.Security);

                SafeFileHandle handle = NativeMethods.CreateFileW(path, access, share, attr, mode, options, new SafeGlobalMemoryBufferHandle());
                if (handle.IsInvalid)
                    NativeError.ThrowException(null, path);

                return handle;
            }
            finally
            {
                if (memory != null)
                    memory.Dispose();

                if (privilegeEnabler != null)
                    privilegeEnabler.Dispose();
            }

        }

        #endregion

        #region Transacted

        /// <summary>
        /// Creates or overwrites a file in the specified path as part of a transaction.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        /// <param name="path">The path and name of the file to create.</param>
        /// <returns>
        /// A <see cref="FileStream"/> that provides read/write access to the file specified in path.
        /// </returns>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public static FileStream Create(KernelTransaction transaction, string path)
        {
            // Using default buffer size of 128 here, no idea what would be a good value though.
            return Create(transaction, path, 128, FileOptions.None, null);
        }

        /// <summary>
        /// Creates or overwrites a file in the specified path as part of a transaction.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        /// <param name="path">The path and name of the file to create.</param>
        /// <param name="bufferSize">The number of bytes buffered for reads and writes to the file.</param>
        /// <returns>
        /// A <see cref="FileStream"/> that provides read/write access to the file specified in path.
        /// </returns>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public static FileStream Create(KernelTransaction transaction, string path, int bufferSize)
        {
            return Create(transaction, path, bufferSize, FileOptions.None, null);
        }

        /// <summary>
        /// Creates or overwrites the specified file as part of a transaction, specifying a buffer size and a <see cref="FileOptions"/> value that describes how to create or overwrite the file.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        /// <param name="path">The name of the file.</param>
        /// <param name="bufferSize">The number of bytes buffered for reads and writes to the file.</param>
        /// <param name="options">One of the <see cref="FileOptions"/> values that describes how to create or overwrite the file.</param>
        /// <returns>
        /// A new file with the specified buffer size.
        /// </returns>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public static FileStream Create(KernelTransaction transaction, string path, int bufferSize, FileOptions options)
        {
            return Create(transaction, path, bufferSize, options, null);
        }

        /// <summary>
        /// Creates or overwrites the specified file as part of a transaction, specifying a buffer size, a <see cref="FileOptions"/> value that describes how to create or overwrite the file and a specified <see cref="FileSecurity"/>.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        /// <param name="path">The name of the file.</param>
        /// <param name="bufferSize">The number of bytes buffered for reads and writes to the file.</param>
        /// <param name="options">One of the <see cref="FileOptions"/> values that describes how to create or overwrite the file.</param>
        /// <param name="fileSecurity">A <see cref="FileSecurity"/> instance that determines the access control and audit security for the file.</param>
        /// <returns>
        /// A new file with the specified buffer size and security options.
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope"), SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public static FileStream Create(KernelTransaction transaction, string path, int bufferSize, FileOptions options, FileSecurity fileSecurity)
        {
            if (path == null)
                throw new ArgumentNullException("path");

            SafeFileHandle handle = CreateInternal(transaction, path, FileMode.Create, (FileSystemRights)FileAccess.ReadWrite, FileShare.None, options, fileSecurity);
            return new System.IO.FileStream(handle, System.IO.FileAccess.ReadWrite, bufferSize, (options & FileOptions.Overlapped) != 0);

        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        internal static SafeFileHandle CreateInternal(KernelTransaction transaction, string path, FileMode mode, FileSystemRights access, FileShare share, FileOptions options, FileSecurity fileSecurity)
        {
            if (path == null)
                throw new ArgumentNullException("path");

            SafeGlobalMemoryBufferHandle memory = null;
            try
            {
                NativeMethods.SecurityAttributes attr = new NativeMethods.SecurityAttributes();
                attr.Initialize(out memory, fileSecurity);

                SafeFileHandle handle = NativeMethods.CreateFileTransactedW(path, access, share, attr, mode, options, new SafeGlobalMemoryBufferHandle(), transaction.SafeHandle, IntPtr.Zero, IntPtr.Zero);
                if (handle.IsInvalid)
                    NativeError.ThrowException(null, path);

                return handle;
            }
            finally
            {
                if (memory != null)
                    memory.Dispose();
            }

        }

        #endregion

        #endregion

        #region CreateText

        /// <overloads>
        /// Creates or opens a file for writing UTF-8 encoded text.
        /// </overloads>
        /// <summary>
        /// Creates or opens a file for writing UTF-8 encoded text.
        /// </summary>
        /// <param name="path">The file to be opened for writing. </param>
        /// <returns>A <see cref="StreamWriter"/> that writes to the specified file using UTF-8 encoding.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope"), SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static StreamWriter CreateText(string path)
        {
            return new StreamWriter(Create(path));
        }

        /// <summary>
        /// Creates or opens a file for writing UTF-8 encoded text as part of a transaction.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        /// <param name="path">The file to be opened for writing.</param>
        /// <returns>
        /// A <see cref="StreamWriter"/> that writes to the specified file using UTF-8 encoding.
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope"), SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static StreamWriter CreateText(KernelTransaction transaction, string path)
        {
            return new StreamWriter(Create(transaction, path));
        }

        #endregion

        #region Delete

        /// <overloads>
        /// <summary>
        /// Deletes the specified file.
        /// </summary>
        /// <remarks>
        ///  An exception is not thrown if the specified file does not exist.
        /// </remarks>
        /// </overloads>
        /// <summary>
        /// Deletes the specified file. An exception is not thrown if the specified file does not exist.
        /// </summary>
        /// <param name="path">The name of the file to be deleted.</param>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static void Delete(string path)
        {
            Delete(path, false);
        }

        /// <summary>
        /// Deletes the specified file. An exception is not thrown if the specified file does not exist.
        /// </summary>
        /// <param name="path">The name of the file to be deleted.</param>
        /// <param name="overrideReadOnly">if set to <c>true</c> overrides read only attribute of the file.</param>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static void Delete(string path, bool overrideReadOnly)
        {
            DeleteInternal(null, path, overrideReadOnly);
        }

        /// <summary>
        /// Deletes the specified file as part of a transaction. An exception is not thrown if the specified file does not exist.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        /// <param name="path">The name of the file to be deleted.</param>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static void Delete(KernelTransaction transaction, string path)
        {
            Delete(transaction, path, false);
        }

        /// <summary>
        /// Deletes the specified file as part of a transaction. An exception is not thrown if the specified file does not exist.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        /// <param name="path">The name of the file to be deleted.</param>
        /// <param name="overrideReadOnly">if set to <c>true</c> overrides read only attribute of the file.</param>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static void Delete(KernelTransaction transaction, string path, bool overrideReadOnly)
        {
            if (transaction == null)
                throw new ArgumentNullException("transaction");

            if (path == null)
                throw new ArgumentNullException("path");

            DeleteInternal(transaction, path, overrideReadOnly);
        }

        private static void DeleteInternal(KernelTransaction transaction, string path, bool overrideReadOnly)
        {
            if (path == null)
                throw new ArgumentNullException("path");

            bool result = (transaction == null ? NativeMethods.DeleteFileW(path) : NativeMethods.DeleteFileTransactedW(path, transaction.SafeHandle));

            if (!result)
            {
                switch ((uint)Marshal.GetLastWin32Error())
                {
                    case Win32Errors.ERROR_ACCESS_DENIED:
                        if (overrideReadOnly)
                        {
                            if (transaction == null)
                                SetAttributes(path, FileAttributes.Normal);
                            else
                                SetAttributes(transaction, path, FileAttributes.Normal);
                            DeleteInternal(transaction, path, overrideReadOnly);
                        }
                        break;
                    case Win32Errors.ERROR_FILE_NOT_FOUND:
                    case Win32Errors.ERROR_PATH_NOT_FOUND:
                        // do nothing to comply with default .NET implementation
                        break;
                    default:
                        NativeError.ThrowException(path, path);
                        break;
                }
            }
        }

        #endregion

        #region Encryption

        /// <summary>
        /// Decrypts a file that was encrypted by the current account using the Encrypt method.
        /// </summary>
        /// <param name="path">A path that describes a file to decrypt.</param>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static void Decrypt(string path)
        {
            if (path == null)
                throw new ArgumentNullException("path");

            if (!NativeMethods.DecryptFileW(path, 0))
                NativeError.ThrowException(null, path);
        }

        /// <summary>
        /// Encrypts a file so that only the account used to encrypt the file can decrypt it.
        /// </summary>
        /// <param name="path">A path that describes a file to encrypt.</param>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static void Encrypt(string path)
        {
            if (path == null)
                throw new ArgumentNullException("path");

            if (!NativeMethods.EncryptFileW(path))
                NativeError.ThrowException(null, path);
        }

        /// <summary>
        /// Retrieves the encryption status of the specified file.
        /// </summary>
        /// <param name="fileName">The name of the file.</param>
        /// <returns>The <see cref="FileEncryptionStatus"/> of the specified <paramref name="fileName"/>.</returns>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static FileEncryptionStatus GetEncryptionStatus(string fileName)
        {
            FileEncryptionStatus status;
            if (!NativeMethods.FileEncryptionStatusW(fileName, out status))
                NativeError.ThrowException(fileName, null);
            return status;
        }


        #endregion

        #region Exists

        /// <overloads>
        /// Determines whether the specified file exists.
        /// </overloads>
        /// <summary>
        /// Determines whether the specified file exists.
        /// </summary>
        /// <param name="path">The file to check. Note that this files may contain wildcards, such as '*'.</param>
        /// <returns>true if the caller has the required permissions and path contains the name of an existing file; otherwise, false. This method also returns false if path is nullNothingnullptra null reference (Nothing in Visual Basic), an invalid path, or a zero-length string. If the caller does not have sufficient permissions to read the specified file, no exception is thrown and the method returns false regardless of the existence of path.</returns>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static bool Exists(string path)
        {
            NativeMethods.WIN32_FIND_DATA findData = new NativeMethods.WIN32_FIND_DATA();
            using (SafeFindFileHandle handle = NativeMethods.FindFirstFileExW(path, NativeMethods.FINDEX_INFO_LEVELS.FindExInfoStandard, findData, NativeMethods.FINDEX_SEARCH_OPS.FindExSearchNameMatch, IntPtr.Zero, NativeMethods.FINDEX_FLAGS.FIND_FIRST_EX_NONE))
            {
                if (handle.IsInvalid)
                    return false;

                if ((new FileSystemEntryInfo(findData).IsFile))
                    return true;
                else
                    return false;
            }
        }

        /// <summary>
        /// Determines whether the specified file exists as part of a transaction.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        /// <param name="path">The file to check. Note that this files may contain wildcards, such as '*'.</param>
        /// <returns>
        /// true if the caller has the required permissions and path contains the name of an existing file; otherwise, false. This method also returns false if path is nullNothingnullptra null reference (Nothing in Visual Basic), an invalid path, or a zero-length string. If the caller does not have sufficient permissions to read the specified file, no exception is thrown and the method returns false regardless of the existence of path.
        /// </returns>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static bool Exists(KernelTransaction transaction, string path)
        {
            NativeMethods.WIN32_FIND_DATA findData = new NativeMethods.WIN32_FIND_DATA();
            using (SafeFindFileHandle handle = NativeMethods.FindFirstFileTransactedW(path, NativeMethods.FINDEX_INFO_LEVELS.FindExInfoStandard, findData, NativeMethods.FINDEX_SEARCH_OPS.FindExSearchNameMatch, IntPtr.Zero, NativeMethods.FINDEX_FLAGS.FIND_FIRST_EX_NONE, transaction.SafeHandle))
            {
                if (handle.IsInvalid)
                    return false;

                if ((new FileSystemEntryInfo(findData).IsFile))
                    return true;
                else
                    return false;
            }
        }

        #endregion

        #region Get/Set Access Control

        /// <overloads>
        /// Gets a <see cref="FileSecurity"/> object that encapsulates access control list (ACL) entries for a particular file.
        /// </overloads>
        /// <summary>
        /// Gets a <see cref="FileSecurity"/> object that encapsulates the specified type of access control list (ACL) entries for a particular file.
        /// </summary>
        /// <param name="path">The path to a file containing a <see cref="FileSecurity"/> object that describes the file's access control list (ACL) information.</param>
        /// <returns>
        /// A <see cref="FileSecurity"/> object that encapsulates the access control rules for the file described by the <paramref name="path"/> parameter.
        /// </returns>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static FileSecurity GetAccessControl(string path)
        {
            return GetAccessControl(path, AccessControlSections.Owner | AccessControlSections.Group | AccessControlSections.Access);
        }

        /// <summary>
        /// Gets a <see cref="FileSecurity"/> object that encapsulates the specified type of access control list (ACL) entries for a particular file.
        /// </summary>
        /// <param name="path">The path to a file containing a <see cref="FileSecurity"/> object that describes the file's access control list (ACL) information.</param>
        /// <param name="includeSections">One (or more) of the <see cref="AccessControlSections"/> values that specifies the type of access control list (ACL) information to receive.</param>
        /// <returns>A <see cref="FileSecurity"/> object that encapsulates the access control rules for the file described by the <paramref name="path"/> parameter. </returns>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static FileSecurity GetAccessControl(string path, AccessControlSections includeSections)
        {
            if (path == null)
                throw new ArgumentNullException("path");

            UInt32 securityInfo = 0;
            PrivilegeEnabler privilegeEnabler = null;

            try
            {
                if ((includeSections & AccessControlSections.Access) != 0)
                    securityInfo |= NativeMethods.DACL_SECURITY_INFORMATION;

                if ((includeSections & AccessControlSections.Audit) != 0)
                {
                    // We need the SE_SECURITY_NAME privilege enabled to be able to get the
                    // SACL descriptor. So we enable it here for the reamined of this function.
                    privilegeEnabler = new PrivilegeEnabler(Privilege.Security);
                    securityInfo |= NativeMethods.SACL_SECURITY_INFORMATION;
                }

                if ((includeSections & AccessControlSections.Group) != 0)
                    securityInfo |= NativeMethods.GROUP_SECURITY_INFORMATION;

                if ((includeSections & AccessControlSections.Owner) != 0)
                    securityInfo |= NativeMethods.OWNER_SECURITY_INFORMATION;

                uint sizeRequired = 0;
                SafeGlobalMemoryBufferHandle buffer = new SafeGlobalMemoryBufferHandle(256);
                try
                {
                   if (!NativeMethods.GetFileSecurity(path, securityInfo, buffer, (uint)buffer.Capacity, out sizeRequired))
                   {
                      int lastError = Marshal.GetLastWin32Error();

                      if (sizeRequired > buffer.Capacity)
                      {
                         // A larger buffer was required to store the descriptor, so we increase the size and try again.
                         buffer.Dispose();
                         buffer = null;
                         buffer = new SafeGlobalMemoryBufferHandle((int)sizeRequired);
                         if (!NativeMethods.GetFileSecurity(path, securityInfo, buffer, (uint)buffer.Capacity, out sizeRequired))
                            NativeError.ThrowException(path, null);
                      }
                      else
                      {
                         NativeError.ThrowException(lastError, path, null);
                      }
                   }
                   FileSecurity fs = new FileSecurity();
                   fs.SetSecurityDescriptorBinaryForm(buffer.ToByteArray(0, (int)sizeRequired));
                   return fs;
                }
                finally
                {
                   if (buffer != null)
                     buffer.Dispose();
                }
            }
            finally
            {
                if (privilegeEnabler != null)
                    privilegeEnabler.Dispose();
            }
        }

        /// <summary>
        /// Applies access control list (ACL) entries described by a <see cref="FileSecurity"/> FileSecurity object to the specified file.
        /// </summary>
        /// <remarks>Note that unlike <see cref="System.IO.File.SetAccessControl"/> this method does <b>not</b> automatically
        /// determine what parts of the specified <see cref="FileSecurity"/> instance has been modified. Instead, the
        /// parameter <paramref name="includeSections"/> is used to specify what entries from <paramref name="fileSecurity"/> to 
        /// apply to <paramref name="path"/>.</remarks>
        /// <param name="path">A file to add or remove access control list (ACL) entries from.</param>
        /// <param name="fileSecurity">A  <see cref="FileSecurity"/> object that describes an ACL entry to apply to the file described by the <paramref name="path"/> parameter.</param>
        /// <param name="includeSections">One or more of the <see cref="AccessControlSections"/> values that specifies the type of access control 
        /// list (ACL) information to set.</param>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public static void SetAccessControl(string path, FileSecurity fileSecurity, AccessControlSections includeSections)
        {

            if (path == null)
                throw new ArgumentNullException(path);

            SecurityNativeMethods.SECURITY_INFORMATION info = 0;
            if ((includeSections & AccessControlSections.Access) != 0)
                info |= SecurityNativeMethods.SECURITY_INFORMATION.DACL_SECURITY_INFORMATION;

            if ((includeSections & AccessControlSections.Audit) != 0)
                info |= SecurityNativeMethods.SECURITY_INFORMATION.SACL_SECURITY_INFORMATION;

            if ((includeSections & AccessControlSections.Group) != 0)
                info |= SecurityNativeMethods.SECURITY_INFORMATION.GROUP_SECURITY_INFORMATION;

            if ((includeSections & AccessControlSections.Owner) != 0)
                info |= SecurityNativeMethods.SECURITY_INFORMATION.OWNER_SECURITY_INFORMATION;

            byte[] d = fileSecurity.GetSecurityDescriptorBinaryForm();
            using (SafeGlobalMemoryBufferHandle descriptor = new SafeGlobalMemoryBufferHandle(d.Length))
            {
               descriptor.CopyFrom(d, 0, d.Length);
               if (!NativeMethods.SetFileSecurityW(path, info, descriptor))
                  NativeError.ThrowException(path, path);
            }
        }


        #endregion

        #region Get/Set File Attributes

        /// <summary>
        /// Gets the <see cref="FileSystemEntryInfo"/> of the file on the path.
        /// </summary>
        /// <param name="path">The path to the file.</param>
        /// <returns>The <see cref="FileSystemEntryInfo"/> of the file on the path.</returns>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static FileSystemEntryInfo GetFileSystemEntryInfo(string path)
        {
            return GetFileSystemEntryInfoInternal(null, path);
        }


        /// <summary>
        /// Gets the <see cref="FileSystemEntryInfo"/> of the file on the path.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        /// <param name="path">The path to the file.</param>
        /// <returns>The <see cref="FileSystemEntryInfo"/> of the file on the path.</returns>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static FileSystemEntryInfo GetFileSystemEntryInfo(KernelTransaction transaction, string path)
        {
            if (transaction == null)
                throw new ArgumentNullException("transaction");

            return GetFileSystemEntryInfoInternal(transaction, path);
        }

        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        private static FileSystemEntryInfo GetFileSystemEntryInfoInternal(KernelTransaction transaction, string path)
        {
            if (path == null)
                throw new ArgumentNullException("path");

            NativeMethods.WIN32_FIND_DATA findData = new NativeMethods.WIN32_FIND_DATA();

            using (SafeFindFileHandle handle = (transaction == null ?
                NativeMethods.FindFirstFileExW(path, NativeMethods.FINDEX_INFO_LEVELS.FindExInfoStandard, findData, NativeMethods.FINDEX_SEARCH_OPS.FindExSearchNameMatch, IntPtr.Zero, NativeMethods.FINDEX_FLAGS.FIND_FIRST_EX_NONE) :
                NativeMethods.FindFirstFileTransactedW(path, NativeMethods.FINDEX_INFO_LEVELS.FindExInfoStandard, findData, NativeMethods.FINDEX_SEARCH_OPS.FindExSearchNameMatch, IntPtr.Zero, NativeMethods.FINDEX_FLAGS.FIND_FIRST_EX_NONE, transaction.SafeHandle)))
            {
                // there are couple commong scenarios when it can fail on valid paths like "C:\" and network root share names
                // like \\server\sharename
                // will use GetExtendedAttributes in this case.
                if (handle.IsInvalid)
                {
                    NativeMethods.WIN32_FILE_ATTRIBUTE_DATA attrData = new NativeMethods.WIN32_FILE_ATTRIBUTE_DATA();
                    if (transaction == null? 
                        NativeMethods.GetFileAttributesExW(path, NativeMethods.GET_FILEEX_INFO_LEVELS.GetFileExInfoStandard, out attrData):
                        NativeMethods.GetFileAttributesTransactedW(path, NativeMethods.GET_FILEEX_INFO_LEVELS.GetFileExInfoStandard, out attrData, transaction.SafeHandle))
                    {
                        findData.cFileName = Path.GetFileName(path);
                        findData.dwFileAttributes = attrData.dwFileAttributes;
                        findData.ftCreationTime = attrData.ftCreationTime;
                        findData.ftLastAccessTime = attrData.ftLastAccessTime;
                        findData.ftLastWriteTime = attrData.ftLastWriteTime;
                        findData.nFileSizeHigh = attrData.nFileSizeHigh;
                        findData.nFileSizeLow = attrData.nFileSizeLow;
                        findData.cAlternateFileName = string.Empty;
                    }
                    else
                    {
                        NativeError.ThrowException(path, path);
                    }
                }
                    
            }

            return new FileSystemEntryInfo(findData);
        }

        #region GetAttributes

        /// <overloads>
        /// Gets the <see cref="FileAttributes"/> of the file on the path.
        /// </overloads>
        /// <summary>
        /// Gets the <see cref="FileAttributes"/> of the file on the path.
        /// </summary>
        /// <param name="path">The path to the file. </param>
        /// <returns>The <see cref="FileAttributes"/> of the file on the path.</returns>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static FileAttributes GetAttributes(string path)
        {
            return (GetFileSystemEntryInfo(path)).Attributes;
        }

        /// <summary>
        /// Gets the <see cref="FileAttributes"/> of the file on the path as part of a transaction.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        /// <param name="path">The path to the file.</param>
        /// <returns>
        /// The <see cref="FileAttributes"/> of the file on the path.
        /// </returns>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static FileAttributes GetAttributes(KernelTransaction transaction, string path)
        {
            if (transaction == null)
                throw new ArgumentNullException("transaction");

            return (GetFileSystemEntryInfo(transaction, path)).Attributes;
        }

        #endregion

        #endregion

        /// <overloads>
        /// Sets the attributes for a file or directory.
        /// </overloads>
        /// <summary>
        /// Sets the attributes for a file or directory.
        /// </summary>
        /// <param name="path">The name of the file whose attributes are to be set. </param>
        /// <param name="fileAttributes">The file attributes to set for the file. Note that all other values override <see cref="FileAttributes.Normal"/>.</param>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static void SetAttributes(string path, FileAttributes fileAttributes)
        {
            if (path == null)
                throw new ArgumentNullException("path");

            if (!NativeMethods.SetFileAttributesW(path, fileAttributes))
                NativeError.ThrowException(path, path);
        }

        /// <summary>
        /// Sets the attributes for a file or directory as part of a transaction.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        /// <param name="path">The name of the file whose attributes are to be set.</param>
        /// <param name="fileAttributes">The file attributes to set for the file. Note that all other values override <see cref="FileAttributes.Normal"/>.</param>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static void SetAttributes(KernelTransaction transaction, string path, FileAttributes fileAttributes)
        {
            if (transaction == null)
                throw new ArgumentNullException("transaction");

            if (path == null)
                throw new ArgumentNullException("path");

            if (!NativeMethods.SetFileAttributesTransactedW(path, fileAttributes, transaction.SafeHandle))
                NativeError.ThrowException(path, path);
        }


        #region Get File Times

        #region Non transacted

        /// <overloads>
        /// Returns the creation date and time of the specified file or directory. 
        /// </overloads>
        /// <summary>
        /// Returns the creation date and time of the specified file or directory. 
        /// </summary>
        /// <param name="path">The file or directory for which to obtain creation date and time information. </param>
        /// <returns>A <see cref="DateTime"/> structure set to the creation date and time for the specified file or directory. This value is expressed in local time.</returns>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static DateTime GetCreationTime(string path)
        {
            return DateTime.FromFileTime(GetFileSystemEntryInfo(path).Win32FindData.ftCreationTime.AsLong());
        }

        /// <overloads>
        /// Returns the creation date and time, in coordinated universal time (UTC), of the specified file or directory. 
        /// </overloads>
        /// <summary>
        /// Returns the creation date and time, in coordinated universal time (UTC), of the specified file or directory. 
        /// </summary>
        /// <param name="path">The file or directory for which to obtain creation date and time information. </param>
        /// <returns>A <see cref="DateTime"/> structure set to the creation date and time for the specified file or directory. This value is expressed in UTC time.</returns>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static DateTime GetCreationTimeUtc(string path)
        {
            return DateTime.FromFileTimeUtc(GetFileSystemEntryInfo(path).Win32FindData.ftCreationTime.AsLong());
        }

        /// <overloads>
        /// Returns the date and time the specified file or directory was last accessed. 
        /// </overloads>
        /// <summary>
        /// Returns the date and time the specified file or directory was last accessed. 
        /// </summary>
        /// <param name="path">The file or directory for which to obtain creation date and time information. </param>
        /// <returns>A <see cref="DateTime"/> structure set to the date and time that the specified file or directory was last accessed. This value is expressed in local time.</returns>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static DateTime GetLastAccessTime(string path)
        {
            return DateTime.FromFileTime(GetFileSystemEntryInfo(path).Win32FindData.ftLastAccessTime.AsLong());
        }

        /// <overloads>
        /// Returns the date and time, in coordinated universal time (UTC), that the specified file or directory was last accessed. 
        /// </overloads>
        /// <summary>
        /// Returns the date and time, in coordinated universal time (UTC), that the specified file or directory was last accessed. 
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>A <see cref="DateTime"/> structure set to the date and time that the specified file or directory was last accessed. This value is expressed in UTC time.</returns>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static DateTime GetLastAccessTimeUtc(string path)
        {
            return DateTime.FromFileTimeUtc(GetFileSystemEntryInfo(path).Win32FindData.ftLastAccessTime.AsLong());
        }

        /// <overloads>
        ///Returns the date and time the specified file or directory was last written to. 
        /// </overloads>
        /// <summary>
        ///Returns the date and time the specified file or directory was last written to. 
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>A <see cref="DateTime"/> structure set to the date and time that the specified file or directory was last written to. This value is expressed in local time.</returns>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static DateTime GetLastWriteTime(string path)
        {
            return DateTime.FromFileTime(GetFileSystemEntryInfo(path).Win32FindData.ftLastWriteTime.AsLong());
        }

        /// <overloads>
        /// Returns the date and time, in coordinated universal time (UTC), that the specified file or directory was last written to.
        /// </overloads>
        /// <summary>
        /// Returns the date and time, in coordinated universal time (UTC), that the specified file or directory was last written to.
        /// </summary>
        /// <param name="path">The file or directory for which to obtain write date and time information. </param>
        /// <returns>A <see cref="DateTime"/> structure set to the date and time that the specified file or directory was last written. This value is expressed in UTC time.</returns>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static DateTime GetLastWriteTimeUtc(string path)
        {
            return DateTime.FromFileTimeUtc(GetFileSystemEntryInfo(path).Win32FindData.ftLastWriteTime.AsLong());
        }

        #endregion

        #region Transacted

        /// <summary>
        /// Returns the creation date and time of the specified file or directory as part of a transaction.
        /// </summary>
        /// <param name="transaction">The transaction to use.</param>
        /// <param name="path">The file or directory for which to obtain creation date and time information.</param>
        /// <returns>
        /// A <see cref="DateTime"/> structure set to the creation date and time for the specified file or directory. This value is expressed in local time.
        /// </returns>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static DateTime GetCreationTime(KernelTransaction transaction, string path)
        {
            return DateTime.FromFileTime(GetFileSystemEntryInfo(transaction, path).Win32FindData.ftCreationTime.AsLong());
        }

        /// <summary>
        /// Returns the creation date and time, in coordinated universal time (UTC), of the specified file or directory as part of a transaction. 
        /// </summary>
        /// <param name="path">The file or directory for which to obtain creation date and time information. </param>
        /// <param name="transaction">The transaction to use.</param>
        /// <returns>A <see cref="DateTime"/> structure set to the creation date and time for the specified file or directory. This value is expressed in UTC time.</returns>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static DateTime GetCreationTimeUtc(KernelTransaction transaction, string path)
        {
            return DateTime.FromFileTimeUtc(GetFileSystemEntryInfo(transaction, path).Win32FindData.ftCreationTime.AsLong());
        }

        /// <summary>
        /// Returns the date and time the specified file or directory was last accessed as part of a transaction. 
        /// </summary>
        /// <param name="path">The file or directory for which to obtain creation date and time information. </param>
        /// <param name="transaction">The transaction to use.</param>
        /// <returns>A <see cref="DateTime"/> structure set to the date and time that the specified file or directory was last accessed. This value is expressed in local time.</returns>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static DateTime GetLastAccessTime(KernelTransaction transaction, string path)
        {
            return DateTime.FromFileTime(GetFileSystemEntryInfo(transaction, path).Win32FindData.ftLastAccessTime.AsLong());
        }

        /// <summary>
        /// Returns the date and time, in coordinated universal time (UTC), that the specified file or directory was last accessed as part of a transaction. 
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>A <see cref="DateTime"/> structure set to the date and time that the specified file or directory was last accessed. This value is expressed in UTC time.</returns>
        /// <param name="transaction">The transaction to use.</param>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static DateTime GetLastAccessTimeUtc(KernelTransaction transaction, string path)
        {
            return DateTime.FromFileTimeUtc(GetFileSystemEntryInfo(transaction, path).Win32FindData.ftLastAccessTime.AsLong());
        }

        /// <summary>
        ///Returns the date and time the specified file or directory was last written to as part of a transaction. 
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="transaction">The transaction to use.</param>
        /// <returns>A <see cref="DateTime"/> structure set to the date and time that the specified file or directory was last written to. This value is expressed in local time.</returns>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static DateTime GetLastWriteTime(KernelTransaction transaction, string path)
        {
            return DateTime.FromFileTime(GetFileSystemEntryInfo(transaction, path).Win32FindData.ftLastWriteTime.AsLong());
        }

        /// <summary>
        /// Returns the date and time, in coordinated universal time (UTC), that the specified file or directory was last written to as part of a transaction.
        /// </summary>
        /// <param name="path">The file or directory for which to obtain write date and time information. </param>
        /// <param name="transaction">The transaction to use.</param>
        /// <returns>A <see cref="DateTime"/> structure set to the date and time that the specified file or directory was last written. This value is expressed in UTC time.</returns>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static DateTime GetLastWriteTimeUtc(KernelTransaction transaction, string path)
        {
            return DateTime.FromFileTimeUtc(GetFileSystemEntryInfo(transaction, path).Win32FindData.ftLastWriteTime.AsLong());
        }

        #endregion


        #endregion

        #region Get File Information

        /// <overloads>
        /// Retrieves file information for the specified file.
        /// </overloads>
        /// <summary>
        /// Retrieves file information for the specified file.
        /// </summary>
        /// <param name="stream">A <see cref="FileStream"/> connected to the open file from which to retrieve the information.</param>
        /// <returns>A <see cref="ByHandleFileInfo"/> object containing the requested information.</returns>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static ByHandleFileInfo GetFileInformationByHandle(FileStream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("stream", "Stream is null.");

            return GetFileInformationByHandle(stream.SafeFileHandle);
        }


        /// <summary>
        /// Retrieves file information for the specified file.
        /// </summary>
        /// <param name="handle">A <see cref="SafeFileHandle"/> connected to the open file from which to retrieve the information.</param>
        /// <returns>A <see cref="ByHandleFileInfo"/> object containing the requested information.</returns>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static ByHandleFileInfo GetFileInformationByHandle(SafeFileHandle handle)
        {
            if (handle == null)
                throw new ArgumentNullException("handle");

            if (handle.IsInvalid)
                throw new ArgumentException("Invalid file handle.", "handle");

            if (handle.IsClosed)
                throw new ArgumentException("Cannot retreive information for closed file handle.", "handle");

            ByHandleFileInfo info = new ByHandleFileInfo();
            if (!NativeMethods.GetFileInformationByHandle(handle, info))
                NativeError.ThrowException();
            return info;
        }

        #endregion

        #region Move

        #region Non transacted

        /// <overloads>
        /// Moves a specified file to a new location, providing the option to specify a new file name.
        /// </overloads>
        /// <summary>
        /// Moves a specified file to a new location, providing the option to specify a new file name.
        /// </summary>
        /// <param name="sourceFileName">The name of the file to move.</param>
        /// <param name="destFileName">The new path for the file.</param>
        /// <remarks>This method works across disk volumes, and it does not throw an exception if the source and destination are 
        /// the same. Note that if you attempt to replace a file by moving a file of the same name into that directory, you 
        /// get an IOException. You cannot use the Move method to overwrite an existing file.</remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "dest"), SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static void Move(string sourceFileName, string destFileName)
        {
            Move(sourceFileName, destFileName, MoveFileOptions.CopyAllowed, null, null);
        }

        /// <summary>
        /// Moves a file or directory, including its children. 
        /// </summary>
        /// <param name="sourcePath"><para>The name of the existing file or directory on the local computer.</para>
        /// <para>If <paramref name="options"/> specifies <see cref="MoveFileOptions.DelayUntilReboot"/>, the file cannot exist on 
        /// a remote share because delayed operations are performed before the network is available.</para></param>
        /// <param name="destinationPath">
        /// <para>The new name of the file or directory on the local computer.</para>
        /// <para>When moving a file, <paramref name="destinationPath"/> can be on a different file system or volume. 
        /// If <paramref name="destinationPath"/> is on another drive, you must set the 
        /// <see cref="MoveFileOptions.CopyAllowed"/> flag in <paramref name="options"/>.
        /// </para>
        /// <para>When moving a directory, <paramref name="sourcePath"/> and <paramref name="destinationPath"/> must be on the same drive. </para>
        /// </param>
        /// <param name="options">The move options.</param>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static void Move(string sourcePath, string destinationPath, MoveFileOptions options)
        {
            Move(sourcePath, destinationPath, options, null, null);
        }

        /// <summary>
        /// Moves a file or directory, including its children. You can provide a callback function that receives 
        /// progress notifications.
        /// </summary>
        /// <param name="sourcePath"><para>The name of the existing file or directory on the local computer.</para>
        /// <para>If <paramref name="options"/> specifies <see cref="MoveFileOptions.DelayUntilReboot"/>, the file cannot exist on 
        /// a remote share because delayed operations are performed before the network is available.</para></param>
        /// <param name="destinationPath">
        /// <para>The new name of the file or directory on the local computer.</para>
        /// <para>When moving a file, <paramref name="destinationPath"/> can be on a different file system or volume. 
        /// If <paramref name="destinationPath"/> is on another drive, you must set the 
        /// <see cref="MoveFileOptions.CopyAllowed"/> flag in <paramref name="options"/>.
        /// </para>
        /// <para>When moving a directory, <paramref name="sourcePath"/> and <paramref name="destinationPath"/> must be on the same drive. </para>
        /// </param>
        /// <param name="options">The move options.</param>
        /// <param name="progressRoutine">A <see cref="CopyProgressRoutine"/> callback function that is called each time another 
        /// portion of the file has been moved. The callback function can be useful if you provide a user interface that displays 
        /// the progress of the operation. This parameter can be <see langword="null"/>.</param>
        /// <param name="userProgressData">An argument to be passed to the <see cref="CopyProgressRoutine"/> callback function. This parameter can be <see langword="null"/>.</param>
        /// <returns><c>true</c> if the file was completely moved, <c>false</c> if the operation was aborted.</returns>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static bool Move(string sourcePath, string destinationPath, MoveFileOptions options, CopyProgressRoutine progressRoutine, object userProgressData)
        {
            if (sourcePath == null || destinationPath == null)
                throw new ArgumentNullException(sourcePath == null ? "sourcePath" : "destinationPath");

            if (sourcePath.Length == 0 || destinationPath.Length == 0)
                throw new ArgumentException("Argument empty", sourcePath.Length == 0 ? "sourcePath" : "destinationPath");

            NativeMethods.NativeCopyProgressRoutine routine = null;

            if (progressRoutine != null)
            {
                routine = delegate(long TotalFileSize, long TotalBytesTransferred, long StreamSize, long StreamBytesTransferred, uint dwStreamNumber, CopyProgressCallbackReason dwCallbackReason, IntPtr hSourceFile, IntPtr hDestinationFile, IntPtr lpData)
                {
                    // Note that lpData will be null, so we don't use that for passing user data.
                    // We don't need to since we have access directly to the userProgressData object passed
                    // to this method.
                    return progressRoutine(TotalFileSize, TotalBytesTransferred, StreamSize, StreamBytesTransferred, dwStreamNumber, (CopyProgressCallbackReason)dwCallbackReason, userProgressData);
                };
            }

            if (!NativeMethods.MoveFileWithProgressW(sourcePath, destinationPath, routine, IntPtr.Zero, options))
            {
                uint result = (uint)Marshal.GetLastWin32Error();
                if (result == Win32Errors.ERROR_REQUEST_ABORTED)
                    return false;
                NativeError.ThrowException(result, sourcePath, destinationPath);
            }
            return true;
        }

        #endregion

        #region Transacted

        /// <summary>
        /// Moves a specified file to a new location as part of a transaction, providing the option to specify a new file name.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        /// <param name="sourceFileName">The name of the file to move.</param>
        /// <param name="destFileName">The new path for the file.</param>
        /// <remarks>This method works across disk volumes, and it does not throw an exception if the source and destination are
        /// the same. Note that if you attempt to replace a file by moving a file of the same name into that directory, you
        /// get an IOException. You cannot use the Move method to overwrite an existing file.</remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "dest"), SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static void Move(KernelTransaction transaction, string sourceFileName, string destFileName)
        {
            Move(transaction, sourceFileName, destFileName, MoveFileOptions.CopyAllowed, null, null);
        }

        /// <summary>
        /// Moves a file or directory  as part of a transaction, including its children.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        /// <param name="sourcePath"><para>The name of the existing file or directory on the local computer.</para>
        /// 	<para>If <paramref name="options"/> specifies <see cref="MoveFileOptions.DelayUntilReboot"/>, the file cannot exist on
        /// a remote share because delayed operations are performed before the network is available.</para></param>
        /// <param name="destinationPath"><para>The new name of the file or directory on the local computer.</para>
        /// 	<para>When moving a file, <paramref name="destinationPath"/> can be on a different file system or volume.
        /// If <paramref name="destinationPath"/> is on another drive, you must set the
        /// <see cref="MoveFileOptions.CopyAllowed"/> flag in <paramref name="options"/>.
        /// </para>
        /// 	<para>When moving a directory, <paramref name="sourcePath"/> and <paramref name="destinationPath"/> must be on the same drive. </para></param>
        /// <param name="options">The move options.</param>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static void Move(KernelTransaction transaction, string sourcePath, string destinationPath, MoveFileOptions options)
        {
            Move(transaction, sourcePath, destinationPath, options, null, null);
        }

        /// <summary>
        /// Moves a file or directory as part of a transaction, including its children. You can provide a callback function that receives
        /// progress notifications.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        /// <param name="sourcePath"><para>The name of the existing file or directory on the local computer.</para>
        /// 	<para>If <paramref name="options"/> specifies <see cref="MoveFileOptions.DelayUntilReboot"/>, the file cannot exist on
        /// a remote share because delayed operations are performed before the network is available.</para></param>
        /// <param name="destinationPath"><para>The new name of the file or directory on the local computer.</para>
        /// 	<para>When moving a file, <paramref name="destinationPath"/> can be on a different file system or volume.
        /// If <paramref name="destinationPath"/> is on another drive, you must set the
        /// <see cref="MoveFileOptions.CopyAllowed"/> flag in <paramref name="options"/>.
        /// </para>
        /// 	<para>When moving a directory, <paramref name="sourcePath"/> and <paramref name="destinationPath"/> must be on the same drive. </para></param>
        /// <param name="options">The move options.</param>
        /// <param name="progressRoutine">A <see cref="CopyProgressRoutine"/> callback function that is called each time another
        /// portion of the file has been moved. The callback function can be useful if you provide a user interface that displays
        /// the progress of the operation. This parameter can be <see langword="null"/>.</param>
        /// <param name="userProgressData">An argument to be passed to the <see cref="CopyProgressRoutine"/> callback function. This parameter can be <see langword="null"/>.</param>
        /// <returns>
        /// 	<c>true</c> if the file was completely moved, <c>false</c> if the operation was aborted.
        /// </returns>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static bool Move(KernelTransaction transaction, string sourcePath, string destinationPath, MoveFileOptions options, CopyProgressRoutine progressRoutine, object userProgressData)
        {
            if (sourcePath == null || destinationPath == null)
                throw new ArgumentNullException(sourcePath == null ? "sourcePath" : "destinationPath");

            if (sourcePath.Length == 0 || destinationPath.Length == 0)
                throw new ArgumentException("Argument empty", sourcePath.Length == 0 ? "sourcePath" : "destinationPath");

            NativeMethods.NativeCopyProgressRoutine routine = null;

            if (progressRoutine != null)
            {
                routine = delegate(long TotalFileSize, long TotalBytesTransferred, long StreamSize, long StreamBytesTransferred, uint dwStreamNumber, CopyProgressCallbackReason dwCallbackReason, IntPtr hSourceFile, IntPtr hDestinationFile, IntPtr lpData)
                {
                    // Note that lpData will be null, so we don't use that for passing user data.
                    // We don't need to since we have access directly to the userProgressData object passed
                    // to this method.
                    return progressRoutine(TotalFileSize, TotalBytesTransferred, StreamSize, StreamBytesTransferred, dwStreamNumber, (CopyProgressCallbackReason)dwCallbackReason, userProgressData);
                };
            }

            if (!NativeMethods.MoveFileTransactedW(sourcePath, destinationPath, routine, IntPtr.Zero, options, transaction.SafeHandle))
            {
                uint result = (uint)Marshal.GetLastWin32Error();
                if (result == Win32Errors.ERROR_REQUEST_ABORTED)
                    return false;
                NativeError.ThrowException(sourcePath, destinationPath);
            }
            return true;
        }

        #endregion

        #endregion

        #region Open

        #region Non Transacted

        /// <overloads>
        /// Opens a <see cref="FileStream"/> on the specified path.
        /// </overloads>
        /// <summary>
        /// Opens a <see cref="FileStream"/> on the specified path with read/write access.
        /// </summary>
        /// <param name="path">The file to open.</param>
        /// <param name="mode">A <see cref="FileMode"/> value that specifies whether a file is created if one does not exist, and determines whether the contents of existing files are retained or overwritten.</param>
        /// <returns>A <see cref="FileStream"/> opened in the specified mode and path, with read/write access and not shared.</returns>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static FileStream Open(String path, FileMode mode)
        {
            return Open(path, mode, FileAccess.ReadWrite, FileShare.None);
        }

        /// <summary>
        /// Opens a <see cref="FileStream"/> on the specified path, with the specified mode and access.
        /// </summary>
        /// <param name="path">The file to open.</param>
        /// <param name="mode">A <see cref="FileMode"/> value that specifies whether a file is created if one does not exist, and determines whether the contents of existing files are retained or overwritten.</param>
        /// <param name="access">A <see cref="FileAccess"/> value that specifies the operations that can be performed on the file.</param>
        /// <returns>An unshared <see cref="FileStream"/> that provides access to the specified file, with the specified mode and access.</returns>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static FileStream Open(String path, FileMode mode, FileAccess access)
        {
            return Open(path, mode, access, FileShare.None);
        }

        /// <summary>
        /// Opens a <see cref="FileStream"/> on the specified path, having the specified mode with read, write, or 
        /// read/write access and the specified sharing option.
        /// </summary>
        /// <param name="path">The file to open. </param>
        /// <param name="mode">A <see cref="FileMode"/> value that specifies whether a file is created if one does not exist, and determines whether the contents of existing files are retained or overwritten.</param>
        /// <param name="access">A <see cref="FileMode"/> value that specifies whether a file is created if one does not exist, and determines whether the contents of existing files are retained or overwritten. </param>
        /// <param name="share">A <see cref="FileShare"/> value specifying the type of access other threads have to the file.</param>
        /// <returns>A <see cref="FileStream"/> on the specified path, having the specified mode with read, write, or read/write access and the specified sharing option.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope"), SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static FileStream Open(String path, FileMode mode, FileAccess access, FileShare share)
        {
            SafeFileHandle hFile = NativeMethods.CreateFileW(path, access, share, null, mode, FileOptions.None, new SafeFileHandle(IntPtr.Zero, true));
            if (hFile.IsInvalid)
                NativeError.ThrowException(path, null);

            return new FileStream(hFile, ToSystemFileAccess(access));
        }

        /// <summary>
        /// Opens a <see cref="FileStream"/> on the specified path, having the specified mode with read, write, or
        /// read/write access, the specified sharing option and additional options specified.
        /// </summary>
        /// <param name="path">The file to open.</param>
        /// <param name="mode">A <see cref="FileMode"/> value that specifies whether a file is created if one does not exist, and determines whether the contents of existing files are retained or overwritten.</param>
        /// <param name="access">A <see cref="FileMode"/> value that specifies whether a file is created if one does not exist, 
        /// and determines whether the contents of existing files are retained or overwritten along with additional options.</param>
        /// <param name="share">A <see cref="FileShare"/> value specifying the type of access other threads have to the file.</param>
        /// <param name="options">Advanced options for this file.</param>
        /// <returns>
        /// A <see cref="FileStream"/> on the specified path, having the specified mode with read, write, or read/write access and the specified sharing option.
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope"), SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static FileStream Open(string path, FileMode mode, FileAccess access, FileShare share, FileOptions options)
        {
            SafeFileHandle handle = CreateInternal(path, mode, (FileSystemRights)access, share, options, null);
            if (handle.IsInvalid)
                NativeError.ThrowException(path, null);

            return new FileStream(handle, ToSystemFileAccess(access));
        }

        /// <summary>
        /// Opens a <see cref="FileStream"/> on the specified path, having the specified mode with read, write, or
        /// read/write access, the specified sharing option and additional options specified.
        /// </summary>
        /// <param name="path">The file to open.</param>
        /// <param name="mode">A <see cref="FileMode"/> value that specifies whether a file is created if one does not exist, and determines whether the contents of existing files are retained or overwritten.</param>
        /// <param name="access">A <see cref="FileMode"/> value that specifies whether a file is created if one does not exist, 
        /// and determines whether the contents of existing files are retained or overwritten along with additional options.</param>
        /// <param name="share">A <see cref="FileShare"/> value specifying the type of access other threads have to the file.</param>
        /// <param name="options">Advanced options for this file.</param>
        /// <returns>
        /// A <see cref="FileStream"/> on the specified path, having the specified mode with read, write, or read/write access and the specified sharing option.
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope"), SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static FileStream Open(string path, FileMode mode, FileSystemRights access, FileShare share, FileOptions options)
        {
            SafeFileHandle handle = CreateInternal(path, mode, access, share, options, null);
            if (handle.IsInvalid)
                NativeError.ThrowException(path, null);

            return new FileStream(handle, ToSystemFileAccess((FileAccess)access));
        }

        /// <summary>
        /// Opens the specified file for reading purposes bypassing security attributes.
        /// This method is simpler to use then BackupFileStream to read only file's data stream.
        /// </summary>
        /// <param name="path">The file path to open.</param>
        /// <returns>A <see cref="FileStream"/> on the specified path, having the read-only mode and sharing options.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope"), SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static FileStream OpenBackupRead(String path)
        {
            SafeFileHandle hFile = NativeMethods.CreateFileW(path, FileSystemRights.ReadData, FileShare.None, null, FileMode.Open, FileOptions.BackupSemantics | FileOptions.SequentialScan | FileOptions.ReadOnly, IntPtr.Zero);
            if (hFile.IsInvalid)
                NativeError.ThrowException(path, null);

            return new FileStream(hFile, ToSystemFileAccess(FileAccess.Read));
        }


        #endregion

        #region Transacted

        /// <summary>
        /// Opens a FileStream on the specified path with read/write access as part of a transaction.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        /// <param name="path">The file to open.</param>
        /// <param name="mode">A <see cref="FileMode"/> value that specifies whether a file is created if one does not exist, and determines whether the contents of existing files are retained or overwritten.</param>
        /// <returns>
        /// A <see cref="FileStream"/> opened in the specified mode and path, with read/write access and not shared.
        /// </returns>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static FileStream Open(KernelTransaction transaction, String path, FileMode mode)
        {
            return Open(transaction, path, mode, FileAccess.ReadWrite, FileShare.None);
        }

        /// <summary>
        /// Opens a <see cref="FileStream"/> on the specified path as part of a transaction, with the specified mode and access.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        /// <param name="path">The file to open.</param>
        /// <param name="mode">A <see cref="FileMode"/> value that specifies whether a file is created if one does not exist, and determines whether the contents of existing files are retained or overwritten.</param>
        /// <param name="access">A <see cref="FileAccess"/> value that specifies the operations that can be performed on the file.</param>
        /// <returns>
        /// An unshared <see cref="FileStream"/> that provides access to the specified file, with the specified mode and access.
        /// </returns>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static FileStream Open(KernelTransaction transaction, String path, FileMode mode, FileAccess access)
        {
            return Open(transaction, path, mode, access, FileShare.None);
        }

        /// <summary>
        /// Opens a <see cref="FileStream"/> on the specified path as part of a transaction, having the specified mode with read, write, or
        /// read/write access and the specified sharing option.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        /// <param name="path">The file to open.</param>
        /// <param name="mode">A <see cref="FileMode"/> value that specifies whether a file is created if one does not exist, and determines whether the contents of existing files are retained or overwritten.</param>
        /// <param name="access">A <see cref="FileMode"/> value that specifies whether a file is created if one does not exist, and determines whether the contents of existing files are retained or overwritten.</param>
        /// <param name="share">A <see cref="FileShare"/> value specifying the type of access other threads have to the file.</param>
        /// <returns>
        /// A <see cref="FileStream"/> on the specified path, having the specified mode with read, write, or read/write access and the specified sharing option.
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope"), SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static FileStream Open(KernelTransaction transaction, String path, FileMode mode, FileAccess access, FileShare share)
        {
           SafeFileHandle hFile = CreateInternal(transaction, path, mode, (FileSystemRights)access, share, FileOptions.Normal, null);               
            if (hFile.IsInvalid)
                NativeError.ThrowException(path, null);

            return new FileStream(hFile, ToSystemFileAccess(access));
        }

        /// <summary>
        /// Opens a <see cref="FileStream"/> on the specified path, having the specified mode with read, write, or
        /// read/write access, the specified sharing option and additional options specified.
        /// </summary>
        /// <param name="transaction">The transaction to use.</param>
        /// <param name="path">The file to open.</param>
        /// <param name="mode">A <see cref="FileMode"/> value that specifies whether a file is created if one does not exist, and determines whether the contents of existing files are retained or overwritten.</param>
        /// <param name="access">A <see cref="FileMode"/> value that specifies whether a file is created if one does not exist,
        /// and determines whether the contents of existing files are retained or overwritten along with additional options.</param>
        /// <param name="share">A <see cref="FileShare"/> value specifying the type of access other threads have to the file.</param>
        /// <param name="options">Advanced options for this file.</param>
        /// <returns>
        /// A <see cref="FileStream"/> on the specified path, having the specified mode with read, write, or read/write access and the specified sharing option.
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope"), SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static FileStream Open(KernelTransaction transaction, string path, FileMode mode, FileAccess access, FileShare share, FileOptions options)
        {
            SafeFileHandle handle = CreateInternal(transaction, path, mode, (FileSystemRights)access, share, options, null);
            if (handle.IsInvalid)
                NativeError.ThrowException(path, null);

            return new FileStream(handle, ToSystemFileAccess(access));
        }

        /// <summary>
        /// Opens a <see cref="FileStream"/> on the specified path, having the specified mode with read, write, or
        /// read/write access, the specified sharing option and additional options specified.
        /// </summary>
        /// <param name="transaction">The transaction to use.</param>
        /// <param name="path">The file to open.</param>
        /// <param name="mode">A <see cref="FileMode"/> value that specifies whether a file is created if one does not exist, and determines whether the contents of existing files are retained or overwritten.</param>
        /// <param name="access">A <see cref="FileMode"/> value that specifies whether a file is created if one does not exist,
        /// and determines whether the contents of existing files are retained or overwritten along with additional options.</param>
        /// <param name="share">A <see cref="FileShare"/> value specifying the type of access other threads have to the file.</param>
        /// <param name="options">Advanced options for this file.</param>
        /// <returns>
        /// A <see cref="FileStream"/> on the specified path, having the specified mode with read, write, or read/write access and the specified sharing option.
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope"), SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static FileStream Open(KernelTransaction transaction, string path, FileMode mode, FileSystemRights access, FileShare share, FileOptions options)
        {
            SafeFileHandle handle = CreateInternal(transaction, path, mode, access, share, options, null);
            if (handle.IsInvalid)
                NativeError.ThrowException(path, null);

            return new FileStream(handle, ToSystemFileAccess((FileAccess)access));
        }


        #endregion

        #endregion

        #region OpenRead

        /// <overloads>
        /// Opens an existing file for reading.
        /// </overloads>
        /// <summary>
        /// Opens an existing file for reading.
        /// </summary>
        /// <param name="path">The file to be opened for reading.</param>
        /// <returns>A read-only <see cref="FileStream"/> on the specified path.</returns>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static FileStream OpenRead(string path)
        {
            return Open(path, FileMode.Open, FileAccess.Read, FileShare.Read);
        }

        /// <summary>
        /// Opens an existing file for reading as part of a transaction.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        /// <param name="path">The file to be opened for reading.</param>
        /// <returns>
        /// A read-only <see cref="FileStream"/> on the specified path.
        /// </returns>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static FileStream OpenRead(KernelTransaction transaction, string path)
        {
            return Open(transaction, path, FileMode.Open, FileAccess.Read, FileShare.Read);
        }

        #endregion

        #region OpenText

        /// <overloads>
        /// Opens an existing UTF-8 encoded text file for reading.
        /// </overloads>
        /// <summary>
        /// Opens an existing UTF-8 encoded text file for reading.
        /// </summary>
        /// <param name="path">The file to be opened for reading.</param>
        /// <returns>A StreamReader on the specified path.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope"), SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static StreamReader OpenText(string path)
        {
            return new StreamReader(OpenRead(path));
        }

        /// <summary>
        /// Opens an existing UTF-8 encoded text file for reading as part of a transaction.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        /// <param name="path">The file to be opened for reading.</param>
        /// <returns>A StreamReader on the specified path.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope"), SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static StreamReader OpenText(KernelTransaction transaction, string path)
        {
            return new StreamReader(OpenRead(transaction, path));
        }

        #endregion

        #region OpenWrite

        /// <overloads>
        /// Opens an existing file for writing.
        /// </overloads>
        /// <summary>
        /// Opens an existing file for writing.
        /// </summary>
        /// <param name="path">The file to be opened for writing.</param>
        /// <returns>An unshared <see cref="FileStream"/> object on the specified path with Write access.</returns>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static FileStream OpenWrite(string path)
        {
            return Open(path, FileMode.Open, FileAccess.Write, FileShare.None);
        }

        /// <summary>
        /// Opens an existing file for writing as part of a transaction.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        /// <param name="path">The file to be opened for writing.</param>
        /// <returns>
        /// An unshared <see cref="FileStream"/> object on the specified path with Write access.
        /// </returns>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static FileStream OpenWrite(KernelTransaction transaction, string path)
        {
            return Open(transaction, path, FileMode.Open, FileAccess.Write, FileShare.None);
        }

        #endregion

        #region Transfer Timestamps and security

        /// <summary>
        /// Transfers the time stamps for files and directories.
        /// </summary>
        /// <param name="source">The source path.</param>
        /// <param name="destination">The destination path.</param>
        /// <remarks>
        /// This method uses BackupSemantics flag to get Timestamp changed for folders.
        /// This method does not change last access time for the source file.
        /// </remarks>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static void TransferTimestamps(string source, string destination)
        {
            FileSystemEntryInfo info = GetFileSystemEntryInfo(source);
            SetFileTimeInternal(destination, info.Win32FindData.ftCreationTime.AsLong(), info.Win32FindData.ftLastAccessTime.AsLong(), info.Win32FindData.ftLastWriteTime.AsLong());
        }

        #endregion

        #region ReadAllBytes

        /// <overloads>
        /// Opens a binary file, reads the contents of the file into a byte array, and then closes the file.
        /// </overloads>
        /// <summary>
        /// Opens a binary file, reads the contents of the file into a byte array, and then closes the file.
        /// </summary>
        /// <param name="path">The file to open for reading. </param>
        /// <returns>A byte array containing the contents of the file.</returns>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static byte[] ReadAllBytes(string path)
        {
            byte[] bytes;

            using (FileStream fs = Open(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                int index = 0;
                long fileLength = fs.Length;

                if (fileLength > Int32.MaxValue)
                    throw new System.IO.IOException("File too large");

                int count = (int)fileLength;
                bytes = new byte[count];
                while (count > 0)
                {
                    int n = fs.Read(bytes, index, count);
                    if (n == 0)
                        throw new System.IO.IOException("Unexpected end of file found");
                    index += n;
                    count -= n;
                }
            }
            return bytes;
        }

        /// <summary>
        /// Opens a binary file, reads the contents of the file into a byte array, and then closes the file as part of a transaction.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        /// <param name="path">The file to open for reading.</param>
        /// <returns>
        /// A byte array containing the contents of the file.
        /// </returns>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static byte[] ReadAllBytes(KernelTransaction transaction, string path)
        {
            byte[] bytes;

            using (FileStream fs = Open(transaction, path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                int index = 0;
                long fileLength = fs.Length;

                if (fileLength > Int32.MaxValue)
                    throw new System.IO.IOException("File too large");

                int count = (int)fileLength;
                bytes = new byte[count];
                while (count > 0)
                {
                    int n = fs.Read(bytes, index, count);
                    if (n == 0)
                        throw new System.IO.IOException("Unexpected end of file found");
                    index += n;
                    count -= n;
                }
            }
            return bytes;
        }

        #endregion

        #region ReadAllLines

        /// <overloads>
        /// Opens a text file, reads all lines of the file, and then closes the file.
        /// </overloads>
        /// <summary>
        /// Opens a text file, reads all lines of the file, and then closes the file.
        /// </summary>
        /// <param name="path">The file to open for reading. </param>
        /// <returns>A string array containing all lines of the file.</returns>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static String[] ReadAllLines(String path)
        {
            return ReadAllLines(path, Encoding.UTF8);
        }

        /// <summary>
        /// Opens a file, reads all lines of the file with the specified encoding, and then closes the file.
        /// </summary>
        /// <param name="path">The file to open for reading.</param>
        /// <param name="encoding">The encoding applied to the contents of the file.</param>
        /// <returns>A string array containing all lines of the file.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope"), SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static String[] ReadAllLines(String path, Encoding encoding)
        {
            String line;
            List<string> lines = new List<string>();

            using (StreamReader sr = new StreamReader(Open(path, FileMode.Open, FileAccess.Read, FileShare.Read), encoding))
                while ((line = sr.ReadLine()) != null)
                    lines.Add(line);

            return lines.ToArray();
        }

        /// <summary>
        /// Opens a text file as part of a transaction, reads all lines of the file, and then closes the file.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        /// <param name="path">The file to open for reading.</param>
        /// <returns>
        /// A string array containing all lines of the file.
        /// </returns>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static String[] ReadAllLines(KernelTransaction transaction, String path)
        {
            return ReadAllLines(transaction, path, Encoding.UTF8);
        }

        /// <summary>
        /// Opens a file as part of a transaction, reads all lines of the file with the specified encoding, and then closes the file.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        /// <param name="path">The file to open for reading.</param>
        /// <param name="encoding">The encoding applied to the contents of the file.</param>
        /// <returns>
        /// A string array containing all lines of the file.
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope"), SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static String[] ReadAllLines(KernelTransaction transaction, String path, Encoding encoding)
        {
            String line;
            List<string> lines = new List<string>();

            using (StreamReader sr = new StreamReader(Open(transaction, path, FileMode.Open, FileAccess.Read, FileShare.Read), encoding))
                while ((line = sr.ReadLine()) != null)
                    lines.Add(line);

            return lines.ToArray();
        }
        #endregion

        #region ReadAllText

        /// <overloads>
        /// Opens a text file, reads all lines of the file, and then closes the file.
        /// </overloads>
        /// <summary>
        /// Opens a text file, reads all lines of the file, and then closes the file.
        /// </summary>
        /// <param name="path">The file to open for reading.</param>
        /// <returns>A string containing all lines of the file.</returns>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static String ReadAllText(String path)
        {
            return ReadAllText(path, Encoding.UTF8);
        }

        /// <summary>
        /// Opens a file, reads all lines of the file with the specified encoding, and then closes the file.
        /// </summary>
        /// <param name="path">The file to open for reading.</param>
        /// <param name="encoding">The encoding applied to the contents of the file.</param>
        /// <returns>A string containing all lines of the file.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope"), SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static String ReadAllText(String path, Encoding encoding)
        {
            using (StreamReader sr = new StreamReader(Open(path, FileMode.Open, FileAccess.Read, FileShare.Read), encoding))
                return sr.ReadToEnd();
        }

        /// <summary>
        /// Opens a text file as part of a transaction, reads all lines of the file, and then closes the file.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        /// <param name="path">The file to open for reading.</param>
        /// <returns>
        /// A string containing all lines of the file.
        /// </returns>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static String ReadAllText(KernelTransaction transaction, String path)
        {
            return ReadAllText(transaction, path, Encoding.UTF8);
        }

        /// <summary>
        /// Opens a text file as part of a transaction, reads all lines of the file with the specified encoding, and then closes the file.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        /// <param name="path">The file to open for reading.</param>
        /// <param name="encoding">The encoding applied to the contents of the file.</param>
        /// <returns>
        /// A string containing all lines of the file.
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope"), SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static String ReadAllText(KernelTransaction transaction, String path, Encoding encoding)
        {
            using (StreamReader sr = new StreamReader(Open(transaction, path, FileMode.Open, FileAccess.Read, FileShare.Read), encoding))
                return sr.ReadToEnd();
        }
        #endregion

        #region Replace

        /// <overloads>
        /// Replaces one file with another file, with the option of creating a backup copy of the original file. The replacement file assumes the name of the replaced file and its identity.
        /// </overloads>
        /// <summary>
        /// Replaces one file with another file, with the option of creating a backup copy of the original file. The replacement file assumes the name of the replaced file and its identity.
        /// </summary>
        /// <param name="sourceFileName">The name of a file that replaces the file specified by <paramref name="destinationFileName"/>.</param>
        /// <param name="destinationFileName">The name of the file being replaced.</param>
        /// <param name="destinationBackupFileName">The name of the backup file.</param>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static void Replace(string sourceFileName, string destinationFileName, string destinationBackupFileName)
        {
            Replace(sourceFileName, destinationFileName, destinationBackupFileName, false);
        }

        /// <summary>
        /// Replaces one file with another file, with the option of creating a backup copy of the original file. The replacement file assumes the name of the replaced file and its identity.
        /// </summary>
        /// <param name="sourceFileName">The name of a file that replaces the file specified by <paramref name="destinationFileName"/>.</param>
        /// <param name="destinationFileName">The name of the file being replaced.</param>
        /// <param name="destinationBackupFileName">The name of the backup file.</param>
        /// <param name="ignoreMetadataErrors">set to <c>true</c> to ignore merge errors (such as attributes and access control lists (ACLs)) from the replaced file to the replacement file; otherwise, <c>false</c>.</param>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static void Replace(string sourceFileName, string destinationFileName, string destinationBackupFileName, bool ignoreMetadataErrors)
        {
            if (sourceFileName == null || destinationFileName == null)
                throw new ArgumentNullException(sourceFileName == null ? "sourceFileName" : "destinationFileName");

            if (!NativeMethods.ReplaceFileW(destinationFileName, sourceFileName, destinationBackupFileName, NativeMethods.REPLACEFILE_WRITE_THROUGH | (ignoreMetadataErrors ? NativeMethods.REPLACEFILE_IGNORE_MERGE_ERRORS : 0), IntPtr.Zero, IntPtr.Zero))
            {
                NativeError.ThrowException();
            }
        }


        #endregion

        #region Set file times

        #region Non Transacted

        /// <summary>
        /// Internal method for setting file times on a file.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="creationTime">The creation time.</param>
        /// <param name="lastAccessTime">The last access time.</param>
        /// <param name="lastWriteTime">The last write time.</param>
        /// <remarks>This method uses FileOptions.BackupSemantics flag to write Timestamps to folders as well.</remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope"), SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        private static void SetFileTimeInternal(string path, long? creationTime, long? lastAccessTime, long? lastWriteTime)
        {
            if (path == null)
                throw new ArgumentNullException("path");

            using (SafeGlobalMemoryBufferHandle hCreationTime = SafeGlobalMemoryBufferHandle.CreateFromLong(creationTime))
            using (SafeGlobalMemoryBufferHandle hLastAccessTime = SafeGlobalMemoryBufferHandle.CreateFromLong(lastAccessTime))
            using (SafeGlobalMemoryBufferHandle hLastWriteTime = SafeGlobalMemoryBufferHandle.CreateFromLong(lastWriteTime))
            using (SafeFileHandle hFile = NativeMethods.CreateFileW(path, FileSystemRights.WriteAttributes, FileShare.None, null, FileMode.Open, FileOptions.BackupSemantics, new SafeFileHandle(IntPtr.Zero, false)))
            {
                if (hFile.IsInvalid)
                    NativeError.ThrowException(path, path);
                if (!NativeMethods.SetFileTime(hFile, hCreationTime, hLastAccessTime, hLastWriteTime))
                    NativeError.ThrowException(path, path);
            }
        }

        /// <overloads>
        /// Sets the date and time the file was created.
        /// </overloads>
        /// <summary>
        /// Sets the date and time the file was created.
        /// </summary>
        /// <param name="path">The file for which to set the creation date and time information. </param>
        /// <param name="creationTime">A <see cref="DateTime"/> containing the value to set for the creation date and time of path. This value is expressed in local time. </param>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static void SetCreationTime(string path, DateTime creationTime)
        {
            SetFileTimeInternal(path, creationTime.ToFileTime(), null, null);
        }

        /// <overloads>
        /// Sets the date and time, in coordinated universal time (UTC), that the file was created.
        /// </overloads>
        /// <summary>
        /// Sets the date and time, in coordinated universal time (UTC), that the file was created.
        /// </summary>
        /// <param name="path">The file for which to set the creation date and time information. </param>
        /// <param name="creationTime">A <see cref="DateTime"/> containing the value to set for the creation date and time of path. This value is expressed in UTC time. </param>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static void SetCreationTimeUtc(string path, DateTime creationTime)
        {
            SetFileTimeInternal(path, creationTime.ToFileTimeUtc(), null, null);
        }

        /// <overloads>
        /// Sets the date and time, in local time, that the file was last accessed.
        /// </overloads>
        /// <summary>
        /// Sets the date and time, in local time, that the file was last accessed.
        /// </summary>
        /// <param name="path">The file for which to set the last access date and time information. </param>
        /// <param name="lastAccessTime">A <see cref="DateTime"/> containing the value to set for the last access date and time of path. This value is expressed in local time. </param>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static void SetLastAccessTime(string path, DateTime lastAccessTime)
        {
            SetFileTimeInternal(path, null, lastAccessTime.ToFileTime(), null);
        }

        /// <overloads>
        /// Sets the date and time, in coordinated universal time (UTC), that the file was last accessed.
        /// </overloads>
        /// <summary>
        /// Sets the date and time, in coordinated universal time (UTC), that the file was last accessed.
        /// </summary>
        /// <param name="path">The file for which to set the last access date and time information. </param>
        /// <param name="lastAccessTime">A <see cref="DateTime"/> containing the value to set for the last access date and time of path. This value is expressed in UTC time. </param>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static void SetLastAccessTimeUtc(string path, DateTime lastAccessTime)
        {
            SetFileTimeInternal(path, null, lastAccessTime.ToFileTimeUtc(), null);
        }

        /// <overloads>
        /// Sets the date and time, in local time, that the file was last modified.
        /// </overloads>
        /// <summary>
        /// Sets the date and time, in local time, that the file was last modified.
        /// </summary>
        /// <param name="path">The file for which to set the last modification date and time information. </param>
        /// <param name="lastWriteTime">A <see cref="DateTime"/> containing the value to set for the last modification date and time of path. This value is expressed in local time. </param>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static void SetLastWriteTime(string path, DateTime lastWriteTime)
        {
            SetFileTimeInternal(path, null, null, lastWriteTime.ToFileTime());
        }

        /// <overloads>
        /// Sets the date and time, in coordinated universal time (UTC), that the file was last modified.
        /// </overloads>
        /// <summary>
        /// Sets the date and time, in coordinated universal time (UTC), that the file was last modified.
        /// </summary>
        /// <param name="path">The file for which to set the last modification date and time information. </param>
        /// <param name="lastWriteTime">A <see cref="DateTime"/> containing the value to set for the last modification date and time of path. This value is expressed in UTC time. </param>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static void SetLastWriteTimeUtc(string path, DateTime lastWriteTime)
        {
            SetFileTimeInternal(path, null, null, lastWriteTime.ToFileTimeUtc());
        }

        /// <overloads>
        /// Sets all the time stamps at once.
        /// </overloads>
        /// <summary>
        /// Sets the time stamps at once.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="creationTime">The creation time.</param>
        /// <param name="lastAccessTime">The last access time.</param>
        /// <param name="lastWriteTime">The last write time.</param>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static void SetTimestamps(string path, DateTime creationTime, DateTime lastAccessTime, DateTime lastWriteTime)
        {
            SetFileTimeInternal(path, creationTime.ToFileTime(), lastAccessTime.ToFileTime(), lastWriteTime.ToFileTime());
        }

        /// <overloads>
        /// Sets all the time stamps at once in UTC.
        /// </overloads>
        /// <summary>
        /// Sets all the time stamps at once in UTC.
        /// </summary>
        /// <remarks>
        /// This method is redundant, because NTFS driver converts any dates in UTC format anyways.
        /// </remarks>
        /// <param name="path">The path.</param>
        /// <param name="creationTime">The creation time.</param>
        /// <param name="lastAccessTime">The last access time.</param>
        /// <param name="lastWriteTime">The last write time.</param>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static void SetTimestampsUtc(string path, DateTime creationTime, DateTime lastAccessTime, DateTime lastWriteTime)
        {
            SetFileTimeInternal(path, creationTime.ToFileTimeUtc(), lastAccessTime.ToFileTimeUtc(), lastWriteTime.ToFileTimeUtc());
        }

        #endregion

        #region Transacted

        /// <summary>
        /// Internal method for setting file times on a file as part of a transaction.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        /// <param name="path">The path.</param>
        /// <param name="creationTime">The creation time.</param>
        /// <param name="lastAccessTime">The last access time.</param>
        /// <param name="lastWriteTime">The last write time.</param>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        private static void SetFileTimeInternal(KernelTransaction transaction, string path, long? creationTime, long? lastAccessTime, long? lastWriteTime)
        {
            if (path == null)
                throw new ArgumentNullException("path");

            using (SafeGlobalMemoryBufferHandle hCreationTime = SafeGlobalMemoryBufferHandle.CreateFromLong(creationTime))
            using (SafeGlobalMemoryBufferHandle hLastAccessTime = SafeGlobalMemoryBufferHandle.CreateFromLong(lastAccessTime))
            using (SafeGlobalMemoryBufferHandle hLastWriteTime = SafeGlobalMemoryBufferHandle.CreateFromLong(lastWriteTime))
            using (SafeFileHandle hFile = NativeMethods.CreateFileTransactedW(path, FileSystemRights.WriteAttributes, FileShare.None, null, FileMode.Open, FileOptions.BackupSemantics, null, transaction.SafeHandle, IntPtr.Zero, IntPtr.Zero))
            {
                if (hFile.IsInvalid)
                    NativeError.ThrowException(path, path);

                if (!NativeMethods.SetFileTime(hFile, hCreationTime, hLastAccessTime, hLastWriteTime))
                    NativeError.ThrowException(path, path);
            }
        }

        /// <summary>
        /// Sets the date and time the file was created as part of a transaction.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        /// <param name="path">The file for which to set the creation date and time information.</param>
        /// <param name="creationTime">A <see cref="DateTime"/> containing the value to set for the creation date and time of path. This value is expressed in local time.</param>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static void SetCreationTime(KernelTransaction transaction, string path, DateTime creationTime)
        {
            SetFileTimeInternal(transaction, path, creationTime.ToFileTime(), null, null);
        }

        /// <summary>
        /// Sets the date and time as part of a transaction, in coordinated universal time (UTC), that the file was created.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        /// <param name="path">The file for which to set the creation date and time information.</param>
        /// <param name="creationTime">A <see cref="DateTime"/> containing the value to set for the creation date and time of path. This value is expressed in UTC time.</param>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static void SetCreationTimeUtc(KernelTransaction transaction, string path, DateTime creationTime)
        {
            SetFileTimeInternal(transaction, path, creationTime.ToFileTimeUtc(), null, null);
        }

        /// <summary>
        /// Sets the date and time as part of a transaction, in local time, that the file was last accessed.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        /// <param name="path">The file for which to set the last access date and time information.</param>
        /// <param name="lastAccessTime">The last access time.</param>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static void SetLastAccessTime(KernelTransaction transaction, string path, DateTime lastAccessTime)
        {
            SetFileTimeInternal(transaction, path, null, lastAccessTime.ToFileTime(), null);
        }

        /// <summary>
        /// Sets the date and time as part of a transaction, in coordinated universal time (UTC), that the file was last accessed.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        /// <param name="path">The file for which to set the last access date and time information.</param>
        /// <param name="lastAccessTime">The last access time.</param>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static void SetLastAccessTimeUtc(KernelTransaction transaction, string path, DateTime lastAccessTime)
        {
            SetFileTimeInternal(transaction, path, null, lastAccessTime.ToFileTimeUtc(), null);
        }

        /// <summary>
        /// Sets the date and time as part of a transaction, in local time, that the file was last modified.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        /// <param name="path">The file for which to set the last modification date and time information.</param>
        /// <param name="lastWriteTime">The last write time.</param>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static void SetLastWriteTime(KernelTransaction transaction, string path, DateTime lastWriteTime)
        {
            SetFileTimeInternal(transaction, path, null, null, lastWriteTime.ToFileTime());
        }

        /// <summary>
        /// Sets the date and time as part of a transaction, in coordinated universal time (UTC), that the file was last modified.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        /// <param name="path">The file for which to set the last modification date and time information.</param>
        /// <param name="lastWriteTime">The last write time.</param>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static void SetLastWriteTimeUtc(KernelTransaction transaction, string path, DateTime lastWriteTime)
        {
            SetFileTimeInternal(transaction, path, null, null, lastWriteTime.ToFileTimeUtc());
        }

        /// <summary>
        /// Sets the time stamps at once.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        /// <param name="path">The path.</param>
        /// <param name="creationTime">The creation time.</param>
        /// <param name="lastAccessTime">The last access time.</param>
        /// <param name="lastWriteTime">The last write time.</param>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static void SetTimestamps(KernelTransaction transaction, string path, DateTime creationTime, DateTime lastAccessTime, DateTime lastWriteTime)
        {
            SetFileTimeInternal(transaction, path, creationTime.ToFileTime(), lastAccessTime.ToFileTime(), lastWriteTime.ToFileTime());
        }

        /// <summary>
        /// Sets the time stamps at once in UTC. But it's redundant, because NTFS driver converts any dates in UTC format anyways.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        /// <param name="path">The path.</param>
        /// <param name="creationTime">The creation time.</param>
        /// <param name="lastAccessTime">The last access time.</param>
        /// <param name="lastWriteTime">The last write time.</param>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static void SetTimestampsUtc(KernelTransaction transaction, string path, DateTime creationTime, DateTime lastAccessTime, DateTime lastWriteTime)
        {
            SetFileTimeInternal(transaction, path, creationTime.ToFileTimeUtc(), lastAccessTime.ToFileTimeUtc(), lastWriteTime.ToFileTimeUtc());
        }

        #endregion

        #endregion

        #region WriteAllBytes

        /// <overloads>
        /// Creates a new file, writes the specified byte array to the file, and then closes the file. If the target file already exists, it is overwritten.
        /// </overloads>
        /// <summary>
        /// Creates a new file, writes the specified byte array to the file, and then closes the file. If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="path">The file to write to.</param>
        /// <param name="bytes">The bytes to write to the file.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "bytes"), SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static void WriteAllBytes(string path, byte[] bytes)
        {
            if (bytes == null)
                throw new ArgumentNullException("bytes");

            using (FileStream fs = Open(path, FileMode.Create, FileAccess.Write, FileShare.Read))
                fs.Write(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// Creates a new file as part of a transaction, writes the specified byte array to the file, and then closes the file. If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        /// <param name="path">The file to write to.</param>
        /// <param name="bytes">The bytes to write to the file.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "bytes"), SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static void WriteAllBytes(KernelTransaction transaction, string path, byte[] bytes)
        {
            if (bytes == null)
                throw new ArgumentNullException("bytes");

            using (FileStream fs = Open(transaction, path, FileMode.Create, FileAccess.Write, FileShare.Read))
                fs.Write(bytes, 0, bytes.Length);
        }

        #endregion

        #region WriteAllLines

        /// <overloads>
        /// Creates a new file, write the specified string array to the file, and then closes the file. If the target file already exists, it is overwritten.
        /// </overloads>
        /// <summary>
        /// Creates a new file, write the specified string array to the file, and then closes the file. If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="path">The file to write to.</param>
        /// <param name="contents">The string array to write to the file.</param>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static void WriteAllLines(string path, string[] contents)
        {
            WriteAllLines(path, contents, new UTF8Encoding(false, true)); // special UTF encoding without BOM as been used by .NET itself
        }

        /// <summary>
        /// Creates a new file, writes the specified string array to the file using the specified encoding, and then closes the file. If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="path">The file to write to.</param>
        /// <param name="contents">he string array to write to the file.</param>
        /// <param name="encoding">An <see cref="Encoding"/> object that represents the character encoding applied to the string array.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope"), SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static void WriteAllLines(string path, string[] contents, Encoding encoding)
        {
            if (contents == null)
                throw new ArgumentNullException("contents");

            using (StreamWriter writer = new StreamWriter(Open(path, FileMode.Create, FileAccess.Write, FileShare.Read), encoding))
                foreach (String line in contents)
                    writer.WriteLine(line);
        }

        /// <summary>
        /// Creates a new file as part of a transaction, write the specified string array to the file, and then closes the file. If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        /// <param name="path">The file to write to.</param>
        /// <param name="contents">The string array to write to the file.</param>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static void WriteAllLines(KernelTransaction transaction, string path, string[] contents)
        {
            WriteAllLines(transaction, path, contents, Encoding.UTF8);
        }

        /// <summary>
        /// Creates a new file as part of a transaction, writes the specified string array to the file using the specified encoding, and then closes the file. If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        /// <param name="path">The file to write to.</param>
        /// <param name="contents">he string array to write to the file.</param>
        /// <param name="encoding">An <see cref="Encoding"/> object that represents the character encoding applied to the string array.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope"), SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static void WriteAllLines(KernelTransaction transaction, string path, string[] contents, Encoding encoding)
        {
            if (contents == null)
                throw new ArgumentNullException("contents");

            using (StreamWriter writer = new StreamWriter(Open(transaction, path, FileMode.Create, FileAccess.Write, FileShare.Read), encoding))
                foreach (String line in contents)
                    writer.WriteLine(line);
        }

        #endregion

        #region WriteAllText

        /// <overloads>
        /// Creates a new file, write the contents to the file, and then closes the file. If the target file already exists, it is overwritten.
        /// </overloads>
        /// <summary>
        /// Creates a new file, write the contents to the file, and then closes the file. If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="path">The file to write to.</param>
        /// <param name="contents">The string to write to the file.</param>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static void WriteAllText(string path, string contents)
        {
            WriteAllText(path, contents, Encoding.UTF8);
        }

        /// <summary>
        /// Creates a new file, writes the specified string to the file using the specified encoding, and then closes the file. If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="path">The file to write to.</param>
        /// <param name="contents">The string to write to the file.</param>
        /// <param name="encoding">An <see cref="Encoding"/> object that represents the encoding to apply to the string.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope"), SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static void WriteAllText(string path, string contents, Encoding encoding)
        {
            using (StreamWriter writer = new StreamWriter(Open(path, FileMode.Create, FileAccess.Write, FileShare.Read), encoding))
                writer.Write(contents);
        }

        /// <summary>
        /// Creates a new file as part of a transaction, write the contents to the file, and then closes the file. If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        /// <param name="path">The file to write to.</param>
        /// <param name="contents">The string to write to the file.</param>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static void WriteAllText(KernelTransaction transaction, string path, string contents)
        {
            WriteAllText(transaction, path, contents, Encoding.UTF8);
        }

        /// <summary>
        /// Creates a new file as part of a transaction, writes the specified string to the file using the specified encoding, and then closes the file. If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        /// <param name="path">The file to write to.</param>
        /// <param name="contents">The string to write to the file.</param>
        /// <param name="encoding">An <see cref="Encoding"/> object that represents the encoding to apply to the string.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope"), SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static void WriteAllText(KernelTransaction transaction, string path, string contents, Encoding encoding)
        {
            using (StreamWriter writer = new StreamWriter(Open(transaction, path, FileMode.Create, FileAccess.Write, FileShare.Read), encoding))
                writer.Write(contents);
        }

        #endregion

        #region Links

        #region Non Transacted

        /// <overloads>
        /// Establishes a hard link between an existing file and a new file. This function is only supported on the NTFS file system, and only for files, not directories.
        /// </overloads>
        /// <summary>
        /// Establishes a hard link between an existing file and a new file. This function is only supported on the NTFS file system, and only for files, not directories.
        /// </summary>
        /// <param name="sourceFile">The source file.</param>
        /// <param name="destinationFile">The destination file.</param>
        /// <exception cref="AlreadyExistsException">The destination file already exists.</exception>
        /// <exception cref="NotSupportedException">An attempt to create a hard-link on a non-supported filesystem</exception>
        /// <exception cref="System.IO.FileNotFoundException"><paramref name="sourceFile"/> could not be found</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Hardlink"), EnvironmentPermissionAttribute(SecurityAction.LinkDemand, Unrestricted = true)]
        public static void CreateHardlink(string sourceFile, string destinationFile)
        {
            if (!NativeMethods.CreateHardLinkW(destinationFile, sourceFile, IntPtr.Zero))
            {
                uint result = (uint)Marshal.GetLastWin32Error();
                if (result == Win32Errors.ERROR_INVALID_FUNCTION)
                    throw new NotSupportedException(Resources.HardLinksOnNonNTFSPartitionsIsNotSupported);

                NativeError.ThrowException(result, sourceFile, destinationFile);
            }
        }

        /// <summary>
        /// Establishes a hard link between an existing file and a new file. This function is only supported on the NTFS file system, and only for files, not directories.
        /// </summary>
        /// <param name="sourceFile">The source file.</param>
        /// <param name="destinationFile">The destination file.</param>
        /// <param name="transaction">The transaction to use.</param>
        /// <exception cref="AlreadyExistsException">The destination file already exists.</exception>
        /// <exception cref="NotSupportedException">An attempt to create a hard-link on a non-supported filesystem</exception>
        /// <exception cref="System.IO.FileNotFoundException"><paramref name="sourceFile"/> could not be found</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Hardlink"), SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static void CreateHardlink(KernelTransaction transaction, string sourceFile, string destinationFile)
        {
            if (!NativeMethods.CreateHardLinkTransactedW(destinationFile, sourceFile, IntPtr.Zero, transaction.SafeHandle))
            {
                uint result = (uint)Marshal.GetLastWin32Error();
                if (result == Win32Errors.ERROR_INVALID_FUNCTION)
                    throw new NotSupportedException(Resources.HardLinksOnNonNTFSPartitionsIsNotSupported);

                NativeError.ThrowException(result, sourceFile, destinationFile);
            }
        }

        /// <overloads>
        /// Creates a symbolic link.
        /// </overloads>		
        /// <summary>
        /// Creates a symbolic link.
        /// </summary>
        /// <param name="sourceFile">The name of the target for the symbolic link to be created.</param>
        /// <param name="destinationFile">The symbolic link to be created.</param>
        /// <param name="targetType">Indicates whether the link target, <paramref name="destinationFile"/>, is a file or directory.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Hardlink"), EnvironmentPermissionAttribute(SecurityAction.LinkDemand, Unrestricted = true)]
        public static void CreateSymbolicLink(string sourceFile, string destinationFile, SymbolicLinkTarget targetType)
        {
            if (!NativeMethods.CreateSymbolicLinkW(destinationFile, sourceFile, targetType))
            {
                NativeError.ThrowException(sourceFile, destinationFile);
            }
        }

        #endregion

        #region Transacted Links

        /// <summary>
        /// Creates a symbolic link as part of a transaction.
        /// </summary>
        /// <param name="sourceFile">The name of the target for the symbolic link to be created.</param>
        /// <param name="destinationFile">The symbolic link to be created.</param>
        /// <param name="targetType">Indicates whether the link target, <paramref name="destinationFile"/>, is a file or directory.</param>
        /// <param name="transaction">The transaction to use.</param>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static void CreateSymbolicLink(KernelTransaction transaction, string sourceFile, string destinationFile, SymbolicLinkTarget targetType)
        {
            if (!NativeMethods.CreateSymbolicLinkTransactedW(destinationFile, sourceFile, targetType, transaction.SafeHandle))
            {
                NativeError.ThrowException(sourceFile, destinationFile);
            }

        }

        /// <summary>
        /// Gets information about the target of a mount point or symbolic link on an NTFS file system.
        /// </summary>
        /// <param name="sourceFile">The path to the reparse point.</param>
        /// <returns>An instance of <see cref="LinkTargetInfo"/> or <see cref="SymbolicLinkTargetInfo"/> containing
        /// information about the symbolic link or mount point pointed to by <paramref name="sourceFile"/>.</returns>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static LinkTargetInfo GetLinkTargetInfo(string sourceFile)
        {
            using (SafeHandle device = DeviceIo.CreateFile(sourceFile, FileAccess.None, FileShare.ReadWrite, FileMode.Open, FileOptions.OpenReparsePoint | FileOptions.BackupSemantics))
            {
                if (device.IsInvalid)
                    NativeError.ThrowException();

                LinkTargetInfo info = DeviceIo.GetLinkTargetInfo(device);
                return info;
            }
        }

        /// <summary>
        /// Gets information about the target of a mount point or symbolic link on an NTFS file system as part of a transaction.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        /// <param name="sourceFile">The path to the reparse point.</param>
        /// <returns>
        /// An instance of <see cref="LinkTargetInfo"/> or <see cref="SymbolicLinkTargetInfo"/> containing
        /// information about the symbolic link or mount point pointed to by <paramref name="sourceFile"/>.
        /// </returns>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static LinkTargetInfo GetLinkTargetInfo(KernelTransaction transaction, string sourceFile)
        {
            using (SafeHandle device = DeviceIo.CreateFile(transaction, sourceFile, FileAccess.None, FileShare.ReadWrite, FileMode.Open, FileOptions.OpenReparsePoint | FileOptions.BackupSemantics))
            {
                if (device.IsInvalid)
                    NativeError.ThrowException();

                LinkTargetInfo info = DeviceIo.GetLinkTargetInfo(device);
                return info;

            }
        }

        #endregion

        #endregion

        #region GetHardlinks

        /// <overloads>
        /// <summary>Enumerates all hard links to the specified file.</summary>
        /// </overloads>
        /// <summary>
        /// Creates an enumeration of all the hard links to the specified <paramref name="fileName"/>. 
        /// </summary>
        /// <remarks><b>Required Windows Vista or later.</b></remarks>
        /// <param name="fileName">The name of the file.</param>
        /// <returns>An enumeration of all the hard links to the specified <paramref name="fileName"/></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Hardlinks")]
        public static IEnumerable<string> GetHardlinks(string fileName)
        {
            return GetHardlinksInternal(
                delegate(ref UInt32 length, StringBuilder linkName)
                {
                    return NativeMethods.FindFirstFileNameW(fileName, 0, ref length, linkName);
                }
            );

        }

        /// <summary>
        /// Creates an enumeration of all the hard links to the specified <paramref name="fileName"/> as part
        /// of the specified <paramref name="transaction"/>.
        /// </summary>
        /// <param name="transaction">The transaction to use.</param>
        /// <param name="fileName">The name of the file.</param>
        /// <returns>
        /// An enumeration of all the hard links to the specified <paramref name="fileName"/>
        /// </returns>
        /// <remarks><b>Required Windows Vista or later.</b></remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Hardlinks")]
        public static IEnumerable<string> GetHardlinks(KernelTransaction transaction, string fileName)
        {
            return GetHardlinksInternal(
                delegate(ref UInt32 length, StringBuilder linkName)
                {
                    return NativeMethods.FindFirstFileNameTransactedW(fileName, 0, ref length, linkName, transaction.SafeHandle);
                }
            );
        }

        private delegate SafeFindFileHandle FindFirstFileNameFunction(ref UInt32 length, StringBuilder linkName);

        private static IEnumerable<string> GetHardlinksInternal(FindFirstFileNameFunction findFirstFileName)
        {
            // Default buffer length, will be extended if needed.
            UInt32 length = 256;
            StringBuilder builder = new StringBuilder(256);
            bool tryAgain = false;

            do
            {
                using (SafeFindFileHandle handle = findFirstFileName(ref length, builder))
                {
                    if (handle.IsInvalid)
                    {
                        int error = Marshal.GetLastWin32Error();
                        if (error == Win32Errors.ERROR_MORE_DATA && tryAgain == false) // We only want to try again once!
                        {
                            builder.EnsureCapacity((int)length);
                            tryAgain = true;
                            continue;
                        }
                        else
                        {
                            NativeError.ThrowException(error);
                        }
                    }

                    yield return builder.ToString();

                    tryAgain = false; // We should not try the outer loop again if it succeeded once.
                    bool innerTryAgain = false;
                    bool hasMore = false;
                    do
                    {
                        builder.Length = 0;
                        hasMore = NativeMethods.FindNextFileNameW(handle, ref length, builder);
                        if (!hasMore)
                        {
                            int error = Marshal.GetLastWin32Error();
                            if (error == Win32Errors.ERROR_MORE_DATA && innerTryAgain == false)
                            {
                                // Buffer needs more space
                                builder.EnsureCapacity((int)length);
                                innerTryAgain = true;
                                continue;
                            }
                            else if (error == Win32Errors.ERROR_HANDLE_EOF)
                            {
                                // We've reached the end of the enumeration.
                                yield break;
                            }
                            else
                            {
                                // An unexpected error occured
                                NativeError.ThrowException(error);
                            }
                        }
                        else
                        {
                            innerTryAgain = false;
                        }

                        yield return builder.ToString();
                    }
                    while (hasMore || innerTryAgain);
                }
            }
            while (tryAgain);
        }

        #endregion

        #region Compressed Files

        /// <overloads>
        /// Retrieves the actual number of bytes of disk storage used to store a specified file. 
        /// </overloads>
        /// <summary>
        /// Retrieves the actual number of bytes of disk storage used to store a specified file. 
        /// </summary>
        /// <remarks>
        /// If the file is located on a volume that
        /// supports compression and the file is compressed, the value obtained is the compressed size of the specified file.
        /// If the file is located on a volume that supports sparse files and the file is a sparse file, the value obtained is the sparse
        /// size of the specified file.
        /// </remarks>		
        /// <param name="fileName"><para>The name of the file.</para>
        /// 	<para>Do not specify the name of a file on a nonseeking device, such as a pipe or a communications device, as its file size has no meaning.</para></param>
        /// <returns>
        /// the actual number of bytes of disk storage used to store the specified file.
        /// </returns>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static long GetCompressedSize(string fileName)
        {
            if (fileName == null)
                throw new ArgumentNullException("fileName");

            UInt32 fileSizeHigh;
            UInt32 fileSizeLow = NativeMethods.GetCompressedFileSizeW(fileName, out fileSizeHigh);
            int errorCode = Marshal.GetLastWin32Error();
            if (errorCode != Win32Errors.NO_ERROR)
                NativeError.ThrowException(errorCode, fileName, null);

            return (((long)fileSizeHigh) << 32) + fileSizeLow;

        }

        /// <summary>
        /// Retrieves the actual number of bytes of disk storage used to store a specified file as part of a transaction. If the file is located on a volume that
        /// supports compression and the file is compressed, the value obtained is the compressed size of the specified file.
        /// If the file is located on a volume that supports sparse files and the file is a sparse file, the value obtained is the sparse
        /// size of the specified file.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        /// <param name="fileName"><para>The name of the file.</para>
        /// 	<para>Do not specify the name of a file on a nonseeking device, such as a pipe or a communications device, as its file size has no meaning.</para></param>
        /// <returns>
        /// the actual number of bytes of disk storage used to store the specified file.
        /// </returns>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static long GetCompressedSize(KernelTransaction transaction, string fileName)
        {
            if (fileName == null)
                throw new ArgumentNullException("fileName");

            UInt32 fileSizeHigh;
            UInt32 fileSizeLow = NativeMethods.GetCompressedFileSizeTransactedW(fileName, out fileSizeHigh, transaction.SafeHandle);
            int errorCode = Marshal.GetLastWin32Error();
            if (errorCode != Win32Errors.NO_ERROR)
                NativeError.ThrowException(errorCode, fileName, null);

            return (((long)fileSizeHigh) << 32) + fileSizeLow;

        }

        #endregion

        #region Internal Utility Methods

        internal static System.IO.FileAccess ToSystemFileAccess(FileAccess fileAccess)
        {
            return (System.IO.FileAccess)fileAccess;
        }

        #endregion

    }
}
