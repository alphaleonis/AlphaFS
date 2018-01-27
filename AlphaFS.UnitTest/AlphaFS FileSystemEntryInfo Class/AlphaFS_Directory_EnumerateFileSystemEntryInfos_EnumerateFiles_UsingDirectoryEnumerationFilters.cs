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
using System.Threading;
using Alphaleonis.Win32;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AlphaFS.UnitTest
{
   partial class AlphaFS_FileSystemEntryInfoTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void AlphaFS_Directory_EnumerateFileSystemEntryInfos_EnumerateFiles_UsingDirectoryEnumerationFilters_LocalAndNetwork_Success()
      {
         Directory_EnumerateFileSystemEntryInfos_EnumerateFiles_UsingDirectoryEnumerationFilters(false);
         Directory_EnumerateFileSystemEntryInfos_EnumerateFiles_UsingDirectoryEnumerationFilters(true);
      }
      

      private void Directory_EnumerateFileSystemEntryInfos_EnumerateFiles_UsingDirectoryEnumerationFilters(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);
         Console.WriteLine();


         bool gotException;


         var inputPath = UnitTestConstants.SysRoot;
         if (isNetwork)
            inputPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(inputPath);


         Console.WriteLine("Input Directory Path: [{0}]", inputPath);
         Console.WriteLine();


         var findExtensions = new[] {".txt", ".ini", ".exe"};
         var skipFolders = new[] {"assembly", "WinSxS"};
         //var skipFolders = new[] {"assembly", "System32", "SysWOW64", "WinSxS"};

         Console.WriteLine("Extensions to search for: {0}", string.Join(" | ", findExtensions));
         Console.WriteLine("Directories to skip     : {0}\\{1}, {0}\\{2}", inputPath, skipFolders[0], skipFolders[1]);
         Console.WriteLine();
         Console.WriteLine();


         var exceptionCount = 0;
         var foundExt1 = 0;
         var foundExt2 = 0;
         var foundExt3 = 0;
         var foundExt1Done = false;
         var foundExt2Done = false;
         var foundExt3Done = false;


         // Used to abort the enumeration.
         var cancelSource = new CancellationTokenSource();


         var filters = new Alphaleonis.Win32.Filesystem.DirectoryEnumerationFilters
         {
            CancellationToken = cancelSource.Token,


            InclusionFilter = fsei =>
            {
               var extension = System.IO.Path.GetExtension(fsei.FileName);

               var gotMatch = findExtensions.Any(found => found.Equals(extension, StringComparison.OrdinalIgnoreCase));
               if (gotMatch)
               {
                  if (!foundExt1Done && extension == findExtensions[0])
                  {
                     foundExt1++;
                     foundExt1Done = foundExt1 == 3;
                     Console.WriteLine("\t#{0:N0}\t\t[{1}]", foundExt1, fsei.FullPath);
                  }

                  if (!foundExt2Done && extension == findExtensions[1])
                  {
                     foundExt2++;
                     foundExt2Done = foundExt2 == 3;
                     Console.WriteLine("\t#{0:N0}\t\t[{1}]", foundExt2, fsei.FullPath);
                  }

                  if (!foundExt3Done && extension == findExtensions[2])
                  {
                     foundExt3++;
                     foundExt3Done = foundExt3 == 3;
                     Console.WriteLine("\t#{0:N0}\t\t[{1}]", foundExt3, fsei.FullPath);
                  }
               }


               if (foundExt1Done && foundExt2Done && foundExt3Done)
               {
                  cancelSource.Cancel();
                  return false;
               }

               return gotMatch;
            },


            RecursionFilter = fsei =>
            {
               // Return true to continue.
               return !skipFolders.Any(found => found.Equals(fsei.FileName, StringComparison.OrdinalIgnoreCase));
            },


            ErrorFilter = delegate(int errorCode, string errorMessage, string pathProcessed)
            {
               gotException = errorCode == Win32Errors.ERROR_ACCESS_DENIED;

               if (gotException)
                  Console.WriteLine("\t#{0:N0}\t\t({1}) {2}: [{3}]", ++exceptionCount, errorCode, errorMessage, pathProcessed);

               // Return true to continue.
               return gotException;
            }
         };


         var dirEnumOptions = Alphaleonis.Win32.Filesystem.DirectoryEnumerationOptions.Files | Alphaleonis.Win32.Filesystem.DirectoryEnumerationOptions.Recursive;

         var fsoCount = Alphaleonis.Win32.Filesystem.Directory.EnumerateFileSystemEntryInfos<string>(inputPath, dirEnumOptions, filters).Count();
         
         
         Console.WriteLine("\n\tFile system objects counted: {0:N0}", fsoCount);


         Assert.IsTrue(foundExt1 > 0, "No " + findExtensions[0] + " files enumerated, but it is expected.");
         Assert.IsTrue(foundExt2 > 0, "No " + findExtensions[1] + " files enumerated, but it is expected.");
         Assert.IsTrue(foundExt3 > 0, "No " + findExtensions[2] + " files enumerated, but it is expected.");
         Assert.IsTrue(fsoCount > 0, "No files enumerated, but it is expected.");


         Console.WriteLine();
      }
   }
}
