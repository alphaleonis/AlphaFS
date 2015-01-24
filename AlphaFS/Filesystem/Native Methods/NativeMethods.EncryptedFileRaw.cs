using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Alphaleonis.Win32.Filesystem
{
   internal static partial class NativeMethods
   {
      /// <summary>Opens an encrypted file in order to backup (export) or restore (import) the file.</summary>
      /// <param name="lpFileName">The name of the file to be opened.</param>
      /// <param name="ulFlags">The operation to be performed.</param>
      /// <param name="pvContext">[out] The address of a context block that must be presented in subsequent calls to
      /// ReadEncryptedFileRaw, WriteEncryptedFileRaw, or CloseEncryptedFileRaw.</param>
      /// <returns>
      ///   <para>
      ///     If the function succeeds, it returns ERROR_SUCCESS.
      ///   </para>
      ///   <para>
      ///     If the function fails, it returns a nonzero error code defined in WinError.h. You can use FormatMessage with
      ///     the FORMAT_MESSAGE_FROM_SYSTEM flag to get a generic text description of the error.
      ///   </para>
      /// </returns>
      [DllImport("Advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      public static extern int OpenEncryptedFileRaw(string lpFileName, EncryptedFileRawMode ulFlags, out SafeEncryptedFileRawHandle pvContext);

      /// <summary>
      ///   Closes an encrypted file after a backup or restore operation, and frees associated system resources.
      /// </summary>
      /// <param name="pvContext">A pointer to a system-defined context block. The OpenEncryptedFileRaw function returns the
      /// context block.</param>
      [DllImport("Advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      public static extern void CloseEncryptedFileRaw(IntPtr pvContext);

      [DllImport("Advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      public static extern int ReadEncryptedFileRaw([MarshalAs(UnmanagedType.FunctionPtr)] EncryptedFileRawExportCallback pfExportCallback, IntPtr pvCallbackContext, SafeEncryptedFileRawHandle pvContext);

      [DllImport("Advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
      [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
      public static extern int WriteEncryptedFileRaw([MarshalAs(UnmanagedType.FunctionPtr)] EncryptedFileRawImportCallback pfExportCallback, IntPtr pvCallbackContext, SafeEncryptedFileRawHandle pvContext);

      public delegate int EncryptedFileRawExportCallback(IntPtr pbData, IntPtr pvCallbackContext, uint ulLength);
      public delegate int EncryptedFileRawImportCallback(IntPtr pbData, IntPtr pvCallbackContext, ref uint ulLength);

      /// <summary>Indicates the operation to be performed when opening a file using the OpenEncryptedFileRaw.</summary>
      [Flags]
      public enum EncryptedFileRawMode
      {
         /// <summary>Open the file for export (backup).</summary>
         CreateForExport = 0,

         /// <summary>The file is being opened for import (restore).</summary>
         CreateForImport = 1,

         /// <summary>Import (restore) a directory containing encrypted files. This must be combined with one of the previous two flags to indicate the operation.</summary>
         CreateForDir = 2,

         /// <summary>Overwrite a hidden file on import.</summary>
         OverwriteHidden = 4
      }

   }
}