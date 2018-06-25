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
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AlphaFS.UnitTest
{
   public partial class CopyTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void AlphaFS_Device_PortableDevice_Copy_ExistingFiles()
      {
         using (var tempRoot = new TemporaryDirectory(false))
         {
            var folder = tempRoot.CreateDirectory();
            
            var deviceCount = 0;
            var fileCount = 0;
            const bool autoConnect = true;

            foreach (var portableDeviceInfo in Alphaleonis.Win32.Device.Local.EnumeratePortableDevices(autoConnect).OrderBy(p => p.DeviceType).ThenBy(p => p.DeviceFriendlyName))
            {
               Console.WriteLine("#{0:000}\tInput Portable Device Path: [{1}]", ++deviceCount, portableDeviceInfo.DeviceId);

               UnitTestConstants.Dump(portableDeviceInfo);

               Console.WriteLine();


               // Copy files.

               const int maxItems = 10;
               fileCount = 0;
               var fseCount = 0;
               long totalSize = 0;


               foreach (var fileInfo in portableDeviceInfo.EnumerateFiles(true))
               {
                  Console.WriteLine("\t\t#{0:000} ID: [{1}]\t\tFullName: [{2}]", ++fseCount, fileInfo.ObjectId, fileInfo.FullName);

                  ++fileCount;

                  totalSize += fileInfo.Length;
                  

                  portableDeviceInfo.Copy(fileInfo, folder.FullName);
                  

                  if (fseCount == maxItems)
                     break;
               }

               Console.WriteLine("\n\t\tCopied {0} files.  Total file size: [{1}]", fileCount, Alphaleonis.Utils.UnitSizeToText(totalSize));
               Console.WriteLine("\t\tDestination Directory Path: [{0}]\n", folder.FullName);

               portableDeviceInfo.Disconnect();
            }


            Assert.IsTrue(fileCount > 0, "No files copied from the portable device, but it is expected.");
         }
      }
   }
}
