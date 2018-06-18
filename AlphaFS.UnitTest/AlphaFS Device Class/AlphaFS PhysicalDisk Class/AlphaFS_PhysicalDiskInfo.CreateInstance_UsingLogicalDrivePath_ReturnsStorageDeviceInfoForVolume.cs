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
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AlphaFS.UnitTest
{
   public partial class AlphaFS_PhysicalDiskInfoTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void AlphaFS_PhysicalDiskInfo_CreateInstance_UsingLogicalDrivePath_ReturnsStorageDeviceInfoForVolume()
      {
         UnitTestConstants.PrintUnitTestHeader(false);

         var driveCount = 0;
         Alphaleonis.Win32.Device.PhysicalDiskInfo prevDiskInfo = null;

         foreach (var driveInfo in System.IO.DriveInfo.GetDrives())
         {
            // System.UnauthorizedAccessException: (5) Access is denied.
            if (driveInfo.DriveType == System.IO.DriveType.Network)
            {
               Console.WriteLine("#{0:000}\tSkipped Network drive: [{1}]\n", ++driveCount, driveInfo.Name);
               continue;
            }

            if (driveInfo.DriveType == System.IO.DriveType.NoRootDirectory)
            {
               Console.WriteLine("#{0:000}\tSkipped NoRootDirectory drive: [{1}]\n", ++driveCount, driveInfo.Name);
               continue;
            }


            Console.WriteLine("#{0:000}\tInput Logical Drive Path: [{1}]", ++driveCount, driveInfo.Name);


            var pDiskInfo = new Alphaleonis.Win32.Device.PhysicalDiskInfo(driveInfo.Name);

            Assert.AreNotEqual(prevDiskInfo, pDiskInfo);

            prevDiskInfo = pDiskInfo;


            UnitTestConstants.Dump(pDiskInfo);

            UnitTestConstants.Dump(pDiskInfo.StorageAdapterInfo, true);

            UnitTestConstants.Dump(pDiskInfo.StorageDeviceInfo, true);

            UnitTestConstants.Dump(pDiskInfo.StoragePartitionInfo, true);


            Assert.IsNotNull(pDiskInfo);

            Assert.IsNotNull(pDiskInfo.LogicalDrives);

            Assert.IsNotNull(pDiskInfo.VolumeGuids);


            // DosDeviceName should be different for logical drive.

            Assert.AreNotEqual(pDiskInfo.DosDeviceName, pDiskInfo.PhysicalDeviceObjectName);


            // PartitionNumber should be -1 for CDRom.

            if (pDiskInfo.StorageDeviceInfo.DeviceType == Alphaleonis.Win32.Device.StorageDeviceType.CDRom)
               Assert.AreEqual(-1, pDiskInfo.StorageDeviceInfo.PartitionNumber);

            // PartitionNumber should be > 0 for logical drive because it is not the device.
            else
               Assert.AreNotEqual(0, pDiskInfo.StorageDeviceInfo.PartitionNumber);




            Assert.AreEqual(pDiskInfo.StorageDeviceInfo.DeviceNumber, pDiskInfo.StorageAdapterInfo.DeviceNumber);

            Assert.AreEqual(pDiskInfo.StorageDeviceInfo.DeviceNumber, pDiskInfo.StoragePartitionInfo.DeviceNumber);


            Console.WriteLine();
         }
      }
   }
}
