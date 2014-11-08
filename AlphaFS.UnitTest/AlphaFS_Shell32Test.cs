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
using Directory = Alphaleonis.Win32.Filesystem.Directory;
using File = Alphaleonis.Win32.Filesystem.File;
using FileInfo = Alphaleonis.Win32.Filesystem.FileInfo;
using Path = Alphaleonis.Win32.Filesystem.Path;

namespace AlphaFS.UnitTest
{
   /// <summary>This is a test class for Shell32 and is intended to contain all Shell32 Unit Tests.</summary>
   [TestClass]
   public class AlphaFS_Shell32Test
   {
      #region AlphaFS_Shell32Test Helpers

      private static readonly string StartupFolder = AppDomain.CurrentDomain.BaseDirectory;
      private static readonly string SysRoot = Environment.GetEnvironmentVariable("SystemRoot");
      private static readonly string SysRoot32 = Path.Combine(Environment.GetEnvironmentVariable("SystemRoot"), "System32");
      private static readonly string AppData = Environment.GetEnvironmentVariable("AppData");

      private const string SpecificX3 = "Windows XP and Windows Server 2003 specific.";
      private const string TextTrue = "IsTrue";
      private const string TextFalse = "IsFalse";

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

      #region Dumpers

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
               case "Directory":
                  if (obj.GetType().Namespace.Equals("Alphaleonis.Win32.Filesystem", StringComparison.OrdinalIgnoreCase))
                  {
                     if (obj != null)
                     {
                        FileInfo fi = (FileInfo)obj;
                        if (fi != null && fi.Directory != null)
                           propValue = fi.Directory.ToString();
                     }
                  }
                  break;

               case "EntryInfo":
                  if (obj.GetType().Namespace.Equals("Alphaleonis.Win32.Filesystem", StringComparison.OrdinalIgnoreCase))
                  {
                     if (obj != null)
                     {
                        FileInfo fi = (FileInfo)obj;
                        if (fi != null && fi.EntryInfo != null)
                           propValue = fi.EntryInfo.FullPath;
                     }
                  }
                  break;
            }

