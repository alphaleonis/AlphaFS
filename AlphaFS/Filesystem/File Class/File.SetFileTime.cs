using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.AccessControl;

namespace Alphaleonis.Win32.Filesystem
{
   public static partial class File
   {
      #region SetCreationTime

      /// <summary>Sets the date and time the file was created.</summary>
      /// <param name="path">The file for which to set the creation date and time information.</param>
      /// <param name="creationTime">
      ///   A <see cref="System.DateTime"/> containing the value to set for the creation date and time of <paramref name="path"/>. This value
      ///   is expressed in local time.
      /// </param>      
      [SecurityCritical]
      public static void SetCreationTime(string path, DateTime creationTime)
      {
         SetFsoDateTimeInternal(false, null, path, creationTime.ToUniversalTime(), null, null, PathFormat.Auto);
      }

      /// <summary>[AlphaFS] Sets the date and time the file was created.</summary>
      /// <param name="path">The file for which to set the creation date and time information.</param>
      /// <param name="creationTime">
      ///   A <see cref="System.DateTime"/> containing the value to set for the creation date and time of <paramref name="path"/>. This value
      ///   is expressed in local time.
      /// </param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SecurityCritical]
      public static void SetCreationTime(string path, DateTime creationTime, PathFormat pathFormat)
      {
         SetFsoDateTimeInternal(false, null, path, creationTime.ToUniversalTime(), null, null, pathFormat);
      }


      #region Transacted

      /// <summary>[AlphaFS] Sets the date and time the file was created.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file for which to set the creation date and time information.</param>
      /// <param name="creationTime">
      ///   A <see cref="System.DateTime"/> containing the value to set for the creation date and time of <paramref name="path"/>. This value
      ///   is expressed in local time.
      /// </param>      
      [SecurityCritical]
      public static void SetCreationTime(KernelTransaction transaction, string path, DateTime creationTime)
      {
         SetFsoDateTimeInternal(false, transaction, path, creationTime.ToUniversalTime(), null, null, PathFormat.Auto);
      }

      /// <summary>[AlphaFS] Sets the date and time the file was created.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file for which to set the creation date and time information.</param>
      /// <param name="creationTime">
      ///   A <see cref="System.DateTime"/> containing the value to set for the creation date and time of <paramref name="path"/>. This value
      ///   is expressed in local time.
      /// </param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SecurityCritical]
      public static void SetCreationTime(KernelTransaction transaction, string path, DateTime creationTime, PathFormat pathFormat)
      {
         SetFsoDateTimeInternal(false, transaction, path, creationTime.ToUniversalTime(), null, null, pathFormat);
      }


      #endregion // Transacted

      #endregion // SetCreationTime

      #region SetCreationTimeUtc

      /// <summary>Sets the date and time, in coordinated universal time (UTC), that the file was created.</summary>
      /// <param name="path">The file for which to set the creation date and time information.</param>
      /// <param name="creationTimeUtc">
      ///   A <see cref="System.DateTime"/> containing the value to set for the creation date and time of <paramref name="path"/>. This value
      ///   is expressed in UTC time.
      /// </param>      
      [SecurityCritical]
      public static void SetCreationTimeUtc(string path, DateTime creationTimeUtc)
      {
         SetFsoDateTimeInternal(false, null, path, creationTimeUtc, null, null, PathFormat.Auto);
      }

      /// <summary>[AlphaFS] Sets the date and time, in coordinated universal time (UTC), that the file was created.</summary>
      /// <param name="path">The file for which to set the creation date and time information.</param>
      /// <param name="creationTimeUtc">
      ///   A <see cref="System.DateTime"/> containing the value to set for the creation date and time of <paramref name="path"/>. This value
      ///   is expressed in UTC time.
      /// </param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SecurityCritical]
      public static void SetCreationTimeUtc(string path, DateTime creationTimeUtc, PathFormat pathFormat)
      {
         SetFsoDateTimeInternal(false, null, path, creationTimeUtc, null, null, pathFormat);
      }

      #region Transacted

