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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;

namespace AlphaFS.UnitTest
{
   public partial class Directory_MoveTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void Directory_Move_ToDifferentVolume_EmulateUsingCopyDelete_LocalAndNetwork_Success()
      {
         Directory_Move_ToDifferentVolume_EmulateUsingCopyDelete(false);
         Directory_Move_ToDifferentVolume_EmulateUsingCopyDelete(true);
      }


      private void Directory_Move_ToDifferentVolume_EmulateUsingCopyDelete(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);
         Console.WriteLine();


         using (var rootDir = new TemporaryDirectory(UnitTestConstants.TempFolder, MethodBase.GetCurrentMethod().Name))
         {
            var random = UnitTestConstants.GetRandomFileNameWithDiacriticCharacters();
            var srcFolderName = System.IO.Path.Combine(rootDir.Directory.FullName, "Existing Source Folder.") + random;
            var destFolderName = System.IO.Path.Combine(rootDir.Directory.FullName, "Destination Folder.") + random;


            var folderSrc = isNetwork
               ? System.IO.Directory.CreateDirectory(Alphaleonis.Win32.Filesystem.Path.LocalToUnc(srcFolderName))
               : System.IO.Directory.CreateDirectory(System.IO.Path.Combine(srcFolderName));

            var folderDst = !isNetwork
               ? new System.IO.DirectoryInfo(Alphaleonis.Win32.Filesystem.Path.LocalToUnc(destFolderName))
               : new System.IO.DirectoryInfo(destFolderName);


            Console.WriteLine("\nSrc Directory Path: [{0}]", folderSrc.FullName);
            Console.WriteLine("Dst Directory Path: [{0}]", folderDst.FullName);

            UnitTestConstants.CreateDirectoriesAndFiles(folderSrc.FullName, new Random().Next(1, 3), false, false, true);


            var dirEnumOptions = Alphaleonis.Win32.Filesystem.DirectoryEnumerationOptions.FilesAndFolders | Alphaleonis.Win32.Filesystem.DirectoryEnumerationOptions.Recursive;

            var props = Alphaleonis.Win32.Filesystem.Directory.GetProperties(folderSrc.FullName, dirEnumOptions);
            var sourceTotal = props["Total"];
            var sourceTotalFiles = props["File"];
            var sourceTotalSize = props["Size"];

            Console.WriteLine("\n\tTotal size: [{0}] - Total Folders: [{1}] - Files: [{2}]", Alphaleonis.Utils.UnitSizeToText(sourceTotalSize), sourceTotal - sourceTotalFiles, sourceTotalFiles);




            var moveResult = Alphaleonis.Win32.Filesystem.Directory.Move(folderSrc.FullName, folderDst.FullName, Alphaleonis.Win32.Filesystem.MoveOptions.CopyAllowed);

            UnitTestConstants.Dump(moveResult, -16);

            props = Alphaleonis.Win32.Filesystem.Directory.GetProperties(folderDst.FullName, dirEnumOptions);
            Assert.AreEqual(sourceTotal, props["Total"], "The number of total file system objects do not match.");
            Assert.AreEqual(sourceTotalFiles, props["File"], "The number of total files do not match.");
            Assert.AreEqual(sourceTotalSize, props["Size"], "The total file size does not match.");


            // Test against moveResult results.

            var isMove = moveResult.IsMove;

            Assert.AreEqual(sourceTotal, moveResult.TotalFolders + moveResult.TotalFiles, "The number of total file system objects do not match.");
            Assert.AreEqual(sourceTotalFiles, moveResult.TotalFiles, "The number of total files do not match.");
            Assert.AreEqual(sourceTotalSize, moveResult.TotalBytes, "The total file size does not match.");
            Assert.IsFalse(isMove, "The action was expected to be a Copy, not a Move.");
            Assert.IsTrue(moveResult.IsEmulatedMove, "The action was expected to be emulated (Copy + Delete).");

            Assert.IsFalse(System.IO.Directory.Exists(folderSrc.FullName), "The original folder exists, but is expected not to.");
         }


         Console.WriteLine();
      }
   }
}
