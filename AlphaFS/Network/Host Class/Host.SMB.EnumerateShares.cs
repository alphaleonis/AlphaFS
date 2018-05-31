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
using System.Linq;
using System.Net.NetworkInformation;
using System.Security;
using Alphaleonis.Win32.Filesystem;

namespace Alphaleonis.Win32.Network
{
   public static partial class Host
   {
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
         var fd = new FunctionData();
         var hasItems = false;
         var yieldAll = shareType == ShareType.All;


         // When host == null, the local computer is used.
         // However, the resulting OpenResourceInfo.Host property will be empty.
         // So, explicitly state Environment.MachineName to prevent this.
         // Furthermore, the UNC prefix: \\ is not required and always removed.
         var stripUnc = Utils.IsNullOrWhiteSpace(host) ? Environment.MachineName : Path.GetRegularPathCore(host, GetFullPathOptions.CheckInvalidPathChars, false).Replace(Path.UncPrefix, string.Empty);
         

         // Start with SHARE_INFO_503 structure.

         foreach (var shareInfo in EnumerateNetworkObjectCore(fd, (NativeMethods.SHARE_INFO_503 structure, SafeGlobalMemoryBufferHandle buffer) => new ShareInfo(stripUnc, ShareInfoLevel.Info503, structure),

            (FunctionData functionData, out SafeGlobalMemoryBufferHandle buffer, int prefMaxLen, out uint entriesRead, out uint totalEntries, out uint resumeHandle) =>

               NativeMethods.NetShareEnum(stripUnc, 503, out buffer, NativeMethods.MaxPreferredLength, out entriesRead, out totalEntries, out resumeHandle), continueOnException).Where(si => yieldAll || si.ShareType == shareType))
         {
            yield return shareInfo;
            hasItems = true;
         }

         if (hasItems)
            yield break;


         // Fallback on SHARE_INFO_2 structure.

         foreach (var shareInfo in EnumerateNetworkObjectCore(fd, (NativeMethods.SHARE_INFO_2 structure, SafeGlobalMemoryBufferHandle buffer) => new ShareInfo(stripUnc, ShareInfoLevel.Info2, structure),

            (FunctionData functionData, out SafeGlobalMemoryBufferHandle buffer, int prefMaxLen, out uint entriesRead, out uint totalEntries, out uint resumeHandle) =>

               NativeMethods.NetShareEnum(stripUnc, 2, out buffer, NativeMethods.MaxPreferredLength, out entriesRead, out totalEntries, out resumeHandle), continueOnException).Where(si => yieldAll || si.ShareType == shareType))
         {
            yield return shareInfo;
            hasItems = true;
         }

         if (hasItems)
            yield break;


         // Fallback on SHARE_INFO_1 structure.

         foreach (var shareInfo in EnumerateNetworkObjectCore(fd, (NativeMethods.SHARE_INFO_1 structure, SafeGlobalMemoryBufferHandle buffer) => new ShareInfo(stripUnc, ShareInfoLevel.Info1, structure),

            (FunctionData functionData, out SafeGlobalMemoryBufferHandle buffer, int prefMaxLen, out uint entriesRead, out uint totalEntries, out uint resumeHandle) =>

               NativeMethods.NetShareEnum(stripUnc, 1, out buffer, NativeMethods.MaxPreferredLength, out entriesRead, out totalEntries, out resumeHandle), continueOnException).Where(si => yieldAll || si.ShareType == shareType))
         {
            yield return shareInfo;
         }
      }
   }
}
