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
      public void AlphaFS_Device_GetStorageAdapterInfo_UsingDevicePathFromSystemDrive_Success()
      {
         UnitTestConstants.PrintUnitTestHeader(false);
         
         var deviceCount = 0;

         var sourceDrive = UnitTestConstants.SysDrive;
         var devicePath = Alphaleonis.Win32.Device.Local.GetPhysicalDiskInfo(sourceDrive).DevicePath;

         var pDisk = Alphaleonis.Win32.Device.Local.GetPhysicalDiskInfo(devicePath);


         Console.WriteLine("#{0:000}\tInput Device Path: [{1}]\t\t{2}", ++deviceCount, devicePath, pDisk.StorageAdapterInfo.ToString());


         var storageAdapterInfo = Alphaleonis.Win32.Device.Local.GetStorageAdapterInfo(devicePath);

         
         UnitTestConstants.Dump(storageAdapterInfo);

         Assert.IsNotNull(storageAdapterInfo);

         Assert.IsNotNull(pDisk);

         Assert.AreEqual(-1, pDisk.StorageDeviceInfo.PartitionNumber);


         if (Alphaleonis.Win32.Security.ProcessContext.IsElevatedProcess)
         {
            Assert.AreEqual(pDisk.StorageAdapterInfo, storageAdapterInfo);

            Assert.AreNotEqual(Alphaleonis.Win32.Device.StorageBusType.Unknown, storageAdapterInfo.BusType);
         }

         else
         {
            Assert.AreNotEqual(pDisk.StorageAdapterInfo, storageAdapterInfo);

            Assert.AreEqual(pDisk.StorageAdapterInfo.BusType, storageAdapterInfo.BusType);
         }
      }
   }
}
