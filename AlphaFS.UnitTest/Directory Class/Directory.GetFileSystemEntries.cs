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
      public void Directory_GetFileSystemEntries_LocalAndNetwork_Success()
      {
         Directory_GetFileSystemEntries(false);
         Directory_GetFileSystemEntries(true);
      }


      private void Directory_GetFileSystemEntries(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         var inputPath = Environment.SystemDirectory;
         if (isNetwork)
            inputPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(inputPath);


         Console.WriteLine("Input Directory Path: [{0}]\n", inputPath);


         var systemIOCount = System.IO.Directory.GetFileSystemEntries(inputPath).Length;
         var alphaFSCount = Alphaleonis.Win32.Filesystem.Directory.GetFileSystemEntries(inputPath).Length;

         Console.WriteLine("\tSystem.IO file system entries get: {0:N0}", systemIOCount);
         Console.WriteLine("\tAlphaFS file system entries get  : {0:N0}", alphaFSCount);


         Assert.AreEqual(systemIOCount, alphaFSCount, "No file system entries get, but it is expected.");


         Console.WriteLine();
      }
   }
}
