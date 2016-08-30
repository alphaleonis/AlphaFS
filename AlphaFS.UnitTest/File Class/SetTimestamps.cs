/*  Copyright (C) 2008-2015 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
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
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AlphaFS.UnitTest
{
   partial class FileTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>
      
      [TestMethod]
      public void AlphaFS_File_SetTimestampsXxx_LocalAndUNC_Success()
      {
         File_SetTimestampsXxx(false);
         File_SetTimestampsXxx(true);
      }


      [TestMethod]
      public void AlphaFS_File_TransferTimestamps_LocalAndUNC_Success()
      {
         File_TransferTimestamps(false);
         File_TransferTimestamps(true);
      }




      private void File_SetTimestampsXxx(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         string tempPath = System.IO.Path.GetTempPath();
         if (isNetwork)
            tempPath = PathUtils.AsUncPath(tempPath);

         var rnd = new Random();


         using (var rootDir = new TemporaryDirectory(tempPath, "File.SetTimestampsXxx"))
         {
            string file = rootDir.RandomFileFullPath + ".txt";
            string symlinkPath = System.IO.Path.Combine(rootDir.Directory.FullName, System.IO.Path.GetRandomFileName()) + "-symlink";

            Console.WriteLine("\nInput File Path: [{0}]", file);


            using (System.IO.File.Create(file)) { }
            Alphaleonis.Win32.Filesystem.File.CreateSymbolicLink(symlinkPath, file, Alphaleonis.Win32.Filesystem.SymbolicLinkTarget.File);


            DateTime creationTime = new DateTime(rnd.Next(1971, 2071), rnd.Next(1, 12), rnd.Next(1, 28), rnd.Next(0, 23), rnd.Next(0, 59), rnd.Next(0, 59));
            DateTime lastAccessTime = new DateTime(rnd.Next(1971, 2071), rnd.Next(1, 12), rnd.Next(1, 28), rnd.Next(0, 23), rnd.Next(0, 59), rnd.Next(0, 59));
            DateTime lastWriteTime = new DateTime(rnd.Next(1971, 2071), rnd.Next(1, 12), rnd.Next(1, 28), rnd.Next(0, 23), rnd.Next(0, 59), rnd.Next(0, 59));


            Alphaleonis.Win32.Filesystem.File.SetTimestamps(file, creationTime, lastAccessTime, lastWriteTime);


            Assert.AreEqual(System.IO.File.GetCreationTime(file), creationTime);
            Assert.AreEqual(System.IO.File.GetLastAccessTime(file), lastAccessTime);
            Assert.AreEqual(System.IO.File.GetLastWriteTime(file), lastWriteTime);


            // SymbolicLink
            Alphaleonis.Win32.Filesystem.File.SetTimestamps(symlinkPath, creationTime.AddDays(1), lastAccessTime.AddDays(1), lastWriteTime.AddDays(1), true, Alphaleonis.Win32.Filesystem.PathFormat.RelativePath);
            Assert.AreEqual(System.IO.File.GetCreationTime(symlinkPath), Alphaleonis.Win32.Filesystem.File.GetCreationTime(symlinkPath));
            Assert.AreEqual(System.IO.File.GetLastAccessTime(symlinkPath), Alphaleonis.Win32.Filesystem.File.GetLastAccessTime(symlinkPath));
            Assert.AreEqual(System.IO.File.GetLastWriteTime(symlinkPath), Alphaleonis.Win32.Filesystem.File.GetLastWriteTime(symlinkPath));


            creationTime = new DateTime(rnd.Next(1971, 2071), rnd.Next(1, 12), rnd.Next(1, 28), rnd.Next(0, 23), rnd.Next(0, 59), rnd.Next(0, 59));
            lastAccessTime = new DateTime(rnd.Next(1971, 2071), rnd.Next(1, 12), rnd.Next(1, 28), rnd.Next(0, 23), rnd.Next(0, 59), rnd.Next(0, 59));
            lastWriteTime = new DateTime(rnd.Next(1971, 2071), rnd.Next(1, 12), rnd.Next(1, 28), rnd.Next(0, 23), rnd.Next(0, 59), rnd.Next(0, 59));


            Alphaleonis.Win32.Filesystem.File.SetTimestampsUtc(file, creationTime, lastAccessTime, lastWriteTime);


            Assert.AreEqual(System.IO.File.GetCreationTimeUtc(file), creationTime);
            Assert.AreEqual(System.IO.File.GetLastAccessTimeUtc(file), lastAccessTime);
            Assert.AreEqual(System.IO.File.GetLastWriteTimeUtc(file), lastWriteTime);


            // SymbolicLink
            Alphaleonis.Win32.Filesystem.File.SetTimestampsUtc(symlinkPath, creationTime.AddDays(1), lastAccessTime.AddDays(1), lastWriteTime.AddDays(1), true, Alphaleonis.Win32.Filesystem.PathFormat.RelativePath);
            Assert.AreEqual(System.IO.File.GetCreationTimeUtc(symlinkPath), Alphaleonis.Win32.Filesystem.File.GetCreationTimeUtc(symlinkPath));
            Assert.AreEqual(System.IO.File.GetLastAccessTimeUtc(symlinkPath), Alphaleonis.Win32.Filesystem.File.GetLastAccessTimeUtc(symlinkPath));
            Assert.AreEqual(System.IO.File.GetLastWriteTimeUtc(symlinkPath), Alphaleonis.Win32.Filesystem.File.GetLastWriteTimeUtc(symlinkPath));
         }

         Console.WriteLine();
      }


      private void File_TransferTimestamps(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         string tempPath = System.IO.Path.GetTempPath();
         if (isNetwork)
            tempPath = PathUtils.AsUncPath(tempPath);


         using (var rootDir = new TemporaryDirectory(tempPath, "File.TransferTimestamps"))
         {
            string file = rootDir.RandomFileFullPath;
            string file2 = rootDir.RandomFileFullPath;
            if (isNetwork)
            {
               file = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(file);
               file2 = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(file2);
            }

            using (System.IO.File.Create(file)) {}
            Thread.Sleep(1500);
            using (System.IO.File.Create(file2)) { }


            Console.WriteLine("\nInput File1 Path: [{0}]", file);
            Console.WriteLine("\nInput File2 Path: [{0}]", file2);


            Assert.AreNotEqual(System.IO.File.GetCreationTime(file), System.IO.File.GetCreationTime(file2));
            Assert.AreNotEqual(System.IO.File.GetLastAccessTime(file), System.IO.File.GetLastAccessTime(file2));
            Assert.AreNotEqual(System.IO.File.GetLastWriteTime(file), System.IO.File.GetLastWriteTime(file2));

            Alphaleonis.Win32.Filesystem.File.TransferTimestamps(file, file2);

            Assert.AreEqual(System.IO.File.GetCreationTime(file), System.IO.File.GetCreationTime(file2));
            Assert.AreEqual(System.IO.File.GetLastAccessTime(file), System.IO.File.GetLastAccessTime(file2));
            Assert.AreEqual(System.IO.File.GetLastWriteTime(file), System.IO.File.GetLastWriteTime(file2));
         }

         Console.WriteLine();
      }
   }
}
