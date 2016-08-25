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
   partial class DirectoryTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>

      [TestMethod]
      public void AlphaFS_Directory_Compress_LocalAndUNC_Success()
      {
         Directory_Compress(false, false);
         Directory_Compress(true, false);
      }


      [TestMethod]
      public void AlphaFS_Directory_Compress_Recursive_LocalAndUNC_Success()
      {
         Directory_Compress(false, true);
         Directory_Compress(true, true);
      }




      private void Directory_Compress(bool isNetwork, bool recursive)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         string tempPath = System.IO.Path.GetTempPath();
         if (isNetwork)
            tempPath = PathUtils.AsUncPath(tempPath);


         using (var rootDir = new TemporaryDirectory(tempPath, "Directory.Compress"))
         {
            string rootFolder = rootDir.RandomFileFullPath;
            Console.WriteLine("\nInput Directory Path: [{0}]\n", rootFolder);

            string folderAaa = System.IO.Path.Combine(rootFolder, "folder-aaa");
            string folderBbb = System.IO.Path.Combine(rootFolder, "folder-bbb");
            string folderCcc = System.IO.Path.Combine(folderBbb, "folder-ccc");
            System.IO.Directory.CreateDirectory(folderAaa);
            System.IO.Directory.CreateDirectory(folderBbb);
            System.IO.Directory.CreateDirectory(folderCcc);


            var fileAaa = System.IO.Path.Combine(folderAaa, "file-aaa.txt");
            var fileBbb = System.IO.Path.Combine(folderAaa, "file-bbb.txt");
            var fileCcc = System.IO.Path.Combine(rootFolder, "file-ccc.txt");
            var fileDdd = System.IO.Path.Combine(rootFolder, "file-ddd.txt");
            using (System.IO.File.CreateText(fileAaa)) { }
            using (System.IO.File.CreateText(fileBbb)) { }
            using (System.IO.File.CreateText(fileCcc)) { }
            using (System.IO.File.CreateText(fileDdd)) { }


            Alphaleonis.Win32.Filesystem.Directory.Compress(rootFolder, recursive ? Alphaleonis.Win32.Filesystem.DirectoryEnumerationOptions.Recursive : Alphaleonis.Win32.Filesystem.DirectoryEnumerationOptions.None);


            DirectoryAssert.IsCompressed(rootFolder);
            DirectoryAssert.IsCompressed(folderAaa);
            DirectoryAssert.IsCompressed(folderBbb);
            FileAssert.IsCompressed(fileCcc);
            FileAssert.IsCompressed(fileDdd);


            if (recursive)
            {
               DirectoryAssert.IsCompressed(folderCcc);
               FileAssert.IsCompressed(fileAaa);
               FileAssert.IsCompressed(fileBbb);
            }
            else
            {
               DirectoryAssert.IsNotCompressed(folderCcc);
               FileAssert.IsNotCompressed(fileAaa);
               FileAssert.IsNotCompressed(fileBbb);
            }
         }

         Console.WriteLine();
      }
   }
}
