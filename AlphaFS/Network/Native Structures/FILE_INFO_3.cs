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
      /// <summary>Contains the identifier and other pertinent information about files, devices, and pipes.</summary>
      /// <remarks>This structure is only for use with the NetFileEnum function.</remarks>
      /// <remarks>Minimum supported client: Windows XP [desktop apps only]</remarks>
      /// <remarks>Minimum supported server: Windows Server 2003 [desktop apps only]</remarks>
      [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
      internal struct FILE_INFO_3
      {
         /// <summary>The identification number assigned to the resource when it is opened.</summary>
         [MarshalAs(UnmanagedType.U4)] public readonly uint fi3_id;

         /// <summary>The access permissions associated with the opening application. This member can be one or more of the following <see cref="AccessPermissions"/> values.</summary>
         [MarshalAs(UnmanagedType.U4)] public readonly AccessPermissions fi3_permissions;

         /// <summary>The number of file locks on the file, device, or pipe.</summary>
         [MarshalAs(UnmanagedType.U4)] public readonly uint fi3_num_locks;

         /// <summary>The path of the opened resource.</summary>
         [MarshalAs(UnmanagedType.LPWStr)] public readonly string fi3_pathname;

         /// <summary>Specifies which user (on servers that have user-level security) or which computer (on servers that have share-level security) opened the resource.</summary>
         [MarshalAs(UnmanagedType.LPWStr)] public readonly string fi3_username;
      }
   }
}
