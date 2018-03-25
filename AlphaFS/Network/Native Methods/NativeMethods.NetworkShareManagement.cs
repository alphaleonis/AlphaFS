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
   internal partial class NativeMethods
   {
      /// <summary>Lists all connections made to a shared resource on the server or all connections established from a particular computer.</summary>
      /// <returns>
      /// If the function succeeds, the return value is NERR_Success.
      /// If the function fails, the return value is a system error code.
      /// </returns>
      /// <remarks>
      /// <para>If there is more than one user using this connection, then it is possible to get more than one structure for the same connection, but with a different user name.</para>
      /// <para>Administrator, Server or Print Operator, or Power User group membership is required to successfully execute the NetConnectionEnum function.</para>
      /// <para>Minimum supported client: Windows XP [desktop apps only]</para>
      /// <para>Minimum supported server: Windows Server 2003 [desktop apps only]</para>
      /// </remarks>
      [SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage"), SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("netapi32.dll", SetLastError = true, CharSet = CharSet.Unicode), SuppressUnmanagedCodeSecurity]
      [return: MarshalAs(UnmanagedType.U4)]
      internal static extern uint NetConnectionEnum([MarshalAs(UnmanagedType.LPWStr)] string serverName, [MarshalAs(UnmanagedType.LPWStr)] string qualifier, [MarshalAs(UnmanagedType.U4)] uint level, out SafeGlobalMemoryBufferHandle bufPtr, [MarshalAs(UnmanagedType.I4)] int prefMaxLen, [MarshalAs(UnmanagedType.U4)] out uint entriesRead, [MarshalAs(UnmanagedType.U4)] out uint totalEntries, [MarshalAs(UnmanagedType.U4)] out uint resumeHandle);


      /// <summary>Forces a resource to close. This function can be used when an error prevents closure by any other means.</summary>
      /// <returns>
      /// If the function succeeds, the return value is NERR_Success.
      /// If the function fails, the return value is a system error code.
      /// </returns>
      /// <remarks>
      /// <para>You should use NetFileClose with caution because it does not write data cached on the client system to the file before closing the file.</para>
      /// <para>Only members of the Administrators or Server Operators local group can successfully execute the NetFileEnum function.</para>
      /// <para>Minimum supported client: Windows XP [desktop apps only]</para>
      /// <para>Minimum supported server: Windows Server 2003 [desktop apps only]</para>
      /// </remarks>
      [SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage"), SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("netapi32.dll", SetLastError = true, CharSet = CharSet.Unicode), SuppressUnmanagedCodeSecurity]
      [return: MarshalAs(UnmanagedType.U4)]
      internal static extern uint NetFileClose([MarshalAs(UnmanagedType.LPWStr)] string serverName, [MarshalAs(UnmanagedType.U4)] uint fileid);


      /// <summary>Returns information about some or all open files on a server, depending on the parameters specified.</summary>
      /// <returns>
      /// If the function succeeds, the return value is NERR_Success.
      /// If the function fails, the return value is a system error code.
      /// </returns>
      /// <remarks>
      /// <para>Only members of the Administrators or Server Operators local group can successfully execute the NetFileEnum function.</para>
      /// <para>Minimum supported client: Windows XP [desktop apps only]</para>
      /// <para>Minimum supported server: Windows Server 2003 [desktop apps only]</para>
      /// </remarks>
      [SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage"), SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("netapi32.dll", SetLastError = true, CharSet = CharSet.Unicode), SuppressUnmanagedCodeSecurity]
      [return: MarshalAs(UnmanagedType.U4)]
      internal static extern uint NetFileEnum([MarshalAs(UnmanagedType.LPWStr)] string serverName, [MarshalAs(UnmanagedType.LPWStr)] string basepath, [MarshalAs(UnmanagedType.LPWStr)] string username, [MarshalAs(UnmanagedType.U4)] uint level, out SafeGlobalMemoryBufferHandle buffer, [MarshalAs(UnmanagedType.I4)] int prefmaxlen, [MarshalAs(UnmanagedType.U4)] out uint entriesRead, [MarshalAs(UnmanagedType.U4)] out uint totalentries, [MarshalAs(UnmanagedType.U4)] out uint resumeHandle);


      /// <summary>Provides information about sessions established on a server.</summary>
      /// <returns>
      /// If the function succeeds, the return value is NERR_Success.
      /// If the function fails, the return value is a system error code.
      /// </returns>
      /// <remarks>
      /// <para>Only members of the Administrators or Server Operators local group can successfully execute the NetSessionEnum function at level 1 or level 2.</para>
      /// <para>No special group membership is required for level 0 or level 10 calls.</para>
      /// <para>Minimum supported client: Windows XP [desktop apps only]</para>
      /// <para>Minimum supported server: Windows Server 2003 [desktop apps only]</para>
      /// </remarks>
      [SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage"), SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("netapi32.dll", SetLastError = true, CharSet = CharSet.Unicode), SuppressUnmanagedCodeSecurity]
      [return: MarshalAs(UnmanagedType.U4)]
      internal static extern uint NetSessionEnum([MarshalAs(UnmanagedType.LPWStr)] string serverName, [MarshalAs(UnmanagedType.LPWStr)] string uncClientName, [MarshalAs(UnmanagedType.LPWStr)] string userName, [MarshalAs(UnmanagedType.U4)] SessionInfoLevel level, out SafeGlobalMemoryBufferHandle bufPtr, [MarshalAs(UnmanagedType.I4)] int prefMaxLen, [MarshalAs(UnmanagedType.U4)] out uint entriesRead, [MarshalAs(UnmanagedType.U4)] out uint totalEntries, [MarshalAs(UnmanagedType.U4)] out uint resumeHandle);


      /// <summary>Retrieves information about each (hidden) Server Message Block (SMB) resource/share on a server.</summary>
      /// <returns>
      /// If the function succeeds, the return value is NERR_Success.
      /// If the function fails, the return value is a system error code.
      /// </returns>
      /// <remarks>
      /// <para>For interactive users (users who are logged on locally to the machine), no special group membership is required to execute the NetShareEnum function.</para>
      /// <para>For non-interactive users, Administrator, Power User, Print Operator, or Server Operator group membership is required</para>
      /// <para>to successfully execute the NetShareEnum function at levels 2, 502, and 503. No special group membership is required for level 0 or level 1 calls.</para>
      /// <para>This function applies only to Server Message Block (SMB) shares.</para>
      /// <para>Windows Server 2003 and Windows XP: For all users, Administrator, Power User, Print Operator, or Server Operator group membership is required to successfully execute the NetShareEnum function at levels 2 and 502.</para>
      /// <para>Minimum supported client: Windows XP [desktop apps only]</para>
      /// <para>Minimum supported server: Windows Server 2003 [desktop apps only]</para>
      /// </remarks>
      [SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage"), SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("netapi32.dll", SetLastError = true, CharSet = CharSet.Unicode), SuppressUnmanagedCodeSecurity]
      [return: MarshalAs(UnmanagedType.U4)]
      internal static extern uint NetShareEnum([MarshalAs(UnmanagedType.LPWStr)] string serverName, [MarshalAs(UnmanagedType.U4)] uint level, out SafeGlobalMemoryBufferHandle bufPtr, [MarshalAs(UnmanagedType.I4)] int prefMaxLen, [MarshalAs(UnmanagedType.U4)] out uint entriesRead, [MarshalAs(UnmanagedType.U4)] out uint totalEntries, [MarshalAs(UnmanagedType.U4)] out uint resumeHandle);


      /// <summary>Retrieves information about a particular Server Message Block (SMB) shared resource on a server.</summary>
      /// <returns>
      /// If the function succeeds, the return value is NERR_Success.
      /// If the function fails, the return value is a system error code.
      /// </returns>
      /// <remarks>
      /// <para>For interactive users (users who are logged on locally to the machine), no special group membership is required to execute the NetShareGetInfo function.</para>
      /// <para>For non-interactive users, Administrator, Power User, Print Operator, or Server Operator group membership is required to successfully execute the NetShareGetInfo function at levels 2, 502, and 503.</para>
      /// <para>This function applies only to Server Message Block (SMB) shares.</para>
      /// <para>Windows Server 2003 and Windows XP: For all users, Administrator, Power User, Print Operator, or Server Operator group membership is required to successfully execute the NetShareGetInfo function at levels 2 and 502.</para>
      /// <para>Minimum supported client: Windows XP [desktop apps only]</para>
      /// <para>Minimum supported server: Windows Server 2003 [desktop apps only]</para>
      /// </remarks>
      [SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage"), SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("netapi32.dll", SetLastError = true, CharSet = CharSet.Unicode), SuppressUnmanagedCodeSecurity]
      [return: MarshalAs(UnmanagedType.U4)]
      internal static extern uint NetShareGetInfo([MarshalAs(UnmanagedType.LPWStr)] string serverName, [MarshalAs(UnmanagedType.LPWStr)] string netName, [MarshalAs(UnmanagedType.U4)] uint level, out SafeGlobalMemoryBufferHandle lpBuffer);



      /// <summary>Retrieves operating statistics for a service. Currently, only the workstation and server services are supported.</summary>
      /// <returns>
      /// If the function succeeds, the return value is NERR_Success.
      /// If the function fails, the return value is a system error code.
      /// </returns>
      /// <remarks>
      /// <para>No special group membership is required to obtain workstation statistics.</para>
      /// <para>Only members of the Administrators or Server Operators local group can successfully execute the NetStatisticsGet function on a remote server.</para>
      /// <para>Minimum supported client: Windows XP [desktop apps only]</para>
      /// <para>Minimum supported server: Windows Server 2003 [desktop apps only]</para>
      /// </remarks>
      [SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage"), SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("netapi32.dll", SetLastError = true, CharSet = CharSet.Unicode), SuppressUnmanagedCodeSecurity]
      internal static extern uint NetStatisticsGet([MarshalAs(UnmanagedType.LPWStr)] string server, [MarshalAs(UnmanagedType.LPWStr)] string service, [MarshalAs(UnmanagedType.U4)] uint level, [MarshalAs(UnmanagedType.U4)] uint options, out SafeGlobalMemoryBufferHandle bufptr);
   }
}
