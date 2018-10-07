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
using Microsoft.Win32;

namespace AlphaFS.UnitTest
{
   public partial class PathTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void AlphaFS_Path_GetShort83PathAndGetLongFrom83ShortPath_FromFile_LocalAndNetwork_Success()
      {
         var isDisabled = (int) Registry.GetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem", "NtfsDisable8dot3NameCreation", 0) > 0;
         if (isDisabled)
            UnitTestAssert.Inconclusive("NtfsDisable8dot3NameCreation is disabled in registry.");
         
         AlphaFS_Path_GetShort83PathAndGetLongFrom83ShortPath_FromFile(false);
         AlphaFS_Path_GetShort83PathAndGetLongFrom83ShortPath_FromFile(true);
      }


      private void AlphaFS_Path_GetShort83PathAndGetLongFrom83ShortPath_FromFile(bool isNetwork)
      {
         using (var tempRoot = new TemporaryDirectory(isNetwork))
         {
            var folder = System.IO.Path.Combine(tempRoot.Directory.FullName, "My Long Data File Or Directory");
            
            string short83Path;

            using (System.IO.File.Create(folder))
               short83Path = Alphaleonis.Win32.Filesystem.Path.GetShort83Path(folder);


            Console.WriteLine("Short 8.3 File Path: [{0}]", short83Path);
            
            Assert.IsTrue(!short83Path.Equals(folder));

            Assert.IsTrue(short83Path.EndsWith(@"~1"));



            var longFrom83Path = Alphaleonis.Win32.Filesystem.Path.GetLongFrom83ShortPath(short83Path);

            Console.WriteLine("Long path from 8.3 path: [{0}]", longFrom83Path);

            Assert.IsTrue(longFrom83Path.Equals(folder));

            Assert.IsFalse(longFrom83Path.EndsWith(@"~1"));
         }

         Console.WriteLine();
      }
   }
}
