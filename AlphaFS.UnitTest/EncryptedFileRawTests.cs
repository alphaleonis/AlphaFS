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

using Alphaleonis.Win32.Filesystem;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FileStream = System.IO.FileStream;

namespace AlphaFS.UnitTest
{
   [TestClass]
   public class EncryptedFileRawTests
   {
      [TestMethod]
      public void File_ExportEncryptedFileRaw_ExportImportRountripOfFile_FileContentsOfImportedFileMatchesTheOriginalFile()
      {
         using (TemporaryDirectory rootDir = new TemporaryDirectory())
         {
            // Create an encrypted file to use for testing
            string inputFile = System.IO.Path.Combine(rootDir.Directory.FullName, "test.txt");
            System.IO.File.WriteAllText(inputFile, "Test file #1");
            File.Encrypt(inputFile);

            // Export the file using the method under test.
            string exportedFile = System.IO.Path.Combine(rootDir.Directory.FullName, "export.dat");
            using (FileStream fs = System.IO.File.Create(exportedFile))
            {
               File.ExportEncryptedFileRaw(inputFile, fs);
            }
            
            FileAssert.Exists(exportedFile);
            FileAssert.IsNotEncrypted(exportedFile);
            FileAssert.AreNotEqual(inputFile, exportedFile);

            // Import the file again
            string importedFile = System.IO.Path.Combine(rootDir.Directory.FullName, "import.txt");
            using (FileStream fs = System.IO.File.OpenRead(exportedFile))
            {
               File.ImportEncryptedFileRaw(fs, importedFile);               
            }

            // Verify that the imported file contents are equal to the original ones.
            FileAssert.Exists(importedFile);
            FileAssert.AreEqual(inputFile, importedFile);
            FileAssert.IsEncrypted(importedFile);
         }
      }
   }
}
