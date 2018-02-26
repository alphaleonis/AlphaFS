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

namespace Alphaleonis.Win32.Filesystem
{
   public enum PartitionType
   {
      /// <summary>PARTITION_ENTRY_UNUSED_GUID: There is no partition.
      /// <remarks>This value can be set for basic and dynamic disks.</remarks>
      /// </summary>
      [Description("00000000-0000-0000-0000-000000000000")]
      Unused,

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
      [Description("AF9B60A0-1431-4F62-BC68-3311714A69AD")]
      LdmData,

      /// <summary>PARTITION_LDM_METADATA_GUID: The partition is a Logical Disk Manager (LDM) metadata partition on a dynamic disk.
      /// <remarks>This value can be set only for dynamic disks.</remarks>
      /// </summary>
      [Description("5808C8AA-7E8F-42E0-85D2-E1E90434CFB3")]
      LdmMetaData,

      /// <summary>PARTITION_MSFT_RECOVERY_GUID: The partition is a Microsoft recovery partition.
      /// <remarks>This value can be set for basic and dynamic disks.</remarks>
      /// </summary>
      [Description("DE94BBA4-06D1-4D40-A16A-BFD50179D6AC")]
      MsftRecovery,

      /// <summary>PARTITION_MSFT_RESERVED_GUID: The partition is a Microsoft reserved partition.
      /// <remarks>This value can be set for basic and dynamic disks.</remarks>
      /// </summary>
      [Description("E3C9E316-0B5C-4DB8-817D-F92DF00215AE")]
      MsftReserved,

      /// <summary>PARTITION_SYSTEM_GUID: The partition is an EFI system partition.
      /// <remarks>This value can be set for basic and dynamic disks.</remarks>
      /// </summary>
      [Description("C12A7328-F81F-11D2-BA4B-00A0C93EC93B")]
      System
   }
}
