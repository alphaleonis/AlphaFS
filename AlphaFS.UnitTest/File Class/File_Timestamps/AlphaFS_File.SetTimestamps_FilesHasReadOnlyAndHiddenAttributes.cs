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
   public partial class TimestampsTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void AlphaFS_File_SetTimestamps_LocalAndNetwork_Success()
      {
         AlphaFS_File_SetTimestamps_FilesHasReadOnlyAndHiddenAttributes(false);
         AlphaFS_File_SetTimestamps_FilesHasReadOnlyAndHiddenAttributes(true);
      }


      private void AlphaFS_File_SetTimestamps_FilesHasReadOnlyAndHiddenAttributes(bool isNetwork)
      {
         using (var tempRoot = new TemporaryDirectory(isNetwork))
         {
            var file = tempRoot.CreateFile();

            Console.WriteLine("Input File Path   : [{0}]", file.FullName);


            file.Attributes |= System.IO.FileAttributes.ReadOnly | System.IO.FileAttributes.Hidden;

            var creationTime = tempRoot.GetRandomFileDate();
            var lastAccessTime = tempRoot.GetRandomFileDate();
            var lastWriteTime = tempRoot.GetRandomFileDate();


            Alphaleonis.Win32.Filesystem.File.SetTimestamps(file.FullName, creationTime, lastAccessTime, lastWriteTime);


            Assert.AreEqual(System.IO.File.GetCreationTime(file.FullName), creationTime);
            Assert.AreEqual(System.IO.File.GetLastAccessTime(file.FullName), lastAccessTime);
            Assert.AreEqual(System.IO.File.GetLastWriteTime(file.FullName), lastWriteTime);
         }

         Console.WriteLine();
      }
   }
}
