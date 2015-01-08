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

using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Security;

namespace Alphaleonis.Win32.Filesystem
{
   partial class DirectoryInfo
   {
      #region .NET

      /// <summary>Returns an array of strongly typed <see cref="FileSystemInfo"/> entries representing all the files and subdirectories in a directory.</summary>
      /// <returns>An array of strongly typed <see cref="FileSystemInfo"/> entries.</returns>
      /// <remarks>
      /// For subdirectories, the <see cref="FileSystemInfo"/> objects returned by this method can be cast to the derived class <see cref="DirectoryInfo"/>.
      /// Use the <see cref="FileAttributes"/> value returned by the <see cref="FileSystemInfo.Attributes"/> property to determine whether the <see cref="FileSystemInfo"/> represents a file or a directory.
      /// </remarks>
      /// <remarks>
      /// If there are no files or directories in the DirectoryInfo, this method returns an empty array. This method is not recursive.
      /// For subdirectories, the FileSystemInfo objects returned by this method can be cast to the derived class DirectoryInfo.
      /// Use the FileAttributes value returned by the Attributes property to determine whether the FileSystemInfo represents a file or a directory.
      /// </remarks>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
      [SecurityCritical]
      public FileSystemInfo[] GetFileSystemInfos()
      {
         return Directory.EnumerateFileSystemEntryInfosInternal<FileSystemInfo>(Transaction, LongFullName, Path.WildcardStarMatchAll, DirectoryEnumerationOptions.FilesAndFolders, PathFormat.LongFullPath).ToArray();
      }

      /// <summary>Retrieves an array of strongly typed <see cref="FileSystemInfo"/> objects representing the files and subdirectories that match the specified search criteria.</summary>
      /// <param name="searchPattern">
      ///   <para>The search string to match against the names of directories. This parameter can contain a</para>
      ///   <para>combination of valid literal path and wildcard (<see cref="Path.WildcardStarMatchAll"/> and <see cref="Path.WildcardQuestion"/>)</para>
      ///   <para>characters, but does not support regular expressions.</para>
      /// </param>
      /// <returns>An array of strongly typed <see cref="FileSystemInfo"/> entries.</returns>
      /// <remarks>
      /// For subdirectories, the <see cref="FileSystemInfo"/> objects returned by this method can be cast to the derived class <see cref="DirectoryInfo"/>.
      /// Use the <see cref="FileAttributes"/> value returned by the <see cref="FileSystemInfo.Attributes"/> property to determine whether the <see cref="FileSystemInfo"/> represents a file or a directory.
      /// </remarks>
      /// <remarks>
      /// If there are no files or directories in the DirectoryInfo, this method returns an empty array. This method is not recursive.
      /// For subdirectories, the FileSystemInfo objects returned by this method can be cast to the derived class DirectoryInfo.
      /// Use the FileAttributes value returned by the Attributes property to determine whether the FileSystemInfo represents a file or a directory.
      /// </remarks>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
      [SecurityCritical]
      public FileSystemInfo[] GetFileSystemInfos(string searchPattern)
      {
         return Directory.EnumerateFileSystemEntryInfosInternal<FileSystemInfo>(Transaction, LongFullName, searchPattern, DirectoryEnumerationOptions.FilesAndFolders, PathFormat.LongFullPath).ToArray();
      }

      /// <summary>Retrieves an array of strongly typed <see cref="FileSystemInfo"/> objects representing the files and subdirectories that match the specified search criteria.</summary>
      /// <param name="searchPattern">
      ///   <para>The search string to match against the names of directories. This parameter can contain a</para>
      ///   <para>combination of valid literal path and wildcard (<see cref="Path.WildcardStarMatchAll"/> and <see cref="Path.WildcardQuestion"/>)</para>
      ///   <para>characters, but does not support regular expressions.</para>
      /// </param>
      /// <param name="searchOption">
      ///   <para>One of the <see cref="SearchOption"/> enumeration values that specifies whether the <paramref name="searchOption"/></para>
      ///   <para> should include only the current directory or should include all subdirectories.</para>
      /// </param>
      /// <returns>An array of strongly typed <see cref="FileSystemInfo"/> entries.</returns>
      /// <remarks>
      /// For subdirectories, the <see cref="FileSystemInfo"/> objects returned by this method can be cast to the derived class <see cref="DirectoryInfo"/>.
      /// Use the <see cref="FileAttributes"/> value returned by the <see cref="FileSystemInfo.Attributes"/> property to determine whether the <see cref="FileSystemInfo"/> represents a file or a directory.
      /// </remarks>
      /// <remarks>
      /// If there are no files or directories in the DirectoryInfo, this method returns an empty array. This method is not recursive.
      /// For subdirectories, the FileSystemInfo objects returned by this method can be cast to the derived class DirectoryInfo.
      /// Use the FileAttributes value returned by the Attributes property to determine whether the FileSystemInfo represents a file or a directory.
      /// </remarks>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
      [SecurityCritical]
      public FileSystemInfo[] GetFileSystemInfos(string searchPattern, SearchOption searchOption)
      {
         var directoryEnumerationOptions = DirectoryEnumerationOptions.FilesAndFolders;

         if (searchOption == SearchOption.AllDirectories)
            directoryEnumerationOptions |= DirectoryEnumerationOptions.Recursive;

         return Directory.EnumerateFileSystemEntryInfosInternal<FileSystemInfo>(Transaction, LongFullName, searchPattern, directoryEnumerationOptions, PathFormat.LongFullPath).ToArray();
      }

      #endregion // .NET
   }
}