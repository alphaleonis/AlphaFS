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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using Alphaleonis.Win32.Filesystem;
using Directory = Alphaleonis.Win32.Filesystem.Directory;
using File = Alphaleonis.Win32.Filesystem.File;
using DirectoryInfo = Alphaleonis.Win32.Filesystem.DirectoryInfo;
using Path = Alphaleonis.Win32.Filesystem.Path;

namespace AlphaFS.UnitTest
{
   partial class DirectoryTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>

      [TestMethod]
      public void Directory_EnumerateAlternateDataStreams_LocalAndUNC_Success()
      {
         Directory_EnumerateAlternateDataStreams(false);
         Directory_EnumerateAlternateDataStreams(true);
      }




      private void Directory_EnumerateAlternateDataStreams(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         string tempPath = Path.GetTempPath("Directory-EnumerateAlternateDataStreams-" + Path.GetRandomFileName());
         if (isNetwork)
            tempPath = Path.LocalToUnc(tempPath);

         const int defaultStreamsDirectory = 0; // The default number of data streams for a folder.

         Console.WriteLine("\nInput Directory Path: [{0}]", tempPath);
         Console.WriteLine("\nA directory is created and {0} streams are added.", UnitTestConstants.AllStreams.Count());


         try
         {
            var di = new DirectoryInfo(tempPath);
            di.Create();

            var currentNumberofStreams = di.EnumerateAlternateDataStreams().Count();

            Assert.AreEqual(defaultStreamsDirectory, currentNumberofStreams, "Total amount of default streams do not match.");
            Assert.AreEqual(currentNumberofStreams, Directory.EnumerateAlternateDataStreams(tempPath).Count(), "Total amount of Directory.EnumerateAlternateDataStreams() streams do not match.");
            Assert.AreEqual(currentNumberofStreams, di.EnumerateAlternateDataStreams().Count(), "Total amount of DirectoryInfo() streams do not match.");


            // Create alternate data streams.
            // Because of the colon, you must supply a full path and use PathFormat.FullPath or PathFormat.LongFullPath,
            // to prevent a: "NotSupportedException: path is in an invalid format." exception.

            File.WriteAllLines(tempPath + Path.StreamSeparator + UnitTestConstants.MyStream, UnitTestConstants.StreamArrayContent, PathFormat.FullPath);
            File.WriteAllText(tempPath + Path.StreamSeparator + UnitTestConstants.MyStream2, UnitTestConstants.StreamStringContent, PathFormat.FullPath);

            
            var newNumberofStreams = Directory.EnumerateAlternateDataStreams(tempPath).Count();
            Console.WriteLine("\n\nCurrent stream Count(): [{0}]", newNumberofStreams);


            // Enumerate all streams from the folder.
            foreach (AlternateDataStreamInfo stream in di.EnumerateAlternateDataStreams())
            {
               Assert.IsTrue(UnitTestConstants.Dump(stream, -10));
            }


            // Show the contents of our streams.
            Console.WriteLine();
            foreach (string streamName in UnitTestConstants.AllStreams)
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
            Console.WriteLine("\n\tCaught (unexpected) {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
            Assert.IsTrue(false);
         }
         finally
         {
            Directory.Delete(tempPath);
            Assert.IsFalse(Directory.Exists(tempPath), "Cleanup failed: Directory should have been removed.");
         }

         Console.WriteLine();
      }
   }
}
