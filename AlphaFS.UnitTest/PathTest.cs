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

using Alphaleonis.Win32;
using Alphaleonis.Win32.Filesystem;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Win32.SafeHandles;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using Directory = Alphaleonis.Win32.Filesystem.Directory;
using DriveInfo = Alphaleonis.Win32.Filesystem.DriveInfo;
using File = Alphaleonis.Win32.Filesystem.File;
using NativeMethods = Alphaleonis.Win32.Filesystem.NativeMethods;
using Path = Alphaleonis.Win32.Filesystem.Path;

namespace AlphaFS.UnitTest
{
   /// <summary>This is a test class for Path and is intended to contain all Path Unit Tests.</summary>
   [TestClass]
   public class PathTest
   {
      #region PathTest Helpers

      private static readonly string StartupFolder = AppDomain.CurrentDomain.BaseDirectory;
      private static readonly string SysDrive = Environment.GetEnvironmentVariable("SystemDrive");
      private static readonly string SysRoot = Environment.GetEnvironmentVariable("SystemRoot");

      private const string SpecificX3 = "Windows XP and Windows Server 2003 specific.";
      private const string TextTrue = "IsTrue";
      private const string TextFalse = "IsFalse";

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

      private static void Dump83Path(string fullPath)
      {
         Console.WriteLine("\nInput Path: [{0}]", fullPath);

         if (Directory.Exists(fullPath))
         {
            // GetShort83Path()
            StopWatcher(true);
            string short83Path = Path.GetShort83Path(fullPath);
            string reporter = Reporter(true);
            bool isShort83Path = !string.IsNullOrWhiteSpace(short83Path) && !short83Path.Equals(fullPath) && Directory.Exists(short83Path);
            bool hasTilde = !string.IsNullOrWhiteSpace(short83Path) && short83Path.IndexOf('~') >= 0;

            Console.WriteLine("\t{0, 20} == [{1}]: [{2}]: [{3}]\n\t{4}", "GetShort83Path()", TextTrue, isShort83Path, short83Path, reporter);
            Assert.IsTrue(isShort83Path);
            Assert.IsTrue(hasTilde); // A bit tricky if fullPath is already a shortPath.

            // GetLongFrom83Path()
            StopWatcher(true);
            string longFrom83Path = Path.GetLongFrom83Path(short83Path);
            reporter = Reporter(true);
            bool isLongFrom83Path = !string.IsNullOrWhiteSpace(longFrom83Path) && !longFrom83Path.Equals(short83Path) && Directory.Exists(longFrom83Path);
            bool noTilde = !string.IsNullOrWhiteSpace(longFrom83Path) && longFrom83Path.IndexOf('~') == -1;

            Console.WriteLine("\n\t{0, 20} == [{1}]: [{2}]: [{3}]\n\t{4}\n", "GetLongFrom83Path()", TextTrue, isLongFrom83Path, longFrom83Path, reporter);
            Assert.IsTrue(isLongFrom83Path);
            Assert.IsTrue(noTilde);
         }
         else
         {
            Console.WriteLine("\tShare inaccessible: {0}", fullPath);
         }
      }
      
      private static void DumpGetDirectoryNameWithoutRoot(bool isLocal)
      {
         Console.WriteLine("\n=== TEST {0} ===", isLocal ? "LOCAL" : "NETWORK");

         const string neDir = "Non-Existing Directory";
         const string sys32 = "system32";

         string fullPath = Path.Combine(Environment.SystemDirectory, neDir);
         if (!isLocal) fullPath = Path.LocalToUnc(fullPath);

         StopWatcher(true);
         string directoryNameWithoutRoot = Path.GetDirectoryNameWithoutRoot(fullPath);
         Console.WriteLine("\nInput Path: [{0}]\n\tGetDirectoryNameWithoutRoot() (Should be: [{1}]): [{2}]", fullPath, sys32, directoryNameWithoutRoot);
         Assert.IsTrue(directoryNameWithoutRoot.Equals(sys32, StringComparison.OrdinalIgnoreCase));


         
         fullPath = Path.Combine(fullPath, "Non-Existing file.txt");
         if (!isLocal) fullPath = Path.LocalToUnc(fullPath);

         StopWatcher(true);
         directoryNameWithoutRoot = Path.GetDirectoryNameWithoutRoot(fullPath);
         Console.WriteLine("\nInput Path: [{0}]\n\tGetDirectoryNameWithoutRoot() (Should be: [{1}]): [{2}]", fullPath, neDir, directoryNameWithoutRoot);
         Assert.IsTrue(directoryNameWithoutRoot.Equals(neDir, StringComparison.OrdinalIgnoreCase));


         
         fullPath = SysRoot;
         if (!isLocal) fullPath = Path.LocalToUnc(fullPath);

         StopWatcher(true);
         directoryNameWithoutRoot = Path.GetDirectoryNameWithoutRoot(fullPath);
         Console.WriteLine("\nInput Path: [{0}]\n\tGetDirectoryNameWithoutRoot() (Should be: [null]): [{1}]", fullPath, directoryNameWithoutRoot ?? "null");
         Assert.AreEqual(directoryNameWithoutRoot, null);

         Console.WriteLine("\n");
      }

