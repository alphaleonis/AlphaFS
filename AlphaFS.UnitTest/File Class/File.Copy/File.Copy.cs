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
   public partial class CopyTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void File_Copy_LocalAndNetwork_Success()
      {
         File_Copy(false);
         File_Copy(true);
      }


      private void File_Copy(bool isNetwork)
      {
         using (var tempRoot = new TemporaryDirectory(isNetwork))
         {
            var fileSource = tempRoot.CreateFileRandomizedAttributes();
            var fileCopy = tempRoot.RandomTxtFileFullPath;

            Console.WriteLine("Src File Path: [{0}] [{1}]", Alphaleonis.Utils.UnitSizeToText(fileSource.Length), fileSource.FullName);
            Console.WriteLine("Dst File Path: [{0}]", fileCopy);
            

            var cmr = Alphaleonis.Win32.Filesystem.File.Copy(fileSource.FullName, fileCopy, Alphaleonis.Win32.Filesystem.PathFormat.FullPath);

            UnitTestConstants.Dump(cmr);


            Assert.IsTrue(System.IO.File.Exists(fileCopy), "The file does not exists, but is expected to.");
            
            Assert.AreEqual(fileSource.Length, new System.IO.FileInfo(fileCopy).Length, "The file sizes do no match, but are expected to.");

            Assert.IsTrue(System.IO.File.Exists(fileSource.FullName), "The original file does not exist, but is expected to.");

            Assert.AreEqual(0, cmr.Retries);
         }
         
         Console.WriteLine();
      }
   }
}
