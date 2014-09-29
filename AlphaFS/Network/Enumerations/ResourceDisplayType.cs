/* Copyright (c) 2008-2014 Peter Palotas, Alexandr Normuradov, Jeffrey Jangli
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
      /// <summary>The display options for the network object in a network browsing user interface.</summary>
      internal enum ResourceDisplayType
      {
         /// <summary>RESOURCEDISPLAYTYPE_GENERIC (0x00000000) - The method used to display the object does not matter.</summary>
         Generic = 0,

         /// <summary>RESOURCEDISPLAYTYPE_DOMAIN (0x00000001) - The object should be displayed as a domain.</summary>
         Domain = 1,

         /// <summary>RESOURCEDISPLAYTYPE_SERVER (0x00000002) - The object should be displayed as a server.</summary>
         Server = 2,

         /// <summary>RESOURCEDISPLAYTYPE_SHARE (0x00000003) - The object should be displayed as a share.</summary>
         Share = 3,

         /// <summary>RESOURCEDISPLAYTYPE_FILE (0x00000004) - The object should be displayed as a file.</summary>
         File = 4,

         /// <summary>RESOURCEDISPLAYTYPE_GROUP (0x00000005) - The object should be displayed as a group.</summary>
         Group = 5,

         /// <summary>RESOURCEDISPLAYTYPE_NETWORK (0x00000006) - The object should be displayed as a network.</summary>
         Network = 6,

         /// <summary>RESOURCEDISPLAYTYPE_ROOT (0x00000007) - The object should be displayed as a logical root for the entire network.</summary>
         Root = 7,

         /// <summary>RESOURCEDISPLAYTYPE_SHAREADMIN (0x00000008) - The object should be displayed as a administrative share.</summary>
         ShareAdmin = 8,

         /// <summary>RESOURCEDISPLAYTYPE_DIRECTORY (0x00000009) - The object should be displayed as a directory.</summary>
         Directory = 9,

         /// <summary>RESOURCEDISPLAYTYPE_TREE (0x0000000A) - The object should be displayed as a tree.</summary>
         Tree = 10,

         /// <summary>RESOURCEDISPLAYTYPE_NDSCONTAINER (0x0000000B) - The object should be displayed as a Netware Directory Service container.</summary>
         Container = 11
      }
   }
}