Changelog
=========

Version 2.1
-----------
### New Features/Enhancements

- Issue #3: Added methods for backing up and restoring encrypted files:
	* `File.ImportEncryptedFileRaw`
	* `File.ExportEncryptedFileRaw`
	* `Directory.ImportEncryptedDirectoryRaw`
	* `Directory.ExportEncryptedDirectoryRaw`

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
- Issue #147: Add overloaded methods to set Reparse Point Timestamp. (Thanks rstarkov!)
- Issue #150: Enhancement: File.IsLocked()

### Bugs Fixed

- Issue #50: `Path.GetLongPath()` does not prefix on "C:\", should it?
- Issue #60: Remove all use of "Problematic" methods such as `DangerousAddRef` and `DangerousGetHandle()`.
- Issue #162 AppendAllLines concatenates content into one line. (pavelhritonenko)
- Issue #176 At DirectoryInfo.GetFileSystemInfos, Long path prefix of GLOBALROOT path is missing. (diontools)
- Issue #181 OpenWrite should create file if it doesn't exist. (Thomas Levesque)
- Issue #183 Add SafeFileHandle null check for BackupFileStream.Dispose. (diontools)
- Isses #179 Path.GetFileName with an empty string throws an exception. (brutaldev)

### Breaking Changes

- Issue #113: Change names of time related properties on `FileSystemEntryInfo` to conform with `FileInfo/DirectoryInfo`.
- Issue #126: Suffix the name of all methods working with TxF with "Transacted".
- Issue #128: Remove `Path.IsLocalPath()` in favour of `Path.IsUncPath()`.
- Issue #140: Replace internal `DFS_INFO_4` structure with `DFS_INFO_9`.

Version 2.0.1
-------------

### Bugs Fixed

- Issue #104: `VolumeInfo.Refresh()` fails with `System.IO.IOException`: (234)".
- Issue #108: `Volume.Refresh()` should throw `DeviceNotReadyException`.
- Issue #110: `Directory.GetDirectoryRoot()` should throw `System.ArgumentException`.
- Issue #117: Fix `Path.GetFullPath()` issues. 
- Issue #123: When `Directory.Encrypt/Decrypt()` is non-recursive, only process the folder.
- Issue #124: Unit tests for long/short path are failing.

Version 2.0
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

Version 1.5
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
