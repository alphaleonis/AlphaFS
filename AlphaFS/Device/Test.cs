///*  Copyright (C) 2008-2017 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
// *  
// *  Permission is hereby granted, free of charge, to any person obtaining a copy 
// *  of this software and associated documentation files (the "Software"), to deal 
// *  in the Software without restriction, including without limitation the rights 
// *  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell 
// *  copies of the Software, and to permit persons to whom the Software is 
// *  furnished to do so, subject to the following conditions:
// *  
// *  The above copyright notice and this permission notice shall be included in 
// *  all copies or substantial portions of the Software.
// *  
// *  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
// *  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
// *  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
// *  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
// *  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
// *  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN 
// *  THE SOFTWARE. 
// */

//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.IO;
//using System.Runtime.InteropServices;
//using System.Security.Cryptography;
//using System.Text;
//using Microsoft.Win32;
//using Microsoft.Win32.SafeHandles;

//namespace Alphaleonis.Win32.Filesystem
//{
//   #region Helper: Enum

//   internal static class EnumHelper
//   {
//      internal static bool HasFlag<T>(this Enum flags, T value) where T : struct
//      {
//         var iFlags = Convert.ToUInt64(flags);
//         var iValue = Convert.ToUInt64(value);
//         return ((iFlags & iValue) == iValue);
//      }

//      internal static T SetFlag<T>(this Enum flags, T value, bool state = true)
//      {
//         if (!Enum.IsDefined(typeof(T), value)) throw new ArgumentException("Enum value and flags types don't match.");
//         if (state) return (T) Enum.ToObject(typeof(T), Convert.ToUInt64(flags) | Convert.ToUInt64(value));
//         return (T) Enum.ToObject(typeof(T), Convert.ToUInt64(flags) & ~Convert.ToUInt64(value));
//      }
//   }

//   #endregion

//   #region Helper: Byte[]

//   internal static class ByteHelper
//   {
//      internal static int IndexOf(this byte[] bytes, byte value, int startIndex = 0)
//      {
//         while (startIndex < bytes.Length)
//            if (bytes[startIndex++] == value)
//               return startIndex - 1;
//         return -1;
//      }

//      internal static string GetString(this byte[] bytes, int offset)
//      {
//         string result = null;
//         var index = -1;
//         index = bytes.IndexOf(0, offset);
//         if (index >= 0) result = Encoding.ASCII.GetString(bytes, offset, index - offset);
//         return result;
//      }

//      internal static T ToStruct<T>(this byte[] bytes, int start = 0) where T : struct
//      {
//         var result = new T();
//         var size = Marshal.SizeOf(result);
//         var ptr = Marshal.AllocHGlobal(size);
//         Marshal.Copy(bytes, start, ptr, size);
//         result = (T) Marshal.PtrToStructure(ptr, result.GetType());
//         Marshal.FreeHGlobal(ptr);
//         return result;
//      }
//   }

//   #endregion

//   internal class Hardware
//   {
//      #region [extern] WindowsAPI

//      [DllImport("USER32.DLL", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
//      private static extern IntPtr CallWindowProcW([In] byte[] bytes, IntPtr hWnd, int msg, [In, Out] byte[] wParam,
//         IntPtr lParam);

//      [DllImport("KERNEL32.DLL", CharSet = CharSet.Unicode, SetLastError = true)]
//      private static extern bool VirtualProtect([In] byte[] bytes, IntPtr size, int newProtect, out int oldProtect);

//      [DllImport("KERNEL32.DLL", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
//      private static extern SafeFileHandle CreateFileW([MarshalAs(UnmanagedType.LPWStr)] string lpFileName,
//         uint dwDesiredAccess, uint dwShareMode, IntPtr lpSecurityAttributes, uint dwCreationDisposition,
//         uint dwFlagsAndAttributes, IntPtr hTemplateFile);

//      [DllImport("KERNEL32.DLL", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto,
//         SetLastError = true)]
//      private static extern bool GetVolumeNameForVolumeMountPoint(string mountPoint, StringBuilder name,
//         uint bufferLength);

//      [DllImport("KERNEL32.DLL", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
//      private static extern bool DeviceIoControl(SafeFileHandle hHandle, uint dwIoControlCode,
//         STORAGE_PROPERTY_QUERY lpInBuffer, int lpInBufferSize, ref STORAGE_DEVICE_ID_DESCRIPTOR lpOutBuffer,
//         int lpOutBufferSize, ref uint lpBytesReturned, int lpOverlapped);

//      [DllImport("KERNEL32.DLL", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
//      private static extern bool DeviceIoControl(SafeFileHandle hHandle, uint dwIoControlCode,
//         STORAGE_PROPERTY_QUERY lpInBuffer, int lpInBufferSize, ref STORAGE_ADAPTER_DESCRIPTOR lpOutBuffer,
//         int lpOutBufferSize, ref uint lpBytesReturned, int lpOverlapped);

//      [DllImport("KERNEL32.DLL", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
//      private static extern bool DeviceIoControl(SafeFileHandle hHandle, uint dwIoControlCode,
//         STORAGE_PROPERTY_QUERY lpInBuffer, int lpInBufferSize, ref STORAGE_DEVICE_DESCRIPTOR lpOutBuffer,
//         int lpOutBufferSize, ref uint lpBytesReturned, int lpOverlapped);

//      [DllImport("KERNEL32.DLL", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
//      private static extern bool DeviceIoControl(SafeFileHandle hHandle, uint dwIoControlCode,
//         SENDCMDINPARAMS lpInBuffer, int lpInBufferSize, ref SENDCMDOUTPARAMS lpOutBuffer, int lpOutBufferSize,
//         ref uint lpBytesReturned, int lpOverlapped);

//      [DllImport("KERNEL32.DLL", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
//      private static extern bool DeviceIoControl(SafeFileHandle hHandle, uint dwIoControlCode, IntPtr lpInBuffer,
//         int lpInBufferSize, ref GETVERSIONOUTPARAMS lpOutBuffer, int lpOutBufferSize, ref uint lpBytesReturned,
//         int lpOverlapped);

//      [DllImport("KERNEL32.DLL", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
//      private static extern bool DeviceIoControl(SafeFileHandle hHandle, uint dwIoControlCode, IntPtr lpInBuffer,
//         int lpInBufferSize, ref VOLUMEDISKEXTENTS lpOutBuffer, int lpOutBufferSize, ref uint lpBytesReturned,
//         int lpOverlapped);

//      [DllImport("KERNEL32.DLL", CallingConvention = CallingConvention.StdCall)]
//      private static extern int CloseHandle(int hObject);

//      [DllImport("KERNEL32.DLL", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
//      internal static extern int EnumSystemFirmwareTables(BiosFirmwareTableProvider providerSignature,
//         IntPtr firmwareTableBuffer, int bufferSize);

//      [DllImport("KERNEL32.DLL", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
//      private static extern int GetSystemFirmwareTable(BiosFirmwareTableProvider providerSignature,
//         int dwFirmwareTableID, IntPtr lpTableBuffer, int dwBufferSize);

