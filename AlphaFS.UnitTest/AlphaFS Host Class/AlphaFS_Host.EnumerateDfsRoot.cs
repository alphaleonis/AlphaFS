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
using System.Net.NetworkInformation;

namespace AlphaFS.UnitTest
{
   public partial class EnumerationTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void AlphaFS_Host_EnumerateDfsRoot_Network_Success()
      {
         UnitTestConstants.PrintUnitTestHeader(true);


         var cnt = 0;
         var noDomainConnection = true;

         // Drill down to get servers from the first namespace retrieved.

         try
         {
            foreach (var dfsName in Alphaleonis.Win32.Network.Host.EnumerateDomainDfsRoot())
            {
               noDomainConnection = false;

               Console.Write("#{0:000}\tDFS Root: [{1}]\n", ++cnt, dfsName);

               try
               {
                  var dfsInfo = Alphaleonis.Win32.Network.Host.GetDfsInfo(dfsName);

                  foreach (var storage in dfsInfo.StorageInfoCollection)
                  {
                     var cnt2 = 0;
                     Console.Write("\n\tEnumerating DFS Namespaces from host: [{0}]\n", storage.ServerName);

                     foreach (var dfsNamespace in Alphaleonis.Win32.Network.Host.EnumerateDfsRoot(storage.ServerName, true))
                        Console.Write("\t#{0:000}\tDFS Root: [{1}]\n", ++cnt2, dfsNamespace);
                  }
               }
               catch (NetworkInformationException ex)
               {
                  Console.WriteLine("NetworkInformationException #1: [{0}]", ex.Message.Replace(Environment.NewLine, "  "));
               }
               catch (Exception ex)
               {
                  Console.WriteLine("\n\tCaught (UNEXPECTED #1) {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
               }

               Console.WriteLine();
            }

            if (cnt == 0)
               UnitTestAssert.InconclusiveBecauseResourcesAreUnavailable();
         }
         catch (NetworkInformationException ex)
         {
            Console.WriteLine("NetworkInformationException #2: [{0}]", ex.Message.Replace(Environment.NewLine, "  "));
         }
         catch (Exception ex)
         {
            Console.WriteLine("Caught (UNEXPECTED #2) {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
         }

         
         if (noDomainConnection)
            UnitTestAssert.Inconclusive("Test ignored because the computer is either not connected to a domain or no DFS root exists.");

         else if (cnt == 0)
            UnitTestAssert.InconclusiveBecauseResourcesAreUnavailable();


         Console.WriteLine();
      }
   }
}
