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
using System.Diagnostics;
using System.Globalization;
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
using FileSystemInfo = Alphaleonis.Win32.Filesystem.FileSystemInfo;
using OperatingSystem = Alphaleonis.Win32.OperatingSystem;
using Path = Alphaleonis.Win32.Filesystem.Path;

namespace AlphaFS.UnitTest
{
   /// <summary>This is a test class for Directory and is intended to contain all Directory class Unit Tests.</summary>
   [TestClass]
   public class DirectoryTest
   {
      #region Unit Test Helpers

      #region Fields

      private readonly string LocalHost = Environment.MachineName;
      private readonly string LocalHostShare = Environment.SystemDirectory;
      private const string Local = @"LOCAL";
      private const string Network = @"NETWORK";

      private static readonly string StartupFolder = AppDomain.CurrentDomain.BaseDirectory;
      private static readonly string SysDrive = Environment.GetEnvironmentVariable("SystemDrive");
      private static readonly string SysRoot = Environment.GetEnvironmentVariable("SystemRoot");
      private static readonly string SysRoot32 = Path.Combine(SysRoot, "System32");
      private static string NotepadExe = Path.Combine(SysRoot32, "notepad.exe");

      private const string TextTrue = "IsTrue";
      private const string TenNumbers = "0123456789";
      private const string TextHelloWorld = "Hëllõ Wørld!";
      private const string TextGoodByeWorld = "GóödByé Wôrld!";
      private const string TextAppend = "GóödByé Wôrld!";
      private const string TextUnicode = "ÛņïÇòdè; ǖŤƑ";

      #endregion // Fields

      #region CreateDirectoriesAndFiles

      private static void CreateDirectoriesAndFiles(string rootPath, int max, bool recurse)
      {
         for (int i = 0; i < max; i++)
         {
            string file = Path.Combine(rootPath, Path.GetRandomFileName());
            string dir = file + "-" + i + "-dir";
            file = file + "-" + i + "-file";

            Directory.CreateDirectory(dir);

            // Some directories will remain empty.
            if (i % 2 != 0)
            {
               File.WriteAllText(file, TextHelloWorld);
               File.WriteAllText(Path.Combine(dir, Path.GetFileName(file)), TextGoodByeWorld);
            }
         }

         if (recurse)
         {
            foreach (string dir in Directory.EnumerateDirectories(rootPath))
               CreateDirectoriesAndFiles(dir, max, false);
         }
      }

      #endregion // CreateDirectoriesAndFiles

      #region StopWatcher

      private static Stopwatch _stopWatcher;
      private static string StopWatcher(bool start = false)
      {
         if (_stopWatcher == null)
            _stopWatcher = new Stopwatch();

         if (start)
         {
            _stopWatcher.Restart();
            return null;
         }

         _stopWatcher.Stop();
         long ms = _stopWatcher.ElapsedMilliseconds;
         TimeSpan elapsed = _stopWatcher.Elapsed;

         return string.Format(CultureInfo.CurrentCulture, "*Duration: [{0}] ms. ({1})", ms, elapsed);
      }

      #endregion // StopWatcher

      #region Reporter

      private static string Reporter(bool onlyTime = false)
      {
         var lastError = new Win32Exception();

         StopWatcher();

         return onlyTime
            ? string.Format(CultureInfo.CurrentCulture, "\t\t{0}", StopWatcher())
            : string.Format(CultureInfo.CurrentCulture, "\t{0} [{1}: {2}]", StopWatcher(), lastError.NativeErrorCode, lastError.Message);
      }

      #endregion // Reporter

      #region IsAdmin

      private static bool IsAdmin()
      {
         bool isAdmin = new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);

         if (!isAdmin)
            Console.WriteLine("\n\tThis Unit Test must be run as Administrator.");

         return isAdmin;
      }

      #endregion // IsAdmin

      #region Dump

      /// <summary>Shows the Object's available Properties and Values.</summary>
      private static bool Dump(object obj, int width = -35, bool indent = false)
      {
         int cnt = 0;
         const string nulll = "\t\tnull";
         string template = "\t{0}#{1:000}\t{2, " + width + "} == \t[{3}]";

         if (obj == null)
         {
            Console.WriteLine(nulll);
            return false;
         }

         Console.WriteLine("\n\t{0}Instance: [{1}]\n", indent ? "\t" : "", obj.GetType().FullName);

         bool loopOk = false;
         foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(obj).Sort().Cast<PropertyDescriptor>().Where(descriptor => descriptor != null))
         {
            string propValue;
            try
            {
               object value = descriptor.GetValue(obj);
               propValue = (value == null) ? "null" : value.ToString();

               loopOk = true;
            }
            catch (Exception ex)
            {
               // Please do tell, oneliner preferably.
               propValue = ex.Message.Replace(Environment.NewLine, "  ");
            }

            Console.WriteLine(template, indent ? "\t" : "", ++cnt, descriptor.Name, propValue);
         }

