/* Copyright 2008-2015 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
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
         var lastError = new Win32Exception();

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

         //"C:\\ ", "Hello World!",  // Path.Combine() result: "C:\ \Hello World!"

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

      private static void Dump83Path(string fullPath)
      {
         Console.WriteLine("\nInput Path: [{0}]", fullPath);

         if (Directory.Exists(fullPath))
         {
            // GetShort83Path()
            StopWatcher(true);
            string short83Path = Path.GetShort83Path(fullPath);
            string reporter = Reporter();
            bool isShort83Path = !string.IsNullOrWhiteSpace(short83Path) && !short83Path.Equals(fullPath) && Directory.Exists(short83Path);
            bool hasTilde = !string.IsNullOrWhiteSpace(short83Path) && short83Path.IndexOf('~') >= 0;

            Console.WriteLine("\t{0, 20} == [{1}]: [{2}]: [{3}]\n\t{4}", "GetShort83Path()", TextTrue, isShort83Path, short83Path, reporter);
            Assert.IsTrue(isShort83Path);
            Assert.IsTrue(hasTilde); // A bit tricky if fullPath is already a shortPath.

            // GetLongFrom83ShortPath()
            StopWatcher(true);
            string longFrom83Path = Path.GetLongFrom83ShortPath(short83Path);
            reporter = Reporter();
            bool isLongFrom83Path = !string.IsNullOrWhiteSpace(longFrom83Path) && !longFrom83Path.Equals(short83Path) && Directory.Exists(longFrom83Path);
            bool noTilde = !string.IsNullOrWhiteSpace(longFrom83Path) && longFrom83Path.IndexOf('~') == -1;

            Console.WriteLine("\n\t{0, 20} == [{1}]: [{2}]: [{3}]\n\t{4}\n", "GetLongFrom83ShortPath()", TextTrue, isLongFrom83Path, longFrom83Path, reporter);
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
         Assert.AreEqual(null, directoryNameWithoutRoot);

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
            Console.WriteLine("\tFilestream.Length: [{0}] (Note: For Windows versions < Vista, the file must be > 0 bytes.)\n", Utils.UnitSizeToText(stream.Length));

            Console.WriteLine("\tFinalPathFormats.None          : [{0}]", fileNameNormalized);
            Console.WriteLine("\tFinalPathFormats.FileNameOpened: [{0}]", fileNameOpened);
            Console.WriteLine("\tFinalPathFormats.VolumeNameDos : [{0}]", volumeNameDos);
            Console.WriteLine("\tFinalPathFormats.VolumeNameGuid: [{0}]", volumeNameGuid);
            Console.WriteLine("\tFinalPathFormats.VolumeNameNT  : [{0}]", volumeNameNt);
            Console.WriteLine("\tFinalPathFormats.VolumeNameNone: [{0}]", volumeNameNone);

            Console.WriteLine("\n\t{0}", Reporter(true));

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
         Assert.AreEqual(null, suffixedDirectoryName);


         fullPath = SysDrive + Path.DirectorySeparator;
         if (!isLocal) fullPath = Path.LocalToUnc(fullPath);

         StopWatcher(true);
         suffixedDirectoryName = Path.GetSuffixedDirectoryNameWithoutRoot(fullPath);
         Console.WriteLine("\nInput Path: [{0}]\n\tGetSuffixedDirectoryName() (Should be: [null]): [{1}]\n{2}", fullPath, suffixedDirectoryName ?? "null", Reporter(false, true));
         Assert.AreEqual(null, suffixedDirectoryName);

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
         Assert.AreEqual(null, suffixedDirectoryNameWithoutRoot, "Path mismatch.");



         fullPath = SysDrive + Path.DirectorySeparator;
         if (!isLocal) fullPath = Path.LocalToUnc(fullPath);

         StopWatcher(true);
         suffixedDirectoryNameWithoutRoot = Path.GetSuffixedDirectoryNameWithoutRoot(fullPath);
         Console.WriteLine("\nInput Path: [{0}]\n\tGetSuffixedDirectoryName() (Should be: [null]): [{1}]\n{2}", fullPath, suffixedDirectoryNameWithoutRoot ?? "null", Reporter(false, true));
         Assert.AreEqual(null, suffixedDirectoryNameWithoutRoot, "Path mismatch.");

         Console.WriteLine("\n");
      }

      private void DumpLocalToUnc(bool isLocal)
      {
         Console.WriteLine("\n=== TEST {0} ===", isLocal ? "LOCAL" : "NETWORK");
         string path = SysRoot;
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
         foreach (string path in InputPaths)
         {
            try
            {
               foreach (string path2 in InputPaths)
               {
                  string method = null;

                  try
                  {
                     method = "AlphaFS";
                     string actual = Path.Combine(path, path2);

                     method = "System.IO";
                     string expected = System.IO.Path.Combine(path, path2);

                     Console.WriteLine("\n\t#{0:000}\tPath 1   : [{1}]\t\tPath 2: [{2}]\n\t\tAlphaFS  : [{3}]\n\t\tSystem.IO: [{4}]", ++pathCnt, path, path2, actual, expected);
                     Assert.AreEqual(expected, actual);
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
            }
            catch (Exception ex)
            {
               Console.WriteLine("\n\t\t(Outer) Caught Exception: [{0}] Path: [{1}]", ex.Message.Replace(Environment.NewLine, "  "), path);
            }
         }
         Console.WriteLine("\n\t{0}", Reporter(true));

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
         foreach (string path in InputPaths)
         {
            string method = null;

            try
            {
               method = "AlphaFS";
               string actual = Path.GetDirectoryName(path);

               method = "System.IO";
               string expected = System.IO.Path.GetDirectoryName(path);

               Console.WriteLine("\n\t#{0:000}\tInput Path: [{1}]\n\t\tAlphaFS   : [{2}]\n\t\tSystem.IO : [{3}]", ++pathCnt, path, actual, expected);
               Assert.AreEqual(expected, actual);
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

      #endregion // GetDirectoryName

      #region GetExtension

      [TestMethod]
      public void GetExtension()
      {
         Console.WriteLine("Path.GetExtension()");

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
               string actual = Path.GetExtension(path);

               method = "System.IO";
               string expected = System.IO.Path.GetExtension(path);

               Console.WriteLine("\n\t#{0:000}\tInput Path: [{1}]\n\t\tAlphaFS   : [{2}]\n\t\tSystem.IO : [{3}]", ++pathCnt, path, actual, expected);
               Assert.AreEqual(expected, actual);
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

      #endregion // GetExtension

      #region GetFileName

      [TestMethod]
      public void GetFileName()
      {
         Console.WriteLine("Path.GetFileName()");

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
               string actual = Path.GetFileName(path);

               method = "System.IO";
               string expected = System.IO.Path.GetFileName(path);

               Console.WriteLine("\n\t#{0:000}\tInput Path: [{1}]\n\t\tAlphaFS   : [{2}]\n\t\tSystem.IO : [{3}]", ++pathCnt, path, actual, expected);
               Assert.AreEqual(expected, actual);
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

      #endregion // GetFileName

      #region GetFileNameWithoutExtension

      [TestMethod]
      public void GetFileNameWithoutExtension()
      {
         Console.WriteLine("Path.GetFileNameWithoutExtension()");

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
               string actual = Path.GetFileNameWithoutExtension(path);

               method = "System.IO";
               string expected = System.IO.Path.GetFileNameWithoutExtension(path);

               Console.WriteLine("\n\t#{0:000}\tInput Path: [{1}]\n\t\tAlphaFS   : [{2}]\n\t\tSystem.IO : [{3}]", ++pathCnt, path, actual, expected);
               Assert.AreEqual(expected, actual);
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

      #endregion // GetFileNameWithoutExtension

      #region GetFullPath

      [TestMethod]
      public void GetFullPath()
      {
         Console.WriteLine("Path.GetFullPath()");

         int pathCnt = 0;
         bool allOk = true;
         int errorCnt = 0;

         try
         {
            Path.GetFullPath(SysDrive + @"\?test.txt");
            //Path.GetFullPath(SysDrive + @"\*test.txt");
            //Path.GetFullPath(SysDrive + @"\\test.txt");
            //Path.GetFullPath(SysDrive + @"\/test.txt");
         }
         catch (ArgumentException)
         {
            allOk = true;
         }
         Assert.IsTrue(allOk, "ArgumentException should have been caught.");



         allOk = false;
         try
         {
            Path.GetFullPath(SysDrive + @"\dev\test\aaa:aaa.txt");
         }
         catch (NotSupportedException)
         {
            allOk = true;
         }
         Assert.IsTrue(allOk, "NotSupportedException should have been caught.");


         allOk = false;
         try
         {
            Path.GetFullPath(@"\\\\.txt");
         }
         catch (ArgumentException)
         {
            allOk = true;
         }
         Assert.IsTrue(allOk, "ArgumentException should have been caught.");



         StopWatcher(true);
         foreach (string path in InputPaths)
         {
            string method = null;

            try
            {
               method = "AlphaFS";
               string actual = Path.GetFullPath(path);

               method = "System.IO";
               string expected = System.IO.Path.GetFullPath(path);

               Console.WriteLine("\n\t#{0:000}\tInput Path: [{1}]\n\t\tAlphaFS   : [{2}]\n\t\tSystem.IO : [{3}]", ++pathCnt, path, actual, expected);
               Assert.AreEqual(expected, actual);
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
         Console.WriteLine("\n\t{0}", Reporter(true));
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
         Console.WriteLine("\n\t{0}", Reporter(true));
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


         string root = Path.GetPathRoot(null);
         Assert.AreEqual(null, root, "root should be null.");


         bool success = true;
         try
         {
            root = Path.GetPathRoot("");
         }
         catch (Exception)
         {
            success = false;
         }
         Assert.IsFalse(success, "Success should be false because of empty path string.");


         StopWatcher(true);
         foreach (string path in InputPaths)
         {
            string method = null;

            try
            {
               method = "AlphaFS";
               string actual = Path.GetPathRoot(path);

               method = "System.IO";
               string expected = System.IO.Path.GetPathRoot(path);

               Console.WriteLine("\n\t#{0:000}\tInput Path: [{1}]\n\t\tAlphaFS   : [{2}]\n\t\tSystem.IO : [{3}]", ++pathCnt, path, actual, expected);
               Assert.AreEqual(expected, actual);
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
         foreach (string path in InputPaths)
         {
            bool action = Path.HasExtension(path);
            Console.WriteLine("\tHasExtension: [{0}]\t\tInput Path: [{1}]", action, path);
            Assert.AreEqual(System.IO.Path.HasExtension(path), action);
         }
         Console.WriteLine("\n\t{0}", Reporter(true));
      }

      #endregion // HasExtension (.NET)

      #region IsPathRooted (.NET)

      [TestMethod]
      public void NET_IsPathRooted()
      {
         Console.WriteLine("Path.IsPathRooted()");
         Console.WriteLine("\nThe .NET method is used.\n");

         StopWatcher(true);
         foreach (string path in InputPaths)
         {
            bool action = Path.IsPathRooted(path);
            Console.WriteLine("\tIsPathRooted: [{0}]\t\tInput Path: [{1}]", action, path);
            Assert.AreEqual(System.IO.Path.IsPathRooted(path), action);
         }
         Console.WriteLine("\n\t{0}", Reporter(true));
      }

      #endregion // IsPathRooted (.NET)

      #endregion // .NET

      #region AlphaFS

      #region AddTrailingDirectorySeparator

      [TestMethod]
      public void AlphaFS_AddTrailingDirectorySeparator()
      {
         Console.WriteLine("Path.AddTrailingDirectorySeparator()\n");

         const string nonSlashedString = "SlashMe";
         Console.WriteLine("\tstring: [{0}];\n", nonSlashedString);

         // True, add DirectorySeparatorChar.
         string hasBackslash = Path.AddTrailingDirectorySeparator(nonSlashedString, false);
         bool addedBackslash = hasBackslash.EndsWith(Path.DirectorySeparatorChar.ToString()) && (nonSlashedString + Path.DirectorySeparatorChar).Equals(hasBackslash);
         Console.WriteLine("\tAddTrailingDirectorySeparator(string);\n\tAdded == [{0}]: {1}\n\tResult: [{2}]\n", TextTrue, addedBackslash, hasBackslash);

         // True, add AltDirectorySeparatorChar.
         string hasSlash = Path.AddTrailingDirectorySeparator(nonSlashedString, true);
         bool addedSlash = hasSlash.EndsWith(Path.AltDirectorySeparatorChar.ToString(CultureInfo.CurrentCulture)) && (nonSlashedString + Path.AltDirectorySeparatorChar).Equals(hasSlash);
         Console.WriteLine("\tAddTrailingDirectorySeparator(string, true);\n\tAdded == [{0}]: {1}\n\tResult: [{2}]\n", TextTrue, addedSlash, hasSlash);

         Assert.IsTrue(addedBackslash);
         Assert.IsTrue(addedSlash);
      }

      #endregion // AddTrailingDirectorySeparator

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

      #region GetLongFrom83ShortPath

      [TestMethod]
      public void AlphaFS_GetLongFrom83Path()
      {
         Console.WriteLine("Path.GetLongFrom83ShortPath()");

         Dump83Path(StartupFolder);
         Dump83Path(Path.LocalToUnc(StartupFolder));
      }

      #endregion // GetLongFrom83ShortPath

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
         foreach (string path in InputPaths)
         {
            string method = null;

            try
            {
               method = "AlphaFS";
               string actual = Path.GetRegularPath(path);

               method = "System.IO";
               string expected = InputPaths[pathCnt];

               if (Path.IsLongPath(expected))
                  Assert.IsFalse(Path.IsLongPath(actual), "Path should be regular.");

               Console.WriteLine("\n\t#{0:000}\tInput Path: [{1}]\n\t\tAlphaFS   : [{2}]", ++pathCnt, path, actual);

               if (expected.StartsWith(Path.LongPathUncPrefix, StringComparison.OrdinalIgnoreCase))
                  expected = expected.Replace(Path.LongPathUncPrefix, Path.UncPrefix);

               expected = expected.Replace(Path.LongPathPrefix, "");

               Assert.AreEqual(expected, actual);
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
         Console.WriteLine("Path.IsLocalPath()\n");

         int isLocalPath = 0;
         bool allOk = true;
         int errorCnt = 0;
         int cnt = 0;

         StopWatcher(true);
         foreach (string path in InputPaths)
         {
            string method = null;

            try
            {
               method = "AlphaFS";
               bool actual = Path.IsLocalPath(path);

               Console.WriteLine("\t#{0:000}\tIsLocalPath(): [{1}]    \tInput Path: [{2}]", ++cnt, actual, path);

               if (actual)
                  isLocalPath++;
            }
            catch (Exception ex)
            {
               Console.WriteLine("\tCaught Exception: Method: [{0}] [{1}]", method, ex.Message.Replace(Environment.NewLine, "  "));
               allOk = false;
               errorCnt++;
            }
         }
         Console.WriteLine("\n\t{0}", Reporter(true));

         // Hand counted 26 True's.
         Assert.AreEqual(26, isLocalPath, "Numbers of matching local paths do not match.", errorCnt);

         Assert.AreEqual(true, allOk, "Encountered: [{0}] paths where AlphaFS != System.IO", errorCnt);
      }

      #endregion // IsLocalPath

      #region IsLongPath

      [TestMethod]
      public void AlphaFS_IsLongPath()
      {
         Console.WriteLine("Path.IsLongPath()\n");

         int isLongPath = 0;
         bool allOk = true;
         int errorCnt = 0;
         int cnt = 0;

         StopWatcher(true);
         foreach (string path in InputPaths)
         {
            string method = null;

            try
            {
               method = "AlphaFS";
               bool actual = Path.IsLongPath(path);

               bool expected = path.StartsWith(Path.LongPathPrefix, StringComparison.OrdinalIgnoreCase);

               Console.WriteLine("\t#{0:000}\tIsLocalPath(): [{1}]    \tInput Path: [{2}]", ++cnt, actual, path);
               Assert.AreEqual(expected, actual);

               if (actual)
                  isLongPath++;
            }
            catch (Exception ex)
            {
               Console.WriteLine("\tCaught Exception: Method: [{0}] [{1}]", method, ex.Message.Replace(Environment.NewLine, "  "));
               allOk = false;
               errorCnt++;
            }
         }
         Console.WriteLine("\n\t{0}", Reporter(true));

         // Hand counted 32 True's.
         Assert.AreEqual(32, isLongPath, "Numbers of matching local paths do not match.", errorCnt);

         Assert.AreEqual(true, allOk, "Encountered: [{0}] paths where AlphaFS != System.IO", errorCnt);
      }

      #endregion // IsLongPath

      #region IsUncPath

      [TestMethod]
      public void AlphaFS_IsUncPath()
      {
         Console.WriteLine("Path.IsUncPath()");

         int isUncPath = 0;
         int isNotUncPath = 0;
         bool allOk = true;
         int errorCnt = 0;
         int cnt = 0;

         StopWatcher(true);
         foreach (string path in InputPaths)
         {
            string method = null;

            try
            {
               method = "AlphaFS";
               bool actual = Path.IsUncPath(path);

               Console.WriteLine("\t#{0:000}\tIsUncPath(): [{1}]\t\tInput Path: [{2}]", ++cnt, actual, path);

               if (actual)
                  isUncPath++;
               else
                  isNotUncPath++;
            }
            catch (Exception ex)
            {
               Console.WriteLine("\tCaught Exception: Method: [{0}] [{1}]", method, ex.Message.Replace(Environment.NewLine, "  "));
               allOk = false;
               errorCnt++;
            }
         }
         Console.WriteLine("\n\t{0}", Reporter(true));

         // Hand counted 30 True's.
         Assert.AreEqual(30, isUncPath, "Numbers of matching UNC paths do not match.", errorCnt);
         Assert.AreEqual(36, isNotUncPath, "Numbers of matching UNC paths do not match.", errorCnt);

         Assert.AreEqual(true, allOk, "Encountered: [{0}] paths where AlphaFS != System.IO", errorCnt);
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

      #region RemoveTrailingDirectorySeparator

      [TestMethod]
      public void AlphaFS_RemoveTrailingDirectorySeparator()
      {
         Console.WriteLine("Path.RemoveTrailingDirectorySeparator()\n");

         const string backslashedString = @"Backslashed\";
         const string slashedString = "Slashed/";
         // True, add DirectorySeparatorChar.
         string hasBackslash = Path.RemoveTrailingDirectorySeparator(backslashedString);
         bool removedBackslash = !hasBackslash.EndsWith(Path.DirectorySeparatorChar.ToString(CultureInfo.CurrentCulture)) && !backslashedString.Equals(hasBackslash);
         Console.WriteLine("\tstring = @[{0}];\n", backslashedString);
         Console.WriteLine("\tRemoveTrailingDirectorySeparator(string);\n\tRemoved == [{0}]: {1}\n\tResult: [{2}]\n", TextTrue, removedBackslash, hasBackslash);

         // True, add AltDirectorySeparatorChar.
         string hasSlash = Path.RemoveTrailingDirectorySeparator(slashedString, true);
         bool removedSlash = !hasSlash.EndsWith(Path.AltDirectorySeparatorChar.ToString(CultureInfo.CurrentCulture)) && !slashedString.Equals(hasSlash);
         Console.WriteLine("\tstring: [{0}];\n", slashedString);
         Console.WriteLine("\tRemoveTrailingDirectorySeparator(string, true);\n\tRemoved == [{0}]: {1}\n\tResult: [{2}]\n", TextTrue, removedSlash, hasSlash);

         Assert.IsTrue(removedBackslash);
         Assert.IsTrue(removedSlash);
      }

      #endregion // RemoveTrailingDirectorySeparator

      #endregion // AlphaFS
   }
}