/*  Copyright (C) 2008-2016 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
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
   /// <summary>This is a test class for Device and is intended to contain all Device Unit Tests.</summary>
   [TestClass]
   public partial class DeviceTest
   {
      [TestMethod]
      public void AlphaFS_Device_EnumerateDevices_Success()
      {
         Console.WriteLine("\nMSDN Note: Beginning in Windows 8 and Windows Server 2012 functionality to access remote machines has been removed.");
         Console.WriteLine("You cannot access remote machines when running on these versions of Windows.\n");

         Device_EnumerateDevices(false);
      }




      private void Device_EnumerateDevices(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         var tempPath = UnitTestConstants.LocalHost;
         var classCnt = 0;

         foreach (var guid in Alphaleonis.Utils.EnumMemberToList<Alphaleonis.Win32.Filesystem.DeviceGuid>())
         {
            Console.WriteLine("\n#{0:000}\tClass: [{1}]", ++classCnt, guid);

            foreach (var device in Alphaleonis.Win32.Filesystem.Device.EnumerateDevices(tempPath, guid))
               UnitTestConstants.Dump(device, -24);
         }

         if (classCnt == 0)
            Assert.Inconclusive("Nothing was enumerated, but it was expected.");
      }
   }
}
