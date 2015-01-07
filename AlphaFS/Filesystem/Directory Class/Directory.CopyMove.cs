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

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Security;

namespace Alphaleonis.Win32.Filesystem
{
   public static partial class Directory
   {
      #region Copy

      // .NET 4.5 and below: Directory class does not contain this method.

      #region AlphaFS
      
      #region Non-Transactional

      // Mimic .NET File.Copy() methods.

      /// <summary>[AlphaFS] Copies an existing directory to a new directory. Overwriting a directory of the same name is not allowed.</summary>
      /// <remarks>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two directories have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException">Passed when the path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">Passed when path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException">Passed when the directory was not found.</exception>
      /// <exception cref="IOException">Passed when an I/O error occurs.</exception>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="sourcePath">The source directory path.</param>
      /// <param name="destinationPath">The destination directory path.</param>
      [SecurityCritical]
      public static void Copy(string sourcePath, string destinationPath)
      {
         CopyMoveInternal(null, sourcePath, destinationPath, CopyOptions.FailIfExists, null, null, null, PathFormat.Relative);
      }

      /// <summary>[AlphaFS] Copies an existing directory to a new directory. Overwriting a directory of the same name is allowed.</summary>
      /// <remarks>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two directories have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException">Passed when the path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">Passed when path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException">Passed when the directory was not found.</exception>
      /// <exception cref="IOException">Passed when an I/O error occurs.</exception>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="sourcePath">The source directory path.</param>
      /// <param name="destinationPath">The destination directory path.</param>
      /// <param name="overwrite"><see langword="true"/> if the destination directory should ignoring the read-only and hidden attributes and overwrite; otherwise, <see langword="false"/>.</param>      
      [SecurityCritical]
      public static void Copy(string sourcePath, string destinationPath, bool overwrite)
      {
         CopyMoveInternal(null, sourcePath, destinationPath, overwrite ? CopyOptions.None : CopyOptions.FailIfExists, null, null, null, PathFormat.Relative);
      }
      
      #endregion // Non-Transactional

      #region Transactional

      /// <summary>[AlphaFS] Copies an existing directory to a new directory. Overwriting a directory of the same name is not allowed.</summary>
      /// <remarks>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two directories have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException">Passed when the path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">Passed when path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException">Passed when the directory was not found.</exception>
      /// <exception cref="IOException">Passed when an I/O error occurs.</exception>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourcePath">The source directory path.</param>
      /// <param name="destinationPath">The destination directory path.</param>
      [SecurityCritical]
      public static void Copy(KernelTransaction transaction, string sourcePath, string destinationPath)
      {
         CopyMoveInternal(transaction, sourcePath, destinationPath, CopyOptions.FailIfExists, null, null, null, PathFormat.Relative);
      }

      /// <summary>[AlphaFS] Copies an existing directory to a new directory. Overwriting a directory of the same name is allowed.</summary>
      /// <remarks>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two directories have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException">Passed when the path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">Passed when path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException">Passed when the directory was not found.</exception>
      /// <exception cref="IOException">Passed when an I/O error occurs.</exception>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourcePath">The source directory path.</param>
      /// <param name="destinationPath">The destination directory path.</param>
      /// <param name="overwrite"><see langword="true"/> if the destination directory should ignoring the read-only and hidden attributes and overwrite; otherwise, <see langword="false"/>.</param>      
      [SecurityCritical]
      public static void Copy(KernelTransaction transaction, string sourcePath, string destinationPath, bool overwrite)
      {
         CopyMoveInternal(transaction, sourcePath, destinationPath, overwrite ? CopyOptions.None : CopyOptions.FailIfExists, null, null, null, PathFormat.Relative);
      }
      
      #endregion // Transactional

      #endregion // AlphaFS

      #endregion // Copy

      #region Copy (CopyOptions)

      #region Non-Transactional

