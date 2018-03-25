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
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Alphaleonis.Win32.Network
{
   /// <summary>The INetworkConnection interface represents a single network connection.</summary>
   [ComImport, TypeLibType(0x1040), Guid("DCB00005-570F-4A9B-8D69-199FDBA5723B")]
   internal interface INetworkConnection
   {
      // Do not change the order of these interface members.


      /// <summary>The GetNetwork method returns the network associated with the connection.</summary>
      /// <returns>Returns S_OK if the method succeeds.</returns>
      /// <remarks>Minimum supported client: Windows Vista [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2008 [desktop apps only]</remarks>
      [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
      [return: MarshalAs(UnmanagedType.Interface)]
      INetwork GetNetwork();


      /// <summary>The IsConnected property specifies if the associated network connection has network connectivity.</summary>
      /// <returns>If TRUE, this network connection has connectivity; if FALSE, it does not.</returns>
      /// <remarks>Minimum supported client: Windows Vista [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2008 [desktop apps only]</remarks>
      bool IsConnected
      {
         [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
         get;
      }


      /// <summary>The IsConnectedToInternet property specifies if the associated network connection has internet connectivity.</summary>
      /// <returns>If TRUE, this network connection has connectivity to the Internet; if FALSE, it does not.</returns>
      /// <remarks>Minimum supported client: Windows Vista [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2008 [desktop apps only]</remarks>
      bool IsConnectedToInternet
      {
         [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
         get;
      }


      /// <summary>The GetConnectivity method returns the connectivity state of the network connection.</summary>
      /// <returns>Returns S_OK if the method succeeds.</returns>
      /// <remarks>Minimum supported client: Windows Vista [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2008 [desktop apps only]</remarks>
      [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
      ConnectivityStates GetConnectivity();


      /// <summary>The GetConnectionId method returns the Connection ID associated with this network connection.</summary>
      /// <returns>Returns S_OK if the method succeeds.</returns>
      /// <remarks>Minimum supported client: Windows Vista [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2008 [desktop apps only]</remarks>
      [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
      Guid GetConnectionId();


      /// <summary>The GetAdapterId method returns the ID of the network adapter used by this connection. </summary>
      /// <returns>Returns S_OK if the method succeeds.</returns>
      /// <remarks>Minimum supported client: Windows Vista [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2008 [desktop apps only]</remarks>
      [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
      Guid GetAdapterId();


      /// <summary>The GetDomainType method returns the domain type of the network connection.</summary>
      /// <returns>Returns S_OK if the method succeeds.</returns>
      /// <remarks>Minimum supported client: Windows Vista [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2008 [desktop apps only]</remarks>
      [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
      DomainType GetDomainType();
   }
}
