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
      /// <summary>The following table identifies the valid partition types that are used by disk drivers.</summary>
      /// <remarks>
      /// <para>Minimum supported client: Windows XP [desktop apps only]</para>
      /// <para>Minimum supported server: Windows Server 2003 [desktop apps only]</para>
      /// </remarks>
      internal enum DiskPartitionTypes
      {
         /// <summary>An unused entry partition.</summary>
         PARTITION_ENTRY_UNUSED = 0,

         /// <summary>A FAT12 file system partition.</summary>
         PARTITION_FAT_12 = 1,

         PARTITION_XENIX_1 = 2,
         PARTITION_XENIX_2 = 3,

         /// <summary>A FAT16 file system partition.</summary>
         PARTITION_FAT_16 = 4,

         /// <summary>An extended partition.</summary>
         PARTITION_EXTENDED = 5,

         PARTITION_HUGE = 6,

         /// <summary>An IFS partition.</summary>
         PARTITION_IFS = 7,

         /// <summary>A FAT32 file system partition.</summary>
         PARTITION_FAT32 = 11,

         PARTITION_FAT32_XINT13 = 12,
         PARTITION_XINT13 = 14,
         PARTITION_XINT13_EXTENDED = 15,
         PARTITION_PREP = 65,

         /// <summary>A logical disk manager (LDM) partition.</summary>
         PARTITION_LDM = 66,

         PARTITION_UNIX = 99,
         
         /// <summary>An NTFT partition.</summary>
         PARTITION_NTFT = 128,
      }
   }
}
