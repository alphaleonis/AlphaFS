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
   partial class FileTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>

      [TestMethod]
      public void File_Delete_LocalAndNetwork_Success()
      {
         File_Delete(false);
         File_Delete(true);
      }


      [TestMethod]
      public void File_Delete_CatchArgumentException_PathContainsInvalidCharacters_LocalAndNetwork_Success()
      {
         File_Delete_CatchArgumentException_PathContainsInvalidCharacters(false);
         File_Delete_CatchArgumentException_PathContainsInvalidCharacters(true);
      }


      [TestMethod]
      public void File_Delete_CatchArgumentException_PathStartsWithColon_Local_Success()
      {
         File_Delete_CatchArgumentException_PathStartsWithColon(false);
      }


      [TestMethod]
      public void File_Delete_CatchDirectoryNotFoundException_NonExistingDriveLetter_LocalAndNetwork_Success()
      {
         File_Delete_CatchDirectoryNotFoundException_NonExistingDriveLetter(false);
         File_Delete_CatchDirectoryNotFoundException_NonExistingDriveLetter(true);
      }


      [TestMethod]
      public void File_Delete_CatchFileNotFoundException_NonExistingFile_LocalAndNetwork_Success()
      {
         File_Delete_CatchFileNotFoundException_NonExistingFile(false);
         File_Delete_CatchFileNotFoundException_NonExistingFile(true);
      }
      

      [TestMethod]
      public void File_Delete_CatchFileReadOnlyException_ReadOnlyFile_LocalAndNetwork_Success()
      {
         File_Delete_CatchFileReadOnlyException_ReadOnlyFile(false);
         File_Delete_CatchFileReadOnlyException_ReadOnlyFile(true);
      }


      [TestMethod]
      public void File_Delete_CatchNotSupportedException_PathContainsColon_LocalAndNetwork_Success()
      {
         File_Delete_CatchNotSupportedException_PathContainsColon(false);
         File_Delete_CatchNotSupportedException_PathContainsColon(true);
      }


      [TestMethod]
      public void File_Delete_CatchUnauthorizedAccessException_PathIsADirectoryNotAFile_LocalAndNetwork_Success()
      {
         File_Delete_CatchUnauthorizedAccessException_PathIsADirectoryNotAFile(false);
         File_Delete_CatchUnauthorizedAccessException_PathIsADirectoryNotAFile(true);
      }




      private void File_Delete(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         var tempPath = System.IO.Path.GetTempPath();
         if (isNetwork)
            tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);


         using (var rootDir = new TemporaryDirectory(tempPath, MethodBase.GetCurrentMethod().Name))
         {
            var file = UnitTestConstants.CreateFile(rootDir.Directory.FullName);
            Console.WriteLine("\nInput File Path: [{0}]", file);
            

            Alphaleonis.Win32.Filesystem.File.Delete(file.FullName);

            Assert.IsFalse(Alphaleonis.Win32.Filesystem.File.Exists(file.FullName), "The file exists, but is expected not to be.");
         }

         Console.WriteLine();
      }


      private void File_Delete_CatchArgumentException_PathContainsInvalidCharacters(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         var file = System.IO.Path.GetTempPath() + @"ThisIs<My>File";
         Console.WriteLine("\nInput File Path: [{0}]", file);


         var gotException = false;
         try
         {
            Alphaleonis.Win32.Filesystem.File.Delete(file);
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


      private void File_Delete_CatchArgumentException_PathStartsWithColon(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         var folder = @":AAAAAAAAAA";
         Console.WriteLine("\nInput File Path: [{0}]", folder);


         var gotException = false;
         try
         {
            Alphaleonis.Win32.Filesystem.File.Delete(folder);
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


      private void File_Delete_CatchNotSupportedException_PathContainsColon(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         var colonText = @"\My:FilePath";
         var folder = (isNetwork ? Alphaleonis.Win32.Filesystem.Path.LocalToUnc(UnitTestConstants.LocalHostShare) : UnitTestConstants.SysDrive + @"\dev\test") + colonText;

         Console.WriteLine("\nInput File Path: [{0}]", folder);


         var gotException = false;
         try
         {
            Alphaleonis.Win32.Filesystem.File.Delete(folder);
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


      private void File_Delete_CatchDirectoryNotFoundException_NonExistingDriveLetter(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         var folder = Alphaleonis.Win32.Filesystem.DriveInfo.GetFreeDriveLetter() + @":\NonExistingDriveLetter";
         if (isNetwork)
            folder = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(folder);

         Console.WriteLine("\nInput File Path: [{0}]", folder);


         var gotException = false;
         try
         {
            Alphaleonis.Win32.Filesystem.File.Delete(folder);
         }
         catch (Exception ex)
         {
            // Local: UnauthorizedAccessException.
            // UNC: IOException.

            var exName = ex.GetType().Name;
            gotException = exName.Equals(isNetwork ? "IOException" : "DirectoryNotFoundException", StringComparison.OrdinalIgnoreCase);
            Console.WriteLine("\n\tCaught {0} Exception: [{1}] {2}", gotException ? "EXPECTED" : "UNEXPECTED", exName, ex.Message);
         }
         Assert.IsTrue(gotException, "The exception is not caught, but is expected to.");

         Console.WriteLine();
      }


      private void File_Delete_CatchFileNotFoundException_NonExistingFile(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         var tempPath = System.IO.Path.GetTempPath() + "File.Delete-" + System.IO.Path.GetRandomFileName();
         if (isNetwork)
            tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);

         Console.WriteLine("\nInput File Path: [{0}]", tempPath);


         var gotException = false;
         try
         {
            Alphaleonis.Win32.Filesystem.File.Delete(tempPath);
         }
         catch (Exception ex)
         {
            Assert.IsTrue(gotException, "An exception occurred, but is expected not to: " + ex.Message);
         }

         Console.WriteLine();
      }
      

      private void File_Delete_CatchUnauthorizedAccessException_PathIsADirectoryNotAFile(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         var tempPath = System.IO.Path.GetTempPath();
         if (isNetwork)
            tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);


         using (var rootDir = new TemporaryDirectory(tempPath, MethodBase.GetCurrentMethod().Name))
         {
            var folder = rootDir.RandomFileFullPath;
            Console.WriteLine("\nInput Directory Path: [{0}]", folder);

            System.IO.Directory.CreateDirectory(folder);


            var gotException = false;
            try
            {
               Alphaleonis.Win32.Filesystem.File.Delete(folder);
            }
            catch (Exception ex)
            {
               var exName = ex.GetType().Name;
               gotException = exName.Equals("UnauthorizedAccessException", StringComparison.OrdinalIgnoreCase);
               Console.WriteLine("\n\tCaught {0} Exception: [{1}] {2}", gotException ? "EXPECTED" : "UNEXPECTED", exName, ex.Message);
            }
            Assert.IsTrue(gotException, "The exception is not caught, but is expected to.");
         }

         Console.WriteLine();
      }


      private void File_Delete_CatchFileReadOnlyException_ReadOnlyFile(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         var tempPath = System.IO.Path.GetTempPath();
         if (isNetwork)
            tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);


         using (var rootDir = new TemporaryDirectory(tempPath, MethodBase.GetCurrentMethod().Name))
         {
            var file = UnitTestConstants.CreateFile(rootDir.Directory.FullName);
            Console.WriteLine("\nInput File Path: [{0}]", file);

            System.IO.File.SetAttributes(file.FullName, System.IO.FileAttributes.ReadOnly);


            var gotException = false;
            try
            {
               Alphaleonis.Win32.Filesystem.File.Delete(file.FullName);

            }
            catch (Exception ex)
            {
               var exName = ex.GetType().Name;
               gotException = exName.Equals("FileReadOnlyException", StringComparison.OrdinalIgnoreCase);
               Console.WriteLine("\n\tCaught {0} Exception: [{1}] {2}", gotException ? "EXPECTED" : "UNEXPECTED", exName, ex.Message);
            }
            Assert.IsTrue(gotException, "The exception is not caught, but is expected to.");


            System.IO.File.SetAttributes(file.FullName, System.IO.FileAttributes.Normal);
         }

         Console.WriteLine();
      }
   }
}
