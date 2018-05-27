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
   public partial class MoveTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void Directory_Move_UsingFile_LocalAndNetwork_Success()
      {
         Directory_Move_UsingFile(false);
         Directory_Move_UsingFile(true);
      }


      private void Directory_Move_UsingFile(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);
         Console.WriteLine();


         // Change the drive letter to suit your environment.
         var destinationDriveLetter = "D";
         var destinationDrivePath = destinationDriveLetter + Alphaleonis.Win32.Filesystem.Path.VolumeSeparator +Alphaleonis.Win32.Filesystem.Path.DirectorySeparator;


         if (!Alphaleonis.Win32.Filesystem.Directory.ExistsDrive(destinationDrivePath, false))
            UnitTestAssert.SetInconclusive("This unit tests needs an additional drive: [" + destinationDrivePath + "]");


         var tempPath = UnitTestConstants.TempFolder;
         if (isNetwork)
            tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);


         using (var rootDir = new TemporaryDirectory(tempPath, MethodBase.GetCurrentMethod().Name))
         {
            const string srcFileName = "Src file.log";
            const string dstFileName = "Dst file.log";

            var srcFile = System.IO.Path.Combine(rootDir.Directory.FullName, srcFileName);

            var dstFile = System.IO.Path.Combine(destinationDrivePath, dstFileName);
            if (isNetwork)
               dstFile = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(dstFile);


            System.IO.File.AppendAllText(srcFile, new string('*', new Random().Next(1, 1024)));

            const Alphaleonis.Win32.Filesystem.MoveOptions options = Alphaleonis.Win32.Filesystem.MoveOptions.CopyAllowed | Alphaleonis.Win32.Filesystem.MoveOptions.WriteThrough | Alphaleonis.Win32.Filesystem.MoveOptions.ReplaceExisting;

            Console.WriteLine("Src File Path: [{0}]", srcFile);
            Console.WriteLine("Dst File Path: [{0}]", dstFile);




            var cmr = Alphaleonis.Win32.Filesystem.Directory.Move(srcFile, dstFile, options);

            UnitTestConstants.Dump(cmr, -18);


            Assert.IsFalse(System.IO.Directory.Exists(dstFile));

            Assert.IsTrue(System.IO.File.Exists(dstFile));

            Assert.AreEqual(new System.IO.FileInfo(srcFile).Length, new System.IO.FileInfo(dstFile).Length);


            Assert.IsTrue(cmr.IsCopy);
            Assert.IsTrue(cmr.IsEmulatedMove);

            Assert.IsTrue(cmr.IsFile);
            Assert.IsFalse(cmr.IsDirectory);

            Assert.IsFalse(cmr.IsCanceled);
            Assert.AreEqual(0, cmr.TotalFolders);
            Assert.AreEqual(1, cmr.TotalFiles);


            System.IO.File.Delete(dstFile);
         }


         Console.WriteLine();
      }
   }
}
