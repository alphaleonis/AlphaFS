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
   public static class DirectoryAssert
   {
      public static void Exists(string directoryPath)
      {
         if (!Directory.Exists(directoryPath))
            throw new AssertFailedException(String.Format("The directory \"{0}\" does not exist, but was expected to.", directoryPath));
      }

      public static void IsEncrypted(string dirPath)
      {
         DirectoryInfo dir = new DirectoryInfo(dirPath);
         if ((dir.Attributes & System.IO.FileAttributes.Encrypted) == 0)
            throw new AssertFailedException(String.Format("The directory \"{0}\" was not encrypted, but was expected to be.", dirPath));
      }

      public static void IsNotEncrypted(string dirPath)
      {
         DirectoryInfo dir = new DirectoryInfo(dirPath);
         if ((dir.Attributes & System.IO.FileAttributes.Encrypted) != 0)
            throw new AssertFailedException(String.Format("The directory \"{0}\" is encrypted, but was expected not to be.", dirPath));
      }
   }
}
