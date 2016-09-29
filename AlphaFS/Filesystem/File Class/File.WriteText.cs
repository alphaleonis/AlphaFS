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
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Security;
using System.Security.AccessControl;
using System.Text;

namespace Alphaleonis.Win32.Filesystem
{
   public static partial class File
   {
      #region AppendAllLines

      #region .NET

      /// <summary>Appends lines to a file, and then closes the file. If the specified file does not exist, this method creates a file, writes the
      ///   specified lines to the file, and then closes the file.
      /// </summary>
      /// <remarks>
      ///   The method creates the file if it doesn't exist, but it doesn't create new directories. Therefore, the value of the path parameter
      ///   must contain existing directories.
      /// </remarks>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="ArgumentOutOfRangeException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="SecurityException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="PlatformNotSupportedException"/>
      /// <param name="path">The file to append the lines to. The file is created if it doesn't already exist.</param>
      /// <param name="contents">The lines to append to the file.</param>
      [SecurityCritical]
      public static void AppendAllLines(string path, IEnumerable<string> contents)
      {
         WriteAppendAllLinesCore(null, path, contents, NativeMethods.DefaultFileEncoding, true, true, PathFormat.RelativePath);
      }

      /// <summary>Appends lines to a file, and then closes the file. If the specified file does not exist, this method creates a file, writes the
      ///   specified lines to the file, and then closes the file.
      /// </summary>
      /// <remarks>
      ///   The method creates the file if it doesn't exist, but it doesn't create new directories. Therefore, the value of the path parameter
      ///   must contain existing directories.
      /// </remarks>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="ArgumentOutOfRangeException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="SecurityException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="PlatformNotSupportedException"/>
      /// <param name="path">The file to append the lines to. The file is created if it doesn't already exist.</param>
      /// <param name="contents">The lines to append to the file.</param>
      /// <param name="encoding">The character <see cref="Encoding"/> to use.</param>
      [SecurityCritical]
      public static void AppendAllLines(string path, IEnumerable<string> contents, Encoding encoding)
      {
         WriteAppendAllLinesCore(null, path, contents, encoding, true, false, PathFormat.RelativePath);
      }

      #endregion // .NET

      /// <summary>[AlphaFS] Appends lines to a file, and then closes the file. If the specified file does not exist, this method creates a file,
      ///   writes the specified lines to the file, and then closes the file.
      /// </summary>
      /// <remarks>
      ///   The method creates the file if it doesn't exist, but it doesn't create new directories. Therefore, the value of the path parameter
      ///   must contain existing directories.
      /// </remarks>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="ArgumentOutOfRangeException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="SecurityException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="PlatformNotSupportedException"/>
      /// <param name="path">The file to append the lines to. The file is created if it doesn't already exist.</param>
      /// <param name="contents">The lines to append to the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void AppendAllLines(string path, IEnumerable<string> contents, PathFormat pathFormat)
      {
         WriteAppendAllLinesCore(null, path, contents, NativeMethods.DefaultFileEncoding, true, false, pathFormat);
      }

      /// <summary>[AlphaFS] Appends lines to a file, and then closes the file. If the specified file does not
      ///   exist, this method creates a file, writes the specified lines to the file, and then closes
      ///   the file.
      /// </summary>
      /// <remarks>
      ///   The method creates the file if it doesn't exist, but it doesn't create new directories.
      ///   Therefore, the value of the path parameter must contain existing directories.
      /// </remarks>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="ArgumentOutOfRangeException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="SecurityException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="PlatformNotSupportedException"/>
      /// <param name="path">
      ///   The file to append the lines to. The file is created if it doesn't already exist.
      /// </param>
      /// <param name="contents">The lines to append to the file.</param>
      /// <param name="encoding">The character <see cref="Encoding"/> to use.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void AppendAllLines(string path, IEnumerable<string> contents, Encoding encoding, PathFormat pathFormat)
      {
         WriteAppendAllLinesCore(null, path, contents, encoding, true, false, pathFormat);
      }

      #region Transactional

      #region .NET

