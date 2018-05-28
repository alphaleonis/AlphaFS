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
using System.Collections.Generic;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AlphaFS.UnitTest
{
   public partial class AlphaFS_JunctionsLinksTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>

      [TestMethod]
      public void AlphaFS_File_CreateHardlink_And_EnumerateHardlinks_Local_Success()
      {
         File_CreateHardlink_And_EnumerateHardlinks(false);
      }


      private void File_CreateHardlink_And_EnumerateHardlinks(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);
         
         using (var tempPath = new TemporaryDirectory(MethodBase.GetCurrentMethod().Name))
         {
            var hardlinkFolder = System.IO.Path.Combine(tempPath.Directory.FullName, "Hardlinks");
            System.IO.Directory.CreateDirectory(hardlinkFolder);


            var file = System.IO.Path.Combine(tempPath.Directory.FullName, "OriginalFile.txt");
            Console.WriteLine("\nInput File Path: [{0}]\n", file);

            // Create original file with text content.
            System.IO.File.WriteAllText(file, UnitTestConstants.TextHelloWorld);


            // Create a random number of hardlinks to the original file.
            var numCreate = new Random().Next(1, 20);
            var hardlinks = new List<string>();

            Console.WriteLine("Created {0} hardlinks:", numCreate + 1);

            for (var i = 0; i < numCreate; i++)
            {
               var newfile = System.IO.Path.Combine(hardlinkFolder, i + "-Hardlink-" + UnitTestConstants.GetRandomFileNameWithDiacriticCharacters() + ".txt");

               Alphaleonis.Win32.Filesystem.File.CreateHardlink(newfile, file);

               hardlinks.Add(newfile);
            }


            var cnt = 0;
            foreach (var hardLink in Alphaleonis.Win32.Filesystem.File.EnumerateHardlinks(file))
               Console.WriteLine("\t\t#{0:000}\tHardlink: [{1}]", ++cnt, hardLink);
            
            Assert.AreEqual(numCreate + 1, cnt);


            using (var stream = System.IO.File.OpenRead(file))
            {
               var bhfi = Alphaleonis.Win32.Filesystem.File.GetFileInfoByHandle(stream.SafeFileHandle);

               Assert.AreEqual(numCreate + 1, bhfi.NumberOfLinks);

               Console.WriteLine("\n\n\tByHandleFileInfo for Input Path, see property: NumberOfLinks");
               UnitTestConstants.Dump(bhfi, -18);
            }
         }

         Console.WriteLine();
      }
   }
}