      /// <summary>[AlphaFS] Sets the date and time, in coordinated universal time (UTC), that the file was created.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file for which to set the creation date and time information.</param>
      /// <param name="creationTimeUtc">
      ///   A <see cref="System.DateTime"/> containing the value to set for the creation date and time of <paramref name="path"/>. This value
      ///   is expressed in UTC time.
      /// </param>      
      [SecurityCritical]
      public static void SetCreationTimeUtc(KernelTransaction transaction, string path, DateTime creationTimeUtc)
      {
         SetFsoDateTimeInternal(false, transaction, path, creationTimeUtc, null, null, PathFormat.Auto);
      }

      /// <summary>[AlphaFS] Sets the date and time, in coordinated universal time (UTC), that the file was created.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file for which to set the creation date and time information.</param>
      /// <param name="creationTimeUtc">
      ///   A <see cref="System.DateTime"/> containing the value to set for the creation date and time of <paramref name="path"/>. This value
      ///   is expressed in UTC time.
      /// </param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SecurityCritical]
      public static void SetCreationTimeUtc(KernelTransaction transaction, string path, DateTime creationTimeUtc, PathFormat pathFormat)
      {
         SetFsoDateTimeInternal(false, transaction, path, creationTimeUtc, null, null, pathFormat);
      }

      #endregion // Transacted

      #endregion // SetCreationTimeUtc

      #region SetLastAccessTime

      /// <summary>Sets the date and time that the specified file was last accessed.</summary>
      /// <param name="path">The file for which to set the access date and time information.</param>
      /// <param name="lastAccessTime">
      ///   A <see cref="System.DateTime"/> containing the value to set for the last access date and time of <paramref name="path"/>. This
      ///   value is expressed in local time.
      /// </param>      
      [SecurityCritical]
      public static void SetLastAccessTime(string path, DateTime lastAccessTime)
      {
         SetFsoDateTimeInternal(false, null, path, null, lastAccessTime.ToUniversalTime(), null, PathFormat.Auto);
      }

      /// <summary>[AlphaFS] Sets the date and time that the specified file was last accessed.</summary>
      /// <param name="path">The file for which to set the access date and time information.</param>
      /// <param name="lastAccessTime">
      ///   A <see cref="System.DateTime"/> containing the value to set for the last access date and time of <paramref name="path"/>. This
      ///   value is expressed in local time.
      /// </param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SecurityCritical]
      public static void SetLastAccessTime(string path, DateTime lastAccessTime, PathFormat pathFormat)
      {
         SetFsoDateTimeInternal(false, null, path, null, lastAccessTime.ToUniversalTime(), null, pathFormat);
      }


      #region Transaction

      /// <summary>[AlphaFS] Sets the date and time that the specified file was last accessed.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file for which to set the access date and time information.</param>
      /// <param name="lastAccessTime">
      ///   A <see cref="System.DateTime"/> containing the value to set for the last access date and time of <paramref name="path"/>. This
      ///   value is expressed in local time.
      /// </param>      
      [SecurityCritical]
      public static void SetLastAccessTime(KernelTransaction transaction, string path, DateTime lastAccessTime)
      {
         SetFsoDateTimeInternal(false, transaction, path, null, lastAccessTime.ToUniversalTime(), null, PathFormat.Auto);
      }

      /// <summary>[AlphaFS] Sets the date and time that the specified file was last accessed.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file for which to set the access date and time information.</param>
      /// <param name="lastAccessTime">
      ///   A <see cref="System.DateTime"/> containing the value to set for the last access date and time of <paramref name="path"/>. This
      ///   value is expressed in local time.
      /// </param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SecurityCritical]
      public static void SetLastAccessTime(KernelTransaction transaction, string path, DateTime lastAccessTime, PathFormat pathFormat)
      {
         SetFsoDateTimeInternal(false, transaction, path, null, lastAccessTime.ToUniversalTime(), null, pathFormat);
      }


      #endregion // Transaction

      #endregion // SetLastAccessTime

      #region SetLastAccessTimeUtc

