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
   public partial class Directory_CurrentDirectoryTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void Directory_GetCurrentDirectory_LocalAndNetwork_Success()
      {
         Directory_GetCurrentDirectory(false);
         Directory_GetCurrentDirectory(true);
      }


      private void Directory_GetCurrentDirectory(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         // #1
         var tempPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
         if (isNetwork)
            tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);


         System.IO.Directory.SetCurrentDirectory(tempPath);

         
         var sysIoCurrPath = System.IO.Directory.GetCurrentDirectory();
         var alphaFSCurrPath = Alphaleonis.Win32.Filesystem.Directory.GetCurrentDirectory();

         Console.WriteLine("\tSystem.IO Current Directory Path: [{0}]", sysIoCurrPath);
         Console.WriteLine("\tAlphaFS   Current Directory Path: [{0}]", alphaFSCurrPath);

         Assert.AreEqual(sysIoCurrPath, alphaFSCurrPath, "The current directories do not match, but are expected to.");
         

         // #2
         tempPath = Environment.SystemDirectory;
         if (isNetwork)
            tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);


         System.IO.Directory.SetCurrentDirectory(tempPath);


         sysIoCurrPath = System.IO.Directory.GetCurrentDirectory();
         alphaFSCurrPath = Alphaleonis.Win32.Filesystem.Directory.GetCurrentDirectory();

         Console.WriteLine("\n\tSystem.IO Current Directory Path: [{0}]", sysIoCurrPath);
         Console.WriteLine("\tAlphaFS   Current Directory Path: [{0}]", alphaFSCurrPath);

         Assert.AreEqual(sysIoCurrPath, alphaFSCurrPath, "The current directories do not match, but are expected to.");


         Console.WriteLine();
      }
   }
}
