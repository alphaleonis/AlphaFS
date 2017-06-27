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
      public void Directory_Move_ToDifferentVolume_EmulateUsingCopyDelete_LocalAndNetwork_Success()
      {
         Directory_Move_ToDifferentVolume_EmulateUsingCopyDelete(false);
         Directory_Move_ToDifferentVolume_EmulateUsingCopyDelete(true);
      }


      [TestMethod]
      public void Directory_Move_Rename_LocalAndNetwork_Success()
      {
         Directory_Move_Rename(false);
         Directory_Move_Rename(true);
      }


      [TestMethod]
      public void Directory_Move_Rename_DifferentCasing_LocalAndNetwork_Success()
      {
         Directory_Move_Rename_DifferentCasing(false);
         Directory_Move_Rename_DifferentCasing(true);
      }


      [TestMethod]
      public void Directory_Move_SameVolume_LocalAndNetwork_Success()
      {
         Directory_Move_SameVolume(false);
         Directory_Move_SameVolume(true);
      }


      [TestMethod]
      public void Directory_Move_Overwrite_DestinationFileAlreadyExists_LocalAndNetwork_Success()
      {
         Directory_Move_Overwrite_DestinationFileAlreadyExists(false);
         Directory_Move_Overwrite_DestinationFileAlreadyExists(true);
      }


      [TestMethod]
      public void Directory_Move_CatchAlreadyExistsException_DestinationFileAlreadyExists_LocalAndNetwork_Success()
      {
         Directory_Move_CatchAlreadyExistsException_DestinationFileAlreadyExists(false);
         Directory_Move_CatchAlreadyExistsException_DestinationFileAlreadyExists(true);
      }


      [TestMethod]
      public void Directory_Move_CatchArgumentException_PathContainsInvalidCharacters_LocalAndNetwork_Success()
      {
         Directory_Move_CatchArgumentException_PathContainsInvalidCharacters(false);
         Directory_Move_CatchArgumentException_PathContainsInvalidCharacters(true);
      }


      [TestMethod]
      public void Directory_Move_CatchArgumentException_PathStartsWithColon_Local_Success()
      {
         Directory_Move_CatchArgumentException_PathStartsWithColon(false);
      }


      [TestMethod]
      public void Directory_Move_CatchDirectoryNotFoundException_NonExistingDriveLetter_LocalAndNetwork_Success()
      {
         Directory_Move_CatchDirectoryNotFoundException_NonExistingDriveLetter(false);
         Directory_Move_CatchDirectoryNotFoundException_NonExistingDriveLetter(true);
      }


      [TestMethod]
      public void Directory_Move_CatchDirectoryNotFoundException_NonExistingDirectory_LocalAndNetwork_Success()
      {
         Directory_Move_CatchDirectoryNotFoundException_NonExistingDirectory(false);
         Directory_Move_CatchDirectoryNotFoundException_NonExistingDirectory(true);
      }


      [TestMethod]
      public void Directory_Move_CatchIOException_SameSourceAndDestination_LocalAndNetwork_Success()
      {
         Directory_Move_CatchIOException_SameSourceAndDestination(false);
         Directory_Move_CatchIOException_SameSourceAndDestination(true);
      }


      [TestMethod]
      public void Directory_Move_CatchNotSupportedException_PathContainsColon_LocalAndNetwork_Success()
      {
         Directory_Move_CatchNotSupportedException_PathContainsColon(false);
         Directory_Move_CatchNotSupportedException_PathContainsColon(true);
      }


      [TestMethod]
      public void Directory_Move_CatchUnauthorizedAccessException_UserExplicitDeny_Local_Success()
      {
         Directory_Move_CatchUnauthorizedAccessException_And_NotSameDeviceException_UserExplicitDeny(false);
      }



      [TestMethod]
      public void Directory_Move_CatchNotSameDeviceException_UserExplicitDeny_Network_Success()
      {
         Directory_Move_CatchUnauthorizedAccessException_And_NotSameDeviceException_UserExplicitDeny(true);
      }




      private void Directory_Move_ToDifferentVolume_EmulateUsingCopyDelete(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         var tempPath = System.IO.Path.GetTempPath();


         using (var rootDir = new TemporaryDirectory(tempPath, MethodBase.GetCurrentMethod().Name))
         {
            var random = System.IO.Path.GetRandomFileName();
            var srcFolderName = System.IO.Path.Combine(rootDir.Directory.FullName, "Source-") + random;
            var destFolderName = System.IO.Path.Combine(rootDir.Directory.FullName, "Destination-") + random;


            var folderSrc = isNetwork
               ? System.IO.Directory.CreateDirectory(Alphaleonis.Win32.Filesystem.Path.LocalToUnc(srcFolderName))
               : System.IO.Directory.CreateDirectory(System.IO.Path.Combine(srcFolderName));

            var folderDst = !isNetwork
               ? new System.IO.DirectoryInfo(Alphaleonis.Win32.Filesystem.Path.LocalToUnc(destFolderName))
               : new System.IO.DirectoryInfo(destFolderName);




            Console.WriteLine("\nSrc Directory Path: [{0}]", folderSrc.FullName);
            Console.WriteLine("Dst Directory Path: [{0}]", folderDst.FullName);

            UnitTestConstants.CreateDirectoriesAndFiles(folderSrc.FullName, new Random().Next(5, 15), false, false, true);


            var dirEnumOptions = Alphaleonis.Win32.Filesystem.DirectoryEnumerationOptions.FilesAndFolders | Alphaleonis.Win32.Filesystem.DirectoryEnumerationOptions.Recursive;
            var props = Alphaleonis.Win32.Filesystem.Directory.GetProperties(folderSrc.FullName, dirEnumOptions);
            var sourceTotal = props["Total"];
            var sourceTotalFiles = props["File"];
            var sourceTotalSize = props["Size"];
            Console.WriteLine("\n\tTotal size: [{0}] - Total Folders: [{1}] - Files: [{2}]", Alphaleonis.Utils.UnitSizeToText(sourceTotalSize), sourceTotal - sourceTotalFiles, sourceTotalFiles);


            var moveResult = Alphaleonis.Win32.Filesystem.Directory.Move(folderSrc.FullName, folderDst.FullName, Alphaleonis.Win32.Filesystem.MoveOptions.CopyAllowed);

            UnitTestConstants.Dump(moveResult, -16);

            props = Alphaleonis.Win32.Filesystem.Directory.GetProperties(folderDst.FullName, dirEnumOptions);
            Assert.AreEqual(sourceTotal, props["Total"], "The number of total file system objects do not match.");
            Assert.AreEqual(sourceTotalFiles, props["File"], "The number of total files do not match.");
            Assert.AreEqual(sourceTotalSize, props["Size"], "The total file size does not match.");


            // Test against moveResult results.

            var isMove = moveResult.IsMove;

            Assert.AreEqual(sourceTotal, moveResult.TotalFolders + moveResult.TotalFiles, "The number of total file system objects do not match.");
            Assert.AreEqual(sourceTotalFiles, moveResult.TotalFiles, "The number of total files do not match.");
            Assert.AreEqual(sourceTotalSize, moveResult.TotalBytes, "The total file size does not match.");
            Assert.IsFalse(isMove, "The action was expected to be a Copy, not a Move.");
            Assert.IsTrue(moveResult.EmulatedMove, "The action was expected to be emulated (Copy + Delete).");

            Assert.IsFalse(System.IO.Directory.Exists(folderSrc.FullName), "The original folder exists, but is expected not to.");
         }

         Console.WriteLine();
      }


      private void Directory_Move_SameVolume(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         var tempPath = System.IO.Path.GetTempPath();
         if (isNetwork)
            tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);


         using (var rootDir = new TemporaryDirectory(tempPath, MethodBase.GetCurrentMethod().Name))
         {
            var folderSrc = System.IO.Directory.CreateDirectory(System.IO.Path.Combine(rootDir.Directory.FullName, "Source Folder"));
            var folderDst = new System.IO.DirectoryInfo(System.IO.Path.Combine(rootDir.Directory.FullName, "Destination Folder"));
            Console.WriteLine("\nSrc Directory Path: [{0}]", folderSrc.FullName);
            Console.WriteLine("Dst Directory Path: [{0}]", folderDst.FullName);

            UnitTestConstants.CreateDirectoriesAndFiles(folderSrc.FullName, new Random().Next(5, 15), false, false, true);


            var dirEnumOptions = Alphaleonis.Win32.Filesystem.DirectoryEnumerationOptions.FilesAndFolders | Alphaleonis.Win32.Filesystem.DirectoryEnumerationOptions.Recursive;
            var props = Alphaleonis.Win32.Filesystem.Directory.GetProperties(folderSrc.FullName, dirEnumOptions);
            var sourceTotal = props["Total"];
            var sourceTotalFiles = props["File"];
            var sourceTotalSize = props["Size"];
            Console.WriteLine("\n\tTotal size: [{0}] - Total Folders: [{1}] - Files: [{2}]", Alphaleonis.Utils.UnitSizeToText(sourceTotalSize), sourceTotal - sourceTotalFiles, sourceTotalFiles);


            var moveResult = Alphaleonis.Win32.Filesystem.Directory.Move(folderSrc.FullName, folderDst.FullName, Alphaleonis.Win32.Filesystem.PathFormat.FullPath);

            UnitTestConstants.Dump(moveResult, -16);

            props = Alphaleonis.Win32.Filesystem.Directory.GetProperties(folderDst.FullName, dirEnumOptions);
            Assert.AreEqual(sourceTotal, props["Total"], "The number of total file system objects do not match.");
            Assert.AreEqual(sourceTotalFiles, props["File"], "The number of total files do not match.");
            Assert.AreEqual(sourceTotalSize, props["Size"], "The total file size does not match.");


            // Test against moveResult results.

            var isMove = moveResult.IsMove;

            Assert.AreEqual(isMove ? 1 : sourceTotal, moveResult.TotalFolders + moveResult.TotalFiles, "The number of total file system objects do not match.");
            Assert.AreEqual(isMove ? 0 : sourceTotalFiles, moveResult.TotalFiles, "The number of total files do not match.");
            Assert.AreEqual(isMove ? 0 : sourceTotalSize, moveResult.TotalBytes, "The total file size does not match.");
            Assert.IsTrue(isMove, "The action was expected to be a Move, not a Copy.");
            Assert.IsFalse(moveResult.IsCopy);
            Assert.IsTrue(moveResult.IsMove);
            Assert.IsTrue(moveResult.IsDirectory);
            Assert.IsFalse(moveResult.IsFile);

            Assert.IsFalse(System.IO.Directory.Exists(folderSrc.FullName), "The original folder exists, but is expected not to.");
         }

         Console.WriteLine();
      }


      private void Directory_Move_Overwrite_DestinationFileAlreadyExists(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         var tempPath = System.IO.Path.GetTempPath();
         if (isNetwork)
            tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);


         using (var rootDir = new TemporaryDirectory(tempPath, MethodBase.GetCurrentMethod().Name))
         {
            var folderSrc = System.IO.Directory.CreateDirectory(System.IO.Path.Combine(rootDir.Directory.FullName, "Source Folder"));
            var folderDst = new System.IO.DirectoryInfo(System.IO.Path.Combine(rootDir.Directory.FullName, "Destination Folder"));
            Console.WriteLine("\nSrc Directory Path: [{0}]", folderSrc.FullName);
            Console.WriteLine("Dst Directory Path: [{0}]", folderDst.FullName);


            UnitTestConstants.CreateDirectoriesAndFiles(folderSrc.FullName, new Random().Next(5, 15), false, false, true);
            Alphaleonis.Win32.Filesystem.Directory.Copy(folderSrc.FullName, folderDst.FullName);


            var gotException = false;
            try
            {
               Alphaleonis.Win32.Filesystem.Directory.Move(folderSrc.FullName, folderDst.FullName);
            }
            catch (Exception ex)
            {
               var exName = ex.GetType().Name;
               gotException = exName.Equals("AlreadyExistsException", StringComparison.OrdinalIgnoreCase);
               Console.WriteLine("\n\tCaught {0} Exception: [{1}] {2}", gotException ? "EXPECTED" : "UNEXPECTED", exName, ex.Message);
            }
            Assert.IsTrue(gotException, "The exception is not caught, but is expected to.");


            // Overwrite using MoveOptions.ReplaceExisting

            var dirEnumOptions = Alphaleonis.Win32.Filesystem.DirectoryEnumerationOptions.FilesAndFolders | Alphaleonis.Win32.Filesystem.DirectoryEnumerationOptions.Recursive;
            var props = Alphaleonis.Win32.Filesystem.Directory.GetProperties(folderSrc.FullName, dirEnumOptions);
            var sourceTotal = props["Total"];
            var sourceTotalFiles = props["File"];
            var sourceTotalSize = props["Size"];
            Console.WriteLine("\n\tTotal size: [{0}] - Total Folders: [{1}] - Files: [{2}]", Alphaleonis.Utils.UnitSizeToText(sourceTotalSize), sourceTotal - sourceTotalFiles, sourceTotalFiles);


            Alphaleonis.Win32.Filesystem.Directory.Move(folderSrc.FullName, folderDst.FullName, Alphaleonis.Win32.Filesystem.MoveOptions.ReplaceExisting);


            props = Alphaleonis.Win32.Filesystem.Directory.GetProperties(folderDst.FullName, dirEnumOptions);
            Assert.AreEqual(sourceTotal, props["Total"], "The number of total file system objects do not match.");
            Assert.AreEqual(sourceTotalFiles, props["File"], "The number of total files do not match.");
            Assert.AreEqual(sourceTotalSize, props["Size"], "The total file size does not match.");
         }

         Console.WriteLine();
      }


      private void Directory_Move_CatchAlreadyExistsException_DestinationFileAlreadyExists(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         var tempPath = System.IO.Path.GetTempPath();
         if (isNetwork)
            tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);


         using (var rootDir = new TemporaryDirectory(tempPath, MethodBase.GetCurrentMethod().Name))
         {
            var folderSrc = System.IO.Directory.CreateDirectory(System.IO.Path.Combine(rootDir.Directory.FullName, "Source Folder"));
            var folderDst = System.IO.Directory.CreateDirectory(System.IO.Path.Combine(rootDir.Directory.FullName, "Destination Folder"));
            Console.WriteLine("\nSrc Directory Path: [{0}]", folderSrc.FullName);
            Console.WriteLine("Dst Directory Path: [{0}]", folderDst.FullName);


            UnitTestConstants.CreateDirectoriesAndFiles(folderSrc.FullName, new Random().Next(5, 15), false, false, true);


            var gotException = false;
            try
            {
               Alphaleonis.Win32.Filesystem.Directory.Move(folderSrc.FullName, folderDst.FullName);
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


      private void Directory_Move_CatchArgumentException_PathContainsInvalidCharacters(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         var folderSrc = System.IO.Path.GetTempPath() + @"ThisIs<My>Folder";
         Console.WriteLine("\nSrc Directory Path: [{0}]", folderSrc);


         var gotException = false;
         try
         {
            Alphaleonis.Win32.Filesystem.Directory.Move(folderSrc, string.Empty);
         }
         catch (Exception ex)
         {
            var exName = ex.GetType().Name;
            gotException = exName.Equals("ArgumentException", StringComparison.OrdinalIgnoreCase);
            Console.WriteLine("\n\tCaught {0} Exception: [{1}] {2}", gotException ? "EXPECTED" : "UNEXPECTED", exName, ex.Message);
         }
         Assert.IsTrue(gotException, "The exception is not caught, but is expected to.");

         Console.WriteLine();
      }


      private void Directory_Move_CatchArgumentException_PathStartsWithColon(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         var folderSrc = @":AAAAAAAAAA";
         Console.WriteLine("\nSrc Directory Path: [{0}]", folderSrc);


         var gotException = false;
         try
         {
            Alphaleonis.Win32.Filesystem.Directory.Move(folderSrc, string.Empty);
         }
         catch (Exception ex)
         {
            var exName = ex.GetType().Name;
            gotException = exName.Equals("ArgumentException", StringComparison.OrdinalIgnoreCase);
            Console.WriteLine("\n\tCaught {0} Exception: [{1}] {2}", gotException ? "EXPECTED" : "UNEXPECTED", exName, ex.Message);
         }
         Assert.IsTrue(gotException, "The exception is not caught, but is expected to.");

         Console.WriteLine();
      }


      private void Directory_Move_CatchDirectoryNotFoundException_NonExistingDriveLetter(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         var tempPath = System.IO.Path.GetTempPath();
         if (isNetwork)
            tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);


         using (var rootDir = new TemporaryDirectory(tempPath, MethodBase.GetCurrentMethod().Name))
         {
            var folderSrc = Alphaleonis.Win32.Filesystem.DriveInfo.GetFreeDriveLetter() + @":\NonExistingFolder";
            if (isNetwork)
               folderSrc = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(folderSrc);

            Console.WriteLine("Dst Directory Path: [{0}]", folderSrc);

            UnitTestConstants.CreateDirectoriesAndFiles(rootDir.Directory.FullName, new Random().Next(5, 15), false, false, true);


            var gotException = false;
            try
            {
               Alphaleonis.Win32.Filesystem.Directory.Move(rootDir.Directory.FullName, folderSrc, Alphaleonis.Win32.Filesystem.MoveOptions.CopyAllowed);
            }
            catch (Exception ex)
            {
               // Local: DirectoryNotFoundException.
               // UNC: IOException.

               var exName = ex.GetType().Name;
               gotException = exName.Equals(isNetwork ? "IOException" : "DirectoryNotFoundException", StringComparison.OrdinalIgnoreCase);
               Console.WriteLine("\n\tCaught {0} Exception: [{1}] {2}", gotException ? "EXPECTED" : "UNEXPECTED", exName, ex.Message);
            }
            Assert.IsTrue(gotException, "The exception is not caught, but is expected to.");
         }

         Console.WriteLine();
      }


      private void Directory_Move_CatchDirectoryNotFoundException_NonExistingDirectory(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         var tempPath = System.IO.Path.GetTempPath();
         if (isNetwork)
            tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);


         using (var rootDir = new TemporaryDirectory(tempPath, MethodBase.GetCurrentMethod().Name))
         {
            var folderSrc = System.IO.Path.Combine(rootDir.Directory.FullName, "Source");
            var folderDst = System.IO.Path.Combine(rootDir.Directory.FullName, "Destination");
            Console.WriteLine("\nSrc Directory Path: [{0}]", folderSrc);
            Console.WriteLine("Dst Directory Path: [{0}]", folderDst);


            var gotException = false;
            try
            {
               Alphaleonis.Win32.Filesystem.Directory.Move(folderSrc, folderDst);
            }
            catch (Exception ex)
            {
               var exName = ex.GetType().Name;
               gotException = exName.Equals("DirectoryNotFoundException", StringComparison.OrdinalIgnoreCase);
               Console.WriteLine("\n\tCaught {0} Exception: [{1}] {2}", gotException ? "EXPECTED" : "UNEXPECTED", exName, ex.Message);
            }
            Assert.IsTrue(gotException, "The exception is not caught, but is expected to.");
         }

         Console.WriteLine();
      }


      private void Directory_Move_CatchIOException_SameSourceAndDestination(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         var tempPath = System.IO.Path.GetTempPath();
         if (isNetwork)
            tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);


         using (var rootDir = new TemporaryDirectory(tempPath, MethodBase.GetCurrentMethod().Name))
         {
            var folderSrc = System.IO.Directory.CreateDirectory(System.IO.Path.Combine(rootDir.Directory.FullName, "Source Folder"));
            var folderDst = folderSrc;


            Console.WriteLine("\nSrc Directory Path: [{0}]", folderSrc.FullName);
            Console.WriteLine("Dst Directory Path: [{0}]", folderDst.FullName);


            var gotException = false;
            try
            {
               System.IO.Directory.Move(folderSrc.FullName, folderDst.FullName);
            }
            catch (Exception ex)
            {
               var exName = ex.GetType().Name;
               gotException = exName.Equals("IOException", StringComparison.OrdinalIgnoreCase);
               Console.WriteLine("\n\tCaught {0} Exception: [{1}] {2}", gotException ? "EXPECTED" : "UNEXPECTED", exName, ex.Message);
            }
            Assert.IsTrue(gotException, "The exception is not caught, but is expected to.");
         }

         Console.WriteLine();
      }


      private void Directory_Move_CatchNotSupportedException_PathContainsColon(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         var colonText = @"\My:FilePath";
         var folderSrc = (isNetwork ? Alphaleonis.Win32.Filesystem.Path.LocalToUnc(UnitTestConstants.LocalHostShare) : UnitTestConstants.SysDrive + @"\dev\test") + colonText;

         Console.WriteLine("\nSrc Directory Path: [{0}]", folderSrc);


         var gotException = false;
         try
         {
            Alphaleonis.Win32.Filesystem.Directory.Move(folderSrc, string.Empty);
         }
         catch (Exception ex)
         {
            var exName = ex.GetType().Name;
            gotException = exName.Equals("NotSupportedException", StringComparison.OrdinalIgnoreCase);
            Console.WriteLine("\n\tCaught {0} Exception: [{1}] {2}", gotException ? "EXPECTED" : "UNEXPECTED", exName, ex.Message);
         }
         Assert.IsTrue(gotException, "The exception is not caught, but is expected to.");

         Console.WriteLine();
      }


      private void Directory_Move_CatchUnauthorizedAccessException_And_NotSameDeviceException_UserExplicitDeny(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         var tempPath = System.IO.Path.GetTempPath();
         if (isNetwork)
            tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);


         using (var rootDir = new TemporaryDirectory(tempPath, MethodBase.GetCurrentMethod().Name))
         {
            var folderSrc = rootDir.RandomDirectoryFullPath;
            Console.WriteLine("\nSrc Directory Path: [{0}]", folderSrc);


            // ╔═════════════╦═════════════╦═══════════════════════════════╦════════════════════════╦══════════════════╦═══════════════════════╦═════════════╦═════════════╗
            // ║             ║ folder only ║ folder, sub-folders and files ║ folder and sub-folders ║ folder and files ║ sub-folders and files ║ sub-folders ║    files    ║
            // ╠═════════════╬═════════════╬═══════════════════════════════╬════════════════════════╬══════════════════╬═══════════════════════╬═════════════╬═════════════╣
            // ║ Propagation ║ none        ║ none                          ║ none                   ║ none             ║ InheritOnly           ║ InheritOnly ║ InheritOnly ║
            // ║ Inheritance ║ none        ║ Container|Object              ║ Container              ║ Object           ║ Container|Object      ║ Container   ║ Object      ║
            // ╚═════════════╩═════════════╩═══════════════════════════════╩════════════════════════╩══════════════════╩═══════════════════════╩═════════════╩═════════════╝


            var user = (Environment.UserDomainName + @"\" + Environment.UserName).TrimStart('\\');
            var rule = new System.Security.AccessControl.FileSystemAccessRule(user, System.Security.AccessControl.FileSystemRights.FullControl, System.Security.AccessControl.InheritanceFlags.ContainerInherit | System.Security.AccessControl.InheritanceFlags.ObjectInherit, System.Security.AccessControl.PropagationFlags.None, System.Security.AccessControl.AccessControlType.Deny);

            var dirInfo = System.IO.Directory.CreateDirectory(folderSrc);

            // Set DENY for current user.
            var dirSecurity = dirInfo.GetAccessControl();
            dirSecurity.AddAccessRule(rule);
            dirInfo.SetAccessControl(dirSecurity);


            var gotException = false;
            try
            {
               Alphaleonis.Win32.Filesystem.Directory.Move(UnitTestConstants.SysDrive + @"\NonExistingFolder", dirInfo.FullName, Alphaleonis.Win32.Filesystem.MoveOptions.ReplaceExisting);
            }
            catch (Exception ex)
            {
               // Local: UnauthorizedAccessException.
               // UNC: NotSameDeviceException.

               var exName = ex.GetType().Name;
               gotException = exName.Equals(isNetwork ? "NotSameDeviceException" : "UnauthorizedAccessException", StringComparison.OrdinalIgnoreCase);
               Console.WriteLine("\n\tCaught {0} Exception: [{1}] {2}", gotException ? "EXPECTED" : "UNEXPECTED", exName, ex.Message);
            }
            Assert.IsTrue(gotException, "The exception is not caught, but is expected to.");


            // Remove DENY for current user.
            dirSecurity = dirInfo.GetAccessControl();
            dirSecurity.RemoveAccessRule(rule);
            dirInfo.SetAccessControl(dirSecurity);
         }

         Console.WriteLine();
      }


      private void Directory_Move_Rename(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         var tempPath = System.IO.Path.GetTempPath();
         if (isNetwork)
            tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);


         using (var rootDir = new TemporaryDirectory(tempPath, MethodBase.GetCurrentMethod().Name))
         {
            var folderSrc = System.IO.Directory.CreateDirectory(System.IO.Path.Combine(rootDir.Directory.FullName, "Source Folder"));
            var folderDst = new System.IO.DirectoryInfo(System.IO.Path.Combine(rootDir.Directory.FullName, "Destination Folder"));

            Console.WriteLine("\nInput Directory Path: [{0}]", folderSrc.FullName);
            Console.WriteLine("\n\tRename folder: [{0}] to: [{1}]", folderSrc.Name, folderDst.Name);


            Assert.IsTrue(folderSrc.Exists, "The source folder does not exist which was not expected.");
            Assert.IsFalse(folderDst.Exists, "The destination folder exists which was not expected.");

            Alphaleonis.Win32.Filesystem.Directory.Move(folderSrc.FullName, folderDst.FullName);

            folderSrc.Refresh();
            folderDst.Refresh();

            Assert.IsFalse(folderSrc.Exists, "The source folder exists which was not expected.");
            Assert.IsTrue(folderDst.Exists, "The destination folder does not exists which was not expected.");
         }

         Console.WriteLine();
      }


      private void Directory_Move_Rename_DifferentCasing(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         var tempPath = System.IO.Path.GetTempPath();
         if (isNetwork)
            tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);


         using (var rootDir = new TemporaryDirectory(tempPath, MethodBase.GetCurrentMethod().Name))
         {
            var folderSrc = System.IO.Directory.CreateDirectory(System.IO.Path.Combine(rootDir.Directory.FullName, "Source"));
            var upperCaseFolderName = System.IO.Path.GetFileName(folderSrc.FullName).ToUpperInvariant();


            Console.WriteLine("\nInput Directory Path: [{0}]", folderSrc.FullName);
            Console.WriteLine("\n\tRename folder: [{0}] to: [{1}]", folderSrc.Name, upperCaseFolderName);


            Assert.IsTrue(folderSrc.Exists, "The source folder does not exist which is not expected.");

            var destFolder = System.IO.Path.Combine(folderSrc.Parent.FullName, upperCaseFolderName);
            Alphaleonis.Win32.Filesystem.Directory.Move(folderSrc.FullName, destFolder);

            var upperDirInfo = new System.IO.DirectoryInfo(destFolder);
            UnitTestConstants.Dump(upperDirInfo, -17);


            Assert.AreEqual(upperCaseFolderName, upperDirInfo.Name, "The source folder name is expected to be in uppercase, but it is not.");
         }

         Console.WriteLine();
      }
   }
}
