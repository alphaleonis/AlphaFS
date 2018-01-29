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
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AlphaFS.UnitTest
{
   public partial class DriveInfoTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>

      
      [TestMethod]
      public void DriveInfo_GetDrives_Local_Success()
      {
         UnitTestConstants.PrintUnitTestHeader(false);
      
         
         var drives = Alphaleonis.Win32.Filesystem.DriveInfo.GetDrives().ToList();

         foreach (var drive in drives)
         {
            Console.WriteLine("\nLogical Drive: [{0}]", drive.Name);

            UnitTestConstants.Dump(drive, -21);


            if (null != drive.DiskSpaceInfo)
               UnitTestConstants.Dump(drive.DiskSpaceInfo, -26, true);


            Console.WriteLine();
         }


         Assert.IsTrue(drives.Count > 0);

         Assert.AreEqual(drives[0].Name[0], UnitTestConstants.SysDrive[0]);
      }
   }
}
