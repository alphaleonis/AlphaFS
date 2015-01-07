using System;
using System.Linq;
using System.Security;

namespace Alphaleonis.Win32.Filesystem
{
   public static partial class Path
   {
      #region ChangeExtension (.NET)

      /// <summary>Changes the extension of a path string.</summary>
      /// <param name="path">
      ///   The path information to modify. The path cannot contain any of the characters defined in <see cref="GetInvalidPathChars"/>.
      /// </param>
      /// <param name="extension">
      ///   The new extension (with or without a leading period). Specify <see langword="null"/> to remove an existing extension from path.
      /// </param>
      /// <returns>The modified path information.</returns>
      ///
      /// <exception cref="ArgumentException">
      ///   The path parameter contains invalid characters, is empty, or contains only white spaces.
      /// </exception>
      [SecurityCritical]
      public static string ChangeExtension(string path, string extension)
      {
         return System.IO.Path.ChangeExtension(path, extension);
      }

      #endregion // ChangeExtension (.NET)

      #region GetDirectoryName

      /// <summary>
      ///   Returns the directory information for the specified path string.
      /// </summary>
      /// <param name="path">The path of a file or directory.</param>
      /// <returns>
      ///   <para>Directory information for <paramref name="path"/>, or <see langword="null"/> if <paramref name="path"/> denotes a root directory or is
      ///   <see langword="null"/>.</para>
      ///   <para>Returns <see cref="string.Empty"/> if <paramref name="path"/> does not contain directory information.</para>
      /// </returns>
      ///
      /// <exception cref="ArgumentException">
      ///   The path parameter contains invalid characters, is empty, or contains only white spaces.
      /// </exception>
      [SecurityCritical]
      public static string GetDirectoryName(string path)
      {
         return GetDirectoryName(path, true);
      }

      /// <summary>[AlphaFS] Returns the directory information for the specified path string.</summary>
      /// <param name="path">The path of a file or directory.</param>
      /// <param name="checkInvalidPathChars"><see langword="true"/> will check <paramref name="path"/> for invalid path characters.</param>
      /// <returns>
      ///   Directory information for <paramref name="path"/>, or <see langword="null"/> if <paramref name="path"/> denotes a root directory or is
      ///   <see langword="null"/>. Returns <see cref="string.Empty"/> if <paramref name="path"/> does not contain directory information.
      /// </returns>
      ///
      /// <exception cref="ArgumentException">
      ///   The path parameter contains invalid characters, is empty, or contains only white spaces.
      /// </exception>
      [SecurityCritical]
      public static string GetDirectoryName(string path, bool checkInvalidPathChars)
      {
         if (path != null)
         {
            int rootLength = GetRootLength(path, checkInvalidPathChars);
            if (path.Length > rootLength)
            {
               int length = path.Length;
               if (length == rootLength)
                  return null;

               while (length > rootLength && path[--length] != DirectorySeparatorChar && path[length] != AltDirectorySeparatorChar) { }
               return path.Substring(0, length);
            }
         }

         return null;
      }

      #endregion // GetDirectoryName

      #region GetExtension

      /// <summary>Returns the extension of the specified path string.</summary>
      /// <param name="path">
      ///   The path string from which to get the extension. The path cannot contain any of the characters defined in
      ///   <see cref="GetInvalidPathChars"/>.
      /// </param>
      /// <returns>
      ///   <para>The extension of the specified path (including the period "."), or null, or <see cref="string.Empty"/>.</para>
      ///   <para>If <paramref name="path"/> is null, this method returns null.</para>
      ///   <para>If <paramref name="path"/> does not have extension information,
      ///   this method returns <see cref="string.Empty"/>.</para>
      /// </returns>
      ///
      /// <exception cref="ArgumentException">
      ///   The path parameter contains invalid characters, is empty, or contains only white spaces.
      /// </exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      [SecurityCritical]
      public static string GetExtension(string path)
      {
         return GetExtension(path, true);
      }

