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
   /// <remarks>https://technet.microsoft.com/en-us/library/cc739412.aspx</remarks>
   public enum DiskPartitionType
   {
      /// <summary>Unused partition entry.</summary>
      UnusedEntry = NativeMethods.DiskPartitionType.PARTITION_ENTRY_UNUSED,

      /// <summary>DOS FAT12 primary partition or logical drive (fewer than 32,680 sectors in the volume).</summary>
      Fat12 = NativeMethods.DiskPartitionType.PARTITION_FAT_12,

      /// <summary>A XENIX root partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Xenix")]
      XenixRoot = 2,

      /// <summary>A XENIX /usr partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Xenix")]
      XenixUser = 3,

      /// <summary>DOS 3.0+ FAT16 partition or logical drive (32,680–65,535 sectors or 16 MB–33 MB).</summary>
      Fat16 = NativeMethods.DiskPartitionType.PARTITION_FAT_16,

      /// <summary>DOS 3.3+ Extended partition.</summary>
      Extended = NativeMethods.DiskPartitionType.PARTITION_EXTENDED,

      /// <summary>DOS 3.31+ BIGDOS FAT16 partition or logical drive (33 MB–4 GB).</summary>
      Fat16B = 6,

      /// <summary>Installable File System (IFS). NTFS partition or logical drive.</summary>
      Ifs = NativeMethods.DiskPartitionType.PARTITION_IFS,

      /// <summary>FAT32 partition or logical drive.</summary>
      Fat32 = NativeMethods.DiskPartitionType.PARTITION_FAT32,

      /// <summary>FAT32 partition or logical drive using BIOS INT 13h extensions.</summary>
      Fat32Int13 = 12,

      /// <summary>BIGDOS FAT16 partition or logical drive using BIOS INT 13h extensions</summary>
      Fat16BInt13 = 14,

      /// <summary>Extended partition using BIOS INT 13h extensions</summary>
      ExtendedInt13 = 15,

      /// <summary>EISA partition or OEM partition</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Eisa")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Oem")]
      EisaOem = 18,

      /// <summary>A PC PReP (Power PC Reference Platform) Boot partition.</summary>
      Prep = 65,

      /// <summary>LDM (Logical Disk Manager) Dynamic volume.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ldm")]
      Ldm = NativeMethods.DiskPartitionType.PARTITION_LDM,

      /// <summary>SCO Unix, ISC, UnixWare, AT+T System V/386, ix, MtXinu BSD 4.3 on Mach, GNU HURD.</summary>
      Unix = 99,

      /// <summary>An NTFT partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ntft")]
      Ntft = NativeMethods.DiskPartitionType.PARTITION_NTFT,

      /// <summary>Power management hibernation partition.</summary>
      PowerManagementHibernation = 132,

      /// <summary>Multidisk FAT16 volume created by using Windows NT 4.0</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Multi")]
      Fat16MultiDisk = 134,

      /// <summary>Multidisk NTFS volume created by using Windows NT 4.0</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Multi")]
      NtfsMultiDisk = 135,

      /// <summary>Laptop hibernation partition</summary>
      LaptopHibernation = 160,

      /// <summary>A valid NTFT partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ntft")]
      ValidNtft = NativeMethods.DiskPartitionType.VALID_NTFT,

      /// <summary>Dell OEM partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Oem")]
      DellOem = 222,

      /// <summary>IBM OEM partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ibm")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Oem")]
      IbmOem = 254,

      /// <summary>GPT partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Gpt")]
      Gpt = 238,

      /// <summary>EFI System partition on an MBR disk.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Efi")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Mbr")]
      EfiSystemOnMbr = 239
  
   }
}
