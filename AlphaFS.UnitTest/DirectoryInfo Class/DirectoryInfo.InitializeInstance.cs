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
using System.Linq;
using System.Reflection;

namespace AlphaFS.UnitTest
{
   public partial class DirectoryInfoTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void DirectoryInfo_InitializeInstance_ExistingDirectory_LocalAndNetwork_Success()
      {
         DirectoryInfo_InitializeInstance_ExistingDirectory(false);
         DirectoryInfo_InitializeInstance_ExistingDirectory(true);
      }


      [TestMethod]
      public void DirectoryInfo_InitializeInstance_NonExistingDirectory_LocalAndNetwork_Success()
      {
         DirectoryInfo_InitializeInstance_NonExistingDirectory(false);
         DirectoryInfo_InitializeInstance_NonExistingDirectory(true);
      }


      [TestMethod]
      public void DirectoryInfo_AnalyzeSecurity_LocalAcess_ShouldNotExist()
      {
         var testDir = GetTempDirectoryName();
         SetSecuritySystem(testDir);
         var dirsec = new System.IO.DirectoryInfo(testDir + @"\inherited").GetAccessControl();
         var accessRules = dirsec.GetAccessRules(true, true, typeof(System.Security.Principal.SecurityIdentifier));
         Assert.IsFalse(HasLocalAces(accessRules), "local access rules found");


         testDir = GetTempDirectoryName();
         SetSecurityAlpha(testDir);
         dirsec = new Alphaleonis.Win32.Filesystem.DirectoryInfo(testDir + @"\inherited").GetAccessControl();
         accessRules = dirsec.GetAccessRules(true, true, typeof(System.Security.Principal.SecurityIdentifier));
         Assert.IsFalse(HasLocalAces(accessRules), "local access rules found");
      }




      private void DirectoryInfo_InitializeInstance_ExistingDirectory(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         var tempPath = System.IO.Path.GetTempPath();
         if (isNetwork)
            tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);


         using (var rootDir = new TemporaryDirectory(tempPath, MethodBase.GetCurrentMethod().Name))
         {
            var folder = rootDir.RandomDirectoryFullPath;
            var dirInfo = new System.IO.DirectoryInfo(folder);
            dirInfo.Create();

            CompareDirectoryInfos(dirInfo, new Alphaleonis.Win32.Filesystem.DirectoryInfo(folder), true);
         }

