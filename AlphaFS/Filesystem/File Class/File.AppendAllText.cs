using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.AccessControl;
using System.Text;

namespace Alphaleonis.Win32.Filesystem
{
   public static partial class File
   {
      #region AppendAllLines

      /// <summary>
      ///   Appends lines to a file, and then closes the file. If the specified file does not exist, this method creates a file, writes the
      ///   specified lines to the file, and then closes the file.
      /// </summary>
      /// <remarks>
      ///   The method creates the file if it doesn't exist, but it doesn't create new directories. Therefore, the value of the path parameter
      ///   must contain existing directories.
      /// </remarks>
      /// <param name="path">The file to append the lines to. The file is created if it doesn't already exist.</param>
      /// <param name="contents">The lines to append to the file.</param>
      [SecurityCritical]
      public static void AppendAllLines(string path, IEnumerable<string> contents)
      {
         WriteAppendAllLinesInternal(null, path, contents, NativeMethods.DefaultFileEncoding, true, false, PathFormat.Auto);
      }

      /// <summary>
      ///   Appends lines to a file, and then closes the file. If the specified file does not exist, this method creates a file, writes the
      ///   specified lines to the file, and then closes the file.
      /// </summary>
      /// <remarks>
      ///   The method creates the file if it doesn't exist, but it doesn't create new directories. Therefore, the value of the path parameter
      ///   must contain existing directories.
      /// </remarks>
      /// <param name="path">The file to append the lines to. The file is created if it doesn't already exist.</param>
      /// <param name="contents">The lines to append to the file.</param>
      /// <param name="encoding">The character <see cref="Encoding"/> to use.</param>
      [SecurityCritical]
      public static void AppendAllLines(string path, IEnumerable<string> contents, Encoding encoding)
      {
         WriteAppendAllLinesInternal(null, path, contents, encoding, true, false, PathFormat.Auto);
      }

      /// <summary>
      ///   Appends lines to a file, and then closes the file. If the specified file does not exist, this method creates a file,
      ///   writes the specified lines to the file, and then closes the file.
      /// </summary>
      /// <remarks>
      ///   The method creates the file if it doesn't exist, but it doesn't create new directories. Therefore, the value of the path parameter
      ///   must contain existing directories.
      /// </remarks>
      /// <param name="path">The file to append the lines to. The file is created if it doesn't already exist.</param>
      /// <param name="contents">The lines to append to the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void AppendAllLines(string path, IEnumerable<string> contents, PathFormat pathFormat)
      {
         WriteAppendAllLinesInternal(null, path, contents, NativeMethods.DefaultFileEncoding, true, false, pathFormat);
      }

      /// <summary>
      ///   Appends lines to a file, and then closes the file. If the specified file does not
      ///   exist, this method creates a file, writes the specified lines to the file, and then closes
      ///   the file.
      /// </summary>
      /// <remarks>
      ///   The method creates the file if it doesn't exist, but it doesn't create new directories.
      ///   Therefore, the value of the path parameter must contain existing directories.
      /// </remarks>
      /// <param name="path">
      ///   The file to append the lines to. The file is created if it doesn't already exist.
      /// </param>
      /// <param name="contents">The lines to append to the file.</param>
      /// <param name="encoding">The character <see cref="Encoding"/> to use.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void AppendAllLines(string path, IEnumerable<string> contents, Encoding encoding, PathFormat pathFormat)
      {
         WriteAppendAllLinesInternal(null, path, contents, encoding, true, false, pathFormat);
      }


      #region Transacted

      /// <summary>
      ///   Appends lines to a file, and then closes the file. If the specified file does not exist, this method creates a file,
      ///   writes the specified lines to the file, and then closes the file.
      /// </summary>
      /// <remarks>
      ///   The method creates the file if it doesn't exist, but it doesn't create new directories. Therefore, the value of the path parameter
      ///   must contain existing directories.
      /// </remarks>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to append the lines to. The file is created if it doesn't already exist.</param>
      /// <param name="contents">The lines to append to the file.</param>
      [SecurityCritical]
      public static void AppendAllLines(KernelTransaction transaction, string path, IEnumerable<string> contents)
      {
         WriteAppendAllLinesInternal(transaction, path, contents, NativeMethods.DefaultFileEncoding, true, false, PathFormat.Auto);
      }

