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
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AlphaFS.UnitTest
{
   public partial class TimestampsTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void AlphaFS_Directory_CopyTimestamps_LocalAndNetwork_Success()
      {
         AlphaFS_Directory_CopyTimestamps(false);
         AlphaFS_Directory_CopyTimestamps(true);
      }

      
      private void AlphaFS_Directory_CopyTimestamps(bool isNetwork)
      {
         using (var tempRoot = new TemporaryDirectory(isNetwork))
         {
            var folderSrc = tempRoot.CreateDirectory();
            var folderDst = tempRoot.CreateDirectory();

            Console.WriteLine("Src Directory Path: [{0}]", folderSrc.FullName);
            Console.WriteLine("Src Directory Path: [{0}]", folderDst.FullName);


            var rnd = new Random();

            var year = DateTime.Now.Year;
            var creationTime = new DateTime(rnd.Next(1971, year), rnd.Next(1, 12), rnd.Next(1, 28), rnd.Next(0, 23), rnd.Next(0, 59), rnd.Next(0, 59));
            var lastAccessTime = new DateTime(rnd.Next(1971, year), rnd.Next(1, 12), rnd.Next(1, 28), rnd.Next(0, 23), rnd.Next(0, 59), rnd.Next(0, 59));
            var lastWriteTime = new DateTime(rnd.Next(1971, year), rnd.Next(1, 12), rnd.Next(1, 28), rnd.Next(0, 23), rnd.Next(0, 59), rnd.Next(0, 59));


            Alphaleonis.Win32.Filesystem.Directory.SetTimestamps(folderSrc.FullName, creationTime, lastAccessTime, lastWriteTime);

            Assert.AreEqual(System.IO.Directory.GetCreationTimeUtc(folderSrc.FullName), folderSrc.CreationTimeUtc);
            Assert.AreEqual(System.IO.Directory.GetLastAccessTimeUtc(folderSrc.FullName), folderSrc.LastAccessTimeUtc);
            Assert.AreEqual(System.IO.Directory.GetLastWriteTimeUtc(folderSrc.FullName), folderSrc.LastWriteTimeUtc);


            Alphaleonis.Win32.Filesystem.Directory.CopyTimestamps(folderSrc.FullName, folderDst.FullName);


            UnitTestConstants.Dump(folderDst);


            Assert.AreEqual(System.IO.Directory.GetCreationTimeUtc(folderSrc.FullName), folderDst.CreationTimeUtc);
            Assert.AreEqual(System.IO.Directory.GetLastAccessTimeUtc(folderSrc.FullName), folderDst.LastAccessTimeUtc);
            Assert.AreEqual(System.IO.Directory.GetLastWriteTimeUtc(folderSrc.FullName), folderDst.LastWriteTimeUtc);
         }

         Console.WriteLine();
      }
   }
}