//      [DllImport("USER32.DLL", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
//      private static extern bool EnumDisplayDevices(string lpDevice, uint iDevNum, ref DISPLAY_DEVICE lpDisplayDevice,
//         uint dwFlags);

//      #endregion

//      private static byte[] _MD5;
//      private static byte[] _SHA1;
//      private static byte[] _SHA256;
//      private static byte[] _SHA512;

//      private static string GenerateDate = DateTime.Now.ToString("yyyyMMddHH");

//      // HDD //

//      #region [constants] HDD

//      private const int VER_PLATFORM_WIN32_NT = 2;
//      private const int IDENTIFY_BUFFER_SIZE = 512;
//      private const int CREATE_NEW = 0x1;
//      private const int OPEN_EXISTING = 0x3;
//      private const uint GENERIC_READ = 0x80000000;
//      private const uint GENERIC_WRITE = 0x40000000;
//      private const int FILE_SHARE_READ = 0x00000001;
//      private const int FILE_SHARE_WRITE = 0x00000002;
//      private const uint IDE_ATAPI_IDENTIFY = 0xA1;
//      private const uint IDE_ATA_IDENTIFY = 0xEC;
//      private const uint CAP_SMART_CMD = 0x04;
//      private const uint METHOD_BUFFERED = 0;
//      private const uint FILE_ANY_ACCESS = 0;
//      private const uint FILE_ATTRIBUTE_NORMAL = 0x00000080;
//      private const uint FILE_DEVICE_CONTROLLER = 0x00000004;
//      private const uint IOCTL_SCSI_BASE = FILE_DEVICE_CONTROLLER;
//      private const uint FILE_DEVICE_MASS_STORAGE = 0x0000002d;
//      private const uint IOCTL_STORAGE_BASE = FILE_DEVICE_MASS_STORAGE;
//      private const uint IOCTL_VOLUME_BASE = 0x00000056;
//      private const uint IOCTL_SCSI_PASS_THROUGH = 0x0004d004;
//      private const uint IOCTL_DISK_GET_DRIVE_GEOMETRY = 0x00070000;
//      private const uint IOCTL_DISK_GET_DRIVE_GEOMETRY_EX = 0x000700a0;
//      private const uint DFP_GET_VERSION = 0x00074080;
//      private const uint DFP_SEND_DRIVE_COMMAND = 0x0007c084;
//      private const uint DFP_RECEIVE_DRIVE_DATA = 0x0007c088;
//      private const uint IOCTL_STORAGE_GET_DEVICE_NUMBER = 0x002d1080;
//      private const uint IOCTL_STORAGE_QUERY_PROPERTY = 0x002d1400;

//      private const uint IOCTL_VOLUME_GET_VOLUME_DISK_EXTENTS = 0x00560000;

//      // PropertyId For DeviceIoControl //
//      private const uint StorageDeviceProperty = 0;
//      private const uint StorageAdapterProperty = 1;
//      private const uint StorageDeviceIdProperty = 2;

//      private const uint StorageDeviceSeekPenaltyProperty = 7;

//      // Query Type For DeviceIoControl //
//      private const uint PropertyStandardQuery = 0;
//      private const uint PropertyExistsQuery = 1;

//      #endregion

//      #region [enum] STORAGE_BUS_TYPE

//      internal enum STORAGE_BUS_TYPE : byte
//      {
//         BusTypeUnknown = 0x00,
//         BusTypeScsi = 0x01,
//         BusTypeAtapi = 0x02,
//         BusTypeAta = 0x03,
//         BusType1394 = 0x04,
//         BusTypeSsa = 0x05,
//         BusTypeFibre = 0x06,
//         BusTypeUsb = 0x07,
//         BusTypeRAID = 0x08,
//         BusTypeiScsi = 0x09,
//         BusTypeSas = 0x0A,
//         BusTypeSata = 0x0B,
//         BusTypeSd = 0x0C,
//         BusTypeMmc = 0x0D,
//         BusTypeVirtual = 0x0E,
//         BusTypeFileBackedVirtual = 0x0F,
//         BusTypeMax = 0x10,
//         BusTypeMaxReserved = 0x7F
//      }

//      #endregion

//      #region [struct] VOLUMEDISKEXTENTS

//      [StructLayout(LayoutKind.Explicit)]
//      internal struct VOLUMEDISKEXTENTS
//      {
//         [FieldOffset(0)] internal uint numberOfDiskExtents;
//         [FieldOffset(8)] internal uint diskNumber;
//         [FieldOffset(16)] internal long startingOffset;
//         [FieldOffset(24)] internal long extentLength;
//      }

//      #endregion

//      #region [struct] STORAGE_PROPERTY_QUERY

//      [StructLayout(LayoutKind.Sequential)]
//      private struct STORAGE_PROPERTY_QUERY
//      {
//         internal uint PropertyId;
//         internal uint QueryType;

//         [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
//         internal byte[] AdditionalParameters;
//      }

//      #endregion

//      #region [struct] STORAGE_DEVICE_DESCRIPTOR

//      [StructLayout(LayoutKind.Sequential)]
//      internal struct STORAGE_DEVICE_DESCRIPTOR
//      {
//         internal int Version;
//         internal int Size;
//         internal byte DeviceType;
//         internal byte DeviceTypeModifier;
//         internal byte RemovableMedia;
//         internal byte CommandQueueing;
//         internal int VendorIdOffset;
//         internal int ProductIdOffset;
//         internal int ProductRevisionOffset;
//         internal int SerialNumberOffset;
//         internal STORAGE_BUS_TYPE BusType;
//         internal int RawPropertiesLength;

//         [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10240)]
//         internal byte[] RawDeviceProperties;
//      }

//      #endregion

//      #region [struct] STORAGE_DEVICE_ID_DESCRIPTOR

//      [StructLayout(LayoutKind.Sequential)]
//      internal struct STORAGE_DEVICE_ID_DESCRIPTOR
//      {
//         internal int Version;
//         internal int Size;
//         internal int NumberOfIdentifiers;

//         [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10240)]
//         internal byte[] Identifiers;
//      }

//      #endregion

//      #region [struct] STORAGE_ADAPTER_DESCRIPTOR

//      [StructLayout(LayoutKind.Sequential)]
//      internal struct STORAGE_ADAPTER_DESCRIPTOR
//      {
//         internal uint Version;
//         internal uint Size;
//         internal uint MaximumTransferLength;
//         internal uint MaximumPhysicalPages;
//         internal uint AlignmentMask;
//         internal byte AdapterUsesPio;
//         internal byte AdapterScansDown;
//         internal byte CommandQueueing;
//         internal byte AcceleratedTransfer;
//         internal STORAGE_BUS_TYPE BusType;
//         internal ushort BusMajorVersion;
//         internal ushort BusMinorVersion;
//         internal byte SrbType;
//         internal byte AddressType;
//      }

//      #endregion

//      #region [struct] GETVERSIONOUTPARAMS

