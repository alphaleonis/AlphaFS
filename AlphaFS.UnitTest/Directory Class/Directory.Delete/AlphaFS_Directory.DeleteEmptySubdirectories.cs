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
   public partial class Directory_DeleteTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void AlphaFS_Directory_DeleteEmptySubdirectories_LocalAndNetwork_Success()
      {
         Directory_DeleteEmptySubdirectories(false);
         Directory_DeleteEmptySubdirectories(true);

         //System.IO.Directory.CreateDirectory(@"c:\temp2\test\Foo");
         //System.IO.Directory.CreateDirectory(@"c:\temp2\test\Bar");
         //System.IO.Directory.CreateDirectory(@"c:\temp2\test\Foo\FooFoo");
         //System.IO.Directory.CreateDirectory(@"c:\temp2\test\Foo\FooBar");

         //Alphaleonis.Win32.Filesystem.Directory.DeleteEmptySubdirectories(@"c:\temp2\test", true);
      }




      private void Directory_DeleteEmptySubdirectories(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);
         Console.WriteLine();


         var tempPath = UnitTestConstants.TempFolder;
         if (isNetwork)
            tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);


         using (var rootDir = new TemporaryDirectory(tempPath, MethodBase.GetCurrentMethod().Name))
         {
            var folder = System.IO.Directory.CreateDirectory(System.IO.Path.Combine(rootDir.Directory.FullName, "Source Folder"));

            Console.WriteLine("Input Directory Path: [{0}]", folder.FullName);


            const int maxDepth = 10;
            const int totalDirectories = maxDepth * maxDepth + maxDepth;            // maxDepth = 10: 110 directories and 110 files.
            const int emptyDirectories = maxDepth * maxDepth / 2;                   // 50 empty directories.
            const int remainingDirectories = totalDirectories - emptyDirectories;   // 60 remaining directories.

            var searchPattern = Alphaleonis.Win32.Filesystem.Path.WildcardStarMatchAll;
            const Alphaleonis.Win32.Filesystem.DirectoryEnumerationOptions enumOptionsFolder = Alphaleonis.Win32.Filesystem.DirectoryEnumerationOptions.Folders | Alphaleonis.Win32.Filesystem.DirectoryEnumerationOptions.Recursive | Alphaleonis.Win32.Filesystem.DirectoryEnumerationOptions.ContinueOnException;
            const Alphaleonis.Win32.Filesystem.DirectoryEnumerationOptions enumOptionsFile = Alphaleonis.Win32.Filesystem.DirectoryEnumerationOptions.Files | Alphaleonis.Win32.Filesystem.DirectoryEnumerationOptions.Recursive | Alphaleonis.Win32.Filesystem.DirectoryEnumerationOptions.ContinueOnException;


            UnitTestConstants.CreateDirectoriesAndFiles(folder.FullName, maxDepth, false, false, true);

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
