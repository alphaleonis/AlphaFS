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
      /// <summary>Contains information about a DFS target, including the DFS target server name and share name as well as the target's state and priority.</summary>
      /// <remarks>Minimum supported client: Windows Vista</remarks>
      /// <remarks>Minimum supported server: Windows Server 2008, Windows Server 2003 with SP1</remarks>
      [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
      internal struct DFS_STORAGE_INFO_1
      {
         /// <summary>State of the target.</summary>
         [MarshalAs(UnmanagedType.U4)] public readonly DfsStorageStates State;

         /// <summary>The DFS root target or link target server name.</summary>
         [MarshalAs(UnmanagedType.LPWStr)] public readonly string ServerName;

         /// <summary>The DFS root target or link target share name.</summary>
         [MarshalAs(UnmanagedType.LPWStr)] public readonly string ShareName;

         /// <summary>DFS_TARGET_PRIORITY structure that contains a DFS target's priority class and rank.</summary>
         [MarshalAs(UnmanagedType.Struct)] public readonly DFS_TARGET_PRIORITY TargetPriority;
      }
   }
}
