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
   /// <summary>This is a test class for Host and is intended to contain all Host Unit Tests.</summary>
   public partial class HostTest
   {
      [TestMethod]
      public void AlphaFS_Host_ConnectTo()
      {
         Console.WriteLine("Network.Host.ConnectTo()");

         #region Connect to computer

         bool connectOk;
         var share = Alphaleonis.Win32.Network.Host.GetUncName();
         try
         {
            UnitTestConstants.StopWatcher(true);
            Alphaleonis.Win32.Network.Host.ConnectTo(share);
            Console.WriteLine("\nConnectTo(): [{0}]\n\n{1}\n", share, UnitTestConstants.Reporter(true));

            connectOk = true;

         }
         catch (Exception ex)
         {
            connectOk = false;

            Console.WriteLine("\nCaught (unexpected) {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));

            Console.WriteLine("\nFailed ConnectTo(): [{0}]", share);
         }

         Assert.IsTrue(connectOk);

         #endregion // Connect to computer

         #region Disconnect from share

         var disconnectOk = false;

         // We only need this for the unit test.
         while (!disconnectOk)
         {
            try
            {
               UnitTestConstants.StopWatcher(true);
               Alphaleonis.Win32.Network.Host.DisconnectFrom(share);
               Console.WriteLine("\nDisconnectFrom(): [{0}]\n\n{1}\n", share, UnitTestConstants.Reporter(true));

               disconnectOk = true;

            }
            catch (Exception ex)
            {
               disconnectOk = false;

               Console.WriteLine("\nCaught (unexpected) {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));

               Console.WriteLine("\nFailed DisconnectFrom(): [{0}]", share);
            }
         }

         Assert.IsTrue(disconnectOk);

         #endregion // Disconnect from share


         #region Connect to share

         share = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(UnitTestConstants.LocalHostShare);
         try
         {
            UnitTestConstants.StopWatcher(true);
            Alphaleonis.Win32.Network.Host.ConnectTo(share);
            Console.WriteLine("\nConnectTo(): [{0}]\n\n{1}\n", share, UnitTestConstants.Reporter(true));

            connectOk = true;

         }
         catch (Exception ex)
         {
            connectOk = false;

            Console.WriteLine("\nCaught (unexpected) {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));

            Console.WriteLine("\nFailed ConnectTo(): [{0}]", share);
         }

         Assert.IsTrue(connectOk);

         #endregion // Connect to share

         #region Disconnect from share

         disconnectOk = false;

         // We only need this for the unit test.
         while (!disconnectOk)
         {
            try
            {
               UnitTestConstants.StopWatcher(true);
               Alphaleonis.Win32.Network.Host.DisconnectFrom(share);
               Console.WriteLine("\nDisconnectFrom(): [{0}]\n\n{1}\n", share, UnitTestConstants.Reporter(true));

               disconnectOk = true;

            }
            catch (Exception ex)
            {
               disconnectOk = false;

               Console.WriteLine("\nCaught (unexpected) {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));

               Console.WriteLine("\nFailed DisconnectFrom(): [{0}]", share);
            }
         }

         Assert.IsTrue(disconnectOk);

         #endregion // Disconnect from share
      }
   }
}
