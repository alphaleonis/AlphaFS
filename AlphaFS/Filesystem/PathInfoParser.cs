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
using System.Globalization;
using System.Text;

namespace Alphaleonis.Win32.Filesystem
{
    public partial class PathInfo
    {
        // This parser is used by PathInfo to create the internal representation of the path.
        // It could have been made simpler by using eg. regular expressions, however this 
        // implementation should be at least twice as efficient, allowing for greater 
        // flexibility.
        private class Parser
        {
            #region Constructors

            static Parser()
            {
                int max = 0;
                foreach (char c in System.IO.Path.GetInvalidFileNameChars())
                {
                    if (c > max)
                        max = c;
                }

                mInvalidFileNameCharsArray = new bool[max];
                for (int i = 0; i < mInvalidFileNameCharsArray.Length; i++)
                {
                    if (Array.IndexOf(System.IO.Path.GetInvalidFileNameChars(), (char)i) != -1)
                        mInvalidFileNameCharsArray[i] = true;
                    else
                        mInvalidFileNameCharsArray[i] = false;
                }
            }

            public Parser(string path, bool allowWildcards)
            {
                if (path == null)
                    throw new ArgumentNullException("path");
				
                mPath = path;
                mAllowFileNameWildcards = allowWildcards;
                MatchRoot();

                if (!IsSeparator(LA(-1)) && LA(-1) != '\0' && LA(0) != '\0')
                    throw new ArgumentException("Invalid Path");

                mIndices.Add(mBuilder.Length);
                MatchDirs();
            }

            #endregion

            #region Matching Functions

            private void MatchDirs()
            {
                mExtensionPos = -1;
                do
                {
                    bool hasWildcard = false;
                    bool hasInvalidFileNameChars = false;

                    // Read everything up to a separator
                    char c;
                    while (!IsSeparator((c = LA(0))) && c != '\0')
                    {
                        if (mIsNotInternalDirectory)
                        {
                            hasWildcard = hasWildcard || IsWildcard(c);
                            hasInvalidFileNameChars = hasInvalidFileNameChars || (!IsWildcard(c) && !IsValidFileNameChar(c));
                        }

                        if (c == '.')
                        {
                            mExtensionPos = mCurPos;
                        }

                        mBuilder.Append(c);
                        ++mCurPos;
                    }

                    bool add = true;

                    // An ending separator has not yet been added under any circumstance.
                    if (mIsNotInternalDirectory)
                    {
                        if (NewComponentMatches(".."))
                        {
                            // Remove references to the parent directory if possible
                            add = !ResolveParentReference();
                        }
                        else 
                            if (NewComponentMatches("."))
                        {
                            // Skip references to the current directory (".")
                            add = false;
                            mBuilder.Length = mBuilder.Length - 1;
                        }
                    }

                    if (IsSeparator(LA(0)))
                    {
                        if (add)
                        {
                            // Wildcards not allowed in directory components
                            if (mIsNotInternalDirectory && (hasInvalidFileNameChars || hasWildcard))
                                throw new ArgumentException("Invalid characters in path");

                            mBuilder.Append(Alphaleonis.Win32.Filesystem.Path.DirectorySeparatorChar);
                            mIndices.Add(mBuilder.Length);
                        }
                        Next();
                        // Don't store extension for directories
                        mExtensionPos = -1;
                    }
                    else
                    {
                        if (mIsNotInternalDirectory && hasInvalidFileNameChars)
                            throw new ArgumentException("Invalid characters in file name");

                        if (mIsNotInternalDirectory && !mAllowFileNameWildcards && hasWildcard)
                            throw new ArgumentException("Wildcards are not allowed");

                        break;
                    }
                }
                while (true);
                MatchFile();
            }