//      [StructLayout(LayoutKind.Sequential)]
//      internal struct GETVERSIONOUTPARAMS
//      {
//         internal byte bVersion;
//         internal byte bRevision;
//         internal byte bReserved;
//         internal byte bIDEDeviceMap;
//         internal int fCapabilities;

//         [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
//         internal int[] dwReserved;
//      }

//      #endregion

//      #region [struct] IDEREGS

//      [StructLayout(LayoutKind.Sequential, Size = 8)]
//      internal struct IDEREGS
//      {
//         internal byte Features;
//         internal byte SectorCount;
//         internal byte SectorNumber;
//         internal byte CylinderLow;
//         internal byte CylinderHigh;
//         internal byte DriveHead;
//         internal byte Command;
//         internal byte Reserved;
//      }

//      #endregion

//      #region [struct] SENDCMDINPARAMS

//      [StructLayout(LayoutKind.Sequential, Size = 32)]
//      internal struct SENDCMDINPARAMS
//      {
//         internal int BufferSize;
//         internal IDEREGS DriveRegs;
//         internal byte DriveNumber;

//         [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
//         internal byte[] bReserved;

//         [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
//         internal int[] dwReserved;
//      }

//      #endregion

//      #region [struct] DRIVERSTATUS

//      [StructLayout(LayoutKind.Sequential, Size = 12)]
//      internal struct DRIVERSTATUS
//      {
//         internal byte DriveError;
//         internal byte IDEStatus;

//         [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
//         internal byte[] bReserved;

//         [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
//         internal int[] dwReserved;
//      }

//      #endregion

//      #region [struct] IDSECTOR

//      [StructLayout(LayoutKind.Sequential)]
//      internal struct IDSECTOR
//      {
//         internal short GenConfig;
//         internal short NumberCylinders;
//         internal short Reserved;
//         internal short NumberHeads;
//         internal short BytesPerTrack;
//         internal short BytesPerSector;
//         internal short SectorsPerTrack;

//         [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
//         internal short[] VendorUnique;

//         [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
//         internal byte[] SerialNumber;

//         internal short BufferClass;
//         internal short BufferSize;
//         internal short ECCSize;

//         [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
//         internal byte[] FirmwareRevision;

//         [MarshalAs(UnmanagedType.ByValArray, SizeConst = 40)]
//         internal byte[] ModelNumber;

//         internal short MoreVendorUnique;
//         internal short DoubleWordIO;
//         internal short Capabilities;
//         internal short Reserved1;
//         internal short PIOTiming;
//         internal short DMATiming;
//         internal short BS;
//         internal short NumberCurrentCyls;
//         internal short NumberCurrentHeads;
//         internal short NumberCurrentSectorsPerTrack;
//         internal int CurrentSectorCapacity;
//         internal short MultipleSectorCapacity;
//         internal short MultipleSectorStuff;
//         internal int TotalAddressableSectors;
//         internal short SingleWordDMA;
//         internal short MultiWordDMA;

//         [MarshalAs(UnmanagedType.ByValArray, SizeConst = 382)]
//         internal byte[] bReserved;
//      }

//      #endregion

//      #region [struct] SENDCMDOUTPARAMS

//      [StructLayout(LayoutKind.Sequential)]
//      internal struct SENDCMDOUTPARAMS
//      {
//         internal uint cBufferSize;
//         internal DRIVERSTATUS Status;
//         internal IDSECTOR IDS;
//      }

//      #endregion

//      [StructLayout(LayoutKind.Sequential)]
//      internal struct PhysicalDisk
//      {
//         internal byte Number;
//         internal string Model;
//         internal string Firmware;
//         internal string SerialNumber;
//         internal bool RemovableMedia;
//         internal VOLUMEDISKEXTENTS Extents;
//         internal STORAGE_DEVICE_DESCRIPTOR Device;
//         internal STORAGE_DEVICE_ID_DESCRIPTOR DeviceID;
//         internal STORAGE_ADAPTER_DESCRIPTOR Adapter;
//         internal GETVERSIONOUTPARAMS Version;
//         internal SENDCMDOUTPARAMS Params;
//      }

//      #region CTL_CODE

//      private uint CTL_CODE(uint DeviceType, uint Function, uint Method, uint Access)
//      {
//         return ((DeviceType << 16) | (Access << 14) | (Function << 2) | Method);
//      }

//      #endregion

//      #region [private] SwapChars

//      private string SwapChars(char[] chars)
//      {
//         for (var i = 0; i <= chars.Length - 2; i += 2)
//         {
//            char T;
//            T = chars[i];
//            chars[i] = chars[i + 1];
//            chars[i + 1] = T;
//         }

//         return new string(chars);
//      }

//      #endregion

//      #region [private] SwapBytes

//      private string SwapBytes(byte[] bytes)
//      {
//         for (var i = 0; i <= bytes.Length - 2; i += 2)
//         {
//            byte T;
//            T = bytes[i];
//            bytes[i] = bytes[i + 1];
//            bytes[i + 1] = T;
//         }

//         return Encoding.ASCII.GetString(bytes);
//      }

//      #endregion

//      #region [private] SwapRawString

//      private string SwapRawString(string hexString)
//      {
//         var Result = new StringBuilder();
//         for (var i = 0; i < hexString.Length; i += 4)
//         {
//            Result.Append(Convert.ToChar(Convert.ToByte(hexString.Substring(i + 2, 2), 16)));
//            Result.Append(Convert.ToChar(Convert.ToByte(hexString.Substring(i + 0, 2), 16)));
//         }

//         return Result.ToString().Trim('\0', ' ');
//      }

//      #endregion

//      #region [private] GetPhysicalDisk

//      private PhysicalDisk GetPhysicalDisk(string path)
//      {
//         var drive = System.IO.Path.GetPathRoot(path).TrimEnd('\\');
         
//         using (var hDisk = CreateFileW(@"\\.\" + drive, GENERIC_READ | GENERIC_WRITE, FILE_SHARE_READ | FILE_SHARE_WRITE, IntPtr.Zero, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, IntPtr.Zero))
//         {
//            var Result = new PhysicalDisk();
//            uint iBytesReturned = 0;
//            Result.SerialNumber = string.Empty;
//            Result.Firmware = string.Empty;
//            Result.Model = string.Empty;

//            //var StoragePropertyQuery = new STORAGE_PROPERTY_QUERY();

//            Result.Extents = new VOLUMEDISKEXTENTS();
//            Result.Adapter = new STORAGE_ADAPTER_DESCRIPTOR();
//            Result.Device = new STORAGE_DEVICE_DESCRIPTOR();
//            Result.DeviceID = new STORAGE_DEVICE_ID_DESCRIPTOR();
//            Result.Version = new GETVERSIONOUTPARAMS();
//            Result.Params = new SENDCMDOUTPARAMS();

