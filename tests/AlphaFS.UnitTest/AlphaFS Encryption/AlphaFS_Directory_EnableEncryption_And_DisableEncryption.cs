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
      public void AlphaFS_Directory_EnableEncryption_And_DisableEncryption_LocalAndNetwork_Success()
      {
         AlphaFS_Directory_EnableEncryption_And_DisableEncryption(false);
         AlphaFS_Directory_EnableEncryption_And_DisableEncryption(true);

      }


      private void AlphaFS_Directory_EnableEncryption_And_DisableEncryption(bool isNetwork)
      {
         using (var tempRoot = new TemporaryDirectory(isNetwork))
         {
            var folder = tempRoot.CreateDirectory();

            Console.WriteLine("Input Directory Path: [{0}]\n", folder.FullName);


            var deskTopIni = System.IO.Path.Combine(folder.FullName, "Desktop.ini");
            var enabled = new[] {"[Encryption]", "Disable=0"};
            var disabled = new[] {"[Encryption]", "Disable=1"};


            // Enable.
            Alphaleonis.Win32.Filesystem.Directory.EnableEncryption(folder.FullName);
            

            // Read Desktop.ini file.
            var collection = System.IO.File.ReadAllLines(deskTopIni);

            for (int i = 0, l = collection.Length; i < l; i++)
               Console.WriteLine("\t" + collection[i]);
            Console.WriteLine();

            CollectionAssert.AreEqual(enabled, collection);




            // Disable.
            Alphaleonis.Win32.Filesystem.Directory.DisableEncryption(folder.FullName);


            // Read Desktop.ini file.
            collection = System.IO.File.ReadAllLines(deskTopIni);

            for (int i = 0, l = collection.Length; i < l; i++)
               Console.WriteLine("\t" + collection[i]);

            CollectionAssert.AreEqual(disabled, collection);
         }

         Console.WriteLine();
      }
   }
}
