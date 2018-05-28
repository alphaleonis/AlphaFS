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
   /// <summary>This is a test class for all OperatingSystem class Unit Tests.</summary>
   [TestClass]
   public class AlphaFS_OperatingSystemTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void AlphaFS_OperatingSystem()
      {
         UnitTestConstants.PrintUnitTestHeader(false);

         Console.WriteLine("VersionName          : [{0}]", Alphaleonis.Win32.OperatingSystem.VersionName);
         Console.WriteLine("OsVersion            : [{0}]", Alphaleonis.Win32.OperatingSystem.OSVersion);
         Console.WriteLine("ServicePackVersion   : [{0}]", Alphaleonis.Win32.OperatingSystem.ServicePackVersion);
         Console.WriteLine("IsServer             : [{0}]", Alphaleonis.Win32.OperatingSystem.IsServer);
         Console.WriteLine("IsWow64Process       : [{0}]", Alphaleonis.Win32.OperatingSystem.IsWow64Process);
         Console.WriteLine("ProcessorArchitecture: [{0}]", Alphaleonis.Win32.OperatingSystem.ProcessorArchitecture);

         Console.WriteLine("\nOperatingSystem.IsAtLeast()\n");


         var isTrue = Alphaleonis.Win32.OperatingSystem.IsAtLeast(Alphaleonis.Win32.OperatingSystem.EnumOsName.Earlier);
         Console.WriteLine("\tOS Earlier           : [{0}]", isTrue);

         Assert.IsTrue(isTrue);


         isTrue = Alphaleonis.Win32.OperatingSystem.IsAtLeast(Alphaleonis.Win32.OperatingSystem.EnumOsName.Windows2000);
         Console.WriteLine("\tWindows 2000         : [{0}]", isTrue);

         Assert.IsTrue(isTrue);


         isTrue = Alphaleonis.Win32.OperatingSystem.IsAtLeast(Alphaleonis.Win32.OperatingSystem.EnumOsName.WindowsXP);
         Console.WriteLine("\tWindows XP           : [{0}]", isTrue);

         Assert.IsTrue(isTrue);


         Console.WriteLine("\tWindows Vista        : [{0}]", Alphaleonis.Win32.OperatingSystem.IsAtLeast(Alphaleonis.Win32.OperatingSystem.EnumOsName.WindowsVista));
         Console.WriteLine("\tWindows 7            : [{0}]", Alphaleonis.Win32.OperatingSystem.IsAtLeast(Alphaleonis.Win32.OperatingSystem.EnumOsName.Windows7));
         Console.WriteLine("\tWindows 8            : [{0}]", Alphaleonis.Win32.OperatingSystem.IsAtLeast(Alphaleonis.Win32.OperatingSystem.EnumOsName.Windows8));
         Console.WriteLine("\tWindows 8.1          : [{0}]", Alphaleonis.Win32.OperatingSystem.IsAtLeast(Alphaleonis.Win32.OperatingSystem.EnumOsName.Windows81));
         Console.WriteLine("\tWindows 10           : [{0}]", Alphaleonis.Win32.OperatingSystem.IsAtLeast(Alphaleonis.Win32.OperatingSystem.EnumOsName.Windows10));

         Console.WriteLine("\tWindows Server 2003  : [{0}]", Alphaleonis.Win32.OperatingSystem.IsAtLeast(Alphaleonis.Win32.OperatingSystem.EnumOsName.WindowsServer2003));
         Console.WriteLine("\tWindows Server 2008  : [{0}]", Alphaleonis.Win32.OperatingSystem.IsAtLeast(Alphaleonis.Win32.OperatingSystem.EnumOsName.WindowsServer2008));
         Console.WriteLine("\tWindows Server 2008R2: [{0}]", Alphaleonis.Win32.OperatingSystem.IsAtLeast(Alphaleonis.Win32.OperatingSystem.EnumOsName.WindowsServer2008R2));
         Console.WriteLine("\tWindows Server 2012  : [{0}]", Alphaleonis.Win32.OperatingSystem.IsAtLeast(Alphaleonis.Win32.OperatingSystem.EnumOsName.WindowsServer2012));
         Console.WriteLine("\tWindows Server 2012R2: [{0}]", Alphaleonis.Win32.OperatingSystem.IsAtLeast(Alphaleonis.Win32.OperatingSystem.EnumOsName.WindowsServer2012R2));
         Console.WriteLine("\tWindows Server 2016  : [{0}]", Alphaleonis.Win32.OperatingSystem.IsAtLeast(Alphaleonis.Win32.OperatingSystem.EnumOsName.WindowsServer2016));


         var isFalse = Alphaleonis.Win32.OperatingSystem.IsAtLeast(Alphaleonis.Win32.OperatingSystem.EnumOsName.Later);

         Console.WriteLine("\tOS Later             : [{0}]", isFalse);

         Assert.IsFalse(isFalse);
      }
   }
}
