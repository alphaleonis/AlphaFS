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
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Security;
using SearchOption = System.IO.SearchOption;

namespace Alphaleonis.Win32.Filesystem
{
   partial class Directory
   {
      #region EnumerateDirectories

      #region Non-Transactional

      /// <summary>Returns an enumerable collection of directory names in a specified <paramref name="path"/>.</summary>
      /// <param name="path">The directory to search.</param>
      /// <returns>An enumerable collection of the full names (including paths) for the directories in the directory specified by <paramref name="path"/>.</returns>
      [SecurityCritical]
      public static IEnumerable<string> EnumerateDirectories(string path)
      {
         return EnumerateFileSystemEntryInfosInternal<string>(null, path, Path.WildcardStarMatchAll, DirectoryEnumerationOptions.Folders, PathFormat.RelativePath);
      }

      /// <summary>Returns an enumerable collection of directory names that match a <paramref name="searchPattern"/> in a specified <paramref name="path"/>.</summary>
      /// <param name="path">The directory to search.</param>
      /// <param name="searchPattern">
      /// <para>The search string to match against the names of directories in <paramref name="path"/>. This parameter can contain a</para>
      /// <para>combination of valid literal path and wildcard (<see cref="Path.WildcardStarMatchAll"/> and <see cref="Path.WildcardQuestion"/>)</para>
      /// <para>characters, but does not support regular expressions.</para>
      /// </param>
      /// <returns>An enumerable collection of the full names (including paths) for the directories in the directory specified by <paramref name="path"/> and that match the specified <paramref name="searchPattern"/>.</returns>
      [SecurityCritical]
      public static IEnumerable<string> EnumerateDirectories(string path, string searchPattern)
      {
         return EnumerateFileSystemEntryInfosInternal<string>(null, path, searchPattern, DirectoryEnumerationOptions.Folders, PathFormat.RelativePath);
      }

      /// <summary>Returns an enumerable collection of directory names that match a <paramref name="searchPattern"/> in a specified <paramref name="path"/>, and optionally searches subdirectories.</summary>
      /// <param name="path">The directory to search.</param>
      /// <param name="searchPattern">
      /// <para>The search string to match against the names of directories in <paramref name="path"/>. This parameter can contain a</para>
      /// <para>combination of valid literal path and wildcard (<see cref="Path.WildcardStarMatchAll"/> and <see cref="Path.WildcardQuestion"/>)</para>
      /// <para>characters, but does not support regular expressions.</para>
      /// </param>
      /// <param name="searchOption">
      /// <para>One of the <see cref="SearchOption"/> enumeration values that specifies whether the <paramref name="searchOption"/></para>
      /// <para> should include only the current directory or should include all subdirectories.</para>
      /// </param>
      /// <returns>An enumerable collection of the full names (including paths) for the directories in the directory specified by <paramref name="path"/> and that match the specified <paramref name="searchPattern"/> and <paramref name="searchOption"/>.</returns>
      [SecurityCritical]
      public static IEnumerable<string> EnumerateDirectories(string path, string searchPattern, SearchOption searchOption)
      {
         var options = DirectoryEnumerationOptions.Folders | ((searchOption == SearchOption.AllDirectories) ? DirectoryEnumerationOptions.Recursive : 0);

         return EnumerateFileSystemEntryInfosInternal<string>(null, path, searchPattern, options, PathFormat.RelativePath);
      }

      #endregion

      #region Transactional

