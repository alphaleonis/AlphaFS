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
   public partial class PathTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void AlphaFS_Path_GetFullPath_WithTrailingDotOrSpace_LocalAndNetwork_Success()
      {
         AlphaFS_Path_GetFullPath_WithTrailingDotOrSpace(false);
         AlphaFS_Path_GetFullPath_WithTrailingDotOrSpace(true);
      }


      private void AlphaFS_Path_GetFullPath_WithTrailingDotOrSpace(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);
         
         const string characterDot = ".";
         const string characterSpace = " ";
         var tempPathDot = "Path-with-trailing-dot-" + characterDot;
         var tempPathSpace = "Path-with-trailing-space-" + characterSpace;


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
