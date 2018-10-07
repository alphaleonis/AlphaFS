---
uid: methods-without-long-path-support
---
# Methods without long path support

> [!CAUTION]
> The following AlphaFS methods do not support long paths, due to MAX_PATH limitation of the Win32 API functions.
* `Directory/DirectoryInfo.DisableEncryption` 
* `Directory/DirectoryInfo.EnableEncryption`
* `File.CreateSymbolicLink()`
* `Shell32.GetFileAssociation()`
* `Shell32.GetFileIcon()`
* `Shell32.GetFileInfo()`

> [!NOTE]
> The following AlphaFS methods, with long path support, will work on Windows 10, version 1607

* `Directory.CurrentDirectory` 
* `Directory.SetCurrentDirectory`
* `File.CreateHardlink` 
* `File.CreateSymbolicLink` 
* `File.EnumerateHardlinks` 
* `File.GetCompressedSize`

**[TODO: Add links to the methods above]**