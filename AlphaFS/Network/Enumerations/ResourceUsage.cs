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

using System;

namespace Alphaleonis.Win32.Network
{
   internal static partial class NativeMethods
   {
      /// <summary>NETRESOURCE structure.
      ///   <para>ResourceUsage: A set of bit flags describing how the resource can be used.</para>
      /// </summary>
      /// <remarks>Note that this member can be specified only if the <see cref="ResourceScope"/> member is equal to <see cref="ResourceScope.GlobalNet"/>.</remarks>
      [Flags]
      internal enum ResourceUsage
      {
         /// <summary>RESOURCEUSAGE_CONNECTABLE
         /// <para>The resource is a connectable resource.</para>
         /// <para>The name pointed to by the lpRemoteName member can be passed to the WNetAddConnection function to make a network connection.</para>
         /// </summary>
         Connectable = 1,

         /// <summary>RESOURCEUSAGE_CONTAINER
         /// <para>The resource is a container resource.</para>
         /// <para>The name pointed to by the lpRemoteName member can be passed to the WNetAddConnection function to make a network connection.</para>
         /// </summary>
         Container = 2,

         /// <summary>RESOURCEUSAGE_NOLOCALDEVICE
         /// <para>The resource is not a local device.</para>
         /// </summary>
         NoLocalDevice = 4,

         /// <summary>RESOURCEUSAGE_SIBLING
         /// <para>The resource is a sibling.</para>
         /// <para>This value is not used by Windows.</para>
         /// </summary>
         Sibling = 8,

         /// <summary>RESOURCEUSAGE_ATTACHED
         /// <para>The resource must be attached.</para>
         /// <para>This value specifies that a function to enumerate this resource should fail</para>
         /// <para>if the caller is not authenticated, even if the network permits enumeration without authentication.</para>
         /// </summary>
         Attached = 16,


         /// <summary>RESOURCEUSAGE_ALL
         /// <para>Setting this value is equivalent to setting: <see cref="ResourceUsage.Connectable"/>, <see cref="ResourceUsage.Container"/>, and <see cref="ResourceUsage.Attached"/>.</para>
         /// </summary>
         All = (Connectable | Container | Attached)
      }
   }
}