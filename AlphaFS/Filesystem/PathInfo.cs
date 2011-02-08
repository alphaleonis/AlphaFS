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
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

// Also see PathInfoParser.cs and PathInfoComponentList.cs

namespace Alphaleonis.Win32.Filesystem
{
    /// <summary>
    /// A representation of a path, providing convenient access to the individual components 
    /// of the path.
    /// </summary>
    /// <remarks>Note that no methods in this class verifies whether the path actually exists or not.</remarks>
    public partial class PathInfo : IEquatable<PathInfo>, IComparable<PathInfo>
    {
        #region Constructors

        /// <overloads>
        /// <summary>
        /// Initializes a new instance of the <see cref="PathInfo"/> class.
        /// </summary>
        /// </overloads>
        /// <param name="path">The path.</param>
        /// <exception cref="ArgumentNullException"><paramref name="path"/> is <see langword="null"/></exception>
        /// <exception cref="IllegalPathException">The path is not a legal path in the Win32 file system.</exception>
        public PathInfo(string path)
            : this(path, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PathInfo"/> class specifying whether wildcards
        /// should be accepted or not.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="allowWildcardsInFileName">if set to <c>true</c> wildcards are allowed in the file 
        /// name part of the path. If set to <c>false</c>, wildcards are not allowed and an
        /// <see cref="ArgumentException" /> will be thrown if they are present.</param>
        /// <exception cref="ArgumentNullException"><paramref name="path"/> is <see langword="null"/></exception>
        /// <remarks>
        ///     <para>Note that under no circumstances will this class accept wildcards in 
        ///           the directory part of the path, only in the file-name, i.e. the component
        ///           after the last backslash or separator. 
        ///     </para>
        ///     <para>
        ///         Extended length unicode paths (also referred to as long paths) (those starting with \\?\) will <b>not</b> be 
        ///         parsed for wildcards etc., regardless of the setting of this parameter.
        ///         In such a path any character is valid and backslashes alone are considered
        ///         to be separators.
        ///     </para>
        /// </remarks>
        public PathInfo(string path, bool allowWildcardsInFileName)
        {
            Parser parser = new Parser(path, allowWildcardsInFileName);
            mPath = parser.Path;
            mIndices = parser.ComponentIndices;
            mExtensionIndex = parser.ExtensionIndex;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PathInfo"/> class.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="indices">The indices.</param>
        /// <param name="extensionIndex">Position of the beginning of the file extension in the path.</param>
        private PathInfo(string path, List<int> indices, int extensionIndex)
        {
            mPath = path;
            mIndices = indices;
            mExtensionIndex = extensionIndex;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Retrieves the parent directory of the specified path, including both absolute and relative paths.
        /// </summary>
        /// <returns>The parent directory, or <see langword="null"/> if path is the root directory, including the root of a UNC server or share name.</returns>
        public PathInfo Parent
        {
            get
            {
                Debug.Assert(mIndices.Count > 0);

                // The parent of just the root, is the root.
                if (mIndices.Count == 1)
                {
                    if (mIndices[0] == 0)
                    {
                        // No root, result will be empty string.
                        return new PathInfo("");
                    }
                    else if (mIndices[mIndices.Count - 1] == mPath.Length)
                    {
                        return this;
                    }
                    else
                    {
                        return new PathInfo(mPath.Substring(0, mIndices[mIndices.Count - 1]), mIndices, -1);
                    }
                }
                else if (HasFileName)
                {
                    return new PathInfo(mPath.Substring(0, mIndices[mIndices.Count - 1]), mIndices, -1);
                }
                else
                {
                    return new PathInfo(mPath.Substring(0, mIndices[mIndices.Count - 2]), mIndices.GetRange(0, mIndices.Count - 1), -1);
                }
            }
        }


		/// <summary>
		/// Gets the full normalized path, with a trailing backslash if the path denotes a directory.
		/// </summary>
		/// <value>The full normalized path, with a trailing backslash if the path denotes a directory.</value>
		/// <seealso cref="Path"/>
		public string SuffixedPath
		{
			get
			{
				return mPath;
			}
		}

        /// <summary>
        /// Gets the full normalized path.
        /// </summary>
        /// <value>The full path.</value>
		/// <seealso cref="SuffixedPath"/>
        public string Path
        {
            get
            {
				if (HasFileName || mIndices.Count == 1)
				{
					return mPath;
				}
				else
				{
					// No filename, so we may have to remove a trailing slash
					return mPath.Substring(0, mPath.Length - 1);
				}
            }
        }

        /// <summary>
        /// Gets the file name part of the path.
        /// </summary>
        /// <value>The file name part of the path, or an empty string if the path does not contain a file name.</value>
        public string FileName
        {
            get
            {
                return mPath.Substring(mIndices[mIndices.Count - 1]);
            }
        }

        /// <summary>
        /// Gets the root of the path.
        /// </summary>
        /// <value>The root of the path, which may be a drive (eg. "C:\"), a remote computer as part of 
        /// an UNC path (eg. "\\OtherComputer\"), a unique volume name 
        /// (eg. "\\?\Volume{c00fa7c5-63eb-11dd-b6ed-806e6f6e6963}\") or a single directory
        /// separator ("\") if no drive or volume is present in the path. If does not contain 
        /// any root, an empty string is returned.</value>
        public string Root
        {
            get
            {
                return mPath.Substring(0, mIndices[0]);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the path is rooted.
        /// </summary>
        /// <value><c>true</c> if this instance is rooted; otherwise, <c>false</c>.</value>
        public bool IsRooted
        {
            get
            {
                return mIndices[0] != 0;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has file name.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has file name; otherwise, <c>false</c>.
        /// </value>
        public bool HasFileName
        {
            get
            {
                return mIndices[mIndices.Count - 1] != mPath.Length;
            }
        }

        /// <summary>
        /// Gets the extension of the file name of this path.
        /// </summary>
        /// <value>The extension of the file name of this path, or an empty string if the path does
        /// not contain a file name or the file name does not have an extension.</value>
        public string Extension
        {
            get
            {
				return HasExtension ? Path.Substring(mExtensionIndex) : String.Empty;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the file name in this path has an extension.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if the file name in this path has an extension; otherwise, <c>false</c>.
        /// </value>
        public bool HasExtension
        {
            get
            {
                return mExtensionIndex < mPath.Length - 1;
            }
        }

        /// <summary>
        /// Gets the file name without extension.
        /// </summary>
        /// <value>The file name without extension or an empty string if the 
        /// path does not contain a file name.</value>
        public string FileNameWithoutExtension
        {
            get
            {
                return mPath.Substring(mIndices[mIndices.Count - 1], mExtensionIndex - mIndices[mIndices.Count - 1]);
            }
        }

		/// <summary>
		/// Returns the directory information for the path with a trailing directory separator.
		/// </summary>
		/// <value>The name of the suffixed directory with a trailing directory separator.</value>
		/// <seealso cref="DirectoryName"/>
		public string SuffixedDirectoryName
		{
			get
			{
				return mPath.Substring(0, mIndices[mIndices.Count - 1]);
			}
		}

		/// <summary>
		/// Returns the directory information for the path without the root information, and with a trailing backslash.
		/// </summary>
		/// <value>The path without the root and file name part (if any) and with a trailing backslash.</value>
		/// <seealso cref="DirectoryNameWithoutRoot"/>
		/// <seealso cref="DirectoryName"/>
		/// <seealso cref="SuffixedDirectoryName"/>
		public string SuffixedDirectoryNameWithoutRoot
		{
			get
			{
				return mPath.Substring(mIndices[0], mIndices[mIndices.Count - 1] - mIndices[0]);
			}
		}

        /// <summary>
        /// Returns the directory information for the path.
        /// </summary>
        /// <value>The path without the file name part (if any).</value>
		/// <seealso cref="SuffixedDirectoryName"/>
        public string DirectoryName
        {
            get
            {
				if (mIndices.Count == 1)
				{
					if (mIndices[0] != 0) // We have a root only
					{
						string root = Root;
						if (root[root.Length - 1] == System.IO.Path.DirectorySeparatorChar)
							return root;
						else
							return String.Empty;
					}
					else
					{
						return String.Empty;
					}
				}
				else
				{
					return mPath.Substring(0, mIndices[mIndices.Count - 1] - 1);
				}
            }
        }

		/// <summary>
		/// Returns the directory information for the path with the root stripped off.
		/// </summary>
		/// <value>The path without the root and file name part (if any).</value>
		public string DirectoryNameWithoutRoot
		{
			get
			{
				int length = mIndices[mIndices.Count - 1] - mIndices[0] - 1;

				return length > 0 ? mPath.Substring(mIndices[0], length) : String.Empty;
			}
		}

        /// <summary>
        /// Gets a list exposing the individual components of the directory part of this path.
        /// </summary>
        /// <value>The directory components of this path.</value>
        public IList<string> DirectoryComponents
        {
            get
            {
                return new ComponentList(this);
            }
        }

        #endregion

        #region Public Methods

		/// <summary>
		///		Retrieves the full long (or extended) unicode version of the path represented by this <see cref="PathInfo"/> instance.
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
		///		If the path represented by this instance is not an absolute path, or is not rooted, the path of the
		///		current directory (and drive) is combined with this path to form
		///		an absolute path.
		/// </para>
		/// </remarks>
		/// <returns>The long or extended unicode version of the specified path.</returns>
		/// <seealso cref="GetFullPath"/>
		/// <seealso cref="Alphaleonis.Win32.Filesystem.Path.GetLongPath"/>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
		public string GetLongPath()
		{
			if (mPath.StartsWith(Alphaleonis.Win32.Filesystem.Path.LongPathPrefix, StringComparison.Ordinal))
			{
				return mPath;
			}
			else if (mPath.StartsWith(Alphaleonis.Win32.Filesystem.Path.UncPrefix, StringComparison.Ordinal))
			{
				return Alphaleonis.Win32.Filesystem.Path.LongPathUncPrefix + mPath.Substring(2);
			}
			else
			{
				return Alphaleonis.Win32.Filesystem.Path.LongPathPrefix + GetFullPath();
			}
		}

        /// <summary>
        /// Gets the full absolute path of the path represented by this instance.
        /// This is done by "applying" the path to the current directory if the path
        /// does not contain a root, or the volume of the current directory if the
        /// path does not contain any drive information.
        /// </summary>
        /// <returns>The full absolute path.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public string GetFullPath()
        {
            if (IsRooted && !Root.Equals(@"\"))
            {
                return mPath;
            }
            else
            {
                if (IsRooted)
                {
                    // A rooted directory without a drive/volume/share root
                    Parser p = new Parser(Directory.GetCurrentDirectory(), true);
                    return p.Root + Path.Substring(1);
                }
                else
                {
                    PathInfo current = new PathInfo(Directory.GetCurrentDirectory());
                    return Combine(current, this).Path;
                }
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents the current <see cref="System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents the current <see cref="System.Object"/>.
        /// </returns>
        public override string  ToString()
        {
             return Path;
        }

        /// <summary>
        /// Performs a lexiographical comparison of the string representations of this and 
        /// the other path, ignoring case.
        /// </summary>
        /// <param name="other">A <see cref="PathInfo"/> to compare with this object.</param>
        /// <returns>
        /// A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the <paramref name="other"/> parameter.Zero This object is equal to <paramref name="other"/>. Greater than zero This object is greater than <paramref name="other"/>.
        /// </returns>
        public int CompareTo(PathInfo other)
        {
            if (other == null)
                throw new ArgumentNullException("other");

            return String.Compare(Path, other.Path, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Performs a lexiographical comparison for equality of the string representations of this and 
        /// the other path, ignoring case.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        public bool Equals(PathInfo other)
        {
            if (other == null)
                return false;

            return String.Equals(Path, other.Path, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Performs a lexiographical comparison for equality of the string representations of this and 
        /// the other path, ignoring case.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with the current <see cref="System.Object"/>.</param>
        /// <returns>
        /// true if the specified <see cref="System.Object"/> is equal to the current <see cref="System.Object"/>; otherwise, false.
        /// </returns>
        /// <exception cref="System.NullReferenceException">The <paramref name="obj"/> parameter is null.</exception>
        public override bool Equals(object obj)
        {
            PathInfo other = obj as PathInfo;
            if (other == null)
                return false;
            return this.Equals(other);
        }

        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="System.Object"/>.
        /// </returns>
        public override int GetHashCode()
        {
            return Path.GetHashCode();
        }

        #endregion

        #region Public Static Methods

        /// <summary>
        /// Combines two paths.
        /// </summary>
        /// <param name="path1">The first path. </param>
        /// <param name="path2">The second path.</param>
        /// <returns>A string containing the combined paths. If one of the specified paths is a zero-length string, this method returns the other path. If path2 contains an absolute path, this method returns path2.</returns>
        public static PathInfo Combine(PathInfo path1, PathInfo path2)
        {
            if (path1 == null)
                throw new ArgumentNullException("path1");

            if (path2 == null)
                throw new ArgumentNullException("path2");

            if (path2.IsRooted || path1.Path.Length == 0)
            {
                return path2;
            }
            else
            {
                // TODO: This method could be made more efficient
                StringBuilder sb = new StringBuilder(path1.Path.Length + path2.Path.Length + 1);
                sb.Append(path1);
                char lastChar = sb[sb.Length - 1];
                if (lastChar != '\\' && lastChar != '/')
                    sb.Append('\\');
                sb.Append(path2);
                return new PathInfo(sb.ToString(), true);
            }
        }

        /// <summary>
        /// Combines two paths.
        /// </summary>
        /// <param name="path1">The first path. </param>
        /// <param name="path2">The second path.</param>
        /// <returns>A string containing the combined paths. If one of the specified paths is a zero-length string, this method returns the other path. If path2 contains an absolute path, this method returns path2.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates")]
        public static PathInfo operator +(PathInfo path1, PathInfo path2)
        {
            return Combine(path1, path2);
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="path1">The path1.</param>
        /// <param name="path2">The path2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(PathInfo path1, PathInfo path2)
        {
            if (object.ReferenceEquals(path1, null))
                return object.ReferenceEquals(path2, null) ? true : false;

            return path1.Equals(path2);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="path1">The path1.</param>
        /// <param name="path2">The path2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(PathInfo path1, PathInfo path2)
        {
            if (object.ReferenceEquals(path1, null))
                return object.ReferenceEquals(path2, null) ? false : true;

            return !path1.Equals(path2);
        }

        /// <summary>
        /// Implements the operator &lt;.
        /// </summary>
        /// <param name="path1">The path1.</param>
        /// <param name="path2">The path2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator <(PathInfo path1, PathInfo path2)
        {
            return path1.CompareTo(path2) < 0;
        }

        /// <summary>
        /// Implements the operator &gt;.
        /// </summary>
        /// <param name="path1">The path1.</param>
        /// <param name="path2">The path2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator >(PathInfo path1, PathInfo path2)
        {
            return path1.CompareTo(path2) > 0;
        }


        #endregion

        #region Private Fields

        string mPath;
        List<int> mIndices;
        int mExtensionIndex;
        

        #endregion
    }

}