      private void DumpGetFinalPathNameByHandle(bool isLocal)
      {
         Console.WriteLine("\n=== TEST {0} ===", isLocal ? "LOCAL" : "NETWORK");

         string tempFile = Path.GetTempFileName();
         if (!isLocal) tempFile = Path.LocalToUnc(tempFile);

         string longTempStream = Path.LongPathPrefix + tempFile;
         bool gotFileNameNormalized;
         bool gotFileNameOpened;
         bool gotVolumeNameDos;
         bool gotVolumeNameGuid;
         bool gotVolumeNameNt;
         bool gotVolumeNameNone;
         bool gotSomething;

         using (FileStream stream = File.Create(tempFile))
         {
            // For Windows versions < Vista, the file must be > 0 bytes.
            if (!NativeMethods.IsAtLeastWindowsVista)
               stream.WriteByte(1);

            SafeFileHandle handle = stream.SafeFileHandle;

            StopWatcher(true);
            string fileNameNormalized = Path.GetFinalPathNameByHandle(handle);
            string fileNameOpened = Path.GetFinalPathNameByHandle(handle, FinalPathFormats.FileNameOpened);

            string volumeNameDos = Path.GetFinalPathNameByHandle(handle, FinalPathFormats.None);
            string volumeNameGuid = Path.GetFinalPathNameByHandle(handle, FinalPathFormats.VolumeNameGuid);
            string volumeNameNt = Path.GetFinalPathNameByHandle(handle, FinalPathFormats.VolumeNameNT);
            string volumeNameNone = Path.GetFinalPathNameByHandle(handle, FinalPathFormats.VolumeNameNone);

            // These three output the same.
            gotFileNameNormalized = !string.IsNullOrWhiteSpace(fileNameNormalized) && longTempStream.Equals(fileNameNormalized);
            gotFileNameOpened = !string.IsNullOrWhiteSpace(fileNameOpened) && longTempStream.Equals(fileNameOpened);
            gotVolumeNameDos = !string.IsNullOrWhiteSpace(volumeNameDos) && longTempStream.Equals(volumeNameDos);

            gotVolumeNameGuid = !string.IsNullOrWhiteSpace(volumeNameGuid) && volumeNameGuid.StartsWith(Path.VolumePrefix) && volumeNameGuid.EndsWith(volumeNameNone);
            gotVolumeNameNt = !string.IsNullOrWhiteSpace(volumeNameNt) && volumeNameNt.StartsWith(Path.DevicePrefix);
            gotVolumeNameNone = !string.IsNullOrWhiteSpace(volumeNameNone) && tempFile.EndsWith(volumeNameNone);

            Console.WriteLine("\nInput Path: [{0}]", tempFile);
            Console.WriteLine("\n\tFilestream.Name  : [{0}]", stream.Name);
            Console.WriteLine("\tFilestream.Length: [{0}] (Note: For Windows versions < Vista, the file must be > 0 bytes.)\n", NativeMethods.UnitSizeToText(stream.Length));

            Console.WriteLine("\tFinalPathFormats.None          : [{0}]", fileNameNormalized);
            Console.WriteLine("\tFinalPathFormats.FileNameOpened: [{0}]", fileNameOpened);
            Console.WriteLine("\tFinalPathFormats.VolumeNameDos : [{0}]", volumeNameDos);
            Console.WriteLine("\tFinalPathFormats.VolumeNameGuid: [{0}]", volumeNameGuid);
            Console.WriteLine("\tFinalPathFormats.VolumeNameNT  : [{0}]", volumeNameNt);
            Console.WriteLine("\tFinalPathFormats.VolumeNameNone: [{0}]", volumeNameNone);

            Console.WriteLine("\n\t{0}\n", Reporter(true));

            gotSomething = true;
         }


         bool fileExists = File.Exists(tempFile);

         File.Delete(tempFile, true);
         Assert.IsFalse(File.Exists(tempFile), "Cleanup failed: File should have been removed.");

         Assert.IsTrue(fileExists);
         Assert.IsTrue(gotFileNameNormalized);
         Assert.IsTrue(gotFileNameOpened);
         Assert.IsTrue(gotVolumeNameDos);
         Assert.IsTrue(gotVolumeNameGuid);
         Assert.IsTrue(gotVolumeNameNt);
         Assert.IsTrue(gotVolumeNameNone);
         Assert.IsTrue(gotSomething);
      }

      private static void DumpGetRemoteNameInfo()
      {
         int cnt = 0;
         foreach (string drive in Directory.GetLogicalDrives().Where(drive => new DriveInfo(drive).IsUnc))
         {
            ++cnt;

            StopWatcher(true);
            string gmCn = Path.GetMappedConnectionName(drive);
            string gmUn = Path.GetMappedUncName(drive);
            Console.WriteLine("\n\tPath: [{0}]\tGetMappedConnectionName(): [{1}]", drive, gmCn);
            Console.WriteLine("\tPath: [{0}]\tGetMappedUncName()       : [{1}]\n{2}", drive, gmUn, Reporter());

            Assert.IsTrue(!string.IsNullOrWhiteSpace(gmCn));
            Assert.IsTrue(!string.IsNullOrWhiteSpace(gmUn));
         }

         if (cnt == 0)
            Assert.Fail("No mapped network drives found.");
      }

