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
using System.Diagnostics.CodeAnalysis;
using System.Net.NetworkInformation;

namespace Alphaleonis.Win32.Network
{
   /// <summary>Represents a connection to a network.</summary>
   public class NetworkConnectionInfo
   {
      #region Private Fields

      private readonly INetworkConnection _networkConnection;

      #endregion // Private Fields


      #region Constructors

      internal NetworkConnectionInfo(INetworkConnection networkConnection)
      {
         _networkConnection = networkConnection;
      }

      #endregion // Constructors


      #region Properties

      /// <summary>Gets the unique identifier for this connection. This value of this property is not cached.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
      public Guid ConnectionId
      {
         get { return _networkConnection.GetConnectionId(); }
      }


      /// <summary>Gets a value that indicates the connectivity of this connection. This value of this property is not cached.</summary>
      public ConnectivityStates Connectivity
      {
         get { return _networkConnection.GetConnectivity(); }
      }


      /// <summary>Gets a value that indicates whether the network associated with this connection is an Active Directory network and whether the machine has been authenticated by Active Directory. This value of this property is not cached.</summary>
      public DomainType DomainType
      {
         get { return _networkConnection.GetDomainType(); }
      }


      /// <summary>Gets a value that indicates whether this connection has network connectivity. This value of this property is not cached.</summary>
      public bool IsConnected
      {
         get { return _networkConnection.IsConnected; }
      }


      /// <summary>Gets a value that indicates whether this connection has Internet access. This value of this property is not cached.</summary>
      public bool IsConnectedToInternet
      {
         get { return _networkConnection.IsConnectedToInternet; }
      }


      /// <summary>Retrieves an instance that represents the network associated with this connection. This value of this property is not cached.</summary>
      /// <returns>A <see cref="NetworkInfo"/> instance.</returns>
      public NetworkInfo NetworkInfo
      {
         get { return new NetworkInfo(_networkConnection.GetNetwork()); }
      }


      /// <summary>Gets the network interface for this connection. This value of this property is not cached.</summary>
      public NetworkInterface NetworkInterface
      {
         get
         {
            Guid guid;
            var adapterId = _networkConnection.GetAdapterId();
            
            
            foreach (var nic in NetworkInterface.GetAllNetworkInterfaces())
            {
#if NET35
               guid = new Guid(nic.Id);

#else
               if (!Guid.TryParse(nic.Id, out guid))
                  continue;
#endif

               if (adapterId.Equals(guid))
                  return nic;
            }


            return null;
         }
      }

      #endregion // Properties
   }
}
