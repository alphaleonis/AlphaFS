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
using System.Security.Principal;

namespace AlphaFS.UnitTest
{
   /// <summary>This is a test class for all ProcessContext class Unit Tests.</summary>
   [TestClass]
   public class AlphaFS_ProcessContextTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void AlphaFS_ProcessContext()
      {
         UnitTestConstants.PrintUnitTestHeader(false);

         var isAdmin = Alphaleonis.Win32.Security.ProcessContext.IsAdministrator;

         var isElevated = Alphaleonis.Win32.Security.ProcessContext.IsElevatedProcess;

         var isUacEnabled = Alphaleonis.Win32.Security.ProcessContext.IsUacEnabled;

         var IsWindowsService = Alphaleonis.Win32.Security.ProcessContext.IsWindowsService;


         Console.WriteLine("IsAdmin         : [{0}]", isAdmin);

         Console.WriteLine("IsElevated      : [{0}]", isElevated);

         Console.WriteLine("IsUacEnabled    : [{0}]", isUacEnabled);

         Console.WriteLine("IsWindowsService: [{0}]", IsWindowsService);


         // TODO: Test code is the same as in AlphaFS.
         Assert.AreEqual(new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator), isAdmin);

         
         // Assumption.
         Assert.IsFalse(IsWindowsService);
      }
   }
}
