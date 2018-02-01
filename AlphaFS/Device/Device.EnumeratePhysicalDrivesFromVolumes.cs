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
      /// <summary>[AlphaFS] Enumerates the volumes and associated physical drives on the Computer.</summary>
      /// <returns>An IEnumerable of type <see cref="PhysicalDriveInfo"/> that represents the physical drives on the Computer.</returns>      
      [SecurityCritical]
      public static IEnumerable<PhysicalDriveInfo> EnumeratePhysicalDrivesFromVolumes()
      {
         var physicalDrives = EnumeratePhysicalDrives().ToArray();

         var volumes = Volume.EnumerateVolumes();

         //var volumes = EnumerateDevicesCore(null, DeviceGuid.Volume, false)

         //   .Select(deviceInfo => GetPhysicalDriveInfoCore(null, deviceInfo, false))

         //   .Where(physicalDrive => null != physicalDrive)

         //   .OrderBy(disk => disk.DeviceNumber).ThenBy(disk => disk.PartitionNumber).ToArray();



         var allDrives = new Collection<PhysicalDriveInfo>();


         foreach (var volume in volumes)
         {
            var pDriveInfo = GetPhysicalDriveInfoCore(volume, null, false);

            if (null == pDriveInfo)
               continue;


            foreach (var physicalDrive in physicalDrives.Where(pd => pd.DeviceNumber == pDriveInfo.DeviceNumber))
            {
               physicalDrive.CopyTo(pDriveInfo);


               foreach (var drive in Volume.EnumerateVolumePathNames(volume))
               {
                  // Get the first entry that starts with a logical drive path, such as: "C:", "D:".

                  if (!Utils.IsNullOrWhiteSpace(drive) && Path.IsLogicalDriveCore(drive, PathFormat.LongFullPath))
                  {
                     pDriveInfo.DriveInfo = new DriveInfo(drive);

                     break;
                  }
               }


               pDriveInfo.VolumeGuids = new[] {volume};

               allDrives.Add(pDriveInfo);

               break;
            }
         }


         return allDrives.OrderBy(pDriveInfo => pDriveInfo.DeviceNumber).ThenBy(pDriveInfo => pDriveInfo.PartitionNumber);
      }
   }
}
