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

using System.Diagnostics.CodeAnalysis;
using System.Security;

namespace Alphaleonis.Win32.Filesystem
{
   public static partial class Path
   {
      #region GetRandomFileName (.NET)

      /// <summary>Returns a random folder name or file name.</summary>
      /// <returns>A random folder name or file name.</returns>
      [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
      [SecurityCritical]
      public static string GetRandomFileName()
      {
         return System.IO.Path.GetRandomFileName();
      }

      #endregion // GetRandomFileName (.NET)

      #region GetTempFileName (.NET)

      /// <summary>Creates a uniquely named, zero-byte temporary file on disk and returns the full path of that file.</summary>
      /// <returns>The full path of the temporary file.</returns>
      [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
      [SecurityCritical]
      public static string GetTempFileName()
      {
         return System.IO.Path.GetTempFileName();
      }

      #endregion // GetTempFileName (.NET)

      #region GetTempPath (.NET)

      /// <summary>Returns the path of the current user's temporary folder.</summary>
      /// <returns>The path to the temporary folder, ending with a backslash.</returns>
      [SecurityCritical]
      public static string GetTempPath()
      {
         return System.IO.Path.GetTempPath();
      }

      /// <summary>[AlphaFS] Returns the path of the current user's temporary folder.</summary>
      /// <param name="combinePath">The folder name to append to the temporary folder.</param>
      /// <returns>The path to the temporary folder, combined with <paramref name="combinePath"/>.</returns>
      [SecurityCritical]
      public static string GetTempPath(string combinePath)
      {
         string tempPath = GetTempPath();
         return !Utils.IsNullOrWhiteSpace(combinePath) ? CombineCore(false, tempPath, combinePath) : tempPath;
      }

      #endregion // GetTempPath (.NET)
   }
}
