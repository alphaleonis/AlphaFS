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
      /// <summary>FILE_INFO_BY_HANDLE_CLASS - Identifies the type of file information that GetFileInformationByHandleEx should retrieve or SetFileInformationByHandle should set.</summary>
      internal enum FileInfoByHandleClass
      {
         ///// <summary>Minimal information for the file should be retrieved or set. Used for file handles. See FILE_BASIC_INFO.</summary>
         //FileBasicInfo = 0,

         ///// <summary>Extended information for the file should be retrieved. Used for file handles. Use only when calling GetFileInformationByHandleEx. See FILE_STANDARD_INFO.</summary>
         //FileStandardInfo = 1,

         ///// <summary>The file name should be retrieved. Used for any handles. Use only when calling GetFileInformationByHandleEx. See FILE_NAME_INFO.</summary>
         //FileNameInfo = 2,

         ///// <summary>The file name should be changed. Used for file handles. Use only when calling SetFileInformationByHandle. See FILE_RENAME_INFO.</summary>
         //FileRenameInfo = 3,

         ///// <summary>The file should be deleted. Used for any handles. Use only when calling SetFileInformationByHandle. See FILE_DISPOSITION_INFO.</summary>
         //FileDispositionInfo = 4,

         ///// <summary>The file allocation information should be changed. Used for file handles. Use only when calling SetFileInformationByHandle. See FILE ALLOCATION INFO.</summary>
         //FileAllocationInfo = 5,

         ///// <summary>The end of the file should be set. Use only when calling SetFileInformationByHandle. See FILE_END_OF_FILE_INFO.</summary>
         //FileEndOfFileInfo = 6,

         ///// <summary>File stream information for the specified file should be retrieved. Used for any handles. Use only when calling GetFileInformationByHandleEx. See FILE_STREAM_INFO.</summary>
         //FileStreamInfo = 7,

         ///// <summary>File compression information should be retrieved. Used for any handles. Use only when calling GetFileInformationByHandleEx. See FILE_COMPRESSION_INFO.</summary>
         //FileCompressionInfo = 8,

         ///// <summary>File attribute information should be retrieved. Used for any handles. Use only when calling GetFileInformationByHandleEx. See FILE_ATTRIBUTE_TAG_INFO.</summary>
         //FileAttributeTagInfo = 9,

         /// <summary>Files in the specified directory should be retrieved. Used for directory handles. Use only when calling GetFileInformationByHandleEx.
         /// The number of files returned for each call to GetFileInformationByHandleEx depends on the size of the buffer that is passed to the function.
         /// Any subsequent calls to GetFileInformationByHandleEx on the same handle will resume the enumeration operation after the last file is returned.
         /// See FILE_ID_BOTH_DIR_INFO.
         /// </summary>
         FileIdBothDirectoryInfo = 10,

         ///// <summary>Identical to FileIdBothDirectoryInfo, but forces the enumeration operation to start again from the beginning. See FILE_ID_BOTH_DIR_INFO.</summary>
         //FileIdBothDirectoryRestartInfo = 11,

         ///// <summary>Priority hint information should be set. Use only when calling SetFileInformationByHandle. See FILE_IO_PRIORITY_HINT_INFO.</summary>
         //FileIoPriorityHintInfo = 12,

         ///// <summary>File remote protocol information should be retrieved. Use for any handles. Use only when calling GetFileInformationByHandleEx. See FILE_REMOTE_PROTOCOL_INFO.</summary>
         //FileRemoteProtocolInfo = 13,

         ///// <summary>Files in the specified directory should be retrieved. Used for directory handles. Use only when calling GetFileInformationByHandleEx. See FILE_FULL_DIR_INFO.</summary>
         ///// <remarks>Windows Server 2008 R2, Windows 7, Windows Server 2008, Windows Vista, Windows Server 2003, and Windows XP:  This value is not supported before Windows 8 and Windows Server 2012</remarks>
         //FileFullDirectoryInfo = 14,

         ///// <summary>dentical to FileFullDirectoryInfo, but forces the enumeration operation to start again from the beginning.
         ///// Use only when calling GetFileInformationByHandleEx. See FILE_FULL_DIR_INFO.</summary>
         ///// <remarks>Windows Server 2008 R2, Windows 7, Windows Server 2008, Windows Vista, Windows Server 2003, and Windows XP:  This value is not supported before Windows 8 and Windows Server 2012</remarks>
         //FileFullDirectoryRestartInfo = 15,

         ///// <summary>File storage information should be retrieved. Use for any handles. Use only when calling GetFileInformationByHandleEx. See FILE_STORAGE_INFO.</summary>
         ///// <remarks>Windows Server 2008 R2, Windows 7, Windows Server 2008, Windows Vista, Windows Server 2003, and Windows XP:  This value is not supported before Windows 8 and Windows Server 2012</remarks>
         //FileStorageInfo = 16,

         ///// <summary>File alignment information should be retrieved. Use for any handles. Use only when calling GetFileInformationByHandleEx. See FILE_ALIGNMENT_INFO.</summary>
         ///// <remarks>Windows Server 2008 R2, Windows 7, Windows Server 2008, Windows Vista, Windows Server 2003, and Windows XP:  This value is not supported before Windows 8 and Windows Server 2012</remarks>
         //FileAlignmentInfo = 17,

         ///// <summary>File information should be retrieved. Use for any handles. Use only when calling GetFileInformationByHandleEx. See FILE_ID_INFO.</summary>
         ///// <remarks>Windows Server 2008 R2, Windows 7, Windows Server 2008, Windows Vista, Windows Server 2003, and Windows XP:  This value is not supported before Windows 8 and Windows Server 2012</remarks>
         //FileIdInfo = 18,

         ///// <summary>Files in the specified directory should be retrieved. Used for directory handles. Use only when calling GetFileInformationByHandleEx. See FILE_ID_EXTD_DIR_INFO.</summary>
         ///// <remarks>Windows Server 2008 R2, Windows 7, Windows Server 2008, Windows Vista, Windows Server 2003, and Windows XP:  This value is not supported before Windows 8 and Windows Server 2012</remarks>
         //FileIdExtdDirectoryInfo = 19,

         ///// <summary>Identical to FileIdExtdDirectoryInfo, but forces the enumeration operation to start again from the beginning. Use only when calling GetFileInformationByHandleEx. See FILE_ID_EXTD_DIR_INFO.</summary>
         ///// <remarks>Windows Server 2008 R2, Windows 7, Windows Server 2008, Windows Vista, Windows Server 2003, and Windows XP:  This value is not supported before Windows 8 and Windows Server 2012</remarks>
         //FileIdExtdDirectoryRestartInfo = 20
      }
   }
}