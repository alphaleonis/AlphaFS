using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Security;
using FileStream = System.IO.FileStream;

namespace Alphaleonis.Win32.Filesystem
{
   public static partial class File
   {
      #region WriteAllBytes

      /// <summary>
      ///   Creates a new file, writes the specified byte array to the file, and then closes the file. If the target file already exists, it is
      ///   overwritten.
      /// </summary>
      /// <param name="path">The file to write to.</param>
      /// <param name="bytes">The bytes to write to the file.</param>
      [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "bytes")]
      [SecurityCritical]
      public static void WriteAllBytes(string path, byte[] bytes)
      {
         WriteAllBytesInternal(null, path, bytes, PathFormat.RelativePath);
      }

      /// <summary>
      ///   [AlphaFS] Creates a new file, writes the specified byte array to the file, and then closes the file. If the target file already
      ///   exists, it is overwritten.
      /// </summary>
      /// <param name="path">The file to write to.</param>
      /// <param name="bytes">The bytes to write to the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "bytes")]
      [SecurityCritical]
      public static void WriteAllBytes(string path, byte[] bytes, PathFormat pathFormat)
      {
         WriteAllBytesInternal(null, path, bytes, pathFormat);
      }

      #region Transactional

      /// <summary>
      ///   [AlphaFS] Creates a new file, writes the specified byte array to the file, and then closes the file. If the target file already
      ///   exists, it is overwritten.
      /// </summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to write to.</param>
      /// <param name="bytes">The bytes to write to the file.</param>
      [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "bytes")]
      [SecurityCritical]
      public static void WriteAllBytes(KernelTransaction transaction, string path, byte[] bytes)
      {
         WriteAllBytesInternal(transaction, path, bytes, PathFormat.RelativePath);
      }

      /// <summary>
      ///   [AlphaFS] Creates a new file, writes the specified byte array to the file, and then closes the file. If the target file already
      ///   exists, it is overwritten.
      /// </summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to write to.</param>
      /// <param name="bytes">The bytes to write to the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "bytes")]
      [SecurityCritical]
      public static void WriteAllBytes(KernelTransaction transaction, string path, byte[] bytes, PathFormat pathFormat)
      {
         WriteAllBytesInternal(transaction, path, bytes, pathFormat);
      }

      #endregion // Transacted

      #endregion // WriteAllBytes

      #region Internal Methods

      /// <summary>
      ///   [AlphaFS] Unified method WriteAllBytesInternal() to create a new file as part of a transaction, writes the specified byte array to
      ///   the file, and then closes the file. If the target file already exists, it is overwritten.
      /// </summary>
      /// <exception cref="ArgumentNullException">Thrown when one or more required arguments are null.</exception>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to write to.</param>
      /// <param name="bytes">The bytes to write to the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "bytes")]
      [SecurityCritical]
      internal static void WriteAllBytesInternal(KernelTransaction transaction, string path, byte[] bytes, PathFormat pathFormat)
      {
         if (bytes == null)
            throw new ArgumentNullException("bytes");

         using (FileStream fs = OpenInternal(transaction, path, FileMode.Create, 0, FileAccess.Write, FileShare.Read, ExtendedFileAttributes.None, pathFormat))
            fs.Write(bytes, 0, bytes.Length);
      }

      #endregion // WriteAllBytesInternal
   }
}
