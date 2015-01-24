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
using Stream = System.IO.Stream;

namespace AlphaFS.UnitTest
{
   public static class FileAssert
   {
      public static void Exists(string filePath)
      {
         if (!File.Exists(filePath))
            throw new AssertFailedException(String.Format("The file \"{0}\" does not exist, but was expected to.", filePath));
      }

      public static void AreEqual(string expectedFilePath, string actualFilePath)
      {
         int position = 0;
         using (System.IO.FileStream s1 = File.OpenRead(expectedFilePath))
         using (System.IO.FileStream s2 = File.OpenRead(actualFilePath))
         {
            if (s1.Length != s2.Length)
            {
               throw new AssertFailedException(String.Format("The files \"{0}\" and \"{1}\" are not equal but was expected to be. Their size differs.", expectedFilePath, actualFilePath));
            }
            int a = s1.ReadByte();
            int b = s2.ReadByte();
            if (a != b)
                  {
                  throw new AssertFailedException(String.Format("The files \"{0}\" and \"{1}\" are not equal but was expected to be. The first difference was at position {2}",
                     expectedFilePath, actualFilePath, position));
                  }

            position++;
         }
      }

      public static void AreNotEqual(string expectedFilePath, string actualFilePath)
      {
         int position = 0;
         using (System.IO.FileStream s1 = File.OpenRead(expectedFilePath))
         using (System.IO.FileStream s2 = File.OpenRead(actualFilePath))
         {
            if (s1.Length != s2.Length)
            {
               return;
            }

            int a = s1.ReadByte();
            int b = s2.ReadByte();
            if (a != b)
            {
               return;
            }

            position++;
         }

         throw new AssertFailedException(String.Format("The files \"{0}\" and \"{1}\" are equal but was not expected to be.", expectedFilePath, actualFilePath));
      }

      public static void IsEncrypted(string filePath)
      {
         if ((File.GetAttributes(filePath) & System.IO.FileAttributes.Encrypted) == 0)
            throw new AssertFailedException(String.Format("The file \"{0}\" was not encrypted, but was expected to be.", filePath));
      }

      public static void IsNotEncrypted(string filePath)
      {
         if ((File.GetAttributes(filePath) & System.IO.FileAttributes.Encrypted) != 0)
            throw new AssertFailedException(String.Format("The file \"{0}\" is encrypted, but was expected not to be.", filePath));
      }
   }
}
