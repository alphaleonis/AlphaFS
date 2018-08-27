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
      /// <summary>Used with the <see cref="T:Constant.IOCTL_STORAGE.QUERY_PROPERTY"/> control code to retrieve the storage adapter descriptor data for a device.</summary>
      /// <remarks>
      ///   Minimum supported client: Windows XP [desktop apps only]
      ///   Minimum supported server: Windows Server 2003 [desktop apps only]
      /// </remarks>
      [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
      internal struct STORAGE_ADAPTER_DESCRIPTOR
      {
         /// <summary>Contains the size of this structure, in bytes. The value of this member will change as members are added to the structure.</summary>
         [MarshalAs(UnmanagedType.U4)] public readonly uint Version;

         /// <summary>Specifies the total size of the data returned, in bytes. This may include data that follows this structure.</summary>
         [MarshalAs(UnmanagedType.U4)] public readonly uint Size;

         /// <summary>Specifies the maximum number of bytes the storage adapter can transfer in a single operation.</summary>
         [MarshalAs(UnmanagedType.U4)] public readonly uint MaximumTransferLength;

         /// <summary>Specifies the maximum number of discontinuous physical pages the storage adapter can manage in a single transfer (in other words, the extent of its scatter/gather support).</summary>
         [MarshalAs(UnmanagedType.U4)] public readonly uint MaximumPhysicalPages;

         /// <summary>Specifies the storage adapter's alignment requirements for transfers.</summary>
         /// <remarks>The alignment mask indicates alignment restrictions for buffers required by the storage adapter for transfer operations. Valid mask values are also restricted by characteristics of the memory managers on different versions of Windows.</remarks>
         /// <value>
         /// 0 - Buffers must be aligned on BYTE boundaries.
         /// 1 - Buffers must be aligned on WORD boundaries.
         /// 3 - Buffers must be aligned on DWORD32 boundaries.
         /// 7 - Buffers must be aligned on DWORD64 boundaries.
         /// </value>
         [MarshalAs(UnmanagedType.U4)] public readonly uint AlignmentMask;

         /// <summary>If this member is TRUE, the storage adapter uses programmed I/O (PIO) and requires the use of system-space virtual addresses mapped to physical memory for data buffers. When this member is FALSE, the storage adapter does not use PIO.</summary>
         [MarshalAs(UnmanagedType.U1)] public readonly bool AdapterUsesPio;

         /// <summary>If this member is TRUE, the storage adapter scans down for BIOS devices, that is, the storage adapter begins scanning with the highest device number rather than the lowest. When this member is FALSE, the storage adapter begins scanning with the lowest device number.</summary>
         /// <remarks>This member is reserved for legacy miniport drivers.</remarks>
         [MarshalAs(UnmanagedType.U1)] public readonly bool AdapterScansDown;

         /// <summary>If this member is TRUE, the storage adapter supports SCSI tagged queuing and/or per-logical-unit internal queues, or the non-SCSI equivalent. When this member is FALSE, the storage adapter neither supports SCSI-tagged queuing nor per-logical-unit internal queues.</summary>
         [MarshalAs(UnmanagedType.U1)] public readonly bool CommandQueueing;

         /// <summary>If this member is TRUE, the storage adapter supports synchronous transfers as a way of speeding up I/O. When this member is FALSE, the storage adapter does not support synchronous transfers as a way of speeding up I/O. </summary>
         [MarshalAs(UnmanagedType.U1)] public readonly bool AcceleratedTransfer;

         /// <summary>Specifies a value of type STORAGE_BUS_TYPE that indicates the type of the bus to which the device is connected.</summary>
         [MarshalAs(UnmanagedType.U1)] private readonly byte busType;

         /// <summary>Specifies the major version number, if any, of the storage adapter. </summary>
         [MarshalAs(UnmanagedType.U2)] public readonly ushort BusMajorVersion;

         /// <summary>Specifies the minor version number, if any, of the storage adapter.</summary>
         [MarshalAs(UnmanagedType.U2)] public readonly ushort BusMinorVersion;

         /// <summary>Specifies the SCSI request block (SRB) type used by the HBA.
         /// <para>SRB_TYPE_SCSI_REQUEST_BLOCK: The HBA uses SCSI request blocks.</para>
         /// <para>SRB_TYPE_STORAGE_REQUEST_BLOCK: The HBA uses extended SCSI request blocks.</para>
         /// </summary>
         [MarshalAs(UnmanagedType.U1)] private readonly byte SrbType;

         /// <summary>Specifies the address type of the HBA.
         /// <para>STORAGE_ADDRESS_TYPE_BTL8: The HBA uses 8-bit bus, target, and LUN addressing.</para>
         /// </summary>
         [MarshalAs(UnmanagedType.U1)] private readonly byte AddressType;

         /// <summary>Specifies the version number, if any, of the storage adapter.</summary>
         public Version BusVersion
         {
            get { return new Version(BusMajorVersion, BusMinorVersion); }
         }

         /// <summary>Specifies a value of type STORAGE_BUS_TYPE that indicates the type of the bus to which the device is connected.</summary>
         public STORAGE_BUS_TYPE BusType
         {
            get { return (STORAGE_BUS_TYPE) busType; }
         }
      }
   }
}
