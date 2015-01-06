using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using StreamWriter = System.IO.StreamWriter;

namespace Alphaleonis.Win32.Filesystem
{
   public static partial class File
   {
      #region Public Methods

      /// <summary>Creates or opens a file for writing UTF-8 encoded text.</summary>
      /// <param name="path">The file to be opened for writing.</param>
      /// <returns>A StreamWriter that writes to the specified file using UTF-8 encoding.</returns>
      [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
      [SecurityCritical]
      public static StreamWriter CreateText(string path)
      {
         return CreateTextInternal(null, path, NativeMethods.DefaultFileEncoding, PathFormat.Relative);
      }

      /// <summary>[AlphaFS] Creates or opens a file for writing UTF-8 encoded text.</summary>
      /// <param name="path">The file to be opened for writing.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>A StreamWriter that writes to the specified file using UTF-8 encoding.</returns>
      [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
      [SecurityCritical]
      public static StreamWriter CreateText(string path, PathFormat pathFormat)
      {
         return CreateTextInternal(null, path, NativeMethods.DefaultFileEncoding, pathFormat);
      }

      /// <summary>[AlphaFS] Creates or opens a file for writing UTF-8 encoded text.</summary>
      /// <param name="path">The file to be opened for writing.</param>
      /// <param name="encoding">The encoding that is applied to the contents of the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>A StreamWriter that writes to the specified file using UTF-8 encoding.</returns>
      [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
      [SecurityCritical]
      public static StreamWriter CreateText(string path, Encoding encoding, PathFormat pathFormat)
      {
         return CreateTextInternal(null, path, encoding, pathFormat);
      }

      /// <summary>[AlphaFS] Creates or opens a file for writing UTF-8 encoded text.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to be opened for writing.</param>
      /// <returns>A StreamWriter that writes to the specified file using UTF-8 encoding.</returns>
      [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
      [SecurityCritical]
      public static StreamWriter CreateText(KernelTransaction transaction, string path)
      {
         return CreateTextInternal(transaction, path, NativeMethods.DefaultFileEncoding, PathFormat.Relative);
      }

      /// <summary>[AlphaFS] Creates or opens a file for writing UTF-8 encoded text.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to be opened for writing.</param>
      /// <param name="encoding">The encoding that is applied to the contents of the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>A StreamWriter that writes to the specified file using UTF-8 encoding.</returns>
      [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
      [SecurityCritical]
      public static StreamWriter CreateText(KernelTransaction transaction, string path, Encoding encoding, PathFormat pathFormat)
      {
         return CreateTextInternal(transaction, path, encoding, pathFormat);
      }

      #endregion

      #region Internal Methods

      /// <summary>
      ///   [AlphaFS] Unified method CreateTextInternal() to create or open a file for writing <see cref="Encoding"/> encoded text.
      /// </summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to be opened for writing.</param>
      /// <param name="encoding">The <see cref="Encoding"/> applied to the contents of the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>A <see cref="StreamWriter"/> that writes to the specified file using NativeMethods.DefaultFileBufferSize encoding.</returns>
      [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
      [SecurityCritical]
      internal static StreamWriter CreateTextInternal(KernelTransaction transaction, string path, Encoding encoding, PathFormat pathFormat)
      {
         return new StreamWriter(CreateFileStreamInternal(transaction, path, ExtendedFileAttributes.SequentialScan, null, FileMode.Create, FileAccess.Write, FileShare.Read, NativeMethods.DefaultFileBufferSize, pathFormat), encoding);
      }

      #endregion // CreateTextInternal
   }
}
