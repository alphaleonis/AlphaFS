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
 *
 * 
 * Copyright (c) Damien Guard.  All rights reserved.
 * AlphaFS has written permission from the author to include the CRC code.
 */

using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Alphaleonis.Win32.Security
{
   /// <summary>Implements a 32-bit CRC hash algorithm compatible with Zip etc.</summary>
   /// <remarks>
   /// Crc32 should only be used for backward compatibility with older file formats
   /// and algorithms. It is not secure enough for new applications.
   /// If you need to call multiple times for the same data either use the HashAlgorithm
   /// interface or remember that the result of one Compute call needs to be ~ (XOR) before
   /// being passed in as the seed for the next Compute call.
   /// </remarks>
   public sealed class Crc32 : HashAlgorithm
   {
      public const uint DefaultPolynomial = 0xedb88320u;
      public const uint DefaultSeed = 0xffffffffu;

      static uint[] defaultTable;

      readonly uint seed;
      readonly uint[] table;
      uint hash;


      public Crc32() : this(DefaultPolynomial, DefaultSeed)
      {
      }


      public Crc32(uint polynomial, uint seed)
      {
         table = InitializeTable(polynomial);
         this.seed = hash = seed;
      }


      public override void Initialize()
      {
         hash = seed;
      }


      protected override void HashCore(byte[] array, int ibStart, int cbSize)
      {
         hash = CalculateHash(table, hash, array, ibStart, cbSize);
      }


      protected override byte[] HashFinal()
      {
         var hashBuffer = UInt32ToBigEndianBytes(~hash);
         HashValue = hashBuffer;
         return hashBuffer;
      }


      public override int HashSize
      {
         get { return 32; }
      }


      public static uint Compute(byte[] buffer)
      {
         return Compute(DefaultSeed, buffer);
      }


      public static uint Compute(uint seed, byte[] buffer)
      {
         return Compute(DefaultPolynomial, seed, buffer);
      }


      public static uint Compute(uint polynomial, uint seed, byte[] buffer)
      {
         return ~CalculateHash(InitializeTable(polynomial), seed, buffer, 0, buffer.Length);
      }


      private static uint[] InitializeTable(uint polynomial)
      {
         if (polynomial == DefaultPolynomial && defaultTable != null)
            return defaultTable;

         var createTable = new uint[256];
         for (var i = 0; i < 256; i++)
         {
            var entry = (uint) i;
            for (var j = 0; j < 8; j++)
               if ((entry & 1) == 1)
                  entry = (entry >> 1) ^ polynomial;
               else
                  entry = entry >> 1;
            createTable[i] = entry;
         }

         if (polynomial == DefaultPolynomial)
            defaultTable = createTable;

         return createTable;
      }


      private static uint CalculateHash(uint[] table, uint seed, IList<byte> buffer, int start, int size)
      {
         var hash = seed;

         for (var i = start; i < start + size; i++)
            hash = (hash >> 8) ^ table[buffer[i] ^ hash & 0xff];

         return hash;
      }


      private static byte[] UInt32ToBigEndianBytes(uint uint32)
      {
         var result = BitConverter.GetBytes(uint32);

         if (BitConverter.IsLittleEndian)
            Array.Reverse(result);

         return result;
      }
   }
}
