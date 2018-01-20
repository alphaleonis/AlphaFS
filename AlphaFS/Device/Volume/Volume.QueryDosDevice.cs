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
using System.Text;

namespace Alphaleonis.Win32.Filesystem
{
   public static partial class Volume
   {
      /// <summary>[AlphaFS] Retrieves a list of all existing MS-DOS device names.</summary>
      /// <returns>An <see cref="IEnumerable{String}"/> with one or more existing MS-DOS device names.</returns>
      [SecurityCritical]
      public static IEnumerable<string> QueryAllDosDevices()
      {
         return QueryDosDevice(null, null);
      }


      /// <summary>[AlphaFS] Retrieves a list of all existing MS-DOS device names.</summary>
      /// <param name="deviceName">
      ///   (Optional, default: <see langword="null"/>) An MS-DOS device name string specifying the target of the query. This parameter can be
      ///   "sort". In that case a sorted list of all existing MS-DOS device names is returned. This parameter can be <see langword="null"/>.
      ///   In that case, the <see cref="QueryDosDevice"/> function will store a list of all existing MS-DOS device names into the buffer.
      /// </param>
      /// <returns>An <see cref="IEnumerable{String}"/> with or more existing MS-DOS device names.</returns>
      [SecurityCritical]
      public static IEnumerable<string> QueryAllDosDevices(string deviceName)
      {
         return QueryDosDevice(null, deviceName);
      }


      /// <summary>[AlphaFS] 
      ///   Retrieves information about MS-DOS device names. The function can obtain the current mapping for a particular MS-DOS device name.
      ///   The function can also obtain a list of all existing MS-DOS device names.
      /// </summary>
      /// <param name="deviceName">
      ///   An MS-DOS device name string, or part of, specifying the target of the query. This parameter can be <see langword="null"/>. In that
      ///   case, the QueryDosDevice function will store a list of all existing MS-DOS device names into the buffer.
      /// </param>
      /// <param name="options">
      ///   (Optional, default: <see langword="false"/>) If options[0] = <see langword="true"/> a sorted list will be returned.
      /// </param>
      /// <returns>An <see cref="IEnumerable{String}"/> with one or more existing MS-DOS device names.</returns>
      [SecurityCritical]
      public static IEnumerable<string> QueryDosDevice(string deviceName, params string[] options)
      {
         // deviceName is allowed to be null.
         // The deviceName cannot have a trailing backslash.
         deviceName = Path.RemoveTrailingDirectorySeparator(deviceName);

         var searchFilter = deviceName != null;

         // Only process options if a device is supplied.
         if (searchFilter)
         {
            // Check that at least one "options[]" has something to say. If so, rebuild them.
            options = options != null && options.Any() ? new[] { deviceName, options[0] } : new[] { deviceName, string.Empty };

            deviceName = null;
         }

         // Choose sorted output.
         var doSort = options != null && options.Any(s => s != null && s.Equals("sort", StringComparison.OrdinalIgnoreCase));

         // Start with a larger buffer when using a searchFilter.
         var bufferSize = (uint)(searchFilter || doSort || null == options ? 8 * NativeMethods.DefaultFileBufferSize : 256);
         uint bufferResult = 0;

         using (new NativeMethods.ChangeErrorMode(NativeMethods.ErrorMode.FailCriticalErrors))
            while (bufferResult == 0)
            {
               var cBuffer = new char[bufferSize];

               // QueryDosDevice()
               // 2014-01-29: MSDN does not confirm LongPath usage but a Unicode version of this function exists.

               bufferResult = NativeMethods.QueryDosDevice(deviceName, cBuffer, bufferSize);
               var lastError = Marshal.GetLastWin32Error();

               if (bufferResult == 0)
                  switch ((uint)lastError)
                  {
                     case Win32Errors.ERROR_MORE_DATA:
                     case Win32Errors.ERROR_INSUFFICIENT_BUFFER:
                        bufferSize *= 2;
                        continue;

                     default:
                        NativeError.ThrowException(lastError, deviceName);
                        break;
                  }

               var dosDev = new List<string>();
               var buffer = new StringBuilder();

               for (var i = 0; i < bufferResult; i++)
               {
                  if (cBuffer[i] != Path.StringTerminatorChar)
                     buffer.Append(cBuffer[i]);

                  else if (buffer.Length > 0)
                  {
                     dosDev.Add(buffer.ToString());
                     buffer.Length = 0;
                  }
               }

               // Choose the yield back query; filtered or list.
               var selectQuery = searchFilter
                  ? dosDev.Where(dev => options != null && dev.StartsWith(options[0], StringComparison.OrdinalIgnoreCase))
                  : dosDev;

               foreach (var dev in doSort ? selectQuery.OrderBy(n => n) : selectQuery)
                  yield return dev;
            }
      }
   }
}
