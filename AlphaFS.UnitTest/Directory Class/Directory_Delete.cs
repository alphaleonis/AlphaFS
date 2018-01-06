/*  Copyright (C) 2008-2017 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
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
using System.Reflection;

namespace AlphaFS.UnitTest
{
   partial class DirectoryTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>
      
      [TestMethod]
      public void Directory_Delete_CatchArgumentException_PathContainsInvalidCharacters_LocalAndNetwork_Success()
      {
         Directory_Delete_CatchArgumentException_PathContainsInvalidCharacters(false);
         Directory_Delete_CatchArgumentException_PathContainsInvalidCharacters(true);
      }


      [TestMethod]
      public void Directory_Delete_CatchArgumentException_PathStartsWithColon_Local_Success()
      {
         Directory_Delete_CatchArgumentException_PathStartsWithColon(false);
      }


      [TestMethod]
      public void Directory_Delete_CatchDirectoryNotEmptyException_NonEmptyDirectory_LocalAndNetwork_Success()
      {
         Directory_Delete_CatchDirectoryNotEmptyException_NonEmptyDirectory(false);
         Directory_Delete_CatchDirectoryNotEmptyException_NonEmptyDirectory(true);
      }


      [TestMethod]
      public void Directory_Delete_CatchDirectoryNotFoundException_NonExistingDirectory_LocalAndNetwork_Success()
      {
         Directory_Delete_CatchDirectoryNotFoundException_NonExistingDirectory(false);
         Directory_Delete_CatchDirectoryNotFoundException_NonExistingDirectory(true);
      }


      [TestMethod]
      public void Directory_Delete_CatchDirectoryNotFoundException_NonExistingDriveLetter_LocalAndNetwork_Success()
      {
         Directory_Delete_CatchDirectoryNotFoundException_NonExistingDriveLetter(false);
         Directory_Delete_CatchDirectoryNotFoundException_NonExistingDriveLetter(true);
      }


      [TestMethod]
      public void Directory_Delete_CatchDirectoryNotFoundException_FileExistsWithSameNameAsDirectory_LocalAndNetwork_Success()
      {
         Directory_Delete_CatchDirectoryNotFoundException_FileExistsWithSameNameAsDirectory(false);
         Directory_Delete_CatchDirectoryNotFoundException_FileExistsWithSameNameAsDirectory(true);
      }


      [TestMethod]
      public void Directory_Delete_CatchDirectoryReadOnlyException_ReadOnlyDirectory_LocalAndNetwork_Success()
      {
         Directory_Delete_CatchDirectoryReadOnlyException_ReadOnlyDirectory(false);
         Directory_Delete_CatchDirectoryReadOnlyException_ReadOnlyDirectory(true);
      }


      [TestMethod]
      public void Directory_Delete_CatchNotSupportedException_PathContainsColon_LocalAndNetwork_Success()
      {
         Directory_Delete_CatchNotSupportedException_PathContainsColon(false);
         Directory_Delete_CatchNotSupportedException_PathContainsColon(true);
      }


      [TestMethod]
      public void Directory_Delete_CatchUnauthorizedAccessException_FolderWithDenyPermission_LocalAndNetwork_Success()
      {
         Directory_Delete_CatchUnauthorizedAccessException_FolderWithDenyPermission(false);
         Directory_Delete_CatchUnauthorizedAccessException_FolderWithDenyPermission(true);
      }




      private void Directory_Delete_CatchArgumentException_PathContainsInvalidCharacters(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         var folder = System.IO.Path.GetTempPath() + @"ThisIs<My>Folder";
         Console.WriteLine("\nInput Directory Path: [{0}]\n", folder);


         var gotException = false;
         try
         {
            Alphaleonis.Win32.Filesystem.Directory.Delete(folder);
         }
         catch (Exception ex)
         {
            var exName = ex.GetType().Name;
            gotException = exName.Equals("ArgumentException", StringComparison.OrdinalIgnoreCase);
            Console.WriteLine("\n\tCaught {0} Exception: [{1}] {2}", gotException ? "EXPECTED" : "UNEXPECTED", exName, ex.Message);
         }
         Assert.IsTrue(gotException, "The exception is not caught, but is expected to.");

         Console.WriteLine();
      }


      private void Directory_Delete_CatchArgumentException_PathStartsWithColon(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         var folder = @":AAAAAAAAAA";
         Console.WriteLine("\nInput Directory Path: [{0}]\n", folder);


         var gotException = false;
         try
         {
            Alphaleonis.Win32.Filesystem.Directory.Delete(folder);
         }
         catch (Exception ex)
         {
            var exName = ex.GetType().Name;
            gotException = exName.Equals("ArgumentException", StringComparison.OrdinalIgnoreCase);
            Console.WriteLine("\n\tCaught {0} Exception: [{1}] {2}", gotException ? "EXPECTED" : "UNEXPECTED", exName, ex.Message);
         }
         Assert.IsTrue(gotException, "The exception is not caught, but is expected to.");

         Console.WriteLine();
      }


      private void Directory_Delete_CatchNotSupportedException_PathContainsColon(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         var colonText = @"\My:FolderPath";
         var folder = (isNetwork ? Alphaleonis.Win32.Filesystem.Path.LocalToUnc(UnitTestConstants.TempFolder) : UnitTestConstants.SysDrive + @"\dev\test") + colonText;

         Console.WriteLine("\nInput Directory Path: [{0}]\n", folder);


         var gotException = false;
         try
         {
            Alphaleonis.Win32.Filesystem.Directory.Delete(folder);
         }
         catch (Exception ex)
         {
            var exName = ex.GetType().Name;
            gotException = exName.Equals("NotSupportedException", StringComparison.OrdinalIgnoreCase);
            Console.WriteLine("\n\tCaught {0} Exception: [{1}] {2}", gotException ? "EXPECTED" : "UNEXPECTED", exName, ex.Message);
         }
         Assert.IsTrue(gotException, "The exception is not caught, but is expected to.");

         Console.WriteLine();
      }


      private void Directory_Delete_CatchDirectoryNotEmptyException_NonEmptyDirectory(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         var tempPath = System.IO.Path.GetTempPath();
         if (isNetwork)
            tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);


         using (var rootDir = new TemporaryDirectory(tempPath, MethodBase.GetCurrentMethod().Name))
         {
            var folder = rootDir.RandomDirectoryFullPath;
            var file = System.IO.Path.Combine(folder, System.IO.Path.GetRandomFileName());

            Console.WriteLine("\nInput Directory Path: [{0}]", folder);
            Console.WriteLine("Input File Path     : [{0}]\n", file);

            System.IO.Directory.CreateDirectory(folder);
            using (System.IO.File.Create(System.IO.Path.Combine(folder, file))) { }
            


            var gotException = false;
            try
            {
               Alphaleonis.Win32.Filesystem.Directory.Delete(folder);

            }
            catch (Exception ex)
            {
               var exName = ex.GetType().Name;
               gotException = exName.Equals("DirectoryNotEmptyException", StringComparison.OrdinalIgnoreCase);
               Console.WriteLine("\n\tCaught {0} Exception: [{1}] {2}", gotException ? "EXPECTED" : "UNEXPECTED", exName, ex.Message);
            }
            Assert.IsTrue(gotException, "The exception is not caught, but is expected to.");
         }

         Console.WriteLine();
      }


      private void Directory_Delete_CatchDirectoryNotFoundException_NonExistingDirectory(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         var tempPath = System.IO.Path.GetTempPath() + "Directory.Delete-" + System.IO.Path.GetRandomFileName();
         if (isNetwork)
            tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);

         Console.WriteLine("\nInput Directory Path: [{0}]\n", tempPath);


         var gotException = false;
         try
         {
            Alphaleonis.Win32.Filesystem.Directory.Delete(tempPath);
         }
         catch (Exception ex)
         {
            var exName = ex.GetType().Name;
            gotException = exName.Equals("DirectoryNotFoundException", StringComparison.OrdinalIgnoreCase);
            Console.WriteLine("\n\tCaught {0} Exception: [{1}] {2}", gotException ? "EXPECTED" : "UNEXPECTED", exName, ex.Message);
         }
         Assert.IsTrue(gotException, "The exception is not caught, but is expected to.");

         Console.WriteLine();
      }


      private void Directory_Delete_CatchDirectoryNotFoundException_NonExistingDriveLetter(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         var folder = Alphaleonis.Win32.Filesystem.DriveInfo.GetFreeDriveLetter() + @":\NonExistingDriveLetter";
         if (isNetwork)
            folder = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(folder);

         Console.WriteLine("\nInput Directory Path: [{0}]\n", folder);


         var gotException = false;
         try
         {
            Alphaleonis.Win32.Filesystem.Directory.Delete(folder);
         }
         catch (Exception ex)
         {
            // Local: DirectoryNotFoundException.
            // UNC: IOException.

            var exName = ex.GetType().Name;
            gotException = exName.Equals(isNetwork ? "IOException" : "DirectoryNotFoundException", StringComparison.OrdinalIgnoreCase);
            Console.WriteLine("\n\tCaught {0} Exception: [{1}] {2}", gotException ? "EXPECTED" : "UNEXPECTED", exName, ex.Message);
         }
         Assert.IsTrue(gotException, "The exception is not caught, but is expected to.");

         Console.WriteLine();
      }


      private void Directory_Delete_CatchDirectoryNotFoundException_FileExistsWithSameNameAsDirectory(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         var tempPath = System.IO.Path.GetTempPath();
         if (isNetwork)
            tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);


         using (var rootDir = new TemporaryDirectory(tempPath, MethodBase.GetCurrentMethod().Name))
         {
            var file = rootDir.RandomFileFullPath;
            Console.WriteLine("\nInput File Path: [{0}]\n", file);

            using (System.IO.File.Create(file)) { }


            var gotException = false;
            try
            {
               Alphaleonis.Win32.Filesystem.Directory.Delete(file);

            }
            catch (Exception ex)
            {
               var exName = ex.GetType().Name;
               gotException = exName.Equals("DirectoryNotFoundException", StringComparison.OrdinalIgnoreCase);
               Console.WriteLine("\n\tCaught {0} Exception: [{1}] {2}", gotException ? "EXPECTED" : "UNEXPECTED", exName, ex.Message);
            }
            Assert.IsTrue(gotException, "The exception is not caught, but is expected to.");
         }

         Console.WriteLine();
      }


      private void Directory_Delete_CatchDirectoryReadOnlyException_ReadOnlyDirectory(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         var tempPath = System.IO.Path.GetTempPath();
         if (isNetwork)
            tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);


         using (var rootDir = new TemporaryDirectory(tempPath, MethodBase.GetCurrentMethod().Name))
         {
            var folder = rootDir.RandomDirectoryFullPath;
            Console.WriteLine("\nInput Directory Path: [{0}]\n", folder);

            System.IO.Directory.CreateDirectory(folder);
            System.IO.File.SetAttributes(folder, System.IO.FileAttributes.ReadOnly);


            var gotException = false;
            try
            {
               Alphaleonis.Win32.Filesystem.Directory.Delete(folder);

            }
            catch (Exception ex)
            {
               var exName = ex.GetType().Name;
               gotException = exName.Equals("DirectoryReadOnlyException", StringComparison.OrdinalIgnoreCase);
               Console.WriteLine("\n\tCaught {0} Exception: [{1}] {2}", gotException ? "EXPECTED" : "UNEXPECTED", exName, ex.Message);
            }
            Assert.IsTrue(gotException, "The exception is not caught, but is expected to.");


            System.IO.File.SetAttributes(folder, System.IO.FileAttributes.Normal);
         }

         Console.WriteLine();
      }


      private void Directory_Delete_CatchUnauthorizedAccessException_FolderWithDenyPermission(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         var tempPath = System.IO.Path.GetTempPath();
         if (isNetwork)
            tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);


         using (var rootDir = new TemporaryDirectory(tempPath, MethodBase.GetCurrentMethod().Name))
         {
            var folder = rootDir.RandomDirectoryFullPath;
            Console.WriteLine("\nInput Directory Path: [{0}]\n", folder);

            System.IO.Directory.CreateDirectory(folder);

            // Create a temp folder and set DENY permission for current user.
            UnitTestConstants.FolderDenyPermission(true, folder);


            var gotException = false;
            try
            {
               Alphaleonis.Win32.Filesystem.Directory.Delete(folder);

            }
            catch (Exception ex)
            {
               var exName = ex.GetType().Name;
               gotException = exName.Equals("UnauthorizedAccessException", StringComparison.OrdinalIgnoreCase);
               Console.WriteLine("\n\tCaught {0} Exception: [{1}] {2}", gotException ? "EXPECTED" : "UNEXPECTED", exName, ex.Message);
            }
            Assert.IsTrue(gotException, "The exception is not caught, but is expected to.");


            // Remove DENY permission for current user and delete folder.
            UnitTestConstants.FolderDenyPermission(false, folder);

         }

         Console.WriteLine();
      }
   }
}
