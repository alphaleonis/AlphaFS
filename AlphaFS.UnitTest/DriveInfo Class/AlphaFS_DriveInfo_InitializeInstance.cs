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
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AlphaFS.UnitTest
{
   /// <summary>This is a test class for DriveInfo and is intended to contain all DriveInfo Unit Tests.</summary>
   [TestClass]
   public class DriveInfoTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>

      
      [TestMethod]
      public void AlphaFS_DriveInfo_InitializeInstance_LocalAndNetwork_Success()
      {
         DriveInfo_InitializeInstance(false);
         DriveInfo_InitializeInstance(true);
      }


      [TestMethod]
      public void AlphaFS_DriveInfo_GetDrives_LocalAndNetwork_Success()
      {
         DriveInfo_InitializeInstance_GetDrives(false);
      }




      private void DriveInfo_InitializeInstance(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);
         Console.WriteLine();


         var tempPath = UnitTestConstants.SysDrive[0].ToString();

         if (isNetwork)
            // Only using a drive letter results in a wrong UNC path.
            tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(UnitTestConstants.SysDrive);


         Console.WriteLine("Input Drive Path: [{0}]", tempPath);


         var actual = new Alphaleonis.Win32.Filesystem.DriveInfo(tempPath);

         Assert.IsTrue(actual.IsReady);
         Assert.IsTrue(actual.IsVolume);

         if (isNetwork)
            Assert.IsTrue(actual.IsUnc);
         else
            Assert.IsFalse(actual.IsUnc);


         // System.IO.DriveInfo cannot handle UNC paths.

         if (!isNetwork)
         {
            // Even 1 byte more or less results in failure, so do these tests asap.
            
            var expected = new System.IO.DriveInfo(tempPath);
            

            Assert.AreEqual(expected.AvailableFreeSpace, actual.AvailableFreeSpace, "AvailableFreeSpace AlphaFS != System.IO");
            Assert.AreEqual(expected.TotalFreeSpace, actual.TotalFreeSpace, "TotalFreeSpace AlphaFS != System.IO");
            Assert.AreEqual(expected.TotalSize, actual.TotalSize, "TotalSize AlphaFS != System.IO");


            Assert.AreEqual(expected.DriveFormat, actual.DriveFormat, "DriveFormat AlphaFS != System.IO");
            Assert.AreEqual(expected.DriveType, actual.DriveType, "DriveType AlphaFS != System.IO");
            Assert.AreEqual(expected.IsReady, actual.IsReady, "IsReady AlphaFS != System.IO");
            Assert.AreEqual(expected.Name, actual.Name, "Name AlphaFS != System.IO");
            Assert.AreEqual(expected.RootDirectory.ToString(), actual.RootDirectory.ToString(), "RootDirectory AlphaFS != System.IO");
            Assert.AreEqual(expected.VolumeLabel, actual.VolumeLabel, "VolumeLabel AlphaFS != System.IO");


            UnitTestConstants.Dump(expected, -21);
            Console.WriteLine();
         }


         UnitTestConstants.Dump(actual, -21);
         UnitTestConstants.Dump(actual.DiskSpaceInfo, -26);
         UnitTestConstants.Dump(actual.VolumeInfo, -26);

         Console.WriteLine();
      }


      private void DriveInfo_InitializeInstance_GetDrives(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);


         var drives = Alphaleonis.Win32.Filesystem.DriveInfo.GetDrives();

         foreach (var drive in drives)
         {
            UnitTestConstants.Dump(drive, -21);
            Console.WriteLine();
         }


         Assert.IsTrue(drives.Length > 0);

         Assert.AreEqual(drives[0].Name[0], UnitTestConstants.SysDrive[0]);
      }
   }
}

