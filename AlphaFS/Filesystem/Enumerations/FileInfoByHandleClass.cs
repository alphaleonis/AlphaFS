/* Copyright (C) 2008-2016 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
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
      /// <para>Identifies the type of file information that GetFileInformationByHandleEx should retrieve or SetFileInformationByHandle should set.</para>
      /// </summary>
      internal enum FileInfoByHandleClass
      {
         #region FILE_BASIC_INFO

         /// <summary>FILE_BASIC_INFO
         /// <para>Minimal information for the file should be retrieved or set. Used for file handles.</para>
         /// </summary>
         FileBasicInfo = 0,

         #endregion // FILE_BASIC_INFO

         #region FILE_STANDARD_INFO

         ///// <summary>FILE_STANDARD_INFO
         ///// <para>Extended information for the file should be retrieved. Used for file handles.</para>
         ///// <para>Use only when calling GetFileInformationByHandleEx.</para>
         ///// </summary>
         //FileStandardInfo = 1,

         #endregion // FILE_STANDARD_INFO

         #region FILE_NAME_INFO

         ///// <summary>FILE_NAME_INFO
         ///// <para>The file name should be retrieved. Used for any handles.</para>
         ///// <para>Use only when calling GetFileInformationByHandleEx.</para>
         ///// </summary>
         //FileNameInfo = 2,

         #endregion // FILE_NAME_INFO

         #region FILE_RENAME_INFO

         ///// <summary>FILE_RENAME_INFO
         ///// <para>The file name should be changed. Used for file handles.</para>
         ///// <para>Use only when calling <see cref="SetFileInformationByHandle"/>.</para>
         ///// </summary>
         //FileRenameInfo = 3,

         #endregion // FILE_RENAME_INFO

         #region FILE_DISPOSITION_INFO

         ///// <summary>FILE_DISPOSITION_INFO
         ///// <para>The file should be deleted. Used for any handles.</para>
         ///// <para>Use only when calling <see cref="SetFileInformationByHandle"/>.</para>
         ///// </summary>
         //FileDispositionInfo = 4,

         #endregion // FILE_DISPOSITION_INFO

         #region FILE_ALLOCATION_INFO

         ///// <summary>FILE_ALLOCATION_INFO
         ///// <para>The file allocation information should be changed. Used for file handles.</para>
         ///// <para>Use only when calling <see cref="SetFileInformationByHandle"/>.</para>
         ///// </summary>
         //FileAllocationInfo = 5,

         #endregion // FILE_ALLOCATION_INFO

         #region FILE_END_OF_FILE_INFO

         ///// <summary>FILE_END_OF_FILE_INFO
         ///// <para>The end of the file should be set. Use only when calling <see cref="SetFileInformationByHandle"/>.</para>
         ///// </summary>
         //FileEndOfFileInfo = 6,

         #endregion // FILE_END_OF_FILE_INFO

         #region FILE_STREAM_INFO

         ///// <summary>FILE_STREAM_INFO
         ///// <para>File stream information for the specified file should be retrieved. Used for any handles.</para>
         ///// <para>Use only when calling GetFileInformationByHandleEx.</para>
         ///// </summary>
         //FileStreamInfo = 7,

         #endregion // FILE_STREAM_INFO

         #region FILE_COMPRESSION_INFO

         ///// <summary>FILE_COMPRESSION_INFO
         ///// <para>File compression information should be retrieved. Used for any handles.</para>
         ///// <para>Use only when calling GetFileInformationByHandleEx.</para>
         ///// </summary>
         //FileCompressionInfo = 8,

         #endregion // FILE_COMPRESSION_INFO

         #region FILE_ATTRIBUTE_TAG_INFO

         ///// <summary>FILE_ATTRIBUTE_TAG_INFO
         ///// <para>File attribute information should be retrieved. Used for any handles.</para>
         ///// <para>Use only when calling GetFileInformationByHandleEx.</para>
         ///// </summary>
         //FileAttributeTagInfo = 9,

         #endregion // FILE_ATTRIBUTE_TAG_INFO

         #region FILE_ID_BOTH_DIR_INFO

         /// <summary>FILE_ID_BOTH_DIR_INFO
         /// <para>Files in the specified directory should be retrieved. Used for directory handles.</para>
         /// <para>Use only when calling GetFileInformationByHandleEx.</para>
         /// <remarks>
         /// <para>The number of files returned for each call to GetFileInformationByHandleEx</para>
         /// <para>depends on the size of the buffer that is passed to the function.</para>
         /// <para>Any subsequent calls to GetFileInformationByHandleEx on the same handle</para>
         /// <para>will resume the enumeration operation after the last file is returned.</para>
         /// </remarks>
         /// </summary>
         FileIdBothDirectoryInfo = 10

         #endregion // FILE_ID_BOTH_DIR_INFO

         #region FILE_ID_BOTH_DIR_INFO

         ///// <summary>FILE_ID_BOTH_DIR_INFO
         ///// <para>Identical to <see cref="FileIdBothDirectoryInfo"/>, but forces the enumeration operation to start again from the beginning.</para>
         ///// </summary>
         //FileIdBothDirectoryInfoRestartInfo = 11,

         #endregion // FILE_ID_BOTH_DIR_INFO

         #region FILE_IO_PRIORITY_HINT_INFO

         ///// <summary>FILE_IO_PRIORITY_HINT_INFO
         ///// <para>Priority hint information should be set.Use only when calling <see cref="SetFileInformationByHandle"/>.</para>
         ///// </summary>
         //FileIoPriorityHintInfo = 12,

         #endregion // FILE_IO_PRIORITY_HINT_INFO

         #region FILE_REMOTE_PROTOCOL_INFO

         ///// <summary>(13) FILE_REMOTE_PROTOCOL_INFO
         ///// <para>File remote protocol information should be retrieved.Use for any handles.</para>
         ///// <para>Use only when calling GetFileInformationByHandleEx.</para>
         ///// </summary>
         //FileRemoteProtocolInfo = 13,

         #endregion // FILE_REMOTE_PROTOCOL_INFO

         #region FILE_FULL_DIR_INFO

         ///// <summary>(14) FILE_FULL_DIR_INFO
         ///// <para>Files in the specified directory should be retrieved. Used for directory handles.</para>
         ///// <para>Use only when calling GetFileInformationByHandleEx.</para>
         ///// <remarks>
         ///// <para>Windows Server 2008 R2, Windows 7, Windows Server 2008, Windows Vista, Windows Server 2003, and Windows XP:</para>
         ///// <para>This value is not supported before Windows 8 and Windows Server 2012</para>
         ///// </remarks>
         ///// </summary>
         //FileFullDirectoryInfo = 14,

         #endregion // FILE_FULL_DIR_INFO

         #region FILE_FULL_DIR_INFO

         ///// <summary>FILE_FULL_DIR_INFO
         ///// <para>Identical to <see cref="FileFullDirectoryInfo"/>, but forces the enumeration operation to start again from the beginning. Use only when calling GetFileInformationByHandleEx.</para>
         ///// <remarks>
         ///// <para>Windows Server 2008 R2, Windows 7, Windows Server 2008, Windows Vista, Windows Server 2003, and Windows XP:</para>
         ///// <para>This value is not supported before Windows 8 and Windows Server 2012</para>
         ///// </remarks>
         ///// </summary>
         //FileFullDirectoryRestartInfo = 15,

         #endregion // FILE_FULL_DIR_INFO

         #region FILE_STORAGE_INFO

         ///// <summary>FILE_STORAGE_INFO
         ///// <para>File storage information should be retrieved. Use for any handles.</para>
         ///// <para>Use only when calling GetFileInformationByHandleEx.</para>
         ///// <remarks>
         ///// <para>Windows Server 2008 R2, Windows 7, Windows Server 2008, Windows Vista, Windows Server 2003, and Windows XP:</para>
         ///// <para>This value is not supported before Windows 8 and Windows Server 2012</para>
         ///// </remarks>
         ///// </summary>
         //FileStorageInfo = 16,

         #endregion // FILE_STORAGE_INFO

         #region FILE_ALIGNMENT_INFO

         ///// <summary>FILE_ALIGNMENT_INFO
         ///// <para>File alignment information should be retrieved. Use for any handles.</para>
         ///// <para>Use only when calling GetFileInformationByHandleEx.</para>
         ///// <remarks>
         ///// <para>Windows Server 2008 R2, Windows 7, Windows Server 2008, Windows Vista, Windows Server 2003, and Windows XP:</para>
         ///// <para>This value is not supported before Windows 8 and Windows Server 2012</para>
         ///// </remarks>
         ///// </summary>
         //FileAlignmentInfo = 17,

         #endregion FILE_ALIGNMENT_INFO

         #region FILE_ID_INFO

         ///// <summary>FILE_ID_INFO
         ///// <para>File information should be retrieved. Use for any handles.</para>
         ///// <para>Use only when calling GetFileInformationByHandleEx.</para>
         ///// <remarks>
         ///// <para>Windows Server 2008 R2, Windows 7, Windows Server 2008, Windows Vista, Windows Server 2003, and Windows XP:</para>
         ///// <para>This value is not supported before Windows 8 and Windows Server 2012</para>
         ///// </remarks>
         ///// </summary>
         //FileIdInfo = 18,

         #endregion // FILE_ID_INFO

         #region FILE_ID_EXTD_DIR_INFO

         ///// <summary>FILE_ID_EXTD_DIR_INFO
         ///// <para>Files in the specified directory should be retrieved. Used for directory handles.</para>
         ///// <para>Use only when calling GetFileInformationByHandleEx.</para>
         ///// <remarks>
         ///// <para>Windows Server 2008 R2, Windows 7, Windows Server 2008, Windows Vista, Windows Server 2003, and Windows XP:</para>
         ///// <para>This value is not supported before Windows 8 and Windows Server 2012</para>
         ///// </remarks>
         ///// </summary>
         //FileIdExtdDirectoryInfo = 19,

         #endregion // FILE_ID_EXTD_DIR_INFO

         #region FILE_ID_EXTD_DIR_INFO

         ///// <summary>FILE_ID_EXTD_DIR_INFO
         ///// <para>Identical to <see cref="FileIdExtdDirectoryInfo"/>, but forces the enumeration operation to start again from the beginning. Use only when calling GetFileInformationByHandleEx.</para>
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