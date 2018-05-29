using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AlphaFS.UnitTest
{
   public partial class CopyTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void AlphaFS_File_Copy_Overwrite_DestinationFileAlreadyExists_LocalAndNetwork_Success()
      {
         AlphaFS_File_Copy_Overwrite_DestinationFileAlreadyExists(false);
         AlphaFS_File_Copy_Overwrite_DestinationFileAlreadyExists(true);
      }

      
      private void AlphaFS_File_Copy_Overwrite_DestinationFileAlreadyExists(bool isNetwork)
      {
         using (var tempRoot = new TemporaryDirectory(isNetwork))
         {
            var srcFile = UnitTestConstants.CreateFile(tempRoot.Directory.FullName);
            var dstFile = tempRoot.RandomTxtFileFullPath;

            Console.WriteLine("Src File Path: [{0}]", srcFile);
            Console.WriteLine("Dst File Path: [{0}]", dstFile);


            System.IO.File.Copy(srcFile.FullName, dstFile);

            ExceptionAssert.IOException(() => System.IO.File.Copy(srcFile.FullName, dstFile));
            
            ExceptionAssert.AlreadyExistsException(() => Alphaleonis.Win32.Filesystem.File.Copy(srcFile.FullName, dstFile));
            

            Alphaleonis.Win32.Filesystem.File.Copy(srcFile.FullName, dstFile, true);


            Assert.IsTrue(System.IO.File.Exists(srcFile.FullName), "The file does not exists, but is expected to.");

            Assert.IsTrue(System.IO.File.Exists(dstFile), "The file does not exists, but is expected to.");
         }


         Console.WriteLine();
      }
   }
}
