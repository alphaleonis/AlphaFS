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
using System.Collections.Generic;
using System.Linq;

namespace AlphaFS.UnitTest
{
   public partial class AlphaFS_VolumeTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void AlphaFS_Volume_QueryAllDosDevices()
      {
         UnitTestConstants.PrintUnitTestHeader(false);
         Console.WriteLine();


         IEnumerable<string> query = Alphaleonis.Win32.Filesystem.Volume.QueryAllDosDevices("sort").ToArray();

         Console.WriteLine("Retrieved: [{0}] items.\n", query.Count());


         var cnt = 0;
         foreach (var dosDev in query)
         {
            Console.WriteLine("#{0:000}\t{1}", ++cnt, dosDev);
         }

         if (cnt == 0)
            Assert.Inconclusive("Nothing is enumerated, but it is expected.");

         Assert.IsTrue(query.Any());
      }


      [TestMethod]
      public void AlphaFS_Volume_QueryDosDevice_Local_Success()
      {
         UnitTestConstants.PrintUnitTestHeader(false);
         Console.WriteLine();


         #region Filtered, UnSorted List Drives

         var filter = "hard";
         
         IEnumerable<string> query = Alphaleonis.Win32.Filesystem.Volume.QueryDosDevice(filter).ToArray();

         Console.WriteLine("\tQueryDosDevice(\"" + filter + "\")\n\tRetrieved Filtered, UnSorted list: [{0}]\n\tList .Count(): [{1}]\n", query.Any(), query.Count());

         var cnt = 0;

         foreach (var dosDev in query)
            Console.WriteLine("\t\t#{0:000}\tMS-Dos Name: [{1}]", ++cnt, dosDev);

         Assert.IsTrue(query.Any() && cnt > 0);

         #endregion // Filtered, UnSorted List Drives


         #region Filtered, Sorted List Volumes

         filter = "volume";

         cnt = 0;

         query = Alphaleonis.Win32.Filesystem.Volume.QueryDosDevice(filter, "sort").ToArray();

         Console.WriteLine("\n\n\tQueryDosDevice(\"" + filter + "\")\n\tRetrieved Filtered, Sorted list: [{0}]\n\tList .Count(): [{1}]\n", query.Any(), query.Count());

         foreach (var dosDev in query)
            Console.WriteLine("\t\t#{0:000}\tMS-Dos Name: [{1}]", ++cnt, dosDev);

         Assert.IsTrue(query.Any() && cnt > 0);

         #endregion // Filtered, Sorted List Volumes


         #region Get from Logical Drives

         Console.WriteLine("\n\n\tQueryDosDevice (from Logical Drives)\n");

         cnt = 0;

         foreach (var existingDriveLetter in System.IO.Directory.GetLogicalDrives())
         {
            foreach (var dosDev in Alphaleonis.Win32.Filesystem.Volume.QueryDosDevice(existingDriveLetter))
            {
               var hasLogicalDrive = !Alphaleonis.Utils.IsNullOrWhiteSpace(dosDev);

               Console.WriteLine("\t\t#{0:000}\tLogical Drive [{1}] MS-Dos Name: [{2}]", ++cnt, existingDriveLetter, dosDev);

               Assert.IsTrue(hasLogicalDrive);
            }
         }

         Assert.IsTrue(cnt > 0, "No entries read.");

         #endregion // Get from Logical Drives


         #region Filtered, Sorted PhysicalDrive

         filter = "PhysicalDrive";

         cnt = 0;

         query = Alphaleonis.Win32.Filesystem.Volume.QueryDosDevice(filter, "sort").ToArray();

         Console.WriteLine("\n\n\tQueryDosDevice(\"" + filter + "\")\n\tRetrieved Filtered, Sorted list: [{0}]\n\tList .Count(): [{1}]\n", query.Any(), query.Count());

         foreach (var dosDev in query)
            Console.WriteLine("\t\t#{0:000}\tMS-Dos Name: [{1}]", ++cnt, dosDev);

         Assert.IsTrue(query.Any() && cnt > 0);

         #endregion // Filtered, Sorted PhysicalDrive


         #region Filtered, Sorted CDRom

         filter = "CdRom";

         cnt = 0;

         UnitTestConstants.StopWatcher(true);

         query = Alphaleonis.Win32.Filesystem.Volume.QueryDosDevice(filter, "sort").ToArray();

         Console.WriteLine("\n\n\tQueryDosDevice(\"" + filter + "\")\n\tRetrieved Filtered, Sorted list: [{0}]\n\tList .Count(): [{1}]\n", query.Any(), query.Count());

         foreach (var dosDev in query)
            Console.WriteLine("\t\t#{0:000}\tMS-Dos Name: [{1}]", ++cnt, dosDev);

         #endregion // Filtered, Sorted CDRom
      }
   }
}
