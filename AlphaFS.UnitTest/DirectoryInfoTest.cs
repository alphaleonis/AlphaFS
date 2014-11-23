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

using Alphaleonis.Win32.Filesystem;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using DirectoryInfo = Alphaleonis.Win32.Filesystem.DirectoryInfo;
using Path = Alphaleonis.Win32.Filesystem.Path;

namespace AlphaFS.UnitTest
{
   /// <summary>This is a test class for DirectoryInfo and is intended to contain all DirectoryInfo Unit Tests.</summary>
   [TestClass]
   public class DirectoryInfoTest
   {
      #region Unit Test Helpers

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


            switch (descriptor.Name)
            {
               case "Parent":
                  if (obj.GetType().Namespace.Equals("Alphaleonis.Win32.Filesystem", StringComparison.OrdinalIgnoreCase))
                  {
                     if (obj != null)
                     {
                        DirectoryInfo di = (DirectoryInfo) obj;
                        if (di != null)
                           propValue = di.Parent == null ? null : di.Parent.ToString();
                     }
                  }
                  break;

               case "Root":
                  if (obj.GetType().Namespace.Equals("Alphaleonis.Win32.Filesystem", StringComparison.OrdinalIgnoreCase))
                  {
                     if (obj != null)
                     {
                        DirectoryInfo di = (DirectoryInfo) obj;
                        if (di != null)
                           propValue = di.Root.ToString();
                     }
                  }
                  break;

               case "EntryInfo":
                  if (obj.GetType().Namespace.Equals("Alphaleonis.Win32.Filesystem", StringComparison.OrdinalIgnoreCase))
                  {
                     if (obj != null)
                     {
                        DirectoryInfo di = (DirectoryInfo) obj;
                        if (di != null && di.EntryInfo != null)
                           propValue = di.EntryInfo.FullPath;
                     }
                  }
                  break;
            }

