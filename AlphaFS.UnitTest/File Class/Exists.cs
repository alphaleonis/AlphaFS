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
using Alphaleonis.Win32.Filesystem;
using File = Alphaleonis.Win32.Filesystem.File;
using Path = Alphaleonis.Win32.Filesystem.Path;

namespace AlphaFS.UnitTest
{
   partial class FileTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>

      [TestMethod]
      public void File_Exists_LocalAndUNC_Success()
      {
         File_Exists(false, Path.GetTempPath("File-Exists-" + Path.GetRandomFileName()));
         File_Exists(true, Path.GetTempPath("File-Exists-" + Path.GetRandomFileName()));
      }


      [TestMethod]
      public void File_Exists_PathContainsLeadingTrailingSpace_LocalAndUNC_Success()
      {
         File_Exists_LeadingTrailingSpace(false, UnitTestConstants.NotepadExe);
         File_Exists_LeadingTrailingSpace(true, UnitTestConstants.NotepadExe);
      }




      private void File_Exists(bool isNetwork, string tempPath)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         if (isNetwork)
            tempPath = Path.LocalToUnc(tempPath);

         string symlinkPath = tempPath + "-symlink";

         Console.WriteLine("\nInput File Path: [{0}]\n", tempPath);

         bool exists = File.Exists(tempPath);

         Assert.IsFalse(exists, "File should not exist.");
         Assert.IsFalse(File.Exists(symlinkPath), "File symlink should not exist.");

         try
         {
            using (File.Create(tempPath)) { }

            exists = File.Exists(tempPath);

            Assert.IsTrue(exists, "File should exist.");


            Assert.IsFalse(File.Exists(symlinkPath), "File symlink should not exist.");
            File.CreateSymbolicLink(symlinkPath, tempPath, SymbolicLinkTarget.File);
            Assert.IsTrue(File.Exists(symlinkPath), "File symlink should exist.");


            File.Delete(symlinkPath);
            Assert.IsTrue(File.Exists(tempPath), "Deleting a symlink should not delete the underlying file.");
            Assert.IsFalse(File.Exists(symlinkPath), "Cleanup failed: File symlink should have been removed.");

            File.Delete(tempPath, true);
            Assert.IsFalse(File.Exists(tempPath), "Cleanup failed: File should have been removed.");
         }
         catch (Exception ex)
         {
            Console.WriteLine("\nCaught (unexpected) {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
            Assert.IsTrue(false);
         }
         finally
         {
            if (File.Exists(symlinkPath))
               File.Delete(symlinkPath);

            if (File.Exists(tempPath))
               File.Delete(tempPath, true);
         }

         Console.WriteLine();
      }


      private void File_Exists_LeadingTrailingSpace(bool isNetwork, string tempPath)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         if (isNetwork)
            tempPath = Path.LocalToUnc(tempPath);


         string path = tempPath + "   ";
         Console.WriteLine("\nInput File Path: [{0}]\n", path);

         Assert.IsTrue(File.Exists(path), "File should exist.");


         path = "   " + tempPath + "   ";
         Console.WriteLine("\nInput File Path: [{0}]\n", path);

         Assert.IsTrue(File.Exists(path), "File should exist.");


         path = "   " + tempPath;
         Console.WriteLine("\nInput File Path: [{0}]\n", path);

         Assert.IsTrue(File.Exists(path), "File should exist.");

         Console.WriteLine();
      }
   }
}
