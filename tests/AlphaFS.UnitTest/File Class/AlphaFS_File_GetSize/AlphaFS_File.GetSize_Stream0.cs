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
      public void AlphaFS_File_GetSize_Stream0_LocalAndNetwork_Success()
      {
         AlphaFS_File_GetSize_Stream0(false);
         AlphaFS_File_GetSize_Stream0(true);
      }
      

      private void AlphaFS_File_GetSize_Stream0(bool isNetwork)
      {
         using (var tempRoot = new TemporaryDirectory(isNetwork))
         {
            var file = tempRoot.CreateFile();

            Console.WriteLine("Input File Path: [{0}]\n", file.FullName);

            var fileLength = Alphaleonis.Win32.Filesystem.File.GetSize(file.FullName);

            Console.WriteLine("\tSystem.IO File size: {0:N0} bytes ({1})", file.Length, Alphaleonis.Utils.UnitSizeToText(file.Length));
            Console.WriteLine("\tAlphaFS   File size: {0:N0} bytes ({1})", fileLength, Alphaleonis.Utils.UnitSizeToText(fileLength));
            
            Assert.AreEqual(file.Length, fileLength, "The file sizes do not match, but are expected to.");
         }

         Console.WriteLine();
      }
   }
}
