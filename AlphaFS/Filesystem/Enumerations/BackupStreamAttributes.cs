/* Copyright (c) 2008-2014 Peter Palotas, Alexandr Normuradov, Jeffrey Jangli
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
   /// <summary>Attributes of data to facilitate cross-operating system transfer.</summary>
   /// <seealso cref="BackupFileStream"/>    
   [Flags]
   public enum BackupStreamAttributes
   {
      /// <summary>This backup stream has no special attributes.</summary>
      None = 0,

      /// <summary>Attribute set if the stream contains data that is modified when read. Allows the backup application to know that verification of data will fail.</summary>
      ModifiedWhenRead = 1,

      /// <summary>Stream contains security data (general attributes). Allows the stream to be ignored on cross-operations restore. This attribute only applies to backup stream of type <see cref="T:BackupStreamType.SecurityData"/>.</summary>
      ContainsSecurity = 2,

      /// <summary>Reserved.</summary>
      ContainsProperties = 4,

      /// <summary>The backup stream is part of a sparse file stream. This attribute applies only to backup stream of type <see cref="T:BackupStreamType.Data"/>, <see cref="T:BackupStreamType.AlternateData"/>, and <see cref="T:BackupStreamType.SparseBlock"/>.</summary>
      Sparse = 8
   }
}