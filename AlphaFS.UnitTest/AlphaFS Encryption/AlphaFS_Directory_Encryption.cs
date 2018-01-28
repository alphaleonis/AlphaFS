/*  Copyright (C) 2008-2017 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
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
using System.Reflection;

namespace AlphaFS.UnitTest
{
   public partial class AlphaFS_EncryptionTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void AlphaFS_Directory_Encrypt_And_Decrypt_LocalAndNetwork_Success()
      {
         Directory_Encrypt_And_Decrypt(false);
         Directory_Encrypt_And_Decrypt(true);
      }


      [TestMethod]
      public void AlphaFS_Directory_Encrypt_And_Decrypt_Recursive_LocalAndNetwork_Success()
      {
         Directory_Encrypt_And_Decrypt_Recursive(false);
         Directory_Encrypt_And_Decrypt_Recursive(true);
      }




      private void Directory_Encrypt_And_Decrypt(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         var tempPath = System.IO.Path.GetTempPath();
         if (isNetwork)
            tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);


         using (var rootDir = new TemporaryDirectory(tempPath, MethodBase.GetCurrentMethod().Name))
         {
            var folder = rootDir.RandomDirectoryFullPath;
            Console.WriteLine("\nInput Directory Path: [{0}]\n", folder);

            const int expectedFso = 10;

            UnitTestConstants.CreateDirectoriesAndFiles(folder, expectedFso, false, false, false);




            // Encrypt root folder only.
            Alphaleonis.Win32.Filesystem.Directory.Encrypt(folder);


            // Verify that the contents of the folder are still decrypted.
            var cnt = 0;
            foreach (var fsei in Alphaleonis.Win32.Filesystem.Directory.EnumerateFileSystemEntryInfos<Alphaleonis.Win32.Filesystem.FileSystemEntryInfo>(folder, Alphaleonis.Win32.Filesystem.Path.WildcardStarMatchAll, Alphaleonis.Win32.Filesystem.DirectoryEnumerationOptions.Recursive))
            {
               cnt++;
               Assert.IsTrue((fsei.Attributes & System.IO.FileAttributes.Encrypted) == 0, "It is expected that the file system object is decrypted, but it is not.");
            }

            if (cnt == 0)
               Assert.Inconclusive("Test encrypt: Nothing is enumerated, but it is expected.");




            // Encrypt entire folder for decrypt test.
            Alphaleonis.Win32.Filesystem.Directory.Encrypt(folder, true);

            // Decrypt root folder only.
            Alphaleonis.Win32.Filesystem.Directory.Decrypt(folder);


            // Verify that the contents of the folder are still encrypted.
            cnt = 0;
            foreach (var fsei in Alphaleonis.Win32.Filesystem.Directory.EnumerateFileSystemEntryInfos<Alphaleonis.Win32.Filesystem.FileSystemEntryInfo>(folder, Alphaleonis.Win32.Filesystem.Path.WildcardStarMatchAll, Alphaleonis.Win32.Filesystem.DirectoryEnumerationOptions.Recursive))
            {
               cnt++;
               Assert.IsTrue((fsei.Attributes & System.IO.FileAttributes.Encrypted) != 0, "It is expected that the file system object is encrypted, but it is not.");
            }

            if (cnt == 0)
               Assert.Inconclusive("Test decrypt: Nothing is enumerated, but it is expected.");
         }

         Console.WriteLine();
      }


      private void Directory_Encrypt_And_Decrypt_Recursive(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         var tempPath = System.IO.Path.GetTempPath();
         if (isNetwork)
            tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);


         using (var rootDir = new TemporaryDirectory(tempPath, MethodBase.GetCurrentMethod().Name))
         {
            var folder = rootDir.RandomDirectoryFullPath;
            Console.WriteLine("\nInput Directory Path: [{0}]\n", folder);

            const int expectedFso = 10;
            UnitTestConstants.CreateDirectoriesAndFiles(folder, expectedFso, false, false, false);




            // Encrypt.
            Alphaleonis.Win32.Filesystem.Directory.Encrypt(folder, true);


            // Verify that the entire folder is encrypted.
            var cnt = 0;
            foreach (var fsei in Alphaleonis.Win32.Filesystem.Directory.EnumerateFileSystemEntryInfos<Alphaleonis.Win32.Filesystem.FileSystemEntryInfo>(folder, Alphaleonis.Win32.Filesystem.Path.WildcardStarMatchAll, Alphaleonis.Win32.Filesystem.DirectoryEnumerationOptions.Recursive))
            {
               cnt++;
               Assert.IsTrue((fsei.Attributes & System.IO.FileAttributes.Encrypted) != 0, "It is expected that the file system object is encrypted, but it is not.");
            }

            if (cnt == 0)
               Assert.Inconclusive("Test encrypt: Nothing is enumerated, but it is expected.");




            // Decrypt.
            Alphaleonis.Win32.Filesystem.Directory.Decrypt(folder, true);


            // Verify that the entire folder is decrypted.
            cnt = 0;
            foreach (var fsei in Alphaleonis.Win32.Filesystem.Directory.EnumerateFileSystemEntryInfos<Alphaleonis.Win32.Filesystem.FileSystemEntryInfo>(folder, Alphaleonis.Win32.Filesystem.Path.WildcardStarMatchAll, Alphaleonis.Win32.Filesystem.DirectoryEnumerationOptions.Recursive))
            {
               cnt++;
               Assert.IsTrue((fsei.Attributes & System.IO.FileAttributes.Encrypted) == 0, "It is expected that the file system object is decrypted, but it is not.");
            }

            if (cnt == 0)
               Assert.Inconclusive("Test decrypt: Nothing is enumerated, but it is expected.");
         }

         Console.WriteLine();
      }
   }
}
