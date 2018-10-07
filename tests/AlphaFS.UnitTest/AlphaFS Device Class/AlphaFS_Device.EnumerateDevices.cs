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
using System.Collections.Generic;
using System.Linq;

namespace AlphaFS.UnitTest
{
   public partial class EnumerationTest
   {
      // Pattern: <class>_<function>_<scenario>_<expected result>


      [TestMethod]
      public void AlphaFS_Device_EnumerateDevices_Local_Success()
      {
         Console.WriteLine("\nMSDN Note: Beginning in Windows 8 and Windows Server 2012 functionality to access remote machines has been removed.");
         Console.WriteLine("You cannot access remote machines when running on these versions of Windows.\n");

         AlphaFS_Device_EnumerateDevices();
      }


      private void AlphaFS_Device_EnumerateDevices()
      {
         UnitTestConstants.PrintUnitTestHeader(false);

         var tempPath = Environment.MachineName;
         var classCnt = 0;

         foreach (var deviceClass in EnumMemberToList<Alphaleonis.Win32.Filesystem.DeviceGuid>())
         {
            Console.WriteLine("#{0:000}\tClass: [{1}]", ++classCnt, deviceClass);

            foreach (var device in Alphaleonis.Win32.Filesystem.Device.EnumerateDevices(tempPath, deviceClass))
               UnitTestConstants.Dump(device);

            Console.WriteLine();
         }

         if (classCnt == 0)
            UnitTestAssert.InconclusiveBecauseResourcesAreUnavailable();
      }


      private static IEnumerable<T> EnumMemberToList<T>()
      {
         var enumType = typeof(T);

         // Can't use generic type constraints on value types, so have to do check like this.
         if (enumType.BaseType != typeof(Enum))
            throw new ArgumentException("T must be of type System.Enum", "T");


         var enumValArray = Enum.GetValues(enumType).Cast<T>().OrderBy(e => e.ToString()).ToList();
         var enumValList = new List<T>(enumValArray.Count);

         enumValList.AddRange(enumValArray.Select(val => (T) Enum.Parse(enumType, val.ToString())));

         return enumValList;
      }
   }
}
