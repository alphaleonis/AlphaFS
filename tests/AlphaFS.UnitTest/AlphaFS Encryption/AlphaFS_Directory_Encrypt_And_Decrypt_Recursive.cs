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
   public partial class AlphaFS_EncryptionTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void AlphaFS_Directory_EncryptDecrypt_Recursive_LocalAndNetwork_Success()
      {
         AlphaFS_Directory_EncryptDecrypt_Recursive(false);
         AlphaFS_Directory_EncryptDecrypt_Recursive(true);
      }


      private void AlphaFS_Directory_EncryptDecrypt_Recursive(bool isNetwork)
      {
         using (var tempRoot = new TemporaryDirectory(isNetwork))
         {
            var folder = tempRoot.CreateTree();

            Console.WriteLine("Input Directory Path: [{0}]", folder.FullName);
            

            // Encrypt.
            Alphaleonis.Win32.Filesystem.Directory.Encrypt(folder.FullName, true);


            // Verify that the entire folder is encrypted.
            var cnt = 0;
            foreach (var fsei in Alphaleonis.Win32.Filesystem.Directory.EnumerateFileSystemEntryInfos<Alphaleonis.Win32.Filesystem.FileSystemEntryInfo>(folder.FullName, Alphaleonis.Win32.Filesystem.DirectoryEnumerationOptions.Recursive))
            {
               cnt++;
               Assert.IsTrue((fsei.Attributes & System.IO.FileAttributes.Encrypted) != 0, "It is expected that the file system object is encrypted, but it is not.");
            }

            if (cnt == 0)
               UnitTestAssert.InconclusiveBecauseResourcesAreUnavailable();




            // Decrypt.
            Alphaleonis.Win32.Filesystem.Directory.Decrypt(folder.FullName, true);


            // Verify that the entire folder is decrypted.
            cnt = 0;
            foreach (var fsei in Alphaleonis.Win32.Filesystem.Directory.EnumerateFileSystemEntryInfos<Alphaleonis.Win32.Filesystem.FileSystemEntryInfo>(folder.FullName, Alphaleonis.Win32.Filesystem.DirectoryEnumerationOptions.Recursive))
            {
               cnt++;
               Assert.IsTrue((fsei.Attributes & System.IO.FileAttributes.Encrypted) == 0, "It is expected that the file system object is decrypted, but it is not.");
            }

            if (cnt == 0)
               UnitTestAssert.InconclusiveBecauseResourcesAreUnavailable();
         }

         Console.WriteLine();
      }
   }
}
