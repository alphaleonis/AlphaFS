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
   public partial class AlphaFS_DirectoryCopyTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void AlphaFS_Directory_Copy_CatchUnauthorizedAccessException_UserExplicitDeny_LocalAndNetwork_Success()
      {
         Directory_Copy_CatchUnauthorizedAccessException_UserExplicitDeny(false);
         Directory_Copy_CatchUnauthorizedAccessException_UserExplicitDeny(true);
      }
      

      private void Directory_Copy_CatchUnauthorizedAccessException_UserExplicitDeny(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);
         Console.WriteLine();


         var gotException = false;


         var tempPath = System.IO.Path.GetTempPath();
         if (isNetwork)
            tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);


         using (var rootDir = new TemporaryDirectory(tempPath, MethodBase.GetCurrentMethod().Name))
         {
            var folderSrc = System.IO.Directory.CreateDirectory(System.IO.Path.Combine(rootDir.Directory.FullName, "Source Folder"));

            Console.WriteLine("Src Directory Path: [{0}]", folderSrc.FullName);
            

            var user = (Environment.UserDomainName + @"\" + Environment.UserName).TrimStart('\\');

            var rule = new System.Security.AccessControl.FileSystemAccessRule(user,
               System.Security.AccessControl.FileSystemRights.FullControl,
               System.Security.AccessControl.InheritanceFlags.ContainerInherit |
               System.Security.AccessControl.InheritanceFlags.ObjectInherit,
               System.Security.AccessControl.PropagationFlags.None,
               System.Security.AccessControl.AccessControlType.Deny);


            var dirInfo = System.IO.Directory.CreateDirectory(folderSrc.FullName);


            // Set DENY for current user.
            var dirSecurity = dirInfo.GetAccessControl();
            dirSecurity.AddAccessRule(rule);
            dirInfo.SetAccessControl(dirSecurity);


            try
            {
               Alphaleonis.Win32.Filesystem.Directory.Copy(UnitTestConstants.SysRoot, dirInfo.FullName);
            }
            catch (Exception ex)
            {
               var exType = ex.GetType();

               gotException = exType == typeof(UnauthorizedAccessException);

               Console.WriteLine("\n\tCaught {0} Exception: [{1}] {2}", gotException ? "EXPECTED" : "UNEXPECTED", exType.Name, ex.Message);
            }
            finally
            {
               // Remove DENY for current user.
               dirSecurity = dirInfo.GetAccessControl();
               dirSecurity.RemoveAccessRule(rule);
               dirInfo.SetAccessControl(dirSecurity);
            }


            Assert.IsTrue(gotException, "The exception is not caught, but is expected to.");
            
            Assert.IsTrue(System.IO.Directory.Exists(dirInfo.FullName), "The directory does not exist, but is expected to.");
         }


         Console.WriteLine();
      }


      // ╔═════════════╦═════════════╦═══════════════════════════════╦════════════════════════╦══════════════════╦═══════════════════════╦═════════════╦═════════════╗
      // ║             ║ folder only ║ folder, sub-folders and files ║ folder and sub-folders ║ folder and files ║ sub-folders and files ║ sub-folders ║    files    ║
      // ╠═════════════╬═════════════╬═══════════════════════════════╬════════════════════════╬══════════════════╬═══════════════════════╬═════════════╬═════════════╣
      // ║ Propagation ║ none        ║ none                          ║ none                   ║ none             ║ InheritOnly           ║ InheritOnly ║ InheritOnly ║
      // ║ Inheritance ║ none        ║ Container|Object              ║ Container              ║ Object           ║ Container|Object      ║ Container   ║ Object      ║
      // ╚═════════════╩═════════════╩═══════════════════════════════╩════════════════════════╩══════════════════╩═══════════════════════╩═════════════╩═════════════╝
   }
}
