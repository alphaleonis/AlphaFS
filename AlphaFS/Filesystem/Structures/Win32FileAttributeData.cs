/*  Copyright (C) 2008-2015 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
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

using System.IO;
using System.Runtime.InteropServices;

namespace Alphaleonis.Win32.Filesystem
{
   internal static partial class NativeMethods
   {
      /// <summary>WIN32_FILE_ATTRIBUTE_DATA structure contains attribute information for a file or directory. The GetFileAttributesEx function uses this structure.</summary>
      /// <remarks>
      /// Not all file systems can record creation and last access time, and not all file systems record them in the same manner.
      /// For example, on the FAT file system, create time has a resolution of 10 milliseconds, write time has a resolution of 2 seconds,
      /// and access time has a resolution of 1 day. On the NTFS file system, access time has a resolution of 1 hour. 
      /// For more information, see File Times.
      /// </remarks>
      [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
      internal struct Win32FileAttributeData
      {
         public Win32FileAttributeData(Win32FindData findData)
         {
            FileAttributes = findData.FileAttributes;
            CreationTime = findData.CreationTime;
            LastAccessTime = findData.LastAccessTime;
            LastWriteTime = findData.LastWriteTime;
            FileSizeHigh = findData.FileSizeHigh;
            FileSizeLow = findData.FileSizeLow;
         }
         
         #region FileAttributes

         /// <summary>The file attributes of a file.</summary>
         [MarshalAs(UnmanagedType.I4)]
         public FileAttributes FileAttributes;

         #endregion // FileAttributes

         #region CreationTime

         /// <summary>A <see cref="NativeMethods.FileTime"/> structure that specifies when a file or directory was created.
         /// If the underlying file system does not support creation time, this member is zero.</summary>
         public readonly FileTime CreationTime;

         #endregion // CreationTime

         #region LastAccessTime

         /// <summary>A <see cref="NativeMethods.FileTime"/> structure.
         /// For a file, the structure specifies when the file was last read from, written to, or for executable files, run.
         /// For a directory, the structure specifies when the directory is created. If the underlying file system does not support last access time, this member is zero.
         /// On the FAT file system, the specified date for both files and directories is correct, but the time of day is always set to midnight.
         /// </summary>
         public readonly FileTime LastAccessTime;

         #endregion // LastAccessTime

         #region LastWriteTime

         /// <summary>A <see cref="NativeMethods.FileTime"/> structure.
         /// For a file, the structure specifies when the file was last written to, truncated, or overwritten, for example, when WriteFile or SetEndOfFile are used.
         /// The date and time are not updated when file attributes or security descriptors are changed.
         /// For a directory, the structure specifies when the directory is created. If the underlying file system does not support last write time, this member is zero.
         /// </summary>
         public readonly FileTime LastWriteTime;

         #endregion // LastWriteTime

         #region FileSizeHigh

         /// <summary>The high-order DWORD of the file size. This member does not have a meaning for directories.
         /// This value is zero unless the file size is greater than MAXDWORD.
         /// The size of the file is equal to (nFileSizeHigh * (MAXDWORD+1)) + nFileSizeLow.
         /// </summary>
         public readonly uint FileSizeHigh;

         #endregion // FileSizeHigh

         #region FileSizeLow

         /// <summary>The low-order DWORD of the file size. This member does not have a meaning for directories.</summary>
         public readonly uint FileSizeLow;

         #endregion // FileSizeLow

         #region FileSize

         /// <summary>The file size.</summary>
         public long FileSize
         {
            get { return ToLong(FileSizeHigh, FileSizeLow); }
         }

         #endregion // FileSize
      }
   }
}