      /// <summary>[AlphaFS] Appends lines to a file, and then closes the file. If the specified file does not exist, this method creates a file,
      ///   writes the specified lines to the file, and then closes the file.
      /// </summary>
      /// <remarks>
      ///   The method creates the file if it doesn't exist, but it doesn't create new directories. Therefore, the value of the path parameter
      ///   must contain existing directories.
      /// </remarks>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="ArgumentOutOfRangeException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="SecurityException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="PlatformNotSupportedException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to append the lines to. The file is created if it doesn't already exist.</param>
      /// <param name="contents">The lines to append to the file.</param>
      [SecurityCritical]
      public static void AppendAllLinesTransacted(KernelTransaction transaction, string path, IEnumerable<string> contents)
      {
         WriteAppendAllLinesCore(transaction, path, contents, NativeMethods.DefaultFileEncoding, true, false, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Appends lines to a file, and then closes the file. If the specified file does not exist, this method creates a file,
      ///   writes the specified lines to the file, and then closes the file.
      /// </summary>
      /// <remarks>
      ///   The method creates the file if it doesn't exist, but it doesn't create new directories. Therefore, the value of the path parameter
      ///   must contain existing directories.
      /// </remarks>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="ArgumentOutOfRangeException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="SecurityException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="PlatformNotSupportedException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to append the lines to. The file is created if it doesn't already exist.</param>
      /// <param name="contents">The lines to append to the file.</param>
      /// <param name="encoding">The character <see cref="Encoding"/> to use.</param>
      [SecurityCritical]
      public static void AppendAllLinesTransacted(KernelTransaction transaction, string path, IEnumerable<string> contents, Encoding encoding)
      {
         WriteAppendAllLinesCore(transaction, path, contents, encoding, true, false, PathFormat.RelativePath);
      }

      #endregion // .NET

      /// <summary>[AlphaFS] Appends lines to a file, and then closes the file. If the specified file does not exist, this method creates a file, writes the
      ///   specified lines to the file, and then closes the file.
      /// </summary>
      /// <remarks>
      ///   The method creates the file if it doesn't exist, but it doesn't create new directories. Therefore, the value of the path parameter
      ///   must contain existing directories.
      /// </remarks>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="ArgumentOutOfRangeException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="SecurityException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="PlatformNotSupportedException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to append the lines to. The file is created if it doesn't already exist.</param>
      /// <param name="contents">The lines to append to the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void AppendAllLinesTransacted(KernelTransaction transaction, string path, IEnumerable<string> contents, PathFormat pathFormat)
      {
         WriteAppendAllLinesCore(transaction, path, contents, NativeMethods.DefaultFileEncoding, true, false, pathFormat);
      }

      /// <summary>[AlphaFS] Appends lines to a file, and then closes the file. If the specified file does not exist, this method creates a file,
      ///   writes the specified lines to the file, and then closes the file.
      /// </summary>
      /// <remarks>
      ///   The method creates the file if it doesn't exist, but it doesn't create new directories. Therefore, the value of the path parameter
      ///   must contain existing directories.
      /// </remarks>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="ArgumentOutOfRangeException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="SecurityException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="PlatformNotSupportedException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to append the lines to. The file is created if it doesn't already exist.</param>
      /// <param name="contents">The lines to append to the file.</param>
      /// <param name="encoding">The character <see cref="Encoding"/> to use.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void AppendAllLinesTransacted(KernelTransaction transaction, string path, IEnumerable<string> contents, Encoding encoding, PathFormat pathFormat)
      {
         WriteAppendAllLinesCore(transaction, path, contents, encoding, true, false, pathFormat);
      }

      #endregion // Transactional

      #endregion // AppendAllLines

      #region AppendAllText

      #region .NET

      /// <summary>Appends the specified stringto the file, creating the file if it does not already exist.</summary>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="ArgumentOutOfRangeException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="SecurityException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="PlatformNotSupportedException"/>
      /// <param name="path">The file to append the specified string to.</param>
      /// <param name="contents">The string to append to the file.</param>
      [SecurityCritical]
      public static void AppendAllText(string path, string contents)
      {
         WriteAppendAllLinesCore(null, path, new[] { contents }, NativeMethods.DefaultFileEncoding, true, false, PathFormat.RelativePath);
      }

      /// <summary>Appends the specified string to the file, creating the file if it does not already exist.</summary>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="ArgumentOutOfRangeException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="SecurityException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="PlatformNotSupportedException"/>
      /// <param name="path">The file to append the specified string to.</param>
      /// <param name="contents">The string to append to the file.</param>
      /// <param name="encoding">The character <see cref="Encoding"/> to use.</param>
      [SecurityCritical]
      public static void AppendAllText(string path, string contents, Encoding encoding)
      {
         WriteAppendAllLinesCore(null, path, new[] { contents }, encoding, true, false, PathFormat.RelativePath);
      }

      #endregion // .NET

      /// <summary>[AlphaFS] Appends the specified stringto the file, creating the file if it does not already exist.</summary>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="ArgumentOutOfRangeException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="SecurityException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="PlatformNotSupportedException"/>
      /// <param name="path">The file to append the specified string to.</param>
      /// <param name="contents">The string to append to the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void AppendAllText(string path, string contents, PathFormat pathFormat)
      {
         WriteAppendAllLinesCore(null, path, new[] { contents }, NativeMethods.DefaultFileEncoding, true, false, pathFormat);
      }

      /// <summary>[AlphaFS] Appends the specified string to the file, creating the file if it does not already exist.</summary>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="ArgumentOutOfRangeException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="SecurityException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="PlatformNotSupportedException"/>
      /// <param name="path">The file to append the specified string to.</param>
      /// <param name="contents">The string to append to the file.</param>
      /// <param name="encoding">The character <see cref="Encoding"/> to use.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void AppendAllText(string path, string contents, Encoding encoding, PathFormat pathFormat)
      {
         WriteAppendAllLinesCore(null, path, new[] { contents }, encoding, true, false, pathFormat);
      }

      #region Transactional

      #region .NET

      /// <summary>[AlphaFS] Appends the specified stringto the file, creating the file if it does not already exist.</summary>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="ArgumentOutOfRangeException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="SecurityException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="PlatformNotSupportedException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to append the specified string to.</param>
      /// <param name="contents">The string to append to the file.</param>
      [SecurityCritical]
      public static void AppendAllTextTransacted(KernelTransaction transaction, string path, string contents)
      {
         WriteAppendAllLinesCore(transaction, path, new[] { contents }, NativeMethods.DefaultFileEncoding, true, false, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Appends the specified string to the file, creating the file if it does not already exist.</summary>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="ArgumentOutOfRangeException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="SecurityException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="PlatformNotSupportedException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to append the specified string to.</param>
      /// <param name="contents">The string to append to the file.</param>
      /// <param name="encoding">The character <see cref="Encoding"/> to use.</param>
      [SecurityCritical]
      public static void AppendAllTextTransacted(KernelTransaction transaction, string path, string contents, Encoding encoding)
      {
         WriteAppendAllLinesCore(transaction, path, new[] { contents }, encoding, true, false, PathFormat.RelativePath);
      }

      #endregion // .NET

      /// <summary>[AlphaFS] Appends the specified stringto the file, creating the file if it does not already exist.</summary>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="ArgumentOutOfRangeException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="SecurityException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="PlatformNotSupportedException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to append the specified string to.</param>
      /// <param name="contents">The string to append to the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void AppendAllTextTransacted(KernelTransaction transaction, string path, string contents, PathFormat pathFormat)
      {
         WriteAppendAllLinesCore(transaction, path, new[] { contents }, NativeMethods.DefaultFileEncoding, true, false, pathFormat);
      }

      /// <summary>[AlphaFS] Appends the specified string to the file, creating the file if it does not already exist.</summary>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="ArgumentOutOfRangeException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="SecurityException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="PlatformNotSupportedException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to append the specified string to.</param>
      /// <param name="contents">The string to append to the file.</param>
      /// <param name="encoding">The character <see cref="Encoding"/> to use.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void AppendAllTextTransacted(KernelTransaction transaction, string path, string contents, Encoding encoding, PathFormat pathFormat)
      {
         WriteAppendAllLinesCore(transaction, path, new[] { contents }, encoding, true, false, pathFormat);
      }

      #endregion // Transactional

      #endregion // AppendAllText

      #region WriteAllLines

      #region .NET

      /// <summary>Creates a new file, writes a collection of strings to the file, and then closes the file.</summary>
      /// <remarks>The default behavior of the method is to write out data by using UTF-8 encoding without a byte order mark (BOM).</remarks>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="ArgumentOutOfRangeException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="SecurityException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="PlatformNotSupportedException"/>
      /// <param name="path">The file to write to.</param>
      /// <param name="contents">The lines to write to the file.</param>
      [SecurityCritical]
      public static void WriteAllLines(string path, IEnumerable<string> contents)
      {
         WriteAppendAllLinesCore(null, path, contents, new UTF8Encoding(false, true), false, true, PathFormat.RelativePath);
      }

      /// <summary>Creates a new file by using the specified encoding, writes a collection of strings to the file, and then closes the file.</summary>
      /// <param name="path">The file to write to.</param>
      /// <param name="contents">The string array to write to the file.</param>
      [SecurityCritical]
      public static void WriteAllLines(string path, string[] contents)
      {
         WriteAppendAllLinesCore(null, path, contents, new UTF8Encoding(false, true), false, true, PathFormat.RelativePath);
      }
      
      /// <summary>Creates a new file by using the specified encoding, writes a collection of strings to the file, and then closes the file.</summary>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="ArgumentOutOfRangeException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="SecurityException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="PlatformNotSupportedException"/>
      /// <param name="path">The file to write to.</param>
      /// <param name="contents">The lines to write to the file.</param>
      /// <param name="encoding">The character <see cref="Encoding"/> to use.</param>
      [SecurityCritical]
      public static void WriteAllLines(string path, IEnumerable<string> contents, Encoding encoding)
      {
         WriteAppendAllLinesCore(null, path, contents, encoding, false, true, PathFormat.RelativePath);
      }

      /// <summary>Creates a new file by using the specified encoding, writes a collection of strings to the file, and then closes the file.</summary>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="ArgumentOutOfRangeException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="SecurityException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="PlatformNotSupportedException"/>
      /// <param name="path">The file to write to.</param>
      /// <param name="contents">The string array to write to the file.</param>
      /// <param name="encoding">The character <see cref="Encoding"/> to use.</param>
      [SecurityCritical]
      public static void WriteAllLines(string path, string[] contents, Encoding encoding)
      {
         WriteAppendAllLinesCore(null, path, contents, encoding, false, true, PathFormat.RelativePath);
      }

      #endregion // .NET

      /// <summary>[AlphaFS] Creates a new file, writes a collection of strings to the file, and then closes the file.</summary>
      /// <remarks>The default behavior of the method is to write out data by using UTF-8 encoding without a byte order mark (BOM).</remarks>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="ArgumentOutOfRangeException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="SecurityException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="PlatformNotSupportedException"/>
      /// <param name="path">The file to write to.</param>
      /// <param name="contents">The lines to write to the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void WriteAllLines(string path, IEnumerable<string> contents, PathFormat pathFormat)
      {
         WriteAppendAllLinesCore(null, path, contents, new UTF8Encoding(false, true), false, true, pathFormat);
      }

      /// <summary>[AlphaFS] Creates a new file by using the specified encoding, writes a collection of strings to the file, and then closes the file.</summary>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="ArgumentOutOfRangeException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="SecurityException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="PlatformNotSupportedException"/>
      /// <param name="path">The file to write to.</param>
      /// <param name="contents">The string array to write to the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void WriteAllLines(string path, string[] contents, PathFormat pathFormat)
      {
         WriteAppendAllLinesCore(null, path, contents, new UTF8Encoding(false, true), false, true, pathFormat);
      }

      /// <summary>[AlphaFS] Creates a new file by using the specified encoding, writes a collection of strings to the file, and then closes the file.</summary>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="ArgumentOutOfRangeException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="SecurityException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="PlatformNotSupportedException"/>
      /// <param name="path">The file to write to.</param>
      /// <param name="contents">The lines to write to the file.</param>
      /// <param name="encoding">The character <see cref="Encoding"/> to use.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void WriteAllLines(string path, IEnumerable<string> contents, Encoding encoding, PathFormat pathFormat)
      {
         WriteAppendAllLinesCore(null, path, contents, encoding, false, true, pathFormat);
      }

      /// <summary>[AlphaFS] Creates a new file by using the specified encoding, writes a collection of strings to the file, and then closes the file.</summary>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="ArgumentOutOfRangeException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="SecurityException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="PlatformNotSupportedException"/>
      /// <param name="path">The file to write to.</param>
      /// <param name="contents">The string array to write to the file.</param>
      /// <param name="encoding">The character <see cref="Encoding"/> to use.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void WriteAllLines(string path, string[] contents, Encoding encoding, PathFormat pathFormat)
      {
         WriteAppendAllLinesCore(null, path, contents, encoding, false, true, pathFormat);
      }

      #region Transactional

      #region .NET

      /// <summary>[AlphaFS] Creates a new file, writes a collection of strings to the file, and then closes the file.</summary>
      /// <remarks>The default behavior of the method is to write out data by using UTF-8 encoding without a byte order mark (BOM).</remarks>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="ArgumentOutOfRangeException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="SecurityException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="PlatformNotSupportedException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to write to.</param>
      /// <param name="contents">The lines to write to the file.</param>
      [SecurityCritical]
      public static void WriteAllLinesTransacted(KernelTransaction transaction, string path, IEnumerable<string> contents)
      {
         WriteAppendAllLinesCore(transaction, path, contents, new UTF8Encoding(false, true), false, true, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Creates a new file, writes a collection of strings to the file, and then closes the file.</summary>
      /// <remarks>The default behavior of the method is to write out data by using UTF-8 encoding without a byte order mark (BOM).</remarks>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="ArgumentOutOfRangeException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="SecurityException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="PlatformNotSupportedException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to write to.</param>
      /// <param name="contents">The string array to write to the file.</param>
      [SecurityCritical]
      public static void WriteAllLinesTransacted(KernelTransaction transaction, string path, string[] contents)
      {
         WriteAppendAllLinesCore(transaction, path, contents, new UTF8Encoding(false, true), false, true, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Creates a new file by using the specified encoding, writes a collection of strings to the file, and then closes the file.</summary>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="ArgumentOutOfRangeException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="SecurityException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="PlatformNotSupportedException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to write to.</param>
      /// <param name="contents">The lines to write to the file.</param>
      /// <param name="encoding">The character <see cref="Encoding"/> to use.</param>
      [SecurityCritical]
      public static void WriteAllLinesTransacted(KernelTransaction transaction, string path, IEnumerable<string> contents, Encoding encoding)
      {
         WriteAppendAllLinesCore(transaction, path, contents, encoding, false, true, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Creates a new file by using the specified encoding, writes a collection of strings to the file, and then closes the file.</summary>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="ArgumentOutOfRangeException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="SecurityException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="PlatformNotSupportedException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to write to.</param>
      /// <param name="contents">The string array to write to the file.</param>
      /// <param name="encoding">The character <see cref="Encoding"/> to use.</param>
      [SecurityCritical]
      public static void WriteAllLinesTransacted(KernelTransaction transaction, string path, string[] contents, Encoding encoding)
      {
         WriteAppendAllLinesCore(transaction, path, contents, encoding, false, true, PathFormat.RelativePath);
      }

      #endregion // .NET

      /// <summary>[AlphaFS] Creates a new file, writes a collection of strings to the file, and then closes the file.</summary>
      /// <remarks>The default behavior of the method is to write out data by using UTF-8 encoding without a byte order mark (BOM).</remarks>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="ArgumentOutOfRangeException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="SecurityException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="PlatformNotSupportedException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to write to.</param>
      /// <param name="contents">The lines to write to the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void WriteAllLinesTransacted(KernelTransaction transaction, string path, IEnumerable<string> contents, PathFormat pathFormat)
      {
         WriteAppendAllLinesCore(transaction, path, contents, new UTF8Encoding(false, true), false, true, pathFormat);
      }

      /// <summary>[AlphaFS] Creates a new file by using the specified encoding, writes a collection of strings to the file, and then closes the file.</summary>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="ArgumentOutOfRangeException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="SecurityException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="PlatformNotSupportedException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to write to.</param>
      /// <param name="contents">The string array to write to the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void WriteAllLinesTransacted(KernelTransaction transaction, string path, string[] contents, PathFormat pathFormat)
      {
         WriteAppendAllLinesCore(transaction, path, contents, new UTF8Encoding(false, true), false, true, pathFormat);
      }

      /// <summary>[AlphaFS] Creates a new file by using the specified encoding, writes a collection of strings to the file, and then closes the file.</summary>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="ArgumentOutOfRangeException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="SecurityException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="PlatformNotSupportedException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to write to.</param>
      /// <param name="contents">The lines to write to the file.</param>
      /// <param name="encoding">The character <see cref="Encoding"/> to use.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void WriteAllLinesTransacted(KernelTransaction transaction, string path, IEnumerable<string> contents, Encoding encoding, PathFormat pathFormat)
      {
         WriteAppendAllLinesCore(transaction, path, contents, encoding, false, true, pathFormat);
      }

      /// <summary>[AlphaFS] Creates a new file by using the specified encoding, writes a collection of strings to the file, and then closes the file.</summary>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="ArgumentOutOfRangeException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="SecurityException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="PlatformNotSupportedException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to write to.</param>
      /// <param name="contents">The string array to write to the file.</param>
      /// <param name="encoding">The character <see cref="Encoding"/> to use.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void WriteAllLinesTransacted(KernelTransaction transaction, string path, string[] contents, Encoding encoding, PathFormat pathFormat)
      {
         WriteAppendAllLinesCore(transaction, path, contents, encoding, false, true, pathFormat);
      }

      #endregion // Transactional

      #endregion // WriteAllLines

      #region WriteAllText

      #region .NET

      /// <summary>Creates a new file, writes the specified string to the file, and then closes the file. If the target file already exists, it is overwritten.</summary>
      /// <remarks>This method uses UTF-8 encoding without a Byte-Order Mark (BOM)</remarks>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="ArgumentOutOfRangeException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="SecurityException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="PlatformNotSupportedException"/>
      /// <param name="path">The file to write to.</param>
      /// <param name="contents">The string to write to the file.</param>
      [SecurityCritical]
      public static void WriteAllText(string path, string contents)
      {
         WriteAppendAllLinesCore(null, path, new[] { contents }, new UTF8Encoding(false, true), false, false, PathFormat.RelativePath);
      }

      /// <summary>Creates a new file, writes the specified string to the file using the specified encoding, and then closes the file. If the target file already exists, it is overwritten.</summary>
      /// <param name="path">The file to write to.</param>
      /// <param name="contents">The string to write to the file.</param>
      /// <param name="encoding">The <see cref="Encoding"/> applied to the contents of the file.</param>
      [SecurityCritical]
      public static void WriteAllText(string path, string contents, Encoding encoding)
      {
         WriteAppendAllLinesCore(null, path, new[] { contents }, encoding, false, false, PathFormat.RelativePath);
      }

      #endregion // .NET

      /// <summary>[AlphaFS] Creates a new file, writes the specified string to the file, and then closes the file. If the target file already exists, it is overwritten.</summary>
      /// <remarks>This method uses UTF-8 encoding without a Byte-Order Mark (BOM)</remarks>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="ArgumentOutOfRangeException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="SecurityException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="PlatformNotSupportedException"/>
      /// <param name="path">The file to write to.</param>
      /// <param name="contents">The string to write to the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void WriteAllText(string path, string contents, PathFormat pathFormat)
      {
         WriteAppendAllLinesCore(null, path, new[] { contents }, new UTF8Encoding(false, true), false, false, pathFormat);
      }

      /// <summary>[AlphaFS] Creates a new file, writes the specified string to the file using the specified encoding, and then closes the file. If the target file already exists, it is overwritten.</summary>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="ArgumentOutOfRangeException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="SecurityException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="PlatformNotSupportedException"/>
      /// <param name="path">The file to write to.</param>
      /// <param name="contents">The string to write to the file.</param>
      /// <param name="encoding">The <see cref="Encoding"/> applied to the contents of the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void WriteAllText(string path, string contents, Encoding encoding, PathFormat pathFormat)
      {
         WriteAppendAllLinesCore(null, path, new[] { contents }, encoding, false, false, pathFormat);
      }

      #region Transactional

      #region .NET

      /// <summary>[AlphaFS] Creates a new file as part of a transaction, write the contents to the file, and then closes the file. If the target file already exists, it is overwritten.</summary>
      /// <remarks>This method uses UTF-8 encoding without a Byte-Order Mark (BOM)</remarks>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="ArgumentOutOfRangeException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="SecurityException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="PlatformNotSupportedException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to write to.</param>
      /// <param name="contents">The string to write to the file.</param>
      [SecurityCritical]
      public static void WriteAllTextTransacted(KernelTransaction transaction, string path, string contents)
      {
         WriteAppendAllLinesCore(transaction, path, new[] { contents }, new UTF8Encoding(false, true), false, false, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Creates a new file as part of a transaction, writes the specified string to the file using the specified encoding, and then closes the file. If the target file already exists, it is overwritten.</summary>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="ArgumentOutOfRangeException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="SecurityException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="PlatformNotSupportedException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to write to.</param>
      /// <param name="contents">The string to write to the file.</param>
      /// <param name="encoding">The <see cref="Encoding"/> applied to the contents of the file.</param>
      [SecurityCritical]
      public static void WriteAllTextTransacted(KernelTransaction transaction, string path, string contents, Encoding encoding)
      {
         WriteAppendAllLinesCore(transaction, path, new[] { contents }, encoding, false, false, PathFormat.RelativePath);
      }

      #endregion // .NET

      /// <summary>[AlphaFS] Creates a new file as part of a transaction, write the contents to the file, and then closes the file. If the target file already exists, it is overwritten.</summary>
      /// <remarks>This method uses UTF-8 encoding without a Byte-Order Mark (BOM)</remarks>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="ArgumentOutOfRangeException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="SecurityException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="PlatformNotSupportedException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to write to.</param>
      /// <param name="contents">The string to write to the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void WriteAllTextTransacted(KernelTransaction transaction, string path, string contents, PathFormat pathFormat)
      {
         WriteAppendAllLinesCore(transaction, path, new[] { contents }, new UTF8Encoding(false, true), false, false, pathFormat);
      }

      /// <summary>[AlphaFS] Creates a new file as part of a transaction, writes the specified string to the file using the specified encoding, and then closes the file. If the target file already exists, it is overwritten.</summary>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="ArgumentOutOfRangeException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="SecurityException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="PlatformNotSupportedException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to write to.</param>
      /// <param name="contents">The string to write to the file.</param>
      /// <param name="encoding">The <see cref="Encoding"/> applied to the contents of the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void WriteAllTextTransacted(KernelTransaction transaction, string path, string contents, Encoding encoding, PathFormat pathFormat)
      {
         WriteAppendAllLinesCore(transaction, path, new[] { contents }, encoding, false, false, pathFormat);
      }

      #endregion // Transactional

      #endregion // WriteAllText

      #region Internal Method

      /// <summary>Creates/appends a new file by using the specified encoding, writes a collection of strings to the file, and then closes the file.</summary>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="ArgumentOutOfRangeException"/>
      /// <exception cref="FileNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="SecurityException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="PlatformNotSupportedException"/>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to write to.</param>
      /// <param name="contents">The lines to write to the file.</param>
      /// <param name="encoding">The character <see cref="Encoding"/> to use.</param>
      /// <param name="isAppend"><see langword="true"/> for file Append, <see langword="false"/> for file Write.</param>
      /// <param name="addNewLine"><see langword="true"/> to a line terminator, <see langword="false"/> to ommit the line terminator.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times", Justification = "Disposing is controlled.")]
      [SecurityCritical]
      internal static void WriteAppendAllLinesCore(KernelTransaction transaction, string path, IEnumerable<string> contents, Encoding encoding, bool isAppend, bool addNewLine, PathFormat pathFormat)
      {
         if (contents == null)
            throw new ArgumentNullException("contents");

         if (encoding == null)
            throw new ArgumentNullException("encoding");


         using (FileStream stream = OpenCore(transaction, path, (isAppend ? FileMode.OpenOrCreate : FileMode.Create), FileSystemRights.AppendData, FileShare.ReadWrite, ExtendedFileAttributes.Normal, null, null, pathFormat))
         {
            if (isAppend)
               stream.Seek(0, SeekOrigin.End);

            using (var writer = new StreamWriter(stream, encoding))
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
      
      #endregion // Method
   }
}
