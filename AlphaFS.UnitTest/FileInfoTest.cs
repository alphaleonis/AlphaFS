/* Copyright (c) 2008-2014 Peter Palotas, Alexandr Normuradov, Jeffrey Jangli
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
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using File = Alphaleonis.Win32.Filesystem.File;
using FileInfo = Alphaleonis.Win32.Filesystem.FileInfo;
using Path = Alphaleonis.Win32.Filesystem.Path;

namespace AlphaFS.UnitTest
{
   /// <summary>This is a test class for FileInfo and is intended to contain all FileInfo UnitTests.</summary>
   [TestClass]
   public class FileInfoTest
   {
      #region FileInfoTest Helpers

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
               propValue = ex.Message.Replace(Environment.NewLine, string.Empty);
            }


            switch (descriptor.Name)
            {
               case "Directory":
                  if (obj.GetType().Namespace.Equals("Alphaleonis.Win32.Filesystem", StringComparison.OrdinalIgnoreCase))
                  {
                     if (obj != null)
                     {
                        FileInfo fi = (FileInfo) obj;
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
                        FileInfo fi = (FileInfo) obj;
                        if (fi != null && fi.EntryInfo != null)
                           propValue = fi.EntryInfo.FullPath;
                     }
                  }
                  break;
            }

            Console.WriteLine(template, indent ? "\t" : "", ++cnt, descriptor.Name, propValue);
         }
      }
      
      #endregion // FileInfoTest Helpers

      #region .NET
      
      // Note: These unit tests are empty and are here to confirm AlphaFS implementation.

      #region AppendText

      [TestMethod]
      public void AppendText()
      {
         Console.WriteLine("FileInfo.AppendText()");
         Console.WriteLine("\nPlease see unit tests from class: File().");
      }

      #endregion // AppendText

      #region CopyTo

      [TestMethod]
      public void CopyTo()
      {
         Console.WriteLine("FileInfo.CopyTo()");
         Console.WriteLine("\nPlease see unit tests from class: File().");
      }

      #endregion // CopyTo

      #region Create

      [TestMethod]
      public void Create()
      {
         Console.WriteLine("FileInfo.Create()");
         Console.WriteLine("\nPlease see unit tests from class: File().");
      }

      #endregion // Create

      #region CreateText

      [TestMethod]
      public void CreateText()
      {
         Console.WriteLine("FileInfo.CreateText()");
         Console.WriteLine("\nPlease see unit tests from class: File().");
      }

      #endregion // CreateText

      #region Decrypt

      [TestMethod]
      public void Decrypt()
      {
         Console.WriteLine("FileInfo.Decrypt()");
         Console.WriteLine("\nPlease see unit tests from class: File().");
      }

      #endregion // Decrypt

      #region Delete

      [TestMethod]
      public void Delete()
      {
         Console.WriteLine("FileInfo.Delete()");
         Console.WriteLine("\nPlease see unit tests from class: File().");
      }

      #endregion // Delete

      #region Encrypt

      [TestMethod]
      public void Encrypt()
      {
         Console.WriteLine("FileInfo.Encrypt()");
         Console.WriteLine("\nPlease see unit tests from class: File().");
      }

      #endregion // Encrypt

      #region GetAccessControl

      [TestMethod]
      public void GetAccessControl()
      {
         Console.WriteLine("FileInfo.GetAccessControl()");
         Console.WriteLine("\nPlease see unit tests from class: File().");
      }

      #endregion // GetAccessControl

      #region MoveTo

      [TestMethod]
      public void MoveTo()
      {
         Console.WriteLine("FileInfo.MoveTo()");
         Console.WriteLine("\nPlease see unit tests from class: File().");
      }

      #endregion // MoveTo

      #region Open

      [TestMethod]
      public void Open()
      {
         Console.WriteLine("FileInfo.Open()");
         Console.WriteLine("\nPlease see unit tests from class: File().");
      }

      #endregion // Open

      #region OpenRead

      [TestMethod]
      public void OpenRead()
      {
         Console.WriteLine("FileInfo.OpenRead()");
         Console.WriteLine("\nPlease see unit tests from class: File().");
      }

      #endregion // OpenRead

      #region OpenText

      [TestMethod]
      public void OpenText()
      {
         Console.WriteLine("FileInfo.OpenText()");
         Console.WriteLine("\nPlease see unit tests from class: File().");
      }

      #endregion // OpenText

      #region OpenWrite

      [TestMethod]
      public void OpenWrite()
      {
         Console.WriteLine("FileInfo.OpenWrite()");
         Console.WriteLine("\nPlease see unit tests from class: File().");
      }

      #endregion // OpenWrite

      #region Refresh

      [TestMethod]
      public void Refresh()
      {
         Console.WriteLine("FileInfo.Refresh()");
         Console.WriteLine("\nPlease see unit tests from class: File().");
      }

      #endregion // Refresh

      #region Replace

      [TestMethod]
      public void Replace()
      {
         Console.WriteLine("FileInfo.Replace()");
         Console.WriteLine("\nPlease see unit tests from class: File().");
      }

      #endregion // Replace

      #region SetAccessControl

      [TestMethod]
      public void SetAccessControl()
      {
         Console.WriteLine("FileInfo.SetAccessControl()");
         Console.WriteLine("\nPlease see unit tests from class: File().");
      }

      #endregion // SetAccessControl

      #endregion // .NET

      #region AlphaFS

      #region Compress

      [TestMethod]
      public void AlphaFS_Compress()
      {
         Console.WriteLine("FileInfo.Compress()");
         Console.WriteLine("\nPlease see unit tests from class: File().");
      }

      #endregion // Compress

      #region Decompress

      [TestMethod]
      public void AlphaFS_Decompress()
      {
         Console.WriteLine("FileInfo.Decompress()");
         Console.WriteLine("\nPlease see unit tests from class: File().");
      }

      #endregion // Decompress]

      #region EnumerateStreams

      [TestMethod]
      public void AlphaFS_EnumerateStreams()
      {
         Console.WriteLine("FileInfo.EnumerateStreams()");
         Console.WriteLine("\nPlease see unit tests from class: File().");
      }

      #endregion // EnumerateStreams

      #endregion // AlphaFS
   }
}