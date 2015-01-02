/* Copyright (c) 2008-2014 Peter Palotas, Jeffrey Jangli, Normalex
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

namespace Alphaleonis.Win32.Filesystem
{
   internal static partial class NativeMethods
   {
      /// <summary>FILE_INFO_BY_HANDLE_CLASS
      /// <para>Identifies the type of file information that <see cref="GetFileInformationByHandleEx"/> should retrieve or SetFileInformationByHandle should set.</para>
      /// </summary>
      internal enum FileInfoByHandleClass
      {
         #region FILE_BASIC_INFO

         /// <summary>(0) FILE_BASIC_INFO - Minimal information for the file should be retrieved or set.
         /// <para>&#160;</para>
         /// <para>Used for file handles.</para>
         /// </summary>
         FileBasicInfo = 0,

         #endregion // FILE_BASIC_INFO

         #region FILE_STANDARD_INFO

         ///// <summary>(1) FILE_STANDARD_INFO - Extended information for the file should be retrieved.
         ///// <para>&#160;</para>
         ///// <para>Used for file handles.</para>
         ///// <para>Use only when calling <see cref="GetFileInformationByHandleEx"/>.</para>
         ///// </summary>
         //FileStandardInfo = 1,

         #endregion // FILE_STANDARD_INFO

         #region FILE_NAME_INFO

         ///// <summary>(2) FILE_NAME_INFO - The file name should be retrieved.
         ///// <para>&#160;</para>
         ///// <para>Used for any handles.</para>
         ///// <para>Use only when calling <see cref="GetFileInformationByHandleEx"/>.</para>
         ///// </summary>
         //FileNameInfo = 2,

         #endregion // FILE_NAME_INFO

         #region FILE_RENAME_INFO

         ///// <summary>(3) FILE_RENAME_INFO - The file name should be changed.
         ///// <para>&#160;</para>
         ///// <para>Used for file handles.</para>
         ///// <para>Use only when calling <see cref="SetFileInformationByHandle"/>.</para>
         ///// </summary>
         //FileRenameInfo = 3,

         #endregion // FILE_RENAME_INFO

         #region FILE_DISPOSITION_INFO

         ///// <summary>(4) FILE_DISPOSITION_INFO - The file should be deleted.
         ///// <para>&#160;</para>
         ///// <para>Used for any handles.</para>
         ///// <para>Use only when calling <see cref="SetFileInformationByHandle"/>.</para>
         ///// </summary>
         //FileDispositionInfo = 4,

         #endregion // FILE_DISPOSITION_INFO

         #region FILE_ALLOCATION_INFO

         ///// <summary>(5) FILE_ALLOCATION_INFO - The file allocation information should be changed.
         ///// <para>&#160;</para>
         ///// <para>Used for file handles.</para>
         ///// <para>Use only when calling <see cref="SetFileInformationByHandle"/>.</para>
         ///// </summary>
         //FileAllocationInfo = 5,

         #endregion // FILE_ALLOCATION_INFO

         #region FILE_END_OF_FILE_INFO

         ///// <summary>(6) FILE_END_OF_FILE_INFO - The end of the file should be set.
         ///// <para>&#160;</para>
         ///// <para>Use only when calling <see cref="SetFileInformationByHandle"/>.</para>
         ///// </summary>
         //FileEndOfFileInfo = 6,

         #endregion // FILE_END_OF_FILE_INFO

         #region FILE_STREAM_INFO

         ///// <summary>(7) FILE_STREAM_INFO - File stream information for the specified file should be retrieved.
         ///// <para>&#160;</para>
         ///// <para>Used for any handles.</para>
         ///// <para>Use only when calling <see cref="GetFileInformationByHandleEx"/>.</para>
         ///// </summary>
         //FileStreamInfo = 7,

         #endregion // FILE_STREAM_INFO

         #region FILE_COMPRESSION_INFO

         ///// <summary>(8) FILE_COMPRESSION_INFO - File compression information should be retrieved.
         ///// <para>&#160;</para>
         ///// <para>Used for any handles.</para>
         ///// <para>Use only when calling <see cref="GetFileInformationByHandleEx"/>.</para>
         ///// </summary>
         //FileCompressionInfo = 8,

         #endregion // FILE_COMPRESSION_INFO

         #region FILE_ATTRIBUTE_TAG_INFO

         ///// <summary>(9) FILE_ATTRIBUTE_TAG_INFO - File attribute information should be retrieved.
         ///// <para>&#160;</para>
         ///// <para>Used for any handles.</para>
         ///// <para>Use only when calling <see cref="GetFileInformationByHandleEx"/>.</para>
         ///// </summary>
         //FileAttributeTagInfo = 9,

         #endregion // FILE_ATTRIBUTE_TAG_INFO

         #region FILE_ID_BOTH_DIR_INFO

         /// <summary>(10) FILE_ID_BOTH_DIR_INFO - Files in the specified directory should be retrieved.
         /// <para>&#160;</para>
         /// <para>Used for directory handles.</para>
         /// <para>Use only when calling <see cref="GetFileInformationByHandleEx"/>.</para>
         /// <para>&#160;</para>
         /// <remarks>
         /// <para>The number of files returned for each call to <see cref="GetFileInformationByHandleEx"/></para>
         /// <para>depends on the size of the buffer that is passed to the function.</para>
         /// <para>Any subsequent calls to <see cref="GetFileInformationByHandleEx"/> on the same handle</para>
         /// <para>will resume the enumeration operation after the last file is returned.</para>
         /// </remarks>
         /// </summary>
         FileIdBothDirectoryInfo = 10,

         #endregion // FILE_ID_BOTH_DIR_INFO

         #region FILE_ID_BOTH_DIR_INFO

         ///// <summary>(11) FILE_ID_BOTH_DIR_INFO - Identical to <see cref="FileIdBothDirectoryInfo"/>, but forces the enumeration operation to start again from the beginning.
         ///// </summary>
         //FileIdBothDirectoryInfoRestartInfo = 11,

         #endregion // FILE_ID_BOTH_DIR_INFO

         #region FILE_IO_PRIORITY_HINT_INFO

         ///// <summary>(12) FILE_IO_PRIORITY_HINT_INFO - Priority hint information should be set.
         ///// <para>&#160;</para>
         ///// <para>Use only when calling <see cref="SetFileInformationByHandle"/>.</para>
         ///// </summary>
         //FileIoPriorityHintInfo = 12,

         #endregion // FILE_IO_PRIORITY_HINT_INFO

         #region FILE_REMOTE_PROTOCOL_INFO

         ///// <summary>(13) FILE_REMOTE_PROTOCOL_INFO - File remote protocol information should be retrieved.
         ///// <para>&#160;</para>
         ///// <para>Use for any handles.</para>
         ///// <para>Use only when calling <see cref="GetFileInformationByHandleEx"/>.</para>
         ///// </summary>
         //FileRemoteProtocolInfo = 13,

         #endregion // FILE_REMOTE_PROTOCOL_INFO

         #region FILE_FULL_DIR_INFO

         ///// <summary>(14) FILE_FULL_DIR_INFO - Files in the specified directory should be retrieved.
         ///// <para>&#160;</para>
         ///// <para>Used for directory handles.</para>
         ///// <para>Use only when calling <see cref="GetFileInformationByHandleEx"/>.</para>
         ///// <para>&#160;</para>
         ///// <remarks>
         ///// <para>Windows Server 2008 R2, Windows 7, Windows Server 2008, Windows Vista, Windows Server 2003, and Windows XP:</para>
         ///// <para>This value is not supported before Windows 8 and Windows Server 2012</para>
         ///// </remarks>
         ///// </summary>
         //FileFullDirectoryInfo = 14,

         #endregion // FILE_FULL_DIR_INFO

         #region FILE_FULL_DIR_INFO

         ///// <summary>(15) FILE_FULL_DIR_INFO - Identical to <see cref="FileFullDirectoryInfo"/>, but forces the enumeration operation to start again from the beginning.
         ///// <para>&#160;</para>
         ///// <para>Use only when calling <see cref="GetFileInformationByHandleEx"/>.</para>
         ///// <para>&#160;</para>
         ///// <remarks>
         ///// <para>Windows Server 2008 R2, Windows 7, Windows Server 2008, Windows Vista, Windows Server 2003, and Windows XP:</para>
         ///// <para>This value is not supported before Windows 8 and Windows Server 2012</para>
         ///// </remarks>
         ///// </summary>
         //FileFullDirectoryRestartInfo = 15,

         #endregion // FILE_FULL_DIR_INFO

         #region FILE_STORAGE_INFO

         ///// <summary>(16) FILE_STORAGE_INFO - File storage information should be retrieved.
         ///// <para>&#160;</para>
         ///// <para>Use for any handles.</para>
         ///// <para>Use only when calling <see cref="GetFileInformationByHandleEx"/>.</para>
         ///// <para>&#160;</para>
         ///// <remarks>
         ///// <para>Windows Server 2008 R2, Windows 7, Windows Server 2008, Windows Vista, Windows Server 2003, and Windows XP:</para>
         ///// <para>This value is not supported before Windows 8 and Windows Server 2012</para>
         ///// </remarks>
         ///// </summary>
         //FileStorageInfo = 16,

         #endregion // FILE_STORAGE_INFO

         #region FILE_ALIGNMENT_INFO

         ///// <summary>(17) FILE_ALIGNMENT_INFO - File alignment information should be retrieved.
         ///// <para>&#160;</para>
         ///// <para>Use for any handles.</para>
         ///// <para>Use only when calling <see cref="GetFileInformationByHandleEx"/>.</para>
         ///// <para>&#160;</para>
         ///// <remarks>
         ///// <para>Windows Server 2008 R2, Windows 7, Windows Server 2008, Windows Vista, Windows Server 2003, and Windows XP:</para>
         ///// <para>This value is not supported before Windows 8 and Windows Server 2012</para>
         ///// </remarks>
         ///// </summary>
         //FileAlignmentInfo = 17,

         #endregion FILE_ALIGNMENT_INFO

         #region FILE_ID_INFO

         ///// <summary>(18) FILE_ID_INFO - File information should be retrieved.
         ///// <para>&#160;</para>
         ///// <para>Use for any handles.</para>
         ///// <para>Use only when calling <see cref="GetFileInformationByHandleEx"/>.</para>
         ///// <para>&#160;</para>
         ///// <remarks>
         ///// <para>Windows Server 2008 R2, Windows 7, Windows Server 2008, Windows Vista, Windows Server 2003, and Windows XP:</para>
         ///// <para>This value is not supported before Windows 8 and Windows Server 2012</para>
         ///// </remarks>
         ///// </summary>
         //FileIdInfo = 18,

         #endregion // FILE_ID_INFO

         #region FILE_ID_EXTD_DIR_INFO

         ///// <summary>(19) FILE_ID_EXTD_DIR_INFO - Files in the specified directory should be retrieved.
         ///// <para>&#160;</para>
         ///// <para>Used for directory handles.</para>
         ///// <para>Use only when calling <see cref="GetFileInformationByHandleEx"/>.</para>
         ///// <para>&#160;</para>
         ///// <remarks>
         ///// <para>Windows Server 2008 R2, Windows 7, Windows Server 2008, Windows Vista, Windows Server 2003, and Windows XP:</para>
         ///// <para>This value is not supported before Windows 8 and Windows Server 2012</para>
         ///// </remarks>
         ///// </summary>
         //FileIdExtdDirectoryInfo = 19,

         #endregion // FILE_ID_EXTD_DIR_INFO

         #region FILE_ID_EXTD_DIR_INFO

         ///// <summary>(20) FILE_ID_EXTD_DIR_INFO - Identical to <see cref="FileIdExtdDirectoryInfo"/>, but forces the enumeration operation to start again from the beginning.
         ///// <para>&#160;</para>
         ///// <para>Use only when calling <see cref="GetFileInformationByHandleEx"/>.</para>
         ///// <para>&#160;</para>
         ///// <remarks>
         ///// <para>Windows Server 2008 R2, Windows 7, Windows Server 2008, Windows Vista, Windows Server 2003, and Windows XP:</para>
         ///// <para>This value is not supported before Windows 8 and Windows Server 2012</para>
         ///// </remarks>
         ///// </summary>
         //FileIdExtdDirectoryRestartInfo = 20

         #endregion // FILE_ID_EXTD_DIR_INFO
      }
   }
}