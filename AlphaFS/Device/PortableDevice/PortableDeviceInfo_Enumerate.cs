/*  Copyright (C) 2008-2018 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
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
using System.Security;
using DirectoryInfo = Alphaleonis.Win32.Filesystem.DirectoryInfo;
using Path = Alphaleonis.Win32.Filesystem.Path;

namespace Alphaleonis.Win32.Device
{
   public sealed partial class PortableDeviceInfo
   {
      #region EnumerateFileSystemEntryInfos

      /// <summary>[AlphaFS] Returns an enumerable collection of file names and directory names from the root of the device.
      /// <para>&#160;</para>
      /// <returns>An enumerable collection of file-system entries from the root of the device.</returns>
      /// </summary>
      [SecurityCritical]
      public IEnumerable<PortableDeviceFileSystemInfo> EnumerateFileSystemEntries()
      {
         return Local.EnumerateFileSystemEntryInfoCore(this, null, null, Path.WildcardStarMatchAll, System.IO.SearchOption.TopDirectoryOnly, null);
      }


      /// <summary>[AlphaFS] Returns an enumerable collection of file instances and directory instances in a specified path.
      /// <para>&#160;</para>
      /// <returns>An enumerable collection of file-system entries in the directory specified by <paramref name="objectId"/>.</returns>
      /// </summary>
      /// <param name="objectId">The ID of the directory to search.</param>
      [SecurityCritical]
      public IEnumerable<PortableDeviceFileSystemInfo> EnumerateFileSystemEntries(string objectId)
      {
         return Local.EnumerateFileSystemEntryInfoCore(this, null, objectId, Path.WildcardStarMatchAll, System.IO.SearchOption.TopDirectoryOnly, null);
      }


      /// <summary>[AlphaFS] Returns an enumerable collection of file instances and directory instances that matches a specified search pattern and search subdirectory option.
      /// <para>&#160;</para>
      /// <returns>An enumerable collection of file-system entries in the directory specified by <paramref name="objectId"/>.</returns>
      /// </summary>
      /// <param name="objectId">The ID of the directory to search.</param>
      /// <param name="searchPattern">The search string to match against the names of directories. This parameter can contain a combination of valid literal path and wildcard (<see cref="T:Path.WildcardStarMatchAll"/> and <see cref="T:Path.WildcardQuestion"/>) characters, but doesn't support regular expressions.</param>
      /// <param name="searchOption">One of the <see cref="T:SearchOption"/> enumeration values that specifies whether the <paramref name="searchOption"/> should include only the current directory or should include all subdirectories.</param>
      [SecurityCritical]
      public IEnumerable<PortableDeviceFileSystemInfo> EnumerateFileSystemEntries(string objectId, string searchPattern, System.IO.SearchOption searchOption)
      {
         return Local.EnumerateFileSystemEntryInfoCore(this, null, objectId, searchPattern, searchOption, null);
      }

      #endregion // EnumerateFileSystemEntryInfos


      //#region EnumerateDirectories

      ///// <summary>[AlphaFS] Returns an enumerable collection of directory information from the root of the device.
      ///// <para>&#160;</para>
      ///// <returns>An enumerable collection of files and directories from the root of the device.</returns>
      ///// </summary>
      //public IEnumerable<PortableDeviceFileSystemInfo> EnumerateDirectories()
      //{
      //   return Local.EnumerateFileSystemEntryInfoCore(this, null, null, Path.WildcardStarMatchAll, SearchOption.TopDirectoryOnly, true);
      //}

      ///// <summary>[AlphaFS] Returns an enumerable collection of directory information in the directory specified by <paramref name="objectId"/>.
      ///// <para>&#160;</para>
      ///// <returns>An enumerable collection of files and directories in the directory specified by <paramref name="objectId"/>.</returns>
      ///// </summary>
      ///// <param name="objectId">The ID of the directory to search.</param>
      //public IEnumerable<PortableDeviceFileSystemInfo> EnumerateDirectories(string objectId)
      //{
      //   return Local.EnumerateFileSystemEntryInfoCore(this, null, objectId, Path.WildcardStarMatchAll, SearchOption.TopDirectoryOnly, true);
      //}

      ///// <summary>[AlphaFS] Returns an enumerable collection of directory information that matches a specified search pattern.
      ///// <returns>An enumerable collection of directories that matches <paramref name="searchPattern"/>.</returns>
      ///// </summary>
      ///// <param name="objectId">The ID of the directory to search.</param>
      ///// <param name="searchPattern">The search string to match against the names of directories. This parameter can contain a combination of valid literal path and wildcard (<see cref="T:Path.WildcardStarMatchAll"/> and <see cref="T:Path.WildcardQuestion"/>) characters, but doesn't support regular expressions.</param>
      //public IEnumerable<PortableDeviceFileSystemInfo> EnumerateDirectories(string objectId, string searchPattern)
      //{
      //   return Local.EnumerateFileSystemEntryInfoCore(this, null, objectId, searchPattern, SearchOption.TopDirectoryOnly, true);
      //}

      ///// <summary>[AlphaFS] Returns an enumerable collection of directory information that matches a specified search pattern and search subdirectory option.
      ///// <returns>An enumerable collection of directories that matches <paramref name="searchPattern"/> and <paramref name="searchOption"/>.</returns>
      ///// </summary>
      ///// <param name="objectId">The ID of the directory to search.</param>
      ///// <param name="searchPattern">The search string to match against the names of directories. This parameter can contain a combination of valid literal path and wildcard (<see cref="T:Path.WildcardStarMatchAll"/> and <see cref="T:Path.WildcardQuestion"/>) characters, but doesn't support regular expressions.</param>
      ///// <param name="searchOption">One of the <see cref="T:SearchOption"/> enumeration values that specifies whether the <paramref name="searchOption"/> should include only the current directory or should include all subdirectories.</param>
      //public IEnumerable<PortableDeviceFileSystemInfo> EnumerateDirectories(string objectId, string searchPattern, SearchOption searchOption)
      //{
      //   return Local.EnumerateFileSystemEntryInfoCore(this, null, objectId, searchPattern, searchOption, true);
      //}

      ///// <summary>[AlphaFS] Returns an enumerable collection of directory information in the current directory.
      ///// <returns>An enumerable collection of files and directories in the current directory.</returns>
      ///// </summary>
      ///// <param name="deviceContent">A Portable Device Content instance.</param>
      //public IEnumerable<PortableDeviceFileSystemInfo> EnumerateDirectories(IPortableDeviceContent deviceContent)
      //{
      //   return Local.EnumerateFileSystemEntryInfoCore(this, deviceContent, null, Path.WildcardStarMatchAll, SearchOption.TopDirectoryOnly, true);
      //}

      ///// <summary>[AlphaFS] Returns an enumerable collection of directory information in the current directory.
      ///// <returns>An enumerable collection of files and directories in the current directory.</returns>
      ///// </summary>
      ///// <param name="deviceContent">A Portable Device Content instance.</param>
      ///// <param name="objectId">The ID of the directory to search.</param>
      //public IEnumerable<PortableDeviceFileSystemInfo> EnumerateDirectories(IPortableDeviceContent deviceContent, string objectId)
      //{
      //   return Local.EnumerateFileSystemEntryInfoCore(this, deviceContent, objectId, Path.WildcardStarMatchAll, SearchOption.TopDirectoryOnly, true);
      //}

      ///// <summary>[AlphaFS] Returns an enumerable collection of directory information that matches a specified search pattern.
      ///// <returns>An enumerable collection of directories that matches <paramref name="searchPattern"/>.</returns>
      ///// </summary>
      ///// <param name="deviceContent">A Portable Device Content instance.</param>
      ///// <param name="objectId">The ID of the directory to search.</param>
      ///// <param name="searchPattern">The search string to match against the names of directories. This parameter can contain a combination of valid literal path and wildcard (<see cref="T:Path.WildcardStarMatchAll"/> and <see cref="T:Path.WildcardQuestion"/>) characters, but doesn't support regular expressions.</param>
      //public IEnumerable<PortableDeviceFileSystemInfo> EnumerateDirectories(IPortableDeviceContent deviceContent, string objectId, string searchPattern)
      //{
      //   return Local.EnumerateFileSystemEntryInfoCore(this, deviceContent, objectId, searchPattern, SearchOption.TopDirectoryOnly, true);
      //}

      ///// <summary>[AlphaFS] Returns an enumerable collection of directory information that matches a specified search pattern and search subdirectory option.
      ///// <returns>An enumerable collection of directories that matches <paramref name="searchPattern"/> and <paramref name="searchOption"/>.</returns>
      ///// </summary>
      ///// <param name="deviceContent">A Portable Device Content instance.</param>
      ///// <param name="objectId">The ID of the directory to search.</param>
      ///// <param name="searchPattern">The search string to match against the names of directories. This parameter can contain a combination of valid literal path and wildcard (<see cref="T:Path.WildcardStarMatchAll"/> and <see cref="T:Path.WildcardQuestion"/>) characters, but doesn't support regular expressions.</param>
      ///// <param name="searchOption">One of the <see cref="T:SearchOption"/> enumeration values that specifies whether the <paramref name="searchOption"/> should include only the current directory or should include all subdirectories.</param>
      //public IEnumerable<PortableDeviceFileSystemInfo> EnumerateDirectories(IPortableDeviceContent deviceContent, string objectId, string searchPattern, SearchOption searchOption)
      //{
      //   return Local.EnumerateFileSystemEntryInfoCore(this, deviceContent, objectId, searchPattern, searchOption, true);
      //}

      //#endregion // EnumerateDirectories


      #region EnumerateFiles

      /// <summary>[AlphaFS] Returns an enumerable collection of file information in the current directory.</summary>
      /// <returns>An enumerable collection of the files in the current directory.</returns>
      public IEnumerable<DirectoryInfo> EnumerateFiles()
      {
         return null;
      }

      #endregion // EnumerateFiles
   }
}
