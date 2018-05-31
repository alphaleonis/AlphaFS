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
using System.Net.NetworkInformation;
using System.Security;
using Alphaleonis.Win32.Filesystem;

namespace Alphaleonis.Win32.Network
{
   public static partial class Host
   {
      /// <summary>[AlphaFS] Enumerates sessions established on the local host.</summary>
      /// <returns>An <see cref="IEnumerable{SessionInfo}"/> collection from the local host.</returns>
      [SecurityCritical]
      public static IEnumerable<SessionInfo> EnumerateSessions()
      {
         return EnumerateSessionsCore(Environment.MachineName, null, null);
      }


      /// <summary>[AlphaFS] Enumerates sessions established on the specified <paramref name="hostName"/>.</summary>
      /// <returns>An <see cref="IEnumerable{SessionInfo}"/> collection from the specified <paramref name="hostName"/>.</returns>
      /// <param name="hostName">The DNS or NetBIOS name of the specified host. If this parameter is <c>null</c>, the local Computer is used.</param>
      [SecurityCritical]
      public static IEnumerable<SessionInfo> EnumerateSessions(string hostName)
      {
         return EnumerateSessionsCore(hostName, null, null);
      }


      /// <summary>[AlphaFS] Enumerates sessions established on the specified <paramref name="hostName"/>.</summary>
      /// <returns>An <see cref="IEnumerable{SessionInfo}"/> collection from the specified <paramref name="hostName"/>.</returns>
      /// <param name="hostName">The DNS or NetBIOS name of the specified host. If this parameter is <c>null</c>, the local Computer is used.</param>
      /// <param name="clientName">The name of the Computer session for which information is to be returned. If this parameter is <c>null</c>, information for all Computer sessions on the server is returned.</param>
      [SecurityCritical]
      public static IEnumerable<SessionInfo> EnumerateSessions(string hostName, string clientName)
      {
         return EnumerateSessionsCore(hostName, clientName, null);
      }


      /// <summary>[AlphaFS] Enumerates sessions established on the specified <paramref name="hostName"/>.</summary>
      /// <returns>An <see cref="IEnumerable{SessionInfo}"/> collection from the specified <paramref name="hostName"/>.</returns>
      /// <param name="hostName">The DNS or NetBIOS name of the specified host. If this parameter is <c>null</c>, the local Computer is used.</param>
      /// <param name="clientName">The name of the Computer session for which information is to be returned. If this parameter is <c>null</c>, information for all Computer sessions on the server is returned.</param>
      /// <param name="userName">The name of the user for which information is to be returned. If this parameter is <c>null</c>, information for all users is returned.</param>
      [SecurityCritical]
      public static IEnumerable<SessionInfo> EnumerateSessions(string hostName, string clientName, string userName)
      {
         return EnumerateSessionsCore(hostName, clientName, userName);
      }


      
      
