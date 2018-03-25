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
            var file = System.IO.Path.Combine(rootPath, GetRandomFileNameWithDiacriticCharacters());
            var dir = file + "-" + i + "-dir";
            file = file + "-" + i + "-file";

            folderCount++;
            System.IO.Directory.CreateDirectory(dir);

            var filePath = System.IO.Path.Combine(dir, System.IO.Path.GetFileName(file));


            // Every other folder is empty.
            if (i % 2 == 0)
            {
               CreateFile(dir);

               System.IO.File.WriteAllText(filePath, TextGoodbyeWorld);

               switch (new Random(max).Next(0, 2))
               {
                  case 1:
                     if (readOnly)
                        System.IO.File.SetAttributes(filePath, System.IO.FileAttributes.ReadOnly);
                     break;

                  case 2:
                     if (hidden)
                        System.IO.File.SetAttributes(filePath, System.IO.FileAttributes.Hidden);
                     break;
               }
            }
         }


         if (recurse)
         {
            foreach (var dir in System.IO.Directory.EnumerateDirectories(rootPath))
               CreateDirectoriesAndFiles(dir, max, readOnly, hidden, false);
         }


         Assert.AreEqual(max, folderCount, "The number of folders does not equal the max folder-level, but is expected to.");
      }
   }
}
