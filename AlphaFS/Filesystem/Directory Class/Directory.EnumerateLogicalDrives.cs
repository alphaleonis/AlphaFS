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
using System.Security;

namespace Alphaleonis.Win32.Filesystem
{
   partial class Directory
   {
      /// <summary>[AlphaFS] Enumerates the drive names of all logical drives on the Computer with the ready status.</summary>
      /// <returns>An IEnumerable of type <see cref="DriveInfo"/> that represents the logical drives on the Computer.</returns>
      [SecurityCritical]
      [Obsolete("Use DriveInfo.GetDrives()")]
      public static IEnumerable<DriveInfo> EnumerateLogicalDrives()
      {
         return DriveInfo.EnumerateLogicalDrivesCore(false, false).OrderBy(driveName => driveName).Select(driveName => new DriveInfo(driveName));
      }


      /// <summary>[AlphaFS] Enumerates the drive names of all logical drives on the Computer.</summary>
      /// <returns>An IEnumerable of type <see cref="DriveInfo"/> that represents the logical drives on the Computer.</returns>
      /// <param name="fromEnvironment">Retrieve logical drives as known by the Environment.</param>
      /// <param name="isReady">Retrieve only when accessible (IsReady) logical drives.</param>
      [SecurityCritical]
      [Obsolete("Use DriveInfo.GetDrives()")]
      public static IEnumerable<DriveInfo> EnumerateLogicalDrives(bool fromEnvironment, bool isReady)
      {
         return DriveInfo.EnumerateLogicalDrivesCore(fromEnvironment, isReady).OrderBy(driveName => driveName).Select(driveName => new DriveInfo(driveName));
      }
   }
}
