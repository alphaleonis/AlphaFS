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
   public partial class CopyTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void AlphaFS_Directory_Copy_CopyTimestamps_LocalAndNetwork_Success()
      {
         UnitTestAssert.IsElevatedProcess();

         AlphaFS_Directory_Copy_CopyTimestamps(false);
         AlphaFS_Directory_Copy_CopyTimestamps(true);
      }


      private void AlphaFS_Directory_Copy_CopyTimestamps(bool isNetwork)
      {
         using (var tempRoot = new TemporaryDirectory(isNetwork))
         {
            var folderSrc = tempRoot.CreateRecursiveRandomizedAttributesTree(5);
            var folderDst = tempRoot.RandomDirectoryFullPath;

            Console.WriteLine("Src Directory Path: [{0}]", folderSrc.FullName);
            Console.WriteLine("Dst Directory Path: [{0}]", folderDst);


            var rnd = new Random();

            var year = DateTime.Now.Year;
            var creationTime = new DateTime(rnd.Next(1971, year), rnd.Next(1, 12), rnd.Next(1, 28), rnd.Next(0, 23), rnd.Next(0, 59), rnd.Next(0, 59));
            var lastAccessTime = new DateTime(rnd.Next(1971, year), rnd.Next(1, 12), rnd.Next(1, 28), rnd.Next(0, 23), rnd.Next(0, 59), rnd.Next(0, 59));
            var lastWriteTime = new DateTime(rnd.Next(1971, year), rnd.Next(1, 12), rnd.Next(1, 28), rnd.Next(0, 23), rnd.Next(0, 59), rnd.Next(0, 59));


            Alphaleonis.Win32.Filesystem.Directory.SetTimestamps(folderSrc.FullName, creationTime, lastAccessTime, lastWriteTime);

            Assert.AreEqual(System.IO.Directory.GetCreationTimeUtc(folderSrc.FullName), folderSrc.CreationTimeUtc);
            Assert.AreEqual(System.IO.Directory.GetLastAccessTimeUtc(folderSrc.FullName), folderSrc.LastAccessTimeUtc);
            Assert.AreEqual(System.IO.Directory.GetLastWriteTimeUtc(folderSrc.FullName), folderSrc.LastWriteTimeUtc);


            Alphaleonis.Win32.Filesystem.Directory.Copy(folderSrc.FullName, folderDst, Alphaleonis.Win32.Filesystem.CopyOptions.CopyTimestamps);


            var dirInfo = new System.IO.DirectoryInfo(folderDst);

            UnitTestConstants.Dump(dirInfo);


            Assert.AreEqual(System.IO.Directory.GetCreationTimeUtc(folderSrc.FullName), dirInfo.CreationTimeUtc);
            Assert.AreEqual(System.IO.Directory.GetLastAccessTimeUtc(folderSrc.FullName), dirInfo.LastAccessTimeUtc);
            Assert.AreEqual(System.IO.Directory.GetLastWriteTimeUtc(folderSrc.FullName), dirInfo.LastWriteTimeUtc);
         }

         Console.WriteLine();
      }
   }
}