      /// <summary>[AlphaFS] Returns an enumerable collection of directory instances in a specified <paramref name="path"/>.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory to search.</param>
      /// <returns>An enumerable collection of the full names (including paths) for the directories in the directory specified by <paramref name="path"/>.</returns>
      [SecurityCritical]
      public static IEnumerable<string> EnumerateDirectories(KernelTransaction transaction, string path)
      {
         return EnumerateFileSystemEntryInfosInternal<string>(transaction, path, Path.WildcardStarMatchAll, DirectoryEnumerationOptions.Folders, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Returns an enumerable collection of directory instances that match a <paramref name="searchPattern"/> in a specified <paramref name="path"/>.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory to search.</param>
      /// <param name="searchPattern">
      /// <para>The search string to match against the names of directories in <paramref name="path"/>. This parameter can contain a</para>
      /// <para>combination of valid literal path and wildcard (<see cref="Path.WildcardStarMatchAll"/> and <see cref="Path.WildcardQuestion"/>)</para>
      /// <para>characters, but does not support regular expressions.</para>
      /// </param>
      /// <returns>An enumerable collection of the full names (including paths) for the directories in the directory specified by <paramref name="path"/> and that match the specified <paramref name="searchPattern"/>.</returns>
      [SecurityCritical]
      public static IEnumerable<string> EnumerateDirectories(KernelTransaction transaction, string path, string searchPattern)
      {
         return EnumerateFileSystemEntryInfosInternal<string>(transaction, path, searchPattern, DirectoryEnumerationOptions.Folders, PathFormat.RelativePath);
      }

      /// <summary>Returns an enumerable collection of directory names that match a <paramref name="searchPattern"/> in a specified <paramref name="path"/>, and optionally searches subdirectories.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory to search.</param>
      /// <param name="searchPattern">
      /// <para>The search string to match against the names of directories in <paramref name="path"/>. This parameter can contain a</para>
      /// <para>combination of valid literal path and wildcard (<see cref="Path.WildcardStarMatchAll"/> and <see cref="Path.WildcardQuestion"/>)</para>
      /// <para>characters, but does not support regular expressions.</para>
      /// </param>
      /// <param name="searchOption">
      /// <para>One of the <see cref="SearchOption"/> enumeration values that specifies whether the <paramref name="searchOption"/></para>
      /// <para> should include only the current directory or should include all subdirectories.</para>
      /// </param>
      /// <returns>An enumerable collection of the full names (including paths) for the directories in the directory specified by <paramref name="path"/> and that match the specified <paramref name="searchPattern"/> and <paramref name="searchOption"/>.</returns>
      [SecurityCritical]
      public static IEnumerable<string> EnumerateDirectories(KernelTransaction transaction, string path, string searchPattern, SearchOption searchOption)
      {
         var options = DirectoryEnumerationOptions.Folders | ((searchOption == SearchOption.AllDirectories) ? DirectoryEnumerationOptions.Recursive : 0);

         return EnumerateFileSystemEntryInfosInternal<string>(transaction, path, searchPattern, options, PathFormat.RelativePath);
      }


      #endregion // Transactional

      #endregion // EnumerateDirectories

      #region EnumerateFiles

      #region Non-Transactional

      /// <summary>Returns an enumerable collection of file names in a specified <paramref name="path"/>.</summary>
      /// <param name="path">The directory to search.</param>
      /// <returns>An enumerable collection of the full names (including paths) for the files in the directory specified by <paramref name="path"/>.</returns>
      [SecurityCritical]
      public static IEnumerable<string> EnumerateFiles(string path)
      {
         return EnumerateFileSystemEntryInfosInternal<string>(null, path, Path.WildcardStarMatchAll, DirectoryEnumerationOptions.Files, PathFormat.RelativePath);
      }

      /// <summary>Returns an enumerable collection of file names in a specified <paramref name="path"/>.</summary>
      /// <param name="path">The directory to search.</param>
      /// <param name="searchPattern">
      /// <para>The search string to match against the names of directories in <paramref name="path"/>. This parameter can contain a</para>
      /// <para>combination of valid literal path and wildcard (<see cref="Path.WildcardStarMatchAll"/> and <see cref="Path.WildcardQuestion"/>)</para>
      /// <para>characters, but does not support regular expressions.</para>
      /// </param>
      /// <returns>An enumerable collection of the full names (including paths) for the files in the directory specified by <paramref name="path"/> and that match the <paramref name="searchPattern"/>.</returns>
      [SecurityCritical]
      public static IEnumerable<string> EnumerateFiles(string path, string searchPattern)
      {
         return EnumerateFileSystemEntryInfosInternal<string>(null, path, searchPattern, DirectoryEnumerationOptions.Files, PathFormat.RelativePath);
      }

      /// <summary>Returns an enumerable collection of file names that match a <paramref name="searchPattern"/> in a specified <paramref name="path"/>, and optionally searches subdirectories.</summary>
      /// <param name="path">The directory to search.</param>
      /// <param name="searchPattern">
      /// <para>The search string to match against the names of directories in <paramref name="path"/>. This parameter can contain a</para>
      /// <para>combination of valid literal path and wildcard (<see cref="Path.WildcardStarMatchAll"/> and <see cref="Path.WildcardQuestion"/>)</para>
      /// <para>characters, but does not support regular expressions.</para>
      /// </param>
      /// <param name="searchOption">
      /// <para>One of the <see cref="SearchOption"/> enumeration values that specifies whether the <paramref name="searchOption"/></para>
      /// <para> should include only the current directory or should include all subdirectories.</para>
      /// </param>
      /// <returns>An enumerable collection of the full names (including paths) for the files in the directory specified by <paramref name="path"/> and that match the specified <paramref name="searchPattern"/> and <paramref name="searchOption"/>.</returns>
      [SecurityCritical]
      public static IEnumerable<string> EnumerateFiles(string path, string searchPattern, SearchOption searchOption)
      {
         var options = DirectoryEnumerationOptions.Files | ((searchOption == SearchOption.AllDirectories) ? DirectoryEnumerationOptions.Recursive : 0);

         return EnumerateFileSystemEntryInfosInternal<string>(null, path, searchPattern, options, PathFormat.RelativePath);
      }

      #endregion

      #region Transactional

      /// <summary>[AlphaFS] Returns an enumerable collection of file names in a specified <paramref name="path"/>.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory to search.</param>
      /// <returns>An enumerable collection of the full names (including paths) for the files in the directory specified by <paramref name="path"/>.</returns>
      [SecurityCritical]
      public static IEnumerable<string> EnumerateFiles(KernelTransaction transaction, string path)
      {
         return EnumerateFileSystemEntryInfosInternal<string>(transaction, path, Path.WildcardStarMatchAll, DirectoryEnumerationOptions.Files, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Returns an enumerable collection of file instances that match a <paramref name="searchPattern"/> in a specified <paramref name="path"/>.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory to search.</param>
      /// <param name="searchPattern">
      /// <para>The search string to match against the names of directories in <paramref name="path"/>. This parameter can contain a</para>
      /// <para>combination of valid literal path and wildcard (<see cref="Path.WildcardStarMatchAll"/> and <see cref="Path.WildcardQuestion"/>)</para>
      /// <para>characters, but does not support regular expressions.</para>
      /// </param>
      /// <returns>An enumerable collection of the full names (including paths) for the files in the directory specified by <paramref name="path"/> and that match the <paramref name="searchPattern"/>.</returns>
      [SecurityCritical]
      public static IEnumerable<string> EnumerateFiles(KernelTransaction transaction, string path, string searchPattern)
      {
         return EnumerateFileSystemEntryInfosInternal<string>(transaction, path, searchPattern, DirectoryEnumerationOptions.Files, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Returns an enumerable collection of file instances instances that match a <paramref name="searchPattern"/> in a specified <paramref name="path"/>, and optionally searches subdirectories.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory to search.</param>
      /// <param name="searchPattern">
      /// <para>The search string to match against the names of directories in <paramref name="path"/>. This parameter can contain a</para>
      /// <para>combination of valid literal path and wildcard (<see cref="Path.WildcardStarMatchAll"/> and <see cref="Path.WildcardQuestion"/>)</para>
      /// <para>characters, but does not support regular expressions.</para>
      /// </param>
      /// <param name="searchOption">
      /// <para>One of the <see cref="SearchOption"/> enumeration values that specifies whether the <paramref name="searchOption"/></para>
      /// <para> should include only the current directory or should include all subdirectories.</para>
      /// </param>
      /// <returns>An enumerable collection of the full names (including paths) for the files in the directory specified by <paramref name="path"/> and that match the specified <paramref name="searchPattern"/> and <paramref name="searchOption"/>.</returns>
      [SecurityCritical]
      public static IEnumerable<string> EnumerateFiles(KernelTransaction transaction, string path, string searchPattern, SearchOption searchOption)
      {
         var options = DirectoryEnumerationOptions.Files | ((searchOption == SearchOption.AllDirectories) ? DirectoryEnumerationOptions.Recursive : 0);

         return EnumerateFileSystemEntryInfosInternal<string>(transaction, path, searchPattern, options, PathFormat.RelativePath);
      }

      #endregion // Transactional

      #endregion // EnumerateFiles

      #region EnumerateFileSystemEntries

      #region Non-Transactional

      /// <summary>Returns an enumerable collection of file names and directory names in a specified <paramref name="path"/>.</summary>
      /// <param name="path">The directory to search.</param>
      /// <returns>An enumerable collection of <see cref="FileSystemEntryInfo"/> entries in the directory specified by <paramref name="path"/>.</returns>
      [SecurityCritical]
      public static IEnumerable<string> EnumerateFileSystemEntries(string path)
      {
         return EnumerateFileSystemEntryInfosInternal<string>(null, path, Path.WildcardStarMatchAll, DirectoryEnumerationOptions.FilesAndFolders, PathFormat.RelativePath);
      }

      /// <summary>Returns an enumerable collection of file names and directory names that match a <paramref name="searchPattern"/> in a specified <paramref name="path"/>.</summary>
      /// <param name="path">The directory to search.</param>
      /// <param name="searchPattern">
      /// <para>The search string to match against the names of directories in <paramref name="path"/>. This parameter can contain a</para>
      /// <para>combination of valid literal path and wildcard (<see cref="Path.WildcardStarMatchAll"/> and <see cref="Path.WildcardQuestion"/>)</para>
      /// <para>characters, but does not support regular expressions.</para>
      /// </param>
      /// <returns>An enumerable collection of file-system entries in the directory specified by <paramref name="path"/> and that match the specified <paramref name="searchPattern"/>.</returns>
      [SecurityCritical]
      public static IEnumerable<string> EnumerateFileSystemEntries(string path, string searchPattern)
      {
         return EnumerateFileSystemEntryInfosInternal<string>(null, path, searchPattern, DirectoryEnumerationOptions.FilesAndFolders, PathFormat.RelativePath);
      }

      /// <summary>Returns an enumerable collection of file names and directory names that match a <paramref name="searchPattern"/> in a specified <paramref name="path"/>, and optionally searches subdirectories.</summary>
      /// <param name="path">The directory to search.</param>
      /// <param name="searchPattern">
      /// <para>The search string to match against the names of directories in <paramref name="path"/>. This parameter can contain a</para>
      /// <para>combination of valid literal path and wildcard (<see cref="Path.WildcardStarMatchAll"/> and <see cref="Path.WildcardQuestion"/>)</para>
      /// <para>characters, but does not support regular expressions.</para>
      /// </param>
      /// <param name="searchOption">
      /// <para>One of the <see cref="SearchOption"/> enumeration values that specifies whether the <paramref name="searchOption"/></para>
      /// <para> should include only the current directory or should include all subdirectories.</para>
      /// </param>
      /// <returns>An enumerable collection of file-system entries in the directory specified by <paramref name="path"/> and that match the specified <paramref name="searchPattern"/> and <paramref name="searchOption"/>.</returns>
      [SecurityCritical]
      public static IEnumerable<string> EnumerateFileSystemEntries(string path, string searchPattern, System.IO.SearchOption searchOption)
      {
         var options = DirectoryEnumerationOptions.FilesAndFolders | ((searchOption == SearchOption.AllDirectories) ? DirectoryEnumerationOptions.Recursive : 0);

         return EnumerateFileSystemEntryInfosInternal<string>(null, path, searchPattern, options, PathFormat.RelativePath);
      }

      #endregion

      #region Transactional

      /// <summary>Returns an enumerable collection of file names and directory names in a specified <paramref name="path"/>.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory to search.</param>
      /// <returns>An enumerable collection of <see cref="FileSystemEntryInfo"/> entries in the directory specified by <paramref name="path"/>.</returns>
      [SecurityCritical]
      public static IEnumerable<string> EnumerateFileSystemEntries(KernelTransaction transaction, string path)
      {
         return EnumerateFileSystemEntryInfosInternal<string>(transaction, path, Path.WildcardStarMatchAll, DirectoryEnumerationOptions.FilesAndFolders, PathFormat.RelativePath);
      }

      /// <summary>Returns an enumerable collection of file names and directory names that match a <paramref name="searchPattern"/> in a specified <paramref name="path"/>.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory to search.</param>
      /// <param name="searchPattern">
      /// <para>The search string to match against the names of directories in <paramref name="path"/>. This parameter can contain a</para>
      /// <para>combination of valid literal path and wildcard (<see cref="Path.WildcardStarMatchAll"/> and <see cref="Path.WildcardQuestion"/>)</para>
      /// <para>characters, but does not support regular expressions.</para>
      /// </param>
      /// <returns>An enumerable collection of file-system entries in the directory specified by <paramref name="path"/> and that match the specified <paramref name="searchPattern"/>.</returns>
      [SecurityCritical]
      public static IEnumerable<string> EnumerateFileSystemEntries(KernelTransaction transaction, string path, string searchPattern)
      {
         return EnumerateFileSystemEntryInfosInternal<string>(transaction, path, searchPattern, DirectoryEnumerationOptions.FilesAndFolders, PathFormat.RelativePath);
      }

      /// <summary>Returns an enumerable collection of file names and directory names that match a <paramref name="searchPattern"/> in a specified <paramref name="path"/>, and optionally searches subdirectories.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory to search.</param>
      /// <param name="searchPattern">
      /// <para>The search string to match against the names of directories in <paramref name="path"/>. This parameter can contain a</para>
      /// <para>combination of valid literal path and wildcard (<see cref="Path.WildcardStarMatchAll"/> and <see cref="Path.WildcardQuestion"/>)</para>
      /// <para>characters, but does not support regular expressions.</para>
      /// </param>
      /// <param name="searchOption">
      /// <para>One of the <see cref="SearchOption"/> enumeration values that specifies whether the <paramref name="searchOption"/></para>
      /// <para> should include only the current directory or should include all subdirectories.</para>
      /// </param>
      /// <returns>An enumerable collection of file-system entries in the directory specified by <paramref name="path"/> and that match the specified <paramref name="searchPattern"/> and <paramref name="searchOption"/>.</returns>
      [SecurityCritical]
      public static IEnumerable<string> EnumerateFileSystemEntries(KernelTransaction transaction, string path, string searchPattern, SearchOption searchOption)
      {
         var options = DirectoryEnumerationOptions.FilesAndFolders | ((searchOption == SearchOption.AllDirectories) ? DirectoryEnumerationOptions.Recursive : 0);

         return EnumerateFileSystemEntryInfosInternal<string>(transaction, path, searchPattern, options, PathFormat.RelativePath);
      }

      #endregion // Transactional

      #endregion // EnumerateFileSystemEntries

      #region EnumerateFileSystemEntryInfos

      #region Non-Transactional

      /// <summary>
      /// [AlphaFS] Returns an enumerable collection of file system entries in a specified path.
      /// </summary>
      /// <typeparam name="T"></typeparam>
      /// <param name="path">The directory to search.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>
      ///    The matching file system entries. The type of the items is determined by the type <typeparamref name="T"/>.
      /// </returns>
      /// <exception cref="ArgumentException"></exception>
      /// <exception cref="ArgumentNullException"></exception>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
      [SecurityCritical]
      public static IEnumerable<T> EnumerateFileSystemEntryInfos<T>(string path, PathFormat pathFormat)
      {
         return EnumerateFileSystemEntryInfosInternal<T>(null, path, Path.WildcardStarMatchAll, DirectoryEnumerationOptions.FilesAndFolders, pathFormat);
      }

      /// <summary>
      ///   [AlphaFS] Returns an enumerable collection of file system entries that match a
      ///   <paramref name="searchPattern"/> in a specified path.
      /// </summary>
      /// <typeparam name="T">
      ///   The type to return. This may be one of the following types:
      ///   <list type="definition">
      ///   <item>
      ///      <term>
      ///        <see cref="FileSystemInfo"/>
      ///      </term>
      ///      <description>
      ///        This method will return instances of <see cref="DirectoryInfo"/>,
      ///        <see cref="FileInfo"/> and <see cref="FileSystemEntryInfo"/> instances.
      ///      </description>
      ///   </item>
      ///   <item>
      ///      <term>
      ///        <see cref="string"/>
      ///      </term>
      ///      <description>
      ///        This method will return the full path of each item.
      ///      </description>
      ///   </item>
      ///   </list>
      /// </typeparam>
      /// <param name="path">The directory to search.</param>
      /// <param name="searchPattern">
      ///   <para>The search string to match against the names of directories in
      ///   <paramref name="path"/>. This parameter can contain a combination of valid literal path and
      ///   wildcard
      ///   (<see cref="Path.WildcardStarMatchAll"/> and <see cref="Path.WildcardQuestion"/>)
      ///   characters, but doesn't support regular expressions.</para>
      /// </param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>
      ///    The matching file system entries. The type of the items is determined by the type <typeparamref name="T"/>.
      /// </returns>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
      [SecurityCritical]
      public static IEnumerable<T> EnumerateFileSystemEntryInfos<T>(string path, string searchPattern, PathFormat pathFormat)
      {
         return EnumerateFileSystemEntryInfosInternal<T>(null, path, searchPattern, DirectoryEnumerationOptions.FilesAndFolders, pathFormat);
      }

      /// <summary>
      ///   [AlphaFS] Returns an enumerable collection of file system entries that match a
      ///   <paramref name="searchPattern"/> in a specified path using
      ///   <see cref="DirectoryEnumerationOptions"/>.
      /// </summary>
      /// <typeparam name="T">
      ///   The type to return. This may be one of the following types:
      ///   <list type="definition">
      ///   <item>
      ///      <term>
      ///        <see cref="FileSystemInfo"/>
      ///      </term>
      ///      <description>
      ///        This method will return instances of <see cref="DirectoryInfo"/>,
      ///        <see cref="FileInfo"/> and <see cref="FileSystemEntryInfo"/> instances.
      ///      </description>
      ///   </item>
      ///   <item>
      ///      <term>
      ///        <see cref="string"/>
      ///      </term>
      ///      <description>
      ///        This method will return the full path of each item.
      ///      </description>
      ///   </item>
      ///   </list>
      /// </typeparam>
      /// <param name="path">The directory to search.</param>
      /// <param name="searchPattern">
      ///   <para>The search string to match against the names of directories in
      ///   <paramref name="path"/>. This parameter can contain a</para>
      ///   <para>combination of valid literal path and wildcard
      ///   (<see cref="Path.WildcardStarMatchAll"/> and <see cref="Path.WildcardQuestion"/>)</para>
      ///   <para>characters, but doesn't support regular expressions.</para>
      /// </param>
      /// <param name="options">
      ///   <see cref="DirectoryEnumerationOptions"/> flags that specify how the directory is to be
      ///   enumerated.
      /// </param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>
      ///    The matching file system entries. The type of the items is determined by the type <typeparamref name="T"/>.
      /// </returns>
      /// <exception cref="ArgumentException">.</exception>
      /// <exception cref="ArgumentNullException">.</exception>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
      [SecurityCritical]
      public static IEnumerable<T> EnumerateFileSystemEntryInfos<T>(string path, string searchPattern, DirectoryEnumerationOptions options, PathFormat pathFormat)
      {
         return EnumerateFileSystemEntryInfosInternal<T>(null, path, searchPattern, options, pathFormat);
      }


      /// <summary>
      /// [AlphaFS] Returns an enumerable collection of file system entries in a specified path.
      /// </summary>
      /// <typeparam name="T">
      ///    The type to return. This may be one of the following types:
      ///    <list type="definition">
      ///    <item>
      ///       <term>
      ///         <see cref="FileSystemInfo"/>
      ///       </term>
      ///       <description>
      ///         This method will return instances of <see cref="DirectoryInfo"/>, <see cref="FileInfo"/> and <see cref="FileSystemEntryInfo"/> instances.
      ///       </description>
      ///    </item>
      ///    <item>
      ///       <term>
      ///         <see cref="string"/>
      ///       </term>
      ///       <description>
      ///         This method will return the full path of each item.
      ///       </description>
      ///    </item>
      /// </list>
      /// </typeparam>
      /// <param name="path">The directory to search.</param>
      /// <returns>
      ///    The matching file system entries. The type of the items is determined by the type <typeparamref name="T"/>.
      /// </returns>
      /// <exception cref="ArgumentException"></exception>
      /// <exception cref="ArgumentNullException"></exception>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
      [SecurityCritical]
      public static IEnumerable<T> EnumerateFileSystemEntryInfos<T>(string path)
      {
         return EnumerateFileSystemEntryInfosInternal<T>(null, path, Path.WildcardStarMatchAll, DirectoryEnumerationOptions.FilesAndFolders, PathFormat.RelativePath);
      }

      /// <summary>
      /// [AlphaFS] Returns an enumerable collection of file system entries that match a <paramref name="searchPattern" /> in a specified path.
      /// </summary>
      /// <typeparam name="T">
      ///    The type to return. This may be one of the following types:
      ///    <list type="definition">
      ///    <item>
      ///       <term>
      ///         <see cref="FileSystemInfo"/>
      ///       </term>
      ///       <description>
      ///         This method will return instances of <see cref="DirectoryInfo"/>, <see cref="FileInfo"/> and <see cref="FileSystemEntryInfo"/> instances.
      ///       </description>
      ///    </item>
      ///    <item>
      ///       <term>
      ///         <see cref="string"/>
      ///       </term>
      ///       <description>
      ///         This method will return the full path of each item.
      ///       </description>
      ///    </item>
      /// </list>
      /// </typeparam>
      /// <param name="path">The directory to search.</param>
      /// <param name="searchPattern"><para>The search string to match against the names of directories in <paramref name="path" />. This parameter can contain a</para>
      /// <para>combination of valid literal path and wildcard (<see cref="Path.WildcardStarMatchAll" /> and <see cref="Path.WildcardQuestion" />)</para>
      /// <para>characters, but doesn't support regular expressions.</para></param>
      /// <returns>
      ///    The matching file system entries. The type of the items is determined by the type <typeparamref name="T"/>.
      /// </returns>
      /// <exception cref="ArgumentException"></exception>
      /// <exception cref="ArgumentNullException"></exception>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
      [SecurityCritical]
      public static IEnumerable<T> EnumerateFileSystemEntryInfos<T>(string path, string searchPattern)
      {
         return EnumerateFileSystemEntryInfosInternal<T>(null, path, searchPattern, DirectoryEnumerationOptions.FilesAndFolders, PathFormat.RelativePath);
      }

      /// <summary>
      ///   [AlphaFS] Returns an enumerable collection of file system entries that match a
      ///   <paramref name="searchPattern"/> in a specified path using
      ///   <see cref="DirectoryEnumerationOptions"/>.
      /// </summary>
      /// <typeparam name="T">
      ///   The type to return. This may be one of the following types:
      ///   <list type="definition">
      ///   <item>
      ///      <term>
      ///        <see cref="FileSystemInfo"/>
      ///      </term>
      ///      <description>
      ///        This method will return instances of <see cref="DirectoryInfo"/>,
      ///        <see cref="FileInfo"/> and <see cref="FileSystemEntryInfo"/> instances.
      ///      </description>
      ///   </item>
      ///   <item>
      ///      <term>
      ///        <see cref="string"/>
      ///      </term>
      ///      <description>
      ///        This method will return the full path of each item.
      ///      </description>
      ///   </item>
      ///   </list>
      /// </typeparam>
      /// <param name="path">The directory to search.</param>
      /// <param name="searchPattern">
      ///   The search string to match against the names of directories in
      ///   <paramref name="path"/>. This parameter can contain a
      ///   combination of valid literal path and wildcard (<see cref="Path.WildcardStarMatchAll"/> and
      ///   <see cref="Path.WildcardQuestion"/>)
      ///   characters, but doesn't support regular expressions.
      /// </param>
      /// <param name="options">
      ///   <see cref="DirectoryEnumerationOptions"/> flags that specify how the directory is to be
      ///   enumerated.
      /// </param>
      /// <returns>
      ///   The matching file system entries. The type of the items is determined by the type
      ///   <typeparamref name="T"/>.
      /// </returns>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
      [SecurityCritical]
      public static IEnumerable<T> EnumerateFileSystemEntryInfos<T>(string path, string searchPattern, DirectoryEnumerationOptions options)
      {
         return EnumerateFileSystemEntryInfosInternal<T>(null, path, searchPattern, options, PathFormat.RelativePath);
      }
      #endregion

      #region Transactional

      /// <summary>[AlphaFS] Returns an enumerable collection of file system entries in a specified path.</summary>
      /// <typeparam name="T">
      ///    The type to return. This may be one of the following types:
      ///    <list type="definition">
      ///    <item>
      ///       <term>
      ///         <see cref="FileSystemInfo"/>
      ///       </term>
      ///       <description>
      ///         This method will return instances of <see cref="DirectoryInfo"/>, <see cref="FileInfo"/> and <see cref="FileSystemEntryInfo"/> instances.
      ///       </description>
      ///    </item>
      ///    <item>
      ///       <term>
      ///         <see cref="string"/>
      ///       </term>
      ///       <description>
      ///         This method will return the full path of each item.
      ///       </description>
      ///    </item>
      /// </list>
      /// </typeparam>
      /// <returns>
      ///   The matching file system entries. The type of the items is determined by the type
      ///   <typeparamref name="T"/>.
      /// </returns>
      /// <exception cref="ArgumentException"></exception>
      /// <exception cref="ArgumentNullException"></exception>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory to search.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
      [SecurityCritical]
      public static IEnumerable<T> EnumerateFileSystemEntryInfos<T>(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         return EnumerateFileSystemEntryInfosInternal<T>(transaction, path, Path.WildcardStarMatchAll, DirectoryEnumerationOptions.FilesAndFolders, pathFormat);
      }

      /// <summary>[AlphaFS] Returns an enumerable collection of file system entries that match a <paramref name="searchPattern"/> in a specified path.
      /// </summary>
      /// <typeparam name="T">
      ///    The type to return. This may be one of the following types:
      ///    <list type="definition">
      ///    <item>
      ///       <term>
      ///         <see cref="FileSystemInfo"/>
      ///       </term>
      ///       <description>
      ///         This method will return instances of <see cref="DirectoryInfo"/>, <see cref="FileInfo"/> and <see cref="FileSystemEntryInfo"/> instances.
      ///       </description>
      ///    </item>
      ///    <item>
      ///       <term>
      ///         <see cref="string"/>
      ///       </term>
      ///       <description>
      ///         This method will return the full path of each item.
      ///       </description>
      ///    </item>
      /// </list>
      /// </typeparam>
      /// <returns>
      ///   The matching file system entries. The type of the items is determined by the type
      ///   <typeparamref name="T"/>.
      /// </returns>
      /// <exception cref="ArgumentException"></exception>
      /// <exception cref="ArgumentNullException"></exception>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory to search.</param>
      /// <param name="searchPattern">
      /// <para>The search string to match against the names of directories in <paramref name="path"/>. This parameter can contain a</para>
      /// <para>combination of valid literal path and wildcard (<see cref="Path.WildcardStarMatchAll"/> and <see cref="Path.WildcardQuestion"/>)</para>
      /// <para>characters, but does not support regular expressions.</para>
      /// </param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
      [SecurityCritical]
      public static IEnumerable<T> EnumerateFileSystemEntryInfos<T>(KernelTransaction transaction, string path, string searchPattern, PathFormat pathFormat)
      {
         return EnumerateFileSystemEntryInfosInternal<T>(transaction, path, searchPattern, DirectoryEnumerationOptions.FilesAndFolders, pathFormat);
      }

      /// <summary>[AlphaFS] Returns an enumerable collection of file system entries that match a <paramref name="searchPattern"/> in a specified path using <see cref="DirectoryEnumerationOptions"/>.</summary>
      /// <typeparam name="T">
      ///    The type to return. This may be one of the following types:
      ///    <list type="definition">
      ///    <item>
      ///       <term>
      ///         <see cref="FileSystemInfo"/>
      ///       </term>
      ///       <description>
      ///         This method will return instances of <see cref="DirectoryInfo"/>, <see cref="FileInfo"/> and <see cref="FileSystemEntryInfo"/> instances.
      ///       </description>
      ///    </item>
      ///    <item>
      ///       <term>
      ///         <see cref="string"/>
      ///       </term>
      ///       <description>
      ///         This method will return the full path of each item.
      ///       </description>
      ///    </item>
      /// </list>
      /// </typeparam>
      /// <returns>
      ///   The matching file system entries. The type of the items is determined by the type
      ///   <typeparamref name="T"/>.
      /// </returns>
      /// <exception cref="ArgumentException"></exception>
      /// <exception cref="ArgumentNullException"></exception>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory to search.</param>
      /// <param name="searchPattern">
      /// <para>The search string to match against the names of directories in <paramref name="path"/>. This parameter can contain a</para>
      /// <para>combination of valid literal path and wildcard (<see cref="Path.WildcardStarMatchAll"/> and <see cref="Path.WildcardQuestion"/>)</para>
      /// <para>characters, but does not support regular expressions.</para>
      /// </param>
      /// <param name="options"><see cref="DirectoryEnumerationOptions"/> flags that specify how the directory is to be enumerated.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
      [SecurityCritical]
      public static IEnumerable<T> EnumerateFileSystemEntryInfos<T>(KernelTransaction transaction, string path, string searchPattern, DirectoryEnumerationOptions options, PathFormat pathFormat)
      {
         return EnumerateFileSystemEntryInfosInternal<T>(transaction, path, searchPattern, options, pathFormat);
      }


      /// <summary>[AlphaFS] Returns an enumerable collection of file system entries in a specified path.</summary>
      /// <typeparam name="T">
      ///    The type to return. This may be one of the following types:
      ///    <list type="definition">
      ///    <item>
      ///       <term>
      ///         <see cref="FileSystemInfo"/>
      ///       </term>
      ///       <description>
      ///         This method will return instances of <see cref="DirectoryInfo"/>, <see cref="FileInfo"/> and <see cref="FileSystemEntryInfo"/> instances.
      ///       </description>
      ///    </item>
      ///    <item>
      ///       <term>
      ///         <see cref="string"/>
      ///       </term>
      ///       <description>
      ///         This method will return the full path of each item.
      ///       </description>
      ///    </item>
      /// </list>
      /// </typeparam>
      /// <returns>
      ///   The matching file system entries. The type of the items is determined by the type
      ///   <typeparamref name="T"/>.
      /// </returns>
      /// <exception cref="ArgumentException"></exception>
      /// <exception cref="ArgumentNullException"></exception>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory to search.</param>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
      [SecurityCritical]
      public static IEnumerable<T> EnumerateFileSystemEntryInfos<T>(KernelTransaction transaction, string path)
      {
         return EnumerateFileSystemEntryInfosInternal<T>(transaction, path, Path.WildcardStarMatchAll, DirectoryEnumerationOptions.FilesAndFolders, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Returns an enumerable collection of file system entries that match a <paramref name="searchPattern"/> in a specified path.</summary>
      /// <typeparam name="T">
      ///    The type to return. This may be one of the following types:
      ///    <list type="definition">
      ///    <item>
      ///       <term>
      ///         <see cref="FileSystemInfo"/>
      ///       </term>
      ///       <description>
      ///         This method will return instances of <see cref="DirectoryInfo"/>, <see cref="FileInfo"/> and <see cref="FileSystemEntryInfo"/> instances.
      ///       </description>
      ///    </item>
      ///    <item>
      ///       <term>
      ///         <see cref="string"/>
      ///       </term>
      ///       <description>
      ///         This method will return the full path of each item.
      ///       </description>
      ///    </item>
      /// </list>
      /// </typeparam>
      /// <returns>
      ///   The matching file system entries. The type of the items is determined by the type
      ///   <typeparamref name="T"/>.
      /// </returns>
      /// <exception cref="ArgumentException"></exception>
      /// <exception cref="ArgumentNullException"></exception>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory to search.</param>
      /// <param name="searchPattern">
      /// <para>The search string to match against the names of directories in <paramref name="path"/>. This parameter can contain a</para>
      /// <para>combination of valid literal path and wildcard (<see cref="Path.WildcardStarMatchAll"/> and <see cref="Path.WildcardQuestion"/>)</para>
      /// <para>characters, but does not support regular expressions.</para>
      /// </param>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
      [SecurityCritical]
      public static IEnumerable<T> EnumerateFileSystemEntryInfos<T>(KernelTransaction transaction, string path, string searchPattern)
      {
         return EnumerateFileSystemEntryInfosInternal<T>(transaction, path, searchPattern, DirectoryEnumerationOptions.FilesAndFolders, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Returns an enumerable collection of file system entries that match a <paramref name="searchPattern"/> in a specified path using <see cref="DirectoryEnumerationOptions"/>.</summary>
      /// <typeparam name="T">
      ///    The type to return. This may be one of the following types:
      ///    <list type="definition">
      ///    <item>
      ///       <term>
      ///         <see cref="FileSystemInfo"/>
      ///       </term>
      ///       <description>
      ///         This method will return instances of <see cref="DirectoryInfo"/>, <see cref="FileInfo"/> and <see cref="FileSystemEntryInfo"/> instances.
      ///       </description>
      ///    </item>
      ///    <item>
      ///       <term>
      ///         <see cref="string"/>
      ///       </term>
      ///       <description>
      ///         This method will return the full path of each item.
      ///       </description>
      ///    </item>
      /// </list>
      /// </typeparam>
      /// <returns>
      ///   The matching file system entries. The type of the items is determined by the type
      ///   <typeparamref name="T"/>.
      /// </returns>
      /// <exception cref="ArgumentException"></exception>
      /// <exception cref="ArgumentNullException"></exception>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory to search.</param>
      /// <param name="searchPattern">
      /// <para>The search string to match against the names of directories in <paramref name="path"/>. This parameter can contain a</para>
      /// <para>combination of valid literal path and wildcard (<see cref="Path.WildcardStarMatchAll"/> and <see cref="Path.WildcardQuestion"/>)</para>
      /// <para>characters, but does not support regular expressions.</para>
      /// </param>
      /// <param name="options"><see cref="DirectoryEnumerationOptions"/> flags that specify how the directory is to be enumerated.</param>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
      [SecurityCritical]
      public static IEnumerable<T> EnumerateFileSystemEntryInfos<T>(KernelTransaction transaction, string path, string searchPattern, DirectoryEnumerationOptions options)
      {
         return EnumerateFileSystemEntryInfosInternal<T>(transaction, path, searchPattern, options, PathFormat.RelativePath);
      }

      #endregion // Transactional

      #endregion // EnumerateFileSystemEntryInfos

      #region Internal Methods

      /// <summary>[AlphaFS] Unified method EnumerateFileSystemEntryInfosInternal() to return an enumerable collection of file system entries in a specified path using <see cref="DirectoryEnumerationOptions"/>.</summary>
      /// <returns>
      /// <para>The return type is based on C# inference. Possible return types are:</para>
      /// <para> <see cref="string"/>- (full path), <see cref="FileSystemInfo"/>- (<see cref="DirectoryInfo"/> / <see cref="FileInfo"/> and <see cref="FileSystemEntryInfo"/> instance.</para>
      /// </returns>
      /// <exception cref="ArgumentException"></exception>
      /// <exception cref="ArgumentNullException"></exception>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory to search.</param>
      /// <param name="searchPattern">
      /// <para>The search string to match against the names of directories in <paramref name="path"/>. This parameter can contain a</para>
      /// <para>combination of valid literal path and wildcard (<see cref="Path.WildcardStarMatchAll"/> and <see cref="Path.WildcardQuestion"/>)</para>
      /// <para>characters, but does not support regular expressions.</para>
      /// </param>
      /// <param name="options"><see cref="DirectoryEnumerationOptions"/> flags that specify how the directory is to be enumerated.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      internal static IEnumerable<T> EnumerateFileSystemEntryInfosInternal<T>(KernelTransaction transaction, string path, string searchPattern, DirectoryEnumerationOptions options, PathFormat pathFormat)
      {
         // Enable BasicSearch and LargeCache by default.
         options &= DirectoryEnumerationOptions.BasicSearch | DirectoryEnumerationOptions.LargeCache;

         return (new FindFileSystemEntryInfo(true, transaction, path, searchPattern, options, typeof(T), pathFormat)).Enumerate<T>();
      }

      #endregion // EnumerateFileSystemEntryInfosInternal
   }
}