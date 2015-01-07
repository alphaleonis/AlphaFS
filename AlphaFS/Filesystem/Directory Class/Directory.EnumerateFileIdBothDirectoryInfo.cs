using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.AccessControl;

namespace Alphaleonis.Win32.Filesystem
{
   public static partial class Directory
   {
      #region EnumerateFileIdBothDirectoryInfo

      #region Non-Transactional

      /// <summary>[AlphaFS] Retrieves information about files in the directory specified by <paramref name="path"/> in <see cref="FileShare.ReadWrite"/> mode.</summary>
      /// <param name="path">A path to a directory from which to retrieve information.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>An enumeration of <see cref="FileIdBothDirectoryInfo"/> records for each file system entry in the specified diretory.</returns>
      [SecurityCritical]
      public static IEnumerable<FileIdBothDirectoryInfo> EnumerateFileIdBothDirectoryInfo(string path, PathFormat pathFormat)
      {
         return EnumerateFileIdBothDirectoryInfoInternal(null, null, path, FileShare.ReadWrite, false, pathFormat);
      }

      /// <summary>[AlphaFS] Retrieves information about files in the directory specified by <paramref name="path"/> in specified <see cref="FileShare"/> mode.</summary>
      /// <param name="path">A path to a directory from which to retrieve information.</param>
      /// <param name="shareMode">The <see cref="FileShare"/> mode with which to open a handle to the directory.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>An enumeration of <see cref="FileIdBothDirectoryInfo"/> records for each file system entry in the specified diretory.</returns>      
      [SecurityCritical]
      public static IEnumerable<FileIdBothDirectoryInfo> EnumerateFileIdBothDirectoryInfo(string path, FileShare shareMode, PathFormat pathFormat)
      {
         return EnumerateFileIdBothDirectoryInfoInternal(null, null, path, shareMode, false, pathFormat);
      }

      /// <summary>[AlphaFS] Retrieves information about files in the directory specified by <paramref name="path"/> in <see cref="FileShare.ReadWrite"/> mode.</summary>
      /// <param name="path">A path to a directory from which to retrieve information.</param>
      /// <returns>An enumeration of <see cref="FileIdBothDirectoryInfo"/> records for each file system entry in the specified diretory.</returns>
      [SecurityCritical]
      public static IEnumerable<FileIdBothDirectoryInfo> EnumerateFileIdBothDirectoryInfo(string path)
      {
         return EnumerateFileIdBothDirectoryInfoInternal(null, null, path, FileShare.ReadWrite, false, PathFormat.Relative);
      }

      /// <summary>[AlphaFS] Retrieves information about files in the directory specified by <paramref name="path"/> in specified <see cref="FileShare"/> mode.</summary>
      /// <param name="path">A path to a directory from which to retrieve information.</param>
      /// <param name="shareMode">The <see cref="FileShare"/> mode with which to open a handle to the directory.</param>
      /// <returns>An enumeration of <see cref="FileIdBothDirectoryInfo"/> records for each file system entry in the specified diretory.</returns>      
      [SecurityCritical]
      public static IEnumerable<FileIdBothDirectoryInfo> EnumerateFileIdBothDirectoryInfo(string path, FileShare shareMode)
      {
         return EnumerateFileIdBothDirectoryInfoInternal(null, null, path, shareMode, false, PathFormat.Relative);
      }

      /// <summary>[AlphaFS] Retrieves information about files in the directory handle specified.</summary>
      /// <param name="handle">An open handle to the directory from which to retrieve information.</param>
      /// <returns>An IEnumerable of <see cref="FileIdBothDirectoryInfo"/> records for each file system entry in the specified diretory.</returns>    
      [SecurityCritical]
      public static IEnumerable<FileIdBothDirectoryInfo> EnumerateFileIdBothDirectoryInfo(SafeFileHandle handle)
      {
         // FileShare has no effect since a handle is already opened.
         return EnumerateFileIdBothDirectoryInfoInternal(null, handle, null, FileShare.ReadWrite, false, PathFormat.Relative);
      }
      #endregion

      #region Transactional

