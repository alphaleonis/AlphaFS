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

using Alphaleonis.Win32.Filesystem;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Security;

namespace Alphaleonis.Win32.Network
{
   public static partial class Host
   {
      /// <summary>Gets the host and share path name for the given <paramref name="uncPath"/>.</summary>
      /// <param name="uncPath">The share in the format: \\host\share.</param>
      /// <returns>The host and share path. For example, if <paramref name="uncPath"/> is: "\\SERVER001\C$\WINDOWS\System32",
      ///   its is returned as string[0] = "SERVER001" and string[1] = "\C$\WINDOWS\System32".
      /// <para>If the conversion from local path to UNC path fails, <see langword="null"/> is returned.</para>
      /// </returns>
      [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Utils.IsNullOrWhiteSpace validates arguments.")]
      [SecurityCritical]
      public static string[] GetHostShareFromPath(string uncPath)
      {
         if (Utils.IsNullOrWhiteSpace(uncPath))
            return null;

         Uri uri;
         if (Uri.TryCreate(Path.GetRegularPathCore(uncPath, GetFullPathOptions.None, false), UriKind.Absolute, out uri) && uri.IsUnc)
         {
            return new[]
            {
               uri.Host,
               uri.GetComponents(UriComponents.Path, UriFormat.Unescaped).Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar)
            };
         }

         return null;
      }
   }
}
