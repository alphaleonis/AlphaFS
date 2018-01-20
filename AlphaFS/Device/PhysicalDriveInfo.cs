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
using System.Security;

namespace Alphaleonis.Win32.Filesystem
{
   /// <summary>Provides access to information of a physical drive.</summary>
   [Serializable]
   [SecurityCritical]
   public sealed class PhysicalDriveInfo
   {
      [NonSerialized] private string _productID;
      [NonSerialized] private string _vendorID;


      /// <summary>Initializes a PhysicalDriveInfo instance.</summary>
      public PhysicalDriveInfo()
      {
         SerialNumber = -1;
         TotalSize = -1;
      }


      /// <summary>Returns the product- and/or model ID, or bus type of the physical drive.</summary>
      /// <returns>A string that represents this instance.</returns>
      public override string ToString()
      {
         return !Utils.IsNullOrWhiteSpace(Name) ? Name : BusType.ToString();
      }


      /// <summary>The bus type of the physical drive.</summary>
      public StorageBusType BusType { get; internal set; }

      
      /// <summary>The device type of the physical drive.</summary>
      public StorageDeviceType DeviceType { get; internal set; }


      /// <summary>The index number of the physical drive.</summary>
      public int DeviceNumber { get; internal set; }


      /// <summary>Indicates the partition number of the device is returned in this member, if the device can be partitioned. Otherwise, -1 is returned.</summary>
      public int PartitionNumber { get; internal set; }

      ///// <summary>Interface type by WMI (Win32_DiskDrive).</summary>
      //public string InterfaceType { get; internal set; }


      /// <summary>Indicates if the physical drive supports multiple outstanding commands (SCSI tagged queuing or equivalent). When false the physical drive does not support SCSI-tagged queuing or the equivalent.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Queueing")]
      public bool SupportsCommandQueueing { get; internal set; }


      /// <summary>Indicates if the physical drive is removable. When true the physical drive's media (if any) is removable. If the device has no media, this member should be ignored. When false the physical drive's media is not removable.</summary>
      public bool IsRemovable { get; internal set; }


      /// <summary>Returns the product ID of the physical drive.</summary>
      public string Name
      {
         get { return _productID ?? string.Empty; }

         internal set { _productID = value; }
      }


      /// <summary>The product revision of the physical drive.</summary>
      public string ProductRevision { get; internal set; }
      

      /// <summary>Gets the serial number of the physical drive. If the physical drive has no serial number, this member is -1.</summary>
      public long SerialNumber { get; internal set; }


      /// <summary>The total size of the physical drive. If the session is not elevated, this member is -1.</summary>
      public long TotalSize { get; internal set; }


      /// <summary>The Vendor ID of the physical drive.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
      public string VendorID
      {
         get { return _vendorID ?? string.Empty; }

         internal set
         {
            // SanDisk X400 M.2 2280 256GB reports VendorID as: "("
            // DeviceInfo.Manufacturer = "(Standard disk drives)" might be a hint?

            if (null != value)
               _vendorID = value.Length > 1 ? value : string.Empty;
         }
      }
   }
}
