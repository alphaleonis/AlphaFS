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
using System.Text;
using Microsoft.Win32.SafeHandles;

namespace Alphaleonis.Win32.Filesystem
{
	internal static class NativeMethods
	{
		#region Constants

		#region Dos Device Flags (Used by DefineDosDevice)

		// Used by DefineDosDevice 
		internal const uint DDD_RAW_TARGET_PATH = 0x00000001;
		internal const uint DDD_REMOVE_DEFINITION = 0x00000002;
		internal const uint DDD_EXACT_MATCH_ON_REMOVE = 0x00000004;
		internal const uint DDD_NO_BROADCAST_SYSTEM = 0x00000008;

		#endregion

		#region File System Flags (Used by GetVolumeInformation)

		// Used by GetVolumeInformation
		internal const uint FILE_CASE_SENSITIVE_SEARCH = 0x00000001;
		internal const uint FILE_CASE_PRESERVED_NAMES = 0x00000002;
		internal const uint FILE_UNICODE_ON_DISK = 0x00000004;
		internal const uint FILE_PERSISTENT_ACLS = 0x00000008;
		internal const uint FILE_FILE_COMPRESSION = 0x00000010;
		internal const uint FILE_VOLUME_QUOTAS = 0x00000020;
		internal const uint FILE_SUPPORTS_SPARSE_FILES = 0x00000040;
		internal const uint FILE_SUPPORTS_REPARSE_POINTS = 0x00000080;
		internal const uint FILE_SUPPORTS_REMOTE_STORAGE = 0x00000100;
		internal const uint FILE_VOLUME_IS_COMPRESSED = 0x00008000;
		internal const uint FILE_SUPPORTS_OBJECT_IDS = 0x00010000;
		internal const uint FILE_SUPPORTS_ENCRYPTION = 0x00020000;
		internal const uint FILE_NAMED_STREAMS = 0x00040000;
		internal const uint FILE_READ_ONLY_VOLUME = 0x00080000;

		#endregion

		#region Drive Types (Used by GetDriveType)

		internal const uint DRIVE_UNKNOWN = 0;
		internal const uint DRIVE_NO_ROOT_DIR = 1;
		internal const uint DRIVE_REMOVABLE = 2;
		internal const uint DRIVE_FIXED = 3;
		internal const uint DRIVE_REMOTE = 4;
		internal const uint DRIVE_CDROM = 5;
		internal const uint DRIVE_RAMDISK = 6;

		#endregion

		#region Standard Values (Used all over)

		public const int MAX_PATH = 261;
		//        internal static readonly IntPtr INVALID_HANDLE_VALUE = (IntPtr)(-1);

		#endregion

		#region File Access and Rights

		public const UInt32 ACCESS_SYSTEM_SECURITY = 0x01000000;
		public const UInt32 DELETE = 0x00010000;
		public const UInt32 READ_CONTROL = 0x00020000;
		public const UInt32 WRITE_DAC = 0x00040000;
		public const UInt32 WRITE_OWNER = 0x00080000;
		public const UInt32 SYNCHRONIZE = 0x00100000;
		public const UInt32 STANDARD_RIGHTS_REQUIRED = 0x000F0000;
		public const UInt32 STANDARD_RIGHTS_READ = READ_CONTROL;
		public const UInt32 STANDARD_RIGHTS_WRITE = READ_CONTROL;
		public const UInt32 STANDARD_RIGHTS_EXECUTE = READ_CONTROL;
		public const UInt32 STANDARD_RIGHTS_ALL = 0x001F0000;
		public const UInt32 SPECIFIC_RIGHTS_ALL = 0x0000FFFF;

