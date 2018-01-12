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
   partial class FileTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>

      [TestMethod]
      public void File_Create_LocalAndNetwork_Success()
      {
         File_Create(false);
         File_Create(true);
      }


      [TestMethod]
      public void File_Create_WithFileSecurity_LocalAndNetwork_Success()
      {
         if (!UnitTestConstants.IsAdmin())
            Assert.Inconclusive();


         File_Create_WithFileSecurity(false);

         File_Create_WithFileSecurity(true);
      }




      private void File_Create(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         var tempPath = System.IO.Path.GetTempPath();
         if (isNetwork)
            tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);


         using (var rootDir = new TemporaryDirectory(tempPath, MethodBase.GetCurrentMethod().Name))
         {
            var file = rootDir.RandomFileFullPath;

#if NET35
            // MSDN: .NET 4+ Trailing spaces are removed from the end of the path parameter before deleting the directory.
            file += UnitTestConstants.EMspace;
#endif

            Console.WriteLine("\nInput File Path: [{0}]\n", file);


            long fileLength;
            var ten = UnitTestConstants.TenNumbers.Length;

            using (var fs = Alphaleonis.Win32.Filesystem.File.Create(file))
            {
               // According to NotePad++, creates a file type: "ANSI", which is reported as: "Unicode (UTF-8)".
               fs.Write(UnitTestConstants.StringToByteArray(UnitTestConstants.TenNumbers), 0, ten);

               fileLength = fs.Length;
            }

            Assert.IsTrue(System.IO.File.Exists(file), "File should exist.");
            Assert.IsTrue(fileLength == ten, "The file is: {0} bytes, but is expected to be: {1} bytes.", fileLength, ten);
         }

         Console.WriteLine();
      }


      private void File_Create_WithFileSecurity(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);
         Console.WriteLine();


         var tempPath = System.IO.Path.GetTempPath();
         if (isNetwork)
            tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);


         using (var rootDir = new TemporaryDirectory(tempPath, MethodBase.GetCurrentMethod().Name))
         {
            var file = rootDir.RandomFileFullPath;
            Console.WriteLine("\tInput File Path: [{0}]", file);
            Console.WriteLine();


            var pathExpected = rootDir.RandomFileFullPath;
            var pathActual = rootDir.RandomFileFullPath;


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