      /// <summary>[AlphaFS] Retrieves information about files in the directory specified by <paramref name="path"/> in <see cref="FileShare.ReadWrite"/> mode.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">A path to a directory from which to retrieve information.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>An enumeration of <see cref="FileIdBothDirectoryInfo"/> records for each file system entry in the specified diretory.</returns>
      [SecurityCritical]
      public static IEnumerable<FileIdBothDirectoryInfo> EnumerateFileIdBothDirectoryInfo(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         return EnumerateFileIdBothDirectoryInfoInternal(transaction, null, path, FileShare.ReadWrite, false, pathFormat);
      }

      /// <summary>[AlphaFS] Retrieves information about files in the directory specified by <paramref name="path"/> in specified <see cref="FileShare"/> mode.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">A path to a directory from which to retrieve information.</param>
      /// <param name="shareMode">The <see cref="FileShare"/> mode with which to open a handle to the directory.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>An enumeration of <see cref="FileIdBothDirectoryInfo"/> records for each file system entry in the specified diretory.</returns>
      [SecurityCritical]
      public static IEnumerable<FileIdBothDirectoryInfo> EnumerateFileIdBothDirectoryInfo(KernelTransaction transaction, string path, FileShare shareMode, PathFormat pathFormat)
      {
         return EnumerateFileIdBothDirectoryInfoInternal(transaction, null, path, shareMode, false, pathFormat);
      }

      /// <summary>[AlphaFS] Retrieves information about files in the directory specified by <paramref name="path"/> in <see cref="FileShare.ReadWrite"/> mode.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">A path to a directory from which to retrieve information.</param>
      /// <returns>An enumeration of <see cref="FileIdBothDirectoryInfo"/> records for each file system entry in the specified diretory.</returns>
      [SecurityCritical]
      public static IEnumerable<FileIdBothDirectoryInfo> EnumerateFileIdBothDirectoryInfo(KernelTransaction transaction, string path)
      {
         return EnumerateFileIdBothDirectoryInfoInternal(transaction, null, path, FileShare.ReadWrite, false, PathFormat.Relative);
      }

      /// <summary>[AlphaFS] Retrieves information about files in the directory specified by <paramref name="path"/> in specified <see cref="FileShare"/> mode.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">A path to a directory from which to retrieve information.</param>
      /// <param name="shareMode">The <see cref="FileShare"/> mode with which to open a handle to the directory.</param>
      /// <returns>An enumeration of <see cref="FileIdBothDirectoryInfo"/> records for each file system entry in the specified diretory.</returns>
      [SecurityCritical]
      public static IEnumerable<FileIdBothDirectoryInfo> EnumerateFileIdBothDirectoryInfo(KernelTransaction transaction, string path, FileShare shareMode)
      {
         return EnumerateFileIdBothDirectoryInfoInternal(transaction, null, path, shareMode, false, PathFormat.Relative);
      }

      #endregion // Transacted

      #endregion // EnumerateFileIdBothDirectoryInfo

      #region Internal Methods

