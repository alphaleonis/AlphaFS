/* Copyright (c) 2008-2015 Peter Palotas, Jeffrey Jangli, Normalex
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

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AlphaFS.UnitTest
{
   /// <summary>This is a test class for DriveInfo and is intended to contain all DriveInfo Unit Tests.</summary>
   [TestClass]
   public class DriveInfoTest
   {
      #region DriveInfoTest Helpers

      #endregion // DriveInfoTest Helpers
      
      #region .NET

      #region GetDrives

      [TestMethod]
      public void GetDrives()
      {
         Console.WriteLine("DriveInfo.GetDrives()");
         Console.WriteLine("\nPlease see unit test: Directory.GetLogicalDrives()");
      }

      #endregion // GetDrives

      #endregion // .NET

      #region AlphaFS

      #region EnumerateDrives

      [TestMethod]
      public void AlphaFS_EnumerateDrives()
      {
         Console.WriteLine("DriveInfo.EnumerateDrives()");
         Console.WriteLine("\nPlease see unit test: Directory.EnumerateLogicalDrives()");
      }

      #endregion // EnumerateDrives

      #region GetFreeDriveLetter

      [TestMethod]
      public void AlphaFS_GetFreeDriveLetter()
      {
         Console.WriteLine("DriveInfo.GetFreeDriveLetter()");
         Console.WriteLine("\nPlease see unit test: Network.Host.ConnectDrive()");
         Console.WriteLine("\nPlease see unit test: Volume.DefineDosDevice()");
         Console.WriteLine("\nPlease see unit test: DirectoryInfo()");
         
      }

      #endregion // EnumerateDrives

      #endregion // AlphaFS
   }
}