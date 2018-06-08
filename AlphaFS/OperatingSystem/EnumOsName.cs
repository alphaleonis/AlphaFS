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

using System.Diagnostics.CodeAnalysis;

namespace Alphaleonis.Win32
{
   /// <summary>[AlphaFS] Static class providing access to information about the operating system under which the assembly is executing.</summary>
   public static partial class OperatingSystem
   {
      /// <summary>[AlphaFS] A set of flags that describe the named Windows versions.</summary>
      /// <remarks>The values of the enumeration are ordered. A later released operating system version has a higher number, so comparisons between named versions are meaningful.</remarks>
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Os")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Os")]
      public enum EnumOsName
      {
         /// <summary>A Windows version earlier than Windows 2000.</summary>
         Earlier = -1,

         /// <summary>Windows 2000 (Server or Professional).</summary>
         Windows2000 = 0,

         /// <summary>Windows XP.</summary>
         WindowsXP = 1,

         /// <summary>Windows Server 2003.</summary>
         WindowsServer2003 = 2,

         /// <summary>Windows Vista.</summary>
         WindowsVista = 3,

         /// <summary>Windows Server 2008.</summary>
         WindowsServer2008 = 4,

         /// <summary>Windows 7.</summary>
         Windows7 = 5,

         /// <summary>Windows Server 2008 R2.</summary>
         WindowsServer2008R2 = 6,

         /// <summary>Windows 8.</summary>
         Windows8 = 7,

         /// <summary>Windows Server 2012.</summary>
         WindowsServer2012 = 8,

         /// <summary>Windows 8.1.</summary>
         Windows81 = 9,

         /// <summary>Windows Server 2012 R2</summary>
         WindowsServer2012R2 = 10,

         /// <summary>Windows 10</summary>
         Windows10 = 11,

         /// <summary>Windows Server 2016</summary>
         WindowsServer2016 = 12,

         /// <summary>A later version of Windows than currently installed.</summary>
         Later = 65535
      }
   }
}
