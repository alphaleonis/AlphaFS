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
using System.Reflection;

namespace AlphaFS.UnitTest
{
   partial class FileTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void File_Replace_NoBackup_LocalAndNetwork_Success()
      {
         File_Replace_NoBackup(false);
         File_Replace_NoBackup(true);
      }


      private void File_Replace_NoBackup(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         using (var tempRoot = new TemporaryDirectory(isNetwork ? Alphaleonis.Win32.Filesystem.Path.LocalToUnc(UnitTestConstants.TempPath) : UnitTestConstants.TempPath, MethodBase.GetCurrentMethod().Name))
         {
            var fileSrc = tempRoot.RandomFileFullPathNoExtension + "-" + UnitTestConstants.TextHelloWorld + ".txt";
            var fileDst = tempRoot.RandomFileFullPathNoExtension + "-" + UnitTestConstants.TextGoodbyeWorld + ".txt";
            var fileBackup = tempRoot.RandomFileFullPathNoExtension + "-Backup.txt";

            Console.WriteLine("\nInput File Path: [{0}]", fileSrc);


            using (var stream = System.IO.File.CreateText(fileSrc))
               stream.Write(UnitTestConstants.TextHelloWorld);

            using (var stream = System.IO.File.CreateText(fileDst))
               stream.Write(UnitTestConstants.TextGoodbyeWorld);


            Alphaleonis.Win32.Filesystem.File.Replace(fileSrc, fileDst, null);


            Assert.IsFalse(System.IO.File.Exists(fileSrc), "The file exists, but is expected not to.");
            Assert.IsTrue(System.IO.File.Exists(fileDst), "The file does not exist, but is expected to.");
            Assert.IsFalse(System.IO.File.Exists(fileBackup), "The file exists, but is expected not to.");


            Assert.AreEqual(UnitTestConstants.TextHelloWorld, Alphaleonis.Win32.Filesystem.File.ReadAllText(fileDst), "The texts do not match, but are expected to.");
         }

         Console.WriteLine();
      }
   }
}
