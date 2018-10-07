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
   partial class FileTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void File_SetAttributes_LocalAndNetwork_Success()
      {
         File_SetAttributes(false);
         File_SetAttributes(true);
      }
      

      private void File_SetAttributes(bool isNetwork)
      {
         using (var tempRoot = new TemporaryDirectory(isNetwork))
         {
            var folder = tempRoot.CreateRecursiveTree(5);

            Console.WriteLine("Input Directory Path: [{0}]", folder);
            

            foreach (var fullPath in System.IO.Directory.EnumerateFileSystemEntries(folder.FullName, "*", System.IO.SearchOption.AllDirectories))
            {
               System.IO.File.SetAttributes(fullPath, System.IO.FileAttributes.Normal);

               const System.IO.FileAttributes attributes = System.IO.FileAttributes.Hidden | System.IO.FileAttributes.Archive | System.IO.FileAttributes.System | System.IO.FileAttributes.ReadOnly;


               Alphaleonis.Win32.Filesystem.File.SetAttributes(fullPath, attributes);


               var sysIO = System.IO.File.GetAttributes(fullPath);

               // Remove Directory attribute for comparing.
               var noDir = sysIO;
               noDir &= ~System.IO.FileAttributes.Directory;
               

               Assert.AreEqual(attributes, noDir);

               Assert.AreEqual(sysIO, Alphaleonis.Win32.Filesystem.File.GetAttributes(fullPath));
            }
         }

         Console.WriteLine();
      }
   }
}
