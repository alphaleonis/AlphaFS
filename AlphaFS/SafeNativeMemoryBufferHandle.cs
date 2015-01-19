/*  Copyright (C) 2008-2015 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
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

using Microsoft.Win32.SafeHandles;
using System;
using System.Runtime.InteropServices;

namespace Alphaleonis.Win32
{
   /// <summary>
   /// Base class for classes representing a block of unmanaged memory.
   /// </summary>
   internal abstract class SafeNativeMemoryBufferHandle : SafeHandleZeroOrMinusOneIsInvalid
   {
      #region Private Fields

      private int m_capacity;

      #endregion

      #region Constructors

      protected SafeNativeMemoryBufferHandle(bool ownsHandle)
         : base(ownsHandle)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="SafeNativeMemoryBufferHandle"/> specifying the allocated capacity of the memory block. 
      /// </summary>
      /// <param name="capacity">The capacity.</param>
      protected SafeNativeMemoryBufferHandle(int capacity)
         : this(true)
      {
         m_capacity = capacity;
      }

      protected SafeNativeMemoryBufferHandle(IntPtr memory, int capacity)
         : this(capacity)
      {
         SetHandle(memory);
      }

      #endregion

      #region Properties

      /// <summary>
      /// Gets the capacity. Only valid if this instance was created using a constructor that specifies the size,
      /// it is not correct if this handle was returned by a native method using p/invoke.
      /// </summary>
      public int Capacity
      {
         get { return m_capacity; }
      }

      #endregion

      #region Public Methods

      /// <summary>
      /// Copies data from a one-dimensional, managed 8-bit unsigned integer array to the unmanaged memory pointer referenced by this instance-
      /// </summary>
      /// <param name="source">The one-dimensional array to copy from. </param>
      /// <param name="startIndex">The zero-based index into the array where Copy should start.</param>
      /// <param name="length">The number of array elements to copy.</param>
      public void CopyFrom(byte[] source, int startIndex, int length)
      {
         Marshal.Copy(source, startIndex, handle, length);
      }

      public void CopyFrom(char[] source, int startIndex, int length)
      {
         Marshal.Copy(source, startIndex, handle, length);
      }

      public void CopyFrom(char[] source, int startIndex, int length, int offset)
      {
         Marshal.Copy(source, startIndex, new IntPtr(handle.ToInt64() + offset), length);
      }

      /// <summary>Copies data from an unmanaged memory pointer to a managed 8-bit unsigned integer array.</summary>
      /// <param name="destination">The array to copy to.</param>
      /// <param name="destinationOffset">The zero-based index in the destination array where copying should start.</param>
      /// <param name="length">The number of array elements to copy.</param>
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

         Marshal.Copy(handle, destination, destinationOffset, length);
      }

      /// <summary>Copies data from an unmanaged memory pointer to a managed 8-bit unsigned integer array.</summary>
      /// <param name="destination">The array to copy to.</param>
      /// <param name="sourceOffset"></param>
      /// <param name="destinationOffset">The zero-based index in the destination array where copying should start.</param>
      /// <param name="length">The number of array elements to copy.</param>
      public void CopyFromSourceOffset(byte[] destination, IntPtr sourceOffset, int destinationOffset, int length)
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

         Marshal.Copy(new IntPtr(handle.ToInt64() + sourceOffset.ToInt64()), destination, destinationOffset, length);
      }

      public byte[] ToByteArray(int startIndex, int length)
      {
         if (IsInvalid)
            return null;

         byte[] arr = new byte[length];
         Marshal.Copy(handle, arr, startIndex, length);
         return arr;
      }

      #region Write

      public void WriteInt16(int offset, short value)
      {
         Marshal.WriteInt16(handle, offset, value);
      }

      public void WriteInt16(int offset, char value)
      {
         Marshal.WriteInt16(handle, offset, value);
      }

      public void WriteInt16(char value)
      {
         Marshal.WriteInt16(handle, value);
      }

      public void WriteInt16(short value)
      {
         Marshal.WriteInt16(handle, value);
      }

      public void WriteInt32(int offset, short value)
      {
         Marshal.WriteInt32(handle, offset, value);
      }

      public void WriteInt32(int value)
      {
         Marshal.WriteInt32(handle, value);
      }

      public void WriteInt64(int offset, long value)
      {
         Marshal.WriteInt64(handle, offset, value);
      }

      public void WriteInt64(long value)
      {
         Marshal.WriteInt64(handle, value);
      }

      public void WriteByte(int offset, byte value)
      {
         Marshal.WriteByte(handle, offset, value);
      }

      public void WriteByte(byte value)
      {
         Marshal.WriteByte(handle, value);
      }

      public void WriteIntPtr(int offset, IntPtr value)
      {
         Marshal.WriteIntPtr(handle, offset, value);
      }

      public void WriteIntPtr(IntPtr value)
      {
         Marshal.WriteIntPtr(handle, value);
      }

      #endregion // Write

      #region Read

      public byte ReadByte()
      {
         return Marshal.ReadByte(handle);
      }

      public byte ReadByte(int offset)
      {
         return Marshal.ReadByte(handle, offset);
      }

      public short ReadInt16()
      {
         return Marshal.ReadInt16(handle);
      }

      public short ReadInt16(int offset)
      {
         return Marshal.ReadInt16(handle, offset);
      }

      public int ReadInt32()
      {
         return Marshal.ReadInt32(handle);
      }

      public int ReadInt32(int offset)
      {
         return Marshal.ReadInt32(handle, offset);
      }

      public long ReadInt64()
      {
         return Marshal.ReadInt64(handle);
      }

      public long ReadInt64(int offset)
      {
         return Marshal.ReadInt64(handle, offset);
      }

      public IntPtr ReadIntPtr()
      {
         return Marshal.ReadIntPtr(handle);
      }

      public IntPtr ReadIntPtr(int offset)
      {
         return Marshal.ReadIntPtr(handle, offset);
      }

      #endregion // Read

      public void StructureToPtr(object structure, bool deleteOld)
      {
         Marshal.StructureToPtr(structure, handle, deleteOld);
      }

      public T PtrToStructure<T>()
      {
         return PtrToStructure<T>(0);
      }

      public T PtrToStructure<T>(int offset)
      {
         return (T)Marshal.PtrToStructure(new IntPtr(handle.ToInt64() + offset), typeof(T));
      }

      public string PtrToStringUni(int length)
      {
         return PtrToStringUni(0, length);
      }

      public string PtrToStringUni(int offset, int length)
      {
         return Marshal.PtrToStringUni(new IntPtr(handle.ToInt64() + offset), length);
      }

      #endregion // Public Methods
   }
}