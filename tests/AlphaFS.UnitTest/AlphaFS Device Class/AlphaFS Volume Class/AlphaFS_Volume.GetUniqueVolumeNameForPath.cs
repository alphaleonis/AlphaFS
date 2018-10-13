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
using System.Linq;

namespace AlphaFS.UnitTest
{
   public partial class AlphaFS_VolumeTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void AlphaFS_Volume_GetUniqueVolumeNameForPath_Local_Success()
      {
         UnitTestConstants.PrintUnitTestHeader(false);


         var logicalDriveCount = 0;

         foreach (var driveInfo in System.IO.DriveInfo.GetDrives())
         {
            // Skip mapped drives and network drives.

            if (driveInfo.DriveType == System.IO.DriveType.NoRootDirectory || driveInfo.DriveType == System.IO.DriveType.Network)
               continue;


            var driveName = driveInfo.Name;

            var deviceGuid = Alphaleonis.Win32.Filesystem.Volume.GetVolumeGuid(driveName);

            Console.WriteLine("#{0:000}\tInput Path: [{1}]", ++logicalDriveCount, driveName);


            var volumeNameResult = Alphaleonis.Win32.Filesystem.Volume.GetUniqueVolumeNameForPath(driveName);

            Console.WriteLine("\n\tGetUniqueVolumeNameForPath: [{0}]", volumeNameResult ?? "null");


            // Typically, only one mount point exists so the Volume GUIDs will match.

            Assert.AreEqual(deviceGuid, volumeNameResult);


            var pathNames = Alphaleonis.Win32.Filesystem.Volume.EnumerateVolumePathNames(volumeNameResult).ToArray();

            foreach (var uniqueName in pathNames)
            {
               Console.WriteLine("\n\tUnique name: [{0}]", uniqueName);


               try
               {
                  var targetInfo = Alphaleonis.Win32.Filesystem.Directory.GetLinkTargetInfo(uniqueName);

                  UnitTestConstants.Dump(targetInfo);

                  Assert.AreEqual(deviceGuid, Alphaleonis.Win32.Filesystem.Path.LongPathPrefix + targetInfo.SubstituteName.Replace(Alphaleonis.Win32.Filesystem.Path.NonInterpretedPathPrefix, string.Empty));
               }
               catch
               {
                  if (!string.IsNullOrWhiteSpace(uniqueName))
                     Assert.AreEqual(driveName, uniqueName);
               }
            }


            Console.WriteLine();
         }


         if (logicalDriveCount == 0)
            UnitTestAssert.InconclusiveBecauseResourcesAreUnavailable();
      }
   }
}
