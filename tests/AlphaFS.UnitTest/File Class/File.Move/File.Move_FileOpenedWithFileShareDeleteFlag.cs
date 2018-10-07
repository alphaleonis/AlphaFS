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
   public partial class MoveTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void AlphaFS_File_Move_FileOpenedWithFileShareDeleteFlag_LocalAndNetwork_Success()
      {
         AlphaFS_File_Move_FileOpenedWithFileShareDeleteFlag(false);
         AlphaFS_File_Move_FileOpenedWithFileShareDeleteFlag(true);
      }


      private void AlphaFS_File_Move_FileOpenedWithFileShareDeleteFlag(bool isNetwork)
      {
         using (var tempRoot = new TemporaryDirectory(isNetwork))
         {
            var folder = tempRoot.CreateDirectory();

            var srcFile = System.IO.Path.Combine(folder.FullName, tempRoot.RandomString);

            var dstFile = System.IO.Path.Combine(folder.FullName, tempRoot.RandomString);

            Console.WriteLine("Src File Path: [{0}]", dstFile);
            Console.WriteLine("Dst File Path: [{0}]", dstFile);


            using (System.IO.File.Open(srcFile, System.IO.FileMode.Create, System.IO.FileAccess.ReadWrite, System.IO.FileShare.ReadWrite | System.IO.FileShare.Delete))
            {
               Alphaleonis.Win32.Filesystem.File.Move(srcFile, dstFile);


               Assert.IsFalse(Alphaleonis.Win32.Filesystem.File.Exists(srcFile));

               Assert.IsTrue(Alphaleonis.Win32.Filesystem.File.Exists(dstFile));
            }
         }

         Console.WriteLine();
      }
   }
}
