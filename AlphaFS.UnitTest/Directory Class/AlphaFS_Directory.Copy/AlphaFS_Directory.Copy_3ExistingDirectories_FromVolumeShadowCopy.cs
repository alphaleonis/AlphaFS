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
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AlphaFS.UnitTest
{
   public partial class CopyTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void AlphaFS_Directory_Copy_3ExistingDirectories_FromVolumeShadowCopy_LocalAndNetwork_Success()
      {
         AlphaFS_Directory_Copy_3ExistingDirectories_FromVolumeShadowCopy(false);
         AlphaFS_Directory_Copy_3ExistingDirectories_FromVolumeShadowCopy(true);
      } 


      private void AlphaFS_Directory_Copy_3ExistingDirectories_FromVolumeShadowCopy(bool isNetwork)
      {
         var foundShadowCopy = false;
         
         using (var tempRoot = new TemporaryDirectory(isNetwork))
         {
            var folder = tempRoot.CreateDirectoryRandomizedAttributes();

            var dosDevices = Alphaleonis.Win32.Filesystem.Volume.QueryAllDosDevices().Where(device => device.StartsWith("HarddiskVolumeShadowCopy", StringComparison.OrdinalIgnoreCase)).ToArray();

            foreach (var dosDevice in dosDevices)
            {
               if (foundShadowCopy)
                  break;

               var shadowSource = Alphaleonis.Win32.Filesystem.Path.GlobalRootDevicePrefix + dosDevice;

               var sourceFolder = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);

               var drive = System.IO.Directory.GetDirectoryRoot(sourceFolder).TrimEnd('\\');

               var globalRoot = sourceFolder.Replace(drive, shadowSource);


               var dirInfo = new Alphaleonis.Win32.Filesystem.DirectoryInfo(globalRoot);

               Console.WriteLine("Input GlobalRoot Path: [{0}]\n", dirInfo.FullName);

               if (!dirInfo.Exists)
                  UnitTestAssert.InconclusiveBecauseFileNotFound("No volume shadow copy found.");


               var enumOptions = Alphaleonis.Win32.Filesystem.DirectoryEnumerationOptions.Recursive | Alphaleonis.Win32.Filesystem.DirectoryEnumerationOptions.SkipReparsePoints;
               var copyOptions = Alphaleonis.Win32.Filesystem.CopyOptions.None;

               var copyCount = 0;
                  
               foreach (var fsi in dirInfo.EnumerateDirectories(enumOptions))
               {
                  if (copyCount == 3)
                     break;


                  // Copy folders from "\Program Files".

                  var folderCopy = System.IO.Path.Combine(folder.FullName, System.IO.Path.GetFileName(fsi.FullName));

                  Console.WriteLine("Copy directory #{0}.", copyCount + 1);


                  var cmr = Alphaleonis.Win32.Filesystem.Directory.Copy(fsi.FullName, folderCopy, copyOptions);


                  UnitTestConstants.Dump(cmr);
                  Console.WriteLine();


                  Assert.AreEqual((int) Alphaleonis.Win32.Win32Errors.NO_ERROR, cmr.ErrorCode);


                  foundShadowCopy = true;

                  copyCount++;
               }
            }
         }


         Console.WriteLine();


         if (!foundShadowCopy)
            UnitTestAssert.InconclusiveBecauseFileNotFound("No volume shadow copy found.");
      }
   }
}
