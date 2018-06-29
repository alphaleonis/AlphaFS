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
      public void AlphaFS_File_Copy_Retry_OverwriteExistingReadOnlyDestinationFile_LocalAndNetwork()
      {
         AlphaFS_File_Copy_Retry_OverwriteExistingReadOnlyDestinationFile(false);
         AlphaFS_File_Copy_Retry_OverwriteExistingReadOnlyDestinationFile(true);
      }


      private void AlphaFS_File_Copy_Retry_OverwriteExistingReadOnlyDestinationFile(bool isNetwork)
      {
         using (var tempRoot = new TemporaryDirectory(isNetwork))
         {
            var fileSrc = tempRoot.CreateFile().FullName;
            var fileDst = tempRoot.CreateFile().FullName;

            Console.WriteLine("Destination File Path: [{0}]", fileDst);


            // Set destination file read-only attribute so that a System.UnauthorizedAccessException is triggered.

            System.IO.File.SetAttributes(fileDst, System.IO.FileAttributes.ReadOnly);


            // Copy file with retry enabled.

            var retry = 3;
            var retryTimeout = 3;


            var sw = Stopwatch.StartNew();

            UnitTestAssert.ThrowsException<UnauthorizedAccessException>(() => Alphaleonis.Win32.Filesystem.File.Copy(retry, retryTimeout, fileSrc, fileDst, Alphaleonis.Win32.Filesystem.CopyOptions.None));

            sw.Stop();


            var waitTime = retry * retryTimeout;

            Console.WriteLine("\n\tTotal wait time: retry * retryTimeout = [{0}] seconds.", waitTime);


            Assert.IsTrue(sw.Elapsed.Seconds >= waitTime);
         }
         
         Console.WriteLine();
      }
   }
}
