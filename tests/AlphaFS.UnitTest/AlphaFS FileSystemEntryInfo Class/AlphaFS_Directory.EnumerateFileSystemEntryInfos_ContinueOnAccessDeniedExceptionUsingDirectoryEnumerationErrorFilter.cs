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

#if !NET35
using System.Threading;
#endif

namespace AlphaFS.UnitTest
{
   public partial class EnumerationTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void AlphaFS_Directory_EnumerateFileSystemEntryInfos_ContinueOnAccessDeniedExceptionUsingDirectoryEnumerationErrorFilter_LocalAndNetwork_Success()
      {
         AlphaFS_Directory_EnumerateFileSystemEntryInfos_ContinueOnAccessDeniedExceptionUsingDirectoryEnumerationErrorFilter(false);
         AlphaFS_Directory_EnumerateFileSystemEntryInfos_ContinueOnAccessDeniedExceptionUsingDirectoryEnumerationErrorFilter(true);
      }


      private void AlphaFS_Directory_EnumerateFileSystemEntryInfos_ContinueOnAccessDeniedExceptionUsingDirectoryEnumerationErrorFilter(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);
         
         var inputPath = System.IO.Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)).FullName;
         if (isNetwork)
            inputPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(inputPath);

         Console.WriteLine("Input Directory Path: [{0}]\n", inputPath);
         
         Test_ContinueOnAccessDeniedExceptionUsingErrorFilter(inputPath);
         
         Console.WriteLine();
      }




      private void Test_ContinueOnAccessDeniedExceptionUsingErrorFilter(string inputPath)
      {
         var gotException = false;
         const int exceptionCatch = 10;
         var exceptionCount = 0;


#if NET35
         var abortEnumeration = false;


         var filters = new Alphaleonis.Win32.Filesystem.DirectoryEnumerationFilters
         {
            // Filter to decide whether to recurse into subdirectories.
            RecursionFilter = fsei =>
            {
               // Return true to continue recursion, false to skip.
               return !abortEnumeration;
            },


            // Filter to process Exception handling.
            ErrorFilter = delegate(int errorCode, string errorMessage, string pathProcessed)
            {
               if (abortEnumeration)
                  return true;


               gotException = errorCode == Alphaleonis.Win32.Win32Errors.ERROR_ACCESS_DENIED;
               if (gotException)
               {
                  exceptionCount++;

                  if (exceptionCount == exceptionCatch)
                     abortEnumeration = true;
               }


               // Report Exception.
               Console.WriteLine("\t#{0:N0}\t({1}) {2}: [{3}]", exceptionCount, errorCode, errorMessage, pathProcessed);


               // Return true to continue, false to throw the Exception.
               return gotException;
            }
         };
#else
         var cancelSource = new CancellationTokenSource();


         var filters = new Alphaleonis.Win32.Filesystem.DirectoryEnumerationFilters
         {
            // Used to abort the enumeration.
            CancellationToken = cancelSource.Token,


            // Filter to process Exception handling.
            ErrorFilter = delegate(int errorCode, string errorMessage, string pathProcessed)
            {
               gotException = errorCode == Alphaleonis.Win32.Win32Errors.ERROR_ACCESS_DENIED;
               if (gotException)
               {
                  exceptionCount++;

                  if (exceptionCount == exceptionCatch)
                     cancelSource.Cancel();
               }


               // Report Exception.
               Console.WriteLine("\t#{0:N0}\t({1}) {2}: [{3}]", exceptionCount, errorCode, errorMessage, pathProcessed);


               // Return true to continue, false to throw the Exception.
               return gotException;
            }
         };
#endif

         
         const Alphaleonis.Win32.Filesystem.DirectoryEnumerationOptions dirEnumOptions = Alphaleonis.Win32.Filesystem.DirectoryEnumerationOptions.Recursive;

         var fsoCount = Alphaleonis.Win32.Filesystem.Directory.EnumerateFileSystemEntryInfos<string>(inputPath, dirEnumOptions, filters).Count();


         Console.WriteLine("\n\tFile system objects counted: {0:N0}", fsoCount);


         Assert.IsTrue(fsoCount > 0, "No file system entries enumerated, but it is expected.");

         Assert.IsTrue(gotException, "The Exception is not caught, but it is expected.");

         Assert.AreEqual(exceptionCatch, exceptionCount, "The number of caught Exceptions does not match, but it is expected.");
      }
   }
}
