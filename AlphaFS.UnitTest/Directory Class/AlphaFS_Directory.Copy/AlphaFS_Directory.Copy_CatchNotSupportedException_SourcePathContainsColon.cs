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
   public partial class AlphaFS_Directory_CopyTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void AlphaFS_Directory_Copy_CatchNotSupportedException_SourcePathContainsColon_LocalAndNetwork_Success()
      {
         Directory_Copy_CatchNotSupportedException_SourcePathContainsColon(false);
         Directory_Copy_CatchNotSupportedException_SourcePathContainsColon(true);
      }


      private void Directory_Copy_CatchNotSupportedException_SourcePathContainsColon(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);
         Console.WriteLine();


         var gotException = false;


         var colonText = @"\My:DirectoryPath";
         var folderSrc = (isNetwork ? Alphaleonis.Win32.Filesystem.Path.LocalToUnc(UnitTestConstants.TempFolder) : UnitTestConstants.TempFolder + @"\dev\test") + colonText;

         Console.WriteLine("Src Directory Path: [{0}]", folderSrc);


         try
         {
            Alphaleonis.Win32.Filesystem.Directory.Copy(folderSrc, "does_not_matter_for_this_test");
         }
         catch (Exception ex)
         {
            var exType = ex.GetType();

            gotException = exType == typeof(NotSupportedException);

            Console.WriteLine("\n\tCaught {0} Exception: [{1}] {2}", gotException ? "EXPECTED" : "UNEXPECTED", exType.Name, ex.Message);
         }


         Assert.IsTrue(gotException, "The exception is not caught, but is expected to.");

         Assert.IsFalse(System.IO.Directory.Exists("does_not_matter_for_this_test"), "The directory exists, but is expected not to.");


         Console.WriteLine();
      }
   }
}
