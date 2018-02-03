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
using System.Globalization;

namespace AlphaFS.UnitTest
{
   public partial class AlphaFS_VolumeTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void AlphaFS_Volume_DefineDosDevice_Local_Success()
      {
         UnitTestConstants.PrintUnitTestHeader(false);
         Console.WriteLine();

         if (!UnitTestConstants.IsAdmin())
            Assert.Inconclusive();


         var TempFolder = UnitTestConstants.TempFolder;


         #region Regular Drive Mapping

         var drive = string.Format(CultureInfo.InvariantCulture, "{0}{1}{2}", Alphaleonis.Win32.Filesystem.DriveInfo.GetFreeDriveLetter(), Alphaleonis.Win32.Filesystem.Path.VolumeSeparatorChar, Alphaleonis.Win32.Filesystem.Path.DirectorySeparatorChar);

         // Create Regular drive mapping.
         var actionIsTrue = false;
         try
         {
            Alphaleonis.Win32.Filesystem.Volume.DefineDosDevice(drive, TempFolder);
            actionIsTrue = true;
         }
         catch
         {
         }

         Console.WriteLine("Created Regular drive mapping (Should be True): [{0}]\nDrive letter: [{1}] now points to: [{2}]", actionIsTrue, drive, TempFolder);

         Assert.IsTrue(actionIsTrue, "Regular drive mapping should have been created.");


         var di = new Alphaleonis.Win32.Filesystem.DriveInfo(drive);
         var diSysIo = new System.IO.DriveInfo(drive);

         try
         {
            Assert.IsTrue(UnitTestConstants.Dump(di, -21));

            Assert.AreEqual(diSysIo.IsReady, di.IsReady);

         }
         finally
         {
            // Remove Regular drive mapping.
            actionIsTrue = false;
            try
            {
               Alphaleonis.Win32.Filesystem.Volume.DeleteDosDevice(drive);
               actionIsTrue = true;
            }
            catch
            {
            }


            Console.WriteLine();
            Console.WriteLine("Removed Regular drive mapping (Should be True): [{0}]\nDrive letter: [{1}] has been set free.", actionIsTrue, drive);
            Console.WriteLine();

            Assert.IsTrue(actionIsTrue, "Regular drive mapping should have been removed.");

            Assert.IsFalse(System.IO.Directory.Exists(drive), "Drive letter should not be visible.");
         }

         #endregion // Regular Drive Mapping


         #region Symbolic Link Drive Mapping

         try
         {
            drive = string.Format(CultureInfo.InvariantCulture, "{0}{1}{2}", Alphaleonis.Win32.Filesystem.DriveInfo.GetFreeDriveLetter(true), Alphaleonis.Win32.Filesystem.Path.VolumeSeparatorChar, Alphaleonis.Win32.Filesystem.Path.DirectorySeparatorChar);
            UnitTestConstants.StopWatcher(true);

            // Create Symbolic Link.
            var createSymbolicLink = false;
            try
            {
               Alphaleonis.Win32.Filesystem.Volume.DefineDosDevice(drive, TempFolder, Alphaleonis.Win32.Filesystem.DosDeviceAttributes.RawTargetPath);
               createSymbolicLink = true;
            }
            catch
            {
            }

            Console.WriteLine("\n\nCreated Symbolic link mapping (Should be True): [{0}]\nDrive letter: [{1}] now points to: [{2}]", createSymbolicLink, drive, TempFolder);

            Assert.IsTrue(createSymbolicLink);


            di = new Alphaleonis.Win32.Filesystem.DriveInfo(drive);
            Assert.IsTrue(UnitTestConstants.Dump(di, -21));

            // The link is created in the NT Device Name namespace and thus not visibile in Explorer.

            // Remove Symbolic Link, no exact match: fail.

            var removeSymbolicLink = false;
            try
            {
               Alphaleonis.Win32.Filesystem.Volume.DeleteDosDevice(drive, "NonExistingFolder", true);
               removeSymbolicLink = true;
            }
            catch
            {
            }

            Console.WriteLine("\n\nRemoved Symbolic link mapping (Should be False): [{0}]\nDrive letter: [{1}] has NOT been set free.\tNo exactMatch MS-DOS device name found.", removeSymbolicLink, drive);

            Assert.IsFalse(removeSymbolicLink);
         }
         finally
         {
            // Remove Symbolic Link, exact match: success.
            var removeSymbolicLink = false;
            try
            {
               Alphaleonis.Win32.Filesystem.Volume.DeleteDosDevice(drive, TempFolder, true);
               removeSymbolicLink = true;
            }
            catch
            {
            }

            Console.WriteLine("\n\nRemoved Symbolic link mapping (Should be True): [{0}]\nDrive letter: [{1}] has been set free.\tFound exactMatch MS-DOS device name.", removeSymbolicLink, drive);

            Assert.IsTrue(removeSymbolicLink);

            Assert.IsFalse(System.IO.Directory.Exists(drive));
         }

         #endregion // Symbolic Link Drive Mapping
      }
   }
}
