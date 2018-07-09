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

namespace AlphaFS.UnitTest
{
   public partial class AlphaFS_EncryptionTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void AlphaFS_File_ExportImportEncryptedFileRaw_RountripOfFile_FileContentsOfImportedFileMatchesTheOriginalFile_LocalAndNetwork_Success()
      {
         AlphaFS_File_ExportImportEncryptedFileRaw(false);
         AlphaFS_File_ExportImportEncryptedFileRaw(true);
      }


      private void AlphaFS_File_ExportImportEncryptedFileRaw(bool isNetwork)
      {
         using (var tempRoot = new TemporaryDirectory(isNetwork))
         {
            // Create an encrypted file to use for testing
            var inputFile = System.IO.Path.Combine(tempRoot.Directory.FullName, "test.txt");
            System.IO.File.WriteAllText(inputFile, DateTime.Now.ToString(CultureInfo.CurrentCulture));

            Alphaleonis.Win32.Filesystem.File.Encrypt(inputFile);
            Console.WriteLine("Encrypted Input File: [{0}]", inputFile);


            // Export the file using the method under test.
            var exportedFile = System.IO.Path.Combine(tempRoot.Directory.FullName, "export.dat");

            using (var fs = System.IO.File.Create(exportedFile))
               Alphaleonis.Win32.Filesystem.File.ExportEncryptedFileRaw(inputFile, fs);

            Console.WriteLine("\nExported Input File: [{0}]", exportedFile);


            FileAssert.Exists(exportedFile);
            FileAssert.IsNotEncrypted(exportedFile);
            FileAssert.AreNotEqual(inputFile, exportedFile);


            // Import the file again.
            var importedFile = System.IO.Path.Combine(tempRoot.Directory.FullName, "import.txt");

            using (var fs = System.IO.File.OpenRead(exportedFile))
               Alphaleonis.Win32.Filesystem.File.ImportEncryptedFileRaw(fs, importedFile);

            Console.WriteLine("\nImported Input File: [{0}]", importedFile);


            // Verify that the imported file contents are equal to the original ones.
            FileAssert.Exists(importedFile);
            FileAssert.AreEqual(inputFile, importedFile);
            FileAssert.IsEncrypted(importedFile);
         }

         Console.WriteLine();
      }
   }
}
