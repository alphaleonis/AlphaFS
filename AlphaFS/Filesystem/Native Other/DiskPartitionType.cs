/*  Copyright (C) 2008-2017 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
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
      /// <summary>The following table identifies the valid partition types that are used by disk drivers.
      /// <para>Minimum supported client: Windows XP [desktop apps only]</para>
      /// <para>Minimum supported server: Windows Server 2003 [desktop apps only]</para>
      /// </summary>
      internal enum DiskPartitionType : byte
      {
         /// <summary>An unused entry partition.</summary>
         PARTITION_ENTRY_UNUSED = 0,

         /// <summary>A FAT12 file system partition.</summary>
         PARTITION_FAT_12 = 1,

         /// <summary>A FAT16 file system partition.</summary>
         PARTITION_FAT_16 = 4,

         /// <summary>An extended partition.</summary>
         PARTITION_EXTENDED = 5,

         /// <summary>An IFS (Installable File System) partition.</summary>
         PARTITION_IFS = 7,

         /// <summary>A FAT32 file system partition.</summary>
         PARTITION_FAT32 = 11,

         /// <summary>An LDM (Logical Disk Manager) partition.</summary>
         PARTITION_LDM = 66,

         /// <summary>An NTFT partition.</summary>
         PARTITION_NTFT = 128,

         /// <summary>A valid NTFT partition. The high bit of a partition type code indicates that a partition is part of an NTFT mirror or striped array.</summary>
         VALID_NTFT = 192
      }
   }
}
