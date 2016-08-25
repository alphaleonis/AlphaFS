/*  Copyright (C) 2008-2015 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
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
   partial class FileTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>

      [TestMethod]
      public void AlphaFS_File_GetCompressedSize_LocalAndUNC_Success()
      {
         File_GetCompressedSize(false);
         File_GetCompressedSize(true);
      }




      private void File_GetCompressedSize(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         string tempPath = System.IO.Path.GetTempPath();
         if (isNetwork)
            tempPath = PathUtils.AsUncPath(tempPath);


         using (var rootDir = new TemporaryDirectory(tempPath, "File-GetCompressedSize"))
         {
            string file = rootDir.RandomFileFullPath;

            long streamLength;
            int thousand = 100 * UnitTestConstants.TenNumbers.Length;
            int compressedSize = 4096;


            // Size: 9,76 KB(10.000 bytes)
            // Size on disk: 12,0 KB(12.288 bytes)
            using (System.IO.FileStream fs = System.IO.File.Create(file))
            {
               // According to NotePad++, creates a file type: "ANSI", which is reported as: "Unicode (UTF-8)".

               for (var count = 0; count < thousand; count ++)
                  fs.Write(UnitTestConstants.StringToByteArray(UnitTestConstants.TenNumbers), 0, UnitTestConstants.TenNumbers.Length);

               streamLength = fs.Length;
            }


            // Compress file.
            // Size on disk: 4,00 KB (4.096 bytes)
            Alphaleonis.Win32.Filesystem.File.Compress(file);


            var fileLength = Alphaleonis.Win32.Filesystem.File.GetCompressedSize(file);

            Assert.IsTrue(fileLength == compressedSize && fileLength != streamLength, "File should be [{0}] bytes in size.", compressedSize);
         }

         Console.WriteLine();
      }
   }
}
