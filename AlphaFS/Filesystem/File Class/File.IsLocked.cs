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
using System.IO;
using System.Runtime.InteropServices;
using System.Security;

namespace Alphaleonis.Win32.Filesystem
{
   partial class File
   {
      /// <summary>[AlphaFS] Determines whether the specified file is in use (locked).</summary>
      /// <returns>Returns <see langword="true"/> if the specified file is in use (locked); otherwise, <see langword="false"/></returns>
      /// <exception cref="IOException"/>
      /// <exception cref="Exception"/>
      /// <param name="path">The file to check.</param>
      [SecurityCritical]
      public static bool IsLocked(string path)
      {
         return IsLockedCore(null, path, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Determines whether the specified file is in use (locked).</summary>
      /// <returns>Returns <see langword="true"/> if the specified file is in use (locked); otherwise, <see langword="false"/></returns>
      /// <exception cref="FileNotFoundException"></exception>
      /// <exception cref="IOException"/>
      /// <exception cref="Exception"/>
      /// <param name="path">The file to check.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static bool IsLocked(string path, PathFormat pathFormat)
      {
         return IsLockedCore(null, path, pathFormat);
      }

      #region Transactional

      /// <summary>[AlphaFS] Determines whether the specified file is in use (locked).</summary>
      /// <returns>Returns <see langword="true"/> if the specified file is in use (locked); otherwise, <see langword="false"/></returns>
      /// <exception cref="FileNotFoundException"></exception>
      /// <exception cref="IOException"/>
      /// <exception cref="Exception"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to check.</param>
      [SecurityCritical]
      public static bool IsLockedTransacted(KernelTransaction transaction, string path)
      {
         return IsLockedCore(transaction, path, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Determines whether the specified file is in use (locked).</summary>
      /// <returns>Returns <see langword="true"/> if the specified file is in use (locked); otherwise, <see langword="false"/></returns>
      /// <exception cref="FileNotFoundException"></exception>
      /// <exception cref="IOException"/>
      /// <exception cref="Exception"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to check.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static bool IsLockedTransacted(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         return IsLockedCore(transaction, path, pathFormat);
      }

      #endregion // Transactional

      /// <summary>[AlphaFS] Determines whether the specified file is in use (locked).</summary>
      /// <returns>Returns <see langword="true"/> if the specified file is in use (locked); otherwise, <see langword="false"/></returns>
      /// <exception cref="FileNotFoundException"></exception>
      /// <exception cref="IOException"/>
      /// <exception cref="Exception"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to check.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      internal static bool IsLockedCore(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         try
         {
            // Use FileAccess.Read since FileAccess.ReadWrite always fails when file is read-only.
            using (OpenCore(transaction, path, FileMode.Open, FileAccess.Read, FileShare.None, ExtendedFileAttributes.Normal, null, null, pathFormat)) {}
         }
         catch (IOException ex)
         {
            int lastError = Marshal.GetHRForException(ex) & NativeMethods.OverflowExceptionBitShift;
            if (lastError == Win32Errors.ERROR_SHARING_VIOLATION || lastError == Win32Errors.ERROR_LOCK_VIOLATION)
               return true;

            throw;
         }
         catch (Exception ex)
         {
            NativeError.ThrowException(Marshal.GetHRForException(ex) & NativeMethods.OverflowExceptionBitShift);
         }

         return false;
      }
   }
}
