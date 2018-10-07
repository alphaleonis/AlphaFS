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

using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#if !NET35
using System.Threading;
#endif

namespace AlphaFS.UnitTest
{
   public partial class EnumerationTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void AlphaFS_Directory_EnumerateFileSystemEntryInfos_EnumerateFiles_UsingDirectoryEnumerationFilters_LocalAndNetwork_Success()
      {
         AlphaFS_Directory_EnumerateFileSystemEntryInfos_EnumerateFiles_UsingDirectoryEnumerationFilters(false);
         AlphaFS_Directory_EnumerateFileSystemEntryInfos_EnumerateFiles_UsingDirectoryEnumerationFilters(true);
      }


      private void AlphaFS_Directory_EnumerateFileSystemEntryInfos_EnumerateFiles_UsingDirectoryEnumerationFilters(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);
         
         var inputPath = Environment.SystemDirectory;
         if (isNetwork)
            inputPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(inputPath);

         Console.WriteLine("Input Directory Path: [{0}]\n", inputPath);
         
         Test_DirectoryEnumerationFilters(inputPath);
         
         Console.WriteLine();
      }




      private void Test_DirectoryEnumerationFilters(string inputPath)
      {
         bool gotException;

         var findExtensions = new[] {".txt", ".ini", ".exe"};
         var skipFolders = new[] {"assembly", "WinSxS"};

         Console.WriteLine("Extensions to search for: {0}", string.Join(" | ", findExtensions));
         Console.WriteLine("Root directories to skip: {0}\\{1}, {0}\\{2}", inputPath, skipFolders[0], skipFolders[1]);
         Console.WriteLine();


         var exceptionCount = 0;
         var foundExt1 = 0;
         var foundExt2 = 0;
         var foundExt3 = 0;
         var foundExt1Done = false;
         var foundExt2Done = false;
         var foundExt3Done = false;


#if NET35
         var abortEnumeration = false;


         var filters = new Alphaleonis.Win32.Filesystem.DirectoryEnumerationFilters
         {
            // Filter to decide whether to recurse into subdirectories.
            RecursionFilter = fsei =>
            {
               if (abortEnumeration)
                  return false;


               // Return true to continue recursion, false to skip.
               return !skipFolders.Any(found => found.Equals(fsei.FileName, StringComparison.OrdinalIgnoreCase));
            },


            // Filter to process Exception handling.
            ErrorFilter = delegate(int errorCode, string errorMessage, string pathProcessed)
            {
               if (abortEnumeration)
                  return true;


               gotException = errorCode == Alphaleonis.Win32.Win32Errors.ERROR_ACCESS_DENIED;

               Console.WriteLine("\t#{0:N0}\t\t({1}) {2}: [{3}]", ++exceptionCount, errorCode, errorMessage, pathProcessed);


               // Return true to continue, false to throw the Exception.
               return gotException;
            },


            // Filter to in-/exclude file system entries during the enumeration.
            InclusionFilter = fsei =>
            {
               if (abortEnumeration)
                  return false;


               var fileExtension = fsei.Extension;

               var gotMatch = findExtensions.Any(found => found.Equals(fileExtension, StringComparison.OrdinalIgnoreCase));
               if (gotMatch)
               {
                  if (!foundExt1Done && fileExtension == findExtensions[0])
                  {
                     foundExt1++;
                     foundExt1Done = foundExt1 == 3;
                     Console.WriteLine("\t#{0:N0}\t\t[{1}]", foundExt1, fsei.FullPath);
                  }

                  else if (!foundExt2Done && fileExtension == findExtensions[1])
                  {
                     foundExt2++;
                     foundExt2Done = foundExt2 == 3;
                     Console.WriteLine("\t#{0:N0}\t\t[{1}]", foundExt2, fsei.FullPath);
                  }

                  else if (!foundExt3Done && fileExtension == findExtensions[2])
                  {
                     foundExt3++;
                     foundExt3Done = foundExt3 == 3;
                     Console.WriteLine("\t#{0:N0}\t\t[{1}]", foundExt3, fsei.FullPath);
                  }
               }


               // Abort the enumeration.
               if (foundExt1Done && foundExt2Done && foundExt3Done)
                  abortEnumeration = true;


               return gotMatch;
            }
         };
#else
         var cancelSource = new CancellationTokenSource();


         var filters = new Alphaleonis.Win32.Filesystem.DirectoryEnumerationFilters
         {
            // Used to abort the enumeration.
            CancellationToken = cancelSource.Token,


            // Filter to decide whether to recurse into subdirectories.
            RecursionFilter = fsei =>
            {
               // Return true to continue recursion, false to skip.
               return !skipFolders.Any(found => found.Equals(fsei.FileName, StringComparison.OrdinalIgnoreCase));
            },


            // Filter to process Exception handling.
            ErrorFilter = delegate(int errorCode, string errorMessage, string pathProcessed)
            {
               gotException = errorCode == Alphaleonis.Win32.Win32Errors.ERROR_ACCESS_DENIED;

               Console.WriteLine("\t#{0:N0}\t\t({1}) {2}: [{3}]", ++exceptionCount, errorCode, errorMessage, pathProcessed);


               // Return true to continue, false to throw the Exception.
               return gotException;
            },


            // Filter to in-/exclude file system entries during the enumeration.
            InclusionFilter = fsei =>
            {
               var fileExtension = fsei.Extension;

               var gotMatch = findExtensions.Any(found => found.Equals(fileExtension, StringComparison.OrdinalIgnoreCase));
               if (gotMatch)
               {
                  if (!foundExt1Done && fileExtension == findExtensions[0])
                  {
                     foundExt1++;
                     foundExt1Done = foundExt1 == 3;
                     Console.WriteLine("\t#{0:N0}\t\t[{1}]", foundExt1, fsei.FullPath);
                  }

                  else if (!foundExt2Done && fileExtension == findExtensions[1])
                  {
                     foundExt2++;
                     foundExt2Done = foundExt2 == 3;
                     Console.WriteLine("\t#{0:N0}\t\t[{1}]", foundExt2, fsei.FullPath);
                  }

                  else if (!foundExt3Done && fileExtension == findExtensions[2])
                  {
                     foundExt3++;
                     foundExt3Done = foundExt3 == 3;
                     Console.WriteLine("\t#{0:N0}\t\t[{1}]", foundExt3, fsei.FullPath);
                  }
               }


               // Abort the enumeration.
               if (foundExt1Done && foundExt2Done && foundExt3Done)
                  cancelSource.Cancel();


               return gotMatch;
            }
         };
#endif


         const Alphaleonis.Win32.Filesystem.DirectoryEnumerationOptions dirEnumOptions = Alphaleonis.Win32.Filesystem.DirectoryEnumerationOptions.Files | Alphaleonis.Win32.Filesystem.DirectoryEnumerationOptions.Recursive;

         var fsoCount = Alphaleonis.Win32.Filesystem.Directory.EnumerateFileSystemEntryInfos<string>(inputPath, dirEnumOptions, filters).Count();
         
         
         Console.WriteLine("\n\tFile system objects counted: {0:N0}", fsoCount);


         Assert.IsTrue(fsoCount > 0, "No files enumerated, but it is expected.");

         Assert.IsTrue(foundExt1 > 0, "No " + findExtensions[0] + " files enumerated, but it is expected.");
         Assert.IsTrue(foundExt2 > 0, "No " + findExtensions[1] + " files enumerated, but it is expected.");
         Assert.IsTrue(foundExt3 > 0, "No " + findExtensions[2] + " files enumerated, but it is expected.");


         Console.WriteLine();
      }
   }
}
