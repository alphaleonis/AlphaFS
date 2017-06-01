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
using System.Security;

namespace Alphaleonis.Win32.Filesystem
{
   public static partial class Path
   {
      #region ChangeExtension (.NET)

      /// <summary>Changes the extension of a path string.</summary>
      /// <returns>The modified path information.</returns>
      /// <exception cref="ArgumentException"/>
      /// <param name="path">The path information to modify. The path cannot contain any of the characters defined in <see cref="GetInvalidPathChars"/>.</param>
      /// <param name="extension">The new extension (with or without a leading period). Specify <see langword="null"/> to remove an existing extension from path.</param>
      [SecurityCritical]
      public static string ChangeExtension(string path, string extension)
      {
         return System.IO.Path.ChangeExtension(path, extension);
      }

      #endregion // ChangeExtension (.NET)

      #region GetDirectoryName

      #region .NET

      /// <summary>Returns the directory information for the specified path string.</summary>
      /// <returns>
      ///   <para>Directory information for <paramref name="path"/>, or <see langword="null"/> if <paramref name="path"/> denotes a root directory or is
      ///   <see langword="null"/>.</para>
      ///   <para>Returns <see cref="string.Empty"/> if <paramref name="path"/> does not contain directory information.</para>
      /// </returns>
      /// <exception cref="ArgumentException"/>
      /// <param name="path">The path of a file or directory.</param>
      [SecurityCritical]
      public static string GetDirectoryName(string path)
      {
         return GetDirectoryName(path, true);
      }

      #endregion // .NET

      #region AlphaFS

      /// <summary>[AlphaFS] Returns the directory information for the specified path string.</summary>
      /// <returns>
      ///   Directory information for <paramref name="path"/>, or <see langword="null"/> if <paramref name="path"/> denotes a root directory or is
      ///   <see langword="null"/>. Returns <see cref="string.Empty"/> if <paramref name="path"/> does not contain directory information.
      /// </returns>
      /// <exception cref="ArgumentException"/>
      /// <param name="path">The path of a file or directory.</param>
      /// <param name="checkInvalidPathChars"><see langword="true"/> will check <paramref name="path"/> for invalid path characters.</param>
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
               
