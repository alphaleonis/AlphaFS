/*  Copyright (C) 2008-2017 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
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
   public partial class AlphaFS_HostTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>

      [TestMethod]
      public void AlphaFS_Host_EnumerateSessions_Local_Success()
      {
         var host = UnitTestConstants.LocalHost;
         
         EnumerateSessions(host);
      }




      private void EnumerateSessions(string host)
      {
         UnitTestConstants.PrintUnitTestHeader(false);
         
         Console.WriteLine("\nInput Host: [{0}]", host);


         var sessionCount = 0;

         foreach (var sessionsInfo in Alphaleonis.Win32.Network.Host.EnumerateSessions(host))
         {
            //Console.WriteLine("\n\t#{0:000}\tShare: [{1}]", ++cnt, shareInfo);

            if (UnitTestConstants.Dump(sessionsInfo, -16))
               sessionCount++;

            Console.WriteLine();
         }


         if (sessionCount == 0)
            Assert.Inconclusive("No sessions enumerated, but it is expected. Try another server name if applicable.");


         Console.WriteLine();
      }
   }
}
