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
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace Alphaleonis.Win32.Filesystem
{
    /// <summary>
    /// Enumerator used to enumerate file system entries (i.e. files and directories).
    /// </summary>
    /// <remarks>The enumerator can only be used to enumerate through the items once, 
    /// and cannot be reset.</remarks>
    public sealed class FileSystemEntryEnumerator : IEnumerator<FileSystemEntryInfo>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileSystemEntryEnumerator"/> class.
        /// </summary>
        /// <param name="searchString">A search string, the path which has wildcard characters,
        ///  for example, an asterisk (*) or a question mark (?).</param>
        /// <remarks>Note that no validation is done whether or not the path specified in searchString actually exists when
        /// the enumerator is constructed. This instead occurs during the first call to <see cref="MoveNext"/>.</remarks>
        public FileSystemEntryEnumerator(string searchString)
        {
            if (searchString == null)
                throw new ArgumentNullException("searchString");

            if (searchString.Length == 0)
                throw new ArgumentException(Resources.ArgumentMustNotBeEmpty, "searchString");

            mSearchString = searchString;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileSystemEntryEnumerator"/> class.
        /// </summary>
        /// <param name="searchString">The directory or searchString, and the file name, which can include
        /// wildcard characters, for example, an asterisk (*) or a question mark (?).</param>
        /// <param name="directoriesOnly">if set to <c>true</c> enumerates only directories.</param>
        /// <remarks>Note that no validation is done whether or not the searchString actually exists when
        /// the enumerator is constructed. This instead occurs during the first call
        /// to <see cref="MoveNext"/>.</remarks>
        public FileSystemEntryEnumerator(string searchString, bool directoriesOnly)
            : this(searchString)
        {
            OnlyFolders = directoriesOnly;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileSystemEntryEnumerator"/> for
        /// enumeration as part of a transaction.
        /// </summary>
        /// <param name="transaction">The kernel transaction object.</param>
		/// <param name="searchString">The directory or searchString, and the file name, which can include
        /// wildcard characters, for example, an asterisk (*) or a question mark (?).</param>
        /// <remarks><para>Note that no validation is done whether or not the searchString actually exists when
        /// the enumerator is constructed. This instead occurs during the first call
        /// to <see cref="MoveNext"/>.</para>
        /// <para>If <paramref name="transaction"/> is <see langword="null"/>, this constructor is equivalent
        /// to <see cref="FileSystemEntryEnumerator(string)"/>, leading to non-transacted call.</para></remarks>
		public FileSystemEntryEnumerator(KernelTransaction transaction, string searchString)
			: this(searchString)
        {
            mTransaction = transaction;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileSystemEntryEnumerator"/> for
        /// enumeration as part of a transaction.
        /// </summary>
        /// <param name="transaction">The kernel transaction object.</param>
		/// <param name="searchString">The directory or searchString, and the file name, which can include
        /// wildcard characters, for example, an asterisk (*) or a question mark (?).</param>
        /// <param name="directoriesOnly">if set to <c>true</c> enumerate only directories.</param>
        /// <remarks><para>Note that no validation is done whether or not the searchString actually exists when
        /// the enumerator is constructed. This instead occurs during the first call
        /// to <see cref="MoveNext"/>.</para>
        /// 	<para>If <paramref name="transaction"/> is <see langword="null"/>, this constructor is equivalent
        /// to <see cref="FileSystemEntryEnumerator(string)"/>, leading to non-transacted call.</para></remarks>
		public FileSystemEntryEnumerator(KernelTransaction transaction, string searchString, bool directoriesOnly)
			: this(transaction, searchString)
        {
            OnlyFolders = directoriesOnly;
        }

        /// <summary>
        /// Gets the <see cref="FileSystemEntryInfo"/> representing the file system entry
        /// at the current position of the enumerator.
        /// </summary>
        /// <value>the <see cref="FileSystemEntryInfo"/> representing the file system entry
        /// at the current position of the enumerator.</value>
        /// <returns>The element in the collection at the current position of the enumerator.</returns>
        public FileSystemEntryInfo Current
        {
            [SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
            get
            {
                if (mHandle == null || mHandle.IsClosed)
                {
                    throw new InvalidOperationException();
                }
                else
                {
                    return mSysEntryInfo;
                }
            }
        }

        /// <summary>
        /// Gets the element in the collection at the current position of the enumerator.
        /// </summary>
        /// <value></value>
        /// <returns>The element in the collection at the current position of the enumerator.</returns>
        object System.Collections.IEnumerator.Current
        {
            [SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
            get
            {
                return Current;
            }
        }

        /// <summary>
        /// Advances the enumerator to the next file system entry matching the specified pattern.
        /// </summary>
        /// <returns>
        ///		<see langword="true"/> if the enumerator was successfully advanced to the next element; 
        ///		<see langword="false"/> if the enumerator has passed the end of the collection.
        /// </returns>
        /// <exception cref="System.InvalidOperationException">The collection was modified after the enumerator was created. </exception>
        [SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
        public bool MoveNext()
        {
            NativeMethods.WIN32_FIND_DATA findData = new NativeMethods.WIN32_FIND_DATA();

            if (mHandle == null)
            {
                if (mTransaction == null)
                {
                    mHandle = NativeMethods.FindFirstFileExW(mSearchString,
                        NativeMethods.FINDEX_INFO_LEVELS.FindExInfoStandard,
                         findData, mFindexSearchOptions,
                        IntPtr.Zero, NativeMethods.FINDEX_FLAGS.FIND_FIRST_EX_NONE);
                }
                else
                {
                    mHandle = NativeMethods.FindFirstFileTransactedW(mSearchString,
                        NativeMethods.FINDEX_INFO_LEVELS.FindExInfoStandard,
                        findData, mFindexSearchOptions, IntPtr.Zero,
                        NativeMethods.FINDEX_FLAGS.FIND_FIRST_EX_NONE, mTransaction.SafeHandle);
                }

                if (mHandle.IsInvalid)
                {
                    uint error = (uint)Marshal.GetLastWin32Error();
                    if (error == Win32Errors.ERROR_FILE_NOT_FOUND)
                    {
                        return false;
                    }
                    else
                    {
                        NativeError.ThrowException(error, mSearchString, mSearchString);
                    }
                }
                mSysEntryInfo = new FileSystemEntryInfo(findData);
                return Filter();
            }
            else if (mHandle.IsInvalid)
            {
                return false;
            }
            else
            {
                if (!NativeMethods.FindNextFileW(mHandle, findData))
                {
                    uint error = (uint)Marshal.GetLastWin32Error();
                    if (error == Win32Errors.ERROR_NO_MORE_FILES)
                    {
                        mHandle.Close();
                        return false;
                    }
                    else
                    {
                        NativeError.ThrowException(error);
                    }
                }
                else
                {
                    mSysEntryInfo = new FileSystemEntryInfo(findData);
                    return Filter();
                }
            }
            // Actually unreachable code, but seems to be neccessary.
            return false;
        }

        /// <summary>
        /// Filters the current and parent folders WIN32 notation ("." and "..").
        /// </summary>
        private bool Filter()
        {
            // if it . (current dir) or .. (parent) move to the next file sys entries.
            if (mSysEntryInfo.FileName.Equals(".", StringComparison.Ordinal) || mSysEntryInfo.FileName.Equals("..", StringComparison.Ordinal))
                return MoveNext();
            else
                return true;
        }

        /// <summary>
        /// This method is not supported.
        /// </summary>
        /// <exception cref="System.NotSupportedException">always.</exception>
        public void Reset()
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or 
        /// resetting unmanaged resources.
        /// </summary>
        [SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
        public void Dispose()
        {
            if (mHandle != null)
            {
                mHandle.Dispose();
                mHandle = null;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to enumerate only folders.
        /// </summary>
        /// <value><c>true</c> if only folders should be enumerated; otherwise, <c>false</c>.</value>
        public bool OnlyFolders
        {
            get
            {
                return (NativeMethods.FINDEX_SEARCH_OPS.FindExSearchLimitToDirectories == mFindexSearchOptions) ? true : false;
            }

            set
            {
                if (true == value)
                {
                    mFindexSearchOptions = NativeMethods.FINDEX_SEARCH_OPS.FindExSearchLimitToDirectories;
                }
                else
                {
                    mFindexSearchOptions = NativeMethods.FINDEX_SEARCH_OPS.FindExSearchNameMatch;
                }
            }
        }

        string mSearchString;
        NativeMethods.FINDEX_SEARCH_OPS mFindexSearchOptions = NativeMethods.FINDEX_SEARCH_OPS.FindExSearchNameMatch;
        KernelTransaction mTransaction;
        SafeFindFileHandle mHandle;
        FileSystemEntryInfo mSysEntryInfo;
    }
}
