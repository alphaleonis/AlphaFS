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
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AlphaFS.UnitTest
{
   public partial class AlphaFS_FileSystemEntryInfoTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void AlphaFS_Directory_EnumerateFileSystemEntryInfos_FolderWithSpaceAsName_LocalAndNetwork_Success()
      {
         Directory_EnumerateFileSystemEntryInfos_FolderWithSpaceAsName(false);
         Directory_EnumerateFileSystemEntryInfos_FolderWithSpaceAsName(true);
      }


      [TestMethod]
      public void AlphaFS_Directory_EnumerateFileSystemEntryInfos_TypeFileSystemEntryInfo_LocalAndNetwork_Success()
      {
         Directory_EnumerateFileSystemEntryInfos_TypeFileSystemEntryInfo(false);
         Directory_EnumerateFileSystemEntryInfos_TypeFileSystemEntryInfo(true);
      }


      [TestMethod]
      public void AlphaFS_Directory_EnumerateFileSystemEntryInfos_TypeDirectoryInfo_LocalAndNetwork_Success()
      {
         Directory_EnumerateFileSystemEntryInfos_TypeDirectoryInfo(false);
         Directory_EnumerateFileSystemEntryInfos_TypeDirectoryInfo(true);
      }


      [TestMethod]
      public void AlphaFS_Directory_EnumerateFileSystemEntryInfos_TypeFileInfo_LocalAndNetwork_Success()
      {
         Directory_EnumerateFileSystemEntryInfos_TypeFileInfo(false);
         Directory_EnumerateFileSystemEntryInfos_TypeFileInfo(true);
      }


      [TestMethod]
      public void AlphaFS_Directory_EnumerateFileSystemEntryInfos_TypeString_LocalAndNetwork_Success()
      {
         Directory_EnumerateFileSystemEntryInfos_TypeString(false);
         Directory_EnumerateFileSystemEntryInfos_TypeString(true);
      }




      private void Directory_EnumerateFileSystemEntryInfos_FolderWithSpaceAsName(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         var tempPath = System.IO.Path.GetTempPath();
         if (isNetwork)
            tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);


         using (var rootDir = new TemporaryDirectory(tempPath, MethodBase.GetCurrentMethod().Name))
         {
            var folder = rootDir.Directory.FullName;

            Console.WriteLine("\nInput Directory Path: [{0}]\n", folder);


            var maxFolder = 10;
            UnitTestConstants.CreateDirectoriesAndFiles(folder, maxFolder / 2, false, false, false);


            for (var i = 0; i < maxFolder / 2; i++)
            {
               var spaceFolder = folder + @"\" + new string(' ', i + 1) + @"\" + "no_void";

               Alphaleonis.Win32.Filesystem.Directory.CreateDirectory(spaceFolder, Alphaleonis.Win32.Filesystem.PathFormat.LongFullPath);
            }



            var countNamedFolders = 0;
            var countSpaceFolders = 0;

            foreach (var fsei in Alphaleonis.Win32.Filesystem.Directory.EnumerateFileSystemEntryInfos<Alphaleonis.Win32.Filesystem.FileSystemInfo>(folder))
            {
               var path = fsei.FullName;

               Console.WriteLine("\tDirectory: [{0}]", path);


               if (char.IsWhiteSpace(path[path.Length - 1]))
               {
                  countSpaceFolders++;
                  Assert.IsTrue(fsei.Exists);
               }

               else
                  countNamedFolders++;
            }


            Assert.AreEqual(maxFolder, countNamedFolders + countSpaceFolders);
         }

         Console.WriteLine();
      }


      private void Directory_EnumerateFileSystemEntryInfos_TypeFileSystemEntryInfo(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         var tempPath = UnitTestConstants.SysRoot;
         if (isNetwork)
            tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);


         Console.WriteLine("\nInput Directory Path: [{0}]\n", tempPath);

         var count = 0;
         foreach (var fsei in Alphaleonis.Win32.Filesystem.Directory.EnumerateFileSystemEntryInfos<Alphaleonis.Win32.Filesystem.FileSystemEntryInfo>(tempPath))
         {
            Assert.IsTrue(fsei.GetType().IsEquivalentTo(typeof(Alphaleonis.Win32.Filesystem.FileSystemEntryInfo)));
            count++;
         }

         Assert.IsTrue(count > 0, "Nothing is enumerated, but it is expected.");
      }


      private void Directory_EnumerateFileSystemEntryInfos_TypeDirectoryInfo(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         var tempPath = UnitTestConstants.SysRoot;
         if (isNetwork)
            tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);


         Console.WriteLine("\nInput Directory Path: [{0}]\n", tempPath);

         var count = 0;
         foreach (var fsei in Alphaleonis.Win32.Filesystem.Directory.EnumerateFileSystemEntryInfos<Alphaleonis.Win32.Filesystem.DirectoryInfo>(tempPath, Alphaleonis.Win32.Filesystem.DirectoryEnumerationOptions.Folders))
         {
            Assert.IsTrue(fsei.GetType().IsEquivalentTo(typeof(Alphaleonis.Win32.Filesystem.DirectoryInfo)));
            count++;
         }

         Assert.IsTrue(count > 0, "Nothing is enumerated, but it is expected.");
      }


      private void Directory_EnumerateFileSystemEntryInfos_TypeFileInfo(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         var tempPath = UnitTestConstants.SysRoot;
         if (isNetwork)
            tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);


         Console.WriteLine("\nInput Directory Path: [{0}]\n", tempPath);

         var count = 0;
         foreach (var fsei in Alphaleonis.Win32.Filesystem.Directory.EnumerateFileSystemEntryInfos<Alphaleonis.Win32.Filesystem.FileInfo>(tempPath, Alphaleonis.Win32.Filesystem.DirectoryEnumerationOptions.Files))
         {
            Assert.IsTrue(fsei.GetType().IsEquivalentTo(typeof(Alphaleonis.Win32.Filesystem.FileInfo)));
            count++;
         }

         Assert.IsTrue(count > 0, "Nothing is enumerated, but it is expected.");
      }


      private void Directory_EnumerateFileSystemEntryInfos_TypeString(bool isNetwork)
      {
         UnitTestConstants.PrintUnitTestHeader(isNetwork);

         var tempPath = UnitTestConstants.SysRoot;
         if (isNetwork)
            tempPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempPath);


         Console.WriteLine("\nInput Directory Path: [{0}]\n", tempPath);

         var count = 0;
         foreach (var fsei in Alphaleonis.Win32.Filesystem.Directory.EnumerateFileSystemEntryInfos<string>(tempPath))
         {
            Assert.IsTrue(fsei.GetType().IsEquivalentTo(typeof(string)));
            count++;
         }

         Assert.IsTrue(count > 0, "Nothing is enumerated, but it is expected.");
      }
   }
}
