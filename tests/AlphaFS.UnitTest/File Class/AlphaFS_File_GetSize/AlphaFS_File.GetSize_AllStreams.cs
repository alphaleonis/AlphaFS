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
   partial class SizeTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void AlphaFS_File_GetSize_AllStreams_LocalAndNetwork_Success()
      {
         AlphaFS_File_GetSize_AllStreams(false);
         AlphaFS_File_GetSize_AllStreams(true);
      }
      

      private void AlphaFS_File_GetSize_AllStreams(bool isNetwork)
      {
         using (var tempRoot = new TemporaryDirectory(isNetwork))
         {
            var file = tempRoot.CreateFile();

            Console.WriteLine("Input File Path: [{0:N0} bytes ({1})] [{2}]\n", file.Length, Alphaleonis.Utils.UnitSizeToText(file.Length), file.FullName);


            var totalStreams = new Random(DateTime.UtcNow.Millisecond).Next(0, 10);
            var allStreamsSize = 0;
            var streamsCount = 0;

            for (var i = 0; i < totalStreams; i++)
            {
               var streamName = file.FullName + ":myStream" + i;

               var streamText = new string('X', new Random(DateTime.UtcNow.Millisecond).Next(0, 5 * UnitTestConstants.OneMebibyte));

               allStreamsSize += streamText.Length;
               streamsCount++;

               Console.WriteLine("\tAdd stream: [{0:N0} bytes ({1})] [{2}]", streamText.Length, Alphaleonis.Utils.UnitSizeToText(streamText.Length), streamName);


               // Create alternate data streams.
               // Because of the colon, you must supply a full path and use PathFormat.FullPath or PathFormat.LongFullPath,
               // to prevent a: "NotSupportedException: path is in an invalid format." exception.

               Alphaleonis.Win32.Filesystem.File.WriteAllText(streamName, streamText, Alphaleonis.Win32.Filesystem.PathFormat.FullPath);
            }


            Console.WriteLine("\n\tTotal added Stream count: [{0}] size: [{1:N0} bytes ({2})]", streamsCount, allStreamsSize, Alphaleonis.Utils.UnitSizeToText(allStreamsSize));


            var fileSizeWithStreams = Alphaleonis.Win32.Filesystem.File.GetSize(file.FullName, true);

            Console.WriteLine("\n\tTotal File size + {0} streams: [{1:N0} bytes ({2})]", streamsCount, fileSizeWithStreams, Alphaleonis.Utils.UnitSizeToText(fileSizeWithStreams));

            Assert.AreEqual(file.Length + allStreamsSize, fileSizeWithStreams, "The file sizes do not match, but it is expected.");
         }

         Console.WriteLine();
      }
   }
}
