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
      public void AlphaFS_Device_GetStorageDeviceInfo_UsingDevicePathFromSystemDrive_Success()
      {
         UnitTestConstants.PrintUnitTestHeader(false);

         var deviceCount = 0;

         var sourceDrive = UnitTestConstants.SysDrive;

         var sourceVolume = Alphaleonis.Win32.Filesystem.Volume.GetVolumeGuid(sourceDrive);

         var devicePath = new Alphaleonis.Win32.Device.PhysicalDiskInfo(sourceDrive).DevicePath;

         Console.WriteLine("#{0:000}\tInput Device Path: [{1}]", ++deviceCount, devicePath);


         var pDiskInfo = new Alphaleonis.Win32.Device.PhysicalDiskInfo(devicePath);
         
         var storageDeviceInfo = Alphaleonis.Win32.Device.Local.GetStorageDeviceInfo(devicePath);

         
         UnitTestConstants.Dump(pDiskInfo);

         UnitTestConstants.Dump(pDiskInfo.StorageAdapterInfo, true);

         UnitTestConstants.Dump(pDiskInfo.StorageDeviceInfo, true);

         UnitTestConstants.Dump(pDiskInfo.StoragePartitionInfo, true);
         Console.WriteLine();


         Assert.IsNotNull(storageDeviceInfo);

         Assert.IsNotNull(pDiskInfo);


         Assert.AreEqual(pDiskInfo.StorageDeviceInfo, storageDeviceInfo);


         Assert.IsNotNull(pDiskInfo.LogicalDrives);

         Assert.IsNotNull(pDiskInfo.VolumeGuids);

         Assert.IsNotNull(pDiskInfo.DosDeviceName);


         Assert.IsTrue(pDiskInfo.LogicalDrives.Contains(sourceDrive, StringComparer.OrdinalIgnoreCase));

         Assert.IsTrue(pDiskInfo.ContainsVolume(sourceDrive));


         Assert.IsTrue(pDiskInfo.VolumeGuids.Contains(sourceVolume, StringComparer.OrdinalIgnoreCase));

         Assert.IsTrue(pDiskInfo.ContainsVolume(sourceVolume));
         

         Assert.AreNotEqual(-1, pDiskInfo.StorageDeviceInfo.PartitionNumber);
      }
   }
}
