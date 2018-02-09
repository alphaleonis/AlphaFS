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
using System.Security;

namespace Alphaleonis.Win32.Network
{
   partial class Host
   {
      /// <summary>
      /// 
      /// </summary>
      /// <returns></returns>
      [SecurityCritical]
      public static ServerStatisticsInfo GetServerNetStatistics()
      {
         return GetNetStatisticsCore(true, Environment.MachineName);
      }


      /// <summary>
      /// 
      /// </summary>
      /// <param name="hostName"></param>
      /// <returns></returns>
      [SecurityCritical]
      public static ServerStatisticsInfo GetServerNetStatistics(string hostName)
      {
         return GetNetStatisticsCore(true, hostName);
      }


      /// <summary>
      /// 
      /// </summary>
      /// <param name="hostName"></param>
      /// <returns></returns>
      [SecurityCritical]
      public static ServerStatisticsInfo GetWorkstationNetStatistics(string hostName)
      {
         return GetNetStatisticsCore(false, hostName);
      }




      /// <summary>
      /// 
      /// </summary>
      /// <param name="isServer"></param>
      /// <param name="hostName"></param>
      /// <returns></returns>
      [SecurityCritical]
      internal static ServerStatisticsInfo GetNetStatisticsCore(bool isServer, string hostName)
      {
         if (Utils.IsNullOrWhiteSpace(hostName))
            throw new ArgumentNullException("hostName");


         return new ServerStatisticsInfo(hostName, GetNetStatisticsNative(isServer, hostName));
      }

      
      [SecurityCritical]
      internal static NativeMethods.STAT_SERVER_0 GetNetStatisticsNative(bool isServer, string hostName)
      {
         SafeGlobalMemoryBufferHandle safeBuffer;

         var lastError = NativeMethods.NetStatisticsGet(hostName, isServer ? "LanmanServer" : "LanmanWorkstation", 0, 0, out safeBuffer);


         using (safeBuffer)
         {
            if (lastError != Win32Errors.NERR_Success)
               NativeError.ThrowException(lastError, hostName);


            return safeBuffer.PtrToStructure<NativeMethods.STAT_SERVER_0>(0);
         }
      }
   }
}