//            var IOCMD = CTL_CODE(IOCTL_VOLUME_BASE, 0, METHOD_BUFFERED, FILE_ANY_ACCESS);
//            if (DeviceIoControl(hDisk, IOCMD, IntPtr.Zero, 0, ref Result.Extents, Marshal.SizeOf(Result.Extents), ref iBytesReturned, 0))
//            {
//               Console.ForegroundColor = ConsoleColor.DarkGray; 
//               Console.WriteLine("Received: IOCTL_VOLUME_BASE");
//               Result.Number = Convert.ToByte(Result.Extents.diskNumber);
//            }



//            //StoragePropertyQuery.PropertyId = StorageAdapterProperty;
//            //StoragePropertyQuery.QueryType = PropertyStandardQuery;

//            //if (DeviceIoControl(hDisk, IOCTL_STORAGE_QUERY_PROPERTY, StoragePropertyQuery, Marshal.SizeOf(StoragePropertyQuery), ref Result.Adapter, Marshal.SizeOf(Result.Adapter), ref iBytesReturned, 0))
//            //{
//            //   Console.ForegroundColor = ConsoleColor.DarkGray;
//            //   Console.WriteLine("Received: STORAGE_ADAPTER_DESCRIPTOR");
//            //   Console.WriteLine("> Version: "+Result.Adapter.Version+", Size: "+Result.Adapter.Size);
//            //   Console.WriteLine("> MaximumPhysicalPages: "+Result.Adapter.MaximumPhysicalPages);
//            //   Console.WriteLine("> MaximumTransferLength: "+Result.Adapter.MaximumTransferLength);
//            //   Console.WriteLine("> CommandQueueing: "+Result.Adapter.CommandQueueing);
//            //   Console.WriteLine("> BusMajorVersion: "+Result.Adapter.BusMajorVersion);
//            //   Console.WriteLine("> BusMinorVersion: "+Result.Adapter.BusMinorVersion);
//            //   Console.WriteLine("> BusType: "+Result.Adapter.BusType);
//            //}

            
            
//            //StoragePropertyQuery.PropertyId = StorageDeviceIdProperty;
//            //StoragePropertyQuery.QueryType = PropertyStandardQuery;
//            //if (DeviceIoControl(hDisk, IOCTL_STORAGE_QUERY_PROPERTY, StoragePropertyQuery, Marshal.SizeOf(StoragePropertyQuery), ref Result.DeviceID, Marshal.SizeOf(Result.DeviceID), ref iBytesReturned, 0))
//            //{
//            //   /File.WriteAllBytes("Hardware."+GenerateDate+".RAID-"+drive[0]+".bin", Result.DeviceID.Identifiers);
//            //   Console.ForegroundColor = ConsoleColor.DarkGray; 
//            //   Console.WriteLine("Received: STORAGE_DEVICE_ID_DESCRIPTOR");
//            //   Console.WriteLine("> Version: "+Result.DeviceID.Version+", Size: "+Result.DeviceID.Size);
//            //   Console.WriteLine("> NumberOfIdentifiers: "+Result.DeviceID.NumberOfIdentifiers);                        
//            //}

            
            
//            //StoragePropertyQuery.PropertyId = StorageDeviceProperty;
//            //StoragePropertyQuery.QueryType = PropertyStandardQuery;
//            //if (DeviceIoControl(hDisk, IOCTL_STORAGE_QUERY_PROPERTY, StoragePropertyQuery, Marshal.SizeOf(StoragePropertyQuery), ref Result.Device, Marshal.SizeOf(Result.Device), ref iBytesReturned, 0))
//            //{
//            //   /File.WriteAllBytes("Hardware."+GenerateDate+".HDD-"+drive[0]+".bin", Result.Device.RawDeviceProperties);
//            //   Console.ForegroundColor = ConsoleColor.DarkGray; 
//            //   Console.WriteLine("Received: STORAGE_DEVICE_DESCRIPTOR");
//            //   Console.WriteLine("> RemovableMedia: "+Convert.ToBoolean(Result.Device.RemovableMedia));
//            //   Console.WriteLine("> DeviceType: "+Result.Device.DeviceType);                    
//            //   Console.WriteLine("> BusType: "+Result.Device.BusType);                    

//            //   var buffer = Encoding.ASCII.GetString(Result.Device.RawDeviceProperties);
//            //   var basepos = Marshal.SizeOf(Result.Device) - Result.Device.RawDeviceProperties.Length;

//            //   if (Result.Device.ProductIdOffset > 0)
//            //      Result.Model = buffer.Substring(Result.Device.ProductIdOffset - basepos, 20).Trim('\0', ' ');
//            //   if (Result.Device.ProductRevisionOffset > 0)
//            //      Result.Firmware = buffer.Substring(Result.Device.ProductRevisionOffset - basepos, 8).Trim('\0', ' ');
//            //   if (Result.Device.SerialNumberOffset > 0)
//            //      Result.SerialNumber =
//            //         buffer.Substring(Result.Device.SerialNumberOffset - basepos, 40).Trim('\0', ' ');
//            //   if (Result.SerialNumber != null && Result.SerialNumber.Length == 40)
//            //      Result.SerialNumber = SwapRawString(Result.SerialNumber);

//            //   Result.RemovableMedia = Convert.ToBoolean(Result.Device.RemovableMedia);
//            //}

//            DeviceIoControl(hDisk, DFP_GET_VERSION, IntPtr.Zero, 0, ref Result.Version, Marshal.SizeOf(Result.Version), ref iBytesReturned, 0);
            
//            if ((Result.Version.fCapabilities & CAP_SMART_CMD) > 0)
//            {
//               Console.ForegroundColor = ConsoleColor.DarkGray;
//               Console.WriteLine("Received: GETVERSIONOUTPARAMS");
//               Console.WriteLine("> Version: "+Result.Version.bVersion+"."+Result.Version.bRevision);
//               var SCI = new SENDCMDINPARAMS
//               {
//                  DriveRegs = {Command = (int) IDE_ATA_IDENTIFY},
//                  DriveNumber = Result.Number,
//                  BufferSize = IDENTIFY_BUFFER_SIZE
//               };
               
//               if (DeviceIoControl(hDisk, DFP_RECEIVE_DRIVE_DATA, SCI, Marshal.SizeOf(SCI), ref Result.Params, Marshal.SizeOf(Result.Params), ref iBytesReturned, 0))
//               {
//                  Console.WriteLine("Received: SENDCMDOUTPARAMS");
//                  Console.WriteLine("> IDS.Capabilities: "+Result.Params.IDS.Capabilities);
//                  Console.WriteLine("> IDS.BufferSize: "+Result.Params.IDS.BufferSize);
//                  Console.WriteLine("> IDS.SectorsPerTrack: "+Result.Params.IDS.SectorsPerTrack);
//                  Console.WriteLine("> IDS.NumberCylinders: "+Result.Params.IDS.NumberCylinders);
//                  Console.WriteLine("> IDS.NumberHeads: "+Result.Params.IDS.NumberHeads);

