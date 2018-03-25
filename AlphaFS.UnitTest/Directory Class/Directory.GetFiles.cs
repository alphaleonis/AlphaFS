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
using System.Linq;

namespace AlphaFS.UnitTest
{
   partial class DirectoryTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void Directory_GetFiles_LocalAndNetwork_Success()
      {
         Directory_GetFiles(false);
         Directory_GetFiles(true);
      }


      private void Directory_GetFiles(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);
         Console.WriteLine();


         var inputPath = UnitTestConstants.SysRoot;
         if (isNetwork)
            inputPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(inputPath);


         Console.WriteLine("Input Directory Path: [{0}]\n", inputPath);


         var systemIOCount = System.IO.Directory.GetFiles(inputPath).Count();
         var alphaFSCount = Alphaleonis.Win32.Filesystem.Directory.GetFiles(inputPath).Count();

         Console.WriteLine("\tSystem.IO files get: {0:N0}", systemIOCount);
         Console.WriteLine("\tAlphaFS files get  : {0:N0}", alphaFSCount);


         Assert.AreEqual(systemIOCount, alphaFSCount, "No files get, but it is expected.");


         Console.WriteLine();
      }


      [TestMethod]
      public void Directory_GetFiles_WithSearchPattern_LocalAndNetwork_Success()
      {
         Directory_GetFiles_WithSearchPattern(true);
         Directory_GetFiles_WithSearchPattern(false);
      }


      [TestMethod]
      public void Directory_GetFiles_AbsoluteAndRelativePath_Local_Success()
      {
         Directory_GetFiles_AbsoluteAndRelativePath(true);
         Directory_GetFiles_AbsoluteAndRelativePath(false);
      }




      


      private void Directory_GetFiles_WithSearchPattern(bool isLocal)
      {
         #region Setup

         UnitTestConstants.PrintUnitTestHeader(!isLocal);

         var tempPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "Directory.GetDirectories_With_SearchPattern()-" + UnitTestConstants.GetRandomFileNameWithDiacriticCharacters());
         if (!isLocal) tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);

         Console.WriteLine("\nInput Directory Path: [{0}]\n", tempPath);

         System.IO.Directory.CreateDirectory(tempPath);
         using (System.IO.File.Create(System.IO.Path.Combine(tempPath, "a.txt"))) { }
         using (System.IO.File.Create(System.IO.Path.Combine(tempPath, "aa.txt"))) { }
         using (System.IO.File.Create(System.IO.Path.Combine(tempPath, "aba.txt"))) { }
         using (System.IO.File.Create(System.IO.Path.Combine(tempPath, "foo.txt"))) { }
         using (System.IO.File.Create(System.IO.Path.Combine(tempPath, "footxt"))) { }

         #endregion // Setup

         try
         {
            var files = Alphaleonis.Win32.Filesystem.Directory.GetFiles(tempPath, "foo.txt");
            Console.WriteLine("\tDirectory.GetFiles(tempPath, \"foo.txt\");");
            Assert.IsTrue(files.Length == 1 && files.Contains(System.IO.Path.Combine(tempPath, "foo.txt"), StringComparer.InvariantCultureIgnoreCase));
            Assert.IsFalse(files.Contains(System.IO.Path.Combine(tempPath, "fooatxt"), StringComparer.InvariantCultureIgnoreCase));

            files = Alphaleonis.Win32.Filesystem.Directory.GetFiles(tempPath, "a?a.txt");
            Console.WriteLine("\tDirectory.GetFiles(tempPath, \"a?a.txt\");");
            Assert.IsTrue(files.Length == 1 && files.Contains(System.IO.Path.Combine(tempPath, "aba.txt"), StringComparer.InvariantCultureIgnoreCase), "? wildcard failed");
            Assert.IsFalse(files.Contains(System.IO.Path.Combine(tempPath, "aa.txt"), StringComparer.InvariantCultureIgnoreCase), "? wildcard failed");

            files = Alphaleonis.Win32.Filesystem.Directory.GetFiles(tempPath, "a*.*");
            Console.WriteLine("\tDirectory.GetFiles(tempPath, \"a*.*\");");
            Assert.IsTrue(files.Length == 3);
            Assert.IsTrue(files.Contains(System.IO.Path.Combine(tempPath, "a.txt"), StringComparer.InvariantCultureIgnoreCase), "* wildcard failed");
            Assert.IsTrue(files.Contains(System.IO.Path.Combine(tempPath, "aa.txt"), StringComparer.InvariantCultureIgnoreCase), "* wildcard failed");
            Assert.IsTrue(files.Contains(System.IO.Path.Combine(tempPath, "aba.txt"), StringComparer.InvariantCultureIgnoreCase), "* wildcard failed");

            files = Alphaleonis.Win32.Filesystem.Directory.GetFiles(tempPath, "*.*");
            Console.WriteLine("\tDirectory.GetFiles(tempPath, \"*.*\");");
            var files2 = Alphaleonis.Win32.Filesystem.Directory.GetFiles(tempPath);
            Assert.IsTrue(files.Length == files2.Length, "*.* failed");
            files = Alphaleonis.Win32.Filesystem.Directory.GetFiles(tempPath, "*.*.*");
            Console.WriteLine("\tDirectory.GetFiles(tempPath, \"*.*.*\");");
            Assert.IsTrue(files.Length == files2.Length, "*.* failed");
         }
         finally
         {
            System.IO.Directory.Delete(tempPath, true);
            Assert.IsFalse(System.IO.Directory.Exists(tempPath), "Cleanup failed: Directory should have been removed.");
            Console.WriteLine();
         }
      }


      private void Directory_GetFiles_AbsoluteAndRelativePath(bool isLocal)
      {
         UnitTestConstants.PrintUnitTestHeader(!isLocal);

         var tempPath = UnitTestConstants.SysRoot32;
         if (!isLocal) tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);

         Assert.IsTrue(System.IO.Path.IsPathRooted(tempPath));
         Environment.CurrentDirectory = tempPath;

         Console.WriteLine("\nInput Directory Path: [{0}]", tempPath);


         Console.WriteLine("\nRelative Paths\n");
         var files = Alphaleonis.Win32.Filesystem.Directory.GetFiles(".");
         Assert.IsTrue(files.Length > 0);
         foreach (var file in files)
         {
            Console.WriteLine("\tFile: " + file);
            Assert.IsFalse(System.IO.Path.IsPathRooted(file), "IsPathRooted of return and argument types should match.");
         }


         Console.WriteLine("\nAbsolute Paths\n");
         files = Alphaleonis.Win32.Filesystem.Directory.GetFiles(tempPath);
         Assert.IsTrue(files.Length > 0);
         foreach (var file in files)
         {
            Console.WriteLine("\tFile: " + file);
            Assert.IsTrue(System.IO.Path.IsPathRooted(file), "IsPathRooted of return and argument types should match.");
         }


         Console.WriteLine();
      }
   }
}
