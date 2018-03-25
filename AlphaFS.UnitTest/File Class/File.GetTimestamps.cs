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
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AlphaFS.UnitTest
{
   partial class FileTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>
      
      [TestMethod]
      public void File_GetXxxTimeXxx_LocalAndNetwork_Success()
      {
         File_GetXxxTimeXxx(false);
         File_GetXxxTimeXxx(true);
      }


      [TestMethod]
      public void File_GetXxxTimeXxx_NonExistingFile_Success()
      {
         const string Path = @"z:\nonExistingPath\nonExistingFile.txt";
         Assert.IsFalse(System.IO.File.Exists(Path));

         var newDateTime = new DateTime(1601, 1, 1);
         var newDateTimeLocaltime = new DateTime(1601, 1, 1).ToLocalTime();


         Assert.AreEqual(newDateTimeLocaltime, System.IO.File.GetCreationTime(Path));
         Assert.AreEqual(newDateTimeLocaltime, Alphaleonis.Win32.Filesystem.File.GetCreationTime(Path));
         
         Assert.AreEqual(newDateTime, System.IO.File.GetCreationTimeUtc(Path));
         Assert.AreEqual(newDateTime, Alphaleonis.Win32.Filesystem.File.GetCreationTimeUtc(Path));


         Assert.AreEqual(newDateTimeLocaltime, System.IO.File.GetLastAccessTime(Path));
         Assert.AreEqual(newDateTimeLocaltime, Alphaleonis.Win32.Filesystem.File.GetLastAccessTime(Path));

         Assert.AreEqual(newDateTime, System.IO.File.GetLastAccessTimeUtc(Path));
         Assert.AreEqual(newDateTime, Alphaleonis.Win32.Filesystem.File.GetLastAccessTimeUtc(Path));


         Assert.AreEqual(newDateTimeLocaltime, System.IO.File.GetLastWriteTime(Path));
         Assert.AreEqual(newDateTimeLocaltime, Alphaleonis.Win32.Filesystem.File.GetLastWriteTime(Path));

         Assert.AreEqual(newDateTime, System.IO.File.GetLastWriteTimeUtc(Path));
         Assert.AreEqual(newDateTime, Alphaleonis.Win32.Filesystem.File.GetLastWriteTimeUtc(Path));
      }


      

      private void File_GetXxxTimeXxx(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         var tempPath = System.IO.Path.GetTempPath();
         if (isNetwork)
            tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);


         using (var rootDir = new TemporaryDirectory(tempPath, MethodBase.GetCurrentMethod().Name))
         {
            var file = UnitTestConstants.NotepadExe;

            Console.WriteLine("\nInput File Path: [{0}]\n", file);


            Assert.AreEqual(System.IO.File.GetCreationTime(file), Alphaleonis.Win32.Filesystem.File.GetCreationTime(file));
            Assert.AreEqual(System.IO.File.GetCreationTimeUtc(file), Alphaleonis.Win32.Filesystem.File.GetCreationTimeUtc(file));
            Assert.AreEqual(System.IO.File.GetLastAccessTime(file), Alphaleonis.Win32.Filesystem.File.GetLastAccessTime(file));
            Assert.AreEqual(System.IO.File.GetLastAccessTimeUtc(file), Alphaleonis.Win32.Filesystem.File.GetLastAccessTimeUtc(file));
            Assert.AreEqual(System.IO.File.GetLastWriteTime(file), Alphaleonis.Win32.Filesystem.File.GetLastWriteTime(file));
            Assert.AreEqual(System.IO.File.GetLastWriteTimeUtc(file), Alphaleonis.Win32.Filesystem.File.GetLastWriteTimeUtc(file));


            // We can not compare ChangeTime against .NET because it does not exist.
            // Creating a file and renaming it triggers ChangeTime, so test for that.

            file = rootDir.RandomFileFullPath;
            if (isNetwork) file = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(file);
            Console.WriteLine("Input File Path: [{0}]\n", file);

            var fileInfo = new System.IO.FileInfo(file);
            using (fileInfo.Create()) { }
            var fileName = fileInfo.Name;


            var changeTimeActual = Alphaleonis.Win32.Filesystem.File.GetChangeTime(file);
            var changeTimeUtcActual = Alphaleonis.Win32.Filesystem.File.GetChangeTimeUtc(file);


            // Sleep for three seconds.
            var delay = 3;

            fileInfo.MoveTo(fileInfo.FullName.Replace(fileName, fileName + "-Renamed"));
            Thread.Sleep(delay*1000);
            fileInfo.MoveTo(fileInfo.FullName.Replace(fileName + "-Renamed", fileName));


            var newChangeTime = changeTimeActual.AddSeconds(3);
            Assert.AreEqual(changeTimeActual.AddSeconds(3), newChangeTime);

            newChangeTime = changeTimeUtcActual.AddSeconds(3);
            Assert.AreEqual(changeTimeUtcActual.AddSeconds(3), newChangeTime);
         }
      }
   }
}
