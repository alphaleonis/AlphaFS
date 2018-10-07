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
      public void Directory_CompareTimestamps_NonExistingDirectory_Local_Success()
      {
         Directory_CompareTimestamps_NonExistingDirectory();
      }


      private void Directory_CompareTimestamps_NonExistingDirectory()
      {
         using (var tempRoot = new TemporaryDirectory())
         {
            var folder = tempRoot.RandomDirectoryFullPath;

            Console.WriteLine("Input Directory Path: [{0}]", folder);


            var newDateTime = new DateTime(1601, 1, 1);
            var newDateTimeLocaltime = new DateTime(1601, 1, 1).ToLocalTime();

            UnitTestConstants.Dump(newDateTime);
            UnitTestConstants.Dump(newDateTimeLocaltime);
            

            Assert.AreEqual(newDateTime, System.IO.Directory.GetCreationTimeUtc(folder));
            Assert.AreEqual(newDateTime, Alphaleonis.Win32.Filesystem.Directory.GetCreationTimeUtc(folder));
            
            Assert.AreEqual(newDateTime, System.IO.Directory.GetLastAccessTimeUtc(folder));
            Assert.AreEqual(newDateTime, Alphaleonis.Win32.Filesystem.Directory.GetLastAccessTimeUtc(folder));
            
            Assert.AreEqual(newDateTime, System.IO.Directory.GetLastWriteTimeUtc(folder));
            Assert.AreEqual(newDateTime, Alphaleonis.Win32.Filesystem.Directory.GetLastWriteTimeUtc(folder));


            Assert.AreEqual(newDateTimeLocaltime, System.IO.Directory.GetCreationTime(folder));
            Assert.AreEqual(newDateTimeLocaltime, Alphaleonis.Win32.Filesystem.Directory.GetCreationTime(folder));

            Assert.AreEqual(newDateTimeLocaltime, System.IO.Directory.GetLastAccessTime(folder));
            Assert.AreEqual(newDateTimeLocaltime, Alphaleonis.Win32.Filesystem.Directory.GetLastAccessTime(folder));

            Assert.AreEqual(newDateTimeLocaltime, System.IO.Directory.GetLastWriteTime(folder));
            Assert.AreEqual(newDateTimeLocaltime, Alphaleonis.Win32.Filesystem.Directory.GetLastWriteTime(folder));
         }

         Console.WriteLine();
      }
   }
}
