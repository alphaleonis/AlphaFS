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

using Alphaleonis.Win32.Filesystem;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Directory = Alphaleonis.Win32.Filesystem.Directory;
using DriveInfo = Alphaleonis.Win32.Filesystem.DriveInfo;
using Path = Alphaleonis.Win32.Filesystem.Path;

namespace AlphaFS.UnitTest
{
   /// <summary>This is a test class for Shell32 and is intended to contain all Volume and VolumeInfo Unit Tests.</summary>
   [TestClass]
   public partial class AlphaFS_VolumeTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      #region Unit Tests

      private readonly string TempFolder = Path.GetTempPath();

      private void DumpGetDriveFormat(bool isLocal)
      {
         Console.WriteLine("=== TEST {0} ===\n", isLocal ? "LOCAL" : "NETWORK");

         var cnt = 0;

         // Get Logical Drives from Environment.
         foreach (var drv in Directory.GetLogicalDrives(false, true))
         {
            UnitTestConstants.StopWatcher(true);

            try
            {
               // GetDriveType() can read an empty cdrom drive.
               // SetCurrentDirectory() wil fail on an empty cdrom drive.
               System.IO.Directory.SetCurrentDirectory(drv);
            }
            catch (Exception ex)
            {
               Console.WriteLine("\nCaught (UNEXPECTED) {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
            }

            var drive = isLocal ? drv : Path.LocalToUnc(drv);

            UnitTestConstants.StopWatcher(true);
            var driveInfo = new DriveInfo(drive);

            Console.WriteLine("#{0:000}\tInput Path   : [{1}]", ++cnt, drive);
            Console.WriteLine("\tGetDriveFormat(): [{0}]", driveInfo.DriveFormat);
            Console.WriteLine("\tGetDriveType()  : [{0}]", driveInfo.DriveType);
            Console.WriteLine("\tIsReady()       : [{0}]", driveInfo.IsReady);
            Console.WriteLine("\tIsVolume()      : [{0}]\n", driveInfo.IsVolume);

            if (isLocal)
               Assert.AreEqual(new System.IO.DriveInfo(drive).IsReady, driveInfo.IsReady, "IsReady AlphaFS != System.IO");
         }
         Console.WriteLine("{0}\n\n", UnitTestConstants.Reporter(true));

         if (cnt == 0)
            Assert.Inconclusive("Nothing is enumerated, but it is expected.");
      }

      private void DumpGetVolumePathName(bool isLocal)
      {
         Console.WriteLine("\n=== TEST {0} ===\n", isLocal ? "LOCAL" : "NETWORK");
         var tempPath = UnitTestConstants.TempFolder;
         Console.WriteLine("Input Path: [{0}]", tempPath);

         UnitTestConstants.StopWatcher(true);
         var volumePathName = Volume.GetVolumePathName(UnitTestConstants.TempFolder);
         var report = UnitTestConstants.Reporter(true);
         Console.WriteLine("\n\tGetVolumePathName(): [{0}]", volumePathName);

         Console.WriteLine("\n\t{0}\n", report);

         if (isLocal)
            Assert.IsTrue(tempPath.StartsWith(volumePathName));
         Assert.IsTrue(volumePathName.EndsWith(Path.DirectorySeparator));
      }

      #endregion // Unit Tests




      [TestMethod]
      public void AlphaFS_Volume_GetDriveFormat()
      {
         Console.WriteLine("Volume.GetDriveFormat()\n");

         DumpGetDriveFormat(true);
         DumpGetDriveFormat(false);
      }


      [TestMethod]
      public void AlphaFS_Volume_GetVolumePathName()
      {
         Console.WriteLine("Volume.GetVolumePathName()");

         DumpGetVolumePathName(true);
      }
   }
}