      private static void DumpGetSuffixedDirectoryName(bool isLocal)
      {
         Console.WriteLine("\n=== TEST {0} ===", isLocal ? "LOCAL" : "NETWORK");

         string neDir = "Non-Existing Directory";
         string sys32 = Environment.SystemDirectory + Path.DirectorySeparator;

         string fullPath = Path.Combine(Environment.SystemDirectory, neDir);
         if (!isLocal)
         {
            fullPath = Path.LocalToUnc(fullPath);
            sys32 = Path.LocalToUnc(sys32);
         }

         StopWatcher(true);
         string suffixedDirectoryName = Path.GetSuffixedDirectoryName(fullPath);
         Console.WriteLine("\nInput Path: [{0}]\n\tGetSuffixedDirectoryName() (Should be: [{1}]): [{2}]\n{3}", fullPath, sys32, suffixedDirectoryName, Reporter(false, true));
         Assert.IsTrue(suffixedDirectoryName.Equals(sys32, StringComparison.OrdinalIgnoreCase));



         fullPath = Path.Combine(fullPath, "Non-Existing file.txt");
         neDir = Path.Combine(Environment.SystemDirectory, neDir) + Path.DirectorySeparator; 
         if (!isLocal)
         {
            fullPath = Path.LocalToUnc(fullPath);
            neDir = Path.LocalToUnc(neDir);
         }
         
         StopWatcher(true);
         suffixedDirectoryName = Path.GetSuffixedDirectoryName(fullPath);
         Console.WriteLine("\nInput Path: [{0}]\n\tGetSuffixedDirectoryName() (Should be: [{1}]): [{2}]\n{3}", fullPath, neDir, suffixedDirectoryName, Reporter(false, true));
         Assert.IsTrue(suffixedDirectoryName.Equals(neDir, StringComparison.OrdinalIgnoreCase));



         fullPath = SysRoot;
         if (!isLocal) fullPath = Path.LocalToUnc(fullPath);

         StopWatcher(true);
         suffixedDirectoryName = Path.GetSuffixedDirectoryName(fullPath);
         Console.WriteLine("\nInput Path: [{0}]\n\tGetSuffixedDirectoryName() (Should be: [null]): [{1}]\n{2}", fullPath, suffixedDirectoryName ?? "null", Reporter(false, true));
         Assert.AreEqual(suffixedDirectoryName, null);


         fullPath = SysDrive + Path.DirectorySeparator;
         if (!isLocal) fullPath = Path.LocalToUnc(fullPath);

         StopWatcher(true);
         suffixedDirectoryName = Path.GetSuffixedDirectoryNameWithoutRoot(fullPath);
         Console.WriteLine("\nInput Path: [{0}]\n\tGetSuffixedDirectoryName() (Should be: [null]): [{1}]\n{2}", fullPath, suffixedDirectoryName ?? "null", Reporter(false, true));
         Assert.AreEqual(suffixedDirectoryName, null);

         Console.WriteLine("\n");
      }

      private void DumpGetSuffixedDirectoryNameWithoutRoot(bool isLocal)
      {
         Console.WriteLine("\n=== TEST {0} ===", isLocal ? "LOCAL" : "NETWORK");

         string neDir = "Non-Existing Directory";
         string sys32 = (SysRoot + Path.DirectorySeparator + "system32" + Path.DirectorySeparator).Replace(SysDrive + Path.DirectorySeparator, "");

         string fullPath = Path.Combine(SysRoot + Path.DirectorySeparator + "system32" + Path.DirectorySeparator, neDir);
         if (!isLocal) fullPath = Path.LocalToUnc(fullPath);

         StopWatcher(true);
         string suffixedDirectoryNameWithoutRoot = Path.GetSuffixedDirectoryNameWithoutRoot(fullPath);
         Console.WriteLine("\nInput Path: [{0}]\n\tGetSuffixedDirectoryName() (Should be: [{1}]): [{2}]\n{3}", fullPath, sys32, suffixedDirectoryNameWithoutRoot, Reporter(false, true));
         Assert.IsTrue(suffixedDirectoryNameWithoutRoot.Equals(sys32, StringComparison.OrdinalIgnoreCase), "Path mismatch.");



         fullPath = Path.Combine(fullPath, "Non-Existing file.txt");
         neDir = (Path.Combine(Environment.SystemDirectory, neDir) + Path.DirectorySeparator).Replace(SysDrive + Path.DirectorySeparator, "");
         if (!isLocal) fullPath = Path.LocalToUnc(fullPath);
         
         StopWatcher(true);
         suffixedDirectoryNameWithoutRoot = Path.GetSuffixedDirectoryNameWithoutRoot(fullPath);
         Console.WriteLine("\nInput Path: [{0}]\n\tGetSuffixedDirectoryName() (Should be: [{1}]): [{2}]\n{3}", fullPath, neDir, suffixedDirectoryNameWithoutRoot, Reporter(false, true));
         Assert.IsTrue(suffixedDirectoryNameWithoutRoot.Equals(neDir, StringComparison.OrdinalIgnoreCase), "Path mismatch.");



         fullPath = SysRoot;
         if (!isLocal) fullPath = Path.LocalToUnc(fullPath);

         StopWatcher(true);
         suffixedDirectoryNameWithoutRoot = Path.GetSuffixedDirectoryNameWithoutRoot(fullPath);
         Console.WriteLine("\nInput Path: [{0}]\n\tGetSuffixedDirectoryName() (Should be: [null]): [{1}]\n{2}", fullPath, suffixedDirectoryNameWithoutRoot ?? "null", Reporter(false, true));
         Assert.AreEqual(suffixedDirectoryNameWithoutRoot, null, "Path mismatch.");



         fullPath = SysDrive + Path.DirectorySeparator;
         if (!isLocal) fullPath = Path.LocalToUnc(fullPath);

         StopWatcher(true);
         suffixedDirectoryNameWithoutRoot = Path.GetSuffixedDirectoryNameWithoutRoot(fullPath);
         Console.WriteLine("\nInput Path: [{0}]\n\tGetSuffixedDirectoryName() (Should be: [null]): [{1}]\n{2}", fullPath, suffixedDirectoryNameWithoutRoot ?? "null", Reporter(false, true));
         Assert.AreEqual(suffixedDirectoryNameWithoutRoot, null, "Path mismatch.");

         Console.WriteLine("\n");
      }

