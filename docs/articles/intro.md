# Introduction

### Long Paths

AlphaFS automatically enables long path support for its methods.
This means you never have to fiddle again with long path prefixes such as : `\\?\` or `\\?\UNC\`

Most AlphaFS methods support long paths, please look here to see [methods without long path support](xref:methods-without-long-path-support).

AlphaFS provides a namespace: @Alphaleonis.Win32.Filesystem containing a number of classes. Most notable are replications of the @System.IO.Path, @System.IO.File, @System.IO.FileInfo, @System.IO.Directory and @System.IO.DirectoryInfo from the `System.IO` namespace.

So if you only use these `System.IO` classes, it is just a matter of replacing:
 `using System.IO`
with
`using Alphaleonis.Win32.Filesystem`
which makes AlphaFS a **true** drop-in replacement.


### Examples

    // Will all be handled as a long path by AlphaFS.
    Alphaleonis.Win32.Filesystem.File.Delete("C:\Data\test.txt");
    Alphaleonis.Win32.Filesystem.File.Delete("\\?\C:\Data\test.txt");
    Alphaleonis.Win32.Filesystem.File.Delete("\\host\c$\Data\test.txt");
    Alphaleonis.Win32.Filesystem.File.Delete("\\?\UNC\host\c$\Data\test.txt");

### PowerShell

To enable AlphaFS from PowerShell, an: `Import-Dll -Name 'C:\Path-to-AlphaFS.dll'` is needed first.
AlphaFS methods can be accessed in the following manner:

    PS C:\> [Alphaleonis.Win32.Filesystem.File]::Delete('C:\Data\test.txt')
    PS C:\> [Alphaleonis.Win32.Filesystem.File]::Delete('\\?\UNC\host\c$\Data\test.txt')

For more information see the [section on PowerShell](xref:powershell)

### Minimize Get FullPath and Path Validation

When a method is executed, the given path to the file or directory is first resolved to get the full path.
After that, numerous checks are applied to make sure that the supplied path is a valid path that can be used by the method.
Of course this all happens in the blink of an eye and you will never notice any delays. You can however reduce these checks and thus increase the overall speed of your script or application.

If you are going to apply multiple actions on a file or directory, (create, get attributes, copy, delete, ...)
you can either create a @Alphaleonis.Win32.Filesystem.FileInfo or @Alphaleonis.Win32.Filesystem.DirectoryInfo object and use this instance to do most of the processing, or you can make use of the @Alphaleonis.Win32.Filesystem.PathFormat parameter.

Some methods support the @Alphaleonis.Win32.Filesystem.PathFormat parameter. To see if a method supports the @Alphaleonis.Win32.Filesystem.PathFormat, simply do this:

    PS C:\> [Alphaleonis.Win32.Filesystem.File]::Delete

    OverloadDefinitions
    -------------------
    static void Delete(string path)
    static void Delete(string path, bool ignoreReadOnly, Alphaleonis.Win32.Filesystem.PathFormat pathFormat)
    static void Delete(string path, bool ignoreReadOnly)

We see that the second overload supports the @Alphaleonis.Win32.Filesystem.PathFormat parameter, now let's use it. First, we create a long path string and store it in a variabele:

    PS C:\> $longPath = [Alphaleonis.Win32.Filesystem.Path]::GetLongPath('\\host\c$\Data\test.txt')`
    PS C:\> $longPath
    \\?\UNC\host\c$\Data\test.txt

Second, we supply the long path variable to the AlphaFS method:

    PS C:\> [Alphaleonis.Win32.Filesystem.File]::Delete($longPath, $True, [Alphaleonis.Win32.Filesystem.PathFormat]::LongFullPath)

And that's it! AlphaFS sees the `LongFullPath` parameter and automatically skips full path resolving and
path validation checks. Note that these have already been applied when using the `Alphaleonis.Win32.Filesystem.Path.GetLongPath()` method.


### Prefer Directory.EnumerateXxx() methods instead of Directory.GetXxx() methods

The `EnumerateDirectories()` and `GetDirectories()` methods differ as follows: When you use `EnumerateDirectories()`, you can start enumerating the collection of names
before the whole collection is returned; when you use `GetDirectories()`, you must wait for the whole array of names to be returned before you can access the array.

So, the big real-life difference: `GetXxx()` methods will fill up your memory with the entire array!
Therefore, when you are working with many files and directories, `EnumerateXxx()` can be more efficient.


### Kernel Transaction Manager

