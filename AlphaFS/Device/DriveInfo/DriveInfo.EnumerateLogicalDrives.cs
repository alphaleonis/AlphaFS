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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;

namespace Alphaleonis.Win32.Filesystem
{
   public sealed partial class DriveInfo
   {
      /// <summary>Enumerates the drive names of all logical drives on the Computer.</summary>
      /// <returns>An IEnumerable of type <see cref="string"/> that represents the logical drives on the Computer.</returns>
      /// <param name="fromEnvironment">Retrieve logical drives as known by the Environment.</param>
      /// <param name="isReady">Retrieve only when accessible (IsReady) logical drives.</param>
      [SecurityCritical]
      internal static IEnumerable<string> EnumerateLogicalDrivesCore(bool fromEnvironment, bool isReady)
      {
         // Get from Environment.

         if (fromEnvironment)
         {
            var drivesEnv = isReady
               ? Environment.GetLogicalDrives().Where(logicalDrive => File.ExistsCore(null, true, logicalDrive, PathFormat.LongFullPath))
               : Environment.GetLogicalDrives().Select(logicalDrive => logicalDrive);

            foreach (var drive in drivesEnv)
            {
               // Optionally check Drive .IsReady.
               if (isReady)
               {
                  if (File.ExistsCore(null, true, drive, PathFormat.LongFullPath))

                     yield return Path.AddTrailingDirectorySeparator(drive, false);
               }

               else
                  yield return Path.AddTrailingDirectorySeparator(drive, false);
            }

            yield break;
         }


         // Get through NativeMethod.
         var allDrives = NativeMethods.GetLogicalDrives();

         var lastError = Marshal.GetLastWin32Error();

         // MSDN: GetLogicalDrives(): If the function fails, the return value is zero.
         if (allDrives == Win32Errors.ERROR_SUCCESS)
            NativeError.ThrowException(lastError);


         var drives = allDrives;
         var count = 0;
         while (drives != 0)
         {
            if ((drives & 1) != 0)
               ++count;

            drives >>= 1;
         }

         var result = new string[count];
         char[] root = {'A', Path.VolumeSeparatorChar};

         drives = allDrives;
         count = 0;

         while (drives != 0)
         {
            if ((drives & 1) != 0)
            {
               var drive = new string(root);

               if (isReady)
               {
                  // Optionally check Drive .IsReady property.

                  if (File.ExistsCore(null, true, drive, PathFormat.LongFullPath))

                     yield return Path.AddTrailingDirectorySeparator(drive, false);
               }

               // Ready or not.
               else
                  yield return Path.AddTrailingDirectorySeparator(drive, false);

               result[count++] = drive;
            }

            drives >>= 1;
            root[0]++;
         }
      }
   }
}
