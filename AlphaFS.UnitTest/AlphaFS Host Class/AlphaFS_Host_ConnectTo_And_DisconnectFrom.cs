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
   public partial class AlphaFS_HostTest
   {
      [TestMethod]
      public void AlphaFS_Host_ConnectTo_And_DisconnectFrom_Network_Success()
      {
         UnitTestConstants.PrintUnitTestHeader(true);
         Console.WriteLine();


         #region Connect to computer

         bool connectOk;
         var share = Alphaleonis.Win32.Network.Host.GetUncName();
         try
         {
            Alphaleonis.Win32.Network.Host.ConnectTo(share);
            Console.WriteLine("ConnectTo(): [{0}]", share);

            connectOk = true;

         }
         catch (Exception ex)
         {
            connectOk = false;

            Console.WriteLine("\nCaught (UNEXPECTED) {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));

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
               Alphaleonis.Win32.Network.Host.DisconnectFrom(share);
               Console.WriteLine("\nDisconnectFrom(): [{0}]", share);

               disconnectOk = true;

            }
            catch (Exception ex)
            {
               disconnectOk = false;

               Console.WriteLine("\nCaught (UNEXPECTED) {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));

               Console.WriteLine("\nFailed DisconnectFrom(): [{0}]", share);
            }
         }

         Assert.IsTrue(disconnectOk);

         #endregion // Disconnect from share


         #region Connect to share

         share = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(UnitTestConstants.TempFolder);
         try
         {
            Alphaleonis.Win32.Network.Host.ConnectTo(share);
            Console.WriteLine("\nConnectTo(): [{0}]", share);

            connectOk = true;

         }
         catch (Exception ex)
         {
            connectOk = false;

            Console.WriteLine("\nCaught (UNEXPECTED) {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));

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
               Alphaleonis.Win32.Network.Host.DisconnectFrom(share);
               Console.WriteLine("\nDisconnectFrom(): [{0}]", share);

               disconnectOk = true;

            }
            catch (Exception ex)
            {
               disconnectOk = false;

               Console.WriteLine("\nCaught (UNEXPECTED) {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));

               Console.WriteLine("\nFailed DisconnectFrom(): [{0}]", share);
            }
         }

         Assert.IsTrue(disconnectOk);

         #endregion // Disconnect from share
      }
   }
}
