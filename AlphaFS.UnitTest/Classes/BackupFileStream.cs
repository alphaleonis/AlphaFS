/*  Copyright (C) 2008-2015 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
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
using System.ComponentModel;
using System.IO;
using Alphaleonis.Win32;
using Alphaleonis.Win32.Filesystem;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using File = Alphaleonis.Win32.Filesystem.File;
using Path = Alphaleonis.Win32.Filesystem.Path;

namespace AlphaFS.UnitTest
{
   /// <summary>This is a test class for several AlphaFS instance classes.</summary>
   partial class AlphaFS_ClassesTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>

      [TestMethod]
      public void BackupFileStream_Initialize_Local_Success()
      {
         BackupFileStream(false);
      }


      [TestMethod]
      public void BackupFileStream_Initialize_Network_Success()
      {
         BackupFileStream(true);
      }
      

      private void BackupFileStream(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         string random = Path.GetRandomFileName();
         string tempPath = Path.GetTempPath("BackupFileStream()-File-" + random);
         string tempNotepad = tempPath + "-" + Path.GetFileName(UnitTestConstants.NotepadExe);

         if (isNetwork)
         {
            tempPath = Path.LocalToUnc(tempPath);
            tempNotepad = Path.LocalToUnc(tempNotepad);
         }
         

         File.Copy(UnitTestConstants.NotepadExe, tempNotepad);
         
         Console.WriteLine("\nInput File Path: [{0}]", tempNotepad);

         if (!File.Exists(tempNotepad))
            Assert.Inconclusive("Test ignored because {0} was not found.", tempNotepad);


         using (var bfs = new BackupFileStream(tempNotepad, FileMode.Open))
         {
            var catchCount = 0;


            #region IOException

            bfs.Lock(0, 10);

            string expectedException = "System.IO.IOException";
            int expectedLastError = (int)Win32Errors.ERROR_LOCK_VIOLATION;
            bool exception = false;
            try
            {
               Console.WriteLine("\nCatch #{0}: [{1}]: The process cannot access the file because another process has locked a portion of the file.", ++catchCount, expectedException);

               bfs.Lock(0, 10);
            }
            catch (IOException ex)
            {
               var win32Error = new Win32Exception("", ex);
               Assert.IsTrue(win32Error.NativeErrorCode == expectedLastError, string.Format("Expected Win32Exception error should be: [{0}], got: [{1}]", expectedLastError, win32Error.NativeErrorCode));
               Assert.IsTrue(ex.Message.StartsWith("(" + expectedLastError + ")"), string.Format("Expected Win32Exception error is: [{0}]", expectedLastError));

               exception = true;
               Console.WriteLine("\n\t[{0}]: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));

            }
            catch (Exception ex)
            {
               Console.WriteLine("\n\tCaught (unexpected) {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
            }
            Assert.IsTrue(exception, "[{0}] should have been caught.", expectedException);
            Console.WriteLine();

            bfs.Unlock(0, 10);

            #endregion // IOException

            #region IOException #2

            expectedException = "System.IO.IOException";
            expectedLastError = (int)Win32Errors.ERROR_NOT_LOCKED;
            exception = false;
            try
            {
               Console.WriteLine("\nCatch #{0}: [{1}]: The segment is already unlocked.", ++catchCount, expectedException);

               bfs.Unlock(0, 10);
            }
            catch (IOException ex)
            {
               var win32Error = new Win32Exception("", ex);
               Assert.IsTrue(win32Error.NativeErrorCode == expectedLastError, string.Format("Expected Win32Exception error should be: [{0}], got: [{1}]", expectedLastError, win32Error.NativeErrorCode));
               Assert.IsTrue(ex.Message.StartsWith("(" + expectedLastError + ")"), string.Format("Expected Win32Exception error is: [{0}]", expectedLastError));

               exception = true;
               Console.WriteLine("\n\t[{0}]: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));

            }
            catch (Exception ex)
            {
               Console.WriteLine("\n\tCaught (unexpected) {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
            }
            Assert.IsTrue(exception, "[{0}] should have been caught.", expectedException);
            Console.WriteLine();

            #endregion // IOException #2


            UnitTestConstants.Dump(bfs.ReadStreamInfo(), -10);
            Console.WriteLine();

            UnitTestConstants.Dump(bfs, -14);
            Console.WriteLine();
         }

         File.Delete(tempNotepad, true);
         Assert.IsFalse(File.Exists(tempNotepad), "Cleanup failed: File should have been removed.");

         File.Delete(tempPath, true);
         Assert.IsFalse(File.Exists(tempPath), "Cleanup failed: File should have been removed.");
         Console.WriteLine();
      }
   }
}