      /// <summary>[AlphaFS] Enumerates sessions established on the specified <paramref name="hostName"/>.</summary>
      /// <returns>An <see cref="IEnumerable{SessionInfo}"/> collection from the specified <paramref name="hostName"/>.</returns>
      /// <exception cref="NetworkInformationException"/>
      /// <param name="hostName">The DNS or NetBIOS name of the specified host. If this parameter is <c>null</c>, the local Computer is used.</param>
      /// <param name="clientName">The name of the Computer session for which information is to be returned. If this parameter is <c>null</c>, information for all Computer sessions on the server is returned.</param>
      /// <param name="userName">The name of the user for which information is to be returned. If this parameter is <c>null</c>, information for all users is returned.</param>
      [SecurityCritical]
      internal static IEnumerable<SessionInfo> EnumerateSessionsCore(string hostName, string clientName, string userName)
      {
         var fd = new FunctionData();
         var hasItems = false;


         // hostName is allowed to be null.
         // clientName is allowed to be null.
         // userName is allowed to be null.

         var stripUnc = !Utils.IsNullOrWhiteSpace(hostName) ? Path.GetRegularPathCore(hostName, GetFullPathOptions.CheckInvalidPathChars, false).Replace(Path.UncPrefix, string.Empty) : null;


         // Start with SESSION_INFO_502 structure.

         foreach (var sessionInfo in EnumerateNetworkObjectCore(fd, (NativeMethods.SESSION_INFO_502 structure, SafeGlobalMemoryBufferHandle buffer) => new SessionInfo(stripUnc, SessionInfoLevel.Info502, structure),

            (FunctionData functionData, out SafeGlobalMemoryBufferHandle buffer, int prefMaxLen, out uint entriesRead, out uint totalEntries, out uint resumeHandle) =>

               NativeMethods.NetSessionEnum(stripUnc, clientName, userName, SessionInfoLevel.Info502, out buffer, NativeMethods.MaxPreferredLength, out entriesRead, out totalEntries, out resumeHandle), true))
         {
            yield return sessionInfo;
            hasItems = true;
         }

         if (hasItems)
            yield break;


         // Fallback on SESSION_INFO_2 structure.

         foreach (var sessionInfo in EnumerateNetworkObjectCore(fd, (NativeMethods.SESSION_INFO_2 structure, SafeGlobalMemoryBufferHandle buffer) => new SessionInfo(stripUnc, SessionInfoLevel.Info2, structure),

            (FunctionData functionData, out SafeGlobalMemoryBufferHandle buffer, int prefMaxLen, out uint entriesRead, out uint totalEntries, out uint resumeHandle) =>

               NativeMethods.NetSessionEnum(stripUnc, clientName, userName, SessionInfoLevel.Info2, out buffer, NativeMethods.MaxPreferredLength, out entriesRead, out totalEntries, out resumeHandle), true))
         {
            yield return sessionInfo;
            hasItems = true;
         }

         if (hasItems)
            yield break;


         // Fallback on SHARE_INFO_1 structure.

         foreach (var sessionInfo in EnumerateNetworkObjectCore(fd, (NativeMethods.SHARE_INFO_1 structure, SafeGlobalMemoryBufferHandle buffer) => new SessionInfo(stripUnc, SessionInfoLevel.Info1, structure),

            (FunctionData functionData, out SafeGlobalMemoryBufferHandle buffer, int prefMaxLen, out uint entriesRead, out uint totalEntries, out uint resumeHandle) =>

               NativeMethods.NetSessionEnum(stripUnc, clientName, userName, SessionInfoLevel.Info1, out buffer, NativeMethods.MaxPreferredLength, out entriesRead, out totalEntries, out resumeHandle), true))
         {
            yield return sessionInfo;
            hasItems = true;
         }

         if (hasItems)
            yield break;


         // Fallback on SESSION_INFO_10 structure.

         foreach (var sessionInfo in EnumerateNetworkObjectCore(fd, (NativeMethods.SESSION_INFO_10 structure, SafeGlobalMemoryBufferHandle buffer) => new SessionInfo(stripUnc, SessionInfoLevel.Info10, structure),

            (FunctionData functionData, out SafeGlobalMemoryBufferHandle buffer, int prefMaxLen, out uint entriesRead, out uint totalEntries, out uint resumeHandle) =>

               NativeMethods.NetSessionEnum(stripUnc, clientName, userName, SessionInfoLevel.Info10, out buffer, NativeMethods.MaxPreferredLength, out entriesRead, out totalEntries, out resumeHandle), true))
         {
            yield return sessionInfo;
            hasItems = true;
         }

         if (hasItems)
            yield break;


         // Fallback on SESSION_INFO_0 structure.

         foreach (var sessionInfo in EnumerateNetworkObjectCore(fd, (NativeMethods.SESSION_INFO_0 structure, SafeGlobalMemoryBufferHandle buffer) => new SessionInfo(stripUnc, SessionInfoLevel.Info0, structure),

            (FunctionData functionData, out SafeGlobalMemoryBufferHandle buffer, int prefMaxLen, out uint entriesRead, out uint totalEntries, out uint resumeHandle) =>

               NativeMethods.NetSessionEnum(stripUnc, clientName, userName, SessionInfoLevel.Info0, out buffer, NativeMethods.MaxPreferredLength, out entriesRead, out totalEntries, out resumeHandle), true))
         {
            yield return sessionInfo;
         }
      }
   }
}
