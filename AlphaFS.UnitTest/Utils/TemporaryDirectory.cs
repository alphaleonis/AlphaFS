/*  Copyright (C) 2008-2016 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
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

using Alphaleonis;
using System;
using System.Globalization;

namespace AlphaFS.UnitTest
{
   /// <summary>Used to create a temporary directory that will be deleted once this instance is disposed.</summary>
   internal sealed class TemporaryDirectory : IDisposable
   {
      public TemporaryDirectory(string prefix = null) : this(System.IO.Path.GetTempPath(), prefix) { }
      
      public TemporaryDirectory(string root, string prefix)
      {
         if (Utils.IsNullOrWhiteSpace(prefix))
            prefix = "AlphaFS";

         do
         {
            Directory = new System.IO.DirectoryInfo(System.IO.Path.Combine(root, prefix + "-" + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture).Substring(0, 6)));

         } while (Directory.Exists);

         Directory.Create();
      }

      public System.IO.DirectoryInfo Directory { get; private set; }

      public string RandomFileFullPath
      {
         get { return System.IO.Path.Combine(Directory.FullName, System.IO.Path.GetRandomFileName()); }
      }

      #region Disposable Members

      ~TemporaryDirectory()
      {
         Dispose(false);
      }

      public void Dispose()
      {
         Dispose(true);
         GC.SuppressFinalize(this);
      }

      private void Dispose(bool isDisposing)
      {
         try
         {
            if (isDisposing)
               System.IO.Directory.Delete(Directory.FullName, true);
         }
         catch (Exception ex)
         {
            Console.WriteLine("\n\nSystem.IO failed to delete TemporaryDirectory: [{0}]\nError: [{1}]", Directory.FullName, ex.Message);
            Console.Write("Retry using AlphaFS... ");

            try
            {
               Alphaleonis.Win32.Filesystem.Directory.Delete(Directory.FullName, true, Alphaleonis.Win32.Filesystem.PathFormat.FullPath);
               Console.WriteLine("Success.");
            }
            catch (Exception ex2)
            {
               Console.WriteLine("Failed to delete TemporaryDirectory.\nError: [{0}]", ex2.Message);
            }
         }
      }

      #endregion // Disposable Members
   }
}
