/*  Copyright (C) 2008-2016 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
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

using Alphaleonis.Win32.Security;
using Microsoft.Win32.SafeHandles;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace Alphaleonis.Win32.Filesystem
{
   public static partial class Path
   {
      /// <summary>[AlphaFS] Retrieves the final path for the specified file, formatted as <see cref="FinalPathFormats"/>.</summary>
      /// <returns>The final path as a string.</returns>
      /// <remarks>
      ///   A final path is the path that is returned when a path is fully resolved. For example, for a symbolic link named "C:\tmp\mydir" that
      ///   points to "D:\yourdir", the final path would be "D:\yourdir".
      /// </remarks>
      /// <param name="handle">Then handle to a <see cref="SafeFileHandle"/> instance.</param>
      [SecurityCritical]
      public static string GetFinalPathNameByHandle(SafeFileHandle handle)
      {
         return GetFinalPathNameByHandleCore(handle, FinalPathFormats.None);
      }

      /// <summary>[AlphaFS] Retrieves the final path for the specified file, formatted as <see cref="FinalPathFormats"/>.</summary>
      /// <returns>The final path as a string.</returns>
      /// <remarks>
      ///   A final path is the path that is returned when a path is fully resolved. For example, for a symbolic link named "C:\tmp\mydir" that
      ///   points to "D:\yourdir", the final path would be "D:\yourdir".
      /// </remarks>
      /// <param name="handle">Then handle to a <see cref="SafeFileHandle"/> instance.</param>
      /// <param name="finalPath">The final path, formatted as <see cref="FinalPathFormats"/></param>
      [SecurityCritical]
      public static string GetFinalPathNameByHandle(SafeFileHandle handle, FinalPathFormats finalPath)
      {
         return GetFinalPathNameByHandleCore(handle, finalPath);
      }

      /// <summary>Retrieves the final path for the specified file, formatted as <see cref="FinalPathFormats"/>.</summary>
      /// <returns>The final path as a string.</returns>
      /// <remarks>
      ///   A final path is the path that is returned when a path is fully resolved. For example, for a symbolic link named "C:\tmp\mydir" that
      ///   points to "D:\yourdir", the final path would be "D:\yourdir". The string that is returned by this function uses the
      ///   <see cref="LongPathPrefix"/> syntax.
      /// </remarks>
      /// <param name="handle">Then handle to a <see cref="SafeFileHandle"/> instance.</param>
      /// <param name="finalPath">The final path, formatted as <see cref="FinalPathFormats"/></param>
      [SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults", MessageId = "Alphaleonis.Win32.Filesystem.NativeMethods.GetMappedFileName(System.IntPtr,Alphaleonis.Win32.SafeGlobalMemoryBufferHandle,System.Text.StringBuilder,System.UInt32)")]
      [SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults", MessageId = "Alphaleonis.Win32.Filesystem.NativeMethods.GetMappedFileName(System.IntPtr,Alphaleonis.Win32.Security.SafeLocalMemoryBufferHandle,System.Text.StringBuilder,System.UInt32)")]
      [SecurityCritical]
      internal static string GetFinalPathNameByHandleCore(SafeFileHandle handle, FinalPathFormats finalPath)
      {
         NativeMethods.IsValidHandle(handle);

         var buffer = new StringBuilder(NativeMethods.MaxPathUnicode);

         using (new NativeMethods.ChangeErrorMode(NativeMethods.ErrorMode.FailCriticalErrors))
         {
            if (NativeMethods.IsAtLeastWindowsVista)
            {
               if (NativeMethods.GetFinalPathNameByHandle(handle, buffer, (uint) buffer.Capacity, finalPath) == Win32Errors.ERROR_SUCCESS)
                  NativeError.ThrowException(Marshal.GetLastWin32Error());

               return buffer.ToString();
            }
         }

         #region Older OperatingSystem

         // Obtaining a File Name From a File Handle
         // http://msdn.microsoft.com/en-us/library/aa366789%28VS.85%29.aspx

         // Be careful when using GetFileSizeEx to check the size of hFile handle of an unknown "File" type object.
         // This is more towards returning a filename from a file handle. If the handle is a named pipe handle it seems to hang the thread.
         // Check for: FileTypes.DiskFile

         // Can't map a 0 byte file.
         long fileSizeHi;
         if (!NativeMethods.GetFileSizeEx(handle, out fileSizeHi))
            if (fileSizeHi == 0)
               return string.Empty;


         // PAGE_READONLY
         // Allows views to be mapped for read-only or copy-on-write access. An attempt to write to a specific region results in an access violation.
         // The file handle that the hFile parameter specifies must be created with the GENERIC_READ access right.
         // PageReadOnly = 0x02,
         using (SafeFileHandle handle2 = NativeMethods.CreateFileMapping(handle, null, 2, 0, 1, null))
         {
            NativeMethods.IsValidHandle(handle, Marshal.GetLastWin32Error());

            // FILE_MAP_READ
            // Read = 4
            using (SafeLocalMemoryBufferHandle pMem = NativeMethods.MapViewOfFile(handle2, 4, 0, 0, (UIntPtr)1))
            {
               if (NativeMethods.IsValidHandle(pMem, Marshal.GetLastWin32Error()))
                  if (NativeMethods.GetMappedFileName(Process.GetCurrentProcess().Handle, pMem, buffer, (uint)buffer.Capacity))
                     NativeMethods.UnmapViewOfFile(pMem);
            }
         }


         // Default output from GetMappedFileName(): "\Device\HarddiskVolumeX\path\filename.ext"
         string dosDevice = buffer.Length > 0 ? buffer.ToString() : string.Empty;

         // Select output format.
         switch (finalPath)
         {
            // As-is: "\Device\HarddiskVolumeX\path\filename.ext"
            case FinalPathFormats.VolumeNameNT:
               return dosDevice;

            // To: "\path\filename.ext"
            case FinalPathFormats.VolumeNameNone:
               return DosDeviceToDosPath(dosDevice, string.Empty);

            // To: "\\?\Volume{GUID}\path\filename.ext"
            case FinalPathFormats.VolumeNameGuid:
               string dosPath = DosDeviceToDosPath(dosDevice, null);
               if (!Utils.IsNullOrWhiteSpace(dosPath))
               {
                  string path = GetSuffixedDirectoryNameWithoutRootCore(null, dosPath);
                  string driveLetter = RemoveTrailingDirectorySeparator(GetPathRoot(dosPath, false), false);
                  string file = GetFileName(dosPath, true);

                  if (!Utils.IsNullOrWhiteSpace(file))
                     foreach (string drive in Directory.EnumerateLogicalDrivesCore(false, false).Select(drv => drv.Name).Where(drv => driveLetter.Equals(RemoveTrailingDirectorySeparator(drv, false), StringComparison.OrdinalIgnoreCase)))
                        return CombineCore(false, Volume.GetUniqueVolumeNameForPath(drive), path, file);
               }

               break;
         }

         // To: "\\?\C:\path\filename.ext"
         return Utils.IsNullOrWhiteSpace(dosDevice)
            ? string.Empty
            : LongPathPrefix + DosDeviceToDosPath(dosDevice, null);

         #endregion // Older OperatingSystem
      }
   }
}
