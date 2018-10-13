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

using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Security;

namespace Alphaleonis.Win32.Network
{
   internal static partial class NativeMethods
   {
      /// <summary>Enumerates the Distributed File System (DFS) namespaces hosted on a server or DFS links of a namespace hosted by a server.</summary>
      /// <returns>
      /// If the function succeeds, the return value is NERR_Success.
      /// If the function fails, the return value is a system error code.
      /// </returns>
      /// <remarks>
      /// <para>No special group membership is required for using the NetDfsEnum function.</para>
      /// <para>Minimum supported client: Windows Vista</para>
      /// <para>Minimum supported server: Windows Server 2003</para>
      /// </remarks>
      [SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage"), SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("netapi32.dll", SetLastError = true, CharSet = CharSet.Unicode), SuppressUnmanagedCodeSecurity]
      [return: MarshalAs(UnmanagedType.U4)]
      internal static extern uint NetDfsEnum([MarshalAs(UnmanagedType.LPWStr)] string dfsName, [MarshalAs(UnmanagedType.U4)] uint level, [MarshalAs(UnmanagedType.U4)] int prefMaxLen, out SafeGlobalMemoryBufferHandle buffer, [MarshalAs(UnmanagedType.U4)] out uint entriesRead, [MarshalAs(UnmanagedType.U4)] out uint resumeHandle);

      
      /// <summary>Retrieves information about a Distributed File System (DFS) root or link from the cache maintained by the DFS client.</summary>
      /// <returns>
      /// If the function succeeds, the return value is NERR_Success.
      /// If the function fails, the return value is a system error code.
      /// </returns>
      /// <remarks>
      /// <para>No special group membership is required for using the NetDfsGetClientInfo function.</para>
      /// <para>Minimum supported client: Windows Vista</para>
      /// <para>Minimum supported server: Windows Server 2003</para>
      /// </remarks>
      [SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage"), SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("netapi32.dll", SetLastError = true, CharSet = CharSet.Unicode), SuppressUnmanagedCodeSecurity]
      [return: MarshalAs(UnmanagedType.U4)]
      internal static extern uint NetDfsGetClientInfo([MarshalAs(UnmanagedType.LPWStr)] string dfsEntryPath, [MarshalAs(UnmanagedType.LPWStr)] string serverName, [MarshalAs(UnmanagedType.LPWStr)] string shareName, [MarshalAs(UnmanagedType.U4)] uint level, out SafeGlobalMemoryBufferHandle buffer);

      
      /// <summary>Retrieves information about a specified Distributed File System (DFS) root or link in a DFS namespace.</summary>
      /// <returns>
      /// If the function succeeds, the return value is NERR_Success.
      /// If the function fails, the return value is a system error code.
      /// </returns>
      /// <remarks>
      /// <para>No special group membership is required for using the NetDfsGetInfo function.</para>
      /// <para>Minimum supported client: Windows Vista</para>
      /// <para>Minimum supported server: Windows Server 2003</para>
      /// </remarks>
      [SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage"), SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("netapi32.dll", SetLastError = true, CharSet = CharSet.Unicode), SuppressUnmanagedCodeSecurity]
      [return: MarshalAs(UnmanagedType.U4)]
      internal static extern uint NetDfsGetInfo([MarshalAs(UnmanagedType.LPWStr)] string dfsEntryPath, [MarshalAs(UnmanagedType.LPWStr)] string serverName, [MarshalAs(UnmanagedType.LPWStr)] string shareName, [MarshalAs(UnmanagedType.U4)] uint level, out SafeGlobalMemoryBufferHandle buffer);
   }
}