            private void MatchFile()
            {
				if (mExtensionPos == -1) // No extension was found
				{
					mExtensionPos = mBuilder.Length;
				}
				else if (mExtensionPos == mBuilder.Length - 1) 
				{
					// An empty extension was found, so we remove the
					// trailing dot.
					mBuilder.Length = mBuilder.Length - 1;
					mExtensionPos = mBuilder.Length;
				}
                // File name should be in 'file'

            }

            private void MatchRoot()
            {
                if (IsSeparator(LA(0)))
                {
                    if (IsSeparator(LA(1)) && LA(2) != '?') // We have "\\X" where X is not a questionmark, so must be UNC path.
                    {
                        MatchUnc();
                    }
                    else if (LaMatchesExact(Alphaleonis.Win32.Filesystem.Path.LongPathPrefix)) // We have "\\?\"
                    {
                        MatchPrefixedLongPath();
                        mIsNotInternalDirectory = false;
                    }
                    else
                    {
                        MatchUnqualifiedRoot(); // We have "\XXXX", i.e. an absolute path without a drive specification.
                    }
                }
                else if (IsDriveLetter(LA(0)) && LA(1) == ':') // We have "X:" for some valid drive letter X.
                {
                    MatchDrive();
                }
            }

            private void MatchUnqualifiedRoot()
            {
                Debug.Assert(IsSeparator(LA(0)) && !IsSeparator(LA(1)));
                mBuilder.Append(Alphaleonis.Win32.Filesystem.Path.DirectorySeparatorChar);
                Next();
            }

            private void MatchPrefixedLongPath()
            {
                Debug.Assert(LaMatchesExact(Alphaleonis.Win32.Filesystem.Path.LongPathPrefix));
                mBuilder.Append(Alphaleonis.Win32.Filesystem.Path.LongPathPrefix);
                Skip(4);

                // Check for \\?\c:\path\file.ext type paths
				if (IsDriveLetter(LA(0)) && LA(1) == System.IO.Path.VolumeSeparatorChar)
				{
					mBuilder.Append(LA(0));
					mBuilder.Append(Alphaleonis.Win32.Filesystem.Path.VolumeSeparatorChar);
					Skip(2);
					if (LA(0) == System.IO.Path.DirectorySeparatorChar)
					{
						mBuilder.Append(Alphaleonis.Win32.Filesystem.Path.DirectorySeparatorChar);
						Skip(1);
					}
				}
                if (IsDriveLetter(LA(0)) && LA(1) == System.IO.Path.VolumeSeparatorChar && LA(2) == System.IO.Path.DirectorySeparatorChar)
                {
                    mBuilder.Append(LA(0));
                    mBuilder.Append(Alphaleonis.Win32.Filesystem.Path.VolumeSeparatorChar + Alphaleonis.Win32.Filesystem.Path.DirectorySeparatorChar);
                    Skip(3);
                }
				else if (LaMatchesVolume()) // Check for \\?\volume{D0E234D3-87C4-4b27-8B88-DA418BFDD0C7}\... type paths
                { 
                    mBuilder.Append(mPath.Substring(mCurPos, 44));
                    Skip(44);
                    if (LA(0) == System.IO.Path.DirectorySeparatorChar)
                    {
                        mBuilder.Append(Alphaleonis.Win32.Filesystem.Path.DirectorySeparatorChar);
                        Next();
                    }
                }
                else if (LaMatchesIgnoreCase("globalroot")) // Check for \\?\GLOBALROOT\... paths
                {
                    mBuilder.Append("GLOBALROOT");
                    Skip(10);
                    if (LA(0) == System.IO.Path.DirectorySeparatorChar)
                    {
                        mBuilder.Append(Alphaleonis.Win32.Filesystem.Path.DirectorySeparatorChar);
                        Next();
                    }
                }
				else if (LaMatchesIgnoreCase("unc")) // Check for \\?\UNC\Server\Share\... type paths.
				{
					mBuilder.Append("UNC");
					Skip(3);
					if (LA(0) == System.IO.Path.DirectorySeparatorChar)
					{
						mBuilder.Append(Alphaleonis.Win32.Filesystem.Path.DirectorySeparatorChar);
						Next();
						MatchServerNameAndShare();
					}
					
				}
            }

