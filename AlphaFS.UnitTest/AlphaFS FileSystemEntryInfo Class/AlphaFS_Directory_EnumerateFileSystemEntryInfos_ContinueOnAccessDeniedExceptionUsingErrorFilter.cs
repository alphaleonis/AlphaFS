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
using System.Linq;
using System.Threading;

namespace AlphaFS.UnitTest
{
   partial class AlphaFS_FileSystemEntryInfoTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void AlphaFS_Directory_EnumerateFileSystemEntryInfos_ContinueOnAccessDeniedExceptionUsingErrorFilter_LocalAndNetwork_Success()
      {
         Directory_EnumerateFileSystemEntryInfos_ContinueOnAccessDeniedExceptionUsingErrorFilter(false);
         Directory_EnumerateFileSystemEntryInfos_ContinueOnAccessDeniedExceptionUsingErrorFilter(true);
      }
      

      private void Directory_EnumerateFileSystemEntryInfos_ContinueOnAccessDeniedExceptionUsingErrorFilter(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);
         Console.WriteLine();


         var gotException = false;


         var inputPath = System.IO.Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)).FullName;
         if (isNetwork)
            inputPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(inputPath);

         Console.WriteLine("Input Directory Path: [{0}]", inputPath);
         Console.WriteLine();


         var exceptionCatch = 10;
         var exceptionCount = 0;


         // Used to abort the enumeration.
         var cancelSource = new CancellationTokenSource();
         

         var filters = new Alphaleonis.Win32.Filesystem.DirectoryEnumerationFilters
         {
            CancellationToken = cancelSource.Token,


            ErrorFilter = delegate(int errorCode, string errorMessage, string pathProcessed)
            {
               gotException = errorCode == Alphaleonis.Win32.Win32Errors.ERROR_ACCESS_DENIED;

               if (gotException)
               {
                  exceptionCount++;

                  // Report Exception.
                  Console.WriteLine("\t#{0:N0}\t({1}) {2}: [{3}]", exceptionCount, errorCode, errorMessage, pathProcessed);


                  if (exceptionCount == exceptionCatch)
                     cancelSource.Cancel();
               }


               // Continue enumeration.
               return gotException;
            }
         };


         var dirEnumOptions = Alphaleonis.Win32.Filesystem.DirectoryEnumerationOptions.Recursive;

         var count = Alphaleonis.Win32.Filesystem.Directory.EnumerateFileSystemEntryInfos<string>(inputPath, dirEnumOptions, filters).Count();


         Console.WriteLine("\n\tFile system objects counted: {0:N0}", count);


         Assert.IsTrue(count > 0, "No file system entries enumerated, but it is expected.");
         Assert.IsTrue(gotException, "The Exception is not caught, but it is expected.");

         Assert.AreEqual(exceptionCatch, exceptionCount, "The number of caught Exceptions does not match, but it is expected.");


         Console.WriteLine();
      }
   }
}
