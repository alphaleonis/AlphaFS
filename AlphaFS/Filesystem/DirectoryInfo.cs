/* Copyright (c) 2008-2009 Peter Palotas
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
using System.Security.Permissions;

namespace Alphaleonis.Win32.Filesystem
{
	/// <summary>
	/// Exposes instance methods for creating, moving, and enumerating through directories and subdirectories. This class cannot be inherited.
	/// </summary>
	[Serializable]
	[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
	public sealed class DirectoryInfo : FileSystemInfo
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DirectoryInfo"/> class on the specified dirPath.
		/// </summary>
		/// <param name="dirPath">A string specifying the path on which to create the DirectoryInfo.</param>
		public DirectoryInfo(string dirPath)
		{
			Initialize(dirPath);
		}

		#region Properties

		/// <summary>
		/// Gets a value indicating whether the file or directory exists.
		/// </summary>
		/// <value><c>true</c> if the file exists; otherwise, <c>false</c>.</value>
		public override bool Exists
		{
			get
			{
				return mExists;
			}
		}

		/// <summary>
		/// Gets the name of the last directory in the hierarchy if a hierarchy exists. Otherwise, the Name property gets the name of the directory.
		/// </summary>
		/// <value></value>
		/// <remarks>
		/// For a directory, Name returns only the name of the parent directory, such as Dir, not c:\Dir. For a subdirectory, Name returns only the name of the subdirectory, such as Sub1, not c:\Dir\Sub1.
		/// </remarks>
		public override string Name
		{
			get
			{
				if (Path.GetPathRoot(FullPath) == FullPath)
				{
					return FullPath;
				}
				else
					return new PathInfo(FullPath).FileName;
			}
		}

		/// <summary>
		/// Gets the parent directory of a specified subdirectory.
		/// </summary>
		/// <value>The parent directory, or nullNothingnullptra null reference (Nothing in Visual Basic) if the path is null or if the file path denotes a root (such as "\", "C:", or * "\\server\share").</value>
		public DirectoryInfo Parent
		{
			get
			{
				return new DirectoryInfo(Directory.GetParent(FullPath));
			}
		}

		/// <summary>
		/// Gets the root portion of a path.
		/// </summary>
		/// <value>A DirectoryInfo object representing the root of a path.</value>
		public DirectoryInfo Root
		{
			get
			{
				return new DirectoryInfo(Directory.GetDirectoryRoot(FullPath));
			}
		}

		#endregion

		#region Methods

		/// <summary>
		/// Refreshes the state of the object.
		/// </summary>
		/// <remarks>
		/// FileSystemInfo.Refresh takes a snapshot of the file from the current file system. Refresh cannot correct the underlying file system even if the file system returns incorrect or outdated information. This can happen on platforms such as Windows 98.
		/// Calls must be made to Refresh before attempting to get the attribute information, or the information will be outdated.
		/// </remarks>
		public override void Refresh()
		{
            try
            {
                mFileSystemEntryInfo = File.GetFileSystemEntryInfo(FullPath);
                if (mFileSystemEntryInfo != null && mFileSystemEntryInfo.IsDirectory)
                    mExists = true;
            }
            catch (Exception)
            {
                mExists = false;
                mFileSystemEntryInfo = null;
            }
		}

		/// <summary>
		/// Deletes a file or directory.
		/// </summary>
		public override void Delete()
		{
			Directory.Delete(FullPath);
			Refresh();
		}

		/// <summary>
		/// Deletes this instance of a DirectoryInfo, specifying whether to delete subdirectories and files.
		/// </summary>
		/// <param name="recursive"><c>true</c> to remove directories, subdirectories, and files in path; otherwise, <c>false</c>.</param>
		/// <remarks>
		/// If the DirectoryInfo has no files or subdirectories, this method deletes the DirectoryInfo even if recursive is false. Attempting to delete a DirectoryInfo that is not empty when recursive is false throws an IOException.
		/// For a list of common I/O tasks, see Common I/O Tasks.
		/// </remarks>
		public void Delete(bool recursive)
		{
			Directory.Delete(FullPath, recursive);
			Refresh();
		}

		/// <summary>
		/// Deletes this instance of a DirectoryInfo, specifying whether to delete subdirectories and files.
		/// </summary>
		/// <param name="recursive"><c>true</c> to remove directories, subdirectories, and files in path; otherwise, <c>false</c>.</param>
		/// <param name="ignoreReadOnly">if set to <c>true</c> ignores read only attribute of files and directories.</param>
		/// <remarks>
		/// If the DirectoryInfo has no files or subdirectories, this method deletes the DirectoryInfo even if recursive is false. Attempting to delete a DirectoryInfo that is not empty when recursive is false throws an IOException.
		/// For a list of common I/O tasks, see Common I/O Tasks.
		/// </remarks>
		public void Delete(bool recursive, bool ignoreReadOnly)
		{
			Directory.Delete(FullPath, recursive, ignoreReadOnly);
			Refresh();
		}

		/// <summary>
		/// Returns the original path that was passed by the user.
		/// </summary>
		/// <returns>Returns the original path that was passed by the user.</returns>
		public override string ToString()
		{
			return OriginalPath;
		}

		/// <summary>
		/// Gets a DirectorySecurity object that encapsulates the access control list (ACL) entries for the directory described by the current DirectoryInfo object.
		/// </summary>
		/// <returns>A DirectorySecurity object that encapsulates the access control rules for the directory.</returns>
		public System.Security.AccessControl.DirectorySecurity GetAccessControl()
		{
			return GetAccessControl(System.Security.AccessControl.AccessControlSections.Access | System.Security.AccessControl.AccessControlSections.Owner | System.Security.AccessControl.AccessControlSections.Group);
		}

		/// <summary>
		/// Gets a DirectorySecurity object that encapsulates the specified type of access control list (ACL) entries for the directory described by the current DirectoryInfo object.
		/// </summary>
		/// <param name="includeSections">One of the AccessControlSections values that specifies the type of access control list (ACL) information to receive.</param>
		/// <returns>A DirectorySecurity object that encapsulates the access control rules for the file described by the path parameter.</returns>
		public System.Security.AccessControl.DirectorySecurity GetAccessControl(System.Security.AccessControl.AccessControlSections includeSections)
		{
			return Directory.GetAccessControl(FullPath, includeSections);
		}

		/// <summary>
		/// Applies access control list (ACL) entries described by a DirectorySecurity object to the directory described by the current DirectoryInfo object.
		/// </summary>
		/// <param name="directorySecurity">A DirectorySecurity object that describes an ACL entry to apply to the directory described by the path parameter.</param>
		public void SetAccessControl(System.Security.AccessControl.DirectorySecurity directorySecurity)
		{
			Directory.SetAccessControl(FullPath, directorySecurity, System.Security.AccessControl.AccessControlSections.All);
		}

		/// <summary>
		/// Returns the subdirectories of the current directory.
		/// </summary>
		/// <returns>An array of DirectoryInfo objects.</returns>
		/// <remarks>If there are no subdirectories, this method returns an empty array. This method is not recursive.</remarks>
		public DirectoryInfo[] GetDirectories()
		{
			return GetDirectories("*", System.IO.SearchOption.TopDirectoryOnly);
		}

		/// <summary>
		/// Returns an array of directories in the current DirectoryInfo matching the given search criteria.
		/// </summary>
		/// <param name="searchPattern">The search string, such as "System*", used to search for all directories beginning with the word "System".</param>
		/// <returns>An array of type DirectoryInfo matching searchPattern.</returns>
		/// <remarks>
		/// Wildcards are permitted. For example, the searchPattern string "*t" searches for all directory names in path ending with the letter "t". The searchPattern string "s*" searches for all directory names in path beginning with the letter "s".
		/// The string ".." can only be used in searchPattern if it is specified as a part of a valid directory name, such as in the directory name "a..b". It cannot be used to move up the directory hierarchy.
		/// If there are no subdirectories, or no subdirectories match the searchPattern parameter, this method returns an empty array.
		/// For a list of common I/O tasks, see Common I/O Tasks.
		/// </remarks>
		public DirectoryInfo[] GetDirectories(string searchPattern)
		{
			return GetDirectories(searchPattern, System.IO.SearchOption.TopDirectoryOnly);
		}

		/// <summary>
		/// Returns an array of directories in the current DirectoryInfo matching the given search criteria and using a value to determine whether to search subdirectories.
		/// </summary>
		/// <param name="searchPattern">The search string, such as "System*", used to search for all directories beginning with the word "System".</param>
		/// <param name="searchOption">One of the values of the SearchOption enumeration that specifies whether the search operation should include only the current directory or should include all subdirectories.</param>
		/// <returns>
		/// An array of type DirectoryInfo matching searchPattern.
		/// </returns>
		/// <remarks>
		/// Wildcards are permitted. For example, the searchPattern string "*t" searches for all directory names in path ending with the letter "t". The searchPattern string "s*" searches for all directory names in path beginning with the letter "s".
		/// The string ".." can only be used in searchPattern if it is specified as a part of a valid directory name, such as in the directory name "a..b". It cannot be used to move up the directory hierarchy.
		/// If there are no subdirectories, or no subdirectories match the searchPattern parameter, this method returns an empty array.
		/// For a list of common I/O tasks, see Common I/O Tasks.
		/// </remarks>
		public DirectoryInfo[] GetDirectories(string searchPattern, System.IO.SearchOption searchOption)
		{
			if (null == searchPattern)
				throw new ArgumentNullException("searchPattern");

			string[] directories = Directory.GetDirectories(FullPath, searchPattern, searchOption);
			System.Collections.Generic.List<DirectoryInfo> dirList = new System.Collections.Generic.List<DirectoryInfo>(directories.Length);
			foreach (string dir in directories)
			{
				DirectoryInfo difo = new DirectoryInfo(dir);
				dirList.Add(difo);
			}

			return dirList.ToArray();
		}

		/// <summary>
		/// Returns a file list from the current directory.
		/// </summary>
		/// <returns>An array of type FileInfo.</returns>
		public FileInfo[] GetFiles()
		{
			return GetFiles("*", System.IO.SearchOption.TopDirectoryOnly);
		}

		/// <summary>
		/// Returns a file list from the current directory matching the given searchPattern.
		/// </summary>
		/// <param name="searchPattern">The search string, such as "*.txt".</param>
		/// <returns>An array of type FileInfo.</returns>
		public FileInfo[] GetFiles(string searchPattern)
		{
			return GetFiles(searchPattern, System.IO.SearchOption.TopDirectoryOnly);
		}

		/// <summary>
		/// Returns a file list from the current directory matching the given searchPattern and using a value to determine whether to search subdirectories.
		/// </summary>
		/// <param name="searchPattern">The search string, such as "System*", used to search for all directories beginning with the word "System".</param>
		/// <param name="searchOption">One of the values of the SearchOption enumeration that specifies whether the search operation should include only the current directory or should include all subdirectories.</param>
		/// <returns>An array of type FileInfo.</returns>
		/// <remarks>
		/// The following wildcard specifiers are permitted in searchPattern: "*" and "?".
		/// The order of the returned file names is not guaranteed; use the Sort()()() method if a specific sort order is required.
		/// Wildcards are permitted. For example, the searchPattern string "*.txt" searches for all file names having an extension of "txt".
		/// The searchPattern string "s*" searches for all file names beginning with the letter "s". If there are no files, or no files that match the searchPattern string in the DirectoryInfo, this method returns an empty array.
		/// </remarks>
		public FileInfo[] GetFiles(string searchPattern, System.IO.SearchOption searchOption)
		{
			if (null == searchPattern)
				throw new ArgumentNullException("searchPattern");

			string[] files = Directory.GetFiles(FullPath, searchPattern, searchOption);
			System.Collections.Generic.List<FileInfo> fileList = new System.Collections.Generic.List<FileInfo>(files.Length);
			foreach (string file in files)
			{
				FileInfo fifo = new FileInfo(file);
				fileList.Add(fifo);
			}

			return fileList.ToArray();
		}

		/// <summary>
		/// Returns an array of strongly typed FileSystemInfo entries representing all the files and subdirectories in a directory.
		/// </summary>
		/// <returns>An array of strongly typed FileSystemInfo entries.</returns>
		/// <remarks>
		/// This method is not recursive.
		/// For subdirectories, the FileSystemInfo objects returned by this method can be cast to the derived class DirectoryInfo. Use the FileAttributes value returned by the FileSystemInfo.Attributes property to determine whether the FileSystemInfo represents a file or a directory.
		/// Wild cards are permitted. For example, the searchPattern string "*t" searches for all directory names in path ending with the letter "t". The searchPattern string "s*" searches for all directory names in path beginning with the letter "s".
		/// The string ".." can only be used in searchPattern if it is specified as a part of a valid directory name, such as in the directory name "a..b". It cannot be used to move up the directory hierarchy. If there are no files or directories, or no files or directories that match the searchPattern string in the DirectoryInfo, this method returns an empty array.
		/// For a list of common I/O tasks, see Common I/O Tasks.
		/// </remarks>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos", Justification = "Microsoft chose this name, so we use it too.")]
		public FileSystemInfo[] GetFileSystemInfos()
		{
			return GetFileSystemInfos("*");
		}

		/// <summary>
		/// Retrieves an array of strongly typed FileSystemInfo objects representing the files and subdirectories matching the specified search criteria.
		/// </summary>
		/// <param name="searchPattern">The search string, such as "System*", used to search for all directories beginning with the word "System".</param>
		/// <returns>An array of strongly typed FileSystemInfo objects matching the search criteria.</returns>
		/// <remarks>
		/// This method is not recursive.
		/// For subdirectories, the FileSystemInfo objects returned by this method can be cast to the derived class DirectoryInfo. Use the FileAttributes value returned by the FileSystemInfo.Attributes property to determine whether the FileSystemInfo represents a file or a directory.
		/// Wild cards are permitted. For example, the searchPattern string "*t" searches for all directory names in path ending with the letter "t". The searchPattern string "s*" searches for all directory names in path beginning with the letter "s".
		/// The string ".." can only be used in searchPattern if it is specified as a part of a valid directory name, such as in the directory name "a..b". It cannot be used to move up the directory hierarchy. If there are no files or directories, or no files or directories that match the searchPattern string in the DirectoryInfo, this method returns an empty array.
		/// For a list of common I/O tasks, see Common I/O Tasks.
		/// </remarks>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos", Justification = "Microsoft chose this name, so we use it too.")]
		public FileSystemInfo[] GetFileSystemInfos(string searchPattern)
		{
			if (null == searchPattern)
				throw new ArgumentNullException("searchPattern");

			if (searchPattern.Trim().Length == 0 || searchPattern.EndsWith("..", StringComparison.Ordinal) ||
				searchPattern.Contains(".." + Path.DirectorySeparatorChar) ||
				searchPattern.Contains(".." + Path.AltDirectorySeparatorChar) ||
				searchPattern.IndexOfAny(Path.GetInvalidPathChars()) != -1)
			{
				throw new ArgumentException(Resources.InvalidSearchPattern, "searchPattern");
			}

			using (FileSystemEntryEnumerator enumerator = new FileSystemEntryEnumerator(FullPath + Path.DirectorySeparatorChar + searchPattern))
			{
				System.Collections.Generic.List<FileSystemInfo> entries = new System.Collections.Generic.List<FileSystemInfo>();

				while (enumerator.MoveNext())
				{
					if (enumerator.Current.IsDirectory)
					{
						entries.Add(new DirectoryInfo(FullPath + Path.DirectorySeparatorChar + enumerator.Current.FileName));
					}
					else
					{
						entries.Add(new FileInfo(FullPath + Path.DirectorySeparatorChar + enumerator.Current.FileName));
					}
				}

				return entries.ToArray();
			}
		}

		/// <summary>
		/// Moves a DirectoryInfo instance and its contents to a new path.
		/// </summary>
		/// <param name="destDirName">The name and path to which to move this directory.
		/// The destination cannot be directory with the identical name. It can be an existing directory to which you want to add this directory as a subdirectory.</param>
		/// <remarks>
		/// This method throws an IOException if, for example, you try to move c:\mydir to c:\public, and c:\public already exists. You must specify "c:\\public\\mydir" as the destDirName parameter, or specify a new directory name such as "c:\\newdir".
		/// This method permits moving a directory to a read-only directory. The read/write attribute of neither directory is affected.
		/// For a list of common I/O tasks, see Common I/O Tasks.
		/// </remarks>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "dest")]
		public void MoveTo(string destDirName)
		{
			if (null == destDirName)
				throw new ArgumentNullException("destDirName");

			if (Directory.Exists(destDirName))
				throw new AlreadyExistsException(destDirName);

			File.Move(FullPath, Path.GetLongPath(destDirName), MoveFileOptions.CopyAllowed);
			Initialize(destDirName);
		}

		#endregion
	}
}
