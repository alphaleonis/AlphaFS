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
using System.IO;

namespace AlphaFS.UnitTest
{
   /// <summary>This is a test class for FileSystemEntryInfo and is intended to contain all FileSystemEntryInfo Unit Tests.</summary>
   [TestClass]
   public partial class FileSystemEntryInfoTest
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
         UnitTestConstants.Dump(fsei, -17);


         Assert.IsTrue(fsei.GetType().IsEquivalentTo(typeof(Alphaleonis.Win32.Filesystem.FileSystemEntryInfo)));
         Assert.IsTrue((fsei.Attributes & System.IO.FileAttributes.Directory) != 0, "The Directory attribute is not found, but is expected.");
         Assert.AreEqual(tempPath, fsei.FullPath, "The paths are not equal, but are expected to be.");
         
         Console.WriteLine();
      }


      private void Directory_GetFileSystemEntryInfo_LogicalDrives()
      {
         UnitTestConstants.PrintUnitTestHeader(false);


         foreach (var drive in Alphaleonis.Win32.Filesystem.Directory.EnumerateLogicalDrives())
         {
            Console.WriteLine("\nInput Directory Path: [{0}]", drive);

            var fsei = Alphaleonis.Win32.Filesystem.Directory.GetFileSystemEntryInfo(drive.Name);
            UnitTestConstants.Dump(fsei, -17);


            Assert.IsTrue(fsei.GetType().IsEquivalentTo(typeof(Alphaleonis.Win32.Filesystem.FileSystemEntryInfo)));
            Assert.IsTrue((fsei.Attributes & System.IO.FileAttributes.Directory) != 0, "The Directory attribute is not found, but is expected.");


            // Fixes local drives should have these attributes.
            if (new Alphaleonis.Win32.Filesystem.DriveInfo(drive.Name).DriveType == DriveType.Fixed)
            {
               Assert.IsTrue((fsei.Attributes & System.IO.FileAttributes.Hidden) != 0, "The Hidden attribute is not found, but is expected.");
               Assert.IsTrue((fsei.Attributes & System.IO.FileAttributes.System) != 0, "The System attribute is not found, but is expected.");
            }


            Assert.AreEqual(".", fsei.FileName, "The file names are not equal, but are expected to be.");
            Assert.AreEqual(drive.Name, fsei.FullPath, "The full paths are not equal, but are expected to be.");
         }
      }
   }
}
