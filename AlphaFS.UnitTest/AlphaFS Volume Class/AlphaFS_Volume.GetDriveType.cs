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
      public void AlphaFS_Volume_GetDriveType_LocalAndNetwork_Success()
      {
         AlphaFS_Volume_GetDriveType(false);
         AlphaFS_Volume_GetDriveType(true);
      }


      private void AlphaFS_Volume_GetDriveType(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);


         var logicalDriveCount = 0;

         foreach (var logicalDrive in System.IO.DriveInfo.GetDrives())
         {
            var driveName = isNetwork ? Alphaleonis.Win32.Filesystem.Path.LocalToUnc(logicalDrive.Name) : logicalDrive.Name;

            Console.Write("#{0:000}\tInput Logical Drive Path: [{1}]", ++logicalDriveCount, driveName);

            if (logicalDrive.DriveType == System.IO.DriveType.CDRom)
            {
               Console.WriteLine();
               continue;
            }


            var driveType = Alphaleonis.Win32.Filesystem.Volume.GetDriveType(driveName);

            Console.WriteLine("\t\tDrive Type: [{0}]", driveType);


            if (isNetwork)
            {
               // Some USB drives do not report drive type.
            }

            else
               Assert.AreEqual(logicalDrive.DriveType, driveType);

         }


         Assert.IsTrue(logicalDriveCount > 0, "No logical drives enumerated, but it is expected.");


         Console.WriteLine();
      }
   }
}
