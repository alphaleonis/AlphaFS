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

namespace Alphaleonis.Win32.Filesystem
{
    /// <summary>
    /// Specifies how the operating system should open a file.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32"), Flags]
    public enum FileOptions : uint
    {
        /// <summary>
        /// None of the options specified.
        /// </summary>
        None                = 0x00,
        /// <summary>
        /// The file should be archived. Applications use this attribute to mark files for backup or removal.
        /// </summary>
        Archive = 0x20,
        /// <summary>
        /// The file or directory is encrypted. For a file, this means that all data in the file is encrypted. For a directory, this means that encryption is the default for newly created files and subdirectories. 
        /// </summary>
        Encrypted           = 0x4000,
        /// <summary>
        /// The file is hidden. Do not include it in an ordinary directory listing.
        /// </summary>
        Hidden              = 0x02,
        /// <summary>
        /// The file does not have other attributes set. This attribute is valid only if used alone.
        /// </summary>
        Normal              = 0x80,
        /// <summary>
        /// The data of a file is not immediately available. This attribute indicates that file data is physically moved to offline storage. This attribute is used by Remote Storage, the hierarchical storage management software. Applications should not arbitrarily change this attribute.
        /// </summary>
        Offline             = 0x1000,
        /// <summary>
        /// The file is read only. Applications can read the file, but cannot write to or delete it.
        /// </summary>
        ReadOnly            = 0x01,
        /// <summary>
        /// The file is part of or used exclusively by an operating system.
        /// </summary>
        System              = 0x04,
        /// <summary>
        /// The file is being used for temporary storage.
        /// </summary>
        Temporary           = 0x100,
        /// <summary>
        /// The file is being opened or created for a backup or restore operation. The system ensures that the calling process 
        /// overrides file security checks when the process has SE_BACKUP_NAME and SE_RESTORE_NAME privileges. 
        /// You must set this flag to obtain a handle to a directory. A directory handle can be passed to some functions instead of a file handle. 
        /// </summary>
        BackupSemantics     = 0x02000000,
        /// <summary>
        /// <para>The file is to be deleted immediately after all of its handles are closed, which includes the specified handle and any other open or duplicated handles.</para>
        /// <para>If there are existing open handles to a file, the call fails unless they were all opened with the <see cref="FileShare.Delete"/> share mode.</para>
        /// <para>Subsequent open requests for the file fail, unless the <see cref="FileShare.Delete"/> share mode is specified.</para>
        /// </summary>
        DeleteOnClose       = 0x04000000,
        /// <summary>
        /// There are strict requirements for successfully working with files opened with the <see cref="NoBuffering"/> flag, for 
        /// details see the section on "File Buffering" in the online MSDN documentation.
        /// </summary>
        NoBuffering         = 0x20000000,
        /// <summary>
        /// The file data is requested, but it should continue to be located in remote storage. It should not be transported back to local storage. This flag is for use by remote storage systems.
        /// </summary>
        OpenNoRecall        = 0x00100000,
        /// <summary>
        /// <para>Normal reparse point processing will not occur; an attempt to open the reparse point will be made. 
        /// When a file is opened, a file handle is returned, whether or not the filter that controls the reparse 
        /// point is operational. See MSDN documentation for more information.</para>
        /// </summary>
        OpenReparsePoint    = 0x00200000,
        /// <summary>
        /// The file or device is being opened or created for asynchronous I/O.
        /// </summary>
        Overlapped          = 0x40000000,
        /// <summary>
        /// Access will occur according to POSIX rules. This includes allowing multiple files with names, differing only in case, for file systems that support that naming. Use care when using this option, because files created with this flag may not be accessible by applications that are written for MS-DOS or 16-bit Windows.
        /// </summary>
        PosixSemantics      = 0x01000000,
        /// <summary>
        /// Access is intended to be random. The system can use this as a hint to optimize file caching.
        /// </summary>
        RandomAccess        = 0x10000000,
        /// <summary>
        /// Access is intended to be sequential from beginning to end. The system can use this as a hint to optimize file caching. 
        /// </summary>
        SequentialScan      = 0x08000000,
        /// <summary>
        /// Write operations will not go through any intermediate cache, they will go directly to disk.
        /// </summary>
        WriteThrough        = 0x80000000
    }
}
