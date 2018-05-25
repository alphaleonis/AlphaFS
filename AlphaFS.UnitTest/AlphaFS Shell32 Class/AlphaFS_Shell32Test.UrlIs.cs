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
      public void AlphaFS_Shell32_UrlIs()
      {
         Console.WriteLine("Filesystem.Shell32.UrlIs()");

         var urlPath = Alphaleonis.Win32.Filesystem.Shell32.UrlCreateFromPath(UnitTestConstants.AppData);
         var filePath = Alphaleonis.Win32.Filesystem.Shell32.PathCreateFromUrlAlloc(urlPath);

         var isFileUrl1 = Alphaleonis.Win32.Filesystem.Shell32.UrlIsFileUrl(urlPath);
         var isFileUrl2 = Alphaleonis.Win32.Filesystem.Shell32.UrlIsFileUrl(filePath);
         var isNoHistory = Alphaleonis.Win32.Filesystem.Shell32.UrlIs(filePath, Alphaleonis.Win32.Filesystem.Shell32.UrlType.IsNoHistory);
         var isOpaque = Alphaleonis.Win32.Filesystem.Shell32.UrlIs(filePath, Alphaleonis.Win32.Filesystem.Shell32.UrlType.IsOpaque);

         Console.WriteLine("\n\tDirectory: [{0}]", UnitTestConstants.AppData);
         Console.WriteLine("\n\tShell32.UrlCreateFromPath()      == IsFileUrl == [{0}] : {1}\t\t[{2}]", UnitTestConstants.TextTrue, isFileUrl1, urlPath);
         Console.WriteLine("\n\tShell32.PathCreateFromUrlAlloc() == IsFileUrl == [{0}]: {1}\t\t[{2}]", UnitTestConstants.TextFalse, isFileUrl2, filePath);

         Console.WriteLine("\n\tShell32.UrlIsFileUrl()   == [{0}]: {1}\t\t[{2}]", UnitTestConstants.TextTrue, isFileUrl1, urlPath);
         Console.WriteLine("\n\tShell32.UrlIsNoHistory() == [{0}]: {1}\t\t[{2}]", UnitTestConstants.TextTrue, isNoHistory, urlPath);
         Console.WriteLine("\n\tShell32.UrlIsOpaque()    == [{0}]: {1}\t\t[{2}]", UnitTestConstants.TextTrue, isOpaque, urlPath);
         

         Assert.IsTrue(isFileUrl1);

         Assert.IsTrue(isFileUrl2 == false);
      }
   }
}
