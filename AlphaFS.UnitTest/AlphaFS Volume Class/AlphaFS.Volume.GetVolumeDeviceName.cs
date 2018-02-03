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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AlphaFS.UnitTest
{
   public partial class AlphaFS_VolumeTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void AlphaFS_Volume_GetVolumeDeviceName_FromLogicalDriveAndVolumeGuid_Local_Success()
      {
         UnitTestConstants.PrintUnitTestHeader(false);


         // GetVolumeDeviceName

         var inputPath =  UnitTestConstants.SysDrive;

         Console.WriteLine("\nInput Logical Drive Path: [{0}]", inputPath);
         

         var deviceNameFromLogicalDrive = Alphaleonis.Win32.Filesystem.Volume.GetVolumeDeviceName(inputPath);

         Console.WriteLine("\n\tDevice Name: [{0}]", deviceNameFromLogicalDrive);
         
         Assert.IsNotNull(deviceNameFromLogicalDrive);




         // GetVolumeGuid

         inputPath = Alphaleonis.Win32.Filesystem.Volume.GetVolumeGuid(inputPath);

         Console.WriteLine("\nInput Volume GUID Path: [{0}]", inputPath);


         var guid = Alphaleonis.Win32.Filesystem.Volume.GetVolumeGuid(inputPath);

         Assert.IsNotNull(guid);


         var deviceNameFromGuid = Alphaleonis.Win32.Filesystem.Volume.GetVolumeDeviceName(guid);

         Console.WriteLine("\n\tDevice Name: [{0}]", deviceNameFromGuid);

         Assert.IsNotNull(deviceNameFromGuid);



         
         Assert.IsNotNull(deviceNameFromLogicalDrive);

         
         var deviceNamePrefix = Alphaleonis.Win32.Filesystem.Path.DevicePrefix + "HarddiskVolume";

         Assert.IsTrue(deviceNameFromLogicalDrive.StartsWith(deviceNamePrefix));

         Assert.IsTrue(deviceNameFromGuid.StartsWith(deviceNamePrefix));


         Assert.AreEqual(deviceNameFromLogicalDrive, deviceNameFromGuid);
      }
   }
}
