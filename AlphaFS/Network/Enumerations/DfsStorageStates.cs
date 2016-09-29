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
using System.Diagnostics.CodeAnalysis;

namespace Alphaleonis.Win32.Network
{
   /// <summary>A set of bit flags that describe the storage state of the DFS root or link target.</summary>
   [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dfs")]
   [Flags]
   public enum DfsStorageStates
   {
      /// <summary>No storage state.</summary>
      None = 0,

      /// <summary>DFS_STORAGE_STATE_OFFLINE
      /// <para>The DFS root or link target is offline.</para>
      /// </summary>
      /// <remarks>Windows Server 2003: The state of a root target cannot be set to DFS_STORAGE_STATE_OFFLINE.</remarks>
      Offline = 1,

      /// <summary>DFS_STORAGE_STATE_ONLINE
      /// <para>The DFS root or link target is online.</para>
      /// </summary>
      Online = 2,

      /// <summary>DFS_STORAGE_STATE_ACTIVE
      /// <para>The DFS root or link target is the active target.</para>
      /// </summary>
      Active = 4
   }
}