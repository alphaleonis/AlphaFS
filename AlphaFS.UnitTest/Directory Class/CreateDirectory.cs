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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AlphaFS.UnitTest
{
   partial class DirectoryTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>

      [TestMethod]
      public void Directory_CreateDirectory_LocalAndUNC_Success()
      {
         Directory_CreateDirectory(false);
         Directory_CreateDirectory(true);
      }


      [TestMethod]
      public void Directory_CreateDirectory_WithFileSecurity_LocalAndUNC_Success()
      {
         if (!UnitTestConstants.IsAdmin())
            Assert.Inconclusive();

         Directory_CreateDirectory_WithFileSecurity(false);
         Directory_CreateDirectory_WithFileSecurity(true);
      }


      [TestMethod]
      public void Directory_CreateDirectory_CatchAlreadyExistsException_FileExistsWithSameNameAsDirectory_LocalAndUNC_Success()
      {
         Directory_CreateDirectory_CatchAlreadyExistsException_FileExistsWithSameNameAsDirectory(false);
         Directory_CreateDirectory_CatchAlreadyExistsException_FileExistsWithSameNameAsDirectory(true);
      }


      [TestMethod]
      public void Directory_CreateDirectory_CatchArgumentException_PathContainsInvalidCharacters_LocalAndUNC_Success()
      {
         Directory_CreateDirectory_CatchArgumentException_PathContainsInvalidCharacters(false);
         Directory_CreateDirectory_CatchArgumentException_PathContainsInvalidCharacters(true);
      }


      [TestMethod]
      public void Directory_CreateDirectory_CatchArgumentException_PathStartsWithColon_Local_Success()
      {
         Directory_CreateDirectory_CatchArgumentException_PathStartsWithColon(false);
      }


      [TestMethod]
      public void Directory_CreateDirectory_CatchNotSupportedException_PathContainsColon_Local_Success()
      {
         Directory_CreateDirectory_CatchNotSupportedException_PathContainsColon(false);
         Directory_CreateDirectory_CatchNotSupportedException_PathContainsColon(true);
      }


      [TestMethod]
      public void Directory_CreateDirectory_CatchDirectoryNotFoundException_NonExistingDriveLetter_LocalAndUNC_Success()
      {
         Directory_CreateDirectory_CatchDirectoryNotFoundException_NonExistingDriveLetter(false);
         Directory_CreateDirectory_CatchDirectoryNotFoundException_NonExistingDriveLetter(true);
      }







      private void Directory_CreateDirectory(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         // Directory depth level.
         int level = new Random().Next(1, 1000);

#if NET35
         string emspace = "\u3000";
         string tempPath =
            Alphaleonis.Win32.Filesystem.Path.GetTempPath("Directory.CreateDirectory()-" + level + "-" +
                                                          System.IO.Path.GetRandomFileName() + emspace);
#else
         // MSDN: .NET 4+ Trailing spaces are removed from the end of the path parameter before deleting the directory.
         string tempPath = Alphaleonis.Win32.Filesystem.Path.GetTempPath("Directory.CreateDirectory()-" + level + "-" + System.IO.Path.GetRandomFileName());
#endif

         if (isNetwork)
            tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);

         try
         {
            string root = System.IO.Path.Combine(tempPath, "MaxPath-Hit-The-Road");

            for (int i = 0; i < level; i++)
               root = System.IO.Path.Combine(root, "-" + (i + 1) + "-subdir");

            Alphaleonis.Win32.Filesystem.Directory.CreateDirectory(root);

            Console.WriteLine("\nCreated directory structure: Depth: [{0}], path length: [{1}] characters.", level,
               root.Length);
            Console.WriteLine("\n{0}", root);

            Assert.IsTrue(Alphaleonis.Win32.Filesystem.Directory.Exists(root), "Directory should exist.");
         }
         finally
         {
            if (System.IO.Directory.Exists(tempPath))
               Alphaleonis.Win32.Filesystem.Directory.Delete(tempPath, true);
            //SysIODirectory.Delete(tempPath, true);

            Assert.IsFalse(System.IO.Directory.Exists(tempPath), "Cleanup failed: Directory should have been removed.");

            Console.WriteLine("\nDirectory deleted.");
         }

         Console.WriteLine();
      }


      private void Directory_CreateDirectory_WithFileSecurity(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         string tempPath = System.IO.Path.GetTempPath();
         if (isNetwork)
            tempPath = PathUtils.AsUncPath(tempPath);


         using (var rootDir = new TemporaryDirectory(tempPath, "Directory-CreateWithFileSecurity"))
         {
            string file = rootDir.RandomFileFullPath + ".txt";
            Console.WriteLine("\nInput Directory Path: [{0}]\n", file);


            string pathExpected = rootDir.RandomFileFullPath + ".txt";
            string pathActual = rootDir.RandomFileFullPath + ".txt";

            var expectedFileSecurity = new System.Security.AccessControl.DirectorySecurity();
            expectedFileSecurity.AddAccessRule(new System.Security.AccessControl.FileSystemAccessRule(new System.Security.Principal.SecurityIdentifier(System.Security.Principal.WellKnownSidType.WorldSid, null), System.Security.AccessControl.FileSystemRights.FullControl, System.Security.AccessControl.AccessControlType.Allow));
            expectedFileSecurity.AddAuditRule(new System.Security.AccessControl.FileSystemAuditRule(new System.Security.Principal.SecurityIdentifier(System.Security.Principal.WellKnownSidType.WorldSid, null), System.Security.AccessControl.FileSystemRights.Read, System.Security.AccessControl.AuditFlags.Success));


            using (new Alphaleonis.Win32.Security.PrivilegeEnabler(Alphaleonis.Win32.Security.Privilege.Security))
            {
               var s1 = System.IO.Directory.CreateDirectory(pathExpected, expectedFileSecurity);
               var s2 = Alphaleonis.Win32.Filesystem.Directory.CreateDirectory(pathActual, expectedFileSecurity);
            }

            string expected = System.IO.File.GetAccessControl(pathExpected).GetSecurityDescriptorSddlForm(System.Security.AccessControl.AccessControlSections.All);
            string actual = Alphaleonis.Win32.Filesystem.File.GetAccessControl(pathActual).GetSecurityDescriptorSddlForm(System.Security.AccessControl.AccessControlSections.All);

            Assert.AreEqual(expected, actual);
         }

         Console.WriteLine();
      }


      private void Directory_CreateDirectory_CatchAlreadyExistsException_FileExistsWithSameNameAsDirectory(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         string tempPath = System.IO.Path.GetTempPath();
         if (isNetwork)
            tempPath = PathUtils.AsUncPath(tempPath);


         using (var rootDir = new TemporaryDirectory(tempPath, "Directory.CreateDirectory"))
         {
            string file = rootDir.RandomFileFullPath + ".txt";
            Console.WriteLine("\nInput File Path: [{0}]\n", file);

            var gotException = false;
            try
            {
               using (System.IO.File.Create(file))
               {
               }
               Alphaleonis.Win32.Filesystem.Directory.CreateDirectory(file);

            }
            catch (Exception ex)
            {
               gotException = ex.GetType().Name.Equals("AlreadyExistsException", StringComparison.OrdinalIgnoreCase);
            }
            Assert.IsTrue(gotException, "The exception was not caught, but was expected to.");
         }
      }


      private void Directory_CreateDirectory_CatchArgumentException_PathContainsInvalidCharacters(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         string tempPath = System.IO.Path.GetTempPath();
         if (isNetwork)
            tempPath = PathUtils.AsUncPath(tempPath);


         using (var rootDir = new TemporaryDirectory(tempPath, "Directory.CreateDirectory"))
         {
            string file = rootDir.RandomFileFullPath + ".txt";
            Console.WriteLine("\nInput File Path: [{0}]\n", file);

            var gotException = false;
            try
            {
               Alphaleonis.Win32.Filesystem.Directory.CreateDirectory(UnitTestConstants.SysDrive + @"\<>");

            }
            catch (Exception ex)
            {
               gotException = ex.GetType().Name.Equals("ArgumentException", StringComparison.OrdinalIgnoreCase);
            }
            Assert.IsTrue(gotException, "The exception was not caught, but was expected to.");
         }
      }


      private void Directory_CreateDirectory_CatchArgumentException_PathStartsWithColon(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         string file = @":AAAAAAAAAA";
         Console.WriteLine("\nInput File Path: [{0}]\n", file);

         var gotException = false;
         try
         {
            Alphaleonis.Win32.Filesystem.Directory.CreateDirectory(file);
         }
         catch (Exception ex)
         {
            gotException = ex.GetType().Name.Equals("ArgumentException", StringComparison.OrdinalIgnoreCase);
         }
         Assert.IsTrue(gotException, "The exception was not caught, but was expected to.");
      }


      private void Directory_CreateDirectory_CatchNotSupportedException_PathContainsColon(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         string colonText = ":aaa.txt";
         string file = (isNetwork ? PathUtils.AsUncPath(UnitTestConstants.LocalHostShare) : UnitTestConstants.SysDrive + @"\dev\test\") + colonText;

         Console.WriteLine("\nInput File Path: [{0}]\n", file);

         var gotException = false;
         try
         {
            Alphaleonis.Win32.Filesystem.Directory.CreateDirectory(file);
         }
         catch (Exception ex)
         {
            gotException = ex.GetType().Name.Equals("NotSupportedException", StringComparison.OrdinalIgnoreCase);
         }
         Assert.IsTrue(gotException, "The exception was not caught, but was expected to.");
      }


      private void Directory_CreateDirectory_CatchDirectoryNotFoundException_NonExistingDriveLetter(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         string tempPath = Alphaleonis.Win32.Filesystem.DriveInfo.GetFreeDriveLetter() + @":\NonExistingDriveLetter";
         if (isNetwork)
            tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);

         var gotException = false;
         try
         {
            Alphaleonis.Win32.Filesystem.Directory.CreateDirectory(tempPath);
         }
         catch (Exception ex)
         {
            gotException = ex.GetType() .Name.Equals(isNetwork ? "IOException" : "DirectoryNotFoundException", StringComparison.OrdinalIgnoreCase);
         }
         Assert.IsTrue(gotException, "The exception was not caught, but was expected to.");
      }
   }
}
