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
using System.Diagnostics.CodeAnalysis;

namespace Alphaleonis.Win32.Network
{
   /// <summary>Specifies types of network connectivity state.</summary>    
   [Flags]
   public enum ConnectivityStates
   {
      /// <summary>The underlying network interfaces have no connectivity to any network.</summary>
      None = 0,

      /// <summary>There is connectivity to a network, but the service cannot detect any IPv4 Network Traffic.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Pv")]
      IPv4NoTraffic = 1,

      /// <summary>There is connectivity to a network, but the service cannot detect any IPv6 Network Traffic.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Pv")]
      IPv6NoTraffic = 2,

      /// <summary>There is connectivity to the local subnet using the IPv4 protocol.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Pv")]
      IPv4Subnet = 16,

      /// <summary>There is connectivity to a routed network using the IPv4 protocol.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Pv")]
      IPv4LocalNetwork = 32,

      /// <summary>There is connectivity to the Internet using the IPv4 protocol.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Pv")]
      IPv4Internet = 64,

      /// <summary>There is connectivity to the local subnet using the IPv6 protocol.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Pv")]
      IPv6Subnet = 256,

      /// <summary>There is connectivity to a local network using the IPv6 protocol.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Pv")]
      IPv6LocalNetwork = 512,

      /// <summary>There is connectivity to the Internet using the IPv6 protocol.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Pv")]
      IPv6Internet = 1024
   }
}
