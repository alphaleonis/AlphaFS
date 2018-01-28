/*  Copyright (C) 2008-2017 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
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
using System.Reflection;

namespace AlphaFS.UnitTest
{
   public partial class AlphaFS_EncryptionTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>

      [TestMethod]
      public void AlphaFS_File_GetHash_LocalAndNetwork_Success()
      {
         File_GetHash(false);
         File_GetHash(true);
      }




      private void File_GetHash(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         var tempPath = System.IO.Path.GetTempPath();
         if (isNetwork)
            tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);


         using (var rootDir = new TemporaryDirectory(tempPath, MethodBase.GetCurrentMethod().Name))
         {
            var file = System.IO.Path.Combine(rootDir.Directory.FullName, "HashFile.txt");
            System.IO.File.WriteAllText(file, UnitTestConstants.TextHelloWorld);

            Console.WriteLine("\nInput File Path: [{0}]\n", file);


            var type = Alphaleonis.Win32.Security.HashType.CRC32;
            var hash = Alphaleonis.Win32.Filesystem.File.GetHash(file, type);
            Console.WriteLine("\t{0,12} = {1}", type, hash);
            Assert.AreEqual("1BF6339C", hash);


            type = Alphaleonis.Win32.Security.HashType.CRC64ISO3309;
            hash = Alphaleonis.Win32.Filesystem.File.GetHash(file, type);
            Console.WriteLine("\t{0,12} = {1}", type, hash);
            Assert.AreEqual("AD3DE8A8C701F74E", hash);


            type = Alphaleonis.Win32.Security.HashType.MD5;
            hash = Alphaleonis.Win32.Filesystem.File.GetHash(file, type);
            Console.WriteLine("\t{0,12} = {1}", type, hash);
            Assert.AreEqual("4CF01535A68A0F4AAFD0631F3A000C52", hash);


            type = Alphaleonis.Win32.Security.HashType.RIPEMD160;
            hash = Alphaleonis.Win32.Filesystem.File.GetHash(file, type);
            Console.WriteLine("\t{0,12} = {1}", type, hash);
            Assert.AreEqual("C2183FB7668FE4A2382A8E6D00501B3C114A6E9A", hash);


            type = Alphaleonis.Win32.Security.HashType.SHA1;
            hash = Alphaleonis.Win32.Filesystem.File.GetHash(file, type);
            Console.WriteLine("\t{0,12} = {1}", type, hash);
            Assert.AreEqual("53F20D3D826642B232F5E514CB2AADFC359417E8", hash);


            type = Alphaleonis.Win32.Security.HashType.SHA256;
            hash = Alphaleonis.Win32.Filesystem.File.GetHash(file, type);
            Console.WriteLine("\t{0,12} = {1}", type, hash);
            Assert.AreEqual("07E7173232F22A1B746FE15C98BCB294A668F8300F645174BF0B83E2D8EADDCB", hash);


            type = Alphaleonis.Win32.Security.HashType.SHA384;
            hash = Alphaleonis.Win32.Filesystem.File.GetHash(file, type);
            Console.WriteLine("\t{0,12} = {1}", type, hash);
            Assert.AreEqual("BF5FFD4053C364A8DB68608D79DB0A016E990B1BE8F1D91FCFC820AC89456F4AFAF8307DEEA4A7F663CE03D0EC21AD58", hash);


            type = Alphaleonis.Win32.Security.HashType.SHA512;
            hash = Alphaleonis.Win32.Filesystem.File.GetHash(file, type);
            Console.WriteLine("\t{0,12} = {1}", type, hash);
            Assert.AreEqual("FFC32D18263B784A6E661E852DC5FF3A4C75425A42974CF067DDC01A17FFD34CF6E9717623A7D9C6EA19B04DBF5FDADECDE06122DAB0C26CFC83B1EA61C7B382", hash);
         }

         Console.WriteLine();
      }
   }
}
