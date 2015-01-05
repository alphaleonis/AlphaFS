using Alphaleonis.Win32.Security;
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
      #region Public Methods

      /// <summary>Gets the <see cref="FileAttributes"/> of the file on the path.</summary>
      /// <param name="path">The path to the file.</param>
      /// <returns>The <see cref="FileAttributes"/> of the file on the path.</returns>
      [SecurityCritical]
      public static FileAttributes GetAttributes(string path)
      {
         return GetAttributesExInternal<FileAttributes>(null, path, PathFormat.Auto);
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
         return GetAttributesExInternal<FileAttributes>(transaction, path, PathFormat.Auto);
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
         if (pathFormat == PathFormat.Auto)
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

      #endregion // GetAttributesExInternal
   }
}
