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
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security;
using Alphaleonis.Win32.Filesystem;

namespace Alphaleonis.Win32.Network
{
   public static partial class Host
   {
      /// <summary>Enumerates the DFS namespaces from the domain.</summary>
      /// <returns><see cref="IEnumerable{String}"/> of DFS Root namespaces from the domain.</returns>
      /// <exception cref="NetworkInformationException"/>
      /// <exception cref="PlatformNotSupportedException">The operating system is older than Windows Vista.</exception>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dfs")]
      [SecurityCritical]
      public static IEnumerable<string> EnumerateDomainDfsRoot()
      {
         return EnumerateDomainDfsRootCore(null, false);
      }

      
      /// <summary>Enumerates the DFS namespaces from a domain.</summary>
      /// <returns><see cref="IEnumerable{String}"/> of DFS Root namespaces from a domain.</returns>
      /// <exception cref="NetworkInformationException"/>
      /// <exception cref="PlatformNotSupportedException">The operating system is older than Windows Vista.</exception>
      /// <param name="domain">A domain name.</param>
      /// <param name="continueOnException"><see langword="true"/> suppress any Exception that might be thrown as a result from a failure, such as unavailable resources.</param>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dfs")]
      [SecurityCritical]
      public static IEnumerable<string> EnumerateDomainDfsRoot(string domain, bool continueOnException)
      {
         return EnumerateDomainDfsRootCore(domain, continueOnException);
      }




      /// <summary>Enumerates the DFS namespaces from a domain.</summary>
      /// <returns><see cref="IEnumerable{String}"/> of DFS Root namespaces from a domain.</returns>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="NetworkInformationException"/>
      /// <exception cref="PlatformNotSupportedException">The operating system is older than Windows Vista.</exception>
      /// <param name="domain">A domain name.</param>
      /// <param name="continueOnException"><see langword="true"/> suppress any Exception that might be thrown as a result from a failure, such as unavailable resources.</param>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dfs")]
      [SecurityCritical]
      internal static IEnumerable<string> EnumerateDomainDfsRootCore(string domain, bool continueOnException)
      {
         if (!Filesystem.NativeMethods.IsAtLeastWindowsVista)
            throw new PlatformNotSupportedException(new Win32Exception((int) Win32Errors.ERROR_OLD_WIN_VERSION) .Message);


         return EnumerateNetworkObjectCore(new FunctionData(), (NativeMethods.DFS_INFO_200 structure, SafeGlobalMemoryBufferHandle buffer) =>

               new DfsInfo {EntryPath = string.Format(CultureInfo.InvariantCulture, "{0}{1}{2}{3}", Path.UncPrefix, !Utils.IsNullOrWhiteSpace(domain) ? domain : NativeMethods.ComputerDomain, Path.DirectorySeparatorChar, structure.FtDfsName)},

            (FunctionData functionData, out SafeGlobalMemoryBufferHandle buffer, int prefMaxLen, out uint entriesRead, out uint totalEntries, out uint resumeHandle) =>
            {
               totalEntries = 0;

               // When host == null, the local computer is used.
               // However, the resulting Host property will be empty.
               // So, explicitly state Environment.MachineName to prevent this.
               // Furthermore, the UNC prefix: \\ is not required and always removed.
               var stripUnc = !Utils.IsNullOrWhiteSpace(domain) ? Path.GetRegularPathCore(domain, GetFullPathOptions.CheckInvalidPathChars, false).Replace(Path.UncPrefix, string.Empty) : NativeMethods.ComputerDomain;

               return NativeMethods.NetDfsEnum(stripUnc, 200, prefMaxLen, out buffer, out entriesRead, out resumeHandle);

            }, continueOnException).Select(dfs => dfs.EntryPath);
      }
   }
}
