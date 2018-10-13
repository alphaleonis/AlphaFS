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
      public void File_Delete_NonExistingDriveLetter_ThrowsIOExceptionOrDeviceNotReadyException_Network_Success()
      {
         File_Delete_NonExistingDriveLetter_ThrowsIOExceptionOrDeviceNotReadyException(true);
      }


      private void File_Delete_NonExistingDriveLetter_ThrowsIOExceptionOrDeviceNotReadyException(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         var folder = Alphaleonis.Win32.Filesystem.DriveInfo.GetFreeDriveLetter() + @":\NonExistingDriveLetter";
         if (isNetwork)
            folder = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(folder);

         Console.WriteLine("Input File Path: [{0}]", folder);

         UnitTestAssert.ThrowsException<System.IO.IOException>(() => System.IO.File.Delete(folder));

         
         // Local: IOException.
         // UNC: IOException or DeviceNotReadyException.
         // The latter occurs when a removable drive is already removed but there's still a cached reference.

         var caught = UnitTestAssert.TestException<System.IO.IOException>(() => Alphaleonis.Win32.Filesystem.File.Delete(folder));

         if (!caught)
            caught = UnitTestAssert.TestException<Alphaleonis.Win32.Filesystem.DeviceNotReadyException>(() => Alphaleonis.Win32.Filesystem.File.Delete(folder));


         Assert.IsTrue(caught);

         Console.WriteLine();
      }
   }
}
