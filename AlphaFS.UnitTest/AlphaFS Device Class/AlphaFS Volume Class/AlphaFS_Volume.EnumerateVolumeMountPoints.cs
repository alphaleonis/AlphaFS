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
using System.Linq;

namespace AlphaFS.UnitTest
{
   public partial class EnumerationTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void AlphaFS_Volume_EnumerateVolumeMountPoints_Local_Success()
      {
         UnitTestAssert.IsElevatedProcess();
         UnitTestConstants.PrintUnitTestHeader(false);
         
         var cnt = 0;
         Console.WriteLine("Logical Drives\n");

         // Get Logical Drives from UnitTestConstants.Local Host, .IsReady Drives only.
         foreach (var drive in Alphaleonis.Win32.Filesystem.Directory.GetLogicalDrives(false, true))
         {
            try
            {
               // Logical Drives --> Volumes --> Volume Mount Points.
               var uniqueVolName = Alphaleonis.Win32.Filesystem.Volume.GetUniqueVolumeNameForPath(drive);

               if (!Alphaleonis.Utils.IsNullOrWhiteSpace(uniqueVolName) && !uniqueVolName.Equals(drive, StringComparison.OrdinalIgnoreCase))
               {
                  foreach (var mountPoint in Alphaleonis.Win32.Filesystem.Volume.EnumerateVolumeMountPoints(uniqueVolName).Where(mp => !Alphaleonis.Utils.IsNullOrWhiteSpace(mp)))
                  {
                     string guid = null;
                     try { guid = Alphaleonis.Win32.Filesystem.Volume.GetVolumeGuid(System.IO.Path.Combine(drive, mountPoint)); }
                     catch (Exception ex)
                     {
                        Console.WriteLine("\n\tCaught (UNEXPECTED #1) {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
                     }

                     Console.WriteLine("\t#{0:000}\tLogical Drive: [{1}]\tGUID: [{2}]\n\t\tDestination  : [{3}]\n", ++cnt, drive, guid ?? "null", mountPoint);
                  }
               }
            }
            catch (Exception ex)
            {
               Console.WriteLine("\n\tCaught (UNEXPECTED #2) {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
            }
         }


         if (cnt == 0)
            UnitTestAssert.InconclusiveBecauseResourcesAreUnavailable();
      }
   }
}
