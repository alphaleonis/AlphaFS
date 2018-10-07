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
      public void DirectoryInfo_MoveTo_FolderToEmptyFolder_LocalAndNetwork_Success()
      {
         DirectoryInfo_MoveTo_FolderToEmptyFolder(false);
         DirectoryInfo_MoveTo_FolderToEmptyFolder(true);
      }


      private void DirectoryInfo_MoveTo_FolderToEmptyFolder(bool isNetwork)
      {
         using (var tempRoot = new TemporaryDirectory(isNetwork))
         {
            var testfolder = tempRoot.RandomDirectoryName;

            var folderSrc = Alphaleonis.Win32.Filesystem.Directory.CreateDirectory(System.IO.Path.Combine(tempRoot.Directory.FullName, testfolder));
            var folderDst = tempRoot.CreateDirectory();
            
            Console.WriteLine("Input Directory Path: [{0}]", folderSrc.FullName);
            Console.WriteLine("\n\tMove folder to: [{0}]", folderDst.FullName);


            folderSrc.MoveTo(System.IO.Path.Combine(folderDst.FullName, testfolder));

            folderSrc.Refresh();
            folderDst.Refresh();
            

            Assert.IsTrue(folderSrc.Exists, "It is expected that the source exists, but is does not.");

            Assert.AreEqual(folderSrc.Parent.FullName, folderDst.FullName);
         }
         
         Console.WriteLine();
      }
   }
}
