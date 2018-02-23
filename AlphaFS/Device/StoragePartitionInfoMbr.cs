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
   public sealed class StoragePartitionInfoMbr
   {
      #region Constructors

      /// <summary>Initializes a StoragePartitionInfoMbr instance.</summary>
      public StoragePartitionInfoMbr()
      {
      }


      internal StoragePartitionInfoMbr(NativeMethods.PARTITION_INFORMATION_MBR gptPartition) : this()
      {
         BootIndicator = gptPartition.BootIndicator;
         
         HiddenSectors = (int) gptPartition.HiddenSectors;

         RecognizedPartition = gptPartition.RecognizedPartition;

         Type = gptPartition.PartitionType;
      }

      #endregion // Constructors


      #region Properties

      /// <summary><see langword="true"/> if the partition is a boot partition.</summary>
      public bool BootIndicator { get; internal set; }


      /// <summary>The number of hidden sectors to be allocated when the partition table is created.</summary>
      public int HiddenSectors { get; internal set; }


      /// <summary><see langword="true"/> if the partition is of a recognized type.</summary>
      public bool RecognizedPartition { get; internal set; }


      /// <summary>The type of the partition.</summary>
      public DiskPartitionTypes Type { get; internal set; }

      #endregion // Properties


      #region Methods

      /// <summary>Returns storage device as: "VendorId ProductId DeviceType DeviceNumber:PartitionNumber".</summary>
      /// <returns>A string that represents this instance.</returns>
      public override string ToString()
      {
         return string.Format(CultureInfo.CurrentCulture, "BootIndicator {0}, Type: {1}, RecognizedPartition: {2}", BootIndicator.ToString(), Type.ToString(), RecognizedPartition.ToString()).Trim();
      }


      /// <summary>Determines whether the specified Object is equal to the current Object.</summary>
      /// <param name="obj">Another object to compare to.</param>
      /// <returns><see langword="true"/> if the specified Object is equal to the current Object; otherwise, <see langword="false"/>.</returns>
      public override bool Equals(object obj)
      {
         if (null == obj || GetType() != obj.GetType())
            return false;

         var other = obj as StoragePartitionInfoMbr;

         return null != other &&
                other.BootIndicator == BootIndicator &&
                other.HiddenSectors == HiddenSectors &&
                other.RecognizedPartition == RecognizedPartition &&
                other.Type == Type;
      }


      /// <summary>Serves as a hash function for a particular type.</summary>
      /// <returns>A hash code for the current Object.</returns>
      public override int GetHashCode()
      {
         unchecked
         {
            return BootIndicator.GetHashCode() + HiddenSectors.GetHashCode() + RecognizedPartition.GetHashCode() + Type.GetHashCode();
         }
      }


      /// <summary>Implements the operator ==</summary>
      /// <param name="left">A.</param>
      /// <param name="right">B.</param>
      /// <returns>The result of the operator.</returns>
      public static bool operator ==(StoragePartitionInfoMbr left, StoragePartitionInfoMbr right)
      {
         return ReferenceEquals(left, null) && ReferenceEquals(right, null) || !ReferenceEquals(left, null) && !ReferenceEquals(right, null) && left.Equals(right);
      }


      /// <summary>Implements the operator !=</summary>
      /// <param name="left">A.</param>
      /// <param name="right">B.</param>
      /// <returns>The result of the operator.</returns>
      public static bool operator !=(StoragePartitionInfoMbr left, StoragePartitionInfoMbr right)
      {
         return !(left == right);
      }

      #endregion // Methods
   }
}
