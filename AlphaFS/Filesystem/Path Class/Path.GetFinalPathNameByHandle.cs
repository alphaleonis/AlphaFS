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
      /// <remarks>
      ///   A final path is the path that is returned when a path is fully resolved. For example, for a symbolic link named "C:\tmp\mydir" that
      ///   points to "D:\yourdir", the final path would be "D:\yourdir".
      /// </remarks>
      /// <param name="handle">Then handle to a <see cref="SafeFileHandle"/> instance.</param>
      /// <returns>Returns the final path as a string.</returns>
      [SecurityCritical]
      public static string GetFinalPathNameByHandle(SafeFileHandle handle)
      {
         return GetFinalPathNameByHandleInternal(handle, FinalPathFormats.None);
      }

      /// <summary>[AlphaFS] Retrieves the final path for the specified file, formatted as <see cref="FinalPathFormats"/>.</summary>
      /// <remarks>
      ///   A final path is the path that is returned when a path is fully resolved. For example, for a symbolic link named "C:\tmp\mydir" that
      ///   points to "D:\yourdir", the final path would be "D:\yourdir".
      /// </remarks>
      /// <param name="handle">Then handle to a <see cref="SafeFileHandle"/> instance.</param>
      /// <param name="finalPath">The final path, formatted as <see cref="FinalPathFormats"/></param>
      /// <returns>Returns the final path as a string.</returns>
      [SecurityCritical]
      public static string GetFinalPathNameByHandle(SafeFileHandle handle, FinalPathFormats finalPath)
      {
         return GetFinalPathNameByHandleInternal(handle, finalPath);
      }


      /// <summary>
      ///   [AlphaFS] Unified method GetFinalPathNameByHandleInternal() to retrieve the final path for the specified file, formatted as
      ///   <see cref="FinalPathFormats"/>.
      /// </summary>
      /// <remarks>
      ///   A final path is the path that is returned when a path is fully resolved. For example, for a symbolic link named "C:\tmp\mydir" that
      ///   points to "D:\yourdir", the final path would be "D:\yourdir". The string that is returned by this function uses the
      ///   <see cref="LongPathPrefix"/> syntax.
      /// </remarks>
      /// <param name="handle">Then handle to a <see cref="SafeFileHandle"/> instance.</param>
      /// <param name="finalPath">The final path, formatted as <see cref="FinalPathFormats"/></param>
      /// <returns>Returns the final path as a string.</returns>
      [SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults", MessageId = "Alphaleonis.Win32.Filesystem.NativeMethods.GetMappedFileName(System.IntPtr,Alphaleonis.Win32.SafeGlobalMemoryBufferHandle,System.Text.StringBuilder,System.UInt32)")]
      [SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults", MessageId = "Alphaleonis.Win32.Filesystem.NativeMethods.GetMappedFileName(System.IntPtr,Alphaleonis.Win32.Security.SafeLocalMemoryBufferHandle,System.Text.StringBuilder,System.UInt32)")]
      [SecurityCritical]
      internal static string GetFinalPathNameByHandleInternal(SafeFileHandle handle, FinalPathFormats finalPath)
      {
         NativeMethods.IsValidHandle(handle);

         var buffer = new StringBuilder(NativeMethods.MaxPathUnicode);


         // ChangeErrorMode is for the Win32 SetThreadErrorMode() method, used to suppress possible pop-ups.
         using (new NativeMethods.ChangeErrorMode(NativeMethods.ErrorMode.FailCriticalErrors))
         {
            if (NativeMethods.IsAtLeastWindowsVista)
            {
               if (NativeMethods.GetFinalPathNameByHandle(handle, buffer, (uint)buffer.Capacity, finalPath) == Win32Errors.ERROR_SUCCESS)
                  // Throws IOException.
                  NativeError.ThrowException(Marshal.GetLastWin32Error(), true);

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
                  string path = GetSuffixedDirectoryNameWithoutRootInternal(null, dosPath);
                  string driveLetter = RemoveDirectorySeparator(GetPathRoot(dosPath, false), false);
                  string file = GetFileName(dosPath, true);

                  if (!Utils.IsNullOrWhiteSpace(file))
                     foreach (string drive in Directory.EnumerateLogicalDrivesInternal(false, false).Select(drv => drv.Name).Where(drv => driveLetter.Equals(RemoveDirectorySeparator(drv, false), StringComparison.OrdinalIgnoreCase)))
                        return CombineInternal(false, Volume.GetUniqueVolumeNameForPath(drive), path, file);
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
