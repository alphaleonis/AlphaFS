/*  Copyright (C) 2008-2015 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
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
using System.Net.NetworkInformation;
using System.Security;

namespace Alphaleonis.Win32.Network
{
   partial class Host
   {
      #region EnumerateOpenConnections

      /// <summary>Enumerates open connections from the local host.</summary>
      /// <returns>Returns <see cref="OpenConnectionInfo"/> connection information from the local host.</returns>
      /// <exception cref="ArgumentNullException">Thrown when one or more required arguments are null.</exception>
      /// <exception cref="NetworkInformationException"></exception>
      [SecurityCritical]
      public static IEnumerable<OpenConnectionInfo> EnumerateOpenConnections()
      {
         return EnumerateOpenConnectionsInternal(null, null, false);
      }

      /// <summary>Enumerates open connections from the specified host.</summary>
      /// <returns>Returns <see cref="OpenConnectionInfo"/> connection information from the specified <paramref name="host"/>.</returns>
      /// <exception cref="ArgumentNullException">Thrown when one or more required arguments are null.</exception>
      /// <exception cref="NetworkInformationException"></exception>
      /// <param name="host">The DNS or NetBIOS name of the remote server. <see langword="null"/> refers to the local host.</param>
      /// <param name="share">The name of the Server Message Block (SMB) share.</param>
      /// <param name="continueOnException">
      ///   <para><see langword="true"/> suppress any Exception that might be thrown a result from a failure,</para>
      ///   <para>such as unavailable resources.</para>
      /// </param>
      [SecurityCritical]
      public static IEnumerable<OpenConnectionInfo> EnumerateOpenConnections(string host, string share, bool continueOnException)
      {
         return EnumerateOpenConnectionsInternal(host, share, continueOnException);
      }

      #endregion // EnumerateOpenConnections

      #region EnumerateShares

      /// <summary>Enumerates Server Message Block (SMB) shares from the local host.</summary>
      /// <returns>Returns <see cref="IEnumerable{ShareInfo}"/> shares from the specified host.</returns>
      [SecurityCritical]
      public static IEnumerable<ShareInfo> EnumerateShares()
      {
         return EnumerateSharesInternal(null, false);
      }

      /// <summary>
      ///   Enumerates Server Message Block (SMB) shares from the specified host.
      /// </summary>
      /// <param name="host">The DNS or NetBIOS name of the specified <paramref name="host"/>.</param>
      /// <param name="continueOnException">
      ///   <para><see langword="true"/> suppress any Exception that might be thrown a result from a failure,</para>
      ///   <para>such as unavailable resources.</para>
      /// </param>
      /// <returns>Returns <see cref="IEnumerable{ShareInfo}"/> shares from the specified host.</returns>
      [SecurityCritical]
      public static IEnumerable<ShareInfo> EnumerateShares(string host, bool continueOnException)
      {
         return EnumerateSharesInternal(host, continueOnException);
      }

      #endregion // EnumerateShares

      #region GetHostShareFromPath

      /// <summary>Gets the host and Server Message Block (SMB) share name for the given <paramref name="uncPath"/>.</summary>
      /// <param name="uncPath">The share in the format: \\host\share.</param>
      /// <returns>string[0] = host, string[1] = share;</returns>
      [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Utils.IsNullOrWhiteSpace validates arguments.")]
      [SecurityCritical]
      public static string[] GetHostShareFromPath(string uncPath)
      {
         if (Utils.IsNullOrWhiteSpace(uncPath))
            return null;

         // Get Host and Share.
         uncPath = uncPath.Replace(Path.LongPathUncPrefix, string.Empty);
         uncPath = uncPath.Replace(Path.UncPrefix, string.Empty);

         return uncPath.Split(Path.DirectorySeparatorChar);
      }

      #endregion // GetHostShareFromPath

      #region GetShareInfo

      /// <summary>Retrieves information about the Server Message Block (SMB) share as defined on the specified host.</summary>
      /// <returns>
      /// A <see cref="ShareInfo"/> class, or <see langword="null"/> on failure or when not available,
      /// and <paramref name="continueOnException"/> is <see langword="true"/>.
      /// </returns>
      /// <param name="uncPath">The share in the format: \\host\share.</param>
      /// <param name="continueOnException"><see langword="true"/> to suppress any Exception that might be thrown a result from a failure, such as unavailable resources.</param>
      [SecurityCritical]
      public static ShareInfo GetShareInfo(string uncPath, bool continueOnException)
      {
         string[] unc = GetHostShareFromPath(uncPath);
         return GetShareInfoInternal(ShareInfoLevel.Info503, unc[0], unc[1], continueOnException);
      }

      /// <summary>Retrieves information about the Server Message Block (SMB) share as defined on the specified host.</summary>
      /// <returns>
      /// A <see cref="ShareInfo"/> class, or <see langword="null"/> on failure or when not available,
      /// and <paramref name="continueOnException"/> is <see langword="true"/>.
      /// </returns>
      /// <param name="shareLevel">One of the <see cref="ShareInfoLevel"/> options.</param>
      /// <param name="uncPath">The share in the format: \\host\share.</param>
      /// <param name="continueOnException"><see langword="true"/> to suppress any Exception that might be thrown a result from a failure, such as unavailable resources.</param>
      [SecurityCritical]
      public static ShareInfo GetShareInfo(ShareInfoLevel shareLevel, string uncPath, bool continueOnException)
      {
         string[] unc = GetHostShareFromPath(uncPath);
         return GetShareInfoInternal(shareLevel, unc[0], unc[1], continueOnException);
      }

      /// <summary>Retrieves information about the Server Message Block (SMB) share as defined on the specified host.</summary>
      /// <returns>
      /// A <see cref="ShareInfo"/> class, or <see langword="null"/> on failure or when not available,
      /// and <paramref name="continueOnException"/> is <see langword="true"/>.
      /// </returns>
      /// <param name="host">The DNS or NetBIOS name of the specified <paramref name="host"/>.</param>
      /// <param name="share">The name of the Server Message Block (SMB) share.</param>
      /// <param name="continueOnException"><see langword="true"/> to suppress any Exception that might be thrown a result from a failure, such as unavailable resources.</param>
      [SecurityCritical]
      public static ShareInfo GetShareInfo(string host, string share, bool continueOnException)
      {
         return GetShareInfoInternal(ShareInfoLevel.Info503, host, share, continueOnException);
      }

      /// <summary>Retrieves information about the Server Message Block (SMB) share as defined on the specified host.</summary>
      /// <returns>
      /// A <see cref="ShareInfo"/> class, or <see langword="null"/> on failure or when not available,
      /// and <paramref name="continueOnException"/> is <see langword="true"/>.
      /// </returns>
      /// <param name="shareLevel">One of the <see cref="ShareInfoLevel"/> options.</param>
      /// <param name="host">A string that specifies the DNS or NetBIOS name of the specified <paramref name="host"/>.</param>
      /// <param name="share">A string that specifies the name of the Server Message Block (SMB) share.</param>
      /// <param name="continueOnException"><see langword="true"/> to suppress any Exception that might be thrown a result from a failure, such as unavailable resources.</param>
      [SecurityCritical]
      public static ShareInfo GetShareInfo(ShareInfoLevel shareLevel, string host, string share, bool continueOnException)
      {
         return GetShareInfoInternal(shareLevel, host, share, continueOnException);
      }

      #endregion // GetShareInfo


      #region Internal Methods

      #region EnumerateOpenConnectionsInternal

      /// <summary>Unified method EnumerateOpenConnectionsInternal() to enumerate open connections from the specified host.</summary>
      /// <returns>Returns <see cref="OpenConnectionInfo"/> connection information from the specified <paramref name="host"/>.</returns>
      /// <exception cref="ArgumentNullException">Thrown when one or more required arguments are null.</exception>
      /// <exception cref="NetworkInformationException"></exception>
      /// <param name="host">The DNS or NetBIOS name of the remote server. <see langword="null"/> refers to the local host.</param>
      /// <param name="share">The name of the Server Message Block (SMB) share.</param>
      /// <param name="continueOnException"><see langword="true"/> suppress any Exception that might be thrown a result from a failure, such as unavailable resources.</param>
      [SecurityCritical]
      private static IEnumerable<OpenConnectionInfo> EnumerateOpenConnectionsInternal(string host, string share, bool continueOnException)
      {
         if (Utils.IsNullOrWhiteSpace(share))
            throw new ArgumentNullException("share");

         return EnumerateNetworkObjectInternal(new FunctionData { ExtraData1 = share }, (NativeMethods.ConnectionInfo1 structure, SafeGlobalMemoryBufferHandle buffer) =>

               new OpenConnectionInfo(host, structure),

            (FunctionData functionData, out SafeGlobalMemoryBufferHandle buffer, int prefMaxLen, out uint entriesRead, out uint totalEntries, out uint resumeHandle) =>
            {
               // When host == null, the local computer is used.
               // However, the resulting OpenResourceInfo.Host property will be empty.
               // So, explicitly state Environment.MachineName to prevent this.
               // Furthermore, the UNC prefix: \\ is not required and always removed.
               string stripUnc = Utils.IsNullOrWhiteSpace(host) ? Environment.MachineName : Path.GetRegularPathInternal(host, GetFullPathOptions.CheckInvalidPathChars).Replace(Path.UncPrefix, string.Empty);

               return NativeMethods.NetConnectionEnum(stripUnc, functionData.ExtraData1, 1, out buffer, NativeMethods.MaxPreferredLength, out entriesRead, out totalEntries, out resumeHandle);

            },
            continueOnException);
      }

      #endregion // EnumerateOpenConnectionsInternal

      #region EnumerateSharesInternal

      /// <summary>Unified method EnumerateSharesInternal() to enumerate (hidden) Server Message Block (SMB) shares from the specified host.</summary>
      /// <returns>Returns <see cref="IEnumerable{ShareInfo}"/> shares from the specified host.</returns>
      /// <exception cref="ArgumentNullException">Thrown when one or more required arguments are null.</exception>
      /// <exception cref="NetworkInformationException"></exception>
      /// <param name="host">The DNS or NetBIOS name of the specified <paramref name="host"/>.</param>
      /// <param name="continueOnException"><see langword="true"/> suppress any Exception that might be thrown a result from a failure, such as unavailable resources.</param>
      [SecurityCritical]
      internal static IEnumerable<ShareInfo> EnumerateSharesInternal(string host, bool continueOnException)
      {
         // When host == null, the local computer is used.
         // However, the resulting OpenResourceInfo.Host property will be empty.
         // So, explicitly state Environment.MachineName to prevent this.
         // Furthermore, the UNC prefix: \\ is not required and always removed.
         string stripUnc = Utils.IsNullOrWhiteSpace(host)
            ? Environment.MachineName
            : Path.GetRegularPathInternal(host, GetFullPathOptions.CheckInvalidPathChars).Replace(Path.UncPrefix, string.Empty);

         var fd = new FunctionData();
         bool hasItems = false;

         // Try ShareInfo503 structure.
         foreach (var si in EnumerateNetworkObjectInternal(fd, (NativeMethods.ShareInfo503 structure, SafeGlobalMemoryBufferHandle buffer) =>
            new ShareInfo(stripUnc, ShareInfoLevel.Info503, structure),
            (FunctionData functionData, out SafeGlobalMemoryBufferHandle buffer, int prefMaxLen, out uint entriesRead, out uint totalEntries, out uint resumeHandle) =>
               NativeMethods.NetShareEnum(stripUnc, 503, out buffer, NativeMethods.MaxPreferredLength, out entriesRead, out totalEntries, out resumeHandle), continueOnException))
         {
            yield return si;
            hasItems = true;
         }

         // ShareInfo503 is requested, but not supported/possible.
         // Try again with ShareInfo2 structure.
         if (!hasItems)
            foreach (var si in EnumerateNetworkObjectInternal(fd, (NativeMethods.ShareInfo2 structure, SafeGlobalMemoryBufferHandle buffer) =>
               new ShareInfo(stripUnc, ShareInfoLevel.Info2, structure),
               (FunctionData functionData, out SafeGlobalMemoryBufferHandle buffer, int prefMaxLen, out uint entriesRead, out uint totalEntries, out uint resumeHandle) =>
                  NativeMethods.NetShareEnum(stripUnc, 2, out buffer, NativeMethods.MaxPreferredLength, out entriesRead, out totalEntries, out resumeHandle), continueOnException))
            {
               yield return si;
               hasItems = true;
            }

         // ShareInfo2 is requested, but not supported/possible.
         // Try again with ShareInfo1 structure.
         if (!hasItems)
            foreach (var si in EnumerateNetworkObjectInternal(fd, (NativeMethods.ShareInfo1 structure, SafeGlobalMemoryBufferHandle buffer) =>
               new ShareInfo(stripUnc, ShareInfoLevel.Info1, structure),
               (FunctionData functionData, out  SafeGlobalMemoryBufferHandle buffer, int prefMaxLen, out uint entriesRead, out uint totalEntries, out uint resumeHandle) =>
                  NativeMethods.NetShareEnum(stripUnc, 1, out buffer, NativeMethods.MaxPreferredLength, out entriesRead, out totalEntries, out resumeHandle), continueOnException))
            {
               yield return si;
            }
      }

      #endregion // EnumerateSharesInternal

      #region GetShareInfoInternal

      /// <summary>Unified method GetShareInfoInternal() to get the <see cref="ShareInfo"/> structure of a Server Message Block (SMB) share.</summary>
      /// <returns>
      /// A <see cref="ShareInfo"/> class, or <see langword="null"/> on failure or when not available,
      /// and <paramref name="continueOnException"/> is <see langword="true"/>.
      /// </returns>
      /// <exception cref="NetworkInformationException"></exception>
      /// <param name="shareLevel">One of the <see cref="ShareInfoLevel"/> options.</param>
      /// <param name="host">A string that specifies the DNS or NetBIOS name of the specified <paramref name="host"/>.</param>
      /// <param name="share">A string that specifies the name of the Server Message Block (SMB) share.</param>
      /// <param name="continueOnException"><see langword="true"/> to suppress any Exception that might be thrown a result from a failure, such as unavailable resources.</param>
      [SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults", MessageId = "Alphaleonis.Win32.Network.NativeMethods.NetApiBufferFree(System.IntPtr)")]
      [SecurityCritical]
      internal static ShareInfo GetShareInfoInternal(ShareInfoLevel shareLevel, string host, string share, bool continueOnException)
      {
         if (Utils.IsNullOrWhiteSpace(share))
            return null;

         // When host == null, the local computer is used.
         // However, the resulting OpenResourceInfo.Host property will be empty.
         // So, explicitly state Environment.MachineName to prevent this.
         // Furthermore, the UNC prefix: \\ is not required and always removed.
         string stripUnc = Utils.IsNullOrWhiteSpace(host)
            ? Environment.MachineName
            : Path.GetRegularPathInternal(host, GetFullPathOptions.CheckInvalidPathChars).Replace(Path.UncPrefix, string.Empty);

         bool fallback = false;


      startNetShareGetInfo:

         var buffer = IntPtr.Zero;

         try
         {
            uint structureLevel = Convert.ToUInt16(shareLevel, CultureInfo.InvariantCulture);

            uint lastError = NativeMethods.NetShareGetInfo(stripUnc, share, structureLevel, out buffer);

            switch (lastError)
            {
               case Win32Errors.NERR_Success:
                  switch (shareLevel)
                  {
                     case ShareInfoLevel.Info1005:
                        return new ShareInfo(stripUnc, shareLevel, Utils.MarshalPtrToStructure<NativeMethods.ShareInfo1005>(0, buffer))
                        {
                           NetFullPath = Path.CombineInternal(false, Path.UncPrefix + stripUnc, share)
                        };

                     case ShareInfoLevel.Info503:
                        return new ShareInfo(stripUnc, shareLevel, Utils.MarshalPtrToStructure<NativeMethods.ShareInfo503>(0, buffer));

                     case ShareInfoLevel.Info2:
                        return new ShareInfo(stripUnc, shareLevel, Utils.MarshalPtrToStructure<NativeMethods.ShareInfo2>(0, buffer));

                     case ShareInfoLevel.Info1:
                        return new ShareInfo(stripUnc, shareLevel, Utils.MarshalPtrToStructure<NativeMethods.ShareInfo1>(0, buffer));
                  }
                  break;


               // Observed when ShareInfo503 is requested, but not supported/possible.
               // Fall back on ShareInfo2 structure and try again.
               case Win32Errors.RPC_X_BAD_STUB_DATA:

               case Win32Errors.ERROR_ACCESS_DENIED:
                  if (!fallback && shareLevel != ShareInfoLevel.Info2)
                  {
                     NativeMethods.NetApiBufferFree(buffer);

                     shareLevel = ShareInfoLevel.Info2;
                     fallback = true;
                     goto startNetShareGetInfo;
                  }
                  break;


               default:
                  if (!continueOnException)
                     throw new NetworkInformationException((int)lastError);
                  break;
            }

            return null;
         }
         finally
         {
            if (buffer != IntPtr.Zero)
               NativeMethods.NetApiBufferFree(buffer);
         }
      }

      #endregion // GetShareInfoInternal

      #endregion // Internal Methods
   }
}