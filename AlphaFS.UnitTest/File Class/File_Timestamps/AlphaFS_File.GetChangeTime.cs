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
   public partial class TimestampsTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void AlphaFS_File_GetChangeTime_LocalAndNetwork_Success()
      {
         AlphaFS_File_GetChangeTime(false);
         AlphaFS_File_GetChangeTime(true);
      }


      private void AlphaFS_File_GetChangeTime(bool isNetwork)
      {
         using (var tempRoot = new TemporaryDirectory(isNetwork))
         {
            var file = UnitTestConstants.NotepadExe;

            Console.WriteLine("Input File Path: [{0}]", file);


            Assert.AreEqual(System.IO.File.GetCreationTime(file), Alphaleonis.Win32.Filesystem.File.GetCreationTime(file));
            Assert.AreEqual(System.IO.File.GetCreationTimeUtc(file), Alphaleonis.Win32.Filesystem.File.GetCreationTimeUtc(file));
            Assert.AreEqual(System.IO.File.GetLastAccessTime(file), Alphaleonis.Win32.Filesystem.File.GetLastAccessTime(file));
            Assert.AreEqual(System.IO.File.GetLastAccessTimeUtc(file), Alphaleonis.Win32.Filesystem.File.GetLastAccessTimeUtc(file));
            Assert.AreEqual(System.IO.File.GetLastWriteTime(file), Alphaleonis.Win32.Filesystem.File.GetLastWriteTime(file));
            Assert.AreEqual(System.IO.File.GetLastWriteTimeUtc(file), Alphaleonis.Win32.Filesystem.File.GetLastWriteTimeUtc(file));


            // We can not compare ChangeTime against .NET because it does not exist.
            // Creating a file and renaming it triggers ChangeTime, so test for that.

            file = tempRoot.RandomFileFullPath;
            Console.WriteLine("Input File Path: [{0}]\n", file);


            var fileInfo = new System.IO.FileInfo(file);
            using (fileInfo.Create()) { }
            var fileName = fileInfo.Name;


            var changeTimeActual = Alphaleonis.Win32.Filesystem.File.GetChangeTime(file);
            var changeTimeUtcActual = Alphaleonis.Win32.Filesystem.File.GetChangeTimeUtc(file);


            // Sleep for three seconds.
            var delay = 3;

            fileInfo.MoveTo(fileInfo.FullName.Replace(fileName, fileName + "-Renamed"));
            Thread.Sleep(delay * 1000);
            fileInfo.MoveTo(fileInfo.FullName.Replace(fileName + "-Renamed", fileName));


            var newChangeTime = changeTimeActual.AddSeconds(3);
            Assert.AreEqual(changeTimeActual.AddSeconds(3), newChangeTime);

            newChangeTime = changeTimeUtcActual.AddSeconds(3);
            Assert.AreEqual(changeTimeUtcActual.AddSeconds(3), newChangeTime);
         }
      }
   }
}
