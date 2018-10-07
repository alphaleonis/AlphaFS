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
   public partial class Directory_CreateDirectoryTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void Directory_CreateDirectory_And_Delete_CreatesStructureWithoutFiles_LocalAndNetwork_Success()
      {
         Directory_CreateDirectory_And_Delete_CreatesStructureWithoutFiles(false);
         Directory_CreateDirectory_And_Delete_CreatesStructureWithoutFiles(true);
      }


      private void Directory_CreateDirectory_And_Delete_CreatesStructureWithoutFiles(bool isNetwork)
      {
         using (var tempRoot = new TemporaryDirectory(isNetwork))
         {
            var folder = tempRoot.Directory.FullName;

            Console.WriteLine("Input Directory Path: [{0}]\n", folder);


            // Directory depth level.
            var level = new Random().Next(10, 500);

#if NET35
            // MSDN: .NET 4+ Trailing spaces are removed from the end of the path parameter before deleting the directory.
            folder += "\u3000"; // EMspace
#endif

            var root = folder;

            for (var i = 0; i < level; i++)
            {
               var isEven = i % 2 == 0;
               root = System.IO.Path.Combine(root, (isEven ? "Level-" : "Lëvél-") + (i + 1) + (isEven ? "-subFolder" : "-sübFôldér"));
            }

            Alphaleonis.Win32.Filesystem.Directory.CreateDirectory(root);

            Console.WriteLine("\tCreated directory structure: Depth: [{0}], path length: [{1}] characters.", level.ToString(), root.Length.ToString());
            Console.WriteLine();

            Console.WriteLine("\t{0}", root);
            Console.WriteLine();

            Assert.IsTrue(Alphaleonis.Win32.Filesystem.Directory.Exists(root), "The directory does not exists, but is expected to.");


            Alphaleonis.Win32.Filesystem.Directory.Delete(folder, true);

            Assert.IsFalse(System.IO.Directory.Exists(folder), "The directory exists, but is expected not to.");
         }

         Console.WriteLine();
      }
   }
}
