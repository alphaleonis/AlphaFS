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
      public void AlphaFS_Volume_DefineDosDevice_RegularDriveMapping_Local_Success()
      {
         AlphaFS_Volume_DefineDosDevice_RegularDriveMapping(false);
         AlphaFS_Volume_DefineDosDevice_RegularDriveMapping(true);
      }


      private void AlphaFS_Volume_DefineDosDevice_RegularDriveMapping(bool isNetwork)
      {
         using (var tempRoot = new TemporaryDirectory(isNetwork))
         {
            var folder = tempRoot.CreateDirectory();
            var drive = Alphaleonis.Win32.Filesystem.DriveInfo.GetFreeDriveLetter() + ":";

            Assert.IsFalse(System.IO.Directory.Exists(drive), "The drive exists, but it is expected not to.");


            try
            {
               Alphaleonis.Win32.Filesystem.Volume.DefineDosDevice(drive, folder.FullName);

               Assert.IsTrue(System.IO.Directory.Exists(drive), "The drive does not exists, but it is expected to.");


               var dirInfoSysIO = new System.IO.DriveInfo(drive);

               var dirInfoAlphaFS = new Alphaleonis.Win32.Filesystem.DriveInfo(drive);
               
               UnitTestConstants.Dump(dirInfoSysIO);
               UnitTestConstants.Dump(dirInfoAlphaFS);


               Assert.AreEqual(dirInfoSysIO.Name, dirInfoAlphaFS.Name);

               Assert.AreEqual(dirInfoSysIO.DriveType, dirInfoAlphaFS.DriveType);
               
               Assert.AreEqual(dirInfoSysIO.TotalSize, dirInfoAlphaFS.TotalSize);
            }
            finally
            {
               Alphaleonis.Win32.Filesystem.Volume.DeleteDosDevice(drive);

               Assert.IsFalse(System.IO.Directory.Exists(drive), "The drive exists, but it is expected not to.");
            }
         }

         Console.WriteLine();
      }
   }
}
