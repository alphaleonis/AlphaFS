/* Copyright (c) 2008-2014 Peter Palotas, Jeffrey Jangli, Normalex
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
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
#if NET35
using System.Security.Permissions;
#endif

namespace Alphaleonis.Win32
{
   /// <summary>An IntPtr wrapper which can be used as the result of a Marshal.AllocHGlobal operation.
   /// <para>Calls Marshal.FreeHGlobal when disposed or finalized.</para>
   /// </summary>
   internal sealed class SafeGlobalMemoryBufferHandle : SafeHandleZeroOrMinusOneIsInvalid
   {
      #region Constructors

      /// <summary>Creates new instance with zero IntPtr.</summary>
      public SafeGlobalMemoryBufferHandle() : base(true)
      {
         _capacity = 0;
      }

      /// <summary>Creates new instance which allocates unmanaged memory of given size. Can throw OutOfMemoryException</summary>      
      public SafeGlobalMemoryBufferHandle(int capacity) : base(true)
      {
         SetHandle(Marshal.AllocHGlobal(capacity));
         _capacity = capacity;
      }

      #endregion // Constructors

      #region Methods
      
      #region CopyFrom

      /// <summary>Copies data from a one-dimensional, managed 8-bit unsigned integer array to the unmanaged memory pointer referenced by this instance.</summary>
      /// <param name="source">The one-dimensional array to copy from.</param>
      /// <param name="startIndex">The zero-based index into the array where Copy should start.</param>
      /// <param name="length">The number of array elements to copy.</param>
#if NET35
      [SecurityPermissionAttribute(SecurityAction.LinkDemand, UnmanagedCode = true)]
#endif
      public void CopyFrom(byte[] source, int startIndex, int length)
      {
         Marshal.Copy(source, startIndex, handle, length);
      }

      #endregion // CopyFrom

      #region CopyTo

#if NET35
      [SecurityPermissionAttribute(SecurityAction.LinkDemand, UnmanagedCode = true)]
#endif
      public void CopyTo(byte[] destination, int destinationOffset, int length)
      {
         if (destination == null)
            throw new ArgumentNullException("destination");

         if (destinationOffset < 0)
            throw new ArgumentOutOfRangeException("destinationOffset", Resources.SafeMemoryBufferHandle_CopyTo_Destination_offset_must_not_be_negative);

         if (length < 0)
            throw new ArgumentOutOfRangeException("length", Resources.SafeMemoryBufferHandle_CopyTo_Length_must_not_be_negative);

         if (destinationOffset + length > destination.Length)
            throw new ArgumentException("Destination buffer not large enough for the requested operation.");

         if (length > Capacity)
            throw new ArgumentOutOfRangeException("length", Resources.SafeGlobalMemoryBufferHandle_CopyTo_Source_offset_and_length_outside_the_bounds_of_the_array);

         Marshal.Copy(handle, destination, destinationOffset, length);
      }

      #endregion // CopyTo

      #region CreateFromLong

      [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
      public static SafeGlobalMemoryBufferHandle CreateFromLong(long? value)
      {
         if (value.HasValue)
         {
            SafeGlobalMemoryBufferHandle safeBuffer = new SafeGlobalMemoryBufferHandle(Marshal.SizeOf(typeof(long)));
            Marshal.WriteInt64(safeBuffer.handle, value.Value);
            return safeBuffer;
         }

         return new SafeGlobalMemoryBufferHandle();
      }

      #endregion // CreateFromLong
      
      #region ReleaseHandle

      /// <summary>Called when object is disposed or finalized.</summary>
#if NET35
      [SecurityPermissionAttribute(SecurityAction.LinkDemand, UnmanagedCode = true)]
#endif
      protected override bool ReleaseHandle()
      {
         if (handle != IntPtr.Zero)
            Marshal.FreeHGlobal(handle);

         return true;
      }

      #endregion // ReleaseHandle

      #region ToByteArray

      public byte[] ToByteArray(int startIndex, int length)
      {
         if (IsInvalid)
            return null;

         byte[] arr = new byte[length];
         Marshal.Copy(handle, arr, startIndex, length);
         return arr;
      }

      #endregion // ToByteArray

      #endregion // Methods

      #region Properties

      #region Capacity

      private readonly int _capacity;

      public int Capacity
      {
         get { return _capacity; }
      }

      #endregion // Capacity

      #endregion // Properties
   }
}