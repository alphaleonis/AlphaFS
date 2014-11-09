/* Copyright (c) 2008-2014 Peter Palotas, Jeffrey Jangli, Normalex
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

namespace Alphaleonis.Win32.Filesystem
{
   /// <summary>Exposes instance methods for creating, moving, and enumerating through directories and subdirectories. This class cannot be inherited.</summary>
   [SerializableAttribute]
   public sealed class DirectoryInfo : FileSystemInfo
   {
      #region Constructors

      #region DirectoryInfo

      #region .NET

      /// <summary>Initializes a new instance of the <see cref="T:Alphaleonis.Win32.Filesystem.DirectoryInfo"/> class on the specified path.</summary>
      /// <param name="path">The path on which to create the <see cref="T:Alphaleonis.Win32.Filesystem.DirectoryInfo"/>.</param>
      /// <remarks>
      /// This constructor does not check if a directory exists. This constructor is a placeholder for a string that is used to access the disk in subsequent operations.
      /// The path parameter can be a file name, including a file on a Universal Naming Convention (UNC) share.
      /// </remarks>
      public DirectoryInfo(string path) : this(null, path, false)
      {
      }
      
      #endregion // .NET

      #region AlphaFS

      /// <summary>[AlphaFS] Initializes a new instance of the <see cref="T:Alphaleonis.Win32.Filesystem.DirectoryInfo"/> class on the specified path.</summary>
      /// <param name="path">The path on which to create the <see cref="T:Alphaleonis.Win32.Filesystem.DirectoryInfo"/>.</param>
      /// <param name="isFullPath">
      /// <para><c>true</c> <paramref name="path"/> is an absolute path. Unicode prefix is applied.</para>
      /// <para><c>false</c> <paramref name="path"/> will be checked and resolved to an absolute path. Unicode prefix is applied.</para>
      /// <para><c>null</c> <paramref name="path"/> is already an absolute path with Unicode prefix. Use as is.</para>
      /// </param>
      /// <remarks>This constructor does not check if a directory exists. This constructor is a placeholder for a string that is used to access the disk in subsequent operations.</remarks>
      public DirectoryInfo(string path, bool? isFullPath) : this(null, path, isFullPath)
      {
      }

      /// <summary>[AlphaFS] Special internal implementation.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="fullPath">The full path on which to create the <see cref="T:Alphaleonis.Win32.Filesystem.DirectoryInfo"/>.</param>
      /// <param name="junk1">Not used.</param>
      /// <param name="junk2">Not used.</param>
      /// <remarks>This constructor does not check if a directory exists. This constructor is a placeholder for a string that is used to access the disk in subsequent operations.</remarks>
      [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "junk1")]
      [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "junk2")]
      private DirectoryInfo(KernelTransaction transaction, string fullPath, bool junk1, bool junk2)
      {
         IsDirectory = true;
         Transaction = transaction;

         LongFullName = Path.GetLongPathInternal(fullPath, false, false, false, false);

         OriginalPath = Path.GetFileName(fullPath, true);

         FullPath = fullPath;

         DisplayPath = GetDisplayName(OriginalPath);
      }

      #region Transacted

      /// <summary>[AlphaFS] Initializes a new instance of the <see cref="T:Alphaleonis.Win32.Filesystem.DirectoryInfo"/> class on the specified path.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path on which to create the <see cref="T:Alphaleonis.Win32.Filesystem.DirectoryInfo"/>.</param>
      /// <remarks>This constructor does not check if a directory exists. This constructor is a placeholder for a string that is used to access the disk in subsequent operations.</remarks>
      public DirectoryInfo(KernelTransaction transaction, string path) : this(transaction, path, false)
      {
      }

      /// <summary>[AlphaFS] Initializes a new instance of the <see cref="T:Alphaleonis.Win32.Filesystem.DirectoryInfo"/> class on the specified path.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path on which to create the <see cref="T:Alphaleonis.Win32.Filesystem.DirectoryInfo"/>.</param>
      /// <param name="isFullPath">
      /// <para><c>true</c> <paramref name="path"/> is an absolute path. Unicode prefix is applied.</para>
      /// <para><c>false</c> <paramref name="path"/> will be checked and resolved to an absolute path. Unicode prefix is applied.</para>
      /// <para><c>null</c> <paramref name="path"/> is already an absolute path with Unicode prefix. Use as is.</para>
      /// </param>
      /// <remarks>This constructor does not check if a directory exists. This constructor is a placeholder for a string that is used to access the disk in subsequent operations.</remarks>
      public DirectoryInfo(KernelTransaction transaction, string path, bool? isFullPath)
      {
         InitializeInternal(true, transaction, path, isFullPath);
      }

      #endregion // Transacted

      #endregion // AlphaFS

      #endregion // DirectoryInfo

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
         Directory.CreateDirectoryInternal(Transaction, LongFullName, null, null, false, null);
      }

      /// <summary>Creates a directory using a <see cref="T:System.Security.AccessControl.DirectorySecurity"/> object.</summary>
      /// <param name="directorySecurity">The access control to apply to the directory.</param>
      /// <remarks>If the directory already exists, this method does nothing.</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
      [SecurityCritical]
      public void Create(DirectorySecurity directorySecurity)
      {
         Directory.CreateDirectoryInternal(Transaction, LongFullName, null, directorySecurity, false, null);
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
         Directory.CreateDirectoryInternal(Transaction, LongFullName, null, null, compress, null);
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
         Directory.CreateDirectoryInternal(Transaction, LongFullName, null, directorySecurity, compress, null);
      }

      #endregion // AlphaFS

      #endregion // Create

      #region CreateSubdirectory

      #region .NET

      /// <summary>Creates a subdirectory or subdirectories on the specified path. The specified path can be relative to this instance of the <see cref="T:DirectoryInfo"/> class.</summary>
      /// <param name="path">The specified path. This cannot be a different disk volume.</param>
      /// <returns>The last directory specified in <paramref name="path"/>.</returns>
      /// <remarks>
      /// Any and all directories specified in path are created, unless some part of path is invalid.
      /// The path parameter specifies a directory path, not a file path.
      /// If the subdirectory already exists, this method does nothing.
      /// </remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public DirectoryInfo CreateSubdirectory(string path)
      {
         return CreateSubdirectoryInternal(path, null, null, false);
      }

      /// <summary>Creates a subdirectory or subdirectories on the specified path. The specified path can be relative to this instance of the <see cref="T:DirectoryInfo"/> class.</summary>
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
      public DirectoryInfo CreateSubdirectory(string path, DirectorySecurity directorySecurity)
      {
         return CreateSubdirectoryInternal(path, null, directorySecurity, false);
      }

      #endregion // .NET

      #region AlphaFS

      /// <summary>[AlphaFS] Creates a subdirectory or subdirectories on the specified path. The specified path can be relative to this instance of the <see cref="T:DirectoryInfo"/> class.</summary>
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
      public DirectoryInfo CreateSubdirectory(string path, bool compress)
      {
         return CreateSubdirectoryInternal(path, null, null, compress);
      }

      /// <summary>[AlphaFS] Creates a subdirectory or subdirectories on the specified path. The specified path can be relative to this instance of the <see cref="T:DirectoryInfo"/> class.</summary>
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
      public DirectoryInfo CreateSubdirectory(string path, string templatePath, bool compress)
      {
         return CreateSubdirectoryInternal(path, templatePath, null, compress);
      }


      /// <summary>[AlphaFS] Creates a subdirectory or subdirectories on the specified path. The specified path can be relative to this instance of the <see cref="T:DirectoryInfo"/> class.</summary>
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
      public DirectoryInfo CreateSubdirectory(string path, DirectorySecurity directorySecurity, bool compress)
      {
         return CreateSubdirectoryInternal(path, null, directorySecurity, compress);
      }

      /// <summary>[AlphaFS] Creates a subdirectory or subdirectories on the specified path. The specified path can be relative to this instance of the <see cref="T:DirectoryInfo"/> class.</summary>
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
      public DirectoryInfo CreateSubdirectory(string path, string templatePath, DirectorySecurity directorySecurity, bool compress)
      {
         return CreateSubdirectoryInternal(path, templatePath, directorySecurity, compress);
      }

      #endregion // AlphaFS

      #endregion // CreateSubdirectory

      #region Delete

      #region .NET

      /// <summary>Deletes this <see cref="T:DirectoryInfo"/> if it is empty.</summary>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public override void Delete()
      {
         if (!Exists)
            throw new DirectoryNotFoundException(LongFullName);

         Directory.DeleteDirectoryInternal(EntryInfo, Transaction, null, false, false, true, false, null);
         
         Reset();
      }

      /// <summary>Deletes this instance of a <see cref="T:DirectoryInfo"/>, specifying whether to delete subdirectories and files.</summary>
      /// <param name="recursive"><c>true</c> to delete this directory, its subdirectories, and all files; otherwise, <c>false</c>.</param>
      /// <remarks>
      /// If the <see cref="T:DirectoryInfo"/> has no files or subdirectories, this method deletes the <see cref="T:DirectoryInfo"/> even if <paramref name="recursive"/> is <c>false</c>.
      /// Attempting to delete a <see cref="T:DirectoryInfo"/> that is not empty when <paramref name="recursive"/> is <c>false</c> throws an <see cref="T:IOException"/>.
      /// </remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public void Delete(bool recursive)
      {
         if (!Exists)
            throw new DirectoryNotFoundException(LongFullName);

         Directory.DeleteDirectoryInternal(EntryInfo, Transaction, null, recursive, false, !recursive, false, null);
         Reset();
      }

      #endregion // .NET

      #region AlphaFS

      /// <summary>[AlphaFS] Deletes this instance of a <see cref="T:DirectoryInfo"/>, specifying whether to delete files and subdirectories.</summary>
      /// <param name="recursive"><c>true</c> to delete this directory, its subdirectories, and all files; otherwise, <c>false</c>.</param>
      /// <param name="ignoreReadOnly"><c>true</c> ignores read only attribute of files and directories.</param>
      /// <remarks>
      /// If the <see cref="T:DirectoryInfo"/> has no files or subdirectories, this method deletes the <see cref="T:DirectoryInfo"/> even if recursive is <c>false</c>.
      /// Attempting to delete a <see cref="T:DirectoryInfo"/> that is not empty when recursive is false throws an <see cref="T:IOException"/>.
      /// </remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public void Delete(bool recursive, bool ignoreReadOnly)
      {
         Directory.DeleteDirectoryInternal(EntryInfo, Transaction, null, recursive, ignoreReadOnly, !recursive, false, null);
         Reset();
      }

      #endregion // AlphaFS

      #endregion // Delete

      #region EnumerateDirectories

      #region .NET

      /// <summary>Returns an enumerable collection of directory information in the current directory.</summary>
      /// <returns>An enumerable collection of directories in the current directory.</returns>
      [SecurityCritical]
      public IEnumerable<DirectoryInfo> EnumerateDirectories()
      {
         return File.EnumerateFileSystemEntryInfoInternal<DirectoryInfo>(Transaction, LongFullName, Path.WildcardStarMatchAll, SearchOption.TopDirectoryOnly, true, false, false, false, false, null);
      }

      /// <summary>Returns an enumerable collection of directory information that matches a specified search pattern.</summary>
      /// <param name="searchPattern">The search string to match against the names of directories. This parameter can contain a combination of valid literal path and wildcard (<see cref="T:Path.WildcardStarMatchAll"/> and <see cref="T:Path.WildcardQuestion"/>) characters, but doesn't support regular expressions.</param>
      /// <returns>An enumerable collection of directories that matches <paramref name="searchPattern"/>.</returns>
      [SecurityCritical]
      public IEnumerable<DirectoryInfo> EnumerateDirectories(string searchPattern)
      {
         return File.EnumerateFileSystemEntryInfoInternal<DirectoryInfo>(Transaction, LongFullName, searchPattern, SearchOption.TopDirectoryOnly, true, false, false, false, false, null);
      }

      /// <summary>Returns an enumerable collection of directory information that matches a specified search pattern and search subdirectory option.</summary>
      /// <param name="searchPattern">The search string to match against the names of directories. This parameter can contain a combination of valid literal path and wildcard (<see cref="T:Path.WildcardStarMatchAll"/> and <see cref="T:Path.WildcardQuestion"/>) characters, but doesn't support regular expressions.</param>
      /// <param name="searchOption">One of the enumeration values that specifies whether the search operation should include only the current directory or all subdirectories. The default value is <see cref="T:SearchOption.TopDirectoryOnly"/>.</param>
      /// <returns>An enumerable collection of directories that matches <paramref name="searchPattern"/> and <paramref name="searchOption"/>.</returns>
      [SecurityCritical]
      public IEnumerable<DirectoryInfo> EnumerateDirectories(string searchPattern, SearchOption searchOption)
      {
         return File.EnumerateFileSystemEntryInfoInternal<DirectoryInfo>(Transaction, LongFullName, searchPattern, searchOption, true, false, false, false, false, null);
      }

      #endregion // .NET

      #region AlphaFS

      /// <summary>[AlphaFS] Returns an enumerable collection of directory information in the current directory.</summary>
      /// <param name="continueOnException"><c>true</c> suppress any Exception that might be thrown a result from a failure, such as ACLs protected directories or non-accessible reparse points.</param>
      /// <returns>An enumerable collection of directories in the current directory.</returns>
      [SecurityCritical]
      public IEnumerable<DirectoryInfo> EnumerateDirectories(bool continueOnException)
      {
         return File.EnumerateFileSystemEntryInfoInternal<DirectoryInfo>(Transaction, LongFullName, Path.WildcardStarMatchAll, SearchOption.TopDirectoryOnly, true, false, false, false, continueOnException, null);
      }

      /// <summary>[AlphaFS] Returns an enumerable collection of directory information that matches a specified search pattern.</summary>
      /// <param name="searchPattern">The search string to match against the names of directories. This parameter can contain a combination of valid literal path and wildcard (<see cref="T:Path.WildcardStarMatchAll"/> and <see cref="T:Path.WildcardQuestion"/>) characters, but doesn't support regular expressions.</param>
      /// <param name="continueOnException"><c>true</c> suppress any Exception that might be thrown a result from a failure, such as ACLs protected directories or non-accessible reparse points.</param>
      /// <returns>An enumerable collection of directories that matches <paramref name="searchPattern"/>.</returns>
      [SecurityCritical]
      public IEnumerable<DirectoryInfo> EnumerateDirectories(string searchPattern, bool continueOnException)
      {
         return File.EnumerateFileSystemEntryInfoInternal<DirectoryInfo>(Transaction, LongFullName, searchPattern, SearchOption.TopDirectoryOnly, true, false, false, false, continueOnException, null);
      }

      /// <summary>[AlphaFS] Returns an enumerable collection of directory information that matches a specified search pattern and search subdirectory option.</summary>
      /// <param name="searchPattern">The search string to match against the names of directories. This parameter can contain a combination of valid literal path and wildcard (<see cref="T:Path.WildcardStarMatchAll"/> and <see cref="T:Path.WildcardQuestion"/>) characters, but doesn't support regular expressions.</param>
      /// <param name="searchOption">One of the enumeration values that specifies whether the search operation should include only the current directory or all subdirectories. The default value is <see cref="T:SearchOption.TopDirectoryOnly"/>.</param>
      /// <param name="continueOnException"><c>true</c> suppress any Exception that might be thrown a result from a failure, such as ACLs protected directories or non-accessible reparse points.</param>
      /// <returns>An enumerable collection of directories that matches <paramref name="searchPattern"/> and <paramref name="searchOption"/>.</returns>
      [SecurityCritical]
      public IEnumerable<DirectoryInfo> EnumerateDirectories(string searchPattern, SearchOption searchOption, bool continueOnException)
      {
         return File.EnumerateFileSystemEntryInfoInternal<DirectoryInfo>(Transaction, LongFullName, searchPattern, searchOption, true, false, false, false, continueOnException, null);
      }

      #endregion // AlphaFS

      #endregion // EnumerateDirectories

      #region EnumerateFiles

      #region .NET

      /// <summary>Returns an enumerable collection of file information in the current directory.</summary>
      /// <returns>An enumerable collection of the files in the current directory.</returns>
      [SecurityCritical]
      public IEnumerable<FileInfo> EnumerateFiles()
      {
         return File.EnumerateFileSystemEntryInfoInternal<FileInfo>(Transaction, LongFullName, Path.WildcardStarMatchAll, SearchOption.TopDirectoryOnly, false, false, false, false, false, null);
      }

      /// <summary>Returns an enumerable collection of file information that matches a search pattern.</summary>
      /// <param name="searchPattern">The search string to match against the names of files. This parameter can contain a combination of valid literal path and wildcard (<see cref="T:Path.WildcardStarMatchAll"/> and <see cref="T:Path.WildcardQuestion"/>) characters, but doesn't support regular expressions.</param>
      /// <returns>An enumerable collection of files that matches <paramref name="searchPattern"/>.</returns>
      [SecurityCritical]
      public IEnumerable<FileInfo> EnumerateFiles(string searchPattern)
      {
         return File.EnumerateFileSystemEntryInfoInternal<FileInfo>(Transaction, LongFullName, searchPattern, SearchOption.TopDirectoryOnly, false, false, false, false, false, null);
      }

      /// <summary>Returns an enumerable collection of file information that matches a specified search pattern and search subdirectory option.</summary>
      /// <param name="searchPattern">The search string to match against the names of files. This parameter can contain a combination of valid literal path and wildcard (<see cref="T:Path.WildcardStarMatchAll"/> and <see cref="T:Path.WildcardQuestion"/>) characters, but doesn't support regular expressions.</param>
      /// <param name="searchOption">One of the enumeration values that specifies whether the search operation should include only the current directory or all subdirectories. The default value is <see cref="T:SearchOption.TopDirectoryOnly"/>.</param>
      /// <returns>An enumerable collection of files that matches <paramref name="searchPattern"/> and <paramref name="searchOption"/>.</returns>
      [SecurityCritical]
      public IEnumerable<FileInfo> EnumerateFiles(string searchPattern, SearchOption searchOption)
      {
         return File.EnumerateFileSystemEntryInfoInternal<FileInfo>(Transaction, LongFullName, searchPattern, searchOption, false, false, false, false, false, null);
      }

      #endregion // .NET

      #region AlphaFS

      /// <summary>[AlphaFS] Returns an enumerable collection of file information in the current directory.</summary>
      /// <param name="continueOnException"><c>true</c> suppress any Exception that might be thrown a result from a failure, such as ACLs protected directories or non-accessible reparse points.</param>
      /// <returns>An enumerable collection of the files in the current directory.</returns>
      [SecurityCritical]
      public IEnumerable<FileInfo> EnumerateFiles(bool continueOnException)
      {
         return File.EnumerateFileSystemEntryInfoInternal<FileInfo>(Transaction, LongFullName, Path.WildcardStarMatchAll, SearchOption.TopDirectoryOnly, false, false, false, false, continueOnException, null);
      }

      /// <summary>[AlphaFS] Returns an enumerable collection of file information that matches a search pattern.</summary>
      /// <param name="searchPattern">The search string to match against the names of files. This parameter can contain a combination of valid literal path and wildcard (<see cref="T:Path.WildcardStarMatchAll"/> and <see cref="T:Path.WildcardQuestion"/>) characters, but doesn't support regular expressions.</param>
      /// <param name="continueOnException"><c>true</c> suppress any Exception that might be thrown a result from a failure, such as ACLs protected directories or non-accessible reparse points.</param>
      /// <returns>An enumerable collection of files that matches <paramref name="searchPattern"/>.</returns>
      [SecurityCritical]
      public IEnumerable<FileInfo> EnumerateFiles(string searchPattern, bool continueOnException)
      {
         return File.EnumerateFileSystemEntryInfoInternal<FileInfo>(Transaction, LongFullName, searchPattern, SearchOption.TopDirectoryOnly, false, false, false, false, continueOnException, null);
      }

      /// <summary>[AlphaFS] Returns an enumerable collection of file information that matches a specified search pattern and search subdirectory option.</summary>
      /// <param name="searchPattern">The search string to match against the names of files. This parameter can contain a combination of valid literal path and wildcard (<see cref="T:Path.WildcardStarMatchAll"/> and <see cref="T:Path.WildcardQuestion"/>) characters, but doesn't support regular expressions.</param>
      /// <param name="searchOption">One of the enumeration values that specifies whether the search operation should include only the current directory or all subdirectories. The default value is <see cref="T:SearchOption.TopDirectoryOnly"/>.</param>
      /// <param name="continueOnException"><c>true</c> suppress any Exception that might be thrown a result from a failure, such as ACLs protected directories or non-accessible reparse points.</param>
      /// <returns>An enumerable collection of files that matches <paramref name="searchPattern"/> and <paramref name="searchOption"/>.</returns>
      [SecurityCritical]
      public IEnumerable<FileInfo> EnumerateFiles(string searchPattern, SearchOption searchOption, bool continueOnException)
      {
         return File.EnumerateFileSystemEntryInfoInternal<FileInfo>(Transaction, LongFullName, searchPattern, searchOption, false, false, false, false, continueOnException, null);
      }

      #endregion // AlphaFS

      #endregion // EnumerateFiles

      #region EnumerateFileSystemInfos

      #region .NET

      /// <summary>Returns an enumerable collection of file system information in the current directory.</summary>
      /// <returns>An enumerable collection of file system information in the current directory. </returns>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
      [SecurityCritical]
      public IEnumerable<FileSystemInfo> EnumerateFileSystemInfos()
      {
         return File.EnumerateFileSystemEntryInfoInternal<FileSystemInfo>(Transaction, LongFullName, Path.WildcardStarMatchAll, SearchOption.TopDirectoryOnly, null, false, false, false, false, null);
      }

      /// <summary>Returns an enumerable collection of file system information that matches a specified search pattern.</summary>
      /// <param name="searchPattern">The search string to match against the names of files. This parameter can contain a combination of valid literal path and wildcard (<see cref="T:Path.WildcardStarMatchAll"/> and <see cref="T:Path.WildcardQuestion"/>) characters, but doesn't support regular expressions.</param>
      /// <returns>An enumerable collection of file system information objects that matches <paramref name="searchPattern"/>.</returns>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
      [SecurityCritical]
      public IEnumerable<FileSystemInfo> EnumerateFileSystemInfos(string searchPattern)
      {
         return File.EnumerateFileSystemEntryInfoInternal<FileSystemInfo>(Transaction, LongFullName, searchPattern, SearchOption.TopDirectoryOnly, null, false, false, false, false, null);
      }

      /// <summary>Returns an enumerable collection of file system information that matches a specified search pattern and search subdirectory option.</summary>
      /// <param name="searchPattern">The search string to match against the names of files. This parameter can contain a combination of valid literal path and wildcard (<see cref="T:Path.WildcardStarMatchAll"/> and <see cref="T:Path.WildcardQuestion"/>) characters, but doesn't support regular expressions.</param>
      /// <param name="searchOption">One of the enumeration values that specifies whether the search operation should include only the current directory or all subdirectories. The default value is <see cref="T:SearchOption.TopDirectoryOnly"/>.</param>
      /// <returns>An enumerable collection of file system information objects that matches <paramref name="searchPattern"/> and <paramref name="searchOption"/>.</returns>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
      [SecurityCritical]
      public IEnumerable<FileSystemInfo> EnumerateFileSystemInfos(string searchPattern, SearchOption searchOption)
      {
         return File.EnumerateFileSystemEntryInfoInternal<FileSystemInfo>(Transaction, LongFullName, searchPattern, searchOption, null, false, false, false, false, null);
      }

      #endregion // .NET

      #region AlphaFS

      /// <summary>[AlphaFS] Returns an enumerable collection of file system information in the current directory.</summary>
      /// <param name="continueOnException"><c>true</c> suppress any Exception that might be thrown a result from a failure, such as ACLs protected directories or non-accessible reparse points.</param>
      /// <returns>An enumerable collection of file system information in the current directory. </returns>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
      [SecurityCritical]
      public IEnumerable<FileSystemInfo> EnumerateFileSystemInfos(bool continueOnException)
      {
         return File.EnumerateFileSystemEntryInfoInternal<FileSystemInfo>(Transaction, LongFullName, Path.WildcardStarMatchAll, SearchOption.TopDirectoryOnly, null, false, false, false, continueOnException, null);
      }

      /// <summary>[AlphaFS] Returns an enumerable collection of file system information that matches a specified search pattern.</summary>
      /// <param name="searchPattern">The search string to match against the names of directories in path. This parameter can contain a combination of valid literal path and wildcard (<see cref="T:Path.WildcardStarMatchAll"/> and <see cref="T:Path.WildcardQuestion"/>) characters, but doesn't support regular expressions.</param>
      /// <param name="continueOnException"><c>true</c> suppress any Exception that might be thrown a result from a failure, such as ACLs protected directories or non-accessible reparse points.</param>
      /// <returns>An enumerable collection of file system information objects that matches <paramref name="searchPattern"/>.</returns>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
      [SecurityCritical]
      public IEnumerable<FileSystemInfo> EnumerateFileSystemInfos(string searchPattern, bool continueOnException)
      {
         return File.EnumerateFileSystemEntryInfoInternal<FileSystemInfo>(Transaction, LongFullName, searchPattern, SearchOption.TopDirectoryOnly, null, false, false, false, continueOnException, null);
      }

      /// <summary>[AlphaFS] Returns an enumerable collection of file system information that matches a specified search pattern and search subdirectory option.</summary>
      /// <param name="searchPattern">The search string to match against the names of directories in path. This parameter can contain a combination of valid literal path and wildcard (<see cref="T:Path.WildcardStarMatchAll"/> and <see cref="T:Path.WildcardQuestion"/>) characters, but doesn't support regular expressions.</param>
      /// <param name="searchOption">One of the <see cref="T:SearchOption"/> enumeration values that specifies whether the <paramref name="searchOption"/> should include only the current directory or should include all subdirectories.</param>
      /// <param name="continueOnException"><c>true</c> suppress any Exception that might be thrown a result from a failure, such as ACLs protected directories or non-accessible reparse points.</param>
      /// <returns>An enumerable collection of file system information objects that matches <paramref name="searchPattern"/> and <paramref name="searchOption"/>.</returns>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
      [SecurityCritical]
      public IEnumerable<FileSystemInfo> EnumerateFileSystemInfos(string searchPattern, SearchOption searchOption, bool continueOnException)
      {
         return File.EnumerateFileSystemEntryInfoInternal<FileSystemInfo>(Transaction, LongFullName, searchPattern, searchOption, null, false, false, false, continueOnException, null);
      }

      #endregion // AlphaFS

      #endregion // EnumerateFileSystemInfos

      #region GetAccessControl

      #region .NET

      /// <summary>Gets a <see cref="T:DirectorySecurity"/> object that encapsulates the access control list (ACL) entries for the directory described by the current DirectoryInfo object.</summary>
      /// <returns>A <see cref="T:DirectorySecurity"/> object that encapsulates the access control rules for the directory.</returns>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public DirectorySecurity GetAccessControl()
      {
         return File.GetAccessControlInternal<DirectorySecurity>(true, LongFullName, AccessControlSections.Access | AccessControlSections.Group | AccessControlSections.Owner, null);
      }

      /// <summary>Gets a <see cref="T:DirectorySecurity"/> object that encapsulates the specified type of access control list (ACL) entries for the directory described by the current <see cref="T:DirectoryInfo"/> object.</summary>
      /// <param name="includeSections">One of the <see cref="T:AccessControlSections"/> values that specifies the type of access control list (ACL) information to receive.</param>
      /// <returns>A <see cref="T:DirectorySecurity"/> object that encapsulates the access control rules for the file described by the path parameter.</returns>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public DirectorySecurity GetAccessControl(AccessControlSections includeSections)
      {
         return File.GetAccessControlInternal<DirectorySecurity>(true, LongFullName, includeSections, null);
      }

      #endregion // .NET

      #endregion // GetAccessControl

      #region GetDirectories

      #region .NET

      /// <summary>Returns the subdirectories of the current directory.</summary>
      /// <returns>An array of <see cref="T:DirectoryInfo"/> objects.</returns>
      /// <remarks>If there are no subdirectories, this method returns an empty array. This method is not recursive.</remarks>
      /// <remarks>
      /// The EnumerateDirectories and GetDirectories methods differ as follows: When you use EnumerateDirectories, you can start enumerating the collection of names
      /// before the whole collection is returned; when you use GetDirectories, you must wait for the whole array of names to be returned before you can access the array.
      /// Therefore, when you are working with many files and directories, EnumerateDirectories can be more efficient.
      /// </remarks>
      [SecurityCritical]
      public DirectoryInfo[] GetDirectories()
      {
         return File.EnumerateFileSystemEntryInfoInternal<DirectoryInfo>(Transaction, LongFullName, Path.WildcardStarMatchAll, SearchOption.TopDirectoryOnly, true, false, false, false, false, null).ToArray();
      }

      /// <summary>Returns an array of directories in the current <see cref="T:DirectoryInfo"/> matching the given search criteria.</summary>
      /// <param name="searchPattern">The search string to match against the names of directories in path. This parameter can contain a combination of valid literal path and wildcard (<see cref="T:Path.WildcardStarMatchAll"/> and <see cref="T:Path.WildcardQuestion"/>) characters, but doesn't support regular expressions.</param>
      /// <returns>An array of type <see cref="T:DirectoryInfo"/> matching <paramref name="searchPattern"/>.</returns>
      /// <remarks>
      /// The EnumerateDirectories and GetDirectories methods differ as follows: When you use EnumerateDirectories, you can start enumerating the collection of names
      /// before the whole collection is returned; when you use GetDirectories, you must wait for the whole array of names to be returned before you can access the array.
      /// Therefore, when you are working with many files and directories, EnumerateDirectories can be more efficient.
      /// </remarks>
      [SecurityCritical]
      public DirectoryInfo[] GetDirectories(string searchPattern)
      {
         return File.EnumerateFileSystemEntryInfoInternal<DirectoryInfo>(Transaction, LongFullName, searchPattern, SearchOption.TopDirectoryOnly, true, false, false, false, false, null).ToArray();
      }

      /// <summary>Returns an array of directories in the current <see cref="T:DirectoryInfo"/> matching the given search criteria and using a value to determine whether to search subdirectories.</summary>
      /// <param name="searchPattern">The search string to match against the names of directories in path. This parameter can contain a combination of valid literal path and wildcard (<see cref="T:Path.WildcardStarMatchAll"/> and <see cref="T:Path.WildcardQuestion"/>) characters, but doesn't support regular expressions.</param>
      /// <param name="searchOption">One of the <see cref="T:SearchOption"/> enumeration values that specifies whether the <paramref name="searchOption"/> should include only the current directory or should include all subdirectories.</param>
      /// <returns>An array of type <see cref="T:DirectoryInfo"/> matching <paramref name="searchPattern"/>.</returns>
      /// <remarks>If there are no subdirectories, or no subdirectories match the searchPattern parameter, this method returns an empty array.</remarks>
      /// <remarks>
      /// The EnumerateDirectories and GetDirectories methods differ as follows: When you use EnumerateDirectories, you can start enumerating the collection of names
      /// before the whole collection is returned; when you use GetDirectories, you must wait for the whole array of names to be returned before you can access the array.
      /// Therefore, when you are working with many files and directories, EnumerateDirectories can be more efficient.
      /// </remarks>
      [SecurityCritical]
      public DirectoryInfo[] GetDirectories(string searchPattern, SearchOption searchOption)
      {
         return File.EnumerateFileSystemEntryInfoInternal<DirectoryInfo>(Transaction, LongFullName, searchPattern, searchOption, true, false, false, false, false, null).ToArray();
      }

      #endregion // .NET

      #endregion // GetDirectories
      
      #region GetFiles

      #region .NET

      /// <summary>Returns a file list from the current directory.</summary>
      /// <returns>An array of type <see cref="T:FileInfo"/>.</returns>
      /// <remarks>The order of the returned file names is not guaranteed; use the Sort() method if a specific sort order is required.</remarks>
      /// <remarks>If there are no files in the <see cref="DirectoryInfo"/>, this method returns an empty array.</remarks>
      /// <remarks>
      /// The EnumerateFiles and GetFiles methods differ as follows: When you use EnumerateFiles, you can start enumerating the collection of names
      /// before the whole collection is returned; when you use GetFiles, you must wait for the whole array of names to be returned before you can access the array.
      /// Therefore, when you are working with many files and directories, EnumerateFiles can be more efficient.
      /// </remarks>
      [SecurityCritical]
      public FileInfo[] GetFiles()
      {
         return File.EnumerateFileSystemEntryInfoInternal<FileInfo>(Transaction, LongFullName, Path.WildcardStarMatchAll, SearchOption.TopDirectoryOnly, false, false, false, false, false, null).ToArray();
      }

      /// <summary>Returns a file list from the current directory matching the given search pattern.</summary>
      /// <param name="searchPattern">The search string to match against the names of directories in path. This parameter can contain a combination of valid literal path and wildcard (<see cref="T:Path.WildcardStarMatchAll"/> and <see cref="T:Path.WildcardQuestion"/>) characters, but doesn't support regular expressions.</param>
      /// <returns>An array of type <see cref="T:FileInfo"/>.</returns>
      /// <remarks>The order of the returned file names is not guaranteed; use the Sort() method if a specific sort order is required.</remarks>
      /// <remarks>If there are no files in the <see cref="DirectoryInfo"/>, this method returns an empty array.</remarks>
      /// <remarks>
      /// The EnumerateFiles and GetFiles methods differ as follows: When you use EnumerateFiles, you can start enumerating the collection of names
      /// before the whole collection is returned; when you use GetFiles, you must wait for the whole array of names to be returned before you can access the array.
      /// Therefore, when you are working with many files and directories, EnumerateFiles can be more efficient.
      /// </remarks>
      [SecurityCritical]
      public FileInfo[] GetFiles(string searchPattern)
      {
         return File.EnumerateFileSystemEntryInfoInternal<FileInfo>(Transaction, LongFullName, searchPattern, SearchOption.TopDirectoryOnly, false, false, false, false, false, null).ToArray();
      }

      /// <summary>Returns a file list from the current directory matching the given search pattern and using a value to determine whether to search subdirectories.</summary>
      /// <param name="searchPattern">The search string to match against the names of directories in path. This parameter can contain a combination of valid literal path and wildcard (<see cref="T:Path.WildcardStarMatchAll"/> and <see cref="T:Path.WildcardQuestion"/>) characters, but doesn't support regular expressions.</param>
      /// <param name="searchOption">One of the <see cref="T:SearchOption"/> enumeration values that specifies whether the <paramref name="searchOption"/> should include only the current directory or should include all subdirectories.</param>
      /// <returns>An array of type <see cref="T:FileInfo"/>.</returns>
      /// <remarks>The order of the returned file names is not guaranteed; use the Sort() method if a specific sort order is required.</remarks>
      /// <remarks>If there are no files in the <see cref="DirectoryInfo"/>, this method returns an empty array.</remarks>
      /// <remarks>
      /// The EnumerateFiles and GetFiles methods differ as follows: When you use EnumerateFiles, you can start enumerating the collection of names
      /// before the whole collection is returned; when you use GetFiles, you must wait for the whole array of names to be returned before you can access the array.
      /// Therefore, when you are working with many files and directories, EnumerateFiles can be more efficient.
      /// </remarks>
      [SecurityCritical]
      public FileInfo[] GetFiles(string searchPattern, SearchOption searchOption)
      {
         return File.EnumerateFileSystemEntryInfoInternal<FileInfo>(Transaction, LongFullName, searchPattern, searchOption, false, false, false, false, false, null).ToArray();
      }

      #endregion // .NET

      #endregion // GetFiles

      #region GetFileSystemInfos

      #region .NET

      /// <summary>Returns an array of strongly typed <see cref="T:FileSystemInfo"/> entries representing all the files and subdirectories in a directory.</summary>
      /// <returns>An array of strongly typed <see cref="T:FileSystemInfo"/> entries.</returns>
      /// <remarks>
      /// For subdirectories, the <see cref="T:FileSystemInfo"/> objects returned by this method can be cast to the derived class <see cref="T:DirectoryInfo"/>.
      /// Use the <see cref="T:FileAttributes"/> value returned by the <see cref="FileSystemInfo.Attributes"/> property to determine whether the <see cref="T:FileSystemInfo"/> represents a file or a directory.
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
         return File.EnumerateFileSystemEntryInfoInternal<FileSystemInfo>(Transaction, LongFullName, Path.WildcardStarMatchAll, SearchOption.TopDirectoryOnly, null, false, false, false, false, null).ToArray();
      }

      /// <summary>Retrieves an array of strongly typed <see cref="T:FileSystemInfo"/> objects representing the files and subdirectories that match the specified search criteria.</summary>
      /// <param name="searchPattern">The search string to match against the names of directories in path. This parameter can contain a combination of valid literal path and wildcard (<see cref="T:Path.WildcardStarMatchAll"/> and <see cref="T:Path.WildcardQuestion"/>) characters, but doesn't support regular expressions.</param>
      /// <returns>An array of strongly typed <see cref="T:FileSystemInfo"/> entries.</returns>
      /// <remarks>
      /// For subdirectories, the <see cref="T:FileSystemInfo"/> objects returned by this method can be cast to the derived class <see cref="T:DirectoryInfo"/>.
      /// Use the <see cref="T:FileAttributes"/> value returned by the <see cref="FileSystemInfo.Attributes"/> property to determine whether the <see cref="T:FileSystemInfo"/> represents a file or a directory.
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
         return File.EnumerateFileSystemEntryInfoInternal<FileSystemInfo>(Transaction, LongFullName, searchPattern, SearchOption.TopDirectoryOnly, null, false, false, false, false, null).ToArray();
      }

      /// <summary>Retrieves an array of strongly typed <see cref="T:FileSystemInfo"/> objects representing the files and subdirectories that match the specified search criteria.</summary>
      /// <param name="searchPattern">The search string to match against the names of directories in path. This parameter can contain a combination of valid literal path and wildcard (<see cref="T:Path.WildcardStarMatchAll"/> and <see cref="T:Path.WildcardQuestion"/>) characters, but doesn't support regular expressions.</param>
      /// <param name="searchOption">One of the <see cref="T:SearchOption"/> enumeration values that specifies whether the <paramref name="searchOption"/> should include only the current directory or should include all subdirectories.</param>
      /// <returns>An array of strongly typed <see cref="T:FileSystemInfo"/> entries.</returns>
      /// <remarks>
      /// For subdirectories, the <see cref="T:FileSystemInfo"/> objects returned by this method can be cast to the derived class <see cref="T:DirectoryInfo"/>.
      /// Use the <see cref="T:FileAttributes"/> value returned by the <see cref="FileSystemInfo.Attributes"/> property to determine whether the <see cref="T:FileSystemInfo"/> represents a file or a directory.
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
         return File.EnumerateFileSystemEntryInfoInternal<FileSystemInfo>(Transaction, LongFullName, searchPattern, searchOption, null, false, false, false, false, null).ToArray();
      }

      #endregion // .NET

      #endregion // GetFileSystemInfos

      #region MoveTo

      #region .NET

      /// <summary>Moves a <see cref="T:DirectoryInfo"/> instance and its contents to a new path.</summary>
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

      /// <summary>[AlphaFS] Moves a <see cref="T:DirectoryInfo"/> instance and its contents to a new path.</summary>
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
         CopyToMoveToInternal(true, destDirName, false, null, overwrite ? NativeMethods.MoveOptionsReplace : MoveOptions.CopyAllowed, null, null, false);
      }

      /// <summary>[AlphaFS] Moves a <see cref="T:DirectoryInfo"/> instance and its contents to a new path.</summary>
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
         CopyToMoveToInternal(true, destDirName, preserveSecurity, null, overwrite ? NativeMethods.MoveOptionsReplace : MoveOptions.CopyAllowed, null, null, false);
      }

      /// <summary>[AlphaFS] Moves a <see cref="T:DirectoryInfo"/> instance and its contents to a new path.</summary>
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
      public void MoveTo(string destDirName, MoveOptions moveOptions, bool preserveSecurity, CopyMoveProgressCallback moveProgress, object userProgressData)
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

      /// <summary>Applies access control list (ACL) entries described by a <see cref="T:DirectorySecurity"/> object to the directory described by the current DirectoryInfo object.</summary>
      /// <param name="directorySecurity">A <see cref="T:DirectorySecurity"/> object that describes an ACL entry to apply to the directory described by the path parameter.</param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
      [SecurityCritical]
      public void SetAccessControl(DirectorySecurity directorySecurity)
      {
         File.SetAccessControlInternal(LongFullName, null, directorySecurity, AccessControlSections.All, null);
      }

      /// <summary>Applies access control list (ACL) entries described by a <see cref="T:DirectorySecurity"/> object to the directory described by the current DirectoryInfo object.</summary>
      /// <param name="directorySecurity">A <see cref="T:DirectorySecurity"/> object that describes an ACL entry to apply to the directory described by the path parameter.</param>
      /// <param name="includeSections">One or more of the <see cref="T:AccessControlSections"/> values that specifies the type of access control list (ACL) information to set.</param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
      [SecurityCritical]
      public void SetAccessControl(DirectorySecurity directorySecurity, AccessControlSections includeSections)
      {
         File.SetAccessControlInternal(LongFullName, null, directorySecurity, includeSections, null);
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

      #region AddStream

      /// <summary>[AlphaFS] Adds an alternate data stream (NTFS ADS) to the directory.</summary>
      /// <param name="name">The name for the stream. If a stream with <paramref name="name"/> already exists, it will be overwritten.</param>
      /// <param name="contents">The lines to add to the stream.</param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public void AddStream(string name, string[] contents)
      {
         AlternateDataStreamInfo.AddStreamInternal(true, Transaction, LongFullName, name, contents, null);
      }

      #endregion // AddStream

      #region CopyTo

      /// <summary>[AlphaFS] Recursive copying of directories and files from one root to another.</summary>
      /// <param name="destDirName">The destination directory path, of type string</param>
      /// <remarks>MSDN: .NET 4+ Trailing spaces are removed from the end of the <paramref name="destDirName"/> parameter before copying the directory.</remarks>
      /// <remarks>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method. If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dir")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "dest")]
      [SecurityCritical]
      public DirectoryInfo CopyTo(string destDirName)
      {
         return CopyToMoveToInternal(false, destDirName, false, NativeMethods.CopyOptionsFail, null, null, null, false);
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
      public DirectoryInfo CopyTo(string destDirName, bool overwrite)
      {
         return CopyToMoveToInternal(false, destDirName, false, overwrite ? NativeMethods.CopyOptionsNone : NativeMethods.CopyOptionsFail, null, null, null, false);
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
      public DirectoryInfo CopyTo(string destDirName, bool overwrite, bool preserveSecurity)
      {
         return CopyToMoveToInternal(false, destDirName, preserveSecurity, overwrite ? NativeMethods.CopyOptionsNone : NativeMethods.CopyOptionsFail, null, null, null, false);
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
      public DirectoryInfo CopyTo(string destDirName, CopyOptions copyOptions, bool preserveSecurity, CopyMoveProgressCallback copyProgress, object userProgressData)
      {
         return CopyToMoveToInternal(false, destDirName, preserveSecurity, copyOptions, null, copyProgress, userProgressData, false);
      }

      #endregion // CopyTo

      #region CountDirectories

      /// <summary>[AlphaFS] Counts directories in a given directory.</summary>
      /// <param name="continueOnException"><c>true</c> suppress any Exception that might be thrown a result from a failure, such as ACLs protected directories or non-accessible reparse points.</param>
      /// <returns>The counted number of directories.</returns>
      /// <exception cref="System.UnauthorizedAccessException">An exception is thrown case of access errors.</exception>
      [SecurityCritical]
      public long CountDirectories(bool continueOnException)
      {
         return File.EnumerateFileSystemEntryInfoInternal<string>(Transaction, LongFullName, Path.WildcardStarMatchAll, SearchOption.TopDirectoryOnly, true, true, false, false, continueOnException, null).Count();
      }

      /// <summary>[AlphaFS] Counts directories in a given directory.</summary>
      /// <param name="continueOnException"><c>true</c> suppress any Exception that might be thrown a result from a failure, such as ACLs protected directories or non-accessible reparse points.</param>
      /// <param name="searchPattern">The search string to match against the names of directories in path. This parameter can contain a combination of valid literal path and wildcard (<see cref="T:Path.WildcardStarMatchAll"/> and <see cref="T:Path.WildcardQuestion"/>) characters, but doesn't support regular expressions.</param>
      /// <param name="searchOption">One of the <see cref="T:SearchOption"/> enumeration values that specifies whether the <paramref name="searchOption"/> should include only the current directory or should include all subdirectories.</param>
      /// <returns>The counted number of directories.</returns>
      /// <exception cref="System.UnauthorizedAccessException">An exception is thrown case of access errors.</exception>
      [SecurityCritical]
      public long CountDirectories(bool continueOnException, string searchPattern, SearchOption searchOption)
      {
         return File.EnumerateFileSystemEntryInfoInternal<string>(Transaction, LongFullName, searchPattern, searchOption, true, true, false, false, continueOnException, null).Count();
      }

      #endregion // CountDirectories

      #region CountFiles

      /// <summary>[AlphaFS] Counts files in a given directory.</summary>
      /// <param name="continueOnException"><c>true</c> suppress any Exception that might be thrown a result from a failure, such as ACLs protected directories or non-accessible reparse points.</param>
      /// <returns>The counted number of files.</returns>
      /// <exception cref="System.UnauthorizedAccessException">An exception is thrown case of access errors.</exception>
      [SecurityCritical]
      public long CountFiles(bool continueOnException)
      {
         return File.EnumerateFileSystemEntryInfoInternal<string>(Transaction, LongFullName, Path.WildcardStarMatchAll, SearchOption.TopDirectoryOnly, false, true, false, false, continueOnException, null).Count();
      }

      /// <summary>[AlphaFS] Counts files in a given directory.</summary>
      /// <param name="continueOnException"><c>true</c> suppress any Exception that might be thrown a result from a failure, such as ACLs protected directories or non-accessible reparse points.</param>
      /// <param name="searchPattern">The search string to match against the names of directories in path. This parameter can contain a combination of valid literal path and wildcard (<see cref="T:Path.WildcardStarMatchAll"/> and <see cref="T:Path.WildcardQuestion"/>) characters, but doesn't support regular expressions.</param>
      /// <param name="searchOption">One of the <see cref="T:SearchOption"/> enumeration values that specifies whether the <paramref name="searchOption"/> should include only the current directory or should include all subdirectories.</param>
      /// <returns>The counted number of files.</returns>
      /// <exception cref="System.UnauthorizedAccessException">An exception is thrown case of access errors.</exception>
      [SecurityCritical]
      public long CountFiles(bool continueOnException, string searchPattern, SearchOption searchOption)
      {
         return File.EnumerateFileSystemEntryInfoInternal<string>(Transaction, LongFullName, searchPattern, searchOption, false, true, false, false, continueOnException, null).Count();
      }

      #endregion // CountFiles

      #region Compress

      /// <summary>[AlphaFS] Compresses a directory using NTFS compression.</summary>
      /// <remarks>This will only compress the root items, non recursive.</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public void Compress()
      {
         Directory.CompressDecompressInternal(Transaction, LongFullName, Path.WildcardStarMatchAll, SearchOption.TopDirectoryOnly, true, false, null);
      }

      /// <summary>[AlphaFS] Compresses a directory using NTFS compression.</summary>
      /// <param name="searchPattern">The search string to match against the names of directories in path. This parameter can contain a combination of valid literal path and wildcard (<see cref="T:Path.WildcardStarMatchAll"/> and <see cref="T:Path.WildcardQuestion"/>) characters, but doesn't support regular expressions.</param>
      /// <param name="searchOption">One of the <see cref="T:SearchOption"/> enumeration values that specifies whether the <paramref name="searchOption"/> should include only the current directory or should include all subdirectories.</param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public void Compress(string searchPattern, SearchOption searchOption)
      {
         Directory.CompressDecompressInternal(Transaction, LongFullName, searchPattern, searchOption, true, false, null);
      }

      #endregion // Compress

      #region DisableCompression

      /// <summary>[AlphaFS] Disables compression of the specified directory and the files in it.</summary>
      /// <remarks>
      /// This method disables the directory-compression attribute. It will not decompress the current contents of the directory.
      /// However, newly created files and directories will be uncompressed.
      /// </remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public void DisableCompression()
      {
         Device.ToggleCompressionInternal(true, Transaction, LongFullName, false, null);
      }

      #endregion // DisableCompression

      #region DisableEncryption

      /// <summary>[AlphaFS] Disables encryption of the specified directory and the files in it. It does not affect encryption of subdirectories below the indicated directory.</summary>
      /// <returns><c>true</c> on success, <c>false</c> otherwise.</returns>
      /// <remarks>This method will create/change the file "Desktop.ini" and wil set Encryption value: "Disable=0"</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public void DisableEncryption()
      {
         Directory.EnableDisableEncryptionInternal(LongFullName, false, null);
      }

      #endregion // DisableEncryption

      #region Decompress

      /// <summary>[AlphaFS] Decompresses an NTFS compressed directory.</summary>
      /// <remarks>This will only decompress the root items, non recursive.</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public void Decompress()
      {
         Directory.CompressDecompressInternal(Transaction, LongFullName, Path.WildcardStarMatchAll, SearchOption.TopDirectoryOnly, false, false, null);
      }

      /// <summary>[AlphaFS] Decompresses an NTFS compressed directory.</summary>
      /// <param name="searchPattern">The search string to match against the names of directories in path. This parameter can contain a combination of valid literal path and wildcard (<see cref="T:Path.WildcardStarMatchAll"/> and <see cref="T:Path.WildcardQuestion"/>) characters, but doesn't support regular expressions.</param>
      /// <param name="searchOption">One of the <see cref="T:SearchOption"/> enumeration values that specifies whether the <paramref name="searchOption"/> should include only the current directory or should include all subdirectories.</param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public void Decompress(string searchPattern, SearchOption searchOption)
      {
         Directory.CompressDecompressInternal(Transaction, LongFullName, searchPattern, searchOption, false, false, null);
      }

      #endregion // Decompress

      #region Decrypt

      /// <summary>[AlphaFS] Decrypts a directory that was encrypted by the current account using the Encrypt method.</summary>
      [SecurityCritical]
      public void Decrypt()
      {
         Directory.EncryptDecryptDirectoryInternal(LongFullName, false, false, null);
      }

      /// <summary>[AlphaFS] Decrypts a directory that was encrypted by the current account using the Encrypt method.</summary>
      /// <param name="recursive"><c>true</c> to decrypt the directory recursively. <c>false</c> only decrypt files and directories in the root of the directory.</param>
      [SecurityCritical]
      public void Decrypt(bool recursive)
      {
         Directory.EncryptDecryptDirectoryInternal(LongFullName, false, recursive, null);
      }

      #endregion // Decrypt
      
      #region DeleteEmpty

      /// <summary>[AlphaFS] Deletes empty subdirectores from the <see cref="T:DirectoryInfo"/> instance.</summary>
      [SecurityCritical]
      public void DeleteEmpty()
      {
         Directory.DeleteEmptyDirectoryInternal(EntryInfo, Transaction, null, false, false, true, null);
         Reset();
      }

      /// <summary>[AlphaFS] Deletes empty subdirectores from the <see cref="T:DirectoryInfo"/> instance.</summary>
      /// <param name="recursive"><c>true</c> deletes empty subdirectories from this directory and its subdirectories.</param>
      [SecurityCritical]
      public void DeleteEmpty(bool recursive)
      {
         Directory.DeleteEmptyDirectoryInternal(EntryInfo, Transaction, null, recursive, false, true, null);
         Reset();
      }

      /// <summary>[AlphaFS] Deletes empty subdirectores from the <see cref="T:DirectoryInfo"/> instance.</summary>
      /// <param name="recursive"><c>true</c> deletes empty subdirectories from this directory and its subdirectories.</param>
      /// <param name="ignoreReadOnly"><c>true</c> overrides read only <see cref="T:FileAttributes"/> of empty directories.</param>
      [SecurityCritical]
      public void DeleteEmpty(bool recursive, bool ignoreReadOnly)
      {
         Directory.DeleteEmptyDirectoryInternal(EntryInfo, Transaction, null, recursive, ignoreReadOnly, true, null);
         Reset();
      }

      #endregion // DeleteEmpty

      #region EnableCompression

      /// <summary>[AlphaFS] Enables compression of the specified directory and the files in it.</summary>
      /// <remarks>
      /// This method enables the directory-compression attribute. It will not compress the current contents of the directory.
      /// However, newly created files and directories will be compressed.
      /// </remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public void EnableCompression()
      {
         Device.ToggleCompressionInternal(true, Transaction, LongFullName, true, null);
      }

      #endregion // EnableCompression
      
      #region EnableEncryption

      /// <summary>[AlphaFS] Enables encryption of the specified directory and the files in it. It does not affect encryption of subdirectories below the indicated directory.</summary>
      /// <returns><c>true</c> on success, <c>false</c> otherwise.</returns>
      /// <remarks>This method will create/change the file "Desktop.ini" and wil set Encryption value: "Disable=1"</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public void EnableEncryption()
      {
         Directory.EnableDisableEncryptionInternal(LongFullName, true, null);
      }

      #endregion // EnableEncryption

      #region Encrypt

      /// <summary>[AlphaFS] Encrypts a directory so that only the account used to encrypt the directory can decrypt it.</summary>
      [SecurityCritical]
      public void Encrypt()
      {
         Directory.EncryptDecryptDirectoryInternal(LongFullName, true, false, null);
      }

      /// <summary>[AlphaFS] Decrypts a directory that was encrypted by the current account using the Encrypt method.</summary>
      /// <param name="recursive"><c>true</c> to encrypt the directory recursively. <c>false</c> only encrypt files and directories in the root of the directory.</param>
      [SecurityCritical]
      public void Encrypt(bool recursive)
      {
         Directory.EncryptDecryptDirectoryInternal(LongFullName, true, recursive, null);
      }

      #endregion // Encrypt

      #region EnumerateStreams

      /// <summary>[AlphaFS] Returns an enumerable collection of <see cref="T:AlternateDataStreamInfo"/> instances for the directory.</summary>
      /// <returns>An enumerable collection of <see cref="T:AlternateDataStreamInfo"/> instances for the directory.</returns>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public IEnumerable<AlternateDataStreamInfo> EnumerateStreams()
      {
         return AlternateDataStreamInfo.EnumerateStreamsInternal(true, Transaction, null, LongFullName, null, null, null);
      }

      /// <summary>[AlphaFS] Returns an enumerable collection of <see cref="T:AlternateDataStreamInfo"/> instances for the directory.</summary>
      /// <returns>An enumerable collection of <see cref="T:AlternateDataStreamInfo"/> of type <see cref="T:StreamType"/> instances for the directory.</returns>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public IEnumerable<AlternateDataStreamInfo> EnumerateStreams(StreamType streamType)
      {
         return AlternateDataStreamInfo.EnumerateStreamsInternal(true, Transaction, null, LongFullName, null, streamType, null);
      }

      #endregion // EnumerateStreams

      #region GetDirName

      private static string GetDirName(string path)
      {
         return path.Length > 3 ? Path.GetFileName(Path.RemoveDirectorySeparator(path, false), true) : path;
      }

      #endregion // GetDirName

      #region GetDisplayName

      private static string GetDisplayName(string path)
      {
         return path.Length != 2 || (path[1] != Path.VolumeSeparatorChar) ? path : Path.CurrentDirectoryPrefix;
      }

      #endregion // GetDisplayName

      #region GetStreamSize

      /// <summary>[AlphaFS] Retrieves the actual number of bytes of disk storage used by all data streams (NTFS ADS).</summary>
      /// <returns>The number of bytes used by all data streams.</returns>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public long GetStreamSize()
      {
         return AlternateDataStreamInfo.GetStreamSizeInternal(true, Transaction, null, LongFullName, null, null, null);
      }

      /// <summary>[AlphaFS] Retrieves the actual number of bytes of disk storage used by a named data streams (NTFS ADS).</summary>
      /// <param name="name">The name of the stream to retrieve.</param>
      /// <returns>The number of bytes used by a named stream.</returns>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public long GetStreamSize(string name)
      {
         return AlternateDataStreamInfo.GetStreamSizeInternal(true, Transaction, null, LongFullName, name, StreamType.Data, null);
      }

      /// <summary>[AlphaFS] Retrieves the actual number of bytes of disk storage used by a <see cref="T:StreamType"/> data streams (NTFS ADS).</summary>
      /// <param name="type">The <see cref="T:StreamType"/> of the stream to retrieve.</param>
      /// <returns>The number of bytes used by stream of type <see cref="T:StreamType"/>.</returns>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public long GetStreamSize(StreamType type)
      {
         return AlternateDataStreamInfo.GetStreamSizeInternal(true, Transaction, null, LongFullName, null, type, null);
      }

      #endregion GetStreamSize

      #region RemoveStream

      /// <summary>[AlphaFS] Removes all alternate data streams (NTFS ADS) from the directory.</summary>
      /// <remarks>This method only removes streams of type <see cref="T:StreamType.AlternateData"/>.</remarks>
      /// <remarks>No Exception is thrown if the stream does not exist.</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public void RemoveStream()
      {
         AlternateDataStreamInfo.RemoveStreamInternal(true, Transaction, LongFullName, null, null);
      }

      /// <summary>[AlphaFS] Removes an alternate data stream (NTFS ADS) from the directory.</summary>
      /// <param name="name">The name of the stream to remove.</param>
      /// <remarks>This method only removes streams of type <see cref="T:StreamType.AlternateData"/>.</remarks>
      /// <remarks>No Exception is thrown if the stream does not exist.</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public void RemoveStream(string name)
      {
         AlternateDataStreamInfo.RemoveStreamInternal(true, Transaction, LongFullName, name, null);
      }

      #endregion // RemoveStream


      #region Unified Internals

      #region CreateSubdirectoryInternal

      /// <summary>[AlphaFS] Unified method CreateSubdirectory() to create a subdirectory or subdirectories on the specified path. The specified path can be relative to this instance of the DirectoryInfo class.</summary>
      /// <param name="path">The specified path. This cannot be a different disk volume or Universal Naming Convention (UNC) name.</param>
      /// <param name="templatePath">The path of the directory to use as a template when creating the new directory.</param>
      /// <param name="directorySecurity">The <see cref="T:DirectorySecurity"/> security to apply.</param>
      /// <param name="compress">When <c>true</c> compresses the directory.</param>
      /// <returns>The last directory specified in path as an <see cref="T:DirectoryInfo"/> object.</returns>
      /// <remarks>
      /// Any and all directories specified in path are created, unless some part of path is invalid.
      /// The path parameter specifies a directory path, not a file path.
      /// If the subdirectory already exists, this method does nothing.
      /// </remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      private DirectoryInfo CreateSubdirectoryInternal(string path, string templatePath, DirectorySecurity directorySecurity, bool compress)
      {
         string pathLp = Path.CombineInternal(false, LongFullName, path);

         if (string.Compare(LongFullName, 0, pathLp, 0, LongFullName.Length, StringComparison.OrdinalIgnoreCase) != 0)
            throw new ArgumentException("Invalid SubPath", pathLp);

         return Directory.CreateDirectoryInternal(Transaction, pathLp, templatePath, directorySecurity, compress, true);
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
      /// <param name="isFullPath">
      /// <para><c>true</c> <paramref name="destinationPath"/> is an absolute path. Unicode prefix is applied.</para>
      /// <para><c>false</c> <paramref name="destinationPath"/> will be checked and resolved to an absolute path. Unicode prefix is applied.</para>
      /// <para><c>null</c> <paramref name="destinationPath"/> is already an absolute path with Unicode prefix. Use as is.</para>
      /// </param>
      /// <returns>When <paramref name="isMove"/> is <c>true</c> <c>null</c> is returned. Otherwise copy; a new <see cref="T:DirectoryInfo"/> instance with a fully qualified path returned.</returns>
      /// <remarks>MSDN: .NET 4+ Trailing spaces are removed from the end of the <paramref name="destinationPath"/> parameter before copying/moving the directory.</remarks>
      /// <remarks>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method. If two files have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior</remarks>
      /// <remarks>This Move method works across disk volumes, and it does not throw an exception if the source and destination are
      /// the same. Note that if you attempt to replace a file by moving a file of the same name into that directory, you
      /// get an IOException. You cannot use the Move method to overwrite an existing file.
      /// </remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      private DirectoryInfo CopyToMoveToInternal(bool isMove, string destinationPath, bool preserveSecurity, CopyOptions? copyOptions, MoveOptions? moveOptions, CopyMoveProgressCallback copyProgress, object userProgressData, bool? isFullPath)
      {
         string destinationPathLp = isFullPath == null
            ? destinationPath
            : (bool) isFullPath
            ? Path.GetLongPathInternal(destinationPath, false, false, false, false)
#if NET35
               : Path.GetFullPathInternal(null, destinationPath, true, false, false, true, false, true);
#else
               : Path.GetFullPathInternal(null, destinationPath, true, true, false, true, false, true);
#endif


         Directory.CopyMoveInternal(isMove, Transaction, LongFullName, destinationPathLp, preserveSecurity, copyOptions, moveOptions, copyProgress, userProgressData, null);

         if (isMove)
         {
            LongFullName = destinationPathLp;
            FullPath = Path.GetRegularPathInternal(destinationPathLp, false, false, false, false);

            OriginalPath = destinationPath;
            DisplayPath = GetDisplayName(OriginalPath);

            // Flush any cached information about the file.
            Reset();
         }

         return isMove ? null : new DirectoryInfo(Transaction, destinationPathLp, null);
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
         get { return (EntryInfo != null && EntryInfo.IsDirectory); }
      }

      #endregion // Exists

      #region Name

      /// <summary>Gets the name of this <see cref="T:DirectoryInfo"/> instance.</summary>
      /// <returns>The directory name.</returns>
      /// <remarks>Returns only the name of the directory, such as "Bin". To get the full path, such as "c:\public\Bin", use the FullName property.</remarks>
      public override string Name
      {
         get { return GetDirName(FullPath); }
      }

      #endregion // Name

      #region Parent

      /// <summary>Gets the parent directory of a specified subdirectory.</summary>
      /// <returns>The parent directory, or <c>null</c> if the path is null or if the file path denotes a root (such as "\", "C:", or * "\\server\share").</returns>
      public DirectoryInfo Parent
      {
         get
         {
            string path = FullPath;

            if (path.Length > 3)
               path = Path.RemoveDirectorySeparator(FullPath, false);

            string dirName = Path.GetDirectoryName(path);
            return dirName == null ? null : new DirectoryInfo(Transaction, dirName, true, true);
         }
      }

      #endregion // Parent

      #region Root

      /// <summary>Gets the root portion of the directory.</summary>
      /// <returns>An object that represents the root of the directory.</returns>
      public DirectoryInfo Root
      {
         get { return new DirectoryInfo(Transaction, Path.GetPathRoot(FullPath, false), false); }
      }

      #endregion // Root

      #endregion // .NET

      #endregion // Properties
   }
}