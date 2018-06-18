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

using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Security;

namespace Alphaleonis.Win32.Device
{
   /// <summary>[AlphaFS] Provides access to information of a storage device.</summary>
   [Serializable]
   [SecurityCritical]
   public sealed class StorageDeviceInfo
   {
      #region Fields


      #endregion // Private Fields


      #region Constructors

      /// <summary>[AlphaFS] Initializes an empty StorageDeviceInfo instance.</summary>
      public StorageDeviceInfo()
      {
         DeviceType = StorageDeviceType.Unknown;

         DeviceNumber = -1;

         PartitionNumber = -1;
      }


      internal StorageDeviceInfo(NativeMethods.STORAGE_DEVICE_NUMBER device) : this()
      {
         DeviceType = device.DeviceType;

         DeviceNumber = device.DeviceNumber;

         PartitionNumber = device.PartitionNumber;
      }

      #endregion // Constructors


      #region Properties

      /// <summary>The type of the bus to which the device is connected.</summary>
      public StorageBusType BusType { get; internal set; }


      /// <summary>
      ///   Indicates if the physical disk supports multiple outstanding commands (SCSI tagged queuing or equivalent).
      ///   When false the physical disk does not support SCSI-tagged queuing or the equivalent.
      /// </summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Queueing")]
      public bool CommandQueueing { get; internal set; }
      

      /// <summary>The storage device type.</summary>
      public StorageDeviceType DeviceType { get; private set; }


      /// <summary>The device number of the storage device, starting at <c>0</c>.</summary>
      public int DeviceNumber { get; private set; }


      /// <summary>
      ///   The partition number of the storage device, starting at <c>1</c>.
      ///   Partition number <c>0</c> is the device.
      ///   If the device cannot be partitioned, like a CDROM, <c>-1</c> is returned.
      ///   <c>-1</c> is also returned if the device is accessed by its path: <c>\\?\scsi#disk...{xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}</c>
      ///   instead of a volume /logical drive path.
      /// </summary>
      public int PartitionNumber { get; private set; }


      /// <summary>The product ID of the physical disk.</summary>
      public string ProductId { get; internal set; }


      /// <summary>The product revision of the physical disk.</summary>
      public string ProductRevision { get; internal set; }


      /// <summary>
      ///   Indicates if the physical disk is removable. When true the physical disk's media (if any) is removable.
      ///   If the device has no media, this member should be ignored. When false the physical disk's media is not removable.
      /// </summary>
      public bool RemovableMedia { get; internal set; }


      /// <summary>The serial number of the physical disk. If the physical disk has no serial number <c>null</c> is returned.</summary>
      public string SerialNumber { get; internal set; }


      // /// <summary>The total size of the physical disk or <c>0</c> if the process is not elevated or the device is a controller.</summary>

      /// <summary>The total size of the physical disk.</summary>
      public long TotalSize { get; internal set; }


      /// <summary>The <see cref="TotalSize"/> of the physical disk, formatted as a unit size.</summary>
      public string TotalSizeUnitSize
      {
         get { return Utils.UnitSizeToText(TotalSize); }
      }


      /// <summary>The Vendor ID of the physical disk.</summary>
      public string VendorId { get; internal set; }

      #endregion // Properties


      #region Methods

      /// <summary>Returns storage device as: "DeviceType DeviceNumber:PartitionNumber VendorId ProductId".</summary>
      /// <returns>Returns a string that represents this instance.</returns>
      public override string ToString()
      {
         return string.Format(CultureInfo.CurrentCulture, "Device: {0}:{1} {2}", DeviceNumber.ToString(CultureInfo.InvariantCulture), PartitionNumber.ToString(CultureInfo.InvariantCulture), (VendorId + " " + ProductId + " " + ProductRevision).Trim()).Trim();
      }


      /// <summary>Determines whether the specified Object is equal to the current Object.</summary>
      /// <param name="obj">Another object to compare to.</param>
      /// <returns><c>true</c> if the specified Object is equal to the current Object; otherwise, <c>false</c>.</returns>
      public override bool Equals(object obj)
      {
         if (null == obj || GetType() != obj.GetType())
            return false;

         var other = obj as StorageDeviceInfo;

         return null != other &&
                other.BusType == BusType &&
                other.DeviceNumber == DeviceNumber &&
                other.DeviceType == DeviceType &&
                other.PartitionNumber == PartitionNumber &&
                other.ProductId == ProductId &&
                other.ProductRevision == ProductRevision &&
                other.RemovableMedia == RemovableMedia &&
                other.SerialNumber == SerialNumber &&
                other.TotalSize == TotalSize &&
                other.VendorId == VendorId;
      }


      /// <summary>Serves as a hash function for a particular type.</summary>
      /// <returns>Returns a hash code for the current Object.</returns>
      public override int GetHashCode()
      {
         unchecked
         {
            return RemovableMedia.GetHashCode() + TotalSize.GetHashCode() + DeviceNumber.GetHashCode() + PartitionNumber.GetHashCode();
         }
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
   }
}
