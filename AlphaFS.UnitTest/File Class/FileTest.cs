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
using System.IO;
using System.Linq;
using System.Text;
using Directory = Alphaleonis.Win32.Filesystem.Directory;
using File = Alphaleonis.Win32.Filesystem.File;
using Path = Alphaleonis.Win32.Filesystem.Path;

namespace AlphaFS.UnitTest
{
   /// <summary>This is a test class for File and is intended to contain all File class Unit Tests.</summary>
   [TestClass]
   public partial class FileTest
   {
      #region Unit Tests

      private void DumpGetSetAttributes(bool isLocal)
      {
         Console.WriteLine("\n=== TEST {0} ===", isLocal ? UnitTestConstants.Local : UnitTestConstants.Network);
         var tmp = Path.Combine(Path.GetTempPath(), "File.SetAttributes()-" + Path.GetRandomFileName());
         var tempPath = isLocal ? tmp : Path.LocalToUnc(tmp);
         var sys32 = isLocal ? UnitTestConstants.SysRoot32 : Path.LocalToUnc(UnitTestConstants.SysRoot32);

         Console.WriteLine("\nInput Path: [{0}]", sys32);

         // Just enumerate and compare attributes in folder: C:\Windows\System32
         foreach (var file in Directory.EnumerateFiles(sys32))
         {
            var actual = File.GetAttributes(file);
            var expected = System.IO.File.GetAttributes(file);

            Assert.AreEqual(expected, actual, "AlphaFS != System.IO");
         }


         Console.WriteLine("\nInput Path: [{0}]", tempPath);

         // Create some folders and files.
         UnitTestConstants.CreateDirectoriesAndFiles(tempPath, 1, false, false, true);

         var apply = FileAttributes.Hidden | FileAttributes.Archive | FileAttributes.System | FileAttributes.ReadOnly;
         Console.WriteLine("\nSetAttributes(): [{0}]", apply);

         var allOk = true;
         var cnt = 0;

         foreach (var file in Directory.EnumerateFiles(tempPath))
         {
            try
            {
               File.SetAttributes(file, apply);

               var actual = File.GetAttributes(file);
               var expected = System.IO.File.GetAttributes(file);

               Console.WriteLine("\n\t#{0:000}\tFile     : [{1}]\n\t\tAlphaFS  : [{2}]\n\t\tSystem.IO: [{3}]", ++cnt, file, expected, actual);

               if (cnt == 0)
                  Assert.Inconclusive("Nothing is enumerated, but it is expected.");

               Assert.AreEqual(expected, actual, "AlphaFS != System.IO");
            }
            catch (Exception ex)
            {
               allOk = false;
               Console.WriteLine("\n\tCaught (UNEXPECTED) {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
            }
         }
         Console.WriteLine();

         Assert.IsTrue(allOk);


         apply = FileAttributes.Normal;
         Console.WriteLine("\nSetAttributes(): [{0}]", apply);

         allOk = true;
         cnt = 0;

         foreach (var file in Directory.EnumerateFiles(tempPath))
         {
            try
            {
               File.SetAttributes(file, apply);

               var actual = File.GetAttributes(file);
               var expected = System.IO.File.GetAttributes(file);

               Console.WriteLine("\n\t#{0:000}\tFile     : [{1}]\n\t\tAlphaFS  : [{2}]\n\t\tSystem.IO: [{3}]", ++cnt, file, expected, actual);

               if (cnt == 0)
                  Assert.Inconclusive("Nothing is enumerated, but it is expected.");

               Assert.AreEqual(expected, actual, "AlphaFS != System.IO");
            }
            catch (Exception ex)
            {
               allOk = false;
               Console.WriteLine("\n\tCaught (UNEXPECTED) {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
            }
         }
         Console.WriteLine();


         Directory.Delete(tempPath, true, true);
         Assert.IsFalse(Directory.Exists(tempPath), "Cleanup failed: Directory should have been removed.");
         Assert.IsTrue(allOk);
         Console.WriteLine();
      }


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


      private void DumpReadWriteAllBytes(bool isLocal)
      {
         Console.WriteLine("\n=== TEST {0} ===", isLocal ? UnitTestConstants.Local : UnitTestConstants.Network);
         var tempPath = Path.GetTempPath("File.ReadWriteAllBytes()-" + Path.GetRandomFileName());
         if (!isLocal) { tempPath = Path.LocalToUnc(tempPath); }

         var size = 10000;
         var text = Encoding.UTF8.GetBytes(new string('X', size));
         var allOk = true;

         try
         {
            File.WriteAllBytes(tempPath, text);
            Console.WriteLine("\nWriteAllBytes(): [{0}] bytes: [{1}]", size, tempPath);
         }
         catch (Exception ex)
         {
            allOk = false;
            Console.WriteLine("\n\tCaught (UNEXPECTED) {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
         }
         Assert.IsTrue(File.Exists(tempPath), "File.WriteAllBytes(): File was not created.");
         var fileSize = File.GetSize(tempPath);
         Assert.AreEqual(size, fileSize);
         Assert.IsTrue(allOk);



         byte[] readAllAlphaFS = { };
         byte[] readAllSysIo = { };

         try
         {
            readAllAlphaFS = File.ReadAllBytes(tempPath);
            readAllSysIo = System.IO.File.ReadAllBytes(tempPath);
            Console.WriteLine("\nReadAllBytes(): [{0}] bytes.", readAllAlphaFS.Length);
         }
         catch (Exception ex)
         {
            allOk = false;
            Console.WriteLine("\n\tCaught (UNEXPECTED) {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
         }

         Assert.AreEqual(readAllAlphaFS.Length, fileSize, "File.ReadAllBytes(): Number of bytes are different.");
         Assert.AreEqual(readAllAlphaFS.Length, readAllSysIo.Length, "File.ReadAllBytes(): AlphaFS != System.IO");



         File.Delete(tempPath, true);
         Assert.IsFalse(File.Exists(tempPath), "Cleanup failed: File should have been removed.");
         Assert.IsTrue(allOk);
         Console.WriteLine("\n");
      }

      #endregion // Unit Tests


      #region .NET

      #region Encrypt

      [TestMethod]
      public void File_Encrypt()
      {
         Console.WriteLine("File.Encrypt()");

         // Create file and append text.
         var tempFile = Path.GetTempFileName();

         // Append text as UTF-8, default.
         File.AppendAllText(tempFile, UnitTestConstants.TextHelloWorld);

         var utf8 = NativeMethods.DefaultFileEncoding.BodyName.ToUpperInvariant();
         var readText8 = File.ReadAllText(tempFile);
         var actual = File.GetAttributes(tempFile);
         var encryptionStatus = File.GetEncryptionStatus(tempFile);
         Console.WriteLine("\n\tCreated {0} file: [{1}]", utf8, tempFile);
         Console.WriteLine("\tContent: [{0}]", readText8);
         Console.WriteLine("\n\tFile.GetAttributes(): [{0}]", actual);
         Console.WriteLine("\tEncryption status   : [{0}]", encryptionStatus);

         var encryptOk = false;
         try
         {
            File.Encrypt(tempFile);
            encryptOk = true;
            actual = File.GetAttributes(tempFile);
         }
         catch (Exception ex)
         {
            Console.WriteLine("\n\tCaught (UNEXPECTED) {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
         }
         encryptionStatus = File.GetEncryptionStatus(tempFile);
         Console.WriteLine("\n\tFile.Encrypt() (Should be True): [{0}]", encryptOk);
         Console.WriteLine("\tFile.GetAttributes()           : [{0}]", actual);
         Console.WriteLine("\tEncryption status              : [{0}]", encryptionStatus);

         var decryptOk = false;
         try
         {
            File.Decrypt(tempFile);
            decryptOk = true;
            actual = File.GetAttributes(tempFile);
         }
         catch (Exception ex)
         {
            Console.WriteLine("\n\tCaught (UNEXPECTED) {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
         }
         var decryptionStatus = File.GetEncryptionStatus(tempFile);
         Console.WriteLine("\n\tFile.Decrypt() (Should be True): [{0}]:", decryptOk);
         Console.WriteLine("\tFile.GetAttributes()           : [{0}]", actual);
         Console.WriteLine("\tDecryption status              : [{0}]", decryptionStatus);

         Assert.IsTrue(encryptOk, "File should be encrypted.");
         Assert.IsTrue(encryptionStatus == FileEncryptionStatus.Encrypted, "File should be encrypted.");
         Assert.IsTrue(decryptOk, "File should be decrypted.");
         Assert.IsTrue(decryptionStatus == FileEncryptionStatus.Encryptable, "File should be decrypted.");

         File.Delete(tempFile, true);
         Assert.IsFalse(File.Exists(tempFile), "Cleanup failed: File should have been removed.");
      }

      #endregion // Encrypt


      #region GetAttributes

      [TestMethod]
      public void File_GetAttributes()
      {
         Console.WriteLine("File.GetAttributes()");

         DumpGetSetAttributes(true);
         DumpGetSetAttributes(false);
      }

      #endregion // GetAttributes


      #region ReadAllBytes

      [TestMethod]
      public void File_ReadAllBytes()
      {
         Console.WriteLine("File.ReadAllBytes()");

         DumpReadWriteAllBytes(true);
         DumpReadWriteAllBytes(false);
      }

      #endregion // WriteAllBytes


      #region ReadAllLines

      [TestMethod]
      public void File_ReadAllLines()
      {
         Console.WriteLine("File.ReadAllLines()");

         DumpReadAllLines(true);
         DumpReadAllLines(false);
      }

      #endregion // ReadAllLines


      #region ReadAllText

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

      #endregion // ReadAllText


      #region ReadLines

      [TestMethod]
      public void File_ReadLines()
      {
         Console.WriteLine("File.ReadLines()");

         File_ReadAllLines();
      }

      #endregion // ReadLines


      #region WriteAllBytes

      [TestMethod]
      public void File_WriteAllBytes()
      {
         Console.WriteLine("File.WriteAllBytes()");

         File_ReadAllBytes();
      }

      #endregion // WriteAllBytes


      #region WriteAllLines

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

      #endregion // WriteAllLines


      #region WriteAllText

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

      #endregion // WriteAllText

      #endregion // .NET
   }
}
