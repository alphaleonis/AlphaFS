using Microsoft.Win32.SafeHandles;
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
      #region GetChangeTime

      /// <summary>Gets the change date and time of the specified file.</summary>
      /// <param name="path">The file for which to obtain creation date and time information.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>
      ///   A <see cref="System.DateTime"/> structure set to the change date and time for the specified file. This value is expressed in local
      ///   time.
      /// </returns>
      [SecurityCritical]
      public static DateTime GetChangeTime(string path, PathFormat pathFormat)
      {
         return GetChangeTimeInternal(false, null, null, path, false, pathFormat);
      }


      /// <summary>Gets the change date and time of the specified file.</summary>
      /// <param name="path">The file for which to obtain creation date and time information.</param>
      /// <returns>
      ///   A <see cref="System.DateTime"/> structure set to the change date and time for the specified file. This value is expressed in local
      ///   time.
      /// </returns>
      [SecurityCritical]
      public static DateTime GetChangeTime(string path)
      {
         return GetChangeTimeInternal(false, null, null, path, false, PathFormat.RelativeOrFullPath);
      }

      /// <summary>Gets the change date and time of the specified file.</summary>
      /// <param name="safeHandle">An open handle to the file or directory from which to retrieve information.</param>
      /// <returns>
      ///   A <see cref="System.DateTime"/> structure set to the change date and time for the specified file. This value is expressed in local
      ///   time.
      /// </returns>
      [SecurityCritical]
      public static DateTime GetChangeTime(SafeFileHandle safeHandle)
      {
         return GetChangeTimeInternal(false, null, safeHandle, null, false, PathFormat.LongFullPath);
      }

      /// <summary>Gets the change date and time of the specified file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file for which to obtain creation date and time information.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>
      ///   A <see cref="System.DateTime"/> structure set to the change date and time for the specified file. This value is expressed in local
      ///   time.
      /// </returns>
      [SecurityCritical]
      public static DateTime GetChangeTime(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         return GetChangeTimeInternal(false, transaction, null, path, false, pathFormat);
      }

      /// <summary>Gets the change date and time of the specified file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file for which to obtain creation date and time information.</param>
      /// <returns>
      ///   A <see cref="System.DateTime"/> structure set to the change date and time for the specified file. This value is expressed in local
      ///   time.
      /// </returns>
      [SecurityCritical]
      public static DateTime GetChangeTime(KernelTransaction transaction, string path)
      {
         return GetChangeTimeInternal(false, transaction, null, path, false, PathFormat.RelativeOrFullPath);
      }

      #endregion // GetChangeTime

      #region GetChangeTimeUtc

      /// <summary>Gets the change date and time, in Coordinated Universal Time (UTC) format, of the specified file.</summary>
      /// <param name="path">The file for which to obtain change date and time information, in Coordinated Universal Time (UTC) format.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>
      ///   A <see cref="System.DateTime"/> structure set to the change date and time for the specified file. This value is expressed in UTC
      ///   time.
      /// </returns>
      [SecurityCritical]
      public static DateTime GetChangeTimeUtc(string path, PathFormat pathFormat)
      {
         return GetChangeTimeInternal(false, null, null, path, true, pathFormat);
      }

      /// <summary>Gets the change date and time, in Coordinated Universal Time (UTC) format, of the specified file.</summary>
      /// <param name="path">The file for which to obtain change date and time information, in Coordinated Universal Time (UTC) format.</param>
      /// <returns>
      ///   A <see cref="System.DateTime"/> structure set to the change date and time for the specified file. This value is expressed in UTC
      ///   time.
      /// </returns>
      [SecurityCritical]
      public static DateTime GetChangeTimeUtc(string path)
      {
         return GetChangeTimeInternal(false, null, null, path, true, PathFormat.RelativeOrFullPath);
      }

      /// <summary>Gets the change date and time, in Coordinated Universal Time (UTC) format, of the specified file.</summary>
      /// <param name="safeHandle">An open handle to the file or directory from which to retrieve information.</param>
      /// <returns>
      ///   A <see cref="System.DateTime"/> structure set to the change date and time for the specified file. This value is expressed in UTC
      ///   time.
      /// </returns>
      [SecurityCritical]
      public static DateTime GetChangeTimeUtc(SafeFileHandle safeHandle)
      {
         return GetChangeTimeInternal(false, null, safeHandle, null, true, PathFormat.LongFullPath);
      }

      /// <summary>Gets the change date and time, in Coordinated Universal Time (UTC) format, of the specified file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file for which to obtain change date and time information, in Coordinated Universal Time (UTC) format.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>
      ///   A <see cref="System.DateTime"/> structure set to the change date and time for the specified file. This value is expressed in UTC
      ///   time.
      /// </returns>
      [SecurityCritical]
      public static DateTime GetChangeTimeUtc(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         return GetChangeTimeInternal(false, transaction, null, path, true, pathFormat);
      }

      /// <summary>Gets the change date and time, in Coordinated Universal Time (UTC) format, of the specified file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file for which to obtain change date and time information, in Coordinated Universal Time (UTC) format.</param>
      /// <returns>
      ///   A <see cref="System.DateTime"/> structure set to the change date and time for the specified file. This value is expressed in UTC
      ///   time.
      /// </returns>
      [SecurityCritical]
      public static DateTime GetChangeTimeUtc(KernelTransaction transaction, string path)
      {
         return GetChangeTimeInternal(false, transaction, null, path, true, PathFormat.RelativeOrFullPath);
      }

      #endregion // GetChangeTimeUtc

      #region Internal Methods

      /// <summary>[AlphaFS] Unified method GetChangeTimeInternal() to get the change date and time of the specified file.</summary>
      /// <remarks><para>Use either <paramref name="path"/> or <paramref name="safeHandle"/>, not both.</para></remarks>
      /// <exception cref="PlatformNotSupportedException">Thrown when a Platform Not Supported error condition occurs.</exception>
      /// <exception cref="ArgumentNullException">Thrown when one or more required arguments are null.</exception>
      /// <param name="isFolder">Specifies that <paramref name="path"/> is a file or directory.</param>
      /// <param name="transaction">The transaction.</param>
      /// <param name="safeHandle">An open handle to the file or directory from which to retrieve information.</param>
      /// <param name="path">The file or directory for which to obtain creation date and time information.</param>
      /// <param name="getUtc">
      ///   <see langword="true"/> gets the Coordinated Universal Time (UTC), <see langword="false"/> gets the local time.
      /// </param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>
      ///   <para>Returns a <see cref="System.DateTime"/> structure set to the change date and time for the specified file.</para>
      ///   <para>This value is expressed in local time.</para>
      /// </returns>
      [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
      [SuppressMessage("Microsoft.Reliability", "CA2001:AvoidCallingProblematicMethods", MessageId = "System.Runtime.InteropServices.SafeHandle.DangerousGetHandle", Justification = "DangerousAddRef() and DangerousRelease() are applied.")]
      [SecurityCritical]
      internal static DateTime GetChangeTimeInternal(bool isFolder, KernelTransaction transaction, SafeFileHandle safeHandle, string path, bool getUtc, PathFormat pathFormat)
      {
         if (!NativeMethods.IsAtLeastWindowsVista)
            throw new PlatformNotSupportedException(Resources.RequiresWindowsVistaOrHigher);

         bool callerHandle = safeHandle != null;
         if (!callerHandle)
         {
            if (pathFormat != PathFormat.LongFullPath && Utils.IsNullOrWhiteSpace(path))
               throw new ArgumentNullException("path");

            string pathLp = Path.GetExtendedLengthPathInternal(transaction, path, pathFormat, GetFullPathOptions.RemoveTrailingDirectorySeparator | GetFullPathOptions.CheckInvalidPathChars | GetFullPathOptions.CheckAdditional);

            safeHandle = CreateFileInternal(transaction, pathLp, isFolder ? ExtendedFileAttributes.BackupSemantics : ExtendedFileAttributes.Normal, null, FileMode.Open, FileSystemRights.ReadData, FileShare.ReadWrite, true, PathFormat.LongFullPath);
         }


         try
         {
            NativeMethods.IsValidHandle(safeHandle);

            using (SafeGlobalMemoryBufferHandle safeBuffer = new SafeGlobalMemoryBufferHandle(NativeMethods.DefaultFileBufferSize))
            {
               NativeMethods.IsValidHandle(safeBuffer);

               if (!NativeMethods.GetFileInformationByHandleEx(safeHandle, NativeMethods.FileInfoByHandleClass.FileBasicInfo, safeBuffer, NativeMethods.DefaultFileBufferSize))
                  // Throws IOException.
                  NativeError.ThrowException(Marshal.GetLastWin32Error(), true);


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


               NativeMethods.FileTime changeTime = Utils.MarshalPtrToStructure<NativeMethods.FileBasicInfo>(0, buffer).ChangeTime;

               return getUtc
                  ? DateTime.FromFileTimeUtc(changeTime)
                  : DateTime.FromFileTime(changeTime);
            }
         }
         finally
         {
            // Handle is ours, dispose.
            if (!callerHandle && safeHandle != null)
               safeHandle.Close();
         }
      }

      #endregion // GetChangeTimeInternal
   }
}
