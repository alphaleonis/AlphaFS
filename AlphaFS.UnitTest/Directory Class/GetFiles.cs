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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;
using Directory = System.IO.Directory;
using File = System.IO.File;
using Path = System.IO.Path;


namespace AlphaFS.UnitTest
{
   partial class DirectoryTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>

      [TestMethod]
      public void Directory_GetFiles_LocalAndUNC_Success()
      {
         DumpDirectory_GetFiles(true);
         DumpDirectory_GetFiles(false);
      }


      [TestMethod]
      public void Directory_GetFiles_WithSearchPattern_LocalAndUNC_Success()
      {
         DumpDirectory_GetFiles_WithSearchPattern(true);
         DumpDirectory_GetFiles_WithSearchPattern(false);
      }


      [TestMethod]
      public void Directory_GetFiles_AbsoluteAndRelativePath_Success()
      {
         DumpDirectory_GetFiles_AbsoluteAndRelativePath(true);
         DumpDirectory_GetFiles_AbsoluteAndRelativePath(false);
      }



      private void DumpDirectory_GetFiles(bool isLocal)
      {
         var isNetwork = !isLocal;

         #region Setup

         UnitTestConstants.PrintUnitTestHeader(!isLocal);

         int cnt = 0;
         string searchPattern = Alphaleonis.Win32.Filesystem.Path.WildcardStarMatchAll;
         SearchOption searchOption = SearchOption.TopDirectoryOnly;

         string random = Path.GetRandomFileName();
         string folderSource = @"folder-source-" + random;

         string originalLetter = Alphaleonis.Win32.Filesystem.DriveInfo.GetFreeDriveLetter() + @":";
         string letter = originalLetter + @"\";
         
         #endregion // Setup

         #region DirectoryNotFoundException (UnitTestConstants.Local) / IOException (UnitTestConstants.Network)

         var gotException = false;
         try
         {
            string nonExistingPath = letter + folderSource;
            if (!isLocal) nonExistingPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(nonExistingPath);

            Console.WriteLine("\nInput Directory Path: [{0}]", nonExistingPath);

            Directory.GetFiles(nonExistingPath);
         }
         catch (Exception ex)
         {
            // DirectoryNotFoundException is only for local.
            // For UNC: IOException or DeviceNotReadyException.

            var exName = ex.GetType().Name;
            gotException = exName.Equals(isNetwork ? "IOException" : "DirectoryNotFoundException", StringComparison.OrdinalIgnoreCase);
            if (!gotException && isNetwork)
               gotException = exName.Equals("DeviceNotReadyException", StringComparison.OrdinalIgnoreCase);
            Console.WriteLine("\tCaught Exception: [{0}] Message: [{1}]", exName, ex.Message);
         }
         Assert.IsTrue(gotException, "The exception is not caught, but is expected to.");

         Console.WriteLine();

         #endregion // DirectoryNotFoundException (UnitTestConstants.Local) / IOException (UnitTestConstants.Network)

         #region IOException

         string tempPath = Alphaleonis.Win32.Filesystem.Path.GetTempPath("Directory.GetFiles-" + Path.GetRandomFileName());
         if (!isLocal) tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);

         Console.WriteLine("\nInput Directory Path: [{0}]", tempPath);

         using (File.Create(tempPath)) { }

         gotException = false;
         try
         {
            Directory.GetFiles(tempPath);
         }
         catch (Exception ex)
         {
            var exName = ex.GetType().Name;
            gotException = exName.Equals("IOException", StringComparison.OrdinalIgnoreCase);
            Console.WriteLine("\tCaught Exception: [{0}] Message: [{1}]", exName, ex.Message);
         }
         finally
         {
            File.Delete(tempPath);
            Assert.IsFalse(File.Exists(tempPath), "Cleanup failed: File should have been removed.");
            Console.WriteLine();
         }

         Assert.IsTrue(gotException, "The exception is not caught, but is expected to.");

         #endregion // IOException

         #region UnauthorizedAccessException

