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
      /// <summary>
      /// The STORAGE_DEVICE_NUMBER structure is used in conjunction with the IOCTL_STORAGE_GET_DEVICE_NUMBER request to retrieve the FILE_DEVICE_XXX device type,
      /// the device number, and, for a device that can be partitioned, the partition number assigned to a device by the driver when the device is started.
      /// <para>The values in the STORAGE_DEVICE_NUMBER structure are guaranteed to remain unchanged until the device is removed or the system is restarted.</para>
      /// <para>It is not guaranteed to be persistent across device restarts or system restarts.</para>
      /// </summary>
      [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
      internal struct STORAGE_DEVICE_NUMBER
      {
         /// <summary>Specifies one of the system-defined FILE_DEVICE_XXX constants indicating the type of device (such as FILE_DEVICE_DISK, FILE_DEVICE_KEYBOARD, and so forth) or a vendor-defined value for a new type of device.</summary>
         [MarshalAs(UnmanagedType.U4)] internal readonly StorageDeviceType DeviceType;

         /// <summary>Indicates the number of this device. This value is set to 0xFFFFFFFF (-1) for the disks that represent the physical paths of an MPIO disk.</summary>
         [MarshalAs(UnmanagedType.U4)]
         internal readonly int DeviceNumber; // 2018-02-28: On MSDN this is defined as ULONG.

         /// <summary>Indicates the partition number of the device is returned in this member, if the device can be partitioned. Otherwise, -1 is returned.</summary>
         [MarshalAs(UnmanagedType.U4)]
         internal readonly int PartitionNumber; // 2018-02-28: On MSDN this is defined as ULONG.
      }
   }
}