         return loopOk;
      }

      #endregion //Dump

      #region InputPaths

      static readonly string[] InputPaths =
      {
         @".",
         @".zip",
         
         Path.DirectorySeparatorChar.ToString(CultureInfo.InvariantCulture),
         Path.DirectorySeparatorChar + @"Program Files\Microsoft Office",
         
         Path.GlobalRootPrefix + @"device\harddisk0\partition1\",
         Path.VolumePrefix + @"{12345678-aac3-31de-3321-3124565341ed}\Program Files\notepad.exe",

         @"Program Files\Microsoft Office",
         SysDrive[0].ToString(CultureInfo.InvariantCulture),
         SysDrive,
         SysDrive + @"\",
         SysDrive + @"\a",
         SysDrive + @"\a\",
         SysDrive + @"\a\b",
         SysDrive + @"\a\b\",
         SysDrive + @"\a\b\c",
         SysDrive + @"\a\b\c\",
         SysDrive + @"\a\b\c\f",
         SysDrive + @"\a\b\c\f.",
         SysDrive + @"\a\b\c\f.t",
         SysDrive + @"\a\b\c\f.tx",
         SysDrive + @"\a\b\c\f.txt",

         Path.LongPathPrefix + @"Program Files\Microsoft Office",
         Path.LongPathPrefix + SysDrive[0].ToString(CultureInfo.InvariantCulture),
         Path.LongPathPrefix + SysDrive,
         Path.LongPathPrefix + SysDrive + @"\",
         Path.LongPathPrefix + SysDrive + @"\a",
         Path.LongPathPrefix + SysDrive + @"\a\",
         Path.LongPathPrefix + SysDrive + @"\a\b",
         Path.LongPathPrefix + SysDrive + @"\a\b\",
         Path.LongPathPrefix + SysDrive + @"\a\b\c",
         Path.LongPathPrefix + SysDrive + @"\a\b\c\",
         Path.LongPathPrefix + SysDrive + @"\a\b\c\f",
         Path.LongPathPrefix + SysDrive + @"\a\b\c\f.",
         Path.LongPathPrefix + SysDrive + @"\a\b\c\f.t",
         Path.LongPathPrefix + SysDrive + @"\a\b\c\f.tx",
         Path.LongPathPrefix + SysDrive + @"\a\b\c\f.txt",

         Path.UncPrefix + @"Server\Share\",
         Path.UncPrefix + @"Server\Share\d",
         Path.UncPrefix + @"Server\Share\d1",
         Path.UncPrefix + @"Server\Share\d1\",
         Path.UncPrefix + @"Server\Share\d1\d",
         Path.UncPrefix + @"Server\Share\d1\d2",
         Path.UncPrefix + @"Server\Share\d1\d2\",
         Path.UncPrefix + @"Server\Share\d1\d2\f",
         Path.UncPrefix + @"Server\Share\d1\d2\fi",
         Path.UncPrefix + @"Server\Share\d1\d2\fil",
         Path.UncPrefix + @"Server\Share\d1\d2\file",
         Path.UncPrefix + @"Server\Share\d1\d2\file.",
         Path.UncPrefix + @"Server\Share\d1\d2\file.e",
         Path.UncPrefix + @"Server\Share\d1\d2\file.ex",
         Path.UncPrefix + @"Server\Share\d1\d2\file.ext",

         Path.LongPathUncPrefix + @"Server\Share\",
         Path.LongPathUncPrefix + @"Server\Share\d",
         Path.LongPathUncPrefix + @"Server\Share\d1",
         Path.LongPathUncPrefix + @"Server\Share\d1\",
         Path.LongPathUncPrefix + @"Server\Share\d1\d",
         Path.LongPathUncPrefix + @"Server\Share\d1\d2",
         Path.LongPathUncPrefix + @"Server\Share\d1\d2\",
         Path.LongPathUncPrefix + @"Server\Share\d1\d2\f",
         Path.LongPathUncPrefix + @"Server\Share\d1\d2\fi",
         Path.LongPathUncPrefix + @"Server\Share\d1\d2\fil",
         Path.LongPathUncPrefix + @"Server\Share\d1\d2\file",
         Path.LongPathUncPrefix + @"Server\Share\d1\d2\file.",
         Path.LongPathUncPrefix + @"Server\Share\d1\d2\file.e",
         Path.LongPathUncPrefix + @"Server\Share\d1\d2\file.ex",
         Path.LongPathUncPrefix + @"Server\Share\d1\d2\file.ext"
      };

      #endregion // InputPaths

      #region StringToByteArray

      private static byte[] StringToByteArray(string str, params Encoding[] encoding)
      {
         Encoding encode = encoding != null && encoding.Any() ? encoding[0] : new UTF8Encoding(true, true);
         return encode.GetBytes(str);
      }

      #endregion // StringToByteArray

      #endregion // Unit Test Helpers

      #region Unit Tests

      #region DumpCreateDirectory

      private static void DumpAccessRules(int cntCheck, DirectorySecurity dsSystem, DirectorySecurity dsAlpha)
      {
         Console.WriteLine("\n\tSanity check AlphaFS <> System.IO {0}.", cntCheck);
         Console.WriteLine("\t\tFile.GetAccessControl().AreAccessRulesProtected: [{0}]", dsAlpha.AreAccessRulesProtected);
         Assert.AreEqual(dsSystem.AreAccessRulesProtected, dsAlpha.AreAccessRulesProtected);

         StopWatcher(true);
         Console.WriteLine("\t\tFile.GetAccessControl().AreAuditRulesProtected: [{0}]", dsAlpha.AreAuditRulesProtected);
         Assert.AreEqual(dsSystem.AreAuditRulesProtected, dsAlpha.AreAuditRulesProtected);

         StopWatcher(true);
         Console.WriteLine("\t\tFile.GetAccessControl().AreAccessRulesCanonical: [{0}]", dsAlpha.AreAccessRulesCanonical);
         Assert.AreEqual(dsSystem.AreAccessRulesCanonical, dsAlpha.AreAccessRulesCanonical);

         StopWatcher(true);
         Console.WriteLine("\t\tFile.GetAccessControl().AreAuditRulesCanonical: [{0}]", dsAlpha.AreAuditRulesCanonical);
         Assert.AreEqual(dsSystem.AreAuditRulesCanonical, dsAlpha.AreAuditRulesCanonical);
      }

      #endregion // DumpCreateDirectory

      #region DumpCompressDecompress

      private void DumpCompressDecompress(bool isLocal)
      {
         Console.WriteLine("\n=== TEST {0} ===", isLocal ? Local : Network);
         string tempPath = Path.Combine(Path.GetTempPath(), "Directory.CompressDecompress()-" + Path.GetRandomFileName());
         if (!isLocal) tempPath = Path.LocalToUnc(tempPath);

         Console.WriteLine("\nInput Directory Path: [{0}]", tempPath);

         int cnt = 0;
         Directory.CreateDirectory(tempPath);
         FileAttributes actual = File.GetAttributes(tempPath);
         bool action = (actual & FileAttributes.Compressed) != 0;

         Console.WriteLine("\nCompressed (Should be False): [{0}]\t\tAttributes: [{1}]\n", action, actual);
         Assert.IsFalse(action, "Compression should be False");
         Assert.IsFalse((actual & FileAttributes.Compressed) != 0, "Compression should be False");


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
            action = (actual & FileAttributes.Compressed) != 0;

            Console.WriteLine("\t#{0:000}\tCompressed (Should be False): [{1}]\tAttributes: [{2}] [{3}]", ++cnt, action, actual, Path.GetFullPath(file));
            Assert.IsTrue(cnt > 0, "Nothing was enumerated.");
            Assert.IsFalse(action, "Compression should be False");
         }


         // Compress directory recursively.
         action = false;
         string report = "";

         StopWatcher(true);
         try
         {
            Directory.Compress(tempPath, DirectoryEnumerationOptions.Recursive);
            report = Reporter();
            action = true;
            actual = File.GetAttributes(tempPath);
         }
         catch (Exception ex)
         {
            Console.WriteLine("\n\tDirectory.Compress(): Caught unexpected Exception: [{0}]\n", ex.Message.Replace(Environment.NewLine, "  "));
            Directory.Delete(tempPath, true);
         }
         Console.WriteLine("\n\nDirectory compressed recursively (Should be True): [{0}]\t\tAttributes: [{1}]\n\t{2}\n", action, actual, report);
         Assert.IsTrue(action, "Compression should be True");
         Assert.IsTrue((actual & FileAttributes.Compressed) != 0, "Compression should be True");

         // Check that everything is compressed.
         cnt = 0;
         foreach (var fsei in Directory.EnumerateFileSystemEntryInfos<FileSystemEntryInfo>(tempPath, Path.WildcardStarMatchAll, DirectoryEnumerationOptions.Recursive))
         {
            actual = fsei.Attributes;
            action = (actual & FileAttributes.Compressed) != 0;

            Console.WriteLine("\t#{0:000}\tFS Entry: [{1}]\t\tCompressed (Should be True): [{2}]\t\tAttributes: [{3}]", ++cnt, fsei.FileName, action, actual);
            Assert.IsTrue(cnt > 0, "Nothing was enumerated.");
            Assert.IsTrue(action, "Compression should be True");
         }


         // Decompress directory recursively.
         action = false;
         StopWatcher(true);

         try
         {
            Directory.Decompress(tempPath, DirectoryEnumerationOptions.Recursive);
            report = Reporter();
            action = true;
            actual = File.GetAttributes(tempPath);
         }
         catch (Exception ex)
         {
            Console.WriteLine("\n\tCaught Exception: [{0}]\n", ex.Message.Replace(Environment.NewLine, "  "));
            Directory.Delete(tempPath, true);
         }
         Console.WriteLine("\n\nDirectory decompressed recursively (Should be True): [{0}]\t\tAttributes: [{1}]\n\t{2}\n", action, actual, report);
         Assert.IsTrue(action, "Compression should be True");
         Assert.IsFalse((actual & FileAttributes.Compressed) != 0, "Compression should be True");

         // Check that everything is decompressed.
         cnt = 0;
         foreach (var fsei in Directory.EnumerateFileSystemEntryInfos<FileSystemEntryInfo>(tempPath, Path.WildcardStarMatchAll, DirectoryEnumerationOptions.Recursive))
         {
            actual = fsei.Attributes;
            action = (actual & FileAttributes.Compressed) == 0;

            Console.WriteLine("\t#{0:000}\tFS Entry: [{1}]\t\tDecompressed (Should be True): [{2}]\t\tAttributes: [{3}]", ++cnt, fsei.FileName, action, actual);
            Assert.IsTrue(cnt > 0, "Nothing was enumerated.");
            Assert.IsTrue(action, "Decompression should be True");
         }


         Directory.Delete(tempPath, true);
         Assert.IsFalse(Directory.Exists(tempPath), "Cleanup failed: Directory should have been removed.");
         Console.WriteLine();
      }

      #endregion // DumpCompressDecompress

      #region DumpCopy

      private void DumpCopy(bool isLocal)
      {
         #region Setup

         Console.WriteLine("\n=== TEST {0} ===", isLocal ? Local : Network);
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
               Console.WriteLine("\nCatch: [{0}]: The caller does not have the required permission.", expectedException);

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
               Console.WriteLine("\n\tCaught Unexpected Exception: [{0}]: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));  
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

            #region DirectoryNotFoundException (Local) / IOException (Network)

            expectedLastError = (int)(isLocal ? Win32Errors.ERROR_PATH_NOT_FOUND : Win32Errors.ERROR_BAD_NET_NAME);
            expectedException = isLocal ? "System.IO.DirectoryNotFoundException" : "System.IO.IOException";
            exception = false;
            try
            {
               Console.WriteLine("\nCatch: [{0}]: The path specified by sourceDirName is invalid (for example, it is on an unmapped drive).", expectedException);
               Directory.Copy(letter + folderSource, letter + folderDestination, CopyOptions.FailIfExists);
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
                  Console.WriteLine("\n\tCaught Unexpected Exception: [{0}]: [{1}]", exceptionTypeName, ex.Message.Replace(Environment.NewLine, "  "));
            }
            Assert.IsTrue(exception, "[{0}] should have been caught.", expectedException);
            Console.WriteLine();

            #endregion // DirectoryNotFoundException (Local) / IOException (Network)

            #region IOException

            expectedLastError = (int)Win32Errors.ERROR_SAME_DRIVE;
            expectedException = "System.IO.IOException";
            exception = false;
            try
            {
               Console.WriteLine("\nCatch: [{0}]: The sourceDirName and destDirName parameters refer to the same file or directory.", expectedException);
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
               Console.WriteLine("\n\tCaught Unexpected Exception: [{0}]: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
            }
            Assert.IsTrue(exception, "[{0}] should have been caught.", expectedException);
            Console.WriteLine();

            #endregion // IOException

            #region Copy

            CreateDirectoriesAndFiles(tempPathSource, 10, true);

            Dictionary<string, long> props = Directory.GetProperties(tempPathSource, DirectoryEnumerationOptions.Recursive);
            long sourceFolder = props["Directory"];
            long sourceFile = props["File"];
            long sourceSize = props["Size"];

            Console.WriteLine("\nCopy from Source Path: [{0}]", tempPathSource);
            Console.WriteLine("\tTotal Directories: [{0}] Files: [{1}] Size: [{2}]", sourceFolder, sourceFile, Utils.UnitSizeToText(sourceSize));

            StopWatcher(true);
            Directory.Copy(tempPathSource, tempPathDestination, CopyOptions.FailIfExists);
            report = Reporter();

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
               Console.WriteLine("\n\tCaught Unexpected Exception: [{0}]: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
            }
            Assert.IsTrue(exception, "[{0}] should have been caught.", expectedException);
            Console.WriteLine();

            #endregion // IOException

            // Overwrite.
            StopWatcher(true);
            Directory.Copy(tempPathSource, tempPathDestination, CopyOptions.None | CopyOptions.NoBuffering);
            report = Reporter();

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

      #endregion // DumpCopy

      #region DumpCreateDirectory

      private void DumpCreateDirectory(bool isLocal)
      {
         #region Setup

         Console.WriteLine("\n=== TEST {0} ===", isLocal ? Local : Network);

         // Directory depth level.
         int level = new Random().Next(1, 1000);

#if NET35
         string emspace = "\u3000";
         string tempPath = Path.GetTempPath("Directory.CreateDirectory()-" + level + "-" + Path.GetRandomFileName() + emspace);
#else
         // MSDN: .NET 4+ Trailing spaces are removed from the end of the path parameter before deleting the directory.
         string tempPath = Path.GetTempPath("Directory.CreateDirectory()-" + level + "-" + Path.GetRandomFileName());
#endif
         if (!isLocal) tempPath = Path.LocalToUnc(tempPath);

         int expectedLastError;
         string expectedException;
         string report;
         bool exist;

         #endregion // Setup

         try
         {
            #region IOException

            using (File.Create(tempPath)) { }

            expectedLastError = (int)Win32Errors.ERROR_ALREADY_EXISTS;
            expectedException = "System.IO.IOException";
            bool exception = false;
            try
            {
               Console.WriteLine("\nCatch: [{0}]: File already exist with same name.", typeof(IOException).FullName);
               Directory.CreateDirectory(tempPath);
            }
            catch (AlreadyExistsException ex)
            {
               Assert.IsTrue(ex.Message.StartsWith("(" + expectedLastError + ")"), string.Format("Expected Win32Exception error is: [{0}]", expectedLastError));
               
               exception = true;
               Console.WriteLine("\n\t[{0}]: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
            }
            catch (Exception ex)
            {
               Console.WriteLine("\n\tCaught Unexpected Exception: [{0}]: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
            }
            Assert.IsTrue(exception, "[{0}] should have been caught.", expectedException);
            Console.WriteLine();

            File.Delete(tempPath);
            Assert.IsFalse(File.Exists(tempPath), "Cleanup failed: File should have been removed.");

            #endregion // IOException

            #region DirectoryNotFoundException (Local) / IOException (Network)

            expectedLastError = (int)(isLocal ? Win32Errors.ERROR_PATH_NOT_FOUND : Win32Errors.ERROR_BAD_NET_NAME);
            expectedException = isLocal ? "System.IO.DirectoryNotFoundException" : "System.IO.IOException";
            exception = false;
            try
            {
               Console.WriteLine("\nCatch: [{0}]: The specified path is invalid (for example, it is on an unmapped drive).", expectedException);
#if NET35
               string letter = DriveInfo.GetFreeDriveLetter() + @":\shouldFail" + emspace;
#else
   // MSDN: .NET 4+: Trailing spaces are removed from the end of the path parameter before deleting the directory.
               string letter = DriveInfo.GetFreeDriveLetter() + @":\Non-Existing";
#endif
               if (!isLocal) letter = Path.LocalToUnc(letter);

               Directory.CreateDirectory(letter);
            }
            catch (Exception ex)
            {
               var win32Error = new Win32Exception("", ex);
               Assert.IsTrue(win32Error.NativeErrorCode == expectedLastError, string.Format("Expected Win32Exception error should be: [{0}], got: [{1}]", expectedLastError, win32Error.NativeErrorCode));

               string exceptionTypeName = ex.GetType().FullName;
               if (exceptionTypeName.Equals(expectedException))
               {
                  exception = true;
                  Console.WriteLine("\n\t[{0}]: [{1}]", exceptionTypeName, ex.Message.Replace(Environment.NewLine, "  "));
               }
               else
                  Console.WriteLine("\n\tCaught Unexpected Exception: [{0}]: [{1}]", exceptionTypeName, ex.Message.Replace(Environment.NewLine, "  "));
            }
            Assert.IsTrue(exception, "[{0}] should have been caught.", expectedException);
            Console.WriteLine();

            #endregion // DirectoryNotFoundException (Local) / IOException (Network)

            #region ArgumentException

            expectedException = "System.ArgumentException";
            exception = false;
            try
            {
               Console.WriteLine("\nCatch: [{0}]: Path is prefixed with, or contains, only a colon character (:).", expectedException);
               Directory.CreateDirectory(@":AAAAAAAAAA");
            }
            catch (ArgumentException ex)
            {
               exception = true;
               Console.WriteLine("\n\t[{0}]: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
            }
            catch (Exception ex)
            {
               Console.WriteLine("\n\tCaught Unexpected Exception: [{0}]: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
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
               Console.WriteLine("\nCatch: [{0}]: Path contains a colon character (:) that is not part of a drive label (C:\\).", expectedException);

               string invalidPath = SysDrive + @"\dev\test\aaa:aaa.txt";
               if (!isLocal) invalidPath = Path.LocalToUnc(invalidPath) + ":aaa.txt";

               Directory.CreateDirectory(invalidPath);
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
               Console.WriteLine("\n\tCaught Unexpected Exception: [{0}]: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
            }
            Assert.IsTrue(exception, "[{0}] should have been caught.", expectedException);
            Console.WriteLine();

            #endregion // NotSupportedException


            Console.WriteLine("\nInput Directory Path: [{0}]", tempPath);

            System.IO.DirectoryInfo dirInfoSysIo = new System.IO.DirectoryInfo(tempPath);
            DirectoryInfo dirInfo = new DirectoryInfo(tempPath);
            Assert.AreEqual(dirInfoSysIo.Exists, dirInfo.Exists, "Exists AlphaFS != System.IO");

            // Should be false.
            Assert.IsFalse(dirInfoSysIo.Exists, "System.IO Directory should not exist: [{0}]", tempPath);
            Assert.IsFalse(dirInfo.Exists, "AlphaFS Directory should not exist: [{0}]", tempPath);


            StopWatcher(true);
            dirInfo.Create(true); // Create compressed directory.

            // dirInfo.Exists should be false.
            Assert.AreEqual(dirInfoSysIo.Exists, dirInfo.Exists, "AlphaFS Exists should match System.IO");


            string root = Path.Combine(tempPath, "Another Sub Directory");

            // MAX_PATH hit the road.
            for (int i = 0; i < level; i++)
               root = Path.Combine(root, "-" + (i + 1) + "-subdir");

            StopWatcher(true);
            dirInfo = Directory.CreateDirectory(root);
            report = Reporter();

            Console.WriteLine("\n\tCreated directory structure (Should be True): [{0}]{1}", dirInfo.Exists, report);
            Console.WriteLine("\n\tSubdirectory depth: [{0}], path length: [{1}] characters.", level, root.Length);
            Assert.IsTrue(Directory.Exists(root), "Directory should exist.");

            bool compressed = (dirInfo.Attributes & FileAttributes.Compressed) != 0;
            Console.WriteLine("\n\tCreated compressed directory (Should be True): [{0}]\n", compressed);
            Assert.IsTrue(compressed, "Directory should be compressed.");

         }
         finally
         {
            if (Directory.Exists(tempPath, PathFormat.FullPath))
            {
               StopWatcher(true);
               Directory.Delete(tempPath, true, true);
               report = Reporter();

               exist = Directory.Exists(tempPath);
               Console.WriteLine("\nDirectory.Delete() (Should be True): [{0}]{1}", !exist, report);
               Assert.IsFalse(exist, "Cleanup failed: Directory should have been removed.");
            }
         }

         Console.WriteLine();
      }

      #endregion // DumpCreateDirectory

      #region TestDelete

      private void TestDelete(bool isLocal)
      {
         #region Setup

         Console.WriteLine("\n=== TEST {0} ===", isLocal ? Local : Network);
         string tempFolder = Path.GetTempPath();
         string tempPath = Path.Combine(tempFolder, "Directory.Delete-" + Path.GetRandomFileName());
         if (!isLocal) tempPath = Path.LocalToUnc(tempPath);

         string notePad = isLocal ? NotepadExe : Path.LocalToUnc(NotepadExe);

         string nonExistingDirectory = SysRoot32 + @"\NonExistingDirectory-" + Path.GetRandomFileName();
         if (!isLocal) nonExistingDirectory = Path.LocalToUnc(nonExistingDirectory);

         string sysDrive = SysDrive;
         if (!isLocal) sysDrive = Path.LocalToUnc(sysDrive);

         string sysRoot = SysRoot;
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

               Directory.Delete(sysDrive + @"\<>");
            }
            catch (ArgumentException ex)
            {
               exception = true;
               Console.WriteLine("\n\t[{0}]: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
            }
            catch (Exception ex)
            {
               Console.WriteLine("\n\tCaught Unexpected Exception: [{0}]: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
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

               string invalidPath = SysDrive + @"\dev\test\aaa:aaa.txt";
               if (!isLocal) invalidPath = Path.LocalToUnc(invalidPath) + ":aaa.txt";

               Directory.Delete(invalidPath);
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
               Console.WriteLine("\n\tCaught Unexpected Exception: [{0}]: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
            }
            Assert.IsTrue(exception, "[{0}] should have been caught.", expectedException);
            Console.WriteLine();

            #endregion // NotSupportedException

            #region UnauthorizedAccessException

            string cscPath = Path.Combine(SysRoot, "CSC");
            if (!isLocal) cscPath = Path.LocalToUnc(cscPath);

            if (Directory.Exists(cscPath))
            {
               // It seems that under Windows 7 x86 Pro, a different Exception is thrown
               // compared to Windows 8.1 X64 Enterprise.
               bool isWin8 = OperatingSystem.IsAtLeast(OperatingSystem.EnumOsName.Windows8);

               expectedLastError = (int)(isWin8 ? Win32Errors.ERROR_ACCESS_DENIED : Win32Errors.ERROR_SHARING_VIOLATION);
               expectedException = isWin8 ? "System.UnauthorizedAccessException" : "System.IO.IOException";
               exception = false;
               try
               {
                  Console.WriteLine("\nCatch #{0}: [{1}]: The caller does not have the required permission.", ++catchCount, expectedException);

                  Directory.Delete(cscPath);
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
                     Console.WriteLine("\n\tCaught Unexpected Exception: [{0}]: [{1}]", exceptionTypeName, ex.Message.Replace(Environment.NewLine, "  "));
               }
               Assert.IsTrue(exception, "[{0}] should have been caught.", expectedException);
               Console.WriteLine();
            }

            #endregion // UnauthorizedAccessException

            #region DirectoryNotFoundException #1 (Local) / IOException (Network)

            expectedLastError = (int)(isLocal ? Win32Errors.ERROR_PATH_NOT_FOUND : Win32Errors.ERROR_BAD_NET_NAME);
            expectedException = isLocal ? "System.IO.DirectoryNotFoundException" : "System.IO.IOException";
            exception = false;
            try
            {
               Console.WriteLine("\nCatch #{0}: [{1}]: The specified path is invalid (for example, it is on an unmapped drive).", ++catchCount, expectedException);

               Directory.Delete(nonExistingDirectory.Replace(sysDrive + @"\", letter));
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
                  Console.WriteLine("\n\tCaught Unexpected Exception: [{0}]: [{1}]", exceptionTypeName, ex.Message.Replace(Environment.NewLine, "  "));
            }
            Assert.IsTrue(exception, "[{0}] should have been caught.", expectedException);
            Console.WriteLine();

            #endregion // DirectoryNotFoundException #1 (Local) / IOException (Network)

            #region DirectoryNotFoundException #2

            expectedLastError = (int)Win32Errors.ERROR_DIRECTORY;
            expectedException = "System.IO.DirectoryNotFoundException";
            exception = false;
            try
            {
               Console.WriteLine("\nCatch #{0}: [{1}]: Path refers to a file instead of a directory.", ++catchCount, expectedException);

               using (File.Create(tempPath)) {}

               Directory.Delete(tempPath);
            }
            catch (DirectoryNotFoundException ex)
            {
               var win32Error = new Win32Exception("", ex);
               Assert.IsTrue(win32Error.NativeErrorCode == expectedLastError, string.Format("Expected Win32Exception error should be: [{0}], got: [{1}]", expectedLastError, win32Error.NativeErrorCode));
               Assert.IsTrue(ex.Message.StartsWith("(" + Win32Errors.ERROR_INVALID_PARAMETER + ")"), string.Format("Expected Win32Exception error is: [{0}]", expectedLastError));

               exception = true;
               Console.WriteLine("\n\t[{0}]: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
            }
            catch (Exception ex)
            {
               Console.WriteLine("\n\tCaught Unexpected Exception: [{0}]: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
            }
            Assert.IsTrue(exception, "[{0}] should have been caught.", expectedException);
            File.Delete(tempPath);
            Assert.IsFalse(File.Exists(tempPath), "Cleanup failed: File should have been removed.");
            Console.WriteLine();

            #endregion // DirectoryNotFoundException #2

            #region IOException #1 (DirectoryNotEmptyException)

            expectedLastError = (int) Win32Errors.ERROR_DIR_NOT_EMPTY;
            expectedException = "System.IO.IOException";
            exception = false;
            try
            {
               Console.WriteLine("\nCatch #{0}: [{1}]: The directory specified by path is not empty.", ++catchCount, expectedException);

               Directory.CreateDirectory(tempPath);

               using (File.Create(Path.Combine(tempPath, "a-created-file.txt"))) { }

               Directory.Delete(tempPath);
            }
            catch (DirectoryNotEmptyException ex)
            {
               var win32Error = new Win32Exception("", ex);
               Assert.IsTrue(win32Error.NativeErrorCode == expectedLastError, string.Format("Expected Win32Exception error should be: [{0}], got: [{1}]", expectedLastError, win32Error.NativeErrorCode));
               Assert.IsTrue(ex.Message.StartsWith("(" + expectedLastError + ")"), string.Format("Expected Win32Exception error is: [{0}]", expectedLastError));

               exception = true;
               Console.WriteLine("\n\t[{0}]: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
            }
            catch (Exception ex)
            {
               Console.WriteLine("\n\tCaught Unexpected Exception: [{0}]: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
            }
            Assert.IsTrue(exception, "[{0}] should have been caught.", expectedException);
            Console.WriteLine();

            #endregion // IOException #1 (DirectoryNotEmptyException)

            #region IOException #2 (DirectoryReadOnlyException)

            File.SetAttributes(tempPath, FileAttributes.ReadOnly);

            expectedLastError = (int) Win32Errors.ERROR_ACCESS_DENIED;
            expectedException = "System.IO.IOException";
            exception = false;
            try
            {
               Console.WriteLine("\nCatch #{0}: [{1}]: The directory is read-only.", ++catchCount, expectedException);

               Directory.Delete(tempPath, true);
            }
            catch (DirectoryReadOnlyException ex)
            {
               var win32Error = new Win32Exception("", ex);
               Assert.IsTrue(win32Error.NativeErrorCode == expectedLastError, string.Format("Expected Win32Exception error should be: [{0}], got: [{1}]", expectedLastError, win32Error.NativeErrorCode));
               Assert.IsTrue(ex.Message.StartsWith("(" + Win32Errors.ERROR_FILE_READ_ONLY + ")"), string.Format("Expected Win32Exception error is: [{0}]", expectedLastError));

               exception = true;
               Console.WriteLine("\n\t[{0}]: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
            }
            catch (Exception ex)
            {
               Console.WriteLine("\n\tCaught Unexpected Exception: [{0}]: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
            }
            Assert.IsTrue(exception, "[{0}] should have been caught.", expectedException);
            File.SetAttributes(tempPath, FileAttributes.Normal);
            Directory.Delete(tempPath, true);
            Assert.IsFalse(File.Exists(tempPath), "Cleanup failed: File should have been removed.");
            Console.WriteLine();

            #endregion // IOException #2 (DirectoryReadOnlyException)

            #region IOException #3

            expectedLastError = (int)Win32Errors.ERROR_ACCESS_DENIED;
            expectedException = "System.UnauthorizedAccessException";
            exception = false;
            try
            {
               Console.WriteLine("\nCatch #{0}: [{1}]: The directory contains a read-only file.", ++catchCount, expectedException);

               Directory.CreateDirectory(tempPath);

               string readOnlyFile = Path.Combine(tempPath, "a-read-only-created-file.txt");
               using (File.Create(readOnlyFile)) { }
               File.SetAttributes(readOnlyFile, FileAttributes.ReadOnly);

               Directory.Delete(tempPath, true);
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
               Console.WriteLine("\n\tCaught Unexpected Exception: [{0}]: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
            }
            Assert.IsTrue(exception, "[{0}] should have been caught.", expectedException);
            Console.WriteLine();

            #endregion // IOException #3
         }
         finally
         {
            Directory.Delete(tempPath, true, true);
            Assert.IsFalse(File.Exists(tempPath), "Cleanup failed: File should have been removed.");
         }

         Console.WriteLine();
      }

      #endregion // TestDelete

      #region DumpEnableDisableCompression

      private void DumpEnableDisableCompression(bool isLocal)
      {
         Console.WriteLine("\n=== TEST {0} ===", isLocal ? Local : Network);
         string tempPath = Path.Combine(Path.GetTempPath(), "Directory.EnableDisableCompression()-" + Path.GetRandomFileName());
         if (!isLocal) tempPath = Path.LocalToUnc(tempPath);

         Console.WriteLine("\nInput Directory Path: [{0}]", tempPath);

         Directory.CreateDirectory(tempPath);
         FileAttributes actual = File.GetAttributes(tempPath);
         Console.WriteLine("Attributes: [{0}]", actual);
         Assert.IsFalse((actual & FileAttributes.Compressed) != 0);
         Assert.IsTrue((actual & FileAttributes.Directory) != 0);


         string report = string.Empty;
         bool action = false;
         StopWatcher(true);
         try
         {
            Directory.EnableCompression(tempPath);
            report = Reporter();
            action = true;
            actual = File.GetAttributes(tempPath);
         }
         catch (Exception ex)
         {
            Console.WriteLine("\n");
            Console.WriteLine("\n\tCaught Exception: [{0}]\n", ex.Message.Replace(Environment.NewLine, "  "));
         }
         Console.WriteLine("\nEnableCompression() successful (Should be True): [{0}]", action);
         Console.WriteLine("Attributes: [{0}]\n\t{1}", actual, report);
         Assert.IsTrue(action, "Directory should have compression enabled.");
         Assert.IsTrue((actual & FileAttributes.Compressed) != 0, "Directory should have compression enabled.");


         action = false;
         try
         {
            Directory.DisableCompression(tempPath);
            report = Reporter();
            action = true;
            actual = File.GetAttributes(tempPath);
         }
         catch (Exception ex)
         {
            Console.WriteLine("\n\tCaught Exception: [{0}]\n", ex.Message.Replace(Environment.NewLine, "  "));
         }
         Console.WriteLine("\nDisableCompression() successful (Should be True): [{0}]", action);
         Console.WriteLine("Attributes: [{0}]\n\t{1}", actual, report);
         Assert.IsTrue(action, "Directory should have compression disabled.");
         Assert.IsFalse((actual & FileAttributes.Compressed) != 0, "Directory should have compression disabled.");


         Directory.Delete(tempPath, true);
         Assert.IsFalse(Directory.Exists(tempPath), "Cleanup failed: Directory should have been removed.");
         Console.WriteLine();
      }

      #endregion // DumpEnableDisableCompression

      #region DumpEnableDisableEncryption

      private void DumpEnableDisableEncryption(bool isLocal)
      {
         Console.WriteLine("\n=== TEST {0} ===", isLocal ? Local : Network);
         string tempPath = Path.Combine(Path.GetTempPath(), "Directory.EnableDisableEncryption()-" + Path.GetRandomFileName());
         if (!isLocal) tempPath = Path.LocalToUnc(tempPath);

         Console.WriteLine("\nInput Directory Path: [{0}]", tempPath);

         const string disabled = "Disable=0";
         const string enabled = "Disable=1";
         string lineDisable = string.Empty;
         string deskTopIni = Path.Combine(tempPath, "Desktop.ini");

         Directory.CreateDirectory(tempPath);
         FileAttributes actual = File.GetAttributes(tempPath);
         Console.WriteLine("Attributes: [{0}]", actual);
         Assert.IsTrue((actual & FileAttributes.Directory) != 0);


         string report = string.Empty;
         bool action = false;
         try
         {
            StopWatcher(true);
            Directory.EnableEncryption(tempPath);
            report = Reporter();
            action = true;
         }
         catch (Exception ex)
         {
            Console.WriteLine("\n\tCaught Exception: [{0}]\n", ex.Message.Replace(Environment.NewLine, "  "));
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
         Console.WriteLine("File Desktop.ini contents: [{0}]\n\t{1}", lineDisable, report);
         Assert.IsTrue(action, "Encryption should be True");
         Assert.IsTrue(File.Exists(deskTopIni), "Desktop.ini should Exist");


         action = false;
         try
         {
            StopWatcher(true);
            Directory.DisableEncryption(tempPath);
            report = Reporter();
            action = true;
         }
         catch (Exception ex)
         {
            Console.WriteLine("\n\tCaught Exception: [{0}]\n", ex.Message.Replace(Environment.NewLine, "  "));
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
         Console.WriteLine("File Desktop.ini contents: [{0}]\n\t{1}", lineDisable, report);
         Assert.IsTrue(action, "Encryption should be True");
         Assert.IsTrue(File.Exists(deskTopIni), "Desktop.ini should Exist");


         Directory.Delete(tempPath, true);
         Assert.IsFalse(Directory.Exists(tempPath), "Cleanup failed: Directory should have been removed.");
         Console.WriteLine();
      }

      #endregion // DumpEnableDisableEncryption

      #region DumpEncryptDecrypt

      private void DumpEncryptDecrypt(bool isLocal)
      {
         #region Setup

         Console.WriteLine("\n=== TEST {0} ===", isLocal ? Local : Network);
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
         StopWatcher(true);
         try
         {
            Directory.Encrypt(tempPath, true);
            report = Reporter();
            action = true;
         }
         catch (Exception ex)
         {
            action = false;
            report = Reporter();
            Console.WriteLine("\n\tDirectory.Encrypt(): Caught unexpected Exception: [{0}]\n", ex.Message.Replace(Environment.NewLine, "  "));
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
         Assert.IsTrue(cnt > 0, "Nothing was enumerated.");

         #endregion // Encrypt

         #region Decrypt

         // Decrypt directory only.
         StopWatcher(true);
         try
         {
            Directory.Decrypt(tempPath);
            report = Reporter();
            action = true;
         }
         catch (Exception ex)
         {
            action = false;
            report = Reporter();
            Console.WriteLine("\n\tDirectory.Decrypt(): Caught unexpected Exception: [{0}]\n", ex.Message.Replace(Environment.NewLine, "  "));
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
         Assert.IsTrue(cnt > 0, "Nothing was enumerated.");

         #endregion // Decrypt

         Directory.Delete(tempPath, true);
         Assert.IsFalse(Directory.Exists(tempPath), "Cleanup failed: Directory should have been removed.");
         Console.WriteLine();
      }

      #endregion // DumpEncryptDecrypt

      #region DumpEnumerateDirectories

      private void DumpEnumerateDirectories(bool isLocal)
      {
         #region Setup

         Console.WriteLine("\n=== TEST {0} ===", isLocal ? Local : Network);

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

         #endregion //Setup

         #region DirectoryNotFoundException (Local) / IOException (Network)

         expectedLastError = (int)(isLocal ? Win32Errors.ERROR_PATH_NOT_FOUND : Win32Errors.ERROR_BAD_NET_NAME);
         expectedException = isLocal ? "System.IO.DirectoryNotFoundException" : "System.IO.IOException";
         exception = false;
         try
         {
            Console.WriteLine("\nCatch: [{0}]: Path is invalid, such as referring to an unmapped drive.", expectedException);

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
               Console.WriteLine("\n\tCaught Unexpected Exception: [{0}]: [{1}]", exceptionTypeName, ex.Message.Replace(Environment.NewLine, "  "));
         }
         Assert.IsTrue(exception, "[{0}] should have been caught.", expectedException);
         Console.WriteLine();

         #endregion // DirectoryNotFoundException (Local) / IOException (Network)

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
               Console.WriteLine("\nCatch: [{0}]: Path is a file name.", expectedException);

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
               Console.WriteLine("\n\tCaught Unexpected Exception: [{0}]: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
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

         tempPath = Path.Combine(SysRoot, "CSC");
         if (!isLocal) tempPath = Path.LocalToUnc(tempPath);

         var di = new DirectoryInfo(tempPath);
         if (di.Exists)
         {
            expectedLastError = (int)Win32Errors.ERROR_ACCESS_DENIED;
            expectedException = "System.UnauthorizedAccessException";
            exception = false;
            try
            {
               Console.WriteLine("\nCatch: [{0}]: The caller does not have the required permission.", expectedException);

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
               Console.WriteLine("\n\tCaught Unexpected Exception: [{0}]: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
            }
            Assert.IsTrue(exception, "[{0}] should have been caught.", expectedException);
            Console.WriteLine();
         }

         #endregion // UnauthorizedAccessException

         string path = isLocal ? SysRoot : Path.LocalToUnc(SysRoot);

         Console.WriteLine("\nInput Directory Path: [{0}]\n", path);
         Console.WriteLine("\tEnumerate directories, using \"SearchOption.{0}\"\n", searchOption);

         StopWatcher(true);
         foreach (DirectoryInfo dirInfo in new DirectoryInfo(path).EnumerateDirectories(searchPattern, searchOption))
         {
            Console.WriteLine("\t#{0:000}\t[{1}]", ++cnt, dirInfo.FullName);

            // Issue #21601: OverflowException when accessing EntryInfo.
            var isMountPoint = dirInfo.EntryInfo.IsMountPoint;

            Assert.IsTrue(dirInfo.EntryInfo.IsDirectory);
         }

         Console.WriteLine();
         Console.WriteLine(Reporter());
         Assert.IsTrue(cnt > 0, "Nothing was enumerated.");
         Console.WriteLine();


         cnt = 0;
         searchPattern = @"*e*e*";
         Console.WriteLine("\tsearchPattern: [{0}]\n", searchPattern);
         StopWatcher(true);
         foreach (DirectoryInfo dirInfo in new DirectoryInfo(path).EnumerateDirectories(searchPattern, searchOption))
         {
            Console.WriteLine("\t#{0:000}\t[{1}]", ++cnt, dirInfo.FullName);

            // Issue #21601: OverflowException when accessing EntryInfo.
            var isMountPoint = dirInfo.EntryInfo.IsMountPoint;

            Assert.IsTrue(dirInfo.EntryInfo.IsDirectory);
         }

         Console.WriteLine();
         Console.WriteLine(Reporter());
         Assert.IsTrue(cnt > 0, "Nothing was enumerated.");
         Console.WriteLine();
      }

      #endregion // DumpEnumerateDirectories

      #region DumpEnumerateFileIdBothDirectoryInfo

      private void DumpEnumerateFileIdBothDirectoryInfo(bool isLocal)
      {
         Console.WriteLine("\n=== TEST {0} ===", isLocal ? Local : Network);
         string tempPath = SysRoot;
         if (!isLocal) tempPath = Path.LocalToUnc(tempPath);

         Console.WriteLine("\nInput Directory Path: [{0}]", tempPath);

         string searchPattern = Path.WildcardStarMatchAll;

         long directories = Directory.CountFileSystemObjects(tempPath, searchPattern, DirectoryEnumerationOptions.Folders);
         long files = Directory.CountFileSystemObjects(tempPath, searchPattern, DirectoryEnumerationOptions.Files);
         Console.WriteLine("\tDirectories: [{0}], Files: [{1}]", directories, files);

         bool foundFse = false;
         long numDirectories = 0;
         long numFiles = 0;

         StopWatcher(true);
         foreach (FileIdBothDirectoryInfo fibdi in Directory.EnumerateFileIdBothDirectoryInfo(tempPath))
         {
            if ((fibdi.FileAttributes & FileAttributes.Directory) != 0)
               numDirectories++;
            else
               numFiles++;

            foundFse = Dump(fibdi, -22);
         }
         string report = Reporter();

         bool matchAll = directories == numDirectories && files == numFiles;
         Console.WriteLine("\n\tDirectories = [{0}], Files = [{1}]\n{2}", numDirectories, numFiles, report);
         Assert.IsTrue(foundFse, "Nothing was enumerated.");
         Assert.IsTrue(matchAll, "Number of directories and/or files don't match.");
         Console.WriteLine();
      }

      #endregion // DumpEnumerateFileIdBothDirectoryInfo

      #region DumpEnumerateFiles

      private void DumpEnumerateFiles(bool isLocal)
      {
         #region Setup

         Console.WriteLine("\n=== TEST {0} ===", isLocal ? Local : Network);

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

         #endregion //Setup

         #region DirectoryNotFoundException (Local) / IOException (Network)

         expectedLastError = (int)(isLocal ? Win32Errors.ERROR_PATH_NOT_FOUND : Win32Errors.ERROR_BAD_NET_NAME);
         expectedException = isLocal ? "System.IO.DirectoryNotFoundException" : "System.IO.IOException";
         exception = false;
         try
         {
            Console.WriteLine("\nCatch: [{0}]: Path is invalid, such as referring to an unmapped drive.", expectedException);

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
               Console.WriteLine("\n\tCaught Unexpected Exception: [{0}]: [{1}]", exceptionTypeName, ex.Message.Replace(Environment.NewLine, "  "));
         }
         Assert.IsTrue(exception, "[{0}] should have been caught.", expectedException);
         Console.WriteLine();

         #endregion // DirectoryNotFoundException (Local) / IOException (Network)

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
               Console.WriteLine("\nCatch: [{0}]: Path is a file name.", expectedException);

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
               Console.WriteLine("\n\tCaught Unexpected Exception: [{0}]: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
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

         tempPath = Path.Combine(SysRoot, "CSC");
         if (!isLocal) tempPath = Path.LocalToUnc(tempPath);

         var di = new DirectoryInfo(tempPath);
         if (di.Exists)
         {
            expectedLastError = (int)Win32Errors.ERROR_ACCESS_DENIED;
            expectedException = "System.UnauthorizedAccessException";
            exception = false;
            try
            {
               Console.WriteLine("\nCatch: [{0}]: The caller does not have the required permission.", expectedException);

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
               Console.WriteLine("\n\tCaught Unexpected Exception: [{0}]: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
            }
            Assert.IsTrue(exception, "[{0}] should have been caught.", expectedException);
            Console.WriteLine();
         }

         #endregion // UnauthorizedAccessException

         string path = isLocal ? SysRoot : Path.LocalToUnc(SysRoot);

         Console.WriteLine("\nInput Directory Path: [{0}]\n", path);
         Console.WriteLine("\tEnumerate files, using \"SearchOption.{0}\"\n", searchOption);

         StopWatcher(true);
         foreach (FileInfo fileInfo in new DirectoryInfo(path).EnumerateFiles(searchPattern, searchOption))
         {
            Console.WriteLine("\t#{0:000}\t[{1}]", ++cnt, fileInfo.FullName);

            // Issue #21601: OverflowException when accessing EntryInfo.
            var isMountPoint = fileInfo.EntryInfo.IsMountPoint;

            Assert.IsFalse(fileInfo.EntryInfo.IsDirectory);
         }

         Console.WriteLine();
         Console.WriteLine(Reporter());
         Assert.IsTrue(cnt > 0, "Nothing was enumerated.");
         Console.WriteLine();


         cnt = 0;
         searchPattern = @"*e*.exe";
         Console.WriteLine("\tsearchPattern: [{0}]\n", searchPattern);
         StopWatcher(true);
         foreach (FileInfo fileInfo in new DirectoryInfo(path).EnumerateFiles(searchPattern, searchOption))
         {
            Console.WriteLine("\t#{0:000}\t[{1}]", ++cnt, fileInfo.FullName);

            // Issue #21601: OverflowException when accessing EntryInfo.
            var isMountPoint = fileInfo.EntryInfo.IsMountPoint;

            Assert.IsFalse(fileInfo.EntryInfo.IsDirectory);
         }

         Console.WriteLine();
         Console.WriteLine(Reporter());
         Assert.IsTrue(cnt > 0, "Nothing was enumerated.");
         Console.WriteLine();
      }

      #endregion // DumpEnumerateFiles

      #region DumpEnumerateFileSystemEntries

      private void DumpEnumerateFileSystemEntries(bool isLocal)
      {
         #region Setup

         Console.WriteLine("\n=== TEST {0} ===", isLocal ? Local : Network);

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

         #endregion //Setup

         #region DirectoryNotFoundException (Local) / IOException (Network)

         expectedLastError = (int)(isLocal ? Win32Errors.ERROR_PATH_NOT_FOUND : Win32Errors.ERROR_BAD_NET_NAME);
         expectedException = isLocal ? "System.IO.DirectoryNotFoundException" : "System.IO.IOException";
         exception = false;
         try
         {
            Console.WriteLine("\nCatch: [{0}]: Path is invalid, such as referring to an unmapped drive.", expectedException);

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
               Console.WriteLine("\n\tCaught Unexpected Exception: [{0}]: [{1}]", exceptionTypeName, ex.Message.Replace(Environment.NewLine, "  "));
         }
         Assert.IsTrue(exception, "[{0}] should have been caught.", expectedException);
         Console.WriteLine();

         #endregion // DirectoryNotFoundException (Local) / IOException (Network)

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
               Console.WriteLine("\nCatch: [{0}]: Path is a file name.", expectedException);

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
               Console.WriteLine("\n\tCaught Unexpected Exception: [{0}]: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
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

         tempPath = Path.Combine(SysRoot, "CSC");
         if (!isLocal) tempPath = Path.LocalToUnc(tempPath);

         var di = new DirectoryInfo(tempPath);
         if (di.Exists)
         {
            expectedLastError = (int)Win32Errors.ERROR_ACCESS_DENIED;
            expectedException = "System.UnauthorizedAccessException";
            exception = false;
            try
            {
               Console.WriteLine("\nCatch: [{0}]: The caller does not have the required permission.", expectedException);

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
               Console.WriteLine("\n\tCaught Unexpected Exception: [{0}]: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
            }
            Assert.IsTrue(exception, "[{0}] should have been caught.", expectedException);
            Console.WriteLine();
         }

         #endregion // UnauthorizedAccessException

         string path = isLocal ? SysRoot : Path.LocalToUnc(SysRoot);

         Console.WriteLine("\nInput Directory Path: [{0}]\n", path);
         Console.WriteLine("\tEnumerate file system entries, using \"SearchOption.{0}\"\n", searchOption);

         StopWatcher(true);
         foreach (FileSystemInfo fsi in new DirectoryInfo(path).EnumerateFileSystemInfos(searchPattern, searchOption))
         {
            Console.WriteLine("\t#{0:000}\t[{1}]", ++cnt, fsi.FullName);

            // Issue #21601: OverflowException when accessing EntryInfo.
            // (Actually only for DirectoryInfo() and FileInfo())
            var isMountPoint = fsi.EntryInfo.IsMountPoint;
         }

         Console.WriteLine();
         Console.WriteLine(Reporter());
         Assert.IsTrue(cnt > 0, "Nothing was enumerated.");
         Console.WriteLine();


         cnt = 0;
         searchPattern = @"*e*e*";
         Console.WriteLine("\tsearchPattern: [{0}]\n", searchPattern);
         StopWatcher(true);
         foreach (FileSystemInfo fsi in new DirectoryInfo(path).EnumerateDirectories(searchPattern, searchOption))
         {
            Console.WriteLine("\t#{0:000}\t[{1}]", ++cnt, fsi.FullName);

            // Issue #21601: OverflowException when accessing EntryInfo.
            var isMountPoint = fsi.EntryInfo.IsMountPoint;

            Assert.IsTrue(fsi.EntryInfo.IsDirectory || !fsi.EntryInfo.IsDirectory);
         }

         Console.WriteLine();
         Console.WriteLine(Reporter());
         Assert.IsTrue(cnt > 0, "Nothing was enumerated.");
         Console.WriteLine();
      }

      #endregion // DumpEnumerateFileSystemEntries

      #region DumpExists

      private void DumpExists(bool isLocal)
      {
         Console.WriteLine("\n=== TEST {0} ===", isLocal ? Local : Network);
         string tempPath = Path.GetTempPath("Directory-Exists-" + Path.GetRandomFileName());
         if (!isLocal) tempPath = Path.LocalToUnc(tempPath);

         Console.WriteLine("\nInput Directory Path: [{0}]\n", tempPath);

         bool exists = Directory.Exists(tempPath);
         Console.WriteLine("\tDirectory.Exists() (Should be False): [{0}]", exists);
         Assert.IsFalse(exists, "Directory should not exist.");

         Directory.CreateDirectory(tempPath);

         exists = Directory.Exists(tempPath);
         Console.WriteLine("\n\tCreated directory.");
         Console.WriteLine("\tDirectory.Exists() (Should be True): [{0}]", exists);
         Assert.IsTrue(exists, "Directory should exist.");

         Directory.Delete(tempPath, true);
         Assert.IsFalse(Directory.Exists(tempPath), "Cleanup failed: Directory should have been removed.");
         Console.WriteLine();
      }

      #endregion // DumpExists

      #region DumpGetAccessControl

      private void DumpGetAccessControl(bool isLocal)
      {
         Console.WriteLine("\n=== TEST {0} ===", isLocal ? Local : Network);

         string tempPath = Path.Combine(Path.GetTempPath(), "Directory.GetAccessControl()-" + Path.GetRandomFileName());
         if (!isLocal) tempPath = Path.LocalToUnc(tempPath);
         Directory.CreateDirectory(tempPath);

         bool foundRules = false;

         StopWatcher(true);
         DirectorySecurity gac = Directory.GetAccessControl(tempPath);
         string report = Reporter();

         AuthorizationRuleCollection accessRules = gac.GetAccessRules(true, true, typeof(NTAccount));
         DirectorySecurity sysIo = System.IO.Directory.GetAccessControl(tempPath);
         AuthorizationRuleCollection sysIoaccessRules = sysIo.GetAccessRules(true, true, typeof(NTAccount));

         Console.WriteLine("\nInput Directory Path: [{0}]", tempPath);
         Console.WriteLine("\n\tGetAccessControl() rules found: [{0}]\n    System.IO rules found         : [{1}]\n{2}", accessRules.Count, sysIoaccessRules.Count, report);
         Assert.AreEqual(sysIoaccessRules.Count, accessRules.Count);

         foreach (FileSystemAccessRule far in accessRules)
         {
            Dump(far, 17);
            DumpAccessRules(1, sysIo, gac);
            foundRules = true;
         }
         Assert.IsTrue(foundRules);

         Directory.Delete(tempPath, true);
         Assert.IsFalse(Directory.Exists(tempPath), "Cleanup failed: Directory should have been removed.");

         Console.WriteLine("\n");
      }

      #endregion // DumpGetAccessControl

      #region DumpGetDirectories

      private void DumpGetDirectories(bool isLocal)
      {
         #region Setup

         Console.WriteLine("\n=== TEST {0} ===", isLocal ? Local : Network);

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

         #endregion //Setup

         #region DirectoryNotFoundException (Local) / IOException (Network)

         expectedLastError = (int)(isLocal ? Win32Errors.ERROR_PATH_NOT_FOUND : Win32Errors.ERROR_BAD_NET_NAME);
         expectedException = isLocal ? "System.IO.DirectoryNotFoundException" : "System.IO.IOException";
         exception = false;
         try
         {
            Console.WriteLine("\nCatch: [{0}]: The specified path is invalid (for example, it is on an unmapped drive).", expectedException);

            string nonExistingPath = letter + folderSource;
            if (!isLocal) nonExistingPath = Path.LocalToUnc(nonExistingPath);

            Directory.GetDirectories(nonExistingPath);
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
               Console.WriteLine("\n\tCaught Unexpected Exception: [{0}]: [{1}]", exceptionTypeName, ex.Message.Replace(Environment.NewLine, "  "));
         }
         Assert.IsTrue(exception, "[{0}] should have been caught.", expectedException);
         Console.WriteLine();

         #endregion // DirectoryNotFoundException (Local) / IOException (Network)

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
               Console.WriteLine("\nCatch: [{0}]: Path is a file name.", expectedException);

               Directory.GetDirectories(tempPath);
            }
            catch (IOException ex)
            {
               Assert.IsTrue(ex.Message.StartsWith("(" + expectedLastError + ")"), string.Format("Expected Win32Exception error is: [{0}]", expectedLastError));

               exception = true;
               Console.WriteLine("\n\t[{0}]: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
            }
            catch (Exception ex)
            {
               Console.WriteLine("\n\tCaught Unexpected Exception: [{0}]: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
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

         tempPath = Path.Combine(SysRoot, "CSC");
         if (!isLocal) tempPath = Path.LocalToUnc(tempPath);

         if (Directory.Exists(tempPath))
         {
            expectedLastError = (int)Win32Errors.ERROR_ACCESS_DENIED;
            expectedException = "System.UnauthorizedAccessException";
            exception = false;
            try
            {
               Console.WriteLine("\nCatch: [{0}]: The caller does not have the required permission.", expectedException);

               Directory.GetDirectories(tempPath, searchPattern, SearchOption.AllDirectories).Any();
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
               Console.WriteLine("\n\tCaught Unexpected Exception: [{0}]: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
            }
            Assert.IsTrue(exception, "[{0}] should have been caught.", expectedException);
            Console.WriteLine();
         }

         #endregion // UnauthorizedAccessException

         string path = isLocal ? SysRoot : Path.LocalToUnc(SysRoot);

         Console.WriteLine("\nInput Directory Path: [{0}]\n", path);
         Console.WriteLine("\tGet directories, using \"SearchOption.{0}\"\n", searchOption);

         StopWatcher(true);
         foreach (string folder in Directory.GetDirectories(path, searchPattern, searchOption))
            Console.WriteLine("\t#{0:000}\t[{1}]", ++cnt, folder);

         Console.WriteLine();
         Console.WriteLine(Reporter());
         Assert.IsTrue(cnt > 0, "Nothing was enumerated.");
         Console.WriteLine();
      }

      #endregion // DumpGetDirectories

      #region DumpGetDrives

      private void DumpGetDrives(bool enumerate)
      {
         Console.WriteLine("\nIf you are missing drives, please see this topic: https://alphafs.codeplex.com/discussions/397693 \n");

         int cnt = 0;
         StopWatcher(true);
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
         Console.WriteLine("\n{0}", Reporter());
      }

      #endregion // DumpGetDrives

      #region DumpGetFiles

      private void DumpGetFiles(bool isLocal)
      {
         #region Setup

         Console.WriteLine("\n=== TEST {0} ===", isLocal ? Local : Network);

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

         #endregion //Setup

         #region DirectoryNotFoundException (Local) / IOException (Network)

         expectedLastError = (int)(isLocal ? Win32Errors.ERROR_PATH_NOT_FOUND : Win32Errors.ERROR_BAD_NET_NAME);
         expectedException = isLocal ? "System.IO.DirectoryNotFoundException" : "System.IO.IOException";
         exception = false;
         try
         {
            Console.WriteLine("\nCatch: [{0}]: The specified path is not found or is invalid (for example, it is on an unmapped drive).", expectedException);

            string nonExistingPath = letter + folderSource;
            if (!isLocal) nonExistingPath = Path.LocalToUnc(nonExistingPath);

            Directory.GetFiles(nonExistingPath);
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
               Console.WriteLine("\n\tCaught Unexpected Exception: [{0}]: [{1}]", exceptionTypeName, ex.Message.Replace(Environment.NewLine, "  "));
         }
         Assert.IsTrue(exception, "[{0}] should have been caught.", expectedException);
         Console.WriteLine();

         #endregion // DirectoryNotFoundException (Local) / IOException (Network)

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
               Console.WriteLine("\nCatch: [{0}]: Path is a file name.", expectedException);

               Directory.GetFiles(tempPath);
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
               Console.WriteLine("\n\tCaught Unexpected Exception: [{0}]: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
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

         tempPath = Path.Combine(SysRoot, "CSC");
         if (!isLocal) tempPath = Path.LocalToUnc(tempPath);

         if (Directory.Exists(tempPath))
         {
            expectedLastError = (int)Win32Errors.ERROR_ACCESS_DENIED;
            expectedException = "System.UnauthorizedAccessException";
            exception = false;
            try
            {
               Console.WriteLine("\nCatch: [{0}]: The caller does not have the required permission.", expectedException);

               Directory.GetFiles(tempPath, searchPattern, SearchOption.AllDirectories).Any();
            }
            catch (UnauthorizedAccessException ex)
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
                  Console.WriteLine("\n\tCaught Unexpected Exception: [{0}]: [{1}]", exceptionTypeName, ex.Message.Replace(Environment.NewLine, "  "));
            }
            catch (Exception ex)
            {
               Console.WriteLine("\n\tCaught Unexpected Exception: [{0}]: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
            }
            Assert.IsTrue(exception, "[{0}] should have been caught.", expectedException);
            Console.WriteLine();
         }

         #endregion // UnauthorizedAccessException

         string path = isLocal ? SysRoot : Path.LocalToUnc(SysRoot);

         Console.WriteLine("\nInput Directory Path: [{0}]\n", path);
         Console.WriteLine("\tGet files, using \"SearchOption.{0}\"\n", searchOption);

         StopWatcher(true);
         foreach (string file in Directory.GetFiles(path, searchPattern, searchOption))
            Console.WriteLine("\t#{0:000}\t[{1}]", ++cnt, file);

         Console.WriteLine();
         Console.WriteLine(Reporter());
         Assert.IsTrue(cnt > 0, "Nothing was enumerated.");
         Console.WriteLine();
      }

      #endregion // DumpGetFiles

      #region DumpGetXxxTime

      private void DumpGetXxxTime(bool isLocal)
      {
         #region Setup

         Console.WriteLine("\n=== TEST {0} ===", isLocal ? Local : Network);
         string path = isLocal ? SysRoot32 : Path.LocalToUnc(SysRoot32);

         Console.WriteLine("\nInput Directory Path: [{0}]\n", path);

         #endregion // Setup

         StopWatcher(true);

         #region GetCreationTimeXxx

         DateTime actual = Directory.GetCreationTime(path);
         DateTime expected = System.IO.Directory.GetCreationTime(path);
         Console.WriteLine("\tGetCreationTime()     : [{0}]    System.IO: [{1}]", actual, expected);
         Assert.AreEqual(expected, actual, "GetCreationTime()");

         actual = Directory.GetCreationTimeUtc(path);
         expected = System.IO.Directory.GetCreationTimeUtc(path);
         Console.WriteLine("\tGetCreationTimeUtc()  : [{0}]    System.IO: [{1}]\n", actual, expected);
         Assert.AreEqual(expected, actual, "GetCreationTimeUtc()");

         #endregion // GetCreationTimeXxx

         #region GetLastAccessTimeXxx

         actual = Directory.GetLastAccessTime(path);
         expected = System.IO.Directory.GetLastAccessTime(path);
         Console.WriteLine("\tGetLastAccessTime()   : [{0}]    System.IO: [{1}]", actual, expected);
         Assert.AreEqual(expected, actual, "GetLastAccessTime()");

         actual = Directory.GetLastAccessTimeUtc(path);
         expected = System.IO.Directory.GetLastAccessTimeUtc(path);
         Console.WriteLine("\tGetLastAccessTimeUtc(): [{0}]    System.IO: [{1}]\n", actual, expected);
         Assert.AreEqual(expected, actual, "GetLastAccessTimeUtc()");

         #endregion // GetLastAccessTimeXxx

         #region GetLastWriteTimeXxx

         actual = Directory.GetLastWriteTime(path);
         expected = System.IO.Directory.GetLastWriteTime(path);
         Console.WriteLine("\tGetLastWriteTime()    : [{0}]    System.IO: [{1}]", actual, expected);
         Assert.AreEqual(expected, actual, "GetLastWriteTime()");

         actual = Directory.GetLastWriteTimeUtc(path);
         expected = System.IO.Directory.GetLastWriteTimeUtc(path);
         Console.WriteLine("\tGetLastWriteTimeUtc() : [{0}]    System.IO: [{1}]\n", actual, expected);
         Assert.AreEqual(expected, actual, "GetLastWriteTimeUtc()");

         #endregion // GetLastWriteTimeXxx


         #region GetChangeTimeXxx

         Console.WriteLine("\tGetChangeTime()       : [{0}]    System.IO: [N/A]", Directory.GetChangeTime(path));
         Console.WriteLine("\tGetChangeTimeUtc()    : [{0}]    System.IO: [N/A]", Directory.GetChangeTimeUtc(path));

         #endregion GetChangeTimeXxx

         Console.WriteLine();
         Console.WriteLine(Reporter());
         Console.WriteLine();

         #region Trigger GetChangeTimeXxx

         // We can not compare ChangeTime against .NET because it does not exist.
         // Creating a directory and renaming it triggers ChangeTime, so test for that.

         path = Path.GetTempPath("Directory-GetChangeTimeXxx()-directory-" + Path.GetRandomFileName());
         if (!isLocal) path = Path.LocalToUnc(path);

         DirectoryInfo di = new DirectoryInfo(path);
         di.Create();
         string fileName = di.Name;

         DateTime lastAccessTimeActual = Directory.GetLastAccessTime(path);
         DateTime lastAccessTimeUtcActual = Directory.GetLastAccessTimeUtc(path);

         DateTime changeTimeActual = Directory.GetChangeTime(path);
         DateTime changeTimeUtcActual = Directory.GetChangeTimeUtc(path);

         Console.WriteLine("\nTesting ChangeTime on a temp directory.");
         Console.WriteLine("\nInput Directory Path: [{0}]\n", path);
         Console.WriteLine("\tGetChangeTime()       : [{0}]\t", changeTimeActual);
         Console.WriteLine("\tGetChangeTimeUtc()    : [{0}]\t", changeTimeUtcActual);

         di.MoveTo(di.FullName.Replace(fileName, fileName + "-Renamed"));

         // Pause for at least a second so that the difference in time can be seen.
         int sleep = new Random().Next(2000, 4000);
         Thread.Sleep(sleep);

         di.MoveTo(di.FullName.Replace(fileName + "-Renamed", fileName));

         DateTime lastAccessTimeExpected = Directory.GetLastAccessTime(path);
         DateTime lastAccessTimeUtcExpected = Directory.GetLastAccessTimeUtc(path);
         DateTime changeTimeExpected = Directory.GetChangeTime(path);
         DateTime changeTimeUtcExpected = Directory.GetChangeTimeUtc(path);

         Console.WriteLine("\nTrigger ChangeTime by renaming the directory.");
         Console.WriteLine("For Unit Test, ChangeTime should differ approximately: [{0}] seconds.\n", sleep / 1000);
         Console.WriteLine("\tGetChangeTime()       : [{0}]\t", changeTimeExpected);
         Console.WriteLine("\tGetChangeTimeUtc()    : [{0}]\t\n", changeTimeUtcExpected);


         Assert.AreNotEqual(changeTimeActual, changeTimeExpected);
         Assert.AreNotEqual(changeTimeUtcActual, changeTimeUtcExpected);

         Assert.AreEqual(lastAccessTimeExpected, lastAccessTimeActual);
         Assert.AreEqual(lastAccessTimeUtcExpected, lastAccessTimeUtcActual);

         #endregion // Trigger GetChangeTimeXxx

         di.Delete();
         di.Refresh(); // Must Refresh() to get actual state.
         Assert.IsFalse(di.Exists, "Cleanup failed: Directory should have been removed.");
         Console.WriteLine();
      }

      #endregion // DumpGetXxxTime

      #region DumpGetFileSystemEntries

      private void DumpGetFileSystemEntries(bool isLocal)
      {
         #region Setup

         Console.WriteLine("\n=== TEST {0} ===", isLocal ? Local : Network);

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

         #endregion //Setup

         #region DirectoryNotFoundException (Local) / IOException (Network)

         expectedLastError = (int)(isLocal ? Win32Errors.ERROR_PATH_NOT_FOUND : Win32Errors.ERROR_BAD_NET_NAME);
         expectedException = isLocal ? "System.IO.DirectoryNotFoundException" : "System.IO.IOException";
         exception = false;
         try
         {
            Console.WriteLine("\nCatch: [{0}]: The specified path is invalid (for example, it is on an unmapped drive).", expectedException);

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
               Console.WriteLine("\n\tCaught Unexpected Exception: [{0}]: [{1}]", exceptionTypeName, ex.Message.Replace(Environment.NewLine, "  "));
         }
         Assert.IsTrue(exception, "[{0}] should have been caught.", expectedException);
         Console.WriteLine();

         #endregion // DirectoryNotFoundException (Local) / IOException (Network)

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
               Console.WriteLine("\nCatch: [{0}]: Path is a file name.", expectedException);

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
               Console.WriteLine("\n\tCaught Unexpected Exception: [{0}]: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
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

         tempPath = Path.Combine(SysRoot, "CSC");
         if (!isLocal) tempPath = Path.LocalToUnc(tempPath);

         if (Directory.Exists(tempPath))
         {
            expectedLastError = (int)Win32Errors.ERROR_ACCESS_DENIED;
            expectedException = "System.UnauthorizedAccessException";
            exception = false;
            try
            {
               Console.WriteLine("\nCatch: [{0}]: The caller does not have the required permission.", expectedException);

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
               Console.WriteLine("\n\tCaught Unexpected Exception: [{0}]: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
            }
            Assert.IsTrue(exception, "[{0}] should have been caught.", expectedException);
            Console.WriteLine();
         }

         #endregion // UnauthorizedAccessException

         string path = isLocal ? SysRoot : Path.LocalToUnc(SysRoot);

         Console.WriteLine("\nInput Directory Path: [{0}]\n", path);
         Console.WriteLine("\tGet FileSystemEntries, using \"SearchOption.{0}\"\n", searchOption);

         StopWatcher(true);
         foreach (string folder in Directory.GetFileSystemEntries(path, searchPattern, searchOption))
            Console.WriteLine("\t#{0:000}\t[{1}]", ++cnt, folder);

         Console.WriteLine();
         Console.WriteLine(Reporter());
         Assert.IsTrue(cnt > 0, "Nothing was enumerated.");
         Console.WriteLine();
      }

      #endregion // DumpGetFileSystemEntries

      #region DumpGetProperties

      private void DumpGetProperties(bool isLocal)
      {
         Console.WriteLine("\n=== TEST {0} ===", isLocal ? Local : Network);
         string path = isLocal ? SysRoot : Path.LocalToUnc(SysRoot);

         Console.WriteLine("\n\tAggregated properties of file system objects from Directory: [{0}]\n", path);

         StopWatcher(true);
         Dictionary<string, long> props = Directory.GetProperties(path, DirectoryEnumerationOptions.FilesAndFolders | DirectoryEnumerationOptions.Recursive | DirectoryEnumerationOptions.ContinueOnException);
         string report = Reporter();

         long total = props["Total"];
         long file = props["File"];
         long size = props["Size"];
         int cnt = 0;

         foreach (var key in props.Keys)
            Console.WriteLine("\t\t#{0:000}\t{1, -17} = [{2}]", ++cnt, key, props[key]);

         Console.WriteLine("\n\t{0}", report);

         Assert.IsTrue(cnt > 0, "Nothing was enumerated.");
         Assert.IsTrue(total > 0, "0 Objects.");
         Assert.IsTrue(file > 0, "0 Files.");
         Assert.IsTrue(size > 0, "0 Size.");
         Console.WriteLine();
      }

      #endregion // DumpGetProperties

      #region DumpMove

      private void DumpMove(bool isLocal)
      {
         #region Setup

         Console.WriteLine("\n=== TEST {0} ===", isLocal ? Local : Network);
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
               Console.WriteLine("\nCatch: [{0}]: The caller does not have the required permission.", expectedException);

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
               Console.WriteLine("\n\tCaught Unexpected Exception: [{0}]: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
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
               Console.WriteLine("\nCatch: [{0}]: The path specified by sourceDirName is invalid (for example, it is on an unmapped drive).", expectedException);
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
               Console.WriteLine("\n\tCaught Unexpected Exception: [{0}]: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
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
               Console.WriteLine("\nCatch: [{0}]: The sourceDirName and destDirName parameters refer to the same file or directory.", expectedException);
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
               Console.WriteLine("\n\tCaught Unexpected Exception: [{0}]: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
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
               Console.WriteLine("\nCatch: [{0}]: An attempt was made to move a directory to a different volume. ", expectedException);
               Directory.Move(SysDrive + @"\" + folderSource, otherDisk);
            }
            catch (IOException ex)
            {
               Assert.IsTrue(ex.Message.StartsWith("(" + expectedLastError + ")"), string.Format("Expected Win32Exception error is: [{0}]", expectedLastError));

               exception = true;
               Console.WriteLine("\n\t[{0}]: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
            }
            catch (Exception ex)
            {
               Console.WriteLine("\n\tCaught Unexpected Exception: [{0}]: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
            }
            Assert.IsTrue(exception, "[{0}] should have been caught.", expectedException);
            Console.WriteLine();

            #endregion // IOException #2

            #region Move

            CreateDirectoriesAndFiles(tempPathSource0, 10, true);
            Directory.Copy(tempPathSource0, otherDisk, CopyOptions.FailIfExists);

            Dictionary<string, long> props = Directory.GetProperties(otherDisk, DirectoryEnumerationOptions.Recursive);
            long sourceFolder = props["Directory"];
            long sourceFile = props["File"];
            long sourceSize = props["Size"];

            Console.WriteLine("\nMove from Source Path: [{0}]", otherDisk);
            Console.WriteLine("\tTotal Directories: [{0}] Files: [{1}] Size: [{2}]", sourceFolder, sourceFile, Utils.UnitSizeToText(sourceSize));

            StopWatcher(true);
            Directory.Move(otherDisk, tempPathSource, MoveOptions.CopyAllowed);
            report = Reporter();

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
               Console.WriteLine("\n\tCaught Unexpected Exception: [{0}]: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
            }
            Assert.IsTrue(exception, "[{0}] should have been caught.", expectedException);
            Console.WriteLine();

            #endregion // IOException

            // Overwrite.
            StopWatcher(true);
            Directory.Move(otherDisk, tempPathSource, MoveOptions.CopyAllowed | MoveOptions.ReplaceExisting);
            report = Reporter();

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

      #endregion // DumpMove

      #region DumpSetTimestamps

      private void DumpSetTimestamps(bool isLocal)
      {
         Console.WriteLine("\n=== TEST {0} ===", isLocal ? Local : Network);
         string path = Path.Combine(Path.GetTempPath(), "Directory.SetTimestamps()-directory-" + Path.GetRandomFileName());
         if (!isLocal) path = Path.LocalToUnc(path);

         Console.WriteLine("\nInput Directory Path: [{0}]", path);

         Directory.CreateDirectory(path);

         Thread.Sleep(new Random().Next(250, 500));
         int seed = (int)DateTime.Now.Ticks & 0x0000FFFF;
         DateTime creationTime = new DateTime(new Random(seed).Next(1971, 2071), new Random(seed).Next(1, 12), new Random(seed).Next(1, 28), new Random(seed).Next(0, 23), new Random(seed).Next(0, 59), new Random(seed).Next(0, 59));

         Thread.Sleep(new Random().Next(250, 500));
         seed += (int)DateTime.Now.Ticks & 0x0000FFFF;
         DateTime lastAccessTime = new DateTime(new Random(seed).Next(1971, 2071), new Random(seed).Next(1, 12), new Random(seed).Next(1, 28), new Random(seed).Next(0, 23), new Random(seed).Next(0, 59), new Random(seed).Next(0, 59));

         Thread.Sleep(new Random().Next(250, 500));
         seed += (int)DateTime.Now.Ticks & 0x0000FFFF;
         DateTime lastWriteTime = new DateTime(new Random(seed).Next(1971, 2071), new Random(seed).Next(1, 12), new Random(seed).Next(1, 28), new Random(seed).Next(0, 23), new Random(seed).Next(0, 59), new Random(seed).Next(0, 59));

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




         creationTime = creationTime.ToUniversalTime();
         lastAccessTime = lastAccessTime.ToUniversalTime();
         lastWriteTime = lastWriteTime.ToUniversalTime();

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

         Directory.Delete(path);
         Assert.IsTrue(!Directory.Exists(path), "Cleanup failed: Directory should have been removed.");
         Console.WriteLine();
      }

      #endregion // DumpSetTimestamps

      #region DumpSetXxxTime

      private void DumpSetXxxTime(bool isLocal)
      {
         Console.WriteLine("\n=== TEST {0} ===", isLocal ? Local : Network);
         string path = Path.Combine(Path.GetTempPath(), "Directory.SetCreationTime()-" + Path.GetRandomFileName());
         if (!isLocal) path = Path.LocalToUnc(path);

         Console.WriteLine("\nInput Path: [{0}]", path);

         Directory.CreateDirectory(path);

         #region SetCreationTime/Utc
         Thread.Sleep(new Random().Next(250, 500));
         int seed = (int)DateTime.Now.Ticks & 0x0000FFFF;
         DateTime creationTime = new DateTime(new Random(seed).Next(1971, 2071), new Random(seed).Next(1, 12), new Random(seed).Next(1, 28), new Random(seed).Next(0, 23), new Random(seed).Next(0, 59), new Random(seed).Next(1, 59));
         Console.WriteLine("\n\tSetCreationTime() to: [{0} {1}]", creationTime, creationTime.ToLongTimeString());
         Directory.SetCreationTime(path, creationTime);
         DateTime actual = Directory.GetCreationTime(path);
         System.IO.Directory.SetCreationTime(path, creationTime);
         DateTime expected = System.IO.Directory.GetCreationTime(path);
         Console.WriteLine("\t\tAlphaFS  : [{0}]\n\t\tSystem.IO: [{1}]", actual, expected);
         Assert.AreEqual(expected, actual, "AlphaFS != System.IO");


         Thread.Sleep(new Random().Next(250, 500));
         seed += (int)DateTime.Now.Ticks & 0x0000FFFF;
         creationTime = new DateTime(new Random(seed).Next(1971, 2071), new Random(seed).Next(1, 12), new Random(seed).Next(1, 28), new Random(seed).Next(0, 23), new Random(seed).Next(0, 59), new Random(seed).Next(1, 59));
         Console.WriteLine("\n\tSetCreationTimeUtc() to: [{0} {1}]", creationTime, creationTime.ToLongTimeString());
         Directory.SetCreationTimeUtc(path, creationTime);
         actual = Directory.GetCreationTimeUtc(path);
         System.IO.Directory.SetCreationTimeUtc(path, creationTime);
         expected = System.IO.Directory.GetCreationTimeUtc(path);
         Console.WriteLine("\t\tAlphaFS  : [{0}]\n\t\tSystem.IO: [{1}]", actual, expected);
         Assert.AreEqual(expected, actual, "AlphaFS != System.IO");
         #endregion // SetCreationTime/Utc

         #region SetLastAccessTime/Utc
         Thread.Sleep(new Random().Next(250, 500));
         seed += (int)DateTime.Now.Ticks & 0x0000FFFF;
         DateTime lastAccessTime = new DateTime(new Random(seed).Next(1971, 2071), new Random(seed).Next(1, 12), new Random(seed).Next(1, 28), new Random(seed).Next(0, 23), new Random(seed).Next(0, 59), new Random(seed).Next(1, 59));
         Console.WriteLine("\n\tSetLastAccessTime() to: [{0} {1}]", lastAccessTime, lastAccessTime.ToLongTimeString());
         Directory.SetLastAccessTime(path, lastAccessTime);
         actual = Directory.GetLastAccessTime(path);
         System.IO.Directory.SetLastAccessTime(path, lastAccessTime);
         expected = System.IO.Directory.GetLastAccessTime(path);
         Console.WriteLine("\t\tAlphaFS  : [{0}]\n\t\tSystem.IO: [{1}]", actual, expected);
         Assert.AreEqual(expected, actual, "AlphaFS != System.IO");


         Thread.Sleep(new Random().Next(250, 500));
         seed += (int)DateTime.Now.Ticks & 0x0000FFFF;
         lastAccessTime = new DateTime(new Random(seed).Next(1971, 2071), new Random(seed).Next(1, 12), new Random(seed).Next(1, 28), new Random(seed).Next(0, 23), new Random(seed).Next(0, 59), new Random(seed).Next(1, 59));
         Console.WriteLine("\n\tSetLastAccessTimeUtc() to: [{0} {1}]", lastAccessTime, lastAccessTime.ToLongTimeString());
         Directory.SetLastAccessTimeUtc(path, lastAccessTime);
         actual = Directory.GetLastAccessTimeUtc(path);
         System.IO.Directory.SetLastAccessTimeUtc(path, lastAccessTime);
         expected = System.IO.Directory.GetLastAccessTimeUtc(path);
         Console.WriteLine("\t\tAlphaFS  : [{0}]\n\t\tSystem.IO: [{1}]", actual, expected);
         Assert.AreEqual(expected, actual, "AlphaFS != System.IO");
         #endregion // SetLastAccessTime/Utc

         #region SetLastWriteTime/Utc
         Thread.Sleep(new Random().Next(250, 500));
         seed += (int)DateTime.Now.Ticks & 0x0000FFFF;
         DateTime lastWriteTime = new DateTime(new Random(seed).Next(1971, 2071), new Random(seed).Next(1, 12), new Random(seed).Next(1, 28), new Random(seed).Next(0, 23), new Random(seed).Next(0, 59), new Random(seed).Next(1, 59));
         Console.WriteLine("\n\tSetLastWriteTime() to: [{0} {1}]", lastWriteTime, lastWriteTime.ToLongTimeString());
         Directory.SetLastWriteTime(path, lastWriteTime);
         actual = Directory.GetLastWriteTime(path);
         System.IO.Directory.SetLastWriteTime(path, lastWriteTime);
         expected = System.IO.Directory.GetLastWriteTime(path);
         Console.WriteLine("\t\tAlphaFS  : [{0}]\n\t\tSystem.IO: [{1}]", actual, expected);
         Assert.AreEqual(expected, actual, "AlphaFS != System.IO");


         Thread.Sleep(new Random().Next(250, 500));
         seed += (int)DateTime.Now.Ticks & 0x0000FFFF;
         lastWriteTime = new DateTime(new Random(seed).Next(1971, 2071), new Random(seed).Next(1, 12), new Random(seed).Next(1, 28), new Random(seed).Next(0, 23), new Random(seed).Next(0, 59), new Random(seed).Next(1, 59));
         Console.WriteLine("\n\tSetLastWriteTimeUtc() to: [{0} {1}]", lastWriteTime, lastWriteTime.ToLongTimeString());
         Directory.SetLastWriteTimeUtc(path, lastWriteTime);
         actual = Directory.GetLastWriteTimeUtc(path);
         System.IO.Directory.SetLastWriteTimeUtc(path, lastWriteTime);
         expected = System.IO.Directory.GetLastWriteTimeUtc(path);
         Console.WriteLine("\t\tAlphaFS  : [{0}]\n\t\tSystem.IO: [{1}]", actual, expected);
         Assert.AreEqual(expected, actual, "AlphaFS != System.IO");
         #endregion // SetLastWriteTime/Utc

         Directory.Delete(path);
         Assert.IsTrue(!Directory.Exists(path));

         Console.WriteLine("\n");
      }

      #endregion // DumpSetXxxTime

      #region DumpTransferTimestamps

      private void DumpTransferTimestamps(bool isLocal)
      {
         Console.WriteLine("\n=== TEST {0} ===", isLocal ? Local : Network);
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

      #endregion // DumpTransferTimestamps

      #region HasInheritedPermissions

      private static bool HasInheritedPermissions(string path)
      {
         DirectorySecurity acl = Directory.GetAccessControl(path);
         return acl.GetAccessRules(false, true, typeof(SecurityIdentifier)).Count > 0;
      }

      #endregion // HasInheritedPermissions


      #region Create directory with trailing dot/space

      private void DumpDirectoryTrailingDotSpace(bool isLocal)
      {
         #region Setup

         Console.WriteLine("\n=== TEST {0} ===\n", isLocal ? Local : Network);
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

            StopWatcher(true);

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

            Console.WriteLine("\tClass DirectoryInfo()\t{0}", Reporter());

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

      #endregion // Create directory with trailing dot/space

      #endregion // Unit Tests

      #region Unit Test Callers

      #region .NET

      #region CreateDirectory

      [TestMethod]
      public void CreateDirectory()
      {
         Console.WriteLine("Directory.CreateDirectory()");

         DumpCreateDirectory(true);
         DumpCreateDirectory(false);
      }

      #endregion // CreateDirectory

      #region Delete

      [TestMethod]
      public void Delete()
      {
         Console.WriteLine("Directory.Delete()");

         TestDelete(true);
         TestDelete(false);
      }

      #endregion // Delete

      #region EnumerateDirectories

      [TestMethod]
      public void EnumerateDirectories()
      {
         Console.WriteLine("Directory.EnumerateDirectories()");

         DumpEnumerateDirectories(true);
         DumpEnumerateDirectories(false);
      }

      #endregion // EnumerateDirectories

      #region EnumerateFiles

      [TestMethod]
      public void EnumerateFiles()
      {
         Console.WriteLine("Directory.EnumerateFiles()");

         DumpEnumerateFiles(true);
         DumpEnumerateFiles(false);
      }

      #endregion // EnumerateFiles

      #region EnumerateFileSystemEntries

      [TestMethod]
      public void EnumerateFileSystemEntries()
      {
         Console.WriteLine("Directory.EnumerateFileSystemEntries()");

         DumpEnumerateFileSystemEntries(true);
         DumpEnumerateFileSystemEntries(false);
      }

      #endregion // EnumerateFileSystemEntries

      #region Exists

      [TestMethod]
      public void Exists()
      {
         Console.WriteLine("Directory.Exists()");

         DumpExists(true);
         DumpExists(false);
      }

      #endregion // Exists

      #region GetAccessControl

      [TestMethod]
      public void GetAccessControl()
      {
         Console.WriteLine("Directory.GetAccessControl()");

         DumpGetAccessControl(true);
         DumpGetAccessControl(false);
      }

      #endregion // GetAccessControl

      #region GetCreationTime

      [TestMethod]
      public void GetCreationTime()
      {
         Console.WriteLine("Directory.GetXxxTime()");

         DumpGetXxxTime(true);
         DumpGetXxxTime(false);
      }

      #endregion // GetCreationTime

      #region GetCreationTimeUtc

      [TestMethod]
      public void GetCreationTimeUtc()
      {
         Console.WriteLine("Directory.GetCreationTimeUtc()");
         Console.WriteLine("\nPlease see unit test: GetCreationTime()");
      }

      #endregion // GetCreationTimeUtc

      #region GetCurrentDirectory (.NET)

      [TestMethod]
      public void NET_GetCurrentDirectory()
      {
         Console.WriteLine("Directory.GetCurrentDirectory()");
         Console.WriteLine("\nThe .NET method is used.");
      }

      #endregion // GetCurrentDirectory (.NET)

      #region GetDirectories

      [TestMethod]
      public void GetDirectories()
      {
         Console.WriteLine("Directory.GetDirectories()");

         DumpGetDirectories(true);
         DumpGetDirectories(false);
      }

      #endregion // GetDirectories

      #region GetDirectoryRoot

      [TestMethod]
      public void GetDirectoryRoot()
      {
         Console.WriteLine("Directory.GetDirectoryRoot()");

         int pathCnt = 0;
         bool allOk = true;
         int errorCnt = 0;

         StopWatcher(true);
         foreach (string path in InputPaths)
         {
            string method = null;

            try
            {
               method = "AlphaFS";
               string actual = Directory.GetDirectoryRoot(path);

               method = "System.IO";
               string expected = System.IO.Directory.GetDirectoryRoot(path);

               Console.WriteLine("\n\t#{0:000}\tInput Path: [{1}]\n\t\tAlphaFS   : [{2}]\n\t\tSystem.IO : [{3}]", ++pathCnt, path, actual, expected);
               Assert.AreEqual(expected, actual);

               ++pathCnt;
            }
            catch (ArgumentException ex)
            {
               Console.WriteLine("\n\tCaught ArgumentException: Method: [{0}]: [{1}]: [{2}", method, ex.Message.Replace(Environment.NewLine, "  "), path);
            }
            catch (Exception ex)
            {
               Console.WriteLine("\tCaught Exception: Method: [{0}] [{1}]", method, ex.Message.Replace(Environment.NewLine, "  "));
               allOk = false;
               errorCnt++;

            }
         }
         Console.WriteLine("\n\t{0}", Reporter(true));

         Assert.IsTrue(pathCnt > 0);

         Assert.AreEqual(true, allOk, "Encountered: [{0}] paths where AlphaFS != System.IO", errorCnt);
      }

      #endregion // GetDirectoryRoot

      #region GetFiles

      [TestMethod]
      public void GetFiles()
      {
         Console.WriteLine("Directory.GetFiles()");

         DumpGetFiles(true);
         DumpGetFiles(false);
      }

      #endregion // GetFiles

      #region GetFileSystemEntries

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

      #endregion // GetFileSystemEntries

      #region GetLastAccessTime

      [TestMethod]
      public void GetLastAccessTime()
      {
         Console.WriteLine("Directory.GetLastAccessTime()");
         Console.WriteLine("\nPlease see unit test: GetCreationTime()");
      }

      #endregion // GetLastAccessTime

      #region GetLastAccessTimeUtc

      [TestMethod]
      public void GetLastAccessTimeUtc()
      {
         Console.WriteLine("Directory.GetLastAccessTimeUtc()");
         Console.WriteLine("\nPlease see unit test: GetCreationTime()");
      }

      #endregion // GetLastAccessTimeUtc

      #region GetLastWriteTime

      [TestMethod]
      public void GetLastWriteTime()
      {
         Console.WriteLine("Directory.GetLastWriteTime()");
         Console.WriteLine("\nPlease see unit test: GetCreationTime()");
      }

      #endregion // GetLastWriteTime

      #region GetLastWriteTimeUtc

      [TestMethod]
      public void GetLastWriteTimeUtc()
      {
         Console.WriteLine("Directory.GetLastWriteTimeUtc()");
         Console.WriteLine("\nPlease see unit test: GetCreationTime()");
      }

      #endregion // GetLastWriteTimeUtc

      #region GetLogicalDrives

      [TestMethod]
      public void GetLogicalDrives()
      {
         Console.WriteLine("Directory.GetLogicalDrives()");

         DumpGetDrives(false);
      }

      #endregion // GetLogicalDrives

      #region GetParent

      [TestMethod]
      public void GetParent()
      {
         Console.WriteLine("Directory.GetParent()");

         int pathCnt = 0;
         bool allOk = true;
         int errorCnt = 0;

         StopWatcher(true);
         foreach (string path in InputPaths)
         {
            string method = null;

            try
            {
               method = "AlphaFS";
               DirectoryInfo diActual = Directory.GetParent(path);

               method = "System.IO";
               System.IO.DirectoryInfo diExpected = System.IO.Directory.GetParent(path);

               if (diActual == null || diExpected == null)
               {
                  Console.WriteLine("\n\t#{0:000}\tInput Path: [{1}]\n\t\tAlphaFS   : [{2}]\n\t\tSystem.IO : [{3}]", ++pathCnt, path, diActual, diExpected);
                  Assert.AreEqual(diActual, diExpected);
               }
               else
               {
                  method = "AlphaFS";
                  string actual = diActual.FullName;

                  method = "System.IO";
                  string expected = diExpected.FullName;

                  Console.WriteLine("\n\t#{0:000}\tInput Path: [{1}]\n\t\tAlphaFS   : [{2}]\n\t\tSystem.IO : [{3}]", ++pathCnt, path, diActual.FullName, diExpected.FullName);
                  Assert.AreEqual(expected, actual);
               }
            }
            catch (ArgumentException ex)
            {
               Console.WriteLine("\n\tCaught ArgumentException: Method: [{0}]: [{1}]: [{2}", method, ex.Message.Replace(Environment.NewLine, "  "), path);
            }
            catch (Exception ex)
            {
               Console.WriteLine("\tCaught Exception: Method: [{0}] [{1}]", method, ex.Message.Replace(Environment.NewLine, "  "));
               allOk = false;
               errorCnt++;
            }
         }
         Console.WriteLine("\n\t{0}", Reporter(true));

         Assert.AreEqual(true, allOk, "Encountered: [{0}] paths where AlphaFS != System.IO", errorCnt);
      }

      #endregion // GetParent

      #region Move

      [TestMethod]
      public void Move()
      {
         Console.WriteLine("Directory.Move()");

         DumpMove(true);
         DumpMove(false);
      }

      #endregion // Move

      #region SetAccessControl

      [TestMethod]
      public void SetAccessControl()
      {
         Console.WriteLine("Directory.SetAccessControl()");

         //string path = SysDrive + @"\AlphaDirectory-" + Path.GetRandomFileName();
         string path = Path.Combine(Path.GetTempPath(), "Directory.GetAccessControl()-" + Path.GetRandomFileName());
         string pathAlpha = path;
         Directory.CreateDirectory(path);

         Console.WriteLine("\n\tDirectory: [{0}]", path);

         // Initial read.
         Console.WriteLine("\n\tInitial read.");
         DirectorySecurity dsAlpha = Directory.GetAccessControl(pathAlpha, AccessControlSections.Access);
         DirectorySecurity dsSystem = System.IO.Directory.GetAccessControl(path, AccessControlSections.Access);
         AuthorizationRuleCollection accessRulesSystem = dsSystem.GetAccessRules(true, true, typeof(NTAccount));
         StopWatcher(true);
         AuthorizationRuleCollection accessRulesAlpha = dsAlpha.GetAccessRules(true, true, typeof(NTAccount));
         Console.WriteLine("\t\tDirectory.GetAccessControl() rules found: [{0}]\n\t{1}", accessRulesAlpha.Count, Reporter());
         Assert.AreEqual(accessRulesSystem.Count, accessRulesAlpha.Count);

         // Sanity check.
         DumpAccessRules(1, dsSystem, dsAlpha);

         // Remove inherited properties.
         // Passing true for first parameter protects the new permission from inheritance, and second parameter removes the existing inherited permissions.
         Console.WriteLine("\n\tRemove inherited properties and persist it.");
         dsAlpha.SetAccessRuleProtection(true, false);

         // Re-read, using instance methods.
         System.IO.DirectoryInfo diSystem = new System.IO.DirectoryInfo(Path.LocalToUnc(path));
         DirectoryInfo diAlpha = new DirectoryInfo(Path.LocalToUnc(path));

         dsSystem = diSystem.GetAccessControl(AccessControlSections.Access);
         dsAlpha = diAlpha.GetAccessControl(AccessControlSections.Access);

         // Sanity check.
         DumpAccessRules(2, dsSystem, dsAlpha);

         // Restore inherited properties.
         Console.WriteLine("\n\tRestore inherited properties and persist it.");
         dsAlpha.SetAccessRuleProtection(false, true);

         // Re-read.
         dsSystem = System.IO.Directory.GetAccessControl(path, AccessControlSections.Access);
         dsAlpha = Directory.GetAccessControl(pathAlpha, AccessControlSections.Access);

         // Sanity check.
         DumpAccessRules(3, dsSystem, dsAlpha);

         diAlpha.Delete();
         diAlpha.Refresh(); // Must Refresh() to get actual state.
         Assert.IsFalse(diAlpha.Exists);
      }

      #endregion // SetAccessControl

      #region SetCreationTime

      [TestMethod]
      public void SetCreationTime()
      {
         Console.WriteLine("Directory.SetXxxTime()");

         DumpSetXxxTime(true);
         DumpSetXxxTime(false);
      }

      #endregion // SetCreationTime

      #region SetCreationTimeUtc

      [TestMethod]
      public void SetCreationTimeUtc()
      {
         Console.WriteLine("Directory.SetCreationTimeUtc()");
         Console.WriteLine("\nPlease see unit test: SetCreationTime()");
      }

      #endregion // SetCreationTimeUtc

      #region SetCurrentDirectory (.NET)

      [TestMethod]
      public void NET_SetCurrentDirectory()
      {
         Console.WriteLine("Directory.SetCurrentDirectory()");
         Console.WriteLine("\nThe .NET method is used.");
      }

      #endregion // SetCurrentDirectory (.NET)

      #region SetLastAccessTime

      [TestMethod]
      public void SetLastAccessTime()
      {
         Console.WriteLine("Directory.SetLastAccessTime()");
         Console.WriteLine("\nPlease see unit test: SetCreationTime()");
      }

      #endregion // SetLastAccessTime

      #region SetLastAccessTimeUtc

      [TestMethod]
      public void SetLastAccessTimeUtc()
      {
         Console.WriteLine("Directory.SetLastAccessTimeUtc()");
         Console.WriteLine("\nPlease see unit test: SetCreationTime()");
      }

      #endregion // SetLastAccessTimeUtc

      #region SetLastWriteTime

      [TestMethod]
      public void SetLastWriteTime()
      {
         Console.WriteLine("Directory.SetLastWriteTime()");
         Console.WriteLine("\nPlease see unit test: SetCreationTime()");
      }

      #endregion // SetLastWriteTime

      #region SetLastWriteTimeUtc

      [TestMethod]
      public void SetLastWriteTimeUtc()
      {
         Console.WriteLine("Directory.SetLastWriteTimeUtc()");
         Console.WriteLine("\nPlease see unit test: SetCreationTime()");
      }

      #endregion // SetLastWriteTimeUtc

      #endregion // .NET

      #region AlphaFS

      #region Compress/Decompress

      [TestMethod]
      public void AlphaFS_Compress()
      {
         Console.WriteLine("Directory.Compress()");

         DumpCompressDecompress(true);
         DumpCompressDecompress(false);
      }

      [TestMethod]
      public void AlphaFS_Decompress()
      {
         Console.WriteLine("Directory.Decompress()");
         Console.WriteLine("\nPlease see unit test: AlphaFS_Compress()");
      }

      [TestMethod]
      public void AlphaFS_DisableCompression()
      {
         Console.WriteLine("Directory.DisableCompression()");

         DumpEnableDisableCompression(true);
         DumpEnableDisableCompression(false);
      }

      [TestMethod]
      public void AlphaFS_EnableCompression()
      {
         Console.WriteLine("Directory.EnableCompression()");

         DumpEnableDisableCompression(true);
         DumpEnableDisableCompression(false);
      }

      #endregion Compress/Decompress

      #region Copy

      [TestMethod]
      public void AlphaFS_Copy()
      {
         Console.WriteLine("Directory.Copy()");

         DumpCopy(true);
         DumpCopy(false);
      }

      #endregion // Copy

      #region CountFileSystemObjects

      [TestMethod]
      public void AlphaFS_CountFileSystemObjects()
      {
         Console.WriteLine("Directory.CountFileSystemObjects()");

         long fsoCount = 0;

         #region Count Directories

         Console.WriteLine("\nCount Directories");

         string path = SysRoot;

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
            Console.WriteLine("\n\tDirectory.CountFileSystemObjects(): Caught Exception: [{0}]", ex.Message.Replace(Environment.NewLine, "  "));
         }
         Console.WriteLine("\n\tCaught Exception (Should be True): [{0}]", gotException);
         Assert.IsTrue(gotException);
         Console.WriteLine("\n\tdirectory\t = [{0}]\n\tSubdirectories = [{1}]\n{2}\n", path, fsoCount, Reporter());

         #endregion // Exception

         searchPattern = Path.WildcardStarMatchAll;

         Console.WriteLine("\n\tsearchPattern: \"{0}\", continue on error. Running as Administrator will count more directories.", searchPattern);

         StopWatcher(true);
         fsoCount = Directory.CountFileSystemObjects(path, searchPattern, DirectoryEnumerationOptions.Folders | DirectoryEnumerationOptions.Recursive | DirectoryEnumerationOptions.ContinueOnException);
         Console.WriteLine("\n\tdirectory\t = [{0}]\n\tSubdirectories = [{1}]\n{2}\n", path, fsoCount, Reporter());
         Assert.IsTrue(fsoCount > 0);


         Console.WriteLine("\n\tsearchPattern: \"{0}\", continue on error. Running as Administrator and using PrivilegeEnabler(Privilege.Backup) will count even more directories.", searchPattern);
         StopWatcher(true);
         using (new PrivilegeEnabler(Privilege.Backup))
         {
            fsoCount = Directory.CountFileSystemObjects(path, searchPattern, DirectoryEnumerationOptions.Folders | DirectoryEnumerationOptions.Recursive | DirectoryEnumerationOptions.ContinueOnException);
            Console.WriteLine("\n\tDirectory\t = [{0}]\n\tSubdirectories = [{1}]\n{2}\n", path, fsoCount, Reporter());
            Assert.IsTrue(fsoCount > 0);
         }
         Console.WriteLine();

         #endregion // Count Directories

         #region Count Files

         Console.WriteLine("Count Files");

         searchPattern = Path.WildcardStarMatchAll;

         path = SysRoot32;
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
            Console.WriteLine("\n\tDirectory.CountFileSystemObjects(): Caught Exception: [{0}]", ex.Message.Replace(Environment.NewLine, "  "));
         }
         Console.WriteLine("\n\tCaught Exception (Should be True): [{0}]", gotException);
         Assert.IsTrue(gotException);
         Console.WriteLine("\n\tDirectory = [{0}]\n\tFiles  = [{1}]\n{2}\n", path, fsoCount, Reporter());

         #endregion // Exception

         StopWatcher(true);
         fsoCount = Directory.CountFileSystemObjects(path, searchPattern, DirectoryEnumerationOptions.Files | DirectoryEnumerationOptions.Recursive | DirectoryEnumerationOptions.ContinueOnException);
         Console.WriteLine("\n\tDirectory = [{0}]\n\tFiles  = [{1}]\n{2}\n", path, fsoCount, Reporter());
         Assert.IsTrue(fsoCount > 0);

         Console.WriteLine("\n\tContinue on error. Running as Administrator will count more files.");
         StopWatcher(true);
         fsoCount = Directory.CountFileSystemObjects(path, searchPattern, DirectoryEnumerationOptions.Files | DirectoryEnumerationOptions.Recursive | DirectoryEnumerationOptions.ContinueOnException);
         Console.WriteLine("\n\tDirectory = [{0}]\n\tFiles  = [{1}]\n{2}\n", path, fsoCount, Reporter());
         Assert.IsTrue(fsoCount > 0);

         Console.WriteLine("\n\tContinue on error. Running as Administrator and using PrivilegeEnabler(Privilege.Backup) will count even more files.");
         StopWatcher(true);
         using (new PrivilegeEnabler(Privilege.Backup))
         {
            fsoCount = Directory.CountFileSystemObjects(path, searchPattern, DirectoryEnumerationOptions.Files | DirectoryEnumerationOptions.Recursive | DirectoryEnumerationOptions.ContinueOnException);
            Console.WriteLine("\n\tDirectory = [{0}]\n\tFiles  = [{1}]\n{2}\n", path, fsoCount, Reporter());
            Assert.IsTrue(fsoCount > 0);
         }
         Console.WriteLine();

         #endregion // Count Files
      }

      #endregion // CountFileSystemObjects

      #region DeleteEmptySubdirectories

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
         CreateDirectoriesAndFiles(tempPath, maxDepth, true);

         string searchPattern = Path.WildcardStarMatchAll;

         StopWatcher(true);
         dirs0 = Directory.CountFileSystemObjects(tempPath, searchPattern, DirectoryEnumerationOptions.Folders | DirectoryEnumerationOptions.Recursive | DirectoryEnumerationOptions.ContinueOnException);
         files0 = Directory.CountFileSystemObjects(tempPath, searchPattern, DirectoryEnumerationOptions.Files | DirectoryEnumerationOptions.Recursive | DirectoryEnumerationOptions.ContinueOnException);
         Console.WriteLine("\nCounted Directories: [{0}]\nCounted Files      : [{1}]\n{2}", dirs0, files0, Reporter());

         StopWatcher(true);
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

         Console.WriteLine("\nDirectory.DeleteEmptySubdirectories() (Should be True): [{0}]\n{1}", deleteOk, Reporter());
         Assert.IsTrue(deleteOk, "DeleteEmptySubdirectories() failed.");

         searchPattern = Path.WildcardStarMatchAll;

         StopWatcher(true);
         dirs1 = Directory.CountFileSystemObjects(tempPath, searchPattern, DirectoryEnumerationOptions.Folders | DirectoryEnumerationOptions.Recursive | DirectoryEnumerationOptions.ContinueOnException);
         files1 = Directory.CountFileSystemObjects(tempPath, searchPattern, DirectoryEnumerationOptions.Files | DirectoryEnumerationOptions.Recursive | DirectoryEnumerationOptions.ContinueOnException);
         Console.WriteLine("\nCounted Directories (Should be 60): [{0}]\nCounted Files (Should be 110)     : [{1}]\n{2}", dirs1, files1, Reporter());
         Assert.IsTrue(dirs1 != dirs0);
         Assert.IsTrue(dirs1 == remainingDirectories);
         Assert.IsTrue(files1 == files0);

         Directory.Delete(tempPath, true);
         bool directoryNotExists = !Directory.Exists(tempPath);
         Assert.IsTrue(directoryNotExists);

         Assert.IsTrue((emptyDirectories + remainingDirectories) == totalDirectories);
      }

      #endregion // DeleteEmptySubdirectories

      #region Encrypt/Decrypt

      [TestMethod]
      public void AlphaFS_Encrypt()
      {
         Console.WriteLine("Directory.Encrypt()");

         DumpEncryptDecrypt(true);
         DumpEncryptDecrypt(false);
      }

      [TestMethod]
      public void AlphaFS_Decrypt()
      {
         Console.WriteLine("Directory.Decrypt()");
         Console.WriteLine("\nPlease see unit test: AlphaFS_Encrypt()");
      }

      [TestMethod]
      public void AlphaFS_DisableEncryption()
      {
         Console.WriteLine("Directory.DisableEncryption()");

         DumpEnableDisableEncryption(true);
         DumpEnableDisableEncryption(false);
      }

      [TestMethod]
      public void AlphaFS_EnableEncryption()
      {
         Console.WriteLine("Directory.EnableEncryption()");

         DumpEnableDisableEncryption(true);
         DumpEnableDisableEncryption(false);
      }

      #endregion // Encrypt/Decrypt

      #region EnumerateAlternateDataStreams

      [TestMethod]
      public void AlphaFS_EnumerateAlternateDataStreams()
      {
         Console.WriteLine("Directory.EnumerateAlternateDataStreams()");
         Console.WriteLine("\nPlease see unit test: Filesystem_Class_AlternateDataStreamInfo()");
      }

      #endregion // EnumerateAlternateDataStreams

      #region EnumerateFileIdBothDirectoryInfo

      [TestMethod]
      public void AlphaFS_EnumerateFileIdBothDirectoryInfo()
      {
         Console.WriteLine("Directory.EnumerateFileIdBothDirectoryInfo()");

         DumpEnumerateFileIdBothDirectoryInfo(true);
         DumpEnumerateFileIdBothDirectoryInfo(false);
      }

      #endregion // EnumerateFileIdBothDirectoryInfo

      #region EnumerateFileSystemEntryInfos

      [TestMethod]
      public void AlphaFS_EnumerateFileSystemEntryInfos()
      {
         Console.WriteLine("Directory.EnumerateFileSystemEntryInfos()");

         Console.WriteLine("\nThis is the main enumeration function for files and folders.\n");

         Console.WriteLine("\nPlease see unit test: EnumerateDirectories()");
         Console.WriteLine("\nPlease see unit test: EnumerateFiles()");
         Console.WriteLine("\nPlease see unit test: EnumerateFileSystemEntries()");
      }

      #endregion // EnumerateFileSystemEntryInfos

      #region EnumerateLogicalDrives

      [TestMethod]
      public void AlphaFS_EnumerateLogicalDrives()
      {
         Console.WriteLine("Directory.EnumerateLogicalDrives()");
         Console.WriteLine("\nPlease see unit test: GetLogicalDrives()");
      }

      #endregion // EnumerateLogicalDrives

      #region GetChangeTime

      [TestMethod]
      public void AlphaFS_GetChangeTime()
      {
         Console.WriteLine("Directory.GetChangeTime()");
         Console.WriteLine("\nPlease see unit test: GetCreationTime()");
      }

      #endregion // GetChangeTime

      #region GetFileSystemEntry

      [TestMethod]
      public void AlphaFS_GetFileSystemEntry()
      {
         Console.WriteLine("Directory.GetFileSystemEntry()");
         Console.WriteLine("\nPlease see unit test: Filesystem_Class_FileSystemEntryInfo()");
      }

      #endregion // GetFileSystemEntry

      #region GetProperties

      [TestMethod]
      public void AlphaFS_GetProperties()
      {
         Console.WriteLine("Directory.GetProperties()");

         DumpGetProperties(true);
         DumpGetProperties(false);
      }

      #endregion // GetProperties

      #region HasInheritedPermissions

      [TestMethod]
      public void AlphaFS_HasInheritedPermissions()
      {
         Console.WriteLine("Directory.HasInheritedPermissions()\n");

         string searchPattern = Path.WildcardStarMatchAll;
         SearchOption searchOption = SearchOption.TopDirectoryOnly;

         int cnt = 0;
         StopWatcher(true);
         foreach (string dir in Directory.EnumerateDirectories(SysRoot, searchPattern, searchOption))
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
               Console.Write("\t#{0:000}\tCaught Exception for directory: [{1}]\t[{2}]\n", cnt, dir, ex.Message.Replace(Environment.NewLine, "  "));
            }
         }
         Console.Write("\n{0}", Reporter());
      }

      #endregion // HasInheritedPermissions

      #region SetTimestamps

      [TestMethod]
      public void AlphaFS_SetTimestamps()
      {
         Console.WriteLine("Directory.SetTimestamps()");

         DumpSetTimestamps(true);
         DumpSetTimestamps(false);
      }

      #endregion // SetTimestamps

      #region TransferTimestamps

      [TestMethod]
      public void AlphaFS_TransferTimestamps()
      {
         Console.WriteLine("Directory.TransferTimestamps()");

         DumpTransferTimestamps(true);
         DumpTransferTimestamps(false);
      }

      #endregion // TransferTimestamps


      #region AlphaFS___DirectoryTrailingDotSpace

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

      #endregion // AlphaFS___DirectoryTrailingDotSpace

      #endregion // AlphaFS

      #endregion Unit Test Callers
   }
}