/*  Copyright (C) 2008-2018 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AlphaFS.UnitTest
{
   public partial class AlphaFS_EncryptionTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void AlphaFS_File_GetHash_LocalAndNetwork_Success()
      {
         AlphaFS_File_GetHash(false);
         AlphaFS_File_GetHash(true);
      }


      private void AlphaFS_File_GetHash(bool isNetwork)
      {
         using (var tempRoot = new TemporaryDirectory(isNetwork))
         {
            var file = System.IO.Path.Combine(tempRoot.Directory.FullName, "HashFile.txt");
            System.IO.File.WriteAllText(file, new Guid().ToString());
            
            Console.WriteLine("Input File Path: [{0}]\n", file);


            var type = Alphaleonis.Win32.Security.HashType.CRC32;
            var hash = Alphaleonis.Win32.Filesystem.File.GetHash(file, type);
            Console.WriteLine("\t{0,12} = {1}", type, hash);
            Assert.AreEqual("8151DA89", hash);


            type = Alphaleonis.Win32.Security.HashType.CRC64ISO3309;
            hash = Alphaleonis.Win32.Filesystem.File.GetHash(file, type);
            Console.WriteLine("\t{0,12} = {1}", type, hash);
            Assert.AreEqual("130BC432FA070BC4", hash);


            type = Alphaleonis.Win32.Security.HashType.MD5;
            hash = Alphaleonis.Win32.Filesystem.File.GetHash(file, type);
            Console.WriteLine("\t{0,12} = {1}", type, hash);
            Assert.AreEqual("9F89C84A559F573636A47FF8DAED0D33", hash);


            type = Alphaleonis.Win32.Security.HashType.RIPEMD160;
            hash = Alphaleonis.Win32.Filesystem.File.GetHash(file, type);
            Console.WriteLine("\t{0,12} = {1}", type, hash);
            Assert.AreEqual("E38FD4F808D316C9671F0808AE1457330AD769AA", hash);


            type = Alphaleonis.Win32.Security.HashType.SHA1;
            hash = Alphaleonis.Win32.Filesystem.File.GetHash(file, type);
            Console.WriteLine("\t{0,12} = {1}", type, hash);
            Assert.AreEqual("B602D594AFD2B0B327E07A06F36CA6A7E42546D0", hash);


            type = Alphaleonis.Win32.Security.HashType.SHA256;
            hash = Alphaleonis.Win32.Filesystem.File.GetHash(file, type);
            Console.WriteLine("\t{0,12} = {1}", type, hash);
            Assert.AreEqual("12B9377CBE7E5C94E8A70D9D23929523D14AFA954793130F8A3959C7B849ACA8", hash);


            type = Alphaleonis.Win32.Security.HashType.SHA384;
            hash = Alphaleonis.Win32.Filesystem.File.GetHash(file, type);
            Console.WriteLine("\t{0,12} = {1}", type, hash);
            Assert.AreEqual("70255C353A82BC55634A251D657F1813A74B3ECA31DDE11DF99017F1DE7504820FB054D1853B6E5F53251AAEB66D0469", hash);


            type = Alphaleonis.Win32.Security.HashType.SHA512;
            hash = Alphaleonis.Win32.Filesystem.File.GetHash(file, type);
            Console.WriteLine("\t{0,12} = {1}", type, hash);
            Assert.AreEqual("A13DC074B31564A6A3CF4A605BFF19FADE6C19992A4123A7022D5A07C2E2D2D5E059FF0BA25AE0750D709FDB0AC757A1C615199A1C1422902D33C41E45B9F9D5", hash);
         }

         Console.WriteLine();
      }
   }
}
