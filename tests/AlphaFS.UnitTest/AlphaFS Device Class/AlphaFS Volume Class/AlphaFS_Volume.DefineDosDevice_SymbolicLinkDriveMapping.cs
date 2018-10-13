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
using System.Globalization;

namespace AlphaFS.UnitTest
{
   public partial class AlphaFS_VolumeTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void AlphaFS_Volume_DefineDosDevice_SymbolicLinkDriveMapping_Local_Success()
      {
         AlphaFS_Volume_DefineDosDevice_SymbolicLinkDriveMapping(false);
         AlphaFS_Volume_DefineDosDevice_SymbolicLinkDriveMapping(true);
      }


      private void AlphaFS_Volume_DefineDosDevice_SymbolicLinkDriveMapping(bool isNetwork)
      {
         using (var tempRoot = new TemporaryDirectory(isNetwork))
         {
            var folder = tempRoot.CreateDirectory();
            var drive = string.Format(CultureInfo.InvariantCulture, @"{0}:\", Alphaleonis.Win32.Filesystem.DriveInfo.GetFreeDriveLetter(true));

            Assert.IsFalse(System.IO.Directory.Exists(drive), "The drive exists, but it is expected not to.");


            try
            {
               Alphaleonis.Win32.Filesystem.Volume.DefineDosDevice(drive, folder.FullName, Alphaleonis.Win32.Filesystem.DosDeviceAttributes.RawTargetPath);


               // Remove Symbolic Link, no exact match: fail.
               
               UnitTestAssert.ThrowsException<System.IO.FileNotFoundException>(() => Alphaleonis.Win32.Filesystem.Volume.DeleteDosDevice(drive, folder.FullName));
            }
            finally
            {
               // Remove Symbolic Link, exact match: success.

               Alphaleonis.Win32.Filesystem.Volume.DeleteDosDevice(drive, folder.FullName, true);

               Assert.IsFalse(System.IO.Directory.Exists(drive), "The drive exists, but it is expected not to.");
            }
         }


         Console.WriteLine();
      }
   }
}
