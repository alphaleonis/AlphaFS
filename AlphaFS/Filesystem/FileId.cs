/*  Copyright (C) 2008-2016 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
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
using System.IO;
using System.Security;

namespace Alphaleonis.Win32.Filesystem
{
    #region FileId

    /// <summary>Contains information that the GetFileInformationByHandle function retrieves.</summary>
    [Serializable]    
    public struct FileId : IComparable, IComparable<FileId>, IEquatable<FileId>
    {
        #region Private Fields
        private long _volumeSerialNumber;

        private long _fileIdHighPart;

        private long _fileIdLowPart;
        #endregion

        #region Constructor

        internal FileId(NativeMethods.BY_HANDLE_FILE_INFORMATION fibh)
        {
            _volumeSerialNumber = fibh.dwVolumeSerialNumber;

            _fileIdHighPart = 0;
            _fileIdLowPart = NativeMethods.ToLong(fibh.nFileIndexHigh, fibh.nFileIndexLow);
        }

        internal FileId(NativeMethods.FILE_ID_INFO fi)
        {
            _volumeSerialNumber = fi.VolumeSerialNumber;

            //the identifier is stored in the array of fi.FileId so that
            //the lowest byte is at index 0

            ArrayToLong(fi.FileId, 0, 8, out _fileIdLowPart);
            ArrayToLong(fi.FileId, 8, 8, out _fileIdHighPart);
        }

        /// <summary>
        /// Construct the value stored in a byte array ordered in Little-Endian
        /// </summary>
        /// <param name="fileId">The array containing the bytes.</param>
        /// <param name="startIndex">The starting index.</param>
        /// <param name="count">The number of bytes to convert.</param>
        /// <param name="value">The output value.</param>        
        private static void ArrayToLong(byte[] fileId, int startIndex, int count, out long value)
        {
            value = 0;
            for (int i = 0; i < count; i++)
            {
                value |= ((long)fileId[startIndex + i]) << (8 * i);
            }
        }
        #endregion // Constructor

        #region IComparable
        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="obj">An object to compare with this instance.</param>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less than zero This instance precedes <paramref name="obj" /> in the sort order. Zero This instance occurs in the same position in the sort order as <paramref name="obj" />. Greater than zero This instance follows <paramref name="obj" /> in the sort order.
        /// </returns>        
        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;

            if (!(obj is FileId))
                throw new ArgumentException("Object must be of type FileId");

            return CompareTo((FileId)obj);
        }

        /// <summary>
        /// Compares the current object with another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the <paramref name="other" /> parameter.Zero This object is equal to <paramref name="other" />. Greater than zero This object is greater than <paramref name="other" />.
        /// </returns>        
        public int CompareTo(FileId other)
        {
            if (this._volumeSerialNumber != other._volumeSerialNumber)
                return Math.Sign(this._volumeSerialNumber - other._volumeSerialNumber);

            if (this._fileIdHighPart != other._fileIdHighPart)
                return Math.Sign(this._fileIdHighPart - other._fileIdHighPart);

            return Math.Sign(this._fileIdLowPart - other._fileIdLowPart);
        }
        #endregion

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is FileId))
                return false;

            return Equals((FileId)obj);
        }

        #region IEquatable
        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
        /// </returns>
        public bool Equals(FileId other)
        {
            return this._fileIdLowPart == other._fileIdLowPart &&
                this._fileIdHighPart == other._fileIdHighPart &&
                this._volumeSerialNumber == other._volumeSerialNumber;
        }
        #endregion

        /// <summary>
        /// Indicates whether the values of two specified <see cref="FileId" /> objects are equal.
        /// </summary>
        /// <param name="first">The first object to compare.</param>
        /// <param name="second">The second object to compare.</param>
        /// <returns>
        /// true if <paramref name="first" /> and <paramref name="second" /> are equal; otherwise, false.
        /// </returns>
        public static bool operator == (FileId first, FileId second)
        {
            return first._fileIdLowPart == second._fileIdLowPart &&
                first._fileIdHighPart == second._fileIdHighPart &&
                first._volumeSerialNumber == second._volumeSerialNumber;
        }

        /// <summary>
        /// Indicates whether the values of two specified <see cref="FileId" /> objects are not equal.
        /// </summary>
        /// <param name="first">The first object to compare.</param>
        /// <param name="second">The second object to compare.</param>
        /// <returns>
        /// true if <paramref name="first" /> and <paramref name="second" /> are not equal; otherwise, false.
        /// </returns>
        public static bool operator !=(FileId first, FileId second)
        {
            return first._fileIdLowPart != second._fileIdLowPart ||
                first._fileIdHighPart != second._fileIdHighPart ||
                first._volumeSerialNumber != second._volumeSerialNumber;
        }

        /// <summary>
        /// Implements the operator &lt;.
        /// </summary>
        /// <param name="first">The first operand.</param>
        /// <param name="second">The second operand.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator < (FileId first, FileId second)
        {
            //N.B. must be tested in this order
            return first._volumeSerialNumber < second._volumeSerialNumber ||
                first._fileIdHighPart < second._fileIdHighPart ||
                first._fileIdLowPart < second._fileIdLowPart;
        }

        /// <summary>
        /// Implements the operator &gt;.
        /// </summary>
        /// <param name="first">The first operand.</param>
        /// <param name="second">The second operand.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator >(FileId first, FileId second)
        {
            //N.B. must be tested in this order
            return first._volumeSerialNumber > second._volumeSerialNumber ||
                first._fileIdHighPart > second._fileIdHighPart ||
                first._fileIdLowPart > second._fileIdLowPart;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                //_fileIdHighPart is 0 on NTFS and should be often 0 on ReFS, thus ignore it
                return (int)this._fileIdLowPart ^ ((int)(this._fileIdLowPart >> 32) | (int)this._volumeSerialNumber);
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            unchecked
            {
                return string.Format(CultureInfo.CurrentCulture, "{0}-{1}-{2} : {3}-{4}-{5}",
                    ((uint)(_volumeSerialNumber >> 32)).ToString("X", CultureInfo.CurrentCulture),
                    ((ushort)(_volumeSerialNumber >> 16)).ToString("X", CultureInfo.CurrentCulture),
                    ((ushort)(_volumeSerialNumber)).ToString("X", CultureInfo.CurrentCulture),
                    _fileIdHighPart.ToString("X", CultureInfo.CurrentCulture),
                    ((uint)(_fileIdLowPart >> 32)).ToString("X", CultureInfo.CurrentCulture),
                    ((uint)_fileIdLowPart).ToString("X", CultureInfo.CurrentCulture));
            }
        }
    }

    #endregion // FileId
}
