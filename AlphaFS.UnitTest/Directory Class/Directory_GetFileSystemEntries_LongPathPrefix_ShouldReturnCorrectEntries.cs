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
using System.Linq;

namespace AlphaFS.UnitTest
{
   public partial class DirectoryTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void Directory_GetFileSystemEntries_LongPathWithPrefix_ShouldReturnCorrectEntries_LocalAndNetwork_Success()
      {
         Directory_GetFileSystemEntries_LongPath_ShouldReturnCorrectEntries(false, true);
         Directory_GetFileSystemEntries_LongPath_ShouldReturnCorrectEntries(true, true);
      }


      [TestMethod]
      public void Directory_GetFileSystemEntries_LongPathWithoutPrefix_ShouldReturnCorrectEntries_Network_Success()
      {
         Directory_GetFileSystemEntries_LongPath_ShouldReturnCorrectEntries(false, false);
         Directory_GetFileSystemEntries_LongPath_ShouldReturnCorrectEntries(true, false);
      }


      private void Directory_GetFileSystemEntries_LongPath_ShouldReturnCorrectEntries(bool isNetwork, bool withPrefix)
      {
         using (var tempRoot = new TemporaryDirectory(isNetwork))
         {
            var folder = tempRoot.RandomDirectoryFullPath;

            Console.WriteLine("Input Directory Path: [{0}]", folder);


            var longDir = System.IO.Path.Combine(folder, new string('x', 128), new string('x', 128), new string('x', 128), new string('x', 128));

            Alphaleonis.Win32.Filesystem.Directory.CreateDirectory(longDir);

            Alphaleonis.Win32.Filesystem.Directory.CreateDirectory(System.IO.Path.Combine(longDir, "A"));

            Alphaleonis.Win32.Filesystem.Directory.CreateDirectory(System.IO.Path.Combine(longDir, "B"));

            Alphaleonis.Win32.Filesystem.File.WriteAllText(System.IO.Path.Combine(longDir, "C"), "C");


            var prefix = withPrefix ? isNetwork ? Alphaleonis.Win32.Filesystem.Path.LongPathUncPrefix : Alphaleonis.Win32.Filesystem.Path.LongPathPrefix : string.Empty;

            var entries = Alphaleonis.Win32.Filesystem.Directory.GetFileSystemEntries(prefix + longDir).ToArray();


            Assert.AreEqual(3, entries.Length);
         }

         Console.WriteLine();
      }
   }
}
