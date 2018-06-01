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
   /// <summary>The NetworkListManager class provides a set of methods to perform network list management functions.</summary>
   [ComImport, ClassInterface((short) 0), Guid("DCB00C01-570F-4A9B-8D69-199FDBA5723B")]
   [ComSourceInterfaces("Microsoft.Windows.NetworkList.Internal.INetworkEvents\0Microsoft.Windows.NetworkList.Internal.INetworkConnectionEvents\0Microsoft.Windows.NetworkList.Internal.INetworkListManagerEvents\0"), TypeLibType(2)]
   internal sealed class NetworkListManager : INetworkListManager
   {
      /// <summary>The GetConnectivity method returns the overall connectivity state of the machine.</summary>
      /// <returns>Returns S_OK if the method succeeds.</returns>
      /// <remarks>Minimum supported client: Windows Vista [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2008 [desktop apps only]</remarks>
      [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(7)]
      public extern ConnectivityStates GetConnectivity();


      /// <summary>The GetNetwork method retrieves a network based on a supplied network ID.</summary>
      /// <returns>Returns S_OK if the method succeeds. Otherwise, the method returns one of the following values:
      /// E_POINTER: The pointer passed is NULL.
      /// E_UNEXPECTED: The specified GUID is invalid.
      /// </returns>
      /// <remarks>Minimum supported client: Windows Vista [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2008 [desktop apps only]</remarks>
      [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(2)]
      [return: MarshalAs(UnmanagedType.Interface)]
      public extern INetwork GetNetwork([In] Guid gdNetworkId);


      /// <summary>The GetNetworkConnection method retrieves a network based on a supplied Network Connection ID.</summary>
      /// <returns>Returns S_OK if the method succeeds. Otherwise, the method returns one of the following values:
      /// S_FALSE: The network associated with the specified network connection ID was not found.
      /// E_POINTER: The pointer passed is NULL.
      /// E_UNEXPECTED: The specified GUID is invalid.
      /// </returns>
      /// <remarks>Minimum supported client: Windows Vista [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2008 [desktop apps only]</remarks>
      [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(4)]
      [return: MarshalAs(UnmanagedType.Interface)]
      public extern INetworkConnection GetNetworkConnection([In] Guid gdNetworkConnectionId);


      /// <summary>The GetNetworkConnections method enumerates a complete list of the network connections that have been made.</summary>
      /// <returns>Returns S_OK if the method succeeds. Otherwise, the method returns one of the following values: E_POINTER: The pointer passed is NULL.</returns>
      /// <remarks>Minimum supported client: Windows Vista [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2008 [desktop apps only]</remarks>
      [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(3)]
      [return: MarshalAs(UnmanagedType.Interface)]
      public extern IEnumerable GetNetworkConnections();


      /// <summary>The GetNetworks method retrieves the list of networks available on the local machine.</summary>
      /// <returns>Returns S_OK if the method succeeds. Otherwise, the method returns one of the following values:
      /// E_POINTER: The pointer passed is NULL.
      /// E_UNEXPECTED: The specified GUID is invalid.
      /// </returns>
      /// <remarks>Minimum supported client: Windows Vista [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2008 [desktop apps only]</remarks>
      [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(1)]
      [return: MarshalAs(UnmanagedType.Interface)]
      public extern IEnumerable GetNetworks([In] NetworkConnectivityLevels flags);


      /// <summary>The IsConnected property specifies if the local machine has network connectivity.</summary>
      /// <returns>
      /// If TRUE , the network has at least local connectivity via IPv4 or IPv6 or both. The network may also have Internet connectivity. Thus, the network is connected.
      /// If FALSE, the network does not have local or Internet connectivity. The network is not connected.
      /// </returns>
      /// <remarks>Minimum supported client: Windows Vista [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2008 [desktop apps only]</remarks>
      [DispId(6)]
      public extern bool IsConnected
      {
         [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(6)]
         get;
      }


      /// <summary>The IsConnectedToInternet property specifies if the local machine has internet connectivity.</summary>
      /// <returns>If TRUE, the local machine is connected to the internet; if FALSE, it is not.</returns>
      /// <remarks>Minimum supported client: Windows Vista [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2008 [desktop apps only]</remarks>
      [DispId(5)]
      public extern bool IsConnectedToInternet
      {
         [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(5)]
         get;
      }
   }
}
