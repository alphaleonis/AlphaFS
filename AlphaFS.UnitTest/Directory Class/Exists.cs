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
using Directory = Alphaleonis.Win32.Filesystem.Directory;
using File = Alphaleonis.Win32.Filesystem.File;
using Path = Alphaleonis.Win32.Filesystem.Path;

namespace AlphaFS.UnitTest
{
   partial class DirectoryTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>

      [TestMethod]
      public void Directory_Exists_LocalAndUNC_Success()
      {
         Directory_Exists(false);
         Directory_Exists(true);
      }


      [TestMethod]
      public void Directory_Exists_PathContainsLeadingTrailingSpace_LocalAndUNC_Success()
      {
         Directory_Exists_PathContainsLeadingTrailingSpace(false);
         Directory_Exists_PathContainsLeadingTrailingSpace(true);
      }



      
      private void Directory_Exists(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         string tempPath = Path.GetTempPath("Directory-Exists-" + Path.GetRandomFileName());
         if (isNetwork)
            tempPath = Path.LocalToUnc(tempPath);

         string symlinkPath = tempPath + "-symlink";

         Console.WriteLine("\nInput Directory Path: [{0}]\n", tempPath);

         bool exists = Directory.Exists(tempPath);

         Assert.IsFalse(exists, "Directory should not exist.");
         Assert.IsFalse(Directory.Exists(symlinkPath), "Directory symlink should not exist.");

         try
         {
            Directory.CreateDirectory(tempPath);

            exists = Directory.Exists(tempPath);

            Assert.IsTrue(exists, "Directory should exist.");
            
            
            Assert.IsFalse(Directory.Exists(symlinkPath), "Directory symlink should not exist.");
            File.CreateSymbolicLink(symlinkPath, tempPath, SymbolicLinkTarget.Directory);
            Assert.IsTrue(Directory.Exists(symlinkPath), "Directory symlink should exist.");


            Directory.Delete(symlinkPath);
            Assert.IsTrue(Directory.Exists(tempPath), "Deleting a symlink should not delete the underlying directory.");
            Assert.IsFalse(Directory.Exists(symlinkPath), "Cleanup failed: Directory symlink should have been removed.");

            Directory.Delete(tempPath, true);
            Assert.IsFalse(Directory.Exists(tempPath), "Cleanup failed: Directory should have been removed.");
         }
         catch (Exception ex)
         {
            Console.WriteLine("\nCaught (unexpected) {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
            Assert.IsTrue(false);
         }
         finally
         {
            if (Directory.Exists(symlinkPath))
               Directory.Delete(symlinkPath);

            if (Directory.Exists(tempPath))
               Directory.Delete(tempPath, true);
         }

         Console.WriteLine();
      }


      private void Directory_Exists_PathContainsLeadingTrailingSpace(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         string tempPath = UnitTestConstants.SysRoot32;
         if (isNetwork)
            tempPath = Path.LocalToUnc(tempPath);


         string path = tempPath + "   ";
         Console.WriteLine("\nInput Directory Path: [{0}]\n", path);

         Assert.IsTrue(Directory.Exists(path), "Directory should exist.");


         path = "   " + tempPath + "   ";
         Console.WriteLine("\nInput Directory Path: [{0}]\n", path);

         Assert.IsTrue(Directory.Exists(path), "Directory should exist.");


         path = "   " + tempPath;
         Console.WriteLine("\nInput Directory Path: [{0}]\n", path);

         Assert.IsTrue(Directory.Exists(path), "Directory should exist.");
      }
   }
}
