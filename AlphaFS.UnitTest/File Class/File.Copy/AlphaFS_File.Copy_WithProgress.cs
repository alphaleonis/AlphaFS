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
using System.Globalization;

namespace AlphaFS.UnitTest
{
   public partial class CopyTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void AlphaFS_File_Copy_WithProgress_LocalAndNetwork_Success()
      {
         AlphaFS_File_Copy_WithProgress(false);
         AlphaFS_File_Copy_WithProgress(true);
      }
      

      private void AlphaFS_File_Copy_WithProgress(bool isNetwork)
      {
         using (var tempRoot = new TemporaryDirectory(isNetwork))
         {
            var fileSource = tempRoot.CreateFile();
            var fileCopy = tempRoot.CreateFile();

            Console.WriteLine("Src File Path: [{0}] [{1}]", Alphaleonis.Utils.UnitSizeToText(fileSource.Length), fileSource.FullName);
            Console.WriteLine("Dst File Path: [{0}]", fileCopy.FullName);

            
            // Allow copy to overwrite an existing file.
            const Alphaleonis.Win32.Filesystem.CopyOptions copyOptions = Alphaleonis.Win32.Filesystem.CopyOptions.None;


            // The copy progress handler.
            var callback = new Alphaleonis.Win32.Filesystem.CopyMoveProgressRoutine(FileCopyProgressHandler);


            Console.WriteLine();


            // You can pass any piece of data to userProgressData. This data can be accessed from the callback method.
            // Specify file length for assertion.
            var userProgressData = fileSource.Length;


            var copyResult = Alphaleonis.Win32.Filesystem.File.Copy(fileSource.FullName, fileCopy.FullName, copyOptions, callback, userProgressData);

            UnitTestConstants.Dump(copyResult);


            Assert.IsTrue(System.IO.File.Exists(fileCopy.FullName), "The file does not exists, but is expected to.");


            Assert.AreEqual(fileSource.Length, fileCopy.Length, "The file sizes do no match, but are expected to.");

            Assert.IsTrue(System.IO.File.Exists(fileSource.FullName), "The original file does not exist, but is expected to.");
         }
         
         Console.WriteLine();
      }


      private static Alphaleonis.Win32.Filesystem.CopyMoveProgressResult FileCopyProgressHandler(long totalFileSize, long totalBytesTransferred, long streamSize,
         long streamBytesTransferred, int streamNumber, Alphaleonis.Win32.Filesystem.CopyMoveProgressCallbackReason callbackReason, object userData)
      {
         if (callbackReason == Alphaleonis.Win32.Filesystem.CopyMoveProgressCallbackReason.StreamSwitch)

            Assert.AreEqual(0, totalBytesTransferred);


         else
         {
            var pct = Convert.ToDouble(totalBytesTransferred, CultureInfo.InvariantCulture) / totalFileSize * 100;

            Console.WriteLine("\tCallback: Copied: [{0}%] --> [{1:N0}] bytes.", pct.ToString("N2", CultureInfo.CurrentCulture), totalBytesTransferred);

            Assert.IsTrue(totalBytesTransferred > 0);


            var bytes = Convert.ToInt64(userData);

            if (pct.Equals(100))
            {
               Assert.AreEqual(totalBytesTransferred, bytes);
               Assert.AreEqual(totalFileSize, bytes);
            }

            else
               Assert.IsTrue(totalBytesTransferred < bytes);
         }


         return Alphaleonis.Win32.Filesystem.CopyMoveProgressResult.Continue;
      }
   }
}
