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
using System.Security.AccessControl;
using Alphaleonis.Win32;
using Alphaleonis.Win32.Filesystem;
using Directory = Alphaleonis.Win32.Filesystem.Directory;
using DirectoryInfo = Alphaleonis.Win32.Filesystem.DirectoryInfo;
using DriveInfo = Alphaleonis.Win32.Filesystem.DriveInfo;
using File = System.IO.File;
using OperatingSystem = Alphaleonis.Win32.OperatingSystem;
using Path = Alphaleonis.Win32.Filesystem.Path;

namespace AlphaFS.UnitTest
{
   partial class DirectoryTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>

      #region ArgumentException: PathContainsInvalidCharacters

      [TestMethod]
      public void Directory_Delete_PathContainsInvalidCharacters_ArgumentException()
      {
         const string expectedException = "System.ArgumentException";
         bool exception = false;

         try
         {
            Directory.Delete(UnitTestConstants.SysDrive + @"\<>");
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
      public void Directory_Delete_PathStartsWithColon_ArgumentException()
      {
         string tempPath = @":AAAAAAAAAA";

         const string expectedException = "System.ArgumentException";
         bool exception = false;

         try
         {
            Directory.Delete(tempPath);
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


      #region DirectoryNotFoundException: NonExistingDirectory

      [TestMethod]
      public void Directory_Delete_NonExistingDirectoryLocal_DirectoryNotFoundException()
      {
         Directory_Delete_NonExistingDirectory_DirectoryNotFoundException(false);
      }

      [TestMethod]
      public void Directory_Delete_NonExistingDirectoryNetwork_DirectoryNotFoundException()
      {
         Directory_Delete_NonExistingDirectory_DirectoryNotFoundException(true);
      }

      private void Directory_Delete_NonExistingDirectory_DirectoryNotFoundException(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         string tempPath = Path.GetTempPath("Directory-Delete-NonExistingDirectory-" + Path.GetRandomFileName());

         if (isNetwork)
            tempPath = Path.LocalToUnc(tempPath);

         int expectedLastError = (int) Win32Errors.ERROR_PATH_NOT_FOUND;
         string expectedException = "System.IO.DirectoryNotFoundException";

         int expectedWin32LastError = (int) Win32Errors.ERROR_FILE_NOT_FOUND;
         bool exception = false;

         try
         {
            Directory.Delete(tempPath);
         }
         catch (DirectoryNotFoundException ex)
         {
            var win32Error = new Win32Exception("", ex);

            exception = true;
            Console.WriteLine("\nCaught: [{0}]\n\n[{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));

            Assert.IsTrue(ex.Message.StartsWith("(" + expectedLastError + ")"), string.Format("Expected Win32Exception error is: [{0}]", expectedLastError));

            Assert.IsTrue(win32Error.NativeErrorCode == expectedWin32LastError, string.Format("Expected Win32Exception error should be: [{0}], got: [{1}]", expectedWin32LastError, win32Error.NativeErrorCode));
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

      #endregion // DirectoryNotFoundException: NonExistingDirectory

      #region DirectoryNotFoundException: NonExistingDriveLetter

      [TestMethod]
      public void Directory_Delete_NonExistingDriveLetterLocal_DirectoryNotFoundException()
      {
         Directory_Delete_NonExistingDriveLetter_XxxException(false);
      }

      [TestMethod]
      public void Directory_Delete_NonExistingDriveLetterNetwork_IOException()
      {
         Directory_Delete_NonExistingDriveLetter_XxxException(true);
      }

      private void Directory_Delete_NonExistingDriveLetter_XxxException(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         string tempPath = UnitTestConstants.SysRoot32 + @"\NonExistingDriveLetter-" + Path.GetRandomFileName();
         string letter = DriveInfo.GetFreeDriveLetter() + @":\";

         if (isNetwork)
         {
            letter = Path.LocalToUnc(letter);
            tempPath = Path.LocalToUnc(tempPath.Replace(UnitTestConstants.SysDrive + @"\", letter));
         }

         int expectedLastError = (int) (isNetwork ? Win32Errors.ERROR_BAD_NET_NAME : Win32Errors.ERROR_PATH_NOT_FOUND);
         string expectedException = isNetwork ? "System.IO.IOException" : "System.IO.DirectoryNotFoundException";

         int expectedWin32Error = (int)Win32Errors.ERROR_FILE_NOT_FOUND;
         bool exception = false;

         try
         {
            Directory.Delete(tempPath);
         }
         catch (DirectoryNotFoundException ex)
         {
            var win32Error = new Win32Exception("", ex);

            exception = true;
            Console.WriteLine("\nCaught: [{0}]\n\n[{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));

            Assert.IsTrue(ex.Message.StartsWith("(" + expectedLastError + ")"), string.Format("Expected Win32Exception error is: [{0}]", expectedLastError));

            Assert.IsTrue(win32Error.NativeErrorCode == expectedWin32Error, string.Format("Expected Win32Exception error should be: [{0}], got: [{1}]", expectedWin32Error, win32Error.NativeErrorCode));
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

      #endregion // DirectoryNotFoundException: NonExistingDriveLetter

      #region DirectoryNotFoundException: PathIsAFileNotADirectory

      [TestMethod]
      public void Directory_Delete_PathIsAFileNotADirectoryLocal_DirectoryNotFoundException()
      {
         Directory_Delete_PathIsAFileNotADirectory_DirectoryNotFoundException(false);
      }

      [TestMethod]
      public void Directory_Delete_PathIsAFileNotADirectoryNetwork_DirectoryNotFoundException()
      {
         Directory_Delete_PathIsAFileNotADirectory_DirectoryNotFoundException(true);
      }

      private void Directory_Delete_PathIsAFileNotADirectory_DirectoryNotFoundException(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         string tempPath = Path.GetTempPath("Directory-Delete-PathIsAFileNotADirectory-" + Path.GetRandomFileName());

         if (isNetwork)
            tempPath = Path.LocalToUnc(tempPath);

         int expectedLastError = (int) Win32Errors.ERROR_INVALID_PARAMETER;
         string expectedException = "System.IO.DirectoryNotFoundException";

         int expectedWin32Error = (int)Win32Errors.ERROR_DIRECTORY;
         bool exception = false;

         try
         {
            using (File.Create(tempPath)) {}

            Directory.Delete(tempPath);
         }
         catch (DirectoryNotFoundException ex)
         {
            var win32Error = new Win32Exception("", ex);

            exception = true;
            Console.WriteLine("\nCaught: [{0}]\n\n[{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));

            Assert.IsTrue(ex.Message.StartsWith("(" + expectedLastError + ")"), string.Format("Expected Win32Exception error is: [{0}]", expectedLastError));

            Assert.IsTrue(win32Error.NativeErrorCode == expectedWin32Error, string.Format("Expected Win32Exception error should be: [{0}], got: [{1}]", expectedWin32Error, win32Error.NativeErrorCode));
         }
         catch (Exception ex)
         {
            Console.WriteLine("\nCaught unexpected {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
         }
         finally
         {
            if (File.Exists(tempPath))
               File.Delete(tempPath);

            Assert.IsFalse(File.Exists(tempPath), "Cleanup failed: File should have been removed.");
         }
         Assert.IsTrue(exception, "[{0}] should have been caught.", expectedException);
      }

      #endregion // DirectoryNotFoundException: PathIsAFileNotADirectory


      #region DirectoryNotEmptyException

      [TestMethod]
      public void Directory_Delete_NonEmptyDirectoryLocal_DirectoryNotEmptyException()
      {
         Directory_Delete_NonEmptyDirectory_DirectoryNotEmptyException(false);
      }

      [TestMethod]
      public void Directory_Delete_NonEmptyDirectoryNetwork_DirectoryNotEmptyException()
      {
         Directory_Delete_NonEmptyDirectory_DirectoryNotEmptyException(true);
      }

      private void Directory_Delete_NonEmptyDirectory_DirectoryNotEmptyException(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         string tempPath = Path.GetTempPath("Directory-Delete-NonEmptyDirectory-" + Path.GetRandomFileName());

         if (isNetwork)
            tempPath = Path.LocalToUnc(tempPath);

         int expectedLastError = (int)Win32Errors.ERROR_DIR_NOT_EMPTY;
         string expectedException = "System.IO.IOException";
         bool exception = false;

         try
         {
            Directory.CreateDirectory(tempPath);

            using (File.Create(Path.Combine(tempPath, "created-a-file.txt"))) { }

            Directory.Delete(tempPath);
         }
         catch (DirectoryNotEmptyException ex)
         {
            exception = true;
            Console.WriteLine("\nCaught: [{0}]\n\n[{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));

            Assert.IsTrue(ex.Message.StartsWith("(" + expectedLastError + ")"), string.Format("Expected Win32Exception error is: [{0}]", expectedLastError));
         }
         catch (Exception ex)
         {
            Console.WriteLine("\nCaught unexpected {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
         }
         finally
         {
            if (Directory.Exists(tempPath))
               Directory.Delete(tempPath, true);

            Assert.IsFalse(Directory.Exists(tempPath), "Cleanup failed: Directory should have been removed.");
         }
         Assert.IsTrue(exception, "[{0}] should have been caught.", expectedException);
      }

      #endregion // DirectoryNotEmptyException

      #region DirectoryReadOnlyException

      [TestMethod]
      public void Directory_Delete_ReadOnlyDirectoryLocal_DirectoryReadOnlyException()
      {
         Directory_Delete_ReadOnlyDirectory_DirectoryReadOnlyException(false);
      }

      [TestMethod]
      public void Directory_Delete_ReadOnlyDirectoryNetwork_DirectoryReadOnlyException()
      {
         Directory_Delete_ReadOnlyDirectory_DirectoryReadOnlyException(true);
      }

      private void Directory_Delete_ReadOnlyDirectory_DirectoryReadOnlyException(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         string tempPath = Path.GetTempPath("Directory-Delete-DirectoryReadOnlyException-" + Path.GetRandomFileName());

         if (isNetwork)
            tempPath = Path.LocalToUnc(tempPath);

         int expectedLastError = (int) Win32Errors.ERROR_FILE_READ_ONLY;
         string expectedException = "System.IO.IOException";

         int expectedWin32Error = (int) Win32Errors.ERROR_ACCESS_DENIED;
         bool exception = false;

         try
         {
            Directory.CreateDirectory(tempPath);

            File.SetAttributes(tempPath, FileAttributes.ReadOnly);

            Directory.Delete(tempPath);
         }
         catch (DirectoryReadOnlyException ex)
         {
            var win32Error = new Win32Exception("", ex);

            exception = true;
            Console.WriteLine("\nCaught: [{0}]\n\n[{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));

            Assert.IsTrue(ex.Message.StartsWith("(" + expectedLastError + ")"), string.Format("Expected Win32Exception error is: [{0}]", expectedLastError));

            Assert.IsTrue(win32Error.NativeErrorCode == expectedWin32Error, string.Format("Expected Win32Exception error should be: [{0}], got: [{1}]", expectedWin32Error, win32Error.NativeErrorCode));
         }
         catch (Exception ex)
         {
            Console.WriteLine("\nCaught unexpected {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
         }
         finally
         {
            if (Directory.Exists(tempPath))
               Directory.Delete(tempPath, true, true);

            Assert.IsFalse(Directory.Exists(tempPath), "Cleanup failed: Directory should have been removed.");
         }
         Assert.IsTrue(exception, "[{0}] should have been caught.", expectedException);
      }

      #endregion // DirectoryReadOnlyException


      #region FileReadOnlyException

      [TestMethod]
      public void Directory_Delete_DirectoryContainsAReadOnlyFileLocal_FileReadOnlyException()
      {
         Directory_Delete_DirectoryContainsAReadOnlyFile_FileReadOnlyException(false);
      }

      [TestMethod]
      public void Directory_Delete_DirectoryContainsAReadOnlyFileNetwork_FileReadOnlyException()
      {
         Directory_Delete_DirectoryContainsAReadOnlyFile_FileReadOnlyException(true);
      }

      private void Directory_Delete_DirectoryContainsAReadOnlyFile_FileReadOnlyException(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         string tempPath = Path.GetTempPath("Directory-Delete-FileReadOnlyException-" + Path.GetRandomFileName());

         if (isNetwork)
            tempPath = Path.LocalToUnc(tempPath);

         int expectedLastError = (int) Win32Errors.ERROR_FILE_READ_ONLY;
         string expectedException = "System.IO.IOException";

         int expectedWin32Error = (int) Win32Errors.ERROR_ACCESS_DENIED;
         bool exception = false;

         try
         {
            Directory.CreateDirectory(tempPath);

            string readOnlyFile = Path.Combine(tempPath, "created-a-read-only-file.txt");
            using (File.Create(readOnlyFile)) { }
            File.SetAttributes(readOnlyFile, FileAttributes.ReadOnly);

            Directory.Delete(tempPath, true);
         }
         catch (FileReadOnlyException ex)
         {
            var win32Error = new Win32Exception("", ex);

            exception = true;
            Console.WriteLine("\nCaught: [{0}]\n\n[{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));

            Assert.IsTrue(ex.Message.StartsWith("(" + expectedLastError + ")"), string.Format("Expected Win32Exception error is: [{0}]", expectedLastError));

            Assert.IsTrue(win32Error.NativeErrorCode == expectedWin32Error, string.Format("Expected Win32Exception error should be: [{0}], got: [{1}]", expectedWin32Error, win32Error.NativeErrorCode));
         }
         catch (Exception ex)
         {
            Console.WriteLine("\nCaught unexpected {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
         }
         finally
         {
            if (Directory.Exists(tempPath))
               Directory.Delete(tempPath, true, true);

            Assert.IsFalse(Directory.Exists(tempPath), "Cleanup failed: Directory should have been removed.");
         }
         Assert.IsTrue(exception, "[{0}] should have been caught.", expectedException);
      }

      #endregion // FileReadOnlyException


      #region NotSupportedException: PathContainsColon

      [TestMethod]
      public void Directory_Delete_PathContainsColonLocal_NotSupportedException()
      {
         Directory_Delete_PathContainsColon_NotSupportedException(false);
      }

      [TestMethod]
      public void Directory_Delete_PathContainsColonNetwork_NotSupportedException()
      {
         Directory_Delete_PathContainsColon_NotSupportedException(true);
      }

      private void Directory_Delete_PathContainsColon_NotSupportedException(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         string tempPath = UnitTestConstants.SysDrive + @"\dev\test\aaa:aaa.txt";

         if (isNetwork)
            tempPath = Path.LocalToUnc(tempPath) + ":aaa.txt";

         const string expectedException = "System.NotSupportedException";
         bool exception = false;

         try
         {
            Directory.Delete(tempPath);
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


      #region UnauthorizedAccessException: FolderWithDenyPermission

      [TestMethod]
      public void Directory_Delete_FolderWithDenyPermissionLocal_UnauthorizedAccessException()
      {
         Directory_Delete_FolderWithDenyPermission_UnauthorizedAccessException(false);
      }

      [TestMethod]
      public void Directory_Delete_FolderWithDenyPermissionNetwork_UnauthorizedAccessException()
      {
         Directory_Delete_FolderWithDenyPermission_UnauthorizedAccessException(true);
      }

      private void Directory_Delete_FolderWithDenyPermission_UnauthorizedAccessException(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         string tempPath = Path.GetTempPath("Directory-Delete-FolderWithDenyPermission-" + Path.GetRandomFileName());

         if (isNetwork)
            tempPath = Path.LocalToUnc(tempPath);

         // Create a temp folder and set DENY permission for current user.
         UnitTestConstants.FolderWithDenyPermission(true, isNetwork, tempPath);

         
         int expectedLastError = (int) Win32Errors.ERROR_ACCESS_DENIED;
         string expectedException = "System.UnauthorizedAccessException";
         bool exception = false;

         try
         {
            Directory.Delete(tempPath);
         }
         catch (UnauthorizedAccessException ex)
         {
            exception = true;
            Console.WriteLine("\nCaught: [{0}]\n\n[{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));

            Assert.IsTrue(ex.Message.StartsWith("(" + expectedLastError + ")"), string.Format("Expected Win32Exception error is: [{0}]", expectedLastError));
         }
         catch (Exception ex)
         {
            Console.WriteLine("\nCaught unexpected {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
         }
         finally
         {
            // Remove DENY permission for current user and delete folder.
            UnitTestConstants.FolderWithDenyPermission(false, isNetwork, tempPath);
         }

         Assert.IsTrue(exception, "[{0}] should have been caught.", expectedException);
      }

      #endregion // UnauthorizedAccessException: FolderWithDenyPermission


      [TestMethod]
      public void Directory_Delete_Local_Success()
      {
         Directory_CreateDirectory(false);
      }

      [TestMethod]
      public void Directory_Delete_Network_Success()
      {
         Directory_CreateDirectory(true);
      }
   }
}
