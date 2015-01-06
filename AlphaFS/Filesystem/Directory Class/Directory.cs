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
using System.Security.Principal;
using SearchOption = System.IO.SearchOption;

namespace Alphaleonis.Win32.Filesystem
{
   /// <summary>Exposes static methods for creating, moving, and enumerating through directories and subdirectories.
   /// <para>This class cannot be inherited.</para>
   /// </summary>
   public static partial class Directory
   {
      #region AlphaFS

      #region AddStream

      #region IsFullPath

      /// <summary>[AlphaFS] Adds an alternate data stream (NTFS ADS) to an existing directory.</summary>
      /// <param name="path">The path to an existing directory.</param>
      /// <param name="name">The name for the stream. If a stream with <paramref name="name"/> already exists, it will be overwritten.</param>
      /// <param name="contents">The lines to add to the stream.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void AddStream(string path, string name, string[] contents, PathFormat pathFormat)
      {
         AlternateDataStreamInfo.AddStreamInternal(true, null, path, name, contents, pathFormat);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Adds an alternate data stream (NTFS ADS) to an existing directory.</summary>
      /// <param name="path">The path to an existing directory.</param>
      /// <param name="name">The name for the stream. If a stream with <paramref name="name"/> already exists, it will be overwritten.</param>
      /// <param name="contents">The lines to add to the stream.</param>
      [SecurityCritical]
      public static void AddStream(string path, string name, string[] contents)
      {
         AlternateDataStreamInfo.AddStreamInternal(true, null, path, name, contents, PathFormat.Relative);
      }

      #region Transacted

      #region IsFullPath

      /// <summary>[AlphaFS] Adds an alternate data stream (NTFS ADS) to an existing directory.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to an existing directory.</param>
      /// <param name="name">The name for the stream. If a stream with <paramref name="name"/> already exists, it will be overwritten.</param>
      /// <param name="contents">The lines to add to the stream.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void AddStream(KernelTransaction transaction, string path, string name, string[] contents, PathFormat pathFormat)
      {
         AlternateDataStreamInfo.AddStreamInternal(true, transaction, path, name, contents, pathFormat);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Adds an alternate data stream (NTFS ADS) to an existing directory.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to an existing directory.</param>
      /// <param name="name">The name for the stream. If a stream with <paramref name="name"/> already exists, it will be overwritten.</param>
      /// <param name="contents">The lines to add to the stream.</param>
      [SecurityCritical]
      public static void AddStream(KernelTransaction transaction, string path, string name, string[] contents)
      {
         AlternateDataStreamInfo.AddStreamInternal(true, transaction, path, name, contents, PathFormat.Relative);
      }

      #endregion Transacted

      #endregion // AddStream

      #region CountFileSystemObjects

      #region IsFullPath

      /// <summary>[AlphaFS] Counts file system objects: files, folders or both) in a given directory.</summary>
      /// <param name="path">The directory path.</param>
      /// <param name="directoryEnumerationOptions"><see cref="DirectoryEnumerationOptions"/> flags that specify how the directory is to be enumerated.</param>
      /// <returns>The counted number of file system objects.</returns>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <exception cref="System.UnauthorizedAccessException">An exception is thrown case of access errors.</exception>
      [SecurityCritical]
      public static long CountFileSystemObjects(string path, DirectoryEnumerationOptions directoryEnumerationOptions, PathFormat pathFormat)
      {
         return EnumerateFileSystemEntryInfosInternal<string>(null, path, Path.WildcardStarMatchAll, directoryEnumerationOptions, pathFormat).Count();
      }

      /// <summary>[AlphaFS] Counts file system objects: files, folders or both) in a given directory.</summary>
      /// <param name="path">The directory path.</param>
      /// <param name="searchPattern">
      /// <para>The search string to match against the names of directories in path. This parameter can contain a</para>
      /// <para>combination of valid literal path and wildcard (<see cref="Path.WildcardStarMatchAll"/> and <see cref="Path.WildcardQuestion"/>)</para>
      /// <para>characters, but does not support regular expressions.</para>
      /// </param>
      /// <param name="directoryEnumerationOptions"><see cref="DirectoryEnumerationOptions"/> flags that specify how the directory is to be enumerated.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>The counted number of file system objects.</returns>
      /// <exception cref="System.UnauthorizedAccessException">An exception is thrown case of access errors.</exception>
      [SecurityCritical]
      public static long CountFileSystemObjects(string path, string searchPattern, DirectoryEnumerationOptions directoryEnumerationOptions, PathFormat pathFormat)
      {
         return EnumerateFileSystemEntryInfosInternal<string>(null, path, searchPattern, directoryEnumerationOptions, pathFormat).Count();
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Counts file system objects: files, folders or both) in a given directory.</summary>
      /// <param name="path">The directory path.</param>
      /// <param name="directoryEnumerationOptions"><see cref="DirectoryEnumerationOptions"/> flags that specify how the directory is to be enumerated.</param>
      /// <returns>The counted number of file system objects.</returns>
      /// <exception cref="System.UnauthorizedAccessException">An exception is thrown case of access errors.</exception>
      [SecurityCritical]
      public static long CountFileSystemObjects(string path, DirectoryEnumerationOptions directoryEnumerationOptions)
      {
         return EnumerateFileSystemEntryInfosInternal<string>(null, path, Path.WildcardStarMatchAll, directoryEnumerationOptions, PathFormat.Relative).Count();
      }

      /// <summary>[AlphaFS] Counts file system objects: files, folders or both) in a given directory.</summary>
      /// <param name="path">The directory path.</param>
      /// <param name="searchPattern">
      /// <para>The search string to match against the names of directories in path. This parameter can contain a</para>
      /// <para>combination of valid literal path and wildcard (<see cref="Path.WildcardStarMatchAll"/> and <see cref="Path.WildcardQuestion"/>)</para>
      /// <para>characters, but does not support regular expressions.</para>
      /// </param>
      /// <param name="directoryEnumerationOptions"><see cref="DirectoryEnumerationOptions"/> flags that specify how the directory is to be enumerated.</param>
      /// <returns>The counted number of file system objects.</returns>
      /// <exception cref="System.UnauthorizedAccessException">An exception is thrown case of access errors.</exception>
      [SecurityCritical]
      public static long CountFileSystemObjects(string path, string searchPattern, DirectoryEnumerationOptions directoryEnumerationOptions)
      {
         return EnumerateFileSystemEntryInfosInternal<string>(null, path, searchPattern, directoryEnumerationOptions, PathFormat.Relative).Count();
      }

      #region Transacted

      #region IsFullPath

      /// <summary>[AlphaFS] Counts file system objects: files, folders or both) in a given directory.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory path.</param>
      /// <param name="directoryEnumerationOptions"><see cref="DirectoryEnumerationOptions"/> flags that specify how the directory is to be enumerated.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>The counted number of file system objects.</returns>
      /// <exception cref="System.UnauthorizedAccessException">An exception is thrown case of access errors.</exception>
      [SecurityCritical]
      public static long CountFileSystemObjects(KernelTransaction transaction, string path, DirectoryEnumerationOptions directoryEnumerationOptions, PathFormat pathFormat)
      {
         return EnumerateFileSystemEntryInfosInternal<string>(transaction, path, Path.WildcardStarMatchAll, directoryEnumerationOptions, pathFormat).Count();
      }

      /// <summary>[AlphaFS] Counts file system objects: files, folders or both) in a given directory.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory path.</param>
      /// <param name="searchPattern">
      /// <para>The search string to match against the names of directories in path. This parameter can contain a</para>
      /// <para>combination of valid literal path and wildcard (<see cref="Path.WildcardStarMatchAll"/> and <see cref="Path.WildcardQuestion"/>)</para>
      /// <para>characters, but does not support regular expressions.</para>
      /// </param>
      /// <param name="directoryEnumerationOptions"><see cref="DirectoryEnumerationOptions"/> flags that specify how the directory is to be enumerated.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>The counted number of file system objects.</returns>
      /// <exception cref="System.UnauthorizedAccessException">An exception is thrown case of access errors.</exception>
      [SecurityCritical]
      public static long CountFileSystemObjects(KernelTransaction transaction, string path, string searchPattern, DirectoryEnumerationOptions directoryEnumerationOptions, PathFormat pathFormat)
      {
         return EnumerateFileSystemEntryInfosInternal<string>(transaction, path, searchPattern, directoryEnumerationOptions, pathFormat).Count();
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Counts file system objects: files, folders or both) in a given directory.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory path.</param>
      /// <param name="directoryEnumerationOptions"><see cref="DirectoryEnumerationOptions"/> flags that specify how the directory is to be enumerated.</param>
      /// <returns>The counted number of file system objects.</returns>
      /// <exception cref="System.UnauthorizedAccessException">An exception is thrown case of access errors.</exception>
      [SecurityCritical]
      public static long CountFileSystemObjects(KernelTransaction transaction, string path, DirectoryEnumerationOptions directoryEnumerationOptions)
      {
         return EnumerateFileSystemEntryInfosInternal<string>(transaction, path, Path.WildcardStarMatchAll, directoryEnumerationOptions, PathFormat.Relative).Count();
      }

      /// <summary>[AlphaFS] Counts file system objects: files, folders or both) in a given directory.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory path.</param>
      /// <param name="searchPattern">
      /// <para>The search string to match against the names of directories in path. This parameter can contain a</para>
      /// <para>combination of valid literal path and wildcard (<see cref="Path.WildcardStarMatchAll"/> and <see cref="Path.WildcardQuestion"/>)</para>
      /// <para>characters, but does not support regular expressions.</para>
      /// </param>
      /// <param name="directoryEnumerationOptions"><see cref="DirectoryEnumerationOptions"/> flags that specify how the directory is to be enumerated.</param>
      /// <returns>The counted number of file system objects.</returns>
      /// <exception cref="System.UnauthorizedAccessException">An exception is thrown case of access errors.</exception>
      [SecurityCritical]
      public static long CountFileSystemObjects(KernelTransaction transaction, string path, string searchPattern, DirectoryEnumerationOptions directoryEnumerationOptions)
      {
         return EnumerateFileSystemEntryInfosInternal<string>(transaction, path, searchPattern, directoryEnumerationOptions, PathFormat.Relative).Count();
      }

      #endregion // Transacted

      #endregion // CountFileSystemObjects

      



      #region DeleteEmpty

      #region IsFullPath

      /// <summary>[AlphaFS] Deletes empty subdirectores from the specified directory.</summary>
      /// <param name="path">The name of the directory to remove empty subdirectories from.</param>
      /// <param name="recursive"><see langword="true"/> deletes empty subdirectories from this directory and its subdirectories.</param>
      /// <param name="ignoreReadOnly"><see langword="true"/> overrides read only <see cref="FileAttributes"/> of empty directories.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void DeleteEmpty(string path, bool recursive, bool ignoreReadOnly, PathFormat pathFormat)
      {
         DeleteEmptyDirectoryInternal(null, null, path, recursive, ignoreReadOnly, true, pathFormat);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Deletes empty subdirectores from the specified directory.</summary>
      /// <param name="path">The name of the directory to remove empty subdirectories from.</param>
      /// <param name="recursive"><see langword="true"/> deletes empty subdirectories from this directory and its subdirectories.</param>
      [SecurityCritical]
      public static void DeleteEmpty(string path, bool recursive)
      {
         DeleteEmptyDirectoryInternal(null, null, path, recursive, false, true, PathFormat.Relative);
      }

      /// <summary>[AlphaFS] Deletes empty subdirectores from the specified directory.</summary>
      /// <param name="path">The name of the directory to remove empty subdirectories from.</param>
      /// <param name="recursive"><see langword="true"/> deletes empty subdirectories from this directory and its subdirectories.</param>
      /// <param name="ignoreReadOnly"><see langword="true"/> overrides read only <see cref="FileAttributes"/> of empty directories.</param>
      [SecurityCritical]
      public static void DeleteEmpty(string path, bool recursive, bool ignoreReadOnly)
      {
         DeleteEmptyDirectoryInternal(null, null, path, recursive, ignoreReadOnly, true, PathFormat.Relative);
      }

      #region Transacted

      #region IsFullPath

      /// <summary>[AlphaFS] Deletes empty subdirectores from the specified directory.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The name of the directory to remove empty subdirectories from.</param>
      /// <param name="recursive"><see langword="true"/> deletes empty subdirectories from this directory and its subdirectories.</param>
      /// <param name="ignoreReadOnly"><see langword="true"/> overrides read only <see cref="FileAttributes"/> of empty directories.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void DeleteEmpty(KernelTransaction transaction, string path, bool recursive, bool ignoreReadOnly, PathFormat pathFormat)
      {
         DeleteEmptyDirectoryInternal(null, transaction, path, recursive, ignoreReadOnly, true, pathFormat);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Deletes empty subdirectores from the specified directory.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The name of the directory to remove empty subdirectories from.</param>
      /// <param name="recursive"><see langword="true"/> deletes empty subdirectories from this directory and its subdirectories.</param>
      [SecurityCritical]
      public static void DeleteEmpty(KernelTransaction transaction, string path, bool recursive)
      {
         DeleteEmptyDirectoryInternal(null, transaction, path, recursive, false, true, PathFormat.Relative);
      }

      /// <summary>[AlphaFS] Deletes empty subdirectores from the specified directory.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The name of the directory to remove empty subdirectories from.</param>
      /// <param name="recursive"><see langword="true"/> deletes empty subdirectories from this directory and its subdirectories.</param>
      /// <param name="ignoreReadOnly"><see langword="true"/> overrides read only <see cref="FileAttributes"/> of empty directories.</param>
      [SecurityCritical]
      public static void DeleteEmpty(KernelTransaction transaction, string path, bool recursive, bool ignoreReadOnly)
      {
         DeleteEmptyDirectoryInternal(null, transaction, path, recursive, ignoreReadOnly, true, PathFormat.Relative);
      }

      #endregion // Transacted

      #endregion // DeleteEmpty

      #region EnumerateFileIdBothDirectoryInfo

      #region IsFullPath

      /// <summary>[AlphaFS] Retrieves information about files in the directory specified by <paramref name="path"/> in <see cref="FileShare.ReadWrite"/> mode.</summary>
      /// <param name="path">A path to a directory from which to retrieve information.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>An enumeration of <see cref="FileIdBothDirectoryInfo"/> records for each file system entry in the specified diretory.</returns>
      [SecurityCritical]
      public static IEnumerable<FileIdBothDirectoryInfo> EnumerateFileIdBothDirectoryInfo(string path, PathFormat pathFormat)
      {
         return EnumerateFileIdBothDirectoryInfoInternal(null, null, path, FileShare.ReadWrite, false, pathFormat);
      }

      /// <summary>[AlphaFS] Retrieves information about files in the directory specified by <paramref name="path"/> in specified <see cref="FileShare"/> mode.</summary>
      /// <param name="path">A path to a directory from which to retrieve information.</param>
      /// <param name="shareMode">The <see cref="FileShare"/> mode with which to open a handle to the directory.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>An enumeration of <see cref="FileIdBothDirectoryInfo"/> records for each file system entry in the specified diretory.</returns>      
      [SecurityCritical]
      public static IEnumerable<FileIdBothDirectoryInfo> EnumerateFileIdBothDirectoryInfo(string path, FileShare shareMode, PathFormat pathFormat)
      {
         return EnumerateFileIdBothDirectoryInfoInternal(null, null, path, shareMode, false, pathFormat);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Retrieves information about files in the directory specified by <paramref name="path"/> in <see cref="FileShare.ReadWrite"/> mode.</summary>
      /// <param name="path">A path to a directory from which to retrieve information.</param>
      /// <returns>An enumeration of <see cref="FileIdBothDirectoryInfo"/> records for each file system entry in the specified diretory.</returns>
      [SecurityCritical]
      public static IEnumerable<FileIdBothDirectoryInfo> EnumerateFileIdBothDirectoryInfo(string path)
      {
         return EnumerateFileIdBothDirectoryInfoInternal(null, null, path, FileShare.ReadWrite, false, PathFormat.Relative);
      }

      /// <summary>[AlphaFS] Retrieves information about files in the directory specified by <paramref name="path"/> in specified <see cref="FileShare"/> mode.</summary>
      /// <param name="path">A path to a directory from which to retrieve information.</param>
      /// <param name="shareMode">The <see cref="FileShare"/> mode with which to open a handle to the directory.</param>
      /// <returns>An enumeration of <see cref="FileIdBothDirectoryInfo"/> records for each file system entry in the specified diretory.</returns>      
      [SecurityCritical]
      public static IEnumerable<FileIdBothDirectoryInfo> EnumerateFileIdBothDirectoryInfo(string path, FileShare shareMode)
      {
         return EnumerateFileIdBothDirectoryInfoInternal(null, null, path, shareMode, false, PathFormat.Relative);
      }

      /// <summary>[AlphaFS] Retrieves information about files in the directory handle specified.</summary>
      /// <param name="handle">An open handle to the directory from which to retrieve information.</param>
      /// <returns>An IEnumerable of <see cref="FileIdBothDirectoryInfo"/> records for each file system entry in the specified diretory.</returns>    
      [SecurityCritical]
      public static IEnumerable<FileIdBothDirectoryInfo> EnumerateFileIdBothDirectoryInfo(SafeFileHandle handle)
      {
         // FileShare has no effect since a handle is already opened.
         return EnumerateFileIdBothDirectoryInfoInternal(null, handle, null, FileShare.ReadWrite, false, PathFormat.Relative);
      }

      #region Transacted

      #region IsFullPath

      /// <summary>[AlphaFS] Retrieves information about files in the directory specified by <paramref name="path"/> in <see cref="FileShare.ReadWrite"/> mode.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">A path to a directory from which to retrieve information.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>An enumeration of <see cref="FileIdBothDirectoryInfo"/> records for each file system entry in the specified diretory.</returns>
      [SecurityCritical]
      public static IEnumerable<FileIdBothDirectoryInfo> EnumerateFileIdBothDirectoryInfo(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         return EnumerateFileIdBothDirectoryInfoInternal(transaction, null, path, FileShare.ReadWrite, false, pathFormat);
      }

      /// <summary>[AlphaFS] Retrieves information about files in the directory specified by <paramref name="path"/> in specified <see cref="FileShare"/> mode.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">A path to a directory from which to retrieve information.</param>
      /// <param name="shareMode">The <see cref="FileShare"/> mode with which to open a handle to the directory.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>An enumeration of <see cref="FileIdBothDirectoryInfo"/> records for each file system entry in the specified diretory.</returns>
      [SecurityCritical]
      public static IEnumerable<FileIdBothDirectoryInfo> EnumerateFileIdBothDirectoryInfo(KernelTransaction transaction, string path, FileShare shareMode, PathFormat pathFormat)
      {
         return EnumerateFileIdBothDirectoryInfoInternal(transaction, null, path, shareMode, false, pathFormat);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Retrieves information about files in the directory specified by <paramref name="path"/> in <see cref="FileShare.ReadWrite"/> mode.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">A path to a directory from which to retrieve information.</param>
      /// <returns>An enumeration of <see cref="FileIdBothDirectoryInfo"/> records for each file system entry in the specified diretory.</returns>
      [SecurityCritical]
      public static IEnumerable<FileIdBothDirectoryInfo> EnumerateFileIdBothDirectoryInfo(KernelTransaction transaction, string path)
      {
         return EnumerateFileIdBothDirectoryInfoInternal(transaction, null, path, FileShare.ReadWrite, false, PathFormat.Relative);
      }

      /// <summary>[AlphaFS] Retrieves information about files in the directory specified by <paramref name="path"/> in specified <see cref="FileShare"/> mode.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">A path to a directory from which to retrieve information.</param>
      /// <param name="shareMode">The <see cref="FileShare"/> mode with which to open a handle to the directory.</param>
      /// <returns>An enumeration of <see cref="FileIdBothDirectoryInfo"/> records for each file system entry in the specified diretory.</returns>
      [SecurityCritical]
      public static IEnumerable<FileIdBothDirectoryInfo> EnumerateFileIdBothDirectoryInfo(KernelTransaction transaction, string path, FileShare shareMode)
      {
         return EnumerateFileIdBothDirectoryInfoInternal(transaction, null, path, shareMode, false, PathFormat.Relative);
      }

      #endregion // Transacted

      #endregion // EnumerateFileIdBothDirectoryInfo

      #region EnumerateFileSystemEntryInfos

      #region IsFullPath

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
      /// <param name="directoryEnumerationOptions">
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
      public static IEnumerable<T> EnumerateFileSystemEntryInfos<T>(string path, string searchPattern, DirectoryEnumerationOptions directoryEnumerationOptions, PathFormat pathFormat)
      {
         return EnumerateFileSystemEntryInfosInternal<T>(null, path, searchPattern, directoryEnumerationOptions, pathFormat);
      }

      #endregion // IsFullPath

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
         return EnumerateFileSystemEntryInfosInternal<T>(null, path, Path.WildcardStarMatchAll, DirectoryEnumerationOptions.FilesAndFolders, PathFormat.Relative);
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
         return EnumerateFileSystemEntryInfosInternal<T>(null, path, searchPattern, DirectoryEnumerationOptions.FilesAndFolders, PathFormat.Relative);
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
      /// <param name="directoryEnumerationOptions">
      ///   <see cref="DirectoryEnumerationOptions"/> flags that specify how the directory is to be
      ///   enumerated.
      /// </param>
      /// <returns>
      ///   The matching file system entries. The type of the items is determined by the type
      ///   <typeparamref name="T"/>.
      /// </returns>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
      [SecurityCritical]
      public static IEnumerable<T> EnumerateFileSystemEntryInfos<T>(string path, string searchPattern, DirectoryEnumerationOptions directoryEnumerationOptions)
      {
         return EnumerateFileSystemEntryInfosInternal<T>(null, path, searchPattern, directoryEnumerationOptions, PathFormat.Relative);
      }

      #region Transacted

      #region IsFullPath

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
      /// <param name="directoryEnumerationOptions"><see cref="DirectoryEnumerationOptions"/> flags that specify how the directory is to be enumerated.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
      [SecurityCritical]
      public static IEnumerable<T> EnumerateFileSystemEntryInfos<T>(KernelTransaction transaction, string path, string searchPattern, DirectoryEnumerationOptions directoryEnumerationOptions, PathFormat pathFormat)
      {
         return EnumerateFileSystemEntryInfosInternal<T>(transaction, path, searchPattern, directoryEnumerationOptions, pathFormat);
      }

      #endregion // IsFullPath

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
         return EnumerateFileSystemEntryInfosInternal<T>(transaction, path, Path.WildcardStarMatchAll, DirectoryEnumerationOptions.FilesAndFolders, PathFormat.Relative);
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
         return EnumerateFileSystemEntryInfosInternal<T>(transaction, path, searchPattern, DirectoryEnumerationOptions.FilesAndFolders, PathFormat.Relative);
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
      /// <param name="directoryEnumerationOptions"><see cref="DirectoryEnumerationOptions"/> flags that specify how the directory is to be enumerated.</param>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
      [SecurityCritical]
      public static IEnumerable<T> EnumerateFileSystemEntryInfos<T>(KernelTransaction transaction, string path, string searchPattern, DirectoryEnumerationOptions directoryEnumerationOptions)
      {
         return EnumerateFileSystemEntryInfosInternal<T>(transaction, path, searchPattern, directoryEnumerationOptions, PathFormat.Relative);
      }

      #endregion // Transacted

      #endregion // EnumerateFileSystemEntryInfos

      #region EnumerateStreams

      #region IsFullPath

      /// <summary>[AlphaFS] Returns an enumerable collection of <see cref="AlternateDataStreamInfo"/> instances for the directory specified by <paramref name="path"/>.</summary>
      /// <param name="path">The file to search.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>An enumerable collection of <see cref="AlternateDataStreamInfo"/> instances for the directory specified by <paramref name="path"/>.</returns>
      [SecurityCritical]
      public static IEnumerable<AlternateDataStreamInfo> EnumerateStreams(string path, PathFormat pathFormat)
      {
         return AlternateDataStreamInfo.EnumerateStreamsInternal(null, null, null, path, null, null, pathFormat);
      }

      /// <summary>[AlphaFS] Returns an enumerable collection of <see cref="AlternateDataStreamInfo"/> of type <see cref="StreamType"/> instances for the directory specified by <paramref name="path"/>.</summary>
      /// <param name="path">The file to search.</param>
      /// <param name="streamType">The type of stream to retrieve.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>An enumerable collection of <see cref="AlternateDataStreamInfo"/> of type <see cref="StreamType"/> instances for the directory specified by <paramref name="path"/>.</returns>
      [SecurityCritical]
      public static IEnumerable<AlternateDataStreamInfo> EnumerateStreams(string path, StreamType streamType, PathFormat pathFormat)
      {
         return AlternateDataStreamInfo.EnumerateStreamsInternal(null, null, null, path, null, streamType, pathFormat);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Returns an enumerable collection of <see cref="AlternateDataStreamInfo"/> instances for the directory specified by <paramref name="path"/>.</summary>
      /// <param name="path">The file to search.</param>
      /// <returns>An enumerable collection of <see cref="AlternateDataStreamInfo"/> instances for the directory specified by <paramref name="path"/>.</returns>
      [SecurityCritical]
      public static IEnumerable<AlternateDataStreamInfo> EnumerateStreams(string path)
      {
         return AlternateDataStreamInfo.EnumerateStreamsInternal(null, null, null, path, null, null, PathFormat.Relative);
      }

      /// <summary>[AlphaFS] Returns an enumerable collection of <see cref="AlternateDataStreamInfo"/> of type <see cref="StreamType"/> instances for the directory specified by <paramref name="path"/>.</summary>
      /// <param name="path">The file to search.</param>
      /// <param name="streamType">The type of stream to retrieve.</param>
      /// <returns>An enumerable collection of <see cref="AlternateDataStreamInfo"/> of type <see cref="StreamType"/> instances for the directory specified by <paramref name="path"/>.</returns>
      [SecurityCritical]
      public static IEnumerable<AlternateDataStreamInfo> EnumerateStreams(string path, StreamType streamType)
      {
         return AlternateDataStreamInfo.EnumerateStreamsInternal(null, null, null, path, null, streamType, PathFormat.Relative);
      }

      /// <summary>[AlphaFS] Returns an enumerable collection of <see cref="AlternateDataStreamInfo"/> instances for the handle specified by <paramref name="handle"/>.</summary>
      /// <param name="handle">A <see cref="SafeFileHandle"/> connected to the file from which to retrieve the information.</param>
      /// <returns>An enumerable collection of <see cref="AlternateDataStreamInfo"/> instances for the handle specified by <paramref name="handle"/>.</returns>
      [SecurityCritical]
      public static IEnumerable<AlternateDataStreamInfo> EnumerateStreams(SafeFileHandle handle)
      {
         return AlternateDataStreamInfo.EnumerateStreamsInternal(null, null, handle, null, null, null, PathFormat.LongFullPath);
      }

      #region Transacted

      #region IsFullPath

      /// <summary>[AlphaFS] Returns an enumerable collection of <see cref="AlternateDataStreamInfo"/> instances for the directory specified by <paramref name="path"/>.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to search.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>An enumerable collection of <see cref="AlternateDataStreamInfo"/> instances for the directory specified by <paramref name="path"/>.</returns>
      [SecurityCritical]
      public static IEnumerable<AlternateDataStreamInfo> EnumerateStreams(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         return AlternateDataStreamInfo.EnumerateStreamsInternal(null, transaction, null, path, null, null, pathFormat);
      }

      /// <summary>[AlphaFS] Returns an enumerable collection of <see cref="AlternateDataStreamInfo"/> of type <see cref="StreamType"/> instances for the directory specified by <paramref name="path"/>.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to search.</param>
      /// <param name="streamType">The type of stream to retrieve.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>An enumerable collection of <see cref="AlternateDataStreamInfo"/> of type <see cref="StreamType"/> instances for the directory specified by <paramref name="path"/>.</returns>
      [SecurityCritical]
      public static IEnumerable<AlternateDataStreamInfo> EnumerateStreams(KernelTransaction transaction, string path, StreamType streamType, PathFormat pathFormat)
      {
         return AlternateDataStreamInfo.EnumerateStreamsInternal(null, transaction, null, path, null, streamType, pathFormat);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Returns an enumerable collection of <see cref="AlternateDataStreamInfo"/> instances for the directory specified by <paramref name="path"/>.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to search.</param>
      /// <returns>An enumerable collection of <see cref="AlternateDataStreamInfo"/> instances for the directory specified by <paramref name="path"/>.</returns>
      [SecurityCritical]
      public static IEnumerable<AlternateDataStreamInfo> EnumerateStreams(KernelTransaction transaction, string path)
      {
         return AlternateDataStreamInfo.EnumerateStreamsInternal(null, transaction, null, path, null, null, PathFormat.Relative);
      }

      /// <summary>[AlphaFS] Returns an enumerable collection of <see cref="AlternateDataStreamInfo"/> of type <see cref="StreamType"/> instances for the directory specified by <paramref name="path"/>.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file to search.</param>
      /// <param name="streamType">The type of stream to retrieve.</param>
      /// <returns>An enumerable collection of <see cref="AlternateDataStreamInfo"/> of type <see cref="StreamType"/> instances for the directory specified by <paramref name="path"/>.</returns>
      [SecurityCritical]
      public static IEnumerable<AlternateDataStreamInfo> EnumerateStreams(KernelTransaction transaction, string path, StreamType streamType)
      {
         return AlternateDataStreamInfo.EnumerateStreamsInternal(null, transaction, null, path, null, streamType, PathFormat.Relative);
      }

      #endregion // Transacted

      #endregion // EnumerateStreams

      #region GetChangeTime

      #region IsFullPath

      /// <summary>Gets the change date and time of the specified directory.</summary>
      /// <param name="path">
      ///   The directory for which to obtain creation date and time information.
      /// </param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>
      ///   A <see cref="System.DateTime"/> structure set to the change date and time for the specified
      ///   directory. This value is expressed in local time.
      /// </returns>
      [SecurityCritical]
      public static DateTime GetChangeTime(string path, PathFormat pathFormat)
      {
         return File.GetChangeTimeInternal(true, null, null, path, false, pathFormat);
      }

      #endregion // IsFullPath

      /// <summary>Gets the change date and time of the specified directory.</summary>
      /// <returns>A <see cref="System.DateTime"/> structure set to the change date and time for the specified directory. This value is expressed in local time.</returns>
      /// <param name="path">The directory for which to obtain creation date and time information.</param>
      [SecurityCritical]
      public static DateTime GetChangeTime(string path)
      {
         return File.GetChangeTimeInternal(true, null, null, path, false, PathFormat.Relative);
      }

      /// <summary>Gets the change date and time of the specified directory.</summary>
      /// <param name="safeHandle">
      ///   An open handle to the directory from which to retrieve information.
      /// </param>
      /// <returns>
      ///   A <see cref="System.DateTime"/> structure set to the change date and time for the specified
      ///   directory. This value is expressed in local time.
      /// </returns>
      [SecurityCritical]
      public static DateTime GetChangeTime(SafeFileHandle safeHandle)
      {
         return File.GetChangeTimeInternal(true, null, safeHandle, null, false, PathFormat.Relative);
      }

      #region Transacted

      #region IsFullPath

      /// <summary>Gets the change date and time of the specified directory.</summary>
      /// <returns>A <see cref="System.DateTime"/> structure set to the change date and time for the specified directory. This value is expressed in local time.</returns>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory for which to obtain creation date and time information.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static DateTime GetChangeTime(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         return File.GetChangeTimeInternal(true, transaction, null, path, false, pathFormat);
      }

      #endregion // IsFullPath

      /// <summary>Gets the change date and time of the specified directory.</summary>
      /// <returns>A <see cref="System.DateTime"/> structure set to the change date and time for the specified directory. This value is expressed in local time.</returns>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory for which to obtain creation date and time information.</param>
      [SecurityCritical]
      public static DateTime GetChangeTime(KernelTransaction transaction, string path)
      {
         return File.GetChangeTimeInternal(true, transaction, null, path, false, PathFormat.Relative);
      }

      #endregion // Transacted

      #endregion // GetChangeTime

      #region GetChangeTimeUtc

      #region IsFullPath

      /// <summary>Gets the change date and time, in Coordinated Universal Time (UTC) format, of the specified directory.</summary>
      /// <returns>A <see cref="System.DateTime"/> structure set to the change date and time for the specified directory. This value is expressed in UTC time.</returns>
      /// <param name="path">The file for which to obtain change date and time information, in Coordinated Universal Time (UTC) format.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static DateTime GetChangeTimeUtc(string path, PathFormat pathFormat)
      {
         return File.GetChangeTimeInternal(true, null, null, path, true, pathFormat);
      }

      #endregion // IsFullPath

      /// <summary>Gets the change date and time, in Coordinated Universal Time (UTC) format, of the specified directory.</summary>
      /// <returns>A <see cref="System.DateTime"/> structure set to the change date and time for the specified directory. This value is expressed in UTC time.</returns>
      /// <param name="path">The file for which to obtain change date and time information, in Coordinated Universal Time (UTC) format.</param>
      [SecurityCritical]
      public static DateTime GetChangeTimeUtc(string path)
      {
         return File.GetChangeTimeInternal(true, null, null, path, true, PathFormat.Relative);
      }

      /// <summary>
      ///   Gets the change date and time, in Coordinated Universal Time (UTC) format, of the specified
      ///   directory.
      /// </summary>
      /// <param name="safeHandle">
      ///   An open handle to the directory from which to retrieve information.
      /// </param>
      /// <returns>
      ///   A <see cref="System.DateTime"/> structure set to the change date and time for the specified
      ///   directory. This value is expressed in UTC time.
      /// </returns>
      [SecurityCritical]
      public static DateTime GetChangeTimeUtc(SafeFileHandle safeHandle)
      {
         return File.GetChangeTimeInternal(true, null, safeHandle, null, true, PathFormat.Relative);
      }

      #region Transacted

      #region IsFullPath

      /// <summary>Gets the change date and time, in Coordinated Universal Time (UTC) format, of the specified directory.</summary>
      /// <returns>A <see cref="System.DateTime"/> structure set to the change date and time for the specified directory. This value is expressed in UTC time.</returns>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file for which to obtain change date and time information, in Coordinated Universal Time (UTC) format.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static DateTime GetChangeTimeUtc(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         return File.GetChangeTimeInternal(true, transaction, null, path, true, pathFormat);
      }

      #endregion // IsFullPath

      /// <summary>Gets the change date and time, in Coordinated Universal Time (UTC) format, of the specified directory.</summary>
      /// <returns>A <see cref="System.DateTime"/> structure set to the change date and time for the specified directory. This value is expressed in UTC time.</returns>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The file for which to obtain change date and time information, in Coordinated Universal Time (UTC) format.</param>
      [SecurityCritical]
      public static DateTime GetChangeTimeUtc(KernelTransaction transaction, string path)
      {
         return File.GetChangeTimeInternal(true, transaction, null, path, true, PathFormat.Relative);
      }

      #endregion // Transacted

      #endregion // GetChangeTimeUtc

      #region GetProperties

      #region IsFullPath

      /// <summary>[AlphaFS] Gets the properties of the particular directory without following any symbolic links or mount points.
      /// Properties include aggregated info from <see cref="FileAttributes"/> of each encountered file system object.
      /// Plus additional ones: Total, File, Size, Error
      /// <para><b>Total:</b> is the total number of enumerated objects.</para>
      /// <para><b>File:</b> is the total number of files. File is considered when object is neither <see cref="FileAttributes.Directory"/> nor <see cref="FileAttributes.ReparsePoint"/>.</para>
      /// <para><b>Size:</b> is the total size of enumerated objects.</para>
      /// <para><b>Error:</b> is the total number of errors encountered during enumeration.</para>
      /// </summary>
      /// <remarks><b>Directory:</b> is an object which has <see cref="FileAttributes.Directory"/> attribute without <see cref="FileAttributes.ReparsePoint"/> one.</remarks>
      /// <param name="path">The target directory.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>A dictionary mapping the keys mentioned above to their respective aggregated values.</returns>
      [SecurityCritical]
      public static Dictionary<string, long> GetProperties(string path, PathFormat pathFormat)
      {
         return GetPropertiesInternal(null, path, DirectoryEnumerationOptions.FilesAndFolders, pathFormat);
      }

      /// <summary>[AlphaFS] Gets the properties of the particular directory without following any symbolic links or mount points.
      /// Properties include aggregated info from <see cref="FileAttributes"/> of each encountered file system object.
      /// Plus additional ones: Total, File, Size, Error
      /// <para><b>Total:</b> is the total number of enumerated objects.</para>
      /// <para><b>File:</b> is the total number of files. File is considered when object is neither <see cref="FileAttributes.Directory"/> nor <see cref="FileAttributes.ReparsePoint"/>.</para>
      /// <para><b>Size:</b> is the total size of enumerated objects.</para>
      /// <para><b>Error:</b> is the total number of errors encountered during enumeration.</para>
      /// </summary>
      /// <remarks><b>Directory:</b> is an object which has <see cref="FileAttributes.Directory"/> attribute without <see cref="FileAttributes.ReparsePoint"/> one.</remarks>
      /// <param name="path">The target directory.</param>
      /// <param name="directoryEnumerationOptions"><see cref="DirectoryEnumerationOptions"/> flags that specify how the directory is to be enumerated.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>A dictionary mapping the keys mentioned above to their respective aggregated values.</returns>
      [SecurityCritical]
      public static Dictionary<string, long> GetProperties(string path, DirectoryEnumerationOptions directoryEnumerationOptions, PathFormat pathFormat)
      {
         return GetPropertiesInternal(null, path, directoryEnumerationOptions, pathFormat);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Gets the properties of the particular directory without following any symbolic links or mount points.
      /// <para>Properties include aggregated info from <see cref="FileAttributes"/> of each encountered file system object,</para>
      /// <para>plus additional ones: Total, File, Size, Error</para>
      /// <para><b>Total:</b> is the total number of enumerated objects.</para>
      /// <para><b>File:</b> is the total number of files. File is considered when object is neither <see cref="FileAttributes.Directory"/> nor <see cref="FileAttributes.ReparsePoint"/>.</para>
      /// <para><b>Size:</b> is the total size of enumerated objects.</para>
      /// <para><b>Error:</b> is the total number of errors encountered during enumeration.</para>
      /// </summary>
      /// <remarks><b>Directory:</b> is an object which has <see cref="FileAttributes.Directory"/> attribute without <see cref="FileAttributes.ReparsePoint"/> one.</remarks>
      /// <param name="path">The target directory.</param>
      /// <returns>A dictionary mapping the keys mentioned above to their respective aggregated values.</returns>
      [SecurityCritical]
      public static Dictionary<string, long> GetProperties(string path)
      {
         return GetPropertiesInternal(null, path, DirectoryEnumerationOptions.FilesAndFolders, PathFormat.Relative);
      }

      /// <summary>[AlphaFS] Gets the properties of the particular directory without following any symbolic links or mount points.
      /// Properties include aggregated info from <see cref="FileAttributes"/> of each encountered file system object.
      /// Plus additional ones: Total, File, Size, Error
      /// <para><b>Total:</b> is the total number of enumerated objects.</para>
      /// <para><b>File:</b> is the total number of files. File is considered when object is neither <see cref="FileAttributes.Directory"/> nor <see cref="FileAttributes.ReparsePoint"/>.</para>
      /// <para><b>Size:</b> is the total size of enumerated objects.</para>
      /// <para><b>Error:</b> is the total number of errors encountered during enumeration.</para>
      /// </summary>
      /// <remarks><b>Directory:</b> is an object which has <see cref="FileAttributes.Directory"/> attribute without <see cref="FileAttributes.ReparsePoint"/> one.</remarks>
      /// <param name="path">The target directory.</param>
      /// <param name="directoryEnumerationOptions"><see cref="DirectoryEnumerationOptions"/> flags that specify how the directory is to be enumerated.</param>
      /// <returns>A dictionary mapping the keys mentioned above to their respective aggregated values.</returns>
      [SecurityCritical]
      public static Dictionary<string, long> GetProperties(string path, DirectoryEnumerationOptions directoryEnumerationOptions)
      {
         return GetPropertiesInternal(null, path, directoryEnumerationOptions, PathFormat.Relative);
      }

      #region Transacted

      #region IsFullPath

      /// <summary>[AlphaFS] Gets the properties of the particular directory without following any symbolic links or mount points.
      /// Properties include aggregated info from <see cref="FileAttributes"/> of each encountered file system object.
      /// Plus additional ones: Total, File, Size, Error
      /// <para><b>Total:</b> is the total number of enumerated objects.</para>
      /// <para><b>File:</b> is the total number of files. File is considered when object is neither <see cref="FileAttributes.Directory"/> nor <see cref="FileAttributes.ReparsePoint"/>.</para>
      /// <para><b>Size:</b> is the total size of enumerated objects.</para>
      /// <para><b>Error:</b> is the total number of errors encountered during enumeration.</para>
      /// </summary>
      /// <remarks><b>Directory:</b> is an object which has <see cref="FileAttributes.Directory"/> attribute without <see cref="FileAttributes.ReparsePoint"/> one.</remarks>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The target directory.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>A dictionary mapping the keys mentioned above to their respective aggregated values.</returns>
      [SecurityCritical]
      public static Dictionary<string, long> GetProperties(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         return GetPropertiesInternal(transaction, path, DirectoryEnumerationOptions.FilesAndFolders, pathFormat);
      }

      /// <summary>[AlphaFS] Gets the properties of the particular directory without following any symbolic links or mount points.
      /// Properties include aggregated info from <see cref="FileAttributes"/> of each encountered file system object.
      /// Plus additional ones: Total, File, Size, Error
      /// <para><b>Total:</b> is the total number of enumerated objects.</para>
      /// <para><b>File:</b> is the total number of files. File is considered when object is neither <see cref="FileAttributes.Directory"/> nor <see cref="FileAttributes.ReparsePoint"/>.</para>
      /// <para><b>Size:</b> is the total size of enumerated objects.</para>
      /// <para><b>Error:</b> is the total number of errors encountered during enumeration.</para>
      /// </summary>
      /// <remarks><b>Directory:</b> is an object which has <see cref="FileAttributes.Directory"/> attribute without <see cref="FileAttributes.ReparsePoint"/> one.</remarks>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The target directory.</param>
      /// <param name="directoryEnumerationOptions"><see cref="DirectoryEnumerationOptions"/> flags that specify how the directory is to be enumerated.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>A dictionary mapping the keys mentioned above to their respective aggregated values.</returns>
      [SecurityCritical]
      public static Dictionary<string, long> GetProperties(KernelTransaction transaction, string path, DirectoryEnumerationOptions directoryEnumerationOptions, PathFormat pathFormat)
      {
         return GetPropertiesInternal(transaction, path, directoryEnumerationOptions, pathFormat);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Gets the properties of the particular directory without following any symbolic links or mount points.
      /// Properties include aggregated info from <see cref="FileAttributes"/> of each encountered file system object.
      /// Plus additional ones: Total, File, Size, Error
      /// <para><b>Total:</b> is the total number of enumerated objects.</para>
      /// <para><b>File:</b> is the total number of files. File is considered when object is neither <see cref="FileAttributes.Directory"/> nor <see cref="FileAttributes.ReparsePoint"/>.</para>
      /// <para><b>Size:</b> is the total size of enumerated objects.</para>
      /// <para><b>Error:</b> is the total number of errors encountered during enumeration.</para>
      /// </summary>
      /// <remarks><b>Directory:</b> is an object which has <see cref="FileAttributes.Directory"/> attribute without <see cref="FileAttributes.ReparsePoint"/> one.</remarks>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The target directory.</param>
      /// <returns>A dictionary mapping the keys mentioned above to their respective aggregated values.</returns>
      [SecurityCritical]
      public static Dictionary<string, long> GetProperties(KernelTransaction transaction, string path)
      {
         return GetPropertiesInternal(transaction, path, DirectoryEnumerationOptions.FilesAndFolders, PathFormat.Relative);
      }

      /// <summary>[AlphaFS] Gets the properties of the particular directory without following any symbolic links or mount points.
      /// Properties include aggregated info from <see cref="FileAttributes"/> of each encountered file system object.
      /// Plus additional ones: Total, File, Size, Error
      /// <para><b>Total:</b> is the total number of enumerated objects.</para>
      /// <para><b>File:</b> is the total number of files. File is considered when object is neither <see cref="FileAttributes.Directory"/> nor <see cref="FileAttributes.ReparsePoint"/>.</para>
      /// <para><b>Size:</b> is the total size of enumerated objects.</para>
      /// <para><b>Error:</b> is the total number of errors encountered during enumeration.</para>
      /// </summary>
      /// <remarks><b>Directory:</b> is an object which has <see cref="FileAttributes.Directory"/> attribute without <see cref="FileAttributes.ReparsePoint"/> one.</remarks>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The target directory.</param>
      /// <param name="directoryEnumerationOptions"><see cref="DirectoryEnumerationOptions"/> flags that specify how the directory is to be enumerated.</param>
      /// <returns>A dictionary mapping the keys mentioned above to their respective aggregated values.</returns>
      [SecurityCritical]
      public static Dictionary<string, long> GetProperties(KernelTransaction transaction, string path, DirectoryEnumerationOptions directoryEnumerationOptions)
      {
         return GetPropertiesInternal(transaction, path, directoryEnumerationOptions, PathFormat.Relative);
      }

      #endregion // Transacted

      #endregion // GetProperties

      #region GetStreamSize

      #region IsFullPath

      /// <summary>[AlphaFS] Retrieves the actual number of bytes of disk storage used by all data streams (NTFS ADS).</summary>
      /// <param name="path">The path to an existing directory.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>The number of bytes used by all data streams.</returns>
      [SecurityCritical]
      public static long GetStreamSize(string path, PathFormat pathFormat)
      {
         return AlternateDataStreamInfo.GetStreamSizeInternal(true, null, null, path, null, null, pathFormat);
      }

      /// <summary>[AlphaFS] Retrieves the actual number of bytes of disk storage used by a named data streams (NTFS ADS).</summary>
      /// <param name="path">The path to an existing directory.</param>
      /// <param name="name">The name of the stream to retrieve.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>The number of bytes used by a named stream.</returns>
      [SecurityCritical]
      public static long GetStreamSize(string path, string name, PathFormat pathFormat)
      {
         return AlternateDataStreamInfo.GetStreamSizeInternal(true, null, null, path, name, StreamType.Data, pathFormat);
      }

      /// <summary>[AlphaFS] Retrieves the actual number of bytes of disk storage used by a <see cref="StreamType"/> data streams (NTFS ADS).</summary>
      /// <param name="path">The path to an existing directory.</param>
      /// <param name="type">The <see cref="StreamType"/> of the stream to retrieve.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>The number of bytes used by stream of type <see cref="StreamType"/>.</returns>
      [SecurityCritical]
      public static long GetStreamSize(string path, StreamType type, PathFormat pathFormat)
      {
         return AlternateDataStreamInfo.GetStreamSizeInternal(true, null, null, path, null, type, pathFormat);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Retrieves the actual number of bytes of disk storage used by all data streams (NTFS ADS).</summary>
      /// <param name="path">The path to an existing directory.</param>
      /// <returns>The number of bytes used by all data streams.</returns>
      [SecurityCritical]
      public static long GetStreamSize(string path)
      {
         return AlternateDataStreamInfo.GetStreamSizeInternal(true, null, null, path, null, null, PathFormat.Relative);
      }

      /// <summary>[AlphaFS] Retrieves the actual number of bytes of disk storage used by a named data streams (NTFS ADS).</summary>
      /// <param name="path">The path to an existing directory.</param>
      /// <param name="name">The name of the stream to retrieve.</param>
      /// <returns>The number of bytes used by a named stream.</returns>
      [SecurityCritical]
      public static long GetStreamSize(string path, string name)
      {
         return AlternateDataStreamInfo.GetStreamSizeInternal(true, null, null, path, name, StreamType.Data, PathFormat.Relative);
      }

      /// <summary>[AlphaFS] Retrieves the actual number of bytes of disk storage used by a <see cref="StreamType"/> data streams (NTFS ADS).</summary>
      /// <param name="path">The path to an existing directory.</param>
      /// <param name="type">The <see cref="StreamType"/> of the stream to retrieve.</param>
      /// <returns>The number of bytes used by stream of type <see cref="StreamType"/>.</returns>
      [SecurityCritical]
      public static long GetStreamSize(string path, StreamType type)
      {
         return AlternateDataStreamInfo.GetStreamSizeInternal(true, null, null, path, null, type, PathFormat.Relative);
      }

      /// <summary>[AlphaFS] Retrieves the actual number of bytes of disk storage used by all data streams (NTFS ADS).</summary>
      /// <param name="handle">The <see cref="SafeFileHandle"/> to the directory.</param>
      /// <param name="name">The name of the stream to retrieve.</param>
      /// <returns>The number of bytes used by a named stream.</returns>
      [SecurityCritical]
      public static long GetStreamSize(SafeFileHandle handle, string name)
      {
         return AlternateDataStreamInfo.GetStreamSizeInternal(true, null, handle, null, name, StreamType.Data, PathFormat.LongFullPath);
      }

      /// <summary>[AlphaFS] Retrieves the actual number of bytes of disk storage used by a <see cref="StreamType"/> data streams (NTFS ADS).</summary>
      /// <param name="handle">The <see cref="SafeFileHandle"/> to the directory.</param>
      /// <param name="type">The <see cref="StreamType"/> of the stream to retrieve.</param>
      /// <returns>The number of bytes used by stream of type <see cref="StreamType"/>.</returns>
      [SecurityCritical]
      public static long GetStreamSize(SafeFileHandle handle, StreamType type)
      {
         return AlternateDataStreamInfo.GetStreamSizeInternal(true, null, handle, null, null, type, PathFormat.LongFullPath);
      }

      #region Transacted

      #region IsFullPath

      /// <summary>[AlphaFS] Retrieves the actual number of bytes of disk storage used by all data streams (NTFS ADS).</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to an existing directory.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>The number of bytes used by all data streams.</returns>
      [SecurityCritical]
      public static long GetStreamSize(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         return AlternateDataStreamInfo.GetStreamSizeInternal(true, transaction, null, path, null, null, pathFormat);
      }

      /// <summary>[AlphaFS] Retrieves the actual number of bytes of disk storage used by a named data streams (NTFS ADS).</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to an existing directory.</param>
      /// <param name="name">The name of the stream to retrieve.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>The number of bytes used by a named stream.</returns>
      [SecurityCritical]
      public static long GetStreamSize(KernelTransaction transaction, string path, string name, PathFormat pathFormat)
      {
         return AlternateDataStreamInfo.GetStreamSizeInternal(true, transaction, null, path, name, StreamType.Data, pathFormat);
      }

      /// <summary>[AlphaFS] Retrieves the actual number of bytes of disk storage used by a <see cref="StreamType"/> data streams (NTFS ADS).</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to an existing directory.</param>
      /// <param name="type">The <see cref="StreamType"/> of the stream to retrieve.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>The number of bytes used by stream of type <see cref="StreamType"/>.</returns>
      [SecurityCritical]
      public static long GetStreamSize(KernelTransaction transaction, string path, StreamType type, PathFormat pathFormat)
      {
         return AlternateDataStreamInfo.GetStreamSizeInternal(true, transaction, null, path, null, type, pathFormat);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Retrieves the actual number of bytes of disk storage used by all data streams (NTFS ADS).</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to an existing directory.</param>
      /// <returns>The number of bytes used by all data streams.</returns>
      [SecurityCritical]
      public static long GetStreamSize(KernelTransaction transaction, string path)
      {
         return AlternateDataStreamInfo.GetStreamSizeInternal(true, transaction, null, path, null, null, PathFormat.Relative);
      }

      /// <summary>[AlphaFS] Retrieves the actual number of bytes of disk storage used by a named data streams (NTFS ADS).</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to an existing directory.</param>
      /// <param name="name">The name of the stream to retrieve.</param>
      /// <returns>The number of bytes used by a named stream.</returns>
      [SecurityCritical]
      public static long GetStreamSize(KernelTransaction transaction, string path, string name)
      {
         return AlternateDataStreamInfo.GetStreamSizeInternal(true, transaction, null, path, name, null, PathFormat.Relative);
      }

      /// <summary>[AlphaFS] Retrieves the actual number of bytes of disk storage used by all data streams (NTFS ADS).</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to an existing directory.</param>
      /// <param name="type">The <see cref="StreamType"/> of the stream to retrieve.</param>
      /// <returns>The number of bytes used by stream of type <see cref="StreamType"/>.</returns>
      [SecurityCritical]
      public static long GetStreamSize(KernelTransaction transaction, string path, StreamType type)
      {
         return AlternateDataStreamInfo.GetStreamSizeInternal(true, transaction, null, path, null, type, PathFormat.Relative);
      }

      #endregion // Transacted

      #endregion GetStreamSize

      #region HasInheritedPermissions

      #region IsFullPath

      /// <summary>[AlphaFS] Check if the directory has permission inheritance enabled.</summary>
      /// <returns><see langword="true"/> if permission inheritance is enabled, <see langword="false"/> if permission inheritance is disabled.</returns>
      /// <param name="path">The full path to the directory to check.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      public static bool HasInheritedPermissions(string path, PathFormat pathFormat)
      {
         if (Utils.IsNullOrWhiteSpace(path))
            throw new ArgumentNullException("path");

         //DirectorySecurity acl = GetAccessControl(directoryPath);
         DirectorySecurity acl = File.GetAccessControlInternal<DirectorySecurity>(true, path, AccessControlSections.Access | AccessControlSections.Group | AccessControlSections.Owner, pathFormat);

         return acl.GetAccessRules(false, true, typeof(SecurityIdentifier)).Count > 0;
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Check if the directory has permission inheritance enabled.</summary>
      /// <param name="path">The full path to the directory to check.</param>
      /// <returns><see langword="true"/> if permission inheritance is enabled, <see langword="false"/> if permission inheritance is disabled.</returns>
      public static bool HasInheritedPermissions(string path)
      {
         if (Utils.IsNullOrWhiteSpace(path))
            throw new ArgumentNullException("path");

         DirectorySecurity acl = File.GetAccessControlInternal<DirectorySecurity>(true, path, AccessControlSections.Access | AccessControlSections.Group | AccessControlSections.Owner, PathFormat.Relative);

         return acl.GetAccessRules(false, true, typeof(SecurityIdentifier)).Count > 0;
      }

      #endregion // HasInheritedPermissions

      #region RemoveStream

      #region IsFullPath

      /// <summary>[AlphaFS] Removes all alternate data streams (NTFS ADS) from an existing directory.</summary>
      /// <param name="path">The path to an existing directory.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <remarks>This method only removes streams of type <see cref="StreamType.AlternateData"/>.</remarks>
      /// <remarks>No Exception is thrown if the stream does not exist.</remarks>
      [SecurityCritical]
      public static void RemoveStream(string path, PathFormat pathFormat)
      {
         AlternateDataStreamInfo.RemoveStreamInternal(true, null, path, null, pathFormat);
      }

      /// <summary>[AlphaFS] Removes an alternate data stream (NTFS ADS) from an existing directory.</summary>
      /// <param name="path">The path to an existing directory.</param>
      /// <param name="name">The name of the stream to remove.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <remarks>This method only removes streams of type <see cref="StreamType.AlternateData"/>.</remarks>
      /// <remarks>No Exception is thrown if the stream does not exist.</remarks>
      [SecurityCritical]
      public static void RemoveStream(string path, string name, PathFormat pathFormat)
      {
         AlternateDataStreamInfo.RemoveStreamInternal(true, null, path, name, pathFormat);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Removes all alternate data streams (NTFS ADS) from an existing directory.</summary>
      /// <param name="path">The path to an existing directory.</param>
      /// <remarks>This method only removes streams of type <see cref="StreamType.AlternateData"/>.</remarks>
      /// <remarks>No Exception is thrown if the stream does not exist.</remarks>
      [SecurityCritical]
      public static void RemoveStream(string path)
      {
         AlternateDataStreamInfo.RemoveStreamInternal(true, null, path, null, PathFormat.Relative);
      }

      /// <summary>[AlphaFS] Removes an alternate data stream (NTFS ADS) from an existing directory.</summary>
      /// <param name="path">The path to an existing directory.</param>
      /// <param name="name">The name of the stream to remove.</param>
      /// <remarks>This method only removes streams of type <see cref="StreamType.AlternateData"/>.</remarks>
      /// <remarks>No Exception is thrown if the stream does not exist.</remarks>
      [SecurityCritical]
      public static void RemoveStream(string path, string name)
      {
         AlternateDataStreamInfo.RemoveStreamInternal(true, null, path, name, PathFormat.Relative);
      }

      #region Transacted

      #region IsFullPath

      /// <summary>[AlphaFS] Removes all alternate data streams (NTFS ADS) from an existing directory.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to an existing directory.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <remarks>This method only removes streams of type <see cref="StreamType.AlternateData"/>.</remarks>
      /// <remarks>No Exception is thrown if the stream does not exist.</remarks>
      [SecurityCritical]
      public static void RemoveStream(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         AlternateDataStreamInfo.RemoveStreamInternal(true, transaction, path, null, pathFormat);
      }

      /// <summary>[AlphaFS] Removes an alternate data stream (NTFS ADS) from an existing directory.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to an existing directory.</param>
      /// <param name="name">The name of the stream to remove.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <remarks>This method only removes streams of type <see cref="StreamType.AlternateData"/>.</remarks>
      /// <remarks>No Exception is thrown if the stream does not exist.</remarks>
      [SecurityCritical]
      public static void RemoveStream(KernelTransaction transaction, string path, string name, PathFormat pathFormat)
      {
         AlternateDataStreamInfo.RemoveStreamInternal(true, transaction, path, name, pathFormat);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Removes all alternate data streams (NTFS ADS) from an existing directory.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to an existing directory.</param>
      /// <remarks>This method only removes streams of type <see cref="StreamType.AlternateData"/>.</remarks>
      /// <remarks>No Exception is thrown if the stream does not exist.</remarks>
      [SecurityCritical]
      public static void RemoveStream(KernelTransaction transaction, string path)
      {
         AlternateDataStreamInfo.RemoveStreamInternal(true, transaction, path, null, PathFormat.Relative);
      }

      /// <summary>[AlphaFS] Removes an alternate data stream (NTFS ADS) from an existing directory.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to an existing directory.</param>
      /// <param name="name">The name of the stream to remove.</param>
      /// <remarks>This method only removes streams of type <see cref="StreamType.AlternateData"/>.</remarks>
      /// <remarks>No Exception is thrown if the stream does not exist.</remarks>
      [SecurityCritical]
      public static void RemoveStream(KernelTransaction transaction, string path, string name)
      {
         AlternateDataStreamInfo.RemoveStreamInternal(true, transaction, path, name, PathFormat.Relative);
      }

      #endregion Transacted

      #endregion // RemoveStream

      #region SetTimestamps

      #region IsFullPath

      /// <summary>[AlphaFS] Sets all the date and time stamps for the specified directory, at once.</summary>
      /// <param name="path">The directory for which to set the dates and times information.</param>
      /// <param name="creationTime">A <see cref="System.DateTime"/> containing the value to set for the creation date and time of <paramref name="path"/>. This value is expressed in local time.</param>
      /// <param name="lastAccessTime">A <see cref="System.DateTime"/> containing the value to set for the last access date and time of <paramref name="path"/>. This value is expressed in local time.</param>
      /// <param name="lastWriteTime">A <see cref="System.DateTime"/> containing the value to set for the last write date and time of <paramref name="path"/>. This value is expressed in local time.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void SetTimestamps(string path, DateTime creationTime, DateTime lastAccessTime, DateTime lastWriteTime, PathFormat pathFormat)
      {
         File.SetFsoDateTimeInternal(true, null, path, creationTime.ToUniversalTime(), lastAccessTime.ToUniversalTime(), lastWriteTime.ToUniversalTime(), pathFormat);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Sets all the date and time stamps for the specified directory, at once.</summary>
      /// <param name="path">The directory for which to set the dates and times information.</param>
      /// <param name="creationTime">A <see cref="System.DateTime"/> containing the value to set for the creation date and time of <paramref name="path"/>. This value is expressed in local time.</param>
      /// <param name="lastAccessTime">A <see cref="System.DateTime"/> containing the value to set for the last access date and time of <paramref name="path"/>. This value is expressed in local time.</param>
      /// <param name="lastWriteTime">A <see cref="System.DateTime"/> containing the value to set for the last write date and time of <paramref name="path"/>. This value is expressed in local time.</param>
      [SecurityCritical]
      public static void SetTimestamps(string path, DateTime creationTime, DateTime lastAccessTime, DateTime lastWriteTime)
      {
         File.SetFsoDateTimeInternal(true, null, path, creationTime.ToUniversalTime(), lastAccessTime.ToUniversalTime(), lastWriteTime.ToUniversalTime(), PathFormat.Relative);
      }

      #region Transacted

      #region IsFullPath

      /// <summary>[AlphaFS] Sets all the date and time stamps for the specified directory, at once.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory for which to set the dates and times information.</param>
      /// <param name="creationTime">A <see cref="System.DateTime"/> containing the value to set for the creation date and time of <paramref name="path"/>. This value is expressed in local time.</param>
      /// <param name="lastAccessTime">A <see cref="System.DateTime"/> containing the value to set for the last access date and time of <paramref name="path"/>. This value is expressed in local time.</param>
      /// <param name="lastWriteTime">A <see cref="System.DateTime"/> containing the value to set for the last write date and time of <paramref name="path"/>. This value is expressed in local time.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void SetTimestamps(KernelTransaction transaction, string path, DateTime creationTime, DateTime lastAccessTime, DateTime lastWriteTime, PathFormat pathFormat)
      {
         File.SetFsoDateTimeInternal(true, transaction, path, creationTime.ToUniversalTime(), lastAccessTime.ToUniversalTime(), lastWriteTime.ToUniversalTime(), pathFormat);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Sets all the date and time stamps for the specified directory, at once.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory for which to set the dates and times information.</param>
      /// <param name="creationTime">A <see cref="System.DateTime"/> containing the value to set for the creation date and time of <paramref name="path"/>. This value is expressed in local time.</param>
      /// <param name="lastAccessTime">A <see cref="System.DateTime"/> containing the value to set for the last access date and time of <paramref name="path"/>. This value is expressed in local time.</param>
      /// <param name="lastWriteTime">A <see cref="System.DateTime"/> containing the value to set for the last write date and time of <paramref name="path"/>. This value is expressed in local time.</param>
      [SecurityCritical]
      public static void SetTimestamps(KernelTransaction transaction, string path, DateTime creationTime, DateTime lastAccessTime, DateTime lastWriteTime)
      {
         File.SetFsoDateTimeInternal(true, transaction, path, creationTime.ToUniversalTime(), lastAccessTime.ToUniversalTime(), lastWriteTime.ToUniversalTime(), PathFormat.Relative);
      }

      #endregion // Transacted

      #endregion // SetTimestamps

      #region SetTimestampsUtc

      #region IsFullPath

      /// <summary>[AlphaFS] Sets all the date and time stamps, in coordinated universal time (UTC), for the specified directory, at once.</summary>
      /// <param name="path">The directory for which to set the dates and times information.</param>
      /// <param name="creationTimeUtc">A <see cref="System.DateTime"/> containing the value to set for the creation date and time of <paramref name="path"/>. This value is expressed in UTC time.</param>
      /// <param name="lastAccessTimeUtc">A <see cref="System.DateTime"/> containing the value to set for the last access date and time of <paramref name="path"/>. This value is expressed in UTC time.</param>
      /// <param name="lastWriteTimeUtc">A <see cref="System.DateTime"/> containing the value to set for the last write date and time of <paramref name="path"/>. This value is expressed in UTC time.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void SetTimestampsUtc(string path, DateTime creationTimeUtc, DateTime lastAccessTimeUtc, DateTime lastWriteTimeUtc, PathFormat pathFormat)
      {
         File.SetFsoDateTimeInternal(true, null, path, creationTimeUtc, lastAccessTimeUtc, lastWriteTimeUtc, pathFormat);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Sets all the date and time stamps, in coordinated universal time (UTC), for the specified directory, at once.</summary>
      /// <param name="path">The directory for which to set the dates and times information.</param>
      /// <param name="creationTimeUtc">A <see cref="System.DateTime"/> containing the value to set for the creation date and time of <paramref name="path"/>. This value is expressed in UTC time.</param>
      /// <param name="lastAccessTimeUtc">A <see cref="System.DateTime"/> containing the value to set for the last access date and time of <paramref name="path"/>. This value is expressed in UTC time.</param>
      /// <param name="lastWriteTimeUtc">A <see cref="System.DateTime"/> containing the value to set for the last write date and time of <paramref name="path"/>. This value is expressed in UTC time.</param>
      [SecurityCritical]
      public static void SetTimestampsUtc(string path, DateTime creationTimeUtc, DateTime lastAccessTimeUtc, DateTime lastWriteTimeUtc)
      {
         File.SetFsoDateTimeInternal(true, null, path, creationTimeUtc, lastAccessTimeUtc, lastWriteTimeUtc, PathFormat.Relative);
      }

      #region Transacted

      #region IsFullPath

      /// <summary>[AlphaFS] Sets all the date and time stamps, in coordinated universal time (UTC), for the specified directory, at once.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory for which to set the dates and times information.</param>
      /// <param name="creationTimeUtc">A <see cref="System.DateTime"/> containing the value to set for the creation date and time of <paramref name="path"/>. This value is expressed in UTC time.</param>
      /// <param name="lastAccessTimeUtc">A <see cref="System.DateTime"/> containing the value to set for the last access date and time of <paramref name="path"/>. This value is expressed in UTC time.</param>
      /// <param name="lastWriteTimeUtc">A <see cref="System.DateTime"/> containing the value to set for the last write date and time of <paramref name="path"/>. This value is expressed in UTC time.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static void SetTimestampsUtc(KernelTransaction transaction, string path, DateTime creationTimeUtc, DateTime lastAccessTimeUtc, DateTime lastWriteTimeUtc, PathFormat pathFormat)
      {
         File.SetFsoDateTimeInternal(true, transaction, path, creationTimeUtc, lastAccessTimeUtc, lastWriteTimeUtc, pathFormat);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Sets all the date and time stamps, in coordinated universal time (UTC), for the specified directory, at once.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory for which to set the dates and times information.</param>
      /// <param name="creationTimeUtc">A <see cref="System.DateTime"/> containing the value to set for the creation date and time of <paramref name="path"/>. This value is expressed in UTC time.</param>
      /// <param name="lastAccessTimeUtc">A <see cref="System.DateTime"/> containing the value to set for the last access date and time of <paramref name="path"/>. This value is expressed in UTC time.</param>
      /// <param name="lastWriteTimeUtc">A <see cref="System.DateTime"/> containing the value to set for the last write date and time of <paramref name="path"/>. This value is expressed in UTC time.</param>
      [SecurityCritical]
      public static void SetTimestampsUtc(KernelTransaction transaction, string path, DateTime creationTimeUtc, DateTime lastAccessTimeUtc, DateTime lastWriteTimeUtc)
      {
         File.SetFsoDateTimeInternal(true, transaction, path, creationTimeUtc, lastAccessTimeUtc, lastWriteTimeUtc, PathFormat.Relative);
      }

      #endregion // Transacted

      #endregion // SetTimestampsUtc

      #region TransferTimestamps

      #region IsFullPath

      /// <summary>[AlphaFS] Transfers the date and time stamps for the specified directories.</summary>
      /// <param name="sourcePath">The source directory to get the date and time stamps from.</param>
      /// <param name="destinationPath">The destination directory to set the date and time stamps.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <remarks>This method uses BackupSemantics flag to get Timestamp changed for directories.</remarks>
      [SecurityCritical]
      public static void TransferTimestamps(string sourcePath, string destinationPath, PathFormat pathFormat)
      {
         File.TransferTimestampsInternal(true, null, sourcePath, destinationPath, pathFormat);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Transfers the date and time stamps for the specified directories.</summary>
      /// <param name="sourcePath">The source directory to get the date and time stamps from.</param>
      /// <param name="destinationPath">The destination directory to set the date and time stamps.</param>
      /// <remarks>This method uses BackupSemantics flag to get Timestamp changed for directories.</remarks>
      [SecurityCritical]
      public static void TransferTimestamps(string sourcePath, string destinationPath)
      {
         File.TransferTimestampsInternal(true, null, sourcePath, destinationPath, PathFormat.Relative);
      }

      #region Transacted

      #region IsFullPath

      /// <summary>[AlphaFS] Transfers the date and time stamps for the specified directories.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourcePath">The source directory to get the date and time stamps from.</param>
      /// <param name="destinationPath">The destination directory to set the date and time stamps.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <remarks>This method uses BackupSemantics flag to get Timestamp changed for directories.</remarks>
      [SecurityCritical]
      public static void TransferTimestamps(KernelTransaction transaction, string sourcePath, string destinationPath, PathFormat pathFormat)
      {
         File.TransferTimestampsInternal(true, transaction, sourcePath, destinationPath, pathFormat);
      }

      #endregion // IsFullPath

      /// <summary>[AlphaFS] Transfers the date and time stamps for the specified directories.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="sourcePath">The source directory to get the date and time stamps from.</param>
      /// <param name="destinationPath">The destination directory to set the date and time stamps.</param>
      /// <remarks>This method uses BackupSemantics flag to get Timestamp changed for directories.</remarks>
      [SecurityCritical]
      public static void TransferTimestamps(KernelTransaction transaction, string sourcePath, string destinationPath)
      {
         File.TransferTimestampsInternal(true, transaction, sourcePath, destinationPath, PathFormat.Relative);
      }

      #endregion // Transacted

      #endregion // TransferTimestamps


      #region Unified Internals


      #region DeleteEmptyDirectoryInternal

      /// <summary>[AlphaFS] Unified method DeleteEmptyDirectoryInternal() to delete empty subdirectores from the specified directory.</summary>
      /// <remarks>Trailing spaces are removed from the end of the <paramref name="path"/> parameter before deleting the empty directory.</remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <see langword="null"/>.</exception>
      /// <exception cref="DirectoryNotFoundException ">path is <see langword="null"/>.</exception>
      /// <param name="fileSystemEntryInfo">A FileSystemEntryInfo instance. Use either <paramref name="fileSystemEntryInfo"/> or <paramref name="path"/>, not both.</param>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The name of the directory to remove empty subdirectories from. Use either <paramref name="path"/> or <paramref name="fileSystemEntryInfo"/>, not both.</param>
      /// <param name="recursive"><see langword="true"/> deletes empty subdirectories from this directory and its subdirectories.</param>
      /// <param name="ignoreReadOnly"><see langword="true"/> overrides read only <see cref="FileAttributes"/> of empty directories.</param>
      /// <param name="initialize">When <see langword="true"/> indicates the method is called externally.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      internal static void DeleteEmptyDirectoryInternal(FileSystemEntryInfo fileSystemEntryInfo, KernelTransaction transaction, string path, bool recursive, bool ignoreReadOnly, bool initialize, PathFormat pathFormat)
      {
         #region Setup

         if (pathFormat == PathFormat.Relative)
            Path.CheckValidPath(path, true, true);

         if (fileSystemEntryInfo == null)
         {
            if (!File.ExistsInternal(true, transaction, path, pathFormat))
               NativeError.ThrowException(Win32Errors.ERROR_PATH_NOT_FOUND, path);

            fileSystemEntryInfo = File.GetFileSystemEntryInfoInternal(transaction,
               Path.GetExtendedLengthPathInternal(transaction, path, pathFormat, GetFullPathOptions.TrimEnd | GetFullPathOptions.RemoveTrailingDirectorySeparator | GetFullPathOptions.CheckInvalidPathChars | GetFullPathOptions.CheckAdditional)
               , false, PathFormat.LongFullPath);
         }

         if (fileSystemEntryInfo == null)
            throw new ArgumentNullException("path");

         string pathLp = fileSystemEntryInfo.LongFullPath;

         #endregion // Setup

         // Ensure path is a directory.
         if (!fileSystemEntryInfo.IsDirectory)
            throw new IOException(String.Format(CultureInfo.CurrentCulture, Resources.FileExistsWithSameNameSpecifiedByPath, pathLp));

         DirectoryEnumerationOptions dirEnumOptions = DirectoryEnumerationOptions.Folders;
         if (recursive)
            dirEnumOptions |= DirectoryEnumerationOptions.Recursive;

         foreach (FileSystemEntryInfo fsei in EnumerateFileSystemEntryInfosInternal<FileSystemEntryInfo>(transaction, pathLp, Path.WildcardStarMatchAll, dirEnumOptions, PathFormat.LongFullPath))
            DeleteEmptyDirectoryInternal(fsei, transaction, null, recursive, ignoreReadOnly, false, PathFormat.LongFullPath);


         if (!EnumerateFileSystemEntryInfosInternal<string>(transaction, pathLp, Path.WildcardStarMatchAll, DirectoryEnumerationOptions.FilesAndFolders, PathFormat.LongFullPath).Any())
         {
            // Prevent deleting path itself.
            if (!initialize)
               DeleteDirectoryInternal(fileSystemEntryInfo, transaction, null, false, ignoreReadOnly, true, true, PathFormat.LongFullPath);
         }
      }

      #endregion // DeleteEmptyDirectoryInternal

      #region EnumerateFileIdBothDirectoryInfoInternal

      /// <summary>[AlphaFS] Unified method EnumerateFileIdBothDirectoryInfoInternal() to return an enumerable collection of information about files in the directory handle specified.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="safeHandle">An open handle to the directory from which to retrieve information.</param>
      /// <param name="path">A path to the directory.</param>
      /// <param name="shareMode">The <see cref="FileShare"/> mode with which to open a handle to the directory.</param>
      /// <param name="continueOnException">
      /// <para><see langword="true"/> suppress any Exception that might be thrown a result from a failure,</para>
      /// <para>such as ACLs protected directories or non-accessible reparse points.</para>
      /// </param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>An IEnumerable of <see cref="FileIdBothDirectoryInfo"/> records for each file system entry in the specified diretory.</returns>    
      /// <remarks>Either use <paramref name="path"/> or <paramref name="safeHandle"/>, not both.</remarks>
      [SecurityCritical]
      internal static IEnumerable<FileIdBothDirectoryInfo> EnumerateFileIdBothDirectoryInfoInternal(KernelTransaction transaction, SafeFileHandle safeHandle, string path, FileShare shareMode, bool continueOnException, PathFormat pathFormat)
      {
         if (!NativeMethods.IsAtLeastWindowsVista)
            throw new PlatformNotSupportedException(Resources.RequiresWindowsVistaOrHigher);

         bool callerHandle = safeHandle != null;
         if (!callerHandle)
         {
            if (Utils.IsNullOrWhiteSpace(path))
               throw new ArgumentNullException("path");

            string pathLp = Path.GetExtendedLengthPathInternal(transaction, path, pathFormat, GetFullPathOptions.RemoveTrailingDirectorySeparator | GetFullPathOptions.CheckInvalidPathChars | GetFullPathOptions.CheckAdditional);
            safeHandle = File.CreateFileInternal(transaction, pathLp, ExtendedFileAttributes.BackupSemantics, null, FileMode.Open, FileSystemRights.ReadData, shareMode, true, PathFormat.LongFullPath);
         }


         try
         {
            if (!NativeMethods.IsValidHandle(safeHandle, Marshal.GetLastWin32Error(), !continueOnException))
               yield break;

            // 2014-10-16: Number of returned items depends on the size of the buffer.
            // That does not seem right, investigate.
            using (SafeGlobalMemoryBufferHandle safeBuffer = new SafeGlobalMemoryBufferHandle(NativeMethods.DefaultFileBufferSize))
            {
               NativeMethods.IsValidHandle(safeBuffer, Marshal.GetLastWin32Error());

               long fileNameOffset = Marshal.OffsetOf(typeof(NativeMethods.FileIdBothDirInfo), "FileName").ToInt64();

               while (NativeMethods.GetFileInformationByHandleEx(safeHandle, NativeMethods.FileInfoByHandleClass.FileIdBothDirectoryInfo, safeBuffer, NativeMethods.DefaultFileBufferSize))
               {
                  // CA2001:AvoidCallingProblematicMethods

                  IntPtr buffer = IntPtr.Zero;
                  bool successRef = false;
                  safeBuffer.DangerousAddRef(ref successRef);

                  // MSDN: The DangerousGetHandle method poses a security risk because it can return a handle that is not valid.
                  if (successRef)
                     buffer = safeBuffer.DangerousGetHandle();

                  safeBuffer.DangerousRelease();

                  if (buffer == IntPtr.Zero)
                     NativeError.ThrowException(Resources.HandleDangerousRef);

                  // CA2001:AvoidCallingProblematicMethods


                  while (buffer != IntPtr.Zero)
                  {
                     NativeMethods.FileIdBothDirInfo fibdi = Utils.MarshalPtrToStructure<NativeMethods.FileIdBothDirInfo>(0, buffer);

                     string fileName = Marshal.PtrToStringUni(new IntPtr(fileNameOffset + buffer.ToInt64()), (int)(fibdi.FileNameLength / 2));

                     if (!Utils.IsNullOrWhiteSpace(fileName) &&
                         !fileName.Equals(Path.CurrentDirectoryPrefix, StringComparison.OrdinalIgnoreCase) &&
                         !fileName.Equals(Path.ParentDirectoryPrefix, StringComparison.OrdinalIgnoreCase))
                        yield return new FileIdBothDirectoryInfo(fibdi, fileName);


                     buffer = fibdi.NextEntryOffset == 0
                        ? IntPtr.Zero
                        : new IntPtr(buffer.ToInt64() + fibdi.NextEntryOffset);
                  }
               }

               int lastError = Marshal.GetLastWin32Error();
               switch ((uint)lastError)
               {
                  case Win32Errors.ERROR_SUCCESS:
                  case Win32Errors.ERROR_NO_MORE_FILES:
                  case Win32Errors.ERROR_HANDLE_EOF:
                     yield break;

                  default:
                     NativeError.ThrowException(lastError, path);
                     break;
               }
            }
         }
         finally
         {
            // Handle is ours, dispose.
            if (!callerHandle && safeHandle != null)
               safeHandle.Close();
         }
      }

      #endregion // EnumerateFileIdBothDirectoryInfoInternal

      #region GetPropertiesInternal

      /// <summary>[AlphaFS] Unified method GetPropertiesInternal() to gets the properties of the particular directory without following any symbolic links or mount points.
      /// Properties include aggregated info from <see cref="FileAttributes"/> of each encountered file system object.
      /// Plus additional ones: "Total", "File", "Size" and "SizeCompressed".
      /// <para><b>Total:</b> is the total number of enumerated objects.</para>
      /// <para><b>File:</b> is the total number of files. File is considered when object is neither <see cref="FileAttributes.Directory"/> nor <see cref="FileAttributes.ReparsePoint"/>.</para>
      /// <para><b>Size:</b> is the total size of enumerated objects.</para>
      /// <para><b>Size:</b> is the total compressed size of enumerated objects.</para>
      /// <para><b>Error:</b> is the total number of errors encountered during enumeration.</para>
      /// </summary>
      /// <remarks><b>Directory:</b> is an object which has <see cref="FileAttributes.Directory"/> attribute without <see cref="FileAttributes.ReparsePoint"/> one.</remarks>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The target directory.</param>
      /// <param name="directoryEnumerationOptions"><see cref="DirectoryEnumerationOptions"/> flags that specify how the directory is to be enumerated.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      /// <returns>A dictionary mapping the keys mentioned above to their respective aggregated values.</returns>
      [SecurityCritical]
      internal static Dictionary<string, long> GetPropertiesInternal(KernelTransaction transaction, string path, DirectoryEnumerationOptions directoryEnumerationOptions, PathFormat pathFormat)
      {
         const string propFile = "File";
         const string propTotal = "Total";
         const string propSize = "Size";
         long total = 0;
         long size = 0;
         Type typeOfAttrs = typeof(FileAttributes);
         Array attributes = Enum.GetValues(typeOfAttrs);
         Dictionary<string, long> props = Enum.GetNames(typeOfAttrs).OrderBy(attrs => attrs).ToDictionary<string, string, long>(name => name, name => 0);

         string pathLp = Path.GetExtendedLengthPathInternal(transaction, path, pathFormat, GetFullPathOptions.RemoveTrailingDirectorySeparator | GetFullPathOptions.CheckInvalidPathChars | GetFullPathOptions.CheckAdditional);

         foreach (FileSystemEntryInfo fsei in EnumerateFileSystemEntryInfosInternal<FileSystemEntryInfo>(transaction, pathLp, Path.WildcardStarMatchAll, directoryEnumerationOptions, PathFormat.LongFullPath))
         {
            total++;

            if (!fsei.IsDirectory)
               size += fsei.FileSize;

            foreach (FileAttributes attributeMarker in attributes)
            {
               // Marker exists in flags.
               if ((fsei.Attributes & attributeMarker) == attributeMarker)

                  // Regular directory that will go to stack, adding directory flag ++
                  props[(((attributeMarker & FileAttributes.Directory) == FileAttributes.Directory) ? FileAttributes.Directory : attributeMarker).ToString()]++;
            }
         }

         // Adjust regular files count.
         props.Add(propFile, total - props[FileAttributes.Directory.ToString()] - props[FileAttributes.ReparsePoint.ToString()]);
         props.Add(propTotal, total);
         props.Add(propSize, size);

         return props;
      }

      #endregion // GetPropertiesInternal

      #endregion // Unified Internals

      #endregion // AlphaFS
   }
}