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
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace Alphaleonis.Win32.Filesystem
{
	/// <summary>
	/// Performs operations on String instances that contain file or directory path information. 
	/// </summary>
	public static class Path
	{
		/// <summary>
		/// Changes the extension of a path string.
		/// </summary>
		/// <param name="path">The path information to modify. The path cannot contain any of the characters defined in GetInvalidPathChars.</param>
		/// <param name="extension">The new extension (with a leading period). Specify <see langword="null"/> to remove an existing extension from path.</param>
		/// <returns>The <paramref name="path"/> specified with the extension of the file name changed to the specified <paramref name="extension"/>.</returns>
		public static string ChangeExtension(string path, string extension)
		{
			if (path == null)
				throw new ArgumentNullException("path");

			PathInfo p = new PathInfo(path);
			if (!p.HasFileName)
			{
				return path;
			}
			else
			{
				if (!p.HasExtension)
				{
					return path + (extension ?? "");
				}
				else
				{
					return path.Substring(0, path.LastIndexOf('.')) + extension;
				}
			}
		}

		/// <summary>
		/// Combines two path strings.
		/// </summary>
		/// <param name="path1">The path1.</param>
		/// <param name="path2">The path2.</param>
		/// <returns>A string containing the combined paths. If one of the specified paths is a zero-length string, this method returns the other path. If <paramref ref="path2"/> contains an absolute path, this method returns <paramref ref="path2"/>.</returns>
		public static string Combine(string path1, string path2)
		{
			return System.IO.Path.Combine(path1, path2);
		}

		/// <summary>
		/// Returns the directory information for the specified <paramref name="path"/> with a trailing directory separator.
		/// </summary>
		/// <param name="path">The path.</param>
		/// <returns>The directory information for the specified <paramref name="path"/> with a trailing directory separator.</returns>
		/// <seealso cref="PathInfo.SuffixedDirectoryName"/>
		/// <seealso cref="GetDirectoryName"/>
		/// <seealso cref="GetDirectoryNameWithoutRoot"/>
		public static string GetSuffixedDirectoryName(string path)
		{
			return new PathInfo(path).SuffixedDirectoryName;
		}

		/// <summary>
		/// Returns the directory information for the specified <paramref name="path"/> without the root and with a trailing directory separator.
		/// </summary>
		/// <param name="path">The path.</param>
		/// <returns>The directory information for the specified <paramref name="path"/> without the root and with a trailing directory separator.</returns>
		/// <seealso cref="PathInfo.SuffixedDirectoryNameWithoutRoot"/>
		/// <seealso cref="GetDirectoryName"/>
		/// <seealso cref="GetDirectoryNameWithoutRoot"/>
		public static string GetSuffixedDirectoryNameWithoutRoot(string path)
		{
            if (string.IsNullOrEmpty(path)) return null;
			return new PathInfo(path).SuffixedDirectoryNameWithoutRoot;
		}

		/// <summary>
		/// Returns the directory information for the specified path string.
		/// </summary>
		/// <param name="path">The path.</param>
		/// <returns>The path without the file name part (if any).</returns>
		public static string GetDirectoryName(string path)
		{
            if (string.IsNullOrEmpty(path)) return null;
			return new PathInfo(path).DirectoryName;
		}

		/// <summary>
		/// Returns the directory information for the specified path string without the root information.
		/// </summary>
		/// <param name="path">The path.</param>
		/// <returns>The path without the file name part and without the root information (if any).</returns>
		public static string GetDirectoryNameWithoutRoot(string path)
		{
            if (string.IsNullOrEmpty(path)) return null;
			return new PathInfo(path).DirectoryNameWithoutRoot;
		}

		/// <summary>
		/// Returns the extension of the specified path string.
		/// </summary>
		/// <param name="path">The path string from which to get the extension.</param>
		/// <returns>The extension of the specified <paramref name="path"/>, or an empty string 
		/// if the path contains no extension. If the path is <see langword="null"/>, this method
		/// returns <see langword="null"/>.</returns>
		public static string GetExtension(string path)
		{
            if (string.IsNullOrEmpty(path)) return null;
			PathInfo p = new PathInfo(path);
			return p.HasExtension ? p.Extension : String.Empty;
		}

		/// <summary>
		/// Returns the file name and extension of the specified path string.
		/// </summary>
		/// <param name="path">The path string from which to obtain the file name and extension.</param>
		/// <returns>A String consisting of the characters after the last directory character in path. 
		/// If the last character of path is a directory or volume separator character, 
		/// this method returns Empty. If path is a null reference, this method returns 
		/// <see langword="null"/>.</returns>
		public static string GetFileName(string path)
		{
            if (string.IsNullOrEmpty(path)) return null;
			PathInfo p = new PathInfo(path);
			return p.HasFileName ? p.FileName : String.Empty;
		}

		/// <summary>
		/// Returns the file name without extension of the specified path string.
		/// </summary>
		/// <param name="path">The path string from which to obtain the file name.</param>
		/// <returns>A String consisting of the characters after the last directory character in path
		/// up to the extension. 
		/// If the last character of path is a directory or volume separator character, 
		/// this method returns an empty string. If path is a null reference, this method returns 
		/// <see langword="null"/>.</returns>
		public static string GetFileNameWithoutExtension(string path)
		{
            if (string.IsNullOrEmpty(path)) return null;
			PathInfo p = new PathInfo(path);
			return p.HasFileName ? p.FileNameWithoutExtension : String.Empty;
		}

		/// <summary>
		/// Returns the absolute path for the specified path string.
		/// </summary>
		/// <param name="path">The file or directory for which to obtain absolute path information.</param>
		/// <returns>A string containing the fully qualified location of path, such as "C:\MyFile.txt".</returns>
		public static string GetFullPath(string path)
		{
			PathInfo p = new PathInfo(path);
			return p.GetFullPath();
		}

        /// <summary>
        /// Retrieves the short 8.3 type path form of the specified path.
        /// </summary>
        /// <param name="path">The existing path. Can be regualr and long paths. Otherwise throws an error.</param>
        /// <returns>A path that has 8.3 type names.</returns>
        /// <remarks>Hasn't been tested on NTFS volumes with disabled 8.3 name generation. Suspect some weirdness.</remarks>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static string GetShort83Path(string path)
        {
            System.Text.StringBuilder buffer = new System.Text.StringBuilder(path.Length);
            int actualLength = NativeMethods.GetShortPathNameW(path, buffer, buffer.Capacity);
            while(actualLength > buffer.Capacity)
                {
                    buffer = new System.Text.StringBuilder(actualLength);
                    actualLength = NativeMethods.GetShortPathNameW(path, buffer, buffer.Capacity);
                }
            if (actualLength <= 0)
                NativeError.ThrowException(path, path);

            return buffer.ToString();
        }

        /// <summary>
        /// Converts the specified existing path to its regular long form.
        /// </summary>
        /// <param name="path">The existing path.</param>
        /// <returns>The full path </returns>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static string GetLongFrom83ShortPath(string path)
        {
            System.Text.StringBuilder buffer = new System.Text.StringBuilder(path.Length);
            int actualLength = NativeMethods.GetShortPathNameW(path, buffer, buffer.Capacity);
            while (actualLength > buffer.Capacity)
            {
                buffer = new System.Text.StringBuilder(actualLength);
                actualLength = NativeMethods.GetLongPathNameW(path, buffer, buffer.Capacity);
            }
            if (actualLength <= 0)
                NativeError.ThrowException(path, path);

            return buffer.ToString();
        }

		/// <summary>
		/// Gets an array containing the characters that are not allowed in file names.
		/// </summary>
		/// <returns>An array containing the characters that are not allowed in file names.</returns>
		/// <remarks>See also <see cref="System.IO.Path.GetInvalidFileNameChars"/></remarks>
		public static char[] GetInvalidFileNameChars()
		{
			return System.IO.Path.GetInvalidFileNameChars();
		}

		/// <summary>
		/// Gets an array containing the characters that are not allowed in path names.
		/// </summary>
		/// <returns>An array containing the characters that are not allowed in path names.</returns>
		/// <remarks>See also <see cref="System.IO.Path.GetInvalidPathChars"/></remarks>
		public static char[] GetInvalidPathChars()
		{
			return System.IO.Path.GetInvalidPathChars();
		}

		/// <summary>
		/// Gets the path root.
		/// </summary>
		/// <param name="path">The path.</param>
		/// <returns>A string containing the root directory of path, such as "C:\", or <see langword="null"/> 
		/// if <paramref name="path"/> is <see langword="null"/>, or an empty string if path does not 
		/// contain root directory information.</returns>
		public static string GetPathRoot(string path)
		{
			if (path == null)
				return null;

			PathInfo p = new PathInfo(path);
			return p.IsRooted ? p.Root : String.Empty;
		}

		/// <summary>
		/// Returns a random folder name or file name.
		/// </summary>
		/// <returns>A random folder name or file name.</returns>
		/// <remarks>This is equivalent to <see cref="System.IO.Path.GetRandomFileName"/>.</remarks>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
		public static string GetRandomFileName()
		{
			return System.IO.Path.GetRandomFileName();
		}

		/// <summary>
		/// Creates a uniquely named, zero-byte temporary file on disk and returns the full path of that file.
		/// </summary>
		/// <returns>A <see cref="String"/> containing the full path of the temporary file.</returns>
		/// <remarks>This is equivalent to <see cref="System.IO.Path.GetTempFileName"/></remarks>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
		public static string GetTempFileName()
		{
			return System.IO.Path.GetTempFileName();
		}

		/// <summary>
		/// Returns the path of the current system's temporary folder.
		/// </summary>
		/// <returns>A <see cref="String"/> containing the path information of a temporary directory.</returns>
		/// <remarks>This is equivalent to <see cref="System.IO.Path.GetTempPath"/></remarks>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
		public static string GetTempPath()
		{
			return System.IO.Path.GetTempPath();
		}

		/// <summary>
		/// Gets the connection name of the locally mapped drive.
		/// </summary>
		/// <param name="path">The local path with drive name. This method does not support long path prefixes.</param>
		/// <returns>String which has the following format <c>\\servername\sharename</c>.</returns>
		[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		public static string GetMappedConnectionName(string path)
		{
			return GetMappedInfoInternal(path).connectionName;
		}

		/// <summary>
		/// Gets the UNC name from the locally mapped path.
		/// </summary>
		/// <param name="path">The local path with drive name. </param>
		/// <returns>String in which drive name being replaced with it's UNC connection name.</returns>
		[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		public static string GetMappedUncName(string path)
		{
			return GetMappedInfoInternal(path).universalName;
		}

		/// <summary>
		/// Gets the mapped info internal.
		/// This method uses NativeMethods.RemoteNameInfo level to retieve more info :)
		/// </summary>
		/// <param name="path">The local path with drive name.</param>
		/// <returns><see cref="NativeMethods.RemoteNameInfo"/></returns>
      [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2001:AvoidCallingProblematicMethods", MessageId = "System.Runtime.InteropServices.SafeHandle.DangerousGetHandle"), SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		private static NativeMethods.RemoteNameInfo GetMappedInfoInternal(string path)
		{
			if (path == null)
				throw new ArgumentNullException("path", "The specified path cannot be NULL.");

			if (IsLongPath(path))
				throw new ArgumentException("Long paths are not supported but this method.");

			if (path.Length > NativeMethods.MAX_PATH)
				throw new System.IO.PathTooLongException();

			int bufferSize = 0;
			SafeGlobalMemoryBufferHandle safeBuffer = new SafeGlobalMemoryBufferHandle(bufferSize);

            try
            {
                uint retVal;
                // first call is to get correct buffer size to store results.
                if ((retVal = NativeMethods.WNetGetUniversalName(path, NativeMethods.REMOTE_NAME_INFO_LEVEL, safeBuffer, ref bufferSize)) != Win32Errors.ERROR_MORE_DATA)
                    NativeError.ThrowException(retVal);
                
                safeBuffer.Dispose();
                safeBuffer = null;
                safeBuffer = new SafeGlobalMemoryBufferHandle(bufferSize);

                if ((retVal = NativeMethods.WNetGetUniversalName(path, NativeMethods.REMOTE_NAME_INFO_LEVEL, safeBuffer, ref bufferSize)) != Win32Errors.NO_ERROR)
                    NativeError.ThrowException(retVal);

                return (NativeMethods.RemoteNameInfo)Marshal.PtrToStructure(safeBuffer.DangerousGetHandle(), typeof(NativeMethods.RemoteNameInfo));
            }
            finally
            {
                if (safeBuffer != null)
                    safeBuffer.Dispose();
            }
		}

		/// <summary>
		/// Determines whether a path includes a file name extension.
		/// </summary>
		/// <param name="path">The path to search for an extension. </param>
		/// <returns>
		/// 	<c>true</c> if the specified path has extension; otherwise, <c>false</c>.
		/// </returns>
		public static bool HasExtension(string path)
		{
			if (path == null)
				return false;

			return new PathInfo(path).HasExtension;
		}

		/// <summary>
		/// Gets a value indicating whether the specified path string contains absolute or relative path information.
		/// </summary>
		/// <param name="path">The path to test. </param>
		/// <returns>
		/// 	<c>true</c> if <paramref ref="path"/> contains an absolute path; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsPathRooted(string path)
		{
			if (path == null)
				return false;

			return new PathInfo(path).IsRooted;
		}

		/// <summary>
		/// Check if file or folder name has any invalid characters.
		/// </summary>
		/// <param name="name">File or folder name.</param>
		/// <returns>True or False</returns>
		public static bool IsValidName(string name)
		{
			if (name == null)
				throw new ArgumentNullException("name");

			return (name.IndexOfAny(GetInvalidFileNameChars()) >= 0) ? false : true;
		}

		/// <summary>
		/// Verifies that the specified <paramref name="path"/> is valid and does not contain any wildcards.
		/// </summary>
		/// <param name="path">The string to test if it contains a valid path.</param>
		/// <returns><see langword="true"/> if <paramref name="path"/> is a valid path, <see langword="false"/> otherwise.</returns>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults", MessageId = "Alphaleonis.Win32.Filesystem.PathInfo")]
		public static bool IsValidPath(string path)
		{
			if (path == null)
				throw new ArgumentNullException("path");

			try
			{
				new PathInfo(path);
				return true;
			}
			catch (ArgumentException)
			{
				return false;
			}
		}

		/// <summary>
		/// Verifies that the specified <paramref name="path"/> is valid and optionally may contain wildcards.
		/// </summary>
		/// <param name="path">The string to test if it contains a valid path.</param>
		/// <param name="allowWildcards">if set to <c>true</c> wildcards are allowed in the filename part of the path, otherwise the 
		/// presence of wildcards will render the path invalid.</param>
		/// <returns>
		/// 	<see langword="true"/> if <paramref name="path"/> is a valid path, <see langword="false"/> otherwise.
		/// </returns>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults", MessageId = "Alphaleonis.Win32.Filesystem.PathInfo")]
		public static bool IsValidPath(string path, bool allowWildcards)
		{
			if (path == null)
				throw new ArgumentNullException("path");

			try
			{
				new PathInfo(path, allowWildcards);
				return true;
			}
			catch (ArgumentException)
			{
				return false;
			}
		}


		/// <summary>
		/// Check if the given path is has the specific long path prefix.
		/// </summary>
		/// <param name="path">File or folder full path.</param>
		/// <returns><c>true</c> if has long path prefix, otherwise <c>false</c>.</returns>
		public static bool IsLongPath(string path)
		{
			if (path == null)
				throw new ArgumentNullException("path");

			return (path.StartsWith(LongPathPrefix, StringComparison.Ordinal)) ? true : false;
		}

		/// <summary>
		///		Retrieves the full long (or extended) unicode version of the specified <paramref name="path"/>.
		/// </summary>
		/// <remarks>
		/// <para>
		///		This method takes care of different path conversions to be usable in Unicode 
		///		variants of the Win32 funcitons (which are internally used throughout AlphaFS).
		/// </para>
		/// <para>
		///		Regular paths are changed like the following:
		///		<list type="table">
		///			<item>
		///				<term><c>C:\Somewhere\Something.txt</c></term>
		///				<description><c>\\?\C:\Somewhere\Something.txt</c></description>
		///			</item>
		///			<item>
		///				<term><c>\\Somewhere\Something.txt</c></term>
		///				<description><c>\\?\UNC\Somewhere\Something.txt</c></description>
		///			</item>
		///		</list>
		/// </para> 
		/// <para>
		///		Already processed paths are preserved untouched so to avoid mistakes of double prefixing.
		/// </para>
		/// <para>
		///		If the <paramref name="path"/> is not an absolute path, or is not rooted, the path of the
		///		current directory (and drive) is combined with the specified <paramref name="path"/> to form
		///		an absolute path.
		/// </para>
		/// </remarks>
		/// <param name="path">File or Folder name to sanitize and prefix with proper standard.</param>
		/// <returns>The full long (or extended) unicode version of the specified <paramref name="path"/>.</returns>
		public static string GetLongPath(string path)
		{
			return new PathInfo(path).GetLongPath();
		}

		/// <summary>
		/// Gets the regular path from long prefixed one. i.e. \\?\C:\Temp\file.txt to C:\Temp\file.txt
		/// \\?\UNC\Server\share\file.txt to \\Server\share\file.txt
		/// </summary>
		/// <param name="path">The path.</param>
		/// <returns>Regular form path string.</returns>
		/// <remarks>This method does not handle paths with volume names, eg. \\?\Volume{c00fa7c5-63eb-11dd-b6ed-806e6f6e6963}\Folder\file.txt </remarks>
		public static string GetRegularPath(string path)
		{
			if (!path.StartsWith(Path.LongPathPrefix, StringComparison.Ordinal))
			{
				return path;
			}
			else if (path.StartsWith(Path.LongPathUncPrefix, StringComparison.Ordinal))
			{
				return UncPrefix + path.Substring(LongPathUncPrefix.Length);
			}
			else
			{
				return path.Substring(Path.LongPathPrefix.Length);
			}
		}

		/// <summary>
		/// Determines whether the specified path is UNC path.
		/// Supports long path prefix.
		/// </summary>
		/// <param name="path">The path.</param>
		/// <returns>
		/// 	<c>true</c> if the specified path is UNC; otherwise, <c>false</c>.
		/// </returns>
      [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Unc")]
      public static bool IsUnc(string path)
		{
			if (path.StartsWith(@"\\", StringComparison.Ordinal))
			{
				if (path.StartsWith(LongPathUncPrefix, StringComparison.Ordinal))
					return true;
				else if (!path.StartsWith(LongPathPrefix, StringComparison.Ordinal))
					return true;
			}

			return false;
		}

		/// <summary>
		/// Provides a platform-specific alternate character used to separate directory levels in a path string that reflects a hierarchical file system organization.
		/// </summary>
		/// <value>The alt directory separator char.</value>
		/// <remarks>Equivalent to <see cref="System.IO.Path.AltDirectorySeparatorChar"/></remarks>
		public static readonly string AltDirectorySeparatorChar = System.IO.Path.AltDirectorySeparatorChar.ToString();

		/// <summary>
		/// Provides a platform-specific character used to separate directory levels in a path string that reflects a hierarchical file system organization.
		/// </summary>
		/// <value>The directory separator char.</value>
		/// <remarks>Equivalent to <see cref="System.IO.Path.DirectorySeparatorChar"/></remarks>
		public static readonly string DirectorySeparatorChar = System.IO.Path.DirectorySeparatorChar.ToString();

		/// <summary>
		/// A platform-specific separator character used to separate path strings in environment variables..
		/// </summary>
		/// <value>The path separator.</value>
		/// <remarks>Equivalent to <see cref="System.IO.Path.PathSeparator"/></remarks>
		public static readonly string PathSeparator = System.IO.Path.PathSeparator.ToString();

		/// <summary>
		/// Provides a platform-specific volume separator character.
		/// </summary>
		/// <value>The volume separator char.</value>
		/// <remarks>Equivalent to <see cref="System.IO.Path.VolumeSeparatorChar"/></remarks>
		public static readonly string VolumeSeparatorChar = System.IO.Path.VolumeSeparatorChar.ToString();

		/// <summary>
		/// Provides standard Windows UNC path prefix
		/// </summary>
		public const string UncPrefix = @"\\";

		/// <summary>
		/// Provides standard Windows long path prefix
		/// </summary>
		public const string LongPathPrefix = @"\\?\";

		/// <summary>
		/// Provides standard Windows long path UNC prefix
		/// </summary>
		public const string LongPathUncPrefix = @"\\?\UNC\";

	}
}
