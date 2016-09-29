<#  Copyright (C) 2008-2016 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
 #  
 #  Permission is hereby granted, free of charge, to any person obtaining a copy 
 #  of this software and associated documentation files (the "Software"), to deal 
 #  in the Software without restriction, including without limitation the rights 
 #  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell 
 #  copies of the Software, and to permit persons to whom the Software is 
 #  furnished to do so, subject to the following conditions:
 #  
 #  The above copyright notice and this permission notice shall be included in 
 #  all copies or substantial portions of the Software.
 #  
 #  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
 #  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
 #  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
 #  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
 #  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
 #  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN 
 #  THE SOFTWARE. 
 #>
 

Param(
    [String]$Path = '.',
	[String]$Filter = '*',
	[Switch]$Recurse,
	[Switch]$ContinueOnException,
	[Switch]$Directory,
	[Switch]$File
)


Function Invoke-GenericMethod {

<#
    .SYNOPSIS
        Function to call a C# method with generic parameters.
#>

    Param(
        [Object]$Instance,
        [String]$MethodName,
        [Type[]]$TypeParameters,
        [Object[]]$MethodParameters
    )

    [Collections.ArrayList]$Private:parameterTypes = @{}
    ForEach ($Private:paramType In $MethodParameters) { [Void]$parameterTypes.Add($paramType.GetType()) }

    $Private:method = $Instance.GetMethod($methodName, "Instance,Static,Public", $Null, $parameterTypes, $Null)

    If ($Null -eq $method) { Throw ('Method: [{0}] not found.' -f ($Instance.ToString() + '.' + $methodName)) }
    Else {
        $method = $method.MakeGenericMethod($TypeParameters)
        $method.Invoke($Instance, $MethodParameters)
    }
}


Function Enumerate-FileSystemEntryInfos {

<#
    .SYNOPSIS
        [Alphaleonis.Win32.Filesystem.Directory]::EnumerateFileSystemEntryInfos()
        AlphaFS 2.1+: A powerful folder/file enumerator which can recover from access denied exceptions.


    .EXAMPLE
        PS C:\> .\Enumerate-FileSystemEntryInfos.ps1 -Path $env:windir -Filter *.dll -Recurse -ContinueOnException


    .OUTPUTS
        An [Alphaleonis.Win32.Filesystem.FileSystemEntryInfo] instance:

        AlternateFileName : 
        Attributes        : Archive
        CreationTime      : 27-4-2016 01:01:14
        CreationTimeUtc   : 26-4-2016 23:01:14
        FileName          : notepad.exe
        FileSize          : 215040
        FullPath          : C:\windows\notepad.exe
        IsCompressed      : False
        IsHidden          : False
        IsDirectory       : False
        IsEncrypted       : False
        IsMountPoint      : False
        IsOffline         : False
        IsReadOnly        : False
        IsReparsePoint    : False
        IsSymbolicLink    : False
        LastAccessTime    : 27-4-2016 01:01:14
        LastAccessTimeUtc : 26-4-2016 23:01:14
        LastWriteTime     : 3-8-2015 03:19:54
        LastWriteTimeUtc  : 3-8-2015 01:19:54
        LongFullPath      : \\?\C:\windows\notepad.exe
        ReparsePointTag   : None
#>

    # Skip ReparsePoints by default.
	$Private:dirEnumOptions = [Alphaleonis.Win32.Filesystem.DirectoryEnumerationOptions]::SkipReparsePoints

	If ($ContinueOnException.IsPresent) { [Alphaleonis.Win32.Filesystem.DirectoryEnumerationOptions]$dirEnumOptions = $dirEnumOptions -bor [Alphaleonis.Win32.Filesystem.DirectoryEnumerationOptions]::ContinueOnException }
	If ($Recurse.IsPresent)             { [Alphaleonis.Win32.Filesystem.DirectoryEnumerationOptions]$dirEnumOptions = $dirEnumOptions -bor [Alphaleonis.Win32.Filesystem.DirectoryEnumerationOptions]::Recursive }

	If (-not $Directory.IsPresent -and -not $File.IsPresent) { [Alphaleonis.Win32.Filesystem.DirectoryEnumerationOptions]$dirEnumOptions = $dirEnumOptions -bor [Alphaleonis.Win32.Filesystem.DirectoryEnumerationOptions]::FilesAndFolders }
	Else {
		If ($Directory.IsPresent) { [Alphaleonis.Win32.Filesystem.DirectoryEnumerationOptions]$dirEnumOptions = $dirEnumOptions -bor [Alphaleonis.Win32.Filesystem.DirectoryEnumerationOptions]::Folders }
		If ($File.IsPresent)      { [Alphaleonis.Win32.Filesystem.DirectoryEnumerationOptions]$dirEnumOptions = $dirEnumOptions -bor [Alphaleonis.Win32.Filesystem.DirectoryEnumerationOptions]::Files }
	}
	

	ForEach ($Private:fsei In (Invoke-GenericMethod `
		-Instance           ([Alphaleonis.Win32.Filesystem.Directory]) `
		-MethodName         EnumerateFileSystemEntryInfos `
		-TypeParameters     Alphaleonis.Win32.Filesystem.FileSystemEntryInfo `
		-MethodParameters   $Path, $Filter, $dirEnumOptions, ([Alphaleonis.Win32.Filesystem.PathFormat]::RelativePath))) {

		Write-Output $fsei
	}
}


Import-Module -Name 'PATH TO AlphaFS.dll'

Enumerate-FileSystemEntryInfos