"The [Kernel Transaction Manager](https://msdn.microsoft.com/en-us/library/mt481588(v=vs.110).aspx) (KTM) enables the development of applications that use transactions. The transaction engine itself is within the kernel, but transactions can be developed for kernel- or user-mode transactions, and within a single host or among distributed hosts."

Example of using Transactions (TxF)

    using (var ts = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.RequiresNew))
    {
        // KernelTransaction is in AlphaFS.
        var kt = new KernelTransaction(System.Transactions.Transaction.Current);

        // Append "hello" to text file named "text.txt"
        Alphaleonis.Win32.Filesystem.File.WriteAllText(kt, "text.txt", "hello");

        // No text appended because exception will be thrown.
        throw new Exception("oops");

        ts.Complete();
    }


### FileSystemEntryInfo

AlphaFS fully supports the .NET `DirectoryInfo` and `FileInfo`, and some more.

	Instance: [System.IO.DirectoryInfo]

	#001	Attributes        = [Directory]
	#002	CreationTime      = [30-5-2017 18:07:28]
	#003	CreationTimeUtc   = [30-5-2017 16:07:28]
	#004	Exists            = [True]
	#005	Extension         = [.rh5]
	#006	FullName          = [\\SERVER\C$\Temp\DirectoryInfo_InitializeInstance_ExistingDirectory-e43eb9\teljluhv.rh5]
	#007	LastAccessTime    = [30-5-2017 18:07:28]
	#008	LastAccessTimeUtc = [30-5-2017 16:07:28]
	#009	LastWriteTime     = [30-5-2017 18:07:28]
	#010	LastWriteTimeUtc  = [30-5-2017 16:07:28]
	#011	Name              = [teljluhv.rh5]
	#012	Parent            = [DirectoryInfo_InitializeInstance_ExistingDirectory-e43eb9]
	#013	Root              = [\\SERVER\C$]


	Instance: [Alphaleonis.Win32.Filesystem.DirectoryInfo]

	#001	Attributes        = [Directory]
	#002	CreationTime      = [30-5-2017 18:07:28]
	#003	CreationTimeUtc   = [30-5-2017 16:07:28]
	#004	EntryInfo         = [\\SERVER\C$\Temp\DirectoryInfo_InitializeInstance_ExistingDirectory-e43eb9\teljluhv.rh5]
	#005	Exists            = [True]
	#006	Extension         = [.rh5]
	#007	FullName          = [\\SERVER\C$\Temp\DirectoryInfo_InitializeInstance_ExistingDirectory-e43eb9\teljluhv.rh5]
	#008	LastAccessTime    = [30-5-2017 18:07:28]
	#009	LastAccessTimeUtc = [30-5-2017 16:07:28]
	#010	LastWriteTime     = [30-5-2017 18:07:28]
	#011	LastWriteTimeUtc  = [30-5-2017 16:07:28]
	#012	Name              = [teljluhv.rh5]
	#013	Parent            = [DirectoryInfo_InitializeInstance_ExistingDirectory-e43eb9]
	#014	Root 

        

	Instance: [System.IO.FileInfo]

	#001	Attributes        = [-1]
	#002	CreationTime      = [1-1-1601 01:00:00]
	#003	CreationTimeUtc   = [1-1-1601 00:00:00]
	#004	Directory         = [\\SERVER\C$\Temp\FileInfo_InitializeInstance_NonExistingFile-401668]
	#005	DirectoryName     = [\\SERVER\C$\Temp\FileInfo_InitializeInstance_NonExistingFile-401668]
	#006	Exists            = [False]
	#007	Extension         = [.txt]
	#008	FullName          = [\\SERVER\C$\Temp\FileInfo_InitializeInstance_NonExistingFile-401668\mf0b4cbh.om5.txt]
	#009	IsReadOnly        = [True]
	#010	LastAccessTime    = [1-1-1601 01:00:00]
	#011	LastAccessTimeUtc = [1-1-1601 00:00:00]
	#012	LastWriteTime     = [1-1-1601 01:00:00]
	#013	LastWriteTimeUtc  = [1-1-1601 00:00:00]
	#014	Length            = [Property accessor 'Length' on object 'System.IO.FileInfo' threw the following exception:'Could not find file '\\SERVER\C$\Temp\FileInfo_InitializeInstance_NonExistingFile-401668\mf0b4cbh.om5.txt'.']
	#015	Name              = [mf0b4cbh.om5.txt]


	Instance: [Alphaleonis.Win32.Filesystem.FileInfo]

	#001	Attributes        = [-1]
	#002	CreationTime      = [1-1-1601 01:00:00]
	#003	CreationTimeUtc   = [1-1-1601 00:00:00]
	#004	Directory         = [\\SERVER\C$\Temp\FileInfo_InitializeInstance_NonExistingFile-401668]
	#005	DirectoryName     = [\\SERVER\C$\Temp\FileInfo_InitializeInstance_NonExistingFile-401668]
	#006	EntryInfo         = [null]
	#007	Exists            = [False]
	#008	Extension         = [.txt]
	#009	FullName          = [\\SERVER\C$\Temp\FileInfo_InitializeInstance_NonExistingFile-401668\mf0b4cbh.om5.txt]
	#010	IsReadOnly        = [True]
	#011	LastAccessTime    = [1-1-1601 01:00:00]
	#012	LastAccessTimeUtc = [1-1-1601 00:00:00]
	#013	LastWriteTime     = [1-1-1601 01:00:00]
	#014	LastWriteTimeUtc  = [1-1-1601 00:00:00]
	#015	Length            = [Property accessor 'Length' on object 'Alphaleonis.Win32.Filesystem.FileInfo' threw the following exception:'(2) The system cannot find the file specified: [\\?\UNC\SERVER\C$\Temp\FileInfo_InitializeInstance_NonExistingFile-401668\mf0b4cbh.om5.txt]']
	#016	Name              = [mf0b4cbh.om5.txt]


