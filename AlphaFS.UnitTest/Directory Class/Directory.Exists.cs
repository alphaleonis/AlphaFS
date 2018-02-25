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
using System.Collections.Generic;
using System.Reflection;

namespace AlphaFS.UnitTest
{
   partial class DirectoryTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void Directory_Exists_ExistingDirectory_Exists_Success()
      {
         Directory_Exists_ExistingDirectory_Exists(false);
         Directory_Exists_ExistingDirectory_Exists(true);
      }


      [TestMethod]
      public void Directory_Exists_NonExistingDirectory_DoesNotExist_Success()
      {
         Directory_Exists_NonExistingDirectory_DoesNotExist(false);
         Directory_Exists_NonExistingDirectory_DoesNotExist(true);
      }


      [TestMethod]
      public void Directory_Exists_DriveLetter_LocalAndNetwork_Success()
      {
         Directory_Exists_DriveLetter(false);
         Directory_Exists_DriveLetter(true);
      }



      [TestMethod]
      public void Directory_Exists_UseCases_LocalAndNetwork_Success()
      {
         Directory_Exists_UseCases(false);
         Directory_Exists_UseCases(true);
      }

      
      [TestMethod]
      public void AlphaFS_Directory_Exists_WithLeadingOrTrailingSpace_LocalAndNetwork_Success()
      {
         Directory_Exists_WithLeadingOrTrailingSpace(false);
         Directory_Exists_WithLeadingOrTrailingSpace(true);
      }




      private void Directory_Exists_ExistingDirectory_Exists(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         var tempPath = System.IO.Path.GetTempPath();
         if (isNetwork)
            tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);


         using (var rootDir = new TemporaryDirectory(tempPath, MethodBase.GetCurrentMethod().Name))
         {
            var folder = rootDir.RandomDirectoryFullPath;
            System.IO.Directory.CreateDirectory(folder);
            Console.WriteLine("\nInput Existing Directory Path: [{0}]\n", folder);

            var shouldBe = true;
            var existSysIO = System.IO.Directory.Exists(folder);
            var existAlpha = Alphaleonis.Win32.Filesystem.Directory.Exists(folder);

            Assert.AreEqual(shouldBe, existSysIO, "The result should be: " + shouldBe.ToString().ToUpperInvariant());
            Assert.AreEqual(existSysIO, existAlpha, "The results are not equal, but are expected to be.");
         }


