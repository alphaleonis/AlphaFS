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
   /// <summary>This is a test class for Path and is intended to contain all Path Unit Tests.</summary>
   public partial class PathTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>

      [TestMethod]
      public void Path_GetFullPath_CompareWithSystemIO_Success()
      {
         Console.WriteLine("Path.GetFullPath()");

         var pathCnt = 0;
         var errorCnt = 0;
         var skipAssert = false;


         foreach (var path in UnitTestConstants.InputPaths)
         {
            string expected = null;
            string actual = null;
            skipAssert = false;

            Console.WriteLine("\n#{0:000}\tInput Path: [{1}]", ++pathCnt, path);

            // System.IO
            try
            {
               expected = System.IO.Path.GetFullPath(path);
            }
            catch (Exception ex)
            {
               skipAssert = ex is ArgumentException;

               Console.WriteLine("\tCaught [System.IO] {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
            }
            Console.WriteLine("\t   System.IO : [{0}]", expected ?? "NULL");


            // AlphaFS
            try
            {
               actual = Alphaleonis.Win32.Filesystem.Path.GetFullPath(path);

               if (!skipAssert)
                  Assert.AreEqual(expected, actual);
            }
            catch (Exception ex)
            {
               errorCnt++;

               Console.WriteLine("\tCaught [AlphaFS]   {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
            }
            Console.WriteLine("\t   AlphaFS   : [{0}]", actual ?? "NULL");
         }
         Console.WriteLine();

         Assert.AreEqual(0, errorCnt, "Encountered paths where AlphaFS != System.IO");
      }


      [TestMethod]
      public void Path_GetFullPath_CatchArgumentException_InvalidPath1_Success()
      {
         var gotException = false;
         try
         {
            Alphaleonis.Win32.Filesystem.Path.GetFullPath(UnitTestConstants.SysDrive + @"\?test.txt");
            //Path.GetFullPath(UnitTestConstants.SysDrive + @"\*test.txt");
            //Path.GetFullPath(UnitTestConstants.SysDrive + @"\\test.txt");
            //Path.GetFullPath(UnitTestConstants.SysDrive + @"\/test.txt");
         }
         catch (Exception ex)
         {
            var exName = ex.GetType().Name;
            gotException = exName.Equals("ArgumentException", StringComparison.OrdinalIgnoreCase);
            Console.WriteLine("\n\tCaught {0} Exception: [{1}] {2}", gotException ? "EXPECTED" : "UNEXPECTED", exName, ex.Message);
         }
         Assert.IsTrue(gotException, "The exception is not caught, but is expected to.");
      }


      [TestMethod]
      public void Path_GetFullPath_CatchArgumentException_InvalidPath2_Success()
      {
         var gotException = false;
         try
         {
            Alphaleonis.Win32.Filesystem.Path.GetFullPath(@"\\\\.txt");
         }
         catch (Exception ex)
         {
            var exName = ex.GetType().Name;
            gotException = exName.Equals("ArgumentException", StringComparison.OrdinalIgnoreCase);
            Console.WriteLine("\n\tCaught {0} Exception: [{1}] {2}", gotException ? "EXPECTED" : "UNEXPECTED", exName, ex.Message);
         }
         Assert.IsTrue(gotException, "The exception is not caught, but is expected to.");
      }


      [TestMethod]
      public void Path_GetFullPath_CatchNotSupportedException_Success()
      {
         var gotException = false;
         try
         {
            Alphaleonis.Win32.Filesystem.Path.GetFullPath(UnitTestConstants.SysDrive + @"\dev\test\aaa:aaa.txt");
         }
         catch (Exception ex)
         {
            var exName = ex.GetType().Name;
            gotException = exName.Equals("NotSupportedException", StringComparison.OrdinalIgnoreCase);
            Console.WriteLine("\n\tCaught {0} Exception: [{1}] {2}", gotException ? "EXPECTED" : "UNEXPECTED", exName, ex.Message);
         }
         Assert.IsTrue(gotException, "The exception is not caught, but is expected to.");
      }
      

      [TestMethod]
      public void AlphaFS_Path_GetFullPath_WithTrailingDotOrSpace_LocalAndNetwork_Success()
      {
         Path_GetFullPath_WithTrailingDotOrSpace(false);
         Path_GetFullPath_WithTrailingDotOrSpace(true);
      }
      



      private void Path_GetFullPath_WithTrailingDotOrSpace(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);


         const string characterDot = ".";
         const string characterSpace = " ";
         var tempPathDot = "Path-with-trailing-dot-" + characterDot;
         var tempPathSpace = "Path-with-trailing-space-" + characterSpace;


         Console.WriteLine();
         Console.WriteLine("Input Path (with dot)  : [{0}]", tempPathDot);
         Console.WriteLine("Input Path (with space): [{0}]", tempPathSpace);




         // By default, trailing dot or space is removed.
         var fileDot = Alphaleonis.Win32.Filesystem.Path.GetFullPath(tempPathDot);
         var fileSpace = Alphaleonis.Win32.Filesystem.Path.GetFullPath(tempPathSpace);
         if (isNetwork)
         {
            fileDot = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(fileDot);
            fileSpace = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(fileSpace);
         }
         Console.WriteLine();
         Console.WriteLine("Path (without dot)  : [{0}]", fileDot);
         Console.WriteLine("Path (without space): [{0}]", fileSpace);

         Assert.IsFalse(fileDot.EndsWith(characterDot), "The path has a trailing dot, but is expected not to.");
         Assert.IsFalse(fileSpace.EndsWith(characterSpace), "The path has a trailing space, but is expected not to.");




         fileDot = Alphaleonis.Win32.Filesystem.Path.GetFullPath(tempPathDot, Alphaleonis.Win32.Filesystem.GetFullPathOptions.KeepDotOrSpace);
         fileSpace = Alphaleonis.Win32.Filesystem.Path.GetFullPath(tempPathSpace, Alphaleonis.Win32.Filesystem.GetFullPathOptions.KeepDotOrSpace);
         if (isNetwork)
         {
            fileDot = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(fileDot);
            fileSpace = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(fileSpace);
         }
         Console.WriteLine();
         Console.WriteLine("Path (with dot)  : [{0}]", fileDot);
         Console.WriteLine("Path (with space): [{0}]", fileSpace);

         Assert.IsTrue(fileDot.EndsWith(characterDot), "The path does not have a trailing dot, but is expected to.");
         Assert.IsTrue(fileSpace.EndsWith(characterSpace), "The path does not have a trailing space, but is expected to.");

         Console.WriteLine();
      }
   }
}
