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
   public class FileTest
   {
      #region Unit Tests

      #region DumpAccessRules

      private static void DumpAccessRules(int cntCheck, FileSecurity dsSystem, FileSecurity dsAlpha)
      {
         Console.WriteLine("\n\tSanity check AlphaFS <> System.IO {0}.", cntCheck);
         Console.WriteLine("\t\tFile.GetAccessControl().AreAccessRulesProtected: [{0}]", dsAlpha.AreAccessRulesProtected);
         Assert.AreEqual(dsSystem.AreAccessRulesProtected, dsAlpha.AreAccessRulesProtected);

         UnitTestConstants.StopWatcher(true);
         Console.WriteLine("\t\tFile.GetAccessControl().AreAuditRulesProtected: [{0}]", dsAlpha.AreAuditRulesProtected);
         Assert.AreEqual(dsSystem.AreAuditRulesProtected, dsAlpha.AreAuditRulesProtected);

         UnitTestConstants.StopWatcher(true);
         Console.WriteLine("\t\tFile.GetAccessControl().AreAccessRulesCanonical: [{0}]", dsAlpha.AreAccessRulesCanonical);
         Assert.AreEqual(dsSystem.AreAccessRulesCanonical, dsAlpha.AreAccessRulesCanonical);

         UnitTestConstants.StopWatcher(true);
         Console.WriteLine("\t\tFile.GetAccessControl().AreAuditRulesCanonical: [{0}]", dsAlpha.AreAuditRulesCanonical);
         Assert.AreEqual(dsSystem.AreAuditRulesCanonical, dsAlpha.AreAuditRulesCanonical);
      }

      #endregion // DumpAccessRules

      #region DumpAppendAllLines

      private static void DumpAppendAllLines(bool isLocal)
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

      private static void DumpCopy(bool isLocal)
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
               Console.WriteLine("\nCatch: [{0}]: The caller does not have the required permission.", expectedException);

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
               Console.WriteLine("\nCatch: [{0}]: sourceFileName was not found.", expectedException);
               
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
               Console.WriteLine("\nCatch: [{0}]: The path specified in sourceFileName or destFileName is invalid (for example, it is on an unmapped drive).", expectedException);
               
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
               Console.WriteLine("\nCatch: [{0}]: destFileName exists and overwrite is false.", expectedException);

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
               Console.WriteLine("\nCatch: [{0}]: A folder with the same name as the file exists.", expectedException);
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

      #region DumpCreate

      private static void DumpCreate(bool isLocal)
      {
         Console.WriteLine("\n=== TEST {0} ===", isLocal ? UnitTestConstants.Local : UnitTestConstants.Network);
         string tempFolder = Path.GetTempPath();
         string tempPath = Path.Combine(tempFolder, "File-Create-" + Path.GetRandomFileName());
         if (!isLocal) tempPath = Path.LocalToUnc(tempPath);

         using (FileStream fs = File.Create(tempPath))
         {
            int ten = UnitTestConstants.TenNumbers.Length;

            // According to NotePad++, creates a file type: "ANSI", which is reported as: "Unicode (UTF-8)".
            fs.Write(UnitTestConstants.StringToByteArray(UnitTestConstants.TenNumbers), 0, ten);

            long fileSize = fs.Length;
            bool isTen = fileSize == ten;

            Console.WriteLine("\nInput File Path: [{0}]", tempPath);
            Console.WriteLine("\n\tFilestream.Name   == [{0}]", fs.Name);
            Console.WriteLine("\n\tFilestream.Length == [{0}] (Should be True): [{1}]", Utils.UnitSizeToText(ten), isTen);

            Assert.IsTrue(File.Exists(tempPath), "File should exist.");
            Assert.IsTrue(isTen, "File should be [{0}] bytes in size.", ten);

            Assert.IsTrue(UnitTestConstants.Dump(fs, -14));
            Assert.IsTrue(UnitTestConstants.Dump(fs.SafeFileHandle, -9));
         }

         using (StreamReader stream = File.OpenText(tempPath))
         {
            Console.WriteLine("\n\n\tEncoding: [{0}]", stream.CurrentEncoding.EncodingName);

            string line;
            while (!string.IsNullOrWhiteSpace((line = stream.ReadLine())))
               Console.WriteLine("\tContent : [{0}]", line);
         }

         File.Delete(tempPath);
         Assert.IsFalse(File.Exists(tempPath), "Cleanup failed: File should have been removed.");
         Console.WriteLine();
      }

      #endregion // DumpCreate

      #region TestDelete

      private static void TestDelete(bool isLocal)
      {
         #region Setup

         Console.WriteLine("\n=== TEST {0} ===", isLocal ? UnitTestConstants.Local : UnitTestConstants.Network);
         string tempFolder = Path.GetTempPath();
         string tempPath = Path.Combine(tempFolder, "File.Delete-" + Path.GetRandomFileName());
         if (!isLocal) tempPath = Path.LocalToUnc(tempPath);

         string notePad = isLocal ? UnitTestConstants.NotepadExe : Path.LocalToUnc(UnitTestConstants.NotepadExe);

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

      #region DumpExists

      private static void DumpExists(bool isLocal)
      {
         Console.WriteLine("\n=== TEST {0} ===", isLocal ? UnitTestConstants.Local : UnitTestConstants.Network);
         string tempPath = Path.GetTempPath("File-Exists-" + Path.GetRandomFileName());
         if (!isLocal) tempPath = Path.LocalToUnc(tempPath);

         Console.WriteLine("\nInput File Path: [{0}]\n", tempPath);

         bool exists = File.Exists(tempPath);
         Console.WriteLine("\tFile.Exists() (Should be False): [{0}]", exists);
         Assert.IsFalse(exists, "File should not exist.");

         using (File.Create(tempPath)) { }

         exists = File.Exists(tempPath);
         Console.WriteLine("\n\tCreated file.");
         Console.WriteLine("\tFile.Exists() (Should be True): [{0}]", exists);
         Assert.IsTrue(exists, "File should exist.");

         File.Delete(tempPath, true);
         exists = File.Exists(tempPath);
         Assert.IsFalse(exists, "File should exist.");

         Console.WriteLine("\n");
      }

      #endregion // DumpExists

      #region DumpGetAccessControl

      private static void DumpGetAccessControl(bool isLocal)
      {
         Console.WriteLine("\n=== TEST {0} ===", isLocal ? UnitTestConstants.Local : UnitTestConstants.Network);

         string tempPath = Path.Combine(Path.GetTempPath(), "File.GetAccessControl()-" + Path.GetRandomFileName());
         if (!isLocal) tempPath = Path.LocalToUnc(tempPath);
         using (File.Create(tempPath)) { }

         bool foundRules = false;

         UnitTestConstants.StopWatcher(true);
         FileSecurity gac = File.GetAccessControl(tempPath);
         string report = UnitTestConstants.Reporter();

         AuthorizationRuleCollection accessRules = gac.GetAccessRules(true, true, typeof(NTAccount));
         FileSecurity sysIo = System.IO.File.GetAccessControl(tempPath);
         AuthorizationRuleCollection sysIoaccessRules = sysIo.GetAccessRules(true, true, typeof(NTAccount));

         Console.WriteLine("\nInput File Path: [{0}]", tempPath);
         Console.WriteLine("\n\tGetAccessControl() rules found: [{0}]\n\t\tSystem.IO rules found  : [{1}]\n{2}", accessRules.Count, sysIoaccessRules.Count, report);
         Assert.AreEqual(sysIoaccessRules.Count, accessRules.Count);

         foreach (FileSystemAccessRule far in accessRules)
         {
            UnitTestConstants.Dump(far, 17);
            DumpAccessRules(1, sysIo, gac);
            foundRules = true;
         }
         Assert.IsTrue(foundRules);

         File.Delete(tempPath, true);
         Assert.IsFalse(File.Exists(tempPath), "Cleanup failed: File should have been removed.");
         Console.WriteLine("\n");
      }

      #endregion // DumpGetAccessControl

      #region DumpGetXxxTimeXxx

      private static void DumpGetXxxTimeXxx(bool isLocal)
      {
         #region Setup

         Console.WriteLine("\n=== TEST {0} ===", isLocal ? UnitTestConstants.Local : UnitTestConstants.Network);
         string path = UnitTestConstants.NotepadExe;
         if (!isLocal) path = Path.LocalToUnc(path);

         Console.WriteLine("\nInput File Path: [{0}]\n", path);

         #endregion // Setup

         UnitTestConstants.StopWatcher(true);

         #region GetCreationTimeXxx

         DateTime actual = File.GetCreationTime(path);
         DateTime expected = System.IO.File.GetCreationTime(path);
         Console.WriteLine("\tGetCreationTime()     : [{0}]    System.IO: [{1}]", actual, expected);
         Assert.AreEqual(expected, actual, "AlphaFS != System.IO");

         actual = File.GetCreationTimeUtc(path);
         expected = System.IO.File.GetCreationTimeUtc(path);
         Console.WriteLine("\tGetCreationTimeUtc()  : [{0}]    System.IO: [{1}]\n", actual, expected);
         Assert.AreEqual(expected, actual, "AlphaFS != System.IO");

         #endregion // GetCreationTimeXxx

         #region GetLastAccessTimeXxx

         actual = File.GetLastAccessTime(path);
         expected = System.IO.File.GetLastAccessTime(path);
         Console.WriteLine("\tGetLastAccessTime()   : [{0}]    System.IO: [{1}]", actual, expected);
         Assert.AreEqual(expected, actual, "AlphaFS != System.IO");

         actual = File.GetLastAccessTimeUtc(path);
         expected = System.IO.File.GetLastAccessTimeUtc(path);
         Console.WriteLine("\tGetLastAccessTimeUtc(): [{0}]    System.IO: [{1}]\n", actual, expected);
         Assert.AreEqual(expected, actual, "AlphaFS != System.IO");

         #endregion // GetLastAccessTimeXxx

         #region GetLastWriteTimeXxx

         actual = File.GetLastWriteTime(path);
         expected = System.IO.File.GetLastWriteTime(path);
         Console.WriteLine("\tGetLastWriteTime()    : [{0}]    System.IO: [{1}]", actual, expected);
         Assert.AreEqual(expected, actual, "AlphaFS != System.IO");

         actual = File.GetLastWriteTimeUtc(path);
         expected = System.IO.File.GetLastWriteTimeUtc(path);
         Console.WriteLine("\tGetLastWriteTimeUtc() : [{0}]    System.IO: [{1}]\n", actual, expected);
         Assert.AreEqual(expected, actual, "AlphaFS != System.IO");

         #endregion // GetLastWriteTimeXxx


         #region GetChangeTimeXxx

         Console.WriteLine("\tGetChangeTime()       : [{0}]    System.IO: [N/A]", File.GetChangeTime(path));
         Console.WriteLine("\tGetChangeTimeUtc()    : [{0}]    System.IO: [N/A]", File.GetChangeTimeUtc(path));

         #endregion // GetChangeTimeXxx

         Console.WriteLine();
         Console.WriteLine(UnitTestConstants.Reporter());
         Console.WriteLine();

         #region Trigger GetChangeTimeXxx

         // We can not compare ChangeTime against .NET because it does not exist.
         // Creating a file and renaming it triggers ChangeTime, so test for that.

         path = Path.GetTempPath("File-GetChangeTimeXxx()-file-" + Path.GetRandomFileName());
         if (!isLocal) path = Path.LocalToUnc(path);

         FileInfo fi = new FileInfo(path);
         using (fi.Create()) { }
         string fileName = fi.Name;

         DateTime lastAccessTimeActual = File.GetLastAccessTime(path);
         DateTime lastAccessTimeUtcActual = File.GetLastAccessTimeUtc(path);

         DateTime changeTimeActual = File.GetChangeTime(path);
         DateTime changeTimeUtcActual = File.GetChangeTimeUtc(path);

         Console.WriteLine("\nTesting ChangeTime on a temp file.");
         Console.WriteLine("\nInput File Path: [{0}]\n", path);
         Console.WriteLine("\tGetChangeTime()       : [{0}]\t", changeTimeActual);
         Console.WriteLine("\tGetChangeTimeUtc()    : [{0}]\t", changeTimeUtcActual);

         fi.MoveTo(fi.FullName.Replace(fileName, fileName + "-Renamed"));

         // Pause for at least a second so that the difference in time can be seen.
         int sleep = new Random().Next(2000, 4000);
         Thread.Sleep(sleep);

         fi.MoveTo(fi.FullName.Replace(fileName + "-Renamed", fileName));

         DateTime lastAccessTimeExpected = File.GetLastAccessTime(path);
         DateTime lastAccessTimeUtcExpected = File.GetLastAccessTimeUtc(path);
         DateTime changeTimeExpected = File.GetChangeTime(path);
         DateTime changeTimeUtcExpected = File.GetChangeTimeUtc(path);

         Console.WriteLine("\nTrigger ChangeTime by renaming the file.");
         Console.WriteLine("For Unit Test, ChangeTime should differ approximately: [{0}] seconds.\n", sleep / 1000);
         Console.WriteLine("\tGetChangeTime()       : [{0}]\t", changeTimeExpected);
         Console.WriteLine("\tGetChangeTimeUtc()    : [{0}]\t\n", changeTimeUtcExpected);


         Assert.AreNotEqual(changeTimeActual, changeTimeExpected);
         Assert.AreNotEqual(changeTimeUtcActual, changeTimeUtcExpected);

         Assert.AreEqual(lastAccessTimeExpected, lastAccessTimeActual);
         Assert.AreEqual(lastAccessTimeUtcExpected, lastAccessTimeUtcActual);

         #endregion // Trigger GetChangeTimeXxx


         fi.Delete();
         fi.Refresh(); // Must Refresh() to get actual state.
         Assert.IsFalse(fi.Exists, "Cleanup failed: File should have been removed.");
         Console.WriteLine();
      }

      #endregion // DumpGetXxxTimeXxx

      #region DumpGetSize

      private static void DumpGetSize(bool isLocal)
      {
         #region Setup

         Console.WriteLine("\n=== TEST {0} ===", isLocal ? UnitTestConstants.Local : UnitTestConstants.Network);
         string tempPath = Path.GetTempPath("File-GetSize()-file-" + Path.GetRandomFileName());
         if (!isLocal) tempPath = Path.LocalToUnc(tempPath);

         Console.WriteLine("\nInput File Path: [{0}]\n", tempPath);

         int randomLines = new Random().Next(1000, 100000);

         // Create file with contents.
         FileInfo fi = new FileInfo(tempPath);
         using (StreamWriter sw = fi.CreateText())
            for (int i = 0; i < randomLines; i++)
               sw.WriteLine(UnitTestConstants.TextHelloWorld);


         long fileGetSize = File.GetSize(tempPath);
         long fileGetCompressedSize = File.GetCompressedSize(tempPath);
         long fiLength = fi.Length;

         Console.WriteLine("\tFile.GetSize()\t\t\t== [{0}] [{1} bytes]", Utils.UnitSizeToText(fileGetSize), fileGetSize);
         Console.WriteLine("\tFile.GetCompressedSize()\t== [{0}] [{1} bytes]", Utils.UnitSizeToText(fileGetCompressedSize), fileGetCompressedSize);

         Console.WriteLine("\tFileInfo().Length\t\t== [{0}] [{1} bytes]", Utils.UnitSizeToText(fiLength), fiLength);
         Console.WriteLine("\tFileInfo().Attributes\t\t== [{0}]", fi.Attributes);

         Assert.IsTrue((fi.Attributes & FileAttributes.Compressed) == 0, "File should be uncompressed.");
         Assert.IsTrue(fiLength == fileGetSize, "Uncompressed size should match.");
         Assert.IsTrue(fiLength == fileGetCompressedSize, "Uncompressed size should match.");

         #endregion // Setup

         #region Compress

         bool compressOk = false;
         string report;
         UnitTestConstants.StopWatcher(true);

         try
         {
            fi.Compress();
            report = UnitTestConstants.Reporter(true);
            compressOk = true;

            fileGetSize = File.GetSize(tempPath);
            fileGetCompressedSize = File.GetCompressedSize(tempPath);
         }
         catch (Exception ex)
         {
            Console.WriteLine("\n\tCaught (unexpected) {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
            report = UnitTestConstants.Reporter(true);
         }


         // FileInfo() must Refresh().
         Assert.IsTrue((fi.Attributes & FileAttributes.Compressed) == 0, "FileInfo() should not know it is compressed.");
         fi.Refresh();
         fiLength = fi.Length;
         Assert.IsTrue((fi.Attributes & FileAttributes.Compressed) != 0, "FileInfo() should know it is compressed.");


         Console.WriteLine("\n\n\tFile.Compress() (Should be True): [{0}]{1}\n", compressOk, report);

         Console.WriteLine("\tFile.GetSize()\t\t\t== [{0}] [{1} bytes]", Utils.UnitSizeToText(fileGetSize), fileGetSize);
         Console.WriteLine("\tFile.GetCompressedSize()\t== [{0}] [{1} bytes]", Utils.UnitSizeToText(fileGetCompressedSize), fileGetCompressedSize);

         Console.WriteLine("\tFileInfo().Length\t\t== [{0}] [{1} bytes]", Utils.UnitSizeToText(fiLength), fiLength);
         Console.WriteLine("\tFileInfo().Attributes\t\t== [{0}]", fi.Attributes);

         Assert.IsTrue(compressOk);

         Assert.IsTrue((fi.Attributes & FileAttributes.Compressed) != 0, "File should be compressed.");
         Assert.IsTrue(fiLength != fileGetCompressedSize, "FileInfo() size should not match compressed size.");
         Assert.IsTrue(fiLength == fileGetSize, "File size should match FileInfo() size.");

         #endregion // Compress

         #region Decompress

         bool decompressOk = false;
         UnitTestConstants.StopWatcher(true);

         try
         {
            File.Decompress(tempPath);
            report = UnitTestConstants.Reporter(true);
            decompressOk = true;

            fileGetSize = File.GetSize(tempPath);
            fileGetCompressedSize = File.GetCompressedSize(tempPath);

         }
         catch (Exception ex)
         {
            Console.WriteLine("\n\tCaught (unexpected) {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
            report = UnitTestConstants.Reporter(true);
         }


         // FileInfo() must Refresh().
         Assert.IsTrue((fi.Attributes & FileAttributes.Compressed) != 0, "FileInfo() should not know it is compressed.");
         fi.Refresh();
         fiLength = fi.Length;
         Assert.IsTrue((fi.Attributes & FileAttributes.Compressed) == 0, "FileInfo() should know it is compressed.");


         Console.WriteLine("\n\n\tFile.Decompress() (Should be True): [{0}]{1}\n", decompressOk, report);

         Console.WriteLine("\tFile.GetSize()\t\t\t== [{0}] [{1} bytes]", Utils.UnitSizeToText(fileGetSize), fileGetSize);
         Console.WriteLine("\tFile.GetCompressedSize()\t== [{0}] [{1} bytes]", Utils.UnitSizeToText(fileGetCompressedSize), fileGetCompressedSize);

         Console.WriteLine("\tFileInfo().Length\t\t== [{0}] [{1} bytes]", Utils.UnitSizeToText(fiLength), fiLength);
         Console.WriteLine("\tFileInfo().Attributes\t\t== [{0}]", fi.Attributes);

         Assert.IsTrue(decompressOk);

         Assert.IsTrue((fi.Attributes & FileAttributes.Compressed) == 0, "File should be uncompressed.");
         Assert.IsTrue(fiLength == fileGetSize, "Uncompressed size should match.");
         Assert.IsTrue(fiLength == fileGetCompressedSize, "Uncompressed size should match.");

         #endregion //Decompress

         fi.Delete();
         fi.Refresh(); // Must Refresh() to get actual state.
         Assert.IsFalse(fi.Exists, "Cleanup failed: File should have been removed.");
         Console.WriteLine();
      }

      #endregion // DumpGetSize

      #region DumpEnumerateHardlinks

      private static void DumpEnumerateHardlinks(bool isLocal)
      {
         Console.WriteLine("\n=== TEST {0} ===", isLocal ? UnitTestConstants.Local : UnitTestConstants.Network);
         string subDir = Directory.CreateDirectory(Path.GetTempPath("Hardlink-" + Path.GetRandomFileName())).FullName;
         string tempPath = Path.Combine(subDir, "File.EnumerateHardlinks()-" + Path.GetRandomFileName());
         if (!isLocal) tempPath = Path.LocalToUnc(tempPath);


         // Create original file with text content.
         File.WriteAllText(tempPath, UnitTestConstants.TextHelloWorld);
         Console.WriteLine("\nInput File Path: [{0}]\n\nContents: [{1}]", tempPath, File.ReadAllText(tempPath));


         // Create a random number of hardlinks to the original file.
         int numCreate = new Random().Next(1, 20);
         List<string> hardlinks = new List<string>();

         for (int i = 0; i < numCreate; i++)
         {
            string subdir2 = Directory.CreateDirectory(Path.Combine(subDir, Path.GetRandomFileName())).FullName;
            string newfile = Path.Combine(subdir2, "File.CreateHardlink()-" + Path.GetRandomFileName());
            if (!isLocal) newfile = Path.LocalToUnc(newfile);
            File.CreateHardlink(newfile, tempPath);

            hardlinks.Add(newfile);
         }

         numCreate++;  // +1 is for the original file.
         Console.WriteLine("\nCreated: [{0}] Hardlinks.\n", numCreate);

         if (isLocal)
         {
            int cnt = 0;
            UnitTestConstants.StopWatcher(true);

            foreach (string hardLink in File.EnumerateHardlinks(tempPath))
               Console.WriteLine("\t#{0:000}\tHardlink: [{1}]", ++cnt, hardLink);

            Console.WriteLine();
            Console.WriteLine(UnitTestConstants.Reporter());
            Assert.AreEqual(numCreate, cnt);
            Console.WriteLine();
         }
         else
            Console.WriteLine("\tEnumerating Hardlinks does not work with UNC paths.");


         using (FileStream stream = File.OpenRead(tempPath))
         {
            UnitTestConstants.StopWatcher(true);
            ByHandleFileInfo bhfi = File.GetFileInfoByHandle(stream.SafeFileHandle);
            string report = UnitTestConstants.Reporter();
            Assert.AreEqual(numCreate, (int)bhfi.NumberOfLinks);

            Console.WriteLine("\n\tByHandleFileInfo for Input Path, see property: NumberOfLinks");
            UnitTestConstants.Dump(bhfi, -18);
            Console.WriteLine("\n{0}", report);
         }


         Directory.Delete(subDir, true, true);
         Assert.IsFalse(Directory.Exists(subDir), "Cleanup failed: Directory should have been removed.");
         Console.WriteLine();
      }

      #endregion // DumpEnumerateHardlinks

      #region DumpMove

      private static void DumpMove(bool isLocal)
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
               Console.WriteLine("\nCatch: [{0}]: The caller does not have the required permission.", expectedException);

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
               Console.WriteLine("\nCatch: [{0}]: sourceFileName was not found.", expectedException);
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
               Console.WriteLine("\nCatch: [{0}]: The path specified in sourceFileName or destFileName is invalid (for example, it is on an unmapped drive).", expectedException);
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
               Console.WriteLine("\nCatch: [{0}]: The destination file already exists.", expectedException);

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
               Console.WriteLine("\nCatch: [{0}]: A folder with the same name as the file exists.", expectedException);
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

      private static void DumpGetSetAttributes(bool isLocal)
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

      #region DumpSetXxxTimeXxx

      private static void DumpSetXxxTimeXxx(bool isLocal)
      {
         #region Setup

         Console.WriteLine("\n=== TEST {0} ===", isLocal ? UnitTestConstants.Local : UnitTestConstants.Network);
         string path = Path.GetTempPath("File.SetCreationTime()-" + Path.GetRandomFileName());
         if (!isLocal) path = Path.LocalToUnc(path);

         Console.WriteLine("\nInput File Path: [{0}]\n", path);

         using (File.Create(path)) { }

         #endregion // Setup

         UnitTestConstants.StopWatcher(true);

         #region SetCreationTimeXxx

         //Thread.Sleep(new Random().Next(250, 500));
         int seed = (int)DateTime.Now.Ticks & 0x0000FFFF;
         DateTime creationTime = new DateTime(new Random(seed).Next(1971, 2071), new Random(seed).Next(1, 12), new Random(seed).Next(1, 28), new Random(seed).Next(0, 23), new Random(seed).Next(0, 59), new Random(seed).Next(0, 59));
         File.SetCreationTime(path, creationTime);
         DateTime actual = File.GetCreationTime(path);
         System.IO.File.SetCreationTime(path, creationTime);
         DateTime expected = System.IO.File.GetCreationTime(path);
         Console.WriteLine("\tSetCreationTime()     : [{0}]    System.IO: [{1}]", actual, expected);
         Assert.AreEqual(expected, actual, "AlphaFS != System.IO");


         //Thread.Sleep(new Random().Next(250, 500));
         seed += (int)DateTime.Now.Ticks & 0x0000FFFF;
         DateTime creationTimeUtc = new DateTime(new Random(seed).Next(1971, 2071), new Random(seed).Next(1, 12), new Random(seed).Next(1, 28), new Random(seed).Next(0, 23), new Random(seed).Next(0, 59), new Random(seed).Next(0, 59)).ToUniversalTime();
         File.SetCreationTimeUtc(path, creationTimeUtc);
         actual = File.GetCreationTimeUtc(path);
         System.IO.File.SetCreationTimeUtc(path, creationTimeUtc);
         expected = System.IO.File.GetCreationTimeUtc(path);
         Console.WriteLine("\tSetCreationTimeUtc()  : [{0}]    System.IO: [{1}]\n", actual, expected);
         Assert.AreEqual(expected, actual, "AlphaFS != System.IO");

         #endregion // SetCreationTimeXxx

         #region SetLastAccessTimeXxx

         //Thread.Sleep(new Random().Next(250, 500));
         seed += (int)DateTime.Now.Ticks & 0x0000FFFF;
         DateTime lastAccessTime = new DateTime(new Random(seed).Next(1971, 2071), new Random(seed).Next(1, 12), new Random(seed).Next(1, 28), new Random(seed).Next(0, 23), new Random(seed).Next(0, 59), new Random(seed).Next(0, 59));
         File.SetLastAccessTime(path, lastAccessTime);
         actual = File.GetLastAccessTime(path);
         System.IO.File.SetLastAccessTime(path, lastAccessTime);
         expected = System.IO.File.GetLastAccessTime(path);
         Console.WriteLine("\tSetLastAccessTime()   : [{0}]    System.IO: [{1}]", actual, expected);
         Assert.AreEqual(expected, actual, "AlphaFS != System.IO");


         //Thread.Sleep(new Random().Next(250, 500));
         seed += (int)DateTime.Now.Ticks & 0x0000FFFF;
         DateTime lastAccessTimeUtc = new DateTime(new Random(seed).Next(1971, 2071), new Random(seed).Next(1, 12), new Random(seed).Next(1, 28), new Random(seed).Next(0, 23), new Random(seed).Next(0, 59), new Random(seed).Next(0, 59)).ToUniversalTime();
         File.SetLastAccessTimeUtc(path, lastAccessTimeUtc);
         actual = File.GetLastAccessTimeUtc(path);
         System.IO.File.SetLastAccessTimeUtc(path, lastAccessTimeUtc);
         expected = System.IO.File.GetLastAccessTimeUtc(path);
         Console.WriteLine("\tSetLastAccessTimeUtc(): [{0}]    System.IO: [{1}]\n", actual, expected);
         Assert.AreEqual(expected, actual, "AlphaFS != System.IO");

         #endregion // SetLastAccessTimeXxx

         #region SetLastWriteTimeXxx

         //Thread.Sleep(new Random().Next(250, 500));
         seed += (int)DateTime.Now.Ticks & 0x0000FFFF;
         DateTime lastWriteTime = new DateTime(new Random(seed).Next(1971, 2071), new Random(seed).Next(1, 12), new Random(seed).Next(1, 28), new Random(seed).Next(0, 23), new Random(seed).Next(0, 59), new Random(seed).Next(0, 59));
         File.SetLastWriteTime(path, lastWriteTime);
         actual = File.GetLastWriteTime(path);
         System.IO.File.SetLastWriteTime(path, lastWriteTime);
         expected = System.IO.File.GetLastWriteTime(path);
         Console.WriteLine("\tSetLastWriteTime()    : [{0}]    System.IO: [{1}]", actual, expected);
         Assert.AreEqual(expected, actual, "AlphaFS != System.IO");


         //Thread.Sleep(new Random().Next(250, 500));
         seed += (int)DateTime.Now.Ticks & 0x0000FFFF;
         DateTime lastWriteTimeUtc = new DateTime(new Random(seed).Next(1971, 2071), new Random(seed).Next(1, 12), new Random(seed).Next(1, 28), new Random(seed).Next(0, 23), new Random(seed).Next(0, 59), new Random(seed).Next(0, 59)).ToUniversalTime();
         File.SetLastWriteTimeUtc(path, lastWriteTimeUtc);
         actual = File.GetLastWriteTimeUtc(path);
         System.IO.File.SetLastWriteTimeUtc(path, lastWriteTimeUtc);
         expected = System.IO.File.GetLastWriteTimeUtc(path);
         Console.WriteLine("\tSetLastWriteTimeUtc() : [{0}]    System.IO: [{1}]\n", actual, expected);
         Assert.AreEqual(expected, actual, "AlphaFS != System.IO");

         #endregion // SetLastWriteTimeXxx

         File.Delete(path);
         Assert.IsFalse(File.Exists(path), "Cleanup failed: File should have been removed.");
         Console.WriteLine();
      }

      #endregion // DumpSetXxxTimeXxx

      #region DumpSetTimestamps

      private static void DumpSetTimestamps(bool isLocal)
      {
         Console.WriteLine("\n=== TEST {0} ===", isLocal ? UnitTestConstants.Local : UnitTestConstants.Network);
         string path = Path.Combine(Path.GetTempPath(), "File.SetTimestamps()-" + Path.GetRandomFileName());
         if (!isLocal) path = Path.LocalToUnc(path);

         Console.WriteLine("\nInput Path: [{0}]", path);

         using (File.Create(path)) { }

         Thread.Sleep(new Random().Next(250, 500));
         int seed = (int)DateTime.Now.Ticks & 0x0000FFFF;
         DateTime creationTime = new DateTime(new Random(seed).Next(1971, 2071), new Random(seed).Next(1, 12), new Random(seed).Next(1, 28), new Random(seed).Next(0, 23), new Random(seed).Next(0, 59), new Random(seed).Next(0, 59));

         Thread.Sleep(new Random().Next(250, 500));
         seed = (int)DateTime.Now.Ticks & 0x0000FFFF;
         DateTime lastAccessTime = new DateTime(new Random(seed).Next(1971, 2071), new Random(seed).Next(1, 12), new Random(seed).Next(1, 28), new Random(seed).Next(0, 23), new Random(seed).Next(0, 59), new Random(seed).Next(0, 59));

         Thread.Sleep(new Random().Next(250, 500));
         seed += (int)DateTime.Now.Ticks & 0x0000FFFF;
         DateTime lastWriteTime = new DateTime(new Random(seed).Next(1971, 2071), new Random(seed).Next(1, 12), new Random(seed).Next(1, 28), new Random(seed).Next(0, 23), new Random(seed).Next(0, 59), new Random(seed).Next(0, 59));

         Console.WriteLine("\n");
         Console.WriteLine("\tCreationTime  : [{0} {1}]", creationTime.ToLongDateString(), creationTime.ToLongTimeString());
         Console.WriteLine("\tLastAccessTime: [{0} {1}]", lastAccessTime.ToLongDateString(), lastAccessTime.ToLongTimeString());
         Console.WriteLine("\tLastWriteTime : [{0} {1}]", lastWriteTime.ToLongDateString(), lastWriteTime.ToLongTimeString());
         Console.WriteLine("\n");

         File.SetTimestamps(path, creationTime, lastAccessTime, lastWriteTime);

         DateTime actual = File.GetCreationTime(path);
         DateTime expected = System.IO.File.GetCreationTime(path);
         Console.WriteLine("\t\tAlphaFS: [{0}]    System.IO: [{1}]", actual, expected);
         Assert.AreEqual(expected, actual, "AlphaFS != System.IO");

         actual = File.GetLastAccessTime(path);
         expected = System.IO.File.GetLastAccessTime(path);
         Console.WriteLine("\t\tAlphaFS: [{0}]    System.IO: [{1}]", actual, expected);
         Assert.AreEqual(expected, actual, "AlphaFS != System.IO");

         actual = File.GetLastWriteTime(path);
         expected = System.IO.File.GetLastWriteTime(path);
         Console.WriteLine("\t\tAlphaFS: [{0}]    System.IO: [{1}]", actual, expected);
         Assert.AreEqual(expected, actual, "AlphaFS != System.IO");




         creationTime = creationTime.ToUniversalTime();
         lastAccessTime = lastAccessTime.ToUniversalTime();
         lastWriteTime = lastWriteTime.ToUniversalTime();

         Console.WriteLine("\n");
         Console.WriteLine("\tCreationTimeUtc  : [{0} {1}]", creationTime.ToLongDateString(), creationTime.ToLongTimeString());
         Console.WriteLine("\tLastAccessTimeUtc: [{0} {1}]", lastAccessTime.ToLongDateString(), lastAccessTime.ToLongTimeString());
         Console.WriteLine("\tLastWriteTimeUtc : [{0} {1}]", lastWriteTime.ToLongDateString(), lastWriteTime.ToLongTimeString());
         Console.WriteLine("\n");

         File.SetTimestampsUtc(path, creationTime, lastAccessTime, lastWriteTime);

         actual = File.GetCreationTimeUtc(path);
         expected = System.IO.File.GetCreationTimeUtc(path);
         Console.WriteLine("\t\tAlphaFS: [{0}]    System.IO: [{1}]", actual, expected);
         Assert.AreEqual(expected, actual, "AlphaFS != System.IO");

         actual = File.GetLastAccessTimeUtc(path);
         expected = System.IO.File.GetLastAccessTimeUtc(path);
         Console.WriteLine("\t\tAlphaFS: [{0}]    System.IO: [{1}]", actual, expected);
         Assert.AreEqual(expected, actual, "AlphaFS != System.IO");

         actual = File.GetLastWriteTimeUtc(path);
         expected = System.IO.File.GetLastWriteTimeUtc(path);
         Console.WriteLine("\t\tAlphaFS: [{0}]    System.IO: [{1}]", actual, expected);
         Assert.AreEqual(expected, actual, "AlphaFS != System.IO");

         File.Delete(path);
         Assert.IsFalse(File.Exists(path));

         Console.WriteLine("\n");
      }

      #endregion // DumpSetTimestamps

      #region DumpTransferTimestamps

      private static void DumpTransferTimestamps(bool isLocal)
      {
         Console.WriteLine("\n=== TEST {0} ===", isLocal ? UnitTestConstants.Local : UnitTestConstants.Network);
         string path = Path.Combine(Path.GetTempPath(), "File.TransferTimestamps()-" + Path.GetRandomFileName());
         string path2 = Path.Combine(Path.GetTempPath(), "File.TransferTimestamps()-" + Path.GetRandomFileName());

         if (!isLocal)
         {
            path = Path.LocalToUnc(path);
            path2 = Path.LocalToUnc(path2);
         }

         Console.WriteLine("\nInput Path1: [{0}]", path);
         Console.WriteLine("\nInput Path2: [{0}]", path2);

         using (File.Create(path)) { }
         using (File.Create(path2)) { }

         try
         {
            Thread.Sleep(new Random().Next(250, 500));
            int seed = (int)DateTime.Now.Ticks & 0x0000FFFF;
            DateTime creationTime = new DateTime(new Random(seed).Next(1971, 2071), new Random(seed).Next(1, 12), new Random(seed).Next(1, 28), new Random(seed).Next(0, 23), new Random(seed).Next(0, 59), new Random(seed).Next(0, 59));

            Thread.Sleep(new Random().Next(250, 500));
            seed += (int)DateTime.Now.Ticks & 0x0000FFFF;
            DateTime lastAccessTime = new DateTime(new Random(seed).Next(1971, 2071), new Random(seed).Next(1, 12), new Random(seed).Next(1, 28), new Random(seed).Next(0, 23), new Random(seed).Next(0, 59), new Random(seed).Next(0, 59));

            Thread.Sleep(new Random().Next(250, 500));
            seed += (int)DateTime.Now.Ticks & 0x0000FFFF;
            DateTime lastWriteTime = new DateTime(new Random(seed).Next(1971, 2071), new Random(seed).Next(1, 12), new Random(seed).Next(1, 28), new Random(seed).Next(0, 23), new Random(seed).Next(0, 59), new Random(seed).Next(0, 59));

            File.SetTimestamps(path, creationTime, lastAccessTime, lastWriteTime);

            Console.WriteLine("\n\tPath1 dates and times:");
            Console.WriteLine("\t\tCreationTime  : [{0} {1}]", creationTime.ToLongDateString(), creationTime.ToLongTimeString());
            Console.WriteLine("\t\tLastAccessTime: [{0} {1}]", lastAccessTime.ToLongDateString(), lastAccessTime.ToLongTimeString());
            Console.WriteLine("\t\tLastWriteTime : [{0} {1}]", lastWriteTime.ToLongDateString(), lastWriteTime.ToLongTimeString());

            Console.WriteLine("\n\tPath2 current dates and times:");
            Console.WriteLine("\t\tCreationTime  : [{0} {1}]", File.GetCreationTime(path2).ToLongDateString(), File.GetCreationTime(path2).ToLongTimeString());
            Console.WriteLine("\t\tLastAccessTime: [{0} {1}]", File.GetLastAccessTime(path2).ToLongDateString(), File.GetLastAccessTime(path2).ToLongTimeString());
            Console.WriteLine("\t\tLastWriteTime : [{0} {1}]", File.GetLastWriteTime(path2).ToLongDateString(), File.GetLastWriteTime(path2).ToLongTimeString());

            File.TransferTimestamps(path, path2);

            Console.WriteLine("\n\tPath2 dates and times after TransferTimestamps():");
            Console.WriteLine("\t\tCreationTime  : [{0} {1}]", File.GetCreationTime(path2).ToLongDateString(), File.GetCreationTime(path2).ToLongTimeString());
            Console.WriteLine("\t\tLastAccessTime: [{0} {1}]", File.GetLastAccessTime(path2).ToLongDateString(), File.GetLastAccessTime(path2).ToLongTimeString());
            Console.WriteLine("\t\tLastWriteTime : [{0} {1}]", File.GetLastWriteTime(path2).ToLongDateString(), File.GetLastWriteTime(path2).ToLongTimeString());

            Assert.AreEqual(File.GetCreationTime(path), File.GetCreationTime(path2));
            Assert.AreEqual(File.GetLastAccessTime(path), File.GetLastAccessTime(path2));
            Assert.AreEqual(File.GetLastWriteTime(path), File.GetLastWriteTime(path2));
         }
         finally
         {
            File.Delete(path);
            File.Delete(path2);

            Assert.IsFalse(File.Exists(path), "Cleanup failed: File should have been removed.");
            Assert.IsFalse(File.Exists(path), "Cleanup failed: File should have been removed.");
         }

         Console.WriteLine("\n");
      }

      #endregion // DumpTransferTimestamps

      #region DumpReadAllLines

      private static void DumpReadAllLines(bool isLocal)
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

      private static void DumpReadWriteAllBytes(bool isLocal)
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

      #region Create file with trailing dot/space

      private static void DumpFileTrailingDotSpace(bool isLocal)
      {
         Console.WriteLine("\n=== TEST {0} ===\n", isLocal ? UnitTestConstants.Local : UnitTestConstants.Network);
         const string characterDot = ".";
         const string characterSpace = " ";
         string random = Path.GetRandomFileName();
         string tempPathDot = Path.GetTempPath("File.Create()-" + random + "-file-with-dot-" + characterDot);
         string tempPathSpace = Path.GetTempPath("File.Create()-" + random + "-file-with-space-" + characterSpace);
         if (!isLocal) tempPathDot = Path.LocalToUnc(tempPathDot);
         if (!isLocal) tempPathSpace = Path.LocalToUnc(tempPathSpace);

         Console.WriteLine("Input File Path (with dot)  : [{0}]", tempPathDot);
         Console.WriteLine("Input File Path (with space): [{0}]", tempPathSpace);

         Assert.IsTrue(tempPathDot.EndsWith(characterDot), "Path should have a trailing dot.");
         Assert.IsTrue(tempPathSpace.EndsWith(characterSpace), "Path should have a trailing space.");

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

         #region File() Class

         UnitTestConstants.StopWatcher(true);

         #region TrailingDot

         #region System.IO

         // tempPathDot contains a trailing dot but gets stripped on path normalization.
         // System.IO handles the file without the trailing dot. Therefore, the file exists.
         // AlphaFS has the same behaviour as .NET for default methods.

         using (FileStream fs = System.IO.File.Create(tempPathDot)) { fs.WriteByte(1); }
         Assert.IsTrue(System.IO.File.Exists(tempPathDot), "File should exist.");
         Assert.IsTrue(File.Exists(tempPathDot), "File should be visible to AlphaFS.");
         Assert.IsFalse(File.Exists(tempPathDot, PathFormat.FullPath), "File should be invisible to AlphaFS.");

         using (StreamWriter sw = System.IO.File.AppendText(tempPathDot))
            sw.WriteLine(UnitTestConstants.TextHelloWorld);

         string lineSysIo;
         using (StreamReader sr = System.IO.File.OpenText(tempPathDot))
            lineSysIo = sr.ReadToEnd();

         System.IO.File.Delete(tempPathDot);
         Assert.IsFalse(System.IO.File.Exists(tempPathDot), "File should not exist.");

         #endregion // System.IO

         #region AlphaFS

         using (FileStream fs = File.Create(tempPathDot, PathFormat.FullPath)) { fs.WriteByte(1); } // Create file without path normalization.
         Assert.IsTrue(File.Exists(tempPathDot, PathFormat.FullPath), "File should exist and visible to AlphaFS.");
         Assert.IsFalse(System.IO.File.Exists(tempPathDot), "File should be invisible to System.IO.");

         using (StreamWriter sw = File.AppendText(tempPathDot, PathFormat.FullPath))
            sw.WriteLine(UnitTestConstants.TextHelloWorld);

         string lineAlphaFs;
         using (StreamReader sr = File.OpenText(tempPathDot, PathFormat.FullPath))
            lineAlphaFs = sr.ReadToEnd();

         File.Delete(tempPathDot, true, PathFormat.FullPath); // Delete file without path normalization.
         Assert.IsFalse(File.Exists(tempPathDot, PathFormat.FullPath), "File should not exist.");

         #endregion // AlphaFS

         Assert.AreEqual(lineSysIo, lineAlphaFs);

         #endregion // TrailingDot

         #region TrailingSpace

         #region System.IO

         // tempPathSpace contains a trailing space but gets stripped on path normalization.
         // System.IO handles the file without the trailing space. Therefore, the file exists.
         // AlphaFS has the same behaviour as .NET for default methods.

         using (FileStream fs = System.IO.File.Create(tempPathSpace)) { fs.WriteByte(1); }
         Assert.IsTrue(System.IO.File.Exists(tempPathSpace), "File should exist.");
         Assert.IsTrue(File.Exists(tempPathSpace), "File should be visible to AlphaFS.");
         Assert.IsFalse(File.Exists(tempPathSpace, PathFormat.FullPath), "File should be invisible to AlphaFS.");

         using (StreamWriter sw = System.IO.File.AppendText(tempPathSpace))
            sw.WriteLine(UnitTestConstants.TextHelloWorld);

         using (StreamReader sr = System.IO.File.OpenText(tempPathSpace))
            lineSysIo = sr.ReadToEnd();

         System.IO.File.Delete(tempPathSpace);
         Assert.IsFalse(System.IO.File.Exists(tempPathSpace), "File should not exist.");

         #endregion // System.IO

         #region AlphaFS

         using (FileStream fs = File.Create(tempPathSpace, PathFormat.FullPath)) { fs.WriteByte(1); } // Create file without path normalization.
         Assert.IsTrue(File.Exists(tempPathSpace, PathFormat.FullPath), "File should exist and visible to AlphaFS.");
         Assert.IsFalse(System.IO.File.Exists(tempPathSpace), "File should be invisible to System.IO.");

         using (StreamWriter sw = File.AppendText(tempPathSpace, PathFormat.FullPath))
            sw.WriteLine(UnitTestConstants.TextHelloWorld);

         using (StreamReader sr = File.OpenText(tempPathSpace, PathFormat.FullPath))
            lineAlphaFs = sr.ReadToEnd();

         File.Delete(tempPathSpace, true, PathFormat.FullPath); // Delete file without path normalization.
         Assert.IsFalse(File.Exists(tempPathSpace, PathFormat.FullPath), "File should not exist.");

         #endregion // AlphaFS

         Assert.AreEqual(lineSysIo, lineAlphaFs);

         #endregion // TrailingSpace

         Console.WriteLine("\tClass File(){0}", UnitTestConstants.Reporter());

         #endregion // File() Class

         #region FileInfo() Class

         UnitTestConstants.StopWatcher(true);

         #region TrailingDot

         #region System.IO

         // tempPathDot contains a trailing dot but gets stripped on path normalization.
         // System.IO handles the file without the trailing dot. Therefore, the file exists.
         // AlphaFS has the same behaviour as .NET for default methods.

         System.IO.FileInfo sysIoFi = new System.IO.FileInfo(tempPathDot);
         Assert.IsTrue(sysIoFi.Name.EndsWith(characterDot), "Path should have a trailing dot.");

         using (FileStream fs = sysIoFi.Create())
         {
            fs.WriteByte(100);

            Assert.IsTrue(sysIoFi.Exists, "File should exist.");
            Assert.IsTrue(File.Exists(tempPathDot), "File should be visible to AlphaFS.");
            Assert.IsFalse(File.Exists(tempPathDot, PathFormat.FullPath), "File should be invisible to AlphaFS.");
         }

         using (StreamWriter sw = sysIoFi.AppendText())
            sw.WriteLine(UnitTestConstants.TextHelloWorld);

         using (StreamReader sr = System.IO.File.OpenText(tempPathDot))
            lineSysIo = sr.ReadToEnd();

         sysIoFi.Delete();
         Assert.IsFalse(System.IO.File.Exists(tempPathDot), "File should not exist.");

         #endregion // System.IO

         #region AlphaFS

         FileInfo alphaFsFi = new FileInfo(tempPathDot, PathFormat.FullPath);
         Assert.IsTrue(alphaFsFi.Name.EndsWith(characterDot), "Path should have a trailing dot.");

         using (FileStream fs = alphaFsFi.Create())
         {
            fs.WriteByte(100);

            Assert.IsTrue(alphaFsFi.Exists, "File should exist.");
            Assert.IsTrue(File.Exists(tempPathDot, PathFormat.FullPath), "File should be visible to AlphaFS.");
            Assert.IsFalse(File.Exists(tempPathDot), "File should be invisible to AlphaFS.");
         }

         using (StreamWriter sw = alphaFsFi.AppendText())
            sw.WriteLine(UnitTestConstants.TextHelloWorld);

         using (StreamReader sr = File.OpenText(tempPathDot, PathFormat.FullPath))
            lineAlphaFs = sr.ReadToEnd();

         alphaFsFi.Delete();
         alphaFsFi.Refresh(); // Must Refresh() to get actual state.
         Assert.IsFalse(File.Exists(tempPathDot, PathFormat.FullPath), "File should not exist.");

         #endregion // AlphaFS

         Assert.AreEqual(lineSysIo, lineAlphaFs);

         #endregion // TrailingDot

         #region TrailingSpace

         #region System.IO

         // tempPathSpace contains a trailing space but gets stripped on path normalization.
         // System.IO handles the file without the trailing space. Therefore, the file exists.
         // AlphaFS has the same behaviour as .NET for default methods.

         sysIoFi = new System.IO.FileInfo(tempPathSpace);
         Assert.IsTrue(sysIoFi.Name.EndsWith(characterSpace), "Path should have a trailing space.");

         using (FileStream fs = sysIoFi.Create())
         {
            fs.WriteByte(100);

            Assert.IsTrue(sysIoFi.Exists, "File should exist.");
            Assert.IsTrue(File.Exists(tempPathSpace), "File should be visible to AlphaFS.");
            Assert.IsFalse(File.Exists(tempPathSpace, PathFormat.FullPath), "File should be invisible to AlphaFS.");
         }

         using (StreamWriter sw = sysIoFi.AppendText())
            sw.WriteLine(UnitTestConstants.TextHelloWorld);

         using (StreamReader sr = System.IO.File.OpenText(tempPathSpace))
            lineSysIo = sr.ReadToEnd();

         sysIoFi.Delete();
         Assert.IsFalse(System.IO.File.Exists(tempPathSpace), "File should not exist.");

         #endregion // System.IO

         #region AlphaFS

         alphaFsFi = new FileInfo(tempPathSpace, PathFormat.FullPath);
         Assert.IsTrue(alphaFsFi.Name.EndsWith(characterSpace), "Path should have a trailing space.");

         using (FileStream fs = alphaFsFi.Create())
         {
            fs.WriteByte(100);

            Assert.IsTrue(alphaFsFi.Exists, "File should exist.");
            Assert.IsTrue(File.Exists(tempPathSpace, PathFormat.FullPath), "File should be visible to AlphaFS.");
            Assert.IsFalse(File.Exists(tempPathSpace), "File should be invisible to AlphaFS.");
         }

         using (StreamWriter sw = alphaFsFi.AppendText())
            sw.WriteLine(UnitTestConstants.TextHelloWorld);

         using (StreamReader sr = File.OpenText(tempPathSpace, PathFormat.FullPath))
            lineAlphaFs = sr.ReadToEnd();

         alphaFsFi.Delete();
         alphaFsFi.Refresh(); // Must Refresh() to get actual state.
         Assert.IsFalse(File.Exists(tempPathSpace, PathFormat.FullPath), "File should not exist.");

         #endregion // AlphaFS

         Assert.AreEqual(lineSysIo, lineAlphaFs);

         #endregion // TrailingSpace

         Console.WriteLine("\tClass FileInfo(){0}", UnitTestConstants.Reporter());

         #endregion // FileInfo() Class

         Console.WriteLine();
      }

      #endregion // Create file with trailing dot/space


      #endregion // Unit Tests

      #region .NET

      #region AppendAllLines

      [TestMethod]
      public void AppendAllLines()
      {
         Console.WriteLine("File.AppendAllLines()");

         DumpAppendAllLines(true);
         DumpAppendAllLines(false);
      }

      #endregion // AppendAllLines

      #region AppendAllText

      [TestMethod]
      public void AppendAllText()
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
      public void AppendText()
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
      public void Copy()
      {
         Console.WriteLine("File.Copy()");

         DumpCopy(true);
         DumpCopy(false);
      }

      #endregion // Copy

      #region Create

      [TestMethod]
      public void Create()
      {
         Console.WriteLine("File.Create()");

         DumpCreate(true);
         DumpCreate(false);
      }

      [TestMethod]
      public void AlphaFS___CreateWithFileSecurity()
      {
         Console.WriteLine("File.Create()");

          if (!UnitTestConstants.IsAdmin())
              Assert.Inconclusive();

         string pathExpected = Path.GetTempPath("AlphaFS CreateWithFileSecurityExpected");
         string pathActual = Path.GetTempPath("AlphaFS CreateWithFileSecurityActual");

         File.Delete(pathExpected);
         File.Delete(pathActual);

         FileSecurity expectedFileSecurity = new FileSecurity();
         expectedFileSecurity.AddAccessRule(new FileSystemAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), FileSystemRights.FullControl, AccessControlType.Allow));
         expectedFileSecurity.AddAuditRule(new FileSystemAuditRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), FileSystemRights.Read, AuditFlags.Success));

         using (new Alphaleonis.Win32.Security.PrivilegeEnabler(Alphaleonis.Win32.Security.Privilege.Security))
         {
            using (FileStream s1 = System.IO.File.Create(pathExpected, 4096, FileOptions.None, expectedFileSecurity))
            using (FileStream s2 = File.Create(pathActual, 4096, FileOptions.None, expectedFileSecurity))
            {
            }
         }

         string expectedFileSecuritySddl = System.IO.File.GetAccessControl(pathExpected).GetSecurityDescriptorSddlForm(AccessControlSections.All);
         string actualFileSecuritySddl = System.IO.File.GetAccessControl(pathActual).GetSecurityDescriptorSddlForm(AccessControlSections.All);

         Assert.AreEqual(expectedFileSecuritySddl, actualFileSecuritySddl);

         File.Delete(pathExpected, true);
         File.Delete(pathActual, true);
      }

      #endregion // Create

      #region CreateText

      [TestMethod]
      public void CreateText()
      {
         Console.WriteLine("File.CreateText()");

         AppendText();
      }

      #endregion // CreateText

      #region Decrypt

      [TestMethod]
      public void Decrypt()
      {
         Console.WriteLine("File.Decrypt()");
         Console.WriteLine("\nPlease see unit test: Encrypt()");
      }

      #endregion // Decrypt

      #region Delete

      [TestMethod]
      public void Delete()
      {
         Console.WriteLine("File.Delete()");

         TestDelete(true);
         TestDelete(false);
      }

      #endregion // Delete

      #region Encrypt

      [TestMethod]
      public void Encrypt()
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

      #region Exists

      [TestMethod]
      public void Exists()
      {
         Console.WriteLine("File.Exists()");

         DumpExists(true);
         DumpExists(false);
      }

      #endregion // Exists

      #region GetAccessControl

      [TestMethod]
      public void GetAccessControl()
      {
         Console.WriteLine("File.GetAccessControl()");

         DumpGetAccessControl(true);
         DumpGetAccessControl(false);
      }

      #endregion // GetAccessControl

      #region GetAttributes

      [TestMethod]
      public void GetAttributes()
      {
         Console.WriteLine("File.GetAttributes()");

         DumpGetSetAttributes(true);
         DumpGetSetAttributes(false);
      }

      #endregion // GetAttributes

      #region GetCreationTime

      [TestMethod]
      public void GetCreationTime()
      {
         Console.WriteLine("File.GetXxxTimeXxx()");

         DumpGetXxxTimeXxx(true);
         DumpGetXxxTimeXxx(false);
      }

      #endregion // GetCreationTime

      #region GetCreationTimeUtc

      [TestMethod]
      public void GetCreationTimeUtc()
      {
         Console.WriteLine("File.GetCreationTimeUtc()");
         Console.WriteLine("\nPlease see unit test: GetCreationTime()");
      }

      #endregion // GetCreationTimeUtc

      #region GetLastAccessTime

      [TestMethod]
      public void GetLastAccessTime()
      {
         Console.WriteLine("File.GetLastAccessTime()");
         Console.WriteLine("\nPlease see unit test: GetCreationTime()");
      }

      #endregion // GetLastAccessTime

      #region GetLastAccessTimeUtc

      [TestMethod]
      public void GetLastAccessTimeUtc()
      {
         Console.WriteLine("File.GetLastAccessTimeUtc()");
         Console.WriteLine("\nPlease see unit test: GetCreationTime()");
      }

      #endregion // GetLastAccessTimeUtc

      #region GetLastWriteTime

      [TestMethod]
      public void GetLastWriteTime()
      {
         Console.WriteLine("File.GetLastWriteTime()");
         Console.WriteLine("\nPlease see unit test: GetCreationTime()");
      }

      #endregion // GetLastWriteTime

      #region GetLastWriteTimeUtc

      [TestMethod]
      public void GetLastWriteTimeUtc()
      {
         Console.WriteLine("File.GetLastWriteTimeUtc()");
         Console.WriteLine("\nPlease see unit test: GetCreationTime()");
      }

      #endregion // GetLastWriteTimeUtc

      #region Move

      [TestMethod]
      public void Move()
      {
         Console.WriteLine("File.Move()");

         DumpMove(true);
         DumpMove(false);
      }

      #endregion // Move

      #region Open

      [TestMethod]
      public void Open()
      {
         Console.WriteLine("File.Open()\n");
         string path = Path.GetTempPath("File.Open()-" + Path.GetRandomFileName());

         using (FileStream fs = File.Create(path))
         {
            // Convert 10000 character string to byte array.
            byte[] text = Encoding.UTF8.GetBytes(new string('X', 10000));
            fs.Write(text, 0, text.Length);
         }

         bool fileExists = File.Exists(path);

         // Open the stream and read it back.
         using (FileStream fs = File.Open(path, FileMode.Open))
         {
            byte[] b = new byte[1024];
            UTF8Encoding temp = new UTF8Encoding(true);

            while (fs.Read(b, 0, b.Length) > 0)
               Console.WriteLine(temp.GetString(b));

            bool exception = false;
            try
            {
               Console.WriteLine("\n\nOpening the file twice is disallowed.");

               // Try to get another handle to the same file.
               using (FileStream fs2 = File.Open(path, FileMode.Open)) { }
            }
            catch (Exception ex)
            {
               exception = true;
               Console.WriteLine("\n\tCaught (unexpected) {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
            }
            Console.WriteLine("\n\tCaught IOException (Should be True): [{0}]", exception);
            Assert.IsTrue(exception, "IOException should have been caught.");
         }

         File.Delete(path, true);
         Assert.IsFalse(File.Exists(path), "Cleanup failed: File should have been removed.");

         Assert.IsTrue(fileExists);
      }

      #endregion // Open

      #region OpenWrite

      [TestMethod]
      public void OpenWrite()
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
      public void ReadAllBytes()
      {
         Console.WriteLine("File.ReadAllBytes()");

         DumpReadWriteAllBytes(true);
         DumpReadWriteAllBytes(false);
      }

      #endregion // WriteAllBytes

      #region ReadAllLines

      [TestMethod]
      public void ReadAllLines()
      {
         Console.WriteLine("File.ReadAllLines()");

         DumpReadAllLines(true);
         DumpReadAllLines(false);
      }

      #endregion // ReadAllLines

      #region ReadAllText

      [TestMethod]
      public void ReadAllText()
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
      public void ReadLines()
      {
         Console.WriteLine("File.ReadLines()");

         ReadAllLines();
      }

      #endregion // ReadLines

      #region SetAccessControl

      [TestMethod]
      public void SetAccessControl()
      {
         Console.WriteLine("File.SetAccessControl()");

         if (!UnitTestConstants.IsAdmin())
             Assert.Inconclusive();

         string path = UnitTestConstants.SysDrive + @"\AlphaFile-" + Path.GetRandomFileName();
         string pathAlpha = path;

         Console.WriteLine("\n\tFile: [{0}]", path);

         try
         {
            using (File.Create(pathAlpha))
            {
            }

            // Initial read.
            Console.WriteLine("\n\tInitial read.");
            FileSecurity dsSystem = System.IO.File.GetAccessControl(path, AccessControlSections.Access);
            FileSecurity dsAlpha = File.GetAccessControl(pathAlpha, AccessControlSections.Access);
            AuthorizationRuleCollection accessRulesSystem = dsSystem.GetAccessRules(true, true, typeof(NTAccount));
            AuthorizationRuleCollection accessRulesAlpha = dsAlpha.GetAccessRules(true, true, typeof(NTAccount));
            Console.WriteLine("\t    System.IO.File.GetAccessControl() rules found: [{0}]", accessRulesSystem.Count);
            Console.WriteLine("\t\t\t   File.GetAccessControl() rules found: [{0}]", accessRulesAlpha.Count);
            Assert.AreEqual(accessRulesSystem.Count, accessRulesAlpha.Count);

            // Sanity check.
            DumpAccessRules(1, dsSystem, dsAlpha);

            // Remove inherited properties.
            // Passing true for first parameter protects the new permission from inheritance, and second parameter removes the existing inherited permissions 
            Console.WriteLine("\n\tRemove inherited properties and persist it.");
            dsAlpha.SetAccessRuleProtection(true, false);
            File.SetAccessControl(pathAlpha, dsAlpha, AccessControlSections.Access);

            // Re-read, using instance methods.
            System.IO.FileInfo fiSystem = new System.IO.FileInfo(Path.LocalToUnc(path));
            FileInfo fiAlpha = new FileInfo(Path.LocalToUnc(path));

            dsSystem = fiSystem.GetAccessControl(AccessControlSections.Access);
            dsAlpha = fiAlpha.GetAccessControl(AccessControlSections.Access);

            // Sanity check.
            DumpAccessRules(2, dsSystem, dsAlpha);

            // Restore inherited properties.
            Console.WriteLine("\n\tRestore inherited properties and persist it.");
            dsAlpha.SetAccessRuleProtection(false, true);
            File.SetAccessControl(pathAlpha, dsAlpha, AccessControlSections.Access);

            // Re-read.
            dsSystem = System.IO.File.GetAccessControl(path, AccessControlSections.Access);
            dsAlpha = File.GetAccessControl(pathAlpha, AccessControlSections.Access);

            // Sanity check.
            DumpAccessRules(3, dsSystem, dsAlpha);

            fiSystem.Delete();
            fiSystem.Refresh(); // Must Refresh() to get actual state.

            fiAlpha.Delete();
            fiAlpha.Refresh(); // Must Refresh() to get actual state.
            Assert.IsFalse(fiAlpha.Exists);
         }
         catch (Exception ex)
         {
            Console.WriteLine("\n\tCaught (unexpected) {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
         }
      }

      #endregion // SetAccessControl

      #region SetAttributes

      [TestMethod]
      public void SetAttributes()
      {
         Console.WriteLine("File.SetAttributes()");
         Console.WriteLine("\nPlease see unit test: GetAttributes()");
      }

      #endregion // SetAttributes

      #region SetCreationTime

      [TestMethod]
      public void SetCreationTime()
      {
         Console.WriteLine("File.SetXxxTimeXxx()");

         DumpSetXxxTimeXxx(true);
         DumpSetXxxTimeXxx(false);
      }

      #endregion // SetCreationTime

      #region SetCreationTimeUtc

      [TestMethod]
      public void SetCreationTimeUtc()
      {
         Console.WriteLine("File.SetCreationTimeUtc()");
         Console.WriteLine("\nPlease see unit test: SetCreationTime()");
      }

      #endregion // SetCreationTimeUtc

      #region SetLastAccessTime

      [TestMethod]
      public void SetLastAccessTime()
      {
         Console.WriteLine("File.SetLastAccessTime()");
         Console.WriteLine("\nPlease see unit test: SetCreationTime()");
      }

      #endregion // SetLastAccessTime

      #region SetLastAccessTimeUtc

      [TestMethod]
      public void SetLastAccessTimeUtc()
      {
         Console.WriteLine("File.SetLastAccessTimeUtc()");
         Console.WriteLine("\nPlease see unit test: SetCreationTime()");
      }

      #endregion // SetLastAccessTimeUtc

      #region SetLastWriteTime

      [TestMethod]
      public void SetLastWriteTime()
      {
         Console.WriteLine("File.SetLastWriteTime()");
         Console.WriteLine("\nPlease see unit test: SetCreationTime()");
      }

      #endregion // SetLastWriteTime

      #region SetLastWriteTimeUtc

      [TestMethod]
      public void SetLastWriteTimeUtc()
      {
         Console.WriteLine("File.SetLastWriteTimeUtc()");
         Console.WriteLine("\nPlease see unit test: SetCreationTime()");
      }

      #endregion // SetLastWriteTimeUtc

      #region WriteAllBytes

      [TestMethod]
      public void WriteAllBytes()
      {
         Console.WriteLine("File.WriteAllBytes()");

         ReadAllBytes();
      }

      #endregion // WriteAllBytes

      #region WriteAllLines

      [TestMethod]
      public void WriteAllLines()
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
      public void WriteAllText()
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

      #region AlphaFS

      #region Compress

      [TestMethod]
      public void AlphaFS_Compress()
      {
         Console.WriteLine("File.Compress()");
         Console.WriteLine("\nPlease see unit test: AlphaFS_GetSize()");
      }

      #endregion // Compress

      #region CreateHardlink

      [TestMethod]
      public void AlphaFS_CreateHardlink()
      {
         Console.WriteLine("File.CreateHardlink()");

         DumpEnumerateHardlinks(true);
         DumpEnumerateHardlinks(false);
      }

      #endregion // CreateHardlink

      #region Decompress

      [TestMethod]
      public void AlphaFS_Decompress()
      {
         Console.WriteLine("File.Decompress()");
         Console.WriteLine("\nPlease see unit test: AlphaFS_GetSize()");
      }

      #endregion // Compress/Decompress

      #region EnumerateHardlinks

      [TestMethod]
      public void AlphaFS_EnumerateHardlinks()
      {
         Console.WriteLine("File.EnumerateHardlinks()");
         Console.WriteLine("\nPlease see unit test: AlphaFS_CreateHardlink()");
      }

      #endregion // EnumerateHardlinks

      #region EnumerateAlternateDataStreams

      [TestMethod]
      public void AlphaFS_EnumerateAlternateDataStreams()
      {
         Console.WriteLine("File.EnumerateAlternateDataStreams()");
         Console.WriteLine("\nPlease see unit test: Filesystem_Class_AlternateDataStreamInfo()");
      }

      #endregion // EnumerateAlternateDataStreams

      #region GetChangeTime

      [TestMethod]
      public void AlphaFS_GetChangeTime()
      {
         Console.WriteLine("File.GetChangeTime()");
         Console.WriteLine("\nPlease see unit test: GetCreationTime()");
      }

      #endregion // GetChangeTime

      #region GetCompressedSize

      [TestMethod]
      public void AlphaFS_GetCompressedSize()
      {
         Console.WriteLine("File.GetCompressedSize()");
         Console.WriteLine("\nPlease see unit test: AlphaFS_Compress()");
      }

      #endregion // GetCompressedSize

      #region GetEncryptionStatus

      [TestMethod]
      public void AlphaFS_GetEncryptionStatus()
      {
         Console.WriteLine("File.GetEncryptionStatus()");
         Console.WriteLine("\nPlease see unit test: Encrypt()");
      }

      #endregion // GetEncryptionStatus

      #region GetFileSystemEntry

      [TestMethod]
      public void AlphaFS_GetFileSystemEntry()
      {
         Console.WriteLine("File.GetFileSystemEntry()");
         Console.WriteLine("\nPlease see unit test: Filesystem_Class_FileSystemEntryInfo()");
      }

      #endregion // GetFileSystemEntry

      #region GetFileInfoByHandle

      [TestMethod]
      public void AlphaFS_GetFileInfoByHandle()
      {
         Console.WriteLine("File.GetFileInfoByHandle()");
         Console.WriteLine("\nPlease see unit test: Filesystem_Class_ByHandleFileInfo()");
      }

      #endregion // GetFileInfoByHandle

      #region GetLinkTargetInfo

      [TestMethod]
      public void AlphaFS_GetLinkTargetInfo()
      {
         Console.WriteLine("File.GetLinkTargetInfo()");
         Console.WriteLine("\nPlease see unit test: Volume.SetVolumeMountPoint()");
      }

      #endregion // GetLinkTargetInfo

      #region GetSize

      [TestMethod]
      public void AlphaFS_GetSize()
      {
         Console.WriteLine("File.GetSize()");

         DumpGetSize(true);
         DumpGetSize(false);
      }

      #endregion // GetSize
      
      #region SetTimestamps

      [TestMethod]
      public void AlphaFS_SetTimestamps()
      {
         Console.WriteLine("File.SetTimestamps()");

         DumpSetTimestamps(true);
         DumpSetTimestamps(false);
      }

      #endregion // SetTimestamps

      #region TransferTimestamps

      [TestMethod]
      public void AlphaFS_TransferTimestamps()
      {
         Console.WriteLine("File.TransferTimestamps()");

         DumpTransferTimestamps(true);
         DumpTransferTimestamps(false);
      }

      #endregion // TransferTimestamps

      #region AlphaFS___FileTrailingDotSpace

      [TestMethod]
      public void AlphaFS___FileTrailingDotSpace()
      {
         Console.WriteLine(".NET does not support the creation/manipulation of files with a trailing dot or space.");
         Console.WriteLine("These will be stripped due to path normalization.");

         Console.WriteLine("\nThe AlphaFS File() class contains overloaded methods which have the");
         Console.WriteLine("isFullPath parameter that enables you to bypass this .NET limitation.\n");

         DumpFileTrailingDotSpace(true);
         DumpFileTrailingDotSpace(false);
      }

      #endregion // AlphaFS___FileTrailingDotSpace

      #endregion // AlphaFS
   }
}
