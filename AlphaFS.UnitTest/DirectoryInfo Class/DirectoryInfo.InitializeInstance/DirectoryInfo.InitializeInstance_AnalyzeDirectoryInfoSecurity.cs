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

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace AlphaFS.UnitTest
{
   public partial class DirectoryInfoTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void AnalyzeDirectoryInfoSecurity_ShouldNotExist_LocalAndNetwork()
      {
         AnalyzeDirectoryInfoSecurity_ShouldNotExist(false);
         AnalyzeDirectoryInfoSecurity_ShouldNotExist(true);
      }


      private void AnalyzeDirectoryInfoSecurity_ShouldNotExist(bool isNetwork)
      {
         using (var tempRoot = new TemporaryDirectory(isNetwork))
         {
            var folder = tempRoot.CreateDirectory();

            Console.WriteLine("Input Directory Path: [{0}]", folder.FullName);


            SetSecuritySystem(folder.FullName);

            var dirsec = new System.IO.DirectoryInfo(folder.FullName + @"\inherited").GetAccessControl();

            var accessRules = dirsec.GetAccessRules(true, true, typeof(System.Security.Principal.SecurityIdentifier));

            UnitTestConstants.Dump(dirsec);

            Assert.IsFalse(HasLocalAces(accessRules), "Local access rules are found, but it is not expected.");

            


            folder = tempRoot.CreateDirectory();

            Console.WriteLine("\nInput Directory Path: [{0}]", folder.FullName);

            SetSecurityAlpha(folder.FullName);

            dirsec = new Alphaleonis.Win32.Filesystem.DirectoryInfo(folder.FullName + @"\inherited").GetAccessControl();

            accessRules = dirsec.GetAccessRules(true, true, typeof(System.Security.Principal.SecurityIdentifier));

            UnitTestConstants.Dump(dirsec);

            Assert.IsFalse(HasLocalAces(accessRules), "Local access rules are found, but it is not expected.");
         }

         Console.WriteLine();
      }

      
      private static void SetSecurityAlpha(string directory)
      {
         if (System.IO.Directory.Exists(directory))
            System.IO.Directory.Delete(directory, true);

         Alphaleonis.Win32.Filesystem.Directory.CreateDirectory(directory);
         Alphaleonis.Win32.Filesystem.Directory.CreateDirectory(System.IO.Path.Combine(directory, "inherited"));


         var testDirInfo = new Alphaleonis.Win32.Filesystem.DirectoryInfo(directory);

         var ds = testDirInfo.GetAccessControl(System.Security.AccessControl.AccessControlSections.Access);

         ds.SetAccessRuleProtection(true, false);

         ds.AddAccessRule(new System.Security.AccessControl.FileSystemAccessRule(new System.Security.Principal.SecurityIdentifier(System.Security.Principal.WellKnownSidType.WorldSid, null), System.Security.AccessControl.FileSystemRights.FullControl, System.Security.AccessControl.InheritanceFlags.ContainerInherit | System.Security.AccessControl.InheritanceFlags.ObjectInherit, System.Security.AccessControl.PropagationFlags.None, System.Security.AccessControl.AccessControlType.Allow));

         testDirInfo.SetAccessControl(ds);
      }


      private static void SetSecuritySystem(string directory)
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
      

      private static bool HasLocalAces(System.Security.AccessControl.AuthorizationRuleCollection rules)
      {
         return null != rules.Cast<System.Security.AccessControl.AccessRule>().FirstOrDefault(a => !a.IsInherited);
      }
   }
}
