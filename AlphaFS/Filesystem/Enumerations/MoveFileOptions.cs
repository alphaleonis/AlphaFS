/* Copyright (c) 2008-2009 Peter Palotas
 *  
 *  Permission is hereby granted, free of charge, to any person obtaining a copy
 *  of this software and associated documentation files (the "Software"), to deal
 *  in the Software without restriction, including without limitation the rights
 *  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 *  copies of the Software, and to permit persons to whom the Software is
 *  furnished to do so, subject to the following conditions:
 *  
 *  The above copyright notice and this permission notice shall be included in
 *  all copies or substantial portions of the Software.
 *  
 *  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 *  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 *  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 *  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 *  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 *  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 *  THE SOFTWARE.
 */
using System;

namespace Alphaleonis.Win32.Filesystem
{
    /// <summary>
    /// The move options for a file move operation.
    /// </summary>
    [Flags]
    public enum MoveFileOptions : int
    {
        /// <summary>
        /// If the destination file name already exists, the function replaces its contents with the contents of the 
        /// source file.
        /// </summary>
        /// <remark>This value cannot be used if either source or destination names a directory.</remark>
        ReplaceExisting = 0x01,

        /// <summary>
        /// If the file is to be moved to a different volume, the function simulates the move by using the CopyFile and DeleteFile functions.
        /// </summary>
        /// <remarks>This value cannot be used with <see cref="MoveFileOptions.DelayUntilReboot"/>.</remarks>
        CopyAllowed = 0x02,

        /// <summary>
        /// <para>The system does not move the file until the operating system is restarted. The system moves the file immediately after AUTOCHK is executed, but before creating any paging files. Consequently, this parameter enables the function to delete paging files from previous startups.</para>
        /// <para>This value can only be used if the process is in the context of a user who belongs to the administrators group or the LocalSystem account.</para>
        /// </summary>
        /// <remarks>This value cannot be used with <see cref="MoveFileOptions.CopyAllowed"/>.</remarks>
        DelayUntilReboot = 0x04,

        /// <summary>
        /// <para>The function does not return until the file has actually been moved on the disk.</para>
        /// <para>Setting this value guarantees that a move performed as a copy and delete operation is flushed 
        /// to disk before the function returns. The flush occurs at the end of the copy operation.</para>
        /// </summary>
        /// <remarks>This value has no effect if <see cref="MoveFileOptions.DelayUntilReboot"/> is set.</remarks>
        WriteThrough = 0x08,

        /// <summary>
        /// Reserved for future use.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Hardlink")]
        CreateHardlink = 0x10,

        /// <summary>
        /// The function fails if the source file is a link source, but the file cannot be tracked after the move. 
        /// This situation can occur if the destination is a volume formatted with the FAT file system.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Trackable")]
        FailIfNotTrackable = 0x20
    }
}
