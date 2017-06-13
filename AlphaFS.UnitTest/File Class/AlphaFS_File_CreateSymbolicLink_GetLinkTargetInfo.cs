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
      public void AlphaFS_File_CreateSymbolicLinkAndGetLinkTargetInfo_LocalAndNetwork_Success()
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
            var folderLink = System.IO.Path.Combine(rootDir.Directory.FullName, "FolderLink-ToDestinationFolder");

            var dirInfo = new System.IO.DirectoryInfo(System.IO.Path.Combine(rootDir.Directory.FullName, "DestinationFolder"));
            dirInfo.Create();

            Console.WriteLine("\nInput Directory Path: [{0}]", dirInfo.FullName);
            Console.WriteLine("Folder Link         : [{0}]", folderLink);


            Alphaleonis.Win32.Filesystem.File.CreateSymbolicLink(folderLink, dirInfo.FullName, Alphaleonis.Win32.Filesystem.SymbolicLinkTarget.Directory);


            var lvi = Alphaleonis.Win32.Filesystem.File.GetLinkTargetInfo(folderLink);
            UnitTestConstants.Dump(lvi, -14);
            Assert.IsTrue(lvi.PrintName.Equals(dirInfo.FullName, StringComparison.OrdinalIgnoreCase));


            UnitTestConstants.Dump(new System.IO.DirectoryInfo(folderLink), -17);
            
            var alphaFSDirInfo = new Alphaleonis.Win32.Filesystem.DirectoryInfo(folderLink);
            UnitTestConstants.Dump(alphaFSDirInfo.EntryInfo, -17);

            Assert.AreEqual(System.IO.Directory.Exists(alphaFSDirInfo.FullName), alphaFSDirInfo.Exists);
            Assert.IsFalse(alphaFSDirInfo.EntryInfo.IsMountPoint);
            Assert.IsTrue(alphaFSDirInfo.EntryInfo.IsReparsePoint);
            Assert.IsTrue(alphaFSDirInfo.EntryInfo.IsSymbolicLink);
            Assert.AreEqual(alphaFSDirInfo.EntryInfo.ReparsePointTag, Alphaleonis.Win32.Filesystem.ReparsePointTag.SymLink);




            var fileLink = System.IO.Path.Combine(rootDir.Directory.FullName, "FileLink-ToDestinationFile.txt");

            var fileInfo = new System.IO.FileInfo(System.IO.Path.Combine(rootDir.Directory.FullName, "DestinationFile.txt"));
            fileInfo.CreateText();

            Console.WriteLine("\nInput File Path: [{0}]", fileInfo.FullName);
            Console.WriteLine("File Link      : [{0}]", fileLink);
            

            Alphaleonis.Win32.Filesystem.File.CreateSymbolicLink(fileLink, fileInfo.FullName, Alphaleonis.Win32.Filesystem.SymbolicLinkTarget.File);
            

            lvi = Alphaleonis.Win32.Filesystem.File.GetLinkTargetInfo(fileLink);
            UnitTestConstants.Dump(lvi, -14);
            Assert.IsTrue(lvi.PrintName.Equals(fileInfo.FullName, StringComparison.OrdinalIgnoreCase));


            UnitTestConstants.Dump(new System.IO.FileInfo(fileLink), -17);

            var alphaFSFileInfo = new Alphaleonis.Win32.Filesystem.FileInfo(fileLink);
            UnitTestConstants.Dump(alphaFSFileInfo.EntryInfo, -17);

            Assert.AreEqual(System.IO.File.Exists(alphaFSFileInfo.FullName), alphaFSFileInfo.Exists);
            Assert.IsFalse(alphaFSFileInfo.EntryInfo.IsMountPoint);
            Assert.IsTrue(alphaFSFileInfo.EntryInfo.IsReparsePoint);
            Assert.IsTrue(alphaFSFileInfo.EntryInfo.IsSymbolicLink);
            Assert.AreEqual(alphaFSFileInfo.EntryInfo.ReparsePointTag, Alphaleonis.Win32.Filesystem.ReparsePointTag.SymLink);
         }

         Console.WriteLine();
      }
   }
}
