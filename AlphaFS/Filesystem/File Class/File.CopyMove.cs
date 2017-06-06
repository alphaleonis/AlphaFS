/*  Copyright (C) 2008-2017 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
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
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;

namespace Alphaleonis.Win32.Filesystem
{
   public static partial class File
   {
      #region Copy

      #region .NET

      /// <summary>Copies an existing file to a new file. Overwriting a file of the same name is not allowed.
      /// <remarks>
      ///   <para>The attributes of the original file are retained in the copied file.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="sourcePath">The file to copy.</param>
      /// <param name="destinationPath">The name of the destination file. This cannot be a directory or an existing file.</param>
      [SecurityCritical]
      public static void Copy(string sourcePath, string destinationPath)
      {
         CopyMoveCore(null, false, null, sourcePath, destinationPath, CopyOptions.FailIfExists, null, false, null, null, PathFormat.RelativePath);
      }

      /// <summary>Copies an existing file to a new file. Overwriting a file of the same name is allowed.
      /// <remarks>
      ///   <para>The attributes of the original file are retained in the copied file.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="sourcePath">The file to copy. </param>
      /// <param name="destinationPath">The name of the destination file. This cannot be a directory.</param>
      /// <param name="overwrite"><see langword="true"/> if the destination file should ignoring the read-only and hidden attributes and overwrite; otherwise, <see langword="false"/>.</param>      
      [SecurityCritical]
      public static void Copy(string sourcePath, string destinationPath, bool overwrite)
      {
         CopyMoveCore(null, false, null, sourcePath, destinationPath, overwrite ? CopyOptions.None : CopyOptions.FailIfExists, null, false, null, null, PathFormat.RelativePath);
      }

      #endregion // .NET


      #region AlphaFS

      /// <summary>[AlphaFS] Copies an existing file to a new file. Overwriting a file of the same name is not allowed.
      /// <remarks>
      ///   <para>The attributes of the original file are retained in the copied file.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <returns>A <see cref="CopyMoveResult"/> class with details of the Copy action.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="sourcePath">The file to copy. </param>
      /// <param name="destinationPath">The name of the destination file. This cannot be a directory.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static CopyMoveResult Copy(string sourcePath, string destinationPath, PathFormat pathFormat)
      {
         return CopyMoveCore(null, false, null, sourcePath, destinationPath, CopyOptions.FailIfExists, null, false, null, null, pathFormat);
      }

      /// <summary>[AlphaFS] Copies an existing file to a new file. Overwriting a file of the same name is allowed.
      /// <remarks>
      ///   <para>The attributes of the original file are retained in the copied file.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <returns>A <see cref="CopyMoveResult"/> class with details of the Copy action.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="sourcePath">The file to copy. </param>
      /// <param name="destinationPath">The name of the destination file. This cannot be a directory.</param>
      /// <param name="overwrite"><see langword="true"/> if the destination file should ignoring the read-only and hidden attributes and overwrite; otherwise, <see langword="false"/>.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static CopyMoveResult Copy(string sourcePath, string destinationPath, bool overwrite, PathFormat pathFormat)
      {
         return CopyMoveCore(null, false, null, sourcePath, destinationPath, overwrite ? CopyOptions.None : CopyOptions.FailIfExists, null, false, null, null, pathFormat);
      }


      #region Transactional

      /// <summary>[AlphaFS] Copies an existing file to a new file. Overwriting a file of the same name is not allowed.
      /// <remarks>
      ///   <para>The attributes of the original file are retained in the copied file.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <returns>A <see cref="CopyMoveResult"/> class with details of the Copy action.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourcePath">The file to copy.</param>
      /// <param name="destinationPath">The name of the destination file. This cannot be a directory or an existing file.</param>
      [SecurityCritical]
      public static CopyMoveResult CopyTransacted(KernelTransaction transaction, string sourcePath, string destinationPath)
      {
         return CopyMoveCore(null, false, transaction, sourcePath, destinationPath, CopyOptions.FailIfExists, null, false, null, null, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Copies an existing file to a new file. Overwriting a file of the same name is not allowed.
      /// <remarks>
      ///   <para>The attributes of the original file are retained in the copied file.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <returns>A <see cref="CopyMoveResult"/> class with details of the Copy action.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourcePath">The file to copy. </param>
      /// <param name="destinationPath">The name of the destination file. This cannot be a directory.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static CopyMoveResult CopyTransacted(KernelTransaction transaction, string sourcePath, string destinationPath, PathFormat pathFormat)
      {
         return CopyMoveCore(null, false, transaction, sourcePath, destinationPath, CopyOptions.FailIfExists, null, false, null, null, pathFormat);
      }


      /// <summary>[AlphaFS] Copies an existing file to a new file. Overwriting a file of the same name is allowed.
      /// <remarks>
      ///   <para>The attributes of the original file are retained in the copied file.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <returns>A <see cref="CopyMoveResult"/> class with details of the Copy action.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourcePath">The file to copy. </param>
      /// <param name="destinationPath">The name of the destination file. This cannot be a directory.</param>
      /// <param name="overwrite"><see langword="true"/> if the destination file should ignoring the read-only and hidden attributes and overwrite; otherwise, <see langword="false"/>.</param>
      [SecurityCritical]
      public static CopyMoveResult CopyTransacted(KernelTransaction transaction, string sourcePath, string destinationPath, bool overwrite)
      {
         return CopyMoveCore(null, false, transaction, sourcePath, destinationPath, overwrite ? CopyOptions.None : CopyOptions.FailIfExists, null, false, null, null, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Copies an existing file to a new file. Overwriting a file of the same name is allowed.
      /// <remarks>
      ///   <para>The attributes of the original file are retained in the copied file.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <returns>A <see cref="CopyMoveResult"/> class with details of the Copy action.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourcePath">The file to copy. </param>
      /// <param name="destinationPath">The name of the destination file. This cannot be a directory.</param>
      /// <param name="overwrite"><see langword="true"/> if the destination file should ignoring the read-only and hidden attributes and overwrite; otherwise, <see langword="false"/>.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static CopyMoveResult CopyTransacted(KernelTransaction transaction, string sourcePath, string destinationPath, bool overwrite, PathFormat pathFormat)
      {
         return CopyMoveCore(null, false, transaction, sourcePath, destinationPath, overwrite ? CopyOptions.None : CopyOptions.FailIfExists, null, false, null, null, pathFormat);
      }

      #endregion // Transactional

      #endregion // AlphaFS

      #endregion // Copy


      #region Copy (CopyOptions)

      /// <summary>[AlphaFS] Copies an existing file to a new file. Overwriting a file of the same name is allowed. <see cref="CopyOptions"/> can be specified.
      /// <remarks>
      ///   <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      ///   <para>The attributes of the original file are retained in the copied file.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <returns>A <see cref="CopyMoveResult"/> class with details of the Copy action.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="sourcePath">The file to copy.</param>
      /// <param name="destinationPath">The name of the destination file. This cannot be a directory.</param>
      /// <param name="copyOptions"><see cref="CopyOptions"/> that specify how the file is to be copied. This parameter can be <see langword="null"/>.</param>
      [SecurityCritical]
      public static CopyMoveResult Copy(string sourcePath, string destinationPath, CopyOptions copyOptions)
      {
         return CopyMoveCore(null, false, null, sourcePath, destinationPath, copyOptions, null, false, null, null, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Copies an existing file to a new file. Overwriting a file of the same name is allowed. <see cref="CopyOptions"/> can be specified.
      /// <remarks>
      ///   <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      ///   <para>The attributes of the original file are retained in the copied file.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <returns>A <see cref="CopyMoveResult"/> class with details of the Copy action.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="sourcePath">The file to copy.</param>
      /// <param name="destinationPath">The name of the destination file. This cannot be a directory.</param>
      /// <param name="copyOptions"><see cref="CopyOptions"/> that specify how the file is to be copied. This parameter can be <see langword="null"/>.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static CopyMoveResult Copy(string sourcePath, string destinationPath, CopyOptions copyOptions, PathFormat pathFormat)
      {
         return CopyMoveCore(null, false, null, sourcePath, destinationPath, copyOptions, null, false, null, null, pathFormat);
      }


      /// <summary>[AlphaFS] Copies an existing file to a new file. Overwriting a file of the same name is allowed. <see cref="CopyOptions"/> can be specified.
      /// <remarks>
      ///   <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      ///   <para>The attributes of the original file are retained in the copied file.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <returns>A <see cref="CopyMoveResult"/> class with details of the Copy action.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="sourcePath">The file to copy.</param>
      /// <param name="destinationPath">The name of the destination file. This cannot be a directory.</param>
      /// <param name="copyOptions"><see cref="CopyOptions"/> that specify how the file is to be copied. This parameter can be <see langword="null"/>.</param>
      /// <param name="preserveDates"><see langword="true"/> if original Timestamps must be preserved, <see langword="false"/> otherwise.</param>
      [SecurityCritical]
      public static CopyMoveResult Copy(string sourcePath, string destinationPath, CopyOptions copyOptions, bool preserveDates)
      {
         return CopyMoveCore(null, false, null, sourcePath, destinationPath, copyOptions, null, preserveDates, null, null, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Copies an existing file to a new file. Overwriting a file of the same name is allowed. <see cref="CopyOptions"/> can be specified.
      /// <remarks>
      ///   <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      ///   <para>The attributes of the original file are retained in the copied file.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <returns>A <see cref="CopyMoveResult"/> class with details of the Copy action.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="sourcePath">The file to copy.</param>
      /// <param name="destinationPath">The name of the destination file. This cannot be a directory.</param>
      /// <param name="copyOptions"><see cref="CopyOptions"/> that specify how the file is to be copied. This parameter can be <see langword="null"/>.</param>
      /// <param name="preserveDates"><see langword="true"/> if original Timestamps must be preserved, <see langword="false"/> otherwise.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static CopyMoveResult Copy(string sourcePath, string destinationPath, CopyOptions copyOptions, bool preserveDates, PathFormat pathFormat)
      {
         return CopyMoveCore(null, false, null, sourcePath, destinationPath, copyOptions, null, preserveDates, null, null, pathFormat);
      }



      /// <summary>[AlphaFS] Copies an existing file to a new file. Overwriting a file of the same name is allowed.  <see cref="CopyOptions"/> can be specified,
      ///   <para>and the possibility of notifying the application of its progress through a callback function.</para>
      /// <remarks>
      ///   <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      ///   <para>The attributes of the original file are retained in the copied file.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <returns>A <see cref="CopyMoveResult"/> class with details of the Copy action.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="sourcePath">The file to copy.</param>
      /// <param name="destinationPath">The name of the destination file. This cannot be a directory.</param>
      /// <param name="copyOptions"><see cref="CopyOptions"/> that specify how the file is to be copied. This parameter can be <see langword="null"/>.</param>
      /// <param name="progressHandler">A callback function that is called each time another portion of the file has been copied. This parameter can be <see langword="null"/>.</param>
      /// <param name="userProgressData">The argument to be passed to the callback function. This parameter can be <see langword="null"/>.</param>
      [SecurityCritical]
      public static CopyMoveResult Copy(string sourcePath, string destinationPath, CopyOptions copyOptions, CopyMoveProgressRoutine progressHandler, object userProgressData)
      {
         return CopyMoveCore(null, false, null, sourcePath, destinationPath, copyOptions, null, false, progressHandler, userProgressData, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Copies an existing file to a new file. Overwriting a file of the same name is allowed.  <see cref="CopyOptions"/> can be specified,
      ///   <para>and the possibility of notifying the application of its progress through a callback function.</para>
      /// <remarks>
      ///   <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      ///   <para>The attributes of the original file are retained in the copied file.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <returns>A <see cref="CopyMoveResult"/> class with details of the Copy action.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="sourcePath">The file to copy.</param>
      /// <param name="destinationPath">The name of the destination file. This cannot be a directory.</param>
      /// <param name="copyOptions"><see cref="CopyOptions"/> that specify how the file is to be copied. This parameter can be <see langword="null"/>.</param>
      /// <param name="progressHandler">A callback function that is called each time another portion of the file has been copied. This parameter can be <see langword="null"/>.</param>
      /// <param name="userProgressData">The argument to be passed to the callback function. This parameter can be <see langword="null"/>.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static CopyMoveResult Copy(string sourcePath, string destinationPath, CopyOptions copyOptions, CopyMoveProgressRoutine progressHandler, object userProgressData, PathFormat pathFormat)
      {
         return CopyMoveCore(null, false, null, sourcePath, destinationPath, copyOptions, null, false, progressHandler, userProgressData, pathFormat);
      }


      /// <summary>[AlphaFS] Copies an existing file to a new file. Overwriting a file of the same name is allowed.  <see cref="CopyOptions"/> can be specified,
      ///   <para>and the possibility of notifying the application of its progress through a callback function.</para>
      /// <remarks>
      ///   <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      ///   <para>The attributes of the original file are retained in the copied file.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <returns>A <see cref="CopyMoveResult"/> class with details of the Copy action.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="sourcePath">The file to copy.</param>
      /// <param name="destinationPath">The name of the destination file. This cannot be a directory.</param>
      /// <param name="copyOptions"><see cref="CopyOptions"/> that specify how the file is to be copied. This parameter can be <see langword="null"/>.</param>
      /// <param name="preserveDates"><see langword="true"/> if original Timestamps must be preserved, <see langword="false"/> otherwise.</param>
      /// <param name="progressHandler">A callback function that is called each time another portion of the file has been copied. This parameter can be <see langword="null"/>.</param>
      /// <param name="userProgressData">The argument to be passed to the callback function. This parameter can be <see langword="null"/>.</param>
      [SecurityCritical]
      public static CopyMoveResult Copy(string sourcePath, string destinationPath, CopyOptions copyOptions, bool preserveDates, CopyMoveProgressRoutine progressHandler, object userProgressData)
      {
         return CopyMoveCore(null, false, null, sourcePath, destinationPath, copyOptions, null, preserveDates, progressHandler, userProgressData, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Copies an existing file to a new file. Overwriting a file of the same name is allowed.  <see cref="CopyOptions"/> can be specified,
      ///   <para>and the possibility of notifying the application of its progress through a callback function.</para>
      /// <remarks>
      ///   <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      ///   <para>The attributes of the original file are retained in the copied file.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <returns>A <see cref="CopyMoveResult"/> class with details of the Copy action.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="sourcePath">The file to copy.</param>
      /// <param name="destinationPath">The name of the destination file. This cannot be a directory.</param>
      /// <param name="copyOptions"><see cref="CopyOptions"/> that specify how the file is to be copied. This parameter can be <see langword="null"/>.</param>
      /// <param name="preserveDates"><see langword="true"/> if original Timestamps must be preserved, <see langword="false"/> otherwise.</param>
      /// <param name="progressHandler">A callback function that is called each time another portion of the file has been copied. This parameter can be <see langword="null"/>.</param>
      /// <param name="userProgressData">The argument to be passed to the callback function. This parameter can be <see langword="null"/>.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static CopyMoveResult Copy(string sourcePath, string destinationPath, CopyOptions copyOptions, bool preserveDates, CopyMoveProgressRoutine progressHandler, object userProgressData, PathFormat pathFormat)
      {
         return CopyMoveCore(null, false, null, sourcePath, destinationPath, copyOptions, null, preserveDates, progressHandler, userProgressData, pathFormat);
      }


      #region Transactional

      /// <summary>[AlphaFS] Copies an existing file to a new file. Overwriting a file of the same name is allowed. <see cref="CopyOptions"/> can be specified.
      /// <remarks>
      ///   <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      ///   <para>The attributes of the original file are retained in the copied file.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <returns>A <see cref="CopyMoveResult"/> class with details of the Copy action.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourcePath">The file to copy.</param>
      /// <param name="destinationPath">The name of the destination file. This cannot be a directory.</param>
      /// <param name="copyOptions"><see cref="CopyOptions"/> that specify how the file is to be copied. This parameter can be <see langword="null"/>.</param>
      [SecurityCritical]
      public static CopyMoveResult CopyTransacted(KernelTransaction transaction, string sourcePath, string destinationPath, CopyOptions copyOptions)
      {
         return CopyMoveCore(null, false, transaction, sourcePath, destinationPath, copyOptions, null, false, null, null, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Copies an existing file to a new file. Overwriting a file of the same name is allowed. <see cref="CopyOptions"/> can be specified.
      /// <remarks>
      ///   <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      ///   <para>The attributes of the original file are retained in the copied file.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <returns>A <see cref="CopyMoveResult"/> class with details of the Copy action.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourcePath">The file to copy.</param>
      /// <param name="destinationPath">The name of the destination file. This cannot be a directory.</param>
      /// <param name="copyOptions"><see cref="CopyOptions"/> that specify how the file is to be copied. This parameter can be <see langword="null"/>.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static CopyMoveResult CopyTransacted(KernelTransaction transaction, string sourcePath, string destinationPath, CopyOptions copyOptions, PathFormat pathFormat)
      {
         return CopyMoveCore(null, false, transaction, sourcePath, destinationPath, copyOptions, null, false, null, null, pathFormat);
      }


      /// <summary>[AlphaFS] Copies an existing file to a new file. Overwriting a file of the same name is allowed. <see cref="CopyOptions"/> can be specified.
      /// <remarks>
      ///   <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      ///   <para>The attributes of the original file are retained in the copied file.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <returns>A <see cref="CopyMoveResult"/> class with details of the Copy action.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourcePath">The file to copy.</param>
      /// <param name="destinationPath">The name of the destination file. This cannot be a directory.</param>
      /// <param name="copyOptions"><see cref="CopyOptions"/> that specify how the file is to be copied. This parameter can be <see langword="null"/>.</param>
      /// <param name="preserveDates"><see langword="true"/> if original Timestamps must be preserved, <see langword="false"/> otherwise.</param>
      [SecurityCritical]
      public static CopyMoveResult CopyTransacted(KernelTransaction transaction, string sourcePath, string destinationPath, CopyOptions copyOptions, bool preserveDates)
      {
         return CopyMoveCore(null, false, transaction, sourcePath, destinationPath, copyOptions, null, preserveDates, null, null, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Copies an existing file to a new file. Overwriting a file of the same name is allowed. <see cref="CopyOptions"/> can be specified.
      /// <remarks>
      ///   <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      ///   <para>The attributes of the original file are retained in the copied file.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <returns>A <see cref="CopyMoveResult"/> class with details of the Copy action.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourcePath">The file to copy.</param>
      /// <param name="destinationPath">The name of the destination file. This cannot be a directory.</param>
      /// <param name="copyOptions"><see cref="CopyOptions"/> that specify how the file is to be copied. This parameter can be <see langword="null"/>.</param>
      /// <param name="preserveDates"><see langword="true"/> if original Timestamps must be preserved, <see langword="false"/> otherwise.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static CopyMoveResult CopyTransacted(KernelTransaction transaction, string sourcePath, string destinationPath, CopyOptions copyOptions, bool preserveDates, PathFormat pathFormat)
      {
         return CopyMoveCore(null, false, transaction, sourcePath, destinationPath, copyOptions, null, preserveDates, null, null, pathFormat);
      }



      /// <summary>[AlphaFS] Copies an existing file to a new file. Overwriting a file of the same name is allowed.  <see cref="CopyOptions"/> can be specified,
      ///   <para>and the possibility of notifying the application of its progress through a callback function.</para>
      /// <remarks>
      ///   <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      ///   <para>The attributes of the original file are retained in the copied file.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <returns>A <see cref="CopyMoveResult"/> class with details of the Copy action.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourcePath">The file to copy.</param>
      /// <param name="destinationPath">The name of the destination file. This cannot be a directory.</param>
      /// <param name="copyOptions"><see cref="CopyOptions"/> that specify how the file is to be copied. This parameter can be <see langword="null"/>.</param>
      /// <param name="progressHandler">A callback function that is called each time another portion of the file has been copied. This parameter can be <see langword="null"/>.</param>
      /// <param name="userProgressData">The argument to be passed to the callback function. This parameter can be <see langword="null"/>.</param>
      [SecurityCritical]
      public static CopyMoveResult CopyTransacted(KernelTransaction transaction, string sourcePath, string destinationPath, CopyOptions copyOptions, CopyMoveProgressRoutine progressHandler, object userProgressData)
      {
         return CopyMoveCore(null, false, transaction, sourcePath, destinationPath, copyOptions, null, false, progressHandler, userProgressData, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Copies an existing file to a new file. Overwriting a file of the same name is allowed.  <see cref="CopyOptions"/> can be specified,
      ///   <para>and the possibility of notifying the application of its progress through a callback function.</para>
      /// <remarks>
      ///   <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      ///   <para>The attributes of the original file are retained in the copied file.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <returns>A <see cref="CopyMoveResult"/> class with details of the Copy action.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourcePath">The file to copy.</param>
      /// <param name="destinationPath">The name of the destination file. This cannot be a directory.</param>
      /// <param name="copyOptions"><see cref="CopyOptions"/> that specify how the file is to be copied. This parameter can be <see langword="null"/>.</param>
      /// <param name="progressHandler">A callback function that is called each time another portion of the file has been copied. This parameter can be <see langword="null"/>.</param>
      /// <param name="userProgressData">The argument to be passed to the callback function. This parameter can be <see langword="null"/>.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static CopyMoveResult CopyTransacted(KernelTransaction transaction, string sourcePath, string destinationPath, CopyOptions copyOptions, CopyMoveProgressRoutine progressHandler, object userProgressData, PathFormat pathFormat)
      {
         return CopyMoveCore(null, false, transaction, sourcePath, destinationPath, copyOptions, null, false, progressHandler, userProgressData, pathFormat);
      }


      /// <summary>[AlphaFS] Copies an existing file to a new file. Overwriting a file of the same name is allowed.  <see cref="CopyOptions"/> can be specified,
      ///   <para>and the possibility of notifying the application of its progress through a callback function.</para>
      /// <remarks>
      ///   <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      ///   <para>The attributes of the original file are retained in the copied file.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <returns>A <see cref="CopyMoveResult"/> class with details of the Copy action.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourcePath">The file to copy.</param>
      /// <param name="destinationPath">The name of the destination file. This cannot be a directory.</param>
      /// <param name="copyOptions"><see cref="CopyOptions"/> that specify how the file is to be copied. This parameter can be <see langword="null"/>.</param>
      /// <param name="preserveDates"><see langword="true"/> if original Timestamps must be preserved, <see langword="false"/> otherwise.</param>
      /// <param name="progressHandler">A callback function that is called each time another portion of the file has been copied. This parameter can be <see langword="null"/>.</param>
      /// <param name="userProgressData">The argument to be passed to the callback function. This parameter can be <see langword="null"/>.</param>
      [SecurityCritical]
      public static CopyMoveResult CopyTransacted(KernelTransaction transaction, string sourcePath, string destinationPath, CopyOptions copyOptions, bool preserveDates, CopyMoveProgressRoutine progressHandler, object userProgressData)
      {
         return CopyMoveCore(null, false, transaction, sourcePath, destinationPath, copyOptions, null, preserveDates, progressHandler, userProgressData, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Copies an existing file to a new file. Overwriting a file of the same name is allowed.  <see cref="CopyOptions"/> can be specified,
      ///   <para>and the possibility of notifying the application of its progress through a callback function.</para>
      /// <remarks>
      ///   <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      ///   <para>The attributes of the original file are retained in the copied file.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <returns>A <see cref="CopyMoveResult"/> class with details of the Copy action.</returns>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourcePath">The file to copy.</param>
      /// <param name="destinationPath">The name of the destination file. This cannot be a directory.</param>
      /// <param name="copyOptions"><see cref="CopyOptions"/> that specify how the file is to be copied. This parameter can be <see langword="null"/>.</param>
      /// <param name="preserveDates"><see langword="true"/> if original Timestamps must be preserved, <see langword="false"/> otherwise.</param>
      /// <param name="progressHandler">A callback function that is called each time another portion of the file has been copied. This parameter can be <see langword="null"/>.</param>
      /// <param name="userProgressData">The argument to be passed to the callback function. This parameter can be <see langword="null"/>.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      [SecurityCritical]
      public static CopyMoveResult CopyTransacted(KernelTransaction transaction, string sourcePath, string destinationPath, CopyOptions copyOptions, bool preserveDates, CopyMoveProgressRoutine progressHandler, object userProgressData, PathFormat pathFormat)
      {
         return CopyMoveCore(null, false, transaction, sourcePath, destinationPath, copyOptions, null, preserveDates, progressHandler, userProgressData, pathFormat);
      }

      #endregion // Transactional

      #endregion // Copy (CopyOptions)


      #region Move

      #region .NET

      /// <summary>Moves a specified file to a new location, providing the option to specify a new file name.
      /// <remarks>
      ///   <para>This method works across disk volumes.</para>
      ///   <para>Note that if you attempt to replace a file by moving a file of the same name into that directory, you get an <see cref="IOException"/>.</para>
      ///   <para>You cannot use the Move method to overwrite an existing file.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="sourcePath">The name of the file to move.</param>
      /// <param name="destinationPath">The new path for the file.</param>
      [SecurityCritical]
      public static void Move(string sourcePath, string destinationPath)
      {
         CopyMoveCore(null, false, null, sourcePath, destinationPath, null, MoveOptions.CopyAllowed, false, null, null, PathFormat.RelativePath);
      }

      #endregion // .NET

      #region AlphaFS

      #region Non-Transactional

      /// <summary>[AlphaFS] Moves a specified file to a new location, providing the option to specify a new file name.
      /// <remarks>
      ///   <para>This method works across disk volumes.</para>
      ///   <para>Note that if you attempt to replace a file by moving a file of the same name into that directory, you get an <see cref="IOException"/>.</para>
      ///   <para>You cannot use the Move method to overwrite an existing file.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <returns>A <see cref="CopyMoveResult"/> class with the status of the Move action.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="sourcePath">The name of the file to move.</param>
      /// <param name="destinationPath">The new path for the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static CopyMoveResult Move(string sourcePath, string destinationPath, PathFormat pathFormat)
      {
         return CopyMoveCore(null, false, null, sourcePath, destinationPath, null, MoveOptions.CopyAllowed, false, null, null, pathFormat);
      }

      #endregion // Non-Transactional

      #region Transactional

      /// <summary>[AlphaFS] Moves a specified file to a new location, providing the option to specify a new file name.
      /// <remarks>
      ///   <para>This method works across disk volumes.</para>
      ///   <para>Note that if you attempt to replace a file by moving a file of the same name into that directory, you get an <see cref="IOException"/>.</para>
      ///   <para>You cannot use the Move method to overwrite an existing file.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <returns>A <see cref="CopyMoveResult"/> class with the status of the Move action.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourcePath">The name of the file to move.</param>
      /// <param name="destinationPath">The new path for the file.</param>
      [SecurityCritical]
      public static CopyMoveResult MoveTransacted(KernelTransaction transaction, string sourcePath, string destinationPath)
      {
         return CopyMoveCore(null, false, transaction, sourcePath, destinationPath, null, MoveOptions.CopyAllowed, false, null, null, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Moves a specified file to a new location, providing the option to specify a new file name.
      /// <remarks>
      ///   <para>This method works across disk volumes.</para>
      ///   <para>Note that if you attempt to replace a file by moving a file of the same name into that directory, you get an <see cref="IOException"/>.</para>
      ///   <para>You cannot use the Move method to overwrite an existing file.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <returns>A <see cref="CopyMoveResult"/> class with the status of the Move action.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourcePath">The name of the file to move.</param>
      /// <param name="destinationPath">The new path for the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static CopyMoveResult MoveTransacted(KernelTransaction transaction, string sourcePath, string destinationPath, PathFormat pathFormat)
      {
         return CopyMoveCore(null, false, transaction, sourcePath, destinationPath, null, MoveOptions.CopyAllowed, false, null, null, pathFormat);
      }

      #endregion // Transactional

      #endregion // AlphaFS

      #endregion // Move


      #region Move (MoveOptions)

      #region Non-Transactional

      /// <summary>[AlphaFS] Moves a specified file to a new location, providing the option to specify a new file name.
      /// <remarks>
      ///   <para>This method works across disk volumes.</para>
      ///   <para>Note that if you attempt to replace a file by moving a file of the same name into that directory, you get an <see cref="IOException"/>.</para>
      ///   <para>You cannot use the Move method to overwrite an existing file.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <returns>A <see cref="CopyMoveResult"/> class with the status of the Move action.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="sourcePath">The name of the file to move.</param>
      /// <param name="destinationPath">The new path for the file.</param>
      /// <param name="moveOptions"><see cref="MoveOptions"/> that specify how the file is to be moved. This parameter can be <see langword="null"/>.</param>
      [SecurityCritical]
      public static CopyMoveResult Move(string sourcePath, string destinationPath, MoveOptions moveOptions)
      {
         return CopyMoveCore(null, false, null, sourcePath, destinationPath, null, moveOptions, false, null, null, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Moves a specified file to a new location, providing the option to specify a new file name.
      /// <remarks>
      ///   <para>This method works across disk volumes.</para>
      ///   <para>Note that if you attempt to replace a file by moving a file of the same name into that directory, you get an <see cref="IOException"/>.</para>
      ///   <para>You cannot use the Move method to overwrite an existing file.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <returns>A <see cref="CopyMoveResult"/> class with the status of the Move action.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="sourcePath">The name of the file to move.</param>
      /// <param name="destinationPath">The new path for the file.</param>
      /// <param name="moveOptions"><see cref="MoveOptions"/> that specify how the file is to be moved. This parameter can be <see langword="null"/>.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static CopyMoveResult Move(string sourcePath, string destinationPath, MoveOptions moveOptions, PathFormat pathFormat)
      {
         return CopyMoveCore(null, false, null, sourcePath, destinationPath, null, moveOptions, false, null, null, pathFormat);
      }



      /// <summary>[AlphaFS] Moves a specified file to a new location, providing the option to specify a new file name.
      /// <remarks>
      ///   <para>This method works across disk volumes.</para>
      ///   <para>Note that if you attempt to replace a file by moving a file of the same name into that directory, you get an <see cref="IOException"/>.</para>
      ///   <para>You cannot use the Move method to overwrite an existing file.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <returns>A <see cref="CopyMoveResult"/> class with the status of the Move action.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="sourcePath">The name of the file to move.</param>
      /// <param name="destinationPath">The new path for the file.</param>
      /// <param name="moveOptions"><see cref="MoveOptions"/> that specify how the file is to be moved. This parameter can be <see langword="null"/>.</param>
      /// <param name="progressHandler">A callback function that is called each time another portion of the file has been moved. This parameter can be <see langword="null"/>.</param>
      /// <param name="userProgressData">The argument to be passed to the callback function. This parameter can be <see langword="null"/>.</param>
      [SecurityCritical]
      public static CopyMoveResult Move(string sourcePath, string destinationPath, MoveOptions moveOptions, CopyMoveProgressRoutine progressHandler, object userProgressData)
      {
         return CopyMoveCore(null, false, null, sourcePath, destinationPath, null, moveOptions, false, progressHandler, userProgressData, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Moves a specified file to a new location, providing the option to specify a new file name.
      /// <returns>A <see cref="CopyMoveResult"/> class with the status of the Move action.</returns>
      /// <remarks>
      ///   <para>This method works across disk volumes.</para>
      ///   <para>Note that if you attempt to replace a file by moving a file of the same name into that directory, you get an <see cref="IOException"/>.</para>
      ///   <para>You cannot use the Move method to overwrite an existing file.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <returns>A <see cref="CopyMoveResult"/> class with the status of the Move action.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="sourcePath">The name of the file to move.</param>
      /// <param name="destinationPath">The new path for the file.</param>
      /// <param name="moveOptions"><see cref="MoveOptions"/> that specify how the file is to be moved. This parameter can be <see langword="null"/>.</param>
      /// <param name="progressHandler">A callback function that is called each time another portion of the file has been moved. This parameter can be <see langword="null"/>.</param>
      /// <param name="userProgressData">The argument to be passed to the callback function. This parameter can be <see langword="null"/>.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static CopyMoveResult Move(string sourcePath, string destinationPath, MoveOptions moveOptions, CopyMoveProgressRoutine progressHandler, object userProgressData, PathFormat pathFormat)
      {
         return CopyMoveCore(null, false, null, sourcePath, destinationPath, null, moveOptions, false, progressHandler, userProgressData, pathFormat);
      }

      #endregion // Non-Transactional

      #region Transactional

      /// <summary>[AlphaFS] Moves a specified file to a new location, providing the option to specify a new file name.
      /// <remarks>
      ///   <para>This method works across disk volumes.</para>
      ///   <para>Note that if you attempt to replace a file by moving a file of the same name into that directory, you get an <see cref="IOException"/>.</para>
      ///   <para>You cannot use the Move method to overwrite an existing file.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <returns>A <see cref="CopyMoveResult"/> class with the status of the Move action.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourcePath">The name of the file to move.</param>
      /// <param name="destinationPath">The new path for the file.</param>
      /// <param name="moveOptions"><see cref="MoveOptions"/> that specify how the file is to be moved. This parameter can be <see langword="null"/>.</param>
      [SecurityCritical]
      public static CopyMoveResult MoveTransacted(KernelTransaction transaction, string sourcePath, string destinationPath, MoveOptions moveOptions)
      {
         return CopyMoveCore(null, false, transaction, sourcePath, destinationPath, null, moveOptions, false, null, null, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Moves a specified file to a new location, providing the option to specify a new file name.
      /// <remarks>
      ///   <para>This method works across disk volumes.</para>
      ///   <para>Note that if you attempt to replace a file by moving a file of the same name into that directory, you get an <see cref="IOException"/>.</para>
      ///   <para>You cannot use the Move method to overwrite an existing file.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <returns>A <see cref="CopyMoveResult"/> class with the status of the Move action.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourcePath">The name of the file to move.</param>
      /// <param name="destinationPath">The new path for the file.</param>
      /// <param name="moveOptions"><see cref="MoveOptions"/> that specify how the file is to be moved. This parameter can be <see langword="null"/>.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static CopyMoveResult MoveTransacted(KernelTransaction transaction, string sourcePath, string destinationPath, MoveOptions moveOptions, PathFormat pathFormat)
      {
         return CopyMoveCore(null, false, transaction, sourcePath, destinationPath, null, moveOptions, false, null, null, pathFormat);
      }



      /// <summary>[AlphaFS] Moves a specified file to a new location, providing the option to specify a new file name.
      /// <remarks>
      ///   <para>This method works across disk volumes.</para>
      ///   <para>Note that if you attempt to replace a file by moving a file of the same name into that directory, you get an <see cref="IOException"/>.</para>
      ///   <para>You cannot use the Move method to overwrite an existing file.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <returns>A <see cref="CopyMoveResult"/> class with the status of the Move action.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourcePath">The name of the file to move.</param>
      /// <param name="destinationPath">The new path for the file.</param>
      /// <param name="moveOptions"><see cref="MoveOptions"/> that specify how the file is to be moved. This parameter can be <see langword="null"/>.</param>
      /// <param name="progressHandler">A callback function that is called each time another portion of the file has been moved. This parameter can be <see langword="null"/>.</param>
      /// <param name="userProgressData">The argument to be passed to the callback function. This parameter can be <see langword="null"/>.</param>
      [SecurityCritical]
      public static CopyMoveResult MoveTransacted(KernelTransaction transaction, string sourcePath, string destinationPath, MoveOptions moveOptions, CopyMoveProgressRoutine progressHandler, object userProgressData)
      {
         return CopyMoveCore(null, false, transaction, sourcePath, destinationPath, null, moveOptions, false, progressHandler, userProgressData, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Moves a specified file to a new location, providing the option to specify a new file name.
      /// <remarks>
      ///   <para>This method works across disk volumes.</para>
      ///   <para>Note that if you attempt to replace a file by moving a file of the same name into that directory, you get an <see cref="IOException"/>.</para>
      ///   <para>You cannot use the Move method to overwrite an existing file.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <returns>A <see cref="CopyMoveResult"/> class with the status of the Move action.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourcePath">The name of the file to move.</param>
      /// <param name="destinationPath">The new path for the file.</param>
      /// <param name="moveOptions"><see cref="MoveOptions"/> that specify how the file is to be moved. This parameter can be <see langword="null"/>.</param>
      /// <param name="progressHandler">A callback function that is called each time another portion of the file has been moved. This parameter can be <see langword="null"/>.</param>
      /// <param name="userProgressData">The argument to be passed to the callback function. This parameter can be <see langword="null"/>.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static CopyMoveResult MoveTransacted(KernelTransaction transaction, string sourcePath, string destinationPath, MoveOptions moveOptions, CopyMoveProgressRoutine progressHandler, object userProgressData, PathFormat pathFormat)
      {
         return CopyMoveCore(null, false, transaction, sourcePath, destinationPath, null, moveOptions, false, progressHandler, userProgressData, pathFormat);
      }

      #endregion // Transactional

      #endregion // Move (MoveOptions)


      #region Internal Methods

      /// <summary>Copy/move a Non-/Transacted file or directory including its children to a new location, <see cref="CopyOptions"/> or <see cref="MoveOptions"/> can be specified,
      ///   and the possibility of notifying the application of its progress through a callback function.
      /// <remarks>
      ///   <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      ///   <para>You cannot use the Move method to overwrite an existing file, unless
      ///   <paramref name="moveOptions"/> contains <see cref="MoveOptions.ReplaceExisting"/>.</para>
      ///   <para>This Move method works across disk volumes, and it does not throw an exception if the
      ///   source and destination are the same. </para>
      ///   <para>Note that if you attempt to replace a file by moving a file of the same name into
      ///   that directory, you get an IOException.</para>
      /// </remarks>
      /// </summary>
      /// <returns>A <see cref="CopyMoveResult"/> class with the status of the Copy or Move action.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="copyMoveResult"></param>
      /// <param name="isFolder">Specifies that <paramref name="sourcePath"/> and <paramref name="destinationPath"/> is either a file or directory.</param>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourcePath">The source directory path plus file name.</param>
      /// <param name="destinationPath">The destination directory path plus file name.</param>
      /// <param name="copyOptions"><see cref="CopyOptions"/> that specify how the file is to be copied. This parameter can be <see langword="null"/>.</param>
      /// <param name="moveOptions">Flags that specify how the file or directory is to be moved. This parameter can be <see langword="null"/>.</param>
      /// <param name="preserveDates"><see langword="true"/> if original Timestamps must be preserved, <see langword="false"/> otherwise. This parameter is ignored for move operations.</param>
      /// <param name="progressHandler">A callback function that is called each time another portion of the file has been copied/moved. This parameter can be <see langword="null"/>.</param>
      /// <param name="userProgressData">The argument to be passed to the callback function. This parameter can be <see langword="null"/>.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
      [SecurityCritical]
      internal static CopyMoveResult CopyMoveCore(CopyMoveResult copyMoveResult, bool isFolder, KernelTransaction transaction, string sourcePath, string destinationPath, CopyOptions? copyOptions, MoveOptions? moveOptions, bool preserveDates, CopyMoveProgressRoutine progressHandler, object userProgressData, PathFormat pathFormat)
      {
         #region Setup

         // MSDN: .NET3.5+: IOException: The sourceDirName and destDirName parameters refer to the same file or directory.
         // Do not use StringComparison.OrdinalIgnoreCase to allow renaming a folder with different casing.
         if (!Utils.IsNullOrWhiteSpace(sourcePath) && sourcePath.Equals(destinationPath))
            NativeError.ThrowException(Win32Errors.ERROR_SAME_DRIVE, destinationPath);


         // Determine Copy or Move action.
         var isCopy = IsCopyAction(copyOptions, moveOptions);
         var isMove = !isCopy;
         var isSingleFileCopyMoveAction = null == copyMoveResult && !isFolder;


         // Only preserve when FSO is a file Copy action.
         preserveDates = preserveDates && isCopy && !isFolder;


         // Determine if MoveOptions.DelayUntilReboot is applicable.
         var delayUntilReboot = isMove && VerifyDelayUntilReboot(sourcePath, moveOptions, pathFormat);



         var sourcePathLp = sourcePath;
         var destinationPathLp = destinationPath;

         if (pathFormat != PathFormat.LongFullPath)
         {
            Path.CheckSupportedPathFormat(sourcePath, true, true);
            sourcePathLp = Path.GetExtendedLengthPathCore(transaction, sourcePath, pathFormat, GetFullPathOptions.RemoveTrailingDirectorySeparator);

            if (!delayUntilReboot)
            {
               Path.CheckSupportedPathFormat(destinationPath, true, true);
               destinationPathLp = Path.GetExtendedLengthPathCore(transaction, destinationPath, pathFormat, GetFullPathOptions.RemoveTrailingDirectorySeparator);
            }
         }


         // MSDN: If this flag is set to TRUE during the copy/move operation, the operation is canceled.
         // Otherwise, the copy/move operation will continue to completion.
         bool cancel;

         var raiseException = null == progressHandler;

         // Setup callback function for progress notifications.
         var routine = !raiseException
            ? (totalFileSize, totalBytesTransferred, streamSize, streamBytesTransferred, dwStreamNumber, dwCallbackReason, hSourceFile, hDestinationFile, lpData) =>
               progressHandler(totalFileSize, totalBytesTransferred, streamSize, streamBytesTransferred, dwStreamNumber, dwCallbackReason, userProgressData)
            : (NativeMethods.NativeCopyMoveProgressRoutine)null;

         #endregion // Setup


         // If the move happened on the same drive, we have no knowledge of the number of files/folders.
         // However, we do know that the one file was moved successfully.
         var cmr = copyMoveResult ?? new CopyMoveResult(sourcePath, destinationPath, isCopy, isFolder, preserveDates, false);


         startCopyMove:

         cmr.ErrorCode = 0;

         var success = null == transaction || !NativeMethods.IsAtLeastWindowsVista

            ? isMove
               // MoveFileWithProgress() / MoveFileTransacted()
               // In the ANSI version of this function, the name is limited to MAX_PATH characters.
               // To extend this limit to 32,767 wide characters, call the Unicode version of the function and prepend "\\?\" to the path.
               // 2013-04-15: MSDN confirms LongPath usage.

               // CopyFileEx() / CopyFileTransacted()
               // In the ANSI version of this function, the name is limited to MAX_PATH characters.
               // To extend this limit to 32,767 wide characters, call the Unicode version of the function and prepend "\\?\" to the path.
               // 2013-04-15: MSDN confirms LongPath usage.


               // Note: MoveFileXxx fails if one of the paths is a UNC path, even though both paths refer to the same volume.
               // For example, src = C:\TempSrc and dst = \\localhost\C$\TempDst

               // MoveFileXxx fails if it cannot access the registry. The function stores the locations of the files to be renamed at restart in the following registry value:
               //
               //    HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\PendingFileRenameOperations
               //
               // This registry value is of type REG_MULTI_SZ. Each rename operation stores one of the following NULL-terminated strings, depending on whether the rename is a delete or not:
               //
               //    szDstFile\0\0              : indicates that the file szDstFile is to be deleted on reboot.
               //    szSrcFile\0szDstFile\0     : indicates that szSrcFile is to be renamed szDstFile on reboot.


               ? NativeMethods.MoveFileWithProgress(sourcePathLp, destinationPathLp, routine, IntPtr.Zero, (MoveOptions)moveOptions)
               : NativeMethods.CopyFileEx(sourcePathLp, destinationPathLp, routine, IntPtr.Zero, out cancel, (CopyOptions)copyOptions)

            : isMove
               ? NativeMethods.MoveFileTransacted(sourcePathLp, destinationPathLp, routine, IntPtr.Zero, (MoveOptions)moveOptions, transaction.SafeHandle)
               : NativeMethods.CopyFileTransacted(sourcePathLp, destinationPathLp, routine, IntPtr.Zero, out cancel, (CopyOptions)copyOptions, transaction.SafeHandle);


         var lastError = (uint)Marshal.GetLastWin32Error();


         if (!success)
         {
            cmr.ErrorCode = (int)lastError;

            if (lastError == Win32Errors.ERROR_REQUEST_ABORTED)
            {
               // MSDN:
               //
               // If lpProgressRoutine returns PROGRESS_CANCEL due to the user canceling the operation,
               // CopyFileEx will return zero and GetLastError will return ERROR_REQUEST_ABORTED.
               // In this case, the partially copied destination file is deleted.
               //
               // If lpProgressRoutine returns PROGRESS_STOP due to the user stopping the operation,
               // CopyFileEx will return zero and GetLastError will return ERROR_REQUEST_ABORTED.
               // In this case, the partially copied destination file is left intact.

               cmr.IsCanceled = true;
            }

            else if (raiseException)
            {
               switch (lastError)
               {
                  // File.Copy()
                  // File.Move()
                  // Directory.Move()
                  // MSDN: .NET 3.5+: DirectoryNotFoundException: The path specified in sourcePath or destinationPath is invalid (for example, it is on an unmapped drive).
                  case Win32Errors.ERROR_PATH_NOT_FOUND:

                  // File.Copy()
                  // File.Move()
                  // MSDN: .NET 3.5+: FileNotFoundException: sourcePath was not found. 
                  case Win32Errors.ERROR_FILE_NOT_FOUND:
                     if (isFolder)
                        lastError = Win32Errors.ERROR_PATH_NOT_FOUND;

                     NativeError.ThrowException(lastError, sourcePathLp);
                     break;


                  // File.Copy()
                  // Directory.Copy()
                  case Win32Errors.ERROR_ALREADY_EXISTS:
                  case Win32Errors.ERROR_FILE_EXISTS:
                     if (isFolder)
                        lastError = Win32Errors.ERROR_ALREADY_EXISTS;

                     NativeError.ThrowException(lastError, destinationPathLp);
                     break;


                  default:
                     var destExists = ExistsCore(isFolder, transaction, destinationPathLp, PathFormat.LongFullPath);

                     // For a number of error codes (sharing violation, path not found, etc)
                     // we don't know if the problem was with the source or destination file.

                     // Check if destination directory already exists.
                     // Directory.Move()
                     // MSDN: .NET 3.5+: IOException: destDirName already exists. 
                     if (isFolder && destExists)
                        NativeError.ThrowException(Win32Errors.ERROR_ALREADY_EXISTS, destinationPathLp);

                     if (isMove)
                     {
                        // Ensure that the source file or directory exists.
                        // Directory.Move()
                        // MSDN: .NET 3.5+: DirectoryNotFoundException: The path specified by sourceDirName is invalid (for example, it is on an unmapped drive). 
                        if (!ExistsCore(isFolder, transaction, sourcePathLp, PathFormat.LongFullPath))
                           NativeError.ThrowException(isFolder ? Win32Errors.ERROR_PATH_NOT_FOUND : Win32Errors.ERROR_FILE_NOT_FOUND, sourcePathLp);
                     }


                     // Try reading the source file.
                     var fileNameLp = destinationPathLp;

                     if (!isFolder)
                     {
                        using (var safeHandle = CreateFileCore(transaction, sourcePathLp, ExtendedFileAttributes.Normal, null, FileMode.Open, 0, FileShare.Read, false, PathFormat.LongFullPath))
                           if (safeHandle != null && safeHandle.IsInvalid)
                              fileNameLp = sourcePathLp;
                     }


                     if (lastError == Win32Errors.ERROR_ACCESS_DENIED)
                     {
                        // File.Copy()
                        // File.Move()
                        // MSDN: .NET 3.5+: IOException: An I/O error has occurred.
                        //
                        //   Directory exists with the same name as the file.
                        if (!isFolder && destExists)
                           NativeError.ThrowException(lastError, string.Format(CultureInfo.InvariantCulture, Resources.Target_File_Is_A_Directory, destinationPathLp));


                        if (isMove)
                        {
                           var data = new NativeMethods.WIN32_FILE_ATTRIBUTE_DATA();
                           FillAttributeInfoCore(transaction, destinationPathLp, ref data, false, true);

                           if (data.dwFileAttributes != (FileAttributes)(-1))
                           {
                              if ((data.dwFileAttributes & FileAttributes.ReadOnly) != 0)
                              {
                                 // MSDN: .NET 3.5+: IOException: The directory specified by path is read-only.
                                 if (HasReplaceExisting(moveOptions))
                                 {
                                    // Reset file system object attributes to Normal.
                                    SetAttributesCore(isFolder, transaction, destinationPathLp, FileAttributes.Normal, PathFormat.LongFullPath);

                                    goto startCopyMove;
                                 }


                                 // MSDN: .NET 3.5+: UnauthorizedAccessException: destinationPath is read-only.
                                 // MSDN: Win32 CopyFileXxx: This function fails with ERROR_ACCESS_DENIED if the destination file already exists
                                 // and has the FILE_ATTRIBUTE_HIDDEN or FILE_ATTRIBUTE_READONLY attribute set.
                                 throw new FileReadOnlyException(destinationPathLp);
                              }


                              // MSDN: Win32 CopyFileXxx: This function fails with ERROR_ACCESS_DENIED if the destination file already exists
                              // and has the FILE_ATTRIBUTE_HIDDEN or FILE_ATTRIBUTE_READONLY attribute set.
                              if ((data.dwFileAttributes & FileAttributes.Hidden) != 0)
                                 NativeError.ThrowException(lastError, string.Format(CultureInfo.InvariantCulture, Resources.File_Is_Hidden, destinationPathLp));
                           }
                        }
                     }


                     // MSDN: .NET 3.5+: An I/O error has occurred. 
                     // File.Copy(): IOException: destinationPath exists and overwrite is false.
                     // File.Move(): The destination file already exists or sourcePath was not found.
                     NativeError.ThrowException(lastError, fileNameLp);

                     break;
               }
            }
         }


         if (success)
         {
            //// Reset file system object attributes to ReadOnly.
            //if (HasReplaceExisting(moveOptions))
            //   SetAttributesCore(isFolder, transaction, destinationPathLp, FileAttributes.ReadOnly, PathFormat.LongFullPath);


            if (!isFolder)
            {
               if (isSingleFileCopyMoveAction)
                  cmr.TotalBytes = GetSizeCore(transaction, null, destinationPathLp, PathFormat.LongFullPath);


               if (preserveDates)
                  CopyTimestampsCore(false, transaction, sourcePathLp, destinationPathLp, false, PathFormat.LongFullPath);


               cmr.TotalFiles++;

               cmr.ActionFinish = DateTime.Now;
            }
         }


         return cmr;
      }


      /// <summary>Determine the Copy or Move action.</summary>
      internal static bool IsCopyAction(CopyOptions? copyOptions, MoveOptions? moveOptions)
      {
         // Determine Copy or Move action.
         var isMove = null != moveOptions && null == copyOptions;
         var isCopy = !isMove && null != copyOptions;

         if (isCopy == isMove)
            throw new NotSupportedException(Resources.Cannot_Determine_Copy_Or_Move);

         return isCopy;
      }


      internal static bool HasCopyAllowed(MoveOptions? moveOptions)
      {
         return (moveOptions & MoveOptions.CopyAllowed) != 0;
      }


      internal static bool HasDelayUntilReboot(MoveOptions? moveOptions)
      {
         return (moveOptions & MoveOptions.DelayUntilReboot) != 0;
      }


      internal static bool HasReplaceExisting(MoveOptions? moveOptions)
      {
         return (moveOptions & MoveOptions.ReplaceExisting) != 0;
      }


      internal static bool VerifyDelayUntilReboot(string sourcePath, MoveOptions? moveOptions, PathFormat pathFormat)
      {
         var delayUntilReboot = HasDelayUntilReboot(moveOptions);

         if (delayUntilReboot)
         {
            if (HasCopyAllowed(moveOptions))
               throw new ArgumentException("This value cannot be used with " + MoveOptions.CopyAllowed, "moveOptions");


            // MoveFileXxx: (lpExistingFileName) If dwFlags specifies MOVEFILE_DELAY_UNTIL_REBOOT,
            // the file cannot exist on a remote share, because delayed operations are performed before the network is available.

            if (Path.IsUncPathCore(sourcePath, pathFormat != PathFormat.LongFullPath, false))
               throw new ArgumentException("UNC path is not allowed when using the " + MoveOptions.CopyAllowed + " flag.", "moveOptions");
         }

         return delayUntilReboot;
      }

      #endregion // Internal Methods
   }
}