            Console.WriteLine(template, indent ? "\t" : "", ++cnt, descriptor.Name, propValue);
         }
      }

      #region Fields

      private readonly string LocalHost = Environment.MachineName;
      private readonly string LocalHostShare = Environment.SystemDirectory;
      private readonly bool _testMyServer = Environment.UserName.Equals(@"jjangli", StringComparison.OrdinalIgnoreCase);
      private const string MyServer = "yomodo";
      private const string MyServerShare = @"\\" + MyServer + @"\video";
      private const string Local = @"LOCAL";
      private const string Network = @"NETWORK";

      private static readonly string SysDrive = Environment.GetEnvironmentVariable("SystemDrive");
      private static readonly string SysRoot = Environment.GetEnvironmentVariable("SystemRoot");
      private static readonly string SysRoot32 = Path.Combine(SysRoot, "System32");

      private const string TextTrue = "IsTrue";
      private const string TenNumbers = "0123456789";
      private const string TextHelloWorld = "Hëllõ Wørld!";
      private const string TextGoodByeWorld = "GóödByé Wôrld!";
      private const string TextAppend = "GóödByé Wôrld!";
      private const string TextUnicode = "ÛņïÇòdè; ǖŤƑ";

      #endregion // Fields

      #endregion // Unit Test Helpers

      #region Unit Tests

      #region DumpRefresh

      private void DumpRefresh(bool isLocal)
      {
         #region Setup

         Console.WriteLine("\n=== TEST {0} ===", isLocal ? Local : Network);

         string tempPathSysIo = Path.GetTempPath("DirectoryInfo.Refresh()-directory-SysIo-" + Path.GetRandomFileName());
         string tempPath = Path.GetTempPath("DirectoryInfo.Refresh()-directory-AlphaFS-" + Path.GetRandomFileName());
         if (!isLocal) tempPathSysIo = Path.LocalToUnc(tempPathSysIo);
         if (!isLocal) tempPath = Path.LocalToUnc(tempPath);

         Console.WriteLine("\nInput Directory Path: [{0}]", tempPath);

         #endregion // Setup

         #region Refresh

         try
         {
            System.IO.DirectoryInfo diSysIo = new System.IO.DirectoryInfo(tempPathSysIo);
            DirectoryInfo di = new DirectoryInfo(tempPath);

            bool existsSysIo = diSysIo.Exists;
            bool exists = di.Exists;
            Console.WriteLine("\nnew DirectoryInfo(): Exists (Should be {0}): [{1}]", existsSysIo, exists); // false
            Assert.AreEqual(existsSysIo, exists);

            diSysIo.Create();
            di.Create();
            existsSysIo = diSysIo.Exists;
            exists = di.Exists;
            Console.WriteLine("\ndi.Create(): Exists (Should be {0}): [{1}]", existsSysIo, exists); // false
            Assert.AreEqual(existsSysIo, exists);

            diSysIo.Refresh();
            di.Refresh();
            existsSysIo = diSysIo.Exists;
            exists = di.Exists;
            Console.WriteLine("\ndi.Refresh(): Exists (Should be {0}): [{1}]", existsSysIo, exists); // true
            Assert.AreEqual(existsSysIo, exists);

            diSysIo.Delete();
            di.Delete();
            existsSysIo = diSysIo.Exists;
            exists = di.Exists;
            Console.WriteLine("\ndi.Delete(): Exists (Should be {0}): [{1}]", existsSysIo, exists); // true
            Assert.AreEqual(existsSysIo, exists);

            diSysIo.Refresh();
            di.Refresh();
            existsSysIo = diSysIo.Exists;
            exists = di.Exists;
            Console.WriteLine("\ndi.Refresh(): Exists (Should be {0}): [{1}]", existsSysIo, exists); // false
            Assert.AreEqual(existsSysIo, exists);
         }
         finally
         {
            if (Directory.Exists(tempPathSysIo))
            {
               Directory.Delete(tempPathSysIo);
               Assert.IsFalse(Directory.Exists(tempPathSysIo), "Cleanup failed: Directory should have been removed.");
            }

            if (Directory.Exists(tempPath))
            {
               Directory.Delete(tempPath);
               Assert.IsFalse(Directory.Exists(tempPath), "Cleanup failed: Directory should have been removed.");
            }

            Console.WriteLine();
         }

         #endregion // Refresh
      }

      #endregion // DumpRefresh

      #endregion // Unit Tests

      #region Unit Test Callers

      // Note: Most of these unit tests are empty and are here to confirm AlphaFS implementation.

      #region .NET
      
      #region Create

      [TestMethod]
      public void Create()
      {
         Console.WriteLine("DirectoryInfo.Create()");
         Console.WriteLine("\nPlease see unit tests from class: Directory().");
      }

      #endregion // Create

      #region CreateSubdirectory

      [TestMethod]
      public void CreateSubdirectory()
      {
         Console.WriteLine("DirectoryInfo.CreateSubdirectory()");
         Console.WriteLine("\nPlease see unit tests from class: Directory().");
      }

      #endregion // CreateSubdirectory

      #region Delete

      [TestMethod]
      public void Delete()
      {
         Console.WriteLine("DirectoryInfo.Delete()");
         Console.WriteLine("\nPlease see unit tests from class: Directory().");
      }

      #endregion // Delete

      #region Exists

      [TestMethod]
      public void Exists()
      {
         Console.WriteLine("DirectoryInfo.Exists()");
         Console.WriteLine("\nPlease see unit test: Refresh().");
      }

      #endregion // Exists

      #region EnumerateDirectories

      [TestMethod]
      public void EnumerateDirectories()
      {
         Console.WriteLine("DirectoryInfo.EnumerateDirectories()");
         Console.WriteLine("\nPlease see unit tests from class: Directory().");
      }

      #endregion // EnumerateDirectories

      #region EnumerateFiles

      [TestMethod]
      public void EnumerateFiles()
      {
         Console.WriteLine("DirectoryInfo.EnumerateFiles()");
         Console.WriteLine("\nPlease see unit tests from class: Directory().");
      }

      #endregion // EnumerateFiles

      #region EnumerateFiles

      [TestMethod]
      public void EnumerateFileSystemInfos()
      {
         Console.WriteLine("DirectoryInfo.EnumerateFileSystemInfos()");
         Console.WriteLine("\nPlease see unit tests from class: Directory().");
      }

      #endregion // EnumerateFileSystemInfos

      #region GetAccessControl

      [TestMethod]
      public void GetAccessControl()
      {
         Console.WriteLine("DirectoryInfo.GetAccessControl()");
         Console.WriteLine("\nPlease see unit tests from class: Directory().");
      }

      #endregion // GetAccessControl

      #region GetDirectories

      [TestMethod]
      public void GetDirectories()
      {
         Console.WriteLine("DirectoryInfo.GetDirectories()");
         Console.WriteLine("\nPlease see unit tests from class: Directory().");
      }

      #endregion // GetDirectories

      #region GetFiles

      [TestMethod]
      public void GetFiles()
      {
         Console.WriteLine("DirectoryInfo.GetFiles()");
         Console.WriteLine("\nPlease see unit tests from class: Directory().");
      }

      #endregion // GetFiles

      #region GetFileSystemInfos

      [TestMethod]
      public void GetFileSystemInfos()
      {
         Console.WriteLine("DirectoryInfo.GetFileSystemInfos()");
         Console.WriteLine("\nPlease see unit tests from class: Directory().");
      }

      #endregion // GetFileSystemInfos

      #region MoveTo

      [TestMethod]
      public void MoveTo()
      {
         Console.WriteLine("DirectoryInfo.MoveTo()");
         Console.WriteLine("\nPlease see unit tests from class: Directory().");
      }

      #endregion // MoveTo

      #region Refresh

      [TestMethod]
      public void Refresh()
      {
         Console.WriteLine("DirectoryInfo.Refresh()");

         DumpRefresh(true);
         DumpRefresh(false);
      }

      #endregion // Refresh

      #region SetAccessControl

      [TestMethod]
      public void SetAccessControl()
      {
         Console.WriteLine("DirectoryInfo.SetAccessControl()");
         Console.WriteLine("\nPlease see unit tests from class: Directory().");
      }

      #endregion // SetAccessControl

      #endregion // .NET

      #region AlphaFS

      #region AddStream

      [TestMethod]
      public void AlphaFS_AddStream()
      {
         Console.WriteLine("DirectoryInfo.AddStream()");
         Console.WriteLine("\nPlease see unit tests from class: Directory().");
      }

      #endregion // AddStream

      #region CopyTo1

      [TestMethod]
      public void AlphaFS_CopyTo1()
      {
         Console.WriteLine("DirectoryInfo.CopyTo1()");
         Console.WriteLine("\nPlease see unit tests from class: Directory().");
      }

      #endregion // CopyTo

      #region CountDirectories

      [TestMethod]
      public void AlphaFS_CountDirectories()
      {
         Console.WriteLine("DirectoryInfo.CopyTo()");
         Console.WriteLine("\nPlease see unit tests from class: Directory().");
      }

      #endregion // CountDirectories

      #region CountFiles

      [TestMethod]
      public void AlphaFS_CountFiles()
      {
         Console.WriteLine("DirectoryInfo.CountFiles()");
         Console.WriteLine("\nPlease see unit tests from class: Directory().");
      }

      #endregion // CountFiles

      #region Compress

      [TestMethod]
      public void AlphaFS_Compress()
      {
         Console.WriteLine("DirectoryInfo.Compress()");
         Console.WriteLine("\nPlease see unit tests from class: Directory().");
      }

      #endregion // Compress

      #region DisableCompression

      [TestMethod]
      public void AlphaFS_DisableCompression()
      {
         Console.WriteLine("DirectoryInfo.DisableCompression()");
         Console.WriteLine("\nPlease see unit tests from class: Directory().");
      }

      #endregion // DisableCompression

      #region DisableEncryption

      [TestMethod]
      public void AlphaFS_DisableEncryption()
      {
         Console.WriteLine("DirectoryInfo.DisableEncryption()");
         Console.WriteLine("\nPlease see unit tests from class: Directory().");
      }

      #endregion // DisableEncryption

      #region Decompress

      [TestMethod]
      public void AlphaFS_Decompress()
      {
         Console.WriteLine("DirectoryInfo.Decompress()");
         Console.WriteLine("\nPlease see unit tests from class: Directory().");
      }

      #endregion // Decompress

      #region Decrypt

      [TestMethod]
      public void AlphaFS_Decrypt()
      {
         Console.WriteLine("DirectoryInfo.Decrypt()");
         Console.WriteLine("\nPlease see unit tests from class: Directory().");
      }

      #endregion // Decrypt

      #region DeleteEmpty

      [TestMethod]
      public void AlphaFS_DeleteEmpty()
      {
         Console.WriteLine("DirectoryInfo.DeleteEmpty()");
         Console.WriteLine("\nPlease see unit tests from class: Directory().");
      }

      #endregion // DeleteEmpty

      #region EnableCompression

      [TestMethod]
      public void AlphaFS_EnableCompression()
      {
         Console.WriteLine("DirectoryInfo.EnableCompression()");
         Console.WriteLine("\nPlease see unit tests from class: Directory().");
      }

      #endregion // EnableCompression

      #region EnableEncryption

      [TestMethod]
      public void AlphaFS_EnableEncryption()
      {
         Console.WriteLine("DirectoryInfo.EnableEncryption()");
         Console.WriteLine("\nPlease see unit tests from class: Directory().");
      }

      #endregion // EnableEncryption

      #region Encrypt

      [TestMethod]
      public void AlphaFS_Encrypt()
      {
         Console.WriteLine("DirectoryInfo.Encrypt()");
         Console.WriteLine("\nPlease see unit tests from class: Directory().");
      }

      #endregion // Encrypt

      #region EnumerateStreams

      [TestMethod]
      public void AlphaFS_EnumerateStreams()
      {
         Console.WriteLine("DirectoryInfo.EnumerateStreams()");
         Console.WriteLine("\nPlease see unit tests from class: Directory().");
      }

      #endregion // EnumerateStreams

      #region GetStreamSize

      [TestMethod]
      public void AlphaFS_GetStreamSize()
      {
         Console.WriteLine("DirectoryInfo.GetStreamSize()");
         Console.WriteLine("\nPlease see unit tests from class: Directory().");
      }

      #endregion // GetStreamSize

      #region AlphaFS_MoveTo1

      [TestMethod]
      public void AlphaFS_MoveTo1()
      {
         Console.WriteLine("DirectoryInfo.MoveTo1()");
         Console.WriteLine("\nPlease see unit tests from class: Directory().");
      }

      #endregion // AlphaFS_MoveTo1

      #region RemoveStream

      [TestMethod]
      public void AlphaFS_RemoveStream()
      {
         Console.WriteLine("DirectoryInfo.RemoveStream()");
         Console.WriteLine("\nPlease see unit tests from class: Directory().");
      }

      #endregion // RemoveStream

      #endregion // AlphaFS

      
      #region SetSecurity (Issue-19788)

      private static string GetTempDirectoryName()
      {
         return Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
      }

      [TestMethod]
      public void Test_AlphaFS___SetSecurity_ShouldNotProduceError()
      {
         SetSecurityAlpha(GetTempDirectoryName());
      }

      [TestMethod]
      public void Test_System___SetSecurity_ShouldNotProduceError()
      {
         SetSecuritySystem(GetTempDirectoryName());
      }

      [TestMethod]
      public void Test_System___AnalyzeSecurity_LocalAcessShouldNotExist()
      {
         string testDir = GetTempDirectoryName();
         SetSecuritySystem(testDir);
         var dirsec = new System.IO.DirectoryInfo(testDir + @"\inherited").GetAccessControl();
         AuthorizationRuleCollection accessRules = dirsec.GetAccessRules(true, true, targetType: typeof(SecurityIdentifier));
         Assert.IsFalse(HasLocalAces(accessRules), "local access rules found");
      }

      [TestMethod]
      public void Test_AlphaFS___AnalyzeSecurity_LocalAcessShouldNotExist()
      {
         string testDir = GetTempDirectoryName();
         SetSecuritySystem(testDir);
         var dirsec = new DirectoryInfo(testDir + @"\inherited").GetAccessControl();
         AuthorizationRuleCollection accessRules = dirsec.GetAccessRules(true, true, targetType: typeof(SecurityIdentifier));
         Assert.IsFalse(HasLocalAces(accessRules), "local access rules found");
      }


      public static bool HasLocalAces(AuthorizationRuleCollection rules)
      {
         bool res = false;

         AccessRule locaACE = rules.Cast<AccessRule>().FirstOrDefault(a => a.IsInherited == false);
         res = (locaACE == null ? false : true);
         return res;
      }

      private static void SetSecurityAlpha(string directory)
      {
         //create the test structure
         if (Directory.Exists(directory))
            Directory.Delete(directory, true);

         Directory.CreateDirectory(directory);
         Directory.CreateDirectory(System.IO.Path.Combine(directory, "inherited"));
         DirectoryInfo testDirInfo = new DirectoryInfo(directory);
         //System.IO.Directory.CreateDirectory(_testDir);
         //System.IO.Directory.CreateDirectory(System.IO.Path.Combine(_testDir, "inherited"));
         //System.IO.DirectoryInfo testDirInfo = new System.IO.DirectoryInfo(_testDir);

         var ds = testDirInfo.GetAccessControl(AccessControlSections.Access);
         ds.SetAccessRuleProtection(true, false);
         ds.AddAccessRule(new FileSystemAccessRule(
               identity: new SecurityIdentifier(WellKnownSidType.WorldSid, null),
               fileSystemRights: System.Security.AccessControl.FileSystemRights.FullControl,
               inheritanceFlags: InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit,
               propagationFlags: PropagationFlags.None,
               type: AccessControlType.Allow
               ));

         //using (new PrivilegeEnabler(Privilege.Impersonate, Privilege.Backup, Privilege.Restore, Privilege.Security, Privilege.TakeOwnership, Privilege.TrustedCredManAccess, Privilege.Audit))
         {
            testDirInfo.SetAccessControl(ds);

         }
      }

      private static void SetSecuritySystem(string directory)
      {
         //create the test structure
         if (System.IO.Directory.Exists(directory))
            System.IO.Directory.Delete(directory, true);

         System.IO.Directory.CreateDirectory(directory);
         System.IO.Directory.CreateDirectory(System.IO.Path.Combine(directory, "inherited"));
         System.IO.DirectoryInfo testDirInfo = new System.IO.DirectoryInfo(directory);

         var ds = testDirInfo.GetAccessControl(AccessControlSections.Access);
         ds.SetAccessRuleProtection(true, false);
         ds.AddAccessRule(new FileSystemAccessRule(
           identity: new SecurityIdentifier(WellKnownSidType.WorldSid, null),
           fileSystemRights: System.Security.AccessControl.FileSystemRights.FullControl,
           inheritanceFlags: InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit,
           propagationFlags: PropagationFlags.None,
           type: AccessControlType.Allow
           ));
         testDirInfo.SetAccessControl(ds);
      }

      #endregion

      #endregion // Unit Test Callers
   }
}