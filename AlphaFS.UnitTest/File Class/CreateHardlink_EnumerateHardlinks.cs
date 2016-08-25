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

using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AlphaFS.UnitTest
{
   partial class FileTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>

      [TestMethod]
      public void AlphaFS_File_CreateHardlink_And_EnumerateHardlinks_Local_Success()
      {
         File_CreateEnumerateHardlinks(false);
         File_CreateEnumerateHardlinks(true);
      }


      private void File_CreateEnumerateHardlinks(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         if (isNetwork)
         {
            Console.WriteLine("\n\tEnumerating Hardlinks does not work with UNC paths.");
            return;
         }


         using (var rootDir = new TemporaryDirectory(System.IO.Path.GetTempPath(), "File.EnumerateHardlinks"))
         {
            var hardlinkFolder = System.IO.Path.Combine(rootDir.Directory.FullName, "Hardlinks");
            System.IO.Directory.CreateDirectory(hardlinkFolder);


            var file = System.IO.Path.Combine(rootDir.Directory.FullName, "File.EnumerateHardlinks.txt");
            Console.WriteLine("\n\tInput File Path: [{0}]\n", file);

            // Create original file with text content.
            System.IO.File.WriteAllText(file, UnitTestConstants.TextHelloWorld);


            // Create a random number of hardlinks to the original file.
            int numCreate = new Random().Next(1, 20);
            List<string> hardlinks = new List<string>();

            Console.WriteLine("\tCreated {0} hardlinks:", numCreate);

            for (int i = 0; i < numCreate; i++)
            {
               string newfile = System.IO.Path.Combine(hardlinkFolder, i + "-Hardlink-" + System.IO.Path.GetRandomFileName() + ".txt");

               Alphaleonis.Win32.Filesystem.File.CreateHardlink(newfile, file);

               hardlinks.Add(newfile);
            }


            int cnt = 0;
            foreach (string hardLink in Alphaleonis.Win32.Filesystem.File.EnumerateHardlinks(file))
            {
               if (!file.EndsWith(hardLink, StringComparison.OrdinalIgnoreCase))
                  Console.WriteLine("\t\t#{0:000}\tHardlink: [{1}]", ++cnt, hardLink);
            }
            
            Assert.AreEqual(numCreate, cnt);
         }

         Console.WriteLine();
      }
   }
}
