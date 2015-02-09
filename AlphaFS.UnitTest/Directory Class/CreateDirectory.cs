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
using Alphaleonis.Win32;
using Alphaleonis.Win32.Filesystem;
using Directory = Alphaleonis.Win32.Filesystem.Directory;
using File = Alphaleonis.Win32.Filesystem.File;
using DriveInfo = Alphaleonis.Win32.Filesystem.DriveInfo;
using Path = Alphaleonis.Win32.Filesystem.Path;

namespace AlphaFS.UnitTest
{
   partial class DirectoryTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>

      #region AlreadyExistsException: FileExistsWithSameNameAsDirectory

      [TestMethod]
      public void Directory_CreateDirectory_FileExistsWithSameNameAsDirectoryLocal_AlreadyExistsException()
      {
         Directory_CreateDirectory_FileExistsWithSameNameAsDirectory_AlreadyExistsException(false);
      }

      [TestMethod]
      public void Directory_CreateDirectory_FileExistsWithSameNameAsDirectoryNetwork_AlreadyExistsException()
      {
         Directory_CreateDirectory_FileExistsWithSameNameAsDirectory_AlreadyExistsException(true);
      }

      private void Directory_CreateDirectory_FileExistsWithSameNameAsDirectory_AlreadyExistsException(bool isNetwork)
      {
         string tempPath = Path.GetTempPath("Directory.CreateDirectory()-FileExistsWithSameNameAsDirectory-" + Path.GetRandomFileName());

         if (isNetwork)
            tempPath = Path.LocalToUnc(tempPath);

         try
         {
            using (File.Create(tempPath)) { }

            const int expectedLastError = (int) Win32Errors.ERROR_ALREADY_EXISTS;
            const string expectedException = "System.IO.IOException";
            bool exception = false;

            try
            {
               Directory.CreateDirectory(tempPath);
            }
            catch (AlreadyExistsException ex)
            {
               exception = true;
               Console.WriteLine("\nCaught: [{0}]\n\n[{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));

               Assert.IsTrue(ex.Message.StartsWith("(" + expectedLastError + ")"), string.Format("Expected Win32Exception error is: [{0}]", expectedLastError));
            }
            catch (Exception ex)
            {
               Console.WriteLine("\nCaught unexpected {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
            }

            Assert.IsTrue(exception, "[{0}] should have been caught.", expectedException);
         }
         finally
         {
            if (File.Exists(tempPath))
            {
               File.Delete(tempPath);
               Assert.IsFalse(File.Exists(tempPath), "Cleanup failed: File should have been removed.");
            }
         }
      }

      #endregion // AlreadyExistsException: FileExistsWithSameNameAsDirectory


      #region ArgumentException: PathContainsInvalidCharacters

      [TestMethod]
      public void Directory_CreateDirectory_PathContainsInvalidCharacters_ArgumentException()
      {
         const string expectedException = "System.ArgumentException";
         bool exception = false;

         try
         {
            Directory.CreateDirectory(UnitTestConstants.SysDrive + @"\<>");
         }
         catch (ArgumentException ex)
         {
            exception = true;
            Console.WriteLine("\nCaught: [{0}]\n\n[{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
         }
         catch (Exception ex)
         {
            Console.WriteLine("\nCaught unexpected {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
         }

         Assert.IsTrue(exception, "[{0}] should have been caught.", expectedException);
      }

      #endregion // ArgumentException: PathContainsInvalidCharacters

      #region ArgumentException: PathStartsWithColon

      [TestMethod]
      public void Directory_CreateDirectory_PathStartsWithColon_ArgumentException()
      {
         string tempPath = @":AAAAAAAAAA";

         const string expectedException = "System.ArgumentException";
         bool exception = false;

         try
         {
            Directory.CreateDirectory(tempPath);
         }
         catch (ArgumentException ex)
         {
            exception = true;
            Console.WriteLine("\nCaught: [{0}]\n\n[{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
         }
         catch (Exception ex)
         {
            Console.WriteLine("\nCaught unexpected {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
         }

         Assert.IsTrue(exception, "[{0}] should have been caught.", expectedException);
      }

      #endregion // ArgumentException: PathStartsWithColon


      #region DirectoryNotFoundException & IOException: NonExistingDriveLetterLocal

      // Note: isNetwork ? "System.IO.IOException" : "System.IO.DirectoryNotFoundException";

      [TestMethod]
      public void Directory_CreateDirectory_NonExistingDriveLetterLocal_DirectoryNotFoundException()
      {
         Directory_CreateDirectory_NonExistingDriveLetter_XxxException(false);
      }

      [TestMethod]
      public void Directory_CreateDirectory_NonExistingDriveLetterNetwork_IOException()
      {
         Directory_CreateDirectory_NonExistingDriveLetter_XxxException(true);
      }

      private void Directory_CreateDirectory_NonExistingDriveLetter_XxxException(bool isNetwork)
      {
         string tempPath = DriveInfo.GetFreeDriveLetter() + @":\NonExistingDriveLetter";

         if (isNetwork)
            tempPath = Path.LocalToUnc(tempPath);

         int expectedLastError = (int) (isNetwork ? Win32Errors.ERROR_BAD_NET_NAME : Win32Errors.ERROR_PATH_NOT_FOUND);
         string expectedException = isNetwork ? "System.IO.IOException" : "System.IO.DirectoryNotFoundException";
         bool exception = false;

         try
         {
            Directory.CreateDirectory(tempPath);
         }
         catch (DirectoryNotFoundException ex)
         {
            var win32Error = new Win32Exception("", ex);

            exception = true;
            Console.WriteLine("\nCaught: [{0}]\n\n[{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));

            Assert.IsTrue(ex.Message.StartsWith("(" + expectedLastError + ")"), string.Format("Expected Win32Exception error is: [{0}]", expectedLastError));

            Assert.IsTrue(win32Error.NativeErrorCode == expectedLastError, string.Format("Expected Win32Exception error should be: [{0}], got: [{1}]", expectedLastError, win32Error.NativeErrorCode));
         }
         catch (IOException ex)
         {
            exception = isNetwork;
            Console.WriteLine("\nCaught: [{0}]\n\n[{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));

            Assert.IsTrue(ex.Message.StartsWith("(" + expectedLastError + ")"), string.Format("Expected Win32Exception error is: [{0}]", expectedLastError));
         }
         catch (Exception ex)
         {
            Console.WriteLine("\nCaught unexpected {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
         }

         Assert.IsTrue(exception, "[{0}] should have been caught.", expectedException);
      }

      #endregion // DirectoryNotFoundException & IOException: NonExistingDriveLetterLocal


      #region NotSupportedException: PathContainsColon

      [TestMethod]
      public void Directory_CreateDirectory_PathContainsColonLocal_NotSupportedException()
      {
         Directory_CreateDirectory_PathContainsColon_NotSupportedException(false);
      }

      [TestMethod]
      public void Directory_CreateDirectory_PathContainsColonNetwork_NotSupportedException()
      {
         Directory_CreateDirectory_PathContainsColon_NotSupportedException(true);
      }

      private void Directory_CreateDirectory_PathContainsColon_NotSupportedException(bool isNetwork)
      {
         string colonText = ":aaa.txt";
         string tempPath = UnitTestConstants.SysDrive + @"\dev\test\"+ colonText;

         if (isNetwork)
            tempPath = Path.LocalToUnc(tempPath) + colonText;

         const string expectedException = "System.NotSupportedException";
         bool exception = false;

         try
         {
            Directory.CreateDirectory(tempPath);
         }
         catch (NotSupportedException ex)
         {
            exception = true;
            Console.WriteLine("\nCaught: [{0}]\n\n[{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
         }
         catch (Exception ex)
         {
            Console.WriteLine("\nCaught unexpected {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
         }

         Assert.IsTrue(exception, "[{0}] should have been caught.", expectedException);
      }

      #endregion // NotSupportedException: PathContainsColon


      [TestMethod]
      public void Directory_CreateDirectory_Success()
      {
         // Directory depth level.
         int level = new Random().Next(1, 1000);

#if NET35
         string emspace = "\u3000";
         string tempPath = Path.GetTempPath("Directory.CreateDirectory()-" + level + "-" + Path.GetRandomFileName() + emspace);
#else
         // MSDN: .NET 4+ Trailing spaces are removed from the end of the path parameter before deleting the directory.
         string tempPath = Path.GetTempPath("Directory.CreateDirectory()-" + level + "-" + Path.GetRandomFileName());
#endif

         try
         {
            string root = Path.Combine(tempPath, "MaxPath-Hit-The-Road");

            for (int i = 0; i < level; i++)
               root = Path.Combine(root, "-" + (i + 1) + "-subdir");
            
            Directory.CreateDirectory(root);

            Console.WriteLine("\nCreated directory structure: Depth: [{0}], path length: [{1}] characters.", level, root.Length);
            Console.WriteLine("\n{0}", root);

            Assert.IsTrue(Directory.Exists(root), "Directory should exist.");
         }
         finally
         {
            if (Directory.Exists(tempPath))
               Directory.Delete(tempPath, true);

            Assert.IsFalse(Directory.Exists(tempPath), "Cleanup failed: Directory should have been removed.");

            Console.WriteLine("\nDirectory deleted.");
         }
      }
   }
}
