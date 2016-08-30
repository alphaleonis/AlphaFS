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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Threading;
using Directory = Alphaleonis.Win32.Filesystem.Directory;
using DirectoryInfo = Alphaleonis.Win32.Filesystem.DirectoryInfo;
using DriveInfo = Alphaleonis.Win32.Filesystem.DriveInfo;
using File = Alphaleonis.Win32.Filesystem.File;
using FileInfo = Alphaleonis.Win32.Filesystem.FileInfo;
using Path = Alphaleonis.Win32.Filesystem.Path;

namespace AlphaFS.UnitTest
{
   /// <summary>This is a test class for File and is intended to contain all File class Unit Tests.</summary>
   [TestClass]
   public partial class FileTest
   {
      #region Unit Tests
      
      #region DumpAppendAllLines

      private void DumpAppendAllLines(bool isLocal)
      {
         #region Setup

         Console.WriteLine("\n=== TEST {0} ===", isLocal ? UnitTestConstants.Local : UnitTestConstants.Network);
         string tempFolder = Path.GetTempPath();
         string tempPath = Path.Combine(tempFolder, "File.Delete-" + Path.GetRandomFileName());
         if (!isLocal) tempPath = Path.LocalToUnc(tempPath);

         // Create file and append text.
         string tempFile = Path.GetTempFileName();
         if (!isLocal) tempFile = Path.LocalToUnc(tempFile);

         IEnumerable<string> allLines = new[] { UnitTestConstants.TenNumbers, UnitTestConstants.TextHelloWorld, UnitTestConstants.TextAppend, UnitTestConstants.TextUnicode };

         #endregion // Setup

         try
         {
            #region AppendAllLines

            Console.WriteLine("\nDefault AlphaFS Encoding: [{0}]", NativeMethods.DefaultFileEncoding.EncodingName);

            // Create real UTF-8 file.
            File.AppendAllLines(tempFile, allLines, NativeMethods.DefaultFileEncoding);

            // Read filestream contents.
            using (StreamReader streamRead = File.OpenText(tempFile))
            {
               string line = streamRead.ReadToEnd();

               Console.WriteLine("\nCreated: [{0}] filestream: [{1}]\n\n\tAppendAllLines content:\n{2}", streamRead.CurrentEncoding.EncodingName, tempFile, line);

               foreach (string line2 in allLines)
                  Assert.IsTrue(line.Contains(line2));
            }

            // Append
            File.AppendAllLines(tempFile, new[] { "Append 1" });
            File.AppendAllLines(tempFile, allLines);
            File.AppendAllLines(tempFile, new[] { "Append 2" });
            File.AppendAllLines(tempFile, allLines);

            // Read filestream contents.
            using (StreamReader streamRead = File.OpenText(tempFile))
            {
               string line = streamRead.ReadToEnd();

               Console.WriteLine("AppendAllLines content:\n{0}", line);

               foreach (string line2 in allLines)
                  Assert.IsTrue(line.Contains(line2));
            }

            #endregion // AppendAllLines
         }
         finally
         {
            File.Delete(tempFile, true);
            Assert.IsFalse(File.Exists(tempFile), "Cleanup failed: File should have been removed.");
         }

         Console.WriteLine();
      }

      #endregion // DumpAppendAllLines

      #region DumpCopy

