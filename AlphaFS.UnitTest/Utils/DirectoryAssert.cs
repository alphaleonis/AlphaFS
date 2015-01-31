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

namespace AlphaFS.UnitTest
{
   public static class DirectoryAssert
   {
      public static void Exists(string directoryPath)
      {
         if (!Directory.Exists(directoryPath))
            throw new AssertFailedException(string.Format("The directory: [{0}] does not exist, but was expected to.", directoryPath));
      }

      public static void IsEncrypted(string dirPath)
      {
         var dir = new DirectoryInfo(dirPath);
         if ((dir.Attributes & System.IO.FileAttributes.Encrypted) == 0)
            throw new AssertFailedException(string.Format("The directory: [{0}] was not encrypted, but was expected to be.", dirPath));
      }

      public static void IsNotEncrypted(string dirPath)
      {
         var dir = new DirectoryInfo(dirPath);
         if ((dir.Attributes & System.IO.FileAttributes.Encrypted) != 0)
            throw new AssertFailedException(string.Format("The directory: [{0}] is encrypted, but was expected not to be.", dirPath));
      }
   }
}
