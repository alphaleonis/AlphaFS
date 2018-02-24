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


Function Copy-FileWithProgress {

<#
    .SYNOPSIS
        Copies an existing file to a new file. Overwriting a file of the same name is allowed.
        CopyOptions can be specified, and the possibility of notifying the application of its progress through a callback function.
#>

    [OutputType([Alphaleonis.Win32.Filesystem.CopyMoveResult])]
    Param(
        [String]$SourceFileFullPath,
        [String]$DestinationFileFullPath,
        [Alphaleonis.Win32.Filesystem.CopyOptions]$CopyOptions,
        [Alphaleonis.Win32.Filesystem.CopyMoveProgressRoutine]$Callback
    )
    

    Process {
        [Alphaleonis.Win32.Filesystem.File]::Copy($SourceFileFullPath, $DestinationFileFullPath, $CopyOptions, $Callback, $Null)
    }
}


Function UnitSizeToText {

<#
    .SYNOPSIS
        Converts a number to a string formated, suffixed with a unit size.
#>

    [OutputType([String])]
    Param(
        [Long]$numberOfBytes
    )

    
    [Array]$Private:sizeFormats = "B", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB"
    [Int]$Private:index = 0

    while ($numberOfBytes -gt 1kb) 
    {
        $numberOfBytes = $numberOfBytes / 1kb
        $index++
    } 
    
    "{0:N0} {1}" -f $numberOfBytes, $sizeFormats[$index]
}




[ScriptBlock]$CopyMoveCallback = {

<#
    .SYNOPSIS
        Handler to process the file copying progress.
#>

    [OutputType([Int])]
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
        # Demo code: Used for assertion.
        $Script:AllBytesTransferred = $TotalBytesTransferred

        [String]$Private:copiedUnit   = UnitSizeToText($TotalBytesTransferred)
        [String]$Private:fileSizeUnit = UnitSizeToText($TotalFileSize)

        [String]$Private:activity1 = ('Copying file. Copied: {0} of {1}'  -f $copiedUnit, $fileSizeUnit)
        #[String]$Private:activity2 = ('Copying file. Copied: {0:N0} of {1:N0} bytes' -f $TotalBytesTransferred, $TotalFileSize)


        # Write-Progress WILL slow things down CONSIDERABLY; Disable when there is no need for progress report.
        # So, then what is the point of... Exactly.

        Write-Progress -Id 0 -Activity $activity1 -PercentComplete ($TotalBytesTransferred / $TotalFileSize * 100)

        
        # Possible Write-Progress alternative.
        #$Host.UI.RawUI.WindowTitle = $activity1
    }
}


Function DemoCopy-FileWithProgress {

    # Specify a file to copy. The destination directory should exist.

    [String]$Private:file = 'my_file.iso'
    [String]$Private:srcFileFullPath = [System.IO.Path]::Combine('C:\SourceDirectory', $file)
    [String]$Private:dstFileFullPath = [System.IO.Path]::Combine('\\server\share\DestinationDirectory', $file)

    


    # Demo code: Used for assertion.
    $Error.Clear()
    [Long]$Script:AllBytesTransferred = 0
    [Bool]$Private:failed = $True


    # Allow copy to overwrite an existing file.
    $Private:copyOptions = [Alphaleonis.Win32.Filesystem.CopyOptions]::None


    $Private:cmr = Copy-FileWithProgress -SourceFileFullPath      $srcFileFullPath `
                                         -DestinationFileFullPath $dstFileFullPath `
                                         -CopyOptions             $copyOptions `
                                         -Callback                $CopyMoveCallback


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
            If ($cmr.ErrorCode -eq 0) { Write-Host -ForegroundColor Green 'File copied successfully.' -NoNewline }

            $cmr
        }
    }
}


Import-Module -Name 'PATH TO\AlphaFS.dll'

DemoCopy-FileWithProgress
