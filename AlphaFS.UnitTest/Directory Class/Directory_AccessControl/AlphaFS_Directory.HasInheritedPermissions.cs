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
using System.Security.AccessControl;

namespace AlphaFS.UnitTest
{
   public partial class AccessControlTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void AlphaFS_Directory_HasInheritedPermissions_LocalAndNetwork_Success()
      {
         AlphaFS_Directory_HasInheritedPermissions(false);
         AlphaFS_Directory_HasInheritedPermissions(true);
      }


      private void AlphaFS_Directory_HasInheritedPermissions(bool isNetwork)
      {
         using (var tempRoot = new TemporaryDirectory(isNetwork))
         {
            var folder = tempRoot.CreateRecursiveRandomizedAttributesTree(5);

            Console.WriteLine("Input Directory Path: [{0}]\n", folder.FullName);
            

            foreach (var dir in System.IO.Directory.EnumerateDirectories(folder.FullName, "*", System.IO.SearchOption.AllDirectories))
            {
               // Assume inheritance is enabled by default.

               Assert.IsTrue(Alphaleonis.Win32.Filesystem.Directory.HasInheritedPermissions(dir));


               DirectorySecurity acl;

               try
               {
                  // Disable inheritance.

                  acl = System.IO.Directory.GetAccessControl(dir);
                  acl.SetAccessRuleProtection(true, false);
                  Alphaleonis.Win32.Filesystem.Directory.SetAccessControl(dir, acl, AccessControlSections.Access);

                  Assert.IsFalse(Alphaleonis.Win32.Filesystem.Directory.HasInheritedPermissions(dir));
               }
               finally
               {

                  // Enable inheritance.

                  acl = System.IO.Directory.GetAccessControl(dir);
                  acl.SetAccessRuleProtection(false, true);
                  Alphaleonis.Win32.Filesystem.Directory.SetAccessControl(dir, acl, AccessControlSections.Access);

                  Assert.IsTrue(Alphaleonis.Win32.Filesystem.Directory.HasInheritedPermissions(dir));
               }
            }
         }

         Console.WriteLine();
      }
   }
}
