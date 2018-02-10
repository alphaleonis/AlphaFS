/*  Copyright (C) 2008-2017 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
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
using System.Net.NetworkInformation;
using System.Security;
using Alphaleonis.Win32.Filesystem;

namespace Alphaleonis.Win32.Network
{
   partial class Host
   {
      /// <summary>[AlphaFS] Enumerates sessions established on the local host.</summary>
      /// <returns>An <see cref="IEnumerable{SessionInfo}"/> collection from the local host.</returns>
      [SecurityCritical]
      public static IEnumerable<SessionInfo> EnumerateSessions()
      {
         return EnumerateSessionsCore(Environment.MachineName);
      }


      /// <summary>[AlphaFS] Enumerates sessions established on the specified <paramref name="hostName"/>.</summary>
      /// <returns>An <see cref="IEnumerable{SessionInfo}"/> collection from the specified <paramref name="hostName"/>.</returns>
      /// <param name="hostName">The DNS or NetBIOS name of the specified host.</param>
      [SecurityCritical]
      public static IEnumerable<SessionInfo> EnumerateSessions(string hostName)
      {
         return EnumerateSessionsCore(hostName);
      }


      /// <summary>[AlphaFS] Enumerates sessions established on the specified <paramref name="hostName"/>.</summary>
      /// <returns>An <see cref="IEnumerable{SessionInfo}"/> collection from the specified <paramref name="hostName"/>.</returns>
      /// <exception cref="NetworkInformationException"/>
      /// <param name="hostName">The DNS or NetBIOS name of the specified host.</param>
      [SecurityCritical]
      internal static IEnumerable<SessionInfo> EnumerateSessionsCore(string hostName)
      {
         var fd = new FunctionData();
         var hasItems = false;

         // When host == null, the local computer is used.
         // However, the resulting Host property will be empty.
         // So, explicitly state Environment.MachineName to prevent this.
         // Furthermore, the UNC prefix: \\ is not required and always removed.
         var stripUnc = Utils.IsNullOrWhiteSpace(hostName) ? Environment.MachineName : Path.GetRegularPathCore(hostName, GetFullPathOptions.CheckInvalidPathChars, false).Replace(Path.UncPrefix, string.Empty);


         // Try SESSION_INFO_502 structure.

         foreach (var sessionInfo in EnumerateNetworkObjectCore(fd, (NativeMethods.SESSION_INFO_502 structure, SafeGlobalMemoryBufferHandle buffer) => new SessionInfo(hostName, SessionInfoLevel.Info502, structure),

            (FunctionData functionData, out SafeGlobalMemoryBufferHandle buffer, int prefMaxLen, out uint entriesRead, out uint totalEntries, out uint resumeHandle) =>

               NativeMethods.NetSessionEnum(stripUnc, null, null, SessionInfoLevel.Info502, out buffer, NativeMethods.MaxPreferredLength, out entriesRead, out totalEntries, out resumeHandle), true))
         {
            yield return sessionInfo;
            hasItems = true;
         }

         if (hasItems)
            yield break;


         // Fallback on SESSION_INFO_2 structure.

         foreach (var sessionInfo in EnumerateNetworkObjectCore(fd, (NativeMethods.SESSION_INFO_2 structure, SafeGlobalMemoryBufferHandle buffer) => new SessionInfo(hostName, SessionInfoLevel.Info2, structure),

            (FunctionData functionData, out SafeGlobalMemoryBufferHandle buffer, int prefMaxLen, out uint entriesRead, out uint totalEntries, out uint resumeHandle) =>

               NativeMethods.NetSessionEnum(stripUnc, null, null, SessionInfoLevel.Info2, out buffer, NativeMethods.MaxPreferredLength, out entriesRead, out totalEntries, out resumeHandle), true))
         {
            yield return sessionInfo;
            hasItems = true;
         }

         if (hasItems)
            yield break;


         // Fallback on SHARE_INFO_1 structure.

         foreach (var sessionInfo in EnumerateNetworkObjectCore(fd, (NativeMethods.SHARE_INFO_1 structure, SafeGlobalMemoryBufferHandle buffer) => new SessionInfo(hostName, SessionInfoLevel.Info1, structure),

            (FunctionData functionData, out SafeGlobalMemoryBufferHandle buffer, int prefMaxLen, out uint entriesRead, out uint totalEntries, out uint resumeHandle) =>

               NativeMethods.NetSessionEnum(stripUnc, null, null, SessionInfoLevel.Info1, out buffer, NativeMethods.MaxPreferredLength, out entriesRead, out totalEntries, out resumeHandle), true))
         {
            yield return sessionInfo;
            hasItems = true;
         }

         if (hasItems)
            yield break;


         // Fallback on SESSION_INFO_10 structure.

         foreach (var sessionInfo in EnumerateNetworkObjectCore(fd, (NativeMethods.SESSION_INFO_10 structure, SafeGlobalMemoryBufferHandle buffer) => new SessionInfo(hostName, SessionInfoLevel.Info10, structure),

            (FunctionData functionData, out SafeGlobalMemoryBufferHandle buffer, int prefMaxLen, out uint entriesRead, out uint totalEntries, out uint resumeHandle) =>

               NativeMethods.NetSessionEnum(stripUnc, null, null, SessionInfoLevel.Info10, out buffer, NativeMethods.MaxPreferredLength, out entriesRead, out totalEntries, out resumeHandle), true))
         {
            yield return sessionInfo;
            hasItems = true;
         }

         if (hasItems)
            yield break;


         // Fallback on SESSION_INFO_0 structure.

         foreach (var sessionInfo in EnumerateNetworkObjectCore(fd, (NativeMethods.SESSION_INFO_0 structure, SafeGlobalMemoryBufferHandle buffer) => new SessionInfo(hostName, SessionInfoLevel.Info0, structure),

            (FunctionData functionData, out SafeGlobalMemoryBufferHandle buffer, int prefMaxLen, out uint entriesRead, out uint totalEntries, out uint resumeHandle) =>

               NativeMethods.NetSessionEnum(stripUnc, null, null, SessionInfoLevel.Info0, out buffer, NativeMethods.MaxPreferredLength, out entriesRead, out totalEntries, out resumeHandle), true))
         {
            yield return sessionInfo;
         }
      }
   }
}
