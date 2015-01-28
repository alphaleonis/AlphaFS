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

using Alphaleonis.Win32.Filesystem;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Directory = Alphaleonis.Win32.Filesystem.Directory;
using File = Alphaleonis.Win32.Filesystem.File;
using Path = Alphaleonis.Win32.Filesystem.Path;

namespace AlphaFS.UnitTest
{
   /// <summary>This is a test class for Shell32 and is intended to contain all Shell32 Unit Tests.</summary>
   [TestClass]
   public class AlphaFS_Shell32Test
   {
      #region DumpGetAssociation

      private static void DumpGetAssociation(bool isLocal)
      {
         Console.WriteLine("\n=== TEST {0} ===", isLocal ? UnitTestConstants.Local : UnitTestConstants.Network);
         string path = isLocal ? UnitTestConstants.SysRoot : Path.LocalToUnc(UnitTestConstants.SysRoot);

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

            UnitTestConstants.StopWatcher(true);
            Shell32Info shell32Info = Shell32.GetShell32Info(file);
            string report = UnitTestConstants.Reporter(true);

            string cmd = "print";
            verbCommand = shell32Info.GetVerbCommand(cmd);
            Console.WriteLine("\n\t\tShell32Info.GetVerbCommand(\"{0}\"): [{1}]", cmd, verbCommand);

            UnitTestConstants.Dump(shell32Info, -15);
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
         Console.WriteLine("\t\tShell32.PathFileExists() == [{0}]: {1}\t\t[{2}]", doesExist ? UnitTestConstants.TextTrue : UnitTestConstants.TextFalse, doesExist == fileExists, path);
         Console.WriteLine("\t\tFile.Exists()            == [{0}]: {1}\t\t[{2}]", doesExist ? UnitTestConstants.TextTrue : UnitTestConstants.TextFalse, doesExist == File.Exists(path), path);
         Console.WriteLine("\t\tDirectory.Exists()       == [{0}]: {1}\t\t[{2}]", doesExist ? UnitTestConstants.TextTrue : UnitTestConstants.TextFalse, doesExist == Directory.Exists(path), path);

         if (doesExist)
            Assert.IsTrue(fileExists);

         if (!doesExist)
            Assert.IsTrue(!fileExists);
      }

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

         Console.WriteLine("\nInput File Path: [{0}]\n", UnitTestConstants.NotepadExe);

         Console.WriteLine("Example usage:");
         Console.WriteLine("\n\tIntPtr icon = Shell32.GetFileIcon(file, FileAttributes.SmallIcon | FileAttributes.AddOverlays);");


         IntPtr icon = Shell32.GetFileIcon(UnitTestConstants.NotepadExe, Shell32.FileAttributes.SmallIcon | Shell32.FileAttributes.AddOverlays);

         Console.WriteLine("\n\tIcon Handle: [{0}]", icon);

         Assert.IsTrue(icon != IntPtr.Zero, "Failed retrieving icon for: [{0}]", UnitTestConstants.NotepadExe);
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
         Console.WriteLine("Filesystem.Shell32.PathCreateFromUrl()");

         string urlPath = Shell32.UrlCreateFromPath(UnitTestConstants.AppData);
         string filePath = Shell32.PathCreateFromUrl(urlPath);

         Console.WriteLine("\n\tDirectory                  : [{0}]", UnitTestConstants.AppData);
         Console.WriteLine("\n\tShell32.UrlCreateFromPath(): [{0}]", urlPath);
         Console.WriteLine("\n\tShell32.PathCreateFromUrl() == [{0}]\n", filePath);

         bool startsWith = urlPath.StartsWith("file:///");
         bool equalsAppData = filePath.Equals(UnitTestConstants.AppData);

         Console.WriteLine("\n\turlPath.StartsWith(\"file:///\") == [{0}]: {1}", UnitTestConstants.TextTrue, startsWith);
         Console.WriteLine("\n\tfilePath.Equals(AppData)       == [{0}]: {1}\n", UnitTestConstants.TextTrue, equalsAppData);
         Assert.IsTrue(startsWith);
         Assert.IsTrue(equalsAppData);
      }

