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


 [CmdletBinding()]
 Param(
    [String]$Path = "$Env:WinDir\System32",
    [String]$Filter = '*',
    [Switch]$Recurse,
    [Switch]$ShowProgress,
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

    $Private:method = $Instance.GetMethod($methodName, "Instance, Static, Public", $Null, $parameterTypes, $Null)

    If ($Null -eq $method) { Throw ('Method not found: {0}.{1}' -f $Instance.ToString(), $methodName) }

    $method = $method.MakeGenericMethod($TypeParameters)

    $method.Invoke($Instance, $MethodParameters)
}


[ScriptBlock]$ErrorFilter = {

<#
    .SYNOPSIS
        Filter to process Exception handling.
#>

    [OutputType([Bool])]
    Param(
        [Int]$errorCode,
        [String]$errorMessage,
        [String]$pathProcessed
    )


    Process {
        [Int]$Private:ERROR_ACCESS_DENIED = 5
        [Bool]$Private:gotException = $False


        $gotException = $errorCode -eq $ERROR_ACCESS_DENIED

        Write-Warning -Message ('[{0:000}] Error: ({1}) "{2}"  Path: [{3}]' -f ++$Script:ErrorCount, $errorCode, $errorMessage, $pathProcessed)


        If ($MaxErrorCount -eq $ErrorCount) {
            Write-Warning -Message 'Maximum Error count reached. Aborting enumeration.'

            $cancelSource.Cancel()

            $gotException = $False
        }


        # When $True continue enumeration.
        return $gotException
    }
}


[ScriptBlock]$InclusionFilter = {

<#
    .SYNOPSIS
        Filter to in-/exclude file system entries during the enumeration.


    .NOTE
        The InclusionFilter provides better filter criteria than the -Filter argument.
#>

    [OutputType([Bool])]
    Param(
        [Alphaleonis.Win32.Filesystem.FileSystemEntryInfo]$fsei
    )


    Process {
        ++$Script:FsoCount

        # Example: Only allow files/folders with a certain extension.
        $Private:findExtensions = '.txt', '.ini', '.log'

        [Bool]$Private:gotMatch = $findExtensions -ccontains $fsei.Extension

        If ($gotMatch) {
            ++$Script:FoundFsoCount

            # Do any other processing here.
            If ($fsei.IsDirectory) {
                # ...
            }

            Else {
                # ...
            }
        }


        [String]$Private:status = ('Processed: [{0:N0}] Found: [{1:N0}] Errors: [{2:N0}]' -f $FsoCount, $FoundFsoCount, $ErrorCount)

        # Write-Progress WILL slow things down CONSIDERABLY; Disable when there is no need for progress report.
        If ($ShowProgress) { Write-Progress -Activity $fsei.FullPath -Status $status }

        # A nice alternative.
        Else { $Host.UI.RawUI.WindowTitle = $status }


        If ($MaxFsoFoundCount -eq $FoundFsoCount) {
            Write-Warning -Message 'Maximum file system entries found. Aborting enumeration.'

            $cancelSource.Cancel()

            $gotMatch = $False
        }


        # When $True the file system entry is returned.
        Return $gotMatch
    }
}


[ScriptBlock]$RecursionFilter = {

<#
    .SYNOPSIS
        Filter to traverse/skip directories during the enumeration.


    .NOTE
        The filter for traverse/skip only works on root level.
#>

    [OutputType([Bool])]
    Param(
        [Alphaleonis.Win32.Filesystem.FileSystemEntryInfo]$fsei
    )


    Process {
        # Example: Skip recursing into these folders.
        $Private:skipFolders = 'drivers', 'DriverStore', 'LogFiles'

        [Bool]$Private:gotMatch = $skipFolders -ccontains $fsei.FileName

        If ($gotMatch) {
            ++$Script:SkippedFolderCount

            Write-Warning -Message ('Skip folder: {0}' -f $fsei.FileName)
        }

        
        # When $True the directory is traversed.
        Return (-not $gotMatch)
    }
}


