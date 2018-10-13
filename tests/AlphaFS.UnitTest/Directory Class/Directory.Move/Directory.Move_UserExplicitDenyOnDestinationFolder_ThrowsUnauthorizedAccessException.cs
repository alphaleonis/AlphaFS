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
   public partial class MoveTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void Directory_Move_UserExplicitDenyOnDestinationFolder_ThrowsUnauthorizedAccessException_LocalAndNetwork_Success()
      {
         Directory_Move_UserExplicitDenyOnDestinationFolder_ThrowsUnauthorizedAccessException(false);
         Directory_Move_UserExplicitDenyOnDestinationFolder_ThrowsUnauthorizedAccessException(true);
      }


      private void Directory_Move_UserExplicitDenyOnDestinationFolder_ThrowsUnauthorizedAccessException(bool isNetwork)
      {
         using (var tempRoot = new TemporaryDirectory(isNetwork))
         {
            var srcFolder = tempRoot.CreateTree();
            var dstFolder = tempRoot.CreateDirectory();

            Console.WriteLine("Src Directory Path: [{0}]", srcFolder.FullName);
            Console.WriteLine("Dst Directory Path: [{0}]", dstFolder.FullName);


            var user = (Environment.UserDomainName + @"\" + Environment.UserName).TrimStart('\\');

            var rule = new System.Security.AccessControl.FileSystemAccessRule(user,
               System.Security.AccessControl.FileSystemRights.FullControl,
               System.Security.AccessControl.InheritanceFlags.ContainerInherit |
               System.Security.AccessControl.InheritanceFlags.ObjectInherit,
               System.Security.AccessControl.PropagationFlags.None,
               System.Security.AccessControl.AccessControlType.Deny);


            // Set DENY for current user.
            var dirSecurity = dstFolder.GetAccessControl();
            dirSecurity.AddAccessRule(rule);
            dstFolder.SetAccessControl(dirSecurity);


            try
            {
               UnitTestAssert.ThrowsException<UnauthorizedAccessException>(() => Alphaleonis.Win32.Filesystem.Directory.Move(srcFolder.FullName, dstFolder.FullName, Alphaleonis.Win32.Filesystem.MoveOptions.ReplaceExisting));
            }
            finally
            {
               // Remove DENY for current user.
               dirSecurity = dstFolder.GetAccessControl();
               dirSecurity.RemoveAccessRule(rule);
               dstFolder.SetAccessControl(dirSecurity);
            }
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
