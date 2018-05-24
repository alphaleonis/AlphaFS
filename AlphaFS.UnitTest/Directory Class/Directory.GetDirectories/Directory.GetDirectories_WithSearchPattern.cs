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
using System.Reflection;

namespace AlphaFS.UnitTest
{
   partial class DirectoryTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void Directory_GetDirectories_WithSearchPattern_LocalAndNetwork_Success()
      {
         Directory_GetDirectories_WithSearchPattern(false);
         Directory_GetDirectories_WithSearchPattern(true);
      }


      private void Directory_GetDirectories_WithSearchPattern(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         var tempPath = System.IO.Path.GetTempPath();
         if (isNetwork)
            tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);


         using (var rootDir = new TemporaryDirectory(tempPath, MethodBase.GetCurrentMethod().Name))
         {
            var folder = new System.IO.DirectoryInfo(rootDir.RandomDirectoryFullPath).FullName;
            Console.WriteLine("\nInput Directory Path: [{0}]", folder);


            System.IO.Directory.CreateDirectory(System.IO.Path.Combine(folder, "a.txt"));
            System.IO.Directory.CreateDirectory(System.IO.Path.Combine(folder, "aa.txt"));
            System.IO.Directory.CreateDirectory(System.IO.Path.Combine(folder, "aba.txt"));
            System.IO.Directory.CreateDirectory(System.IO.Path.Combine(folder, "foo.txt"));
            System.IO.Directory.CreateDirectory(System.IO.Path.Combine(folder, "footxt"));


            var folders = Alphaleonis.Win32.Filesystem.Directory.GetDirectories(folder, "foo.txt");

            Assert.IsTrue(folders.Contains(System.IO.Path.Combine(folder, "foo.txt"), StringComparer.InvariantCultureIgnoreCase));
            Assert.IsFalse(folders.Contains(System.IO.Path.Combine(folder, "fooatxt"), StringComparer.InvariantCultureIgnoreCase));


            folders = Alphaleonis.Win32.Filesystem.Directory.GetDirectories(folder, "a?a.txt");

            Assert.IsTrue(folders.Contains(System.IO.Path.Combine(folder, "aba.txt"), StringComparer.InvariantCultureIgnoreCase), "? wildcard failed");
            Assert.IsFalse(folders.Contains(System.IO.Path.Combine(folder, "aa.txt"), StringComparer.InvariantCultureIgnoreCase), "? wildcard failed");


            folders = Alphaleonis.Win32.Filesystem.Directory.GetDirectories(folder, "a*.*");

            Assert.IsTrue(folders.Contains(System.IO.Path.Combine(folder, "a.txt"), StringComparer.InvariantCultureIgnoreCase), "* wildcard failed");
            Assert.IsTrue(folders.Contains(System.IO.Path.Combine(folder, "aa.txt"), StringComparer.InvariantCultureIgnoreCase), "* wildcard failed");
            Assert.IsTrue(folders.Contains(System.IO.Path.Combine(folder, "aba.txt"), StringComparer.InvariantCultureIgnoreCase), "* wildcard failed");


            folders = Alphaleonis.Win32.Filesystem.Directory.GetDirectories(folder, "*.*");
            var folders2 = Alphaleonis.Win32.Filesystem.Directory.GetDirectories(folder);

            Assert.IsTrue(folders.Length == folders2.Length, "*.* failed");


            folders = Alphaleonis.Win32.Filesystem.Directory.GetDirectories(folder, "*.*.*");

            Assert.IsTrue(folders.Length == folders2.Length, "*.* failed");
         }

         Console.WriteLine();
      }
   }
}
