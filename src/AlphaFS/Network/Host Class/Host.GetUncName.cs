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
using System.Globalization;
using System.Security;
using Path = Alphaleonis.Win32.Filesystem.Path;

namespace Alphaleonis.Win32.Network
{
   /// <summary>Provides static methods to retrieve network resource information from a local- or remote host.</summary>
   public static partial class Host
   {
      /// <summary>Return the host name in UNC format, for example: \\hostname</summary>
      /// <returns>The unc name.</returns>
      [SecurityCritical]
      public static string GetUncName()
      {
         return string.Format(CultureInfo.InvariantCulture, "{0}{1}", Path.UncPrefix, Environment.MachineName);
      }


      /// <summary>Return the host name in UNC format, for example: \\hostname</summary>
      /// <param name="computerName">Name of the computer.</param>
      /// <returns>The unc name.</returns>
      [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0")]
      [SecurityCritical]
      public static string GetUncName(string computerName)
      {
         return Utils.IsNullOrWhiteSpace(computerName) ? GetUncName() : (computerName.StartsWith(Path.UncPrefix, StringComparison.Ordinal) ? computerName.Trim() : Path.UncPrefix + computerName.Trim());
      }
   }
}
