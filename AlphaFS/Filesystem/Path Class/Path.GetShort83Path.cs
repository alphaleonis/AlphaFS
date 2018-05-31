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
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace Alphaleonis.Win32.Filesystem
{
   public static partial class Path
   {
      /// <summary>[AlphaFS] Retrieves the short path form of the specified path.</summary>
      /// <returns>A path that has the 8.3 path form.</returns>
      /// <remarks>Will fail on NTFS volumes with disabled 8.3 name generation.</remarks>
      /// <remarks>The path must actually exist to be able to get the short path name.</remarks>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <param name="path">An existing path to a folder or file.</param>
      [SecurityCritical]
      public static string GetShort83Path(string path)
      {
         return GetLongShort83PathCore(null, path, true);
      }


      /// <summary>[AlphaFS] Retrieves the short path form of the specified path.</summary>
      /// <returns>A path that has the 8.3 path form.</returns>
      /// <remarks>Will fail on NTFS volumes with disabled 8.3 name generation.</remarks>
      /// <remarks>The path must actually exist to be able to get the short path name.</remarks>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">An existing path to a folder or file.</param>
      [SecurityCritical]
      public static string GetShort83PathTransacted(KernelTransaction transaction, string path)
      {
         return GetLongShort83PathCore(transaction, path, true);
      }



      /// <summary>Retrieves the short path form, or the regular long form of the specified <paramref name="path"/>.</summary>
      /// <returns>If <paramref name="getShort"/> is <see langword="true"/>, a path of the 8.3 form otherwise the regular long form.</returns>
      /// <remarks>
      ///   <para>Will fail on NTFS volumes with disabled 8.3 name generation.</para>
      ///   <para>The path must actually exist to be able to get the short- or long path name.</para>
      /// </remarks>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">An existing path to a folder or file.</param>
      /// <param name="getShort"><see langword="true"/> to retrieve the short path form, <see langword="false"/> to retrieve the regular long form from the 8.3 <paramref name="path"/>.</param>
      [SecurityCritical]
      private static string GetLongShort83PathCore(KernelTransaction transaction, string path, bool getShort)
      {
         var pathLp = GetFullPathCore(transaction, path, GetFullPathOptions.AsLongPath | GetFullPathOptions.FullCheck);

         var buffer = new StringBuilder();
         var actualLength = getShort ? NativeMethods.GetShortPathName(pathLp, null, 0) : (uint)path.Length;

         while (actualLength > buffer.Capacity)
         {
            buffer = new StringBuilder((int)actualLength);
            actualLength = getShort

               // GetShortPathName()
               // 2014-01-29: MSDN confirms LongPath usage.

               // GetLongPathName()
               // 2014-01-29: MSDN confirms LongPath usage.

               ? NativeMethods.GetShortPathName(pathLp, buffer, (uint)buffer.Capacity)
               : transaction == null || !NativeMethods.IsAtLeastWindowsVista

                  ? NativeMethods.GetLongPathName(pathLp, buffer, (uint)buffer.Capacity)
                  : NativeMethods.GetLongPathNameTransacted(pathLp, buffer, (uint)buffer.Capacity, transaction.SafeHandle);


            var lastError = Marshal.GetLastWin32Error();

            if (actualLength == 0)
               NativeError.ThrowException(lastError, pathLp);
         }

         return GetRegularPathCore(buffer.ToString(), GetFullPathOptions.None, false);
      }
   }
}
