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
   public partial class AlphaFS_Directory_CountFileSystemObjectsTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void AlphaFS_Directory_CountFileSystemObjects_FilesOnly_NonRecursive_LocalAndNetwork_Success()
      {
         AlphaFS_Directory_CountFileSystemObjects_FilesOnly_NonRecursive(false);
         AlphaFS_Directory_CountFileSystemObjects_FilesOnly_NonRecursive(true);
      }


      private void AlphaFS_Directory_CountFileSystemObjects_FilesOnly_NonRecursive(bool isNetwork)
      {
         using (var tempRoot = new TemporaryDirectory(isNetwork))
         {
            const int maxFso = 10;
            const int expectedFso = maxFso / 2;

            var folder = tempRoot.CreateTree(maxFso);

            Console.WriteLine("Input Directory Path: [{0}]", folder.FullName);


            var fsoCount = Alphaleonis.Win32.Filesystem.Directory.CountFileSystemObjects(folder.FullName, "*", Alphaleonis.Win32.Filesystem.DirectoryEnumerationOptions.Files);


            Console.WriteLine("\n\tTotal file system objects: [{0}]", fsoCount);

            Assert.AreEqual(expectedFso, fsoCount, string.Format(CultureInfo.InvariantCulture, "The number of file system objects: {0} is not equal than expected: {1}", expectedFso, fsoCount));
         }

         Console.WriteLine();
      }
   }
}
