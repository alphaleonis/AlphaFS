/* Copyright (C) 2008-2015 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
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
   /// <summary>The type of the shared resource.</summary>
   /// <remarks>MSDN "2.2.2.4 Share Types": http://msdn.microsoft.com/en-us/library/cc247110.aspx </remarks>
   [SuppressMessage("Microsoft.Naming", "CA1714:FlagsEnumsShouldHavePluralNames")]
   [SuppressMessage("Microsoft.Usage", "CA2217:DoNotMarkEnumsWithFlags")]
   [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
   [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
   [Flags]  // Needs Flags attribute to sometimes create combinations of: Ipc, Special.
   public enum ShareType
   {
      /// <summary>(0x00000000) - Disk drive</summary>
      DiskTree = 0,

      /// <summary>(0x00000001) - Print queue</summary>
      PrintQueue = 1,

      /// <summary>(0x00000002) - Communication device</summary>
      Device = 2,

      /// <summary>(0x00000003) - Interprocess communication (IPC)</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ipc")]
      Ipc = 3,

      /// <summary>(0x02000000) - A cluster share</summary>
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Fs")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Fs")]
      ClusterFs = 33554432,

      /// <summary>(0x04000000) - A Scale-Out cluster share</summary>
      [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Fs")]
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Fs")]
      ClusterSoFs = 67108864,

      /// <summary>(0x08000000) - A DFS share in a cluster</summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dfs")]
      ClusterDfs = 134217728,


      // The following table of values can be OR'd with the values in the preceding table to further specify the characteristics of a shared resource.
      // It is possible to use both values in this OR operation.

      /// <summary>(0x80000000) - Special share reserved for interprocess communication (IPC$) or remote administration of the server (ADMIN$). Can also refer to administrative shares such as C$, D$, E$, and so forth.</summary>
      Special = -2147483648,

      /// <summary>(0x40000000) - A temporary share that is not persisted for creation each time the file server initializes.</summary>
      Temporary = 1073741824
   }

   // uint STYPE_MASK = 0xF0000000;
}