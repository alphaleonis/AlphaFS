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

using Alphaleonis.Win32.Network;
using Alphaleonis.Win32.Security;
using Microsoft.Win32.SafeHandles;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace Alphaleonis.Win32.Filesystem
{
   /// <summary>Performs operations on String instances that contain file or directory path information. These operations are performed in a cross-platform manner.</summary>
   public static class Path
   {
      #region Methods

      #region .NET

      #region ChangeExtension (.NET)

      #region .NET

      /// <summary>Changes the extension of a path string.</summary>
      /// <param name="path">The path information to modify. The path cannot contain any of the characters defined in <see cref="T:GetInvalidPathChars"/>.</param>
      /// <param name="extension">The new extension (with or without a leading period). Specify <c>null</c> to remove an existing extension from path.</param>
      /// <returns>The modified path information.</returns>
      /// <exception cref="ArgumentException">Path contains one or more of the invalid characters defined in GetInvalidPathChars.</exception>
      [SecurityCritical]
      public static string ChangeExtension(string path, string extension)
      {
         return System.IO.Path.ChangeExtension(path, extension);
      }

      #endregion // .NET

      #endregion // ChangeExtension (.NET)

      #region Combine

      #region .NET

      /// <summary>Combines an array of strings into a path.</summary>
      /// <param name="paths">An array of parts of the path.</param>
      /// <returns>The combined paths.</returns>
      /// <exception cref="ArgumentException">One of the strings in the array contains one or more of the invalid characters defined in <see cref="T:GetInvalidPathChars"/>.</exception>
      /// <exception cref="ArgumentNullException">One of the strings in the array is <c>null</c>.</exception>
      [SecurityCritical]
      public static string Combine(params string[] paths)
      {
         return CombineInternal(true, paths);
      }

      #endregion // .NET

      #endregion // Combine

      #region GetDirectoryName

      #region .NET

      /// <summary>Returns the directory information for the specified path string.</summary>
      /// <param name="path">The path of a file or directory.</param>
      /// <returns>Directory information for <paramref name="path"/>, or <c>null</c> if <paramref name="path"/> denotes a root directory or is <c>null</c>. Returns <see cref="T:string.Empty"/> if <paramref name="path"/> does not contain directory information.</returns>
      [SecurityCritical]
      public static string GetDirectoryName(string path)
      {
         if (path != null)
         {
            int rootLength = GetRootLength(path, true);
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

      #endregion // .NET

      #endregion // GetDirectoryName

      #region GetExtension (.NET)

      #region .NET

      /// <summary>Returns the extension of the specified path string.</summary>
      /// <param name="path">The path string from which to get the extension. The path cannot contain any of the characters defined in <see cref="T:GetInvalidPathChars"/>.</param>
      /// <returns>The extension of the specified path (including the period "."), or null, or <see cref="F:System.String.Empty"/>. If <paramref name="path"/> is null, <see cref="M:System.IO.Path.GetExtension(System.String)"/> returns null. If <paramref name="path"/> does not have extension information, <see cref="M:System.IO.Path.GetExtension(System.String)"/> returns <see cref="F:System.String.Empty"/>.</returns>
      [SecurityCritical]
      public static string GetExtension(string path)
      {
         return System.IO.Path.GetExtension(path);
      }

      #endregion // .NET

      #endregion // GetExtension (.NET)

      #region GetFileName (.NET)

      #region .NET

      /// <summary>Returns the file name and extension of the specified path string.</summary>
      /// <param name="path">The path string from which to obtain the file name and extension. The path cannot contain any of the characters defined in <see cref="T:GetInvalidPathChars"/>.</param>
      /// <returns>The characters after the last directory character in <paramref name="path"/>. If the last character of <paramref name="path"/> is a directory or volume separator character, this method returns <c>string.Empty</c>. If path is null, this method returns null.</returns>
      [SecurityCritical]
      public static string GetFileName(string path)
      {
         return System.IO.Path.GetFileName(path);
      }

      #endregion // .NET

      #region AlphaFS

      #region IsFullPath

      /// <summary>Returns the file name and extension of the specified path string.</summary>
      /// <param name="path">The path string from which to obtain the file name and extension.</param>
      /// <param name="checkInvalidChars"><c>true</c> will not check <paramref name="path"/> for invalid path characters.</param>
      /// <returns>The characters after the last directory character in <paramref name="path"/>. If the last character of <paramref name="path"/> is a directory or volume separator character, this method returns <c>string.Empty</c>. If path is null, this method returns null.</returns>
      public static string GetFileName(string path, bool checkInvalidChars)
      {
         if (path != null)
         {
            if (checkInvalidChars)
               CheckInvalidPathChars(path);

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

      #endregion // IsFullPath

      #endregion // AlphaFS

      #endregion // GetFileName (.NET)

      #region GetFileNameWithoutExtension (.NET)

      #region .NET

      /// <summary>Returns the file name of the specified path string without the extension.</summary>
      /// <param name="path">The path of the file. The path cannot contain any of the characters defined in <see cref="T:GetInvalidPathChars"/>.</param>
      /// <returns>The string returned by GetFileName, minus the last period (.) and all characters following it.</returns>
      [SecurityCritical]
      public static string GetFileNameWithoutExtension(string path)
      {
         return System.IO.Path.GetFileNameWithoutExtension(path);
      }

      #endregion // .NET

      #endregion // GetFileNameWithoutExtension (.NET)

      #region GetFullPath

      #region .NET

      /// <summary>Returns the absolute path for the specified path string.</summary>
      /// <param name="path">The file or directory for which to obtain absolute path information.</param>
      /// <returns>The fully qualified location of path, such as "C:\MyFile.txt".</returns>
      /// <remarks>The .NET Framework does not support direct access to physical disks through paths that are device names, such as "\\.\PHYSICALDRIVE0"</remarks>
      /// <remarks>GetFullPath is not recommended for multithreaded applications or shared library code.</remarks>
      [SecurityCritical]
      public static string GetFullPath(string path)
      {
         return GetFullPathInternal(null, path, false, false, false, false, false);
      }

      #endregion // .NET

      #region AlphaFS

      /// <summary>[AlphaFS] Returns the absolute path for the specified path string.</summary>
      /// <param name="path">The file or directory for which to obtain absolute path information.</param>
      /// <param name="asLongPath"><c>true</c> returns the path in long path (Unicode) format, when <c>false</c> returns the path as a regular path.</param>
      /// <returns>The fully qualified location of path, such as "C:\MyFile.txt".</returns>
      /// <remarks>The .NET Framework does not support direct access to physical disks through paths that are device names, such as "\\.\PHYSICALDRIVE0"</remarks>
      /// <remarks>GetFullPath is not recommended for multithreaded applications or shared library code.</remarks>
      [SecurityCritical]
      public static string GetFullPath(string path, bool asLongPath)
      {
         return GetFullPathInternal(null, path, asLongPath, false, false, false, false);
      }

      /// <summary>[AlphaFS] Returns the absolute path for the specified path string.</summary>
      /// <param name="path">The file or directory for which to obtain absolute path information.</param>
      /// <param name="asLongPath"><c>true</c> returns the path in long path (Unicode) format, when <c>false</c> returns the path as a regular path.</param>
      /// <param name="addDirectorySeparator"><c>true</c> adds a directory separator to that path.</param>
      /// <param name="removeDirectorySeparator"><c>true</c> removes any directory separator to that path.</param>
      /// <returns>The fully qualified location of path, such as "C:\MyFile.txt".</returns>
      /// <remarks>The .NET Framework does not support direct access to physical disks through paths that are device names, such as "\\.\PHYSICALDRIVE0"</remarks>
      /// <remarks>GetFullPath is not recommended for multithreaded applications or shared library code.</remarks>
      [SecurityCritical]
      public static string GetFullPath(string path, bool asLongPath, bool addDirectorySeparator, bool removeDirectorySeparator)
      {
         return GetFullPathInternal(null, path, asLongPath, false, addDirectorySeparator, removeDirectorySeparator, false);
      }
      
      #region Transacted

      /// <summary>[AlphaFS] Returns the absolute path for the specified path string.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file or directory for which to obtain absolute path information.</param>
      /// <returns>The fully qualified location of path, such as "C:\MyFile.txt".</returns>
      /// <remarks>The .NET Framework does not support direct access to physical disks through paths that are device names, such as "\\.\PHYSICALDRIVE0"</remarks>
      /// <remarks>GetFullPath is not recommended for multithreaded applications or shared library code.</remarks>
      [SecurityCritical]
      public static string GetFullPath(KernelTransaction transaction, string path)
      {
         return GetFullPathInternal(transaction, path, false, false, false, false, false);
      }

      /// <summary>[AlphaFS] Returns the absolute path for the specified path string.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file or directory for which to obtain absolute path information.</param>
      /// <param name="asLongPath"><c>true</c> returns the path in long path (Unicode) format, when <c>false</c> returns the path as a regular path.</param>
      /// <returns>The fully qualified location of path, such as "C:\MyFile.txt".</returns>
      /// <remarks>The .NET Framework does not support direct access to physical disks through paths that are device names, such as "\\.\PHYSICALDRIVE0"</remarks>
      /// <remarks>GetFullPath is not recommended for multithreaded applications or shared library code.</remarks>
      [SecurityCritical]
      public static string GetFullPath(KernelTransaction transaction, string path, bool asLongPath)
      {
         return GetFullPathInternal(transaction, path, asLongPath, false, false, false, false);
      }

      /// <summary>[AlphaFS] Returns the absolute path for the specified path string.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file or directory for which to obtain absolute path information.</param>
      /// <param name="asLongPath"><c>true</c> returns the path in long path (Unicode) format, when <c>false</c> returns the path as a regular path.</param>
      /// <param name="addDirectorySeparator"><c>true</c> adds a directory separator to that path.</param>
      /// <param name="removeDirectorySeparator"><c>true</c> removes any directory separator to that path.</param>
      /// <returns>The fully qualified location of path, such as "C:\MyFile.txt".</returns>
      /// <remarks>The .NET Framework does not support direct access to physical disks through paths that are device names, such as "\\.\PHYSICALDRIVE0"</remarks>
      /// <remarks>GetFullPath is not recommended for multithreaded applications or shared library code.</remarks>
      [SecurityCritical]
      public static string GetFullPath(KernelTransaction transaction, string path, bool asLongPath, bool addDirectorySeparator, bool removeDirectorySeparator)
      {
         return GetFullPathInternal(transaction, path, asLongPath, false, addDirectorySeparator, removeDirectorySeparator, false);
      }

      #endregion Transacted

      #endregion // AlphaFS

      #endregion // GetFullPath

      #region GetInvalidFileNameChars (.NET)

      #region .NET

      /// <summary>Gets an array containing the characters that are not allowed in file names.</summary>
      /// <returns>An array containing the characters that are not allowed in file names.</returns>
      [SecurityCritical]
      public static char[] GetInvalidFileNameChars()
      {
         return System.IO.Path.GetInvalidFileNameChars();
      }

      #endregion // .NET

      #endregion // GetInvalidFileNameChars (.NET)

      #region GetInvalidPathChars (.NET)

      #region .NET

      /// <summary>Gets an array containing the characters that are not allowed in path names.</summary>
      /// <returns>An array containing the characters that are not allowed in path names.</returns>
      [SecurityCritical]
      public static char[] GetInvalidPathChars()
      {
         return System.IO.Path.GetInvalidPathChars();
      }

      #endregion // .NET

      #endregion // GetInvalidPathChars (.NET)

      #region GetPathRoot

      #region .NET

      /// <summary>Gets the root directory information of the specified path.</summary>
      /// <param name="path">The path from which to obtain root directory information.</param>
      /// <returns>The root directory of <paramref name="path"/>, such as "C:\", or <c>null</c> if <paramref name="path"/> is <c>null</c>, or <c>string.Empty</c> if <paramref name="path"/> does not contain root directory information.</returns>
      [SecurityCritical]
      public static string GetPathRoot(string path)
      {
         return GetPathRoot(path, false);
      }

      #endregion // .NET

      #region AlphaFS

      /// <summary>[AlphaFS] Gets the root directory information of the specified path.</summary>
      /// <param name="path">The path from which to obtain root directory information.</param>
      /// <param name="checkInvalidChars"><c>true</c> will not check <paramref name="path"/> for invalid path characters.</param>
      /// <returns>The root directory of <paramref name="path"/>, such as "C:\", or <c>null</c> if <paramref name="path"/> is <c>null</c>, or <c>string.Empty</c> if <paramref name="path"/> does not contain root directory information.</returns>
      [SecurityCritical]
      public static string GetPathRoot(string path, bool checkInvalidChars)
      {
         if (path == null)
            return null;

         return path.Substring(0, GetRootLength(path, checkInvalidChars));
      }

      #endregion // AlphaFS

      #endregion // GetPathRoot

      #region GetRandomFileName (.NET)

      #region .NET

      /// <summary>Returns a random folder name or file name.</summary>
      /// <returns>A random folder name or file name.</returns>
      [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
      [SecurityCritical]
      public static string GetRandomFileName()
      {
         return System.IO.Path.GetRandomFileName();
      }

      #endregion // .NET

      #endregion // GetRandomFileName (.NET)

      #region GetTempFileName (.NET)

      #region .NET

      /// <summary>Creates a uniquely named, zero-byte temporary file on disk and returns the full path of that file.</summary>
      /// <returns>The full path of the temporary file.</returns>
      [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
      [SecurityCritical]
      public static string GetTempFileName()
      {
         return System.IO.Path.GetTempFileName();
      }

      #endregion // .NET

      #endregion // GetTempFileName (.NET)

      #region GetTempPath (.NET)

      #region .NET

      /// <summary>Returns the path of the current user's temporary folder.</summary>
      /// <returns>The path to the temporary folder, ending with a backslash.</returns>
      [SecurityCritical]
      public static string GetTempPath()
      {
         return System.IO.Path.GetTempPath();
      }

      #endregion // .NET

      #region AlphaFS

      /// <summary>[AlphaFS] Returns the path of the current user's temporary folder.</summary>
      /// <param name="combinePath">The folder name to append to the temporary folder.</param>
      /// <returns>The path to the temporary folder, combined with <paramref name="combinePath"/>.</returns>
      [SecurityCritical]
      public static string GetTempPath(string combinePath)
      {
         string tempPath = GetTempPath();
         return !Utils.IsNullOrWhiteSpace(combinePath) ? CombineInternal(false, tempPath, combinePath) : tempPath;
      }

      #endregion // AlphaFS

      #endregion // GetTempPath (.NET)

      #region HasExtension (.NET)

      #region .NET

      /// <summary>Determines whether a path includes a file name extension.</summary>
      /// <param name="path">The path to search for an extension. The path cannot contain any of the characters defined in <see cref="T:GetInvalidPathChars"/>.</param>
      /// <returns><c>true</c> if the characters that follow the last directory separator (\\ or /) or volume separator (:) in the path include a period (.) followed by one or more characters; otherwise, <c>false</c>.</returns>
      [SecurityCritical]
      public static bool HasExtension(string path)
      {
         return System.IO.Path.HasExtension(path);
      }

      #endregion // .NET

      #endregion // HasExtension (.NET)

      #region IsPathRooted (.NET)

      #region .NET

      /// <summary>Gets a value indicating whether the specified path string contains absolute or relative path information.</summary>
      /// <param name="path">The path to test. The path cannot contain any of the characters defined in <see cref="T:GetInvalidPathChars"/>.</param>
      /// <returns><c>true</c> if <paramref name="path"/> contains a root; otherwise, <c>false</c>.</returns>
      /// <remarks>The IsPathRooted method returns true if the first character is a directory separator character such as <see cref="T:DirectorySeparatorChar"/>, or if the path starts with a drive letter and colon (<see cref="T:VolumeSeparatorChar"/>). For example, it returns true for path strings such as "\\MyDir\\MyFile.txt", "C:\\MyDir", or "C:MyDir". It returns <c>false</c> for path strings such as "MyDir".</remarks>
      /// <remarks>This method does not verify that the path or file name exists.</remarks>
      [SecurityCritical]
      public static bool IsPathRooted(string path)
      {
         return System.IO.Path.IsPathRooted(path);
      }

      #endregion // .NET

      #endregion // IsPathRooted (.NET)

      #endregion // .NET

      #region AlphaFS

      #region AddDirectorySeparator

      /// <summary>[AlphaFS] Adds a <see cref="T:DirectorySeparatorChar"/> or <see cref="T:AltDirectorySeparatorChar"/> character to the string.</summary>
      /// <param name="path">A text string to which the <see cref="T:DirectorySeparatorChar"/> or <see cref="T:AltDirectorySeparatorChar"/> is to be added.</param>
      /// <returns>A text string with the <see cref="T:DirectorySeparatorChar"/> or <see cref="T:AltDirectorySeparatorChar"/> character suffixed. The function returns <c>null</c> when <paramref name="path"/> is <c>null</c>.</returns>
      [SecurityCritical]
      public static string AddDirectorySeparator(string path)
      {
         return AddDirectorySeparator(path, false);
      }

      /// <summary>[AlphaFS] Adds a <see cref="T:DirectorySeparatorChar"/> or <see cref="T:AltDirectorySeparatorChar"/> character to the string.</summary>
      /// <param name="path">A text string to which the <see cref="T:DirectorySeparatorChar"/> or <see cref="T:AltDirectorySeparatorChar"/> is to be added.</param>
      /// <param name="addAlternateSeparator">if <c>true</c> the <see cref="T:AltDirectorySeparatorChar"/> character will be added instead.</param>
      /// <returns>A text string with the <see cref="T:DirectorySeparatorChar"/> or <see cref="T:AltDirectorySeparatorChar"/> character suffixed. The function returns <c>null</c> when <paramref name="path"/> is <c>null</c>.</returns>
      [SecurityCritical]
      public static string AddDirectorySeparator(string path, bool addAlternateSeparator)
      {
         if (path == null)
            return null;

         return addAlternateSeparator
            ? ((!path.EndsWith(AltDirectorySeparatorChar.ToString(CultureInfo.CurrentCulture), StringComparison.OrdinalIgnoreCase))
               ? path + AltDirectorySeparatorChar : path)
            : ((!path.EndsWith(DirectorySeparatorChar.ToString(CultureInfo.CurrentCulture), StringComparison.OrdinalIgnoreCase))
               ? path + DirectorySeparatorChar : path);
      }

      #endregion // AddDirectorySeparator
      
      #region GetDirectoryNameWithoutRoot

      /// <summary>[AlphaFS] Returns the directory information for the specified path string without the root information, for example: C:\Windows\system32 --> Windows</summary>
      /// <param name="path">The path.</param>
      /// <returns>The <paramref name="path"/>without the file name part and without the root information (if any), or <c>null</c> if <paramref name="path"/> is <c>null</c> or if <paramref name="path"/> denotes a root (such as "\", "C:", or * "\\server\share").</returns>
      [SecurityCritical]
      public static string GetDirectoryNameWithoutRoot(string path)
      {
         return GetDirectoryNameWithoutRootInternal(null, path);
      }

      #region Transacted

      /// <summary>[AlphaFS] Returns the directory information for the specified path string without the root information, for example: C:\Windows\system32 --> Windows</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path.</param>
      /// <returns>The <paramref name="path"/>without the file name part and without the root information (if any), or <c>null</c> if <paramref name="path"/> is <c>null</c> or if <paramref name="path"/> denotes a root (such as "\", "C:", or * "\\server\share").</returns>
      [SecurityCritical]
      public static string GetDirectoryNameWithoutRoot(KernelTransaction transaction, string path)
      {
         return GetDirectoryNameWithoutRootInternal(transaction, path);
      }

      #endregion // Transacted

      #endregion // GetDirectoryNameWithoutRoot

      #region GetFinalPathNameByHandle

      /// <summary>[AlphaFS] Retrieves the final path for the specified file, formatted as <see cref="T:FinalPathFormats"/>.</summary>
      /// <param name="handle">Then handle to a <see cref="T:SafeFileHandle"/> instance.</param>
      /// <returns>Returns the final path as a string.</returns>
      /// <remarks>
      /// A final path is the path that is returned when a path is fully resolved.
      /// For example, for a symbolic link named "C:\tmp\mydir" that points to "D:\yourdir", the final path would be "D:\yourdir".
      /// </remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static string GetFinalPathNameByHandle(SafeFileHandle handle)
      {
         return GetFinalPathNameByHandleInternal(handle, FinalPathFormats.None);
      }

      /// <summary>[AlphaFS] Retrieves the final path for the specified file, formatted as <see cref="T:FinalPathFormats"/>.</summary>
      /// <param name="handle">Then handle to a <see cref="T:SafeFileHandle"/> instance.</param>
      /// <param name="finalPath">The final path, formatted as <see cref="T:FinalPathFormats"/></param>
      /// <returns>Returns the final path as a string.</returns>
      /// <remarks>
      /// A final path is the path that is returned when a path is fully resolved.
      /// For example, for a symbolic link named "C:\tmp\mydir" that points to "D:\yourdir", the final path would be "D:\yourdir".
      /// </remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static string GetFinalPathNameByHandle(SafeFileHandle handle, FinalPathFormats finalPath)
      {
         return GetFinalPathNameByHandleInternal(handle, finalPath);
      }

      #endregion // GetFinalPathNameByHandle

      #region GetLongPath

      /// <summary>[AlphaFS] Makes a Unicode path (LongPath) of the specified <paramref name="path"/> by prefixing <see cref="T:LongPathPrefix"/>.</summary>
      /// <param name="path">The local or UNC path to the file or directory.</param>
      /// <returns>The <paramref name="path"/> prefixed with a <see cref="T:LongPathPrefix"/>.</returns>
      [SecurityCritical]
      public static string GetLongPath(string path)
      {
         return GetLongPathInternal(path, true, false, false, false);
      }

      #endregion // GetLongPath

      #region GetLongFrom83Path

      /// <summary>[AlphaFS] Converts the specified existing path to its regular long form.</summary>
      /// <param name="path">An existing path to a folder or file.</param>
      /// <returns>The regular full path.</returns>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static string GetLongFrom83Path(string path)
      {
         return GetLongShort83PathInternal(null, path, false);
      }

      #region Transacted

      /// <summary>[AlphaFS] Converts the specified existing path to its regular long form.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">An existing path to a folder or file.</param>
      /// <returns>The regular full path.</returns>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static string GetLongFrom83Path(KernelTransaction transaction, string path)
      {
         return GetLongShort83PathInternal(transaction, path, false);
      }

      #endregion // Transacted

      #endregion // GetLongFrom83Path

      #region GetMappedConnectionName

      /// <summary>[AlphaFS] Gets the connection name of the locally mapped drive.</summary>
      /// <param name="path">The local path with drive name.</param>
      /// <returns>The server and share as: \\servername\sharename.</returns>
      /// <exception cref="PathTooLongException">When <paramref name="path"/> exceeds <see cref="T:NativeMethods.MaxPath"/></exception>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static string GetMappedConnectionName(string path)
      {
         return Host.GetRemoteNameInfoInternal(path, true).ConnectionName;
      }

      #endregion // GetMappedConnectionName

      #region GetMappedUncName

      /// <summary>[AlphaFS] Gets the network share name from the locally mapped path.</summary>
      /// <param name="path">The local path with drive name.</param>
      /// <returns>The network share connection name of <paramref name="path"/>.</returns>
      /// <exception cref="PathTooLongException">When <paramref name="path"/> exceeds <see cref="T:NativeMethods.MaxPath"/></exception>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static string GetMappedUncName(string path)
      {
         return Host.GetRemoteNameInfoInternal(path, true).UniversalName;
      }

      #endregion // GetMappedUncName

      #region GetRegularPath

      /// <summary>[AlphaFS] Gets the regular path from long prefixed one. i.e.: \\?\C:\Temp\file.txt to C:\Temp\file.txt or: \\?\UNC\Server\share\file.txt to \\Server\share\file.txt</summary>
      /// <param name="path">The path.</param>
      /// <returns>Regular form path string.</returns>
      /// <remarks>This method does not handle paths with volume names, eg. \\?\Volume{GUID}\Folder\file.txt </remarks>
      [SecurityCritical]
      public static string GetRegularPath(string path)
      {
         return GetRegularPathInternal(path, true, false, false, false);
      }

      #endregion // GetRegularPath

      #region GetShort83Path

      /// <summary>[AlphaFS] Retrieves the short path form of the specified path.</summary>
      /// <param name="path">An existing path to a folder or file.</param>
      /// <returns>A path that has the 8.3 path form.</returns>
      /// <remarks>Will fail on NTFS volumes with disabled 8.3 name generation.</remarks>
      /// <remarks>The path must actually exist to be able to get the short path name.</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static string GetShort83Path(string path)
      {
         return GetLongShort83PathInternal(null, path, true);
      }

      #region Transacted

      /// <summary>[AlphaFS] Retrieves the short path form of the specified path.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">An existing path to a folder or file.</param>
      /// <returns>A path that has the 8.3 path form.</returns>
      /// <remarks>Will fail on NTFS volumes with disabled 8.3 name generation.</remarks>
      /// <remarks>The path must actually exist to be able to get the short path name.</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public static string GetShort83Path(KernelTransaction transaction, string path)
      {
         return GetLongShort83PathInternal(transaction, path, true);
      }

      #endregion // Transacted

      #endregion // GetShort83Path

      #region GetSuffixedDirectoryName

      /// <summary>[AlphaFS] Returns the directory information for the specified <paramref name="path"/> with a trailing directory separator.</summary>
      /// <param name="path">The path.</param>
      /// <returns>The suffixed directory information for the specified <paramref name="path"/> with a trailing directory separator, or <c>null</c> if <paramref name="path"/> is <c>null</c> or if <paramref name="path"/> denotes a root (such as "\", "C:", or * "\\server\share").</returns>
      [SecurityCritical]
      public static string GetSuffixedDirectoryName(string path)
      {
         return GetSuffixedDirectoryNameInternal(null, path);
      }

      #region Transacted

      /// <summary>[AlphaFS] Returns the directory information for the specified <paramref name="path"/> with a trailing directory separator.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path.</param>
      /// <returns>The suffixed directory information for the specified <paramref name="path"/> with a trailing directory separator, or <c>null</c> if <paramref name="path"/> is <c>null</c> or if <paramref name="path"/> denotes a root (such as "\", "C:", or * "\\server\share").</returns>
      [SecurityCritical]
      public static string GetSuffixedDirectoryName(KernelTransaction transaction, string path)
      {
         return GetSuffixedDirectoryNameInternal(transaction, path);
      }

      #endregion // Transacted

      #endregion // GetSuffixedDirectoryName

      #region GetSuffixedDirectoryNameWithoutRoot

      /// <summary>[AlphaFS] Returns the directory information for the specified <paramref name="path"/> without the root and with a trailing directory separator.</summary>
      /// <param name="path">The path.</param>
      /// <returns>The directory information for the specified <paramref name="path"/> without the root and with a trailing directory separator, or <c>null</c> if <paramref name="path"/> is <c>null</c>.</returns>
      [SecurityCritical]
      public static string GetSuffixedDirectoryNameWithoutRoot(string path)
      {
         return GetSuffixedDirectoryNameWithoutRootInternal(null, path);
      }

      #region Transacted

      /// <summary>[AlphaFS] Returns the directory information for the specified <paramref name="path"/> without the root and with a trailing directory separator.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path.</param>
      /// <returns>The directory information for the specified <paramref name="path"/> without the root and with a trailing directory separator, or <c>null</c> if <paramref name="path"/> is <c>null</c>.</returns>
      [SecurityCritical]
      public static string GetSuffixedDirectoryNameWithoutRoot(KernelTransaction transaction, string path)
      {
         return GetSuffixedDirectoryNameWithoutRootInternal(transaction, path);
      }

      #endregion // Transacted

      #endregion // GetSuffixedDirectoryNameWithoutRoot
      
      #region IsLocalPath

      /// <summary>[AlphaFS] Determines whether the specified path is a local path.</summary>
      /// <param name="path">The path to check.</param>
      /// <returns><c>true</c> if the specified path is a local path, <c>false</c> otherwise.</returns>
      [SecurityCritical]
      public static bool IsLocalPath(string path)
      {
         return IsLocalPath(path, true);
      }

      /// <summary>[AlphaFS] Determines whether the specified path is a local path.</summary>
      /// <param name="path">The path to check.</param>
      /// <param name="checkInvalidChars"><c>true</c> will not check <paramref name="path"/> for invalid path characters.</param>
      /// <returns><c>true</c> if the specified path is a local path, <c>false</c> otherwise.</returns>
      [SecurityCritical]
      public static bool IsLocalPath(string path, bool checkInvalidChars)
      {
         if (Utils.IsNullOrWhiteSpace(path))
            return false;

         path = GetRegularPathInternal(path, checkInvalidChars, false, false, false);

         // Don't use char.IsLetter() here as that can be misleading.
         // The only valid drive letters are: a-z and A-Z.
         char c = path[0];
         return IsPathRooted(path) && ((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z')) && path[1] == VolumeSeparatorChar;
      }

      #endregion // IsLocalPath

      #region IsLongPath

      /// <summary>[AlphaFS] Determines whether the specified path is starts with <see cref="T:LongPathPrefix"/> or <see cref="T:LongPathUncPrefix"/>.</summary>
      /// <param name="path">The path to the file or directory.</param>
      /// <returns><c>true</c> if the specified path has a long path (UNC) prefix, <c>false</c> otherwise.</returns>
      [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0")]
      [SecurityCritical]
      public static bool IsLongPath(string path)
      {
         return !Utils.IsNullOrWhiteSpace(path) && path.StartsWith(LongPathPrefix, StringComparison.OrdinalIgnoreCase);
      }

      #endregion // IsLongPath

      #region IsUncPath

      /// <summary>[AlphaFS] Determines if a path string is a valid Universal Naming Convention (UNC) path.</summary>
      /// <param name="path">The path to check.</param>
      /// <returns><c>true</c> if the specified path is a Universal Naming Convention (UNC) path, <c>false</c> otherwise.</returns>
      [SecurityCritical]
      public static bool IsUncPath(string path)
      {
         return IsUncPath(path, true);
      }

      /// <summary>[AlphaFS] Determines if a path string is a valid Universal Naming Convention (UNC) path.</summary>
      /// <param name="path">The path to check.</param>
      /// <param name="checkInvalidChars"><c>true</c> will not check <paramref name="path"/> for invalid path characters.</param>
      /// <returns><c>true</c> if the specified path is a Universal Naming Convention (UNC) path, <c>false</c> otherwise.</returns>
      [SecurityCritical]
      public static bool IsUncPath(string path, bool checkInvalidChars)
      {
         Uri uri;
         return Uri.TryCreate(GetRegularPathInternal(path, checkInvalidChars, false, false, false), UriKind.Absolute, out uri) && uri.IsUnc;
      }

      #endregion // IsUncPath

      #region LocalToUnc

      /// <summary>[AlphaFS] Converts a local path to a network share path.
      /// A Local path, e.g.: "C:\Windows" will be returned as: "\\localhostname\C$\Windows"
      /// If a logical drive points to a network share path, the share path will be returned.
      /// </summary>
      /// <param name="localPath">A local path, e.g.: "C:\Windows"</param>
      /// <returns>A UNC path or <c>null</c> when <paramref name="localPath"/> is <c>string.Empty</c> or <c>null</c>.</returns>
      [SecurityCritical]
      public static string LocalToUnc(string localPath)
      {
         return LocalToUncInternal(localPath, false, false, false, false);
      }

      /// <summary>[AlphaFS] Converts a local path to a network share path.
      /// A Local path, e.g.: "C:\Windows" will be returned as: "\\localhostname\C$\Windows"
      /// If a logical drive points to a network share path, the share path will be returned.
      /// </summary>
      /// <param name="localPath">A local path, e.g.: "C:\Windows"</param>
      /// <param name="asLongPath"><c>true</c> returns the path in long path (Unicode) format, when <c>false</c> returns the path as a regular path.</param>
      /// <returns>A UNC path or <c>null</c> when <paramref name="localPath"/> is <c>string.Empty</c> or <c>null</c>.</returns>
      [SecurityCritical]
      public static string LocalToUnc(string localPath, bool asLongPath)
      {
         return LocalToUncInternal(localPath, asLongPath, false, false, false);
      }

      /// <summary>[AlphaFS] Converts a local path to a network share path.
      /// A Local path, e.g.: "C:\Windows" will be returned as: "\\localhostname\C$\Windows"
      /// If a logical drive points to a network share path, the share path will be returned.
      /// </summary>
      /// <param name="localPath">A local path, e.g.: "C:\Windows"</param>
      /// <param name="asLongPath"><c>true</c> returns the path in long path (Unicode) format, when <c>false</c> returns the path as a regular path.</param>
      /// <param name="addDirectorySeparator"><c>true</c> adds a directory separator to that path.</param>
      /// <param name="removeDirectorySeparator"><c>true</c> removes any directory separator to that path.</param>
      /// <returns>A UNC path or <c>null</c> when <paramref name="localPath"/> is <c>string.Empty</c> or <c>null</c>.</returns>
      [SecurityCritical]
      public static string LocalToUnc(string localPath, bool asLongPath, bool addDirectorySeparator, bool removeDirectorySeparator)
      {
         return LocalToUncInternal(localPath, asLongPath, false, addDirectorySeparator, removeDirectorySeparator);
      }

      #endregion // LocalToUnc
      
      #region RemoveDirectorySeparator

      /// <summary>[AlphaFS] Removes the <see cref="T:DirectorySeparatorChar"/> character from the string.</summary>
      /// <param name="path">A text string from which the <see cref="T:DirectorySeparatorChar"/> is to be removed.</param>
      /// <returns>A text string where the suffixed <see cref="T:DirectorySeparatorChar"/> has been removed. The function returns <c>null</c> when <paramref name="path"/> is <c>null</c>.</returns>
      [SecurityCritical]
      public static string RemoveDirectorySeparator(string path)
      {
         return path == null ? null : path.TrimEnd(DirectorySeparatorChar, AltDirectorySeparatorChar);
      }

      /// <summary>[AlphaFS] Removes the <see cref="T:DirectorySeparatorChar"/> or <see cref="T:AltDirectorySeparatorChar"/> character from the string.</summary>
      /// <param name="path">A text string from which the <see cref="T:DirectorySeparatorChar"/> or <see cref="T:AltDirectorySeparatorChar"/> is to be removed.</param>
      /// <param name="removeAlternateSeparator">If <c>true</c> the <see cref="T:AltDirectorySeparatorChar"/> character will be removed instead.</param>
      /// <returns>A text string where the suffixed <see cref="T:DirectorySeparatorChar"/> or <see cref="T:AltDirectorySeparatorChar"/> character has been removed. The function returns <c>null</c> when <paramref name="path"/> is <c>null</c>.</returns>
      [SecurityCritical]
      public static string RemoveDirectorySeparator(string path, bool removeAlternateSeparator)
      {
         return path == null
            ? null
            : path.TrimEnd(removeAlternateSeparator ? AltDirectorySeparatorChar : DirectorySeparatorChar);
      }

      #endregion // RemoveDirectorySeparator


      #region Internal Utility

      #region CheckInvalidPathChars

      /// <summary>[AlphaFS] Checks that the path contains only valid path-characters.</summary>
      /// <param name="path">A path to the file or directory.</param>
      [SecurityCritical]
      private static void CheckInvalidPathChars(string path)
      {
         if (path == null)
            throw new ArgumentNullException("path");

         for (int index = 0, l = path.Length; index < l; ++index)
         {
            int num = path[index];
            switch (num)
            {
               case 34:    // "  (quote)
               case 60:    // <  (less than)
               case 62:    // >  (greater than)
               case 124:   // |  (pipe)
                  throw new ArgumentException("Invalid Path Chars");

               //default:
               //   // 32: space
               //   if (num >= 32 && (!checkAdditional || num != WildcardQuestionChar && num != WildcardStarMatchAllChar))
               //      continue;

               //   goto case 34;
            }
         }
      }

      #endregion // CheckInvalidPathChars

      #region DosDeviceToDosPath

      /// <summary>[AlphaFS] Tranlates DosDevicePath, Volume GUID.
      /// For example: "\Device\HarddiskVolumeX\path\filename.ext" can translate to: "\path\filename.ext" or: "\\?\Volume{GUID}\path\filename.ext".
      /// </summary>
      /// <param name="dosDevice">A DosDevicePath, for example: \Device\HarddiskVolumeX\path\filename.ext</param>
      /// <param name="deviceReplacement">Alternate path/device text, usually <c>string.Empty</c> or <c>null</c>.</param>
      /// <returns>A translated dos path.</returns>
      [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
      [SecurityCritical]
      private static string DosDeviceToDosPath(string dosDevice, string deviceReplacement)
      {
         if (Utils.IsNullOrWhiteSpace(dosDevice))
            return string.Empty;

         foreach (string drive in Directory.EnumerateLogicalDrivesInternal(false, false).Select(drv => drv.Name))
         {
            try
            {
               string path = RemoveDirectorySeparator(drive, false);
               foreach (string devNt in Volume.QueryDosDevice(path).Where(dosDevice.StartsWith))
                  return dosDevice.Replace(devNt, deviceReplacement ?? path);
            }
            catch
            {
            }
         }
         return string.Empty;
      }

      #endregion // DosDeviceToDosPath

      #region EndsWithDVsc

      /// <summary>[AlphaFS] Check if <paramref name="path"/> ends with a directory- and/or volume-separator character.</summary>
      /// <param name="path">The patch to check.</param>
      /// <param name="checkVolumeSeparatorChar">
      /// If <c>null</c>, checks for all separator characters: <see cref="T:DirectorySeparatorChar"/>, <see cref="T:AltDirectorySeparatorChar"/> and <see cref="T:VolumeSeparatorChar"/>
      /// If <c>false</c>, only checks for: <see cref="T:DirectorySeparatorChar"/> and <see cref="T:AltDirectorySeparatorChar"/>
      /// If <c>true</c>, only checks for: <see cref="T:VolumeSeparatorChar"/>
      ///</param>
      /// <returns><c>true</c> if <paramref name="path"/> ends with a separator character.</returns>
      [SecurityCritical]
      internal static bool EndsWithDVsc(string path, bool? checkVolumeSeparatorChar)
      {
         return path != null && path.Length >= 1 && IsDVsc(path[path.Length - 1], checkVolumeSeparatorChar);
      }

      #endregion // EndsWithDVsc

      #region GetRootLength

      [SecurityCritical]
      internal static int GetRootLength(string path, bool checkInvalidChars)
      {
         if (checkInvalidChars)
            CheckInvalidPathChars(path);

         int index = 0;
         int length = path.Length;

         if (length >= 1 && IsDVsc(path[0], false))
         {
            index = 1;
            if (length >= 2 && IsDVsc(path[1], false))
            {
               index = 2;
               int num = 2;

               while (index < length && (!IsDVsc(path[index], false) || --num > 0))
                  ++index;
            }
         }
         else if (length >= 2 && IsDVsc(path[1], true))
         {
            index = 2;
            if (length >= 3 && IsDVsc(path[2], false))
               ++index;
         }

         return index;
      }

      #endregion // GetRootLength

      #region IsDVsc

      /// <summary>[AlphaFS] Check if <paramref name="c"/> is a directory- and/or volume-separator character.</summary>
      /// <param name="c">The character to check.</param>
      /// <param name="checkSeparatorChar">
      /// If <c>null</c>, checks for all separator characters: <see cref="T:DirectorySeparatorChar"/>, <see cref="T:AltDirectorySeparatorChar"/> and <see cref="T:VolumeSeparatorChar"/>
      /// If <c>false</c>, only checks for: <see cref="T:DirectorySeparatorChar"/> and <see cref="T:AltDirectorySeparatorChar"/>
      /// If <c>true</c> only checks for: <see cref="T:VolumeSeparatorChar"/>
      ///</param>
      /// <returns><c>true</c> if <paramref name="c"/> is a separator character.</returns>
      [SecurityCritical]
      internal static bool IsDVsc(char c, bool? checkSeparatorChar)
      {
         return checkSeparatorChar == null

            // Check for all separator characters.
            ? c == DirectorySeparatorChar || c == AltDirectorySeparatorChar || c == VolumeSeparatorChar

            // Check for some separator characters.
            : ((bool) checkSeparatorChar
               ? c == VolumeSeparatorChar
               : c == DirectorySeparatorChar || c == AltDirectorySeparatorChar);
      }

      #endregion // IsDVsc

      #endregion // Internal Utility

      #region Unified Internals

      #region Combine

      #region .NET

      /// <summary>Unified method Combine() to combine an array of strings into a path.</summary>
      /// <param name="paths">An array of parts of the path.</param>
      /// <param name="checkInvalidChars"><c>true</c> will not check <paramref name="paths"/> for invalid path characters.</param>
      /// <returns>The combined paths.</returns>
      /// <exception cref="ArgumentException">One of the strings in the array contains one or more of the invalid characters defined in <see cref="T:GetInvalidPathChars"/>.</exception>
      /// <exception cref="ArgumentNullException">One of the strings in the array is <c>null</c>.</exception>
      [SecurityCritical]
      internal static string CombineInternal(bool checkInvalidChars, params string[] paths)
      {
         if (paths == null)
            throw new ArgumentNullException("paths");

         int capacity = 0;
         int num = 0;
         for (int index = 0, l = paths.Length; index < l; ++index)
         {
            if (paths[index] == null)
               throw new ArgumentNullException("paths");

            if (paths[index].Length != 0)
            {
               if (checkInvalidChars)
                  CheckInvalidPathChars(paths[index]);

               if (IsPathRooted(paths[index]))
               {
                  num = index;
                  capacity = paths[index].Length;
               }
               else
                  capacity += paths[index].Length;

               char ch = paths[index][paths[index].Length - 1];

               if (!IsDVsc(ch, null))
                  ++capacity;
            }
         }

         StringBuilder buffer = new StringBuilder(capacity);
         for (int index = num; index < paths.Length; ++index)
         {
            if (paths[index].Length != 0)
            {
               if (buffer.Length == 0)
                  buffer.Append(paths[index]);

               else
               {
                  char ch = buffer[buffer.Length - 1];

                  if (!IsDVsc(ch, null))
                     buffer.Append(DirectorySeparatorChar);

                  buffer.Append(paths[index]);
               }
            }
         }

         return buffer.ToString();
      }

      #endregion // .NET

      #endregion // Combine

      #region GetDirectoryNameWithoutRootInternal

      /// <summary>[AlphaFS] Unified method GetDirectoryNameWithoutRootInternal() to return the directory information for the specified path string without the root information, for example: C:\Windows\system32 --> Windows</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path.</param>
      /// <returns>The <paramref name="path"/>without the file name part and without the root information (if any), or <c>null</c> if <paramref name="path"/> is <c>null</c> or if <paramref name="path"/> denotes a root (such as "\", "C:", or * "\\server\share").</returns>
      [SecurityCritical]
      private static string GetDirectoryNameWithoutRootInternal(KernelTransaction transaction, string path)
      {
         if (path == null)
            return null;

         DirectoryInfo di = Directory.GetParentInternal(transaction, path, false);
         return di != null && di.Parent != null ? di.Name : null;
      }

      #endregion // GetDirectoryNameWithoutRootInternal

      #region GetFinalPathNameByHandleInternal

      /// <summary>[AlphaFS] Unified method GetFinalPathNameByHandleInternal() to retrieve the final path for the specified file, formatted as <see cref="T:FinalPathFormats"/>.</summary>
      /// <param name="handle">Then handle to a <see cref="T:SafeFileHandle"/> instance.</param>
      /// <param name="finalPath">The final path, formatted as <see cref="T:FinalPathFormats"/></param>
      /// <returns>Returns the final path as a string.</returns>
      /// <remarks>
      /// A final path is the path that is returned when a path is fully resolved.
      /// For example, for a symbolic link named "C:\tmp\mydir" that points to "D:\yourdir", the final path would be "D:\yourdir".
      /// The string that is returned by this function uses the <see cref="T:LongPathPrefix"/> syntax.
      /// </remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults", MessageId = "Alphaleonis.Win32.Filesystem.NativeMethods.GetMappedFileName(System.IntPtr,Alphaleonis.Win32.SafeGlobalMemoryBufferHandle,System.Text.StringBuilder,System.UInt32)")]
      [SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults", MessageId = "Alphaleonis.Win32.Filesystem.NativeMethods.GetMappedFileName(System.IntPtr,Alphaleonis.Win32.Security.SafeLocalMemoryBufferHandle,System.Text.StringBuilder,System.UInt32)")]
      [SecurityCritical]
      internal static string GetFinalPathNameByHandleInternal(SafeFileHandle handle, FinalPathFormats finalPath)
      {
         NativeMethods.IsValidHandle(handle);
         
         StringBuilder buffer = new StringBuilder(NativeMethods.MaxPathUnicode);


         // ChangeErrorMode is for the Win32 SetThreadErrorMode() method, used to suppress possible pop-ups.
         using (new NativeMethods.ChangeErrorMode(NativeMethods.ErrorMode.FailCriticalErrors))
         {
            if (NativeMethods.IsAtLeastWindowsVista)
            {
               if (NativeMethods.GetFinalPathNameByHandle(handle, buffer, (uint) buffer.Capacity, finalPath) == Win32Errors.ERROR_SUCCESS)
                  NativeError.ThrowException(Marshal.GetLastWin32Error());

               return buffer.ToString();
            }
         }

         #region Older OperatingSystem
         
         // Obtaining a File Name From a File Handle
         // http://msdn.microsoft.com/en-us/library/aa366789%28VS.85%29.aspx

         // Be careful when using GetFileSizeEx to check the size of hFile handle of an unknown "File" type object.
         // This is more towards returning a filename from a file handle. If the handle is a named pipe handle it seems to hang the thread.
         // Check for: FileTypes.DiskFile

         // Can't map a 0 byte file.
         long fileSizeHi;
         if (!NativeMethods.GetFileSizeEx(handle, out fileSizeHi))
            if (fileSizeHi == 0)
               return string.Empty;

         
         // PAGE_READONLY
         // Allows views to be mapped for read-only or copy-on-write access. An attempt to write to a specific region results in an access violation.
         // The file handle that the hFile parameter specifies must be created with the GENERIC_READ access right.
         // PageReadOnly = 0x02,
         using (SafeFileHandle handle2 = NativeMethods.CreateFileMapping(handle, null, 2, 0, 1, null))
         {
            NativeMethods.IsValidHandle(handle, Marshal.GetLastWin32Error());

            // FILE_MAP_READ
            // Read = 4
            using (SafeLocalMemoryBufferHandle pMem = NativeMethods.MapViewOfFile(handle2, 4, 0, 0, (UIntPtr)1))
            {
               if (NativeMethods.IsValidHandle(pMem, Marshal.GetLastWin32Error()))
                  if (NativeMethods.GetMappedFileName(Process.GetCurrentProcess().Handle, pMem, buffer, (uint) buffer.Capacity))
                     NativeMethods.UnmapViewOfFile(pMem);
            }
         }


         // Default output from GetMappedFileName(): "\Device\HarddiskVolumeX\path\filename.ext"
         string dosDevice = buffer.Length > 0 ? buffer.ToString() : string.Empty;

         // Select output format.
         switch (finalPath)
         {
            // As-is: "\Device\HarddiskVolumeX\path\filename.ext"
            case FinalPathFormats.VolumeNameNT:
               return dosDevice;

            // To: "\path\filename.ext"
            case FinalPathFormats.VolumeNameNone:
               return DosDeviceToDosPath(dosDevice, string.Empty);

            // To: "\\?\Volume{GUID}\path\filename.ext"
            case FinalPathFormats.VolumeNameGuid:
               string dosPath = DosDeviceToDosPath(dosDevice, null);
               if (!Utils.IsNullOrWhiteSpace(dosPath))
               {
                  string path = GetSuffixedDirectoryNameWithoutRootInternal(null, dosPath);
                  string driveLetter = RemoveDirectorySeparator(GetPathRoot(dosPath, false), false);
                  string file = GetFileName(dosPath, true);

                  if (!Utils.IsNullOrWhiteSpace(file))
                     foreach (string drive in Directory.EnumerateLogicalDrivesInternal(false, false).Select(drv => drv.Name).Where(drv => driveLetter.Equals(RemoveDirectorySeparator(drv, false), StringComparison.OrdinalIgnoreCase)))
                        return CombineInternal(false, Volume.GetUniqueVolumeNameForPath(drive), path, file);
               }

               break;
         }

         // To: "\\?\C:\path\filename.ext"
         return Utils.IsNullOrWhiteSpace(dosDevice)
            ? string.Empty
            : LongPathPrefix + DosDeviceToDosPath(dosDevice, null);

         #endregion // Older OperatingSystem
      }

      #endregion // GetFinalPathNameByHandleInternal

      #region GetFullPathInternal

      /// <summary>[AlphaFS] Unified method GetFullPathInternal() to retrieve the absolute path for the specified path string.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file or directory for which to obtain absolute path information.</param>
      /// <param name="asLongPath"><c>true</c> returns the path in long path (Unicode) format, when <c>false</c> returns the path as a regular path.</param>
      /// <param name="trimEnd"><c>true</c> removes trailing whitespace from <paramref name="path"/>.</param>
      /// <param name="addDirectorySeparator"><c>true</c> adds a directory separator to that path.</param>
      /// <param name="removeDirectorySeparator"><c>true</c> removes any directory separator to that path.</param>
      /// <param name="continueOnNotExist"><c>true</c> does not throw an Exception when the file system object does not exist.</param>
      /// <returns>The fully qualified location of path, such as "C:\MyFile.txt".</returns>
      /// <remarks>This method does not verify that the resulting path and file name are valid, or that they see an existing file on the associated volume.</remarks>
      /// <remarks>GetFullPath does not work reliable with relative paths.</remarks>
      /// <remarks>GetFullPath is not recommended for multithreaded applications or shared library code.</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      internal static string GetFullPathInternal(KernelTransaction transaction, string path, bool asLongPath, bool trimEnd, bool addDirectorySeparator, bool removeDirectorySeparator, bool continueOnNotExist)
      {
         if (path == null)
            return null;

         string pathLp = GetLongPathInternal(path, true, trimEnd, addDirectorySeparator, removeDirectorySeparator);
         uint bufferSize = NativeMethods.MaxPathUnicode;

         // ChangeErrorMode is for the Win32 SetThreadErrorMode() method, used to suppress possible pop-ups.
         using (new NativeMethods.ChangeErrorMode(NativeMethods.ErrorMode.FailCriticalErrors))
         {
            startGetFullPathName:

            StringBuilder buffer = new StringBuilder((int) bufferSize);
            uint returnLength = (transaction == null || !NativeMethods.IsAtLeastWindowsVista

               // GetFullPathName() / GetFullPathNameTransacted()
               // In the ANSI version of this function, the name is limited to MAX_PATH characters.
               // To extend this limit to 32,767 wide characters, call the Unicode version of the function and prepend "\\?\" to the path.
               // 2013-04-15: MSDN confirms LongPath usage.

               ? NativeMethods.GetFullPathName(pathLp, bufferSize, buffer, IntPtr.Zero)
               : NativeMethods.GetFullPathNameTransacted(pathLp, bufferSize, buffer, IntPtr.Zero, transaction.SafeHandle));

            if (returnLength != Win32Errors.NO_ERROR)
            {
               if (returnLength > bufferSize)
               {
                  bufferSize = returnLength;
                  goto startGetFullPathName;
               }
            }
            else
            {
               if (continueOnNotExist)
                  return null;

               NativeError.ThrowException(pathLp);
            }

            return asLongPath
               ? GetLongPathInternal(buffer.ToString(), false, false, false, false)
               : GetRegularPathInternal(buffer.ToString(), false, false, false, false);
         }
      }

      #endregion GetFullPathInternal

      #region GetLongPathInternal

      /// <summary>[AlphaFS] Unified method GetLongPathInternal() to get a long path (Unicode path) of the specified <paramref name="path"/></summary>
      /// <param name="path">The path to the file or directory, this may also be an UNC path.</param>
      /// <param name="checkInvalidChars">Checks that the path contains only valid path-characters.</param>
      /// <param name="trimEnd"><c>true</c> removes trailing whitespace from <paramref name="path"/>.</param>
      /// <param name="addDirectorySeparator"><c>true</c> adds a directory separator to that path.</param>
      /// <param name="removeDirectorySeparator"><c>true</c> removes any directory separator to that path.</param>
      /// <returns>The <paramref name="path"/> as a long path.</returns>
      [SecurityCritical]
      internal static string GetLongPathInternal(string path, bool checkInvalidChars, bool trimEnd, bool addDirectorySeparator, bool removeDirectorySeparator)
      {
         if (path == null)
            return null;

         if (checkInvalidChars)
            CheckInvalidPathChars(path);

         // MSDN: Notes to Callers
         // http://msdn.microsoft.com/en-us/library/system.string.trimend%28v=vs.110%29.aspx
         //
         // The .NET Framework 3.5 SP1 and earlier versions maintains an internal list of white-space characters that this method trims if trimChars is null or an empty array.
         // Starting with the .NET Framework 4, if trimChars is null or an empty array, the method trims all Unicode white-space characters
         // (that is, characters that produce a true return value when they are passed to the Char.IsWhiteSpace method). Because of this change,
         // the Trim() method in the .NET Framework 3.5 SP1 and earlier versions removes two characters, ZERO WIDTH SPACE (U+200B) and ZERO WIDTH NO-BREAK SPACE (U+FEFF),
         // that the Trim() method in the .NET Framework 4 and later versions does not remove. In addition, the Trim() method in the .NET Framework 3.5 SP1 and earlier versions
         // does not trim three Unicode white-space characters: MONGOLIAN VOWEL SEPARATOR (U+180E), NARROW NO-BREAK SPACE (U+202F), and MEDIUM MATHEMATICAL SPACE (U+205F).
         
         if (trimEnd) path = path.TrimEnd();
         if (addDirectorySeparator) path = AddDirectorySeparator(path, false);
         if (removeDirectorySeparator) path = RemoveDirectorySeparator(path, false);

         if (path.StartsWith(LongPathPrefix, StringComparison.OrdinalIgnoreCase) ||
             path.StartsWith(LogicalDrivePrefix, StringComparison.OrdinalIgnoreCase))
            return path;

         //path = GetRegularPathInternal(path, false, trimEnd, addDirectorySeparator, removeDirectorySeparator);

         // ".", "C:"
         return path.Length > 2 && (IsLocalPath(path, false) || IsUncPath(path, false))
            ? path.StartsWith(UncPrefix, StringComparison.OrdinalIgnoreCase)
               ? LongPathUncPrefix + path.Substring(UncPrefix.Length)
               : LongPathPrefix + path
            : path;
      }

      #endregion // GetLongPathInternal

      #region GetLongShort83PathInternal

      /// <summary>[AlphaFS] Unified method GetLongShort83PathInternal() to retrieve the short path form, or the regular long form of the specified path.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">An existing path to a folder or file.</param>
      /// <param name="getShort"><c>true</c> to retrieve the short path form, <c>false</c> to retrieve the regular long form from the 8.3 <paramref name="path"/>.</param>
      /// <returns>If <paramref name="getShort"/> is <c>true</c>, a path of the 8.3 form otherwise the regular long form.</returns>
      /// <remarks>
      /// <para>Will fail on NTFS volumes with disabled 8.3 name generation.</para>
      /// <para>The path must actually exist to be able to get the short- or long path name.</para>
      /// </remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      private static string GetLongShort83PathInternal(KernelTransaction transaction, string path, bool getShort)
      {
         if (Utils.IsNullOrWhiteSpace(path))
            throw new ArgumentNullException("path");

         string pathLp = GetFullPathInternal(transaction, path, true, false, false, false, false);

         StringBuilder buffer = new StringBuilder();
         uint actualLength = getShort ? NativeMethods.GetShortPathName(pathLp, null, 0) : (uint) path.Length;

         while (actualLength > buffer.Capacity)
         {
            buffer = new StringBuilder((int) actualLength);
            actualLength = getShort

               // GetShortPathName()
               // In the ANSI version of this function, the name is limited to MAX_PATH characters.
               // To extend this limit to 32,767 wide characters, call the Unicode version of the function and prepend "\\?\" to the path.
               // 2014-01-29: MSDN confirms LongPath usage.

               ? NativeMethods.GetShortPathName(pathLp, buffer, (uint) buffer.Capacity)
               : transaction == null || !NativeMethods.IsAtLeastWindowsVista

                  // GetLongPathName()
                  // In the ANSI version of this function, the name is limited to MAX_PATH characters.
                  // To extend this limit to 32,767 wide characters, call the Unicode version of the function and prepend "\\?\" to the path.
                  // 2014-01-29: MSDN confirms LongPath usage.

                  ? NativeMethods.GetLongPathName(pathLp, buffer, (uint) buffer.Capacity)
                  : NativeMethods.GetLongPathNameTransacted(pathLp, buffer, (uint) buffer.Capacity, transaction.SafeHandle);

            if (actualLength == Win32Errors.ERROR_SUCCESS)
               NativeError.ThrowException(pathLp);
         }

         return GetRegularPathInternal(buffer.ToString(), false, false, false, false); 
      }

      #endregion // GetLongShort83PathInternal

      #region GetRegularPathInternal

      /// <summary>[AlphaFS] Unified method GetRegularPathInternal() to get the regular path from long prefixed one. i.e.: \\?\C:\Temp\file.txt to C:\Temp\file.txt or: \\?\UNC\Server\share\file.txt to \\Server\share\file.txt</summary>
      /// <param name="path">The path.</param>
      /// <param name="checkInvalidChars">Checks that the path contains only valid path-characters.</param>
      /// <param name="trimEnd"><c>true</c> removes trailing whitespace from <paramref name="path"/>.</param>
      /// <param name="addDirectorySeparator"><c>true</c> adds a directory separator to that path.</param>
      /// <param name="removeDirectorySeparator"><c>true</c> removes any directory separator to that path.</param>
      /// <returns>The <paramref name="path"/> as a regular path.</returns>
      [SecurityCritical]
      internal static string GetRegularPathInternal(string path, bool checkInvalidChars, bool trimEnd, bool addDirectorySeparator, bool removeDirectorySeparator)
      {
         if (path == null)
            return null;

         if (checkInvalidChars)
            CheckInvalidPathChars(path);

         // MSDN: Notes to Callers
         // http://msdn.microsoft.com/en-us/library/system.string.trimend%28v=vs.110%29.aspx
         //
         // The .NET Framework 3.5 SP1 and earlier versions maintains an internal list of white-space characters that this method trims if trimChars is null or an empty array.
         // Starting with the .NET Framework 4, if trimChars is null or an empty array, the method trims all Unicode white-space characters
         // (that is, characters that produce a true return value when they are passed to the Char.IsWhiteSpace method). Because of this change,
         // the Trim() method in the .NET Framework 3.5 SP1 and earlier versions removes two characters, ZERO WIDTH SPACE (U+200B) and ZERO WIDTH NO-BREAK SPACE (U+FEFF),
         // that the Trim() method in the .NET Framework 4 and later versions does not remove. In addition, the Trim() method in the .NET Framework 3.5 SP1 and earlier versions
         // does not trim three Unicode white-space characters: MONGOLIAN VOWEL SEPARATOR (U+180E), NARROW NO-BREAK SPACE (U+202F), and MEDIUM MATHEMATICAL SPACE (U+205F).

         if (trimEnd) path = path.TrimEnd();

         if (removeDirectorySeparator)
            path = path.TrimEnd(DirectorySeparatorChar, AltDirectorySeparatorChar);

         if (addDirectorySeparator)
            path = AddDirectorySeparator(path, false);

         
         if (!path.StartsWith(LongPathPrefix, StringComparison.OrdinalIgnoreCase))
            return path;

         
         if (path.StartsWith(LongPathUncPrefix, StringComparison.OrdinalIgnoreCase))
            return UncPrefix + path.Substring(LongPathUncPrefix.Length);

         
         return path.Substring(LongPathPrefix.Length);
      }

      #endregion // GetRegularPathInternal

      #region GetSuffixedDirectoryNameInternal

      /// <summary>[AlphaFS] Unified method GetSuffixedDirectoryNameInternal() to return the directory information for the specified <paramref name="path"/> with a trailing directory separator.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path.</param>
      /// <returns>The suffixed directory information for the specified <paramref name="path"/> with a trailing directory separator, or <c>null</c> if <paramref name="path"/> is <c>null</c> or if <paramref name="path"/> denotes a root (such as "\", "C:", or * "\\server\share").</returns>
      [SecurityCritical]
      private static string GetSuffixedDirectoryNameInternal(KernelTransaction transaction, string path)
      {
         if (path == null)
            return null;

         DirectoryInfo di = Directory.GetParentInternal(transaction, path, false);
         return di != null && di.Parent != null && di.Name != null
            ? AddDirectorySeparator(CombineInternal(false, di.Parent.FullName, di.Name), false)
            : null;
      }

      #endregion // GetSuffixedDirectoryNameInternal

      #region GetSuffixedDirectoryNameWithoutRootInternal

      /// <summary>[AlphaFS] Unified method GetSuffixedDirectoryNameWithoutRootInternal() to return the directory information for the specified <paramref name="path"/> with a trailing directory separator.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path.</param>
      /// <returns>The suffixed directory information for the specified <paramref name="path"/> with a trailing directory separator, or <c>null</c> if <paramref name="path"/> is <c>null</c> or if <paramref name="path"/> denotes a root (such as "\", "C:", or * "\\server\share").</returns>
      [SecurityCritical]
      private static string GetSuffixedDirectoryNameWithoutRootInternal(KernelTransaction transaction, string path)
      {
         if (path == null)
            return null;

         DirectoryInfo di = Directory.GetParentInternal(transaction, path, false);
         if (di == null || di.Parent == null)
            return null;

         DirectoryInfo tmp = di;
         string suffixedDirectoryNameWithoutRoot;

         do
         {
            suffixedDirectoryNameWithoutRoot = tmp.DisplayPath.Replace(di.Root.ToString(), "");

            if (tmp.Parent != null)
               tmp = di.Parent.Parent;

         } while (tmp != null && tmp.Root.Parent != null && tmp.Parent != null && !Utils.IsNullOrWhiteSpace(tmp.Parent.ToString()));

         return Utils.IsNullOrWhiteSpace(suffixedDirectoryNameWithoutRoot)
            ? null
            : AddDirectorySeparator(suffixedDirectoryNameWithoutRoot.TrimStart(DirectorySeparatorChar), false);
         // TrimStart() for network-drive, like: C$
      }

      #endregion // GetSuffixedDirectoryNameWithoutRootInternal

      #region LocalToUncInternal

      /// <summary>[AlphaFS] Unified method LocalToUncInternal() to converts a local path to a network share path.
      /// A Local path, e.g.: "C:\Windows" will be returned as: "\\localhostname\C$\Windows"
      /// If a logical drive points to a network share path, the share path will be returned.
      /// </summary>
      /// <param name="localPath">A local path, e.g.: "C:\Windows"</param>
      /// <param name="asLongPath"><c>true</c> returns the path in long path (Unicode) format, when <c>false</c> returns the path as a regular path.</param>
      /// <param name="trimEnd"><c>true</c> removes trailing whitespace from <paramref name="localPath"/>.</param>
      /// <param name="addDirectorySeparator"><c>true</c> adds a directory separator to that <paramref name="localPath"/>.</param>
      /// <param name="removeDirectorySeparator"><c>true</c> removes any directory separator from <paramref name="localPath"/>.</param>
      /// <returns>A UNC path or <c>null</c> when <paramref name="localPath"/> is <c>string.Empty</c> or <c>null</c>.</returns>
      [SecurityCritical]
      internal static string LocalToUncInternal(string localPath, bool asLongPath, bool trimEnd, bool addDirectorySeparator, bool removeDirectorySeparator)
      {
         if (Utils.IsNullOrWhiteSpace(localPath))
            return null;

         localPath = (localPath.Equals(CurrentDirectoryPrefix, StringComparison.OrdinalIgnoreCase))
            ? GetFullPathInternal(null, localPath, asLongPath, trimEnd, addDirectorySeparator, removeDirectorySeparator, false)
            : GetRegularPathInternal(localPath, true, trimEnd, addDirectorySeparator, removeDirectorySeparator);

         if (IsUncPath(localPath, false))
            return localPath;

         string drive = IsLocalPath(localPath, false) ? GetPathRoot(localPath, false) : null;

         if (Utils.IsNullOrWhiteSpace(drive))
            return null;

         
         Network.NativeMethods.RemoteNameInfo unc = Host.GetRemoteNameInfoInternal(drive, true);

         if (!Utils.IsNullOrWhiteSpace(unc.ConnectionName))
            // Only leave trailing backslash if "localPath" also ends with backslash.
            return localPath.EndsWith(DirectorySeparator, StringComparison.OrdinalIgnoreCase) ? AddDirectorySeparator(unc.ConnectionName, false) : RemoveDirectorySeparator(unc.ConnectionName, false);

         // Split: localDrive[0] = "C", localDrive[1] = "\Windows"
         string[] localDrive = localPath.Split(VolumeSeparatorChar);

         // Return: "\\MachineName\C$\Windows"
         string pathUnc = string.Format(CultureInfo.CurrentCulture, "{0}{1}{2}${3}", Host.GetUncName(), DirectorySeparatorChar, localDrive[0], localDrive[1]);

         // Only leave trailing backslash if "localPath" also ends with backslash.
         return localPath.EndsWith(DirectorySeparator, StringComparison.OrdinalIgnoreCase) ? AddDirectorySeparator(pathUnc, false) : RemoveDirectorySeparator(pathUnc, false);
      }

      #endregion // LocalToUncInternal

      #endregion // Unified Internals

      #endregion // AlphaFS

      #endregion // Methods

      #region Fields

      #region .NET

      /// <summary>AltDirectorySeparatorChar = '/' Provides a platform-specific alternate character used to separate directory levels in a path string that reflects a hierarchical file system organization.</summary>
      public static readonly char AltDirectorySeparatorChar = System.IO.Path.AltDirectorySeparatorChar;

      /// <summary>DirectorySeparatorChar = '\' Provides a platform-specific character used to separate directory levels in a path string that reflects a hierarchical file system organization.</summary>
      public static readonly char DirectorySeparatorChar = System.IO.Path.DirectorySeparatorChar;

      /// <summary>PathSeparator = ';' A platform-specific separator character used to separate path strings in environment variables.</summary>
      public static readonly char PathSeparator = System.IO.Path.PathSeparator;
      
      /// <summary>VolumeSeparatorChar = ':' Provides a platform-specific Volume Separator character.</summary>
      public static readonly char VolumeSeparatorChar = System.IO.Path.VolumeSeparatorChar;

      #endregion // .NET

      #region AlphaFS

      /// <summary>[AlphaFS] AltDirectorySeparatorChar = "/" Provides a platform-specific alternate string used to separate directory levels in a path string that reflects a hierarchical file system organization.</summary>
      public static readonly string AltDirectorySeparator = AltDirectorySeparatorChar.ToString(CultureInfo.CurrentCulture);

      /// <summary>[AlphaFS] CurrentDirectoryPrefix = '.' Provides a current directory character.</summary>
      public const char CurrentDirectoryPrefixChar = '.';

      /// <summary>[AlphaFS] CurrentDirectoryPrefix = "." Provides a current directory string.</summary>
      public static readonly string CurrentDirectoryPrefix = CurrentDirectoryPrefixChar.ToString(CultureInfo.CurrentCulture);

      /// <summary>[AlphaFS] DirectorySeparator = "\" Provides a platform-specific string used to separate directory levels in a path string that reflects a hierarchical file system organization.</summary>
      public static readonly string DirectorySeparator = DirectorySeparatorChar.ToString(CultureInfo.CurrentCulture);

      /// <summary>[AlphaFS] ExtensionSeparatorChar = '.' Provides an Extension Separator character.</summary>
      public const char ExtensionSeparatorChar = '.';

      /// <summary>[AlphaFS] ParentDirectoryPrefix = ".." Provides a parent directory string.</summary>
      public const string ParentDirectoryPrefix = "..";

      /// <summary>[AlphaFS] StreamSeparator = ':' Provides a platform-specific Stream-name character.</summary>
      public static readonly char StreamSeparatorChar = System.IO.Path.VolumeSeparatorChar;

      /// <summary>[AlphaFS] StreamSeparator = ':' Provides a platform-specific Stream-name character.</summary>
      public static readonly string StreamSeparator = StreamSeparatorChar.ToString(CultureInfo.CurrentCulture);

      /// <summary>[AlphaFS] StringTerminatorChar = '\0' String Terminator Suffix.</summary>
      public const char StringTerminatorChar = '\0';

      /// <summary>[AlphaFS] VolumeSeparatorChar = ':' Provides a platform-specific Volume Separator character.</summary>
      public static readonly string VolumeSeparator = VolumeSeparatorChar.ToString(CultureInfo.CurrentCulture);

      /// <summary>[AlphaFS] WildcardStarMatchAll = "*" Provides a match-all-items string.</summary>
      public const string WildcardStarMatchAll = "*";

      /// <summary>[AlphaFS] WildcardStarMatchAll = '*' Provides a match-all-items character.</summary>
      public const char WildcardStarMatchAllChar = '*';

      /// <summary>[AlphaFS] WildcardQuestion = "?" Provides a replace-item string.</summary>
      public const string WildcardQuestion = "?";

      /// <summary>[AlphaFS] WildcardQuestion = '?' Provides a replace-item string.</summary>
      public const char WildcardQuestionChar = '?';


      /// <summary>[AlphaFS] UncPrefix = "\\" Provides standard Windows Path UNC prefix.</summary>
      public static readonly string UncPrefix = string.Format(CultureInfo.CurrentCulture, "{0}{0}", DirectorySeparatorChar);


      /// <summary>[AlphaFS] LongPathPrefix = "\\?\" Provides standard Windows Long Path prefix.</summary>
      public static readonly string LongPathPrefix = string.Format(CultureInfo.CurrentCulture, "{0}{1}{2}", UncPrefix, WildcardQuestion, DirectorySeparatorChar);

      /// <summary>[AlphaFS] LongPathUncPrefix = "\\?\UNC\" Provides standard Windows Long Path UNC prefix.</summary>
      public static readonly string LongPathUncPrefix = string.Format(CultureInfo.CurrentCulture, "{0}{1}{2}", LongPathPrefix, "UNC", DirectorySeparatorChar);


      /// <summary>[AlphaFS] GlobalRootPrefix = "\\?\GLOBALROOT\" Provides standard Windows Volume prefix.</summary>
      public static readonly string GlobalRootPrefix = string.Format(CultureInfo.CurrentCulture, "{0}{1}{2}", LongPathPrefix, "GLOBALROOT", DirectorySeparatorChar);

      /// <summary>[AlphaFS] MsDosNamespacePrefix = "\\.\" Provides standard logical drive prefix.</summary>
      public static readonly string LogicalDrivePrefix = string.Format(CultureInfo.CurrentCulture, "{0}{0}.{0}", DirectorySeparatorChar);

      /// <summary>[AlphaFS] SubstitutePrefix = "\??\" Provides a SUBST.EXE Path prefix to a Logical Drive.</summary>
      public static readonly string SubstitutePrefix = string.Format(CultureInfo.CurrentCulture, "{0}{1}{2}{0}", DirectorySeparatorChar, WildcardQuestion, WildcardQuestion);

      /// <summary>[AlphaFS] VolumePrefix = "\\?\Volume" Provides standard Windows Volume prefix.</summary>
      public static readonly string VolumePrefix = string.Format(CultureInfo.CurrentCulture, "{0}{1}", LongPathPrefix, "Volume");

      /// <summary>[AlphaFS] DevicePrefix = "\Device\" Provides standard Windows Device prefix.</summary>
      public static readonly string DevicePrefix = string.Format(CultureInfo.CurrentCulture, "{0}{1}{0}", DirectorySeparatorChar, "Device");

      /// <summary>[AlphaFS] DosDeviceLanmanPrefix = "\Device\LanmanRedirector\" Provides a MS-Dos Lanman Redirector Path UNC prefix to a network share.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Lanman")]
      public static readonly string DosDeviceLanmanPrefix = string.Format(CultureInfo.CurrentCulture, "{0}{1}{2}", DevicePrefix, "LanmanRedirector", DirectorySeparatorChar);

      /// <summary>[AlphaFS] DosDeviceMupPrefix = "\Device\Mup\" Provides a MS-Dos Mup Redirector Path UNC prefix to a network share.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Mup")]
      public static readonly string DosDeviceMupPrefix = string.Format(CultureInfo.CurrentCulture, "{0}{1}{2}", DevicePrefix, "Mup", DirectorySeparatorChar);

      /// <summary>[AlphaFS] DosDeviceUncPrefix = "\??\UNC\" Provides a SUBST.EXE Path UNC prefix to a network share.</summary>
      public static readonly string DosDeviceUncPrefix = string.Format(CultureInfo.CurrentCulture, "{0}{1}{2}", SubstitutePrefix, "UNC", DirectorySeparatorChar);

      #endregion // AlphaFS

      #endregion // Fields
   }
}