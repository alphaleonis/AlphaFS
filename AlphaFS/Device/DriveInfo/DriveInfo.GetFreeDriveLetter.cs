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

namespace Alphaleonis.Win32.Filesystem
{
   public sealed partial class DriveInfo
   {
      /// <summary>[AlphaFS] Gets the first available drive letter on the local system.</summary>
      /// <returns>A drive letter as <see cref="char"/>. When no drive letters are available, an exception is thrown.</returns>
      /// <remarks>The letters "A" and "B" are reserved for floppy drives and will never be returned by this function.</remarks>
      public static char GetFreeDriveLetter()
      {
         return GetFreeDriveLetter(false);
      }


      /// <summary>[AlphaFS] Gets an available drive letter on the local system.</summary>
      /// <param name="getLastAvailable">When <see langword="true"/> get the last available drive letter. When <see langword="false"/> gets the first available drive letter.</param>
      /// <returns>A drive letter as <see cref="char"/>. When no drive letters are available, an exception is thrown.</returns>
      /// <remarks>The letters "A" and "B" are reserved for floppy drives and will never be returned by this function.</remarks>
      /// <exception cref="ArgumentOutOfRangeException">No drive letters available.</exception>
      [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
      public static char GetFreeDriveLetter(bool getLastAvailable)
      {
         var freeDriveLetters = "CDEFGHIJKLMNOPQRSTUVWXYZ".Except(EnumerateLogicalDrivesCore(false, false).OrderBy(driveName => driveName).Select(driveName => driveName[0]));

         try
         {
            return getLastAvailable ? freeDriveLetters.Last() : freeDriveLetters.First();
         }
         catch
         {
            throw new ArgumentOutOfRangeException(Resources.No_Drive_Letters_Available);
         }
      }
   }
}
