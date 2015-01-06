using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using SearchOption = System.IO.SearchOption;

namespace Alphaleonis.Win32.Filesystem
{
   public static partial class Directory
   {
      #region EnumerateDirectories

      #region Non-Transactional

      /// <summary>Returns an enumerable collection of directory names in a specified <paramref name="path"/>.</summary>
      /// <param name="path">The directory to search.</param>
      /// <returns>An enumerable collection of the full names (including paths) for the directories in the directory specified by <paramref name="path"/>.</returns>
      [SecurityCritical]
      public static IEnumerable<string> EnumerateDirectories(string path)
      {
         return EnumerateFileSystemEntryInfosInternal<string>(null, path, Path.WildcardStarMatchAll, DirectoryEnumerationOptions.Folders, PathFormat.Relative);
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
         return EnumerateFileSystemEntryInfosInternal<string>(null, path, searchPattern, DirectoryEnumerationOptions.Folders, PathFormat.Relative);
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
         var directoryEnumerationOptions = DirectoryEnumerationOptions.Folders;

         if (searchOption == SearchOption.AllDirectories)
            directoryEnumerationOptions |= DirectoryEnumerationOptions.Recursive;

         return EnumerateFileSystemEntryInfosInternal<string>(null, path, searchPattern, directoryEnumerationOptions, PathFormat.Relative);
      }

      #endregion

      #region Transacted

      /// <summary>[AlphaFS] Returns an enumerable collection of directory instances in a specified <paramref name="path"/>.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory to search.</param>
      /// <returns>An enumerable collection of the full names (including paths) for the directories in the directory specified by <paramref name="path"/>.</returns>
      [SecurityCritical]
      public static IEnumerable<string> EnumerateDirectories(KernelTransaction transaction, string path)
      {
         return EnumerateFileSystemEntryInfosInternal<string>(transaction, path, Path.WildcardStarMatchAll, DirectoryEnumerationOptions.Folders, PathFormat.Relative);
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
         return EnumerateFileSystemEntryInfosInternal<string>(transaction, path, searchPattern, DirectoryEnumerationOptions.Folders, PathFormat.Relative);
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
         var directoryEnumerationOptions = DirectoryEnumerationOptions.Folders;

         if (searchOption == SearchOption.AllDirectories)
            directoryEnumerationOptions |= DirectoryEnumerationOptions.Recursive;

         return EnumerateFileSystemEntryInfosInternal<string>(transaction, path, searchPattern, directoryEnumerationOptions, PathFormat.Relative);
      }


      #endregion // Transacted

      #endregion // EnumerateDirectories

      #region EnumerateFiles

      #region Non-Transactional

      /// <summary>Returns an enumerable collection of file names in a specified <paramref name="path"/>.</summary>
      /// <param name="path">The directory to search.</param>
      /// <returns>An enumerable collection of the full names (including paths) for the files in the directory specified by <paramref name="path"/>.</returns>
      [SecurityCritical]
      public static IEnumerable<string> EnumerateFiles(string path)
      {
         return EnumerateFileSystemEntryInfosInternal<string>(null, path, Path.WildcardStarMatchAll, DirectoryEnumerationOptions.Files, PathFormat.Relative);
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
         return EnumerateFileSystemEntryInfosInternal<string>(null, path, searchPattern, DirectoryEnumerationOptions.Files, PathFormat.Relative);
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
         var directoryEnumerationOptions = DirectoryEnumerationOptions.Files;

         if (searchOption == SearchOption.AllDirectories)
            directoryEnumerationOptions |= DirectoryEnumerationOptions.Recursive;

         return EnumerateFileSystemEntryInfosInternal<string>(null, path, searchPattern, directoryEnumerationOptions, PathFormat.Relative);
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
         return EnumerateFileSystemEntryInfosInternal<string>(transaction, path, Path.WildcardStarMatchAll, DirectoryEnumerationOptions.Files, PathFormat.Relative);
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
         return EnumerateFileSystemEntryInfosInternal<string>(transaction, path, searchPattern, DirectoryEnumerationOptions.Files, PathFormat.Relative);
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
         var directoryEnumerationOptions = DirectoryEnumerationOptions.Files;

         if (searchOption == SearchOption.AllDirectories)
            directoryEnumerationOptions |= DirectoryEnumerationOptions.Recursive;

         return EnumerateFileSystemEntryInfosInternal<string>(transaction, path, searchPattern, directoryEnumerationOptions, PathFormat.Relative);
      }

