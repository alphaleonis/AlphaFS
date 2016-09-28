/*  Copyright (C) 2008-2015 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
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
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Directory = Alphaleonis.Win32.Filesystem.Directory;
using DriveInfo = Alphaleonis.Win32.Filesystem.DriveInfo;
using File = Alphaleonis.Win32.Filesystem.File;
using Path = Alphaleonis.Win32.Filesystem.Path;

namespace AlphaFS.UnitTest
{
   /// <summary>This is a test class for Volume and is intended to contain all Volume class Unit Tests.</summary>
   [TestClass]
   public partial class VolumeTest
   {
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
               Console.WriteLine("\nCaught (unexpected) {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
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
            Assert.Inconclusive("Nothing was enumerated, but it was expected.");
      }
      
      private void DumpGetVolumePathName(bool isLocal)
      {
         Console.WriteLine("\n=== TEST {0} ===\n", isLocal ? "LOCAL" : "NETWORK");
         var tempPath = UnitTestConstants.LocalHostShare;
         Console.WriteLine("Input Path: [{0}]", tempPath);

         UnitTestConstants.StopWatcher(true);
         var volumePathName = Volume.GetVolumePathName(UnitTestConstants.LocalHostShare);
         var report = UnitTestConstants.Reporter(true);
         Console.WriteLine("\n\tGetVolumePathName(): [{0}]", volumePathName);

         Console.WriteLine("\n\t{0}\n", report);

         if (isLocal)
            Assert.IsTrue(tempPath.StartsWith(volumePathName));
         Assert.IsTrue(volumePathName.EndsWith(Path.DirectorySeparator));
      }

      private void DumpGetDriveNameForNtDeviceName(bool isLocal)
      {
         Console.WriteLine("\n=== TEST {0} ===\n", isLocal ? "LOCAL" : "NETWORK");
         var cnt = 0;

         // Get Logical Drives from UnitTestConstants.Local Host.
         foreach (var drive in Directory.GetLogicalDrives())
         {
            var tempPath = isLocal ? drive : Path.LocalToUnc(drive);

            foreach (var dosDev in Volume.QueryDosDevice(tempPath))
            {
               Console.WriteLine("#{0:000}\tInput Path                 : [{1}]", ++cnt, dosDev);

               var result = Volume.GetDriveNameForNtDeviceName(dosDev);
               Console.WriteLine("\tGetDriveNameForNtDeviceName() : [{0}]", result ?? "null");
               Assert.AreEqual(drive, result);

               result = Volume.GetVolumeGuidForNtDeviceName(dosDev);
               Console.WriteLine("\tGetVolumeGuidForNtDeviceName(): [{0}]\n", result ?? "null");
            }
         }
         Console.WriteLine("\t{0}\n", UnitTestConstants.Reporter(true));

         if (isLocal && cnt == 0)
            Assert.Inconclusive("Nothing was enumerated, but it was expected.");
      }

      private void DumpGetUniqueVolumeNameForPath(bool isLocal)
      {
         Console.WriteLine("\n=== TEST {0} ===\n", isLocal ? "LOCAL" : "NETWORK");

         var cnt = 0;
         var testedSystemDrive = false;
         UnitTestConstants.StopWatcher(true);

         // Get Logical Drives from UnitTestConstants.Local Host.
         foreach (var drive in Directory.GetLogicalDrives())
         {
            var tempPath = isLocal ? drive : Path.LocalToUnc(drive);

            Console.WriteLine("#{0:000}\tInput Path               : [{1}]", ++cnt, tempPath);

            var result = Volume.GetUniqueVolumeNameForPath(tempPath);
            Console.WriteLine("\tGetUniqueVolumeNameForPath(): [{0}]", result ?? "null");

            var deviceName = Volume.GetVolumeDeviceName(tempPath);
            Console.WriteLine("\tGetVolumeDeviceName()       : [{0}]", deviceName ?? "null");

            result = null;
            try { result = Volume.GetVolumeGuid(tempPath); }
            catch {}
            
            Console.WriteLine("\tGetVolumeGuid()             : [{0}]\n", result ?? "null");


            // At least the system drive should contain valid data.
            if (isLocal && drive.TrimEnd('\\').Equals(UnitTestConstants.SysDrive))
            {
               Assert.IsFalse(deviceName != null && deviceName.StartsWith(@"\\Device\", StringComparison.OrdinalIgnoreCase));
               testedSystemDrive = true;
            }
         }
         Console.WriteLine("\t{0}\n", UnitTestConstants.Reporter(true));

         if (isLocal)
         {
            if (cnt == 0)
               Assert.Inconclusive("Nothing was enumerated, but it was expected.");

            Assert.IsTrue(testedSystemDrive);
         }
      }

      #endregion // Unit Tests

      #region Unit Test Callers

      #region DosDevice

      #region DefineDosDevice

      [TestMethod]
      public void AlphaFS_Volume_DefineDosDevice()
      {
         Console.WriteLine("Volume.DefineDosDevice()");

          if (!UnitTestConstants.IsAdmin())
              Assert.Inconclusive();

         #region Regular Drive Mapping

         var drive = string.Format(CultureInfo.CurrentCulture, "{0}{1}{2}", DriveInfo.GetFreeDriveLetter(), Path.VolumeSeparatorChar, Path.DirectorySeparatorChar);
         UnitTestConstants.StopWatcher(true);

         // Create Regular drive mapping.
         var actionIsTrue = false;
         try
         {
            Volume.DefineDosDevice(drive, TempFolder);
            actionIsTrue = true;
         }
         catch
         {
         }

         Console.WriteLine("\nCreated Regular drive mapping (Should be True): [{0}]\nDrive letter: [{1}] now points to: [{2}]\n\t{3}", actionIsTrue, drive, TempFolder, UnitTestConstants.Reporter(true));
         Assert.IsTrue(actionIsTrue, "Regular drive mapping should have been created.");

         var di = new DriveInfo(drive);
         var diSysIo = new System.IO.DriveInfo(drive);

         try
         {
            Assert.IsTrue(UnitTestConstants.Dump(di, -21));

            // A Regular drive mapping that should be visible immediately in Explorer.
            // Seems to be invisible in Explorer on Windows 8, but visibile in an elevated cmd shell.
            //Assert.IsTrue(Directory.Exists(_driveLetter), "Drive letter not visible.");

            Console.WriteLine("\nDrive Letter: [{0}]\tGetVolumeDevice(): [{1}]", drive, Volume.GetVolumeDeviceName(drive));
            Assert.AreEqual(diSysIo.IsReady, di.IsReady);
            
         }
         finally 
         {
            UnitTestConstants.StopWatcher(true);
            
            // Remove Regular drive mapping.
            actionIsTrue = false;
            try
            {
               Volume.DeleteDosDevice(drive);
               actionIsTrue = true;
            }
            catch
            {
            }

            Console.WriteLine("\n\nRemoved Regular drive mapping (Should be True): [{0}]\nDrive letter: [{1}] has been set free.\n\t{2}\n", actionIsTrue, drive, UnitTestConstants.Reporter(true));
            Assert.IsTrue(actionIsTrue, "Regular drive mapping should have been removed.");
            Assert.IsFalse(Directory.Exists(drive), "Drive letter should not be visible.");
         }
         
         #endregion // Regular Drive Mapping

         #region Symbolic Link Drive Mapping

         try
         {
            drive = string.Format(CultureInfo.CurrentCulture, "{0}{1}{2}", DriveInfo.GetFreeDriveLetter(true), Path.VolumeSeparatorChar, Path.DirectorySeparatorChar);
            UnitTestConstants.StopWatcher(true);

            // Create Symbolic Link.
            var createSymbolicLink = false;
            try
            {
               Volume.DefineDosDevice(drive, TempFolder, DosDeviceAttributes.RawTargetPath);
               createSymbolicLink = true;
            }
            catch
            {
            }

            Console.WriteLine("\n\nCreated Symbolic link mapping (Should be True): [{0}]\nDrive letter: [{1}] now points to: [{2}]\n\t{3}", createSymbolicLink, drive, TempFolder, UnitTestConstants.Reporter(true));
            Assert.IsTrue(createSymbolicLink);

            di = new DriveInfo(drive);
            Assert.IsTrue(UnitTestConstants.Dump(di, -21));

            // The link is created in the NT Device Name namespace and thus not visibile in Explorer.

            // Remove Symbolic Link, no exact match: fail.
            UnitTestConstants.StopWatcher(true);

            var removeSymbolicLink = false;
            try
            {
               Volume.DeleteDosDevice(drive, "NonExistingFolder", true);
               removeSymbolicLink = true;
            }
            catch
            {
            }

            Console.WriteLine("\n\nRemoved Symbolic link mapping (Should be False): [{0}]\nDrive letter: [{1}] has NOT been set free.\tNo exactMatch MS-DOS device name found.\n\t{2}", removeSymbolicLink, drive, UnitTestConstants.Reporter(true));
            Assert.IsFalse(removeSymbolicLink);
         }
         finally
         {
            UnitTestConstants.StopWatcher(true);

            // Remove Symbolic Link, exact match: success.
            var removeSymbolicLink = false;
            try
            {
               Volume.DeleteDosDevice(drive, TempFolder, true);
               removeSymbolicLink = true;
            }
            catch
            {
            }

            Console.WriteLine("\n\nRemoved Symbolic link mapping (Should be True): [{0}]\nDrive letter: [{1}] has been set free.\tFound exactMatch MS-DOS device name.\n\t{2}", removeSymbolicLink, drive, UnitTestConstants.Reporter(true));
            Assert.IsTrue(removeSymbolicLink);
            Assert.IsFalse(Directory.Exists(drive));
         }

         #endregion // Symbolic Link Drive Mapping
      }

      #endregion // DefineDosDevice

      #region QueryAllDosDevices

      [TestMethod]
      public void AlphaFS_Volume_QueryAllDosDevices()
      {
         Console.WriteLine("Volume.QueryAllDosDevices()");

         UnitTestConstants.StopWatcher(true);

         IEnumerable<string> query = Volume.QueryAllDosDevices("sort").ToArray();
         var report = UnitTestConstants.Reporter(true);

         Console.WriteLine("\nRetrieved: [{0}] items.{1}\n", query.Count(), report);

         var cnt = 0;
         foreach (var dosDev in query)
         {
            Console.WriteLine("#{0:000}\t{1}", ++cnt, dosDev);
         }

         if (cnt == 0)
            Assert.Inconclusive("Nothing was enumerated, but it was expected.");

         Assert.IsTrue(query.Any());
      }

      #endregion // QueryAllDosDevices

      #region QueryDosDevice

      [TestMethod]
      public void AlphaFS_Volume_QueryDosDevice()
      {
         Console.WriteLine("Volume.QueryDosDevice()");

         #region Filtered, UnSorted List Drives

         var filter = "hard";

         UnitTestConstants.StopWatcher(true);

         IEnumerable<string> query = Volume.QueryDosDevice(filter).ToArray();

         Console.WriteLine("\n\tQueryDosDevice(\"" + filter + "\")\n\tRetrieved Filtered, UnSorted list: [{0}]\n\tList .Count(): [{1}]\n{2}\n", query.Any(), query.Count(), UnitTestConstants.Reporter());

         var cnt = 0;

         foreach (var dosDev in query)
            Console.WriteLine("\t\t#{0:000}\tMS-Dos Name: [{1}]", ++cnt, dosDev);

         Assert.IsTrue(query.Any() && cnt > 0);

         #endregion // Filtered, UnSorted List Drives

         #region Filtered, Sorted List Volumes

         filter = "volume";

         cnt = 0;

         UnitTestConstants.StopWatcher(true);

         query = Volume.QueryDosDevice(filter, "sort").ToArray();

         Console.WriteLine("\n\n\tQueryDosDevice(\"" + filter + "\")\n\tRetrieved Filtered, Sorted list: [{0}]\n\tList .Count(): [{1}]\n", query.Any(), query.Count(), UnitTestConstants.Reporter());

         foreach (var dosDev in query)
            Console.WriteLine("\t\t#{0:000}\tMS-Dos Name: [{1}]", ++cnt, dosDev);

         Assert.IsTrue(query.Any() && cnt > 0);

         #endregion // Filtered, Sorted List Volumes

         #region Get from Logical Drives

         Console.WriteLine("\n\n\tQueryDosDevice (from Logical Drives)\n");

         cnt = 0;

         foreach (var existingDriveLetter in Directory.GetLogicalDrives())
         {
            foreach (var dosDev in Volume.QueryDosDevice(existingDriveLetter))
            {
               var hasLogicalDrive = !string.IsNullOrWhiteSpace(dosDev);

               Console.WriteLine("\t\t#{0:000}\tLogical Drive [{1}] MS-Dos Name: [{2}]", ++cnt, existingDriveLetter, dosDev);

               Assert.IsTrue(hasLogicalDrive);
            }
         }

         Assert.IsTrue(cnt > 0, "No entries read.");

         #endregion // Get from Logical Drives

         #region Filtered, Sorted PhysicalDrive

         filter = "PhysicalDrive";

         cnt = 0;

         UnitTestConstants.StopWatcher(true);

         query = Volume.QueryDosDevice(filter, "sort").ToArray();

         Console.WriteLine("\n\n\tQueryDosDevice(\"" + filter + "\")\n\tRetrieved Filtered, Sorted list: [{0}]\n\tList .Count(): [{1}]\n", query.Any(), query.Count(), UnitTestConstants.Reporter());

         foreach (var dosDev in query)
            Console.WriteLine("\t\t#{0:000}\tMS-Dos Name: [{1}]", ++cnt, dosDev);

         Assert.IsTrue(query.Any() && cnt > 0);

         #endregion // Filtered, Sorted PhysicalDrive

         #region Filtered, Sorted CDRom

         filter = "CdRom";

         cnt = 0;

         UnitTestConstants.StopWatcher(true);

         query = Volume.QueryDosDevice(filter, "sort").ToArray();

         Console.WriteLine("\n\n\tQueryDosDevice(\"" + filter + "\")\n\tRetrieved Filtered, Sorted list: [{0}]\n\tList .Count(): [{1}]\n", query.Any(), query.Count(), UnitTestConstants.Reporter());

         foreach (var dosDev in query)
            Console.WriteLine("\t\t#{0:000}\tMS-Dos Name: [{1}]", ++cnt, dosDev);

         //Assert.IsTrue(query.Any() && cnt > 0);

         #endregion // Filtered, Sorted CDRom
      }

      #endregion // QueryDosDevice

      #endregion // DosDevice

      #region Drive

      #region GetDriveFormat

      [TestMethod]
      public void AlphaFS_Volume_GetDriveFormat()
      {
         Console.WriteLine("Volume.GetDriveFormat()\n");
         
         DumpGetDriveFormat(true);
         DumpGetDriveFormat(false);
      }

      #endregion // GetDriveFormat

      #region GetDriveNameForNtDeviceName

      [TestMethod]
      public void AlphaFS_Volume_GetDriveNameForNtDeviceName()
      {
         Console.WriteLine("Volume.GetDriveNameForNtDeviceName()");

         DumpGetDriveNameForNtDeviceName(true);
         DumpGetDriveNameForNtDeviceName(false);
      }

      #endregion // GetDriveNameForNtDeviceName

      #endregion // Drive

      #region Volume
      
      #region EnumerateVolumeMountPoints

      [TestMethod]
      public void AlphaFS_Volume_EnumerateVolumeMountPoints()
      {
         Console.WriteLine("Volume.EnumerateVolumeMountPoints()");

          if (!UnitTestConstants.IsAdmin())
              Assert.Inconclusive();

         #region Logical Drives

         var cnt = 0;
         Console.WriteLine("\nLogical Drives\n");

         // Get Logical Drives from UnitTestConstants.Local Host, .IsReady Drives only.
         foreach (var drive in Directory.GetLogicalDrives(false, true))
         {
            try
            {
               // Logical Drives --> Volumes --> Volume Mount Points.
               var uniqueVolName = Volume.GetUniqueVolumeNameForPath(drive);

               if (!string.IsNullOrWhiteSpace(uniqueVolName) && !uniqueVolName.Equals(drive, StringComparison.OrdinalIgnoreCase))
               {
                  foreach (var mountPoint in Volume.EnumerateVolumeMountPoints(uniqueVolName).Where(mp => !string.IsNullOrWhiteSpace(mp)))
                  {
                     UnitTestConstants.StopWatcher(true);

                     string guid = null;
                     try { guid = Volume.GetVolumeGuid(Path.Combine(drive, mountPoint)); }
                     catch (Exception ex)
                     {
                        Console.WriteLine("\n\tCaught (unexpected #1) {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
                     }

                     Console.WriteLine("\t#{0:000}\tLogical Drive: [{1}]\tGUID: [{2}]\n\t\tDestination  : [{3}]\n\t{4}", ++cnt, drive, guid ?? "null", mountPoint, UnitTestConstants.Reporter(true));
                  }
               }
            }
            catch (Exception ex)
            {
               Console.WriteLine("\n\tCaught (unexpected #2) {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
            }
         }


         if (cnt == 0)
            Assert.Inconclusive("No Volume Mount Points found.");

         #endregion // Logical Drives
      }

      #endregion // EnumerateVolumeMountPoints

      #region EnumerateVolumes

      [TestMethod]
      public void AlphaFS_Volume_EnumerateVolumes()
      {
         Console.WriteLine("Volume.EnumerateVolumes()");

         Console.WriteLine("\nShould give the same enumeration as \"mountvol.exe\"\n");

         var cnt = 0;
         UnitTestConstants.StopWatcher(true);
         foreach (var volume in Volume.EnumerateVolumes())
         {
            Console.WriteLine("#{0:000}\tVolume: [{1}]\n", ++cnt, volume);
            Console.WriteLine("\tVolume() class methods that accept a volume guid as input argument:\n");

            Console.WriteLine("\t\tIsReady()                 : [{0}]", Volume.IsReady(volume));
            Console.WriteLine("\t\tIsVolume()                : [{0}]", Volume.IsVolume(volume));
            Console.WriteLine("\t\tGetDiskFreeSpace()        : [{0}]", Volume.GetDiskFreeSpace(volume).AvailableFreeSpaceUnitSize);

            var result = Volume.GetDriveFormat(volume);
            Console.WriteLine("\t\tGetDriveFormat()          : [{0}]", result ?? "null");

            Console.WriteLine("\t\tGetDriveType()            : [{0}]", Volume.GetDriveType(volume));
            Console.WriteLine("\t\tGetVolumeLabel()          : [{0}]", Volume.GetVolumeLabel(volume));
            Console.WriteLine("\t\tGetVolumeDisplayName()    : [{0}]", Volume.GetVolumeDisplayName(volume));
            

            foreach (var displayName in Volume.EnumerateVolumePathNames(volume))
            {
               Console.WriteLine("\t\tEnumerateVolumePathNames(): [{0}]\n", displayName);
               Assert.IsTrue(!string.IsNullOrWhiteSpace(displayName));
            }
         }
         Console.WriteLine("\t{0}\n", UnitTestConstants.Reporter(true));

         if (cnt == 0)
            Assert.Inconclusive("Nothing was enumerated, but it was expected.");
      }

      #endregion // EnumerateVolumes
      
      #region GetUniqueVolumeNameForPath

      [TestMethod]
      public void AlphaFS_Volume_GetUniqueVolumeNameForPath()
      {
         Console.WriteLine("Volume.GetUniqueVolumeNameForPath()");

         DumpGetUniqueVolumeNameForPath(true);
         DumpGetUniqueVolumeNameForPath(false);
      }

      #endregion // GetUniqueVolumeNameForPath

      #region GetVolumeLabel

      [TestMethod]
      public void AlphaFS_Volume_GetVolumeLabel()
      {
         Console.WriteLine("Volume.GetVolumeLabel()");

         int cnt;
         bool looped;
         string label;

         #region Logical Drives

         Console.WriteLine("\nLogical Drives\n");

         cnt = 0;
         looped = false;

         UnitTestConstants.StopWatcher(true);
         foreach (var drive in Directory.GetLogicalDrives())
         {
            label = Volume.GetVolumeLabel(drive);
            Console.WriteLine("\t#{0:000}\tLogical Drive: [{1}]\t\tLabel: [{2}]", ++cnt, drive, label);
            looped = true;
         }
         Console.WriteLine("\n\t{0}\n", UnitTestConstants.Reporter(true));
         Assert.IsTrue(looped && cnt > 0);

         #endregion // Logical Drives

         #region Volumes

         Console.WriteLine("\nVolumes\n");

         cnt = 0;
         looped = false;

         UnitTestConstants.StopWatcher(true);
         foreach (var volume in Volume.EnumerateVolumes())
         {
            label = Volume.GetVolumeLabel(volume);
            Console.WriteLine("\t#{0:000}\tVolume: [{1}]\t\tLabel: [{2}]", ++cnt, volume, label);
            looped = true;
         }
         Console.WriteLine("\n\t{0}\n", UnitTestConstants.Reporter(true));
         Assert.IsTrue(looped && cnt > 0);

         #endregion // Volumes

         #region DosDevices

         Console.WriteLine("\nDosDevices\n");

         cnt = 0;
         looped = false;
         var devices = new List<string>(new[] { "volume", "hard", "physical", "storage" });

         foreach (var dd in devices)
         {
            cnt = 0;

            UnitTestConstants.StopWatcher(true);
            foreach (var dosDevice in Volume.QueryDosDevice(dd))
            {
               label = Volume.GetVolumeLabel(dosDevice);
               Console.WriteLine("\t#{0:000}\tDosDevice: [{1}]\t\tLabel: [{2}]", ++cnt, dosDevice, label);

               looped = true;
            }
            Console.WriteLine("\n\t{0}\n", UnitTestConstants.Reporter(true));
         }

         Assert.IsTrue(looped && cnt > 0);

         #endregion // DosDevices
      }

      #endregion // GetVolumeLabel
      
      #region GetVolumePathName

      [TestMethod]
      public void AlphaFS_Volume_GetVolumePathName()
      {
         Console.WriteLine("Volume.GetVolumePathName()");

         DumpGetVolumePathName(true);
      }

      #endregion // GetVolumePathName
      
      #region IsSameVolume

      [TestMethod]
      public void AlphaFS_Volume_IsSameVolume()
      {
         Console.WriteLine("Volume.IsSameVolume()");

         var file1 = Path.GetTempFileName();
         var file2 = Path.GetTempFileName();
         var fileTmp = file2;

         // Same C:
         UnitTestConstants.StopWatcher(true);
         var isSame = Volume.IsSameVolume(file1, fileTmp);
         Console.WriteLine("\nOn same Volume (Should be True): [{0}]\n\tFile1: [{1}]\n\tFile2: [{2}]\n\t{3}", isSame, file1, fileTmp, UnitTestConstants.Reporter(true));
         Assert.IsTrue(isSame, "Should be the same volume.");

         // Same C: -> C$
         fileTmp = Path.LocalToUnc(file2);
         UnitTestConstants.StopWatcher(true);
         isSame = Volume.IsSameVolume(file1, fileTmp);
         Console.WriteLine("\nOn same Volume (Should be True): [{0}]\n\tFile1: [{1}]\n\tFile2: [{2}]\n\t{3}", isSame, file1, fileTmp, UnitTestConstants.Reporter(true));
         Assert.IsTrue(isSame, "Should be the same volume.");
      }

      #endregion // IsSameVolume

      #region SetVolumeLabel

      [TestMethod]
      public void AlphaFS_Volume_SetVolumeLabel()
      {
         Console.WriteLine("Volume.SetVolumeLabel()");

         if (!UnitTestConstants.IsAdmin())
             Assert.Inconclusive();

         const string newLabel = "ÂĽpĥæƑŞ ŠëtVőlümèĻāßƩl() Ťest";
         const string template = "\nSystem Drive: [{0}]\tCurrent Label: [{1}]";
         var drive = UnitTestConstants.SysDrive;

         #region Get Label

         var originalLabel = Volume.GetVolumeLabel(drive);
         Console.WriteLine(template, drive, originalLabel);

         Assert.IsTrue(originalLabel.Equals(Volume.GetVolumeLabel(drive)));

         #endregion // Get Label

         #region Set Label

         var isLabelSet = false;
         var currentLabel = Volume.GetVolumeLabel(drive);
         try
         {
            Volume.SetVolumeLabel(drive, newLabel);
            isLabelSet = true;

            Console.WriteLine(template, drive, newLabel);
            Console.WriteLine("Set label.");
            Assert.IsTrue(!currentLabel.Equals(newLabel));
         }
         catch (Exception ex)
         {
            Console.WriteLine("\nCaught (unexpected) {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
         }
         Assert.IsTrue(isLabelSet);

         #endregion // Set Label

         #region Remove Label

         var isLabelRemoved = false;
         try
         {
            Volume.DeleteVolumeLabel(drive);
            isLabelRemoved = true;

            currentLabel = Volume.GetVolumeLabel(drive);

            Console.WriteLine(template, drive, currentLabel);
            Console.WriteLine("Removed label.");
            Assert.IsTrue(currentLabel.Equals(string.Empty));
         }
         catch (Exception ex)
         {
            Console.WriteLine("\nCaught (unexpected) {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
         }
         Assert.IsTrue(isLabelRemoved);

         #endregion // Remove Label

         #region Restore Label

         isLabelSet = false;
         try
         {
            Volume.SetVolumeLabel(drive, originalLabel);
            isLabelSet = true;

            currentLabel = Volume.GetVolumeLabel(drive);

            Console.WriteLine(template, drive, currentLabel);
            Console.WriteLine("Restored label.");
            Assert.IsTrue(currentLabel.Equals(originalLabel));
         }
         catch (Exception ex)
         {
            Console.WriteLine("\nCaught (unexpected) {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
         }
         Assert.IsTrue(isLabelSet);

         #endregion // Restore Label
      }

      #endregion // SetVolumeLabel

      #region SetVolumeMountPoint

      [TestMethod]
      public void AlphaFS_Volume_SetVolumeMountPoint()
      {
         Console.WriteLine("Volume.SetVolumeMountPoint()");

          if (!UnitTestConstants.IsAdmin())
              Assert.Inconclusive();

         #region Logical Drives

         var cnt = 0;
         var destFolder = Path.Combine(TempFolder, "Volume.SetVolumeMountPoint()-" + Path.GetRandomFileName());
         Directory.CreateDirectory(destFolder);

         var guid = Volume.GetUniqueVolumeNameForPath(UnitTestConstants.SysDrive);

         try
         {
            UnitTestConstants.StopWatcher(true);

            Volume.SetVolumeMountPoint(destFolder, guid);
            Console.WriteLine(
               "\t#{0:000}\tSystem Drive: [{1}]\tGUID: [{2}]\n\t\tDestination : [{3}]\n\t\tCreated Mount Point.\n\t{4}",
               ++cnt, UnitTestConstants.SysDrive, guid, destFolder, UnitTestConstants.Reporter(true));

            Console.WriteLine("\n");
            AlphaFS_Volume_EnumerateVolumeMountPoints();

            Console.WriteLine("\n\nFile.GetLinkTargetInfo()");

            var lti = File.GetLinkTargetInfo(destFolder);
            Assert.IsTrue(!string.IsNullOrWhiteSpace(lti.PrintName));
            Assert.IsTrue(!string.IsNullOrWhiteSpace(lti.SubstituteName));
            Assert.IsTrue(UnitTestConstants.Dump(lti, -14), "Unable to dump object.");

            // Cleanup.
            UnitTestConstants.StopWatcher(true);
            var deleteOk = false;
            try
            {
               Volume.DeleteVolumeMountPoint(destFolder);
               deleteOk = true;
            }
            catch (Exception ex)
            {
               Console.WriteLine("\nCaught (unexpected) {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
            }

            Console.WriteLine("\n\nVolume.DeleteVolumeMountPoint() (Should be True): [{0}]\tFolder: [{1}]\n{2}\n",
               deleteOk, destFolder, UnitTestConstants.Reporter());

            Directory.Delete(destFolder, true, true);
            Assert.IsTrue(deleteOk && !Directory.Exists(destFolder));

            AlphaFS_Volume_EnumerateVolumeMountPoints();
         }
         catch (Exception ex)
         {
            cnt = 0;

            Console.WriteLine("\nCaught (unexpected) {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
         }
         finally
         {
            // Always remove mount point.
            // Experienced: CCleaner deletes through mount points!
            try { Volume.DeleteVolumeMountPoint(destFolder); }
            catch { }
         }

         if (cnt == 0)
            Assert.Inconclusive("Nothing was enumerated, but it was expected.");

         #endregion // Logical Drives
      }

      #endregion // SetVolumeMountPoint

      #endregion // Volume

      #endregion // Unit Test Callers
   }
}
