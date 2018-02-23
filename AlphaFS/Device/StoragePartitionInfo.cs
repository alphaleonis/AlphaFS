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
   [Serializable]
   [SecurityCritical]
   public sealed class StoragePartitionInfo
   {
      #region Constructors

      /// <summary>Initializes a StoragePartitionInfo instance.</summary>
      public StoragePartitionInfo()
      {
         DeviceNumber = -1;

         PartitionNumber = -1;

         PartitionStyle = PartitionStyle.Raw;
      }


      internal StoragePartitionInfo(NativeMethods.STORAGE_DEVICE_NUMBER device, NativeMethods.PARTITION_INFORMATION_EX partition) : this()
      {
         DeviceNumber = device.DeviceNumber;

         PartitionNumber = (int) partition.PartitionNumber;

         PartitionStyle = (PartitionStyle) partition.PartitionStyle;

         
         RewritePartition = partition.RewritePartition;

         TotalSize = (long) partition.PartitionLength;

         GptPartitionInfo = new StoragePartitionInfoGpt(partition.Gpt);

         MbrPartitionInfo = new StoragePartitionInfoMbr(partition.Mbr);
      }

      #endregion // Constructors


      #region Properties

      /// <summary>The device number of the storage partition, starting at 0.</summary>
      public int DeviceNumber { get; internal set; }


      /// <summary>Contains GUID partition table (GPT) partition information.</summary>
      public StoragePartitionInfoGpt GptPartitionInfo { get; internal set; }

      
      /// <summary>Contains partition information specific to master boot record (MBR) disks.</summary>
      public StoragePartitionInfoMbr MbrPartitionInfo { get; internal set; }

      
      /// <summary>The storage partition number, starting at 1.</summary>
      public int PartitionNumber { get; internal set; }


      /// <summary>The format of the partition. For a list of values, see <see cref="PartitionStyle"/>.</summary>
      public PartitionStyle PartitionStyle { get; internal set; }


      /// <summary>The rewritable status of the storage partition.</summary>
      public bool RewritePartition { get; internal set; }


      /// <summary>The total size of the storage partition.</summary>
      public long TotalSize { get; internal set; }


      /// <summary>The total size of the physical drive, formatted as a unit size.</summary>
      public string TotalSizeUnitSize
      {
         get { return Utils.UnitSizeToText(TotalSize); }
      }

      #endregion // Properties


      #region Methods

      /// <summary>Returns storage device as: "VendorId ProductId DeviceType DeviceNumber:PartitionNumber".</summary>
      /// <returns>A string that represents this instance.</returns>
      public override string ToString()
      {
         return string.Format(CultureInfo.CurrentCulture, "{0}:{1} {2} {3}",

            DeviceNumber.ToString(), PartitionNumber.ToString(), PartitionStyle.ToString(), TotalSizeUnitSize).Trim();
      }


      /// <summary>Determines whether the specified Object is equal to the current Object.</summary>
      /// <param name="obj">Another object to compare to.</param>
      /// <returns><see langword="true"/> if the specified Object is equal to the current Object; otherwise, <see langword="false"/>.</returns>
      public override bool Equals(object obj)
      {
         if (null == obj || GetType() != obj.GetType())
            return false;

         var other = obj as StoragePartitionInfo;

         return null != other &&
                other.DeviceNumber == DeviceNumber &&
                other.PartitionNumber == PartitionNumber &&
                other.PartitionStyle == PartitionStyle &&
                other.TotalSize == TotalSize;
      }


      /// <summary>Serves as a hash function for a particular type.</summary>
      /// <returns>A hash code for the current Object.</returns>
      public override int GetHashCode()
      {
         unchecked
         {
            return DeviceNumber + PartitionNumber + PartitionStyle.GetHashCode() + TotalSize.GetHashCode();
         }
      }


      /// <summary>Implements the operator ==</summary>
      /// <param name="left">A.</param>
      /// <param name="right">B.</param>
      /// <returns>The result of the operator.</returns>
      public static bool operator ==(StoragePartitionInfo left, StoragePartitionInfo right)
      {
         return ReferenceEquals(left, null) && ReferenceEquals(right, null) || !ReferenceEquals(left, null) && !ReferenceEquals(right, null) && left.Equals(right);
      }


      /// <summary>Implements the operator !=</summary>
      /// <param name="left">A.</param>
      /// <param name="right">B.</param>
      /// <returns>The result of the operator.</returns>
      public static bool operator !=(StoragePartitionInfo left, StoragePartitionInfo right)
      {
         return !(left == right);
      }

      #endregion // Methods
   }
}