      private void DumpIsLocalPath(bool isLocal)
      {
         Console.WriteLine("\n=== TEST {0} ===\n", isLocal ? "LOCAL" : "NETWORK");

         int isLocalPath = 0;
         int errorCnt = 0;
         int cnt = 0;

         StopWatcher(true);
         foreach (string input in InputPaths)
         {
            try
            {
               bool actual = Path.IsLocalPath(input);
               Console.WriteLine("\t#{0:000}\tIsLocalPath(): [{1}]\t\tInput Path: [{2}]", ++cnt, actual, input);

               if (actual)
                  isLocalPath++;
            }
            catch (Exception ex)
            {
               Console.WriteLine("\n\t\tCaught Exception: [{0}]", ex.Message.Replace(Environment.NewLine, string.Empty));

               // Exception to the Exception.
               if (input != null && !input.StartsWith(Path.GlobalRootPrefix, StringComparison.OrdinalIgnoreCase))
               {
                  errorCnt++;
               }
            }
         }
         Console.WriteLine("\n\t{0}\n", Reporter(true));

         // Hand counted 26 True's.
         Assert.AreEqual(26, isLocalPath, "Numbers of matching local paths do not match.", errorCnt);
         Console.WriteLine();
      }

      private void DumpIsLongPath(bool isLocal)
      {
         Console.WriteLine("\n=== TEST {0} ===\n", isLocal ? "LOCAL" : "NETWORK");
         string path = Path.GetFullPath(Directory.GetCurrentDirectory());
         if (!isLocal) path = Path.LocalToUnc(path);

         string longPath = Path.GetLongPath(path);
         
         // True
         bool isLongPath = Path.IsLongPath(longPath) && longPath.StartsWith(Path.LongPathPrefix);
         Console.WriteLine("Input Path: [{0}]\n", longPath);
         Console.WriteLine("\tIsLongPath() (Should be True): [{0}]\n", isLongPath);

         // False
         path = Path.GetFullPath(Directory.GetCurrentDirectory());
         bool isNotLongPath = Path.IsLongPath(path) && path.StartsWith(Path.LongPathPrefix);
         Console.WriteLine("Input Path: [{0}]\n", path);
         Console.WriteLine("\tIsLongPath() (Should be False): [{0}]", isNotLongPath);

         Assert.IsTrue(isLongPath, "Path should be a LongPath.");
         Assert.IsFalse(isNotLongPath, "Path should not be a LongPath.");
         Console.WriteLine();
      }

      private void DumpIsUncPath(bool isLocal)
      {
         Console.WriteLine("\n=== TEST {0} ===\n", isLocal ? "LOCAL" : "NETWORK");

         int isUncPath = 0;
         int isNotUncPath = 0;
         int errorCnt = 0;
         int cnt = 0;

         StopWatcher(true);
         foreach (string input in InputPaths)
         {
            try
            {
               bool actual = Path.IsUncPath(input);
               Console.WriteLine("\t#{0:000}\tIsUncPath(): [{1}]\t\tInput Path: [{2}]", ++cnt, actual, input);

               if (actual)
                  isUncPath++;
               else
                  isNotUncPath++;
            }
            catch (Exception ex)
            {
               Console.WriteLine("\n\t\tCaught Exception: [{0}]", ex.Message.Replace(Environment.NewLine, string.Empty));

               // Exception to the Exception.
               if (input != null && !input.StartsWith(Path.GlobalRootPrefix, StringComparison.OrdinalIgnoreCase))
               {
                  //allOk = false;
                  errorCnt++;
               }
            }
         }
         Console.WriteLine("\n\t{0}\n", Reporter(true));

         // Hand counted 30 True's.
         Assert.AreEqual(30, isUncPath, "Numbers of matching UNC paths do not match.", errorCnt);
         Assert.AreEqual(36, isNotUncPath, "Numbers of matching UNC paths do not match.", errorCnt);
         Console.WriteLine();
      }

      private void DumpLocalToUnc(bool isLocal)
      {
         Console.WriteLine("\n=== TEST {0} ===", isLocal ? "LOCAL" : "NETWORK");
         string path = SysDrive;
         if (!isLocal) path = Path.LocalToUnc(path);

         Console.WriteLine("\nInput Path: [{0}]", path);

         int cnt = 0;
         foreach (string path2 in Directory.EnumerateFileSystemEntries(path))
         {
            string uncPath = Path.LocalToUnc(path2);
            Console.WriteLine("\n\t#{0:000}\tPath: [{1}]\t\tLocalToUnc(): [{2}]", ++cnt, path2, uncPath);

            Assert.IsTrue(Path.IsUncPath(uncPath));

         }

         Assert.IsTrue(cnt > 0, "Nothing was enumerated.");
         Console.WriteLine();
      }

