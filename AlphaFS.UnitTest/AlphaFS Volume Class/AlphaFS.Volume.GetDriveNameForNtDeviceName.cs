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
   public partial class AlphaFS_VolumeTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void AlphaFS_Volume_GetDriveNameForNtDeviceName_And_GetVolumeGuidForNtDeviceName_Local_Success()
      {
         UnitTestConstants.PrintUnitTestHeader(false);


         var logicalDriveCount = 0;

         foreach (var drive in System.IO.DriveInfo.GetDrives())
         {
            if (drive.DriveType == System.IO.DriveType.Network)
               continue;


            var driveName = drive.Name;

            var dosDeviceName = Alphaleonis.Win32.Filesystem.Volume.GetVolumeDeviceName(driveName);

            Console.WriteLine("\n#{0:000}\tInput Path: [{1}]", ++logicalDriveCount, dosDeviceName);




            var driveNameResult = Alphaleonis.Win32.Filesystem.Volume.GetDriveNameForNtDeviceName(dosDeviceName);

            Console.WriteLine("\n\tGetDriveNameForNtDeviceName: [{0}]", driveNameResult ?? "null");

            Assert.AreEqual(driveName, driveNameResult);




            var driveGuidResult = Alphaleonis.Win32.Filesystem.Volume.GetVolumeGuidForNtDeviceName(dosDeviceName);

            Console.WriteLine("\n\t(Input Path) GetVolumeGuidForNtDeviceName: [{0}]\n", driveGuidResult ?? "null");

            Assert.AreEqual(Alphaleonis.Win32.Filesystem.Volume.GetVolumeGuid(driveName), driveGuidResult);
         }


         if (logicalDriveCount == 0)
            Assert.Inconclusive("No logical drives enumerated, but it is expected.");
      }
   }
}
