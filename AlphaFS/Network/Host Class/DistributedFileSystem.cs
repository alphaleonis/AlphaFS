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

using Alphaleonis.Win32.Filesystem;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security;

namespace Alphaleonis.Win32.Network
{
   partial class Host
   {
      #region EnumerateDfsLinks

      /// <summary>Enumerates the DFS Links from a DFS namespace.</summary>
      /// <returns><see cref="IEnumerable{DfsInfo}"/> of DFS namespaces.</returns>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="NetworkInformationException"/>
      /// <exception cref="PlatformNotSupportedException"/>
      /// <param name="dfsName">The Universal Naming Convention (UNC) path of a DFS root or link.</param>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dfs")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "dfs")]
      [SecurityCritical]
      public static IEnumerable<DfsInfo> EnumerateDfsLinks(string dfsName)
      {
         if (!Filesystem.NativeMethods.IsAtLeastWindowsVista)
            throw new PlatformNotSupportedException(Resources.Requires_Windows_Vista_Or_Higher);

         if (Utils.IsNullOrWhiteSpace(dfsName))
            throw new ArgumentNullException("dfsName");

         var fd = new FunctionData();

         return EnumerateNetworkObjectCore(fd, (NativeMethods.DFS_INFO_9 structure, SafeGlobalMemoryBufferHandle buffer) =>

            new DfsInfo(structure),

            (FunctionData functionData, out SafeGlobalMemoryBufferHandle buffer, int prefMaxLen, out uint entriesRead, out uint totalEntries, out uint resumeHandle1) =>
            {
               totalEntries = 0;
               return NativeMethods.NetDfsEnum(dfsName, 9, prefMaxLen, out buffer, out entriesRead, out resumeHandle1);

            }, false);
      }

      #endregion // EnumerateDfsLinks

      #region EnumerateDfsRoot

      /// <summary>Enumerates the DFS namespaces from the local host.</summary>
      /// <returns><see cref="IEnumerable{String}"/> of DFS Root namespaces from the local host.</returns>
      /// <exception cref="NetworkInformationException"/>
      /// <exception cref="PlatformNotSupportedException"/>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dfs")]
      [SecurityCritical]
      public static IEnumerable<string> EnumerateDfsRoot()
      {
         return EnumerateDfsRootCore(null, false);
      }

      /// <summary>Enumerates the DFS namespaces from a host.</summary>
      /// <returns><see cref="IEnumerable{String}"/> of DFS Root namespaces from a host.</returns>
      /// <exception cref="NetworkInformationException"/>
      /// <exception cref="PlatformNotSupportedException"/>
      /// <param name="host">The DNS or NetBIOS name of a host.</param>
      /// <param name="continueOnException"><see langword="true"/> suppress any Exception that might be thrown as a result from a failure, such as unavailable resources.</param>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dfs")]
      [SecurityCritical]
      public static IEnumerable<string> EnumerateDfsRoot(string host, bool continueOnException)
      {
         return EnumerateDfsRootCore(host, continueOnException);
      }

      #endregion // EnumerateDfsRoot

      #region EnumerateDomainDfsRoot

      /// <summary>Enumerates the DFS namespaces from the domain.</summary>
      /// <returns><see cref="IEnumerable{String}"/> of DFS Root namespaces from the domain.</returns>
      /// <exception cref="NetworkInformationException"/>
      /// <exception cref="PlatformNotSupportedException"/>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dfs")]
      [SecurityCritical]
      public static IEnumerable<string> EnumerateDomainDfsRoot()
      {
         return EnumerateDomainDfsRootCore(null, false);
      }

      /// <summary>Enumerates the DFS namespaces from a domain.</summary>
      /// <returns><see cref="IEnumerable{String}"/> of DFS Root namespaces from a domain.</returns>
      /// <exception cref="NetworkInformationException"/>
      /// <exception cref="PlatformNotSupportedException"/>
      /// <param name="domain">A domain name.</param>
      /// <param name="continueOnException"><see langword="true"/> suppress any Exception that might be thrown as a result from a failure, such as unavailable resources.</param>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dfs")]
      [SecurityCritical]
      public static IEnumerable<string> EnumerateDomainDfsRoot(string domain, bool continueOnException)
      {
         return EnumerateDomainDfsRootCore(domain, continueOnException);
      }

