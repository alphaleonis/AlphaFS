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
      [TestMethod]
      public void AlphaFS_File_EncryptDecrypt_GetEncryptionStatus_LocalAndNetwork_Success()
      {
         AlphaFS_File_EncryptDecrypt_GetEncryptionStatus(false);
         AlphaFS_File_EncryptDecrypt_GetEncryptionStatus(true);

      }


      private void AlphaFS_File_EncryptDecrypt_GetEncryptionStatus(bool isNetwork)
      {
         using (var tempRoot = new TemporaryDirectory(isNetwork))
         {
            var file = tempRoot.CreateFile();

            Console.WriteLine("Input File Path: [{0}]", file.FullName);

            
            // Encrypt file.
            Alphaleonis.Win32.Filesystem.File.Encrypt(file.FullName);
            
            FileAssert.IsEncrypted(file.FullName);

            Assert.AreEqual(Alphaleonis.Win32.Filesystem.FileEncryptionStatus.Encrypted, Alphaleonis.Win32.Filesystem.File.GetEncryptionStatus(file.FullName), "The file is not encrypted, but it is expected.");

            
            // Decrypt file.
            Alphaleonis.Win32.Filesystem.File.Decrypt(file.FullName);

            FileAssert.IsNotEncrypted(file.FullName);

            Assert.AreNotEqual(Alphaleonis.Win32.Filesystem.FileEncryptionStatus.Encrypted, Alphaleonis.Win32.Filesystem.File.GetEncryptionStatus(file.FullName), "The file is encrypted, but it is not expected.");

         }

         Console.WriteLine();
      }
   }
}
