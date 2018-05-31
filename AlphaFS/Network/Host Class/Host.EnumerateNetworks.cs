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
      /// <summary>[AlphaFS] Returns an enumerable collection of networks available on the local host.</summary>
      /// <returns>An <see cref="IEnumerable{NetworkInfo}"/> collection of connected and disconnected networks on the local host.</returns>
      [SecurityCritical]
      public static IEnumerable<NetworkInfo> EnumerateNetworks()
      {
         return EnumerateNetworksCore(null, NetworkConnectivityLevels.All);
      }


      /// <summary>[AlphaFS] Returns an enumerable collection of networks available on the local host.</summary>
      /// <returns>An <see cref="IEnumerable{NetworkInfo}"/> collection of networks on the local host, as specified by <paramref name="networkConnectivityLevels"/>.</returns>
      /// <param name="networkConnectivityLevels">The <see cref="NetworkConnectivityLevels"/> that specify the connectivity level of the returned <see cref="NetworkInfo"/> instances.</param>
      [SecurityCritical]
      public static IEnumerable<NetworkInfo> EnumerateNetworks(NetworkConnectivityLevels networkConnectivityLevels)
      {
         return EnumerateNetworksCore(null, networkConnectivityLevels);
      }




      /// <summary>[AlphaFS] Returns an enumerable collection of networks available on the local host.</summary>
      /// <returns>An <see cref="IEnumerable{NetworkInfo}"/> collection of networks on the local host, as specified by <paramref name="networkConnectivityLevels"/>.</returns>
      /// <param name="networkID">The <see cref="Guid"/> that defines a network.</param>
      /// <param name="networkConnectivityLevels">The <see cref="NetworkConnectivityLevels"/> that specify the connectivity level of the returned <see cref="NetworkInfo"/> instances.</param>
      [SecurityCritical]
      internal static IEnumerable<NetworkInfo> EnumerateNetworksCore(Guid? networkID, NetworkConnectivityLevels networkConnectivityLevels)
      {
         if (null != networkID)
            yield return new NetworkInfo(Manager.GetNetwork((Guid) networkID));

         else
         {
            foreach (INetwork network in Manager.GetNetworks(networkConnectivityLevels))

               yield return new NetworkInfo(network);
         }
      }
   }
}
