<#  Copyright (C) 2008-2017 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
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

    [OutputType([Void])]
    Param(
        [Object]$Instance,
        [String]$MethodName,
        [Type[]]$TypeParameters,
        [Object[]]$MethodParameters
    )

    [Collections.ArrayList]$Private:parameterTypes = @{}
    ForEach ($Private:paramType In $MethodParameters) { [Void]$parameterTypes.Add($paramType.GetType()) }

    $Private:method = $Instance.GetMethod($methodName, "Instance, Static, Public", $Null, $parameterTypes, $Null)

    If ($Null -eq $method) { Throw ('Method: [{0}] not found.' -f ($Instance.ToString() + '.' + $methodName)) }
    Else {
        $method = $method.MakeGenericMethod($TypeParameters)
        $method.Invoke($Instance, $MethodParameters)
    }
}


[ScriptBlock]$ReportException = {

<#
    .SYNOPSIS
        The callback function that is executed for each Exception
        that is thrown, while enumerating file system entries.

    .NOTE
        It seems that the callback is not called
        when this script is run from Windows PowerShell ISE.
#>

    [OutputType([Bool])]
    Param(
        [Int]$errorCode,
        [String]$errorMessage,
        [String]$pathProcessed
    )


    [Int]$Private:ERROR_ACCESS_DENIED = 5


    If ($errorCode -eq $ERROR_ACCESS_DENIED) { Write-Warning -Message ('Error: {0}  {1}  Path: [{2}]' -f $errorCode, $errorMessage, $pathProcessed) }

    # Continue enumeration.
    return $True
}



Function Enumerate-FileSystemEntryInfos {

<#
    .SYNOPSIS
        [Alphaleonis.Win32.Filesystem.Directory]::EnumerateFileSystemEntryInfos()
        AlphaFS 2.1+: A powerful folder/file enumerator which can recover from, and report, access denied exceptions.


    .EXAMPLE
        PS C:\> .\Enumerate-FileSystemEntryInfos.ps1 -Path $env:windir -Filter *.dll -Recurse


    .OUTPUTS
        An [Alphaleonis.Win32.Filesystem.FileSystemEntryInfo] instance:

        AlternateFileName   = []
		Attributes          = [Directory]
		CreationTime        = [22-8-2013 15:36:16]
		CreationTimeUtc     = [22-8-2013 13:36:16]
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
#>

    # SkipReparsePoints = Skip reparse points by default.
    # LargeCache        = Uses a larger buffer for directory queries, which can increase performance of the find operation.
    # BasicSearch       = The function does not query the short file name, improving overall enumeration speed.

    $Private:enumOptions = [Alphaleonis.Win32.Filesystem.DirectoryEnumerationOptions]
    [Alphaleonis.Win32.Filesystem.DirectoryEnumerationOptions]$Private:dirEnumOptions = $enumOptions::SkipReparsePoints -bor $enumOptions::LargeCache -bor $enumOptions::BasicSearch


    If ($ContinueOnException.IsPresent) { $dirEnumOptions = $dirEnumOptions -bor $enumOptions::ContinueOnException }
    If ($Recurse.IsPresent)             { $dirEnumOptions = $dirEnumOptions -bor $enumOptions::Recursive }

    If (-not $Directory.IsPresent -and -not $File.IsPresent) { $dirEnumOptions = $dirEnumOptions -bor $enumOptions::FilesAndFolders }

    If ($Directory.IsPresent) { $dirEnumOptions = $dirEnumOptions -bor $enumOptions::Folders }
    If ($File.IsPresent)      { $dirEnumOptions = $dirEnumOptions -bor $enumOptions::Files   }


    [Alphaleonis.Win32.Filesystem.DirectoryEnumerationFilters]$Private:dirEnumFilters = New-Object -TypeName Alphaleonis.Win32.Filesystem.DirectoryEnumerationFilters

    # The callback [ScriptBlock] to execute.
    $dirEnumFilters.ErrorFilter = $ReportException


    Write-Progress -Activity $Path -Status ('Processing input path: {0}' -f $Path)

    ForEach ($Private:fsei In (Invoke-GenericMethod `
        -Instance           ([Alphaleonis.Win32.Filesystem.Directory]) `
        -MethodName         EnumerateFileSystemEntryInfos `
        -TypeParameters     Alphaleonis.Win32.Filesystem.FileSystemEntryInfo `
        -MethodParameters   $Path, $Filter, $dirEnumOptions, $dirEnumFilters, ([Alphaleonis.Win32.Filesystem.PathFormat]::RelativePath))) {

        Write-Progress -Activity $fsei.FullPath -Status ('Processing input path: {0}' -f $Path)


        # Return only the path (as a string).
        #Write-Output $fsei.FullPath


        # Return FileSystemEntryInfo object.
        #Write-Output $fsei
    }
}


Import-Module -Name 'PATH TO AlphaFS.dll'

Enumerate-FileSystemEntryInfos
