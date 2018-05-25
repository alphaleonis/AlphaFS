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
   public partial class AlphaFS_Shell32Test
   {
      [TestMethod]
      public void AlphaFS_Shell32_PathFileExists()
      {
         Console.WriteLine("Filesystem.Shell32.PathFileExists()");

         var path = UnitTestConstants.SysRoot;
         Shell32_PathFileExists(path, true);
         Shell32_PathFileExists(Alphaleonis.Win32.Filesystem.Path.LocalToUnc(path), true);
         Shell32_PathFileExists("BlaBlaBla", false);
         Shell32_PathFileExists(System.IO.Path.Combine(UnitTestConstants.SysRoot, "BlaBlaBla"), false);

         var cnt = 0;
         foreach (var file in System.IO.Directory.EnumerateFiles(UnitTestConstants.SysRoot))
         {
            var fileExists = Alphaleonis.Win32.Filesystem.Shell32.PathFileExists(file);

            Console.WriteLine("\t#{0:000}\tShell32.PathFileExists() == [{1}]: {2}\t\t[{3}]", ++cnt, UnitTestConstants.TextTrue, fileExists, file);


            Assert.IsTrue(fileExists);
         }

         Console.WriteLine();
      }




      private void Shell32_PathFileExists(string path, bool doesExist)
      {
         Console.WriteLine("\n\tPath: [{0}]\n", path);

         var fileExists = Alphaleonis.Win32.Filesystem.Shell32.PathFileExists(path);
         Console.WriteLine("\t\tShell32.PathFileExists() == [{0}]: {1}\t\t[{2}]", doesExist ? UnitTestConstants.TextTrue : UnitTestConstants.TextFalse, doesExist == fileExists, path);
         Console.WriteLine("\t\tFile.Exists()            == [{0}]: {1}\t\t[{2}]", doesExist ? UnitTestConstants.TextTrue : UnitTestConstants.TextFalse, doesExist == System.IO.File.Exists(path), path);
         Console.WriteLine("\t\tDirectory.Exists()       == [{0}]: {1}\t\t[{2}]", doesExist ? UnitTestConstants.TextTrue : UnitTestConstants.TextFalse, doesExist == System.IO.Directory.Exists(path), path);


         if (doesExist)
            Assert.IsTrue(fileExists);

         if (!doesExist)
            Assert.IsTrue(!fileExists);
      }
   }
}
