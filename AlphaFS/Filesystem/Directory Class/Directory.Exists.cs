using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;

namespace Alphaleonis.Win32.Filesystem
{
   public static partial class Directory
   {
      /// <summary>
      ///   Determines whether the given path refers to an existing directory on disk.
      /// </summary>
      /// <remarks>
      ///   <para>MSDN: .NET 3.5+: Trailing spaces are removed from the end of the
      ///   <paramref name="path"/>  parameter before checking whether the directory exists.</para>
      ///   <para>The Exists method returns <see langword="false"/> if any error occurs while trying to determine
      ///   if the specified file exists.</para>
      ///   <para>This can occur in situations that raise exceptions such as passing a file name with
      ///   invalid characters or too many characters,</para>
      ///   <para>a failing or missing disk, or if the caller does not have permission to read the
      ///   file.</para>
      /// </remarks>
      /// <param name="path">The path to test.</param>
      /// <returns>
      ///   <para>Returns <see langword="true"/> if <paramref name="path"/> refers to an existing directory.</para>
      ///   <para>Returns <see langword="false"/> if the directory does not exist or an error occurs when trying
      ///   to determine if the specified file exists.</para>
      /// </returns>
      [SecurityCritical]
      public static bool Exists(string path)
      {
         return File.ExistsInternal(true, null, path, PathFormat.Relative);
      }

      /// <summary>[AlphaFS] Determines whether the given path refers to an existing directory on disk.</summary>
      /// <returns>
      /// <para>Returns <see langword="true"/> if <paramref name="path"/> refers to an existing directory.</para>
      /// <para>Returns <see langword="false"/> if the directory does not exist or an error occurs when trying to determine if the specified file exists.</para>
      /// </returns>
      /// <remarks>
      /// <para>MSDN: .NET 3.5+: Trailing spaces are removed from the end of the <paramref name="path"/> parameter before checking whether the directory exists.</para>
      /// <para>The Exists method returns <see langword="false"/> if any error occurs while trying to determine if the specified file exists.</para>
      /// <para>This can occur in situations that raise exceptions such as passing a file name with invalid characters or too many characters,</para>
      /// <para>a failing or missing disk, or if the caller does not have permission to read the file.</para>
      /// </remarks>
      /// <param name="path">The path to test.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static bool Exists(string path, PathFormat pathFormat)
      {
         return File.ExistsInternal(true, null, path, pathFormat);
      }

      /// <summary>
      ///   [AlphaFS] Determines whether the given path refers to an existing directory on disk.
      /// </summary>
      /// <remarks>
      ///   <para>MSDN: .NET 3.5+: Trailing spaces are removed from the end of the
      ///   <paramref name="path"/> parameter before checking whether the directory exists.</para>
      ///   <para>The Exists method returns <see langword="false"/> if any error occurs while trying to
      ///   determine if the specified file exists.</para>
      ///   <para>This can occur in situations that raise exceptions such as passing a file name with
      ///   invalid characters or too many characters,</para>
      ///   <para>a failing or missing disk, or if the caller does not have permission to read the
      ///   file.</para>
      /// </remarks>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to test.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>
      ///   <para>Returns <see langword="true"/> if <paramref name="path"/> refers to an existing
      ///   directory.</para>
      ///   <para>Returns <see langword="false"/> if the directory does not exist or an error occurs
      ///   when trying to determine if the specified file exists.</para>
      /// </returns>
      [SecurityCritical]
      public static bool Exists(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         return File.ExistsInternal(true, transaction, path, pathFormat);
      }

      /// <summary>[AlphaFS] Determines whether the given path refers to an existing directory on disk.</summary>
      /// <returns>
      /// <para>Returns <see langword="true"/> if <paramref name="path"/> refers to an existing directory.</para>
      /// <para>Returns <see langword="false"/> if the directory does not exist or an error occurs when trying to determine if the specified file exists.</para>
      /// </returns>
      /// <remarks>
      /// <para>MSDN: .NET 3.5+: Trailing spaces are removed from the end of the <paramref name="path"/> parameter before checking whether the directory exists.</para>
      /// <para>The Exists method returns <see langword="false"/> if any error occurs while trying to determine if the specified file exists.</para>
      /// <para>This can occur in situations that raise exceptions such as passing a file name with invalid characters or too many characters,</para>
      /// <para>a failing or missing disk, or if the caller does not have permission to read the file.</para>
      /// </remarks>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to test.</param>
      [SecurityCritical]
      public static bool Exists(KernelTransaction transaction, string path)
      {
         return File.ExistsInternal(true, transaction, path, PathFormat.Relative);
      }
   }
}