		public const UInt32 FILE_READ_DATA = 0x0001;
		public const UInt32 FILE_LIST_DIRECTORY = 0x0001;
		public const UInt32 FILE_WRITE_DATA = 0x0002;
		public const UInt32 FILE_ADD_FILE = 0x0002;
		public const UInt32 FILE_APPEND_DATA = 0x0004;
		public const UInt32 FILE_ADD_SUBDIRECTORY = 0x0004;
		public const UInt32 FILE_CREATE_PIPE_INSTANCE = 0x0004;
		public const UInt32 FILE_READ_EA = 0x0008;
		public const UInt32 FILE_WRITE_EA = 0x0010;
		public const UInt32 FILE_EXECUTE = 0x0020;
		public const UInt32 FILE_TRAVERSE = 0x0020;
		public const UInt32 FILE_DELETE_CHILD = 0x0040;
		public const UInt32 FILE_READ_ATTRIBUTES = 0x0080;
		public const UInt32 FILE_WRITE_ATTRIBUTES = 0x0100;
		public const UInt32 FILE_ALL_ACCESS = STANDARD_RIGHTS_REQUIRED | SYNCHRONIZE | 0x1FF;
		public const UInt32 FILE_GENERIC_READ = STANDARD_RIGHTS_READ | FILE_READ_DATA | FILE_READ_ATTRIBUTES | FILE_READ_EA | SYNCHRONIZE;
		public const UInt32 FILE_GENERIC_WRITE = STANDARD_RIGHTS_WRITE | FILE_WRITE_DATA | FILE_WRITE_ATTRIBUTES | FILE_WRITE_EA | FILE_APPEND_DATA | SYNCHRONIZE;
		public const UInt32 FILE_GENERIC_EXECUTE = STANDARD_RIGHTS_EXECUTE | FILE_READ_ATTRIBUTES | FILE_EXECUTE | SYNCHRONIZE;
		public const UInt32 FILE_SHARE_READ = 0x00000001;
		public const UInt32 FILE_SHARE_WRITE = 0x00000002;
		public const UInt32 FILE_SHARE_DELETE = 0x00000004;
		public const UInt32 FILE_ATTRIBUTE_READONLY = 0x00000001;
		public const UInt32 FILE_ATTRIBUTE_HIDDEN = 0x00000002;
		public const UInt32 FILE_ATTRIBUTE_SYSTEM = 0x00000004;
		public const UInt32 FILE_ATTRIBUTE_DIRECTORY = 0x00000010;
		public const UInt32 FILE_ATTRIBUTE_ARCHIVE = 0x00000020;
		public const UInt32 FILE_ATTRIBUTE_DEVICE = 0x00000040;
		public const UInt32 FILE_ATTRIBUTE_NORMAL = 0x00000080;
		public const UInt32 FILE_ATTRIBUTE_TEMPORARY = 0x00000100;
		public const UInt32 FILE_ATTRIBUTE_SPARSE_FILE = 0x00000200;
		public const UInt32 FILE_ATTRIBUTE_REPARSE_POINT = 0x00000400;
		public const UInt32 FILE_ATTRIBUTE_COMPRESSED = 0x00000800;
		public const UInt32 FILE_ATTRIBUTE_OFFLINE = 0x00001000;
		public const UInt32 FILE_ATTRIBUTE_NOT_CONTENT_INDEXED = 0x00002000;
		public const UInt32 FILE_ATTRIBUTE_ENCRYPTED = 0x00004000;
		public const UInt32 FILE_ATTRIBUTE_VIRTUAL = 0x00010000;
		public const UInt32 FILE_NOTIFY_CHANGE_FILE_NAME = 0x00000001;
		public const UInt32 FILE_NOTIFY_CHANGE_DIR_NAME = 0x00000002;
		public const UInt32 FILE_NOTIFY_CHANGE_ATTRIBUTES = 0x00000004;
		public const UInt32 FILE_NOTIFY_CHANGE_SIZE = 0x00000008;
		public const UInt32 FILE_NOTIFY_CHANGE_LAST_WRITE = 0x00000010;
		public const UInt32 FILE_NOTIFY_CHANGE_LAST_ACCESS = 0x00000020;
		public const UInt32 FILE_NOTIFY_CHANGE_CREATION = 0x00000040;
		public const UInt32 FILE_NOTIFY_CHANGE_SECURITY = 0x00000100;
		public const UInt32 FILE_ACTION_ADDED = 0x00000001;
		public const UInt32 FILE_ACTION_REMOVED = 0x00000002;
		public const UInt32 FILE_ACTION_MODIFIED = 0x00000003;
		public const UInt32 FILE_ACTION_RENAMED_OLD_NAME = 0x00000004;
		public const UInt32 FILE_ACTION_RENAMED_NEW_NAME = 0x00000005;
		public const UInt32 FILE_SEQUENTIAL_WRITE_ONCE = 0x00100000;
		public const UInt32 FILE_SUPPORTS_TRANSACTIONS = 0x00200000;

		public const UInt32 REPLACEFILE_WRITE_THROUGH = 0x01;
		public const UInt32 REPLACEFILE_IGNORE_MERGE_ERRORS = 0x02;
		public const UInt32 REPLACEFILE_IGNORE_ACL_ERRORS = 0x04;

		#endregion

		#region Security
		public const int OWNER_SECURITY_INFORMATION = 0x00000001;
		public const int GROUP_SECURITY_INFORMATION = 0x00000002;
		public const int DACL_SECURITY_INFORMATION = 0x00000004;
		public const int SACL_SECURITY_INFORMATION = 0x00000008;
		/* Not needed?
		public const uint LABEL_SECURITY_INFORMATION = 0x00000010;
		public const uint PROTECTED_DACL_SECURITY_INFORMATION = 0x80000000;
		public const uint PROTECTED_SACL_SECURITY_INFORMATION = 0x40000000;
		public const uint UNPROTECTED_DACL_SECURITY_INFORMATION = 0x20000000;
		public const uint UNPROTECTED_SACL_SECURITY_INFORMATION = 0x10000000;
		*/
		#endregion