      /// <summary>[AlphaFS] Copies a directory and its contents to a new location, <see cref="CopyOptions"/> can be specified.</summary>
      /// <remarks>
      ///   <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two directories have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException">Passed when the path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">Passed when path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException">Passed when the directory was not found.</exception>
      /// <exception cref="IOException">Passed when an I/O error occurs.</exception>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="sourcePath">The source directory path.</param>
      /// <param name="destinationPath">The destination directory path.</param>
      /// <param name="copyOptions"><see cref="CopyOptions"/> that specify how the directory is to be copied. This parameter can be <see langword="null"/>.</param>
      [SecurityCritical]
      public static void Copy(string sourcePath, string destinationPath, CopyOptions copyOptions)
      {
         CopyMoveInternal(null, sourcePath, destinationPath, copyOptions, null, null, null, PathFormat.Relative);
      }

      /// <summary>[AlphaFS] Copies a directory and its contents to a new location, <see cref="CopyOptions"/> can be specified.</summary>
      /// <remarks>
      ///   <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two directories have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException">Passed when the path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">Passed when path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException">Passed when the directory was not found.</exception>
      /// <exception cref="IOException">Passed when an I/O error occurs.</exception>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="sourcePath">The source directory path.</param>
      /// <param name="destinationPath">The destination directory path.</param>
      /// <param name="copyOptions"><see cref="CopyOptions"/> that specify how the directory is to be copied. This parameter can be <see langword="null"/>.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void Copy(string sourcePath, string destinationPath, CopyOptions copyOptions, PathFormat pathFormat)
      {
         CopyMoveInternal(null, sourcePath, destinationPath, copyOptions, null, null, null, pathFormat);
      }
      
      

      /// <summary>[AlphaFS] Copies a directory and its contents to a new location, <see cref="CopyOptions"/> can be specified,
      ///   <para>and the possibility of notifying the application of its progress through a callback function.</para>
      /// </summary>
      /// <returns>Returns a <see cref="CopyMoveResult"/> class with the status of the Copy action.</returns>
      /// <remarks>
      ///   <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two directories have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException">Passed when the path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">Passed when path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException">Passed when the directory was not found.</exception>
      /// <exception cref="IOException">Passed when an I/O error occurs.</exception>
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
         return CopyMoveInternal(null, sourcePath, destinationPath, copyOptions, null, progressHandler, userProgressData, PathFormat.Relative);
      }

