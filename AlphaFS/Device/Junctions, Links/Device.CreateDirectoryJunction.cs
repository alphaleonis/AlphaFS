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
using System.Runtime.InteropServices;
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
   }
}
