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
   public partial class DirectoryInfoTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void DirectoryInfo_CreateInstance_ExistingFileAsDirectoryName_PropertyExistsShouldBeFalse_LocalAndNetwork_Success()
      {
         DirectoryInfo_CreateInstance_ExistingFileAsDirectoryName_PropertyExistsShouldBeFalse(false);
         DirectoryInfo_CreateInstance_ExistingFileAsDirectoryName_PropertyExistsShouldBeFalse(true);
      }


      private void DirectoryInfo_CreateInstance_ExistingFileAsDirectoryName_PropertyExistsShouldBeFalse(bool isNetwork)
      {
         using (var tempRoot = new TemporaryDirectory(isNetwork))
         {
            var file = tempRoot.CreateFileRandomizedAttributes();

            Console.WriteLine("Input File Path: [{0}]", file.FullName);

            var systemIODirInfo = new System.IO.DirectoryInfo(file.FullName);

            var alphaFSDirInfo = new Alphaleonis.Win32.Filesystem.DirectoryInfo(file.FullName);

            UnitTestConstants.Dump(alphaFSDirInfo);


            Assert.AreEqual(systemIODirInfo.Exists, alphaFSDirInfo.Exists);

            Assert.IsFalse(alphaFSDirInfo.EntryInfo.IsDirectory);
         }

         Console.WriteLine();
      }
   }
}
