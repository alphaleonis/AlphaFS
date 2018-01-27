using System;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AlphaFS.UnitTest
{
   public partial class File_CopyTest
   {
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


         using (var rootDir = new TemporaryDirectory(tempPath, MethodBase.GetCurrentMethod().Name))
         {
            // Min: 1 bytes, Max: 10485760 = 10 MB.
            var fileLength = new Random().Next(1, 10485760);
            var fileSource = UnitTestConstants.CreateFile(rootDir.Directory.FullName, fileLength);
            var fileCopy = rootDir.RandomFileFullPath;

            Console.WriteLine("\nInput  Source      File Path: [{0}] [{1}]",
               Alphaleonis.Utils.UnitSizeToText(fileLength), fileSource);
            Console.WriteLine("\nOutput Destination File Path: [{0}]", fileCopy);


            Alphaleonis.Win32.Filesystem.File.Copy(fileSource.FullName, fileCopy);

            Assert.IsTrue(System.IO.File.Exists(fileSource.FullName), "The file does not exists, but is expected to.");
            Assert.IsTrue(System.IO.File.Exists(fileCopy), "The file does not exists, but is expected to.");

            var fileLen = new System.IO.FileInfo(fileCopy).Length;
            Assert.AreEqual(fileLength, fileLen, "The file copy is: {0} bytes, but is expected to be: {1} bytes.",
               fileLen, fileLength);


            Assert.IsTrue(System.IO.File.Exists(fileSource.FullName),
               "The original file does not exist, but is expected to.");
         }

         Console.WriteLine();
      }

      private void File_Copy_Overwrite_DestinationFileAlreadyExists(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         var tempPath = System.IO.Path.GetTempPath();
         if (isNetwork)
            tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);


         using (var rootDir = new TemporaryDirectory(tempPath, MethodBase.GetCurrentMethod().Name))
         {
            var fileSource = UnitTestConstants.CreateFile(rootDir.Directory.FullName);
            var fileCopy = rootDir.RandomFileFullPath;
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
               Console.WriteLine("\n\tCaught {0} Exception: [{1}] {2}", gotException ? "EXPECTED" : "UNEXPECTED",
                  exName, ex.Message);

               Assert.IsTrue(System.IO.File.Exists(fileSource.FullName),
                  "The file does not exists, but is expected to.");
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


         using (var rootDir = new TemporaryDirectory(tempPath, MethodBase.GetCurrentMethod().Name))
         {
            var fileSource = UnitTestConstants.CreateFile(rootDir.Directory.FullName);
            var fileCopy = rootDir.RandomFileFullPath;
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
               Console.WriteLine("\n\tCaught {0} Exception: [{1}] {2}", gotException ? "EXPECTED" : "UNEXPECTED",
                  exName, ex.Message);
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
            Alphaleonis.Win32.Filesystem.File.Copy(fileSource, "does_not_matter_for_this_test");
         }
         catch (Exception ex)
         {
            var exName = ex.GetType().Name;
            gotException = exName.Equals("ArgumentException", StringComparison.OrdinalIgnoreCase);
            Console.WriteLine("\n\tCaught {0} Exception: [{1}] {2}", gotException ? "EXPECTED" : "UNEXPECTED", exName,
               ex.Message);
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
            Console.WriteLine("\n\tCaught {0} Exception: [{1}] {2}", gotException ? "EXPECTED" : "UNEXPECTED", exName,
               ex.Message);
         }

         Assert.IsTrue(gotException, "The exception is not caught, but is expected to.");

         Console.WriteLine();
      }

      private void File_Copy_CatchNotSupportedException_PathContainsColon(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         var colonText = @"\My:FilePath";
         var fileSource =
         (isNetwork
            ? Alphaleonis.Win32.Filesystem.Path.LocalToUnc(UnitTestConstants.TempFolder)
            : UnitTestConstants.TempFolder + @"\dev\test") + colonText;

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
            Console.WriteLine("\n\tCaught {0} Exception: [{1}] {2}", gotException ? "EXPECTED" : "UNEXPECTED", exName,
               ex.Message);
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


         using (var rootDir = new TemporaryDirectory(tempPath, MethodBase.GetCurrentMethod().Name))
         {
            var fileSource = UnitTestConstants.CreateFile(rootDir.Directory.FullName);
            var fileCopy = Alphaleonis.Win32.Filesystem.DriveInfo.GetFreeDriveLetter() + @":\NonExistingDriveLetter\" +
                           UnitTestConstants.GetRandomFileName();
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
               gotException = exName.Equals(isNetwork ? "IOException" : "DirectoryNotFoundException",
                  StringComparison.OrdinalIgnoreCase);
               Console.WriteLine("\n\tCaught {0} Exception: [{1}] {2}", gotException ? "EXPECTED" : "UNEXPECTED",
                  exName, ex.Message);
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
            Console.WriteLine("\n\tCaught {0} Exception: [{1}] {2}", gotException ? "EXPECTED" : "UNEXPECTED", exName,
               ex.Message);
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


         using (var rootDir = new TemporaryDirectory(tempPath, MethodBase.GetCurrentMethod().Name))
         {
            var fileSource = UnitTestConstants.CreateFile(rootDir.Directory.FullName);
            var fileCopy = rootDir.RandomFileFullPath;
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
               Console.WriteLine("\n\tCaught {0} Exception: [{1}] {2}", gotException ? "EXPECTED" : "UNEXPECTED",
                  exName, ex.Message);
            }

            Assert.IsTrue(gotException, "The exception is not caught, but is expected to.");


            System.IO.File.SetAttributes(fileCopy, System.IO.FileAttributes.Normal);
         }

         Console.WriteLine();
      }
   }
}