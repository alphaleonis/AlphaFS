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
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace Alphaleonis.Win32.Filesystem
{
    /// <summary>
    /// Provides the base class for both FileInfo and DirectoryInfo objects.
    /// </summary>
    [SerializableAttribute]
    [ComVisibleAttribute(true)]
    [SecurityPermission(SecurityAction.InheritanceDemand, UnmanagedCode = true)]
    [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode=true)]
    public abstract class FileSystemInfo : MarshalByRefObject
    {
        #region Fields
        /// <summary>
        /// Indicator of file existence. It refreshes each time Refresh() has been called.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        protected bool mExists;

        /// <summary>
        /// Represents extended file information.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        protected FileSystemEntryInfo mFileSystemEntryInfo;

        /// <summary>
        /// Represents the fully qualified path of the directory or file.
        /// </summary>
        /// <remarks>
        /// Notes to Inheritors: 
        /// Classes derived from FileSystemInfo can use the FullPath field to determine the full path of the object being manipulated.
        /// </remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        protected string FullPath;

        /// <summary>
        /// The path originally specified by the user, whether relative or absolute.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        protected string OriginalPath;

        #endregion

        #region Properties

        /// <summary>
        /// Gets a full path string representing the file's parent directory.
        /// </summary>
        /// <value>A string representing the parent directory full path.</value>
        public string DirectoryName
        {
            get
            {
                return Alphaleonis.Win32.Filesystem.Directory.GetParent(FullPath);
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="FileAttributes"/> of the current <see cref="FileSystemInfo"/>. 
        /// </summary>
        public FileAttributes Attributes
        {
            get
            {
                return mFileSystemEntryInfo.Attributes;
            }

            [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
            set
            {
                if (mExists)
                {
                    File.SetAttributes(FullPath, value);
                }
                else
                {
                    ThrowDoesNotExistsException();
                }
            }
        }

        /// <summary>
        /// Gets or sets the creation time of the current <see cref="FileSystemInfo"/> object.
        /// </summary>
        /// <returns>The creation date and time of the current System.IO.FileSystemInfo object.</returns>
        public DateTime CreationTime
        {
            get
            {
                if (mExists)
                {
                    return mFileSystemEntryInfo.Created;
                }
                else
                {
                    return DateTime.MinValue;
                }
            }

            [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
            set
            {
                if (mExists)
                {
                    File.SetCreationTime(FullPath, value);
                }
                else
                {
                    ThrowDoesNotExistsException();
                }
            }
        }

        /// <summary>
        /// Gets or sets the creation time, in coordinated universal time (UTC), of the current FileSystemInfo object.
        /// </summary>
        [ComVisibleAttribute(false)]
        public DateTime CreationTimeUtc
        {
            get
            {
                if (mExists)
                {
                    return mFileSystemEntryInfo.Created.ToUniversalTime();
                }
                else
                {
                    return DateTime.MinValue;
                }
            }

            [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
            set
            {
                if (mExists)
                {
                    File.SetCreationTimeUtc(FullPath, value);
                }
                else
                {
                    ThrowDoesNotExistsException();
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether the file or directory exists.
        /// </summary>
        public abstract bool Exists { get; }

        /// <summary>
        /// The Extension property returns the FileSystemInfo extension, including the period (.). For example, for a file c:\NewFile.txt, this property returns ".txt".
        /// </summary>
        public string Extension
        {
            get
            {
                return Path.GetExtension(OriginalPath);
            }
        }

        /// <summary>
        /// A string containing the name with full path.
        /// </summary>
        public virtual string FullName { get { return FullPath; } }
        //SecurityException The caller does not have the required permission.

		/// <summary>
		/// Gets the system info.
		/// </summary>
		/// <value>The system info.</value>
		public FileSystemEntryInfo SystemInfo
		{
			get
			{
				return mFileSystemEntryInfo;
			}
		}

        /// <summary>
        /// Gets or sets the time the current file or directory was last accessed. 
        /// </summary>
        /// <remarks>When first called, <see cref="FileSystemInfo"/> calls Refresh and returns the cached information on APIs to get attributes and so on. On subsequent calls, you must call Refresh to get the latest copy of the information. 
        /// If the file described in the <see cref="FileSystemInfo"/> object does not exist, this property will return 12:00 midnight, January 1, 1601 A.D. (C.E.) Coordinated Universal Time (UTC), adjusted to local time. 
        /// </remarks>
        public DateTime LastAccessTime
        {
            get
            {
                if (mExists)
                {
                    return mFileSystemEntryInfo.LastAccessed;
                }
                else
                {
                    return DateTime.MinValue;
                }
            }

            [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
            set
            {
                if (mExists)
                {
                    File.SetLastAccessTime(FullPath, value);
                }
                else
                {
                    ThrowDoesNotExistsException();
                }
            }
        }

        /// <summary>
        /// Gets or sets the time, in coordinated universal time (UTC), that the current file or directory was last accessed
        /// </summary>
        /// <remarks>When first called, <see cref="FileSystemInfo"/> calls Refresh and returns the cached information on APIs to get attributes and so on. On subsequent calls, you must call Refresh to get the latest copy of the information. 
        /// If the file described in the <see cref="FileSystemInfo"/> object does not exist, this property will return 12:00 midnight, January 1, 1601 A.D. (C.E.) Coordinated Universal Time (UTC), adjusted to local time. 
        /// </remarks>
        [ComVisibleAttribute(false)]
        public DateTime LastAccessTimeUtc
        {
            [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
            get
            {
                if (mExists)
                {
                    return mFileSystemEntryInfo.LastAccessed.ToUniversalTime();
                }
                else
                {
                    return DateTime.MinValue;
                }
            }

            [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
            set
            {
                if (mExists)
                {
                    File.SetLastAccessTimeUtc(FullPath, value);
                }
                else
                {
                    ThrowDoesNotExistsException();
                }
            }
        }

        /// <summary>
        /// Gets or sets the time when the current file or directory was last written to.
        /// </summary>
        public DateTime LastWriteTime
        {
            get
            {
                if (mExists)
                {
                    return mFileSystemEntryInfo.LastModified;
                }
                else
                {
                    return DateTime.MinValue;
                }
            }

            [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
            set
            {
                if (mExists)
                {
                    File.SetLastWriteTime(FullPath, value);
                }
                else
                {
                    ThrowDoesNotExistsException();
                }
            }
        }

        /// <summary>
        /// Gets or sets the time, in coordinated universal time (UTC), when the current file or directory was last written to.
        /// </summary>
        [ComVisibleAttribute(false)]
        public DateTime LastWriteTimeUtc
        {
            get
            {
                if (mExists)
                {
                    return mFileSystemEntryInfo.LastModified.ToUniversalTime();
                }
                else
                {
                    return DateTime.MinValue;
                }
            }

            [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
            set
            {
                if (mExists)
                {
                    File.SetLastWriteTimeUtc(FullPath, value);
                }
                else
                {
                    ThrowDoesNotExistsException();
                }
            }
        }

        /// <summary>
        /// For files, gets the name of the file. For directories, gets the name of the last directory in the hierarchy if a hierarchy exists. Otherwise, the Name property gets the name of the directory.
        /// </summary>
        /// <remarks>
        /// For a directory, Name returns only the name of the parent directory, such as Dir, not c:\Dir. For a subdirectory, Name returns only the name of the subdirectory, such as Sub1, not c:\Dir\Sub1. 
        /// For a file, Name returns only the file name and file name extension, such as MyFile.txt, not c:\Dir\Myfile.txt. 
        /// </remarks>
        public abstract string Name { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Deletes a file or directory.
        /// </summary>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)] 
        public abstract void Delete();

        /// <summary>
        /// Refreshes the state of the object.
        /// </summary>
        /// <remarks>
        /// FileSystemInfo.Refresh takes a snapshot of the file from the current file system. Refresh cannot correct the underlying file system even if the file system returns incorrect or outdated information. This can happen on platforms such as Windows 98. 
        /// Calls must be made to Refresh before attempting to get the attribute information, or the information will be outdated. 
        /// </remarks>
        public abstract void Refresh();

        /// <summary>
        /// Returns a String that represents the current Object. 
        /// </summary>
        /// <returns>String</returns>
        public new virtual string ToString()
        {
            return FullPath;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Throws the does not exists exception.
        /// </summary>
        protected void ThrowDoesNotExistsException()
        {
            throw new System.IO.FileNotFoundException("File or folder not found.", FullPath);
        }

        /// <summary>
        /// Initializes the specified file name.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        protected void Initialize(string fileName)
        {
            OriginalPath = fileName;
            FullPath = Path.GetFullPath(fileName);
            Refresh();
        }

        #endregion
    }
}
