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
         var path = Environment.SystemDirectory;

         Shell32_PathFileExists(path, true);

         Shell32_PathFileExists(Alphaleonis.Win32.Filesystem.Path.LocalToUnc(path), true);

         Shell32_PathFileExists("BlaBlaBla", false);

         Shell32_PathFileExists(System.IO.Path.Combine(Environment.SystemDirectory, "BlaBlaBla"), false);


         foreach (var file in System.IO.Directory.EnumerateFiles(path))

            Assert.IsTrue(Alphaleonis.Win32.Filesystem.Shell32.PathFileExists(file));
      }

      
      private static void Shell32_PathFileExists(string path, bool doesExist)
      {
         var fileExists = Alphaleonis.Win32.Filesystem.Shell32.PathFileExists(path);

         if (doesExist)
            Assert.IsTrue(fileExists);

         if (!doesExist)
            Assert.IsTrue(!fileExists);
      }
   }
}
