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
      /// <summary>Contains information about the session, including name of the computer; name of the user; and active and idle times for the session.</summary>
      /// <remarks>
      /// <para>Minimum supported client: Windows XP [desktop apps only]</para>
      /// <para>Minimum supported server: Windows Server 2003 [desktop apps only]</para>
      /// </remarks>
      [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
      internal struct SESSION_INFO_10
      {
         /// <summary>Pointer to a Unicode string specifying the name of the computer that established the session. This string cannot contain a backslash (\).</summary>
         [MarshalAs(UnmanagedType.LPWStr)] public readonly string sesi10_cname;

         /// <summary>Pointer to a Unicode string specifying the name of the user who established the session.</summary>
         [MarshalAs(UnmanagedType.LPWStr)] public readonly string sesi10_username;

         /// <summary>Specifies the number of seconds the session has been active.</summary>
         [MarshalAs(UnmanagedType.U4)] public readonly uint sesi10_time;

         /// <summary>Specifies the number of seconds the session has been idle.</summary>
         [MarshalAs(UnmanagedType.U4)] public readonly uint sesi10_idle_time;
      }
   }
}
