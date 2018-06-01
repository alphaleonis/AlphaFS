/*  Copyright (C) 2008-2018 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
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
      /// <summary>Valid partition types that are used by disk drivers.
      /// <para>Minimum supported client: Windows XP [desktop apps only]</para>
      /// <para>Minimum supported server: Windows Server 2003 [desktop apps only]</para>
      /// </summary>
      internal enum DiskPartitionType : byte
      {
         /// <summary>Unused partition entry.</summary>
         PARTITION_ENTRY_UNUSED = 0,

         /// <summary>DOS FAT12 primary partition or logical drive (fewer than 32,680 sectors in the volume).</summary>
         PARTITION_FAT_12 = 1,

         /// <summary>DOS 3.0+ FAT16 partition or logical drive (32,680–65,535 sectors or 16 MB–33 MB).</summary>
         PARTITION_FAT_16 = 4,

         /// <summary>DOS 3.3+ Extended partition.</summary>
         PARTITION_EXTENDED = 5,

         /// <summary>Installable File System (IFS). NTFS partition or logical drive.</summary>
         PARTITION_IFS = 7,

         /// <summary>FAT32 partition or logical drive.</summary>
         PARTITION_FAT32 = 11,

         /// <summary>LDM (Logical Disk Manager) Data partition on a dynamic disk.</summary>
         PARTITION_LDM = 66,

         /// <summary>An NTFT partition.</summary>
         PARTITION_NTFT = 128,

         /// <summary>A valid NTFT partition. The high bit of a partition type code indicates that a partition is part of an NTFT mirror or striped array.</summary>
         VALID_NTFT = 192
      }
   }
}
