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
using System.Globalization;
using Alphaleonis.Win32.Filesystem;

namespace Alphaleonis.Win32.Device
{
   internal static partial class FilesystemHelper
   {
      /// <summary>Determines and retrieves the <see cref="DeviceInfo.DevicePath"/> such as: <c>\\.\C:</c> or <c>\\?\scsi#disk...{xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}</c></summary>
      /// <returns>Returns the <see cref="DeviceInfo.DevicePath"/> path string such as: <c>\\.\C:</c> or <c>\\?\scsi#disk...{xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}</c></returns>
      /// <param name="devicePath">
      /// <para>A disk path such as: <c>\\.\PhysicalDrive0</c></para>
      /// <para>A drive path such as: <c>C</c>, <c>C:</c> or <c>C:\</c></para>
      /// <para>A volume <see cref="Guid"/> such as: <c>\\?\Volume{xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}\</c></para>
      /// <para>A <see cref="DeviceInfo.DevicePath"/> string such as: <c>\\?\scsi#disk...{xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}</c></para>
      /// </param>
      internal static string GetLocalDevicePath(string devicePath)
      {
         string unusedLogicalDrive;

         return GetLocalDevicePath(devicePath, out unusedLogicalDrive);
      }


      /// <summary>Determines and retrieves the <see cref="DeviceInfo.DevicePath"/> such as: <c>\\.\C:</c> or <c>\\?\scsi#disk...{xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}</c></summary>
      /// <returns>Returns the <see cref="DeviceInfo.DevicePath"/> path string such as: <c>\\.\C:</c> or <c>\\?\scsi#disk...{xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}</c></returns>
      /// <param name="devicePath">
      /// <para>A disk path such as: <c>\\.\PhysicalDrive0</c></para>
      /// <para>A drive path such as: <c>C</c>, <c>C:</c> or <c>C:\</c></para>
      /// <para>A volume <see cref="Guid"/> such as: <c>\\?\Volume{xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}\</c></para>
      /// <para>A <see cref="DeviceInfo.DevicePath"/> string such as: <c>\\?\scsi#disk...{xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}</c></para>
      /// </param>
      /// <param name="logicalDrive">If <paramref name="devicePath"/> is a logical drive, it is returned in <paramref name="logicalDrive"/></param>
      internal static string GetLocalDevicePath(string devicePath, out string logicalDrive)
      {
         bool isDrive;
         bool isVolume;
         bool isDeviceInfo;

         var validatedDevicePath = GetValidatedDevicePath(devicePath, out isDrive, out isVolume, out isDeviceInfo);

         if (!validatedDevicePath.StartsWith(Path.LogicalDrivePrefix, StringComparison.OrdinalIgnoreCase))

            validatedDevicePath = string.Format(CultureInfo.InvariantCulture, "{0}{1}", isDrive ? Path.LogicalDrivePrefix : string.Empty, Path.RemoveTrailingDirectorySeparator(validatedDevicePath));


         if (isDrive)
         {
            logicalDrive = Path.GetRegularPathCore(validatedDevicePath, GetFullPathOptions.RemoveTrailingDirectorySeparator, false);

            logicalDrive = validatedDevicePath.Substring(Path.LogicalDrivePrefix.Length);
         }

         else
            logicalDrive = null;


         return validatedDevicePath;
      }
   }
}
