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

using Microsoft.Win32.SafeHandles;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace Alphaleonis.Win32.Filesystem
{
   /// <summary>Provides static methods to retrieve device resource information from a local or remote host.</summary>
   public static partial class Device
   {
      /// <summary>[AlphaFS] Creates an NTFS directory junction (similar to CMD command: "MKLINK /J").</summary>
      internal static void CreateDirectoryJunction(SafeFileHandle safeHandle, string directoryPath)
      {
         var targetDirBytes = Encoding.Unicode.GetBytes(Path.NonInterpretedPathPrefix + Path.GetRegularPathCore(directoryPath, GetFullPathOptions.AddTrailingDirectorySeparator, false));
         
         var header = new NativeMethods.ReparseDataBufferHeader
         {
            ReparseTag = ReparsePointTag.MountPoint,
            ReparseDataLength = (ushort) (targetDirBytes.Length + 12)
         };

         var mountPoint = new NativeMethods.MountPointReparseBuffer
         {
            SubstituteNameOffset = 0,
            SubstituteNameLength = (ushort) targetDirBytes.Length,
            PrintNameOffset = (ushort) (targetDirBytes.Length + UnicodeEncoding.CharSize),
            PrintNameLength = 0
         };

         var reparseDataBuffer = new NativeMethods.REPARSE_DATA_BUFFER
         {
            ReparseTag = header.ReparseTag,
            ReparseDataLength = header.ReparseDataLength,

            SubstituteNameOffset = mountPoint.SubstituteNameOffset,
            SubstituteNameLength = mountPoint.SubstituteNameLength,
            PrintNameOffset = mountPoint.PrintNameOffset,
            PrintNameLength = mountPoint.PrintNameLength,

            PathBuffer = new byte[NativeMethods.MAXIMUM_REPARSE_DATA_BUFFER_SIZE - 16] // 16368
         };
         
         targetDirBytes.CopyTo(reparseDataBuffer.PathBuffer, 0);


         using (var safeBuffer = new SafeGlobalMemoryBufferHandle(Marshal.SizeOf(reparseDataBuffer)))
         {
            safeBuffer.StructureToPtr(reparseDataBuffer, false);

            uint bytesReturned;
            var success = NativeMethods.DeviceIoControl2(safeHandle, NativeMethods.IoControlCode.FSCTL_SET_REPARSE_POINT, safeBuffer, (uint) (targetDirBytes.Length + 20), IntPtr.Zero, 0, out bytesReturned, IntPtr.Zero);

            var lastError = Marshal.GetLastWin32Error();
            if (!success)
               NativeError.ThrowException(lastError, directoryPath);
         }
      }


      /// <summary>[AlphaFS] Deletes an NTFS directory junction.</summary>
      internal static void DeleteDirectoryJunction(SafeFileHandle safeHandle)
      {
         var reparseDataBuffer = new NativeMethods.REPARSE_DATA_BUFFER
         {
            ReparseTag = ReparsePointTag.MountPoint,
            ReparseDataLength = 0,
            PathBuffer = new byte[NativeMethods.MAXIMUM_REPARSE_DATA_BUFFER_SIZE - 16] // 16368
         };


         using (var safeBuffer = new SafeGlobalMemoryBufferHandle(Marshal.SizeOf(reparseDataBuffer)))
         {
            safeBuffer.StructureToPtr(reparseDataBuffer, false);

            uint bytesReturned;
            var success = NativeMethods.DeviceIoControl2(safeHandle, NativeMethods.IoControlCode.FSCTL_DELETE_REPARSE_POINT, safeBuffer, NativeMethods.REPARSE_DATA_BUFFER_HEADER_SIZE, IntPtr.Zero, 0, out bytesReturned, IntPtr.Zero);

            var lastError = Marshal.GetLastWin32Error();
            if (!success)
               NativeError.ThrowException(lastError);
         }
      }
      



      /// <summary>[AlphaFS] Get information about the target of a mount point or symbolic link on an NTFS file system.</summary>
      /// <exception cref="NotAReparsePointException"/>
      /// <exception cref="UnrecognizedReparsePointException"/>
      [SecurityCritical]
      internal static LinkTargetInfo GetLinkTargetInfo(SafeFileHandle safeHandle, string reparsePath)
      {
         using (var safeBuffer = GetLinkTargetData(safeHandle, reparsePath))
         {
            var header = safeBuffer.PtrToStructure<NativeMethods.ReparseDataBufferHeader>(0);
            
            var marshalReparseBuffer = (int) Marshal.OffsetOf(typeof(NativeMethods.ReparseDataBufferHeader), "data");

            var dataOffset = (int) (marshalReparseBuffer + (header.ReparseTag == ReparsePointTag.MountPoint
               ? Marshal.OffsetOf(typeof(NativeMethods.MountPointReparseBuffer), "data")
               : Marshal.OffsetOf(typeof(NativeMethods.SymbolicLinkReparseBuffer), "data")).ToInt64());

            var dataBuffer = new byte[NativeMethods.MAXIMUM_REPARSE_DATA_BUFFER_SIZE - dataOffset];


            switch (header.ReparseTag)
            {
               // MountPoint can be a junction or mounted drive (mounted drive starts with "\??\Volume").

               case ReparsePointTag.MountPoint:
                  var mountPoint = safeBuffer.PtrToStructure<NativeMethods.MountPointReparseBuffer>(marshalReparseBuffer);

                  safeBuffer.CopyTo(dataOffset, dataBuffer);

                  return new LinkTargetInfo(
                     Encoding.Unicode.GetString(dataBuffer, mountPoint.SubstituteNameOffset, mountPoint.SubstituteNameLength),
                     Encoding.Unicode.GetString(dataBuffer, mountPoint.PrintNameOffset, mountPoint.PrintNameLength));


               case ReparsePointTag.SymLink:
                  var symLink = safeBuffer.PtrToStructure<NativeMethods.SymbolicLinkReparseBuffer>(marshalReparseBuffer);

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
      private static SafeGlobalMemoryBufferHandle GetLinkTargetData(SafeFileHandle safeHandle, string reparsePath)
      {
         var safeBuffer = new SafeGlobalMemoryBufferHandle(NativeMethods.MAXIMUM_REPARSE_DATA_BUFFER_SIZE);

         while (true)
         {
            uint bytesReturned;
            var success = NativeMethods.DeviceIoControl(safeHandle, NativeMethods.IoControlCode.FSCTL_GET_REPARSE_POINT, IntPtr.Zero, 0, safeBuffer, (uint) safeBuffer.Capacity, out bytesReturned, IntPtr.Zero);

            var lastError = Marshal.GetLastWin32Error();
            if (!success)
            {
               switch ((uint) lastError)
               {
                  case Win32Errors.ERROR_MORE_DATA:
                  case Win32Errors.ERROR_INSUFFICIENT_BUFFER:

                     // Should not happen since we already use the maximum size.

                     if (safeBuffer.Capacity < bytesReturned)
                        safeBuffer.Close();
                     break;
               }

               if (lastError != Win32Errors.ERROR_SUCCESS)
                  NativeError.ThrowException(lastError, reparsePath);
            }

            else
               break;
         }


         return safeBuffer;
      }
   }
}
