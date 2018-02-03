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
using System.Reflection;

namespace AlphaFS.UnitTest
{
   public partial class AlphaFS_VolumeTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void AlphaFS_Volume_SetVolumeLabel_Local_Success()
      {
         if (!UnitTestConstants.IsAdmin())
            Assert.Inconclusive();

         UnitTestConstants.PrintUnitTestHeader(false);
         Console.WriteLine();


         var drive = UnitTestConstants.SysDrive;

         const string newLabel = "ÂĽpĥæƑŞ ŠëtVőlümèĻāßƩl Ťest";

         const string template = "System Drive: [{0}]\tCurrent Label: [{1}]";




         // Get label.

         var originalLabel = new System.IO.DriveInfo(drive).VolumeLabel;

         Console.WriteLine(template, drive, originalLabel);

         Assert.AreEqual(originalLabel, Alphaleonis.Win32.Filesystem.Volume.GetVolumeLabel(drive));


         
         
         // Set label.

         Console.WriteLine("\nSet label.");

         Alphaleonis.Win32.Filesystem.Volume.SetVolumeLabel(drive, newLabel);

         Console.WriteLine(template, drive, newLabel);

         Assert.AreEqual(newLabel, new System.IO.DriveInfo(drive).VolumeLabel);

         Assert.AreEqual(newLabel, Alphaleonis.Win32.Filesystem.Volume.GetVolumeLabel(drive));




         // Remove label.

         Console.WriteLine("\nRemove label.");

         Alphaleonis.Win32.Filesystem.Volume.DeleteVolumeLabel(drive);

         var currentLabel = Alphaleonis.Win32.Filesystem.Volume.GetVolumeLabel(drive);

         Console.WriteLine(template, drive, currentLabel);

         Assert.AreEqual(new System.IO.DriveInfo(drive).VolumeLabel, Alphaleonis.Win32.Filesystem.Volume.GetVolumeLabel(drive));




         // Restore label.

         Console.WriteLine("\nRestore label.");

         Alphaleonis.Win32.Filesystem.Volume.SetVolumeLabel(drive, originalLabel);

         currentLabel = Alphaleonis.Win32.Filesystem.Volume.GetVolumeLabel(drive);

         Console.WriteLine(template, drive, currentLabel);

         Assert.AreEqual(currentLabel, new System.IO.DriveInfo(drive).VolumeLabel);

         Assert.AreEqual(currentLabel, Alphaleonis.Win32.Filesystem.Volume.GetVolumeLabel(drive));
      }
   }
}
