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
using System.Linq;
using System.Security;
using Alphaleonis.Win32.Filesystem;

namespace Alphaleonis.Win32.Device
{
   /// <summary>[AlphaFS] Provides access to information of a physical disk.</summary>
   [Serializable]
   [SecurityCritical]
   public sealed class PhysicalDiskInfo
   {
      #region Constructors

      /// <summary>[AlphaFS] Initializes an empty PhysicalDiskInfo instance.</summary>
      public PhysicalDiskInfo()
      {
      }


      /// <summary>[AlphaFS] Initializes a PhysicalDiskInfo instance.</summary>
      /// <param name="physicalDiskInfo">An initialized <see cref="PhysicalDiskInfo"/> instance.</param>
      internal PhysicalDiskInfo(PhysicalDiskInfo physicalDiskInfo)
      {
         CopyTo(physicalDiskInfo, this);
      }

      #endregion // Constructors


      #region Properties

      /// <summary>The device description.</summary>
      public string DeviceDescription { get; internal set; }


      /// <summary>The path to the device.</summary>
      /// <returns>Returns a string that represents the path to the device.
      ///   A drive path such as: <c>C:</c>, <c>D:\</c>,
      ///   a volume <see cref="Guid"/> path such as: <c>\\?\Volume{xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}\</c>
      ///   or a <see cref="DeviceInfo.DevicePath"/> string such as: <c>\\?\scsi#disk...{xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}</c> string.
      /// </returns>
      public string DevicePath { get; internal set; }


      /// <summary>An <see cref="ICollection{String}"/> of logical drives that are located on the physical disk.</summary>
      public ICollection<string> LogicalDrives { get; internal set; }

      
      /// <summary>The "FriendlyName" of the physical disk.</summary>
      public string Name { get; internal set; }


      /// <summary>An <see cref="ICollection{String}"/> of partition index numbers that are located on the physical disk.</summary>
      public ICollection<int> PartitionIndexes { get; internal set; }


      /// <summary>Encapsulates the physical device location (PDO) information provided by a device's firmware to Windows.</summary>
      public string PhysicalDeviceObjectName { get; internal set; }


      /// <summary>The storage device adapter information. Retrieving this information requires an elevated state.</summary>
      public StorageAdapterInfo StorageAdapterInfo { get; internal set; }


      /// <summary>The storage device information.</summary>
      public StorageDeviceInfo StorageDeviceInfo { get; internal set; }

      
      /// <summary>The storage device partitiion information.</summary>
      public StoragePartitionInfo StoragePartitionInfo { get; internal set; }


      /// <summary>An <see cref="ICollection{String}"/> of volume <see cref="Guid"/> strings of volumes that are located on the physical disk.</summary>
      public ICollection<string> VolumeGuids { get; internal set; }


      ///// <summary>An <see cref="ICollection{String}"/> of volume label strings of volumes that are located on the physical disk.</summary>
      //public ICollection<string> VolumeLabels { get; internal set; }

      #endregion // Properties


      #region Methods

      /// <summary>Checks if the volume or logical drive is located on the physical disk.
      /// <para>A drive path such as: <c>C</c>, <c>C:</c> or <c>C:\</c></para>
      /// <para>A volume <see cref="Guid"/> such as: <c>\\?\Volume{xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}\</c></para>
      /// </summary>
      /// <returns><c>true</c> if the volume or logical drive is located on the physical disk; otherwise, <c>false</c>.</returns>
      /// <param name="devicePath">
      /// <para>A disk path such as: <c>\\.\PhysicalDrive0</c></para>
      /// <para>A drive path such as: <c>C</c>, <c>C:</c> or <c>C:\</c></para>
      /// <para>A volume <see cref="Guid"/> such as: <c>\\?\Volume{xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}\</c></para>
      /// <para>A <see cref="DeviceInfo.DevicePath"/> string such as: <c>\\?\scsi#disk...{xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}</c></para>
      /// </param>
      public bool ContainsVolume(string devicePath)
      {
         bool isDrive;
         bool isVolume;
         bool isDeviceInfo;

         devicePath = FileSystemHelper.GetValidatedDevicePath(devicePath, out isDrive, out isVolume, out isDeviceInfo);


         if (isDrive && null != LogicalDrives)
         {
            devicePath = devicePath.Replace(Path.LogicalDrivePrefix, string.Empty);

            devicePath = Path.RemoveTrailingDirectorySeparator(devicePath, false);

            return LogicalDrives.Any(driveName => driveName.Equals(devicePath, StringComparison.OrdinalIgnoreCase));
         }


         return isVolume && null != VolumeGuids && VolumeGuids.Any(guid => guid.Equals(devicePath, StringComparison.OrdinalIgnoreCase));
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


      private static void CopyTo<T>(T source, T destination)
      {
         // Properties listed here should not be overwritten by the physical disk template.

         //var excludedProps = new[] {"PartitionNumber"};


         //var srcProps = typeof(T).GetProperties().Where(x => x.CanRead && x.CanWrite && !excludedProps.Any(prop => prop.Equals(x.Name))).ToArray();

         var srcProps = typeof(T).GetProperties().Where(x => x.CanRead && x.CanWrite).ToArray();

         var dstProps = srcProps.ToArray();


         foreach (var srcProp in srcProps)
         {
            var dstProp = dstProps.First(x => x.Name.Equals(srcProp.Name));

            dstProp.SetValue(destination, srcProp.GetValue(source, null), null);
         }
      }

      #endregion // Methods
   }
}
