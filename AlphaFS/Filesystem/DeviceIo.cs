/* Copyright (c) 2008-2009 Peter Palotas
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
using System.Security.Permissions;
using System.Text;

namespace Alphaleonis.Win32.Filesystem
{
    internal static class DeviceIo
    {

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope"), SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static SafeHandle CreateFile(string path, FileAccess access, FileShare share, FileMode mode, FileOptions options)
        {
            SafeHandle h = NativeMethods.CreateFileW(path, access, share, null, mode, options, new SafeGlobalMemoryBufferHandle());
            if (h.IsInvalid)
                NativeError.ThrowException();
            return h;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope"), SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static SafeHandle CreateFile(KernelTransaction trans, string path, FileAccess access, FileShare share, FileMode mode, FileOptions options)
        {
            SafeHandle h = NativeMethods.CreateFileTransactedW(path, access, share, null, mode, options, new SafeGlobalMemoryBufferHandle(), trans.SafeHandle, IntPtr.Zero, IntPtr.Zero);
            if (h.IsInvalid)
                NativeError.ThrowException();
            return h;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2001:AvoidCallingProblematicMethods", MessageId = "System.Runtime.InteropServices.SafeHandle.DangerousGetHandle"), SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static LinkTargetInfo GetLinkTargetInfo(SafeHandle device)
        {
            UInt32 bytesReturned;

            int lastError = 0;
            const int maxCapacity = 0x3FF0;

            SafeGlobalMemoryBufferHandle buffer = new SafeGlobalMemoryBufferHandle(512);
            try
            {
                do
                {
                    if (!NativeMethods.DeviceIoControl(device, IoControlCode.FsctlGetReparsePoint, new SafeGlobalMemoryBufferHandle(), 0, buffer, (uint)buffer.Capacity, out bytesReturned, IntPtr.Zero))
                    {
                        lastError = Marshal.GetLastWin32Error();
                        if (lastError == Win32Errors.ERROR_INSUFFICIENT_BUFFER && buffer.Capacity < maxCapacity)
                        {
                            buffer.Dispose();
                            buffer = null;
                            buffer = new SafeGlobalMemoryBufferHandle(maxCapacity);
                            continue;
                        }
                        NativeError.ThrowException(lastError);
                    }
                    else
                        break;
                }
                while (true);

                IntPtr bufPtr = buffer.DangerousGetHandle();
                ReparseDataBufferHeader header = (ReparseDataBufferHeader)Marshal.PtrToStructure(bufPtr, typeof(ReparseDataBufferHeader));

                if (header.ReparseTag == ReparsePointTag.MountPoint)
                {
                    MountPointReparseBuffer mpBuf = (MountPointReparseBuffer)Marshal.PtrToStructure(AddPtr(bufPtr, Marshal.OffsetOf(typeof(ReparseDataBufferHeader), "data")), typeof(MountPointReparseBuffer));
                    IntPtr dataPos = AddPtr(Marshal.OffsetOf(typeof(ReparseDataBufferHeader), "data"), Marshal.OffsetOf(typeof(MountPointReparseBuffer), "data"));
                    byte[] dataBuffer = new byte[bytesReturned - dataPos.ToInt64()];
                    Marshal.Copy(AddPtr(bufPtr, dataPos), dataBuffer, 0, dataBuffer.Length);
                    return new LinkTargetInfo(
                        Encoding.Unicode.GetString(dataBuffer, mpBuf.SubstituteNameOffset, mpBuf.SubstituteNameLength),
                        Encoding.Unicode.GetString(dataBuffer, mpBuf.PrintNameOffset, mpBuf.PrintNameLength)
                    );
                }
                else if (header.ReparseTag == ReparsePointTag.SymLink)
                {
                    SymbolicLinkReparseBuffer mpBuf = (SymbolicLinkReparseBuffer)Marshal.PtrToStructure(AddPtr(bufPtr, Marshal.OffsetOf(typeof(ReparseDataBufferHeader), "data")), typeof(SymbolicLinkReparseBuffer));
                    IntPtr dataPos = AddPtr(Marshal.OffsetOf(typeof(ReparseDataBufferHeader), "data"), Marshal.OffsetOf(typeof(SymbolicLinkReparseBuffer), "data"));
                    byte[] dataBuffer = new byte[bytesReturned - dataPos.ToInt64()];
                    Marshal.Copy(AddPtr(bufPtr, dataPos), dataBuffer, 0, dataBuffer.Length);
                    return new SymbolicLinkTargetInfo(
                        Encoding.Unicode.GetString(dataBuffer, mpBuf.SubstituteNameOffset, mpBuf.SubstituteNameLength),
                        Encoding.Unicode.GetString(dataBuffer, mpBuf.PrintNameOffset, mpBuf.PrintNameLength),
                        (SymbolicLinkType)mpBuf.Flags
                    );
                }
                else
                {
                    throw new UnrecognizedReparsePointException();
                }

            }
            finally
            {
                if (buffer != null)
                    buffer.Dispose();
            }
        }

        private static IntPtr AddPtr(IntPtr lhs, IntPtr rhs)
        {
            switch (IntPtr.Size)
            {
                case 4:
                    return new IntPtr(lhs.ToInt32() + rhs.ToInt32());
                case 8:
                    return new IntPtr(lhs.ToInt64() + rhs.ToInt64());
                default:
                    throw new NotSupportedException("Unsupported pointer size: " + IntPtr.Size);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct ReparseDataBufferHeader
        {
            [MarshalAs(UnmanagedType.U4)]
            public ReparsePointTag ReparseTag;
            public ushort ReparseDataLength;
            public ushort Reserved;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=1)]
            public byte[] data;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct SymbolicLinkReparseBuffer 
        {
            public ushort SubstituteNameOffset;
            public ushort SubstituteNameLength;
            public ushort PrintNameOffset;
            public ushort PrintNameLength;
            public uint Flags;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public byte[] data;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MountPointReparseBuffer
        {
            public ushort SubstituteNameOffset;
            public ushort SubstituteNameLength;
            public ushort PrintNameOffset;
            public ushort PrintNameLength;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public byte[] data;
        }
    }
}
