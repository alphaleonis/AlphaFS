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

using Alphaleonis.Win32;
using Alphaleonis.Win32.Filesystem;
using Alphaleonis.Win32.Network;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using Directory = Alphaleonis.Win32.Filesystem.Directory;
using DirectoryInfo = Alphaleonis.Win32.Filesystem.DirectoryInfo;
using DriveInfo = Alphaleonis.Win32.Filesystem.DriveInfo;
using File = Alphaleonis.Win32.Filesystem.File;
using FileInfo = Alphaleonis.Win32.Filesystem.FileInfo;
using OperatingSystem = Alphaleonis.Win32.OperatingSystem;
using Path = Alphaleonis.Win32.Filesystem.Path;

namespace AlphaFS.UnitTest
{
   /// <summary>This is a test class for several AlphaFS instance classes.</summary>
   [TestClass]
   public partial class AlphaFS_ClassesTest
   {
      #region Unit Tests

      #region DumpClassAlternateDataStreamInfo

      private void DumpClassAlternateDataStreamInfo(bool isLocal)
      {
         #region Setup

         Console.WriteLine("\n=== TEST {0} ===", isLocal ? UnitTestConstants.Local : UnitTestConstants.Network);

         const int defaultStreamsFile = 1; // The default number of data streams for a file.

         string tempPath;
         int currentNumberofStreams;
         int newNumberofStreams;
         string reporter;
         long fileSize;

         string random = Path.GetRandomFileName();
         string myStream = "ӍƔŞtrëƛɱ-" + random;
         string myStream2 = "myStreamTWO-" + random;

         var arrayContent = new[]
         {
            "(1) The quick brown fox jumps over the lazy dog.",
            "(2) Albert Einstein: \"Science is a wonderful thing if one does not have to earn one's living at it.",
            "(3) " + UnitTestConstants.TextHelloWorld + " " + UnitTestConstants.TextUnicode
         };

         string stringContent = "(1) Computer: [" + UnitTestConstants.LocalHost + "]" + "\tHello there, " + Environment.UserName;

         #endregion // Setup

         #region Create Stream

         tempPath = Path.GetTempPath("Class.AlternateDataStreamInfo()-file-" + Path.GetRandomFileName());
         if (!isLocal) tempPath = Path.LocalToUnc(tempPath);

         Console.WriteLine("\nInput File Path: [{0}]", tempPath);
         Console.WriteLine("\nA file is created and three streams are added.");

         
         // Create file and add 10 characters to it, file is created in ANSI format.
         File.WriteAllText(tempPath, UnitTestConstants.TenNumbers);


         var fi = new FileInfo(tempPath);

         currentNumberofStreams = File.EnumerateAlternateDataStreams(tempPath).Count();            

         Assert.AreEqual(defaultStreamsFile, currentNumberofStreams, "Total amount of default streams do not match.");
         Assert.AreEqual(currentNumberofStreams, File.EnumerateAlternateDataStreams(tempPath).Count(), "Total amount of File.EnumerateAlternateDataStreams() streams do not match.");
         Assert.AreEqual(currentNumberofStreams, fi.EnumerateAlternateDataStreams().Count(), "Total amount of FileInfo() streams do not match.");


         fileSize = File.GetSize(tempPath);
         Assert.AreEqual(UnitTestConstants.TenNumbers.Length, fileSize);
         
         
         // Create alternate data streams.
         // Because of the colon, you must supply a full path and use PathFormat.FullPath or PathFormat.FullPath
         // to prevent a NotSupportedException: path is in an invalid format.

         File.WriteAllLines(tempPath + ":" + myStream, arrayContent, PathFormat.FullPath);
         File.WriteAllText(tempPath + ":" + myStream2, stringContent, PathFormat.FullPath);

         UnitTestConstants.StopWatcher(true);
         newNumberofStreams = File.EnumerateAlternateDataStreams(tempPath).Count();
         reporter = UnitTestConstants.Reporter(true);

         // Enumerate all streams from the file.
         foreach (AlternateDataStreamInfo stream in fi.EnumerateAlternateDataStreams())
         {
            Assert.IsTrue(UnitTestConstants.Dump(stream, -10));

            // The default stream, a file as you know it.
            if (stream.StreamName == "")
               Assert.AreEqual(fileSize, stream.Size);
         }

         Console.WriteLine("\n\n\tCurrent stream Count(): [{0}]    {1}", newNumberofStreams, reporter);

         Assert.AreEqual(newNumberofStreams, File.EnumerateAlternateDataStreams(tempPath).Count(), "Total amount of streams do not match.");

         
         // Show the contents of our streams.
         foreach (string streamName in (new[] {myStream, myStream2 }))
         {
            Console.WriteLine("\n\tStream name: [{0}]", streamName);

            // Because of the colon, you must supply a full path and use PathFormat.FullPath or a NotSupportedException is thrown: path is in an invalid format.
            foreach (var line in File.ReadAllLines(tempPath + ":" + streamName, PathFormat.FullPath))
               Console.WriteLine("\t\t{0}", line);
         }

         
         UnitTestConstants.StopWatcher(true);

         #endregion // Create Stream

         File.Delete(tempPath);
         Assert.IsFalse(File.Exists(tempPath), "Cleanup failed: File should have been removed.");
         Console.WriteLine();
      }

      #endregion // DumpClassAlternateDataStreamInfo

      #region DumpClassByHandleFileInfo

      private void DumpClassByHandleFileInfo(bool isLocal)
      {
         Console.WriteLine("\n=== TEST {0} ===", isLocal ? UnitTestConstants.Local : UnitTestConstants.Network);
         string tempPath = Path.GetTempPath("File.GetFileInfoByHandle()-" + Path.GetRandomFileName());
         if (!isLocal) tempPath = Path.LocalToUnc(tempPath);

         Console.WriteLine("\nInput File Path: [{0}]", tempPath);

         FileStream stream = File.Create(tempPath);
         stream.WriteByte(1);

         UnitTestConstants.StopWatcher(true);
         ByHandleFileInfo bhfi = File.GetFileInfoByHandle(stream.SafeFileHandle);
         Console.WriteLine(UnitTestConstants.Reporter());

         Assert.IsTrue(UnitTestConstants.Dump(bhfi, -18));

         Assert.AreEqual(System.IO.File.GetCreationTimeUtc(tempPath), bhfi.CreationTimeUtc);
         Assert.AreEqual(System.IO.File.GetLastAccessTimeUtc(tempPath), bhfi.LastAccessTimeUtc);
         Assert.AreEqual(System.IO.File.GetLastWriteTimeUtc(tempPath), bhfi.LastWriteTimeUtc);

         stream.Close();

         File.Delete(tempPath, true);
         Assert.IsFalse(File.Exists(tempPath), "Cleanup failed: File should have been removed.");
         Console.WriteLine();
      }

      #endregion // DumpClassByHandleFileInfo

      #region DumpClassDeviceInfo

