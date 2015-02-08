/*  Copyright (C) 2008-2015 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
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

using Alphaleonis;
using Alphaleonis.Win32.Filesystem;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AlphaFS.UnitTest
{
   /// <summary>This is a test class for Device and is intended to contain all Device Unit Tests.</summary>
   [TestClass]
   public class AlphaFS_DeviceTest
   {
      #region DumpEnumerateDevices

      private void DumpEnumerateDevices(bool isLocal)
      {
         Console.WriteLine("\n=== TEST {0} ===", isLocal ? UnitTestConstants.Local : UnitTestConstants.Network);
         string tempPath = UnitTestConstants.LocalHost;
         Console.Write("\nEnumerating devices from host: [{0}]\n", tempPath);

         int classCnt = 0;
         UnitTestConstants.StopWatcher(true);

         try
         {
            foreach (DeviceGuid guid in Utils.EnumMemberToList<DeviceGuid>())
            {
               Console.WriteLine("\n#{0:000}\tClass: [{1}]", ++classCnt, guid);

               int cnt = 0;
               foreach (DeviceInfo device in Device.EnumerateDevices(tempPath, guid))
               {
                  Console.WriteLine("\n\t#{0:000}\tDevice Description: [{1}]", ++cnt, device.DeviceDescription);
                  UnitTestConstants.Dump(device, -24);
               }
            }
         }
         catch (Exception ex)
         {
            Console.WriteLine("\nCaught (unexpected) {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
         }


         Console.WriteLine("\n\t{0}\n", UnitTestConstants.Reporter(true));

         if (isLocal && classCnt == 0)
            Assert.Inconclusive("Nothing was enumerated.");
      }

      #endregion //DumpEnumerateDevices

      #region EnumerateDevices

      [TestMethod]
      public void EnumerateDevices()
      {
         Console.WriteLine("Device.EnumerateDevices()");
         Console.WriteLine("Enumerating all classes defined in enum: Filesystem.DeviceGuid");

         Console.WriteLine("\nMSDN Note: Beginning in Windows 8 and Windows Server 2012 functionality to access remote machines has been removed.");
         Console.WriteLine("You cannot access remote machines when running on these versions of Windows.\n");

         DumpEnumerateDevices(true);
      }

      #endregion // EnumerateDevices
   }
}
