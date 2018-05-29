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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AlphaFS.UnitTest
{
   public partial class File_CreateTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void File_Create_WithFileSecurity_LocalAndNetwork_Success()
      {
         UnitTestAssert.IsElevatedProcess();

         File_Create_WithFileSecurity(false);
         File_Create_WithFileSecurity(true);
      }


      private void File_Create_WithFileSecurity(bool isNetwork)
      {
         using (var tempRoot = new TemporaryDirectory(isNetwork))
         {
            var file = tempRoot.RandomTxtFileFullPath;

            Console.WriteLine("Input File Path: [{0}]\n", file);


            var pathExpected = tempRoot.RandomTxtFileFullPath;
            var pathActual = tempRoot.RandomTxtFileFullPath;


            var sid = new System.Security.Principal.SecurityIdentifier(System.Security.Principal.WellKnownSidType.WorldSid, null);
            var expectedFileSecurity = new System.Security.AccessControl.FileSecurity();
            expectedFileSecurity.AddAccessRule(new System.Security.AccessControl.FileSystemAccessRule(sid, System.Security.AccessControl.FileSystemRights.FullControl, System.Security.AccessControl.AccessControlType.Allow));
            expectedFileSecurity.AddAuditRule(new System.Security.AccessControl.FileSystemAuditRule(sid, System.Security.AccessControl.FileSystemRights.Read, System.Security.AccessControl.AuditFlags.Success));


            //using (new Alphaleonis.Win32.Security.PrivilegeEnabler(Alphaleonis.Win32.Security.Privilege.Security))
            //using (var s1 = System.IO.File.Create(pathExpected, 4096, System.IO.FileOptions.None, expectedFileSecurity))
            //using (var s2 = Alphaleonis.Win32.Filesystem.File.Create(pathActual, 4096, System.IO.FileOptions.None, expectedFileSecurity)) { }

            //var expected = System.IO.File.GetAccessControl(pathExpected).GetSecurityDescriptorSddlForm(System.Security.AccessControl.AccessControlSections.All);
            //var actual = Alphaleonis.Win32.Filesystem.File.GetAccessControl(pathActual).GetSecurityDescriptorSddlForm(System.Security.AccessControl.AccessControlSections.All);

            //Assert.AreEqual(expected, actual);




            using (new Alphaleonis.Win32.Security.PrivilegeEnabler(Alphaleonis.Win32.Security.Privilege.Security))
            //using (var s1 = new System.IO.FileInfo(pathExpected).Create())
            //using (var s2 = new Alphaleonis.Win32.Filesystem.FileInfo(pathActual).Create())
            {
               // TODO 2018-01-12: BUG FileInfo.GetAccessControl(): System.UnauthorizedAccessException: Attempted to perform an unauthorized operation.
               //var expected = s1.GetAccessControl().GetSecurityDescriptorSddlForm(System.Security.AccessControl.AccessControlSections.All);
               //var actual = s2.GetAccessControl().GetSecurityDescriptorSddlForm(System.Security.AccessControl.AccessControlSections.All);


               using (new Alphaleonis.Win32.Security.PrivilegeEnabler(Alphaleonis.Win32.Security.Privilege.Security))
               using (var s1 = System.IO.File.Create(pathExpected, 4096, System.IO.FileOptions.None, expectedFileSecurity))
               using (var s2 = Alphaleonis.Win32.Filesystem.File.Create(pathActual, 4096, System.IO.FileOptions.None, expectedFileSecurity))
               {
                  var expected = s1.GetAccessControl().GetSecurityDescriptorSddlForm(System.Security.AccessControl.AccessControlSections.All);


                  // TODO 2018-01-12: BUG FileInfo.GetAccessControl(): System.UnauthorizedAccessException: Attempted to perform an unauthorized operation.
                  //var actual = s2.GetAccessControl().GetSecurityDescriptorSddlForm(System.Security.AccessControl.AccessControlSections.All);

                  var actual = Alphaleonis.Win32.Filesystem.File.GetAccessControl(pathActual).GetSecurityDescriptorSddlForm(System.Security.AccessControl.AccessControlSections.All);


                  Console.WriteLine("\tSystem.IO: {0}", expected);
                  Console.WriteLine("\tAlphaFS  : {0}", actual);


                  Assert.AreEqual(expected, actual);
               }
            }
         }

         Console.WriteLine();
      }
   }
}
