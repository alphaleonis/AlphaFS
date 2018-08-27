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
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using Alphaleonis.Win32.Filesystem;
using Microsoft.Win32.SafeHandles;

namespace Alphaleonis.Win32.Device
{
   internal static partial class FilesystemHelper
   {
      /// <summary>[AlphaFS] Get information about the target of a mount point or symbolic link on an NTFS file system.</summary>
      /// <exception cref="NotAReparsePointException"/>
      /// <exception cref="UnrecognizedReparsePointException"/>
      [SecurityCritical]
      internal static LinkTargetInfo GetLinkTargetInfo(SafeFileHandle safeFileHandle, string reparsePath)
      {
         using (var safeBuffer = GetLinkTargetData(safeFileHandle, reparsePath))
         {
            var header = safeBuffer.PtrToStructure<Filesystem.NativeMethods.ReparseDataBufferHeader>();

            var marshalReparseBuffer = (int)Marshal.OffsetOf(typeof(Filesystem.NativeMethods.ReparseDataBufferHeader), "data");

            var dataOffset = (int)(marshalReparseBuffer + (header.ReparseTag == ReparsePointTag.MountPoint
               ? Marshal.OffsetOf(typeof(Filesystem.NativeMethods.MountPointReparseBuffer), "data")
               : Marshal.OffsetOf(typeof(Filesystem.NativeMethods.SymbolicLinkReparseBuffer), "data")).ToInt64());

            var dataBuffer = new byte[MAXIMUM_REPARSE_DATA_BUFFER_SIZE - dataOffset];


            switch (header.ReparseTag)
            {
               // MountPoint can be a junction or mounted drive (mounted drive starts with "\??\Volume").

               case ReparsePointTag.MountPoint:
                  var mountPoint = safeBuffer.PtrToStructure<Filesystem.NativeMethods.MountPointReparseBuffer>(marshalReparseBuffer);

                  safeBuffer.CopyTo(dataOffset, dataBuffer);

                  return new LinkTargetInfo(
                     Encoding.Unicode.GetString(dataBuffer, mountPoint.SubstituteNameOffset, mountPoint.SubstituteNameLength),
                     Encoding.Unicode.GetString(dataBuffer, mountPoint.PrintNameOffset, mountPoint.PrintNameLength));


               case ReparsePointTag.SymLink:
                  var symLink = safeBuffer.PtrToStructure<Filesystem.NativeMethods.SymbolicLinkReparseBuffer>(marshalReparseBuffer);

                  safeBuffer.CopyTo(dataOffset, dataBuffer);

                  return new SymbolicLinkTargetInfo(
                     Encoding.Unicode.GetString(dataBuffer, symLink.SubstituteNameOffset, symLink.SubstituteNameLength),
                     Encoding.Unicode.GetString(dataBuffer, symLink.PrintNameOffset, symLink.PrintNameLength), symLink.Flags);


               default:
                  throw new UnrecognizedReparsePointException(reparsePath);
            }
         }
      }


      /// <summary>[AlphaFS] Get information about the target of a mount point or symbolic link on an NTFS file system.</summary>
      /// <exception cref="NotAReparsePointException"/>
      /// <exception cref="UnrecognizedReparsePointException"/>
      [SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
      [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Disposing is controlled.")]
      [SecurityCritical]
      private static SafeGlobalMemoryBufferHandle GetLinkTargetData(SafeFileHandle safeFileHandle, string reparsePath)
      {
         Utils.IsValidHandle(safeFileHandle);

         var bufferSize = MAXIMUM_REPARSE_DATA_BUFFER_SIZE;

         while (true)
         {
            var safeBuffer = new SafeGlobalMemoryBufferHandle(bufferSize);

            var success = NativeMethods.DeviceIoControl(safeFileHandle, NativeMethods.IoControlCode.FSCTL_GET_REPARSE_POINT, IntPtr.Zero, 0, safeBuffer, (uint) safeBuffer.Capacity, IntPtr.Zero, IntPtr.Zero);

            var lastError = Marshal.GetLastWin32Error();


            if (success)
               return safeBuffer;

            bufferSize = Utils.GetDoubledBufferSizeOrThrowException(safeBuffer, lastError, bufferSize, reparsePath);
         }
      }
   }
}
