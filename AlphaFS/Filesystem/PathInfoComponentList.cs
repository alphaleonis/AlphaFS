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

namespace Alphaleonis.Win32.Filesystem
{
    public partial class PathInfo
    {
        private class ComponentList : IList<string>
        {
            public ComponentList(PathInfo pathInfo)
            {
                mPathInfo = pathInfo;
            }

            #region IList<string> Members

            /// <summary>
            /// Determines the index of a specific item in the <see cref="T:System.Collections.Generic.IList`1"/>.
            /// </summary>
            /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.IList`1"/>.</param>
            /// <returns>
            /// The index of <paramref name="item"/> if found in the list; otherwise, -1.
            /// </returns>
            public int IndexOf(string item)
            {
				
                for (int i = 0; i < Count; i++)
                    if (item.Equals(this[i]))
                        return i;
                return -1;
            }

            /// <summary>
            /// Inserts an item to the <see cref="T:System.Collections.Generic.IList`1"/> at the specified index.
            /// </summary>
            /// <param name="index">The zero-based index at which <paramref name="item"/> should be inserted.</param>
            /// <param name="item">The object to insert into the <see cref="T:System.Collections.Generic.IList`1"/>.</param>
            /// <exception cref="System.ArgumentOutOfRangeException">
            /// 	<paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"/>.</exception>
            /// <exception cref="System.NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1"/> is read-only.</exception>
            public void Insert(int index, string item)
            {
                throw new NotSupportedException("Modification of path component list is not allowed.");
            }

            /// <summary>
            /// Removes the <see cref="T:System.Collections.Generic.IList`1"/> item at the specified index.
            /// </summary>
            /// <param name="index">The zero-based index of the item to remove.</param>
            /// <exception cref="System.ArgumentOutOfRangeException">
            /// 	<paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"/>.</exception>
            /// <exception cref="System.NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1"/> is read-only.</exception>
            public void RemoveAt(int index)
            {
                throw new NotSupportedException("Modification of path component list is not allowed.");
            }

            /// <summary>
            /// Gets the <see cref="System.String"/> at the specified index.
            /// </summary>
            /// <value>The component of the directory at the specified index</value>
            /// <exception cref="ArgumentOutOfRangeException">If <paramref name="index"/> is out of range.</exception>
            public string this[int index]
            {
                get
                {
                    if (index < 0 || index >= Count)
                        throw new ArgumentOutOfRangeException("index");

                    return mPathInfo.mPath.Substring(mPathInfo.mIndices[index], mPathInfo.mIndices[index + 1] - mPathInfo.mIndices[index] - 1);
                }

                set
                {
                    throw new NotSupportedException("Modification of path component list is not allowed.");
                }
            }

            #endregion

            #region ICollection<string> Members

            /// <summary>
            /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"/>.
            /// </summary>
            /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
            /// <exception cref="System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
            public void Add(string item)
            {
                throw new NotSupportedException("Modification of path component list is not allowed.");
            }

            /// <summary>
            /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
            /// </summary>
            /// <exception cref="System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only. </exception>
            public void Clear()
            {
                throw new NotSupportedException("Modification of path component list is not allowed.");
            }

            /// <summary>
            /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"/> contains a specific value.
            /// </summary>
            /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
            /// <returns>
            /// true if <paramref name="item"/> is found in the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false.
            /// </returns>
            public bool Contains(string item)
            {
                for (int i = 0; i < Count; i++)
                    if (item.Equals(this[i]))
                        return true;

                return false;
            }

            /// <summary>
            /// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"/> to an <see cref="System.Array"/>, starting at a particular <see cref="System.Array"/> index.
            /// </summary>
            /// <param name="array">The one-dimensional <see cref="System.Array"/> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1"/>. The <see cref="System.Array"/> must have zero-based indexing.</param>
            /// <param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param>
            /// <exception cref="System.ArgumentNullException">
            /// 	<paramref name="array"/> is null.</exception>
            /// <exception cref="System.ArgumentOutOfRangeException">
            /// 	<paramref name="arrayIndex"/> is less than 0.</exception>
            /// <exception cref="System.ArgumentException">
            /// 	<paramref name="array"/> is multidimensional.-or-<paramref name="arrayIndex"/> is equal to or greater than the length of <paramref name="array"/>.-or-The number of elements in the source <see cref="T:System.Collections.Generic.ICollection`1"/> is greater than the available space from <paramref name="arrayIndex"/> to the end of the destination <paramref name="array"/>.</exception>
            public void CopyTo(string[] array, int arrayIndex)
            {
                if (array.Rank != 1 || arrayIndex >= array.Length || arrayIndex + Count > array.Length)
                    throw new ArgumentException("invalid array");

                if (arrayIndex < 0)
                    throw new ArgumentOutOfRangeException("arrayIndex");

                if (array == null)
                    throw new ArgumentNullException("array");

                for (int i = 0; i < Count; i++)
                {
                    array[i + arrayIndex] = this[i];
                }
            }

            /// <summary>
            /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
            /// </summary>
            /// <value></value>
            /// <returns>The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.</returns>
            public int Count
            {
                get
                {
                    return mPathInfo.mIndices.Count - 1;
                }
            }

            /// <summary>
            /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
            /// </summary>
            /// <value></value>
            /// <returns>true</returns>
            public bool IsReadOnly
            {
                get { return true; }
            }

            /// <summary>
            /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
            /// </summary>
            /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
            /// <returns>
            /// true if <paramref name="item"/> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false. This method also returns false if <paramref name="item"/> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"/>.
            /// </returns>
            /// <exception cref="System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
            public bool Remove(string item)
            {
                throw new NotSupportedException("Modification of path component list is not allowed.");
            }

            #endregion

            #region IEnumerable<string> Members

            /// <summary>
            /// Returns an enumerator that iterates through the collection.
            /// </summary>
            /// <returns>
            /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
            /// </returns>
            public IEnumerator<string> GetEnumerator()
            {
                for (int i = 0; i < Count; i++)
                    yield return this[i];
            }

            #endregion

            #region IEnumerable Members

            /// <summary>
            /// Returns an enumerator that iterates through a collection.
            /// </summary>
            /// <returns>
            /// An <see cref="System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
            /// </returns>
            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                for (int i = 0; i < Count; i++)
                    yield return this[i];
            }

            #endregion

            private PathInfo mPathInfo;
        }
    }
}
