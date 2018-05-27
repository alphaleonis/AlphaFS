/*  Copyright (C) 2008-2018 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using Directory = Alphaleonis.Win32.Filesystem.Directory;
using DriveInfo = Alphaleonis.Win32.Filesystem.DriveInfo;
using File = Alphaleonis.Win32.Filesystem.File;
using Path = Alphaleonis.Win32.Filesystem.Path;

namespace AlphaFS.UnitTest
{
   /// <summary>This is a test class for Path and is intended to contain all Path Unit Tests.</summary>
   [TestClass]
   public partial class PathTest
   {
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

            short83Path = Path.GetShort83Path(myLongPath);

            Console.WriteLine("Short 8.3 file path    : [{0}]", short83Path);

            Assert.IsTrue(!short83Path.Equals(myLongPath));

            Assert.IsTrue(short83Path.EndsWith(@"~1"));



            var longFrom83Path = Path.GetLongFrom83ShortPath(short83Path);

            Console.WriteLine("Long path from 8.3 path: [{0}]{1}", longFrom83Path);

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

            short83Path = Path.GetShort83Path(myLongPath);

            Console.WriteLine("Short 8.3 directory path: [{0}]", short83Path);

            Assert.IsFalse(short83Path.Equals(myLongPath));

            Assert.IsTrue(short83Path.EndsWith(@"~1"));



            var longFrom83Path = Path.GetLongFrom83ShortPath(short83Path);

            Console.WriteLine("Long path from 8.3 path : [{0}]{1}", longFrom83Path);

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
         foreach (var drive in Directory.GetLogicalDrives().Where(drive => new DriveInfo(drive).IsUnc))
         {
            ++cnt;

            var gmCn = Path.GetMappedConnectionName(drive);
            var gmUn = Path.GetMappedUncName(drive);
            Console.WriteLine("\n\tMapped drive: [{0}]\tGetMappedConnectionName(): [{1}]", drive, gmCn);
            Console.WriteLine("\tMapped drive: [{0}]\tGetMappedUncName()       : [{1}]", drive, gmUn);

            Assert.IsTrue(!Utils.IsNullOrWhiteSpace(gmCn));
            Assert.IsTrue(!Utils.IsNullOrWhiteSpace(gmUn));
         }
         Console.WriteLine("\n{0}");

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
            Console.WriteLine("\t    AlphaFS   : [{0}]", actual ?? "null");
         }
         Console.WriteLine("\n{0}");

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
         Console.WriteLine("\n{0}");

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
         Console.WriteLine("\n{0}");

         // Hand counted 32 True's.
         Assert.AreEqual(32, uncPathCnt, "Number of UNC paths do not match.", errorCnt);

         Assert.AreEqual(0, errorCnt, "No errors were expected.");
      }

      #endregion // IsUncPath
   }
}
