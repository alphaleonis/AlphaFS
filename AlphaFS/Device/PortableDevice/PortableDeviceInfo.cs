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

using PortableDeviceApiLib;
using System;
using System.Reflection;
using Alphaleonis.Win32.Filesystem;


namespace Alphaleonis.Win32.Device
{
   /// <summary>Provides access to information to a Windows Portable Device (WPD).</summary>
   [Serializable]
   public sealed partial class PortableDeviceInfo
   {
      #region Fields

      //private static readonly FileVersionInfo ProductVersion = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
      private readonly bool _mtpOnly;

      // The underlying <see cref="_deviceInfo"/> instance of the Portable Device.
      private readonly DeviceInfo _deviceInfo;


      private readonly AssemblyName _product;
      private readonly Version _version;

      #endregion // Fields


      #region Constructors

      private PortableDeviceInfo()
      {

      }


      ///// <summary>Initializes a new instance of the <see cref="T:Alphaleonis.Win32.Filesystem.PortableDeviceInfo"/> class, which acts as a wrapper for a portable device (WPD) file system object path.</summary>
      ///// <param name="deviceId">The ID of the Portable Device in the format \\?\usb#vid_...</param>
      ///// <remarks>The <paramref name="deviceId"/> is the same as the <see cref="T:DeviceInfo.DevicePath"/> property.</remarks>
      //public PortableDeviceInfo(string deviceId) : this((DeviceInfo) null, false, false)
      //{
      //}


      ///// <summary>Initializes a new instance of the <see cref="T:Alphaleonis.Win32.Filesystem.PortableDeviceInfo"/> class, which acts as a wrapper for a portable device (WPD) file system object path.</summary>
      ///// <param name="deviceId">The ID of the Portable Device in the format \\?\usb#vid_...</param>
      ///// <param name="connect"><c>true</c> connects to the Portable Device as soon as the instance is created. <c>false</c> does not connect to the device. Use method <see cref="M:Connect()"/> to manually connect.</param>
      ///// <remarks>The <paramref name="deviceId"/> is the same as the <see cref="T:DeviceInfo.DevicePath"/> property.</remarks>
      //public PortableDeviceInfo(string deviceId, bool connect) : this((DeviceInfo) null, connect, false)
      //{
      //}


      ///// <summary>Initializes a new instance of the <see cref="T:Alphaleonis.Win32.Filesystem.PortableDeviceInfo"/> class, which acts as a wrapper for a portable device (WPD) file system object path.</summary>
      ///// <param name="deviceId">The ID of the Portable Device in the format \\?\usb#vid_...</param>
      ///// <param name="connect"><c>true</c> connects to the Portable Device as soon as the instance is created. <c>false</c> does not connect to the device. Use method <see cref="M:Connect()"/> to manually connect.</param>
      ///// <param name="mtpOnly"><c>true</c> to only enumerate WPD devices using the <see cref="PortableDeviceProtocol.MediaTransferProtocol"/>.</param>
      ///// <remarks>The <paramref name="deviceId"/> is the same as the <see cref="T:DeviceInfo.DevicePath"/> property.</remarks>
      //[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "mtp")]
      //public PortableDeviceInfo(string deviceId, bool connect, bool mtpOnly) : this((DeviceInfo) null, connect, mtpOnly)
      //{
      //}


      internal PortableDeviceInfo(DeviceInfo deviceInfo, bool connect, bool mtpOnly)
      {
         _product = Assembly.GetExecutingAssembly().GetName();
         _version = _product.Version;

         _mtpOnly = mtpOnly;

         DeviceType = PortableDeviceType.Unknown;
         DevicePowerSource = PortableDevicePowerSource.Unknown;

         _deviceInfo = deviceInfo ?? new DeviceInfo();

         PortableDevice = new PortableDeviceClass();

         if (connect || mtpOnly)
            Connect();

         if (IsConnected && !connect && !mtpOnly)
            Disconnect();
      }

      #endregion // Constructors


      #region Properties
      
      /// <summary>The PnP Device ID (<see cref="DeviceInfo.DevicePath"/>) of the Portable Device.</summary>
      public string DeviceId
      {
         get
         {
            return null != _deviceInfo ? _deviceInfo.DevicePath : string.Empty;
         }
      }


      /// <summary>Indicates the Portable Device's connection status.</summary>
      /// <remarks>Set to internal as this is a snapshot state.</remarks>
      internal bool IsConnected { get; private set; }


      /// <summary>The firmware version for the Portable Device.</summary>
      public string DeviceFirmwareVersion { get; private set; }


      /// <summary>The Friendly Name of the Portable Device.</summary>
      public string DeviceFriendlyName
      {
         get { return null != _deviceInfo ? _deviceInfo.FriendlyName : string.Empty; }
      }


      /// <summary>The Manufacturer of the Portable Device.</summary>
      public string DeviceManufacturer
      {
         get { return null != _deviceInfo ? _deviceInfo.Manufacturer : string.Empty; }
      }


      /// <summary>The model name of the Portable Device.</summary>
      public string DeviceModel
      {
         get { return null != _deviceInfo ? _deviceInfo.DeviceDescription : string.Empty; }
      }


      internal PortableDeviceClass PortableDevice { get; private set; }


      /// <summary>A value from 0 to 100 that indicates the power level of the Portable Device battery, with 0 being none, and 100 being fully charged.</summary>
      public int DevicePowerLevel { get; private set; }