      /// <summary>
      ///   Returns the extension of the specified path string.
      /// </summary>
      /// <param name="path">
      ///   The path string from which to get the extension. The path cannot contain any of the characters defined in
      ///   <see cref="GetInvalidPathChars"/>.
      /// </param>
      /// <param name="checkInvalidPathChars"><see langword="true"/> will check <paramref name="path"/> for invalid path characters.</param>
      /// <returns>
      ///   <para>The extension of the specified path (including the period "."), or null, or <see cref="string.Empty"/>.</para>
      ///   <para>If <paramref name="path"/> is null, this method returns null.</para>
      ///   <para>If <paramref name="path"/> does not have extension information,
      ///   this method returns <see cref="string.Empty"/>.</para>
      /// </returns>
      ///
      /// <exception cref="ArgumentException">
      ///   The path parameter contains invalid characters, is empty, or contains only white spaces.
      /// </exception>
      [SecurityCritical]
      public static string GetExtension(string path, bool checkInvalidPathChars)
      {
         if (path == null)
            return null;

         if (checkInvalidPathChars)
            CheckInvalidPathChars(path, false);

         int length = path.Length;
         int index = length;
         while (--index >= 0)
         {
            char ch = path[index];
            if (ch == ExtensionSeparatorChar)
               return index != length - 1 ? path.Substring(index, length - index) : string.Empty;

            if (IsDVsc(ch, null))
               break;
         }

         return string.Empty;
      }


      #endregion // GetExtension

      #region GetFileName

      /// <summary>Returns the file name and extension of the specified path string.</summary>
      /// <param name="path">
      ///   The path string from which to obtain the file name and extension. The path cannot contain any of the characters defined in
      ///   <see cref="GetInvalidPathChars"/>.
      /// </param>
      /// <returns>
      ///   The characters after the last directory character in <paramref name="path"/>. If the last character of <paramref name="path"/> is a
      ///   directory or volume separator character, this method returns <c>string.Empty</c>. If path is null, this method returns null.
      /// </returns>
      ///
      /// <exception cref="ArgumentException">
      ///   The path parameter contains invalid characters, is empty, or contains only white spaces.
      /// </exception>
      [SecurityCritical]
      public static string GetFileName(string path)
      {
         return GetFileName(path, true);
      }

      /// <summary>[AlphaFS] Returns the file name and extension of the specified path string.</summary>
      /// <param name="path">The path string from which to obtain the file name and extension.</param>
      /// <param name="checkInvalidPathChars"><see langword="true"/> will check <paramref name="path"/> for invalid path characters.</param>
      /// <returns>
      ///   The characters after the last directory character in <paramref name="path"/>. If the last character of <paramref name="path"/> is a
      ///   directory or volume separator character, this method returns <c>string.Empty</c>. If path is null, this method returns null.
      /// </returns>
      ///
      /// <exception cref="ArgumentException">
      ///   The path parameter contains invalid characters, is empty, or contains only white spaces.
      /// </exception>
      public static string GetFileName(string path, bool checkInvalidPathChars)
      {
         if (path != null)
         {
            if (checkInvalidPathChars)
               CheckInvalidPathChars(path, false);

            int length = path.Length;
            int index = length;
            while (--index >= 0)
            {
               char ch = path[index];
               if (IsDVsc(ch, null))
                  return path.Substring(index + 1, length - index - 1);
            }
         }

         return path;
      }

      #endregion // GetFileName

      #region GetFileNameWithoutExtension

      /// <summary>Returns the file name of the specified path string without the extension.</summary>
      /// <param name="path">
      ///   The path of the file. The path cannot contain any of the characters defined in <see cref="GetInvalidPathChars"/>.
      /// </param>
      /// <returns>The string returned by GetFileName, minus the last period (.) and all characters following it.</returns>
      ///
      /// <exception cref="ArgumentException">
      ///   The path parameter contains invalid characters, is empty, or contains only white spaces.
      /// </exception>
      [SecurityCritical]
      public static string GetFileNameWithoutExtension(string path)
      {
         return GetFileNameWithoutExtension(path, true);
      }

      /// <summary>[AlphaFS] Returns the file name of the specified path string without the extension.</summary>
      /// <param name="path">
      ///   The path of the file. The path cannot contain any of the characters defined in <see cref="GetInvalidPathChars"/>.
      /// </param>
      /// <param name="checkInvalidPathChars"><see langword="true"/> will check <paramref name="path"/> for invalid path characters.</param>
      /// <returns>The string returned by GetFileName, minus the last period (.) and all characters following it.</returns>
      ///
      /// <exception cref="ArgumentException">
      ///   The path parameter contains invalid characters, is empty, or contains only white spaces.
      /// </exception>
      [SecurityCritical]
      public static string GetFileNameWithoutExtension(string path, bool checkInvalidPathChars)
      {
         path = GetFileName(path, checkInvalidPathChars);

         if (path != null)
         {
            int i;
            return (i = path.LastIndexOf('.')) == -1 ? path : path.Substring(0, i);
         }

         return null;
      }

