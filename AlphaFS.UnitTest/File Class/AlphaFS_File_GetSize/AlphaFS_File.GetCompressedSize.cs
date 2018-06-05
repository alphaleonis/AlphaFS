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

namespace AlphaFS.UnitTest
{
   partial class SizeTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void AlphaFS_File_GetCompressedSize_LocalAndNetwork_Success()
      {
         AlphaFS_File_GetCompressedSize(false);
         AlphaFS_File_GetCompressedSize(true);
      }


      private void AlphaFS_File_GetCompressedSize(bool isNetwork)
      {
         using (var tempRoot = new TemporaryDirectory(isNetwork))
         {
            var file = tempRoot.RandomTxtFileFullPath;

            Console.WriteLine("Input File Path: [{0}]\n", file);


            long streamLength;
            var tenNumbers = "0123456789";
            var thousand = 100 * tenNumbers.Length;
            var compressedSize = 4096;


            // Size: 9,76 KB(10.000 bytes)
            // Size on disk: 12,0 KB(12.288 bytes)
            using (var fs = System.IO.File.Create(file))
            {
               // According to NotePad++, creates a file type: "ANSI", which is reported as: "Unicode (UTF-8)".

               for (var count = 0; count < thousand; count ++)
                  fs.Write(UnitTestConstants.StringToByteArray(tenNumbers), 0, tenNumbers.Length);

               streamLength = fs.Length;
            }

            Console.WriteLine("\tFile size (uncompressed): {0:N0} bytes ({1})", streamLength, Alphaleonis.Utils.UnitSizeToText(streamLength));


            // Compress file.
            // Size on disk: 4,00 KB (4.096 bytes)
            Alphaleonis.Win32.Filesystem.File.Compress(file);


            var fileLength = Alphaleonis.Win32.Filesystem.File.GetCompressedSize(file);

            Console.WriteLine("\tFile size (compressed)  : {0:N0} bytes ({1})", fileLength, Alphaleonis.Utils.UnitSizeToText(fileLength));


            Assert.IsTrue(fileLength == compressedSize && fileLength != streamLength, "File should be [{0:N0}] bytes in size.", compressedSize);
         }

         Console.WriteLine();
      }
   }
}