         tempPath = Path.Combine(UnitTestConstants.SysRoot, "CSC");
         if (!isLocal) tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);

         Console.WriteLine("\nInput Directory Path: [{0}]", tempPath);

         if (Directory.Exists(tempPath))
         {
            gotException = false;
            try
            {
               Directory.GetFiles(tempPath, searchPattern, SearchOption.AllDirectories).Any();
            }
            catch (Exception ex)
            {
               var exName = ex.GetType().Name;
               gotException = exName.Equals("UnauthorizedAccessException", StringComparison.OrdinalIgnoreCase);
               Console.WriteLine("\tCaught Exception: [{0}] Message: [{1}]", exName, ex.Message);
            }
            Assert.IsTrue(gotException, "The exception is not caught, but is expected to.");
            Console.WriteLine();
         }

         #endregion // UnauthorizedAccessException

         string path = isLocal ? UnitTestConstants.SysRoot : Alphaleonis.Win32.Filesystem.Path.LocalToUnc(UnitTestConstants.SysRoot);

         Console.WriteLine("\nInput Directory Path: [{0}]\n", path);
         Console.WriteLine("\tGet files, using \"SearchOption.{0}\"\n", searchOption);

         UnitTestConstants.StopWatcher(true);
         foreach (string file in Directory.GetFiles(path, searchPattern, searchOption))
            Console.WriteLine("\t#{0:000}\t[{1}]", ++cnt, file);

         Console.WriteLine();
         Console.WriteLine(UnitTestConstants.Reporter());

         if (cnt == 0)
            Assert.Inconclusive("Nothing was enumerated.");

         Console.WriteLine();
      }


      private void DumpDirectory_GetFiles_WithSearchPattern(bool isLocal)
      {
         #region Setup

         UnitTestConstants.PrintUnitTestHeader(!isLocal);

         string tempPath = Path.Combine(Path.GetTempPath(), "Directory.GetDirectories_With_SearchPattern()-" + Path.GetRandomFileName());
         if (!isLocal) tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);

         Console.WriteLine("\nInput Directory Path: [{0}]", tempPath);

         Directory.CreateDirectory(tempPath);
         using (File.Create(Path.Combine(tempPath, "a.txt"))) { }
         using (File.Create(Path.Combine(tempPath, "aa.txt"))) { }
         using (File.Create(Path.Combine(tempPath, "aba.txt"))) { }
         using (File.Create(Path.Combine(tempPath, "foo.txt"))) { }
         using (File.Create(Path.Combine(tempPath, "footxt"))) { }

         #endregion // Setup

         try
         {
            var files = Alphaleonis.Win32.Filesystem.Directory.GetFiles(tempPath, "foo.txt");
            Console.WriteLine("\tDirectory.GetFiles(tempPath, \"foo.txt\");");
            Assert.IsTrue(files.Length == 1 && files.Contains(Path.Combine(tempPath, "foo.txt"), StringComparer.InvariantCultureIgnoreCase));
            Assert.IsFalse(files.Contains(Path.Combine(tempPath, "fooatxt"), StringComparer.InvariantCultureIgnoreCase));

            files = Alphaleonis.Win32.Filesystem.Directory.GetFiles(tempPath, "a?a.txt");
            Console.WriteLine("\tDirectory.GetFiles(tempPath, \"a?a.txt\");");
            Assert.IsTrue(files.Length == 1 && files.Contains(Path.Combine(tempPath, "aba.txt"), StringComparer.InvariantCultureIgnoreCase), "? wildcard failed");
            Assert.IsFalse(files.Contains(Path.Combine(tempPath, "aa.txt"), StringComparer.InvariantCultureIgnoreCase), "? wildcard failed");

            files = Alphaleonis.Win32.Filesystem.Directory.GetFiles(tempPath, "a*.*");
            Console.WriteLine("\tDirectory.GetFiles(tempPath, \"a*.*\");");
            Assert.IsTrue(files.Length == 3);
            Assert.IsTrue(files.Contains(Path.Combine(tempPath, "a.txt"), StringComparer.InvariantCultureIgnoreCase), "* wildcard failed");
            Assert.IsTrue(files.Contains(Path.Combine(tempPath, "aa.txt"), StringComparer.InvariantCultureIgnoreCase), "* wildcard failed");
            Assert.IsTrue(files.Contains(Path.Combine(tempPath, "aba.txt"), StringComparer.InvariantCultureIgnoreCase), "* wildcard failed");

            files = Alphaleonis.Win32.Filesystem.Directory.GetFiles(tempPath, "*.*");
            Console.WriteLine("\tDirectory.GetFiles(tempPath, \"*.*\");");
            var files2 = Alphaleonis.Win32.Filesystem.Directory.GetFiles(tempPath);
            Assert.IsTrue(files.Length == files2.Length, "*.* failed");
            files = Alphaleonis.Win32.Filesystem.Directory.GetFiles(tempPath, "*.*.*");
            Console.WriteLine("\tDirectory.GetFiles(tempPath, \"*.*.*\");");
            Assert.IsTrue(files.Length == files2.Length, "*.* failed");
         }
         finally
         {
            Directory.Delete(tempPath, true);
            Assert.IsFalse(Directory.Exists(tempPath), "Cleanup failed: Directory should have been removed.");
            Console.WriteLine();
         }
      }


      private void DumpDirectory_GetFiles_AbsoluteAndRelativePath(bool isLocal)
      {
         UnitTestConstants.PrintUnitTestHeader(!isLocal);

         var tempPath = System.IO.Path.GetTempPath();
         if (!isLocal) tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);

         Assert.IsTrue(System.IO.Path.IsPathRooted(tempPath));
         Environment.CurrentDirectory = tempPath;

         Console.WriteLine("\nInput Directory Path: [{0}]", tempPath);


         Console.WriteLine("\nRelative Paths\n");
         var files = Alphaleonis.Win32.Filesystem.Directory.GetFiles(".");
         Assert.IsTrue(files.Length > 0);
         foreach (var file in files)
         {
            Console.WriteLine("\tFile: " + file);
            Assert.IsFalse(System.IO.Path.IsPathRooted(file), "IsPathRooted of return and argument types should match.");
         }


         Console.WriteLine("\nAbsolute Paths\n");
         files = Alphaleonis.Win32.Filesystem.Directory.GetFiles(tempPath);
         Assert.IsTrue(files.Length > 0);
         foreach (var file in files)
         {
            Console.WriteLine("\tFile: " + file);
            Assert.IsTrue(System.IO.Path.IsPathRooted(file), "IsPathRooted of return and argument types should match.");
         }


         Console.WriteLine();
      }
   }
}
