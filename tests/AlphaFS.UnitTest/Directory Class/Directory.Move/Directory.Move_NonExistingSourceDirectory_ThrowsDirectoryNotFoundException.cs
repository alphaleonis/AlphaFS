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
      public void Directory_Move_NonExistingSourceDirectory_ThrowsDirectoryNotFoundException_LocalAndNetwork_Success()
      {
         Directory_Move_NonExistingSourceDirectory_ThrowsDirectoryNotFoundException(false);
         Directory_Move_NonExistingSourceDirectory_ThrowsDirectoryNotFoundException(true);
      }


      private void Directory_Move_NonExistingSourceDirectory_ThrowsDirectoryNotFoundException(bool isNetwork)
      {
         using (var tempRoot = new TemporaryDirectory(isNetwork))
         {
            var srcFolder = System.IO.Path.Combine(tempRoot.Directory.FullName, "NonExisting Source Folder");
            var dstFolder = System.IO.Path.Combine(tempRoot.Directory.FullName, "NonExisting Destination Folder");

            if (isNetwork)
            {
               srcFolder = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(srcFolder);
               dstFolder = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(dstFolder);
            }

            Console.WriteLine("Src Directory Path: [{0}]", srcFolder);
            Console.WriteLine("Dst Directory Path: [{0}]", dstFolder);


            UnitTestAssert.ThrowsException<System.IO.DirectoryNotFoundException>(() => System.IO.Directory.Move(srcFolder, dstFolder), srcFolder);

            UnitTestAssert.ThrowsException<System.IO.DirectoryNotFoundException>(() => Alphaleonis.Win32.Filesystem.Directory.Move(srcFolder, dstFolder), srcFolder);
         }

         Console.WriteLine();
      }
   }
}
