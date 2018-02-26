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
using System.Globalization;
using System.Security;

namespace Alphaleonis.Win32.Filesystem
{
   /// <summary>Provides access to MBR partition information of a storage device.</summary>
   [Serializable]
   [SecurityCritical]
   public sealed class StorageMbrPartitionInfo
   {
      #region Private Fields

      private ulong _partitionLength;
      private ulong _startingOffset;

      #endregion // Private Fields


      #region Constructors

      /// <summary>Initializes a StorageMbrPartitionInfo instance.</summary>
      public StorageMbrPartitionInfo()
      {
         HiddenSectors = -1;

         PartitionNumber = -1;
      }

      

      internal StorageMbrPartitionInfo(NativeMethods.PARTITION_INFORMATION_EX partition) : this()
      {
         _partitionLength = partition.PartitionLength;

         _startingOffset = partition.StartingOffset;
         
         PartitionNumber = (int) partition.PartitionNumber;
         
         RewritePartition = partition.RewritePartition;
         

         var mbrPartition = partition.Mbr;
         
         BootIndicator = mbrPartition.BootIndicator;

         HiddenSectors = (int) mbrPartition.HiddenSectors;

         RecognizedPartition = mbrPartition.RecognizedPartition;

         PartitionType = mbrPartition.PartitionType;
      }

      #endregion // Constructors


      #region Properties

      /// <summary><see langword="true"/> if the partition is a boot partition.</summary>
      public bool BootIndicator { get; internal set; }


      /// <summary>The number of hidden sectors to be allocated when the partition table is created.</summary>
      public int HiddenSectors { get; internal set; }


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


      /// <summary>The type of the partition.</summary>
      public DiskPartitionTypes PartitionType { get; internal set; }


      /// <summary><see langword="true"/> if the partition is of a recognized type.</summary>
      public bool RecognizedPartition { get; internal set; }


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
         return string.Format(CultureInfo.CurrentCulture, "BootIndicator {0}, Type: {1}, RecognizedPartition: {2}", BootIndicator.ToString(), PartitionType.ToString(), RecognizedPartition.ToString()).Trim();
      }


      /// <summary>Determines whether the specified Object is equal to the current Object.</summary>
      /// <param name="obj">Another object to compare to.</param>
      /// <returns><see langword="true"/> if the specified Object is equal to the current Object; otherwise, <see langword="false"/>.</returns>
      public override bool Equals(object obj)
      {
         if (null == obj || GetType() != obj.GetType())
            return false;

         var other = obj as StorageMbrPartitionInfo;

         return null != other &&
                other.BootIndicator == BootIndicator &&
                other.HiddenSectors == HiddenSectors &&
                other.RecognizedPartition == RecognizedPartition &&
                other.PartitionType == PartitionType;
      }


      /// <summary>Serves as a hash function for a particular type.</summary>
      /// <returns>A hash code for the current Object.</returns>
      public override int GetHashCode()
      {
         unchecked
         {
            return BootIndicator.GetHashCode() + HiddenSectors.GetHashCode() + RecognizedPartition.GetHashCode() + PartitionType.GetHashCode();
         }
      }


      /// <summary>Implements the operator ==</summary>
      /// <param name="left">A.</param>
      /// <param name="right">B.</param>
      /// <returns>The result of the operator.</returns>
      public static bool operator ==(StorageMbrPartitionInfo left, StorageMbrPartitionInfo right)
      {
         return ReferenceEquals(left, null) && ReferenceEquals(right, null) || !ReferenceEquals(left, null) && !ReferenceEquals(right, null) && left.Equals(right);
      }


      /// <summary>Implements the operator !=</summary>
      /// <param name="left">A.</param>
      /// <param name="right">B.</param>
      /// <returns>The result of the operator.</returns>
      public static bool operator !=(StorageMbrPartitionInfo left, StorageMbrPartitionInfo right)
      {
         return !(left == right);
      }

      #endregion // Methods
   }
}
