/* Copyright (C) 2008-2015 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
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
using System.Collections.Generic;
using System.Linq;
using System.Security;

namespace Alphaleonis.Win32.Filesystem
{
   partial class Directory
   {
      #region GetCreationTime

      /// <summary>Gets the creation date and time of the specified directory.</summary>
      /// <param name="path">The directory for which to obtain creation date and time information.</param>
      /// <returns>A <see cref="System.DateTime"/> structure set to the creation date and time for the specified directory. This value is expressed in local time.</returns>
      [SecurityCritical]
      public static DateTime GetCreationTime(string path)
      {
         return File.GetCreationTimeInternal(null, path, false, PathFormat.RelativePath).ToLocalTime();
      }

      /// <summary>[AlphaFS] Gets the creation date and time of the specified directory.</summary>
      /// <param name="path">The directory for which to obtain creation date and time information.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>A <see cref="System.DateTime"/> structure set to the creation date and time for the specified directory. This value is expressed in local time.</returns>
      [SecurityCritical]
      public static DateTime GetCreationTime(string path, PathFormat pathFormat)
      {
         return File.GetCreationTimeInternal(null, path, false, pathFormat).ToLocalTime();
      }

      /// <summary>[AlphaFS] Gets the creation date and time of the specified directory.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory for which to obtain creation date and time information.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>A <see cref="System.DateTime"/> structure set to the creation date and time for the specified directory. This value is expressed in local time.</returns>
      [SecurityCritical]
      public static DateTime GetCreationTime(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         return File.GetCreationTimeInternal(transaction, path, false, pathFormat).ToLocalTime();
      }

      /// <summary>[AlphaFS] Gets the creation date and time of the specified directory.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory for which to obtain creation date and time information.</param>
      /// <returns>A <see cref="System.DateTime"/> structure set to the creation date and time for the specified directory. This value is expressed in local time.</returns>
      [SecurityCritical]
      public static DateTime GetCreationTime(KernelTransaction transaction, string path)
      {
         return File.GetCreationTimeInternal(transaction, path, false, PathFormat.RelativePath).ToLocalTime();
      }

      #endregion // GetCreationTime

      #region GetCreationTimeUtc

      /// <summary>Gets the creation date and time, in Coordinated Universal Time (UTC) format, of the specified directory.</summary>
      /// <param name="path">The directory for which to obtain creation date and time information, in Coordinated Universal Time (UTC) format.</param>
      /// <returns>A <see cref="System.DateTime"/> structure set to the creation date and time for the specified directory. This value is expressed in UTC time.</returns>
      [SecurityCritical]
      public static DateTime GetCreationTimeUtc(string path)
      {
         return File.GetCreationTimeInternal(null, path, true, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Gets the creation date and time, in Coordinated Universal Time (UTC) format, of the specified directory.</summary>
      /// <param name="path">The directory for which to obtain creation date and time information, in Coordinated Universal Time (UTC) format.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>A <see cref="System.DateTime"/> structure set to the creation date and time for the specified directory. This value is expressed in UTC time.</returns>
      [SecurityCritical]
      public static DateTime GetCreationTimeUtc(string path, PathFormat pathFormat)
      {
         return File.GetCreationTimeInternal(null, path, true, pathFormat);
      }

      /// <summary>[AlphaFS] Gets the creation date and time, in Coordinated Universal Time (UTC) format, of the specified directory.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory for which to obtain creation date and time information, in Coordinated Universal Time (UTC) format.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>A <see cref="System.DateTime"/> structure set to the creation date and time for the specified directory. This value is expressed in UTC time.</returns>
      [SecurityCritical]
      public static DateTime GetCreationTimeUtc(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         return File.GetCreationTimeInternal(transaction, path, true, pathFormat);
      }

      /// <summary>[AlphaFS] Gets the creation date and time, in Coordinated Universal Time (UTC) format, of the specified directory.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory for which to obtain creation date and time information, in Coordinated Universal Time (UTC) format.</param>
      /// <returns>A <see cref="System.DateTime"/> structure set to the creation date and time for the specified directory. This value is expressed in UTC time.</returns>
      [SecurityCritical]
      public static DateTime GetCreationTimeUtc(KernelTransaction transaction, string path)
      {
         return File.GetCreationTimeInternal(transaction, path, true, PathFormat.RelativePath);
      }

      #endregion // GetCreationTimeUtc

      #region GetLastAccessTime

      /// <summary>Gets the date and time that the specified directory was last accessed.</summary>
      /// <param name="path">The directory for which to obtain access date and time information.</param>
      /// <returns>A <see cref="System.DateTime"/> structure set to the date and time that the specified directory was last accessed. This value is expressed in local time.</returns>
      [SecurityCritical]
      public static DateTime GetLastAccessTime(string path)
      {
         return File.GetLastAccessTimeInternal(null, path, false, PathFormat.RelativePath).ToLocalTime();
      }

      /// <summary>[AlphaFS] Gets the date and time that the specified directory was last accessed.</summary>
      /// <param name="path">The directory for which to obtain access date and time information.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>A <see cref="System.DateTime"/> structure set to the date and time that the specified directory was last accessed. This value is expressed in local time.</returns>
      [SecurityCritical]
      public static DateTime GetLastAccessTime(string path, PathFormat pathFormat)
      {
         return File.GetLastAccessTimeInternal(null, path, false, pathFormat).ToLocalTime();
      }

      /// <summary>[AlphaFS] Gets the date and time that the specified directory was last accessed.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory for which to obtain access date and time information.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>A <see cref="System.DateTime"/> structure set to the date and time that the specified directory was last accessed. This value is expressed in local time.</returns>
      [SecurityCritical]
      public static DateTime GetLastAccessTime(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         return File.GetLastAccessTimeInternal(transaction, path, false, pathFormat).ToLocalTime();
      }

      /// <summary>[AlphaFS] Gets the date and time that the specified directory was last accessed.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory for which to obtain access date and time information.</param>
      /// <returns>A <see cref="System.DateTime"/> structure set to the date and time that the specified directory was last accessed. This value is expressed in local time.</returns>
      [SecurityCritical]
      public static DateTime GetLastAccessTime(KernelTransaction transaction, string path)
      {
         return File.GetLastAccessTimeInternal(transaction, path, false, PathFormat.RelativePath).ToLocalTime();
      }

      #endregion // GetLastAccessTime

      #region GetLastAccessTimeUtc

      /// <summary>Gets the date and time, in coordinated universal time (UTC), that the specified directory was last accessed.</summary>
      /// <param name="path">The directory for which to obtain access date and time information.</param>
      /// <returns>A <see cref="System.DateTime"/> structure set to the date and time that the specified directory was last accessed. This value is expressed in UTC time.</returns>
      [SecurityCritical]
      public static DateTime GetLastAccessTimeUtc(string path)
      {
         return File.GetLastAccessTimeInternal(null, path, true, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Gets the date and time, in coordinated universal time (UTC), that the specified directory was last accessed.</summary>
      /// <param name="path">The directory for which to obtain access date and time information.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>A <see cref="System.DateTime"/> structure set to the date and time that the specified directory was last accessed. This value is expressed in UTC time.</returns>
      [SecurityCritical]
      public static DateTime GetLastAccessTimeUtc(string path, PathFormat pathFormat)
      {
         return File.GetLastAccessTimeInternal(null, path, true, pathFormat);
      }

      /// <summary>[AlphaFS] Gets the date and time, in coordinated universal time (UTC), that the specified directory was last accessed.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory for which to obtain access date and time information.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>A <see cref="System.DateTime"/> structure set to the date and time that the specified directory was last accessed. This value is expressed in UTC time.</returns>
      [SecurityCritical]
      public static DateTime GetLastAccessTimeUtc(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         return File.GetLastAccessTimeInternal(transaction, path, true, pathFormat);
      }

      /// <summary>[AlphaFS] Gets the date and time, in coordinated universal time (UTC), that the specified directory was last accessed.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory for which to obtain access date and time information.</param>
      /// <returns>A <see cref="System.DateTime"/> structure set to the date and time that the specified directory was last accessed. This value is expressed in UTC time.</returns>
      [SecurityCritical]
      public static DateTime GetLastAccessTimeUtc(KernelTransaction transaction, string path)
      {
         return File.GetLastAccessTimeInternal(transaction, path, true, PathFormat.RelativePath);
      }

      #endregion // GetLastAccessTimeUtc

      #region GetLastWriteTime

      /// <summary>Gets the date and time that the specified directory was last written to.</summary>
      /// <param name="path">The directory for which to obtain write date and time information.</param>
      /// <returns>A <see cref="System.DateTime"/> structure set to the date and time that the specified directory was last written to. This value is expressed in local time.</returns>
      [SecurityCritical]
      public static DateTime GetLastWriteTime(string path)
      {
         return File.GetLastWriteTimeInternal(null, path, false, PathFormat.RelativePath).ToLocalTime();
      }

      /// <summary>[AlphaFS] Gets the date and time that the specified directory was last written to.</summary>
      /// <param name="path">The directory for which to obtain write date and time information.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>A <see cref="System.DateTime"/> structure set to the date and time that the specified directory was last written to. This value is expressed in local time.</returns>
      [SecurityCritical]
      public static DateTime GetLastWriteTime(string path, PathFormat pathFormat)
      {
         return File.GetLastWriteTimeInternal(null, path, false, pathFormat).ToLocalTime();
      }

      /// <summary>[AlphaFS] Gets the date and time that the specified directory was last written to.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory for which to obtain write date and time information.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>A <see cref="System.DateTime"/> structure set to the date and time that the specified directory was last written to. This value is expressed in local time.</returns>
      [SecurityCritical]
      public static DateTime GetLastWriteTime(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         return File.GetLastWriteTimeInternal(transaction, path, false, pathFormat).ToLocalTime();
      }

      /// <summary>[AlphaFS] Gets the date and time that the specified directory was last written to.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory for which to obtain write date and time information.</param>
      /// <returns>A <see cref="System.DateTime"/> structure set to the date and time that the specified directory was last written to. This value is expressed in local time.</returns>
      [SecurityCritical]
      public static DateTime GetLastWriteTime(KernelTransaction transaction, string path)
      {
         return File.GetLastWriteTimeInternal(transaction, path, false, PathFormat.RelativePath).ToLocalTime();
      }

      #endregion // GetLastWriteTime

      #region GetLastWriteTimeUtc

      /// <summary>Gets the date and time, in coordinated universal time (UTC) time, that the specified directory was last written to.</summary>
      /// <param name="path">The directory for which to obtain write date and time information.</param>
      /// <returns>A <see cref="System.DateTime"/> structure set to the date and time that the specified directory was last written to. This value is expressed in UTC time.</returns>
      [SecurityCritical]
      public static DateTime GetLastWriteTimeUtc(string path)
      {
         return File.GetLastWriteTimeInternal(null, path, true, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Gets the date and time, in coordinated universal time (UTC) time, that the specified directory was last written to.</summary>
      /// <param name="path">The directory for which to obtain write date and time information.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>A <see cref="System.DateTime"/> structure set to the date and time that the specified directory was last written to. This value is expressed in UTC time.</returns>
      [SecurityCritical]
      public static DateTime GetLastWriteTimeUtc(string path, PathFormat pathFormat)
      {
         return File.GetLastWriteTimeInternal(null, path, true, pathFormat);
      }

      /// <summary>[AlphaFS] Gets the date and time, in coordinated universal time (UTC) time, that the specified directory was last written to.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory for which to obtain write date and time information.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>A <see cref="System.DateTime"/> structure set to the date and time that the specified directory was last written to. This value is expressed in UTC time.</returns>
      [SecurityCritical]
      public static DateTime GetLastWriteTimeUtc(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         return File.GetLastWriteTimeInternal(transaction, path, true, pathFormat);
      }

      /// <summary>[AlphaFS] Gets the date and time, in coordinated universal time (UTC) time, that the specified directory was last written to.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory for which to obtain write date and time information.</param>
      /// <returns>A <see cref="System.DateTime"/> structure set to the date and time that the specified directory was last written to. This value is expressed in UTC time.</returns>
      [SecurityCritical]
      public static DateTime GetLastWriteTimeUtc(KernelTransaction transaction, string path)
      {
         return File.GetLastWriteTimeInternal(transaction, path, true, PathFormat.RelativePath);
      }

      #endregion // GetLastWriteTimeUtc

      #region GetChangeTime

      /// <summary>Gets the change date and time of the specified directory.</summary>
      /// <param name="path">
      ///   The directory for which to obtain creation date and time information.
      /// </param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>
      ///   A <see cref="System.DateTime"/> structure set to the change date and time for the specified
      ///   directory. This value is expressed in local time.
      /// </returns>
      [SecurityCritical]
      public static DateTime GetChangeTime(string path, PathFormat pathFormat)
      {
         return File.GetChangeTimeInternal(true, null, null, path, false, pathFormat);
      }

      /// <summary>Gets the change date and time of the specified directory.</summary>
      /// <returns>A <see cref="System.DateTime"/> structure set to the change date and time for the specified directory. This value is expressed in local time.</returns>
      /// <param name="path">The directory for which to obtain creation date and time information.</param>
      [SecurityCritical]
      public static DateTime GetChangeTime(string path)
      {
         return File.GetChangeTimeInternal(true, null, null, path, false, PathFormat.RelativePath);
      }

      /// <summary>Gets the change date and time of the specified directory.</summary>
      /// <param name="safeHandle">
      ///   An open handle to the directory from which to retrieve information.
      /// </param>
      /// <returns>
      ///   A <see cref="System.DateTime"/> structure set to the change date and time for the specified
      ///   directory. This value is expressed in local time.
      /// </returns>
      [SecurityCritical]
      public static DateTime GetChangeTime(SafeFileHandle safeHandle)
      {
         return File.GetChangeTimeInternal(true, null, safeHandle, null, false, PathFormat.RelativePath);
      }

      /// <summary>Gets the change date and time of the specified directory.</summary>
      /// <returns>A <see cref="System.DateTime"/> structure set to the change date and time for the specified directory. This value is expressed in local time.</returns>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory for which to obtain creation date and time information.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static DateTime GetChangeTime(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         return File.GetChangeTimeInternal(true, transaction, null, path, false, pathFormat);
      }

      /// <summary>Gets the change date and time of the specified directory.</summary>
      /// <returns>A <see cref="System.DateTime"/> structure set to the change date and time for the specified directory. This value is expressed in local time.</returns>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory for which to obtain creation date and time information.</param>
      [SecurityCritical]
      public static DateTime GetChangeTime(KernelTransaction transaction, string path)
      {
         return File.GetChangeTimeInternal(true, transaction, null, path, false, PathFormat.RelativePath);
      }

      #endregion // GetChangeTime

      #region GetChangeTimeUtc

      /// <summary>Gets the change date and time, in Coordinated Universal Time (UTC) format, of the specified directory.</summary>
      /// <returns>A <see cref="System.DateTime"/> structure set to the change date and time for the specified directory. This value is expressed in UTC time.</returns>
      /// <param name="path">The file for which to obtain change date and time information, in Coordinated Universal Time (UTC) format.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static DateTime GetChangeTimeUtc(string path, PathFormat pathFormat)
      {
         return File.GetChangeTimeInternal(true, null, null, path, true, pathFormat);
      }

      /// <summary>Gets the change date and time, in Coordinated Universal Time (UTC) format, of the specified directory.</summary>
      /// <returns>A <see cref="System.DateTime"/> structure set to the change date and time for the specified directory. This value is expressed in UTC time.</returns>
      /// <param name="path">The file for which to obtain change date and time information, in Coordinated Universal Time (UTC) format.</param>
      [SecurityCritical]
      public static DateTime GetChangeTimeUtc(string path)
      {
         return File.GetChangeTimeInternal(true, null, null, path, true, PathFormat.RelativePath);
      }

      /// <summary>
      ///   Gets the change date and time, in Coordinated Universal Time (UTC) format, of the specified
      ///   directory.
      /// </summary>
      /// <param name="safeHandle">
      ///   An open handle to the directory from which to retrieve information.
      /// </param>
      /// <returns>
      ///   A <see cref="System.DateTime"/> structure set to the change date and time for the specified
      ///   directory. This value is expressed in UTC time.
      /// </returns>
      [SecurityCritical]
      public static DateTime GetChangeTimeUtc(SafeFileHandle safeHandle)
      {
         return File.GetChangeTimeInternal(true, null, safeHandle, null, true, PathFormat.RelativePath);
      }

      /// <summary>Gets the change date and time, in Coordinated Universal Time (UTC) format, of the specified directory.</summary>
      /// <returns>A <see cref="System.DateTime"/> structure set to the change date and time for the specified directory. This value is expressed in UTC time.</returns>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file for which to obtain change date and time information, in Coordinated Universal Time (UTC) format.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static DateTime GetChangeTimeUtc(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         return File.GetChangeTimeInternal(true, transaction, null, path, true, pathFormat);
      }

      /// <summary>Gets the change date and time, in Coordinated Universal Time (UTC) format, of the specified directory.</summary>
      /// <returns>A <see cref="System.DateTime"/> structure set to the change date and time for the specified directory. This value is expressed in UTC time.</returns>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file for which to obtain change date and time information, in Coordinated Universal Time (UTC) format.</param>
      [SecurityCritical]
      public static DateTime GetChangeTimeUtc(KernelTransaction transaction, string path)
      {
         return File.GetChangeTimeInternal(true, transaction, null, path, true, PathFormat.RelativePath);
      }

      #endregion // GetChangeTimeUtc
   }
}
