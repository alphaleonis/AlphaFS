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

namespace AlphaFS.UnitTest
{
   public partial class AlphaFS_HostTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void AlphaFS_Host_ConnectDrive_And_DisconnectDrive_Network_Success()
      {
         using (var tempRoot = new TemporaryDirectory(true))
         {
            // Randomly test the share where the local folder possibly has the read-only and/or hidden attributes set.

            var folder = tempRoot.CreateDirectoryRandomizedAttributes();

            var drive = Alphaleonis.Win32.Filesystem.DriveInfo.GetFreeDriveLetter() + ":";


            try
            {
               Console.WriteLine("Connect drive [{0}] to share [{1}]", drive, folder.FullName);

               Alphaleonis.Win32.Network.Host.ConnectDrive(drive, folder.FullName);

               UnitTestConstants.Dump(new System.IO.DriveInfo(drive));

               Assert.IsTrue(System.IO.Directory.Exists(drive), "The drive does not exists, but is expected to.");
            }
            finally
            {
               Console.WriteLine("\nDisconnect drive from share.");

               Alphaleonis.Win32.Network.Host.DisconnectDrive(drive);

               Assert.IsFalse(System.IO.Directory.Exists(drive), "The drive exists, but is expected not to.");
            }
         }
      }
   }
}
