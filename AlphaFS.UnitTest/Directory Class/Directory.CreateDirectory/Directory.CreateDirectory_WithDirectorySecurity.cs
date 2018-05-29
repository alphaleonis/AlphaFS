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
   public partial class Directory_CreateDirectoryTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void Directory_CreateDirectory_WithDirectorySecurity_LocalAndNetwork_Success()
      {
         UnitTestAssert.IsElevatedProcess();

         Directory_CreateDirectory_WithDirectorySecurity(false);
         Directory_CreateDirectory_WithDirectorySecurity(true);
      }


      private void Directory_CreateDirectory_WithDirectorySecurity(bool isNetwork)
      {
         using (var tempRoot = new TemporaryDirectory(isNetwork))
         {
            var folder = tempRoot.RandomDirectoryFullPath;

            Console.WriteLine("Input Directory Path: [{0}]\n", folder);


            var pathExpected = tempRoot.RandomDirectoryFullPath;
            var pathActual = tempRoot.RandomDirectoryFullPath;
            

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
   }
}
