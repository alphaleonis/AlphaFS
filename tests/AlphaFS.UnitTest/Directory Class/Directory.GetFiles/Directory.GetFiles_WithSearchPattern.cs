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
using System.Globalization;

namespace AlphaFS.UnitTest
{
   partial class DirectoryTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void Directory_GetFiles_WithSearchPattern_LocalAndNetwork_Success()
      {
         Directory_GetFiles_WithSearchPattern(false);
         Directory_GetFiles_WithSearchPattern(true);
      }


      private void Directory_GetFiles_WithSearchPattern(bool isNetwork)
      {
         using (var tempRoot = new TemporaryDirectory(isNetwork))
         {
            var folder = tempRoot.CreateDirectory();

            Console.WriteLine("Input Directory Path: [{0}]\n", folder.FullName);
            
            var count = 0;
            var folders = new[]
            {
               tempRoot.RandomString, tempRoot.RandomDirectoryName, tempRoot.RandomTxtFileName, tempRoot.RandomString, tempRoot.RandomDirectoryName,
               tempRoot.RandomTxtFileName, tempRoot.RandomString, tempRoot.RandomDirectoryName, tempRoot.RandomTxtFileName, tempRoot.RandomString
            };


            foreach (var folderName in folders)
            {
               var newFolder = System.IO.Path.Combine(folder.FullName, folderName);

               System.IO.File.AppendAllText(newFolder + (count++ % 2 == 0 ? string.Empty : "-uneven"), DateTime.Now.ToString(CultureInfo.CurrentCulture));
            }


            var folderCount = 0;

            foreach (var folderResult in folders)
            {
               var systemIOCollection = System.IO.Directory.GetFiles(folder.FullName, folderResult, System.IO.SearchOption.AllDirectories);

               var alphaFSCollection = Alphaleonis.Win32.Filesystem.Directory.GetFiles(folder.FullName, folderResult, System.IO.SearchOption.AllDirectories);

               Console.WriteLine("\t#{0:000}\t{1}", ++folderCount, folderResult);
               
               CollectionAssert.AreEquivalent(systemIOCollection, alphaFSCollection);
            }
         }

         Console.WriteLine();
      }
   }
}
