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
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void AlphaFS_Shell32_GetFileIcon_LocalAndNetwork_Success()
      {
         AlphaFS_Shell32_GetFileIcon(false);
         AlphaFS_Shell32_GetFileIcon(true);
      }




      private void AlphaFS_Shell32_GetFileIcon(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);
         
         var file = System.IO.Path.Combine(Environment.SystemDirectory, "notepad.exe");
         if (isNetwork)
            file = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(System.IO.Path.Combine(Environment.SystemDirectory, "notepad.exe"));

         Console.WriteLine("Input File Path: {0}", file);


         var icon = IntPtr.Zero;
         try
         {
            icon = Alphaleonis.Win32.Filesystem.Shell32.GetFileIcon(file, Alphaleonis.Win32.Filesystem.Shell32.FileAttributes.SmallIcon | Alphaleonis.Win32.Filesystem.Shell32.FileAttributes.AddOverlays);
            Console.WriteLine("\n\tIcon Handle: [{0}]", icon);

            Assert.AreNotEqual(icon, IntPtr.Zero, "The file does not contain an icon, but is expected to.");
         }
         finally
         {
            Alphaleonis.Win32.Filesystem.Shell32.DestroyIcon(icon);
         }

         Console.WriteLine();
      }
   }
}
