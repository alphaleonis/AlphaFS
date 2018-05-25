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
   public partial class File_TimestampsTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void CompareTimestamps_NonExistingFile_Local_Success()
      {
         CompareTimestamps_NonExistingFile();
      }


      private void CompareTimestamps_NonExistingFile()
      {
         UnitTestConstants.PrintUnitTestHeader(false);

         var tempPath = UnitTestConstants.SysDrive + @"\nonExistingFile-" + UnitTestConstants.GetRandomFileNameWithDiacriticCharacters();

         Console.WriteLine("\nInput File Path: [{0}]", tempPath);


         Assert.IsFalse(System.IO.File.Exists(tempPath));


         var newDateTime = new DateTime(1601, 1, 1);
         var newDateTimeLocaltime = new DateTime(1601, 1, 1).ToLocalTime();


         Assert.AreEqual(newDateTimeLocaltime, System.IO.File.GetCreationTime(tempPath));
         Assert.AreEqual(newDateTimeLocaltime, Alphaleonis.Win32.Filesystem.File.GetCreationTime(tempPath));
         
         Assert.AreEqual(newDateTime, System.IO.File.GetCreationTimeUtc(tempPath));
         Assert.AreEqual(newDateTime, Alphaleonis.Win32.Filesystem.File.GetCreationTimeUtc(tempPath));


         Assert.AreEqual(newDateTimeLocaltime, System.IO.File.GetLastAccessTime(tempPath));
         Assert.AreEqual(newDateTimeLocaltime, Alphaleonis.Win32.Filesystem.File.GetLastAccessTime(tempPath));

         Assert.AreEqual(newDateTime, System.IO.File.GetLastAccessTimeUtc(tempPath));
         Assert.AreEqual(newDateTime, Alphaleonis.Win32.Filesystem.File.GetLastAccessTimeUtc(tempPath));


         Assert.AreEqual(newDateTimeLocaltime, System.IO.File.GetLastWriteTime(tempPath));
         Assert.AreEqual(newDateTimeLocaltime, Alphaleonis.Win32.Filesystem.File.GetLastWriteTime(tempPath));

         Assert.AreEqual(newDateTime, System.IO.File.GetLastWriteTimeUtc(tempPath));
         Assert.AreEqual(newDateTime, Alphaleonis.Win32.Filesystem.File.GetLastWriteTimeUtc(tempPath));


         Console.WriteLine();
      }
   }
}
