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

using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AlphaFS.UnitTest
{
   public static class FileAssert
   {
      public static void Exists(string filePath)
      {
         if (!System.IO.File.Exists(filePath))
            throw new AssertFailedException(string.Format(CultureInfo.CurrentCulture, "The file: [{0}] does not exist, but is expected to.", filePath));
      }


      public static void AreEqual(string expectedFilePath, string actualFilePath)
      {
         var position = 0;
         using (var s1 = System.IO.File.OpenRead(expectedFilePath))
         using (var s2 = System.IO.File.OpenRead(actualFilePath))
         {
            if (s1.Length != s2.Length)
               throw new AssertFailedException(string.Format(CultureInfo.CurrentCulture, "The files: [{0}] and \"{1}\" are not equal but are expected to be. Their size differs.", expectedFilePath, actualFilePath));

            var a = s1.ReadByte();
            var b = s2.ReadByte();

            if (a != b)
               throw new AssertFailedException(string.Format(CultureInfo.CurrentCulture, "The files: [{0}] and \"{1}\" are not equal but are expected to be. The first difference was at position: [{2}]", expectedFilePath, actualFilePath, position));

            position++;
         }
      }


      public static void AreNotEqual(string expectedFilePath, string actualFilePath)
      {
         var position = 0;
         using (var s1 = System.IO.File.OpenRead(expectedFilePath))
         using (var s2 = System.IO.File.OpenRead(actualFilePath))
         {
            if (s1.Length != s2.Length)
               return;

            var a = s1.ReadByte();
            var b = s2.ReadByte();

            if (a != b)
               return;

            position++;
         }

         throw new AssertFailedException(string.Format(CultureInfo.CurrentCulture, "The files: [{0}] and \"{1}\" are equal but are not expected to be.", expectedFilePath, actualFilePath));
      }


      public static void IsEncrypted(string filePath)
      {
         if ((System.IO.File.GetAttributes(filePath) & System.IO.FileAttributes.Encrypted) == 0)
            throw new AssertFailedException(string.Format(CultureInfo.CurrentCulture, "The file: [{0}] is not encrypted, but is expected to.", filePath));
      }


      public static void IsNotEncrypted(string filePath)
      {
         if ((System.IO.File.GetAttributes(filePath) & System.IO.FileAttributes.Encrypted) != 0)
            throw new AssertFailedException(string.Format(CultureInfo.CurrentCulture, "The file: [{0}] is encrypted, but is expected not to.", filePath));
      }


      public static void IsCompressed(string filePath)
      {
         if ((System.IO.File.GetAttributes(filePath) & System.IO.FileAttributes.Compressed) == 0)
            throw new AssertFailedException(string.Format(CultureInfo.CurrentCulture, "The file: [{0}] is not compressed, but is expected to.", filePath));
      }


      public static void IsNotCompressed(string filePath)
      {
         if ((System.IO.File.GetAttributes(filePath) & System.IO.FileAttributes.Compressed) != 0)
            throw new AssertFailedException(string.Format(CultureInfo.CurrentCulture, "The file: [{0}] is compressed, but is expected not to.", filePath));
      }
   }
}
