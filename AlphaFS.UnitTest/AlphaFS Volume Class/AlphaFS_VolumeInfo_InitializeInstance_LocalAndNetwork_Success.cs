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
   public partial class VolumeTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void AlphaFS_VolumeInfo_InitializeInstance_LocalAndNetwork_Success()
      {
         if (!UnitTestConstants.IsAdmin())
            Assert.Inconclusive();

         VolumeInfo_InitializeInstance(false);
         VolumeInfo_InitializeInstance(true);
      }


      private void VolumeInfo_InitializeInstance(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         var tempPath = UnitTestConstants.SysDrive;
         if (isNetwork)
            tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);


         var volInfo = Alphaleonis.Win32.Filesystem.Volume.GetVolumeInfo(tempPath);
         UnitTestConstants.Dump(volInfo, -26);


         if (isNetwork)
         {
            var driveInfo = new Alphaleonis.Win32.Filesystem.DriveInfo(tempPath);

            Assert.AreEqual(driveInfo.VolumeLabel, volInfo.Name);
            Assert.AreEqual(driveInfo.DriveFormat, volInfo.FileSystemName);
            Assert.AreEqual(driveInfo.Name, volInfo.FullPath);

            Assert.IsNull(volInfo.Guid);
         }

         else
         {
            // System.IO.DriveInfo does not support UNC paths.

            var driveInfo = new System.IO.DriveInfo(tempPath);

            Assert.AreEqual(driveInfo.VolumeLabel, volInfo.Name);
            Assert.AreEqual(driveInfo.DriveFormat, volInfo.FileSystemName);
            Assert.AreEqual(driveInfo.Name, volInfo.FullPath);

            Assert.IsNotNull(volInfo.Guid);
         }

         Console.WriteLine();
      }
   }
}
