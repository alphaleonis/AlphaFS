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

namespace AlphaFS.UnitTest
{
   public partial class AlphaFS_FileSystemEntryInfoTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void AlphaFS_Directory_GetFileSystemEntryInfo_LogicalDrives_Local_Success()
      {
         AlphaFS_Directory_GetFileSystemEntryInfo_LogicalDrives();
      }


      private void AlphaFS_Directory_GetFileSystemEntryInfo_LogicalDrives()
      {
         UnitTestConstants.PrintUnitTestHeader(false);
         
         var drives = Alphaleonis.Win32.Filesystem.DriveInfo.GetDrives().ToArray();

         foreach (var driveInfo in drives)
         {
            Console.WriteLine("Input Logical Drive: [{0}]", driveInfo.Name);

            try
            { 
               var fsei = Alphaleonis.Win32.Filesystem.Directory.GetFileSystemEntryInfo(driveInfo.Name);

               UnitTestConstants.Dump(fsei);


               Assert.IsTrue(fsei.GetType().IsEquivalentTo(typeof(Alphaleonis.Win32.Filesystem.FileSystemEntryInfo)));
               Assert.IsTrue((fsei.Attributes & System.IO.FileAttributes.Directory) != 0, "The Directory attribute is not found, but is expected.");


               // Fixed local drives should have these attributes.
               if (new Alphaleonis.Win32.Filesystem.DriveInfo(driveInfo.Name).DriveType == System.IO.DriveType.Fixed)
               {
                  Assert.IsTrue((fsei.Attributes & System.IO.FileAttributes.Hidden) != 0, "The Hidden attribute is not found, but is expected.");
                  Assert.IsTrue((fsei.Attributes & System.IO.FileAttributes.System) != 0, "The System attribute is not found, but is expected.");
               }


               Assert.AreEqual(".", fsei.FileName, "The file names are not equal, but are expected to be.");
               Assert.AreEqual(driveInfo.Name, fsei.FullPath, "The full paths are not equal, but are expected to be.");
            }
            catch (Exception ex)
            {
               Console.WriteLine("\n\tCaught UNEXPECTED Exception: [{0}] {1}", ex.GetType().Name, ex.Message);
            }


            Console.WriteLine();
         }
      }
   }
}
