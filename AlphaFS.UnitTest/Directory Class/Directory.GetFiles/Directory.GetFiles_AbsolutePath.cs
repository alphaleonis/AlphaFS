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

namespace AlphaFS.UnitTest
{
   partial class DirectoryTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void Directory_GetFiles_AbsolutePath_LocalAndNetwork_Success()
      {
         Directory_GetFiles_AbsolutePath(false);
         Directory_GetFiles_AbsolutePath(true);
      }


      private void Directory_GetFiles_AbsolutePath(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         var tempPath = UnitTestConstants.SysRoot;
         if (isNetwork)
            tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);

         Console.WriteLine("\nInput Directory Path: [{0}]", tempPath);


         var systemIOFiles = System.IO.Directory.GetFiles(tempPath).OrderBy(path => path).ToArray();
         var alphaFSFiles = Alphaleonis.Win32.Filesystem.Directory.GetFiles(tempPath).OrderBy(path => path).ToArray();

         Assert.AreEqual(systemIOFiles.Length, alphaFSFiles.Length);

         
         var fileCount = 0;
         for (var i = 0; i < systemIOFiles.Length; i++)
         {
            Console.WriteLine("\t#{0:000}\t{1}", ++fileCount, alphaFSFiles[i]);

            Assert.AreEqual(systemIOFiles[i], alphaFSFiles[i]);
         }


         Console.WriteLine();
      }
   }
}
