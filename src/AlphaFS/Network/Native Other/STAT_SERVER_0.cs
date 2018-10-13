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
      /// <summary>Contains statistical information about the server.</summary>
      /// <remarks>
      /// <para>Minimum supported client: Windows XP [desktop apps only]</para>
      /// <para>Minimum supported server: Windows Server 2003 [desktop apps only]</para>
      /// </remarks>
      [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
      internal struct STAT_SERVER_0
      {
         /// <summary>
         /// <para>Specifies a DWORD value that indicates the time when statistics collection started (or when the statistics were last cleared).</para>
         /// <para>The value is stored as the number of seconds that have elapsed since 00:00:00, January 1, 1970, GMT.</para>
         /// <para>To calculate the length of time that statistics have been collected,</para>
         /// <para>subtract the value of this member from the present time.</para>
         /// </summary>
         [MarshalAs(UnmanagedType.U4)] public readonly uint sts0_start;

         /// <summary>Specifies a DWORD value that indicates the number of times a file is opened on a server. This includes the number of times named pipes are opened.</summary>
         [MarshalAs(UnmanagedType.U4)] public readonly uint sts0_fopens;

         /// <summary>Specifies a DWORD value that indicates the number of times a server device is opened.</summary>
         [MarshalAs(UnmanagedType.U4)] public readonly uint sts0_devopens;

         /// <summary>Specifies a DWORD value that indicates the number of server print jobs spooled.</summary>
         [MarshalAs(UnmanagedType.U4)] public readonly uint sts0_jobsqueued;

         /// <summary>Specifies a DWORD value that indicates the number of times the server session started.</summary>
         [MarshalAs(UnmanagedType.U4)] public readonly uint sts0_sopens;

         /// <summary>Specifies a DWORD value that indicates the number of times the server session automatically disconnected.</summary>
         [MarshalAs(UnmanagedType.U4)] public readonly uint sts0_stimedout;

         /// <summary>Specifies a DWORD value that indicates the number of times the server sessions failed with an error.</summary>
         [MarshalAs(UnmanagedType.U4)] public readonly uint sts0_serrorout;

         /// <summary>Specifies a DWORD value that indicates the number of server password violations.</summary>
         [MarshalAs(UnmanagedType.U4)] public readonly uint sts0_pwerrors;

         /// <summary>Specifies a DWORD value that indicates the number of server access permission errors.</summary>
         [MarshalAs(UnmanagedType.U4)] public readonly uint sts0_permerrors;

         /// <summary>Specifies a DWORD value that indicates the number of server system errors.</summary>
         [MarshalAs(UnmanagedType.U4)] public readonly uint sts0_syserrors;

         /// <summary>Specifies the low-order DWORD of the number of server bytes sent to the network.</summary>
         [MarshalAs(UnmanagedType.U4)] public readonly uint sts0_bytessent_low;

         /// <summary>Specifies the high-order DWORD of the number of server bytes sent to the network.</summary>
         [MarshalAs(UnmanagedType.U4)] public readonly uint sts0_bytessent_high;

         /// <summary>Specifies the low-order DWORD of the number of server bytes received from the network.</summary>
         [MarshalAs(UnmanagedType.U4)] public readonly uint sts0_bytesrcvd_low;

         /// <summary>Specifies the high-order DWORD of the number of server bytes received from the network.</summary>
         [MarshalAs(UnmanagedType.U4)] public readonly uint sts0_bytesrcvd_high;

         /// <summary>Specifies a DWORD value that indicates the average server response time (in milliseconds).</summary>
         [MarshalAs(UnmanagedType.U4)] public readonly uint sts0_avresponse;

         /// <summary>Specifies a DWORD value that indicates the number of times the server required a request buffer but failed to allocate one. This value indicates that the server parameters may need adjustment.</summary>
         [MarshalAs(UnmanagedType.U4)] public readonly uint sts0_reqbufneed;

         /// <summary>Specifies a DWORD value that indicates the number of times the server required a big buffer but failed to allocate one. This value indicates that the server parameters may need adjustment.</summary>
         [MarshalAs(UnmanagedType.U4)] public readonly uint sts0_bigbufneed;
      }
   }
}
