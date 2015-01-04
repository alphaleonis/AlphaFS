/* Copyright (c) 2008-2015 Peter Palotas, Jeffrey Jangli, Normalex
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
using System.Globalization;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;

namespace Alphaleonis.Win32.Network
{
   internal static partial class NativeMethods
   {
      #region Constants
      
      /// <summary>A constant of type DWORD that is set to –1. This value is valid as an input parameter to any method in section 3.1.4 that takes a PreferedMaximumLength parameter. When specified as an input parameter, this value indicates that the method MUST allocate as much space as the data requires.</summary>
      /// <remarks>MSDN "2.2.2.2 MAX_PREFERRED_LENGTH": http://msdn.microsoft.com/en-us/library/cc247107.aspx </remarks>
      internal const int MaxPreferredLength = -1;

      #endregion // Constants

      #region GetComputerDomain

      internal static readonly string ComputerDomain = GetComputerDomain();

      [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
      private static string GetComputerDomain(bool fdqn = false)
      {
         string domain = Environment.UserDomainName;
         string machine = Environment.MachineName.ToUpper(CultureInfo.InvariantCulture);

         try
         {
            if (fdqn)
            {
               domain = Dns.GetHostEntry("LocalHost").HostName.ToUpper(CultureInfo.InvariantCulture).Replace(machine + ".", string.Empty);
               domain = domain.Replace(machine, string.Empty);
            }
         }
         catch
         {
         }

         return domain;
      }

      #endregion // GetComputerDomain


      #region Distributed File System Functions

      #region NetDfsEnum

      /// <summary>Enumerates the Distributed File System (DFS) namespaces hosted on a server or DFS links of a namespace hosted by a server.</summary>
      /// <remarks>
      /// If the function succeeds, the return value is NERR_Success.
      /// If the function fails, the return value is a system error code.
      /// </remarks>
      /// <remarks>No special group membership is required for using the NetDfsEnum function.</remarks>
      /// <remarks>Minimum supported client: Windows XP [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003 [desktop apps only]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("netapi32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "NetDfsEnum")]
      [return: MarshalAs(UnmanagedType.U4)]
      internal static extern uint NetDfsEnum([MarshalAs(UnmanagedType.LPWStr)] string dfsName, [MarshalAs(UnmanagedType.U4)] uint level, [MarshalAs(UnmanagedType.U4)] int prefMaxLen, out SafeNetApiBuffer buffer, [MarshalAs(UnmanagedType.U4)] out uint entriesRead, [MarshalAs(UnmanagedType.U4)] out uint resumeHandle);

      #endregion // NetDfsEnum

      #region NetDfsGetClientInfo

      /// <summary>Retrieves information about a Distributed File System (DFS) root or link from the cache maintained by the DFS client.</summary>
      /// <returns>
      /// If the function succeeds, the return value is NERR_Success.
      /// If the function fails, the return value is a system error code.
      /// </returns>
      /// <remarks>No special group membership is required for using the NetDfsGetClientInfo function.</remarks>
      /// <remarks>Minimum supported client: Windows XP [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003 [desktop apps only]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("netapi32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "NetDfsGetClientInfo")]
      [return: MarshalAs(UnmanagedType.U4)]
      internal static extern uint NetDfsGetClientInfo([MarshalAs(UnmanagedType.LPWStr)] string dfsEntryPath, [MarshalAs(UnmanagedType.LPWStr)] string serverName, [MarshalAs(UnmanagedType.LPWStr)] string shareName, [MarshalAs(UnmanagedType.U4)] uint level, out SafeNetApiBuffer buffer);

      #endregion // NetDfsGetClientInfo

      #region NetDfsGetInfo

      /// <summary>Retrieves information about a specified Distributed File System (DFS) root or link in a DFS namespace.</summary>
      /// <returns>
      /// If the function succeeds, the return value is NERR_Success.
      /// If the function fails, the return value is a system error code.
      /// </returns>
      /// <remarks>No special group membership is required for using the NetDfsGetInfo function.</remarks>
      /// <remarks>Minimum supported client: Windows XP [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003 [desktop apps only]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("netapi32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "NetDfsGetInfo")]
      [return: MarshalAs(UnmanagedType.U4)]
      internal static extern uint NetDfsGetInfo([MarshalAs(UnmanagedType.LPWStr)] string dfsEntryPath, [MarshalAs(UnmanagedType.LPWStr)] string serverName, [MarshalAs(UnmanagedType.LPWStr)] string shareName, [MarshalAs(UnmanagedType.U4)] uint level, out SafeNetApiBuffer buffer);

      #endregion // NetDfsGetInfo

      #endregion // Distributed File System Functions
      
      #region Network Management Functions

      #region NetApiBufferFree

      /// <summary>The NetApiBufferFree function frees the memory that the NetApiBufferAllocate function allocates.</summary>
      /// <returns>
      /// If the function succeeds, the return value is NERR_Success.
      /// If the function fails, the return value is a system error code.
      /// </returns>
      /// <remarks>SetLastError is set to <see langword="false"/>.</remarks>
      /// <remarks>Minimum supported client: Windows 2000 Professional [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows 2000 Server [desktop apps only]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("netapi32.dll", SetLastError = false, CharSet = CharSet.Unicode, EntryPoint = "NetApiBufferFree")]
      [return: MarshalAs(UnmanagedType.U4)]
      internal static extern uint NetApiBufferFree(IntPtr buffer);

      #endregion // NetApiBufferFree

      #region NetServerDiskEnum

      /// <summary>The NetServerDiskEnum function retrieves a list of disk drives on a server.</summary>
      /// <returns>
      /// If the function succeeds, the return value is NERR_Success.
      /// If the function fails, the return value is a system error code.
      /// </returns>
      /// <remarks>The function returns an array of three-character strings (a drive letter, a colon, and a terminating null character).</remarks>
      /// <remarks>Only members of the Administrators or Server Operators local group can successfully execute the NetServerDiskEnum function on a remote computer.</remarks>
      /// <remarks>Minimum supported client: Windows 2000 Professional [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows 2000 Server [desktop apps only]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("netapi32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "NetServerDiskEnum")]
      [return: MarshalAs(UnmanagedType.U4)]
      internal static extern uint NetServerDiskEnum([MarshalAs(UnmanagedType.LPWStr)] string serverName, [MarshalAs(UnmanagedType.U4)] uint level, out SafeNetApiBuffer bufPtr, [MarshalAs(UnmanagedType.I4)] int prefMaxLen, [MarshalAs(UnmanagedType.U4)] out uint entriesRead, [MarshalAs(UnmanagedType.U4)] out uint totalEntries, [MarshalAs(UnmanagedType.U4)] out uint resumeHandle);

      #endregion // NetServerDiskEnum

      #endregion // Network Management Functions
      
      #region Network Share Management Functions

      #region NetConnectionEnum

      /// <summary>Lists all connections made to a shared resource on the server or all connections established from a particular computer.</summary>
      /// <returns>
      /// If the function succeeds, the return value is NERR_Success.
      /// If the function fails, the return value is a system error code.
      /// </returns>
      /// <remarks>Administrator, Server or Print Operator, or Power User group membership is required to successfully execute the NetConnectionEnum function.</remarks>
      /// <remarks>Minimum supported client: Windows XP [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003 [desktop apps only]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("netapi32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "NetConnectionEnum")]
      [return: MarshalAs(UnmanagedType.U4)]
      internal static extern uint NetConnectionEnum([MarshalAs(UnmanagedType.LPWStr)] string serverName, [MarshalAs(UnmanagedType.LPWStr)] string qualifier, [MarshalAs(UnmanagedType.U4)] uint level, out SafeNetApiBuffer bufPtr, [MarshalAs(UnmanagedType.I4)] int prefMaxLen, [MarshalAs(UnmanagedType.U4)] out uint entriesRead, [MarshalAs(UnmanagedType.U4)] out uint totalEntries, [MarshalAs(UnmanagedType.U4)] out uint resumeHandle);

      #endregion // NetConnectionEnum

      #region NetFileClose

      /// <summary>Forces a resource to close. This function can be used when an error prevents closure by any other means.</summary>
      /// <returns>
      /// If the function succeeds, the return value is NERR_Success.
      /// If the function fails, the return value is a system error code.
      /// </returns>
      /// <remarks>You should use NetFileClose with caution because it does not write data cached on the client system to the file before closing the file.</remarks>
      /// <remarks>Only members of the Administrators or Server Operators local group can successfully execute the NetFileEnum function.</remarks>
      /// <remarks>Minimum supported client: Windows XP [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003 [desktop apps only]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("netapi32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "NetFileClose")]
      [return: MarshalAs(UnmanagedType.U4)]
      internal static extern uint NetFileClose([MarshalAs(UnmanagedType.LPWStr)] string serverName, [MarshalAs(UnmanagedType.U4)] uint fileid);

      #endregion // NetFileClose
      
      #region NetFileEnum

      /// <summary>Returns information about some or all open files on a server, depending on the parameters specified.</summary>
      /// <returns>
      /// If the function succeeds, the return value is NERR_Success.
      /// If the function fails, the return value is a system error code.
      /// </returns>
      /// <remarks>Only members of the Administrators or Server Operators local group can successfully execute the NetFileEnum function.</remarks>
      /// <remarks>Minimum supported client: Windows XP [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003 [desktop apps only]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("netapi32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "NetFileEnum")]
      [return: MarshalAs(UnmanagedType.U4)]
      internal static extern uint NetFileEnum([MarshalAs(UnmanagedType.LPWStr)] string serverName, [MarshalAs(UnmanagedType.LPWStr)] string basepath, [MarshalAs(UnmanagedType.LPWStr)] string username, [MarshalAs(UnmanagedType.U4)] uint level, out SafeNetApiBuffer buffer, [MarshalAs(UnmanagedType.I4)] int prefmaxlen, [MarshalAs(UnmanagedType.U4)] out uint entriesRead, [MarshalAs(UnmanagedType.U4)] out uint totalentries, [MarshalAs(UnmanagedType.U4)] out uint resumeHandle);

      #endregion // NetFileEnum

      #region NetShareEnum

      /// <summary>Retrieves information about each (hidden) Server Message Block (SMB) resource/share on a server.</summary>
      /// <returns>
      /// If the function succeeds, the return value is NERR_Success.
      /// If the function fails, the return value is a system error code.
      /// </returns>
      /// <remarks>
      /// For interactive users (users who are logged on locally to the machine), no special group membership is required to execute the NetShareEnum function.
      /// For non-interactive users, Administrator, Power User, Print Operator, or Server Operator group membership is required to successfully execute
      /// the NetShareEnum function at levels 2, 502, and 503. No special group membership is required for level 0 or level 1 calls.
      /// </remarks>
      /// <remarks>Windows Server 2003 and Windows XP: For all users, Administrator, Power User, Print Operator, or Server Operator group membership is required to successfully execute the NetShareEnum function at levels 2 and 502.</remarks>
      /// <remarks>You can also use the WNetEnumResource function to retrieve resource information. However, WNetEnumResource does not enumerate hidden shares or users connected to a share.</remarks>
      /// <remarks>This function applies only to Server Message Block (SMB) shares.</remarks>
      /// <remarks>Minimum supported client: Windows XP [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003 [desktop apps only]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("netapi32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "NetShareEnum")]
      [return: MarshalAs(UnmanagedType.U4)]
      internal static extern uint NetShareEnum([MarshalAs(UnmanagedType.LPWStr)] string serverName, [MarshalAs(UnmanagedType.U4)] uint level, out SafeNetApiBuffer bufPtr, [MarshalAs(UnmanagedType.I4)] int prefMaxLen, [MarshalAs(UnmanagedType.U4)] out uint entriesRead, [MarshalAs(UnmanagedType.U4)] out uint totalEntries, [MarshalAs(UnmanagedType.U4)] out uint resumeHandle);

      #endregion // NetShareEnum
      
      #region NetShareGetInfo

      /// <summary>Retrieves information about a particular Server Message Block (SMB) shared resource on a server.</summary>
      /// <returns>
      /// If the function succeeds, the return value is NERR_Success.
      /// If the function fails, the return value is a system error code.
      /// </returns>
      /// <remarks>This function applies only to Server Message Block (SMB) shares.</remarks>
      /// <remarks>Minimum supported client: Windows XP [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003 [desktop apps only]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("netapi32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "NetShareGetInfo")]
      [return: MarshalAs(UnmanagedType.U4)]
      internal static extern uint NetShareGetInfo([MarshalAs(UnmanagedType.LPWStr)] string serverName, [MarshalAs(UnmanagedType.LPWStr)] string netName, [MarshalAs(UnmanagedType.U4)] uint level, out SafeNetApiBuffer lpBuffer);

      #endregion // NetShareGetInfo

      #endregion // Network Share Management Functions

      #region Windows Networking Functions

      #region WNetCancelConnection

      /// <summary>The WNetCancelConnection function cancels an existing network connection. You can also call the function to remove remembered network connections that are not currently connected.</summary>
      /// <returns>
      /// If the function succeeds, the return value is <see cref="Win32Errors.NO_ERROR"/>
      /// If the function fails, the return value is a system error code.
      /// </returns>
      /// <remarks>Minimum supported client: Windows 2000 Professional [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows 2000 Server [desktop apps only]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("mpr.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "WNetCancelConnection2W")]
      [return: MarshalAs(UnmanagedType.U4)]
      internal static extern uint WNetCancelConnection([MarshalAs(UnmanagedType.LPWStr)] string lpName, Connect dwFlags, [MarshalAs(UnmanagedType.Bool)] bool fForce);

      #endregion // WNetCancelConnection

      #region WNetGetUniversalName

      /// <summary>The WNetGetUniversalName function takes a drive-based path for a network resource and returns an information structure that contains a more universal form of the name.</summary>
      /// <returns>
      /// If the function succeeds, the return value is <see cref="Win32Errors.NO_ERROR"/>
      /// If the function fails, the return value is a system error code.
      /// </returns>
      /// <remarks>Minimum supported client: Windows 2000 Professional [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows 2000 Server [desktop apps only]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("mpr.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "WNetGetUniversalNameW")]
      [return: MarshalAs(UnmanagedType.U4)]
      internal static extern uint WNetGetUniversalName([MarshalAs(UnmanagedType.LPWStr)] string lpLocalPath, [MarshalAs(UnmanagedType.U4)] uint dwInfoLevel, SafeGlobalMemoryBufferHandle lpBuffer, [MarshalAs(UnmanagedType.U4)] out uint lpBufferSize);

      #endregion // WNetGetUniversalName

      #region WNetUseConnection

      /// <summary>The WNetUseConnection function creates a connection to a network resource. The function can redirect a local device to a network resource.</summary>
      /// <returns>
      /// If the function succeeds, the return value is <see cref="Win32Errors.NO_ERROR"/>
      /// If the function fails, the return value is a system error code.
      /// </returns>
      /// <remarks>Minimum supported client: Windows 2000 Professional [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows 2000 Server [desktop apps only]</remarks>
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      [DllImport("mpr.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "WNetUseConnectionW")]
      [return: MarshalAs(UnmanagedType.U4)]
      internal static extern uint WNetUseConnection(IntPtr hwndOwner, [MarshalAs(UnmanagedType.Struct)] ref NetResource lpNetResource, [MarshalAs(UnmanagedType.LPWStr)] string lpPassword, [MarshalAs(UnmanagedType.LPWStr)] string lpUserId, [MarshalAs(UnmanagedType.U4)] Connect dwFlags, StringBuilder lpAccessName, [MarshalAs(UnmanagedType.U4)] out uint lpBufferSize, [MarshalAs(UnmanagedType.U4)] out uint lpResult);
      // Note: When NetResource is struct: use ref, when NetResource is class: ommit ref.

      #endregion // WNetUseConnection

      #endregion // Windows Networking Functions
   }
}