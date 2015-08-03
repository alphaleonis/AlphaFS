/*  Copyright (C) 2008-2015 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
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
using System.IO;
using System.Text;
using SysIOPath = System.IO.Path;
using SysIOFile = System.IO.File;

namespace AlphaFS.UnitTest
{
   partial class FileTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>

      [TestMethod]
      public void File_Open_Create1Append2And3_LocalUNC_Success()
      {
         File_Open(false);
         File_Open(true);
      }




      private void File_Open(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         string tempPath = SysIOPath.GetTempPath();
         if (isNetwork)
            tempPath = PathUtils.AsUncPath(tempPath);


         using (var rootDir = new TemporaryDirectory(tempPath, "File-Open"))
         {
            string file = rootDir.RandomFileFullPath;
            Console.WriteLine("\nInput File Path: [{0}]\n", file);


            try
            {
               using (FileStream fs = File.Open(file, FileMode.Create))
               {
                  byte[] text = Encoding.UTF8.GetBytes(new string('1', 100));
                  fs.Write(text, 0, text.Length);
               }
               Assert.AreEqual(SysIOFile.ReadAllBytes(file).Length, 100);


               using (FileStream fs = File.Open(file, FileMode.Append))
               {
                  byte[] text = Encoding.UTF8.GetBytes(new string('2', 100));
                  fs.Write(text, 0, text.Length);
               }
               Assert.AreEqual(SysIOFile.ReadAllBytes(file).Length, 200);
               


               using (FileStream fs = File.Open(file, FileMode.Append))
               {
                  byte[] text = Encoding.UTF8.GetBytes(new string('3', 100));
                  fs.Write(text, 0, text.Length);
               }
               Assert.AreEqual(SysIOFile.ReadAllBytes(file).Length, 300);


               // Open the stream and read it back.
               using (FileStream fs = SysIOFile.Open(file, FileMode.Open))
               {
                  byte[] b = new byte[50];
                  UTF8Encoding temp = new UTF8Encoding(true);

                  while (fs.Read(b, 0, b.Length) > 0)
                     Console.WriteLine(temp.GetString(b));

                  Assert.AreEqual(fs.Length, 300);
               }
            }
            catch (Exception ex)
            {
               Console.WriteLine("\nCaught (unexpected) {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
               Assert.IsTrue(false);
            }
         }

         Console.WriteLine();
      }
   }
}
