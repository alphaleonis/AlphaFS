Changelog
=========

Version vNext  (xxxx-xx-xx)
-----------

### Bugs Fixed

- Issue #268: There are multiple warnings when building the documentation.
- Issue #286: Property `FileSystemEntryInfo.AlternateFileName` is always an empty string.
- Issue #292: `CopyOptions.CopySymbolicLink` not working in 2.1.2  (Thx v2kiran)
- Issue #325: `DeleteEmptySubdirectories` (with `recursive=true`) throws `System.IO.DirectoryNotFoundException`  (Thx kryvoplias)
- Issue #328: Several instances of `ArgumentException.ParamName` not set/used correctly  (Thx elgonzo)
- Issue #330: Correct the parameter order for Privilege class constructors using the `ArgumentNullException`.
- Issue #339: `Directory/File.Encrypt/Decrypt` should restore read-only attribute.
- Issue #340: DirectoryReadOnlyException inherits from System.IO.IOException, wrong?
- Issue #344: `Directory.Copy` throws `UnauthorizedAccessException` "The target file is a directory, not a file", while it is a file.
- Issue #349: `File.GetFileSystemEntryInfoCore` should throw `Directory/FileNotFoundException`, depending on `isFolder` argument.
- Issue #369: `Directory.EnumerateFileSystemEntryInfos` does not return subdirectories with spaces as name.  (Thx Lupinho)
- Issue #371: Fix `.gitignore` to accommodate new directory structure in AlphaFS.UnitTest project.  (Thx damiarnold)
- Issue #372: `SetFsoDateTimeCore` should always use `BackupSemantics`.  (Thx damiarnold)

### New Features/Enhancements

- Issue #212: Provide a way to retrieve errors when you choose to `ContinueOnException`
- Issue #273: Improve methods `Directory/File.CopyMoveCore`: Make code recursive-aware, skip additional path checks and validations.
- Issue #274: Improve methods `Directory/File.CopyMoveCore`: Improve detection of same volume.
- Issue #275: Improve methods `Directory/File.CopyMoveCore`: Eliminate recursion.
- Issue #277: `Directory.DeleteDirectoryCore()`: Eliminate recursion. 
- Issue #278: `Directory.DeleteEmptySubdirectoriesCore()`: Eliminate recursion.
- Issue #303: `Path.Constants.cs`: Don't use `CurrentCulture`  (Thx HugoRoss)
- Issue #306: Include `ShareInfoLevel.Info502` and set as a fallback in `GetShareInfoCore()`  (Thx damiarnold)
- Issue #314: Added Feature: `Directory.GetFileSystemEntryInfo`  (Thx besoft)
- Issue #331: Rename method `File/Directory.TransferTimestamps` to `CopyTimestamps`.
- Issue #335: Add overloaded methods to `File/Directory.TransferTimestamps` to apply to ReparsePoint.
- Issue #336: Implement methods for `Directory` class: `CreateJunction`, `DeleteJunction` and `ExistsJunction`.
- Issue #338: Add convenience method `Directory.IsEmpty`
- Issue #342: Add instance method: `FileInfo.IsLocked()`
- Issue #343: Add method `File.GetProcessForFileLock`
- Issue #345: `AlreadyExistsException` should only throw message from 1 error.
- Issue #348: Implement method `Directory.GetLinkTargetInfo`
- Issue #350: Add overloaded methods `Directory.GetFileSystemEntryInfo`
- Issue #351: Enable copying of Directory symbolic links.
- Issue #352: Ignore `NonInterpretedPathPrefix` in methods: `Path.GetFullPathCore` and `Path.GetLongPathCore`  (Thx besoft)
- Issue #353: Modify method `Directory.GetFileSystemEntryInfo` to return `FileSystemEntryInfo` structure for directories supporting also root directories, e.g., `C:\`  (Thx besoft)
- Issue #354: Add methods `File.GetFileId` and `Directory.GetFileId` to return a unique file identifier.  (Thx besoft)
- Issue #355: Methods throwing an `IOException` due to error code 17 (`ERROR_NOT_SAME_DEVICE`) now throw a specific exception (`NotSameDeviceException`)
- Issue #357: Added new Windows 10 property: `FILE_DAX_VOLUME` to `VolumeInfo` class.
- Issue #373: Improve method `Directory.CreateDirectory` to allow creating a folder consisting only of spaces.

