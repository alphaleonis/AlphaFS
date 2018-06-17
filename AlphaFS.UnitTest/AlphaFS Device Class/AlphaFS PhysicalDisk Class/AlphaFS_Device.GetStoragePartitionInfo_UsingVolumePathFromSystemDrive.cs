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
      public void AlphaFS_Device_GetStoragePartitionInfo_UsingVolumePathFromSystemDrive_Success()
      {
         UnitTestConstants.PrintUnitTestHeader(false);

         var volumeCount = 0;

         var sourceDrive = UnitTestConstants.SysDrive;

         var sourceVolume = Alphaleonis.Win32.Filesystem.Volume.GetVolumeGuid(sourceDrive);

         Console.WriteLine("#{0:000}\tInput Volume Path: [{1}]", ++volumeCount, sourceVolume);


         var pDiskInfo = new Alphaleonis.Win32.Device.PhysicalDiskInfo(sourceVolume);

         var storagePartitionInfo = Alphaleonis.Win32.Device.Local.GetStoragePartitionInfo(sourceVolume);

         
         UnitTestConstants.Dump(storagePartitionInfo);

         Assert.IsNotNull(storagePartitionInfo);

         Assert.AreEqual(0, storagePartitionInfo.DeviceNumber);


         Assert.IsNotNull(pDiskInfo);

         Assert.AreEqual(pDiskInfo.StorageDeviceInfo.DeviceNumber, storagePartitionInfo.DeviceNumber);

         Assert.AreEqual(pDiskInfo.StoragePartitionInfo, storagePartitionInfo);

         Assert.IsNotNull(pDiskInfo.DosDevice);


         // Show all partition information.

         if (null != storagePartitionInfo.GptPartitionInfo)
         {
            foreach (var partition in storagePartitionInfo.GptPartitionInfo)
               UnitTestConstants.Dump(partition, true);
         }


         if (null != storagePartitionInfo.MbrPartitionInfo)
         {
            foreach (var partition in storagePartitionInfo.MbrPartitionInfo)
               UnitTestConstants.Dump(partition, true);
         }
      }
   }
}
