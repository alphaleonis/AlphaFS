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
      public void AlphaFS_Path_GetSuffixedDirectoryName_LocalAndNetwork_Success()
      {
         AlphaFS_Path_GetSuffixedDirectoryName(false);
         AlphaFS_Path_GetSuffixedDirectoryName(true);
      }


      private void AlphaFS_Path_GetSuffixedDirectoryName(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         var neDir = "Non-Existing Directory";
         var sys32 = UnitTestConstants.SysRoot32 + Alphaleonis.Win32.Filesystem.Path.DirectorySeparator;


         var fullPath = System.IO.Path.Combine(UnitTestConstants.SysRoot32, neDir);
         if (isNetwork)
         {
            fullPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(fullPath);
            sys32 = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(sys32);
         }

         var suffixedDirectoryName = Alphaleonis.Win32.Filesystem.Path.GetSuffixedDirectoryName(fullPath);

         Console.WriteLine("Full Path               : " + fullPath);
         Console.WriteLine("GetSuffixedDirectoryName: " + suffixedDirectoryName);
         
         Assert.IsTrue(suffixedDirectoryName.Equals(sys32, StringComparison.OrdinalIgnoreCase));




         fullPath = System.IO.Path.Combine(fullPath, "Non-Existing file.txt");
         neDir = System.IO.Path.Combine(UnitTestConstants.SysRoot32, neDir) + Alphaleonis.Win32.Filesystem.Path.DirectorySeparator;
         if (isNetwork)
         {
            fullPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(fullPath);
            neDir = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(neDir);
         }

         suffixedDirectoryName = Alphaleonis.Win32.Filesystem.Path.GetSuffixedDirectoryName(fullPath);

         Console.WriteLine("\nFull Path               : " + fullPath);
         Console.WriteLine("GetSuffixedDirectoryName: " + suffixedDirectoryName);

         Assert.IsTrue(suffixedDirectoryName.Equals(neDir, StringComparison.OrdinalIgnoreCase));




         fullPath = UnitTestConstants.SysRoot;
         if (isNetwork)
            fullPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(fullPath);

         suffixedDirectoryName = Alphaleonis.Win32.Filesystem.Path.GetSuffixedDirectoryName(fullPath);

         Console.WriteLine("\nFull Path               : " + fullPath);
         Console.WriteLine("GetSuffixedDirectoryName: " + suffixedDirectoryName);

         Assert.AreEqual(null, suffixedDirectoryName);




         fullPath = UnitTestConstants.SysDrive + Alphaleonis.Win32.Filesystem.Path.DirectorySeparator;
         if (isNetwork)
            fullPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(fullPath);

         suffixedDirectoryName = Alphaleonis.Win32.Filesystem.Path.GetSuffixedDirectoryNameWithoutRoot(fullPath);

         Console.WriteLine("\nFull Path               : " + fullPath);
         Console.WriteLine("GetSuffixedDirectoryName: " + suffixedDirectoryName);

         Assert.AreEqual(null, suffixedDirectoryName);


         Console.WriteLine();
      }
   }
}
