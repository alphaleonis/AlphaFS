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
using System.Net.NetworkInformation;
using System.Security;

namespace Alphaleonis.Win32.Network
{
   public static partial class Host
   {
      /// <summary>Enumerates the DFS Links from a DFS namespace.</summary>
      /// <returns><see cref="IEnumerable{DfsInfo}"/> of DFS namespaces.</returns>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="NetworkInformationException"/>
      /// <exception cref="PlatformNotSupportedException">The operating system is older than Windows Vista.</exception>
      /// <param name="dfsName">The Universal Naming Convention (UNC) path of a DFS root or link.</param>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dfs")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "dfs")]
      [SecurityCritical]
      public static IEnumerable<DfsInfo> EnumerateDfsLinks(string dfsName)
      {
         if (!Filesystem.NativeMethods.IsAtLeastWindowsVista)
            throw new PlatformNotSupportedException(new Win32Exception((int) Win32Errors.ERROR_OLD_WIN_VERSION).Message);


         if (Utils.IsNullOrWhiteSpace(dfsName))
            throw new ArgumentNullException("dfsName");


         var fd = new FunctionData();

         return EnumerateNetworkObjectCore(fd, (NativeMethods.DFS_INFO_9 structure, SafeGlobalMemoryBufferHandle buffer) => new DfsInfo(structure),

            (FunctionData functionData, out SafeGlobalMemoryBufferHandle buffer, int prefMaxLen, out uint entriesRead, out uint totalEntries, out uint resumeHandle1) =>
            {
               totalEntries = 0;

               return NativeMethods.NetDfsEnum(dfsName, 9, prefMaxLen, out buffer, out entriesRead, out resumeHandle1);

            }, false);
      }
   }
}
