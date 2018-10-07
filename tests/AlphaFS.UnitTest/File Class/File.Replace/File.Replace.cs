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
   partial class FileTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void File_Replace_LocalAndNetwork_Success()
      {
         File_Replace(false);
         File_Replace(true);
      }

      
      private void File_Replace(bool isNetwork)
      {
         using (var tempRoot = new TemporaryDirectory(isNetwork))
         {
            var fileSrc = tempRoot.RandomTxtFileFullPath;
            var fileDst = tempRoot.RandomTxtFileFullPath;
            var fileBackup = tempRoot.RandomTxtFileFullPath + "-Backup.txt";
            var text = System.IO.Path.GetRandomFileName();

            Console.WriteLine("Src    File Path: [{0}]", fileSrc);
            Console.WriteLine("Dst    File Path: [{0}]", fileDst);
            Console.WriteLine("Backup File Path: [{0}]", fileBackup);
            Console.WriteLine("Text            : [{0}]", text);


            using (var stream = System.IO.File.CreateText(fileSrc))
               stream.Write(text);

            using (var stream = System.IO.File.CreateText(fileDst))
               stream.Write(text);


            Alphaleonis.Win32.Filesystem.File.Replace(fileSrc, fileDst, fileBackup);


            Assert.IsFalse(System.IO.File.Exists(fileSrc), "The file exists, but is expected not to.");

            Assert.IsTrue(System.IO.File.Exists(fileDst), "The file does not exist, but is expected to.");

            Assert.IsTrue(System.IO.File.Exists(fileBackup), "The file does not exist, but is expected to.");
            
            Assert.AreEqual(text, Alphaleonis.Win32.Filesystem.File.ReadAllText(fileDst), "The texts do not match, but are expected to.");

            Assert.AreEqual(text, Alphaleonis.Win32.Filesystem.File.ReadAllText(fileBackup), "The texts do not match, but are expected to.");
         }

         Console.WriteLine();
      }
   }
}