//                  Result.Model = SwapBytes(Result.Params.IDS.ModelNumber).Trim();
//                  Result.Firmware = SwapBytes(Result.Params.IDS.FirmwareRevision).Trim();
//                  Result.SerialNumber = SwapBytes(Result.Params.IDS.SerialNumber).Trim();
//               }

//            }

//            return Result;
//         }

//         throw new Win32Exception();
//      }

//      #endregion

//      // CPU //

//      #region [enum] BiosFirmwareTableProvider

//      internal enum BiosFirmwareTableProvider
//      {
//         ACPI = (byte) 'A' << 24 | (byte) 'C' << 16 | (byte) 'P' << 8 | (byte) 'I',
//         FIRM = (byte) 'F' << 24 | (byte) 'I' << 16 | (byte) 'R' << 8 | (byte) 'M',
//         RSMB = (byte) 'R' << 24 | (byte) 'S' << 16 | (byte) 'M' << 8 | (byte) 'B'
//      }

//      #endregion

//      #region [enum] SMBIOSTableType

//      internal enum SMBIOSTableType : sbyte
//      {
//         BIOSInformation = 0,
//         SystemInformation = 1,
//         BaseBoardInformation = 2,
//         EnclosureInformation = 3,
//         ProcessorInformation = 4,
//         MemoryControllerInformation = 5,
//         MemoryModuleInformation = 6,
//         CacheInformation = 7,
//         PortConnectorInformation = 8,
//         SystemSlotsInformation = 9,
//         OnBoardDevicesInformation = 10,
//         OEMStrings = 11,
//         SystemConfigurationOptions = 12,
//         BIOSLanguageInformation = 13,
//         GroupAssociations = 14,
//         SystemEventLog = 15,
//         PhysicalMemoryArray = 16,
//         MemoryDevice = 17,
//         MemoryErrorInformation = 18,
//         MemoryArrayMappedAddress = 19,
//         MemoryDeviceMappedAddress = 20,
//         EndofTable = 127
//      }

//      #endregion

//      #region [enum] SMBIOSTableEntry

//      [StructLayout(LayoutKind.Sequential)]
//      internal struct SMBIOSTableHeader
//      {
//         internal SMBIOSTableType type;
//         internal byte length;
//         internal ushort Handle;
//      }

//      [StructLayout(LayoutKind.Sequential)]
//      internal struct SMBIOSTableEntry
//      {
//         internal SMBIOSTableHeader header;
//         internal uint index;
//      }

//      #endregion

//      #region [enum] SMBIOSTableInfo

//      [StructLayout(LayoutKind.Sequential)]
//      internal struct SMBIOSTableBiosInfo
//      {
//         internal SMBIOSTableHeader header;
//         internal byte vendor;
//         internal byte version;
//         internal ushort startingSegment;
//         internal byte releaseDate;
//         internal byte biosRomSize;
//         internal ulong characteristics;

//         [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
//         internal byte[] extensionBytes;
//      }

//      [StructLayout(LayoutKind.Sequential)]
//      internal struct SMBIOSTableSystemInfo
//      {
//         internal SMBIOSTableHeader header;
//         internal byte manufacturer;
//         internal byte productName;
//         internal byte version;
//         internal byte serialNumber;

//         [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
//         internal byte[] UUID;

//         internal byte wakeUpType;
//      }

//      [StructLayout(LayoutKind.Sequential)]
//      internal struct SMBIOSTableBaseBoardInfo
//      {
//         internal SMBIOSTableHeader header;
//         internal byte manufacturer;
//         internal byte productName;
//         internal byte version;
//         internal byte serialNumber;
//      }

//      [StructLayout(LayoutKind.Sequential)]
//      internal struct SMBIOSTableEnclosureInfo
//      {
//         internal SMBIOSTableHeader header;
//         internal byte manufacturer;
//         internal byte type;
//         internal byte version;
//         internal byte serialNumber;
//         internal byte assetTagNumber;
//         internal byte bootUpState;
//         internal byte powerSupplyState;
//         internal byte thermalState;
//         internal byte securityStatus;
//         internal long OEM_Defined;
//      }

//      [StructLayout(LayoutKind.Sequential)]
//      internal struct SMBIOSTableProcessorInfo
//      {
//         internal SMBIOSTableHeader header;
//         internal byte socketDesignation;
//         internal byte processorType;
//         internal byte processorFamily;
//         internal byte processorManufacturer;
//         internal ulong processorID;
//         internal byte processorVersion;
//         internal byte processorVoltage;
//         internal ushort externalClock;
//         internal ushort maxSpeed;
//         internal ushort currentSpeed;
//         internal byte status;
//         internal byte processorUpgrade;
//         internal ushort L1CacheHandler;
//         internal ushort L2CacheHandler;
//         internal ushort L3CacheHandler;
//         internal byte serialNumber;
//         internal byte assetTag;
//         internal byte partNumber;
//      }

//      [StructLayout(LayoutKind.Sequential)]
//      internal struct SMBIOSTableCacheInfo
//      {
//         internal SMBIOSTableHeader header;
//         internal byte socketDesignation;
//         internal long cacheConfiguration;
//         internal ushort maximumCacheSize;
//         internal ushort installedSize;
//         internal ushort supportedSRAMType;
//         internal ushort currentSRAMType;
//         internal byte cacheSpeed;
//         internal byte errorCorrectionType;
//         internal byte systemCacheType;
//         internal byte associativity;
//      }

//      #endregion

//      #region [struct] RawSMBIOSData

//      [StructLayout(LayoutKind.Sequential)]
//      internal struct RawSMBIOSData
//      {
//         internal byte Used20CallingMethod;
//         internal byte MajorVersion;
//         internal byte MinorVersion;
//         internal byte DmiRevision;
//         internal uint Length;
//         internal SMBIOSTableBiosInfo BiosInfo;
//         internal SMBIOSTableSystemInfo SystemInfo;
//         internal SMBIOSTableBaseBoardInfo BaseBoardInfo;
//         internal SMBIOSTableEnclosureInfo EnclosureInfo;
//         internal SMBIOSTableProcessorInfo ProcessorInfo;
//         internal SMBIOSTableCacheInfo CacheInfo;
//      }

//      #endregion

//      #region [private] GetTable

//      private static byte[] GetTable(BiosFirmwareTableProvider provider, string table)
//      {
//         var id = table[3] << 24 | table[2] << 16 | table[1] << 8 | table[0];
//         return GetTable(provider, id);
//      }

//      private static byte[] GetTable(BiosFirmwareTableProvider provider, int table)
//      {
//         var Result = new byte[0];
//         try
//         {
//            var sizeNeeded = GetSystemFirmwareTable(provider, table, IntPtr.Zero, 0);
//            if (sizeNeeded > 0)
//            {
//               var bufferPtr = Marshal.AllocHGlobal(sizeNeeded);
//               GetSystemFirmwareTable(provider, table, bufferPtr, sizeNeeded);
//               if (Marshal.GetLastWin32Error() == 0)
//               {
//                  Result = new byte[sizeNeeded];
//                  Marshal.Copy(bufferPtr, Result, 0, sizeNeeded);
//               }

