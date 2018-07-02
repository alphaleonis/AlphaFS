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
using System.Diagnostics;

namespace AlphaFS.UnitTest
{
   public partial class CopyTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void AlphaFS_Directory_Copy_Retry_Fails3Times_LocalAndNetwork()
      {
         AlphaFS_Directory_Copy_Retry_Fails3Times(false);
         AlphaFS_Directory_Copy_Retry_Fails3Times(true);
      }


      private void AlphaFS_Directory_Copy_Retry_Fails3Times(bool isNetwork)
      {
         using (var tempRoot = new TemporaryDirectory(isNetwork))
         {
            var folderSrc = tempRoot.CreateDirectory().FullName;
            var folderDst = tempRoot.CreateDirectory().FullName;

            Console.WriteLine("Destination Directory Path: [{0}]", folderDst);


            // Set destination folder read-only attribute so that a System.UnauthorizedAccessException is triggered on folder copy.

            var existingFileSrc = new System.IO.FileInfo(System.IO.Path.Combine(folderSrc, "ExistingFile.txt"));
            var existingFileDst = new System.IO.FileInfo(System.IO.Path.Combine(folderDst, "ExistingFile.txt"));

            using (existingFileSrc.Create()) { }
            using (existingFileDst.Create()) { }

            System.IO.File.SetAttributes(existingFileDst.FullName, System.IO.FileAttributes.ReadOnly);


            // Copy folder with retry enabled.

            var retry = 3;
            var retryTimeout = 3;


            var sw = Stopwatch.StartNew();

            UnitTestAssert.ThrowsException<UnauthorizedAccessException>(() => Alphaleonis.Win32.Filesystem.Directory.Copy(retry, retryTimeout, folderSrc, folderDst, Alphaleonis.Win32.Filesystem.CopyOptions.None));

            sw.Stop();


            var waitTime = retry * retryTimeout;

            Console.WriteLine("\n\tTotal wait time: retry * retryTimeout = [{0}] seconds.", waitTime);


            Assert.AreEqual(waitTime, sw.Elapsed.Seconds);
         }
         
         Console.WriteLine();
      }
   }
}