      #endregion // PathTest Helpers

      #region .NET

      #region ChangeExtension (.NET)

      [TestMethod]
      public void NET_ChangeExtension()
      {
         Console.WriteLine("Path.ChangeExtension()");
         Console.WriteLine("\nThe .NET method is used.");
      }

      #endregion // ChangeExtension (.NET)

      #region Combine

      [TestMethod]
      public void Combine()
      {
         Console.WriteLine("Path.Combine()");

         int pathCnt = 0;
         bool allOk = true;
         int errorCnt = 0;

         StopWatcher(true);
         foreach (string input in InputPaths)
         {
            foreach (string input2 in InputPaths)
            {
               try
               {
                  string actual = Path.Combine(input, input2);
                  string expected = System.IO.Path.Combine(input, input2);
                  Console.WriteLine("\n\t#{0:000}\tPath 1   : [{1}]\t\tPath 2: [{2}]\n\t\tAlphaFS  : [{3}]\n\t\tSystem.IO: [{4}]", ++pathCnt, input, input2, actual, expected);
                  Assert.AreEqual(expected, actual);
               }
               catch (Exception ex)
               {
                  Console.WriteLine("\n\t\tCaught Exception: [{0}]", ex.Message.Replace(Environment.NewLine, string.Empty));

                  // Exception to the Exception.
                  //if (input != null && !input.StartsWith(Path.GlobalRootPrefix, StringComparison.OrdinalIgnoreCase))
                  //{
                     allOk = false;
                     errorCnt++;
                  //}
               }
            }
         }
         Console.WriteLine("\n\t{0}\n", Reporter(true));

         Assert.AreEqual(true, allOk, "Encountered: [{0}] paths where AlphaFS != System.IO", errorCnt);
      }

      #endregion // Combine

      #region GetDirectoryName

      [TestMethod]
      public void GetDirectoryName()
      {
         Console.WriteLine("Path.GetDirectoryName()");

         int pathCnt = 0;
         bool allOk = true;
         int errorCnt = 0;

         StopWatcher(true);
         foreach (string input in InputPaths)
         {
            try
            {
               string actual = Path.GetDirectoryName(input);
               string expected = System.IO.Path.GetDirectoryName(input);
               Console.WriteLine("\n\t#{0:000}\tInput Path: [{1}]\n\t\tAlphaFS   : [{2}]\n\t\tSystem.IO : [{3}]", ++pathCnt, input, actual, expected);
               Assert.AreEqual(expected, actual);
            }
            catch (Exception ex)
            {
               Console.WriteLine("\n\t\tCaught Exception: [{0}]", ex.Message.Replace(Environment.NewLine, string.Empty));

               // Exception to the Exception.
               if (input != null && !input.StartsWith(Path.GlobalRootPrefix, StringComparison.OrdinalIgnoreCase))
               {
                  allOk = false;
                  errorCnt++;
               }
            }
         }
         Console.WriteLine("\n\t{0}\n", Reporter(true));

         Assert.AreEqual(true, allOk, "Encountered: [{0}] paths where AlphaFS != System.IO", errorCnt);
      }

      #endregion // GetDirectoryName

      #region GetExtension (.NET)

      [TestMethod]
      public void NET_GetExtension()
      {
         Console.WriteLine("Path.GetExtension()");
         Console.WriteLine("\nThe .NET method is used.\n");

         StopWatcher(true);
         foreach (string input in InputPaths)
         {
            string action = Path.GetExtension(input);
            Console.WriteLine("\tGetExtension: [{0}]\t\tInput Path: [{1}]", action, input);
            Assert.AreEqual(System.IO.Path.GetExtension(input), action);
         }
         Console.WriteLine("\n\t{0}\n", Reporter(true));
      }

      #endregion // GetExtension (.NET)

      #region GetFileName (.NET)

      [TestMethod]
      public void NET_GetFileName()
      {
         Console.WriteLine("Path.GetFileName()");
         Console.WriteLine("\nThe .NET method is used.\n");

         StopWatcher(true);
         foreach (string input in InputPaths)
         {
            string action = Path.GetFileName(input);
            Console.WriteLine("\tGetFileName: [{0}]\t\tInput Path: [{1}]", action, input);
            Assert.AreEqual(System.IO.Path.GetFileName(input), action);
         }
         Console.WriteLine("\n\t{0}\n", Reporter(true));
      }

      #endregion // GetFileName (.NET)

      #region GetFileNameWithoutExtension (.NET)

      [TestMethod]
      public void NET_GetFileNameWithoutExtension()
      {
         Console.WriteLine("Path.GetFileNameWithoutExtension()");
         Console.WriteLine("\nThe .NET method is used.\n");

         StopWatcher(true);
         foreach (string input in InputPaths)
         {
            string action = Path.GetFileNameWithoutExtension(input);
            Console.WriteLine("\tGetFileNameWithoutExtension: [{0}]\t\tInput Path: [{1}]", action, input);
            Assert.AreEqual(System.IO.Path.GetFileNameWithoutExtension(input), action);
         }
         Console.WriteLine("\n\t{0}\n", Reporter(true));
      }

