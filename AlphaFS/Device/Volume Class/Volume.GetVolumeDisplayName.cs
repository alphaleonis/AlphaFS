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
using System.Linq;
using System.Security;

namespace Alphaleonis.Win32.Filesystem
{
   public static partial class Volume
   {
      /// <summary>[AlphaFS] Gets the shortest display name for the specified <paramref name="volumeGuid"/>.</summary>
      /// <remarks>This method basically returns the shortest string returned by <see cref="EnumerateVolumePathNames"/></remarks>
      /// <param name="volumeGuid">A volume <see cref="Guid"/> path: <c>\\?\Volume{xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}\</c></param>
      /// <returns>
      ///   The shortest display name for the specified volume found, or <c>null</c> if no display names were found.
      /// </returns>
      [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
      [SecurityCritical]
      public static string GetVolumeDisplayName(string volumeGuid)
      {
         string[] smallestMountPoint = {new string(Path.WildcardStarMatchAllChar, NativeMethods.MaxPathUnicode)};

         try
         {
            foreach (var mountPoint in EnumerateVolumePathNames(volumeGuid).Where(mPoint => !Utils.IsNullOrWhiteSpace(mPoint) && mPoint.Length < smallestMountPoint[0].Length))

               smallestMountPoint[0] = mountPoint;
         }
         catch
         {
         }


         var result = smallestMountPoint[0][0] == Path.WildcardStarMatchAllChar ? null : smallestMountPoint[0];

         return !Utils.IsNullOrWhiteSpace(result) ? result : null;
      }
   }
}
