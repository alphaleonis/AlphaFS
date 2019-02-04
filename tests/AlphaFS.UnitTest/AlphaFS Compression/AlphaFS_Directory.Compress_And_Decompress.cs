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
   partial class AlphaFS_CompressionTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void AlphaFS_Directory_Compress_And_Decompress_LocalAndNetwork_Success()
      {
         AlphaFS_Directory_Compress_And_Decompress(false, false);
         AlphaFS_Directory_Compress_And_Decompress(true, false);
      }


      private void AlphaFS_Directory_Compress_And_Decompress(bool isNetwork, bool recursive)
      {
         using (var tempRoot = new TemporaryDirectory(isNetwork))
         {
            var folder = tempRoot.RandomDirectoryFullPath;

            Console.WriteLine("Input Directory Path: [{0}]", folder);


            var folderAaa = System.IO.Path.Combine(folder, "folder-aaa");
            var folderBbb = System.IO.Path.Combine(folder, "folder-bbb");
            var folderCcc = System.IO.Path.Combine(folderBbb, "folder-ccc");
            System.IO.Directory.CreateDirectory(folderAaa);
            System.IO.Directory.CreateDirectory(folderBbb);
            System.IO.Directory.CreateDirectory(folderCcc);


            var fileAaa = System.IO.Path.Combine(folderAaa, "file-aaa.txt");
            var fileBbb = System.IO.Path.Combine(folderAaa, "file-bbb.txt");
            var fileCcc = System.IO.Path.Combine(folder, "file-ccc.txt");
            var fileDdd = System.IO.Path.Combine(folder, "file-ddd.txt");
            using (System.IO.File.CreateText(fileAaa)) { }
            using (System.IO.File.CreateText(fileBbb)) { }
            using (System.IO.File.CreateText(fileCcc)) { }
            using (System.IO.File.CreateText(fileDdd)) { }


            Alphaleonis.Win32.Filesystem.Directory.Compress(folder, recursive ? Alphaleonis.Win32.Filesystem.DirectoryEnumerationOptions.Recursive : Alphaleonis.Win32.Filesystem.DirectoryEnumerationOptions.None);


            DirectoryAssert.IsCompressed(folder);
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


            Alphaleonis.Win32.Filesystem.Directory.Decompress(folder, recursive ? Alphaleonis.Win32.Filesystem.DirectoryEnumerationOptions.Recursive : Alphaleonis.Win32.Filesystem.DirectoryEnumerationOptions.None);

            
            DirectoryAssert.IsNotCompressed(folderCcc);
            FileAssert.IsNotCompressed(fileAaa);
            FileAssert.IsNotCompressed(fileBbb);           
         }

         Console.WriteLine();
      }
   }
}
