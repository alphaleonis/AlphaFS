/*  Copyright (C) 2008-2018 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
 *  
 *  Permission is hereby granted, free of charge, to any person obtaining a copy 
 *  of this software and associated documentation directorys (the "Software"), to deal 
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
   public partial class FileInfoTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void FileInfo_Attributes_LocalAndNetwork_Success()
      {
         FileInfo_Attributes(false);
         FileInfo_Attributes(true);
      }
      

      private void FileInfo_Attributes(bool isNetwork)
      {
         using (var tempRoot = new TemporaryDirectory(isNetwork))
         {
            var file = tempRoot.RandomTxtFileFullPath;

            var alphaFSFileInfo = new Alphaleonis.Win32.Filesystem.FileInfo(file);


            Console.WriteLine("Input File Path: [{0}]", alphaFSFileInfo.FullName);

            using (alphaFSFileInfo.Create())
            {
               alphaFSFileInfo.Attributes |= System.IO.FileAttributes.ReadOnly;
               Assert.IsTrue((alphaFSFileInfo.Attributes & System.IO.FileAttributes.ReadOnly) != 0, "The file is not ReadOnly, but is expected to.");

               alphaFSFileInfo.Attributes &= ~System.IO.FileAttributes.ReadOnly;
               Assert.IsTrue((alphaFSFileInfo.Attributes & System.IO.FileAttributes.ReadOnly) == 0, "The file is ReadOnly, but is expected not to.");


               alphaFSFileInfo.Attributes |= System.IO.FileAttributes.Hidden;
               Assert.IsTrue((alphaFSFileInfo.Attributes & System.IO.FileAttributes.Hidden) != 0, "The file is not Hidden, but is expected to.");

               alphaFSFileInfo.Attributes &= ~System.IO.FileAttributes.Hidden;
               Assert.IsTrue((alphaFSFileInfo.Attributes & System.IO.FileAttributes.Hidden) == 0, "The file is Hidden, but is expected not to.");


               alphaFSFileInfo.Attributes |= System.IO.FileAttributes.System;
               Assert.IsTrue((alphaFSFileInfo.Attributes & System.IO.FileAttributes.System) != 0, "The file is not System, but is expected to.");

               alphaFSFileInfo.Attributes &= ~System.IO.FileAttributes.System;
               Assert.IsTrue((alphaFSFileInfo.Attributes & System.IO.FileAttributes.System) == 0, "The file is System, but is expected not to.");
            }
         }

         Console.WriteLine();
      }
   }
}
