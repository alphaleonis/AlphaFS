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

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security;

namespace Alphaleonis.Win32.Filesystem
{
   public static partial class Device
   {
      /// <summary>[AlphaFS] Enumerates the physical drives on the Computer.</summary>
      /// <returns>An IEnumerable of type <see cref="PhysicalDriveInfo"/> that represents the physical drives on the Computer.</returns>      
      [SecurityCritical]
      public static IEnumerable<PhysicalDriveInfo> EnumeratePhysicalDrives()
      {
         var physicalDrives = EnumerateDevicesCore(null, DeviceGuid.Disk, false).Select(deviceInfo => GetPhysicalDriveInfoCore(null, deviceInfo)).Where(physicalDrive => null != physicalDrive).OrderBy(physicalDrive => physicalDrive.DeviceNumber).ToArray();

         //var volumes = Volume.EnumerateVolumes().Select(volume => GetPhysicalDriveInfoCore(volume, null, false)).Where(physicalDrive => null != physicalDrive).OrderBy(physicalDrive => physicalDrive.DeviceNumber).ThenBy(physicalDrive => physicalDrive.PartitionNumber).ToArray();

         //var logicalDrives = DriveInfo.EnumerateLogicalDrivesCore(false, false).Select(driveName => GetPhysicalDriveInfoCore(driveName, null, false)).Where(physicalDrive => null != physicalDrive).OrderBy(physicalDrive => physicalDrive.DeviceNumber).ThenBy(physicalDrive => physicalDrive.PartitionNumber).ToArray();


         //var populatedPhysicalDrives = new Collection<PhysicalDriveInfo>();


         //foreach (var volume in volumes)
         //{
         //   foreach (var pDrive in physicalDrives.Where(pDrive => pDrive.DeviceNumber == volume.DeviceNumber))
         //   {
         //      CopyTo(pDrive, volume);


         //      foreach (var lDrive in Volume.EnumerateVolumePathNames(volume.DevicePath).Where(drive => !Utils.IsNullOrWhiteSpace(drive) && Path.IsLogicalDriveCore(drive, PathFormat.LongFullPath)))
         //      {
         //         if (null == volume.DriveInfo)
         //            volume.DriveInfo = new Collection<DriveInfo>();

         //         volume.DriveInfo.Add(new DriveInfo(lDrive));
         //      }


         //      volume.VolumeGuids = new[] {volume.DevicePath};

         //      populatedPhysicalDrives.Add(volume);

         //      break;
         //   }
         //}


         //return populatedPhysicalDrives.OrderBy(pDriveInfo => pDriveInfo.DeviceNumber).ThenBy(pDriveInfo => pDriveInfo.PartitionNumber);

         return physicalDrives.OrderBy(pDriveInfo => pDriveInfo.DeviceNumber).ThenBy(pDriveInfo => pDriveInfo.PartitionNumber);
      }


      private static void CopyTo<T>(T source, T destination)
      {
         // Properties listed here should not be overwritten by the physical drive template.

         var excludedProps = new[] {"PartitionNumber"};


         var srcProps = typeof(T).GetProperties().Where(x => x.CanRead && x.CanWrite && !excludedProps.Any(prop => prop.Equals(x.Name))).ToArray();

         var dstProps = srcProps.ToArray();


         foreach (var srcProp in srcProps)
         {
            var dstProp = dstProps.First(x => x.Name.Equals(srcProp.Name));

            dstProp.SetValue(destination, srcProp.GetValue(source, null), null);
         }
      }
   }
}