         Console.WriteLine();
      }


      private void DirectoryInfo_InitializeInstance_NonExistingDirectory(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         var tempPath = System.IO.Path.GetTempPath();
         if (isNetwork)
            tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);


         using (var rootDir = new TemporaryDirectory(tempPath, MethodBase.GetCurrentMethod().Name))
         {
            var folder = rootDir.RandomDirectoryFullPath;
            CompareDirectoryInfos(new System.IO.DirectoryInfo(folder), new Alphaleonis.Win32.Filesystem.DirectoryInfo(folder), false);
         }

         Console.WriteLine();
      }


      private void CompareDirectoryInfos(System.IO.DirectoryInfo expected, Alphaleonis.Win32.Filesystem.DirectoryInfo actual, bool exists)
      {
         if (expected == null || actual == null)
            Assert.AreEqual(expected, actual, "The two DirectoryInfo instances are not the same, but are expected to be.");

         UnitTestConstants.Dump(expected, -17);
         Console.WriteLine();
         UnitTestConstants.Dump(actual, -17);


         if (exists)
            Assert.IsTrue(expected.Exists && actual.Exists, "The directory does not exist, but is expected to.");
         else
            Assert.IsFalse(expected.Exists && actual.Exists, "The directory exists, but is expected not to.");


         var cnt = -1;
         while (cnt != 13)
         {
            cnt++;
            // Compare values of both instances.
            switch (cnt)
            {
               case 0:
                  Assert.AreEqual(expected.Attributes, actual.Attributes, "The property Attributes is not the same, but is expected to.");
                  break;
               case 1:
                  Assert.AreEqual(expected.CreationTime, actual.CreationTime, "The property CreationTime is not the same, but is expected to.");
                  break;
               case 2:
                  Assert.AreEqual(expected.CreationTimeUtc, actual.CreationTimeUtc, "The property CreationTimeUtc is not the same, but is expected to.");
                  break;
               case 3:
                  Assert.AreEqual(expected.Exists, actual.Exists, "The property Exists is not the same, but is expected to.");
                  break;
               case 4:
                  Assert.AreEqual(expected.Extension, actual.Extension, "The property Extension is not the same, but is expected to.");
                  break;
               case 5:
                  Assert.AreEqual(expected.FullName, actual.FullName, "The property FullName is not the same, but is expected to.");
                  break;
               case 6:
                  Assert.AreEqual(expected.LastAccessTime, actual.LastAccessTime, "The property LastAccessTime is not the same, but is expected to.");
                  break;
               case 7:
                  Assert.AreEqual(expected.LastAccessTimeUtc, actual.LastAccessTimeUtc, "The property LastAccessTimeUtc is not the same, but is expected to.");
                  break;
               case 8:
                  Assert.AreEqual(expected.LastWriteTime, actual.LastWriteTime, "The property LastWriteTime is not the same, but is expected to.");
                  break;
               case 9:
                  Assert.AreEqual(expected.LastWriteTimeUtc, actual.LastWriteTimeUtc, "The property LastWriteTimeUtc is not the same, but is expected to.");
                  break;
               case 10:
                  Assert.AreEqual(expected.Name, actual.Name, "The property Name is not the same, but is expected to.");
                  break;

               // Need .ToString() here since the object types are obviously not the same.
               case 11: Assert.AreEqual(expected.Parent.ToString(), actual.Parent.ToString(), "The property Parent is not the same, but is expected to.");
                  break;
               case 12: Assert.AreEqual(expected.Root.ToString(), actual.Root.ToString(), "The property Root is not the same, but is expected to.");
                  break;
            }
         }
      }




      private void SetSecurityAlpha(string directory)
      {
         if (System.IO.Directory.Exists(directory))
            System.IO.Directory.Delete(directory, true);
         System.IO.Directory.CreateDirectory(directory);
         System.IO.Directory.CreateDirectory(System.IO.Path.Combine(directory, "inherited"));


         var testDirInfo = new Alphaleonis.Win32.Filesystem.DirectoryInfo(directory);

         var ds = testDirInfo.GetAccessControl(System.Security.AccessControl.AccessControlSections.Access);
         ds.SetAccessRuleProtection(true, false);
         ds.AddAccessRule(new System.Security.AccessControl.FileSystemAccessRule(new System.Security.Principal.SecurityIdentifier(System.Security.Principal.WellKnownSidType.WorldSid, null), System.Security.AccessControl.FileSystemRights.FullControl, System.Security.AccessControl.InheritanceFlags.ContainerInherit | System.Security.AccessControl.InheritanceFlags.ObjectInherit, System.Security.AccessControl.PropagationFlags.None, System.Security.AccessControl.AccessControlType.Allow));

         testDirInfo.SetAccessControl(ds);
      }

      private void SetSecuritySystem(string directory)
      {
         if (System.IO.Directory.Exists(directory))
            System.IO.Directory.Delete(directory, true);
         System.IO.Directory.CreateDirectory(directory);
         System.IO.Directory.CreateDirectory(System.IO.Path.Combine(directory, "inherited"));


         var testDirInfo = new System.IO.DirectoryInfo(directory);

         var ds = testDirInfo.GetAccessControl(System.Security.AccessControl.AccessControlSections.Access);
         ds.SetAccessRuleProtection(true, false);
         ds.AddAccessRule(new System.Security.AccessControl.FileSystemAccessRule(new System.Security.Principal.SecurityIdentifier(System.Security.Principal.WellKnownSidType.WorldSid, null), System.Security.AccessControl.FileSystemRights.FullControl, System.Security.AccessControl.InheritanceFlags.ContainerInherit | System.Security.AccessControl.InheritanceFlags.ObjectInherit, System.Security.AccessControl.PropagationFlags.None, System.Security.AccessControl.AccessControlType.Allow));

         testDirInfo.SetAccessControl(ds);
      }
      
      private string GetTempDirectoryName()
      {
         return System.IO.Path.Combine(System.IO.Path.GetTempPath(), Guid.NewGuid().ToString("N"));
      }

      private bool HasLocalAces(System.Security.AccessControl.AuthorizationRuleCollection rules)
      {
         var localACE = rules.Cast<System.Security.AccessControl.AccessRule>().FirstOrDefault(a => !a.IsInherited);
         return localACE != null;
      }
   }
}
