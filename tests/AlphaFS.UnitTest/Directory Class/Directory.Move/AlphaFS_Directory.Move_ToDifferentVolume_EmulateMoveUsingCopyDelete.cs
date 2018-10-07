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
      public void AlphaFS_Directory_Move_ToDifferentVolume_EmulateMoveUsingCopyDelete_LocalAndNetwork_Success()
      {
         AlphaFS_Directory_Move_ToDifferentVolume_EmulateMoveUsingCopyDelete(false);
         AlphaFS_Directory_Move_ToDifferentVolume_EmulateMoveUsingCopyDelete(true);
      }


      private void AlphaFS_Directory_Move_ToDifferentVolume_EmulateMoveUsingCopyDelete(bool isNetwork)
      {
         // Do not pass isNetwork, as to always use UnitTestConstants.TempPath

         using (var tempRoot = new TemporaryDirectory())
         {
            var folderSrc = isNetwork ? tempRoot.CreateRecursiveTree(5, Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempRoot.RandomDirectoryFullPath)) : tempRoot.CreateRecursiveTree(5);

            var folderDst = !isNetwork ? new System.IO.DirectoryInfo(Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempRoot.RandomDirectoryFullPath)).FullName : tempRoot.RandomDirectoryFullPath;


            Console.WriteLine("Src Directory Path: [{0}]", folderSrc.FullName);
            Console.WriteLine("Dst Directory Path: [{0}]", folderDst);

            
            var dirEnumOptions = Alphaleonis.Win32.Filesystem.DirectoryEnumerationOptions.FilesAndFolders | Alphaleonis.Win32.Filesystem.DirectoryEnumerationOptions.Recursive;

            var props = Alphaleonis.Win32.Filesystem.Directory.GetProperties(folderSrc.FullName, dirEnumOptions);
            var sourceTotal = props["Total"];
            var sourceTotalFiles = props["File"];
            var sourceTotalSize = props["Size"];

            Console.WriteLine("\n\tTotal size: [{0}] - Total Folders: [{1}] - Files: [{2}]", Alphaleonis.Utils.UnitSizeToText(sourceTotalSize), sourceTotal - sourceTotalFiles, sourceTotalFiles);




            var moveResult = Alphaleonis.Win32.Filesystem.Directory.Move(folderSrc.FullName, folderDst, Alphaleonis.Win32.Filesystem.MoveOptions.CopyAllowed);

            UnitTestConstants.Dump(moveResult);

            props = Alphaleonis.Win32.Filesystem.Directory.GetProperties(folderDst, dirEnumOptions);
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
