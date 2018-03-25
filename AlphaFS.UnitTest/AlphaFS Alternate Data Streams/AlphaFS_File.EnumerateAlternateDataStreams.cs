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
using System.Linq;
using Alphaleonis.Win32.Filesystem;
using File = Alphaleonis.Win32.Filesystem.File;
using FileInfo = Alphaleonis.Win32.Filesystem.FileInfo;
using Path = Alphaleonis.Win32.Filesystem.Path;

namespace AlphaFS.UnitTest
{
   public partial class AlphaFS_AlternateDataStreamsTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>

      [TestMethod]
      public void AlphaFS_File_EnumerateAlternateDataStreams_LocalAndNetwork_Success()
      {
         File_EnumerateAlternateDataStreams(false);
         File_EnumerateAlternateDataStreams(true);
      }




      private void File_EnumerateAlternateDataStreams(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         var tempPath = Path.GetTempPath("File-EnumerateAlternateDataStreams-" + Path.GetRandomFileName());
         if (isNetwork)
            tempPath = Path.LocalToUnc(tempPath);

         const int defaultStreamsFile = 1; // The default number of data streams for a file.

         Console.WriteLine("\nInput File Path: [{0}]", tempPath);
         Console.WriteLine("\nA file is created and {0} streams are added.", UnitTestConstants.AllStreams.Length);


         try
         {
            // Create file and add 10 characters to it, file is created in ANSI format.
            File.WriteAllText(tempPath, UnitTestConstants.TenNumbers);


            var fi = new FileInfo(tempPath);

            var currentNumberofStreams = fi.EnumerateAlternateDataStreams().Count();

            Assert.AreEqual(defaultStreamsFile, currentNumberofStreams, "Total amount of default streams do not match.");
            Assert.AreEqual(currentNumberofStreams, File.EnumerateAlternateDataStreams(tempPath).Count(), "Total amount of File.EnumerateAlternateDataStreams() streams do not match.");


            var fileSize = File.GetSize(tempPath);
            Assert.AreEqual(UnitTestConstants.TenNumbers.Length, fileSize);


            // Create alternate data streams.
            // Because of the colon, you must supply a full path and use PathFormat.FullPath or PathFormat.LongFullPath,
            // to prevent a: "NotSupportedException: path is in an invalid format." exception.

            File.WriteAllLines(tempPath + Path.StreamSeparator + UnitTestConstants.MyStream, UnitTestConstants.StreamArrayContent, PathFormat.FullPath);
            File.WriteAllText(tempPath + Path.StreamSeparator + UnitTestConstants.MyStream2, UnitTestConstants.StreamStringContent, PathFormat.FullPath);

            
            var newNumberofStreams = File.EnumerateAlternateDataStreams(tempPath).Count();
            Console.WriteLine("\n\nCurrent stream Count(): [{0}]", newNumberofStreams);


            // Enumerate all streams from the file.
            foreach (var stream in fi.EnumerateAlternateDataStreams())
            {
               Assert.IsTrue(UnitTestConstants.Dump(stream, -10));

               // The default stream, a file as we know it.
               if (Alphaleonis.Utils.IsNullOrWhiteSpace(stream.StreamName))
                  Assert.AreEqual(fileSize, stream.Size);
            }


            // Show the contents of our streams.
            Console.WriteLine();
            foreach (var streamName in UnitTestConstants.AllStreams)
            {
               Console.WriteLine("\n\tStream name: [{0}]", streamName);

               // Because of the colon, you must supply a full path and use PathFormat.FullPath or PathFormat.LongFullPath,
               // to prevent a: "NotSupportedException: path is in an invalid format." exception.

               foreach (var line in File.ReadAllLines(tempPath + Path.StreamSeparator + streamName, PathFormat.FullPath))
                  Console.WriteLine("\t\t{0}", line);
            }
         }
         catch (Exception ex)
         {
            Console.WriteLine("\n\tCaught (UNEXPECTED) {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
            Assert.IsTrue(false);
         }
         finally
         {
            File.Delete(tempPath);
            Assert.IsFalse(File.Exists(tempPath), "Cleanup failed: File should have been removed.");
         }

         Console.WriteLine();
      }
   }
}
