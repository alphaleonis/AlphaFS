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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AlphaFS.UnitTest
{
   public partial class AlphaFS_VolumeTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void AlphaFS_Volume_GetXxx_Local_Success()
      {
         UnitTestConstants.PrintUnitTestHeader(false);


         var deviceNamePrefix = Alphaleonis.Win32.Filesystem.Path.DevicePrefix + "HarddiskVolume";

         var volumePrefix = Alphaleonis.Win32.Filesystem.Path.VolumePrefix + "{";
         
         var logicalDriveCount = 0;


         foreach (var driveInfo in System.IO.DriveInfo.GetDrives())
         {
            var isCDRom = driveInfo.DriveType == System.IO.DriveType.CDRom;
            var isNetwork = driveInfo.DriveType == System.IO.DriveType.Network;
            var isNoRootDirectory = driveInfo.DriveType == System.IO.DriveType.NoRootDirectory;


            Console.WriteLine("#{0:000}\tInput Logical Drive Path: [{1}]\n", ++logicalDriveCount, driveInfo.Name);
            



            // GetVolumeDeviceName: "C:\" --> "\Device\HarddiskVolume4"

            var deviceNameFromLogicalDrive = Alphaleonis.Win32.Filesystem.Volume.QueryDosDevice(driveInfo.Name);

            Console.WriteLine("\tGetVolumeDeviceName\t\t\t: [{0}]", deviceNameFromLogicalDrive);


            Assert.IsNotNull(deviceNameFromLogicalDrive);


            if (isNetwork)
            {
               Assert.IsTrue(deviceNameFromLogicalDrive.StartsWith(Alphaleonis.Win32.Filesystem.Path.DosDeviceLanmanPrefix + @";" + driveInfo.Name[0] + ":", StringComparison.OrdinalIgnoreCase));
            }
               
            


            if (!isNoRootDirectory && !isNetwork)
            {

               if (isCDRom)
                  Assert.IsTrue(deviceNameFromLogicalDrive.StartsWith(Alphaleonis.Win32.Filesystem.Path.DevicePrefix, StringComparison.OrdinalIgnoreCase));

               else
                  Assert.IsTrue(deviceNameFromLogicalDrive.StartsWith(deviceNamePrefix, StringComparison.OrdinalIgnoreCase));




               // GetVolumeGuid: "C:\" --> "\\?\Volume{db5044f9-bd1f-4243-ab97-4b985eb29e80}\"

               var volumeGuidFromLogicalDrive = Alphaleonis.Win32.Filesystem.Volume.GetVolumeGuid(driveInfo.Name);

               Console.WriteLine("\tGetVolumeGuid\t\t\t\t: [{0}]", volumeGuidFromLogicalDrive);

               Assert.IsNotNull(volumeGuidFromLogicalDrive);

               Assert.IsTrue(volumeGuidFromLogicalDrive.StartsWith(volumePrefix));




               // GetUniqueVolumeNameForPath: "C:\" --> "\\?\Volume{db5044f9-bd1f-4243-ab97-4b985eb29e80}\"

               var uniqueVolumeNameFromlDriveInputPath = Alphaleonis.Win32.Filesystem.Volume.GetUniqueVolumeNameForPath(driveInfo.Name);

               Console.WriteLine("\tGetUniqueVolumeNameForPath\t: [{0}]", uniqueVolumeNameFromlDriveInputPath);

               Assert.IsNotNull(uniqueVolumeNameFromlDriveInputPath);

               Assert.IsTrue(uniqueVolumeNameFromlDriveInputPath.StartsWith(volumePrefix));
            }


            if (isCDRom)
               Console.WriteLine("\tSkipped Logical Drive.\n");

            else
            {
               // GetVolumePathName: "C:\" or "C:\Windows" --> "C:\"

               var volumePathNameFromLogicalDrive = Alphaleonis.Win32.Filesystem.Volume.GetVolumePathName(driveInfo.Name);

               Console.WriteLine("\tGetVolumePathName\t\t\t: [{0}]\n", volumePathNameFromLogicalDrive);

               Assert.IsNotNull(volumePathNameFromLogicalDrive);

               Assert.AreEqual(driveInfo.Name, volumePathNameFromLogicalDrive);
            }
         }

         if (logicalDriveCount == 0)
            UnitTestAssert.InconclusiveBecauseResourcesAreUnavailable();
      }
   }
}
