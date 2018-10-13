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
using System.Globalization;
using System.Threading;

namespace AlphaFS.UnitTest
{
   public partial class CopyTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void AlphaFS_Directory_Copy_RetryFails1Time_ThenSucceeds_LocalAndNetwork()
      {
         AlphaFS_Directory_Copy_RetryFails1Time_ThenSucceeds(false);
         AlphaFS_Directory_Copy_RetryFails1Time_ThenSucceeds(true);
      }


      private void AlphaFS_Directory_Copy_RetryFails1Time_ThenSucceeds(bool isNetwork)
      {
         using (var tempRoot = new TemporaryDirectory(isNetwork))
         {
            var folderSrc = tempRoot.CreateDirectory().FullName;
            var folderDst = tempRoot.CreateDirectory().FullName;

            Console.WriteLine("Destination Directory Path: [{0}]\n", folderDst);

            
            var existingFileSrc = System.IO.Path.Combine(folderSrc, "ExistingFile.txt");
            var existingFileDst = System.IO.Path.Combine(folderDst, "ExistingFile.txt");

            System.IO.File.WriteAllText(existingFileSrc, DateTime.Now.ToString(CultureInfo.CurrentCulture));
            System.IO.File.WriteAllText(existingFileDst, DateTime.Now.ToString(CultureInfo.CurrentCulture));

            
            // Set destination file read-only attribute so that a System.UnauthorizedAccessException is triggered on folder copy.

            System.IO.File.SetAttributes(existingFileDst, System.IO.FileAttributes.ReadOnly);




            // Copy folder with retry enabled.

            var errorCount = 0;

            using (var cancelSource = new CancellationTokenSource())
            {
               var filters = new Alphaleonis.Win32.Filesystem.DirectoryEnumerationFilters
               {
                  // Used to abort the enumeration.
                  CancellationToken = cancelSource.Token,
                  
                  ErrorRetry = 2,

                  ErrorRetryTimeout = 3,

                  ErrorFilter = delegate(int errorCode, string errorMessage, string pathProcessed)
                  {
                     // Report Exception.
                     Console.WriteLine("\tErrorFilter: Attempt #{0:N0}: ({1}) {2}: [{3}]", ++errorCount, errorCode, errorMessage, pathProcessed);

                     if (errorCount == 2)
                     {
                        System.IO.File.SetAttributes(pathProcessed, System.IO.FileAttributes.Normal);
                     }

                     // Return true to continue, false to throw the Exception.
                     return true;
                  }
               };


               Alphaleonis.Win32.Filesystem.CopyMoveResult cmr = null;
               var sw = Stopwatch.StartNew();

               try
               {
                  cmr = Alphaleonis.Win32.Filesystem.Directory.Copy(folderSrc, folderDst, Alphaleonis.Win32.Filesystem.CopyOptions.None, filters);
               }
               catch { }

               sw.Stop();


               UnitTestConstants.Dump(cmr);

               Assert.IsNotNull(cmr);
               Assert.AreEqual(0, cmr.ErrorCode);
               Assert.AreEqual(2, cmr.Retries);


               var waitTime = filters.ErrorRetry * filters.ErrorRetryTimeout;

               Assert.AreEqual(6, waitTime, "The timeout is not what is expected.");
               Assert.AreEqual(6, sw.Elapsed.Seconds, "The timeout is not what is expected.");
               Assert.AreEqual(2, errorCount);
            }
         }
         
         Console.WriteLine();
      }
   }
}
