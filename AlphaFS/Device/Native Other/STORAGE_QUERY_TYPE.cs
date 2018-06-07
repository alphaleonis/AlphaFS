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

namespace Alphaleonis.Win32.Device
{
   internal static partial class NativeMethods
   {
      /// <summary>Used by the <see cref="STORAGE_PROPERTY_QUERY"/> structure passed to the IOCTL_STORAGE_QUERY_PROPERTY control code to indicate what information is returned about a property of a storage device or adapter.
      /// <para>Minimum supported client: Windows XP [desktop apps only]</para>
      /// <para>Minimum supported server: Windows Server 2003 [desktop apps only]</para>
      /// </summary>
      public enum STORAGE_QUERY_TYPE
      {
         /// <summary>Instructs the driver to return an appropriate descriptor.</summary>
         PropertyStandardQuery = 0

         ///// <summary>Instructs the driver to report whether the descriptor is supported.</summary>
         //PropertyExistsQuery = 1,

         ///// <summary>Not currently supported. Do not use.</summary>
         //PropertyMaskQuery = 2,

         ///// <summary>Specifies the upper limit of the list of query types. This is used to validate the query type.</summary>
         //PropertyQueryMaxDefined = 3
      }
   }
}
