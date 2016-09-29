/*  Copyright (C) 2008-2016 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
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

using System;
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
         return GetCreationTimeCore(null, path, false, PathFormat.RelativePath).ToLocalTime();
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
         return GetCreationTimeCore(null, path, false, pathFormat).ToLocalTime();
      }

      #region Transactional

      /// <summary>[AlphaFS] Gets the creation date and time of the specified file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file for which to obtain creation date and time information.</param>
      /// <returns>
      ///   A <see cref="System.DateTime"/> structure set to the creation date and time for the specified file. This value is expressed in
      ///   local time.
      /// </returns>
      [SecurityCritical]
      public static DateTime GetCreationTimeTransacted(KernelTransaction transaction, string path)
      {
         return GetCreationTimeCore(transaction, path, false, PathFormat.RelativePath).ToLocalTime();
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
      public static DateTime GetCreationTimeTransacted(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         return GetCreationTimeCore(transaction, path, false, pathFormat).ToLocalTime();
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
         return GetCreationTimeCore(null, path, true, PathFormat.RelativePath);
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
         return GetCreationTimeCore(null, path, true, pathFormat);
      }

      #region Transactional

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
      public static DateTime GetCreationTimeUtcTransacted(KernelTransaction transaction, string path)
      {
         return GetCreationTimeCore(transaction, path, true, PathFormat.RelativePath);
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
      public static DateTime GetCreationTimeUtcTransacted(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         return GetCreationTimeCore(transaction, path, true, pathFormat);
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
      internal static DateTime GetCreationTimeCore(KernelTransaction transaction, string path, bool returnUtc, PathFormat pathFormat)
      {
         NativeMethods.FILETIME creationTime = GetAttributesExCore<NativeMethods.WIN32_FILE_ATTRIBUTE_DATA>(transaction, path, pathFormat, false).ftCreationTime;

         return returnUtc
            ? DateTime.FromFileTimeUtc(creationTime)
            : DateTime.FromFileTime(creationTime);
      }

      #endregion // Internal Methods
   }
}
