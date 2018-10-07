---
uid: powershell
---
## Getting Started with AlphaFS in PowerShell

* Make sure that AlphaFS.dll is not blocked, due to archive [download](https://github.com/alphaleonis/AlphaFS/releases).
* Check: _File Properties_ > _Details_ and click button/checkmark "**Unblock**" if shown.
* Also, make sure you use the correct .NET version of the AlphaFS.dll file.

**1. Get the currently used .NET version**

    PS C:\> $PSVersionTable.CLRVersion

    Major  Minor  Build  Revision
    -----  -----  -----  --------
    4      0      30319  42000


**2. Import Module**

    # Load AlphaFS (Method 1).
    PS C:\> Import-Module -Name 'C:\AlphaFS 2.2\Lib\Net4.0\AlphaFS.dll'

    # Load AlphaFS (Method 2).
    # Returns a System.Reflection.Assembly instance.
    PS C:\> $assembly = [Reflection.Assembly]::LoadFile('C:\AlphaFS 2.2\Lib\Net4.0\AlphaFS.dll')
    PS C:\> $assembly

    GAC    Version        Location
    ---    -------        --------
    False  v4.0.30319     C:\AlphaFS 2.2\Lib\Net4.0\AlphaFS.dll

**3. AlphaFS Classes**

    (.NET) Alphaleonis.Win32.Filesystem.Directory
    (.NET) Alphaleonis.Win32.Filesystem.DirectoryInfo
    (.NET) Alphaleonis.Win32.Filesystem.File
    (.NET) Alphaleonis.Win32.Filesystem.FileInfo
    (.NET) Alphaleonis.Win32.Filesystem.Path
    (AlphaFS) Alphaleonis.Win32.Filesystem.Device
    (AlphaFS) Alphaleonis.Win32.Network.Host

A plethora of functions (methods) is available, check the documentation at the
website: [http://alphafs.alphaleonis.com](http://alphafs.alphaleonis.com)
