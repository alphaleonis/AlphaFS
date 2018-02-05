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

using Alphaleonis.Win32.Filesystem;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Security;

namespace Alphaleonis.Win32.Network
{
   partial class Host
   {
      /// <summary>Enumerates drives from the local host.</summary>
      /// <returns><see cref="IEnumerable{String}"/> drives from the local host.</returns>
      /// <exception cref="NetworkInformationException"/>
      [SecurityCritical]
      public static IEnumerable<Network> EnumerateNetworks()
      {
         return EnumerateNetworksCore(NetworkConnectivityLevels.All);
      }


      /// <summary>Enumerates drives from the local host.</summary>
      /// <returns><see cref="IEnumerable{String}"/> drives from the local host.</returns>
      /// <exception cref="NetworkInformationException"/>
      [SecurityCritical]
      public static IEnumerable<Network> EnumerateNetworks(NetworkConnectivityLevels networkConnectivityLevels)
      {
         return EnumerateNetworksCore(networkConnectivityLevels);
      }


      /// <summary>Enumerates local drives from the specified host.</summary>
      /// <returns><see cref="IEnumerable{String}"/> drives from the specified host.</returns>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="NetworkInformationException"/>
      /// <param name="host">The DNS or NetBIOS name of the remote server. <see langword="null"/> refers to the local host.</param>
      /// <param name="continueOnException">
      ///   <para><see langword="true"/> suppress any Exception that might be thrown as a result from a failure,</para>
      ///   <para>such as unavailable resources.</para>
      /// </param>
      [SecurityCritical]
      private static IEnumerable<Network> EnumerateNetworksCore(NetworkConnectivityLevels networkConnectivityLevels)
      {
         foreach (var network in NetworkListManager.GetNetworks(networkConnectivityLevels))

            yield return network;
      }
   }
}
