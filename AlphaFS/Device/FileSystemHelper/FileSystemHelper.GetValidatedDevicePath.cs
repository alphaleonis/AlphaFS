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
using Alphaleonis.Win32.Filesystem;

namespace Alphaleonis.Win32.Device
{
   internal static partial class FileSystemHelper
   {
      /// <summary>Determines and retrieves the type of device path.</summary>
      /// <returns>The final device path with a <see cref="Path.DirectorySeparator"/>.</returns>
      internal static string GetValidatedDevicePath(string devicePath, out bool isDrive, out bool isVolume, out bool isDevice)
      {
         if (null == devicePath)
            throw new ArgumentNullException("devicePath");

         if (devicePath.Trim().Length == 0)
            throw new ArgumentException(Resources.Path_Is_Zero_Length_Or_Only_White_Space, "devicePath");


         if (devicePath.StartsWith(Path.PhysicalDrivePrefix, StringComparison.OrdinalIgnoreCase))
         {
            isDrive = false;
            isVolume = false;
            isDevice = false;

            return devicePath;
         }


         // Resolve single drive letter or get root directory information.

         devicePath = devicePath.Length == 1 ? devicePath + Path.VolumeSeparatorChar : Path.GetPathRoot(devicePath, false);
         
         var hasPath = !Utils.IsNullOrWhiteSpace(devicePath);


         isVolume = hasPath && devicePath.StartsWith(Path.VolumePrefix + "{", StringComparison.OrdinalIgnoreCase);

         isDevice = hasPath && !isVolume && devicePath.StartsWith(Path.LongPathPrefix, StringComparison.Ordinal);

         isDrive = hasPath && !isDevice && !isVolume && Path.IsLogicalDriveCore(devicePath, false, PathFormat.LongFullPath);



         if (!hasPath || !isVolume && !isDevice && !isDrive)
            throw new ArgumentException(Resources.Argument_must_be_DriveLetter_or_VolumeGuid_or_DevicePath, "devicePath");


         return Path.AddTrailingDirectorySeparator(devicePath, false);
      }
   }
}
