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

namespace AlphaFS.UnitTest
{
   public partial class EnumerationTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void AlphaFS_Device_EnumeratePhysicalDisks_Local_Success()
      {
         UnitTestConstants.PrintUnitTestHeader(false);

         var pDriveCount = 0;

         var physicalDrives = Alphaleonis.Win32.Filesystem.Volume.QueryAllDosDevices().Where(device => device.StartsWith("PhysicalDrive", StringComparison.OrdinalIgnoreCase)).ToArray();

         var pDrives = Alphaleonis.Win32.Device.Local.EnumeratePhysicalDisks().OrderBy(pDiskInfo => pDiskInfo.StorageDeviceInfo.DeviceNumber).ThenBy(pDiskInfo => pDiskInfo.StorageDeviceInfo.PartitionNumber).ToArray();


         Console.WriteLine("Found: [{0}] physical drives.\n", physicalDrives.Length);

         Assert.AreEqual(physicalDrives.Length, pDrives.Length);


         foreach (var pDisk in pDrives)
         {
            Console.WriteLine("#{0:000}\tPhysical Disk: [{1}]\t\t{2}\t\t{3}", ++pDriveCount, pDisk.StorageDeviceInfo.DeviceNumber, pDisk.StorageAdapterInfo.ToString(), pDisk.StorageDeviceInfo.ToString());

            UnitTestConstants.Dump(pDisk);

            UnitTestConstants.Dump(pDisk.StorageAdapterInfo, true);

            UnitTestConstants.Dump(pDisk.StorageDeviceInfo, true);

            UnitTestConstants.Dump(pDisk.StoragePartitionInfo, true);


            // Show all partition information.

            if (null != pDisk.StoragePartitionInfo && null != pDisk.StoragePartitionInfo.GptPartitionInfo)
               foreach (var partition in pDisk.StoragePartitionInfo.GptPartitionInfo)
                  UnitTestConstants.Dump(partition, true);

            if (null != pDisk.StoragePartitionInfo && null != pDisk.StoragePartitionInfo.MbrPartitionInfo)
               foreach (var partition in pDisk.StoragePartitionInfo.MbrPartitionInfo)
                  UnitTestConstants.Dump(partition, true);


            Console.WriteLine();
         }


         Assert.IsTrue(pDrives.Length > 0, "No physical disks enumerated, but it is expected.");
      }
   }
}
