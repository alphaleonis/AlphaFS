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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security;
using Microsoft.Win32.SafeHandles;

namespace Alphaleonis.Win32.Device
{
   public static partial class Local
   {
      /// <returns>Returns an <see cref="IEnumerable{Int}"/> of physical drive device numbers used by the specified <paramref name="localDevicePath"/> volume.</returns>
      /// <exception cref="Exception"/>
      /// <param name="safeFileHandle">An initialized <see cref="SafeFileHandle"/> instance.</param>
      /// <param name="localDevicePath">
      ///    <para>A drive path such as: <c>\\.\C:</c></para>
      ///    <para>A volume <see cref="Guid"/> such as: <c>\\?\Volume{xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}\</c></para>
      /// </param>
      [SecurityCritical]
      private static IEnumerable<int> GetDeviceNumbersForVolume(SafeFileHandle safeFileHandle, string localDevicePath)
      {
         var physicalDrives = new Collection<int>();

         var disposeHandle = null == safeFileHandle;
         
         try
         {
            if (null == safeFileHandle)
               safeFileHandle = OpenDevice(localDevicePath, NativeMethods.FILE_ANY_ACCESS);
            
            var volDiskExtents = GetVolumeDiskExtents(safeFileHandle, localDevicePath);

            if (volDiskExtents.HasValue)

               foreach (var extent in volDiskExtents.Value.Extents)

                  physicalDrives.Add((int) extent.DiskNumber);
         }
         finally
         {
            if (disposeHandle && null != safeFileHandle && !safeFileHandle.IsClosed)
               safeFileHandle.Close();
         }

         return physicalDrives;
      }
   }
}
