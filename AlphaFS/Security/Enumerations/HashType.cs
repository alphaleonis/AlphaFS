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

namespace Alphaleonis.Win32.Security
{
   /// <summary>Enum containing the supported hash types.</summary>
   public enum HashType
   {
      /// <summary>Cyclic Redundancy Check</summary>
      CRC32,

      /// <summary>Cyclic Redundancy Check</summary>
      CRC64ISO3309,

      /// <summary>Message Authentication Code Triple Data Encryption Standard</summary>
      MACTripleDES,

      /// <summary>Message digest</summary>
      MD5,

      /// <summary>RACE Integrity Primitives Evaluation Message Digest</summary>
      RIPEMD160,

      /// <summary>Secure Hash Algorithm</summary>
      SHA1,

      /// <summary>Secure Hash Algorithm</summary>
      SHA256,

      /// <summary>Secure Hash Algorithm</summary>
      SHA384,

      /// <summary>Secure Hash Algorithm</summary>
      SHA512
   }
}