//               Marshal.FreeHGlobal(bufferPtr);
//            }
//         }
//         catch
//         {
//         }

//         return Result;
//      }

//      #endregion

//      #region [private] EnumerateTables

//      private static string[] EnumerateTables(BiosFirmwareTableProvider provider)
//      {
//         var Result = new string[0];
//         try
//         {
//            var sizeNeeded = EnumSystemFirmwareTables(provider, IntPtr.Zero, 0);
//            if (sizeNeeded > 0)
//            {
//               var buffer = new byte[sizeNeeded];
//               var bufferPtr = Marshal.AllocHGlobal(sizeNeeded);
//               EnumSystemFirmwareTables(provider, bufferPtr, sizeNeeded);
//               if (Marshal.GetLastWin32Error() == 0)
//               {
//                  Result = new string[sizeNeeded / 4];
//                  Marshal.Copy(bufferPtr, buffer, 0, sizeNeeded);
//                  for (var i = 0; i < Result.Length; i++) Result[i] = Encoding.ASCII.GetString(buffer, 4 * i, 4);
//               }

//               Marshal.FreeHGlobal(bufferPtr);
//            }
//         }
//         catch
//         {
//         }

//         return Result;
//      }

//      #endregion

//      #region [private] GetRawStrings

//      private static string[] GetRawStrings(byte[] bytes, SMBIOSTableEntry entry, ref int pos)
//      {
//         var result = new List<string>();
//         pos += entry.header.length;
//         do
//         {
//            string s = bytes.GetString(pos);
//            result.Add(s);
//            pos += s.Length;
//         } while (bytes[++pos] != 0);

//         pos++;
//         return result.ToArray();
//      }

//      #endregion

//      // VIDEO //

//      #region [enum] DisplayDeviceStateFlags

//      internal enum DisplayDeviceStateFlags
//      {
//         AttachedToDesktop = 0x0000001,
//         MultiDriver = 0x0000002,
//         PrimaryDevice = 0x0000004,
//         MirroringDriver = 0x0000008,
//         VGACompatible = 0x0000010,
//         Removable = 0x0000020,
//         ModesPruned = 0x8000000,
//         Remote = 0x4000000,
//         Disconnect = 0x2000000
//      }

//      #endregion

//      #region [struct] DISPLAY_DEVICE

//      [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
//      internal struct DISPLAY_DEVICE
//      {
//         [MarshalAs(UnmanagedType.U4)] internal int structSize;

//         [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
//         internal string DeviceName;

//         [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
//         internal string DeviceString;

//         [MarshalAs(UnmanagedType.U4)] internal DisplayDeviceStateFlags StateFlags;

//         [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
//         internal string DeviceID;

//         [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
//         internal string DeviceKey;
//      }

//      #endregion

//      internal Hardware()
//      {
//         using (var Binary = new BinaryWriter(new MemoryStream()))
//         {
            
//            Console.WriteLine();
            
//            Console.ForegroundColor = ConsoleColor.White;
            
//            Console.WriteLine("[SMBIOS]");

//            // Get BIOS Information //
//            var RSMB_Tables = EnumerateTables(BiosFirmwareTableProvider.RSMB);
//            if (RSMB_Tables.Length > 0)
//            {
//               var RSMB = GetTable(BiosFirmwareTableProvider.RSMB, RSMB_Tables[0]);
//               if (RSMB.Length > 0)
//               {
                  
//                  File.WriteAllBytes("BIOS." + BitConverter.ToString(new MD5CryptoServiceProvider().ComputeHash(RSMB), 0).Replace("-", "") + ".RSMB.bin", RSMB);
                  
//                  Binary.Write(RSMB);

//                  var RawStrings = new string[0];
//                  RawSMBIOSData SMBIOS = RSMB.ToStruct<RawSMBIOSData>();

//                  if (SMBIOS.Length > 8)
//                  {
//                     var pos = 0x08;
//                     while (pos < SMBIOS.Length)
//                     {
                        
//                        Console.ForegroundColor = ConsoleColor.Green;
//                        SMBIOSTableEntry tableEntry = RSMB.ToStruct<SMBIOSTableEntry>(pos);
//                        switch (tableEntry.header.type)
//                        {
//                           case SMBIOSTableType.BIOSInformation:
//                              SMBIOS.BiosInfo = RSMB.ToStruct<SMBIOSTableBiosInfo>(pos);
//                              RawStrings = GetRawStrings(RSMB, tableEntry, ref pos);
                              
//                              Console.WriteLine("BIOSInformation");
                              
//                              Console.ForegroundColor = ConsoleColor.Yellow;
                              
//                              if (SMBIOS.BiosInfo.vendor > 0 && SMBIOS.BiosInfo.vendor <= RawStrings.Length)
//                                 Console.WriteLine("Vendor: " + RawStrings[SMBIOS.BiosInfo.vendor - 1]);
                              
//                              if (SMBIOS.BiosInfo.version > 0 && SMBIOS.BiosInfo.version <= RawStrings.Length)
//                                 Console.WriteLine("Version: " + RawStrings[SMBIOS.BiosInfo.version - 1]);
//                              break;
//                           case SMBIOSTableType.SystemInformation:
//                              SMBIOS.SystemInfo = RSMB.ToStruct<SMBIOSTableSystemInfo>(pos);
//                              RawStrings = GetRawStrings(RSMB, tableEntry, ref pos);
                              
//                              Console.WriteLine("SystemInformation");
                              
//                              Console.ForegroundColor = ConsoleColor.Yellow;
                              
//                              if (SMBIOS.SystemInfo.manufacturer > 0 &&
//                                  SMBIOS.SystemInfo.manufacturer <= RawStrings.Length)
//                                 Console.WriteLine("Manufacturer: " + RawStrings[SMBIOS.SystemInfo.manufacturer - 1]);
                              
//                              if (SMBIOS.SystemInfo.productName > 0 &&
//                                  SMBIOS.SystemInfo.productName <= RawStrings.Length)
//                                 Console.WriteLine("Product name: " + RawStrings[SMBIOS.SystemInfo.productName - 1]);
                              
//                              if (SMBIOS.SystemInfo.version > 0 && SMBIOS.SystemInfo.version <= RawStrings.Length)
//                                 Console.WriteLine("Version: " + RawStrings[SMBIOS.SystemInfo.version - 1]);
                              
//                              if (SMBIOS.SystemInfo.serialNumber > 0 &&
//                                  SMBIOS.SystemInfo.serialNumber <= RawStrings.Length)
//                                 Console.WriteLine("Serial Number: " + RawStrings[SMBIOS.SystemInfo.serialNumber - 1]);
//                              break;
//                           case SMBIOSTableType.BaseBoardInformation:
//                              SMBIOS.BaseBoardInfo = RSMB.ToStruct<SMBIOSTableBaseBoardInfo>(pos);
//                              RawStrings = GetRawStrings(RSMB, tableEntry, ref pos);
                              
