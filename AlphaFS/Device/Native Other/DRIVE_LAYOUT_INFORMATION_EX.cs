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

namespace Alphaleonis.Win32.Device
{
   internal static partial class NativeMethods
   {
      /// <summary>Contains extended information about a drive's partitions.
      /// <para>Minimum supported client: Windows XP [desktop apps only]</para>
      /// <para>Minimum supported server: Windows Server 2003 [desktop apps only]</para>
      /// </summary>
      [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Unicode)]
      internal struct DRIVE_LAYOUT_INFORMATION_EX
      {
         /// <summary>The style of the partitions on the drive enumerated by the <see cref="PARTITION_STYLE"/> enumeration.</summary>
         [FieldOffset(0)] [MarshalAs(UnmanagedType.U4)]
         public readonly PartitionStyle PartitionStyle;

         /// <summary>The number of partitions on the drive. On hard disks with the MBR layout, this value will always be a multiple of 4.</summary>
         [FieldOffset(4)] [MarshalAs(UnmanagedType.U4)]
         public readonly uint PartitionCount;

         /// <summary>A <see cref="DRIVE_LAYOUT_INFORMATION_MBR"/> structure containing information about the master boot record type partitioning on the drive.</summary>
         [FieldOffset(8)] public DRIVE_LAYOUT_INFORMATION_MBR Mbr;

         /// <summary>A <see cref="DRIVE_LAYOUT_INFORMATION_GPT"/> structure containing information about the GUID disk partition type partitioning on the drive.</summary>
         [FieldOffset(8)] public DRIVE_LAYOUT_INFORMATION_GPT Gpt;
      }
   }
}
