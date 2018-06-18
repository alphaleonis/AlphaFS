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

using System.Collections.Generic;
using System.Security;
using Alphaleonis.Win32.Security;

namespace Alphaleonis.Win32.Device
{
   public static partial class Local
   {
      /// <summary>[AlphaFS] Enumerates the physical disks (including CD-ROM/DVD devices) on the Computer, populated with volume/logical drive information.</summary>
      /// <returns>Returns an <see cref="IEnumerable{PhysicalDiskInfo}"/> collection that represents the physical disks on the Computer.</returns>
      [SecurityCritical]
      public static IEnumerable<PhysicalDiskInfo> EnumeratePhysicalDisks()
      {
         return EnumeratePhysicalDisksCore(ProcessContext.IsElevatedProcess, -1);
      }


      /// <summary>[AlphaFS] Enumerates the physical disks (including CD-ROM/DVD devices) on the Computer, populated with volume/logical drive information.</summary>
      /// <returns>Returns an <see cref="IEnumerable{PhysicalDiskInfo}"/> collection that represents the physical disks on the Computer.</returns>
      /// <param name="deviceNumber">Retrieve <see cref="PhysicalDiskInfo"/> instances by device number.</param>
      [SecurityCritical]
      public static IEnumerable<PhysicalDiskInfo> EnumeratePhysicalDisks(int deviceNumber)
      {
         return EnumeratePhysicalDisksCore(ProcessContext.IsElevatedProcess, deviceNumber);
      }


      /// <summary>[AlphaFS] Enumerates the physical disks (including CD-ROM/DVD devices) on the Computer, populated with volume/logical drive information.</summary>
      /// <returns>Returns an <see cref="IEnumerable{PhysicalDiskInfo}"/> collection that represents the physical disks on the Computer.</returns>
      /// <param name="isElevated"><c>true</c> indicates the current process is in an elevated state, allowing to retrieve more data.</param>
      /// <param name="deviceNumber">Retrieve a <see cref="PhysicalDiskInfo"/> instance by device number.</param>
      [SecurityCritical]
      internal static IEnumerable<PhysicalDiskInfo> EnumeratePhysicalDisksCore(bool isElevated, int deviceNumber)
      {
         foreach (var deviceInfo in EnumerateDevicesCore(null, new []{DeviceGuid.Disk, DeviceGuid.CDRom, DeviceGuid.Wpd, DeviceGuid.WpdPrivate}, false))
         {
            string unusedPhysicalDriveNumberPath;


            // The StorageDeviceInfo is always needed as it contains the device- and partition number.

            var storageDeviceInfo = GetStorageDeviceInfo(isElevated, true, deviceNumber, deviceInfo.DevicePath, out unusedPhysicalDriveNumberPath);

            if (null != storageDeviceInfo)
               yield return new PhysicalDiskInfo(deviceNumber, storageDeviceInfo, deviceInfo);
         }
      }
   }
}
