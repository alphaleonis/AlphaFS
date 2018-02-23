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
      UnusedGuid,

      /// <summary>PARTITION_BASIC_DATA_GUID: The data partition type that is created and recognized by Windows.
      /// <remarks>
      /// <para>This value can be set only for basic disks, with one exception.</para>
      /// <para>Only partitions of this type can be assigned drive letters, receive volume GUID paths, host mounted folders (also called volume mount points), and be enumerated by calls to FindFirstVolume and FindNextVolume.</para>
      /// </remarks>
      /// </summary>
      [Description("EBD0A0A2-B9E5-4433-87C0-68B6B72699C7")]
      BasicDataGuid,

      /// <summary>PARTITION_LDM_DATA_GUID: The partition is an LDM data partition on a dynamic disk.
      /// <remarks>This value can be set only for dynamic disks.</remarks>
      /// </summary>
      [Description("AF9B60A0-1431-4F62-BC68-3311714A69AD")]
      LdmDataGuid,

      /// <summary>PARTITION_LDM_METADATA_GUID: The partition is a Logical Disk Manager (LDM) metadata partition on a dynamic disk.
      /// <remarks>This value can be set only for dynamic disks.</remarks>
      /// </summary>
      [Description("5808C8AA-7E8F-42E0-85D2-E1E90434CFB3")]
      LdmMetaDataGuid,

      /// <summary>PARTITION_MSFT_RECOVERY_GUID: The partition is a Microsoft recovery partition.
      /// <remarks>This value can be set for basic and dynamic disks.</remarks>
      /// </summary>
      [Description("DE94BBA4-06D1-4D40-A16A-BFD50179D6AC")]
      MsftRecoveryguid,

      /// <summary>PARTITION_MSFT_RESERVED_GUID: The partition is a Microsoft reserved partition.
      /// <remarks>This value can be set for basic and dynamic disks.</remarks>
      /// </summary>
      [Description("E3C9E316-0B5C-4DB8-817D-F92DF00215AE")]
      MsftReservedGuid,

      /// <summary>PARTITION_SYSTEM_GUID: The partition is an EFI system partition.
      /// <remarks>This value can be set for basic and dynamic disks.</remarks>
      /// </summary>
      [Description("C12A7328-F81F-11D2-BA4B-00A0C93EC93B")]
      SystemGuid
   }


   ///// <summary>A wrapper class which represents the possible media types for DirectShow. Since a MediaType is a Guid, a simple enum couldn't be used.</summary>
   //internal class PartitionTypeGuid : AbstractGuidEnum<PartitionType>
   //{
   //   /// <summary> Creates a new PartitionTypeGuid instance, wrapping the given GUID.</summary>
   //   /// <param name="guid">The GUID to wrap.</param>
   //   public PartitionTypeGuid(Guid guid) : base(guid)
   //   {
   //   }


   //   public PartitionTypeGuid(PartitionType partitionType) : base(partitionType)
   //   {
   //   }


   //   protected override void FillTypeList(Dictionary<Guid, PartitionType> typeList)
   //   {
   //      typeList.Add(NativeMethods.PartitionType.PARTITION_ENTRY_UNUSED_GUID, PartitionType.UnusedGuid);

   //      typeList.Add(NativeMethods.PartitionType.PARTITION_BASIC_DATA_GUID, PartitionType.BasicDataGuid);

   //      typeList.Add(NativeMethods.PartitionType.PARTITION_LDM_DATA_GUID, PartitionType.LdmDataGuid);

   //      typeList.Add(NativeMethods.PartitionType.PARTITION_LDM_METADATA_GUID, PartitionType.LdmMetaDataGuid);

   //      typeList.Add(NativeMethods.PartitionType.PARTITION_MSFT_RECOVERY_GUID, PartitionType.MsftRecoveryguid);

   //      typeList.Add(NativeMethods.PartitionType.PARTITION_MSFT_RESERVED_GUID, PartitionType.MsftReservedGuid);

   //      typeList.Add(NativeMethods.PartitionType.PARTITION_SYSTEM_GUID, PartitionType.SystemGuid);
   //   }
   //}
   

   ///// <summary>An abstract class to represent a set of GUIDs as an enum.</summary>
   ///// <typeparam name="T">An enum to use for the type to expose.</typeparam>
   //internal abstract class AbstractGuidEnum<T>
   //{
   //   /// <summary>The internal GUID.</summary>
   //   public Guid Guid { get; private set; }


   //   /// <summary>The enum type for this GUID.</summary>
   //   public T Type { get; private set; }


   //   /// <summary>Specifies whether the GUID is known as a specific type or not.</summary>
   //   public bool IsKnownType { get; private set; }


   //   /// <summary>List matching GUIDs with the enum types.</summary>
   //   private static Dictionary<Guid, T> _typeList;


   //   /// <summary>Create a new GUID enum wrapping the given GUID.</summary>
   //   /// <param name="guid"></param>
   //   protected AbstractGuidEnum(Guid guid)
   //   {
   //      Guid = guid;

   //      IsKnownType = IsGuidKnownType(guid);

   //      if (IsKnownType)
   //         Type = GetType(guid);
   //   }


   //   /// <summary>Create a new GUID enum for the specified type.</summary>
   //   /// <param name="type"></param>
   //   protected AbstractGuidEnum(T type)
   //   {
   //      Type = type;
   //      Guid = GetGuid(type);
   //   }


   //   /// <summary>Call to make sure the list of types is initialized.</summary>
   //   private void InitializeTypes()
   //   {
   //      if (null == _typeList)
   //      {
   //         _typeList = new Dictionary<Guid, T>();

   //         FillTypeList(_typeList);
   //      }
   //   }


   //   public bool IsGuidKnownType(Guid guid)
   //   {
   //      InitializeTypes();

   //      return _typeList.ContainsKey(guid);
   //   }


   //   /// <summary>Returns the type for a given GUID.</summary>
   //   /// <param name="type">The GUID to get the type for.</param>
   //   /// <returns>The type for the given GUID.</returns>
   //   public T GetType(Guid type)
   //   {
   //      InitializeTypes();

   //      if (!_typeList.ContainsKey(type))
   //         throw new ArgumentException("No type is defined for the given GUID.", "type");

   //      return _typeList[type];
   //   }


   //   /// <summary>Return the guid for a given type.</summary>
   //   /// <param name="type">The Type to get the Guid for.</param>
   //   /// <returns>The Guid for the given type.</returns>
   //   /// <exception cref="InvalidCastException">Thrown when no Guid exists for the given type.</exception>
   //   private Guid GetGuid(T type)
   //   {
   //      InitializeTypes();

         
   //      try
   //      {
   //         return _typeList.Keys.First(guid => _typeList[guid].Equals(type));
   //      }
   //      catch (InvalidOperationException)
   //      {
   //         throw new InvalidCastException("No Guid exists for the given type.");
   //      }
   //   }


   //   public override bool Equals(object obj)
   //   {
   //      if (!(obj is AbstractGuidEnum<T>))
   //         return false;

   //      AbstractGuidEnum<T> guidObj = obj as AbstractGuidEnum<T>;

   //      return Guid.Equals(guidObj.Guid);
   //   }


   //   public override int GetHashCode()
   //   {
   //      unchecked
   //      {
   //         return Guid.GetHashCode();
   //      }
   //   }


   //   /// <summary>Fill up the list which matches GUIDs to the desired enum types.</summary>
   //   /// <param name="typeList"></param>
   //   protected abstract void FillTypeList(Dictionary<Guid, T> typeList);
   //}
}
