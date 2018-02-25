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


Function Copy-DirectoryWithProgress {

<#
    .SYNOPSIS
        Copies a directory and its contents to a new location, CopyOptions can be specified,
        and the possibility of notifying the application of its progress through a callback function.
#>

    [OutputType([Alphaleonis.Win32.Filesystem.CopyMoveResult])]
    Param(
        [String]$SourceFolderFullPath,
        [String]$DestinationFolderFullPath,
        [Alphaleonis.Win32.Filesystem.CopyOptions]$CopyOptions,
        [Alphaleonis.Win32.Filesystem.CopyMoveProgressRoutine]$Callback
    )


    Process {
        [Alphaleonis.Win32.Filesystem.Directory]::Copy($SourceFolderFullPath, $DestinationFolderFullPath, $CopyOptions, $Callback, $Null)
    }
}


Function UnitSizeToText {

<#
    .SYNOPSIS
        Converts a number to a string, suffixed with a unit size.
#>

    [OutputType([String])]
    Param(
        [Long]$numberOfBytes
    )


    [Array]$Private:sizeFormats = "B", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB"
    [Int]$Private:index = 0

    while ($numberOfBytes -gt 1kb)
    {
        $numberOfBytes /= 1kb
        $index++
    }

    "{0:N0} {1}" -f $numberOfBytes, $sizeFormats[$index]
}




[ScriptBlock]$FolderCopyProgressHandler = {

<#
    .SYNOPSIS
        The folder copy progress handler.
#>

    [OutputType([Alphaleonis.Win32.Filesystem.CopyMoveProgressResult])]
    Param(

        # The total size of the file, in bytes.
        [Long]$TotalFileSize,

        # The total number of bytes transferred from the source file to the destination file since the copy operation began.
        [Long]$TotalBytesTransferred,

        # The total size of the current file stream, in bytes.
        [Long]$StreamSize,

        # The total number of bytes in the current stream that have been transferred from the source file to the destination file since the copy operation began.
        [Long]$StreamBytesTransferred,

        # A handle to the current stream. The first time CopyProgressRoutine is called, the stream number is 1.
        [Int]$StreamNumber,

        # The reason that CopyProgressRoutine was called.
        [Int]$CallbackReason,

        [Object]$userData
    )


    Process {
        # Another part of the data file was copied.
        #[Int]$Private:CALLBACK_CHUNK_FINISHED = 0

        # Another stream was created and is about to be copied. This is the callback reason given when the callback routine is first invoked.
        [Int]$Private:CALLBACK_STREAM_SWITCH = 1


        # Add size of all copied files.
        If ($CallbackReason -eq $CALLBACK_STREAM_SWITCH) { $Script:AllBytesTransferred += $TotalFileSize }

        [Double]$Private:percent =  ($TotalBytesTransferred / $TotalFileSize * 100)

        [String]$Private:activity = ('Copying folder. Copied: {0}' -f (UnitSizeToText($AllBytesTransferred)))

        # Write-Progress can slow things down CONSIDERABLY; Disable when there is no need for progress report.
        Write-Progress -Activity $activity -Status ('{0:N2}%' -f $percent) -PercentComplete $percent


        # Possible Write-Progress alternative.
        #$Host.UI.RawUI.WindowTitle = $activity


        return [Alphaleonis.Win32.Filesystem.CopyMoveProgressResult]::Continue
    }
}


Function DemoCopy-DirectoryWithProgress {

<#
    2018-02-25 Tested on:

    Name                           Value
    ----                           -----
    PSVersion                      5.1.14409.1005
    PSEdition                      Desktop
    PSCompatibleVersions           {1.0, 2.0, 3.0, 4.0...}
    BuildVersion                   10.0.14409.1005
    CLRVersion                     4.0.30319.42000
    WSManStackVersion              3.0
    PSRemotingProtocolVersion      2.3
    SerializationVersion           1.1.0.1
#>

    Begin {
        Clear-Host
        $Error.Clear()


        # Specify an existing folder to copy.

        [String]$Private:srcFolderFullPath = 'C:\A Big Folder'
        [String]$Private:dstFolderFullPath = "$Env:Temp\Copy-DirectoryWithProgress-Test"


        # Demo code: Used for assertion.
        [Long]$Script:AllBytesTransferred = 0
        [Bool]$Private:failed = $True


        # Allow copy to overwrite an existing file.
        $Private:copyOptions = [Alphaleonis.Win32.Filesystem.CopyOptions]::None
    }


    Process {
        $Private:cmr = Copy-DirectoryWithProgress `
            -SourceFolderFullPath      $srcFolderFullPath `
            -DestinationFolderFullPath $dstFolderFullPath `
            -CopyOptions               $copyOptions `
            -Callback                  $FolderCopyProgressHandler
    }


    End {
        # Assert no $Error's.
        $failed = $Error.Count -gt 0
        If ($failed) { Write-Warning -Message 'Errors encountered.' }

        # Assert not $Null.
        $failed = $Null -eq $cmr
        If ($failed) { Write-Warning -Message 'Result is null.' }

        Else {
            # Assert bytes transferred.
            $failed = $Script:AllBytesTransferred -ne $cmr.TotalBytes
            If ($failed) { Write-Warning -Message ('The number of bytes copied does not match: {0:N0} vs {1:N0}' -f $Script:AllBytesTransferred, $cmr.TotalBytes) }

            Else {

                If ($cmr.ErrorCode -eq 0) { Write-Host -ForegroundColor Green ('Folder copied successfully. Message: {0}' -f $cmr.ErrorMessage) -NoNewline }
                Else                      { Write-Host -ForegroundColor Red   ('Folder copied with errors. Message: {0}'  -f $cmr.ErrorMessage) -NoNewline }
            }


            $cmr
        }
    }
}


Import-Module -Name 'PATH TO\AlphaFS.dll'

DemoCopy-DirectoryWithProgress
