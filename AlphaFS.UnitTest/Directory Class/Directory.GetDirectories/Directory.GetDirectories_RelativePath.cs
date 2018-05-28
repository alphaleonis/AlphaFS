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
using System.Reflection;

namespace AlphaFS.UnitTest
{
   partial class DirectoryTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void Directory_GetDirectories_RelativePath_LocalAndNetwork_Success()
      {
         Directory_GetDirectories_RelativePath(false);
         Directory_GetDirectories_RelativePath(true);
      }


      private void Directory_GetDirectories_RelativePath(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         using (var tempRoot = new TemporaryDirectory(isNetwork ? Alphaleonis.Win32.Filesystem.Path.LocalToUnc(UnitTestConstants.TempPath) : UnitTestConstants.TempPath, MethodBase.GetCurrentMethod().Name))
         {
            var folder = new System.IO.DirectoryInfo(tempRoot.RandomDirectoryFullPath);

            var currentDirectory = tempRoot.Directory.Parent.FullName;
            Environment.CurrentDirectory = currentDirectory;

            Console.WriteLine("\nInput Directory Path: [{0}]\n", currentDirectory);
            
            UnitTestConstants.CreateDirectoriesAndFiles(folder.FullName, 5, true, true, true);


            var relativeFolder = folder.Parent.Name + @"\" + folder.Name;


            var systemIOCollection = System.IO.Directory.GetDirectories(relativeFolder, "*", System.IO.SearchOption.AllDirectories);

            var alphaFSCollection = Alphaleonis.Win32.Filesystem.Directory.GetDirectories(relativeFolder, "*", System.IO.SearchOption.AllDirectories);


            var folderCount = 0;
            foreach (var fso in alphaFSCollection)
               Console.WriteLine("\t#{0:000}\t{1}", ++folderCount, fso);


            CollectionAssert.AreEquivalent(systemIOCollection, alphaFSCollection);
         }

         Console.WriteLine();
      }
   }
}
