using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;

namespace Alphaleonis.Win32.Filesystem
{
   public static partial class File
   {
      #region GetCreationTime

      /// <summary>Gets the creation date and time of the specified file.</summary>
      /// <param name="path">The file for which to obtain creation date and time information.</param>
      /// <returns>
      ///   A <see cref="System.DateTime"/> structure set to the creation date and time for the specified file. This value is expressed in
      ///   local time.
      /// </returns>
      [SecurityCritical]
      public static DateTime GetCreationTime(string path)
      {
         return GetCreationTimeInternal(null, path, false, PathFormat.Auto).ToLocalTime();
      }

      /// <summary>[AlphaFS] Gets the creation date and time of the specified file.</summary>
      /// <param name="path">The file for which to obtain creation date and time information.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>
      ///   A <see cref="System.DateTime"/> structure set to the creation date and time for the specified file. This value is expressed in
      ///   local time.
      /// </returns>
      [SecurityCritical]
      public static DateTime GetCreationTime(string path, PathFormat pathFormat)
      {
         return GetCreationTimeInternal(null, path, false, pathFormat).ToLocalTime();
      }

      #region Transacted

      /// <summary>[AlphaFS] Gets the creation date and time of the specified file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file for which to obtain creation date and time information.</param>
      /// <returns>
      ///   A <see cref="System.DateTime"/> structure set to the creation date and time for the specified file. This value is expressed in
      ///   local time.
      /// </returns>
      [SecurityCritical]
      public static DateTime GetCreationTime(KernelTransaction transaction, string path)
      {
         return GetCreationTimeInternal(transaction, path, false, PathFormat.Auto).ToLocalTime();
      }

      /// <summary>[AlphaFS] Gets the creation date and time of the specified file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file for which to obtain creation date and time information.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>
      ///   A <see cref="System.DateTime"/> structure set to the creation date and time for the specified file. This value is expressed in
      ///   local time.
      /// </returns>
      [SecurityCritical]
      public static DateTime GetCreationTime(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         return GetCreationTimeInternal(transaction, path, false, pathFormat).ToLocalTime();
      }

      #endregion // Transacted

      #endregion

      #region GetCreationTimeUtc

      /// <summary>Gets the creation date and time, in Coordinated Universal Time (UTC) format, of the specified file.</summary>
      /// <param name="path">
      ///   The file for which to obtain creation date and time information, in Coordinated Universal Time (UTC) format.
      /// </param>
      /// <returns>
      ///   A <see cref="System.DateTime"/> structure set to the creation date and time for the specified file. This value is expressed in UTC
      ///   time.
      /// </returns>
      [SecurityCritical]
      public static DateTime GetCreationTimeUtc(string path)
      {
         return GetCreationTimeInternal(null, path, true, PathFormat.Auto);
      }

      /// <summary>[AlphaFS] Gets the creation date and time, in Coordinated Universal Time (UTC) format, of the specified file.</summary>
      /// <param name="path">
      ///   The file for which to obtain creation date and time information, in Coordinated Universal Time (UTC) format.
      /// </param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>
      ///   A <see cref="System.DateTime"/> structure set to the creation date and time for the specified file. This value is expressed in UTC
      ///   time.
      /// </returns>
      [SecurityCritical]
      public static DateTime GetCreationTimeUtc(string path, PathFormat pathFormat)
      {
         return GetCreationTimeInternal(null, path, true, pathFormat);
      }

      #region Transacted

      /// <summary>[AlphaFS] Gets the creation date and time, in Coordinated Universal Time (UTC) format, of the specified file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">
      ///   The file for which to obtain creation date and time information, in Coordinated Universal Time (UTC) format.
      /// </param>
      /// <returns>
      ///   A <see cref="System.DateTime"/> structure set to the creation date and time for the specified file. This value is expressed in UTC
      ///   time.
      /// </returns>
      [SecurityCritical]
      public static DateTime GetCreationTimeUtc(KernelTransaction transaction, string path)
      {
         return GetCreationTimeInternal(transaction, path, true, PathFormat.Auto);
      }

      /// <summary>[AlphaFS] Gets the creation date and time, in Coordinated Universal Time (UTC) format, of the specified file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">
      ///   The file for which to obtain creation date and time information, in Coordinated Universal Time (UTC) format.
      /// </param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>
      ///   A <see cref="System.DateTime"/> structure set to the creation date and time for the specified file. This value is expressed in UTC
      ///   time.
      /// </returns>
      [SecurityCritical]
      public static DateTime GetCreationTimeUtc(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         return GetCreationTimeInternal(transaction, path, true, pathFormat);
      }

      #endregion // Transacted

      #endregion // GetCreationTimeUtc

      #region Internal Methods

      /// <summary>
      ///   [AlphaFS] Gets the creation date and time, in Coordinated Universal Time (UTC) or local time, of the specified file or directory.
      /// </summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file or directory for which to obtain creation date and time information.</param>
      /// <param name="returnUtc">
      ///   <see langword="true"/> gets the Coordinated Universal Time (UTC), <see langword="false"/> gets the local time.
      /// </param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>
      ///   A <see cref="System.DateTime"/> structure set to the creation date and time for the specified file or directory. Depending on
      ///   <paramref name="returnUtc"/> this value is expressed in UTC- or local time.
      /// </returns>
      [SecurityCritical]
      internal static DateTime GetCreationTimeInternal(KernelTransaction transaction, string path, bool returnUtc, PathFormat pathFormat)
      {
         NativeMethods.FileTime creationTime = GetAttributesExInternal<NativeMethods.Win32FileAttributeData>(transaction, path, pathFormat).CreationTime;

         return returnUtc
            ? DateTime.FromFileTimeUtc(creationTime)
            : DateTime.FromFileTime(creationTime);
      }

      #endregion // GetCreationTimeInternal
   }
}
