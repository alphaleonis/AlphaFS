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
   public partial class AlphaFS_VolumeTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void AlphaFS_Volume_EnumerateVolumes_And_EnumerateVolumePathNames_Local_Success()
      {
         UnitTestConstants.PrintUnitTestHeader(false);


         Console.WriteLine("\nShould give the same (or more) enumeration as \"mountvol.exe\"\n");


         var volumeCount = 0;
         var pathNamesCount = 0;

         var volumes = Alphaleonis.Win32.Filesystem.Volume.EnumerateVolumes().ToArray();

         foreach (var volume in volumes)
         {
            Console.WriteLine("#{0:000}\tVolume: [{1}]", ++volumeCount, volume);
            Console.WriteLine();


            Assert.IsTrue(volumes.Any(vol => vol.StartsWith(Alphaleonis.Win32.Filesystem.Path.VolumePrefix + "{") &&

                                             vol.EndsWith("}" + Alphaleonis.Win32.Filesystem.Path.DirectorySeparator)), "Volume path name is not valid, but it is expected.");
            

            var pathNames = Alphaleonis.Win32.Filesystem.Volume.EnumerateVolumePathNames(volume).ToArray();

            foreach (var displayName in pathNames)
            {
               Console.WriteLine("\t\tEnumerateVolumePathNames: Volume points to logcal drive: [{0}]", displayName);
               Console.WriteLine();


               // Volumes don't always have drive letters.

               if (!string.IsNullOrWhiteSpace(displayName))
                  Assert.IsTrue(char.IsLetter(displayName[0]) && displayName.EndsWith(Alphaleonis.Win32.Filesystem.Path.VolumeSeparator + Alphaleonis.Win32.Filesystem.Path.DirectorySeparator));


               pathNamesCount++;
            }


            Console.WriteLine();


            if (pathNamesCount == 0)
               Assert.Inconclusive("No path names enumerated, but it is expected.");
         }


         if (volumeCount == 0)
            Assert.Inconclusive("No volumes enumerated, but it is expected.");
      }
   }
}
