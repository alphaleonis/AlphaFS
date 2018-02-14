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
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AlphaFS.UnitTest
{
   public partial class AlphaFS_PhysicalDriveInfoTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void AlphaFS_Device_GetPhysicalDriveInfo_FromVolume_Success()
      {
         UnitTestConstants.PrintUnitTestHeader(false);


         var volumeCount = 0;

         // Use lowercase drive letter because .Contains() is case sensitive by default.
         var sourceDrive = UnitTestConstants.SysDrive.ToLowerInvariant();

         // Use uppercase volume guid because .Contains() is case sensitive by default.
         var sourceVolume = Alphaleonis.Win32.Filesystem.Volume.GetVolumeGuid(sourceDrive).ToUpperInvariant();


         //var pDrive = Alphaleonis.Win32.Filesystem.Device.GetPhysicalDriveInfo(@"t:\");

         // \\?\Volume{db5044f9-bd1f-4243-ab97-4b985eb29e80}\,
         // \\?\Volume{43dd0652-8ecf-4943-8275-016fc09f02c7}\,
         // \\?\Volume{e32f9cf5-7978-4aaa-9525-86cf401487ad}\,
         // \\?\Volume{50685374-f895-11e7-a43e-f49634afb3a5}\

         var pDrive = Alphaleonis.Win32.Filesystem.Device.GetPhysicalDriveInfo(sourceVolume);


         Console.WriteLine();
         Console.WriteLine("#{0:000}\tVolume: [{1}]", ++volumeCount, sourceVolume);


         UnitTestConstants.Dump(pDrive, -17);

         UnitTestConstants.Dump(pDrive.StorageDeviceInfo, -15, true);


         Assert.IsNotNull(pDrive);


         Assert.IsNotNull(pDrive.VolumeGuids);
         //Assert.IsTrue(pDrive.VolumeGuids.Contains(sourceVolume));
         Assert.IsTrue(pDrive.ContainsVolume(sourceVolume));
      }
   }
}
