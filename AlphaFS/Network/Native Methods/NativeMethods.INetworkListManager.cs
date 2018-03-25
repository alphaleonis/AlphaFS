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
using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Alphaleonis.Win32.Network
{
   /// <summary>The INetworkListManager interface provides a set of methods to perform network list management functions.</summary>
   [ComImport, Guid("DCB00000-570F-4A9B-8D69-199FDBA5723B"), TypeLibType(0x1040)]
   internal interface INetworkListManager
   {
      // Do not change the order of these interface members.


      /// <summary>Retrieves networks based on the supplied Network IDs.</summary>
      /// <returns>Returns S_OK if the method succeeds. Otherwise, the method returns one of the following values:
      /// E_POINTER: The pointer passed is NULL.
      /// E_UNEXPECTED: The specified GUID is invalid.
      /// </returns>
      /// <remarks>Minimum supported client: Windows Vista [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2008 [desktop apps only]</remarks>
      [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
      [return: MarshalAs(UnmanagedType.Interface)]
      IEnumerable GetNetworks([In] NetworkConnectivityLevels flags);


      /// <summary>Retrieves a network based on a supplied Network ID.</summary>
      /// <returns>Returns S_OK if the method succeeds. Otherwise, the method returns one of the following values:
      /// E_POINTER: The pointer passed is NULL.
      /// E_UNEXPECTED: The specified GUID is invalid.
      /// </returns>
      /// <remarks>Minimum supported client: Windows Vista [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2008 [desktop apps only]</remarks>
      [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
      [return: MarshalAs(UnmanagedType.Interface)]
      INetwork GetNetwork([In] Guid gdNetworkId);


      /// <summary>Gets an enumerator that contains a complete list of the network connections that have been made.</summary>
      /// <returns>Returns S_OK if the method succeeds. Otherwise, the method returns one of the following values: E_POINTER: The pointer passed is NULL.</returns>
      /// <remarks>Minimum supported client: Windows Vista [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2008 [desktop apps only]</remarks>
      [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
      [return: MarshalAs(UnmanagedType.Interface)]
      IEnumerable GetNetworkConnections();


      /// <summary>Retrieves a network based on a supplied Network Connection ID.</summary>
      /// <returns>Returns S_OK if the method succeeds. Otherwise, the method returns one of the following values:
      /// S_FALSE: The network associated with the specified network connection ID was not found.
      /// E_POINTER: The pointer passed is NULL.
      /// E_UNEXPECTED: The specified GUID is invalid.
      /// </returns>
      /// <remarks>Minimum supported client: Windows Vista [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2008 [desktop apps only]</remarks>
      [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
      [return: MarshalAs(UnmanagedType.Interface)]
      INetworkConnection GetNetworkConnection([In] Guid gdNetworkConnectionId);
      
      
      /// <summary>Specifies if the machine has network connectivity.</summary>
      /// <returns>
      /// If TRUE , the network has at least local connectivity via IPv4 or IPv6 or both. The network may also have Internet connectivity. Thus, the network is connected.
      /// If FALSE, the network does not have local or Internet connectivity. The network is not connected.
      /// </returns>
      /// <remarks>Minimum supported client: Windows Vista [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2008 [desktop apps only]</remarks>
      bool IsConnected
      {
         [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
         get;
      }


      /// <summary>Specifies if the machine has Internet connectivity.</summary>
      /// <returns>If TRUE, the local machine is connected to the internet; if FALSE, it is not.</returns>
      /// <remarks>Minimum supported client: Windows Vista [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2008 [desktop apps only]</remarks>
      bool IsConnectedToInternet
      {
         [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
         get;
      }


      /// <summary>Returns the connectivity state of all the networks on a machine.</summary>
      /// <returns>Returns S_OK if the method succeeds.</returns>
      /// <remarks>Minimum supported client: Windows Vista [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2008 [desktop apps only]</remarks>
      [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
      ConnectivityStates GetConnectivity();
   }
}
