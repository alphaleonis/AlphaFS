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
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace Alphaleonis.Win32.Filesystem
{
   public static partial class Volume
   {
      /// <summary>[AlphaFS] Returns an <see cref="IEnumerable{String}"/> collection of drive letters and mounted folder paths for the specified volume.</summary>
      /// <returns>RReturns an <see cref="IEnumerable{String}"/> collection of path names for the specified volume.</returns>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="ArgumentException"/>
      /// <param name="volumeGuid">A volume <see cref="Guid"/> path: <c>\\?\Volume{xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}\</c></param>
      [SecurityCritical]
      public static IEnumerable<string> EnumerateVolumePathNames(string volumeGuid)
      {
         if (Utils.IsNullOrWhiteSpace(volumeGuid))
            throw new ArgumentNullException("volumeGuid");


         if (!volumeGuid.StartsWith(Path.VolumePrefix + "{", StringComparison.OrdinalIgnoreCase))
            throw new ArgumentException(Resources.Not_A_Valid_Guid, "volumeGuid");


         uint bufferSize = 50;
         var buffer = new char[bufferSize];

         volumeGuid = Path.AddTrailingDirectorySeparator(volumeGuid, false);


         using (new NativeMethods.ChangeErrorMode(NativeMethods.ErrorMode.FailCriticalErrors))
            while (!NativeMethods.GetVolumePathNamesForVolumeName(volumeGuid, buffer, (uint) buffer.Length, out bufferSize))
            {
               var lastError = Marshal.GetLastWin32Error();

               switch ((uint) lastError)
               {
                  case Win32Errors.ERROR_MORE_DATA:
                  case Win32Errors.ERROR_INSUFFICIENT_BUFFER:
                     buffer = new char[bufferSize];
                     break;

                  default:
                     NativeError.ThrowException(lastError, volumeGuid);
                     break;
               }
            }


         var pathNameBuffer = new StringBuilder(buffer.Length);

         foreach (var c in buffer)
         {
            if (c != Path.StringTerminatorChar)

               pathNameBuffer.Append(c);

            else
            {
               if (pathNameBuffer.Length > 0)
               {
                  yield return pathNameBuffer.ToString();

                  pathNameBuffer.Length = 0;
               }
            }
         }
      }
   }
}
