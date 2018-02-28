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

using System.Diagnostics.CodeAnalysis;

namespace Alphaleonis.Win32.Filesystem
{
   /// <summary>The following table identifies the valid partition types that are used by disk drivers.</summary>
   /// <remarks>http://www.win.tue.nl/~aeb/partitions/partition_types-1.html</remarks>
   public enum DiskPartitionType
   {
      /// <summary>An unused entry partition.</summary>
      UnusedEntry = NativeMethods.DiskPartitionType.PARTITION_ENTRY_UNUSED,

      /// <summary>DOS 12-bit FAT partition.</summary>
      Fat12 = NativeMethods.DiskPartitionType.PARTITION_FAT_12,

      /// <summary>A XENIX root partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Xenix")]
      XenixRoot = 2,

      /// <summary>A XENIX /usr partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Xenix")]
      XenixUser = 3,

      /// <summary>DOS 3.0+ 16-bit FAT partition (up to 32M).</summary>
      Fat16 = NativeMethods.DiskPartitionType.PARTITION_FAT_16,

      /// <summary>DOS 3.3+ Extended partition.</summary>
      Extended = NativeMethods.DiskPartitionType.PARTITION_EXTENDED,

      /// <summary>DOS 3.31+ 16-bit FAT partition (over 32M).</summary>
      Fat16Big = 6,

      /// <summary>OS/2 IFS (Installable File System) (e.g., HPFS) partition.</summary>
      Ifs = NativeMethods.DiskPartitionType.PARTITION_IFS,

      /// <summary>A FAT32 file system partition.</summary>
      Fat32 = NativeMethods.DiskPartitionType.PARTITION_FAT32,

      /// <summary>A FAT32 XINT13 partition.</summary>
      Fat32XInt13 = 12,

      /// <summary>An XINT13 partition.</summary>
      XInt13 = 14,

      /// <summary>An XINT13 Extended partition.</summary>
      XInt13Extended = 15,

      /// <summary>A PREP (Power PC Reference Platform) partition.</summary>
      Prep = 65,

      /// <summary>An LDM (Logical Disk Manager) partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ldm")]
      Ldm = NativeMethods.DiskPartitionType.PARTITION_LDM,

      /// <summary>A UNIX partition.</summary>
      Unix = 99,

      /// <summary>An NTFT partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ntft")]
      Ntft = NativeMethods.DiskPartitionType.PARTITION_NTFT,

      /// <summary>A valid NTFT partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ntft")]
      ValidNtft = NativeMethods.DiskPartitionType.VALID_NTFT
   }
}
