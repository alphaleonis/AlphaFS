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
         // Note: System.IO.Path does not have a similar method to compare with.

         AlphaFS_Path_GetSuffixedDirectoryNameWithoutRoot(false);
         AlphaFS_Path_GetSuffixedDirectoryNameWithoutRoot(true);
      }


      private void AlphaFS_Path_GetSuffixedDirectoryNameWithoutRoot(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         var backslash = Alphaleonis.Win32.Filesystem.Path.DirectorySeparator;
         var neDir = "Non-Existing Directory";
         var system32Folder = (Environment.SystemDirectory + backslash).Replace(UnitTestConstants.SysDrive + backslash, string.Empty);


         var fullPath = System.IO.Path.Combine(Environment.SystemDirectory, neDir);
         if (isNetwork)
            fullPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(fullPath);

         var suffixedDirectoryNameWithoutRoot = Alphaleonis.Win32.Filesystem.Path.GetSuffixedDirectoryNameWithoutRoot(fullPath);

         Console.WriteLine("Full Path                          : " + fullPath);
         Console.WriteLine("GetSuffixedDirectoryNameWithoutRoot: " + suffixedDirectoryNameWithoutRoot);

         Assert.AreEqual(system32Folder, suffixedDirectoryNameWithoutRoot);




         fullPath = System.IO.Path.Combine(fullPath, "Non-Existing file.txt");
         neDir = (System.IO.Path.Combine(Environment.SystemDirectory, neDir) + backslash).Replace(UnitTestConstants.SysDrive + backslash, string.Empty);
         if (isNetwork)
            fullPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(fullPath);

         suffixedDirectoryNameWithoutRoot = Alphaleonis.Win32.Filesystem.Path.GetSuffixedDirectoryNameWithoutRoot(fullPath);

         Console.WriteLine("\nFull Path                          : " + fullPath);
         Console.WriteLine("GetSuffixedDirectoryNameWithoutRoot: " + suffixedDirectoryNameWithoutRoot);

         Assert.AreEqual(neDir, suffixedDirectoryNameWithoutRoot);




         fullPath = Environment.SystemDirectory;
         if (isNetwork)
            fullPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(fullPath);

         suffixedDirectoryNameWithoutRoot = Alphaleonis.Win32.Filesystem.Path.GetSuffixedDirectoryNameWithoutRoot(fullPath);

         Console.WriteLine("\nFull Path                          : " + fullPath);
         Console.WriteLine("GetSuffixedDirectoryNameWithoutRoot: " + suffixedDirectoryNameWithoutRoot);


         var windowsFolderName = System.IO.Path.GetDirectoryName(fullPath).Replace(System.IO.Directory.GetDirectoryRoot(fullPath), string.Empty) + backslash;

         if (isNetwork)
            windowsFolderName = windowsFolderName.TrimStart(Alphaleonis.Win32.Filesystem.Path.DirectorySeparatorChar);

         Assert.AreEqual(windowsFolderName, suffixedDirectoryNameWithoutRoot);




         fullPath = UnitTestConstants.SysDrive + backslash;
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
