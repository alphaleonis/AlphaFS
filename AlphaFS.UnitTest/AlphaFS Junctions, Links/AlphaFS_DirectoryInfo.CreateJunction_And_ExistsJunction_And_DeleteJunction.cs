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
   public partial class AlphaFS_JunctionsLinksTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void AlphaFS_DirectoryInfo_CreateJunction_And_ExistsJunction_And_DeleteJunction_Local_Success()
      {
         using (var tempRoot = new TemporaryDirectory())
         {
            var toDelete = tempRoot.Directory.CreateSubdirectory("ToDelete");
            var junction = System.IO.Path.Combine(toDelete.FullName, "MyJunctionPoint");
            var target = tempRoot.Directory.CreateSubdirectory("JunctionTarget");

            Console.WriteLine("Input Directory JunctionPoint  Path: [{0}]", junction);
            Console.WriteLine("Input Directory JunctionTarget Path: [{0}]", target);


            #region CreateJunction

            var dirInfo = new Alphaleonis.Win32.Filesystem.DirectoryInfo(target.FullName);


            // On success, dirInfo.FullName now points to the directory junction.
            // The DirectoryInfo instance is updated.
            dirInfo.CreateJunction(junction);

            Assert.AreEqual(dirInfo.FullName, junction, "It is expected that the DirectoryInfo points to the directory junction, but it is not.");




            var dirInfoSysIO = new System.IO.DirectoryInfo(junction);
            UnitTestConstants.Dump(dirInfoSysIO);

            Assert.IsTrue((dirInfoSysIO.Attributes & System.IO.FileAttributes.ReparsePoint) != 0);


            

            var lvi = Alphaleonis.Win32.Filesystem.Directory.GetLinkTargetInfo(dirInfo.FullName);
            UnitTestConstants.Dump(lvi);
            UnitTestConstants.Dump(dirInfo.EntryInfo);

            Assert.AreEqual(System.IO.Directory.Exists(dirInfo.FullName), Alphaleonis.Win32.Filesystem.Directory.Exists(dirInfo.FullName));
            Assert.AreEqual(junction, dirInfo.FullName);




            Assert.IsTrue(dirInfo.EntryInfo.IsDirectory);
            Assert.IsTrue(dirInfo.EntryInfo.IsMountPoint);
            Assert.IsTrue(dirInfo.EntryInfo.IsReparsePoint);
            Assert.IsFalse(dirInfo.EntryInfo.IsSymbolicLink);
            Assert.AreEqual(dirInfo.EntryInfo.ReparsePointTag, Alphaleonis.Win32.Filesystem.ReparsePointTag.MountPoint);




            // Create a folder in the junction and test the target.
            const string subFolder = "Test folder";
            dirInfo.CreateSubdirectory(subFolder);

            Assert.IsTrue(System.IO.Directory.Exists(System.IO.Path.Combine(target.FullName, subFolder)));

            #endregion // CreateJunction


            // ExistsJunction

            Assert.IsTrue(Alphaleonis.Win32.Filesystem.Directory.ExistsJunction(dirInfo.FullName), "It is expected that the directory junction exists, but is does not.");

            Assert.IsTrue(System.IO.Directory.Exists(dirInfo.FullName), "It is expected that the directory exists, but is does not.");


            #region DeleteJunction

            // Remove directory junction, target directory should remain.
            dirInfo.DeleteJunction();


            Assert.IsTrue(System.IO.Directory.Exists(dirInfo.FullName));
            Assert.IsTrue(System.IO.Directory.Exists(dirInfo.Parent.FullName));

            Assert.IsTrue(dirInfo.EntryInfo.IsDirectory);
            Assert.IsFalse(dirInfo.EntryInfo.IsMountPoint);
            Assert.IsFalse(dirInfo.EntryInfo.IsReparsePoint);
            Assert.IsFalse(dirInfo.EntryInfo.IsSymbolicLink);
            Assert.AreNotEqual(dirInfo.EntryInfo.ReparsePointTag, Alphaleonis.Win32.Filesystem.ReparsePointTag.MountPoint);

            #endregion // DeleteJunction


            // ExistsJunction

            Assert.IsFalse(Alphaleonis.Win32.Filesystem.Directory.ExistsJunction(dirInfo.FullName), "It is expected that the directory junction does not exist, but is does.");

            Assert.IsTrue(System.IO.Directory.Exists(dirInfo.FullName), "It is expected that the directory exists, but is does not.");
         }
      }
   }
}
