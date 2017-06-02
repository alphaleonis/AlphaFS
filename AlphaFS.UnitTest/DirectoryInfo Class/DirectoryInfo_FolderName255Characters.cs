﻿/*  Copyright (C) 2008-2016 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
 *  
 *  Permission is hereby granted, free of charge, to any person obtaining a copy 
 *  of this software and associated documentation directorys (the "Software"), to deal 
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
   /// <summary>This is a test class for DirectoryInfo and is intended to contain all DirectoryInfo UnitTests.</summary>
   public partial class DirectoryInfoTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>

      [TestMethod]
      public void DirectoryInfo_CatchPathTooLongException_FolderNameGreaterThan255Characters_LocalAndNetwork_Success()
      {
         DirectoryInfo_CatchPathTooLongException_FolderNameGreaterThan255Characters(false);
         DirectoryInfo_CatchPathTooLongException_FolderNameGreaterThan255Characters(true);
      }


      [TestMethod]
      public void DirectoryInfo_FolderName255Characters_LocalAndNetwork_Success()
      {
         DirectoryInfo_FolderName255Characters(false);
         DirectoryInfo_FolderName255Characters(true);
      }




      private void DirectoryInfo_CatchPathTooLongException_FolderNameGreaterThan255Characters(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         var tempPath = System.IO.Path.GetTempPath();
         if (isNetwork)
            tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);


         using (var rootDir = new TemporaryDirectory(tempPath, "DirectoryInfo_CatchPathTooLongException_FolderNameGreaterThan255Characters"))
         {
            var folder = rootDir.Directory.FullName;


            Console.WriteLine("\nInput Directory Path: [{0}]\n", folder);


            // System.IO: 244, anything higher throws System.IO.PathTooLongException: The specified path, file name, or both are too long. The fully qualified file name must be less than 260 characters, and the directory name must be less than 248 characters.
            // AlphaFS  : 255, anything higher throws System.IO.PathTooLongException.
            var subFolder = new string('b', 256);


            var local = Alphaleonis.Win32.Filesystem.Path.Combine(folder, subFolder);
            var unc = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(local);
            Console.WriteLine("SubFolder length: {0}, total path length: {1}", subFolder.Length, isNetwork ? unc.Length : local.Length);
            Console.WriteLine();


            var gotException = false;

            try
            {
               // Fail.
               var di1 = new Alphaleonis.Win32.Filesystem.DirectoryInfo(isNetwork ? unc : local);
            }
            catch (Exception ex)
            {
               var exName = ex.GetType().Name;
               gotException = exName.Equals("PathTooLongException", StringComparison.OrdinalIgnoreCase);
               Console.WriteLine("\tCaught Exception (Expected): [{0}] Message: [{1}]", exName, ex.Message);
            }


            Assert.IsTrue(gotException, "The exception is not caught, but is expected to.");
         }

         Console.WriteLine();
      }


      private void DirectoryInfo_FolderName255Characters(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         var tempPath = System.IO.Path.GetTempPath();
         if (isNetwork)
            tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);


         using (var rootDir = new TemporaryDirectory(tempPath, "DirectoryInfo_FolderName255Characters"))
         {
            var folder = rootDir.Directory.FullName;


            Console.WriteLine("\nInput Directory Path: [{0}]\n", folder);


            // System.IO: 244, anything higher throws System.IO.PathTooLongException: The specified path, file name, or both are too long. The fully qualified file name must be less than 260 characters, and the directory name must be less than 248 characters.
            // AlphaFS  : 255, anything higher throws System.IO.PathTooLongException.
            var subFolder = new string('b', 255);


            var local = Alphaleonis.Win32.Filesystem.Path.Combine(folder, subFolder);
            var unc = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(local);
            Console.WriteLine("SubFolder length: {0}, total path length: {1}", subFolder.Length, isNetwork ? unc.Length : local.Length);

            // Success.
            var di1 = new Alphaleonis.Win32.Filesystem.DirectoryInfo(isNetwork ? unc : local);

            // Fail.
            //var di1 = new System.IO.DirectoryInfo(local);


            di1.Create();
            Assert.IsTrue(di1.Exists);


            UnitTestConstants.Dump(di1, -17);
         }

         Console.WriteLine();
      }
   }
}
