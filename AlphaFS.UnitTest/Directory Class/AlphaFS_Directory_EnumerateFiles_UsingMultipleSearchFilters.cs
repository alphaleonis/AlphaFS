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

namespace AlphaFS.UnitTest
{
   partial class DirectoryTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>

      [TestMethod]
      public void AlphaFS_Directory_EnumerateFiles_UsingMultipleSearchFilters_LocalAndNetwork_Success()
      {
         Directory_EnumerateFiles_UsingMultipleSearchFilters(false);
         Directory_EnumerateFiles_UsingMultipleSearchFilters(true);
      }




      private void Directory_EnumerateFiles_UsingMultipleSearchFilters(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);
         Console.WriteLine();

         var inputPath = UnitTestConstants.SysRoot32;
         if (isNetwork)
            inputPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(inputPath);


         Console.WriteLine("Input Directory Path: [{0}]", inputPath);
         Console.WriteLine();


         var searchForExtensions = new[] {".ocx", ".tlb", ".cpl"};

         Console.WriteLine("Extensions to search for: [{0}]", string.Join(", ", searchForExtensions));
         Console.WriteLine();


         var gotException = false;
         var exceptionCount = 0;
         var foundCount = 0;
         

         var filters = new Alphaleonis.Win32.Filesystem.DirectoryEnumerationFilters
         {
            InclusionFilter = delegate(Alphaleonis.Win32.Filesystem.FileSystemEntryInfo fsei)
            {
               var ext = Alphaleonis.Win32.Filesystem.Path.GetExtension(fsei.FileName, false);

               var gotMatch = ext.Equals(".ocx", StringComparison.OrdinalIgnoreCase) ||
                              ext.Equals(".tlb", StringComparison.OrdinalIgnoreCase) ||
                              ext.Equals(".cpl", StringComparison.OrdinalIgnoreCase);

               if (gotMatch)
                  Console.WriteLine("\t#{0:N0}\t\t[{1}]", ++foundCount, fsei.FullPath);

               return gotMatch;
            },


            ErrorFilter = delegate(int errorCode, string errorMessage, string pathProcessed)
            {
               gotException = errorCode == Alphaleonis.Win32.Win32Errors.ERROR_ACCESS_DENIED;

               if (gotException)
                  Console.WriteLine("\t#{0:N0}\t\t({1}) {2}: [{3}]", ++exceptionCount, errorCode, errorMessage, pathProcessed);

               return gotException;
            }
         };


         var dirEnumOptions = Alphaleonis.Win32.Filesystem.DirectoryEnumerationOptions.Recursive;

         var count = Alphaleonis.Win32.Filesystem.Directory.EnumerateFiles(inputPath, dirEnumOptions, filters).Count();


         Console.WriteLine("\n\tFile system objects counted: {0:N0}", count);


         Assert.IsTrue(count > 0, "No files enumerated, but it is expected.");
         Assert.IsTrue(gotException, "The Exception is not caught, but it is expected.");

         Console.WriteLine();
      }
   }
}