### Breaking Changes

- Issue #350: Add overloaded methods `Directory.GetFileSystemEntryInfo`

   Current code to retrieve a directory using `File.GetFileSystemEntryInfo` will now fail.
   Use Directory.GetFileSystemEntryInfo instead.

- Issue #331: Rename method `File/Directory.TransferTimestamps` to `CopyTimestamps`.
   Currently non-breaking, the old methods are still there.


Version 2.1.3 (2017-06-05)
-------------

### Bugs Fixed

- Issue #288: `Directory.Exists` on root drive problem has come back with recent updates  (Thx warrenlbrown)
- Issue #289: `Alphaleonis.Win32.Network.Host.GetShareInfo` doesn't work since 2.1.0  (Thx Schoolmonkey/damiarnold)
- Issue #296: Folder rename (casing) throws IOException with HResult `ERROR_SAME_DRIVE`  (Thx doormalena)
- Issue #297: Incorrect domain returned from `Host.EnumerateDomainDfsRoot` when specifying domain  (Thx damiarnold)
- Issue #299: `FileInfo.MoveTo` and `DirectoryInfo.MoveTo` throw `ArgumentNullException` on empty destination path  (Thx doormalena)
- Issue #312: `Volume.EnumerateVolumes` skips first volume  (Thx springy76)
- Issue #313: `GetHostShareFromPath()` fails with spaces in share name  (Thx damiarnold)
- Issue #320: Minor changes in comments in `Win32Errors.cs` to eliminate compiler warnings.  (Thx besoft)
- Issue #321: `DirectoryInfo.CopyToMoveToCore()` calls `Path.GetExtendedLengthPathCore()` without `Transaction` parameter.


Version 2.1.2 (2016-10-30)
-------------

### Bugs Fixed

- Issue #270: Method `File.GetFileSystemEntryInfoCore` uses wildcard ? (questionmark) instead of * (asterisk)
- Issue #276: `Directory.DeleteDirectory()`: Method can get stuck in infinite loop.
- Issue #279: The unit tests for CRC32/64 are failing.


Version 2.1  (2016-09-29)
-----------

### New Features/Enhancements

- Issue #3: Added methods for backing up and restoring encrypted files:
	* `File.ImportEncryptedFileRaw`
	* `File.ExportEncryptedFileRaw`
	* `Directory.ImportEncryptedDirectoryRaw`
	* `Directory.ExportEncryptedDirectoryRaw`
