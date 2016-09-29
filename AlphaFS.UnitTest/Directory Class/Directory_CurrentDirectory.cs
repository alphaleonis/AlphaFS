/*  Copyright (C) 2008-2016 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
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
   partial class DirectoryTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>

      [TestMethod]
      public void Directory_GetCurrentDirectory_LocalAndNetwork_Success()
      {
         Directory_GetCurrentDirectory(false);
         Directory_GetCurrentDirectory(true);
      }


      [TestMethod]
      public void Directory_SetCurrentDirectory_LocalAndNetwork_Success()
      {
         Directory_SetCurrentDirectory(false);
         Directory_SetCurrentDirectory(true);
      }


      [TestMethod]
      public void Directory_SetCurrentDirectory_WithLongPath_LocalAndNetwork_Success()
      {
         Directory_SetCurrentDirectory_WithLongPath(false);
         Directory_SetCurrentDirectory_WithLongPath(true);
      }




      private void Directory_GetCurrentDirectory(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         // #1
         var tempPath = UnitTestConstants.AppData;
         if (isNetwork)
            tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);


         System.IO.Directory.SetCurrentDirectory(tempPath);

         
         var sysIoCurrPath = System.IO.Directory.GetCurrentDirectory();
         var alphaFSCurrPath = Alphaleonis.Win32.Filesystem.Directory.GetCurrentDirectory();

         Console.WriteLine("\n\tSystem.IO Current Directory Path: [{0}]", sysIoCurrPath);
         Console.WriteLine("\tAlphaFS   Current Directory Path: [{0}]", alphaFSCurrPath);

         Assert.AreEqual(sysIoCurrPath, alphaFSCurrPath, "The current directories do not match, but are expected to.");
         

         // #2
         tempPath = UnitTestConstants.SysRoot;
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


      private void Directory_SetCurrentDirectory(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         var tempPath = UnitTestConstants.SysRoot;
         if (isNetwork)
            tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);

         
         Console.WriteLine("\n\tAlphaFS Set Current Directory Path: [{0}]\n", tempPath);

         Alphaleonis.Win32.Filesystem.Directory.SetCurrentDirectory(tempPath, Alphaleonis.Win32.Filesystem.PathFormat.FullPath);


         var sysIoCurrPath = System.IO.Directory.GetCurrentDirectory();
         var alphaFSCurrPath = Alphaleonis.Win32.Filesystem.Directory.GetCurrentDirectory();

         Console.WriteLine("\tSystem.IO Get Current Directory Path: [{0}]", sysIoCurrPath);
         Console.WriteLine("\tAlphaFS   Get Current Directory Path: [{0}]", alphaFSCurrPath);

         Assert.AreEqual(sysIoCurrPath, alphaFSCurrPath, "The current directories do not match, but are expected to.");


         Console.WriteLine();
      }


      private void Directory_SetCurrentDirectory_WithLongPath(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         var tempPath = UnitTestConstants.SysRoot;
         if (isNetwork)
            tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);


         Console.WriteLine("\n\tNo compare with System.IO possible because of: \"System.ArgumentException: Illegal characters in path.\"\n");


         Console.WriteLine("\tAlphaFS Set Current Directory Path: [{0}]", tempPath);
         Alphaleonis.Win32.Filesystem.Directory.SetCurrentDirectory(tempPath);


         // No compare with System.IO possible: System.ArgumentException: Illegal characters in path.
         //var sysIoCurrPath = System.IO.Directory.GetCurrentDirectory();

         var alphaFSCurrPath = Alphaleonis.Win32.Filesystem.Directory.GetCurrentDirectory();


         Console.WriteLine("\tAlphaFS Get Current Directory Path: [{0}]", alphaFSCurrPath);


         var lpPrefix = isNetwork
            ? Alphaleonis.Win32.Filesystem.Path.LongPathUncPrefix
            : Alphaleonis.Win32.Filesystem.Path.LongPathPrefix;

         if (isNetwork)
            tempPath = tempPath.TrimStart('\\');

         Assert.AreEqual(lpPrefix + tempPath, alphaFSCurrPath, "The current directories do not match, but are expected to.");


         Console.WriteLine();
      }
   }
}
