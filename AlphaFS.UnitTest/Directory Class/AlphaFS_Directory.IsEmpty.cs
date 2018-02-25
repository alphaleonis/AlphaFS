/*  Copyright (C) 2008-2017 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
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
using System.Reflection;

namespace AlphaFS.UnitTest
{
   partial class DirectoryTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void AlphaFS_Directory_IsEmpty_LocalAndNetwork_Success()
      {
         Directory_IsEmpty(false);
         Directory_IsEmpty(true);
      }


      [TestMethod]
      public void AlphaFS_Directory_IsEmpty_ContainsAFile_IsFalse_LocalAndNetwork_Success()
      {
         Directory_IsEmpty_ContainsAFile_IsFalse(false);
         Directory_IsEmpty_ContainsAFile_IsFalse(true);
      }




      private void Directory_IsEmpty(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         var tempPath = System.IO.Path.GetTempPath();
         if (isNetwork)
            tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);


         using (var rootDir = new TemporaryDirectory(tempPath, MethodBase.GetCurrentMethod().Name))
         {
            var folder = System.IO.Directory.CreateDirectory(System.IO.Path.Combine(rootDir.Directory.FullName, "Empty Folder"));
            Console.WriteLine("\nInput Directory Path: [{0}]", folder.FullName);


            Assert.IsTrue(Alphaleonis.Win32.Filesystem.Directory.IsEmpty(folder.FullName), "It is expected that the folder is empty.");
         }

         Console.WriteLine();
      }


      private void Directory_IsEmpty_ContainsAFile_IsFalse(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         var tempPath = System.IO.Path.GetTempPath();
         if (isNetwork)
            tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);


         using (var rootDir = new TemporaryDirectory(tempPath, MethodBase.GetCurrentMethod().Name))
         {
            var folder = System.IO.Directory.CreateDirectory(System.IO.Path.Combine(rootDir.Directory.FullName, "Empty Folder"));
            Console.WriteLine("\nInput Directory Path: [{0}]", folder.FullName);


            // Create file and test again.
            var file = System.IO.Path.Combine(folder.FullName, "a_file.txt");

            using (System.IO.File.Create(file))
               Console.WriteLine("\n\tCreated File: [{0}]", file);


            Assert.IsFalse(Alphaleonis.Win32.Filesystem.Directory.IsEmpty(folder.FullName), "It is expected that the folder is not empty.");
         }

         Console.WriteLine();
      }
   }
}