      /// <summary>
      ///   Appends lines to a file, and then closes the file. If the specified file does not exist, this method creates a file,
      ///   writes the specified lines to the file, and then closes the file.
      /// </summary>
      /// <remarks>
      ///   The method creates the file if it doesn't exist, but it doesn't create new directories. Therefore, the value of the path parameter
      ///   must contain existing directories.
      /// </remarks>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to append the lines to. The file is created if it doesn't already exist.</param>
      /// <param name="contents">The lines to append to the file.</param>
      /// <param name="encoding">The character <see cref="Encoding"/> to use.</param>
      [SecurityCritical]
      public static void AppendAllLines(KernelTransaction transaction, string path, IEnumerable<string> contents, Encoding encoding)
      {
         WriteAppendAllLinesInternal(transaction, path, contents, encoding, true, false, PathFormat.Auto);
      }

      /// <summary>
      ///   Appends lines to a file, and then closes the file. If the specified file does not exist, this method creates a file, writes the
      ///   specified lines to the file, and then closes the file.
      /// </summary>
      /// <remarks>
      ///   The method creates the file if it doesn't exist, but it doesn't create new directories. Therefore, the value of the path parameter
      ///   must contain existing directories.
      /// </remarks>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to append the lines to. The file is created if it doesn't already exist.</param>
      /// <param name="contents">The lines to append to the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void AppendAllLines(KernelTransaction transaction, string path, IEnumerable<string> contents, PathFormat pathFormat)
      {
         WriteAppendAllLinesInternal(transaction, path, contents, NativeMethods.DefaultFileEncoding, true, false, pathFormat);
      }

      /// <summary>
      ///   Appends lines to a file, and then closes the file. If the specified file does not exist, this method creates a file,
      ///   writes the specified lines to the file, and then closes the file.
      /// </summary>
      /// <remarks>
      ///   The method creates the file if it doesn't exist, but it doesn't create new directories. Therefore, the value of the path parameter
      ///   must contain existing directories.
      /// </remarks>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to append the lines to. The file is created if it doesn't already exist.</param>
      /// <param name="contents">The lines to append to the file.</param>
      /// <param name="encoding">The character <see cref="Encoding"/> to use.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void AppendAllLines(KernelTransaction transaction, string path, IEnumerable<string> contents, Encoding encoding, PathFormat pathFormat)
      {
         WriteAppendAllLinesInternal(transaction, path, contents, encoding, true, false, pathFormat);
      }


      #endregion // Transacted

      #endregion // AppendAllLines

      #region AppendAllText

      /// <summary>Appends the specified stringto the file, creating the file if it does not already exist.</summary>
      /// <param name="path">The file to append the specified string to.</param>
      /// <param name="contents">The string to append to the file.</param>
      [SecurityCritical]
      public static void AppendAllText(string path, string contents)
      {
         WriteAppendAllLinesInternal(null, path, new[] { contents }, NativeMethods.DefaultFileEncoding, true, false, PathFormat.Auto);
      }

      /// <summary>Appends the specified string to the file, creating the file if it does not already exist.</summary>
      /// <param name="path">The file to append the specified string to.</param>
      /// <param name="contents">The string to append to the file.</param>
      /// <param name="encoding">The character <see cref="Encoding"/> to use.</param>
      [SecurityCritical]
      public static void AppendAllText(string path, string contents, Encoding encoding)
      {
         WriteAppendAllLinesInternal(null, path, new[] { contents }, encoding, true, false, PathFormat.Auto);
      }

      /// <summary>Appends the specified stringto the file, creating the file if it does not already exist.</summary>
      /// <param name="path">The file to append the specified string to.</param>
      /// <param name="contents">The string to append to the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void AppendAllText(string path, string contents, PathFormat pathFormat)
      {
         WriteAppendAllLinesInternal(null, path, new[] { contents }, NativeMethods.DefaultFileEncoding, true, false, pathFormat);
      }

