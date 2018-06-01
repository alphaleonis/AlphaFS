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
      public void AlphaFS_Directory_CreateJunction_FromMappedDrive_ThrowsArgumentException_Netwerk_Success()
      {
         using (var tempRoot = new TemporaryDirectory())
         using (var connection = new Alphaleonis.Win32.Network.DriveConnection(Alphaleonis.Win32.Filesystem.Path.LocalToUnc(tempRoot.CreateDirectory().FullName)))
         {
            var mappedPath = connection.LocalName + @"\" + tempRoot.RandomDirectoryName;

            Console.WriteLine("Mapped drive [{0}] to [{1}]", connection.LocalName, connection.Share);

            var target = System.IO.Directory.CreateDirectory(mappedPath);

            var toDelete = tempRoot.Directory.CreateSubdirectory("ToDelete");

            var junction = System.IO.Path.Combine(toDelete.FullName, "JunctionPoint");
            
            UnitTestAssert.ThrowsException<ArgumentException>(() => Alphaleonis.Win32.Filesystem.Directory.CreateJunction(junction, target.FullName));
         }
      }
   }
}
