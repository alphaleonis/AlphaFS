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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AlphaFS.UnitTest
{
   partial class FileTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>

      [TestMethod]
      public void AlphaFS_File_GetHash_LocalAndUNC_Success()
      {
         File_GetHash(false);
         File_GetHash(true);
      }




      private void File_GetHash(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         string tempPath = System.IO.Path.GetTempPath();
         if (isNetwork)
            tempPath = PathUtils.AsUncPath(tempPath);


         using (var rootDir = new TemporaryDirectory(tempPath, "File.GetHash"))
         {
            var file = System.IO.Path.Combine(rootDir.Directory.FullName, "HashFile.txt");
            System.IO.File.WriteAllText(file, UnitTestConstants.TextHelloWorld);

            Console.WriteLine("\nInput File Path: [{0}]\n", file);


            var type = Alphaleonis.Win32.Security.HashType.CRC32;
            var hash = Alphaleonis.Win32.Filesystem.File.GetHash(file, type);
            Console.WriteLine("\t{0,12} = {1}", type, hash);
            Assert.AreEqual("1bf6339c", hash);


            type = Alphaleonis.Win32.Security.HashType.CRC64ISO3309;
            hash = Alphaleonis.Win32.Filesystem.File.GetHash(file, type);
            Console.WriteLine("\t{0,12} = {1}", type, hash);
            Assert.AreEqual("ad3de8a8c701f74e", hash);


            type = Alphaleonis.Win32.Security.HashType.MACTripleDES;
            hash = Alphaleonis.Win32.Filesystem.File.GetHash(file, type);
            Console.WriteLine("\t{0,12} = {1}", type, hash);
            // The hash value is different each time!
            //Assert.AreEqual("de2142bf5bd4bfff728b7756d8d5364a63314177", hash);


            type = Alphaleonis.Win32.Security.HashType.MD5;
            hash = Alphaleonis.Win32.Filesystem.File.GetHash(file, type);
            Console.WriteLine("\t{0,12} = {1}", type, hash);
            Assert.AreEqual("4cf01535a68a0f4aafd0631f3a000c52", hash);


            type = Alphaleonis.Win32.Security.HashType.RIPEMD160;
            hash = Alphaleonis.Win32.Filesystem.File.GetHash(file, type);
            Console.WriteLine("\t{0,12} = {1}", type, hash);
            Assert.AreEqual("c2183fb7668fe4a2382a8e6d00501b3c114a6e9a", hash);


            type = Alphaleonis.Win32.Security.HashType.SHA1;
            hash = Alphaleonis.Win32.Filesystem.File.GetHash(file, type);
            Console.WriteLine("\t{0,12} = {1}", type, hash);
            Assert.AreEqual("53f20d3d826642b232f5e514cb2aadfc359417e8", hash);


            type = Alphaleonis.Win32.Security.HashType.SHA256;
            hash = Alphaleonis.Win32.Filesystem.File.GetHash(file, type);
            Console.WriteLine("\t{0,12} = {1}", type, hash);
            Assert.AreEqual("07e7173232f22a1b746fe15c98bcb294a668f8300f645174bf0b83e2d8eaddcb", hash);


            type = Alphaleonis.Win32.Security.HashType.SHA384;
            hash = Alphaleonis.Win32.Filesystem.File.GetHash(file, type);
            Console.WriteLine("\t{0,12} = {1}", type, hash);
            Assert.AreEqual("bf5ffd4053c364a8db68608d79db0a016e990b1be8f1d91fcfc820ac89456f4afaf8307deea4a7f663ce03d0ec21ad58", hash);


            type = Alphaleonis.Win32.Security.HashType.SHA512;
            hash = Alphaleonis.Win32.Filesystem.File.GetHash(file, type);
            Console.WriteLine("\t{0,12} = {1}", type, hash);
            Assert.AreEqual("ffc32d18263b784a6e661e852dc5ff3a4c75425a42974cf067ddc01a17ffd34cf6e9717623a7d9c6ea19b04dbf5fdadecde06122dab0c26cfc83b1ea61c7b382", hash);
         }

         Console.WriteLine();
      }
   }
}
