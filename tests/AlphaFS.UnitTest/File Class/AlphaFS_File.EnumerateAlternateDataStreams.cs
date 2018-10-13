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
using System.Globalization;
using System.Linq;

namespace AlphaFS.UnitTest
{
   public partial class EnumerationTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>

      [TestMethod]
      public void AlphaFS_File_EnumerateAlternateDataStreams_LocalAndNetwork_Success()
      {
         AlphaFS_File_EnumerateAlternateDataStreams(false);
         AlphaFS_File_EnumerateAlternateDataStreams(true);
      }
      

      private void AlphaFS_File_EnumerateAlternateDataStreams(bool isNetwork)
      {
         using (var tempRoot = new TemporaryDirectory(isNetwork))
         {
            var file = tempRoot.RandomTxtFileFullPath;

            Console.WriteLine("Input File Path: [{0}]", file);


            var myStream = "ӍƔŞtrëƛɱ-" + tempRoot.RandomString;
            var myStream2 = "myStreamTWO-" + tempRoot.RandomString;
            var allStreams = new[] {myStream, myStream2};
            var streamStringContent = "(1) Computer: [" + Environment.MachineName + "]" + "\tHello there, " + Environment.UserName;
            var tenNumbers = "0123456789";

            Console.WriteLine("\n\tA file is created and {0} streams are added.", allStreams.Length.ToString(CultureInfo.CurrentCulture));
            
            // Create file and add 10 characters to it, file is created in ANSI format.
            System.IO.File.WriteAllText(file, tenNumbers);


            var fi = new Alphaleonis.Win32.Filesystem.FileInfo(file);

            const int defaultStreamsFile = 1; // The default number of data streams for a file.
            var currentNumberofStreams = fi.EnumerateAlternateDataStreams().Count();

               
            Assert.AreEqual(defaultStreamsFile, currentNumberofStreams, "Total amount of default streams do not match.");
            Assert.AreEqual(currentNumberofStreams, Alphaleonis.Win32.Filesystem.File.EnumerateAlternateDataStreams(file).Count(), "Total amount of streams do not match.");


            var fileSize = Alphaleonis.Win32.Filesystem.File.GetSize(file);
            Assert.AreEqual(tenNumbers.Length, fileSize);


            // Create alternate data streams.
            // Because of the colon, you must supply a full path and use PathFormat.FullPath or PathFormat.LongFullPath,
            // to prevent a: "NotSupportedException: path is in an invalid format." exception.

            var stream1Name = file + Alphaleonis.Win32.Filesystem.Path.StreamSeparator + myStream;
            var stream2Name = file + Alphaleonis.Win32.Filesystem.Path.StreamSeparator + myStream2;

            Alphaleonis.Win32.Filesystem.File.WriteAllLines(stream1Name, UnitTestConstants.StreamArrayContent, Alphaleonis.Win32.Filesystem.PathFormat.FullPath);
            Alphaleonis.Win32.Filesystem.File.WriteAllText(stream2Name, streamStringContent, Alphaleonis.Win32.Filesystem.PathFormat.FullPath);
            



            var newNumberofStreams = Alphaleonis.Win32.Filesystem.File.EnumerateAlternateDataStreams(file).Count();
            Console.WriteLine("\n\tNew stream count: [{0}]", newNumberofStreams);


            // Enumerate all streams from the file.

            foreach (var stream in fi.EnumerateAlternateDataStreams())
            {
               UnitTestConstants.Dump(stream);

               Assert.IsNotNull(stream);

               // The default stream, a file as we know it.

               if (Alphaleonis.Utils.IsNullOrWhiteSpace(stream.StreamName))
                  Assert.AreEqual(fileSize, stream.Size);
            }


            // Show the contents of our streams.

            Console.WriteLine();
            foreach (var streamName in allStreams)
            {
               Console.WriteLine("\n\tStream name: [{0}]", streamName);


               // Because of the colon, you must supply a full path and use PathFormat.FullPath or PathFormat.LongFullPath,
               // to prevent a: "NotSupportedException: path is in an invalid format." exception.

               foreach (var line in Alphaleonis.Win32.Filesystem.File.ReadAllLines(file + Alphaleonis.Win32.Filesystem.Path.StreamSeparator + streamName, Alphaleonis.Win32.Filesystem.PathFormat.FullPath))
                  Console.WriteLine("\t\t{0}", line);
            }

            
            // Check FileInfo instance data of the streams.

            var fileInfo1 = new Alphaleonis.Win32.Filesystem.FileInfo(stream1Name, Alphaleonis.Win32.Filesystem.PathFormat.LongFullPath);
            var fileInfo2 = new Alphaleonis.Win32.Filesystem.FileInfo(stream2Name, Alphaleonis.Win32.Filesystem.PathFormat.LongFullPath);

            Assert.AreEqual(myStream, fileInfo1.Name);
            Assert.AreEqual(myStream2, fileInfo2.Name);

            Assert.IsNull(fileInfo1.EntryInfo);
            Assert.IsNull(fileInfo2.EntryInfo);
         }

         Console.WriteLine();
      }
   }
}
