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
      public void AlphaFS_Volume_SetVolumeMountPoint_And_DeleteVolumeMountPoint_Local_Success()
      {
         UnitTestAssert.IsElevatedProcess();

         using (var tempRoot = new TemporaryDirectory())
         {
            var folder = tempRoot.CreateDirectory();

            Console.WriteLine("Input Directory Path: [{0}]", folder.FullName);

            var guid = Alphaleonis.Win32.Filesystem.Volume.GetVolumeGuid(UnitTestConstants.SysDrive);
            

            Console.WriteLine("\nCreate mount point.");

            Alphaleonis.Win32.Filesystem.Volume.SetVolumeMountPoint(folder.FullName, guid);


            Assert.IsTrue(Alphaleonis.Win32.Filesystem.Volume.EnumerateVolumeMountPoints(guid).Any(mountPoint => folder.FullName.EndsWith(mountPoint.TrimEnd('\\'))));




            var lvi = Alphaleonis.Win32.Filesystem.Directory.GetLinkTargetInfo(folder.FullName);

            UnitTestConstants.Dump(lvi);

            Assert.IsTrue(lvi.SubstituteName.StartsWith(Alphaleonis.Win32.Filesystem.Path.NonInterpretedPathPrefix));

            Assert.IsTrue(lvi.SubstituteName.EndsWith(guid.Replace(Alphaleonis.Win32.Filesystem.Path.LongPathPrefix, string.Empty)));




            Console.WriteLine("\nRemove mount point.");

            Alphaleonis.Win32.Filesystem.Volume.DeleteVolumeMountPoint(folder.FullName);

            Assert.IsFalse(Alphaleonis.Win32.Filesystem.Volume.EnumerateVolumeMountPoints(guid).Any(mountPoint => folder.FullName.EndsWith(mountPoint.TrimEnd('\\'))));
         }
      }
   }
}