      /// <summary>Appends the specified string to the file, creating the file if it does not already exist.</summary>
      /// <param name="path">The file to append the specified string to.</param>
      /// <param name="contents">The string to append to the file.</param>
      /// <param name="encoding">The character <see cref="Encoding"/> to use.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void AppendAllText(string path, string contents, Encoding encoding, PathFormat pathFormat)
      {
         WriteAppendAllLinesInternal(null, path, new[] { contents }, encoding, true, false, pathFormat);
      }


      #region Transacted

      /// <summary>Appends the specified stringto the file, creating the file if it does not already exist.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to append the specified string to.</param>
      /// <param name="contents">The string to append to the file.</param>
      [SecurityCritical]
      public static void AppendAllText(KernelTransaction transaction, string path, string contents)
      {
         WriteAppendAllLinesInternal(transaction, path, new[] { contents }, NativeMethods.DefaultFileEncoding, true, false, PathFormat.Auto);
      }

      /// <summary>Appends the specified string to the file, creating the file if it does not already exist.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to append the specified string to.</param>
      /// <param name="contents">The string to append to the file.</param>
      /// <param name="encoding">The character <see cref="Encoding"/> to use.</param>
      [SecurityCritical]
      public static void AppendAllText(KernelTransaction transaction, string path, string contents, Encoding encoding)
      {
         WriteAppendAllLinesInternal(transaction, path, new[] { contents }, encoding, true, false, PathFormat.Auto);
      }

      /// <summary>Appends the specified stringto the file, creating the file if it does not already exist.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to append the specified string to.</param>
      /// <param name="contents">The string to append to the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void AppendAllText(KernelTransaction transaction, string path, string contents, PathFormat pathFormat)
      {
         WriteAppendAllLinesInternal(transaction, path, new[] { contents }, NativeMethods.DefaultFileEncoding, true, false, pathFormat);
      }

      /// <summary>Appends the specified string to the file, creating the file if it does not already exist.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to append the specified string to.</param>
      /// <param name="contents">The string to append to the file.</param>
      /// <param name="encoding">The character <see cref="Encoding"/> to use.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void AppendAllText(KernelTransaction transaction, string path, string contents, Encoding encoding, PathFormat pathFormat)
      {
         WriteAppendAllLinesInternal(transaction, path, new[] { contents }, encoding, true, false, pathFormat);
      }


      #endregion // Transacted


      #endregion // AppendAllText

      #region Internal

      /// <summary>
      ///   Unified method WriteAppendAllLinesInternal() to create/append a new file by using the specified encoding, writes a
      ///   collection of strings to the file, and then closes the file.
      /// </summary>
      /// <exception cref="ArgumentNullException">Thrown when one or more required arguments are null.</exception>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to write to.</param>
      /// <param name="contents">The lines to write to the file.</param>
      /// <param name="encoding">The character <see cref="Encoding"/> to use.</param>
      /// <param name="isAppend"><see langword="true"/> for file Append, <see langword="false"/> for file Write.</param>
      /// <param name="addNewLine"><see langword="true"/> to a line terminator, <see langword="false"/> to ommit the line terminator.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
      [SecurityCritical]
      internal static void WriteAppendAllLinesInternal(KernelTransaction transaction, string path, IEnumerable<string> contents, Encoding encoding, bool isAppend, bool addNewLine, PathFormat pathFormat)
      {
         if (contents == null)
            throw new ArgumentNullException("contents");

         if (encoding == null)
            throw new ArgumentNullException("encoding");


         using (FileStream stream = OpenInternal(transaction, path, (isAppend ? FileMode.OpenOrCreate : FileMode.Create), FileSystemRights.AppendData, FileAccess.Write, FileShare.ReadWrite, ExtendedFileAttributes.None, pathFormat))
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
      #endregion
   }
}
