/*  Copyright (C) 2008-2017 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
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
   public partial class AlphaFS_PhysicalDriveInfoTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void AlphaFS_Device_EnumeratePhysicalDrives_Local_Success()
      {
         UnitTestConstants.PrintUnitTestHeader(false);


         var pDriveCount = 0;
         var pDrives = Alphaleonis.Win32.Filesystem.Device.EnumeratePhysicalDrives().OrderBy(pDriveInfo => pDriveInfo.StorageDeviceInfo.DeviceNumber).ThenBy(pDriveInfo => pDriveInfo.StorageDeviceInfo.PartitionNumber).ToArray();

         foreach (var pDrive in pDrives)
         {
            Console.WriteLine();
            Console.WriteLine("#{0:000}\tPhysical Drive: [{1}]\t\t{2}\t\t{3}", ++pDriveCount, pDrive.StorageDeviceInfo.DeviceNumber, pDrive.StorageAdapterInfo.ToString(), pDrive.StorageDeviceInfo.ToString());

            UnitTestConstants.Dump(pDrive, -24);

            UnitTestConstants.Dump(pDrive.StorageAdapterInfo, -28, true);

            UnitTestConstants.Dump(pDrive.StorageDeviceInfo, -17, true);
            Console.WriteLine();
         }


         Assert.IsTrue(pDrives.Length > 0, "No physical drives enumerated, but it is expected.");
      }
   }
}
