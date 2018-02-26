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
      /// <summary>
      /// The DISK_INT13_INFO structure is used by the BIOS to report disk detection data for a partition with an INT13 format.
      /// </summary>
      /// <remarks>MSDN: http://msdn.microsoft.com/en-us/library/windows/hardware/ff552624(v=vs.85).aspx </remarks>
      [StructLayout(LayoutKind.Sequential)]
      internal struct DISK_INT13_INFO
      {
         /// <summary>
         /// Corresponds to the Device/Head register defined in the AT Attachment (ATA) specification. When zero, the fourth bit of this register 
         /// indicates that drive zero is selected. When 1, it indicates that drive one is selected. The values of bits 0, 1, 2, 3, and 6 depend 
         /// on the command in the command register. Bits 5 and 7 are no longer used. For more information about the values that the Device/Head 
         /// register can hold, see the ATA specification.
         /// </summary>
         public ushort DriveSelect;

         /// <summary>
         /// Indicates the maximum number of cylinders on the disk.
         /// </summary>
         public ulong MaxCylinders;

         /// <summary>
         /// Indicates the number of sectors per track.
         /// </summary>
         public ushort SectorsPerTrack;

         /// <summary>
         /// Indicates the maximum number of disk heads.
         /// </summary>
         public ushort MaxHeads;

         /// <summary>
         /// Indicates the number of drives.
         /// </summary>
         public ushort NumberDrives;
      }
   }
}
