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

using System;
using System.Linq;
using Alphaleonis.Win32;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AlphaFS.UnitTest
{
   partial class DirectoryTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>

      [TestMethod]
      public void AlphaFS_Directory_EnumerateFiles_UsingDirectoryEnumerationFilters_LocalAndNetwork_Success()
      {
         Directory_EnumerateFiles_UsingDirectoryEnumerationFilters(false);
         Directory_EnumerateFiles_UsingDirectoryEnumerationFilters(true);
      }




      private void Directory_EnumerateFiles_UsingDirectoryEnumerationFilters(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);
         Console.WriteLine();

         var inputPath = UnitTestConstants.SysRoot;
         if (isNetwork)
            inputPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(inputPath);


         Console.WriteLine("Input Directory Path: [{0}]", inputPath);
         Console.WriteLine();


         var findExtensions = new[] {".txt", ".ini", ".exe"};
         var skipFolders = new[] { "assembly", "SysWOW64", "WinSxS" };

         Console.WriteLine("Extensions to search for: {0}", string.Join(" | ", findExtensions));
         Console.WriteLine("Directories to skip     : {0}\\{1}, {0}\\{2}, {0}\\{3}", inputPath, skipFolders[0], skipFolders[1], skipFolders[2]);
         Console.WriteLine();


         var gotException = false;
         var exceptionCount = 0;
         var foundCount = 0;
         var foundExt1 = 0;
         var foundExt2 = 0;
         var foundExt3 = 0;


         var filters = new Alphaleonis.Win32.Filesystem.DirectoryEnumerationFilters
         {
            InclusionFilter = fsei =>
            {
               var extension = System.IO.Path.GetExtension(fsei.FileName);

               var gotMatch = findExtensions.Any(found => found.Equals(extension, StringComparison.OrdinalIgnoreCase));
               if (gotMatch)
               {
                  if (extension == findExtensions[0])
                     foundExt1++;

                  if (extension == findExtensions[1])
                     foundExt2++;

                  if (extension == findExtensions[2])
                     foundExt3++;


                  Console.WriteLine("\t#{0:N0}\t\t[{1}]", ++foundCount, fsei.FullPath);
               }

               return gotMatch;
            },


            RecursionFilter = fsei =>
            {
               return !skipFolders.Any(found => found.Equals(fsei.FileName, StringComparison.OrdinalIgnoreCase));
            },


            ErrorFilter = delegate(int errorCode, string errorMessage, string pathProcessed)
            {
               gotException = errorCode == Win32Errors.ERROR_ACCESS_DENIED;

               if (gotException)
                  Console.WriteLine("\t#{0:N0}\t\t({1}) {2}: [{3}]", ++exceptionCount, errorCode, errorMessage, pathProcessed);

               return gotException;
            }
         };


         var dirEnumOptions = Alphaleonis.Win32.Filesystem.DirectoryEnumerationOptions.Recursive;

         var fsoCount = Alphaleonis.Win32.Filesystem.Directory.EnumerateFiles(inputPath, dirEnumOptions, filters).Count();


         Console.WriteLine("\n\tFile system objects counted: {0:N0}", fsoCount);


         Assert.IsTrue(foundExt1 > 0, "No " + findExtensions[0] + " files enumerated, but it is expected.");
         Assert.IsTrue(foundExt2 > 0, "No " + findExtensions[1] + " files enumerated, but it is expected.");
         Assert.IsTrue(foundExt3 > 0, "No " + findExtensions[2] + " files enumerated, but it is expected.");
         Assert.IsTrue(fsoCount > 0, "No files enumerated, but it is expected.");
         Assert.IsTrue(gotException, "The Exception is not caught, but it is expected.");

         Console.WriteLine();
      }
   }
}
