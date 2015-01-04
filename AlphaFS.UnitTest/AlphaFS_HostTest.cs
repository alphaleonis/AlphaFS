/* Copyright 2008-2015 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
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
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using Directory = Alphaleonis.Win32.Filesystem.Directory;
using NativeMethods = Alphaleonis.Win32.Network.NativeMethods;
using Path = Alphaleonis.Win32.Filesystem.Path;

namespace AlphaFS.UnitTest
{
   /// <summary>This is a test class for Host and is intended to contain all Host Unit Tests.</summary>
   [TestClass]
   public class AlphaFS_HostTest
   {
      #region HostTest Helpers

      private readonly string LocalHost = Environment.MachineName;
      private readonly string LocalHostShare = Environment.SystemDirectory;
      private readonly bool _testMyServer = Environment.UserName.Equals(@"jjangli", StringComparison.OrdinalIgnoreCase);
      private const string MyServer = "yomodo";
      private const string MyServerShare = @"\\" + MyServer + @"\video";
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

         if (_stopWatcher.IsRunning)
            _stopWatcher.Stop();

         long ms = _stopWatcher.ElapsedMilliseconds;
         TimeSpan elapsed = _stopWatcher.Elapsed;

         return string.Format(CultureInfo.CurrentCulture, "*Duration: [{0}] ms. ({1})", ms, elapsed);
      }

      private static string Reporter(bool condensed = false, bool onlyTime = false)
      {
         Win32Exception lastError = new Win32Exception();

         StopWatcher();

         return onlyTime
            ? string.Format(CultureInfo.CurrentCulture, condensed
               ? "{0} [{1}: {2}]"
               : "\t\t{0}", StopWatcher())
            : string.Format(CultureInfo.CurrentCulture, condensed
               ? "{0} [{1}: {2}]"
               : "\t\t{0}\t*Win32 Result: [{1, 4}]\t*Win32 Message: [{2}]", StopWatcher(), lastError.NativeErrorCode, lastError.Message);
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
            string propValue;
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

      #region Dumpers

      #region DumpEnumerateDrives

      private void DumpEnumerateDrives(bool isLocal)
      {
         Console.WriteLine("\n=== TEST {0} ===", isLocal ? "LOCAL" : "NETWORK");
         string host = isLocal ? LocalHost : MyServer;


         if (isLocal)
         {
            string nonX = Path.GetRandomFileName();
            bool caughtException = false;
            Console.WriteLine("\nEnumerating drives from (non-existing) host: [{0}]\n", nonX);
            StopWatcher(true);
            try
            {
               Host.EnumerateDrives(nonX, false).Any();
            }
            catch (Exception ex)
            {
               caughtException = true;
               Console.Write("Caught Exception (As expected): [{0}]", ex.Message.Replace(Environment.NewLine, "  "));
            }
            Console.WriteLine("\n\t{0}\n", Reporter(true));
            Assert.IsTrue(caughtException, "No Exception was caught.");

            Console.Write("\n");
         }


         Console.WriteLine("Enumerating drives from host: [{0}]\n", host);
         int cnt = 0;
         StopWatcher(true);

         foreach (string drive in Host.EnumerateDrives(host, true))
         {
            Console.WriteLine("\t#{0:000}\tDrive: [{1}]", ++cnt, drive);
         }

         Console.WriteLine("\n\t{0}\n", Reporter(true));
         if (isLocal)
            Assert.IsTrue(cnt > 0, "Nothing was enumerated.");
      }

      #endregion // DumpEnumerateDrives

      #region DumpEnumerateShares

      private void DumpEnumerateShares(bool isLocal)
      {
         Console.WriteLine("\n=== TEST {0} ===", isLocal ? "LOCAL" : "NETWORK");
         string host = isLocal ? LocalHost : MyServer;


         if (isLocal)
         {
            string nonX = Path.GetRandomFileName();
            bool caughtException = false;
            Console.WriteLine("\nEnumerating shares from (non-existing) host: [{0}]\n", nonX);
            StopWatcher(true);
            try
            {
               Host.EnumerateShares(nonX, false).Any();
            }
            catch (Exception ex)
            {
               caughtException = true;
               Console.Write("Caught Exception (As expected): [{0}]", ex.Message.Replace(Environment.NewLine, "  "));
            }
            Console.Write("\t{0}", Reporter(true));
            Assert.IsTrue(caughtException, "No Exception was caught.");
            Console.Write("\n\n");
         }


         Console.WriteLine("\nEnumerating shares from host: [{0}]\n", host);
         int cnt = 0;
         StopWatcher(true);
         foreach (ShareInfo share in Host.EnumerateShares(host, true))
         {
            Console.WriteLine("\t#{0:000}\tShare: [{1}]\tPath on host: [{2}]", ++cnt, share, share.Path);

            Dump(share, -18);
            Console.Write("\n\n");
         }

         Console.WriteLine("\n\t{0}\n", Reporter(true));
         if (isLocal)
            Assert.IsTrue(cnt > 0, "Nothing was enumerated.");
      }

      #endregion // DumpEnumerateShares

      #endregion Dumpers

      #endregion // FileTest Helpers

      #region ConnectDrive

      [TestMethod]
      public void ConnectDrive()
      {
         Console.WriteLine("Network.Host.ConnectDrive()");

         #region Connect drive to share

         string drive = string.Format(CultureInfo.CurrentCulture, "{0}{1}{2}", DriveInfo.GetFreeDriveLetter(), Path.VolumeSeparatorChar, Path.DirectorySeparatorChar);
         string share = Path.LocalToUnc(LocalHostShare);
         bool connectOk;
         Console.WriteLine("\nConnect using a designated drive: [{0}]", drive);
         try
         {
            StopWatcher(true);
            Host.ConnectDrive(drive, share);
            Console.WriteLine("\nConnectDrive(): [{0}] to: [{1}]\n\n\t{2}\n", drive, share, Reporter(true));

            connectOk = true;

         }
         catch (Exception ex)
         {
            connectOk = false;
            Console.WriteLine("\nFailed ConnectDrive(): [{0}] to: [{1}]", drive, share);
            Console.WriteLine("\nCaught Exception: [{0}]", ex.Message.Replace(Environment.NewLine, "  "));
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
               StopWatcher(true);
               Host.DisconnectDrive(drive);
               Console.WriteLine("\nDisconnectDrive(): [{0}] from: [{1}]\n\n\t{2}\n", drive, share, Reporter(true));

               disconnectOk = true;

            }
            catch (Exception ex)
            {
               disconnectOk = false;
               Console.WriteLine("\nFailed DisconnectDrive(): [{0}] from: [{1}]", drive, share);
               Console.WriteLine("\nCaught Exception: [{0}]", ex.Message.Replace(Environment.NewLine, "  "));
            }
         }

         Assert.IsTrue(disconnectOk);

         #endregion // Disconnect  drive from share


         #region Connect last available drive to share

         Console.WriteLine("\nConnect using the last available drive.");
         drive = null;
         try
         {
            StopWatcher(true);
            drive = Host.ConnectDrive(null, share);
            Console.WriteLine("\nConnectDrive(): [{0}] to: [{1}]\n\n\t{2}\n", drive, share, Reporter(true));

            connectOk = true;

         }
         catch (Exception ex)
         {
            connectOk = false;
            Console.WriteLine("\nFailed ConnectDrive(): [{0}] to: [{1}]", drive, share);
            Console.WriteLine("\nCaught Exception: [{0}]", ex.Message.Replace(Environment.NewLine, "  "));
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
               StopWatcher(true);
               Host.DisconnectDrive(drive);
               Console.WriteLine("\nDisconnectDrive(): [{0}] from: [{1}]\n\n\t{2}\n", drive, share, Reporter(true));

               disconnectOk = true;

            }
            catch (Exception ex)
            {
               disconnectOk = false;
               Console.WriteLine("\nFailed DisconnectDrive(): [{0}] from: [{1}]", drive, share);
               Console.WriteLine("\nCaught Exception: [{0}]", ex.Message.Replace(Environment.NewLine, "  "));
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
            StopWatcher(true);
            Host.ConnectTo(share);
            Console.WriteLine("\nConnectTo(): [{0}]\n\n\t{1}\n", share, Reporter(true));

            connectOk = true;

         }
         catch (Exception ex)
         {
            connectOk = false;
            Console.WriteLine("\nFailed ConnectTo(): [{0}]", share);
            Console.WriteLine("\nCaught Exception: [{0}]", ex.Message.Replace(Environment.NewLine, "  "));
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
               StopWatcher(true);
               Host.DisconnectFrom(share);
               Console.WriteLine("\nDisconnectFrom(): [{0}]\n\n\t{1}\n", share, Reporter(true));

               disconnectOk = true;

            }
            catch (Exception ex)
            {
               disconnectOk = false;
               Console.WriteLine("\nFailed DisconnectFrom(): [{0}]", share);
               Console.WriteLine("\nCaught Exception: [{0}]", ex.Message.Replace(Environment.NewLine, "  "));
            }
         }

         Assert.IsTrue(disconnectOk);

         #endregion // Disconnect from share


         #region Connect to share

         share = Path.LocalToUnc(LocalHostShare);
         try
         {
            StopWatcher(true);
            Host.ConnectTo(share);
            Console.WriteLine("\nConnectTo(): [{0}]\n\n\t{1}\n", share, Reporter(true));

            connectOk = true;

         }
         catch (Exception ex)
         {
            connectOk = false;
            Console.WriteLine("\nFailed ConnectTo(): [{0}]", share);
            Console.WriteLine("\nCaught Exception: [{0}]", ex.Message.Replace(Environment.NewLine, "  "));
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
               StopWatcher(true);
               Host.DisconnectFrom(share);
               Console.WriteLine("\nDisconnectFrom(): [{0}]\n\n\t{1}\n", share, Reporter(true));

               disconnectOk = true;

            }
            catch (Exception ex)
            {
               disconnectOk = false;
               Console.WriteLine("\nFailed DisconnectFrom(): [{0}]", share);
               Console.WriteLine("\nCaught Exception: [{0}]", ex.Message.Replace(Environment.NewLine, "  "));
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
         StopWatcher(true);
         try
         {
            foreach (string dfsNamespace in Host.EnumerateDomainDfsRoot())
            {
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
                  Console.WriteLine("\n\tException (1): [{0}]", ex.Message.Replace(Environment.NewLine, "  "));
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
            Console.WriteLine("\n\tException (2): [{0}]", ex.Message.Replace(Environment.NewLine, "  "));
         }

         Console.WriteLine("\n\n\t{0}", Reporter(true));
         Assert.IsTrue(cnt > 0, "Nothing was enumerated.");

         Console.WriteLine();
      }

      #endregion // EnumerateDfsLinks

      #region EnumerateDfsRoot

      [TestMethod]
      public void EnumerateDfsRoot()
      {
         Console.WriteLine("Network.Host.EnumerateDfsRoot()");

         int cnt = 0;
         StopWatcher(true);

         // Drill down to get servers from the first namespace retrieved.

         try
         { 
            foreach (string dfsName in Host.EnumerateDomainDfsRoot())
            {
               Console.Write("\n#{0:000}\tDFS Root: [{1}]\n", ++cnt, dfsName);

               try
               {
                  DfsInfo dfsInfo = Host.GetDfsInfo(dfsName);

                  foreach (DfsStorage storage in dfsInfo.NumberOfStorages)
                  {
                     int cnt2 = 0;
                     Console.Write("\n\tEnumerating DFS Namespaces from host: [{0}]\n", storage.ServerName);

                     foreach (string dfsNamespace in Host.EnumerateDfsRoot(storage.ServerName, true))
                     {
                        Console.Write("\t#{0:000}\tDFS Root: [{1}]\n", ++cnt2, dfsNamespace);

                        //ShareInfo share = Host.GetShareInfo(storage.ServerName, storage.ShareName, true);
                        //Dump(share, -18);
                     }
                  }
               }
               catch (NetworkInformationException ex)
               {
                  Console.WriteLine("\n\tNetworkInformationException #1: [{0}]", ex.Message.Replace(Environment.NewLine, "  "));
               }
               catch (Exception ex)
               {
                  Console.WriteLine("\n\tException (1): [{0}]", ex.Message.Replace(Environment.NewLine, "  "));
               }
            }

            Console.Write("\n\t{0}", Reporter(true));
            Assert.IsTrue(cnt > 0, "Nothing was enumerated.");
         }
         catch (NetworkInformationException ex)
         {
            Console.WriteLine("\n\tNetworkInformationException #2: [{0}]", ex.Message.Replace(Environment.NewLine, "  "));
         }
         catch (Exception ex)
         {
            Console.WriteLine("\n\tException (2): [{0}]", ex.Message.Replace(Environment.NewLine, "  "));
         }

         Console.WriteLine("\n\n\t{0}", Reporter(true));
         Assert.IsTrue(cnt > 0, "Nothing was enumerated.");

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
         StopWatcher(true);
         try
         {
            foreach (string dfsNamespace in Host.EnumerateDomainDfsRoot())
               Console.Write("\n\t#{0:000}\tDFS Root: [{1}]", ++cnt, dfsNamespace);
         }
         catch (NetworkInformationException ex)
         {
            Console.WriteLine("\n\tNetworkInformationException: [{0}]", ex.Message.Replace(Environment.NewLine, "  "));
         }
         catch (Exception ex)
         {
            Console.WriteLine("\n\tException: [{0}]", ex.Message.Replace(Environment.NewLine, "  "));
         }

         Console.WriteLine("\n\n\t{0}", Reporter(true));
         Assert.IsTrue(cnt > 0, "Nothing was enumerated.");

         Console.WriteLine();
      }

      #endregion // EnumerateDomainDfsRoot

      #region EnumerateDrives

      [TestMethod]
      public void EnumerateDrives()
      {
         Console.WriteLine("Network.Host.EnumerateDrives()");

         DumpEnumerateDrives(true);
         DumpEnumerateDrives(false);
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

      #region GetDfsClientInfo

      [TestMethod]
      public void GetDfsClientInfo()
      {
         Console.WriteLine("Network.Host.GetDfsClientInfo()");

         int cnt = 0;
         StopWatcher(true);
         try
         {
            foreach (string dfsLink in Host.EnumerateDomainDfsRoot())
            {
               try
               {
                  foreach (string dir in Directory.EnumerateDirectories(dfsLink))
                  {
                     Console.Write("\n#{0:000}\tDFS Target Directory: [{1}]\n", ++cnt, dir);
                     StopWatcher(true);
                     Dump(Host.GetDfsClientInfo(dir).NumberOfStorages.First(), -10);
                     Console.Write("\n\t{0}\n", Reporter(true));
                     break;
                  }
               }
               catch (NetworkInformationException ex)
               {
                  Console.WriteLine("\n\tNetworkInformationException #1: [{0}]", ex.Message.Replace(Environment.NewLine, "  "));
               }
               catch (Exception ex)
               {
                  Console.WriteLine("\n\tException (1): [{0}]", ex.Message.Replace(Environment.NewLine, "  "));
               }
            }
         }
         catch (NetworkInformationException ex)
         {
            Console.WriteLine("\n\tNetworkInformationException #2: [{0}]", ex.Message.Replace(Environment.NewLine, "  "));
         }
         catch (Exception ex)
         {
            Console.WriteLine("\n\tException (2): [{0}]", ex.Message.Replace(Environment.NewLine, "  "));
         }

         Console.WriteLine("\n\n\t{0}", Reporter(true));
         Assert.IsTrue(cnt > 0, "Nothing was enumerated.");

         Console.WriteLine();
      }

      #endregion // GetDfsClientInfo

      #region GetDfsInfo

      [TestMethod]
      public void GetDfsInfo()
      {
         Console.WriteLine("Network.Host.GetDfsInfo()");
         Console.WriteLine("\nPlease see unit test: EnumerateDfsRoot()()");
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

         string share = Path.LocalToUnc(LocalHostShare);
         bool connectOk;
         Console.WriteLine("\nUsing IDisposable class.");
         try
         {
            StopWatcher(true);
            using (Host.DriveConnection connection = new Host.DriveConnection(share))
            {
               Console.WriteLine("\nUsing DriveConnection(): [{0}] to: [{1}]\n\n\t{2}\n", connection.LocalName, share, Reporter(true));
               connectOk = true;
            }
         }
         catch (Exception ex)
         {
            connectOk = false;
            Console.WriteLine("\nFailed DriveConnection() to: [{0}]", share);
            Console.WriteLine("\nCaught Exception: [{0}]", ex.Message.Replace(Environment.NewLine, "  "));
         }

         Assert.IsTrue(connectOk);

         #endregion // Using last available drive to share
      }

      #endregion // DriveConnection
   }
}