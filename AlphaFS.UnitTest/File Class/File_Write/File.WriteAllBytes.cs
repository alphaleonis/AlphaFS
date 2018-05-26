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
using System.Reflection;
using System.Text;

namespace AlphaFS.UnitTest
{
   public partial class FileTest
   {
      [TestMethod]
      public void File_WriteAllBytes_LocalAndNetwork_Success()
      {
         File_WriteAllBytes(false);
         File_WriteAllBytes(true);

      }


      private void File_WriteAllBytes(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);
         
         using (var tempRoot = new TemporaryDirectory(isNetwork ? Alphaleonis.Win32.Filesystem.Path.LocalToUnc(UnitTestConstants.TempFolder) : UnitTestConstants.TempFolder, MethodBase.GetCurrentMethod().Name))
         {
            var file1 = tempRoot.RandomFileFullPath;
            var file2 = tempRoot.RandomFileFullPath;

            Console.WriteLine("\nInput File1 Path: [{0}]", file1);
            Console.WriteLine("\nInput File2 Path: [{0}]", file2);


            var bytes = Encoding.UTF8.GetBytes(new string('X', new Random(DateTime.Now.Millisecond).Next(10, 1000)));

            System.IO.File.WriteAllBytes(file1, bytes);
            

            Alphaleonis.Win32.Filesystem.File.WriteAllBytes(file2, bytes);
            

            CollectionAssert.AreEquivalent(System.IO.File.ReadAllBytes(file1), System.IO.File.ReadAllBytes(file2));
         }

         Console.WriteLine();
      }
   }
}
