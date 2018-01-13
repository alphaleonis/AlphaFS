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
   public partial class AlphaFS_Shell32Test
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void AlphaFS_Shell32Info_GetVerbCommand_LocalAndNetwork_Success()
      {
         Shell32Info_GetVerbCommand(false);
         Shell32Info_GetVerbCommand(true);
      }




      private void Shell32Info_GetVerbCommand(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);
         
         var tempPath = System.IO.Path.GetTempPath();
         if (isNetwork)
            tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);

         // Assumption: Extention: .txt is associated with: C:\Windows\System32\notepad.exe

         using (var rootDir = new TemporaryDirectory(tempPath, MethodBase.GetCurrentMethod().Name))
         {
            var file = rootDir.RandomFileFullPath;

            using (System.IO.File.CreateText(file)) {}
            Console.WriteLine("\nInput File Path: [{0}]\n", file);


            var shell32Info = Alphaleonis.Win32.Filesystem.Shell32.GetShell32Info(file);

            var cmd = "open";
            var result = shell32Info.GetVerbCommand(cmd);
            Console.WriteLine("\tMethod: Shell32Info.GetVerbCommand(\"{0}\")  == [{1}]", cmd, result);
            Assert.IsTrue(result.StartsWith(UnitTestConstants.NotepadExe, StringComparison.OrdinalIgnoreCase));

            cmd = "print";
            result = shell32Info.GetVerbCommand(cmd);
            Console.WriteLine("\tMethod: Shell32Info.GetVerbCommand(\"{0}\") == [{1}]\n", cmd, result);
            Assert.IsTrue(result.StartsWith(UnitTestConstants.NotepadExe, StringComparison.OrdinalIgnoreCase));
         }
      }
   }
}
