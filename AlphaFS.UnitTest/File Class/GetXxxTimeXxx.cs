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
using Alphaleonis.Win32.Filesystem;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AlphaFS.UnitTest
{
   partial class FileTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>
      
      [TestMethod]
      public void File_GetXxxTimeXxx_LocalAndUNC_Success()
      {
         DumpGetXxxTimeXxx(true);
         DumpGetXxxTimeXxx(false);
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




      private void DumpGetXxxTimeXxx(bool isLocal)
      {
         UnitTestConstants.PrintUnitTestHeader(!isLocal);

         string path = UnitTestConstants.NotepadExe;
         if (!isLocal) path = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(path);

         Console.WriteLine("\nInput File Path: [{0}]\n", path);
         UnitTestConstants.StopWatcher(true);


         DateTime actual = Alphaleonis.Win32.Filesystem.File.GetCreationTime(path);
         DateTime expected = System.IO.File.GetCreationTime(path);
         Console.WriteLine("\tGetCreationTime()     : [{0}]    System.IO: [{1}]", actual, expected);
         Assert.AreEqual(expected, actual);

         actual = Alphaleonis.Win32.Filesystem.File.GetCreationTimeUtc(path);
         expected = System.IO.File.GetCreationTimeUtc(path);
         Console.WriteLine("\tGetCreationTimeUtc()  : [{0}]    System.IO: [{1}]\n", actual, expected);
         Assert.AreEqual(expected, actual);


         actual = Alphaleonis.Win32.Filesystem.File.GetLastAccessTime(path);
         expected = System.IO.File.GetLastAccessTime(path);
         Console.WriteLine("\tGetLastAccessTime()   : [{0}]    System.IO: [{1}]", actual, expected);
         Assert.AreEqual(expected, actual);

         actual = Alphaleonis.Win32.Filesystem.File.GetLastAccessTimeUtc(path);
         expected = System.IO.File.GetLastAccessTimeUtc(path);
         Console.WriteLine("\tGetLastAccessTimeUtc(): [{0}]    System.IO: [{1}]\n", actual, expected);
         Assert.AreEqual(expected, actual);


         actual = Alphaleonis.Win32.Filesystem.File.GetLastWriteTime(path);
         expected = System.IO.File.GetLastWriteTime(path);
         Console.WriteLine("\tGetLastWriteTime()    : [{0}]    System.IO: [{1}]", actual, expected);
         Assert.AreEqual(expected, actual);

         actual = Alphaleonis.Win32.Filesystem.File.GetLastWriteTimeUtc(path);
         expected = System.IO.File.GetLastWriteTimeUtc(path);
         Console.WriteLine("\tGetLastWriteTimeUtc() : [{0}]    System.IO: [{1}]\n", actual, expected);
         Assert.AreEqual(expected, actual);
         

         Console.WriteLine("\tGetChangeTime()       : [{0}]    System.IO: [N/A]", Alphaleonis.Win32.Filesystem.File.GetChangeTime(path));
         Console.WriteLine("\tGetChangeTimeUtc()    : [{0}]    System.IO: [N/A]", Alphaleonis.Win32.Filesystem.File.GetChangeTimeUtc(path));

         Console.WriteLine();
         Console.WriteLine(UnitTestConstants.Reporter());
         Console.WriteLine();
         

         
         
         // We can not compare ChangeTime against .NET because it does not exist.
         // Creating a file and renaming it triggers ChangeTime, so test for that.

         path = Alphaleonis.Win32.Filesystem.Path.GetTempPath("File-GetChangeTimeXxx()-file-" + Path.GetRandomFileName());
         if (!isLocal) path = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(path);

         System.IO.FileInfo fi = new System.IO.FileInfo(path);
         using (fi.Create()) { }
         string fileName = fi.Name;

         DateTime lastAccessTimeActual = System.IO.File.GetLastAccessTime(path);
         DateTime lastAccessTimeUtcActual = System.IO.File.GetLastAccessTimeUtc(path);

         DateTime changeTimeActual = Alphaleonis.Win32.Filesystem.File.GetChangeTime(path);
         DateTime changeTimeUtcActual = Alphaleonis.Win32.Filesystem.File.GetChangeTimeUtc(path);

         Console.WriteLine("\nTesting ChangeTime on a temp file.");
         Console.WriteLine("\nInput File Path: [{0}]\n", path);
         Console.WriteLine("\tGetChangeTime()       : [{0}]\t", changeTimeActual);
         Console.WriteLine("\tGetChangeTimeUtc()    : [{0}]\t", changeTimeUtcActual);

         fi.MoveTo(fi.FullName.Replace(fileName, fileName + "-Renamed"));

         // Pause for at least a second so that the difference in time can be seen.
         int sleep = new Random().Next(2000, 4000);
         Thread.Sleep(sleep);

         fi.MoveTo(fi.FullName.Replace(fileName + "-Renamed", fileName));

         DateTime lastAccessTimeExpected = System.IO.File.GetLastAccessTime(path);
         DateTime lastAccessTimeUtcExpected = System.IO.File.GetLastAccessTimeUtc(path);
         DateTime changeTimeExpected = Alphaleonis.Win32.Filesystem.File.GetChangeTime(path);
         DateTime changeTimeUtcExpected = Alphaleonis.Win32.Filesystem.File.GetChangeTimeUtc(path);

         Console.WriteLine("\nTrigger ChangeTime by renaming the file.");
         Console.WriteLine("For Unit Test, ChangeTime should differ approximately: [{0}] seconds.\n", sleep / 1000);
         Console.WriteLine("\tGetChangeTime()       : [{0}]\t", changeTimeExpected);
         Console.WriteLine("\tGetChangeTimeUtc()    : [{0}]\t\n", changeTimeUtcExpected);


         Assert.AreNotEqual(changeTimeActual, changeTimeExpected);
         Assert.AreNotEqual(changeTimeUtcActual, changeTimeUtcExpected);

         Assert.AreEqual(lastAccessTimeExpected, lastAccessTimeActual);
         Assert.AreEqual(lastAccessTimeUtcExpected, lastAccessTimeUtcActual);


         fi.Delete();
         fi.Refresh(); // Must Refresh() to get actual state.
         Assert.IsFalse(fi.Exists, "Cleanup failed: File should have been removed.");
         Console.WriteLine();
      }
   }
}
