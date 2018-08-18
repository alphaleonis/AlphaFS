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


      /*
Test method AlphaFS.UnitTest.EnumerationTest.AlphaFS_Device_EnumeratePhysicalDisks_ReturnsStorageDeviceInfoForDevice threw exception: 

System.IO.IOException: (998) Invalid access to memory location: [Buffer size: 2048. Path: \\?\pcistor#disk&ven_rsper&prod_rts5208lun0&rev_1.00#0000#{53f56307-b6bf-11d0-94f2-00a0c91efb8b}]

    at Alphaleonis.Win32.NativeError.ThrowException(UInt32 errorCode, String readPath, String writePath) in C:\Users\jjangli\Documents\GitHub\AlphaFS\AlphaFS\NativeError.cs:line 189
   at Alphaleonis.Win32.NativeError.ThrowException(Int32 errorCode, String readPath) in C:\Users\jjangli\Documents\GitHub\AlphaFS\AlphaFS\NativeError.cs:line 50
   at Alphaleonis.Utils.IsValidHandle(SafeHandle handle, Int32 lastError, String path, Boolean throwException) in C:\Users\jjangli\Documents\GitHub\AlphaFS\AlphaFS\Utils.cs:line 228
   at Alphaleonis.Utils.GetDoubledBufferSizeOrThrowException(SafeHandle safeBuffer, Int32 lastError, Int32 bufferSize, String pathForException) in C:\Users\jjangli\Documents\GitHub\AlphaFS\AlphaFS\Utils.cs:line 96
   at Alphaleonis.Win32.Device.Local.InvokeDeviceIoData[T](SafeFileHandle safeFileHandle, IoControlCode controlCode, T anyObject, String pathForException, Int32 size) in C:\Users\jjangli\Documents\GitHub\AlphaFS\AlphaFS\Device\Local Class\Local.InvokeDeviceIoData.cs:line 67
   at Alphaleonis.Win32.Device.Local.SetStorageDeviceInfoData(Boolean isElevated, SafeFileHandle safeFileHandle, String pathForException, StorageDeviceInfo storageDeviceInfo) in C:\Users\jjangli\Documents\GitHub\AlphaFS\AlphaFS\Device\Local Class\Local.SetStorageDeviceInfoData.cs:line 39
   at Alphaleonis.Win32.Device.Local.GetStorageDeviceInfo(Boolean isElevated, Boolean isDevice, Int32 deviceNumber, String devicePath, String& localDevicePath) in C:\Users\jjangli\Documents\GitHub\AlphaFS\AlphaFS\Device\Local Class\Local.GetStorageDeviceInfo.cs:line 60
   at Alphaleonis.Win32.Device.Local.<EnumeratePhysicalDisksCore>d__25.MoveNext() in C:\Users\jjangli\Documents\GitHub\AlphaFS\AlphaFS\Device\Local Class\Local.EnumeratePhysicalDisks.cs:line 60
   at System.Linq.Buffer`1..ctor(IEnumerable`1 source)
   at System.Linq.OrderedEnumerable`1.<GetEnumerator>d__1.MoveNext()
   at System.Linq.Buffer`1..ctor(IEnumerable`1 source)
   at System.Linq.Enumerable.ToArray[TSource](IEnumerable`1 source)
   at AlphaFS.UnitTest.EnumerationTest.AlphaFS_Device_EnumeratePhysicalDisks_ReturnsStorageDeviceInfoForDevice() in C:\Users\jjangli\Documents\GitHub\AlphaFS\AlphaFS.UnitTest\AlphaFS Device Class\AlphaFS PhysicalDisk Class\AlphaFS_Device.EnumeratePhysicalDisks_ReturnsStorageDeviceInfoForDevice.cs:line 44
       */


      [TestMethod]
      public void AlphaFS_Device_EnumeratePhysicalDisks_ReturnsStorageDeviceInfoForDevice()
      {
         UnitTestConstants.PrintUnitTestHeader(false);
         
         var driveCount = 0;

         var physicalDrives = Alphaleonis.Win32.Filesystem.Volume.QueryAllDosDevices().Where(device => device.StartsWith("PhysicalDrive", StringComparison.OrdinalIgnoreCase)).ToArray();

         var cdRoms = Alphaleonis.Win32.Filesystem.Volume.QueryAllDosDevices().Where(device => device.StartsWith("CdRom", StringComparison.OrdinalIgnoreCase)).ToArray();

         var pDrives = Alphaleonis.Win32.Device.Local.EnumeratePhysicalDisks().OrderBy(pDiskInfo => pDiskInfo.StorageDeviceInfo.DeviceNumber).ThenByDescending(pDiskInfo => pDiskInfo.StorageDeviceInfo.PartitionNumber).ToArray();

         var allPhysicalDrives = physicalDrives.Length + cdRoms.Length;


         Console.WriteLine("Found: [{0}] physical drives.\n", allPhysicalDrives);

         Assert.AreEqual(allPhysicalDrives, pDrives.Length);


         foreach (var pDiskInfo in pDrives)
         {
            Console.WriteLine("#{0:000}\tPhysical Disk: [{1}]", driveCount, pDiskInfo.StorageDeviceInfo.DeviceNumber);

            UnitTestConstants.Dump(pDiskInfo);

            UnitTestConstants.Dump(pDiskInfo.StorageAdapterInfo, true);

            UnitTestConstants.Dump(pDiskInfo.StorageDeviceInfo, true);

            UnitTestConstants.Dump(pDiskInfo.StoragePartitionInfo, true);


            Assert.IsNotNull(pDiskInfo);

            Assert.IsNotNull(pDiskInfo.LogicalDrives);

            Assert.IsNotNull(pDiskInfo.VolumeGuids);


            // DosDeviceName should be the same for device.

            Assert.AreEqual(pDiskInfo.DosDeviceName, pDiskInfo.PhysicalDeviceObjectName);


            // PartitionNumber should be 0 for logical drive on dynamic disk.

            if (pDiskInfo.StoragePartitionInfo.OnDynamicDisk)
               Assert.AreNotEqual(0, pDiskInfo.StorageDeviceInfo.PartitionNumber);

            else
            {
               // PartitionNumber should be -1 for CDRom.

               if (pDiskInfo.StorageDeviceInfo.DeviceType == Alphaleonis.Win32.Device.DeviceType.CDRom)
                  Assert.AreEqual(-1, pDiskInfo.StorageDeviceInfo.PartitionNumber);

               // PartitionNumber should be > 0 for logical drive because it is not the device.
               else
                  Assert.AreEqual(0, pDiskInfo.StorageDeviceInfo.PartitionNumber);
            }


            // TotalSize depends on dynamic disk on device.

            if (pDiskInfo.StoragePartitionInfo.OnDynamicDisk)
               Assert.AreNotEqual(pDiskInfo.StorageDeviceInfo.TotalSize, pDiskInfo.StoragePartitionInfo.TotalSize);
            else
               Assert.AreEqual(pDiskInfo.StorageDeviceInfo.TotalSize, pDiskInfo.StoragePartitionInfo.TotalSize);


            // Show all partition information.

            if (null != pDiskInfo.StoragePartitionInfo && null != pDiskInfo.StoragePartitionInfo.GptPartitionInfo)
               foreach (var partition in pDiskInfo.StoragePartitionInfo.GptPartitionInfo)
                  UnitTestConstants.Dump(partition, true);

            if (null != pDiskInfo.StoragePartitionInfo && null != pDiskInfo.StoragePartitionInfo.MbrPartitionInfo)
               foreach (var partition in pDiskInfo.StoragePartitionInfo.MbrPartitionInfo)
                  UnitTestConstants.Dump(partition, true);


            driveCount++;

            Console.WriteLine();
         }


         Assert.IsTrue(driveCount > 0, "No physical disks enumerated, but it is expected.");
      }
   }
}
