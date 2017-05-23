/*  Copyright (C) 2008-2016 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
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
         var drive = @"\\?\" + UnitTestConstants.SysDrive;
         if (isNetwork) { driveSysIO = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(driveSysIO); drive = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(drive); }
         Console.Write("\nDrive path (System.IO, should be " + shouldBe + "):\t\t" + driveSysIO + "\t\t\t");
         var existSystemIO = System.IO.Directory.Exists(driveSysIO);
         Console.WriteLine(existSystemIO); 
         var existAlphaFS = Alphaleonis.Win32.Filesystem.Directory.Exists(drive);
         Console.Write("Drive path (AlphaFS, should be " + shouldBe + "):\t\t" + drive + "\t\t\t");
         Console.WriteLine(existAlphaFS);
         Assert.AreEqual(shouldBe, existSystemIO, "The result should be: " + shouldBe);
         Assert.AreEqual(existSystemIO, existAlphaFS, "The results are not equal, but were expected to be.");
         Console.WriteLine();




         // Both: "\\?\C:\"
         shouldBe = isNetwork; // True when network (UNC path), false when local path.
         drive = @"\\?\" + UnitTestConstants.SysDrive + @"\";
         if (isNetwork) drive = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(drive);
         Console.Write("Drive path (System.IO, should be " + shouldBe + "):\t\t" + drive + "\t\t\t");
         existSystemIO = System.IO.Directory.Exists(drive);
         existAlphaFS = Alphaleonis.Win32.Filesystem.Directory.Exists(drive);
         Console.WriteLine(existSystemIO);
         Assert.AreEqual(shouldBe, existSystemIO, "The result should be: " + shouldBe);
         Assert.AreEqual(existSystemIO, existAlphaFS, "The results are not equal, but were expected to be.");



         // Both: "C:\"
         shouldBe = true;
         drive = UnitTestConstants.SysDrive + @"\";
         if (isNetwork) drive = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(drive);
         Console.Write("Drive path (System.IO, should be " + shouldBe + "):\t\t" + drive + "\t\t\t");
         existSystemIO = System.IO.Directory.Exists(drive);
         existAlphaFS = Alphaleonis.Win32.Filesystem.Directory.Exists(drive);
         Console.WriteLine(existSystemIO);
         Assert.AreEqual(shouldBe, existSystemIO, "The result should be: " + shouldBe);
         Assert.AreEqual(existSystemIO, existAlphaFS, "The results are not equal, but were expected to be.");




         // Test Folder.

         var tempPath = System.IO.Path.GetTempPath();
         if (isNetwork)
            tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);


         using (var rootDir = new TemporaryDirectory(tempPath, "Directory.Exists"))
         {
            var folder = rootDir.RandomFileFullPath;
            Console.WriteLine("\nInput Directory Path: [{0}]\n", folder);

            Assert.IsFalse(Alphaleonis.Win32.Filesystem.Directory.Exists(folder), "The directory exists, but is expected not to.");

            System.IO.Directory.CreateDirectory(folder);

            Assert.IsTrue(Alphaleonis.Win32.Filesystem.Directory.Exists(folder), "The directory does not exists, but is expected to be.");
         }


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
