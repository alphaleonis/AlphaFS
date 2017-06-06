/*  Copyright (C) 2008-2017 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
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
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AlphaFS.UnitTest
{
   partial class FileTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>

      [TestMethod]
      public void AlphaFS_File_CreateSymbolicLink_And_GetLinkTargetInfo_LocalUNC_Success()
      {
         if (!UnitTestConstants.IsAdmin())
            Assert.Inconclusive();

         File_CreateSymbolicLink_GetLinkTargetInfo(false);
         File_CreateSymbolicLink_GetLinkTargetInfo(true);
      }


      private void File_CreateSymbolicLink_GetLinkTargetInfo(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);
         
         var tempPath = System.IO.Path.GetTempPath();
         if (isNetwork)
            tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);


         using (var rootDir = new TemporaryDirectory(tempPath, MethodBase.GetCurrentMethod().Name))
         {
            var symboliclinkFolder = System.IO.Path.Combine(rootDir.Directory.FullName, "DestinationFolder");
            System.IO.Directory.CreateDirectory(symboliclinkFolder);

            var fileName = "DestinationFile.txt";
            var file = System.IO.Path.Combine(symboliclinkFolder, fileName);
            using (System.IO.File.CreateText(file)) { }
            Assert.IsTrue(System.IO.File.Exists(file));




            var folderLink = System.IO.Path.Combine(rootDir.Directory.FullName, "FolderLink-ToDestinationFolder");
            Alphaleonis.Win32.Filesystem.File.CreateSymbolicLink(folderLink, symboliclinkFolder, Alphaleonis.Win32.Filesystem.SymbolicLinkTarget.Directory);
            Assert.IsTrue(System.IO.Directory.Exists(folderLink));
            Assert.IsTrue(System.IO.File.Exists(System.IO.Path.Combine(folderLink, fileName)));

            var lvi = Alphaleonis.Win32.Filesystem.File.GetLinkTargetInfo(folderLink);
            UnitTestConstants.Dump(lvi, -14);
            Assert.IsTrue(lvi.PrintName.Equals(symboliclinkFolder, StringComparison.OrdinalIgnoreCase));




            var fileLink = System.IO.Path.Combine(rootDir.Directory.FullName, "FileLink-ToDestinationFolder");
            file = System.IO.Path.Combine(symboliclinkFolder, fileName);
            Alphaleonis.Win32.Filesystem.File.CreateSymbolicLink(fileLink, file, Alphaleonis.Win32.Filesystem.SymbolicLinkTarget.File);
            Assert.IsTrue(System.IO.File.Exists(fileLink));

            lvi = Alphaleonis.Win32.Filesystem.File.GetLinkTargetInfo(fileLink);
            UnitTestConstants.Dump(lvi, -14);
            Assert.IsTrue(lvi.PrintName.Equals(file, StringComparison.OrdinalIgnoreCase));
         }

         Console.WriteLine();
      }
   }
}
