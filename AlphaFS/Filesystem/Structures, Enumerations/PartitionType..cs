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

using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Alphaleonis.Win32.Filesystem
{
   public enum PartitionType
   {
      /// <summary>PARTITION_ENTRY_UNUSED_GUID: There is no partition.
      /// <remarks>This value can be set for basic and dynamic disks.</remarks>
      /// </summary>
      [Description("00000000-0000-0000-0000-000000000000")]
      EntryUnused,

      /// <summary>PARTITION_BASIC_DATA_GUID: The data partition type that is created and recognized by Windows.
      /// <remarks>
      /// <para>This value can be set only for basic disks, with one exception.</para>
      /// <para>Only partitions of this type can be assigned drive letters, receive volume GUID paths, host mounted folders (also called volume mount points), and be enumerated by calls to FindFirstVolume and FindNextVolume.</para>
      /// </remarks>
      /// </summary>
      [Description("EBD0A0A2-B9E5-4433-87C0-68B6B72699C7")]
      BasicData,

      /// <summary>PARTITION_LDM_DATA_GUID: The partition is an LDM data partition on a dynamic disk.
      /// <remarks>This value can be set only for dynamic disks.</remarks>
      /// </summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ldm")]
      [Description("AF9B60A0-1431-4F62-BC68-3311714A69AD")]
      LdmData,

      /// <summary>PARTITION_LDM_METADATA_GUID: The partition is a Logical Disk Manager (LDM) metadata partition on a dynamic disk.
      /// <remarks>This value can be set only for dynamic disks.</remarks>
      /// </summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ldm")]
      [Description("5808C8AA-7E8F-42E0-85D2-E1E90434CFB3")]
      LdmMetadata,

      /// <summary>PARTITION_MSFT_RECOVERY_GUID: The partition is a Microsoft recovery partition.
      /// <remarks>This value can be set for basic and dynamic disks.</remarks>
      /// </summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Msft")]
      [Description("DE94BBA4-06D1-4D40-A16A-BFD50179D6AC")]
      MsftRecovery,

      /// <summary>PARTITION_MSFT_RESERVED_GUID: The partition is a Microsoft reserved partition.
      /// <remarks>This value can be set for basic and dynamic disks.</remarks>
      /// </summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Msft")]
      [SuppressMessage("Microsoft.Naming", "CA1700:DoNotNameEnumValuesReserved")]
      [Description("E3C9E316-0B5C-4DB8-817D-F92DF00215AE")]
      MsftReserved,

      /// <summary>PARTITION_SYSTEM_GUID: The partition is an EFI system partition.
      /// <remarks>This value can be set for basic and dynamic disks.</remarks>
      /// </summary>
      [Description("C12A7328-F81F-11D2-BA4B-00A0C93EC93B")]
      System,




      // 2018-02-27 GUID Partition Table
      // https://en.wikipedia.org/wiki/GUID_Partition_Table


      /// <summary>IBM General Parallel File System (GPFS) partition.</summary>
      [Description("37AFFC90-EF7D-4E96-91C3-2D7AE055B174")]
      IbmGpfs,

      /// <summary>Storage Spaces partition.</summary>
      [Description("E75CAF8F-F680-4CEE-AFA3-B001E56EFC2D")]
      StorageSpace,

      /// <summary>MBR partition scheme.</summary>
      [Description("024DEE41-33E7-11D3-9D69-0008C781F39F")]
      MbrScheme,

      /// <summary>BIOS boot partition.</summary>
      [Description("21686148-6449-6E6F-744E-656564454649F")]
      BiosBoot,

      /// <summary>Intel Fast Flash (iFFS) partition (for Intel Rapid Start technology).</summary>
      [Description("D3BFE2DE-3DAF-11DF-BA40-E3A556D89593")]
      iFfs,

      /// <summary>Sony boot partition.</summary>
      [Description("F4019732-066E-4E12-8273-346C5641494F")]
      SonyBoot,

      /// <summary>Lenovo boot partition.</summary>
      [Description("BFBFAFE7-A34F-448A-9A5B-6213EB736C22")]
      LenovoBoot,
   }
}
