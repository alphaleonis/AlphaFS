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

using System.Net.NetworkInformation;
using Alphaleonis.Win32.Filesystem;
using Alphaleonis.Win32.Network;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Globalization;
using System.Linq;
using NativeMethods = Alphaleonis.Win32.Network.NativeMethods;
using Path = Alphaleonis.Win32.Filesystem.Path;

namespace AlphaFS.UnitTest
{
   /// <summary>This is a test class for Host and is intended to contain all Host Unit Tests.</summary>
   [TestClass]
   public class AlphaFS_HostTest
   {
      #region DumpEnumerateDrives

      private static void DumpEnumerateDrives(bool isLocal)
      {
         Console.WriteLine("\n=== TEST {0} ===", isLocal ? UnitTestConstants.Local : UnitTestConstants.Network);
         string host = UnitTestConstants.LocalHost;


         if (isLocal)
         {
            string nonX = Path.GetRandomFileName();
            bool caughtException = false;
            Console.WriteLine("\nEnumerating drives from (non-existing) host: [{0}]\n", nonX);
            UnitTestConstants.StopWatcher(true);
            try
            {
               Host.EnumerateDrives(nonX, false).Any();
            }
            catch (Exception ex)
            {
               caughtException = true;
               Console.Write("Caught (expected) {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
            }
            Console.WriteLine("\n{0}\n", UnitTestConstants.Reporter(true));
            Assert.IsTrue(caughtException, "No Exception was caught.");

            Console.Write("\n");
         }


         Console.WriteLine("Enumerating drives from host: [{0}]\n", host);
         int cnt = 0;
         UnitTestConstants.StopWatcher(true);

         foreach (string drive in Host.EnumerateDrives(host, true))
         {
            Console.WriteLine("\t#{0:000}\tDrive: [{1}]", ++cnt, drive);
         }

         Console.WriteLine("\n{0}", UnitTestConstants.Reporter(true));

         if (cnt == 0)
            Assert.Inconclusive("Nothing was enumerated.");

         Console.WriteLine();
      }

      #endregion // DumpEnumerateDrives


      #region ConnectDrive

      [TestMethod]
      public void ConnectDrive()
      {
         Console.WriteLine("Network.Host.ConnectDrive()");

         #region Connect drive to share

         string drive = string.Format(CultureInfo.CurrentCulture, "{0}{1}{2}", DriveInfo.GetFreeDriveLetter(), Path.VolumeSeparatorChar, Path.DirectorySeparatorChar);
         string share = Path.LocalToUnc(UnitTestConstants.LocalHostShare);
         bool connectOk;
         Console.WriteLine("\nConnect using a designated drive: [{0}]", drive);
         try
         {
            UnitTestConstants.StopWatcher(true);
            Host.ConnectDrive(drive, share);
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

         bool disconnectOk = false;

         // We only need this for the unit test.
         while (!disconnectOk)
         {
            try
            {
               UnitTestConstants.StopWatcher(true);
               Host.DisconnectDrive(drive);
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
            drive = Host.ConnectDrive(null, share);
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
               Host.DisconnectDrive(drive);
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

      #endregion // ConnectDrive
      
      #region ConnectTo

      [TestMethod]
      public void ConnectTo()
      {
         Console.WriteLine("Network.Host.ConnectTo()");

         #region Connect to computer

         bool connectOk;
         string share = Host.GetUncName();
         try
         {
            UnitTestConstants.StopWatcher(true);
            Host.ConnectTo(share);
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

         bool disconnectOk = false;

         // We only need this for the unit test.
         while (!disconnectOk)
         {
            try
            {
               UnitTestConstants.StopWatcher(true);
               Host.DisconnectFrom(share);
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

         share = Path.LocalToUnc(UnitTestConstants.LocalHostShare);
         try
         {
            UnitTestConstants.StopWatcher(true);
            Host.ConnectTo(share);
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
               Host.DisconnectFrom(share);
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

      #endregion // ConnectTo

      #region DisconnectDrive

      [TestMethod]
      public void DisconnectDrive()
      {
         Console.WriteLine("Network.Host.DisconnectDrive()");
         Console.WriteLine("\nPlease see unit test: ConnectDrive()");
      }

      #endregion // DisconnectDrive

      #region DisconnectFrom

      [TestMethod]
      public void DisconnectFrom()
      {
         Console.WriteLine("Network.Host.DisconnectFrom()");
         Console.WriteLine("\nPlease see unit test: ConnectTo()");
      }

      #endregion // DisconnectFrom


      #region EnumerateDfsLinks

      [TestMethod]
      public void EnumerateDfsLinks()
      {
         Console.WriteLine("Network.Host.EnumerateDfsLinks()");

         int cnt = 0;
         bool noDomainConnection = true;
         UnitTestConstants.StopWatcher(true);
         try
         {
            foreach (string dfsNamespace in Host.EnumerateDomainDfsRoot())
            {
               noDomainConnection = false;

               Console.Write("\n#{0:000}\tDFS Root: [{1}]\n", ++cnt, dfsNamespace);
               int cnt2 = 0;

               try
               {
                  foreach (DfsInfo dfsInfo in Host.EnumerateDfsLinks(dfsNamespace).OrderBy(d => d.EntryPath))
                  Console.Write("\n\t#{0:000}\tDFS Link: [{1}]", ++cnt2, dfsInfo.EntryPath);
               }
               catch (NetworkInformationException ex)
               {
                  Console.WriteLine("\n\tNetworkInformationException #1: [{0}]", ex.Message.Replace(Environment.NewLine, "  "));
               }
               catch (Exception ex)
               {
                  Console.WriteLine("\n\tCaught (unexpected #1) {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
               }

               Console.WriteLine();
            }
         }
         catch (NetworkInformationException ex)
         {
            Console.WriteLine("\n\tNetworkInformationException #2: [{0}]", ex.Message.Replace(Environment.NewLine, "  "));
         }
         catch (Exception ex)
         {
            Console.WriteLine("\n\tCaught (unexpected #2) {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
         }

         Console.WriteLine("\n\n{0}", UnitTestConstants.Reporter(true));

         if (noDomainConnection)
            Assert.Inconclusive("Test ignored because the computer is probably not connected to a domain.");
         else if (cnt == 0)
            Assert.Inconclusive("Nothing was enumerated.");

         Console.WriteLine();
      }

      #endregion // EnumerateDfsLinks

      #region EnumerateDfsRoot

      [TestMethod]
      public void EnumerateDfsRoot()
      {
         Console.WriteLine("Network.Host.EnumerateDfsRoot()");

         int cnt = 0;
         bool noDomainConnection = true;
         UnitTestConstants.StopWatcher(true);

         // Drill down to get servers from the first namespace retrieved.

         try
         { 
            foreach (string dfsName in Host.EnumerateDomainDfsRoot())
            {
               noDomainConnection = false;

               Console.Write("\n#{0:000}\tDFS Root: [{1}]\n", ++cnt, dfsName);

               try
               {
                  DfsInfo dfsInfo = Host.GetDfsInfo(dfsName);

                  foreach (DfsStorageInfo storage in dfsInfo.StorageInfoCollection)
                  {
                     int cnt2 = 0;
                     Console.Write("\n\tEnumerating DFS Namespaces from host: [{0}]\n", storage.ServerName);

                     foreach (string dfsNamespace in Host.EnumerateDfsRoot(storage.ServerName, true))
                        Console.Write("\t#{0:000}\tDFS Root: [{1}]\n", ++cnt2, dfsNamespace);
                  }
               }
               catch (NetworkInformationException ex)
               {
                  Console.WriteLine("\n\tNetworkInformationException #1: [{0}]", ex.Message.Replace(Environment.NewLine, "  "));
               }
               catch (Exception ex)
               {
                  Console.WriteLine("\n\tCaught (unexpected #1) {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
               }
            }

            Console.Write("\n{0}", UnitTestConstants.Reporter(true));

            if (cnt == 0)
               Assert.Inconclusive("Nothing was enumerated.");
         }
         catch (NetworkInformationException ex)
         {
            Console.WriteLine("\n\tNetworkInformationException #2: [{0}]", ex.Message.Replace(Environment.NewLine, "  "));
         }
         catch (Exception ex)
         {
            Console.WriteLine("\n\tCaught (unexpected #2) {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
         }

         Console.WriteLine("\n\n{0}", UnitTestConstants.Reporter(true));

         if (noDomainConnection)
            Assert.Inconclusive("Test ignored because the computer is probably not connected to a domain.");
         else if (cnt == 0)
            Assert.Inconclusive("Nothing was enumerated.");

         Console.WriteLine();
      }

      #endregion // EnumerateDfsRoot

      #region EnumerateDomainDfsRoot

      [TestMethod]
      public void EnumerateDomainDfsRoot()
      {
         Console.WriteLine("Network.Host.EnumerateDomainDfsRoot()");

         Console.Write("\nEnumerating DFS Root from user domain: [{0}]\n", NativeMethods.ComputerDomain);
         int cnt = 0;
         bool noDomainConnection = true;
         UnitTestConstants.StopWatcher(true);
         try
         {
            foreach (string dfsNamespace in Host.EnumerateDomainDfsRoot())
            {
               noDomainConnection = false;
               Console.Write("\n\t#{0:000}\tDFS Root: [{1}]", ++cnt, dfsNamespace);
            }
         }
         catch (NetworkInformationException ex)
         {
            Console.WriteLine("\n\tNetworkInformationException: [{0}]", ex.Message.Replace(Environment.NewLine, "  "));
         }
         catch (Exception ex)
         {
            Console.WriteLine("\n\tCaught (unexpected) {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
         }

         Console.WriteLine("\n\n{0}", UnitTestConstants.Reporter(true));

         if (noDomainConnection)
            Assert.Inconclusive("Test ignored because the computer is probably not connected to a domain.");
         else if (cnt == 0)
            Assert.Inconclusive("Nothing was enumerated.");

         Console.WriteLine();
      }

      #endregion // EnumerateDomainDfsRoot

      #region EnumerateDrives

      [TestMethod]
      public void EnumerateDrives()
      {
         Console.WriteLine("Network.Host.EnumerateDrives()");

         DumpEnumerateDrives(true);
      }

      #endregion // EnumerateDrives
      
      #region EnumerateOpenConnections

      [TestMethod]
      public void EnumerateOpenConnections()
      {
         Console.WriteLine("Network.Host.EnumerateOpenConnections()");
         Console.WriteLine("\nPlease see unit test: Network_Class_OpenConnectionInfo");
      }

      #endregion // EnumerateOpenConnections

      #region EnumerateOpenResources

      [TestMethod]
      public void EnumerateOpenResources()
      {
         Console.WriteLine("Network.Host.EnumerateOpenResources()");
         Console.WriteLine("\nPlease see unit test: Network_Class_OpenResourceInfo()");
      }

      #endregion // EnumerateOpenResources

      #region EnumerateShares

      [TestMethod]
      public void EnumerateShares()
      {
         Console.WriteLine("Network.Host.EnumerateShares()");
         Console.WriteLine("\nPlease see unit test: Network_Class_ShareInfo()");
      }

      #endregion // EnumerateShares

      #region GetHostShareFromPath

      [TestMethod]
      public void GetHostShareFromPath()
      {
         Console.WriteLine("Network.Host.GetHostShareFromPath\n");

         string uncPath = UnitTestConstants.SysRoot32;
         string[] hostAndShare = Host.GetHostShareFromPath(uncPath);
         Console.WriteLine("Input local path: [{0}]", uncPath);
         Assert.AreEqual(null, hostAndShare);

         uncPath = Path.GetLongPath(UnitTestConstants.SysRoot32);
         hostAndShare = Host.GetHostShareFromPath(uncPath);
         Console.WriteLine("Input local path: [{0}]", uncPath);
         Assert.AreEqual(null, hostAndShare);
         
         Console.WriteLine();

         uncPath = Path.LocalToUnc(UnitTestConstants.SysRoot32);
         hostAndShare = Host.GetHostShareFromPath(uncPath);
         Console.WriteLine("Input UNC path: [{0}]", uncPath);
         Console.WriteLine("\tHost : [{0}]", hostAndShare[0]);
         Console.WriteLine("\tShare: [{0}]", hostAndShare[1]);

         Assert.AreEqual(Environment.MachineName, hostAndShare[0].ToUpperInvariant());

         Console.WriteLine();
         
         uncPath = Path.LocalToUnc(UnitTestConstants.SysRoot32, true);
         hostAndShare = Host.GetHostShareFromPath(uncPath);
         Console.WriteLine("Input UNC path: [{0}]", uncPath);
         Console.WriteLine("\tHost : [{0}]", hostAndShare[0]);
         Console.WriteLine("\tShare: [{0}]", hostAndShare[1]);

         Assert.AreEqual(Environment.MachineName, hostAndShare[0].ToUpperInvariant());
      }

      #endregion // GetHostShareFromPath

      #region GetDfsClientInfo

      [TestMethod]
      public void GetDfsClientInfo()
      {
         Console.WriteLine("Network.Host.GetDfsClientInfo()");
         Console.WriteLine("\nPlease see unit test: Network_Class_DfsXxx()");
      }

      #endregion // GetDfsClientInfo

      #region GetDfsInfo

      [TestMethod]
      public void GetDfsInfo()
      {
         Console.WriteLine("Network.Host.GetDfsInfo()");
         Console.WriteLine("\nPlease see unit test: EnumerateDfsRoot()");
      }

      #endregion // GetDfsInfo

      #region GetShareInfo

      [TestMethod]
      public void GetShareInfo()
      {
         Console.WriteLine("Network.Host.GetShareInfo()");
         Console.WriteLine("\nPlease see unit test: Network_Class_ShareInfo()");
      }

      #endregion // GetShareInfo

      #region GetUncName

      [TestMethod]
      public void GetUncName()
      {
         Console.WriteLine("Network.Host.GetUncName()");

         string hostUncName = Host.GetUncName();
         Console.WriteLine("\nHost.GetUncName(): [{0}]", hostUncName);

         Assert.AreEqual(Path.UncPrefix + Environment.MachineName, hostUncName);
      }

      #endregion // GetUncName


      #region DriveConnection

      [TestMethod]
      public void DriveConnection()
      {
         Console.WriteLine("Network.Host.DriveConnection()");

         #region Using last available drive to share

         string share = Path.LocalToUnc(UnitTestConstants.LocalHostShare);
         bool connectOk;
         Console.WriteLine("\nUsing IDisposable class.");
         try
         {
            UnitTestConstants.StopWatcher(true);
            using (var connection = new DriveConnection(share))
            {
               Console.WriteLine("\nUsing DriveConnection(): [{0}] to: [{1}]\n\n{2}\n", connection.LocalName, share, UnitTestConstants.Reporter(true));
               connectOk = true;
            }
         }
         catch (Exception ex)
         {
          
            connectOk = false;

            Console.WriteLine("\nCaught (unexpected) {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));

            Console.WriteLine("\nFailed DriveConnection() to: [{0}]", share);
         }

         Assert.IsTrue(connectOk);

         #endregion // Using last available drive to share
      }

      #endregion // DriveConnection
   }
}
