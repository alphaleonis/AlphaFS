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
using System.ComponentModel;
using System.IO;
using System.Linq;
using Alphaleonis.Win32;
using Directory = System.IO.Directory;
using File = System.IO.File;
using Path = System.IO.Path;


namespace AlphaFS.UnitTest
{
   partial class DirectoryTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>

      [TestMethod]
      public void Directory_GetDirectories_LocalAndUNC_Success()
      {
         DumpDirectory_GetDirectories(true);
         DumpDirectory_GetDirectories(false);
      }


      [TestMethod]
      public void Directory_GetDirectories_WithSearchPattern_LocalAndUNC_Success()
      {
         DumpDirectory_GetDirectories_WithSearchPattern(true);
         DumpDirectory_GetDirectories_WithSearchPattern(false);
      }


      [TestMethod]
      public void Directory_GetDirectories_AbsoluteAndRelativePath_Success()
      {
         Directory_GetDirectories_AbsoluteAndRelativePath_Success(true);
         Directory_GetDirectories_AbsoluteAndRelativePath_Success(false);
      }




      private void DumpDirectory_GetDirectories(bool isLocal)
      {
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

         var expectedLastError = (int)(isLocal ? Win32Errors.ERROR_PATH_NOT_FOUND : Win32Errors.ERROR_BAD_NET_NAME);
         var expectedException = isLocal ? "System.IO.DirectoryNotFoundException" : "System.IO.IOException";
         var exception = false;
         try
         {
            Console.WriteLine("\nCatch: [{0}]", expectedException);

            string nonExistingPath = letter + folderSource;
            if (!isLocal) nonExistingPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(nonExistingPath);

            Directory.GetDirectories(nonExistingPath);
         }
         catch (Exception ex)
         {
            // win32Error is always 0
            var win32Error = new Win32Exception("", ex);
            Assert.IsTrue(win32Error.NativeErrorCode == expectedLastError, string.Format("Expected Win32Exception error should be: [{0}], got: [{1}]", expectedLastError, win32Error.NativeErrorCode));

            string exceptionTypeName = ex.GetType().FullName;
            if (exceptionTypeName.Equals(expectedException))
            {
               exception = true;
               Console.WriteLine("\n\t[{0}]: [{1}]", exceptionTypeName, ex.Message.Replace(Environment.NewLine, "  "));
            }
            else
               Console.WriteLine("\n\tCaught (unexpected) {0}: [{1}]", exceptionTypeName, ex.Message.Replace(Environment.NewLine, "  "));
         }
         Assert.IsTrue(exception, "[{0}] should have been caught.", expectedException);
         Console.WriteLine();

         #endregion // DirectoryNotFoundException (UnitTestConstants.Local) / IOException (UnitTestConstants.Network)

         #region IOException

         string tempPath = Alphaleonis.Win32.Filesystem.Path.GetTempPath("Directory.GetDirectories-file-" + Path.GetRandomFileName());
         if (!isLocal) tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);

