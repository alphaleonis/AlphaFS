/* Copyright (c) 2008-2014 Peter Palotas, Alexandr Normuradov, Jeffrey Jangli
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

using Alphaleonis.Win32.Security;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.AccessControl;
using System.Text;
using FileStream = System.IO.FileStream;
using StreamReader = System.IO.StreamReader;
using StreamWriter = System.IO.StreamWriter;

namespace Alphaleonis.Win32.Filesystem
{
   /// <summary>Provides static methods for the creation, copying, deletion, moving, and opening of files, and aids in the creation of <see cref="T:FileStream"/> objects.</summary>
   public static class File
   {
      #region .NET

      #region AppendAllLines

      #region .NET

      /// <summary>Appends lines to a file, and then closes the file. If the specified file does not exist, this method creates a file, writes the specified lines to the file, and then closes the file.</summary>
      /// <param name="path">The file to append the lines to. The file is created if it doesn't already exist.</param>
      /// <param name="contents">The lines to append to the file.</param>
      /// <remarks>The method creates the file if it doesn’t exist, but it doesn't create new directories. Therefore, the value of the path parameter must contain existing directories.</remarks>
      [SecurityCritical]
      public static void AppendAllLines(string path, IEnumerable<string> contents)
      {
         WriteAppendAllLinesInternal(null, path, contents, NativeMethods.DefaultFileEncoding, true, false, false);
      }

      /// <summary>Appends lines to a file, and then closes the file. If the specified file does not exist, this method creates a file, writes the specified lines to the file, and then closes the file.</summary>
      /// <param name="path">The file to append the lines to. The file is created if it doesn't already exist.</param>
      /// <param name="contents">The lines to append to the file.</param>
      /// <param name="encoding">The character <see cref="T:Encoding"/> to use.</param>
      /// <remarks>The method creates the file if it doesn’t exist, but it doesn't create new directories. Therefore, the value of the path parameter must contain existing directories.</remarks>
      [SecurityCritical]
      public static void AppendAllLines(string path, IEnumerable<string> contents, Encoding encoding)
      {
         WriteAppendAllLinesInternal(null, path, contents, encoding, true, false, false);
      }

      #endregion // .NET

      #region AlphaFS

      #region IsFullPath

      /// <summary>[AlphaFS] Appends lines to a file, and then closes the file. If the specified file does not exist, this method creates a file, writes the specified lines to the file, and then closes the file.</summary>
      /// <param name="path">The file to append the lines to. The file is created if it doesn't already exist.</param>
      /// <param name="contents">The lines to append to the file.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <remarks>The method creates the file if it doesn’t exist, but it doesn't create new directories. Therefore, the value of the path parameter must contain existing directories.</remarks>
      [SecurityCritical]
      public static void AppendAllLines(string path, IEnumerable<string> contents, bool? isFullPath)
      {
         WriteAppendAllLinesInternal(null, path, contents, NativeMethods.DefaultFileEncoding, true, false, isFullPath);
      }

      /// <summary>[AlphaFS] Appends lines to a file, and then closes the file. If the specified file does not exist, this method creates a file, writes the specified lines to the file, and then closes the file.</summary>
      /// <param name="path">The file to append the lines to. The file is created if it doesn't already exist.</param>
      /// <param name="contents">The lines to append to the file.</param>
      /// <param name="encoding">The character <see cref="T:Encoding"/> to use.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <remarks>The method creates the file if it doesn’t exist, but it doesn't create new directories. Therefore, the value of the path parameter must contain existing directories.</remarks>
      [SecurityCritical]
      public static void AppendAllLines(string path, IEnumerable<string> contents, Encoding encoding, bool? isFullPath)
      {
         WriteAppendAllLinesInternal(null, path, contents, encoding, true, false, isFullPath);
      }

      #endregion // IsFullPath

      #region Transacted

      #region .NET

      /// <summary>[AlphaFS] Appends lines to a file, and then closes the file. If the specified file does not exist, this method creates a file, writes the specified lines to the file, and then closes the file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to append the lines to. The file is created if it doesn't already exist.</param>
      /// <param name="contents">The lines to append to the file.</param>
      /// <remarks>The method creates the file if it doesn’t exist, but it doesn't create new directories. Therefore, the value of the path parameter must contain existing directories.</remarks>
      [SecurityCritical]
      public static void AppendAllLines(KernelTransaction transaction, string path, IEnumerable<string> contents)
      {
         WriteAppendAllLinesInternal(transaction, path, contents, NativeMethods.DefaultFileEncoding, true, false, false);
      }

      /// <summary>[AlphaFS] Appends lines to a file, and then closes the file. If the specified file does not exist, this method creates a file, writes the specified lines to the file, and then closes the file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to append the lines to. The file is created if it doesn't already exist.</param>
      /// <param name="contents">The lines to append to the file.</param>
      /// <param name="encoding">The character <see cref="T:Encoding"/> to use.</param>
      /// <remarks>The method creates the file if it doesn’t exist, but it doesn't create new directories. Therefore, the value of the path parameter must contain existing directories.</remarks>
      [SecurityCritical]
      public static void AppendAllLines(KernelTransaction transaction, string path, IEnumerable<string> contents, Encoding encoding)
      {
         WriteAppendAllLinesInternal(transaction, path, contents, encoding, true, false, false);
      }

      #endregion // .NET

      #region IsFullPath

      /// <summary>[AlphaFS] Appends lines to a file, and then closes the file. If the specified file does not exist, this method creates a file, writes the specified lines to the file, and then closes the file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to append the lines to. The file is created if it doesn't already exist.</param>
      /// <param name="contents">The lines to append to the file.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <remarks>The method creates the file if it doesn’t exist, but it doesn't create new directories. Therefore, the value of the path parameter must contain existing directories.</remarks>
      [SecurityCritical]
      public static void AppendAllLines(KernelTransaction transaction, string path, IEnumerable<string> contents, bool? isFullPath)
      {
         WriteAppendAllLinesInternal(transaction, path, contents, NativeMethods.DefaultFileEncoding, true, false, isFullPath);
      }

      /// <summary>[AlphaFS] Appends lines to a file, and then closes the file. If the specified file does not exist, this method creates a file, writes the specified lines to the file, and then closes the file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to append the lines to. The file is created if it doesn't already exist.</param>
      /// <param name="contents">The lines to append to the file.</param>
      /// <param name="encoding">The character <see cref="T:Encoding"/> to use.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <remarks>The method creates the file if it doesn’t exist, but it doesn't create new directories. Therefore, the value of the path parameter must contain existing directories.</remarks>
      [SecurityCritical]
      public static void AppendAllLines(KernelTransaction transaction, string path, IEnumerable<string> contents, Encoding encoding, bool? isFullPath)
      {
         WriteAppendAllLinesInternal(transaction, path, contents, encoding, true, false, isFullPath);
      }

      #endregion // IsFullPath

      #endregion // Transacted

      #endregion // AlphaFS

      #endregion // AppendAllLines

      #region AppendAllText

      #region .NET

      /// <summary>Appends the specified stringto the file, creating the file if it does not already exist.</summary>
      /// <param name="path">The file to append the specified string to.</param>
      /// <param name="contents">The string to append to the file.</param>
      [SecurityCritical]
      public static void AppendAllText(string path, string contents)
      {
         WriteAppendAllLinesInternal(null, path, new[] { contents }, NativeMethods.DefaultFileEncoding, true, false, false);
      }

      /// <summary>Appends the specified string to the file, creating the file if it does not already exist.</summary>
      /// <param name="path">The file to append the specified string to.</param>
      /// <param name="contents">The string to append to the file.</param>
      /// <param name="encoding">The character <see cref="T:Encoding"/> to use.</param>
      [SecurityCritical]
      public static void AppendAllText(string path, string contents, Encoding encoding)
      {
         WriteAppendAllLinesInternal(null, path, new[] { contents }, encoding, true, false, false);
      }

      #endregion // .NET

      #region AlphaFS

      #region IsFullPath

      /// <summary>[AlphaFS] Appends the specified stringto the file, creating the file if it does not already exist.</summary>
      /// <param name="path">The file to append the specified string to.</param>
      /// <param name="contents">The string to append to the file.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      [SecurityCritical]
      public static void AppendAllText(string path, string contents, bool? isFullPath)
      {
         WriteAppendAllLinesInternal(null, path, new[] { contents }, NativeMethods.DefaultFileEncoding, true, false, isFullPath);
      }

      /// <summary>[AlphaFS] Appends the specified string to the file, creating the file if it does not already exist.</summary>
      /// <param name="path">The file to append the specified string to.</param>
      /// <param name="contents">The string to append to the file.</param>
      /// <param name="encoding">The character <see cref="T:Encoding"/> to use.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      [SecurityCritical]
      public static void AppendAllText(string path, string contents, Encoding encoding, bool? isFullPath)
      {
         WriteAppendAllLinesInternal(null, path, new[] { contents }, encoding, true, false, isFullPath);
      }

      #endregion // IsFullPath

      #region Transacted

      #region .NET

      /// <summary>[AlphaFS] Appends the specified stringto the file, creating the file if it does not already exist.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to append the specified string to.</param>
      /// <param name="contents">The string to append to the file.</param>
      [SecurityCritical]
      public static void AppendAllText(KernelTransaction transaction, string path, string contents)
      {
         WriteAppendAllLinesInternal(transaction, path, new[] { contents }, NativeMethods.DefaultFileEncoding, true, false, false);
      }

      /// <summary>[AlphaFS] Appends the specified string to the file, creating the file if it does not already exist.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to append the specified string to.</param>
      /// <param name="contents">The string to append to the file.</param>
      /// <param name="encoding">The character <see cref="T:Encoding"/> to use.</param>
      [SecurityCritical]
      public static void AppendAllText(KernelTransaction transaction, string path, string contents, Encoding encoding)
      {
         WriteAppendAllLinesInternal(transaction, path, new[] { contents }, encoding, true, false, false);
      }

      #endregion // .NET

      #region IsFullPath

      /// <summary>[AlphaFS] Appends the specified stringto the file, creating the file if it does not already exist.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to append the specified string to.</param>
      /// <param name="contents">The string to append to the file.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      [SecurityCritical]
      public static void AppendAllText(KernelTransaction transaction, string path, string contents, bool? isFullPath)
      {
         WriteAppendAllLinesInternal(transaction, path, new[] { contents }, NativeMethods.DefaultFileEncoding, true, false, isFullPath);
      }

      /// <summary>[AlphaFS] Appends the specified string to the file, creating the file if it does not already exist.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to append the specified string to.</param>
      /// <param name="contents">The string to append to the file.</param>
      /// <param name="encoding">The character <see cref="T:Encoding"/> to use.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      [SecurityCritical]
      public static void AppendAllText(KernelTransaction transaction, string path, string contents, Encoding encoding, bool? isFullPath)
      {
         WriteAppendAllLinesInternal(transaction, path, new[] { contents }, encoding, true, false, isFullPath);
      }

      #endregion // IsFullPath
      
      #endregion // Transacted

      #endregion // AlphaFS

      #endregion // AppendAllText

      #region AppendText

      #region .NET

      /// <summary>Creates a <see cref="T:StreamWriter"/> that appends <see cref="T:NativeMethods.DefaultFileEncoding"/> encoded text to an existing file, or to a new file if the specified file does not exist.</summary>
      /// <param name="path">The path to the file to append to.</param>
      /// <returns>A stream writer that appends <see cref="T:NativeMethods.DefaultFileEncoding"/> encoded text to the specified file or to a new file.</returns>
      [SecurityCritical]
      public static StreamWriter AppendText(string path)
      {
         return AppendTextInternal(null, path, NativeMethods.DefaultFileEncoding, false);
      }

      #endregion // .NET

      #region AlphaFS

      #region IsFullPath

      /// <summary>[AlphaFS] Creates a <see cref="T:StreamWriter"/> that appends <see cref="T:NativeMethods.DefaultFileEncoding"/> encoded text to an existing file, or to a new file if the specified file does not exist.</summary>
      /// <param name="path">The path to the file to append to.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>A stream writer that appends <see cref="T:NativeMethods.DefaultFileEncoding"/> encoded text to the specified file or to a new file.</returns>
      [SecurityCritical]
      public static StreamWriter AppendText(string path, bool? isFullPath)
      {
         return AppendTextInternal(null, path, NativeMethods.DefaultFileEncoding, isFullPath);
      }

      /// <summary>[AlphaFS] Creates a <see cref="T:StreamWriter"/> that appends <see cref="T:NativeMethods.DefaultFileEncoding"/> encoded text to an existing file, or to a new file if the specified file does not exist.</summary>
      /// <param name="path">The path to the file to append to.</param>
      /// <param name="encoding">The character <see cref="T:Encoding"/> to use.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>A stream writer that appends <see cref="T:NativeMethods.DefaultFileEncoding"/> encoded text to the specified file or to a new file.</returns>
      [SecurityCritical]
      public static StreamWriter AppendText(string path, Encoding encoding, bool? isFullPath)
      {
         return AppendTextInternal(null, path, encoding, isFullPath);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Creates a <see cref="T:StreamWriter"/> that appends <see cref="T:NativeMethods.DefaultFileEncoding"/> encoded text to an existing file, or to a new file if the specified file does not exist.</summary>
      /// <param name="path">The path to the file to append to.</param>
      /// <param name="encoding">The character <see cref="T:Encoding"/> to use.</param>
      /// <returns>A stream writer that appends <see cref="T:NativeMethods.DefaultFileEncoding"/> encoded text to the specified file or to a new file.</returns>
      [SecurityCritical]
      public static StreamWriter AppendText(string path, Encoding encoding)
      {
         return AppendTextInternal(null, path, encoding, false);
      }

      #region Transacted

      #region .NET

      /// <summary>[AlphaFS] Creates a <see cref="T:StreamWriter"/> that appends <see cref="T:NativeMethods.DefaultFileEncoding"/> encoded text to an existing file, or to a new file if the specified file does not exist.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to the file to append to.</param>
      /// <returns>A stream writer that appends <see cref="T:NativeMethods.DefaultFileEncoding"/> encoded text to the specified file or to a new file.</returns>
      [SecurityCritical]
      public static StreamWriter AppendText(KernelTransaction transaction, string path)
      {
         return AppendTextInternal(transaction, path, NativeMethods.DefaultFileEncoding, false);
      }

      #endregion // .NET

      #region IsFullPath

      /// <summary>[AlphaFS] Creates a <see cref="T:StreamWriter"/> that appends <see cref="T:NativeMethods.DefaultFileEncoding"/> encoded text to an existing file, or to a new file if the specified file does not exist.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to the file to append to.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>A stream writer that appends <see cref="T:NativeMethods.DefaultFileEncoding"/> encoded text to the specified file or to a new file.</returns>
      [SecurityCritical]
      public static StreamWriter AppendText(KernelTransaction transaction, string path, bool? isFullPath)
      {
         return AppendTextInternal(transaction, path, NativeMethods.DefaultFileEncoding, isFullPath);
      }

      /// <summary>[AlphaFS] Creates a <see cref="T:StreamWriter"/> that appends <see cref="T:NativeMethods.DefaultFileEncoding"/> encoded text to an existing file, or to a new file if the specified file does not exist.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to the file to append to.</param>
      /// <param name="encoding">The character <see cref="T:Encoding"/> to use.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>A stream writer that appends <see cref="T:NativeMethods.DefaultFileEncoding"/> encoded text to the specified file or to a new file.</returns>
      [SecurityCritical]
      public static StreamWriter AppendText(KernelTransaction transaction, string path, Encoding encoding, bool? isFullPath)
      {
         return AppendTextInternal(transaction, path, encoding, isFullPath);
      }

      #endregion // IsFullPath
      
      /// <summary>[AlphaFS] Creates a <see cref="T:StreamWriter"/> that appends <see cref="T:NativeMethods.DefaultFileEncoding"/> encoded text to an existing file, or to a new file if the specified file does not exist.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to the file to append to.</param>
      /// <param name="encoding">The character <see cref="T:Encoding"/> to use.</param>
      /// <returns>A stream writer that appends <see cref="T:NativeMethods.DefaultFileEncoding"/> encoded text to the specified file or to a new file.</returns>
      [SecurityCritical]
      public static StreamWriter AppendText(KernelTransaction transaction, string path, Encoding encoding)
      {
         return AppendTextInternal(transaction, path, encoding, false);
      }

      #endregion // Transacted

      #endregion // AlphaFS

      #endregion // AppendText

      #region Copy

      #region .NET

      /// <summary>Copies an existing file to a new file. Overwriting a file of the same name is not allowed.</summary>
      /// <param name="sourceFileName">The file to copy.</param>
      /// <param name="destFileName">The name of the destination file. This cannot be a directory or an existing file.</param>
      /// <remarks>The attributes of the original file are retained in the copied file.</remarks>
      /// <remarks>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method. If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "dest")]
      [SecurityCritical]
      public static void Copy(string sourceFileName, string destFileName)
      {
         CopyMoveInternal(false, null, sourceFileName, destFileName, false, CopyOptions.FailIfExists, null, null, null, false);
      }

      /// <summary>Copies an existing file to a new file. Overwriting a file of the same name is allowed.</summary>
      /// <param name="sourceFileName">The file to copy.</param>
      /// <param name="destFileName">The name of the destination file. This cannot be a directory.</param>
      /// <param name="overwrite"><c>true</c> if the destination file should be overwritten; <c>false</c> otherwise.</param>
      /// <remarks>The attributes of the original file are retained in the copied file.</remarks>
      /// <remarks>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method. If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "dest")]
      [SecurityCritical]
      public static void Copy(string sourceFileName, string destFileName, bool overwrite)
      {
         CopyMoveInternal(false, null, sourceFileName, destFileName, false, overwrite ? CopyOptions.None : CopyOptions.FailIfExists, null, null, null, false);
      }

      #endregion // .NET

      #region AlphaFS

      #region IsFullPath

      /// <summary>[AlphaFS] Copies an existing file to a new file.</summary>
      /// <param name="sourceFileName">The name of an existing file.</param>
      /// <param name="destFileName">The name of the new file.</param>
      /// <param name="overwrite"><c>true</c> if the destination file should be overwritten; <c>false</c> otherwise.</param>
      /// <param name="preserveDates"><c>true</c> if original Timestamps must be preserved, <c>false</c> otherwise.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="sourceFileName"/> and <paramref name="destFileName"/> will be normalized and long path prefixed. <c>null</c> <paramref name="sourceFileName"/> and <paramref name="destFileName"/> are already a full path with long path prefix, will be used as is.</param>
      /// <remarks>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method. If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "dest")]
      [SecurityCritical]
      public static void Copy(string sourceFileName, string destFileName, bool overwrite, bool preserveDates, bool? isFullPath)
      {
         CopyMoveInternal(false, null, sourceFileName, destFileName, preserveDates, overwrite ? CopyOptions.None : CopyOptions.FailIfExists, null, null, null, isFullPath);
      }

      /// <summary>[AlphaFS] Copies an existing file to a new file, notifying the application of its progress through a callback function.</summary>
      /// <param name="sourceFileName">The name of an existing file.</param>
      /// <param name="destFileName">The name of the new file.</param>
      /// <param name="overwrite"><c>true</c> if the destination file should be overwritten; <c>false</c> otherwise.</param>
      /// <param name="preserveDates"><c>true</c> if original Timestamps must be preserved, <c>false</c> otherwise.</param>
      /// <param name="copyProgress">A callback function that is called each time another portion of the file has been copied. This parameter can be <c>null</c>.</param>
      /// <param name="userProgressData">The argument to be passed to the callback function. This parameter can be <c>null</c>.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="sourceFileName"/> and <paramref name="destFileName"/> will be normalized and long path prefixed. <c>null</c> <paramref name="sourceFileName"/> and <paramref name="destFileName"/> are already a full path with long path prefix, will be used as is.</param>
      /// <remarks>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method. If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "dest")]
      [SecurityCritical]
      public static void Copy(string sourceFileName, string destFileName, bool overwrite, bool preserveDates, CopyMoveProgressCallback copyProgress, object userProgressData, bool? isFullPath)
      {
         CopyMoveInternal(false, null, sourceFileName, destFileName, preserveDates, overwrite ? CopyOptions.None : CopyOptions.FailIfExists, null, copyProgress, userProgressData, isFullPath);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Copies an existing file to a new file, notifying the application of its progress through a callback function.</summary>
      /// <param name="sourceFileName">The name of an existing file.</param>
      /// <param name="destFileName">The name of the new file.</param>
      /// <param name="overwrite"><c>true</c> if the destination file should be overwritten; <c>false</c> otherwise.</param>
      /// <param name="preserveDates"><c>true</c> if original Timestamps must be preserved, <c>false</c> otherwise.</param>
      /// <param name="copyProgress">A callback function that is called each time another portion of the file has been copied. This parameter can be <c>null</c>.</param>
      /// <param name="userProgressData">The argument to be passed to the callback function. This parameter can be <c>null</c>.</param>
      /// <remarks>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method. If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "dest")]
      [SecurityCritical]
      public static void Copy(string sourceFileName, string destFileName, bool overwrite, bool preserveDates, CopyMoveProgressCallback copyProgress, object userProgressData)
      {
         CopyMoveInternal(false, null, sourceFileName, destFileName, preserveDates, overwrite ? CopyOptions.None : CopyOptions.FailIfExists, null, copyProgress, userProgressData, false);
      }

      #region Transacted

      #region .NET

      /// <summary>[AlphaFS] Copies an existing file to a new file. Overwriting a file of the same name is not allowed.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourceFileName">The file to copy.</param>
      /// <param name="destFileName">The name of the destination file. This cannot be a directory or an existing file.</param>
      /// <remarks>The attributes of the original file are retained in the copied file.</remarks>
      /// <remarks>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method. If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "dest")]
      [SecurityCritical]
      public static void Copy(KernelTransaction transaction, string sourceFileName, string destFileName)
      {
         CopyMoveInternal(false, transaction, sourceFileName, destFileName, false, CopyOptions.FailIfExists, null, null, null, false);
      }

      /// <summary>[AlphaFS] Copies an existing file to a new file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourceFileName">The file to copy.</param>
      /// <param name="destFileName">The name of the destination file. This cannot be a directory.</param>
      /// <param name="overwrite"><c>true</c> if the destination file should be overwritten; <c>false</c> otherwise.</param>
      /// <remarks>The attributes of the original file are retained in the copied file.</remarks>
      /// <remarks>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method. If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "dest")]
      [SecurityCritical]
      public static void Copy(KernelTransaction transaction, string sourceFileName, string destFileName, bool overwrite)
      {
         CopyMoveInternal(false, transaction, sourceFileName, destFileName, false, overwrite ? CopyOptions.None : CopyOptions.FailIfExists, null, null, null, false);
      }

      #endregion // .NET

      #region IsFullPath

      /// <summary>[AlphaFS] Copies an existing file to a new file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourceFileName">The name of an existing file.</param>
      /// <param name="destFileName">The name of the new file.</param>
      /// <param name="overwrite"><c>true</c> if the destination file should be overwritten; <c>false</c> otherwise.</param>
      /// <param name="preserveDates"><c>true</c> if original Timestamps must be preserved, <c>false</c> otherwise.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="sourceFileName"/> and <paramref name="destFileName"/> will be normalized and long path prefixed. <c>null</c> <paramref name="sourceFileName"/> and <paramref name="destFileName"/> are already a full path with long path prefix, will be used as is.</param>
      /// <remarks>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method. If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "dest")]
      [SecurityCritical]
      public static void Copy(KernelTransaction transaction, string sourceFileName, string destFileName, bool overwrite, bool preserveDates, bool? isFullPath)
      {
         CopyMoveInternal(false, transaction, sourceFileName, destFileName, preserveDates, overwrite ? CopyOptions.None : CopyOptions.FailIfExists, null, null, null, isFullPath);
      }

      /// <summary>[AlphaFS] Copies an existing file to a new file, notifying the application of its progress through a callback function.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourceFileName">The name of an existing file.</param>
      /// <param name="destFileName">The name of the new file.</param>
      /// <param name="overwrite"><c>true</c> if the destination file should be overwritten; <c>false</c> otherwise.</param>
      /// <param name="preserveDates"><c>true</c> if original Timestamps must be preserved, <c>false</c> otherwise.</param>
      /// <param name="copyProgress">A callback function that is called each time another portion of the file has been copied. This parameter can be <c>null</c>.</param>
      /// <param name="userProgressData">The argument to be passed to the callback function. This parameter can be <c>null</c>.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="sourceFileName"/> and <paramref name="destFileName"/> will be normalized and long path prefixed. <c>null</c> <paramref name="sourceFileName"/> and <paramref name="destFileName"/> are already a full path with long path prefix, will be used as is.</param>
      /// <returns><c>true</c> if the file was completely copied, or <c>false</c> if the copy operation was aborted/failed.</returns>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "dest")]
      [SecurityCritical]
      public static void Copy(KernelTransaction transaction, string sourceFileName, string destFileName, bool overwrite, bool preserveDates, CopyMoveProgressCallback copyProgress, object userProgressData, bool? isFullPath)
      {
         CopyMoveInternal(false, transaction, sourceFileName, destFileName, preserveDates, overwrite ? CopyOptions.None : CopyOptions.FailIfExists, null, copyProgress, userProgressData, isFullPath);
      }

      #endregion // IsFullPath
      
      /// <summary>[AlphaFS] Copies an existing file to a new file, notifying the application of its progress through a callback function.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourceFileName">The name of an existing file.</param>
      /// <param name="destFileName">The name of the new file.</param>
      /// <param name="overwrite"><c>true</c> if the destination file should be overwritten; <c>false</c> otherwise.</param>
      /// <param name="preserveDates"><c>true</c> if original Timestamps must be preserved, <c>false</c> otherwise.</param>
      /// <param name="copyProgress">A callback function that is called each time another portion of the file has been copied. This parameter can be <c>null</c>.</param>
      /// <param name="userProgressData">The argument to be passed to the callback function. This parameter can be <c>null</c>.</param>
      /// <returns><c>true</c> if the file was completely copied, or <c>false</c> if the copy operation was aborted/failed.</returns>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "dest")]
      [SecurityCritical]
      public static void Copy(KernelTransaction transaction, string sourceFileName, string destFileName,  bool overwrite, bool preserveDates, CopyMoveProgressCallback copyProgress, object userProgressData)
      {
         CopyMoveInternal(false, transaction, sourceFileName, destFileName, preserveDates, overwrite ? CopyOptions.None : CopyOptions.FailIfExists, null, copyProgress, userProgressData, false);
      }

      #endregion // Transacted

      #endregion // AlphaFS

      #endregion // Copy

      #region Create

      #region .NET

      /// <summary>Creates or overwrites a file in the specified path.</summary>
      /// <param name="path">The path and name of the file to create.</param>
      /// <returns>A <see cref="T:FileStream"/> that provides read/write access to the file specified in <paramref name="path"/>.</returns>
      [SecurityCritical]
      public static FileStream Create(string path)
      {
         return CreateFileInternal(null, path, NativeMethods.DefaultFileBufferSize, EFileAttributes.None, null, FileMode.Create, FileAccess.ReadWrite, FileShare.None, false);
      }

      /// <summary>Creates or overwrites the specified file.</summary>
      /// <param name="path">The name of the file.</param>
      /// <param name="bufferSize">The number of bytes buffered for reads and writes to the file.</param>
      /// <returns>A <see cref="T:FileStream"/> with the specified buffer size that provides read/write access to the file specified in <paramref name="path"/>.</returns>
      [SecurityCritical]
      public static FileStream Create(string path, int bufferSize)
      {
         return CreateFileInternal(null, path, bufferSize, EFileAttributes.None, null, FileMode.Create, FileAccess.ReadWrite, FileShare.None, false);
      }

      /// <summary>Creates or overwrites the specified file, specifying a buffer size and a <see cref="T:FileOptions"/> value that describes how to create or overwrite the file.</summary>
      /// <param name="path">The name of the file.</param>
      /// <param name="bufferSize">The number of bytes buffered for reads and writes to the file.</param>
      /// <param name="options">One of the <see cref="T:FileOptions"/> values that describes how to create or overwrite the file.</param>
      /// <returns>A new file with the specified buffer size.</returns>
      [SecurityCritical]
      public static FileStream Create(string path, int bufferSize, FileOptions options)
      {
         return CreateFileInternal(null, path, bufferSize, (EFileAttributes) options, null, FileMode.Create, FileAccess.ReadWrite, FileShare.None, false);
      }

      /// <summary>Creates or overwrites the specified file, specifying a buffer size and a <see cref="T:FileOptions"/> value that describes how to create or overwrite the file.</summary>
      /// <param name="path">The name of the file.</param>
      /// <param name="bufferSize">The number of bytes buffered for reads and writes to the file.</param>
      /// <param name="options">One of the <see cref="T:FileOptions"/> values that describes how to create or overwrite the file.</param>
      /// <param name="fileSecurity">One of the <see cref="T:FileSecurity"/> values that determines the access control and audit security for the file.</param>
      /// <returns>A new file with the specified buffer size, file options, and file security.</returns>
      [SecurityCritical]
      public static FileStream Create(string path, int bufferSize, FileOptions options, FileSecurity fileSecurity)
      {
         return CreateFileInternal(null, path, bufferSize, (EFileAttributes) options, fileSecurity, FileMode.Create, FileAccess.ReadWrite, FileShare.None, false);
      }

      #endregion // .NET

      #region AlphaFS

      #region IsFullPath

      /// <summary>[AlphaFS] Creates or overwrites a file in the specified path.</summary>
      /// <param name="path">The path and name of the file to create.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>A <see cref="T:FileStream"/> that provides read/write access to the file specified in <paramref name="path"/>.</returns>
      [SecurityCritical]
      public static FileStream Create(string path, bool? isFullPath)
      {
         return CreateFileInternal(null, path, NativeMethods.DefaultFileBufferSize, EFileAttributes.None, null, FileMode.Create, FileAccess.ReadWrite, FileShare.None, isFullPath);
      }

      /// <summary>[AlphaFS] Creates or overwrites the specified file.</summary>
      /// <param name="path">The name of the file.</param>
      /// <param name="bufferSize">The number of bytes buffered for reads and writes to the file.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>A <see cref="T:FileStream"/> with the specified buffer size that provides read/write access to the file specified in <paramref name="path"/>.</returns>
      [SecurityCritical]
      public static FileStream Create(string path, int bufferSize, bool? isFullPath)
      {
         return CreateFileInternal(null, path, bufferSize, EFileAttributes.None, null, FileMode.Create, FileAccess.ReadWrite, FileShare.None, isFullPath);
      }

      /// <summary>[AlphaFS] Creates or overwrites the specified file, specifying a buffer size and a <see cref="T:FileOptions"/> value that describes how to create or overwrite the file.</summary>
      /// <param name="path">The name of the file.</param>
      /// <param name="bufferSize">The number of bytes buffered for reads and writes to the file.</param>
      /// <param name="options">One of the <see cref="T:FileOptions"/> values that describes how to create or overwrite the file.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>A new file with the specified buffer size.</returns>
      [SecurityCritical]
      public static FileStream Create(string path, int bufferSize, FileOptions options, bool? isFullPath)
      {
         return CreateFileInternal(null, path, bufferSize, (EFileAttributes) options, null, FileMode.Create, FileAccess.ReadWrite, FileShare.None, isFullPath);
      }

      /// <summary>[AlphaFS] Creates or overwrites the specified file, specifying a buffer size and a <see cref="T:FileOptions"/> value that describes how to create or overwrite the file.</summary>
      /// <param name="path">The name of the file.</param>
      /// <param name="bufferSize">The number of bytes buffered for reads and writes to the file.</param>
      /// <param name="options">One of the <see cref="T:FileOptions"/> values that describes how to create or overwrite the file.</param>
      /// <param name="fileSecurity">One of the <see cref="T:FileSecurity"/> values that determines the access control and audit security for the file.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>A new file with the specified buffer size, file options, and file security.</returns>
      [SecurityCritical]
      public static FileStream Create(string path, int bufferSize, FileOptions options, FileSecurity fileSecurity, bool? isFullPath)
      {
         return CreateFileInternal(null, path, bufferSize, (EFileAttributes) options, fileSecurity, FileMode.Create, FileAccess.ReadWrite, FileShare.None, isFullPath);
      }

      #endregion // IsFullPath

      #region Transacted

      #region .NET

      /// <summary>[AlphaFS] Creates or overwrites a file in the specified path.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path and name of the file to create.</param>
      /// <returns>A <see cref="T:FileStream"/> that provides read/write access to the file specified in <paramref name="path"/>.</returns>
      [SecurityCritical]
      public static FileStream Create(KernelTransaction transaction, string path)
      {
         return CreateFileInternal(transaction, path, NativeMethods.DefaultFileBufferSize, EFileAttributes.None, null, FileMode.Create, FileAccess.ReadWrite, FileShare.None, false);
      }

      /// <summary>[AlphaFS] Creates or overwrites the specified file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The name of the file.</param>
      /// <param name="bufferSize">The number of bytes buffered for reads and writes to the file.</param>
      /// <returns>A <see cref="T:FileStream"/> with the specified buffer size that provides read/write access to the file specified in <paramref name="path"/>.</returns>
      [SecurityCritical]
      public static FileStream Create(KernelTransaction transaction, string path, int bufferSize)
      {
         return CreateFileInternal(transaction, path, bufferSize, EFileAttributes.None, null, FileMode.Create, FileAccess.ReadWrite, FileShare.None, false);
      }

      /// <summary>[AlphaFS] Creates or overwrites the specified file, specifying a buffer size and a <see cref="T:FileOptions"/> value that describes how to create or overwrite the file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The name of the file.</param>
      /// <param name="bufferSize">The number of bytes buffered for reads and writes to the file.</param>
      /// <param name="options">One of the <see cref="T:FileOptions"/> values that describes how to create or overwrite the file.</param>
      /// <returns>A new file with the specified buffer size.</returns>
      [SecurityCritical]
      public static FileStream Create(KernelTransaction transaction, string path, int bufferSize, FileOptions options)
      {
         return CreateFileInternal(transaction, path, bufferSize, (EFileAttributes)options, null, FileMode.Create, FileAccess.ReadWrite, FileShare.None, false);
      }

      /// <summary>[AlphaFS] Creates or overwrites the specified file, specifying a buffer size and a <see cref="T:FileOptions"/> value that describes how to create or overwrite the file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The name of the file.</param>
      /// <param name="bufferSize">The number of bytes buffered for reads and writes to the file.</param>
      /// <param name="options">One of the <see cref="T:FileOptions"/> values that describes how to create or overwrite the file.</param>
      /// <param name="fileSecurity">One of the <see cref="T:FileSecurity"/> values that determines the access control and audit security for the file.</param>
      /// <returns>A new file with the specified buffer size, file options, and file security.</returns>
      [SecurityCritical]
      public static FileStream Create(KernelTransaction transaction, string path, int bufferSize, FileOptions options, FileSecurity fileSecurity)
      {
         return CreateFileInternal(transaction, path, bufferSize, (EFileAttributes)options, fileSecurity, FileMode.Create, FileAccess.ReadWrite, FileShare.None, false);
      }

      #endregion // .NET

      #region IsFullPath

      /// <summary>[AlphaFS] Creates or overwrites a file in the specified path.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path and name of the file to create.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>A <see cref="T:FileStream"/> that provides read/write access to the file specified in <paramref name="path"/>.</returns>
      [SecurityCritical]
      public static FileStream Create(KernelTransaction transaction, string path, bool? isFullPath)
      {
         return CreateFileInternal(transaction, path, NativeMethods.DefaultFileBufferSize, EFileAttributes.None, null, FileMode.Create, FileAccess.ReadWrite, FileShare.None, isFullPath);
      }

      /// <summary>[AlphaFS] Creates or overwrites the specified file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The name of the file.</param>
      /// <param name="bufferSize">The number of bytes buffered for reads and writes to the file.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>A <see cref="T:FileStream"/> with the specified buffer size that provides read/write access to the file specified in <paramref name="path"/>.</returns>
      [SecurityCritical]
      public static FileStream Create(KernelTransaction transaction, string path, int bufferSize, bool? isFullPath)
      {
         return CreateFileInternal(transaction, path, bufferSize, EFileAttributes.None, null, FileMode.Create, FileAccess.ReadWrite, FileShare.None, isFullPath);
      }

      /// <summary>[AlphaFS] Creates or overwrites the specified file, specifying a buffer size and a <see cref="T:FileOptions"/> value that describes how to create or overwrite the file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The name of the file.</param>
      /// <param name="bufferSize">The number of bytes buffered for reads and writes to the file.</param>
      /// <param name="options">One of the <see cref="T:FileOptions"/> values that describes how to create or overwrite the file.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>A new file with the specified buffer size.</returns>
      [SecurityCritical]
      public static FileStream Create(KernelTransaction transaction, string path, int bufferSize, FileOptions options, bool? isFullPath)
      {
         return CreateFileInternal(transaction, path, bufferSize, (EFileAttributes)options, null, FileMode.Create, FileAccess.ReadWrite, FileShare.None, isFullPath);
      }
      
      /// <summary>[AlphaFS] Creates or overwrites the specified file, specifying a buffer size and a <see cref="T:FileOptions"/> value that describes how to create or overwrite the file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The name of the file.</param>
      /// <param name="bufferSize">The number of bytes buffered for reads and writes to the file.</param>
      /// <param name="options">One of the <see cref="T:FileOptions"/> values that describes how to create or overwrite the file.</param>
      /// <param name="fileSecurity">One of the <see cref="T:FileSecurity"/> values that determines the access control and audit security for the file.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>A new file with the specified buffer size, file options, and file security.</returns>
      [SecurityCritical]
      public static FileStream Create(KernelTransaction transaction, string path, int bufferSize, FileOptions options, FileSecurity fileSecurity, bool? isFullPath)
      {
         return CreateFileInternal(transaction, path, bufferSize, (EFileAttributes)options, fileSecurity, FileMode.Create, FileAccess.ReadWrite, FileShare.None, isFullPath);
      }

      #endregion // IsFullPath
      
      #endregion // Transacted

      #endregion // AlphaFS

      #endregion // Create

      #region CreateText

      #region .NET

      /// <summary>Creates or opens a file for writing UTF-8 encoded text.</summary>
      /// <param name="path">The file to be opened for writing.</param>
      /// <returns>A StreamWriter that writes to the specified file using UTF-8 encoding.</returns>
      [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
      [SecurityCritical]
      public static StreamWriter CreateText(string path)
      {
         return CreateTextInternal(null, path, NativeMethods.DefaultFileEncoding, false);
      }

      #endregion // .NET

      #region AlphaFS

      #region IsFullPath

      /// <summary>[AlphaFS] Creates or opens a file for writing UTF-8 encoded text.</summary>
      /// <param name="path">The file to be opened for writing.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>A StreamWriter that writes to the specified file using UTF-8 encoding.</returns>
      [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
      [SecurityCritical]
      public static StreamWriter CreateText(string path, bool? isFullPath)
      {
         return CreateTextInternal(null, path, NativeMethods.DefaultFileEncoding, isFullPath);
      }

      /// <summary>[AlphaFS] Creates or opens a file for writing UTF-8 encoded text.</summary>
      /// <param name="path">The file to be opened for writing.</param>
      /// <param name="encoding">The encoding that is applied to the contents of the file.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>A StreamWriter that writes to the specified file using UTF-8 encoding.</returns>
      [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
      [SecurityCritical]
      public static StreamWriter CreateText(string path, Encoding encoding, bool? isFullPath)
      {
         return CreateTextInternal(null, path, encoding, isFullPath);
      }

      #endregion // IsFullPath

      #region Transacted

      #region .NET

      /// <summary>[AlphaFS] Creates or opens a file for writing UTF-8 encoded text.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to be opened for writing.</param>
      /// <returns>A StreamWriter that writes to the specified file using UTF-8 encoding.</returns>
      [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
      [SecurityCritical]
      public static StreamWriter CreateText(KernelTransaction transaction, string path)
      {
         return CreateTextInternal(transaction, path, NativeMethods.DefaultFileEncoding, false);
      }

      #endregion // .NET

      #region IsFullPath

      /// <summary>[AlphaFS] Creates or opens a file for writing UTF-8 encoded text.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to be opened for writing.</param>
      /// <param name="encoding">The encoding that is applied to the contents of the file.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>A StreamWriter that writes to the specified file using UTF-8 encoding.</returns>
      [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
      [SecurityCritical]
      public static StreamWriter CreateText(KernelTransaction transaction, string path, Encoding encoding, bool? isFullPath)
      {
         return CreateTextInternal(transaction, path, encoding, isFullPath);
      }

      #endregion // IsFullPath
      
      #endregion // Transacted

      #endregion // AlphaFS

      #endregion // CreateText

      #region Decrypt

      #region .NET

      /// <summary>Decrypts a file that was encrypted by the current account using the Encrypt method.</summary>
      /// <param name="path">A path that describes a file to decrypt.</param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static void Decrypt(string path)
      {
         EncryptDecryptFileInternal(path, false, false);
      }

      #endregion // .NET

      #region AlphaFS

      #region IsFullPath

      /// <summary>[AlphaFS] Decrypts a file that was encrypted by the current account using the Encrypt method.</summary>
      /// <param name="path">A path that describes a file to decrypt.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static void Decrypt(string path, bool? isFullPath)
      {
         EncryptDecryptFileInternal(path, false, isFullPath);
      }

      #endregion // IsFullPath

      #endregion // AlphaFS

      #endregion // Decrypt

      #region Delete

      #region .NET

      /// <summary>Deletes the specified file.</summary>
      /// <param name="path">The name of the file to be deleted. Wildcard characters are not supported.</param>
      /// <remarks>If the file to be deleted does not exist, no exception is thrown.</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static void Delete(string path)
      {
         DeleteFileInternal(null, path, false, false);
      }

      #endregion // .NET

      #region AlphaFS

      #region IsFullPath

      /// <summary>[AlphaFS] Deletes the specified file.</summary>
      /// <param name="path">The name of the file to be deleted. Wildcard characters are not supported.</param>
      /// <param name="ignoreReadOnly"><c>true</c> overrides the read only <see cref="T:FileAttributes"/> of the file.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <remarks>If the file to be deleted does not exist, no exception is thrown.</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static void Delete(string path, bool ignoreReadOnly, bool? isFullPath)
      {
         DeleteFileInternal(null, path, ignoreReadOnly, isFullPath);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Deletes the specified file.</summary>
      /// <param name="path">The name of the file to be deleted. Wildcard characters are not supported.</param>
      /// <param name="ignoreReadOnly"><c>true</c> overrides the read only <see cref="T:FileAttributes"/> of the file.</param>
      /// <remarks>If the file to be deleted does not exist, no exception is thrown.</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static void Delete(string path, bool ignoreReadOnly)
      {
         DeleteFileInternal(null, path, ignoreReadOnly, false);
      }

      #region Transacted

      #region .NET

      /// <summary>[AlphaFS] Deletes the specified file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The name of the file to be deleted. Wildcard characters are not supported.</param>
      /// <remarks>If the file to be deleted does not exist, no exception is thrown.</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static void Delete(KernelTransaction transaction, string path)
      {
         DeleteFileInternal(transaction, path, false, false);
      }

      #endregion // .NET

      #region IsFullPath

      /// <summary>[AlphaFS] Deletes the specified file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The name of the file to be deleted. Wildcard characters are not supported.</param>
      /// <param name="ignoreReadOnly"><c>true</c> overrides the read only <see cref="T:FileAttributes"/> of the file.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <remarks>If the file to be deleted does not exist, no exception is thrown.</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static void Delete(KernelTransaction transaction, string path, bool ignoreReadOnly, bool? isFullPath)
      {
         DeleteFileInternal(transaction, path, ignoreReadOnly, isFullPath);
      }

      #endregion // IsFullPath
      
      /// <summary>[AlphaFS] Deletes the specified file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The name of the file to be deleted. Wildcard characters are not supported.</param>
      /// <param name="ignoreReadOnly"><c>true</c> overrides the read only <see cref="T:FileAttributes"/> of the file.</param>
      /// <remarks>If the file to be deleted does not exist, no exception is thrown.</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static void Delete(KernelTransaction transaction, string path, bool ignoreReadOnly)
      {
         DeleteFileInternal(transaction, path, ignoreReadOnly, false);
      }

      #endregion // Transacted

      #endregion // AlphaFS

      #endregion // Delete

      #region Encrypt

      #region .NET

      /// <summary>Encrypts a file so that only the account used to encrypt the file can decrypt it.</summary>
      /// <param name="path">A path that describes a file to encrypt.</param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static void Encrypt(string path)
      {
         EncryptDecryptFileInternal(path, true, false);
      }

      #endregion // .NET

      #region AlphaFS

      #region IsFullPath

      /// <summary>[AlphaFS] Encrypts a file so that only the account used to encrypt the file can decrypt it.</summary>
      /// <param name="path">A path that describes a file to encrypt.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static void Encrypt(string path, bool? isFullPath)
      {
         EncryptDecryptFileInternal(path, true, isFullPath);
      }

      #endregion // IsFullPath

      #endregion // AlphaFS

      #endregion // Encrypt

      #region Exists

      #region .NET

      /// <summary>Determines whether the specified file exists.</summary>
      /// <param name="path">The file to check. Note that this files may contain wildcards, such as '*'.</param>
      /// <returns><c>true</c> on success, <c>false</c> otherwise.</returns>
      /// <remarks>Note that this files may contain wildcards, such as '*'</remarks>
      /// <remarks>Trailing spaces are removed from the <paramref name="path"/> parameter before determining if the file exists.</remarks>
      /// <remarks><c>true</c> if the caller has the required permissions and path contains the name of an existing file; <c>false</c> otherwise.</remarks>
      /// <remarks>This method also returns <c>false</c> if path is a null reference, an invalid path, or a zero-length string.</remarks>
      /// <remarks>If the caller does not have sufficient permissions to read the specified file, no exception is thrown and the method returns <c>false</c> regardless of the existence of path.</remarks>
      [SecurityCritical]
      public static bool Exists(string path)
      {
         return ExistsInternal(false, null, path, false);
      }

      #endregion // .NET

      #region AlphaFS

      #region IsFullPath

      /// <summary>[AlphaFS] Determines whether the specified file exists.</summary>
      /// <param name="path">The file to check. Note that this files may contain wildcards, such as '*'.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns><c>true</c> on success, <c>false</c> otherwise.</returns>
      /// <remarks>Note that this files may contain wildcards, such as '*'</remarks>
      /// <remarks>If <paramref name="isFullPath"/> is <c>false></c> trailing spaces are removed from the <paramref name="path"/> parameter before determining if the file exists.</remarks>
      /// <remarks><c>true</c> if the caller has the required permissions and path contains the name of an existing file; <c>false</c> otherwise.</remarks>
      /// <remarks>This method also returns <c>false</c> if path is a null reference, an invalid path, or a zero-length string.</remarks>
      /// <remarks>If the caller does not have sufficient permissions to read the specified file, no exception is thrown and the method returns <c>false</c> regardless of the existence of path.</remarks>
      [SecurityCritical]
      public static bool Exists(string path, bool? isFullPath)
      {
         return ExistsInternal(false, null, path, isFullPath);
      }

      #endregion // IsFullPath

      #region Transacted

      #region .NET

      /// <summary>[AlphaFS] Determines whether the specified file exists.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to check. Note that this files may contain wildcards, such as '*'.</param>
      /// <returns><c>true</c> on success, <c>false</c> otherwise.</returns>
      /// <remarks>Note that this files may contain wildcards, such as '*'</remarks>
      /// <remarks>Trailing spaces are removed from the <paramref name="path"/> parameter before determining if the file exists.</remarks>
      /// <remarks>Return value is <c>true</c> if the caller has the required permissions and path contains the name of an existing file; <c>false</c> otherwise.</remarks>
      /// <remarks>This method also returns <c>false</c> if path is a null reference, an invalid path, or a zero-length string.</remarks>
      /// <remarks>If the caller does not have sufficient permissions to read the specified file, no exception is thrown and the method returns <c>false</c> regardless of the existence of path.</remarks>
      /// <remarks>A trailing backslash is not allowed and will be removed.</remarks>
      [SecurityCritical]
      public static bool Exists(KernelTransaction transaction, string path)
      {
         return ExistsInternal(false, transaction, path, false);
      }

      #endregion // .NET

      #region IsFullPath

      /// <summary>[AlphaFS] Determines whether the specified file exists.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to check. Note that this files may contain wildcards, such as '*'.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns><c>true</c> on success, <c>false</c> otherwise.</returns>
      /// <remarks>Note that this files may contain wildcards, such as '*'</remarks>
      /// <remarks>If <paramref name="isFullPath"/> is <c>false></c> trailing spaces are removed from the <paramref name="path"/> parameter before determining if the file exists.</remarks>
      /// <remarks>Return value is <c>true</c> if the caller has the required permissions and path contains the name of an existing file; <c>false</c> otherwise.</remarks>
      /// <remarks>This method also returns <c>false</c> if path is a null reference, an invalid path, or a zero-length string.</remarks>
      /// <remarks>If the caller does not have sufficient permissions to read the specified file, no exception is thrown and the method returns <c>false</c> regardless of the existence of path.</remarks>
      /// <remarks>A trailing backslash is not allowed and will be removed.</remarks>
      [SecurityCritical]
      public static bool Exists(KernelTransaction transaction, string path, bool? isFullPath)
      {
         return ExistsInternal(false, transaction, path, isFullPath);
      }

      #endregion // IsFullPath
      
      #endregion // Transacted

      #endregion // AlphaFS

      #endregion // Exists

      #region GetAccessControl

      #region .NET

      /// <summary>Gets a <see cref="T:FileSecurity"/> object that encapsulates the access control list (ACL) entries for a specified file.</summary>
      /// <param name="path">The path to a file containing a <see cref="T:FileSecurity"/> object that describes the file's access control list (ACL) information.</param>
      /// <returns>A <see cref="T:FileSecurity"/> object that encapsulates the access control rules for the file described by the <paramref name="path"/> parameter.</returns>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static FileSecurity GetAccessControl(string path)
      {
         return GetAccessControlInternal<FileSecurity>(false, path, AccessControlSections.Access | AccessControlSections.Group | AccessControlSections.Owner, false);
      }

      /// <summary>Gets a <see cref="T:FileSecurity"/> object that encapsulates the access control list (ACL) entries for a specified file.</summary>
      /// <param name="path">The path to a file containing a <see cref="T:FileSecurity"/> object that describes the file's access control list (ACL) information.</param>
      /// <param name="includeSections">One (or more) of the <see cref="T:AccessControlSections"/> values that specifies the type of access control list (ACL) information to receive.</param>
      /// <returns>A <see cref="T:FileSecurity"/> object that encapsulates the access control rules for the file described by the <paramref name="path"/> parameter.</returns>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static FileSecurity GetAccessControl(string path, AccessControlSections includeSections)
      {
         return GetAccessControlInternal<FileSecurity>(false, path, includeSections, false);
      }

      #endregion // .NET

      #region AlphaFS

      #region IsFullPath

      /// <summary>[AlphaFS] Gets a <see cref="T:FileSecurity"/> object that encapsulates the access control list (ACL) entries for a specified file.</summary>
      /// <param name="path">The path to a file containing a <see cref="T:FileSecurity"/> object that describes the file's access control list (ACL) information.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>A <see cref="T:FileSecurity"/> object that encapsulates the access control rules for the file described by the <paramref name="path"/> parameter.</returns>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static FileSecurity GetAccessControl(string path, bool? isFullPath)
      {
         return GetAccessControlInternal<FileSecurity>(false, path, AccessControlSections.Access | AccessControlSections.Group | AccessControlSections.Owner, isFullPath);
      }

      /// <summary>[AlphaFS] Gets a <see cref="T:FileSecurity"/> object that encapsulates the access control list (ACL) entries for a specified file.</summary>
      /// <param name="path">The path to a file containing a <see cref="T:FileSecurity"/> object that describes the file's access control list (ACL) information.</param>
      /// <param name="includeSections">One (or more) of the <see cref="T:AccessControlSections"/> values that specifies the type of access control list (ACL) information to receive.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>A <see cref="T:FileSecurity"/> object that encapsulates the access control rules for the file described by the <paramref name="path"/> parameter.</returns>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static FileSecurity GetAccessControl(string path, AccessControlSections includeSections, bool? isFullPath)
      {
         return GetAccessControlInternal<FileSecurity>(false, path, includeSections, isFullPath);
      }

      #endregion // IsFullPath

      #endregion // AlphaFS

      #endregion // GetAccessControl

      #region GetAttributes

      #region .NET

      /// <summary>Gets the <see cref="T:FileAttributes"/> of the file on the path.</summary>
      /// <param name="path">The path to the file.</param>
      /// <returns>The <see cref="T:FileAttributes"/> of the file on the path.</returns>
      [SecurityCritical]
      public static FileAttributes GetAttributes(string path)
      {
         return GetAttributesInternal(false, null, path, true, false, false);
      }

      #endregion // .NET

      #region AlphaFS

      #region IsFullPath

      /// <summary>[AlphaFS] Gets the <see cref="T:FileAttributes"/> of the file on the path.</summary>
      /// <param name="path">The path to the file.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>The <see cref="T:FileAttributes"/> of the file on the path.</returns>
      [SecurityCritical]
      public static FileAttributes GetAttributes(string path, bool? isFullPath)
      {
         return GetAttributesInternal(false, null, path, true, false, isFullPath);
      }

      #endregion // IsFullPath

      #region Transacted

      #region .NET

      /// <summary>[AlphaFS] Gets the <see cref="T:FileAttributes"/> of the file on the path.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to the file.</param>
      /// <returns>The <see cref="T:FileAttributes"/> of the file on the path.</returns>
      [SecurityCritical]
      public static FileAttributes GetAttributes(KernelTransaction transaction, string path)
      {
         return GetAttributesInternal(false, transaction, path, true, false, false);
      }

      #endregion //.NET

      #region IsFullPath

      /// <summary>[AlphaFS] Gets the <see cref="T:FileAttributes"/> of the file on the path.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to the file.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>The <see cref="T:FileAttributes"/> of the file on the path.</returns>
      [SecurityCritical]
      public static FileAttributes GetAttributes(KernelTransaction transaction, string path, bool? isFullPath)
      {
         return GetAttributesInternal(false, transaction, path, true, false, isFullPath);
      }

      #endregion // IsFullPath

      #endregion // Transacted

      #endregion // AlphaFS

      #endregion GetAttributes

      #region GetCreationTime

      #region .NET

      /// <summary>Gets the creation date and time of the specified file.</summary>
      /// <param name="path">The file for which to obtain creation date and time information.</param>
      /// <returns>A <see cref="T:System.DateTime"/> structure set to the creation date and time for the specified file. This value is expressed in local time.</returns>
      [SecurityCritical]
      public static DateTime GetCreationTime(string path)
      {
         return GetCreationTimeInternal(false, null, path, false, false).ToLocalTime();
      }

      #endregion // .NET

      #region AlphaFS

      #region IsFullPath

      /// <summary>[AlphaFS] Gets the creation date and time of the specified file.</summary>
      /// <param name="path">The file for which to obtain creation date and time information.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>A <see cref="T:System.DateTime"/> structure set to the creation date and time for the specified file. This value is expressed in local time.</returns>
      [SecurityCritical]
      public static DateTime GetCreationTime(string path, bool? isFullPath)
      {
         return GetCreationTimeInternal(false, null, path, false, isFullPath).ToLocalTime();
      }

      #endregion // IsFullPath

      #region Transacted

      #region .NET

      /// <summary>[AlphaFS] Gets the creation date and time of the specified file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file for which to obtain creation date and time information.</param>
      /// <returns>A <see cref="T:System.DateTime"/> structure set to the creation date and time for the specified file. This value is expressed in local time.</returns>
      [SecurityCritical]
      public static DateTime GetCreationTime(KernelTransaction transaction, string path)
      {
         return GetCreationTimeInternal(false, transaction, path, false, false).ToLocalTime();
      }

      #endregion // .NET

      #region IsFullPath

      /// <summary>[AlphaFS] Gets the creation date and time of the specified file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file for which to obtain creation date and time information.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>A <see cref="T:System.DateTime"/> structure set to the creation date and time for the specified file. This value is expressed in local time.</returns>
      [SecurityCritical]
      public static DateTime GetCreationTime(KernelTransaction transaction, string path, bool? isFullPath)
      {
         return GetCreationTimeInternal(false, transaction, path, false, isFullPath).ToLocalTime();
      }

      #endregion // IsFullPath
      
      #endregion // Transacted

      #endregion // AlphaFS

      #endregion // GetCreationTime

      #region GetCreationTimeUtc

      #region .NET

      /// <summary>Gets the creation date and time, in Coordinated Universal Time (UTC) format, of the specified file.</summary>
      /// <param name="path">The file for which to obtain creation date and time information, in Coordinated Universal Time (UTC) format.</param>
      /// <returns>A <see cref="T:System.DateTime"/> structure set to the creation date and time for the specified file. This value is expressed in UTC time.</returns>
      [SecurityCritical]
      public static DateTime GetCreationTimeUtc(string path)
      {
         return GetCreationTimeInternal(false, null, path, true, false);
      }

      #endregion // .NET

      #region AlphaFS

      #region IsFullPath

      /// <summary>[AlphaFS] Gets the creation date and time, in Coordinated Universal Time (UTC) format, of the specified file.</summary>
      /// <param name="path">The file for which to obtain creation date and time information, in Coordinated Universal Time (UTC) format.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>A <see cref="T:System.DateTime"/> structure set to the creation date and time for the specified file. This value is expressed in UTC time.</returns>
      [SecurityCritical]
      public static DateTime GetCreationTimeUtc(string path, bool? isFullPath)
      {
         return GetCreationTimeInternal(false, null, path, true, isFullPath);
      }

      #endregion // IsFullPath

      #region Transacted

      #region .NET

      /// <summary>[AlphaFS] Gets the creation date and time, in Coordinated Universal Time (UTC) format, of the specified file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file for which to obtain creation date and time information, in Coordinated Universal Time (UTC) format.</param>
      /// <returns>A <see cref="T:System.DateTime"/> structure set to the creation date and time for the specified file. This value is expressed in UTC time.</returns>
      [SecurityCritical]
      public static DateTime GetCreationTimeUtc(KernelTransaction transaction, string path)
      {
         return GetCreationTimeInternal(false, transaction, path, true, false);
      }

      #endregion // .NET

      #region IsFullPath

      /// <summary>[AlphaFS] Gets the creation date and time, in Coordinated Universal Time (UTC) format, of the specified file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file for which to obtain creation date and time information, in Coordinated Universal Time (UTC) format.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>A <see cref="T:System.DateTime"/> structure set to the creation date and time for the specified file. This value is expressed in UTC time.</returns>
      [SecurityCritical]
      public static DateTime GetCreationTimeUtc(KernelTransaction transaction, string path, bool? isFullPath)
      {
         return GetCreationTimeInternal(false, transaction, path, true, isFullPath);
      }

      #endregion // IsFullPath
      
      #endregion // Transacted

      #endregion // AlphaFS

      #endregion // GetCreationTimeUtc

      #region GetLastAccessTime

      #region .NET

      /// <summary>Gets the date and time that the specified file was last accessed.</summary>
      /// <param name="path">The file for which to obtain access date and time information.</param>
      /// <returns>A <see cref="T:System.DateTime"/> structure set to the date and time that the specified file was last accessed. This value is expressed in local time.</returns>
      [SecurityCritical]
      public static DateTime GetLastAccessTime(string path)
      {
         return GetLastAccessTimeInternal(false, null, path, false, false).ToLocalTime();
      }

      #endregion // .NET

      #region AlphaFS

      #region IsFullPath

      /// <summary>[AlphaFS] Gets the date and time that the specified file was last accessed.</summary>
      /// <param name="path">The file for which to obtain access date and time information.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>A <see cref="T:System.DateTime"/> structure set to the date and time that the specified file was last accessed. This value is expressed in local time.</returns>
      [SecurityCritical]
      public static DateTime GetLastAccessTime(string path, bool? isFullPath)
      {
         return GetLastAccessTimeInternal(false, null, path, false, isFullPath).ToLocalTime();
      }

      #endregion // IsFullPath

      #region Transacted

      #region .NET

      /// <summary>[AlphaFS] Gets the date and time that the specified file was last accessed.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file for which to obtain access date and time information.</param>
      /// <returns>A <see cref="T:System.DateTime"/> structure set to the date and time that the specified file was last accessed. This value is expressed in local time.</returns>
      [SecurityCritical]
      public static DateTime GetLastAccessTime(KernelTransaction transaction, string path)
      {
         return GetLastAccessTimeInternal(false, transaction, path, false, false).ToLocalTime();
      }

      #endregion // .NET

      #region IsFullPath

      /// <summary>[AlphaFS] Gets the date and time that the specified file was last accessed.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file for which to obtain access date and time information.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>A <see cref="T:System.DateTime"/> structure set to the date and time that the specified file was last accessed. This value is expressed in local time.</returns>
      [SecurityCritical]
      public static DateTime GetLastAccessTime(KernelTransaction transaction, string path, bool? isFullPath)
      {
         return GetLastAccessTimeInternal(false, transaction, path, false, isFullPath).ToLocalTime();
      }

      #endregion // IsFullPath
      
      #endregion // Transacted

      #endregion // AlphaFS

      #endregion // GetLastAccessTime

      #region GetLastAccessTimeUtc

      #region .NET

      /// <summary>Gets the date and time, in coordinated universal time (UTC), that the specified file was last accessed.</summary>
      /// <param name="path">The file for which to obtain access date and time information.</param>
      /// <returns>A <see cref="T:System.DateTime"/> structure set to the date and time that the specified file was last accessed. This value is expressed in UTC time.</returns>
      [SecurityCritical]
      public static DateTime GetLastAccessTimeUtc(string path)
      {
         return GetLastAccessTimeInternal(false, null, path, true, false);
      }

      #endregion // .NET

      #region AlphaFS

      #region IsFullPath

      /// <summary>[AlphaFS] Gets the date and time, in coordinated universal time (UTC), that the specified file was last accessed.</summary>
      /// <param name="path">The file for which to obtain access date and time information.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>A <see cref="T:System.DateTime"/> structure set to the date and time that the specified file was last accessed. This value is expressed in UTC time.</returns>
      [SecurityCritical]
      public static DateTime GetLastAccessTimeUtc(string path, bool? isFullPath)
      {
         return GetLastAccessTimeInternal(false, null, path, true, isFullPath);
      }

      #endregion // IsFullPath

      #region Transacted

      #region .NET

      /// <summary>[AlphaFS] Gets the date and time, in coordinated universal time (UTC), that the specified file was last accessed.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file for which to obtain access date and time information.</param>
      /// <returns>A <see cref="T:System.DateTime"/> structure set to the date and time that the specified file was last accessed. This value is expressed in UTC time.</returns>
      [SecurityCritical]
      public static DateTime GetLastAccessTimeUtc(KernelTransaction transaction, string path)
      {
         return GetLastAccessTimeInternal(false, transaction, path, true, false);
      }

      #endregion // .NET

      #region IsFullPath

      /// <summary>[AlphaFS] Gets the date and time, in coordinated universal time (UTC), that the specified file was last accessed.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file for which to obtain access date and time information.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>A <see cref="T:System.DateTime"/> structure set to the date and time that the specified file was last accessed. This value is expressed in UTC time.</returns>
      [SecurityCritical]
      public static DateTime GetLastAccessTimeUtc(KernelTransaction transaction, string path, bool? isFullPath)
      {
         return GetLastAccessTimeInternal(false, transaction, path, true, isFullPath);
      }

      #endregion // IsFullPath

      #endregion // Transacted

      #endregion // AlphaFS

      #endregion // GetLastAccessTimeUtc

      #region GetLastWriteTime

      #region .NET

      /// <summary>Gets the date and time that the specified file was last written to.</summary>
      /// <param name="path">The file for which to obtain write date and time information.</param>
      /// <returns>A <see cref="T:System.DateTime"/> structure set to the date and time that the specified file was last written to. This value is expressed in local time.</returns>
      [SecurityCritical]
      public static DateTime GetLastWriteTime(string path)
      {
         return GetLastWriteTimeInternal(false, null, path, false, false).ToLocalTime();
      }

      #endregion //.NET

      #region AlphaFS

      #region IsFullPath

      /// <summary>[AlphaFS] Gets the date and time that the specified file was last written to.</summary>
      /// <param name="path">The file for which to obtain write date and time information.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>A <see cref="T:System.DateTime"/> structure set to the date and time that the specified file was last written to. This value is expressed in local time.</returns>
      [SecurityCritical]
      public static DateTime GetLastWriteTime(string path, bool? isFullPath)
      {
         return GetLastWriteTimeInternal(false, null, path, false, isFullPath).ToLocalTime();
      }

      #endregion // IsFullPath

      #region Transacted

      #region .NET

      /// <summary>[AlphaFS] Gets the date and time that the specified file was last written to.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file for which to obtain write date and time information.</param>
      /// <returns>A <see cref="T:System.DateTime"/> structure set to the date and time that the specified file was last written to. This value is expressed in local time.</returns>
      [SecurityCritical]
      public static DateTime GetLastWriteTime(KernelTransaction transaction, string path)
      {
         return GetLastWriteTimeInternal(false, transaction, path, false, false).ToLocalTime();
      }

      #endregion // .NET

      #region IsFullPath

      /// <summary>[AlphaFS] Gets the date and time that the specified file was last written to.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file for which to obtain write date and time information.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>A <see cref="T:System.DateTime"/> structure set to the date and time that the specified file was last written to. This value is expressed in local time.</returns>
      [SecurityCritical]
      public static DateTime GetLastWriteTime(KernelTransaction transaction, string path, bool? isFullPath)
      {
         return GetLastWriteTimeInternal(false, transaction, path, false, isFullPath).ToLocalTime();
      }

      #endregion // IsFullPath

      #endregion // Transacted

      #endregion // AlphaFS

      #endregion // GetLastWriteTime

      #region GetLastWriteTimeUtc

      #region .NET

      /// <summary>Gets the date and time, in coordinated universal time (UTC) time, that the specified file was last written to.</summary>
      /// <param name="path">The file for which to obtain write date and time information.</param>
      /// <returns>A <see cref="T:System.DateTime"/> structure set to the date and time that the specified file was last written to. This value is expressed in UTC time.</returns>
      [SecurityCritical]
      public static DateTime GetLastWriteTimeUtc(string path)
      {
         return GetLastWriteTimeInternal(false, null, path, true, false);
      }

      #endregion // .NET

      #region AlphaFS

      #region IsFullPath

      /// <summary>[AlphaFS] Gets the date and time, in coordinated universal time (UTC) time, that the specified file was last written to.</summary>
      /// <param name="path">The file for which to obtain write date and time information.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>A <see cref="T:System.DateTime"/> structure set to the date and time that the specified file was last written to. This value is expressed in UTC time.</returns>
      [SecurityCritical]
      public static DateTime GetLastWriteTimeUtc(string path, bool? isFullPath)
      {
         return GetLastWriteTimeInternal(false, null, path, true, isFullPath);
      }

      #endregion // IsFullPath

      #region Transacted

      #region .NET

      /// <summary>[AlphaFS] Gets the date and time, in coordinated universal time (UTC) time, that the specified file was last written to.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file for which to obtain write date and time information.</param>
      /// <returns>A <see cref="T:System.DateTime"/> structure set to the date and time that the specified file was last written to. This value is expressed in UTC time.</returns>
      [SecurityCritical]
      public static DateTime GetLastWriteTimeUtc(KernelTransaction transaction, string path)
      {
         return GetLastWriteTimeInternal(false, transaction, path, true, false);
      }

      #endregion // .NET

      #region IsFullPath

      /// <summary>[AlphaFS] Gets the date and time, in coordinated universal time (UTC) time, that the specified file was last written to.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file for which to obtain write date and time information.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>A <see cref="T:System.DateTime"/> structure set to the date and time that the specified file was last written to. This value is expressed in UTC time.</returns>
      [SecurityCritical]
      public static DateTime GetLastWriteTimeUtc(KernelTransaction transaction, string path, bool? isFullPath)
      {
         return GetLastWriteTimeInternal(false, transaction, path, true, isFullPath);
      }

      #endregion // IsFullPath
      
      #endregion // Transacted

      #endregion // AlphaFS

      #endregion // GetLastWriteTimeUtc

      #region Move

      #region .NET

      /// <summary>Moves a specified file to a new location, providing the option to specify a new file name.</summary>
      /// <param name="sourceFileName">The name of the file to move.</param>
      /// <param name="destFileName">The new path for the file.</param>
      /// <remarks>This method works across disk volumes.</remarks>
      /// <remarks>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method. If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "dest")]
      [SecurityCritical]
      public static void Move(string sourceFileName, string destFileName)
      {
         CopyMoveInternal(true, null, sourceFileName, destFileName, false, null, MoveOptions.CopyAllowed, null, null, false);
      }

      #endregion // .NET

      #region AlphaFS

      #region IsFullPath

      /// <summary>[AlphaFS] Moves a specified file to a new location, providing the option to specify a new file name.</summary>
      /// <param name="sourceFileName">The name of the file to move.</param>
      /// <param name="destFileName">The new path for the file.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="sourceFileName"/> and <paramref name="destFileName"/> will be normalized and long path prefixed. <c>null</c> <paramref name="sourceFileName"/> and <paramref name="destFileName"/> are already a full path with long path prefix, will be used as is.</param>
      /// <remarks>This method works across disk volumes.</remarks>
      /// <remarks>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method. If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "dest")]
      [SecurityCritical]
      public static void Move(string sourceFileName, string destFileName, bool? isFullPath)
      {
         CopyMoveInternal(true, null, sourceFileName, destFileName, false, null, MoveOptions.CopyAllowed, null, null, isFullPath);
      }

      /// <summary>[AlphaFS] Moves a specified file to a new location, providing the option to specify a new file name.</summary>
      /// <param name="sourceFileName">The name of the file to move.</param>
      /// <param name="destFileName">The new path for the file.</param>
      /// <param name="options">The <see cref="T:MoveOptions"/> options.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="sourceFileName"/> and <paramref name="destFileName"/> will be normalized and long path prefixed. <c>null</c> <paramref name="sourceFileName"/> and <paramref name="destFileName"/> are already a full path with long path prefix, will be used as is.</param>
      /// <remarks>This method works across disk volumes.</remarks>
      /// <remarks>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method. If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "dest")]
      [SecurityCritical]
      public static void Move(string sourceFileName, string destFileName, MoveOptions options, bool? isFullPath)
      {
         CopyMoveInternal(true, null, sourceFileName, destFileName, false, null, options, null, null, isFullPath);
      }

      /// <summary>[AlphaFS] Moves a file or directory, including its children. You can provide a callback function that receives progress notifications.</summary>
      /// <param name="sourceFileName"><para>The name of the existing file or directory on the local computer.</para>
      /// <para>If <paramref name="options"/> specifies <see cref="T:MoveOptions.DelayUntilReboot"/>, the file cannot exist on
      /// a remote share because delayed operations are performed before the network is available.</para></param>
      /// <param name="destFileName">
      /// <para>The new name of the file or directory on the local computer.</para>
      /// <para>When moving a file, <paramref name="destFileName"/> can be on a different file system or volume.
      /// If <paramref name="destFileName"/> is on another drive, you must set the
      /// <see cref="T:MoveOptions.CopyAllowed"/> flag in <paramref name="options"/>.
      /// </para>
      /// <para>When moving a directory, <paramref name="sourceFileName"/> and <paramref name="destFileName"/> must be on the same drive. </para>
      ///</param>
      /// <param name="options">The move options.</param>
      /// <param name="moveProgress">A <see cref="T:CopyMoveProgressCallback"/> callback function that is called each time another
      /// portion of the file has been moved. The callback function can be useful if you provide a user interface that displays
      /// the progress of the operation. This parameter can be <c>null</c>.</param>
      /// <param name="userProgressData">An argument to be passed to the <see cref="T:CopyMoveProgressCallback"/> callback function. This parameter can be <c>null</c>.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="sourceFileName"/> and <paramref name="destFileName"/> will be normalized and long path prefixed. <c>null</c> <paramref name="sourceFileName"/> and <paramref name="destFileName"/> are already a full path with long path prefix, will be used as is.</param>
      /// <returns><c>true</c> when successfully moved, <c>false</c> otherwise or the operation was aborted.</returns>
      /// <remarks>This method works across disk volumes.</remarks>
      /// <remarks>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method. If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "dest")]
      [SecurityCritical]
      public static void Move(string sourceFileName, string destFileName, MoveOptions options, CopyMoveProgressCallback moveProgress, object userProgressData, bool? isFullPath)
      {
         CopyMoveInternal(true, null, sourceFileName, destFileName, false, null, options, moveProgress, userProgressData, isFullPath);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Moves a file or directory, including its children. You can provide a callback function that receives progress notifications.</summary>
      /// <param name="sourceFileName"><para>The name of the existing file or directory on the local computer.</para>
      /// <para>If <paramref name="options"/> specifies <see cref="T:MoveOptions.DelayUntilReboot"/>, the file cannot exist on
      /// a remote share because delayed operations are performed before the network is available.</para></param>
      /// <param name="destFileName">
      /// <para>The new name of the file or directory on the local computer.</para>
      /// <para>When moving a file, <paramref name="destFileName"/> can be on a different file system or volume.
      /// If <paramref name="destFileName"/> is on another drive, you must set the
      /// <see cref="T:MoveOptions.CopyAllowed"/> flag in <paramref name="options"/>.
      /// </para>
      /// <para>When moving a directory, <paramref name="sourceFileName"/> and <paramref name="destFileName"/> must be on the same drive. </para>
      ///</param>
      /// <param name="options">The move options.</param>
      /// <param name="moveProgress">A <see cref="T:CopyMoveProgressCallback"/> callback function that is called each time another
      /// portion of the file has been moved. The callback function can be useful if you provide a user interface that displays
      /// the progress of the operation. This parameter can be <c>null</c>.</param>
      /// <param name="userProgressData">An argument to be passed to the <see cref="T:CopyMoveProgressCallback"/> callback function. This parameter can be <c>null</c>.</param>
      /// <returns><c>true</c> when successfully moved, <c>false</c> otherwise or the operation was aborted.</returns>
      /// <remarks>This method works across disk volumes.</remarks>
      /// <remarks>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method. If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "dest")]
      [SecurityCritical]
      public static void Move(string sourceFileName, string destFileName, MoveOptions options, CopyMoveProgressCallback moveProgress, object userProgressData)
      {
         CopyMoveInternal(true, null, sourceFileName, destFileName, false, null, options, moveProgress, userProgressData, false);
      }

      #region Transacted
      
      #region .NET

      /// <summary>[AlphaFS] Moves a specified file to a new location as part of a transaction, providing the option to specify a new file name.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourceFileName">The name of the file to move.</param>
      /// <param name="destFileName">The new path for the file.</param>
      /// <returns><c>true</c> when successfully moved, <c>false</c> otherwise.</returns>
      /// <remarks>This method works across disk volumes.</remarks>
      /// <remarks>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method. If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "dest")]
      [SecurityCritical]
      public static void Move(KernelTransaction transaction, string sourceFileName, string destFileName)
      {
         CopyMoveInternal(true, transaction, sourceFileName, destFileName, false, null, MoveOptions.CopyAllowed, null, null, false);
      }

      #endregion // .NET

      #region IsFullPath

      /// <summary>[AlphaFS] Moves a specified file to a new location as part of a transaction, providing the option to specify a new file name.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourceFileName">The name of the file to move.</param>
      /// <param name="destFileName">The new path for the file.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="sourceFileName"/> and <paramref name="destFileName"/> will be normalized and long path prefixed. <c>null</c> <paramref name="sourceFileName"/> and <paramref name="destFileName"/> are already a full path with long path prefix, will be used as is.</param>
      /// <returns><c>true</c> when successfully moved, <c>false</c> otherwise.</returns>
      /// <remarks>This method works across disk volumes.</remarks>
      /// <remarks>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method. If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "dest")]
      [SecurityCritical]
      public static void Move(KernelTransaction transaction, string sourceFileName, string destFileName, bool? isFullPath)
      {
         CopyMoveInternal(true, transaction, sourceFileName, destFileName, false, null, MoveOptions.CopyAllowed, null, null, isFullPath);
      }

      /// <summary>[AlphaFS] Moves a file or directory  as part of a transaction, including its children.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourceFileName"><para>The name of the existing file or directory on the local computer.</para>
      /// 	<para>If <paramref name="options"/> specifies <see cref="T:MoveOptions.DelayUntilReboot"/>, the file cannot exist on
      /// a remote share because delayed operations are performed before the network is available.</para></param>
      /// <param name="destFileName"><para>The new name of the file or directory on the local computer.</para>
      /// 	<para>When moving a file, <paramref name="destFileName"/> can be on a different file system or volume.
      /// If <paramref name="destFileName"/> is on another drive, you must set the
      /// <see cref="T:MoveOptions.CopyAllowed"/> flag in <paramref name="options"/>.
      /// </para>
      /// 	<para>When moving a directory, <paramref name="sourceFileName"/> and <paramref name="destFileName"/> must be on the same drive. </para></param>
      /// <param name="options">The move options.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="sourceFileName"/> and <paramref name="destFileName"/> will be normalized and long path prefixed. <c>null</c> <paramref name="sourceFileName"/> and <paramref name="destFileName"/> are already a full path with long path prefix, will be used as is.</param>
      /// <returns><c>true</c> when successfully moved, <c>false</c> otherwise.</returns>
      /// <remarks>This method works across disk volumes.</remarks>
      /// <remarks>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method. If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "dest")]
      [SecurityCritical]
      public static void Move(KernelTransaction transaction, string sourceFileName, string destFileName, MoveOptions options, bool? isFullPath)
      {
         CopyMoveInternal(true, transaction, sourceFileName, destFileName, false, null, options, null, null, isFullPath);
      }

      /// <summary>[AlphaFS] Moves a file or directory as part of a transaction, including its children. You can provide a callback function that receives progress notifications.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourceFileName"><para>The name of the existing file or directory on the local computer.</para>
      /// 	<para>If <paramref name="options"/> specifies <see cref="T:MoveOptions.DelayUntilReboot"/>, the file cannot exist on
      /// a remote share because delayed operations are performed before the network is available.</para></param>
      /// <param name="destFileName"><para>The new name of the file or directory on the local computer.</para>
      /// 	<para>When moving a file, <paramref name="destFileName"/> can be on a different file system or volume.
      /// If <paramref name="destFileName"/> is on another drive, you must set the
      /// <see cref="T:MoveOptions.CopyAllowed"/> flag in <paramref name="options"/>.
      /// </para>
      /// 	<para>When moving a directory, <paramref name="sourceFileName"/> and <paramref name="destFileName"/> must be on the same drive. </para></param>
      /// <param name="options">The move options.</param>
      /// <param name="moveProgress">A <see cref="T:CopyMoveProgressCallback"/> callback function that is called each time another
      /// portion of the file has been moved. The callback function can be useful if you provide a user interface that displays
      /// the progress of the operation. This parameter can be <c>null</c>.</param>
      /// <param name="userProgressData">An argument to be passed to the <see cref="T:CopyMoveProgressCallback"/> callback function. This parameter can be <c>null</c>.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="sourceFileName"/> and <paramref name="destFileName"/> will be normalized and long path prefixed. <c>null</c> <paramref name="sourceFileName"/> and <paramref name="destFileName"/> are already a full path with long path prefix, will be used as is.</param>
      /// <returns><c>true</c> when successfully moved, <c>false</c> otherwise or the operation was aborted.</returns>
      /// <remarks>This method works across disk volumes.</remarks>
      /// <remarks>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method. If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "dest")]
      [SecurityCritical]
      public static void Move(KernelTransaction transaction, string sourceFileName, string destFileName, MoveOptions options, CopyMoveProgressCallback moveProgress, object userProgressData, bool? isFullPath)
      {
         CopyMoveInternal(true, transaction, sourceFileName, destFileName, false, null, options, moveProgress, userProgressData, isFullPath);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Moves a file or directory as part of a transaction, including its children. You can provide a callback function that receives progress notifications.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourceFileName"><para>The name of the existing file or directory on the local computer.</para>
      /// 	<para>If <paramref name="options"/> specifies <see cref="T:MoveOptions.DelayUntilReboot"/>, the file cannot exist on
      /// a remote share because delayed operations are performed before the network is available.</para></param>
      /// <param name="destFileName"><para>The new name of the file or directory on the local computer.</para>
      /// 	<para>When moving a file, <paramref name="destFileName"/> can be on a different file system or volume.
      /// If <paramref name="destFileName"/> is on another drive, you must set the
      /// <see cref="T:MoveOptions.CopyAllowed"/> flag in <paramref name="options"/>.
      /// </para>
      /// 	<para>When moving a directory, <paramref name="sourceFileName"/> and <paramref name="destFileName"/> must be on the same drive. </para></param>
      /// <param name="options">The move options.</param>
      /// <param name="moveProgress">A <see cref="T:CopyMoveProgressCallback"/> callback function that is called each time another
      /// portion of the file has been moved. The callback function can be useful if you provide a user interface that displays
      /// the progress of the operation. This parameter can be <c>null</c>.</param>
      /// <param name="userProgressData">An argument to be passed to the <see cref="T:CopyMoveProgressCallback"/> callback function. This parameter can be <c>null</c>.</param>
      /// <returns><c>true</c> when successfully moved, <c>false</c> otherwise or the operation was aborted.</returns>
      /// <remarks>This method works across disk volumes.</remarks>
      /// <remarks>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method. If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "dest")]
      [SecurityCritical]
      public static void Move(KernelTransaction transaction, string sourceFileName, string destFileName, MoveOptions options, CopyMoveProgressCallback moveProgress, object userProgressData)
      {
         CopyMoveInternal(true, transaction, sourceFileName, destFileName, false, null, options, moveProgress, userProgressData, false);
      }

      #endregion // Transacted

      #endregion // AlphaFS

      #endregion // Move

      #region Open

      #region .NET

      /// <summary>Opens a <see cref="T:FileStream"/> on the specified path with read/write access.</summary>
      /// <param name="path">The file to open.</param>
      /// <param name="mode">A <see cref="T:FileMode"/> value that specifies whether a file is created if one does not exist, and determines whether the contents of existing files are retained or overwritten.</param>
      /// <returns>A <see cref="T:FileStream"/> opened in the specified mode and path, with read/write access and not shared.</returns>
      [SecurityCritical]
      public static FileStream Open(string path, FileMode mode)
      {
         return OpenInternal(null, path, mode, 0, mode == FileMode.Append ? FileAccess.Write : FileAccess.ReadWrite, FileShare.None, EFileAttributes.Normal, false);
      }

      /// <summary>Opens a <see cref="T:FileStream"/> on the specified path, with the specified mode and access.</summary>
      /// <param name="path">The file to open.</param>
      /// <param name="mode">A <see cref="T:FileMode"/> value that specifies whether a file is created if one does not exist, and determines whether the contents of existing files are retained or overwritten.</param>
      /// <param name="access">A <see cref="T:FileAccess"/> value that specifies the operations that can be performed on the file.</param>
      /// <returns>An unshared <see cref="T:FileStream"/> that provides access to the specified file, with the specified mode and access.</returns>
      [SecurityCritical]
      public static FileStream Open(string path, FileMode mode, FileAccess access)
      {
         return OpenInternal(null, path, mode, 0, access, FileShare.None, EFileAttributes.Normal, false);
      }

      /// <summary>Opens a <see cref="T:FileStream"/> on the specified path, having the specified mode with read, write, or read/write access and the specified sharing option.</summary>
      /// <param name="path">The file to open.</param>
      /// <param name="mode">A <see cref="T:FileMode"/> value that specifies whether a file is created if one does not exist, and determines whether the contents of existing files are retained or overwritten.</param>
      /// <param name="access">A <see cref="T:FileAccess"/> value that specifies the operations that can be performed on the file.</param>
      /// <param name="share">A <see cref="T:FileShare"/> value specifying the type of access other threads have to the file.</param>
      /// <returns>A <see cref="T:FileStream"/> on the specified path, having the specified mode with read, write, or read/write access and the specified sharing option.</returns>
      [SecurityCritical]
      public static FileStream Open(string path, FileMode mode, FileAccess access, FileShare share)
      {
         return OpenInternal(null, path, mode, 0, access, share, EFileAttributes.Normal, false);
      }

      #endregion // .NET

      #region AlphaFS

      #region IsFullPath

      /// <summary>[AlphaFS] Opens a <see cref="T:FileStream"/> on the specified path with read/write access.</summary>
      /// <param name="path">The file to open.</param>
      /// <param name="mode">A <see cref="T:FileMode"/> value that specifies whether a file is created if one does not exist, and determines whether the contents of existing files are retained or overwritten.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>A <see cref="T:FileStream"/> opened in the specified mode and path, with read/write access and not shared.</returns>
      [SecurityCritical]
      public static FileStream Open(string path, FileMode mode, bool? isFullPath)
      {
         return OpenInternal(null, path, mode, 0, mode == FileMode.Append ? FileAccess.Write : FileAccess.ReadWrite, FileShare.None, EFileAttributes.Normal, isFullPath);
      }

      /// <summary>[AlphaFS] Opens a <see cref="T:FileStream"/> on the specified path, with the specified mode and access.</summary>
      /// <param name="path">The file to open.</param>
      /// <param name="mode">A <see cref="T:FileMode"/> value that specifies whether a file is created if one does not exist, and determines whether the contents of existing files are retained or overwritten.</param>
      /// <param name="access">A <see cref="T:FileAccess"/> value that specifies the operations that can be performed on the file.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>An unshared <see cref="T:FileStream"/> that provides access to the specified file, with the specified mode and access.</returns>
      [SecurityCritical]
      public static FileStream Open(string path, FileMode mode, FileAccess access, bool? isFullPath)
      {
         return OpenInternal(null, path, mode, 0, access, FileShare.None, EFileAttributes.Normal, isFullPath);
      }

      /// <summary>[AlphaFS] Opens a <see cref="T:FileStream"/> on the specified path, having the specified mode with read, write, or read/write access and the specified sharing option.</summary>
      /// <param name="path">The file to open.</param>
      /// <param name="mode">A <see cref="T:FileMode"/> value that specifies whether a file is created if one does not exist, and determines whether the contents of existing files are retained or overwritten.</param>
      /// <param name="access">A <see cref="T:FileAccess"/> value that specifies the operations that can be performed on the file.</param>
      /// <param name="share">A <see cref="T:FileShare"/> value specifying the type of access other threads have to the file.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>A <see cref="T:FileStream"/> on the specified path, having the specified mode with read, write, or read/write access and the specified sharing option.</returns>
      [SecurityCritical]
      public static FileStream Open(string path, FileMode mode, FileAccess access, FileShare share, bool? isFullPath)
      {
         return OpenInternal(null, path, mode, 0, access, share, EFileAttributes.Normal, isFullPath);
      }

      #endregion // IsFullPath

      #region Transacted

      #region .NET

      /// <summary>[AlphaFS] (Transacted) Opens a <see cref="T:FileStream"/> on the specified path with read/write access.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to open.</param>
      /// <param name="mode">A <see cref="T:FileMode"/> value that specifies whether a file is created if one does not exist, and determines whether the contents of existing files are retained or overwritten.</param>
      /// <returns>A <see cref="T:FileStream"/> opened in the specified mode and path, with read/write access and not shared.</returns>
      [SecurityCritical]
      public static FileStream Open(KernelTransaction transaction, string path, FileMode mode)
      {
         return OpenInternal(transaction, path, mode, 0, mode == FileMode.Append ? FileAccess.Write : FileAccess.ReadWrite, FileShare.None, EFileAttributes.Normal, false);
      }

      /// <summary>[AlphaFS] Opens a <see cref="T:FileStream"/> on the specified path, with the specified mode and access.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to open.</param>
      /// <param name="mode">A <see cref="T:FileMode"/> value that specifies whether a file is created if one does not exist, and determines whether the contents of existing files are retained or overwritten.</param>
      /// <param name="access">A <see cref="T:FileAccess"/> value that specifies the operations that can be performed on the file.</param>
      /// <returns>An unshared <see cref="T:FileStream"/> that provides access to the specified file, with the specified mode and access.</returns>
      [SecurityCritical]
      public static FileStream Open(KernelTransaction transaction, string path, FileMode mode, FileAccess access)
      {
         return OpenInternal(transaction, path, mode, 0, access, FileShare.None, EFileAttributes.Normal, false);
      }

      /// <summary>[AlphaFS] Opens a <see cref="T:FileStream"/> on the specified path, having the specified mode with read, write, or read/write access and the specified sharing option.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to open.</param>
      /// <param name="mode">A <see cref="T:FileMode"/> value that specifies whether a file is created if one does not exist, and determines whether the contents of existing files are retained or overwritten.</param>
      /// <param name="access">A <see cref="T:FileAccess"/> value that specifies the operations that can be performed on the file.</param>
      /// <param name="share">A <see cref="T:FileShare"/> value specifying the type of access other threads have to the file.</param>
      /// <returns>A <see cref="T:FileStream"/> on the specified path, having the specified mode with read, write, or read/write access and the specified sharing option.</returns>
      [SecurityCritical]
      public static FileStream Open(KernelTransaction transaction, string path, FileMode mode, FileAccess access, FileShare share)
      {
         return OpenInternal(transaction, path, mode, 0, access, share, EFileAttributes.Normal, false);
      }

      #endregion // .NET

      #region IsFullPath

      /// <summary>[AlphaFS] (Transacted) Opens a <see cref="T:FileStream"/> on the specified path with read/write access.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to open.</param>
      /// <param name="mode">A <see cref="T:FileMode"/> value that specifies whether a file is created if one does not exist, and determines whether the contents of existing files are retained or overwritten.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>A <see cref="T:FileStream"/> opened in the specified mode and path, with read/write access and not shared.</returns>
      [SecurityCritical]
      public static FileStream Open(KernelTransaction transaction, string path, FileMode mode, bool? isFullPath)
      {
         return OpenInternal(transaction, path, mode, 0, mode == FileMode.Append ? FileAccess.Write : FileAccess.ReadWrite, FileShare.None, EFileAttributes.Normal, isFullPath);
      }

      /// <summary>[AlphaFS] Opens a <see cref="T:FileStream"/> on the specified path, with the specified mode and access.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to open.</param>
      /// <param name="mode">A <see cref="T:FileMode"/> value that specifies whether a file is created if one does not exist, and determines whether the contents of existing files are retained or overwritten.</param>
      /// <param name="access">A <see cref="T:FileAccess"/> value that specifies the operations that can be performed on the file.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>An unshared <see cref="T:FileStream"/> that provides access to the specified file, with the specified mode and access.</returns>
      [SecurityCritical]
      public static FileStream Open(KernelTransaction transaction, string path, FileMode mode, FileAccess access, bool? isFullPath)
      {
         return OpenInternal(transaction, path, mode, 0, access, FileShare.None, EFileAttributes.Normal, isFullPath);
      }

      /// <summary>[AlphaFS] Opens a <see cref="T:FileStream"/> on the specified path, having the specified mode with read, write, or read/write access and the specified sharing option.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to open.</param>
      /// <param name="mode">A <see cref="T:FileMode"/> value that specifies whether a file is created if one does not exist, and determines whether the contents of existing files are retained or overwritten.</param>
      /// <param name="access">A <see cref="T:FileAccess"/> value that specifies the operations that can be performed on the file.</param>
      /// <param name="share">A <see cref="T:FileShare"/> value specifying the type of access other threads have to the file.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>A <see cref="T:FileStream"/> on the specified path, having the specified mode with read, write, or read/write access and the specified sharing option.</returns>
      [SecurityCritical]
      public static FileStream Open(KernelTransaction transaction, string path, FileMode mode, FileAccess access, FileShare share, bool? isFullPath)
      {
         return OpenInternal(transaction, path, mode, 0, access, share, EFileAttributes.Normal, isFullPath);
      }

      #endregion // IsFullPath
      
      #endregion // Transacted

      #endregion // AlphaFS

      #endregion // Open

      #region OpenRead

      #region .NET

      /// <summary>Opens an existing file for reading.</summary>
      /// <param name="path">The file to be opened for reading.</param>
      /// <returns>A read-only <see cref="T:FileStream"/> on the specified path.</returns>
      /// <remarks>This method is equivalent to the FileStream(string, FileMode, FileAccess, FileShare) constructor overload with a <see cref="T:FileMode"/> value of Open, a <see cref="T:FileAccess"/> value of Read and a <see cref="T:FileShare"/> value of Read.</remarks>
      [SecurityCritical]
      public static FileStream OpenRead(string path)
      {
         return OpenInternal(null, path, FileMode.Open, 0, FileAccess.Read, FileShare.Read, EFileAttributes.Normal, false);
      }

      #endregion // .NET

      #region AlphaFS

      #region IsFullPath

      /// <summary>[AlphaFS] Opens an existing file for reading.</summary>
      /// <param name="path">The file to be opened for reading.</param>
      /// <returns>A read-only <see cref="T:FileStream"/> on the specified path.</returns>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <remarks>This method is equivalent to the FileStream(string, FileMode, FileAccess, FileShare) constructor overload with a <see cref="T:FileMode"/> value of Open, a <see cref="T:FileAccess"/> value of Read and a <see cref="T:FileShare"/> value of Read.</remarks>
      [SecurityCritical]
      public static FileStream OpenRead(string path, bool? isFullPath)
      {
         return OpenInternal(null, path, FileMode.Open, 0, FileAccess.Read, FileShare.Read, EFileAttributes.Normal, isFullPath);
      }

      #endregion // IsFullPath

      #region Transacted

      #region .NET

      /// <summary>[AlphaFS] Opens an existing file for reading.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to be opened for reading.</param>
      /// <returns>A read-only <see cref="T:FileStream"/> on the specified path.</returns>
      /// <remarks>This method is equivalent to the FileStream(string, FileMode, FileAccess, FileShare) constructor overload with a <see cref="T:FileMode"/> value of Open, a <see cref="T:FileAccess"/> value of Read and a <see cref="T:FileShare"/> value of Read.</remarks>
      [SecurityCritical]
      public static FileStream OpenRead(KernelTransaction transaction, string path)
      {
         return OpenInternal(transaction, path, FileMode.Open, 0, FileAccess.Read, FileShare.Read, EFileAttributes.Normal, false);
      }

      #endregion //.NET

      #region IsFullPath

      /// <summary>[AlphaFS] Opens an existing file for reading.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to be opened for reading.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>A read-only <see cref="T:FileStream"/> on the specified path.</returns>
      /// <remarks>This method is equivalent to the FileStream(string, FileMode, FileAccess, FileShare) constructor overload with a <see cref="T:FileMode"/> value of Open, a <see cref="T:FileAccess"/> value of Read and a <see cref="T:FileShare"/> value of Read.</remarks>
      [SecurityCritical]
      public static FileStream OpenRead(KernelTransaction transaction, string path, bool? isFullPath)
      {
         return OpenInternal(transaction, path, FileMode.Open, 0, FileAccess.Read, FileShare.Read, EFileAttributes.Normal, isFullPath);
      }

      #endregion // IsFullPath
      
      #endregion // Transacted

      #endregion // AlphaFS

      #endregion // OpenRead

      #region OpenText

      #region .NET

      /// <summary>Opens an existing <see cref="T:NativeMethods.DefaultFileEncoding"/> encoded text file for reading.</summary>
      /// <param name="path">The file to be opened for reading.</param>
      /// <returns>A <see cref="T:StreamReader"/> on the specified path.</returns>
      [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
      [SecurityCritical]
      public static StreamReader OpenText(string path)
      {
         return new StreamReader(OpenInternal(null, path, FileMode.Open, 0, FileAccess.Read, FileShare.Read, EFileAttributes.Normal, false), NativeMethods.DefaultFileEncoding);
      }

      #endregion // .NET

      #region AlphaFS

      #region IsFullPath

      /// <summary>[AlphaFS] Opens an existing <see cref="T:NativeMethods.DefaultFileEncoding"/> encoded text file for reading.</summary>
      /// <param name="path">The file to be opened for reading.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>A <see cref="T:StreamReader"/> on the specified path.</returns>
      [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
      [SecurityCritical]
      public static StreamReader OpenText(string path, bool? isFullPath)
      {
         return new StreamReader(OpenInternal(null, path, FileMode.Open, 0, FileAccess.Read, FileShare.Read, EFileAttributes.Normal, isFullPath), NativeMethods.DefaultFileEncoding);
      }

      /// <summary>[AlphaFS] Opens an existing <see cref="T:Encoding"/> encoded text file for reading.</summary>
      /// <param name="path">The file to be opened for reading.</param>
      /// <param name="encoding">The <see cref="T:Encoding"/> applied to the contents of the file.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>A <see cref="T:StreamReader"/> on the specified path.</returns>
      [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
      [SecurityCritical]
      public static StreamReader OpenText(string path, Encoding encoding, bool? isFullPath)
      {
         return new StreamReader(OpenInternal(null, path, FileMode.Open, 0, FileAccess.Read, FileShare.Read, EFileAttributes.Normal, isFullPath), encoding);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Opens an existing <see cref="T:Encoding"/> encoded text file for reading.</summary>
      /// <param name="path">The file to be opened for reading.</param>
      /// <param name="encoding">The <see cref="T:Encoding"/> applied to the contents of the file.</param>
      /// <returns>A <see cref="T:StreamReader"/> on the specified path.</returns>
      [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
      [SecurityCritical]
      public static StreamReader OpenText(string path, Encoding encoding)
      {
         return new StreamReader(OpenInternal(null, path, FileMode.Open, 0, FileAccess.Read, FileShare.Read, EFileAttributes.Normal, false), encoding);
      }

      #region Transacted

      #region .NET

      /// <summary>[AlphaFS] Opens an existing <see cref="T:NativeMethods.DefaultFileEncoding"/> encoded text file for reading.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to be opened for reading.</param>
      /// <returns>A <see cref="T:StreamReader"/> on the specified path.</returns>
      [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
      public static StreamReader OpenText(KernelTransaction transaction, string path)
      {
         return new StreamReader(OpenInternal(transaction, path, FileMode.Open, 0, FileAccess.Read, FileShare.Read, EFileAttributes.Normal, false), NativeMethods.DefaultFileEncoding);
      }

      #endregion // .NET

      #region IsFullPath

      /// <summary>[AlphaFS] Opens an existing <see cref="T:NativeMethods.DefaultFileEncoding"/> encoded text file for reading.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to be opened for reading.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>A <see cref="T:StreamReader"/> on the specified path.</returns>
      [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
      public static StreamReader OpenText(KernelTransaction transaction, string path, bool? isFullPath)
      {
         return new StreamReader(OpenInternal(transaction, path, FileMode.Open, 0, FileAccess.Read, FileShare.Read, EFileAttributes.Normal, isFullPath), NativeMethods.DefaultFileEncoding);
      }

      /// <summary>[AlphaFS] Opens an existing <see cref="T:Encoding"/> encoded text file for reading.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to be opened for reading.</param>
      /// <param name="encoding">The <see cref="T:Encoding"/> applied to the contents of the file.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>A <see cref="T:StreamReader"/> on the specified path.</returns>
      [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
      public static StreamReader OpenText(KernelTransaction transaction, string path, Encoding encoding, bool? isFullPath)
      {
         return new StreamReader(OpenInternal(transaction, path, FileMode.Open, 0, FileAccess.Read, FileShare.Read, EFileAttributes.Normal, isFullPath), encoding);
      }

      #endregion // IsFullPath
      
      /// <summary>[AlphaFS] Opens an existing <see cref="T:Encoding"/> encoded text file for reading.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to be opened for reading.</param>
      /// <param name="encoding">The <see cref="T:Encoding"/> applied to the contents of the file.</param>
      /// <returns>A <see cref="T:StreamReader"/> on the specified path.</returns>
      [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
      public static StreamReader OpenText(KernelTransaction transaction, string path, Encoding encoding)
      {
         return new StreamReader(OpenInternal(transaction, path, FileMode.Open, 0, FileAccess.Read, FileShare.Read, EFileAttributes.Normal, false), encoding);
      }

      #endregion // Transacted

      #endregion // AlphaFS

      #endregion // OpenText

      #region OpenWrite

      #region .NET

      /// <summary>Opens an existing file or creates a new file for writing.</summary>
      /// <param name="path">The file to be opened for writing.</param>
      /// <returns>An unshared <see cref="T:FileStream"/> object on the specified path with <see cref="T:FileAccess.Write"/> access.</returns>
      [SecurityCritical]
      public static FileStream OpenWrite(string path)
      {
         return OpenInternal(null, path, FileMode.OpenOrCreate, 0, FileAccess.Write, FileShare.None, EFileAttributes.Normal, false);
      }

      #endregion // .NET

      #region AlphaFS

      #region IsFullPath

      /// <summary>[AlphaFS] Opens an existing file or creates a new file for writing.</summary>
      /// <param name="path">The file to be opened for writing.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>An unshared <see cref="T:FileStream"/> object on the specified path with <see cref="T:FileAccess.Write"/> access.</returns>
      [SecurityCritical]
      public static FileStream OpenWrite(string path, bool? isFullPath)
      {
         return OpenInternal(null, path, FileMode.OpenOrCreate, 0, FileAccess.Write, FileShare.None, EFileAttributes.Normal, isFullPath);
      }

      #endregion // IsFullPath

      #region Transacted

      #region .NET

      /// <summary>[AlphaFS] Opens an existing file or creates a new file for writing.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to be opened for writing.</param>
      /// <returns>An unshared <see cref="T:FileStream"/> object on the specified path with <see cref="T:FileAccess.Write"/> access.</returns>
      [SecurityCritical]
      public static FileStream OpenWrite(KernelTransaction transaction, string path)
      {
         return OpenInternal(transaction, path, FileMode.OpenOrCreate, 0, FileAccess.Write, FileShare.None, EFileAttributes.Normal, false);
      }

      #endregion // .NET

      #region IsFullPath

      /// <summary>[AlphaFS] Opens an existing file or creates a new file for writing.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to be opened for writing.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>An unshared <see cref="T:FileStream"/> object on the specified path with <see cref="T:FileAccess.Write"/> access.</returns>
      [SecurityCritical]
      public static FileStream OpenWrite(KernelTransaction transaction, string path, bool? isFullPath)
      {
         return OpenInternal(transaction, path, FileMode.OpenOrCreate, 0, FileAccess.Write, FileShare.None, EFileAttributes.Normal, isFullPath);
      }

      #endregion // IsFullPath

      #endregion // Transacted

      #endregion // AlphaFS

      #endregion // OpenWrite

      #region ReadAllBytes

      #region .NET

      /// <summary>Opens a binary file, reads the contents of the file into a byte array, and then closes the file.</summary>
      /// <param name="path">The file to open for reading.</param>
      /// <returns>A byte array containing the contents of the file.</returns>
      [SecurityCritical]
      public static byte[] ReadAllBytes(string path)
      {
         return ReadAllBytesInternal(null, path, false);
      }

      #endregion // .NET

      #region AlphaFS

      #region IsFullPath

      /// <summary>[AlphaFS] Opens a binary file, reads the contents of the file into a byte array, and then closes the file.</summary>
      /// <param name="path">The file to open for reading.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>A byte array containing the contents of the file.</returns>
      [SecurityCritical]
      public static byte[] ReadAllBytes(string path, bool? isFullPath)
      {
         return ReadAllBytesInternal(null, path, isFullPath);
      }

      #endregion // IsFullPath

      #region Transacted

      #region .NET

      /// <summary>[AlphaFS] Opens a binary file, reads the contents of the file into a byte array, and then closes the file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to open for reading.</param>
      /// <returns>A byte array containing the contents of the file.</returns>
      [SecurityCritical]
      public static byte[] ReadAllBytes(KernelTransaction transaction, string path)
      {
         return ReadAllBytesInternal(transaction, path, false);
      }

      #endregion // .NET

      #region IsFullPath

      /// <summary>[AlphaFS] Opens a binary file, reads the contents of the file into a byte array, and then closes the file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to open for reading.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>A byte array containing the contents of the file.</returns>
      [SecurityCritical]
      public static byte[] ReadAllBytes(KernelTransaction transaction, string path, bool? isFullPath)
      {
         return ReadAllBytesInternal(transaction, path, isFullPath);
      }

      #endregion // IsFullPath
      
      #endregion // Transacted

      #endregion // AlphaFS

      #endregion // ReadAllBytes

      #region ReadAllLines

      #region .NET

      /// <summary>Opens a text file, reads all lines of the file, and then closes the file.</summary>
      /// <param name="path">The file to open for reading.</param>
      /// <returns>All lines of the file.</returns>
      [SecurityCritical]
      public static string[] ReadAllLines(string path)
      {
         return ReadAllLinesInternal(null, path, NativeMethods.DefaultFileEncoding, false).ToArray();
      }

      /// <summary>Opens a file, reads all lines of the file with the specified encoding, and then closes the file.</summary>
      /// <param name="path">The file to open for reading.</param>
      /// <param name="encoding">The <see cref="T:Encoding"/> applied to the contents of the file.</param>
      /// <returns>All lines of the file.</returns>
      [SecurityCritical]
      public static string[] ReadAllLines(string path, Encoding encoding)
      {
         return ReadAllLinesInternal(null, path, encoding, false).ToArray();
      }

      #endregion // .NET

      #region AlphaFS

      #region IsFullPath

      /// <summary>[AlphaFS] Opens a text file, reads all lines of the file, and then closes the file.</summary>
      /// <param name="path">The file to open for reading.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>All lines of the file.</returns>
      [SecurityCritical]
      public static string[] ReadAllLines(string path, bool? isFullPath)
      {
         return ReadAllLinesInternal(null, path, NativeMethods.DefaultFileEncoding, isFullPath).ToArray();
      }

      /// <summary>[AlphaFS] Opens a file, reads all lines of the file with the specified encoding, and then closes the file.</summary>
      /// <param name="path">The file to open for reading.</param>
      /// <param name="encoding">The <see cref="T:Encoding"/> applied to the contents of the file.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>All lines of the file.</returns>
      [SecurityCritical]
      public static string[] ReadAllLines(string path, Encoding encoding, bool? isFullPath)
      {
         return ReadAllLinesInternal(null, path, encoding, isFullPath).ToArray();
      }

      #endregion // IsFullPath

      #region Transacted

      #region .NET

      /// <summary>[AlphaFS] Opens a text file, reads all lines of the file, and then closes the file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to open for reading.</param>
      /// <returns>All lines of the file.</returns>
      [SecurityCritical]
      public static string[] ReadAllLines(KernelTransaction transaction, string path)
      {
         return ReadAllLinesInternal(transaction, path, NativeMethods.DefaultFileEncoding, false).ToArray();
      }

      /// <summary>[AlphaFS] Opens a file, reads all lines of the file with the specified encoding, and then closes the file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to open for reading.</param>
      /// <param name="encoding">The <see cref="T:Encoding"/> applied to the contents of the file.</param>
      /// <returns>All lines of the file.</returns>
      [SecurityCritical]
      public static string[] ReadAllLines(KernelTransaction transaction, string path, Encoding encoding)
      {
         return ReadAllLinesInternal(transaction, path, encoding, false).ToArray();
      }

      #endregion // .NET

      #region IsFullPath

      /// <summary>[AlphaFS] Opens a text file, reads all lines of the file, and then closes the file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to open for reading.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>All lines of the file.</returns>
      [SecurityCritical]
      public static string[] ReadAllLines(KernelTransaction transaction, string path, bool? isFullPath)
      {
         return ReadAllLinesInternal(transaction, path, NativeMethods.DefaultFileEncoding, isFullPath).ToArray();
      }

      /// <summary>[AlphaFS] Opens a file, reads all lines of the file with the specified encoding, and then closes the file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to open for reading.</param>
      /// <param name="encoding">The <see cref="T:Encoding"/> applied to the contents of the file.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>All lines of the file.</returns>
      [SecurityCritical]
      public static string[] ReadAllLines(KernelTransaction transaction, string path, Encoding encoding, bool? isFullPath)
      {
         return ReadAllLinesInternal(transaction, path, encoding, isFullPath).ToArray();
      }

      #endregion // IsFullPath

      #endregion // Transacted

      #endregion // AlphaFS

      #endregion // ReadAllLines

      #region ReadAllText

      #region .NET

      /// <summary>Opens a text file, reads all lines of the file, and then closes the file.</summary>
      /// <param name="path">The file to open for reading.</param>
      /// <returns>All lines of the file.</returns>
      [SecurityCritical]
      public static string ReadAllText(string path)
      {
         return ReadAllTextInternal(null, path, NativeMethods.DefaultFileEncoding, false);
      }

      /// <summary>Opens a file, reads all lines of the file with the specified encoding, and then closes the file.</summary>
      /// <param name="path">The file to open for reading.</param>
      /// <param name="encoding">The <see cref="T:Encoding"/> applied to the contents of the file.</param>
      /// <returns>All lines of the file.</returns>
      [SecurityCritical]
      public static string ReadAllText(string path, Encoding encoding)
      {
         return ReadAllTextInternal(null, path, encoding, false);
      }

      #endregion // .NET

      #region AlphaFS

      #region IsFullPath

      /// <summary>[AlphaFS] Opens a text file, reads all lines of the file, and then closes the file.</summary>
      /// <param name="path">The file to open for reading.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>All lines of the file.</returns>
      [SecurityCritical]
      public static string ReadAllText(string path, bool? isFullPath)
      {
         return ReadAllTextInternal(null, path, NativeMethods.DefaultFileEncoding, isFullPath);
      }

      /// <summary>[AlphaFS] Opens a file, reads all lines of the file with the specified encoding, and then closes the file.</summary>
      /// <param name="path">The file to open for reading.</param>
      /// <param name="encoding">The <see cref="T:Encoding"/> applied to the contents of the file.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>All lines of the file.</returns>
      [SecurityCritical]
      public static string ReadAllText(string path, Encoding encoding, bool? isFullPath)
      {
         return ReadAllTextInternal(null, path, encoding, isFullPath);
      }

      #endregion // IsFullPath

      #region Transacted

      #region .NET

      /// <summary>[AlphaFS] Opens a text file, reads all lines of the file, and then closes the file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to open for reading.</param>
      /// <returns>All lines of the file.</returns>
      [SecurityCritical]
      public static string ReadAllText(KernelTransaction transaction, string path)
      {
         return ReadAllTextInternal(transaction, path, NativeMethods.DefaultFileEncoding, false);
      }

      /// <summary>[AlphaFS] Opens a file, reads all lines of the file with the specified encoding, and then closes the file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to open for reading.</param>
      /// <param name="encoding">The <see cref="T:Encoding"/> applied to the contents of the file.</param>
      /// <returns>All lines of the file.</returns>
      [SecurityCritical]
      public static string ReadAllText(KernelTransaction transaction, string path, Encoding encoding)
      {
         return ReadAllTextInternal(transaction, path, encoding, false);
      }

      #endregion // .NET

      #region IsFullPath

      /// <summary>[AlphaFS] Opens a text file, reads all lines of the file, and then closes the file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to open for reading.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>All lines of the file.</returns>
      [SecurityCritical]
      public static string ReadAllText(KernelTransaction transaction, string path, bool? isFullPath)
      {
         return ReadAllTextInternal(transaction, path, NativeMethods.DefaultFileEncoding, isFullPath);
      }

      /// <summary>[AlphaFS] Opens a file, reads all lines of the file with the specified encoding, and then closes the file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to open for reading.</param>
      /// <param name="encoding">The <see cref="T:Encoding"/> applied to the contents of the file.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>All lines of the file.</returns>
      [SecurityCritical]
      public static string ReadAllText(KernelTransaction transaction, string path, Encoding encoding, bool? isFullPath)
      {
         return ReadAllTextInternal(transaction, path, encoding, isFullPath);
      }
      
      #endregion // IsFullPath
      
      #endregion // Transacted

      #endregion // AlphaFS

      #endregion // ReadAllText

      #region ReadLines

      #region .NET

      /// <summary>Reads the lines of a file.</summary>
      /// <param name="path">The file to read.</param>
      /// <returns>All the lines of the file, or the lines that are the result of a query.</returns>
      [SecurityCritical]
      public static IEnumerable<string> ReadLines(string path)
      {
         return ReadLinesInternal(null, path, NativeMethods.DefaultFileEncoding, false);
      }

      /// <summary>Read the lines of a file that has a specified encoding.</summary>
      /// <param name="path">The file to read.</param>
      /// <param name="encoding">The encoding that is applied to the contents of the file.</param>
      /// <returns>All the lines of the file, or the lines that are the result of a query.</returns>
      [SecurityCritical]
      public static IEnumerable<string> ReadLines(string path, Encoding encoding)
      {
         return ReadLinesInternal(null, path, encoding, false);
      }

      #endregion // .NET

      #region AlphaFS

      #region IsFullPath

      /// <summary>[AlphaFS] Reads the lines of a file.</summary>
      /// <param name="path">The file to read.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>All the lines of the file, or the lines that are the result of a query.</returns>
      [SecurityCritical]
      public static IEnumerable<string> ReadLines(string path, bool? isFullPath)
      {
         return ReadLinesInternal(null, path, NativeMethods.DefaultFileEncoding, isFullPath);
      }

      /// <summary>[AlphaFS] Read the lines of a file that has a specified encoding.</summary>
      /// <param name="path">The file to read.</param>
      /// <param name="encoding">The encoding that is applied to the contents of the file.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>All the lines of the file, or the lines that are the result of a query.</returns>
      [SecurityCritical]
      public static IEnumerable<string> ReadLines(string path, Encoding encoding, bool? isFullPath)
      {
         return ReadLinesInternal(null, path, encoding, isFullPath);
      }

      #endregion // IsFullPath

      #region Transacted

      #region .NET

      /// <summary>[AlphaFS] Reads the lines of a file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to read.</param>
      /// <returns>All the lines of the file, or the lines that are the result of a query.</returns>
      [SecurityCritical]
      public static IEnumerable<string> ReadLines(KernelTransaction transaction, string path)
      {
         return ReadLinesInternal(transaction, path, NativeMethods.DefaultFileEncoding, false);
      }

      /// <summary>[AlphaFS] Read the lines of a file that has a specified encoding.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to read.</param>
      /// <param name="encoding">The encoding that is applied to the contents of the file.</param>
      /// <returns>All the lines of the file, or the lines that are the result of a query.</returns>
      [SecurityCritical]
      public static IEnumerable<string> ReadLines(KernelTransaction transaction, string path, Encoding encoding)
      {
         return ReadLinesInternal(transaction, path, encoding, false);
      }

      #endregion // .NET

      #region IsFullPath

      /// <summary>[AlphaFS] Reads the lines of a file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to read.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>All the lines of the file, or the lines that are the result of a query.</returns>
      [SecurityCritical]
      public static IEnumerable<string> ReadLines(KernelTransaction transaction, string path, bool? isFullPath)
      {
         return ReadLinesInternal(transaction, path, NativeMethods.DefaultFileEncoding, isFullPath);
      }

      /// <summary>[AlphaFS] Read the lines of a file that has a specified encoding.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to read.</param>
      /// <param name="encoding">The encoding that is applied to the contents of the file.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>All the lines of the file, or the lines that are the result of a query.</returns>
      [SecurityCritical]
      public static IEnumerable<string> ReadLines(KernelTransaction transaction, string path, Encoding encoding, bool? isFullPath)
      {
         return ReadLinesInternal(transaction, path, encoding, isFullPath);
      }

      #endregion // IsFullPath

      #endregion // Transacted

      #endregion // AlphaFS

      #endregion // ReadLines

      #region Replace

      #region .NET

      /// <summary>Replaces one file with another file, with the option of creating a backup copy of the original file. The replacement file assumes the name of the replaced file and its identity.</summary>
      /// <param name="sourceFileName">The name of a file that replaces the file specified by <paramref name="destFileName"/>.</param>
      /// <param name="destFileName">The name of the file being replaced.</param>
      /// <param name="destinationBackupFileName">The name of the backup file.</param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "dest")]
      [SecurityCritical]
      public static void Replace(string sourceFileName, string destFileName, string destinationBackupFileName)
      {
         ReplaceInternal(sourceFileName, destFileName, destinationBackupFileName, false, false);
      }

      /// <summary>Replaces one file with another file, with the option of creating a backup copy of the original file. The replacement file assumes the name of the replaced file and its identity.</summary>
      /// <param name="sourceFileName">The name of a file that replaces the file specified by <paramref name="destFileName"/>.</param>
      /// <param name="destFileName">The name of the file being replaced.</param>
      /// <param name="destinationBackupFileName">The name of the backup file.</param>
      /// <param name="ignoreMetadataErrors"><c>true</c> to ignore merge errors (such as attributes and access control lists (ACLs)) from the replaced file to the replacement file; <c>false</c> otherwise.</param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "dest")]
      [SecurityCritical]
      public static void Replace(string sourceFileName, string destFileName, string destinationBackupFileName, bool ignoreMetadataErrors)
      {
         ReplaceInternal(sourceFileName, destFileName, destinationBackupFileName, ignoreMetadataErrors, false);
      }

      #endregion // .NET

      #region AlphaFS

      #region IsFullPath

      /// <summary>[AlphaFS] Replaces one file with another file, with the option of creating a backup copy of the original file. The replacement file assumes the name of the replaced file and its identity.</summary>
      /// <param name="sourceFileName">The name of a file that replaces the file specified by <paramref name="destFileName"/>.</param>
      /// <param name="destFileName">The name of the file being replaced.</param>
      /// <param name="destinationBackupFileName">The name of the backup file.</param>
      /// <param name="ignoreMetadataErrors"><c>true</c> to ignore merge errors (such as attributes and access control lists (ACLs)) from the replaced file to the replacement file; <c>false</c> otherwise.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="sourceFileName"/> and <paramref name="destFileName"/> will be normalized and long path prefixed. <c>null</c> <paramref name="sourceFileName"/> and <paramref name="destFileName"/> are already a full path with long path prefix, will be used as is.</param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "dest")]
      [SecurityCritical]
      public static void Replace(string sourceFileName, string destFileName, string destinationBackupFileName, bool ignoreMetadataErrors, bool? isFullPath)
      {
         ReplaceInternal(sourceFileName, destFileName, destinationBackupFileName, ignoreMetadataErrors, isFullPath);
      }

      #endregion // IsFullPath

      #endregion // AlphaFS

      #endregion // Replace

      #region SetAccessControl

      #region .NET

      /// <summary>Applies access control list (ACL) entries described by a <see cref="T:FileSecurity"/> FileSecurity object to the specified file.</summary>
      /// <param name="path">A file to add or remove access control list (ACL) entries from.</param>
      /// <param name="fileSecurity">A  <see cref="T:FileSecurity"/> object that describes an ACL entry to apply to the file described by the <paramref name="path"/> parameter.</param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
      [SecurityCritical]
      public static void SetAccessControl(string path, FileSecurity fileSecurity)
      {
         SetAccessControlInternal(path, null, fileSecurity, AccessControlSections.All, false);
      }

      /// <summary>Applies access control list (ACL) entries described by a <see cref="T:DirectorySecurity"/> object to the specified directory.</summary>
      /// <param name="path">A directory to add or remove access control list (ACL) entries from.</param>
      /// <param name="fileSecurity">A <see cref="T:FileSecurity "/> object that describes an ACL entry to apply to the directory described by the path parameter.</param>
      /// <param name="includeSections">One or more of the <see cref="T:AccessControlSections"/> values that specifies the type of access control list (ACL) information to set.</param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
      [SecurityCritical]
      public static void SetAccessControl(string path, FileSecurity fileSecurity, AccessControlSections includeSections)
      {
         SetAccessControlInternal(path, null, fileSecurity, includeSections, false);
      }

      #endregion // .NET

      #region AlphaFS

      #region IsFullPath

      /// <summary>[AlphaFS] Applies access control list (ACL) entries described by a <see cref="T:FileSecurity"/> FileSecurity object to the specified file.</summary>
      /// <param name="path">A file to add or remove access control list (ACL) entries from.</param>
      /// <param name="fileSecurity">A  <see cref="T:FileSecurity"/> object that describes an ACL entry to apply to the file described by the <paramref name="path"/> parameter.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
      [SecurityCritical]
      public static void SetAccessControl(string path, FileSecurity fileSecurity, bool? isFullPath)
      {
         SetAccessControlInternal(path, null, fileSecurity, AccessControlSections.All, isFullPath);
      }

      /// <summary>[AlphaFS] Applies access control list (ACL) entries described by a <see cref="T:DirectorySecurity"/> object to the specified directory.</summary>
      /// <param name="path">A directory to add or remove access control list (ACL) entries from.</param>
      /// <param name="fileSecurity">A <see cref="T:FileSecurity "/> object that describes an ACL entry to apply to the directory described by the path parameter.</param>
      /// <param name="includeSections">One or more of the <see cref="T:AccessControlSections"/> values that specifies the type of access control list (ACL) information to set.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
      [SecurityCritical]
      public static void SetAccessControl(string path, FileSecurity fileSecurity, AccessControlSections includeSections, bool? isFullPath)
      {
         SetAccessControlInternal(path, null, fileSecurity, includeSections, isFullPath);
      }

      #endregion // IsFullPath

      #endregion // AlphaFS

      #endregion // SetAccessControl

      #region SetAttributes

      #region .NET

      /// <summary>Sets the specified <see cref="T:FileAttributes"/> of the file or directory on the specified path.</summary>
      /// <param name="path">The path to the file or directory.</param>
      /// <param name="fileAttributes">A bitwise combination of the enumeration values. </param>
      /// <remarks>It is not possible to change the compression status of a File object using the SetAttributes method.</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static void SetAttributes(string path, FileAttributes fileAttributes)
      {
         SetAttributesInternal(false, null, path, fileAttributes, false, false);
      }

      #endregion // .NET

      #region AlphaFS

      #region IsFullPath

      /// <summary>[AlphaFS] Sets the specified <see cref="T:FileAttributes"/> of the file or directory on the specified path.</summary>
      /// <param name="path">The path to the file or directory.</param>
      /// <param name="fileAttributes">A bitwise combination of the enumeration values. </param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <remarks>It is not possible to change the compression status of a File object using the SetAttributes method.</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static void SetAttributes(string path, FileAttributes fileAttributes, bool? isFullPath)
      {
         SetAttributesInternal(false, null, path, fileAttributes, false, isFullPath);
      }

      #endregion // IsFullPath

      #region Transacted

      #region .NET

      /// <summary>[AlphaFS] Sets the specified <see cref="T:FileAttributes"/> of the file on the specified path.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to the file.</param>
      /// <param name="fileAttributes">A bitwise combination of the enumeration values. </param>
      /// <remarks>It is not possible to change the compression status of a File object using the <see cref="M:SetAttributes"/> method.</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static void SetAttributes(KernelTransaction transaction, string path, FileAttributes fileAttributes)
      {
         SetAttributesInternal(false, transaction, path, fileAttributes, false, false);
      }

      #endregion // .NET

      #region IsFullPath

      /// <summary>[AlphaFS] Sets the specified <see cref="T:FileAttributes"/> of the file on the specified path.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to the file.</param>
      /// <param name="fileAttributes">A bitwise combination of the enumeration values. </param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <remarks>It is not possible to change the compression status of a File object using the <see cref="M:SetAttributes"/> method.</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static void SetAttributes(KernelTransaction transaction, string path, FileAttributes fileAttributes, bool? isFullPath)
      {
         SetAttributesInternal(false, transaction, path, fileAttributes, false, isFullPath);
      }

      #endregion // IsFullPath

      #endregion // Transacted

      #endregion // AlphaFS

      #endregion // SetAttributes

      #region SetCreationTime

      #region .NET

      /// <summary>Sets the date and time the file was created.</summary>
      /// <param name="path">The file for which to set the creation date and time information.</param>
      /// <param name="creationTime">A <see cref="T:System.DateTime"/> containing the value to set for the creation date and time of <paramref name="path"/>. This value is expressed in local time.</param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static void SetCreationTime(string path, DateTime creationTime)
      {
         SetFsoDateTimeInternal(false, null, path, creationTime.ToUniversalTime(), null, null, false);
      }

      #endregion // .NET

      #region AlphaFS

      #region IsFullPath

      /// <summary>[AlphaFS] Sets the date and time the file was created.</summary>
      /// <param name="path">The file for which to set the creation date and time information.</param>
      /// <param name="creationTime">A <see cref="T:System.DateTime"/> containing the value to set for the creation date and time of <paramref name="path"/>. This value is expressed in local time.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static void SetCreationTime(string path, DateTime creationTime, bool? isFullPath)
      {
         SetFsoDateTimeInternal(false, null, path, creationTime.ToUniversalTime(), null, null, isFullPath);
      }

      #endregion // IsFullPath

      #region Transacted

      #region .NET

      /// <summary>[AlphaFS] Sets the date and time the file was created.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file for which to set the creation date and time information.</param>
      /// <param name="creationTime">A <see cref="T:System.DateTime"/> containing the value to set for the creation date and time of <paramref name="path"/>. This value is expressed in local time.</param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static void SetCreationTime(KernelTransaction transaction, string path, DateTime creationTime)
      {
         SetFsoDateTimeInternal(false, transaction, path, creationTime.ToUniversalTime(), null, null, false);
      }

      #endregion // .NET

      #region IsFullPath

      /// <summary>[AlphaFS] Sets the date and time the file was created.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file for which to set the creation date and time information.</param>
      /// <param name="creationTime">A <see cref="T:System.DateTime"/> containing the value to set for the creation date and time of <paramref name="path"/>. This value is expressed in local time.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static void SetCreationTime(KernelTransaction transaction, string path, DateTime creationTime, bool? isFullPath)
      {
         SetFsoDateTimeInternal(false, transaction, path, creationTime.ToUniversalTime(), null, null, isFullPath);
      }

      #endregion // IsFullPath

      #endregion // Transacted

      #endregion // AlphaFS

      #endregion // SetCreationTime

      #region SetCreationTimeUtc

      #region .NET

      /// <summary>Sets the date and time, in coordinated universal time (UTC), that the file was created.</summary>
      /// <param name="path">The file for which to set the creation date and time information.</param>
      /// <param name="creationTimeUtc">A <see cref="T:System.DateTime"/> containing the value to set for the creation date and time of <paramref name="path"/>. This value is expressed in UTC time.</param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static void SetCreationTimeUtc(string path, DateTime creationTimeUtc)
      {
         SetFsoDateTimeInternal(false, null, path, creationTimeUtc, null, null, false);
      }

      #endregion // .NET

      #region AlphaFS

      #region IsFullPath

      /// <summary>[AlphaFS] Sets the date and time, in coordinated universal time (UTC), that the file was created.</summary>
      /// <param name="path">The file for which to set the creation date and time information.</param>
      /// <param name="creationTimeUtc">A <see cref="T:System.DateTime"/> containing the value to set for the creation date and time of <paramref name="path"/>. This value is expressed in UTC time.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static void SetCreationTimeUtc(string path, DateTime creationTimeUtc, bool? isFullPath)
      {
         SetFsoDateTimeInternal(false, null, path, creationTimeUtc, null, null, isFullPath);
      }

      #endregion // IsFullPath

      #region Transacted

      #region .NET

      /// <summary>[AlphaFS] Sets the date and time, in coordinated universal time (UTC), that the file was created.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file for which to set the creation date and time information.</param>
      /// <param name="creationTimeUtc">A <see cref="T:System.DateTime"/> containing the value to set for the creation date and time of <paramref name="path"/>. This value is expressed in UTC time.</param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static void SetCreationTimeUtc(KernelTransaction transaction, string path, DateTime creationTimeUtc)
      {
         SetFsoDateTimeInternal(false, transaction, path, creationTimeUtc, null, null, false);
      }

      #endregion // .NET

      #region IsFullPath

      /// <summary>[AlphaFS] Sets the date and time, in coordinated universal time (UTC), that the file was created.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file for which to set the creation date and time information.</param>
      /// <param name="creationTimeUtc">A <see cref="T:System.DateTime"/> containing the value to set for the creation date and time of <paramref name="path"/>. This value is expressed in UTC time.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static void SetCreationTimeUtc(KernelTransaction transaction, string path, DateTime creationTimeUtc, bool? isFullPath)
      {
         SetFsoDateTimeInternal(false, transaction, path, creationTimeUtc, null, null, isFullPath);
      }

      #endregion // IsFullPath
      
      #endregion // Transacted

      #endregion // AlphaFS

      #endregion // SetCreationTimeUtc

      #region SetLastAccessTime

      #region .NET

      /// <summary>Sets the date and time that the specified file was last accessed.</summary>
      /// <param name="path">The file for which to set the access date and time information.</param>
      /// <param name="lastAccessTime">A <see cref="T:System.DateTime"/> containing the value to set for the last access date and time of <paramref name="path"/>. This value is expressed in local time.</param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static void SetLastAccessTime(string path, DateTime lastAccessTime)
      {
         SetFsoDateTimeInternal(false, null, path, null, lastAccessTime.ToUniversalTime(), null, false);
      }

      #endregion // .NET

      #region AlphaFS

      #region IsFullPath

      /// <summary>[AlphaFS] Sets the date and time that the specified file was last accessed.</summary>
      /// <param name="path">The file for which to set the access date and time information.</param>
      /// <param name="lastAccessTime">A <see cref="T:System.DateTime"/> containing the value to set for the last access date and time of <paramref name="path"/>. This value is expressed in local time.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static void SetLastAccessTime(string path, DateTime lastAccessTime, bool? isFullPath)
      {
         SetFsoDateTimeInternal(false, null, path, null, lastAccessTime.ToUniversalTime(), null, isFullPath);
      }

      #endregion // IsFullPath

      #region Transaction

      #region .NET

      /// <summary>[AlphaFS] Sets the date and time that the specified file was last accessed.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file for which to set the access date and time information.</param>
      /// <param name="lastAccessTime">A <see cref="T:System.DateTime"/> containing the value to set for the last access date and time of <paramref name="path"/>. This value is expressed in local time.</param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static void SetLastAccessTime(KernelTransaction transaction, string path, DateTime lastAccessTime)
      {
         SetFsoDateTimeInternal(false, transaction, path, null, lastAccessTime.ToUniversalTime(), null, false);
      }

      #endregion // .NET

      #region IsFullPath

      /// <summary>[AlphaFS] Sets the date and time that the specified file was last accessed.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file for which to set the access date and time information.</param>
      /// <param name="lastAccessTime">A <see cref="T:System.DateTime"/> containing the value to set for the last access date and time of <paramref name="path"/>. This value is expressed in local time.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static void SetLastAccessTime(KernelTransaction transaction, string path, DateTime lastAccessTime, bool? isFullPath)
      {
         SetFsoDateTimeInternal(false, transaction, path, null, lastAccessTime.ToUniversalTime(), null, isFullPath);
      }

      #endregion // IsFullPath

      #endregion // Transaction

      #endregion // AlphaFS

      #endregion // SetLastAccessTime

      #region SetLastAccessTimeUtc

      #region .NET

      /// <summary>Sets the date and time, in coordinated universal time (UTC), that the specified file was last accessed.</summary>
      /// <param name="path">The file for which to set the access date and time information.</param>
      /// <param name="lastAccessTimeUtc">A <see cref="T:System.DateTime"/> containing the value to set for the last access date and time of <paramref name="path"/>. This value is expressed in UTC time.</param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static void SetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc)
      {
         SetFsoDateTimeInternal(false, null, path, null, lastAccessTimeUtc, null, false);
      }

      #endregion // .NET

      #region AlphaFS

      #region IsFullPath

      /// <summary>[AlphaFS] Sets the date and time, in coordinated universal time (UTC), that the specified file was last accessed.</summary>
      /// <param name="path">The file for which to set the access date and time information.</param>
      /// <param name="lastAccessTimeUtc">A <see cref="T:System.DateTime"/> containing the value to set for the last access date and time of <paramref name="path"/>. This value is expressed in UTC time.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static void SetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc, bool? isFullPath)
      {
         SetFsoDateTimeInternal(false, null, path, null, lastAccessTimeUtc, null, isFullPath);
      }

      #endregion // IsFullPath

      #region Transacted

      #region .NET

      /// <summary>[AlphaFS] Sets the date and time, in coordinated universal time (UTC), that the specified file was last accessed.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file for which to set the access date and time information.</param>
      /// <param name="lastAccessTimeUtc">A <see cref="T:System.DateTime"/> containing the value to set for the last access date and time of <paramref name="path"/>. This value is expressed in UTC time.</param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static void SetLastAccessTimeUtc(KernelTransaction transaction, string path, DateTime lastAccessTimeUtc)
      {
         SetFsoDateTimeInternal(false, transaction, path, null, lastAccessTimeUtc, null, false);
      }

      #endregion // .NET

      #region IsFullPath

      /// <summary>[AlphaFS] Sets the date and time, in coordinated universal time (UTC), that the specified file was last accessed.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file for which to set the access date and time information.</param>
      /// <param name="lastAccessTimeUtc">A <see cref="T:System.DateTime"/> containing the value to set for the last access date and time of <paramref name="path"/>. This value is expressed in UTC time.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static void SetLastAccessTimeUtc(KernelTransaction transaction, string path, DateTime lastAccessTimeUtc, bool? isFullPath)
      {
         SetFsoDateTimeInternal(false, transaction, path, null, lastAccessTimeUtc, null, isFullPath);
      }

      #endregion // IsFullPath
      
      #endregion // Transacted

      #endregion // AlphaFS

      #endregion // SetLastAccessTimeUtc

      #region SetLastWriteTime

      #region .NET

      /// <summary>Sets the date and time that the specified file was last written to.</summary>
      /// <param name="path">The file for which to set the date and time information.</param>
      /// <param name="lastWriteTime">A <see cref="T:System.DateTime"/> containing the value to set for the last write date and time of <paramref name="path"/>. This value is expressed in local time.</param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static void SetLastWriteTime(string path, DateTime lastWriteTime)
      {
         SetFsoDateTimeInternal(false, null, path, null, null, lastWriteTime.ToUniversalTime(), false);
      }

      #endregion // .NET

      #region AlphaFS

      #region IsFullPath

      /// <summary>[AlphaFS] Sets the date and time that the specified file was last written to.</summary>
      /// <param name="path">The file for which to set the date and time information.</param>
      /// <param name="lastWriteTime">A <see cref="T:System.DateTime"/> containing the value to set for the last write date and time of <paramref name="path"/>. This value is expressed in local time.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static void SetLastWriteTime(string path, DateTime lastWriteTime, bool? isFullPath)
      {
         SetFsoDateTimeInternal(false, null, path, null, null, lastWriteTime.ToUniversalTime(), isFullPath);
      }

      #endregion // IsFullPath

      #region Transacted

      #region .NET

      /// <summary>[AlphaFS] Sets the date and time that the specified file was last written to.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file for which to set the date and time information.</param>
      /// <param name="lastWriteTime">A <see cref="T:System.DateTime"/> containing the value to set for the last write date and time of <paramref name="path"/>. This value is expressed in local time.</param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static void SetLastWriteTime(KernelTransaction transaction, string path, DateTime lastWriteTime)
      {
         SetFsoDateTimeInternal(false, transaction, path, null, null, lastWriteTime.ToUniversalTime(), false);
      }

      #endregion // .NET

      #region IsFullPath

      /// <summary>[AlphaFS] Sets the date and time that the specified file was last written to.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file for which to set the date and time information.</param>
      /// <param name="lastWriteTime">A <see cref="T:System.DateTime"/> containing the value to set for the last write date and time of <paramref name="path"/>. This value is expressed in local time.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static void SetLastWriteTime(KernelTransaction transaction, string path, DateTime lastWriteTime, bool? isFullPath)
      {
         SetFsoDateTimeInternal(false, transaction, path, null, null, lastWriteTime.ToUniversalTime(), isFullPath);
      }

      #endregion // IsFullPath

      #endregion // Transacted

      #endregion // AlphaFS

      #endregion // SetLastWriteTime

      #region SetLastWriteTimeUtc

      #region .NET

      /// <summary>Sets the date and time, in coordinated universal time (UTC), that the specified file was last written to.</summary>
      /// <param name="path">The file for which to set the date and time information.</param>
      /// <param name="lastWriteTimeUtc">A <see cref="T:System.DateTime"/> containing the value to set for the last write date and time of <paramref name="path"/>. This value is expressed in UTC time.</param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static void SetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc)
      {
         SetFsoDateTimeInternal(false, null, path, null, null, lastWriteTimeUtc, false);
      }

      #endregion // .NET

      #region AlphaFS

      #region IsFullPath

      /// <summary>[AlphaFS] Sets the date and time, in coordinated universal time (UTC), that the specified file was last written to.</summary>
      /// <param name="path">The file for which to set the date and time information.</param>
      /// <param name="lastWriteTimeUtc">A <see cref="T:System.DateTime"/> containing the value to set for the last write date and time of <paramref name="path"/>. This value is expressed in UTC time.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static void SetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc, bool? isFullPath)
      {
         SetFsoDateTimeInternal(false, null, path, null, null, lastWriteTimeUtc, isFullPath);
      }

      #endregion // IsFullPath

      #region Transacted

      #region .NET

      /// <summary>[AlphaFS] Sets the date and time, in coordinated universal time (UTC), that the specified file was last written to.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file for which to set the date and time information.</param>
      /// <param name="lastWriteTimeUtc">A <see cref="T:System.DateTime"/> containing the value to set for the last write date and time of <paramref name="path"/>. This value is expressed in UTC time.</param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static void SetLastWriteTimeUtc(KernelTransaction transaction, string path, DateTime lastWriteTimeUtc)
      {
         SetFsoDateTimeInternal(false, transaction, path, null, null, lastWriteTimeUtc, false);
      }

      #endregion // .NET

      #region IsFullPath

      /// <summary>[AlphaFS] Sets the date and time, in coordinated universal time (UTC), that the specified file was last written to.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file for which to set the date and time information.</param>
      /// <param name="lastWriteTimeUtc">A <see cref="T:System.DateTime"/> containing the value to set for the last write date and time of <paramref name="path"/>. This value is expressed in UTC time.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static void SetLastWriteTimeUtc(KernelTransaction transaction, string path, DateTime lastWriteTimeUtc, bool? isFullPath)
      {
         SetFsoDateTimeInternal(false, transaction, path, null, null, lastWriteTimeUtc, isFullPath);
      }

      #endregion // IsFullPath

      #endregion // Transacted

      #endregion // AlphaFS

      #endregion // SetLastWriteTimeUtc

      #region WriteAllBytes

      #region .NET

      /// <summary>Creates a new file, writes the specified byte array to the file, and then closes the file. If the target file already exists, it is overwritten.</summary>
      /// <param name="path">The file to write to.</param>
      /// <param name="bytes">The bytes to write to the file.</param>
      [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "bytes")]
      [SecurityCritical]
      public static void WriteAllBytes(string path, byte[] bytes)
      {
         WriteAllBytesInternal(null, path, bytes, false);
      }

      #endregion // .NET

      #region AlphaFS

      #region IsFullPath

      /// <summary>[AlphaFS] Creates a new file, writes the specified byte array to the file, and then closes the file. If the target file already exists, it is overwritten.</summary>
      /// <param name="path">The file to write to.</param>
      /// <param name="bytes">The bytes to write to the file.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "bytes")]
      [SecurityCritical]
      public static void WriteAllBytes(string path, byte[] bytes, bool? isFullPath)
      {
         WriteAllBytesInternal(null, path, bytes, isFullPath);
      }

      #endregion // IsFullPath

      #region Transacted

      #region .NET

      /// <summary>[AlphaFS] Creates a new file, writes the specified byte array to the file, and then closes the file. If the target file already exists, it is overwritten.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to write to.</param>
      /// <param name="bytes">The bytes to write to the file.</param>
      [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "bytes")]
      [SecurityCritical]
      public static void WriteAllBytes(KernelTransaction transaction, string path, byte[] bytes)
      {
         WriteAllBytesInternal(transaction, path, bytes, false);
      }

      #endregion // .NET

      #region IsFullPath

      /// <summary>[AlphaFS] Creates a new file, writes the specified byte array to the file, and then closes the file. If the target file already exists, it is overwritten.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to write to.</param>
      /// <param name="bytes">The bytes to write to the file.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "bytes")]
      [SecurityCritical]
      public static void WriteAllBytes(KernelTransaction transaction, string path, byte[] bytes, bool? isFullPath)
      {
         WriteAllBytesInternal(transaction, path, bytes, isFullPath);
      }

      #endregion // IsFullPath

      #endregion // Transacted

      #endregion // AlphaFS

      #endregion // WriteAllBytes

      #region WriteAllLines

      #region .NET

      /// <summary>Creates a new file, writes a collection of strings to the file, and then closes the file.</summary>
      /// <param name="path">The file to write to.</param>
      /// <param name="contents">The lines to write to the file.</param>
      /// <remarks>The default behavior of the method is to write out data by using UTF-8 encoding without a byte order mark (BOM).</remarks>
      [SecurityCritical]
      public static void WriteAllLines(string path, IEnumerable<string> contents)
      {
         WriteAppendAllLinesInternal(null, path, contents, new UTF8Encoding(false, true), false, true, false);
      }

      /// <summary>Creates a new file by using the specified encoding, writes a collection of strings to the file, and then closes the file.</summary>
      /// <param name="path">The file to write to.</param>
      /// <param name="contents">The string array to write to the file.</param>
      [SecurityCritical]
      public static void WriteAllLines(string path, string[] contents)
      {
         WriteAppendAllLinesInternal(null, path, contents, new UTF8Encoding(false, true), false, true, false);
      }

      /// <summary>Creates a new file by using the specified encoding, writes a collection of strings to the file, and then closes the file.</summary>
      /// <param name="path">The file to write to.</param>
      /// <param name="contents">The lines to write to the file.</param>
      /// <param name="encoding">The character <see cref="T:Encoding"/> to use.</param>
      [SecurityCritical]
      public static void WriteAllLines(string path, IEnumerable<string> contents, Encoding encoding)
      {
         WriteAppendAllLinesInternal(null, path, contents, encoding, false, true, false);
      }

      /// <summary>Creates a new file by using the specified encoding, writes a collection of strings to the file, and then closes the file.</summary>
      /// <param name="path">The file to write to.</param>
      /// <param name="contents">The string array to write to the file.</param>
      /// <param name="encoding">The character <see cref="T:Encoding"/> to use.</param>
      [SecurityCritical]
      public static void WriteAllLines(string path, string[] contents, Encoding encoding)
      {
         WriteAppendAllLinesInternal(null, path, contents, encoding, false, true, false);
      }

      #endregion // .NET

      #region AlphaFS

      #region IsFullPath

      /// <summary>[AlphaFS] Creates a new file, writes a collection of strings to the file, and then closes the file.</summary>
      /// <param name="path">The file to write to.</param>
      /// <param name="contents">The lines to write to the file.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <remarks>The default behavior of the method is to write out data by using UTF-8 encoding without a byte order mark (BOM).</remarks>
      [SecurityCritical]
      public static void WriteAllLines(string path, IEnumerable<string> contents, bool? isFullPath)
      {
         WriteAppendAllLinesInternal(null, path, contents, new UTF8Encoding(false, true), false, true, isFullPath);
      }

      /// <summary>[AlphaFS] Creates a new file by using the specified encoding, writes a collection of strings to the file, and then closes the file.</summary>
      /// <param name="path">The file to write to.</param>
      /// <param name="contents">The string array to write to the file.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      [SecurityCritical]
      public static void WriteAllLines(string path, string[] contents, bool? isFullPath)
      {
         WriteAppendAllLinesInternal(null, path, contents, new UTF8Encoding(false, true), false, true, isFullPath);
      }

      /// <summary>[AlphaFS] Creates a new file by using the specified encoding, writes a collection of strings to the file, and then closes the file.</summary>
      /// <param name="path">The file to write to.</param>
      /// <param name="contents">The lines to write to the file.</param>
      /// <param name="encoding">The character <see cref="T:Encoding"/> to use.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      [SecurityCritical]
      public static void WriteAllLines(string path, IEnumerable<string> contents, Encoding encoding, bool? isFullPath)
      {
         WriteAppendAllLinesInternal(null, path, contents, encoding, false, true, isFullPath);
      }

      /// <summary>[AlphaFS] Creates a new file by using the specified encoding, writes a collection of strings to the file, and then closes the file.</summary>
      /// <param name="path">The file to write to.</param>
      /// <param name="contents">The string array to write to the file.</param>
      /// <param name="encoding">The character <see cref="T:Encoding"/> to use.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      [SecurityCritical]
      public static void WriteAllLines(string path, string[] contents, Encoding encoding, bool? isFullPath)
      {
         WriteAppendAllLinesInternal(null, path, contents, encoding, false, true, isFullPath);
      }

      #endregion // IsFullPath

      #region Transacted

      #region .NET

      /// <summary>[AlphaFS] Creates a new file, writes a collection of strings to the file, and then closes the file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to write to.</param>
      /// <param name="contents">The lines to write to the file.</param>
      /// <remarks>The default behavior of the method is to write out data by using UTF-8 encoding without a byte order mark (BOM).</remarks>
      [SecurityCritical]
      public static void WriteAllLines(KernelTransaction transaction, string path, IEnumerable<string> contents)
      {
         WriteAppendAllLinesInternal(transaction, path, contents, new UTF8Encoding(false, true), false, true, false);
      }

      /// <summary>[AlphaFS] Creates a new file, writes a collection of strings to the file, and then closes the file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to write to.</param>
      /// <param name="contents">The string array to write to the file.</param>
      /// <remarks>The default behavior of the method is to write out data by using UTF-8 encoding without a byte order mark (BOM).</remarks>
      [SecurityCritical]
      public static void WriteAllLines(KernelTransaction transaction, string path, string[] contents)
      {
         WriteAppendAllLinesInternal(transaction, path, contents, new UTF8Encoding(false, true), false, true, false);
      }

      /// <summary>[AlphaFS] Creates a new file by using the specified encoding, writes a collection of strings to the file, and then closes the file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to write to.</param>
      /// <param name="contents">The lines to write to the file.</param>
      /// <param name="encoding">The character <see cref="T:Encoding"/> to use.</param>
      [SecurityCritical]
      public static void WriteAllLines(KernelTransaction transaction, string path, IEnumerable<string> contents, Encoding encoding)
      {
         WriteAppendAllLinesInternal(transaction, path, contents, encoding, false, true, false);
      }

      /// <summary>Creates a new file by using the specified encoding, writes a collection of strings to the file, and then closes the file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to write to.</param>
      /// <param name="contents">The string array to write to the file.</param>
      /// <param name="encoding">The character <see cref="T:Encoding"/> to use.</param>
      [SecurityCritical]
      public static void WriteAllLines(KernelTransaction transaction, string path, string[] contents, Encoding encoding)
      {
         WriteAppendAllLinesInternal(transaction, path, contents, encoding, false, true, false);
      }

      #endregion // .NET

      #region IsFullPath

      /// <summary>[AlphaFS] Creates a new file, writes a collection of strings to the file, and then closes the file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to write to.</param>
      /// <param name="contents">The lines to write to the file.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <remarks>The default behavior of the method is to write out data by using UTF-8 encoding without a byte order mark (BOM).</remarks>
      [SecurityCritical]
      public static void WriteAllLines(KernelTransaction transaction, string path, IEnumerable<string> contents, bool? isFullPath)
      {
         WriteAppendAllLinesInternal(transaction, path, contents, new UTF8Encoding(false, true), false, true, isFullPath);
      }

      /// <summary>[AlphaFS] Creates a new file, writes a collection of strings to the file, and then closes the file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to write to.</param>
      /// <param name="contents">The string array to write to the file.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <remarks>The default behavior of the method is to write out data by using UTF-8 encoding without a byte order mark (BOM).</remarks>
      [SecurityCritical]
      public static void WriteAllLines(KernelTransaction transaction, string path, string[] contents, bool? isFullPath)
      {
         WriteAppendAllLinesInternal(transaction, path, contents, new UTF8Encoding(false, true), false, true, isFullPath);
      }

      /// <summary>[AlphaFS] Creates a new file by using the specified encoding, writes a collection of strings to the file, and then closes the file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to write to.</param>
      /// <param name="contents">The lines to write to the file.</param>
      /// <param name="encoding">The character <see cref="T:Encoding"/> to use.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      [SecurityCritical]
      public static void WriteAllLines(KernelTransaction transaction, string path, IEnumerable<string> contents, Encoding encoding, bool? isFullPath)
      {
         WriteAppendAllLinesInternal(transaction, path, contents, encoding, false, true, isFullPath);
      }

      /// <summary>[AlphaFS] Creates a new file by using the specified encoding, writes a collection of strings to the file, and then closes the file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to write to.</param>
      /// <param name="contents">The string array to write to the file.</param>
      /// <param name="encoding">The character <see cref="T:Encoding"/> to use.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      [SecurityCritical]
      public static void WriteAllLines(KernelTransaction transaction, string path, string[] contents, Encoding encoding, bool? isFullPath)
      {
         WriteAppendAllLinesInternal(transaction, path, contents, encoding, false, true, isFullPath);
      }

      #endregion // IsFullPath
      
      #endregion // Transacted

      #endregion // AlphaFS

      #endregion // WriteAllLines

      #region WriteAllText

      #region .NET

      /// <summary>Creates a new file, writes the specified string to the file, and then closes the file. If the target file already exists, it is overwritten.</summary>
      /// <param name="path">The file to write to.</param>
      /// <param name="contents">The string to write to the file.</param>
      /// <remarks>This method uses UTF-8 encoding without a Byte-Order Mark (BOM)</remarks>
      [SecurityCritical]
      public static void WriteAllText(string path, string contents)
      {
         WriteAppendAllLinesInternal(null, path, new[] { contents }, new UTF8Encoding(false, true), false, false, false);
      }

      /// <summary>Creates a new file, writes the specified string to the file using the specified encoding, and then closes the file. If the target file already exists, it is overwritten.</summary>
      /// <param name="path">The file to write to.</param>
      /// <param name="contents">The string to write to the file.</param>
      /// <param name="encoding">The <see cref="T:Encoding"/> applied to the contents of the file.</param>
      [SecurityCritical]
      public static void WriteAllText(string path, string contents, Encoding encoding)
      {
         WriteAppendAllLinesInternal(null, path, new[] { contents }, encoding, false, false, false);
      }

      #endregion // .NET

      #region AlphaFS

      #region IsFullPath

      /// <summary>[AlphaFS] Creates a new file, writes the specified string to the file, and then closes the file. If the target file already exists, it is overwritten.</summary>
      /// <param name="path">The file to write to.</param>
      /// <param name="contents">The string to write to the file.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <remarks>This method uses UTF-8 encoding without a Byte-Order Mark (BOM)</remarks>
      [SecurityCritical]
      public static void WriteAllText(string path, string contents, bool? isFullPath)
      {
         WriteAppendAllLinesInternal(null, path, new[] { contents }, new UTF8Encoding(false, true), false, false, isFullPath);
      }

      /// <summary>[AlphaFS] Creates a new file, writes the specified string to the file using the specified encoding, and then closes the file. If the target file already exists, it is overwritten.</summary>
      /// <param name="path">The file to write to.</param>
      /// <param name="contents">The string to write to the file.</param>
      /// <param name="encoding">The <see cref="T:Encoding"/> applied to the contents of the file.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      [SecurityCritical]
      public static void WriteAllText(string path, string contents, Encoding encoding, bool? isFullPath)
      {
         WriteAppendAllLinesInternal(null, path, new[] { contents }, encoding, false, false, isFullPath);
      }
      
      #endregion // IsFullPath

      #region Transacted

      #region .NET

      /// <summary>[AlphaFS] Creates a new file as part of a transaction, write the contents to the file, and then closes the file. If the target file already exists, it is overwritten.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to write to.</param>
      /// <param name="contents">The string to write to the file.</param>
      /// <remarks>This method uses UTF-8 encoding without a Byte-Order Mark (BOM)</remarks>
      [SecurityCritical]
      public static void WriteAllText(KernelTransaction transaction, string path, string contents)
      {
         WriteAppendAllLinesInternal(transaction, path, new[] { contents }, new UTF8Encoding(false, true), false, false, false);
      }

      /// <summary>[AlphaFS] Creates a new file as part of a transaction, writes the specified string to the file using the specified encoding, and then closes the file. If the target file already exists, it is overwritten.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to write to.</param>
      /// <param name="contents">The string to write to the file.</param>
      /// <param name="encoding">The <see cref="T:Encoding"/> applied to the contents of the file.</param>
      [SecurityCritical]
      public static void WriteAllText(KernelTransaction transaction, string path, string contents, Encoding encoding)
      {
         WriteAppendAllLinesInternal(transaction, path, new[] { contents }, encoding, false, false, false);
      }

      #endregion // .NET

      #region IsFullPath

      /// <summary>[AlphaFS] Creates a new file as part of a transaction, write the contents to the file, and then closes the file. If the target file already exists, it is overwritten.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to write to.</param>
      /// <param name="contents">The string to write to the file.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <remarks>This method uses UTF-8 encoding without a Byte-Order Mark (BOM)</remarks>
      [SecurityCritical]
      public static void WriteAllText(KernelTransaction transaction, string path, string contents, bool? isFullPath)
      {
         WriteAppendAllLinesInternal(transaction, path, new[] { contents }, new UTF8Encoding(false, true), false, false, isFullPath);
      }

      /// <summary>[AlphaFS] Creates a new file as part of a transaction, writes the specified string to the file using the specified encoding, and then closes the file. If the target file already exists, it is overwritten.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to write to.</param>
      /// <param name="contents">The string to write to the file.</param>
      /// <param name="encoding">The <see cref="T:Encoding"/> applied to the contents of the file.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      [SecurityCritical]
      public static void WriteAllText(KernelTransaction transaction, string path, string contents, Encoding encoding, bool? isFullPath)
      {
         WriteAppendAllLinesInternal(transaction, path, new[] { contents }, encoding, false, false, isFullPath);
      }

      #endregion // IsFullPath

      #endregion // Transacted

      #endregion // AlphaFS

      #endregion // WriteAllText

      #endregion // .NET

      #region AlphaFS

      #region AddStream

      #region IsFullPath

      /// <summary>[AlphaFS] Adds an alternate data stream (NTFS ADS) to an existing file.</summary>
      /// <param name="path">The path to an existing file.</param>
      /// <param name="name">The name for the stream. If a stream with <paramref name="name"/> already exists, it will be overwritten.</param>
      /// <param name="contents">The lines to add to the stream.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static void AddStream(string path, string name, string[] contents, bool? isFullPath)
      {
         AlternateDataStreamInfo.AddStreamInternal(false, null, path, name, contents, isFullPath);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Adds an alternate data stream (NTFS ADS) to an existing file.</summary>
      /// <param name="path">The path to an existing file.</param>
      /// <param name="name">The name for the stream. If a stream with <paramref name="name"/> already exists, it will be overwritten.</param>
      /// <param name="contents">The lines to add to the stream.</param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static void AddStream(string path, string name, string[] contents)
      {
         AlternateDataStreamInfo.AddStreamInternal(false, null, path, name, contents, false);
      }

      #region Transacted

      #region IsFullPath

      /// <summary>[AlphaFS] Adds an alternate data stream (NTFS ADS) to an existing file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to an existing file.</param>
      /// <param name="name">The name for the stream. If a stream with <paramref name="name"/> already exists, it will be overwritten.</param>
      /// <param name="contents">The lines to add to the stream.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static void AddStream(KernelTransaction transaction, string path, string name, string[] contents, bool? isFullPath)
      {
         AlternateDataStreamInfo.AddStreamInternal(false, transaction, path, name, contents, isFullPath);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Adds an alternate data stream (NTFS ADS) to an existing file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to an existing file.</param>
      /// <param name="name">The name for the stream. If a stream with <paramref name="name"/> already exists, it will be overwritten.</param>
      /// <param name="contents">The lines to add to the stream.</param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static void AddStream(KernelTransaction transaction, string path, string name, string[] contents)
      {
         AlternateDataStreamInfo.AddStreamInternal(false, transaction, path, name, contents, false);
      }

      #endregion Transacted

      #endregion // AddStream

      #region Compress

      #region IsFullPath

      /// <summary>[AlphaFS] Compresses a file using NTFS compression.</summary>
      /// <param name="path">A path that describes a file to compress.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static void Compress(string path, bool? isFullPath)
      {
         Device.ToggleCompressionInternal(false, null, path, true, isFullPath);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Compresses a file using NTFS compression.</summary>
      /// <param name="path">A path that describes a file to compress.</param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static void Compress(string path)
      {
         Device.ToggleCompressionInternal(false, null, path, true, false);
      }

      #region Transacted

      #region IsFullPath

      /// <summary>[AlphaFS] Compresses a file using NTFS compression.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">A path that describes a file to compress.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static void Compress(KernelTransaction transaction, string path, bool? isFullPath)
      {
         Device.ToggleCompressionInternal(false, transaction, path, true, isFullPath);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Compresses a file using NTFS compression.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">A path that describes a file to compress.</param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static void Compress(KernelTransaction transaction, string path)
      {
         Device.ToggleCompressionInternal(false, transaction, path, true, false);
      }

      #endregion // Transacted

      #endregion // Compress

      #region CreateHardlink

      #region IsFullPath

      /// <summary>[AlphaFS] Establishes a hard link between an existing file and a new file. This function is only supported on the NTFS file system, and only for files, not directories.</summary>
      /// <param name="fileName">The name of the new file. This parameter cannot specify the name of a directory.</param>
      /// <param name="existingFileName">The name of the existing file. This parameter cannot specify the name of a directory.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="fileName"/> and <paramref name="existingFileName"/> will be normalized and long path prefixed. <c>null</c> <paramref name="fileName"/> and <paramref name="existingFileName"/> are already a full path with long path prefix, will be used as is.</param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Hardlink")]
      [SecurityCritical]
      public static void CreateHardlink(string fileName, string existingFileName, bool? isFullPath)
      {
         CreateHardlinkInternal(null, fileName, existingFileName, isFullPath);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Establishes a hard link between an existing file and a new file. This function is only supported on the NTFS file system, and only for files, not directories.</summary>
      /// <param name="fileName">The name of the new file. This parameter cannot specify the name of a directory.</param>
      /// <param name="existingFileName">The name of the existing file. This parameter cannot specify the name of a directory.</param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Hardlink")]
      [SecurityCritical]
      public static void CreateHardlink(string fileName, string existingFileName)
      {
         CreateHardlinkInternal(null, fileName, existingFileName, false);
      }

      #region Transacted

      #region IsFullPath

      /// <summary>[AlphaFS] Establishes a hard link between an existing file and a new file. This function is only supported on the NTFS file system, and only for files, not directories.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="fileName">The name of the new file. This parameter cannot specify the name of a directory.</param>
      /// <param name="existingFileName">The name of the existing file. This parameter cannot specify the name of a directory.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="fileName"/> and <paramref name="existingFileName"/> will be normalized and long path prefixed. <c>null</c> <paramref name="fileName"/> and <paramref name="existingFileName"/> are already a full path with long path prefix, will be used as is.</param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Hardlink")]
      [SecurityCritical]
      public static void CreateHardlink(KernelTransaction transaction, string fileName, string existingFileName, bool? isFullPath)
      {
         CreateHardlinkInternal(transaction, fileName, existingFileName, isFullPath);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Establishes a hard link between an existing file and a new file. This function is only supported on the NTFS file system, and only for files, not directories.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="fileName">The name of the new file. This parameter cannot specify the name of a directory.</param>
      /// <param name="existingFileName">The name of the existing file. This parameter cannot specify the name of a directory.</param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Hardlink")]
      [SecurityCritical]
      public static void CreateHardlink(KernelTransaction transaction, string fileName, string existingFileName)
      {
         CreateHardlinkInternal(transaction, fileName, existingFileName, false);
      }

      #endregion // Transacted

      #endregion // CreateHardlink

      #region CreateSymbolicLink

      #region IsFullPath

      /// <summary>[AlphaFS] Creates a symbolic link.</summary>
      /// <param name="symlinkFileName">The name of the target for the symbolic link to be created.</param>
      /// <param name="targetFileName">The symbolic link to be created.</param>
      /// <param name="targetType">Indicates whether the link target, <paramref name="targetFileName"/>, is a file or directory.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="symlinkFileName"/> and <paramref name="targetFileName"/> will be normalized and long path prefixed. <c>null</c> <paramref name="symlinkFileName"/> and <paramref name="targetFileName"/> are already a full path with long path prefix, will be used as is.</param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "symlink")]
      [SecurityCritical]
      public static void CreateSymbolicLink(string symlinkFileName, string targetFileName, SymbolicLinkTarget targetType, bool? isFullPath)
      {
         CreateSymbolicLinkInternal(null, symlinkFileName, targetFileName, targetType, isFullPath);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Creates a symbolic link.</summary>
      /// <param name="symlinkFileName">The name of the target for the symbolic link to be created.</param>
      /// <param name="targetFileName">The symbolic link to be created.</param>
      /// <param name="targetType">Indicates whether the link target, <paramref name="targetFileName"/>, is a file or directory.</param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "symlink")]
      [SecurityCritical]
      public static void CreateSymbolicLink(string symlinkFileName, string targetFileName, SymbolicLinkTarget targetType)
      {
         CreateSymbolicLinkInternal(null, symlinkFileName, targetFileName, targetType, false);
      }

      #region Transacted

      #region IsFullPath

      /// <summary>[AlphaFS] Creates a symbolic link.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="symlinkFileName">The name of the target for the symbolic link to be created.</param>
      /// <param name="targetFileName">The symbolic link to be created.</param>
      /// <param name="targetType">Indicates whether the link target, <paramref name="targetFileName"/>, is a file or directory.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="symlinkFileName"/> and <paramref name="targetFileName"/> will be normalized and long path prefixed. <c>null</c> <paramref name="symlinkFileName"/> and <paramref name="targetFileName"/> are already a full path with long path prefix, will be used as is.</param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "symlink")]
      [SecurityCritical]
      public static void CreateSymbolicLink(KernelTransaction transaction, string symlinkFileName, string targetFileName, SymbolicLinkTarget targetType, bool? isFullPath)
      {
         CreateSymbolicLinkInternal(transaction, symlinkFileName, targetFileName, targetType, isFullPath);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Creates a symbolic link.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="symlinkFileName">The name of the target for the symbolic link to be created.</param>
      /// <param name="targetFileName">The symbolic link to be created.</param>
      /// <param name="targetType">Indicates whether the link target, <paramref name="targetFileName"/>, is a file or directory.</param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "symlink")]
      [SecurityCritical]
      public static void CreateSymbolicLink(KernelTransaction transaction, string symlinkFileName, string targetFileName, SymbolicLinkTarget targetType)
      {
         CreateSymbolicLinkInternal(transaction, symlinkFileName, targetFileName, targetType, false);
      }

      #endregion // Transacted

      #endregion // CreateSymbolicLink

      #region Decompress

      #region IsFullPath

      /// <summary>[AlphaFS] Decompresses an NTFS compressed file.</summary>
      /// <param name="path">A path that describes a file to decompress.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static void Decompress(string path, bool? isFullPath)
      {
         Device.ToggleCompressionInternal(false, null, path, false, isFullPath);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Decompresses an NTFS compressed file.</summary>
      /// <param name="path">A path that describes a file to decompress.</param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static void Decompress(string path)
      {
         Device.ToggleCompressionInternal(false, null, path, false, false);
      }

      #region Transacted

      #region IsFullPath

      /// <summary>[AlphaFS] Decompresses an NTFS compressed file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">A path that describes a file to decompress.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static void Decompress(KernelTransaction transaction, string path, bool? isFullPath)
      {
         Device.ToggleCompressionInternal(false, transaction, path, false, isFullPath);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Decompresses an NTFS compressed file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">A path that describes a file to decompress.</param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static void Decompress(KernelTransaction transaction, string path)
      {
         Device.ToggleCompressionInternal(false, transaction, path, false, false);
      }

      #endregion // Transacted

      #endregion // Decompress

      #region EnumerateFileSystemEntryInfos

      #region IsFullPath

      /// <summary>[AlphaFS] Returns an enumerable collection of file instances and directory instances in a specified path.</summary>
      /// <param name="path">The directory to search.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>An enumerable collection of file-system entries in the directory specified by <paramref name="path"/>.</returns>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
      [SecurityCritical]
      public static IEnumerable<FileSystemEntryInfo> EnumerateFileSystemEntryInfos(string path, bool? isFullPath)
      {
         return EnumerateFileSystemEntryInfoInternal<FileSystemEntryInfo>(null, path, Path.WildcardStarMatchAll, SearchOption.TopDirectoryOnly, null, null, false, false, isFullPath);
      }

      /// <summary>[AlphaFS] Returns an enumerable collection of file instances and directory instances in a specified path.</summary>
      /// <param name="path">The directory to search.</param>
      /// <param name="continueOnException"><c>true</c> suppress any Exception that might be thrown a result from a failure, such as ACLs protected directories or non-accessible reparse points.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>An enumerable collection of file-system entries in the directory specified by <paramref name="path"/>.</returns>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
      [
         SecurityCritical]
      public static IEnumerable<FileSystemEntryInfo> EnumerateFileSystemEntryInfos(string path, bool continueOnException, bool? isFullPath)
      {
         return EnumerateFileSystemEntryInfoInternal<FileSystemEntryInfo>(null, path, Path.WildcardStarMatchAll, SearchOption.TopDirectoryOnly, null, null, false, continueOnException, isFullPath);
      }

      /// <summary>[AlphaFS] Returns an enumerable collection of file instances and directory instances  that match a <paramref name="searchPattern"/> in a specified path.</summary>
      /// <param name="path">The directory to search.</param>
      /// <param name="searchPattern">The search string to match against the names of directories in <paramref name="path"/>. This parameter can contain a combination of valid literal path and wildcard (<see cref="T:Path.WildcardStarMatchAll"/> and <see cref="T:Path.WildcardQuestion"/>) characters, but doesn't support regular expressions.</param>
      /// <param name="continueOnException"><c>true</c> suppress any Exception that might be thrown a result from a failure, such as ACLs protected directories or non-accessible reparse points.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>An enumerable collection of file-system entries in the directory specified by <paramref name="path"/> and that match the specified <paramref name="searchPattern"/>.</returns>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
      [SecurityCritical]
      public static IEnumerable<FileSystemEntryInfo> EnumerateFileSystemEntryInfos(string path, string searchPattern, bool continueOnException, bool? isFullPath)
      {
         return EnumerateFileSystemEntryInfoInternal<FileSystemEntryInfo>(null, path, searchPattern, SearchOption.TopDirectoryOnly, null, null, false, continueOnException, isFullPath);
      }

      /// <summary>[AlphaFS] Returns an enumerable collection of file instances and directory instances  that match a <paramref name="searchPattern"/> in a specified path, and optionally searches subdirectories.</summary>
      /// <param name="path">The directory to search.</param>
      /// <param name="searchPattern">The search string to match against the names of directories in <paramref name="path"/>. This parameter can contain a combination of valid literal path and wildcard (<see cref="T:Path.WildcardStarMatchAll"/> and <see cref="T:Path.WildcardQuestion"/>) characters, but doesn't support regular expressions.</param>
      /// <param name="searchOption">One of the <see cref="T:SearchOption"/> enumeration values that specifies whether the <paramref name="searchOption"/> should include only the current directory or should include all subdirectories.</param>
      /// <param name="continueOnException"><c>true</c> suppress any Exception that might be thrown a result from a failure, such as ACLs protected directories or non-accessible reparse points.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>An enumerable collection of file-system entries in the directory specified by <paramref name="path"/> and that match the specified <paramref name="searchPattern"/> and <paramref name="searchOption"/>.</returns>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
      [SecurityCritical]
      public static IEnumerable<FileSystemEntryInfo> EnumerateFileSystemEntryInfos(string path, string searchPattern, SearchOption searchOption, bool continueOnException, bool? isFullPath)
      {
         return EnumerateFileSystemEntryInfoInternal<FileSystemEntryInfo>(null, path, searchPattern, searchOption, null, null, false, continueOnException, isFullPath);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Returns an enumerable collection of file instances and directory instances in a specified path.</summary>
      /// <param name="path">The directory to search.</param>
      /// <returns>An enumerable collection of file-system entries in the directory specified by <paramref name="path"/>.</returns>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
      [SecurityCritical]
      public static IEnumerable<FileSystemEntryInfo> EnumerateFileSystemEntryInfos(string path)
      {
         return EnumerateFileSystemEntryInfoInternal<FileSystemEntryInfo>(null, path, Path.WildcardStarMatchAll, SearchOption.TopDirectoryOnly, null, null, false, false, false);
      }

      /// <summary>[AlphaFS] Returns an enumerable collection of file instances and directory instances  that match a <paramref name="searchPattern"/> in a specified path.</summary>
      /// <param name="path">The directory to search.</param>
      /// <param name="searchPattern">The search string to match against the names of directories in <paramref name="path"/>. This parameter can contain a combination of valid literal path and wildcard (<see cref="T:Path.WildcardStarMatchAll"/> and <see cref="T:Path.WildcardQuestion"/>) characters, but doesn't support regular expressions.</param>
      /// <returns>An enumerable collection of file-system entries in the directory specified by <paramref name="path"/> and that match the specified <paramref name="searchPattern"/>.</returns>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
      [SecurityCritical]
      public static IEnumerable<FileSystemEntryInfo> EnumerateFileSystemEntryInfos(string path, string searchPattern)
      {
         return EnumerateFileSystemEntryInfoInternal<FileSystemEntryInfo>(null, path, searchPattern, SearchOption.TopDirectoryOnly, null, null, false, false, false);
      }

      /// <summary>[AlphaFS] Returns an enumerable collection of file instances and directory instances  that match a <paramref name="searchPattern"/> in a specified path, and optionally searches subdirectories.</summary>
      /// <param name="path">The directory to search.</param>
      /// <param name="searchPattern">The search string to match against the names of directories in <paramref name="path"/>. This parameter can contain a combination of valid literal path and wildcard (<see cref="T:Path.WildcardStarMatchAll"/> and <see cref="T:Path.WildcardQuestion"/>) characters, but doesn't support regular expressions.</param>
      /// <param name="searchOption">One of the <see cref="T:SearchOption"/> enumeration values that specifies whether the <paramref name="searchOption"/> should include only the current directory or should include all subdirectories.</param>
      /// <returns>An enumerable collection of file-system entries in the directory specified by <paramref name="path"/> and that match the specified <paramref name="searchPattern"/> and <paramref name="searchOption"/>.</returns>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
      [SecurityCritical]
      public static IEnumerable<FileSystemEntryInfo> EnumerateFileSystemEntryInfos(string path, string searchPattern, SearchOption searchOption)
      {
         return EnumerateFileSystemEntryInfoInternal<FileSystemEntryInfo>(null, path, searchPattern, searchOption, null, null, false, false, false);
      }

      /// <summary>[AlphaFS] Returns an enumerable collection of file instances and directory instances  that match a <paramref name="searchPattern"/> in a specified path.</summary>
      /// <param name="path">The directory to search.</param>
      /// <param name="searchPattern">The search string to match against the names of directories in <paramref name="path"/>. This parameter can contain a combination of valid literal path and wildcard (<see cref="T:Path.WildcardStarMatchAll"/> and <see cref="T:Path.WildcardQuestion"/>) characters, but doesn't support regular expressions.</param>
      /// <param name="continueOnException"><c>true</c> suppress any Exception that might be thrown a result from a failure, such as ACLs protected directories or non-accessible reparse points.</param>
      /// <returns>An enumerable collection of file-system entries in the directory specified by <paramref name="path"/> and that match the specified <paramref name="searchPattern"/>.</returns>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
      [SecurityCritical]
      public static IEnumerable<FileSystemEntryInfo> EnumerateFileSystemEntryInfos(string path, string searchPattern, bool continueOnException)
      {
         return EnumerateFileSystemEntryInfoInternal<FileSystemEntryInfo>(null, path, searchPattern, SearchOption.TopDirectoryOnly, null, null, false, continueOnException, false);
      }

      /// <summary>[AlphaFS] Returns an enumerable collection of file instances and directory instances  that match a <paramref name="searchPattern"/> in a specified path, and optionally searches subdirectories.</summary>
      /// <param name="path">The directory to search.</param>
      /// <param name="searchPattern">The search string to match against the names of directories in <paramref name="path"/>. This parameter can contain a combination of valid literal path and wildcard (<see cref="T:Path.WildcardStarMatchAll"/> and <see cref="T:Path.WildcardQuestion"/>) characters, but doesn't support regular expressions.</param>
      /// <param name="searchOption">One of the <see cref="T:SearchOption"/> enumeration values that specifies whether the <paramref name="searchOption"/> should include only the current directory or should include all subdirectories.</param>
      /// <param name="continueOnException"><c>true</c> suppress any Exception that might be thrown a result from a failure, such as ACLs protected directories or non-accessible reparse points.</param>
      /// <returns>An enumerable collection of <see cref="T:FileSystemEntryInfo"/> file-system entries in the directory specified by <paramref name="path"/> and that match the specified <paramref name="searchPattern"/> and <paramref name="searchOption"/>.</returns>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
      [SecurityCritical]
      public static IEnumerable<FileSystemEntryInfo> EnumerateFileSystemEntryInfos(string path, string searchPattern, SearchOption searchOption, bool continueOnException)
      {
         return EnumerateFileSystemEntryInfoInternal<FileSystemEntryInfo>(null, path, searchPattern, searchOption, null, null, false, continueOnException, false);
      }

      #region Transacted

      #region IsFullPath

      /// <summary>[AlphaFS] Returns an enumerable collection of file instances and directory instances in a specified path.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory to search.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>An enumerable collection of file-system entries in the directory specified by <paramref name="path"/>.</returns>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
      [SecurityCritical]
      public static IEnumerable<FileSystemEntryInfo> EnumerateFileSystemEntryInfos(KernelTransaction transaction, string path, bool? isFullPath)
      {
         return EnumerateFileSystemEntryInfoInternal<FileSystemEntryInfo>(transaction, path, Path.WildcardStarMatchAll, SearchOption.TopDirectoryOnly, null, null, false, false, isFullPath);
      }

      /// <summary>[AlphaFS] Returns an enumerable collection of file instances and directory instances in a specified path.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory to search.</param>
      /// <param name="continueOnException"><c>true</c> suppress any Exception that might be thrown a result from a failure, such as ACLs protected directories or non-accessible reparse points.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>An enumerable collection of file-system entries in the directory specified by <paramref name="path"/>.</returns>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
      [SecurityCritical]
      public static IEnumerable<FileSystemEntryInfo> EnumerateFileSystemEntryInfos(KernelTransaction transaction, string path, bool continueOnException, bool? isFullPath)
      {
         return EnumerateFileSystemEntryInfoInternal<FileSystemEntryInfo>(transaction, path, Path.WildcardStarMatchAll, SearchOption.TopDirectoryOnly, null, null, false, continueOnException, isFullPath);
      }

      /// <summary>[AlphaFS] Returns an enumerable collection of file instances and directory instances  that match a <paramref name="searchPattern"/> in a specified path.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory to search.</param>
      /// <param name="searchPattern">The search string to match against the names of directories in <paramref name="path"/>. This parameter can contain a combination of valid literal path and wildcard (<see cref="T:Path.WildcardStarMatchAll"/> and <see cref="T:Path.WildcardQuestion"/>) characters, but doesn't support regular expressions.</param>
      /// <param name="continueOnException"><c>true</c> suppress any Exception that might be thrown a result from a failure, such as ACLs protected directories or non-accessible reparse points.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>An enumerable collection of file-system entries in the directory specified by <paramref name="path"/> and that match the specified <paramref name="searchPattern"/>.</returns>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
      [SecurityCritical]
      public static IEnumerable<FileSystemEntryInfo> EnumerateFileSystemEntryInfos(KernelTransaction transaction, string path, string searchPattern, bool continueOnException, bool? isFullPath)
      {
         return EnumerateFileSystemEntryInfoInternal<FileSystemEntryInfo>(transaction, path, searchPattern, SearchOption.TopDirectoryOnly, null, null, false, continueOnException, isFullPath);
      }

      /// <summary>[AlphaFS] Returns an enumerable collection of file instances and directory instances  that match a <paramref name="searchPattern"/> in a specified path, and optionally searches subdirectories.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory to search.</param>
      /// <param name="searchPattern">The search string to match against the names of directories in <paramref name="path"/>. This parameter can contain a combination of valid literal path and wildcard (<see cref="T:Path.WildcardStarMatchAll"/> and <see cref="T:Path.WildcardQuestion"/>) characters, but doesn't support regular expressions.</param>
      /// <param name="searchOption">One of the <see cref="T:SearchOption"/> enumeration values that specifies whether the <paramref name="searchOption"/> should include only the current directory or should include all subdirectories.</param>
      /// <param name="continueOnException"><c>true</c> suppress any Exception that might be thrown a result from a failure, such as ACLs protected directories or non-accessible reparse points.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>An enumerable collection of file-system entries in the directory specified by <paramref name="path"/> and that match the specified <paramref name="searchPattern"/> and <paramref name="searchOption"/>.</returns>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
      [SecurityCritical]
      public static IEnumerable<FileSystemEntryInfo> EnumerateFileSystemEntryInfos(KernelTransaction transaction, string path, string searchPattern, SearchOption searchOption, bool continueOnException, bool? isFullPath)
      {
         return EnumerateFileSystemEntryInfoInternal<FileSystemEntryInfo>(transaction, path, searchPattern, searchOption, null, null, false, continueOnException, isFullPath);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Returns an enumerable collection of file instances and directory instances in a specified path.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory to search.</param>
      /// <returns>An enumerable collection of file-system entries in the directory specified by <paramref name="path"/>.</returns>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
      [SecurityCritical]
      public static IEnumerable<FileSystemEntryInfo> EnumerateFileSystemEntryInfos(KernelTransaction transaction, string path)
      {
         return EnumerateFileSystemEntryInfoInternal<FileSystemEntryInfo>(transaction, path, Path.WildcardStarMatchAll, SearchOption.TopDirectoryOnly, null, null, false, false, false);
      }

      /// <summary>[AlphaFS] Returns an enumerable collection of file instances and directory instances  that match a <paramref name="searchPattern"/> in a specified path.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory to search.</param>
      /// <param name="searchPattern">The search string to match against the names of directories in <paramref name="path"/>. This parameter can contain a combination of valid literal path and wildcard (<see cref="T:Path.WildcardStarMatchAll"/> and <see cref="T:Path.WildcardQuestion"/>) characters, but doesn't support regular expressions.</param>
      /// <returns>An enumerable collection of file-system entries in the directory specified by <paramref name="path"/> and that match the specified <paramref name="searchPattern"/>.</returns>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
      [SecurityCritical]
      public static IEnumerable<FileSystemEntryInfo> EnumerateFileSystemEntryInfos(KernelTransaction transaction, string path, string searchPattern)
      {
         return EnumerateFileSystemEntryInfoInternal<FileSystemEntryInfo>(transaction, path, searchPattern, SearchOption.TopDirectoryOnly, null, null, false, false, false);
      }

      /// <summary>[AlphaFS] Returns an enumerable collection of file instances and directory instances  that match a <paramref name="searchPattern"/> in a specified path, and optionally searches subdirectories.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory to search.</param>
      /// <param name="searchPattern">The search string to match against the names of directories in <paramref name="path"/>. This parameter can contain a combination of valid literal path and wildcard (<see cref="T:Path.WildcardStarMatchAll"/> and <see cref="T:Path.WildcardQuestion"/>) characters, but doesn't support regular expressions.</param>
      /// <param name="searchOption">One of the <see cref="T:SearchOption"/> enumeration values that specifies whether the <paramref name="searchOption"/> should include only the current directory or should include all subdirectories.</param>
      /// <returns>An enumerable collection of file-system entries in the directory specified by <paramref name="path"/> and that match the specified <paramref name="searchPattern"/> and <paramref name="searchOption"/>.</returns>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
      [SecurityCritical]
      public static IEnumerable<FileSystemEntryInfo> EnumerateFileSystemEntryInfos(KernelTransaction transaction, string path, string searchPattern, SearchOption searchOption)
      {
         return EnumerateFileSystemEntryInfoInternal<FileSystemEntryInfo>(transaction, path, searchPattern, searchOption, null, null, false, false, false);
      }

      /// <summary>[AlphaFS] Returns an enumerable collection of file instances and directory instances  that match a <paramref name="searchPattern"/> in a specified path.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory to search.</param>
      /// <param name="searchPattern">The search string to match against the names of directories in <paramref name="path"/>. This parameter can contain a combination of valid literal path and wildcard (<see cref="T:Path.WildcardStarMatchAll"/> and <see cref="T:Path.WildcardQuestion"/>) characters, but doesn't support regular expressions.</param>
      /// <param name="continueOnException"><c>true</c> suppress any Exception that might be thrown a result from a failure, such as ACLs protected directories or non-accessible reparse points.</param>
      /// <returns>An enumerable collection of <see cref="T:FileSystemEntryInfo"/> file-system entries in the directory specified by <paramref name="path"/> and that match the specified <paramref name="searchPattern"/>.</returns>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
      [SecurityCritical]
      public static IEnumerable<FileSystemEntryInfo> EnumerateFileSystemEntryInfos(KernelTransaction transaction, string path, string searchPattern, bool continueOnException)
      {
         return EnumerateFileSystemEntryInfoInternal<FileSystemEntryInfo>(transaction, path, searchPattern, SearchOption.TopDirectoryOnly, null, null, false, continueOnException, false);
      }

      /// <summary>[AlphaFS] Returns an enumerable collection of file instances and directory instances  that match a <paramref name="searchPattern"/> in a specified path, and optionally searches subdirectories.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory to search.</param>
      /// <param name="searchPattern">The search string to match against the names of directories in <paramref name="path"/>. This parameter can contain a combination of valid literal path and wildcard (<see cref="T:Path.WildcardStarMatchAll"/> and <see cref="T:Path.WildcardQuestion"/>) characters, but doesn't support regular expressions.</param>
      /// <param name="searchOption">One of the <see cref="T:SearchOption"/> enumeration values that specifies whether the <paramref name="searchOption"/> should include only the current directory or should include all subdirectories.</param>
      /// <param name="continueOnException"><c>true</c> suppress any Exception that might be thrown a result from a failure, such as ACLs protected directories or non-accessible reparse points.</param>
      /// <returns>An enumerable collection of file-system entries in the directory specified by <paramref name="path"/> and that match the specified <paramref name="searchPattern"/> and <paramref name="searchOption"/>.</returns>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
      [SecurityCritical]
      public static IEnumerable<FileSystemEntryInfo> EnumerateFileSystemEntryInfos(KernelTransaction transaction, string path, string searchPattern, SearchOption searchOption, bool continueOnException)
      {
         return EnumerateFileSystemEntryInfoInternal<FileSystemEntryInfo>(transaction, path, searchPattern, searchOption, null, null, false, continueOnException, false);
      }

      #endregion // Transacted

      #endregion // EnumerateFileSystemEntryInfos

      #region EnumerateHardlinks

      #region IsFullPath

      /// <summary>[AlphaFS] Creates an enumeration of all the hard links to the specified <paramref name="path"/>.</summary>
      /// <param name="path">The name of the file.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>An enumerable collection of <see cref="T:String"/> of all the hard links to the specified <paramref name="path"/></returns>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Hardlinks")]
      [SecurityCritical]
      public static IEnumerable<string> EnumerateHardlinks(string path, bool? isFullPath)
      {
         return EnumerateHardlinksInternal(null, path, isFullPath);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Creates an enumeration of all the hard links to the specified <paramref name="path"/>.</summary>
      /// <param name="path">The name of the file.</param>
      /// <returns>An enumerable collection of <see cref="T:String"/> of all the hard links to the specified <paramref name="path"/></returns>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Hardlinks")]
      [SecurityCritical]
      public static IEnumerable<string> EnumerateHardlinks(string path)
      {
         return EnumerateHardlinksInternal(null, path, false);
      }

      #region Transacted

      #region IsFullPath

      /// <summary>[AlphaFS] Creates an enumeration of all the hard links to the specified <paramref name="path"/>.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The name of the file.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>An enumerable collection of <see cref="T:String"/> of all the hard links to the specified <paramref name="path"/></returns>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Hardlinks")]
      [SecurityCritical]
      public static IEnumerable<string> EnumerateHardlinks(KernelTransaction transaction, string path, bool? isFullPath)
      {
         return EnumerateHardlinksInternal(transaction, path, isFullPath);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Creates an enumeration of all the hard links to the specified <paramref name="path"/>.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The name of the file.</param>
      /// <returns>An enumerable collection of <see cref="T:String"/> of all the hard links to the specified <paramref name="path"/></returns>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Hardlinks")]
      [SecurityCritical]
      public static IEnumerable<string> EnumerateHardlinks(KernelTransaction transaction, string path)
      {
         return EnumerateHardlinksInternal(transaction, path, false);
      }

      #endregion // Transacted

      #endregion // EnumerateHardlinks

      #region EnumerateStreams

      #region IsFullPath

      /// <summary>[AlphaFS] Returns an enumerable collection of <see cref="T:AlternateDataStreamInfo"/> instances for the file specified by <paramref name="path"/>.</summary>
      /// <param name="path">The file to search.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>An enumerable collection of <see cref="T:AlternateDataStreamInfo"/> instances for the file specified by <paramref name="path"/>.</returns>
      [SecurityCritical]
      public static IEnumerable<AlternateDataStreamInfo> EnumerateStreams(string path, bool isFullPath)
      {
         return AlternateDataStreamInfo.EnumerateStreamsInternal(null, null, null, path, null, null, isFullPath);
      }

      /// <summary>[AlphaFS] Returns an enumerable collection of <see cref="T:AlternateDataStreamInfo"/> of type <see cref="T:StreamType"/> instances for the file specified by <paramref name="path"/>.</summary>
      /// <param name="path">The file to search.</param>
      /// <param name="streamType">The type of stream to retrieve.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>An enumerable collection of <see cref="T:AlternateDataStreamInfo"/> of type <see cref="T:StreamType"/> instances for the file specified by <paramref name="path"/>.</returns>
      [SecurityCritical]
      public static IEnumerable<AlternateDataStreamInfo> EnumerateStreams(string path, StreamType streamType, bool isFullPath)
      {
         return AlternateDataStreamInfo.EnumerateStreamsInternal(null, null, null, path, null, streamType, isFullPath);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Returns an enumerable collection of <see cref="T:AlternateDataStreamInfo"/> instances for the file specified by <paramref name="path"/>.</summary>
      /// <param name="path">The file to search.</param>
      /// <returns>An enumerable collection of <see cref="T:AlternateDataStreamInfo"/> instances for the file specified by <paramref name="path"/>.</returns>
      [SecurityCritical]
      public static IEnumerable<AlternateDataStreamInfo> EnumerateStreams(string path)
      {
         return AlternateDataStreamInfo.EnumerateStreamsInternal(null, null, null, path, null, null, false);
      }

      /// <summary>[AlphaFS] Returns an enumerable collection of <see cref="T:AlternateDataStreamInfo"/> of type <see cref="T:StreamType"/> instances for the file specified by <paramref name="path"/>.</summary>
      /// <param name="path">The file to search.</param>
      /// <param name="streamType">The type of stream to retrieve.</param>
      /// <returns>An enumerable collection of <see cref="T:AlternateDataStreamInfo"/> of type <see cref="T:StreamType"/> instances for the file specified by <paramref name="path"/>.</returns>
      [SecurityCritical]
      public static IEnumerable<AlternateDataStreamInfo> EnumerateStreams(string path, StreamType streamType)
      {
         return AlternateDataStreamInfo.EnumerateStreamsInternal(null, null, null, path, null, streamType, false);
      }

      /// <summary>[AlphaFS] Returns an enumerable collection of <see cref="T:AlternateDataStreamInfo"/> instances for the handle specified by <paramref name="handle"/>.</summary>
      /// <param name="handle">A <see cref="T:SafeFileHandle"/> connected to the file from which to retrieve the information.</param>
      /// <returns>An enumerable collection of <see cref="T:AlternateDataStreamInfo"/> instances for the handle specified by <paramref name="handle"/>.</returns>
      [SecurityCritical]
      public static IEnumerable<AlternateDataStreamInfo> EnumerateStreams(SafeFileHandle handle)
      {
         return AlternateDataStreamInfo.EnumerateStreamsInternal(null, null, handle, null, null, null, null);
      }

      #region Transacted

      #region IsFullPath

      /// <summary>[AlphaFS] Returns an enumerable collection of <see cref="T:AlternateDataStreamInfo"/> instances for the file specified by <paramref name="path"/>.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to search.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>An enumerable collection of <see cref="T:AlternateDataStreamInfo"/> instances for the file specified by <paramref name="path"/>.</returns>
      [SecurityCritical]
      public static IEnumerable<AlternateDataStreamInfo> EnumerateStreams(KernelTransaction transaction, string path, bool isFullPath)
      {
         return AlternateDataStreamInfo.EnumerateStreamsInternal(null, transaction, null, path, null, null, isFullPath);
      }

      /// <summary>[AlphaFS] Returns an enumerable collection of <see cref="T:AlternateDataStreamInfo"/> of type <see cref="T:StreamType"/> instances for the file specified by <paramref name="path"/>.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to search.</param>
      /// <param name="streamType">The type of stream to retrieve.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>An enumerable collection of <see cref="T:AlternateDataStreamInfo"/> of type <see cref="T:StreamType"/> instances for the file specified by <paramref name="path"/>.</returns>
      [SecurityCritical]
      public static IEnumerable<AlternateDataStreamInfo> EnumerateStreams(KernelTransaction transaction, string path, StreamType streamType, bool isFullPath)
      {
         return AlternateDataStreamInfo.EnumerateStreamsInternal(null, transaction, null, path, null, streamType, isFullPath);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Returns an enumerable collection of <see cref="T:AlternateDataStreamInfo"/> instances for the file specified by <paramref name="path"/>.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to search.</param>
      /// <returns>An enumerable collection of <see cref="T:AlternateDataStreamInfo"/> instances for the file specified by <paramref name="path"/>.</returns>
      [SecurityCritical]
      public static IEnumerable<AlternateDataStreamInfo> EnumerateStreams(KernelTransaction transaction, string path)
      {
         return AlternateDataStreamInfo.EnumerateStreamsInternal(null, transaction, null, path, null, null, false);
      }

      /// <summary>[AlphaFS] Returns an enumerable collection of <see cref="T:AlternateDataStreamInfo"/> of type <see cref="T:StreamType"/> instances for the file specified by <paramref name="path"/>.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to search.</param>
      /// <param name="streamType">The type of stream to retrieve.</param>
      /// <returns>An enumerable collection of <see cref="T:AlternateDataStreamInfo"/> of type <see cref="T:StreamType"/> instances for the file specified by <paramref name="path"/>.</returns>
      [SecurityCritical]
      public static IEnumerable<AlternateDataStreamInfo> EnumerateStreams(KernelTransaction transaction, string path, StreamType streamType)
      {
         return AlternateDataStreamInfo.EnumerateStreamsInternal(null, transaction, null, path, null, streamType, false);
      }

      #endregion // Transacted

      #endregion // EnumerateStreams

      #region GetCompressedSize

      #region IsFullPath

      /// <summary>[AlphaFS] Retrieves the actual number of bytes of disk storage used to store a specified file.</summary>
      /// <remarks>
      /// If the file is located on a volume that
      /// supports compression and the file is compressed, the value obtained is the compressed size of the specified file.
      /// If the file is located on a volume that supports sparse files and the file is a sparse file, the value obtained is the sparse
      /// size of the specified file.
      /// </remarks>
      /// <param name="path"><para>The name of the file.</para></param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>The actual number of bytes of disk storage used to store the specified file.</returns>
      [SecurityCritical]
      public static long GetCompressedSize(string path, bool? isFullPath)
      {
         return GetCompressedSizeInternal(null, path, isFullPath);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Retrieves the actual number of bytes of disk storage used to store a specified file.</summary>
      /// <remarks>
      /// If the file is located on a volume that
      /// supports compression and the file is compressed, the value obtained is the compressed size of the specified file.
      /// If the file is located on a volume that supports sparse files and the file is a sparse file, the value obtained is the sparse
      /// size of the specified file.
      /// </remarks>
      /// <param name="path"><para>The name of the file.</para></param>
      /// <returns>The actual number of bytes of disk storage used to store the specified file.</returns>
      [SecurityCritical]
      public static long GetCompressedSize(string path)
      {
         return GetCompressedSizeInternal(null, path, false);
      }

      #region Transacted

      #region IsFullPath

      /// <summary>[AlphaFS] Retrieves the actual number of bytes of disk storage used to store a specified file as part of a transaction.
      /// If the file is located on a volume that supports compression and the file is compressed, the value obtained is the compressed size of the specified file.
      /// If the file is located on a volume that supports sparse files and the file is a sparse file, the value obtained is the sparse
      /// size of the specified file.
      /// </summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path"><para>The name of the file.</para></param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>The actual number of bytes of disk storage used to store the specified file.</returns>
      [SecurityCritical]
      public static long GetCompressedSize(KernelTransaction transaction, string path, bool? isFullPath)
      {
         return GetCompressedSizeInternal(transaction, path, isFullPath);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Retrieves the actual number of bytes of disk storage used to store a specified file as part of a transaction.
      /// If the file is located on a volume that supports compression and the file is compressed, the value obtained is the compressed size of the specified file.
      /// If the file is located on a volume that supports sparse files and the file is a sparse file, the value obtained is the sparse
      /// size of the specified file.
      /// </summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path"><para>The name of the file.</para></param>
      /// <returns>The actual number of bytes of disk storage used to store the specified file.</returns>
      [SecurityCritical]
      public static long GetCompressedSize(KernelTransaction transaction, string path)
      {
         return GetCompressedSizeInternal(transaction, path, false);
      }

      #endregion // Transacted

      #endregion // GetCompressedSize

      #region GetEncryptionStatus

      #region IsFullPath

      /// <summary>[AlphaFS] Retrieves the encryption status of the specified file.</summary>
      /// <param name="path">The name of the file.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>The <see cref="T:FileEncryptionStatus"/> of the specified <paramref name="path"/>.</returns>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static FileEncryptionStatus GetEncryptionStatus(string path, bool? isFullPath)
      {
         return GetEncryptionStatusInternal(path, isFullPath);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Retrieves the encryption status of the specified file.</summary>
      /// <param name="path">The name of the file.</param>
      /// <returns>The <see cref="T:FileEncryptionStatus"/> of the specified <paramref name="path"/>.</returns>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static FileEncryptionStatus GetEncryptionStatus(string path)
      {
         return GetEncryptionStatusInternal(path, false);
      }

      #endregion // GetEncryptionStatus

      #region GetFileInformationByHandle

      /// <summary>[AlphaFS] Retrieves file information for the specified <see cref="T:SafeFileHandle"/>.</summary>
      /// <param name="handle">A <see cref="T:SafeFileHandle"/> connected to the open file from which to retrieve the information.</param>
      /// <returns>A <see cref="T:ByHandleFileInformation"/> object containing the requested information.</returns>
      /// <exception cref="NativeError.ThrowException()"></exception>
      [SecurityCritical]
      public static ByHandleFileInformation GetFileInformationByHandle(SafeFileHandle handle)
      {
         NativeMethods.IsValidHandle(handle);

         NativeMethods.ByHandleFileInformation info;

         if (!NativeMethods.GetFileInformationByHandle(handle, out info))
            NativeError.ThrowException(Marshal.GetLastWin32Error());

         return new ByHandleFileInformation(info);
      }

      #endregion // GetFileInformationByHandle

      #region GetFileSystemEntryInfo

      #region IsFullPath

      /// <summary>[AlphaFS] Gets the <see cref="T:FileSystemEntryInfo"/> of the file or directory on the path.</summary>
      /// <param name="path">The path to the file or directory.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>The <see cref="T:FileSystemEntryInfo"/> instance of the file or directory on the path.</returns>
      /// <exception cref="NativeError.ThrowException()"></exception>
      [SecurityCritical]
      public static FileSystemEntryInfo GetFileSystemEntryInfo(string path, bool? isFullPath)
      {
         return GetFileSystemEntryInfoInternal(false, null, path, true, false, isFullPath);
      }

      /// <summary>[AlphaFS] Gets the <see cref="T:FileSystemEntryInfo"/> of the file or directory on the path.</summary>
      /// /// <param name="isFolder">The main reason for this parameter is to throw a more appropriate error: DirectoryNotFound vs FileNotFound. <c>true</c> indicates a directory object, DirectoryNotFound will be thrown. <c>false</c> indicates a file object.</param>
      /// <param name="path">The path to the file or directory.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>The <see cref="T:FileSystemEntryInfo"/> instance of the file or directory on the path.</returns>
      /// <exception cref="NativeError.ThrowException()"></exception>
      [SecurityCritical]
      public static FileSystemEntryInfo GetFileSystemEntryInfo(bool isFolder, string path, bool? isFullPath)
      {
         return GetFileSystemEntryInfoInternal(isFolder, null, path, true, false, isFullPath);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Gets the <see cref="T:FileSystemEntryInfo"/> of the file or directory on the path.</summary>
      /// <param name="path">The path to the file or directory.</param>
      /// <returns>The <see cref="T:FileSystemEntryInfo"/> instance of the file or directory on the path.</returns>
      /// <exception cref="NativeError.ThrowException()"></exception>
      [SecurityCritical]
      public static FileSystemEntryInfo GetFileSystemEntryInfo(string path)
      {
         return GetFileSystemEntryInfoInternal(false, null, path, true, false, false);
      }

      /// <summary>[AlphaFS] Gets the <see cref="T:FileSystemEntryInfo"/> of the file or directory on the path.</summary>
      /// /// <param name="isFolder">The main reason for this parameter is to throw a more appropriate error: DirectoryNotFound vs FileNotFound. <c>true</c> indicates a directory object, DirectoryNotFound will be thrown. <c>false</c> indicates a file object.</param>
      /// <param name="path">The path to the file or directory.</param>
      /// <returns>The <see cref="T:FileSystemEntryInfo"/> instance of the file or directory on the path.</returns>
      /// <exception cref="NativeError.ThrowException()"></exception>
      [SecurityCritical]
      public static FileSystemEntryInfo GetFileSystemEntryInfo(bool isFolder, string path)
      {
         return GetFileSystemEntryInfoInternal(isFolder, null, path, true, false, false);
      }

      #region Transacted

      #region IsFullPath

      /// <summary>[AlphaFS] Gets the <see cref="T:FileSystemEntryInfo"/> of the file or directory on the path.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to the file or directory.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>The <see cref="T:FileSystemEntryInfo"/> instance of the file or directory on the path.</returns>
      /// <exception cref="NativeError.ThrowException()"></exception>
      [SecurityCritical]
      public static FileSystemEntryInfo GetFileSystemEntryInfo(KernelTransaction transaction, string path, bool? isFullPath)
      {
         return GetFileSystemEntryInfoInternal(false, transaction, path, true, false, isFullPath);
      }

      /// <summary>[AlphaFS] Gets the <see cref="T:FileSystemEntryInfo"/> of the file or directory on the path.</summary>
      /// /// <param name="isFolder">The main reason for this parameter is to throw a more appropriate error: DirectoryNotFound vs FileNotFound. <c>true</c> indicates a directory object, DirectoryNotFound will be thrown. <c>false</c> indicates a file object.</param>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to the file or directory.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>The <see cref="T:FileSystemEntryInfo"/> instance of the file or directory on the path.</returns>
      /// <exception cref="NativeError.ThrowException()"></exception>
      [SecurityCritical]
      public static FileSystemEntryInfo GetFileSystemEntryInfo(bool isFolder, KernelTransaction transaction, string path, bool? isFullPath)
      {
         return GetFileSystemEntryInfoInternal(isFolder, transaction, path, true, false, isFullPath);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Gets the <see cref="T:FileSystemEntryInfo"/> of the file or directory on the path.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to the file or directory.</param>
      /// <returns>The <see cref="T:FileSystemEntryInfo"/> instance of the file or directory on the path.</returns>
      /// <exception cref="NativeError.ThrowException()"></exception>
      [SecurityCritical]
      public static FileSystemEntryInfo GetFileSystemEntryInfo(KernelTransaction transaction, string path)
      {
         return GetFileSystemEntryInfoInternal(false, transaction, path, true, false, false);
      }

      /// <summary>[AlphaFS] Gets the <see cref="T:FileSystemEntryInfo"/> of the file or directory on the path.</summary>
      /// /// <param name="isFolder">The main reason for this parameter is to throw a more appropriate error: DirectoryNotFound vs FileNotFound. <c>true</c> indicates a directory object, DirectoryNotFound will be thrown. <c>false</c> indicates a file object.</param>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to the file or directory.</param>
      /// <returns>The <see cref="T:FileSystemEntryInfo"/> instance of the file or directory on the path.</returns>
      /// <exception cref="NativeError.ThrowException()"></exception>
      [SecurityCritical]
      public static FileSystemEntryInfo GetFileSystemEntryInfo(bool isFolder, KernelTransaction transaction, string path)
      {
         return GetFileSystemEntryInfoInternal(isFolder, transaction, path, true, false, false);
      }

      #endregion // Transacted

      #endregion // GetFileSystemEntryInfo

      #region GetLinkTargetInfo

      #region IsFullPath

      /// <summary>[AlphaFS] Gets information about the target of a mount point or symbolic link on an NTFS file system.</summary>
      /// <param name="path">The path to the reparse point.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>An instance of <see cref="T:LinkTargetInfo"/> or <see cref="T:SymbolicLinkTargetInfo"/> containing
      /// information about the symbolic link or mount point pointed to by <paramref name="path"/>.</returns>
      [SecurityCritical]
      public static LinkTargetInfo GetLinkTargetInfo(string path, bool? isFullPath)
      {
         return GetLinkTargetInfoInternal(null, path, isFullPath);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Gets information about the target of a mount point or symbolic link on an NTFS file system.</summary>
      /// <param name="path">The path to the reparse point.</param>
      /// <returns>An instance of <see cref="T:LinkTargetInfo"/> or <see cref="T:SymbolicLinkTargetInfo"/> containing
      /// information about the symbolic link or mount point pointed to by <paramref name="path"/>.</returns>
      [SecurityCritical]
      public static LinkTargetInfo GetLinkTargetInfo(string path)
      {
         return GetLinkTargetInfoInternal(null, path, false);
      }
      
      #region Transacted

      #region IsFullPath

      /// <summary>[AlphaFS] Gets information about the target of a mount point or symbolic link on an NTFS file system.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to the reparse point.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>An instance of <see cref="T:LinkTargetInfo"/> or <see cref="T:SymbolicLinkTargetInfo"/> containing
      /// information about the symbolic link or mount point pointed to by <paramref name="path"/>.
      /// </returns>
      [SecurityCritical]
      public static LinkTargetInfo GetLinkTargetInfo(KernelTransaction transaction, string path, bool? isFullPath)
      {
         return GetLinkTargetInfoInternal(transaction, path, isFullPath);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Gets information about the target of a mount point or symbolic link on an NTFS file system.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to the reparse point.</param>
      /// <returns>An instance of <see cref="T:LinkTargetInfo"/> or <see cref="T:SymbolicLinkTargetInfo"/> containing
      /// information about the symbolic link or mount point pointed to by <paramref name="path"/>.
      /// </returns>
      [SecurityCritical]
      public static LinkTargetInfo GetLinkTargetInfo(KernelTransaction transaction, string path)
      {
         return GetLinkTargetInfoInternal(transaction, path, false);
      }

      #endregion // Transacted

      #endregion // GetLinkTargetInfo

      #region GetSize

      #region IsFullPath

      /// <summary>[AlphaFS] Retrieves the file size, in bytes to store a specified file.</summary>
      /// <param name="path">The path to the file.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>The file size, in bytes.</returns>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static long GetSize(string path, bool? isFullPath)
      {
         return GetSizeInternal(null, null, path, isFullPath);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Retrieves the file size, in bytes to store a specified file.</summary>
      /// <param name="path">The path to the file.</param>
      /// <returns>The file size, in bytes.</returns>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static long GetSize(string path)
      {
         return GetSizeInternal(null, null, path, false);
      }

      /// <summary>[AlphaFS] Retrieves the file size, in bytes to store a specified file.</summary>
      /// <param name="handle">The <see cref="T:SafeFileHandle"/> to the file.</param>
      /// <returns>The file size, in bytes.</returns>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static long GetSize(SafeFileHandle handle)
      {
         return GetSizeInternal(null, handle, null, null);
      }

      #region Transacted

      #region IsFullPath

      /// <summary>[AlphaFS] Retrieves the file size, in bytes to store a specified file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to the file.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>The number of bytes of disk storage used to store the specified file.</returns>
      [SecurityCritical]
      public static long GetSize(KernelTransaction transaction, string path, bool? isFullPath)
      {
         return GetSizeInternal(transaction, null, path, isFullPath);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Retrieves the file size, in bytes to store a specified file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to the file.</param>
      /// <returns>The number of bytes of disk storage used to store the specified file.</returns>
      [SecurityCritical]
      public static long GetSize(KernelTransaction transaction, string path)
      {
         return GetSizeInternal(transaction, null, path, false);
      }

      #endregion // Transacted

      #endregion // GetSize

      #region GetStreamSize

      #region IsFullPath

      /// <summary>[AlphaFS] Retrieves the actual number of bytes of disk storage used by all data streams (NTFS ADS).</summary>
      /// <param name="path">The path to an existing file.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>The number of bytes used by all data streams.</returns>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static long GetStreamSize(string path, bool isFullPath)
      {
         return AlternateDataStreamInfo.GetStreamSizeInternal(false, null, null, path, null, null, isFullPath);
      }

      /// <summary>[AlphaFS] Retrieves the actual number of bytes of disk storage used by a named data streams (NTFS ADS).</summary>
      /// <param name="path">The path to an existing file.</param>
      /// <param name="name">The name of the stream to retrieve.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>The number of bytes used by a named stream.</returns>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static long GetStreamSize(string path, string name, bool isFullPath)
      {
         return AlternateDataStreamInfo.GetStreamSizeInternal(false, null, null, path, name, StreamType.Data, isFullPath);
      }

      /// <summary>[AlphaFS] Retrieves the actual number of bytes of disk storage used by a <see cref="T:StreamType"/> data streams (NTFS ADS).</summary>
      /// <param name="path">The path to an existing file.</param>
      /// <param name="type">The <see cref="T:StreamType"/> of the stream to retrieve.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>The number of bytes used by stream of type <see cref="T:StreamType"/>.</returns>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static long GetStreamSize(string path, StreamType type, bool isFullPath)
      {
         return AlternateDataStreamInfo.GetStreamSizeInternal(false, null, null, path, null, type, isFullPath);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Retrieves the actual number of bytes of disk storage used by all data streams (NTFS ADS).</summary>
      /// <param name="path">The path to an existing file.</param>
      /// <returns>The number of bytes used by all data streams.</returns>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static long GetStreamSize(string path)
      {
         return AlternateDataStreamInfo.GetStreamSizeInternal(false, null, null, path, null, null, false);
      }

      /// <summary>[AlphaFS] Retrieves the actual number of bytes of disk storage used by a named data streams (NTFS ADS).</summary>
      /// <param name="path">The path to an existing file.</param>
      /// <param name="name">The name of the stream to retrieve.</param>
      /// <returns>The number of bytes used by a named stream.</returns>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static long GetStreamSize(string path, string name)
      {
         return AlternateDataStreamInfo.GetStreamSizeInternal(false, null, null, path, name, StreamType.Data, false);
      }

      /// <summary>[AlphaFS] Retrieves the actual number of bytes of disk storage used by a <see cref="T:StreamType"/> data streams (NTFS ADS).</summary>
      /// <param name="path">The path to an existing file.</param>
      /// <param name="type">The <see cref="T:StreamType"/> of the stream to retrieve.</param>
      /// <returns>The number of bytes used by stream of type <see cref="T:StreamType"/>.</returns>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static long GetStreamSize(string path, StreamType type)
      {
         return AlternateDataStreamInfo.GetStreamSizeInternal(false, null, null, path, null, type, false);
      }

      /// <summary>[AlphaFS] Retrieves the actual number of bytes of disk storage used by all data streams (NTFS ADS).</summary>
      /// <param name="handle">The <see cref="T:SafeFileHandle"/> to the file.</param>
      /// <param name="name">The name of the stream to retrieve.</param>
      /// <returns>The number of bytes used by a named stream.</returns>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static long GetStreamSize(SafeFileHandle handle, string name)
      {
         return AlternateDataStreamInfo.GetStreamSizeInternal(false, null, handle, null, name, StreamType.Data, null);
      }

      /// <summary>[AlphaFS] Retrieves the actual number of bytes of disk storage used by a <see cref="T:StreamType"/> data streams (NTFS ADS).</summary>
      /// <param name="handle">The <see cref="T:SafeFileHandle"/> to the file.</param>
      /// <param name="type">The <see cref="T:StreamType"/> of the stream to retrieve.</param>
      /// <returns>The number of bytes used by stream of type <see cref="T:StreamType"/>.</returns>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static long GetStreamSize(SafeFileHandle handle, StreamType type)
      {
         return AlternateDataStreamInfo.GetStreamSizeInternal(false, null, handle, null, null, type, null);
      }

      #region Transacted

      #region IsFullPath

      /// <summary>[AlphaFS] Retrieves the actual number of bytes of disk storage used by all data streams (NTFS ADS).</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to an existing file.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>The number of bytes used by all data streams.</returns>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static long GetStreamSize(KernelTransaction transaction, string path, bool isFullPath)
      {
         return AlternateDataStreamInfo.GetStreamSizeInternal(false, transaction, null, path, null, null, isFullPath);
      }

      /// <summary>[AlphaFS] Retrieves the actual number of bytes of disk storage used by a named data streams (NTFS ADS).</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to an existing file.</param>
      /// <param name="name">The name of the stream to retrieve.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>The number of bytes used by a named stream.</returns>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static long GetStreamSize(KernelTransaction transaction, string path, string name, bool isFullPath)
      {
         return AlternateDataStreamInfo.GetStreamSizeInternal(false, transaction, null, path, name, StreamType.Data, isFullPath);
      }

      /// <summary>[AlphaFS] Retrieves the actual number of bytes of disk storage used by a <see cref="T:StreamType"/> data streams (NTFS ADS).</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to an existing file.</param>
      /// <param name="type">The <see cref="T:StreamType"/> of the stream to retrieve.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>The number of bytes used by stream of type <see cref="T:StreamType"/>.</returns>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static long GetStreamSize(KernelTransaction transaction, string path, StreamType type, bool isFullPath)
      {
         return AlternateDataStreamInfo.GetStreamSizeInternal(false, transaction, null, path, null, type, isFullPath);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Retrieves the actual number of bytes of disk storage used by all data streams (NTFS ADS).</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to an existing file.</param>
      /// <returns>The number of bytes used by all data streams.</returns>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static long GetStreamSize(KernelTransaction transaction, string path)
      {
         return AlternateDataStreamInfo.GetStreamSizeInternal(false, transaction, null, path, null, null, false);
      }

      /// <summary>[AlphaFS] Retrieves the actual number of bytes of disk storage used by a named data streams (NTFS ADS).</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to an existing file.</param>
      /// <param name="name">The name of the stream to retrieve.</param>
      /// <returns>The number of bytes used by a named stream.</returns>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static long GetStreamSize(KernelTransaction transaction, string path, string name)
      {
         return AlternateDataStreamInfo.GetStreamSizeInternal(false, transaction, null, path, name, null, false);
      }

      /// <summary>[AlphaFS] Retrieves the actual number of bytes of disk storage used by all data streams (NTFS ADS).</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to an existing file.</param>
      /// <param name="type">The <see cref="T:StreamType"/> of the stream to retrieve.</param>
      /// <returns>The number of bytes used by stream of type <see cref="T:StreamType"/>.</returns>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static long GetStreamSize(KernelTransaction transaction, string path, StreamType type)
      {
         return AlternateDataStreamInfo.GetStreamSizeInternal(false, transaction, null, path, null, type, false);
      }

      #endregion // Transacted

      #endregion GetStreamSize

      #region RemoveStream

      #region IsFullPath

      /// <summary>[AlphaFS] Removes all alternate data streams (NTFS ADS) from an existing file.</summary>
      /// <param name="path">The path to an existing file.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <remarks>This method only removes streams of type <see cref="T:StreamType.AlternateData"/>.</remarks>
      /// <remarks>No Exception is thrown if the stream does not exist.</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static void RemoveStream(string path, bool? isFullPath)
      {
         AlternateDataStreamInfo.RemoveStreamInternal(false, null, path, null, isFullPath);
      }

      /// <summary>[AlphaFS] Removes an alternate data stream (NTFS ADS) from an existing file.</summary>
      /// <param name="path">The path to an existing file.</param>
      /// <param name="name">The name of the stream to remove.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <remarks>This method only removes streams of type <see cref="T:StreamType.AlternateData"/>.</remarks>
      /// <remarks>No Exception is thrown if the stream does not exist.</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static void RemoveStream(string path, string name, bool? isFullPath)
      {
         AlternateDataStreamInfo.RemoveStreamInternal(false, null, path, name, isFullPath);
      }
      
      #endregion // IsFullPath

      /// <summary>[AlphaFS] Removes all alternate data streams (NTFS ADS) from an existing file.</summary>
      /// <param name="path">The path to an existing file.</param>
      /// <remarks>This method only removes streams of type <see cref="T:StreamType.AlternateData"/>.</remarks>
      /// <remarks>No Exception is thrown if the stream does not exist.</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static void RemoveStream(string path)
      {
         AlternateDataStreamInfo.RemoveStreamInternal(false, null, path, null, false);
      }

      /// <summary>[AlphaFS] Removes an alternate data stream (NTFS ADS) from an existing file.</summary>
      /// <param name="path">The path to an existing file.</param>
      /// <param name="name">The name of the stream to remove.</param>
      /// <remarks>This method only removes streams of type <see cref="T:StreamType.AlternateData"/>.</remarks>
      /// <remarks>No Exception is thrown if the stream does not exist.</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static void RemoveStream(string path, string name)
      {
         AlternateDataStreamInfo.RemoveStreamInternal(false, null, path, name, false);
      }

      #region Transacted

      #region IsFullPath

      /// <summary>[AlphaFS] Removes all alternate data streams (NTFS ADS) from an existing file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to an existing file.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <remarks>This method only removes streams of type <see cref="T:StreamType.AlternateData"/>.</remarks>
      /// <remarks>No Exception is thrown if the stream does not exist.</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static void RemoveStream(KernelTransaction transaction, string path, bool? isFullPath)
      {
         AlternateDataStreamInfo.RemoveStreamInternal(false, transaction, path, null, isFullPath);
      }

      /// <summary>[AlphaFS] Removes an alternate data stream (NTFS ADS) from an existing file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to an existing file.</param>
      /// <param name="name">The name of the stream to remove.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <remarks>This method only removes streams of type <see cref="T:StreamType.AlternateData"/>.</remarks>
      /// <remarks>No Exception is thrown if the stream does not exist.</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static void RemoveStream(KernelTransaction transaction, string path, string name, bool? isFullPath)
      {
         AlternateDataStreamInfo.RemoveStreamInternal(false, transaction, path, name, isFullPath);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Removes all alternate data streams (NTFS ADS) from an existing file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to an existing file.</param>
      /// <remarks>This method only removes streams of type <see cref="T:StreamType.AlternateData"/>.</remarks>
      /// <remarks>No Exception is thrown if the stream does not exist.</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static void RemoveStream(KernelTransaction transaction, string path)
      {
         AlternateDataStreamInfo.RemoveStreamInternal(false, transaction, path, null, false);
      }

      /// <summary>[AlphaFS] Removes an alternate data stream (NTFS ADS) from an existing file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to an existing file.</param>
      /// <param name="name">The name of the stream to remove.</param>
      /// <remarks>This method only removes streams of type <see cref="T:StreamType.AlternateData"/>.</remarks>
      /// <remarks>No Exception is thrown if the stream does not exist.</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static void RemoveStream(KernelTransaction transaction, string path, string name)
      {
         AlternateDataStreamInfo.RemoveStreamInternal(false, transaction, path, name, false);
      }
      
      #endregion Transacted

      #endregion // RemoveStream

      #region SetTimestamps

      #region IsFullPath

      /// <summary>[AlphaFS] Sets all the date and time stamps for the specified file, at once.</summary>
      /// <param name="path">The file for which to set the dates and times information.</param>
      /// <param name="creationTime">A <see cref="T:System.DateTime"/> containing the value to set for the creation date and time of <paramref name="path"/>. This value is expressed in local time.</param>
      /// <param name="lastAccessTime">A <see cref="T:System.DateTime"/> containing the value to set for the last access date and time of <paramref name="path"/>. This value is expressed in local time.</param>
      /// <param name="lastWriteTime">A <see cref="T:System.DateTime"/> containing the value to set for the last write date and time of <paramref name="path"/>. This value is expressed in local time.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static void SetTimestamps(string path, DateTime creationTime, DateTime lastAccessTime, DateTime lastWriteTime, bool? isFullPath)
      {
         SetFsoDateTimeInternal(false, null, path, creationTime.ToUniversalTime(), lastAccessTime.ToUniversalTime(), lastWriteTime.ToUniversalTime(), isFullPath);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Sets all the date and time stamps for the specified file, at once.</summary>
      /// <param name="path">The file for which to set the dates and times information.</param>
      /// <param name="creationTime">A <see cref="T:System.DateTime"/> containing the value to set for the creation date and time of <paramref name="path"/>. This value is expressed in local time.</param>
      /// <param name="lastAccessTime">A <see cref="T:System.DateTime"/> containing the value to set for the last access date and time of <paramref name="path"/>. This value is expressed in local time.</param>
      /// <param name="lastWriteTime">A <see cref="T:System.DateTime"/> containing the value to set for the last write date and time of <paramref name="path"/>. This value is expressed in local time.</param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static void SetTimestamps(string path, DateTime creationTime, DateTime lastAccessTime, DateTime lastWriteTime)
      {
         SetFsoDateTimeInternal(false, null, path, creationTime.ToUniversalTime(), lastAccessTime.ToUniversalTime(), lastWriteTime.ToUniversalTime(), false);
      }

      #region Transacted

      #region IsFullPath

      /// <summary>[AlphaFS] Sets all the date and time stamps for the specified file, at once.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file for which to set the dates and times information.</param>
      /// <param name="creationTime">A <see cref="T:System.DateTime"/> containing the value to set for the creation date and time of <paramref name="path"/>. This value is expressed in local time.</param>
      /// <param name="lastAccessTime">A <see cref="T:System.DateTime"/> containing the value to set for the last access date and time of <paramref name="path"/>. This value is expressed in local time.</param>
      /// <param name="lastWriteTime">A <see cref="T:System.DateTime"/> containing the value to set for the last write date and time of <paramref name="path"/>. This value is expressed in local time.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static void SetTimestamps(KernelTransaction transaction, string path, DateTime creationTime, DateTime lastAccessTime, DateTime lastWriteTime, bool? isFullPath)
      {
         SetFsoDateTimeInternal(false, transaction, path, creationTime.ToUniversalTime(), lastAccessTime.ToUniversalTime(), lastWriteTime.ToUniversalTime(), isFullPath);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Sets all the date and time stamps for the specified file, at once.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file for which to set the dates and times information.</param>
      /// <param name="creationTime">A <see cref="T:System.DateTime"/> containing the value to set for the creation date and time of <paramref name="path"/>. This value is expressed in local time.</param>
      /// <param name="lastAccessTime">A <see cref="T:System.DateTime"/> containing the value to set for the last access date and time of <paramref name="path"/>. This value is expressed in local time.</param>
      /// <param name="lastWriteTime">A <see cref="T:System.DateTime"/> containing the value to set for the last write date and time of <paramref name="path"/>. This value is expressed in local time.</param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static void SetTimestamps(KernelTransaction transaction, string path, DateTime creationTime, DateTime lastAccessTime, DateTime lastWriteTime)
      {
         SetFsoDateTimeInternal(false, transaction, path, creationTime.ToUniversalTime(), lastAccessTime.ToUniversalTime(), lastWriteTime.ToUniversalTime(), false);
      }

      #endregion // Transacted

      #endregion // SetTimestamps

      #region SetTimestampsUtc

      #region IsFullPath

      /// <summary>[AlphaFS] Sets all the date and time stamps, in coordinated universal time (UTC), for the specified file, at once.</summary>
      /// <param name="path">The file for which to set the dates and times information.</param>
      /// <param name="creationTimeUtc">A <see cref="T:System.DateTime"/> containing the value to set for the creation date and time of <paramref name="path"/>. This value is expressed in UTC time.</param>
      /// <param name="lastAccessTimeUtc">A <see cref="T:System.DateTime"/> containing the value to set for the last access date and time of <paramref name="path"/>. This value is expressed in UTC time.</param>
      /// <param name="lastWriteTimeUtc">A <see cref="T:System.DateTime"/> containing the value to set for the last write date and time of <paramref name="path"/>. This value is expressed in UTC time.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static void SetTimestampsUtc(string path, DateTime creationTimeUtc, DateTime lastAccessTimeUtc, DateTime lastWriteTimeUtc, bool? isFullPath)
      {
         SetFsoDateTimeInternal(false, null, path, creationTimeUtc, lastAccessTimeUtc, lastWriteTimeUtc, isFullPath);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Sets all the date and time stamps, in coordinated universal time (UTC), for the specified file, at once.</summary>
      /// <param name="path">The file for which to set the dates and times information.</param>
      /// <param name="creationTimeUtc">A <see cref="T:System.DateTime"/> containing the value to set for the creation date and time of <paramref name="path"/>. This value is expressed in UTC time.</param>
      /// <param name="lastAccessTimeUtc">A <see cref="T:System.DateTime"/> containing the value to set for the last access date and time of <paramref name="path"/>. This value is expressed in UTC time.</param>
      /// <param name="lastWriteTimeUtc">A <see cref="T:System.DateTime"/> containing the value to set for the last write date and time of <paramref name="path"/>. This value is expressed in UTC time.</param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static void SetTimestampsUtc(string path, DateTime creationTimeUtc, DateTime lastAccessTimeUtc, DateTime lastWriteTimeUtc)
      {
         SetFsoDateTimeInternal(false, null, path, creationTimeUtc, lastAccessTimeUtc, lastWriteTimeUtc, false);
      }

      #region Transacted

      #region IsFullPath

      /// <summary>[AlphaFS] Sets all the date and time stamps, in coordinated universal time (UTC), for the specified file, at once.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file for which to set the dates and times information.</param>
      /// <param name="creationTimeUtc">A <see cref="T:System.DateTime"/> containing the value to set for the creation date and time of <paramref name="path"/>. This value is expressed in UTC time.</param>
      /// <param name="lastAccessTimeUtc">A <see cref="T:System.DateTime"/> containing the value to set for the last access date and time of <paramref name="path"/>. This value is expressed in UTC time.</param>
      /// <param name="lastWriteTimeUtc">A <see cref="T:System.DateTime"/> containing the value to set for the last write date and time of <paramref name="path"/>. This value is expressed in UTC time.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static void SetTimestampsUtc(KernelTransaction transaction, string path, DateTime creationTimeUtc, DateTime lastAccessTimeUtc, DateTime lastWriteTimeUtc, bool? isFullPath)
      {
         SetFsoDateTimeInternal(false, transaction, path, creationTimeUtc, lastAccessTimeUtc, lastWriteTimeUtc, isFullPath);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Sets all the date and time stamps, in coordinated universal time (UTC), for the specified file, at once.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file for which to set the dates and times information.</param>
      /// <param name="creationTimeUtc">A <see cref="T:System.DateTime"/> containing the value to set for the creation date and time of <paramref name="path"/>. This value is expressed in UTC time.</param>
      /// <param name="lastAccessTimeUtc">A <see cref="T:System.DateTime"/> containing the value to set for the last access date and time of <paramref name="path"/>. This value is expressed in UTC time.</param>
      /// <param name="lastWriteTimeUtc">A <see cref="T:System.DateTime"/> containing the value to set for the last write date and time of <paramref name="path"/>. This value is expressed in UTC time.</param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static void SetTimestampsUtc(KernelTransaction transaction, string path, DateTime creationTimeUtc, DateTime lastAccessTimeUtc, DateTime lastWriteTimeUtc)
      {
         SetFsoDateTimeInternal(false, transaction, path, creationTimeUtc, lastAccessTimeUtc, lastWriteTimeUtc, false);
      }

      #endregion // Transacted

      #endregion // SetTimestampsUtc

      #region TransferTimestamps

      #region IsFullPath

      /// <summary>[AlphaFS] Transfers the date and time stamps for the specified files.</summary>
      /// <param name="source">The source file to get the date and time stamps from.</param>
      /// <param name="destination">The destination file to set the date and time stamps.</param>
      /// <remarks>This method does not change last access time for the source file.</remarks>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="source"/> and <paramref name="destination"/> will be normalized and long path prefixed. <c>null</c> <paramref name="source"/> and <paramref name="destination"/> are already a full path with long path prefix, will be used as is.</param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static void TransferTimestamps(string source, string destination, bool? isFullPath)
      {
         TransferTimestampsInternal(false, null, source, destination, isFullPath);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Transfers the date and time stamps for the specified files.</summary>
      /// <param name="source">The source file to get the date and time stamps from.</param>
      /// <param name="destination">The destination file to set the date and time stamps.</param>
      /// <remarks>This method does not change last access time for the source file.</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static void TransferTimestamps(string source, string destination)
      {
         TransferTimestampsInternal(false, null, source, destination, false);
      }

      #region Transacted

      #region IsFullPath

      /// <summary>[AlphaFS] Transfers the date and time stamps for the specified files.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="source">The source file to get the date and time stamps from.</param>
      /// <param name="destination">The destination file to set the date and time stamps.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="source"/> and <paramref name="destination"/> will be normalized and long path prefixed. <c>null</c> <paramref name="source"/> and <paramref name="destination"/> are already a full path with long path prefix, will be used as is.</param>
      /// <remarks>This method does not change last access time for the source file.</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static void TransferTimestamps(KernelTransaction transaction, string source, string destination, bool? isFullPath)
      {
         TransferTimestampsInternal(false, transaction, source, destination, isFullPath);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Transfers the date and time stamps for the specified files.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="source">The source file to get the date and time stamps from.</param>
      /// <param name="destination">The destination file to set the date and time stamps.</param>
      /// <remarks>This method does not change last access time for the source file.</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static void TransferTimestamps(KernelTransaction transaction, string source, string destination)
      {
         TransferTimestampsInternal(false, transaction, source, destination, false);
      }
      
      #endregion // Transacted

      #endregion // TransferTimestamps


      #region Unified Internals

      #region AppendTextInternal

      /// <summary>[AlphaFS] Unified method AppendTextInternal() to create a <see cref="T:StreamWriter"/> that appends <see cref="T:NativeMethods.DefaultFileEncoding"/> encoded text to an existing file, or to a new file if the specified file does not exist.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to the file to append to.</param>
      /// <param name="encoding">The character <see cref="T:Encoding"/> to use.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>A stream writer that appends <see cref="T:NativeMethods.DefaultFileEncoding"/> encoded text to the specified file or to a new file.</returns>
      [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
      [SecurityCritical]
      internal static StreamWriter AppendTextInternal(KernelTransaction transaction, string path, Encoding encoding, bool? isFullPath)
      {
         FileStream fs = OpenInternal(transaction, path, FileMode.OpenOrCreate, 0, FileAccess.Write, FileShare.None, EFileAttributes.Normal, isFullPath);

         try
         {
            fs.Seek(0, SeekOrigin.End);
            return new StreamWriter(fs, encoding);
         }
         catch (IOException)
         {
            fs.Dispose();
            throw;
         }
      }

      #endregion // AppendTextInternal

      #region CopyMoveInternal

      /// <summary>[AlphaFS] Unified method CopyMoveInternal() to copy/move a Non-/Transacted file or directory including its children.
      /// You can provide a callback function that receives progress notifications.</summary>
      /// <param name="isMove"><c>true</c> indicates a file move, <c>false</c> indicates a file copy.</param>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourceFileName">The source directory path.</param>
      /// <param name="destFileName">The destination directory path.</param>
      /// <param name="preserveDates"><c>true</c> if original Timestamps must be preserved, <c>false</c> otherwise. This parameter is ignored for move operations.</param>
      /// <param name="copyOptions"><see cref="T:CopyOptions"/> that specify how the file is to be copied. This parameter can be <c>null</c>.</param>
      /// <param name="moveOptions">Flags that specify how the file is to be move. This parameter can be <c>null</c>.</param>
      /// <param name="copyMoveProgress">A callback function that is called each time another portion of the file has been copied/moved. This parameter can be <c>null</c>.</param>
      /// <param name="userProgressData">The argument to be passed to the callback function. This parameter can be <c>null</c>.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="sourceFileName"/> and <paramref name="destFileName"/> will be normalized and long path prefixed. <c>null</c> <paramref name="sourceFileName"/> and <paramref name="destFileName"/> are already a full path with long path prefix, will be used as is.</param>
      /// <remarks>This Move method works across disk volumes, and it does not throw an exception if the source and destination are
      /// the same. Note that if you attempt to replace a file by moving a file of the same name into that directory, you
      /// get an IOException. You cannot use the Move method to overwrite an existing file.</remarks>
      /// <returns><c>true</c> when successfully copied or moved, <c>false</c> otherwise or the operation was aborted.</returns>
      /// <exception cref="NativeError.ThrowException()"/>
      [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
      [SecurityCritical]
      internal static void CopyMoveInternal(bool isMove, KernelTransaction transaction, string sourceFileName, string destFileName, bool preserveDates, CopyOptions? copyOptions, MoveOptions? moveOptions, CopyMoveProgressCallback copyMoveProgress, object userProgressData, bool? isFullPath)
      {
         if (Utils.IsNullOrWhiteSpace(sourceFileName))
            throw new ArgumentNullException("sourceFileName");

         if (Utils.IsNullOrWhiteSpace(destFileName))
            throw new ArgumentNullException("destFileName");

         string sourceFileNameLp = isFullPath == null
            ? sourceFileName
            : (bool) isFullPath
            ? Path.GetLongPathInternal(sourceFileName, false, false, false, false)
            : Path.GetFullPathInternal(transaction, sourceFileName, true, false, false, true);

         string destFileNameLp = isFullPath == null
            ? destFileName
            : (bool) isFullPath
            ? Path.GetLongPathInternal(destFileName, false, false, false, false)
            : Path.GetFullPathInternal(transaction, destFileName, true, false, false, false);

         
         // Setup callback function for progress notifications.
         NativeMethods.CopyMoveProgressDelegate routine = (copyMoveProgress != null)
            ? (totalFileSize, totalBytesTransferred, streamSize, streamBytesTransferred, dwStreamNumber, dwCallbackReason, hSourceFile, hDestinationFile, lpData) =>
               copyMoveProgress(totalFileSize, totalBytesTransferred, streamSize, streamBytesTransferred, dwStreamNumber, dwCallbackReason, userProgressData)
            : (NativeMethods.CopyMoveProgressDelegate) null;

         // MoveFileWithProgress() / MoveFileTransacted
         // MoveOptions.CopyAllowed: If the file is to be moved to a different volume,
         // the function simulates the move by using the CopyFile and DeleteFile functions.

         // Determine Copy or Move action.
         int cancel;
         bool doCopy = !isMove && copyOptions != null && moveOptions == null;
         bool doMove = isMove && moveOptions != null && copyOptions == null;
         bool overwrite = doCopy
            ? ((CopyOptions) copyOptions & CopyOptions.FailIfExists) == CopyOptions.None  // copyOptions does NOT contain CopyOptions.FailIfExists.
            : doMove && ((MoveOptions) moveOptions & MoveOptions.ReplaceExisting) != MoveOptions.None;  // moveOptions does NOT contain MoveOptions.ReplaceExisting.


         // Remove file read-only attribute, if applicable.
         // If the file is read-only, the function can fail with ERROR_ACCESS_DENIED.
         if (overwrite)
            DeleteFileInternal(transaction, destFileNameLp, true, null);


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

                       ? NativeMethods.MoveFileWithProgress(sourceFileNameLp, destFileNameLp, routine, IntPtr.Zero, (MoveOptions)moveOptions)
                       : NativeMethods.CopyFileEx(sourceFileNameLp, destFileNameLp, routine, IntPtr.Zero, out cancel, copyOptions ?? CopyOptions.FailIfExists)
                  : doMove

                       ? NativeMethods.MoveFileTransacted(sourceFileNameLp, destFileNameLp, routine, IntPtr.Zero, (MoveOptions)moveOptions, transaction.SafeHandle)
                       : NativeMethods.CopyFileTransacted(sourceFileNameLp, destFileNameLp, routine, IntPtr.Zero, out cancel, copyOptions ?? CopyOptions.FailIfExists, transaction.SafeHandle)))
         {
            int lastError = Marshal.GetLastWin32Error();

            // MSDN
            //
            // If lpProgressRoutine returns PROGRESS_CANCEL due to the user canceling the operation,
            // CopyFileEx will return zero and GetLastError will return ERROR_REQUEST_ABORTED.
            // In this case, the partially copied destination file is deleted.
            //
            // If lpProgressRoutine returns PROGRESS_STOP due to the user stopping the operation,
            // CopyFileEx will return zero and GetLastError will return ERROR_REQUEST_ABORTED.
            // In this case, the partially copied destination file is left intact.

            if ((uint) lastError == Win32Errors.ERROR_REQUEST_ABORTED)
               return;

            NativeError.ThrowException(lastError, sourceFileName, destFileName);
         }

         // Apply original Timestamps if requested and action is Copy().
         if (preserveDates && doCopy)
         {
            NativeMethods.Win32FileAttributeData originalAttrs = GetAttributesExInternal(false, transaction, sourceFileNameLp, true, false, null);
            
            SetFsoDateTimeInternal(false, transaction, destFileNameLp,
               DateTime.FromFileTimeUtc(originalAttrs.CreationTime),
               DateTime.FromFileTimeUtc(originalAttrs.LastAccessTime),
               DateTime.FromFileTimeUtc(originalAttrs.LastWriteTime), null);
         }
      }

      #endregion // CopyMoveInternal

      #region CreateFileInternal

      /// <summary>[AlphaFS] Unified method CreateFileInternal() to create or overwrite a file in the specified path, specifying a buffer size and advanced options:
      /// <see cref="T:EFileAttributes"/>, <see cref="T:FileSecurity"/>, <see cref="T:FileMode"/>, <see cref="T:FileAccess"/>, <see cref="T:FileShare"/>.
      /// </summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The name of the file.</param>
      /// <param name="bufferSize">The number of bytes buffered for reads and writes to the file.</param>
      /// <param name="attributes">The <see cref="T:EFileAttributes"/> additional advanced options to create a file.</param>
      /// <param name="fileSecurity">A <see cref="T:FileSecurity"/> instance that determines the access control and audit security for the file.</param>
      /// <param name="mode">The <see cref="T:FileMode"/> option gives you more precise control over how you want to create a file.</param>
      /// <param name="access">The <see cref="T:FileAccess"/> allow you additionally specify to default read/write capability - just write, bypassing any cache.</param>
      /// <param name="share">The <see cref="T:FileShare"/> option controls how you would like to share created file with other requesters.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>A <see cref="T:FileStream"/> that provides read/write access to the file specified in path.</returns>
      [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
      [SecurityCritical]
      internal static FileStream CreateFileInternal(KernelTransaction transaction, string path, int bufferSize, EFileAttributes attributes, FileSecurity fileSecurity, FileMode mode, FileAccess access, FileShare share, bool? isFullPath)
      {
         SafeFileHandle handle = CreateFileInternal(true, transaction, path, attributes, fileSecurity, mode, (FileSystemRights) access, share, isFullPath);
         return new FileStream(handle, access, bufferSize, (attributes & EFileAttributes.Overlapped) != 0);
      }

      /// <summary>[AlphaFS] Unified method CreateFileInternal() to create or open a file, directory or I/O device.</summary>
      /// <param name="isFile"><c>null</c> indicates a device. <c>true</c> indicates a file object. <c>false</c> indicates a folder object.</param>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path and name of the file or directory to create.</param>
      /// <param name="attributes">One of the <see cref="T:EFileAttributes"/> values that describes how to create or overwrite the file or directory.</param>
      /// <param name="fileSecurity">A <see cref="T:FileSecurity"/> instance that determines the access control and audit security for the file or directory.</param>
      /// <param name="fileMode">A <see cref="T:FileMode"/> constant that determines how to open or create the file or directory.</param>
      /// <param name="fileSystemRights">A <see cref="T:FileSystemRights"/> constant that determines the access rights to use when creating access and audit rules for the file or directory.</param>
      /// <param name="fileShare">A <see cref="T:FileShare"/> constant that determines how the file or directory will be shared by processes.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <remarks>To obtain a directory handle using CreateFile, specify the FILE_FLAG_BACKUP_SEMANTICS flag as part of dwFlagsAndAttributes.</remarks>
      /// <remarks>The most commonly used I/O devices are as follows: file, file stream, directory, physical disk, volume, console buffer, tape drive, communications resource, mailslot, and pipe.</remarks>
      /// <returns>A <see cref="T:SafeFileHandle"/> that provides read/write access to the file or directory specified by <paramref name="path"/>.</returns>
      [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Object needs to be disposed by caller.")]
      [SecurityCritical]
      internal static SafeFileHandle CreateFileInternal(bool? isFile, KernelTransaction transaction, string path, EFileAttributes attributes, FileSecurity fileSecurity, FileMode fileMode, FileSystemRights fileSystemRights, FileShare fileShare, bool? isFullPath)
      {
         if (Utils.IsNullOrWhiteSpace(path))
            throw new ArgumentNullException("path");

         PrivilegeEnabler privilegeEnabler = null;
         try
         {
            if (fileSecurity != null)
               fileSystemRights |= (FileSystemRights)0x1000000;

            // AccessSystemSecurity = 0x1000000    AccessSystemAcl access type.
            // MaximumAllowed       = 0x2000000    MaximumAllowed access type.            
            if ((fileSystemRights & (FileSystemRights)0x1000000) != 0 ||
                (fileSystemRights & (FileSystemRights)0x2000000) != 0)
               privilegeEnabler = new PrivilegeEnabler(Privilege.Security);


            using (Security.NativeMethods.SecurityAttributes securityAttributes = new Security.NativeMethods.SecurityAttributes(fileSecurity))
            {
               // When isFile == null, we're working with a device.
               // When opening a VOLUME or removable media drive (for example, a floppy disk drive or flash memory thumb drive),
               // the path string should be the following form: "\\.\X:"
               // Do not use a trailing backslash (\), which indicates the root.

               string pathLp = isFullPath == null
                  ? path
                  : (bool)isFullPath
                     ? Path.GetLongPathInternal(path, false, false, false, (isFile == null || (bool)isFile))
                     : Path.GetFullPathInternal(transaction, path, true, false, false, (isFile == null || (bool)isFile));

               SafeFileHandle handle = transaction == null || !NativeMethods.IsAtLeastWindowsVista

                  // CreateFile() / CreateFileTransacted()
                  // In the ANSI version of this function, the name is limited to MAX_PATH characters.
                  // To extend this limit to 32,767 wide characters, call the Unicode version of the function and prepend "\\?\" to the path.
                  // 2013-01-13: MSDN confirms LongPath usage.

                  ? NativeMethods.CreateFile(pathLp, fileSystemRights, fileShare, securityAttributes, fileMode, attributes, IntPtr.Zero)
                  : NativeMethods.CreateFileTransacted(pathLp, fileSystemRights, fileShare, securityAttributes, fileMode, attributes, IntPtr.Zero, transaction.SafeHandle, IntPtr.Zero, IntPtr.Zero);

               int lastError = Marshal.GetLastWin32Error();
               switch ((uint)lastError)
               {
                  case Win32Errors.ERROR_PATH_NOT_FOUND:
                     if (pathLp.Equals(Path.GetPathRoot(pathLp), StringComparison.OrdinalIgnoreCase))
                        lastError = 5;
                     break;

                  case Win32Errors.ERROR_FILE_NOT_FOUND:
                     lastError = (int)((isFile == null || (bool)!isFile)
                        ? Win32Errors.ERROR_PATH_NOT_FOUND
                        : Win32Errors.ERROR_FILE_NOT_FOUND);
                     break;
               }

               if (handle.IsInvalid)
               {
                  handle.Close();
                  NativeError.ThrowException(lastError, pathLp);
               }

               return handle;
            }
         }
         finally
         {
            if (privilegeEnabler != null)
               privilegeEnabler.Dispose();
         }
      }

      #endregion // CreateFileInternal

      #region CreateHardlinkInternal

      /// <summary>[AlphaFS] Unified method CreateHardlinkInternal() to establish a hard link between an existing file and a new file. This function is only supported on the NTFS file system, and only for files, not directories.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="fileName">The name of the new file. This parameter cannot specify the name of a directory.</param>
      /// <param name="existingFileName">The name of the existing file. This parameter cannot specify the name of a directory.</param>
      /// /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="fileName"/> and <paramref name="existingFileName"/> will be normalized and long path prefixed. <c>null</c> <paramref name="fileName"/> and <paramref name="existingFileName"/> are already a full path with long path prefix, will be used as is.</param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Hardlink")]
      [SecurityCritical]
      internal static void CreateHardlinkInternal(KernelTransaction transaction, string fileName, string existingFileName, bool? isFullPath)
      {
         string fileNameLp = isFullPath == null
            ? fileName
            : (bool) isFullPath
            ? Path.GetLongPathInternal(fileName, false, false, false, false)
            : Path.GetFullPathInternal(transaction, fileName, true, false, false, true);

         string existingFileNameLp = isFullPath == null
            ? existingFileName
            : (bool) isFullPath
            ? Path.GetLongPathInternal(existingFileName, false, false, false, false)
            : Path.GetFullPathInternal(transaction, existingFileName, true, false, false, true);


         if (!(transaction == null || !NativeMethods.IsAtLeastWindowsVista

            // CreateHardLink() / CreateHardLinkTransacted()
            // In the ANSI version of this function, the name is limited to MAX_PATH characters.
            // To extend this limit to 32,767 wide characters, call the Unicode version of the function and prepend "\\?\" to the path.
            // 2013-01-13: MSDN does not confirm LongPath usage but a Unicode version of this function exists.

            ? NativeMethods.CreateHardLink(fileNameLp, existingFileNameLp, IntPtr.Zero)
            : NativeMethods.CreateHardLinkTransacted(fileNameLp, existingFileNameLp, IntPtr.Zero, transaction.SafeHandle)))
         {
            int lastError = Marshal.GetLastWin32Error();
            switch ((uint) lastError)
            {
               case Win32Errors.ERROR_INVALID_FUNCTION:
                  throw new NotSupportedException(Resources.HardLinksOnNonNTFSPartitionsIsNotSupported);

               default:
                  NativeError.ThrowException(lastError, fileName, existingFileName);
                  break;
            }
         }
      }

      #endregion // CreateHardlinkInternal

      #region CreateSymbolicLinkInternal

      /// <summary>[AlphaFS] Unified method CreateSymbolicLinkInternal() to create a symbolic link.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="symlinkFileName">The name of the target for the symbolic link to be created.</param>
      /// <param name="targetFileName">The symbolic link to be created.</param>
      /// <param name="targetType">Indicates whether the link target, <paramref name="targetFileName"/>, is a file or directory.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="symlinkFileName"/> and <paramref name="targetFileName"/> will be normalized and long path prefixed. <c>null</c> <paramref name="symlinkFileName"/> and <paramref name="targetFileName"/> are already a full path with long path prefix, will be used as is.</param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      internal static void CreateSymbolicLinkInternal(KernelTransaction transaction, string symlinkFileName, string targetFileName, SymbolicLinkTarget targetType, bool? isFullPath)
      {
         string symlinkFileNameLp = isFullPath == null
            ? symlinkFileName
            : (bool) isFullPath
            ? Path.GetLongPathInternal(symlinkFileName, false, false, false, false)
            : Path.GetFullPathInternal(transaction, symlinkFileName, true, false, false, true);

         string targetFileNameLp = isFullPath == null
            ? targetFileName
            : (bool) isFullPath
            ? Path.GetLongPathInternal(targetFileName, false, false, false, false)
            : Path.GetFullPathInternal(transaction, targetFileName, true, false, false, true);

         if (!(transaction == null || !NativeMethods.IsAtLeastWindowsVista

            // CreateSymbolicLink() / CreateSymbolicLinkTransacted()
            // In the ANSI version of this function, the name is limited to MAX_PATH characters.
            // To extend this limit to 32,767 wide characters, call the Unicode version of the function and prepend "\\?\" to the path.
            // 2014-02-14: MSDN does not confirm LongPath usage but a Unicode version of this function exists.

            ? NativeMethods.CreateSymbolicLink(symlinkFileNameLp, targetFileNameLp, targetType)
            : NativeMethods.CreateSymbolicLinkTransacted(symlinkFileNameLp, targetFileNameLp, targetType, transaction.SafeHandle)))
            NativeError.ThrowException(symlinkFileNameLp, targetFileNameLp);
      }

      #endregion // CreateSymbolicLinkInternal

      #region CreateTextInternal

      /// <summary>[AlphaFS] Unified method CreateTextInternal() to create or open a file for writing <see cref="T:Encoding"/> encoded text.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to be opened for writing.</param>
      /// <param name="encoding">The <see cref="T:Encoding"/> applied to the contents of the file.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>A <see cref="T:StreamWriter"/> that writes to the specified file using <see cref="T:NativeMethods.DefaultFileBufferSize"/> encoding.</returns>
      [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
      [SecurityCritical]
      internal static StreamWriter CreateTextInternal(KernelTransaction transaction, string path, Encoding encoding, bool? isFullPath)
      {
         return new StreamWriter(CreateFileInternal(transaction, path, NativeMethods.DefaultFileBufferSize, EFileAttributes.SequentialScan, null, FileMode.Create, FileAccess.Write, FileShare.Read, isFullPath), encoding);
      }

      #endregion // CreateTextInternal

      #region DeleteFileInternal

      /// <summary>[AlphaFS] Unified method DeleteFileInternal() to delete a Non-/Transacted file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The name of the file to be deleted.</param>
      /// <param name="ignoreReadOnly"><c>true</c> overrides the read only <see cref="T:FileAttributes"/> of the file.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <remarks>If the file to be deleted does not exist, no exception is thrown.</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      internal static void DeleteFileInternal(KernelTransaction transaction, string path, bool ignoreReadOnly, bool? isFullPath)
      {
         if (Utils.IsNullOrWhiteSpace(path))
            throw new ArgumentNullException("path");

         string pathLp = isFullPath == null
            ? path
            : (bool) isFullPath
               ? Path.GetLongPathInternal(path, false, false, false, false)
               : Path.GetFullPathInternal(transaction, path, true, false, false, true);
         
         // Remove file read-only attribute, if applicable.
         // If the file is read-only, the function DeleteFile() can fail with ERROR_ACCESS_DENIED.
         if (ignoreReadOnly)
            SetAttributesInternal(false, transaction, pathLp, FileAttributes.Normal, true, null);

         // If the path points to a symbolic link, the symbolic link is deleted, not the target.
         
         if (!(transaction == null || !NativeMethods.IsAtLeastWindowsVista

            // DeleteFile() / DeleteFileTransacted()
            // In the ANSI version of this function, the name is limited to MAX_PATH characters.
            // To extend this limit to 32,767 wide characters, call the Unicode version of the function and prepend "\\?\" to the path.
            // 2013-01-13: MSDN confirms LongPath usage.

            ? NativeMethods.DeleteFile(pathLp)
            : NativeMethods.DeleteFileTransacted(pathLp, transaction.SafeHandle)))
         {
            int lastError = Marshal.GetLastWin32Error();

            // MSDN: If the file to be deleted does not exist, no exception is thrown.
            if (lastError != Win32Errors.ERROR_FILE_NOT_FOUND)
               NativeError.ThrowException(lastError, pathLp);
         }
      }

      #endregion // DeleteFileInternal

      #region EncryptDecryptDirectoryInternal

      /// <summary>[AlphaFS] Unified method EncryptDecryptFileInternal() to decrypt/encrypt a file or directory so that only the account used to encrypt the file can decrypt it.</summary>
      /// <param name="path">A path that describes a directory to encrypt.</param>
      /// <param name="encrypt"><c>true</c> encrypt, <c>false</c> decrypt.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      internal static void EncryptDecryptDirectoryInternal(string path, bool encrypt, bool? isFullPath)
      {
         string pathLp = isFullPath == null
            ? path
            : (bool) isFullPath
               ? Path.GetLongPathInternal(path, false, false, false, false)
               : Path.GetFullPathInternal(null, path, true, false, false, false);

         // Process folders and files.
         foreach (string fso in EnumerateFileSystemEntryInfoInternal<string>(null, pathLp, Path.WildcardStarMatchAll, SearchOption.TopDirectoryOnly, null, true, true, false, null))
            EncryptDecryptFileInternal(fso, encrypt, null);

         // Process the root folder, the given path.
         EncryptDecryptFileInternal(pathLp, encrypt, null);
      }

      #endregion // EncryptDecryptDirectoryInternal

      #region EncryptDecryptFileInternal

      /// <summary>[AlphaFS] Unified method EncryptDecryptFileInternal() to decrypt/encrypt a file or directory so that only the account used to encrypt the file can decrypt it.</summary>
      /// <param name="path">A path that describes a file to encrypt.</param>
      /// <param name="encrypt"><c>true</c> encrypt, <c>false</c> decrypt.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      internal static void EncryptDecryptFileInternal(string path, bool encrypt, bool? isFullPath)
      {
         string pathLp = isFullPath == null
            ? path
            : (bool) isFullPath
               ? Path.GetLongPathInternal(path, false, false, false, false)
               : Path.GetFullPathInternal(null, path, true, false, false, true);

         // EncryptFile() / DecryptFile()
         // In the ANSI version of this function, the name is limited to 248 characters.
         // To extend this limit to 32,767 wide characters, call the Unicode version of the function and prepend "\\?\" to the path.
         // 2013-01-13: MSDN does not confirm LongPath usage but a Unicode version of this function exists.

         if (!(encrypt
            ? NativeMethods.EncryptFile(pathLp)
            : NativeMethods.DecryptFile(pathLp, 0)))
         {
            int lastError = Marshal.GetLastWin32Error();
            switch ((uint)lastError)
            {
               case Win32Errors.ERROR_ACCESS_DENIED:
                  string root = Path.GetPathRoot(pathLp, false);
                  if (!string.Equals("NTFS", new DriveInfo(root).DriveFormat, StringComparison.OrdinalIgnoreCase))
                     throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, "The drive does not support NTFS encryption: [{0}]", root));
                  break;

               default:
                  NativeError.ThrowException(lastError, pathLp);
                  break;
            }
         }
      }

      #endregion // EncryptDecryptFileInternal

      #region EnumerateHardlinksInternal

      /// <summary>[AlphaFS] Creates an enumeration of all the hard links to the specified <paramref name="path"/>.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The name of the file.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>An enumerable collection of <see cref="T:String"/> of all the hard links to the specified <paramref name="path"/></returns>
      internal static IEnumerable<string> EnumerateHardlinksInternal(KernelTransaction transaction, string path, bool? isFullPath)
      {
         if (!NativeMethods.IsAtLeastWindowsVista)
            throw new PlatformNotSupportedException(Resources.RequiresWindowsVistaOrHigher);

         string pathLp = isFullPath == null
            ? path
            : (bool)isFullPath
            ? Path.GetLongPathInternal(path, false, false, false, false)
            : Path.GetFullPathInternal(transaction, path, true, false, false, true);

         // Default buffer length, will be extended if needed, although this should not happen.
         uint length = NativeMethods.MaxPathUnicode;
         StringBuilder builder = new StringBuilder((int) length);


         getFindFirstFileName:

         using (SafeFindFileHandle handle = transaction == null

            // FindFirstFileName() / FindFirstFileNameTransacted()
            // In the ANSI version of this function, the name is limited to MAX_PATH characters.
            // To extend this limit to 32,767 wide characters, call the Unicode version of the function and prepend "\\?\" to the path.
            // 2013-01-13: MSDN does not confirm LongPath usage but a Unicode version of this function exists.

            ? NativeMethods.FindFirstFileName(pathLp, 0, out length, builder)
            : NativeMethods.FindFirstFileNameTransacted(pathLp, 0, out length, builder, transaction.SafeHandle))
         {
            if (handle.IsInvalid)
            {
               int lastError = Marshal.GetLastWin32Error();
               switch ((uint) lastError)
               {
                  case Win32Errors.ERROR_MORE_DATA:
                     builder = new StringBuilder((int) length);
                     handle.Close();
                     goto getFindFirstFileName;

                  default:
                     // If the function fails, the return value is INVALID_HANDLE_VALUE.
                     NativeError.ThrowException(lastError, pathLp);
                     break;
               }
            }

            yield return builder.ToString();


            //length = NativeMethods.MaxPathUnicode;
            //builder = new StringBuilder((int)length);

            do
            {
               while (!NativeMethods.FindNextFileName(handle, out length, builder))
               {
                  int lastError = Marshal.GetLastWin32Error();
                  switch ((uint) lastError)
                  {
                        // We've reached the end of the enumeration.
                     case Win32Errors.ERROR_HANDLE_EOF:
                        yield break;

                     case Win32Errors.ERROR_MORE_DATA:
                        builder = new StringBuilder((int) length);
                        continue;

                     default:
                        //If the function fails, the return value is zero (0).
                        NativeError.ThrowException(lastError);
                        break;
                  }
               }

               yield return builder.ToString();

            } while (true);
         }
      }

      #endregion // EnumerateHardlinksInternal

      #region EnumerateFileSystemEntryInfoInternal

      /// <summary>[AlphaFS] Unified method EnumerateFileSystemEntryInfoInternal() to enumerate Non-/Transacted files/directories.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory to search.</param>
      /// <param name="searchPattern">The search string to match against the names of directories in <paramref name="path"/>. This parameter can contain a combination of valid literal path and wildcard (<see cref="T:Path.WildcardStarMatchAll"/> and <see cref="T:Path.WildcardQuestion"/>) characters, but doesn't support regular expressions.</param>
      /// <param name="searchOption">One of the <see cref="T:SearchOption"/> enumeration values that specifies whether the <paramref name="searchOption"/> should include only the current directory or should include all subdirectories.</param>
      /// <param name="getFolders">
      /// <c>true</c> folders will be returned.
      /// <c>false</c> files will be returned.
      /// <c>null</c> both folders and files will be returned.
      /// </param>
      /// <param name="getAsString">
      /// <c>true</c> returns the results as an enumerable <see cref="T:string"/> object.
      /// <c>false</c> returns the results as an enumerable <see cref="T:FileSystemInfo"/> object.
      /// <c>null</c> returns the results as an enumerable <see cref="T:FileSystemEntryInfo"/> object.
      /// </param>
      /// <param name="asLongPath"><c>true</c> returns the full path in Unicode (LongPath) format.</param>
      /// <param name="continueOnException"><c>true</c> suppress any Exception that might be thrown a result from a failure, such as ACLs protected directories or non-accessible reparse points.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>
      /// If <paramref name="getAsString"/> is <c>null</c> an enumerable <see cref="T:FileSystemEntryInfo"/> collection that match <paramref name="searchPattern"/> and <paramref name="searchOption"/>.
      /// If <paramref name="getAsString"/> is <c>true</c> an enumerable <see cref="T:string"/> collection of the full pathnames that match searchPattern and searchOption.
      /// If <paramref name="getAsString"/> is <c>false</c>, an enumerable <see cref="T:FileSystemInfo"/> (<see cref="T:DirectoryInfo"/> / <see cref="T:FileInfo"/>) collection that match <paramref name="searchPattern"/> and <paramref name="searchOption"/>.
      /// </returns>
      [SecurityCritical]
      internal static IEnumerable<T> EnumerateFileSystemEntryInfoInternal<T>(KernelTransaction transaction, string path, string searchPattern, SearchOption searchOption, bool? getFolders, bool? getAsString, bool asLongPath, bool continueOnException, bool? isFullPath)
      {
         foreach (FileSystemEntryInfo fsei in new FindFileSystemEntryInfo
         {
            IsFullPath = isFullPath,
            InputPath = path,
            AsLongPath = asLongPath,
            FileSystemObjectType = getFolders,
            SearchOption = searchOption,
            SearchPattern = searchPattern,
            Transaction = transaction,
            ContinueOnException = continueOnException

         }.Enumerate())
         {
            // Return FullPath property as string.
            if (getAsString != null && (bool)getAsString)
               yield return (T)(object)(asLongPath ? fsei.LongFullPath : fsei.FullPath);

            else
            {
               // Return FileSystemEntryInfo instance.
               if (getAsString == null)
               {
                  switch (getFolders)
                  {
                     case true:
                        if (fsei.IsDirectory)
                           yield return (T)(object)fsei;
                        break;

                     case false:
                        if (!fsei.IsDirectory)
                           yield return (T)(object)fsei;
                        break;

                     default:
                        yield return (T)(object)fsei;
                        break;
                  }
               }
               else
               {
                  // Return a specific instance of type: FileSystemInfo, DirectoryInfo or FileInfo.
                  // Bonus: the returned FileSystemEntryInfo instance is constructed from a Win32FindData data structure
                  // with properties already populated by the Win32 FindFirstFileEx() function.
                  // This means that the returned DirectoryInfo/FileInfo instance is already .Refresh()-ed.
                  // I call it: Cached LazyLoading.

                  switch (getFolders)
                  {
                     // true = return instance of type: DirectoryInfo.
                     case true:
                        yield return (T)(object)new DirectoryInfo(transaction, fsei.LongFullPath, true) { EntryInfo = fsei };
                        break;

                     // false = return instance of type: FileInfo.
                     case false:
                        yield return (T)(object)new FileInfo(transaction, fsei.LongFullPath, true) { EntryInfo = fsei };
                        break;

                     // null = return instances of type: DirectoryInfo or FileInfo.
                     default:
                        yield return (T)(object)(fsei.IsDirectory
                              ? (FileSystemInfo)new DirectoryInfo(transaction, fsei.LongFullPath, true) { EntryInfo = fsei }
                              : new FileInfo(transaction, fsei.LongFullPath, true) { EntryInfo = fsei });
                        break;
                  }
               }
            }
         }
      }

      #endregion // EnumerateFileSystemEntryInfoInternal

      #region ExistsInternal

      /// <summary>[AlphaFS] Unified method ExistsInternal() to determine whether the given path refers to an existing file or directory on disk.</summary>
      /// <param name="isFolder">The main reason for this parameter is to throw a more appropriate error: DirectoryNotFound vs FileNotFound. <c>true</c> indicates a directory object, DirectoryNotFound will be thrown. <c>false</c> indicates a file object.</param>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to test.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns><c>true</c> on success, <c>false</c> otherwise.</returns>
      /// <remarks>Note that this files may contain wildcards, such as '*'</remarks>
      /// <remarks>Trailing spaces are removed from the end of the path parameter before checking whether the directory exists.</remarks>
      /// <remarks>Trailing spaces are removed from the path parameter before determining if the file exists.</remarks>
      /// <remarks>Return value is <c>true</c> if the caller has the required permissions and path contains the name of an existing file; <c>false</c> otherwise.</remarks>
      /// <remarks>This method also returns <c>false</c> if path is a <c>null</c> reference, an invalid path, or a zero-length string.</remarks>
      /// <remarks>If the caller does not have sufficient permissions to read the specified file, no exception is thrown and the method returns <c>false</c> regardless of the existence of path.</remarks>
      [SecurityCritical]
      internal static bool ExistsInternal(bool isFolder, KernelTransaction transaction, string path, bool? isFullPath)
      {
         string pathLp = isFullPath == null
            ? path
            : (bool) isFullPath
            ? Path.GetLongPathInternal(path, false, false, false, false)
               : Path.GetFullPathInternal(transaction, path, true, false, false, false);

         // MSDN: Trailing spaces are removed from the end of the path parameter before checking whether the directory exists.
         // MSDN: Trailing spaces are removed from the path parameter before determining if the file exists.

         FileAttributes attrs = GetAttributesInternal(isFolder, transaction, pathLp, true, true, null);

         return attrs != (FileAttributes)(-1) && (isFolder
            ? (attrs & FileAttributes.Directory) == FileAttributes.Directory
            : (attrs & FileAttributes.Directory) != FileAttributes.Directory);
      }

      #endregion ExistsInternal

      #region GetAccessControlInternal

      /// <summary>[AlphaFS] Unified method GetAccessControlInternal() to get/set an <see cref="T:ObjectSecurity"/> for a particular file or directory.</summary>
      /// <param name="isFolder">The main reason for this parameter is to throw a more appropriate error: DirectoryNotFound vs FileNotFound. <c>true</c> indicates a directory object, DirectoryNotFound will be thrown. <c>false</c> indicates a file object.</param>
      /// <param name="path">The path to a directory containing a <see cref="T:DirectorySecurity"/> object that describes the directory's or file's access control list (ACL) information.</param>
      /// <param name="includeSections">One (or more) of the <see cref="T:AccessControlSections"/> values that specifies the type of access control list (ACL) information to receive.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>An <see cref="T:ObjectSecurity"/> object that encapsulates the access control rules for the file or directory described by the <paramref name="path"/> parameter. </returns>
      /// <exception cref="NativeError.ThrowException()"/>
      [SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
      [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
      [SecurityCritical]
      internal static T GetAccessControlInternal<T>(bool isFolder, string path, AccessControlSections includeSections, bool? isFullPath)
      {
         if (Utils.IsNullOrWhiteSpace(path))
            throw new ArgumentNullException("path");

         SecurityInformation securityInfo = 0;
         PrivilegeEnabler privilegeEnabler = null;

         if ((includeSections & AccessControlSections.Access) != 0)
            securityInfo |= SecurityInformation.Dacl;

         if ((includeSections & AccessControlSections.Group) != 0)
            securityInfo |= SecurityInformation.Group;

         if ((includeSections & AccessControlSections.Owner) != 0)
            securityInfo |= SecurityInformation.Owner;

         if ((includeSections & AccessControlSections.Audit) != 0)
         {
            // We need the SE_SECURITY_NAME privilege enabled to be able to get the
            // SACL descriptor. So we enable it here for the remainder of this function.
            privilegeEnabler = new PrivilegeEnabler(Privilege.Security);
            securityInfo |= SecurityInformation.Sacl;
         }

         using (privilegeEnabler)
         {
            string pathLp = isFullPath == null
               ? path
               : (bool) isFullPath
                  ? Path.GetLongPathInternal(path, false, false, false, false)
                  : Path.GetFullPathInternal(null, path, true, false, false, true);

            uint sizeRequired = 1024;

         startGetFileSecurity:

            using (SafeGlobalMemoryBufferHandle safeBuffer = new SafeGlobalMemoryBufferHandle((int)sizeRequired))
            {
               // GetFileSecurity()
               // Seems to perform better than GetNamedSecurityInfo() and doesn't require Administrator rights.
               // In the ANSI version of this function, the name is limited to MAX_PATH characters.
               // To extend this limit to 32,767 wide characters, call the Unicode version of the function and prepend "\\?\" to the path.
               // 2013-01-13: MSDN does not confirm LongPath usage but a Unicode version of this function exists.

               if (!Security.NativeMethods.GetFileSecurity(pathLp, securityInfo, safeBuffer, (uint)safeBuffer.Capacity, out sizeRequired))
               {
                  int lastError = Marshal.GetLastWin32Error();
                  switch ((uint)lastError)
                  {
                     case Win32Errors.ERROR_INSUFFICIENT_BUFFER:
                        safeBuffer.Dispose();
                        goto startGetFileSecurity;

                     case Win32Errors.ERROR_FILE_NOT_FOUND:
                     case Win32Errors.ERROR_PATH_NOT_FOUND:
                        lastError = (int)(isFolder ? Win32Errors.ERROR_PATH_NOT_FOUND : Win32Errors.ERROR_FILE_NOT_FOUND);
                        break;
                  }

                  // If the function fails, the return value is zero.
                  NativeError.ThrowException(lastError, path);
               }

               ObjectSecurity objectSecurity = (isFolder) ? (ObjectSecurity)new DirectorySecurity() : new FileSecurity();
               objectSecurity.SetSecurityDescriptorBinaryForm(safeBuffer.ToByteArray(0, safeBuffer.Capacity));

               return (T)(object)objectSecurity;
            }
         }
      }

      #endregion // GetAccessControlInternal

      #region GetAttributesInternal

      /// <summary>[AlphaFS] Gets the <see cref="T:FileAttributes"/> of the specified file or directory.</summary>
      /// <param name="isFolder">The main reason for this parameter is to throw a more appropriate error: DirectoryNotFound vs FileNotFound. <c>true</c> indicates a directory object, DirectoryNotFound will be thrown. <c>false</c> indicates a file object.</param>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to the file or directory.</param>
      /// <param name="fallBack"><c>true</c> fallback on function FindFirstFileXxx() in case function GetFileAttributesXxx() fails.</param>
      /// <param name="continueOnNotExist"><c>true</c> does not throw an Exception when the file system object does not exist.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>The <see cref="T:FileAttributes"/> of the specified file or directory.</returns>
      [SuppressMessage("Microsoft.Interoperability", "CA1404:CallGetLastErrorImmediatelyAfterPInvoke", Justification = "Marshal.GetLastWin32Error() is manipulated.")]
      [SecurityCritical]
      internal static FileAttributes GetAttributesInternal(bool isFolder, KernelTransaction transaction, string path, bool fallBack, bool continueOnNotExist, bool? isFullPath)
      {
         string pathLp = isFullPath == null
            ? path
            : (bool) isFullPath
            ? Path.GetLongPathInternal(path, false, false, false, false)
            : Path.GetFullPathInternal(transaction, path, true, false, false, false);

         // GetFileAttributes()
         // In the ANSI version of this function, the name is limited to MAX_PATH characters.
         // To extend this limit to 32,767 wide characters, call the Unicode version of the function and prepend "\\?\" to the path.
         // 2013-01-13: MSDN confirms LongPath usage.

         FileAttributes attributes = NativeMethods.GetFileAttributes(pathLp);
         int lastError = Marshal.GetLastWin32Error();

         if (lastError == Win32Errors.ERROR_FILE_NOT_FOUND ||
             lastError == Win32Errors.ERROR_PATH_NOT_FOUND ||
             lastError == Win32Errors.ERROR_NOT_READY)
         {
            if (lastError == Win32Errors.ERROR_FILE_NOT_FOUND && isFolder)
               lastError = (int) Win32Errors.ERROR_PATH_NOT_FOUND;

            if (!fallBack && !continueOnNotExist)
               NativeError.ThrowException(lastError, pathLp);
         }


         // Try again using FindFirstFileXxx().
         if (attributes == (FileAttributes) (-1))
         {
            if (fallBack)
            {
               FileSystemEntryInfo fsei = GetFileSystemEntryInfoInternal(isFolder, transaction, pathLp, false, continueOnNotExist, null);
               lastError = Marshal.GetLastWin32Error(); // Might not be the latest Exception.

               if (lastError == Win32Errors.ERROR_FILE_NOT_FOUND && isFolder)
                  lastError = (int)Win32Errors.ERROR_PATH_NOT_FOUND;

               if (fsei != null)
                  attributes = fsei.Win32FindData.FileAttributes;
            }

            if (attributes == (FileAttributes) (-1) && !continueOnNotExist)
               NativeError.ThrowException(lastError, pathLp);
         }

         return attributes;
      }

      #endregion // GetAttributesInternal

      #region GetAttributesExInternal

      /// <summary>[AlphaFS] Gets the <see cref="T:Win32FileAttributeData"/> of the specified file or directory.</summary>
      /// <param name="isFolder">The main reason for this parameter is to throw a more appropriate error: DirectoryNotFound vs FileNotFound. <c>true</c> indicates a directory object, DirectoryNotFound will be thrown. <c>false</c> indicates a file object.</param>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to the file or directory.</param>
      /// <param name="fallBack"><c>true</c> fallback on function FindFirstFileXxx() in case function GetFileAttributesXxx() fails.</param>
      /// <param name="continueOnNotExist"><c>true</c> does not throw an Exception when the file system object does not exist.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>The <see cref="T:FileAttributes"/> of the specified file or directory.</returns>
      [SuppressMessage("Microsoft.Interoperability", "CA1404:CallGetLastErrorImmediatelyAfterPInvoke", Justification = "Marshal.GetLastWin32Error() is manipulated.")]
      [SecurityCritical]
      internal static NativeMethods.Win32FileAttributeData GetAttributesExInternal(bool isFolder, KernelTransaction transaction, string path, bool fallBack, bool continueOnNotExist, bool? isFullPath)
      {
         string pathLp = isFullPath == null
            ? path
            : (bool) isFullPath
            ? Path.GetLongPathInternal(path, false, false, false, false)
            : Path.GetFullPathInternal(transaction, path, true, false, false, false);

         NativeMethods.Win32FileAttributeData win32AttrData;

         if (!(transaction == null || !NativeMethods.IsAtLeastWindowsVista

            // GetFileAttributesEx() / GetFileAttributesTransacted()
            // In the ANSI version of this function, the name is limited to MAX_PATH characters.
            // To extend this limit to 32,767 wide characters, call the Unicode version of the function and prepend "\\?\" to the path.
            // 2013-01-13: MSDN confirms LongPath usage.

            ? NativeMethods.GetFileAttributesEx(pathLp, NativeMethods.GetFileExInfoLevels.GetFileExInfoStandard, out win32AttrData)
            : NativeMethods.GetFileAttributesTransacted(pathLp, NativeMethods.GetFileExInfoLevels.GetFileExInfoStandard, out win32AttrData, transaction.SafeHandle)))
         {
            int lastError = Marshal.GetLastWin32Error();

            if (lastError == Win32Errors.ERROR_FILE_NOT_FOUND ||
                lastError == Win32Errors.ERROR_PATH_NOT_FOUND ||
                lastError == Win32Errors.ERROR_NOT_READY)
            {
               if (lastError == Win32Errors.ERROR_FILE_NOT_FOUND && isFolder)
                  lastError = (int)Win32Errors.ERROR_PATH_NOT_FOUND;

               if (!fallBack && !continueOnNotExist)
                  NativeError.ThrowException(lastError, pathLp);
            }


            // Try again using FindFirstFileXxx().
            if (win32AttrData.FileAttributes == 0)
            {
               if (fallBack)
               {
                  FileSystemEntryInfo fsei = GetFileSystemEntryInfoInternal(isFolder, transaction, pathLp, false, continueOnNotExist, null);
                  lastError = Marshal.GetLastWin32Error(); // Might not be the latest Exception.

                  if (lastError == Win32Errors.ERROR_FILE_NOT_FOUND && isFolder)
                     lastError = (int) Win32Errors.ERROR_PATH_NOT_FOUND;

                  if (fsei != null)
                  {
                     NativeMethods.Win32FindData win32FindData = fsei.Win32FindData;
                     win32AttrData = new NativeMethods.Win32FileAttributeData(win32FindData.FileAttributes,
                        win32FindData.CreationTime, win32FindData.LastAccessTime, win32FindData.LastWriteTime,
                        win32FindData.FileSizeHigh, win32FindData.FileSizeLow);
                  }
               }

               if (win32AttrData.FileAttributes == 0 && !continueOnNotExist)
                  NativeError.ThrowException(lastError, pathLp);
               else
                  return new NativeMethods.Win32FileAttributeData((FileAttributes) (-1));
            }
         }

         return win32AttrData;
      }

      #endregion // GetAttributesExInternal

      #region GetCompressedSizeInternal

      /// <summary>[AlphaFS] Unified method GetCompressedSizeInternal() to retrieve the actual number of bytes of disk storage used to store a specified file as part of a transaction.
      /// If the file is located on a volume that supports compression and the file is compressed, the value obtained is the compressed size of the specified file.
      /// If the file is located on a volume that supports sparse files and the file is a sparse file, the value obtained is the sparse
      /// size of the specified file.
      /// </summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path"><para>The name of the file.</para></param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>The actual number of bytes of disk storage used to store the specified file.</returns>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      internal static long GetCompressedSizeInternal(KernelTransaction transaction, string path, bool? isFullPath)
      {
         if (Utils.IsNullOrWhiteSpace(path))
            throw new ArgumentNullException("path");

         string pathLp = isFullPath == null
            ? path
            : (bool) isFullPath
            ? Path.GetLongPathInternal(path, false, false, false, false)
            : Path.GetFullPathInternal(transaction, path, true, false, false, false);

         uint fileSizeHigh;
         uint fileSizeLow = transaction == null || !NativeMethods.IsAtLeastWindowsVista

            // GetCompressedFileSize() / GetCompressedFileSizeTransacted()
            // In the ANSI version of this function, the name is limited to 248 characters.
            // To extend this limit to 32,767 wide characters, call the Unicode version of the function and prepend "\\?\" to the path.
            // 2013-01-13: MSDN does not confirm LongPath usage but a Unicode version of this function exists.

            ? NativeMethods.GetCompressedFileSize(pathLp, out fileSizeHigh)
            : NativeMethods.GetCompressedFileSizeTransacted(pathLp, out fileSizeHigh, transaction.SafeHandle);

         // If the function fails, and lpFileSizeHigh is NULL, the return value is INVALID_FILE_SIZE.
         if (fileSizeLow == Win32Errors.ERROR_INVALID_FILE_SIZE && fileSizeHigh == 0)
            NativeError.ThrowException(Marshal.GetLastWin32Error(), pathLp);

         return NativeMethods.ToLong(fileSizeHigh, fileSizeLow);
      }

      #endregion // GetCompressedSizeInternal

      #region GetCreationTimeInternal

      /// <summary>[AlphaFS] Gets the creation date and time, in Coordinated Universal Time (UTC) or local time, of the specified file or directory.</summary>
      /// <param name="isFolder">The main reason for this parameter is to throw a more appropriate error: DirectoryNotFound vs FileNotFound. <c>true</c> indicates a directory object, DirectoryNotFound will be thrown. <c>false</c> indicates a file object.</param>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file or directory for which to obtain creation date and time information.</param>
      /// <param name="getUtc"><c>true</c> gets the Coordinated Universal Time (UTC), <c>false</c> gets the local time.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>A <see cref="T:System.DateTime"/> structure set to the creation date and time for the specified file or directory. Depending on <paramref name="getUtc"/> this value is expressed in UTC- or local time.</returns>
      [SecurityCritical]
      internal static DateTime GetCreationTimeInternal(bool isFolder, KernelTransaction transaction, string path, bool getUtc, bool? isFullPath)
      {
         return (getUtc)
            ? DateTime.FromFileTimeUtc(GetAttributesExInternal(isFolder, transaction, path, true, true, isFullPath).CreationTime)
            : DateTime.FromFileTime(GetAttributesExInternal(isFolder, transaction, path, true, true, isFullPath).CreationTime);
      }

      #endregion // GetCreationTimeInternal

      #region GetEncryptionStatusInternal

      /// <summary>[AlphaFS] Unified method GetEncryptionStatusInternal() to retrieve the encryption status of the specified file.</summary>
      /// <param name="path">The name of the file.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>The <see cref="T:FileEncryptionStatus"/> of the specified <paramref name="path"/>.</returns>
      [SecurityCritical]
      internal static FileEncryptionStatus GetEncryptionStatusInternal(string path, bool? isFullPath)
      {
         if (Utils.IsNullOrWhiteSpace(path))
            throw new ArgumentNullException("path");

         string pathLp = isFullPath == null
            ? path
            : (bool)isFullPath
            ? Path.GetLongPathInternal(path, false, false, false, false)
            : Path.GetFullPathInternal(null, path, true, false, false, false);

         FileEncryptionStatus status;

         // FileEncryptionStatus()
         // In the ANSI version of this function, the name is limited to 248 characters.
         // To extend this limit to 32,767 wide characters, call the Unicode version of the function and prepend "\\?\" to the path.
         // 2013-01-13: MSDN does not confirm LongPath usage but a Unicode version of this function exists.

         if (!NativeMethods.FileEncryptionStatus(pathLp, out status))
            NativeError.ThrowException(Marshal.GetLastWin32Error(), pathLp);

         return status;
      }

      #endregion // GetEncryptionStatusInternal

      #region GetFileSystemEntryInfoInternal

      /// <summary>[AlphaFS] Unified method GetFileSystemEntryInfoInternal() to get a FileSystemEntryInfo from a Non-/Transacted directory/file.</summary>
      /// <param name="isFolder">The main reason for this parameter is to throw a more appropriate error: DirectoryNotFound vs FileNotFound. <c>true</c> indicates a directory object, DirectoryNotFound will be thrown. <c>false</c> indicates a file object.</param>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to the file or directory.</param>
      /// <param name="fallback"><c>true</c> enable fallback on function GetFileAttributesXxx() when function FindFirstFileXxx() fails. <c>false</c> disable fallback.</param>
      /// <param name="continueOnException">If <c>null</c>, function GetFileAttributesXxx() will be skipped in case function FindFirstFileXxx() fails. <c>true</c> suppress any Exception that might be thrown a result from a failure, such as ACLs protected directories or non-accessible reparse points.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>The <see cref="T:FileSystemEntryInfo"/> instance of the file or directory, or <c>null</c> on Exception when <paramref name="continueOnException"/> is <c>true</c>.</returns>
      /// <exception cref="NativeError.ThrowException()"></exception>
      [SecurityCritical]
      internal static FileSystemEntryInfo GetFileSystemEntryInfoInternal(bool isFolder, KernelTransaction transaction, string path, bool fallback, bool continueOnException, bool? isFullPath)
      {
         return new FindFileSystemEntryInfo
         {
            IsFullPath = isFullPath,
            IsFolder = isFolder,
            InputPath = path,
            Transaction = transaction,
            Fallback = fallback,
            ContinueOnException = continueOnException,

         }.Get();
      }

      #endregion // GetFileSystemEntryInfoInternal

      #region GetLastAccessTimeInternal

      /// <summary>[AlphaFS] Gets the date and time, in coordinated universal time (UTC) or local time, that the specified file or directory was last accessed.</summary>
      /// <param name="isFolder">The main reason for this parameter is to throw a more appropriate error: DirectoryNotFound vs FileNotFound. <c>true</c> indicates a directory object, DirectoryNotFound will be thrown. <c>false</c> indicates a file object.</param>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file or directory for which to obtain access date and time information.</param>
      /// <param name="getUtc"><c>true</c> gets the Coordinated Universal Time (UTC), <c>false</c> gets the local time.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>A <see cref="T:System.DateTime"/> structure set to the date and time that the specified file or directory was last accessed. Depending on <paramref name="getUtc"/> this value is expressed in UTC- or local time.</returns>
      [SecurityCritical]
      internal static DateTime GetLastAccessTimeInternal(bool isFolder, KernelTransaction transaction, string path, bool getUtc, bool? isFullPath)
      {
         return (getUtc)
            ? DateTime.FromFileTimeUtc(GetAttributesExInternal(isFolder, transaction, path, true, true, isFullPath).LastAccessTime)
            : DateTime.FromFileTime(GetAttributesExInternal(isFolder, transaction, path, true, true, isFullPath).LastAccessTime);
      }

      #endregion // GetLastAccessTimeInternal

      #region GetLastWriteTimeUtcInternal

      /// <summary>[AlphaFS] Gets the date and time, in coordinated universal time (UTC) or local time, that the specified file or directory was last written to.</summary>
      /// <param name="isFolder">The main reason for this parameter is to throw a more appropriate error: DirectoryNotFound vs FileNotFound. <c>true</c> indicates a directory object, DirectoryNotFound will be thrown. <c>false</c> indicates a file object.</param>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file or directory for which to obtain write date and time information.</param>
      /// <param name="getUtc"><c>true</c> gets the Coordinated Universal Time (UTC), <c>false</c> gets the local time.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>A <see cref="T:System.DateTime"/> structure set to the date and time that the specified file or directory was last written to. Depending on <paramref name="getUtc"/> this value is expressed in UTC- or local time.</returns>
      [SecurityCritical]
      internal static DateTime GetLastWriteTimeInternal(bool isFolder, KernelTransaction transaction, string path, bool getUtc, bool? isFullPath)
      {
         return (getUtc)
            ? DateTime.FromFileTimeUtc(GetAttributesExInternal(isFolder, transaction, path, true, true, isFullPath).LastWriteTime)
            : DateTime.FromFileTime(GetAttributesExInternal(isFolder, transaction, path, true, true, isFullPath).LastWriteTime);
      }

      #endregion // GetLastWriteTimeUtcInternal

      #region GetLinkTargetInfoInternal

      /// <summary>[AlphaFS] Unified method GetLinkTargetInfoInternal() to get information about the target of a mount point or symbolic link on an NTFS file system.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to the reparse point.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>An instance of <see cref="T:LinkTargetInfo"/> or <see cref="T:SymbolicLinkTargetInfo"/> containing information about the symbolic link or mount point pointed to by <paramref name="path"/>.
      /// </returns>
      [SecurityCritical]
      internal static LinkTargetInfo GetLinkTargetInfoInternal(KernelTransaction transaction, string path, bool? isFullPath)
      {
         using (SafeFileHandle handle = CreateFileInternal(true, transaction, path, EFileAttributes.OpenReparsePoint | EFileAttributes.BackupSemantics, null, FileMode.Open, 0, FileShare.ReadWrite, isFullPath))
            return Device.GetLinkTargetInfoInternal(handle);
      }

      #endregion // GetLinkTargetInfoInternal

      #region GetSizeInternal

      /// <summary>[AlphaFS] Unified method GetSizeInternal() to retrieve the file size, in bytes to store a specified file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="handle">The <see cref="T:SafeFileHandle"/> to the file.</param>
      /// <param name="path">The path to the file.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>The number of bytes of disk storage used to store the specified file.</returns>
      [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
      [SecurityCritical]
      internal static long GetSizeInternal(KernelTransaction transaction, SafeFileHandle handle, string path, bool? isFullPath)
      {
         bool ownsHandle = handle != null;
         if (!ownsHandle)
         {
            if (Utils.IsNullOrWhiteSpace(path))
               throw new ArgumentNullException("path");

            string pathLp = isFullPath == null
               ? path
               : (bool)isFullPath
               ? Path.GetLongPathInternal(path, false, false, false, false)
               : Path.GetFullPathInternal(transaction, path, true, false, false, false);

            handle = CreateFileInternal(true, transaction, pathLp, EFileAttributes.None, null, FileMode.Open, FileSystemRights.ReadData, FileShare.Read, null);
         }
         else
            NativeMethods.IsValidHandle(handle);

         
         long fileSize;
         
         try
         {
            NativeMethods.GetFileSizeEx(handle, out fileSize);
         }
         finally
         {
            if (!ownsHandle && handle != null)
               handle.Close();
         }

         return fileSize;
      }

      #endregion // GetSizeInternal

      #region OpenInternal

      /// <summary>[AlphaFS] Unified method OpenInternal() to opens a <see cref="T:FileStream"/> on the specified path, having the specified mode with read, write, or read/write access, the specified sharing option and additional options specified.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to open.</param>
      /// <param name="mode">A <see cref="T:FileMode"/> value that specifies whether a file is created if one does not exist, and determines whether the contents of existing files are retained or overwritten.</param>
      /// <param name="rights">A <see cref="T:FileSystemRights"/> value that specifies whether a file is created if one does not exist, and determines whether the contents of existing files are retained or overwritten along with additional options.</param>
      /// <param name="access">A <see cref="T:FileAccess"/> value that specifies the operations that can be performed on the file.</param>
      /// <param name="share">A <see cref="T:FileShare"/> value specifying the type of access other threads have to the file.</param>
      /// <param name="attributes">Advanced <see cref="T:EFileAttributes"/> options for this file.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>An <see cref="T:FileStream"/> instance on the specified path, having the specified mode with read, write, or read/write access and the specified sharing option.</returns>
      [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
      [SecurityCritical]
      internal static FileStream OpenInternal(KernelTransaction transaction, string path, FileMode mode, FileSystemRights rights, FileAccess access, FileShare share, EFileAttributes attributes, bool? isFullPath)
      {
         SafeFileHandle handle = CreateFileInternal(true, transaction, path, attributes, null, mode, rights != 0 ? rights : (FileSystemRights) access, share, isFullPath);

         return rights != 0
            ? new FileStream(handle, FileAccess.Write)
            : new FileStream(handle, access);
      }

      #endregion // OpenInternal

      #region ReadAllBytesInternal

      /// <summary>[AlphaFS] Unified method ReadAllBytesInternal() to open a binary file, reads the contents of the file into a byte array, and then closes the file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to open for reading.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>A byte array containing the contents of the file.</returns>
      [SecurityCritical]
      internal static byte[] ReadAllBytesInternal(KernelTransaction transaction, string path, bool? isFullPath)
      {
         byte[] buffer;

         using (FileStream fs = OpenInternal(transaction, path, FileMode.Open, 0, FileAccess.Read, FileShare.Read, EFileAttributes.None, isFullPath))
         {
            int offset = 0;
            long length = fs.Length;

            if (length > int.MaxValue)
               throw new IOException(string.Format(CultureInfo.CurrentCulture, "File larger than 2GB: [{0}]", path));

            int count = (int)length;
            buffer = new byte[count];
            while (count > 0)
            {
               int n = fs.Read(buffer, offset, count);
               if (n == 0)
                  throw new IOException("Unexpected end of file found");
               offset += n;
               count -= n;
            }
         }
         return buffer;
      }

      #endregion // ReadAllBytesInternal

      #region ReadAllLinesInternal

      /// <summary>[AlphaFS] Unified method ReadAllLinesInternal() to open a file, read all lines of the file with the specified encoding, and then close the file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to open for reading.</param>
      /// <param name="encoding">The <see cref="T:Encoding"/> applied to the contents of the file.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>An IEnumerable string containing all lines of the file.</returns>
      [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
      [SecurityCritical]
      internal static IEnumerable<string> ReadAllLinesInternal(KernelTransaction transaction, string path, Encoding encoding, bool? isFullPath)
      {
         using (StreamReader sr = new StreamReader(OpenInternal(transaction, path, FileMode.Open, 0, FileAccess.Read, FileShare.Read, EFileAttributes.SequentialScan, isFullPath), encoding))
         {
            string line;
            while ((line = sr.ReadLine()) != null)
               yield return line;
         }
      }

      #endregion // ReadAllLinesInternal

      #region ReadAllTextInternal

      /// <summary>[AlphaFS] Unified method ReadAllTextInternal() to open a file, read all lines of the file with the specified encoding, and then close the file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to open for reading.</param>
      /// <param name="encoding">The <see cref="T:Encoding"/> applied to the contents of the file.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>All lines of the file.</returns>
      [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
      [SecurityCritical]
      internal static string ReadAllTextInternal(KernelTransaction transaction, string path, Encoding encoding, bool? isFullPath)
      {
         using (StreamReader sr = new StreamReader(OpenInternal(transaction, path, FileMode.Open, 0, FileAccess.Read, FileShare.Read, EFileAttributes.SequentialScan, isFullPath), encoding))
            return sr.ReadToEnd();
      }

      #endregion // ReadAllTextInternal

      #region ReadLinesInternal

      /// <summary>[AlphaFS] Unified method ReadLinesInternal() to read the lines of a file that has a specified encoding.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to read.</param>
      /// <param name="encoding">The encoding that is applied to the contents of the file.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <returns>All the lines of the file, or the lines that are the result of a query.</returns>
      [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
      [SecurityCritical]
      internal static IEnumerable<string> ReadLinesInternal(KernelTransaction transaction, string path, Encoding encoding, bool? isFullPath)
      {
         using (StreamReader sr = new StreamReader(OpenInternal(transaction, path, FileMode.Open, 0, FileAccess.Read, FileShare.Read, EFileAttributes.SequentialScan, isFullPath), encoding))
         {
            string line;
            while ((line = sr.ReadLine()) != null)
               yield return line;
         }
      }

      #endregion // ReadLinesInternal

      #region ReplaceInternal

      /// <summary>[AlphaFS] Unified method ReplaceInternal() to replace one file with another file, with the option of creating a backup copy of the original file. The replacement file assumes the name of the replaced file and its identity.</summary>
      /// <param name="sourceFileName">The name of a file that replaces the file specified by <paramref name="destFileName"/>.</param>
      /// <param name="destFileName">The name of the file being replaced.</param>
      /// <param name="destinationBackupPath">The name of the backup file.</param>
      /// <param name="ignoreMetadataErrors"><c>true</c> to ignore merge errors (such as attributes and access control lists (ACLs)) from the replaced file to the replacement file; <c>false</c> otherwise.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="sourceFileName"/> and <paramref name="destinationBackupPath"/> will be normalized and long path prefixed. <c>null</c> <paramref name="sourceFileName"/> and <paramref name="destinationBackupPath"/> are already a full path with long path prefix, will be used as is.</param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      internal static void ReplaceInternal(string sourceFileName, string destFileName, string destinationBackupPath, bool ignoreMetadataErrors, bool? isFullPath)
      {
         if (Utils.IsNullOrWhiteSpace(sourceFileName))
            throw new ArgumentNullException("sourceFileName");

         if (Utils.IsNullOrWhiteSpace(destFileName))
            throw new ArgumentNullException("destFileName");

         string sourceFileNameLp = isFullPath == null
            ? sourceFileName
            : (bool) isFullPath
            ? Path.GetLongPathInternal(sourceFileName, false, false, false, false)
            : Path.GetFullPathInternal(null, sourceFileName, true, false, false, true);

         string destFileNameLp = isFullPath == null
            ? destFileName
            : (bool) isFullPath
            ? Path.GetLongPathInternal(destFileName, false, false, false, false)
            : Path.GetFullPathInternal(null, destFileName, true, false, false, true);

         string destinationBackupPathLp = isFullPath == null
            ? destinationBackupPath
            : (bool) isFullPath
            ? Path.GetLongPathInternal(destinationBackupPath, false, false, false, false)
            : Path.GetFullPathInternal(null, destinationBackupPath, true, false, false, true);

         const int replacefileWriteThrough = 1;
         const int replacefileIgnoreMergeErrors = 2;
         FileSystemRights dwReplaceFlags = (FileSystemRights) replacefileWriteThrough;
         if (ignoreMetadataErrors)
            dwReplaceFlags |= (FileSystemRights) replacefileIgnoreMergeErrors;

         // ReplaceFile()
         // In the ANSI version of this function, the name is limited to MAX_PATH characters.
         // To extend this limit to 32,767 wide characters, call the Unicode version of the function and prepend "\\?\" to the path.
         // 2013-01-13: MSDN does not confirm LongPath usage but a Unicode version of this function exists.

         if (!NativeMethods.ReplaceFile(destFileNameLp, sourceFileNameLp, destinationBackupPathLp, dwReplaceFlags, IntPtr.Zero, IntPtr.Zero))
            NativeError.ThrowException(Marshal.GetLastWin32Error(), sourceFileNameLp, destinationBackupPathLp);
      }

      #endregion // ReplaceInternal

      #region SetAccessControlInternal

      /// <summary>[AlphaFS] Unified method SetAccessControlInternal() applies access control list (ACL) entries described by a <see cref="T:FileSecurity"/> FileSecurity object to the specified file.</summary>
      /// <param name="path">A file to add or remove access control list (ACL) entries from. This parameter This parameter may be <c>null</c>.</param>
      /// <param name="handle">A handle to add or remove access control list (ACL) entries from. This parameter This parameter may be <c>null</c>.</param>
      /// <param name="objectSecurity">A <see cref="T:DirectorySecurity"/> or <see cref="T:FileSecurity"/> object that describes an ACL entry to apply to the file described by the <paramref name="path"/> parameter.</param>
      /// <param name="includeSections">One or more of the <see cref="T:AccessControlSections"/> values that specifies the type of access control list (ACL) information to set.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <remarks>Use either <paramref name="path"/> or <paramref name="handle"/>, not both.</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      internal static void SetAccessControlInternal(string path, SafeHandle handle, ObjectSecurity objectSecurity, AccessControlSections includeSections, bool? isFullPath)
      {
         if (objectSecurity == null)
            throw new ArgumentNullException("objectSecurity");

         byte[] managedDescriptor = objectSecurity.GetSecurityDescriptorBinaryForm();
         using (SafeGlobalMemoryBufferHandle hDescriptor = new SafeGlobalMemoryBufferHandle(managedDescriptor.Length))
         {
            hDescriptor.CopyFrom(managedDescriptor, 0, managedDescriptor.Length);

            SecurityDescriptorControl control;
            uint revision;
            if (!Security.NativeMethods.GetSecurityDescriptorControl(hDescriptor, out control, out revision))
               NativeError.ThrowException(Marshal.GetLastWin32Error());

            PrivilegeEnabler privilegeEnabler = null;
            try
            {
               SecurityInformation securityInfo = SecurityInformation.None;

               IntPtr pDacl = IntPtr.Zero;
               if ((includeSections & AccessControlSections.Access) != 0)
               {
                  bool daclDefaulted, daclPresent;
                  if (!Security.NativeMethods.GetSecurityDescriptorDacl(hDescriptor, out daclPresent, out pDacl, out daclDefaulted))
                     NativeError.ThrowException(Marshal.GetLastWin32Error());

                  if (daclPresent)
                  {
                     securityInfo |= SecurityInformation.Dacl;
                     securityInfo |= (control & SecurityDescriptorControl.DaclProtected) != 0
                        ? SecurityInformation.ProtectedDacl
                        : SecurityInformation.UnprotectedDacl;
                  }
               }

               IntPtr pSacl = IntPtr.Zero;
               if ((includeSections & AccessControlSections.Audit) != 0)
               {
                  bool saclDefaulted, saclPresent;
                  if (!Security.NativeMethods.GetSecurityDescriptorSacl(hDescriptor, out saclPresent, out pSacl, out saclDefaulted))
                     NativeError.ThrowException(Marshal.GetLastWin32Error());

                  if (saclPresent)
                  {
                     securityInfo |= SecurityInformation.Sacl;
                     securityInfo |= (control & SecurityDescriptorControl.SaclProtected) != 0
                        ? SecurityInformation.ProtectedSacl
                        : SecurityInformation.UnprotectedSacl;

                     privilegeEnabler = new PrivilegeEnabler(Privilege.Security);
                  }
               }

               IntPtr pOwner = IntPtr.Zero;
               if ((includeSections & AccessControlSections.Owner) != 0)
               {
                  bool ownerDefaulted;
                  if (!Security.NativeMethods.GetSecurityDescriptorOwner(hDescriptor, out pOwner, out ownerDefaulted))
                     NativeError.ThrowException(Marshal.GetLastWin32Error());

                  if (pOwner != IntPtr.Zero)
                     securityInfo |= SecurityInformation.Owner;
               }

               IntPtr pGroup = IntPtr.Zero;
               if ((includeSections & AccessControlSections.Group) != 0)
               {
                  bool groupDefaulted;
                  if (!Security.NativeMethods.GetSecurityDescriptorGroup(hDescriptor, out pGroup, out groupDefaulted))
                     NativeError.ThrowException(Marshal.GetLastWin32Error());

                  if (pGroup != IntPtr.Zero)
                     securityInfo |= SecurityInformation.Group;
               }


               uint lastError;
               if (!Utils.IsNullOrWhiteSpace(path))
               {
                  string pathLp = isFullPath == null
                     ? path
                     : (bool) isFullPath
                        ? Path.GetLongPathInternal(path, false, false, false, false)
                        : Path.GetFullPathInternal(null, path, true, false, false, false);

                  // SetNamedSecurityInfo()
                  // In the ANSI version of this function, the name is limited to MAX_PATH characters.
                  // To extend this limit to 32,767 wide characters, call the Unicode version of the function and prepend "\\?\" to the path.
                  // 2013-01-13: MSDN does not confirm LongPath usage but a Unicode version of this function exists.

                  lastError = Security.NativeMethods.SetNamedSecurityInfo(pathLp, ObjectType.FileObject, securityInfo, pOwner, pGroup, pDacl, pSacl);
                  if (lastError != Win32Errors.ERROR_SUCCESS)
                     NativeError.ThrowException(lastError, pathLp);
               }
               else if (NativeMethods.IsValidHandle(handle))
               {
                  lastError = Security.NativeMethods.SetSecurityInfo(handle, ObjectType.FileObject, securityInfo, pOwner, pGroup, pDacl, pSacl);
                  if (lastError != Win32Errors.ERROR_SUCCESS)
                     NativeError.ThrowException(lastError);
               }
            }
            finally
            {
               if (privilegeEnabler != null)
                  privilegeEnabler.Dispose();
            }
         }
      }

      #endregion // SetAccessControlInternal

      #region SetAttributesInternal

      /// <summary>[AlphaFS] Unified method SetAttributesInternal() to set the attributes for a Non-/Transacted file/directory.</summary>
      /// <param name="isFolder">The main reason for this parameter is to throw a more appropriate error: DirectoryNotFound vs FileNotFound. <c>true</c> indicates a directory object, DirectoryNotFound will be thrown. <c>false</c> indicates a file object.</param>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The name of the file or directory whose attributes are to be set.</param>
      /// <param name="fileAttributes">The attributes to set for the file or directory. Note that all other values override <see cref="T:FileAttributes.Normal"/>.</param>
      /// <param name="continueOnNotExist"><c>true</c> does not throw an Exception when the file system object does not exist.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      internal static void SetAttributesInternal(bool isFolder, KernelTransaction transaction, string path, FileAttributes fileAttributes, bool continueOnNotExist, bool? isFullPath)
      {
         if (Utils.IsNullOrWhiteSpace(path))
            throw new ArgumentNullException("path");

         string pathLp = isFullPath == null
            ? path
            : (bool) isFullPath
               ? Path.GetLongPathInternal(path, false, false, false, false)
               : Path.GetFullPathInternal(transaction, path, true, false, false, false);

         if (!(transaction == null || !NativeMethods.IsAtLeastWindowsVista

            // SetFileAttributes()
            // In the ANSI version of this function, the name is limited to MAX_PATH characters.
            // To extend this limit to 32,767 wide characters, call the Unicode version of the function and prepend "\\?\" to the path.
            // 2013-01-13: MSDN confirms LongPath usage.

            ? NativeMethods.SetFileAttributes(pathLp, fileAttributes)
            : NativeMethods.SetFileAttributesTransacted(pathLp, fileAttributes, transaction.SafeHandle)))
         {
            if (continueOnNotExist)
               return;

            int lastError = Marshal.GetLastWin32Error();
            if (lastError == Win32Errors.ERROR_FILE_NOT_FOUND && isFolder)
               lastError = (int)Win32Errors.ERROR_PATH_NOT_FOUND;

            NativeError.ThrowException(lastError, pathLp);
         }
      }

      #endregion // SetAttributesInternal

      #region SetFsoDateTimeInternal

      /// <summary>[AlphaFS] Unified method SetFsoDateTimeInternal() to set the date and time, in coordinated universal time (UTC), that the file or directory was created and/or last accessed and/or written to.</summary>
      /// <param name="isFolder">The main reason for this parameter is to throw a more appropriate error: DirectoryNotFound vs FileNotFound. <c>true</c> indicates a directory object, DirectoryNotFound will be thrown. <c>false</c> indicates a file object.</param>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file or directory for which to set the date and time information.</param>
      /// <param name="creationTimeUtc">A <see cref="T:System.DateTime"/> containing the value to set for the creation date and time of <paramref name="path"/>. This value is expressed in UTC time.</param>
      /// <param name="lastAccessTimeUtc">A <see cref="T:System.DateTime"/> containing the value to set for the last access date and time of <paramref name="path"/>. This value is expressed in UTC time.</param>
      /// <param name="lastWriteTimeUtc">A <see cref="T:System.DateTime"/> containing the value to set for the last write date and time of <paramref name="path"/>. This value is expressed in UTC time.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      internal static void SetFsoDateTimeInternal(bool isFolder, KernelTransaction transaction, string path, DateTime? creationTimeUtc, DateTime? lastAccessTimeUtc, DateTime? lastWriteTimeUtc, bool? isFullPath)
      {
         using (SafeGlobalMemoryBufferHandle hCreationTime = SafeGlobalMemoryBufferHandle.CreateFromLong(creationTimeUtc.HasValue ? creationTimeUtc.Value.ToFileTimeUtc() : (long?)null))
         using (SafeGlobalMemoryBufferHandle hLastAccessTime = SafeGlobalMemoryBufferHandle.CreateFromLong(lastAccessTimeUtc.HasValue ? lastAccessTimeUtc.Value.ToFileTimeUtc() : (long?)null))
         using (SafeGlobalMemoryBufferHandle hLastWriteTime = SafeGlobalMemoryBufferHandle.CreateFromLong(lastWriteTimeUtc.HasValue ? lastWriteTimeUtc.Value.ToFileTimeUtc() : (long?)null))
         using (SafeFileHandle handle = CreateFileInternal(!isFolder, transaction, path, isFolder ? EFileAttributes.BackupSemantics : EFileAttributes.Normal, null, FileMode.Open, FileSystemRights.WriteAttributes, FileShare.Delete | FileShare.Write, isFullPath))
            if (!NativeMethods.SetFileTime(handle, hCreationTime, hLastAccessTime, hLastWriteTime))
               NativeError.ThrowException(path);
      }

      #endregion // SetFsoDateTimeInternal

      #region TransferTimestampsInternal

      /// <summary>[AlphaFS] Unified method TransferTimestampsInternal() to transfer the date and time stamps for the specified files and directories.</summary>
      /// <param name="isFolder">The main reason for this parameter is to throw a more appropriate error: DirectoryNotFound vs FileNotFound. <c>true</c> indicates a directory object, DirectoryNotFound will be thrown. <c>false</c> indicates a file object.</param>
      /// <param name="transaction">The transaction.</param>
      /// <param name="source">The source path.</param>
      /// <param name="destination">The destination path.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="source"/> and <paramref name="destination"/> will be normalized and long path prefixed. <c>null</c> <paramref name="source"/> and <paramref name="destination"/> are already a full path with long path prefix, will be used as is.</param>
      /// <remarks>This method does not change last access time for the source file.</remarks>
      /// <remarks>This method uses BackupSemantics flag to get Timestamp changed for directories.</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      internal static void TransferTimestampsInternal(bool isFolder, KernelTransaction transaction, string source, string destination, bool? isFullPath)
      {
         FileSystemEntryInfo fsei = GetFileSystemEntryInfoInternal(isFolder, transaction, source, true, false, isFullPath);
         SetFsoDateTimeInternal(isFolder, transaction, destination, DateTime.FromFileTimeUtc(fsei.Win32FindData.CreationTime.ToLong()), DateTime.FromFileTimeUtc(fsei.Win32FindData.LastAccessTime.ToLong()), DateTime.FromFileTimeUtc(fsei.Win32FindData.LastWriteTime.ToLong()), isFullPath);
      }

      #endregion // TransferTimestampsInternal

      #region WriteAllBytesInternal

      /// <summary>[AlphaFS] Unified method WriteAllBytesInternal() to create a new file as part of a transaction, writes the specified byte array to the file, and then closes the file. If the target file already exists, it is overwritten.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to write to.</param>
      /// <param name="bytes">The bytes to write to the file.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "bytes")]
      [SecurityCritical]
      internal static void WriteAllBytesInternal(KernelTransaction transaction, string path, byte[] bytes, bool? isFullPath)
      {
         if (bytes == null)
            throw new ArgumentNullException("bytes");

         using (FileStream fs = OpenInternal(transaction, path, FileMode.Create, 0, FileAccess.Write, FileShare.Read, EFileAttributes.None, isFullPath))
            fs.Write(bytes, 0, bytes.Length);
      }

      #endregion // WriteAllBytesInternal

      #region WriteAppendAllLinesInternal

      /// <summary>[AlphaFS] Unified method WriteAppendAllLinesInternal() to create/append a new file by using the specified encoding, writes a collection of strings to the file, and then closes the file.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to write to.</param>
      /// <param name="contents">The lines to write to the file.</param>
      /// <param name="encoding">The character <see cref="T:Encoding"/> to use.</param>
      /// <param name="isAppend"><c>true</c> for file Append, <c>false</c> for file Write.</param>
      /// <param name="addNewLine"><c>true</c> to a line terminator, <c>false</c> to ommit the line terminator.</param>
      /// <param name="isFullPath"><c>true</c> No path normalization and only long path prefixing is performed. <c>false</c> <paramref name="path"/> will be normalized and long path prefixed. <c>null</c> <paramref name="path"/> is already a full path with long path prefix, will be used as is.</param>
      [SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
      [SecurityCritical]
      internal static void WriteAppendAllLinesInternal(KernelTransaction transaction, string path, IEnumerable<string> contents, Encoding encoding, bool isAppend, bool addNewLine, bool? isFullPath)
      {
         if (Utils.IsNullOrWhiteSpace(path))
            throw new ArgumentNullException("path");

         if (contents == null)
            throw new ArgumentNullException("contents");

         if (encoding == null)
            throw new ArgumentNullException("encoding");


         using (FileStream stream = OpenInternal(transaction, path, (isAppend ? FileMode.OpenOrCreate : FileMode.Create), FileSystemRights.AppendData, FileAccess.Write, FileShare.ReadWrite, EFileAttributes.None, isFullPath))
         {
            if (isAppend)
               stream.Seek(0, SeekOrigin.End);

            using (StreamWriter writer = new StreamWriter(stream, encoding))
            {
               if (addNewLine)
                  foreach (string line in contents)
                     writer.WriteLine(line);

               else
                  foreach (string line in contents)
                     writer.Write(line);
            }
         }
      }

      #endregion // WriteAppendAllLinesInternal

      #endregion // Unified Internals

      #endregion // AlphaFS
   }
}