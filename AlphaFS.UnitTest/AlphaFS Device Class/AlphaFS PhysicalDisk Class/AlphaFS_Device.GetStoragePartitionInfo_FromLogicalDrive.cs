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
      public void AlphaFS_Device_GetStoragePartitionInfo_FromLogicalDrive_Success()
      {
         UnitTestConstants.PrintUnitTestHeader(false);

         var gotDisk = false;
         var driveCount = 0;
         

         foreach (var driveInfo in Alphaleonis.Win32.Filesystem.DriveInfo.GetDrives())
         {
            if (driveInfo.DriveType == System.IO.DriveType.NoRootDirectory || driveInfo.DriveType == System.IO.DriveType.Network)
               continue;

            var storagePartitionInfo = Alphaleonis.Win32.Device.Local.GetStoragePartitionInfo(driveInfo.Name);

            Console.WriteLine("#{0:000}\tInput Logical Drive: [{1}]\t\t{2}", ++driveCount, driveInfo.Name, storagePartitionInfo.ToString());

            UnitTestConstants.Dump(storagePartitionInfo);


            Assert.IsNotNull(storagePartitionInfo);


            if (null != storagePartitionInfo.GptPartitionInfo)
            {
               gotDisk = true;

               foreach (var partition in storagePartitionInfo.GptPartitionInfo)
                  UnitTestConstants.Dump(partition, true);
            }


            if (null != storagePartitionInfo.MbrPartitionInfo)
            {
               gotDisk = true;

               foreach (var partition in storagePartitionInfo.MbrPartitionInfo)
                  UnitTestConstants.Dump(partition, true);
            }


            //if (drive.DriveType == System.IO.DriveType.Fixed)
            //{
            //   gotDisk = true;
            //   Assert.AreEqual(Alphaleonis.Win32.Filesystem.StorageDeviceType.Disk, storagePartitionInfo.DeviceType);
            //}


            //if (drive.DriveType == System.IO.DriveType.CDRom)
            //   Assert.AreEqual(Alphaleonis.Win32.Filesystem.StorageDeviceType.CDRom, storagePartitionInfo.DeviceType);


            Console.WriteLine();
         }


         Assert.IsTrue(gotDisk);
      }
   }
}