      #endregion // GetFileNameWithoutExtension (.NET)

      #region GetFullPath

      [TestMethod]
      public void GetFullPath()
      {
         Console.WriteLine("Path.GetFullPath()");

         int pathCnt = 0;
         bool allOk = true;
         int errorCnt = 0;

         StopWatcher(true);
         foreach (string input in InputPaths)
         {
            try
            {
               string actual = Path.GetFullPath(input);
               string expected = System.IO.Path.GetFullPath(input);
               Console.WriteLine("\n\t#{0:000}\tInput Path: [{1}]\n\t\tAlphaFS   : [{2}]\n\t\tSystem.IO : [{3}]", ++pathCnt, input, actual, expected);
               Assert.AreEqual(expected, actual);
            }
            catch (ArgumentException) { }
            catch (Exception ex)
            {
               Console.WriteLine("\n\t\tCaught Exception: [{0}]", ex.Message.Replace(Environment.NewLine, string.Empty));
               allOk = false;
               errorCnt++;
            }
         }
         Console.WriteLine("\n\t{0}\n", Reporter(true));

         Assert.AreEqual(true, allOk, "Encountered: [{0}] paths where AlphaFS != System.IO", errorCnt);
      }

      #endregion // GetFullPath

      #region GetInvalidFileNameChars (.NET)

      [TestMethod]
      public void NET_GetInvalidFileNameChars()
      {
         Console.WriteLine("Path.GetInvalidFileNameChars()");
         Console.WriteLine("\nThe .NET method is used.\n");

         StopWatcher(true);
         foreach (char c in Path.GetInvalidFileNameChars())
            Console.WriteLine("\tChar: [{0}]", c);
         Console.WriteLine("\n\t{0}\n", Reporter(true));
      }

      #endregion // GetInvalidFileNameChars (.NET)

      #region GetInvalidPathChars (.NET)

      [TestMethod]
      public void NET_GetInvalidPathChars()
      {
         Console.WriteLine("Path.GetInvalidPathChars()");
         Console.WriteLine("\nThe .NET method is used.\n");

         StopWatcher(true);
         foreach (char c in Path.GetInvalidPathChars())
            Console.WriteLine("\tChar: [{0}]", c);
         Console.WriteLine("\n\t{0}\n", Reporter(true));
      }

      #endregion // GetInvalidPathChars (.NET)

      #region GetPathRoot

      [TestMethod]
      public void GetPathRoot()
      {
         Console.WriteLine("Path.GetPathRoot()");

         int pathCnt = 0;
         bool allOk = true;
         int errorCnt = 0;

         StopWatcher(true);
         foreach (string input in InputPaths)
         {
            try
            {
               string actual = Path.GetPathRoot(input);
               string expected = System.IO.Path.GetPathRoot(input);
               Console.WriteLine("\n\t#{0:000}\tInput Path: [{1}]\n\t\tAlphaFS   : [{2}]\n\t\tSystem.IO : [{3}]", ++pathCnt, input, actual, expected);
               Assert.AreEqual(expected, actual);
            }
            catch (ArgumentException) { }
            catch (Exception ex)
            {
               Console.WriteLine("\n\t\tCaught Exception: [{0}]", ex.Message.Replace(Environment.NewLine, string.Empty));
               allOk = false;
               errorCnt++;
            }
         }
         Console.WriteLine("\n\t{0}\n", Reporter(true));

         Assert.AreEqual(true, allOk, "Encountered: [{0}] paths where AlphaFS != System.IO", errorCnt);
      }

      #endregion // GetPathRoot

      #region GetRandomFileName (.NET)

      [TestMethod]
      public void NET_GetRandomFileName()
      {
         Console.WriteLine("Path.GetRandomFileName()");
         Console.WriteLine("\nThe .NET method is used.");
      }

      #endregion //GetRandomFileName (.NET)

      #region GetTempFileName (.NET)

      [TestMethod]
      public void NET_GetTempFileName()
      {
         Console.WriteLine("Path.GetTempFileName()");
         Console.WriteLine("\nThe .NET method is used.");
      }

      #endregion // GetTempFileName (.NET)

      #region GetTempPath (.NET)

      [TestMethod]
      public void NET_GetTempPath()
      {
         Console.WriteLine("Path.GetTempPath()");
         Console.WriteLine("\nThe .NET method is used.");
      }

      #endregion // GetTempPath (.NET)

      #region HasExtension (.NET)

      [TestMethod]
      public void NET_HasExtension()
      {
         Console.WriteLine("Path.HasExtension()");
         Console.WriteLine("\nThe .NET method is used.\n");

         StopWatcher(true);
         foreach (string input in InputPaths)
         {
            bool action = Path.HasExtension(input);
            Console.WriteLine("\tHasExtension: [{0}]\t\tInput Path: [{1}]", action, input);
            Assert.AreEqual(System.IO.Path.HasExtension(input), action);
         }
         Console.WriteLine("\n\t{0}\n", Reporter(true));
      }

      #endregion // HasExtension (.NET)

      #region IsPathRooted (.NET)

