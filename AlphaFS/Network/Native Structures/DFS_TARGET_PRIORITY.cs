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

using System.Runtime.InteropServices;

namespace Alphaleonis.Win32.Network
{
   internal static partial class NativeMethods
   {
      /// <summary>Contains the priority class and rank of a specific DFS target.</summary>
      /// <remarks>Minimum supported client: Windows Vista</remarks>
      /// <remarks>Minimum supported server: Windows Server 2008, Windows Server 2003 with SP1</remarks>
      [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
      internal struct DFS_TARGET_PRIORITY
      {
         /// <summary>DFS_TARGET_PRIORITY_CLASS enumeration value that specifies the priority class of the target.</summary>
         [MarshalAs(UnmanagedType.I4)] public readonly DfsTargetPriorityClass TargetPriorityClass;

         /// <summary>Specifies the priority rank value of the target. The default value is 0, which indicates the highest priority rank within a priority class.</summary>
         public readonly ushort TargetPriorityRank;

         /// <summary>This member is reserved and must be zero.</summary>
         public readonly ushort Reserved;
      }
   }
}
