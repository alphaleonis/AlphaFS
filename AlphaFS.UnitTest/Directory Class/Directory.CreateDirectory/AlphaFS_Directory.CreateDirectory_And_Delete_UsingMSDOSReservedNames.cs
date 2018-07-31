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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AlphaFS.UnitTest
{
   public partial class Directory_CreateDirectoryTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void AlphaFS_Directory_CreateDirectory_And_Delete_UsingMSDOSReservedNames_Local_Success()
      {
         using (var tempRoot = new TemporaryDirectory(false))
         {
            Console.WriteLine("Input Directory Path: [{0}]\n", tempRoot.Directory.FullName);


            var msDosReservedNames = new[]
            {
               "CON", "PRN", "AUX", "NUL",
               "COM1", "COM2", "COM3", "COM4", "COM5", "COM6", "COM7", "COM8", "COM9",
               "LPT1", "LPT2", "LPT3", "LPT4", "LPT5", "LPT6", "LPT7", "LPT8", "LPT9"
            };


            foreach (var msDosName in msDosReservedNames)
            {
               var folder = Alphaleonis.Win32.Filesystem.Directory.CreateDirectory(System.IO.Path.Combine(tempRoot.Directory.FullName, msDosName));

               Console.WriteLine("\tCreated Directory: [{0}]", folder.FullName);

               
               Assert.IsFalse(System.IO.Directory.Exists(folder.FullName));


               Assert.IsTrue(Alphaleonis.Win32.Filesystem.Directory.Exists(folder.FullName));


               // Create subfolder in the MSDOS reserved name folder.
               var subFolder = System.IO.Path.Combine(folder.FullName, tempRoot.RandomString);


               Alphaleonis.Win32.Filesystem.Directory.CreateDirectory(subFolder);

               Console.WriteLine("\tCreated Directory: [{0}]\n", subFolder);


               Assert.IsTrue(Alphaleonis.Win32.Filesystem.Directory.Exists(subFolder));


               Alphaleonis.Win32.Filesystem.Directory.Delete(folder.FullName, true);

               Assert.IsFalse(Alphaleonis.Win32.Filesystem.Directory.Exists(folder.FullName));
            }
         }

         Console.WriteLine();
      }
   }
}
