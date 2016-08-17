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
   partial class DirectoryTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>
      
      [TestMethod]
      public void Directory_GetXxxTimeXxx_LocalAndUNC_Success()
      {
         DumpGetXxxTimeXxx(true);
         DumpGetXxxTimeXxx(false);
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




      private void DumpGetXxxTimeXxx(bool isLocal)
      {
         UnitTestConstants.PrintUnitTestHeader(!isLocal);

         string path = isLocal ? UnitTestConstants.SysRoot32 : Alphaleonis.Win32.Filesystem.Path.LocalToUnc(UnitTestConstants.SysRoot32);

         Console.WriteLine("\nInput Directory Path: [{0}]\n", path);
         UnitTestConstants.StopWatcher(true);


         DateTime actual = Alphaleonis.Win32.Filesystem.Directory.GetCreationTime(path);
         DateTime expected = System.IO.Directory.GetCreationTime(path);
         Console.WriteLine("\tGetCreationTime()     : [{0}]    System.IO: [{1}]", actual, expected);
         Assert.AreEqual(expected, actual, "GetCreationTime()");

         actual = Alphaleonis.Win32.Filesystem.Directory.GetCreationTimeUtc(path);
         expected = System.IO.Directory.GetCreationTimeUtc(path);
         Console.WriteLine("\tGetCreationTimeUtc()  : [{0}]    System.IO: [{1}]\n", actual, expected);
         Assert.AreEqual(expected, actual, "GetCreationTimeUtc()");


         actual = Alphaleonis.Win32.Filesystem.Directory.GetLastAccessTime(path);
         expected = System.IO.Directory.GetLastAccessTime(path);
         Console.WriteLine("\tGetLastAccessTime()   : [{0}]    System.IO: [{1}]", actual, expected);
         Assert.AreEqual(expected, actual, "GetLastAccessTime()");

         actual = Alphaleonis.Win32.Filesystem.Directory.GetLastAccessTimeUtc(path);
         expected = System.IO.Directory.GetLastAccessTimeUtc(path);
         Console.WriteLine("\tGetLastAccessTimeUtc(): [{0}]    System.IO: [{1}]\n", actual, expected);
         Assert.AreEqual(expected, actual, "GetLastAccessTimeUtc()");


         actual = Alphaleonis.Win32.Filesystem.Directory.GetLastWriteTime(path);
         expected = System.IO.Directory.GetLastWriteTime(path);
         Console.WriteLine("\tGetLastWriteTime()    : [{0}]    System.IO: [{1}]", actual, expected);
         Assert.AreEqual(expected, actual, "GetLastWriteTime()");

         actual = Alphaleonis.Win32.Filesystem.Directory.GetLastWriteTimeUtc(path);
         expected = System.IO.Directory.GetLastWriteTimeUtc(path);
         Console.WriteLine("\tGetLastWriteTimeUtc() : [{0}]    System.IO: [{1}]\n", actual, expected);
         Assert.AreEqual(expected, actual, "GetLastWriteTimeUtc()");


         Console.WriteLine("\tGetChangeTime()       : [{0}]    System.IO: [N/A]", Alphaleonis.Win32.Filesystem.Directory.GetChangeTime(path));
         Console.WriteLine("\tGetChangeTimeUtc()    : [{0}]    System.IO: [N/A]", Alphaleonis.Win32.Filesystem.Directory.GetChangeTimeUtc(path));

         Console.WriteLine();
         Console.WriteLine(UnitTestConstants.Reporter());
         Console.WriteLine();




         // We can not compare ChangeTime against .NET because it does not exist.
         // Creating a directory and renaming it triggers ChangeTime, so test for that.

         path = Alphaleonis.Win32.Filesystem.Path.GetTempPath("Directory-GetChangeTimeXxx()-directory-" + Path.GetRandomFileName());
         if (!isLocal) path = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(path);

         System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(path);
         di.Create();
         string fileName = di.Name;

         DateTime lastAccessTimeActual = System.IO.Directory.GetLastAccessTime(path);
         DateTime lastAccessTimeUtcActual = System.IO.Directory.GetLastAccessTimeUtc(path);
         DateTime changeTimeActual = Alphaleonis.Win32.Filesystem.Directory.GetChangeTime(path);
         DateTime changeTimeUtcActual = Alphaleonis.Win32.Filesystem.Directory.GetChangeTimeUtc(path);

         Console.WriteLine("\nTesting ChangeTime on a temp directory.");
         Console.WriteLine("\nInput Directory Path: [{0}]\n", path);
         Console.WriteLine("\tGetChangeTime()       : [{0}]\t", changeTimeActual);
         Console.WriteLine("\tGetChangeTimeUtc()    : [{0}]\t", changeTimeUtcActual);

         di.MoveTo(di.FullName.Replace(fileName, fileName + "-Renamed"));

         // Pause for at least a second so that the difference in time can be seen.
         int sleep = new Random().Next(2000, 4000);
         Thread.Sleep(sleep);

         di.MoveTo(di.FullName.Replace(fileName + "-Renamed", fileName));

         DateTime lastAccessTimeExpected = System.IO.Directory.GetLastAccessTime(path);
         DateTime lastAccessTimeUtcExpected = System.IO.Directory.GetLastAccessTimeUtc(path);
         DateTime changeTimeExpected = Alphaleonis.Win32.Filesystem.Directory.GetChangeTime(path);
         DateTime changeTimeUtcExpected = Alphaleonis.Win32.Filesystem.Directory.GetChangeTimeUtc(path);

         Console.WriteLine("\nTrigger ChangeTime by renaming the directory.");
         Console.WriteLine("For Unit Test, ChangeTime should differ approximately: [{0}] seconds.\n", sleep / 1000);
         Console.WriteLine("\tGetChangeTime()       : [{0}]\t", changeTimeExpected);
         Console.WriteLine("\tGetChangeTimeUtc()    : [{0}]\t\n", changeTimeUtcExpected);


         Assert.AreNotEqual(changeTimeActual, changeTimeExpected);
         Assert.AreNotEqual(changeTimeUtcActual, changeTimeUtcExpected);

         Assert.AreEqual(lastAccessTimeExpected, lastAccessTimeActual);
         Assert.AreEqual(lastAccessTimeUtcExpected, lastAccessTimeUtcActual);


         di.Delete();
         di.Refresh(); // Must Refresh() to get actual state.
         Assert.IsFalse(di.Exists, "Cleanup failed: Directory should have been removed.");
         Console.WriteLine();
      }
   }
}
