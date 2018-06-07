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

namespace Alphaleonis.Win32.Device
{
   internal static partial class NativeMethods
   {
      /// <summary>Contains GUID partition table (GPT) partition information.
      /// <para>Minimum supported client: Windows XP [desktop apps only]</para>
      /// <para>Minimum supported server: Windows Server 2003 [desktop apps only]</para>
      /// </summary>
      [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Unicode)]
      internal struct PARTITION_INFORMATION_GPT
      {
         /// <summary>A GUID that identifies the partition type.
         /// <remarks>Each partition type that the EFI specification supports is identified by its own GUID, which is published by the developer of the partition.</remarks>
         /// </summary>
         [FieldOffset(0)] public readonly Guid PartitionType;

         /// <summary>The GUID of the partition.</summary>
         [FieldOffset(16)] public readonly Guid PartitionId;

         /// <summary>The Extensible Firmware Interface (EFI) attributes of the partition.</summary>
         [FieldOffset(32)] [MarshalAs(UnmanagedType.U8)]
         public readonly EfiPartitionAttributes Attributes;

         /// <summary>A wide-character string that describes the partition.</summary>
         [FieldOffset(40)] [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 36)]
         public readonly string Name;
      }
   }
}
