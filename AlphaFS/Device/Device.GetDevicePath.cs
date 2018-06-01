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

using System.Globalization;

namespace Alphaleonis.Win32.Filesystem
{
   public static partial class Device
   {
      private static string GetDevicePath(string devicePath, out string logicalDrive)
      {
         bool isDrive;
         bool isVolume;
         bool isDeviceInfo;

         devicePath = ValidateDevicePath(devicePath, out isDrive, out isVolume, out isDeviceInfo);

         devicePath = string.Format(CultureInfo.InvariantCulture, "{0}{1}", isDrive ? Path.LogicalDrivePrefix : string.Empty, Path.RemoveTrailingDirectorySeparator(devicePath));

         if (isDrive)
         {
            logicalDrive = Path.GetRegularPathCore(devicePath, GetFullPathOptions.RemoveTrailingDirectorySeparator, false);

            logicalDrive = devicePath.Substring(Path.LogicalDrivePrefix.Length);
         }

         else
            logicalDrive = null;


         return devicePath;
      }
   }
}
