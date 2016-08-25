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
using Alphaleonis.Win32;
using Alphaleonis.Win32.Filesystem;
using Alphaleonis.Win32.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading;
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
      private void DumpCopy(bool isLocal)
      {
         #region Setup

         Console.WriteLine("\n=== TEST {0} ===", isLocal ? UnitTestConstants.Local : UnitTestConstants.Network);
         string tempPath = Path.GetTempPath("Directory.Copy()-") + Path.GetRandomFileName();
         if (!isLocal) tempPath = Path.LocalToUnc(tempPath);

         string tempPathSource = Path.Combine(tempPath, "Source");
         string tempPathDestination = Path.Combine(tempPath, "Destination");

         bool exception;
         int expectedLastError;
         string expectedException;
         string report;

         string letter = DriveInfo.GetFreeDriveLetter() + @":\";
         if (!isLocal) letter = Path.LocalToUnc(letter);

         string random = Path.GetRandomFileName();
         string fileSource = @"file-source-" + random + ".exe";
         string fileDestination = @"file-destination-" + random + ".exe";

         string folderSource = @"folder-source-" + random;
         string folderDestination = @"folder-destination-" + random;

         string fullPathSource = Path.Combine(tempPath, folderSource, fileSource);
         string fullPathDestinationParent = Path.Combine(tempPath, folderDestination);
         string fullPathDestination = Path.Combine(fullPathDestinationParent, fileDestination);
         if (!isLocal) fullPathSource = Path.LocalToUnc(fullPathSource);
         if (!isLocal) fullPathDestination = Path.LocalToUnc(fullPathDestination);

         DirectoryInfo dirInfoParent = new DirectoryInfo(fullPathDestinationParent);

         #endregion // Setup

         try
         {
            #region UnauthorizedAccessException

            DirectoryInfo dirInfo = Directory.CreateDirectory(fullPathSource);
            Directory.CreateDirectory(fullPathDestinationParent);

            DirectorySecurity dirSecurity;

            string user = (Environment.UserDomainName + @"\" + Environment.UserName).TrimStart('\\');

            // ╔═════════════╦═════════════╦═══════════════════════════════╦════════════════════════╦══════════════════╦═══════════════════════╦═════════════╦═════════════╗
            // ║             ║ folder only ║ folder, sub-folders and files ║ folder and sub-folders ║ folder and files ║ sub-folders and files ║ sub-folders ║    files    ║
            // ╠═════════════╬═════════════╬═══════════════════════════════╬════════════════════════╬══════════════════╬═══════════════════════╬═════════════╬═════════════╣
            // ║ Propagation ║ none        ║ none                          ║ none                   ║ none             ║ InheritOnly           ║ InheritOnly ║ InheritOnly ║
            // ║ Inheritance ║ none        ║ Container|Object              ║ Container              ║ Object           ║ Container|Object      ║ Container   ║ Object      ║
            // ╚═════════════╩═════════════╩═══════════════════════════════╩════════════════════════╩══════════════════╩═══════════════════════╩═════════════╩═════════════╝

            var rule = new FileSystemAccessRule(user, FileSystemRights.FullControl, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Deny);


            expectedLastError = (int)Win32Errors.ERROR_ACCESS_DENIED;
            expectedException = "System.UnauthorizedAccessException";
            exception = false;
            try
            {
               Console.WriteLine("\nCatch: [{0}]", expectedException);

               // Set DENY for current user.
               dirSecurity = dirInfoParent.GetAccessControl();
               dirSecurity.AddAccessRule(rule);
               dirInfoParent.SetAccessControl(dirSecurity);

               dirInfo.MoveTo(fullPathDestination);
            }
            catch (UnauthorizedAccessException ex)
            {
               Assert.IsTrue(ex.Message.StartsWith("(" + expectedLastError + ")"), string.Format("Expected Win32Exception error is: [{0}]", expectedLastError));

               exception = true;
               Console.WriteLine("\n\t[{0}]: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
            }
            catch (Exception ex)
            {
               Console.WriteLine("\n\tCaught (unexpected) {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
            }
            finally
            {
               // Remove DENY for current user.
               dirSecurity = dirInfoParent.GetAccessControl();
               dirSecurity.RemoveAccessRule(rule);
               dirInfoParent.SetAccessControl(dirSecurity, AccessControlSections.Access);

               Directory.Delete(tempPath, true, true);
            }
            Assert.IsTrue(exception, "[{0}] should have been caught.", expectedException);

            Console.WriteLine();

            #endregion // UnauthorizedAccessException

            #region DirectoryNotFoundException (UnitTestConstants.Local) / IOException (UnitTestConstants.Network)

            expectedLastError = (int)(isLocal ? Win32Errors.ERROR_PATH_NOT_FOUND : Win32Errors.ERROR_BAD_NET_NAME);
            expectedException = isLocal ? "System.IO.DirectoryNotFoundException" : "System.IO.IOException";
            exception = false;
            try
            {
               Console.WriteLine("\nCatch: [{0}]", expectedException);
               Directory.Copy(letter + folderSource, letter + folderDestination, CopyOptions.FailIfExists);
            }
            catch (Exception ex)
            {
               var win32Error = new Win32Exception("", ex);
               //Assert.IsTrue(win32Error.NativeErrorCode == expectedLastError, string.Format("Expected Win32Exception error should be: [{0}], got: [{1}]", expectedLastError, win32Error.NativeErrorCode));
               Assert.IsTrue(ex.Message.StartsWith("(" + expectedLastError + ")"), string.Format("Expected Win32Exception error is: [{0}]", expectedLastError));

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

            expectedLastError = (int)Win32Errors.ERROR_SAME_DRIVE;
            expectedException = "System.IO.IOException";
            exception = false;
            try
            {
               Console.WriteLine("\nCatch: [{0}]", expectedException);
               Directory.Move(letter + folderDestination, letter + folderDestination);
            }
            catch (IOException ex)
            {
               Assert.IsTrue(ex.Message.StartsWith("(" + expectedLastError + ")"), string.Format("Expected Win32Exception error is: [{0}]", expectedLastError));

               exception = true;
               Console.WriteLine("\n\t[{0}]: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
            }
            catch (Exception ex)
            {
               Console.WriteLine("\n\tCaught (unexpected) {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
            }
            Assert.IsTrue(exception, "[{0}] should have been caught.", expectedException);
            Console.WriteLine();

            #endregion // IOException

            #region Copy

            UnitTestConstants.CreateDirectoriesAndFiles(tempPathSource, 10, true);

            Dictionary<string, long> props = Directory.GetProperties(tempPathSource, DirectoryEnumerationOptions.Recursive);
            long sourceFolder = props["Directory"];
            long sourceFile = props["File"];
            long sourceSize = props["Size"];

            Console.WriteLine("\nCopy from Source Path: [{0}]", tempPathSource);
            Console.WriteLine("\tTotal Directories: [{0}] Files: [{1}] Size: [{2}]", sourceFolder, sourceFile, Utils.UnitSizeToText(sourceSize));

            UnitTestConstants.StopWatcher(true);
            Directory.Copy(tempPathSource, tempPathDestination, CopyOptions.FailIfExists);
            report = UnitTestConstants.Reporter();

            props = Directory.GetProperties(tempPathDestination, DirectoryEnumerationOptions.Recursive);
            long destinationFolder = props["Directory"];
            long destinationFile = props["File"];
            long destinationSize = props["Size"];

            Console.WriteLine("\nCopied to Destination Path: [{0}]", tempPathDestination);
            Console.WriteLine("\tTotal Directories: [{0}] Files: [{1}] Size: [{2}]{3}", destinationFolder, destinationFile, Utils.UnitSizeToText(destinationSize), report);

            #region IOException

            expectedLastError = (int)Win32Errors.ERROR_FILE_EXISTS;
            expectedException = "System.IO.IOException";
            exception = false;
            try
            {
               Console.WriteLine("\n\nCatch: [{0}]: Copy same directory again: destDirName already exists.", expectedException);
               Directory.Copy(tempPathSource, tempPathDestination, CopyOptions.FailIfExists);
            }
            catch (IOException ex)
            {
               var win32Error = new Win32Exception("", ex);
               Assert.IsTrue(win32Error.NativeErrorCode == expectedLastError, string.Format("Expected Win32Exception error should be: [{0}], got: [{1}]", expectedLastError, win32Error.NativeErrorCode));
               Assert.IsTrue(ex.Message.StartsWith("(" + expectedLastError + ")"), string.Format("Expected Win32Exception error is: [{0}]", expectedLastError));

               exception = true;
               Console.WriteLine("\n\t[{0}]: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
            }
            catch (Exception ex)
            {
               Console.WriteLine("\n\tCaught (unexpected) {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
            }
            Assert.IsTrue(exception, "[{0}] should have been caught.", expectedException);
            Console.WriteLine();

            #endregion // IOException

            // Overwrite.
            UnitTestConstants.StopWatcher(true);
            Directory.Copy(tempPathSource, tempPathDestination, CopyOptions.None | CopyOptions.NoBuffering);
            report = UnitTestConstants.Reporter();

            Console.WriteLine("\nCopy again with overwrite enabled.\n{0}", report);

            #endregion // Copy
         }
         finally
         {
            if (Directory.Exists(tempPath))
            {
               Directory.Delete(tempPath, true, true);
               Assert.IsFalse(Directory.Exists(tempPath), "Cleanup failed: Directory should have been removed.");
            }
            Console.WriteLine();
         }
      }

      private void DumpEnableDisableEncryption(bool isLocal)
      {
         Console.WriteLine("\n=== TEST {0} ===", isLocal ? UnitTestConstants.Local : UnitTestConstants.Network);
         string tempPath = Path.Combine(Path.GetTempPath(), "Directory.EnableDisableEncryption()-" + Path.GetRandomFileName());
         if (!isLocal) tempPath = Path.LocalToUnc(tempPath);

         Console.WriteLine("\nInput Directory Path: [{0}]", tempPath);

         const string disabled = "Disable=0";
         const string enabled = "Disable=1";
         string lineDisable = string.Empty;
         string deskTopIni = Path.Combine(tempPath, "Desktop.ini");

         Directory.CreateDirectory(tempPath);
         FileAttributes actual = File.GetAttributes(tempPath);


         string report = string.Empty;
         bool action = false;
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
         using (StreamReader streamRead = File.OpenText(deskTopIni))
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
         using (StreamReader streamRead = File.OpenText(deskTopIni))
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
         string tempPath = Path.Combine(Path.GetTempPath(), "Directory.EncryptDecrypt()-" + Path.GetRandomFileName());
         if (!isLocal) tempPath = Path.LocalToUnc(tempPath);

         Console.WriteLine("\nInput Directory Path: [{0}]", tempPath);


         int cnt;
         string report = "";
         Directory.CreateDirectory(tempPath);
         FileAttributes actual = File.GetAttributes(tempPath);
         bool action = (actual & FileAttributes.Encrypted) != 0;

         Console.WriteLine("\nDirectory Encrypted (Should be False): [{0}]\tAttributes: [{1}]", action, actual);
         Assert.IsFalse(action, "Encryption should be False");
         Assert.IsFalse((actual & FileAttributes.Encrypted) != 0, "Encryption should be False");

         // Create some directories and files.
         for (int i = 0; i < 5; i++)
         {
            string file = Path.Combine(tempPath, Path.GetRandomFileName());

            string dir = file + "-dir";
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
         #region Setup

         Console.WriteLine("\n=== TEST {0} ===", isLocal ? UnitTestConstants.Local : UnitTestConstants.Network);

         int cnt = 0;
         string searchPattern = Path.WildcardStarMatchAll;
         SearchOption searchOption = SearchOption.TopDirectoryOnly;

         bool exception;
         int expectedLastError;
         string expectedException;

         string random = Path.GetRandomFileName();
         string folderSource = @"folder-source-" + random;

         string originalLetter = DriveInfo.GetFreeDriveLetter() + @":";
         string letter = originalLetter + @"\";

         #endregion // Setup

         #region DirectoryNotFoundException (UnitTestConstants.Local) / IOException (UnitTestConstants.Network)

         expectedLastError = (int)(isLocal ? Win32Errors.ERROR_PATH_NOT_FOUND : Win32Errors.ERROR_BAD_NET_NAME);
         expectedException = isLocal ? "System.IO.DirectoryNotFoundException" : "System.IO.IOException";
         exception = false;
         try
         {
            Console.WriteLine("\nCatch: [{0}]", expectedException);

            string nonExistingPath = letter + folderSource;
            if (!isLocal) nonExistingPath = Path.LocalToUnc(nonExistingPath);

            new DirectoryInfo(nonExistingPath).EnumerateDirectories().Any();
         }
         catch (Exception ex)
         {
            var win32Error = new Win32Exception("", ex);
            Assert.IsTrue(win32Error.NativeErrorCode == expectedLastError, string.Format("Expected Win32Exception error should be: [{0}], got: [{1}]", expectedLastError, win32Error.NativeErrorCode));
            Assert.IsTrue(ex.Message.StartsWith("(" + expectedLastError + ")"), string.Format("Expected Win32Exception error is: [{0}]", expectedLastError));

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

         string tempPath = Path.GetTempPath("Directory.EnumerateDirectories-file-" + Path.GetRandomFileName());
         if (!isLocal) tempPath = Path.LocalToUnc(tempPath);

         try
         {
            using (File.Create(tempPath)) { }

            expectedLastError = (int)Win32Errors.ERROR_DIRECTORY;
            expectedException = "System.IO.IOException";
            exception = false;
            try
            {
               Console.WriteLine("\nCatch: [{0}]", expectedException);

               new DirectoryInfo(tempPath).EnumerateDirectories().Any();
            }
            catch (IOException ex)
            {
               var win32Error = new Win32Exception("", ex);
               Assert.IsTrue(win32Error.NativeErrorCode == expectedLastError, string.Format("Expected Win32Exception error should be: [{0}], got: [{1}]", expectedLastError, win32Error.NativeErrorCode));
               Assert.IsTrue(ex.Message.StartsWith("(" + expectedLastError + ")"), string.Format("Expected Win32Exception error is: [{0}]", expectedLastError));

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
         if (!isLocal) tempPath = Path.LocalToUnc(tempPath);

         var di = new DirectoryInfo(tempPath);
         if (di.Exists)
         {
            expectedLastError = (int)Win32Errors.ERROR_ACCESS_DENIED;
            expectedException = "System.UnauthorizedAccessException";
            exception = false;
            try
            {
               Console.WriteLine("\nCatch: [{0}]", expectedException);

               di.EnumerateDirectories(searchPattern, SearchOption.AllDirectories).All(o => o.Exists);
            }
            catch (UnauthorizedAccessException ex)
            {
               var win32Error = new Win32Exception("", ex);
               Assert.IsTrue(win32Error.NativeErrorCode == expectedLastError, string.Format("Expected Win32Exception error should be: [{0}], got: [{1}]", expectedLastError, win32Error.NativeErrorCode));
               Assert.IsTrue(ex.Message.StartsWith("(" + expectedLastError + ")"), string.Format("Expected Win32Exception error is: [{0}]", expectedLastError));

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

         string path = isLocal ? UnitTestConstants.SysRoot : Path.LocalToUnc(UnitTestConstants.SysRoot);

         Console.WriteLine("\nInput Directory Path: [{0}]\n", path);
         Console.WriteLine("\tEnumerate directories, using \"SearchOption.{0}\"\n", searchOption);

         UnitTestConstants.StopWatcher(true);
         foreach (DirectoryInfo dirInfo in new DirectoryInfo(path).EnumerateDirectories(searchPattern, searchOption))
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
         foreach (DirectoryInfo dirInfo in new DirectoryInfo(path).EnumerateDirectories(searchPattern, searchOption))
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

         foreach (string dir in Directory.EnumerateDirectories(UnitTestConstants.SysRoot, DirectoryEnumerationOptions.FilesAndFolders))
            Assert.IsTrue((File.GetAttributes(dir) & FileAttributes.Directory) != 0, string.Format("Expected a folder, not a file: [{0}]", dir));

         foreach (string dir in Directory.EnumerateDirectories(UnitTestConstants.SysRoot, DirectoryEnumerationOptions.Files))
            Assert.IsTrue((File.GetAttributes(dir) & FileAttributes.Directory) != 0, string.Format("Expected a folder, not a file: [{0}]", dir));

         #endregion // DirectoryEnumerationOptions

         Console.WriteLine();
      }

      private void DumpEnumerateFileIdBothDirectoryInfo(bool isLocal)
      {
         Console.WriteLine("\n=== TEST {0} ===", isLocal ? UnitTestConstants.Local : UnitTestConstants.Network);
         string tempPath = UnitTestConstants.SysRoot;
         if (!isLocal) tempPath = Path.LocalToUnc(tempPath);

         string searchPattern = Path.WildcardStarMatchAll;

         long directories = Directory.CountFileSystemObjects(tempPath, searchPattern, DirectoryEnumerationOptions.Folders);
         long files = Directory.CountFileSystemObjects(tempPath, searchPattern, DirectoryEnumerationOptions.Files);

         Console.WriteLine("\nInput Directory Path: [{0}]\tCounted: Directories = [{1}] Files = [{2}]", tempPath, directories, files);

         bool foundFse = false;
         long numDirectories = 0;
         long numFiles = 0;

         UnitTestConstants.StopWatcher(true);
         foreach (FileIdBothDirectoryInfo fibdi in Directory.EnumerateFileIdBothDirectoryInfo(tempPath))
         {
            if ((fibdi.FileAttributes & FileAttributes.Directory) != 0)
               numDirectories++;
            else
               numFiles++;

            foundFse = UnitTestConstants.Dump(fibdi, -22);
         }
         string report = UnitTestConstants.Reporter();

         Console.WriteLine("\n\tEnumerated: Directories = [{0}] Files = [{1}]\t{2}", numDirectories, numFiles, report);

         if (!foundFse)
            Assert.Inconclusive("Nothing was enumerated.");

         bool matchAll = directories == numDirectories && files == numFiles;
         Assert.IsTrue(matchAll, "Number of directories and/or files don't match.");

         Console.WriteLine();
      }

      private void DumpEnumerateFiles(bool isLocal)
      {
         #region Setup

         Console.WriteLine("\n=== TEST {0} ===", isLocal ? UnitTestConstants.Local : UnitTestConstants.Network);

         int cnt = 0;
         string searchPattern = Path.WildcardStarMatchAll;
         SearchOption searchOption = SearchOption.TopDirectoryOnly;

         bool exception;
         int expectedLastError;
         string expectedException;

         string random = Path.GetRandomFileName();
         string folderSource = @"folder-source-" + random;

         string originalLetter = DriveInfo.GetFreeDriveLetter() + @":";
         string letter = originalLetter + @"\";

         #endregion // Setup

         #region DirectoryNotFoundException (UnitTestConstants.Local) / IOException (UnitTestConstants.Network)

         expectedLastError = (int)(isLocal ? Win32Errors.ERROR_PATH_NOT_FOUND : Win32Errors.ERROR_BAD_NET_NAME);
         expectedException = isLocal ? "System.IO.DirectoryNotFoundException" : "System.IO.IOException";
         exception = false;
         try
         {
            Console.WriteLine("\nCatch: [{0}]", expectedException);

            string nonExistingPath = letter + folderSource;
            if (!isLocal) nonExistingPath = Path.LocalToUnc(nonExistingPath);

            new DirectoryInfo(nonExistingPath).EnumerateFiles().Any();
         }
         catch (Exception ex)
         {
            // win32Error is always 0
            var win32Error = new Win32Exception("", ex);
            Assert.IsTrue(win32Error.NativeErrorCode == expectedLastError, string.Format("Expected Win32Exception error should be: [{0}], got: [{1}]", expectedLastError, win32Error.NativeErrorCode));
            Assert.IsTrue(ex.Message.StartsWith("(" + expectedLastError + ")"), string.Format("Expected Win32Exception error is: [{0}]", expectedLastError));

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

         string tempPath = Path.GetTempPath("Directory.EnumerateFiles-file-" + Path.GetRandomFileName());
         if (!isLocal) tempPath = Path.LocalToUnc(tempPath);

         try
         {
            using (File.Create(tempPath)) { }

            expectedLastError = (int)Win32Errors.ERROR_DIRECTORY;
            expectedException = "System.IO.IOException";
            exception = false;
            try
            {
               Console.WriteLine("\nCatch: [{0}]", expectedException);
               new DirectoryInfo(tempPath).EnumerateFiles().Any();
            }
            catch (IOException ex)
            {
               Assert.IsTrue(ex.Message.StartsWith("(" + expectedLastError + ")"), string.Format("Expected Win32Exception error is: [{0}]", expectedLastError));

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
         if (!isLocal) tempPath = Path.LocalToUnc(tempPath);

         var di = new DirectoryInfo(tempPath);
         if (di.Exists)
         {
            expectedLastError = (int)Win32Errors.ERROR_ACCESS_DENIED;
            expectedException = "System.UnauthorizedAccessException";
            exception = false;
            try
            {
               Console.WriteLine("\nCatch: [{0}]", expectedException);
               di.EnumerateFiles(searchPattern, SearchOption.AllDirectories).All(o => o.Exists);
            }
            catch (UnauthorizedAccessException ex)
            {
               var win32Error = new Win32Exception("", ex);
               Assert.IsTrue(win32Error.NativeErrorCode == expectedLastError, string.Format("Expected Win32Exception error should be: [{0}], got: [{1}]", expectedLastError, win32Error.NativeErrorCode));
               Assert.IsTrue(ex.Message.StartsWith("(" + expectedLastError + ")"), string.Format("Expected Win32Exception error is: [{0}]", expectedLastError));

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

         string path = isLocal ? UnitTestConstants.SysRoot : Path.LocalToUnc(UnitTestConstants.SysRoot);

         Console.WriteLine("\nInput Directory Path: [{0}]\n", path);
         Console.WriteLine("\tEnumerate files, using \"SearchOption.{0}\"\n", searchOption);

         UnitTestConstants.StopWatcher(true);
         foreach (FileInfo fileInfo in new DirectoryInfo(path).EnumerateFiles(searchPattern, searchOption))
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
         foreach (FileInfo fileInfo in new DirectoryInfo(path).EnumerateFiles(searchPattern, searchOption))
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

         foreach (string file in Directory.EnumerateFiles(UnitTestConstants.SysRoot, DirectoryEnumerationOptions.FilesAndFolders))
            Assert.IsTrue((File.GetAttributes(file) & FileAttributes.Directory) == 0, string.Format("Expected a file, not a folder: [{0}]", file));

         foreach (string file in Directory.EnumerateFiles(UnitTestConstants.SysRoot, DirectoryEnumerationOptions.Folders))
            Assert.IsTrue((File.GetAttributes(file) & FileAttributes.Directory) == 0, string.Format("Expected a file, not a folder: [{0}]", file));

         #endregion // DirectoryEnumerationOptions

         Console.WriteLine();
      }

      private void DumpEnumerateFileSystemEntries(bool isLocal)
      {
         #region Setup

         Console.WriteLine("\n=== TEST {0} ===", isLocal ? UnitTestConstants.Local : UnitTestConstants.Network);

         int cnt = 0;
         string searchPattern = Path.WildcardStarMatchAll;
         SearchOption searchOption = SearchOption.TopDirectoryOnly;

         bool exception;
         int expectedLastError;
         string expectedException;

         string random = Path.GetRandomFileName();
         string folderSource = @"folder-source-" + random;

         string originalLetter = DriveInfo.GetFreeDriveLetter() + @":";
         string letter = originalLetter + @"\";

         #endregion // Setup

         #region DirectoryNotFoundException (UnitTestConstants.Local) / IOException (UnitTestConstants.Network)

         expectedLastError = (int)(isLocal ? Win32Errors.ERROR_PATH_NOT_FOUND : Win32Errors.ERROR_BAD_NET_NAME);
         expectedException = isLocal ? "System.IO.DirectoryNotFoundException" : "System.IO.IOException";
         exception = false;
         try
         {
            Console.WriteLine("\nCatch: [{0}]", expectedException);

            string nonExistingPath = letter + folderSource;
            if (!isLocal) nonExistingPath = Path.LocalToUnc(nonExistingPath);

            new DirectoryInfo(nonExistingPath).EnumerateFileSystemInfos().Any();
         }
         catch (Exception ex)
         {
            var win32Error = new Win32Exception("", ex);
            Assert.IsTrue(win32Error.NativeErrorCode == expectedLastError, string.Format("Expected Win32Exception error should be: [{0}], got: [{1}]", expectedLastError, win32Error.NativeErrorCode));
            Assert.IsTrue(ex.Message.StartsWith("(" + expectedLastError + ")"), string.Format("Expected Win32Exception error is: [{0}]", expectedLastError));

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

         string tempPath = Path.GetTempPath("Directory.EnumerateFileSystemEntries-file-" + Path.GetRandomFileName());
         if (!isLocal) tempPath = Path.LocalToUnc(tempPath);

         try
         {
            using (File.Create(tempPath)) { }

            expectedLastError = (int)Win32Errors.ERROR_DIRECTORY;
            expectedException = "System.IO.IOException";
            exception = false;
            try
            {
               Console.WriteLine("\nCatch: [{0}]", expectedException);
               new DirectoryInfo(tempPath).EnumerateFileSystemInfos().Any();
            }
            catch (IOException ex)
            {
               Assert.IsTrue(ex.Message.StartsWith("(" + expectedLastError + ")"), string.Format("Expected Win32Exception error is: [{0}]", expectedLastError));

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
         if (!isLocal) tempPath = Path.LocalToUnc(tempPath);

         var di = new DirectoryInfo(tempPath);
         if (di.Exists)
         {
            expectedLastError = (int)Win32Errors.ERROR_ACCESS_DENIED;
            expectedException = "System.UnauthorizedAccessException";
            exception = false;
            try
            {
               Console.WriteLine("\nCatch: [{0}]", expectedException);
               di.EnumerateFileSystemInfos(searchPattern, SearchOption.AllDirectories).All(o => o.Exists);
            }
            catch (UnauthorizedAccessException ex)
            {
               var win32Error = new Win32Exception("", ex);
               Assert.IsTrue(win32Error.NativeErrorCode == expectedLastError, string.Format("Expected Win32Exception error should be: [{0}], got: [{1}]", expectedLastError, win32Error.NativeErrorCode));
               Assert.IsTrue(ex.Message.StartsWith("(" + expectedLastError + ")"), string.Format("Expected Win32Exception error is: [{0}]", expectedLastError));

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

         string path = isLocal ? UnitTestConstants.SysRoot : Path.LocalToUnc(UnitTestConstants.SysRoot);

         Console.WriteLine("\nInput Directory Path: [{0}]\n", path);
         Console.WriteLine("\tEnumerate file system entries, using \"SearchOption.{0}\"\n", searchOption);

         UnitTestConstants.StopWatcher(true);
         foreach (FileSystemInfo fsi in new DirectoryInfo(path).EnumerateFileSystemInfos(searchPattern, searchOption))
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

         foreach (string dir in Directory.EnumerateFileSystemEntries(UnitTestConstants.SysRoot, DirectoryEnumerationOptions.Folders))
            Assert.IsTrue((File.GetAttributes(dir) & FileAttributes.Directory) != 0, string.Format("Expected a folder, not a file: [{0}]", dir));


         // Should only return files.

         foreach (string file in Directory.EnumerateFileSystemEntries(UnitTestConstants.SysRoot, DirectoryEnumerationOptions.Files))
            Assert.IsTrue((File.GetAttributes(file) & FileAttributes.Directory) == 0, string.Format("Expected a file, not a folder: [{0}]", file));

         #endregion // DirectoryEnumerationOptions

         Console.WriteLine();
      }
      
      private void DumpGetDrives(bool enumerate)
      {
         Console.WriteLine("\nIf you are missing drives, please see this topic: https://alphafs.codeplex.com/discussions/397693 \n");

         int cnt = 0;
         UnitTestConstants.StopWatcher(true);
         foreach (DriveInfo actual in enumerate ? Directory.EnumerateLogicalDrives(false, false) : DriveInfo.GetDrives())
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
         #region Setup

         Console.WriteLine("\n=== TEST {0} ===", isLocal ? UnitTestConstants.Local : UnitTestConstants.Network);

         int cnt = 0;
         string searchPattern = Path.WildcardStarMatchAll;
         SearchOption searchOption = SearchOption.TopDirectoryOnly;

         bool exception;
         int expectedLastError;
         string expectedException;

         string random = Path.GetRandomFileName();
         string folderSource = @"folder-source-" + random;

         string originalLetter = DriveInfo.GetFreeDriveLetter() + @":";
         string letter = originalLetter + @"\";

         #endregion // Setup

         #region DirectoryNotFoundException (UnitTestConstants.Local) / IOException (UnitTestConstants.Network)
         
         expectedLastError = (int)(isLocal ? Win32Errors.ERROR_PATH_NOT_FOUND : Win32Errors.ERROR_BAD_NET_NAME);
         expectedException = isLocal ? "System.IO.DirectoryNotFoundException" : "System.IO.IOException";
         exception = false;
         try
         {
            Console.WriteLine("\nCatch: [{0}]", expectedException);

            string nonExistingPath = letter + folderSource;
            if (!isLocal) nonExistingPath = Path.LocalToUnc(nonExistingPath);

            Directory.GetFileSystemEntries(nonExistingPath);
         }
         catch (Exception ex)
         {
            // win32Error is always 0
            var win32Error = new Win32Exception("", ex);
            Assert.IsTrue(win32Error.NativeErrorCode == expectedLastError, string.Format("Expected Win32Exception error should be: [{0}], got: [{1}]", expectedLastError, win32Error.NativeErrorCode));
            Assert.IsTrue(ex.Message.StartsWith("(" + expectedLastError + ")"), string.Format("Expected Win32Exception error is: [{0}]", expectedLastError));

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

         string tempPath = Path.GetTempPath("Directory.GetDirectories-file-" + Path.GetRandomFileName());
         if (!isLocal) tempPath = Path.LocalToUnc(tempPath);

         try
         {
            using (File.Create(tempPath)) { }

            expectedLastError = (int)Win32Errors.ERROR_DIRECTORY;
            expectedException = "System.IO.IOException";
            exception = false;
            try
            {
               Console.WriteLine("\nCatch: [{0}]", expectedException);
               Directory.GetFileSystemEntries(tempPath).Any();
            }
            catch (IOException ex)
            {
               Assert.IsTrue(ex.Message.StartsWith("(" + expectedLastError + ")"), string.Format("Expected Win32Exception error is: [{0}]", expectedLastError));

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
         if (!isLocal) tempPath = Path.LocalToUnc(tempPath);

         if (Directory.Exists(tempPath))
         {
            expectedLastError = (int)Win32Errors.ERROR_ACCESS_DENIED;
            expectedException = "System.UnauthorizedAccessException";
            exception = false;
            try
            {
               Console.WriteLine("\nCatch: [{0}]", expectedException);
               Directory.GetFileSystemEntries(tempPath, searchPattern, SearchOption.AllDirectories).Any();
            }
            catch (UnauthorizedAccessException ex)
            {
               var win32Error = new Win32Exception("", ex);
               Assert.IsTrue(win32Error.NativeErrorCode == expectedLastError, string.Format("Expected Win32Exception error should be: [{0}], got: [{1}]", expectedLastError, win32Error.NativeErrorCode));
               Assert.IsTrue(ex.Message.StartsWith("(" + expectedLastError + ")"), string.Format("Expected Win32Exception error is: [{0}]", expectedLastError));

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

         string path = isLocal ? UnitTestConstants.SysRoot : Path.LocalToUnc(UnitTestConstants.SysRoot);

         Console.WriteLine("\nInput Directory Path: [{0}]\n", path);
         Console.WriteLine("\tGet FileSystemEntries, using \"SearchOption.{0}\"\n", searchOption);

         UnitTestConstants.StopWatcher(true);
         foreach (string folder in Directory.GetFileSystemEntries(path, searchPattern, searchOption))
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
         string path = isLocal ? UnitTestConstants.SysRoot : Path.LocalToUnc(UnitTestConstants.SysRoot);

         Console.WriteLine("\n\tAggregated properties of file system objects from Directory: [{0}]\n", path);

         UnitTestConstants.StopWatcher(true);
         Dictionary<string, long> props = Directory.GetProperties(path, DirectoryEnumerationOptions.FilesAndFolders | DirectoryEnumerationOptions.Recursive | DirectoryEnumerationOptions.ContinueOnException);
         string report = UnitTestConstants.Reporter();

         long total = props["Total"];
         long file = props["File"];
         long size = props["Size"];
         int cnt = 0;

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

      private void DumpMove(bool isLocal)
      {
         #region Setup

         Console.WriteLine("\n=== TEST {0} ===", isLocal ? UnitTestConstants.Local : UnitTestConstants.Network);
         string tempPath = Path.GetTempPath("Directory.Move()-" + Path.GetRandomFileName());
         if (!isLocal) tempPath = Path.LocalToUnc(tempPath);

         string tempPathSource = Path.Combine(tempPath, "Source");
         string tempPathSource0 = Path.Combine(tempPath, "Source0");
         string tempPathDestination = Path.Combine(tempPath, "Destination");

         bool exception;
         int expectedLastError;
         string report;

         string random = Path.GetRandomFileName();
         string fileSource = @"folder2-source-" + random;
         string fileDestination = @"folder2-destination-" + random;

         string folderSource = @"folder1-source-" + random;
         string folderDestination = @"folder1-destination-" + random;

         string originalLetter = DriveInfo.GetFreeDriveLetter() + @":";
         string letter = originalLetter + @"\";
         string otherDisk = letter + folderDestination;

         if (!isLocal) letter = Path.LocalToUnc(letter);

         string fullPathSource = Path.Combine(tempPath, folderSource, fileSource);
         string fullPathDestinationParent = Path.Combine(tempPath, folderDestination);
         string fullPathDestination = Path.Combine(fullPathDestinationParent, fileDestination);
         if (!isLocal) fullPathSource = Path.LocalToUnc(fullPathSource);
         if (!isLocal) fullPathDestinationParent = Path.LocalToUnc(fullPathDestinationParent);
         if (!isLocal) fullPathDestination = Path.LocalToUnc(fullPathDestination);

         var dirInfoParent = new DirectoryInfo(fullPathDestinationParent);

         #endregion // Setup

         try
         {
            #region UnauthorizedAccessException

            DirectoryInfo dirInfo = Directory.CreateDirectory(fullPathSource);
            Directory.CreateDirectory(fullPathDestinationParent);

            DirectorySecurity dirSecurity;

            string user = (Environment.UserDomainName + @"\" + Environment.UserName).TrimStart('\\');

            // ╔═════════════╦═════════════╦═══════════════════════════════╦════════════════════════╦══════════════════╦═══════════════════════╦═════════════╦═════════════╗
            // ║             ║ folder only ║ folder, sub-folders and files ║ folder and sub-folders ║ folder and files ║ sub-folders and files ║ sub-folders ║    files    ║
            // ╠═════════════╬═════════════╬═══════════════════════════════╬════════════════════════╬══════════════════╬═══════════════════════╬═════════════╬═════════════╣
            // ║ Propagation ║ none        ║ none                          ║ none                   ║ none             ║ InheritOnly           ║ InheritOnly ║ InheritOnly ║
            // ║ Inheritance ║ none        ║ Container|Object              ║ Container              ║ Object           ║ Container|Object      ║ Container   ║ Object      ║
            // ╚═════════════╩═════════════╩═══════════════════════════════╩════════════════════════╩══════════════════╩═══════════════════════╩═════════════╩═════════════╝

            var rule = new FileSystemAccessRule(user, FileSystemRights.FullControl, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Deny);


            expectedLastError = (int)Win32Errors.ERROR_ACCESS_DENIED;
            string expectedException = "System.UnauthorizedAccessException";
            exception = false;
            try
            {
               Console.WriteLine("\nCatch: [{0}]", expectedException);

               // Set DENY for current user.
               dirSecurity = dirInfoParent.GetAccessControl();
               dirSecurity.AddAccessRule(rule);
               dirInfoParent.SetAccessControl(dirSecurity);

               dirInfo.MoveTo(fullPathDestination);
            }
            catch (UnauthorizedAccessException ex)
            {
               Assert.IsTrue(ex.Message.StartsWith("(" + expectedLastError + ")"), string.Format("Expected Win32Exception error is: [{0}]", expectedLastError));

               exception = true;
               Console.WriteLine("\n\t[{0}]: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
            }
            catch (Exception ex)
            {
               Console.WriteLine("\n\tCaught (unexpected) {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
            }
            finally
            {
               // Remove DENY for current user.
               dirSecurity = dirInfoParent.GetAccessControl();
               dirSecurity.RemoveAccessRule(rule);
               dirInfoParent.SetAccessControl(dirSecurity, AccessControlSections.Access);

               Directory.Delete(tempPath, true, true);
            }
            Assert.IsTrue(exception, "[{0}] should have been caught.", expectedException);

            Console.WriteLine();

            #endregion // UnauthorizedAccessException

            Volume.DefineDosDevice(originalLetter, Path.GetDirectoryName(tempPathDestination));

            #region DirectoryNotFoundException

            expectedLastError = (int)(isLocal ? Win32Errors.ERROR_PATH_NOT_FOUND : Win32Errors.ERROR_BAD_NET_NAME);
            expectedException = "System.IO.DirectoryNotFoundException"; // Exception is the same, even though last error is different.
            exception = false;
            try
            {
               Console.WriteLine("\nCatch: [{0}]", expectedException);
               Directory.Move(letter + folderSource, letter + folderDestination);
            }
            catch (DirectoryNotFoundException ex)
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

            #endregion // DirectoryNotFoundException

            #region IOException #1

            expectedLastError = (int)Win32Errors.ERROR_SAME_DRIVE;
            expectedException = "System.IO.IOException";
            exception = false;
            try
            {
               Console.WriteLine("\nCatch: [{0}]", expectedException);
               Directory.Move(letter + folderDestination, letter + folderDestination);
            }
            catch (IOException ex)
            {
               Assert.IsTrue(ex.Message.StartsWith("(" + expectedLastError + ")"), string.Format("Expected Win32Exception error is: [{0}]", expectedLastError));

               exception = true;
               Console.WriteLine("\n\t[{0}]: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
            }
            catch (Exception ex)
            {
               Console.WriteLine("\n\tCaught (unexpected) {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
            }
            Assert.IsTrue(exception, "[{0}] should have been caught.", expectedException);
            Console.WriteLine();

            #endregion // IOException #1

            #region IOException #2

            expectedLastError = (int)Win32Errors.ERROR_NOT_SAME_DEVICE;
            expectedException = "System.IO.IOException";
            exception = false;
            try
            {
               Console.WriteLine("\nCatch: [{0}]", expectedException);
               Directory.Move(UnitTestConstants.SysDrive + @"\" + folderSource, otherDisk);
            }
            catch (IOException ex)
            {
               Assert.IsTrue(ex.Message.StartsWith("(" + expectedLastError + ")"), string.Format("Expected Win32Exception error is: [{0}]", expectedLastError));

               exception = true;
               Console.WriteLine("\n\t[{0}]: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
            }
            catch (Exception ex)
            {
               Console.WriteLine("\n\tCaught (unexpected) {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
            }
            Assert.IsTrue(exception, "[{0}] should have been caught.", expectedException);
            Console.WriteLine();

            #endregion // IOException #2

            #region Move

            UnitTestConstants.CreateDirectoriesAndFiles(tempPathSource0, 10, true);
            Directory.Copy(tempPathSource0, otherDisk, CopyOptions.FailIfExists);

            Dictionary<string, long> props = Directory.GetProperties(otherDisk, DirectoryEnumerationOptions.Recursive);
            long sourceFolder = props["Directory"];
            long sourceFile = props["File"];
            long sourceSize = props["Size"];

            Console.WriteLine("\nMove from Source Path: [{0}]", otherDisk);
            Console.WriteLine("\tTotal Directories: [{0}] Files: [{1}] Size: [{2}]", sourceFolder, sourceFile, Utils.UnitSizeToText(sourceSize));

            UnitTestConstants.StopWatcher(true);
            Directory.Move(otherDisk, tempPathSource, MoveOptions.CopyAllowed);
            report = UnitTestConstants.Reporter();

            props = Directory.GetProperties(tempPathSource, DirectoryEnumerationOptions.Recursive);
            long destinationFolder = props["Directory"];
            long destinationFile = props["File"];
            long destinationSize = props["Size"];

            Console.WriteLine("\nMoved to Destination Path: [{0}]", tempPathSource);
            Console.WriteLine("\tTotal Directories: [{0}] Files: [{1}] Size: [{2}]{3}", destinationFolder, destinationFile, Utils.UnitSizeToText(destinationSize), report);

            Assert.AreEqual(sourceFolder, destinationFolder, "Total number of directories should match.");
            Assert.AreEqual(sourceFile, destinationFile, "Total number of files should match.");
            Assert.AreEqual(sourceSize, destinationSize, "Total number of bytes should match.");

            #region IOException

            Directory.Copy(tempPathSource0, otherDisk, CopyOptions.FailIfExists);

            expectedLastError = (int)Win32Errors.ERROR_ALREADY_EXISTS;
            expectedException = "System.IO.IOException";
            exception = false;
            try
            {
               Console.WriteLine("\n\nCatch: [{0}]: Move same directory again: destDirName already exists.", expectedException);
               Directory.Move(otherDisk, tempPathSource, MoveOptions.CopyAllowed);
            }
            catch (IOException ex)
            {
               Win32Exception win32Error = new Win32Exception("", ex.InnerException);
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

            #endregion // IOException

            // Overwrite.
            UnitTestConstants.StopWatcher(true);
            Directory.Move(otherDisk, tempPathSource, MoveOptions.CopyAllowed | MoveOptions.ReplaceExisting);
            report = UnitTestConstants.Reporter();

            Console.WriteLine("\nMove again with overwrite enabled and other volume allowed.\n{0}", report);

            #endregion // Move
         }
         finally
         {
            Volume.DeleteDosDevice(originalLetter);

            if (Directory.Exists(tempPath))
            {
               Directory.Delete(tempPath, true, true);
               Assert.IsFalse(Directory.Exists(tempPath), "Cleanup failed: Directory should have been removed.");
            }
            Console.WriteLine();
         }
      }

      private void DumpSetTimestamps(bool isLocal)
      {
         Console.WriteLine("\n=== TEST {0} ===", isLocal ? UnitTestConstants.Local : UnitTestConstants.Network);
         string path = Path.Combine(Path.GetTempPath(), "Directory.SetTimestamps()-directory-" + Path.GetRandomFileName());
         if (!isLocal) path = Path.LocalToUnc(path);
         string symlinkPath = path + "-symlink";
         var rnd = new Random();

         Console.WriteLine("\nInput Directory Path: [{0}]", path);

         Directory.CreateDirectory(path);
         File.CreateSymbolicLink(symlinkPath, path, SymbolicLinkTarget.Directory);

         DateTime creationTime = new DateTime(rnd.Next(1971, 2071), rnd.Next(1, 12), rnd.Next(1, 28), rnd.Next(0, 23), rnd.Next(0, 59), rnd.Next(0, 59));
         DateTime lastAccessTime = new DateTime(rnd.Next(1971, 2071), rnd.Next(1, 12), rnd.Next(1, 28), rnd.Next(0, 23), rnd.Next(0, 59), rnd.Next(0, 59));
         DateTime lastWriteTime = new DateTime(rnd.Next(1971, 2071), rnd.Next(1, 12), rnd.Next(1, 28), rnd.Next(0, 23), rnd.Next(0, 59), rnd.Next(0, 59));

         Console.WriteLine("\nSet timestamps to:\n");
         Console.WriteLine("\tCreationTime  : [{0}]", creationTime);
         Console.WriteLine("\tLastAccessTime: [{0}]", lastAccessTime);
         Console.WriteLine("\tLastWriteTime : [{0}]", lastWriteTime);
         Console.WriteLine();

         Directory.SetTimestamps(path, creationTime, lastAccessTime, lastWriteTime);

         DateTime actual = Directory.GetCreationTime(path);
         DateTime expected = System.IO.Directory.GetCreationTime(path);
         Console.WriteLine("\tGetCreationTime()   AlphaFS: [{0}]    System.IO: [{1}]", actual, expected);
         Assert.AreEqual(expected, actual, "AlphaFS != System.IO");

         actual = Directory.GetLastAccessTime(path);
         expected = System.IO.Directory.GetLastAccessTime(path);
         Console.WriteLine("\tGetLastAccessTime() AlphaFS: [{0}]    System.IO: [{1}]", actual, expected);
         Assert.AreEqual(expected, actual, "AlphaFS != System.IO");

         actual = Directory.GetLastWriteTime(path);
         expected = System.IO.Directory.GetLastWriteTime(path);
         Console.WriteLine("\tGetLastWriteTime()  AlphaFS: [{0}]    System.IO: [{1}]", actual, expected);
         Assert.AreEqual(expected, actual, "AlphaFS != System.IO");

         Directory.SetTimestamps(symlinkPath, creationTime.AddDays(1), lastAccessTime.AddDays(1), lastWriteTime.AddDays(1), true, PathFormat.RelativePath);
         Assert.AreEqual(System.IO.Directory.GetCreationTime(symlinkPath), Directory.GetCreationTime(symlinkPath), "AlphaFS != System.IO");
         Assert.AreEqual(System.IO.Directory.GetLastAccessTime(symlinkPath), Directory.GetLastAccessTime(symlinkPath), "AlphaFS != System.IO");
         Assert.AreEqual(System.IO.Directory.GetLastWriteTime(symlinkPath), Directory.GetLastWriteTime(symlinkPath), "AlphaFS != System.IO");



         creationTime = new DateTime(rnd.Next(1971, 2071), rnd.Next(1, 12), rnd.Next(1, 28), rnd.Next(0, 23), rnd.Next(0, 59), rnd.Next(0, 59));
         lastAccessTime = new DateTime(rnd.Next(1971, 2071), rnd.Next(1, 12), rnd.Next(1, 28), rnd.Next(0, 23), rnd.Next(0, 59), rnd.Next(0, 59));
         lastWriteTime = new DateTime(rnd.Next(1971, 2071), rnd.Next(1, 12), rnd.Next(1, 28), rnd.Next(0, 23), rnd.Next(0, 59), rnd.Next(0, 59));

         Console.WriteLine("\nSet timestampsUtc to:\n");
         Console.WriteLine("\tCreationTimeUtc  : [{0}]", creationTime);
         Console.WriteLine("\tLastAccessTimeUtc: [{0}]", lastAccessTime);
         Console.WriteLine("\tLastWriteTimeUtc : [{0}]", lastWriteTime);
         Console.WriteLine();

         Directory.SetTimestampsUtc(path, creationTime, lastAccessTime, lastWriteTime);

         actual = Directory.GetCreationTimeUtc(path);
         expected = System.IO.Directory.GetCreationTimeUtc(path);
         Console.WriteLine("\tGetCreationTimeUtc()   AlphaFS: [{0}]    System.IO: [{1}]", actual, expected);
         Assert.AreEqual(expected, actual, "AlphaFS != System.IO");

         actual = Directory.GetLastAccessTimeUtc(path);
         expected = System.IO.Directory.GetLastAccessTimeUtc(path);
         Console.WriteLine("\tGetLastAccessTimeUtc() AlphaFS: [{0}]    System.IO: [{1}]", actual, expected);
         Assert.AreEqual(expected, actual, "AlphaFS != System.IO");

         actual = Directory.GetLastWriteTimeUtc(path);
         expected = System.IO.Directory.GetLastWriteTimeUtc(path);
         Console.WriteLine("\tGetLastWriteTimeUtc()  AlphaFS: [{0}]    System.IO: [{1}]", actual, expected);
         Assert.AreEqual(expected, actual, "AlphaFS != System.IO");

         Directory.SetTimestamps(symlinkPath, creationTime.AddDays(1), lastAccessTime.AddDays(1), lastWriteTime.AddDays(1), true, PathFormat.RelativePath);
         Assert.AreEqual(System.IO.Directory.GetCreationTimeUtc(symlinkPath), Directory.GetCreationTimeUtc(symlinkPath), "AlphaFS != System.IO");
         Assert.AreEqual(System.IO.Directory.GetLastAccessTimeUtc(symlinkPath), Directory.GetLastAccessTimeUtc(symlinkPath), "AlphaFS != System.IO");
         Assert.AreEqual(System.IO.Directory.GetLastWriteTimeUtc(symlinkPath), Directory.GetLastWriteTimeUtc(symlinkPath), "AlphaFS != System.IO");


         Directory.Delete(symlinkPath);
         Assert.IsFalse(Directory.Exists(symlinkPath), "Cleanup failed: Directory symlink should have been removed.");
         Directory.Delete(path);
         Assert.IsTrue(!Directory.Exists(path), "Cleanup failed: Directory should have been removed.");
         Console.WriteLine();
      }

      private void DumpSetXxxTime(bool isLocal)
      {
         Console.WriteLine("\n=== TEST {0} ===", isLocal ? UnitTestConstants.Local : UnitTestConstants.Network);
         string path = Path.Combine(Path.GetTempPath(), "Directory.SetCreationTime()-" + Path.GetRandomFileName());
         if (!isLocal) path = Path.LocalToUnc(path);
         string symlinkPath = path + "-symlink";

         Console.WriteLine("\nInput Path: [{0}]", path);

         Directory.CreateDirectory(path);
         File.CreateSymbolicLink(symlinkPath, path, SymbolicLinkTarget.Directory);

         var rnd = new Random();

         #region SetCreationTime/Utc
         DateTime creationTime = new DateTime(rnd.Next(1971, 2071), rnd.Next(1, 12), rnd.Next(1, 28), rnd.Next(0, 23), rnd.Next(0, 59), rnd.Next(1, 59));
         Console.WriteLine("\n\tSetCreationTime() to: [{0} {1}]", creationTime, creationTime.ToLongTimeString());
         Directory.SetCreationTime(path, creationTime);
         DateTime actual = Directory.GetCreationTime(path);
         System.IO.Directory.SetCreationTime(path, creationTime);
         DateTime expected = System.IO.Directory.GetCreationTime(path);
         Console.WriteLine("\t\tAlphaFS  : [{0}]\n\t\tSystem.IO: [{1}]", actual, expected);
         Assert.AreEqual(expected, actual, "AlphaFS != System.IO");
         Directory.SetCreationTime(symlinkPath, creationTime.AddDays(1), true, PathFormat.RelativePath);
         Assert.AreEqual(expected, Directory.GetCreationTime(path), "SetCreationTime modify-reparse-point should not have altered the underlying directory's timestamp");
         expected = System.IO.Directory.GetCreationTime(symlinkPath);
         Assert.AreEqual(expected, Directory.GetCreationTime(symlinkPath), "AlphaFS != System.IO");
         Assert.AreEqual(creationTime.AddDays(1), expected, "Time set != time read back");


         creationTime = new DateTime(rnd.Next(1971, 2071), rnd.Next(1, 12), rnd.Next(1, 28), rnd.Next(0, 23), rnd.Next(0, 59), rnd.Next(1, 59));
         Console.WriteLine("\n\tSetCreationTimeUtc() to: [{0} {1}]", creationTime, creationTime.ToLongTimeString());
         Directory.SetCreationTimeUtc(path, creationTime);
         actual = Directory.GetCreationTimeUtc(path);
         System.IO.Directory.SetCreationTimeUtc(path, creationTime);
         expected = System.IO.Directory.GetCreationTimeUtc(path);
         Console.WriteLine("\t\tAlphaFS  : [{0}]\n\t\tSystem.IO: [{1}]", actual, expected);
         Assert.AreEqual(expected, actual, "AlphaFS != System.IO");
         Directory.SetCreationTimeUtc(symlinkPath, creationTime.AddDays(1), true, PathFormat.RelativePath);
         Assert.AreEqual(expected, Directory.GetCreationTimeUtc(path), "SetCreationTimeUtc modify-reparse-point should not have altered the underlying directory's timestamp");
         expected = System.IO.Directory.GetCreationTimeUtc(symlinkPath);
         Assert.AreEqual(expected, Directory.GetCreationTimeUtc(symlinkPath), "AlphaFS != System.IO");
         Assert.AreEqual(creationTime.AddDays(1), expected, "Time set != time read back");
         #endregion // SetCreationTime/Utc

         #region SetLastAccessTime/Utc
         DateTime lastAccessTime = new DateTime(rnd.Next(1971, 2071), rnd.Next(1, 12), rnd.Next(1, 28), rnd.Next(0, 23), rnd.Next(0, 59), rnd.Next(1, 59));
         Console.WriteLine("\n\tSetLastAccessTime() to: [{0} {1}]", lastAccessTime, lastAccessTime.ToLongTimeString());
         Directory.SetLastAccessTime(path, lastAccessTime);
         actual = Directory.GetLastAccessTime(path);
         System.IO.Directory.SetLastAccessTime(path, lastAccessTime);
         expected = System.IO.Directory.GetLastAccessTime(path);
         Console.WriteLine("\t\tAlphaFS  : [{0}]\n\t\tSystem.IO: [{1}]", actual, expected);
         Assert.AreEqual(expected, actual, "AlphaFS != System.IO");
         Directory.SetLastAccessTime(symlinkPath, lastAccessTime.AddDays(1), true, PathFormat.RelativePath);
         Assert.AreEqual(expected, Directory.GetLastAccessTime(path), "SetLastAccessTime modify-reparse-point should not have altered the underlying directory's timestamp");
         expected = System.IO.Directory.GetLastAccessTime(symlinkPath);
         Assert.AreEqual(expected, Directory.GetLastAccessTime(symlinkPath), "AlphaFS != System.IO");
         Assert.AreEqual(lastAccessTime.AddDays(1), expected, "Time set != time read back");


         lastAccessTime = new DateTime(rnd.Next(1971, 2071), rnd.Next(1, 12), rnd.Next(1, 28), rnd.Next(0, 23), rnd.Next(0, 59), rnd.Next(1, 59));
         Console.WriteLine("\n\tSetLastAccessTimeUtc() to: [{0} {1}]", lastAccessTime, lastAccessTime.ToLongTimeString());
         Directory.SetLastAccessTimeUtc(path, lastAccessTime);
         actual = Directory.GetLastAccessTimeUtc(path);
         System.IO.Directory.SetLastAccessTimeUtc(path, lastAccessTime);
         expected = System.IO.Directory.GetLastAccessTimeUtc(path);
         Console.WriteLine("\t\tAlphaFS  : [{0}]\n\t\tSystem.IO: [{1}]", actual, expected);
         Assert.AreEqual(expected, actual, "AlphaFS != System.IO");
         Directory.SetLastAccessTimeUtc(symlinkPath, lastAccessTime.AddDays(1), true, PathFormat.RelativePath);
         Assert.AreEqual(expected, Directory.GetLastAccessTimeUtc(path), "SetLastAccessTimeUtc modify-reparse-point should not have altered the underlying directory's timestamp");
         expected = System.IO.Directory.GetLastAccessTimeUtc(symlinkPath);
         Assert.AreEqual(expected, Directory.GetLastAccessTimeUtc(symlinkPath), "AlphaFS != System.IO");
         Assert.AreEqual(lastAccessTime.AddDays(1), expected, "Time set != time read back");
         #endregion // SetLastAccessTime/Utc

         #region SetLastWriteTime/Utc
         DateTime lastWriteTime = new DateTime(rnd.Next(1971, 2071), rnd.Next(1, 12), rnd.Next(1, 28), rnd.Next(0, 23), rnd.Next(0, 59), rnd.Next(1, 59));
         Console.WriteLine("\n\tSetLastWriteTime() to: [{0} {1}]", lastWriteTime, lastWriteTime.ToLongTimeString());
         Directory.SetLastWriteTime(path, lastWriteTime);
         actual = Directory.GetLastWriteTime(path);
         System.IO.Directory.SetLastWriteTime(path, lastWriteTime);
         expected = System.IO.Directory.GetLastWriteTime(path);
         Console.WriteLine("\t\tAlphaFS  : [{0}]\n\t\tSystem.IO: [{1}]", actual, expected);
         Assert.AreEqual(expected, actual, "AlphaFS != System.IO");
         Directory.SetLastWriteTime(symlinkPath, lastWriteTime.AddDays(1), true, PathFormat.RelativePath);
         Assert.AreEqual(expected, Directory.GetLastWriteTime(path), "SetLastWriteTime modify-reparse-point should not have altered the underlying directory's timestamp");
         expected = System.IO.Directory.GetLastWriteTime(symlinkPath);
         Assert.AreEqual(expected, Directory.GetLastWriteTime(symlinkPath), "AlphaFS != System.IO");
         Assert.AreEqual(lastWriteTime.AddDays(1), expected, "Time set != time read back");


         lastWriteTime = new DateTime(rnd.Next(1971, 2071), rnd.Next(1, 12), rnd.Next(1, 28), rnd.Next(0, 23), rnd.Next(0, 59), rnd.Next(1, 59));
         Console.WriteLine("\n\tSetLastWriteTimeUtc() to: [{0} {1}]", lastWriteTime, lastWriteTime.ToLongTimeString());
         Directory.SetLastWriteTimeUtc(path, lastWriteTime);
         actual = Directory.GetLastWriteTimeUtc(path);
         System.IO.Directory.SetLastWriteTimeUtc(path, lastWriteTime);
         expected = System.IO.Directory.GetLastWriteTimeUtc(path);
         Console.WriteLine("\t\tAlphaFS  : [{0}]\n\t\tSystem.IO: [{1}]", actual, expected);
         Assert.AreEqual(expected, actual, "AlphaFS != System.IO");
         Directory.SetLastWriteTimeUtc(symlinkPath, lastWriteTime.AddDays(1), true, PathFormat.RelativePath);
         Assert.AreEqual(expected, Directory.GetLastWriteTimeUtc(path), "SetLastWriteTimeUtc modify-reparse-point should not have altered the underlying directory's timestamp");
         expected = System.IO.Directory.GetLastWriteTimeUtc(symlinkPath);
         Assert.AreEqual(expected, Directory.GetLastWriteTimeUtc(symlinkPath), "AlphaFS != System.IO");
         Assert.AreEqual(lastWriteTime.AddDays(1), expected, "Time set != time read back");
         #endregion // SetLastWriteTime/Utc

         Directory.Delete(symlinkPath);
         Directory.Delete(path);
         Assert.IsTrue(!Directory.Exists(symlinkPath), "Cleanup failed: Symlink should have been removed.");
         Assert.IsTrue(!Directory.Exists(path));

         Console.WriteLine("\n");
      }

      private void DumpTransferTimestamps(bool isLocal)
      {
         Console.WriteLine("\n=== TEST {0} ===", isLocal ? UnitTestConstants.Local : UnitTestConstants.Network);
         string path = Path.Combine(Path.GetTempPath(), "Directory.TransferTimestamps()-" + Path.GetRandomFileName());
         string path2 = Path.Combine(Path.GetTempPath(), "Directory.TransferTimestamps()-" + Path.GetRandomFileName());

         if (!isLocal)
         {
            path = Path.LocalToUnc(path);
            path2 = Path.LocalToUnc(path2);
         }

         Console.WriteLine("\nInput Path1: [{0}]", path);
         Console.WriteLine("\nInput Path2: [{0}]", path2);

         Directory.CreateDirectory(path);
         Directory.CreateDirectory(path2);

         Thread.Sleep(new Random().Next(250, 500));
         int seed = (int)DateTime.Now.Ticks & 0x0000FFFF;
         DateTime creationTime = new DateTime(new Random(seed).Next(1971, 2071), new Random(seed).Next(1, 12), new Random(seed).Next(1, 28), new Random(seed).Next(0, 23), new Random(seed).Next(0, 59), new Random(seed).Next(0, 59));

         Thread.Sleep(new Random().Next(250, 500));
         seed += (int)DateTime.Now.Ticks & 0x0000FFFF;
         DateTime lastAccessTime = new DateTime(new Random(seed).Next(1971, 2071), new Random(seed).Next(1, 12), new Random(seed).Next(1, 28), new Random(seed).Next(0, 23), new Random(seed).Next(0, 59), new Random(seed).Next(0, 59));

         Thread.Sleep(new Random().Next(250, 500));
         seed += (int)DateTime.Now.Ticks & 0x0000FFFF;
         DateTime lastWriteTime = new DateTime(new Random(seed).Next(1971, 2071), new Random(seed).Next(1, 12), new Random(seed).Next(1, 28), new Random(seed).Next(0, 23), new Random(seed).Next(0, 59), new Random(seed).Next(0, 59));

         Directory.SetTimestamps(path, creationTime, lastAccessTime, lastWriteTime);

         Console.WriteLine("\n\tPath1 dates and times:");
         Console.WriteLine("\t\tCreationTime  : [{0} {1}]", creationTime, creationTime.ToLongTimeString());
         Console.WriteLine("\t\tLastAccessTime: [{0} {1}]", lastAccessTime, lastAccessTime.ToLongTimeString());
         Console.WriteLine("\t\tLastWriteTime : [{0} {1}]", lastWriteTime, lastWriteTime.ToLongTimeString());

         Console.WriteLine("\n\tPath2 current dates and times:");
         Console.WriteLine("\t\tCreationTime  : [{0} {1}]", Directory.GetCreationTime(path2), Directory.GetCreationTime(path2).ToLongTimeString());
         Console.WriteLine("\t\tLastAccessTime: [{0} {1}]", Directory.GetLastAccessTime(path2), Directory.GetLastAccessTime(path2).ToLongTimeString());
         Console.WriteLine("\t\tLastWriteTime : [{0} {1}]", Directory.GetLastWriteTime(path2), Directory.GetLastWriteTime(path2).ToLongTimeString());

         Directory.TransferTimestamps(path, path2);

         Console.WriteLine("\n\tPath2 dates and times after TransferTimestamps():");
         Console.WriteLine("\t\tCreationTime  : [{0} {1}]", Directory.GetCreationTime(path2), Directory.GetCreationTime(path2).ToLongTimeString());
         Console.WriteLine("\t\tLastAccessTime: [{0} {1}]", Directory.GetLastAccessTime(path2), Directory.GetLastAccessTime(path2).ToLongTimeString());
         Console.WriteLine("\t\tLastWriteTime : [{0} {1}]", Directory.GetLastWriteTime(path2), Directory.GetLastWriteTime(path2).ToLongTimeString());

         Assert.AreEqual(Directory.GetCreationTime(path), Directory.GetCreationTime(path2));
         Assert.AreEqual(Directory.GetLastAccessTime(path), Directory.GetLastAccessTime(path2));
         Assert.AreEqual(Directory.GetLastWriteTime(path), Directory.GetLastWriteTime(path2));

         Directory.Delete(path);
         Assert.IsTrue(!Directory.Exists(path));
         Directory.Delete(path2);
         Assert.IsTrue(!Directory.Exists(path2));

         Console.WriteLine("\n");
      }

      private bool HasInheritedPermissions(string path)
      {
         DirectorySecurity acl = Directory.GetAccessControl(path);
         return acl.GetAccessRules(false, true, typeof(SecurityIdentifier)).Count > 0;
      }

      private void DumpDirectoryTrailingDotSpace(bool isLocal)
      {
         #region Setup

         Console.WriteLine("\n=== TEST {0} ===\n", isLocal ? UnitTestConstants.Local : UnitTestConstants.Network);
         const string characterDot = ".";
         const string characterSpace = " ";
         string random = Path.GetRandomFileName();
         string tempPathDot = Path.GetTempPath("Directory.CreateDirectory()-" + random + "-directory-with-dot-" + characterDot);
         string tempPathSpace = Path.GetTempPath("Directory.CreateDirectory()-" + random + "-directory-with-space-" + characterSpace);
         if (!isLocal) tempPathDot = Path.LocalToUnc(tempPathDot);
         if (!isLocal) tempPathSpace = Path.LocalToUnc(tempPathSpace);

         Console.WriteLine("Input Directory Path (with dot)  : [{0}]", tempPathDot);
         Console.WriteLine("Input Directory Path (with space): [{0}]", tempPathSpace);

         Assert.IsTrue(tempPathDot.EndsWith(characterDot), "Path should have a trailing dot.");
         Assert.IsTrue(tempPathSpace.EndsWith(characterSpace), "Path should have a trailing space.");

         #endregion // Setup

         try
         {
            #region Path.GetFullPath(), Path Normalization

            string sysIo = System.IO.Path.GetFullPath(tempPathDot);
            Assert.IsFalse(sysIo.EndsWith(characterDot), "Path should not have a trailing dot.");

            sysIo = System.IO.Path.GetFullPath(tempPathSpace);
            Assert.IsFalse(sysIo.EndsWith(characterSpace), "Path should not have a trailing space.");


            string alphaFs = Path.GetFullPath(tempPathDot);
            Assert.IsFalse(alphaFs.EndsWith(characterDot), "Path should not have a trailing dot.");

            alphaFs = Path.GetFullPath(tempPathSpace);
            Assert.IsFalse(alphaFs.EndsWith(characterSpace), "Path should not have a trailing space.");


            Assert.AreEqual(sysIo, alphaFs, "Paths should be the same.");

            #endregion // Path.GetFullPath(), Path Normalization

            #region Path.GetLongPath(), No Path Normalization

            alphaFs = Path.GetLongPath(tempPathDot);
            Assert.IsTrue(alphaFs.EndsWith(characterDot), "Path should have a trailing dot.");

            alphaFs = Path.GetLongPath(tempPathSpace);
            Assert.IsTrue(alphaFs.EndsWith(characterSpace), "Path should have a trailing space.");

            Assert.AreNotEqual(alphaFs, sysIo, "Paths should not be the same.");

            #endregion // Path.GetLongPath(), No Path Normalization

            #region Path.GetRegularPath(), No Path Normalization

            alphaFs = Path.GetRegularPath(tempPathDot);
            Assert.IsTrue(alphaFs.EndsWith(characterDot), "Path should have a trailing dot.");

            alphaFs = Path.GetRegularPath(tempPathSpace);
            Assert.IsTrue(alphaFs.EndsWith(characterSpace), "Path should have a trailing space.");

            Assert.AreNotEqual(alphaFs, sysIo, "Paths should not be the same.");

            #endregion // Path.GetRegularPath(), No Path Normalization

            Console.WriteLine();

            #region Directory() Class

            UnitTestConstants.StopWatcher(true);

            #region TrailingDot

            #region System.IO

            // tempPathDot contains a trailing dot but gets stripped on path normalization.
            // System.IO handles the directory without the trailing dot. Therefore, the directory exists.
            // AlphaFS has the same behaviour as .NET for default methods.

            System.IO.DirectoryInfo sysIoDi = System.IO.Directory.CreateDirectory(tempPathDot);
            Assert.IsFalse(sysIoDi.Name.EndsWith(characterDot), "Path should not have a trailing dot.");
            Assert.IsTrue(System.IO.Directory.Exists(tempPathDot), "Directory should exist.");
            Assert.IsTrue(Directory.Exists(tempPathDot), "Directory should exist.");

            System.IO.Directory.Delete(tempPathDot);
            Assert.IsFalse(System.IO.Directory.Exists(tempPathDot), "Directory should not exist.");

            #endregion // System.IO

            #region AlphaFS

            DirectoryInfo alphaFsDi = Directory.CreateDirectory(tempPathDot, false, PathFormat.FullPath);
            Assert.IsTrue(alphaFsDi.Name.EndsWith(characterDot), "Path should have a trailing dot.");
            Assert.IsFalse(Directory.Exists(tempPathDot), "Directory should not exist.");
            Assert.IsTrue(Directory.Exists(tempPathDot, PathFormat.FullPath), "Directory should exist.");

            DirectoryInfo alphaFsDi2 = alphaFsDi.CreateSubdirectory("Directory-with-dot-" + characterDot, false);
            Assert.IsTrue(alphaFsDi2.Exists, "Directory should exist.");

            alphaFsDi2 = alphaFsDi.CreateSubdirectory("Directory-with-space-" + characterSpace, false);
            Assert.IsTrue(alphaFsDi2.Exists, "Directory should exist.");

            Directory.Delete(tempPathDot, true, true, PathFormat.FullPath);
            Assert.IsFalse(Directory.Exists(tempPathDot, PathFormat.FullPath), "Directory should not exist.");

            #endregion // AlphaFS

            #endregion // TrailingDot

            #region TrailingSpace

            #region System.IO

            // tempPathSpace contains a trailing space but gets stripped on path normalization.
            // System.IO handles the file without the trailing space. Therefore, the file exists.
            // AlphaFS has the same behaviour as .NET for default methods.

            sysIoDi = System.IO.Directory.CreateDirectory(tempPathSpace);
            Assert.IsFalse(sysIoDi.Name.EndsWith(characterSpace), "Path should not have a trailing dot.");
            Assert.IsTrue(System.IO.Directory.Exists(tempPathSpace), "Directory should exist.");
            Assert.IsTrue(Directory.Exists(tempPathSpace), "Directory should exist.");

            System.IO.Directory.Delete(tempPathSpace);
            Assert.IsFalse(System.IO.Directory.Exists(tempPathSpace), "Directory should not exist.");

            #endregion // System.IO

            #region AlphaFS

            alphaFsDi = Directory.CreateDirectory(tempPathSpace, false, PathFormat.FullPath);
            Assert.IsTrue(alphaFsDi.Name.EndsWith(characterSpace), "Path should have a trailing space.");
            Assert.IsFalse(Directory.Exists(tempPathSpace), "Directory should exist.");  // Because trailing space is removed.
            Assert.IsTrue(Directory.Exists(tempPathSpace, PathFormat.FullPath), "Directory should exist.");

            alphaFsDi2 = alphaFsDi.CreateSubdirectory("Directory-with-space-" + characterSpace, false);
            Assert.IsTrue(alphaFsDi2.Exists, "Directory should exist.");

            alphaFsDi2 = alphaFsDi.CreateSubdirectory("Directory-with-dot-" + characterDot, false);
            Assert.IsTrue(alphaFsDi2.Exists, "Directory should exist.");

            Directory.Delete(tempPathSpace, true, true, PathFormat.FullPath);
            Assert.IsFalse(Directory.Exists(tempPathSpace, PathFormat.FullPath), "Directory should not exist.");

            #endregion // AlphaFS

            #endregion // TrailingSpace

            Console.WriteLine("\tClass DirectoryInfo()\t{0}", UnitTestConstants.Reporter());

            #endregion // Directory() Class
         }
         finally
         {
            if (Directory.Exists(tempPathDot, PathFormat.FullPath))
               Directory.Delete(tempPathDot, true, true);

            if (Directory.Exists(tempPathSpace, PathFormat.FullPath))
               Directory.Delete(tempPathSpace, true, true);

            Assert.IsFalse(Directory.Exists(tempPathDot), "Cleanup failed: Directory should have been removed.");
            Assert.IsFalse(Directory.Exists(tempPathSpace), "Cleanup failed: Directory should have been removed.");
         }

         Console.WriteLine();
      }


      #region .NET

      [TestMethod]
      public void EnumerateDirectories()
      {
         Console.WriteLine("Directory.EnumerateDirectories()");

         DumpEnumerateDirectories(true);
         DumpEnumerateDirectories(false);
      }

      [TestMethod]
      public void EnumerateFiles()
      {
         Console.WriteLine("Directory.EnumerateFiles()");

         DumpEnumerateFiles(true);
         DumpEnumerateFiles(false);
      }

      [TestMethod]
      public void EnumerateFileSystemEntries()
      {
         Console.WriteLine("Directory.EnumerateFileSystemEntries()");

         DumpEnumerateFileSystemEntries(true);
         DumpEnumerateFileSystemEntries(false);
      }

      [TestMethod]
      public void NET_GetCurrentDirectory()
      {
         Console.WriteLine("Directory.GetCurrentDirectory()");
         Console.WriteLine("\nThe .NET method is used.");
      }
      
      [TestMethod]
      public void GetDirectoryRoot()
      {
         Console.WriteLine("Directory.GetDirectoryRoot()");

         int pathCnt = 0;
         int errorCnt = 0;

         #region ArgumentException

         string expectedException = "System.ArgumentException";
         bool exception = false;
         try
         {
            Console.WriteLine("\nCatch: [{0}]", expectedException);
            Directory.GetDirectoryRoot(@"\\\\.txt");
         }
         catch (ArgumentException ex)
         {
            exception = true;
            Console.WriteLine("\n\t[{0}]: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
         }
         catch (Exception ex)
         {
            Console.WriteLine("\n\tCaught (unexpected) {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
         }
         Assert.IsTrue(exception, "[{0}] should have been caught.", expectedException);
         Console.WriteLine();

         #endregion // ArgumentException

         UnitTestConstants.StopWatcher(true);
         foreach (string path in UnitTestConstants.InputPaths)
         {
            string expected = null;
            string actual = null;
            bool skipAssert = false;

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
      public void GetFileSystemEntries()
      {
         Console.WriteLine("Directory.GetFileSystemEntries()");

         DumpGetFileSystemEntries(true);
         DumpGetFileSystemEntries(false);
      }

      [TestMethod]
      public void GetFileSystemEntries_LongPathWithPrefix_ShouldReturnCorrectEntries()
      {
         using (var tempDir = new TemporaryDirectory("GetFileSystemEntries"))
         {
            string longDir = Path.Combine(tempDir.Directory.FullName, new string('x', 128), new string('x', 128), new string('x', 128), new string('x', 128));
            Directory.CreateDirectory(longDir);
            Directory.CreateDirectory(Path.Combine(longDir, "A"));
            Directory.CreateDirectory(Path.Combine(longDir, "B"));
            File.WriteAllText(Path.Combine(longDir, "C"), "C");

            var entries = Directory.GetFileSystemEntries("\\\\?\\" + longDir).ToArray();

            Assert.AreEqual(3, entries.Length);
         }
      }

      [TestMethod]
      public void GetFileSystemEntries_LongPathWithoutPrefix_ShouldReturnCorrectEntries()
      {
         using (var tempDir = new TemporaryDirectory("GetFileSystemEntries"))
         {
            string longDir = Path.Combine(tempDir.Directory.FullName, new string('x', 128), new string('x', 128), new string('x', 128), new string('x', 128));
            Directory.CreateDirectory(longDir);
            Directory.CreateDirectory(Path.Combine(longDir, "A"));
            Directory.CreateDirectory(Path.Combine(longDir, "B"));
            File.WriteAllText(Path.Combine(longDir, "C"), "C");

            var entries = Directory.GetFileSystemEntries(longDir).ToArray();

            Assert.AreEqual(3, entries.Length);
         }
      }

      [TestMethod]
      public void GetLogicalDrives()
      {
         Console.WriteLine("Directory.GetLogicalDrives()");

         DumpGetDrives(false);
      }

      [TestMethod]
      public void GetParent()
      {
         Console.WriteLine("Directory.GetParent()");

         int pathCnt = 0;
         int errorCnt = 0;

         UnitTestConstants.StopWatcher(true);
         foreach (string path in UnitTestConstants.InputPaths)
         {
            string expected = null;
            string actual = null;
            bool skipAssert = false;

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

      [TestMethod]
      public void Move()
      {
         Console.WriteLine("Directory.Move()");

         DumpMove(true);
         DumpMove(false);
      }

      [TestMethod]
      public void SetCreationTime()
      {
         Console.WriteLine("Directory.SetXxxTime()");

         DumpSetXxxTime(true);
         DumpSetXxxTime(false);
      }

      [TestMethod]
      public void NET_SetCurrentDirectory()
      {
         Console.WriteLine("Directory.SetCurrentDirectory()");
         Console.WriteLine("\nThe .NET method is used.");
      }

      #endregion // .NET

      #region AlphaFS

      [TestMethod]
      public void AlphaFS_Copy()
      {
         Console.WriteLine("Directory.Copy()");

         DumpCopy(true);
         DumpCopy(false);
      }

      [TestMethod]
      public void AlphaFS_CountFileSystemObjects()
      {
         Console.WriteLine("Directory.CountFileSystemObjects()");

         long fsoCount = 0;

         #region Count Directories

         Console.WriteLine("\nCount Directories");

         string path = UnitTestConstants.SysRoot;

         string searchPattern = Path.WildcardStarMatchAll;

         Console.WriteLine("\n\tsearchPattern: \"{0}\", abort on error.", searchPattern);

         #region Exception

         bool gotException = false;
         try
         {
            fsoCount = Directory.CountFileSystemObjects(path, searchPattern, DirectoryEnumerationOptions.Folders | DirectoryEnumerationOptions.Recursive);
         }
         catch (Exception ex)
         {
            gotException = true;
            Console.WriteLine("\n\tCaught (unexpected) {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
         }
         Console.WriteLine("\n\tCaught Exception (Should be True): [{0}]", gotException);
         Assert.IsTrue(gotException);
         Console.WriteLine("\n\tdirectory\t = [{0}]\n\tSubdirectories = [{1}]\n{2}\n", path, fsoCount, UnitTestConstants.Reporter());

         #endregion // Exception

         searchPattern = Path.WildcardStarMatchAll;

         Console.WriteLine("\n\tsearchPattern: \"{0}\", continue on error. Running as Administrator will count more directories.", searchPattern);

         UnitTestConstants.StopWatcher(true);
         fsoCount = Directory.CountFileSystemObjects(path, searchPattern, DirectoryEnumerationOptions.Folders | DirectoryEnumerationOptions.Recursive | DirectoryEnumerationOptions.ContinueOnException);
         Console.WriteLine("\n\tdirectory\t = [{0}]\n\tSubdirectories = [{1}]\n{2}\n", path, fsoCount, UnitTestConstants.Reporter());
         Assert.IsTrue(fsoCount > 0);


         Console.WriteLine("\n\tsearchPattern: \"{0}\", continue on error. Running as Administrator and using PrivilegeEnabler(Privilege.Backup) will count even more directories.", searchPattern);
         UnitTestConstants.StopWatcher(true);
         using (new PrivilegeEnabler(Privilege.Backup))
         {
            fsoCount = Directory.CountFileSystemObjects(path, searchPattern, DirectoryEnumerationOptions.Folders | DirectoryEnumerationOptions.Recursive | DirectoryEnumerationOptions.ContinueOnException);
            Console.WriteLine("\n\tDirectory\t = [{0}]\n\tSubdirectories = [{1}]\n{2}\n", path, fsoCount, UnitTestConstants.Reporter());
            Assert.IsTrue(fsoCount > 0);
         }
         Console.WriteLine();

         #endregion // Count Directories

         #region Count Files

         Console.WriteLine("Count Files");

         searchPattern = Path.WildcardStarMatchAll;

         path = UnitTestConstants.SysRoot32;
         fsoCount = 0;

         #region Exception

         gotException = false;
         try
         {
            fsoCount = Directory.CountFileSystemObjects(path, searchPattern, DirectoryEnumerationOptions.Files | DirectoryEnumerationOptions.Recursive);
         }
         catch (Exception ex)
         {
            gotException = true;
            Console.WriteLine("\n\tCaught (unexpected) {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
         }
         Console.WriteLine("\n\tCaught Exception (Should be True): [{0}]", gotException);
         Assert.IsTrue(gotException);
         Console.WriteLine("\n\tDirectory = [{0}]\n\tFiles  = [{1}]\n{2}\n", path, fsoCount, UnitTestConstants.Reporter());

         #endregion // Exception

         UnitTestConstants.StopWatcher(true);
         fsoCount = Directory.CountFileSystemObjects(path, searchPattern, DirectoryEnumerationOptions.Files | DirectoryEnumerationOptions.Recursive | DirectoryEnumerationOptions.ContinueOnException);
         Console.WriteLine("\n\tDirectory = [{0}]\n\tFiles  = [{1}]\n{2}\n", path, fsoCount, UnitTestConstants.Reporter());
         Assert.IsTrue(fsoCount > 0);

         Console.WriteLine("\n\tContinue on error. Running as Administrator will count more files.");
         UnitTestConstants.StopWatcher(true);
         fsoCount = Directory.CountFileSystemObjects(path, searchPattern, DirectoryEnumerationOptions.Files | DirectoryEnumerationOptions.Recursive | DirectoryEnumerationOptions.ContinueOnException);
         Console.WriteLine("\n\tDirectory = [{0}]\n\tFiles  = [{1}]\n{2}\n", path, fsoCount, UnitTestConstants.Reporter());
         Assert.IsTrue(fsoCount > 0);

         Console.WriteLine("\n\tContinue on error. Running as Administrator and using PrivilegeEnabler(Privilege.Backup) will count even more files.");
         UnitTestConstants.StopWatcher(true);
         using (new PrivilegeEnabler(Privilege.Backup))
         {
            fsoCount = Directory.CountFileSystemObjects(path, searchPattern, DirectoryEnumerationOptions.Files | DirectoryEnumerationOptions.Recursive | DirectoryEnumerationOptions.ContinueOnException);
            Console.WriteLine("\n\tDirectory = [{0}]\n\tFiles  = [{1}]\n{2}\n", path, fsoCount, UnitTestConstants.Reporter());
            Assert.IsTrue(fsoCount > 0);
         }
         Console.WriteLine();

         #endregion // Count Files
      }

      [TestMethod]
      public void AlphaFS_DeleteEmptySubdirectories()
      {
         Console.WriteLine("Directory.DeleteEmptySubdirectories()");

         string tempPath = Path.Combine(Path.GetTempPath(), "Directory.DeleteEmptySubdirectories()-" + Path.GetRandomFileName());
         long dirs0, dirs1, files0, files1;

         const int maxDepth = 10;
         const int totalDirectories = (maxDepth * maxDepth) + maxDepth;  // maxDepth = 10: 110 directories and 110 files.
         const int emptyDirectories = (maxDepth * maxDepth) / 2;         // 50 empty directories.
         const int remainingDirectories = totalDirectories - emptyDirectories;   // 60 remaining directories.

         Console.WriteLine("\nInput Path: [{0}]", tempPath);
         UnitTestConstants.CreateDirectoriesAndFiles(tempPath, maxDepth, true);

         string searchPattern = Path.WildcardStarMatchAll;

         UnitTestConstants.StopWatcher(true);
         dirs0 = Directory.CountFileSystemObjects(tempPath, searchPattern, DirectoryEnumerationOptions.Folders | DirectoryEnumerationOptions.Recursive | DirectoryEnumerationOptions.ContinueOnException);
         files0 = Directory.CountFileSystemObjects(tempPath, searchPattern, DirectoryEnumerationOptions.Files | DirectoryEnumerationOptions.Recursive | DirectoryEnumerationOptions.ContinueOnException);
         Console.WriteLine("\nCounted Directories: [{0}]\nCounted Files      : [{1}]\n{2}", dirs0, files0, UnitTestConstants.Reporter());

         UnitTestConstants.StopWatcher(true);
         bool deleteOk = false;
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
         bool directoryNotExists = !Directory.Exists(tempPath);
         Assert.IsTrue(directoryNotExists);

         Assert.IsTrue((emptyDirectories + remainingDirectories) == totalDirectories);
      }

      [TestMethod]
      public void AlphaFS_Encrypt()
      {
         Console.WriteLine("Directory.Encrypt()");

         DumpEncryptDecrypt(true);
         DumpEncryptDecrypt(false);
      }

      [TestMethod]
      public void AlphaFS_EnableEncryption()
      {
         Console.WriteLine("Directory.EnableEncryption()");

         DumpEnableDisableEncryption(true);
         DumpEnableDisableEncryption(false);
      }

      [TestMethod]
      public void AlphaFS_EnumerateFileIdBothDirectoryInfo()
      {
         Console.WriteLine("Directory.EnumerateFileIdBothDirectoryInfo()");

         DumpEnumerateFileIdBothDirectoryInfo(true);
         DumpEnumerateFileIdBothDirectoryInfo(false);
      }

      [TestMethod]
      public void AlphaFS_GetProperties()
      {
         Console.WriteLine("Directory.GetProperties()");

         DumpGetProperties(true);
         DumpGetProperties(false);
      }

      [TestMethod]
      public void AlphaFS_HasInheritedPermissions()
      {
         Console.WriteLine("Directory.HasInheritedPermissions()\n");

         string searchPattern = Path.WildcardStarMatchAll;
         SearchOption searchOption = SearchOption.TopDirectoryOnly;

         int cnt = 0;
         UnitTestConstants.StopWatcher(true);
         foreach (string dir in Directory.EnumerateDirectories(UnitTestConstants.SysRoot, searchPattern, searchOption))
         {
            try
            {
               bool hasIp = Directory.HasInheritedPermissions(dir);

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

      [TestMethod]
      public void AlphaFS_SetTimestamps()
      {
         Console.WriteLine("Directory.SetTimestamps()");

         DumpSetTimestamps(true);
         DumpSetTimestamps(false);
      }

      [TestMethod]
      public void AlphaFS_TransferTimestamps()
      {
         Console.WriteLine("Directory.TransferTimestamps()");

         DumpTransferTimestamps(true);
         DumpTransferTimestamps(false);
      }

      [TestMethod]
      public void AlphaFS___DirectoryTrailingDotSpace()
      {
         Console.WriteLine(".NET does not support the creation/manipulation of directory with a trailing dot or space.");
         Console.WriteLine("These will be stripped due to path normalization.");

         Console.WriteLine("\nThe AlphaFS Directory() class contains overloaded methods which have the");
         Console.WriteLine("isFullPath parameter that enables you to bypass this .NET limitation.\n");

         DumpDirectoryTrailingDotSpace(true);
         DumpDirectoryTrailingDotSpace(false);
      }

      #endregion // AlphaFS
   }
}
