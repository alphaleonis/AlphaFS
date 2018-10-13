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

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AlphaFS.UnitTest
{
   public partial class PathTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void AlphaFS_Path_GetRelativePathResolveRelativePath_LocalAndNetwork_Success()
      {
         AlphaFS_Path_GetRelativePathResolveRelativePath(false);
         AlphaFS_Path_GetRelativePathResolveRelativePath(true);
      }


      private void AlphaFS_Path_GetRelativePathResolveRelativePath(bool isNetwork)
      {
         using (var tempRoot = new TemporaryDirectory(isNetwork))
         {
            var sysDrive = isNetwork ? Alphaleonis.Win32.Filesystem.Path.LocalToUnc(UnitTestConstants.SysDrive) : UnitTestConstants.SysDrive;
            
            string[] relativePaths =
            {
               // fso = Folder or file.

               @"a\b\c",
               @"a\fso",
               @"..\..\fso",
               @"..\b\c",

               @"a\b\c",
               @"b\fso",
               @"b\fso",
               @"a\b\c",

               @"a\b\c",
               @"fso",
               @"fso",
               @"a\b\c",

               @"\a\b\",
               @"\",
               @"..\..\",
               @"a\b\",


               sysDrive + @"\a",
               sysDrive + @"\fso",
               @"..\fso",
               @"..\a",

               sysDrive + @"\a",
               sysDrive + @"\a\fso",
               @"fso",
               @"..\a",

               sysDrive + @"\a",
               sysDrive + @"\a\b\fso",
               @"b\fso",
               @"..\..\a",

               sysDrive + @"\a",
               sysDrive + @"\a\b\c\fso",
               @"b\c\fso",
               @"..\..\..\a",

               sysDrive + @"\a",
               sysDrive + @"\b\fso",
               @"..\b\fso",
               @"..\..\a",

               sysDrive + @"\a",
               sysDrive + @"\b\c\fso",
               @"..\b\c\fso",
               @"..\..\..\a",


               sysDrive + @"\a\b",
               sysDrive + @"\a\fso",
               @"..\fso",
               @"..\b",
            
               sysDrive + @"\a\b",
               sysDrive + @"\a",
               @"..\a",
               @"b",
            
               sysDrive + @"\a\b",
               sysDrive + @"\x\y\fso",
               @"..\..\x\y\fso",
               @"..\..\..\a\b",

               sysDrive + @"\a\b\",
               sysDrive + @"\",
               @"..\..\",
               @"a\b\",


               sysDrive + @"\a\b\c",
               sysDrive + @"\a\fso",
               @"..\..\fso",
               @"..\b\c",
            
               sysDrive + @"\a\b\c",
               sysDrive + @"\a\x\fso",
               @"..\..\x\fso",
               @"..\..\b\c",
            
               sysDrive + @"\a\b\c",
               sysDrive + @"\a\x\y\fso",
               @"..\..\x\y\fso",
               @"..\..\..\b\c",
            
               sysDrive + @"\a\b\c",
               sysDrive + @"\fso",
               @"..\..\..\fso",
               @"..\a\b\c",


               tempRoot.Directory.FullName + @"\",
               tempRoot.Directory.FullName + @"\",
               string.Empty,
               string.Empty,


               tempRoot.Directory.FullName,
               tempRoot.Directory.FullName,
               tempRoot.Directory.Name,
               tempRoot.Directory.Name
            };


            var totalPaths = relativePaths.Length;
         
            for (var i = 0; i < totalPaths; i += 4)
            {
               var current = relativePaths[0 + i];
               var selected = relativePaths[1 + i];
               var shouldBeCurrent = relativePaths[2 + i];
               var shouldBeSelected = relativePaths[3 + i];

               var relative = Alphaleonis.Win32.Filesystem.Path.GetRelativePath(current, selected);
               var absolute = Alphaleonis.Win32.Filesystem.Path.ResolveRelativePath(current, selected);

               Console.WriteLine("\tCurrent : [{0}]", current);
               Console.WriteLine("\tSelected: [{0}]", selected);
               Console.WriteLine("\tRelative: [{0}]", relative);


               var verify = System.IO.Path.IsPathRooted(current) && current[0] != Alphaleonis.Win32.Filesystem.Path.DirectorySeparatorChar;

               if (verify)
               {
                  Console.WriteLine("\tAbsolute: [{0}]", absolute);

                  Assert.AreEqual(selected, absolute, "The absolute paths do not match, but are expected to.");
               }

               Console.WriteLine();

               Assert.AreEqual(shouldBeCurrent, relative, "The relative paths do not match, but are expected to.");


               relative = Alphaleonis.Win32.Filesystem.Path.GetRelativePath(selected, current);
               absolute = Alphaleonis.Win32.Filesystem.Path.ResolveRelativePath(selected, current);

               Console.WriteLine("\t  Current : [{0}]", selected);
               Console.WriteLine("\t  Selected: [{0}]", current);
               Console.WriteLine("\t  Relative: [{0}]", relative);


               verify = System.IO.Path.IsPathRooted(selected) && selected[0] != Alphaleonis.Win32.Filesystem.Path.DirectorySeparatorChar;

               if (verify)
               {
                  Console.WriteLine("\t  Absolute: [{0}]", absolute);

                  Assert.AreEqual(current, absolute, "The absolute paths do not match, but are expected to.");
               }


               Assert.AreEqual(shouldBeSelected, relative, "The relative paths do not match, but are expected to.");


               Console.WriteLine();
            }
         }
      }
   }
}
