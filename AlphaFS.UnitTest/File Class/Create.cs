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
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
using File = Alphaleonis.Win32.Filesystem.File;
using SysIOFile = System.IO.File;
using SysIOPath = System.IO.Path;

namespace AlphaFS.UnitTest
{
   partial class FileTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>

      [TestMethod]
      public void File_Create_A10ByteFile_LocalUNC_Success()
      {
         File_Create(false);
         File_Create(true);
      }


      [TestMethod]
      public void File_Create_WithFileSecurity_LocalUNC_Success()
      {
         File_Create_WithFileSecurity(false);
         File_Create_WithFileSecurity(true);
      }




      private void File_Create(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         string tempPath = SysIOPath.GetTempPath();
         if (isNetwork)
            tempPath = PathUtils.AsUncPath(tempPath);


         using (var rootDir = new TemporaryDirectory(tempPath, "File-Create"))
         {
            string file = rootDir.RandomFileFullPath;
            Console.WriteLine("\nInput File Path: [{0}]\n", file);


            try
            {
               long fileLength;
               int ten = UnitTestConstants.TenNumbers.Length;

               using (FileStream fs = File.Create(file))
               {
                  // According to NotePad++, creates a file type: "ANSI", which is reported as: "Unicode (UTF-8)".
                  fs.Write(UnitTestConstants.StringToByteArray(UnitTestConstants.TenNumbers), 0, ten);

                  fileLength = fs.Length;
               }

               Assert.IsTrue(SysIOFile.Exists(file), "File should exist.");
               Assert.IsTrue(fileLength == ten, "File should be [{0}] bytes in size.", ten);
            }
            catch (Exception ex)
            {
               Console.WriteLine("\nCaught (unexpected) {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
               Assert.IsTrue(false);
            }
         }

         Console.WriteLine();
      }


      private void File_Create_WithFileSecurity(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         if (!UnitTestConstants.IsAdmin())
            Assert.Inconclusive();

         string tempPath = SysIOPath.GetTempPath();
         if (isNetwork)
            tempPath = PathUtils.AsUncPath(tempPath);


         using (var rootDir = new TemporaryDirectory(tempPath, "File-CreateWithFileSecurity"))
         {
            string file = rootDir.RandomFileFullPath;
            Console.WriteLine("\nInput File Path: [{0}]\n", file);


            string pathExpected = rootDir.RandomFileFullPath;
            string pathActual = rootDir.RandomFileFullPath;

            FileSecurity expectedFileSecurity = new FileSecurity();
            expectedFileSecurity.AddAccessRule(new FileSystemAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), FileSystemRights.FullControl, AccessControlType.Allow));
            expectedFileSecurity.AddAuditRule(new FileSystemAuditRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), FileSystemRights.Read, AuditFlags.Success));


            using (new Alphaleonis.Win32.Security.PrivilegeEnabler(Alphaleonis.Win32.Security.Privilege.Security))
            {
               using (FileStream s1 = SysIOFile.Create(pathExpected, 4096, FileOptions.None, expectedFileSecurity))
               using (FileStream s2 = File.Create(pathActual, 4096, FileOptions.None, expectedFileSecurity))
               {
               }
            }
            
            string expectedFileSecuritySddl = SysIOFile.GetAccessControl(pathExpected).GetSecurityDescriptorSddlForm(AccessControlSections.All);
            string actualFileSecuritySddl = SysIOFile.GetAccessControl(pathActual).GetSecurityDescriptorSddlForm(AccessControlSections.All);

            Assert.AreEqual(expectedFileSecuritySddl, actualFileSecuritySddl);
         }

         Console.WriteLine();
      }
   }
}
