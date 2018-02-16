/*  Copyright (C) 2008-2017 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
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

namespace Alphaleonis.Win32.Filesystem
{
   internal static partial class NativeMethods
   {
      /// <summary>Contains partition information for standard AT-style master boot record (MBR) and Extensible Firmware Interface (EFI) disks.</summary>
      /// <remarks>
      /// <para>Minimum supported client: Windows XP [desktop apps only]</para>
      /// <para>Minimum supported server: Windows Server 2003 [desktop apps only]</para>
      /// </remarks>
      [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
      internal struct PARTITION_INFORMATION_EX
      {
         /// <summary>The format of the partition.</summary>
         public PARTITION_STYLE PartitionStyle;

         /// <summary>The starting offset of the partition.</summary>
         [MarshalAs(UnmanagedType.U8)] public long StartingOffset;

         /// <summary>The size of the partition, in bytes.</summary>
         [MarshalAs(UnmanagedType.U8)] public long PartitionLength;

         /// <summary>The number of the partition (1-based).</summary>
         [MarshalAs(UnmanagedType.U4)] public uint PartitionNumber;

         /// <summary>If this member is TRUE, the partition is rewritable. The value of this parameter should be set to TRUE.</summary>
         [MarshalAs(UnmanagedType.Bool)] public bool RewritePartition;

         //public PARTITION_INFORMATION_UNION PartitionInformation;
      }


      [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
      internal struct PARTITION_INFORMATION_UNION
      {
         public PARTITION_INFORMATION_MBR Mbr;
         public PARTITION_INFORMATION_GPT Gpt;
      }
   }
}
