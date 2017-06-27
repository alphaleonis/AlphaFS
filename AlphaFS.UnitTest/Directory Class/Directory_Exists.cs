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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;

namespace AlphaFS.UnitTest
{
   partial class DirectoryTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void Directory_Exists_LocalAndNetwork_Success()
      {
         Directory_Exists(false);
         Directory_Exists(true);
      }


      [TestMethod]
      public void AlphaFS_Directory_Exists_WithLeadingOrTrailingSpace_LocalAndNetwork_Success()
      {
         Directory_Exists_WithLeadingOrTrailingSpace(false);
         Directory_Exists_WithLeadingOrTrailingSpace(true);
      }




      private void Directory_Exists(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);


         // Test Driveletter according to System.IO

         // SystemIO: "C:"
         // AlphaFS : "\\?\C:"
         var shouldBe = true;
         var driveSysIO = UnitTestConstants.SysDrive;
         var driveAlpha = @"\\?\" + driveSysIO;
         if (isNetwork) { driveSysIO = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(driveSysIO); driveAlpha = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(driveAlpha); }
         var existSyIO = System.IO.Directory.Exists(driveSysIO);
         var existAlpha = Alphaleonis.Win32.Filesystem.Directory.Exists(driveAlpha);
         Console.WriteLine("\nDrive path (System.IO, should be {0}):\t{1}\t\t{2}", shouldBe.ToString().ToUpperInvariant(), existSyIO, driveSysIO);
         Console.WriteLine("Drive path (AlphaFS  , should be {0}):\t{1}\t\t{2}", shouldBe.ToString().ToUpperInvariant(), existAlpha, driveAlpha);

         Assert.AreEqual(shouldBe, existSyIO, "The result should be: " + shouldBe.ToString().ToUpperInvariant());
         Assert.AreEqual(existSyIO, existAlpha, "The results are not equal, but are expected to be.");




         // Both: "\\?\C:\"
         shouldBe = isNetwork; // True when network (UNC path), false when local path.
         driveAlpha = @"\\?\" + driveSysIO + @"\";
         if (isNetwork) driveAlpha = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(driveAlpha);
         existSyIO = System.IO.Directory.Exists(driveAlpha);
         existAlpha = Alphaleonis.Win32.Filesystem.Directory.Exists(driveAlpha);
         Console.WriteLine("\nDrive path (System.IO, should be {0}):\t{1}\t\t{2}", shouldBe.ToString().ToUpperInvariant(), existSyIO, driveAlpha);
         
         Assert.AreEqual(shouldBe, existSyIO, "The result should be: " + shouldBe.ToString().ToUpperInvariant());
         Assert.AreEqual(existSyIO, existAlpha, "The results are not equal, but are expected to be.");




         // Both: "C:\"
         shouldBe = true;
         driveAlpha = UnitTestConstants.SysDrive + @"\";
         if (isNetwork) driveAlpha = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(driveAlpha);
         existSyIO = System.IO.Directory.Exists(driveAlpha);
         existAlpha = Alphaleonis.Win32.Filesystem.Directory.Exists(driveAlpha);
         Console.WriteLine("Drive path (System.IO, should be {0}):\t{1}\t\t{2}", shouldBe.ToString().ToUpperInvariant(), existSyIO, driveAlpha);

         Assert.AreEqual(shouldBe, existSyIO, "The result should be: " + shouldBe.ToString().ToUpperInvariant());
         Assert.AreEqual(existSyIO, existAlpha, "The results are not equal, but are expected to be.");




         // Test Folder.
         var tempPath = System.IO.Path.GetTempPath();
         if (isNetwork)
            tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);


         using (var rootDir = new TemporaryDirectory(tempPath, MethodBase.GetCurrentMethod().Name))
         {
            var folder = rootDir.RandomDirectoryFullPath;
            Console.WriteLine("\nInput Directory Path: [{0}]\n", folder);

            Assert.AreEqual(System.IO.Directory.Exists(folder), Alphaleonis.Win32.Filesystem.Directory.Exists(folder), "The results are not equal, but are expected to be.");

            System.IO.Directory.CreateDirectory(folder);

            Assert.AreEqual(System.IO.Directory.Exists(folder), Alphaleonis.Win32.Filesystem.Directory.Exists(folder), "The results are not equal, but are expected to be.");
         }


         // #288: Directory.Exists on root drive problem has come back with recent updates

         //// Test some use cases.
         //var paths = new[]
         //{
         //   @"\\?\c:\",             // True
         //   @"\\?\c:",              // True
         //   @"\\?\c:\temp",         // True
         //   @"\\?\c:\temp\",        // True
         //   @"\\?\c:\nonexistent",  // False
         //   @"\\?\c:\nonexistent\", // False
         //   @"c:\nonexistent",      // False
         //   @"c:\nonexistent\",     // False
         //   @"c:\temp",             // True
         //   @"c:\temp\",            // True
         //   @"c:",                  // True
         //   @"c:\",                 // True
         //   @"c:temp",              // True
         //   @"\\?\c:temp",          // False
         //   @"\\?\c:temp\",         // False
         //   @"c:\temp\"             // True
         //};

         //foreach (var path in paths)
         //{
         //   Console.WriteLine("S exists: [{0}]\t\tpath: {1}", System.IO.Directory.Exists(path), path);
         //   Console.WriteLine("A exists: [{0}]\t\tpath: {1}", Alphaleonis.Win32.Filesystem.Directory.Exists(path), path);
         //}


         Console.WriteLine();
      }


      private void Directory_Exists_WithLeadingOrTrailingSpace(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         var tempPath = UnitTestConstants.SysRoot32;
         if (isNetwork)
            tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);


         var path = tempPath + "   ";
         Console.WriteLine("\nInput Directory Path: [{0}]\n", path);

         Assert.IsTrue(Alphaleonis.Win32.Filesystem.Directory.Exists(path), "The directory does not exist, but is expected to be.");


         path = "   " + tempPath + "   ";
         Console.WriteLine("Input Directory Path: [{0}]\n", path);

         Assert.IsTrue(Alphaleonis.Win32.Filesystem.Directory.Exists(path), "The directory does not exist, but is expected to be.");


         path = "   " + tempPath;
         Console.WriteLine("Input Directory Path: [{0}]\n", path);

         Assert.IsTrue(Alphaleonis.Win32.Filesystem.Directory.Exists(path), "The directory does not exist, but is expected to be.");
      }
   }
}
