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
   public partial class CopyTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void AlphaFS_Directory_Copy_SymbolicLink_SourceIsASymbolicLink_TargetMustAlsoBeASymbolicLink_LocalAndNetwork_Success()
      {
         UnitTestAssert.IsElevatedProcess();

         AlphaFS_Directory_Copy_SymbolicLink_SourceIsASymbolicLink_TargetMustAlsoBeASymbolicLink(false);
         AlphaFS_Directory_Copy_SymbolicLink_SourceIsASymbolicLink_TargetMustAlsoBeASymbolicLink(true);
      }
      
      
      private void AlphaFS_Directory_Copy_SymbolicLink_SourceIsASymbolicLink_TargetMustAlsoBeASymbolicLink(bool isNetwork)
      {
         using (var tempRoot = new TemporaryDirectory(isNetwork))
         {
            var sourceFolderLink = System.IO.Path.Combine(tempRoot.Directory.FullName, "SourceDirectoryLink-ToOriginalDirectory");

            var dirInfo = new System.IO.DirectoryInfo(System.IO.Path.Combine(tempRoot.Directory.FullName, "OriginalDirectory"));
            dirInfo.Create();

            Console.WriteLine("Input Directory Path: [{0}]", dirInfo.FullName);
            Console.WriteLine("Input Directory Link: [{0}]", sourceFolderLink);

            Alphaleonis.Win32.Filesystem.Directory.CreateSymbolicLink(sourceFolderLink, dirInfo.FullName);


            var destinationFolderLink = System.IO.Path.Combine(tempRoot.Directory.FullName, "DestinationDirectoryLink-ToOriginalDirectory");

            Alphaleonis.Win32.Filesystem.Directory.Copy(sourceFolderLink, destinationFolderLink, Alphaleonis.Win32.Filesystem.CopyOptions.CopySymbolicLink);


            var lviSrc = Alphaleonis.Win32.Filesystem.Directory.GetLinkTargetInfo(sourceFolderLink);
            var lviDst = Alphaleonis.Win32.Filesystem.Directory.GetLinkTargetInfo(destinationFolderLink);

            Console.WriteLine("\n\tLink Info of source Link:");
            UnitTestConstants.Dump(lviSrc);

            Console.WriteLine("\n\tLink Info of copied Link:");
            UnitTestConstants.Dump(lviDst);


            Assert.AreEqual(lviSrc.PrintName, lviDst.PrintName);

            Assert.AreEqual(lviSrc.SubstituteName, lviDst.SubstituteName);
         }

         
         Console.WriteLine();
      }
   }
}
