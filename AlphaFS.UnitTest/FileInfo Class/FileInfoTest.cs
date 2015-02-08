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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using FileInfo = Alphaleonis.Win32.Filesystem.FileInfo;
using Path = Alphaleonis.Win32.Filesystem.Path;

namespace AlphaFS.UnitTest
{
   /// <summary>This is a test class for FileInfo and is intended to contain all FileInfo UnitTests.</summary>
   [TestClass]
   public class FileInfoTest
   {
      #region Unit Tests

      #region DumpRefresh

      private void DumpRefresh(bool isLocal)
      {
         #region Setup

         Console.WriteLine("\n=== TEST {0} ===", isLocal ? UnitTestConstants.Local : UnitTestConstants.Network);

         string tempPathSysIo = Path.GetTempPath("FileInfo.Refresh()-file-SysIo-" + Path.GetRandomFileName());
         string tempPath = Path.GetTempPath("FileInfo.Refresh()-file-AlphaFS-" + Path.GetRandomFileName());
         if (!isLocal) tempPathSysIo = Path.LocalToUnc(tempPathSysIo);
         if (!isLocal) tempPath = Path.LocalToUnc(tempPath);

         Console.WriteLine("\nInput File Path: [{0}]", tempPath);

         #endregion // Setup

         #region Refresh

         try
         {
            System.IO.FileInfo fiSysIo = new System.IO.FileInfo(tempPathSysIo);
            FileInfo fi = new FileInfo(tempPath);

            bool existsSysIo = fiSysIo.Exists;
            bool exists = fi.Exists;
            Console.WriteLine("\nnew FileInfo(): Exists (Should be {0}): [{1}]", existsSysIo, exists); // false
            Assert.AreEqual(existsSysIo, exists);

            FileStream fsSysIo = fiSysIo.Create();
            FileStream fs = fi.Create();
            existsSysIo = fiSysIo.Exists;
            exists = fi.Exists;
            Console.WriteLine("\nfi.Create(): Exists (Should be {0}): [{1}]", existsSysIo, exists); // false
            Assert.AreEqual(existsSysIo, exists);

            fiSysIo.Refresh();
            fi.Refresh();
            existsSysIo = fiSysIo.Exists;
            exists = fi.Exists;
            Console.WriteLine("\nfi.Refresh(): Exists (Should be {0}): [{1}]", existsSysIo, exists); // true
            Assert.AreEqual(existsSysIo, exists);

            fsSysIo.Close();
            fs.Close();
            existsSysIo = fiSysIo.Exists;
            exists = fi.Exists;
            Console.WriteLine("\nfi.Close(): Exists (Should be {0}): [{1}]", existsSysIo, exists); // true
            Assert.AreEqual(existsSysIo, exists);

            fiSysIo.Delete();
            fi.Delete();
            existsSysIo = fiSysIo.Exists;
            exists = fi.Exists;
            Console.WriteLine("\nfi.Delete(): Exists (Should be {0}): [{1}]", existsSysIo, exists); // true
            Assert.AreEqual(existsSysIo, exists);
            
            fiSysIo.Refresh();
            fi.Refresh();
            existsSysIo = fiSysIo.Exists;
            exists = fi.Exists;
            Console.WriteLine("\nfi.Refresh(): Exists (Should be False): [{0}]", exists); // false
            Assert.AreEqual(existsSysIo, exists);
         }
         finally
         {
            File.Delete(tempPath);
            Assert.IsFalse(File.Exists(tempPath), "Cleanup failed: File should have been removed.");

            File.Delete(tempPathSysIo);
            Assert.IsFalse(File.Exists(tempPath), "Cleanup failed: File should have been removed.");
            Console.WriteLine();
         }

         #endregion // Refresh
      }

      #endregion // DumpRefresh

      #endregion // Unit Tests

      #region Refresh

      [TestMethod]
      public void Refresh()
      {
         Console.WriteLine("FileInfo.Refresh()");

         DumpRefresh(true);
         DumpRefresh(false);
      }

      #endregion // Refresh
   }
}
