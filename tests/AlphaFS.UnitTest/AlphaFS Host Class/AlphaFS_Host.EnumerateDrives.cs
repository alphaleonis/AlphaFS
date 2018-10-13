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
using System.Linq;

namespace AlphaFS.UnitTest
{
   public partial class EnumerationTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void AlphaFS_Host_EnumerateDrives_Network_Success()
      {
         UnitTestAssert.IsElevatedProcess();
         UnitTestConstants.PrintUnitTestHeader(true);
         

         var host = Environment.MachineName;

         var drives = Alphaleonis.Win32.Network.Host.EnumerateDrives(host).ToArray();

         foreach (var driveInfo in drives)
         {
            Console.WriteLine("Host Local Drive: [{0}]", driveInfo.Name);
            
            UnitTestConstants.Dump(driveInfo);

            UnitTestConstants.Dump(driveInfo.DiskSpaceInfo, true);

            UnitTestConstants.Dump(driveInfo.VolumeInfo, true);
            
            Assert.IsNull(driveInfo.DosDeviceName);

            Assert.IsNull(driveInfo.VolumeInfo.Guid);

            Console.WriteLine();
         }


         Assert.IsTrue(drives.Length > 0, "Nothing is enumerated, but it is expected.");


         // \\localhost\C$

         host = Alphaleonis.Win32.Network.Host.GetUncName() + Alphaleonis.Win32.Filesystem.Path.DirectorySeparator +
                UnitTestConstants.SysDrive[0] + Alphaleonis.Win32.Filesystem.Path.NetworkDriveSeparator +
                Alphaleonis.Win32.Filesystem.Path.DirectorySeparator;

         Assert.AreEqual(drives[0].Name, host);
      }
   }
}
