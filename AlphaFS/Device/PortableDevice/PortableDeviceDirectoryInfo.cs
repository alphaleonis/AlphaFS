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

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.AccessControl;
using Alphaleonis.Win32.Filesystem;
using Path = Alphaleonis.Win32.Filesystem.Path;
using Directory = Alphaleonis.Win32.Filesystem.Directory;
using File = Alphaleonis.Win32.Filesystem.File;
using FileInfo = Alphaleonis.Win32.Filesystem.FileInfo;
using FileSystemInfo = Alphaleonis.Win32.Filesystem.FileSystemInfo;

namespace Alphaleonis.Win32.Device
{
   /// <summary>Exposes instance methods for creating, moving, and enumerating through directories and subdirectories. This class cannot be inherited.</summary>
   [SerializableAttribute]
   public sealed class PortableDeviceDirectoryInfo : PortableDeviceFileSystemInfo
   {
      #region Constructors

      #region PortableDeviceDirectoryInfo

      /// <summary>Initializes a new instance of the <see cref="T:Alphaleonis.Win32.Filesystem.PortableDeviceDirectoryInfo"/> class on the specified path.</summary>
      /// <param name="fullName">The path on which to create the <see cref="T:Alphaleonis.Win32.Filesystem.PortableDeviceDirectoryInfo"/>.</param>
      /// <remarks>
      /// This constructor does not check if a directory exists. This constructor is a placeholder for a string that is used to access the disk in subsequent operations.
      /// The path parameter can be a file name, including a file on a Universal Naming Convention (UNC) share.
      /// </remarks>
      public PortableDeviceDirectoryInfo(string fullName)
      {
         InitializeInternal(true, fullName, null);

         Name = Path.GetFileName(Path.RemoveTrailingDirectorySeparator(fullName, false), false);
      }

      /// <summary>Initializes a new instance of the <see cref="T:Alphaleonis.Win32.Filesystem.PortableDeviceDirectoryInfo"/> class on the specified path.</summary>
      /// <param name="objectId">The path on which to create the <see cref="T:Alphaleonis.Win32.Filesystem.PortableDeviceDirectoryInfo"/>.</param>
      /// <param name="name"></param>
      /// <remarks>
      /// This constructor does not check if a directory exists. This constructor is a placeholder for a string that is used to access the disk in subsequent operations.
      /// The path parameter can be a file name, including a file on a Universal Naming Convention (UNC) share.
      /// </remarks>
      internal PortableDeviceDirectoryInfo(string objectId, string name)
      {
         InitializeInternal(true, objectId, null);

         Name = name;
      }

      /// <summary>Initializes a new instance of the <see cref="T:Alphaleonis.Win32.Filesystem.PortableDeviceDirectoryInfo"/> class on the specified path.</summary>
      /// <param name="objectId">The path on which to create the <see cref="T:Alphaleonis.Win32.Filesystem.PortableDeviceDirectoryInfo"/>.</param>
      /// <param name="name"></param>
      /// <param name="fullName"></param>
      /// <remarks>
      /// This constructor does not check if a directory exists. This constructor is a placeholder for a string that is used to access the disk in subsequent operations.
      /// The path parameter can be a file name, including a file on a Universal Naming Convention (UNC) share.
      /// </remarks>
      internal PortableDeviceDirectoryInfo(string objectId, string name, string fullName)
      {
         InitializeInternal(true, objectId, fullName);

         Name = name;
      }

      #region AlphaFS

