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
   public partial class SizeTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void AlphaFS_Directory_Delete_ExistingDirectory_ReturnTotalBytes_LocalAndNetwork_Success()
      {
         AlphaFS_Directory_Delete_ExistingDirectory_ReturnTotalBytes(false);
         AlphaFS_Directory_Delete_ExistingDirectory_ReturnTotalBytes(true);
      }


      private void AlphaFS_Directory_Delete_ExistingDirectory_ReturnTotalBytes(bool isNetwork)
      {
         using (var tempRoot = new TemporaryDirectory(isNetwork))
         {
            var folder = tempRoot.CreateRecursiveTree(5);

            Console.WriteLine("Input Directory Path: [{0}]", folder.FullName);


            const Alphaleonis.Win32.Filesystem.DirectoryEnumerationOptions dirEnumOptions = Alphaleonis.Win32.Filesystem.DirectoryEnumerationOptions.FilesAndFolders | Alphaleonis.Win32.Filesystem.DirectoryEnumerationOptions.Recursive;

            var props = Alphaleonis.Win32.Filesystem.Directory.GetProperties(folder.FullName, dirEnumOptions);

            var fsoTotal = props["Total"];
            var filesTotal = props["File"];
            var foldersTotal = fsoTotal - filesTotal;
            var sizeTotal = props["Size"];

            Console.WriteLine("\n\tTotal size: [{0}]  Total Directories: [{1}]  Total Files: [{2}]", Alphaleonis.Utils.UnitSizeToText(sizeTotal), foldersTotal, filesTotal);


            var deleteArgs = new Alphaleonis.Win32.Filesystem.DeleteArguments {TargetPath = folder.FullName, Recursive = true, IgnoreReadOnly = true, GetSize = true};
            
            var deleteResult = Alphaleonis.Win32.Filesystem.Directory.Delete(deleteArgs);


            UnitTestConstants.Dump(deleteResult);


            Assert.AreEqual(System.IO.Directory.Exists(folder.FullName), Alphaleonis.Win32.Filesystem.Directory.Exists(folder.FullName));

            Assert.AreEqual(filesTotal, deleteResult.TotalFiles);

            Assert.AreEqual(foldersTotal + 1, deleteResult.TotalFolders);

            Assert.AreEqual(sizeTotal, deleteResult.TotalBytes);
         }

         Console.WriteLine();
      }
   }
}
