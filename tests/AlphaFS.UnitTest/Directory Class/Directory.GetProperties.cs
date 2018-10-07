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
   partial class DirectoryTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void AlphaFS_Directory_GetProperties_LocalAndNetwork_Success()
      {
         AlphaFS_Directory_GetProperties(false);
         AlphaFS_Directory_GetProperties(true);
      }


      private void AlphaFS_Directory_GetProperties(bool isNetwork)
      {
         using (var tempRoot = new TemporaryDirectory(isNetwork))
         {
            var folder = tempRoot.CreateRecursiveRandomizedAttributesTree(5);

            Console.WriteLine("Aggregated properties of file system objects from Directory: [{0}]\n", folder);


            var props = Alphaleonis.Win32.Filesystem.Directory.GetProperties(folder.FullName, Alphaleonis.Win32.Filesystem.DirectoryEnumerationOptions.FilesAndFolders | Alphaleonis.Win32.Filesystem.DirectoryEnumerationOptions.Recursive);


            var total = props["Total"];
            var file = props["File"];
            var size = props["Size"];
            var cnt = 0;

            foreach (var key in props.Keys)
               Console.WriteLine("\t\t#{0:000}\t{1, -17} = [{2}]", ++cnt, key, props[key]);


            Assert.AreEqual(60, total, "The number of file system objects does not match, but it is expected.");

            Assert.AreEqual(30, file, "The number of files does not match, but it is expected.");

            Assert.AreNotEqual(0, size, "The total file system objects size is zero, which is not expected.");
         }

         Console.WriteLine();
      }
   }
}