- Issue #2  : Unit tests for methods: `File.OpenRead()`, `File.OpenText()` and `File.Replace()` are missing.
- Issue #101: The release now also contains a build targetting .NET 4.5.2.
- Issue #109: Add overloaded methods for `Host.EnumerateShares()`.
- Issue #112: Add `CreationTimeUtc`, `LastAccessTimeUtc` and `LastWriteTimeUtc` to "Info" classes.
- Issue #119: Fix `Path.IsLocalPath()` issues.
- Issue #125: AlphaFS is now CLSCompliant.
- Issue #127: Modify method `Volume.QueryDosDevice()` so that is doesn't rely on `Path.IsLocalPath()` anymore.
- Issue #130: Modify method `Path.LocalToUnc()` so that is doesn't rely on `Path.IsLocalPath()` anymore.
- Issue #131: Modify method `Path.GetPathRoot()` to handle UNC paths in long path format.
- Issue #132: Modify method `VolumeInfo()` constructor to better handle input paths.
- Issue #133: Add missing unit test `Host.GetHostShareFromPath()`.
- Issue #134: Improved upon `FindFileSystemEntryInfo.FindFirstFile()` when throwing `Directory-/FileNotFoundException()`.
- Issue #138: Modify `GetShareInfo()` to use `SafeGlobalMemoryBufferHandle` instead of `IntPtr`.
- Issue #139: Modify `GetDfsInfoInternal()` to use `SafeGlobalMemoryBufferHandle` instead of `IntPtr`.
- Issue #141: Remove obsolete Resources (resx) string messages.
- Issue #142: Move literal strings to Resources (resx).
- Issue #144: Add `DirectoryInfo.EnumerateXxx()` methods with support for `DirectoryEnumerationOptions` enum.
- Issue #151: Add `Directory.EnumerateXxx()` methods with support for `DirectoryEnumerationOptions`- and `PathFormat` enum.
- Issue #154: Modify private method `FindFileSystemEntryInfo.FindFirstFile()` to report the full path on Exception. 
- Issue #146: Add method `DirectoryInfo.EnumerateAlternateDataStreams()`.
- Issue #147: Add overloaded methods to set Reparse Point Timestamp. (rstarkov)
- Issue #150: Enhancement: `File.IsLocked()`
- Issue #158: Add SuppressUnmanagedCodeSecurity attribute to [DllImport] tag.
- Issue #184: `File.CreateSymbolicLink()` should throw `PlatformNotSupportedException()` if OS < Vista. 
- Issue #186: Replace WIN32 API `NativeMethods.GetVersionEx()` with `NativeMethods.RtlGetVersion()`.
- Issue #188: Make `ShareInfo` class property setters private: `ShareType`, `ResourceType`.
- Issue #189: Improve method `Utils.UnitSizeToText()`.
- Issue #190: Add overloaded methods for `File/Directory.Get/SetAccessControl()` that accept `SafeFileHandle`.
- Issue #191: Make class `BackupFileStream` sealed.
- Issue #192: Add `null`-checks to `SafeHandle.IsInvalid` usage.
- Issue #193: Use unicode version of WIN32 API `OpenEncryptedFileRaw()`.
- Issue #194: Add bitshift for Marshal.GetHRForException(ex) usage. 
- Issue #195: Add useful FileAttributes as properties to `FileSystemEntryInfo` class.
- Issue #199: Change `FindFileSystemEntryInfo.FindFirstFile()` to show actual path instead of inputpath on access error.
- Issue #214: Howto `Get-Filehash`.
- Issue #235: Implement unicode versions of methods: CM_Connect_Machine and CM_Get_Device_ID_Ex.
- Issue #239: Enable long path support for `File.CreateSymbolicLink()` source parameter.
- Issue #240: Add `KeepDotOrSpace` to `GetFullPathOptions` enum.
- Issue #241: Add method `Path.GetFullPath()` overload that supports `GetFullPathOptions` enum.
- Issue #245: Implement CRC-32/64 (Thanks to Damien Guard for implementing his code).
- Issue #247: Add method `FileInfo.GetHash()`.
- Issue #251: Implement unicode versions of `Directory.GetCurrentDirectory()` and `Directory.SetCurrentDirectory()`.
- Issue #266: Add PowerShell script: `Enumerate-FileSystemEntryInfos.ps1`

### Bugs Fixed

