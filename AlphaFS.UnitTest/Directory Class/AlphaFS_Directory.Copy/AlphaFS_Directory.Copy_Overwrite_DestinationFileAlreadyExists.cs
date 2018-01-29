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
using System.Reflection;

namespace AlphaFS.UnitTest
{
   public partial class AlphaFS_Directory_CopyTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void AlphaFS_Directory_Copy_Overwrite_DestinationFileAlreadyExists_LocalAndNetwork_Success()
      {
         Directory_Copy_Overwrite_DestinationFileAlreadyExists(false);
         Directory_Copy_Overwrite_DestinationFileAlreadyExists(true);
      }


      private void Directory_Copy_Overwrite_DestinationFileAlreadyExists(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);
         Console.WriteLine();
         

         var tempPath = UnitTestConstants.TempFolder;
         if (isNetwork)
            tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);


         using (var rootDir = new TemporaryDirectory(tempPath, MethodBase.GetCurrentMethod().Name))
         {
            var folderSrc = System.IO.Directory.CreateDirectory(System.IO.Path.Combine(rootDir.Directory.FullName, "Existing Source Folder"));
            var folderDst = System.IO.Directory.CreateDirectory(System.IO.Path.Combine(rootDir.Directory.FullName, "Existing Destination Folder"));

            Console.WriteLine("Src Directory Path: [{0}]", folderSrc.FullName);
            Console.WriteLine("Dst Directory Path: [{0}]", folderDst.FullName);


            UnitTestConstants.CreateDirectoriesAndFiles(folderSrc.FullName, 1, false, false, true);


            Alphaleonis.Win32.Filesystem.Directory.Copy(folderSrc.FullName, folderDst.FullName);


            var dirEnumOptions = Alphaleonis.Win32.Filesystem.DirectoryEnumerationOptions.FilesAndFolders | Alphaleonis.Win32.Filesystem.DirectoryEnumerationOptions.Recursive;

            var props = Alphaleonis.Win32.Filesystem.Directory.GetProperties(folderSrc.FullName, dirEnumOptions);
            var sourceTotal = props["Total"];
            var sourceTotalFiles = props["File"];
            var sourceTotalSize = props["Size"];

            Console.WriteLine("\n\tTotal size: [{0}] - Total Folders: [{1}] - Files: [{2}]", Alphaleonis.Utils.UnitSizeToText(sourceTotalSize), sourceTotal - sourceTotalFiles, sourceTotalFiles);




            var gotException = false;

            try
            {
               Alphaleonis.Win32.Filesystem.Directory.Copy(folderSrc.FullName, folderDst.FullName);
            }
            catch (Exception ex)
            {
               var exType = ex.GetType();

               gotException = exType == typeof(Alphaleonis.Win32.Filesystem.AlreadyExistsException);

               //Console.WriteLine("\n\tCaught {0} Exception: [{1}] {2}", gotException ? "EXPECTED" : "UNEXPECTED", exType.Name, ex.Message);
            }


            Assert.IsTrue(gotException, "The exception is not caught, but is expected to.");

            Assert.IsTrue(System.IO.Directory.Exists(folderDst.FullName), "The directory does not exist, but is expected to.");
            

            

            // Overwrite using CopyOptions.None

            var copyResult = Alphaleonis.Win32.Filesystem.Directory.Copy(folderSrc.FullName, folderDst.FullName, Alphaleonis.Win32.Filesystem.CopyOptions.None);


            props = Alphaleonis.Win32.Filesystem.Directory.GetProperties(folderDst.FullName, dirEnumOptions);
            Assert.AreEqual(sourceTotal, props["Total"], "The number of total file system objects does not match, but is expected to.");
            Assert.AreEqual(sourceTotalFiles, props["File"], "The number of total files does not match, but is expected to.");
            Assert.AreEqual(sourceTotalSize, props["Size"], "The total file size does not match, but is expected to.");


            // Test against copyResult results.

            Assert.IsNotNull(copyResult);
            Assert.AreEqual(sourceTotal, copyResult.TotalFolders + copyResult.TotalFiles, "The number of total file system objects does not match, but is expected to.");
            Assert.AreEqual(sourceTotalFiles, copyResult.TotalFiles, "The number of total files does not match, but is expected to.");
            Assert.AreEqual(sourceTotalSize, copyResult.TotalBytes, "The total file size does not match, but is expected to.");
         }


         Console.WriteLine();
      }
   }
}