//                              Console.WriteLine("BaseBoardInformation");
                              
//                              Console.ForegroundColor = ConsoleColor.Yellow;
                              
//                              if (SMBIOS.BaseBoardInfo.manufacturer > 0 &&
//                                  SMBIOS.BaseBoardInfo.manufacturer <= RawStrings.Length)
//                                 Console.WriteLine("Manufacturer: " +
//                                                   RawStrings[SMBIOS.BaseBoardInfo.manufacturer - 1]);
                              
//                              if (SMBIOS.BaseBoardInfo.productName > 0 &&
//                                  SMBIOS.BaseBoardInfo.productName <= RawStrings.Length)
//                                 Console.WriteLine("Product name: " + RawStrings[SMBIOS.BaseBoardInfo.productName - 1]);
                              
//                              if (SMBIOS.BaseBoardInfo.version > 0 && SMBIOS.BaseBoardInfo.version <= RawStrings.Length)
//                                 Console.WriteLine("Version: " + RawStrings[SMBIOS.BaseBoardInfo.version - 1]);
                              
//                              if (SMBIOS.BaseBoardInfo.serialNumber > 0 &&
//                                  SMBIOS.BaseBoardInfo.serialNumber <= RawStrings.Length)
//                                 Console.WriteLine(
//                                    "Serial Number: " + RawStrings[SMBIOS.BaseBoardInfo.serialNumber - 1]);
//                              break;
//                           case SMBIOSTableType.EnclosureInformation:
//                              SMBIOS.EnclosureInfo = RSMB.ToStruct<SMBIOSTableEnclosureInfo>(pos);
//                              RawStrings = GetRawStrings(RSMB, tableEntry, ref pos);
                              
//                              Console.WriteLine("EnclosureInformation");
                              
//                              Console.ForegroundColor = ConsoleColor.Yellow;
                              
//                              if (SMBIOS.EnclosureInfo.manufacturer > 0 &&
//                                  SMBIOS.EnclosureInfo.manufacturer <= RawStrings.Length)
//                                 Console.WriteLine("Manufacturer: " +
//                                                   RawStrings[SMBIOS.EnclosureInfo.manufacturer - 1]);
                              
//                              if (SMBIOS.EnclosureInfo.version > 0 && SMBIOS.EnclosureInfo.version <= RawStrings.Length)
//                                 Console.WriteLine("Version: " + RawStrings[SMBIOS.EnclosureInfo.version - 1]);
                              
//                              if (SMBIOS.EnclosureInfo.serialNumber > 0 &&
//                                  SMBIOS.EnclosureInfo.serialNumber <= RawStrings.Length)
//                                 Console.WriteLine(
//                                    "Serial Number: " + RawStrings[SMBIOS.EnclosureInfo.serialNumber - 1]);
                              
//                              if (SMBIOS.EnclosureInfo.assetTagNumber > 0 &&
//                                  SMBIOS.EnclosureInfo.assetTagNumber <= RawStrings.Length)
//                                 Console.WriteLine("Asset Tag Number: " +
//                                                   RawStrings[SMBIOS.EnclosureInfo.assetTagNumber - 1]);
//                              break;
//                           case SMBIOSTableType.ProcessorInformation:
//                              SMBIOS.ProcessorInfo = RSMB.ToStruct<SMBIOSTableProcessorInfo>(pos);
//                              RawStrings = GetRawStrings(RSMB, tableEntry, ref pos);
                              
//                              Console.WriteLine("ProcessorInformation");
                              
//                              Console.ForegroundColor = ConsoleColor.Yellow;
                              
//                              if (SMBIOS.ProcessorInfo.processorManufacturer > 0 &&
//                                  SMBIOS.ProcessorInfo.processorManufacturer <= RawStrings.Length)
//                                 Console.WriteLine("Manufacturer: " +
//                                                   RawStrings[SMBIOS.ProcessorInfo.processorManufacturer - 1]);
                              
//                              if (SMBIOS.ProcessorInfo.processorType > 0 &&
//                                  SMBIOS.ProcessorInfo.processorType <= RawStrings.Length)
//                                 Console.WriteLine("Model: " + RawStrings[SMBIOS.ProcessorInfo.processorType - 1]);
                              
//                              if (SMBIOS.ProcessorInfo.processorFamily > 0 &&
//                                  SMBIOS.ProcessorInfo.processorFamily <= RawStrings.Length)
//                                 Console.WriteLine("Family: " + RawStrings[SMBIOS.ProcessorInfo.processorFamily - 1]);
                              
//                              if (SMBIOS.ProcessorInfo.serialNumber > 0 &&
//                                  SMBIOS.ProcessorInfo.serialNumber <= RawStrings.Length)
//                                 Console.WriteLine(
//                                    "Serial Number: " + RawStrings[SMBIOS.ProcessorInfo.serialNumber - 1]);
                              
//                              if (SMBIOS.ProcessorInfo.assetTag > 0 &&
//                                  SMBIOS.ProcessorInfo.assetTag <= RawStrings.Length)
//                                 Console.WriteLine("Asset Tag: " + RawStrings[SMBIOS.ProcessorInfo.assetTag - 1]);
//                              break;
//                           case SMBIOSTableType.CacheInformation:
//                              SMBIOS.CacheInfo = RSMB.ToStruct<SMBIOSTableCacheInfo>(pos);
//                              RawStrings = GetRawStrings(RSMB, tableEntry, ref pos);
                              
//                              Console.WriteLine("CacheInformation [" + RawStrings[0] + "]");
                              
//                              Console.ForegroundColor = ConsoleColor.Yellow;
                              
//                              Console.WriteLine("Installed Size: " + SMBIOS.CacheInfo.installedSize);
                              
//                              Console.WriteLine("Maximum CacheSize: " + SMBIOS.CacheInfo.maximumCacheSize);
//                              break;
//                           default:
//                              Console.WriteLine(tableEntry.header.type);
//                              RawStrings = GetRawStrings(RSMB, tableEntry, ref pos);
//                              break;
//                        }
//                     }
//                  }
//               }
//            }

//            var tableIndex = 0;
//            foreach (var biosTable in EnumerateTables(BiosFirmwareTableProvider.FIRM))
//            {
//               var FIRM = GetTable(BiosFirmwareTableProvider.FIRM, biosTable);
               
//               File.WriteAllBytes("BIOS." + BitConverter.ToString(new MD5CryptoServiceProvider().ComputeHash(FIRM), 0).Replace("-", "") + ".FIRM" + (tableIndex++) + ".bin", FIRM);

//            }

            
//            Console.WriteLine();
            
//            Console.ForegroundColor = ConsoleColor.White;
            
//            Console.WriteLine("[CPU]");

