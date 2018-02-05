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
   /// <summary>Represents a connection to a network.</summary>
   /// <remarks> A collection containing instances of this class is obtained by calling
   /// the <see cref="P:Microsoft.WindowsAPICodePack.Net.Network.Connections"/> property.</remarks>
   public class NetworkConnection
   {
      private readonly INetworkConnection _networkConnection;


      internal NetworkConnection(INetworkConnection networkConnection)
      {
         _networkConnection = networkConnection;
      }


      /// <summary>Retrieves an object that represents the network associated with this connection.</summary>
      /// <returns>A <see cref="Network"/> object.</returns>
      public Network Network
      {
         get { return new Network(_networkConnection.GetNetwork()); }
      }

      
      /// <summary>Gets the adapter identifier for this connection.</summary>
      /// <value>A <see cref="System.Guid"/> object.</value>
      public Guid AdapterId
      {
         get { return _networkConnection.GetAdapterId(); }
      }

      
      /// <summary>Gets the unique identifier for this connection.</summary>
      /// <value>A <see cref="System.Guid"/> object.</value>
      public Guid ConnectionId
      {
         get { return _networkConnection.GetConnectionId(); }
      }

      
      /// <summary>Gets a value that indicates the connectivity of this connection.</summary>
      /// <value>A <see cref="Connectivity"/> value.</value>
      public ConnectivityStates Connectivity
      {
         get { return _networkConnection.GetConnectivity(); }
      }

      
      /// <summary>Gets a value that indicates whether the network associated with this connection is an Active Directory network and whether the machine has been authenticated by Active Directory.</summary>
      /// <value>A <see cref="DomainType"/> value.</value>
      public DomainType DomainType
      {
         get { return _networkConnection.GetDomainType(); }
      }

      
      /// <summary>Gets a value that indicates whether this connection has Internet access.</summary>
      /// <value>A <see cref="System.Boolean"/> value.</value>
      public bool IsConnectedToInternet
      {
         get { return _networkConnection.IsConnectedToInternet; }
      }

      
      /// <summary>Gets a value that indicates whether this connection has network connectivity.</summary>
      /// <value>A <see cref="System.Boolean"/> value.</value>
      public bool IsConnected
      {
         get { return _networkConnection.IsConnected; }
      }
   }
}