      /// <summary>[AlphaFS] Special internal implementation.</summary>
      /// <param name="fullName">The full path on which to create the <see cref="T:Alphaleonis.Win32.Filesystem.PortableDeviceDirectoryInfo"/>.</param>
      /// <param name="junk1">Not used.</param>
      /// <param name="junk2">Not used.</param>
      /// <remarks>This constructor does not check if a directory exists. This constructor is a placeholder for a string that is used to access the disk in subsequent operations.</remarks>
      [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "junk1")]
      [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "junk2")]
      private PortableDeviceDirectoryInfo(string fullName, bool junk1, bool junk2)
      {
         IsDirectory = true;

         OriginalPath = Path.GetFileName(fullName, true);

         FullPath = fullName;

         DisplayPath = GetDisplayName(OriginalPath);
      }

      #endregion // AlphaFS

      #endregion // PortableDeviceDirectoryInfo

      #endregion // Constructors

      #region Methods

      #region .NET

      #region Create

      #region .NET

      /// <summary>Creates a directory.</summary>
      /// <remarks>If the directory already exists, this method does nothing.</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public void Create()
      {
         //Directory.CreateDirectoryInternal(null, null, null, null, false, null);
      }

      /// <summary>Creates a directory using a <see cref="T:System.Security.AccessControl.DirectorySecurity"/> object.</summary>
      /// <param name="directorySecurity">The access control to apply to the directory.</param>
      /// <remarks>If the directory already exists, this method does nothing.</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
      [SecurityCritical]
      public void Create(DirectorySecurity directorySecurity)
      {
         //Directory.CreateDirectoryInternal(null, null, null, directorySecurity, false, null);
      }

      #endregion // .NET

      #region AlphaFS

      /// <summary>[AlphaFS] Creates a directory using a <see cref="T:System.Security.AccessControl.DirectorySecurity"/> object.</summary>
      /// <param name="compress">When <c>true</c> compresses the directory.</param>
      /// <remarks>If the directory already exists, this method does nothing.</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
      [SecurityCritical]
      public void Create(bool compress)
      {
         //Directory.CreateDirectoryInternal(null, null, null, null, compress, null);
      }

      /// <summary>[AlphaFS] Creates a directory using a <see cref="T:System.Security.AccessControl.DirectorySecurity"/> object.</summary>
      /// <param name="directorySecurity">The access control to apply to the directory.</param>
      /// <param name="compress">When <c>true</c> compresses the directory.</param>
      /// <remarks>If the directory already exists, this method does nothing.</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
      [SecurityCritical]
      public void Create(DirectorySecurity directorySecurity, bool compress)
      {
         //Directory.CreateDirectoryInternal(null, null, null, directorySecurity, compress, null);
      }

      #endregion // AlphaFS

      #endregion // Create

      #region CreateSubdirectory

      #region .NET

      /// <summary>Creates a subdirectory or subdirectories on the specified path. The specified path can be relative to this instance of the <see cref="T:PortableDeviceDirectoryInfo"/> class.</summary>
      /// <param name="path">The specified path. This cannot be a different disk volume.</param>
      /// <returns>The last directory specified in <paramref name="path"/>.</returns>
      /// <remarks>
      /// Any and all directories specified in path are created, unless some part of path is invalid.
      /// The path parameter specifies a directory path, not a file path.
      /// If the subdirectory already exists, this method does nothing.
      /// </remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public PortableDeviceDirectoryInfo CreateSubdirectory(string path)
      {
         return CreateSubdirectoryInternal(path, null, null, false);
      }

      /// <summary>Creates a subdirectory or subdirectories on the specified path. The specified path can be relative to this instance of the <see cref="T:PortableDeviceDirectoryInfo"/> class.</summary>
      /// <param name="path">The specified path. This cannot be a different disk volume.</param>
      /// <param name="directorySecurity">The <see cref="T:DirectorySecurity"/> security to apply.</param>
      /// <returns>The last directory specified in <paramref name="path"/>.</returns>
      /// <remarks>
      /// Any and all directories specified in path are created, unless some part of path is invalid.
      /// The path parameter specifies a directory path, not a file path.
      /// If the subdirectory already exists, this method does nothing.
      /// </remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public PortableDeviceDirectoryInfo CreateSubdirectory(string path, DirectorySecurity directorySecurity)
      {
         return CreateSubdirectoryInternal(path, null, directorySecurity, false);
      }

      #endregion // .NET

      #region AlphaFS

      /// <summary>[AlphaFS] Creates a subdirectory or subdirectories on the specified path. The specified path can be relative to this instance of the <see cref="T:PortableDeviceDirectoryInfo"/> class.</summary>
      /// <param name="path">The specified path. This cannot be a different disk volume.</param>
      /// <param name="compress">When <c>true</c> compresses the directory.</param>
      /// <returns>The last directory specified in <paramref name="path"/>.</returns>
      /// <remarks>
      /// Any and all directories specified in path are created, unless some part of path is invalid.
      /// The path parameter specifies a directory path, not a file path.
      /// If the subdirectory already exists, this method does nothing.
      /// </remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public PortableDeviceDirectoryInfo CreateSubdirectory(string path, bool compress)
      {
         return CreateSubdirectoryInternal(path, null, null, compress);
      }

      /// <summary>[AlphaFS] Creates a subdirectory or subdirectories on the specified path. The specified path can be relative to this instance of the <see cref="T:PortableDeviceDirectoryInfo"/> class.</summary>
      /// <param name="path">The specified path. This cannot be a different disk volume.</param>
      /// <param name="templatePath">The path of the directory to use as a template when creating the new directory.</param>
      /// <param name="compress">When <c>true</c> compresses the directory.</param>
      /// <returns>The last directory specified in <paramref name="path"/>.</returns>
      /// <remarks>
      /// Any and all directories specified in path are created, unless some part of path is invalid.
      /// The path parameter specifies a directory path, not a file path.
      /// If the subdirectory already exists, this method does nothing.
      /// </remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public PortableDeviceDirectoryInfo CreateSubdirectory(string path, string templatePath, bool compress)
      {
         return CreateSubdirectoryInternal(path, templatePath, null, compress);
      }


      /// <summary>[AlphaFS] Creates a subdirectory or subdirectories on the specified path. The specified path can be relative to this instance of the <see cref="T:PortableDeviceDirectoryInfo"/> class.</summary>
      /// <param name="path">The specified path. This cannot be a different disk volume.</param>
      /// <param name="directorySecurity">The <see cref="T:DirectorySecurity"/> security to apply.</param>
      /// <param name="compress">When <c>true</c> compresses the directory.</param>
      /// <returns>The last directory specified in <paramref name="path"/>.</returns>
      /// <remarks>
      /// Any and all directories specified in path are created, unless some part of path is invalid.
      /// The path parameter specifies a directory path, not a file path.
      /// If the subdirectory already exists, this method does nothing.
      /// </remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public PortableDeviceDirectoryInfo CreateSubdirectory(string path, DirectorySecurity directorySecurity, bool compress)
      {
         return CreateSubdirectoryInternal(path, null, directorySecurity, compress);
      }

      /// <summary>[AlphaFS] Creates a subdirectory or subdirectories on the specified path. The specified path can be relative to this instance of the <see cref="T:PortableDeviceDirectoryInfo"/> class.</summary>
      /// <param name="templatePath">The path of the directory to use as a template when creating the new directory.</param>
      /// <param name="path">The specified path. This cannot be a different disk volume.</param>
      /// <param name="compress">When <c>true</c> compresses the directory.</param>
      /// <param name="directorySecurity">The <see cref="T:DirectorySecurity"/> security to apply.</param>
      /// <returns>The last directory specified in <paramref name="path"/>.</returns>
      /// <remarks>
      /// Any and all directories specified in path are created, unless some part of path is invalid.
      /// The path parameter specifies a directory path, not a file path.
      /// If the subdirectory already exists, this method does nothing.
      /// </remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public PortableDeviceDirectoryInfo CreateSubdirectory(string path, string templatePath, DirectorySecurity directorySecurity, bool compress)
      {
         return CreateSubdirectoryInternal(path, templatePath, directorySecurity, compress);
      }

      #endregion // AlphaFS

      #endregion // CreateSubdirectory

      #region Delete

      #region .NET

      /// <summary>Deletes this <see cref="T:PortableDeviceDirectoryInfo"/> if it is empty.</summary>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public override void Delete()
      {
         //Directory.DeleteDirectoryInternal(EntryInfo, null, null, false, false, true, false, null);
         Reset();
      }

      /// <summary>Deletes this instance of a <see cref="T:PortableDeviceDirectoryInfo"/>, specifying whether to delete subdirectories and files.</summary>
      /// <param name="recursive"><c>true</c> to delete this directory, its subdirectories, and all files; otherwise, <c>false</c>.</param>
      /// <remarks>
      /// If the <see cref="T:PortableDeviceDirectoryInfo"/> has no files or subdirectories, this method deletes the <see cref="T:PortableDeviceDirectoryInfo"/> even if <paramref name="recursive"/> is <c>false</c>.
      /// Attempting to delete a <see cref="T:PortableDeviceDirectoryInfo"/> that is not empty when <paramref name="recursive"/> is <c>false</c> throws an <see cref="T:IOException"/>.
      /// </remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public void Delete(bool recursive)
      {
         //Directory.DeleteDirectoryInternal(EntryInfo, null, null, recursive, false, !recursive, false, null);
         Reset();
      }

      #endregion // .NET

      #region AlphaFS

      /// <summary>[AlphaFS] Deletes this instance of a <see cref="T:PortableDeviceDirectoryInfo"/>, specifying whether to delete files and subdirectories.</summary>
      /// <param name="recursive"><c>true</c> to delete this directory, its subdirectories, and all files; otherwise, <c>false</c>.</param>
      /// <param name="ignoreReadOnly"><c>true</c> ignores read only attribute of files and directories.</param>
      /// <remarks>
      /// If the <see cref="T:PortableDeviceDirectoryInfo"/> has no files or subdirectories, this method deletes the <see cref="T:PortableDeviceDirectoryInfo"/> even if recursive is <c>false</c>.
      /// Attempting to delete a <see cref="T:PortableDeviceDirectoryInfo"/> that is not empty when recursive is false throws an <see cref="T:IOException"/>.
      /// </remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public void Delete(bool recursive, bool ignoreReadOnly)
      {
         //Directory.DeleteDirectoryInternal(EntryInfo, null, null, recursive, ignoreReadOnly, !recursive, false, null);
         Reset();
      }

      #endregion // AlphaFS

      #endregion // Delete

      #region EnumerateDirectories

      #region .NET

      /// <summary>Returns an enumerable collection of directory information in the current directory.</summary>
      /// <returns>An enumerable collection of directories in the current directory.</returns>
      [SecurityCritical]
      public IEnumerable<PortableDeviceDirectoryInfo> EnumerateDirectories()
      {
         return null;
         //return Directory.EnumerateFileSystemEntryInfosInternal<PortableDeviceDirectoryInfo>(null, null, Path.WildcardStarMatchAll, DirectoryEnumerationOptions.FilesAndFolders, null);
      }

      /// <summary>Returns an enumerable collection of directory information that matches a specified search pattern.</summary>
      /// <param name="searchPattern">The search string to match against the names of directories. This parameter can contain a combination of valid literal path and wildcard (<see cref="T:Path.WildcardStarMatchAll"/> and <see cref="T:Path.WildcardQuestion"/>) characters, but doesn't support regular expressions.</param>
      /// <returns>An enumerable collection of directories that matches <paramref name="searchPattern"/>.</returns>
      [SecurityCritical]
      public IEnumerable<PortableDeviceDirectoryInfo> EnumerateDirectories(string searchPattern)
      {
         return null;
         //return Directory.EnumerateFileSystemEntryInfosInternal<PortableDeviceDirectoryInfo>(null, null, searchPattern, DirectoryEnumerationOptions.FilesAndFolders, null);
      }

      /// <summary>Returns an enumerable collection of directory information that matches a specified search pattern and search subdirectory option.</summary>
      /// <param name="searchPattern">The search string to match against the names of directories. This parameter can contain a combination of valid literal path and wildcard (<see cref="T:Path.WildcardStarMatchAll"/> and <see cref="T:Path.WildcardQuestion"/>) characters, but doesn't support regular expressions.</param>
      /// <param name="searchOption">One of the enumeration values that specifies whether the search operation should include only the current directory or all subdirectories. The default value is <see cref="T:SearchOption.TopDirectoryOnly"/>.</param>
      /// <returns>An enumerable collection of directories that matches <paramref name="searchPattern"/> and <paramref name="searchOption"/>.</returns>
      [SecurityCritical]
      public IEnumerable<PortableDeviceDirectoryInfo> EnumerateDirectories(string searchPattern, SearchOption searchOption)
      {
         DirectoryEnumerationOptions enumOptions = DirectoryEnumerationOptions.FilesAndFolders;
         if (searchOption == SearchOption.AllDirectories)
            enumOptions |= DirectoryEnumerationOptions.Recursive;

         return null;
         //return Directory.EnumerateFileSystemEntryInfosInternal<PortableDeviceDirectoryInfo>(null, null, searchPattern, enumOptions, null);
      }

      #endregion // .NET

      #endregion // EnumerateDirectories

      #region EnumerateFiles

      #region .NET

      /// <summary>Returns an enumerable collection of file information in the current directory.</summary>
      /// <returns>An enumerable collection of the files in the current directory.</returns>
      [SecurityCritical]
      public IEnumerable<FileInfo> EnumerateFiles()
      {
         return null;
         //return Directory.EnumerateFileSystemEntryInfosInternal<FileInfo>(null, null, Path.WildcardStarMatchAll, DirectoryEnumerationOptions.Files, null);
      }

      /// <summary>Returns an enumerable collection of file information that matches a search pattern.</summary>
      /// <param name="searchPattern">The search string to match against the names of files. This parameter can contain a combination of valid literal path and wildcard (<see cref="T:Path.WildcardStarMatchAll"/> and <see cref="T:Path.WildcardQuestion"/>) characters, but doesn't support regular expressions.</param>
      /// <returns>An enumerable collection of files that matches <paramref name="searchPattern"/>.</returns>
      [SecurityCritical]
      public IEnumerable<FileInfo> EnumerateFiles(string searchPattern)
      {
         return null;
         //return Directory.EnumerateFileSystemEntryInfosInternal<FileInfo>(null, null, searchPattern, DirectoryEnumerationOptions.Files, null);
      }

      /// <summary>Returns an enumerable collection of file information that matches a specified search pattern and search subdirectory option.</summary>
      /// <param name="searchPattern">The search string to match against the names of files. This parameter can contain a combination of valid literal path and wildcard (<see cref="T:Path.WildcardStarMatchAll"/> and <see cref="T:Path.WildcardQuestion"/>) characters, but doesn't support regular expressions.</param>
      /// <param name="searchOption">One of the enumeration values that specifies whether the search operation should include only the current directory or all subdirectories. The default value is <see cref="T:SearchOption.TopDirectoryOnly"/>.</param>
      /// <returns>An enumerable collection of files that matches <paramref name="searchPattern"/> and <paramref name="searchOption"/>.</returns>
      [SecurityCritical]
      public IEnumerable<FileInfo> EnumerateFiles(string searchPattern, SearchOption searchOption)
      {
         DirectoryEnumerationOptions enumOptions = DirectoryEnumerationOptions.FilesAndFolders;
         if (searchOption == SearchOption.AllDirectories)
            enumOptions |= DirectoryEnumerationOptions.Recursive;

         return null;
         //return Directory.EnumerateFileSystemEntryInfosInternal<FileInfo>(null, null, searchPattern, enumOptions, null);
      }

      #endregion // .NET

      #endregion // EnumerateFiles

      #region EnumerateFileSystemInfos

      #region .NET

      /// <summary>Returns an enumerable collection of file system information in the current directory.</summary>
      /// <returns>An enumerable collection of file system information in the current directory. </returns>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
      [SecurityCritical]
      public IEnumerable<FileSystemInfo> EnumerateFileSystemInfos()
      {
         return null;
         //return Directory.EnumerateFileSystemEntryInfosInternal<FileSystemInfo>(null, null, Path.WildcardStarMatchAll, DirectoryEnumerationOptions.FilesAndFolders, null);
      }

      /// <summary>Returns an enumerable collection of file system information that matches a specified search pattern.</summary>
      /// <param name="searchPattern">The search string to match against the names of files. This parameter can contain a combination of valid literal path and wildcard (<see cref="T:Path.WildcardStarMatchAll"/> and <see cref="T:Path.WildcardQuestion"/>) characters, but doesn't support regular expressions.</param>
      /// <returns>An enumerable collection of file system information objects that matches <paramref name="searchPattern"/>.</returns>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
      [SecurityCritical]
      public IEnumerable<FileSystemInfo> EnumerateFileSystemInfos(string searchPattern)
      {
         return null;
         //return Directory.EnumerateFileSystemEntryInfosInternal<FileSystemInfo>(null, null, searchPattern, DirectoryEnumerationOptions.FilesAndFolders, null);
      }

      /// <summary>Returns an enumerable collection of file system information that matches a specified search pattern and search subdirectory option.</summary>
      /// <param name="searchPattern">The search string to match against the names of files. This parameter can contain a combination of valid literal path and wildcard (<see cref="T:Path.WildcardStarMatchAll"/> and <see cref="T:Path.WildcardQuestion"/>) characters, but doesn't support regular expressions.</param>
      /// <param name="searchOption">One of the enumeration values that specifies whether the search operation should include only the current directory or all subdirectories. The default value is <see cref="T:SearchOption.TopDirectoryOnly"/>.</param>
      /// <returns>An enumerable collection of file system information objects that matches <paramref name="searchPattern"/> and <paramref name="searchOption"/>.</returns>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
      [SecurityCritical]
      public IEnumerable<FileSystemInfo> EnumerateFileSystemInfos(string searchPattern, SearchOption searchOption)
      {
         DirectoryEnumerationOptions enumOptions = DirectoryEnumerationOptions.FilesAndFolders;
         if (searchOption == SearchOption.AllDirectories)
            enumOptions |= DirectoryEnumerationOptions.Recursive;

         return null;
         //return Directory.EnumerateFileSystemEntryInfosInternal<FileSystemInfo>(null, null, searchPattern, enumOptions, null);
      }

      #endregion // .NET

      #endregion // EnumerateFileSystemInfos

      #region GetAccessControl

      #region .NET

      /// <summary>Gets a <see cref="T:DirectorySecurity"/> object that encapsulates the access control list (ACL) entries for the directory described by the current PortableDeviceDirectoryInfo object.</summary>
      /// <returns>A <see cref="T:DirectorySecurity"/> object that encapsulates the access control rules for the directory.</returns>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public DirectorySecurity GetAccessControl()
      {
         return null;
         //return File.GetAccessControlInternal<DirectorySecurity>(true, null, AccessControlSections.Access | AccessControlSections.Group | AccessControlSections.Owner, null);
      }

      /// <summary>Gets a <see cref="T:DirectorySecurity"/> object that encapsulates the specified type of access control list (ACL) entries for the directory described by the current <see cref="T:PortableDeviceDirectoryInfo"/> object.</summary>
      /// <param name="includeSections">One of the <see cref="T:AccessControlSections"/> values that specifies the type of access control list (ACL) information to receive.</param>
      /// <returns>A <see cref="T:DirectorySecurity"/> object that encapsulates the access control rules for the file described by the path parameter.</returns>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public DirectorySecurity GetAccessControl(AccessControlSections includeSections)
      {
         return null;
         //return File.GetAccessControlInternal<DirectorySecurity>(true, null, includeSections, null);
      }

      #endregion // .NET

      #endregion // GetAccessControl

      #region MoveTo

      #region .NET

      /// <summary>Moves a <see cref="T:PortableDeviceDirectoryInfo"/> instance and its contents to a new path.</summary>
      /// <param name="destDirName">The name and path to which to move this directory. The destination cannot be another disk volume or a directory with the identical name. It can be an existing directory to which you want to add this directory as a subdirectory.</param>
      /// <remarks>MSDN: .NET 4+ Trailing spaces are removed from the end of the <paramref name="destDirName"/> parameter before moving the directory.</remarks>
      /// <remarks>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method. If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dir")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "dest")]
      [SecurityCritical]
      public void MoveTo(string destDirName)
      {
         CopyToMoveToInternal(true, destDirName, false, null, MoveOptions.None, null, null, false);
      }

      #endregion // .NET

      #region AlphaFS

      /// <summary>[AlphaFS] Moves a <see cref="T:PortableDeviceDirectoryInfo"/> instance and its contents to a new path.</summary>
      /// <param name="destDirName">The path to the new location for sourcePath.</param>
      /// <param name="overwrite"><c>true</c> Delete destination directory if it exists; <c>false</c> Move will fail on existing directories or files.</param>
      /// <remarks>MSDN: .NET 4+ Trailing spaces are removed from the end of the <paramref name="destDirName"/> parameter before moving the directory.</remarks>
      /// <remarks>This method works across disk volumes.</remarks>
      /// <remarks>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method. If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dir")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "dest")]
      [SecurityCritical]
      public void MoveTo(string destDirName, bool overwrite)
      {
         CopyToMoveToInternal(true, destDirName, false, null, overwrite ? MoveOptions.None : MoveOptions.CopyAllowed, null, null, false);
      }

      /// <summary>[AlphaFS] Moves a <see cref="T:PortableDeviceDirectoryInfo"/> instance and its contents to a new path.</summary>
      /// <param name="destDirName">The destination directory path, of type string</param>
      /// <param name="overwrite"><c>true</c> Delete destination directory if it exists; <c>false</c> Move will fail on existing directories or files.</param>
      /// <param name="preserveSecurity"><c>true</c> Preserves ACLs information.</param>
      /// <remarks>MSDN: .NET 4+ Trailing spaces are removed from the end of the <paramref name="destDirName"/> parameter before moving the directory.</remarks>
      /// <remarks>This method works across disk volumes.</remarks>
      /// <remarks>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method. If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dir")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "dest")]
      [SecurityCritical]
      public void MoveTo(string destDirName, bool overwrite, bool preserveSecurity)
      {
         CopyToMoveToInternal(true, destDirName, preserveSecurity, null, overwrite ? MoveOptions.None : MoveOptions.CopyAllowed, null, null, false);
      }

      /// <summary>[AlphaFS] Moves a <see cref="T:PortableDeviceDirectoryInfo"/> instance and its contents to a new path.</summary>
      /// <param name="destDirName">The destination directory path, of type string</param>
      /// <param name="moveOptions">Flags that specify how the file is to be move. This parameter can be <c>null</c>.</param>
      /// <param name="preserveSecurity"><c>true</c> Preserves ACLs information.</param>
      /// <param name="moveProgress">A callback function that is called each time another portion of the file has been moved. This parameter can be <c>null</c>.</param>
      /// <param name="userProgressData">The argument to be passed to the callback function. This parameter can be <c>null</c>.</param>
      /// <remarks>MSDN: .NET 4+ Trailing spaces are removed from the end of the <paramref name="destDirName"/> parameter before moving the directory.</remarks>
      /// <remarks>This method works across disk volumes.</remarks>
      /// <remarks>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method. If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dir")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "dest")]
      [SecurityCritical]
      public void MoveTo(string destDirName, MoveOptions moveOptions, bool preserveSecurity, CopyMoveProgressRoutine moveProgress, object userProgressData)
      {
         CopyToMoveToInternal(true, destDirName, preserveSecurity, null, moveOptions, moveProgress, userProgressData, false);
      }

      #endregion // AlphaFS

      #endregion // MoveTo

      #region Refresh

      #region .NET

      /// <summary>Refreshes the state of the object.</summary>
      [SecurityCritical]
      public new void Refresh()
      {
         base.Refresh();
      }

      #endregion // .NET

      #endregion // Refresh

      #region SetAccessControl

      #region .NET

      /// <summary>Applies access control list (ACL) entries described by a <see cref="T:DirectorySecurity"/> object to the directory described by the current PortableDeviceDirectoryInfo object.</summary>
      /// <param name="directorySecurity">A <see cref="T:DirectorySecurity"/> object that describes an ACL entry to apply to the directory described by the path parameter.</param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
      [SecurityCritical]
      public void SetAccessControl(DirectorySecurity directorySecurity)
      {
         //File.SetAccessControlInternal(null, null, directorySecurity, AccessControlSections.All, null);
      }

      /// <summary>Applies access control list (ACL) entries described by a <see cref="T:DirectorySecurity"/> object to the directory described by the current PortableDeviceDirectoryInfo object.</summary>
      /// <param name="directorySecurity">A <see cref="T:DirectorySecurity"/> object that describes an ACL entry to apply to the directory described by the path parameter.</param>
      /// <param name="includeSections">One or more of the <see cref="T:AccessControlSections"/> values that specifies the type of access control list (ACL) information to set.</param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
      [SecurityCritical]
      public void SetAccessControl(DirectorySecurity directorySecurity, AccessControlSections includeSections)
      {
         //File.SetAccessControlInternal(null, null, directorySecurity, includeSections, null);
      }

      #endregion // .NET

      #endregion // SetAccessControl

      #region ToString

      #region .NET

      /// <summary>Returns the original path that was passed by the user.</summary>
      public override string ToString()
      {
         return DisplayPath;
      }

      #endregion // .NET

      #endregion // ToString

      #endregion // .NET

      #region AlphaFS

      #region CopyTo

      /// <summary>[AlphaFS] Recursive copying of directories and files from one root to another.</summary>
      /// <param name="destDirName">The destination directory path, of type string</param>
      /// <remarks>MSDN: .NET 4+ Trailing spaces are removed from the end of the <paramref name="destDirName"/> parameter before copying the directory.</remarks>
      /// <remarks>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method. If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dir")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "dest")]
      [SecurityCritical]
      public PortableDeviceDirectoryInfo CopyTo(string destDirName)
      {
         return CopyToMoveToInternal(false, destDirName, false, CopyOptions.FailIfExists, null, null, null, false);
      }

      /// <summary>[AlphaFS] Recursive copying of directories and files from one root to another.</summary>
      /// <param name="destDirName">The destination directory path, of type string</param>
      /// <param name="overwrite"><c>true</c> Delete destination directory if it exists; <c>false</c> Copy will fail on existing directories or files.</param>
      /// <remarks>MSDN: .NET 4+ Trailing spaces are removed from the end of the <paramref name="destDirName"/> parameter before copying the directory.</remarks>
      /// <remarks>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method. If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dir")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "dest")]
      [SecurityCritical]
      public PortableDeviceDirectoryInfo CopyTo(string destDirName, bool overwrite)
      {
         return CopyToMoveToInternal(false, destDirName, false, overwrite ? CopyOptions.None : CopyOptions.FailIfExists, null, null, null, false);
      }

      /// <summary>[AlphaFS] Recursive copying of directories and files from one root to another.</summary>
      /// <param name="destDirName">The destination directory path, of type string</param>
      /// <param name="overwrite"><c>true</c> Delete destination directory if it exists; <c>false</c> Copy will fail on existing directories or files.</param>
      /// <param name="preserveSecurity"><c>true</c> Preserves ACLs information.</param>
      /// <remarks>MSDN: .NET 4+ Trailing spaces are removed from the end of the <paramref name="destDirName"/> parameter before copying the directory.</remarks>
      /// <remarks>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method. If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dir")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "dest")]
      [SecurityCritical]
      public PortableDeviceDirectoryInfo CopyTo(string destDirName, bool overwrite, bool preserveSecurity)
      {
         return CopyToMoveToInternal(false, destDirName, preserveSecurity, overwrite ? CopyOptions.None : CopyOptions.FailIfExists, null, null, null, false);
      }

      /// <summary>[AlphaFS] Recursive copying of directories and files from one root to another.</summary>
      /// <param name="destDirName">The destination directory path, of type string</param>
      /// <param name="copyOptions"><see cref="T:CopyOptions"/> that specify how the file is to be copied. This parameter can be <c>null</c>.</param>
      /// <param name="preserveSecurity"><c>true</c> Preserves ACLs information.</param>
      /// <param name="copyProgress">A callback function that is called each time another portion of the file has been copied. This parameter can be <c>null</c>.</param>
      /// <param name="userProgressData">The argument to be passed to the callback function. This parameter can be <c>null</c>.</param>
      /// <remarks>MSDN: .NET 4+ Trailing spaces are removed from the end of the <paramref name="destDirName"/> parameter before copying the directory.</remarks>
      /// <remarks>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method. If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dir")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "dest")]
      [SecurityCritical]
      public PortableDeviceDirectoryInfo CopyTo(string destDirName, CopyOptions copyOptions, bool preserveSecurity, CopyMoveProgressRoutine copyProgress, object userProgressData)
      {
         return CopyToMoveToInternal(false, destDirName, preserveSecurity, copyOptions, null, copyProgress, userProgressData, false);
      }

      #endregion // CopyTo

      #region DeleteEmpty

      /// <summary>[AlphaFS] Deletes empty subdirectores from the <see cref="T:PortableDeviceDirectoryInfo"/> instance.</summary>
      [SecurityCritical]
      public void DeleteEmpty()
      {
         //Directory.DeleteEmptyDirectoryInternal(EntryInfo, null, null, false, false, true, null);
         Reset();
      }

      /// <summary>[AlphaFS] Deletes empty subdirectores from the <see cref="T:PortableDeviceDirectoryInfo"/> instance.</summary>
      /// <param name="recursive"><c>true</c> deletes empty subdirectories from this directory and its subdirectories.</param>
      [SecurityCritical]
      public void DeleteEmpty(bool recursive)
      {
         //Directory.DeleteEmptyDirectoryInternal(EntryInfo, null, null, recursive, false, true, null);
         Reset();
      }

      /// <summary>[AlphaFS] Deletes empty subdirectores from the <see cref="T:PortableDeviceDirectoryInfo"/> instance.</summary>
      /// <param name="recursive"><c>true</c> deletes empty subdirectories from this directory and its subdirectories.</param>
      /// <param name="ignoreReadOnly"><c>true</c> overrides read only <see cref="T:FileAttributes"/> of empty directories.</param>
      [SecurityCritical]
      public void DeleteEmpty(bool recursive, bool ignoreReadOnly)
      {
         //Directory.DeleteEmptyDirectoryInternal(EntryInfo, null, null, recursive, ignoreReadOnly, true, null);
         Reset();
      }

      #endregion // DeleteEmpty

      #region GetDirName

      private static string GetDirName(string path)
      {
         return path.Length > 3 ? Path.GetFileName(Path.RemoveTrailingDirectorySeparator(path, false)) : path;
      }

      #endregion // GetDirName

      #region GetDisplayName

      private static string GetDisplayName(string path)
      {
         return path.Length != 2 || (path[1] != Path.VolumeSeparatorChar) ? path : Path.CurrentDirectoryPrefix;
      }

      #endregion // GetDisplayName


      #region Unified Internals

      #region CreateSubdirectoryInternal

      /// <summary>[AlphaFS] Unified method CreateSubdirectory() to create a subdirectory or subdirectories on the specified path. The specified path can be relative to this instance of the PortableDeviceDirectoryInfo class.</summary>
      /// <param name="path">The specified path. This cannot be a different disk volume or Universal Naming Convention (UNC) name.</param>
      /// <param name="templatePath">The path of the directory to use as a template when creating the new directory.</param>
      /// <param name="directorySecurity">The <see cref="T:DirectorySecurity"/> security to apply.</param>
      /// <param name="compress">When <c>true</c> compresses the directory.</param>
      /// <returns>The last directory specified in path as an <see cref="T:PortableDeviceDirectoryInfo"/> object.</returns>
      /// <remarks>
      /// Any and all directories specified in path are created, unless some part of path is invalid.
      /// The path parameter specifies a directory path, not a file path.
      /// If the subdirectory already exists, this method does nothing.
      /// </remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      private PortableDeviceDirectoryInfo CreateSubdirectoryInternal(string path, string templatePath, DirectorySecurity directorySecurity, bool compress)
      {
         //string pathLp = Path.CombineInternal(false, null, path);

         //if (string.Compare(null, 0, pathLp, 0, null.Length, StringComparison.OrdinalIgnoreCase) != 0)
            //throw new ArgumentException("Invalid SubPath", pathLp);

         return null; //Directory.CreateDirectoryInternal(null, pathLp, templatePath, directorySecurity, compress, true);
      }

      #endregion // CreateSubdirectoryInternal

      #region CopyToMoveToInternal

      /// <summary>[AlphaFS] Unified method CopyToMoveToInternal() to recursively copy directories and files from one root to another.</summary>
      /// <param name="isMove"><c>true</c> indicates a directory move, <c>false</c> indicates a directory copy.</param>
      /// <param name="destinationPath">The destination directory path, of type string</param>
      /// <param name="preserveSecurity"><c>true</c> Preserves ACLs information.</param>
      /// <param name="copyOptions"><see cref="T:CopyOptions"/> that specify how the file is to be copied. This parameter can be <c>null</c>.</param>
      /// <param name="moveOptions"><see cref="T:MoveOptions"/> that specify how the file is to be moved. This parameter can be <c>null</c>.</param>
      /// <param name="copyProgress">A callback function that is called each time another portion of the file has been copied. This parameter can be <c>null</c>.</param>
      /// <param name="userProgressData">The argument to be passed to the callback function. This parameter can be <c>null</c>.</param>
      /// <param name="isfullName">
      /// <para><c>true</c> <paramref name="destinationPath"/> is an absolute path. Unicode prefix is applied.</para>
      /// <para><c>false</c> <paramref name="destinationPath"/> will be checked and resolved to an absolute path. Unicode prefix is applied.</para>
      /// <para><c>null</c> <paramref name="destinationPath"/> is already an absolute path with Unicode prefix. Use as is.</para>
      /// </param>
      /// <returns>When <paramref name="isMove"/> is <c>true</c> <c>null</c> is returned. Otherwise copy; a new <see cref="T:PortableDeviceDirectoryInfo"/> instance with a fully qualified path returned.</returns>
      /// <remarks>MSDN: .NET 4+ Trailing spaces are removed from the end of the <paramref name="destinationPath"/> parameter before copying/moving the directory.</remarks>
      /// <remarks>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method. If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior</remarks>
      /// <remarks>This Move method works across disk volumes, and it does not throw an exception if the source and destination are
      /// the same. Note that if you attempt to replace a file by moving a file of the same name into that directory, you
      /// get an IOException. You cannot use the Move method to overwrite an existing file.
      /// </remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      private PortableDeviceDirectoryInfo CopyToMoveToInternal(bool isMove, string destinationPath, bool preserveSecurity, CopyOptions? copyOptions, MoveOptions? moveOptions, CopyMoveProgressRoutine copyProgress, object userProgressData, bool? isfullName)
      {
         return null;
         

//         string destinationPathLp = isfullName == null
//            ? destinationPath
//            : (bool) isfullName
//               ? Path.GetLongPathInternal(destinationPath, false, false, false, false)
//#if NET35
//               : Path.GetFullPathInternal(null, destinationPath, true, false, false, true, false, true, false);
//#else
//               : Path.GetFullPathInternal(null, destinationPath, true, true, false, true, false, true, false);
//#endif


//         //Directory.CopyMoveInternal(isMove, null, null, destinationPathLp, preserveSecurity, copyOptions, moveOptions, copyProgress, userProgressData, null, null);

//         if (isMove)
//         {
//            //null = destinationPathLp;
//            FullPath = Path.GetRegularPathInternal(destinationPathLp, false, false, false, false);

//            OriginalPath = destinationPath;
//            DisplayPath = GetDisplayName(OriginalPath);

//            // Flush any cached information about the file.
//            Reset();
//         }

//         return isMove ? null : new PortableDeviceDirectoryInfo(null);
      }

      #endregion // CopyToMoveToInternal

      #endregion // Unified Internals

      #endregion // AlphaFS

      #endregion // Methods

      #region Properties

      #region .NET

      #region Exists

      /// <summary>Gets a value indicating whether the directory exists.</summary>
      /// <returns><c>true</c> if the directory exists; otherwise, <c>false</c>.</returns>
      public override bool Exists
      {
         //get { return (EntryInfo != null && EntryInfo.IsDirectory); }
         get { return true; }
      }

      #endregion // Exists

      #region Name

      /// <summary>Gets the name of this <see cref="T:PortableDeviceDirectoryInfo"/> instance.</summary>
      /// <returns>The directory name.</returns>
      /// <remarks>Returns only the name of the directory, such as "Bin". To get the full path, such as "c:\public\Bin", use the FullName property.</remarks>
      public override string Name { get; internal set; }

      #endregion // Name

      #region Parent

      /// <summary>Gets the parent directory of a specified subdirectory.</summary>
      /// <returns>The parent directory, or <c>null</c> if the path is null or if the file path denotes a root (such as "\", "C:", or * "\\server\share").</returns>
      public PortableDeviceDirectoryInfo Parent
      {
         get
         {
            string path = FullPath;

            if (path.Length > 3)
               path = Path.RemoveTrailingDirectorySeparator(FullPath, false);

            string dirName = Path.GetDirectoryName(path);
            return dirName == null ? null : new PortableDeviceDirectoryInfo(dirName, true, true);
         }
      }

      #endregion // Parent

      #region Root

      /// <summary>Gets the root portion of the directory.</summary>
      /// <returns>An object that represents the root of the directory.</returns>
      public PortableDeviceDirectoryInfo Root
      {
         get
         {
            string root = Path.GetPathRoot(FullPath, false);

            if (Utils.IsNullOrWhiteSpace(root))
               root = PortableDeviceConstants.DeviceObjectId;
            
            return new PortableDeviceDirectoryInfo(root, FullPath);
         }
      }

      #endregion // Root

      #endregion // .NET

      #endregion // Properties
   }
}