- Issue #50 : `Path.GetLongPath()` does not prefix on "C:\", should it?
- Issue #60 : Remove all use of "Problematic" methods such as `DangerousAddRef` and `DangerousGetHandle()`.
- Issue #160: `File.CreateSymbolicLink()` creates shortcut with no target. (martin-john-green)
- Issue #162: `File.AppendAllLines()` concatenates content into one line. (pavelhritonenko)
- Issue #166: `File.Exists` & `Directory.Exists` fail when path has leading space(s).
- Issue #168: Error on `File.Open()` with access-mode Append?
- Issue #169: `DirectoryInfo .ToString()` returns path with `\\UNC` prefix.
- Issue #176: At `DirectoryInfo.GetFileSystemInfos()`, Long path prefix of GLOBALROOT path is missing. (diontools)
- Issue #179: `Path.GetFileName()` with an empty string throws an exception. (brutaldev)
- Issue #180: Network connects methods hangs in Windows service when credentials fail. (brutaldev)
- Issue #181: `File.OpenWrite()` should create file if it doesn't exist. (Thomas Levesque)
- Issue #183: Add `SafeFileHandle` null check for BackupFileStream.Dispose. (diontools)
- Issue #185: Correct pinvoke signatures of `CreateSymbolicLink()` and `CreateSymbolicLinkTransacted()` functions.
- Issue #196: Replace usage of `ExtendedFileAttributes.None` with `ExtendedFileAttributes.Normal`.
- Issue #197: Fix: Prevent normalization of GlobalRootPrefix paths.
- Issue #198: `Path.GetRegularPathCore()` should not normalize `\\?\Volume` prefix.
- Issue #201: Some exceptions contain an incorrect `HRESULT` (Thomas Levesque)
- Issue #203: `Directory.GetDirectories()` and `Directory.GetFiles()` return absolute paths when given relative argument.
- Issue #204: Giving empty string to `Directory.GetFileName()` and related methods throws exception.
- Issue #206: `File.GetLastWriteTime()` throws exception for non-existing path.
- Issue #217: `File.Replace()` raises an exception.
- Issue #218: `Volume.GetVolumeInfo()` fails for global root paths.
- Issue #219: Mismatching Implementation to `System.IO.Path.GetDirectoryName()`.
- Issue #226: `DirectoryInfo` using searchoption.
- Issue #232: Enable null for destinationBackupFileName for `File.Replace()` and `FileInfo.Replace()`.
- Issue #234: `Path.CheckInvalidPathChars` breaks `IsPathRooted` for whitespace strings.
- Issue #242: `File.Open(file, System.IO.FileMode.Append)` does not append.
- Issue #244: `File.Copy(src, dst, true)` does not respect `FILE_ATTRIBUTE_READONLY`.
- Issue #246: Using `Directory.EnumerateFileSystemEntries()` recursively with a relative path may fail.
- Issue #248: `Directory.Move()` throws `FileNotFoundException` instead of `DirectoryNotFoundException` when source folder doesn't exist.
- Issue #249: Change `File.GetHashCore()` `.ToString("X2")` to `.ToString("X2", CultureInfo.InvariantCulture)`.
- Issue #252: Correct `FileSystemEntryInfos.FullPath` property when input path is a dot (current directory).
- Issue #253: Apply `Dispose()` to method `File.GetHashCore()`.
- Issue #254: Change `File.GetHashCore()` output from `.ToLowerInvariant()` to `.ToUpperInvariant()`.
- Issue #255: Creating Folder with Empty name. (ardestan)
- Issue #256: `Directory.Move()` not working over volumes with `MoveOptions.CopyAllowed`. (frontier777)
- Issue #263: `Directory.GetDirectories()` Method `(String, String, SearchOption)` with pattern "* " (ardestan)

### Breaking Changes

- Issue #113: Change names of time related properties on `FileSystemEntryInfo` to conform with `FileInfo/DirectoryInfo`.
- Issue #126: Suffix the name of all methods working with TxF with "Transacted".
- Issue #128: Remove `Path.IsLocalPath()` in favour of `Path.IsUncPath()`.
- Issue #140: Replace internal `DFS_INFO_4` structure with `DFS_INFO_9`.
- Issue #184: `File.CreateSymbolicLink()` should throw `PlatformNotSupportedException()` if OS < Vista. 
- Issue #250: Change `FileSystemEntryInfo.ToString()` to show full path instead of `ReparsePointTag`.


Version 2.0.1  (2015-02-07)
-------------

### Bugs Fixed

- Issue #104: `VolumeInfo.Refresh()` fails with `System.IO.IOException`: (234)".
- Issue #108: `Volume.Refresh()` should throw `DeviceNotReadyException`.
- Issue #110: `Directory.GetDirectoryRoot()` should throw `System.ArgumentException`.
- Issue #117: Fix `Path.GetFullPath()` issues. 
- Issue #123: When `Directory.Encrypt/Decrypt()` is non-recursive, only process the folder.
- Issue #124: Unit tests for long/short path are failing.


