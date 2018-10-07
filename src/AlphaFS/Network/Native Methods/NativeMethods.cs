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

using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Net;

namespace Alphaleonis.Win32.Network
{
   internal static partial class NativeMethods
   {
      #region Constants
      
      /// <summary>A constant of type DWORD that is set to –1. This value is valid as an input parameter to any method in section 3.1.4 that takes a PreferedMaximumLength parameter. When specified as an input parameter, this value indicates that the method MUST allocate as much space as the data requires.</summary>
      /// <remarks>MSDN "2.2.2.2 MAX_PREFERRED_LENGTH": http://msdn.microsoft.com/en-us/library/cc247107.aspx </remarks>
      internal const int MaxPreferredLength = -1;

      #endregion // Constants

      #region GetComputerDomain

      internal static readonly string ComputerDomain = GetComputerDomain();

      [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
      private static string GetComputerDomain(bool fdqn = false)
      {
         string domain = Environment.UserDomainName;
         string machine = Environment.MachineName.ToUpper(CultureInfo.InvariantCulture);

         try
         {
            if (fdqn)
            {
               domain = Dns.GetHostEntry("LocalHost").HostName.ToUpper(CultureInfo.InvariantCulture).Replace(machine + ".", string.Empty);
               domain = domain.Replace(machine, string.Empty);
            }
         }
         catch
         {
         }

         return domain;
      }

      #endregion // GetComputerDomain
   }
}