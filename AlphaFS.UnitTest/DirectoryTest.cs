/* Copyright (c) 2008-2014 Peter Palotas, Alexandr Normuradov, Jeffrey Jangli
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
using FileSystemInfo = Alphaleonis.Win32.Filesystem.FileSystemInfo;
using NativeMethods = Alphaleonis.Win32.Filesystem.NativeMethods;
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
      private readonly bool _testMyServer = Environment.UserName.Equals(@"jjangli", StringComparison.OrdinalIgnoreCase);
      private const string MyServer = "yomodo";
      private const string MyServerShare = @"\\" + MyServer + @"\video";
      private const string Local = @"LOCAL";
      private const string Network = @"NETWORK";

      private static readonly string SysDrive = Environment.GetEnvironmentVariable("SystemDrive");
      private static readonly string SysRoot = Environment.GetEnvironmentVariable("SystemRoot");
      private static readonly string SysRoot32 = Path.Combine(SysRoot, "System32");

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

      private static string Reporter(bool condensed = false, bool onlyTime = false)
      {
         Win32Exception lastError = new Win32Exception();

         StopWatcher();

         return onlyTime
            ? string.Format(CultureInfo.CurrentCulture, condensed
               ? "{0} [{1}: {2}]"
               : "\t\t{0}", StopWatcher())
            : string.Format(CultureInfo.CurrentCulture, condensed
               ? "{0} [{1}: {2}]"
               : "\t\t{0}\t*Win32 Result: [{1, 4}]\t*Win32 Message: [{2}]", StopWatcher(), lastError.NativeErrorCode, lastError.Message);
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
               propValue = ex.Message.Replace(Environment.NewLine, string.Empty);
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
         @"C",
         @"C:",
         @"C:\",
         @"C:\a",
         @"C:\a\",
         @"C:\a\b",
         @"C:\a\b\",
         @"C:\a\b\c",
         @"C:\a\b\c\",
         @"C:\a\b\c\f",
         @"C:\a\b\c\f.",
         @"C:\a\b\c\f.t",
         @"C:\a\b\c\f.tx",
         @"C:\a\b\c\f.txt",

         Path.LongPathPrefix + @"Program Files\Microsoft Office",
         Path.LongPathPrefix + "C",
         Path.LongPathPrefix + @"C:",
         Path.LongPathPrefix + @"C:\",
         Path.LongPathPrefix + @"C:\a",
         Path.LongPathPrefix + @"C:\a\",
         Path.LongPathPrefix + @"C:\a\b",
         Path.LongPathPrefix + @"C:\a\b\",
         Path.LongPathPrefix + @"C:\a\b\c",
         Path.LongPathPrefix + @"C:\a\b\c\",
         Path.LongPathPrefix + @"C:\a\b\c\f",
         Path.LongPathPrefix + @"C:\a\b\c\f.",
         Path.LongPathPrefix + @"C:\a\b\c\f.t",
         Path.LongPathPrefix + @"C:\a\b\c\f.tx",
         Path.LongPathPrefix + @"C:\a\b\c\f.txt",

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

      private static void DumpAccessRules(int cntCheck, DirectorySecurity dsSystem, DirectorySecurity dsAlpha)
      {
         Console.WriteLine("\n\tSanity check AlphaFS <> System.IO {0}.", cntCheck);
         Console.WriteLine("\t\tFile.GetAccessControl().AreAccessRulesProtected: [{0}]", dsAlpha.AreAccessRulesProtected);
         Assert.AreEqual(dsAlpha.AreAccessRulesProtected, dsSystem.AreAccessRulesProtected);

         StopWatcher(true);
         Console.WriteLine("\t\tFile.GetAccessControl().AreAuditRulesProtected: [{0}]", dsAlpha.AreAuditRulesProtected);
         Assert.AreEqual(dsAlpha.AreAuditRulesProtected, dsSystem.AreAuditRulesProtected);

         StopWatcher(true);
         Console.WriteLine("\t\tFile.GetAccessControl().AreAccessRulesCanonical: [{0}]", dsAlpha.AreAccessRulesCanonical);
         Assert.AreEqual(dsAlpha.AreAccessRulesCanonical, dsSystem.AreAccessRulesCanonical);

         StopWatcher(true);
         Console.WriteLine("\t\tFile.GetAccessControl().AreAuditRulesCanonical: [{0}]", dsAlpha.AreAuditRulesCanonical);
         Assert.AreEqual(dsAlpha.AreAuditRulesCanonical, dsSystem.AreAuditRulesCanonical);
      }
      
      
      private void DumpCompressDecompress(bool isLocal)
      {
         Console.WriteLine("\n=== TEST {0} ===", isLocal ? Local : Network);
         string tempPath = Path.Combine(Path.GetTempPath(), "Directory.CompressDecompress()-" + Path.GetRandomFileName());
         if (!isLocal) tempPath = Path.LocalToUnc(tempPath);

         Console.WriteLine("\nInput Path: [{0}]", tempPath);

         int cnt = 0;
         Directory.CreateDirectory(tempPath);
         FileAttributes actual = File.GetAttributes(tempPath);
         bool action = (actual & FileAttributes.Compressed) == FileAttributes.Compressed;

         Console.WriteLine("\nCompressed (Should be False): [{0}]\t\tAttributes: [{1}]\n", action, actual);
         Assert.IsFalse(action, "Compression should be False");
         Assert.IsFalse((actual & FileAttributes.Compressed) == FileAttributes.Compressed, "Compression should be False");


         // Create some directories and files.
         for (int i = 0; i < 5; i++)
         {
            string file = Path.Combine(tempPath, Path.GetRandomFileName());

            string dir = file + "-dir";
            Directory.CreateDirectory(dir);

            // using() == Dispose() == Close() = deletable.
            using (File.Create(file)) { }
            using (File.Create(Path.Combine(dir, Path.GetFileName(file)))) { }

            actual = File.GetAttributes(file);
            action = (actual & FileAttributes.Compressed) == FileAttributes.Compressed;

            Console.WriteLine("\t#{0:000}\tCompressed (Should be False): [{1}]\tAttributes: [{2}] [{3}]", ++cnt, action, actual, Path.GetFullPath(file));
            Assert.IsTrue(cnt > 0, "Nothing was enumerated.");
            Assert.IsFalse(action, "Compression should be False");
         }


         // Compress directory recursively.
         string searchPattern = Path.WildcardStarMatchAll;
         SearchOption searchOption = SearchOption.AllDirectories;
         action = false;
         StopWatcher(true);
         try
         {
            Directory.Compress(tempPath, searchPattern, searchOption);
            action = true;
            actual = File.GetAttributes(tempPath);
         }
         catch (Exception ex)
         {
            Console.WriteLine("\n\tCaught Exception: [{0}]\n", ex.Message.Replace(Environment.NewLine, string.Empty));
            Directory.Delete(tempPath, true);
         }
         Console.WriteLine("\n\nDirectory compressed recursively (Should be True): [{0}]\t\tAttributes: [{1}]\n\t{2}\n", action, actual, Reporter(true));
         Assert.IsTrue(action, "Compression should be True");
         Assert.IsTrue((actual & FileAttributes.Compressed) == FileAttributes.Compressed, "Compression should be True");

         // Check that everything is compressed.
         cnt = 0;
         foreach (FileSystemEntryInfo fsei in File.EnumerateFileSystemEntryInfos(tempPath, searchPattern, searchOption))
         {
            actual = fsei.Attributes;
            action = (actual & FileAttributes.Compressed) == FileAttributes.Compressed;

            Console.WriteLine("\t#{0:000}\tFS Entry: [{1}]\t\tCompressed (Should be True): [{2}]\t\tAttributes: [{3}]", ++cnt, fsei.FileName, action, actual);
            Assert.IsTrue(cnt > 0, "Nothing was enumerated.");
            Assert.IsTrue(action, "Compression should be True");
         }


         // Decompress directory recursively.
         action = false;
         StopWatcher(true);
         try
         {
            Directory.Decompress(tempPath, searchPattern, searchOption);
            action = true;
            actual = File.GetAttributes(tempPath);
         }
         catch (Exception ex)
         {
            Console.WriteLine("\n\tCaught Exception: [{0}]\n", ex.Message.Replace(Environment.NewLine, string.Empty));
            Directory.Delete(tempPath, true);
         }
         Console.WriteLine("\n\nDirectory decompressed recursively (Should be True): [{0}]\t\tAttributes: [{1}]\n\t{2}\n", action, actual, Reporter(true));
         Assert.IsTrue(action, "Compression should be True");
         Assert.IsFalse((actual & FileAttributes.Compressed) == FileAttributes.Compressed, "Compression should be True");

         // Check that everything is decompressed.
         cnt = 0;
         foreach (FileSystemEntryInfo fsei in File.EnumerateFileSystemEntryInfos(tempPath, searchPattern, searchOption))
         {
            actual = fsei.Attributes;
            action = (actual & FileAttributes.Compressed) != FileAttributes.Compressed;

            Console.WriteLine("\t#{0:000}\tFS Entry: [{1}]\t\tDecompressed (Should be True): [{2}]\t\tAttributes: [{3}]", ++cnt, fsei.FileName, action, actual);
            Assert.IsTrue(cnt > 0, "Nothing was enumerated.");
            Assert.IsTrue(action, "Decompression should be True");
         }


         Directory.Delete(tempPath, true);
         Assert.IsFalse(Directory.Exists(tempPath), "Cleanup failed: Directory should have been removed.");
         Console.WriteLine();
      }

      #region DumpCopy

      private void DumpCopy(bool isLocal)
      {
         Console.WriteLine("\n=== TEST {0} ===", isLocal ? Local : Network);
         string tempPath = Path.Combine(Path.GetTempPath(), "Directory.Copy()-" + Path.GetRandomFileName());
         if (!isLocal) tempPath = Path.LocalToUnc(tempPath);

         DirectoryInfo sourceDir = new DirectoryInfo(Path.GetTempPath(tempPath + @"\sourceDir"));
         Assert.IsFalse(sourceDir.Exists, "Directory should not exist.");
         sourceDir.Create();
         Assert.IsTrue(sourceDir.Exists, "Directory should exist.");

         // Create some directories and files.
         CreateDirectoriesAndFiles(sourceDir.FullName, 10, true);

         Console.WriteLine("\nInput Directory Path: [{0}]", sourceDir.FullName);

         #region 1st: DirectoryInfo.CopyTo()

         StopWatcher(true);
         DirectoryInfo firstCopy = sourceDir.CopyTo(tempPath + @"\1stCopy");
         Console.WriteLine("\n\t1st: DirectoryInfo.CopyTo()\n\n\t  Source:      [{0}]\n\t  Destination: [{1}]\n\t{2}", sourceDir.FullName, firstCopy.FullName, Reporter(true));
         Dictionary<string, long> props1stCopy = Directory.GetProperties(sourceDir.FullName, SearchOption.AllDirectories);

         Dictionary<string, long> propsSourceDir = Directory.GetProperties(sourceDir.FullName, SearchOption.AllDirectories);
         Console.WriteLine("\n\t\tTotal size       : [{0}]\n\t\tTotal directories: [{1}]\n\t\tTotal files      : [{2}]", NativeMethods.UnitSizeToText(props1stCopy["Size"]), props1stCopy["Directory"], props1stCopy["File"]);

         Assert.AreEqual(propsSourceDir["Size"], props1stCopy["Size"], "Total directory size should be equal.");
         Assert.AreEqual(propsSourceDir["Directory"], props1stCopy["Directory"], "Total number of directories should be equal.");
         Assert.AreEqual(propsSourceDir["File"], props1stCopy["File"], "Total number of files should be equal.");
         
         #endregion // 1st: DirectoryInfo.CopyTo()

         #region 2nd: Directory.Copy()

         string secondCopy = Path.GetTempPath(tempPath + @"\2ndCopy");
         StopWatcher(true);
         Directory.Copy(firstCopy.FullName, secondCopy);
         Console.WriteLine("\n\n\t2nd: Directory.Copy()\n\n\t  Source:      [{0}]\n\t  Destination: [{1}]\n\t{2}", firstCopy.FullName, secondCopy, Reporter(true));

         Dictionary<string, long> props2ndDirCopy = Directory.GetProperties(secondCopy, SearchOption.AllDirectories);
         Console.WriteLine("\n\t\tTotal size       : [{0}]\n\t\tTotal directories: [{1}]\n\t\tTotal files      : [{2}]", NativeMethods.UnitSizeToText(props2ndDirCopy["Size"]), props2ndDirCopy["Directory"], props2ndDirCopy["File"]);

         Assert.AreEqual(propsSourceDir["Size"], props2ndDirCopy["Size"], "Total directory size should be equal.");
         Assert.AreEqual(propsSourceDir["Directory"], props2ndDirCopy["Directory"], "Total number of directories should be equal.");
         Assert.AreEqual(propsSourceDir["File"], props2ndDirCopy["File"], "Total number of files should be equal.");

         #endregion // 2nd: Directory.Copy()

         #region 3rd: Directory.Copy()/DirectoryInfo.CopyTo(), Exception/Overwrite

         StopWatcher(true);
         DirectoryInfo thirdCopy = sourceDir.CopyTo(tempPath + @"\3rdCopy");
         Console.WriteLine("\n\n\t3rd: Directory.Copy()/DirectoryInfo.CopyTo(), Exception/Overwrite\n\n\t  Source:      [{0}]\n\t  Destination: [{1}]\n\t{2}", sourceDir.FullName, thirdCopy.FullName, Reporter(true));

         Dictionary<string, long> props3rdDirCopy = Directory.GetProperties(thirdCopy.FullName, SearchOption.AllDirectories);
         Console.WriteLine("\n\t\tTotal size       : [{0}]\n\t\tTotal directories: [{1}]\n\t\tTotal files      : [{2}]", NativeMethods.UnitSizeToText(props3rdDirCopy["Size"]), props3rdDirCopy["Directory"], props3rdDirCopy["File"]);


         Console.WriteLine("\n\tCopy same directories again, should throw AlreadyExistsException.");
         bool exception = false;
         try
         {
            sourceDir.CopyTo(thirdCopy.FullName);
            //Directory.Copy(sourceDir.FullName, thirdCopy.FullName);
         }
         catch (AlreadyExistsException ex)
         {
            exception = true;
            Console.WriteLine("\n\tCaught Exception: [{0}]", ex.Message.Replace(Environment.NewLine, string.Empty));
         }

         Console.WriteLine("\n\tCaught Exception (Should be True): [{0}]", exception);
         Assert.IsTrue(exception);

         Console.WriteLine("\n\tTry again: Directory.Copy(,,true)");
         Directory.Copy(sourceDir.FullName, thirdCopy.FullName, true);

         #endregion // 3rd: Directory.Copy()/DirectoryInfo.CopyTo(), Exception/Overwrite

         Directory.Delete(tempPath, true, true);
         Assert.IsFalse(Directory.Exists(tempPath), "Cleanup failed: Directory should have been removed.");
         Console.WriteLine();
      }

      #endregion // DumpCopy

      private void DumpCreateDirectory(bool isLocal)
      {
         Console.WriteLine("\n=== TEST {0} ===", isLocal ? Local : Network);

         // Directory depth level.
         int level = new Random().Next(1, 1000);

         string tempPath = Path.GetTempPath("Directory.CreateDirectory()-" + level + "-" + Path.GetRandomFileName());
         if (!isLocal) tempPath = Path.LocalToUnc(tempPath);

         Console.WriteLine("\nInput Directory Path: [{0}]", tempPath);

         DirectoryInfo dirInfo = new DirectoryInfo(tempPath);
         System.IO.DirectoryInfo dirInfoSysIo = new System.IO.DirectoryInfo(tempPath);
         Assert.AreEqual(dirInfoSysIo.Exists, dirInfo.Exists, "Exists AlphaFS != System.IO");

         // Should be false.
         Assert.IsFalse(dirInfo.Exists, "Directory should not exist: [{0}]", tempPath);

         
         StopWatcher(true);
         dirInfo.Create(true);  // Create compressed directory.
         string report = Reporter(true);
         bool exist = dirInfo.Exists;
         Console.WriteLine("\nCreateDirectory() (Should be True): [{0}]\n\t{1}\n", exist, report);
         
         
         bool compressed = (dirInfo.Attributes & FileAttributes.Compressed) == FileAttributes.Compressed;
         Console.WriteLine("Is compressed (Should be True): [{0}]\n", compressed);
         Assert.IsTrue(compressed, "Compression should be True");

         
         // dirInfoSysIO does not know about the new directory at this point.
         Assert.AreNotEqual(dirInfoSysIo.Exists, dirInfo.Exists, "Exists AlphaFS should != System.IO");

         
         // Notify dirInfoSysIO that the directory now exists.
         dirInfoSysIo.Refresh();
         Assert.IsTrue(dirInfoSysIo.Exists, "Exists System.IO in error??!");


         StopWatcher(true);
         string pathSub = dirInfo.CreateSubdirectory("A Sub Directory").FullName;
         report = Reporter(true);

         Console.WriteLine("CreateSubdirectory(): [{0}]\n\t{1}\n", pathSub, report);

         // After the refresh, should be true.
         dirInfo.Refresh();
         exist = dirInfo.Exists;
         
         Assert.IsFalse(Utils.IsNullOrWhiteSpace(pathSub));
         Assert.IsTrue(exist, "Directory should exist.");
         Assert.IsTrue(!pathSub.StartsWith(Path.LongPathPrefix), "Directory should not start with LongPath prefix.");
         

         
         string root = Path.Combine(tempPath, "Another Sub Directory");

         // MAX_PATH hit the road.
         for (int i = 0; i < level; i++)
            root = Path.Combine(root, "-" + (i + 1) + "-subdir");
         
         StopWatcher(true);
         dirInfo = Directory.CreateDirectory(root);
         report = Reporter(true);
         Console.WriteLine("Created directory structure (Should be True): [{0}]\n\t{1}", dirInfo != null, report);
         Console.WriteLine("\nSubdirectory depth: [{0}], path length: [{1}] characters.\n", level, root.Length);

         
         compressed = (dirInfo.Attributes & FileAttributes.Compressed) == FileAttributes.Compressed;
         Assert.IsTrue(compressed, "Compression should be True");
         Assert.IsTrue(Directory.Exists(root), "Directory should exist.");


         // Fail; directory is not empty;
         bool exception = false;
         try
         {
            Directory.Delete(tempPath);
         }
         catch (Exception ex)
         {
            exception = true;
            Console.WriteLine("\nDirectory.Delete() Exception: [{0}]", ex.Message.Replace(Environment.NewLine, string.Empty));
         }
         Console.WriteLine("\nCaught Exception (Should be True): [{0}]", exception);
         Assert.IsTrue(exception, "Exception should have been caught.");


         StopWatcher(true);
         Directory.Delete(tempPath, true);
         report = Reporter(true);
         exist = Directory.Exists(tempPath);

         Console.WriteLine("\n\nDirectory.Delete() (Should be True): [{0}]\n\t{1}", !exist, report);
         Assert.IsFalse(exist, "Cleanup failed: Directory should have been removed.");
         Console.WriteLine();
      }

      private void DumpEnableDisableCompression(bool isLocal)
      {
         Console.WriteLine("\n=== TEST {0} ===", isLocal ? Local : Network);
         string tempPath = Path.Combine(Path.GetTempPath(), "Directory.EnableDisableCompression()-" + Path.GetRandomFileName());
         if (!isLocal) tempPath = Path.LocalToUnc(tempPath);

         Console.WriteLine("\nInput Directory Path: [{0}]", tempPath);

         Directory.CreateDirectory(tempPath);
         FileAttributes actual = File.GetAttributes(tempPath);
         Console.WriteLine("Attributes: [{0}]", actual);
         Assert.IsFalse((actual & FileAttributes.Compressed) == FileAttributes.Compressed);
         Assert.IsTrue((actual & FileAttributes.Directory) == FileAttributes.Directory);


         string report = string.Empty;
         bool action = false;
         StopWatcher(true);
         try
         {
            Directory.EnableCompression(tempPath);
            report = Reporter(true);
            action = true;
            actual = File.GetAttributes(tempPath);
         }
         catch (Exception ex)
         {Console.WriteLine("\n");
            Console.WriteLine("\n\tCaught Exception: [{0}]\n", ex.Message.Replace(Environment.NewLine, string.Empty));
         }
         Console.WriteLine("\nEnableCompression() successful (Should be True): [{0}]", action);
         Console.WriteLine("Attributes: [{0}]\n\t{1}", actual, report);
         Assert.IsTrue(action, "Directory should have compression enabled.");
         Assert.IsTrue((actual & FileAttributes.Compressed) == FileAttributes.Compressed, "Directory should have compression enabled.");


         action = false;
         try
         {
            Directory.DisableCompression(tempPath);
            report = Reporter(true);
            action = true;
            actual = File.GetAttributes(tempPath);
         }
         catch (Exception ex)
         {
            Console.WriteLine("\n\tCaught Exception: [{0}]\n", ex.Message.Replace(Environment.NewLine, string.Empty));
         }
         Console.WriteLine("\nDisableCompression() successful (Should be True): [{0}]", action);
         Console.WriteLine("Attributes: [{0}]\n\t{1}", actual, report);
         Assert.IsTrue(action, "Directory should have compression disabled.");
         Assert.IsFalse((actual & FileAttributes.Compressed) == FileAttributes.Compressed, "Directory should have compression disabled.");


         Directory.Delete(tempPath, true);
         Assert.IsFalse(Directory.Exists(tempPath), "Cleanup failed: Directory should have been removed.");
         Console.WriteLine();
      }

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
         Assert.IsTrue((actual & FileAttributes.Directory) == FileAttributes.Directory);


         string report = string.Empty;
         bool action = false;
         try
         {
            StopWatcher(true);
            Directory.EnableEncryption(tempPath);
            report = Reporter(true);
            action = true;
         }
         catch (Exception ex)
         {
            Console.WriteLine("\n\tCaught Exception: [{0}]\n", ex.Message.Replace(Environment.NewLine, string.Empty));
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
            report = Reporter(true);
            action = true;
         }
         catch (Exception ex)
         {
            Console.WriteLine("\n\tCaught Exception: [{0}]\n", ex.Message.Replace(Environment.NewLine, string.Empty));
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

      private void DumpEncryptDecrypt(bool isLocal)
      {
         Console.WriteLine("\n=== TEST {0} ===", isLocal ? Local : Network);
         string tempPath = Path.Combine(Path.GetTempPath(), "Directory.EncryptDecrypt()-" + Path.GetRandomFileName());
         if (!isLocal) tempPath = Path.LocalToUnc(tempPath);

         Console.WriteLine("\nInput Path: [{0}]", tempPath);

         int cnt = 0;
         Directory.CreateDirectory(tempPath);
         FileAttributes actual = File.GetAttributes(tempPath);
         bool action = (actual & FileAttributes.Encrypted) == FileAttributes.Encrypted;

         Console.WriteLine("\nEncrypted (Should be False): [{0}]\t\tAttributes: [{1}]\n", action, actual);
         Assert.IsFalse(action, "Encryption should be False");
         Assert.IsFalse((actual & FileAttributes.Encrypted) == FileAttributes.Encrypted, "Encryption should be False");


         // Create some directories and files.
         for (int i = 0; i < 5; i++)
         {
            string file = Path.Combine(tempPath, Path.GetRandomFileName());

            string dir = file + "-dir";
            Directory.CreateDirectory(dir);

            // using() == Dispose() == Close() = deletable.
            using (File.Create(file)) { }
            using (File.Create(Path.Combine(dir, Path.GetFileName(file)))) { }

            actual = File.GetAttributes(file);
            action = (actual & FileAttributes.Encrypted) == FileAttributes.Encrypted;

            Console.WriteLine("\t#{0:000}\tEncrypted (Should be False): [{1}]\t\tAttributes: [{2}] [{3}]", ++cnt, action, actual, Path.GetFullPath(file));
            Assert.IsTrue(cnt > 0, "Nothing was enumerated.");
            Assert.IsFalse(action, "Encryption should be False");
         }


         // Encrypt directory recursively.
         action = false;
         StopWatcher(true);
         try
         {
            Directory.Encrypt(tempPath);
            action = true;
            actual = File.GetAttributes(tempPath);
         }
         catch (Exception ex)
         {
            Console.WriteLine("\n\tCaught Exception: [{0}]\n", ex.Message.Replace(Environment.NewLine, string.Empty));
            Directory.Delete(tempPath, true);
         }
         Console.WriteLine("\nDirectory encrypted recursively (Should be True): [{0}]\t\tAttributes: [{1}]\n{2}\n", action, actual, Reporter());
         Assert.IsTrue(action, "Encryption should be True");
         Assert.IsTrue((actual & FileAttributes.Encrypted) == FileAttributes.Encrypted, "Encryption should be True");

         // Check that everything is encrypted.
         string searchPattern = Path.WildcardStarMatchAll;
         SearchOption searchOption = SearchOption.AllDirectories;
         cnt = 0;
         foreach (FileSystemEntryInfo fsei in File.EnumerateFileSystemEntryInfos(tempPath, searchPattern, searchOption))
         {
            actual = fsei.Attributes;
            action = (actual & FileAttributes.Encrypted) == FileAttributes.Encrypted;

            Console.WriteLine("\t#{0:000}\tFS Entry: [{1}]\t\tEncrypted (Should be True): [{2}]\t\tAttributes: [{3}]", ++cnt, fsei.FileName, action, actual);
            Assert.IsTrue(cnt > 0, "Nothing was enumerated.");
            Assert.IsTrue(action, "Encryption should be True");
         }


         // Decrypt directory recursively.
         action = false;
         StopWatcher(true);
         try
         {
            Directory.Decrypt(tempPath);
            action = true;
            actual = File.GetAttributes(tempPath);
         }
         catch (Exception ex)
         {
            Console.WriteLine("\n\tCaught Exception: [{0}]", ex.Message.Replace(Environment.NewLine, string.Empty));
            Directory.Delete(tempPath, true);
         }
         Console.WriteLine("\n\nDirectory decrypted recursively (Should be True): [{0}]\t\tAttributes: [{1}]\n{2}\n", action, actual, Reporter());
         Assert.IsTrue(action, "Decryption should be True");
         Assert.IsFalse((actual & FileAttributes.Encrypted) == FileAttributes.Encrypted, "Decryption should be True");

         // Check that everything is decrypted.
         searchPattern = Path.WildcardStarMatchAll;
         searchOption = SearchOption.AllDirectories;
         cnt = 0;
         foreach (FileSystemEntryInfo fsei in File.EnumerateFileSystemEntryInfos(tempPath, searchPattern, searchOption))
         {
            actual = fsei.Attributes;
            action = (actual & FileAttributes.Encrypted) != FileAttributes.Encrypted;

            Console.WriteLine("\t#{0:000}\tFS Entry: [{1}]\t\tDecrypted (Should be True): [{2}]\t\tAttributes: [{3}]", ++cnt, fsei.FileName, action, actual);
            Assert.IsTrue(cnt > 0, "Nothing was enumerated.");
            Assert.IsTrue(action, "Decryption should be True");
         }


         Directory.Delete(tempPath, true);
         Assert.IsFalse(Directory.Exists(tempPath), "Cleanup failed: Directory should have been removed.");
         Console.WriteLine("\n");
      }

      #region DumpEnumerateDirectories

      private void DumpEnumerateDirectories(bool isLocal)
      {
         Console.WriteLine("\n=== TEST {0} ===", isLocal ? Local : Network);
         string path = isLocal ? SysRoot : Path.LocalToUnc(SysRoot);

         int cntAlphaFS = 0;
         string searchPattern = Path.WildcardStarMatchAll;
         SearchOption searchOption = SearchOption.TopDirectoryOnly;

         Console.WriteLine("\nInput Directory Path: [{0}]\n", path);
         Console.WriteLine("\tEnumerate directories, using \"SearchOption.{0}\"\n", searchOption);

         StopWatcher(true);
         foreach (string dir in Directory.EnumerateDirectories(path, searchPattern, searchOption))
            Console.WriteLine("\t#{0:000}\t[{1}]", ++cntAlphaFS, dir);

         Console.WriteLine("\n\t{0}\n", Reporter(true));
         Assert.IsTrue(cntAlphaFS > 0, "Nothing was enumerated.");
      }

      #endregion // DumpEnumerateDirectories

      #region DumpEnumerateFiles

      private void DumpEnumerateFiles(bool isLocal)
      {
         Console.WriteLine("\n=== TEST {0} ===", isLocal ? Local : Network);
         string path = isLocal ? SysRoot : Path.LocalToUnc(SysRoot);

         int cntAlphaFS = 0;
         string searchPattern = Path.WildcardStarMatchAll;
         SearchOption searchOption = SearchOption.TopDirectoryOnly;

         Console.WriteLine("\nInput Directory Path: [{0}]\n", path);
         Console.WriteLine("\tEnumerate files, using \"SearchOption.{0}\"\n", searchOption);

         StopWatcher(true);
         foreach (string file in Directory.EnumerateFiles(path, searchPattern, searchOption))
            Console.WriteLine("\t#{0:000}\t[{1}]", ++cntAlphaFS, file);

         Console.WriteLine("\n\t{0}\n", Reporter(true));
         Assert.IsTrue(cntAlphaFS > 0, "Nothing was enumerated.");
      }

      #endregion // DumpEnumerateFiles

      #region DumpEnumerateFileSystemEntries

      private void DumpEnumerateFileSystemEntries(bool isLocal)
      {
         Console.WriteLine("\n=== TEST {0} ===", isLocal ? Local : Network);
         string path = isLocal ? SysRoot : Path.LocalToUnc(SysRoot);

         int cntAlphaFs = 0;
         string searchPattern = Path.WildcardStarMatchAll;
         SearchOption searchOption = SearchOption.TopDirectoryOnly;

         Console.WriteLine("\nInput Directory Path: [{0}]\n", path);
         Console.WriteLine("\tEnumerate directories and files, using \"SearchOption.{0}\"\n", searchOption);

         StopWatcher(true);
         foreach (string dir in Directory.EnumerateFileSystemEntries(path, searchPattern, searchOption, true))
            Console.WriteLine("\t#{0:000}\t{1}\t[{2}]", ++cntAlphaFs, ((File.GetAttributes(dir) & FileAttributes.Directory) == FileAttributes.Directory) ? "[Directory]" : "[File]\t", dir);

         Console.WriteLine("\n\t{0}\n", Reporter(true));
         Assert.IsTrue(cntAlphaFs > 0, "Nothing was enumerated.");
               }

      #endregion // DumpEnumerateFileSystemEntries

      private static void DumpExists(bool isLocal)
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
         exists = Directory.Exists(tempPath);
         Assert.IsFalse(exists, "Directory should exist.");
         Console.WriteLine();
      }

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

         Console.WriteLine("\nInput Path: [{0}]", tempPath);
         Console.WriteLine("\n\tGetAccessControl() rules found: [{0}]\n\tSystem.IO rules found         : [{1}]\n{2}", accessRules.Count, sysIoaccessRules.Count, report);
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
      
      private void DumpGetDirectories(bool isLocal)
      {
         Console.WriteLine("\n=== TEST {0} ===", isLocal ? Local : Network);
         string path = isLocal ? SysRoot : Path.LocalToUnc(SysRoot);

         int cntAlphaFS = 0;
         string searchPattern = Path.WildcardStarMatchAll;
         SearchOption searchOption = SearchOption.TopDirectoryOnly;

         Console.WriteLine("\nInput Path: [{0}]\n", path);
         Console.WriteLine("\tGet directories, using \"SearchOption.{0}\"\n", searchOption);

         StopWatcher(true);
         foreach (string dir in Directory.GetDirectories(path, searchPattern, searchOption))
            Console.WriteLine("\t#{0:000}\t[{1}]\t[{2}]", ++cntAlphaFS, File.GetAttributes(dir), dir);

         Console.WriteLine("\n\t{0}\n", Reporter(true));
         Assert.IsTrue(cntAlphaFS > 0);
      }

      private void DumpGetDirectoryTime(bool isLocal)
      {
         Console.WriteLine("\n=== TEST {0} ===", isLocal ? Local : Network);
         string path = isLocal ? SysRoot32 : Path.LocalToUnc(SysRoot32);

         Console.WriteLine("\nInput Path: [{0}]\n", path);

         DateTime actual = Directory.GetCreationTime(path);
         DateTime expected = System.IO.Directory.GetCreationTime(path);
         Console.WriteLine("\tGetCreationTime()     : [{0}]\tSystem.IO: [{1}]", actual, expected);
         Assert.AreEqual(expected, actual, "GetCreationTime()");

         actual = Directory.GetCreationTimeUtc(path);
         expected = System.IO.Directory.GetCreationTimeUtc(path);
         Console.WriteLine("\tGetCreationTimeUtc()  : [{0}]\tSystem.IO: [{1}]\n", actual, expected);
         Assert.AreEqual(expected, actual, "GetCreationTimeUtc()");

         actual = Directory.GetLastAccessTime(path);
         expected = System.IO.Directory.GetLastAccessTime(path);
         Console.WriteLine("\tGetLastAccessTime()   : [{0}]\tSystem.IO: [{1}]", actual, expected);
         Assert.AreEqual(expected, actual, "GetLastAccessTime()");

         actual = Directory.GetLastAccessTimeUtc(path);
         expected = System.IO.Directory.GetLastAccessTimeUtc(path);
         Console.WriteLine("\tGetLastAccessTimeUtc(): [{0}]\tSystem.IO: [{1}]\n", actual, expected);
         Assert.AreEqual(expected, actual, "GetLastAccessTimeUtc()");

         actual = Directory.GetLastWriteTime(path);
         expected = System.IO.Directory.GetLastWriteTime(path);
         Console.WriteLine("\tGetLastWriteTime()    : [{0}]\tSystem.IO: [{1}]", actual, expected);
         Assert.AreEqual(expected, actual, "GetLastWriteTime()");

         actual = Directory.GetLastWriteTimeUtc(path);
         expected = System.IO.Directory.GetLastWriteTimeUtc(path);
         Console.WriteLine("\tGetLastWriteTimeUtc() : [{0}]\tSystem.IO: [{1}]", actual, expected);
         Assert.AreEqual(expected, actual, "GetLastWriteTimeUtc()");

         Console.WriteLine("\n");
      }
      
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
         Console.WriteLine("\n\t{0}", Reporter(true));
      }

      private void DumpGetFiles(bool isLocal)
      {
         Console.WriteLine("\n=== TEST {0} ===", isLocal ? Local : Network);
         string path = isLocal ? SysRoot : Path.LocalToUnc(SysRoot);

         int cntAlphaFS = 0;
         string searchPattern = Path.WildcardStarMatchAll;
         SearchOption searchOption = SearchOption.TopDirectoryOnly;

         Console.WriteLine("\nInput Path: [{0}]\n", path);
         Console.WriteLine("\tGet files, using \"SearchOption.{0}\"\n", searchOption);

         StopWatcher(true);
         foreach (string file in Directory.GetFiles(path, searchPattern, searchOption))
            Console.WriteLine("\t#{0:000}\t[{1}]\t[{2}]", ++cntAlphaFS, File.GetAttributes(file), file);

         Console.WriteLine("\n\t{0}\n", Reporter(true));
         Assert.IsTrue(cntAlphaFS > 0);
      }

      private void DumpEnumerateFileIdBothDirectoryInfo(bool isLocal)
      {
         Console.WriteLine("\n=== TEST {0} ===", isLocal ? Local : Network);
         string tempPath = SysRoot;
         if (!isLocal) tempPath = Path.LocalToUnc(tempPath);

         Console.WriteLine("\nInput Directory Path: [{0}]", tempPath);

         string searchPattern = Path.WildcardStarMatchAll;
         SearchOption searchOption = SearchOption.TopDirectoryOnly;

         long directories = Directory.CountDirectories(tempPath, searchPattern, searchOption, true, false);
         long files = Directory.CountFiles(tempPath, searchPattern, searchOption, true, false);
         Console.WriteLine("\tDirectories: [{0}], Files: [{1}]", directories, files);

         bool foundFse = false;
         long numDirectories = 0;
         long numFiles = 0;

         StopWatcher(true);
         foreach (FileIdBothDirectoryInfo fse in Directory.EnumerateFileIdBothDirectoryInfo(tempPath))
         {
            if ((fse.FileAttributes & FileAttributes.Directory) == FileAttributes.Directory)
               numDirectories++;
            else
               numFiles++;

            foundFse = Dump(fse, -22);
         }
         string report = Reporter(true);

         bool matchAll = directories == numDirectories && files == numFiles;
         Console.WriteLine("\n\tDirectories = [{0}], Files = [{1}]\n\t{2}", numDirectories, numFiles, report);
         Assert.IsTrue(foundFse);
         Assert.IsTrue(matchAll, "Number of directories and/or files don't match.");
         Console.WriteLine();
      }
      
      private void DumpGetFileSystemEntries(bool isLocal)
      {
         Console.WriteLine("\n=== TEST {0} ===", isLocal ? Local : Network);
         string path = isLocal ? SysRoot : Path.LocalToUnc(SysRoot);

         int cntAlphaFS = 0;
         string searchPattern = Path.WildcardStarMatchAll;
         SearchOption searchOption = SearchOption.TopDirectoryOnly;

         Console.WriteLine("\nInput Path: [{0}]\n", path);
         Console.WriteLine("\tEnumerate directories and files, using \"SearchOption.{0}\"\n", searchOption);

         StopWatcher(true);
         foreach (string file in Directory.GetFileSystemEntries(path, searchPattern, searchOption))
            Console.WriteLine("\t#{0:000}\t[{1}]\t[{2}]", ++cntAlphaFS, File.GetAttributes(file), file);

         Console.WriteLine("\n\t{0}\n", Reporter(true));
         Assert.IsTrue(cntAlphaFS > 0);
      }

      private void DumpGetProperties(bool isLocal)
      {
         Console.WriteLine("\n=== TEST {0} ===", isLocal ? Local : Network);
         string path = isLocal ? SysRoot32 : Path.LocalToUnc(SysRoot32);
         
         Console.WriteLine("\n\tAggregated properties of file system objects from Directory: [{0}]\n", path);

         SearchOption searchOption = SearchOption.AllDirectories;

         StopWatcher(true);
         Dictionary<string, long> props = Directory.GetProperties(path, searchOption, true, false);
         string report = Reporter(true);

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

      private void DumpSetDirectoryTime(bool isLocal)
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
         Console.WriteLine("\n\tSetCreationTime() to: [{0} {1}]", creationTime.ToLongDateString(), creationTime.ToLongTimeString());
         Directory.SetCreationTime(path, creationTime);
         DateTime actual = Directory.GetCreationTime(path);
         System.IO.Directory.SetCreationTime(path, creationTime);
         DateTime expected = System.IO.Directory.GetCreationTime(path);
         Console.WriteLine("\t\tAlphaFS  : [{0}]\n\t\tSystem.IO: [{1}]", actual, expected);
         Assert.AreEqual(expected, actual, "AlphaFS != System.IO");


         Thread.Sleep(new Random().Next(250, 500));
         seed = (int)DateTime.Now.Ticks & 0x0000FFFF;
         creationTime = new DateTime(new Random(seed).Next(1971, 2071), new Random(seed).Next(1, 12), new Random(seed).Next(1, 28), new Random(seed).Next(0, 23), new Random(seed).Next(0, 59), new Random(seed).Next(1, 59));
         Console.WriteLine("\n\tSetCreationTimeUtc() to: [{0} {1}]", creationTime.ToLongDateString(), creationTime.ToLongTimeString());
         Directory.SetCreationTimeUtc(path, creationTime);
         actual = Directory.GetCreationTimeUtc(path);
         System.IO.Directory.SetCreationTimeUtc(path, creationTime);
         expected = System.IO.Directory.GetCreationTimeUtc(path);
         Console.WriteLine("\t\tAlphaFS  : [{0}]\n\t\tSystem.IO: [{1}]", actual, expected);
         Assert.AreEqual(expected, actual, "AlphaFS != System.IO");
         #endregion // SetCreationTime/Utc

         #region SetLastAccessTime/Utc
         Thread.Sleep(new Random().Next(250, 500));
         seed = (int)DateTime.Now.Ticks & 0x0000FFFF;
         DateTime lastAccessTime = new DateTime(new Random(seed).Next(1971, 2071), new Random(seed).Next(1, 12), new Random(seed).Next(1, 28), new Random(seed).Next(0, 23), new Random(seed).Next(0, 59), new Random(seed).Next(1, 59));
         Console.WriteLine("\n\tSetLastAccessTime() to: [{0} {1}]", lastAccessTime.ToLongDateString(), lastAccessTime.ToLongTimeString());
         Directory.SetLastAccessTime(path, lastAccessTime);
         actual = Directory.GetLastAccessTime(path);
         System.IO.Directory.SetLastAccessTime(path, lastAccessTime);
         expected = System.IO.Directory.GetLastAccessTime(path);
         Console.WriteLine("\t\tAlphaFS  : [{0}]\n\t\tSystem.IO: [{1}]", actual, expected);
         Assert.AreEqual(expected, actual, "AlphaFS != System.IO");


         Thread.Sleep(new Random().Next(250, 500));
         seed = (int)DateTime.Now.Ticks & 0x0000FFFF;
         lastAccessTime = new DateTime(new Random(seed).Next(1971, 2071), new Random(seed).Next(1, 12), new Random(seed).Next(1, 28), new Random(seed).Next(0, 23), new Random(seed).Next(0, 59), new Random(seed).Next(1, 59));
         Console.WriteLine("\n\tSetLastAccessTimeUtc() to: [{0} {1}]", lastAccessTime.ToLongDateString(), lastAccessTime.ToLongTimeString());
         Directory.SetLastAccessTimeUtc(path, lastAccessTime);
         actual = Directory.GetLastAccessTimeUtc(path);
         System.IO.Directory.SetLastAccessTimeUtc(path, lastAccessTime);
         expected = System.IO.Directory.GetLastAccessTimeUtc(path);
         Console.WriteLine("\t\tAlphaFS  : [{0}]\n\t\tSystem.IO: [{1}]", actual, expected);
         Assert.AreEqual(expected, actual, "AlphaFS != System.IO");
         #endregion // SetLastAccessTime/Utc

         #region SetLastWriteTime/Utc
         Thread.Sleep(new Random().Next(250, 500));
         seed = (int)DateTime.Now.Ticks & 0x0000FFFF;
         DateTime lastWriteTime = new DateTime(new Random(seed).Next(1971, 2071), new Random(seed).Next(1, 12), new Random(seed).Next(1, 28), new Random(seed).Next(0, 23), new Random(seed).Next(0, 59), new Random(seed).Next(1, 59));
         Console.WriteLine("\n\tSetLastWriteTime() to: [{0} {1}]", lastWriteTime.ToLongDateString(), lastWriteTime.ToLongTimeString());
         Directory.SetLastWriteTime(path, lastWriteTime);
         actual = Directory.GetLastWriteTime(path);
         System.IO.Directory.SetLastWriteTime(path, lastWriteTime);
         expected = System.IO.Directory.GetLastWriteTime(path);
         Console.WriteLine("\t\tAlphaFS  : [{0}]\n\t\tSystem.IO: [{1}]", actual, expected);
         Assert.AreEqual(expected, actual, "AlphaFS != System.IO");


         Thread.Sleep(new Random().Next(250, 500));
         seed = (int)DateTime.Now.Ticks & 0x0000FFFF;
         lastWriteTime = new DateTime(new Random(seed).Next(1971, 2071), new Random(seed).Next(1, 12), new Random(seed).Next(1, 28), new Random(seed).Next(0, 23), new Random(seed).Next(0, 59), new Random(seed).Next(1, 59));
         Console.WriteLine("\n\tSetLastWriteTimeUtc() to: [{0} {1}]", lastWriteTime.ToLongDateString(), lastWriteTime.ToLongTimeString());
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

      private void DumpSetTimestamps(bool isLocal)
      {
         Console.WriteLine("\n=== TEST {0} ===", isLocal ? Local : Network);
         string path = Path.Combine(Path.GetTempPath(), "Directory.SetTimestamps()-" + Path.GetRandomFileName());
         if (!isLocal) path = Path.LocalToUnc(path);

         Console.WriteLine("\nInput Path: [{0}]", path);

         Directory.CreateDirectory(path);

         Thread.Sleep(new Random().Next(250, 500));
         int seed = (int)DateTime.Now.Ticks & 0x0000FFFF;
         DateTime creationTime = new DateTime(new Random(seed).Next(1971, 2071), new Random(seed).Next(1, 12), new Random(seed).Next(1, 28), new Random(seed).Next(0, 23), new Random(seed).Next(0, 59), new Random(seed).Next(0, 59));

         Thread.Sleep(new Random().Next(250, 500));
         seed = (int)DateTime.Now.Ticks & 0x0000FFFF;
         DateTime lastAccessTime = new DateTime(new Random(seed).Next(1971, 2071), new Random(seed).Next(1, 12), new Random(seed).Next(1, 28), new Random(seed).Next(0, 23), new Random(seed).Next(0, 59), new Random(seed).Next(0, 59));

         Thread.Sleep(new Random().Next(250, 500));
         seed = (int)DateTime.Now.Ticks & 0x0000FFFF;
         DateTime lastWriteTime = new DateTime(new Random(seed).Next(1971, 2071), new Random(seed).Next(1, 12), new Random(seed).Next(1, 28), new Random(seed).Next(0, 23), new Random(seed).Next(0, 59), new Random(seed).Next(0, 59));

         Console.WriteLine("\n");
         Console.WriteLine("\tCreationTime  : [{0} {1}]", creationTime.ToLongDateString(), creationTime.ToLongTimeString());
         Console.WriteLine("\tLastAccessTime: [{0} {1}]", lastAccessTime.ToLongDateString(), lastAccessTime.ToLongTimeString());
         Console.WriteLine("\tLastWriteTime : [{0} {1}]", lastWriteTime.ToLongDateString(), lastWriteTime.ToLongTimeString());
         Console.WriteLine("\n");

         Directory.SetTimestamps(path, creationTime, lastAccessTime, lastWriteTime);

         DateTime actual = Directory.GetCreationTime(path);
         DateTime expected = System.IO.Directory.GetCreationTime(path);
         Console.WriteLine("\t\tAlphaFS: [{0}]\tSystem.IO: [{1}]", actual, expected);
         Assert.AreEqual(expected, actual, "AlphaFS != System.IO");

         actual = Directory.GetLastAccessTime(path);
         expected = System.IO.Directory.GetLastAccessTime(path);
         Console.WriteLine("\t\tAlphaFS: [{0}]\tSystem.IO: [{1}]", actual, expected);
         Assert.AreEqual(expected, actual, "AlphaFS != System.IO");

         actual = Directory.GetLastWriteTime(path);
         expected = System.IO.Directory.GetLastWriteTime(path);
         Console.WriteLine("\t\tAlphaFS: [{0}]\tSystem.IO: [{1}]", actual, expected);
         Assert.AreEqual(expected, actual, "AlphaFS != System.IO");




         creationTime = creationTime.ToUniversalTime();
         lastAccessTime = lastAccessTime.ToUniversalTime();
         lastWriteTime = lastWriteTime.ToUniversalTime();

         Console.WriteLine("\n");
         Console.WriteLine("\tCreationTimeUtc  : [{0} {1}]", creationTime.ToLongDateString(), creationTime.ToLongTimeString());
         Console.WriteLine("\tLastAccessTimeUtc: [{0} {1}]", lastAccessTime.ToLongDateString(), lastAccessTime.ToLongTimeString());
         Console.WriteLine("\tLastWriteTimeUtc : [{0} {1}]", lastWriteTime.ToLongDateString(), lastWriteTime.ToLongTimeString());
         Console.WriteLine("\n");

         Directory.SetTimestampsUtc(path, creationTime, lastAccessTime, lastWriteTime);

         actual = Directory.GetCreationTimeUtc(path);
         expected = System.IO.Directory.GetCreationTimeUtc(path);
         Console.WriteLine("\t\tAlphaFS: [{0}]\tSystem.IO: [{1}]", actual, expected);
         Assert.AreEqual(expected, actual, "AlphaFS != System.IO");

         actual = Directory.GetLastAccessTimeUtc(path);
         expected = System.IO.Directory.GetLastAccessTimeUtc(path);
         Console.WriteLine("\t\tAlphaFS: [{0}]\tSystem.IO: [{1}]", actual, expected);
         Assert.AreEqual(expected, actual, "AlphaFS != System.IO");

         actual = Directory.GetLastWriteTimeUtc(path);
         expected = System.IO.Directory.GetLastWriteTimeUtc(path);
         Console.WriteLine("\t\tAlphaFS: [{0}]\tSystem.IO: [{1}]", actual, expected);
         Assert.AreEqual(expected, actual, "AlphaFS != System.IO");

         Directory.Delete(path);
         Assert.IsTrue(!Directory.Exists(path));

         Console.WriteLine("\n");
      }

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
         seed = (int)DateTime.Now.Ticks & 0x0000FFFF;
         DateTime lastAccessTime = new DateTime(new Random(seed).Next(1971, 2071), new Random(seed).Next(1, 12), new Random(seed).Next(1, 28), new Random(seed).Next(0, 23), new Random(seed).Next(0, 59), new Random(seed).Next(0, 59));

         Thread.Sleep(new Random().Next(250, 500));
         seed = (int)DateTime.Now.Ticks & 0x0000FFFF;
         DateTime lastWriteTime = new DateTime(new Random(seed).Next(1971, 2071), new Random(seed).Next(1, 12), new Random(seed).Next(1, 28), new Random(seed).Next(0, 23), new Random(seed).Next(0, 59), new Random(seed).Next(0, 59));

         Directory.SetTimestamps(path, creationTime, lastAccessTime, lastWriteTime);

         Console.WriteLine("\n\tPath1 dates and times:");
         Console.WriteLine("\t\tCreationTime  : [{0} {1}]", creationTime.ToLongDateString(), creationTime.ToLongTimeString());
         Console.WriteLine("\t\tLastAccessTime: [{0} {1}]", lastAccessTime.ToLongDateString(), lastAccessTime.ToLongTimeString());
         Console.WriteLine("\t\tLastWriteTime : [{0} {1}]", lastWriteTime.ToLongDateString(), lastWriteTime.ToLongTimeString());

         Console.WriteLine("\n\tPath2 current dates and times:");
         Console.WriteLine("\t\tCreationTime  : [{0} {1}]", Directory.GetCreationTime(path2).ToLongDateString(), Directory.GetCreationTime(path2).ToLongTimeString());
         Console.WriteLine("\t\tLastAccessTime: [{0} {1}]", Directory.GetLastAccessTime(path2).ToLongDateString(), Directory.GetLastAccessTime(path2).ToLongTimeString());
         Console.WriteLine("\t\tLastWriteTime : [{0} {1}]", Directory.GetLastWriteTime(path2).ToLongDateString(), Directory.GetLastWriteTime(path2).ToLongTimeString());

         Directory.TransferTimestamps(path, path2);

         Console.WriteLine("\n\tPath2 dates and times after TransferTimestamps():");
         Console.WriteLine("\t\tCreationTime  : [{0} {1}]", Directory.GetCreationTime(path2).ToLongDateString(), Directory.GetCreationTime(path2).ToLongTimeString());
         Console.WriteLine("\t\tLastAccessTime: [{0} {1}]", Directory.GetLastAccessTime(path2).ToLongDateString(), Directory.GetLastAccessTime(path2).ToLongTimeString());
         Console.WriteLine("\t\tLastWriteTime : [{0} {1}]", Directory.GetLastWriteTime(path2).ToLongDateString(), Directory.GetLastWriteTime(path2).ToLongTimeString());

         Assert.AreEqual(Directory.GetCreationTime(path), Directory.GetCreationTime(path2));
         Assert.AreEqual(Directory.GetLastAccessTime(path), Directory.GetLastAccessTime(path2));
         Assert.AreEqual(Directory.GetLastWriteTime(path), Directory.GetLastWriteTime(path2));

         Directory.Delete(path);
         Assert.IsTrue(!Directory.Exists(path));
         Directory.Delete(path2);
         Assert.IsTrue(!Directory.Exists(path2));

         Console.WriteLine("\n");
      }
     
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

         DirectoryInfo alphaFsDi = Directory.CreateDirectory(tempPathDot, false, true);
         Assert.IsTrue(alphaFsDi.Name.EndsWith(characterDot), "Path should have a trailing dot.");
         Assert.IsFalse(Directory.Exists(tempPathDot), "Directory should not exist.");
         Assert.IsTrue(Directory.Exists(tempPathDot, true), "Directory should exist.");

         DirectoryInfo alphaFsDi2 = alphaFsDi.CreateSubdirectory("Directory-with-dot-" + characterDot, false);
         Assert.IsTrue(alphaFsDi2.Exists, "Directory should exist.");

         alphaFsDi2 = alphaFsDi.CreateSubdirectory("Directory-with-space-" + characterSpace, false);
         Assert.IsTrue(alphaFsDi2.Exists, "Directory should exist.");

         Directory.Delete(tempPathDot, true, true, true);
         Assert.IsFalse(Directory.Exists(tempPathDot, true), "Directory should not exist.");

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

         alphaFsDi = Directory.CreateDirectory(tempPathSpace, false, true);
         Assert.IsTrue(alphaFsDi.Name.EndsWith(characterSpace), "Path should have a trailing space.");
         Assert.IsFalse(Directory.Exists(tempPathSpace), "Directory should exist.");  // Because trailing space is removed.
         Assert.IsTrue(Directory.Exists(tempPathSpace, true), "Directory should exist.");

         alphaFsDi2 = alphaFsDi.CreateSubdirectory("Directory-with-space-" + characterSpace, false);
         Assert.IsTrue(alphaFsDi2.Exists, "Directory should exist.");

         alphaFsDi2 = alphaFsDi.CreateSubdirectory("Directory-with-dot-" + characterDot, false);
         Assert.IsTrue(alphaFsDi2.Exists, "Directory should exist.");

         Directory.Delete(tempPathSpace, true, true, true);
         Assert.IsFalse(Directory.Exists(tempPathSpace, true), "Directory should not exist.");

         #endregion // AlphaFS

         #endregion // TrailingSpace

         Console.WriteLine("\tClass DirectoryInfo()\t{0}", Reporter(true));

         #endregion // Directory() Class

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
         Console.WriteLine("\nPlease see unit test: CreateDirectory()");
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
         Console.WriteLine("Directory.GetCreationTime()");
         
         DumpGetDirectoryTime(true);
         DumpGetDirectoryTime(false);
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

         string tempPath = Path.GetTempPath();
         string orgPathActual = Directory.GetCurrentDirectory();
         string orgPathExpected = System.IO.Directory.GetCurrentDirectory();
         Assert.AreEqual(orgPathExpected, orgPathActual, "AlphaFS != System.IO");

         Directory.SetCurrentDirectory(tempPath);

         foreach (string path in InputPaths)
         {
            Console.WriteLine("\nInput Path: [{0}]", path);
            string method = null;

            try
            {
               method = "AlphaFS";
               string actual = Directory.GetDirectoryRoot(path);
               Console.WriteLine("\t\tAlphaFS  : [{0}]", actual);

               method = "System.IO";
               string expected = System.IO.Directory.GetDirectoryRoot(path);
               Console.WriteLine("\t\tSystem.IO: [{0}]", expected);

               Assert.AreEqual(expected, actual);
               Assert.AreEqual(Directory.GetCurrentDirectory(), System.IO.Directory.GetCurrentDirectory(), "AlphaFS != System.IO");

               ++pathCnt;
            }
            catch (Exception ex)
            {
               Console.WriteLine("\t\t{0} Exception: [{1}]", method, ex.Message.Replace(Environment.NewLine, string.Empty));

               // Exception to the Exception.
               if (path != null && !path.StartsWith(Path.GlobalRootPrefix, StringComparison.OrdinalIgnoreCase) &&
                  !ex.Message.Equals("Illegal characters in path.", StringComparison.OrdinalIgnoreCase))
               {
                  allOk = false;
                  errorCnt++;
               }
            }
         }

         Assert.IsTrue(pathCnt > 0);

         Directory.SetCurrentDirectory(tempPath);
         Assert.AreEqual(Directory.GetCurrentDirectory(), System.IO.Directory.GetCurrentDirectory(), "AlphaFS != System.IO");
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

         foreach (string input in InputPaths)
         {
            string path = input;

            try
            {
               DirectoryInfo diActual = Directory.GetParent(path);
               DirectoryInfo diExpected = Directory.GetParent(path);

               if (diActual == null || diExpected == null)
               {
                  Console.WriteLine("\n\t#{0:000}\tInput Path: [{1}]\n\t\tAlphaFS   : [{2}]\n\t\tSystem.IO : [{3}]", ++pathCnt, input, diActual, diExpected);
                  Assert.AreEqual(diActual, diExpected);
               }
               else
               {
                  string actual = diActual.FullName;
                  string expected = diExpected.FullName;
                  Console.WriteLine("\n\t#{0:000}\tInput Path: [{1}]\n\t\tAlphaFS   : [{2}]\n\t\tSystem.IO : [{3}]", ++pathCnt, input, diActual.FullName, diExpected.FullName);
                  Assert.AreEqual(expected, actual);
               }
            }
            catch (Exception ex)
            {
               Console.WriteLine("\n\tCaught Exception: [{0}]", ex.Message.Replace(Environment.NewLine, string.Empty));

               // Exception to the Exception.
               if (path != null && !path.StartsWith(Path.GlobalRootPrefix, StringComparison.OrdinalIgnoreCase))
               {
                  allOk = false;
                  errorCnt++;
               }
            }
         }

         Assert.AreEqual(true, allOk, "Encountered: [{0}] paths where AlphaFS != System.IO", errorCnt);
      }

      #endregion // GetParent

      #region Move

      [TestMethod]
      public void Move()
      {
         Console.WriteLine("Directory.Move()");

         string path = Path.GetTempPath("Directory.Move()-" + Path.GetRandomFileName());

         DirectoryInfo sourceDir = new DirectoryInfo(Path.GetTempPath(path + @"\sourceDir"));
         DirectoryInfo destDir = new DirectoryInfo(Path.GetTempPath(path + @"\destinationDir"));
         string the3rdDir = Path.LocalToUnc(Path.GetTempPath(path + @"\the3rdDirectory"));

         // Create some directories and files.
         CreateDirectoriesAndFiles(sourceDir.FullName, 10, true);

         Console.WriteLine("\n\tSRC directory: [{0}]", sourceDir.FullName);

         #region MoveTo

         StopWatcher(true);
         sourceDir.MoveTo(destDir.FullName);
         Console.WriteLine("\n\tDirectoryInfo.MoveTo()");
         Console.WriteLine("\t\tDST directory: [{0}]\n\t{1}", destDir.FullName, Reporter());

         Assert.IsFalse(sourceDir.Exists);
         Assert.IsTrue(destDir.Exists);

         #endregion // MoveTo

         #region Move

         string pathUnc = Path.LocalToUnc(destDir.FullName);
         Assert.IsTrue(Directory.Exists(pathUnc), "Share inaccessible: {0}\n", pathUnc);

         StopWatcher(true);
         Directory.Move(destDir.FullName, the3rdDir);
         Console.WriteLine("\n\tDirectory.Move()");
         Console.WriteLine("\t\t3RD directoriy: [{0}]\n\t{1}", the3rdDir, Reporter());
         Assert.IsTrue(Directory.Exists(the3rdDir));
         Directory.Delete(the3rdDir, true);
         Assert.IsFalse(Directory.Exists(the3rdDir), "Cleanup failed: Directory should have been removed.");

         #endregion // Move

         #region Move, delete destination first.

         // Create some directories and files.
         CreateDirectoriesAndFiles(sourceDir.FullName, 10, true);

         StopWatcher(true);
         sourceDir.MoveTo(new DirectoryInfo(the3rdDir).FullName, true);
         Console.WriteLine("\n\tDirectory.MoveTo()");
         Console.WriteLine("\t\t3RD directory: [{0}]\n\t{1}", the3rdDir, Reporter());
         Assert.IsTrue(Directory.Exists(the3rdDir));

         #endregion // Move, delete destination first.

         Assert.IsFalse(sourceDir.Exists);
         Assert.IsTrue(destDir.Exists);

         Directory.Delete(path, true);
         Assert.IsFalse(Directory.Exists(path), "Cleanup failed: Directory should have been removed.");
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
         Assert.IsFalse(diAlpha.Exists);
      }

      #endregion // SetAccessControl
      
      #region SetCreationTime

      [TestMethod]
      public void SetCreationTime()
      {
         Console.WriteLine("Directory.SetCreationTime()");

         DumpSetDirectoryTime(true);
         DumpSetDirectoryTime(false);
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

      #region CountDirectories

      [TestMethod]
      public void AlphaFS_CountDirectories()
      {
         Console.WriteLine("Directory.CountDirectories()");

         string path = SysRoot;
         long dirs = 0;

         string searchPattern = Path.WildcardStarMatchAll;
         SearchOption searchOption = SearchOption.AllDirectories;

         Console.WriteLine("\n\tsearchPattern: \"{0}\", abort on error.", searchPattern);

         #region Exception
         
         bool gotException = false;
         try
         {
            dirs = Directory.CountDirectories(path, searchPattern, searchOption, false, false);
         }
         catch (Exception ex)
         {
            gotException = true;
            Console.WriteLine("\n\tCaught Exception: [{0}]", ex.Message.Replace(Environment.NewLine, string.Empty));
         }
         Console.WriteLine("\n\tCaught Exception: [{0}]", gotException);
         Assert.IsTrue(gotException);
         Console.WriteLine("\n\tdirectory\t = [{0}]\n\tSubdirectories = [{1}]\n{2}\n", path, dirs, Reporter());

         #endregion // Exception

         searchPattern = Path.WildcardStarMatchAll;
         searchOption = SearchOption.AllDirectories;

         Console.WriteLine("\n\tsearchPattern: \"{0}\", continue on error. Running as Administrator will count more directories.", searchPattern);

         StopWatcher(true);
         dirs = Directory.CountDirectories(path, searchPattern, searchOption, true, false);
         Console.WriteLine("\n\tdirectory\t = [{0}]\n\tSubdirectories = [{1}]\n{2}\n", path, dirs, Reporter());
         Assert.IsTrue(dirs > 0);


         Console.WriteLine("\n\tsearchPattern: \"{0}\", continue on error. Running as Administrator and using PrivilegeEnabler(Privilege.Backup) will count even more directories.", searchPattern);
         StopWatcher(true);
         using (new PrivilegeEnabler(Privilege.Backup))
         {
            dirs = Directory.CountDirectories(path, searchPattern, searchOption, true, false);
            Console.WriteLine("\n\tDirectory\t = [{0}]\n\tSubdirectories = [{1}]\n{2}\n", path, dirs, Reporter());
            Assert.IsTrue(dirs > 0);
         }

      }

      #endregion // CountDirectories

      #region CountFiles

      [TestMethod]
      public void AlphaFS_CountFiles()
      {
         Console.WriteLine("Directory.CountFiles()");

         string searchPattern = Path.WildcardStarMatchAll;
         SearchOption searchOption = SearchOption.AllDirectories;

         string path = SysRoot32;
         long files = 0;
         Console.WriteLine("\n\t{0}, abort on error.", searchOption);

         #region Exception

         bool gotException = false;
         try
         {
            files = Directory.CountFiles(path, searchPattern, searchOption, false, false);
         }
         catch (Exception ex)
         {
            gotException = true;
            Console.WriteLine("\n\tCaught Exception: [{0}]", ex.Message.Replace(Environment.NewLine, string.Empty));
         }
         Console.WriteLine("\n\tCaught Exception: [{0}]", gotException);
         Assert.IsTrue(gotException);
         Console.WriteLine("\n\tDirectory = [{0}]\n\tFiles  = [{1}]\n{2}\n", path, files, Reporter());

         #endregion // Exception

         StopWatcher(true);
         files = Directory.CountFiles(path, searchPattern, searchOption, true, false);
         Console.WriteLine("\n\tDirectory = [{0}]\n\tFiles  = [{1}]\n{2}\n", path, files, Reporter());
         Assert.IsTrue(files > 0);

         Console.WriteLine("\n\t{0}, continue on error. Running as Administrator will count more files.", searchOption);
         StopWatcher(true);
         files = Directory.CountFiles(path, searchPattern, searchOption, true, false);
         Console.WriteLine("\n\tDirectory = [{0}]\n\tFiles  = [{1}]\n{2}\n", path, files, Reporter());
         Assert.IsTrue(files > 0);

         Console.WriteLine("\n\t{0}, continue on error. Running as Administrator and using PrivilegeEnabler(Privilege.Backup) will count even more files.", searchOption);
         StopWatcher(true);
         using (new PrivilegeEnabler(Privilege.Backup))
         {
            files = Directory.CountFiles(path, searchPattern, searchOption, true, false);
            Console.WriteLine("\n\tDirectory = [{0}]\n\tFiles  = [{1}]\n{2}\n", path, files, Reporter());
            Assert.IsTrue(files > 0);
         }

      }

      #endregion // CountFiles

      #region DeleteEmpty

      [TestMethod]
      public void AlphaFS_DeleteEmpty()
      {
         Console.WriteLine("Directory.DeleteEmpty()");

         string tempPath = Path.Combine(Path.GetTempPath(), "Directory.DeleteEmpty()-" + Path.GetRandomFileName());
         long dirs0, dirs1, files0, files1;

         const int maxDepth = 10;
         const int totalDirectories = (maxDepth * maxDepth) + maxDepth;  // maxDepth = 10: 110 directories and 110 files.
         const int emptyDirectories = (maxDepth * maxDepth) / 2;         // 50 empty directories.
         const int remainingDirectories = totalDirectories - emptyDirectories;   // 60 remaining directories.

         Console.WriteLine("\nInput Path: [{0}]", tempPath);
         CreateDirectoriesAndFiles(tempPath, maxDepth, true);

         string searchPattern = Path.WildcardStarMatchAll;
         SearchOption searchOption = SearchOption.AllDirectories;

         StopWatcher(true);
         dirs0 = Directory.CountDirectories(tempPath, searchPattern, searchOption, true, false);
         files0 = Directory.CountFiles(tempPath, searchPattern, searchOption, true, false);
         Console.WriteLine("\nCountDirectories(): [{0}]\nCountFiles()      : [{1}]\n{2}", dirs0, files0, Reporter());

         StopWatcher(true);
         bool deleteOk = false;
         try
         {
            Directory.DeleteEmpty(tempPath, false);
            deleteOk = true;
         }
         catch
         {
         }


         // Issue-21123: Method Directory- and DirectoryInfo.DeleteEmpty() also deletes the given directories when totally empty.
         Assert.IsTrue(Directory.Exists(tempPath), "Directory should exist.");

         Console.WriteLine("\nDirectory.DeleteEmpty() (Should be True): [{0}]\n{1}", deleteOk, Reporter());
         Assert.IsTrue(deleteOk, "DeleteEmpty() failed.");
         
         searchPattern = Path.WildcardStarMatchAll;
         searchOption = SearchOption.AllDirectories;

         StopWatcher(true);
         dirs1 = Directory.CountDirectories(tempPath, searchPattern, searchOption, true, false);
         files1 = Directory.CountFiles(tempPath, searchPattern, searchOption, true, false);
         Console.WriteLine("\nCountDirectories() (Should be 60): [{0}]\nCountFiles() (Should be 110)     : [{1}]\n{2}", dirs1, files1, Reporter());
         Assert.IsTrue(dirs1 != dirs0);
         Assert.IsTrue(dirs1 == remainingDirectories);
         Assert.IsTrue(files1 == files0);

         Directory.Delete(tempPath, true);
         bool directoryNotExists = !Directory.Exists(tempPath);
         Assert.IsTrue(directoryNotExists);

         Assert.IsTrue((emptyDirectories + remainingDirectories) == totalDirectories);
      }

      #endregion // Delete

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

      #region EnumerateFileIdBothDirectoryInfo

      [TestMethod]
      public void AlphaFS_EnumerateFileIdBothDirectoryInfo()
      {
         Console.WriteLine("Directory.EnumerateFileIdBothDirectoryInfo()");

         DumpEnumerateFileIdBothDirectoryInfo(true);
         DumpEnumerateFileIdBothDirectoryInfo(false);
      }

      #endregion // EnumerateFileIdBothDirectoryInfo

      #region EnumerateLogicalDrives

      [TestMethod]
      public void AlphaFS_EnumerateLogicalDrives()
      {
         Console.WriteLine("Directory.EnumerateLogicalDrives()");
         Console.WriteLine("\nPlease see unit test: GetLogicalDrives()");
      }

      #endregion // EnumerateLogicalDrives
      
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
               Console.Write("\t#{0:000}\tCaught Exception for directory: [{1}]\t[{2}]\n", cnt, dir, ex.Message.Replace(Environment.NewLine, string.Empty));
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


      #region AlphaFS___DirectoryWithTrailingDotSpace

      [TestMethod]
      public void AlphaFS___DirectoryWithTrailingDotSpace()
      {
         Console.WriteLine(".NET does not support the creation/manipulation of directory with a trailing dot or space.");
         Console.WriteLine("These will be stripped due to path normalization.");

         Console.WriteLine("\nThe AlphaFS Directory() class contains overloaded methods which have the");
         Console.WriteLine("isFullPath parameter that enables you to bypass this .NET limitation.\n");

         DumpDirectoryTrailingDotSpace(true);
         DumpDirectoryTrailingDotSpace(false);
      }

      #endregion // AlphaFS___DirectoryWithTrailingDotSpace

      #endregion // AlphaFS

      #endregion Unit Test Callers
   }
}