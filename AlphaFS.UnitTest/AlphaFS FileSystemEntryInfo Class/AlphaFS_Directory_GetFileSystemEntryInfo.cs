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
   public partial class AlphaFS_FileSystemEntryInfoTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void AlphaFS_Directory_GetFileSystemEntryInfo_LocalAndNetwork_Success()
      {
         Directory_GetFileSystemEntryInfo(false);
         Directory_GetFileSystemEntryInfo(true);
      }


      [TestMethod]
      public void AlphaFS_Directory_GetFileSystemEntryInfo_LogicalDrives_Local_Success()
      {
         Directory_GetFileSystemEntryInfo_LogicalDrives();
      }


      [TestMethod]
      public void AlphaFS_Directory_GetFileSystemEntryInfo_CatchDirectoryNotFoundException_FileExistsWithSameNameAsDirectory_LocalAndNetwork_Success()
      {
         Directory_GetFileSystemEntryInfo_CatchDirectoryNotFoundException_FileExistsWithSameNameAsDirectory(false);
         Directory_GetFileSystemEntryInfo_CatchDirectoryNotFoundException_FileExistsWithSameNameAsDirectory(true);
      }




      private void Directory_GetFileSystemEntryInfo(bool isNetwork)
      {
         var path = UnitTestConstants.SysRoot;

         if (!System.IO.Directory.Exists(path))
            Assert.Inconclusive("Test ignored because {0} was not found.", path);


         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         var tempPath = path;
         if (isNetwork)
            tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);


         Console.WriteLine("\nInput Directory Path: [{0}]", tempPath);

         var fsei = Alphaleonis.Win32.Filesystem.Directory.GetFileSystemEntryInfo(tempPath);
         UnitTestConstants.Dump(fsei, -19);


         Assert.IsTrue(fsei.GetType().IsEquivalentTo(typeof(Alphaleonis.Win32.Filesystem.FileSystemEntryInfo)));
         Assert.IsTrue((fsei.Attributes & System.IO.FileAttributes.Directory) != 0, "The Directory attribute is not found, but is expected.");
         Assert.AreEqual(tempPath, fsei.FullPath, "The paths are not equal, but are expected to be.");
         
         Console.WriteLine();
      }


      private void Directory_GetFileSystemEntryInfo_LogicalDrives()
      {
         UnitTestConstants.PrintUnitTestHeader(false);


         var drives = Alphaleonis.Win32.Filesystem.Directory.EnumerateLogicalDrives().ToList();

         foreach (var drive in drives)
         {
            Console.WriteLine("\nInput Logical Drive: [{0}]", drive.Name);

            try
            { 
               var fsei = Alphaleonis.Win32.Filesystem.Directory.GetFileSystemEntryInfo(drive.Name);
               UnitTestConstants.Dump(fsei, -19);


               Assert.IsTrue(fsei.GetType().IsEquivalentTo(typeof(Alphaleonis.Win32.Filesystem.FileSystemEntryInfo)));
               Assert.IsTrue((fsei.Attributes & System.IO.FileAttributes.Directory) != 0, "The Directory attribute is not found, but is expected.");


               // Fixed local drives should have these attributes.
               if (new Alphaleonis.Win32.Filesystem.DriveInfo(drive.Name).DriveType == System.IO.DriveType.Fixed)
               {
                  Assert.IsTrue((fsei.Attributes & System.IO.FileAttributes.Hidden) != 0, "The Hidden attribute is not found, but is expected.");
                  Assert.IsTrue((fsei.Attributes & System.IO.FileAttributes.System) != 0, "The System attribute is not found, but is expected.");
               }


               Assert.AreEqual(".", fsei.FileName, "The file names are not equal, but are expected to be.");
               Assert.AreEqual(drive.Name, fsei.FullPath, "The full paths are not equal, but are expected to be.");
            }
            catch (Exception ex)
            {
               Console.WriteLine("\n\tCaught UNEXPECTED Exception: [{0}] {1}", ex.GetType().Name, ex.Message);
            }


            Console.WriteLine();
         }
      }


      private void Directory_GetFileSystemEntryInfo_CatchDirectoryNotFoundException_FileExistsWithSameNameAsDirectory(bool isNetwork)
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
            var exName = ex.GetType().Name;
            gotException = exName.Equals("DirectoryNotFoundException", StringComparison.OrdinalIgnoreCase);
            Console.WriteLine("\n\tCaught {0} Exception: [{1}] {2}", gotException ? "EXPECTED" : "UNEXPECTED", exName, ex.Message);
         }
         Assert.IsTrue(gotException, "The exception is not caught, but is expected to.");


         Console.WriteLine();
      }
   }
}
