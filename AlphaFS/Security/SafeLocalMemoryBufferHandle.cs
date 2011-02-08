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
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using System;
using System.Security.Permissions;

namespace Alphaleonis.Win32.Security
{
    /// <summary>
    /// IntPtr wrapper which can be used as result of
    /// Marshal.AllocHGlobal operation.
    /// Calls Marshal.FreeHGlobal when disposed or finalized.
    /// </summary>
    internal sealed class SafeLocalMemoryBufferHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        /// <summary>
        /// Creates new instance with zero IntPtr
        /// </summary>
        [SecurityPermission(SecurityAction.LinkDemand)]
        public SafeLocalMemoryBufferHandle()
            : base(true)
        {
        }

        /// <summary>
        /// Copies data from a one-dimensional, managed 8-bit unsigned integer array to the unmanaged memory pointer referenced by this instance-
        /// </summary>
        /// <param name="source">The one-dimensional array to copy from. </param>
        /// <param name="startIndex">The zero-based index into the array where Copy should start.</param>
        /// <param name="length">The number of array elements to copy.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"), SecurityPermission(SecurityAction.LinkDemand)]
        public void CopyFrom(byte[] source, int startIndex, int length)
        {
            Marshal.Copy(source, startIndex, handle, length);
        }

        [SecurityPermission(SecurityAction.LinkDemand)]
        public void CopyTo(byte[] destination, int destinationOffset, int length)
        {
            if (destination == null)
                throw new ArgumentNullException("destination");

            if (destinationOffset < 0)
                throw new ArgumentOutOfRangeException("destinationOffset", "Destination offset must not be negative");

            if (length < 0)
                throw new ArgumentOutOfRangeException("length", "Length must not be negative.");

            if (destinationOffset + length > destination.Length)
                throw new ArgumentException("Destination buffer not large enough for the requested operation.");

            Marshal.Copy(this.handle, destination, destinationOffset, length); 
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public byte[] ToByteArray(int startIndex, int length)
        {
            if (IsInvalid)
                return null;

            byte [] arr = new byte[length];
            Marshal.Copy(handle, arr, startIndex, length);
            return arr;
        }

          /// <summary>
        /// Called when object is disposed or finalized.
        /// </summary>
        protected override bool ReleaseHandle()
        {
            if (NativeMethods.LocalFree(this.handle) != IntPtr.Zero)
                return false;
            return true;
        }

    }
}
