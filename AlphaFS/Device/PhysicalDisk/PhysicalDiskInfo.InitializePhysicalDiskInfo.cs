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
using System.Security;
using Alphaleonis.Win32.Filesystem;

namespace Alphaleonis.Win32.Device
{
   public sealed partial class PhysicalDiskInfo
   {
      /// <summary>[AlphaFS] Initializes a <see cref="PhysicalDiskInfo"/> instance that is related to the logical drive name, volume <see cref="Guid"/> or <see cref="DeviceInfo.DevicePath"/>.</summary>
      ///  <returns>Returns a <see cref="PhysicalDiskInfo"/> instance that represents the physical disk on the Computer or <c>null</c> if the device can not be retrieved.</returns>
      /// <remarks>Use either <paramref name="devicePath"/> or <paramref name="deviceInfo"/>, not both.</remarks>
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
      /// <param name="storageDeviceInfo">An initialized <see cref="StorageDeviceInfo"/> instance.</param>
      /// <param name="deviceInfo">An initialized <see cref="DeviceInfo"/> instance.</param>
      [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Object is disposed.")]
      [SecurityCritical]
      internal static PhysicalDiskInfo InitializePhysicalDiskInfo(bool isElevated, string devicePath, StorageDeviceInfo storageDeviceInfo, DeviceInfo deviceInfo)
      {
         if (null == devicePath && null == deviceInfo)
            return null;

         var isDevice = null != deviceInfo && !Utils.IsNullOrWhiteSpace(deviceInfo.DevicePath);

         if (isDevice)
            devicePath = deviceInfo.DevicePath;

         return new PhysicalDiskInfo
         {
            DevicePath = devicePath,

            DeviceDescription = isDevice ? deviceInfo.DeviceDescription : null,

            Name = isDevice ? deviceInfo.FriendlyName : null,

            PhysicalDeviceObjectName = isDevice ? deviceInfo.PhysicalDeviceObjectName : null,


            StorageAdapterInfo = Local.GetStorageAdapterInfoCore(devicePath, deviceInfo),

            StorageDeviceInfo = null != storageDeviceInfo ? storageDeviceInfo : Local.GetStorageDeviceInfoCore(isElevated, -1, devicePath),

            StoragePartitionInfo = Local.GetStoragePartitionInfo(isElevated, devicePath)
         };
      }
   }
}
