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

using Alphaleonis.Win32.Filesystem;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using File = Alphaleonis.Win32.Filesystem.File;
using Path = Alphaleonis.Win32.Filesystem.Path;

namespace AlphaFS.UnitTest
{
   /// <summary>This is a test class for File and is intended to contain all File class Unit Tests.</summary>
   [TestClass]
   public partial class FileTest
   {
      private void DumpReadAllLines(bool isLocal)
      {
         Console.WriteLine("\n=== TEST {0} ===", isLocal ? UnitTestConstants.Local : UnitTestConstants.Network);
         var tmp = Path.Combine(Path.GetTempPath(), "File.SetAttributes()-" + Path.GetRandomFileName());
         var tempPath = isLocal ? tmp : Path.LocalToUnc(tmp);

         // Create file and append text.
         var tempFile = Path.GetTempFileName();

         string[] createText = { "Hello", "And", "Welcome" };
         File.WriteAllLines(tempFile, createText);

         Console.WriteLine("\nFile.ReadAllLines()\n");
         var readText = File.ReadAllLines(tempFile);
         foreach (var s in readText)
         {
            Console.WriteLine("\t{0}", s);
            Assert.IsTrue(createText.Contains(s));
         }

         Console.WriteLine("\nFile.ReadLines()\n");
         foreach (var s in File.ReadLines(tempFile))
         {
            Console.WriteLine("\t{0}", s);
            Assert.IsTrue(createText.Contains(s));
         }

         File.Delete(tempFile, true);
         Assert.IsFalse(File.Exists(tempFile), "Cleanup failed: File should have been removed.");
      }


      [TestMethod]
      public void File_ReadAllLines()
      {
         Console.WriteLine("File.ReadAllLines()");

         DumpReadAllLines(true);
         DumpReadAllLines(false);
      }


      [TestMethod]
      public void File_ReadAllText()
      {
         Console.WriteLine("File.ReadAllText()\n");

         // Create file and append text.
         var tempFile = Path.GetTempFileName();

         string[] createText = { "Hello", "And", "Welcome" };
         File.WriteAllLines(tempFile, createText);

         // Open the file to read from. 
         var textRead = File.ReadAllText(tempFile);
         Console.WriteLine(textRead);

         File.Delete(tempFile, true);
         Assert.IsFalse(File.Exists(tempFile), "Cleanup failed: File should have been removed.");
      }


      [TestMethod]
      public void File_ReadLines()
      {
         Console.WriteLine("File.ReadLines()");

         File_ReadAllLines();
      }


      [TestMethod]
      public void File_WriteAllLines()
      {
         Console.WriteLine("File.WriteAllLines()");
         Console.WriteLine("\n Default AlphaFS Encoding: [{0}]", NativeMethods.DefaultFileEncoding.EncodingName);

         // Create file and append text.
         var tempFile = Path.GetTempFileName();

         var allLines = new[] { UnitTestConstants.TenNumbers, UnitTestConstants.TextHelloWorld, UnitTestConstants.TextGoodbyeWorld, UnitTestConstants.TextUnicode };

         // Create real UTF-8 file.
         File.WriteAllLines(tempFile, allLines, NativeMethods.DefaultFileEncoding);

         // Read filestream contents.
         using (var streamRead = File.OpenText(tempFile))
         {
            var line = streamRead.ReadToEnd();

            Console.WriteLine("\n Created: [{0}] filestream: [{1}]\n\n WriteAllLines content:\n{2}", streamRead.CurrentEncoding.EncodingName, tempFile, line);

            foreach (var line2 in allLines)
               Assert.IsTrue(line.Contains(line2));
         }

         File.Delete(tempFile, true);
         Assert.IsFalse(File.Exists(tempFile), "Cleanup failed: File should have been removed.");
      }


      [TestMethod]
      public void File_WriteAllText()
      {
         Console.WriteLine("File.WriteAllText()");
         Console.WriteLine("\n\tDefault AlphaFS Encoding: [{0}]", NativeMethods.DefaultFileEncoding.EncodingName);

         // Create file and append text.
         var tempFile = Path.GetTempFileName();

         var allLines = UnitTestConstants.TextHelloWorld;

         // Create real UTF-8 file.
         File.WriteAllText(tempFile, allLines, NativeMethods.DefaultFileEncoding);

         // Read filestream contents.
         using (var streamRead = File.OpenText(tempFile))
         {
            var line = streamRead.ReadToEnd();

            Console.WriteLine("\n\tCreated: [{0}] filestream: [{1}]\n\n\tWriteAllText content:\n{2}", streamRead.CurrentEncoding.EncodingName, tempFile, line);

            Assert.IsTrue(line.Contains(allLines));
         }

         // (over)Write.
         File.WriteAllText(tempFile, "Append 1");
         File.WriteAllText(tempFile, allLines);
         File.WriteAllText(tempFile, "Append 2");
         File.WriteAllText(tempFile, allLines);

         // Read filestream contents.
         using (var streamRead = File.OpenText(tempFile))
         {
            var line = streamRead.ReadToEnd();

            Console.WriteLine("\tWriteAllText content:\n{0}", line);

            Assert.IsTrue(line.Contains(allLines));
            Assert.IsTrue(!line.Contains("Append 1"));
            Assert.IsTrue(!line.Contains("Append 2"));
         }

         File.Delete(tempFile, true);
         Assert.IsFalse(File.Exists(tempFile), "Cleanup failed: File should have been removed.");
      }
   }
}
