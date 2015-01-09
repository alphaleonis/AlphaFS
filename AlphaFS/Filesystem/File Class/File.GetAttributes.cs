using Alphaleonis.Win32.Security;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.AccessControl;

namespace Alphaleonis.Win32.Filesystem
{
   public static partial class File
   {
      #region GetAttributes

      /// <summary>Gets the <see cref="FileAttributes"/> of the file on the path.</summary>
      /// <param name="path">The path to the file.</param>
      /// <returns>The <see cref="FileAttributes"/> of the file on the path.</returns>
      [SecurityCritical]
      public static FileAttributes GetAttributes(string path)
      {
         return GetAttributesExInternal<FileAttributes>(null, path, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Gets the <see cref="FileAttributes"/> of the file on the path.</summary>
      /// <param name="path">The path to the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>The <see cref="FileAttributes"/> of the file on the path.</returns>
      [SecurityCritical]
      public static FileAttributes GetAttributes(string path, PathFormat pathFormat)
      {
         return GetAttributesExInternal<FileAttributes>(null, path, pathFormat);
      }

      /// <summary>[AlphaFS] Gets the <see cref="FileAttributes"/> of the file on the path.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to the file.</param>
      /// <returns>The <see cref="FileAttributes"/> of the file on the path.</returns>
      [SecurityCritical]
      public static FileAttributes GetAttributes(KernelTransaction transaction, string path)
      {
         return GetAttributesExInternal<FileAttributes>(transaction, path, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Gets the <see cref="FileAttributes"/> of the file on the path.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>The <see cref="FileAttributes"/> of the file on the path.</returns>
      [SecurityCritical]
      public static FileAttributes GetAttributes(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         return GetAttributesExInternal<FileAttributes>(transaction, path, pathFormat);
      }

      #endregion

      #region Internal Methods

      /// <summary>
      ///   [AlphaFS] Gets the <see cref="FileAttributes"/> or <see cref="Alphaleonis.Win32.Filesystem.NativeMethods.Win32FileAttributeData"/>
      ///   of the specified file or directory.
      /// </summary>
      /// <typeparam name="T">Generic type parameter.</typeparam>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to the file or directory.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>
      ///   Returns the <see cref="FileAttributes"/> or <see cref="Alphaleonis.Win32.Filesystem.NativeMethods.Win32FileAttributeData"/> of the
      ///   specified file or directory.
      /// </returns>
      [SuppressMessage("Microsoft.Interoperability", "CA1404:CallGetLastErrorImmediatelyAfterPInvoke", Justification = "Marshal.GetLastWin32Error() is manipulated.")]
      [SecurityCritical]
      internal static T GetAttributesExInternal<T>(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         if (pathFormat == PathFormat.RelativePath)
            Path.CheckValidPath(path, true, true);

         string pathLp = Path.GetExtendedLengthPathInternal(transaction, path, pathFormat, GetFullPathOptions.RemoveTrailingDirectorySeparator | GetFullPathOptions.CheckInvalidPathChars);

         var data = new NativeMethods.Win32FileAttributeData();
         int dataInitialised = FillAttributeInfoInternal(transaction, pathLp, ref data, false, true);

         if (dataInitialised != Win32Errors.ERROR_SUCCESS)
            // Throws IOException.
            NativeError.ThrowException(dataInitialised, pathLp, true);

         return (typeof(T) == typeof(FileAttributes)
            ? (T)(object)data.FileAttributes
            : (T)(object)data);
      }

      /// <summary>
      ///   Calls NativeMethods.GetFileAttributesEx to retrieve Win32FileAttributeData.
      ///   <para>Note that classes should use -1 as the uninitialized state for dataInitialized when relying on this method.</para>
      /// </summary>
      /// <remarks>No path (null, empty string) checking or normalization is performed.</remarks>
      /// <param name="transaction">.</param>
      /// <param name="pathLp">.</param>
      /// <param name="win32AttrData">[in,out].</param>
      /// <param name="tryagain">.</param>
      /// <param name="returnErrorOnNotFound">.</param>
      /// <returns>Returns 0 on success, otherwise a Win32 error code.</returns>
      [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
      [SecurityCritical]
      internal static int FillAttributeInfoInternal(KernelTransaction transaction, string pathLp, ref NativeMethods.Win32FileAttributeData win32AttrData, bool tryagain, bool returnErrorOnNotFound)
      {
         int dataInitialised = (int)Win32Errors.ERROR_SUCCESS;

         #region Try Again

         // Someone has a handle to the file open, or other error.
         if (tryagain)
         {
            NativeMethods.Win32FindData findData;

            // ChangeErrorMode is for the Win32 SetThreadErrorMode() method, used to suppress possible pop-ups.
            using (new NativeMethods.ChangeErrorMode(NativeMethods.ErrorMode.FailCriticalErrors))
            {
               bool error = false;

               SafeFindFileHandle handle = transaction == null || !NativeMethods.IsAtLeastWindowsVista

                  // FindFirstFileEx() / FindFirstFileTransacted()
                  // In the ANSI version of this function, the name is limited to MAX_PATH characters.
                  // To extend this limit to 32,767 wide characters, call the Unicode version of the function and prepend "\\?\" to the path.
                  // 2013-01-13: MSDN confirms LongPath usage.

                  // A trailing backslash is not allowed.
                  ? NativeMethods.FindFirstFileEx(Path.RemoveTrailingDirectorySeparator(pathLp, false), NativeMethods.FindExInfoLevel, out findData, NativeMethods.FindExSearchOps.SearchNameMatch, IntPtr.Zero, NativeMethods.LargeCache)
                  : NativeMethods.FindFirstFileTransacted(Path.RemoveTrailingDirectorySeparator(pathLp, false), NativeMethods.FindExInfoLevel, out findData, NativeMethods.FindExSearchOps.SearchNameMatch, IntPtr.Zero, NativeMethods.LargeCache, transaction.SafeHandle);

               try
               {
                  if (handle.IsInvalid)
                  {
                     error = true;
                     dataInitialised = Marshal.GetLastWin32Error();

                     if (dataInitialised == Win32Errors.ERROR_FILE_NOT_FOUND ||
                         dataInitialised == Win32Errors.ERROR_PATH_NOT_FOUND ||
                         dataInitialised == Win32Errors.ERROR_NOT_READY) // Floppy device not ready.
                     {
                        if (!returnErrorOnNotFound)
                        {
                           // Return default value for backward compatibility
                           dataInitialised = (int)Win32Errors.ERROR_SUCCESS;
                           win32AttrData.FileAttributes = (FileAttributes)(-1);
                        }
                     }

                     return dataInitialised;
                  }
               }
               finally
               {
                  try
                  {
                     // Close the Win32 handle.
                     handle.Close();
                  }
                  catch
                  {
                     // If we're already returning an error, don't throw another one.
                     if (!error)
                        NativeError.ThrowException(dataInitialised, pathLp, true);
                  }
               }
            }

            // Copy the attribute information.
            win32AttrData = new NativeMethods.Win32FileAttributeData(findData);
         }

         #endregion // Try Again

         else
         {
            using (new NativeMethods.ChangeErrorMode(NativeMethods.ErrorMode.FailCriticalErrors))
            {
               if (!(transaction == null || !NativeMethods.IsAtLeastWindowsVista

                  // GetFileAttributesEx() / GetFileAttributesTransacted()
                  // In the ANSI version of this function, the name is limited to MAX_PATH characters.
                  // To extend this limit to 32,767 wide characters, call the Unicode version of the function and prepend "\\?\" to the path.
                  // 2013-01-13: MSDN confirms LongPath usage.

                  ? NativeMethods.GetFileAttributesEx(pathLp, NativeMethods.GetFileExInfoLevels.GetFileExInfoStandard, out win32AttrData)
                  : NativeMethods.GetFileAttributesTransacted(pathLp, NativeMethods.GetFileExInfoLevels.GetFileExInfoStandard, out win32AttrData, transaction.SafeHandle)))
               {
                  dataInitialised = Marshal.GetLastWin32Error();

                  if (dataInitialised != Win32Errors.ERROR_FILE_NOT_FOUND &&
                      dataInitialised != Win32Errors.ERROR_PATH_NOT_FOUND &&
                      dataInitialised != Win32Errors.ERROR_NOT_READY) // Floppy device not ready.
                  {
                     // In case someone latched onto the file. Take the perf hit only for failure.
                     return FillAttributeInfoInternal(transaction, pathLp, ref win32AttrData, true, returnErrorOnNotFound);
                  }

                  if (!returnErrorOnNotFound)
                  {
                     // Return default value for backward compbatibility.
                     dataInitialised = (int)Win32Errors.ERROR_SUCCESS;
                     win32AttrData.FileAttributes = (FileAttributes)(-1);
                  }
               }
            }
         }

         return dataInitialised;
      }


      #endregion // GetAttributesExInternal
   }
}
