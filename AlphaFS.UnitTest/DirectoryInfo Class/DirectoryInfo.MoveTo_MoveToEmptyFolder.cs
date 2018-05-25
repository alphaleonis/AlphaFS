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
using System.Reflection;

namespace AlphaFS.UnitTest
{
   public partial class DirectoryInfoTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void DirectoryInfo_MoveTo_MoveToEmptyFolder_LocalAndNetwork_Success()
      {
         DirectoryInfo_MoveTo_MoveToEmptyFolder(false);
         DirectoryInfo_MoveTo_MoveToEmptyFolder(true);
      }


      private void DirectoryInfo_MoveTo_MoveToEmptyFolder(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);
         Console.WriteLine();


         var tempPath = UnitTestConstants.TempFolder;
         if (isNetwork)
            tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);


         using (var rootDir = new TemporaryDirectory(tempPath, MethodBase.GetCurrentMethod().Name))
         {
            var testfolder = UnitTestConstants.GetRandomFileNameWithDiacriticCharacters();

            var folderSrc = Alphaleonis.Win32.Filesystem.Directory.CreateDirectory(System.IO.Path.Combine(rootDir.Directory.FullName, testfolder));
            var folderDst = System.IO.Directory.CreateDirectory(System.IO.Path.Combine(rootDir.Directory.FullName, "Existing Destination Folder"));


            Console.WriteLine("Input Directory Path: [{0}]", folderSrc.FullName);
            Console.WriteLine("\n\tMove folder to: [{0}]", folderDst.FullName);


            // Move folder.
            var newLocation = System.IO.Path.Combine(folderDst.FullName, testfolder);

            folderSrc.MoveTo(newLocation);


            folderSrc.Refresh();
            folderDst.Refresh();
            

            Assert.IsTrue(folderSrc.Exists, "It is expected that the source exists, but is does not.");
            Assert.AreEqual(folderSrc.Parent.FullName, folderDst.FullName);
         }


         Console.WriteLine();
      }
   }
}