      /// <summary>Sets the date and time, in coordinated universal time (UTC), that the specified file was last accessed.</summary>
      /// <param name="path">The file for which to set the access date and time information.</param>
      /// <param name="lastAccessTimeUtc">
      ///   A <see cref="System.DateTime"/> containing the value to set for the last access date and time of <paramref name="path"/>. This
      ///   value is expressed in UTC time.
      /// </param>      
      [SecurityCritical]
      public static void SetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc)
      {
         SetFsoDateTimeInternal(false, null, path, null, lastAccessTimeUtc, null, PathFormat.Auto);
      }

      /// <summary>[AlphaFS] Sets the date and time, in coordinated universal time (UTC), that the specified file was last accessed.</summary>
      /// <param name="path">The file for which to set the access date and time information.</param>
      /// <param name="lastAccessTimeUtc">
      ///   A <see cref="System.DateTime"/> containing the value to set for the last access date and time of <paramref name="path"/>. This
      ///   value is expressed in UTC time.
      /// </param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SecurityCritical]
      public static void SetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc, PathFormat pathFormat)
      {
         SetFsoDateTimeInternal(false, null, path, null, lastAccessTimeUtc, null, pathFormat);
      }


      #region Transacted

      /// <summary>[AlphaFS] Sets the date and time, in coordinated universal time (UTC), that the specified file was last accessed.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file for which to set the access date and time information.</param>
      /// <param name="lastAccessTimeUtc">
      ///   A <see cref="System.DateTime"/> containing the value to set for the last access date and time of <paramref name="path"/>. This
      ///   value is expressed in UTC time.
      /// </param>      
      [SecurityCritical]
      public static void SetLastAccessTimeUtc(KernelTransaction transaction, string path, DateTime lastAccessTimeUtc)
      {
         SetFsoDateTimeInternal(false, transaction, path, null, lastAccessTimeUtc, null, PathFormat.Auto);
      }


      /// <summary>[AlphaFS] Sets the date and time, in coordinated universal time (UTC), that the specified file was last accessed.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file for which to set the access date and time information.</param>
      /// <param name="lastAccessTimeUtc">
      ///   A <see cref="System.DateTime"/> containing the value to set for the last access date and time of <paramref name="path"/>. This
      ///   value is expressed in UTC time.
      /// </param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SecurityCritical]
      public static void SetLastAccessTimeUtc(KernelTransaction transaction, string path, DateTime lastAccessTimeUtc, PathFormat pathFormat)
      {
         SetFsoDateTimeInternal(false, transaction, path, null, lastAccessTimeUtc, null, pathFormat);
      }


      #endregion // Transacted

      #endregion // SetLastAccessTimeUtc

      #region SetLastWriteTime

      /// <summary>Sets the date and time that the specified file was last written to.</summary>
      /// <param name="path">The file for which to set the date and time information.</param>
      /// <param name="lastWriteTime">
      ///   A <see cref="System.DateTime"/> containing the value to set for the last write date and time of <paramref name="path"/>. This value
      ///   is expressed in local time.
      /// </param>      
      [SecurityCritical]
      public static void SetLastWriteTime(string path, DateTime lastWriteTime)
      {
         SetFsoDateTimeInternal(false, null, path, null, null, lastWriteTime.ToUniversalTime(), PathFormat.Auto);
      }

      /// <summary>[AlphaFS] Sets the date and time that the specified file was last written to.</summary>
      /// <param name="path">The file for which to set the date and time information.</param>
      /// <param name="lastWriteTime">
      ///   A <see cref="System.DateTime"/> containing the value to set for the last write date and time of <paramref name="path"/>. This value
      ///   is expressed in local time.
      /// </param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SecurityCritical]
      public static void SetLastWriteTime(string path, DateTime lastWriteTime, PathFormat pathFormat)
      {
         SetFsoDateTimeInternal(false, null, path, null, null, lastWriteTime.ToUniversalTime(), pathFormat);
      }

      #region Transacted

      /// <summary>[AlphaFS] Sets the date and time that the specified file was last written to.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file for which to set the date and time information.</param>
      /// <param name="lastWriteTime">
      ///   A <see cref="System.DateTime"/> containing the value to set for the last write date and time of <paramref name="path"/>. This value
      ///   is expressed in local time.
      /// </param>      
      [SecurityCritical]
      public static void SetLastWriteTime(KernelTransaction transaction, string path, DateTime lastWriteTime)
      {
         SetFsoDateTimeInternal(false, transaction, path, null, null, lastWriteTime.ToUniversalTime(), PathFormat.Auto);
      }

      /// <summary>[AlphaFS] Sets the date and time that the specified file was last written to.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file for which to set the date and time information.</param>
      /// <param name="lastWriteTime">
      ///   A <see cref="System.DateTime"/> containing the value to set for the last write date and time of <paramref name="path"/>. This value
      ///   is expressed in local time.
      /// </param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SecurityCritical]
      public static void SetLastWriteTime(KernelTransaction transaction, string path, DateTime lastWriteTime, PathFormat pathFormat)
      {
         SetFsoDateTimeInternal(false, transaction, path, null, null, lastWriteTime.ToUniversalTime(), pathFormat);
      }


      #endregion // Transacted

      #endregion // SetLastWriteTime

      #region SetLastWriteTimeUtc

      /// <summary>Sets the date and time, in coordinated universal time (UTC), that the specified file was last written to.</summary>
      /// <param name="path">The file for which to set the date and time information.</param>
      /// <param name="lastWriteTimeUtc">
      ///   A <see cref="System.DateTime"/> containing the value to set for the last write date and time of <paramref name="path"/>. This value
      ///   is expressed in UTC time.
      /// </param>      
      [SecurityCritical]
      public static void SetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc)
      {
         SetFsoDateTimeInternal(false, null, path, null, null, lastWriteTimeUtc, PathFormat.Auto);
      }

      /// <summary>
      ///   [AlphaFS] Sets the date and time, in coordinated universal time (UTC), that the specified file was last written to.
      /// </summary>
      /// <param name="path">The file for which to set the date and time information.</param>
      /// <param name="lastWriteTimeUtc">
      ///   A <see cref="System.DateTime"/> containing the value to set for the last write date and time of <paramref name="path"/>. This value
      ///   is expressed in UTC time.
      /// </param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SecurityCritical]
      public static void SetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc, PathFormat pathFormat)
      {
         SetFsoDateTimeInternal(false, null, path, null, null, lastWriteTimeUtc, pathFormat);
      }

      #region Transacted

      /// <summary>
      ///   [AlphaFS] Sets the date and time, in coordinated universal time (UTC), that the specified file was last written to.
      /// </summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file for which to set the date and time information.</param>
      /// <param name="lastWriteTimeUtc">
      ///   A <see cref="System.DateTime"/> containing the value to set for the last write date and time of <paramref name="path"/>. This value
      ///   is expressed in UTC time.
      /// </param>      
      [SecurityCritical]
      public static void SetLastWriteTimeUtc(KernelTransaction transaction, string path, DateTime lastWriteTimeUtc)
      {
         SetFsoDateTimeInternal(false, transaction, path, null, null, lastWriteTimeUtc, PathFormat.Auto);
      }

      /// <summary>
      ///   [AlphaFS] Sets the date and time, in coordinated universal time (UTC), that the specified file was last written to.
      /// </summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file for which to set the date and time information.</param>
      /// <param name="lastWriteTimeUtc">
      ///   A <see cref="System.DateTime"/> containing the value to set for the last write date and time of <paramref name="path"/>. This value
      ///   is expressed in UTC time.
      /// </param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SecurityCritical]
      public static void SetLastWriteTimeUtc(KernelTransaction transaction, string path, DateTime lastWriteTimeUtc, PathFormat pathFormat)
      {
         SetFsoDateTimeInternal(false, transaction, path, null, null, lastWriteTimeUtc, pathFormat);
      }


      #endregion // Transacted


      #endregion // SetLastWriteTimeUtc

      #region SetTimestamps

      /// <summary>[AlphaFS] Sets all the date and time stamps for the specified file, at once.</summary>
      /// <param name="path">The file for which to set the dates and times information.</param>
      /// <param name="creationTime">
      ///   A <see cref="System.DateTime"/> containing the value to set for the creation date and time of <paramref name="path"/>. This value
      ///   is expressed in local time.
      /// </param>
      /// <param name="lastAccessTime">
      ///   A <see cref="System.DateTime"/> containing the value to set for the last access date and time of <paramref name="path"/>. This
      ///   value is expressed in local time.
      /// </param>
      /// <param name="lastWriteTime">
      ///   A <see cref="System.DateTime"/> containing the value to set for the last write date and time of <paramref name="path"/>. This value
      ///   is expressed in local time.
      /// </param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SecurityCritical]
      public static void SetTimestamps(string path, DateTime creationTime, DateTime lastAccessTime, DateTime lastWriteTime, PathFormat pathFormat)
      {
         SetFsoDateTimeInternal(false, null, path, creationTime.ToUniversalTime(), lastAccessTime.ToUniversalTime(), lastWriteTime.ToUniversalTime(), pathFormat);
      }

      /// <summary>[AlphaFS] Sets all the date and time stamps for the specified file, at once.</summary>
      /// <param name="path">The file for which to set the dates and times information.</param>
      /// <param name="creationTime">
      ///   A <see cref="System.DateTime"/> containing the value to set for the creation date and time of <paramref name="path"/>. This value
      ///   is expressed in local time.
      /// </param>
      /// <param name="lastAccessTime">
      ///   A <see cref="System.DateTime"/> containing the value to set for the last access date and time of <paramref name="path"/>. This
      ///   value is expressed in local time.
      /// </param>
      /// <param name="lastWriteTime">
      ///   A <see cref="System.DateTime"/> containing the value to set for the last write date and time of <paramref name="path"/>. This value
      ///   is expressed in local time.
      /// </param>     
      [SecurityCritical]
      public static void SetTimestamps(string path, DateTime creationTime, DateTime lastAccessTime, DateTime lastWriteTime)
      {
         SetFsoDateTimeInternal(false, null, path, creationTime.ToUniversalTime(), lastAccessTime.ToUniversalTime(), lastWriteTime.ToUniversalTime(), PathFormat.Auto);
      }

      /// <summary>[AlphaFS] Sets all the date and time stamps for the specified file, at once.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file for which to set the dates and times information.</param>
      /// <param name="creationTime">
      ///   A <see cref="System.DateTime"/> containing the value to set for the creation date and time of <paramref name="path"/>. This value
      ///   is expressed in local time.
      /// </param>
      /// <param name="lastAccessTime">
      ///   A <see cref="System.DateTime"/> containing the value to set for the last access date and time of <paramref name="path"/>. This
      ///   value is expressed in local time.
      /// </param>
      /// <param name="lastWriteTime">
      ///   A <see cref="System.DateTime"/> containing the value to set for the last write date and time of <paramref name="path"/>. This value
      ///   is expressed in local time.
      /// </param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SecurityCritical]
      public static void SetTimestamps(KernelTransaction transaction, string path, DateTime creationTime, DateTime lastAccessTime, DateTime lastWriteTime, PathFormat pathFormat)
      {
         SetFsoDateTimeInternal(false, transaction, path, creationTime.ToUniversalTime(), lastAccessTime.ToUniversalTime(), lastWriteTime.ToUniversalTime(), pathFormat);
      }

      /// <summary>[AlphaFS] Sets all the date and time stamps for the specified file, at once.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file for which to set the dates and times information.</param>
      /// <param name="creationTime">
      ///   A <see cref="System.DateTime"/> containing the value to set for the creation date and time of <paramref name="path"/>. This value
      ///   is expressed in local time.
      /// </param>
      /// <param name="lastAccessTime">
      ///   A <see cref="System.DateTime"/> containing the value to set for the last access date and time of <paramref name="path"/>. This
      ///   value is expressed in local time.
      /// </param>
      /// <param name="lastWriteTime">
      ///   A <see cref="System.DateTime"/> containing the value to set for the last write date and time of <paramref name="path"/>. This value
      ///   is expressed in local time.
      /// </param>      
      [SecurityCritical]
      public static void SetTimestamps(KernelTransaction transaction, string path, DateTime creationTime, DateTime lastAccessTime, DateTime lastWriteTime)
      {
         SetFsoDateTimeInternal(false, transaction, path, creationTime.ToUniversalTime(), lastAccessTime.ToUniversalTime(), lastWriteTime.ToUniversalTime(), PathFormat.Auto);
      }

      #endregion // SetTimestamps

      #region SetTimestampsUtc

      /// <summary>[AlphaFS] Sets all the date and time stamps, in coordinated universal time (UTC), for the specified file, at once.</summary>
      /// <param name="path">The file for which to set the dates and times information.</param>
      /// <param name="creationTimeUtc">
      ///   A <see cref="System.DateTime"/> containing the value to set for the creation date and time of <paramref name="path"/>. This value
      ///   is expressed in UTC time.
      /// </param>
      /// <param name="lastAccessTimeUtc">
      ///   A <see cref="System.DateTime"/> containing the value to set for the last access date and time of <paramref name="path"/>. This
      ///   value is expressed in UTC time.
      /// </param>
      /// <param name="lastWriteTimeUtc">
      ///   A <see cref="System.DateTime"/> containing the value to set for the last write date and time of <paramref name="path"/>. This value
      ///   is expressed in UTC time.
      /// </param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SecurityCritical]
      public static void SetTimestampsUtc(string path, DateTime creationTimeUtc, DateTime lastAccessTimeUtc, DateTime lastWriteTimeUtc, PathFormat pathFormat)
      {
         SetFsoDateTimeInternal(false, null, path, creationTimeUtc, lastAccessTimeUtc, lastWriteTimeUtc, pathFormat);
      }

      /// <summary>[AlphaFS] Sets all the date and time stamps, in coordinated universal time (UTC), for the specified file, at once.</summary>
      /// <param name="path">The file for which to set the dates and times information.</param>
      /// <param name="creationTimeUtc">
      ///   A <see cref="System.DateTime"/> containing the value to set for the creation date and time of <paramref name="path"/>. This value
      ///   is expressed in UTC time.
      /// </param>
      /// <param name="lastAccessTimeUtc">
      ///   A <see cref="System.DateTime"/> containing the value to set for the last access date and time of <paramref name="path"/>. This
      ///   value is expressed in UTC time.
      /// </param>
      /// <param name="lastWriteTimeUtc">
      ///   A <see cref="System.DateTime"/> containing the value to set for the last write date and time of <paramref name="path"/>. This value
      ///   is expressed in UTC time.
      /// </param>      
      [SecurityCritical]
      public static void SetTimestampsUtc(string path, DateTime creationTimeUtc, DateTime lastAccessTimeUtc, DateTime lastWriteTimeUtc)
      {
         SetFsoDateTimeInternal(false, null, path, creationTimeUtc, lastAccessTimeUtc, lastWriteTimeUtc, PathFormat.Auto);
      }

      /// <summary>[AlphaFS] Sets all the date and time stamps, in coordinated universal time (UTC), for the specified file, at once.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file for which to set the dates and times information.</param>
      /// <param name="creationTimeUtc">
      ///   A <see cref="System.DateTime"/> containing the value to set for the creation date and time of <paramref name="path"/>. This value
      ///   is expressed in UTC time.
      /// </param>
      /// <param name="lastAccessTimeUtc">
      ///   A <see cref="System.DateTime"/> containing the value to set for the last access date and time of <paramref name="path"/>. This
      ///   value is expressed in UTC time.
      /// </param>
      /// <param name="lastWriteTimeUtc">
      ///   A <see cref="System.DateTime"/> containing the value to set for the last write date and time of <paramref name="path"/>. This value
      ///   is expressed in UTC time.
      /// </param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SecurityCritical]
      public static void SetTimestampsUtc(KernelTransaction transaction, string path, DateTime creationTimeUtc, DateTime lastAccessTimeUtc, DateTime lastWriteTimeUtc, PathFormat pathFormat)
      {
         SetFsoDateTimeInternal(false, transaction, path, creationTimeUtc, lastAccessTimeUtc, lastWriteTimeUtc, pathFormat);
      }

      /// <summary>[AlphaFS] Sets all the date and time stamps, in coordinated universal time (UTC), for the specified file, at once.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file for which to set the dates and times information.</param>
      /// <param name="creationTimeUtc">
      ///   A <see cref="System.DateTime"/> containing the value to set for the creation date and time of <paramref name="path"/>. This value
      ///   is expressed in UTC time.
      /// </param>
      /// <param name="lastAccessTimeUtc">
      ///   A <see cref="System.DateTime"/> containing the value to set for the last access date and time of <paramref name="path"/>. This
      ///   value is expressed in UTC time.
      /// </param>
      /// <param name="lastWriteTimeUtc">
      ///   A <see cref="System.DateTime"/> containing the value to set for the last write date and time of <paramref name="path"/>. This value
      ///   is expressed in UTC time.
      /// </param>      
      [SecurityCritical]
      public static void SetTimestampsUtc(KernelTransaction transaction, string path, DateTime creationTimeUtc, DateTime lastAccessTimeUtc, DateTime lastWriteTimeUtc)
      {
         SetFsoDateTimeInternal(false, transaction, path, creationTimeUtc, lastAccessTimeUtc, lastWriteTimeUtc, PathFormat.Auto);
      }

      #endregion // SetTimestampsUtc

      #region Internal Methods

      /// <summary>
      ///   [AlphaFS] Unified method SetFsoDateTimeInternal() to set the date and time, in coordinated universal time (UTC), that the file or
      ///   directory was created and/or last accessed and/or written to.
      /// </summary>
      /// <param name="isFolder">Specifies that <paramref name="path"/> is a file or directory.</param>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file or directory for which to set the date and time information.</param>
      /// <param name="creationTimeUtc">
      ///   A <see cref="System.DateTime"/> containing the value to set for the creation date and time of <paramref name="path"/>. This value
      ///   is expressed in UTC time.
      /// </param>
      /// <param name="lastAccessTimeUtc">
      ///   A <see cref="System.DateTime"/> containing the value to set for the last access date and time of <paramref name="path"/>. This
      ///   value is expressed in UTC time.
      /// </param>
      /// <param name="lastWriteTimeUtc">
      ///   A <see cref="System.DateTime"/> containing the value to set for the last write date and time of <paramref name="path"/>. This value
      ///   is expressed in UTC time.
      /// </param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SecurityCritical]
      internal static void SetFsoDateTimeInternal(bool isFolder, KernelTransaction transaction, string path, DateTime? creationTimeUtc, DateTime? lastAccessTimeUtc, DateTime? lastWriteTimeUtc, PathFormat pathFormat)
      {
         // Because we already check here, use false for CreateFileInternal() to prevent another check.
         if (pathFormat == PathFormat.Auto)
            Path.CheckValidPath(path, false, false);

         using (SafeGlobalMemoryBufferHandle creationTime = SafeGlobalMemoryBufferHandle.CreateFromLong(creationTimeUtc.HasValue ? creationTimeUtc.Value.ToFileTimeUtc() : (long?)null))
         using (SafeGlobalMemoryBufferHandle lastAccessTime = SafeGlobalMemoryBufferHandle.CreateFromLong(lastAccessTimeUtc.HasValue ? lastAccessTimeUtc.Value.ToFileTimeUtc() : (long?)null))
         using (SafeGlobalMemoryBufferHandle lastWriteTime = SafeGlobalMemoryBufferHandle.CreateFromLong(lastWriteTimeUtc.HasValue ? lastWriteTimeUtc.Value.ToFileTimeUtc() : (long?)null))
         using (SafeFileHandle safeHandle = CreateFileInternal(transaction, path, isFolder ? ExtendedFileAttributes.BackupSemantics : ExtendedFileAttributes.Normal, null, FileMode.Open, FileSystemRights.WriteAttributes, FileShare.Delete | FileShare.Write, false, pathFormat))
            if (!NativeMethods.SetFileTime(safeHandle, creationTime, lastAccessTime, lastWriteTime))
               NativeError.ThrowException(path);
      }

      #endregion // SetFsoDateTimeInternal

   }
}
