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
   public partial class AlphaFS_PhysicalDriveInfoTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void AlphaFS_Device_GetPhysicalDriveInfo_FromDevicePath_Success()
      {
         UnitTestConstants.PrintUnitTestHeader(false);


         var deviceCount = 0;

         var sourceDrive = UnitTestConstants.SysDrive + @"\";
         var sourceVolume = Alphaleonis.Win32.Filesystem.Volume.GetVolumeGuid(sourceDrive);
         var devicePath = Alphaleonis.Win32.Filesystem.Device.GetPhysicalDriveInfo(sourceDrive).DevicePath;

         var pDrive = Alphaleonis.Win32.Filesystem.Device.GetPhysicalDriveInfo(devicePath);


         Console.WriteLine();
         Console.WriteLine("#{0:000}\tDevice Path: [{1}]", ++deviceCount, devicePath);

         UnitTestConstants.Dump(pDrive, -17);


         Assert.IsNotNull(pDrive);
         

         Assert.IsNotNull(pDrive.VolumeGuids);
         Assert.IsTrue(pDrive.VolumeGuids.Contains(sourceVolume));


         Assert.IsNotNull(pDrive.LogicalDrives);
         Assert.IsTrue(pDrive.LogicalDrives.Contains(sourceDrive));
      }
   }
}
