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
using System.Globalization;
using System.Reflection;

namespace AlphaFS.UnitTest
{
   public partial class AlphaFS_BackupFileStreamTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void AlphaFS_BackupFileStream_InitializeInstance_LocalAndNetwork_Success()
      {
         AlphaFS_BackupFileStream_InitializeInstance(false);
         AlphaFS_BackupFileStream_InitializeInstance(true);
      }
      

      private void AlphaFS_BackupFileStream_InitializeInstance(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         using (var tempRoot = new TemporaryDirectory(isNetwork ? Alphaleonis.Win32.Filesystem.Path.LocalToUnc(UnitTestConstants.TempFolder) : UnitTestConstants.TempFolder, MethodBase.GetCurrentMethod().Name))
         {
            var file = tempRoot.RandomFileFullPath;

            Console.WriteLine("\nInput File Path: [{0}]", file);

            System.IO.File.WriteAllText(file, DateTime.Now.ToString(CultureInfo.InvariantCulture));


            using (var bfs = new Alphaleonis.Win32.Filesystem.BackupFileStream(file, System.IO.FileMode.Open))
            {
               UnitTestConstants.Dump(bfs.ReadStreamInfo(), -10);

               UnitTestConstants.Dump(bfs, -11);
            }
         }

         Console.WriteLine();
      }
   }
}
