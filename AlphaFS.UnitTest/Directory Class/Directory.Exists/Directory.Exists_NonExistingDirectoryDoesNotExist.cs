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
   public partial class Directory_ExistsTest
   {
      [TestMethod]
      public void Directory_Exists_NonExistingDirectoryDoesNotExist_Success()
      {
         Directory_Exists_NonExistingDirectoryDoesNotExist(false);
         Directory_Exists_NonExistingDirectoryDoesNotExist(true);
      }


      private void Directory_Exists_NonExistingDirectoryDoesNotExist(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         var tempPath = System.IO.Path.GetTempPath();
         if (isNetwork)
            tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);


         using (var rootDir = new TemporaryDirectory(tempPath, MethodBase.GetCurrentMethod().Name))
         {
            var folder = rootDir.RandomDirectoryFullPath;

            var shouldBe = false;
            var existSysIO = System.IO.Directory.Exists(folder);
            var existAlpha = Alphaleonis.Win32.Filesystem.Directory.Exists(folder);
            Console.WriteLine("\nInput Non-Existing Directory Path: [{0}]", folder);

            Assert.AreEqual(shouldBe, existSysIO, "The result should be: " + shouldBe.ToString().ToUpperInvariant());
            Assert.AreEqual(existSysIO, existAlpha, "The results are not equal, but are expected to be.");
         }


         Console.WriteLine();
      }
   }
}
