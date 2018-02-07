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

using System.Globalization;
using System.Runtime.InteropServices;
using System.Security;

namespace Alphaleonis.Win32.Filesystem
{
   /// <summary>[AlphaFS] Provides static methods to retrieve device resource information from a local or remote host.</summary>
   public static partial class Device
   {
      /// <summary>MAXIMUM_REPARSE_DATA_BUFFER_SIZE = 16384</summary>
      private const int MAXIMUM_REPARSE_DATA_BUFFER_SIZE = 16384;

      /// <summary>REPARSE_DATA_BUFFER_HEADER_SIZE = 8</summary>
      private const int REPARSE_DATA_BUFFER_HEADER_SIZE = 8;


      [SecurityCritical]
      internal static int GetDoubledBufferSizeOrThrowException(int lastError, SafeHandle safeBuffer, int bufferSize, string pathForException)
      {
         if (null != safeBuffer && !safeBuffer.IsClosed)
            safeBuffer.Close();


         switch ((uint) lastError)
         {
            case Win32Errors.ERROR_MORE_DATA:
            case Win32Errors.ERROR_INSUFFICIENT_BUFFER:
               bufferSize *= 2;
               break;


            default:
               NativeMethods.IsValidHandle(safeBuffer, lastError, string.Format(CultureInfo.InvariantCulture, "Buffer size: {0}. Path: {1}", bufferSize.ToString(CultureInfo.InvariantCulture), pathForException));
               break;
         }


         return bufferSize;
      }
   }
}
