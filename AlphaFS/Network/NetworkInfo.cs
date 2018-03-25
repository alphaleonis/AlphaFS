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
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Net.NetworkInformation;

namespace Alphaleonis.Win32.Network
{
   /// <summary>Represents a network on the local machine. It can also represent a collection of network connections with a similar network signature.</summary>
   public class NetworkInfo
   {
      #region Private Fields

      private readonly INetwork _network;

      #endregion // Private Fields


      #region Constructors

      internal NetworkInfo(INetwork network)
      {
         _network = network;
      }

      #endregion // Constructors


      #region Properties

      /// <summary>Gets the category of a network. The categories are trusted, untrusted, or authenticated. This value of this property is not cached.</summary>
      public NetworkCategory Category
      {
         get { return _network.GetCategory(); }

         // Should we allow this in AlphaFS?
         // set { _network.SetCategory(value); }
      }


      /// <summary>Gets the network connections for the network. This value of this property is not cached.</summary>
      public IEnumerable<NetworkConnectionInfo> Connections
      {
         get
         {
            foreach (var connection in _network.GetNetworkConnections())

               yield return new NetworkConnectionInfo((INetworkConnection) connection);
         }
      }


      /// <summary>Gets the local date and time when the network was connected. This value of this property is not cached.</summary>
      public DateTime ConnectionTime
      {
         get { return ConnectionTimeUtc.ToLocalTime(); }
      }


      /// <summary>Gets the date and time when the network was connected. This value of this property is not cached.</summary>
      public DateTime ConnectionTimeUtc
      {
         get
         {
            uint low, high, unused1, unused2;

            _network.GetTimeCreatedAndConnected(out unused1, out unused2, out low, out high);
            
            long time = high;

            // Shift the day info into the high order bits.
            time <<= 32;
            time |= low;

            return DateTime.FromFileTimeUtc(time);
         }
      }


      /// <summary>Gets the connectivity state of the network. This value of this property is not cached.</summary>
      /// <remarks>Connectivity provides information on whether the network is connected, and the protocols in use for network traffic.</remarks>
      public ConnectivityStates Connectivity
      {
         get { return _network.GetConnectivity(); }
      }


      /// <summary>Gets the local date and time when the network was created. This value of this property is not cached.</summary>
      public DateTime CreationTime
      {
         get { return CreationTimeUtc.ToLocalTime(); }
      }


      /// <summary>Gets the date and time when the network was created. This value of this property is not cached.</summary>
      public DateTime CreationTimeUtc
      {
         get
         {
            uint low, high, unused1, unused2;

            _network.GetTimeCreatedAndConnected(out low, out high, out unused1, out unused2);

            long time = high;

            // Shift the value into the high order bits.
            time <<= 32;
            time |= low;

            return DateTime.FromFileTimeUtc(time);
         }
      }


      /// <summary>Gets a description for the network. This value of this property is not cached.</summary>
      public string Description
      {
         get { return _network.GetDescription(); }

         // Should we allow this in AlphaFS?
         //private set { _network.SetDescription(value); }
      }


      /// <summary>Gets the domain type of the network. This value of this property is not cached.</summary>
      /// <remarks>The domain indictates whether the network is an Active Directory Network, and whether the machine has been authenticated by Active Directory.</remarks>
      public DomainType DomainType
      {
         get { return _network.GetDomainType(); }
      }


      /// <summary>Gets a value that indicates whether there is network connectivity. This value of this property is not cached.</summary>
      public bool IsConnected
      {
         get { return _network.IsConnected; }
      }


      /// <summary>Gets a value that indicates whether there is Internet connectivity. This value of this property is not cached.</summary>
      public bool IsConnectedToInternet
      {
         get { return _network.IsConnectedToInternet; }
      }


      /// <summary>Gets the name of the network. This value of this property is not cached.</summary>
      public string Name
      {
         get { return _network.GetName(); }

         // Should we allow this in AlphaFS?
         //private set { _network.SetName(value); }
      }


      /// <summary>Gets a unique identifier for the network. This value of this property is not cached.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID")]
      public Guid NetworkId
      {
         get { return _network.GetNetworkId(); }
      }

      #endregion // Properties


      #region Methods

      /// <summary>Returns storage device as: "VendorId ProductId DeviceType DeviceNumber:PartitionNumber".</summary>
      /// <returns>A string that represents this instance.</returns>
      public override string ToString()
      {
         var description = !Utils.IsNullOrWhiteSpace(Description) && !Equals(Name, Description) ? " (" + Description + ")" : string.Empty;

         return null != Name ? string.Format(CultureInfo.CurrentCulture, "{0}{1}, {2}", Name, description, Category) : GetType().Name;
      }


      /// <summary>Determines whether the specified Object is equal to the current Object.</summary>
      /// <param name="obj">Another object to compare to.</param>
      /// <returns><see langword="true"/> if the specified Object is equal to the current Object; otherwise, <see langword="false"/>.</returns>
      public override bool Equals(object obj)
      {
         if (null == obj || GetType() != obj.GetType())
            return false;

         var other = obj as NetworkInfo;

         return null != other &&
                Equals(NetworkId, other.NetworkId) &&
                Equals(Category, other.Category);
      }


      /// <summary>Serves as a hash function for a particular type.</summary>
      /// <returns>A hash code for the current Object.</returns>
      public override int GetHashCode()
      {
         unchecked
         {
            return NetworkId.GetHashCode() + Category.GetHashCode();
         }
      }


      /// <summary>Implements the operator ==</summary>
      /// <param name="left">A.</param>
      /// <param name="right">B.</param>
      /// <returns>The result of the operator.</returns>
      public static bool operator ==(NetworkInfo left, NetworkInfo right)
      {
         return ReferenceEquals(left, null) && ReferenceEquals(right, null) || !ReferenceEquals(left, null) && !ReferenceEquals(right, null) && left.Equals(right);
      }


      /// <summary>Implements the operator !=</summary>
      /// <param name="left">A.</param>
      /// <param name="right">B.</param>
      /// <returns>The result of the operator.</returns>
      public static bool operator !=(NetworkInfo left, NetworkInfo right)
      {
         return !(left == right);
      }

      #endregion // Methods
   }
}
