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
      /// <summary>Contains statistical information about the specified workstation.</summary>
      /// <remarks>
      /// <para>Minimum supported client: Windows XP [desktop apps only]</para>
      /// <para>Minimum supported server: Windows Server 2003 [desktop apps only]</para>
      /// </remarks>
      [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
      internal struct STAT_WORKSTATION_0
      {
         /// <summary>
         /// <para>Specifies the time statistics collection started. This member also indicates when statistics for the workstations were last cleared.</para>
         /// <para>The value is stored as the number of seconds elapsed since 00:00:00, January 1, 1970.</para>
         /// </summary>
         [MarshalAs(UnmanagedType.U8)] public readonly long StatisticsStartTime;

         
         /// <summary>Specifies the total number of bytes received by the workstation.</summary>
         [MarshalAs(UnmanagedType.U8)] public readonly long BytesReceived;

         /// <summary>Specifies the total number of server message blocks (SMBs) received by the workstation.</summary>
         [MarshalAs(UnmanagedType.U8)] public readonly long SmbsReceived;

         /// <summary>Specifies the total number of bytes that have been read by paging I/O requests.</summary>
         [MarshalAs(UnmanagedType.U8)] public readonly long PagingReadBytesRequested;

         /// <summary>Specifies the total number of bytes that have been read by non-paging I/O requests.</summary>
         [MarshalAs(UnmanagedType.U8)] public readonly long NonPagingReadBytesRequested;

         /// <summary>Specifies the total number of bytes that have been read by cache I/O requests.</summary>
         [MarshalAs(UnmanagedType.U8)] public readonly long CacheReadBytesRequested;

         /// <summary>Specifies the total amount of bytes that have been read by disk I/O requests.</summary>
         [MarshalAs(UnmanagedType.U8)] public readonly long NetworkReadBytesRequested;

         /// <summary>Specifies the total number of bytes transmitted by the workstation.</summary>
         [MarshalAs(UnmanagedType.U8)] public readonly long BytesTransmitted;

         /// <summary>Specifies the total number of SMBs transmitted by the workstation.</summary>
         [MarshalAs(UnmanagedType.U8)] public readonly long SmbsTransmitted;

         /// <summary>Specifies the total number of bytes that have been written by paging I/O requests.</summary>
         [MarshalAs(UnmanagedType.U8)] public readonly long PagingWriteBytesRequested;

         /// <summary>Specifies the total number of bytes that have been written by non-paging I/O requests.</summary>
         [MarshalAs(UnmanagedType.U8)] public readonly long NonPagingWriteBytesRequested;

         /// <summary>Specifies the total number of bytes that have been written by cache I/O requests.</summary>
         [MarshalAs(UnmanagedType.U8)] public readonly long CacheWriteBytesRequested;

         /// <summary>Specifies the total number of bytes that have been written by disk I/O requests.</summary>
         [MarshalAs(UnmanagedType.U8)] public readonly long NetworkWriteBytesRequested;

         /// <summary>Specifies the total number of network operations that failed to begin.</summary>
         [MarshalAs(UnmanagedType.U4)] public readonly uint InitiallyFailedOperations;

         /// <summary>Specifies the total number of network operations that failed to complete.</summary>
         [MarshalAs(UnmanagedType.U4)] public readonly uint FailedCompletionOperations;

         /// <summary>Specifies the total number of read operations initiated by the workstation.</summary>
         [MarshalAs(UnmanagedType.U4)] public readonly uint ReadOperations;

         /// <summary>Specifies the total number of random access reads initiated by the workstation.</summary>
         [MarshalAs(UnmanagedType.U4)] public readonly uint RandomReadOperations;

         /// <summary>Specifies the total number of read requests the workstation has sent to servers.</summary>
         [MarshalAs(UnmanagedType.U4)] public readonly uint ReadSmbs;

         /// <summary>Specifies the total number of read requests the workstation has sent to servers that are greater than twice the size of the server's negotiated buffer size.</summary>
         [MarshalAs(UnmanagedType.U4)] public readonly uint LargeReadSmbs;

         /// <summary>Specifies the total number of read requests the workstation has sent to servers that are less than 1/4 of the size of the server's negotiated buffer size.</summary>
         [MarshalAs(UnmanagedType.U4)] public readonly uint SmallReadSmbs;

         /// <summary>Specifies the total number of write operations initiated by the workstation.</summary>
         [MarshalAs(UnmanagedType.U4)] public readonly uint WriteOperations;

         /// <summary>Specifies the total number of random access writes initiated by the workstation.</summary>
         [MarshalAs(UnmanagedType.U4)] public readonly uint RandomWriteOperations;

         /// <summary>Specifies the total number of write requests the workstation has sent to servers.</summary>
         [MarshalAs(UnmanagedType.U4)] public readonly uint WriteSmbs;

         /// <summary>Specifies the total number of write requests the workstation has sent to servers that are greater than twice the size of the server's negotiated buffer size.</summary>
         [MarshalAs(UnmanagedType.U4)] public readonly uint LargeWriteSmbs;

         /// <summary>Specifies the total number of write requests the workstation has sent to servers that are less than 1/4 of the size of the server's negotiated buffer size.</summary>
         [MarshalAs(UnmanagedType.U4)] public readonly uint SmallWriteSmbs;

         /// <summary>Specifies the total number of raw read requests made by the workstation that have been denied.</summary>
         [MarshalAs(UnmanagedType.U4)] public readonly uint RawReadsDenied;

         /// <summary>Specifies the total number of raw write requests made by the workstation that have been denied.</summary>
         [MarshalAs(UnmanagedType.U4)] public readonly uint RawWritesDenied;

         /// <summary>Specifies the total number of network errors received by the workstation.</summary>
         [MarshalAs(UnmanagedType.U4)] public readonly uint NetworkErrors;

         /// <summary>Specifies the total number of workstation sessions that were established.</summary>
         [MarshalAs(UnmanagedType.U4)] public readonly uint Sessions;

         /// <summary>Specifies the number of times the workstation attempted to create a session but failed.</summary>
         [MarshalAs(UnmanagedType.U4)] public readonly uint FailedSessions;

         /// <summary>Specifies the total number of connections that have failed.</summary>
         [MarshalAs(UnmanagedType.U4)] public readonly uint Reconnects;

         /// <summary>Specifies the total number of connections to servers supporting the PCNET dialect that have succeeded.</summary>
         [MarshalAs(UnmanagedType.U4)] public readonly uint CoreConnects;

         /// <summary>Specifies the total number of connections to servers supporting the LanManager 2.0 dialect that have succeeded.</summary>
         [MarshalAs(UnmanagedType.U4)] public readonly uint Lanman20Connects;

         /// <summary>Specifies the total number of connections to servers supporting the LanManager 2.1 dialect that have succeeded.</summary>
         [MarshalAs(UnmanagedType.U4)] public readonly uint Lanman21Connects;

         /// <summary>Specifies the total number of connections to servers supporting the NTLM dialect that have succeeded.</summary>
         [MarshalAs(UnmanagedType.U4)] public readonly uint LanmanNtConnects;

         /// <summary>Specifies the number of times the workstation was disconnected by a network server.</summary>
         [MarshalAs(UnmanagedType.U4)] public readonly uint ServerDisconnects;

         /// <summary>Specifies the total number of sessions that have expired on the workstation.</summary>
         [MarshalAs(UnmanagedType.U4)] public readonly uint HungSessions;

         /// <summary>Specifies the total number of network connections established by the workstation.</summary>
         [MarshalAs(UnmanagedType.U4)] public readonly uint UseCount;

         /// <summary>Specifies the total number of failed network connections for the workstation.</summary>
         [MarshalAs(UnmanagedType.U4)] public readonly uint FailedUseCount;

         /// <summary>Specifies the number of current requests that have not been completed.</summary>
         [MarshalAs(UnmanagedType.U4)] public readonly uint CurrentCommands;
      }
   }
}
