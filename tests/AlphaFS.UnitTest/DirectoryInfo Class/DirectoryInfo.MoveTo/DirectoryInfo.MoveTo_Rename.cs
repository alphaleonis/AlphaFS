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
      public void DirectoryInfo_MoveTo_Rename_LocalAndNetwork_Success()
      {
         DirectoryInfo_MoveTo_Rename(false);
         DirectoryInfo_MoveTo_Rename(true);
      }


      private void DirectoryInfo_MoveTo_Rename(bool isNetwork)
      {
         using (var tempRoot = new TemporaryDirectory(isNetwork))
         {
            var newFolderName = "Rename_to_" + tempRoot.RandomDirectoryName;

            var dirInfoAlphaFS = new Alphaleonis.Win32.Filesystem.DirectoryInfo(tempRoot.RandomDirectoryFullPath);

            var dirInfoSystemIO = new System.IO.DirectoryInfo(System.IO.Path.Combine(tempRoot.Directory.FullName, newFolderName));

            Console.WriteLine("Input Directory Path: [{0}]", dirInfoAlphaFS.FullName);
            Console.WriteLine("\n\tRename folder to: [{0}]", dirInfoSystemIO.Name);


            // Create folder.
            dirInfoAlphaFS.Create();


            // Rename folder.
            dirInfoAlphaFS.MoveTo(dirInfoSystemIO.FullName);


            dirInfoAlphaFS.Refresh();
            dirInfoSystemIO.Refresh();


            Assert.IsTrue(dirInfoAlphaFS.Exists, "It is expected that the source exists, but is does not.");

            Assert.AreEqual(dirInfoAlphaFS.Parent.FullName, dirInfoSystemIO.Parent.FullName);
         }

         Console.WriteLine();
      }
   }
}
