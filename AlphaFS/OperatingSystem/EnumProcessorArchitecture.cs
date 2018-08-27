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
      /// <summary>[AlphaFS] A set of flags to indicate the current processor architecture for which the operating system is targeted and running.</summary>
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Pa")]
      [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]      
      public enum EnumProcessorArchitecture 
      {
         /// <summary>PROCESSOR_ARCHITECTURE_INTEL
         /// <para>The system is running a 32-bit version of Windows.</para>
         /// </summary>
         X86 = 0,

         /// <summary>PROCESSOR_ARCHITECTURE_IA64
         /// <para>The system is running on a Itanium processor.</para>
         /// </summary>
         [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ia")]
         [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Ia")]
         IA64 = 6,

         /// <summary>PROCESSOR_ARCHITECTURE_AMD64
         /// <para>The system is running a 64-bit version of Windows.</para>
         /// </summary>
         X64 = 9,

         /// <summary>PROCESSOR_ARCHITECTURE_UNKNOWN
         /// <para>Unknown architecture.</para>
         /// </summary>
         Unknown = 65535
      }
   }
}
