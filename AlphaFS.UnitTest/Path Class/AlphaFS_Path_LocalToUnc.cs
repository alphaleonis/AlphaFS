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
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AlphaFS.UnitTest
{
   /// <summary>This is a test class for Path and is intended to contain all Path Unit Tests.</summary>
   public partial class PathTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>

      [TestMethod]
      public void AlphaFS_Path_LocalToUnc_LocalSystemDrive_Success()
      {
         Path_LocalToUnc();
      }


      [TestMethod]
      public void AlphaFS_Path_LocalToUnc_MappedDrive_Success()
      {
         Path_LocalToUnc_MappedDrive();
      }


      [TestMethod]
      public void AlphaFS_Path_LocalToUnc_MappedDriveWithSubFolder_Success()
      {
         Path_LocalToUnc_MappedDriveWithSubFolder();
      }




      private void Path_LocalToUnc()
      {
         UnitTestConstants.PrintUnitTestHeader(false);
         Console.WriteLine();


         var sysDrive = UnitTestConstants.SysDrive;
         var sysRoot = UnitTestConstants.SysRoot;
         var backslash = Alphaleonis.Win32.Filesystem.Path.DirectorySeparator;
         var hostName = Environment.MachineName + backslash + sysDrive.Replace(":", "$");
         var uncPrefix = Alphaleonis.Win32.Filesystem.Path.UncPrefix;
         var uncLongPrefix = Alphaleonis.Win32.Filesystem.Path.LongPathUncPrefix;
         

         string[] localToUncPaths =
         {
            sysDrive, 
            uncPrefix + hostName,
            uncLongPrefix + hostName,

            sysDrive + backslash,
            uncPrefix + hostName + backslash,
            uncLongPrefix + hostName + backslash,

            sysRoot,
            uncPrefix + hostName + backslash + System.IO.Path.GetFileName(sysRoot),
            uncLongPrefix + hostName + backslash + System.IO.Path.GetFileName(sysRoot),

            sysRoot + backslash + "TempFolder" + backslash,
            uncPrefix + hostName + backslash + System.IO.Path.GetFileName(sysRoot) + backslash + "TempFolder" + backslash,
            uncLongPrefix + hostName + backslash + System.IO.Path.GetFileName(sysRoot) + backslash + "TempFolder" + backslash
         };


         var totalPaths = localToUncPaths.Length;
         
         for (var i = 0; i < totalPaths; i += 3)
         {
            var localPath = localToUncPaths[0 + i];
            var shouldBeUncPath = localToUncPaths[1 + i];
            var shouldBeUncLongPath = localToUncPaths[2 + i];

            var uncPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(localPath);
            var uncLongPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(localPath, true);

            Console.WriteLine("\tLocal Path   : [{0}]", localPath);
            Console.WriteLine("\tUNC Path     : [{0}]", uncPath);
            Console.WriteLine("\tUNC Long Path: [{0}]", uncLongPath);


            Assert.AreEqual(shouldBeUncPath, uncPath, "The UNC paths do not match, but are expected to.");
            Assert.AreEqual(shouldBeUncLongPath, uncLongPath, "The UNC paths do not match, but are expected to.");


            Console.WriteLine();
         }
      }


      private void Path_LocalToUnc_MappedDrive()
      {
         UnitTestConstants.PrintUnitTestHeader(false);
         Console.WriteLine();


         var backslash = Alphaleonis.Win32.Filesystem.Path.DirectorySeparator;
         var hostName = Alphaleonis.Win32.Network.Host.GetUncName() + backslash;
         var localTempFolder = UnitTestConstants.TempFolder;
         var netTempFolder = localTempFolder.Replace(":", "$");


         string[] localToUncPaths =
         {
            localTempFolder,
            hostName + netTempFolder,
            Alphaleonis.Win32.Filesystem.Path.GetLongPath(hostName + netTempFolder)
         };


         using (var connection = new Alphaleonis.Win32.Network.DriveConnection(localToUncPaths[1]))
         {
            var driveLetter = connection.LocalName;
            var uncPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(driveLetter);
            var uncLongPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(driveLetter, true);


            Console.WriteLine("\tMapped letter: [{0}] to: [{1}]", driveLetter, connection.Share);
            Console.WriteLine();

            Console.WriteLine("\tUNC Path     : [{0}]", uncPath);
            Console.WriteLine("\tUNC Long Path: [{0}]", uncLongPath);
            Console.WriteLine();


            Assert.AreEqual(localToUncPaths[1], uncPath);
            Assert.AreEqual(localToUncPaths[2], uncLongPath);
         }
      }


      private void Path_LocalToUnc_MappedDriveWithSubFolder()
      {
         UnitTestConstants.PrintUnitTestHeader(false);
         Console.WriteLine();


         var backslash = Alphaleonis.Win32.Filesystem.Path.DirectorySeparator;
         var hostName = Alphaleonis.Win32.Network.Host.GetUncName() + backslash;
         var localTempFolder = UnitTestConstants.TempFolder;
         var netTempFolder = localTempFolder.Replace(":", "$");
         var subFolderName = MethodBase.GetCurrentMethod().Name;
         var subFolderPath = hostName + netTempFolder + backslash + subFolderName;


         string[] localToUncPaths =
         {
            localTempFolder,
            hostName + netTempFolder,
            subFolderPath,
            Alphaleonis.Win32.Filesystem.Path.GetLongPath(subFolderPath)
         };


         using (var connection = new Alphaleonis.Win32.Network.DriveConnection(localToUncPaths[1]))
         {
            var driveLetter = connection.LocalName;
            var subFolder = driveLetter + backslash + subFolderName;
            
            var uncPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(subFolder);
            var uncLongPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(subFolder, true);


            Console.WriteLine("\tMapped letter: [{0}] to: [{1}]", driveLetter, connection.Share);
            Console.WriteLine();

            Console.WriteLine("\tSub Folder Path         : [{0}]", subFolder);
            Console.WriteLine("\tUNC Sub Folder Path     : [{0}]", uncPath);
            Console.WriteLine("\tUNC Sub Folder Long Path: [{0}]", uncLongPath);
            Console.WriteLine();


            Assert.AreEqual(localToUncPaths[2], uncPath);
            Assert.AreEqual(localToUncPaths[3], uncLongPath);
         }
      }
   }
}