      #endregion // Transacted

      #endregion // EnumerateFiles

      #region EnumerateFileSystemEntries

      #region Non-Transactional

      /// <summary>Returns an enumerable collection of file names and directory names in a specified <paramref name="path"/>.</summary>
      /// <param name="path">The directory to search.</param>
      /// <returns>An enumerable collection of <see cref="FileSystemEntryInfo"/> entries in the directory specified by <paramref name="path"/>.</returns>
      [SecurityCritical]
      public static IEnumerable<string> EnumerateFileSystemEntries(string path)
      {
         return EnumerateFileSystemEntryInfosInternal<string>(null, path, Path.WildcardStarMatchAll, DirectoryEnumerationOptions.FilesAndFolders, PathFormat.Relative);
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
         return EnumerateFileSystemEntryInfosInternal<string>(null, path, searchPattern, DirectoryEnumerationOptions.FilesAndFolders, PathFormat.Relative);
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
         var directoryEnumerationOptions = DirectoryEnumerationOptions.FilesAndFolders;

         if (searchOption == SearchOption.AllDirectories)
            directoryEnumerationOptions |= DirectoryEnumerationOptions.Recursive;

         return EnumerateFileSystemEntryInfosInternal<string>(null, path, searchPattern, directoryEnumerationOptions, PathFormat.Relative);
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
         return EnumerateFileSystemEntryInfosInternal<string>(transaction, path, Path.WildcardStarMatchAll, DirectoryEnumerationOptions.FilesAndFolders, PathFormat.Relative);
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
         return EnumerateFileSystemEntryInfosInternal<string>(transaction, path, searchPattern, DirectoryEnumerationOptions.FilesAndFolders, PathFormat.Relative);
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
         var directoryEnumerationOptions = DirectoryEnumerationOptions.FilesAndFolders;

         if (searchOption == SearchOption.AllDirectories)
            directoryEnumerationOptions |= DirectoryEnumerationOptions.Recursive;

         return EnumerateFileSystemEntryInfosInternal<string>(transaction, path, searchPattern, directoryEnumerationOptions, PathFormat.Relative);
      }

      #endregion // Transacted

      #endregion // EnumerateFileSystemEntries

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
      /// <param name="directoryEnumerationOptions"><see cref="DirectoryEnumerationOptions"/> flags that specify how the directory is to be enumerated.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      internal static IEnumerable<T> EnumerateFileSystemEntryInfosInternal<T>(KernelTransaction transaction, string path, string searchPattern, DirectoryEnumerationOptions directoryEnumerationOptions, PathFormat pathFormat)
      {
         // Enable BasicSearch by default, when absent.
         if ((directoryEnumerationOptions & DirectoryEnumerationOptions.BasicSearch) != DirectoryEnumerationOptions.BasicSearch)
            directoryEnumerationOptions |= DirectoryEnumerationOptions.BasicSearch;

         // Enable LargeCache by default, when absent.
         if ((directoryEnumerationOptions & DirectoryEnumerationOptions.LargeCache) != DirectoryEnumerationOptions.LargeCache)
            directoryEnumerationOptions |= DirectoryEnumerationOptions.LargeCache;

         return (new FindFileSystemEntryInfo(true, transaction, path, searchPattern, directoryEnumerationOptions, typeof(T), pathFormat)).Enumerate<T>();
      }

      #endregion // EnumerateFileSystemEntryInfosInternal
   }
}
