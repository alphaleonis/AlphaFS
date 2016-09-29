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
      /// <summary>The display options for the network object in a network browsing user interface.</summary>
      internal enum ResourceDisplayType
      {
         /// <summary>RESOURCEDISPLAYTYPE_GENERIC
         /// <para>The method used to display the object does not matter.</para>
         /// </summary>
         Generic = 0,

         /// <summary>RESOURCEDISPLAYTYPE_DOMAIN
         /// <para>The object should be displayed as a domain.</para>
         /// </summary>
         Domain = 1,

         /// <summary>RESOURCEDISPLAYTYPE_SERVER
         /// <para>The object should be displayed as a server.</para>
         /// </summary>
         Server = 2,

         /// <summary>RESOURCEDISPLAYTYPE_SHARE
         /// <para>The object should be displayed as a share</para>
         /// </summary>
         Share = 3,

         /// <summary>RESOURCEDISPLAYTYPE_FILE
         /// <para>The object should be displayed as a file.</para>
         /// </summary>
         File = 4,

         /// <summary>RESOURCEDISPLAYTYPE_GROUP
         /// <para>The object should be displayed as a group.</para>
         /// </summary>
         Group = 5,

         /// <summary>RESOURCEDISPLAYTYPE_NETWORK
         /// <para>The object should be displayed as a network.</para>
         /// </summary>
         Network = 6,

         /// <summary>RESOURCEDISPLAYTYPE_ROOT
         /// <para>The object should be displayed as a logical root for the entire network.</para>
         /// </summary>
         Root = 7,

         /// <summary>RESOURCEDISPLAYTYPE_SHAREADMIN
         /// <para>The object should be displayed as a administrative share.</para>
         /// </summary>
         ShareAdmin = 8,

         /// <summary>RESOURCEDISPLAYTYPE_DIRECTORY
         /// <para>The object should be displayed as a directory.</para>
         /// </summary>
         Directory = 9,

         /// <summary>RESOURCEDISPLAYTYPE_TREE
         /// <para>The object should be displayed as a tree.</para>
         /// </summary>
         Tree = 10,

         /// <summary>RESOURCEDISPLAYTYPE_NDSCONTAINER
         /// <para>The object should be displayed as a Netware Directory Service container.</para>
         /// </summary>
         NdsContainer = 11
      }
   }
}