      private void DumpCopy(bool isLocal)
      {
         #region Setup

         Console.WriteLine("\n=== TEST {0} ===", isLocal ? UnitTestConstants.Local : UnitTestConstants.Network);
         string path = isLocal ? UnitTestConstants.SysRoot : Path.LocalToUnc(UnitTestConstants.SysRoot);
         string tempPath = Path.GetTempPath("File-Copy-" + Path.GetRandomFileName());
         if (!isLocal) tempPath = Path.LocalToUnc(tempPath);

         bool exception;
         int expectedLastError;
         string expectedException;

         string fileSource = @"file-source-" + Path.GetRandomFileName() + ".exe";
         string fileDestination = @"file-destination-" + Path.GetRandomFileName() + ".exe";

         string folderSource = tempPath + @"\folder-source-" + Path.GetRandomFileName();
         string folderDestination = tempPath + @"\folder-destination-" + Path.GetRandomFileName();

         string fullPathSource = folderSource + @"\" + fileSource;
         string fullPathDestination = folderDestination + @"\" + fileDestination;
         if (!isLocal) fullPathSource = Path.LocalToUnc(fullPathSource);
         if (!isLocal) fullPathDestination = Path.LocalToUnc(fullPathDestination);

         DirectoryInfo dirInfo = new DirectoryInfo(folderDestination);

         #endregion // Setup

         try
         {
            #region UnauthorizedAccessException

            Directory.CreateDirectory(folderSource);
            Directory.CreateDirectory(folderDestination);

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

               FileInfo fileInfo = new FileInfo(fullPathSource);
               using (StreamWriter sw = fileInfo.CreateText())
                  sw.WriteLine("MoveTo-TestFile");

               // Set DENY for current user.
               dirSecurity = dirInfo.GetAccessControl();
               dirSecurity.AddAccessRule(rule);
               dirInfo.SetAccessControl(dirSecurity);

               fileInfo.CopyTo(fullPathDestination);
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
               dirSecurity = dirInfo.GetAccessControl();
               dirSecurity.RemoveAccessRule(rule);
               dirInfo.SetAccessControl(dirSecurity, AccessControlSections.Access);

               Directory.Delete(tempPath, true, true);
            }
            Assert.IsTrue(exception, "[{0}] should have been caught.", expectedException);
            Console.WriteLine();

            #endregion // UnauthorizedAccessException

            #region FileNotFoundException

            expectedLastError = (int)Win32Errors.ERROR_FILE_NOT_FOUND;
            expectedException = "System.IO.FileNotFoundException";
            exception = false;
            try
            {
               Console.WriteLine("\nCatch: [{0}]", expectedException);
               File.Copy(isLocal ? fileSource : Path.LocalToUnc(fileSource), isLocal ? fileDestination : Path.LocalToUnc(fileDestination));
            }
            catch (FileNotFoundException ex)
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

            #endregion // FileNotFoundException

            #region DirectoryNotFoundException

            expectedLastError = (int)Win32Errors.ERROR_PATH_NOT_FOUND;
            expectedException = "System.IO.DirectoryNotFoundException";
            exception = false;
            try
            {
               Console.WriteLine("\nCatch: [{0}]", expectedException);
               File.Copy(fullPathSource, fullPathDestination);
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

            #region IOException #1 (AlreadyExistsException)

            Directory.CreateDirectory(folderSource);
            Directory.CreateDirectory(folderDestination);
            using (File.Create(fullPathSource)) { }
            using (File.Create(fullPathDestination)) { }

            expectedLastError = (int)Win32Errors.ERROR_FILE_EXISTS;
            expectedException = "System.IO.IOException";
            exception = false;
            try
            {
               Console.WriteLine("\nCatch: [{0}]", expectedException);
               File.Copy(fullPathSource, fullPathDestination);
            }
            catch (AlreadyExistsException ex)
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

            Directory.Delete(folderSource, true);
            Directory.Delete(folderDestination, true);

            #endregion // IOException #1 (AlreadyExistsException)

            #region IOException #2

            string folderfileName = null;

            expectedLastError = (int)Win32Errors.ERROR_ACCESS_DENIED;
            expectedException = "System.IO.IOException";
            exception = false;
            try
            {
               Console.WriteLine("\nCatch: [{0}]", expectedException);

               foreach (string file in Directory.EnumerateFiles(path))
               {
                  string newFile = Path.Combine(tempPath, Path.GetFileName(file, true));
                  folderfileName = newFile;

                  // Trigger the Exception.
                  Directory.CreateDirectory(folderfileName);

                  // true: overwrite existing.
                  File.Copy(file, folderfileName, true);
               }
            }
            catch (AlreadyExistsException ex)
            {
               var win32Error = new Win32Exception("", ex);
               Assert.IsTrue(win32Error.NativeErrorCode == expectedLastError, string.Format("Expected Win32Exception error should be: [{0}], got: [{1}]", expectedLastError, win32Error.NativeErrorCode));

               exception = true;
               Directory.Delete(folderfileName);
               Console.WriteLine("\n\t[{0}]: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
            }
            catch (Exception ex)
            {
               Console.WriteLine("\n\tCaught (unexpected) {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
            }
            Assert.IsTrue(exception, "[{0}] should have been caught.", expectedException);
            Console.WriteLine();

            #endregion // IOException #2

            #region Copy

            Console.WriteLine("\nInput Directory Path: [{0}]\n", path);
            int cnt = 0;
            string readOnlySource = null;
            string readOnlyDestination = null;

            UnitTestConstants.StopWatcher(true);
            foreach (string file in Directory.EnumerateFiles(path))
            {
               string newFile = Path.Combine(tempPath, Path.GetFileName(file, true));
               File.Copy(file, newFile);

               // Set the first file as read-only to trigger an Exception when copying again.
               if (cnt == 0)
               {
                  File.SetAttributes(newFile, FileAttributes.ReadOnly);
                  readOnlySource = file;
                  readOnlyDestination = newFile;
               }

               Console.WriteLine("\t#{0:000}\tCopied to: [{1}]", ++cnt, newFile);
               Assert.IsTrue(File.Exists(newFile));
            }
            Console.WriteLine("\n\tTotal Size: [{0}]{1}", Utils.UnitSizeToText(Directory.GetProperties(tempPath)["Size"]), UnitTestConstants.Reporter());
            Console.WriteLine();

            #endregion // Copy

            #region Preserve Timestamps

            // Test preservation of timestamps.
            int seed = (int)DateTime.Now.Ticks & 0x0000FFFF;
            DateTime creationTime = new DateTime(new Random(seed).Next(1971, 2071), new Random(seed).Next(1, 12), new Random(seed).Next(1, 28), new Random(seed).Next(0, 23), new Random(seed).Next(0, 59), new Random(seed).Next(0, 59));
            seed += (int)DateTime.Now.Ticks & 0x0000FFFF;
            DateTime lastAccessTime = new DateTime(new Random(seed).Next(1971, 2071), new Random(seed).Next(1, 12), new Random(seed).Next(1, 28), new Random(seed).Next(0, 23), new Random(seed).Next(0, 59), new Random(seed).Next(0, 59));
            seed += (int)DateTime.Now.Ticks & 0x0000FFFF;
            DateTime lastWriteTime = new DateTime(new Random(seed).Next(1971, 2071), new Random(seed).Next(1, 12), new Random(seed).Next(1, 28), new Random(seed).Next(0, 23), new Random(seed).Next(0, 59), new Random(seed).Next(0, 59));

            string preservePath = Path.Combine(tempPath, "PreserveTimestamps");
            string preserveFile = Path.GetFileName(readOnlySource);
            string preserveReadOnlySource = Path.Combine(preservePath, preserveFile);

            Directory.CreateDirectory(preservePath);
            File.Copy(readOnlySource, preserveReadOnlySource);

            File.SetCreationTime(preserveReadOnlySource, creationTime);
            File.SetLastAccessTime(preserveReadOnlySource, lastAccessTime);
            File.SetLastWriteTime(preserveReadOnlySource, lastWriteTime);
            
            UnitTestConstants.StopWatcher(true);

            // 3rd parameter CopyOptions.None: overwrite existing.
            // 4rd parameter true: preserve timestamps of source.
            File.Copy(preserveReadOnlySource, readOnlyDestination, CopyOptions.None, true);

            Console.WriteLine("\tFile copied.{0}", UnitTestConstants.Reporter());

            Assert.IsTrue(File.Exists(preserveReadOnlySource));
            Assert.IsTrue(File.Exists(readOnlyDestination));

            Assert.AreEqual(File.GetCreationTime(readOnlyDestination), creationTime, "File CreationTime should match.");
            Assert.AreEqual(File.GetLastAccessTime(readOnlyDestination), lastAccessTime, "File LastAccessTime should match.");
            Assert.AreEqual(File.GetLastWriteTime(readOnlyDestination), lastWriteTime, "File LastWriteTime should match.");
            Console.WriteLine("\nTimestamps are transferred.");

            #endregion Preserve Timestamps
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

      #endregion // DumpCopy

      #region TestDelete

      private void TestDelete(bool isLocal)
      {
         #region Setup

         Console.WriteLine("\n=== TEST {0} ===", isLocal ? UnitTestConstants.Local : UnitTestConstants.Network);
         string tempFolder = Path.GetTempPath();
         string tempPath = Path.Combine(tempFolder, "File.Delete-" + Path.GetRandomFileName());
         if (!isLocal) tempPath = Path.LocalToUnc(tempPath);

         string nonExistingFile = UnitTestConstants.SysRoot32 + @"\NonExistingFile-" + Path.GetRandomFileName();
         if (!isLocal) nonExistingFile = Path.LocalToUnc(nonExistingFile);

         string sysDrive = UnitTestConstants.SysDrive;
         if (!isLocal) sysDrive = Path.LocalToUnc(sysDrive);

         string sysRoot = UnitTestConstants.SysRoot;
         if (!isLocal) sysRoot = Path.LocalToUnc(sysRoot);

         string letter = DriveInfo.GetFreeDriveLetter() + @":\";
         if (!isLocal) letter = Path.LocalToUnc(letter);

         int catchCount = 0;
         bool exception;
         int expectedLastError;
         string expectedException;

         #endregion // Setup

         try
         {
            #region ArgumentException

            expectedException = "System.ArgumentException";
            exception = false;
            try
            {
               Console.WriteLine("\nCatch #{0}: [{1}]: Path is a zero-length string, contains only white space, or contains one or more invalid characters.", ++catchCount, expectedException);

               File.Delete(sysDrive + @"\<>");
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

            #region NotSupportedException

            expectedLastError = (int)(isLocal ? Win32Errors.ERROR_FILE_EXISTS : Win32Errors.NERR_UseNotFound);
            expectedException = "System.NotSupportedException";
            exception = false;
            try
            {
               Console.WriteLine("\nCatch #{0}: [{1}]: path is in an invalid format.", ++catchCount, expectedException);

               string invalidPath = UnitTestConstants.SysDrive + @"\dev\test\aaa:aaa.txt";
               if (!isLocal) invalidPath = Path.LocalToUnc(invalidPath) + ":aaa.txt";

               File.Delete(invalidPath);
            }
            catch (NotSupportedException ex)
            {
               // win32Error is always 0 for local.
               if (!isLocal)
               {
                  var win32Error = new Win32Exception("", ex);
                  Assert.IsTrue(win32Error.NativeErrorCode == expectedLastError, string.Format("Expected Win32Exception error should be: [{0}], got: [{1}]", expectedLastError, win32Error.NativeErrorCode));
               }

               exception = true;
               Console.WriteLine("\n\t[{0}]: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
            }
            catch (Exception ex)
            {
               Console.WriteLine("\n\tCaught (unexpected) {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
            }
            Assert.IsTrue(exception, "[{0}] should have been caught.", expectedException);
            Console.WriteLine();

            #endregion // NotSupportedException

            #region UnauthorizedAccessException

            expectedLastError = (int)Win32Errors.ERROR_ACCESS_DENIED;
            expectedException = "System.UnauthorizedAccessException";
            exception = false;
            try
            {
               Console.WriteLine("\nCatch #{0}: [{1}]: The caller does not have the required permission.", ++catchCount, expectedException);

               File.Delete(UnitTestConstants.SysRoot32 + @"\kernel32.dll");
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

            #endregion // UnauthorizedAccessException

            #region DirectoryNotFoundException (UnitTestConstants.Local) / IOException (UnitTestConstants.Network)

            expectedLastError = (int)(isLocal ? Win32Errors.ERROR_PATH_NOT_FOUND : Win32Errors.ERROR_BAD_NET_NAME);
            expectedException = isLocal ? "System.IO.DirectoryNotFoundException" : "System.IO.IOException";
            exception = false;
            try
            {
               Console.WriteLine("\nCatch #{0}: [{1}]: The specified path is invalid (for example, it is on an unmapped drive).", ++catchCount, expectedException);

               File.Delete(nonExistingFile.Replace(sysDrive + @"\", letter));
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
            }
            Assert.IsTrue(exception, "[{0}] should have been caught.", expectedException);
            Console.WriteLine();

            #endregion // DirectoryNotFoundException (UnitTestConstants.Local) / IOException (UnitTestConstants.Network)

            #region UnauthorizedAccessException #1

            expectedLastError = (int)Win32Errors.ERROR_ACCESS_DENIED;
            expectedException = "System.UnauthorizedAccessException";
            exception = false;
            try
            {
               Console.WriteLine("\nCatch #{0}: [{1}]: Path is a directory.", ++catchCount, expectedException);

               Directory.CreateDirectory(tempPath);

               File.Delete(tempPath);
            }
            catch (UnauthorizedAccessException ex)
            {
               var win32Error = new Win32Exception("", ex);
               Assert.IsTrue(win32Error.NativeErrorCode == expectedLastError, string.Format("Expected Win32Exception error should be: [{0}], got: [{1}]", expectedLastError, win32Error.NativeErrorCode));
               Assert.IsTrue(ex.Message.StartsWith("(" + Win32Errors.ERROR_INVALID_PARAMETER + ")"), string.Format("Expected Win32Exception error is: [{0}]", expectedLastError));

               exception = true;
               Console.WriteLine("\n\t[{0}]: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
            }
            catch (Exception ex)
            {
               Console.WriteLine("\n\tCaught (unexpected) {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
            }
            Assert.IsTrue(exception, "[{0}] should have been caught.", expectedException);
            Directory.Delete(tempPath);
            Assert.IsFalse(Directory.Exists(tempPath), "Cleanup failed: Directory should have been removed.");
            Console.WriteLine();

            #endregion // UnauthorizedAccessException #1

            #region IOException

            using (File.Create(tempPath))
            {
            }
            FileStream streamCreate2 = File.Open(tempPath, FileMode.Open);

            expectedLastError = (int)Win32Errors.ERROR_SHARING_VIOLATION;
            expectedException = "System.IO.IOException";
            exception = false;
            try
            {
               Console.WriteLine("\nCatch #{0}: [{1}]: The specified file is in use or there is an open handle on the file.", ++catchCount, expectedException);

               File.Delete(tempPath);
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
            streamCreate2.Close();
            Console.WriteLine();

            #endregion // IOException

            #region UnauthorizedAccessException #2 (FileReadOnlyException)

            expectedLastError = (int)Win32Errors.ERROR_ACCESS_DENIED;
            expectedException = "System.UnauthorizedAccessException";
            exception = false;
            try
            {
               Console.WriteLine("\nCatch #{0}: [{1}]: Path specified a read-only file.", ++catchCount, expectedException);

               File.SetAttributes(tempPath, FileAttributes.ReadOnly);

               File.Delete(tempPath);
            }
            catch (FileReadOnlyException ex)
            {
               var win32Error = new Win32Exception("", ex);
               Assert.IsTrue(win32Error.NativeErrorCode == expectedLastError, string.Format("Expected Win32Exception error should be: [{0}], got: [{1}]", expectedLastError, win32Error.NativeErrorCode));
               Assert.IsTrue(ex.Message.StartsWith("(" + Win32Errors.ERROR_FILE_READ_ONLY + ")"), string.Format("Expected Win32Exception error is: [{0}]", expectedLastError));

               exception = true;
               Console.WriteLine("\n\t[{0}]: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
            }
            catch (Exception ex)
            {
               Console.WriteLine("\n\tCaught (unexpected) {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
            }
            Assert.IsTrue(exception, "[{0}] should have been caught.", expectedException);
            Console.WriteLine();

            #endregion // UnauthorizedAccessException #2 (FileReadOnlyException)
         }
         finally
         {
            File.Delete(tempPath, true);
            Assert.IsFalse(File.Exists(tempPath), "Cleanup failed: File should have been removed.");
         }

         Console.WriteLine();
      }

      #endregion // TestDelete
      
      #region DumpMove

      private void DumpMove(bool isLocal)
      {
         #region Setup

         Console.WriteLine("\n=== TEST {0} ===", isLocal ? UnitTestConstants.Local : UnitTestConstants.Network);
         string path = isLocal ? UnitTestConstants.SysRoot : Path.LocalToUnc(UnitTestConstants.SysRoot);
         string tempPath = Path.GetTempPath("File-Move-" + Path.GetRandomFileName());
         if (!isLocal) tempPath = Path.LocalToUnc(tempPath);

         bool exception;
         int expectedLastError;
         string expectedException;

         string fileSource = @"file-source-" + Path.GetRandomFileName() + ".exe";
         string fileDestination = @"file-destination-" + Path.GetRandomFileName() + ".exe";

         string folderSource = tempPath + @"\folder-source-" + Path.GetRandomFileName();
         string folderDestination = tempPath + @"\folder-destination-" + Path.GetRandomFileName();

         string fullPathSource = folderSource + @"\" + fileSource;
         string fullPathDestination = folderDestination + @"\" + fileDestination;
         if (!isLocal) fullPathSource = Path.LocalToUnc(fullPathSource);
         if (!isLocal) fullPathDestination = Path.LocalToUnc(fullPathDestination);

         DirectoryInfo dirInfo = new DirectoryInfo(folderDestination);

         #endregion // Setup

         try
         {
            #region UnauthorizedAccessException

            Directory.CreateDirectory(folderSource);
            Directory.CreateDirectory(folderDestination);

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

               FileInfo fileInfo = new FileInfo(fullPathSource);
               using (StreamWriter sw = fileInfo.CreateText())
                  sw.WriteLine("MoveTo-TestFile");

               // Set DENY for current user.
               dirSecurity = dirInfo.GetAccessControl();
               dirSecurity.AddAccessRule(rule);
               dirInfo.SetAccessControl(dirSecurity);

               fileInfo.MoveTo(fullPathDestination);
            }
            catch (Exception ex)
            {
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
            finally
            {
               // Remove DENY for current user.
               dirSecurity = dirInfo.GetAccessControl();
               dirSecurity.RemoveAccessRule(rule);
               dirInfo.SetAccessControl(dirSecurity, AccessControlSections.Access);

               Directory.Delete(tempPath, true, true);
            }
            Assert.IsTrue(exception, "[{0}] should have been caught.", expectedException);

            Console.WriteLine();

            #endregion // UnauthorizedAccessException

            #region FileNotFoundException

            expectedLastError = (int)Win32Errors.ERROR_FILE_NOT_FOUND;
            expectedException = "System.IO.FileNotFoundException";
            exception = false;
            try
            {
               Console.WriteLine("\nCatch: [{0}]", expectedException);
               File.Move(isLocal ? fileSource : Path.LocalToUnc(fileSource), isLocal ? fileDestination : Path.LocalToUnc(fileDestination));
            }
            catch (FileNotFoundException ex)
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

            #endregion // FileNotFoundException

            #region DirectoryNotFoundException

            expectedLastError = (int)Win32Errors.ERROR_PATH_NOT_FOUND;
            expectedException = "System.IO.DirectoryNotFoundException";
            exception = false;
            try
            {
               Console.WriteLine("\nCatch: [{0}]", expectedException);
               File.Move(fullPathSource, fullPathDestination);
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

            Directory.CreateDirectory(folderSource);
            Directory.CreateDirectory(folderDestination);
            using (File.Create(fullPathSource)) { }
            using (File.Create(fullPathDestination)) { }

            expectedLastError = (int)Win32Errors.ERROR_ALREADY_EXISTS;
            expectedException = "System.IO.IOException";
            exception = false;
            try
            {
               Console.WriteLine("\nCatch: [{0}]", expectedException);
               File.Move(fullPathSource, fullPathDestination);
            }
            catch (AlreadyExistsException ex)
            {
               // win32Error is always 0
               //var win32Error = new Win32Exception("", ex);
               //Assert.IsTrue(win32Error.NativeErrorCode == expectedLastError, string.Format("Expected Win32Exception error should be: [{0}], got: [{1}]", expectedLastError, win32Error.NativeErrorCode));
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

            Directory.Delete(folderSource, true);
            Directory.Delete(folderDestination, true);

            #endregion // IOException #1

            #region IOException #2

            string folderfileName = null;

            expectedLastError = (int)Win32Errors.ERROR_ACCESS_DENIED;
            expectedException = "System.IO.IOException";
            exception = false;
            try
            {
               Console.WriteLine("\nCatch: [{0}]", expectedException);
               foreach (string file in Directory.EnumerateFiles(path))
               {
                  string newFile = Path.Combine(tempPath, Path.GetFileName(file, true));
                  folderfileName = newFile;

                  // Trigger the Exception.
                  Directory.CreateDirectory(folderfileName);

                  // MoveOptions.None: overwrite existing.
                  File.Move(file, folderfileName, MoveOptions.None);
               }
            }
            catch (AlreadyExistsException ex)
            {
               var win32Error = new Win32Exception("", ex);
               Assert.IsTrue(win32Error.NativeErrorCode == expectedLastError, string.Format("Expected Win32Exception error should be: [{0}], got: [{1}]", expectedLastError, win32Error.NativeErrorCode));

               exception = true;
               Directory.Delete(folderfileName);
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

            #region Copy (Use as Move() source)

            foreach (string file in Directory.EnumerateFiles(path))
               File.Copy(file, Path.Combine(tempPath, Path.GetFileName(file, true)));

            #endregion // Copy (Use as Move() source)

            string movePath = Path.GetTempPath("File-Move-II-" + Path.GetRandomFileName());
            if (!isLocal) movePath = Path.LocalToUnc(movePath);

            Directory.CreateDirectory(movePath);

            Console.WriteLine("\nInput Directory Path: [{0}]\n", tempPath);
            int cnt = 0;
            string readOnlySource = null;
            string readOnlyDestination = null;

            UnitTestConstants.StopWatcher(true);
            foreach (string file in Directory.EnumerateFiles(tempPath))
            {
               string newFile = Path.Combine(movePath, Path.GetFileName(file, true));
               File.Move(file, newFile);

               // A read-only file triggers UnauthorizedAccessException when moving again.
               if (cnt == 0)
               {
                  File.SetAttributes(newFile, FileAttributes.ReadOnly);
                  readOnlySource = file;
                  readOnlyDestination = newFile;
               }

               Console.WriteLine("\t#{0:000}\tMoved to: [{1}]", ++cnt, newFile);
               Assert.IsTrue(File.Exists(newFile));
            }
            Console.WriteLine("\n\tTotal Size: [{0}]{1}", Utils.UnitSizeToText(Directory.GetProperties(movePath)["Size"]), UnitTestConstants.Reporter());
            Console.WriteLine();

            #endregion // Move

            // Move again, use overwrite to prevent IOException: The destination file already exists.

            #region Copy (Use as Move() source)

            foreach (string file in Directory.EnumerateFiles(path))
               File.Copy(file, Path.Combine(tempPath, Path.GetFileName(file, true)));

            #endregion // Copy (Use as Move() source)

            #region Remove Read-Only Attribute, Move Again

            Console.WriteLine("\nRemove read-only attribute and move again.");

            #region Preserve Timestamps

            // Test preservation of timestamps.
            int seed = (int)DateTime.Now.Ticks & 0x0000FFFF;
            DateTime creationTime = new DateTime(new Random(seed).Next(1971, 2071), new Random(seed).Next(1, 12), new Random(seed).Next(1, 28), new Random(seed).Next(0, 23), new Random(seed).Next(0, 59), new Random(seed).Next(0, 59));
            seed += (int)DateTime.Now.Ticks & 0x0000FFFF;
            DateTime lastAccessTime = new DateTime(new Random(seed).Next(1971, 2071), new Random(seed).Next(1, 12), new Random(seed).Next(1, 28), new Random(seed).Next(0, 23), new Random(seed).Next(0, 59), new Random(seed).Next(0, 59));
            seed += (int)DateTime.Now.Ticks & 0x0000FFFF;
            DateTime lastWriteTime = new DateTime(new Random(seed).Next(1971, 2071), new Random(seed).Next(1, 12), new Random(seed).Next(1, 28), new Random(seed).Next(0, 23), new Random(seed).Next(0, 59), new Random(seed).Next(0, 59));

            File.SetCreationTime(readOnlySource, creationTime);
            File.SetLastAccessTime(readOnlySource, lastAccessTime);
            File.SetLastWriteTime(readOnlySource, lastWriteTime);

            #endregion Preserve Timestamps

            UnitTestConstants.StopWatcher(true);

            // 3rd parameter MoveOptions.ReplaceExisting: overwrite existing.
            // File.Move() automatically preserves Timestamps.
            File.Move(readOnlySource, readOnlyDestination, MoveOptions.ReplaceExisting);

            Console.WriteLine("\tFile moved.{0}", UnitTestConstants.Reporter());

            Assert.IsFalse(File.Exists(readOnlySource));
            Assert.IsTrue(File.Exists(readOnlyDestination));

            Assert.AreEqual(File.GetCreationTime(readOnlyDestination), creationTime, "File CreationTime should match.");
            Assert.AreEqual(File.GetLastAccessTime(readOnlyDestination), lastAccessTime, "File LastAccessTime should match.");
            Assert.AreEqual(File.GetLastWriteTime(readOnlyDestination), lastWriteTime, "File LastWriteTime should match.");
            Console.WriteLine("\nTimestamps are transferred.");

            Directory.Delete(movePath, true, true);
            Assert.IsFalse(Directory.Exists(movePath), "Cleanup failed: Directory should have been removed.");

            #endregion // Remove Read-Only Attribute, Move Again
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

      #endregion // DumpMove

      #region DumpGetSetAttributes

      private void DumpGetSetAttributes(bool isLocal)
      {
         Console.WriteLine("\n=== TEST {0} ===", isLocal ? UnitTestConstants.Local : UnitTestConstants.Network);
         string tmp = Path.Combine(Path.GetTempPath(), "File.SetAttributes()-" + Path.GetRandomFileName());
         string tempPath = isLocal ? tmp : Path.LocalToUnc(tmp);
         string sys32 = isLocal ? UnitTestConstants.SysRoot32 : Path.LocalToUnc(UnitTestConstants.SysRoot32);

         Console.WriteLine("\nInput Path: [{0}]", sys32);

         // Just enumerate and compare attributes in folder: C:\Windows\System32
         foreach (string file in Directory.EnumerateFiles(sys32))
         {
            FileAttributes actual = File.GetAttributes(file);
            FileAttributes expected = System.IO.File.GetAttributes(file);

            Assert.AreEqual(expected, actual, "AlphaFS != System.IO");
         }


         Console.WriteLine("\nInput Path: [{0}]", tempPath);

         // Create some folders and files.
         UnitTestConstants.CreateDirectoriesAndFiles(tempPath, 10, true);

         FileAttributes apply = FileAttributes.Hidden | FileAttributes.Archive | FileAttributes.System | FileAttributes.ReadOnly;
         Console.WriteLine("\nSetAttributes(): [{0}]", apply);

         bool allOk = true;
         int cnt = 0;
         UnitTestConstants.StopWatcher(true);
         foreach (string file in Directory.EnumerateFiles(tempPath))
         {
            try
            {
               File.SetAttributes(file, apply);

               FileAttributes actual = File.GetAttributes(file);
               FileAttributes expected = System.IO.File.GetAttributes(file);

               Console.WriteLine("\n\t#{0:000}\tFile     : [{1}]\n\t\tAlphaFS  : [{2}]\n\t\tSystem.IO: [{3}]", ++cnt, file, expected, actual);

               if (cnt == 0)
                  Assert.Inconclusive("Nothing was enumerated.");

               Assert.AreEqual(expected, actual, "AlphaFS != System.IO");
            }
            catch (Exception ex)
            {
               allOk = false;
               Console.WriteLine("\n\tCaught (unexpected) {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
            }
         }
         Console.WriteLine();
         Console.WriteLine(UnitTestConstants.Reporter());
         Assert.IsTrue(allOk);


         apply = FileAttributes.Normal;
         Console.WriteLine("\nSetAttributes(): [{0}]", apply);

         allOk = true;
         cnt = 0;
         UnitTestConstants.StopWatcher(true);
         foreach (string file in Directory.EnumerateFiles(tempPath))
         {
            try
            {
               File.SetAttributes(file, apply);

               FileAttributes actual = File.GetAttributes(file);
               FileAttributes expected = System.IO.File.GetAttributes(file);

               Console.WriteLine("\n\t#{0:000}\tFile     : [{1}]\n\t\tAlphaFS  : [{2}]\n\t\tSystem.IO: [{3}]", ++cnt, file, expected, actual);

               if (cnt == 0)
                  Assert.Inconclusive("Nothing was enumerated.");

               Assert.AreEqual(expected, actual, "AlphaFS != System.IO");
            }
            catch (Exception ex)
            {
               allOk = false;
               Console.WriteLine("\n\tCaught (unexpected) {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
            }
         }
         Console.WriteLine();
         Console.WriteLine(UnitTestConstants.Reporter());


         Directory.Delete(tempPath, true);
         Assert.IsFalse(Directory.Exists(tempPath), "Cleanup failed: Directory should have been removed.");
         Assert.IsTrue(allOk);
         Console.WriteLine();
      }

      #endregion // DumpGetSetAttributes

      #region DumpReadAllLines

      private void DumpReadAllLines(bool isLocal)
      {
         Console.WriteLine("\n=== TEST {0} ===", isLocal ? UnitTestConstants.Local : UnitTestConstants.Network);
         string tmp = Path.Combine(Path.GetTempPath(), "File.SetAttributes()-" + Path.GetRandomFileName());
         string tempPath = isLocal ? tmp : Path.LocalToUnc(tmp);

         // Create file and append text.
         string tempFile = Path.GetTempFileName();

         string[] createText = { "Hello", "And", "Welcome" };
         File.WriteAllLines(tempFile, createText);

         Console.WriteLine("\nFile.ReadAllLines()\n");
         string[] readText = File.ReadAllLines(tempFile);
         foreach (string s in readText)
         {
            Console.WriteLine("\t{0}", s);
            Assert.IsTrue(createText.Contains(s));
         }

         Console.WriteLine("\nFile.ReadLines()\n");
         foreach (string s in File.ReadLines((tempFile)))
         {
            Console.WriteLine("\t{0}", s);
            Assert.IsTrue(createText.Contains(s));
         }

         File.Delete(tempFile, true);
         Assert.IsFalse(File.Exists(tempFile), "Cleanup failed: File should have been removed.");
      }

      #endregion // DumpReadAllLines

      #region DumpReadWriteAllBytes

      private void DumpReadWriteAllBytes(bool isLocal)
      {
         Console.WriteLine("\n=== TEST {0} ===", isLocal ? UnitTestConstants.Local : UnitTestConstants.Network);
         string tempPath = Path.GetTempPath("File.ReadWriteAllBytes()-" + Path.GetRandomFileName());
         if (!isLocal) { tempPath = Path.LocalToUnc(tempPath); }

         int size = 10000;
         byte[] text = Encoding.UTF8.GetBytes(new string('X', size));
         bool allOk = true;

         try
         {
            File.WriteAllBytes(tempPath, text);
            Console.WriteLine("\nWriteAllBytes(): [{0}] bytes: [{1}]", size, tempPath);
         }
         catch (Exception ex)
         {
            allOk = false;
            Console.WriteLine("\n\tCaught (unexpected) {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
         }
         Assert.IsTrue(File.Exists(tempPath), "File.WriteAllBytes(): File was not created.");
         long fileSize = File.GetSize(tempPath);
         Assert.AreEqual(size, fileSize);
         Assert.IsTrue(allOk);



         byte[] readAllAlphaFS = { };
         byte[] readAllSysIo = { };

         try
         {
            readAllAlphaFS = File.ReadAllBytes(tempPath);
            readAllSysIo = System.IO.File.ReadAllBytes(tempPath);
            Console.WriteLine("\nReadAllBytes(): [{0}] bytes.", readAllAlphaFS.Length);
         }
         catch (Exception ex)
         {
            allOk = false;
            Console.WriteLine("\n\tCaught (unexpected) {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
         }

         Assert.AreEqual(readAllAlphaFS.Length, fileSize, "File.ReadAllBytes(): Number of bytes are different.");
         Assert.AreEqual(readAllAlphaFS.Length, readAllSysIo.Length, "File.ReadAllBytes(): AlphaFS != System.IO");



         File.Delete(tempPath, true);
         Assert.IsFalse(File.Exists(tempPath), "Cleanup failed: File should have been removed.");
         Assert.IsTrue(allOk);
         Console.WriteLine("\n");
      }

      #endregion // DumpReadWriteAllBytes
      
      #endregion // Unit Tests

      #region .NET

      #region AppendAllLines

      [TestMethod]
      public void File_AppendAllLines()
      {
         Console.WriteLine("File.AppendAllLines()");

         DumpAppendAllLines(true);
         DumpAppendAllLines(false);
      }

      [TestMethod]
      public void File_AppendAllLinesThenReadAllLinesShouldReturnSameCollection()
      {
         var file = Path.GetTempFileName();
         var sample = new []{ "line one", "line two" };

         try
         {
            File.AppendAllLines(file, sample);
            CollectionAssert.AreEquivalent(sample, File.ReadAllLines(file).ToArray());
         }
         finally
         {
            File.Delete(file);
         }
      }

      #endregion // AppendAllLines

      #region AppendAllText

      [TestMethod]
      public void File_AppendAllText()
      {
         Console.WriteLine("File.AppendAllText()");
         Console.WriteLine("\n\tDefault AlphaFS Encoding: [{0}]", NativeMethods.DefaultFileEncoding.EncodingName);

         // Create file and append text.
         string tempFile = Path.GetTempFileName();

         string allLines = UnitTestConstants.TextHelloWorld;

         // Create real UTF-8 file.
         File.AppendAllText(tempFile, allLines, NativeMethods.DefaultFileEncoding);

         // Read filestream contents.
         using (StreamReader streamRead = File.OpenText(tempFile))
         {
            string line = streamRead.ReadToEnd();

            Console.WriteLine("\n\tCreated: [{0}] filestream: [{1}]\n\n\tAppendAllText content:\n{2}", streamRead.CurrentEncoding.EncodingName, tempFile, line);

            Assert.IsTrue(line.Contains(allLines));
         }

         // Append
         File.AppendAllText(tempFile, "Append 1");
         File.AppendAllText(tempFile, allLines);
         File.AppendAllText(tempFile, "Append 2");
         File.AppendAllText(tempFile, allLines);

         // Read filestream contents.
         using (StreamReader streamRead = File.OpenText(tempFile))
         {
            string line = streamRead.ReadToEnd();

            Console.WriteLine("\tAppendAllText content:\n{0}", line);

            Assert.IsTrue(line.Contains(allLines));
            Assert.IsTrue(line.Contains("Append 1"));
            Assert.IsTrue(line.Contains("Append 2"));
         }

         File.Delete(tempFile, true);
         Assert.IsFalse(File.Exists(tempFile), "Cleanup failed: File should have been removed.");
      }

      #endregion // AppendAllText

      #region AppendText

      [TestMethod]
      public void File_AppendText()
      {
         Console.WriteLine("File.AppendText()");

         string utf8 = NativeMethods.DefaultFileEncoding.BodyName.ToUpperInvariant();
         string line;
         string matchLine = string.Empty;
         string tempFile = Path.GetTempFileName();

         StreamReader streamRead;
         StreamWriter streamWrite;

         Console.WriteLine("Default AlphaFS Encoding: {0}", NativeMethods.DefaultFileEncoding.EncodingName);

         #region Create Filestream, CreateText()

         // Create filestream and append text as UTF-8, default.
         using (streamWrite = File.CreateText(tempFile))
         {
            streamWrite.Write(UnitTestConstants.TextHelloWorld);
         }

         // Read filestream contents.
         using (streamRead = File.OpenText(tempFile))
         {
            while ((line = streamRead.ReadLine()) != null)
            {
               Console.WriteLine("\n CreateText(): [{0}] filestream: [{1}]\n  Appended: [{2}]\n  Content : [{3}]", streamRead.CurrentEncoding.EncodingName, tempFile, UnitTestConstants.TextHelloWorld, line);
               matchLine = line; // Catch the last line.
            }
         }
         Assert.IsTrue(matchLine.Equals(UnitTestConstants.TextHelloWorld, StringComparison.OrdinalIgnoreCase));

         #endregion // Create Filestream, CreateText()

         #region AppendText() to Filestream

         // Append text as UTF-8, default.
         using (streamWrite = File.AppendText(tempFile))
         {
            streamWrite.Write(UnitTestConstants.TextAppend);
         }

         // Read filestream contents.
         using (streamRead = File.OpenText(tempFile))
         {
            while ((line = streamRead.ReadLine()) != null)
            {
               Console.WriteLine("\n AppendText() as [{0}]\n  Appended: [{1}]\n  Content : [{2}]", utf8, UnitTestConstants.TextAppend, line);
            }
         }

         // Append text as UTF-8, default.
         using (streamWrite = File.AppendText(tempFile))
         {
            streamWrite.WriteLine(UnitTestConstants.TextUnicode);
         }

         // Read filestream contents.
         matchLine = string.Empty;
         using (streamRead = File.OpenText(tempFile))
         {
            while ((line = streamRead.ReadLine()) != null)
            {
               Console.WriteLine("\n AppendText() as [{0}]\n  Appended: [{1}]\n  Content : [{2}]", utf8, UnitTestConstants.TextAppend, line);
               matchLine = line; // Catch the last line.
            }
         }

         Assert.IsTrue(matchLine.Equals(UnitTestConstants.TextHelloWorld + UnitTestConstants.TextAppend + UnitTestConstants.TextUnicode, StringComparison.OrdinalIgnoreCase));

         File.Delete(tempFile, true);
         Assert.IsFalse(File.Exists(tempFile), "Cleanup failed: File should have been removed.");

         #endregion // AppendText() to Filestream
      }

      #endregion // AppendText

      #region Copy

      [TestMethod]
      public void File_Copy()
      {
         Console.WriteLine("File.Copy()");

         DumpCopy(true);
         DumpCopy(false);
      }

      #endregion // Copy

      #region CreateText

      [TestMethod]
      public void File_CreateText()
      {
         Console.WriteLine("File.CreateText()");

         File_AppendText();
      }

      #endregion // CreateText

      #region Delete

      [TestMethod]
      public void File_Delete()
      {
         Console.WriteLine("File.Delete()");

         TestDelete(true);
         TestDelete(false);
      }

      #endregion // Delete

      #region Encrypt

      [TestMethod]
      public void File_Encrypt()
      {
         Console.WriteLine("File.Encrypt()");

         // Create file and append text.
         string tempFile = Path.GetTempFileName();

         // Append text as UTF-8, default.
         File.AppendAllText(tempFile, UnitTestConstants.TextHelloWorld);

         string utf8 = NativeMethods.DefaultFileEncoding.BodyName.ToUpperInvariant();
         string readText8 = File.ReadAllText(tempFile);
         FileAttributes actual = File.GetAttributes(tempFile);
         FileEncryptionStatus encryptionStatus = File.GetEncryptionStatus(tempFile);
         Console.WriteLine("\n\tCreated {0} file: [{1}]", utf8, tempFile);
         Console.WriteLine("\tContent: [{0}]", readText8);
         Console.WriteLine("\n\tFile.GetAttributes(): [{0}]", actual);
         Console.WriteLine("\tEncryption status   : [{0}]", encryptionStatus);

         bool encryptOk = false;
         try
         {
            File.Encrypt(tempFile);
            encryptOk = true;
            actual = File.GetAttributes(tempFile);
         }
         catch (Exception ex)
         {
            Console.WriteLine("\n\tCaught (unexpected) {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
         }
         encryptionStatus = File.GetEncryptionStatus(tempFile);
         Console.WriteLine("\n\tFile.Encrypt() (Should be True): [{0}]", encryptOk);
         Console.WriteLine("\tFile.GetAttributes()           : [{0}]", actual);
         Console.WriteLine("\tEncryption status              : [{0}]", encryptionStatus);

         bool decryptOk = false;
         try
         {
            File.Decrypt(tempFile);
            decryptOk = true;
            actual = File.GetAttributes(tempFile);
         }
         catch (Exception ex)
         {
            Console.WriteLine("\n\tCaught (unexpected) {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
         }
         FileEncryptionStatus decryptionStatus = File.GetEncryptionStatus(tempFile);
         Console.WriteLine("\n\tFile.Decrypt() (Should be True): [{0}]:", decryptOk);
         Console.WriteLine("\tFile.GetAttributes()           : [{0}]", actual);
         Console.WriteLine("\tDecryption status              : [{0}]", decryptionStatus);

         Assert.IsTrue(encryptOk, "File should be encrypted.");
         Assert.IsTrue(encryptionStatus == FileEncryptionStatus.Encrypted, "File should be encrypted.");
         Assert.IsTrue(decryptOk, "File should be decrypted.");
         Assert.IsTrue(decryptionStatus == FileEncryptionStatus.Encryptable, "File should be decrypted.");

         File.Delete(tempFile, true);
         Assert.IsFalse(File.Exists(tempFile), "Cleanup failed: File should have been removed.");
      }

      #endregion // Encrypt

      #region GetAttributes

      [TestMethod]
      public void File_GetAttributes()
      {
         Console.WriteLine("File.GetAttributes()");

         DumpGetSetAttributes(true);
         DumpGetSetAttributes(false);
      }

      #endregion // GetAttributes


      #region Move

      [TestMethod]
      public void File_Move()
      {
         Console.WriteLine("File.Move()");

         DumpMove(true);
         DumpMove(false);
      }

      #endregion // Move

      #region OpenWrite

      [TestMethod]
      public void File_OpenWrite()
      {
         Console.WriteLine("File.OpenWrite()\n");

         string path = Path.GetTempPath("File.OpenWrite()-" + Path.GetRandomFileName());

         // Open the stream and write to it.
         using (FileStream fs = File.OpenWrite(path))
         {
            byte[] info = new UTF8Encoding(true).GetBytes("This is to test the OpenWrite method.");

            // Add some information to the file.
            fs.Write(info, 0, info.Length);
         }

         bool fileExists = File.Exists(path);


         // Open the stream and read it back.
         using (FileStream fs = File.OpenRead(path))
         {
            byte[] b = new byte[1024];
            UTF8Encoding temp = new UTF8Encoding(true);

            while (fs.Read(b, 0, b.Length) > 0)
            {
               Console.WriteLine(temp.GetString(b));
            }
         }

         File.Delete(path, true);
         Assert.IsFalse(File.Exists(path), "Cleanup failed: File should have been removed.");

         Assert.IsTrue(fileExists);
      }

      #endregion // OpenWrite

      #region ReadAllBytes

      [TestMethod]
      public void File_ReadAllBytes()
      {
         Console.WriteLine("File.ReadAllBytes()");

         DumpReadWriteAllBytes(true);
         DumpReadWriteAllBytes(false);
      }

      #endregion // WriteAllBytes

      #region ReadAllLines

      [TestMethod]
      public void File_ReadAllLines()
      {
         Console.WriteLine("File.ReadAllLines()");

         DumpReadAllLines(true);
         DumpReadAllLines(false);
      }

      #endregion // ReadAllLines

      #region ReadAllText

      [TestMethod]
      public void File_ReadAllText()
      {
         Console.WriteLine("File.ReadAllText()\n");

         // Create file and append text.
         string tempFile = Path.GetTempFileName();

         string[] createText = { "Hello", "And", "Welcome" };
         File.WriteAllLines(tempFile, createText);

         // Open the file to read from. 
         string textRead = File.ReadAllText(tempFile);
         Console.WriteLine(textRead);

         File.Delete(tempFile, true);
         Assert.IsFalse(File.Exists(tempFile), "Cleanup failed: File should have been removed.");
      }

      #endregion // ReadAllText

      #region ReadLines

      [TestMethod]
      public void File_ReadLines()
      {
         Console.WriteLine("File.ReadLines()");

         File_ReadAllLines();
      }

      #endregion // ReadLines

      #region WriteAllBytes

      [TestMethod]
      public void File_WriteAllBytes()
      {
         Console.WriteLine("File.WriteAllBytes()");

         File_ReadAllBytes();
      }

      #endregion // WriteAllBytes

      #region WriteAllLines

      [TestMethod]
      public void File_WriteAllLines()
      {
         Console.WriteLine("File.WriteAllLines()");
         Console.WriteLine("\n Default AlphaFS Encoding: [{0}]", NativeMethods.DefaultFileEncoding.EncodingName);

         // Create file and append text.
         string tempFile = Path.GetTempFileName();

         string[] allLines = new[] { UnitTestConstants.TenNumbers, UnitTestConstants.TextHelloWorld, UnitTestConstants.TextAppend, UnitTestConstants.TextUnicode };

         // Create real UTF-8 file.
         File.WriteAllLines(tempFile, allLines, NativeMethods.DefaultFileEncoding);

         // Read filestream contents.
         using (StreamReader streamRead = File.OpenText(tempFile))
         {
            string line = streamRead.ReadToEnd();

            Console.WriteLine("\n Created: [{0}] filestream: [{1}]\n\n WriteAllLines content:\n{2}", streamRead.CurrentEncoding.EncodingName, tempFile, line);

            foreach (string line2 in allLines)
               Assert.IsTrue(line.Contains(line2));
         }

         File.Delete(tempFile, true);
         Assert.IsFalse(File.Exists(tempFile), "Cleanup failed: File should have been removed.");
      }

      #endregion // WriteAllLines

      #region WriteAllText

      [TestMethod]
      public void File_WriteAllText()
      {
         Console.WriteLine("File.WriteAllText()");
         Console.WriteLine("\n\tDefault AlphaFS Encoding: [{0}]", NativeMethods.DefaultFileEncoding.EncodingName);

         // Create file and append text.
         string tempFile = Path.GetTempFileName();

         string allLines = UnitTestConstants.TextHelloWorld;

         // Create real UTF-8 file.
         File.WriteAllText(tempFile, allLines, NativeMethods.DefaultFileEncoding);

         // Read filestream contents.
         using (StreamReader streamRead = File.OpenText(tempFile))
         {
            string line = streamRead.ReadToEnd();

            Console.WriteLine("\n\tCreated: [{0}] filestream: [{1}]\n\n\tWriteAllText content:\n{2}", streamRead.CurrentEncoding.EncodingName, tempFile, line);

            Assert.IsTrue(line.Contains(allLines));
         }

         // (over)Write.
         File.WriteAllText(tempFile, "Append 1");
         File.WriteAllText(tempFile, allLines);
         File.WriteAllText(tempFile, "Append 2");
         File.WriteAllText(tempFile, allLines);

         // Read filestream contents.
         using (StreamReader streamRead = File.OpenText(tempFile))
         {
            string line = streamRead.ReadToEnd();

            Console.WriteLine("\tWriteAllText content:\n{0}", line);

            Assert.IsTrue(line.Contains(allLines));
            Assert.IsTrue(!line.Contains("Append 1"));
            Assert.IsTrue(!line.Contains("Append 2"));
         }

         File.Delete(tempFile, true);
         Assert.IsFalse(File.Exists(tempFile), "Cleanup failed: File should have been removed.");
      }

      #endregion // WriteAllText

      #endregion // .NET
   }
}