      #endregion // EnumerateDomainDfsRoot


      #region GetDfsClientInfo

      /// <summary>Gets information about a DFS root or link from the cache maintained by the DFS client.</summary>
      /// <returns>A <see cref="DfsInfo"/> instance.</returns>
      /// <exception cref="NetworkInformationException"/>
      /// <exception cref="PlatformNotSupportedException"/>
      /// <param name="dfsName">The Universal Naming Convention (UNC) path of a DFS root or link.</param>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dfs")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "dfs")]
      [SecurityCritical]
      public static DfsInfo GetDfsClientInfo(string dfsName)
      {
         return GetDfsInfoCore(true, dfsName, null, null);
      }

      /// <summary>Gets information about a DFS root or link from the cache maintained by the DFS client.</summary>
      /// <returns>A <see cref="DfsInfo"/> instance.</returns>
      /// <exception cref="NetworkInformationException"/>
      /// <exception cref="PlatformNotSupportedException"/>
      /// <param name="dfsName">The Universal Naming Convention (UNC) path of a DFS root or link.</param>
      /// <param name="serverName">The name of the DFS root target or link target server.</param>
      /// <param name="shareName">The name of the share corresponding to the DFS root target or link target.</param>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dfs")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "dfs")]
      [SecurityCritical]
      public static DfsInfo GetDfsClientInfo(string dfsName, string serverName, string shareName)
      {
         return GetDfsInfoCore(true, dfsName, serverName, shareName);
      }

      #endregion // GetDfsClientInfo

      #region GetDfsInfo

      /// <summary>Gets information about a specified DFS root or link in a DFS namespace.</summary>
      /// <returns>A <see cref="DfsInfo"/> instance.</returns>
      /// <exception cref="NetworkInformationException"/>
      /// <exception cref="PlatformNotSupportedException"/>
      /// <param name="dfsName">The Universal Naming Convention (UNC) path of a DFS root or link.</param>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dfs")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "dfs")]
      [SecurityCritical]
      public static DfsInfo GetDfsInfo(string dfsName)
      {
         return GetDfsInfoCore(false, dfsName, null, null);
      }

      #endregion // GetDfsInfo


      #region Internal Methods

      /// <summary>Enumerates the DFS namespaces from a host.</summary>
      /// <returns><see cref="IEnumerable{String}"/> of DFS Root namespaces from a host.</returns>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="NetworkInformationException"/>
      /// <exception cref="PlatformNotSupportedException"/>
      /// <param name="host">The DNS or NetBIOS name of a host.</param>
      /// <param name="continueOnException"><see langword="true"/> suppress any Exception that might be thrown as a result from a failure, such as unavailable resources.</param>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dfs")]
      [SecurityCritical]
      private static IEnumerable<string> EnumerateDfsRootCore(string host, bool continueOnException)
      {
         if (!Filesystem.NativeMethods.IsAtLeastWindowsVista)
            throw new PlatformNotSupportedException(Resources.Requires_Windows_Vista_Or_Higher);

         return EnumerateNetworkObjectCore(new FunctionData(), (NativeMethods.DFS_INFO_300 structure, SafeGlobalMemoryBufferHandle buffer) =>

            new DfsInfo { EntryPath = structure.DfsName },

            (FunctionData functionData, out SafeGlobalMemoryBufferHandle buffer, int prefMaxLen, out uint entriesRead, out uint totalEntries, out uint resumeHandle) =>
            {
               totalEntries = 0;

               // When host == null, the local computer is used.
               // However, the resulting Host property will be empty.
               // So, explicitly state Environment.MachineName to prevent this.
               // Furthermore, the UNC prefix: \\ is not required and always removed.
               string stripUnc = Utils.IsNullOrWhiteSpace(host) ? Environment.MachineName : Path.GetRegularPathCore(host, GetFullPathOptions.CheckInvalidPathChars, false).Replace(Path.UncPrefix, string.Empty);

               return NativeMethods.NetDfsEnum(stripUnc, 300, prefMaxLen, out buffer, out entriesRead, out resumeHandle);

            }, continueOnException).Select(dfs => dfs.EntryPath);
      }


