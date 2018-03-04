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
   public partial class Directory_CreateDirectoryTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void Directory_CreateDirectory_And_Delete_LocalAndNetwork_Success()
      {
         Directory_CreateDirectory_And_Delete(false);
         Directory_CreateDirectory_And_Delete(true);
      }


      [TestMethod]
      public void AlphaFS_Directory_CreateDirectory_FolderWithSpaceAsName_LocalAndNetwork_Success()
      {
         Directory_CreateDirectory_FolderWithSpaceAsName(false);
         Directory_CreateDirectory_FolderWithSpaceAsName(true);
      }


      [TestMethod]
      public void Directory_CreateDirectory_WithMultipleSpacesAndSlashes_LocalAndNetwork_Success()
      {
         Directory_CreateDirectory_WithMultipleSpacesAndSlashes(false);
         Directory_CreateDirectory_WithMultipleSpacesAndSlashes(true);
      }


      [TestMethod]
      public void Directory_CreateDirectory_WithDirectorySecurity_LocalAndNetwork_Success()
      {
         if (!UnitTestConstants.IsAdmin())
            Assert.Inconclusive();


         Directory_CreateDirectory_WithDirectorySecurity(false);
         Directory_CreateDirectory_WithDirectorySecurity(true);
      }


      [TestMethod]
      public void AlphaFS_Directory_CreateDirectory_CatchAlreadyExistsException_FileExistsWithSameNameAsDirectory_LocalAndNetwork_Success()
      {
         Directory_CreateDirectory_CatchAlreadyExistsException_FileExistsWithSameNameAsDirectory(false);
         Directory_CreateDirectory_CatchAlreadyExistsException_FileExistsWithSameNameAsDirectory(true);
      }


      [TestMethod]
      public void Directory_CreateDirectory_CatchDirectoryNotFoundException_NonExistingDriveLetter_Local_Success()
      {
         Directory_CreateDirectory_CatchDirectoryNotFoundException_NonExistingDriveLetter(false);
      }


      [TestMethod]
      public void Directory_CreateDirectory_CatchDeviceNotReadyException_NonExistingDriveLetter_Network_Success()
      {
         Directory_CreateDirectory_CatchDeviceNotReadyException_NonExistingDriveLetter(true);
      }


      private void Directory_CreateDirectory_And_Delete(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);
         Console.WriteLine();


         var tempPath = UnitTestConstants.TempFolder;
         if (isNetwork)
            tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);


         using (var rootDir = new TemporaryDirectory(tempPath, MethodBase.GetCurrentMethod().Name))
         {
            var folder = rootDir.Directory.FullName;

            // Directory depth level.
            var level = new Random().Next(10, 500);

#if NET35
            // MSDN: .NET 4+ Trailing spaces are removed from the end of the path parameter before deleting the directory.
            folder += UnitTestConstants.EMspace;
#endif

            Console.WriteLine("Input Directory Path: [{0}]", folder);
            Console.WriteLine();


            var root = folder;

            for (var i = 0; i < level; i++)
            {
               var isEven = i % 2 == 0;
               root = System.IO.Path.Combine(root, (isEven ? "Level-" : "Lëvél-") + (i + 1) + (isEven ? "-subFolder" : "-sübFôldér"));
            }

            Alphaleonis.Win32.Filesystem.Directory.CreateDirectory(root);

            Console.WriteLine("\tCreated directory structure: Depth: [{0}], path length: [{1}] characters.", level.ToString(), root.Length.ToString());
            Console.WriteLine();

            Console.WriteLine("\t{0}", root);
            Console.WriteLine();

            Assert.IsTrue(Alphaleonis.Win32.Filesystem.Directory.Exists(root), "The directory does not exists, but is expected to.");


            Alphaleonis.Win32.Filesystem.Directory.Delete(folder, true);

            Assert.IsFalse(System.IO.Directory.Exists(folder), "The directory exists, but is expected not to.");
         }

         Console.WriteLine();
      }


      private void Directory_CreateDirectory_FolderWithSpaceAsName(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         var tempPath = UnitTestConstants.TempFolder;
         if (isNetwork)
            tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);


         using (var rootDir = new TemporaryDirectory(tempPath, MethodBase.GetCurrentMethod().Name))
         {
            var folder = rootDir.Directory.FullName;

            Console.WriteLine("\nInput Directory Path: [{0}]\n", folder);


            var numSpace = 1;
            var spaceFolder1 = folder + @"\" + new string(' ', numSpace);

            var dirInfo1 = Alphaleonis.Win32.Filesystem.Directory.CreateDirectory(spaceFolder1, Alphaleonis.Win32.Filesystem.PathFormat.LongFullPath);

            Assert.IsTrue(dirInfo1.Exists, "The folder with " + numSpace + " space does not exist, but is expected to.");


            numSpace = 5;
            var spaceFolder2 = folder + @"\" + new string(' ', numSpace);

            var dirInfo2 = Alphaleonis.Win32.Filesystem.Directory.CreateDirectory(spaceFolder2, Alphaleonis.Win32.Filesystem.PathFormat.LongFullPath);

            Assert.IsTrue(dirInfo2.Exists, "The folder with " + numSpace + " spaces does not exist, but is expected to.");
         }

         Console.WriteLine();
      }
      
      
      private void Directory_CreateDirectory_WithMultipleSpacesAndSlashes(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         var tempPath = UnitTestConstants.TempFolder;
         if (isNetwork)
            tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);


         using (var rootDir = new TemporaryDirectory(tempPath, MethodBase.GetCurrentMethod().Name))
         {
            var folder = rootDir.Directory.FullName;
            var subFolders = new[]
            {
               @"f�lder1",
               @"\ \",
               @"fold�r2 2",
               @"///",
               @" f�ld�r3 33"
            };


            var fullPath = folder + @"\" + subFolders[0] + subFolders[1] + subFolders[2] + subFolders[3] + subFolders[4];
            Console.WriteLine("\nInput Directory Path: [{0}]\n", fullPath);


            Alphaleonis.Win32.Filesystem.Directory.CreateDirectory(fullPath);


            var count = 0;
            foreach (var dir in Alphaleonis.Win32.Filesystem.Directory.EnumerateFileSystemEntryInfos<Alphaleonis.Win32.Filesystem.DirectoryInfo>(folder, Alphaleonis.Win32.Filesystem.DirectoryEnumerationOptions.Recursive))
            {
               Console.WriteLine("\tFolder name: \"{0}\"", dir.Name);

               switch (count)
               {
                  case 0:
                     Assert.IsTrue(dir.Name.Equals(subFolders[0]));
                     break;

                  case 1:
                     Assert.IsTrue(dir.Name.Equals(subFolders[2]));
                     break;

                  case 2:
                     Assert.IsTrue(dir.Name.Equals(subFolders[4]));
                     break;
               }

               count++;
            }
         }

         Console.WriteLine();
      }


      private void Directory_CreateDirectory_WithDirectorySecurity(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);
         Console.WriteLine();


         var tempPath = UnitTestConstants.TempFolder;
         if (isNetwork)
            tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);


         using (var rootDir = new TemporaryDirectory(tempPath, MethodBase.GetCurrentMethod().Name))
         {
            var folder = rootDir.RandomDirectoryFullPath;
            Console.WriteLine("\tInput Directory Path: [{0}]", folder);
            Console.WriteLine();


            var pathExpected = rootDir.RandomDirectoryFullPath;
            var pathActual = rootDir.RandomDirectoryFullPath;

            var sid = new System.Security.Principal.SecurityIdentifier(System.Security.Principal.WellKnownSidType.WorldSid, null);
            var expectedDirectorySecurity = new System.Security.AccessControl.DirectorySecurity();
            expectedDirectorySecurity.AddAccessRule(new System.Security.AccessControl.FileSystemAccessRule(sid, System.Security.AccessControl.FileSystemRights.FullControl, System.Security.AccessControl.AccessControlType.Allow));
            expectedDirectorySecurity.AddAuditRule(new System.Security.AccessControl.FileSystemAuditRule(sid, System.Security.AccessControl.FileSystemRights.Read, System.Security.AccessControl.AuditFlags.Success));


            using (new Alphaleonis.Win32.Security.PrivilegeEnabler(Alphaleonis.Win32.Security.Privilege.Security))
            {
               var s1 = System.IO.Directory.CreateDirectory(pathExpected, expectedDirectorySecurity);
               var s2 = Alphaleonis.Win32.Filesystem.Directory.CreateDirectory(pathActual, expectedDirectorySecurity);


               var expected = s1.GetAccessControl().GetSecurityDescriptorSddlForm(System.Security.AccessControl.AccessControlSections.All);
               var actual = s2.GetAccessControl().GetSecurityDescriptorSddlForm(System.Security.AccessControl.AccessControlSections.All);


               Console.WriteLine("\tSystem.IO: {0}", expected);
               Console.WriteLine("\tAlphaFS  : {0}", actual);


               Assert.AreEqual(expected, actual);
            }
         }

         Console.WriteLine();
      }


      private void Directory_CreateDirectory_CatchAlreadyExistsException_FileExistsWithSameNameAsDirectory(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         var tempPath = UnitTestConstants.TempFolder;
         if (isNetwork)
            tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);


         using (var rootDir = new TemporaryDirectory(tempPath, MethodBase.GetCurrentMethod().Name))
         {
            var file = rootDir.RandomFileFullPath;
            Console.WriteLine("\nInput File Path: [{0}]", file);

            using (System.IO.File.Create(file)) { }


            var gotException = false;
            try
            {
               Alphaleonis.Win32.Filesystem.Directory.CreateDirectory(file);

            }
            catch (Exception ex)
            {
               var exName = ex.GetType().Name;
               gotException = exName.Equals("AlreadyExistsException", StringComparison.OrdinalIgnoreCase);
               Console.WriteLine("\n\tCaught {0} Exception: [{1}] {2}", gotException ? "EXPECTED" : "UNEXPECTED", exName, ex.Message);
            }
            Assert.IsTrue(gotException, "The exception is not caught, but is expected to.");
         }

         Console.WriteLine();
      }


      private void Directory_CreateDirectory_CatchDirectoryNotFoundException_NonExistingDriveLetter(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         var folder = Alphaleonis.Win32.Filesystem.DriveInfo.GetFreeDriveLetter() + @":\NonExistingDriveLetter";
         if (isNetwork)
            folder = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(folder);

         Console.WriteLine("\nInput Directory Path: [{0}]", folder);


         var gotException = false;
         try
         {
            Alphaleonis.Win32.Filesystem.Directory.CreateDirectory(folder);
         }
         catch (Exception ex)
         {
            var exType = ex.GetType();

            // Local: DirectoryNotFoundException.
            // UNC: DeviceNotReadyException.

            gotException = exType == typeof(System.IO.DirectoryNotFoundException);

            Console.WriteLine("\n\tCaught {0} Exception: [{1}] {2}", gotException ? "EXPECTED" : "UNEXPECTED", exType.Name, ex.Message);
         }
         
         Assert.IsTrue(gotException, "The exception is not caught, but is expected to.");

         Console.WriteLine();
      }


      private void Directory_CreateDirectory_CatchDeviceNotReadyException_NonExistingDriveLetter(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         var folder = Alphaleonis.Win32.Filesystem.DriveInfo.GetFreeDriveLetter() + @":\NonExistingDriveLetter";
         if (isNetwork)
            folder = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(folder);

         Console.WriteLine("\nInput Directory Path: [{0}]", folder);


         var gotException = false;
         try
         {
            Alphaleonis.Win32.Filesystem.Directory.CreateDirectory(folder);
         }
         catch (Exception ex)
         {
            var exType = ex.GetType();

            // Local: DirectoryNotFoundException.
            // UNC: DeviceNotReadyException.

            gotException = exType == typeof(Alphaleonis.Win32.Filesystem.DeviceNotReadyException);

            Console.WriteLine("\n\tCaught {0} Exception: [{1}] {2}", gotException ? "EXPECTED" : "UNEXPECTED", exType.Name, ex.Message);
         }

         Assert.IsTrue(gotException, "The exception is not caught, but is expected to.");

         Console.WriteLine();
      }
   }
}
