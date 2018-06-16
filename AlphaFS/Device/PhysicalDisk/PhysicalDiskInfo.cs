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

      private int _deviceNumber = -1;
      private string _localDevicePath;
      private bool _isDrive;
      private bool _isVolume;
      private bool _isDevice;

      #endregion // Fields


      #region Constructors

      /// <summary>[AlphaFS] Initializes an empty PhysicalDiskInfo instance.</summary>
      public PhysicalDiskInfo()
      {
      }


      /// <summary>[AlphaFS] Initializes a PhysicalDiskInfo instance from a physical disk number such as: <c>0</c>, <c>1</c>, ...</summary>
      /// <param name="deviceNumber">A number that indicates a physical disk on the Computer.</param>
      public PhysicalDiskInfo(int deviceNumber)
      {
         if (deviceNumber < 0)
            throw new ArgumentOutOfRangeException("deviceNumber");

         _deviceNumber = deviceNumber;
         

         var pDiskInfo = Local.GetPhysicalDiskInfoCore(Security.ProcessContext.IsElevatedProcess, _deviceNumber, null);

         if (null != pDiskInfo)
         {
            SetDevicePath(pDiskInfo.DevicePath);

            Utils.CopyTo(pDiskInfo, this);
         }
      }


      /// <summary>[AlphaFS] Initializes a PhysicalDiskInfo instance from a physical disk number such as: <c>0</c>, <c>1</c>, ...</summary>
      /// <remark>
      ///   Do not create and instance for every volume/logical drive on the Computer as each call queries all physical disks, associated volumes and logical drives.
      ///   Instead, use method <see cref="EnumeratePhysicalDisks()"/> and property <see cref="VolumeGuids"/> or <see cref="LogicalDrives"/>.
      /// </remark>
      /// <param name="devicePath">
      ///    <para>A disk path such as: <c>\\.\PhysicalDrive0</c></para>
      ///    <para>A drive path such as: <c>C</c>, <c>C:</c> or <c>C:\</c></para>
      ///    <para>A volume <see cref="Guid"/> such as: <c>\\?\Volume{xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}\</c></para>
      ///    <para>A <see cref="DeviceInfo.DevicePath"/> string such as: <c>\\?\scsi#disk...{xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}</c></para>
      /// </param>
      public PhysicalDiskInfo(string devicePath)
      {
         SetDevicePath(devicePath);

         var pDiskInfo = Local.GetPhysicalDiskInfoCore(Security.ProcessContext.IsElevatedProcess, -1, _localDevicePath);

         if (null != pDiskInfo)
            Utils.CopyTo(pDiskInfo, this);
      }

      #endregion // Constructors


      private void SetDevicePath(string devicePath)
      {
         _localDevicePath = FileSystemHelper.GetValidatedDevicePath(devicePath, out _isDrive, out _isVolume, out _isDevice);

         if (_isDrive)
            _localDevicePath = FileSystemHelper.GetLocalDevicePath(_localDevicePath);
      }


      /// <summary>Returns the "FriendlyName" of the physical disk.</summary>
      /// <returns>Returns a string that represents this instance.</returns>
      public override string ToString()
      {
         return Name ?? DevicePath;
      }


      /// <summary>Determines whether the specified Object is equal to the current Object.</summary>
      /// <param name="obj">Another object to compare to.</param>
      /// <returns><c>true</c> if the specified Object is equal to the current Object; otherwise, <c>false</c>.</returns>
      public override bool Equals(object obj)
      {
         if (null == obj || GetType() != obj.GetType())
            return false;

         var other = obj as PhysicalDiskInfo;

         return null != other && null != other.DevicePath && null != other.StorageDeviceInfo &&
                other.DevicePath.Equals(DevicePath, StringComparison.OrdinalIgnoreCase) &&
                other.StorageDeviceInfo.Equals(StorageDeviceInfo) &&
                other.StorageDeviceInfo.DeviceNumber.Equals(StorageDeviceInfo.DeviceNumber) && other.StorageDeviceInfo.PartitionNumber.Equals(StorageDeviceInfo.PartitionNumber);
      }


      /// <summary>Serves as a hash function for a particular type.</summary>
      /// <returns>Returns a hash code for the current Object.</returns>
      public override int GetHashCode()
      {
         return null != DevicePath ? DevicePath.GetHashCode() : 0;
      }


      /// <summary>Implements the operator ==</summary>
      /// <param name="left">A.</param>
      /// <param name="right">B.</param>
      /// <returns>The result of the operator.</returns>
      public static bool operator ==(PhysicalDiskInfo left, PhysicalDiskInfo right)
      {
         return ReferenceEquals(left, null) && ReferenceEquals(right, null) || !ReferenceEquals(left, null) && !ReferenceEquals(right, null) && left.Equals(right);
      }


      /// <summary>Implements the operator !=</summary>
      /// <param name="left">A.</param>
      /// <param name="right">B.</param>
      /// <returns>The result of the operator.</returns>
      public static bool operator !=(PhysicalDiskInfo left, PhysicalDiskInfo right)
      {
         return !(left == right);
      }
   }
}
