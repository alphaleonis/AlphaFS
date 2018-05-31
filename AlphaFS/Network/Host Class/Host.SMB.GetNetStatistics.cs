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
using System.Net.NetworkInformation;
using System.Security;
using Alphaleonis.Win32.Filesystem;

namespace Alphaleonis.Win32.Network
{
   public static partial class Host
   {
      /// <summary>[AlphaFS] Retrieves <see cref="ServerStatisticsInfo"/> operating statistics for the Server service from the local host.</summary>
      /// <returns>A <see cref="ServerStatisticsInfo"/> instance.</returns>
      /// <exception cref="NetworkInformationException"/>
      [SecurityCritical]
      public static ServerStatisticsInfo GetServerStatistics()
      {
         return (ServerStatisticsInfo) GetNetStatisticsCore(true, Environment.MachineName);
      }


      /// <summary>[AlphaFS] Retrieves <see cref="ServerStatisticsInfo"/> operating statistics for the Server service from the specified host.</summary>
      /// <returns>A <see cref="ServerStatisticsInfo"/> instance.</returns>
      /// <exception cref="NetworkInformationException"/>
      /// <param name="hostName">The DNS or NetBIOS name of the local or remote host to retrieve statistics from.</param>
      [SecurityCritical]
      public static ServerStatisticsInfo GetServerStatistics(string hostName)
      {
         return (ServerStatisticsInfo) GetNetStatisticsCore(true, hostName);
      }




      /// <summary>[AlphaFS] Retrieves <see cref="WorkstationStatisticsInfo"/> operating statistics for the Workstation service from the local host.</summary>
      /// <returns>A <see cref="WorkstationStatisticsInfo"/> instance.</returns>
      /// <exception cref="NetworkInformationException"/>
      [SecurityCritical]
      public static WorkstationStatisticsInfo GetWorkstationStatistics()
      {
         return (WorkstationStatisticsInfo) GetNetStatisticsCore(false, Environment.MachineName);
      }


      /// <summary>[AlphaFS] Retrieves <see cref="WorkstationStatisticsInfo"/> operating statistics for the Workstation service from the specified host.</summary>
      /// <returns>A <see cref="WorkstationStatisticsInfo"/> instance.</returns>
      /// <exception cref="NetworkInformationException"/>
      /// <param name="hostName">The DNS or NetBIOS name of the local or remote host to retrieve statistics from.</param>
      [SecurityCritical]
      public static WorkstationStatisticsInfo GetWorkstationStatistics(string hostName)
      {
         return (WorkstationStatisticsInfo) GetNetStatisticsCore(false, hostName);
      }




      /// <summary>[AlphaFS] Retrieves <see cref="ServerStatisticsInfo"/> or <see cref="WorkstationStatisticsInfo"/> operating statistics for the Server- or Workstation service from the specified host.</summary>
      /// <returns>A <see cref="ServerStatisticsInfo"/> or <see cref="WorkstationStatisticsInfo"/> instance, depending on the <paramref name="isServer"/> value.</returns>
      /// <exception cref="NetworkInformationException"/>
      /// <param name="isServer">true returns <see cref="ServerStatisticsInfo"/> information, false <see cref="WorkstationStatisticsInfo"/>.</param>
      /// <param name="hostName">The DNS or NetBIOS name of the local or remote host to retrieve statistics from.</param>
      [SecurityCritical]
      internal static object GetNetStatisticsCore(bool isServer, string hostName)
      {
         return isServer

            ? (object) new ServerStatisticsInfo(hostName, null)

            : new WorkstationStatisticsInfo(hostName, null);
      }


      [SecurityCritical]
      internal static T GetNetStatisticsNative<T>(bool isServer, string hostName)
      {
         SafeGlobalMemoryBufferHandle safeBuffer;


         // hostName is allowed to be null.

         var stripUnc = !Utils.IsNullOrWhiteSpace(hostName) ? Path.GetRegularPathCore(hostName, GetFullPathOptions.CheckInvalidPathChars, false).Replace(Path.UncPrefix, string.Empty) : null;

         var lastError = NativeMethods.NetStatisticsGet(stripUnc, isServer ? "LanmanServer" : "LanmanWorkstation", 0, 0, out safeBuffer);


         using (safeBuffer)
         {
            if (lastError != Win32Errors.NERR_Success)
               throw new NetworkInformationException((int) lastError);


            return safeBuffer.PtrToStructure<T>(0);
         }
      }
   }
}
