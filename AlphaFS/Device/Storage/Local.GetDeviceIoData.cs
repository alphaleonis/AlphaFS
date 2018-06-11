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
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Security;
using Microsoft.Win32.SafeHandles;

namespace Alphaleonis.Win32.Device
{
   public static partial class Local
   {
      [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Object needs to be disposed by caller.")]
      [SecurityCritical]
      internal static SafeGlobalMemoryBufferHandle GetDeviceIoData<T>(SafeFileHandle safeHandle, NativeMethods.IoControlCode controlCode, string pathForException, int size = -1)
      {
         Utils.IsValidHandle(safeHandle);


         var bufferSize = size > -1 ? size : Marshal.SizeOf(typeof(T));

         while (true)
         {
            var safeBuffer = new SafeGlobalMemoryBufferHandle(bufferSize);

            var success = NativeMethods.DeviceIoControl(safeHandle, controlCode, IntPtr.Zero, 0, safeBuffer, (uint) safeBuffer.Capacity, IntPtr.Zero, IntPtr.Zero);

            var lastError = Marshal.GetLastWin32Error();


            if (success)
               return safeBuffer;


            using (safeBuffer)
            {
               if (lastError == Win32Errors.ERROR_NOT_READY ||

                   // Dynamic disk.
                   lastError == Win32Errors.ERROR_INVALID_FUNCTION ||

                   // Request device number from a DeviceGuid.Image device.
                   lastError == Win32Errors.ERROR_NOT_SUPPORTED)

                  return null;
            }

            bufferSize = Utils.GetDoubledBufferSizeOrThrowException(safeBuffer, lastError, bufferSize, pathForException);
         }
      }
   }
}
