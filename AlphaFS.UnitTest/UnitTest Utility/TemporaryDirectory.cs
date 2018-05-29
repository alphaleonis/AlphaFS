/*  Copyright (C) 2008-2018 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
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

namespace AlphaFS.UnitTest
{
   /// <summary>Used to create a temporary directory that will be deleted once this instance is disposed.</summary>
   internal sealed class TemporaryDirectory : IDisposable
   {
      public TemporaryDirectory() : this(false, null, null) { }


      //public TemporaryDirectory(string folderPrefix) : this(false, folderPrefix, null) { }


      public TemporaryDirectory(bool isNetwork) : this(isNetwork, null, null) { }

      
      //public TemporaryDirectory(bool isNetwork, string folderPrefix) : this(isNetwork, folderPrefix, null) { }


      public TemporaryDirectory(bool isNetwork, string folderPrefix, string root)
      {
         if (Utils.IsNullOrWhiteSpace(folderPrefix))
            folderPrefix = "AlphaFS.TempRoot";

         if (Utils.IsNullOrWhiteSpace(root))
            root = UnitTestConstants.TempPath;

         if (isNetwork)
            root = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(root);

         UnitTestConstants.PrintUnitTestHeader(isNetwork);


         do
         {
            Directory = new System.IO.DirectoryInfo(System.IO.Path.Combine(root, folderPrefix + "." + UnitTestConstants.GetRandomFileName().Substring(0, 6)));

         } while (Directory.Exists);

         Directory.Create();
      }


      public System.IO.DirectoryInfo Directory { get; private set; }


      /// <summary>Returns the full path to a non-existing directory with a random name, such as: "C:\Users\UserName\AppData\Local\Temp\AlphaFS.TempRoot.lpqdzf\Directory_wqáánmvh.z03".</summary>
      public string RandomDirectoryFullPath
      {
         get { return System.IO.Path.Combine(Directory.FullName, "Directory_" + RandomFileName); }
      }


      /// <summary>Returns the full path to a non-existing file with a random name, such as: "C:\Users\UserName\AppData\Local\Temp\AlphaFS.TempRoot.lpqdzf\File_wqáánmvh.txt".</summary>
      public string RandomTxtFileFullPath
      {
         get { return System.IO.Path.Combine(Directory.FullName, "File_" + System.IO.Path.GetFileNameWithoutExtension(RandomFileName)) + ".txt"; }
      }


      /// <summary>Returns the full path to a non-existing file with a random name and without an extension, such as: "C:\Users\UserName\AppData\Local\Temp\AlphaFS.TempRoot.lpqdzf\File_wqáánmvh".</summary>
      public string RandomFileNoExtensionFullPath
      {
         get { return System.IO.Path.GetFileNameWithoutExtension(System.IO.Path.Combine(Directory.FullName, "File_" + RandomFileName)); }
      }


      /// <summary>Returns a random folder or file name, such as: "wqáánmvh".</summary>
      public string RandomFileName
      {
         get { return UnitTestConstants.GetRandomFileName(); }
      }


      public override string ToString()
      {
         return Directory.FullName;
      }




      /// <summary>Returns a <see cref="System.IO.DirectoryInfo"/> instance to an existing directory.</summary>
      public System.IO.DirectoryInfo CreateRandomDirectory()
      {
         return System.IO.Directory.CreateDirectory(RandomDirectoryFullPath);
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
            Console.WriteLine("\n\nDelete TemporaryDirectory: [{0}]. Error: [{1}]", Directory.FullName, ex.Message.Replace(Environment.NewLine, string.Empty));
            Console.Write("Retry using AlphaFS... ");

            try
            {
               var dirInfo = new Alphaleonis.Win32.Filesystem.DirectoryInfo(Directory.FullName, Alphaleonis.Win32.Filesystem.PathFormat.FullPath);
               if (dirInfo.Exists)
               {
                  dirInfo.Delete(true, true);
                  Console.WriteLine("Success.");
               }

               else
                  Console.WriteLine("TemporaryDirectory was already removed.");
            }
            catch (Exception ex2)
            {
               Console.WriteLine("Delete failure TemporaryDirectory. Error: {0}", ex2.Message.Replace(Environment.NewLine, string.Empty));
            }
         }
      }

      #endregion // Disposable Members
   }
}
