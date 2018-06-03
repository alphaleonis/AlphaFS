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
   partial class SizeTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void AlphaFS_Directory_GetSize_WithStreamsOnRootFolder_LocalAndNetwork_Success()
      {
         AlphaFS_Directory_GetSize_WithStreamsOnRootFolder(false);
         AlphaFS_Directory_GetSize_WithStreamsOnRootFolder(true);
      }
      

      private void AlphaFS_Directory_GetSize_WithStreamsOnRootFolder(bool isNetwork)
      {
         using (var tempRoot = new TemporaryDirectory(isNetwork))
         {
            var folder = tempRoot.CreateTree(5);

            Console.WriteLine("Input Directory Path: [{0}]", folder.FullName);


            var totalFolderSizeNoStreamsOnRootFolder = Alphaleonis.Win32.Filesystem.Directory.GetSize(folder.FullName, true);

            Console.WriteLine("\n\tTotal Directory size: [{0:N0} bytes ({1})]\n", totalFolderSizeNoStreamsOnRootFolder, Alphaleonis.Utils.UnitSizeToText(totalFolderSizeNoStreamsOnRootFolder));


            var props = Alphaleonis.Win32.Filesystem.Directory.GetProperties(folder.FullName, Alphaleonis.Win32.Filesystem.DirectoryEnumerationOptions.FilesAndFolders | Alphaleonis.Win32.Filesystem.DirectoryEnumerationOptions.Recursive);

            Assert.AreEqual(10, props["Total"], "The number of file system objects does not match, but it is expected.");
            Assert.AreEqual(props["Total"], props["Directory"] + props["File"], "The number of file system objects does not match, but it is expected.");
            Assert.AreEqual(5, props["Directory"], "The number of folders does not match, but it is expected.");
            Assert.AreEqual(5, props["File"], "The number of files does not match, but it is expected.");
            Assert.AreNotEqual(0, props["Size"], "The total file system objects size is zero, which is not expected.");




            var totalStreams = new Random(DateTime.UtcNow.Millisecond).Next(0, 9);

            var allStreamsSize = 0;

            for (var i = 0; i < totalStreams; i++)
            {
               var streamName = folder.FullName + ":myStream" + i;

               var streamText = new string((char)totalStreams, new Random(DateTime.UtcNow.Millisecond).Next(0, 5 * UnitTestConstants.OneMebibyte));

               allStreamsSize += streamText.Length;

               Console.WriteLine("\tAdd stream: [{0:N0} bytes ({1})] [{2}]", streamText.Length, Alphaleonis.Utils.UnitSizeToText(streamText.Length), streamName);


               // Create alternate data streams.
               // Because of the colon, you must supply a full path and use PathFormat.FullPath or PathFormat.LongFullPath,
               // to prevent a: "NotSupportedException: path is in an invalid format." exception.

               Alphaleonis.Win32.Filesystem.File.WriteAllText(streamName, streamText, Alphaleonis.Win32.Filesystem.PathFormat.FullPath);
            }


            var streams = Alphaleonis.Win32.Filesystem.Directory.EnumerateAlternateDataStreams(folder.FullName).ToArray();

            Console.WriteLine("\n\tTotal Stream count: [{0}] size: [{1:N0} bytes ({2})]", streams.Length, allStreamsSize, Alphaleonis.Utils.UnitSizeToText(allStreamsSize));


            var totalFolderSizeWithStreamsOnRootFolder = Alphaleonis.Win32.Filesystem.Directory.GetSize(folder.FullName, true);

            Console.WriteLine("\n\tTotal Directory size + {0} streams: [{1:N0} bytes ({2})]", totalStreams, totalFolderSizeWithStreamsOnRootFolder, Alphaleonis.Utils.UnitSizeToText(totalFolderSizeWithStreamsOnRootFolder));

            Assert.AreEqual(totalFolderSizeNoStreamsOnRootFolder + allStreamsSize, totalFolderSizeWithStreamsOnRootFolder, "The total folder size does not match, but it is expected.");
         }

         Console.WriteLine();
      }
   }
}
