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
      #region EnumerateOpenConnections

      /// <summary>Enumerates open connections from the local host.</summary>
      /// <returns><see cref="OpenConnectionInfo"/> connection information from the local host.</returns>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="NetworkInformationException"/>
      [SecurityCritical]
      public static IEnumerable<OpenConnectionInfo> EnumerateOpenConnections()
      {
         return EnumerateOpenConnectionsCore(null, null, false);
      }

      /// <summary>Enumerates open connections from the specified host.</summary>
      /// <returns><see cref="OpenConnectionInfo"/> connection information from the specified <paramref name="host"/>.</returns>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="NetworkInformationException"/>
      /// <param name="host">The DNS or NetBIOS name of the remote server. <see langword="null"/> refers to the local host.</param>
      /// <param name="share">The name of the Server Message Block (SMB) share.</param>
      /// <param name="continueOnException">
      ///   <para><see langword="true"/> suppress any Exception that might be thrown as a result from a failure,</para>
      ///   <para>such as unavailable resources.</para>
      /// </param>
      [SecurityCritical]
      public static IEnumerable<OpenConnectionInfo> EnumerateOpenConnections(string host, string share, bool continueOnException)
      {
         return EnumerateOpenConnectionsCore(host, share, continueOnException);
      }

      #endregion // EnumerateOpenConnections

      #region EnumerateShares

      /// <summary>Enumerates Server Message Block (SMB) shares from the local host.</summary>
      /// <returns><see cref="IEnumerable{ShareInfo}"/> shares from the specified host.</returns>
      /// <remarks>This method also enumerates hidden shares.</remarks>
      [SecurityCritical]
      public static IEnumerable<ShareInfo> EnumerateShares()
      {
         return EnumerateSharesCore(null, ShareType.All, false);
      }

      /// <summary>Enumerates Server Message Block (SMB) shares from the local host.</summary>
      /// <returns><see cref="IEnumerable{ShareInfo}"/> shares from the specified host.</returns>
      /// <remarks>This method also enumerates hidden shares.</remarks>
      /// <param name="continueOnException"><see langword="true"/> suppress any Exception that might be thrown as a result from a failure, such as unavailable resources.</param>
      [SecurityCritical]
      public static IEnumerable<ShareInfo> EnumerateShares(bool continueOnException)
      {
         return EnumerateSharesCore(null, ShareType.All, continueOnException);
      }

      /// <summary>Enumerates Server Message Block (SMB) shares from the local host.</summary>
      /// <returns><see cref="IEnumerable{ShareInfo}"/> shares from the specified host.</returns>
      /// <remarks>This method also enumerates hidden shares.</remarks>
      /// <param name="shareType">The type of the shared resource to retrieve.</param>
      /// <param name="continueOnException"><see langword="true"/> suppress any Exception that might be thrown as a result from a failure, such as unavailable resources.</param>
      [SecurityCritical]
      public static IEnumerable<ShareInfo> EnumerateShares(ShareType shareType, bool continueOnException)
      {
         return EnumerateSharesCore(null, shareType, continueOnException);
      }

      /// <summary>Enumerates Server Message Block (SMB) shares from the specified <paramref name="host"/>.</summary>
      /// <returns><see cref="IEnumerable{ShareInfo}"/> shares from the specified host.</returns>
      /// <remarks>This method also enumerates hidden shares.</remarks>
      /// <param name="host">The DNS or NetBIOS name of the specified host.</param>
      [SecurityCritical]
      public static IEnumerable<ShareInfo> EnumerateShares(string host)
      {
         return EnumerateSharesCore(host, ShareType.All, false);
      }

      /// <summary>Enumerates Server Message Block (SMB) shares from the specified <paramref name="host"/>.</summary>
      /// <returns><see cref="IEnumerable{ShareInfo}"/> shares from the specified host.</returns>
      /// <remarks>This method also enumerates hidden shares.</remarks>
      /// <param name="host">The DNS or NetBIOS name of the specified host.</param>
      /// <param name="continueOnException"><see langword="true"/> suppress any Exception that might be thrown as a result from a failure, such as unavailable resources.</param>
      [SecurityCritical]
      public static IEnumerable<ShareInfo> EnumerateShares(string host, bool continueOnException)
      {
         return EnumerateSharesCore(host, ShareType.All, continueOnException);
      }

      /// <summary>Enumerates Server Message Block (SMB) shares from the specified <paramref name="host"/>.</summary>
      /// <returns><see cref="IEnumerable{ShareInfo}"/> shares from the specified host.</returns>
      /// <remarks>This method also enumerates hidden shares.</remarks>
      /// <param name="host">The DNS or NetBIOS name of the specified host.</param>
      /// <param name="shareType">The type of the shared resource to retrieve.</param>
      /// <param name="continueOnException"><see langword="true"/> suppress any Exception that might be thrown as a result from a failure, such as unavailable resources.</param>
      [SecurityCritical]
      public static IEnumerable<ShareInfo> EnumerateShares(string host, ShareType shareType, bool continueOnException)
      {
         return EnumerateSharesCore(host, shareType, continueOnException);
      }

      #endregion // EnumerateShares

      #region GetHostShareFromPath

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
               uri.AbsolutePath.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar)
            };
         }

         return null;
      }

      #endregion // GetHostShareFromPath

      #region GetShareInfo

      /// <summary>Retrieves information about the Server Message Block (SMB) share as defined on the specified host.</summary>
      /// <returns>A <see cref="ShareInfo"/> class, or <see langword="null"/> on failure or when not available, and <paramref name="continueOnException"/> is <see langword="true"/>.</returns>
      /// <param name="uncPath">The share in the format: \\host\share.</param>
      /// <param name="continueOnException"><see langword="true"/> to suppress any Exception that might be thrown as a result from a failure, such as unavailable resources.</param>
      [SecurityCritical]
      public static ShareInfo GetShareInfo(string uncPath, bool continueOnException)
      {
         string[] unc = GetHostShareFromPath(uncPath);
         return GetShareInfoCore(ShareInfoLevel.Info503, unc[0], unc[1], continueOnException);
      }

      /// <summary>Retrieves information about the Server Message Block (SMB) share as defined on the specified host.</summary>
      /// <returns>A <see cref="ShareInfo"/> class, or <see langword="null"/> on failure or when not available, and <paramref name="continueOnException"/> is <see langword="true"/>.</returns>
      /// <param name="shareLevel">One of the <see cref="ShareInfoLevel"/> options.</param>
      /// <param name="uncPath">The share in the format: \\host\share.</param>
      /// <param name="continueOnException"><see langword="true"/> to suppress any Exception that might be thrown as a result from a failure, such as unavailable resources.</param>
      [SecurityCritical]
      public static ShareInfo GetShareInfo(ShareInfoLevel shareLevel, string uncPath, bool continueOnException)
      {
         string[] unc = GetHostShareFromPath(uncPath);
         return GetShareInfoCore(shareLevel, unc[0], unc[1], continueOnException);
      }

      /// <summary>Retrieves information about the Server Message Block (SMB) share as defined on the specified host.</summary>
      /// <returns>A <see cref="ShareInfo"/> class, or <see langword="null"/> on failure or when not available, and <paramref name="continueOnException"/> is <see langword="true"/>.</returns>
      /// <param name="host">The DNS or NetBIOS name of the specified host.</param>
      /// <param name="share">The name of the Server Message Block (SMB) share.</param>
      /// <param name="continueOnException"><see langword="true"/> to suppress any Exception that might be thrown as a result from a failure, such as unavailable resources.</param>
      [SecurityCritical]
      public static ShareInfo GetShareInfo(string host, string share, bool continueOnException)
      {
         return GetShareInfoCore(ShareInfoLevel.Info503, host, share, continueOnException);
      }

      /// <summary>Retrieves information about the Server Message Block (SMB) share as defined on the specified host.</summary>
      /// <returns>A <see cref="ShareInfo"/> class, or <see langword="null"/> on failure or when not available, and <paramref name="continueOnException"/> is <see langword="true"/>.</returns>
      /// <param name="shareLevel">One of the <see cref="ShareInfoLevel"/> options.</param>
      /// <param name="host">A string that specifies the DNS or NetBIOS name of the specified <paramref name="host"/>.</param>
      /// <param name="share">A string that specifies the name of the Server Message Block (SMB) share.</param>
      /// <param name="continueOnException"><see langword="true"/> to suppress any Exception that might be thrown as a result from a failure, such as unavailable resources.</param>
      [SecurityCritical]
      public static ShareInfo GetShareInfo(ShareInfoLevel shareLevel, string host, string share, bool continueOnException)
      {
         return GetShareInfoCore(shareLevel, host, share, continueOnException);
      }

      #endregion // GetShareInfo

      #region Internal Methods

      #region EnumerateOpenConnectionsCore

      /// <summary>Enumerates open connections from the specified host.</summary>
      /// <returns><see cref="OpenConnectionInfo"/> connection information from the specified <paramref name="host"/>.</returns>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="NetworkInformationException"/>
      /// <param name="host">The DNS or NetBIOS name of the remote server. <see langword="null"/> refers to the local host.</param>
      /// <param name="share">The name of the Server Message Block (SMB) share.</param>
      /// <param name="continueOnException"><see langword="true"/> suppress any Exception that might be thrown as a result from a failure, such as unavailable resources.</param>
      [SecurityCritical]
      private static IEnumerable<OpenConnectionInfo> EnumerateOpenConnectionsCore(string host, string share, bool continueOnException)
      {
         if (Utils.IsNullOrWhiteSpace(share))
            throw new ArgumentNullException("share");

         return EnumerateNetworkObjectCore(new FunctionData { ExtraData1 = share }, (NativeMethods.CONNECTION_INFO_1 structure, SafeGlobalMemoryBufferHandle buffer) =>

               new OpenConnectionInfo(host, structure),

            (FunctionData functionData, out SafeGlobalMemoryBufferHandle buffer, int prefMaxLen, out uint entriesRead, out uint totalEntries, out uint resumeHandle) =>
            {
               // When host == null, the local computer is used.
               // However, the resulting OpenResourceInfo.Host property will be empty.
               // So, explicitly state Environment.MachineName to prevent this.
               // Furthermore, the UNC prefix: \\ is not required and always removed.
               string stripUnc = Utils.IsNullOrWhiteSpace(host) ? Environment.MachineName : Path.GetRegularPathCore(host, GetFullPathOptions.CheckInvalidPathChars, false).Replace(Path.UncPrefix, string.Empty);

               return NativeMethods.NetConnectionEnum(stripUnc, functionData.ExtraData1, 1, out buffer, NativeMethods.MaxPreferredLength, out entriesRead, out totalEntries, out resumeHandle);

            },
            continueOnException);
      }

      #endregion // EnumerateOpenConnectionsCore

      #region EnumerateSharesCore

      /// <summary>Enumerates Server Message Block (SMB) shares from a local or remote host.</summary>
      /// <returns><see cref="IEnumerable{ShareInfo}"/> shares from the specified host.</returns>
      /// <remarks>This method also enumerates hidden shares.</remarks>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="NetworkInformationException"/>
      /// <param name="host">The DNS or NetBIOS name of the specified host.</param>
      /// <param name="shareType">The type of the shared resource to retrieve.</param>
      /// <param name="continueOnException"><see langword="true"/> suppress any Exception that might be thrown as a result from a failure, such as unavailable resources.</param>
      [SecurityCritical]
      internal static IEnumerable<ShareInfo> EnumerateSharesCore(string host, ShareType shareType, bool continueOnException)
      {
         // When host == null, the local computer is used.
         // However, the resulting OpenResourceInfo.Host property will be empty.
         // So, explicitly state Environment.MachineName to prevent this.
         // Furthermore, the UNC prefix: \\ is not required and always removed.
         string stripUnc = Utils.IsNullOrWhiteSpace(host)
            ? Environment.MachineName
            : Path.GetRegularPathCore(host, GetFullPathOptions.CheckInvalidPathChars, false).Replace(Path.UncPrefix, string.Empty);

         var fd = new FunctionData();
         bool hasItems = false;
         bool yieldAll = shareType == ShareType.All;

         // Try SHARE_INFO_503 structure.
         foreach (var si in EnumerateNetworkObjectCore(fd, (NativeMethods.SHARE_INFO_503 structure, SafeGlobalMemoryBufferHandle buffer) =>
            new ShareInfo(stripUnc, ShareInfoLevel.Info503, structure),
            (FunctionData functionData, out SafeGlobalMemoryBufferHandle buffer, int prefMaxLen, out uint entriesRead, out uint totalEntries, out uint resumeHandle) =>
               NativeMethods.NetShareEnum(stripUnc, 503, out buffer, NativeMethods.MaxPreferredLength, out entriesRead, out totalEntries, out resumeHandle), continueOnException).Where(si => yieldAll || si.ShareType == shareType))
         {
            yield return si;
            hasItems = true;
         }

         // SHARE_INFO_503 is requested, but not supported/possible.
         // Try again with SHARE_INFO_2 structure.
         if (!hasItems)
            foreach (var si in EnumerateNetworkObjectCore(fd, (NativeMethods.SHARE_INFO_2 structure, SafeGlobalMemoryBufferHandle buffer) =>
               new ShareInfo(stripUnc, ShareInfoLevel.Info2, structure),
               (FunctionData functionData, out SafeGlobalMemoryBufferHandle buffer, int prefMaxLen, out uint entriesRead, out uint totalEntries, out uint resumeHandle) =>
                  NativeMethods.NetShareEnum(stripUnc, 2, out buffer, NativeMethods.MaxPreferredLength, out entriesRead, out totalEntries, out resumeHandle), continueOnException).Where(si => yieldAll || si.ShareType == shareType))
            {
               yield return si;
               hasItems = true;
            }

         // SHARE_INFO_2 is requested, but not supported/possible.
         // Try again with SHARE_INFO_1 structure.
         if (!hasItems)
            foreach (var si in EnumerateNetworkObjectCore(fd, (NativeMethods.SHARE_INFO_1 structure, SafeGlobalMemoryBufferHandle buffer) =>
               new ShareInfo(stripUnc, ShareInfoLevel.Info1, structure),
               (FunctionData functionData, out  SafeGlobalMemoryBufferHandle buffer, int prefMaxLen, out uint entriesRead, out uint totalEntries, out uint resumeHandle) =>
                  NativeMethods.NetShareEnum(stripUnc, 1, out buffer, NativeMethods.MaxPreferredLength, out entriesRead, out totalEntries, out resumeHandle), continueOnException).Where(si => yieldAll || si.ShareType == shareType))
            {
               yield return si;
            }
      }

      #endregion // EnumerateSharesCore

      #region GetShareInfoCore

      /// <summary>Gets the <see cref="ShareInfo"/> structure of a Server Message Block (SMB) share.</summary>
      /// <returns>A <see cref="ShareInfo"/> class, or <see langword="null"/> on failure or when not available, and <paramref name="continueOnException"/> is <see langword="true"/>.</returns>
      /// <exception cref="NetworkInformationException"/>
      /// <param name="shareLevel">One of the <see cref="ShareInfoLevel"/> options.</param>
      /// <param name="host">A string that specifies the DNS or NetBIOS name of the specified <paramref name="host"/>.</param>
      /// <param name="share">A string that specifies the name of the Server Message Block (SMB) share.</param>
      /// <param name="continueOnException"><see langword="true"/> to suppress any Exception that might be thrown as a result from a failure, such as unavailable resources.</param>
      [SecurityCritical]
      internal static ShareInfo GetShareInfoCore(ShareInfoLevel shareLevel, string host, string share, bool continueOnException)
      {
         if (Utils.IsNullOrWhiteSpace(share))
            return null;

         // When host == null, the local computer is used.
         // However, the resulting OpenResourceInfo.Host property will be empty.
         // So, explicitly state Environment.MachineName to prevent this.
         // Furthermore, the UNC prefix: \\ is not required and always removed.
         string stripUnc = Utils.IsNullOrWhiteSpace(host)
            ? Environment.MachineName
            : Path.GetRegularPathCore(host, GetFullPathOptions.CheckInvalidPathChars, false).Replace(Path.UncPrefix, string.Empty);

         bool fallback = false;


         startNetShareGetInfo:

         SafeGlobalMemoryBufferHandle safeBuffer;

         uint structureLevel = Convert.ToUInt16(shareLevel, CultureInfo.InvariantCulture);
         uint lastError = NativeMethods.NetShareGetInfo(stripUnc, share, structureLevel, out safeBuffer);

         using (safeBuffer)
         {
            switch (lastError)
            {
               case Win32Errors.NERR_Success:
                  switch (shareLevel)
                  {
                     case ShareInfoLevel.Info1005:
                        return new ShareInfo(stripUnc, shareLevel, safeBuffer.PtrToStructure<NativeMethods.SHARE_INFO_1005>(0))
                        {
                           NetFullPath = Path.CombineCore(false, Path.UncPrefix + stripUnc, share)
                        };

                     case ShareInfoLevel.Info503:
                        return new ShareInfo(stripUnc, shareLevel, safeBuffer.PtrToStructure<NativeMethods.SHARE_INFO_503>(0));

                     case ShareInfoLevel.Info2:
                        return new ShareInfo(stripUnc, shareLevel, safeBuffer.PtrToStructure<NativeMethods.SHARE_INFO_2>(0));

                     case ShareInfoLevel.Info1:
                        return new ShareInfo(stripUnc, shareLevel, safeBuffer.PtrToStructure<NativeMethods.SHARE_INFO_1>(0));
                  }
                  break;


               // Observed when SHARE_INFO_503 is requested, but not supported/possible.
               // Fall back on SHARE_INFO_2 structure and try again.
               case Win32Errors.RPC_X_BAD_STUB_DATA:

               case Win32Errors.ERROR_ACCESS_DENIED:
                  if (!fallback && shareLevel != ShareInfoLevel.Info2)
                  {
                     shareLevel = ShareInfoLevel.Info2;
                     fallback = true;
                     goto startNetShareGetInfo;
                  }
                  break;

               default:
                  if (!continueOnException)
                     throw new NetworkInformationException((int) lastError);
                  break;
            }

            return null;
         }
      }

      #endregion // GetShareInfoCore

      #endregion // Internal Methods
   }
}
