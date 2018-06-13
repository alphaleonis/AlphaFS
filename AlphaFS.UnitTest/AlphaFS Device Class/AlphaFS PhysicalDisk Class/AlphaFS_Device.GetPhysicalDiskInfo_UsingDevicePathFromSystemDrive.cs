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
      public void AlphaFS_Device_GetPhysicalDiskInfo_UsingDevicePathFromSystemDrive_Success()
      {
         UnitTestConstants.PrintUnitTestHeader(false);

         var deviceCount = 0;

         var sourceDrive = UnitTestConstants.SysDrive;

         var sourceVolume = Alphaleonis.Win32.Filesystem.Volume.GetVolumeGuid(sourceDrive);

         var devicePath = Alphaleonis.Win32.Device.Local.GetPhysicalDiskInfo(sourceDrive).DevicePath;

         Console.WriteLine("#{0:000}\tInput Device Path: [{1}]", ++deviceCount, devicePath);


         var pDisk = Alphaleonis.Win32.Device.Local.GetPhysicalDiskInfo(devicePath);
         

         UnitTestConstants.Dump(pDisk);

         UnitTestConstants.Dump(pDisk.StorageAdapterInfo, true);

         UnitTestConstants.Dump(pDisk.StorageDeviceInfo, true);

         UnitTestConstants.Dump(pDisk.StoragePartitionInfo, true);
         Console.WriteLine();


         Assert.IsNotNull(pDisk);

         Assert.IsNotNull(pDisk.LogicalDrives);

         Assert.IsNotNull(pDisk.VolumeGuids);

         Assert.AreEqual(pDisk.LogicalDrives.Contains(sourceDrive, StringComparer.OrdinalIgnoreCase), pDisk.ContainsVolume(sourceDrive));

         Assert.AreEqual(pDisk.VolumeGuids.Contains(sourceVolume, StringComparer.OrdinalIgnoreCase), pDisk.ContainsVolume(sourceVolume));


         // Show all partition information.

         if (null != pDisk.StoragePartitionInfo && null != pDisk.StoragePartitionInfo.GptPartitionInfo)
         {
            foreach (var partition in pDisk.StoragePartitionInfo.GptPartitionInfo)
               UnitTestConstants.Dump(partition, true);
         }

         if (null != pDisk.StoragePartitionInfo && null != pDisk.StoragePartitionInfo.MbrPartitionInfo)
         {
            foreach (var partition in pDisk.StoragePartitionInfo.MbrPartitionInfo)
               UnitTestConstants.Dump(partition, true);
         }

         Console.WriteLine();
      }
   }
}