      /// <summary>A <see cref="T:PortableDevicePowerSource"/> enumeration that indicates the power source of the Portable Device.</summary>
      public PortableDevicePowerSource DevicePowerSource { get; private set; }


      /// <summary>The protocol that is being used by the Portable Device.</summary>
      public PortableDeviceProtocol DeviceProtocol { get; private set; }


      /// <summary>The serial number of the Portable Device.</summary>
      public string DeviceSerialNumber { get; private set; }


      /// <summary>Indicates the Portable Device type.</summary>
      public PortableDeviceType DeviceType { get; private set; }


      /// <summary>The total storage capacity, in bytes.</summary>
      public long StorageCapacity { get; private set; }


      /// <summary>The total storage capacity, formatted as a unit size.</summary>
      public string StorageCapacityUnitSize
      {
         get { return Utils.UnitSizeToText(StorageCapacity); }
      }


      ///// <summary>Indicates the total storage capacity in objects, for example the available slots on a SIM card.</summary>
      //public long WPD_STORAGE_CAPACITY_IN_OBJECTS { get; private set; }


      /// <summary>A human-readable description of the storage.</summary>
      public string StorageDescription { get; private set; }


      /// <summary>A string description of the file system used by the storage: for example, "FAT32", "NTFS", "Contoso File System".</summary>
      public string StorageFileSystemType { get; private set; }


      /// <summary>The available storage space, in bytes.</summary>
      public long StorageFreeSpaceInBytes { get; private set; }


      /// <summary>The available storage space, formatted as a unit size.</summary>
      public string StorageFreeSpaceUnitSize
      {
         get { return Utils.UnitSizeToText(StorageFreeSpaceInBytes); }
      }


      ///// <summary>The number of additional objects that can be written to the device. For example, if a device only allows a single object, this would be zero if the object already existed.</summary>
      //public long WPD_STORAGE_FREE_SPACE_IN_OBJECTS { get; private set; }


      ///// <summary>Specifies the maximum size of a single object (in bytes) that can be placed on this storage.</summary>
      //public long WPD_STORAGE_MAX_OBJECT_SIZE { get; private set; }


      /// <summary>A vendor-specific serial number for the storage.</summary>
      public string StorageSerialNumber { get; private set; }


      /// <summary>Describes the physical type of a memory storage medium.</summary>
      public PortableDeviceStorageType StorageType { get; private set; }


      /// <summary>The transport supported by the device, such as USB, IP, or Bluetooth.</summary>
      public PortableDeviceTransportType TransportType { get; private  set; }

      #endregion // Properties


      #region Methods

      /// <summary>Returns the "FriendlyName" of the physical disk.</summary>
      /// <returns>Returns a string that represents this instance.</returns>
      public override string ToString()
      {
         return DeviceFriendlyName;
      }


      /// <summary>Determines whether the specified Object is equal to the current Object.</summary>
      /// <param name="obj">Another object to compare to.</param>
      /// <returns><c>true</c> if the specified Object is equal to the current Object; otherwise, <c>false</c>.</returns>
      public override bool Equals(object obj)
      {
         if (null == obj || GetType() != obj.GetType())
            return false;

         var other = obj as PortableDeviceInfo;

         return null != other &&
                other.DeviceId == DeviceId &&
                other.DeviceType == DeviceType &&
                other.DeviceFirmwareVersion == DeviceFirmwareVersion &&
                other.DeviceFriendlyName == DeviceFriendlyName &&
                other.DeviceManufacturer == DeviceManufacturer &&
                other.DeviceModel == DeviceModel &&
                other.PortableDevice == PortableDevice &&
                other.DeviceProtocol == DeviceProtocol &&
                other.DeviceSerialNumber == DeviceSerialNumber &&
                other.StorageCapacity == StorageCapacity &&
                other.StorageSerialNumber == StorageSerialNumber &&
                other.StorageType == StorageType &&
                other.TransportType == TransportType;
      }


      /// <summary>Serves as a hash function for a particular type.</summary>
      /// <returns>Returns a hash code for the current Object.</returns>
      public override int GetHashCode()
      {
         unchecked
         {
            return DeviceType.GetHashCode() +
                   (null != DeviceId ? DeviceId.GetHashCode() : 0) +
                   (null != DeviceFirmwareVersion ? DeviceFirmwareVersion.GetHashCode() : 0) +
                   (null != DeviceManufacturer ? DeviceManufacturer.GetHashCode() : 0) +
                   (null != DeviceModel ? DeviceModel.GetHashCode() : 0) +
                   (null != DeviceSerialNumber ? DeviceSerialNumber.GetHashCode() : 0) +
                   (null != StorageSerialNumber ? StorageSerialNumber.GetHashCode() : 0);
         }
      }


      /// <summary>Implements the operator ==</summary>
      /// <param name="left">A.</param>
      /// <param name="right">B.</param>
      /// <returns>The result of the operator.</returns>
      public static bool operator ==(PortableDeviceInfo left, PortableDeviceInfo right)
      {
         return ReferenceEquals(left, null) && ReferenceEquals(right, null) || !ReferenceEquals(left, null) && !ReferenceEquals(right, null) && left.Equals(right);
      }


      /// <summary>Implements the operator !=</summary>
      /// <param name="left">A.</param>
      /// <param name="right">B.</param>
      /// <returns>The result of the operator.</returns>
      public static bool operator !=(PortableDeviceInfo left, PortableDeviceInfo right)
      {
         return !(left == right);
      }

      #endregion // Methods
   }
}