Version 2.0  (2015-01-16)
-----------
* New: The public key of AlphaFS.dll has changed, delay-signing is no longer used.
* New: Unit Tests, also act as code samples.
* New: Numerous bugfixes, optimizations and (AlphaFS) overloaded methods implementations.
* New: Complete implementation of .NET 4.5 File(Info) and Directory(Info) classes.
* New: Complete implementation of .NET 4.5 DriveInfo() class and with UNC support.
* New: Complete implementation of .NET 4.5 Path() class.
* New: Implemented Unicode aka "Long Path" handling for all Win32 API functions that support it.
* New: Added support for NuGet.
* New: Added support for building against .NET 4.0, 4.5, and 4.5.1 in addition to 3.5.
* New: Supports networking by enumerating hosts and shares (SMB/DFS) and connect/disconnect to/from network resources (AlphaFS.Network.Host() class).
* New: Supports working with NTFS ADS (Alternate Data Streams) on files and folders (AlphaFS.Filesystem.AlternateDataStreamInfo() class).
* New: Supports enumerating connected PnP devices (AlphaFS.Filesystem.Device() / AlphaFS.Filesystem.DeviceInfo() classes).
* New: Supports extracting icons from files and folders (AlphaFS.Filesystem.Shell32Info() class).
* New: Supports PathFormat parameter for numerous methods to control path normalization. This speeds up things internally (less string processing and GetFullPath() calls) and also enables working with files and folders with a trailing dot or space:
	* `RelativePath` (slow): (default) Path will be checked and resolved to an absolute path. Unicode prefix is applied.
	* `FullPath`  (fast): Path is an absolute path. Unicode prefix is applied.
	* `LongFullPath`  (fastest): Path is already an absolute path with Unicode prefix. Use as is.
* Mod: Enabled KernelTransaction parameter for all Win32 API functions that support it.
* Mod: Added public read only properties to class FileSystemInfo(). Available for: DirectoryInfo() / FileInfo():
	* EntryInfo	 : Instance of the FileSystemEntryInfo() class.
	* Transaction  : Represents the KernelTransaction that was passed to the constructor.	
* Mod: Added more entries to enum ReparsePointTag.
* Mod: Removed method Directory.CountFiles() and added method Directory.CountFileSystemObjects().	
* Mod: Removed method Directory.GetFullFileSystemEntries() and added method Directory.EnumerateFileSystemEntryInfos().
	* Note: This new method currently does not support DirectoryEnumerationExceptionHandler, this will probably be added in a future release.
* Mod: Renamed method Directory.GetFileIdBothDirectoryInfo() to Directory.EnumerateFileIdBothDirectoryInfo().
* Mod: Method Directory.CreateDirectory() signature change: Using template directory. Ability for NTFS-compressed folders added.
* Mod: Method Directory.GetProperties() signature change.
* Mod: Renamed method File.GetFileInformationByHandle() to File.GetFileInfoByHandle().
* Mod: Removed overloaded method File.GetFileInformationByHandle(FileStream).h
* Mod: Removed overloaded AlphaFS methods File.Move() using MoveFileOptions and CopyProgressRoutine, and added method File.Move().
* Mod: Renamed method Volume.GetDeviceForVolumeName() to Volume.GetVolumeDeviceName().
* Mod: Renamed method Volume.GetDisplayNameForVolume() to Volume.GetVolumeDisplayName().
* Mod: Renamed method Volume.GetVolumeInformation() to Volume.GetVolumeInfo().
* Mod: Renamed method Volume.GetVolumeMountPoints() to Volume.EnumerateVolumeMountPoints().
* Mod: Renamed method Volume.GetVolumePathNamesForVolume() to Volume.EnumerateVolumePathNames().
* Mod: Renamed method Volume.GetVolumes() to Volume.EnumerateVolumes().
* Mod: Method Volume.DefineDosDevice() signature change.
* Mod: Method Volume.QueryDosDevice() signature change.
* Mod: Method Volume.QueryAllDosDevices() signature change.
* Mod: Removed method Volume.GetLogicalDrives() in favor of method Directory.GetLogicalDrives().
* Mod: Class VolumeInfo() constructor signature change.
* Mod: Class VolumeInfo() properties updated/changed.
* Mod: Added method Volume.Refresh().
* Mod: Changed struct DiskSpaceInfo() to class.
* Mod: Class DiskSpaceInfo() constructor signature change.
* Mod: Class DiskSpaceInfo() properties added.
* Mod: Added method DiskSpaceInfo.Refresh().
* Mod: Refactored Path() class.
* Mod: Improved upon the correct (.NET) exceptions thrown. Added AlphaFS specific: DirectoryReadOnlyException and FileReadOnlyException.
* Removed classes PathInfoXxx().
* Removed method Path.IsValidPath(), was part of PathInfo() class.
* Removed IllegalPathException.
* Removed enum DriveType in favor of System.IO.DriveType enum.
* Removed enum FileAccess in favor of System.IO.FileAccess enum.
* Removed enum FileAttributes in favor of System.IO.FileAttributes enum.
* Removed enum FileMode in favor of System.IO.FileMode enum.
* Removed enum FileOptions in favor of System.IO.FileOptions enum.
* Removed enum FileShare in favor of System.IO.FileShare enum.
* Removed enum FileSystemRights in favor of System.Security.AccessControl.FileSystemRights enum.
* Removed enum FileType, obsolete.
* Removed enum EnumerationExceptionDecision, obsolete.
* Removed enum IoControlCode.cs, obsolete.
* Renamed enum CopyProgressResult to CopyMoveProgressResult.
* Renamed enum MoveFileOptions to MoveOptions.
* Renamed class DeviceIo to Device.
* Renamed delegate CopyProgressResult to CopyMoveProgressResult.


