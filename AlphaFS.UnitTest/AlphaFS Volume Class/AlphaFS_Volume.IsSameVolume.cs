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
   public partial class AlphaFS_VolumeTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void AlphaFS_Volume_IsSameVolume()
      {
         UnitTestConstants.PrintUnitTestHeader(false);
         
         var file1 = System.IO.Path.GetTempFileName();
         var file2 = System.IO.Path.GetTempFileName();
         var fileTmp = file2;


         // Same C:
         var isSame = Alphaleonis.Win32.Filesystem.Volume.IsSameVolume(file1, fileTmp);

         Console.WriteLine("On same Volume (Should be True): [{0}]\n\tFile1: [{1}]\n\tFile2: [{2}]", isSame, file1, fileTmp);

         Assert.IsTrue(isSame, "Should be the same volume.");


         // Same C: -> C$
         fileTmp = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(file2);
         
         isSame = Alphaleonis.Win32.Filesystem.Volume.IsSameVolume(file1, fileTmp);

         Console.WriteLine("\nOn same Volume (Should be True): [{0}]\n\tFile1: [{1}]\n\tFile2: [{2}]", isSame, file1, fileTmp);

         Assert.IsTrue(isSame, "Should be the same volume.");
      }
   }
}
