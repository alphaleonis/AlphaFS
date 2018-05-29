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
   public partial class FileInfoTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void FileInfo_Refresh_LocalAndNetwork_Success()
      {
         FileInfo_Refresh(false);
         FileInfo_Refresh(true);
      }
      

      private void FileInfo_Refresh(bool isNetwork)
      {
         using (var tempRoot = new TemporaryDirectory(isNetwork))
         {
            var file = tempRoot.RandomTxtFileFullPath;
            var fiSysIo = new System.IO.FileInfo(file + "-System.IO.txt");
            var fiAlphaFS = new Alphaleonis.Win32.Filesystem.FileInfo(file + "-AlphaFS.txt");

            Console.WriteLine("System.IO Input File Path: [{0}]", fiSysIo.FullName);
            Console.WriteLine("AlphaFS   Input File Path: [{0}]", fiAlphaFS.FullName);


            var existsSysIo = fiSysIo.Exists;
            var exists = fiAlphaFS.Exists;
            Assert.AreEqual(existsSysIo, exists);
            Assert.IsFalse(exists, "The file exists, but is expected not to.");


            var fsSysIo = fiSysIo.Create();
            var fsAlphaFS = fiAlphaFS.Create();
            existsSysIo = fiSysIo.Exists;
            exists = fiAlphaFS.Exists;
            Assert.AreEqual(existsSysIo, exists);
            Assert.IsFalse(exists, "The file exists, but is expected not to.");


            fiSysIo.Refresh();
            fiAlphaFS.Refresh();
            existsSysIo = fiSysIo.Exists;
            exists = fiAlphaFS.Exists;
            Assert.AreEqual(existsSysIo, exists);
            Assert.IsTrue(exists, "The file does not exists, but is expected to.");


            fsSysIo.Close();
            fsAlphaFS.Close();
            fiSysIo.Delete();
            fiAlphaFS.Delete();
            existsSysIo = fiSysIo.Exists;
            exists = fiAlphaFS.Exists;
            Assert.AreEqual(existsSysIo, exists);
            Assert.IsTrue(exists, "The file does not exists, but is expected to.");


            fiSysIo.Refresh();
            fiAlphaFS.Refresh();
            existsSysIo = fiSysIo.Exists;
            exists = fiAlphaFS.Exists;
            Assert.AreEqual(existsSysIo, exists);
            Assert.IsFalse(exists, "The file exists, but is expected not to.");
         }

         Console.WriteLine();
      }
   }
}
