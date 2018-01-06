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
using System.Globalization;

namespace AlphaFS.UnitTest
{
   /// <summary>This is a test class for Host and is intended to contain all Host Unit Tests.</summary>
   public partial class HostTest
   {
      [TestMethod]
      public void AlphaFS_Host_ConnectDrive()
      {
         Console.WriteLine("Network.Host.ConnectDrive()");

         #region Connect drive to share

         var drive = string.Format(CultureInfo.InvariantCulture, "{0}{1}{2}", Alphaleonis.Win32.Filesystem.DriveInfo.GetFreeDriveLetter(), Alphaleonis.Win32.Filesystem.Path.VolumeSeparatorChar, Alphaleonis.Win32.Filesystem.Path.DirectorySeparatorChar);
         var share = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(UnitTestConstants.TempFolder);
         bool connectOk;
         Console.WriteLine("\nConnect using a designated drive: [{0}]", drive);
         try
         {
            UnitTestConstants.StopWatcher(true);
            Alphaleonis.Win32.Network.Host.ConnectDrive(drive, share);
            Console.WriteLine("\nConnectDrive(): [{0}] to: [{1}]\n\n{2}\n", drive, share, UnitTestConstants.Reporter(true));

            connectOk = true;

         }
         catch (Exception ex)
         {
            connectOk = false;

            Console.WriteLine("\nCaught (unexpected) {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));

            Console.WriteLine("\nFailed ConnectDrive(): [{0}] to: [{1}]", drive, share);
         }

         Assert.IsTrue(connectOk);

         #endregion // Connect drive to share


         #region Disconnect drive from share

         var disconnectOk = false;

         // We only need this for the unit test.
         while (!disconnectOk)
         {
            try
            {
               UnitTestConstants.StopWatcher(true);
               Alphaleonis.Win32.Network.Host.DisconnectDrive(drive);
               Console.WriteLine("\nDisconnectDrive(): [{0}] from: [{1}]\n\n{2}\n", drive, share, UnitTestConstants.Reporter(true));

               disconnectOk = true;

            }
            catch (Exception ex)
            {
               disconnectOk = false;
               Console.WriteLine("\nCaught (unexpected) {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));

               Console.WriteLine("\nFailed DisconnectDrive(): [{0}] from: [{1}]", drive, share);
            }
         }

         Assert.IsTrue(disconnectOk);

         #endregion // Disconnect  drive from share




         #region Connect last available drive to share

         Console.WriteLine("\nConnect using the last available drive.");
         drive = null;
         try
         {
            UnitTestConstants.StopWatcher(true);
            drive = Alphaleonis.Win32.Network.Host.ConnectDrive(null, share);
            Console.WriteLine("\nConnectDrive(): [{0}] to: [{1}]\n\n{2}\n", drive, share, UnitTestConstants.Reporter(true));

            connectOk = true;

         }
         catch (Exception ex)
         {
            connectOk = false;

            Console.WriteLine("\nCaught (unexpected) {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));

            Console.WriteLine("\nFailed ConnectDrive(): [{0}] to: [{1}]", drive, share);
         }

         Assert.IsTrue(connectOk);

         #endregion // Connect last available drive to share


         #region Disconnect last available drive from share

         disconnectOk = false;

         // We only need this for the unit test.
         while (!disconnectOk)
         {
            try
            {
               UnitTestConstants.StopWatcher(true);
               Alphaleonis.Win32.Network.Host.DisconnectDrive(drive);
               Console.WriteLine("\nDisconnectDrive(): [{0}] from: [{1}]\n\n{2}\n", drive, share, UnitTestConstants.Reporter(true));

               disconnectOk = true;

            }
            catch (Exception ex)
            {
               disconnectOk = false;

               Console.WriteLine("\nCaught (unexpected) {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));

               Console.WriteLine("\nFailed DisconnectDrive(): [{0}] from: [{1}]", drive, share);
            }
         }

         Assert.IsTrue(disconnectOk);

         #endregion // Disconnect last available drive from share
      }
   }
}
