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
   public partial class AlphaFS_VolumeTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void AlphaFS_Volume_GetVolumeInfo_UsingLogicalDrivePath_LocalAndNetwork_Success()
      {
         AlphaFS_Volume_GetVolumeInfo_UsingLogicalDrivePath(false);
         AlphaFS_Volume_GetVolumeInfo_UsingLogicalDrivePath(true);
      }


      private void AlphaFS_Volume_GetVolumeInfo_UsingLogicalDrivePath(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);


         var driveCount = 0;

         foreach (var driveInfo in System.IO.DriveInfo.GetDrives())
         {
            var driveName = isNetwork ? Alphaleonis.Win32.Filesystem.Path.LocalToUnc(driveInfo.Name) : driveInfo.Name;

            Console.WriteLine("#{0:000}\tInput Logical Drive Path: [{1}]", ++driveCount, driveName);


            //// Skip mapped drives and CDRom drives.

            //if (logicalDrive.DriveType == System.IO.DriveType.NoRootDirectory || logicalDrive.DriveType == System.IO.DriveType.CDRom)
            //{
            //   Console.WriteLine();
            //   continue;
            //}
            

            if (isNetwork)
            {
               var driveInfo2 = new Alphaleonis.Win32.Filesystem.DriveInfo(driveName);


               // GetVolumeInfo fails when DriveType is not of type Network.

               if (driveInfo2.DriveType != System.IO.DriveType.Network)
                  continue;


               var volInfo = Alphaleonis.Win32.Filesystem.Volume.GetVolumeInfo(driveName);

               UnitTestConstants.Dump(volInfo);


               Assert.AreEqual(driveInfo2.VolumeLabel, volInfo.Name);
               Assert.AreEqual(driveInfo2.DriveFormat, volInfo.FileSystemName);

               if (driveInfo.DriveType != System.IO.DriveType.Network)
                  Assert.AreEqual(driveInfo2.Name, volInfo.FullPath);

               Assert.IsNull(volInfo.Guid);
            }

            else
            {
               var volInfo = Alphaleonis.Win32.Filesystem.Volume.GetVolumeInfo(driveName, true);

               UnitTestConstants.Dump(volInfo);


               // System.IO.DriveInfo does not support UNC paths.

               var driveInfo2 = new System.IO.DriveInfo(driveName);

               
               // Prevent System.IO.IOException: (1005) The volume does not contain a recognized file system.

               if (null != volInfo && null != volInfo.FileSystemName)
               {
                  Assert.AreEqual(driveInfo2.VolumeLabel, volInfo.Name);
                  Assert.AreEqual(driveInfo2.DriveFormat, volInfo.FileSystemName);
                  Assert.AreEqual(driveInfo2.Name, volInfo.FullPath);

                  if (driveInfo.DriveType != System.IO.DriveType.Network)
                  {
                     Assert.IsNotNull(volInfo.Guid);

                     Assert.IsTrue(volInfo.Guid.StartsWith(Alphaleonis.Win32.Filesystem.Path.VolumePrefix));
                  }
               }
            }


            Console.WriteLine();
         }


         Assert.IsTrue(driveCount > 0, "No logical drives enumerated, but it is expected.");


         Console.WriteLine();
      }
   }
}
