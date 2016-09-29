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

using System.Diagnostics.CodeAnalysis;

namespace Alphaleonis.Win32.Filesystem
{
   /// <summary>The type of the data contained in the backup stream.</summary>
   public enum BackupStreamType
   {
      /// <summary>This indicates an error.</summary>
      None = 0,

      /// <summary>BACKUP_DATA
      /// <para>Standard data. This corresponds to the NTFS $DATA stream type on the default (unnamed) data stream.</para>
      /// </summary>
      Data = 1,

      /// <summary>BACKUP_EA_DATA
      /// <para>Extended attribute data. This corresponds to the NTFS $EA stream type.</para>
      /// </summary>
      ExtendedAttributesData = 2,

      /// <summary>BACKUP_SECURITY_DATA
      /// <para>Security descriptor data.</para>
      /// </summary>
      SecurityData = 3,

      /// <summary>BACKUP_ALTERNATE_DATA
      /// <para>Alternative data streams. This corresponds to the NTFS $DATA stream type on a named data stream.</para>
      /// </summary>
      AlternateData = 4,

      /// <summary>BACKUP_LINK
      /// <para>Hard link information. This corresponds to the NTFS $FILE_NAME stream type.</para>
      /// </summary>
      Link = 5,

      /// <summary>BACKUP_PROPERTY_DATA
      /// <para>Property data.</para>
      /// </summary>
      PropertyData = 6,

      /// <summary>BACKUP_OBJECT_ID
      /// <para>Objects identifiers. This corresponds to the NTFS $OBJECT_ID stream type.</para>
      /// </summary>
      ObjectId = 7,

      /// <summary>BACKUP_REPARSE_DATA
      /// <para>Reparse points. This corresponds to the NTFS $REPARSE_POINT stream type.</para>
      /// </summary>
      ReparseData = 8,

      /// <summary>BACKUP_SPARSE_BLOCK
      /// <para>Sparse file. This corresponds to the NTFS $DATA stream type for a sparse file.</para>
      /// </summary>
      SparseBlock = 9,

      /// <summary>BACKUP_TXFS_DATA
      /// <para>Transactional NTFS (TxF) data stream.</para>
      /// </summary>
      /// <remarks>Windows Server 2003 and Windows XP:  This value is not supported.</remarks>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Txfs")]
      TxfsData = 10
   }
}