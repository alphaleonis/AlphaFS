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

using Alphaleonis.Win32.Network;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AlphaFS.UnitTest
{
   public partial class AlphaFS_HostTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void AlphaFS_Host_GetHostShareFromPath_Network_Success()
      {
         UnitTestConstants.PrintUnitTestHeader(true);

         var folderWithspaces = @"\Folder with spaces";


         // Local Path.
         var uncPath = UnitTestConstants.SysDrive;
         var hostAndShare = Host.GetHostShareFromPath(uncPath);
         Console.WriteLine("Input local path: [{0}]", uncPath);
         Console.WriteLine("\tResult == null: {0}", null == hostAndShare);

         Assert.AreEqual(null, hostAndShare);


         Console.WriteLine();


         // Local Path.
         uncPath = UnitTestConstants.SysDrive + folderWithspaces;
         hostAndShare = Host.GetHostShareFromPath(uncPath);
         Console.WriteLine("Input local path: [{0}]", uncPath);
         Console.WriteLine("\tResult == null: {0}", null == hostAndShare);

         Assert.AreEqual(null, hostAndShare);


         Console.WriteLine();


         // Local Path.
         uncPath = Alphaleonis.Win32.Filesystem.Path.GetLongPath(Environment.SystemDirectory + folderWithspaces);
         hostAndShare = Host.GetHostShareFromPath(uncPath);
         Console.WriteLine("Input local path: [{0}]", uncPath);
         Console.WriteLine("\tResult == null: {0}", null == hostAndShare);
         Assert.AreEqual(null, hostAndShare);


         Console.WriteLine();


         // Network Path as regular path.
         uncPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(UnitTestConstants.SysDrive);
         hostAndShare = Host.GetHostShareFromPath(uncPath);
         Console.WriteLine("Input UNC path: [{0}]", uncPath);
         Console.WriteLine("\tHost : [{0}]", hostAndShare[0]);
         Console.WriteLine("\tShare: [{0}]", hostAndShare[1]);

         Assert.IsTrue(hostAndShare[1].EndsWith(Alphaleonis.Win32.Filesystem.Path.NetworkDriveSeparator));
         Assert.AreEqual(Environment.MachineName, hostAndShare[0].ToUpperInvariant());

         
         Console.WriteLine();


         // Network Path as regular path.
         uncPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(UnitTestConstants.SysDrive + @"\");
         hostAndShare = Host.GetHostShareFromPath(uncPath);
         Console.WriteLine("Input UNC path: [{0}]", uncPath);
         Console.WriteLine("\tHost : [{0}]", hostAndShare[0]);
         Console.WriteLine("\tShare: [{0}]", hostAndShare[1]);

         Assert.IsTrue(hostAndShare[1].EndsWith(@"\"));
         Assert.AreEqual(Environment.MachineName, hostAndShare[0].ToUpperInvariant());


         Console.WriteLine();
         

         // Network Path as regular path.
         uncPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(UnitTestConstants.SysDrive + folderWithspaces);
         hostAndShare = Host.GetHostShareFromPath(uncPath);
         Console.WriteLine("Input UNC path: [{0}]", uncPath);
         Console.WriteLine("\tHost : [{0}]", hostAndShare[0]);
         Console.WriteLine("\tShare: [{0}]", hostAndShare[1]);

         Assert.AreEqual(Environment.MachineName, hostAndShare[0].ToUpperInvariant());


         Console.WriteLine();


         // Network Path as long path.
         uncPath = Alphaleonis.Win32.Filesystem.Path.LocalToUnc(Environment.SystemDirectory + folderWithspaces, Alphaleonis.Win32.Filesystem.GetFullPathOptions.AsLongPath);
         hostAndShare = Host.GetHostShareFromPath(uncPath);
         Console.WriteLine("Input UNC path: [{0}]", uncPath);
         Console.WriteLine("\tHost : [{0}]", hostAndShare[0]);
         Console.WriteLine("\tShare: [{0}]", hostAndShare[1]);

         Assert.IsFalse(hostAndShare[0].Contains("/"));
         Assert.AreEqual(Environment.MachineName, hostAndShare[0].ToUpperInvariant());
      }
   }
}
