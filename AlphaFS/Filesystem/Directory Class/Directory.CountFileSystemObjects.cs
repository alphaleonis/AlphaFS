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

using System.Linq;
using System.Security;

namespace Alphaleonis.Win32.Filesystem
{
   partial class Directory
   {
      #region AlphaFS

      #region Non-Transactional

      /// <summary>[AlphaFS] Counts file system objects: files, folders or both) in a given directory.</summary>
      /// <returns>The counted number of file system objects.</returns>
      /// <exception cref="System.UnauthorizedAccessException">An exception is thrown case of access errors.</exception>
      /// <param name="path">The directory path.</param>
      /// <param name="options"><see cref="DirectoryEnumerationOptions"/> flags that specify how the directory is to be enumerated.</param>
      [SecurityCritical]
      public static long CountFileSystemObjects(string path, DirectoryEnumerationOptions options)
      {
         return EnumerateFileSystemEntryInfosInternal<string>(null, path, Path.WildcardStarMatchAll, options, PathFormat.RelativePath).Count();
      }

      /// <summary>[AlphaFS] Counts file system objects: files, folders or both) in a given directory.</summary>
      /// <returns>The counted number of file system objects.</returns>
      /// <exception cref="System.UnauthorizedAccessException">An exception is thrown case of access errors.</exception>
      /// <param name="path">The directory path.</param>
      /// <param name="options"><see cref="DirectoryEnumerationOptions"/> flags that specify how the directory is to be enumerated.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static long CountFileSystemObjects(string path, DirectoryEnumerationOptions options, PathFormat pathFormat)
      {
         return EnumerateFileSystemEntryInfosInternal<string>(null, path, Path.WildcardStarMatchAll, options, pathFormat).Count();
      }



      /// <summary>[AlphaFS] Counts file system objects: files, folders or both) in a given directory.</summary>
      /// <returns>The counted number of file system objects.</returns>
      /// <exception cref="System.UnauthorizedAccessException">An exception is thrown case of access errors.</exception>
      /// <param name="path">The directory path.</param>
      /// <param name="searchPattern">
      /// <para>The search string to match against the names of directories in path. This parameter can contain a</para>
      /// <para>combination of valid literal path and wildcard (<see cref="Path.WildcardStarMatchAll"/> and <see cref="Path.WildcardQuestion"/>)</para>
      /// <para>characters, but does not support regular expressions.</para>
      /// </param>
      /// <param name="options"><see cref="DirectoryEnumerationOptions"/> flags that specify how the directory is to be enumerated.</param>
      [SecurityCritical]
      public static long CountFileSystemObjects(string path, string searchPattern, DirectoryEnumerationOptions options)
      {
         return EnumerateFileSystemEntryInfosInternal<string>(null, path, searchPattern, options, PathFormat.RelativePath).Count();
      }

      /// <summary>[AlphaFS] Counts file system objects: files, folders or both) in a given directory.</summary>
      /// <returns>The counted number of file system objects.</returns>
      /// <exception cref="System.UnauthorizedAccessException">An exception is thrown case of access errors.</exception>
      /// <param name="path">The directory path.</param>
      /// <param name="searchPattern">
      /// <para>The search string to match against the names of directories in path. This parameter can contain a</para>
      /// <para>combination of valid literal path and wildcard (<see cref="Path.WildcardStarMatchAll"/> and <see cref="Path.WildcardQuestion"/>)</para>
      /// <para>characters, but does not support regular expressions.</para>
      /// </param>
      /// <param name="options"><see cref="DirectoryEnumerationOptions"/> flags that specify how the directory is to be enumerated.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static long CountFileSystemObjects(string path, string searchPattern, DirectoryEnumerationOptions options, PathFormat pathFormat)
      {
         return EnumerateFileSystemEntryInfosInternal<string>(null, path, searchPattern, options, pathFormat).Count();
      }

      #endregion // Non-Transactional

      #region Transactional

      /// <summary>[AlphaFS] Counts file system objects: files, folders or both) in a given directory.</summary>
      /// <returns>The counted number of file system objects.</returns>
      /// <exception cref="System.UnauthorizedAccessException">An exception is thrown case of access errors.</exception>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory path.</param>
      /// <param name="options"><see cref="DirectoryEnumerationOptions"/> flags that specify how the directory is to be enumerated.</param>
      [SecurityCritical]
      public static long CountFileSystemObjects(KernelTransaction transaction, string path, DirectoryEnumerationOptions options)
      {
         return EnumerateFileSystemEntryInfosInternal<string>(transaction, path, Path.WildcardStarMatchAll, options, PathFormat.RelativePath).Count();
      }

      /// <summary>[AlphaFS] Counts file system objects: files, folders or both) in a given directory.</summary>
      /// <returns>The counted number of file system objects.</returns>
      /// <exception cref="System.UnauthorizedAccessException">An exception is thrown case of access errors.</exception>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory path.</param>
      /// <param name="options"><see cref="DirectoryEnumerationOptions"/> flags that specify how the directory is to be enumerated.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static long CountFileSystemObjects(KernelTransaction transaction, string path, DirectoryEnumerationOptions options, PathFormat pathFormat)
      {
         return EnumerateFileSystemEntryInfosInternal<string>(transaction, path, Path.WildcardStarMatchAll, options, pathFormat).Count();
      }



      /// <summary>[AlphaFS] Counts file system objects: files, folders or both) in a given directory.</summary>
      /// <returns>The counted number of file system objects.</returns>
      /// <exception cref="System.UnauthorizedAccessException">An exception is thrown case of access errors.</exception>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory path.</param>
      /// <param name="searchPattern">
      /// <para>The search string to match against the names of directories in path. This parameter can contain a</para>
      /// <para>combination of valid literal path and wildcard (<see cref="Path.WildcardStarMatchAll"/> and <see cref="Path.WildcardQuestion"/>)</para>
      /// <para>characters, but does not support regular expressions.</para>
      /// </param>
      /// <param name="options"><see cref="DirectoryEnumerationOptions"/> flags that specify how the directory is to be enumerated.</param>
      [SecurityCritical]
      public static long CountFileSystemObjects(KernelTransaction transaction, string path, string searchPattern, DirectoryEnumerationOptions options)
      {
         return EnumerateFileSystemEntryInfosInternal<string>(transaction, path, searchPattern, options, PathFormat.RelativePath).Count();
      }

      /// <summary>[AlphaFS] Counts file system objects: files, folders or both) in a given directory.</summary>
      /// <returns>The counted number of file system objects.</returns>
      /// <exception cref="System.UnauthorizedAccessException">An exception is thrown case of access errors.</exception>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory path.</param>
      /// <param name="searchPattern">
      /// <para>The search string to match against the names of directories in path. This parameter can contain a</para>
      /// <para>combination of valid literal path and wildcard (<see cref="Path.WildcardStarMatchAll"/> and <see cref="Path.WildcardQuestion"/>)</para>
      /// <para>characters, but does not support regular expressions.</para>
      /// </param>
      /// <param name="options"><see cref="DirectoryEnumerationOptions"/> flags that specify how the directory is to be enumerated.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static long CountFileSystemObjects(KernelTransaction transaction, string path, string searchPattern, DirectoryEnumerationOptions options, PathFormat pathFormat)
      {
         return EnumerateFileSystemEntryInfosInternal<string>(transaction, path, searchPattern, options, pathFormat).Count();
      }
      
      #endregion // Transactional

      #endregion // AlphaFS
   }
}