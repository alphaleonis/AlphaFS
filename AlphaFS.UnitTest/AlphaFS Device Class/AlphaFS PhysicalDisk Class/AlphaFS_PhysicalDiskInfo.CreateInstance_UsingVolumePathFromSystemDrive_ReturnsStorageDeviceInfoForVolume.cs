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
   public partial class AlphaFS_PhysicalDiskInfoTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void AlphaFS_PhysicalDiskInfo_CreateInstance_UsingVolumePathFromSystemDrive_ReturnsStorageDeviceInfoForVolume()
      {
         UnitTestConstants.PrintUnitTestHeader(false);

         var volumeCount = 0;

         var sourceDrive = UnitTestConstants.SysDrive;

         var sourceVolume = Alphaleonis.Win32.Filesystem.Volume.GetVolumeGuid(sourceDrive);

         Console.WriteLine("#{0:000}\tInput Volume Path: [{1}]", ++volumeCount, sourceVolume);


         var pDiskInfo = new Alphaleonis.Win32.Device.PhysicalDiskInfo(sourceVolume);

         
         UnitTestConstants.Dump(pDiskInfo);

         UnitTestConstants.Dump(pDiskInfo.StorageAdapterInfo, true);

         UnitTestConstants.Dump(pDiskInfo.StorageDeviceInfo, true);

         UnitTestConstants.Dump(pDiskInfo.StoragePartitionInfo, true);
         

         Assert.IsNotNull(pDiskInfo);

         Assert.IsNotNull(pDiskInfo.LogicalDrives);

         Assert.IsNotNull(pDiskInfo.VolumeGuids);


         // DosDeviceName should be different for volume.

         Assert.AreNotEqual(pDiskInfo.DosDeviceName, pDiskInfo.PhysicalDeviceObjectName);


         // PartitionNumber should be > 0 for volume because it is not the device.

         Assert.AreNotEqual(0, pDiskInfo.StorageDeviceInfo.PartitionNumber);


         // ContainsVolume should find volumes/logical drives.

         Assert.IsTrue(pDiskInfo.LogicalDrives.Contains(sourceDrive, StringComparer.OrdinalIgnoreCase));

         Assert.IsTrue(pDiskInfo.ContainsVolume(sourceDrive));


         Assert.IsTrue(pDiskInfo.VolumeGuids.Contains(sourceVolume, StringComparer.OrdinalIgnoreCase));

         Assert.IsTrue(pDiskInfo.ContainsVolume(sourceVolume));


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
