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

using Alphaleonis.Win32.Filesystem;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using DirectoryInfo = Alphaleonis.Win32.Filesystem.DirectoryInfo;
using Path = Alphaleonis.Win32.Filesystem.Path;

namespace AlphaFS.UnitTest
{
   /// <summary>This is a test class for DirectoryInfo and is intended to contain all DirectoryInfo Unit Tests.</summary>
   [TestClass]
   public class DirectoryInfoTest
   {
      private void DumpRefresh(bool isLocal)
      {
         #region Setup

         Console.WriteLine("\n=== TEST {0} ===", isLocal ? UnitTestConstants.Local : UnitTestConstants.Network);

         string tempPathSysIo = Path.GetTempPath("DirectoryInfo.Refresh()-directory-SysIo-" + Path.GetRandomFileName());
         string tempPath = Path.GetTempPath("DirectoryInfo.Refresh()-directory-AlphaFS-" + Path.GetRandomFileName());
         if (!isLocal) tempPathSysIo = Path.LocalToUnc(tempPathSysIo);
         if (!isLocal) tempPath = Path.LocalToUnc(tempPath);

         Console.WriteLine("\nInput Directory Path: [{0}]", tempPath);

         #endregion // Setup

         #region Refresh

         try
         {
            System.IO.DirectoryInfo diSysIo = new System.IO.DirectoryInfo(tempPathSysIo);
            DirectoryInfo di = new DirectoryInfo(tempPath);

            bool existsSysIo = diSysIo.Exists;
            bool exists = di.Exists;
            Console.WriteLine("\nnew DirectoryInfo(): Exists (Should be {0}): [{1}]", existsSysIo, exists); // false
            Assert.AreEqual(existsSysIo, exists);

            diSysIo.Create();
            di.Create();
            existsSysIo = diSysIo.Exists;
            exists = di.Exists;
            Console.WriteLine("\ndi.Create(): Exists (Should be {0}): [{1}]", existsSysIo, exists); // false
            Assert.AreEqual(existsSysIo, exists);

            diSysIo.Refresh();
            di.Refresh();
            existsSysIo = diSysIo.Exists;
            exists = di.Exists;
            Console.WriteLine("\ndi.Refresh(): Exists (Should be {0}): [{1}]", existsSysIo, exists); // true
            Assert.AreEqual(existsSysIo, exists);

            diSysIo.Delete();
            di.Delete();
            existsSysIo = diSysIo.Exists;
            exists = di.Exists;
            Console.WriteLine("\ndi.Delete(): Exists (Should be {0}): [{1}]", existsSysIo, exists); // true
            Assert.AreEqual(existsSysIo, exists);

            diSysIo.Refresh();
            di.Refresh();
            existsSysIo = diSysIo.Exists;
            exists = di.Exists;
            Console.WriteLine("\ndi.Refresh(): Exists (Should be {0}): [{1}]", existsSysIo, exists); // false
            Assert.AreEqual(existsSysIo, exists);
         }
         finally
         {
            if (Directory.Exists(tempPathSysIo))
            {
               Directory.Delete(tempPathSysIo);
               Assert.IsFalse(Directory.Exists(tempPathSysIo), "Cleanup failed: Directory should have been removed.");
            }

            if (Directory.Exists(tempPath))
            {
               Directory.Delete(tempPath);
               Assert.IsFalse(Directory.Exists(tempPath), "Cleanup failed: Directory should have been removed.");
            }

            Console.WriteLine();
         }

         #endregion // Refresh
      }



      [TestMethod]
      public void EnumerateDirectories()
      {
         Console.WriteLine("DirectoryInfo.EnumerateDirectories()");

         // Should only return folders.

         foreach (var di in new DirectoryInfo(UnitTestConstants.SysRoot).EnumerateDirectories(DirectoryEnumerationOptions.FilesAndFolders))
            Assert.IsTrue(di.IsDirectory, "Expected a folder, not a file.");

         foreach (var di in new DirectoryInfo(UnitTestConstants.SysRoot).EnumerateDirectories(DirectoryEnumerationOptions.Files))
            Assert.IsTrue(di.IsDirectory, "Expected a folder, not a file.");
      }

      
      
      [TestMethod]
      public void EnumerateFiles()
      {
         Console.WriteLine("DirectoryInfo.EnumerateFiles()");

         // Should only return files.

         foreach (var di in new DirectoryInfo(UnitTestConstants.SysRoot).EnumerateFiles(DirectoryEnumerationOptions.FilesAndFolders))
            Assert.IsTrue(!di.IsDirectory, "Expected a file, not a folder.");

         foreach (var di in new DirectoryInfo(UnitTestConstants.SysRoot).EnumerateFiles(DirectoryEnumerationOptions.Folders))
            Assert.IsTrue(!di.IsDirectory, "Expected a file, not a folder.");
      }



