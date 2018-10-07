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

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AlphaFS.UnitTest
{
   public partial class PathTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void AlphaFS_Path_LocalToUnc_Success()
      {
         UnitTestConstants.PrintUnitTestHeader(false);
         
         var sysDrive = UnitTestConstants.SysDrive;
         var windowsFolder = Environment.GetFolderPath(Environment.SpecialFolder.Windows);
         var backslash = Alphaleonis.Win32.Filesystem.Path.DirectorySeparator;
         var hostName = Environment.MachineName + backslash + sysDrive[0] + Alphaleonis.Win32.Filesystem.Path.NetworkDriveSeparator;
         var uncPrefix = Alphaleonis.Win32.Filesystem.Path.UncPrefix;
         var uncLongPrefix = Alphaleonis.Win32.Filesystem.Path.LongPathUncPrefix;
         

         string[] localToUncPaths =
         {
            sysDrive, 
            uncPrefix + hostName,
            uncLongPrefix + hostName,

            sysDrive + backslash,
            uncPrefix + hostName + backslash,
            uncLongPrefix + hostName + backslash,

            windowsFolder,
            uncPrefix + hostName + backslash + System.IO.Path.GetFileName(windowsFolder),
            uncLongPrefix + hostName + backslash + System.IO.Path.GetFileName(windowsFolder),

            windowsFolder + backslash + "TempPath" + backslash,
            uncPrefix + hostName + backslash + System.IO.Path.GetFileName(windowsFolder) + backslash + "TempPath" + backslash,
            uncLongPrefix + hostName + backslash + System.IO.Path.GetFileName(windowsFolder) + backslash + "TempPath" + backslash
         };


         var totalPaths = localToUncPaths.Length;
         
         for (var i = 0; i < totalPaths; i += 3)
         {
            var localPath = localToUncPaths[0 + i];
            var shouldBeUncPath = localToUncPaths[1 + i];
            var shouldBeUncLongPath = localToUncPaths[2 + i];

            var uncPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(localPath);
            var uncLongPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(localPath, Alphaleonis.Win32.Filesystem.GetFullPathOptions.AsLongPath);

            Console.WriteLine("\tLocal Path   : [{0}]", localPath);
            Console.WriteLine("\tUNC Path     : [{0}]", uncPath);
            Console.WriteLine("\tUNC Long Path: [{0}]", uncLongPath);


            Assert.AreEqual(shouldBeUncPath, uncPath, "The UNC paths do not match, but are expected to.");

            Assert.AreEqual(shouldBeUncLongPath, uncLongPath, "The UNC paths do not match, but are expected to.");


            if (i % 2 != 0)
               Assert.IsTrue(uncPath.EndsWith(backslash), "The UNC path does not end with a " + backslash + " but is expected to.");


            Console.WriteLine();
         }
      }
   }
}
