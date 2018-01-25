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
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security;

namespace Alphaleonis.Win32.Filesystem
{
   public static partial class Volume
   {
      /// <summary>[AlphaFS] Retrieves the Win32 Device name from the Volume name.</summary>
      /// <exception cref="ArgumentNullException"/>
      /// <param name="volumeName">Name of the Volume.</param>
      /// <returns>
      ///   The Win32 Device name from the Volume name (for example: "\Device\HarddiskVolume2"), or <see langword="null"/> on error or if
      ///   unavailable.
      /// </returns>
      [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
      [SecurityCritical]
      public static string GetVolumeDeviceName(string volumeName)
      {
         if (Utils.IsNullOrWhiteSpace(volumeName))
            throw new ArgumentNullException("volumeName");


         bool doQueryDos;
         volumeName = Path.RemoveTrailingDirectorySeparator(volumeName);


         // GlobalRoot

         if (volumeName.StartsWith(Path.GlobalRootPrefix, StringComparison.OrdinalIgnoreCase))
            return volumeName.Substring(Path.GlobalRootPrefix.Length);


         

         // Volume

         if (volumeName.StartsWith(Path.VolumePrefix, StringComparison.OrdinalIgnoreCase))
         {
            // Isolate the DOS Device from the Volume name, in the format: Volume{GUID}
            volumeName = volumeName.Substring(Path.LongPathPrefix.Length);

            doQueryDos = true;
         }


         // Logical Drive

         else
            doQueryDos = Path.IsLogicalDriveCore(volumeName, PathFormat.LongFullPath);


         if (doQueryDos)
         {
            try
            {
               // Get the real Device underneath.

               var dev = QueryDosDevice(volumeName).FirstOrDefault();

               return !Utils.IsNullOrWhiteSpace(dev) ? dev : null;
            }
            catch
            {
            }
         }

         return null;
      }
   }
}
