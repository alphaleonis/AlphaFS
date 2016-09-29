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

using System.Diagnostics.CodeAnalysis;

namespace Alphaleonis.Win32.Network
{
   /// <summary>Contains information about a DFS root or link target in a DFS namespace or from the cache maintained by the DFS client.
   /// <para>This class cannot be inherited.</para>
   /// </summary>
   [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dfs")]
   public sealed class DfsStorageInfo
   {
      #region Constructor

      /// <summary>Initializes a new instance of the <see cref="DfsStorageInfo"/> class, which acts as a wrapper for a DFS root or link target.</summary>
      public DfsStorageInfo()
      {
      }

      /// <summary>Initializes a new instance of the <see cref="DfsStorageInfo"/> class, which acts as a wrapper for a DFS root or link target.</summary>
      /// <param name="structure">An initialized <see cref="NativeMethods.DFS_STORAGE_INFO_1"/> instance.</param>
      internal DfsStorageInfo(NativeMethods.DFS_STORAGE_INFO_1 structure)
      {
         ServerName = structure.ServerName;
         ShareName = structure.ShareName;

         State = structure.State;

         TargetPriorityClass = structure.TargetPriority.TargetPriorityClass;
         TargetPriorityRank = structure.TargetPriority.TargetPriorityRank;
      }

      #endregion // Constructor

      #region Methods

      /// <summary>The share name of the DFS root target or link target.</summary>
      /// <returns>A string that represents this instance.</returns>
      public override string ToString()
      {
         return ShareName;
      }

      #endregion // Methods

      #region Properties

      /// <summary>The server name of the DFS root target or link target.</summary>
      public string ServerName { get; private set; }

      /// <summary>The share name of the DFS root target or link target.</summary>
      public string ShareName { get; private set; }

      /// <summary>An <see cref="DfsStorageStates"/> enum of the DFS root target or link target.</summary>
      public DfsStorageStates State { get; private set; }

      /// <summary>Contains a DFS target's priority class and rank.</summary>
      public DfsTargetPriorityClass TargetPriorityClass { get; private set; }

      /// <summary>Specifies the priority rank value of the target. The default value is 0, which indicates the highest priority rank within a priority class.</summary>
      public int TargetPriorityRank { get; private set; }

      #endregion // Properties
   }
}
