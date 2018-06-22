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
   public partial class EnumerationTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void AlphaFS_Device_EnumeratePortableDevices_OnlyDevicesUsingMTP()
      {
         UnitTestConstants.PrintUnitTestHeader(false);

         var deviceCount = 0;
         var autoConnect = false;

         // false: All  WPD devices that use UMS.
         // true : Only WPD devices that use MTP.
         const bool mtpOnly = true;
         

         foreach (var portableDeviceInfo in Alphaleonis.Win32.Device.Local.EnumeratePortableDevices(autoConnect, mtpOnly).OrderBy(p => p.DeviceType).ThenBy(p => p.FriendlyName))
         {
            Console.WriteLine("#{0:000}\tInput Portable Device Path: [{1}]", ++deviceCount, portableDeviceInfo.DeviceId);


            if (!autoConnect)
            {
               if (!mtpOnly)
               {
                  Assert.IsFalse(portableDeviceInfo.IsConnected, "The portable device is connected, but it is not expected.");

                  portableDeviceInfo.Connect();
               }
            }


            UnitTestConstants.Dump(portableDeviceInfo);

            Assert.IsTrue(portableDeviceInfo.IsConnected, "The portable device is not connected, but it is expected.");


            // Enumerate

            Console.WriteLine("\n\tEnumerating root folders from portable device.");
            
            foreach (var pdfse in portableDeviceInfo.EnumerateFileSystemEntries().OrderBy(fse => !fse.IsDirectory).ThenBy(fse => fse.FullName))
            {
               UnitTestConstants.Dump(pdfse, true);
            }


            portableDeviceInfo.Disconnect();
               
            Assert.IsFalse(portableDeviceInfo.IsConnected, "The portable device is connected, but it is not expected.");
         }


         if (deviceCount == 0)
            UnitTestAssert.InconclusiveBecauseEnumerationIsEmpty();
      }
   }
}
