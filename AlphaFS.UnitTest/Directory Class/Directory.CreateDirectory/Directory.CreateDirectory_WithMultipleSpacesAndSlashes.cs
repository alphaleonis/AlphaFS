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
      public void Directory_CreateDirectory_WithMultipleSpacesAndSlashes_LocalAndNetwork_Success()
      {
         Directory_CreateDirectory_WithMultipleSpacesAndSlashes(false);
         Directory_CreateDirectory_WithMultipleSpacesAndSlashes(true);
      }


      private void Directory_CreateDirectory_WithMultipleSpacesAndSlashes(bool isNetwork)
      {
         using (var tempRoot = new TemporaryDirectory(isNetwork))
         {
            var folder = tempRoot.Directory.FullName;
            var subFolders = new[]
            {
               @"f�lder1",
               @"\ \",
               @"fold�r2 2",
               @"///",
               @" f�ld�r3 33"
            };


            var fullPath = folder + @"\" + subFolders[0] + subFolders[1] + subFolders[2] + subFolders[3] + subFolders[4];

            Console.WriteLine("Input Directory Path: [{0}]\n", fullPath);


            Alphaleonis.Win32.Filesystem.Directory.CreateDirectory(fullPath);


            var count = 0;
            foreach (var dir in Alphaleonis.Win32.Filesystem.Directory.EnumerateFileSystemEntryInfos<Alphaleonis.Win32.Filesystem.DirectoryInfo>(folder, Alphaleonis.Win32.Filesystem.DirectoryEnumerationOptions.Recursive))
            {
               Console.WriteLine("\tFolder name: \"{0}\"", dir.Name);

               switch (count)
               {
                  case 0:
                     Assert.IsTrue(dir.Name.Equals(subFolders[0]));
                     break;

                  case 1:
                     Assert.IsTrue(dir.Name.Equals(subFolders[2]));
                     break;

                  case 2:
                     Assert.IsTrue(dir.Name.Equals(subFolders[4]));
                     break;
               }

               count++;
            }
         }

         Console.WriteLine();
      }
   }
}