Both `DirectoryInfo` and `FileInfo` contain the `EntryInfo` property containing
lots of other useful information:

	Instance: [Alphaleonis.Win32.Filesystem.FileSystemEntryInfo]

	#001	AlternateFileName = []
	#002	Attributes        = [Directory]
	#003	CreationTime      = [22-8-2013 15:36:16]
	#004	CreationTimeUtc   = [22-8-2013 13:36:16]
	#005	FileName          = [Windows]
	#006	FileSize          = [0]
	#007	FullPath          = [C:\Windows]
	#008	IsCompressed      = [False]
	#009	IsDirectory       = [True]
	#010	IsEncrypted       = [False]
	#011	IsHidden          = [False]
	#012	IsMountPoint      = [False]
	#013	IsOffline         = [False]
	#014	IsReadOnly        = [False]
	#015	IsReparsePoint    = [False]
	#016	IsSymbolicLink    = [False]
	#017	LastAccessTime    = [30-5-2017 15:54:40]
	#018	LastAccessTimeUtc = [30-5-2017 13:54:40]
	#019	LastWriteTime     = [30-5-2017 15:54:40]
	#020	LastWriteTimeUtc  = [30-5-2017 13:54:40]
	#021	LongFullPath      = [\\?\C:\Windows]
	#022	ReparsePointTag   = [None]


	Instance: [Alphaleonis.Win32.Filesystem.FileSystemEntryInfo]

	#001	AlternateFileName = []
	#002	Attributes        = [Archive]
	#003	CreationTime      = [22-8-2013 13:00:13]
	#004	CreationTimeUtc   = [22-8-2013 11:00:13]
	#005	FileName          = [notepad.exe]
	#006	FileSize          = [217600]
	#007	FullPath          = [\\SERVER\C$\Windows\System32\notepad.exe]
	#008	IsCompressed      = [False]
	#009	IsDirectory       = [False]
	#010	IsEncrypted       = [False]
	#011	IsHidden          = [False]
	#012	IsMountPoint      = [False]
	#013	IsOffline         = [False]
	#014	IsReadOnly        = [False]
	#015	IsReparsePoint    = [False]
	#016	IsSymbolicLink    = [False]
	#017	LastAccessTime    = [22-8-2013 13:00:13]
	#018	LastAccessTimeUtc = [22-8-2013 11:00:13]
	#019	LastWriteTime     = [22-8-2013 13:00:12]
	#020	LastWriteTimeUtc  = [22-8-2013 11:00:12]
	#021	LongFullPath      = [\\?\UNC\SERVER\C$\Windows\System32\notepad.exe]
	#022	ReparsePointTag   = [None]


Most properties are lazy loading and cached. Call the `directoryInfo.RefreshEntryInfo()` or `fileInfo.RefreshEntryInfo()` instance method to update the information.

At your disposal there are two more static methods you can use to obtain a `FileSystemEntryInfo` instance to a file or folder: `File.GetFileSystemEntryInfo()` and `Directory.EnumerateFileSystemEntries()`.
