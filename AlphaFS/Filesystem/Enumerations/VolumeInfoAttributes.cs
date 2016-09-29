/*  Copyright (C) 2008-2016 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
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
   internal static partial class NativeMethods
   {
      /// <summary>Volume Attributes used by the GetVolumeInfo() function.</summary>
      [Flags]
      internal enum VolumeInfoAttributes
      {
         /// <summary>No VolumeInfo attributes.</summary>
         None = 0,

         /// <summary>FILE_CASE_SENSITIVE_SEARCH
         /// <para>The specified volume supports case-sensitive file names.</para>
         /// </summary>
         CaseSensitiveSearch = 1,

         /// <summary>FILE_CASE_PRESERVED_NAMES
         /// <para>The specified volume supports preserved case of file names when it places a name on disk.</para>
         /// </summary>
         CasePreservedNames = 2,

         /// <summary>FILE_UNICODE_ON_DISK
         /// <para>The specified volume supports Unicode in file names as they appear on disk.</para>
         /// </summary>
         UnicodeOnDisk = 4,

         /// <summary>FILE_PERSISTENT_ACLS
         /// <para>
         /// The specified volume preserves and enforces access control lists (ACL).
         /// For example, the NTFS file system preserves and enforces ACLs, and the FAT file system does not.
         /// </para>
         /// </summary>
         PersistentAcls = 8,

         /// <summary>FILE_FILE_COMPRESSION
         /// <para>The specified volume supports file-based compression.</para>
         /// </summary>
         Compression = 16,

         /// <summary>FILE_VOLUME_QUOTAS
         /// <para>The specified volume supports disk quotas.</para>
         /// </summary>
         VolumeQuotas = 32,

         /// <summary>FILE_SUPPORTS_SPARSE_FILES
         /// <para>The specified volume supports sparse files.</para>
         /// </summary>
         SupportsSparseFiles = 64,

         /// <summary>FILE_SUPPORTS_REPARSE_POINTS
         /// <para>The specified volume supports re-parse points.</para>
         /// </summary>
         SupportsReparsePoints = 128,

         /// <summary>(does not appear on MSDN)</summary>
         SupportsRemoteStorage = 256,

         /// <summary>FILE_VOLUME_IS_COMPRESSED
         /// <para>The specified volume is a compressed volume, for example, a DoubleSpace volume.</para>
         /// </summary>
         VolumeIsCompressed = 32768,

         /// <summary>FILE_SUPPORTS_OBJECT_IDS
         /// <para>The specified volume supports object identifiers.</para>
         /// </summary>
         SupportsObjectIds = 65536,

         /// <summary>FILE_SUPPORTS_ENCRYPTION
         /// <para>The specified volume supports the Encrypted File System (EFS). For more information, see File Encryption.</para>
         /// </summary>
         SupportsEncryption = 131072,

         /// <summary>FILE_NAMED_STREAMS
         /// <para>The specified volume supports named streams.</para>
         /// </summary>
         NamedStreams = 262144,

         /// <summary>FILE_READ_ONLY_VOLUME
         /// <para>The specified volume is read-only.</para>
         /// </summary>
         ReadOnlyVolume = 524288,

         /// <summary>FILE_SEQUENTIAL_WRITE_ONCE
         /// <para>The specified volume is read-only.</para>
         /// </summary>
         SequentialWriteOnce = 1048576,

         /// <summary>FILE_SUPPORTS_TRANSACTIONS
         /// <para>The specified volume supports transactions.For more information, see About KTM.</para>
         /// </summary>
         SupportsTransactions = 2097152,

         /// <summary>FILE_SUPPORTS_HARD_LINKS
         /// <para>The specified volume supports hard links. For more information, see Hard Links and Junctions.</para>
         /// </summary>
         /// <remarks>Windows Server 2008, Windows Vista, Windows Server 2003, and Windows XP: This value is not supported until Windows Server 2008 R2 and Windows 7.</remarks>
         SupportsHardLinks = 4194304,

         /// <summary>FILE_SUPPORTS_EXTENDED_ATTRIBUTES
         /// <para>
         /// The specified volume supports extended attributes. An extended attribute is a piece of application-specific metadata
         /// that an application can associate with a file and is not part of the file's data.
         /// </para>
         /// </summary>
         /// <remarks>Windows Server 2008, Windows Vista, Windows Server 2003, and Windows XP: This value is not supported until Windows Server 2008 R2 and Windows 7.</remarks>
         SupportsExtendedAttributes = 8388608,

         /// <summary>FILE_SUPPORTS_OPEN_BY_FILE_ID
         /// <para>The file system supports open by FileID. For more information, see FILE_ID_BOTH_DIR_INFO.</para>
         /// </summary>
         /// <remarks>Windows Server 2008, Windows Vista, Windows Server 2003, and Windows XP: This value is not supported until Windows Server 2008 R2 and Windows 7.</remarks>
         SupportsOpenByFileId = 16777216,

         /// <summary>FILE_SUPPORTS_USN_JOURNAL
         /// <para>The specified volume supports update sequence number (USN) journals. For more information, see Change Journal Records.</para>
         /// </summary>
         /// <remarks>Windows Server 2008, Windows Vista, Windows Server 2003, and Windows XP: This value is not supported until Windows Server 2008 R2 and Windows 7.</remarks>
         SupportsUsnJournal = 33554432
      }
   }
}