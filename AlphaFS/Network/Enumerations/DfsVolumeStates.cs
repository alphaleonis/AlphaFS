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
   /// <summary>A set of bit flags that describe the state of the DFS root or link;
   /// <para>the state of the DFS namespace root cannot be changed.</para>
   /// <para>One <see cref="DfsVolumeStates"/> flag is set, and one DFS_VOLUME_FLAVOR flag is set.</para>
   /// </summary>
   [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
   [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dfs")]
   [Flags]
   public enum DfsVolumeStates
   {
      /// <summary>No volume state.</summary>
      None = 0,

      /// <summary>DFS_VOLUME_STATE_OK
      /// <para>The specified DFS root or link is in the normal state.</para>
      /// </summary>
      Ok = 1,

      /// <summary>DFS_VOLUME_STATE_INCONSISTENT
      /// <para>The internal DFS database is inconsistent with the specified DFS root or link.</para>
      /// <para>Attempts to repair the inconsistency have failed.</para>
      /// </summary>
      Inconsistent = 2,

      /// <summary>DFS_VOLUME_STATE_OFFLINE
      /// <para>The specified DFS root or link is offline or unavailable.</para>
      /// </summary>
      Offline = 3,

      /// <summary>DFS_VOLUME_STATE_ONLINE
      /// <para>The specified DFS root or link is available.</para>
      /// </summary>
      Online = 4,

      /// <summary>DFS_VOLUME_FLAVOR_STANDALONE
      /// <para>The system sets this flag if the root is associated with a stand-alone DFS namespace.</para>
      /// </summary>
      /// <remarks>Windows XP: This value is not supported.</remarks>
      FlavorStandalone = 256,

      /// <summary>DFS_VOLUME_FLAVOR_AD_BLOB
      /// <para>The system sets this flag if the root is associated with a domain-based DFS namespace.</para>
      /// </summary>
      /// <remarks>Windows XP: This value is not supported.</remarks>
      FlavorAdBlob = 512
   }
}