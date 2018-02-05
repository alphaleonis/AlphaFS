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

namespace Alphaleonis.Win32.Network
{
   /// <summary>Provides access to objects that represent networks and network connections.</summary>
   public static class NetworkListManager
   {
      private static readonly NetworkListManagerClass Manager = new NetworkListManagerClass();


      /// <summary>Retrieves a collection of <see cref="Network"/> objects that represent the networks defined for this machine.</summary>
      /// <param name="level">The <see cref="NetworkConnectivityLevels"/> that specify the connectivity level of the returned <see cref="Network"/> objects.</param>
      /// <returns>A <see cref="NetworkCollection"/> of <see cref="Network"/> objects.</returns>
      public static NetworkCollection GetNetworks(NetworkConnectivityLevels level)
      {
         return new NetworkCollection(Manager.GetNetworks(level));
      }


      /// <summary>Retrieves the <see cref="Network"/> identified by the specified network identifier.</summary>
      /// <param name="networkId">A <see cref="System.Guid"/> that specifies the unique identifier for the network.</param>
      /// <returns>The <see cref="Network"/> that represents the network identified by the identifier.</returns>
      public static Network GetNetwork(Guid networkId)
      {
         return new Network(Manager.GetNetwork(networkId));
      }


      /// <summary>Retrieves a collection of <see cref="NetworkConnection"/> objects that represent the connections for this machine.</summary>
      /// <returns>A <see cref="NetworkConnectionCollection"/> containing the network connections.</returns>
      public static NetworkConnectionCollection GetNetworkConnections()
      {
         return new NetworkConnectionCollection(Manager.GetNetworkConnections());
      }


      /// <summary>Retrieves the <see cref="NetworkConnection"/> identified by the specified connection identifier.</summary>
      /// <param name="networkConnectionId">A <see cref="System.Guid"/> that specifies the unique identifier for the network connection.</param>
      /// <returns>The <see cref="NetworkConnection"/> identified by the specified identifier.</returns>
      public static NetworkConnection GetNetworkConnection(Guid networkConnectionId)
      {
         return new NetworkConnection(Manager.GetNetworkConnection(networkConnectionId));
      }


      /// <summary>Gets a value that indicates whether this machine has Internet connectivity.</summary>
      /// <value>A <see cref="System.Boolean"/> value.</value>
      public static bool IsConnectedToInternet
      {
         get { return Manager.IsConnectedToInternet; }
      }


      /// <summary>Gets a value that indicates whether this machine has network connectivity.</summary>
      /// <value>A <see cref="System.Boolean"/> value.</value>
      public static bool IsConnected
      {
         get { return Manager.IsConnected; }
      }


      /// <summary>Gets the connectivity state of this machine.</summary>
      /// <value>A <see cref="Connectivity"/> value.</value>
      public static ConnectivityStates Connectivity
      {
         get { return Manager.GetConnectivity(); }
      }
   }
}