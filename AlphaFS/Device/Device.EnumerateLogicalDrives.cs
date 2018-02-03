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
      /// <summary>[AlphaFS] Enumerates the logical drives and associated physical drives on the Computer.</summary>
      /// <returns>An IEnumerable of type <see cref="PhysicalDriveInfo"/> that represents the physical drives on the Computer.</returns>      
      [SecurityCritical]
      public static IEnumerable<PhysicalDriveInfo> EnumerateLogicalDrives()
      {
         //var pDrives = new Collection<PhysicalDriveInfo>();

         //var logicalDrives = DriveInfo.EnumerateLogicalDrivesCore(false, false).OrderBy(driveName => driveName).ToArray();


         //foreach (var pDrive in EnumeratePhysicalDrives().Where(drive => null != drive.DriveInfo))
         //{
         //   foreach (var driveInfo in pDrive.DriveInfo)
         //   {
         //      pDrives.Add(pDrive);
         //   }
         //}


         //return pDrives;


         var physicalDrives = EnumeratePhysicalDrives().ToArray();

         var logicalDrives = DriveInfo.EnumerateLogicalDrivesCore(false, false).OrderBy(driveName => driveName).ToArray();


         foreach (var drive in logicalDrives)
         {
            var pDriveInfo = GetPhysicalDriveInfoCore(drive, null, false);

            if (null == pDriveInfo)
               continue;


            foreach (var pDrive in physicalDrives.Where(pDrive => pDrive.DeviceNumber == pDriveInfo.DeviceNumber))
            {
               CopyTo(pDrive, pDriveInfo);


               // Get the first entry that starts with a logical drive path, such as: "C:", "D:".

               if (Path.IsLogicalDriveCore(drive, PathFormat.LongFullPath))
               {
                  if (null == pDriveInfo.DriveInfo)
                     pDriveInfo.DriveInfo= new Collection<DriveInfo>();


                  pDriveInfo.DriveInfo.Add(new DriveInfo(drive));


                  var guid = Volume.GetVolumeGuid(drive);

                  if (!Utils.IsNullOrWhiteSpace(guid))
                     pDriveInfo.VolumeGuids = new[] {guid};
               }


               yield return pDriveInfo;

               break;
            }
         }
      }
   }
}
