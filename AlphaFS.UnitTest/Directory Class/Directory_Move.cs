﻿/*  Copyright (C) 2008-2016 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
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
         Directory_Move(false);
         Directory_Move(true);
      }


      [TestMethod]
      public void Directory_Move_FromLocalToNetwork_Success()
      {
         Directory_Move_DifferentSourceAndDestination(false);
      }


      [TestMethod]
      public void Directory_Move_FromNetworkToLocal_Success()
      {
         Directory_Move_DifferentSourceAndDestination(true);
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
      public void Directory_Move_CatchUnauthorizedAccessException_UserExplicitDeny_LocalAndNetwork_Success()
      {
         Directory_Move_CatchUnauthorizedAccessException_UserExplicitDeny(false);
         Directory_Move_CatchUnauthorizedAccessException_UserExplicitDeny(true);
      }

      

      
      private void Directory_Move(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         var tempPath = System.IO.Path.GetTempPath();
         if (isNetwork)
            tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);


         using (var rootDir = new TemporaryDirectory(tempPath, "Directory.Move"))
         {
            var folderSrc = System.IO.Directory.CreateDirectory(System.IO.Path.Combine(rootDir.Directory.FullName, "Source-") + System.IO.Path.GetRandomFileName());
            var folderDst = new System.IO.DirectoryInfo(System.IO.Path.Combine(rootDir.Directory.FullName, "Destination-") + System.IO.Path.GetRandomFileName());
            Console.WriteLine("\nSrc Directory Path: [{0}]", folderSrc.FullName);
            Console.WriteLine("Dst Directory Path: [{0}]", folderDst.FullName);

            UnitTestConstants.CreateDirectoriesAndFiles(folderSrc.FullName, new Random().Next(5, 15), true);


            var dirEnumOptions = Alphaleonis.Win32.Filesystem.DirectoryEnumerationOptions.FilesAndFolders | Alphaleonis.Win32.Filesystem.DirectoryEnumerationOptions.Recursive;
            var props = Alphaleonis.Win32.Filesystem.Directory.GetProperties(folderSrc.FullName, dirEnumOptions);
            var sourceTotal = props["Total"];
            var sourceTotalFiles = props["File"];
            var sourceTotalSize = props["Size"];
            Console.WriteLine("\n\tTotal size: [{0}] - Total Folders: [{1}] - Files: [{2}]", Alphaleonis.Utils.UnitSizeToText(sourceTotalSize), sourceTotal - sourceTotalFiles, sourceTotalFiles);

            
            Alphaleonis.Win32.Filesystem.Directory.Move(folderSrc.FullName, folderDst.FullName);


            props = Alphaleonis.Win32.Filesystem.Directory.GetProperties(folderDst.FullName, dirEnumOptions);
            Assert.AreEqual(sourceTotal, props["Total"], "The number of total file system objects do not match.");
            Assert.AreEqual(sourceTotalFiles, props["File"], "The number of total files do not match.");
            Assert.AreEqual(sourceTotalSize, props["Size"], "The total file size does not match.");


            Assert.IsFalse(System.IO.Directory.Exists(folderSrc.FullName), "The original folder exists, but is expected not to.");
         }

         Console.WriteLine();
      }


      private void Directory_Move_DifferentSourceAndDestination(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         var tempPath = System.IO.Path.GetTempPath();


         using (var rootDir = new TemporaryDirectory(tempPath, "Directory.Move"))
         {
            var src = System.IO.Path.Combine(rootDir.Directory.FullName, "Source-") + System.IO.Path.GetRandomFileName();
            var dst = System.IO.Path.Combine(rootDir.Directory.FullName, "Destination-") + System.IO.Path.GetRandomFileName();

            if (isNetwork)
               src = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(src);
            else
               dst = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(dst);


            var folderSrc = System.IO.Directory.CreateDirectory(src);
            var folderDst = new System.IO.DirectoryInfo(dst);
            Console.WriteLine("\nSrc Directory Path: [{0}]", folderSrc.FullName);
            Console.WriteLine("Dst Directory Path: [{0}]", folderDst.FullName);

            UnitTestConstants.CreateDirectoriesAndFiles(folderSrc.FullName, new Random().Next(5, 15), true);


            var dirEnumOptions = Alphaleonis.Win32.Filesystem.DirectoryEnumerationOptions.FilesAndFolders | Alphaleonis.Win32.Filesystem.DirectoryEnumerationOptions.Recursive;
            var props = Alphaleonis.Win32.Filesystem.Directory.GetProperties(folderSrc.FullName, dirEnumOptions);
            var sourceTotal = props["Total"];
            var sourceTotalFiles = props["File"];
            var sourceTotalSize = props["Size"];
            Console.WriteLine("\n\tTotal size: [{0}] - Total Folders: [{1}] - Files: [{2}]", Alphaleonis.Utils.UnitSizeToText(sourceTotalSize), sourceTotal - sourceTotalFiles, sourceTotalFiles);


            Alphaleonis.Win32.Filesystem.Directory.Move(folderSrc.FullName, folderDst.FullName, Alphaleonis.Win32.Filesystem.MoveOptions.CopyAllowed);


            props = Alphaleonis.Win32.Filesystem.Directory.GetProperties(folderDst.FullName, dirEnumOptions);
            Assert.AreEqual(sourceTotal, props["Total"], "The number of total file system objects do not match.");
            Assert.AreEqual(sourceTotalFiles, props["File"], "The number of total files do not match.");
            Assert.AreEqual(sourceTotalSize, props["Size"], "The total file size does not match.");


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


         using (var rootDir = new TemporaryDirectory(tempPath, "Directory.Move"))
         {
            var folderSrc = System.IO.Directory.CreateDirectory(System.IO.Path.Combine(rootDir.Directory.FullName, "Source-") + System.IO.Path.GetRandomFileName());
            var folderDst = new System.IO.DirectoryInfo(System.IO.Path.Combine(rootDir.Directory.FullName, "Destination-") + System.IO.Path.GetRandomFileName());
            Console.WriteLine("\nSrc Directory Path: [{0}]", folderSrc.FullName);
            Console.WriteLine("Dst Directory Path: [{0}]", folderDst.FullName);


            UnitTestConstants.CreateDirectoriesAndFiles(folderSrc.FullName, new Random().Next(5, 15), true);
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
               Console.WriteLine("\n\tCaught Exception: [{0}] Message: [{1}]", exName, ex.Message);
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


         using (var rootDir = new TemporaryDirectory(tempPath, "Directory.Move"))
         {
            var folderSrc = System.IO.Directory.CreateDirectory(System.IO.Path.Combine(rootDir.Directory.FullName, "Source-") + System.IO.Path.GetRandomFileName());
            var folderDst = System.IO.Directory.CreateDirectory(System.IO.Path.Combine(rootDir.Directory.FullName, "Destination-") + System.IO.Path.GetRandomFileName());
            Console.WriteLine("\nSrc Directory Path: [{0}]", folderSrc.FullName);
            Console.WriteLine("Dst Directory Path: [{0}]", folderDst.FullName);


            UnitTestConstants.CreateDirectoriesAndFiles(folderSrc.FullName, new Random().Next(5, 15), true);


            var gotException = false;
            try
            {
               Alphaleonis.Win32.Filesystem.Directory.Move(folderSrc.FullName, folderDst.FullName);
            }
            catch (Exception ex)
            {
               var exName = ex.GetType().Name;
               gotException = exName.Equals("AlreadyExistsException", StringComparison.OrdinalIgnoreCase);
               Console.WriteLine("\n\tCaught Exception: [{0}] Message: [{1}]", exName, ex.Message);
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
            Console.WriteLine("\n\tCaught Exception: [{0}] Message: [{1}]", exName, ex.Message);
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
            Console.WriteLine("\n\tCaught Exception: [{0}] Message: [{1}]", exName, ex.Message);
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
         

         using (var rootDir = new TemporaryDirectory(tempPath, "Directory.Move"))
         {
            var folderSrc = Alphaleonis.Win32.Filesystem.DriveInfo.GetFreeDriveLetter() + @":\NonExistingFolder";
            if (isNetwork)
               folderSrc = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(folderSrc);

            Console.WriteLine("Dst Directory Path: [{0}]", folderSrc);

            UnitTestConstants.CreateDirectoriesAndFiles(rootDir.Directory.FullName, new Random().Next(5, 15), true);


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
               Console.WriteLine("\n\tCaught Exception: [{0}] Message: [{1}]", exName, ex.Message);
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


         using (var rootDir = new TemporaryDirectory(tempPath, "Directory.Move"))
         {
            var folderSrc = System.IO.Path.Combine(rootDir.Directory.FullName, "Source-") + System.IO.Path.GetRandomFileName();
            var folderDst = System.IO.Path.Combine(rootDir.Directory.FullName, "Destination-") + System.IO.Path.GetRandomFileName();
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
               Console.WriteLine("\n\tCaught Exception: [{0}] Message: [{1}]", exName, ex.Message);
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


         using (var rootDir = new TemporaryDirectory(tempPath, "Directory_Move_CatchIOException_SameSourceAndDestination"))
         {
            var folderSrc = System.IO.Directory.CreateDirectory(System.IO.Path.Combine(rootDir.Directory.FullName, "Source-") + System.IO.Path.GetRandomFileName());
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
               Console.WriteLine("\n\tCaught Exception: [{0}] Message: [{1}]", exName, ex.Message);
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
            Console.WriteLine("\n\tCaught Exception: [{0}] Message: [{1}]", exName, ex.Message);
         }
         Assert.IsTrue(gotException, "The exception is not caught, but is expected to.");

         Console.WriteLine();
      }


      private void Directory_Move_CatchUnauthorizedAccessException_UserExplicitDeny(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         var tempPath = System.IO.Path.GetTempPath();
         if (isNetwork)
            tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);


         using (var rootDir = new TemporaryDirectory(tempPath, "Directory.Move"))
         {
            var folderSrc = rootDir.RandomFileFullPath;
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
               Alphaleonis.Win32.Filesystem.Directory.Move(UnitTestConstants.SysRoot, dirInfo.FullName, Alphaleonis.Win32.Filesystem.MoveOptions.ReplaceExisting);
            }
            catch (Exception ex)
            {
               // Local: UnauthorizedAccessException.
               // UNC: IOException.

               var exName = ex.GetType().Name;
               gotException = exName.Equals(isNetwork ? "IOException" : "UnauthorizedAccessException", StringComparison.OrdinalIgnoreCase);
               Console.WriteLine("\n\tCaught Exception: [{0}] Message: [{1}]", exName, ex.Message);
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


         using (var rootDir = new TemporaryDirectory(tempPath, "Directory.Move (Rename)"))
         {
            var folderSrc = System.IO.Directory.CreateDirectory(System.IO.Path.Combine(rootDir.Directory.FullName, "Source-") + System.IO.Path.GetRandomFileName());
            var folderDst = new System.IO.DirectoryInfo(System.IO.Path.Combine(rootDir.Directory.FullName, "Destination-") + System.IO.Path.GetRandomFileName());

            Console.WriteLine("\nInput Directory Path: [{0}]", folderSrc.FullName);
            Console.WriteLine("\nRename folder: [{0}] to: [{1}]", folderSrc.Name, folderDst.Name);


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


         using (var rootDir = new TemporaryDirectory(tempPath, "Directory.Move (Rename_DifferentCasing)"))
         {
            var folderSrc = System.IO.Directory.CreateDirectory((System.IO.Path.Combine(rootDir.Directory.FullName, "Source-") + System.IO.Path.GetRandomFileName()).ToLowerInvariant());
            var folderDst = folderSrc;
            var newFolderName = System.IO.Path.GetFileName(folderDst.FullName.ToUpperInvariant());


            Console.WriteLine("\nInput Directory Path: [{0}]", folderSrc.FullName);
            Console.WriteLine("\nRename folder: [{0}] to: [{1}]", folderSrc.Name, newFolderName);


            Assert.IsTrue(folderSrc.Exists, "The source folder does not exist which was not expected.");
            Assert.IsTrue(folderDst.Exists, "The destination folder exists which was not expected.");
            
            Alphaleonis.Win32.Filesystem.Directory.Move(folderSrc.FullName, folderDst.FullName.ToUpperInvariant());

            folderSrc.Refresh();
            folderDst.Refresh();

            Assert.IsTrue(folderSrc.Exists, "The source folder exists which was not expected.");
            Assert.IsTrue(folderDst.Exists, "The destination folder does not exists which was not expected.");
         }

         Console.WriteLine();
      }
   }
}
