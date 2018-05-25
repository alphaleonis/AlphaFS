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
   public partial class File_MoveTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void File_Move_Overwrite_DestinationFileAlreadyExists_LocalAndNetwork_Success()
      {
         File_Move_Overwrite_DestinationFileAlreadyExists(false);
         File_Move_Overwrite_DestinationFileAlreadyExists(true);
      }


      private void File_Move_Overwrite_DestinationFileAlreadyExists(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         var tempPath = UnitTestConstants.TempFolder;
         if (isNetwork)
            tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);


         using (var rootDir = new TemporaryDirectory(tempPath, MethodBase.GetCurrentMethod().Name))
         {
            var fileSource = UnitTestConstants.CreateFile(rootDir.Directory.FullName);
            var fileCopy = rootDir.RandomFileFullPath;
            Console.WriteLine("\nSource File Path: [{0}]", fileSource);

            // Copy it.
            System.IO.File.Copy(fileSource.FullName, fileCopy);


            var gotException = false;
            try
            {
               Alphaleonis.Win32.Filesystem.File.Move(fileSource.FullName, fileCopy);
            }
            catch (Exception ex)
            {
               var exType = ex.GetType();

               gotException = exType == typeof(Alphaleonis.Win32.Filesystem.AlreadyExistsException);

               Console.WriteLine("\n\tCaught {0} Exception: [{1}] {2}", gotException ? "EXPECTED" : "UNEXPECTED", exType.Name, ex.Message);


               Alphaleonis.Win32.Filesystem.File.Move(fileSource.FullName, fileCopy, Alphaleonis.Win32.Filesystem.MoveOptions.ReplaceExisting);

               Assert.IsFalse(System.IO.File.Exists(fileSource.FullName), "The file does exists, but is expected not to.");
               Assert.IsTrue(System.IO.File.Exists(fileCopy), "The file does not exists, but is expected to.");
            }

            Assert.IsTrue(gotException, "The exception is not caught, but is expected to.");
         }

         Console.WriteLine();
      }
   }
}
