using System;
using System.Runtime.InteropServices;

namespace Alphaleonis.Win32.Filesystem
{
	/// <summary>
	/// Represents information retrieved by <see cref="File.GetFileInformationByHandle(System.IO.FileStream)"/>.
	/// </summary>
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class ByHandleFileInfo
	{
		private FileAttributes dwFileAttributes;
		private FileTime ftCreationTime;
		private FileTime ftLastAccessTime;
		private FileTime ftLastWriteTime;
		private uint dwVolumeSerialNumber;
		private uint nFileSizeHigh;
		private uint nFileSizeLow;
		private uint nNumberOfLinks;
		private uint nFileIndexHigh;
		private uint nFileIndexLow;

		internal ByHandleFileInfo()
		{
		}

		/// <summary>
		/// Gets the file attributes.
		/// </summary>
		/// <value>The file attributes.</value>
		public FileAttributes Attributes
		{
			get
			{
				return dwFileAttributes;
			}
		}

		/// <summary>
		/// Gets a <see cref="DateTime"/> structure that specified when a file or directory was created.
		/// </summary>
		/// <value>A <see cref="DateTime"/> structure that specified when a file or directory was created.</value>
		public DateTime CreationTime
		{
			get
			{
				return ftCreationTime.AsDateTime();
			}
		}

		/// <summary>
		/// Gets a <see cref="DateTime"/> structure. 
		/// For a file, the structure specifies the last time that a file is read from or written to. 
		/// For a directory, the structure specifies when the directory is created. 
		/// For both files and directories, the specified date is correct, but the time of day is always set to midnight. 
		/// If the underlying file system does not support the last access time, this member is zero (0).
		/// </summary>
		/// <value>A <see cref="DateTime"/> structure that specified when a file was last written to or the directory created.</value>
		public DateTime LastAccessTime
		{
			get
			{
				return ftLastAccessTime.AsDateTime();
			}
		}

		/// <summary>
		/// Gets a <see cref="DateTime"/> structure. 
		/// For a file, the structure specifies the last time that a file is written to. 
		/// For a directory, the structure specifies when the directory is created. 
		/// If the underlying file system does not support the last access time, this member is zero (0).
		/// </summary>
		/// <value>A <see cref="DateTime"/> structure that specified when a file was last written to or the directory created.</value>
		public DateTime LastWriteTime
		{
			get
			{
				return ftLastWriteTime.AsDateTime();
			}
		}

		/// <summary>
		/// Gets the the serial number of the volume that contains a file.
		/// </summary>
		/// <value>The serial number of the volume that contains a file.</value>
		public UInt32 VolumeSerialNumber
		{
			get
			{
				return dwVolumeSerialNumber;
			}
		}

		/// <summary>
		/// Gets the size of the file.
		/// </summary>
		/// <value>The size of the file.</value>
		public long FileSize
		{
			get
			{
				return ToLong(nFileSizeHigh, nFileSizeLow);
			}
		}

		/// <summary>
		/// Gets the number of links to this file. For the FAT file system this member is always 1. For the NTFS file system, it can be more than 1.
		/// </summary>
		/// <value>The number of links to this file. </value>
		public uint NumberOfLinks
		{
			get
			{
				return nNumberOfLinks;
			}
		}

		/// <summary>
		/// Gets the unique identifier associated with the file. The identifier and the volume serial number uniquely identify a 
		/// file on a single computer. To determine whether two open handles represent the same file, combine the identifier 
		/// and the volume serial number for each file and compare them.
		/// </summary>
		/// <value>The unique identifier of the file.</value>
		public long FileIndex
		{
			get 
			{
				return ToLong(nFileIndexHigh, nFileIndexLow);
			}
		}

		private static long ToLong(uint dwHigh, uint dwLow)
		{
			return ((((long)dwHigh) << 32) | (((long)dwLow) & 0xFFFFFFFF));
		}


	};

}
