using Alphaleonis.Win32.Security;
using Microsoft.Win32.SafeHandles;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Text;

namespace Alphaleonis.Win32.Filesystem
{
   internal static partial class NativeMethods
   {
      #region AssocXxx

      /// <summary>Returns a pointer to an IQueryAssociations object.</summary>
      /// <returns>If this function succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
      /// <remarks>Minimum supported client: Windows 2000 Professional, Windows XP [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows 2000 Server [desktop apps only]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("shlwapi.dll", SetLastError = true, CharSet = CharSet.Unicode)]
      [return: MarshalAs(UnmanagedType.U4)]
      internal static extern uint AssocCreate(Guid clsid, ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out IQueryAssociations ppv);

      /// <summary>Searches for and retrieves a file or protocol association-related string from the registry.</summary>
      /// <returns>Return value Type: HRESULT. Returns a standard COM error value, including the following: S_OK, E_POINTER and S_FALSE.</returns>
      /// <remarks>Minimum supported client: Windows 2000 Professional</remarks>
      /// <remarks>Minimum supported server: Windows 2000 Server</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("shlwapi.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "AssocQueryStringW")]
      [return: MarshalAs(UnmanagedType.U4)]
      internal static extern uint AssocQueryString(Shell32.AssociationAttributes flags, Shell32.AssociationString str, [MarshalAs(UnmanagedType.LPWStr)] string pszAssoc, [MarshalAs(UnmanagedType.LPWStr)] string pszExtra, StringBuilder pszOut, [MarshalAs(UnmanagedType.U4)] out uint pcchOut);


      #region IQueryAssociations

      internal static readonly Guid ClsidQueryAssociations = new Guid("A07034FD-6CAA-4954-AC3F-97A27216F98A");
      internal const string QueryAssociationsGuid = "C46CA590-3C3F-11D2-BEE6-0000F805CA57";

