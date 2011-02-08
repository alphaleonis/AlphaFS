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

namespace Alphaleonis.Win32
{
    /// <summary>
    /// IntPtr wrapper which can be used as result of
    /// Marshal.AllocHGlobal operation.
    /// Calls Marshal.FreeHGlobal when disposed or finalized.
    /// </summary>
    internal sealed class SafeGlobalMemoryBufferHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        /// <summary>
        /// Creates new instance with zero IntPtr
        /// </summary>
        [SecurityPermission(SecurityAction.LinkDemand)]
        public SafeGlobalMemoryBufferHandle()
            : base(true)
        {
            // Apparently it is regarded bad practice by FxCop to explicitly initialize this to zero,
            // since it will be done automatically. So commented out.
            //mCapacity = 0; 
        }

        /// <summary>
        /// Creates new instance which allocates unmanaged memory of given size
        /// Can throw OutOfMemoryException 
        /// </summary>
        [SecurityPermission(SecurityAction.LinkDemand)]
        public SafeGlobalMemoryBufferHandle(int capacity) :
            base(true)
        {
            SetHandle(Marshal.AllocHGlobal(capacity));
            mCapacity = capacity;
        }

        /// <summary>
        /// Copies data from a one-dimensional, managed 8-bit unsigned integer array to the unmanaged memory pointer referenced by this instance-
        /// </summary>
        /// <param name="source">The one-dimensional array to copy from. </param>
        /// <param name="startIndex">The zero-based index into the array where Copy should start.</param>
        /// <param name="length">The number of array elements to copy.</param>
        [SecurityPermission(SecurityAction.LinkDemand)]
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

            if (length > Capacity)
                throw new ArgumentOutOfRangeException("length", "Source offset and length outside the bounds of the array");

            Marshal.Copy(this.handle, destination, destinationOffset, length); 
        }

        public byte[] ToByteArray(int startIndex, int length)
        {
            if (IsInvalid)
                return null;

            byte [] arr = new byte[length];
            Marshal.Copy(handle, arr, startIndex, length);
            return arr;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        public static SafeGlobalMemoryBufferHandle CreateFromLong(long? value)
        {
            if (value.HasValue)
            {
                SafeGlobalMemoryBufferHandle handle = new SafeGlobalMemoryBufferHandle(Marshal.SizeOf(typeof(long)));
                Marshal.WriteInt64(handle.handle, value.Value);
                return handle;
            }
            else
                return new SafeGlobalMemoryBufferHandle();
        }

        /// <summary>
        /// Called when object is disposed or finalized.
        /// </summary>
        protected override bool ReleaseHandle()
        {
            Marshal.FreeHGlobal(handle);
            return true;
        }

        public int Capacity
        {
            get { return mCapacity; }
        }

        private int mCapacity;
    }
}
