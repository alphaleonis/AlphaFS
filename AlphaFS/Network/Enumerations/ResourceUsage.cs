/* Copyright (c) 2008-2014 Peter Palotas, Jeffrey Jangli, Normalex
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
      /// <summary>A set of bit flags describing how the resource can be used.</summary>
      [Flags]
      internal enum ResourceUsage
      {
         /// <summary>No ResourceUsage options used.</summary>
         None = 0,

         /// <summary>RESOURCEUSAGE_CONNECTABLE (0x00000001) - The resource is a connectable resource.</summary>
         Connectable = 1,

         /// <summary>RESOURCEUSAGE_CONTAINER (0x00000002) - The resource is a container resource.</summary>
         Container = 2,

         /// <summary>RESOURCEUSAGE_NOLOCALDEVICE (0x00000004) - The resource is not a local device.</summary>
         NoLocalDevice = 4,

         /// <summary>RESOURCEUSAGE_SIBLING (0x00000008) - The resource is a sibling.</summary>
         Sibling = 8,

         /// <summary>RESOURCEUSAGE_ATTACHED (0x00000010) - The resource must be attached. This value specifies that a function to enumerate resource this should fail if the caller is not authenticated, even if the network permits enumeration without authentication.</summary>
         Attached = 16,

         /// <summary>A combination of <see cref="T:Connectable"/>, <see cref="T:Container"/> and <see cref="T:Attached"/> flags.</summary>
         All = (Connectable | Container | Attached)
      }
   }
}