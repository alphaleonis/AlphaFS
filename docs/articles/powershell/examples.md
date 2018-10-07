## PowerShell Examples

This section contains various examples of how to perfom some common tasks in PowerShell using AlphaFS.

### Show static methods from Directory class.

    PS C:\> [Alphaleonis.Win32.Filesystem.Directory] | gm -Static -MemberType Method

    TypeName: Alphaleonis.Win32.Filesystem.Directory

    Name                             MemberType Definition
    ----                             ---------- ----------
    Compress                         Method     static void Compress(string path, Alphaleonis.Win32.Filesystem.Directory...
    Copy                             Method     static void Copy(string sourcePath, string destinationPath), static void...
    CountFileSystemObjects           Method     static long CountFileSystemObjects(string path, Alphaleonis.Win32.Filesy...
    CreateDirectory                  Method     static Alphaleonis.Win32.Filesystem.DirectoryInfo CreateDirectory(string...
    ...


### Get all overloaded methods of a particular method

    PS C:\> [Alphaleonis.Win32.Filesystem.Directory]::Copy

    OverloadDefinitions
    -------------------
    static void Copy(string sourcePath, string destinationPath)
    static void Copy(string sourcePath, string destinationPath, Alphaleonis.Win32.Filesystem.PathFormat pathFormat)
    static void Copy(string sourcePath, string destinationPath, bool overwrite)
    static void Copy(string sourcePath, string destinationPath, bool overwrite, Alphaleonis.Win32.Filesystem.PathFormat pathFormat)
    static void Copy(string sourcePath, string destinationPath, Alphaleonis.Win32.Filesystem.CopyOptions copyOptions)
    static void Copy(string sourcePath, string destinationPath, Alphaleonis.Win32.Filesystem.CopyOptions copyOptions, Alphaleonis.Win32.Filesystem.PathFormat pathFormat)
    ...

### AlphaFS References

    # Call the CreateDirectory() method from the Directory class.
    PS C:\> [Alphaleonis.Win32.Filesystem.Directory]::CreateDirectory('C:\MyFolder')

    # If you prefer a shorter reference, assign the TypeName to a PowerShell variable.
    PS C:\> $AlphaFSDir = [Alphaleonis.Win32.Filesystem.Directory]

    # And then use the short form.
    # Create a compressed directory.
    PS C:\> $AlphaFSDir::CreateDirectory('C:\MyCompressedFolder', $True)

### Emulate Get-ChildItem to overcome "Path Too Long"

    # How people learn about "Path Too Long".
    PS C:\> Get-ChildItem -Recurse -Path $folderPath

    # How salvation is offered.
    PS C:\> [Alphaleonis.Win32.Filesystem.Directory]::EnumerateFileSystemEntries($folderPath, '*', [System.IO.SearchOption]::AllDirectories)


### Get a FileSystemEntryInfo object of a file
This is the most powerful method to retrieve file or folder information!

    # Get a FileSystemEntryInfo object of a file.
    PS C:\> $fsei = [Alphaleonis.Win32.Filesystem.File]::GetFileSystemEntryInfo($Env:WinDir + '\notepad.exe')
    
    PS C:\> $fsei.GetType()

    IsPublic IsSerial Name                                     BaseType
    -------- -------- ----                                     --------
    True     True     FileSystemEntryInfo                      System.Object

    
    PS C:\> $fsei

    AlternateFileName   = []
    Attributes          = [Archive]
    CreationTime        = [22-8-2013 13:00:13]
    CreationTimeUtc     = [22-8-2013 11:00:13]
    Extension           = [.exe]
    FileName            = [notepad.exe]
    FileSize            = [217600]
    FullPath            = [C:\Windows\System32\notepad.exe]
    IsArchive           = [True]
    IsCompressed        = [False]
    IsDevice            = [False]
    IsDirectory         = [False]
    IsEncrypted         = [False]
    IsHidden            = [False]
    IsMountPoint        = [False]
    IsNormal            = [False]
    IsNotContentIndexed = [False]
    IsOffline           = [False]
    IsReadOnly          = [False]
    IsReparsePoint      = [False]
    IsSparseFile        = [False]
    IsSymbolicLink      = [False]
    IsSystem            = [False]
    IsTemporary         = [False]
    LastAccessTime      = [22-8-2013 13:00:13]
    LastAccessTimeUtc   = [22-8-2013 11:00:13]
    LastWriteTime       = [22-8-2013 13:00:12]
    LastWriteTimeUtc    = [22-8-2013 11:00:12]
    LongFullPath        = [\\?\C:\Windows\System32\notepad.exe]
    ReparsePointTag     = [None]
   
### Get a FileSystemEntryInfo object of a directory

    # Get a FileSystemEntryInfo object of a directory.
    PS C:\> $fsei = [Alphaleonis.Win32.Filesystem.File]::GetFileSystemEntryInfo($Env:WinDir)
    PS C:\> $fsei

    AlternateFileName   = []
    Attributes          = [Directory]
    CreationTime        = [22-8-2013 15:36:16]
    CreationTimeUtc     = [22-8-2013 13:36:16]
    Extension           = []
    FileName            = [Windows]
    FileSize            = [0]
    FullPath            = [C:\Windows]
    IsArchive           = [False]
    IsCompressed        = [False]
    IsDevice            = [False]
    IsDirectory         = [True]
    IsEncrypted         = [False]
    IsHidden            = [False]
    IsMountPoint        = [False]
    IsNormal            = [False]
    IsNotContentIndexed = [False]
    IsOffline           = [False]
    IsReadOnly          = [False]
    IsReparsePoint      = [False]
    IsSparseFile        = [False]
    IsSymbolicLink      = [False]
    IsSystem            = [False]
    IsTemporary         = [False]
    LastAccessTime      = [12-1-2018 15:54:39]
    LastAccessTimeUtc   = [12-1-2018 14:54:39]
    LastWriteTime       = [12-1-2018 15:54:39]
    LastWriteTimeUtc    = [12-1-2018 14:54:39]
    LongFullPath        = [\\?\C:\Windows]
    ReparsePointTag     = [None]

### Get a DirectoryInfo object of a directory

    # Get a DirectoryInfo object of a directory.
    PS C:\> $dirInfo = New-Object -TypeName Alphaleonis.Win32.Filesystem.DirectoryInfo($Env:WinDir)
    
    PS C:\> $dirInfo.GetType()

    IsPublic IsSerial Name                                     BaseType
    -------- -------- ----                                     --------
    True     True     DirectoryInfo                            Alphaleonis.Win32.Filesystem.FileSystemInfo

    
    PS C:\> $dirInfo

    Exists            : True
    Name              : WINDOWS
    Parent            :
    Root              : C:\
    Attributes        : Directory
    CreationTime      : 22-8-2013 15:36:15
    CreationTimeUtc   : 22-8-2013 13:36:15
    Extension         :
    FullName          : C:\WINDOWS
    LastAccessTime    : 28-12-2014 00:54:51
    LastAccessTimeUtc : 27-12-2014 23:54:51
    LastWriteTime     : 28-12-2014 00:54:51
    LastWriteTimeUtc  : 27-12-2014 23:54:51
    EntryInfo         : None
    Transaction       :

### Enumerate files and directories

     # Get all root folders from the Windows folder, starting with an 'a'
     PS C:\> [Alphaleonis.Win32.Filesystem.Directory]::EnumerateDirectories($Env:WinDir, 'a*')

     C:\Windows\addins
     C:\Windows\ADFS
     C:\Windows\AppCompat
     C:\Windows\apppatch
     C:\Windows\AppReadiness
     C:\Windows\assembly


     # Get all files and folders from a network share.
     PS C:\> $AlphaFSDir::EnumerateFileSystemEntries('\\server\share', '*', [System.IO.SearchOption]::AllDirectories)

For overall performance improvement, use the ```Directory.EnumerateXxx()``` versions as much as possible
and avoid using ```Directory.GetFileSystemEntries/GetDirectories/GetFiles``` functions.
When you are working with many files and/or directories, the ```Directory.EnumerateXxx()``` methods
can be more efficient.


### Copy a directory recursively

     # Set copy options.
     PS C:\> $copyOptions = [Alphaleonis.Win32.Filesystem.CopyOptions]::FailIfExists

     # Set source and destination directories.
     PS C:\> $source = 'C:\sourceDir'
     PS C:\> $destination = 'C:\destinationDir'

     # Copy directory recursively.
     PS C:\> [Alphaleonis.Win32.Filesystem.Directory]::Copy($source, $destination, $copyOptions)


**Copy directory recursively using a DirectoryInfo instance**:

     PS C:\> $dirInfo.CopyTo($destination)
     PS C:\> $dirInfo.CopyTo($destination, $copyOptions)

**Example Copy() Exception (missing source directory)**:

     Exception calling "Copy" with "3" argument(s): "(3) The system cannot find the path specified: [\\?\C:\sourceDir]"
     At line:1 char:2
     +  [Alphaleonis.Win32.Filesystem.Directory]::Copy($source, $destination, $copyOpt ...
     + ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
         + CategoryInfo          : NotSpecified: (:) [], MethodInvocationException
         + FullyQualifiedErrorId : DirectoryNotFoundException

### Copy a file

     # Copy file.
     PS C:\> [Alphaleonis.Win32.Filesystem.File]::Copy('C:\Folder\oldFile.txt', 'D:\Folder\newFile.txt')

### Remove file with a trailing space in the file name

     # Remove file, use path as is.
     # .NET does not allow for a file name to end with a space or a dot.
     # Use the [Alphaleonis.Win32.Filesystem.PathFormat]::FullPath parameter to bypass this.

     PS C:\> [Alphaleonis.Win32.Filesystem.File]::Delete('C:\Temp\file ', [Alphaleonis.Win32.Filesystem.PathFormat]::FullPath)

### Enumerate shares from a host

     PS C:\> [Alphaleonis.Win32.Network.Host]::EnumerateShares($Env:COMPUTERNAME, $True)

     CurrentUses        : 0
     DirectoryInfo      : \\SERVER001\ADMIN$
     NetFullPath        : \\SERVER001\ADMIN$
     MaxUses            : 4294967295
     NetName            : ADMIN$
     Password           :
     Path               : C:\WINDOWS
     Permissions        : None
     Remark             : Remote Admin
     SecurityDescriptor : 0
     ServerName         : SERVER001
     ShareType          : Special
     ResourceType       : None
     ShareLevel         : Info503

     ...

### Example: Operating System information

     PS C:\> [Alphaleonis.Win32.OperatingSystem]::IsServer
     False

     PS C:\> [Alphaleonis.Win32.OperatingSystem]::IsWow64Process
     False

     PS C:\> [Alphaleonis.Win32.OperatingSystem]::OSVersion
     Major  Minor  Build  Revision
     -----  -----  -----  --------
     6      3      9600   0

     PS C:\> [Alphaleonis.Win32.OperatingSystem]::ProcessorArchitecture
     X64

     PS C:\> [Alphaleonis.Win32.OperatingSystem]::ServicePackVersion
     Major  Minor  Build  Revision
     -----  -----  -----  --------
     0      0      -1     -1

     PS C:\> [Alphaleonis.Win32.OperatingSystem]::VersionName
     Windows81

     PS C:\> [Alphaleonis.Win32.OperatingSystem]::IsAtLeast([Alphaleonis.Win32.OperatingSystem+EnumOsName]::WindowsServer2003)
     True

### DriveInfo

    # System.IO DriveInfo()
    PS C:\> [System.IO.DriveInfo]('C')

    Name               : C:\
    DriveType          : Fixed
    DriveFormat        : NTFS
    IsReady            : True
    AvailableFreeSpace : 681759399936
    TotalFreeSpace     : 681759399936
    TotalSize          : 749786361856
    RootDirectory      : C:\
    VolumeLabel        : Windows8


    # AlphaFS DriveInfo()
    PS C:\> $driveInfo = [Alphaleonis.Win32.Filesystem.DriveInfo]('C')
    PS C:\> $driveInfo

    AvailableFreeSpace    = [22910414848]
    DiskSpaceInfo         = [C:\]
    DosDeviceName         = [C:]
    DriveFormat           = [NTFS]
    DriveType             = [Fixed]
    IsDosDeviceSubstitute = [False]
    IsReady               = [True]
    IsUnc                 = [False]
    IsVolume              = [True]
    Name                  = [C:\]
    RootDirectory         = [C:\]
    TotalFreeSpace        = [22910414848]
    TotalSize             = [96079966208]
    VolumeInfo            = [\\?\Volume{657d4f85-1da2-478a-946a-318c99706878}\]
    VolumeLabel           = [System]

    PS C:\> $driveInfo.DiskSpaceInfo

    AvailableFreeSpacePercent  = [23.85%]
    AvailableFreeSpaceUnitSize = [21.34 GB]
    BytesPerSector             = [512]
    ClusterSize                = [4096]
    DriveName                  = [C:\]
    FreeBytesAvailable         = [22910414848]
    NumberOfFreeClusters       = [5593363]
    SectorsPerCluster          = [8]
    TotalNumberOfBytes         = [96079966208]
    TotalNumberOfClusters      = [23457023]
    TotalNumberOfFreeBytes     = [22910414848]
    TotalSizeUnitSize          = [89.48 GB]
    UsedSpacePercent           = [76.15%]
    UsedSpaceUnitSize          = [68.14 GB]

    PS C:\> $driveInfo.VolumeInfo

    CasePreservedNames         = [True]
    CaseSensitiveSearch        = [True]
    Compression                = [True]
    DirectAccess               = [False]
    FileSystemName             = [NTFS]
    FullPath                   = [C:\]
    Guid                       = [\\?\Volume{657d4f85-1da2-478a-946a-318c99706878}\]
    MaximumComponentLength     = [255]
    Name                       = [System]
    NamedStreams               = [True]
    PersistentAcls             = [True]
    ReadOnlyVolume             = [False]
    SequentialWriteOnce        = [False]
    SerialNumber               = [1686851484]
    SupportsEncryption         = [True]
    SupportsExtendedAttributes = [True]
    SupportsHardLinks          = [True]
    SupportsObjectIds          = [True]
    SupportsOpenByFileId       = [True]
    SupportsRemoteStorage      = [False]
    SupportsReparsePoints      = [True]
    SupportsSparseFiles        = [True]
    SupportsTransactions       = [True]
    SupportsUsnJournal         = [True]
    UnicodeOnDisk              = [True]
    VolumeIsCompressed         = [False]
    VolumeQuotas               = [True]


    # Get the next available free drive letter (the example returns "E").
    PS C:\> $letter = [Alphaleonis.Win32.Filesystem.DriveInfo]::GetFreeDriveLetter()
    PS C:\> $letter
    E

### Browse folders using PrivilegeEnabler

    # Use the "Backup" privilege to browse folder: C:\Windows\CSC
    # Browsing without the PrivilegeEnabler results in ACCESS DENIED.
    $privilegeEnabler = $Null

    Try {
        $privilege = [Alphaleonis.Win32.Security.Privilege]::Backup
        $privilegeEnabler = New-Object Alphaleonis.Win32.Security.PrivilegeEnabler($privilege)

        Dir -Recurse C:\Windows\CSC
    }
    Finally {
        If ($Null -ne $privilegeEnabler) {
            $privilegeEnabler.Dispose()
        }
    }

### Get aggregated properties, including size, of a folder

     # Set directory enumeration options.
     PS C:\> $dirEnumOptions = [Alphaleonis.Win32.Filesystem.DirectoryEnumerationOptions]::Recursive -bor
                               [Alphaleonis.Win32.Filesystem.DirectoryEnumerationOptions]::SkipReparsePoints -bor
                               [Alphaleonis.Win32.Filesystem.DirectoryEnumerationOptions]::ContinueOnException

     PS C:\> $pathFormat = [Alphaleonis.Win32.Filesystem.PathFormat]::FullPath

     # Get aggregated properties, including size, of a folder.
     PS C:\> $properties = [Alphaleonis.Win32.Filesystem.Directory]::GetProperties('C:\', $dirEnumOptions, $pathFormat)

     PS C:\> $properties.Size
     188647966064

     # Show all aggregated properties for the folder.
     PS C:\> $properties | Format-Table -AutoSize

     Key                      Value
     ---                      -----
     Archive                 304390
     Compressed                3364
     Device                       0
     Directory                65178
     Encrypted                   10
     Hidden                    3960
     IntegrityStream              0
     Normal                   20954
     NoScrubData                  0
     NotContentIndexed         5555
     Offline                      0
     ReadOnly                  8278
     ReparsePoint                 0
     SparseFile                   1
     System                    2493
     Temporary                    6
     File                    325431
     Total                   390609
     Size              188647966064
