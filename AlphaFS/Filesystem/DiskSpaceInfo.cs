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

namespace Alphaleonis.Win32.Filesystem
{
    /// <summary>
    /// Represents information about free and used space on a disk.
    /// </summary>
    public struct DiskSpaceInfo
    {
        /// <summary>
        /// Gets or sets the free bytes available.
        /// </summary>
        /// <value>The free bytes available.</value>
        public UInt64 FreeBytesAvailable { get; set; }

        /// <summary>
        /// Gets or sets the total number of bytes.
        /// </summary>
        /// <value>The total number of bytes.</value>
        public UInt64 TotalNumberOfBytes { get; set; }

        /// <summary>
        /// Gets or sets the total number of free bytes.
        /// </summary>
        /// <value>The total number of free bytes.</value>
        public UInt64 TotalNumberOfFreeBytes { get; set; }

        /// <summary>
        /// Initializes the structure
        /// </summary>
        /// <param name="freeBytes">The free bytes available</param>
        /// <param name="totalBytes">The total number of bytes</param>
        /// <param name="totalFreeBytes">The total number of free bytes</param>
        public DiskSpaceInfo(UInt64 freeBytes, UInt64 totalBytes, UInt64 totalFreeBytes)
            : this()
        {
            FreeBytesAvailable = freeBytes;
            TotalNumberOfBytes = totalBytes;
            TotalNumberOfFreeBytes = totalFreeBytes;
        }

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <param name="obj">Another object to compare to.</param>
        /// <returns>
        /// true if <paramref name="obj"/> and this instance are the same type and represent the same value; otherwise, false.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is DiskSpaceInfo))
                return false;

            DiskSpaceInfo other = (DiskSpaceInfo)obj;
            return other.FreeBytesAvailable.Equals(FreeBytesAvailable) &&
                other.TotalNumberOfBytes.Equals(TotalNumberOfBytes) &&
                other.TotalNumberOfFreeBytes.Equals(TotalNumberOfFreeBytes);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer that is the hash code for this instance.
        /// </returns>
        public override int GetHashCode()
        {
            return FreeBytesAvailable.GetHashCode() ^ TotalNumberOfFreeBytes.GetHashCode() + TotalNumberOfFreeBytes.GetHashCode();
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">A.</param>
        /// <param name="right">B.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(DiskSpaceInfo left, DiskSpaceInfo right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">A.</param>
        /// <param name="right">B.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(DiskSpaceInfo left, DiskSpaceInfo right)
        {
            return !left.Equals(right);
        }

    }
}
