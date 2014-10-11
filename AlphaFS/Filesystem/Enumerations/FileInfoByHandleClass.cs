/* Copyright (c) 2008-2014 Peter Palotas, Alexandr Normuradov, Jeffrey Jangli
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
      /// <para>Identifies the type of file information that <see cref="M:GetFileInformationByHandleEx"/> should retrieve or <see cref="M:SetFileInformationByHandle"/> should set.</para>
      /// </summary>
      internal enum FileInfoByHandleClass
      {
         #region FILE_BASIC_INFO

         ///// <summary>FILE_BASIC_INFO (0)
         ///// <para>Minimal information for the file should be retrieved or set. Used for file handles.</para>
         ///// <para>See <see cref="NativeMethods.FileBasicInfo"/></para>
         ///// </summary>
         //FileBasicInfo = 0,

         #endregion // FILE_BASIC_INFO

         #region FILE_STANDARD_INFO

         ///// <summary>FILE_STANDARD_INFO (1)
         ///// <para>Extended information for the file should be retrieved. Used for file handles.</para>
         ///// <para>Use only when calling <see cref="M:GetFileInformationByHandleEx"/>.</para>
         ///// </summary>
         //FileStandardInfo = 1,

         #endregion // FILE_STANDARD_INFO

         #region FILE_NAME_INFO

         ///// <summary>FILE_NAME_INFO (2) 
         ///// <para>The file name should be retrieved. Used for any handles.</para>
         ///// <para>Use only when calling <see cref="M:GetFileInformationByHandleEx"/>.</para>
         ///// </summary>
         //FileNameInfo = 2,

         #endregion // FILE_NAME_INFO

         #region FILE_NAME_INFO

         ///// <summary>FILE_RENAME_INFO (3)
         ///// <para>The file name should be changed. Used for file handles.</para>
         ///// <para>Use only when calling <see cref="M:SetFileInformationByHandle"/>.</para>
         ///// </summary>
         //FileRenameInfo = 3,

         #endregion // FILE_NAME_INFO

         #region FILE_DISPOSITION_INFO

         ///// <summary>FILE_DISPOSITION_INFO (4)
         ///// <para>The file should be deleted. Used for any handles.</para>
         ///// <para>Use only when calling <see cref="M:SetFileInformationByHandle"/>.</para>
         ///// </summary>
         //FileDispositionInfo = 4,

         #endregion // FILE_DISPOSITION_INFO

         #region FILE_ALLOCATION_INFO

         ///// <summary>FILE_ALLOCATION_INFO (5)
         ///// <para>The file allocation information should be changed. Used for file handles.</para>
         ///// <para>Use only when calling <see cref="M:SetFileInformationByHandle"/>.</para>
         ///// </summary>
         //FileAllocationInfo = 5,

         #endregion // FILE_ALLOCATION_INFO

         #region FILE_END_OF_FILE_INFO

         ///// <summary>FILE_END_OF_FILE_INFO (6)
         ///// <para>The end of the file should be set.</para>
         ///// <para>Use only when calling <see cref="M:SetFileInformationByHandle"/>.</para>
         ///// </summary>
         //FileEndOfFileInfo = 6,

         #endregion // FILE_END_OF_FILE_INFO

         #region FILE_STREAM_INFO

         ///// <summary>FILE_STREAM_INFO (7)
         ///// <para>File stream information for the specified file should be retrieved. Used for any handles.</para>
         ///// <para>Use only when calling <see cref="M:GetFileInformationByHandleEx"/>.</para>
         ///// </summary>
         //FileStreamInfo = 7,

         #endregion // FILE_STREAM_INFO

         #region FILE_COMPRESSION_INFO

         ///// <summary>FILE_COMPRESSION_INFO (8)
         ///// <para>File compression information should be retrieved. Used for any handles.</para>
         ///// <para>Use only when calling <see cref="M:GetFileInformationByHandleEx"/>.</para>
         ///// </summary>
         //FileCompressionInfo = 8,

         #endregion // FILE_COMPRESSION_INFO

         #region FILE_ATTRIBUTE_TAG_INFO

         ///// <summary>FILE_ATTRIBUTE_TAG_INFO (9)
         ///// <para>File attribute information should be retrieved. Used for any handles.</para>
         ///// <para>Use only when calling <see cref="M:GetFileInformationByHandleEx"/>.</para>
         ///// </summary>
         //FileAttributeTagInfo = 9,

         #endregion // FILE_ATTRIBUTE_TAG_INFO

         #region FILE_ID_BOTH_DIR_INFO

         /// <summary>FILE_ID_BOTH_DIR_INFO (10)
         /// <para>Files in the specified directory should be retrieved. Used for directory handles.</para>
         /// <para>Use only when calling <see cref="M:GetFileInformationByHandleEx"/>.</para>
         /// <remarks>
         /// <para>The number of files returned for each call to <see cref="M:GetFileInformationByHandleEx"/> depends on the size of the buffer that is passed to the function.</para>
         /// <para>Any subsequent calls to <see cref="M:GetFileInformationByHandleEx"/> on the same handle will resume the enumeration operation after the last file is returned.</para>
         /// </remarks>
         /// </summary>
         FileIdBothDirectoryInfo = 10,

         #endregion // FILE_ID_BOTH_DIR_INFO

         #region FILE_ID_BOTH_DIR_INFO

         ///// <summary>FILE_ID_BOTH_DIR_INFO (11)
         ///// <para>Identical to <see cref="FileIdBothDirectoryInfo"/>, but forces the enumeration operation to start again from the beginning.</para>
         ///// </summary>
         //FileIdBothDirectoryInfoRestartInfo = 11,

         #endregion // FILE_ID_BOTH_DIR_INFO

         #region FILE_IO_PRIORITY_HINT_INFO

         ///// <summary>FILE_IO_PRIORITY_HINT_INFO (12)
         ///// <para>Priority hint information should be set.</para>
         ///// <para>Use only when calling <see cref="M:SetFileInformationByHandle"/>.</para>
         ///// </summary>
         //FileIoPriorityHintInfo = 12,

         #endregion // FILE_IO_PRIORITY_HINT_INFO

         #region FILE_REMOTE_PROTOCOL_INFO

         ///// <summary>FILE_REMOTE_PROTOCOL_INFO (13)
         ///// <para>File remote protocol information should be retrieved. Use for any handles.</para>
         ///// <para>Use only when calling <see cref="M:GetFileInformationByHandleEx"/>.</para>
         ///// </summary>
         //FileRemoteProtocolInfo = 13,

         #endregion // FILE_REMOTE_PROTOCOL_INFO

         #region FILE_FULL_DIR_INFO

         ///// <summary>FILE_FULL_DIR_INFO (14)
         ///// <para>Files in the specified directory should be retrieved. Used for directory handles.</para>
         ///// <para>Use only when calling <see cref="M:GetFileInformationByHandleEx"/>.</para>
         ///// <remarks>
         ///// <para>Windows Server 2008 R2, Windows 7, Windows Server 2008, Windows Vista, Windows Server 2003, and Windows XP:</para>
         ///// <para>This value is not supported before Windows 8 and Windows Server 2012</para>
         ///// </remarks>
         ///// </summary>
         //FileFullDirectoryInfo = 14,

         #endregion // FILE_FULL_DIR_INFO

         #region FILE_FULL_DIR_INFO

         ///// <summary>FILE_FULL_DIR_INFO (15)
         ///// <para>Identical to <see cref="FileFullDirectoryInfo"/>, but forces the enumeration operation to start again from the beginning.</para>
         ///// <para>Use only when calling <see cref="M:GetFileInformationByHandleEx"/>.</para>
         ///// <remarks>
         ///// <para>Windows Server 2008 R2, Windows 7, Windows Server 2008, Windows Vista, Windows Server 2003, and Windows XP:</para>
         ///// <para>This value is not supported before Windows 8 and Windows Server 2012</para>
         ///// </remarks>
         ///// </summary>
         //FileFullDirectoryRestartInfo = 15,

         #endregion // FILE_FULL_DIR_INFO

         #region FILE_STORAGE_INFO

         ///// <summary>FILE_STORAGE_INFO (16)
         ///// <para>File storage information should be retrieved. Use for any handles.</para>
         ///// <para>Use only when calling <see cref="M:GetFileInformationByHandleEx"/>.</para>
         ///// <remarks>
         ///// <para>Windows Server 2008 R2, Windows 7, Windows Server 2008, Windows Vista, Windows Server 2003, and Windows XP:</para>
         ///// <para>This value is not supported before Windows 8 and Windows Server 2012</para>
         ///// </remarks>
         ///// </summary>
         //FileStorageInfo = 16,

         #endregion // FILE_STORAGE_INFO

         #region FILE_ALIGNMENT_INFO

         ///// <summary>FILE_ALIGNMENT_INFO (17)
         ///// <para>File alignment information should be retrieved. Use for any handles.</para>
         ///// <para>Use only when calling <see cref="M:GetFileInformationByHandleEx"/>.</para>
         ///// <remarks>
         ///// <para>Windows Server 2008 R2, Windows 7, Windows Server 2008, Windows Vista, Windows Server 2003, and Windows XP:</para>
         ///// <para>This value is not supported before Windows 8 and Windows Server 2012</para>
         ///// </remarks>
         ///// </summary>
         //FileAlignmentInfo = 17,

         #endregion FILE_ALIGNMENT_INFO

         #region FILE_ID_INFO

         ///// <summary>FILE_ID_INFO (18)
         ///// <para>File information should be retrieved. Use for any handles.</para>
         ///// <para>Use only when calling <see cref="M:GetFileInformationByHandleEx"/>.</para>
         ///// <remarks>
         ///// <para>Windows Server 2008 R2, Windows 7, Windows Server 2008, Windows Vista, Windows Server 2003, and Windows XP:</para>
         ///// <para>This value is not supported before Windows 8 and Windows Server 2012</para>
         ///// </remarks>
         ///// </summary>
         //FileIdInfo = 18,

         #endregion // FILE_ID_INFO

         #region FILE_ID_EXTD_DIR_INFO

         ///// <summary>FILE_ID_EXTD_DIR_INFO (19)
         ///// <para>Files in the specified directory should be retrieved. Used for directory handles.</para>
         ///// <para>Use only when calling <see cref="M:GetFileInformationByHandleEx"/>.</para>
         ///// <remarks>
         ///// <para>Windows Server 2008 R2, Windows 7, Windows Server 2008, Windows Vista, Windows Server 2003, and Windows XP:</para>
         ///// <para>This value is not supported before Windows 8 and Windows Server 2012</para>
         ///// </remarks>
         ///// </summary>
         //FileIdExtdDirectoryInfo = 19,

         #endregion // FILE_ID_EXTD_DIR_INFO

         #region FILE_ID_EXTD_DIR_INFO

         ///// <summary>FILE_ID_EXTD_DIR_INFO (20)
         ///// <para>Identical to <see cref="FileIdExtdDirectoryInfo"/>, but forces the enumeration operation to start again from the beginning.</para>
         ///// <para>Use only when calling <see cref="M:GetFileInformationByHandleEx"/>.</para>
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