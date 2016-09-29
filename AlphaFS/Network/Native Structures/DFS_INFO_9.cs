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
using System.Runtime.InteropServices;

namespace Alphaleonis.Win32.Network
{
   internal static partial class NativeMethods
   {
      /// <summary>Contains the name, status, GUID, time-out, property flags, metadata size, DFS target information, link reparse point security descriptor, and a list of DFS targets for a root or link.</summary>
      /// <remarks>Minimum supported client: Windows Vista with SP1</remarks>
      /// <remarks>Minimum supported server: Windows Server 2008</remarks>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dfs")]
      [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
      internal struct DFS_INFO_9
      {
         /// <summary>The Universal Naming Convention (UNC) path of a DFS root or link.</summary>
         [MarshalAs(UnmanagedType.LPWStr)] public readonly string EntryPath;

         /// <summary>The comment associated with the DFS root or link.</summary>
         [MarshalAs(UnmanagedType.LPWStr)] public readonly string Comment;

         /// <summary>A <see cref="DfsVolumeStates"/> that specifies a set of bit flags that describe the DFS root or link.</summary>
         [MarshalAs(UnmanagedType.U4)] public readonly DfsVolumeStates State;

         /// <summary>Specifies the time-out, in seconds, of the DFS root or link.</summary>
         [MarshalAs(UnmanagedType.U4)] public readonly uint Timeout;

         /// <summary>Specifies the GUID of the DFS root or link.</summary>
         public readonly Guid Guid;

         /// <summary>Specifies a set of flags that describe specific properties of a DFS namespace, root, or link.</summary>
         [MarshalAs(UnmanagedType.U4)] public readonly DfsPropertyFlags PropertyFlags;

         /// <summary>For domain-based DFS namespaces, this member specifies the size of the corresponding Active Directory data blob, in bytes.
         /// For stand-alone DFS namespaces, this field specifies the size of the metadata stored in the registry,
         /// including the key names and value names, in addition to the specific data items associated with them.
         /// This field is valid for DFS roots only.
         /// </summary>
         [MarshalAs(UnmanagedType.U4)] public readonly uint MetadataSize;

         /// <summary>This member is reserved for system use.</summary>
         [MarshalAs(UnmanagedType.U4)] public readonly uint SdLengthReserved;

         /// <summary>Pointer to a SECURITY_DESCRIPTOR structure that specifies a self-relative security descriptor to be associated with the DFS link's reparse point.
         /// This field is valid for DFS links only.
         /// </summary>
         public IntPtr pSecurityDescriptor;
         
         /// <summary>Specifies the number of DFS targets.</summary>
         [MarshalAs(UnmanagedType.U4)] public readonly uint NumberOfStorages;

         /// <summary>An array of <see cref="DFS_STORAGE_INFO_1"/> structures.</summary>
         public readonly IntPtr Storage;
      }
   }
}
