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
   public partial class AlphaFS_Directory_DeleteEmptySubdirectoriesTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void AlphaFS_Directory_DeleteEmptySubdirectories_LocalAndNetwork_Success()
      {
         AlphaFS_Directory_DeleteEmptySubdirectories(false);
         AlphaFS_Directory_DeleteEmptySubdirectories(true);
      }

      
      private void AlphaFS_Directory_DeleteEmptySubdirectories(bool isNetwork)
      {
         using (var tempRoot = new TemporaryDirectory(isNetwork))
         {
            const int maxDepth = 10;
            const int totalDirectories = maxDepth * maxDepth + maxDepth;            // maxDepth = 10: 110 directories and 110 files.
            const int emptyDirectories = maxDepth * maxDepth / 2;                   // 50 empty directories.
            const int remainingDirectories = totalDirectories - emptyDirectories;   // 60 remaining directories.
            
            var folder = tempRoot.CreateRecursiveTree(maxDepth);

            Console.WriteLine("Input Directory Path: [{0}]", folder.FullName);
            

            var searchPattern = Alphaleonis.Win32.Filesystem.Path.WildcardStarMatchAll;
            const Alphaleonis.Win32.Filesystem.DirectoryEnumerationOptions enumOptionsFolder = Alphaleonis.Win32.Filesystem.DirectoryEnumerationOptions.Folders | Alphaleonis.Win32.Filesystem.DirectoryEnumerationOptions.Recursive | Alphaleonis.Win32.Filesystem.DirectoryEnumerationOptions.ContinueOnException;
            const Alphaleonis.Win32.Filesystem.DirectoryEnumerationOptions enumOptionsFile = Alphaleonis.Win32.Filesystem.DirectoryEnumerationOptions.Files | Alphaleonis.Win32.Filesystem.DirectoryEnumerationOptions.Recursive | Alphaleonis.Win32.Filesystem.DirectoryEnumerationOptions.ContinueOnException;
            

            var dirs0 = Alphaleonis.Win32.Filesystem.Directory.CountFileSystemObjects(folder.FullName, searchPattern, enumOptionsFolder);
            var files0 = Alphaleonis.Win32.Filesystem.Directory.CountFileSystemObjects(folder.FullName, searchPattern, enumOptionsFile);
            Console.WriteLine("\n\tCounted Directories: [{0}]  Empty Directories: [{1}]", dirs0, emptyDirectories);
            Console.WriteLine("\tCounted Files      : [{0}]", files0);


            Alphaleonis.Win32.Filesystem.Directory.DeleteEmptySubdirectories(folder.FullName, true);


            Assert.IsTrue(System.IO.Directory.Exists(folder.FullName), "The root directory does not exist, but is expected to.");

            var dirs1 = Alphaleonis.Win32.Filesystem.Directory.CountFileSystemObjects(folder.FullName, searchPattern, enumOptionsFolder);
            var files1 = Alphaleonis.Win32.Filesystem.Directory.CountFileSystemObjects(folder.FullName, searchPattern, enumOptionsFile);
            Console.WriteLine("\n\tCounted Directories: [{0}]", dirs1);
            Console.WriteLine("\tCounted Files      : [{0}]", files1);

            Assert.AreNotEqual(dirs0, dirs1, "The number of directories are equal, but are expected not to.");
            Assert.AreEqual(remainingDirectories, dirs1, "The number of directories are not equal, but are expected to be.");
            Assert.AreEqual(files0, files1, "The number of files are not equal, but are expected to be.");
            Assert.AreEqual(totalDirectories, emptyDirectories + remainingDirectories, "The number of directories are not equal, but are expected to be.");
         }

         Console.WriteLine();
      }
   }
}
