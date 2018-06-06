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
   public partial class MoveTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void File_Move_ExistingFile_LocalAndNetwork_Success()
      {
         File_Move_ExistingFile(false);
         File_Move_ExistingFile(true);
      }


      private void File_Move_ExistingFile(bool isNetwork)
      {
         using (var tempRoot = new TemporaryDirectory(isNetwork))
         {
            var srcFile = tempRoot.CreateFile();

            var dstFile = System.IO.Path.Combine(tempRoot.Directory.FullName, srcFile.Name + "-Moved.File");

            var fileLength = srcFile.Length;

            Console.WriteLine("Src File Path: [{0}] [{1}]", Alphaleonis.Utils.UnitSizeToText(srcFile.Length), srcFile.FullName);
            Console.WriteLine("Dst File Path: [{0}]", dstFile);


            var moveResult = Alphaleonis.Win32.Filesystem.File.Move(srcFile.FullName, dstFile, Alphaleonis.Win32.Filesystem.PathFormat.FullPath);

            UnitTestConstants.Dump(moveResult);


            // Test against moveResult results.

            Assert.IsFalse(moveResult.IsCopy);
            Assert.IsTrue(moveResult.IsMove);
            Assert.IsFalse(moveResult.IsDirectory);
            Assert.IsTrue(moveResult.IsFile);

            Assert.AreEqual(fileLength, moveResult.TotalBytes);

            Assert.AreEqual(1, moveResult.TotalFiles);
            Assert.AreEqual(0, moveResult.TotalFolders);


            Assert.IsFalse(System.IO.File.Exists(srcFile.FullName), "The file does exists, but is expected not to.");
            Assert.IsTrue(System.IO.File.Exists(dstFile), "The file does not exists, but is expected to.");

            var fileLen = new System.IO.FileInfo(dstFile).Length;
            Assert.AreEqual(fileLength, fileLen, "The file copy is: {0} bytes, but is expected to be: {1} bytes.", fileLen, fileLength);


            Assert.IsFalse(System.IO.File.Exists(srcFile.FullName), "The original file exists, but is expected not to.");
         }


         Console.WriteLine();
      }
   }
}
