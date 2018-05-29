/*  Copyright (C) 2008-2018 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
 *  
 *  Permission is hereby granted, free of charge, to any person obtaining a copy 
 *  of this software and associated documentation directorys (the "Software"), to deal 
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
   public partial class DirectoryInfoTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void DirectoryInfo_Refresh_LocalAndNetwork_Success()
      {
         DirectoryInfo_Refresh(false);
         DirectoryInfo_Refresh(true);
      }
      

      private void DirectoryInfo_Refresh(bool isNetwork)
      {
         using (var tempRoot = new TemporaryDirectory(isNetwork))
         {
            var folder = tempRoot.RandomDirectoryFullPath;
            var diSysIo = new System.IO.DirectoryInfo(folder + "-System.IO");
            var diAlphaFS = new Alphaleonis.Win32.Filesystem.DirectoryInfo(folder + "-AlphaFS");

            Console.WriteLine("System.IO Input Directory Path: [{0}]", diSysIo.FullName);
            Console.WriteLine("AlphaFS   Input Directory Path: [{0}]", diAlphaFS.FullName);


            var existsSysIo = diSysIo.Exists;
            var exists = diAlphaFS.Exists;
            Assert.AreEqual(existsSysIo, exists);
            Assert.IsFalse(exists, "The directory exists, but is expected not to.");


            diSysIo.Create();
            diAlphaFS.Create();
            existsSysIo = diSysIo.Exists;
            exists = diAlphaFS.Exists;
            Assert.AreEqual(existsSysIo, exists);
            Assert.IsFalse(exists, "The directory exists, but is expected not to.");


            diSysIo.Refresh();
            diAlphaFS.Refresh();
            existsSysIo = diSysIo.Exists;
            exists = diAlphaFS.Exists;
            Assert.AreEqual(existsSysIo, exists);
            Assert.IsTrue(exists, "The directory does not exists, but is expected to.");


            diSysIo.Delete();
            diAlphaFS.Delete();
            existsSysIo = diSysIo.Exists;
            exists = diAlphaFS.Exists;
            Assert.AreEqual(existsSysIo, exists);
            Assert.IsTrue(exists, "The directory does not exists, but is expected to.");


            diSysIo.Refresh();
            diAlphaFS.Refresh();
            existsSysIo = diSysIo.Exists;
            exists = diAlphaFS.Exists;
            Assert.AreEqual(existsSysIo, exists);
            Assert.IsFalse(exists, "The directory exists, but is expected not to.");
         }

         Console.WriteLine();
      }
   }
}
