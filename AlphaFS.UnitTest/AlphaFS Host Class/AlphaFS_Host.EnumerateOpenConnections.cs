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
   public partial class EnumerationTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void AlphaFS_Host_EnumerateOpenConnections_Local_Success()
      {
         var host = Environment.MachineName;
         var share = UnitTestConstants.SysDrive[0] + Alphaleonis.Win32.Filesystem.Path.NetworkDriveSeparator;

         // Create an active connection to the "remote" host.
         var currentDir = System.IO.Directory.GetCurrentDirectory();

         // This might cause other unit tests to fail with ArgumentException: Illegal characters in path.
         System.IO.Directory.SetCurrentDirectory(Alphaleonis.Win32.Filesystem.Path.UncPrefix + host + Alphaleonis.Win32.Filesystem.Path.DirectorySeparator + share);


         EnumerateOpenConnections(host, share);


         System.IO.Directory.SetCurrentDirectory(currentDir);
      }




      private void EnumerateOpenConnections(string host, string share)
      {
         UnitTestAssert.IsElevatedProcess();
         UnitTestConstants.PrintUnitTestHeader(false);
         
         Console.WriteLine(@"Connected to Share: [\\{0}\{1}]", host, share);
         

         var count = 0;
         foreach (var openConnectionInfo in Alphaleonis.Win32.Network.Host.EnumerateOpenConnections(host, share))
         {
            UnitTestConstants.Dump(openConnectionInfo);

            Assert.IsNotNull(openConnectionInfo);

            count++;

            Console.WriteLine();
         }


         if (count == 0)
            UnitTestAssert.InconclusiveBecauseResourcesAreUnavailable();
      }
   }
}
