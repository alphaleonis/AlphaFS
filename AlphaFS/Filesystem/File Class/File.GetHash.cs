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

using System.Globalization;
using System.IO;
using System.Security;
using System.Text;
using Alphaleonis.Win32.Security;

namespace Alphaleonis.Win32.Filesystem
{
   public static partial class File
   {
      /// <summary>[AlphaFS] Calculates the hash/checksum for the given <paramref name="fileFullPath"/>.</summary>
      /// <param name="fileFullPath">The name of the file.</param>
      /// <param name="hashType">One of the <see cref="HashType"/> values.</param>
      [SecurityCritical]
      public static string GetHash(string fileFullPath, HashType hashType)
      {
         return GetHashCore(null, fileFullPath, hashType, PathFormat.RelativePath);
      }


      /// <summary>[AlphaFS] Calculates the hash/checksum for the given <paramref name="fileFullPath"/>.</summary>
      /// <param name="fileFullPath">The name of the file.</param>
      /// <param name="hashType">One of the <see cref="HashType"/> values.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static string GetHash(string fileFullPath, HashType hashType, PathFormat pathFormat)
      {
         return GetHashCore(null, fileFullPath, hashType, pathFormat);
      }


      /// <summary>[AlphaFS] Calculates the hash/checksum for the given <paramref name="fileFullPath"/>.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="fileFullPath">The name of the file.</param>
      /// <param name="hashType">One of the <see cref="HashType"/> values.</param>
      [SecurityCritical]
      public static string GetHash(KernelTransaction transaction, string fileFullPath, HashType hashType)
      {
         return GetHashCore(transaction, fileFullPath, hashType, PathFormat.RelativePath);
      }


      /// <summary>[AlphaFS] Calculates the hash/checksum for the given <paramref name="fileFullPath"/>.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="fileFullPath">The name of the file.</param>
      /// <param name="hashType">One of the <see cref="HashType"/> values.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static string GetHash(KernelTransaction transaction, string fileFullPath, HashType hashType, PathFormat pathFormat)
      {
         return GetHashCore(transaction, fileFullPath, hashType, pathFormat);
      }




      /// <summary>[AlphaFS] Calculates the hash/checksum for the given <paramref name="fileFullPath"/>.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="hashType">One of the <see cref="HashType"/> values.</param>
      /// <param name="fileFullPath">The name of the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param> 
      [SecurityCritical]
      internal static string GetHashCore(KernelTransaction transaction, string fileFullPath, HashType hashType, PathFormat pathFormat)
      {
         const GetFullPathOptions options = GetFullPathOptions.RemoveTrailingDirectorySeparator | GetFullPathOptions.FullCheck;
         var fileNameLp = Path.GetExtendedLengthPathCore(transaction, fileFullPath, pathFormat, options);

         byte[] hash = null;


         using (var fs = OpenCore(transaction, fileNameLp, FileMode.Open, FileAccess.Read, FileShare.Read, ExtendedFileAttributes.Normal, null, null, PathFormat.LongFullPath))
         {
            switch (hashType)
            {
               case HashType.CRC32:
                  using (var hType = new Crc32())
                     hash = hType.ComputeHash(fs);
                  break;


               case HashType.CRC64ISO3309:
                  using (var hType = new Crc64())
                     hash = hType.ComputeHash(fs);
                  break;


               case HashType.MD5:
                  using (var hType = System.Security.Cryptography.MD5.Create())
                     hash = hType.ComputeHash(fs);
                  break;


               case HashType.RIPEMD160:
                  using (var hType = System.Security.Cryptography.RIPEMD160.Create())
                     hash = hType.ComputeHash(fs);
                  break;


               case HashType.SHA1:
                  using (var hType = System.Security.Cryptography.SHA1.Create())
                     hash = hType.ComputeHash(fs);
                  break;


               case HashType.SHA256:
                  using (var hType = System.Security.Cryptography.SHA256.Create())
                     hash = hType.ComputeHash(fs);
                  break;


               case HashType.SHA384:
                  using (var hType = System.Security.Cryptography.SHA384.Create())
                     hash = hType.ComputeHash(fs);
                  break;


               case HashType.SHA512:
                  using (var hType = System.Security.Cryptography.SHA512.Create())
                     hash = hType.ComputeHash(fs);
                  break;
            }
         }


         if (null != hash)
         {
            var sb = new StringBuilder(hash.Length);

            foreach (byte b in hash)
               sb.Append(b.ToString("X2", CultureInfo.InvariantCulture));

            return sb.ToString().ToUpperInvariant();
         }

         return string.Empty;
      }
   }
}
