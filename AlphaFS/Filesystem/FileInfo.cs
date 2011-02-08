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
using System.Globalization;

namespace Alphaleonis.Win32.Filesystem
{
	/// <summary>
	/// Provides instance methods for the creation, copying, deletion, moving, and opening of files, and aids in the creation of FileStream objects.
	/// This class cannot be inherited.
	/// </summary>
	[Serializable]
	[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
	public sealed class FileInfo : FileSystemInfo
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the FileInfo class, which acts as a wrapper for a file path. 
		/// </summary>
		/// <param name="filePath">The path to the file.</param>
		/// <exception cref="ArgumentNullException"><paramref name="filePath"/> is a <see langword="null"/> reference.</exception>
		/// <remarks>
		/// You can specify either the fully qualified or the relative file name, but the security check gets the fully qualified name.
		/// </remarks>
		public FileInfo(string filePath)
		{
			Initialize(filePath);
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets an instance of the parent directory.
		/// </summary>
		/// <value>A <see cref="DirectoryInfo"/> object representing the parent directory of this file.</value>
		/// <remarks>To get the parent directory as a string, use the DirectoryName property.</remarks>
		/// 
		public DirectoryInfo Directory
		{
			get
			{
				return new DirectoryInfo(DirectoryName);
			}
		}

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
		/// Gets or sets a value that determines if the current file is read only.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if the current file is read only; otherwise, <c>false</c>.
		/// </value>
		public bool IsReadOnly
		{
			get
			{
				return ((Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly) ? true : false;
			}

			[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
			set
			{
				if (value)
				{
					Attributes |= FileAttributes.ReadOnly;
				}
				else
				{
					if (IsReadOnly)
					{
						Attributes = (FileAttributes)(Attributes - FileAttributes.ReadOnly);
					}
				}
			}
		}

		/// <summary>
		/// Gets the name of the file with extension.
		/// </summary>
		/// <value>File name with extension.</value>
		/// <remarks>
		/// For a file, Name returns only the file name and file name extension, such as MyFile.txt, not c:\Dir\Myfile.txt.
		/// </remarks>
		public override string Name
		{
			get
			{
				return Path.GetFileName(FullPath);
			}
		}

		/// <summary>
		/// Gets the file size.
		/// </summary>
		/// <value>The file size.</value>
      [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
      public long Length
		{
			get
			{
				if (mExists)
				{
					return mFileSystemEntryInfo.FileSize;
				}
				else
				{
					throw new System.IO.FileNotFoundException();
				}
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
              if (mFileSystemEntryInfo != null && mFileSystemEntryInfo.IsFile)
                  mExists = true;
          }
          catch
          {
              mExists = false;
              mFileSystemEntryInfo = null;
          }
      }

		/// <summary>
		/// Deletes a file.
		/// </summary>
		[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		public override void Delete()
		{
			File.Delete(FullPath);
			Refresh();
		}

		/// <summary>
		/// Returns the path as a string.
		/// </summary>
		/// <returns>A string representing the path.</returns>
		public override string ToString()
		{
			return FullPath;
		}

		/// <summary>
		/// Creates a <see cref="System.IO.StreamWriter"/> that appends text to the file represented by this instance of the <see cref="FileInfo"/>.
		/// </summary>
		/// <returns>A <see cref="System.IO.StreamWriter"/> that appends UTF-8 encoded text to an existing file.</returns>
		[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		public System.IO.StreamWriter AppendText()
		{
			return File.AppendText(FullPath);
		}

		/// <summary>
		/// Copies an existing file to a new file, disallowing the overwriting of an existing file.
		/// </summary>
		/// <param name="destFileName">The name of the new file to copy to.</param>
		/// <returns><see cref="FileInfo"/>A new file with a fully qualified path.</returns>
		[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		public FileInfo CopyTo(string destFileName)
		{
			File.Copy(FullPath, Path.GetLongPath(destFileName), false);
			return new FileInfo(destFileName);
		}

		/// <summary>
		/// Copies an existing file to a new file, allowing the overwriting of an existing file. 
		/// </summary>
		/// <param name="destFileName">The name of the new file to copy to.</param>
		/// <param name="overwrite"><c>true</c> to allow an existing file to be overwritten; otherwise, <c>false</c>.</param>
        /// <returns><see cref="FileInfo"/>A new file, or an overwrite of an existing file if overwrite is true. If the file exists and overwrite is false, an IOException is thrown.</returns>
		[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		public FileInfo CopyTo(string destFileName, bool overwrite)
		{
			File.Copy(FullPath, Path.GetLongPath(destFileName), overwrite);
			return new FileInfo(destFileName);
		}

		/// <summary>
		/// Creates a file.
		/// </summary>
        /// <returns><see cref="System.IO.FileStream"/>A new file.</returns>
		[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		public System.IO.FileStream Create()
		{
			return File.Create(FullPath);
		}

		/// <summary>
        /// Creates a <see crefe="System.IO.StreamWriter"/> that writes a new text file. 
		/// </summary>
        /// <returns>A new <see cref="System.IO.StreamWriter"/></returns>
		[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		public System.IO.StreamWriter CreateText()
		{
			return File.CreateText(FullPath);
		}

		/// <summary>
		/// Decrypts a file that was encrypted by the current account using the <see cref="FileInfo.Encrypt"/> method.
		/// </summary>
		/// <remarks>The Decrypt method allows you to decrypt a file that was encrypted using the Encrypt method.
		/// The Decrypt method can decrypt only files that were encrypted using the current user account.
		/// Both the Encrypt method and the Decrypt method use the cryptographic service provider (CSP) installed on the computer and the file encryption keys of the process calling the method.
		/// The current file system must be formatted as NTFS and the current operating system must be Microsoft Windows NT or later.
		/// </remarks>
		[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		public void Decrypt()
		{
			File.Decrypt(FullPath);
		}

		/// <summary>
		/// Encrypts a file so that only the account used to encrypt the file can decrypt it.
		/// </summary>
		/// <remarks>
		/// The Encrypt method allows you to encrypt a file so that only the account used to call this method can decrypt it. Use the Decrypt method to decrypt a file encrypted by the Encrypt method. 
		/// Both the Encrypt method and the Decrypt method use the cryptographic service provider (CSP) installed on the computer and the file encryption keys of the process calling the method. 
		/// The current file system must be formatted as NTFS and the current operating system must be Microsoft Windows NT or later.
		/// </remarks>
		[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		public void Encrypt()
		{
			File.Encrypt(FullPath);
		}

		/// <summary>
		/// Gets a <see cref="System.Security.AccessControl.FileSecurity"/> object that encapsulates the access control list (ACL) entries for the file described by the current <see cref="FileInfo"/> object.
		/// </summary>
        /// <returns><see cref="System.Security.AccessControl.FileSecurity"/>A FileSecurity object that encapsulates the access control rules for the current file. </returns>
		[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		public System.Security.AccessControl.FileSecurity GetAccessControl()
		{
			return File.GetAccessControl(FullPath);
		}

		/// <summary>
		/// Gets a <see cref="System.Security.AccessControl.FileSecurity"/> object that encapsulates the specified type of access control list (ACL) entries for the file described by the current FileInfo object.
		/// </summary>
		/// <param name="includeSections">The include sections.</param>
        /// <returns><see cref="System.Security.AccessControl.FileSecurity"/> object that encapsulates the specified type of access control list (ACL) entries for the file described by the current FileInfo object.</returns>
		[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		public System.Security.AccessControl.FileSecurity GetAccessControl(System.Security.AccessControl.AccessControlSections includeSections)
		{
			return File.GetAccessControl(FullPath, includeSections);
		}

		/// <summary>
		/// Moves a specified file to a new location, providing the option to specify a new file name.
		/// </summary>
		/// <param name="destFileName">The path to move the file to, which can specify a different file name.</param>
		/// <remarks>This method works across disk volumes. For example, the file c:\MyFile.txt can be moved to d:\public and renamed NewFile.txt.</remarks>
		[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		public void MoveTo(string destFileName)
		{
			File.Move(FullPath, Path.GetLongPath(destFileName));
			Initialize(destFileName);
		}

		#region Open
		/// <summary>
		/// Opens a file in the specified mode.
		/// </summary>
		/// <param name="mode">A FileMode constant specifying the mode (for example, Open or Append) in which to open the file.</param>
        /// <returns><see cref="System.IO.FileStream"/>A file opened in the specified mode, with read/write access and unshared.</returns>
		[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		public System.IO.FileStream Open(FileMode mode)
		{
			return File.Open(FullPath, mode);
		}

		/// <summary>
		/// Opens a file in the specified mode with read, write, or read/write access. 
		/// </summary>
		/// <param name="mode">A FileMode constant specifying the mode (for example, Open or Append) in which to open the file.</param>
        /// <param name="access"><see cref="System.IO.FileAccess"/>A FileAccess constant specifying whether to open the file with Read, Write, or ReadWrite file access. </param>
        /// <returns><see cref="System.IO.FileStream"/>A file opened in the specified mode, with read/write access and unshared.</returns>
		[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		public System.IO.FileStream Open(FileMode mode, FileAccess access)
		{
			return File.Open(FullPath, mode, access);
		}

		/// <summary>
		/// Opens a file in the specified mode with read, write, or read/write access.
		/// </summary>
		/// <param name="mode">A FileMode constant specifying the mode (for example, Open or Append) in which to open the file.</param>
        /// <param name="access"><see cref="System.IO.FileAccess"/>A FileAccess constant specifying whether to open the file with Read, Write, or ReadWrite file access.</param>
		/// <param name="share">A FileShare constant specifying the type of access other FileStream objects have to this file.</param>
		/// <returns><see cref="System.IO.FileStream"/>A file opened in the specified mode, with read/write access and unshared.</returns>
		[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		public System.IO.FileStream Open(FileMode mode, FileAccess access, FileShare share)
		{
			return File.Open(FullPath, mode, access, share);
		}

		/// <summary>
		/// Creates a read-only FileStream.
		/// </summary>
		/// <returns>A new read-only FileStream object.</returns>
		[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		public System.IO.FileStream OpenRead()
		{
			return File.OpenRead(FullPath);
		}

		/// <summary>
		/// Creates a StreamReader with UTF8 encoding that reads from an existing text file.
		/// </summary>
		/// <returns>A new StreamReader with UTF8 encoding.</returns>
		[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		public System.IO.StreamReader OpenText()
		{
			return File.OpenText(FullPath);
		}

		/// <summary>
		/// Creates a write-only FileStream.
		/// </summary>
		/// <returns>A new write-only unshared FileStream object.</returns>
		[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		public System.IO.FileStream OpenWrite()
		{
			return File.OpenWrite(FullPath);
		}
		#endregion

		#region Replace

		/// <summary>
		/// Replaces the contents of a specified file with the file described by the current FileInfo object, deleting the original file, and creating a backup of the replaced file.
		/// </summary>
        /// <param name="destinationFileName"><see cref="System.String"/>The name of a file to replace with the current file.</param>
        /// <param name="destinationBackupFileName"><see cref="System.String"/>The name of a file with which to create a backup of the file described by the destFileName parameter.</param>
        /// <returns><see cref="FileInfo"/>A FileInfo object that encapsulates information about the file described by the destFileName parameter.</returns>
		/// <remarks>The Replace method replaces the contents of a specified file with the contents of the file described by the current FileInfo object. It also creates a backup of the file that was replaced. Finally, it returns a new FileInfo object that describes the overwritten file.</remarks>
		[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		public FileInfo Replace(string destinationFileName, string destinationBackupFileName)
		{
			return Replace(destinationFileName, destinationBackupFileName, false);
		}

		/// <summary>
		/// Replaces the specified destination file name.
		/// </summary>
        /// <param name="destinationFileName"><see cref="System.String"/>The name of a file to replace with the current file.</param>
        /// <param name="destinationBackupFileName"><see cref="System.String"/>The name of a file with which to create a backup of the file described by the destFileName parameter.</param>
		/// <param name="ignoreMetadataErrors">true to ignore merge errors (such as attributes and ACLs) from the replaced file to the replacement file; otherwise false.</param>
        /// <returns><see cref="FileInfo"/>A FileInfo object that encapsulates information about the file described by the destFileName parameter.</returns>
		/// <remarks>The Replace method replaces the contents of a specified file with the contents of the file described by the current FileInfo object. It also creates a backup of the file that was replaced. Finally, it returns a new FileInfo object that describes the overwritten file.
		/// The last parameter <paramref name="ignoreMetadataErrors"/> is not supported yet.
		/// </remarks>
		[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		public FileInfo Replace(string destinationFileName, string destinationBackupFileName, bool ignoreMetadataErrors)
		{
			File.Replace(this.FullPath, destinationFileName, destinationBackupFileName, ignoreMetadataErrors);
			return new FileInfo(destinationFileName);
		}

		#endregion

		/// <summary>
		/// Applies access control list (ACL) entries described by a FileSecurity object to the file described by the current FileInfo object.
		/// </summary>
        /// <param name="fileSecurity"><see cref="System.Security.AccessControl"/>A FileSecurity object that describes an access control list (ACL) entry to apply to the current file.</param>
		/// <remarks>The SetAccessControl method applies access control list (ACL) entries to the current file that represents the noninherited ACL list. 
		/// Use the SetAccessControl method whenever you need to add or remove ACL entries from a file.
		/// </remarks>
		[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		public void SetAccessControl(System.Security.AccessControl.FileSecurity fileSecurity)
		{
			File.SetAccessControl(FullPath, fileSecurity, System.Security.AccessControl.AccessControlSections.All);
		}

		#endregion

	}
}
