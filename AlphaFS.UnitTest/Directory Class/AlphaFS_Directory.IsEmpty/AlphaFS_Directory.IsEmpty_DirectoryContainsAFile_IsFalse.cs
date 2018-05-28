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
   public partial class AlphaFS_Directory_IsEmptyTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void AlphaFS_Directory_IsEmpty_DirectoryContainsAFile_IsFalse_LocalAndNetwork_Success()
      {
         AlphaFS_Directory_IsEmpty_DirectoryContainsAFile_IsFalse(false);
         AlphaFS_Directory_IsEmpty_DirectoryContainsAFile_IsFalse(true);
      }



      private void AlphaFS_Directory_IsEmpty_DirectoryContainsAFile_IsFalse(bool isNetwork)
      {
         using (var tempRoot = new TemporaryDirectory(isNetwork))
         {
            var folder = System.IO.Directory.CreateDirectory(System.IO.Path.Combine(tempRoot.Directory.FullName, "Empty Folder"));

            Console.WriteLine("Input Directory Path: [{0}]", folder.FullName);


            // Create file and test again.
            var file = System.IO.Path.Combine(folder.FullName, "a_file.txt");

            using (System.IO.File.Create(file))
               Console.WriteLine("\n\tCreated File: [{0}]", file);


            var isEmpty = Alphaleonis.Win32.Filesystem.Directory.IsEmpty(folder.FullName);
            Console.WriteLine("\n\tFolder is empty: [{0}]", isEmpty);


            Assert.IsFalse(isEmpty, "It is expected that the folder is not empty.");
         }

         Console.WriteLine();
      }
   }
}
