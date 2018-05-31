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
using System.Security;

namespace Alphaleonis.Win32.Network
{
   public static partial class Host
   {
      /// <summary>[AlphaFS] Returns an enumerable collection of defined network connections on the local host.</summary>
      /// <returns>An <see cref="IEnumerable{NetworkConnectionInfo}"/> collection of defined network connections on the local host.</returns>
      [SecurityCritical]
      public static IEnumerable<NetworkConnectionInfo> EnumerateNetworkConnections()
      {
         return EnumerateNetworkConnectionsCore(null);
      }




      /// <summary>[AlphaFS] Returns an enumerable collection of defined network connections on the local host.</summary>
      /// <param name="networkConnectionID">A <see cref="Guid"/> that specifies the network connection ID.</param>
      /// <returns>An <see cref="IEnumerable{NetworkConnectionInfo}"/> collection of network connection ID's on the local host.</returns>
      [SecurityCritical]
      internal static IEnumerable<NetworkConnectionInfo> EnumerateNetworkConnectionsCore(Guid? networkConnectionID)
      {
         if (null != networkConnectionID)
            yield return new NetworkConnectionInfo(Manager.GetNetworkConnection((Guid) networkConnectionID));

         else
         {
            foreach (INetworkConnection networkConnection in Manager.GetNetworkConnections())

               yield return new NetworkConnectionInfo(networkConnection);
         }
      }
   }
}