               return path.Substring(0, length).Replace(AltDirectorySeparatorChar, DirectorySeparatorChar);
            }
         }

         return null;
      }

      #endregion // AlphaFS

      #endregion // GetDirectoryName

      #region GetDirectoryNameWithoutRoot

      #region AlphaFS

      /// <summary>[AlphaFS] Returns the directory information for the specified path string without the root information, for example: "C:\Windows\system32" returns: "Windows".</summary>
      /// <returns>The <paramref name="path"/>without the file name part and without the root information (if any), or <see langword="null"/> if <paramref name="path"/> is <see langword="null"/> or if <paramref name="path"/> denotes a root (such as "\", "C:", or * "\\server\share").</returns>
      /// <param name="path">The path.</param>
      [SecurityCritical]
      public static string GetDirectoryNameWithoutRoot(string path)
      {
         return GetDirectoryNameWithoutRootTransacted(null, path);
      }

      /// <summary>[AlphaFS] Returns the directory information for the specified path string without the root information, for example: "C:\Windows\system32" returns: "Windows".</summary>
      /// <returns>The <paramref name="path"/>without the file name part and without the root information (if any), or <see langword="null"/> if <paramref name="path"/> is <see langword="null"/> or if <paramref name="path"/> denotes a root (such as "\", "C:", or * "\\server\share").</returns>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path.</param>
      [SecurityCritical]
      public static string GetDirectoryNameWithoutRootTransacted(KernelTransaction transaction, string path)
      {
         if (path == null)
            return null;

         DirectoryInfo di = Directory.GetParentCore(transaction, path, PathFormat.RelativePath);
         return di != null && di.Parent != null ? di.Name : null;
      }

      #endregion // AlphaFS

      #endregion // GetDirectoryNameWithoutRoot

      #region GetExtension

      #region .NET

      /// <summary>Returns the extension of the specified path string.</summary>
      /// <returns>
      ///   <para>The extension of the specified path (including the period "."), or null, or <see cref="string.Empty"/>.</para>
      ///   <para>If <paramref name="path"/> is null, this method returns null.</para>
      ///   <para>If <paramref name="path"/> does not have extension information,
      ///   this method returns <see cref="string.Empty"/>.</para>
      /// </returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <param name="path">The path string from which to get the extension. The path cannot contain any of the characters defined in <see cref="GetInvalidPathChars"/>.</param>
      [SecurityCritical]
      public static string GetExtension(string path)
      {
         return GetExtension(path, !Utils.IsNullOrWhiteSpace(path));
      }

      #endregion // .NET

      #region AlphaFS

      /// <summary>Returns the extension of the specified path string.</summary>
      /// <returns>
      ///   <para>The extension of the specified path (including the period "."), or null, or <see cref="string.Empty"/>.</para>
      ///   <para>If <paramref name="path"/> is null, this method returns null.</para>
      ///   <para>If <paramref name="path"/> does not have extension information,
      ///   this method returns <see cref="string.Empty"/>.</para>
      /// </returns>
      /// <exception cref="ArgumentException"/>
      /// <param name="path">The path string from which to get the extension. The path cannot contain any of the characters defined in <see cref="GetInvalidPathChars"/>.</param>
      /// <param name="checkInvalidPathChars"><see langword="true"/> will check <paramref name="path"/> for invalid path characters.</param>
      [SecurityCritical]
      public static string GetExtension(string path, bool checkInvalidPathChars)
      {
         if (path == null)
            return null;

         if (checkInvalidPathChars)
            CheckInvalidPathChars(path, false, true);

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

      #endregion // AlphaFS

      #endregion // GetExtension

      #region GetFileName

      #region .NET

      /// <summary>Returns the file name and extension of the specified path string.</summary>
      /// <returns>
      ///   The characters after the last directory character in <paramref name="path"/>. If the last character of <paramref name="path"/> is a
      ///   directory or volume separator character, this method returns <c>string.Empty</c>. If path is null, this method returns null.
      /// </returns>
      /// <exception cref="ArgumentException"/>
      /// <param name="path">The path string from which to obtain the file name and extension. The path cannot contain any of the characters defined in <see cref="GetInvalidPathChars"/>.</param>
      [SecurityCritical]
      public static string GetFileName(string path)
      {
         return GetFileName(path, true);
      }

      #endregion // .NET

      #region AlphaFS

      /// <summary>[AlphaFS] Returns the file name and extension of the specified path string.</summary>
      /// <returns>
      ///   The characters after the last directory character in <paramref name="path"/>. If the last character of <paramref name="path"/> is a
      ///   directory or volume separator character, this method returns <c>string.Empty</c>. If path is null, this method returns null.
      /// </returns>
      /// <exception cref="ArgumentException"/>
      /// <param name="path">The path string from which to obtain the file name and extension.</param>
      /// <param name="checkInvalidPathChars"><see langword="true"/> will check <paramref name="path"/> for invalid path characters.</param>
      [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Utils.IsNullOrWhiteSpace validates arguments.")]
      public static string GetFileName(string path, bool checkInvalidPathChars)
      {
         if (Utils.IsNullOrWhiteSpace(path))
            return path;

         if (checkInvalidPathChars)
            CheckInvalidPathChars(path, false, true);

         int length = path.Length;
         int index = length;
         while (--index >= 0)
         {
            char ch = path[index];
            if (IsDVsc(ch, null))
               return path.Substring(index + 1, length - index - 1);
         }

         return path;
      }

      #endregion // AlphaFS

      #endregion // GetFileName

      #region GetFileNameWithoutExtension

      #region .NET

      /// <summary>Returns the file name of the specified path string without the extension.</summary>
      /// <returns>The string returned by GetFileName, minus the last period (.) and all characters following it.</returns>
      /// <exception cref="ArgumentException"/>
      /// <param name="path">The path of the file. The path cannot contain any of the characters defined in <see cref="GetInvalidPathChars"/>.</param>
      [SecurityCritical]
      public static string GetFileNameWithoutExtension(string path)
      {
         return GetFileNameWithoutExtension(path, true);
      }

      #endregion // .NET

      #region AlphaFS

      /// <summary>[AlphaFS] Returns the file name of the specified path string without the extension.</summary>
      /// <returns>The string returned by GetFileName, minus the last period (.) and all characters following it.</returns>
      /// <exception cref="ArgumentException"/>
      /// <param name="path">The path of the file. The path cannot contain any of the characters defined in <see cref="GetInvalidPathChars"/>.</param>
      /// <param name="checkInvalidPathChars"><see langword="true"/> will check <paramref name="path"/> for invalid path characters.</param>
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

      #endregion // AlphaFS

      #endregion // GetFileNameWithoutExtension

      #region GetInvalidFileNameChars (.NET)

      /// <summary>Gets an array containing the characters that are not allowed in file names.</summary>
      /// <returns>An array containing the characters that are not allowed in file names.</returns>
      [SecurityCritical]
      public static char[] GetInvalidFileNameChars()
      {
         return System.IO.Path.GetInvalidFileNameChars();
      }

      #endregion // GetInvalidFileNameChars (.NET)

      #region GetInvalidPathChars (.NET)

      /// <summary>Gets an array containing the characters that are not allowed in path names.</summary>
      /// <returns>An array containing the characters that are not allowed in path names.</returns>
      [SecurityCritical]
      public static char[] GetInvalidPathChars()
      {
         return System.IO.Path.GetInvalidPathChars();
      }

      #endregion // GetInvalidPathChars (.NET)
      
      #region GetPathRoot

      #region .NET

      /// <summary>Gets the root directory information of the specified path.</summary>
      /// <returns>
      ///   Returns the root directory of <paramref name="path"/>, such as "C:\",
      ///   or <see langword="null"/> if <paramref name="path"/> is <see langword="null"/>,
      ///   or an empty string if <paramref name="path"/> does not contain root directory information.
      /// </returns>
      /// <exception cref="ArgumentException"/>
      /// <param name="path">The path from which to obtain root directory information.</param>
      [SecurityCritical]
      public static string GetPathRoot(string path)
      {
         return GetPathRoot(path, true);
      }

      #endregion // .NET

      /// <summary>[AlphaFS] Gets the root directory information of the specified path.</summary>
      /// <returns>
      ///   Returns the root directory of <paramref name="path"/>, such as "C:\",
      ///   or <see langword="null"/> if <paramref name="path"/> is <see langword="null"/>,
      ///   or an empty string if <paramref name="path"/> does not contain root directory information.
      /// </returns>
      /// <exception cref="ArgumentException"/>
      /// <param name="path">The path from which to obtain root directory information.</param>
      /// <param name="checkInvalidPathChars"><see langword="true"/> will check <paramref name="path"/> for invalid path characters.</param>
      [SecurityCritical]
      public static string GetPathRoot(string path, bool checkInvalidPathChars)
      {
         if (path == null)
            return null;

         if (path.Trim().Length == 0)
            throw new ArgumentException(Resources.Path_Is_Zero_Length_Or_Only_White_Space, "path");

         string pathRp = GetRegularPathCore(path, checkInvalidPathChars ? GetFullPathOptions.CheckInvalidPathChars : GetFullPathOptions.None, false);

         var rootLengthPath = GetRootLength(path, false);
         var rootLengthPathRp = GetRootLength(pathRp, false);

         // Check if pathRp is an empty string.
         if (rootLengthPathRp == 0)
            if (path.StartsWith(LongPathPrefix, StringComparison.OrdinalIgnoreCase))
               return GetLongPathCore(path.Substring(0, rootLengthPath), GetFullPathOptions.None);

         return path.StartsWith(LongPathUncPrefix, StringComparison.OrdinalIgnoreCase)
            ? GetLongPathCore(pathRp.Substring(0, rootLengthPathRp), GetFullPathOptions.None)
            : path.Substring(0, rootLengthPath);
      }

      #endregion // GetPathRoot
   }
}
