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
using Alphaleonis.Win32.Filesystem;

namespace Alphaleonis.Win32.Device
{
   /// <summary>[AlphaFS] Provides access to information of a physical disk on the Computer.</summary>
   [Serializable]
   [SecurityCritical]
   public sealed partial class PhysicalDiskInfo
   {
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

         Utils.CopyTo(Local.GetPhysicalDiskInfoCore(Security.ProcessContext.IsElevatedProcess, deviceNumber, null), this);
      }

      #endregion // Constructors


      /// <summary>[AlphaFS] Retrieves the physical disk that is related to the logical drive name, volume <see cref="Guid"/> or <see cref="DeviceInfo.DevicePath"/>.</summary>
      ///  <returns>A <see cref="PhysicalDiskInfo"/> instance that represents the physical disk on the Computer or <c>null</c> on error/no data available.</returns>
      /// <remark>
      ///   Most properties of the returned <see cref="StorageAdapterInfo"/> and <see cref="StorageDeviceInfo"/> instances are meaningless unless this method is called from an elevated state.
      ///   Do not call this method for every logical drive/volume on the Computer as each call queries all physical disks, associated volumes and logical drives.
      ///   Instead, use method <see cref="Local.EnumeratePhysicalDisks()"/> and property <see cref="VolumeGuids"/> or <see cref="LogicalDrives"/>.
      /// </remark>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="Exception"/>
      /// <param name="isElevated"><c>true</c> indicates the current process is in an elevated state, allowing to retrieve more data.</param>
      /// <param name="devicePath">
      ///    <para>A disk path such as: <c>\\.\PhysicalDrive0</c></para>
      ///    <para>A drive path such as: <c>C</c>, <c>C:</c> or <c>C:\</c></para>
      ///    <para>A volume <see cref="Guid"/> such as: <c>\\?\Volume{xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}\</c></para>
      ///    <para>A <see cref="DeviceInfo.DevicePath"/> string such as: <c>\\?\scsi#disk...{xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}</c></para>
      /// </param>
      /// <param name="deviceInfo">A <see cref="DeviceInfo"/> instance.</param>
      /// <remarks>Use either <paramref name="devicePath"/> or <paramref name="deviceInfo"/>, not both.</remarks>
      [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Object is disposed.")]
      [SecurityCritical]
      internal static PhysicalDiskInfo InitializePhysicalDiskInfo(bool isElevated, string devicePath, DeviceInfo deviceInfo)
      {
         if (null == devicePath && null == deviceInfo)
            return null;

         var isDevice = null != deviceInfo && !Utils.IsNullOrWhiteSpace(deviceInfo.DevicePath);

         if (isDevice)
            devicePath = deviceInfo.DevicePath;



         var storageDeviceInfo = Local.GetStorageDeviceInfoCore(isElevated, devicePath);

         if (null == storageDeviceInfo)
            return null;


         var pDiskInfo = new PhysicalDiskInfo
         {
            DevicePath = devicePath,

            DeviceDescription = isDevice ? deviceInfo.DeviceDescription : null,

            Name = isDevice ? deviceInfo.FriendlyName : null,

            PhysicalDeviceObjectName = isDevice ? deviceInfo.PhysicalDeviceObjectName : null,


            StorageAdapterInfo = Local.GetStorageAdapterInfo(isElevated, devicePath),

            StorageDeviceInfo = storageDeviceInfo,

            StoragePartitionInfo = Local.GetStoragePartitionInfo(isElevated, devicePath)
         };


         if (isDevice)
            pDiskInfo.StorageAdapterInfo.BusReportedDeviceDescription = deviceInfo.BusReportedDeviceDescription;


         return pDiskInfo;
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
