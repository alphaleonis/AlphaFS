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
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace Alphaleonis.Win32.Network
{
   internal partial class NativeMethods
   {
      /// <summary>The WNetCancelConnection function cancels an existing network connection. You can also call the function to remove remembered network connections that are not currently connected.</summary>
      /// <returns>
      /// If the function succeeds, the return value is <see cref="Win32Errors.NO_ERROR"/>
      /// If the function fails, the return value is a system error code.
      /// </returns>
      /// <remarks>
      /// <para>Minimum supported client: Windows 2000 Professional [desktop apps only]</para>
      /// <para>Minimum supported server: Windows 2000 Server [desktop apps only]</para>
      /// </remarks>
      [SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage"), SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("mpr.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "WNetCancelConnection2W"), SuppressUnmanagedCodeSecurity]
      [return: MarshalAs(UnmanagedType.U4)]
      internal static extern uint WNetCancelConnection([MarshalAs(UnmanagedType.LPWStr)] string lpName, Connect dwFlags, [MarshalAs(UnmanagedType.Bool)] bool fForce);


      /// <summary>The WNetGetUniversalName function takes a drive-based path for a network resource and returns an information structure that contains a more universal form of the name.</summary>
      /// <returns>
      /// If the function succeeds, the return value is <see cref="Win32Errors.NO_ERROR"/>
      /// If the function fails, the return value is a system error code.
      /// </returns>
      /// <remarks>
      /// <para>Minimum supported client: Windows 2000 Professional [desktop apps only]</para>
      /// <para>Minimum supported server: Windows 2000 Server [desktop apps only]</para>
      /// </remarks>
      [SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage"), SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("mpr.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "WNetGetUniversalNameW"), SuppressUnmanagedCodeSecurity]
      [return: MarshalAs(UnmanagedType.U4)]
      internal static extern uint WNetGetUniversalName([MarshalAs(UnmanagedType.LPWStr)] string lpLocalPath, [MarshalAs(UnmanagedType.U4)] uint dwInfoLevel, SafeGlobalMemoryBufferHandle lpBuffer, [MarshalAs(UnmanagedType.U4)] out uint lpBufferSize);


      /// <summary>The WNetUseConnection function creates a connection to a network resource. The function can redirect a local device to a network resource.</summary>
      /// <returns>
      /// If the function succeeds, the return value is <see cref="Win32Errors.NO_ERROR"/>
      /// If the function fails, the return value is a system error code.
      /// </returns>
      /// <remarks>
      /// <para>Minimum supported client: Windows 2000 Professional [desktop apps only]</para>
      /// <para>Minimum supported server: Windows 2000 Server [desktop apps only]</para>
      /// </remarks>
      [SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage"), SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("mpr.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "WNetUseConnectionW"), SuppressUnmanagedCodeSecurity]
      [return: MarshalAs(UnmanagedType.U4)]
      internal static extern uint WNetUseConnection(IntPtr hwndOwner, [MarshalAs(UnmanagedType.Struct)] ref NETRESOURCE lpNetResource, [MarshalAs(UnmanagedType.LPWStr)] string lpPassword, [MarshalAs(UnmanagedType.LPWStr)] string lpUserId, [MarshalAs(UnmanagedType.U4)] Connect dwFlags, StringBuilder lpAccessName, [MarshalAs(UnmanagedType.U4)] out uint lpBufferSize, [MarshalAs(UnmanagedType.U4)] out uint lpResult);

      // Note: When NETRESOURCE is struct: use ref, when NETRESOURCE is class: ommit ref.
   }
}
