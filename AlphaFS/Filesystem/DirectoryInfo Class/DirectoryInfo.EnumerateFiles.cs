/*  Copyright (C) 2008-2015 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
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

using System.Collections.Generic;
using System.IO;
using System.Security;

namespace Alphaleonis.Win32.Filesystem
{
   partial class DirectoryInfo
   {
      #region .NET

      /// <summary>Returns an enumerable collection of file information in the current directory.</summary>
      /// <returns>An enumerable collection of the files in the current directory.</returns>
      [SecurityCritical]
      public IEnumerable<FileInfo> EnumerateFiles()
      {
         return Directory.EnumerateFileSystemEntryInfosInternal<FileInfo>(Transaction, LongFullName, Path.WildcardStarMatchAll, DirectoryEnumerationOptions.Files, PathFormat.LongFullPath);
      }

      /// <summary>Returns an enumerable collection of file information that matches a search pattern.</summary>
      /// <param name="searchPattern">
      ///   <para>The search string to match against the names of directories. This parameter can contain a</para>
      ///   <para>combination of valid literal path and wildcard (<see cref="Path.WildcardStarMatchAll"/> and <see cref="Path.WildcardQuestion"/>)</para>
      ///   <para>characters, but does not support regular expressions.</para>
      /// </param>
      /// <returns>An enumerable collection of files that matches <paramref name="searchPattern"/>.</returns>
      [SecurityCritical]
      public IEnumerable<FileInfo> EnumerateFiles(string searchPattern)
      {
         return Directory.EnumerateFileSystemEntryInfosInternal<FileInfo>(Transaction, LongFullName, searchPattern, DirectoryEnumerationOptions.Files, PathFormat.LongFullPath);
      }

      /// <summary>Returns an enumerable collection of file information that matches a specified search pattern and search subdirectory option.</summary>
      /// <param name="searchPattern">
      ///   <para>The search string to match against the names of directories. This parameter can contain a</para>
      ///   <para>combination of valid literal path and wildcard (<see cref="Path.WildcardStarMatchAll"/> and <see cref="Path.WildcardQuestion"/>)</para>
      ///   <para>characters, but does not support regular expressions.</para>
      /// </param>
      /// <param name="searchOption">
      ///   <para>One of the <see cref="SearchOption"/> enumeration values that specifies whether the <paramref name="searchOption"/></para>
      ///   <para> should include only the current directory or should include all subdirectories.</para>
      /// </param>
      /// <returns>An enumerable collection of files that matches <paramref name="searchPattern"/> and <paramref name="searchOption"/>.</returns>
      [SecurityCritical]
      public IEnumerable<FileInfo> EnumerateFiles(string searchPattern, SearchOption searchOption)
      {
         var options = DirectoryEnumerationOptions.Files | ((searchOption == SearchOption.AllDirectories) ? DirectoryEnumerationOptions.Recursive : 0);

         return Directory.EnumerateFileSystemEntryInfosInternal<FileInfo>(Transaction, LongFullName, searchPattern, options, PathFormat.LongFullPath);
      }

      #endregion // .NET
   }
}