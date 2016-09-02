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
   /// <summary>Implements a 64-bit CRC hash algorithm for a given polynomial.</summary>
   /// <remarks>Use <see cref="Crc64Iso"/> for ISO 3309 compliant 64-bit CRC's.</remarks>
   public class Crc64 : HashAlgorithm
   {
      public const ulong DefaultSeed = 0x0;

      readonly ulong[] table;

      readonly ulong seed;
      ulong hash;


      /// <summary>
      /// 
      /// </summary>
      /// <param name="polynomial"></param>
      public Crc64(ulong polynomial) : this(polynomial, DefaultSeed)
      {
      }


      /// <summary>
      /// 
      /// </summary>
      /// <param name="polynomial"></param>
      /// <param name="seed"></param>
      public Crc64(ulong polynomial, ulong seed)
      {
         table = InitializeTable(polynomial);
         this.seed = hash = seed;
      }


      /// <summary>
      /// 
      /// </summary>
      public override void Initialize()
      {
         hash = seed;
      }


      /// <summary>
      /// 
      /// </summary>
      protected override void HashCore(byte[] array, int ibStart, int cbSize)
      {
         hash = CalculateHash(hash, table, array, ibStart, cbSize);
      }


      /// <summary>
      /// 
      /// </summary>
      protected override byte[] HashFinal()
      {
         var hashBuffer = UInt64ToBigEndianBytes(hash);
         HashValue = hashBuffer;
         return hashBuffer;
      }


      /// <summary>
      /// 
      /// </summary>
      public override int HashSize
      {
         get { return 64; }
      }


      /// <summary>
      /// 
      /// </summary>
      /// <param name="seed"></param>
      /// <param name="table"></param>
      /// <param name="buffer"></param>
      /// <param name="start"></param>
      /// <param name="size"></param>
      protected static ulong CalculateHash(ulong seed, ulong[] table, IList<byte> buffer, int start, int size)
      {
         var hash = seed;
         for (var i = start; i < start + size; i++)
            unchecked
            {
               hash = (hash >> 8) ^ table[(buffer[i] ^ hash) & 0xff];
            }
         return hash;
      }


      private static byte[] UInt64ToBigEndianBytes(ulong value)
      {
         var result = BitConverter.GetBytes(value);

         if (BitConverter.IsLittleEndian)
            Array.Reverse(result);

         return result;
      }


      private static ulong[] InitializeTable(ulong polynomial)
      {
         if (polynomial == Crc64Iso.Iso3309Polynomial && Crc64Iso.Table != null)
            return Crc64Iso.Table;

         var createTable = CreateTable(polynomial);

         if (polynomial == Crc64Iso.Iso3309Polynomial)
            Crc64Iso.Table = createTable;

         return createTable;
      }


      /// <summary>
      /// 
      /// </summary>
      /// <param name="polynomial"></param>
      protected static ulong[] CreateTable(ulong polynomial)
      {
         var createTable = new ulong[256];
         for (var i = 0; i < 256; ++i)
         {
            var entry = (ulong) i;
            for (var j = 0; j < 8; ++j)
               if ((entry & 1) == 1)
                  entry = (entry >> 1) ^ polynomial;
               else
                  entry = entry >> 1;
            createTable[i] = entry;
         }

         return createTable;
      }
   }


   /// <summary>Implements a 64-bit CRC hash algorithm for a given polynomial.</summary>
   public class Crc64Iso : Crc64
   {
      internal static ulong[] Table;

      /// <summary>
      /// 
      /// </summary>
      public const ulong Iso3309Polynomial = 0xD800000000000000;


      /// <summary>
      /// 
      /// </summary>
      public Crc64Iso() : base(Iso3309Polynomial)
      {
      }


      /// <summary>
      /// 
      /// </summary>
      /// <param name="seed"></param>
      public Crc64Iso(ulong seed) : base(Iso3309Polynomial, seed)
      {
      }


      /// <summary>
      /// 
      /// </summary>
      /// <param name="buffer"></param>
      /// <returns></returns>
      public static ulong Compute(byte[] buffer)
      {
         return Compute(DefaultSeed, buffer);
      }


      /// <summary>
      /// 
      /// </summary>
      /// <param name="seed"></param>
      /// <param name="buffer"></param>
      /// <returns></returns>
      public static ulong Compute(ulong seed, byte[] buffer)
      {
         if (Table == null)
            Table = CreateTable(Iso3309Polynomial);

         return CalculateHash(seed, Table, buffer, 0, buffer.Length);
      }
   }
}
