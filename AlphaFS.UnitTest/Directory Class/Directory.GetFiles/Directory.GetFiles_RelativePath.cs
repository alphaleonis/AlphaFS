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
      public void Directory_GetFiles_RelativePath_LocalAndNetwork_Success()
      {
         Directory_GetFiles_RelativePath(false);
         Directory_GetFiles_RelativePath(true);
      }


      private void Directory_GetFiles_RelativePath(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         var tempPath = System.IO.Path.GetTempPath();
         if (isNetwork)
            tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);


         using (var rootDir = new TemporaryDirectory(tempPath, MethodBase.GetCurrentMethod().Name))
         {
            var folder = new System.IO.DirectoryInfo(rootDir.RandomDirectoryFullPath);
            Console.WriteLine("\nInput Directory Path: [{0}]\n", tempPath);

            UnitTestConstants.CreateDirectoriesAndFiles(folder.FullName, 10, false, false, false);

            Environment.CurrentDirectory = tempPath;


            var relativeFolder = folder.Parent.Name + @"\" + folder.Name;

            var systemIOFiles = System.IO.Directory.GetFiles(relativeFolder, "*", System.IO.SearchOption.AllDirectories).OrderBy(path => path).ToArray();
            var alphaFSFiles = Alphaleonis.Win32.Filesystem.Directory.GetFiles(relativeFolder, "*", System.IO.SearchOption.AllDirectories).OrderBy(path => path).ToArray();

            Assert.AreEqual(systemIOFiles.Length, alphaFSFiles.Length);


            var fileCount = 0;
            for (var i = 0; i < systemIOFiles.Length; i++)
            {
               Console.WriteLine("\t#{0:000}\t{1}", ++fileCount, alphaFSFiles[i]);

               Assert.AreEqual(systemIOFiles[i], alphaFSFiles[i]);
            }
         }

         Console.WriteLine();
      }
   }
}
