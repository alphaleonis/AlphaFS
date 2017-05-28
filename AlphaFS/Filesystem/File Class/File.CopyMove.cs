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

      /// <summary>Copies an existing file to a new file. Overwriting a file of the same name is not allowed.</summary>
      /// <remarks>
      ///   <para>The attributes of the original file are retained in the copied file.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="sourceFileName">The file to copy.</param>
      /// <param name="destinationFileName">The name of the destination file. This cannot be a directory or an existing file.</param>
      [SecurityCritical]
      public static void Copy(string sourceFileName, string destinationFileName)
      {
         CopyMoveCore(false, null, sourceFileName, destinationFileName, false, CopyOptions.FailIfExists, null, null, null, PathFormat.RelativePath);
      }

      /// <summary>Copies an existing file to a new file. Overwriting a file of the same name is allowed.</summary>
      /// <remarks>
      ///   <para>The attributes of the original file are retained in the copied file.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="sourceFileName">The file to copy. </param>
      /// <param name="destinationFileName">The name of the destination file. This cannot be a directory.</param>
      /// <param name="overwrite"><see langword="true"/> if the destination file should ignoring the read-only and hidden attributes and overwrite; otherwise, <see langword="false"/>.</param>      
      [SecurityCritical]
      public static void Copy(string sourceFileName, string destinationFileName, bool overwrite)
      {
         CopyMoveCore(false, null, sourceFileName, destinationFileName, false, overwrite ? CopyOptions.None : CopyOptions.FailIfExists, null, null, null, PathFormat.RelativePath);
      }

      #endregion // .NET

      #region AlphaFS

      #region Non-Transactional

      /// <summary>[AlphaFS] Copies an existing file to a new file. Overwriting a file of the same name is not allowed.</summary>
      /// <remarks>
      ///   <para>The attributes of the original file are retained in the copied file.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="sourceFileName">The file to copy. </param>
      /// <param name="destinationFileName">The name of the destination file. This cannot be a directory.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void Copy(string sourceFileName, string destinationFileName, PathFormat pathFormat)
      {
         CopyMoveCore(false, null, sourceFileName, destinationFileName, false, CopyOptions.FailIfExists, null, null, null, pathFormat);
      }

      /// <summary>[AlphaFS] Copies an existing file to a new file. Overwriting a file of the same name is allowed.</summary>
      /// <remarks>
      ///   <para>The attributes of the original file are retained in the copied file.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="sourceFileName">The file to copy. </param>
      /// <param name="destinationFileName">The name of the destination file. This cannot be a directory.</param>
      /// <param name="overwrite"><see langword="true"/> if the destination file should ignoring the read-only and hidden attributes and overwrite; otherwise, <see langword="false"/>.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void Copy(string sourceFileName, string destinationFileName, bool overwrite, PathFormat pathFormat)
      {
         CopyMoveCore(false, null, sourceFileName, destinationFileName, false, overwrite ? CopyOptions.None : CopyOptions.FailIfExists, null, null, null, pathFormat);
      }

      #endregion // Non-Transactional

      #region Transactional

      /// <summary>[AlphaFS] Copies an existing file to a new file. Overwriting a file of the same name is not allowed.</summary>
      /// <remarks>
      ///   <para>The attributes of the original file are retained in the copied file.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourceFileName">The file to copy.</param>
      /// <param name="destinationFileName">The name of the destination file. This cannot be a directory or an existing file.</param>
      [SecurityCritical]
      public static void CopyTransacted(KernelTransaction transaction, string sourceFileName, string destinationFileName)
      {
         CopyMoveCore(false, transaction, sourceFileName, destinationFileName, false, CopyOptions.FailIfExists, null, null, null, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Copies an existing file to a new file. Overwriting a file of the same name is not allowed.</summary>
      /// <remarks>
      ///   <para>The attributes of the original file are retained in the copied file.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourceFileName">The file to copy. </param>
      /// <param name="destinationFileName">The name of the destination file. This cannot be a directory.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void CopyTransacted(KernelTransaction transaction, string sourceFileName, string destinationFileName, PathFormat pathFormat)
      {
         CopyMoveCore(false, transaction, sourceFileName, destinationFileName, false, CopyOptions.FailIfExists, null, null, null, pathFormat);
      }



      /// <summary>[AlphaFS] Copies an existing file to a new file. Overwriting a file of the same name is allowed.</summary>
      /// <remarks>
      ///   <para>The attributes of the original file are retained in the copied file.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourceFileName">The file to copy. </param>
      /// <param name="destinationFileName">The name of the destination file. This cannot be a directory.</param>
      /// <param name="overwrite"><see langword="true"/> if the destination file should ignoring the read-only and hidden attributes and overwrite; otherwise, <see langword="false"/>.</param>
      [SecurityCritical]
      public static void CopyTransacted(KernelTransaction transaction, string sourceFileName, string destinationFileName, bool overwrite)
      {
         CopyMoveCore(false, transaction, sourceFileName, destinationFileName, false, overwrite ? CopyOptions.None : CopyOptions.FailIfExists, null, null, null, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Copies an existing file to a new file. Overwriting a file of the same name is allowed.</summary>
      /// <remarks>
      ///   <para>The attributes of the original file are retained in the copied file.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourceFileName">The file to copy. </param>
      /// <param name="destinationFileName">The name of the destination file. This cannot be a directory.</param>
      /// <param name="overwrite"><see langword="true"/> if the destination file should ignoring the read-only and hidden attributes and overwrite; otherwise, <see langword="false"/>.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void CopyTransacted(KernelTransaction transaction, string sourceFileName, string destinationFileName, bool overwrite, PathFormat pathFormat)
      {
         CopyMoveCore(false, transaction, sourceFileName, destinationFileName, false, overwrite ? CopyOptions.None : CopyOptions.FailIfExists, null, null, null, pathFormat);
      }

      #endregion // Transactional

      #endregion // AlphaFS

      #endregion // Copy


      #region Copy (CopyOptions)

      #region Non-Transactional

      /// <summary>[AlphaFS] Copies an existing file to a new file. Overwriting a file of the same name is allowed. <see cref="CopyOptions"/> can be specified.</summary>
      /// <remarks>
      ///   <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      ///   <para>The attributes of the original file are retained in the copied file.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="sourceFileName">The file to copy.</param>
      /// <param name="destinationFileName">The name of the destination file. This cannot be a directory.</param>
      /// <param name="copyOptions"><see cref="CopyOptions"/> that specify how the file is to be copied. This parameter can be <see langword="null"/>.</param>
      [SecurityCritical]
      public static void Copy(string sourceFileName, string destinationFileName, CopyOptions copyOptions)
      {
         CopyMoveCore(false, null, sourceFileName, destinationFileName, false, copyOptions, null, null, null, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Copies an existing file to a new file. Overwriting a file of the same name is allowed. <see cref="CopyOptions"/> can be specified.</summary>
      /// <remarks>
      ///   <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      ///   <para>The attributes of the original file are retained in the copied file.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="sourceFileName">The file to copy.</param>
      /// <param name="destinationFileName">The name of the destination file. This cannot be a directory.</param>
      /// <param name="copyOptions"><see cref="CopyOptions"/> that specify how the file is to be copied. This parameter can be <see langword="null"/>.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void Copy(string sourceFileName, string destinationFileName, CopyOptions copyOptions, PathFormat pathFormat)
      {
         CopyMoveCore(false, null, sourceFileName, destinationFileName, false, copyOptions, null, null, null, pathFormat);
      }



      /// <summary>[AlphaFS] Copies an existing file to a new file. Overwriting a file of the same name is allowed. <see cref="CopyOptions"/> can be specified.</summary>
      /// <remarks>
      ///   <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      ///   <para>The attributes of the original file are retained in the copied file.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="sourceFileName">The file to copy.</param>
      /// <param name="destinationFileName">The name of the destination file. This cannot be a directory.</param>
      /// <param name="copyOptions"><see cref="CopyOptions"/> that specify how the file is to be copied. This parameter can be <see langword="null"/>.</param>
      /// <param name="preserveDates"><see langword="true"/> if original Timestamps must be preserved, <see langword="false"/> otherwise.</param>
      [SecurityCritical]
      public static void Copy(string sourceFileName, string destinationFileName, CopyOptions copyOptions, bool preserveDates)
      {
         CopyMoveCore(false, null, sourceFileName, destinationFileName, preserveDates, copyOptions, null, null, null, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Copies an existing file to a new file. Overwriting a file of the same name is allowed. <see cref="CopyOptions"/> can be specified.</summary>
      /// <remarks>
      ///   <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      ///   <para>The attributes of the original file are retained in the copied file.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="sourceFileName">The file to copy.</param>
      /// <param name="destinationFileName">The name of the destination file. This cannot be a directory.</param>
      /// <param name="copyOptions"><see cref="CopyOptions"/> that specify how the file is to be copied. This parameter can be <see langword="null"/>.</param>
      /// <param name="preserveDates"><see langword="true"/> if original Timestamps must be preserved, <see langword="false"/> otherwise.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void Copy(string sourceFileName, string destinationFileName, CopyOptions copyOptions, bool preserveDates, PathFormat pathFormat)
      {
         CopyMoveCore(false, null, sourceFileName, destinationFileName, preserveDates, copyOptions, null, null, null, pathFormat);
      }



      /// <summary>[AlphaFS] Copies an existing file to a new file. Overwriting a file of the same name is allowed.  <see cref="CopyOptions"/> can be specified,
      ///   <para>and the possibility of notifying the application of its progress through a callback function.</para>
      /// </summary>
      /// <returns>A <see cref="CopyMoveResult"/> class with the status of the Copy action.</returns>
      /// <remarks>
      ///   <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      ///   <para>The attributes of the original file are retained in the copied file.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="sourceFileName">The file to copy.</param>
      /// <param name="destinationFileName">The name of the destination file. This cannot be a directory.</param>
      /// <param name="copyOptions"><see cref="CopyOptions"/> that specify how the file is to be copied. This parameter can be <see langword="null"/>.</param>
      /// <param name="progressHandler">A callback function that is called each time another portion of the file has been copied. This parameter can be <see langword="null"/>.</param>
      /// <param name="userProgressData">The argument to be passed to the callback function. This parameter can be <see langword="null"/>.</param>
      [SecurityCritical]
      public static CopyMoveResult Copy(string sourceFileName, string destinationFileName, CopyOptions copyOptions, CopyMoveProgressRoutine progressHandler, object userProgressData)
      {
         return CopyMoveCore(false, null, sourceFileName, destinationFileName, false, copyOptions, null, progressHandler, userProgressData, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Copies an existing file to a new file. Overwriting a file of the same name is allowed.  <see cref="CopyOptions"/> can be specified,
      ///   <para>and the possibility of notifying the application of its progress through a callback function.</para>
      /// </summary>
      /// <returns>A <see cref="CopyMoveResult"/> class with the status of the Copy action.</returns>
      /// <remarks>
      ///   <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      ///   <para>The attributes of the original file are retained in the copied file.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="sourceFileName">The file to copy.</param>
      /// <param name="destinationFileName">The name of the destination file. This cannot be a directory.</param>
      /// <param name="copyOptions"><see cref="CopyOptions"/> that specify how the file is to be copied. This parameter can be <see langword="null"/>.</param>
      /// <param name="progressHandler">A callback function that is called each time another portion of the file has been copied. This parameter can be <see langword="null"/>.</param>
      /// <param name="userProgressData">The argument to be passed to the callback function. This parameter can be <see langword="null"/>.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static CopyMoveResult Copy(string sourceFileName, string destinationFileName, CopyOptions copyOptions, CopyMoveProgressRoutine progressHandler, object userProgressData, PathFormat pathFormat)
      {
         return CopyMoveCore(false, null, sourceFileName, destinationFileName, false, copyOptions, null, progressHandler, userProgressData, pathFormat);
      }

      

      /// <summary>[AlphaFS] Copies an existing file to a new file. Overwriting a file of the same name is allowed.  <see cref="CopyOptions"/> can be specified,
      ///   <para>and the possibility of notifying the application of its progress through a callback function.</para>
      /// </summary>
      /// <returns>A <see cref="CopyMoveResult"/> class with the status of the Copy action.</returns>
      /// <remarks>
      ///   <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      ///   <para>The attributes of the original file are retained in the copied file.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="sourceFileName">The file to copy.</param>
      /// <param name="destinationFileName">The name of the destination file. This cannot be a directory.</param>
      /// <param name="copyOptions"><see cref="CopyOptions"/> that specify how the file is to be copied. This parameter can be <see langword="null"/>.</param>
      /// <param name="preserveDates"><see langword="true"/> if original Timestamps must be preserved, <see langword="false"/> otherwise.</param>
      /// <param name="progressHandler">A callback function that is called each time another portion of the file has been copied. This parameter can be <see langword="null"/>.</param>
      /// <param name="userProgressData">The argument to be passed to the callback function. This parameter can be <see langword="null"/>.</param>
      [SecurityCritical]
      public static CopyMoveResult Copy(string sourceFileName, string destinationFileName, CopyOptions copyOptions, bool preserveDates, CopyMoveProgressRoutine progressHandler, object userProgressData)
      {
         return CopyMoveCore(false, null, sourceFileName, destinationFileName, preserveDates, copyOptions, null, progressHandler, userProgressData, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Copies an existing file to a new file. Overwriting a file of the same name is allowed.  <see cref="CopyOptions"/> can be specified,
      ///   <para>and the possibility of notifying the application of its progress through a callback function.</para>
      /// </summary>
      /// <returns>A <see cref="CopyMoveResult"/> class with the status of the Copy action.</returns>
      /// <remarks>
      ///   <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      ///   <para>The attributes of the original file are retained in the copied file.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="sourceFileName">The file to copy.</param>
      /// <param name="destinationFileName">The name of the destination file. This cannot be a directory.</param>
      /// <param name="copyOptions"><see cref="CopyOptions"/> that specify how the file is to be copied. This parameter can be <see langword="null"/>.</param>
      /// <param name="preserveDates"><see langword="true"/> if original Timestamps must be preserved, <see langword="false"/> otherwise.</param>
      /// <param name="progressHandler">A callback function that is called each time another portion of the file has been copied. This parameter can be <see langword="null"/>.</param>
      /// <param name="userProgressData">The argument to be passed to the callback function. This parameter can be <see langword="null"/>.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static CopyMoveResult Copy(string sourceFileName, string destinationFileName, CopyOptions copyOptions, bool preserveDates, CopyMoveProgressRoutine progressHandler, object userProgressData, PathFormat pathFormat)
      {
         return CopyMoveCore(false, null, sourceFileName, destinationFileName, preserveDates, copyOptions, null, progressHandler, userProgressData, pathFormat);
      }

      #endregion // Non-Transactional

      #region Transactional

      /// <summary>[AlphaFS] Copies an existing file to a new file. Overwriting a file of the same name is allowed. <see cref="CopyOptions"/> can be specified.</summary>
      /// <remarks>
      ///   <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      ///   <para>The attributes of the original file are retained in the copied file.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourceFileName">The file to copy.</param>
      /// <param name="destinationFileName">The name of the destination file. This cannot be a directory.</param>
      /// <param name="copyOptions"><see cref="CopyOptions"/> that specify how the file is to be copied. This parameter can be <see langword="null"/>.</param>
      [SecurityCritical]
      public static void CopyTransacted(KernelTransaction transaction, string sourceFileName, string destinationFileName, CopyOptions copyOptions)
      {
         CopyMoveCore(false, transaction, sourceFileName, destinationFileName, false, copyOptions, null, null, null, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Copies an existing file to a new file. Overwriting a file of the same name is allowed. <see cref="CopyOptions"/> can be specified.</summary>
      /// <remarks>
      ///   <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      ///   <para>The attributes of the original file are retained in the copied file.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourceFileName">The file to copy.</param>
      /// <param name="destinationFileName">The name of the destination file. This cannot be a directory.</param>
      /// <param name="copyOptions"><see cref="CopyOptions"/> that specify how the file is to be copied. This parameter can be <see langword="null"/>.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void CopyTransacted(KernelTransaction transaction, string sourceFileName, string destinationFileName, CopyOptions copyOptions, PathFormat pathFormat)
      {
         CopyMoveCore(false, transaction, sourceFileName, destinationFileName, false, copyOptions, null, null, null, pathFormat);
      }



      /// <summary>[AlphaFS] Copies an existing file to a new file. Overwriting a file of the same name is allowed. <see cref="CopyOptions"/> can be specified.</summary>
      /// <remarks>
      ///   <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      ///   <para>The attributes of the original file are retained in the copied file.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourceFileName">The file to copy.</param>
      /// <param name="destinationFileName">The name of the destination file. This cannot be a directory.</param>
      /// <param name="copyOptions"><see cref="CopyOptions"/> that specify how the file is to be copied. This parameter can be <see langword="null"/>.</param>
      /// <param name="preserveDates"><see langword="true"/> if original Timestamps must be preserved, <see langword="false"/> otherwise.</param>
      [SecurityCritical]
      public static void CopyTransacted(KernelTransaction transaction, string sourceFileName, string destinationFileName, CopyOptions copyOptions, bool preserveDates)
      {
         CopyMoveCore(false, transaction, sourceFileName, destinationFileName, preserveDates, copyOptions, null, null, null, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Copies an existing file to a new file. Overwriting a file of the same name is allowed. <see cref="CopyOptions"/> can be specified.</summary>
      /// <remarks>
      ///   <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      ///   <para>The attributes of the original file are retained in the copied file.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourceFileName">The file to copy.</param>
      /// <param name="destinationFileName">The name of the destination file. This cannot be a directory.</param>
      /// <param name="copyOptions"><see cref="CopyOptions"/> that specify how the file is to be copied. This parameter can be <see langword="null"/>.</param>
      /// <param name="preserveDates"><see langword="true"/> if original Timestamps must be preserved, <see langword="false"/> otherwise.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void CopyTransacted(KernelTransaction transaction, string sourceFileName, string destinationFileName, CopyOptions copyOptions, bool preserveDates, PathFormat pathFormat)
      {
         CopyMoveCore(false, transaction, sourceFileName, destinationFileName, preserveDates, copyOptions, null, null, null, pathFormat);
      }



      /// <summary>[AlphaFS] Copies an existing file to a new file. Overwriting a file of the same name is allowed.  <see cref="CopyOptions"/> can be specified,
      ///   <para>and the possibility of notifying the application of its progress through a callback function.</para>
      /// </summary>
      /// <returns>A <see cref="CopyMoveResult"/> class with the status of the Copy action.</returns>
      /// <remarks>
      ///   <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      ///   <para>The attributes of the original file are retained in the copied file.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourceFileName">The file to copy.</param>
      /// <param name="destinationFileName">The name of the destination file. This cannot be a directory.</param>
      /// <param name="copyOptions"><see cref="CopyOptions"/> that specify how the file is to be copied. This parameter can be <see langword="null"/>.</param>
      /// <param name="progressHandler">A callback function that is called each time another portion of the file has been copied. This parameter can be <see langword="null"/>.</param>
      /// <param name="userProgressData">The argument to be passed to the callback function. This parameter can be <see langword="null"/>.</param>
      [SecurityCritical]
      public static CopyMoveResult CopyTransacted(KernelTransaction transaction, string sourceFileName, string destinationFileName, CopyOptions copyOptions, CopyMoveProgressRoutine progressHandler, object userProgressData)
      {
         return CopyMoveCore(false, transaction, sourceFileName, destinationFileName, false, copyOptions, null, progressHandler, userProgressData, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Copies an existing file to a new file. Overwriting a file of the same name is allowed.  <see cref="CopyOptions"/> can be specified,
      ///   <para>and the possibility of notifying the application of its progress through a callback function.</para>
      /// </summary>
      /// <returns>A <see cref="CopyMoveResult"/> class with the status of the Copy action.</returns>
      /// <remarks>
      ///   <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      ///   <para>The attributes of the original file are retained in the copied file.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourceFileName">The file to copy.</param>
      /// <param name="destinationFileName">The name of the destination file. This cannot be a directory.</param>
      /// <param name="copyOptions"><see cref="CopyOptions"/> that specify how the file is to be copied. This parameter can be <see langword="null"/>.</param>
      /// <param name="progressHandler">A callback function that is called each time another portion of the file has been copied. This parameter can be <see langword="null"/>.</param>
      /// <param name="userProgressData">The argument to be passed to the callback function. This parameter can be <see langword="null"/>.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static CopyMoveResult CopyTransacted(KernelTransaction transaction, string sourceFileName, string destinationFileName, CopyOptions copyOptions, CopyMoveProgressRoutine progressHandler, object userProgressData, PathFormat pathFormat)
      {
         return CopyMoveCore(false, transaction, sourceFileName, destinationFileName, false, copyOptions, null, progressHandler, userProgressData, pathFormat);
      }


      
      /// <summary>[AlphaFS] Copies an existing file to a new file. Overwriting a file of the same name is allowed.  <see cref="CopyOptions"/> can be specified,
      ///   <para>and the possibility of notifying the application of its progress through a callback function.</para>
      /// </summary>
      /// <returns>A <see cref="CopyMoveResult"/> class with the status of the Copy action.</returns>
      /// <remarks>
      ///   <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      ///   <para>The attributes of the original file are retained in the copied file.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourceFileName">The file to copy.</param>
      /// <param name="destinationFileName">The name of the destination file. This cannot be a directory.</param>
      /// <param name="copyOptions"><see cref="CopyOptions"/> that specify how the file is to be copied. This parameter can be <see langword="null"/>.</param>
      /// <param name="preserveDates"><see langword="true"/> if original Timestamps must be preserved, <see langword="false"/> otherwise.</param>
      /// <param name="progressHandler">A callback function that is called each time another portion of the file has been copied. This parameter can be <see langword="null"/>.</param>
      /// <param name="userProgressData">The argument to be passed to the callback function. This parameter can be <see langword="null"/>.</param>
      [SecurityCritical]
      public static CopyMoveResult CopyTransacted(KernelTransaction transaction, string sourceFileName, string destinationFileName, CopyOptions copyOptions, bool preserveDates, CopyMoveProgressRoutine progressHandler, object userProgressData)
      {
         return CopyMoveCore(false, transaction, sourceFileName, destinationFileName, preserveDates, copyOptions, null, progressHandler, userProgressData, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Copies an existing file to a new file. Overwriting a file of the same name is allowed.  <see cref="CopyOptions"/> can be specified,
      ///   <para>and the possibility of notifying the application of its progress through a callback function.</para>
      /// </summary>
      /// <returns>A <see cref="CopyMoveResult"/> class with the status of the Copy action.</returns>
      /// <remarks>
      ///   <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      ///   <para>The attributes of the original file are retained in the copied file.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourceFileName">The file to copy.</param>
      /// <param name="destinationFileName">The name of the destination file. This cannot be a directory.</param>
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
      public static CopyMoveResult CopyTransacted(KernelTransaction transaction, string sourceFileName, string destinationFileName, CopyOptions copyOptions, bool preserveDates, CopyMoveProgressRoutine progressHandler, object userProgressData, PathFormat pathFormat)
      {
         return CopyMoveCore(false, transaction, sourceFileName, destinationFileName, preserveDates, copyOptions, null, progressHandler, userProgressData, pathFormat);
      }

      #endregion // Transactional

      #endregion // Copy (CopyOptions)


      #region Move

      #region .NET

      /// <summary>Moves a specified file to a new location, providing the option to specify a new file name.</summary>
      /// <remarks>
      ///   <para>This method works across disk volumes, and it does not throw an exception if the source and destination are the same.</para>
      ///   <para>Note that if you attempt to replace a file by moving a file of the same name into that directory, you get an <see cref="IOException"/>.</para>
      ///   <para>You cannot use the Move method to overwrite an existing file.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="sourceFileName">The name of the file to move.</param>
      /// <param name="destinationFileName">The new path for the file.</param>
      [SecurityCritical]
      public static void Move(string sourceFileName, string destinationFileName)
      {
         CopyMoveCore(false, null, sourceFileName, destinationFileName, false, null, MoveOptions.CopyAllowed, null, null, PathFormat.RelativePath);
      }

      #endregion // .NET


      #region AlphaFS

      #region Non-Transactional

      /// <summary>[AlphaFS] Moves a specified file to a new location, providing the option to specify a new file name.</summary>
      /// <remarks>
      ///   <para>This method works across disk volumes, and it does not throw an exception if the source and destination are the same.</para>
      ///   <para>Note that if you attempt to replace a file by moving a file of the same name into that directory, you get an <see cref="IOException"/>.</para>
      ///   <para>You cannot use the Move method to overwrite an existing file.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="sourceFileName">The name of the file to move.</param>
      /// <param name="destinationFileName">The new path for the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void Move(string sourceFileName, string destinationFileName, PathFormat pathFormat)
      {
         CopyMoveCore(false, null, sourceFileName, destinationFileName, false, null, MoveOptions.CopyAllowed, null, null, pathFormat);
      }

      #endregion // Non-Transactional

      #region Transactional

      /// <summary>[AlphaFS] Moves a specified file to a new location, providing the option to specify a new file name.</summary>
      /// <remarks>
      ///   <para>This method works across disk volumes, and it does not throw an exception if the source and destination are the same.</para>
      ///   <para>Note that if you attempt to replace a file by moving a file of the same name into that directory, you get an <see cref="IOException"/>.</para>
      ///   <para>You cannot use the Move method to overwrite an existing file.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourceFileName">The name of the file to move.</param>
      /// <param name="destinationFileName">The new path for the file.</param>
      [SecurityCritical]
      public static void MoveTransacted(KernelTransaction transaction, string sourceFileName, string destinationFileName)
      {
         CopyMoveCore(false, transaction, sourceFileName, destinationFileName, false, null, MoveOptions.CopyAllowed, null, null, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Moves a specified file to a new location, providing the option to specify a new file name.</summary>
      /// <remarks>
      ///   <para>This method works across disk volumes, and it does not throw an exception if the source and destination are the same.</para>
      ///   <para>Note that if you attempt to replace a file by moving a file of the same name into that directory, you get an <see cref="IOException"/>.</para>
      ///   <para>You cannot use the Move method to overwrite an existing file.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourceFileName">The name of the file to move.</param>
      /// <param name="destinationFileName">The new path for the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void MoveTransacted(KernelTransaction transaction, string sourceFileName, string destinationFileName, PathFormat pathFormat)
      {
         CopyMoveCore(false, transaction, sourceFileName, destinationFileName, false, null, MoveOptions.CopyAllowed, null, null, pathFormat);
      }
      
      #endregion // Transactional

      #endregion // AlphaFS

      #endregion // Move


      #region Move (MoveOptions)

      #region Non-Transactional

      /// <summary>[AlphaFS] Moves a specified file to a new location, providing the option to specify a new file name.</summary>
      /// <remarks>
      ///   <para>This method works across disk volumes, and it does not throw an exception if the source and destination are the same.</para>
      ///   <para>Note that if you attempt to replace a file by moving a file of the same name into that directory, you get an <see cref="IOException"/>.</para>
      ///   <para>You cannot use the Move method to overwrite an existing file.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="sourceFileName">The name of the file to move.</param>
      /// <param name="destinationFileName">The new path for the file.</param>
      /// <param name="moveOptions"><see cref="MoveOptions"/> that specify how the file is to be moved. This parameter can be <see langword="null"/>.</param>
      [SecurityCritical]
      public static void Move(string sourceFileName, string destinationFileName, MoveOptions moveOptions)
      {
         CopyMoveCore(false, null, sourceFileName, destinationFileName, false, null, moveOptions, null, null, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Moves a specified file to a new location, providing the option to specify a new file name.</summary>
      /// <remarks>
      ///   <para>This method works across disk volumes, and it does not throw an exception if the source and destination are the same.</para>
      ///   <para>Note that if you attempt to replace a file by moving a file of the same name into that directory, you get an <see cref="IOException"/>.</para>
      ///   <para>You cannot use the Move method to overwrite an existing file.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="sourceFileName">The name of the file to move.</param>
      /// <param name="destinationFileName">The new path for the file.</param>
      /// <param name="moveOptions"><see cref="MoveOptions"/> that specify how the file is to be moved. This parameter can be <see langword="null"/>.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void Move(string sourceFileName, string destinationFileName, MoveOptions moveOptions, PathFormat pathFormat)
      {
         CopyMoveCore(false, null, sourceFileName, destinationFileName, false, null, moveOptions, null, null, pathFormat);
      }



      /// <summary>[AlphaFS] Moves a specified file to a new location, providing the option to specify a new file name.</summary>
      /// <returns>A <see cref="CopyMoveResult"/> class with the status of the Move action.</returns>
      /// <remarks>
      ///   <para>This method works across disk volumes, and it does not throw an exception if the source and destination are the same.</para>
      ///   <para>Note that if you attempt to replace a file by moving a file of the same name into that directory, you get an <see cref="IOException"/>.</para>
      ///   <para>You cannot use the Move method to overwrite an existing file.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="sourceFileName">The name of the file to move.</param>
      /// <param name="destinationFileName">The new path for the file.</param>
      /// <param name="moveOptions"><see cref="MoveOptions"/> that specify how the file is to be moved. This parameter can be <see langword="null"/>.</param>
      /// <param name="progressHandler">A callback function that is called each time another portion of the file has been moved. This parameter can be <see langword="null"/>.</param>
      /// <param name="userProgressData">The argument to be passed to the callback function. This parameter can be <see langword="null"/>.</param>
      [SecurityCritical]
      public static CopyMoveResult Move(string sourceFileName, string destinationFileName, MoveOptions moveOptions, CopyMoveProgressRoutine progressHandler, object userProgressData)
      {
         return CopyMoveCore(false, null, sourceFileName, destinationFileName, false, null, moveOptions, progressHandler, userProgressData, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Moves a specified file to a new location, providing the option to specify a new file name.</summary>
      /// <returns>A <see cref="CopyMoveResult"/> class with the status of the Move action.</returns>
      /// <remarks>
      ///   <para>This method works across disk volumes, and it does not throw an exception if the source and destination are the same.</para>
      ///   <para>Note that if you attempt to replace a file by moving a file of the same name into that directory, you get an <see cref="IOException"/>.</para>
      ///   <para>You cannot use the Move method to overwrite an existing file.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="sourceFileName">The name of the file to move.</param>
      /// <param name="destinationFileName">The new path for the file.</param>
      /// <param name="moveOptions"><see cref="MoveOptions"/> that specify how the file is to be moved. This parameter can be <see langword="null"/>.</param>
      /// <param name="progressHandler">A callback function that is called each time another portion of the file has been moved. This parameter can be <see langword="null"/>.</param>
      /// <param name="userProgressData">The argument to be passed to the callback function. This parameter can be <see langword="null"/>.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static CopyMoveResult Move(string sourceFileName, string destinationFileName, MoveOptions moveOptions, CopyMoveProgressRoutine progressHandler, object userProgressData, PathFormat pathFormat)
      {
         return CopyMoveCore(false, null, sourceFileName, destinationFileName, false, null, moveOptions, progressHandler, userProgressData, pathFormat);
      }

      #endregion // Non-Transactional

      #region Transactional

      /// <summary>[AlphaFS] Moves a specified file to a new location, providing the option to specify a new file name.</summary>
      /// <remarks>
      ///   <para>This method works across disk volumes, and it does not throw an exception if the source and destination are the same.</para>
      ///   <para>Note that if you attempt to replace a file by moving a file of the same name into that directory, you get an <see cref="IOException"/>.</para>
      ///   <para>You cannot use the Move method to overwrite an existing file.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourceFileName">The name of the file to move.</param>
      /// <param name="destinationFileName">The new path for the file.</param>
      /// <param name="moveOptions"><see cref="MoveOptions"/> that specify how the file is to be moved. This parameter can be <see langword="null"/>.</param>
      [SecurityCritical]
      public static void MoveTransacted(KernelTransaction transaction, string sourceFileName, string destinationFileName, MoveOptions moveOptions)
      {
         CopyMoveCore(false, transaction, sourceFileName, destinationFileName, false, null, moveOptions, null, null, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Moves a specified file to a new location, providing the option to specify a new file name.</summary>
      /// <remarks>
      ///   <para>This method works across disk volumes, and it does not throw an exception if the source and destination are the same.</para>
      ///   <para>Note that if you attempt to replace a file by moving a file of the same name into that directory, you get an <see cref="IOException"/>.</para>
      ///   <para>You cannot use the Move method to overwrite an existing file.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourceFileName">The name of the file to move.</param>
      /// <param name="destinationFileName">The new path for the file.</param>
      /// <param name="moveOptions"><see cref="MoveOptions"/> that specify how the file is to be moved. This parameter can be <see langword="null"/>.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void MoveTransacted(KernelTransaction transaction, string sourceFileName, string destinationFileName, MoveOptions moveOptions, PathFormat pathFormat)
      {
         CopyMoveCore(false, transaction, sourceFileName, destinationFileName, false, null, moveOptions, null, null, pathFormat);
      }



      /// <summary>[AlphaFS] Moves a specified file to a new location, providing the option to specify a new file name.</summary>
      /// <returns>A <see cref="CopyMoveResult"/> class with the status of the Move action.</returns>
      /// <remarks>
      ///   <para>This method works across disk volumes, and it does not throw an exception if the source and destination are the same.</para>
      ///   <para>Note that if you attempt to replace a file by moving a file of the same name into that directory, you get an <see cref="IOException"/>.</para>
      ///   <para>You cannot use the Move method to overwrite an existing file.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourceFileName">The name of the file to move.</param>
      /// <param name="destinationFileName">The new path for the file.</param>
      /// <param name="moveOptions"><see cref="MoveOptions"/> that specify how the file is to be moved. This parameter can be <see langword="null"/>.</param>
      /// <param name="progressHandler">A callback function that is called each time another portion of the file has been moved. This parameter can be <see langword="null"/>.</param>
      /// <param name="userProgressData">The argument to be passed to the callback function. This parameter can be <see langword="null"/>.</param>
      [SecurityCritical]
      public static CopyMoveResult MoveTransacted(KernelTransaction transaction, string sourceFileName, string destinationFileName, MoveOptions moveOptions, CopyMoveProgressRoutine progressHandler, object userProgressData)
      {
         return CopyMoveCore(false, transaction, sourceFileName, destinationFileName, false, null, moveOptions, progressHandler, userProgressData, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Moves a specified file to a new location, providing the option to specify a new file name.</summary>
      /// <returns>A <see cref="CopyMoveResult"/> class with the status of the Move action.</returns>
      /// <remarks>
      ///   <para>This method works across disk volumes, and it does not throw an exception if the source and destination are the same.</para>
      ///   <para>Note that if you attempt to replace a file by moving a file of the same name into that directory, you get an <see cref="IOException"/>.</para>
      ///   <para>You cannot use the Move method to overwrite an existing file.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourceFileName">The name of the file to move.</param>
      /// <param name="destinationFileName">The new path for the file.</param>
      /// <param name="moveOptions"><see cref="MoveOptions"/> that specify how the file is to be moved. This parameter can be <see langword="null"/>.</param>
      /// <param name="progressHandler">A callback function that is called each time another portion of the file has been moved. This parameter can be <see langword="null"/>.</param>
      /// <param name="userProgressData">The argument to be passed to the callback function. This parameter can be <see langword="null"/>.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static CopyMoveResult MoveTransacted(KernelTransaction transaction, string sourceFileName, string destinationFileName, MoveOptions moveOptions, CopyMoveProgressRoutine progressHandler, object userProgressData, PathFormat pathFormat)
      {
         return CopyMoveCore(false, transaction, sourceFileName, destinationFileName, false, null, moveOptions, progressHandler, userProgressData, pathFormat);
      }
      
      #endregion // Transactional

      #endregion // Move (MoveOptions)
      

      #region Internal Methods

      /// <summary>Copy/move a Non-/Transacted file or directory including its children to a new location, <see cref="CopyOptions"/> or <see cref="MoveOptions"/> can be specified,
      ///   and the possibility of notifying the application of its progress through a callback function.
      /// </summary>
      /// <returns>A <see cref="CopyMoveResult"/> class with the status of the Copy or Move action.</returns>      
      /// <remarks>
      ///   <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      ///   <para>You cannot use the Move method to overwrite an existing file, unless
      ///   <paramref name="moveOptions"/> contains <see cref="MoveOptions.ReplaceExisting"/>.</para>
      ///   <para>This Move method works across disk volumes, and it does not throw an exception if the
      ///   source and destination are the same. </para>
      ///   <para>Note that if you attempt to replace a file by moving a file of the same name into
      ///   that directory, you get an IOException.</para>
      /// </remarks>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="isFolder">Specifies that <paramref name="sourceFileName"/> and <paramref name="destinationFileName"/> are a file or directory.</param>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourcePath">The source directory path plus file name.</param>
      /// <param name="destinationPath">The destination directory path plus file name.</param>
      /// <param name="preserveDates"><see langword="true"/> if original Timestamps must be preserved, <see langword="false"/> otherwise. This parameter is ignored for move operations.</param>
      /// <param name="copyOptions"><see cref="CopyOptions"/> that specify how the file is to be copied. This parameter can be <see langword="null"/>.</param>
      /// <param name="moveOptions">Flags that specify how the file or directory is to be moved. This parameter can be <see langword="null"/>.</param>
      /// <param name="progressHandler">A callback function that is called each time another portion of the file has been copied/moved. This parameter can be <see langword="null"/>.</param>
      /// <param name="userProgressData">The argument to be passed to the callback function. This parameter can be <see langword="null"/>.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
      [SecurityCritical]
      internal static CopyMoveResult CopyMoveCore(bool isFolder, KernelTransaction transaction, string sourcePath, string destinationPath, bool preserveDates, CopyOptions? copyOptions, MoveOptions? moveOptions, CopyMoveProgressRoutine progressHandler, object userProgressData, PathFormat pathFormat)
      {
         #region Setup

         // Determine Copy or Move action.
         var doCopy = copyOptions != null && null == moveOptions;
         var doMove = moveOptions != null && null == copyOptions;

         if (doCopy == doMove)
            throw new NotSupportedException(Resources.Cannot_Determine_Copy_Or_Move);


         var fullCheck = pathFormat == PathFormat.RelativePath;

         // Allow null value for destinationPath when flag MoveOptions.DelayUntilReboot is specified.
         var delayUntilReboot = null == destinationPath && doMove && ((MoveOptions) moveOptions & MoveOptions.DelayUntilReboot) != 0;


         Path.CheckSupportedPathFormat(sourcePath, fullCheck, fullCheck);

         if (!delayUntilReboot)
            Path.CheckSupportedPathFormat(destinationPath, fullCheck, fullCheck);


         var sourcePathLp = Path.GetExtendedLengthPathCore(transaction, sourcePath, pathFormat, GetFullPathOptions.RemoveTrailingDirectorySeparator);
         var destinationPathLp = delayUntilReboot ? null : Path.GetExtendedLengthPathCore(transaction, destinationPath, pathFormat, GetFullPathOptions.RemoveTrailingDirectorySeparator);


         // MSDN: .NET3.5+: IOException: The sourceDirName and destDirName parameters refer to the same file or directory.
         // Do not use StringComparison.OrdinalIgnoreCase to allow renaming a folder with different casing.
         if (!delayUntilReboot && sourcePathLp.Equals(destinationPathLp))
            NativeError.ThrowException(Win32Errors.ERROR_SAME_DRIVE, destinationPathLp);


         // MSDN: If this flag is set to TRUE during the copy/move operation, the operation is canceled.
         // Otherwise, the copy/move operation will continue to completion.
         var cancel = false;


         var raiseException = null == progressHandler;

         // Setup callback function for progress notifications.
         var routine = raiseException
            ? (NativeMethods.NativeCopyMoveProgressRoutine) null
            : (totalFileSize, totalBytesTransferred, streamSize, streamBytesTransferred, dwStreamNumber, dwCallbackReason, hSourceFile, hDestinationFile, lpData) =>
               progressHandler(totalFileSize, totalBytesTransferred, streamSize, streamBytesTransferred, dwStreamNumber, dwCallbackReason, userProgressData);

         #endregion //Setup


      startCopyMove:

         var lastError = Win32Errors.ERROR_SUCCESS;


         #region Win32 Copy/Move

         if (!(transaction == null || !NativeMethods.IsAtLeastWindowsVista
            ? doMove
               // MoveFileWithProgress() / MoveFileTransacted()
               // In the ANSI version of this function, the name is limited to MAX_PATH characters.
               // To extend this limit to 32,767 wide characters, call the Unicode version of the function and prepend "\\?\" to the path.
               // 2013-04-15: MSDN confirms LongPath usage.

               // CopyFileEx() / CopyFileTransacted()
               // In the ANSI version of this function, the name is limited to MAX_PATH characters.
               // To extend this limit to 32,767 wide characters, call the Unicode version of the function and prepend "\\?\" to the path.
               // 2013-04-15: MSDN confirms LongPath usage.

               ? NativeMethods.MoveFileWithProgress(sourcePathLp, destinationPathLp, routine, IntPtr.Zero, (MoveOptions) moveOptions)
               : NativeMethods.CopyFileEx(sourcePathLp, destinationPathLp, routine, IntPtr.Zero, out cancel, (CopyOptions) copyOptions)

            : doMove
               ? NativeMethods.MoveFileTransacted(sourcePathLp, destinationPathLp, routine, IntPtr.Zero, (MoveOptions) moveOptions, transaction.SafeHandle)
               : NativeMethods.CopyFileTransacted(sourcePathLp, destinationPathLp, routine, IntPtr.Zero, out cancel, (CopyOptions) copyOptions, transaction.SafeHandle)))
         {
            lastError = (uint) Marshal.GetLastWin32Error();

            if (lastError == Win32Errors.ERROR_REQUEST_ABORTED)
            {
               // If lpProgressRoutine returns PROGRESS_CANCEL due to the user canceling the operation,
               // CopyFileEx will return zero and GetLastError will return ERROR_REQUEST_ABORTED.
               // In this case, the partially copied destination file is deleted.
               //
               // If lpProgressRoutine returns PROGRESS_STOP due to the user stopping the operation,
               // CopyFileEx will return zero and GetLastError will return ERROR_REQUEST_ABORTED.
               // In this case, the partially copied destination file is left intact.

               cancel = true;
            }

            else if (raiseException)
            {
               raiseException:

               #region Win32Errors

               switch (lastError)
               {
                  case Win32Errors.ERROR_FILE_NOT_FOUND:
                     if (isFolder)
                     {
                        lastError = Win32Errors.ERROR_PATH_NOT_FOUND;
                        goto raiseException;
                     }

                     // File.Copy()
                     // File.Move()
                     // MSDN: .NET 3.5+: FileNotFoundException: sourceFileName was not found. 
                     NativeError.ThrowException(lastError, sourcePathLp);
                     break;

                  case Win32Errors.ERROR_PATH_NOT_FOUND:
                     // File.Copy()
                     // File.Move()
                     // Directory.Move()
                     // MSDN: .NET 3.5+: DirectoryNotFoundException: The path specified in sourceFileName or destinationFileName is invalid (for example, it is on an unmapped drive).
                     NativeError.ThrowException(lastError, sourcePathLp);
                     break;

                  case Win32Errors.ERROR_FILE_EXISTS:
                     // File.Copy()
                     // Directory.Copy()
                     NativeError.ThrowException(lastError, destinationPathLp);
                     break;

                  default:
                     // For a number of error codes (sharing violation, path not found, etc)
                     // we don't know if the problem was with the source or dest file.

                     // Check if destination directory already exists.
                     // Directory.Move()
                     // MSDN: .NET 3.5+: IOException: destDirName already exists. 
                     if (ExistsCore(true, transaction, destinationPathLp, PathFormat.LongFullPath))
                        NativeError.ThrowException(Win32Errors.ERROR_ALREADY_EXISTS, destinationPathLp);

                     if (doMove)
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
                        //   Directory exists with the same name as the file.
                        if (!isFolder && ExistsCore(true, transaction, destinationPathLp, PathFormat.LongFullPath))
                           NativeError.ThrowException(lastError, string.Format(CultureInfo.CurrentCulture, Resources.Target_File_Is_A_Directory, destinationPathLp));

                        if (doMove)
                        {
                           var data = new NativeMethods.WIN32_FILE_ATTRIBUTE_DATA();
                           FillAttributeInfoCore(transaction, destinationPathLp, ref data, false, true);

                           if (data.dwFileAttributes != (FileAttributes) (-1))
                           {
                              if ((data.dwFileAttributes & FileAttributes.ReadOnly) != 0)
                              {
                                 // MSDN: .NET 3.5+: IOException: The directory specified by path is read-only.
                                 if (((MoveOptions) moveOptions & MoveOptions.ReplaceExisting) != 0)
                                 {
                                    // Reset file system object attributes.
                                    SetAttributesCore(isFolder, transaction, destinationPathLp, FileAttributes.Normal, true, PathFormat.LongFullPath);

                                    goto startCopyMove;
                                 }


                                 // MSDN: .NET 3.5+: UnauthorizedAccessException: destinationFileName is read-only.
                                 // MSDN: Win32 CopyFileXxx: This function fails with ERROR_ACCESS_DENIED if the destination file already exists
                                 // and has the FILE_ATTRIBUTE_HIDDEN or FILE_ATTRIBUTE_READONLY attribute set.
                                 throw new FileReadOnlyException(destinationPathLp);
                              }


                              // MSDN: Win32 CopyFileXxx: This function fails with ERROR_ACCESS_DENIED if the destination file already exists
                              // and has the FILE_ATTRIBUTE_HIDDEN or FILE_ATTRIBUTE_READONLY attribute set.
                              if ((data.dwFileAttributes & FileAttributes.Hidden) != 0)
                                 NativeError.ThrowException(lastError, string.Format(CultureInfo.CurrentCulture, Resources.File_Is_Hidden, destinationPathLp));
                           }
                        }
                     }


                     // MSDN: .NET 3.5+: An I/O error has occurred. 
                     // File.Copy(): IOException: destinationFileName exists and overwrite is false.
                     // File.Move(): The destination file already exists or sourceFileName was not found.
                     NativeError.ThrowException(lastError, fileNameLp);

                     break;
               }

               #endregion // Win32Errors
            }
         }

         #endregion // Win32 Copy/Move


         #region Transfer Timestamps

         // Apply original Timestamps if requested.
         // MoveFileWithProgress() / MoveFileTransacted() automatically preserve Timestamps.
         // File.Copy()
         if (preserveDates && doCopy && lastError == Win32Errors.ERROR_SUCCESS)
         {
            // Currently preserveDates is only used with files.
            var data = new NativeMethods.WIN32_FILE_ATTRIBUTE_DATA();
            var dataInitialised = FillAttributeInfoCore(transaction, sourcePathLp, ref data, false, true);

            if (dataInitialised == Win32Errors.ERROR_SUCCESS && data.dwFileAttributes != (FileAttributes) (-1))
               SetFsoDateTimeCore(false, transaction, destinationPathLp, DateTime.FromFileTimeUtc(data.ftCreationTime),
                  DateTime.FromFileTimeUtc(data.ftLastAccessTime), DateTime.FromFileTimeUtc(data.ftLastWriteTime), false, PathFormat.LongFullPath);
         }

         #endregion // Transfer Timestamps


         // The copy/move operation succeeded, failed or was canceled.
         return new CopyMoveResult(sourcePathLp, destinationPathLp, isFolder, doMove, cancel, (int) lastError);
      }

      #endregion // Internal Methods
   }
}
