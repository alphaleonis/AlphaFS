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
   public sealed class StoragePartitionInfoGpt
   {
      #region Constructors

      /// <summary>Initializes a StoragePartitionInfoGpt instance.</summary>
      public StoragePartitionInfoGpt()
      {
      }


      internal StoragePartitionInfoGpt(NativeMethods.PARTITION_INFORMATION_GPT gptPartition) : this()
      {
         Attributes = (PartitionAttributes) gptPartition.Attributes;
         
         Id = gptPartition.PartitionId;

         Description = gptPartition.Name;

         Type = gptPartition.PartitionType;
      }

      #endregion // Constructors


      #region Properties

      /// <summary>The Extensible Firmware Interface (EFI) attributes of the partition.</summary>
      public PartitionAttributes Attributes { get; internal set; }

      
      /// <summary>The GUID of the partition.</summary>
      public Guid Id { get; internal set; }

      
      /// <summary>The description of the partition.</summary>
      public string Description { get; internal set; }


      /// <summary>The GUID of the partition that identifies the partition type.
      /// <remarks>Each partition type that the EFI specification supports is identified by its own GUID, which is published by the developer of the partition.</remarks>
      /// </summary>
      public Guid Type { get; internal set; }

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

         var other = obj as StoragePartitionInfoGpt;

         return null != other &&
                other.Attributes == Attributes &&
                other.Id == Id &&
                other.Description == Description &&
                other.Type == Type;
      }


      /// <summary>Serves as a hash function for a particular type.</summary>
      /// <returns>A hash code for the current Object.</returns>
      public override int GetHashCode()
      {
         unchecked
         {
            return Attributes.GetHashCode() + Id.GetHashCode() + Description.GetHashCode() + Type.GetHashCode();
         }
      }


      /// <summary>Implements the operator ==</summary>
      /// <param name="left">A.</param>
      /// <param name="right">B.</param>
      /// <returns>The result of the operator.</returns>
      public static bool operator ==(StoragePartitionInfoGpt left, StoragePartitionInfoGpt right)
      {
         return ReferenceEquals(left, null) && ReferenceEquals(right, null) || !ReferenceEquals(left, null) && !ReferenceEquals(right, null) && left.Equals(right);
      }


      /// <summary>Implements the operator !=</summary>
      /// <param name="left">A.</param>
      /// <param name="right">B.</param>
      /// <returns>The result of the operator.</returns>
      public static bool operator !=(StoragePartitionInfoGpt left, StoragePartitionInfoGpt right)
      {
         return !(left == right);
      }

      #endregion // Methods
   }
}
