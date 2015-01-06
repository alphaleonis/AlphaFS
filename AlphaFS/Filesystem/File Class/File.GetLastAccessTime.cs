using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;

namespace Alphaleonis.Win32.Filesystem
{
   public static partial class File
   {
      #region GetLastAccessTime

      /// <summary>Gets the date and time that the specified file was last accessed.</summary>
      /// <param name="path">The file for which to obtain access date and time information.</param>
      /// <returns>
      ///   A <see cref="System.DateTime"/> structure set to the date and time that the specified file was last accessed. This value is
      ///   expressed in local time.
      /// </returns>
      [SecurityCritical]
      public static DateTime GetLastAccessTime(string path)
      {
         return GetLastAccessTimeInternal(null, path, false, PathFormat.RelativeOrFullPath).ToLocalTime();
      }

      /// <summary>[AlphaFS] Gets the date and time that the specified file was last accessed.</summary>
      /// <param name="path">The file for which to obtain access date and time information.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>
      ///   A <see cref="System.DateTime"/> structure set to the date and time that the specified file was last accessed. This value is
      ///   expressed in local time.
      /// </returns>
      [SecurityCritical]
      public static DateTime GetLastAccessTime(string path, PathFormat pathFormat)
      {
         return GetLastAccessTimeInternal(null, path, false, pathFormat).ToLocalTime();
      }


      #region Transacted

      /// <summary>[AlphaFS] Gets the date and time that the specified file was last accessed.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file for which to obtain access date and time information.</param>
      /// <returns>
      ///   A <see cref="System.DateTime"/> structure set to the date and time that the specified file was last accessed. This value is
      ///   expressed in local time.
      /// </returns>
      [SecurityCritical]
      public static DateTime GetLastAccessTime(KernelTransaction transaction, string path)
      {
         return GetLastAccessTimeInternal(transaction, path, false, PathFormat.RelativeOrFullPath).ToLocalTime();
      }

      /// <summary>[AlphaFS] Gets the date and time that the specified file was last accessed.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file for which to obtain access date and time information.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>
      ///   A <see cref="System.DateTime"/> structure set to the date and time that the specified file was last accessed. This value is
      ///   expressed in local time.
      /// </returns>
      [SecurityCritical]
      public static DateTime GetLastAccessTime(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         return GetLastAccessTimeInternal(transaction, path, false, pathFormat).ToLocalTime();
      }


      #endregion // Transacted

      #endregion

      #region GetLastAccessTimeUtc

      /// <summary>Gets the date and time, in coordinated universal time (UTC), that the specified file was last accessed.</summary>
      /// <param name="path">The file for which to obtain access date and time information.</param>
      /// <returns>
      ///   A <see cref="System.DateTime"/> structure set to the date and time that the specified file was last accessed. This value is
      ///   expressed in UTC time.
      /// </returns>
      [SecurityCritical]
      public static DateTime GetLastAccessTimeUtc(string path)
      {
         return GetLastAccessTimeInternal(null, path, true, PathFormat.RelativeOrFullPath);
      }

      /// <summary>[AlphaFS] Gets the date and time, in coordinated universal time (UTC), that the specified file was last accessed.</summary>
      /// <param name="path">The file for which to obtain access date and time information.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>
      ///   A <see cref="System.DateTime"/> structure set to the date and time that the specified file was last accessed. This value is
      ///   expressed in UTC time.
      /// </returns>
      [SecurityCritical]
      public static DateTime GetLastAccessTimeUtc(string path, PathFormat pathFormat)
      {
         return GetLastAccessTimeInternal(null, path, true, pathFormat);
      }


      #region Transacted

      /// <summary>[AlphaFS] Gets the date and time, in coordinated universal time (UTC), that the specified file was last accessed.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file for which to obtain access date and time information.</param>
      /// <returns>
      ///   A <see cref="System.DateTime"/> structure set to the date and time that the specified file was last accessed. This value is
      ///   expressed in UTC time.
      /// </returns>
      [SecurityCritical]
      public static DateTime GetLastAccessTimeUtc(KernelTransaction transaction, string path)
      {
         return GetLastAccessTimeInternal(transaction, path, true, PathFormat.RelativeOrFullPath);
      }

      /// <summary>[AlphaFS] Gets the date and time, in coordinated universal time (UTC), that the specified file was last accessed.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file for which to obtain access date and time information.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>
      ///   A <see cref="System.DateTime"/> structure set to the date and time that the specified file was last accessed. This value is
      ///   expressed in UTC time.
      /// </returns>
      [SecurityCritical]
      public static DateTime GetLastAccessTimeUtc(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         return GetLastAccessTimeInternal(transaction, path, true, pathFormat);
      }

      #endregion // Transacted

      #endregion // GetLastAccessTimeUtc

      #region Internal Methods

      /// <summary>
      ///   [AlphaFS] Gets the date and time, in coordinated universal time (UTC) or local time, that the specified file or directory was last
      ///   accessed.
      /// </summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file or directory for which to obtain access date and time information.</param>
      /// <param name="returnUtc">
      ///   <see langword="true"/> gets the Coordinated Universal Time (UTC), <see langword="false"/> gets the local time.
      /// </param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>
      ///   A <see cref="System.DateTime"/> structure set to the date and time that the specified file or directory was last accessed.
      ///   Depending on <paramref name="returnUtc"/> this value is expressed in UTC- or local time.
      /// </returns>
      [SecurityCritical]
      internal static DateTime GetLastAccessTimeInternal(KernelTransaction transaction, string path, bool returnUtc, PathFormat pathFormat)
      {
         NativeMethods.FileTime lastAccessTime = GetAttributesExInternal<NativeMethods.Win32FileAttributeData>(transaction, path, pathFormat).LastAccessTime;

         return returnUtc
            ? DateTime.FromFileTimeUtc(lastAccessTime)
            : DateTime.FromFileTime(lastAccessTime);
      }

      #endregion // GetLastAccessTimeInternal
   }
}
