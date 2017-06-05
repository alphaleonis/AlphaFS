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
using Microsoft.Win32;

namespace AlphaFS.UnitTest
{
   /// <summary>This is a test class for FileInfo and is intended to contain all FileInfo UnitTests.</summary>
   public partial class DirectoryInfoTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>

      [TestMethod]
      public void DirectoryInfo_MoveTo_DelayUntilReboot_LocalAndNetwork_Success()
      {
         DirectoryInfo_MoveTo_DelayUntilReboot(false);
         DirectoryInfo_MoveTo_DelayUntilReboot(true);
      }




      private void DirectoryInfo_MoveTo_DelayUntilReboot(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         var tempPath = System.IO.Path.GetTempPath();
         if (isNetwork)
            tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);


         using (var rootDir = new TemporaryDirectory(tempPath, "DirectoryInfo.MoveTo_DelayUntilReboot"))
         {
            var folder = rootDir.Directory.FullName;
            var folderSrc = Alphaleonis.Win32.Filesystem.Directory.CreateDirectory(System.IO.Path.Combine(folder, "Source-" + System.IO.Path.GetRandomFileName()));
            var pendingEntry = folderSrc.FullName;

            Console.WriteLine("\nSrc Directory Path: [{0}]", pendingEntry);

            UnitTestConstants.CreateDirectoriesAndFiles(pendingEntry, new Random().Next(5, 15), true);

            
            // Trigger DelayUntilReboot.
            folderSrc.MoveTo(null, Alphaleonis.Win32.Filesystem.MoveOptions.DelayUntilReboot);


            // Verify DelayUntilReboot in registry.
            var pendingList = (string[]) Registry.GetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager", "PendingFileRenameOperations", null);


            Assert.IsNotNull(pendingList, "The PendingFileRenameOperations is null, which is not expected.");


            var found = false;

            foreach (var line in pendingList)
            {
               var entry = isNetwork ? pendingEntry.TrimStart('\\') : pendingEntry;

               var prefix = @"\??\" + (isNetwork ? @"UNC\" : string.Empty);


               found = !Alphaleonis.Utils.IsNullOrWhiteSpace(line) && line.Replace(entry, string.Empty).Equals(prefix, StringComparison.OrdinalIgnoreCase);
               if (found)
               {
                  Console.WriteLine("\n\tPending entry found in registry: [{0}]", line);

                  // TODO: Remove unit test entry from registry.


                  break;
               }
            }


            Assert.IsTrue(found, "No pending entry found in registry, which is not expected.");
         }

         Console.WriteLine();
      }
   }
}
