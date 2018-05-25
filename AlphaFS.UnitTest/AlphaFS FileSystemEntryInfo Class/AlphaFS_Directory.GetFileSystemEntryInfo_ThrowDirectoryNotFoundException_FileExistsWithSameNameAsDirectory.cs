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
   public partial class AlphaFS_FileSystemEntryInfoTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void AlphaFS_Directory_GetFileSystemEntryInfo_ThrowDirectoryNotFoundException_FileExistsWithSameNameAsDirectory_LocalAndNetwork_Success()
      {
         Directory_GetFileSystemEntryInfo_ThrowDirectoryNotFoundException_FileExistsWithSameNameAsDirectory(false);
         Directory_GetFileSystemEntryInfo_ThrowDirectoryNotFoundException_FileExistsWithSameNameAsDirectory(true);
      }


      private void Directory_GetFileSystemEntryInfo_ThrowDirectoryNotFoundException_FileExistsWithSameNameAsDirectory(bool isNetwork)
      {
         var path = UnitTestConstants.NotepadExe;

         if (!System.IO.File.Exists(path))
            Assert.Inconclusive("Test ignored because {0} was not found.", path);


         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         var tempPath = path;
         if (isNetwork)
            tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);


         Console.WriteLine("\nInput Directory Path: [{0}]", tempPath);


         var gotException = false;
         try
         {
            Alphaleonis.Win32.Filesystem.Directory.GetFileSystemEntryInfo(tempPath);
         }
         catch (Exception ex)
         {
            var exType = ex.GetType();

            gotException = exType == typeof(System.IO.DirectoryNotFoundException);

            Console.WriteLine("\n\tCaught {0} Exception: [{1}] {2}", gotException ? "EXPECTED" : "UNEXPECTED", exType.Name, ex.Message);
         }

         Assert.IsTrue(gotException, "The exception is not caught, but is expected to.");


         Console.WriteLine();
      }
   }
}
