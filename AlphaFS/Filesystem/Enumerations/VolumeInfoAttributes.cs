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

using System;

namespace Alphaleonis.Win32.Filesystem
{
   internal static partial class NativeMethods
   {
      /// <summary>Volume Attributes used by the GetVolumeInformation() function.</summary>
      [Flags]
      internal enum VolumeInfoAttributes
      {
         /// <summary>No VolumeInfo attributes.</summary>
         None = 0,

         /// <summary>FILE_CASE_SENSITIVE_SEARCH The specified volume supports case-sensitive file names.</summary>
         CaseSensitiveSearch = 1,

         /// <summary> FILE_CASE_PRESERVED_NAMES The specified volume supports preserved case of file names when it places a name on disk.</summary>
         CasePreservedNames = 2,

         /// <summary> FILE_UNICODE_ON_DISK The specified volume supports Unicode in file names as they appear on disk.</summary>
         UnicodeOnDisk = 4,

         /// <summary> FILE_PERSISTENT_ACLS The specified volume preserves and enforces access control lists (ACL).
         /// For example, the NTFS file system preserves and enforces ACLs, and the FAT file system does not.
         /// </summary>
         PersistentAcls = 8,

         /// <summary> FILE_FILE_COMPRESSION The specified volume supports file-based compression.</summary>
         Compression = 16,

         /// <summary> FILE_VOLUME_QUOTAS The specified volume supports disk quotas.</summary>
         VolumeQuotas = 32,

         /// <summary> FILE_SUPPORTS_SPARSE_FILES The specified volume supports sparse files.</summary>
         SupportsSparseFiles = 64,

         /// <summary> FILE_SUPPORTS_REPARSE_POINTS The specified volume supports re-parse points.</summary>
         SupportsReparsePoints = 128,

         /// <summary>(doesn't appear on MSDN)</summary>
         SupportsRemoteStorage = 256,

         /// <summary>FILE_VOLUME_IS_COMPRESSED The specified volume is a compressed volume, for example, a DoubleSpace volume.</summary>
         VolumeIsCompressed = 32768,

         /// <summary>FILE_SUPPORTS_OBJECT_IDS The specified volume supports object identifiers.</summary>
         SupportsObjectIds = 65536,

         /// <summary>FILE_SUPPORTS_ENCRYPTION The specified volume supports the Encrypted File System (EFS). For more information, see File Encryption.</summary>
         SupportsEncryption = 131072,

         /// <summary>FILE_NAMED_STREAMS The specified volume supports named streams.</summary>
         NamedStreams = 262144,

         /// <summary>FILE_READ_ONLY_VOLUME The specified volume is read-only.</summary>
         ReadOnlyVolume = 524288,

         /// <summary>FILE_SEQUENTIAL_WRITE_ONCE The specified volume is read-only.</summary>
         SequentialWriteOnce = 1048576,

         /// <summary>FILE_SUPPORTS_TRANSACTIONS The specified volume supports transactions.For more information, see About KTM.</summary>
         SupportsTransactions = 2097152,

         /// <summary>FILE_SUPPORTS_HARD_LINKS The specified volume supports hard links. For more information, see Hard Links and Junctions.</summary>
         /// <remarks>
         /// Windows Server 2008, Windows Vista, Windows Server 2003, and Windows XP:
         /// This value is not supported until Windows Server 2008 R2 and Windows 7.
         /// </remarks>
         SupportsHardLinks = 4194304,

         /// <summary>FILE_SUPPORTS_EXTENDED_ATTRIBUTES The specified volume supports extended attributes.
         /// An extended attribute is a piece of application-specific metadata that
         /// an application can associate with a file and is not part of the file's data.
         /// </summary>
         /// <remarks>
         /// Windows Server 2008, Windows Vista, Windows Server 2003, and Windows XP:
         /// This value is not supported until Windows Server 2008 R2 and Windows 7.
         /// </remarks>
         SupportsExtendedAttributes = 8388608,

         /// <summary>FILE_SUPPORTS_OPEN_BY_FILE_ID The file system supports open by FileID. For more information, see FILE_ID_BOTH_DIR_INFO.</summary>
         /// <remarks>
         /// Windows Server 2008, Windows Vista, Windows Server 2003, and Windows XP:
         /// This value is not supported until Windows Server 2008 R2 and Windows 7.
         /// </remarks>
         SupportsOpenByFileId = 16777216,

         /// <summary>FILE_SUPPORTS_USN_JOURNAL The specified volume supports update sequence number (USN) journals. For more information, see Change Journal Records.</summary>
         /// <remarks>
         /// Windows Server 2008, Windows Vista, Windows Server 2003, and Windows XP:
         /// This value is not supported until Windows Server 2008 R2 and Windows 7.
         /// </remarks>
         SupportsUsnJournal = 33554432
      }
   }
}