		#region Backup

		public const UInt32 STREAM_NORMAL_ATTRIBUTE = 0x00000000;
		public const UInt32 STREAM_MODIFIED_WHEN_READ = 0x00000001;
		public const UInt32 STREAM_CONTAINS_SECURITY = 0x00000002;
		public const UInt32 STREAM_CONTAINS_PROPERTIES = 0x00000004;
		public const UInt32 STREAM_SPARSE_ATTRIBUTE = 0x00000008;

		#endregion

		public const uint UNIVERSAL_NAME_INFO_LEVEL = 0x00000001;
		public const uint REMOTE_NAME_INFO_LEVEL = 0x00000002;

		#endregion

		#region Volume Management Functions

		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern uint QueryDosDeviceW(string lpDeviceName, [Out] char[] lpTargetPath, uint ucchMax);

		[DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
		internal static extern uint GetLogicalDriveStringsW(uint nBufferLength, [Out] char[] lpBuffer);

		[DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool DefineDosDeviceW(uint dwFlags, string lpDeviceName, string lpTargetPath);

		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool GetVolumeNameForVolumeMountPointW([In] string lpszVolumeMountPoint, [Out] StringBuilder lpszVolumeName, uint cchBufferLength);

		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool GetVolumePathNameW([In] string lpszFileName, [Out] StringBuilder lpszVolumePathName, uint cchBufferLength);

		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool GetVolumePathNamesForVolumeNameW(string lpszVolumeName, char[] lpszVolumePathNames, uint cchBuferLength, ref uint lpcchReturnLength);

		[DllImport("Kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal extern static bool GetVolumeInformationW(string RootPathName, StringBuilder VolumeNameBuffer, int VolumeNameSize, out uint VolumeSerialNumber, out uint MaximumComponentLength, out uint FileSystemFlags, StringBuilder FileSystemNameBuffer, int nFileSystemNameSize);

		[DllImport("Kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal extern static bool SetVolumeLabelW(string lpRootPathName, string lpVolumeName);

		[DllImport("Kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal extern static bool SetVolumeMountPointW(string lpszVolumeMountPoint, string lpszVolumeName);

		[DllImport("Kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal extern static bool DeleteVolumeMountPointW(string lpszVolumeMountPoint);

		[DllImport("Kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal extern static SafeFindVolumeHandle FindFirstVolumeW([Out] StringBuilder lpszVolumeName, int cchBufferLength);

		[DllImport("Kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal extern static bool FindNextVolumeW(SafeHandle hFindVolume, [Out] StringBuilder lpszVolumeName, int cchBufferLength);

		[DllImport("Kernel32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal extern static bool FindVolumeClose(IntPtr hFindVolume);

		[DllImport("Kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal extern static SafeFindVolumeMountPointHandle FindFirstVolumeMountPointW(string lpszRootPathName, [Out] StringBuilder lpszVolumeMountPoint, int cchBufferLength);

		[DllImport("Kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal extern static bool FindNextVolumeMountPointW(SafeHandle hFindVolume, [Out] StringBuilder lpszVolumeName, int cchBufferLength);

		[DllImport("Kernel32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal extern static bool FindVolumeMountPointClose(IntPtr hFindVolume);

		[DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
		internal extern static uint GetDriveType(string lpRootPathName);

		[DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal extern static bool GetVolumeInformationByHandleW(SafeHandle hFile, StringBuilder VolumeNameBuffer, int VolumeNameSize, out uint VolumeSerialNumber, out uint MaximumComponentLength, out uint FileSystemFlags, StringBuilder FileSystemNameBuffer, int nFileSystemNameSize);

		#endregion

		#region Disk Management

		[DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal extern static bool GetDiskFreeSpaceEx([In] string lpDirectoryName, out UInt64 lpFreeBytesAvailable, out UInt64 lpTotalNumberOfBytes, out UInt64 lpTotalNumberOfFreeBytes);
		#endregion

		#region Directory Management

		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal extern static bool CreateDirectoryW([In] string lpPathName, [In] SecurityAttributes lpSecurityAttributes);

		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal extern static bool CreateDirectoryExW([In] string lpTemplateDirectory, [In] string lpPathName, [In] SecurityAttributes lpSecurityAttributes);

		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal extern static bool RemoveDirectoryW([In] string lpPathName);

		[DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
		[return: MarshalAs(UnmanagedType.Bool)]
		static internal extern bool CreateDirectoryTransactedW([In] string lpTemplateDirectory, [In] string lpNewDirectory, [In] [MarshalAs(UnmanagedType.LPStruct)] SecurityAttributes lpSecurityAttributes, SafeHandle hTransaction);

		[DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
		[return: MarshalAs(UnmanagedType.Bool)]
		static internal extern bool RemoveDirectoryTransactedW([In] string lpPathName, [In] SafeHandle hTransaction);

		#endregion

		#region File Management

		#region Types

        /// <summary>
        /// Defines values that are used with the GetFileAttributes function to specify the information level of the returned data.
        /// </summary>
        internal enum GET_FILEEX_INFO_LEVELS : uint
        {
            /// <summary>
            /// The GetFileAttributes function retrieves a standard set of attribute information. The data is returned in a WIN32_FILE_ATTRIBUTE_DATA structure.
            /// </summary>
            GetFileExInfoStandard,
            /// <summary>
            /// One greater than the maximum value. Valid values for this enumeration will be less than this value.
            /// </summary>
            GetFileExMaxInfoLevel
        }

        /// <summary>
        /// Note: for some marshalling reason WIN32_FIND_DATA whould be declared as class not a struct.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal class WIN32_FIND_DATA
        {
            public FileAttributes dwFileAttributes;
            public FileTime ftCreationTime;
            public FileTime ftLastAccessTime;
            public FileTime ftLastWriteTime;
            public uint nFileSizeHigh;
            public uint nFileSizeLow;
            public ReparsePointTag dwReserved0;
            /// <summary>
            /// Reserved for future use
            /// </summary>
            public uint dwReserved1;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string cFileName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 14)]
            public string cAlternateFileName;
        }

		[StructLayout(LayoutKind.Sequential)]
		internal struct WIN32_FILE_ATTRIBUTE_DATA
		{
			public FileAttributes dwFileAttributes;
			public FileTime ftCreationTime;
			public FileTime ftLastAccessTime;
			public FileTime ftLastWriteTime;
			public uint nFileSizeHigh;
			public uint nFileSizeLow;
		}

		[StructLayout(LayoutKind.Sequential)]
        internal struct RemoteNameInfo
		{
			public string universalName;
			public string connectionName;
			public string remainingPath;
		}

		[StructLayout(LayoutKind.Sequential)]
        internal struct UNIVERSAL_NAME_INFO
		{
			public string universalName;
		}

		#endregion

		[DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
		[return: MarshalAs(UnmanagedType.Bool)]
		static internal extern bool CopyFileExW(string lpExistingFileName, string lpNewFileName, NativeCopyProgressRoutine lpProgressRoutine, SafeHandle lpData, ref Int32 pbCancel, CopyOptions dwCopyFlags);

		[DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
		[return: MarshalAs(UnmanagedType.Bool)]
		static internal extern bool CopyFileTransactedW(string lpExistingFileName, string lpNewFileName, NativeCopyProgressRoutine lpProgressRoutine, SafeHandle lpData, ref Int32 pbCancel, CopyOptions dwCopyFlags, SafeHandle hTransaction);

		[DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
		static internal extern SafeFileHandle CreateFileW(string lpFileName, [MarshalAs(UnmanagedType.U4)] FileAccess dwDesiredAccess, [MarshalAs(UnmanagedType.U4)] FileShare dwShareMode, SecurityAttributes lpSecurityAttributes, [MarshalAs(UnmanagedType.U4)] FileMode dwCreationDisposition, [MarshalAs(UnmanagedType.U4)] FileOptions dwFlagsAndAttributes, SafeHandle hTemplateFile);

		[DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
		static internal extern SafeFileHandle CreateFileW(string lpFileName, [MarshalAs(UnmanagedType.U4)] FileSystemRights dwDesiredAccess, [MarshalAs(UnmanagedType.U4)] FileShare dwShareMode, SecurityAttributes lpSecurityAttributes, [MarshalAs(UnmanagedType.U4)] FileMode dwCreationDisposition, [MarshalAs(UnmanagedType.U4)] FileOptions dwFlagsAndAttributes, SafeHandle hTemplateFile);

		[DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
		static internal extern SafeFileHandle CreateFileW(string lpFileName, [MarshalAs(UnmanagedType.U4)] FileSystemRights dwDesiredAccess, [MarshalAs(UnmanagedType.U4)] FileShare dwShareMode, SecurityAttributes lpSecurityAttributes, [MarshalAs(UnmanagedType.U4)] FileMode dwCreationDisposition, [MarshalAs(UnmanagedType.U4)] FileOptions dwFlagsAndAttributes, IntPtr hTemplateFile);

		[DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
		static internal extern SafeFileHandle CreateFileTransactedW(string lpFileName, [MarshalAs(UnmanagedType.U4)] FileSystemRights dwDesiredAccess, [MarshalAs(UnmanagedType.U4)] FileShare dwShareMode, SecurityAttributes lpSecurityAttributes, [MarshalAs(UnmanagedType.U4)] FileMode dwCreationDisposition, [MarshalAs(UnmanagedType.U4)] FileOptions dwFlagsAndAttributes, SafeHandle hTemplateFile, SafeHandle hTransaction, IntPtr pusMiniVersion, IntPtr pExtendedParameter);

		[DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
		static internal extern SafeFileHandle CreateFileTransactedW(string lpFileName, [MarshalAs(UnmanagedType.U4)] FileAccess dwDesiredAccess, [MarshalAs(UnmanagedType.U4)] FileShare dwShareMode, SecurityAttributes lpSecurityAttributes, [MarshalAs(UnmanagedType.U4)] FileMode dwCreationDisposition, [MarshalAs(UnmanagedType.U4)] FileOptions dwFlagsAndAttributes, SafeHandle hTemplateFile, SafeHandle hTransaction, IntPtr pusMiniVersion, IntPtr pExtendedParameter);

		[DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
		[return: MarshalAs(UnmanagedType.Bool)]
		static internal extern bool DeleteFileW(string lpFileName);

		[DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
		[return: MarshalAs(UnmanagedType.Bool)]
		static internal extern bool DeleteFileTransactedW(string lpFileName, SafeHandle hTransaction);

		[DllImport("Advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
		[return: MarshalAs(UnmanagedType.Bool)]
		static internal extern bool EncryptFileW(string lpFileName);

		[DllImport("Advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
		[return: MarshalAs(UnmanagedType.Bool)]
		static internal extern bool DecryptFileW(string lpFileName, UInt32 dwReserved);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
		static internal extern SafeFindFileHandle FindFirstFileTransactedW([In] string lpFileName, [In] FINDEX_INFO_LEVELS fInfoLevelId, [In, Out] WIN32_FIND_DATA lpFindFileData, [In] FINDEX_SEARCH_OPS fSearchOp, [In] IntPtr lpSearchFilter, [In] FINDEX_FLAGS dwAdditionalFlags, SafeHandle hTransaction);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
		[return: MarshalAs(UnmanagedType.Bool)]
		static internal extern bool FindNextFileW([In] SafeFindFileHandle hFindFile, [In, Out] WIN32_FIND_DATA lpFindFileData);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
		static internal extern SafeFindFileHandle FindFirstFileExW([In] string lpFileName, [In] FINDEX_INFO_LEVELS fInfoLevelId, [In, Out] WIN32_FIND_DATA lpFindFileData, [In] FINDEX_SEARCH_OPS fSearchOp, [In] IntPtr lpSearchFilter, [In] FINDEX_FLAGS dwAdditionalFlags);

		[DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
		[return: MarshalAs(UnmanagedType.Bool)]
		static internal extern bool FindClose([In, Out] IntPtr hFindFile);

		[DllImport("Advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
		[return: MarshalAs(UnmanagedType.Bool)]
		static internal extern bool GetFileSecurity(string lpFileName, UInt32 RequestedInformation, SafeHandle pSecurityDescriptor, UInt32 nLength, out UInt32 lpnLengthNeeded);

		[DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
		[return: MarshalAs(UnmanagedType.Bool)]
        static internal extern bool GetFileAttributesTransactedW(string lpFileName, [In, MarshalAs(UnmanagedType.U4)] GET_FILEEX_INFO_LEVELS fInfoLevelId, out WIN32_FILE_ATTRIBUTE_DATA lpFileInformation, SafeHandle hTransaction);

		[DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
		[return: MarshalAs(UnmanagedType.Bool)]
        static internal extern bool GetFileAttributesExW(string lpFileName, [In, MarshalAs(UnmanagedType.U4)] GET_FILEEX_INFO_LEVELS fInfoLevelId, out WIN32_FILE_ATTRIBUTE_DATA lpFileInformation);

		[DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
		[return: MarshalAs(UnmanagedType.Bool)]
		static internal extern bool MoveFileWithProgressW([In] string existingFileName, [In] string newFileName, NativeCopyProgressRoutine lpProgressRoutine, object lpData, [MarshalAs(UnmanagedType.U4)] MoveFileOptions dwFlags);

		[DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
		[return: MarshalAs(UnmanagedType.Bool)]
		static internal extern bool MoveFileTransactedW(string lpExistingFileName, string lpNewFileName, NativeCopyProgressRoutine lpProgressRoutine, IntPtr lpData, [MarshalAs(UnmanagedType.U4)] MoveFileOptions dwCopyFlags, SafeHandle hTransaction);

		[DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
		[return: MarshalAs(UnmanagedType.Bool)]
		static internal extern bool ReplaceFileW(string lpReplacedFileName, string lpReplacementFileName, string lpBackupFileName, UInt32 dwReplaceFlags, IntPtr lpExclude, IntPtr lpReserved);

		[DllImport("Advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
		[return: MarshalAs(UnmanagedType.Bool)]
		static internal extern bool SetFileSecurityW(string lpFileName, Security.NativeMethods.SECURITY_INFORMATION SecurityInformation, SafeHandle pSecurityDescriptor);

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2205:UseManagedEquivalentsOfWin32Api"), DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
		[return: MarshalAs(UnmanagedType.Bool)]
		static internal extern bool SetFileAttributesW(string lpFileName, [MarshalAs(UnmanagedType.U4)] FileAttributes dwFileAttributes);

		[DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
		[return: MarshalAs(UnmanagedType.Bool)]
		static internal extern bool SetFileAttributesTransactedW(string lpFileName, [MarshalAs(UnmanagedType.U4)] FileAttributes dwFileAttributes, SafeHandle hTransaction);

		[DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
		[return: MarshalAs(UnmanagedType.Bool)]
		static internal extern bool SetFileTime(SafeFileHandle hFile, SafeHandle lpCreationTime, SafeHandle lpLastAccessTime, SafeHandle lpLastWriteTime);

		[DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
		[return: MarshalAs(UnmanagedType.Bool)]
		static internal extern bool CreateHardLinkW([In] string lpFileName, [In] string lpExistingFileName, [In] IntPtr lpSecurityAttributes);

		[DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
		[return: MarshalAs(UnmanagedType.Bool)]
		static internal extern bool CreateSymbolicLinkW([In] string lpFileName, [In] string lpExistingFileName, [In, MarshalAs(UnmanagedType.U4)] SymbolicLinkTarget dwFlags);

		[DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
		[return: MarshalAs(UnmanagedType.Bool)]
		static internal extern bool CreateHardLinkTransactedW([In] string lpFileName, [In] string lpExistingFileName, IntPtr lpSecurityAttributes, SafeHandle hTransaction);

		[DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
		[return: MarshalAs(UnmanagedType.Bool)]
		static internal extern bool CreateSymbolicLinkTransactedW([In] string lpSymlinkFileName, [In] string lpTargetFileName, [In, MarshalAs(UnmanagedType.U4)] SymbolicLinkTarget dwFlags, SafeHandle hTransaction);

		[DllImport("Advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
		[return: MarshalAs(UnmanagedType.Bool)]
		static internal extern bool FileEncryptionStatusW(string lpFileName, [MarshalAs(UnmanagedType.U4)] out FileEncryptionStatus lpStatus);

		[DllImport("Advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
		[return: MarshalAs(UnmanagedType.Bool)]
		static internal extern bool EncryptionDisable(string path, [MarshalAs(UnmanagedType.Bool)] bool disable);

		[DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
		static internal extern UInt32 GetCompressedFileSizeW(string lpFileName, out UInt32 lpFileSizeHigh);

		[DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
		static internal extern UInt32 GetCompressedFileSizeTransactedW(string lpFileName, out UInt32 lpFileSizeHigh, SafeHandle hTransaction);

		[DllImport("kernel32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		static internal extern bool FlushFileBuffers(SafeFileHandle hFile);

		[DllImport("kernel32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		static internal extern bool LockFile(SafeFileHandle hFile, uint dwFileOffsetLow, uint dwFileOffsetHigh, uint nNumberOfBytesToLockLow, uint nNumberOfBytesToLockHigh);

		[DllImport("kernel32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		static internal extern bool UnlockFile(SafeFileHandle hFile, uint dwFileOffsetLow, uint dwFileOffsetHigh, uint nNumberOfBytesToUnlockLow, uint nNumberOfBytesToUnlockHigh);

		[DllImport("mpr.dll", SetLastError = true, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
		static internal extern uint WNetGetUniversalName([MarshalAs(UnmanagedType.LPStr)] string localPath, uint dwInfoLevel, [Out] SafeGlobalMemoryBufferHandle buffer, ref int lpBufferSize);

		[DllImport("kernel32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		static internal extern bool GetFileInformationByHandle([In] SafeFileHandle hFile, [In, Out] ByHandleFileInfo lpFileInformation);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static internal extern bool GetFileInformationByHandleEx([In] SafeFileHandle hFile, [In] FileInformationClass fileInformationClass, [Out] SafeHandle lpFileInformation, [In] int dwBufferSize);

		[DllImport("kernel32.dll", SetLastError = true, CharSet=CharSet.Unicode)]
		static internal extern SafeFindFileHandle FindFirstFileNameW([In] string lpFileName, Int32 dwFlags, [In, Out] ref UInt32 StringLength, StringBuilder LinkName);

		[DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
		static internal extern SafeFindFileHandle FindFirstFileNameTransactedW([In] string lpFileName, Int32 dwFlags, [In, Out] ref UInt32 StringLength, StringBuilder LinkName, SafeHandle hTransaction);

		[DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
		[return: MarshalAs(UnmanagedType.Bool)]		
		static internal extern bool FindNextFileNameW(SafeHandle hFindStream, [In, Out] ref UInt32 StringLength, [In, Out] StringBuilder LinkName);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        static internal extern int GetShortPathNameW([In] string lpLongPath,  [Out] StringBuilder lpShortPathBuffer, int bufferLength);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        static internal extern int GetLongPathNameW([In] string lpShortPath, [Out] StringBuilder lpLongPathBuffer, int bufferLength);

		#endregion

		#region Backup

		[DllImport("kernel32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		static internal extern bool BackupRead(SafeHandle hFile, SafeGlobalMemoryBufferHandle lpBuffer, uint nNumberOfBytesToRead, out uint lpNumberOfBytesRead, [MarshalAs(UnmanagedType.Bool)] bool bAbort, [MarshalAs(UnmanagedType.Bool)] bool bProcessSecurity, [In, Out] ref IntPtr lpContext);

		[DllImport("kernel32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		static internal extern bool BackupWrite(SafeHandle hFile, SafeGlobalMemoryBufferHandle lpBuffer, uint nNumberOfBytesToWrite, out uint lpNumberOfBytesWritten, [MarshalAs(UnmanagedType.Bool)] bool bAbort, [MarshalAs(UnmanagedType.Bool)] bool bProcessSecurity, [In, Out] ref IntPtr lpContext);

		[DllImport("kernel32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		static internal extern bool BackupSeek(SafeHandle hFile, uint dwLowBytesToSeek, uint dwHighBytesToSeek, out uint lpdwLowBytesSeeked, out uint lpdwHighBytesSeeked, [In, Out] ref IntPtr lpContext);

		#endregion

		#region Kernel Transaction Manager

		[DllImport("KtmW32", SetLastError = true, CharSet = CharSet.Unicode)]
		static internal extern SafeKernelTransactionHandle CreateTransaction([In] SecurityAttributes lpTransactionAttributes, [In] IntPtr UOW, [In] uint CreateOptions, [In] uint IsolationLevel, [In] uint IsolationFlags, [In] uint Timeout, [In] string Description);

		[DllImport("KtmW32", SetLastError = true, CharSet = CharSet.Unicode)]
		[return: MarshalAs(UnmanagedType.Bool)]
		static internal extern bool CommitTransaction(SafeHandle hTrans);

		[DllImport("KtmW32", SetLastError = true, CharSet = CharSet.Unicode)]
		[return: MarshalAs(UnmanagedType.Bool)]
		static internal extern bool RollbackTransaction(SafeHandle hTrans);

		#endregion

		#region Handles and Objects

		[DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
		[return: MarshalAs(UnmanagedType.Bool)]
		static internal extern bool CloseHandle(IntPtr hObject);

		#endregion

		#region Types
		[StructLayout(LayoutKind.Sequential, Pack = 4)]
		internal struct WIN32_STREAM_ID
		{
			public UInt32 dwStreamId;
			public UInt32 dwStreamAttributes;
			public UInt64 Size;
			public UInt32 dwStreamNameSize;
			// + WCHAR[ANYSIZE_ARRAY]
		}

		/// <summary>
		/// Class used to represent the SECURITY_ATTRIBUES native win32 structure. 
		/// It provides initialization function from an ObjectSecurity object.
		/// </summary>
		[StructLayout(LayoutKind.Sequential)]
		internal sealed class SecurityAttributes
		{
			/// <summary>
			/// Initializes the SecurityAttributes structure from an instance of <see cref="ObjectSecurity"/>.
			/// </summary>
			/// <param name="memoryHandle">A handle that will refer to the memory allocated by this object for storage of the 
			/// security descriptor. As long as this object is used, the memory handle should be kept alive, and afterwards it
			/// should be disposed as soon as possible.</param>
			/// <param name="securityDescriptor">The security descriptor to assign to this object. This parameter may be <see langword="null"/>.</param>
			public void Initialize(out SafeGlobalMemoryBufferHandle memoryHandle, ObjectSecurity securityDescriptor)
			{
				nLength = (uint)Marshal.SizeOf(this);

				if (securityDescriptor == null)
				{
					memoryHandle = new SafeGlobalMemoryBufferHandle();
				}
				else
				{
					byte[] src = securityDescriptor.GetSecurityDescriptorBinaryForm();
					memoryHandle = new SafeGlobalMemoryBufferHandle(src.Length);
					memoryHandle.CopyFrom(src, 0, src.Length);
				}
				bInheritHandle = 0;
			}

			public UInt32 nLength;
			public IntPtr lpSecurityDescriptor;
			public int bInheritHandle;
		}

		internal delegate CopyProgressResult NativeCopyProgressRoutine(Int64 TotalFileSize, Int64 TotalBytesTransferred, Int64 StreamSize, Int64 StreamBytesTransferred, UInt32 dwStreamNumber, CopyProgressCallbackReason dwCallbackReason, IntPtr hSourceFile, IntPtr hDestinationFile, IntPtr lpData);

		#region FINDEX constants (FindFirstFileEx/FindNextFile etc.)

		internal enum FINDEX_INFO_LEVELS : uint
		{
			FindExInfoStandard,
            /// <summary>
            /// This value is not supported until Windows Server 2008 R2 and Windows 7.
            /// </summary>
            FindExInfoBasic,
			FindExInfoMaxLevel
		}

		internal enum FINDEX_SEARCH_OPS : uint
		{
			FindExSearchNameMatch,
			FindExSearchLimitToDirectories,
			FindExSearchLimitToDevices
		}

		[Flags]
		internal enum FINDEX_FLAGS : uint
		{
			FIND_FIRST_EX_NONE = 0x00,
			FIND_FIRST_EX_CASE_SENSITIVE = 0x01
		}

		#endregion

      [StructLayout(LayoutKind.Sequential)]
      internal struct FILE_ID_BOTH_DIR_INFO
      {
         public int NextEntryOffset;
         public int FileIndex;
         public Alphaleonis.Win32.Filesystem.FileTime CreationTime;
         public Alphaleonis.Win32.Filesystem.FileTime LastAccessTime;
         public Alphaleonis.Win32.Filesystem.FileTime LastWriteTime;
         public Alphaleonis.Win32.Filesystem.FileTime ChangeTime;
         public Int64 EndOfFile;
         public Int64 AllocationSize;
         public Int32 FileAttributes;
         public Int32 FileNameLength;
         public Int32 EaSize;
         public byte ShortNameLength;

         [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12, ArraySubType = UnmanagedType.U2)]
         public char[] ShortName;
         public Int64 FileId;
         public IntPtr FileName;
      }

      public enum FileInformationClass
      {
         FileBasicInfo,
         FileStandardInfo,
         FileNameInfo,
         FileRenameInfo,
         FileDispositionInfo,
         FileAllocationInfo,
         FileEndOfFileInfo,
         FileStreamInfo,
         FileCompressionInfo,
         FileAttributeTagInfo,
         FileIdBothDirectoryInfo,
         FileIdBothDirectoryRestartInfo,
         FileIoPriorityHintInfo,
         FileRemoteProtocolInfo,
         MaximumFileInfoByHandlesClass
      }

		#endregion

		#region Device Management

		#region Types

		[StructLayout(LayoutKind.Sequential)]
		public struct SymbolicLinkReparseBuffer
		{
			UInt16 SubstituteNameOffset;
			UInt16 SubstituteNameLength;
			UInt16 PrintNameOffset;
			UInt16 PrintNameLength;
			UInt64 Flags;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct MountPointReparseBuffer
		{
			UInt16 SubstituteNameOffset;
			UInt16 SubstituteNameLength;
			UInt16 PrintNameOffset;
			UInt16 PrintNameLength;
		}

		#endregion

		[DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
		[return: MarshalAs(UnmanagedType.Bool)]
		static internal extern bool DeviceIoControl(SafeHandle hDevice,
													 IoControlCode dwIoControlCode,
													 SafeHandle lpInBuffer,
													 UInt32 nInBufferSize,
													 SafeHandle lpOutBuffer,
													 UInt32 nOutBufferSize,
													 out UInt32 lpBytesReturned,
													 IntPtr lpOverlapped);


		#endregion

		#region Utility

		public static uint GetLowOrderDword(long value)
		{
			return (uint)(value & 0xFFFFFFFF);
		}

		public static uint GetHighOrderDword(long value)
		{
			return (uint)((value >> 32) & 0xFFFFFFFF);
		}

		#endregion

	}
}