         try
         {
            using (File.Create(tempPath)) { }

            expectedLastError = (int) Win32Errors.ERROR_DIRECTORY;
            expectedException = "System.IO.IOException";
            exception = false;
            try
            {
               Console.WriteLine("\nCatch: [{0}]", expectedException);
               Directory.GetDirectories(tempPath);
            }
            catch (IOException ex)
            {
               exception = true;
               Console.WriteLine("\n\t[{0}]: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
            }
            catch (Exception ex)
            {
               Console.WriteLine("\n\tCaught (unexpected) {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
            }
            Assert.IsTrue(exception, "[{0}] should have been caught.", expectedException);
         }
         finally
         {
            File.Delete(tempPath);
            Assert.IsFalse(File.Exists(tempPath), "Cleanup failed: File should have been removed.");
            Console.WriteLine();
         }

         #endregion // IOException

         #region UnauthorizedAccessException

         tempPath = Path.Combine(UnitTestConstants.SysRoot, "CSC");
         if (!isLocal) tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);

         if (Directory.Exists(tempPath))
         {
            expectedLastError = (int)Win32Errors.ERROR_ACCESS_DENIED;
            expectedException = "System.UnauthorizedAccessException";
            exception = false;
            try
            {
               Console.WriteLine("\nCatch: [{0}]", expectedException);
               Directory.GetDirectories(tempPath, searchPattern, SearchOption.AllDirectories).Any();
            }
            catch (UnauthorizedAccessException ex)
            {
               var win32Error = new Win32Exception("", ex);
               Assert.IsTrue(win32Error.NativeErrorCode == expectedLastError, string.Format("Expected Win32Exception error should be: [{0}], got: [{1}]", expectedLastError, win32Error.NativeErrorCode));

               exception = true;
               Console.WriteLine("\n\t[{0}]: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
            }
            catch (Exception ex)
            {
               Console.WriteLine("\n\tCaught (unexpected) {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
            }
            Assert.IsTrue(exception, "[{0}] should have been caught.", expectedException);
            Console.WriteLine();
         }

         #endregion // UnauthorizedAccessException

         string path = isLocal ? UnitTestConstants.SysRoot : Alphaleonis.Win32.Filesystem.Path.LocalToUnc(UnitTestConstants.SysRoot);

         Console.WriteLine("\nInput Directory Path: [{0}]\n", path);
         Console.WriteLine("\tGet directories, using \"SearchOption.{0}\"\n", searchOption);

         UnitTestConstants.StopWatcher(true);
         foreach (string folder in Directory.GetDirectories(path, searchPattern, searchOption))
            Console.WriteLine("\t#{0:000}\t[{1}]", ++cnt, folder);

         Console.WriteLine();
         Console.WriteLine(UnitTestConstants.Reporter());

         if (cnt == 0)
            Assert.Inconclusive("Nothing was enumerated.");

         Console.WriteLine();
      }


      private void DumpDirectory_GetDirectories_WithSearchPattern(bool isLocal)
      {
         #region Setup

         UnitTestConstants.PrintUnitTestHeader(!isLocal);

         string tempPath = Path.Combine(Path.GetTempPath(), "Directory.GetDirectories_With_SearchPattern()-" + Path.GetRandomFileName());
         if (!isLocal) tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);

         Console.WriteLine("\nInput Directory Path: [{0}]", tempPath);

         Directory.CreateDirectory(tempPath);
         Directory.CreateDirectory(Path.Combine(tempPath, "a.txt"));
         Directory.CreateDirectory(Path.Combine(tempPath, "aa.txt"));
         Directory.CreateDirectory(Path.Combine(tempPath, "aba.txt"));
         Directory.CreateDirectory(Path.Combine(tempPath, "foo.txt"));
         Directory.CreateDirectory(Path.Combine(tempPath, "footxt"));

         #endregion // Setup

         try
         {
            var folders = Alphaleonis.Win32.Filesystem.Directory.GetDirectories(tempPath, "foo.txt");
            Console.WriteLine("\tDirectory.GetDirectories(tempPath, \"foo.txt\");");
            Assert.IsTrue(folders.Contains(Path.Combine(tempPath, "foo.txt"), StringComparer.InvariantCultureIgnoreCase));
            Assert.IsFalse(folders.Contains(Path.Combine(tempPath, "fooatxt"), StringComparer.InvariantCultureIgnoreCase));

            folders = Alphaleonis.Win32.Filesystem.Directory.GetDirectories(tempPath, "a?a.txt");
            Console.WriteLine("\tDirectory.GetDirectories(tempPath, \"a?a.txt\");");
            Assert.IsTrue(folders.Contains(Path.Combine(tempPath, "aba.txt"), StringComparer.InvariantCultureIgnoreCase), "? wildcard failed");
            Assert.IsFalse(folders.Contains(Path.Combine(tempPath, "aa.txt"), StringComparer.InvariantCultureIgnoreCase), "? wildcard failed");

            folders = Alphaleonis.Win32.Filesystem.Directory.GetDirectories(tempPath, "a*.*");
            Console.WriteLine("\tDirectory.GetDirectories(tempPath, \"a*.*\");");
            Assert.IsTrue(folders.Contains(Path.Combine(tempPath, "a.txt"), StringComparer.InvariantCultureIgnoreCase), "* wildcard failed");
            Assert.IsTrue(folders.Contains(Path.Combine(tempPath, "aa.txt"), StringComparer.InvariantCultureIgnoreCase), "* wildcard failed");
            Assert.IsTrue(folders.Contains(Path.Combine(tempPath, "aba.txt"), StringComparer.InvariantCultureIgnoreCase), "* wildcard failed");

            folders = Alphaleonis.Win32.Filesystem.Directory.GetDirectories(tempPath, "*.*");
            Console.WriteLine("\tDirectory.GetDirectories(tempPath, \"*.*\");");
            var folders2 = Alphaleonis.Win32.Filesystem.Directory.GetDirectories(tempPath);
            Assert.IsTrue(folders.Length == folders2.Length, "*.* failed");
            folders = Alphaleonis.Win32.Filesystem.Directory.GetDirectories(tempPath, "*.*.*");
            Console.WriteLine("\tDirectory.GetDirectories(tempPath, \"*.*.*\");");
            Assert.IsTrue(folders.Length == folders2.Length, "*.* failed");
         }
         finally
         {
            Directory.Delete(tempPath, true);
            Assert.IsFalse(Directory.Exists(tempPath), "Cleanup failed: Directory should have been removed.");
            Console.WriteLine();
         }
      }


      private void Directory_GetDirectories_AbsoluteAndRelativePath_Success(bool isLocal)
      {
         UnitTestConstants.PrintUnitTestHeader(!isLocal);

         var tempPath = System.IO.Path.GetTempPath();
         if (!isLocal) tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);

         Assert.IsTrue(System.IO.Path.IsPathRooted(tempPath));
         Environment.CurrentDirectory = tempPath;

         Console.WriteLine("\nInput Directory Path: [{0}]", tempPath);


         Console.WriteLine("\nRelative Paths\n");
         var folders = Alphaleonis.Win32.Filesystem.Directory.GetDirectories(".");
         Assert.IsTrue(folders.Length > 0);
         foreach (var folder in folders)
         {
            Console.WriteLine("\tDirectory: " + folder);
            Assert.IsFalse(System.IO.Path.IsPathRooted(folder), "IsPathRooted of return and argument types should match.");
         }


         Console.WriteLine("\nAbsolute Paths\n");
         folders = Alphaleonis.Win32.Filesystem.Directory.GetDirectories(tempPath);
         Assert.IsTrue(folders.Length > 0);
         foreach (var folder in folders)
         {
            Console.WriteLine("\tDirectory: " + folder);
            Assert.IsTrue(System.IO.Path.IsPathRooted(folder), "IsPathRooted of return and argument types should match.");
         }


         Console.WriteLine();
      }
   }
}
