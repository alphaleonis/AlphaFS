/*  Copyright (C) 2008-2016 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
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

namespace Alphaleonis.Win32.Network
{
   internal static partial class NativeMethods
   {
      /// <summary>NETRESOURCE structure.
      /// <para>ResourceScope: The scope of the enumeration.</para>
      /// </summary>
      internal enum ResourceScope
      {
         /// <summary>RESOURCE_CONNECTED
         /// <para>Enumerate all currently connected resources.</para>
         /// <para>The function ignores the <see cref="ResourceUsage"/> parameter.</para>
         /// </summary>
         Connected = 1,

         /// <summary>RESOURCE_GLOBALNET
         /// <para>Enumerate all resources on the network.</para>
         /// </summary>
         GlobalNet = 2,

         /// <summary>RESOURCE_REMEMBERED
         /// <para>Enumerate all remembered (persistent) connections.</para>
         /// <para>The function ignores the <see cref="ResourceUsage"/> parameter.</para>
         /// </summary>
         Remembered = 3,

         /// <summary>RESOURCE_RECENT</summary>
         Recent = 4,

         /// <summary>RESOURCE_CONTEXT
         /// <para>Enumerate only resources in the network context of the caller. Specify this value for a Network Neighborhood view.</para>
         /// <para>The function ignores the <see cref="ResourceUsage"/> parameter.</para>
         /// </summary>
         Context = 5
      }
   }
}