      #endregion // PathCreateFromUrl

      #region PathCreateFromUrlAlloc

      [TestMethod]
      public void PathCreateFromUrlAlloc()
      {
         Console.WriteLine("Filesystem.Shell32.PathCreateFromUrlAlloc()");

         string urlPath = Shell32.UrlCreateFromPath(UnitTestConstants.AppData);
         string filePath = Shell32.PathCreateFromUrlAlloc(urlPath);

         Console.WriteLine("\n\tDirectory                  : [{0}]", UnitTestConstants.AppData);
         Console.WriteLine("\n\tShell32.UrlCreateFromPath(): [{0}]", urlPath);
         Console.WriteLine("\n\tShell32.PathCreateFromUrlAlloc() == [{0}]\n", filePath);

         bool startsWith = urlPath.StartsWith("file:///");
         bool equalsAppData = filePath.Equals(UnitTestConstants.AppData);

         Console.WriteLine("\n\turlPath.StartsWith(\"file:///\") == [{0}]: {1}", UnitTestConstants.TextTrue, startsWith);
         Console.WriteLine("\n\tfilePath.Equals(AppData)       == [{0}]: {1}\n", UnitTestConstants.TextTrue, equalsAppData);
         Assert.IsTrue(startsWith);
         Assert.IsTrue(equalsAppData);
      }

      #endregion // PathCreateFromUrlAlloc

      #region PathFileExists

      [TestMethod]
      public void PathFileExists()
      {
         Console.WriteLine("Filesystem.Shell32.PathFileExists()");

         string path = UnitTestConstants.SysRoot;
         DumpPathFileExists(path, true);
         DumpPathFileExists(Path.LocalToUnc(path), true);
         DumpPathFileExists("BlaBlaBla", false);
         DumpPathFileExists(Path.Combine(UnitTestConstants.SysRoot, "BlaBlaBla"), false);

         int cnt = 0;
         UnitTestConstants.StopWatcher(true);
         foreach (string file in Directory.EnumerateFiles(UnitTestConstants.SysRoot))
         {
            bool fileExists = Shell32.PathFileExists(file);

            Console.WriteLine("\t#{0:000}\tShell32.PathFileExists() == [{1}]: {2}\t\t[{3}]", ++cnt, UnitTestConstants.TextTrue, fileExists, file);
            Assert.IsTrue(fileExists);
         }
         Console.WriteLine("\n\t{0}\n", UnitTestConstants.Reporter(true));
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

         string urlPath = Shell32.UrlCreateFromPath(UnitTestConstants.AppData);
         string filePath = Shell32.PathCreateFromUrlAlloc(urlPath);

         bool isFileUrl1 = Shell32.UrlIsFileUrl(urlPath);
         bool isFileUrl2 = Shell32.UrlIsFileUrl(filePath);
         bool isNoHistory = Shell32.UrlIs(filePath, Shell32.UrlType.IsNoHistory);
         bool isOpaque = Shell32.UrlIs(filePath, Shell32.UrlType.IsOpaque);

         Console.WriteLine("\n\tDirectory: [{0}]", UnitTestConstants.AppData);
         Console.WriteLine("\n\tShell32.UrlCreateFromPath()      == IsFileUrl == [{0}] : {1}\t\t[{2}]", UnitTestConstants.TextTrue, isFileUrl1, urlPath);
         Console.WriteLine("\n\tShell32.PathCreateFromUrlAlloc() == IsFileUrl == [{0}]: {1}\t\t[{2}]", UnitTestConstants.TextFalse, isFileUrl2, filePath);

         Console.WriteLine("\n\tShell32.UrlIsFileUrl()   == [{0}]: {1}\t\t[{2}]", UnitTestConstants.TextTrue, isFileUrl1, urlPath);
         Console.WriteLine("\n\tShell32.UrlIsNoHistory() == [{0}]: {1}\t\t[{2}]", UnitTestConstants.TextTrue, isNoHistory, urlPath);
         Console.WriteLine("\n\tShell32.UrlIsOpaque()    == [{0}]: {1}\t\t[{2}]", UnitTestConstants.TextTrue, isOpaque, urlPath);
         
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