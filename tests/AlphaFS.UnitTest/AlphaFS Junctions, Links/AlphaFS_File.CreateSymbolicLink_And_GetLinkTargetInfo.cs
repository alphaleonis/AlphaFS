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

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AlphaFS.UnitTest
{
   public partial class AlphaFS_JunctionsLinksTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>

      [TestMethod]
      public void AlphaFS_File_CreateSymbolicLink_And_GetLinkTargetInfo_LocalAndNetwork_Success()
      {
         UnitTestAssert.IsElevatedProcess();

         AlphaFS_File_CreateSymbolicLink_And_GetLinkTargetInfo(false);
         AlphaFS_File_CreateSymbolicLink_And_GetLinkTargetInfo(true);
      }


      private void AlphaFS_File_CreateSymbolicLink_And_GetLinkTargetInfo(bool isNetwork)
      {
         using (var tempRoot = new TemporaryDirectory(isNetwork))
         {
            var fileLink = System.IO.Path.Combine(tempRoot.Directory.FullName, "FileLink-ToOriginalFile.txt");

            var fileInfo = new System.IO.FileInfo(System.IO.Path.Combine(tempRoot.Directory.FullName, "OriginalFile.txt"));
            using (fileInfo.CreateText()) {}

            Console.WriteLine("Input File Path: [{0}]", fileInfo.FullName);
            Console.WriteLine("Input File Link: [{0}]", fileLink);
            

            Alphaleonis.Win32.Filesystem.File.CreateSymbolicLink(fileLink, fileInfo.FullName);
            

            var lvi = Alphaleonis.Win32.Filesystem.File.GetLinkTargetInfo(fileLink);
            UnitTestConstants.Dump(lvi);
            Assert.IsTrue(lvi.PrintName.Equals(fileInfo.FullName, StringComparison.OrdinalIgnoreCase));


            UnitTestConstants.Dump(new System.IO.FileInfo(fileLink));

            var alphaFSFileInfo = new Alphaleonis.Win32.Filesystem.FileInfo(fileLink);
            UnitTestConstants.Dump(alphaFSFileInfo.EntryInfo);

            Assert.AreEqual(System.IO.File.Exists(alphaFSFileInfo.FullName), alphaFSFileInfo.Exists);
            Assert.IsFalse(alphaFSFileInfo.EntryInfo.IsDirectory);
            Assert.IsFalse(alphaFSFileInfo.EntryInfo.IsMountPoint);
            Assert.IsTrue(alphaFSFileInfo.EntryInfo.IsReparsePoint);
            Assert.IsTrue(alphaFSFileInfo.EntryInfo.IsSymbolicLink);
            Assert.AreEqual(alphaFSFileInfo.EntryInfo.ReparsePointTag, Alphaleonis.Win32.Filesystem.ReparsePointTag.SymLink);
         }

         Console.WriteLine();
      }
   }
}
