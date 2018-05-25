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
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AlphaFS.UnitTest
{
   public partial class AlphaFS_JunctionsLinksTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void AlphaFS_File_CreateSymbolicLink_ThrowIOException_DirectoryExistsWithSameNameAsFile_LocalAndNetwork_Success()
      {
         if (!UnitTestConstants.IsAdmin())
            Assert.Inconclusive();

         File_CreateSymbolicLink_ThrowIOException_DirectoryExistsWithSameNameAsFile(false);
         File_CreateSymbolicLink_ThrowIOException_DirectoryExistsWithSameNameAsFile(true);
      }


      private void File_CreateSymbolicLink_ThrowIOException_DirectoryExistsWithSameNameAsFile(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         var tempPath = System.IO.Path.GetTempPath();
         if (isNetwork)
            tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);


         using (var rootDir = new TemporaryDirectory(tempPath, MethodBase.GetCurrentMethod().Name))
         {
            var folderLink = System.IO.Path.Combine(rootDir.Directory.FullName, "FolderLink-ToDestinationFolder");

            var dirInfo = new System.IO.DirectoryInfo(System.IO.Path.Combine(rootDir.Directory.FullName, "DestinationFolder"));
            dirInfo.Create();

            Console.WriteLine("\nInput Directory Path: [{0}]", dirInfo.FullName);
            Console.WriteLine("Input Directory Link: [{0}]", folderLink);


            var gotException = false;

            try
            {
               Alphaleonis.Win32.Filesystem.File.CreateSymbolicLink(folderLink, dirInfo.FullName);

            }
            catch (Exception ex)
            {
               var exType = ex.GetType();

               gotException = exType == typeof(System.IO.IOException);

               Console.WriteLine("\n\tCaught {0} Exception: [{1}] {2}", gotException ? "EXPECTED" : "UNEXPECTED", exType.Name, ex.Message);
            }

            Assert.IsTrue(gotException, "The exception is not caught, but is expected to.");
         }

         Console.WriteLine();
      }
   }
}