			private void MatchServerNameAndShare()
			{
				Debug.Assert(IsDirNameChar(LA(0)));

				char c;
				while ( !IsSeparator(c = LA(0)) && c != '\0')
				{
					mBuilder.Append(c);
					Next();
				}

				if (c == '\0')
				{
					throw new ArgumentException(@"UNC path should match the form \\Server\Share", "path");
				}

				Skip(1);
				mBuilder.Append(System.IO.Path.DirectorySeparatorChar);

				while (!IsSeparator(c = LA(0)) && c != '\0')
				{
					mBuilder.Append(c);
					Next();
				}

				if (c != '\0')
				{
					Skip(1);
					mBuilder.Append(System.IO.Path.DirectorySeparatorChar);
				}
				else
				{
					// No directory or file specified, only server+share. Always append trailing slash to this.
					mBuilder.Append(System.IO.Path.DirectorySeparatorChar);
				}
			}

			private void MatchShortPathUncPrefix()
			{
				Debug.Assert(IsSeparator(LA(0)) && IsSeparator(LA(1)));
				Skip(2);
				mBuilder.Append(Alphaleonis.Win32.Filesystem.Path.DirectorySeparatorChar);
				mBuilder.Append(Alphaleonis.Win32.Filesystem.Path.DirectorySeparatorChar);
			}

            private void MatchUnc()
            {
				MatchShortPathUncPrefix();

				MatchServerNameAndShare();
            }

            private void MatchDrive()
            {
                Debug.Assert(IsDriveLetter(LA(0)) && LA(1) == ':');
                mBuilder.Append(LA(0));
                mBuilder.Append(':');
                Skip(2);
                if (IsSeparator(LA(0)))
                {
                    mBuilder.Append(Alphaleonis.Win32.Filesystem.Path.DirectorySeparatorChar);
                    Next();
                }
            }

            #endregion

            #region Private Utility Methods

            private bool LaMatchesVolume()
            {
				// Matches eg. volume{D0E234D3-87C4-4b27-8B88-DA418BFDD0C7}
                if (!LaMatchesIgnoreCase("volume{"))
                    return false;

                int i = "volume{".Length;

                if (!LaMatchesHexDigitSequence(8, i))
                    return false;
                i += 8;

                if (LA(i) != '-')
                    return false;

                if (!LaMatchesHexDigitSequence(4, ++i))
                    return false;
                i += 4;

                if (LA(i) != '-')
                    return false;

                if (!LaMatchesHexDigitSequence(4, ++i))
                    return false;
                i += 4;

                if (LA(i) != '-')
                    return false;

                if (!LaMatchesHexDigitSequence(4, ++i))
                    return false;
                i += 4;

                if (LA(i) != '-')
                    return false;

                if (!LaMatchesHexDigitSequence(12, ++i))
                    return false;
                i += 12;

                if (LA(i++) != '}')
                    return false;

                if (LA(i) != System.IO.Path.DirectorySeparatorChar && LA(i) != '\0')
                    return false;
                return true;
            }

            private bool LaMatchesHexDigitSequence(int length, int index)
            {
                for (int i = index; i < length + index; i++)
                {
                    if ((LA(i) < '0' || LA(i) > '9') && (LA(i) < 'a' || LA(i) > 'f') && (LA(i) < 'A' || LA(i) > 'F'))
                        return false;
                }
                return true;
            }

            private bool LaMatchesIgnoreCase(string s)
            {
                for (int i = 0; i < s.Length; i++)
                    if (Char.ToLower(LA(i), CultureInfo.InvariantCulture) != Char.ToLower(s[i], CultureInfo.InvariantCulture))
                        return false;
                return true;
            }

            private bool LaMatchesExact(string s)
            {
                for (int i = 0; i < s.Length; i++)
                    if (LA(i) != s[i])
                        return false;
                return true;
            }

