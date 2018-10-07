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

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AlphaFS.UnitTest
{
   public partial class PathTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void AlphaFS_Path_GetFinalPathNameByHandle_Local_Success()
      {
         using (var tempRoot = new TemporaryDirectory())
         {
            var file = tempRoot.RandomDirectoryFullPath;

            Console.WriteLine("Input File Path: [{0}]\n", file);


            var longTempStream = Alphaleonis.Win32.Filesystem.Path.LongPathPrefix + file;
            bool gotFileNameNormalized;
            bool gotFileNameOpened;
            bool gotVolumeNameDos;
            bool gotVolumeNameGuid;
            bool gotVolumeNameNt;
            bool gotVolumeNameNone;


            using (var stream = System.IO.File.Create(file))
            {
               // For Windows versions < Vista, the file must be > 0 bytes.
               if (!Alphaleonis.Win32.Filesystem.NativeMethods.IsAtLeastWindowsVista)
                  stream.WriteByte(1);


               var handle = stream.SafeFileHandle;

               var fileNameNormalized = Alphaleonis.Win32.Filesystem.Path.GetFinalPathNameByHandle(handle);
               var fileNameOpened = Alphaleonis.Win32.Filesystem.Path.GetFinalPathNameByHandle(handle, Alphaleonis.Win32.Filesystem.FinalPathFormats.FileNameOpened);

               var volumeNameDos = Alphaleonis.Win32.Filesystem.Path.GetFinalPathNameByHandle(handle, Alphaleonis.Win32.Filesystem.FinalPathFormats.None);
               var volumeNameGuid = Alphaleonis.Win32.Filesystem.Path.GetFinalPathNameByHandle(handle, Alphaleonis.Win32.Filesystem.FinalPathFormats.VolumeNameGuid);
               var volumeNameNt = Alphaleonis.Win32.Filesystem.Path.GetFinalPathNameByHandle(handle, Alphaleonis.Win32.Filesystem.FinalPathFormats.VolumeNameNT);
               var volumeNameNone = Alphaleonis.Win32.Filesystem.Path.GetFinalPathNameByHandle(handle, Alphaleonis.Win32.Filesystem.FinalPathFormats.VolumeNameNone);


               gotFileNameNormalized = !Alphaleonis.Utils.IsNullOrWhiteSpace(fileNameNormalized) && longTempStream.Equals(fileNameNormalized);
               gotFileNameOpened = !Alphaleonis.Utils.IsNullOrWhiteSpace(fileNameOpened) && longTempStream.Equals(fileNameOpened);
               gotVolumeNameDos = !Alphaleonis.Utils.IsNullOrWhiteSpace(volumeNameDos) && longTempStream.Equals(volumeNameDos);


               gotVolumeNameGuid = !Alphaleonis.Utils.IsNullOrWhiteSpace(volumeNameGuid) && volumeNameGuid.StartsWith(Alphaleonis.Win32.Filesystem.Path.VolumePrefix) && volumeNameGuid.EndsWith(volumeNameNone);
               gotVolumeNameNt = !Alphaleonis.Utils.IsNullOrWhiteSpace(volumeNameNt) && volumeNameNt.StartsWith(Alphaleonis.Win32.Filesystem.Path.DevicePrefix);
               gotVolumeNameNone = !Alphaleonis.Utils.IsNullOrWhiteSpace(volumeNameNone) && file.EndsWith(volumeNameNone);


               Console.WriteLine("\tFilestream.Name                : [{0}]", stream.Name);
               Console.WriteLine("\tFinalPathFormats.None          : [{0}]", fileNameNormalized);
               Console.WriteLine("\tFinalPathFormats.FileNameOpened: [{0}]", fileNameOpened);
               Console.WriteLine("\tFinalPathFormats.VolumeNameDos : [{0}]", volumeNameDos);
               Console.WriteLine("\tFinalPathFormats.VolumeNameGuid: [{0}]", volumeNameGuid);
               Console.WriteLine("\tFinalPathFormats.VolumeNameNT  : [{0}]", volumeNameNt);
               Console.WriteLine("\tFinalPathFormats.VolumeNameNone: [{0}]", volumeNameNone);
            }


            Assert.IsTrue(gotFileNameNormalized);
            Assert.IsTrue(gotFileNameOpened);
            Assert.IsTrue(gotVolumeNameDos);
            Assert.IsTrue(gotVolumeNameGuid);
            Assert.IsTrue(gotVolumeNameNt);
            Assert.IsTrue(gotVolumeNameNone);
         }
      }
   }
}