      /// <summary>[AlphaFS] Copies a directory and its contents to a new location, <see cref="CopyOptions"/> can be specified,
      ///   <para>and the possibility of notifying the application of its progress through a callback function.</para>
      /// </summary>
      /// <returns>Returns a <see cref="CopyMoveResult"/> class with the status of the Copy action.</returns>
      /// <remarks>
      ///   <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two directories have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException">Passed when the path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">Passed when path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException">Passed when the directory was not found.</exception>
      /// <exception cref="IOException">Passed when an I/O error occurs.</exception>
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
         return CopyMoveInternal(null, sourcePath, destinationPath, copyOptions, null, progressHandler, userProgressData, pathFormat);
      }

      #endregion // Non-Transactional

      #region Transactional

      /// <summary>[AlphaFS] Copies a directory and its contents to a new location, <see cref="CopyOptions"/> can be specified.</summary>
      /// <remarks>
      ///   <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two directories have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException">Passed when the path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">Passed when path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException">Passed when the directory was not found.</exception>
      /// <exception cref="IOException">Passed when an I/O error occurs.</exception>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourcePath">The source directory path.</param>
      /// <param name="destinationPath">The destination directory path.</param>
      /// <param name="copyOptions"><see cref="CopyOptions"/> that specify how the directory is to be copied. This parameter can be <see langword="null"/>.</param>
      [SecurityCritical]
      public static void Copy(KernelTransaction transaction, string sourcePath, string destinationPath, CopyOptions copyOptions)
      {
         CopyMoveInternal(transaction, sourcePath, destinationPath, copyOptions, null, null, null, PathFormat.Relative);
      }

      /// <summary>[AlphaFS] Copies a directory and its contents to a new location, <see cref="CopyOptions"/> can be specified.</summary>
      /// <remarks>
      ///   <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two directories have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException">Passed when the path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">Passed when path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException">Passed when the directory was not found.</exception>
      /// <exception cref="IOException">Passed when an I/O error occurs.</exception>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourcePath">The source directory path.</param>
      /// <param name="destinationPath">The destination directory path.</param>
      /// <param name="copyOptions"><see cref="CopyOptions"/> that specify how the directory is to be copied. This parameter can be <see langword="null"/>.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void Copy(KernelTransaction transaction, string sourcePath, string destinationPath, CopyOptions copyOptions, PathFormat pathFormat)
      {
         CopyMoveInternal(transaction, sourcePath, destinationPath, copyOptions, null, null, null, pathFormat);
      }



      /// <summary>[AlphaFS] Copies a directory and its contents to a new location, <see cref="CopyOptions"/> can be specified,
      ///   <para>and the possibility of notifying the application of its progress through a callback function.</para>
      /// </summary>
      /// <returns>Returns a <see cref="CopyMoveResult"/> class with the status of the Copy action.</returns>
      /// <remarks>
      ///   <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two directories have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException">Passed when the path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">Passed when path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException">Passed when the directory was not found.</exception>
      /// <exception cref="IOException">Passed when an I/O error occurs.</exception>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourcePath">The source directory path.</param>
      /// <param name="destinationPath">The destination directory path.</param>
      /// <param name="copyOptions"><see cref="CopyOptions"/> that specify how the directory is to be copied. This parameter can be <see langword="null"/>.</param>
      /// <param name="progressHandler">A callback function that is called each time another portion of the directory has been copied. This parameter can be <see langword="null"/>.</param>
      /// <param name="userProgressData">The argument to be passed to the callback function. This parameter can be <see langword="null"/>.</param>
      [SecurityCritical]
      public static CopyMoveResult Copy(KernelTransaction transaction, string sourcePath, string destinationPath, CopyOptions copyOptions, CopyMoveProgressRoutine progressHandler, object userProgressData)
      {
         return CopyMoveInternal(transaction, sourcePath, destinationPath, copyOptions, null, progressHandler, userProgressData, PathFormat.Relative);
      }

      /// <summary>[AlphaFS] Copies a directory and its contents to a new location, <see cref="CopyOptions"/> can be specified,
      ///   <para>and the possibility of notifying the application of its progress through a callback function.</para>
      /// </summary>
      /// <returns>Returns a <see cref="CopyMoveResult"/> class with the status of the Copy action.</returns>
      /// <remarks>
      ///   <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two directories have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException">Passed when the path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">Passed when path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException">Passed when the directory was not found.</exception>
      /// <exception cref="IOException">Passed when an I/O error occurs.</exception>
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
      public static CopyMoveResult Copy(KernelTransaction transaction, string sourcePath, string destinationPath, CopyOptions copyOptions, CopyMoveProgressRoutine progressHandler, object userProgressData, PathFormat pathFormat)
      {
         return CopyMoveInternal(transaction, sourcePath, destinationPath, copyOptions, null, progressHandler, userProgressData, pathFormat);
      }
      
      #endregion // Transactional

      #endregion // Copy (CopyOptions)

      #region Move

      #region .NET

      /// <summary>Moves a file or a directory and its contents to a new location.</summary>
      /// <remarks>
      ///   <para>This method does not work across disk volumes.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two directories have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException">Passed when the path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">Passed when path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException">Passed when the directory was not found.</exception>
      /// <exception cref="IOException">Passed when an I/O error occurs.</exception>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="sourcePath">The source directory path.</param>
      /// <param name="destinationPath">The destination directory path.</param>
      [SecurityCritical]
      public static void Move(string sourcePath, string destinationPath)
      {
         CopyMoveInternal(null, sourcePath, destinationPath, null, MoveOptions.None, null, null, PathFormat.Relative);
      }

      #endregion // .NET

      #region AlphaFS
      
      #region Non-Transactional
      
      /// <summary>[AlphaFS] Moves a file or a directory and its contents to a new location.</summary>
      /// <remarks>
      ///   <para>This method does not work across disk volumes.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two directories have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException">Passed when the path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">Passed when path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException">Passed when the directory was not found.</exception>
      /// <exception cref="IOException">Passed when an I/O error occurs.</exception>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="sourcePath">The source directory path.</param>
      /// <param name="destinationPath">The destination directory path.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void Move(string sourcePath, string destinationPath, PathFormat pathFormat)
      {
         CopyMoveInternal(null, sourcePath, destinationPath, null, MoveOptions.None, null, null, pathFormat);
      }
      
      #endregion // Non-Transactional

      #region Transactional

      /// <summary>[AlphaFS] Moves a file or a directory and its contents to a new location.</summary>
      /// <remarks>
      ///   <para>This method does not work across disk volumes.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two directories have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException">Passed when the path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">Passed when path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException">Passed when the directory was not found.</exception>
      /// <exception cref="IOException">Passed when an I/O error occurs.</exception>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourcePath">The source directory path.</param>
      /// <param name="destinationPath">The destination directory path.</param>
      [SecurityCritical]
      public static void Move(KernelTransaction transaction, string sourcePath, string destinationPath)
      {
         CopyMoveInternal(transaction, sourcePath, destinationPath, null, MoveOptions.None, null, null, PathFormat.Relative);
      }

      /// <summary>[AlphaFS] Moves a file or a directory and its contents to a new location.</summary>
      /// <remarks>
      ///   <para>This method does not work across disk volumes.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two directories have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException">Passed when the path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">Passed when path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException">Passed when the directory was not found.</exception>
      /// <exception cref="IOException">Passed when an I/O error occurs.</exception>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourcePath">The source directory path.</param>
      /// <param name="destinationPath">The destination directory path.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void Move(KernelTransaction transaction, string sourcePath, string destinationPath, PathFormat pathFormat)
      {
         CopyMoveInternal(transaction, sourcePath, destinationPath, null, MoveOptions.None, null, null, pathFormat);
      }
      
      #endregion // Transactional

      #endregion // AlphaFS

      #endregion // Move

      #region Move (MoveOptions)

      #region Non-Transactional

      /// <summary>[AlphaFS] Moves a file or a directory and its contents to a new location, <see cref="MoveOptions"/> can be specified.</summary>
      /// <remarks>
      ///   <para>This method does not work across disk volumes unless <paramref name="moveOptions"/> contains <see cref="MoveOptions.CopyAllowed"/>.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two directories have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException">Passed when the path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">Passed when path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException">Passed when the directory was not found.</exception>
      /// <exception cref="IOException">Passed when an I/O error occurs.</exception>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="sourcePath">The source directory path.</param>
      /// <param name="destinationPath">The destination directory path.</param>
      /// <param name="moveOptions"><see cref="MoveOptions"/> that specify how the directory is to be moved. This parameter can be <see langword="null"/>.</param>
      [SecurityCritical]
      public static void Move(string sourcePath, string destinationPath, MoveOptions moveOptions)
      {
         CopyMoveInternal(null, sourcePath, destinationPath, null, moveOptions, null, null, PathFormat.Relative);
      }

      /// <summary>[AlphaFS] Moves a file or a directory and its contents to a new location, <see cref="MoveOptions"/> can be specified.</summary>
      /// <remarks>
      ///   <para>This method does not work across disk volumes unless <paramref name="moveOptions"/> contains <see cref="MoveOptions.CopyAllowed"/>.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two directories have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException">Passed when the path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">Passed when path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException">Passed when the directory was not found.</exception>
      /// <exception cref="IOException">Passed when an I/O error occurs.</exception>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="sourcePath">The source directory path.</param>
      /// <param name="destinationPath">The destination directory path.</param>
      /// <param name="moveOptions"><see cref="MoveOptions"/> that specify how the directory is to be moved. This parameter can be <see langword="null"/>.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void Move(string sourcePath, string destinationPath, MoveOptions moveOptions, PathFormat pathFormat)
      {
         CopyMoveInternal(null, sourcePath, destinationPath, null, moveOptions, null, null, pathFormat);
      }



      /// <summary>[AlphaFS] Moves a file or a directory and its contents to a new location, <see cref="MoveOptions"/> can be specified,
      ///   <para>and the possibility of notifying the application of its progress through a callback function.</para>
      /// </summary>
      /// <returns>Returns a <see cref="CopyMoveResult"/> class with the status of the Move action.</returns>
      /// <remarks>
      ///   <para>This method does not work across disk volumes unless <paramref name="moveOptions"/> contains <see cref="MoveOptions.CopyAllowed"/>.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two directories have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException">Passed when the path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">Passed when path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException">Passed when the directory was not found.</exception>
      /// <exception cref="IOException">Passed when an I/O error occurs.</exception>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="sourcePath">The source directory path.</param>
      /// <param name="destinationPath">The destination directory path.</param>
      /// <param name="moveOptions"><see cref="MoveOptions"/> that specify how the directory is to be moved. This parameter can be <see langword="null"/>.</param>
      /// <param name="progressHandler">A callback function that is called each time another portion of the directory has been moved. This parameter can be <see langword="null"/>.</param>
      /// <param name="userProgressData">The argument to be passed to the callback function. This parameter can be <see langword="null"/>.</param>
      [SecurityCritical]
      public static CopyMoveResult Move(string sourcePath, string destinationPath, MoveOptions moveOptions, CopyMoveProgressRoutine progressHandler, object userProgressData)
      {
         return CopyMoveInternal(null, sourcePath, destinationPath, null, moveOptions, progressHandler, userProgressData, PathFormat.Relative);
      }

      /// <summary>[AlphaFS] Moves a file or a directory and its contents to a new location, <see cref="MoveOptions"/> can be specified,
      ///   <para>and the possibility of notifying the application of its progress through a callback function.</para>
      /// </summary>
      /// <returns>Returns a <see cref="CopyMoveResult"/> class with the status of the Move action.</returns>
      /// <remarks>
      ///   <para>This method does not work across disk volumes unless <paramref name="moveOptions"/> contains <see cref="MoveOptions.CopyAllowed"/>.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two directories have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException">Passed when the path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">Passed when path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException">Passed when the directory was not found.</exception>
      /// <exception cref="IOException">Passed when an I/O error occurs.</exception>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="sourcePath">The source directory path.</param>
      /// <param name="destinationPath">The destination directory path.</param>
      /// <param name="moveOptions"><see cref="MoveOptions"/> that specify how the directory is to be moved. This parameter can be <see langword="null"/>.</param>
      /// <param name="progressHandler">A callback function that is called each time another portion of the directory has been moved. This parameter can be <see langword="null"/>.</param>
      /// <param name="userProgressData">The argument to be passed to the callback function. This parameter can be <see langword="null"/>.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static CopyMoveResult Move(string sourcePath, string destinationPath, MoveOptions moveOptions, CopyMoveProgressRoutine progressHandler, object userProgressData, PathFormat pathFormat)
      {
         return CopyMoveInternal(null, sourcePath, destinationPath, null, moveOptions, progressHandler, userProgressData, pathFormat);
      }

      #endregion // Non-Transactional

      #region Transactional

      /// <summary>[AlphaFS] Moves a file or a directory and its contents to a new location, <see cref="MoveOptions"/> can be specified.</summary>
      /// <remarks>
      ///   <para>This method does not work across disk volumes unless <paramref name="moveOptions"/> contains <see cref="MoveOptions.CopyAllowed"/>.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two directories have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException">Passed when the path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">Passed when path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException">Passed when the directory was not found.</exception>
      /// <exception cref="IOException">Passed when an I/O error occurs.</exception>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourcePath">The source directory path.</param>
      /// <param name="destinationPath">The destination directory path.</param>
      /// <param name="moveOptions"><see cref="MoveOptions"/> that specify how the directory is to be moved. This parameter can be <see langword="null"/>.</param>
      [SecurityCritical]
      public static void Move(KernelTransaction transaction, string sourcePath, string destinationPath, MoveOptions moveOptions)
      {
         CopyMoveInternal(transaction, sourcePath, destinationPath, null, moveOptions, null, null, PathFormat.Relative);
      }

      /// <summary>[AlphaFS] Moves a file or a directory and its contents to a new location, <see cref="MoveOptions"/> can be specified.</summary>
      /// <remarks>
      ///   <para>This method does not work across disk volumes unless <paramref name="moveOptions"/> contains <see cref="MoveOptions.CopyAllowed"/>.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two directories have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException">Passed when the path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">Passed when path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException">Passed when the directory was not found.</exception>
      /// <exception cref="IOException">Passed when an I/O error occurs.</exception>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourcePath">The source directory path.</param>
      /// <param name="destinationPath">The destination directory path.</param>
      /// <param name="moveOptions"><see cref="MoveOptions"/> that specify how the directory is to be moved. This parameter can be <see langword="null"/>.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void Move(KernelTransaction transaction, string sourcePath, string destinationPath, MoveOptions moveOptions, PathFormat pathFormat)
      {
         CopyMoveInternal(transaction, sourcePath, destinationPath, null, moveOptions, null, null, pathFormat);
      }



      /// <summary>[AlphaFS] Moves a file or a directory and its contents to a new location, <see cref="MoveOptions"/> can be specified,
      ///   <para>and the possibility of notifying the application of its progress through a callback function.</para>
      /// </summary>
      /// <returns>Returns a <see cref="CopyMoveResult"/> class with the status of the Move action.</returns>
      /// <remarks>
      ///   <para>This method does not work across disk volumes unless <paramref name="moveOptions"/> contains <see cref="MoveOptions.CopyAllowed"/>.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two directories have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException">Passed when the path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">Passed when path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException">Passed when the directory was not found.</exception>
      /// <exception cref="IOException">Passed when an I/O error occurs.</exception>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourcePath">The source directory path.</param>
      /// <param name="destinationPath">The destination directory path.</param>
      /// <param name="moveOptions"><see cref="MoveOptions"/> that specify how the directory is to be moved. This parameter can be <see langword="null"/>.</param>
      /// <param name="progressHandler">A callback function that is called each time another portion of the directory has been moved. This parameter can be <see langword="null"/>.</param>
      /// <param name="userProgressData">The argument to be passed to the callback function. This parameter can be <see langword="null"/>.</param>
      [SecurityCritical]
      public static CopyMoveResult Move(KernelTransaction transaction, string sourcePath, string destinationPath, MoveOptions moveOptions, CopyMoveProgressRoutine progressHandler, object userProgressData)
      {
         return CopyMoveInternal(transaction, sourcePath, destinationPath, null, moveOptions, progressHandler, userProgressData, PathFormat.Relative);
      }

      /// <summary>[AlphaFS] Moves a file or a directory and its contents to a new location, <see cref="MoveOptions"/> can be specified,
      ///   <para>and the possibility of notifying the application of its progress through a callback function.</para>
      /// </summary>
      /// <returns>Returns a <see cref="CopyMoveResult"/> class with the status of the Move action.</returns>
      /// <remarks>
      ///   <para>This method does not work across disk volumes unless <paramref name="moveOptions"/> contains <see cref="MoveOptions.CopyAllowed"/>.</para>
      ///   <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      ///   <para>If two directories have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException">Passed when the path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">Passed when path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException">Passed when the directory was not found.</exception>
      /// <exception cref="IOException">Passed when an I/O error occurs.</exception>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourcePath">The source directory path.</param>
      /// <param name="destinationPath">The destination directory path.</param>
      /// <param name="moveOptions"><see cref="MoveOptions"/> that specify how the directory is to be moved. This parameter can be <see langword="null"/>.</param>
      /// <param name="progressHandler">A callback function that is called each time another portion of the directory has been moved. This parameter can be <see langword="null"/>.</param>
      /// <param name="userProgressData">The argument to be passed to the callback function. This parameter can be <see langword="null"/>.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static CopyMoveResult Move(KernelTransaction transaction, string sourcePath, string destinationPath, MoveOptions moveOptions, CopyMoveProgressRoutine progressHandler, object userProgressData, PathFormat pathFormat)
      {
         return CopyMoveInternal(transaction, sourcePath, destinationPath, null, moveOptions, progressHandler, userProgressData, pathFormat);
      }

      #endregion // Transactional

      #endregion // Move (MoveOptions)

      #region Internal Methods

      /// <summary>[AlphaFS] Unified method CopyMoveInternal() to copy/move a Non-/Transacted file or directory including its children to a new location,
      ///   <para><see cref="CopyOptions"/> or <see cref="MoveOptions"/> can be specified,</para>
      ///   <para>and the possibility of notifying the application of its progress through a callback function.</para>
      /// </summary>
      /// <returns>Returns a <see cref="CopyMoveResult"/> class with the status of the Copy or Move action.</returns>
      /// <remarks>
      ///   <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      ///   <para>You cannot use the Move method to overwrite an existing file, unless <paramref name="moveOptions"/> contains <see cref="MoveOptions.ReplaceExisting"/>.</para>
      ///   <para>This Move method works across disk volumes, and it does not throw an exception if the source and destination are the same. </para>
      ///   <para>Note that if you attempt to replace a file by moving a file of the same name into that directory, you get an IOException.</para>
      /// </remarks>
      /// <exception cref="ArgumentException">Passed when the path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">Passed when path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException">Passed when the directory was not found.</exception>
      /// <exception cref="IOException">Passed when an I/O error occurs.</exception>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourcePath">The source directory path.</param>
      /// <param name="destinationPath">The destination directory path.</param>
      /// <param name="copyOptions"><see cref="CopyOptions"/> that specify how the directory is to be copied. This parameter can be <see langword="null"/>.</param>
      /// <param name="moveOptions"><see cref="MoveOptions"/> that specify how the directory is to be moved. This parameter can be <see langword="null"/>.</param>
      /// <param name="progressHandler">A callback function that is called each time another portion of the file has been copied/moved. This parameter can be <see langword="null"/>.</param>
      /// <param name="userProgressData">The argument to be passed to the callback function. This parameter can be <see langword="null"/>.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
      [SecurityCritical]
      internal static CopyMoveResult CopyMoveInternal(KernelTransaction transaction, string sourcePath, string destinationPath, CopyOptions? copyOptions, MoveOptions? moveOptions, CopyMoveProgressRoutine progressHandler, object userProgressData, PathFormat pathFormat)
      {
         #region Setup

         if (pathFormat == PathFormat.Relative)
         {
            Path.CheckValidPath(sourcePath, true, true);
            Path.CheckValidPath(destinationPath, true, true);
         }
         else
         {
            // MSDN:. NET 3.5+: NotSupportedException: Path contains a colon character (:) that is not part of a drive label ("C:\").
            Path.CheckValidPath(sourcePath, false, false);
            Path.CheckValidPath(destinationPath, false, false);
         }

         // MSDN: .NET 4+ Trailing spaces are removed from the end of the path parameters before moving the directory.
         // TrimEnd() is also applied for AlphaFS implementation of method Directory.Copy(), .NET does not have this method.

         var options = GetFullPathOptions.TrimEnd | GetFullPathOptions.RemoveTrailingDirectorySeparator;

         string sourcePathLp = Path.GetExtendedLengthPathInternal(transaction, sourcePath, pathFormat, options);
         string destinationPathLp = Path.GetExtendedLengthPathInternal(transaction, destinationPath, pathFormat, options);

         // MSDN: .NET3.5+: IOException: The sourceDirName and destDirName parameters refer to the same file or directory.
         if (sourcePathLp.Equals(destinationPathLp, StringComparison.OrdinalIgnoreCase))
            NativeError.ThrowException(Win32Errors.ERROR_SAME_DRIVE, destinationPathLp, true);


         // Determine Copy or Move action.
         bool doCopy = copyOptions != null;
         bool doMove = !doCopy && moveOptions != null;

         if ((!doCopy && !doMove) || (doCopy && doMove))
            throw new NotSupportedException(Resources.UndeterminedCopyMoveAction);

         bool overwrite = doCopy
            ? (((CopyOptions)copyOptions & CopyOptions.FailIfExists) != CopyOptions.FailIfExists)
            : (((MoveOptions)moveOptions & MoveOptions.ReplaceExisting) == MoveOptions.ReplaceExisting);

         var cmr = new CopyMoveResult(sourcePathLp, destinationPathLp, true, doMove, false, (int)Win32Errors.ERROR_SUCCESS);

         #endregion //Setup

         #region Copy

         if (doCopy)
         {
            CreateDirectoryInternal(transaction, destinationPathLp, null, null, false, PathFormat.LongFullPath);

            foreach (var fsei in EnumerateFileSystemEntryInfosInternal<FileSystemEntryInfo>(transaction, sourcePathLp, Path.WildcardStarMatchAll, DirectoryEnumerationOptions.FilesAndFolders, PathFormat.LongFullPath))
            {
               string newDestinationPathLp = Path.CombineInternal(false, destinationPathLp, fsei.FileName);

               cmr = fsei.IsDirectory
                  ? CopyMoveInternal(transaction, fsei.LongFullPath, newDestinationPathLp, copyOptions, null, progressHandler, userProgressData, PathFormat.LongFullPath)
                  : File.CopyMoveInternal(false, transaction, fsei.LongFullPath, newDestinationPathLp, false, copyOptions, null, progressHandler, userProgressData, PathFormat.LongFullPath);

               if (cmr.IsCanceled)
                  return cmr;
            }
         }

         #endregion // Copy

         #region Move

         else
         {
            // MSDN: .NET3.5+: IOException: An attempt was made to move a directory to a different volume.
            if (((MoveOptions)moveOptions & MoveOptions.CopyAllowed) != MoveOptions.CopyAllowed)
               if (!Path.GetPathRoot(sourcePathLp, false).Equals(Path.GetPathRoot(destinationPathLp, false), StringComparison.OrdinalIgnoreCase))
                  NativeError.ThrowException(Win32Errors.ERROR_NOT_SAME_DEVICE, destinationPathLp, true);


            // MoveOptions.ReplaceExisting: This value cannot be used if lpNewFileName or lpExistingFileName names a directory.
            if (overwrite && File.ExistsInternal(true, transaction, destinationPathLp, PathFormat.LongFullPath))
               DeleteDirectoryInternal(null, transaction, destinationPathLp, true, true, false, true, PathFormat.LongFullPath);


            // Moves a file or directory, including its children.
            // Copies an existing directory, including its children to a new directory.
            cmr = File.CopyMoveInternal(true, transaction, sourcePathLp, destinationPathLp, false, null, moveOptions, progressHandler, userProgressData, PathFormat.LongFullPath);
         }

         #endregion // Move

         // The copy/move operation succeeded or was canceled.
         return cmr;
      }

      #endregion // CopyMoveInternal
   }
}
