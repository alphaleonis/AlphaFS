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
using System.Runtime.InteropServices;

namespace Alphaleonis.Win32.Filesystem
{
   internal static partial class NativeMethods
   {
      /// <summary>Contains the disk partition information.
      /// <para>Minimum supported client: Windows XP [desktop apps only]</para>
      /// <para>Minimum supported server: Windows Server 2003 [desktop apps only]</para>
      /// </summary>
      [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Unicode)]
      internal struct DISK_PARTITION_INFO
      {
         /// <summary>The size of this structure, in bytes.</summary>
         [FieldOffset(0)] [MarshalAs(UnmanagedType.U4)]
         public readonly uint SizeOfPartitionInfo;

         /// <summary>The format of a partition.</summary>
         [FieldOffset(4)] [MarshalAs(UnmanagedType.U4)]
         public readonly PARTITION_STYLE PartitionStyle;

         /// <summary>Signature: MBR signature of the partition.
         /// <para>If PartitionStyle is PARTITION_STYLE_MBR (0), the union is a structure that contains information for an master boot record partition, which includes a disk signature and a checksum.</para>
         /// </summary>
         [FieldOffset(8)] [MarshalAs(UnmanagedType.U4)]
         public readonly uint MbrSignature;

         /// <summary>DiskId: GUID of the GPT partition.
         /// <para>If PartitionStyle is PARTITION_STYLE_GPT (1), the union is a structure that contains information for a GUID partition table partition, which includes a disk identifier (GUID).</para>
         /// </summary>
         [FieldOffset(8)] public Guid DiskId;
      }
   }
}
