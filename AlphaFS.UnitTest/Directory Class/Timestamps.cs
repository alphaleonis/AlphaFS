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
   partial class DirectoryTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>
      
      [TestMethod]
      public void Directory_GetXxxTimeXxx_LocalAndUNC_Success()
      {
         Directory_GetXxxTimeXxx(false);
         Directory_GetXxxTimeXxx(true);
      }


      [TestMethod]
      public void Directory_GetXxxTimeXxx_NonExistingDirectory_Success()
      {
         const string Path = @"z:\nonExistingPath\nonExistingSubFolder";
         Assert.IsFalse(System.IO.Directory.Exists(Path));

         var newDateTime = new DateTime(1601, 1, 1);
         var newDateTimeLocaltime = new DateTime(1601, 1, 1).ToLocalTime();


         Assert.AreEqual(newDateTimeLocaltime, System.IO.Directory.GetCreationTime(Path));
         Assert.AreEqual(newDateTimeLocaltime, Alphaleonis.Win32.Filesystem.Directory.GetCreationTime(Path));
         
         Assert.AreEqual(newDateTime, System.IO.Directory.GetCreationTimeUtc(Path));
         Assert.AreEqual(newDateTime, Alphaleonis.Win32.Filesystem.Directory.GetCreationTimeUtc(Path));


         Assert.AreEqual(newDateTimeLocaltime, System.IO.Directory.GetLastAccessTime(Path));
         Assert.AreEqual(newDateTimeLocaltime, Alphaleonis.Win32.Filesystem.Directory.GetLastAccessTime(Path));

         Assert.AreEqual(newDateTime, System.IO.Directory.GetLastAccessTimeUtc(Path));
         Assert.AreEqual(newDateTime, Alphaleonis.Win32.Filesystem.Directory.GetLastAccessTimeUtc(Path));


         Assert.AreEqual(newDateTimeLocaltime, System.IO.Directory.GetLastWriteTime(Path));
         Assert.AreEqual(newDateTimeLocaltime, Alphaleonis.Win32.Filesystem.Directory.GetLastWriteTime(Path));

         Assert.AreEqual(newDateTime, System.IO.Directory.GetLastWriteTimeUtc(Path));
         Assert.AreEqual(newDateTime, Alphaleonis.Win32.Filesystem.Directory.GetLastWriteTimeUtc(Path));
      }


      [TestMethod]
      public void AlphaFS_Directory_SetTimestampsXxx_LocalAndUNC_Success()
      {
         Directory_SetTimestampsXxx(false);
         Directory_SetTimestampsXxx(true);
      }


      [TestMethod]
      public void AlphaFS_Directory_TransferTimestamps_LocalAndUNC_Success()
      {
         Directory_TransferTimestamps(false);
         Directory_TransferTimestamps(true);
      }




      private void Directory_GetXxxTimeXxx(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         string tempPath = System.IO.Path.GetTempPath();
         if (isNetwork)
            tempPath = PathUtils.AsUncPath(tempPath);


         using (var rootDir = new TemporaryDirectory(tempPath, "Directory.GetXxxTimeXxx"))
         {
            string folder = isNetwork ? Alphaleonis.Win32.Filesystem.Path.LocalToUnc(UnitTestConstants.SysRoot32) : UnitTestConstants.SysRoot32;

            Console.WriteLine("\nInput Directory Path: [{0}]\n", folder);


            Assert.AreEqual(System.IO.Directory.GetCreationTime(folder), Alphaleonis.Win32.Filesystem.Directory.GetCreationTime(folder));
            Assert.AreEqual(System.IO.Directory.GetCreationTimeUtc(folder), Alphaleonis.Win32.Filesystem.Directory.GetCreationTimeUtc(folder));
            Assert.AreEqual(System.IO.Directory.GetLastAccessTime(folder), Alphaleonis.Win32.Filesystem.Directory.GetLastAccessTime(folder));
            Assert.AreEqual(System.IO.Directory.GetLastAccessTimeUtc(folder), Alphaleonis.Win32.Filesystem.Directory.GetLastAccessTimeUtc(folder));
            Assert.AreEqual(System.IO.Directory.GetLastWriteTime(folder), Alphaleonis.Win32.Filesystem.Directory.GetLastWriteTime(folder));
            Assert.AreEqual(System.IO.Directory.GetLastWriteTimeUtc(folder), Alphaleonis.Win32.Filesystem.Directory.GetLastWriteTimeUtc(folder));


            // We can not compare ChangeTime against .NET because it does not exist.
            // Creating a directory and renaming it triggers ChangeTime, so test for that.

            folder = rootDir.RandomFileFullPath;
            if (isNetwork) folder = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(folder);
            Console.WriteLine("Input Directory Path: [{0}]\n", folder);

            var dirInfo = new System.IO.DirectoryInfo(folder);
            dirInfo.Create();
            string fileName = dirInfo.Name;


            DateTime changeTimeActual = Alphaleonis.Win32.Filesystem.Directory.GetChangeTime(folder);
            DateTime changeTimeUtcActual = Alphaleonis.Win32.Filesystem.Directory.GetChangeTimeUtc(folder);


            // Sleep for three seconds.
            var delay = 3;

            dirInfo.MoveTo(dirInfo.FullName.Replace(fileName, fileName + "-Renamed"));
            Thread.Sleep(delay * 1000);
            dirInfo.MoveTo(dirInfo.FullName.Replace(fileName + "-Renamed", fileName));


            var newChangeTime = changeTimeActual.AddSeconds(3);
            Assert.AreEqual(changeTimeActual.AddSeconds(3), newChangeTime);

            newChangeTime = changeTimeUtcActual.AddSeconds(3);
            Assert.AreEqual(changeTimeUtcActual.AddSeconds(3), newChangeTime);
         }
      }


      private void Directory_SetTimestampsXxx(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         string tempPath = System.IO.Path.GetTempPath();
         if (isNetwork)
            tempPath = PathUtils.AsUncPath(tempPath);

         var rnd = new Random();


         using (var rootDir = new TemporaryDirectory(tempPath, "Directory.SetTimestampsXxx"))
         {
            string folder = rootDir.RandomFileFullPath;
            string symlinkPath = System.IO.Path.Combine(rootDir.Directory.FullName, System.IO.Path.GetRandomFileName()) + "-symlink";

            string folder2 = rootDir.RandomFileFullPath;
            System.IO.Directory.CreateDirectory(folder2);
            if (isNetwork)
               folder2 = PathUtils.AsUncPath(folder2);


            Console.WriteLine("\nInput Directory Path: [{0}]", folder);


            System.IO.Directory.CreateDirectory(folder);
            Alphaleonis.Win32.Filesystem.File.CreateSymbolicLink(symlinkPath, folder, Alphaleonis.Win32.Filesystem.SymbolicLinkTarget.Directory);


            DateTime creationTime = new DateTime(rnd.Next(1971, 2071), rnd.Next(1, 12), rnd.Next(1, 28), rnd.Next(0, 23), rnd.Next(0, 59), rnd.Next(0, 59));
            DateTime lastAccessTime = new DateTime(rnd.Next(1971, 2071), rnd.Next(1, 12), rnd.Next(1, 28), rnd.Next(0, 23), rnd.Next(0, 59), rnd.Next(0, 59));
            DateTime lastWriteTime = new DateTime(rnd.Next(1971, 2071), rnd.Next(1, 12), rnd.Next(1, 28), rnd.Next(0, 23), rnd.Next(0, 59), rnd.Next(0, 59));


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


      private void Directory_TransferTimestamps(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         string tempPath = System.IO.Path.GetTempPath();
         if (isNetwork)
            tempPath = PathUtils.AsUncPath(tempPath);


         using (var rootDir = new TemporaryDirectory(tempPath, "Directory.TransferTimestamps"))
         {
            string folder = rootDir.RandomFileFullPath;
            string folder2 = rootDir.RandomFileFullPath;
            if (isNetwork)
            {
               folder = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(folder);
               folder2 = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(folder2);
            }

            System.IO.Directory.CreateDirectory(folder);
            Thread.Sleep(1500);
            System.IO.Directory.CreateDirectory(folder2);


            Console.WriteLine("\nInput Directory Path: [{0}]", folder);
            Console.WriteLine("\nInput Directory2 Path: [{0}]", folder2);


            Assert.AreNotEqual(System.IO.Directory.GetCreationTime(folder), System.IO.Directory.GetCreationTime(folder2));
            Assert.AreNotEqual(System.IO.Directory.GetLastAccessTime(folder), System.IO.Directory.GetLastAccessTime(folder2));
            Assert.AreNotEqual(System.IO.Directory.GetLastWriteTime(folder), System.IO.Directory.GetLastWriteTime(folder2));

            Alphaleonis.Win32.Filesystem.Directory.TransferTimestamps(folder, folder2);

            Assert.AreEqual(System.IO.Directory.GetCreationTime(folder), System.IO.Directory.GetCreationTime(folder2));
            Assert.AreEqual(System.IO.Directory.GetLastAccessTime(folder), System.IO.Directory.GetLastAccessTime(folder2));
            Assert.AreEqual(System.IO.Directory.GetLastWriteTime(folder), System.IO.Directory.GetLastWriteTime(folder2));
         }

         Console.WriteLine();
      }
   }
}
