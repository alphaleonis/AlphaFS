///*  Copyright (C) 2008-2018 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
// *  
// *  Permission is hereby granted, free of charge, to any person obtaining a copy 
// *  of this software and associated documentation files (the "Software"), to deal 
// *  in the Software without restriction, including without limitation the rights 
// *  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell 
// *  copies of the Software, and to permit persons to whom the Software is 
// *  furnished to do so, subject to the following conditions:
// *  
// *  The above copyright notice and this permission notice shall be included in 
// *  all copies or substantial portions of the Software.
// *  
// *  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
// *  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
// *  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
// *  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
// *  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
// *  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN 
// *  THE SOFTWARE. 
// */

//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System;
//using System.Reflection;

//namespace AlphaFS.UnitTest
//{
//   partial class FileTest
//   {
//      // Pattern: <class>_<function>_<scenario>_<expected result>


//      [TestMethod]
//      public void File_Open_OverlappedIO_LocalAndNetwork_Success()
//      {
//         // https://gist.github.com/mattpodwysocki/1862069
//         // https://gist.github.com/PaulStovell/1696375


//         File_Open_OverlappedIO(false);
//         File_Open_OverlappedIO(true);
//      }
      

//      private void File_Open_OverlappedIO(bool isNetwork)
//      {
//         UnitTestConstants.PrintUnitTestHeader(isNetwork);

//         var tempPath = UnitTestConstants.TempPath;
//         if (isNetwork)
//            tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);


//         using (var rootDir = new TemporaryDirectory(tempPath, MethodBase.GetCurrentMethod().Name))
//         {
//            var file = rootDir.RandomTxtFileFullPath;
//            Console.WriteLine("\nInput File Path: [{0}]", file);


//            long fileLength;
//            var ten = UnitTestConstants.TenNumbers.Length;

//            using (var fs = Alphaleonis.Win32.Filesystem.File.Open(file, System.IO.FileMode.Create,
//               System.IO.FileAccess.ReadWrite, System.IO.FileShare.ReadWrite, 4096,
//               Alphaleonis.Win32.Filesystem.ExtendedFileAttributes.Overlapped,
//               Alphaleonis.Win32.Filesystem.PathFormat.FullPath))
//            {
//               // According to NotePad++, creates a file type: "ANSI", which is reported as: "Unicode (UTF-8)".
//               fs.Write(UnitTestConstants.StringToByteArray(UnitTestConstants.TenNumbers), 0, ten);

//               fileLength = fs.Length;
//            }

//            Assert.IsTrue(System.IO.File.Exists(file), "The file does not exists, but is expected to.");
//            Assert.IsTrue(fileLength == ten, "The file is: {0} bytes, but is expected to be: {1} bytes.", fileLength, ten);
//         }

//         Console.WriteLine();
//      }
//   }
//}
