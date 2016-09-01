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
using System.IO;
using Alphaleonis.Win32.Filesystem;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Path = System.IO.Path;
using File = System.IO.File;

namespace AlphaFS.UnitTest
{
   /// <summary>This is a test class for several AlphaFS instance classes.</summary>
   partial class AlphaFS_ClassesTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>
      
      [TestMethod]
      public void BackupFileStream_LocalAndUNC_Success()
      {
         BackupFileStream(false);
         BackupFileStream(true);
      }
      



      private void BackupFileStream(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         if (!File.Exists(UnitTestConstants.NotepadExe))
            Assert.Inconclusive("Test ignored because {0} was not found.", UnitTestConstants.NotepadExe);

         string tempPath = Path.GetTempPath();
         if (isNetwork)
            tempPath = PathUtils.AsUncPath(tempPath);
         

         using (var rootDir = new TemporaryDirectory(tempPath, "BackupFileStream"))
         {
            string file = Path.Combine(rootDir.Directory.FullName, Path.GetFileName(UnitTestConstants.NotepadExe));
            Console.WriteLine("\nInput File Path: [{0}]\n", file);


            File.Copy(UnitTestConstants.NotepadExe, file);
            

            using (var bfs = new BackupFileStream(file, FileMode.Open))
            {
               var catchCount = 0;


               #region IOException

               bfs.Lock(0, 10);

               var gotException = false;
               try
               {
                  bfs.Lock(0, 10);
               }
               catch (Exception ex)
               {
                  var exName = ex.GetType().Name;
                  gotException = exName.Equals("IOException", StringComparison.OrdinalIgnoreCase);
                  Console.WriteLine("\tCaught Exception: [{0}] Message: [{1}]", exName, ex.Message);
               }
               Assert.IsTrue(gotException, "The exception is not caught, but is expected to.");
               Console.WriteLine();

               bfs.Unlock(0, 10);

               #endregion // IOException

               #region IOException #2

               gotException = false;
               try
               {
                  bfs.Unlock(0, 10);
               }
               catch (Exception ex)
               {
                  var exName = ex.GetType().Name;
                  gotException = exName.Equals("IOException", StringComparison.OrdinalIgnoreCase);
                  Console.WriteLine("\tCaught Exception: [{0}] Message: [{1}]", exName, ex.Message);
               }
               Assert.IsTrue(gotException, "The exception is not caught, but is expected to.");
               Console.WriteLine();

               #endregion // IOException #2


               UnitTestConstants.Dump(bfs.ReadStreamInfo(), -10);
               Console.WriteLine();

               UnitTestConstants.Dump(bfs, -14);
               Console.WriteLine();
            }
         }

         Console.WriteLine();
      }
   }
}
