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

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AlphaFS.UnitTest
{
   public partial class EnumerationTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void AlphaFS_Directory_EnumerateFileSystemEntryInfos_TypeFileSystemEntryInfo_LocalAndNetwork_Success()
      {
         AlphaFS_Directory_EnumerateFileSystemEntryInfos_TypeFileSystemEntryInfo(false);
         AlphaFS_Directory_EnumerateFileSystemEntryInfos_TypeFileSystemEntryInfo(true);
      }


      private void AlphaFS_Directory_EnumerateFileSystemEntryInfos_TypeFileSystemEntryInfo(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         var tempPath = Environment.SystemDirectory;
         if (isNetwork)
            tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);

         Console.WriteLine("Input Directory Path: [{0}]\n", tempPath);


         var count = 0;

         foreach (var fsei in Alphaleonis.Win32.Filesystem.Directory.EnumerateFileSystemEntryInfos<Alphaleonis.Win32.Filesystem.FileSystemEntryInfo>(tempPath))
         {
            Assert.IsTrue(fsei.GetType().IsEquivalentTo(typeof(Alphaleonis.Win32.Filesystem.FileSystemEntryInfo)));
            count++;
         }


         Assert.IsTrue(count > 0, "Nothing is enumerated, but it is expected.");
      }
   }
}
