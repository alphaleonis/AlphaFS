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

using Alphaleonis;
using Alphaleonis.Win32.Filesystem;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using Directory = Alphaleonis.Win32.Filesystem.Directory;
using DirectoryInfo = Alphaleonis.Win32.Filesystem.DirectoryInfo;
using DriveInfo = Alphaleonis.Win32.Filesystem.DriveInfo;
using File = Alphaleonis.Win32.Filesystem.File;
using FileInfo = Alphaleonis.Win32.Filesystem.FileInfo;
using FileSystemInfo = Alphaleonis.Win32.Filesystem.FileSystemInfo;
using Path = Alphaleonis.Win32.Filesystem.Path;

namespace AlphaFS.UnitTest
{
   /// <summary>This is a test class for Directory and is intended to contain all Directory class Unit Tests.</summary>
   [TestClass]
   public partial class DirectoryTest
   {
      private void DumpEnableDisableEncryption(bool isLocal)
      {
         Console.WriteLine("\n=== TEST {0} ===", isLocal ? UnitTestConstants.Local : UnitTestConstants.Network);
         var tempPath = Path.Combine(Path.GetTempPath(), "Directory.EnableDisableEncryption()-" + Path.GetRandomFileName());
         if (!isLocal) tempPath = Path.LocalToUnc(tempPath);

         Console.WriteLine("\nInput Directory Path: [{0}]", tempPath);

         const string disabled = "Disable=0";
         const string enabled = "Disable=1";
         var lineDisable = string.Empty;
         var deskTopIni = Path.Combine(tempPath, "Desktop.ini");

         Directory.CreateDirectory(tempPath);
         var actual = File.GetAttributes(tempPath);


         var report = string.Empty;
         var action = false;
         try
         {
            UnitTestConstants.StopWatcher(true);
            Directory.EnableEncryption(tempPath);
            report = UnitTestConstants.Reporter(true);
            action = true;
         }
         catch (Exception ex)
         {
            Console.WriteLine("\n\tCaught (unexpected) {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
         }
         Assert.IsTrue(action, "Encryption should be True");


         // Read filestream contents, get the last line.
         using (var streamRead = File.OpenText(deskTopIni))
         {
            string line;
            while ((line = streamRead.ReadLine()) != null)
            {
               lineDisable = line;
            }
         }
         action = lineDisable.Equals(disabled);
         Console.WriteLine("\nEnableEncryption() (Should be True): [{0}]", action);
         Console.WriteLine("File Desktop.ini contents: [{0}]\t{1}", lineDisable, report);
         Assert.IsTrue(action, "Encryption should be True");
         Assert.IsTrue(File.Exists(deskTopIni), "Desktop.ini should Exist");


         action = false;
         try
         {
            UnitTestConstants.StopWatcher(true);
            Directory.DisableEncryption(tempPath);
            report = UnitTestConstants.Reporter(true);
            action = true;
         }
         catch (Exception ex)
         {
            Console.WriteLine("\n\tCaught (unexpected) {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
         }
         Assert.IsTrue(action, "Encryption should be True");


         // Read filestream contents, get the last line.
         using (var streamRead = File.OpenText(deskTopIni))
         {
            string line;
            while ((line = streamRead.ReadLine()) != null)
            {
               lineDisable = line;
            }
         }
         action = lineDisable.Equals(enabled);
         Console.WriteLine("\nDisableEncryption() (Should be True): [{0}]", action);
         Console.WriteLine("File Desktop.ini contents: [{0}]\t{1}", lineDisable, report);
         Assert.IsTrue(action, "Encryption should be True");
         Assert.IsTrue(File.Exists(deskTopIni), "Desktop.ini should Exist");


         Directory.Delete(tempPath, true);
         Assert.IsFalse(Directory.Exists(tempPath), "Cleanup failed: Directory should have been removed.");
         Console.WriteLine();
      }

      private void DumpEncryptDecrypt(bool isLocal)
      {
         #region Setup

         Console.WriteLine("\n=== TEST {0} ===", isLocal ? UnitTestConstants.Local : UnitTestConstants.Network);
         var tempPath = Path.Combine(Path.GetTempPath(), "Directory.EncryptDecrypt()-" + Path.GetRandomFileName());
         if (!isLocal) tempPath = Path.LocalToUnc(tempPath);

         Console.WriteLine("\nInput Directory Path: [{0}]", tempPath);


         int cnt;
         var report = "";
         Directory.CreateDirectory(tempPath);
         var actual = File.GetAttributes(tempPath);
         var action = (actual & FileAttributes.Encrypted) != 0;

         Console.WriteLine("\nDirectory Encrypted (Should be False): [{0}]\tAttributes: [{1}]", action, actual);
         Assert.IsFalse(action, "Encryption should be False");
         Assert.IsFalse((actual & FileAttributes.Encrypted) != 0, "Encryption should be False");

         // Create some directories and files.
         for (var i = 0; i < 5; i++)
         {
            var file = Path.Combine(tempPath, Path.GetRandomFileName());

            var dir = file + "-dir";
            Directory.CreateDirectory(dir);

            // using() == Dispose() == Close() = deletable.
            using (File.Create(file)) { }
            using (File.Create(Path.Combine(dir, Path.GetFileName(file, true)))) { }

            actual = File.GetAttributes(file);
            action = (actual & FileAttributes.Encrypted) != 0;
            Assert.IsFalse(action, "Encryption should be False");
         }

         #endregion // Setup

         #region Encrypt

         // Encrypt directory recursively.
         UnitTestConstants.StopWatcher(true);
         try
         {
            Directory.Encrypt(tempPath, true);
            report = UnitTestConstants.Reporter();
            action = true;
         }
         catch (Exception ex)
         {
            Console.WriteLine("\n\tCaught (unexpected) {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));

            action = false;
            report = UnitTestConstants.Reporter();
         }
         Assert.IsTrue(action, "Unexpected Exception");

         actual = File.GetAttributes(tempPath);
         action = (actual & FileAttributes.Encrypted) != 0;
         Assert.IsTrue(action, "File system ojbect should be encrypted.");
         Console.WriteLine("\nDirectory Encrypted (Should be True): [{0}]\tAttributes: [{1}]\n\t{2}\n", action, actual, report);


         // Verify that everything is encrypted.
         cnt = 0;
         foreach (var fsei in Directory.EnumerateFileSystemEntryInfos<FileSystemEntryInfo>(tempPath, Path.WildcardStarMatchAll, DirectoryEnumerationOptions.Recursive))
         {
            actual = fsei.Attributes;
            action = (actual & FileAttributes.Encrypted) != 0;

            Console.WriteLine("\t#{0:000}\tFS Entry: [{1}]\t\tEncrypted (Should be True): [{2}]\t\tAttributes: [{3}]", ++cnt, fsei.FileName, action, actual);
            Assert.IsTrue(action, "File system ojbect should be encrypted.");
         }

         if (cnt == 0)
            Assert.Inconclusive("Nothing was enumerated.");

         #endregion // Encrypt

         #region Decrypt

         // Decrypt directory only.
         UnitTestConstants.StopWatcher(true);
         try
         {
            Directory.Decrypt(tempPath);
            report = UnitTestConstants.Reporter();
            action = true;
         }
         catch (Exception ex)
         {
            Console.WriteLine("\n\tCaught (unexpected) {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));

            action = false;
            report = UnitTestConstants.Reporter();
         }
         Assert.IsTrue(action, "Unexpected Exception");

         actual = File.GetAttributes(tempPath);
         action = (actual & FileAttributes.Encrypted) == 0;
         Assert.IsTrue(action, "File system ojbect should be decrypted.");
         Console.WriteLine("\nDirectory Decrypted (Should be True): [{0}]\tAttributes: [{1}]\n\t{2}\n", action, actual, report);


         // Verify that everything is still decrypted.
         cnt = 0;
         foreach (var fsei in Directory.EnumerateFileSystemEntryInfos<FileSystemEntryInfo>(tempPath, Path.WildcardStarMatchAll, DirectoryEnumerationOptions.Recursive))
         {
            actual = fsei.Attributes;
            action = (actual & FileAttributes.Encrypted) == 0;

            Console.WriteLine("\t#{0:000}\tFS Entry: [{1}]\t\tDecrypted (Should be False): [{2}]\t\tAttributes: [{3}]", ++cnt, fsei.FileName, action, actual);
            Assert.IsFalse(action, "File system ojbect should be encrypted.");
         }

         if (cnt == 0)
            Assert.Inconclusive("Nothing was enumerated.");

         #endregion // Decrypt

         Directory.Delete(tempPath, true);
         Assert.IsFalse(Directory.Exists(tempPath), "Cleanup failed: Directory should have been removed.");
         Console.WriteLine();
      }

      private void DumpEnumerateDirectories(bool isLocal)
      {
         var isNetwork = !isLocal;

         #region Setup

         Console.WriteLine("\n=== TEST {0} ===", isLocal ? UnitTestConstants.Local : UnitTestConstants.Network);

         var cnt = 0;
         var searchPattern = Path.WildcardStarMatchAll;
         var searchOption = SearchOption.TopDirectoryOnly;

         var random = Path.GetRandomFileName();
         var folderSource = @"folder-source-" + random;

         var originalLetter = DriveInfo.GetFreeDriveLetter() + @":";
         var letter = originalLetter + @"\";

         #endregion // Setup

         #region DirectoryNotFoundException (UnitTestConstants.Local) / IOException (UnitTestConstants.Network)

         var gotException = false;
         try
         {
            var nonExistingPath = letter + folderSource;
            if (!isLocal) nonExistingPath = Path.LocalToUnc(nonExistingPath);

            new DirectoryInfo(nonExistingPath).EnumerateDirectories().Any();
         }
         catch (Exception ex)
         {
            // Local: DirectoryNotFoundException.
            // UNC: IOException.

            var exName = ex.GetType().Name;
            gotException = exName.Equals(isNetwork ? "IOException" : "DirectoryNotFoundException", StringComparison.OrdinalIgnoreCase);
            Console.WriteLine("\n\tCaught Exception: [{0}] Message: [{1}]", exName, ex.Message);
         }
         Assert.IsTrue(gotException, "The exception is not caught, but is expected to.");
         Console.WriteLine();

         #endregion // DirectoryNotFoundException (UnitTestConstants.Local) / IOException (UnitTestConstants.Network)

         #region IOException

         var tempPath = Path.GetTempPath("Directory.EnumerateDirectories-file-" + Path.GetRandomFileName());
         if (!isLocal) tempPath = Path.LocalToUnc(tempPath);

         try
         {
            using (File.Create(tempPath)) { }

            gotException = false;
            try
            {
               new DirectoryInfo(tempPath).EnumerateDirectories().Any();
            }
            catch (Exception ex)
            {
               var exName = ex.GetType().Name;
               gotException = exName.Equals("IOException", StringComparison.OrdinalIgnoreCase);
               Console.WriteLine("\n\tCaught Exception: [{0}] Message: [{1}]", exName, ex.Message);
            }
            Assert.IsTrue(gotException, "The exception is not caught, but is expected to.");
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
         if (!isLocal) tempPath = Path.LocalToUnc(tempPath);

         var di = new DirectoryInfo(tempPath);
         if (di.Exists)
         {
            gotException = false;
            try
            {
               di.EnumerateDirectories(searchPattern, SearchOption.AllDirectories).All(o => o.Exists);
            }
            catch (Exception ex)
            {
               var exName = ex.GetType().Name;
               gotException = exName.Equals("UnauthorizedAccessException", StringComparison.OrdinalIgnoreCase);
               Console.WriteLine("\n\tCaught Exception: [{0}] Message: [{1}]", exName, ex.Message);
            }
            Assert.IsTrue(gotException, "The exception is not caught, but is expected to.");
            Console.WriteLine();
         }

         #endregion // UnauthorizedAccessException

         var path = isLocal ? UnitTestConstants.SysRoot : Path.LocalToUnc(UnitTestConstants.SysRoot);

         Console.WriteLine("\nInput Directory Path: [{0}]\n", path);
         Console.WriteLine("\tEnumerate directories, using \"SearchOption.{0}\"\n", searchOption);

         UnitTestConstants.StopWatcher(true);
         foreach (var dirInfo in new DirectoryInfo(path).EnumerateDirectories(searchPattern, searchOption))
         {
            Console.WriteLine("\t#{0:000}\t[{1}]", ++cnt, dirInfo.FullName);

            // Issue #21601: OverflowException when accessing EntryInfo.
            var isMountPoint = dirInfo.EntryInfo.IsMountPoint;

            Assert.IsTrue(dirInfo.EntryInfo.IsDirectory);
         }

         Console.WriteLine();
         Console.WriteLine(UnitTestConstants.Reporter());

         if (cnt == 0)
            Assert.Inconclusive("Nothing was enumerated.");

         Console.WriteLine();


         cnt = 0;
         searchPattern = @"*e*e*";
         Console.WriteLine("\tsearchPattern: [{0}]\n", searchPattern);
         UnitTestConstants.StopWatcher(true);
         foreach (var dirInfo in new DirectoryInfo(path).EnumerateDirectories(searchPattern, searchOption))
         {
            Console.WriteLine("\t#{0:000}\t[{1}]", ++cnt, dirInfo.FullName);

            // Issue #21601: OverflowException when accessing EntryInfo.
            var isMountPoint = dirInfo.EntryInfo.IsMountPoint;

            Assert.IsTrue(dirInfo.EntryInfo.IsDirectory);
         }

         Console.WriteLine();
         Console.WriteLine(UnitTestConstants.Reporter());

         if (cnt == 0)
            Assert.Inconclusive("Nothing was enumerated.");

         #region DirectoryEnumerationOptions

         // Should only return folders.

         foreach (var dir in Directory.EnumerateDirectories(UnitTestConstants.SysRoot, DirectoryEnumerationOptions.FilesAndFolders))
            Assert.IsTrue((File.GetAttributes(dir) & FileAttributes.Directory) != 0, string.Format("Expected a folder, not a file: [{0}]", dir));

         foreach (var dir in Directory.EnumerateDirectories(UnitTestConstants.SysRoot, DirectoryEnumerationOptions.Files))
            Assert.IsTrue((File.GetAttributes(dir) & FileAttributes.Directory) != 0, string.Format("Expected a folder, not a file: [{0}]", dir));

         #endregion // DirectoryEnumerationOptions

         Console.WriteLine();
      }

      private void DumpEnumerateFileIdBothDirectoryInfo(bool isLocal)
      {
         Console.WriteLine("\n=== TEST {0} ===", isLocal ? UnitTestConstants.Local : UnitTestConstants.Network);
         var tempPath = UnitTestConstants.SysRoot;
         if (!isLocal) tempPath = Path.LocalToUnc(tempPath);

         var searchPattern = Path.WildcardStarMatchAll;

         var directories = Directory.CountFileSystemObjects(tempPath, searchPattern, DirectoryEnumerationOptions.Folders);
         var files = Directory.CountFileSystemObjects(tempPath, searchPattern, DirectoryEnumerationOptions.Files);

         Console.WriteLine("\nInput Directory Path: [{0}]\tCounted: Directories = [{1}] Files = [{2}]", tempPath, directories, files);

         var foundFse = false;
         long numDirectories = 0;
         long numFiles = 0;

         UnitTestConstants.StopWatcher(true);
         foreach (var fibdi in Directory.EnumerateFileIdBothDirectoryInfo(tempPath))
         {
            if ((fibdi.FileAttributes & FileAttributes.Directory) != 0)
               numDirectories++;
            else
               numFiles++;

            foundFse = UnitTestConstants.Dump(fibdi, -22);
         }
         var report = UnitTestConstants.Reporter();

         Console.WriteLine("\n\tEnumerated: Directories = [{0}] Files = [{1}]\t{2}", numDirectories, numFiles, report);

         if (!foundFse)
            Assert.Inconclusive("Nothing was enumerated.");

         var matchAll = directories == numDirectories && files == numFiles;
         Assert.IsTrue(matchAll, "Number of directories and/or files don't match.");

         Console.WriteLine();
      }

      private void DumpEnumerateFiles(bool isLocal)
      {
         var isNetwork = !isLocal;

         #region Setup

         Console.WriteLine("\n=== TEST {0} ===", isLocal ? UnitTestConstants.Local : UnitTestConstants.Network);

         var cnt = 0;
         var searchPattern = Path.WildcardStarMatchAll;
         var searchOption = SearchOption.TopDirectoryOnly;

         var random = Path.GetRandomFileName();
         var folderSource = @"folder-source-" + random;

         var originalLetter = DriveInfo.GetFreeDriveLetter() + @":";
         var letter = originalLetter + @"\";

         #endregion // Setup

         #region DirectoryNotFoundException (UnitTestConstants.Local) / IOException (UnitTestConstants.Network)

         var gotException = false;
         try
         {
            var nonExistingPath = letter + folderSource;
            if (!isLocal) nonExistingPath = Path.LocalToUnc(nonExistingPath);

            new DirectoryInfo(nonExistingPath).EnumerateFiles().Any();
         }
         catch(Exception ex)
            {
            // Local: DirectoryNotFoundException.
            // UNC: IOException.

            var exName = ex.GetType().Name;
            gotException = exName.Equals(isNetwork ? "IOException" : "DirectoryNotFoundException", StringComparison.OrdinalIgnoreCase);
            Console.WriteLine("\n\tCaught Exception: [{0}] Message: [{1}]", exName, ex.Message);
         }
         Assert.IsTrue(gotException, "The exception is not caught, but is expected to.");
         Console.WriteLine();

         #endregion // DirectoryNotFoundException (UnitTestConstants.Local) / IOException (UnitTestConstants.Network)

         #region IOException

         var tempPath = Path.GetTempPath("Directory.EnumerateFiles-file-" + Path.GetRandomFileName());
         if (!isLocal) tempPath = Path.LocalToUnc(tempPath);

         using (File.Create(tempPath)) { }

         gotException = false;
         try
         {
            new DirectoryInfo(tempPath).EnumerateFiles().Any();
         }
         catch (Exception ex)
         {
            var exName = ex.GetType().Name;
            gotException = exName.Equals("IOException", StringComparison.OrdinalIgnoreCase);
            Console.WriteLine("\n\tCaught Exception: [{0}] Message: [{1}]", exName, ex.Message);
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
         if (!isLocal) tempPath = Path.LocalToUnc(tempPath);

         var di = new DirectoryInfo(tempPath);
         if (di.Exists)
         {
            gotException = false;
            try
            {
               di.EnumerateFiles(searchPattern, SearchOption.AllDirectories).All(o => o.Exists);
            }
            catch (Exception ex)
            {
               var exName = ex.GetType().Name;
               gotException = exName.Equals("UnauthorizedAccessException", StringComparison.OrdinalIgnoreCase);
               Console.WriteLine("\n\tCaught Exception: [{0}] Message: [{1}]", exName, ex.Message);
            }
            Assert.IsTrue(gotException, "The exception is not caught, but is expected to.");
            Console.WriteLine();
         }

         #endregion // UnauthorizedAccessException

         var path = isLocal ? UnitTestConstants.SysRoot : Path.LocalToUnc(UnitTestConstants.SysRoot);

         Console.WriteLine("\nInput Directory Path: [{0}]\n", path);
         Console.WriteLine("\tEnumerate files, using \"SearchOption.{0}\"\n", searchOption);

         UnitTestConstants.StopWatcher(true);
         foreach (var fileInfo in new DirectoryInfo(path).EnumerateFiles(searchPattern, searchOption))
         {
            Console.WriteLine("\t#{0:000}\t[{1}]", ++cnt, fileInfo.FullName);

            // Issue #21601: OverflowException when accessing EntryInfo.
            var isMountPoint = fileInfo.EntryInfo.IsMountPoint;

            Assert.IsFalse(fileInfo.EntryInfo.IsDirectory);
         }

         Console.WriteLine();
         Console.WriteLine(UnitTestConstants.Reporter());

         if (cnt == 0)
            Assert.Inconclusive("Nothing was enumerated.");

         Console.WriteLine();


         cnt = 0;
         searchPattern = @"*e*.exe";
         Console.WriteLine("\tsearchPattern: [{0}]\n", searchPattern);
         UnitTestConstants.StopWatcher(true);
         foreach (var fileInfo in new DirectoryInfo(path).EnumerateFiles(searchPattern, searchOption))
         {
            Console.WriteLine("\t#{0:000}\t[{1}]", ++cnt, fileInfo.FullName);

            // Issue #21601: OverflowException when accessing EntryInfo.
            var isMountPoint = fileInfo.EntryInfo.IsMountPoint;

            Assert.IsFalse(fileInfo.EntryInfo.IsDirectory);
         }

         Console.WriteLine();
         Console.WriteLine(UnitTestConstants.Reporter());

         if (cnt == 0)
            Assert.Inconclusive("Nothing was enumerated.");

         #region DirectoryEnumerationOptions

         // Should only return files.

         foreach (var file in Directory.EnumerateFiles(UnitTestConstants.SysRoot, DirectoryEnumerationOptions.FilesAndFolders))
            Assert.IsTrue((File.GetAttributes(file) & FileAttributes.Directory) == 0, string.Format("Expected a file, not a folder: [{0}]", file));

         foreach (var file in Directory.EnumerateFiles(UnitTestConstants.SysRoot, DirectoryEnumerationOptions.Folders))
            Assert.IsTrue((File.GetAttributes(file) & FileAttributes.Directory) == 0, string.Format("Expected a file, not a folder: [{0}]", file));

         #endregion // DirectoryEnumerationOptions

         Console.WriteLine();
      }

      private void DumpEnumerateFileSystemEntries(bool isLocal)
      {
         var isNetwork = !isLocal;

         #region Setup

         Console.WriteLine("\n=== TEST {0} ===", isLocal ? UnitTestConstants.Local : UnitTestConstants.Network);

         var cnt = 0;
         var searchPattern = Path.WildcardStarMatchAll;
         var searchOption = SearchOption.TopDirectoryOnly;
         
         var random = Path.GetRandomFileName();
         var folderSource = @"folder-source-" + random;

         var originalLetter = DriveInfo.GetFreeDriveLetter() + @":";
         var letter = originalLetter + @"\";

         #endregion // Setup

         #region DirectoryNotFoundException (UnitTestConstants.Local) / IOException (UnitTestConstants.Network)

         var gotException = false;
         try
         {
            var nonExistingPath = letter + folderSource;
            if (!isLocal) nonExistingPath = Path.LocalToUnc(nonExistingPath);

            new DirectoryInfo(nonExistingPath).EnumerateFileSystemInfos().Any();
         }
         catch (Exception ex)
         {
            // Local: DirectoryNotFoundException.
            // UNC: IOException.

            var exName = ex.GetType().Name;
            gotException = exName.Equals(isNetwork ? "IOException" : "DirectoryNotFoundException", StringComparison.OrdinalIgnoreCase);
            Console.WriteLine("\n\tCaught Exception: [{0}] Message: [{1}]", exName, ex.Message);
         }
         Assert.IsTrue(gotException, "The exception is not caught, but is expected to.");
         Console.WriteLine();

         #endregion // DirectoryNotFoundException (UnitTestConstants.Local) / IOException (UnitTestConstants.Network)

         #region IOException

         var tempPath = Path.GetTempPath("Directory.EnumerateFileSystemEntries-file-" + Path.GetRandomFileName());
         if (!isLocal) tempPath = Path.LocalToUnc(tempPath);

         try
         {
            using (File.Create(tempPath)) { }

            gotException = false;
            try
            {
               new DirectoryInfo(tempPath).EnumerateFileSystemInfos().Any();
            }
            catch (Exception ex)
            {
               var exName = ex.GetType().Name;
               gotException = exName.Equals("IOException", StringComparison.OrdinalIgnoreCase);
               Console.WriteLine("\n\tCaught Exception: [{0}] Message: [{1}]", exName, ex.Message);
            }
            Assert.IsTrue(gotException, "The exception is not caught, but is expected to.");
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
         if (!isLocal) tempPath = Path.LocalToUnc(tempPath);

         var di = new DirectoryInfo(tempPath);
         if (di.Exists)
         {
            gotException = false;
            try
            {
               di.EnumerateFileSystemInfos(searchPattern, SearchOption.AllDirectories).All(o => o.Exists);
            }
            catch (Exception ex)
            {
               var exName = ex.GetType().Name;
               gotException = exName.Equals("UnauthorizedAccessException", StringComparison.OrdinalIgnoreCase);
               Console.WriteLine("\n\tCaught Exception: [{0}] Message: [{1}]", exName, ex.Message);
            }
            Assert.IsTrue(gotException, "The exception is not caught, but is expected to.");
            Console.WriteLine();
         }

         #endregion // UnauthorizedAccessException

         var path = isLocal ? UnitTestConstants.SysRoot : Path.LocalToUnc(UnitTestConstants.SysRoot);

         Console.WriteLine("\nInput Directory Path: [{0}]\n", path);
         Console.WriteLine("\tEnumerate file system entries, using \"SearchOption.{0}\"\n", searchOption);

         UnitTestConstants.StopWatcher(true);
         foreach (var fsi in new DirectoryInfo(path).EnumerateFileSystemInfos(searchPattern, searchOption))
         {
            Console.WriteLine("\t#{0:000}\t[{1}]", ++cnt, fsi.FullName);

            // Issue #21601: OverflowException when accessing EntryInfo.
            // (Actually only for DirectoryInfo() and FileInfo())
            var isMountPoint = fsi.EntryInfo.IsMountPoint;
         }

         Console.WriteLine();
         Console.WriteLine(UnitTestConstants.Reporter());

         if (cnt == 0)
            Assert.Inconclusive("Nothing was enumerated.");

         Console.WriteLine();


         cnt = 0;
         searchPattern = @"*e*e*";
         Console.WriteLine("\tsearchPattern: [{0}]\n", searchPattern);
         UnitTestConstants.StopWatcher(true);
         foreach (FileSystemInfo fsi in new DirectoryInfo(path).EnumerateDirectories(searchPattern, searchOption))
         {
            Console.WriteLine("\t#{0:000}\t[{1}]", ++cnt, fsi.FullName);

            // Issue #21601: OverflowException when accessing EntryInfo.
            var isMountPoint = fsi.EntryInfo.IsMountPoint;

            Assert.IsTrue(fsi.EntryInfo.IsDirectory || !fsi.EntryInfo.IsDirectory);
         }

         Console.WriteLine();
         Console.WriteLine(UnitTestConstants.Reporter());

         if (cnt == 0)
            Assert.Inconclusive("Nothing was enumerated.");

         #region DirectoryEnumerationOptions

         // Should only return folders.

         foreach (var dir in Directory.EnumerateFileSystemEntries(UnitTestConstants.SysRoot, DirectoryEnumerationOptions.Folders))
            Assert.IsTrue((File.GetAttributes(dir) & FileAttributes.Directory) != 0, string.Format("Expected a folder, not a file: [{0}]", dir));


         // Should only return files.

         foreach (var file in Directory.EnumerateFileSystemEntries(UnitTestConstants.SysRoot, DirectoryEnumerationOptions.Files))
            Assert.IsTrue((File.GetAttributes(file) & FileAttributes.Directory) == 0, string.Format("Expected a file, not a folder: [{0}]", file));

         #endregion // DirectoryEnumerationOptions

         Console.WriteLine();
      }
      
      private void DumpGetDrives(bool enumerate)
      {
         Console.WriteLine("\nIf you are missing drives, please see this topic: https://alphafs.codeplex.com/discussions/397693 \n");

         var cnt = 0;
         UnitTestConstants.StopWatcher(true);
         foreach (var actual in enumerate ? Directory.EnumerateLogicalDrives(false, false) : DriveInfo.GetDrives())
         {
            Console.WriteLine("#{0:000}\tLogical Drive: [{1}]", ++cnt, actual.Name);

            if (actual.IsReady && actual.DriveType != DriveType.CDRom)
            {
               // GetFreeSpaceEx()
               Assert.IsTrue(actual.AvailableFreeSpace > 0 && actual.TotalSize > 0 && actual.TotalFreeSpace > 0);

               // GetFreeSpace()
               Assert.IsTrue(actual.DiskSpaceInfo.SectorsPerCluster > 0 && actual.DiskSpaceInfo.BytesPerSector > 0 && actual.DiskSpaceInfo.TotalNumberOfClusters > 0);

               // DriveInfo()
               Assert.IsTrue(actual.DiskSpaceInfo.ClusterSize > 0 &&
                             !string.IsNullOrWhiteSpace(actual.DiskSpaceInfo.TotalSizeUnitSize) &&
                             !string.IsNullOrWhiteSpace(actual.DiskSpaceInfo.UsedSpaceUnitSize) &&
                             !string.IsNullOrWhiteSpace(actual.DiskSpaceInfo.AvailableFreeSpaceUnitSize));
            }
         }
         Console.WriteLine("\n{0}", UnitTestConstants.Reporter(true));
      }
      
      private void DumpGetFileSystemEntries(bool isLocal)
      {
         var isNetwork = !isLocal;

         #region Setup

         Console.WriteLine("\n=== TEST {0} ===", isLocal ? UnitTestConstants.Local : UnitTestConstants.Network);

         var cnt = 0;
         var searchPattern = Path.WildcardStarMatchAll;
         var searchOption = SearchOption.TopDirectoryOnly;

         var random = Path.GetRandomFileName();
         var folderSource = @"folder-source-" + random;

         var originalLetter = DriveInfo.GetFreeDriveLetter() + @":";
         var letter = originalLetter + @"\";

         #endregion // Setup

         #region DirectoryNotFoundException (UnitTestConstants.Local) / IOException (UnitTestConstants.Network)
         
         var gotException = false;
         try
         {
            var nonExistingPath = letter + folderSource;
            if (!isLocal) nonExistingPath = Path.LocalToUnc(nonExistingPath);

            Directory.GetFileSystemEntries(nonExistingPath);
         }
         catch (Exception ex)
         {
            // Local: DirectoryNotFoundException.
            // UNC: IOException.

            var exName = ex.GetType().Name;
            gotException = exName.Equals(isNetwork ? "IOException" : "DirectoryNotFoundException", StringComparison.OrdinalIgnoreCase);
            Console.WriteLine("\n\tCaught Exception: [{0}] Message: [{1}]", exName, ex.Message);
         }
         Assert.IsTrue(gotException, "The exception is not caught, but is expected to.");

         Console.WriteLine();

         #endregion // DirectoryNotFoundException (UnitTestConstants.Local) / IOException (UnitTestConstants.Network)

         #region IOException

         var tempPath = Path.GetTempPath("Directory.GetDirectories-file-" + Path.GetRandomFileName());
         if (!isLocal) tempPath = Path.LocalToUnc(tempPath);

         gotException = false;
         try
         {
            using (File.Create(tempPath)) { }

            try
            {
               Directory.GetFileSystemEntries(tempPath).Any();
            }
            catch (Exception ex)
            {
               var exName = ex.GetType().Name;
               gotException = exName.Equals("IOException", StringComparison.OrdinalIgnoreCase);
               Console.WriteLine("\n\tCaught Exception: [{0}] Message: [{1}]", exName, ex.Message);
            }
            Assert.IsTrue(gotException, "The exception is not caught, but is expected to.");
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
         if (!isLocal) tempPath = Path.LocalToUnc(tempPath);

         if (Directory.Exists(tempPath))
         {
            gotException = false;
            try
            {
               Directory.GetFileSystemEntries(tempPath, searchPattern, SearchOption.AllDirectories).Any();
            }
            catch (Exception ex)
            {
               var exName = ex.GetType().Name;
               gotException = exName.Equals("UnauthorizedAccessException", StringComparison.OrdinalIgnoreCase);
               Console.WriteLine("\n\tCaught Exception: [{0}] Message: [{1}]", exName, ex.Message);
            }
            Assert.IsTrue(gotException, "The exception is not caught, but is expected to.");
            Console.WriteLine();
         }

         #endregion // UnauthorizedAccessException

         var path = isLocal ? UnitTestConstants.SysRoot : Path.LocalToUnc(UnitTestConstants.SysRoot);

         Console.WriteLine("\nInput Directory Path: [{0}]\n", path);
         Console.WriteLine("\tGet FileSystemEntries, using \"SearchOption.{0}\"\n", searchOption);

         UnitTestConstants.StopWatcher(true);
         foreach (var folder in Directory.GetFileSystemEntries(path, searchPattern, searchOption))
            Console.WriteLine("\t#{0:000}\t[{1}]", ++cnt, folder);

         Console.WriteLine();
         Console.WriteLine(UnitTestConstants.Reporter());

         if (cnt == 0)
            Assert.Inconclusive("Nothing was enumerated.");

         Console.WriteLine();
      }

      private void DumpGetProperties(bool isLocal)
      {
         Console.WriteLine("\n=== TEST {0} ===", isLocal ? UnitTestConstants.Local : UnitTestConstants.Network);
         var path = isLocal ? UnitTestConstants.SysRoot : Path.LocalToUnc(UnitTestConstants.SysRoot);

         Console.WriteLine("\n\tAggregated properties of file system objects from Directory: [{0}]\n", path);

         UnitTestConstants.StopWatcher(true);
         var props = Directory.GetProperties(path, DirectoryEnumerationOptions.FilesAndFolders | DirectoryEnumerationOptions.Recursive | DirectoryEnumerationOptions.ContinueOnException);
         var report = UnitTestConstants.Reporter();

         var total = props["Total"];
         var file = props["File"];
         var size = props["Size"];
         var cnt = 0;

         foreach (var key in props.Keys)
            Console.WriteLine("\t\t#{0:000}\t{1, -17} = [{2}]", ++cnt, key, props[key]);

         Console.WriteLine("\n\t{0}", report);

         if (cnt == 0)
            Assert.Inconclusive("Nothing was enumerated.");

         Assert.IsTrue(total > 0, "0 Objects.");
         Assert.IsTrue(file > 0, "0 Files.");
         Assert.IsTrue(size > 0, "0 Size.");
         Console.WriteLine();
      }

      private bool HasInheritedPermissions(string path)
      {
         var acl = Directory.GetAccessControl(path);
         return acl.GetAccessRules(false, true, typeof(SecurityIdentifier)).Count > 0;
      }

      #region .NET

      [TestMethod]
      public void Directory_EnumerateDirectories()
      {
         Console.WriteLine("Directory.EnumerateDirectories()");

         DumpEnumerateDirectories(true);
         DumpEnumerateDirectories(false);
      }

      [TestMethod]
      public void Directory_EnumerateFiles()
      {
         Console.WriteLine("Directory.EnumerateFiles()");

         DumpEnumerateFiles(true);
         DumpEnumerateFiles(false);
      }

      [TestMethod]
      public void Directory_EnumerateFileSystemEntries()
      {
         Console.WriteLine("Directory.EnumerateFileSystemEntries()");

         DumpEnumerateFileSystemEntries(true);
         DumpEnumerateFileSystemEntries(false);
      }
      
      [TestMethod]
      public void Directory_GetDirectoryRoot()
      {
         Console.WriteLine("Directory.GetDirectoryRoot()");

         var pathCnt = 0;
         var errorCnt = 0;

         #region ArgumentException

         var gotException = false;
         try
         {
            Directory.GetDirectoryRoot(@"\\\\.txt");
         }
         catch (Exception ex)
         {
            var exName = ex.GetType().Name;
            gotException = exName.Equals("ArgumentException", StringComparison.OrdinalIgnoreCase);
            Console.WriteLine("\n\tCaught Exception: [{0}] Message: [{1}]", exName, ex.Message);
         }
         Assert.IsTrue(gotException, "The exception is not caught, but is expected to.");
         Console.WriteLine();

         #endregion // ArgumentException

         UnitTestConstants.StopWatcher(true);
         foreach (var path in UnitTestConstants.InputPaths)
         {
            string expected = null;
            string actual = null;
            var skipAssert = false;

            Console.WriteLine("\n#{0:000}\tInput Path: [{1}]", ++pathCnt, path);

            // System.IO
            try
            {
               expected = System.IO.Directory.GetDirectoryRoot(path);
            }
            catch (Exception ex)
            {
               skipAssert = ex is ArgumentException;

               Console.WriteLine("\tCaught [System.IO] {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
            }
            Console.WriteLine("\t   System.IO : [{0}]", expected ?? "null");


            // AlphaFS
            try
            {
               actual = Directory.GetDirectoryRoot(path);

               if (!skipAssert)
                  Assert.AreEqual(expected, actual);
            }
            catch (Exception ex)
            {
               errorCnt++;

               Console.WriteLine("\tCaught [AlphaFS] {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
            }
            Console.WriteLine("\t   AlphaFS   : [{0}]", actual ?? "null");
         }
         Console.WriteLine("\n{0}", UnitTestConstants.Reporter(true));

         Assert.AreEqual(0, errorCnt, "Encountered paths where AlphaFS != System.IO");
      }

      [TestMethod]
      public void Directory_GetFileSystemEntries()
      {
         Console.WriteLine("Directory.GetFileSystemEntries()");

         DumpGetFileSystemEntries(true);
         DumpGetFileSystemEntries(false);
      }

      [TestMethod]
      public void Directory_GetFileSystemEntries_LongPathWithPrefix_ShouldReturnCorrectEntries()
      {
         using (var tempDir = new TemporaryDirectory("GetFileSystemEntries"))
         {
            var longDir = Path.Combine(tempDir.Directory.FullName, new string('x', 128), new string('x', 128), new string('x', 128), new string('x', 128));
            Directory.CreateDirectory(longDir);
            Directory.CreateDirectory(Path.Combine(longDir, "A"));
            Directory.CreateDirectory(Path.Combine(longDir, "B"));
            File.WriteAllText(Path.Combine(longDir, "C"), "C");

            var entries = Directory.GetFileSystemEntries("\\\\?\\" + longDir).ToArray();

            Assert.AreEqual(3, entries.Length);
         }
      }

      [TestMethod]
      public void Directory_GetFileSystemEntries_LongPathWithoutPrefix_ShouldReturnCorrectEntries()
      {
         using (var tempDir = new TemporaryDirectory("GetFileSystemEntries"))
         {
            var longDir = Path.Combine(tempDir.Directory.FullName, new string('x', 128), new string('x', 128), new string('x', 128), new string('x', 128));
            Directory.CreateDirectory(longDir);
            Directory.CreateDirectory(Path.Combine(longDir, "A"));
            Directory.CreateDirectory(Path.Combine(longDir, "B"));
            File.WriteAllText(Path.Combine(longDir, "C"), "C");

            var entries = Directory.GetFileSystemEntries(longDir).ToArray();

            Assert.AreEqual(3, entries.Length);
         }
      }

      [TestMethod]
      public void Directory_GetLogicalDrives()
      {
         Console.WriteLine("Directory.GetLogicalDrives()");

         DumpGetDrives(false);
      }

      [TestMethod]
      public void Directory_GetParent()
      {
         Console.WriteLine("Directory.GetParent()");

         var pathCnt = 0;
         var errorCnt = 0;

         UnitTestConstants.StopWatcher(true);
         foreach (var path in UnitTestConstants.InputPaths)
         {
            string expected = null;
            string actual = null;
            var skipAssert = false;

            Console.WriteLine("\n#{0:000}\tInput Path: [{1}]", ++pathCnt, path);

            // System.IO
            try
            {
               var result = System.IO.Directory.GetParent(path);
               expected = result == null ? null : result.FullName;
            }
            catch (Exception ex)
            {
               skipAssert = ex is ArgumentException;

               Console.WriteLine("\tCaught [System.IO] {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
            }
            Console.WriteLine("\t   System.IO : [{0}]", expected ?? "null");


            // AlphaFS
            try
            {
               var result = Directory.GetParent(path);
               actual = result == null ? null : result.FullName;

               if (!skipAssert)
                  Assert.AreEqual(expected, actual);
            }
            catch (Exception ex)
            {
               errorCnt++;

               Console.WriteLine("\tCaught [AlphaFS] {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
            }
            Console.WriteLine("\t   AlphaFS   : [{0}]", actual ?? "null");
         }
         Console.WriteLine("\n{0}", UnitTestConstants.Reporter(true));

         Assert.AreEqual(0, errorCnt, "Encountered paths where AlphaFS != System.IO");
      }

      #endregion // .NET

      #region AlphaFS

      [TestMethod]
      public void AlphaFS_Directory_DeleteEmptySubdirectories()
      {
         Console.WriteLine("Directory.DeleteEmptySubdirectories()");

         var tempPath = Path.Combine(Path.GetTempPath(), "Directory.DeleteEmptySubdirectories()-" + Path.GetRandomFileName());
         long dirs0, dirs1, files0, files1;

         const int maxDepth = 10;
         const int totalDirectories = (maxDepth * maxDepth) + maxDepth;  // maxDepth = 10: 110 directories and 110 files.
         const int emptyDirectories = (maxDepth * maxDepth) / 2;         // 50 empty directories.
         const int remainingDirectories = totalDirectories - emptyDirectories;   // 60 remaining directories.

         Console.WriteLine("\nInput Path: [{0}]", tempPath);
         UnitTestConstants.CreateDirectoriesAndFiles(tempPath, maxDepth, true);

         var searchPattern = Path.WildcardStarMatchAll;

         UnitTestConstants.StopWatcher(true);
         dirs0 = Directory.CountFileSystemObjects(tempPath, searchPattern, DirectoryEnumerationOptions.Folders | DirectoryEnumerationOptions.Recursive | DirectoryEnumerationOptions.ContinueOnException);
         files0 = Directory.CountFileSystemObjects(tempPath, searchPattern, DirectoryEnumerationOptions.Files | DirectoryEnumerationOptions.Recursive | DirectoryEnumerationOptions.ContinueOnException);
         Console.WriteLine("\nCounted Directories: [{0}]\nCounted Files      : [{1}]\n{2}", dirs0, files0, UnitTestConstants.Reporter());

         UnitTestConstants.StopWatcher(true);
         var deleteOk = false;
         try
         {
            Directory.DeleteEmptySubdirectories(tempPath, false);
            deleteOk = true;
         }
         catch
         {
         }


         // Issue-21123: Method Directory- and DirectoryInfo.DeleteEmptySubdirectories() also deletes the given directories when totally empty.
         Assert.IsTrue(Directory.Exists(tempPath), "Directory should exist.");

         Console.WriteLine("\nDirectory.DeleteEmptySubdirectories() (Should be True): [{0}]\n{1}", deleteOk, UnitTestConstants.Reporter());
         Assert.IsTrue(deleteOk, "DeleteEmptySubdirectories() failed.");

         searchPattern = Path.WildcardStarMatchAll;

         UnitTestConstants.StopWatcher(true);
         dirs1 = Directory.CountFileSystemObjects(tempPath, searchPattern, DirectoryEnumerationOptions.Folders | DirectoryEnumerationOptions.Recursive | DirectoryEnumerationOptions.ContinueOnException);
         files1 = Directory.CountFileSystemObjects(tempPath, searchPattern, DirectoryEnumerationOptions.Files | DirectoryEnumerationOptions.Recursive | DirectoryEnumerationOptions.ContinueOnException);
         Console.WriteLine("\nCounted Directories (Should be 60): [{0}]\nCounted Files (Should be 110)     : [{1}]\n{2}", dirs1, files1, UnitTestConstants.Reporter());
         Assert.IsTrue(dirs1 != dirs0);
         Assert.IsTrue(dirs1 == remainingDirectories);
         Assert.IsTrue(files1 == files0);

         Directory.Delete(tempPath, true);
         var directoryNotExists = !Directory.Exists(tempPath);
         Assert.IsTrue(directoryNotExists);

         Assert.IsTrue((emptyDirectories + remainingDirectories) == totalDirectories);
      }

      [TestMethod]
      public void AlphaFS_Directory_Encrypt()
      {
         Console.WriteLine("Directory.Encrypt()");

         DumpEncryptDecrypt(true);
         DumpEncryptDecrypt(false);
      }

      [TestMethod]
      public void AlphaFS_Directory_EnableEncryption()
      {
         Console.WriteLine("Directory.EnableEncryption()");

         DumpEnableDisableEncryption(true);
         DumpEnableDisableEncryption(false);
      }

      [TestMethod]
      public void AlphaFS_Directory_EnumerateFileIdBothDirectoryInfo()
      {
         Console.WriteLine("Directory.EnumerateFileIdBothDirectoryInfo()");

         DumpEnumerateFileIdBothDirectoryInfo(true);
         DumpEnumerateFileIdBothDirectoryInfo(false);
      }

      [TestMethod]
      public void AlphaFS_Directory_GetProperties()
      {
         Console.WriteLine("Directory.GetProperties()");

         DumpGetProperties(true);
         DumpGetProperties(false);
      }

      [TestMethod]
      public void AlphaFS_Directory_HasInheritedPermissions()
      {
         Console.WriteLine("Directory.HasInheritedPermissions()\n");

         var searchPattern = Path.WildcardStarMatchAll;
         var searchOption = SearchOption.TopDirectoryOnly;

         var cnt = 0;
         UnitTestConstants.StopWatcher(true);
         foreach (var dir in Directory.EnumerateDirectories(UnitTestConstants.SysRoot, searchPattern, searchOption))
         {
            try
            {
               var hasIp = Directory.HasInheritedPermissions(dir);

               if (hasIp)
                  Console.WriteLine("\t#{0:000}\t[{1}]\t\tDirectory has inherited permissions: [{2}]", ++cnt, hasIp, dir);

               Assert.AreEqual(hasIp, HasInheritedPermissions(dir));
            }
            catch (Exception ex)
            {
               Console.Write("\t#{0:000}\tCaught {1} for directory: [{2}]\t[{3}]\n", cnt, ex.GetType().FullName, dir, ex.Message.Replace(Environment.NewLine, "  "));
            }
         }
         Console.Write("\n{0}", UnitTestConstants.Reporter());
      }

      #endregion // AlphaFS
   }
}