      private void DumpClassDeviceInfo(string host)
      {
         Console.WriteLine("\n=== TEST ===");

         bool allOk = false;
         try
         {
            #region DeviceGuid.Volume

            Console.Write("\nEnumerating volumes from host: [{0}]\n", host);
            int cnt = 0;
            UnitTestConstants.StopWatcher(true);
            foreach (DeviceInfo device in Device.EnumerateDevices(host, DeviceGuid.Volume))
            {
               Console.WriteLine("\n#{0:000}\tClass: [{1}]", ++cnt, device.Class);

               UnitTestConstants.Dump(device, -24);

               try
               {
                  string getDriveLetter = Volume.GetDriveNameForNtDeviceName(device.PhysicalDeviceObjectName);
                  string dosdeviceGuid = Volume.GetVolumeGuidForNtDeviceName(device.PhysicalDeviceObjectName);

                  Console.WriteLine("\n\tVolume.GetDriveNameForNtDeviceName() : [{0}]", getDriveLetter ?? "null");
                  Console.WriteLine("\tVolume.GetVolumeGuidForNtDeviceName(): [{0}]", dosdeviceGuid ?? "null");
               }
               catch (Exception ex)
               {
                  Console.WriteLine("\nCaught (unexpected) {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
               }
            }
            Console.WriteLine(UnitTestConstants.Reporter());
            Console.WriteLine();

            #endregion // DeviceGuid.Volume

            #region DeviceGuid.Disk

            Console.Write("\nEnumerating disks from host: [{0}]\n", host);
            cnt = 0;
            UnitTestConstants.StopWatcher(true);
            foreach (DeviceInfo device in Device.EnumerateDevices(host, DeviceGuid.Disk))
            {
               Console.WriteLine("\n#{0:000}\tClass: [{1}]", ++cnt, device.Class);

               UnitTestConstants.Dump(device, -24);

               try
               {
                  string getDriveLetter = Volume.GetDriveNameForNtDeviceName(device.PhysicalDeviceObjectName);
                  string dosdeviceGuid = Volume.GetVolumeGuidForNtDeviceName(device.PhysicalDeviceObjectName);

                  Console.WriteLine("\n\tVolume.GetDriveNameForNtDeviceName() : [{0}]", getDriveLetter ?? "null");
                  Console.WriteLine("\tVolume.GetVolumeGuidForNtDeviceName(): [{0}]", dosdeviceGuid ?? "null");
               }
               catch (Exception ex)
               {
                  Console.WriteLine("\nCaught (unexpected) {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
               }
            }
            Console.WriteLine(UnitTestConstants.Reporter());

            #endregion // DeviceGuid.Disk

            allOk = true;
         }
         catch (Exception ex)
         {
            Console.WriteLine("\tCaught (unexpected) {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
         }

         Assert.IsTrue(allOk, "Could (probably) not connect to host: [{0}]", host);
         Console.WriteLine();
      }

      #endregion // DumpClassDeviceInfo

      #region DumpClassDirectoryInfo
      
      private void DumpClassDirectoryInfo(bool isLocal)
      {
         #region Setup

         Console.WriteLine("\n=== TEST {0} ===", isLocal ? UnitTestConstants.Local : UnitTestConstants.Network);
         string tempPath = Path.Combine(Path.GetTempPath(), "DirectoryInfo()-" + Path.GetRandomFileName());
         if (!isLocal) tempPath = Path.LocalToUnc(tempPath);

         int expectedLastError;
         string expectedException;

         string nonExistingDirectory = UnitTestConstants.SysRoot32 + @"\NonExistingDirectory-" + Path.GetRandomFileName();
         if (!isLocal) nonExistingDirectory = Path.LocalToUnc(nonExistingDirectory);

         string sysDrive = UnitTestConstants.SysDrive;
         if (!isLocal) sysDrive = Path.LocalToUnc(sysDrive);

         string sysRoot = UnitTestConstants.SysRoot;
         if (!isLocal) sysRoot = Path.LocalToUnc(sysRoot);

         string letter = DriveInfo.GetFreeDriveLetter() + @":\";
         if (!isLocal) letter = Path.LocalToUnc(letter);

         #endregion // Setup

         #region NotSupportedException

         expectedLastError = (int) (isLocal ? Win32Errors.ERROR_ENVVAR_NOT_FOUND : Win32Errors.NERR_UseNotFound);
         expectedException = "System.NotSupportedException";
         bool exception = false;
         try
         {
            Console.WriteLine("\nCatch: [{0}]: The given path's format is not supported.", expectedException);

            string invalidPath = UnitTestConstants.SysDrive + @"\:a";
            if (!isLocal) invalidPath = Path.LocalToUnc(invalidPath) + @":a";

            DirectoryInfo di = new DirectoryInfo(invalidPath);
         }
         catch (Exception ex)
         {
            // Not reliable.
            //var win32Error = new Win32Exception("", ex);
            //Assert.IsTrue(win32Error.NativeErrorCode == expectedLastError, string.Format("Expected Win32Exception error should be: [{0}], got: [{1}]", expectedLastError, win32Error.NativeErrorCode));
            //Assert.IsTrue(ex.Message.StartsWith("(" + expectedLastError + ")"), string.Format("Expected Win32Exception error is: [{0}]", expectedLastError));

            string exceptionTypeName = ex.GetType().FullName;
            if (exceptionTypeName.Equals(expectedException))
            {
               exception = true;
               Console.WriteLine("\n\t[{0}]: [{1}]", exceptionTypeName, ex.Message.Replace(Environment.NewLine, "  "));
            }
            else
               Console.WriteLine("\n\tCaught (unexpected) {0}: [{1}]", exceptionTypeName, ex.Message.Replace(Environment.NewLine, "  "));
         }
         Assert.IsTrue(exception, "[{0}] should have been caught.", expectedException);
         Console.WriteLine();

         #endregion // NotSupportedException


         #region Current Directory

         tempPath = Path.CurrentDirectoryPrefix;
         if (!isLocal) tempPath = Path.LocalToUnc(tempPath);

         Console.WriteLine("\nInput Directory Path (Current directory): [{0}]\n", tempPath);

         UnitTestConstants.StopWatcher(true);
         System.IO.DirectoryInfo expected = new System.IO.DirectoryInfo(tempPath);
         Console.WriteLine("\tSystem.IO DirectoryInfo(){0}", UnitTestConstants.Reporter());

         UnitTestConstants.StopWatcher(true);
         DirectoryInfo actual = new DirectoryInfo(tempPath);
         Console.WriteLine("\tAlphaFS DirectoryInfo(){0}", UnitTestConstants.Reporter());

         // Compare values of both instances.
         CompareDirectoryInfos(expected, actual);

         #endregion // Current Directory

         #region Non-Existing Directory

         Console.WriteLine("\nInput Directory Path: [{0}]\n", nonExistingDirectory);

         UnitTestConstants.StopWatcher(true);
         expected = new System.IO.DirectoryInfo(tempPath);
         Console.WriteLine("\tSystem.IO DirectoryInfo(){0}", UnitTestConstants.Reporter());

         UnitTestConstants.StopWatcher(true);
         actual = new DirectoryInfo(tempPath);
         Console.WriteLine("\tAlphaFS DirectoryInfo(){0}", UnitTestConstants.Reporter());

         // Compare values of both instances.
         CompareDirectoryInfos(expected, actual);

         #endregion // Non-Existing Directory

         #region Existing Directory

         tempPath = Path.Combine(Path.GetTempPath(), "DirectoryInfo()-Directory-" + Path.GetRandomFileName());
         if (!isLocal) tempPath = Path.LocalToUnc(tempPath);

         try
         {
            Directory.CreateDirectory(tempPath);

            Console.WriteLine("\n\nInput Directory Path: [{0}]\n", tempPath);

            UnitTestConstants.StopWatcher(true);
            expected = new System.IO.DirectoryInfo(tempPath);
            Console.WriteLine("\tSystem.IO DirectoryInfo(){0}", UnitTestConstants.Reporter());

            UnitTestConstants.StopWatcher(true);
            actual = new DirectoryInfo(tempPath);
            Console.WriteLine("\tAlphaFS DirectoryInfo(){0}", UnitTestConstants.Reporter());

            // Compare values of both instances.
            CompareDirectoryInfos(expected, actual);
         }
         finally
         {
             if (Directory.Exists(tempPath))
                 Directory.Delete(tempPath);

            Assert.IsFalse(Directory.Exists(tempPath), "Cleanup failed: Directory should have been removed.");
         }
         
         #endregion // Existing Directory

         #region Method .ToString()

         Console.WriteLine("\nMethod .ToString()");
         Console.WriteLine("Both strings should be the same.\n");

         expected = new System.IO.DirectoryInfo("ToString()-TestDirectory");
         actual = new DirectoryInfo("ToString()-TestDirectory");

         string expectedToString = expected.ToString();
         string actualToString = actual.ToString();

         Console.WriteLine("\tSystem.IO: [{0}]", expectedToString);
         Console.WriteLine("\tAlphaFS  : [{0}]", actualToString);

         Assert.AreEqual(expectedToString, actualToString, false);
         Console.WriteLine();

         #endregion Method .ToString()
      }

      private void CompareDirectoryInfos(System.IO.DirectoryInfo expected, DirectoryInfo actual)
      {
         if (expected == null || actual == null)
            Assert.AreEqual(expected, actual, "Mismatch");

         UnitTestConstants.Dump(expected, -17);
         UnitTestConstants.Dump(actual, -17);
         

         int errorCnt = 0;
         int cnt = -1;
         while (cnt != 13)
         {
            cnt++;

            if (expected == null || actual == null)
               Assert.AreEqual(expected, actual, "One or both of the DirectoryInfo instances is/are null.");

            else
            {
               try
               {
                  // Compare values of both instances.
                  switch (cnt)
                  {
                     case 0: Assert.AreEqual(expected.Attributes, actual.Attributes, "Attributes AlphaFS != System.IO"); break;
                     case 1: Assert.AreEqual(expected.CreationTime, actual.CreationTime, "CreationTime AlphaFS != System.IO"); break;
                     case 2: Assert.AreEqual(expected.CreationTimeUtc, actual.CreationTimeUtc, "CreationTimeUtc AlphaFS != System.IO"); break;
                     case 3: Assert.AreEqual(expected.Exists, actual.Exists, "Exists AlphaFS != System.IO"); break;
                     case 4: Assert.AreEqual(expected.Extension, actual.Extension, "Extension AlphaFS != System.IO"); break;
                     case 5: Assert.AreEqual(expected.FullName, actual.FullName, "FullName AlphaFS != System.IO"); break;
                     case 6: Assert.AreEqual(expected.LastAccessTime, actual.LastAccessTime, "LastAccessTime AlphaFS != System.IO"); break;
                     case 7: Assert.AreEqual(expected.LastAccessTimeUtc, actual.LastAccessTimeUtc, "LastAccessTimeUtc AlphaFS != System.IO"); break;
                     case 8: Assert.AreEqual(expected.LastWriteTime, actual.LastWriteTime, "LastWriteTime AlphaFS != System.IO"); break;
                     case 9: Assert.AreEqual(expected.LastWriteTimeUtc, actual.LastWriteTimeUtc, "LastWriteTimeUtc AlphaFS != System.IO"); break;
                     case 10: Assert.AreEqual(expected.Name, actual.Name, "Name AlphaFS != System.IO"); break;

                     // Need .ToString() here since the object types are obviously not the same.
                     case 11: Assert.AreEqual(expected.Parent.ToString(), actual.Parent.ToString(), "Parent AlphaFS != System.IO"); break;
                     case 12: Assert.AreEqual(expected.Root.ToString(), actual.Root.ToString(), "Root AlphaFS != System.IO"); break;
                  }
               }
               catch (Exception ex)
               {
                  errorCnt++;
                  Console.WriteLine("\n\t\tProperty cnt #{0}\tCaught {1}: [{2}]", (cnt + 1), ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
               }
            }
         }

         //Assert.IsTrue(errorCnt == 0, "\tEncountered: [{0}] DirectoryInfo Properties where AlphaFS != System.IO", errorCnt);
         Console.WriteLine();
      }

      #endregion // DumpClassDirectoryInfo

      #region DumpClassDiskSpaceInfo

      private void DumpClassDiskSpaceInfo(bool isLocal)
      {
         Console.WriteLine("\n=== TEST {0} ===", isLocal ? UnitTestConstants.Local : UnitTestConstants.Network);
         string tempPath = UnitTestConstants.SysDrive;
         if (!isLocal) tempPath = Path.LocalToUnc(tempPath);
         Console.WriteLine("\nInput Path: [{0}]", tempPath);

         int cnt = 0;
         int errorCount = 0;

          // Get only .IsReady drives.
         foreach (var drv in Directory.EnumerateLogicalDrives(false, true))
         {
            if (drv.DriveType == DriveType.NoRootDirectory)
               continue;

            string drive = isLocal ? drv.Name : Path.LocalToUnc(drv.Name);

            UnitTestConstants.StopWatcher(true);

            try
            {
               // null (default) == All information.
               DiskSpaceInfo dsi = drv.DiskSpaceInfo;
               string report = UnitTestConstants.Reporter(true);

               Console.WriteLine("\n#{0:000}\tInput Path: [{1}]{2}", ++cnt, drive, report);
               Assert.IsTrue(UnitTestConstants.Dump(dsi, -26));

               Assert.AreNotEqual((int) 0, (int) dsi.BytesPerSector);
               Assert.AreNotEqual((int) 0, (int) dsi.SectorsPerCluster);
               Assert.AreNotEqual((int) 0, (int) dsi.TotalNumberOfClusters);
               Assert.AreNotEqual((int) 0, (int) dsi.TotalNumberOfBytes);

               if (drv.DriveType == DriveType.CDRom)
               {
                  Assert.AreEqual((int) 0, (int) dsi.FreeBytesAvailable);
                  Assert.AreEqual((int) 0, (int) dsi.NumberOfFreeClusters);
                  Assert.AreEqual((int) 0, (int) dsi.TotalNumberOfFreeBytes);
               }
               else
               {
                  Assert.AreNotEqual((int) 0, (int) dsi.FreeBytesAvailable);
                  Assert.AreNotEqual((int) 0, (int) dsi.NumberOfFreeClusters);
                  Assert.AreNotEqual((int) 0, (int) dsi.TotalNumberOfFreeBytes);
               }

               // false == Size information only.
               dsi = Volume.GetDiskFreeSpace(drive, false);
               Assert.AreEqual((int)0, (int)dsi.BytesPerSector);
               Assert.AreEqual((int)0, (int)dsi.NumberOfFreeClusters);
               Assert.AreEqual((int)0, (int)dsi.SectorsPerCluster);
               Assert.AreEqual((int)0, (int)dsi.TotalNumberOfClusters);


               // true == Cluster information only.
               dsi = Volume.GetDiskFreeSpace(drive, true);
               Assert.AreEqual((int)0, (int)dsi.FreeBytesAvailable);
               Assert.AreEqual((int)0, (int)dsi.TotalNumberOfBytes);
               Assert.AreEqual((int)0, (int)dsi.TotalNumberOfFreeBytes);
            }
            catch (Exception ex)
            {
               Console.WriteLine("\n\nCaught (unexpected) {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
               errorCount++;
            }
         }

         if (cnt == 0)
            Assert.Inconclusive("Nothing was enumerated.");

         Assert.AreEqual(0, errorCount, "No errors were expected.");
         Console.WriteLine();
      }

      #endregion // DumpClassDiskSpaceInfo

      #region DumpClassDriveInfo

      private void DumpClassDriveInfo(bool isLocal, string drive)
      {
         Console.WriteLine("\n=== TEST {0} ===", isLocal ? UnitTestConstants.Local : UnitTestConstants.Network);
         string tempPath = drive;
         if (!isLocal) tempPath = Path.LocalToUnc(tempPath);

         UnitTestConstants.StopWatcher(true);
         DriveInfo actual = new DriveInfo(tempPath);
         Console.WriteLine("\nInput Path: [{0}]{1}", tempPath, UnitTestConstants.Reporter());

         #region UnitTestConstants.Local Drive
         if (isLocal)
         {
            // System.IO.DriveInfo() can not handle UNC paths.
            System.IO.DriveInfo expected = new System.IO.DriveInfo(tempPath);
            //Dump(expected, -21);

            // Even 1 byte more or less results in failure, so do these tests asap.
            Assert.AreEqual(expected.AvailableFreeSpace, actual.AvailableFreeSpace, "AvailableFreeSpace AlphaFS != System.IO");
            Assert.AreEqual(expected.TotalFreeSpace, actual.TotalFreeSpace, "TotalFreeSpace AlphaFS != System.IO");
            Assert.AreEqual(expected.TotalSize, actual.TotalSize, "TotalSize AlphaFS != System.IO");

            Assert.AreEqual(expected.DriveFormat, actual.DriveFormat, "DriveFormat AlphaFS != System.IO");
            Assert.AreEqual(expected.DriveType, actual.DriveType, "DriveType AlphaFS != System.IO");
            Assert.AreEqual(expected.IsReady, actual.IsReady, "IsReady AlphaFS != System.IO");
            Assert.AreEqual(expected.Name, actual.Name, "Name AlphaFS != System.IO");
            Assert.AreEqual(expected.RootDirectory.ToString(), actual.RootDirectory.ToString(), "RootDirectory AlphaFS != System.IO");
            Assert.AreEqual(expected.VolumeLabel, actual.VolumeLabel, "VolumeLabel AlphaFS != System.IO");
         }

         #region Class Equality

         ////int getHashCode1 = actual.GetHashCode();

         //DriveInfo driveInfo2 = new DriveInfo(tempPath);
         ////int getHashCode2 = driveInfo2.GetHashCode();

         //DriveInfo clone = actual;
         ////int getHashCode3 = clone.GetHashCode();

         //bool isTrue1 = clone.Equals(actual);
         //bool isTrue2 = clone == actual;
         //bool isTrue3 = !(clone != actual);
         //bool isTrue4 = actual == driveInfo2;
         //bool isTrue5 = !(actual != driveInfo2);

         ////Console.WriteLine("\n\t\t actual.GetHashCode() : [{0}]", getHashCode1);
         ////Console.WriteLine("\t\t     clone.GetHashCode() : [{0}]", getHashCode3);
         ////Console.WriteLine("\t\tdriveInfo2.GetHashCode() : [{0}]\n", getHashCode2);

         ////Console.WriteLine("\t\t obj clone.ToString() == [{0}]", clone.ToString());
         ////Console.WriteLine("\t\t obj clone.Equals()   == [{0}] : {1}", TextTrue, isTrue1);
         ////Console.WriteLine("\t\t obj clone ==         == [{0}] : {1}", TextTrue, isTrue2);
         ////Console.WriteLine("\t\t obj clone !=         == [{0}]: {1}\n", TextFalse, isTrue3);

         ////Console.WriteLine("\t\tdriveInfo == driveInfo2 == [{0}] : {1}", TextTrue, isTrue4);
         ////Console.WriteLine("\t\tdriveInfo != driveInfo2 == [{0}] : {1}", TextFalse, isTrue5);

         //Assert.IsTrue(isTrue1, "clone.Equals(actual)");
         //Assert.IsTrue(isTrue2, "clone == actual");
         //Assert.IsTrue(isTrue3, "!(clone != actual)");
         //Assert.IsTrue(isTrue4, "actual == driveInfo2");
         //Assert.IsTrue(isTrue5, "!(actual != driveInfo2)");

         #endregion // Class Equality

         #endregion // UnitTestConstants.Local Drive

         UnitTestConstants.StopWatcher(true);
         UnitTestConstants.Dump(actual, -21);

         UnitTestConstants.Dump(actual.DiskSpaceInfo, -26);

         //if (expected != null) Dump(expected.RootDirectory, -17);
         //Dump(actual.RootDirectory, -17);

         UnitTestConstants.Dump(actual.VolumeInfo, -26);

         Console.WriteLine(UnitTestConstants.Reporter());
         Console.WriteLine();
      }

      #endregion // DumpClassDriveInfo

      #region DumpClassFileInfo
      
      private void DumpClassFileInfo(bool isLocal)
      {
         #region Setup

         Console.WriteLine("\n=== TEST {0} ===", isLocal ? UnitTestConstants.Local : UnitTestConstants.Network);
         string tempPath = Path.Combine(Path.GetTempPath(), "FileInfo()-" + Path.GetRandomFileName());
         if (!isLocal) tempPath = Path.LocalToUnc(tempPath);

         int expectedLastError;
         string expectedException;

         string nonExistingFile = UnitTestConstants.SysRoot32 + @"\NonExistingFile-" + Path.GetRandomFileName();
         if (!isLocal) nonExistingFile = Path.LocalToUnc(nonExistingFile);

         string sysDrive = UnitTestConstants.SysDrive;
         if (!isLocal) sysDrive = Path.LocalToUnc(sysDrive);

         string sysRoot = UnitTestConstants.SysRoot;
         if (!isLocal) sysRoot = Path.LocalToUnc(sysRoot);

         string letter = DriveInfo.GetFreeDriveLetter() + @":\";
         if (!isLocal) letter = Path.LocalToUnc(letter);

         #endregion // Setup

         #region NotSupportedException

         expectedLastError = (int) (isLocal ? Win32Errors.ERROR_ENVVAR_NOT_FOUND : Win32Errors.NERR_UseNotFound);
         expectedException = "System.NotSupportedException";
         bool exception = false;
         try
         {
            Console.WriteLine("\nCatch: [{0}]: The given path's format is not supported.", expectedException);

            string invalidPath = UnitTestConstants.SysDrive + @"\:a";
            if (!isLocal) invalidPath = Path.LocalToUnc(invalidPath) + @":a";

            FileInfo fi = new FileInfo(invalidPath);
         }
         catch (Exception ex)
         {
            // Not reliable.
            //var win32Error = new Win32Exception("", ex);
            //Assert.IsTrue(win32Error.NativeErrorCode == expectedLastError, string.Format("Expected Win32Exception error should be: [{0}], got: [{1}]", expectedLastError, win32Error.NativeErrorCode));
            //Assert.IsTrue(ex.Message.StartsWith("(" + expectedLastError + ")"), string.Format("Expected Win32Exception error is: [{0}]", expectedLastError));

            string exceptionTypeName = ex.GetType().FullName;
            if (exceptionTypeName.Equals(expectedException))
            {
               exception = true;
               Console.WriteLine("\n\t[{0}]: [{1}]", exceptionTypeName, ex.Message.Replace(Environment.NewLine, "  "));
            }
            else
               Console.WriteLine("\n\tCaught (unexpected) {0}: [{1}]", exceptionTypeName, ex.Message.Replace(Environment.NewLine, "  "));
         }
         Assert.IsTrue(exception, "[{0}] should have been caught.", expectedException);
         Console.WriteLine();

         #endregion // NotSupportedException

         #region Length Property

         #region FileNotFoundException #1

         expectedLastError = (int) Win32Errors.ERROR_FILE_NOT_FOUND;
         expectedException = "System.IO.FileNotFoundException";
         exception = false;
         try
         {
            Console.WriteLine("\nCatch: [{0}]: Length property is called, the file does not exist.", expectedException);
            Console.WriteLine(new FileInfo(nonExistingFile).Length);
         }
         catch (Exception ex)
         {
            var win32Error = new Win32Exception("", ex);
            Assert.IsTrue(win32Error.NativeErrorCode == expectedLastError, string.Format("Expected Win32Exception error should be: [{0}], got: [{1}]", expectedLastError, win32Error.NativeErrorCode));
            Assert.IsTrue(ex.Message.StartsWith("(" + expectedLastError + ")"), string.Format("Expected Win32Exception error is: [{0}]", expectedLastError));

            string exceptionTypeName = ex.GetType().FullName;
            if (exceptionTypeName.Equals(expectedException))
            {
               exception = true;
               Console.WriteLine("\n\t[{0}]: [{1}]", exceptionTypeName, ex.Message.Replace(Environment.NewLine, "  "));
            }
            else
               Console.WriteLine("\n\tCaught (unexpected) {0}: [{1}]", exceptionTypeName, ex.Message.Replace(Environment.NewLine, "  "));
         }
         Assert.IsTrue(exception, "[{0}] should have been caught.", expectedException);
         Console.WriteLine();

         #endregion // FileNotFoundException #1

         #region FileNotFoundException #2

         expectedLastError = (int) (isLocal ? Win32Errors.ERROR_PATH_NOT_FOUND : Win32Errors.ERROR_BAD_NET_NAME);
         expectedException = isLocal ? "System.IO.FileNotFoundException" : "System.IO.IOException";
         exception = false;
         try
         {
            Console.WriteLine("\nCatch: [{0}]: Length property is called, the file does not exist (Unmapped drive).", expectedException);

            Console.WriteLine(new FileInfo(nonExistingFile.Replace(sysDrive + @"\", letter)).Length);
         }
         catch (Exception ex)
         {
            var win32Error = new Win32Exception("", ex);
            Assert.IsTrue(win32Error.NativeErrorCode == expectedLastError, string.Format("Expected Win32Exception error should be: [{0}], got: [{1}]", expectedLastError, win32Error.NativeErrorCode));
            //Assert.IsTrue(ex.Message.StartsWith("(" + expectedLastError + ")"), string.Format("Expected Win32Exception error is: [{0}]", expectedLastError));

            string exceptionTypeName = ex.GetType().FullName;
            if (exceptionTypeName.Equals(expectedException))
            {
               exception = true;
               Console.WriteLine("\n\t[{0}]: [{1}]", exceptionTypeName, ex.Message.Replace(Environment.NewLine, "  "));
            }
            else
               Console.WriteLine("\n\tCaught (unexpected) {0}: [{1}]", exceptionTypeName, ex.Message.Replace(Environment.NewLine, "  "));
         }
         Assert.IsTrue(exception, "[{0}] should have been caught.", expectedException);
         Console.WriteLine();

         #endregion // FileNotFoundException #1
         
         #region FileNotFoundException #3

         expectedLastError = (int) Win32Errors.ERROR_FILE_NOT_FOUND;
         expectedException = "System.IO.FileNotFoundException";
         exception = false;
         try
         {
            Console.WriteLine("\nCatch: [{0}]: Length property is called, the file is a directory.", expectedException);
            Console.WriteLine(new FileInfo(sysRoot).Length);
         }
         catch (Exception ex)
         {
            // win32Error is always 0
            //var win32Error = new Win32Exception("", ex);
            //Assert.IsTrue(win32Error.NativeErrorCode == expectedLastError, string.Format("Expected Win32Exception error should be: [{0}], got: [{1}]", expectedLastError, win32Error.NativeErrorCode));
            Assert.IsTrue(ex.Message.StartsWith("(" + expectedLastError + ")"), string.Format("Expected Win32Exception error is: [{0}]", expectedLastError));

            string exceptionTypeName = ex.GetType().FullName;
            if (exceptionTypeName.Equals(expectedException))
            {
               exception = true;
               Console.WriteLine("\n\t[{0}]: [{1}]", exceptionTypeName, ex.Message.Replace(Environment.NewLine, "  "));
            }
            else
               Console.WriteLine("\n\tCaught (unexpected) {0}: [{1}]", exceptionTypeName, ex.Message.Replace(Environment.NewLine, "  "));
         }
         Assert.IsTrue(exception, "[{0}] should have been caught.", expectedException);
         Console.WriteLine();

         #endregion // FileNotFoundException #3

         #endregion // Length Property

         
         #region Current Directory

         tempPath = Path.CurrentDirectoryPrefix;
         if (!isLocal) tempPath = Path.LocalToUnc(tempPath);

         Console.WriteLine("\nInput File Path (Current directory): [{0}]\n", tempPath);

         UnitTestConstants.StopWatcher(true);
         System.IO.FileInfo expected = new System.IO.FileInfo(tempPath);
         Console.WriteLine("\tSystem.IO FileInfo(){0}", UnitTestConstants.Reporter());

         UnitTestConstants.StopWatcher(true);
         FileInfo actual = new FileInfo(tempPath);
         Console.WriteLine("\tAlphaFS FileInfo(){0}", UnitTestConstants.Reporter());

         // Compare values of both instances.
         CompareFileInfos(expected, actual);

         #endregion // Current Directory

         #region Non-Existing File

         Console.WriteLine("\nInput File Path: [{0}]\n", nonExistingFile);

         UnitTestConstants.StopWatcher(true);
         expected = new System.IO.FileInfo(nonExistingFile);
         Console.WriteLine("\tSystem.IO FileInfo(){0}", UnitTestConstants.Reporter());

         UnitTestConstants.StopWatcher(true);
         actual = new FileInfo(nonExistingFile);
         Console.WriteLine("\tAlphaFS FileInfo(){0}", UnitTestConstants.Reporter());

         // Compare values of both instances.
         CompareFileInfos(expected, actual);

         #endregion // Non-Existing File

         #region Existing File

         tempPath = Path.Combine(Path.GetTempPath(), "FileInfo()-File-" + Path.GetRandomFileName());
         if (!isLocal) tempPath = Path.LocalToUnc(tempPath);

         try
         {
            using (File.Create(tempPath)) {}

            Console.WriteLine("\nInput File Path: [{0}]\n", tempPath);

            UnitTestConstants.StopWatcher(true);
            expected = new System.IO.FileInfo(tempPath);
            Console.WriteLine("\tSystem.IO FileInfo(){0}", UnitTestConstants.Reporter());

            UnitTestConstants.StopWatcher(true);
            actual = new FileInfo(tempPath);
            Console.WriteLine("\tAlphaFS FileInfo(){0}", UnitTestConstants.Reporter());

            // Compare values of both instances.
            CompareFileInfos(expected, actual);
         }
         finally
         {
            File.Delete(tempPath);
            Assert.IsFalse(File.Exists(tempPath), "Cleanup failed: File should have been removed.");
         }

         #endregion // Existing File

         #region Method .ToString()

         Console.WriteLine("\nMethod .ToString()");
         Console.WriteLine("Both strings should be the same.\n");

         expected = new System.IO.FileInfo("ToString()-TestFile");
         actual = new FileInfo("ToString()-TestFile");

         string expectedToString = expected.ToString();
         string actualToString = actual.ToString();

         Console.WriteLine("\tSystem.IO: [{0}]", expectedToString);
         Console.WriteLine("\tAlphaFS  : [{0}]", actualToString);

         Assert.AreEqual(expectedToString, actualToString, false);
         Console.WriteLine();

         #endregion Method .ToString()
      }

      private void CompareFileInfos(System.IO.FileInfo expected, FileInfo actual)
      {
         if (expected == null || actual == null)
            Assert.AreEqual(expected, actual, "Mismatch");

         UnitTestConstants.Dump(expected, -17);
         UnitTestConstants.Dump(actual, -17);


         int errorCnt = 0;
         int cnt = -1;
         while (cnt != 15)
         {
            cnt++;
            try
            {
               // Compare values of both instances.
               switch (cnt)
               {
                  case 0:
                     Assert.AreEqual(expected.Attributes, actual.Attributes, "Attributes AlphaFS != System.IO");
                     break;
                  case 1:
                     Assert.AreEqual(expected.CreationTime, actual.CreationTime, "CreationTime AlphaFS != System.IO");
                     break;
                  case 2:
                     Assert.AreEqual(expected.CreationTimeUtc, actual.CreationTimeUtc, "CreationTimeUtc AlphaFS != System.IO");
                     break;

                  // Need .ToString() here since the object types are obviously not the same.
                  case 3:
                     Assert.AreEqual(expected.Directory.ToString(), actual.Directory.ToString(), "Directory AlphaFS != System.IO");
                     break;

                  case 4:
                     Assert.AreEqual(expected.DirectoryName, actual.DirectoryName, "DirectoryName AlphaFS != System.IO");
                     break;
                  case 5:
                     Assert.AreEqual(expected.Exists, actual.Exists, "Exists AlphaFS != System.IO");
                     break;
                  case 6:
                     Assert.AreEqual(expected.Extension, actual.Extension, "Extension AlphaFS != System.IO");
                     break;
                  case 7:
                     Assert.AreEqual(expected.FullName, actual.FullName, "FullName AlphaFS != System.IO");
                     break;
                  case 8:
                     Assert.AreEqual(expected.IsReadOnly, actual.IsReadOnly, "IsReadOnly AlphaFS != System.IO");
                     break;
                  case 9:
                     Assert.AreEqual(expected.LastAccessTime, actual.LastAccessTime, "LastAccessTime AlphaFS != System.IO");
                     break;
                  case 10:
                     Assert.AreEqual(expected.LastAccessTimeUtc, actual.LastAccessTimeUtc, "LastAccessTimeUtc AlphaFS != System.IO");
                     break;
                  case 11:
                     Assert.AreEqual(expected.LastWriteTime, actual.LastWriteTime, "LastWriteTime AlphaFS != System.IO");
                     break;
                  case 12:
                     Assert.AreEqual(expected.LastWriteTimeUtc, actual.LastWriteTimeUtc, "LastWriteTimeUtc AlphaFS != System.IO");
                     break;
                  case 13:
                     Assert.AreEqual(expected.Length, actual.Length, "Length AlphaFS != System.IO");
                     break;
                  case 14:
                     Assert.AreEqual(expected.Name, actual.Name, "Name AlphaFS != System.IO");
                     break;
               }
            }
            catch (Exception ex)
            {
               errorCnt++;
               Console.WriteLine("\n\t\t\tProperty cnt #{0}\tCaught {1}: [{2}]", (cnt + 1), ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
            }
         }

         //Assert.IsTrue(errorCnt == 0, "\tEncountered: [{0}] FileInfo Properties where AlphaFS != System.IO", errorCnt);
         Console.WriteLine();
      }

      #endregion // DumpClassFileInfo
      
      #region DumpClassFileSystemEntryInfo

      private void DumpClassFileSystemEntryInfo(bool isLocal)
      {
         Console.WriteLine("\n=== TEST {0} ===", isLocal ? UnitTestConstants.Local : UnitTestConstants.Network);

         Console.WriteLine("\nThe return type is based on C# inference. Possible return types are:");
         Console.WriteLine("string (full path), FileSystemInfo (DiskInfo / FileInfo) or FileSystemEntryInfo instance.\n");

         #region Directory

         string path = UnitTestConstants.SysRoot;
         if (!isLocal) path = Path.LocalToUnc(path);

         Console.WriteLine("\nInput Directory Path: [{0}]", path);

         Console.WriteLine("\n\nvar fsei = Directory.GetFileSystemEntry(path);");
         var asFileSystemEntryInfo = File.GetFileSystemEntryInfo(path);
         Assert.IsTrue((asFileSystemEntryInfo.GetType().IsEquivalentTo(typeof(FileSystemEntryInfo))));
         Assert.IsTrue(UnitTestConstants.Dump(asFileSystemEntryInfo, -17));

         Console.WriteLine();

         #endregion // Directory

         #region File

         path = UnitTestConstants.NotepadExe;
         if (!isLocal) path = Path.LocalToUnc(path);

         Console.WriteLine("\nInput File Path: [{0}]", path);

         Console.WriteLine("\n\nvar fsei = File.GetFileSystemEntry(path);");
         asFileSystemEntryInfo = File.GetFileSystemEntryInfo(path);
         Assert.IsTrue((asFileSystemEntryInfo.GetType().IsEquivalentTo(typeof(FileSystemEntryInfo))));
         Assert.IsTrue(UnitTestConstants.Dump(asFileSystemEntryInfo, -17));

         Console.WriteLine();

         #endregion // File
      }

      #endregion // DumpClassFileSystemEntryInfo

      #region DumpClassShell32Info

      private void DumpClassShell32Info(bool isLocal)
      {
         Console.WriteLine("\n=== TEST {0} ===", isLocal ? UnitTestConstants.Local : UnitTestConstants.Network);
         string tempPath = Path.GetTempPath("Class.Shell32Info()-" + Path.GetRandomFileName());
         if (!isLocal) tempPath = Path.LocalToUnc(tempPath);

         Console.WriteLine("\nInput File Path: [{0}]\n", tempPath);

         using (File.Create(tempPath))
         {}


         
         UnitTestConstants.StopWatcher(true);
         Shell32Info shell32Info = Shell32.GetShell32Info(tempPath);
         string report = UnitTestConstants.Reporter();

         Console.WriteLine("\tMethod: Shell32Info.Refresh()");
         Console.WriteLine("\tMethod: Shell32Info.GetIcon()");
         

         string cmd = "print";
         Console.WriteLine("\tMethod: Shell32Info.GetVerbCommand(\"{0}\") == [{1}]", cmd, shell32Info.GetVerbCommand(cmd));

         Assert.IsTrue(UnitTestConstants.Dump(shell32Info, -15));

         File.Delete(tempPath, true);
         Assert.IsFalse(File.Exists(tempPath), "Cleanup failed: File should have been removed.");

         Console.WriteLine("\n{0}", report);
         Console.WriteLine();
      }

      #endregion // DumpClassShell32Info

      #region DumpClassVolumeInfo

      private void DumpClassVolumeInfo(bool isLocal)
      {
         Console.WriteLine("\n=== TEST {0} ===", isLocal ? UnitTestConstants.Local : UnitTestConstants.Network);
         Console.WriteLine("\nEnumerating logical drives.");

         int cnt = 0;
         foreach (string drive in Directory.GetLogicalDrives())
         {
            string tempPath = drive;
            if (!isLocal) tempPath = Path.LocalToUnc(tempPath);

            UnitTestConstants.StopWatcher(true);
            try
            {
               VolumeInfo volInfo = Volume.GetVolumeInfo(tempPath);
               Console.WriteLine("\n#{0:000}\tLogical Drive: [{1}]", ++cnt, tempPath);
               UnitTestConstants.Dump(volInfo, -26);
            }
            catch (Exception ex)
            {
               Console.WriteLine("#{0:000}\tLogical Drive: [{1}]\n\tCaught: {2}: {3}", ++cnt, tempPath, ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
            }
            Console.WriteLine();
         }

         if (cnt == 0)
            Assert.Inconclusive("Nothing was enumerated.");

         Console.WriteLine();
      }

      #endregion // DumpClassVolumeInfo



      #region DumpClassDfsInfo

      private void DumpClassDfsInfo()
      {
         int cnt = 0;
         bool noDomainConnection = true;

         UnitTestConstants.StopWatcher(true);
         try
         {
            foreach (string dfsNamespace in Host.EnumerateDomainDfsRoot())
            {
               noDomainConnection = false;

               try
               {
                  Console.Write("\n#{0:000}\tDFS Root: [{1}]\n", ++cnt, dfsNamespace);

                  DfsInfo dfsInfo = Host.GetDfsInfo(dfsNamespace);
                  
                  UnitTestConstants.Dump(dfsInfo, -21);


                  Console.Write("\n\tNumber of Storages: [{0}]\n", dfsInfo.StorageInfoCollection.Count());

                  foreach (DfsStorageInfo store in dfsInfo.StorageInfoCollection)
                     UnitTestConstants.Dump(store, -19);

                  Console.WriteLine();
               }
               catch (NetworkInformationException ex)
               {
                  Console.WriteLine("\n\tNetworkInformationException #1: [{0}]", ex.Message.Replace(Environment.NewLine, "  "));
               }
               catch (Exception ex)
               {
                  Console.WriteLine("\n\t(1) {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
               }
            }
            Console.Write("\n{0}", UnitTestConstants.Reporter());

            if (cnt == 0)
               Assert.Inconclusive("Nothing was enumerated.");
         }
         catch (NetworkInformationException ex)
         {
            Console.WriteLine("\n\tNetworkInformationException #2: [{0}]", ex.Message.Replace(Environment.NewLine, "  "));
         }
         catch (Exception ex)
         {
            Console.WriteLine("\n\t(2) {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
         }

         Console.WriteLine("\n\n\t{0}", UnitTestConstants.Reporter(true));

         if (noDomainConnection)
            Assert.Inconclusive("Test ignored because the computer is probably not connected to a domain.");
         
         if (cnt == 0)
            Assert.Inconclusive("Nothing was enumerated.");

         Console.WriteLine();
      }

      #endregion // DumpClassDfsInfo

      #region DumpOpenConnectionInfo

      private void DumpOpenConnectionInfo(string host)
      {
         Console.WriteLine("\n=== TEST ===");
         Console.WriteLine("\nNetwork.Host.EnumerateOpenResources() from host: [{0}]", host);
          
         UnitTestConstants.StopWatcher(true);
         foreach (OpenConnectionInfo connectionInfo in Host.EnumerateOpenConnections(host, "IPC$", false))
            UnitTestConstants.Dump(connectionInfo, -16);

         Console.WriteLine(UnitTestConstants.Reporter(true));
         Console.WriteLine();
      }
      
      #endregion // DumpClassOpenResourceInfo

      #region DumpClassOpenResourceInfo

      private void DumpClassOpenResourceInfo(string host, string share)
      {
         Console.WriteLine("\n=== TEST ===");
         string tempPath = Path.LocalToUnc(share);
         Console.WriteLine("\nNetwork.Host.EnumerateOpenResources() from host: [{0}]", tempPath);

         Directory.SetCurrentDirectory(tempPath);

         UnitTestConstants.StopWatcher(true);
         int cnt = 0;
         foreach (OpenResourceInfo openResource in Host.EnumerateOpenResources(host, null, null, false))
         {
            if (UnitTestConstants.Dump(openResource, -11))
            {
               Console.Write("\n");
               cnt++;
            }
         }

         Console.WriteLine(UnitTestConstants.Reporter());
         
         if (cnt == 0)
            Assert.Inconclusive("Nothing was enumerated.");

         Console.WriteLine();
      }
      
      #endregion // DumpClassOpenResourceInfo

      #region DumpClassShareInfo

      #endregion // DumpClassShareInfo

      #endregion // Unit Tests

      #region Unit Test Callers

      #region Filesystem

      #region Filesystem_Class_AlternateDataStreamInfo

      [TestMethod]
      public void Filesystem_Class_AlternateDataStreamInfo()
      {
         Console.WriteLine("Class Filesystem.AlternateDataStreamInfo()");

         DumpClassAlternateDataStreamInfo(true);
         DumpClassAlternateDataStreamInfo(false);
      }

      #endregion // Filesystem_Class_AlternateDataStreamInfo

      #region Filesystem_Class_ByHandleFileInfo

      [TestMethod]
      public void Filesystem_Class_ByHandleFileInfo()
      {
         Console.WriteLine("Class Filesystem.ByHandleFileInfo()");

         DumpClassByHandleFileInfo(true);
         DumpClassByHandleFileInfo(false);
      }

      #endregion // Filesystem_Class_ByHandleFileInfo

      #region Filesystem_Class_DeviceInfo

      [TestMethod]
      public void Filesystem_Class_DeviceInfo()
      {
         Console.WriteLine("Class Filesystem.DeviceInfo()");
         Console.WriteLine("\nMSDN Note: Beginning in Windows 8 and Windows Server 2012 functionality to access remote machines has been removed.");
         Console.WriteLine("You cannot access remote machines when running on these versions of Windows.\n");

         DumpClassDeviceInfo(UnitTestConstants.LocalHost);
      }

      #endregion // Filesystem_Class_Shell32Info

      #region Filesystem_Class_DirectoryInfo

      [TestMethod]
      public void Filesystem_Class_DirectoryInfo()
      {
         Console.WriteLine("Class Filesystem.DirectoryInfo()");

         DumpClassDirectoryInfo(true);
         DumpClassDirectoryInfo(false);
      }

      #endregion // Filesystem_Class_DirectoryInfo

      #region Filesystem_Class_DiskSpaceInfo

      [TestMethod]
      public void Filesystem_Class_DiskSpaceInfo()
      {
         Console.WriteLine("Class Filesystem.DiskSpaceInfo()");

         DumpClassDiskSpaceInfo(true);
         DumpClassDiskSpaceInfo(false);
      }

      #endregion // Filesystem_Class_DiskSpaceInfo

      #region Filesystem_Class_DriveInfo

      [TestMethod]
      public void Filesystem_Class_DriveInfo()
      {
         Console.WriteLine("Class Filesystem.DriveInfo()");

         DumpClassDriveInfo(true, UnitTestConstants.SysDrive);
         DumpClassDriveInfo(false, UnitTestConstants.SysDrive);
      }

      #endregion // Filesystem_Class_DriveInfo

      #region Filesystem_Class_FileInfo

      [TestMethod]
      public void Filesystem_Class_FileInfo()
      {
         Console.WriteLine("Class Filesystem.FileInfo()");

         DumpClassFileInfo(true);
         DumpClassFileInfo(false);
      }

      #endregion // Filesystem_Class_FileInfo
      
      #region Filesystem_Class_FileSystemEntryInfo

      [TestMethod]
      public void Filesystem_Class_FileSystemEntryInfo()
      {
         Console.WriteLine("Class Filesystem.FileSystemEntryInfo()");

         DumpClassFileSystemEntryInfo(true);
         DumpClassFileSystemEntryInfo(false);
      }

      #endregion // Filesystem_Class_FileSystemEntryInfo

      #region Filesystem_Class_Shell32Info

      [TestMethod]
      public void Filesystem_Class_Shell32Info()
      {
         Console.WriteLine("Class Filesystem.Shell32Info()");

         DumpClassShell32Info(true);
         DumpClassShell32Info(false);
      }

      #endregion // Filesystem_Class_Shell32Info

      #region Filesystem_Class_VolumeInfo

      [TestMethod]
      public void Filesystem_Class_VolumeInfo()
      {
         Console.WriteLine("Class Filesystem.VolumeInfo()");

         DumpClassVolumeInfo(true);
         DumpClassVolumeInfo(false);
      }

      #endregion // Filesystem_Class_VolumeInfo

      #endregion Filesystem

      #region Network

      #region Network_Class_DfsXxx

      [TestMethod]
      public void Network_Class_DfsXxx()
      {
         Console.WriteLine("Class Network.DfsInfo()");
         Console.WriteLine("Class Network.DfsStorageInfo()");

         DumpClassDfsInfo();
      }

      #endregion // Network_Class_DfsXxx

      #region Network_Class_OpenConnectionInfo

      [TestMethod]
      public void Network_Class_OpenConnectionInfo()
      {
         Console.WriteLine("Class Network.OpenConnectionInfo()");

         if (!UnitTestConstants.IsAdmin())
             Assert.Inconclusive();

         DumpOpenConnectionInfo(UnitTestConstants.LocalHost);
      }

      #endregion // Network_Class_OpenConnectionInfo

      #region Network_Class_OpenResourceInfo

      [TestMethod]
      public void Network_Class_OpenResourceInfo()
      {
         Console.WriteLine("Class Network.OpenResourceInfo()");

         if (!UnitTestConstants.IsAdmin())
             Assert.Inconclusive();

         DumpClassOpenResourceInfo(UnitTestConstants.LocalHost, UnitTestConstants.LocalHostShare);
      }

      #endregion // Network_Class_OpenResourceInfo

      #region Network_Class_ShareInfo

      [TestMethod]
      public void Network_Class_ShareInfo()
      {
         Console.WriteLine("Class Network.ShareInfo()");

         string host = UnitTestConstants.LocalHost;

         Console.WriteLine("\n=== TEST ===");
         Console.Write("\nNetwork.Host.EnumerateShares() from host: [{0}]\n", host);

         int cnt = 0;
         UnitTestConstants.StopWatcher(true);
         foreach (ShareInfo share in Host.EnumerateShares(host, true))
         {
            Console.WriteLine("\n\t#{0:000}\tShare: [{1}]", ++cnt, share);
            UnitTestConstants.Dump(share, -18);
         }

         Console.WriteLine("\n{0}", UnitTestConstants.Reporter(true));

         if (cnt == 0)
            Assert.Inconclusive("Nothing was enumerated.");
      }

      #endregion // Network_Class_ShareInfo

      #endregion // Network

      #region OperatingSystem

      #region Class_OperatingSystem

      [TestMethod]
      public void Class_OperatingSystem()
      {
         Console.WriteLine("Class Win32.OperatingSystem()\n");

         UnitTestConstants.StopWatcher(true);

         Console.WriteLine("VersionName          : [{0}]", OperatingSystem.VersionName);
         Console.WriteLine("OsVersion            : [{0}]", OperatingSystem.OSVersion);
         Console.WriteLine("ServicePackVersion   : [{0}]", OperatingSystem.ServicePackVersion);
         Console.WriteLine("IsServer             : [{0}]", OperatingSystem.IsServer);
         Console.WriteLine("IsWow64Process       : [{0}]", OperatingSystem.IsWow64Process);
         Console.WriteLine("ProcessorArchitecture: [{0}]", OperatingSystem.ProcessorArchitecture);

         Console.WriteLine("\nOperatingSystem.IsAtLeast()\n");

         Console.WriteLine("\tOS Earlier           : [{0}]", OperatingSystem.IsAtLeast(OperatingSystem.EnumOsName.Earlier));
         Console.WriteLine("\tWindows 2000         : [{0}]", OperatingSystem.IsAtLeast(OperatingSystem.EnumOsName.Windows2000));
         Console.WriteLine("\tWindows XP           : [{0}]", OperatingSystem.IsAtLeast(OperatingSystem.EnumOsName.WindowsXP));
         Console.WriteLine("\tWindows Vista        : [{0}]", OperatingSystem.IsAtLeast(OperatingSystem.EnumOsName.WindowsVista));
         Console.WriteLine("\tWindows 7            : [{0}]", OperatingSystem.IsAtLeast(OperatingSystem.EnumOsName.Windows7));
         Console.WriteLine("\tWindows 8            : [{0}]", OperatingSystem.IsAtLeast(OperatingSystem.EnumOsName.Windows8));
         Console.WriteLine("\tWindows 8.1          : [{0}]", OperatingSystem.IsAtLeast(OperatingSystem.EnumOsName.Windows81));
         Console.WriteLine("\tWindows 10           : [{0}]", OperatingSystem.IsAtLeast(OperatingSystem.EnumOsName.Windows10));

         Console.WriteLine("\tWindows Server 2003  : [{0}]", OperatingSystem.IsAtLeast(OperatingSystem.EnumOsName.WindowsServer2003));
         Console.WriteLine("\tWindows Server 2008  : [{0}]", OperatingSystem.IsAtLeast(OperatingSystem.EnumOsName.WindowsServer2008));
         Console.WriteLine("\tWindows Server 2008R2: [{0}]", OperatingSystem.IsAtLeast(OperatingSystem.EnumOsName.WindowsServer2008R2));
         Console.WriteLine("\tWindows Server 2012  : [{0}]", OperatingSystem.IsAtLeast(OperatingSystem.EnumOsName.WindowsServer2012));
         Console.WriteLine("\tWindows Server 2012R2: [{0}]", OperatingSystem.IsAtLeast(OperatingSystem.EnumOsName.WindowsServer2012R2));
         Console.WriteLine("\tWindows Server       : [{0}]", OperatingSystem.IsAtLeast(OperatingSystem.EnumOsName.WindowsServer));

         Console.WriteLine("\tOS Later             : [{0}]", OperatingSystem.IsAtLeast(OperatingSystem.EnumOsName.Later));

         Console.WriteLine();
         Console.WriteLine(UnitTestConstants.Reporter());
      }

      #endregion // Class_OperatingSystem

      #endregion // OperatingSystem

      #endregion Unit Test Callers
   }
}
