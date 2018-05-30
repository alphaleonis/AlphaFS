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
   partial class FileTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void File_OpenText_LocalAndNetwork_Success()
      {
         File_OpenText(false);
         File_OpenText(true);
      }

      
      private void File_OpenText(bool isNetwork)
      {
         using (var tempRoot = new TemporaryDirectory(isNetwork))
         {
            var file = tempRoot.RandomTxtFileFullPath;

            Console.WriteLine("Input File Path: [{0}]\n", file);


            System.IO.File.WriteAllText(file, DateTime.Now.ToString(CultureInfo.CurrentCulture));


            var sysIOStreamText = string.Empty;
            using (var stream = System.IO.File.OpenText(file))
               sysIOStreamText = stream.ReadLine();


            var alphaFSStreamText = string.Empty;
            using (var stream = Alphaleonis.Win32.Filesystem.File.OpenText(file))
               alphaFSStreamText = stream.ReadLine();


            Console.WriteLine("\tSystem IO: " + sysIOStreamText);
            Console.WriteLine("\tAlphaFS  : " + alphaFSStreamText);


            Assert.AreEqual(sysIOStreamText, alphaFSStreamText, "The content of the two files is not equal, but is expected to.");
         }

         Console.WriteLine();
      }
   }
}
