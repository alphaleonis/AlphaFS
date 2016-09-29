/*  Copyright (C) 2008-2016 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
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
using System.Runtime.InteropServices;

namespace Alphaleonis.Win32.Network
{
   internal static partial class NativeMethods
   {
      /// <summary>Contains information about the shared resource, including the server name, name of the resource, type, and permissions,
      /// the number of connections, and other pertinent information.
      /// </summary>
      /// <remarks>Minimum supported client: Windows XP [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003 [desktop apps only]</remarks>
      [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
      internal struct SHARE_INFO_503
      {
         /// <summary>The name of a shared resource.</summary>
         [MarshalAs(UnmanagedType.LPWStr)] public readonly string shi503_netname;

         /// <summary>The type of share.</summary>
         [MarshalAs(UnmanagedType.U4)] public readonly ShareType shi503_type;

         /// <summary>An optional comment about the shared resource.</summary>
         [MarshalAs(UnmanagedType.LPWStr)] public readonly string shi503_remark;

         /// <summary>The shared resource's permissions for servers running with share-level security.</summary>
         /// <remarks>Note that Windows does not support share-level security. This member is ignored on a server running user-level security.</remarks>
         [MarshalAs(UnmanagedType.U4)] public readonly AccessPermissions shi503_permissions;

         /// <summary>The maximum number of concurrent connections that the shared resource can accommodate.</summary>
         /// <remarks>The number of connections is unlimited if the value specified in this member is –1.</remarks>
         [MarshalAs(UnmanagedType.U4)] public readonly uint shi503_max_uses;

         /// <summary>The number of current connections to the resource.</summary>
         [MarshalAs(UnmanagedType.U4)] public readonly uint shi503_current_uses;

         /// <summary>The local path for the shared resource.</summary>
         /// <remarks>For disks, this member is the path being shared. For print queues, this member is the name of the print queue being shared.</remarks>
         [MarshalAs(UnmanagedType.LPWStr)] public readonly string shi503_path;

         /// <summary>The share's password (when the server is running with share-level security).</summary>
         [MarshalAs(UnmanagedType.LPWStr)] public readonly string shi503_passwd;

         /// <summary>The DNS or NetBIOS name of the remote server on which the shared resource resides.</summary>
         /// <remarks>A value of "*" indicates no configured server name.</remarks>
         [MarshalAs(UnmanagedType.LPWStr)] public readonly string shi503_servername;

         /// <summary>Reserved; must be zero.</summary>
         [MarshalAs(UnmanagedType.U4)] public readonly uint shi503_reserved;

         /// <summary>Specifies the SECURITY_DESCRIPTOR associated with this share.</summary>
         public IntPtr shi503_security_descriptor;
      }
   }
}
