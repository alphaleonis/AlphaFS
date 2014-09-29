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

using System.Runtime.InteropServices;

namespace Alphaleonis.Win32.Filesystem
{
   internal static partial class NativeMethods
   {
      /// <summary>The Win32StreamId structure contains stream data.</summary>
      [StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Unicode)]
      internal struct Win32StreamId
      {
         /// <summary>Type of data.
         /// This member can be one of the following values:
         ///   BACKUP_DATA            0x00000001  Standard data. This corresponds to the NTFS $DATA stream type on the default (unnamed) data stream.
         ///   BACKUP_EA_DATA         0x00000002  Extended attribute data. This corresponds to the NTFS $EA stream type.
         ///   BACKUP_SECURITY_DATA   0x00000003  Security descriptor data.
         ///   BACKUP_ALTERNATE_DATA  0x00000004  Alternative data streams. This corresponds to the NTFS $DATA stream type on a named data stream.
         ///   BACKUP_LINK            0x00000005  Hard link information. This corresponds to the NTFS $FILE_NAME stream type.
         ///   BACKUP_PROPERTY_DATA   0x00000006  Property data.
         ///   BACKUP_OBJECT_ID       0x00000007  Objects identifiers. This corresponds to the NTFS $OBJECT_ID stream type.
         ///   BACKUP_REPARSE_DATA    0x00000008  Reparse points. This corresponds to the NTFS $REPARSE_POINT stream type.
         ///   BACKUP_SPARSE_BLOCK    0x00000009  Sparse file. This corresponds to the NTFS $DATA stream type for a sparse file.
         ///   BACKUP_TXFS_DATA       0x0000000A  Transactional NTFS (TxF) data stream. This corresponds to the NTFS $TXF_DATA stream type.
         ///                                      Windows Server 2003 and Windows XP:  This value is not supported.
         /// </summary>
         public readonly uint StreamId;

         /// <summary>Attributes of data to facilitate cross-operating system transfer.
         /// This member can be one or more of the following values:
         ///   STREAM_MODIFIED_WHEN_READ  0x00000001  Attribute set if the stream contains data that is modified when read. Allows the backup application to know that verification of data will fail.
         ///   STREAM_CONTAINS_SECURITY   0x00000002  Stream contains security data (general attributes). Allows the stream to be ignored on cross-operations restore.
         /// </summary>
         public readonly BackupStreamAttributes StreamAttributes;

         /// <summary>Size of data, in bytes.</summary>
         public readonly ulong StreamSize;

         /// <summary>Length of the name of the alternative data stream, in bytes.</summary>
         public readonly uint StreamNameSize;
      }
   }
}