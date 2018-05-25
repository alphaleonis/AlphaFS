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

namespace AlphaFS.UnitTest
{
   partial class AlphaFS_JunctionsLinksTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void AlphaFS_File_Copy_CopySymbolicLink_SourceIsASymbolicLink_TargetMustAlsoBeASymbolicLink_LocalAndNetwork_Success()
      {
         if (!UnitTestConstants.IsAdmin())
            Assert.Inconclusive();


         File_Copy_CopySymbolicLink_SourceIsASymbolicLink_TargetMustAlsoBeASymbolicLink(false);
         File_Copy_CopySymbolicLink_SourceIsASymbolicLink_TargetMustAlsoBeASymbolicLink(true);
      }


      private void File_Copy_CopySymbolicLink_SourceIsASymbolicLink_TargetMustAlsoBeASymbolicLink(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         var tempPath = UnitTestConstants.TempFolder;
         if (isNetwork)
            tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);


         using (var rootDir = new TemporaryDirectory(tempPath, MethodBase.GetCurrentMethod().Name))
         {
            var sourceFileLink = System.IO.Path.Combine(rootDir.Directory.FullName, "SourceFileLink-ToOriginalFile.txt");

            var fileInfo = new System.IO.FileInfo(System.IO.Path.Combine(rootDir.Directory.FullName, "OriginalFile.txt"));
            using (fileInfo.CreateText()) { }

            Console.WriteLine("\nInput File Path: [{0}]", fileInfo.FullName);
            Console.WriteLine("Input File Link: [{0}]", sourceFileLink);

            Alphaleonis.Win32.Filesystem.File.CreateSymbolicLink(sourceFileLink, fileInfo.FullName);


            var destinationFileLink = System.IO.Path.Combine(rootDir.Directory.FullName, "DestinationFileLink-ToOriginalFile.txt");

            Alphaleonis.Win32.Filesystem.File.Copy(sourceFileLink, destinationFileLink, Alphaleonis.Win32.Filesystem.CopyOptions.CopySymbolicLink);


            var lviSrc = Alphaleonis.Win32.Filesystem.File.GetLinkTargetInfo(sourceFileLink);
            var lviDst = Alphaleonis.Win32.Filesystem.File.GetLinkTargetInfo(destinationFileLink);

            Console.WriteLine("\n\tLink Info of source Link:");
            UnitTestConstants.Dump(lviSrc, -14);

            Console.WriteLine("\n\tLink Info of copied Link:");
            UnitTestConstants.Dump(lviDst, -14);


            Assert.AreEqual(lviSrc.PrintName, lviDst.PrintName);
            Assert.AreEqual(lviSrc.SubstituteName, lviDst.SubstituteName);
         }

         
         Console.WriteLine();
      }
   }
}
