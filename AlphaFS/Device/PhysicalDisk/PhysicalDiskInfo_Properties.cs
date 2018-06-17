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
using System.Collections.Generic;
using Alphaleonis.Win32.Filesystem;

namespace Alphaleonis.Win32.Device
{
   public sealed partial class PhysicalDiskInfo
   {
      /// <summary>An initialized <see cref="Filesystem.DeviceInfo"/> instance.</summary>
      private DeviceInfo DeviceInfo { get; set; }


      /// <summary>The <see cref="Filesystem.DeviceInfo.DeviceDescription"/>.</summary>
      public string DeviceDescription
      {
         get { return null != DeviceInfo ? DeviceInfo.DeviceDescription : null; }
      }
      

      /// <summary>The path to the device.</summary>
      /// <returns>Returns a string that represents the path to the device.
      ///   A drive path such as: <c>C:</c>, <c>D:\</c>,
      ///   a volume <see cref="Guid"/> path such as: <c>\\?\Volume{xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}\</c>
      ///   or a <see cref="Filesystem.DeviceInfo.DevicePath"/> string such as: <c>\\?\scsi#disk...{xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}</c> string.
      /// </returns>
      public string DevicePath
      {
         get { return null != DeviceInfo ? DeviceInfo.DevicePath : null; }
      }


      /// <summary>The Win32 Device name.</summary>
      public string DosDeviceName { get; private set; }


      /// <summary>An <see cref="Array"/> of logical drives that are located on the physical disk or <c>null</c> when no entries found.</summary>
      public IEnumerable<string> LogicalDrives { get; private set; }

      
      /// <summary>The <see cref="Filesystem.DeviceInfo.FriendlyName"/>.</summary>
      public string Name
      {
         get { return null != DeviceInfo ? DeviceInfo.FriendlyName : null; }
      }


      /// <summary>An <see cref="Array"/> of partition index numbers that are located on the physical disk or <c>null</c> when no entries found.</summary>
      public IEnumerable<int> PartitionIndexes { get; private set; }


      /// <summary>The <see cref="Filesystem.DeviceInfo.PhysicalDeviceObjectName"/> (PDO) information provided by a device's firmware to Windows.</summary>
      public string PhysicalDeviceObjectName
      {
         get { return null != DeviceInfo ? DeviceInfo.PhysicalDeviceObjectName : null; }
      }


      /// <summary>The storage device adapter information. Retrieving this information requires an elevated state.</summary>
      public StorageAdapterInfo StorageAdapterInfo { get; private set; }


      /// <summary>The storage device information.</summary>
      public StorageDeviceInfo StorageDeviceInfo { get; private set; }

      
      /// <summary>The storage device partition information.</summary>
      public StoragePartitionInfo StoragePartitionInfo { get; private set; }


      /// <summary>An <see cref="Array"/> of volume <see cref="Guid"/> strings of volumes that are located on the physical disk or <c>null</c> when no entries found.</summary>
      public IEnumerable<string> VolumeGuids { get; private set; }
   }
}