//            // Get CPU Information (Count, ID) //
//            byte[] code_x86 = {
//               0x55, 0x89, 0xe5, 0x57, 0x8b, 0x7d, 0x10, 0x6a, 0x01, 0x58, 0x53, 0x0f, 0xa2, 0x89, 0x07, 0x89, 0x57,
//               0x04, 0x5b, 0x5f, 0x89, 0xec, 0x5d, 0xc2, 0x10, 0x00
//            };
//            byte[] code_x64 = {
//               0x53, 0x48, 0xc7, 0xc0, 0x01, 0x00, 0x00, 0x00, 0x0f, 0xa2, 0x41, 0x89, 0x00, 0x41, 0x89, 0x50, 0x04,
//               0x5b, 0xc3
//            };
//            var CPUID = new byte[8];
//            byte[] asmCode;
//            int protect;
//            if (IntPtr.Size == 8) asmCode = code_x64;
//            else asmCode = code_x86;
//            var asmSize = new IntPtr(asmCode.Length);
//            if (!VirtualProtect(asmCode, asmSize, 0x40, out protect))
//               Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
//            asmSize = new IntPtr(CPUID.Length);
//            if (CallWindowProcW(asmCode, IntPtr.Zero, 0, CPUID, asmSize) == IntPtr.Zero) return;
//            var RegKey =
//               Registry.LocalMachine.OpenSubKey(@"HARDWARE\DESCRIPTION\System\CentralProcessor");
//            var ProcessorName = RegKey.OpenSubKey("0").GetValue("ProcessorNameString").ToString();
//            var CPUIdentifier = RegKey.OpenSubKey("0").GetValue("Identifier").ToString();
//            var ProcessorCores = RegKey.GetSubKeyNames();

            
//            Console.ForegroundColor = ConsoleColor.Green;
            
//            Console.WriteLine(ProcessorName + ", " + CPUIdentifier);
            
//            Console.ForegroundColor = ConsoleColor.Yellow;
            
//            Console.WriteLine("> Processor Cores: " + ProcessorCores.Length);
            
//            Console.WriteLine("> ID: " + BitConverter.ToString(CPUID, 0).Replace("-", ""));

//            Binary.Write(ProcessorName);
//            Binary.Write(CPUIdentifier);
//            Binary.Write(ProcessorCores.Length);
//            Binary.Write(CPUID);

            
//            Console.WriteLine();
            
//            Console.ForegroundColor = ConsoleColor.White;
            
//            Console.WriteLine("[DRIVES]");

//            // Get HDD Information //
//            foreach (var drive in Environment.GetLogicalDrives())
//            {
//               var disk = GetPhysicalDisk(drive);
//               if (disk.RemovableMedia) continue;
               
//               Console.ForegroundColor = ConsoleColor.Green;
               
//               Console.WriteLine("Disk: " + drive);
               
//               Console.ForegroundColor = ConsoleColor.Yellow;
               
//               Console.WriteLine("> Model: " + disk.Model + ", Firmware: " + disk.Firmware + "\n> Serial: " +
//                                 disk.SerialNumber);
//               if (string.IsNullOrEmpty(disk.Model) || string.IsNullOrEmpty(disk.Firmware)) continue;
//               Binary.Write(disk.Model);
//               Binary.Write(disk.Firmware);
//               Binary.Write(disk.SerialNumber);
//            }

            
//            Console.WriteLine();
            
//            Console.ForegroundColor = ConsoleColor.White;
            
//            Console.WriteLine("[VIDEO]");

//            // Get VIDEO Information //
//            try
//            {
//               var display = new DISPLAY_DEVICE();
//               display.structSize = Marshal.SizeOf(display);
//               for (uint i = 0; EnumDisplayDevices(null, i, ref display, 0); i++)
//               {
//                  if (display.DeviceString == null) continue;
//                  if (display.StateFlags.HasFlag(DisplayDeviceStateFlags.PrimaryDevice))
//                  {
                     
//                     Console.ForegroundColor = ConsoleColor.Green;
                     
//                     Console.WriteLine(display.DeviceString);
                     
//                     Console.ForegroundColor = ConsoleColor.Yellow;
                     
//                     Console.WriteLine("DeviceID: " + display.DeviceID);
                     
//                     Console.WriteLine("State Flags: " + display.StateFlags);
//                     Binary.Write(display.DeviceString);
//                     if (display.DeviceID != null) Binary.Write(display.DeviceID);
//                  }
//               }
//            }
//            catch (Exception)
//            {
//               /* Ignore all exceptions of detecting video */
//            }

//            // Seek Stream Origin To Beginning and Compute Binary Buffer to MD5 //
//            Binary.BaseStream.Seek(0, SeekOrigin.Begin);
//            _MD5 = new MD5CryptoServiceProvider().ComputeHash(Binary.BaseStream);
//            // Seek Stream Origin To Beginning and Compute Binary Buffer to SHA1 //
//            Binary.BaseStream.Seek(0, SeekOrigin.Begin);
//            _SHA1 = new SHA1CryptoServiceProvider().ComputeHash(Binary.BaseStream);
//            // Seek Stream Origin To Beginning and Compute Binary Buffer to SHA256 //
//            Binary.BaseStream.Seek(0, SeekOrigin.Begin);
//            _SHA256 = new SHA256CryptoServiceProvider().ComputeHash(Binary.BaseStream);
//            // Seek Stream Origin To Beginning and Compute Binary Buffer to SHA512 //
//            Binary.BaseStream.Seek(0, SeekOrigin.Begin);
//            _SHA512 = new SHA512CryptoServiceProvider().ComputeHash(Binary.BaseStream);
//         }

//         Console.ResetColor();
//      }

//      internal byte[] MD5
//      {
//         get
//         {
//            if (_MD5 == null || _MD5.Length < 16) return new byte[0];
//            return _MD5;
//         }
//      }

//      internal byte[] SHA1
//      {
//         get
//         {
//            if (_SHA1 == null || _SHA1.Length < 16) return new byte[0];
//            return _SHA1;
//         }
//      }

//      internal byte[] SHA256
//      {
//         get
//         {
//            if (_SHA256 == null || _SHA256.Length < 16) return new byte[0];
//            return _SHA256;
//         }
//      }

//      internal byte[] SHA512
//      {
//         get
//         {
//            if (_SHA512 == null || _SHA512.Length < 16) return new byte[0];
//            return _SHA512;
//         }
//      }

//      internal string MD5String
//      {
//         get
//         {
//            if (_MD5 == null || _MD5.Length < 16) return "";
//            return BitConverter.ToString(_MD5, 0).Replace("-", "");
//         }
//      }

//      internal string SHA1String
//      {
//         get
//         {
//            if (_SHA1 == null || _SHA1.Length < 16) return "";
//            return BitConverter.ToString(_SHA1, 0).Replace("-", "");
//         }
//      }

//      internal string SHA256String
//      {
//         get
//         {
//            if (_SHA256 == null || _SHA256.Length < 16) return "";
//            return BitConverter.ToString(_SHA256, 0).Replace("-", "");
//         }
//      }

//      internal string SHA512String
//      {
//         get
//         {
//            if (_SHA512 == null || _SHA512.Length < 16) return "";
//            return BitConverter.ToString(_SHA512, 0).Replace("-", "");
//         }
//      }
//   }
//}
