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
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Net.NetworkInformation;
using System.Security;

namespace Alphaleonis.Win32.Network
{
   public static partial class Host
   {
      /// <summary>Gets information about a specified DFS root or link in a DFS namespace.</summary>
      /// <returns>A <see cref="DfsInfo"/> instance.</returns>
      /// <exception cref="NetworkInformationException"/>
      /// <exception cref="PlatformNotSupportedException">The operating system is older than Windows Vista.</exception>
      /// <param name="dfsName">The Universal Naming Convention (UNC) path of a DFS root or link.</param>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dfs")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "dfs")]
      [SecurityCritical]
      public static DfsInfo GetDfsInfo(string dfsName)
      {
         return GetDfsInfoCore(false, dfsName, null, null);
      }




      /// <summary>Retrieves information about a specified DFS root or link in a DFS namespace.</summary>
      /// <returns>A <see cref="DfsInfo"/> instance.</returns>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="NetworkInformationException"/>
      /// <exception cref="PlatformNotSupportedException">The operating system is older than Windows Vista.</exception>
      /// <param name="getFromClient">
      ///   <see langword="true"/> retrieves information about a Distributed File System (DFS) root or link from the cache maintained by the
      ///   DFS client. When <see langword="false"/> retrieves information about a specified Distributed File System (DFS) root or link in a
      ///   DFS namespace.
      /// </param>
      /// <param name="dfsName">The Universal Naming Convention (UNC) path of a DFS root or link.</param>
      /// <param name="serverName">
      ///   The name of the DFS root target or link target server. If <paramref name="getFromClient"/> is <see langword="false"/>, this
      ///   parameter is always <see langword="null"/>.
      /// </param>
      /// <param name="shareName">
      ///   The name of the share corresponding to the DFS root target or link target. If <paramref name="getFromClient"/> is
      ///   <see langword="false"/>, this parameter is always <see langword="null"/>.
      /// </param>
      [SecurityCritical]
      internal static DfsInfo GetDfsInfoCore(bool getFromClient, string dfsName, string serverName, string shareName)
      {
         if (!Filesystem.NativeMethods.IsAtLeastWindowsVista)
            throw new PlatformNotSupportedException(new Win32Exception((int) Win32Errors.ERROR_OLD_WIN_VERSION).Message);


         if (Utils.IsNullOrWhiteSpace(dfsName))
            throw new ArgumentNullException("dfsName");


         serverName = !Utils.IsNullOrWhiteSpace(serverName) ? serverName : null;
         shareName = !Utils.IsNullOrWhiteSpace(shareName) ? shareName : null;

         SafeGlobalMemoryBufferHandle safeBuffer;


         // Level 9 = DFS_INFO_9

         var lastError = getFromClient ? NativeMethods.NetDfsGetClientInfo(dfsName, serverName, shareName, 9, out safeBuffer) : NativeMethods.NetDfsGetInfo(dfsName, null, null, 9, out safeBuffer);

         if (lastError == Win32Errors.NERR_Success)
            return new DfsInfo(safeBuffer.PtrToStructure<NativeMethods.DFS_INFO_9>(0));

         throw new NetworkInformationException((int)lastError);
      }
   }
}
