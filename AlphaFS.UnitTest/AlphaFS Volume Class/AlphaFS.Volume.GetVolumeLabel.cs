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

namespace AlphaFS.UnitTest
{
   public partial class AlphaFS_VolumeTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void AlphaFS_Volume_GetVolumeLabel()
      {
         UnitTestConstants.PrintUnitTestHeader(false);
         Console.WriteLine();


         int cnt;
         bool looped;
         string label;


         // Logical Drives

         Console.WriteLine("Logical Drives\n");

         cnt = 0;
         looped = false;

         foreach (var drive in System.IO.Directory.GetLogicalDrives())
         {
            label = Alphaleonis.Win32.Filesystem.Volume.GetVolumeLabel(drive);

            Console.WriteLine("\t#{0:000}\tLogical Drive: [{1}]\t\tLabel: [{2}]", ++cnt, drive, label);

            looped = true;
         }

         Assert.IsTrue(looped && cnt > 0);



         
         // Volumes

         Console.WriteLine("\nVolumes\n");

         cnt = 0;
         looped = false;
         
         foreach (var volume in Alphaleonis.Win32.Filesystem.Volume.EnumerateVolumes())
         {
            label = Alphaleonis.Win32.Filesystem.Volume.GetVolumeLabel(volume);

            Console.WriteLine("\t#{0:000}\tVolume: [{1}]\t\tLabel: [{2}]", ++cnt, volume, label);

            looped = true;
         }

         Assert.IsTrue(looped && cnt > 0);

         
         
         
         // DosDevices

         Console.WriteLine("\nDosDevices\n");

         cnt = 0;
         looped = false;
         var devices = new List<string>(new[] {"volume", "hard", "physical", "storage"});


         foreach (var dd in devices)
         {
            cnt = 0;

            foreach (var dosDevice in Alphaleonis.Win32.Filesystem.Volume.QueryDosDevice(dd))
            {
               label = Alphaleonis.Win32.Filesystem.Volume.GetVolumeLabel(dosDevice);

               Console.WriteLine("\t#{0:000}\tDosDevice: [{1}]\t\tLabel: [{2}]", ++cnt, dosDevice, label);

               looped = true;
            }

            Console.WriteLine();
         }


         Assert.IsTrue(looped && cnt > 0);
      }
   }
}
