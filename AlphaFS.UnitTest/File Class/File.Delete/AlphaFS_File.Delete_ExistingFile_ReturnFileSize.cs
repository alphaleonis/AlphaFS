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
   public partial class DeleteTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void AlphaFS_File_Delete_ExistingFile_ReturnFileSize_LocalAndNetwork_Success()
      {
         AlphaFS_File_Delete_ExistingFile_ReturnFileSize(false);
         AlphaFS_File_Delete_ExistingFile_ReturnFileSize(true);
      }


      private void AlphaFS_File_Delete_ExistingFile_ReturnFileSize(bool isNetwork)
      {
         using (var tempRoot = new TemporaryDirectory(isNetwork))
         {
            var file = tempRoot.CreateFile();

            var fileSize = new System.IO.FileInfo(file.FullName).Length;

            Console.WriteLine("Input File Path: [{0}] [{1}]", Alphaleonis.Utils.UnitSizeToText(fileSize), file.FullName);


            var deleteResult = Alphaleonis.Win32.Filesystem.File.Delete(new Alphaleonis.Win32.Filesystem.DeleteArguments(file.FullName) {GetSize = true});
            

            UnitTestConstants.Dump(deleteResult);
            
            Assert.AreEqual(System.IO.File.Exists(file.FullName), Alphaleonis.Win32.Filesystem.File.Exists(file.FullName));

            Assert.AreEqual(1, deleteResult.TotalFiles);

            Assert.AreEqual(0, deleteResult.TotalFolders);

            Assert.AreEqual(fileSize, deleteResult.TotalBytes);
         }

         Console.WriteLine();
      }
   }
}
