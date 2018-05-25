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
   public partial class Directory_CreateDirectoryTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>
      

      [TestMethod]
      public void Directory_CreateDirectory_ThrowIOExceptionOrDeviceNotReadyException_NonExistingDriveLetter_Network_Success()
      {
         Directory_CreateDirectory_ThrowIOExceptionOrDeviceNotReadyException_NonExistingDriveLetter(true);
      }




      private void Directory_CreateDirectory_ThrowIOExceptionOrDeviceNotReadyException_NonExistingDriveLetter(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         var folder = Alphaleonis.Win32.Filesystem.DriveInfo.GetFreeDriveLetter() + @":\NonExistingDriveLetter";
         if (isNetwork)
            folder = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(folder);

         Console.WriteLine("\nInput Directory Path: [{0}]", folder);


         var gotException = false;
         try
         {
            Alphaleonis.Win32.Filesystem.Directory.CreateDirectory(folder);
         }
         catch (Exception ex)
         {
            var exType = ex.GetType();

            // Local: DirectoryNotFoundException.
            // UNC: IOException or DeviceNotReadyException.
            // The latter occurs when a removable drive is already removed but there's still a cached reference.

            gotException = exType == typeof(System.IO.IOException);

            if (!gotException && isNetwork)
               gotException = exType == typeof(Alphaleonis.Win32.Filesystem.DeviceNotReadyException);

            Console.WriteLine("\n\tCaught {0} Exception: [{1}] {2}", gotException ? "EXPECTED" : "UNEXPECTED", exType.Name, ex.Message);
         }

         Assert.IsTrue(gotException, "The exception is not caught, but is expected to.");

         Console.WriteLine();
      }
   }
}
