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

using System;
using System.Security.AccessControl;
using System.Security.Principal;
using Alphaleonis.Win32.Filesystem;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AlphaFS.UnitTest
{
   partial class FileTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>

      [TestMethod]
      public void File_SetAccessControl_LocalUNC_Success()
      {
         File_SetAccessControl(false);
         File_SetAccessControl(true);
      }




      private void File_SetAccessControl(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         if (!UnitTestConstants.IsAdmin())
            Assert.Inconclusive();

         string tempPath = Path.Combine(Path.GetTempPath(), "File.SetAccessControl()-" + Path.GetRandomFileName());

         if (isNetwork)
            tempPath = Path.LocalToUnc(tempPath);

         try
         {
            using (File.Create(tempPath))
            {
            }

            FileSecurity sysIO = System.IO.File.GetAccessControl(tempPath);
            AuthorizationRuleCollection sysIOaccessRules = sysIO.GetAccessRules(true, true, typeof(NTAccount));

            FileSecurity alphaFS = File.GetAccessControl(tempPath);
            AuthorizationRuleCollection alphaFSaccessRules = alphaFS.GetAccessRules(true, true, typeof(NTAccount));


            Console.WriteLine("\nInput File Path: [{0}]", tempPath);
            Console.WriteLine("\n\tSystem.IO rules found: [{0}]\n\tAlphaFS rules found  : [{1}]", sysIOaccessRules.Count, alphaFSaccessRules.Count);
            Assert.AreEqual(sysIOaccessRules.Count, alphaFSaccessRules.Count);


            // Sanity check.
            UnitTestConstants.TestAccessRules(sysIO, alphaFS);


            // Remove inherited properties.
            // Passing true for first parameter protects the new permission from inheritance,
            // and second parameter removes the existing inherited permissions 
            Console.WriteLine("\n\tRemove inherited properties and persist it.");
            alphaFS.SetAccessRuleProtection(true, false);
            File.SetAccessControl(tempPath, alphaFS, AccessControlSections.Access);


            // Re-read, using instance methods.
            System.IO.FileInfo sysIOfi = new System.IO.FileInfo(tempPath);
            FileInfo alphaFSfi = new FileInfo(tempPath);

            sysIO = sysIOfi.GetAccessControl(AccessControlSections.Access);
            alphaFS = alphaFSfi.GetAccessControl(AccessControlSections.Access);

            // Sanity check.
            UnitTestConstants.TestAccessRules(sysIO, alphaFS);


            // Restore inherited properties.
            Console.WriteLine("\n\tRestore inherited properties and persist it.");
            alphaFS.SetAccessRuleProtection(false, true);
            File.SetAccessControl(tempPath, alphaFS, AccessControlSections.Access);


            // Re-read.
            sysIO = System.IO.File.GetAccessControl(tempPath, AccessControlSections.Access);
            alphaFS = File.GetAccessControl(tempPath, AccessControlSections.Access);

            // Sanity check.
            UnitTestConstants.TestAccessRules(sysIO, alphaFS);
         }
         catch (Exception ex)
         {
            Console.WriteLine("\n\tCaught (unexpected) {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
            Assert.IsTrue(false);
         }
         finally
         {
            File.Delete(tempPath, true);
            Assert.IsFalse(File.Exists(tempPath), "Cleanup failed: File should have been removed.");
         }
      }
   }
}
