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
using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Alphaleonis.Win32.Network
{
   /// <summary>The INetwork interface represents a network on the local machine. It can also represent a collection of network connections with a similar network signature.</summary>
   [ComImport, TypeLibType(0x1040), Guid("DCB00002-570F-4A9B-8D69-199FDBA5723B")]
   internal interface INetwork
   {
      // Do not change the order of these interface members.


      /// <summary>Returns the name of the network.</summary>
      /// <returns>Returns S_OK if the method succeeds. Otherwise, the method returns one of the following values: E_POINTER: The pointer passed is NULL.</returns>
      /// <remarks>Minimum supported client: Windows Vista [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2008 [desktop apps only]</remarks>
      [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
      [return: MarshalAs(UnmanagedType.BStr)]
      string GetName();


      /// <summary>Sets or renames the network. This change occurs immediately.</summary>
      /// <returns>Returns S_OK if the method succeeds. Otherwise, the method returns one of the following values:
      /// E_POINTER: The pointer passed is NULL.
      /// HRESULT_FROM_WIN32(ERROR_FILENAME_EXCED_RANGE): The given name is too long.
      /// </returns>
      /// <remarks>Minimum supported client: Windows Vista [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2008 [desktop apps only]</remarks>
      [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
      void SetName([In, MarshalAs(UnmanagedType.BStr)] string szNetworkNewName);


      /// <summary>Returns a description string for the network.</summary>
      /// <returns>Returns S_OK if the method succeeds. Otherwise, the method returns one of the following values: E_POINTER: The pointer passed is NULL.</returns>
      /// <remarks>Minimum supported client: Windows Vista [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2008 [desktop apps only]</remarks>
      [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
      [return: MarshalAs(UnmanagedType.BStr)]
      string GetDescription();


      /// <summary>Sets a new description for the network.</summary>
      /// <returns>Returns S_OK if the method succeeds. Otherwise, the method returns one of the following values:
      /// E_POINTER: The pointer passed is NULL.
      /// HRESULT_FROM_WIN32(ERROR_FILENAME_EXCED_RANGE): The given name is too long.
      /// </returns>
      /// <remarks>Minimum supported client: Windows Vista [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2008 [desktop apps only]</remarks>
      [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
      void SetDescription([In, MarshalAs(UnmanagedType.BStr)] string szDescription);


      /// <summary>Returns the unique identifier of a network.</summary>
      /// <returns>Returns S_OK if the method succeeds.</returns>
      /// <remarks>Minimum supported client: Windows Vista [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2008 [desktop apps only]</remarks>
      [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
      Guid GetNetworkId();


      /// <summary>Returns the type of network.</summary>
      /// <returns>Returns S_OK if the method succeeds.</returns>
      /// <remarks>Minimum supported client: Windows Vista [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2008 [desktop apps only]</remarks>
      [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
      DomainType GetDomainType();


      /// <summary>Returns an enumeration of all network connections for a network.</summary>
      /// <returns>Returns S_OK if the method succeeds.</returns>
      /// <remarks>Minimum supported client: Windows Vista [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2008 [desktop apps only]</remarks>
      [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
      [return: MarshalAs(UnmanagedType.Interface)]
      IEnumerable GetNetworkConnections();


      /// <summary>Returns the local date and time when the network was created and connected.</summary>
      /// <returns>Returns S_OK if the method succeeds. Otherwise, the method returns one of the following values: E_POINTER: The pointer passed is NULL.</returns>
      /// <remarks>Minimum supported client: Windows Vista [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2008 [desktop apps only]</remarks>
      [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
      void GetTimeCreatedAndConnected(out uint pdwLowDateTimeCreated, out uint pdwHighDateTimeCreated, out uint pdwLowDateTimeConnected, out uint pdwHighDateTimeConnected);


      /// <summary>Specifies if the network has any network connectivity.</summary>
      /// <returns>If TRUE, this network is connected; if FALSE, it is not.</returns>
      /// <remarks>Minimum supported client: Windows Vista [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2008 [desktop apps only]</remarks>
      bool IsConnected
      {
         [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
         get;
      }


      /// <summary>Specifies if the network has Internet connectivity.</summary>
      /// <returns>If TRUE, this network has connectivity to the internet; if FALSE, it does not.</returns>
      /// <remarks>Minimum supported client: Windows Vista [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2008 [desktop apps only]</remarks>
      bool IsConnectedToInternet
      {
         [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
         get;
      }


      /// <summary>Returns the connectivity state of the network.</summary>
      /// <returns>Returns S_OK if the method succeeds.</returns>
      /// <remarks>Minimum supported client: Windows Vista [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2008 [desktop apps only]</remarks>
      [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
      ConnectivityStates GetConnectivity();


      /// <summary>Returns the category of a network.</summary>
      /// <returns>Returns S_OK if the method succeeds.</returns>
      /// <remarks>Minimum supported client: Windows Vista [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2008 [desktop apps only]</remarks>
      [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
      NetworkCategory GetCategory();


      /// <summary>Sets the category of a network. Administrative privileges are needed for this API call.</summary>
      /// <returns>Returns S_OK if the method succeeds.</returns>
      /// <remarks>Minimum supported client: Windows Vista [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2008 [desktop apps only]</remarks>
      [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
      void SetCategory([In] NetworkCategory NewCategory);
   }
}
