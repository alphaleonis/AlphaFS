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
using System.Linq;
using System.Net.NetworkInformation;
using System.Security;
using Alphaleonis.Win32.Filesystem;

namespace Alphaleonis.Win32.Network
{
   public static partial class Host
   {
      /// <summary>Enumerates the DFS namespaces from the local host.</summary>
      /// <returns><see cref="IEnumerable{String}"/> of DFS Root namespaces from the local host.</returns>
      /// <exception cref="NetworkInformationException"/>
      /// <exception cref="PlatformNotSupportedException">The operating system is older than Windows Vista.</exception>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dfs")]
      [SecurityCritical]
      public static IEnumerable<string> EnumerateDfsRoot()
      {
         return EnumerateDfsRootCore(null, false);
      }

      
      /// <summary>Enumerates the DFS namespaces from a host.</summary>
      /// <returns><see cref="IEnumerable{String}"/> of DFS Root namespaces from a host.</returns>
      /// <exception cref="NetworkInformationException"/>
      /// <exception cref="PlatformNotSupportedException">The operating system is older than Windows Vista.</exception>
      /// <param name="host">The DNS or NetBIOS name of a host.</param>
      /// <param name="continueOnException"><see langword="true"/> suppress any Exception that might be thrown as a result from a failure, such as unavailable resources.</param>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dfs")]
      [SecurityCritical]
      public static IEnumerable<string> EnumerateDfsRoot(string host, bool continueOnException)
      {
         return EnumerateDfsRootCore(host, continueOnException);
      }




      /// <summary>Enumerates the DFS namespaces from a host.</summary>
      /// <returns><see cref="IEnumerable{String}"/> of DFS Root namespaces from a host.</returns>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="NetworkInformationException"/>
      /// <exception cref="PlatformNotSupportedException">The operating system is older than Windows Vista.</exception>
      /// <param name="host">The DNS or NetBIOS name of a host.</param>
      /// <param name="continueOnException"><see langword="true"/> suppress any Exception that might be thrown as a result from a failure, such as unavailable resources.</param>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dfs")]
      [SecurityCritical]
      internal static IEnumerable<string> EnumerateDfsRootCore(string host, bool continueOnException)
      {
         if (!Filesystem.NativeMethods.IsAtLeastWindowsVista)
            throw new PlatformNotSupportedException(new Win32Exception((int) Win32Errors.ERROR_OLD_WIN_VERSION).Message);


         return EnumerateNetworkObjectCore(new FunctionData(), (NativeMethods.DFS_INFO_300 structure, SafeGlobalMemoryBufferHandle buffer) => new DfsInfo {EntryPath = structure.DfsName},

            (FunctionData functionData, out SafeGlobalMemoryBufferHandle buffer, int prefMaxLen, out uint entriesRead, out uint totalEntries, out uint resumeHandle) =>
            {
               totalEntries = 0;

               // When host == null, the local computer is used.
               // However, the resulting OpenResourceInfo.Host property will be empty.
               // So, explicitly state Environment.MachineName to prevent this.
               // Furthermore, the UNC prefix: \\ is not required and always removed.
               var stripUnc = !Utils.IsNullOrWhiteSpace(host) ? Path.GetRegularPathCore(host, GetFullPathOptions.CheckInvalidPathChars, false).Replace(Path.UncPrefix, string.Empty) : Environment.MachineName;

               return NativeMethods.NetDfsEnum(stripUnc, 300, prefMaxLen, out buffer, out entriesRead, out resumeHandle);

            }, continueOnException).Select(dfs => dfs.EntryPath);
      }
   }
}
