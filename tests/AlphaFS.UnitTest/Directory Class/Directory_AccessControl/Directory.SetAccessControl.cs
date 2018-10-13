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
using System.Security.AccessControl;
using System.Security.Principal;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AlphaFS.UnitTest
{
   public partial class AccessControlTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void Directory_SetAccessControl_LocalAndNetwork_Success()
      {
         UnitTestAssert.IsElevatedProcess();

         Directory_SetAccessControl(false);
         Directory_SetAccessControl(true);
      }


      private void Directory_SetAccessControl(bool isNetwork)
      {
         using (var tempRoot = new TemporaryDirectory(isNetwork))
         {
            var folder = tempRoot.CreateDirectory();

            var sysIO = System.IO.Directory.GetAccessControl(folder.FullName);
            var sysIOaccessRules = sysIO.GetAccessRules(true, true, typeof(NTAccount));

            var alphaFS = Alphaleonis.Win32.Filesystem.Directory.GetAccessControl(folder.FullName);
            var alphaFSaccessRules = alphaFS.GetAccessRules(true, true, typeof(NTAccount));


            Console.WriteLine("Input Directory Path: [{0}]", folder.FullName);
            Console.WriteLine("\n\tSystem.IO rules found: [{0}]\n\tAlphaFS rules found  : [{1}]", sysIOaccessRules.Count, alphaFSaccessRules.Count);
            Assert.AreEqual(sysIOaccessRules.Count, alphaFSaccessRules.Count);


            // Sanity check.
            UnitTestConstants.TestAccessRules(sysIO, alphaFS);


            // Remove inherited properties.
            // Passing true for first parameter protects the new permission from inheritance,
            // and second parameter removes the existing inherited permissions 
            Console.WriteLine("\n\tRemove inherited properties and persist it.");
            alphaFS.SetAccessRuleProtection(true, false);
            Alphaleonis.Win32.Filesystem.Directory.SetAccessControl(folder.FullName, alphaFS, AccessControlSections.Access);


            // Re-read, using instance methods.
            var sysIOdi = new System.IO.DirectoryInfo(folder.FullName);
            var alphaFSdi = new Alphaleonis.Win32.Filesystem.DirectoryInfo(folder.FullName);

            sysIO = sysIOdi.GetAccessControl(AccessControlSections.Access);
            alphaFS = alphaFSdi.GetAccessControl(AccessControlSections.Access);

            // Sanity check.
            UnitTestConstants.TestAccessRules(sysIO, alphaFS);


            // Restore inherited properties.
            Console.WriteLine("\n\tRestore inherited properties and persist it.");
            alphaFS.SetAccessRuleProtection(false, true);
            Alphaleonis.Win32.Filesystem.Directory.SetAccessControl(folder.FullName, alphaFS, AccessControlSections.Access);


            // Re-read.
            sysIO = System.IO.Directory.GetAccessControl(folder.FullName, AccessControlSections.Access);
            alphaFS = Alphaleonis.Win32.Filesystem.Directory.GetAccessControl(folder.FullName, AccessControlSections.Access);

            // Sanity check.
            UnitTestConstants.TestAccessRules(sysIO, alphaFS);
         }

         Console.WriteLine();
      }
   }
}
