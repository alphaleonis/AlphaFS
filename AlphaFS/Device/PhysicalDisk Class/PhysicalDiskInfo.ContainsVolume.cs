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
using System.Linq;
using Alphaleonis.Win32.Filesystem;

namespace Alphaleonis.Win32.Device
{
   public sealed partial class PhysicalDiskInfo
   {
      /// <summary>Checks if the volume/logical drive is located on the physical disk.
      /// <para>A drive path such as: <c>C</c>, <c>C:</c> or <c>C:\</c></para>
      /// <para>A volume <see cref="Guid"/> such as: <c>\\?\Volume{xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}\</c></para>
      /// </summary>
      /// <returns><c>true</c> if the volume/logical drive is located on the physical disk; otherwise, <c>false</c>.</returns>
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

         devicePath = FilesystemHelper.GetValidatedDevicePath(devicePath, out isDrive, out isVolume, out isDeviceInfo);


         if (isDrive && null != LogicalDrives)
         {
            devicePath = devicePath.Replace(Path.LogicalDrivePrefix, string.Empty);

            devicePath = Path.RemoveTrailingDirectorySeparator(devicePath, false);

            return LogicalDrives.Any(driveName => driveName.Equals(devicePath, StringComparison.OrdinalIgnoreCase));
         }


         return isVolume && null != VolumeGuids && VolumeGuids.Any(guid => guid.Equals(devicePath, StringComparison.OrdinalIgnoreCase));
      }
   }
}
