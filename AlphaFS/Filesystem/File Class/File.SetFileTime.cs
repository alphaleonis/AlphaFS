/*  Copyright (C) 2008-2015 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
 *  
 *  Permission is hereby granted, free of charge, to any person obtaining a copy 
 *  of this software and associated documentation files (the "Software"), to deal 
 *  in the Software without restriction, including without limitation the rights 
 *  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell 
 *  copies of the Software, and to permit persons to whom the Software is 
 *  furnished to do so, subject to the following conditions:
 *  
 *  The above copyright notice and this permission notice shall be included in 
 *  all copies or substantial portions of the Software.
 *  
 *  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
 *  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
 *  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
 *  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
 *  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
 *  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN 
 *  THE SOFTWARE. 
 */

using Microsoft.Win32.SafeHandles;
using System;
using System.IO;
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
         SetFsoDateTimeInternal(false, null, path, creationTime.ToUniversalTime(), null, null, PathFormat.RelativePath);
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


      #region Transactional

      /// <summary>[AlphaFS] Sets the date and time the file was created.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file for which to set the creation date and time information.</param>
      /// <param name="creationTime">
      ///   A <see cref="System.DateTime"/> containing the value to set for the creation date and time of <paramref name="path"/>. This value
      ///   is expressed in local time.
      /// </param>      
      [SecurityCritical]
      public static void SetCreationTimeTransacted(KernelTransaction transaction, string path, DateTime creationTime)
      {
         SetFsoDateTimeInternal(false, transaction, path, creationTime.ToUniversalTime(), null, null, PathFormat.RelativePath);
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
      public static void SetCreationTimeTransacted(KernelTransaction transaction, string path, DateTime creationTime, PathFormat pathFormat)
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
         SetFsoDateTimeInternal(false, null, path, creationTimeUtc, null, null, PathFormat.RelativePath);
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

      #region Transactional

      /// <summary>[AlphaFS] Sets the date and time, in coordinated universal time (UTC), that the file was created.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file for which to set the creation date and time information.</param>
      /// <param name="creationTimeUtc">
      ///   A <see cref="System.DateTime"/> containing the value to set for the creation date and time of <paramref name="path"/>. This value
      ///   is expressed in UTC time.
      /// </param>      
      [SecurityCritical]
      public static void SetCreationTimeUtcTransacted(KernelTransaction transaction, string path, DateTime creationTimeUtc)
      {
         SetFsoDateTimeInternal(false, transaction, path, creationTimeUtc, null, null, PathFormat.RelativePath);
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
      public static void SetCreationTimeUtcTransacted(KernelTransaction transaction, string path, DateTime creationTimeUtc, PathFormat pathFormat)
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
         SetFsoDateTimeInternal(false, null, path, null, lastAccessTime.ToUniversalTime(), null, PathFormat.RelativePath);
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
      public static void SetLastAccessTimeTransacted(KernelTransaction transaction, string path, DateTime lastAccessTime)
      {
         SetFsoDateTimeInternal(false, transaction, path, null, lastAccessTime.ToUniversalTime(), null, PathFormat.RelativePath);
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
      public static void SetLastAccessTimeTransacted(KernelTransaction transaction, string path, DateTime lastAccessTime, PathFormat pathFormat)
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
         SetFsoDateTimeInternal(false, null, path, null, lastAccessTimeUtc, null, PathFormat.RelativePath);
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


      #region Transactional

      /// <summary>[AlphaFS] Sets the date and time, in coordinated universal time (UTC), that the specified file was last accessed.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file for which to set the access date and time information.</param>
      /// <param name="lastAccessTimeUtc">
      ///   A <see cref="System.DateTime"/> containing the value to set for the last access date and time of <paramref name="path"/>. This
      ///   value is expressed in UTC time.
      /// </param>      
      [SecurityCritical]
      public static void SetLastAccessTimeUtcTransacted(KernelTransaction transaction, string path, DateTime lastAccessTimeUtc)
      {
         SetFsoDateTimeInternal(false, transaction, path, null, lastAccessTimeUtc, null, PathFormat.RelativePath);
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
      public static void SetLastAccessTimeUtcTransacted(KernelTransaction transaction, string path, DateTime lastAccessTimeUtc, PathFormat pathFormat)
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
         SetFsoDateTimeInternal(false, null, path, null, null, lastWriteTime.ToUniversalTime(), PathFormat.RelativePath);
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

      #region Transactional

      /// <summary>[AlphaFS] Sets the date and time that the specified file was last written to.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file for which to set the date and time information.</param>
      /// <param name="lastWriteTime">
      ///   A <see cref="System.DateTime"/> containing the value to set for the last write date and time of <paramref name="path"/>. This value
      ///   is expressed in local time.
      /// </param>      
      [SecurityCritical]
      public static void SetLastWriteTimeTransacted(KernelTransaction transaction, string path, DateTime lastWriteTime)
      {
         SetFsoDateTimeInternal(false, transaction, path, null, null, lastWriteTime.ToUniversalTime(), PathFormat.RelativePath);
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
      public static void SetLastWriteTimeTransacted(KernelTransaction transaction, string path, DateTime lastWriteTime, PathFormat pathFormat)
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
         SetFsoDateTimeInternal(false, null, path, null, null, lastWriteTimeUtc, PathFormat.RelativePath);
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

      #region Transactional

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
      public static void SetLastWriteTimeUtcTransacted(KernelTransaction transaction, string path, DateTime lastWriteTimeUtc)
      {
         SetFsoDateTimeInternal(false, transaction, path, null, null, lastWriteTimeUtc, PathFormat.RelativePath);
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
      public static void SetLastWriteTimeUtcTransacted(KernelTransaction transaction, string path, DateTime lastWriteTimeUtc, PathFormat pathFormat)
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
         SetFsoDateTimeInternal(false, null, path, creationTime.ToUniversalTime(), lastAccessTime.ToUniversalTime(), lastWriteTime.ToUniversalTime(), PathFormat.RelativePath);
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
      public static void SetTimestampsTransacted(KernelTransaction transaction, string path, DateTime creationTime, DateTime lastAccessTime, DateTime lastWriteTime, PathFormat pathFormat)
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
      public static void SetTimestampsTransacted(KernelTransaction transaction, string path, DateTime creationTime, DateTime lastAccessTime, DateTime lastWriteTime)
      {
         SetFsoDateTimeInternal(false, transaction, path, creationTime.ToUniversalTime(), lastAccessTime.ToUniversalTime(), lastWriteTime.ToUniversalTime(), PathFormat.RelativePath);
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
         SetFsoDateTimeInternal(false, null, path, creationTimeUtc, lastAccessTimeUtc, lastWriteTimeUtc, PathFormat.RelativePath);
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
      public static void SetTimestampsUtcTransacted(KernelTransaction transaction, string path, DateTime creationTimeUtc, DateTime lastAccessTimeUtc, DateTime lastWriteTimeUtc, PathFormat pathFormat)
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
      public static void SetTimestampsUtcTransacted(KernelTransaction transaction, string path, DateTime creationTimeUtc, DateTime lastAccessTimeUtc, DateTime lastWriteTimeUtc)
      {
         SetFsoDateTimeInternal(false, transaction, path, creationTimeUtc, lastAccessTimeUtc, lastWriteTimeUtc, PathFormat.RelativePath);
      }

      #endregion // SetTimestampsUtc

      #region Internal Methods

      /// <summary>Unified method SetFsoDateTimeInternal() to set the date and time, in coordinated universal time (UTC),
      /// that the file or directory was created and/or last accessed and/or written to.</summary>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="NotSupportedException"/>
      /// <param name="isFolder">Specifies that <paramref name="path"/> is a file or directory.</param>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file or directory for which to set the date and time information.</param>
      /// <param name="creationTimeUtc">A <see cref="System.DateTime"/> containing the value to set for the creation date and time of <paramref name="path"/>. This value is expressed in UTC time.</param>
      /// <param name="lastAccessTimeUtc">A <see cref="System.DateTime"/> containing the value to set for the last access date and time of <paramref name="path"/>. This value is expressed in UTC time.</param>
      /// <param name="lastWriteTimeUtc">A <see cref="System.DateTime"/> containing the value to set for the last write date and time of <paramref name="path"/>. This value is expressed in UTC time.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>      
      [SecurityCritical]
      internal static void SetFsoDateTimeInternal(bool isFolder, KernelTransaction transaction, string path, DateTime? creationTimeUtc, DateTime? lastAccessTimeUtc, DateTime? lastWriteTimeUtc, PathFormat pathFormat)
      {
         // Because we already check here, use false for CreateFileInternal() to prevent another check.
         if (pathFormat == PathFormat.RelativePath)
            Path.CheckSupportedPathFormat(path, false, false);

         using (var creationTime = SafeGlobalMemoryBufferHandle.FromLong(creationTimeUtc.HasValue ? creationTimeUtc.Value.ToFileTimeUtc() : (long?)null))
         using (var lastAccessTime = SafeGlobalMemoryBufferHandle.FromLong(lastAccessTimeUtc.HasValue ? lastAccessTimeUtc.Value.ToFileTimeUtc() : (long?)null))
         using (var lastWriteTime = SafeGlobalMemoryBufferHandle.FromLong(lastWriteTimeUtc.HasValue ? lastWriteTimeUtc.Value.ToFileTimeUtc() : (long?)null))
         using (var safeHandle = CreateFileInternal(transaction, path, isFolder ? ExtendedFileAttributes.BackupSemantics : ExtendedFileAttributes.Normal, null, FileMode.Open, FileSystemRights.WriteAttributes, FileShare.Delete | FileShare.Write, false, pathFormat))
            if (!NativeMethods.SetFileTime(safeHandle, creationTime, lastAccessTime, lastWriteTime))
               NativeError.ThrowException(path);
      }

      #endregion // SetFsoDateTimeInternal

   }
}
