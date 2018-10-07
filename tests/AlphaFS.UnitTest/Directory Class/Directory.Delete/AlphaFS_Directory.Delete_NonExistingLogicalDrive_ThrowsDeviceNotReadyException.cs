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
   public partial class DeleteTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>

      
      [TestMethod]
      public void AlphaFS_Directory_Delete_NonExistingLogicalDrive_ThrowsDeviceNotReadyException_LocalAndNetwork_Success()
      {
         AlphaFS_Directory_Delete_NonExistingLogicalDrive_ThrowsDeviceNotReadyException(false);
         AlphaFS_Directory_Delete_NonExistingLogicalDrive_ThrowsDeviceNotReadyException(true);
      }


      private void AlphaFS_Directory_Delete_NonExistingLogicalDrive_ThrowsDeviceNotReadyException(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         var nonExistingDriveLetter = Alphaleonis.Win32.Filesystem.DriveInfo.GetFreeDriveLetter();

         var folder = nonExistingDriveLetter + @":\NonExisting Source Folder";
         if (isNetwork)
            folder = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(folder);

         Console.WriteLine("Input Directory Path: [{0}]", folder);


         if (isNetwork)
         {
            UnitTestAssert.ThrowsException<System.IO.IOException>(() => System.IO.Directory.Delete(folder));

            UnitTestAssert.ThrowsException<Alphaleonis.Win32.Filesystem.DeviceNotReadyException>(() => Alphaleonis.Win32.Filesystem.Directory.Delete(folder));
         }

         else
         {
            UnitTestAssert.ThrowsException<System.IO.DirectoryNotFoundException>(() => System.IO.Directory.Delete(folder));

            UnitTestAssert.ThrowsException<Alphaleonis.Win32.Filesystem.DeviceNotReadyException>(() => Alphaleonis.Win32.Filesystem.Directory.Delete(folder));
         }
         
         Console.WriteLine();
      }
   }
}
