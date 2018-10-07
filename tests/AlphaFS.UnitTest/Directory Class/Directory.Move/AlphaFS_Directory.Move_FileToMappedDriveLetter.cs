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
      public void AlphaFS_Directory_Move_FileToMappedDriveLetter_LocalAndNetwork_Success()
      {
         // Do not pass isNetwork, as to always use UnitTestConstants.TempPath

         using (var tempRoot = new TemporaryDirectory())
         {
            var srcFileName = "Src file.log";
            var dstFileName = "Dst file.log";

            var srcFile = System.IO.Path.Combine(tempRoot.Directory.FullName, srcFileName);


            var drive = Alphaleonis.Win32.Filesystem.DriveInfo.GetFreeDriveLetter() + @":\";

            var share = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempRoot.CreateDirectory().FullName);

            var dstFile = System.IO.Path.Combine(drive, dstFileName);
            

            try
            {
               Alphaleonis.Win32.Network.Host.ConnectDrive(drive, share);

               Console.WriteLine("Mapped drive [{0}] to [{1}]\n", drive, share);


               System.IO.File.AppendAllText(srcFile, new string('*', new Random().Next(1, 1024)));

               var srcFileSize = new System.IO.FileInfo(srcFile).Length;

               const Alphaleonis.Win32.Filesystem.MoveOptions options = Alphaleonis.Win32.Filesystem.MoveOptions.CopyAllowed | Alphaleonis.Win32.Filesystem.MoveOptions.WriteThrough | Alphaleonis.Win32.Filesystem.MoveOptions.ReplaceExisting;

               Console.WriteLine("Src File Path: [{0}]", srcFile);
               Console.WriteLine("Dst File Path: [{0}]", dstFile);


               var cmr = Alphaleonis.Win32.Filesystem.Directory.Move(srcFile, dstFile, options);

               UnitTestConstants.Dump(cmr);


               Assert.IsFalse(System.IO.Directory.Exists(dstFile));

               Assert.IsTrue(System.IO.File.Exists(dstFile));

               Assert.AreEqual(srcFileSize, new System.IO.FileInfo(dstFile).Length);


               Assert.IsTrue(cmr.IsCopy);
               Assert.IsTrue(cmr.IsEmulatedMove);

               Assert.IsTrue(cmr.IsFile);
               Assert.IsFalse(cmr.IsDirectory);

               Assert.IsFalse(cmr.IsCanceled);
               Assert.AreEqual(0, cmr.TotalFolders);
               Assert.AreEqual(1, cmr.TotalFiles);
            }
            finally
            {
               Alphaleonis.Win32.Network.Host.DisconnectDrive(drive);
            }
         }

         Console.WriteLine();
      }
   }
}
