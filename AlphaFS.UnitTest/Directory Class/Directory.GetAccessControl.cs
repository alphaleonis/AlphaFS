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

namespace AlphaFS.UnitTest
{
   partial class DirectoryTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>

      [TestMethod]
      public void Directory_GetAccessControl_LocalAndNetwork_Success()
      {
         Directory_GetAccessControl(false);
         Directory_GetAccessControl(true);
      }


      private void Directory_GetAccessControl(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         var tempPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "Directory.GetAccessControl()-" + UnitTestConstants.GetRandomFileName());

         if (isNetwork)
            tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);

         try
         {
            System.IO.Directory.CreateDirectory(tempPath);

            var foundRules = false;

            var sysIO = System.IO.File.GetAccessControl(tempPath);
            var sysIOaccessRules = sysIO.GetAccessRules(true, true, typeof(System.Security.Principal.NTAccount));

            var alphaFS = System.IO.File.GetAccessControl(tempPath);
            var alphaFSaccessRules = alphaFS.GetAccessRules(true, true, typeof(System.Security.Principal.NTAccount));


            Console.WriteLine("\nInput Directory Path: [{0}]", tempPath);
            Console.WriteLine("\n\tSystem.IO rules found: [{0}]\n\tAlphaFS rules found  : [{1}]", sysIOaccessRules.Count, alphaFSaccessRules.Count);
            Assert.AreEqual(sysIOaccessRules.Count, alphaFSaccessRules.Count);


            foreach (var far in alphaFSaccessRules)
            {
               UnitTestConstants.Dump(far, -17);

               UnitTestConstants.TestAccessRules(sysIO, alphaFS);

               foundRules = true;
            }

            Assert.IsTrue(foundRules);
         }
         catch (Exception ex)
         {
            Console.WriteLine("\n\tCaught (UNEXPECTED) {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
            Assert.IsTrue(false);
         }
         finally
         {
            System.IO.Directory.Delete(tempPath, true);
            Assert.IsFalse(System.IO.Directory.Exists(tempPath), "Cleanup failed: Directory should have been removed.");
         }

         Console.WriteLine();
      }
   }
}