Version 1.5  (2014-05-20)
-----------
   * New: Various file system objects enumeration methods in Directory class.
   * Numerous bugfixes and optimizations
   * New: more unit tests
   * New: VS 2010 help file format, aka Help Viewer 1, dumped MS HELP 2 format


Version 1.0
-----------
  * New: Directory.GetFileIdBothDirectoryInfo, which provides access to the GetFileInformationByHandleEx Win32 API 
         function with the FileInformationClass set to FileIdBothDirectoryInfo.
  * New: Directory.CountFiles
  * Mod: Additional overloads for File.Open method.
  * Mod: FileAttributes.Invalid flag removed.
  * New: Directory.GetProperties method for retrieving aggregated information about files in a directory.
  * New: File.GetFileInformationByHandle added providing information about file index and link count.
  * New: KernelTransaction can now be created from a System.Transaction to participate in the ambient transaction
  * New: File.GetHardlinks providing an enumeration about all hardlinks pointing to the same file.
  * Mod: Many improvements and bug-fixes to Path/PathInfo path-parsing.
  * Mod: More functions for manipulating timestamps on files and directories.
  * Mod: Directory.GetFullFileSystemEntries added to provide more convenient usage of the FileSystemEnumerator.
  * Mod: ... and many more minor changes and fixes.

Version 0.7 alpha
-----------------
  * New: DirectoryInfo and FileInfo classes added
  * New: PathInfo.GetLongFullPath() and Path.GetLongFullPath() methods added
  * Mod: Path and PathInfo got many bugfixes, and some new functionality was added.
  * Mod: AlphaFS now targets the .NET Framework 2.0 instead of 3.5 previously.
  * Mod: KernelTransaction can now be created from, and participate in a System.Transactions.Transaction.
  * New: BackupFileStream added, in support of the BackupWrite(), BackupRead() and BackupSeek() functions from the Win32 API.
  * Mod: Inheritance structure for several classes was modified, mainly to add MarshalByRefObject to the relevant classes.
  * Mod: FileSystemEntryInfo was changed to a reference type (class) instead of the previous value type (struct).
  * Mod: PathInfo now accepts more types of internal paths, such as \\?\GLOBALROOT\Device\HarddiskVolumeShadowCopy5\ etc.
  * ... and many minor changes and fixes, not mentioned here.


Version 0.3.1
-------------
  * New: Added support for hardlinks and symbolic links in File.
  * New: Added Directory.EnableEncryption() and Directory.DisableEncryption()
  * New: Added File.GetCompressedSize()
  * Mod: Applied CLSCompliant(false) to the assembly
  * Mod: Improved error reporting, and cleanup of internal class NativeError.
  

Version 0.3.0
-------------
  * Initial release
