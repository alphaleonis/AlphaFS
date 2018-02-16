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
   /// <summary>Provides access to information of a device, on a local or remote host.</summary>
   [Serializable]
   [SecurityCritical]
   public sealed class StorageDeviceInfo
   {
      #region Constructors

      /// <summary>Initializes a StorageDeviceInfo instance.</summary>
      public StorageDeviceInfo()
      {
         BusType = StorageBusType.Unknown;
         DeviceType = StorageDeviceType.Unknown;
      }


      internal StorageDeviceInfo(NativeMethods.STORAGE_DEVICE_NUMBER device) : this()
      {
         DeviceType = device.DeviceType;
         DeviceNumber = device.DeviceNumber;
         PartitionNumber = device.PartitionNumber;
      }

      #endregion // Constructors


      #region Methods

      /// <summary>Returns storage device as: "DeviceNumber:PartitionNumber DeviceType/BusType".</summary>
      /// <returns>A string that represents this instance.</returns>
      public override string ToString()
      {
         return string.Format(CultureInfo.CurrentCulture, "{0}:{1} {2}/{3}", DeviceNumber, PartitionNumber, DeviceType.ToString(), BusType.ToString());
      }


      /// <summary>Determines whether the specified Object is equal to the current Object.</summary>
      /// <param name="obj">Another object to compare to.</param>
      /// <returns><see langword="true"/> if the specified Object is equal to the current Object; otherwise, <see langword="false"/>.</returns>
      public override bool Equals(object obj)
      {
         if (null == obj || GetType() != obj.GetType())
            return false;

         var other = obj as StorageDeviceInfo;

         return null != other && other.DeviceType == DeviceType && other.DeviceNumber == DeviceNumber && other.PartitionNumber == PartitionNumber;
      }


      /// <summary>Serves as a hash function for a particular type.</summary>
      /// <returns>A hash code for the current Object.</returns>
      public override int GetHashCode()
      {
         return DeviceNumber + PartitionNumber + DeviceType.GetHashCode();
      }


      /// <summary>Implements the operator ==</summary>
      /// <param name="left">A.</param>
      /// <param name="right">B.</param>
      /// <returns>The result of the operator.</returns>
      public static bool operator ==(StorageDeviceInfo left, StorageDeviceInfo right)
      {
         return ReferenceEquals(left, null) && ReferenceEquals(right, null) || !ReferenceEquals(left, null) && !ReferenceEquals(right, null) && left.Equals(right);
      }


      /// <summary>Implements the operator !=</summary>
      /// <param name="left">A.</param>
      /// <param name="right">B.</param>
      /// <returns>The result of the operator.</returns>
      public static bool operator !=(StorageDeviceInfo left, StorageDeviceInfo right)
      {
         return !(left == right);
      }
      
      #endregion // Methods


      #region Properties

      /// <summary>The storage bus type. Requires elevated rights.</summary>
      public StorageBusType BusType { get; internal set; }


      /// <summary>The storage device type.</summary>
      public StorageDeviceType DeviceType { get; internal set; }


      /// <summary>The device number of the storage device, starting at 0.</summary>
      public int DeviceNumber { get; internal set; }


      /// <summary>The partition number of the storage device, starting at 1. If the device cannot be partitioned, like a CDROM, -1 is returned.</summary>
      public int PartitionNumber { get; internal set; }

      #endregion // Properties
   }
}
