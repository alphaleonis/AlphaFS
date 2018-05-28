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
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AlphaFS.UnitTest
{
   public partial class PathTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void AlphaFS_Path_GetFinalPathNameByHandle_ToGetFileStreamName_Success()
      {
         // Issue #438, filestream name.
         // AlphaFS implementation of fileStream.Name returns = "[Unknown]"
         // System.IO returns the full path.


         using (var tempRoot = new TemporaryDirectory())
         {
            var file = tempRoot.RandomFileFullPath;

            Console.WriteLine("\nInput File Path: [{0}]\n", file);


            string sysIoStreamName;

            using (var fs = System.IO.File.Create(file))
               sysIoStreamName = fs.Name;
            Assert.AreEqual(sysIoStreamName, file);
            

            using (var fs = Alphaleonis.Win32.Filesystem.File.Create(file))
            {
               var fileStreamName = Alphaleonis.Win32.Filesystem.Path.GetFinalPathNameByHandle(fs.SafeFileHandle);

               fileStreamName = Alphaleonis.Win32.Filesystem.Path.GetRegularPath(fileStreamName);


               Console.WriteLine("\tSystem.IO Filestream Name: " + sysIoStreamName);

               Console.WriteLine("\tAlphaFS   Filestream Name: " + fileStreamName);


               Assert.AreEqual("[Unknown]", fs.Name);

               Assert.AreEqual(sysIoStreamName, fileStreamName);
            }
         }
      }
   }
}