      [TestMethod]
      public void EnumerateFileSystemInfos()
      {
         Console.WriteLine("DirectoryInfo.EnumerateFileSystemInfos()");

         // Should only return folders.

         foreach (FileSystemInfo di in new DirectoryInfo(UnitTestConstants.SysRoot).EnumerateFileSystemInfos(DirectoryEnumerationOptions.Folders))
            Assert.IsTrue(di.IsDirectory, string.Format("Expected a folder, not a file: [{0}]", di.FullName));


         // Should only return files.

         foreach (FileSystemInfo fi in new DirectoryInfo(UnitTestConstants.SysRoot).EnumerateFileSystemInfos(DirectoryEnumerationOptions.Files))
            Assert.IsTrue(!fi.IsDirectory, string.Format("Expected a file, not a folder: [{0}]", fi.FullName));
      }



      [TestMethod]
      public void Refresh()
      {
         Console.WriteLine("DirectoryInfo.Refresh()");

         DumpRefresh(true);
         DumpRefresh(false);
      }
      


      #region SetSecurity (Issue-19788)

      private string GetTempDirectoryName()
      {
         return Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
      }

      [TestMethod]
      public void Test_AlphaFS___SetSecurity_ShouldNotProduceError()
      {
         SetSecurityAlpha(GetTempDirectoryName());
      }

      [TestMethod]
      public void Test_System___SetSecurity_ShouldNotProduceError()
      {
         SetSecuritySystem(GetTempDirectoryName());
      }

      [TestMethod]
      public void Test_System___AnalyzeSecurity_LocalAcessShouldNotExist()
      {
         string testDir = GetTempDirectoryName();
         SetSecuritySystem(testDir);
         var dirsec = new System.IO.DirectoryInfo(testDir + @"\inherited").GetAccessControl();
         AuthorizationRuleCollection accessRules = dirsec.GetAccessRules(true, true, targetType: typeof(SecurityIdentifier));
         Assert.IsFalse(HasLocalAces(accessRules), "local access rules found");
      }

      [TestMethod]
      public void Test_AlphaFS___AnalyzeSecurity_LocalAcessShouldNotExist()
      {
         string testDir = GetTempDirectoryName();
         SetSecuritySystem(testDir);
         var dirsec = new DirectoryInfo(testDir + @"\inherited").GetAccessControl();
         AuthorizationRuleCollection accessRules = dirsec.GetAccessRules(true, true, targetType: typeof(SecurityIdentifier));
         Assert.IsFalse(HasLocalAces(accessRules), "local access rules found");
      }


      public bool HasLocalAces(AuthorizationRuleCollection rules)
      {
         bool res = false;

         AccessRule locaACE = rules.Cast<AccessRule>().FirstOrDefault(a => a.IsInherited == false);
         res = (locaACE == null ? false : true);
         return res;
      }

      private void SetSecurityAlpha(string directory)
      {
         //create the test structure
         if (Directory.Exists(directory))
            Directory.Delete(directory, true);

         Directory.CreateDirectory(directory);
         Directory.CreateDirectory(System.IO.Path.Combine(directory, "inherited"));
         DirectoryInfo testDirInfo = new DirectoryInfo(directory);
         //System.IO.Directory.CreateDirectory(_testDir);
         //System.IO.Directory.CreateDirectory(System.IO.Path.Combine(_testDir, "inherited"));
         //System.IO.DirectoryInfo testDirInfo = new System.IO.DirectoryInfo(_testDir);

         var ds = testDirInfo.GetAccessControl(AccessControlSections.Access);
         ds.SetAccessRuleProtection(true, false);
         ds.AddAccessRule(new FileSystemAccessRule(
               identity: new SecurityIdentifier(WellKnownSidType.WorldSid, null),
               fileSystemRights: FileSystemRights.FullControl,
               inheritanceFlags: InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit,
               propagationFlags: PropagationFlags.None,
               type: AccessControlType.Allow
               ));

         //using (new PrivilegeEnabler(Privilege.Impersonate, Privilege.Backup, Privilege.Restore, Privilege.Security, Privilege.TakeOwnership, Privilege.TrustedCredManAccess, Privilege.Audit))
         {
            testDirInfo.SetAccessControl(ds);

         }
      }

      private void SetSecuritySystem(string directory)
      {
         //create the test structure
         if (System.IO.Directory.Exists(directory))
            System.IO.Directory.Delete(directory, true);

         System.IO.Directory.CreateDirectory(directory);
         System.IO.Directory.CreateDirectory(System.IO.Path.Combine(directory, "inherited"));
         System.IO.DirectoryInfo testDirInfo = new System.IO.DirectoryInfo(directory);

         var ds = testDirInfo.GetAccessControl(AccessControlSections.Access);
         ds.SetAccessRuleProtection(true, false);
         ds.AddAccessRule(new FileSystemAccessRule(
           identity: new SecurityIdentifier(WellKnownSidType.WorldSid, null),
           fileSystemRights: FileSystemRights.FullControl,
           inheritanceFlags: InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit,
           propagationFlags: PropagationFlags.None,
           type: AccessControlType.Allow
           ));
         testDirInfo.SetAccessControl(ds);
      }

      #endregion
   }
}
