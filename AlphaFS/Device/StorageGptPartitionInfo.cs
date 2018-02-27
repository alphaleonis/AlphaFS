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
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Security;

namespace Alphaleonis.Win32.Filesystem
{
   /// <summary>Provides access to GPT partition information of a storage device.</summary>
   [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Gpt"), Serializable]
   [SecurityCritical]
   public sealed class StorageGptPartitionInfo
   {
      #region Private Fields

      private ulong _partitionLength;
      private ulong _startingOffset;
      private static PartitionType[] _partitionTypes;

      #endregion // Private Fields


      #region Constructors

      /// <summary>Initializes a StorageGptPartitionInfo instance.</summary>
      public StorageGptPartitionInfo()
      {
         if (null == _partitionTypes)
            _partitionTypes = Utils.EnumToArray<PartitionType>();

         PartitionNumber = -1;
      }
      

      internal StorageGptPartitionInfo(NativeMethods.PARTITION_INFORMATION_EX partition) : this()
      {
         _partitionLength = partition.PartitionLength;

         _startingOffset = partition.StartingOffset;

         PartitionNumber = (int) partition.PartitionNumber;
         
         RewritePartition = partition.RewritePartition;


         


         var gptPartition = partition.Gpt;
         
         Attributes = (EfiPartitionAttributes) gptPartition.Attributes;

         

         Description = gptPartition.Name.Trim();

         PartitionId = gptPartition.PartitionId;


         foreach (var guid in _partitionTypes)
            if (gptPartition.PartitionType.Equals(new Guid(Utils.GetEnumDescription(guid))))
            {
               PartitionType = guid;
               break;
            }
      }

      #endregion // Constructors


      #region Properties


      /// <summary>The Extensible Firmware Interface (EFI) attributes of the partition.</summary>
      public EfiPartitionAttributes Attributes { get; internal set; }


      /// <summary>The description of the partition.</summary>
      public string Description { get; internal set; }


      /// <summary>The GUID of the partition.</summary>
      public Guid PartitionId { get; internal set; }


      /// <summary>The starting offset of the partition.</summary>
      public long PartitionLength
      {
         get
         {
            unchecked
            {
               return (long) _partitionLength;
            }
         }

         internal set
         {
            unchecked
            {
               _partitionLength = (ulong) value;
            }
         }
      }


      /// <summary>The starting offset of the partition, formatted as a unit size.</summary>
      public string PartitionLengthUnitSize
      {
         get { return Utils.UnitSizeToText(PartitionLength); }
      }


      /// <summary>The storage partition number, starting at 1.</summary>
      public int PartitionNumber { get; internal set; }


      /// <summary>The the partition type. Each partition type that the EFI specification supports is identified by its own GUID, which is published by the developer of the partition.</summary>
      public PartitionType PartitionType { get; internal set; }


      /// <summary>The rewritable status of the storage partition.</summary>
      public bool RewritePartition { get; internal set; }


      /// <summary>The starting offset of the partition.</summary>
      public long StartingOffset
      {
         get
         {
            unchecked
            {
               return (long) _startingOffset;
            }
         }

         internal set
         {
            unchecked
            {
               _startingOffset = (ulong) value;
            }
         }
      }

      #endregion // Properties


      #region Methods

      /// <summary>Returns storage device as: "VendorId ProductId DeviceType DeviceNumber:PartitionNumber".</summary>
      /// <returns>A string that represents this instance.</returns>
      public override string ToString()
      {
         return string.Format(CultureInfo.CurrentCulture, "{0} {1}", Description, Attributes.ToString()).Trim();
      }


      /// <summary>Determines whether the specified Object is equal to the current Object.</summary>
      /// <param name="obj">Another object to compare to.</param>
      /// <returns><see langword="true"/> if the specified Object is equal to the current Object; otherwise, <see langword="false"/>.</returns>
      public override bool Equals(object obj)
      {
         if (null == obj || GetType() != obj.GetType())
            return false;

         var other = obj as StorageGptPartitionInfo;

         return null != other &&
                other.Attributes == Attributes &&
                other.PartitionId == PartitionId &&
                other.Description == Description &&
                other.PartitionType == PartitionType;
      }


      /// <summary>Serves as a hash function for a particular type.</summary>
      /// <returns>A hash code for the current Object.</returns>
      public override int GetHashCode()
      {
         unchecked
         {
            return Attributes.GetHashCode() + PartitionId.GetHashCode() + Description.GetHashCode() + PartitionType.GetHashCode();
         }
      }


      /// <summary>Implements the operator ==</summary>
      /// <param name="left">A.</param>
      /// <param name="right">B.</param>
      /// <returns>The result of the operator.</returns>
      public static bool operator ==(StorageGptPartitionInfo left, StorageGptPartitionInfo right)
      {
         return ReferenceEquals(left, null) && ReferenceEquals(right, null) || !ReferenceEquals(left, null) && !ReferenceEquals(right, null) && left.Equals(right);
      }


      /// <summary>Implements the operator !=</summary>
      /// <param name="left">A.</param>
      /// <param name="right">B.</param>
      /// <returns>The result of the operator.</returns>
      public static bool operator !=(StorageGptPartitionInfo left, StorageGptPartitionInfo right)
      {
         return !(left == right);
      }

      #endregion // Methods
   }
}
