/*  Copyright (C) 2008-2018 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
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
using System.Collections.Generic;
using System.IO;
using System.Security;

namespace Alphaleonis.Win32.Filesystem
{
   public static partial class Directory
   {
      // .NET: Directory class does not contain the Copy() method.
      // Mimic .NET File.Copy() methods.


      /// <summary>[AlphaFS] Copies an existing directory to a new directory. Overwriting a directory of the same name is not allowed.
      /// <remarks>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two directories have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <returns>A <see cref="CopyMoveResult"/> class with details of the Copy action.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="sourcePath">The source directory path.</param>
      /// <param name="destinationPath">The destination directory path.</param>
      [SecurityCritical]
      public static CopyMoveResult Copy(string sourcePath, string destinationPath)
      {
         return CopyMoveCore(null, sourcePath, destinationPath, false, CopyOptions.FailIfExists, null, null, null, null, PathFormat.RelativePath);
      }


      /// <summary>[AlphaFS] Copies an existing directory to a new directory. Overwriting a directory of the same name is not allowed.
      /// <remarks>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two directories have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <returns>A <see cref="CopyMoveResult"/> class with details of the Copy action.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="sourcePath">The source directory path.</param>
      /// <param name="destinationPath">The destination directory path.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static CopyMoveResult Copy(string sourcePath, string destinationPath, PathFormat pathFormat)
      {
         return CopyMoveCore(null, sourcePath, destinationPath, false, CopyOptions.FailIfExists, null, null, null, null, pathFormat);
      }


      /// <summary>[AlphaFS] Copies an existing directory to a new directory. Overwriting a directory of the same name is allowed.
      /// <remarks>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two directories have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <returns>A <see cref="CopyMoveResult"/> class with details of the Copy action.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="sourcePath">The source directory path.</param>
      /// <param name="destinationPath">The destination directory path.</param>
      /// <param name="overwrite"><see langword="true"/> if the destination directory should ignoring the read-only and hidden attributes and overwrite; otherwise, <see langword="false"/>.</param>      
      [SecurityCritical]
      public static CopyMoveResult Copy(string sourcePath, string destinationPath, bool overwrite)
      {
         return CopyMoveCore(null, sourcePath, destinationPath, false, overwrite ? CopyOptions.None : CopyOptions.FailIfExists, null, null, null, null, PathFormat.RelativePath);
      }


      /// <summary>[AlphaFS] Copies an existing directory to a new directory. Overwriting a directory of the same name is allowed.
      /// <remarks>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two directories have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <returns>A <see cref="CopyMoveResult"/> class with details of the Copy action.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="sourcePath">The source directory path.</param>
      /// <param name="destinationPath">The destination directory path.</param>
      /// <param name="overwrite"><see langword="true"/> if the destination directory should ignoring the read-only and hidden attributes and overwrite; otherwise, <see langword="false"/>.</param>      
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static CopyMoveResult Copy(string sourcePath, string destinationPath, bool overwrite, PathFormat pathFormat)
      {
         return CopyMoveCore(null, sourcePath, destinationPath, false, overwrite ? CopyOptions.None : CopyOptions.FailIfExists, null, null, null, null, pathFormat);
      }


      /// <summary>[AlphaFS] Copies an existing directory to a new directory. Overwriting a directory of the same name is not allowed.
      /// <remarks>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two directories have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <returns>A <see cref="CopyMoveResult"/> class with details of the Copy action.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourcePath">The source directory path.</param>
      /// <param name="destinationPath">The destination directory path.</param>
      [SecurityCritical]
      public static CopyMoveResult CopyTransacted(KernelTransaction transaction, string sourcePath, string destinationPath)
      {
         return CopyMoveCore(transaction, sourcePath, destinationPath, false, CopyOptions.FailIfExists, null, null, null, null, PathFormat.RelativePath);
      }


      /// <summary>[AlphaFS] Copies an existing directory to a new directory. Overwriting a directory of the same name is not allowed.
      /// <remarks>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two directories have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <returns>A <see cref="CopyMoveResult"/> class with details of the Copy action.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourcePath">The source directory path.</param>
      /// <param name="destinationPath">The destination directory path.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static CopyMoveResult CopyTransacted(KernelTransaction transaction, string sourcePath, string destinationPath, PathFormat pathFormat)
      {
         return CopyMoveCore(transaction, sourcePath, destinationPath, false, CopyOptions.FailIfExists, null, null, null, null, pathFormat);
      }


      /// <summary>[AlphaFS] Copies an existing directory to a new directory. Overwriting a directory of the same name is allowed.
      /// <remarks>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two directories have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <returns>A <see cref="CopyMoveResult"/> class with details of the Copy action.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourcePath">The source directory path.</param>
      /// <param name="destinationPath">The destination directory path.</param>
      /// <param name="overwrite"><see langword="true"/> if the destination directory should ignoring the read-only and hidden attributes and overwrite; otherwise, <see langword="false"/>.</param>      
      [SecurityCritical]
      public static CopyMoveResult CopyTransacted(KernelTransaction transaction, string sourcePath, string destinationPath, bool overwrite)
      {
         return CopyMoveCore(transaction, sourcePath, destinationPath, false, overwrite ? CopyOptions.None : CopyOptions.FailIfExists, null, null, null, null, PathFormat.RelativePath);
      }


      /// <summary>[AlphaFS] Copies an existing directory to a new directory. Overwriting a directory of the same name is allowed.
      /// <remarks>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two directories have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <returns>A <see cref="CopyMoveResult"/> class with details of the Copy action.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourcePath">The source directory path.</param>
      /// <param name="destinationPath">The destination directory path.</param>
      /// <param name="overwrite"><see langword="true"/> if the destination directory should ignoring the read-only and hidden attributes and overwrite; otherwise, <see langword="false"/>.</param>      
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static CopyMoveResult CopyTransacted(KernelTransaction transaction, string sourcePath, string destinationPath, bool overwrite, PathFormat pathFormat)
      {
         return CopyMoveCore(transaction, sourcePath, destinationPath, false, overwrite ? CopyOptions.None : CopyOptions.FailIfExists, null, null, null, null, pathFormat);
      }




      /// <summary>[AlphaFS] Copies a directory and its contents to a new location, <see cref="CopyOptions"/> can be specified.
      /// <remarks>
      ///   <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two directories have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <returns>A <see cref="CopyMoveResult"/> class with details of the Copy action.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="sourcePath">The source directory path.</param>
      /// <param name="destinationPath">The destination directory path.</param>
      /// <param name="copyOptions"><see cref="CopyOptions"/> that specify how the directory is to be copied. This parameter can be <see langword="null"/>.</param>
      [SecurityCritical]
      public static CopyMoveResult Copy(string sourcePath, string destinationPath, CopyOptions copyOptions)
      {
         return CopyMoveCore(null, sourcePath, destinationPath, false, copyOptions, null, null, null, null, PathFormat.RelativePath);
      }


      /// <summary>[AlphaFS] Copies a directory and its contents to a new location, <see cref="CopyOptions"/> can be specified.
      /// <remarks>
      ///   <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two directories have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <returns>A <see cref="CopyMoveResult"/> class with details of the Copy action.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="sourcePath">The source directory path.</param>
      /// <param name="destinationPath">The destination directory path.</param>
      /// <param name="copyOptions"><see cref="CopyOptions"/> that specify how the directory is to be copied. This parameter can be <see langword="null"/>.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static CopyMoveResult Copy(string sourcePath, string destinationPath, CopyOptions copyOptions, PathFormat pathFormat)
      {
         return CopyMoveCore(null, sourcePath, destinationPath, false, copyOptions, null, null, null, null, pathFormat);
      }


      /// <summary>[AlphaFS] Copies a directory and its contents to a new location, <see cref="CopyOptions"/> can be specified.
      /// <remarks>
      ///   <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two directories have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <returns>A <see cref="CopyMoveResult"/> class with details of the Copy action.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="sourcePath">The source directory path.</param>
      /// <param name="destinationPath">The destination directory path.</param>
      /// <param name="copyOptions"><see cref="CopyOptions"/> that specify how the directory is to be copied. This parameter can be <see langword="null"/>.</param>
      /// <param name="preserveDates"><see langword="true"/> if original Timestamps must be preserved, <see langword="false"/> otherwise.</param>
      [SecurityCritical]
      public static CopyMoveResult Copy(string sourcePath, string destinationPath, CopyOptions copyOptions, bool preserveDates)
      {
         return CopyMoveCore(null, sourcePath, destinationPath, preserveDates, copyOptions, null, null, null, null, PathFormat.RelativePath);
      }


      /// <summary>[AlphaFS] Copies a directory and its contents to a new location, <see cref="CopyOptions"/> can be specified.
      /// <remarks>
      ///   <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two directories have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <returns>A <see cref="CopyMoveResult"/> class with details of the Copy action.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="sourcePath">The source directory path.</param>
      /// <param name="destinationPath">The destination directory path.</param>
      /// <param name="copyOptions"><see cref="CopyOptions"/> that specify how the directory is to be copied. This parameter can be <see langword="null"/>.</param>
      /// <param name="preserveDates"><see langword="true"/> if original Timestamps must be preserved, <see langword="false"/> otherwise.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static CopyMoveResult Copy(string sourcePath, string destinationPath, CopyOptions copyOptions, bool preserveDates, PathFormat pathFormat)
      {
         return CopyMoveCore(null, sourcePath, destinationPath, preserveDates, copyOptions, null, null, null, null, pathFormat);
      }




      /// <summary>[AlphaFS] Copies a directory and its contents to a new location, <see cref="CopyOptions"/> can be specified,
      ///   and the possibility of notifying the application of its progress through a callback function.
      /// <remarks>
      ///   <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two directories have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <returns>A <see cref="CopyMoveResult"/> class with details of the Copy action.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="sourcePath">The source directory path.</param>
      /// <param name="destinationPath">The destination directory path.</param>
      /// <param name="copyOptions"><see cref="CopyOptions"/> that specify how the directory is to be copied. This parameter can be <see langword="null"/>.</param>
      /// <param name="progressHandler">A callback function that is called each time another portion of the directory has been copied. This parameter can be <see langword="null"/>.</param>
      /// <param name="userProgressData">The argument to be passed to the callback function. This parameter can be <see langword="null"/>.</param>
      [SecurityCritical]
      public static CopyMoveResult Copy(string sourcePath, string destinationPath, CopyOptions copyOptions, CopyMoveProgressRoutine progressHandler, object userProgressData)
      {
         return CopyMoveCore(null, sourcePath, destinationPath, false, copyOptions, null, progressHandler, userProgressData, null, PathFormat.RelativePath);
      }


      /// <summary>[AlphaFS] Copies a directory and its contents to a new location, <see cref="CopyOptions"/> can be specified,
      ///   and the possibility of notifying the application of its progress through a callback function.
      /// <remarks>
      ///   <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two directories have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <returns>A <see cref="CopyMoveResult"/> class with details of the Copy action.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="sourcePath">The source directory path.</param>
      /// <param name="destinationPath">The destination directory path.</param>
      /// <param name="copyOptions"><see cref="CopyOptions"/> that specify how the directory is to be copied. This parameter can be <see langword="null"/>.</param>
      /// <param name="progressHandler">A callback function that is called each time another portion of the directory has been copied. This parameter can be <see langword="null"/>.</param>
      /// <param name="userProgressData">The argument to be passed to the callback function. This parameter can be <see langword="null"/>.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static CopyMoveResult Copy(string sourcePath, string destinationPath, CopyOptions copyOptions, CopyMoveProgressRoutine progressHandler, object userProgressData, PathFormat pathFormat)
      {
         return CopyMoveCore(null, sourcePath, destinationPath, false, copyOptions, null, progressHandler, userProgressData, null, pathFormat);
      }


      /// <summary>[AlphaFS] Copies a directory and its contents to a new location, <see cref="CopyOptions"/> can be specified,
      ///   and the possibility of notifying the application of its progress through a callback function.
      /// <remarks>
      ///   <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two directories have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <returns>A <see cref="CopyMoveResult"/> class with details of the Copy action.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="sourcePath">The source directory path.</param>
      /// <param name="destinationPath">The destination directory path.</param>
      /// <param name="copyOptions"><see cref="CopyOptions"/> that specify how the directory is to be copied. This parameter can be <see langword="null"/>.</param>
      /// <param name="preserveDates"><see langword="true"/> if original Timestamps must be preserved, <see langword="false"/> otherwise.</param>
      /// <param name="progressHandler">A callback function that is called each time another portion of the directory has been copied. This parameter can be <see langword="null"/>.</param>
      /// <param name="userProgressData">The argument to be passed to the callback function. This parameter can be <see langword="null"/>.</param>
      [SecurityCritical]
      public static CopyMoveResult Copy(string sourcePath, string destinationPath, CopyOptions copyOptions, bool preserveDates, CopyMoveProgressRoutine progressHandler, object userProgressData)
      {
         return CopyMoveCore(null, sourcePath, destinationPath, preserveDates, copyOptions, null, progressHandler, userProgressData, null, PathFormat.RelativePath);
      }


      /// <summary>[AlphaFS] Copies a directory and its contents to a new location, <see cref="CopyOptions"/> can be specified,
      ///   and the possibility of notifying the application of its progress through a callback function.
      /// <remarks>
      ///   <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two directories have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <returns>A <see cref="CopyMoveResult"/> class with details of the Copy action.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="sourcePath">The source directory path.</param>
      /// <param name="destinationPath">The destination directory path.</param>
      /// <param name="copyOptions"><see cref="CopyOptions"/> that specify how the directory is to be copied. This parameter can be <see langword="null"/>.</param>
      /// <param name="preserveDates"><see langword="true"/> if original Timestamps must be preserved, <see langword="false"/> otherwise.</param>
      /// <param name="progressHandler">A callback function that is called each time another portion of the directory has been copied. This parameter can be <see langword="null"/>.</param>
      /// <param name="userProgressData">The argument to be passed to the callback function. This parameter can be <see langword="null"/>.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static CopyMoveResult Copy(string sourcePath, string destinationPath, CopyOptions copyOptions, bool preserveDates, CopyMoveProgressRoutine progressHandler, object userProgressData, PathFormat pathFormat)
      {
         return CopyMoveCore(null, sourcePath, destinationPath, preserveDates, copyOptions, null, progressHandler, userProgressData, null, pathFormat);
      }



      /// <summary>[AlphaFS] Copies a directory and its contents to a new location, <see cref="CopyOptions"/> can be specified.
      /// <remarks>
      ///   <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two directories have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <returns>A <see cref="CopyMoveResult"/> class with details of the Copy action.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourcePath">The source directory path.</param>
      /// <param name="destinationPath">The destination directory path.</param>
      /// <param name="copyOptions"><see cref="CopyOptions"/> that specify how the directory is to be copied. This parameter can be <see langword="null"/>.</param>
      [SecurityCritical]
      public static CopyMoveResult CopyTransacted(KernelTransaction transaction, string sourcePath, string destinationPath, CopyOptions copyOptions)
      {
         return CopyMoveCore(transaction, sourcePath, destinationPath, false, copyOptions, null, null, null, null, PathFormat.RelativePath);
      }


      /// <summary>[AlphaFS] Copies a directory and its contents to a new location, <see cref="CopyOptions"/> can be specified.
      /// <remarks>
      ///   <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two directories have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <returns>A <see cref="CopyMoveResult"/> class with details of the Copy action.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourcePath">The source directory path.</param>
      /// <param name="destinationPath">The destination directory path.</param>
      /// <param name="copyOptions"><see cref="CopyOptions"/> that specify how the directory is to be copied. This parameter can be <see langword="null"/>.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static CopyMoveResult CopyTransacted(KernelTransaction transaction, string sourcePath, string destinationPath, CopyOptions copyOptions, PathFormat pathFormat)
      {
         return CopyMoveCore(transaction, sourcePath, destinationPath, false, copyOptions, null, null, null, null, pathFormat);
      }


      /// <summary>[AlphaFS] Copies a directory and its contents to a new location, <see cref="CopyOptions"/> can be specified.
      /// <remarks>
      ///   <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two directories have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <returns>A <see cref="CopyMoveResult"/> class with details of the Copy action.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourcePath">The source directory path.</param>
      /// <param name="destinationPath">The destination directory path.</param>
      /// <param name="copyOptions"><see cref="CopyOptions"/> that specify how the directory is to be copied. This parameter can be <see langword="null"/>.</param>
      /// <param name="preserveDates"><see langword="true"/> if original Timestamps must be preserved, <see langword="false"/> otherwise.</param>
      [SecurityCritical]
      public static CopyMoveResult CopyTransacted(KernelTransaction transaction, string sourcePath, string destinationPath, CopyOptions copyOptions, bool preserveDates)
      {
         return CopyMoveCore(transaction, sourcePath, destinationPath, preserveDates, copyOptions, null, null, null, null, PathFormat.RelativePath);
      }


      /// <summary>[AlphaFS] Copies a directory and its contents to a new location, <see cref="CopyOptions"/> can be specified.
      /// <remarks>
      ///   <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two directories have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <returns>A <see cref="CopyMoveResult"/> class with details of the Copy action.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourcePath">The source directory path.</param>
      /// <param name="destinationPath">The destination directory path.</param>
      /// <param name="copyOptions"><see cref="CopyOptions"/> that specify how the directory is to be copied. This parameter can be <see langword="null"/>.</param>
      /// <param name="preserveDates"><see langword="true"/> if original Timestamps must be preserved, <see langword="false"/> otherwise.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static CopyMoveResult CopyTransacted(KernelTransaction transaction, string sourcePath, string destinationPath, CopyOptions copyOptions, bool preserveDates, PathFormat pathFormat)
      {
         return CopyMoveCore(transaction, sourcePath, destinationPath, preserveDates, copyOptions, null, null, null, null, pathFormat);
      }




      /// <summary>[AlphaFS] Copies a directory and its contents to a new location, <see cref="CopyOptions"/> can be specified,
      ///   and the possibility of notifying the application of its progress through a callback function.
      /// <remarks>
      ///   <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two directories have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <returns>A <see cref="CopyMoveResult"/> class with details of the Copy action.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourcePath">The source directory path.</param>
      /// <param name="destinationPath">The destination directory path.</param>
      /// <param name="copyOptions"><see cref="CopyOptions"/> that specify how the directory is to be copied. This parameter can be <see langword="null"/>.</param>
      /// <param name="progressHandler">A callback function that is called each time another portion of the directory has been copied. This parameter can be <see langword="null"/>.</param>
      /// <param name="userProgressData">The argument to be passed to the callback function. This parameter can be <see langword="null"/>.</param>
      [SecurityCritical]
      public static CopyMoveResult CopyTransacted(KernelTransaction transaction, string sourcePath, string destinationPath, CopyOptions copyOptions, CopyMoveProgressRoutine progressHandler, object userProgressData)
      {
         return CopyMoveCore(transaction, sourcePath, destinationPath, false, copyOptions, null, progressHandler, userProgressData, null, PathFormat.RelativePath);
      }


      /// <summary>[AlphaFS] Copies a directory and its contents to a new location, <see cref="CopyOptions"/> can be specified,
      ///   and the possibility of notifying the application of its progress through a callback function.
      /// <remarks>
      ///   <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two directories have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <returns>A <see cref="CopyMoveResult"/> class with details of the Copy action.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourcePath">The source directory path.</param>
      /// <param name="destinationPath">The destination directory path.</param>
      /// <param name="copyOptions"><see cref="CopyOptions"/> that specify how the directory is to be copied. This parameter can be <see langword="null"/>.</param>
      /// <param name="progressHandler">A callback function that is called each time another portion of the directory has been copied. This parameter can be <see langword="null"/>.</param>
      /// <param name="userProgressData">The argument to be passed to the callback function. This parameter can be <see langword="null"/>.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static CopyMoveResult CopyTransacted(KernelTransaction transaction, string sourcePath, string destinationPath, CopyOptions copyOptions, CopyMoveProgressRoutine progressHandler, object userProgressData, PathFormat pathFormat)
      {
         return CopyMoveCore(transaction, sourcePath, destinationPath, false, copyOptions, null, progressHandler, userProgressData, null, pathFormat);
      }


      /// <summary>[AlphaFS] Copies a directory and its contents to a new location, <see cref="CopyOptions"/> can be specified,
      ///   and the possibility of notifying the application of its progress through a callback function.
      /// <remarks>
      ///   <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two directories have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <returns>A <see cref="CopyMoveResult"/> class with details of the Copy action.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourcePath">The source directory path.</param>
      /// <param name="destinationPath">The destination directory path.</param>
      /// <param name="copyOptions"><see cref="CopyOptions"/> that specify how the directory is to be copied. This parameter can be <see langword="null"/>.</param>
      /// <param name="preserveDates"><see langword="true"/> if original Timestamps must be preserved, <see langword="false"/> otherwise.</param>
      /// <param name="progressHandler">A callback function that is called each time another portion of the directory has been copied. This parameter can be <see langword="null"/>.</param>
      /// <param name="userProgressData">The argument to be passed to the callback function. This parameter can be <see langword="null"/>.</param>
      [SecurityCritical]
      public static CopyMoveResult CopyTransacted(KernelTransaction transaction, string sourcePath, string destinationPath, CopyOptions copyOptions, bool preserveDates, CopyMoveProgressRoutine progressHandler, object userProgressData)
      {
         return CopyMoveCore(transaction, sourcePath, destinationPath, preserveDates, copyOptions, null, progressHandler, userProgressData, null, PathFormat.RelativePath);
      }


      /// <summary>[AlphaFS] Copies a directory and its contents to a new location, <see cref="CopyOptions"/> can be specified,
      ///   and the possibility of notifying the application of its progress through a callback function.
      /// <remarks>
      ///   <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two directories have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <returns>A <see cref="CopyMoveResult"/> class with details of the Copy action.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourcePath">The source directory path.</param>
      /// <param name="destinationPath">The destination directory path.</param>
      /// <param name="copyOptions"><see cref="CopyOptions"/> that specify how the directory is to be copied. This parameter can be <see langword="null"/>.</param>
      /// <param name="preserveDates"><see langword="true"/> if original Timestamps must be preserved, <see langword="false"/> otherwise.</param>
      /// <param name="progressHandler">A callback function that is called each time another portion of the directory has been copied. This parameter can be <see langword="null"/>.</param>
      /// <param name="userProgressData">The argument to be passed to the callback function. This parameter can be <see langword="null"/>.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static CopyMoveResult CopyTransacted(KernelTransaction transaction, string sourcePath, string destinationPath, CopyOptions copyOptions, bool preserveDates, CopyMoveProgressRoutine progressHandler, object userProgressData, PathFormat pathFormat)
      {
         return CopyMoveCore(transaction, sourcePath, destinationPath, preserveDates, copyOptions, null, progressHandler, userProgressData, null, pathFormat);
      }




      #region Internal Methods

      /// <summary>[AlphaFS] Copy/move a Non-/Transacted file or directory including its children to a new location,
      ///   <see cref="CopyOptions"/> or <see cref="MoveOptions"/> can be specified,
      ///   and the possibility of notifying the application of its progress through a callback function.
      /// <remarks>
      ///   <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      ///   <para>You cannot use the Move method to overwrite an existing file, unless <paramref name="moveOptions"/> contains <see cref="MoveOptions.ReplaceExisting"/>.</para>
      ///   <para>Note that if you attempt to replace a file by moving a file of the same name into that directory, you get an IOException.</para>
      /// </remarks>
      /// </summary>
      /// <returns>A <see cref="CopyMoveResult"/> class with the status of the Copy or Move action.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="PlatformNotSupportedException">The operating system is older than Windows Vista.</exception>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourcePath">The source directory path.</param>
      /// <param name="destinationPath">The destination directory path.</param>
      /// <param name="preserveDates"><see langword="true"/> if original Timestamps must be preserved, <see langword="false"/> otherwise. This parameter is ignored for move operations.</param>
      /// <param name="copyOptions"><see cref="CopyOptions"/> that specify how the directory is to be copied. This parameter can be <see langword="null"/>.</param>
      /// <param name="moveOptions"><see cref="MoveOptions"/> that specify how the directory is to be moved. This parameter can be <see langword="null"/>.</param>
      /// <param name="progressHandler">A callback function that is called each time another portion of the file has been copied/moved. This parameter can be <see langword="null"/>.</param>
      /// <param name="userProgressData">The argument to be passed to the callback function. This parameter can be <see langword="null"/>.</param>
      /// <param name="copyMoveResult">A <see cref="CopyMoveResult"/> instance containing Copy or Move action progress.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      internal static CopyMoveResult CopyMoveCore(KernelTransaction transaction, string sourcePath, string destinationPath, bool preserveDates, CopyOptions? copyOptions, MoveOptions? moveOptions, CopyMoveProgressRoutine progressHandler, object userProgressData, CopyMoveResult copyMoveResult, PathFormat pathFormat)
      {
         string sourcePathLp;
         string destinationPathLp;
         bool isCopy;
         
         // A Move action fallback using Copy + Delete.
         bool emulateMove;

         // A file or folder will be deleted or renamed on Computer startup.
         bool delayUntilReboot;
         bool deleteOnStartup;
         

         File.ValidateAndUpdatePathsAndOptions(transaction, sourcePath, destinationPath, copyOptions, moveOptions, pathFormat, out sourcePathLp, out destinationPathLp, out isCopy, out emulateMove, out delayUntilReboot, out deleteOnStartup);


         // Directory.Move is applicable to both folders and files.
         var isFile = File.ExistsCore(transaction, false, sourcePath, PathFormat.LongFullPath);


         // Check for local or network drives, such as: "C:" or "\\server\c$".
         ExistsDriveOrFolderOrFile(transaction, sourcePathLp, !isFile, (int) Win32Errors.NO_ERROR, true, false);
         

         // File Move action: destinationPath is allowed to be null when MoveOptions.DelayUntilReboot is specified.
         if (!delayUntilReboot)
            ExistsDriveOrFolderOrFile(transaction, destinationPathLp, !isFile, (int) Win32Errors.NO_ERROR, true, false);
         
         
         // Process Move action options, possible fallback to Copy action.
         if (!isCopy && !deleteOnStartup)
            ValidateAndUpdateCopyMoveAction(sourcePathLp, destinationPathLp, copyOptions, moveOptions, out copyOptions, out moveOptions, out isCopy, out emulateMove);
         
         
         pathFormat = PathFormat.LongFullPath;
         
         var cmr = copyMoveResult ?? new CopyMoveResult(sourcePath, destinationPath, isCopy, !isFile, preserveDates, emulateMove);
         

         if (isCopy)
         {
            // Copy folder SymbolicLinks.
            // Cannot be done by CopyFileEx() so emulate this.
            
            if (File.HasCopySymbolicLink(copyOptions))
            {
               var lvi = File.GetLinkTargetInfoCore(transaction, sourcePathLp, true, pathFormat);

               if (null != lvi)
               {
                  File.CreateSymbolicLinkCore(transaction, destinationPathLp, lvi.SubstituteName, SymbolicLinkTarget.Directory, pathFormat);

                  cmr.TotalFolders = 1;
               }
            }

            else
            {
               cmr = isFile
                  ? File.CopyMoveCore(transaction, true, false, sourcePathLp, destinationPathLp, copyOptions, null, preserveDates, progressHandler, userProgressData, cmr, PathFormat.LongFullPath)
                  : CopyDeleteDirectoryCore(transaction, sourcePathLp, destinationPathLp, preserveDates, emulateMove, copyOptions, progressHandler, userProgressData, cmr);
            }
         }

         // Move
         else
         {
            // AlphaFS feature to overcome a MoveFileXxx limitation.
            // MoveOptions.ReplaceExisting: This value cannot be used if lpNewFileName or lpExistingFileName names a directory.

            if (!isFile && !delayUntilReboot && File.CanOverwrite(moveOptions))
               DeleteDirectoryCore(transaction, null, destinationPathLp, true, true, true, pathFormat);

            // 2017-06-07: A large target directory will probably create a progress-less delay in UI.
            // One way to get around this is to perform the delete in the File.CopyMove method.


            // Moves a file or directory, including its children.
            // Copies an existing directory, including its children to a new directory.

            cmr = File.CopyMoveCore(transaction, true, !isFile, sourcePathLp, destinationPathLp, copyOptions, moveOptions, preserveDates, progressHandler, userProgressData, cmr, pathFormat);


            // If the move happened on the same drive, we have no knowledge of the number of files/folders.
            // However, we do know that the one folder was moved successfully.

            if (cmr.ErrorCode == Win32Errors.NO_ERROR)
               cmr.TotalFolders = 1;
         }

         
         cmr._stopwatch.Stop();

         return cmr;
      }


      private static CopyMoveResult CopyDeleteDirectoryCore(KernelTransaction transaction, string sourcePathLp, string destinationPathLp, bool preserveDates, bool emulateMove, CopyOptions? copyOptions, CopyMoveProgressRoutine progressHandler, object userProgressData, CopyMoveResult copyMoveResult)
      {
         var cmr = copyMoveResult ?? new CopyMoveResult(sourcePathLp, destinationPathLp, true, true, preserveDates, emulateMove);
         

         var dirs = new Queue<string>(NativeMethods.DefaultFileBufferSize);

         dirs.Enqueue(sourcePathLp);
         

         while (dirs.Count > 0)
         {
            var srcLp = dirs.Dequeue();

            // TODO 2018-01-09: Not 100% yet with local + UNC paths.
            var dstLp = srcLp.Replace(sourcePathLp, destinationPathLp);


            // Traverse the source folder, processing files and folders.

            foreach (var fseiSource in EnumerateFileSystemEntryInfosCore<FileSystemEntryInfo>(null, transaction, srcLp, Path.WildcardStarMatchAll, null, null, null, PathFormat.LongFullPath))
            {
               var fseiSourcePath = fseiSource.LongFullPath;
               var fseiDestinationPath = Path.CombineCore(false, dstLp, fseiSource.FileName);


               if (fseiSource.IsDirectory)
               {
                  CreateDirectoryCore(transaction, fseiDestinationPath, null, null, false, PathFormat.LongFullPath);

                  cmr.TotalFolders++;

                  dirs.Enqueue(fseiSourcePath);
               }


               // File.
               else
               {
                  // File count is done in File.CopyMoveCore method.

                  cmr = File.CopyMoveCore(transaction, true, false, fseiSourcePath, fseiDestinationPath, copyOptions, null, preserveDates, progressHandler, userProgressData, cmr, PathFormat.LongFullPath);

                  if (cmr.IsCanceled)
                  {
                     // Break while loop.
                     dirs.Clear();

                     // Break foreach loop.
                     break;
                  }


                  if (cmr.ErrorCode == Win32Errors.ERROR_SUCCESS)
                  {
                     cmr.TotalBytes += fseiSource.FileSize;

                     if (emulateMove)
                        File.DeleteFileCore(transaction, fseiSourcePath, true, PathFormat.LongFullPath);
                  }
               }
            }
         }


         if (cmr.ErrorCode == Win32Errors.ERROR_SUCCESS)
         {
            if (preserveDates)
            {
               // TODO 2018-01-09: Not 100% yet with local + UNC paths.
               var dstLp = sourcePathLp.Replace(sourcePathLp, destinationPathLp);


               // Traverse the source folder, processing subfolders.

               foreach (var fseiSource in EnumerateFileSystemEntryInfosCore<FileSystemEntryInfo>(true, transaction, sourcePathLp, Path.WildcardStarMatchAll, null, null, null, PathFormat.LongFullPath))

                  File.CopyTimestampsCore(transaction, fseiSource.LongFullPath, Path.CombineCore(false, dstLp, fseiSource.FileName), false, PathFormat.LongFullPath);

               // TODO: When enabled on Computer, FindFirstFile will change the last accessed time.
            }


            if (emulateMove)
               DeleteDirectoryCore(transaction, null, sourcePathLp, true, true, true, PathFormat.LongFullPath);
         }


         return cmr;
      }


      private static void ValidateAndUpdateCopyMoveAction(string sourcePathLp, string destinationPathLp, CopyOptions? copyOptions, MoveOptions? moveOptions, out CopyOptions? newCopyOptions, out MoveOptions? newMoveOptions, out bool isCopy, out bool emulateMove)
      {
         // Determine if a Move action or Copy action-fallback is possible.
         isCopy = emulateMove = false;


         // Compare the root part of both paths.
         var equalRootPaths = Path.GetPathRoot(sourcePathLp, false).Equals(Path.GetPathRoot(destinationPathLp, false), StringComparison.OrdinalIgnoreCase);


         // Method Volume.IsSameVolume() returns true when both paths refer to the same volume, even if one of the paths is a UNC path.
         // For example, src = C:\TempSrc and dst = \\localhost\C$\TempDst
         var isSameVolume = equalRootPaths || Volume.IsSameVolume(sourcePathLp, destinationPathLp);


         var isMove = isSameVolume && equalRootPaths;

         if (!isMove)
         {
            // A Move() can be emulated by using Copy() and Delete(), but only if the MoveOptions.CopyAllowed flag is set.
            isMove = File.AllowEmulate(moveOptions);

            // MSDN: .NET3.5+: IOException: An attempt was made to move a directory to a different volume.
            if (!isMove)
               NativeError.ThrowException(Win32Errors.ERROR_NOT_SAME_DEVICE, sourcePathLp, destinationPathLp);
         }


         // The MoveFileXxx methods fail when:
         // - A directory is being moved;
         // - One of the paths is a UNC path, even though both paths refer to the same volume.
         //   For example, src = C:\TempSrc and dst = \\localhost\C$\TempDst
         if (isMove)
         {
            var srcIsUncPath = Path.IsUncPathCore(sourcePathLp, false, false);
            var dstIsUncPath = Path.IsUncPathCore(destinationPathLp, false, false);

            isMove = srcIsUncPath == dstIsUncPath;
         }


         isMove = isMove && isSameVolume && equalRootPaths;


         // Emulate Move().
         if (!isMove)
         {
            moveOptions = null;

            isCopy = emulateMove = true;
            copyOptions = CopyOptions.None;
         }


         newCopyOptions = copyOptions;
         newMoveOptions = moveOptions;
      }

      #endregion // Internal Methods
   }
}
