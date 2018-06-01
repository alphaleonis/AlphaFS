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
      /// <summary>Used in conjunction with the IOCTL_STORAGE_QUERY_PROPERTY control code to retrieve the storage device descriptor data for a device.</summary>
      [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
      internal struct STORAGE_DEVICE_DESCRIPTOR
      {
         /// <summary>Contains the size of this structure, in bytes. The value of this member will change as members are added to the structure.</summary>
         [MarshalAs(UnmanagedType.U4)] public readonly uint Version;

         /// <summary>Specifies the total size of the descriptor, in bytes, which may include vendor ID, product ID, product revision, device serial number strings and bus-specific data which are appended to the structure.</summary>
         [MarshalAs(UnmanagedType.U4)] public readonly uint Size;

         /// <summary>Specifies the device type as defined by the Small Computer Systems Interface (SCSI) specification.</summary>
         [MarshalAs(UnmanagedType.I1)] public readonly byte DeviceType;

         /// <summary>Specifies the device type modifier, if any, as defined by the SCSI specification. If no device type modifier exists, this member is zero.</summary>
         [MarshalAs(UnmanagedType.I1)] public readonly byte DeviceTypeModifier;

         /// <summary>Indicates when TRUE that the device's media (if any) is removable. If the device has no media, this member should be ignored. When FALSE the device's media is not removable.</summary>
         [MarshalAs(UnmanagedType.I1)] public readonly bool RemovableMedia;

         /// <summary>Indicates when TRUE that the device supports multiple outstanding commands (SCSI tagged queuing or equivalent). When FALSE, the device does not support SCSI-tagged queuing or the equivalent.</summary>
         [MarshalAs(UnmanagedType.I1)] public readonly bool CommandQueueing;

         /// <summary>Specifies the byte offset from the beginning of the structure to a null-terminated ASCII string that contains the device's vendor ID. If the device has no vendor ID, this member is zero.</summary>
         [MarshalAs(UnmanagedType.U4)] public readonly uint VendorIdOffset;

         /// <summary>Specifies the byte offset from the beginning of the structure to a null-terminated ASCII string that contains the device's product ID. If the device has no product ID, this member is zero.</summary>
         [MarshalAs(UnmanagedType.U4)] public readonly uint ProductIdOffset;

         /// <summary>Specifies the byte offset from the beginning of the structure to a null-terminated ASCII string that contains the device's product revision string. If the device has no product revision string, this member is zero.</summary>
         [MarshalAs(UnmanagedType.U4)] public readonly uint ProductRevisionOffset;

         /// <summary>Specifies the byte offset from the beginning of the structure to a null-terminated ASCII string that contains the device's serial number. If the device has no serial number, this member is zero.</summary>
         [MarshalAs(UnmanagedType.U4)] public readonly uint SerialNumberOffset;

         /// <summary>Specifies an enumerator value of type STORAGE_BUS_TYPE that indicates the type of bus to which the device is connected. This should be used to interpret the raw device properties at the end of this structure (if any).</summary>
         [MarshalAs(UnmanagedType.U4)] public readonly STORAGE_BUS_TYPE BusType;

         /// <summary>Indicates the number of bytes of bus-specific data that have been appended to this descriptor.</summary>
         [MarshalAs(UnmanagedType.U4)] public readonly uint RawPropertiesLength;

         /// <summary>Contains an array of length one that serves as a place holder for the first byte of the bus specific property data.</summary>
         [MarshalAs(UnmanagedType.ByValArray, SizeConst = 22)] public readonly byte[] RawDeviceProperties;
      }
   }
}