            bool IsDirNameChar(char c)
            {
                return !IsSeparator(c) && !IsWildcard(c) && c != '\0';
            }

            void Next()
            {
                ++mCurPos;
            }

            void Skip(int num)
            {
                mCurPos += num;
            }

            bool IsSeparator(char c)
            {
                return (c == System.IO.Path.DirectorySeparatorChar) || (mIsNotInternalDirectory && c == System.IO.Path.AltDirectorySeparatorChar);
            }

            char LA(int index)
            {
                int realPos = mCurPos + index;
                return (realPos < 0 || realPos >= mPath.Length) ? '\0' : mPath[realPos];
            }

            private static bool IsWildcard(char p)
            {
                return p == '*' || p == '?';
            }

            private static bool IsDriveLetter(char c)
            {
                return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z');
            }

            private static bool IsValidFileNameChar(char c)
            {
                int i = (int)c;
                return i >= mInvalidFileNameCharsArray.Length || !mInvalidFileNameCharsArray[i];
            }

            /// <summary>
            /// Removes a reference to the parent directory ("..") if possible.
            /// This must be called *before* the reference to the parent directory has been
            /// added.
            /// </summary>
            /// <returns><c>true</c> if the reference was removed, and <c>false</c> if it was kept.</returns>
            private bool ResolveParentReference()
            {
                Debug.Assert(NewComponentMatches(".."));

                // Is the new reference the first directory component?
                if (mIndices.Count < 2)
                {
                    // Do we have a root?
                    if (HasRoot)
                    {
                        // If so, we simply remove it, since the parent of the root
                        // is the root itself.
                        mBuilder.Length = mIndices[mIndices.Count - 1];
                        return true;
                    }
                    else
                    {
                        // Otherwise we cannot remove the parent reference,
                        // so we keep it.
                        return false;
                    }
                }
                else
                {
                    // The new reference is not the first directory component.
                    // We need to check the previous directory component.
                    int start = mIndices[mIndices.Count - 2];
                    int end = mIndices[mIndices.Count - 1] - 1;

                    // Is the previous component also a parent reference?
                    if (end - start == 2 && mBuilder[start] == '.' && mBuilder[start + 1] == '.')
                    {
                        // If so, we need to keep it.
                        return false;
                    }
                    else
                    {
                        // otherwise, we remove the new reference, *and* the parent 
                        // reference.
                        mBuilder.Length = start;
                        mIndices.RemoveAt(mIndices.Count - 1);
                        return true;
                    }
                }
            }

            private bool NewComponentMatches(string s)
            {
                // If there is no last component, we cannot compare it.
                if (mIndices.Count < 1)
                    return false;

                if (s.Length != mBuilder.Length - mIndices[mIndices.Count - 1])
                    return false;

                int c = 0;
                for (int i = mIndices[mIndices.Count - 1]; i < mBuilder.Length; i++)
                    if (mBuilder[i] != s[c++])
                        return false;
                return true;
            }

            #endregion

            #region Public Properties

            public int ExtensionIndex
            {
                get
                {
                    return mExtensionPos;
                }
            }

            public string Path
            {
                get
                {
                    return mBuilder.ToString();
                }
            }

            public List<int> ComponentIndices
            {
                get
                {
                    return mIndices;
                }
            }

            public string Root
            {
                get
                {
                    return mBuilder.ToString().Substring(0, mIndices[0]);
                }
            }

            public bool HasRoot
            {
                get
                {
                    return mIndices[0] != 0;
                }
            }

            #endregion

            #region Private Fields

            bool mIsNotInternalDirectory = true;
            private List<int> mIndices = new List<int>();
            private StringBuilder mBuilder = new StringBuilder();
            private int mExtensionPos;
            private int mCurPos;
            private string mPath;
            private bool mAllowFileNameWildcards = true;
            private static bool[] mInvalidFileNameCharsArray;

            #endregion
        }

    }
}
