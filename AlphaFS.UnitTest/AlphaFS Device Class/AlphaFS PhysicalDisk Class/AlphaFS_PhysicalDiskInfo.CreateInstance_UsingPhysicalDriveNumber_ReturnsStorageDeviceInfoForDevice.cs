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
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Globalization;

namespace AlphaFS.UnitTest
{
   public partial class AlphaFS_PhysicalDiskInfoTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void AlphaFS_PhysicalDiskInfo_CreateInstance_UsingPhysicalDriveNumber_ReturnsStorageDeviceInfoForDevice()
      {
         UnitTestConstants.PrintUnitTestHeader(false);

         var physicalDisks = Alphaleonis.Win32.Filesystem.Volume.QueryAllDosDevices().Where(device => device.StartsWith("PhysicalDrive", StringComparison.OrdinalIgnoreCase)).ToArray();

         Console.WriteLine("Found: [{0}] physical drives.\n", physicalDisks.Length);


         for (var physicalDiskNumber = 0; physicalDiskNumber < physicalDisks.Length; physicalDiskNumber++)
         {
            Console.WriteLine("#{0:000}\tInput Physical Disk Path: [{1}{2}]", physicalDiskNumber + 1, Alphaleonis.Win32.Filesystem.Path.PhysicalDrivePrefix, physicalDiskNumber);

            var pDiskInfo = new Alphaleonis.Win32.Device.PhysicalDiskInfo(physicalDiskNumber);

            UnitTestConstants.Dump(pDiskInfo);

            UnitTestConstants.Dump(pDiskInfo.StorageAdapterInfo, true);

            UnitTestConstants.Dump(pDiskInfo.StorageDeviceInfo, true);

            UnitTestConstants.Dump(pDiskInfo.StoragePartitionInfo, true);


            Assert.IsNotNull(pDiskInfo);

            Assert.IsNotNull(pDiskInfo.LogicalDrives);

            Assert.IsNotNull(pDiskInfo.VolumeGuids);


            // DosDeviceName should contain "X\DR" for device.

            Assert.IsTrue(pDiskInfo.DosDeviceName.Contains(string.Format(CultureInfo.InvariantCulture, @"{0}\DR", physicalDiskNumber)));


            // PartitionNumber should be 0 for device on dynamic disk.

            if (pDiskInfo.StoragePartitionInfo.OnDynamicDisk)
               Assert.AreNotEqual(0, pDiskInfo.StorageDeviceInfo.PartitionNumber);

            else
            {
               // PartitionNumber should be -1 for CDRom.

               if (pDiskInfo.StorageDeviceInfo.DeviceType == Alphaleonis.Win32.Device.DeviceType.CDRom)
                  Assert.AreEqual(-1, pDiskInfo.StorageDeviceInfo.PartitionNumber);

               // PartitionNumber should be 0 for device.
               else
                  Assert.AreEqual(0, pDiskInfo.StorageDeviceInfo.PartitionNumber);
            }


            // TotalSize depends on dynamic disk on device.

            if (pDiskInfo.StoragePartitionInfo.OnDynamicDisk)
               Assert.AreNotEqual(pDiskInfo.StorageDeviceInfo.TotalSize, pDiskInfo.StoragePartitionInfo.TotalSize);
            else
               Assert.AreEqual(pDiskInfo.StorageDeviceInfo.TotalSize, pDiskInfo.StoragePartitionInfo.TotalSize);



            Assert.AreEqual(physicalDiskNumber, pDiskInfo.StorageAdapterInfo.DeviceNumber);

            Assert.AreEqual(physicalDiskNumber, pDiskInfo.StorageDeviceInfo.DeviceNumber);

            Assert.AreEqual(physicalDiskNumber, pDiskInfo.StoragePartitionInfo.DeviceNumber);
            

            // Show all partition information.

            if (null != pDiskInfo.StoragePartitionInfo && null != pDiskInfo.StoragePartitionInfo.GptPartitionInfo)
               foreach (var partition in pDiskInfo.StoragePartitionInfo.GptPartitionInfo)
                  UnitTestConstants.Dump(partition, true);

            if (null != pDiskInfo.StoragePartitionInfo && null != pDiskInfo.StoragePartitionInfo.MbrPartitionInfo)
               foreach (var partition in pDiskInfo.StoragePartitionInfo.MbrPartitionInfo)
                  UnitTestConstants.Dump(partition, true);


            Console.WriteLine();
         }
      }
   }
}
