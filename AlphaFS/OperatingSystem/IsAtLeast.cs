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

namespace Alphaleonis.Win32
{
   /// <summary>[AlphaFS] Static class providing access to information about the operating system under which the assembly is executing.</summary>
   public static partial class OperatingSystem
   {
      /// <summary>[AlphaFS] Determines whether the operating system is of the specified version or later.</summary>
      /// <returns><c>true</c> if the operating system is of the specified <paramref name="version"/> or later; otherwise, <c>false</c>.</returns>      
      /// <param name="version">The lowest version for which to return true.</param>
      public static bool IsAtLeast(EnumOsName version)
      {
         return VersionName >= version;
      }


      /// <summary>[AlphaFS] Determines whether the operating system is of the specified version or later, allowing specification of a minimum service pack that must be installed on the lowest version.</summary>
      /// <returns><c>true</c> if the operating system matches the specified <paramref name="version"/> with the specified service pack, or if the operating system is of a later version; otherwise, <c>false</c>.</returns>      
      /// <param name="version">The minimum required version.</param>
      /// <param name="servicePackVersion">The major version of the service pack that must be installed on the minimum required version to return true. This can be 0 to indicate that no service pack is required.</param>
      public static bool IsAtLeast(EnumOsName version, int servicePackVersion)
      {
         return IsAtLeast(version) && ServicePackVersion.Major >= servicePackVersion;
      }
   }
}
