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
      public void AlphaFS_Path_GetSuffixedDirectoryNameWithoutRoot_LocalAndNetwork_Success()
      {
         // Note: since System.IO.Path does not have a similar method,
         // some more work is needed to test the validity of these result.


         AlphaFS_Path_GetSuffixedDirectoryNameWithoutRoot(false);
         AlphaFS_Path_GetSuffixedDirectoryNameWithoutRoot(true);
      }


      private void AlphaFS_Path_GetSuffixedDirectoryNameWithoutRoot(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         var neDir = "Non-Existing Directory";
         var sys32 = (UnitTestConstants.SysRoot32 + Alphaleonis.Win32.Filesystem.Path.DirectorySeparator).Replace(UnitTestConstants.SysDrive + Alphaleonis.Win32.Filesystem.Path.DirectorySeparator, "");


         var fullPath = System.IO.Path.Combine(UnitTestConstants.SysRoot32 + Alphaleonis.Win32.Filesystem.Path.DirectorySeparator, neDir);
         if (isNetwork)
            fullPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(fullPath);

         var suffixedDirectoryNameWithoutRoot = Alphaleonis.Win32.Filesystem.Path.GetSuffixedDirectoryNameWithoutRoot(fullPath);

         Console.WriteLine("\nFull Path                          : " + fullPath);
         Console.WriteLine("GetSuffixedDirectoryNameWithoutRoot: " + suffixedDirectoryNameWithoutRoot);

         Assert.AreEqual(sys32, suffixedDirectoryNameWithoutRoot);




         fullPath = System.IO.Path.Combine(fullPath, "Non-Existing file.txt");
         neDir = (System.IO.Path.Combine(UnitTestConstants.SysRoot32, neDir) + Alphaleonis.Win32.Filesystem.Path.DirectorySeparator).Replace(UnitTestConstants.SysDrive + Alphaleonis.Win32.Filesystem.Path.DirectorySeparator, "");
         if (isNetwork)
            fullPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(fullPath);

         suffixedDirectoryNameWithoutRoot = Alphaleonis.Win32.Filesystem.Path.GetSuffixedDirectoryNameWithoutRoot(fullPath);

         Console.WriteLine("\nFull Path                          : " + fullPath);
         Console.WriteLine("GetSuffixedDirectoryNameWithoutRoot: " + suffixedDirectoryNameWithoutRoot);

         Assert.AreEqual(suffixedDirectoryNameWithoutRoot, neDir);




         fullPath = UnitTestConstants.SysRoot;
         if (isNetwork)
            fullPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(fullPath);

         suffixedDirectoryNameWithoutRoot = Alphaleonis.Win32.Filesystem.Path.GetSuffixedDirectoryNameWithoutRoot(fullPath);

         Console.WriteLine("\nFull Path                          : " + fullPath);
         Console.WriteLine("GetSuffixedDirectoryNameWithoutRoot: " + suffixedDirectoryNameWithoutRoot);

         Assert.AreEqual(null, suffixedDirectoryNameWithoutRoot);




         fullPath = UnitTestConstants.SysDrive + Alphaleonis.Win32.Filesystem.Path.DirectorySeparator;
         if (isNetwork)
            fullPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(fullPath);

         suffixedDirectoryNameWithoutRoot = Alphaleonis.Win32.Filesystem.Path.GetSuffixedDirectoryNameWithoutRoot(fullPath);

         Console.WriteLine("\nFull Path                          : " + fullPath);
         Console.WriteLine("GetSuffixedDirectoryNameWithoutRoot: " + suffixedDirectoryNameWithoutRoot);

         Assert.AreEqual(null, suffixedDirectoryNameWithoutRoot);


         Console.WriteLine();
      }
   }
}
