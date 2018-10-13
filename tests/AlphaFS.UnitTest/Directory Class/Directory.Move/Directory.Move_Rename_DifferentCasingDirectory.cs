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
      public void Directory_Move_Rename_DifferentCasingDirectory_LocalAndNetwork_Success()
      {
         Directory_Move_Rename_DifferentCasingDirectory(false);
         Directory_Move_Rename_DifferentCasingDirectory(true);
      }


      private void Directory_Move_Rename_DifferentCasingDirectory(bool isNetwork)
      {
         using (var tempRoot = new TemporaryDirectory(isNetwork))
         {
            var folderSrc = System.IO.Directory.CreateDirectory(System.IO.Path.Combine(tempRoot.Directory.FullName, "Existing Source Folder"));

            var upperCaseFolderName = System.IO.Path.GetFileName(folderSrc.FullName).ToUpperInvariant();

            var destFolder = System.IO.Path.Combine(folderSrc.Parent.FullName, upperCaseFolderName);

            Console.WriteLine("Input Directory Path: [{0}]\n", folderSrc.FullName);
            Console.WriteLine("\tRename folder [{0}] to [{1}]", folderSrc.Name, upperCaseFolderName);


            Alphaleonis.Win32.Filesystem.Directory.Move(folderSrc.FullName, destFolder);


            var upperDirInfo = new System.IO.DirectoryInfo(destFolder);

            UnitTestConstants.Dump(upperDirInfo);
            

            Assert.AreEqual(upperCaseFolderName, upperDirInfo.Name, "The source folder name is not uppercase, but is expected to.");
         }


         Console.WriteLine();
      }
   }
}
