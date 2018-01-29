/*  Copyright (C) 2008-2017 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
 *  
 *  Permission is hereby granted, free of charge, to any person obtaining a copy 
 *  of this software and associated documentation directorys (the "Software"), to deal 
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
using Microsoft.Win32;

namespace AlphaFS.UnitTest
{
   public partial class DirectoryInfoTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void DirectoryInfo_MoveTo_CatchArgumentException_UNCPathNotAllowed_DelayUntilRebootFlagCombinedWithCopyAllowedFlag_Network_Success()
      {
         DirectoryInfo_MoveTo_DelayUntilReboot(true);
      }

      
      [TestMethod]
      public void DirectoryInfo_MoveTo_DelayUntilReboot_Local_Success()
      {
         if (!UnitTestConstants.IsAdmin())
            Assert.Inconclusive();


         DirectoryInfo_MoveTo_DelayUntilReboot(false);
      }




      private void DirectoryInfo_MoveTo_DelayUntilReboot(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);
         Console.WriteLine();


         var tempPath = System.IO.Path.GetTempPath();
         if (isNetwork)
            tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);


         using (var rootDir = new TemporaryDirectory(tempPath, MethodBase.GetCurrentMethod().Name))
         {
            var folder = rootDir.Directory.FullName;
            var folderSrc = Alphaleonis.Win32.Filesystem.Directory.CreateDirectory(System.IO.Path.Combine(folder, "Source-" + UnitTestConstants.GetRandomFileName()));
            var pendingEntry = folderSrc.FullName;

            Console.WriteLine("Src Directory Path: [{0}]", pendingEntry);

            UnitTestConstants.CreateDirectoriesAndFiles(pendingEntry, 1, false, false, true);


            var gotException = false;
            

            try
            {
               // Trigger DelayUntilReboot.

               folderSrc.MoveTo(null, Alphaleonis.Win32.Filesystem.MoveOptions.DelayUntilReboot);
            }
            catch (Exception ex)
            {
               var exType = ex.GetType();

               gotException = exType == typeof(ArgumentException);

               Console.WriteLine("\n\tCaught {0} Exception: [{1}] {2}", gotException ? "EXPECTED" : "UNEXPECTED", exType.Name, ex.Message);
            }




            if (isNetwork)
               Assert.IsTrue(gotException, "The exception is not caught, but is expected to.");

            else
            {
               // Verify DelayUntilReboot in registry.

               var pendingList = (string[]) Registry.GetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager", "PendingFileRenameOperations", null);


               Assert.IsNotNull(pendingList, "The PendingFileRenameOperations is null, but is not expected to.");


               var found = false;

               foreach (var line in pendingList)
               {
                  found = !Alphaleonis.Utils.IsNullOrWhiteSpace(line) && line.Replace(pendingEntry, string.Empty).Equals(Alphaleonis.Win32.Filesystem.Path.NonInterpretedPathPrefix, StringComparison.Ordinal);

                  if (found)
                  {
                     Console.WriteLine("\n\tPending entry found in registry: [{0}]", line);

                     // TODO: Remove unit test entry from registry.

                     break;
                  }
               }


               Assert.IsTrue(found, "Registry does not contain pending entry, but is expected to.");
            }
         }

         Console.WriteLine();
      }
   }
}