      /// <summary>Enumerates the DFS namespaces from a domain.</summary>
      /// <returns><see cref="IEnumerable{String}"/> of DFS Root namespaces from a domain.</returns>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="NetworkInformationException"/>
      /// <exception cref="PlatformNotSupportedException"/>
      /// <param name="domain">A domain name.</param>
      /// <param name="continueOnException"><see langword="true"/> suppress any Exception that might be thrown as a result from a failure, such as unavailable resources.</param>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dfs")]
      [SecurityCritical]
      private static IEnumerable<string> EnumerateDomainDfsRootCore(string domain, bool continueOnException)
      {
         if (!Filesystem.NativeMethods.IsAtLeastWindowsVista)
            throw new PlatformNotSupportedException(Resources.Requires_Windows_Vista_Or_Higher);

         return EnumerateNetworkObjectCore(new FunctionData(), (NativeMethods.DFS_INFO_200 structure, SafeGlobalMemoryBufferHandle buffer) =>

            new DfsInfo { EntryPath = string.Format(CultureInfo.CurrentCulture, "{0}{1}{2}{3}", Path.UncPrefix, NativeMethods.ComputerDomain, Path.DirectorySeparatorChar, structure.FtDfsName) },

            (FunctionData functionData, out SafeGlobalMemoryBufferHandle buffer, int prefMaxLen, out uint entriesRead, out uint totalEntries, out uint resumeHandle) =>
            {
               totalEntries = 0;

               // When host == null, the local computer is used.
               // However, the resulting Host property will be empty.
               // So, explicitly state Environment.MachineName to prevent this.
               // Furthermore, the UNC prefix: \\ is not required and always removed.
               string stripUnc = Utils.IsNullOrWhiteSpace(domain) ? NativeMethods.ComputerDomain : Path.GetRegularPathCore(domain, GetFullPathOptions.CheckInvalidPathChars, false).Replace(Path.UncPrefix, string.Empty);

               return NativeMethods.NetDfsEnum(stripUnc, 200, prefMaxLen, out buffer, out entriesRead, out resumeHandle);

            }, continueOnException).Select(dfs => dfs.EntryPath);
      }


      /// <summary>Retrieves information about a specified DFS root or link in a DFS namespace.</summary>
      /// <returns>A <see cref="DfsInfo"/> instance.</returns>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="NetworkInformationException"/>
      /// <exception cref="PlatformNotSupportedException"/>
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
      private static DfsInfo GetDfsInfoCore(bool getFromClient, string dfsName, string serverName, string shareName)
      {
         if (!Filesystem.NativeMethods.IsAtLeastWindowsVista)
            throw new PlatformNotSupportedException(Resources.Requires_Windows_Vista_Or_Higher);

         if (Utils.IsNullOrWhiteSpace(dfsName))
            throw new ArgumentNullException("dfsName");

         serverName = Utils.IsNullOrWhiteSpace(serverName) ? null : serverName;
         shareName = Utils.IsNullOrWhiteSpace(shareName) ? null : shareName;

         SafeGlobalMemoryBufferHandle safeBuffer;

         // Level 9 = DFS_INFO_9

         uint lastError = getFromClient
            ? NativeMethods.NetDfsGetClientInfo(dfsName, serverName, shareName, 9, out safeBuffer)
            : NativeMethods.NetDfsGetInfo(dfsName, null, null, 9, out safeBuffer);

         if (lastError == Win32Errors.NERR_Success)
            return new DfsInfo(safeBuffer.PtrToStructure<NativeMethods.DFS_INFO_9>(0));

         throw new NetworkInformationException((int) lastError);
      }

      #endregion // Internal Methods
   }
}