            Console.WriteLine(template, indent ? "\t" : "", ++cnt, descriptor.Name, propValue);
         }
      }

      #region DumpGetAssociation

      private static void DumpGetAssociation(bool isLocal)
      {
         Console.WriteLine("\n=== TEST {0} ===", isLocal ? "LOCAL" : "NETWORK");
         string path = isLocal ? SysRoot : Path.LocalToUnc(SysRoot);

         Console.WriteLine("\nInput Directory Path: [{0}]\n", path);

         int cnt = 0;
         foreach (string file in Directory.EnumerateFiles(path))
         {
            string association = Shell32.GetFileAssociation(file);
            string contentType = Shell32.GetFileContentType(file);
            string defaultIconPath = Shell32.GetFileDefaultIcon(file);
            string friendlyAppName = Shell32.GetFileFriendlyAppName(file);
            string friendlyDocName = Shell32.GetFileFriendlyDocName(file);
            string openWithApp = Shell32.GetFileOpenWithAppName(file);
            string verbCommand = Shell32.GetFileVerbCommand(file);

            Console.WriteLine("\t#{0:000}\t[{1}]\n", ++cnt, file);
            Console.WriteLine("\t\tGetFileAssociation()    : [{0}]", association);
            Console.WriteLine("\t\tGetFileContentType()    : [{0}]", contentType);
            Console.WriteLine("\t\tGetFileDefaultIcon()    : [{0}]", defaultIconPath);
            Console.WriteLine("\t\tGetFileFriendlyAppName(): [{0}]", friendlyAppName);
            Console.WriteLine("\t\tGetFileFriendlyDocName(): [{0}]", friendlyDocName);
            Console.WriteLine("\t\tGetFileOpenWithAppName(): [{0}]", openWithApp);
            Console.WriteLine("\t\tGetFileVerbCommand()    : [{0}]", verbCommand);
            
            StopWatcher(true);
            Shell32Info shell32Info = Shell32.GetShell32Info(file);
            string report = Reporter(true);

            string cmd = "print";
            verbCommand = shell32Info.GetVerbCommand(cmd);
            Console.WriteLine("\n\t\tShell32Info.GetVerbCommand(\"{0}\"): [{1}]", cmd, verbCommand);

            Dump(shell32Info, -15);
            Console.WriteLine("\n\t{0}\n\n", report);
         }
         Console.WriteLine("\n");
         Assert.IsTrue(cnt > 0, "No entries enumerated.");
      }

      #endregion // DumpGetAssociation

      private static void DumpPathFileExists(string path, bool doesExist)
      {
         Console.WriteLine("\n\tPath: [{0}]\n", path);

         bool fileExists = Shell32.PathFileExists(path);
         Console.WriteLine("\t\tShell32.PathFileExists() == [{0}]: {1}\t\t[{2}]", doesExist ? TextTrue : TextFalse, doesExist == fileExists, path);
         Console.WriteLine("\t\tFile.Exists()            == [{0}]: {1}\t\t[{2}]", doesExist ? TextTrue : TextFalse, doesExist == File.Exists(path), path);
         Console.WriteLine("\t\tDirectory.Exists()       == [{0}]: {1}\t\t[{2}]", doesExist ? TextTrue : TextFalse, doesExist == Directory.Exists(path), path);

         if (doesExist)
            Assert.IsTrue(fileExists);

         if (!doesExist)
            Assert.IsTrue(!fileExists);
      }

      #endregion // Dumpers

      #endregion // AlphaFS_Shell32Test Helpers
      
      #region GetFileAssociation

      [TestMethod]
      public void GetFileAssociation()
      {
         Console.WriteLine("Filesystem.Shell32.GetFileAssociation()");

         DumpGetAssociation(true);
         DumpGetAssociation(false);
      }

      #endregion // GetFileAssociation

      #region GetFileVerbCommand (Empty)

      [TestMethod]
      public void GetFileVerbCommand()
      {
         Console.WriteLine("Filesystem.Shell32.GetFileVerbCommand()");
         Console.WriteLine("\nPlease see unit test: GetFileAssociation()");
      }

      #endregion // GetFileVerbCommand (Empty)

      #region GetFileContentType (Empty)

      [TestMethod]
      public void GetFileContentType()
      {
         Console.WriteLine("Filesystem.Shell32.GetFileContentType()");
         Console.WriteLine("\nPlease see unit test: GetFileAssociation()");
      }

      #endregion // GetFileContentType (Empty)

      #region GetFileDefaultIcon (Empty)

      [TestMethod]
      public void GetFileDefaultIcon()
      {
         Console.WriteLine("Filesystem.Shell32.GetFileDefaultIcon()");
         Console.WriteLine("\nPlease see unit test: GetFileAssociation()");
      }

      #endregion // GetFileDefaultIcon (Empty)

      #region GetFileFriendlyAppName (Empty)

      [TestMethod]
      public void GetFileFriendlyAppName()
      {
         Console.WriteLine("Filesystem.Shell32.GetFileFriendlyAppName()");
         Console.WriteLine("\nPlease see unit test: GetFileAssociation()");
      }

      #endregion // GetFileFriendlyAppName (Empty)

      #region GetFileFriendlyDocName (Empty)

      [TestMethod]
      public void GetFileFriendlyDocName()
      {
         Console.WriteLine("Filesystem.Shell32.GetFileFriendlyDocName()");
         Console.WriteLine("\nPlease see unit test: GetFileAssociation()");
      }

      #endregion // GetFileFriendlyDocName (Empty)

      #region GetFileIcon

      [TestMethod]
      public void GetFileIcon()
      {
         Console.WriteLine("Filesystem.Shell32.GetFileIcon()");

         string notepad = Path.Combine(SysRoot32, "notepad.exe");
         Console.WriteLine("\nInput File Path: [{0}]\n", notepad);

         Console.WriteLine("Example usage:");
         Console.WriteLine("\n\tIntPtr icon = Shell32.GetFileIcon(file, FileAttributes.SmallIcon | FileAttributes.AddOverlays);");

         
         IntPtr icon = Shell32.GetFileIcon(notepad, Shell32.FileAttributes.SmallIcon | Shell32.FileAttributes.AddOverlays);

         Console.WriteLine("\n\tIcon Handle: [{0}]", icon);

         Assert.IsTrue(icon != IntPtr.Zero, "Failed retrieving icon for: [{0}]", notepad);
      }

      #endregion // GetFileIcon

      #region GetFileInfo

      [TestMethod]
      public void GetFileInfo()
      {
         Console.WriteLine("Filesystem.Shell32.GetFileInfo()");
         Console.WriteLine("\nPlease see unit test: GetFileAssociation()");
      }

      #endregion // GetFileInfo

      #region GetFileOpenWithAppName (Empty)

      [TestMethod]
      public void GetFileOpenWithAppName()
      {
         Console.WriteLine("Filesystem.Shell32.GetFileOpenWithAppName()");
         Console.WriteLine("\nPlease see unit test: GetFileAssociation()");
      }

      #endregion // GetFileOpenWithAppName (Empty)

      #region PathCreateFromUrl

      [TestMethod]
      public void PathCreateFromUrl()
      {
         Console.WriteLine("Filesystem.Shell32.PathCreateFromUrl() - {0}", SpecificX3);

         string urlPath = Shell32.UrlCreateFromPath(AppData);
         string filePath = Shell32.PathCreateFromUrl(urlPath);

         Console.WriteLine("\n\tDirectory                  : [{0}]", AppData);
         Console.WriteLine("\n\tShell32.UrlCreateFromPath(): [{0}]", urlPath);
         Console.WriteLine("\n\tShell32.PathCreateFromUrl() == [{0}]\n", filePath);

         bool startsWith = urlPath.StartsWith("file:///");
         bool equalsAppData = filePath.Equals(AppData);

         Console.WriteLine("\n\turlPath.StartsWith(\"file:///\") == [{0}]: {1}", TextTrue, startsWith);
         Console.WriteLine("\n\tfilePath.Equals(AppData)       == [{0}]: {1}\n", TextTrue, equalsAppData);
         Assert.IsTrue(startsWith);
         Assert.IsTrue(equalsAppData);
      }

      #endregion // PathCreateFromUrl

      #region PathCreateFromUrlAlloc

      [TestMethod]
      public void PathCreateFromUrlAlloc()
      {
         Console.WriteLine("Filesystem.Shell32.PathCreateFromUrlAlloc()");

         string urlPath = Shell32.UrlCreateFromPath(AppData);
         string filePath = Shell32.PathCreateFromUrlAlloc(urlPath);

         Console.WriteLine("\n\tDirectory                  : [{0}]", AppData);
         Console.WriteLine("\n\tShell32.UrlCreateFromPath(): [{0}]", urlPath);
         Console.WriteLine("\n\tShell32.PathCreateFromUrlAlloc() == [{0}]\n", filePath);

         bool startsWith = urlPath.StartsWith("file:///");
         bool equalsAppData = filePath.Equals(AppData);

         Console.WriteLine("\n\turlPath.StartsWith(\"file:///\") == [{0}]: {1}", TextTrue, startsWith);
         Console.WriteLine("\n\tfilePath.Equals(AppData)       == [{0}]: {1}\n", TextTrue, equalsAppData);
         Assert.IsTrue(startsWith);
         Assert.IsTrue(equalsAppData);
      }

      #endregion // PathCreateFromUrlAlloc

      #region PathFileExists

      [TestMethod]
      public void PathFileExists()
      {
         Console.WriteLine("Filesystem.Shell32.PathFileExists()");

         string path = SysRoot;
         DumpPathFileExists(path, true);
         DumpPathFileExists(Path.LocalToUnc(path), true);
         DumpPathFileExists("BlaBlaBla", false);
         DumpPathFileExists(Path.Combine(SysRoot, "BlaBlaBla"), false);

         int cnt = 0;
         StopWatcher(true);
         foreach (string file in Directory.EnumerateFiles(SysRoot))
         {
            bool fileExists = Shell32.PathFileExists(file);

            Console.WriteLine("\t#{0:000}\tShell32.PathFileExists() == [{1}]: {2}\t\t[{3}]", ++cnt, TextTrue, fileExists, file);
            Assert.IsTrue(fileExists);
         }
         Console.WriteLine("\n\t{0}\n", Reporter(true));
      }

      #endregion // PathFileExists
      
      #region UrlCreateFromPath

      [TestMethod]
      public void UrlCreateFromPath()
      {
         Console.WriteLine("Filesystem.Shell32.UrlCreateFromPath()");

         PathCreateFromUrl();
         PathCreateFromUrlAlloc();
      }

      #endregion // UrlCreateFromPath

      #region UrlIs

      [TestMethod]
      public void UrlIs()
      {
         Console.WriteLine("Filesystem.Shell32.UrlIs()");

         string urlPath = Shell32.UrlCreateFromPath(AppData);
         string filePath = Shell32.PathCreateFromUrlAlloc(urlPath);

         bool isFileUrl1 = Shell32.UrlIsFileUrl(urlPath);
         bool isFileUrl2 = Shell32.UrlIsFileUrl(filePath);
         bool isNoHistory = Shell32.UrlIs(filePath, Shell32.UrlType.IsNoHistory);
         bool isOpaque = Shell32.UrlIs(filePath, Shell32.UrlType.IsOpaque);

         Console.WriteLine("\n\tDirectory: [{0}]", AppData);
         Console.WriteLine("\n\tShell32.UrlCreateFromPath()      == IsFileUrl == [{0}] : {1}\t\t[{2}]", TextTrue, isFileUrl1, urlPath);
         Console.WriteLine("\n\tShell32.PathCreateFromUrlAlloc() == IsFileUrl == [{0}]: {1}\t\t[{2}]", TextFalse, isFileUrl2, filePath);

         Console.WriteLine("\n\tShell32.UrlIsFileUrl()   == [{0}]: {1}\t\t[{2}]", TextTrue, isFileUrl1, urlPath);
         Console.WriteLine("\n\tShell32.UrlIsNoHistory() == [{0}]: {1}\t\t[{2}]", TextTrue, isNoHistory, urlPath);
         Console.WriteLine("\n\tShell32.UrlIsOpaque()    == [{0}]: {1}\t\t[{2}]", TextTrue, isOpaque, urlPath);
         
         Assert.IsTrue(isFileUrl1);
         Assert.IsTrue(isFileUrl2 == false);
      }

      #endregion // UrlIs

      #region UrlIsFileUrl (Empty)

      [TestMethod]
      public void UrlIsFileUrl()
      {
         Console.WriteLine("Filesystem.Shell32.UrlIsFileUrl()");
         Console.WriteLine("\nPlease see unit test: UrlIs()");
      }

      #endregion // UrlIsFileUrl (Empty)

      #region UrlIsNoHistory (Empty)

      [TestMethod]
      public void UrlIsNoHistory()
      {
         Console.WriteLine("Filesystem.Shell32.UrlIsNoHistory()");
         Console.WriteLine("\nPlease see unit test: UrlIs()");
      }

      #endregion // UrlIsNoHistory (Empty)

      #region UrlIsOpaque (Empty)

      [TestMethod]
      public void UrlIsOpaque()
      {
         Console.WriteLine("Filesystem.Shell32.UrlIsOpaque()");
         Console.WriteLine("\nPlease see unit test: UrlIs()");
      }

      #endregion // UrlIsOpaque (Empty)
   }
}