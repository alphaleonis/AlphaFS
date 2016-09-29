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

using System.Runtime.InteropServices;

namespace Alphaleonis.Win32.Network
{
   internal static partial class NativeMethods
   {
      /// <summary>Contains the identification number of a connection, number of open files, connection time, number of users on the connection, and the type of connection.</summary>
      /// <remarks>Minimum supported client: Windows XP [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003 [desktop apps only]</remarks>
      [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
      internal struct CONNECTION_INFO_1
      {
         /// <summary>Specifies a connection identification number.</summary>
         [MarshalAs(UnmanagedType.U4)] public readonly uint coni1_id;

         /// <summary>A combination of values that specify the type of connection made from the local device name to the shared resource.</summary>
         [MarshalAs(UnmanagedType.U4)] public readonly ShareType coni1_type;

         /// <summary>Specifies the number of files currently open as a result of the connection.</summary>
         [MarshalAs(UnmanagedType.U4)] public readonly uint coni1_num_opens;

         /// <summary>Specifies the number of users on the connection.</summary>
         [MarshalAs(UnmanagedType.U4)] public readonly uint coni1_num_users;

         /// <summary>Specifies the number of seconds that the connection has been established.</summary>
         [MarshalAs(UnmanagedType.U4)] public readonly uint coni1_time;

         /// <summary>If the server sharing the resource is running with user-level security, the UserName member describes which user made the connection. If the server is running with share-level security, coni1_username describes which computer (computername) made the connection.</summary>
         /// <remarks>Note that Windows does not support share-level security.</remarks>
         [MarshalAs(UnmanagedType.LPWStr)] public readonly string coni1_username;

         /// <summary>String that specifies either the share name of the server's shared resource or the computername of the client. The value of this member depends on which name was specified as the qualifier parameter to the NetConnectionEnum function.</summary>
         [MarshalAs(UnmanagedType.LPWStr)] public readonly string oni1_netname;
      }
   }
}