Function Enumerate-FileSystemEntryInfos {

<#
    .SYNOPSIS
        AlphaFS v2.2 EnumerateFileSystemEntryInfos

        A powerful folder/file enumerator which can report and recover from access denied exceptions and supports custom filtering.


    .EXAMPLE
        PS C:\> .\Enumerate-FileSystemEntryInfos.ps1 -Recurse -Path $ENV:windir


    .NOTES
        Backup privileges are enables when run elevated.
        This allows for browsing folders which are normally inaccessible.


    .OUTPUTS
        An [Alphaleonis.Win32.Filesystem.FileSystemEntryInfo] instance. For example:

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
#>

    [CmdletBinding()]
    Param(
        [String]$Path = '.',
        [String]$Filter = '*',
        [Switch]$Recurse,
        [Switch]$ShowProgress,
        [Switch]$ContinueOnException,
        [Switch]$Directory,
        [Switch]$File
    )


    Begin {
        $Error.Clear()


        # Demo counters for the DirectoryEnumerationFilters; Enumeration will abort when either condition is met.
        [Long]$Script:MaxErrorCount    = 10 #-1
        [Long]$Script:MaxFsoFoundCount = 10 #-1


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


        # Counters to keep track of stuff.
        [Long]$Script:FsoCount           = 0
        [Long]$Script:FoundFsoCount      = 0
        [Long]$Script:SkippedFolderCount = 0
        [Long]$Script:ErrorCount         = 0


        # Used to abort the enumeration.
        [System.Threading.CancellationTokenSource]$Script:cancelSource = [System.Threading.CancellationTokenSource]::new()
        $dirEnumFilters.CancellationToken = $cancelSource.Token


        # The callback error handler.
        $dirEnumFilters.ErrorFilter = $ErrorFilter
        
                
        # The callback file/folder handler.
        $dirEnumFilters.InclusionFilter = $InclusionFilter


        # The callback folder recursion handler.
        $dirEnumFilters.RecursionFilter = $RecursionFilter


        # Enables backup privileges when run elevated; This allows for browsing folders which are normally inaccessible.
        $Private:privilegeEnabler = New-Object Alphaleonis.Win32.Security.PrivilegeEnabler([Alphaleonis.Win32.Security.Privilege]::Backup)


        [String]$Private:startupTitle = $Host.UI.RawUI.WindowTitle

        [Bool]$ShowProgress = $ShowProgress.IsPresent
        If ($ShowProgress) { Write-Progress -Activity $Path -Status 'Processing items...' }


        $Private:stopwatch = [system.diagnostics.stopwatch]::StartNew()
    }


	Process {
        Try {
            Invoke-GenericMethod `
                -Instance           ([Alphaleonis.Win32.Filesystem.Directory]) `
                -MethodName         EnumerateFileSystemEntryInfos `
                -TypeParameters     Alphaleonis.Win32.Filesystem.FileSystemEntryInfo `
                -MethodParameters   $Path, $Filter, $dirEnumOptions, $dirEnumFilters, ([Alphaleonis.Win32.Filesystem.PathFormat]::RelativePath)
        }

        Finally {
            $Host.UI.RawUI.WindowTitle = $startupTitle

            # Place disposing here to ensure it is always executed.
            If ($Null -ne $privilegeEnabler) { $privilegeEnabler.Dispose() }
            If ($Null -ne $cancelSource)     { $cancelSource.Dispose()     }
        }
    }


    End {
        [String]$Private:foundFsoText = $(If ($Null -ne $dirEnumFilters.InclusionFilter) { (' Found: [{0:N0}] ' -f $FoundFsoCount) } Else { ' ' })

        [Console]::ForegroundColor = [ConsoleColor]::Green
        [Console]::WriteLine(('Duration: [{0}]  Processed: [{1:N0}] {2} Skipped Folders: [{3:N0}]  Errors: [{4:N0}]' -f [Timespan]::FromMilliseconds($stopwatch.Elapsed.TotalMilliseconds), $FsoCount, $foundFsoText, $SkippedFolderCount, $ErrorCount))
        [Console]::ResetColor()
    }
}


Import-Module -Name 'PATH TO AlphaFS.dll'

Enumerate-FileSystemEntryInfos -Path $Path -Filter $Filter -Recurse:$Recurse.IsPresent -ShowProgress:$ShowProgress.IsPresent -ContinueOnException:$ContinueOnException.IsPresent -Directory:$Directory.IsPresent -File:$File.IsPresent
