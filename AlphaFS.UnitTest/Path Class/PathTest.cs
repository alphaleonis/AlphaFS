/*  Copyright (C) 2008-2017 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
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
using System.Globalization;
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
   public partial class PathTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>

      #region Unit Tests

      private void Dump83Path(bool isLocal)
      {
         #region Setup

         Console.WriteLine("\n=== TEST {0} ===", isLocal ? UnitTestConstants.Local : UnitTestConstants.Network);

         var myLongPath = Path.GetTempPath("My Long Data File Or Directory");
         if (!isLocal) myLongPath = Path.LocalToUnc(myLongPath);

         Console.WriteLine("\nInput Path: [{0}]\n", myLongPath);

         #endregion // Setup

         #region File

         string short83Path;

         try
         {
            using (File.Create(myLongPath))

            UnitTestConstants.StopWatcher(true);

            short83Path = Path.GetShort83Path(myLongPath);

            Console.WriteLine("Short 8.3 file path    : [{0}]\t\t\t{1}", short83Path, UnitTestConstants.Reporter(true));

            Assert.IsTrue(!short83Path.Equals(myLongPath));

            Assert.IsTrue(short83Path.EndsWith(@"~1"));



            UnitTestConstants.StopWatcher(true);

            var longFrom83Path = Path.GetLongFrom83ShortPath(short83Path);

            Console.WriteLine("Long path from 8.3 path: [{0}]{1}", longFrom83Path, UnitTestConstants.Reporter(true));

            Assert.IsTrue(longFrom83Path.Equals(myLongPath));

            Assert.IsFalse(longFrom83Path.EndsWith(@"~1"));

         }
         catch (Exception ex)
         {
            Console.WriteLine("Caught (UNEXPECTED) {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
         }
         finally
         {
            if (File.Exists(myLongPath))
               File.Delete(myLongPath);
         }
         Console.WriteLine();

         #endregion // File

         #region Directory

         try
         {
            Directory.CreateDirectory(myLongPath);

            UnitTestConstants.StopWatcher(true);

            short83Path = Path.GetShort83Path(myLongPath);

            Console.WriteLine("Short 8.3 directory path: [{0}]\t\t\t{1}", short83Path, UnitTestConstants.Reporter(true));

            Assert.IsFalse(short83Path.Equals(myLongPath));

            Assert.IsTrue(short83Path.EndsWith(@"~1"));



            UnitTestConstants.StopWatcher(true);

            var longFrom83Path = Path.GetLongFrom83ShortPath(short83Path);

            Console.WriteLine("Long path from 8.3 path : [{0}]{1}", longFrom83Path, UnitTestConstants.Reporter(true));

            Assert.IsTrue(longFrom83Path.Equals(myLongPath));

            Assert.IsFalse(longFrom83Path.EndsWith(@"~1"));
         }
         catch (Exception ex)
         {
            Console.WriteLine("Caught (UNEXPECTED) {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
         }
         finally
         {
            if (Directory.Exists(myLongPath))
               Directory.Delete(myLongPath);
         }
         Console.WriteLine();

         #endregion // Directory
      }

      private void DumpGetDirectoryNameWithoutRoot(bool isLocal)
      {
         Console.WriteLine("\n=== TEST {0} ===", isLocal ? UnitTestConstants.Local : UnitTestConstants.Network);

         const string neDir = "Non-Existing Directory";
         const string sys32 = "system32";

         var fullPath = Path.Combine(Environment.SystemDirectory, neDir);
         if (!isLocal) fullPath = Path.LocalToUnc(fullPath);

         UnitTestConstants.StopWatcher(true);
         var directoryNameWithoutRoot = Path.GetDirectoryNameWithoutRoot(fullPath);
         Console.WriteLine("\nInput Path: [{0}]\n\tGetDirectoryNameWithoutRoot() (Should be: [{1}]): [{2}]", fullPath, sys32, directoryNameWithoutRoot);
         Assert.IsTrue(directoryNameWithoutRoot.Equals(sys32, StringComparison.OrdinalIgnoreCase));



         fullPath = Path.Combine(fullPath, "Non-Existing file.txt");
         if (!isLocal) fullPath = Path.LocalToUnc(fullPath);

         UnitTestConstants.StopWatcher(true);
         directoryNameWithoutRoot = Path.GetDirectoryNameWithoutRoot(fullPath);
         Console.WriteLine("\nInput Path: [{0}]\n\tGetDirectoryNameWithoutRoot() (Should be: [{1}]): [{2}]", fullPath, neDir, directoryNameWithoutRoot);
         Assert.IsTrue(directoryNameWithoutRoot.Equals(neDir, StringComparison.OrdinalIgnoreCase));



         fullPath = UnitTestConstants.SysRoot;
         if (!isLocal) fullPath = Path.LocalToUnc(fullPath);

         UnitTestConstants.StopWatcher(true);
         directoryNameWithoutRoot = Path.GetDirectoryNameWithoutRoot(fullPath);
         Console.WriteLine("\nInput Path: [{0}]\n\tGetDirectoryNameWithoutRoot() (Should be: [null]): [{1}]", fullPath, directoryNameWithoutRoot ?? "null");
         Assert.AreEqual(null, directoryNameWithoutRoot);

         Console.WriteLine("\n");
      }

      private void DumpGetFinalPathNameByHandle(bool isLocal)
      {
         Console.WriteLine("\n=== TEST {0} ===", isLocal ? UnitTestConstants.Local : UnitTestConstants.Network);

         var tempFile = Path.GetTempFileName();
         if (!isLocal) tempFile = Path.LocalToUnc(tempFile);

         var longTempStream = Path.LongPathPrefix + tempFile;
         bool gotFileNameNormalized;
         bool gotFileNameOpened;
         bool gotVolumeNameDos;
         bool gotVolumeNameGuid;
         bool gotVolumeNameNt;
         bool gotVolumeNameNone;
         bool gotSomething;

         using (var stream = File.Create(tempFile))
         {
            // For Windows versions < Vista, the file must be > 0 bytes.
            if (!NativeMethods.IsAtLeastWindowsVista)
               stream.WriteByte(1);

            var handle = stream.SafeFileHandle;

            UnitTestConstants.StopWatcher(true);
            var fileNameNormalized = Path.GetFinalPathNameByHandle(handle);
            var fileNameOpened = Path.GetFinalPathNameByHandle(handle, FinalPathFormats.FileNameOpened);

            var volumeNameDos = Path.GetFinalPathNameByHandle(handle, FinalPathFormats.None);
            var volumeNameGuid = Path.GetFinalPathNameByHandle(handle, FinalPathFormats.VolumeNameGuid);
            var volumeNameNt = Path.GetFinalPathNameByHandle(handle, FinalPathFormats.VolumeNameNT);
            var volumeNameNone = Path.GetFinalPathNameByHandle(handle, FinalPathFormats.VolumeNameNone);

            // These three output the same.
            gotFileNameNormalized = !Utils.IsNullOrWhiteSpace(fileNameNormalized) && longTempStream.Equals(fileNameNormalized);
            gotFileNameOpened = !Utils.IsNullOrWhiteSpace(fileNameOpened) && longTempStream.Equals(fileNameOpened);
            gotVolumeNameDos = !Utils.IsNullOrWhiteSpace(volumeNameDos) && longTempStream.Equals(volumeNameDos);

            gotVolumeNameGuid = !Utils.IsNullOrWhiteSpace(volumeNameGuid) && volumeNameGuid.StartsWith(Path.VolumePrefix) && volumeNameGuid.EndsWith(volumeNameNone);
            gotVolumeNameNt = !Utils.IsNullOrWhiteSpace(volumeNameNt) && volumeNameNt.StartsWith(Path.DevicePrefix);
            gotVolumeNameNone = !Utils.IsNullOrWhiteSpace(volumeNameNone) && tempFile.EndsWith(volumeNameNone);

            Console.WriteLine("\nInput Path: [{0}]", tempFile);
            Console.WriteLine("\n\tFilestream.Name  : [{0}]", stream.Name);
            Console.WriteLine("\tFilestream.Length: [{0}] (Note: For Windows versions < Vista, the file must be > 0 bytes.)\n", Utils.UnitSizeToText(stream.Length));

            Console.WriteLine("\tFinalPathFormats.None          : [{0}]", fileNameNormalized);
            Console.WriteLine("\tFinalPathFormats.FileNameOpened: [{0}]", fileNameOpened);
            Console.WriteLine("\tFinalPathFormats.VolumeNameDos : [{0}]", volumeNameDos);
            Console.WriteLine("\tFinalPathFormats.VolumeNameGuid: [{0}]", volumeNameGuid);
            Console.WriteLine("\tFinalPathFormats.VolumeNameNT  : [{0}]", volumeNameNt);
            Console.WriteLine("\tFinalPathFormats.VolumeNameNone: [{0}]", volumeNameNone);

            Console.WriteLine("\n{0}", UnitTestConstants.Reporter(true));

            gotSomething = true;
         }


         var fileExists = File.Exists(tempFile);

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



         // AlphaFS implementation of fileStream.Name returns = "[Unknown]"
         // System.IO returns the full path.
         Console.WriteLine();
         var fileName = Path.Combine(Environment.ExpandEnvironmentVariables("%temp%") + "foo.bar");

         var fileStream2 = System.IO.File.Create(fileName);
         Assert.AreEqual(fileStream2.Name, fileName);
         fileStream2.Close();
         File.Delete(fileName);

         var fileStream = File.Create(fileName);
         var fileStreamName = Path.GetFinalPathNameByHandle(fileStream.SafeFileHandle);

         Assert.AreNotEqual(fileName, fileStream.Name);
         Assert.AreEqual(fileName, Path.GetRegularPath(fileStreamName));

         fileStream.Close();
         File.Delete(fileName);
      }

      private void DumpGetSuffixedDirectoryName(bool isLocal)
      {
         Console.WriteLine("\n=== TEST {0} ===", isLocal ? UnitTestConstants.Local : UnitTestConstants.Network);

         var neDir = "Non-Existing Directory";
         var sys32 = Environment.SystemDirectory + Path.DirectorySeparator;

         var fullPath = Path.Combine(Environment.SystemDirectory, neDir);
         if (!isLocal)
         {
            fullPath = Path.LocalToUnc(fullPath);
            sys32 = Path.LocalToUnc(sys32);
         }

         UnitTestConstants.StopWatcher(true);
         var suffixedDirectoryName = Path.GetSuffixedDirectoryName(fullPath);
         Console.WriteLine("\nInput Path: [{0}]\n\tGetSuffixedDirectoryName() (Should be: [{1}]): [{2}]\n{3}", fullPath, sys32, suffixedDirectoryName, UnitTestConstants.Reporter());
         Assert.IsTrue(suffixedDirectoryName.Equals(sys32, StringComparison.OrdinalIgnoreCase));



         fullPath = Path.Combine(fullPath, "Non-Existing file.txt");
         neDir = Path.Combine(Environment.SystemDirectory, neDir) + Path.DirectorySeparator;
         if (!isLocal)
         {
            fullPath = Path.LocalToUnc(fullPath);
            neDir = Path.LocalToUnc(neDir);
         }

         UnitTestConstants.StopWatcher(true);
         suffixedDirectoryName = Path.GetSuffixedDirectoryName(fullPath);
         Console.WriteLine("\nInput Path: [{0}]\n\tGetSuffixedDirectoryName() (Should be: [{1}]): [{2}]\n{3}", fullPath, neDir, suffixedDirectoryName, UnitTestConstants.Reporter());
         Assert.IsTrue(suffixedDirectoryName.Equals(neDir, StringComparison.OrdinalIgnoreCase));



         fullPath = UnitTestConstants.SysRoot;
         if (!isLocal) fullPath = Path.LocalToUnc(fullPath);

         UnitTestConstants.StopWatcher(true);
         suffixedDirectoryName = Path.GetSuffixedDirectoryName(fullPath);
         Console.WriteLine("\nInput Path: [{0}]\n\tGetSuffixedDirectoryName() (Should be: [null]): [{1}]\n{2}", fullPath, suffixedDirectoryName ?? "null", UnitTestConstants.Reporter());
         Assert.AreEqual(null, suffixedDirectoryName);


         fullPath = UnitTestConstants.SysDrive + Path.DirectorySeparator;
         if (!isLocal) fullPath = Path.LocalToUnc(fullPath);

         UnitTestConstants.StopWatcher(true);
         suffixedDirectoryName = Path.GetSuffixedDirectoryNameWithoutRoot(fullPath);
         Console.WriteLine("\nInput Path: [{0}]\n\tGetSuffixedDirectoryName() (Should be: [null]): [{1}]\n{2}", fullPath, suffixedDirectoryName ?? "null", UnitTestConstants.Reporter());
         Assert.AreEqual(null, suffixedDirectoryName);

         Console.WriteLine("\n");
      }

      private void DumpGetSuffixedDirectoryNameWithoutRoot(bool isLocal)
      {
         Console.WriteLine("\n=== TEST {0} ===", isLocal ? UnitTestConstants.Local : UnitTestConstants.Network);

         var neDir = "Non-Existing Directory";
         var sys32 = (UnitTestConstants.SysRoot + Path.DirectorySeparator + "system32" + Path.DirectorySeparator).Replace(UnitTestConstants.SysDrive + Path.DirectorySeparator, "");

         var fullPath = Path.Combine(UnitTestConstants.SysRoot + Path.DirectorySeparator + "system32" + Path.DirectorySeparator, neDir);
         if (!isLocal) fullPath = Path.LocalToUnc(fullPath);

         UnitTestConstants.StopWatcher(true);
         var suffixedDirectoryNameWithoutRoot = Path.GetSuffixedDirectoryNameWithoutRoot(fullPath);
         Console.WriteLine("\nInput Path: [{0}]\n\tGetSuffixedDirectoryName() (Should be: [{1}]): [{2}]\n{3}", fullPath, sys32, suffixedDirectoryNameWithoutRoot, UnitTestConstants.Reporter());
         Assert.IsTrue(suffixedDirectoryNameWithoutRoot.Equals(sys32, StringComparison.OrdinalIgnoreCase), "Path mismatch.");



         fullPath = Path.Combine(fullPath, "Non-Existing file.txt");
         neDir = (Path.Combine(Environment.SystemDirectory, neDir) + Path.DirectorySeparator).Replace(UnitTestConstants.SysDrive + Path.DirectorySeparator, "");
         if (!isLocal) fullPath = Path.LocalToUnc(fullPath);

         UnitTestConstants.StopWatcher(true);
         suffixedDirectoryNameWithoutRoot = Path.GetSuffixedDirectoryNameWithoutRoot(fullPath);
         Console.WriteLine("\nInput Path: [{0}]\n\tGetSuffixedDirectoryName() (Should be: [{1}]): [{2}]\n{3}", fullPath, neDir, suffixedDirectoryNameWithoutRoot, UnitTestConstants.Reporter());
         Assert.IsTrue(suffixedDirectoryNameWithoutRoot.Equals(neDir, StringComparison.OrdinalIgnoreCase), "Path mismatch.");



         fullPath = UnitTestConstants.SysRoot;
         if (!isLocal) fullPath = Path.LocalToUnc(fullPath);

         UnitTestConstants.StopWatcher(true);
         suffixedDirectoryNameWithoutRoot = Path.GetSuffixedDirectoryNameWithoutRoot(fullPath);
         Console.WriteLine("\nInput Path: [{0}]\n\tGetSuffixedDirectoryName() (Should be: [null]): [{1}]\n{2}", fullPath, suffixedDirectoryNameWithoutRoot ?? "null", UnitTestConstants.Reporter());
         Assert.AreEqual(null, suffixedDirectoryNameWithoutRoot, "Path mismatch.");



         fullPath = UnitTestConstants.SysDrive + Path.DirectorySeparator;
         if (!isLocal) fullPath = Path.LocalToUnc(fullPath);

         UnitTestConstants.StopWatcher(true);
         suffixedDirectoryNameWithoutRoot = Path.GetSuffixedDirectoryNameWithoutRoot(fullPath);
         Console.WriteLine("\nInput Path: [{0}]\n\tGetSuffixedDirectoryName() (Should be: [null]): [{1}]\n{2}", fullPath, suffixedDirectoryNameWithoutRoot ?? "null", UnitTestConstants.Reporter());
         Assert.AreEqual(null, suffixedDirectoryNameWithoutRoot, "Path mismatch.");

         Console.WriteLine("\n");
      }

      #endregion // Unit Tests

      #region Unit Test Callers

      #region .NET

      #region Combine

      [TestMethod]
      public void Path_Combine()
      {
         Console.WriteLine("Path.Combine()");

         var pathCnt = 0;
         var errorCnt = 0;

         UnitTestConstants.StopWatcher(true);
         foreach (var path in UnitTestConstants.InputPaths)
         {
            foreach (var path2 in UnitTestConstants.InputPaths)
            {
               string expected = null;
               string actual = null;

               Console.WriteLine("\n#{0:000}\tInput Path: [{1}]\tCombine with: [{2}]", ++pathCnt, path, path2);

               // System.IO
               try
               {
                  expected = System.IO.Path.Combine(path, path2);
               }
               catch (Exception ex)
               {
                  Console.WriteLine("\tCaught [System.IO] {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
               }
               Console.WriteLine("\t   System.IO : [{0}]", expected ?? "null");


               // AlphaFS
               try
               {
                  actual = Path.Combine(path, path2);

                  Assert.AreEqual(expected, actual);
               }
               catch (Exception ex)
               {
                  errorCnt++;

                  Console.WriteLine("\tCaught [AlphaFS] {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
               }
               Console.WriteLine("\t   AlphaFS   : [{0}]", actual ?? "null");
            }
         }
         Console.WriteLine("\n{0}", UnitTestConstants.Reporter(true));

         Assert.AreEqual(0, errorCnt, "Encountered paths where AlphaFS != System.IO");
      }

      #endregion // Combine

      #region GetFileName

      [TestMethod]
      public void Path_GetFileName()
      {
         Console.WriteLine("Path.GetFileName()");

         var pathCnt = 0;
         var errorCnt = 0;
         var skipAssert = false;

         UnitTestConstants.StopWatcher(true);
         foreach (var path in UnitTestConstants.InputPaths)
         {
            string expected = null;
            string actual = null;

            Console.WriteLine("\n#{0:000}\tInput Path: [{1}]", ++pathCnt, path);

            // System.IO
            try
            {
               expected = System.IO.Path.GetFileName(path);
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
               actual = Path.GetFileName(path);

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

      #endregion // GetFileName

      #region GetFileNameWithoutExtension

      [TestMethod]
      public void Path_GetFileNameWithoutExtension()
      {
         Console.WriteLine("Path.GetFileNameWithoutExtension()");

         var pathCnt = 0;
         var errorCnt = 0;
         var skipAssert = false;

         UnitTestConstants.StopWatcher(true);
         foreach (var path in UnitTestConstants.InputPaths)
         {
            string expected = null;
            string actual = null;

            Console.WriteLine("\n#{0:000}\tInput Path: [{1}]", ++pathCnt, path);

            // System.IO
            try
            {
               expected = System.IO.Path.GetFileNameWithoutExtension(path);
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
               actual = Path.GetFileNameWithoutExtension(path);

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

      #endregion // GetFileNameWithoutExtension

      #region GetPathRoot

      [TestMethod]
      public void Path_GetPathRoot()
      {
         Console.WriteLine("Path.GetPathRoot()");

         var pathCnt = 0;
         var errorCnt = 0;
         var skipAssert = false;

         #region Exceptions

         try
         {
            Path.GetPathRoot("");
         }
         catch (ArgumentException)
         {
            skipAssert = true;
         }
         Assert.IsTrue(skipAssert, "ArgumentException should have been caught.");

         #endregion // Exceptions

         UnitTestConstants.StopWatcher(true);
         foreach (var path in UnitTestConstants.InputPaths)
         {
            string expected = null;
            string actual = null;

            Console.WriteLine("\n#{0:000}\tInput Path: [{1}]", ++pathCnt, path);

            // System.IO
            try
            {
               expected = System.IO.Path.GetPathRoot(path);

               skipAssert = path.StartsWith(Path.LongPathUncPrefix);
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
               actual = Path.GetPathRoot(path);

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

      #endregion // GetPathRoot




      #endregion // .NET

      #region AlphaFS

      #region AddTrailingDirectorySeparator

      [TestMethod]
      public void AlphaFS_Path_AddTrailingDirectorySeparator()
      {
         Console.WriteLine("Path.AddTrailingDirectorySeparator()\n");

         const string nonSlashedString = "SlashMe";
         Console.WriteLine("\tstring: [{0}];\n", nonSlashedString);

         // True, add DirectorySeparatorChar.
         var hasBackslash = Path.AddTrailingDirectorySeparator(nonSlashedString, false);
         var addedBackslash = hasBackslash.EndsWith(Path.DirectorySeparatorChar.ToString()) && (nonSlashedString + Path.DirectorySeparatorChar).Equals(hasBackslash);
         Console.WriteLine("\tAddTrailingDirectorySeparator(string);\n\tAdded == [{0}]: {1}\n\tResult: [{2}]\n", UnitTestConstants.TextTrue, addedBackslash, hasBackslash);

         // True, add AltDirectorySeparatorChar.
         var hasSlash = Path.AddTrailingDirectorySeparator(nonSlashedString, true);
         var addedSlash = hasSlash.EndsWith(Path.AltDirectorySeparatorChar.ToString(CultureInfo.InvariantCulture)) && (nonSlashedString + Path.AltDirectorySeparatorChar).Equals(hasSlash);
         Console.WriteLine("\tAddTrailingDirectorySeparator(string, true);\n\tAdded == [{0}]: {1}\n\tResult: [{2}]\n", UnitTestConstants.TextTrue, addedSlash, hasSlash);

         Assert.IsTrue(addedBackslash);
         Assert.IsTrue(addedSlash);
      }

      #endregion // AddTrailingDirectorySeparator

      #region GetDirectoryNameWithoutRoot

      [TestMethod]
      public void AlphaFS_Path_GetDirectoryNameWithoutRoot()
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
      public void AlphaFS_Path_GetFinalPathNameByHandle()
      {
         Console.WriteLine("Path.GetFinalPathNameByHandle()");

         DumpGetFinalPathNameByHandle(true);
      }

      #endregion // GetFinalPathNameByHandle

      #region GetSuffixedDirectoryName

      [TestMethod]
      public void AlphaFS_Path_GetSuffixedDirectoryName()
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
      public void AlphaFS_Path_GetSuffixedDirectoryNameWithoutRoot()
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
      public void AlphaFS_Path_GetLongPath()
      {
         Console.WriteLine("Path.GetLongPath()");

         var pathCnt = 0;
         var errorCnt = 0;

         UnitTestConstants.StopWatcher(true);
         foreach (var path in UnitTestConstants.InputPaths)
         {
            string actual = null;

            Console.WriteLine("\n#{0:000}\tInput Path: [{1}]", ++pathCnt, path);

            // AlphaFS
            try
            {
               actual = Path.GetLongPath(path);

               if (Path.IsUncPath(path))
                  Assert.IsTrue(actual.StartsWith(Path.LongPathUncPrefix), "Path should start with a long unc path prefix.");
               else
               {
                  var c = path[0];
                  if (!Path.IsPathRooted(path) && (c >= 'a' && c <= 'z' || c >= 'A' && c <= 'Z'))
                     Assert.IsFalse(actual.StartsWith(Path.LongPathPrefix), "Path should not start with a long path prefix.");
                  else
                  {
                     if (!Path.IsPathRooted(path) && !Utils.IsNullOrWhiteSpace(Path.GetDirectoryName(path)))
                        Assert.IsTrue(actual.StartsWith(Path.LongPathUncPrefix), "Path should start with a long path prefix.");
                  }
               }
            }
            catch (Exception ex)
            {
               Console.WriteLine("\tCaught [AlphaFS] {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));

               errorCnt++;
            }
            Console.WriteLine("\t   AlphaFS   : [{0}]", actual ?? "null");
         }
         Console.WriteLine("\n{0}", UnitTestConstants.Reporter(true));

         Assert.AreEqual(0, errorCnt, "No errors were expected.");
      }

      #endregion // GetLongPath

      #region GetLongFrom83ShortPath

      [TestMethod]
      public void AlphaFS_Path_GetLongFrom83ShortPath()
      {
         Console.WriteLine("Path.GetLongFrom83ShortPath()");

         Dump83Path(true);
         Dump83Path(false);
      }

      #endregion // GetLongFrom83ShortPath

      #region GetMappedConnectionName

      [TestMethod]
      public void AlphaFS_Path_GetMappedConnectionName()
      {
         Console.WriteLine("Path.GetMappedConnectionName()");

         var cnt = 0;
         UnitTestConstants.StopWatcher(true);
         foreach (var drive in Directory.GetLogicalDrives().Where(drive => new DriveInfo(drive).IsUnc))
         {
            ++cnt;

            UnitTestConstants.StopWatcher(true);
            var gmCn = Path.GetMappedConnectionName(drive);
            var gmUn = Path.GetMappedUncName(drive);
            Console.WriteLine("\n\tMapped drive: [{0}]\tGetMappedConnectionName(): [{1}]", drive, gmCn);
            Console.WriteLine("\tMapped drive: [{0}]\tGetMappedUncName()       : [{1}]", drive, gmUn);

            Assert.IsTrue(!Utils.IsNullOrWhiteSpace(gmCn));
            Assert.IsTrue(!Utils.IsNullOrWhiteSpace(gmUn));
         }
         Console.WriteLine("\n{0}", UnitTestConstants.Reporter(true));

         if (cnt == 0)
            Assert.Inconclusive("Nothing was enumerated because no mapped drives were found.");
      }

      #endregion // GetMappedConnectionName

      #region GetRegularPath

      [TestMethod]
      public void AlphaFS_Path_GetRegularPath()
      {
         Console.WriteLine("Path.GetRegularPath()");

         var pathCnt = 0;
         var errorCnt = 0;

         UnitTestConstants.StopWatcher(true);
         foreach (var path in UnitTestConstants.InputPaths)
         {
            string actual = null;

            Console.WriteLine("\n#{0:000}\tInput Path: [{1}]", ++pathCnt, path);

            // AlphaFS
            try
            {
               actual = Path.GetRegularPath(path);

               if (actual.StartsWith(Path.GlobalRootPrefix, StringComparison.OrdinalIgnoreCase) ||
                   actual.StartsWith(Path.VolumePrefix, StringComparison.OrdinalIgnoreCase))
                  continue;

               Assert.IsFalse(actual.StartsWith(Path.LongPathPrefix, StringComparison.OrdinalIgnoreCase));
            }
            catch (Exception ex)
            {
               errorCnt++;

               Console.WriteLine("\tCaught [AlphaFS] {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
            }
            Console.WriteLine("\t   AlphaFS   : [{0}]", actual ?? "null");
         }
         Console.WriteLine("\n{0}", UnitTestConstants.Reporter(true));

         Assert.AreEqual(0, errorCnt, "No errors were expected.");
      }

      #endregion // GetRegularPath

      #region IsLongPath

      [TestMethod]
      public void AlphaFS_Path_IsLongPath()
      {
         Console.WriteLine("Path.IsLongPath()");

         var pathCnt = 0;
         var errorCnt = 0;
         var longPathCnt = 0;

         UnitTestConstants.StopWatcher(true);
         foreach (var path in UnitTestConstants.InputPaths)
         {
            var actual = false;

            Console.WriteLine("\n#{0:000}\tInput Path: [{1}]", ++pathCnt, path);

            // AlphaFS
            try
            {
               var expected = path.StartsWith(Path.LongPathPrefix, StringComparison.OrdinalIgnoreCase);
               actual = Path.IsLongPath(path);

               Assert.AreEqual(expected, actual);

               if (actual)
                  longPathCnt++;
            }
            catch (Exception ex)
            {
               errorCnt++;

               Console.WriteLine("\tCaught [AlphaFS] {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
            }
            Console.WriteLine("\tAlphaFS   : [{0}]", actual);
         }
         Console.WriteLine("\n{0}", UnitTestConstants.Reporter(true));

         // Hand counted 33 True's.
         Assert.AreEqual(33, longPathCnt, "Number of local paths do not match.", errorCnt);

         Assert.AreEqual(0, errorCnt, "No errors were expected.");
      }

      #endregion // IsLongPath

      #region IsUncPath

      [TestMethod]
      public void AlphaFS_Path_IsUncPath()
      {
         Console.WriteLine("Path.IsUncPath()");

         var pathCnt = 0;
         var errorCnt = 0;
         var uncPathCnt = 0;

         UnitTestConstants.StopWatcher(true);
         foreach (var path in UnitTestConstants.InputPaths)
         {
            var actual = false;

            Console.WriteLine("\n#{0:000}\tInput Path: [{1}]", ++pathCnt, path);

            // AlphaFS
            try
            {
               var expected = path.StartsWith(Path.UncPrefix, StringComparison.OrdinalIgnoreCase);

               actual = Path.IsUncPath(path);

               if (!(!path.StartsWith(Path.GlobalRootPrefix, StringComparison.OrdinalIgnoreCase) ||
                     !path.StartsWith(Path.VolumePrefix, StringComparison.OrdinalIgnoreCase)))
                  Assert.AreEqual(expected, actual);

               if (actual)
                  uncPathCnt++;
            }
            catch (Exception ex)
            {
               errorCnt++;

               Console.WriteLine("\tCaught [AlphaFS] {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
            }
            Console.WriteLine("\tAlphaFS   : [{0}]", actual);
         }
         Console.WriteLine("\n{0}", UnitTestConstants.Reporter(true));

         // Hand counted 32 True's.
         Assert.AreEqual(32, uncPathCnt, "Number of UNC paths do not match.", errorCnt);

         Assert.AreEqual(0, errorCnt, "No errors were expected.");
      }

      #endregion // IsUncPath


      #region RemoveTrailingDirectorySeparator

      [TestMethod]
      public void AlphaFS_Path_RemoveTrailingDirectorySeparator()
      {
         Console.WriteLine("Path.RemoveTrailingDirectorySeparator()\n");

         const string backslashedString = @"Backslashed\";
         const string slashedString = "Slashed/";
         // True, add DirectorySeparatorChar.
         var hasBackslash = Path.RemoveTrailingDirectorySeparator(backslashedString);
         var removedBackslash = !hasBackslash.EndsWith(Path.DirectorySeparatorChar.ToString(CultureInfo.InvariantCulture)) && !backslashedString.Equals(hasBackslash);
         Console.WriteLine("\tstring = @[{0}];\n", backslashedString);
         Console.WriteLine("\tRemoveTrailingDirectorySeparator(string);\n\tRemoved == [{0}]: {1}\n\tResult: [{2}]\n", UnitTestConstants.TextTrue, removedBackslash, hasBackslash);

         // True, add AltDirectorySeparatorChar.
         var hasSlash = Path.RemoveTrailingDirectorySeparator(slashedString, true);
         var removedSlash = !hasSlash.EndsWith(Path.AltDirectorySeparatorChar.ToString(CultureInfo.InvariantCulture)) && !slashedString.Equals(hasSlash);
         Console.WriteLine("\tstring: [{0}];\n", slashedString);
         Console.WriteLine("\tRemoveTrailingDirectorySeparator(string, true);\n\tRemoved == [{0}]: {1}\n\tResult: [{2}]\n", UnitTestConstants.TextTrue, removedSlash, hasSlash);

         Assert.IsTrue(removedBackslash);
         Assert.IsTrue(removedSlash);
      }

      #endregion // RemoveTrailingDirectorySeparator

      #endregion // AlphaFS

      #endregion // Unit Test Callers
   }
}
