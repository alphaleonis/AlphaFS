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

namespace Alphaleonis.Win32.Filesystem
{
    /// <summary>
    /// Represents information about a volume.
    /// </summary>
    public class VolumeInfo
    {
        #region Public properties

        /// <summary>
        /// Gets the name of the volume.
        /// </summary>
        /// <value>The name of the volume.</value>
        public string Name
        {
            get { return mName; }
        }

        /// <summary>
        /// Gets a value indicating whether the file system preserves the case of file names when it places a name on disk..
        /// </summary>
        /// <value><c>true</c> if the file system preserves the case of file names when it places a name on disk.]; otherwise, <c>false</c>.</value>
        public bool PreservesCase
        {
            get { return GetFlag(NativeMethods.FILE_CASE_PRESERVED_NAMES); }
        }

        /// <summary>
        /// Gets a value indicating whether the file system supports case-sensitive file names..
        /// </summary>
        /// <value>
        /// 	<c>true</c> if the file system supports case-sensitive file names; otherwise, <c>false</c>.
        /// </value>
        public bool SupportsCaseSensitiveFileNames
        {
            get { return GetFlag(NativeMethods.FILE_CASE_SENSITIVE_SEARCH); }
        }

        /// <summary>
        /// Gets a value indicating whether the file system supports file-based compression.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if the file system supports file-based compression; otherwise, <c>false</c>.
        /// </value>
        public bool SupportsFileCompression
        {
            get { return GetFlag(NativeMethods.FILE_FILE_COMPRESSION); }
        }

        /// <summary>
        /// Gets a value indicating whether the file system supports named streams.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if the file system supports named streams; otherwise, <c>false</c>.
        /// </value>
        public bool SupportsNamedStreams
        {
            get { return GetFlag(NativeMethods.FILE_NAMED_STREAMS); }
        }

        /// <summary>
        /// Gets a value indicating whether the file system preserves and enforces access control lists (ACL).
        /// </summary>
        /// <value>
        /// 	<c>true</c> if the file system preserves and enforces access control lists (ACL); otherwise, <c>false</c>.
        /// </value>
        /// <remarks>For example, the NTFS file system preserves and enforces ACLs, and the FAT file system does not.</remarks>
        public bool HasPersistentAccessControlLists
        {
            get { return GetFlag(NativeMethods.FILE_PERSISTENT_ACLS); }
        }

        /// <summary>
        /// Gets a value indicating whether the specified volume is read-only..
        /// </summary>
        /// <value>
        /// 	<c>true</c> if the specified volume is read-only.; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>This value is not supported on Windows 2000/NT and Windows Me/98/95.</remarks>
        public bool IsReadOnly
        {
            get { return GetFlag(NativeMethods.FILE_READ_ONLY_VOLUME); }
        }

        /// <summary>
        /// Gets a value indicating whether the file system supports the Encrypted File System (EFS).
        /// </summary>
        /// <value><c>true</c> if the file system supports the Encrypted File System (EFS); otherwise, <c>false</c>.</value>
        public bool SupportsEncryption
        {
            get { return GetFlag(NativeMethods.FILE_SUPPORTS_ENCRYPTION); }
        }

        /// <summary>
        /// Gets a value indicating whether the file system supports object identifiers.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if the file system supports object identifiers; otherwise, <c>false</c>.
        /// </value>
        public bool SupportsObjectIdentifiers
        {
            get { return GetFlag(NativeMethods.FILE_SUPPORTS_OBJECT_IDS); }
        }

        /// <summary>
        /// Gets a value indicating whether he file system supports re-parse points.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if he file system supports re-parse points; otherwise, <c>false</c>.
        /// </value>
        public bool SupportsReparsePoints
        {
            get { return GetFlag(NativeMethods.FILE_SUPPORTS_REPARSE_POINTS); }
        }

        /// <summary>
        /// Gets a value indicating whether the file system supports sparse files.
        /// </summary>
        /// <value><c>true</c> if the file system supports sparse files; otherwise, <c>false</c>.</value>
        public bool SupportsSparseFiles
        {
            get { return GetFlag(NativeMethods.FILE_SUPPORTS_SPARSE_FILES); }
        }

        /// <summary>
        /// Gets a value indicating whether the file system supports Unicode in file names as they appear on disk.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if the file system supports Unicode in file names as they appear on disk; otherwise, <c>false</c>.
        /// </value>
        public bool SupportsUnicodeFileNames
        {
            get { return GetFlag(NativeMethods.FILE_UNICODE_ON_DISK); }
        }

        /// <summary>
        /// Gets a value indicating whether the specified volume is a compressed volume, for example, a DoubleSpace volume.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if the specified volume is a compressed volume; otherwise, <c>false</c>.
        /// </value>
        public bool IsCompressed
        {
            get { return GetFlag(NativeMethods.FILE_VOLUME_IS_COMPRESSED); }
        }

        /// <summary>
        /// Gets a value indicating whether the file system supports disk quotas.
        /// </summary>
        /// <value><c>true</c> if the file system supports disk quotas; otherwise, <c>false</c>.</value>
        public bool SupportsDiskQuotas
        {
            get { return GetFlag(NativeMethods.FILE_VOLUME_QUOTAS); }
        }


        /// <summary>
        /// Gets the volume serial number that the operating system assigns when a hard disk is formatted.
        /// </summary>
        /// <value>The volume serial number that the operating system assigns when a hard disk is formatted.</value>
        public long SerialNumber
        {
            get { return mSerialNumber; }
        }

        /// <summary>
        /// Gets the maximum length of a file name component that the file system supports.
        /// </summary>
        /// <value>The maximum length of a file name component that the file system supports.</value>
        public int MaximumComponentLength
        {
            get { return mMaximumComponentLength; }
        }

        /// <summary>
        /// Gets the name of the file system, for example, the FAT file system or the NTFS file system.
        /// </summary>
        /// <value>The name of the file system.</value>
        public string FileSystemName
        {
            get { return mFileSystemName; }
        }

        #endregion

        #region Private methods

        private bool GetFlag(uint flag)
        {
            return (mFileSystemFlags & flag) == flag;
        }

        #endregion

        #region Private fields

        private string mName;
        private uint mFileSystemFlags;
        private long mSerialNumber;
        private int mMaximumComponentLength;
        private string mFileSystemName;

        #endregion

        internal VolumeInfo(string name, uint fileSystemFlags, long serialNumber, int maximumComponentLength, string fileSystemName)
        {
            mName = name;
            mFileSystemFlags = fileSystemFlags;
            mSerialNumber = serialNumber;
            mMaximumComponentLength = maximumComponentLength;
            mFileSystemName = fileSystemName;
        }
    }
}
