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
   public partial class AlphaFS_Shell32Test
   {
      [TestMethod]
      public void AlphaFS_Shell32_GetFileAssociation()
      {
         AlphaFS_Shell32_GetFileAssociation(false);
         AlphaFS_Shell32_GetFileAssociation(true);
      }


      private void AlphaFS_Shell32_GetFileAssociation(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         var inputPath = Environment.SystemDirectory;
         if (isNetwork)
            inputPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(inputPath);

         Console.WriteLine("Input Directory Path: [{0}]", inputPath);


         var cnt = 0;
         foreach (var file in System.IO.Directory.EnumerateFiles(inputPath))
         {
            var shell32Info = Alphaleonis.Win32.Filesystem.Shell32.GetShell32Info(file);

            // Not much of a test...
            Assert.IsNotNull(shell32Info);

            UnitTestConstants.Dump(shell32Info);

            if (++cnt == 5)
               break;
         }

         Console.WriteLine();

         Assert.IsTrue(cnt > 0, "No entries enumerated.");
      }
   }
}