      [TestMethod]
      public void NET_IsPathRooted()
      {
         Console.WriteLine("Path.IsPathRooted()");
         Console.WriteLine("\nThe .NET method is used.\n");

         StopWatcher(true);
         foreach (string input in InputPaths)
         {
            bool action = Path.IsPathRooted(input);
            Console.WriteLine("\tIsPathRooted: [{0}]\t\tInput Path: [{1}]", action, input);
            Assert.AreEqual(System.IO.Path.IsPathRooted(input), action);
         }
         Console.WriteLine("\n\t{0}\n", Reporter(true));
      }

      #endregion // IsPathRooted (.NET)

      #endregion // .NET

      #region AlphaFS

      #region AddDirectorySeparator

      [TestMethod]
      public void AlphaFS_AddDirectorySeparator()
      {
         Console.WriteLine("Path.AddDirectorySeparator()\n");

         const string nonSlashedString = "SlashMe";
         Console.WriteLine("\tstring: [{0}];\n", nonSlashedString);

         // True, add DirectorySeparatorChar.
         string hasBackslash = Path.AddDirectorySeparator(nonSlashedString, false);
         bool addedBackslash = hasBackslash.EndsWith(Path.DirectorySeparatorChar.ToString()) && (nonSlashedString + Path.DirectorySeparatorChar).Equals(hasBackslash);
         Console.WriteLine("\tAddDirectorySeparator(string);\n\tAdded == [{0}]: {1}\n\tResult: [{2}]\n", TextTrue, addedBackslash, hasBackslash);

         // True, add AltDirectorySeparatorChar.
         string hasSlash = Path.AddDirectorySeparator(nonSlashedString, true);
         bool addedSlash = hasSlash.EndsWith(Path.AltDirectorySeparatorChar.ToString(CultureInfo.CurrentCulture)) && (nonSlashedString + Path.AltDirectorySeparatorChar).Equals(hasSlash);
         Console.WriteLine("\tAddDirectorySeparator(string, true);\n\tAdded == [{0}]: {1}\n\tResult: [{2}]\n", TextTrue, addedSlash, hasSlash);

         Assert.IsTrue(addedBackslash);
         Assert.IsTrue(addedSlash);
      }

      #endregion // AddDirectorySeparator

      #region GetDirectoryNameWithoutRoot

      [TestMethod]
      public void AlphaFS_GetDirectoryNameWithoutRoot()
      {
         Console.WriteLine("Path.GetDirectoryNameWithoutRoot()");

         // Note: since System.IO.Path does not have a similar method,
         // some more work is needs to test the validity of these result.

         DumpGetDirectoryNameWithoutRoot(true);
         DumpGetDirectoryNameWithoutRoot(false);
      }

      #endregion // GetDirectoryNameWithoutRoot

      #region GetFinalPathNameByHandle

      [TestMethod]
      public void AlphaFS_GetFinalPathNameByHandle()
      {
         Console.WriteLine("Path.GetFinalPathNameByHandle()");

         DumpGetFinalPathNameByHandle(true);
         //DumpGetFinalPathNameByHandle(false, false);
      }

      #endregion // GetFinalPathNameByHandle

      #region GetSuffixedDirectoryName

      [TestMethod]
      public void AlphaFS_GetSuffixedDirectoryName()
      {
         Console.WriteLine("Path.GetSuffixedDirectoryName()");

         // Note: since System.IO.Path does not have a similar method,
         // some more work is needs to test the validity of these result.

         DumpGetSuffixedDirectoryName(true);
         DumpGetSuffixedDirectoryName(false);
      }

      #endregion // GetSuffixedDirectoryName

      #region GetSuffixedDirectoryNameWithoutRoot

      [TestMethod]
      public void AlphaFS_GetSuffixedDirectoryNameWithoutRoot()
      {
         Console.WriteLine("Path.GetSuffixedDirectoryNameWithoutRoot()");

         // Note: since System.IO.Path does not have a similar method,
         // some more work is needed to test the validity of these result.

         DumpGetSuffixedDirectoryNameWithoutRoot(true);
         DumpGetSuffixedDirectoryNameWithoutRoot(false);
      }

      #endregion // GetSuffixedDirectoryNameWithoutRoot

      #region GetLongPath

      [TestMethod]
      public void AlphaFS_GetLongPath()
      {
         Console.WriteLine("Path.GetLongPath()");
         Console.WriteLine("\n\tDirectory: [{0}]", SysRoot);

         string folderName = Path.GetFileName(SysRoot, true);
         string longPath = Path.GetLongPath(SysRoot);
         Console.WriteLine("\n\tGetLongPath(): [{0}]", longPath);
         Assert.IsTrue(longPath.StartsWith(Path.LongPathPrefix));
         Assert.IsTrue(longPath.EndsWith(SysRoot));
         Assert.IsTrue(Directory.Exists(longPath));

         string longPathUnc = Path.LocalToUnc(SysRoot);
         longPathUnc = Path.GetLongPath(longPathUnc);
         if (Directory.Exists(longPathUnc))
         {
            Console.WriteLine("\n\tGetLongPath() UNC: [{0}]", longPathUnc);
            Assert.IsTrue(longPathUnc.StartsWith(Path.LongPathUncPrefix));
            Assert.IsTrue(longPathUnc.EndsWith(folderName));
            Assert.IsTrue(Directory.Exists(longPathUnc));
         }
         else
            Assert.IsTrue(false, "Share inaccessible: {0}", longPathUnc);
      }

      #endregion // GetLongPath

      #region GetLongFrom83Path