         Console.WriteLine();
      }


      private void Directory_Exists_DriveLetter(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         // Test Driveletter according to System.IO
         var driveSysIO = UnitTestConstants.SysDrive;
         if (isNetwork)
            driveSysIO = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(driveSysIO);


         // C:
         var sysIOshouldBe = true;
         var inputDrive = driveSysIO;
         var existSysIO = System.IO.Directory.Exists(inputDrive);
         var existAlpha = Alphaleonis.Win32.Filesystem.Directory.Exists(inputDrive);
         Console.WriteLine("\nSystem.IO/AlphaFS (should be {0}):\t[{1}]\t\tdrive= {2}", sysIOshouldBe.ToString().ToUpperInvariant(), existSysIO, inputDrive);

         Assert.AreEqual(sysIOshouldBe, existSysIO, "The result should be: " + sysIOshouldBe.ToString().ToUpperInvariant());
         Assert.AreEqual(existSysIO, existAlpha, "The results are not equal, but are expected to be.");




         //C:\
         sysIOshouldBe = true;
         inputDrive = driveSysIO + @"\";
         existSysIO = System.IO.Directory.Exists(inputDrive);
         existAlpha = Alphaleonis.Win32.Filesystem.Directory.Exists(inputDrive);
         Console.WriteLine("System.IO/AlphaFS (should be {0}):\t[{1}]\t\tdrive= {2}", sysIOshouldBe.ToString().ToUpperInvariant(), existSysIO, inputDrive);

         Assert.AreEqual(sysIOshouldBe, existSysIO, "The result should be: " + sysIOshouldBe.ToString().ToUpperInvariant());
         Assert.AreEqual(existSysIO, existAlpha, "The results are not equal, but are expected to be.");



         
         // \\?\C:
         sysIOshouldBe = false;
         inputDrive = isNetwork ? Alphaleonis.Win32.Filesystem.Path.LongPathUncPrefix + driveSysIO.TrimStart('\\') : Alphaleonis.Win32.Filesystem.Path.LongPathPrefix + driveSysIO;
         existSysIO = System.IO.Directory.Exists(inputDrive);
         existAlpha = Alphaleonis.Win32.Filesystem.Directory.Exists(inputDrive);
         Console.WriteLine("\nSystem.IO (should be {0}):\t[{1}]\t\tdrive= {2}", sysIOshouldBe.ToString().ToUpperInvariant(), existSysIO, inputDrive);
         Console.WriteLine("AlphaFS   (should be {0}):\t[{1}]\t\tdrive= {2}", true.ToString().ToUpperInvariant(), existAlpha, inputDrive);

         Assert.AreEqual(sysIOshouldBe, existSysIO, "The result should be: " + sysIOshouldBe.ToString().ToUpperInvariant());
         Assert.IsTrue(existAlpha);
         



         // \\?\C:\
         sysIOshouldBe = false;
         inputDrive = (isNetwork ? Alphaleonis.Win32.Filesystem.Path.LongPathUncPrefix + driveSysIO.TrimStart('\\') : Alphaleonis.Win32.Filesystem.Path.LongPathPrefix + driveSysIO) + @"\";
         existSysIO = System.IO.Directory.Exists(inputDrive);
         existAlpha = Alphaleonis.Win32.Filesystem.Directory.Exists(inputDrive);
         Console.WriteLine("\nSystem.IO (should be {0}):\t[{1}]\t\tdrive= {2}", sysIOshouldBe.ToString().ToUpperInvariant(), existSysIO, inputDrive);
         Console.WriteLine("AlphaFS   (should be {0}):\t[{1}]\t\tdrive= {2}", true.ToString().ToUpperInvariant(), existAlpha, inputDrive);

         if (!isNetwork)
            Console.WriteLine("BUG: AlphaFS should return TRUE.");


         Assert.AreEqual(sysIOshouldBe, existSysIO, "The result should be: " + sysIOshouldBe.ToString().ToUpperInvariant());
         Assert.AreEqual(isNetwork, existAlpha);


         Console.WriteLine();
      }


      private void Directory_Exists_NonExistingDirectory_DoesNotExist(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         var tempPath = System.IO.Path.GetTempPath();
         if (isNetwork)
            tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);


         using (var rootDir = new TemporaryDirectory(tempPath, MethodBase.GetCurrentMethod().Name))
         {
            var folder = rootDir.RandomDirectoryFullPath;

            var shouldBe = false;
            var existSysIO = System.IO.Directory.Exists(folder);
            var existAlpha = Alphaleonis.Win32.Filesystem.Directory.Exists(folder);
            Console.WriteLine("\nInput Non-Existing Directory Path: [{0}]", folder);

            Assert.AreEqual(shouldBe, existSysIO, "The result should be: " + shouldBe.ToString().ToUpperInvariant());
            Assert.AreEqual(existSysIO, existAlpha, "The results are not equal, but are expected to be.");
         }


         Console.WriteLine();
      }


      private void Directory_Exists_UseCases(bool isNetwork)
      {
         // #288: Directory.Exists on root drive problem has come back with recent updates


         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         var tempPath = System.IO.Path.GetTempPath();
         if (isNetwork)
            tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);


         using (new TemporaryDirectory(tempPath, MethodBase.GetCurrentMethod().Name))
         {
            var randomName = UnitTestConstants.GetRandomFileNameWithDiacriticCharacters();

            // C:\randomName
            var nonExistingFolder1 = UnitTestConstants.SysDrive + @"\" + randomName;

            // C:randomName
            var nonExistingFolder2 = UnitTestConstants.SysDrive + randomName;


            // C:\randomName-exists
            var existingFolder1 = nonExistingFolder1 + "-exists";
            System.IO.Directory.CreateDirectory(existingFolder1);

            // C:randomName-exists
            var existingFolder2 = nonExistingFolder2 + "-exists";
            System.IO.Directory.CreateDirectory(existingFolder1);



            if (isNetwork)
            {
               nonExistingFolder1 = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(nonExistingFolder1);
               nonExistingFolder2 = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(nonExistingFolder2);
               existingFolder1 = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(existingFolder1);
               existingFolder2 = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(existingFolder2);
            }


            // Some use cases.
            var paths = new Dictionary<string, List<bool>>
            {
               {nonExistingFolder1, new List<bool> {false, false}},
               {nonExistingFolder2, new List<bool> {false, false}},
               {nonExistingFolder1 + @"\", new List<bool> {false, false}},
               {nonExistingFolder2 + @"\", new List<bool> {false, false}},

               {existingFolder1, new List<bool> {true, true}},
               {existingFolder2, new List<bool> {!isNetwork, !isNetwork}},
               {existingFolder1 + @"\", new List<bool> {true, true}},
               {existingFolder2 + @"\", new List<bool> {!isNetwork, !isNetwork}}
            };


            try
            {
               foreach (var path in paths)
               {
                  var sysIOshouldBe = path.Value[0];
                  var alphaFSshouldBe = path.Value[1];
                  var inputPath = path.Key;
                  var existSysIO = System.IO.Directory.Exists(inputPath);
                  var existAlpha = Alphaleonis.Win32.Filesystem.Directory.Exists(inputPath);


                  Console.WriteLine("\nSystem.IO (should be {0}):\t[{1}]\t\tdirectory= {2}", sysIOshouldBe.ToString().ToUpperInvariant(), existSysIO, inputPath);
                  Console.WriteLine("AlphaFS   (should be {0}):\t[{1}]\t\tdirectory= {2}", alphaFSshouldBe.ToString().ToUpperInvariant(), existAlpha, inputPath);


                  //if (sysIOshouldBe)
                  //   Assert.IsTrue(existSysIO);
                  //else
                  //   Assert.IsFalse(existSysIO);
                  Assert.AreEqual(sysIOshouldBe, existSysIO);



                  //if (alphaFSshouldBe)
                  //   Assert.IsTrue(alphaFSshouldBe);
                  //else
                  //   Assert.IsFalse(alphaFSshouldBe);
                  //Assert.AreEqual(alphaFSshouldBe, existAlpha);
               }
            }
            finally
            {
               System.IO.Directory.Delete(existingFolder1);
            }
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

         Assert.IsTrue(Alphaleonis.Win32.Filesystem.Directory.Exists(path), "The directory does not exist, but is expected to.");


         path = "   " + tempPath + "   ";
         Console.WriteLine("Input Directory Path: [{0}]\n", path);

         Assert.IsTrue(Alphaleonis.Win32.Filesystem.Directory.Exists(path), "The directory does not exist, but is expected to.");


         path = "   " + tempPath;
         Console.WriteLine("Input Directory Path: [{0}]\n", path);

         Assert.IsTrue(Alphaleonis.Win32.Filesystem.Directory.Exists(path), "The directory does not exist, but is expected to.");
      }
   }
}
