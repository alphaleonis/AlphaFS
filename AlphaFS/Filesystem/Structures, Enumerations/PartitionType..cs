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

using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Alphaleonis.Win32.Filesystem
{
   /// <summary>Specifies the different types of partition GUIDs.
   /// <para>https://en.wikipedia.org/wiki/GUID_Partition_Table</para>
   /// </summary>
   public enum PartitionType
   {
      #region Non-OS

      /// <summary>There is no partition.
      /// <remarks>This value can be set for basic and dynamic disks.</remarks>
      /// </summary>
      [Description("00000000-0000-0000-0000-000000000000")]
      UnusedEntry,

      /// <summary>Legacy MBR partition. A partition that is sub-partitioned by a Master Boot Record; "partitions-inside-a-slice configuration".</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Mbr")]
      [Description("024DEE41-33E7-11D3-9D69-0008C781F39F")]
      LegacyMbr,

      /// <summary>The partition is an EFI system partition.
      /// <remarks>This value can be set for basic and dynamic disks.</remarks>
      /// </summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Efi")]
      [Description("C12A7328-F81F-11D2-BA4B-00A0C93EC93B")]
      EfiSystem,

      /// <summary>BIOS boot partition.</summary>
      [Description("21686148-6449-6E6F-744E-656564454649F")]
      BiosBoot,

      /// <summary>Intel Fast Flash (iFFS, Intel Rapid Start technology) partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Iffs")]
      [Description("D3BFE2DE-3DAF-11DF-BA40-E3A556D89593")]
      Iffs,

      /// <summary>Sony boot partition.</summary>
      [Description("F4019732-066E-4E12-8273-346C5641494F")]
      SonyBoot,

      /// <summary>Lenovo boot partition.</summary>
      [Description("BFBFAFE7-A34F-448A-9A5B-6213EB736C22")]
      LenovoBoot,

      #endregion // Non-OS


      #region Windows

      /// <summary>The partition is a Microsoft reserved partition.
      /// <remarks>This value can be set for basic and dynamic disks.</remarks>
      /// </summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Msft")]
      [SuppressMessage("Microsoft.Naming", "CA1700:DoNotNameEnumValuesReserved")]
      [Description("E3C9E316-0B5C-4DB8-817D-F92DF00215AE")]
      MsftReserved,
      
      /// <summary>The data partition type that is created and recognized by Windows.
      /// <remarks>
      /// <para>This value can be set only for basic disks, with one exception.</para>
      /// <para>Only partitions of this type can be assigned drive letters, receive volume GUID paths, host mounted folders (also called volume mount points), and be enumerated by calls to FindFirstVolume and FindNextVolume.</para>
      /// </remarks>
      /// </summary>
      [Description("EBD0A0A2-B9E5-4433-87C0-68B6B72699C7")]
      BasicData,

      /// <summary>The partition is a Logical Disk Manager (LDM) metadata partition on a dynamic disk.
      /// <remarks>This value can be set only for dynamic disks.</remarks>
      /// </summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ldm")]
      [Description("5808C8AA-7E8F-42E0-85D2-E1E90434CFB3")]
      LdmMetadata,

      /// <summary>The partition is an LDM data partition on a dynamic disk.
      /// <remarks>This value can be set only for dynamic disks.</remarks>
      /// </summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ldm")]
      [Description("AF9B60A0-1431-4F62-BC68-3311714A69AD")]
      LdmData,
      
      /// <summary>The partition is a Microsoft recovery partition.
      /// <remarks>This value can be set for basic and dynamic disks.</remarks>
      /// </summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Msft")]
      [Description("DE94BBA4-06D1-4D40-A16A-BFD50179D6AC")]
      MsftRecovery,
      
      /// <summary>IBM General Parallel File System (GPFS) partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ibm")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Gpfs")]
      [Description("37AFFC90-EF7D-4E96-91C3-2D7AE055B174")]
      IbmGpfs,

      /// <summary>Storage Spaces partition.</summary>
      [Description("E75CAF8F-F680-4CEE-AFA3-B001E56EFC2D")]
      StorageSpace,

      #endregion // Windows


      #region HP-UX

      /// <summary>HP-UX Data partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ux")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Hp")]
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Ux")]
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Hp")]
      [Description("75894C1E-3AEB-11D3-B7C1-7B03A0000000")]
      HpUxData,

      /// <summary>HP-UX Service partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Ux")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ux")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Hp")]
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Hp")]
      [Description("E2A1E728-32E3-11D6-A682-7B03A0000000")]
      HpUxService,

      #endregion // HP-UX


      #region Linux
      
      /// <summary>Linux filesystem data partition.</summary>
      [Description("0FC63DAF-8483-4772-8E79-3D69D8477DE4")]
      LinuxData,

      /// <summary>Linux RAID partition.</summary>
      [Description("A19D880F-05FC-4D3B-A006-743F0F84911E")]
      LinuxRaid,

      /// <summary>Linux Root (x86) partition.</summary>
      [Description("44479540-F297-41B2-9AF7-D131D5F0458A")]
      LinuxRootX86,

      /// <summary>Linux Root (x86-64) partition.</summary>
      [Description("4F68BCE3-E8CD-4DB1-96E7-FBCAF984B709")]
      LinuxRootX8664,

      /// <summary>Linux Root (32bit ARM) partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "bit")]
      [Description("69DAD710-2CE4-4E3C-B16C-21A1D49ABED3")]
      LinuxRoot32bitArm,

      /// <summary>Linux Root (64bit ARM/AArch64) partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "bit")]
      [Description("B921B045-1DF0-41C3-AF44-4C6F280D3FAE")]
      LinuxRoot64bitArm,

      /// <summary>Linux Swap partition.</summary>
      [Description("0657FD6D-A4AB-43C4-84E5-0933C84B4F4F")]
      LinuxSwap,

      /// <summary>Linux Logical Volume Manager(LVM) partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Lvm")]
      [Description("E6D6D379-F507-44C2-A23C-238F2A3DF928")]
      LinuxLvm,

      /// <summary>LinuxLinux /home partition.</summary>
      [Description("933AC7E1-2EB4-4F13-B844-0E14E2AEF915")]
      LinuxHome,

      /// <summary>Linux /srv (server data) partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Srv")]
      [Description("3B8F8425-20E0-4F3B-907F-1A25A76F98E8")]
      LinuxSrv,

      /// <summary>Linux Plain dm-crypt partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dm")]
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Dm")]
      [Description("7FFEC5C9-2D00-49B7-8941-3EA10A5586B7")]
      LinuxPlainDmCrypt,

      /// <summary>Linux LUKS partition.</summary>
      [Description("CA7D7CCB-63ED-4C53-861C-1742536059CC")]
      LinuxLuks,

      /// <summary>Linux Reserved partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1700:DoNotNameEnumValuesReserved")]
      [Description("8DA63339-0007-60C0-C436-083AC8230908")]
      LinuxReserved,

      #endregion // Linux


      #region FreeBSD

      /// <summary>FreeBSD Boot partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Bsd")]
      [Description("83BD6B9D-7F41-11DC-BE0B-001560B84F0F")]
      FreeBsdBoot,

      /// <summary>FreeBSD Data partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Bsd")]
      [Description("516E7CB4-6ECF-11D6-8FF8-00022D09712B")]
      FreeBsdData,

      /// <summary>FreeBSD Swap partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Bsd")]
      [Description("516E7CB5-6ECF-11D6-8FF8-00022D09712B")]
      FreeBsdSwap,

      /// <summary>FreeBSD Unix File System (UFS) partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Bsd")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ufs")]
      [Description("516E7CB6-6ECF-11D6-8FF8-00022D09712B")]
      FreeBsdUfs,

      /// <summary>FreeBSD Vinum Volume Manager partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Bsd")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Vvm")]
      [Description("516E7CB8-6ECF-11D6-8FF8-00022D09712B")]
      FreeBsdVvm,

      /// <summary>FreeBSD ZFS partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Bsd")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Zfs")]
      [Description("516E7CBA-6ECF-11D6-8FF8-00022D09712B")]
      FreeBsdZfs,

      #endregion // FreeBSD


      #region MacOS Darwin

      /// <summary>OSX Hierarchical File System Plus (HFS+) partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Os")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Hfs")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Os")]
      [Description("48465300-0000-11AA-AA11-00306543ECAC")]
      OsXHfs,

      /// <summary>OSX Apple APFS partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Os")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Apfs")]
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Os")]
      [Description("7C3457EF-0000-11AA-AA11-00306543ECAC")]
      OsXAppleApfs,

      /// <summary>OSX Apple UFS partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Os")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ufs")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Os")]
      [Description("55465300-0000-11AA-AA11-00306543ECAC")]
      OsXAppleUfs,

      /// <summary>OSX ZFS partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Zfs")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Os")]
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Os")]
      [Description("6A898CC3-1DD2-11B2-99A6-080020736631")]
      OsXZfs,

      /// <summary>OSX Apple RAID partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Os")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Os")]
      [Description("52414944-0000-11AA-AA11-00306543ECAC")]
      OsXAppleRaid,

      /// <summary>OSX Apple RAID offline partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Os")]
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Os")]
      [Description("52414944-5F4F-11AA-AA11-00306543ECAC")]
      OsXAppleRaidOffline,

      /// <summary>OSX Apple Boot (Recovery HD) partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Os")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Os")]
      [Description("426F6F74-0000-11AA-AA11-00306543ECAC")]
      OsXBootRecovery,

      /// <summary>OSX Apple Label partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Os")]
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Os")]
      [Description("C616265-6C00-11AA-AA11-00306543ECAC")]
      OsXLabel,

      /// <summary>OSX Apple TV Recovery partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Tv")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Os")]
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Tv")]
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Os")]
      [Description("5265636F-7665-11AA-AA11-00306543ECAC")]
      OsXTvRecovery,

      /// <summary>OSX Apple Core Storage (i.e. Lion FileVault) partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Os")]
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Os")]
      [Description("53746F72-6167-11AA-AA11-00306543ECAC")]
      OsXCoreStorage,

      /// <summary>OSX SoftRAID_Status partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Os")]
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Os")]
      [Description("B6FA30DA-92D2-4A9A-96F1-871EC6486200")]
      OsXSoftRaidStatus,

      /// <summary>OSX SoftRAID_Scratch partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Os")]
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Os")]
      [Description("2E313465-19B9-463F-8126-8A7993773801")]
      OsXSoftRaidScratch,

      /// <summary>OSX SoftRAID_Volume partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Os")]
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Os")]
      [Description("FA709C7E-65B1-4593-BFD5-E71D61DE9B02")]
      OsXSoftRaidVolume,

      /// <summary>OSX SoftRAID_Cache partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Os")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Os")]
      [Description("BBBA6DF5-F46F-4A89-8F59-8765B2727503")]
      OsXSoftRaidCache,

      #endregion // MacOS Darwin


      #region Solaris Illumos

      /// <summary>Solaris Boot partition.</summary>
      [Description("6A82CB45-1DD2-11B2-99A6-080020736631")]
      SolarisBoot,

      /// <summary>Solaris Root partition.</summary>
      [Description("6A85CF4D-1DD2-11B2-99A6-080020736631")]
      SolarisRoot,

      /// <summary>Solaris Swap partition.</summary>
      [Description("6A87C46F-1DD2-11B2-99A6-080020736631")]
      SolarisSwap,

      /// <summary>Solaris Backup partition.</summary>
      [Description("6A8B642B-1DD2-11B2-99A6-080020736631")]
      SolarisBackup,

      /// <summary>Solaris /usr partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Usr")]
      [Description("6A898CC3-1DD2-11B2-99A6-080020736631")]
      SolarisUsr,

      /// <summary>Solaris /var partition.</summary>
      [Description("6A8EF2E9-1DD2-11B2-99A6-080020736631")]
      SolarisVar,

      /// <summary>Solaris /home partition.</summary>
      [Description("6A90BA39-1DD2-11B2-99A6-080020736631")]
      SolarisHome,

      /// <summary>Solaris Alternate sector partition.</summary>
      [Description("6A9283A5-1DD2-11B2-99A6-080020736631")]
      SolarisAlternateSector,

      /// <summary>Solaris Reserved partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1700:DoNotNameEnumValuesReserved")]
      [Description("6A945A3B-1DD2-11B2-99A6-080020736631")]
      SolarisReserved1,

      /// <summary>Solaris Reserved partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1700:DoNotNameEnumValuesReserved")]
      [Description("6A9630D1-1DD2-11B2-99A6-080020736631")]
      SolarisReserved2,

      /// <summary>Solaris Reserved partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1700:DoNotNameEnumValuesReserved")]
      [Description("6A980767-1DD2-11B2-99A6-080020736631")]
      SolarisReserved3,

      /// <summary>Solaris Reserved partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1700:DoNotNameEnumValuesReserved")]
      [Description("6A96237F-1DD2-11B2-99A6-080020736631")]
      SolarisReserved4,

      /// <summary>Solaris Reserved partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1700:DoNotNameEnumValuesReserved")]
      [Description("6A8D2AC7-1DD2-11B2-99A6-080020736631")]
      SolarisReserved5,

      #endregion // Solaris Illumos


      #region NetBSD

      /// <summary>NetBSD Swap partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Bsd")]
      [Description("6A82CB45-1DD2-11B2-99A6-080020736631")]
      NetBsdSwap,

      /// <summary>NetBSD Fast File System (FFS) partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ffs")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Bsd")]
      [Description("49F48D5A-B10E-11DC-B99B-0019D1879648")]
      NetBsdFfs,

      /// <summary>NetBSD Log-Structured File System (LFS) partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Lfs")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Bsd")]
      [Description("49F48D82-B10E-11DC-B99B-0019D1879648")]
      NetBsdLfs,

      /// <summary>NetBSD RAID partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Bsd")]
      [Description("49F48DAA-B10E-11DC-B99B-0019D1879648")]
      NetBsdRaid,

      /// <summary>NetBSD Concatenated partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Bsd")]
      [Description("2DB519C4-B10F-11DC-B99B-0019D1879648")]
      NetBsdConcatenated,

      /// <summary>NetBSD Encrypted partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Bsd")]
      [Description("2DB519EC-B10F-11DC-B99B-0019D1879648")]
      NetBsdEncrypted,

      #endregion // NetBSD


      #region Chrome OS

      /// <summary>Chrome OS Kernel partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Chromse")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Os")]
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Os")]
      [Description("FE3A2A5D-4F32-41A7-B725-ACCC3285A309")]
      ChromseOsKernel,

      /// <summary>Chrome OS RootFS partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Os")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Os")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Fs")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Chromse")]
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Fs")]
      [Description("3CB8E202-3B7E-47DD-8A3C-7FF2A13CFCEC")]
      ChromseOsRootFs,

      /// <summary>Chrome OS (future use) partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Os")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Chromse")]
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Os")]
      [Description("2E0A753D-9E48-43B0-8337-B15192CB1B5E")]
      ChromseOsFutureUse,

      #endregion // Chrome OS


      #region Haiku

      /// <summary>Haiku Be File System (BFS) partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Bfs")]
      [Description("42465331-3BA3-10F1-802A-4861696B7521")]
      HaikuBfs,

      #endregion // Haiku


      #region MidnightBSD

      /// <summary>MidnightBSD Boot partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Bsd")]
      [Description("85D5E45E-237C-11E1-B4B3-E89A8F7FC3A7")]
      MidnightBsdBoot,

      /// <summary>MidnightBSD Data partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Bsd")]
      [Description("85D5E45A-237C-11E1-B4B3-E89A8F7FC3A7")]
      MidnightBsdData,

      /// <summary>MidnightBSD Swap partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Bsd")]
      [Description("85D5E45B-237C-11E1-B4B3-E89A8F7FC3A7")]
      MidnightBsdSwap,

      /// <summary>MidnightBSD Unix File System (UFS) partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Bsd")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ufs")]
      [Description("0394EF8B-237E-11E1-B4B3-E89A8F7FC3A7")]
      MidnightBsdUfs,

      /// <summary>MidnightBSD Vinum Volume Manager partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Bsd")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Vvm")]
      [Description("85D5E45C-237C-11E1-B4B3-E89A8F7FC3A7")]
      MidnightBsdVvm,

      /// <summary>MidnightBSD ZFS partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Bsd")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Zfs")]
      [Description("85D5E45D-237C-11E1-B4B3-E89A8F7FC3A7")]
      MidnightBsdZfs,

      #endregion // MidnightBSD


      #region Ceph

      /// <summary>Ceph journal partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ceph")]
      [Description("45B0969E-9B03-4F30-B4C6-B4B80CEFF106")]
      CephJournal,

      /// <summary>Ceph dm-crypt journal partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Dm")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ceph")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dm")]
      [Description("45B0969E-9B03-4F30-B4C6-5EC00CEFF106")]
      CephDmCryptJournal,

      /// <summary>Ceph OSD partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Osd")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ceph")]
      [Description("4FBD7E29-9D25-41B8-AFD0-062C0CEFF05D")]
      CephOsd,

      /// <summary>Ceph dm-crypt OSD partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Dm")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ceph")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dm")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Osd")]
      [Description("4FBD7E29-9D25-41B8-AFD0-5EC00CEFF05D")]
      CephDmCryptOsd,

      /// <summary>Ceph disk in creation partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ceph")]
      [Description("89C57F98-2FE5-4DC0-89C1-F3AD0CEFF2BE")]
      CephDiskInCreation,

      /// <summary>Ceph dm-crypt disk in creation partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dm")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ceph")]
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Dm")]
      [Description("89C57F98-2FE5-4DC0-89C1-5EC00CEFF2BE")]
      CephDmCryptDiskInCreation,

      /// <summary>Ceph block partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ceph")]
      [Description("CAFECAFE-9B03-4F30-B4C6-B4B80CEFF106")]
      CephBlock,

      /// <summary>Ceph block DB partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ceph")]
      [Description("30CD0809-C2B2-499C-8879-2D6B78529876")]
      CephBlockDb,

      /// <summary>Ceph block write-ahead log partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ceph")]
      [Description("5CE17FCE-4087-4169-B7FF-056CC58473F9")]
      CephBlockWriteAheadLog,

      /// <summary>Ceph lockbox for dm-crypt keys partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dm")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ceph")]
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Dm")]
      [Description("FB3AABF9-D25F-47CC-BF5E-721D1816496B")]
      CephDmCryptKeysLockbox,

      /// <summary>Ceph multipath OSD partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Osd")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ceph")]
      [Description("4FBD7E29-8AE0-4982-BF9D-5A8D867AF560")]
      CephMultipathOsd,

      /// <summary>Ceph multipath journal partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ceph")]
      [Description("45B0969E-8AE0-4982-BF9D-5A8D867AF560")]
      CephMultipathJournal,

      /// <summary>Ceph multipath block partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ceph")]
      [Description("CAFECAFE-8AE0-4982-BF9D-5A8D867AF560")]
      CephMultipathBlock1,

      /// <summary>Ceph multipath block partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ceph")]
      [Description("7F4A666A-16F3-47A2-8445-152EF4D03F6C")]
      CephMultipathBlock2,

      /// <summary>Ceph multipath block DB partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ceph")]
      [Description("EC6D6385-E346-45DC-BE91-DA2A7C8B3261")]
      CephMultipathBlockDb,

      /// <summary>Ceph multipath block write-ahead log partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ceph")]
      [Description("01B41E1B-002A-453C-9F17-88793989FF8F")]
      CephMultipathBlockWriteAheadLog,

      /// <summary>Ceph dm-crypt block partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dm")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ceph")]
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Dm")]
      [Description("CAFECAFE-9B03-4F30-B4C6-5EC00CEFF106")]
      CephDmCryptBlock,

      /// <summary>Ceph dm-crypt block DB partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dm")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ceph")]
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Dm")]
      [Description("93B0052D-02D9-4D8A-A43B-33A3EE4DFBC3")]
      CephDmCryptBlockDb,

      /// <summary>Ceph dm-crypt block write-ahead log partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dm")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ceph")]
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Dm")]
      [Description("306E8683-4FE2-4330-B7C0-00A917C16966")]
      CephDmCryptBlockWriteAheadLog,

      /// <summary>Ceph dm-crypt LUKS journal partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dm")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ceph")]
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Dm")]
      [Description("45B0969E-9B03-4F30-B4C6-35865CEFF106")]
      CephDmCryptLuksJournal,

      /// <summary>Ceph dm-crypt LUKS block partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ceph")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dm")]
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Dm")]
      [Description("CAFECAFE-9B03-4F30-B4C6-35865CEFF106")]
      CephDmCryptLuksBlock,

      /// <summary>Ceph dm-crypt LUKS block DB partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ceph")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dm")]
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Dm")]
      [Description("166418DA-C469-4022-ADF4-B30AFD37F176")]
      CephDmCryptLuksBlockDb,

      /// <summary>Ceph dm-crypt LUKS block write-ahead log partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ceph")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dm")]
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Dm")]
      [Description("166418DA-C469-4022-ADF4-B30AFD37F176")]
      CephDmCryptLuksBlockWriteAheadLog,

      /// <summary>Ceph dm-crypt LUKS OSD partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ceph")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Osd")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dm")]
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Dm")]
      [Description("166418DA-C469-4022-ADF4-B30AFD37F176")]
      CephDmCryptLuksOsd,

      #endregion // Ceph


      #region OpenBSD

      /// <summary>OpenBSD Data partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Bsd")]
      [Description("824CC7A0-36A8-11E3-890A-952519AD3F61")]
      OpenBsdData,

      #endregion // OpenBSD


      #region QNX

      /// <summary>QNX Power-safe (QNX6) file system partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Qnx")]
      [Description("CEF5A9AD-73BC-4601-89F3-CDEEEEE321A1")]
      Qnx6,

      #endregion // QNX


      #region Plan 9

      /// <summary>Plan 9 partition.</summary>
      [Description("C91818F9-8025-47AF-89D2-F030D7000C2C")]
      Plan9,

      #endregion // Plan 9


      #region VMware ESX

      /// <summary>VMware ESX vmkcore (coredump) partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Vm")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Esx")]
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Vm")]
      [Description("9D275380-40AD-11DB-BF97-000C2911D1B8")]
      VmWareEsxCore,

      /// <summary>VMware ESX Virtual Machine File System (VMFS) partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Vm")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Vmfs")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Esx")]
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Vm")]
      [Description("AA31E02A-400F-11DB-9590-000C2911D1B8")]
      VmWareEsxVmfs,

      /// <summary>VMware ESX Reserved partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Esx")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Vm")]
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Vm")]
      [SuppressMessage("Microsoft.Naming", "CA1700:DoNotNameEnumValuesReserved")]
      [Description("9198EFFC-31C0-11DB-8F78-000C2911D1B8")]
      VmWareEsxReserved,

      #endregion // VMware ESX


      #region Android-IA

      /// <summary>Android-IA Bootloader partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Bootloader")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ia")]
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Ia")]
      [Description("2568845D-2332-4675-BC39-8FA5A4748D15")]
      AndroidIaBootloader,

      /// <summary>Android-IA Bootloader2 partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Bootloader")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ia")]
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Ia")]
      [Description("114EAFFE-1552-4022-B26E-9B053604CF84")]
      AndroidIaBootloader2,

      /// <summary>Android-IA Boot partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ia")]
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Ia")]
      [Description("49A4D17F-93A3-45C1-A0DE-F50B2EBE2599")]
      AndroidIaBoot,

      /// <summary>Android-IA Recovery partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ia")]
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Ia")]
      [Description("4177C722-9E92-4AAB-8644-43502BFD5506")]
      AndroidIaRecovery,

      /// <summary>Android-IA Misc partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ia")]
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Ia")]
      [Description("EF32A33B-A409-486C-9141-9FFB711F6266")]
      AndroidIaMisc,

      /// <summary>Android-IA Metadata partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ia")]
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Ia")]
      [Description("20AC26BE-20B7-11E3-84C5-6CFDB94711E9")]
      AndroidIaMetadata,

      /// <summary>Android-IA System partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ia")]
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Ia")]
      [Description("38F428E6-D326-425D-9140-6E0EA133647C")]
      AndroidIaSystem,

      /// <summary>Android-IA Cache partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ia")]
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Ia")]
      [Description("A893EF21-E428-470A-9E55-0668FD91A2D9")]
      AndroidIaCache,

      /// <summary>Android-IA Data partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ia")]
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Ia")]
      [Description("DC76DDA9-5AC1-491C-AF42-A82591580C0D")]
      AndroidIaData,

      /// <summary>Android-IA Persistent partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ia")]
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Ia")]
      [Description("EBC597D0-2053-4B15-8B64-E0AAC75F4DB1")]
      AndroidIaPersistent,

      /// <summary>Android-IA Vendor partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ia")]
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Ia")]
      [Description("C5A0AEEC-13EA-11E5-A1B1-001E67CA0C3C")]
      AndroidIaVendor,

      /// <summary>Android-IA Config partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ia")]
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Ia")]
      [Description("BD59408B-4514-490D-BF12-9878D963F378")]
      AndroidIaConfig,

      /// <summary>Android-IA Factory partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ia")]
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Ia")]
      [Description("8F68CC74-C5E5-48DA-BE91-A0C8C15E9C80")]
      AndroidIaFactory,

      /// <summary>Android-IA Factory (Alternate) partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ia")]
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Ia")]
      [Description("9FDAA6EF-4B3F-40D2-BA8D-BFF16BFB887B")]
      AndroidIaFactoryAlternate,

      /// <summary>Android-IA Tertiary partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ia")]
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Ia")]
      [Description("767941D0-2085-11E3-AD3B-6CFDB94711E9")]
      AndroidIaTertiary,

      /// <summary>Android-IA OEM partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Oem")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ia")]
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Ia")]
      [Description("AC6D7924-EB71-4DF8-B48D-E267B27148FF")]
      AndroidIaOem,

      #endregion // Android-IA


      #region ONIE

      /// <summary>Open Network Install Environment (ONIE) Boot partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Onie")]
      [Description("7412F7D5-A156-4B13-81DC-867174929325")]
      OnieBoot,

      /// <summary>Open Network Install Environment (ONIE) Config partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Onie")]
      [Description("D4E6E2CD-4469-46F3-B5CB-1BFF57AFC149")]
      OnieConfig,

      #endregion // ONIE


      #region PowerPC

      /// <summary>PowerPC PReP Boot partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Pc")]
      [Description("9E1A2D38-C612-4316-AA26-8B49521E5A8B")]
      PowerPcPrepBoot,
      
      #endregion // PowerPC


      #region freedesktop.org OSes (Linux, etc.)

      /// <summary>Shared boot loader configuration partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Bootloader")]
      [Description("BC13C2FF-59E6-4262-A352-B275FD6F7172")]
      SharedBootloaderConfiguration,

      #endregion // freedesktop.org OSes (Linux, etc.)


      #region Atari TOS

      /// <summary>Atari TOS Basic data (GEM, BGM, F32) partition.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Tos")]
      [Description("734E5AFE-F61A-11E6-BC64-92361F002671")]
      AtariTosBasicData

      #endregion // Atari TOS
   }
}