      [TestMethod]
      public void AlphaFS_GetLongFrom83Path()
      {
         Console.WriteLine("Path.GetLongFrom83Path()");

         Dump83Path(StartupFolder);
         Dump83Path(Path.LocalToUnc(StartupFolder));
      }

      #endregion // GetLongFrom83Path

      #region GetMappedConnectionName

      [TestMethod]
      public void AlphaFS_GetMappedConnectionName()
      {
         Console.WriteLine("Path.GetMappedConnectionName()");

         DumpGetRemoteNameInfo();
      }

      #endregion // GetMappedConnectionName

      #region GetMappedUncName

      [TestMethod]
      public void AlphaFS_GetMappedUncName()
      {
         Console.WriteLine("Path.GetMappedUncName()");

         DumpGetRemoteNameInfo();
      }

      #endregion // GetMappedUncName
      
      #region GetRegularPath

      [TestMethod]
      public void AlphaFS_GetRegularPath()
      {
         Console.WriteLine("Path.GetRegularPath()");

         int pathCnt = 0;
         bool allOk = true;
         int errorCnt = 0;

         StopWatcher(true);
         foreach (string input in InputPaths)
         {
            try
            {
               string actual = Path.GetRegularPath(input);
               string expected = InputPaths[pathCnt];

               if (Path.IsLongPath(expected))
                  Assert.IsFalse(Path.IsLongPath(actual), "Path should be regular.");

               Console.WriteLine("\n\t#{0:000}\tInput Path: [{1}]\n\t\tAlphaFS   : [{2}]", ++pathCnt, input, actual);
               //Assert.AreEqual(expected, actual);
            }
            catch (Exception ex)
            {
               Console.WriteLine("\n\t\tCaught Exception: [{0}]", ex.Message.Replace(Environment.NewLine, string.Empty));

               // Exception to the Exception.
               if (input != null &&
                   (!input.StartsWith(Path.VolumePrefix, StringComparison.OrdinalIgnoreCase) &&
                    !input.StartsWith(Path.GlobalRootPrefix, StringComparison.OrdinalIgnoreCase)))
               {
                  allOk = false;
                  errorCnt++;
               }
            }
         }
         Console.WriteLine("\n\t{0}\n", Reporter(true));

         Assert.AreEqual(true, allOk, "Encountered: [{0}] paths where AlphaFS != System.IO", errorCnt);
      }

      #endregion // GetRegularPath

      #region GetShort83Path

      [TestMethod]
      public void AlphaFS_GetShort83Path()
      {
         Console.WriteLine("Path.GetShort83Path()");

         AlphaFS_GetLongFrom83Path();
      }

      #endregion // GetShort83Path
      
      #region IsLocalPath

      [TestMethod]
      public void AlphaFS_IsLocalPath()
      {
         Console.WriteLine("Path.IsLocalPath()");

         DumpIsLocalPath(true);
      }

      #endregion // IsLocalPath

      #region IsLongPath

      [TestMethod]
      public void AlphaFS_IsLongPath()
      {
         Console.WriteLine("Path.IsLongPath()");

         DumpIsLongPath(true);
         DumpIsLongPath(false);
      }

      #endregion // IsLongPath

      #region IsUncPath

      [TestMethod]
      public void AlphaFS_IsUncPath()
      {
         Console.WriteLine("Path.IsUncPath()");

         DumpIsUncPath(true);
      }

      #endregion // IsUncPath

      #region LocalToUnc

      [TestMethod]
      public void AlphaFS_LocalToUnc()
      {
         Console.WriteLine("Path.LocalToUnc()");

         DumpLocalToUnc(true);
         DumpLocalToUnc(false);
      }

      #endregion // LocalToUnc

      #region RemoveDirectorySeparator

      [TestMethod]
      public void AlphaFS_RemoveDirectorySeparator()
      {
         Console.WriteLine("Path.RemoveDirectorySeparator()\n");

         const string backslashedString = @"Backslashed\";
         const string slashedString = "Slashed/";
         // True, add DirectorySeparatorChar.
         string hasBackslash = Path.RemoveDirectorySeparator(backslashedString);
         bool removedBackslash = !hasBackslash.EndsWith(Path.DirectorySeparatorChar.ToString(CultureInfo.CurrentCulture)) && !backslashedString.Equals(hasBackslash);
         Console.WriteLine("\tstring = @[{0}];\n", backslashedString);
         Console.WriteLine("\tRemoveDirectorySeparator(string);\n\tRemoved == [{0}]: {1}\n\tResult: [{2}]\n", TextTrue, removedBackslash, hasBackslash);

         // True, add AltDirectorySeparatorChar.
         string hasSlash = Path.RemoveDirectorySeparator(slashedString, true);
         bool removedSlash = !hasSlash.EndsWith(Path.AltDirectorySeparatorChar.ToString(CultureInfo.CurrentCulture)) && !slashedString.Equals(hasSlash);
         Console.WriteLine("\tstring: [{0}];\n", slashedString);
         Console.WriteLine("\tRemoveDirectorySeparator(string, true);\n\tRemoved == [{0}]: {1}\n\tResult: [{2}]\n", TextTrue, removedSlash, hasSlash);

         Assert.IsTrue(removedBackslash);
         Assert.IsTrue(removedSlash);
      }

      #endregion // RemoveDirectorySeparator

      #endregion // AlphaFS
   }
}