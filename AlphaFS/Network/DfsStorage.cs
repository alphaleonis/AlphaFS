/* Copyright (c) 2008-2015 Peter Palotas, Jeffrey Jangli, Normalex
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

using System.Diagnostics.CodeAnalysis;

namespace Alphaleonis.Win32.Network
{
   /// <summary>
   /// <para>Contains information about a DFS root or link target in a DFS namespace</para>
   /// <para>or from the cache maintained by the DFS client.</para>
   /// <para>Win32: DFS_STORAGE_INFO structure.</para>
   /// </summary>
   [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dfs")]
   public sealed class DfsStorage
   {
      #region Constructor

      /// <summary>Initializes a new instance of the <see cref="DfsStorage"/> class,
      /// <para>which acts as a wrapper for a DFS root or link target.</para>
      /// </summary>
      public DfsStorage()
      {
      }

      /// <summary>Initializes a new instance of the <see cref="DfsStorage"/> class,
      /// <para>which acts as a wrapper for a DFS root or link target.</para>
      /// </summary>
      /// <param name="structure">An initialized <see cref="NativeMethods.DfsStorageInfo"/> instance.</param>
      internal DfsStorage(NativeMethods.DfsStorageInfo structure)
      {
         ServerName = structure.ServerName;
         ShareName = structure.ShareName;
         State = structure.State;
      }

      #endregion // Constructor

      #region Properties

      #region ServerName

      /// <summary>The server name of the DFS root target or link target.</summary>
      public string ServerName { get; private set; }

      #endregion // ServerName

      #region ShareName

      /// <summary>The share name of the DFS root target or link target.</summary>
      public string ShareName { get; private set; }

      #endregion // ShareName

      #region State

      /// <summary>An <see cref="DfsStorageStates"/> enum of the DFS root target or link target.</summary>
      public DfsStorageStates State { get; private set; }

      #endregion // State

      #endregion // Properties
   }
}