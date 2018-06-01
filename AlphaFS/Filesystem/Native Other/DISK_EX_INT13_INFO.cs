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

namespace Alphaleonis.Win32.Filesystem
{
   internal static partial class NativeMethods
   {
      /// <summary>Contains extended Int13 drive parameters.
      /// <para>Minimum supported client: Windows XP [desktop apps only]</para>
      /// <para>Minimum supported server: Windows Server 2003 [desktop apps only]</para>
      /// </summary>
      [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
      internal struct DISK_EX_INT13_INFO
      {
         /// <summary>The size of the extended drive parameter buffer for this partition or disk. For valid values, see the BIOS documentation.</summary>
         [MarshalAs(UnmanagedType.U2)] public readonly ushort ExBufferSize;

         /// <summary>The information flags for this partition or disk. For valid values, see the BIOS documentation.</summary>
         [MarshalAs(UnmanagedType.U2)] public readonly ushort ExFlags;

         /// <summary>The number of cylinders per head. For valid values, see the BIOS documentation.</summary>
         [MarshalAs(UnmanagedType.U4)] public readonly uint ExCylinders;

         /// <summary>The maximum number of heads for this hard disk. For valid values, see the BIOS documentation.</summary>
         [MarshalAs(UnmanagedType.U4)] public readonly uint ExHeads;

         /// <summary>The number of sectors per track. For valid values, see the BIOS documentation.</summary>
         [MarshalAs(UnmanagedType.U4)] public readonly uint ExSectorsPerTrack;

         /// <summary>The total number of sectors for this disk. For valid values, see the BIOS documentation.</summary>
         [MarshalAs(UnmanagedType.U8)] public readonly ulong ExSectorsPerDrive;

         /// <summary>The sector size for this disk. For valid values, see the BIOS documentation.</summary>
         [MarshalAs(UnmanagedType.U2)] public readonly ushort ExSectorSize;

         /// <summary>Reserved for future use.</summary>
         [MarshalAs(UnmanagedType.U2)] public readonly ushort ExReserved;
      }
   }
}
