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
   public partial class SizeTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void AlphaFS_File_Move_DelayUntilRebootFlag_GetSize_LocalAndNetwork_Success()
      {
         AlphaFS_File_Move_DelayUntilRebootFlag_GetSize(false);
         AlphaFS_File_Move_DelayUntilRebootFlag_GetSize(true);
      }


      private void AlphaFS_File_Move_DelayUntilRebootFlag_GetSize(bool isNetwork)
      {
         using (var tempRoot = new TemporaryDirectory(isNetwork))
         {
            var pendingEntry = tempRoot.CreateFile();

            Console.WriteLine("Src File Path: [{0}] [{1}]", Alphaleonis.Utils.UnitSizeToText(pendingEntry.Length), pendingEntry.FullName);


            if (isNetwork)
            {
               UnitTestAssert.ThrowsException<ArgumentException>(() => Alphaleonis.Win32.Filesystem.File.Move(pendingEntry.FullName, null, Alphaleonis.Win32.Filesystem.MoveOptions.DelayUntilReboot));
            }

            else
            {
               var moveOptions = Alphaleonis.Win32.Filesystem.MoveOptions.DelayUntilReboot | Alphaleonis.Win32.Filesystem.MoveOptions.GetSizes;

               var moveResult = Alphaleonis.Win32.Filesystem.File.Move(pendingEntry.FullName, null, moveOptions);


               UnitTestConstants.Dump(moveResult);


               UnitTestAssert.RegistryContainsPendingEntry(pendingEntry.FullName);


               // Test against moveResult results.

               Assert.IsFalse(moveResult.IsCopy);

               Assert.IsTrue(moveResult.IsMove);

               Assert.IsFalse(moveResult.IsDirectory);

               Assert.IsTrue(moveResult.IsFile);

               Assert.AreEqual(1, moveResult.TotalFiles);

               Assert.AreEqual(0, moveResult.TotalFolders);

               Assert.AreEqual(pendingEntry.Length, moveResult.TotalBytes);

               Assert.IsNull(moveResult.Destination);

               Assert.IsTrue(System.IO.File.Exists(pendingEntry.FullName), "The file does not exists, but it is expected.");
            }
         }


         Console.WriteLine();
      }
   }
}