      /// <summary>[AlphaFS] Unified method EnumerateFileIdBothDirectoryInfoInternal() to return an enumerable collection of information about files in the directory handle specified.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="safeHandle">An open handle to the directory from which to retrieve information.</param>
      /// <param name="path">A path to the directory.</param>
      /// <param name="shareMode">The <see cref="FileShare"/> mode with which to open a handle to the directory.</param>
      /// <param name="continueOnException">
      /// <para><see langword="true"/> suppress any Exception that might be thrown a result from a failure,</para>
      /// <para>such as ACLs protected directories or non-accessible reparse points.</para>
      /// </param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>An IEnumerable of <see cref="FileIdBothDirectoryInfo"/> records for each file system entry in the specified diretory.</returns>    
      /// <remarks>Either use <paramref name="path"/> or <paramref name="safeHandle"/>, not both.</remarks>
      [SecurityCritical]
      internal static IEnumerable<FileIdBothDirectoryInfo> EnumerateFileIdBothDirectoryInfoInternal(KernelTransaction transaction, SafeFileHandle safeHandle, string path, FileShare shareMode, bool continueOnException, PathFormat pathFormat)
      {
         if (!NativeMethods.IsAtLeastWindowsVista)
            throw new PlatformNotSupportedException(Resources.RequiresWindowsVistaOrHigher);

         bool callerHandle = safeHandle != null;
         if (!callerHandle)
         {
            if (Utils.IsNullOrWhiteSpace(path))
               throw new ArgumentNullException("path");

            string pathLp = Path.GetExtendedLengthPathInternal(transaction, path, pathFormat, GetFullPathOptions.RemoveTrailingDirectorySeparator | GetFullPathOptions.CheckInvalidPathChars | GetFullPathOptions.CheckAdditional);
            safeHandle = File.CreateFileInternal(transaction, pathLp, ExtendedFileAttributes.BackupSemantics, null, FileMode.Open, FileSystemRights.ReadData, shareMode, true, PathFormat.LongFullPath);
         }


         try
         {
            if (!NativeMethods.IsValidHandle(safeHandle, Marshal.GetLastWin32Error(), !continueOnException))
               yield break;

            // 2014-10-16: Number of returned items depends on the size of the buffer.
            // That does not seem right, investigate.
            using (SafeGlobalMemoryBufferHandle safeBuffer = new SafeGlobalMemoryBufferHandle(NativeMethods.DefaultFileBufferSize))
            {
               NativeMethods.IsValidHandle(safeBuffer, Marshal.GetLastWin32Error());

               long fileNameOffset = Marshal.OffsetOf(typeof(NativeMethods.FileIdBothDirInfo), "FileName").ToInt64();

               while (NativeMethods.GetFileInformationByHandleEx(safeHandle, NativeMethods.FileInfoByHandleClass.FileIdBothDirectoryInfo, safeBuffer, NativeMethods.DefaultFileBufferSize))
               {
                  // CA2001:AvoidCallingProblematicMethods

                  IntPtr buffer = IntPtr.Zero;
                  bool successRef = false;
                  safeBuffer.DangerousAddRef(ref successRef);

                  // MSDN: The DangerousGetHandle method poses a security risk because it can return a handle that is not valid.
                  if (successRef)
                     buffer = safeBuffer.DangerousGetHandle();

                  safeBuffer.DangerousRelease();

                  if (buffer == IntPtr.Zero)
                     NativeError.ThrowException(Resources.HandleDangerousRef);

                  // CA2001:AvoidCallingProblematicMethods


                  while (buffer != IntPtr.Zero)
                  {
                     NativeMethods.FileIdBothDirInfo fibdi = Utils.MarshalPtrToStructure<NativeMethods.FileIdBothDirInfo>(0, buffer);

                     string fileName = Marshal.PtrToStringUni(new IntPtr(fileNameOffset + buffer.ToInt64()), (int)(fibdi.FileNameLength / 2));

                     if (!Utils.IsNullOrWhiteSpace(fileName) &&
                         !fileName.Equals(Path.CurrentDirectoryPrefix, StringComparison.OrdinalIgnoreCase) &&
                         !fileName.Equals(Path.ParentDirectoryPrefix, StringComparison.OrdinalIgnoreCase))
                        yield return new FileIdBothDirectoryInfo(fibdi, fileName);


                     buffer = fibdi.NextEntryOffset == 0
                        ? IntPtr.Zero
                        : new IntPtr(buffer.ToInt64() + fibdi.NextEntryOffset);
                  }
               }

               int lastError = Marshal.GetLastWin32Error();
               switch ((uint)lastError)
               {
                  case Win32Errors.ERROR_SUCCESS:
                  case Win32Errors.ERROR_NO_MORE_FILES:
                  case Win32Errors.ERROR_HANDLE_EOF:
                     yield break;

                  default:
                     NativeError.ThrowException(lastError, path);
                     break;
               }
            }
         }
         finally
         {
            // Handle is ours, dispose.
            if (!callerHandle && safeHandle != null)
               safeHandle.Close();
         }
      }

      #endregion // EnumerateFileIdBothDirectoryInfoInternal
   }
}
