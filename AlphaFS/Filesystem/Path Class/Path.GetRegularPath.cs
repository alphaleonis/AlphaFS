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
using System.Security;

namespace Alphaleonis.Win32.Filesystem
{
   public static partial class Path
   {
      /// <summary>[AlphaFS] Gets the regular path from long prefixed one. i.e.: "\\?\C:\Temp\file.txt" to C:\Temp\file.txt" or: "\\?\UNC\Server\share\file.txt" to "\\Server\share\file.txt".</summary>
      /// <returns>Regular form path string.</returns>
      /// <remarks>This method does not handle paths with volume names, eg. \\?\Volume{GUID}\Folder\file.txt.</remarks>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <param name="path">The path.</param>
      [SecurityCritical]
      public static string GetRegularPath(string path)
      {
         return GetRegularPathCore(path, GetFullPathOptions.CheckInvalidPathChars, false);
      }



      /// <summary>Gets the regular path from a long path.</summary>
      /// <returns>
      ///   <para>Returns the regular form of a long <paramref name="path"/>.</para>
      ///   <para>For example: "\\?\C:\Temp\file.txt" to: "C:\Temp\file.txt", or: "\\?\UNC\Server\share\file.txt" to: "\\Server\share\file.txt".</para>
      /// </returns>
      /// <remarks>
      ///   MSDN: String.TrimEnd Method notes to Callers: http://msdn.microsoft.com/en-us/library/system.string.trimend%28v=vs.110%29.aspx
      /// </remarks>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <param name="path">The path.</param>
      /// <param name="options">Options for controlling the full path retrieval.</param>
      /// <param name="allowEmpty">When <see langword="false"/>, throws an <see cref="ArgumentException"/>.</param>
      [SecurityCritical]
      internal static string GetRegularPathCore(string path, GetFullPathOptions options, bool allowEmpty)
      {
         if (path == null)
            throw new ArgumentNullException("path");

         if (!allowEmpty && (path.Length == 0 || Utils.IsNullOrWhiteSpace(path)))
            throw new ArgumentException(Resources.Path_Is_Zero_Length_Or_Only_White_Space, "path");

         if (options != GetFullPathOptions.None)
            path = ApplyFullPathOptions(path, options);


         return path.StartsWith(DosDeviceUncPrefix, StringComparison.OrdinalIgnoreCase)

            ? UncPrefix + path.Substring(DosDeviceUncPrefix.Length)
            : (path.StartsWith(NonInterpretedPathPrefix, StringComparison.Ordinal)

               ? path.Substring(NonInterpretedPathPrefix.Length)
               : (path.StartsWith(GlobalRootPrefix, StringComparison.OrdinalIgnoreCase) || path.StartsWith(VolumePrefix, StringComparison.OrdinalIgnoreCase) ||

                  !path.StartsWith(LongPathPrefix, StringComparison.Ordinal)
                  ? path
                  : (path.StartsWith(LongPathUncPrefix, StringComparison.OrdinalIgnoreCase) ? UncPrefix + path.Substring(LongPathUncPrefix.Length) : path.Substring(LongPathPrefix.Length))));
      }
   }
}
