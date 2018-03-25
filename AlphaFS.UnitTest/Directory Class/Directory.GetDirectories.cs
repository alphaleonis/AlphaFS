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
      public void Directory_GetDirectories_LocalAndNetwork_Success()
      {
         Directory_GetDirectories(false);
         Directory_GetDirectories(true);
      }


      [TestMethod]
      public void Directory_GetDirectories_WithSearchPattern_LocalAndNetwork_Success()
      {
         Directory_GetDirectories_WithSearchPattern(true);
         Directory_GetDirectories_WithSearchPattern(false);
      }


      [TestMethod]
      public void Directory_GetDirectories_AbsoluteAndRelativePath_Local_Success()
      {
         Directory_GetDirectories_AbsoluteAndRelativePath_Success(true);
         Directory_GetDirectories_AbsoluteAndRelativePath_Success(false);
      }




      private void Directory_GetDirectories(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);
         Console.WriteLine();


         var inputPath = UnitTestConstants.SysRoot;
         if (isNetwork)
            inputPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(inputPath);


         Console.WriteLine("Input Directory Path: [{0}]\n", inputPath);


         var systemIOCount = System.IO.Directory.GetDirectories(inputPath).Length;
         var alphaFSCount = Alphaleonis.Win32.Filesystem.Directory.GetDirectories(inputPath).Length;

         Console.WriteLine("\tSystem.IO directories get: {0:N0}", systemIOCount);
         Console.WriteLine("\tAlphaFS directories get  : {0:N0}", alphaFSCount);


         Assert.AreEqual(systemIOCount, alphaFSCount, "No directories get, but it is expected.");


         Console.WriteLine();
      }


      private void Directory_GetDirectories_WithSearchPattern(bool isLocal)
      {
         #region Setup

         UnitTestConstants.PrintUnitTestHeader(!isLocal);

         var tempPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "Directory.GetDirectories_With_SearchPattern()-" + UnitTestConstants.GetRandomFileNameWithDiacriticCharacters());
         if (!isLocal) tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);

         Console.WriteLine("\nInput Directory Path: [{0}]\n", tempPath);

         System.IO.Directory.CreateDirectory(tempPath);
         System.IO.Directory.CreateDirectory(System.IO.Path.Combine(tempPath, "a.txt"));
         System.IO.Directory.CreateDirectory(System.IO.Path.Combine(tempPath, "aa.txt"));
         System.IO.Directory.CreateDirectory(System.IO.Path.Combine(tempPath, "aba.txt"));
         System.IO.Directory.CreateDirectory(System.IO.Path.Combine(tempPath, "foo.txt"));
         System.IO.Directory.CreateDirectory(System.IO.Path.Combine(tempPath, "footxt"));

         #endregion // Setup

         try
         {
            var folders = Alphaleonis.Win32.Filesystem.Directory.GetDirectories(tempPath, "foo.txt");
            Console.WriteLine("\tDirectory.GetDirectories(tempPath, \"foo.txt\");");
            Assert.IsTrue(folders.Contains(System.IO.Path.Combine(tempPath, "foo.txt"), StringComparer.InvariantCultureIgnoreCase));
            Assert.IsFalse(folders.Contains(System.IO.Path.Combine(tempPath, "fooatxt"), StringComparer.InvariantCultureIgnoreCase));

            folders = Alphaleonis.Win32.Filesystem.Directory.GetDirectories(tempPath, "a?a.txt");
            Console.WriteLine("\tDirectory.GetDirectories(tempPath, \"a?a.txt\");");
            Assert.IsTrue(folders.Contains(System.IO.Path.Combine(tempPath, "aba.txt"), StringComparer.InvariantCultureIgnoreCase), "? wildcard failed");
            Assert.IsFalse(folders.Contains(System.IO.Path.Combine(tempPath, "aa.txt"), StringComparer.InvariantCultureIgnoreCase), "? wildcard failed");

            folders = Alphaleonis.Win32.Filesystem.Directory.GetDirectories(tempPath, "a*.*");
            Console.WriteLine("\tDirectory.GetDirectories(tempPath, \"a*.*\");");
            Assert.IsTrue(folders.Contains(System.IO.Path.Combine(tempPath, "a.txt"), StringComparer.InvariantCultureIgnoreCase), "* wildcard failed");
            Assert.IsTrue(folders.Contains(System.IO.Path.Combine(tempPath, "aa.txt"), StringComparer.InvariantCultureIgnoreCase), "* wildcard failed");
            Assert.IsTrue(folders.Contains(System.IO.Path.Combine(tempPath, "aba.txt"), StringComparer.InvariantCultureIgnoreCase), "* wildcard failed");

            folders = Alphaleonis.Win32.Filesystem.Directory.GetDirectories(tempPath, "*.*");
            Console.WriteLine("\tDirectory.GetDirectories(tempPath, \"*.*\");");
            var folders2 = Alphaleonis.Win32.Filesystem.Directory.GetDirectories(tempPath);
            Assert.IsTrue(folders.Length == folders2.Length, "*.* failed");
            folders = Alphaleonis.Win32.Filesystem.Directory.GetDirectories(tempPath, "*.*.*");
            Console.WriteLine("\tDirectory.GetDirectories(tempPath, \"*.*.*\");");
            Assert.IsTrue(folders.Length == folders2.Length, "*.* failed");
         }
         finally
         {
            System.IO.Directory.Delete(tempPath, true);
            Assert.IsFalse(System.IO.Directory.Exists(tempPath), "Cleanup failed: Directory should have been removed.");
            Console.WriteLine();
         }
      }


      private void Directory_GetDirectories_AbsoluteAndRelativePath_Success(bool isLocal)
      {
         UnitTestConstants.PrintUnitTestHeader(!isLocal);

         var tempPath = UnitTestConstants.SysRoot32;
         if (!isLocal) tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);

         Assert.IsTrue(System.IO.Path.IsPathRooted(tempPath));
         Environment.CurrentDirectory = tempPath;

         Console.WriteLine("\nInput Directory Path: [{0}]", tempPath);


         Console.WriteLine("\nRelative Paths\n");
         var folders = Alphaleonis.Win32.Filesystem.Directory.GetDirectories(".");
         Assert.IsTrue(folders.Length > 0);
         foreach (var folder in folders)
         {
            Console.WriteLine("\tDirectory: " + folder);
            Assert.IsFalse(System.IO.Path.IsPathRooted(folder), "IsPathRooted of return and argument types should match.");
         }


         Console.WriteLine("\nAbsolute Paths\n");
         folders = Alphaleonis.Win32.Filesystem.Directory.GetDirectories(tempPath);
         Assert.IsTrue(folders.Length > 0);
         foreach (var folder in folders)
         {
            Console.WriteLine("\tDirectory: " + folder);
            Assert.IsTrue(System.IO.Path.IsPathRooted(folder), "IsPathRooted of return and argument types should match.");
         }


         Console.WriteLine();
      }
   }
}
