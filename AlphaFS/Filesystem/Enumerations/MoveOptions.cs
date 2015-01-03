/* Copyright (c) 2008-2015 Peter Palotas, Jeffrey Jangli, Normalex
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
using System.Diagnostics.CodeAnalysis;

namespace Alphaleonis.Win32.Filesystem
{
   /// <summary>Used by MoveFileXxx.
   /// <para>Flags that specify how a file or directory is to be moved.</para>
   /// </summary>
   [Flags]
   public enum MoveOptions
   {
      /// <summary>(0) No MoveOptions used, this fails when the file name already exists.</summary>
      None = 0,

      /// <summary>(1) MOVE_FILE_REPLACE_EXISTSING - If the destination file name already exists,
      /// <para>the function replaces its contents with the contents of the source file.</para>
      /// <para>This value cannot be used if lpNewFileName or lpExistingFileName names a directory.</para>
      /// </summary>
      /// <remark>This value cannot be used if either source or destination names a directory.</remark>
      ReplaceExisting = 1,

      /// <summary>(2) MOVE_FILE_COPY_ALLOWED - If the file is to be moved to a different volume,
      /// <para>the function simulates the move by using the CopyFile and DeleteFile functions.</para>
      /// </summary>
      /// <remarks>This value cannot be used with <see cref="MoveOptions.DelayUntilReboot"/>.</remarks>
      CopyAllowed = 2,

      /// <summary>(4) MOVE_FILE_DELAY_UNTIL_REBOOT - The system does not move the file until the operating system is restarted.
      /// <para> The system moves the file immediately after AUTOCHK is executed, but before creating any paging files.</para>
      /// <para>Consequently, this parameter enables the function to delete paging files from previous startups.</para>
      /// <para>This value can only be used if the process is in the context of a user who belongs to the administrators group or the LocalSystem account.</para>
      /// </summary>
      /// <remarks>This value cannot be used with <see cref="MoveOptions.CopyAllowed"/>.</remarks>
      DelayUntilReboot = 4,

      /// <summary>(8) MOVE_FILE_WRITE_THROUGH - The function does not return until the file has actually been moved on the disk.
      /// <para>Setting this value guarantees that a move performed as a copy and delete operation is flushed </para>
      /// <para>to disk before the function returns. The flush occurs at the end of the copy operation.</para>
      /// </summary>
      /// <remarks>This value has no effect if <see cref="MoveOptions.DelayUntilReboot"/> is set.</remarks>
      WriteThrough = 8,

      /// <summary>(16) MOVE_FILE_CREATE_HARDLINK - Reserved for future use.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Hardlink")]
      CreateHardlink = 16,

      /// <summary>(32) MOVE_FILE_FAIL_IF_NOT_TRACKABLE - The function fails if the source file is a link source, but the file cannot be tracked after the move.
      /// <para>This situation can occur if the destination is a volume formatted with the FAT file system.</para>
      /// </summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Trackable")]
      FailIfNotTrackable = 32
   }
}