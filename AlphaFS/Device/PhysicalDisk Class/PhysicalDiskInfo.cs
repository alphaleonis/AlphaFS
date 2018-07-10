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
using System.Collections.ObjectModel;
using System.Security;
using Alphaleonis.Win32.Filesystem;

namespace Alphaleonis.Win32.Device
{
   /// <summary>[AlphaFS] Provides access to information of a physical disk on the Computer.</summary>
   [Serializable]
   [SecurityCritical]
   public sealed partial class PhysicalDiskInfo
   {
      #region Fields

      /// <summary>An initialized <see cref="DeviceInfo"/> instance.</summary>
      private DeviceInfo _deviceInfo; //{ get; set; }

      private Collection<int> _partitionIndexCollection;
      private Collection<string> _volumeGuidCollection;
      private Collection<string> _logicalDriveCollection;

      #endregion // Fields


      #region Constructors

      private PhysicalDiskInfo()
      {
      }


      /// <summary>[AlphaFS] Initializes an instance from a physical disk device number.</summary>
      /// <param name="deviceNumber">A device number that indicates a physical disk on the Computer.</param>
      public PhysicalDiskInfo(int deviceNumber)
      {
         if (deviceNumber < 0)
            throw new ArgumentOutOfRangeException("deviceNumber");

         CreatePhysicalDiskInfoInstance(deviceNumber, null, null, null);
      }


      /// <summary>[AlphaFS] Initializes an instance from a physical disk device path.</summary>
      /// <remark>
      ///   Creating an instance for every volume/logical drive on the Computer is expensive as each call queries all physical disks, associated volumes/logical drives.
      ///   Instead, use method <see cref="Local.EnumeratePhysicalDisks()"/> and property <see cref="VolumeGuids"/> or <see cref="LogicalDrives"/>.
      /// </remark>
      /// <param name="devicePath">
      ///    <para>A disk path such as: <c>\\.\PhysicalDrive0</c></para>
      ///    <para>A drive path such as: <c>C</c>, <c>C:</c> or <c>C:\</c></para>
      ///    <para>A volume <see cref="Guid"/> such as: <c>\\?\Volume{xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}\</c></para>
      ///    <para>A <see cref="Filesystem.DeviceInfo.DevicePath"/> string such as: <c>\\?\scsi#disk...{xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}</c></para>
      /// </param>
      public PhysicalDiskInfo(string devicePath)
      {
         CreatePhysicalDiskInfoInstance(-1, devicePath, null, null);
      }


      /// <summary>Used by <see cref="Local.EnumeratePhysicalDisks()"/></summary>
      internal PhysicalDiskInfo(int deviceNumber, StorageDeviceInfo storageDeviceInfo, DeviceInfo deviceInfo)
      {
         CreatePhysicalDiskInfoInstance(deviceNumber, null, storageDeviceInfo, deviceInfo);
      }

      #endregion // Constructors


      #region Properties
      
      /// <summary>The device description.</summary>
      public string DeviceDescription
      {
         get { return null != _deviceInfo ? _deviceInfo.DeviceDescription : string.Empty; }
      }


      /// <summary>The device path.</summary>
      public string DevicePath
      {
         get { return null != _deviceInfo ? _deviceInfo.DevicePath : string.Empty; }
      }


      /// <summary>The Win32 device name.</summary>
      public string DosDeviceName { get; private set; }


      /// <summary>An <see cref="IEnumerable{String}"/> of logical drives that are located on the physical disk.</summary>
      public IEnumerable<string> LogicalDrives { get; private set; }


      /// <summary>The device (friendly) name.</summary>
      public string Name
      {
         get { return null != _deviceInfo ? _deviceInfo.FriendlyName : string.Empty; }
      }


      /// <summary>An <see cref="IEnumerable{String}"/> of partition index numbers that are located on the physical disk.</summary>
      public IEnumerable<int> PartitionIndexes { get; private set; }


      /// <summary>The device (PDO) information provided by a device's firmware to Windows.</summary>
      public string PhysicalDeviceObjectName
      {
         get { return null != _deviceInfo ? _deviceInfo.PhysicalDeviceObjectName : string.Empty; }
      }


      /// <summary>Retrieves the current power state of the device.</summary>
      /// <remarks>This property is not cached.</remarks>
      public bool PowerStateEnabled
      {
         get { return !Utils.IsNullOrWhiteSpace(DevicePath) && Local.GetDevicePowerState(DevicePath); }
      }


      /// <summary>The storage device adapter information.</summary>
      public StorageAdapterInfo StorageAdapterInfo { get; private set; }


      /// <summary>The storage device information.</summary>
      public StorageDeviceInfo StorageDeviceInfo { get; private set; }


      /// <summary>The storage device partition information.</summary>
      public StoragePartitionInfo StoragePartitionInfo { get; private set; }


      /// <summary>An <see cref="IEnumerable{String}"/> of volume <see cref="Guid"/> strings that are located on the physical disk.</summary>
      public IEnumerable<string> VolumeGuids { get; private set; }

      #endregion // Properties
   }
}
