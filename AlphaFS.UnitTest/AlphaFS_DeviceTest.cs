/* Copyright (c) 2008-2014 Peter Palotas, Jeffrey Jangli, Normalex
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
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

namespace AlphaFS.UnitTest
{
   /// <summary>This is a test class for Device and is intended to contain all Device Unit Tests.</summary>
   [TestClass]
   public class AlphaFS_DeviceTest
   {
      #region DeviceTest Helpers

      private static readonly string LocalHost = Environment.MachineName; // Environment.MachineName equals using null.
      private readonly string MyServer = @"\\" + LocalHost;
      private static Stopwatch _stopWatcher;

      private static string StopWatcher(bool start = false)
      {
         if (_stopWatcher == null)
            _stopWatcher = new Stopwatch();

         if (start)
         {
            _stopWatcher.Restart();
            return null;
         }

         _stopWatcher.Stop();
         long ms = _stopWatcher.ElapsedMilliseconds;
         TimeSpan elapsed = _stopWatcher.Elapsed;

         return string.Format(CultureInfo.CurrentCulture, "*Duration: {0, 4} ms. ({1})", ms, elapsed);
      }

      private static string Reporter(bool condensed = false)
      {
         Win32Exception lastError = new Win32Exception();

         StopWatcher();

         if (condensed)
            return string.Format(CultureInfo.CurrentCulture, "{0} [{1}: {2}]", StopWatcher(), lastError.NativeErrorCode,
                                 lastError.Message);

         return string.Format(CultureInfo.CurrentCulture, "\t\t{0}\t*Win32 Result: [{1, 4}]\t*Win32 Message: [{2}]",
                              StopWatcher(), lastError.NativeErrorCode, lastError.Message);
      }

      /// <summary>Shows the Object's available Properties and Values.</summary>
      private static void Dump(object obj, int width = -35, bool indent = false)
      {
         int cnt = 0;
         const string nulll = "\t\tnull";
         string template = "\t{0}#{1:000}\t{2, " + width + "} == \t[{3}]";

         if (obj == null)
         {
            Console.WriteLine(nulll);
            return;
         }

         Console.WriteLine("\n\t{0}Instance: [{1}]\n", indent ? "\t" : "", obj.GetType().FullName);

         foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(obj).Sort().Cast<PropertyDescriptor>().Where(descriptor => descriptor != null))
         {
            string propValue = null;
            try
            {
               object value = descriptor.GetValue(obj);
               propValue = (value == null) ? "null" : value.ToString();
            }
            catch (Exception ex)
            {
               // Please do tell, oneliner preferably.
               propValue = ex.Message.Replace(Environment.NewLine, "  ");
            }

            Console.WriteLine(template, indent ? "\t" : "", ++cnt, descriptor.Name, propValue);
         }
      }

      #region DumpEnumerateDevices

      private void DumpEnumerateDevices(bool isLocal)
      {
         Console.WriteLine("\n=== TEST {0} ===", isLocal ? "LOCAL" : "NETWORK");
         string tempPath = isLocal ? LocalHost : MyServer;
         Console.Write("\nEnumerating devices from host: [{0}]\n", tempPath);

         int classCnt = 0;
         StopWatcher(true);

         try
         {
            foreach (DeviceGuid guid in Utils.EnumMemberToList<DeviceGuid>())
            {
               Console.WriteLine("\n#{0:000}\tClass: [{1}]", ++classCnt, guid);

               int cnt = 0;
               foreach (DeviceInfo device in Device.EnumerateDevices(tempPath, guid))
               {
                  Console.WriteLine("\n\t#{0:000}\tDevice Description: [{1}]", ++cnt, device.DeviceDescription);
                  Dump(device, -24);
               }
            }
         }
         catch (Exception ex)
         {
            Console.WriteLine("\nCaught Exception: [{0}]", ex.Message.Replace(Environment.NewLine, "  "));
         }


         Console.WriteLine("\n\t{0}\n", Reporter(true));
         if (isLocal)
            Assert.IsTrue(classCnt > 0, "Nothing was enumerated.");
      }

      #endregion //DumpEnumerateDevices

      #endregion // DeviceTest Helpers


      #region EnumerateDevices

      [TestMethod]
      public void EnumerateDevices()
      {
         Console.WriteLine("Device.EnumerateDevices()");
         Console.WriteLine("Enumerating all classes defined in enum: Filesystem.DeviceGuid");

         Console.WriteLine("\nMSDN Note: Beginning in Windows 8 and Windows Server 2012 functionality to access remote machines has been removed.");
         Console.WriteLine("You cannot access remote machines when running on these versions of Windows.\n");

         DumpEnumerateDevices(true);
         //DumpEnumerateDevices(false);
      }

      #endregion // EnumerateDevices
   }
}
