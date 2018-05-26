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
      public void CompareTimestamps_NonExistingDirectory_Local_Success()
      {
         CompareTimestamps_NonExistingDirectory();
      }


      private void CompareTimestamps_NonExistingDirectory()
      {
         UnitTestConstants.PrintUnitTestHeader(false);

         var tempPath = UnitTestConstants.SysDrive + @"\nonExistingFolder-" + UnitTestConstants.GetRandomFileNameWithDiacriticCharacters();

         Console.WriteLine("\nInput Directory Path: [{0}]", tempPath);


         Assert.IsFalse(System.IO.Directory.Exists(tempPath));


         var newDateTime = new DateTime(1601, 1, 1);
         var newDateTimeLocaltime = new DateTime(1601, 1, 1).ToLocalTime();


         Assert.AreEqual(newDateTimeLocaltime, System.IO.Directory.GetCreationTime(tempPath));
         Assert.AreEqual(newDateTimeLocaltime, Alphaleonis.Win32.Filesystem.Directory.GetCreationTime(tempPath));

         Assert.AreEqual(newDateTime, System.IO.Directory.GetCreationTimeUtc(tempPath));
         Assert.AreEqual(newDateTime, Alphaleonis.Win32.Filesystem.Directory.GetCreationTimeUtc(tempPath));


         Assert.AreEqual(newDateTimeLocaltime, System.IO.Directory.GetLastAccessTime(tempPath));
         Assert.AreEqual(newDateTimeLocaltime, Alphaleonis.Win32.Filesystem.Directory.GetLastAccessTime(tempPath));

         Assert.AreEqual(newDateTime, System.IO.Directory.GetLastAccessTimeUtc(tempPath));
         Assert.AreEqual(newDateTime, Alphaleonis.Win32.Filesystem.Directory.GetLastAccessTimeUtc(tempPath));


         Assert.AreEqual(newDateTimeLocaltime, System.IO.Directory.GetLastWriteTime(tempPath));
         Assert.AreEqual(newDateTimeLocaltime, Alphaleonis.Win32.Filesystem.Directory.GetLastWriteTime(tempPath));

         Assert.AreEqual(newDateTime, System.IO.Directory.GetLastWriteTimeUtc(tempPath));
         Assert.AreEqual(newDateTime, Alphaleonis.Win32.Filesystem.Directory.GetLastWriteTimeUtc(tempPath));


         Console.WriteLine();
      }
   }
}