      #endregion // GetFileNameWithoutExtension

      #region GetPathRoot

      /// <summary>Gets the root directory information of the specified path.</summary>
      /// <param name="path">The path from which to obtain root directory information.</param>
      /// <returns>
      ///   <para>Returns the root directory of <paramref name="path"/>, such as "C:\", or <see langword="null"/> if <paramref name="path"/> is
      ///   <see langword="null"/>, </para>
      ///   <para>or an empty string if <paramref name="path"/> does not contain root directory information.</para>
      /// </returns>
      ///
      /// <exception cref="ArgumentException">
      ///   The path parameter contains invalid characters, is empty, or contains only white spaces.
      /// </exception>
      [SecurityCritical]
      public static string GetPathRoot(string path)
      {
         return GetPathRoot(path, true);
      }

      /// <summary>[AlphaFS] Gets the root directory information of the specified path.</summary>
      /// <exception cref="ArgumentException">
      ///   The path parameter contains invalid characters, is empty, or contains only white spaces.
      /// </exception>
      /// <param name="path">The path from which to obtain root directory information.</param>
      /// <param name="checkInvalidPathChars"><see langword="true"/> will check <paramref name="path"/> for invalid path characters.</param>
      /// <returns>
      ///   <para>Returns the root directory of <paramref name="path"/>, such as "C:\", or <see langword="null"/> if <paramref name="path"/> is
      ///   <see langword="null"/>, </para>
      ///   <para>or an empty string if <paramref name="path"/> does not contain root directory information.</para>
      /// </returns>
      [SecurityCritical]
      public static string GetPathRoot(string path, bool checkInvalidPathChars)
      {
         if (path == null)
            return null;

         if (path.Trim().Length == 0)
            throw new ArgumentException(Resources.PathIsZeroLengthOrOnlyWhiteSpace, "path");

         return path.Substring(0, GetRootLength(path, checkInvalidPathChars));
      }

      #endregion // GetPathRoot

      #region GetDirectoryNameWithoutRoot

      /// <summary>
      ///   [AlphaFS] Returns the directory information for the specified path string without the root information, for example: C:\Windows\
      ///   system32 --> Windows.
      /// </summary>
      /// <param name="path">The path.</param>
      /// <returns>
      ///   The <paramref name="path"/>without the file name part and without the root information (if any), or <see langword="null"/> if
      ///   <paramref name="path"/> is <see langword="null"/> or if <paramref name="path"/> denotes a root (such as "\", "C:", or * "\\server\share").
      /// </returns>
      [SecurityCritical]
      public static string GetDirectoryNameWithoutRoot(string path)
      {
         return GetDirectoryNameWithoutRootInternal(null, path);
      }

      /// <summary>
      ///   [AlphaFS] Returns the directory information for the specified path string without the root information, for example: C:\Windows\
      ///   system32 --> Windows.
      /// </summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path.</param>
      /// <returns>
      ///   The <paramref name="path"/>without the file name part and without the root information (if any), or <see langword="null"/> if
      ///   <paramref name="path"/> is <see langword="null"/> or if <paramref name="path"/> denotes a root (such as "\", "C:", or * "\\server\share").
      /// </returns>
      [SecurityCritical]
      public static string GetDirectoryNameWithoutRoot(KernelTransaction transaction, string path)
      {
         return GetDirectoryNameWithoutRootInternal(transaction, path);
      }

      #endregion // GetDirectoryNameWithoutRoot

      #region Internal Methods

      /// <summary>
      ///   [AlphaFS] Unified method GetDirectoryNameWithoutRootInternal() to return the directory information for the specified path string
      ///   without the root information, for example: C:\Windows\system32 --> Windows.
      /// </summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path.</param>
      /// <returns>
      ///   The <paramref name="path"/>without the file name part and without the root information (if any), or <see langword="null"/> if
      ///   <paramref name="path"/> is <see langword="null"/> or if <paramref name="path"/> denotes a root (such as "\", "C:", or * "\\server\
      ///   share").
      /// </returns>
      [SecurityCritical]
      private static string GetDirectoryNameWithoutRootInternal(KernelTransaction transaction, string path)
      {
         if (path == null)
            return null;

         DirectoryInfo di = Directory.GetParentInternal(transaction, path, PathFormat.Relative);
         return di != null && di.Parent != null ? di.Name : null;
      }

      #endregion
   }
}
