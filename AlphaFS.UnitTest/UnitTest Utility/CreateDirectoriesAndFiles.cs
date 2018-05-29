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

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AlphaFS.UnitTest
{
   public static partial class UnitTestConstants
   {
      // A high "max" increases the change of path too long.
      public static void CreateDirectoriesAndFiles(string rootPath, int max, bool readOnly, bool hidden, bool recurse)
      {
         var folderCount = 0;

         for (var i = 0; i < max; i++)
         {
            var fsoName = GetRandomFileName();
            var file = System.IO.Path.Combine(rootPath, fsoName);
            var dir = file + "-" + i + "-dir";
            file = file + "-" + i + "-file.txt";

            folderCount++;


            System.IO.Directory.CreateDirectory(dir);

            CreateFile(rootPath, fsoName, i, readOnly, hidden);


            if (readOnly && new Random(DateTime.UtcNow.Millisecond).Next(0, 1000) % 2 == 0)
               System.IO.File.SetAttributes(dir, System.IO.FileAttributes.ReadOnly);

            if (hidden && new Random(DateTime.UtcNow.Millisecond).Next(0, 1000) % 2 == 0)
               System.IO.File.SetAttributes(dir, System.IO.FileAttributes.Hidden);


            var filePath = System.IO.Path.Combine(dir, System.IO.Path.GetFileName(file));


            // Every other folder.
            if (i % 2 == 0)
            {
               CreateFile(dir, null, i, readOnly, hidden);


               System.IO.File.WriteAllText(filePath, DateTime.Now.ToLongDateString());

               if (readOnly && new Random(DateTime.UtcNow.Millisecond).Next(0, 1000) % 2 == 0)
                  System.IO.File.SetAttributes(filePath, System.IO.FileAttributes.ReadOnly);

               if (hidden && new Random(DateTime.UtcNow.Millisecond).Next(0, 1000) % 2 == 0)
                  System.IO.File.SetAttributes(filePath, System.IO.FileAttributes.Hidden);
            }
         }


         if (recurse)
         {
            foreach (var dir in System.IO.Directory.EnumerateDirectories(rootPath))
               CreateDirectoriesAndFiles(dir, max, readOnly, hidden, false);
         }


         Assert.AreEqual(max, folderCount, "The number of folders does not equal the max folder-level, but is expected to.");
      }


      public static System.IO.FileInfo CreateFile(string rootFolder, int fileLength = 0)
      {
         return CreateFile(rootFolder, null, 0, false, false, fileLength);
      }


      public static System.IO.FileInfo CreateFile(string rootFolder, string fileName, int count, bool readOnly, bool hidden, int fileLength = 0)
      {
         var file = System.IO.Path.Combine(rootFolder, (!Alphaleonis.Utils.IsNullOrWhiteSpace(fileName) ? fileName : GetRandomFileName()) + "-" + count + "-file");

         using (var fs = System.IO.File.Create(file))
         {
            if (fileLength <= 0)
               fileLength = new Random().Next(0, 10485760);

            fs.SetLength(fileLength);
         }


         if (readOnly && new Random(DateTime.UtcNow.Millisecond).Next(0, 1000) % 2 == 0)
            System.IO.File.SetAttributes(file, System.IO.FileAttributes.ReadOnly);

         if (hidden && new Random(DateTime.UtcNow.Millisecond).Next(0, 1000) % 2 == 0)
            System.IO.File.SetAttributes(file, System.IO.FileAttributes.Hidden);


         return new System.IO.FileInfo(file);
      }
   }
}
