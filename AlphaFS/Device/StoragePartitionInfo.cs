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

using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Security;

namespace Alphaleonis.Win32.Filesystem
{
   /// <summary>Provides access to partition information of a storage device.</summary>
   [Serializable]
   [SecurityCritical]
   public sealed class StoragePartitionInfo
   {
      #region Private Fields

      private ulong _gptStartingUsableOffset;

      #endregion // Private Fields


      #region Constructors

      /// <summary>Initializes a StoragePartitionInfo instance.</summary>
      public StoragePartitionInfo()
      {
         DeviceNumber = -1;

         PartitionCount = -1;

         PartitionStyle = PartitionStyle.Raw;
      }


      internal StoragePartitionInfo(NativeMethods.DISK_GEOMETRY_EX disk, NativeMethods.DRIVE_LAYOUT_INFORMATION_EX drive, NativeMethods.PARTITION_INFORMATION_EX[] partitions) : this()
      {
         MbrSignature = disk.PartitionInformation.MbrSignature;

         GptDiskId = disk.PartitionInformation.DiskId;

         MediaType = (StorageMediaType) disk.Geometry.MediaType;
         
         PartitionStyle = (PartitionStyle) disk.PartitionInformation.PartitionStyle;

         TotalSize = disk.DiskSize;


         PartitionCount = (int) drive.PartitionCount;


         switch (PartitionStyle)
         {
            case PartitionStyle.Gpt:
               GptMaxPartitionCount = (int) drive.Gpt.MaxPartitionCount;

               _gptStartingUsableOffset = drive.Gpt.StartingUsableOffset;

               
               GptPartitionInfo = new Collection<StorageGptPartitionInfo>();

               var partitionTypes = Utils.EnumToArray<PartitionType>();

               for (var i = 0; i <= PartitionCount - 1; i++)
                  GptPartitionInfo.Add(new StorageGptPartitionInfo(partitions[i], partitionTypes));
               
               break;


            case PartitionStyle.Mbr:
               for (var i = 0; i <= PartitionCount - 1; i++)
               {
                  var partition = partitions[i];

                  // MSDN: PartitionCount: On hard disks with the MBR layout, this value will always be a multiple of 4.
                  // Any partitions that are actually unused will have a partition type of PARTITION_ENTRY_UNUSED (0).

                  if (partition.Mbr.PartitionType == (NativeMethods.DiskPartitionType) DiskPartitionType.EntryUnused)
                     continue;


                  if (null == MbrPartitionInfo)
                     MbrPartitionInfo = new Collection<StorageMbrPartitionInfo>();

                  MbrPartitionInfo.Add(new StorageMbrPartitionInfo(partition));
               }


               // Update to reflect the real number of used partition entries.
               PartitionCount = MbrPartitionInfo.Count;

               break;
         }
      }

      #endregion // Constructors


      #region Properties
      
      /// <summary>The GUID of the disk.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Gpt")]
      public Guid GptDiskId { get; internal set; }
      

      /// <summary>The device number of the storage partition, starting at 0.</summary>
      public int DeviceNumber { get; internal set; }


      /// <summary>The maximum number of partitions that can be defined in the usable block.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Gpt")]
      public int GptMaxPartitionCount { get; internal set; }


      /// <summary>Contains GUID partition table (GPT) partition information.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Gpt")]
      public Collection<StorageGptPartitionInfo> GptPartitionInfo { get; internal set; }


      /// <summary>The starting byte offset of the first usable block.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Gpt")]
      public long GptStartingUsableOffset
      {
         get
         {
            unchecked
            {
               return (long) _gptStartingUsableOffset;
            }
         }

         internal set
         {
            unchecked
            {
               _gptStartingUsableOffset = (ulong) value;
            }
         }
      }
      

      ///// <summary>The size of the usable blocks on the disk, in bytes.</summary>
      //public long GptUsableLength
      //{
      //   get
      //   {
      //      unchecked
      //      {
      //         return (long) _gptUsableLength;
      //      }
      //   }

