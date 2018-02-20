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
using System.Text;

namespace AlphaFS.UnitTest
{
   partial class FileTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>

      [TestMethod]
      public void File_OpenRead_LocalAndNetwork_Success()
      {
         File_OpenRead(false);
         File_OpenRead(true);
      }


      [TestMethod]
      public void File_OpenRead_OpenReadTwiceShouldNotLock_LocalAndNetwork_Success()
      {
         File_OpenRead_OpenReadTwiceShouldNotLock(false);
         File_OpenRead_OpenReadTwiceShouldNotLock(true);
      }




      private void File_OpenRead(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         var tempPath = System.IO.Path.GetTempPath();
         if (isNetwork)
            tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);


         using (var rootDir = new TemporaryDirectory(tempPath, MethodBase.GetCurrentMethod().Name))
         {
            var file = rootDir.RandomFileFullPath;
            Console.WriteLine("\nInput File Path: [{0}]\n", file);


            System.IO.File.WriteAllText(file, UnitTestConstants.TextHelloWorld);


            var sysIoStreamText = string.Empty;
            var alphaStreamText = string.Empty;

            using (var stream = System.IO.File.OpenRead(file))
            {
               var b = new byte[15];
               var temp = new UTF8Encoding(true);

               while (stream.Read(b, 0, b.Length) > 0)
                  sysIoStreamText += temp.GetString(b);
            }

            using (var stream = Alphaleonis.Win32.Filesystem.File.OpenRead(file))
            {
               var b = new byte[15];
               var temp = new UTF8Encoding(true);

               while (stream.Read(b, 0, b.Length) > 0)
                  alphaStreamText += temp.GetString(b);
            }

            Console.WriteLine("\tSystem IO: " + sysIoStreamText);
            Console.WriteLine("\tAlphaFS  : " + alphaStreamText);


            Assert.AreEqual(sysIoStreamText, alphaStreamText, "The content of the two files is not equal, but is expected to.");
         }

         Console.WriteLine();
      }


      private void File_OpenRead_OpenReadTwiceShouldNotLock(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         var tempPath = System.IO.Path.GetTempPath();
         if (isNetwork)
            tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);


         using (var rootDir = new TemporaryDirectory(tempPath, MethodBase.GetCurrentMethod().Name))
         {
            var filePath = rootDir.RandomFileFullPath;
            Console.WriteLine("\nInput File Path: [{0}]\n", filePath);


            using (System.IO.File.Create(filePath)) { }

            using (var s1 = Alphaleonis.Win32.Filesystem.File.OpenRead(filePath))
            using (var s2 = Alphaleonis.Win32.Filesystem.File.OpenRead(filePath)) {}
         }


         Console.WriteLine();
      }
   }
}
