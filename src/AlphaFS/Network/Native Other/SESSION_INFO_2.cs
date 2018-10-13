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

using System.Runtime.InteropServices;

namespace Alphaleonis.Win32.Network
{
   internal static partial class NativeMethods
   {
      /// <summary>Contains information about the session, including name of the computer; name of the user; open files, pipes, and devices on the computer; and the type of client that established the session.</summary>
      /// <remarks>
      /// <para>Minimum supported client: Windows XP [desktop apps only]</para>
      /// <para>Minimum supported server: Windows Server 2003 [desktop apps only]</para>
      /// </remarks>
      [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
      internal struct SESSION_INFO_2
      {
         /// <summary>Pointer to a Unicode string specifying the name of the computer that established the session. This string cannot contain a backslash (\).</summary>
         [MarshalAs(UnmanagedType.LPWStr)] public readonly string sesi2_cname;

         /// <summary>Pointer to a Unicode string specifying the name of the user who established the session.</summary>
         [MarshalAs(UnmanagedType.LPWStr)] public readonly string sesi2_username;

         /// <summary>Specifies a DWORD value that contains the number of files, devices, and pipes opened during the session.</summary>
         [MarshalAs(UnmanagedType.U4)] public readonly uint sesi2_num_opens;

         /// <summary>Specifies a DWORD value that contains the number of seconds the session has been active.</summary>
         [MarshalAs(UnmanagedType.U4)] public readonly uint sesi2_time;

         /// <summary>Specifies a DWORD value that contains the number of seconds the session has been idle.</summary>
         [MarshalAs(UnmanagedType.U4)] public readonly uint sesi2_idle_time;

         /// <summary>
         /// <para>Specifies a DWORD value that describes how the user established the session. This member can be one of the following values:</para>
         /// <para>SESS_GUEST: The user specified by the <see cref="sesi2_username"/> member established the session using a guest account.</para>
         /// <para>SESS_NOENCRYPTION: The user specified by the <see cref="sesi2_username"/> member established the session without using password encryption.</para>
         /// </summary>
         [MarshalAs(UnmanagedType.U4)] public readonly uint sesi2_user_flags;


         /// <summary>
         /// <para>Pointer to a Unicode string that specifies the type of client that established the session. Following are the defined types for LAN Manager servers:</para>
         /// <para>DOS LM 1.0: LAN Manager for MS-DOS 1.0 clients</para>
         /// <para>DOS LM 2.0: LAN Manager for MS-DOS 2.0 clients</para>
         /// <para>OS/2 LM 1.0: LAN Manager for MS-OS/2 1.0 clients</para>
         /// <para>OS/2 LM 2.0: LAN Manager for MS-OS/2 2.0 clients</para>
         /// <para>Sessions from LAN Manager servers running UNIX also will appear as LAN Manager 2.0.</para>
         /// </summary>
         [MarshalAs(UnmanagedType.LPWStr)] public readonly string sesi2_cltype_name;
      }
   }
}
