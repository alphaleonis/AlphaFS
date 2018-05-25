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

namespace AlphaFS.UnitTest
{
   public partial class AlphaFS_AlternateDataStreamsTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>

      [TestMethod]
      public void AlphaFS_Directory_EnumerateAlternateDataStreams_LocalAndNetwork_Success()
      {
         Directory_EnumerateAlternateDataStreams(false);
         Directory_EnumerateAlternateDataStreams(true);
      }




      private void Directory_EnumerateAlternateDataStreams(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         var tempPath = Alphaleonis.Win32.Filesystem.Path.GetTempPath("Directory-EnumerateAlternateDataStreams-" + UnitTestConstants.GetRandomFileNameWithDiacriticCharacters());
         if (isNetwork)
            tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);

         const int defaultStreamsDirectory = 0; // The default number of data streams for a folder.

         Console.WriteLine("\nInput Directory Path: [{0}]", tempPath);
         Console.WriteLine("\nA directory is created and {0} streams are added.", UnitTestConstants.AllStreams.Count());


         try
         {
            var di = new Alphaleonis.Win32.Filesystem.DirectoryInfo(tempPath);
            di.Create();

            var currentNumberofStreams = di.EnumerateAlternateDataStreams().Count();

            Assert.AreEqual(defaultStreamsDirectory, currentNumberofStreams, "Total amount of default streams do not match.");
            Assert.AreEqual(currentNumberofStreams, Alphaleonis.Win32.Filesystem.Directory.EnumerateAlternateDataStreams(tempPath).Count(), "Total amount of Directory.EnumerateAlternateDataStreams() streams do not match.");


            // Create alternate data streams.
            // Because of the colon, you must supply a full path and use PathFormat.FullPath or PathFormat.LongFullPath,
            // to prevent a: "NotSupportedException: path is in an invalid format." exception.

            var stream1Name = tempPath + Alphaleonis.Win32.Filesystem.Path.StreamSeparator + UnitTestConstants.MyStream;
            var stream2Name = tempPath + Alphaleonis.Win32.Filesystem.Path.StreamSeparator + UnitTestConstants.MyStream2;

            Alphaleonis.Win32.Filesystem.File.WriteAllLines(stream1Name, UnitTestConstants.StreamArrayContent, Alphaleonis.Win32.Filesystem.PathFormat.FullPath);
            Alphaleonis.Win32.Filesystem.File.WriteAllText(stream2Name, UnitTestConstants.StreamStringContent, Alphaleonis.Win32.Filesystem.PathFormat.FullPath);


            var newNumberofStreams = Alphaleonis.Win32.Filesystem.Directory.EnumerateAlternateDataStreams(tempPath).Count();
            Console.WriteLine("\n\nCurrent stream Count(): [{0}]", newNumberofStreams);


            // Enumerate all streams from the folder.
            foreach (var stream in di.EnumerateAlternateDataStreams())
            {
               Assert.IsTrue(UnitTestConstants.Dump(stream, -10));
            }


            // Show the contents of our streams.
            Console.WriteLine();
            foreach (var streamName in UnitTestConstants.AllStreams)
            {
               Console.WriteLine("\n\tStream name: [{0}]", streamName);

               // Because of the colon, you must supply a full path and use PathFormat.FullPath or PathFormat.LongFullPath,
               // to prevent a: "NotSupportedException: path is in an invalid format." exception.

               foreach (var line in Alphaleonis.Win32.Filesystem.File.ReadAllLines(tempPath + Alphaleonis.Win32.Filesystem.Path.StreamSeparator + streamName, Alphaleonis.Win32.Filesystem.PathFormat.FullPath))
                  Console.WriteLine("\t\t{0}", line);
            }



            // Show FileInfo instance data of the streams.

            var fileInfo1 = new Alphaleonis.Win32.Filesystem.FileInfo(stream1Name, Alphaleonis.Win32.Filesystem.PathFormat.LongFullPath);
            var fileInfo2 = new Alphaleonis.Win32.Filesystem.FileInfo(stream2Name, Alphaleonis.Win32.Filesystem.PathFormat.LongFullPath);

            Console.WriteLine();
            UnitTestConstants.Dump(fileInfo1, -17);
            UnitTestConstants.Dump(fileInfo2, -17);


            Assert.AreEqual(UnitTestConstants.MyStream, fileInfo1.Name);
            Assert.AreEqual(UnitTestConstants.MyStream2, fileInfo2.Name);

            Assert.IsNull(fileInfo1.EntryInfo);
            Assert.IsNull(fileInfo2.EntryInfo);
         }
         catch (Exception ex)
         {
            Console.WriteLine("\n\tCaught (UNEXPECTED) {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
            Assert.IsTrue(false);
         }
         finally
         {
            Alphaleonis.Win32.Filesystem.Directory.Delete(tempPath);
            Assert.IsFalse(Alphaleonis.Win32.Filesystem.Directory.Exists(tempPath), "Cleanup failed: Directory should have been removed.");
         }

         Console.WriteLine();
      }
   }
}
