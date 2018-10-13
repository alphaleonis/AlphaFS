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

namespace Alphaleonis.Win32.Network
{
   internal static partial class NativeMethods
   {
      /// <summary>NETRESOURCE structure.
      /// <para>ResourceType: The type of resource.</para>
      /// <remarks>
      /// <para>If a network provider cannot distinguish between</para>
      /// <para>print and disk resources, it can enumerate all resources.</para>
      /// </remarks>
      /// </summary>
      internal enum ResourceType
      {
         /// <summary>RESOURCETYPE_ANY
         /// <para>ResourceType: All resources.</para>
         /// <remarks>
         /// <para>If a network provider cannot distinguish between print and disk resources, it can enumerate all resources.</para>
         /// <para>This value cannot be combined with <see cref="Disk"/> or <see cref="ResourceType.Print"/>.</para>
         /// </remarks>
         /// </summary>
         Any = 0,

         /// <summary>RESOURCETYPE_DISK
         /// <para>All disk resources.</para>
         /// </summary>
         Disk = 1,

         /// <summary>RESOURCETYPE_PRINT
         /// <para>All print resources.</para>
         /// </summary>
         Print = 2
      }
   }
}