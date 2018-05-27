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

using System;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AlphaFS.UnitTest
{
   public partial class TimestampsTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void AlphaFS_Directory_SetTimestamps_LocalAndNetwork_Success()
      {
         ElevationAssert.IsElevated();

         AlphaFS_Directory_SetTimestamps(false);
         AlphaFS_Directory_SetTimestamps(true);
      }


      private void AlphaFS_Directory_SetTimestamps(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         var tempPath = System.IO.Path.GetTempPath();
         if (isNetwork)
            tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);

         var rnd = new Random();


         using (var rootDir = new TemporaryDirectory(tempPath, MethodBase.GetCurrentMethod().Name))
         {
            var folder = rootDir.RandomDirectoryFullPath;
            var symlinkPath = System.IO.Path.Combine(rootDir.Directory.FullName, UnitTestConstants.GetRandomFileNameWithDiacriticCharacters()) + "-symlink";

            Console.WriteLine("\nInput Directory Path: [{0}]", folder);


            System.IO.Directory.CreateDirectory(folder);
            Alphaleonis.Win32.Filesystem.Directory.CreateSymbolicLink(symlinkPath, folder);


            var creationTime = new DateTime(rnd.Next(1971, 2071), rnd.Next(1, 12), rnd.Next(1, 28), rnd.Next(0, 23), rnd.Next(0, 59), rnd.Next(0, 59));
            var lastAccessTime = new DateTime(rnd.Next(1971, 2071), rnd.Next(1, 12), rnd.Next(1, 28), rnd.Next(0, 23), rnd.Next(0, 59), rnd.Next(0, 59));
            var lastWriteTime = new DateTime(rnd.Next(1971, 2071), rnd.Next(1, 12), rnd.Next(1, 28), rnd.Next(0, 23), rnd.Next(0, 59), rnd.Next(0, 59));


            Alphaleonis.Win32.Filesystem.Directory.SetTimestamps(folder, creationTime, lastAccessTime, lastWriteTime);


            Assert.AreEqual(System.IO.Directory.GetCreationTime(folder), creationTime);
            Assert.AreEqual(System.IO.Directory.GetLastAccessTime(folder), lastAccessTime);
            Assert.AreEqual(System.IO.Directory.GetLastWriteTime(folder), lastWriteTime);


            // SymbolicLink
            Alphaleonis.Win32.Filesystem.Directory.SetTimestamps(symlinkPath, creationTime.AddDays(1), lastAccessTime.AddDays(1), lastWriteTime.AddDays(1), true, Alphaleonis.Win32.Filesystem.PathFormat.RelativePath);
            Assert.AreEqual(System.IO.Directory.GetCreationTime(symlinkPath), Alphaleonis.Win32.Filesystem.Directory.GetCreationTime(symlinkPath));
            Assert.AreEqual(System.IO.Directory.GetLastAccessTime(symlinkPath), Alphaleonis.Win32.Filesystem.Directory.GetLastAccessTime(symlinkPath));
            Assert.AreEqual(System.IO.Directory.GetLastWriteTime(symlinkPath), Alphaleonis.Win32.Filesystem.Directory.GetLastWriteTime(symlinkPath));


            creationTime = new DateTime(rnd.Next(1971, 2071), rnd.Next(1, 12), rnd.Next(1, 28), rnd.Next(0, 23), rnd.Next(0, 59), rnd.Next(0, 59));
            lastAccessTime = new DateTime(rnd.Next(1971, 2071), rnd.Next(1, 12), rnd.Next(1, 28), rnd.Next(0, 23), rnd.Next(0, 59), rnd.Next(0, 59));
            lastWriteTime = new DateTime(rnd.Next(1971, 2071), rnd.Next(1, 12), rnd.Next(1, 28), rnd.Next(0, 23), rnd.Next(0, 59), rnd.Next(0, 59));


            Alphaleonis.Win32.Filesystem.Directory.SetTimestampsUtc(folder, creationTime, lastAccessTime, lastWriteTime);


            Assert.AreEqual(System.IO.Directory.GetCreationTimeUtc(folder), creationTime);
            Assert.AreEqual(System.IO.Directory.GetLastAccessTimeUtc(folder), lastAccessTime);
            Assert.AreEqual(System.IO.Directory.GetLastWriteTimeUtc(folder), lastWriteTime);


            // SymbolicLink
            Alphaleonis.Win32.Filesystem.Directory.SetTimestampsUtc(symlinkPath, creationTime.AddDays(1), lastAccessTime.AddDays(1), lastWriteTime.AddDays(1), true, Alphaleonis.Win32.Filesystem.PathFormat.RelativePath);
            Assert.AreEqual(System.IO.Directory.GetCreationTimeUtc(symlinkPath), Alphaleonis.Win32.Filesystem.File.GetCreationTimeUtc(symlinkPath));
            Assert.AreEqual(System.IO.Directory.GetLastAccessTimeUtc(symlinkPath), Alphaleonis.Win32.Filesystem.File.GetLastAccessTimeUtc(symlinkPath));
            Assert.AreEqual(System.IO.Directory.GetLastWriteTimeUtc(symlinkPath), Alphaleonis.Win32.Filesystem.File.GetLastWriteTimeUtc(symlinkPath));
         }

         Console.WriteLine();
      }
   }
}