      /// <summary>Exposes methods that simplify the process of retrieving information stored in the registry in association with defining a file type or protocol and associating it with an application.</summary>
      [Guid(QueryAssociationsGuid), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
      internal interface IQueryAssociations
      {
         /// <summary>Initializes the IQueryAssociations interface and sets the root key to the appropriate ProgID.</summary>
         /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
         /// <remarks>Minimum supported client: Windows 2000 Professional, Windows XP [desktop apps only]</remarks>
         /// <remarks>Minimum supported server: Windows 2000 Server [desktop apps only]</remarks>
         void Init(Shell32.AssociationAttributes flags, [MarshalAs(UnmanagedType.LPWStr)] string pszAssoc, IntPtr hkProgid, IntPtr hwnd);
         //[return: MarshalAs(UnmanagedType.U4)]
         //uint Init(Shell32.AssociationAttributes flags, [MarshalAs(UnmanagedType.LPWStr)] string pszAssoc, IntPtr hkProgid, IntPtr hwnd);

         /// <summary>Searches for and retrieves a file or protocol association-related string from the registry.</summary>
         /// <returns>Returns a standard COM error value, including the following: S_OK, E_POINTER, S_FALSE</returns>
         /// <remarks>Minimum supported client: Windows 2000 Professional, Windows XP [desktop apps only]</remarks>
         /// <remarks>Minimum supported server: Windows 2000 Server [desktop apps only]</remarks>
         void GetString(Shell32.AssociationAttributes flags, Shell32.AssociationString str, [MarshalAs(UnmanagedType.LPWStr)] string pwszExtra, StringBuilder pwszOut, [MarshalAs(UnmanagedType.I4)] out int pcchOut);
         //[return: MarshalAs(UnmanagedType.U4)]
         //void GetString(Shell32.AssociationAttributes flags, Shell32.AssociationString str, [MarshalAs(UnmanagedType.LPWStr)] string pwszExtra, StringBuilder pwszOut, [MarshalAs(UnmanagedType.I4)] out int pcchOut);

         ///// <summary>Searches for and retrieves a file or protocol association-related key from the registry.</summary>
         ///// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
         ///// <remarks>Minimum supported client: Windows 2000 Professional, Windows XP [desktop apps only]</remarks>
         ///// <remarks>Minimum supported server: Windows 2000 Server [desktop apps only]</remarks>
         //[return: MarshalAs(UnmanagedType.U4)]
         //uint GetKey(Shell32.AssociationAttributes flags, Shell32.AssociationKey str, [MarshalAs(UnmanagedType.LPWStr)] string pwszExtra, out UIntPtr phkeyOut);

         ///// <summary>Searches for and retrieves file or protocol association-related binary data from the registry.</summary>
         ///// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
         ///// <remarks>Minimum supported client: Windows 2000 Professional, Windows XP [desktop apps only]</remarks>
         ///// <remarks>Minimum supported server: Windows 2000 Server [desktop apps only]</remarks>
         //[return: MarshalAs(UnmanagedType.U4)]
         //uint GetData(Shell32.AssociationAttributes flags, Shell32.AssociationData data, [MarshalAs(UnmanagedType.LPWStr)] string pwszExtra, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] out byte[] pvOut, [MarshalAs(UnmanagedType.I4)] out int pcbOut);

         ///// <summary>This method is not implemented.</summary>
         //void GetEnum();
      }

      #endregion // IQueryAssociations

      #endregion // AssocXxx

      /// <summary>
      ///   Copies an existing file to a new file, notifying the application of its progress through a callback function.
      /// </summary>
      /// <remarks>
      ///   <para>This function fails with ERROR_ACCESS_DENIED if the destination file already exists and has the FILE_ATTRIBUTE_HIDDEN or
      ///   FILE_ATTRIBUTE_READONLY attribute set.</para>
      ///   <para>This function preserves extended attributes, OLE structured storage, NTFS file system alternate data streams, security
      ///   resource attributes, and file attributes.</para>
      ///   <para>Windows 7, Windows Server 2008 R2, Windows Server 2008, Windows Vista, Windows Server 2003, and Windows XP:
      ///   Security resource attributes (ATTRIBUTE_SECURITY_INFORMATION) for the existing file are not copied to the new file until
      ///   Windows 8 and Windows Server 2012.</para>
      ///   <para>Minimum supported client: Windows XP [desktop apps only]</para>
      ///   <para>Minimum supported server: Windows Server 2003 [desktop apps only]</para>
      /// </remarks>
      /// <param name="lpExistingFileName">Filename of the existing file.</param>
      /// <param name="lpNewFileName">Filename of the new file.</param>
      /// <param name="lpProgressRoutine">The progress routine.</param>
      /// <param name="lpData">The data.</param>
      /// <param name="pbCancel">[out] The pb cancel.</param>
      /// <param name="dwCopyFlags">The copy flags.</param>
      /// <returns>
      ///   <para>If the function succeeds, the return value is nonzero.</para>
      ///   <para>If the function fails, the return value is zero. To get extended error information, call GetLastError.</para>
      /// </returns>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "CopyFileExW")]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool CopyFileEx([MarshalAs(UnmanagedType.LPWStr)] string lpExistingFileName, [MarshalAs(UnmanagedType.LPWStr)] string lpNewFileName, NativeCopyMoveProgressRoutine lpProgressRoutine, IntPtr lpData, [MarshalAs(UnmanagedType.Bool)] out bool pbCancel, CopyOptions dwCopyFlags);

      /// <summary>
      ///   Copies an existing file to a new file as a transacted operation, notifying the application of its progress through a callback
      ///   function.
      /// </summary>
      /// <remarks>
      ///   <para>This function fails with ERROR_ACCESS_DENIED if the destination file already exists and has the FILE_ATTRIBUTE_HIDDEN or
      ///   FILE_ATTRIBUTE_READONLY attribute set.</para>
      ///   <para>This function preserves extended attributes, OLE structured storage, NTFS file system alternate data streams, security
      ///   resource attributes, and file attributes.</para>
      ///   <para>Windows 7, Windows Server 2008 R2, Windows Server 2008, Windows Vista, Windows Server 2003, and Windows XP:
      ///   Security resource attributes (ATTRIBUTE_SECURITY_INFORMATION) for the existing file are not copied to the new file until
      ///   Windows 8 and Windows Server 2012.</para>
      ///   <para>Minimum supported client: Windows Vista [desktop apps only]</para>
      ///   <para>Minimum supported server: Windows Server 2008 [desktop apps only]</para>
      /// </remarks>
      /// <returns>
      ///   <para>If the function succeeds, the return value is nonzero.</para>
      ///   <para>If the function fails, the return value is zero. To get extended error information, call GetLastError.</para>
      /// </returns>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "CopyFileTransactedW")]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool CopyFileTransacted([MarshalAs(UnmanagedType.LPWStr)] string lpExistingFileName, [MarshalAs(UnmanagedType.LPWStr)] string lpNewFileName, NativeCopyMoveProgressRoutine lpProgressRoutine, IntPtr lpData, [MarshalAs(UnmanagedType.Bool)] out bool pbCancel, CopyOptions dwCopyFlags, SafeHandle hTransaction);

      /// <summary>
      ///   Creates or opens a file or I/O device. The most commonly used I/O devices are as follows: file, file stream, directory, physical
      ///   disk, volume, console buffer, tape drive, communications resource, mailslot, and pipe.
      /// </summary>
      /// <remarks>Minimum supported client: Windows XP.</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003.</remarks>
      /// <returns>
      ///   If the function succeeds, the return value is an open handle to the specified file, device, named pipe, or mail slot. If the
      ///   function fails, the return value is Win32Errors.ERROR_INVALID_HANDLE. To get extended error information, call GetLastError.
      /// </returns>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "CreateFileW")]
      internal static extern SafeFileHandle CreateFile([MarshalAs(UnmanagedType.LPWStr)] string lpFileName, [MarshalAs(UnmanagedType.U4)] FileSystemRights dwDesiredAccess, [MarshalAs(UnmanagedType.U4)] FileShare dwShareMode, [MarshalAs(UnmanagedType.LPStruct)] Security.NativeMethods.SecurityAttributes lpSecurityAttributes, [MarshalAs(UnmanagedType.U4)] FileMode dwCreationDisposition, [MarshalAs(UnmanagedType.U4)] ExtendedFileAttributes dwFlagsAndAttributes, IntPtr hTemplateFile);

      /// <summary>
      ///   Creates or opens a file or I/O device. The most commonly used I/O devices are as follows: file, file stream, directory, physical
      ///   disk, volume, console buffer, tape drive, communications resource, mailslot, and pipe.
      /// </summary>
      /// <remarks>Minimum supported client: Windows Vista [desktop apps only].</remarks>
      /// <remarks>Minimum supported server: Windows Server 2008 [desktop apps only].</remarks>
      /// <returns>
      ///   If the function succeeds, the return value is an open handle to the specified file, device, named pipe, or mail slot. If the
      ///   function fails, the return value is Win32Errors.ERROR_INVALID_HANDLE". To get extended error information, call GetLastError.
      /// </returns>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "CreateFileTransactedW")]
      internal static extern SafeFileHandle CreateFileTransacted([MarshalAs(UnmanagedType.LPWStr)] string lpFileName, [MarshalAs(UnmanagedType.U4)] FileSystemRights dwDesiredAccess, [MarshalAs(UnmanagedType.U4)] FileShare dwShareMode, [MarshalAs(UnmanagedType.LPStruct)] Security.NativeMethods.SecurityAttributes lpSecurityAttributes, [MarshalAs(UnmanagedType.U4)] FileMode dwCreationDisposition, [MarshalAs(UnmanagedType.U4)] ExtendedFileAttributes dwFlagsAndAttributes, IntPtr hTemplateFile, SafeHandle hTransaction, IntPtr pusMiniVersion, IntPtr pExtendedParameter);

      /// <summary>Creates or opens a named or unnamed file mapping object for a specified file.</summary>
      /// <remarks>Minimum supported client: Windows XP.</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003.</remarks>
      /// <returns>
      ///   If the function succeeds, the return value is a handle to the newly created file mapping object. If the function fails, the return
      ///   value is <see langword="null"/>.
      /// </returns>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = false, CharSet = CharSet.Unicode, EntryPoint = "CreateFileMappingW")]
      internal static extern SafeFileHandle CreateFileMapping(SafeFileHandle hFile, SafeHandle lpSecurityAttributes, [MarshalAs(UnmanagedType.U4)] uint flProtect, [MarshalAs(UnmanagedType.U4)] uint dwMaximumSizeHigh, [MarshalAs(UnmanagedType.U4)] uint dwMaximumSizeLow, [MarshalAs(UnmanagedType.LPWStr)] string lpName);

      /// <summary>
      ///   Establishes a hard link between an existing file and a new file. This function is only supported on the NTFS file system, and only
      ///   for files, not directories.
      /// </summary>
      /// <remarks>Minimum supported client: Windows XP [desktop apps only].</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003 [desktop apps only].</remarks>
      /// <returns>
      ///   If the function succeeds, the return value is nonzero. If the function fails, the return value is zero (0). To get extended error
      ///   information, call GetLastError.
      /// </returns>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "CreateHardLinkW")]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool CreateHardLink([MarshalAs(UnmanagedType.LPWStr)] string lpFileName, [MarshalAs(UnmanagedType.LPWStr)] string lpExistingFileName, IntPtr lpSecurityAttributes);

      /// <summary>
      ///   Establishes a hard link between an existing file and a new file as a transacted operation. This function is only supported on the
      ///   NTFS file system, and only for files, not directories.
      /// </summary>
      /// <remarks>Minimum supported client: Windows Vista [desktop apps only].</remarks>
      /// <remarks>Minimum supported server: Windows Server 2008 [desktop apps only].</remarks>
      /// <returns>
      ///   If the function succeeds, the return value is nonzero. If the function fails, the return value is zero (0). To get extended error
      ///   information, call GetLastError.
      /// </returns>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "CreateHardLinkTransactedW")]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool CreateHardLinkTransacted([MarshalAs(UnmanagedType.LPWStr)] string lpFileName, [MarshalAs(UnmanagedType.LPWStr)] string lpExistingFileName, IntPtr lpSecurityAttributes, SafeHandle hTransaction);

      /// <summary>Creates a symbolic link.</summary>
      /// <remarks>Minimum supported client: Windows Vista [desktop apps only].</remarks>
      /// <remarks>Minimum supported server: Windows Server 2008 [desktop apps only].</remarks>
      /// <returns>
      ///   If the function succeeds, the return value is nonzero. If the function fails, the return value is zero (0). To get extended error
      ///   information, call GetLastError.
      /// </returns>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "CreateSymbolicLinkW")]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool CreateSymbolicLink([MarshalAs(UnmanagedType.LPWStr)] string lpSymlinkFileName, [MarshalAs(UnmanagedType.LPWStr)] string lpTargetFileName, [MarshalAs(UnmanagedType.U4)] SymbolicLinkTarget dwFlags);

      /// <summary>Creates a symbolic link as a transacted operation.</summary>
      /// <remarks>Minimum supported client: Windows Vista [desktop apps only].</remarks>
      /// <remarks>Minimum supported server: Windows Server 2008 [desktop apps only].</remarks>
      /// <returns>
      ///   If the function succeeds, the return value is nonzero. If the function fails, the return value is zero (0). To get extended error
      ///   information, call GetLastError.
      /// </returns>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "CreateSymbolicLinkTransactedW")]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool CreateSymbolicLinkTransacted([MarshalAs(UnmanagedType.LPWStr)] string lpSymlinkFileName, [MarshalAs(UnmanagedType.LPWStr)] string lpTargetFileName, [MarshalAs(UnmanagedType.U4)] SymbolicLinkTarget dwFlags, SafeHandle hTransaction);

      /// <summary>Decrypts an encrypted file or directory.</summary>
      /// <remarks>
      ///   The DecryptFile function requires exclusive access to the file being decrypted, and will fail if another process is using the file.
      ///   If the file is not encrypted, DecryptFile simply returns a nonzero value, which indicates success. If lpFileName specifies a read-
      ///   only file, the function fails and GetLastError returns ERROR_FILE_READ_ONLY. If lpFileName specifies a directory that contains a
      ///   read-only file, the functions succeeds but the directory is not decrypted.
      /// </remarks>
      /// <remarks>Minimum supported client: Windows XP.</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003.</remarks>
      /// <returns>
      ///   If the function succeeds, the return value is nonzero. If the function fails, the return value is zero. To get extended error
      ///   information, call GetLastError.
      /// </returns>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "DecryptFileW")]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool DecryptFile([MarshalAs(UnmanagedType.LPWStr)] string lpFileName, [MarshalAs(UnmanagedType.U4)] uint dwReserved);

      /// <summary>Deletes an existing file.</summary>
      /// <remarks>
      ///   If an application attempts to delete a file that does not exist, the DeleteFile function fails with ERROR_FILE_NOT_FOUND.
      /// </remarks>
      /// <remarks>If the file is a read-only file, the function fails with ERROR_ACCESS_DENIED.</remarks>
      /// <remarks>
      ///   If the path points to a symbolic link, the symbolic link is deleted, not the target. To delete a target, you must call CreateFile
      ///   and specify FILE_FLAG_DELETE_ON_CLOSE.
      /// </remarks>
      /// <remarks>Minimum supported client: Windows XP [desktop apps | Windows Store apps].</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003 [desktop apps | Windows Store apps].</remarks>
      /// <returns>
      ///   If the function succeeds, the return value is nonzero. If the function fails, the return value is zero (0). To get extended error
      ///   information, call GetLastError.
      /// </returns>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "DeleteFileW")]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool DeleteFile([MarshalAs(UnmanagedType.LPWStr)] string lpFileName);

      /// <summary>Deletes an existing file as a transacted operation.</summary>
      /// <remarks>Minimum supported client: Windows Vista [desktop apps only].</remarks>
      /// <remarks>Minimum supported server: Windows Server 2008 [desktop apps only].</remarks>
      /// <returns>
      ///   If the function succeeds, the return value is nonzero. If the function fails, the return value is zero (0). To get extended error
      ///   information, call GetLastError.
      /// </returns>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "DeleteFileTransactedW")]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool DeleteFileTransacted([MarshalAs(UnmanagedType.LPWStr)] string lpFileName, SafeHandle hTransaction);

      /// <summary>
      ///   Encrypts a file or directory. All data streams in a file are encrypted. All new files created in an encrypted directory are
      ///   encrypted.
      /// </summary>
      /// <remarks>
      ///   The EncryptFile function requires exclusive access to the file being encrypted, and will fail if another process is using the file.
      ///   If the file is already encrypted, EncryptFile simply returns a nonzero value, which indicates success. If the file is compressed,
      ///   EncryptFile will decompress the file before encrypting it. If lpFileName specifies a read-only file, the function fails and
      ///   GetLastError returns ERROR_FILE_READ_ONLY. If lpFileName specifies a directory that contains a read-only file, the functions
      ///   succeeds but the directory is not encrypted.
      /// </remarks>
      /// <remarks>Minimum supported client: Windows XP.</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003.</remarks>
      /// <returns>
      ///   If the function succeeds, the return value is nonzero. If the function fails, the return value is zero. To get extended error
      ///   information, call GetLastError.
      /// </returns>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "EncryptFileW")]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool EncryptFile([MarshalAs(UnmanagedType.LPWStr)] string lpFileName);

      /// <summary>
      ///   Disables or enables encryption of the specified directory and the files in it. It does not affect encryption of subdirectories
      ///   below the indicated directory.
      /// </summary>
      /// <remarks>
      ///   EncryptionDisable() disables encryption of directories and files. It does not affect the visibility of files with the
      ///   FILE_ATTRIBUTE_SYSTEM attribute set. This method will create/change the file "Desktop.ini" and wil set Encryption value:
      ///   "Disable=0|1".
      /// </remarks>
      /// <remarks>Minimum supported client: Windows XP Professional [desktop apps only].</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003 [desktop apps only].</remarks>
      /// <returns>
      ///   If the function succeeds, the return value is nonzero. If the function fails, the return value is zero. To get extended error
      ///   information, call GetLastError.
      /// </returns>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool EncryptionDisable([MarshalAs(UnmanagedType.LPWStr)] string dirPath, [MarshalAs(UnmanagedType.Bool)] bool disable);

      /// <summary>Retrieves the encryption status of the specified file.</summary>
      /// <remarks>Minimum supported client: Windows XP Professional [desktop apps only].</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003 [desktop apps only].</remarks>
      /// <returns>
      ///   If the function succeeds, the return value is nonzero. If the function fails, the return value is zero. To get extended error
      ///   information, call GetLastError.
      /// </returns>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "FileEncryptionStatusW")]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool FileEncryptionStatus([MarshalAs(UnmanagedType.LPWStr)] string lpFileName, out FileEncryptionStatus lpStatus);

      /// <summary>
      ///   Closes a file search handle opened by the FindFirstFile, FindFirstFileEx, FindFirstFileNameW, FindFirstFileNameTransactedW,
      ///   FindFirstFileTransacted, FindFirstStreamTransactedW, or FindFirstStreamW functions.
      /// </summary>
      /// <remarks>Minimum supported client: Windows XP [desktop apps | Windows Store apps].</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003 [desktop apps | Windows Store apps].</remarks>
      /// <returns>
      ///   If the function succeeds, the return value is nonzero. If the function fails, the return value is zero. To get extended error
      ///   information, call GetLastError.
      /// </returns>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = false, CharSet = CharSet.Unicode)]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool FindClose(IntPtr hFindFile);

      /// <summary>Searches a directory for a file or subdirectory with a name and attributes that match those specified.</summary>
      /// <remarks>A trailing backslash is not allowed and will be removed.</remarks>
      /// <remarks>Minimum supported client: Windows XP [desktop apps | Windows Store apps].</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003 [desktop apps | Windows Store apps].</remarks>
      /// <returns>
      ///   If the function succeeds, the return value is a search handle used in a subsequent call to FindNextFile or FindClose, and the
      ///   lpFindFileData parameter contains information about the first file or directory found. If the function fails or fails to locate
      ///   files from the search string in the lpFileName parameter, the return value is INVALID_HANDLE_VALUE and the contents of
      ///   lpFindFileData are indeterminate. To get extended error information, call the GetLastError function.
      /// </returns>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "FindFirstFileExW")]
      internal static extern SafeFindFileHandle FindFirstFileEx([MarshalAs(UnmanagedType.LPWStr)] string lpFileName, FindExInfoLevels fInfoLevelId, out Win32FindData lpFindFileData, FindExSearchOps fSearchOp, IntPtr lpSearchFilter, FindExAdditionalFlags dwAdditionalFlags);

      /// <summary>
      ///   Searches a directory for a file or subdirectory with a name that matches a specific name as a transacted operation.
      /// </summary>
      /// <remarks>A trailing backslash is not allowed and will be removed.</remarks>
      /// <remarks>Minimum supported client: Windows Vista [desktop apps only].</remarks>
      /// <remarks>Minimum supported server: Windows Server 2008 [desktop apps only].</remarks>
      /// <returns>
      ///   If the function succeeds, the return value is a search handle used in a subsequent call to FindNextFile or FindClose, and the
      ///   lpFindFileData parameter contains information about the first file or directory found. If the function fails or fails to locate
      ///   files from the search string in the lpFileName parameter, the return value is INVALID_HANDLE_VALUE and the contents of
      ///   lpFindFileData are indeterminate. To get extended error information, call the GetLastError function.
      /// </returns>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "FindFirstFileTransactedW")]
      internal static extern SafeFindFileHandle FindFirstFileTransacted([MarshalAs(UnmanagedType.LPWStr)] string lpFileName, FindExInfoLevels fInfoLevelId, out Win32FindData lpFindFileData, FindExSearchOps fSearchOp, IntPtr lpSearchFilter, FindExAdditionalFlags dwAdditionalFlags, SafeHandle hTransaction);

      /// <summary>
      ///   Creates an enumeration of all the hard links to the specified file. The FindFirstFileNameW function returns a handle to the
      ///   enumeration that can be used on subsequent calls to the FindNextFileNameW function.
      /// </summary>
      /// <remarks>Minimum supported client: Windows Vista [desktop apps only].</remarks>
      /// <remarks>Minimum supported server: Windows Server 2008 [desktop apps only].</remarks>
      /// <returns>
      ///   If the function succeeds, the return value is a search handle that can be used with the FindNextFileNameW function or closed with
      ///   the FindClose function. If the function fails, the return value is INVALID_HANDLE_VALUE (0xffffffff). To get extended error
      ///   information, call the GetLastError function.
      /// </returns>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "FindFirstFileNameW")]
      internal static extern SafeFindFileHandle FindFirstFileName([MarshalAs(UnmanagedType.LPWStr)] string lpFileName, [MarshalAs(UnmanagedType.U4)] uint dwFlags, [MarshalAs(UnmanagedType.U4)] out uint stringLength, StringBuilder linkName);

      /// <summary>
      ///   Creates an enumeration of all the hard links to the specified file as a transacted operation. The function returns a handle to the
      ///   enumeration that can be used on subsequent calls to the FindNextFileNameW function.
      /// </summary>
      /// <remarks>Minimum supported client: Windows Vista [desktop apps only].</remarks>
      /// <remarks>Minimum supported server: Windows Server 2008 [desktop apps only].</remarks>
      /// <returns>
      ///   If the function succeeds, the return value is a search handle that can be used with the FindNextFileNameW function or closed with
      ///   the FindClose function. If the function fails, the return value is INVALID_HANDLE_VALUE (0xffffffff). To get extended error
      ///   information, call the GetLastError function.
      /// </returns>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "FindFirstFileNameTransactedW")]
      internal static extern SafeFindFileHandle FindFirstFileNameTransacted([MarshalAs(UnmanagedType.LPWStr)] string lpFileName, [MarshalAs(UnmanagedType.U4)] uint dwFlags, [MarshalAs(UnmanagedType.U4)] out uint stringLength, StringBuilder linkName, SafeHandle hTransaction);

      /// <summary>
      ///   Continues a file search from a previous call to the FindFirstFile, FindFirstFileEx, or FindFirstFileTransacted functions.
      /// </summary>
      /// <remarks>Minimum supported client: Windows XP [desktop apps | Windows Store apps].</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003 [desktop apps | Windows Store apps].</remarks>
      /// <returns>
      ///   If the function succeeds, the return value is nonzero and the lpFindFileData parameter contains information about the next file or
      ///   directory found. If the function fails, the return value is zero and the contents of lpFindFileData are indeterminate. To get
      ///   extended error information, call the GetLastError function. If the function fails because no more matching files can be found, the
      ///   GetLastError function returns ERROR_NO_MORE_FILES.
      /// </returns>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "FindNextFileW")]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool FindNextFile(SafeFindFileHandle hFindFile, out Win32FindData lpFindFileData);

      /// <summary>
      ///   Continues enumerating the hard links to a file using the handle returned by a successful call to the FindFirstFileName function.
      /// </summary>
      /// <remarks>Minimum supported client: Windows Vista [desktop apps only].</remarks>
      /// <remarks>Minimum supported server: Windows Server 2008 [desktop apps only].</remarks>
      /// <returns>
      ///   If the function succeeds, the return value is nonzero. If the function fails, the return value is zero (0). To get extended error
      ///   information, call GetLastError. If no matching files can be found, the GetLastError function returns ERROR_HANDLE_EOF.
      /// </returns>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "FindNextFileNameW")]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool FindNextFileName(SafeFindFileHandle hFindStream, [MarshalAs(UnmanagedType.U4)] out uint stringLength, StringBuilder linkName);

      /// <summary>Flushes the buffers of a specified file and causes all buffered data to be written to a file.</summary>
      /// <remarks>Minimum supported client: Windows XP [desktop apps | Windows Store apps].</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003 [desktop apps | Windows Store apps].</remarks>
      /// <returns>
      ///   If the function succeeds, the return value is nonzero. If the function fails, the return value is zero. To get extended error
      ///   information, call GetLastError.
      /// </returns>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool FlushFileBuffers(SafeFileHandle hFile);

      /// <summary>Retrieves the actual number of bytes of disk storage used to store a specified file.</summary>
      /// <remarks>Minimum supported client: Windows XP [desktop apps only].</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003 [desktop apps only].</remarks>
      /// <returns>
      ///   If the function succeeds, the return value is the low-order DWORD of the actual number of bytes of disk storage used to store the
      ///   specified file, and if lpFileSizeHigh is non-NULL, the function puts the high-order DWORD of that actual value into the DWORD
      ///   pointed to by that parameter. This is the compressed file size for compressed files, the actual file size for noncompressed files.
      ///   If the function fails, and lpFileSizeHigh is NULL, the return value is INVALID_FILE_SIZE. To get extended error information, call
      ///   GetLastError. If the return value is INVALID_FILE_SIZE and lpFileSizeHigh is non-NULL, an application must call GetLastError to
      ///   determine whether the function has succeeded (value is NO_ERROR) or failed (value is other than NO_ERROR).
      /// </returns>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "GetCompressedFileSizeW")]
      [return: MarshalAs(UnmanagedType.U4)]
      internal static extern uint GetCompressedFileSize([MarshalAs(UnmanagedType.LPWStr)] string lpFileName, [MarshalAs(UnmanagedType.U4)] out uint lpFileSizeHigh);

      /// <summary>Retrieves the actual number of bytes of disk storage used to store a specified file as a transacted operation.</summary>
      /// <remarks>Minimum supported client: Windows Vista [desktop apps only].</remarks>
      /// <remarks>Minimum supported server: Windows Server 2008 [desktop apps only].</remarks>
      /// <returns>
      ///   If the function succeeds, the return value is the low-order DWORD of the actual number of bytes of disk storage used to store the
      ///   specified file, and if lpFileSizeHigh is non-NULL, the function puts the high-order DWORD of that actual value into the DWORD
      ///   pointed to by that parameter. This is the compressed file size for compressed files, the actual file size for noncompressed files.
      ///   If the function fails, and lpFileSizeHigh is NULL, the return value is INVALID_FILE_SIZE. To get extended error information, call
      ///   GetLastError. If the return value is INVALID_FILE_SIZE and lpFileSizeHigh is non-NULL, an application must call GetLastError to
      ///   determine whether the function has succeeded (value is NO_ERROR) or failed (value is other than NO_ERROR).
      /// </returns>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "GetCompressedFileSizeTransactedW")]
      [return: MarshalAs(UnmanagedType.U4)]
      internal static extern uint GetCompressedFileSizeTransacted([MarshalAs(UnmanagedType.LPWStr)] string lpFileName, [MarshalAs(UnmanagedType.U4)] out uint lpFileSizeHigh, SafeHandle hTransaction);

      /// <summary>
      ///   Retrieves attributes for a specified file or directory.
      /// </summary>
      /// <remarks>
      ///   <para>The GetFileAttributes function retrieves file system attribute information.</para>
      ///   <para>GetFileAttributesEx can obtain other sets of file or directory attribute information.</para>
      ///   <para>Currently, GetFileAttributesEx retrieves a set of standard attributes that is a superset of the file system attribute
      ///   information.
      ///   When the GetFileAttributesEx function is called on a directory that is a mounted folder, it returns the attributes of the directory,
      ///   not those of the root directory in the volume that the mounted folder associates with the directory. To obtain the attributes of
      ///   the associated volume, call GetVolumeNameForVolumeMountPoint to obtain the name of the associated volume. Then use the resulting
      ///   name in a call to GetFileAttributesEx. The results are the attributes of the root directory on the associated volume.</para>
      ///   <para>Symbolic link behavior: If the path points to a symbolic link, the function returns attributes for the symbolic link.</para>
      ///   <para>Minimum supported client: Windows XP [desktop apps only]</para>
      ///   <para>Minimum supported server: Windows Server 2003 [desktop apps only]</para>
      /// </remarks>
      /// <returns>
      ///   <para>If the function succeeds, the return value is nonzero.</para>
      ///   <para>If the function fails, the return value is zero. To get extended error information, call GetLastError.</para>
      /// </returns>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "GetFileAttributesExW")]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool GetFileAttributesEx([MarshalAs(UnmanagedType.LPWStr)] string lpFileName, [MarshalAs(UnmanagedType.U4)] GetFileExInfoLevels fInfoLevelId, out Win32FileAttributeData lpFileInformation);

      /// <summary>Retrieves attributes for a specified file or directory.</summary>
      /// <remarks>
      ///   <para>The GetFileAttributes function retrieves file system attribute information.</para>
      ///   <para>GetFileAttributesEx can obtain other sets of file or directory attribute information.</para>
      ///   <para>
      ///   Currently, GetFileAttributesEx retrieves a set of standard attributes that is a superset of the file system attribute information.
      ///   When the GetFileAttributesEx function is called on a directory that is a mounted folder, it returns the attributes of the directory,
      ///   not those of the root directory in the volume that the mounted folder associates with the directory. To obtain the attributes of
      ///   the associated volume, call GetVolumeNameForVolumeMountPoint to obtain the name of the associated volume. Then use the resulting
      ///   name in a call to GetFileAttributesEx. The results are the attributes of the root directory on the associated volume.</para>
      ///   <para>Symbolic link behavior: If the path points to a symbolic link, the function returns attributes for the symbolic link.</para>
      ///   <para>Transacted Operations</para>
      ///   <para>If a file is open for modification in a transaction, no other thread can open the file for modification until the transaction
      ///   is committed. Conversely, if a file is open for modification outside of a transaction, no transacted thread can open the file for
      ///   modification until the non-transacted handle is closed. If a non-transacted thread has a handle opened to modify a file, a call to
      ///   GetFileAttributesTransacted for that file will fail with an ERROR_TRANSACTIONAL_CONFLICT error.</para>
      ///   <para>Minimum supported client: Windows Vista [desktop apps only]</para>
      ///   <para>Minimum supported server: Windows Server 2008 [desktop apps only]</para>
      /// </remarks>
      /// <returns>
      ///   <para>If the function succeeds, the return value is nonzero.</para>
      ///   <para>If the function fails, the return value is zero. To get extended error information, call GetLastError.</para>
      /// </returns>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "GetFileAttributesTransactedW")]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool GetFileAttributesTransacted([MarshalAs(UnmanagedType.LPWStr)] string lpFileName, [MarshalAs(UnmanagedType.U4)] GetFileExInfoLevels fInfoLevelId, out Win32FileAttributeData lpFileInformation, SafeHandle hTransaction);

      /// <summary>Retrieves file information for the specified file.</summary>
      /// <returns>
      /// If the function succeeds, the return value is nonzero and file information data is contained in the buffer pointed to by the lpByHandleFileInformation parameter.
      /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
      /// </returns>
      /// <remarks>
      /// Depending on the underlying network features of the operating system and the type of server connected to,
      /// the GetFileInformationByHandle function may fail, return partial information, or full information for the given file.
      /// </remarks>
      /// <remarks>Minimum supported client: Windows XP [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003 [desktop apps only]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool GetFileInformationByHandle(SafeFileHandle hFile, [MarshalAs(UnmanagedType.Struct)] out ByHandleFileInfo lpByHandleFileInformation);

      /// <summary>
      ///   Retrieves file information for the specified file.
      /// </summary>
      /// <remarks>
      ///   <para>Minimum supported client: Windows Vista [desktop apps | Windows Store apps]</para>
      ///   <para>Minimum supported server: Windows Server 2008 [desktop apps | Windows Store apps]</para>
      ///   <para>Redistributable: Windows SDK on Windows Server 2003 and Windows XP.</para>
      /// </remarks>
      /// <param name="hFile">The file.</param>
      /// <param name="fileInfoByHandleClass">The file information by handle class.</param>
      /// <param name="lpFileInformation">Information describing the file.</param>
      /// <param name="dwBufferSize">Size of the buffer.</param>
      /// <returns>
      ///   <para>If the function succeeds, the return value is nonzero and file information data is contained in the buffer pointed to by the
      ///   lpByHandleFileInformation parameter.</para>
      ///   <para>If the function fails, the return value is zero. To get extended error information, call GetLastError.</para>
      /// </returns>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool GetFileInformationByHandleEx(SafeFileHandle hFile, [MarshalAs(UnmanagedType.I4)] FileInfoByHandleClass fileInfoByHandleClass, SafeGlobalMemoryBufferHandle lpFileInformation, [MarshalAs(UnmanagedType.U4)] uint dwBufferSize);

      /// <summary>
      ///   Retrieves the size of the specified file.
      /// </summary>
      /// <remarks>
      ///   <para>Minimum supported client: Windows XP [desktop apps only]</para>
      ///   <para>Minimum supported server: Windows Server 2003 [desktop apps only]</para>
      /// </remarks>
      /// <returns>
      ///   <para>If the function succeeds, the return value is nonzero.</para>
      ///   <para>If the function fails, the return value is zero. To get extended error information, call GetLastError.</para>
      /// </returns>
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool GetFileSizeEx(SafeFileHandle hFile, out long lpFileSize);

      /// <summary>Retrieves the final path for the specified file.</summary>
      /// <remarks>Minimum supported client: Windows Vista [desktop apps only].</remarks>
      /// <remarks>Minimum supported server: Windows Server 2008 [desktop apps only].</remarks>
      /// <returns>
      ///   If the function succeeds, the return value is nonzero. If the function fails, the return value is zero. To get extended error
      ///   information, call GetLastError.
      /// </returns>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "GetFinalPathNameByHandleW")]
      [return: MarshalAs(UnmanagedType.U4)]
      internal static extern uint GetFinalPathNameByHandle(SafeFileHandle hFile, StringBuilder lpszFilePath, [MarshalAs(UnmanagedType.U4)] uint cchFilePath, FinalPathFormats dwFlags);

      /// <summary>
      ///   Checks whether the specified address is within a memory-mapped file in the address space of the specified process. If so, the
      ///   function returns the name of the memory-mapped file.
      /// </summary>
      /// <remarks>Minimum supported client: Windows XP.</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003.</remarks>
      /// <returns>
      ///   If the function succeeds, the return value is nonzero. If the function fails, the return value is zero. To get extended error
      ///   information, call GetLastError.
      /// </returns>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("psapi.dll", SetLastError = false, CharSet = CharSet.Unicode, EntryPoint = "GetMappedFileNameW")]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool GetMappedFileName(IntPtr hProcess, SafeLocalMemoryBufferHandle lpv, StringBuilder lpFilename, [MarshalAs(UnmanagedType.U4)] uint nSize);

      /// <summary>Locks the specified file for exclusive access by the calling process.</summary>
      /// <remarks>Minimum supported client: Windows XP.</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003.</remarks>
      /// <returns>
      ///   If the function succeeds, the return value is nonzero (TRUE). If the function fails, the return value is zero (FALSE). To get
      ///   extended error information, call GetLastError.
      /// </returns>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool LockFile(SafeFileHandle hFile, [MarshalAs(UnmanagedType.U4)] uint dwFileOffsetLow, [MarshalAs(UnmanagedType.U4)] uint dwFileOffsetHigh, [MarshalAs(UnmanagedType.U4)] uint nNumberOfBytesToLockLow, [MarshalAs(UnmanagedType.U4)] uint nNumberOfBytesToLockHigh);

      /// <summary>Maps a view of a file mapping into the address space of a calling process.</summary>
      /// <remarks>Minimum supported client: Windows XP.</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003.</remarks>
      /// <returns>
      ///   If the function succeeds, the return value is the starting address of the mapped view. If the function fails, the return value is
      ///   <see langword="null"/>.
      /// </returns>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = false, CharSet = CharSet.Unicode)]
      internal static extern SafeLocalMemoryBufferHandle MapViewOfFile(SafeFileHandle hFileMappingObject, [MarshalAs(UnmanagedType.U4)] uint dwDesiredAccess, [MarshalAs(UnmanagedType.U4)] uint dwFileOffsetHigh, [MarshalAs(UnmanagedType.U4)] uint dwFileOffsetLow, UIntPtr dwNumberOfBytesToMap);

      /// <summary>
      ///   Moves a file or directory, including its children.
      ///   <para>You can provide a callback function that receives progress notifications.</para>
      /// </summary>
      /// <remarks>
      ///   <para>The MoveFileWithProgress function coordinates its operation with the link tracking service, so link sources can be tracked as
      ///   they are moved.</para>
      ///   <para>Minimum supported client: Windows XP [desktop apps only]</para>
      ///   <para>Minimum supported server: Windows Server 2003 [desktop apps only]</para>
      /// </remarks>
      /// <param name="lpExistingFileName">Filename of the existing file.</param>
      /// <param name="lpNewFileName">Filename of the new file.</param>
      /// <param name="lpProgressRoutine">The progress routine.</param>
      /// <param name="lpData">The data.</param>
      /// <param name="dwFlags">The flags.</param>
      /// <returns>
      ///   <para>If the function succeeds, the return value is nonzero.</para>
      ///   <para>If the function fails, the return value is zero. To get extended error information, call GetLastError.</para>
      /// </returns>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "MoveFileWithProgressW")]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool MoveFileWithProgress([MarshalAs(UnmanagedType.LPWStr)] string lpExistingFileName, [MarshalAs(UnmanagedType.LPWStr)] string lpNewFileName, NativeCopyMoveProgressRoutine lpProgressRoutine, IntPtr lpData, [MarshalAs(UnmanagedType.U4)] MoveOptions dwFlags);

      /// <summary>
      ///   Moves an existing file or a directory, including its children, as a transacted operation.
      ///   <para>You can provide a callback function that receives progress notifications.</para>      
      /// </summary>
      /// <remarks>
      ///   <para>Minimum supported client: Windows Vista [desktop apps only]</para>
      ///   <para>Minimum supported server: Windows Server 2008 [desktop apps only]</para>
      /// </remarks>     
      /// <returns>
      ///   <para>If the function succeeds, the return value is nonzero.</para>
      ///   <para>If the function fails, the return value is zero. To get extended error information, call GetLastError.</para>
      /// </returns>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "MoveFileTransactedW")]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool MoveFileTransacted([MarshalAs(UnmanagedType.LPWStr)] string lpExistingFileName, [MarshalAs(UnmanagedType.LPWStr)] string lpNewFileName, NativeCopyMoveProgressRoutine lpProgressRoutine, IntPtr lpData, [MarshalAs(UnmanagedType.U4)] MoveOptions dwCopyFlags, SafeHandle hTransaction);

      /// <summary>An application-defined callback function used with the CopyFileEx, MoveFileTransacted, and MoveFileWithProgress functions.
      /// <para>It is called when a portion of a copy or move operation is completed.</para>
      /// <para>The LPPROGRESS_ROUTINE type defines a pointer to this callback function.</para>
      /// <para>NativeCopyMoveProgressRoutine (NativeCopyMoveProgressRoutine) is a placeholder for the application-defined function name.</para>
      /// </summary>
      internal delegate CopyMoveProgressResult NativeCopyMoveProgressRoutine(long totalFileSize, long totalBytesTransferred, long streamSize, long streamBytesTransferred, [MarshalAs(UnmanagedType.U4)] uint dwStreamNumber, CopyMoveProgressCallbackReason dwCallbackReason, IntPtr hSourceFile, IntPtr hDestinationFile, IntPtr lpData);

      /// <summary>Determines whether a path to a file system object such as a file or folder is valid.</summary>
      /// <returns><see langword="true"/> if the file exists; otherwise, <see langword="false"/>. Call GetLastError for extended error information.</returns>
      /// <remarks>
      /// This function tests the validity of the path.
      /// A path specified by Universal Naming Convention (UNC) is limited to a file only; that is, \\server\share\file is permitted.
      /// A network share path to a server or server share is not permitted; that is, \\server or \\server\share.
      /// This function returns FALSE if a mounted remote drive is out of service.
      /// </remarks>
      /// <remarks>Minimum supported client: Windows 2000 Professional</remarks>
      /// <remarks>Minimum supported server: Windows 2000 Server</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("Shlwapi.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "PathFileExistsW")]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool PathFileExists([MarshalAs(UnmanagedType.LPWStr)] string pszPath);

      /// <summary>Replaces one file with another file, with the option of creating a backup copy of the original file. The replacement file assumes the name of the replaced file and its identity.</summary>
      /// <returns>
      /// If the function succeeds, the return value is nonzero.
      /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
      /// </returns>
      /// <remarks>Minimum supported client: Windows XP [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003 [desktop apps only]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "ReplaceFileW")]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool ReplaceFile([MarshalAs(UnmanagedType.LPWStr)] string lpReplacedFileName, [MarshalAs(UnmanagedType.LPWStr)] string lpReplacementFileName, [MarshalAs(UnmanagedType.LPWStr)] string lpBackupFileName, FileSystemRights dwReplaceFlags, IntPtr lpExclude, IntPtr lpReserved);

      /// <summary>Sets the attributes for a file or directory.</summary>
      /// <returns>
      /// If the function succeeds, the return value is nonzero.
      /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
      /// </returns>
      /// <remarks>Minimum supported client: Windows XP</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [SuppressMessage("Microsoft.Usage", "CA2205:UseManagedEquivalentsOfWin32Api")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "SetFileAttributesW")]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool SetFileAttributes([MarshalAs(UnmanagedType.LPWStr)] string lpFileName, [MarshalAs(UnmanagedType.U4)] FileAttributes dwFileAttributes);

      /// <summary>Sets the attributes for a file or directory as a transacted operation.</summary>
      /// <returns>
      /// If the function succeeds, the return value is nonzero.
      /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
      /// </returns>
      /// <remarks>Minimum supported client: Windows Vista [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2008 [desktop apps only]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "SetFileAttributesTransactedW")]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool SetFileAttributesTransacted([MarshalAs(UnmanagedType.LPWStr)] string lpFileName, [MarshalAs(UnmanagedType.U4)] FileAttributes dwFileAttributes, SafeHandle hTransaction);

      /// <summary>Sets the date and time that the specified file or directory was created, last accessed, or last modified.</summary>
      /// <returns>
      /// If the function succeeds, the return value is nonzero.
      /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
      /// </returns>
      /// <remarks>Minimum supported client: Windows XP [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003 [desktop apps only]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool SetFileTime(SafeFileHandle hFile, SafeGlobalMemoryBufferHandle lpCreationTime, SafeGlobalMemoryBufferHandle lpLastAccessTime, SafeGlobalMemoryBufferHandle lpLastWriteTime);

      /// <summary>Retrieves information about an object in the file system, such as a file, folder, directory, or drive root.</summary>
      /// <remarks>You should call this function from a background thread. Failure to do so could cause the UI to stop responding.</remarks>
      /// <remarks>Minimum supported client: Windows XP [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows 2000 Server [desktop apps only]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("shell32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "SHGetFileInfoW")]
      internal static extern IntPtr ShGetFileInfo([MarshalAs(UnmanagedType.LPWStr)] string pszPath, FileAttributes dwFileAttributes, [MarshalAs(UnmanagedType.Struct)] out Shell32.FileInfo psfi, [MarshalAs(UnmanagedType.U4)] uint cbFileInfo, [MarshalAs(UnmanagedType.U4)] Shell32.FileAttributes uFlags);

      /// <summary>Unlocks a region in an open file. Unlocking a region enables other processes to access the region.</summary>
      /// <returns>
      /// If the function succeeds, the return value is nonzero.
      /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
      /// </returns>
      /// <remarks>Minimum supported client: Windows XP</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool UnlockFile(SafeFileHandle hFile, [MarshalAs(UnmanagedType.U4)] uint dwFileOffsetLow, [MarshalAs(UnmanagedType.U4)] uint dwFileOffsetHigh, [MarshalAs(UnmanagedType.U4)] uint nNumberOfBytesToUnlockLow, [MarshalAs(UnmanagedType.U4)] uint nNumberOfBytesToUnlockHigh);

      /// <summary>Unmaps a mapped view of a file from the calling process's address space.</summary>
      /// <remarks>Minimum supported client: Windows XP.</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003.</remarks>
      /// <param name="lpBaseAddress">The base address.</param>
      /// <returns>
      ///   If the function succeeds, the return value is nonzero. If the function fails, the return value is zero. To get extended error
      ///   information, call GetLastError.
      /// </returns>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = false, CharSet = CharSet.Unicode)]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool UnmapViewOfFile(SafeLocalMemoryBufferHandle lpBaseAddress);

      [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
      public struct WIN32_FIND_STREAM_DATA
      {
         public long StreamSize;
         [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MaxPath + 36)]
         public string cStreamName;
      }

      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
      internal static extern SafeFindFileHandle FindFirstStreamTransactedW(string fileName, StreamInfoLevels infoLevel, SafeGlobalMemoryBufferHandle lpFindStreamData, int flags, SafeHandle hTransaction);

      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
      internal static extern SafeFindFileHandle FindFirstStreamW(string fileName, StreamInfoLevels infoLevel, SafeGlobalMemoryBufferHandle lpFindStreamData, int flags);

      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool FindNextStreamW(SafeFindFileHandle handle, SafeGlobalMemoryBufferHandle lpFindStreamData);

      public enum StreamInfoLevels
      {
         FindStreamInfoStandard = 0
      }
   }
}
