/*  Copyright (C) 2008-2016 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
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
using System.Security;

namespace Alphaleonis.Win32.Filesystem
{
   partial class Directory
   {
      #region .NET

      /// <summary>Retrieves the names of the logical drives on this computer in the form "&lt;drive letter&gt;:\".</summary>
      /// <returns>An array of type <see cref="String"/> that represents the logical drives on a computer.</returns>
      [SecurityCritical]
      public static string[] GetLogicalDrives()
      {
         return EnumerateLogicalDrivesCore(false, false).Select(drive => drive.Name).ToArray();
      }

      #endregion // .NET
      
      /// <summary>[AlphaFS] Retrieves the names of the logical drives on this computer in the form "&lt;drive letter&gt;:\".</summary>
      /// <returns>An array of type <see cref="String"/> that represents the logical drives on a computer.</returns>
      /// <param name="fromEnvironment">Retrieve logical drives as known by the Environment.</param>
      /// <param name="isReady">Retrieve only when accessible (IsReady) logical drives.</param>
      [SecurityCritical]
      public static string[] GetLogicalDrives(bool fromEnvironment, bool isReady)
      {
         return EnumerateLogicalDrivesCore(fromEnvironment, isReady).Select(drive => drive.Name).ToArray();
      }


      /// <summary>[AlphaFS] Enumerates the drive names of all logical drives on a computer.</summary>
      /// <returns>An IEnumerable of type <see cref="Alphaleonis.Win32.Filesystem.DriveInfo"/> that represents the logical drives on a computer.</returns>
      /// <param name="fromEnvironment">Retrieve logical drives as known by the Environment.</param>
      /// <param name="isReady">Retrieve only when accessible (IsReady) logical drives.</param>
      [SecurityCritical]
      public static IEnumerable<DriveInfo> EnumerateLogicalDrives(bool fromEnvironment, bool isReady)
      {
         return EnumerateLogicalDrivesCore(fromEnvironment, isReady);
      }

      #region Internal Methods

      /// <summary>Enumerates the drive names of all logical drives on a computer.</summary>
      /// <returns>An IEnumerable of type <see cref="Alphaleonis.Win32.Filesystem.DriveInfo"/> that represents the logical drives on a computer.</returns>
      /// <param name="fromEnvironment">Retrieve logical drives as known by the Environment.</param>
      /// <param name="isReady">Retrieve only when accessible (IsReady) logical drives.</param>
      [SecurityCritical]
      internal static IEnumerable<DriveInfo> EnumerateLogicalDrivesCore(bool fromEnvironment, bool isReady)
      {
         #region Get from Environment

         if (fromEnvironment)
         {
            IEnumerable<string> drivesEnv = isReady
               ? Environment.GetLogicalDrives().Where(ld => File.ExistsCore(true, null, ld, PathFormat.FullPath))
               : Environment.GetLogicalDrives().Select(ld => ld);

            foreach (string drive in drivesEnv)
            {
               // Optionally check Drive .IsReady.
               if (isReady)
               {
                  if (File.ExistsCore(true, null, drive, PathFormat.FullPath))
                     yield return new DriveInfo(drive);
               }
               else
                  yield return new DriveInfo(drive);
            }

            yield break;
         }

         #endregion // Get from Environment

         #region Get through NativeMethod

         uint lastError = NativeMethods.GetLogicalDrives();
         if (lastError == Win32Errors.ERROR_SUCCESS)
            NativeError.ThrowException((int)lastError);

         uint drives = lastError;
         int count = 0;
         while (drives != 0)
         {
            if ((drives & 1) != 0)
               ++count;

            drives >>= 1;
         }

         string[] result = new string[count];
         char[] root = { 'A', Path.VolumeSeparatorChar };

         drives = lastError;
         count = 0;

         while (drives != 0)
         {
            if ((drives & 1) != 0)
            {
               string drive = new string(root);

               if (isReady)
               {
                  // Optionally check Drive .IsReady.
                  if (File.ExistsCore(true, null, drive, PathFormat.FullPath))
                     yield return new DriveInfo(drive);
               }
               else
               {
                  // Ready or not.
                  yield return new DriveInfo(drive);
               }

               result[count++] = drive;
            }

            drives >>= 1;
            root[0]++;
         }

         #endregion // Get through NativeMethod
      }

      #endregion // Internal Methods
   }
}
