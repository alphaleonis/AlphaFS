using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.AccessControl;

namespace Alphaleonis.Win32.Filesystem
{
   public static partial class File
   {
      #region GetSize

      /// <summary>[AlphaFS] Retrieves the file size, in bytes to store a specified file.</summary>
      /// <param name="path">The path to the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>The file size, in bytes.</returns>      
      [SecurityCritical]
      public static long GetSize(string path, PathFormat pathFormat)
      {         
         return GetSizeInternal(null, null, path, pathFormat);
      }

      /// <summary>[AlphaFS] Retrieves the file size, in bytes to store a specified file.</summary>
      /// <param name="path">The path to the file.</param>
      /// <returns>The file size, in bytes.</returns>      
      [SecurityCritical]
      public static long GetSize(string path)
      {
         return GetSizeInternal(null, null, path, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Retrieves the file size, in bytes to store a specified file.</summary>
      /// <param name="handle">The <see cref="SafeFileHandle"/> to the file.</param>
      /// <returns>The file size, in bytes.</returns>      
      [SecurityCritical]
      public static long GetSize(SafeFileHandle handle)
      {
         return GetSizeInternal(null, handle, null, PathFormat.LongFullPath);
      }

      /// <summary>[AlphaFS] Retrieves the file size, in bytes to store a specified file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>The number of bytes of disk storage used to store the specified file.</returns>
      [SecurityCritical]
      public static long GetSize(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         return GetSizeInternal(transaction, null, path, pathFormat);
      }

      /// <summary>[AlphaFS] Retrieves the file size, in bytes to store a specified file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to the file.</param>
      /// <returns>The number of bytes of disk storage used to store the specified file.</returns>
      [SecurityCritical]
      public static long GetSize(KernelTransaction transaction, string path)
      {
         return GetSizeInternal(transaction, null, path, PathFormat.RelativePath);
      }

      #endregion // GetSize

      #region Internal Methods

      /// <summary>[AlphaFS] Unified method GetSizeInternal() to retrieve the file size, in bytes to store a specified file.</summary>
      /// <remarks>Use either <paramref name="path"/> or <paramref name="safeHandle"/>, not both.</remarks>
      /// <param name="transaction">The transaction.</param>
      /// <param name="safeHandle">The <see cref="SafeFileHandle"/> to the file.</param>
      /// <param name="path">The path to the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>The number of bytes of disk storage used to store the specified file.</returns>
      [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
      [SecurityCritical]
      internal static long GetSizeInternal(KernelTransaction transaction, SafeFileHandle safeHandle, string path, PathFormat pathFormat)
      {
         bool callerHandle = safeHandle != null;
         if (!callerHandle)
         {
            string pathLp = Path.GetExtendedLengthPathInternal(transaction, path, pathFormat, GetFullPathOptions.RemoveTrailingDirectorySeparator | GetFullPathOptions.FullCheck);

            safeHandle = CreateFileInternal(transaction, pathLp, ExtendedFileAttributes.None, null, FileMode.Open, FileSystemRights.ReadData, FileShare.Read, true, PathFormat.LongFullPath);
         }


         long fileSize;

         try
         {
            NativeMethods.GetFileSizeEx(safeHandle, out fileSize);
         }
         finally
         {
            // Handle is ours, dispose.
            if (!callerHandle && safeHandle != null)
               safeHandle.Close();
         }

         return fileSize;
      }

      #endregion // GetSizeInternal
   }
}
