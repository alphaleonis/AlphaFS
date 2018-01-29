/*  Copyright (C) 2008-2017 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
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
   public partial class File_CopyTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void File_Copy_CatchFileNotFoundException_NonExistingSourceFile_LocalAndNetwork_Success()
      {
         File_Copy_CatchFileNotFoundException_NonExistingSourceFile(false);
         File_Copy_CatchFileNotFoundException_NonExistingSourceFile(true);
      }


      private void File_Copy_CatchFileNotFoundException_NonExistingSourceFile(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);
         Console.WriteLine();


         var gotException = false;


         var srcFile = UnitTestConstants.SysDrive + @"\NonExisting Source File";
         var dstFile = UnitTestConstants.SysDrive + @"\NonExisting Destination File";

         Console.WriteLine("Src File Path: [{0}]", srcFile);
         Console.WriteLine("Dst File Path: [{0}]", dstFile);



         try
         {
            Alphaleonis.Win32.Filesystem.File.Copy(srcFile, dstFile);
         }
         catch (Exception ex)
         {
            var exType = ex.GetType();

            gotException = exType == typeof(System.IO.FileNotFoundException);

            Console.WriteLine("\n\tCaught {0} Exception: [{1}] {2}", gotException ? "EXPECTED" : "UNEXPECTED", exType.Name, ex.Message);
         }


         Assert.IsTrue(gotException, "The exception is not caught, but is expected to.");

         Assert.IsFalse(System.IO.Directory.Exists(dstFile), "The file exists, but is expected not to.");


         Console.WriteLine();
      }
   }
}
