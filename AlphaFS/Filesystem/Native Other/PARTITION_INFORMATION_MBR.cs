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
      /// <summary>Contains partition information specific to master boot record (MBR) disks.</summary>
      /// <remarks>
      /// <para>Minimum supported client: Windows XP [desktop apps only]</para>
      /// <para>Minimum supported server: Windows Server 2003 [desktop apps only]</para>
      /// </remarks>
      [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
      internal struct PARTITION_INFORMATION_MBR
      {
         /// <summary>The type of partition. For a list of values, see <see cref="DiskPartitionTypes"/>.</summary>
         [MarshalAs(UnmanagedType.U1)] public readonly DiskPartitionTypes PartitionType;

         /// <summary>If the member is TRUE, the partition is a boot partition. When this structure is used with the IOCTL_DISK_SET_PARTITION_INFO_EX control code, the value of this parameter is ignored.</summary>
         [MarshalAs(UnmanagedType.Bool)] public readonly bool BootIndicator;

         /// <summary>If this member is TRUE, the partition is of a recognized type. When this structure is used with the IOCTL_DISK_SET_PARTITION_INFO_EX control code, the value of this parameter is ignored.</summary>
         [MarshalAs(UnmanagedType.Bool)] public readonly bool RecognizedPartition;

         /// <summary>The number of hidden sectors to be allocated when the partition table is created.</summary>
         [MarshalAs(UnmanagedType.U4)] public readonly uint HiddenSectors;
      }
   }
}