      //   internal set
      //   {
      //      unchecked
      //      {
      //         _gptUsableLength = (ulong) value;
      //      }
      //   }
      //}
      

      ///// <summary>(Only applicable to GPT partition) The size of the usable blocks on the disk in bytes, formatted as a unit size.</summary>
      //public string GptUsableLengthUnitSize
      //{
      //   get { return Utils.UnitSizeToText(GptUsableLength); }
      //}
      

      /// <summary>Contains partition information specific to master boot record (MBR) disks.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Mbr")]
      public Collection<StorageMbrPartitionInfo> MbrPartitionInfo { get; internal set; }


      /// <summary>The MBR signature of the drive.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Mbr")]
      public long MbrSignature { get; internal set; }


      /// <summary>The media type of the storage partition.</summary>
      public StorageMediaType MediaType { get; internal set; }


      /// <summary>The number of partitions on the drive.</summary>
      public int PartitionCount { get; internal set; }


      /// <summary>The format of the partition. For a list of values, see <see cref="Filesystem.PartitionStyle"/>.</summary>
      public PartitionStyle PartitionStyle { get; internal set; }


      /// <summary>The total size of the storage partition.</summary>
      public long TotalSize { get; internal set; }


      /// <summary>The total size of the storage partition, formatted as a unit size.</summary>
      public string TotalSizeUnitSize
      {
         get { return Utils.UnitSizeToText(TotalSize); }
      }


      #endregion // Properties


      //#region Methods

      ///// <summary>Returns storage device as: "VendorId ProductId DeviceType DeviceNumber:PartitionNumber".</summary>
      ///// <returns>A string that represents this instance.</returns>
      //public override string ToString()
      //{
      //   return string.Format(CultureInfo.CurrentCulture, "{0}:{1} {2} {3}",

      //      DeviceNumber.ToString(CultureInfo.InvariantCulture), PartitionNumber.ToString(CultureInfo.InvariantCulture), PartitionStyle.ToString(), TotalSizeUnitSize).Trim();
      //}


      ///// <summary>Determines whether the specified Object is equal to the current Object.</summary>
      ///// <param name="obj">Another object to compare to.</param>
      ///// <returns><see langword="true"/> if the specified Object is equal to the current Object; otherwise, <see langword="false"/>.</returns>
      //public override bool Equals(object obj)
      //{
      //   if (null == obj || GetType() != obj.GetType())
      //      return false;

      //   var other = obj as StoragePartitionInfo;

      //   return null != other &&
      //          other.DeviceNumber == DeviceNumber &&
      //          other.PartitionNumber == PartitionNumber &&
      //          other.PartitionStyle == PartitionStyle &&
      //          other.TotalSize == TotalSize;
      //}


      ///// <summary>Serves as a hash function for a particular type.</summary>
      ///// <returns>A hash code for the current Object.</returns>
      //public override int GetHashCode()
      //{
      //   unchecked
      //   {
      //      return PartitionCount + PartitionStyle.GetHashCode() + TotalSize.GetHashCode();
      //   }
      //}


      ///// <summary>Implements the operator ==</summary>
      ///// <param name="left">A.</param>
      ///// <param name="right">B.</param>
      ///// <returns>The result of the operator.</returns>
      //public static bool operator ==(StoragePartitionInfo left, StoragePartitionInfo right)
      //{
      //   return ReferenceEquals(left, null) && ReferenceEquals(right, null) || !ReferenceEquals(left, null) && !ReferenceEquals(right, null) && left.Equals(right);
      //}


      ///// <summary>Implements the operator !=</summary>
      ///// <param name="left">A.</param>
      ///// <param name="right">B.</param>
      ///// <returns>The result of the operator.</returns>
      //public static bool operator !=(StoragePartitionInfo left, StoragePartitionInfo right)
      //{
      //   return !(left == right);
      //}

      //#endregion // Methods
   }
}
