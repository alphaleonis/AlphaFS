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

namespace AlphaFS.UnitTest
{
   public partial class AlphaFS_VolumeTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void AlphaFS_Volume_GetVolumeInfo_GlobalRootPath_Success()
      {
         // From an elevated prompt, type: vssadmin list shadows
         // and pick any Shadow Copy Volume: \\?\GLOBALROOT\Device\HarddiskVolume...

         var globalRootPath = @"\\?\GLOBALROOT\Device\HarddiskVolumeShadowCopy1";

         try
         {
            var volume = Alphaleonis.Win32.Filesystem.Volume.GetVolumeInfo(globalRootPath);
            Console.WriteLine("Volume full path: " + volume.FullPath);

            Assert.AreEqual(globalRootPath + @"\", volume.FullPath);
         }
         catch (Exception)
         {
            Assert.Inconclusive("Volume not found: " + globalRootPath);
         }
      }


      [TestMethod]
      public void AlphaFS_Volume_GetVolumeInfo_SystemDrivePath_Success()
      {
         var volume = Alphaleonis.Win32.Filesystem.Volume.GetVolumeInfo(UnitTestConstants.SysDrive);
         Console.WriteLine("Volume full path: " + volume.FullPath);

         Assert.AreEqual(UnitTestConstants.SysDrive + @"\", volume.FullPath);
      }
   }
}
