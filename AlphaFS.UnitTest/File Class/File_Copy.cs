/*  Copyright (C) 2008-2016 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
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

namespace AlphaFS.UnitTest
{
   partial class FileTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>

      [TestMethod]
      public void File_Copy_LocalAndNetwork_Success()
      {
         File_Copy(false);
         File_Copy(true);
      }


      [TestMethod]
      public void File_Copy_Overwrite_DestinationFileAlreadyExists_LocalAndNetwork_Success()
      {
         File_Copy_Overwrite_DestinationFileAlreadyExists(false);
         File_Copy_Overwrite_DestinationFileAlreadyExists(true);
      }


      [TestMethod]
      public void File_Copy_CatchAlreadyExistsException_DestinationFileAlreadyExists_LocalAndNetwork_Success()
      {
         File_Copy_CatchAlreadyExistsException_DestinationFileAlreadyExists(false);
         File_Copy_CatchAlreadyExistsException_DestinationFileAlreadyExists(true);
      }


      [TestMethod]
      public void File_Copy_CatchArgumentException_PathContainsInvalidCharacters_LocalAndNetwork_Success()
      {
         File_Copy_CatchArgumentException_PathContainsInvalidCharacters(false);
         File_Copy_CatchArgumentException_PathContainsInvalidCharacters(true);
      }


      [TestMethod]
      public void File_Copy_CatchArgumentException_PathStartsWithColon_Local_Success()
      {
         File_Copy_CatchArgumentException_PathStartsWithColon(false);
      }


      [TestMethod]
      public void File_Copy_CatchDirectoryNotFoundException_NonExistingDriveLetter_LocalAndNetwork_Success()
      {
         File_Copy_CatchDirectoryNotFoundException_NonExistingDriveLetter(false);
         File_Copy_CatchDirectoryNotFoundException_NonExistingDriveLetter(true);
      }


      [TestMethod]
      public void File_Copy_CatchFileNotFoundException_NonExistingFile_LocalAndNetwork_Success()
      {
         File_Copy_CatchFileNotFoundException_NonExistingFile(false);
         File_Copy_CatchFileNotFoundException_NonExistingFile(true);
      }
      

      [TestMethod]
      public void File_Copy_CatchNotSupportedException_PathContainsColon_LocalAndNetwork_Success()
      {
         File_Copy_CatchNotSupportedException_PathContainsColon(false);
         File_Copy_CatchNotSupportedException_PathContainsColon(true);
      }


      [TestMethod]
      public void File_Copy_CatchUnauthorizedAccessException_DestinationFileIsReadOnly_LocalAndNetwork_Success()
      {
         File_Copy_CatchUnauthorizedAccessException_DestinationFileIsReadOnly(false);
         File_Copy_CatchUnauthorizedAccessException_DestinationFileIsReadOnly(true);
      }




      private void File_Copy(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         var tempPath = System.IO.Path.GetTempPath();
         if (isNetwork)
            tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);


         using (var rootDir = new TemporaryDirectory(tempPath, "File.Copy"))
         {
            // Min: 1 bytes, Max: 10485760 = 10 MB.
            var fileLength = new Random().Next(1, 10485760);
            var fileSource = UnitTestConstants.CreateFile(rootDir.Directory.FullName, fileLength);
            var fileCopy = rootDir.RandomFileFullPath + ".txt";
            Console.WriteLine("\nInput File Path: [{0}] [{1}]", Alphaleonis.Utils.UnitSizeToText(fileLength), fileSource);
            

            Alphaleonis.Win32.Filesystem.File.Copy(fileSource.FullName, fileCopy);

            Assert.IsTrue(System.IO.File.Exists(fileSource.FullName), "The file does not exists, but is expected to.");
            Assert.IsTrue(System.IO.File.Exists(fileCopy), "The file does not exists, but is expected to.");

            var fileLen = new System.IO.FileInfo(fileCopy).Length;
            Assert.AreEqual(fileLength, fileLen, "The file copy is: {0} bytes, but is expected to be: {1} bytes.", fileLen, fileLength);


            Assert.IsTrue(System.IO.File.Exists(fileSource.FullName), "The original file does not exist, but is expected to.");
         }

         Console.WriteLine();
      }


      private void File_Copy_Overwrite_DestinationFileAlreadyExists(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         var tempPath = System.IO.Path.GetTempPath();
         if (isNetwork)
            tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);


         using (var rootDir = new TemporaryDirectory(tempPath, "File.Copy"))
         {
            var fileSource = UnitTestConstants.CreateFile(rootDir.Directory.FullName);
            var fileCopy = rootDir.RandomFileFullPath + ".txt";
            Console.WriteLine("\nInput File Path: [{0}]", fileSource);

            System.IO.File.Copy(fileSource.FullName, fileCopy);
            

            var gotException = false;
            try
            {
               Alphaleonis.Win32.Filesystem.File.Copy(fileSource.FullName, fileCopy);
            }
            catch (Exception ex)
            {
               Alphaleonis.Win32.Filesystem.File.Copy(fileSource.FullName, fileCopy, true);

               var exName = ex.GetType().Name;
               gotException = exName.Equals("AlreadyExistsException", StringComparison.OrdinalIgnoreCase);
               Console.WriteLine("\n\tCaught Exception: [{0}] Message: [{1}]", exName, ex.Message);

               Assert.IsTrue(System.IO.File.Exists(fileSource.FullName), "The file does not exists, but is expected to.");
               Assert.IsTrue(System.IO.File.Exists(fileCopy), "The file does not exists, but is expected to.");
            }
            Assert.IsTrue(gotException, "The exception is not caught, but is expected to.");
         }

         Console.WriteLine();
      }


      private void File_Copy_CatchAlreadyExistsException_DestinationFileAlreadyExists(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         var tempPath = System.IO.Path.GetTempPath();
         if (isNetwork)
            tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);


         using (var rootDir = new TemporaryDirectory(tempPath, "File.Copy"))
         {
            var fileSource = UnitTestConstants.CreateFile(rootDir.Directory.FullName);
            var fileCopy = rootDir.RandomFileFullPath + ".txt";
            Console.WriteLine("\nInput File Path: [{0}]", fileSource);

            System.IO.File.Copy(fileSource.FullName, fileCopy);


            var gotException = false;
            try
            {
               Alphaleonis.Win32.Filesystem.File.Copy(fileSource.FullName, fileCopy);
            }
            catch (Exception ex)
            {
               var exName = ex.GetType().Name;
               gotException = exName.Equals("AlreadyExistsException", StringComparison.OrdinalIgnoreCase);
               Console.WriteLine("\n\tCaught Exception: [{0}] Message: [{1}]", exName, ex.Message);
            }
            Assert.IsTrue(gotException, "The exception is not caught, but is expected to.");
         }

         Console.WriteLine();
      }


      private void File_Copy_CatchArgumentException_PathContainsInvalidCharacters(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         var fileSource = System.IO.Path.GetTempPath() + @"ThisIs<My>File";
         Console.WriteLine("\nInput File Path: [{0}]", fileSource);


         var gotException = false;
         try
         {
            Alphaleonis.Win32.Filesystem.File.Copy(fileSource, string.Empty);
         }
         catch (Exception ex)
         {
            var exName = ex.GetType().Name;
            gotException = exName.Equals("ArgumentException", StringComparison.OrdinalIgnoreCase);
            Console.WriteLine("\n\tCaught Exception: [{0}] Message: [{1}]", exName, ex.Message);
         }
         Assert.IsTrue(gotException, "The exception is not caught, but is expected to.");

         Console.WriteLine();
      }


      private void File_Copy_CatchArgumentException_PathStartsWithColon(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         var fileSource = @":AAAAAAAAAA";
         Console.WriteLine("\nInput File Path: [{0}]", fileSource);


         var gotException = false;
         try
         {
            Alphaleonis.Win32.Filesystem.File.Copy(fileSource, string.Empty);
         }
         catch (Exception ex)
         {
            var exName = ex.GetType().Name;
            gotException = exName.Equals("ArgumentException", StringComparison.OrdinalIgnoreCase);
            Console.WriteLine("\n\tCaught Exception: [{0}] Message: [{1}]", exName, ex.Message);
         }
         Assert.IsTrue(gotException, "The exception is not caught, but is expected to.");

         Console.WriteLine();
      }

      
      private void File_Copy_CatchNotSupportedException_PathContainsColon(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         var colonText = @"\My:FilePath";
         var fileSource = (isNetwork ? Alphaleonis.Win32.Filesystem.Path.LocalToUnc(UnitTestConstants.LocalHostShare) : UnitTestConstants.SysDrive + @"\dev\test") + colonText;

         Console.WriteLine("\nInput File Path: [{0}]", fileSource);


         var gotException = false;
         try
         {
            Alphaleonis.Win32.Filesystem.File.Copy(fileSource, string.Empty);
         }
         catch (Exception ex)
         {
            var exName = ex.GetType().Name;
            gotException = exName.Equals("NotSupportedException", StringComparison.OrdinalIgnoreCase);
            Console.WriteLine("\n\tCaught Exception: [{0}] Message: [{1}]", exName, ex.Message);
         }
         Assert.IsTrue(gotException, "The exception is not caught, but is expected to.");

         Console.WriteLine();
      }


      private void File_Copy_CatchDirectoryNotFoundException_NonExistingDriveLetter(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         var tempPath = System.IO.Path.GetTempPath();
         if (isNetwork)
            tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);


         using (var rootDir = new TemporaryDirectory(tempPath, "File.Copy"))
         {
            var fileSource = UnitTestConstants.CreateFile(rootDir.Directory.FullName);
            var fileCopy = Alphaleonis.Win32.Filesystem.DriveInfo.GetFreeDriveLetter() + @":\NonExistingDriveLetter\" + System.IO.Path.GetRandomFileName();
            if (isNetwork)
               fileCopy = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(fileCopy);

            Console.WriteLine("\nSrc File Path: [{0}]", fileSource);
            Console.WriteLine("Dst File Path: [{0}]", fileCopy);


            var gotException = false;
            try
            {
               Alphaleonis.Win32.Filesystem.File.Copy(fileSource.FullName, fileCopy);
            }
            catch (Exception ex)
            {
               // Local: UnauthorizedAccessException.
               // UNC: IOException.

               var exName = ex.GetType().Name;
               gotException = exName.Equals(isNetwork ? "IOException" : "DirectoryNotFoundException", StringComparison.OrdinalIgnoreCase);
               Console.WriteLine("\n\tCaught Exception: [{0}] Message: [{1}]", exName, ex.Message);
            }
            Assert.IsTrue(gotException, "The exception is not caught, but is expected to.");
         }

         Console.WriteLine();
      }


      private void File_Copy_CatchFileNotFoundException_NonExistingFile(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         var fileSource = System.IO.Path.GetTempPath() + "nonExistingFileSource.txt";
         var fileCopy = System.IO.Path.GetTempPath() + "nonExistingFileCopy.txt";
         Console.WriteLine("\nInput File Path: [{0}]", fileSource);


         var gotException = false;
         try
         {
            Alphaleonis.Win32.Filesystem.File.Copy(fileSource, fileCopy);
         }
         catch (Exception ex)
         {
            var exName = ex.GetType().Name;
            gotException = exName.Equals("FileNotFoundException", StringComparison.OrdinalIgnoreCase);
            Console.WriteLine("\n\tCaught Exception: [{0}] Message: [{1}]", exName, ex.Message);
         }
         Assert.IsTrue(gotException, "The exception is not caught, but is expected to.");

         Console.WriteLine();
      }


      private void File_Copy_CatchUnauthorizedAccessException_DestinationFileIsReadOnly(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         var tempPath = System.IO.Path.GetTempPath();
         if (isNetwork)
            tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);


         using (var rootDir = new TemporaryDirectory(tempPath, "File.Copy"))
         {
            var fileSource = UnitTestConstants.CreateFile(rootDir.Directory.FullName);
            var fileCopy = rootDir.RandomFileFullPath + ".txt";
            Console.WriteLine("\nInput File Path: [{0}]", fileSource);


            System.IO.File.Copy(fileSource.FullName, fileCopy);
            System.IO.File.SetAttributes(fileCopy, System.IO.FileAttributes.ReadOnly);


            var gotException = false;
            try
            {
               Alphaleonis.Win32.Filesystem.File.Copy(fileSource.FullName, fileCopy, true);
            }
            catch (Exception ex)
            {
               var exName = ex.GetType().Name;
               gotException = exName.Equals("UnauthorizedAccessException", StringComparison.OrdinalIgnoreCase);
               Console.WriteLine("\n\tCaught Exception: [{0}] Message: [{1}]", exName, ex.Message);
            }
            Assert.IsTrue(gotException, "The exception is not caught, but is expected to.");


            System.IO.File.SetAttributes(fileCopy, System.IO.FileAttributes.Normal);
         }

         Console.WriteLine();
      }
   }
}
