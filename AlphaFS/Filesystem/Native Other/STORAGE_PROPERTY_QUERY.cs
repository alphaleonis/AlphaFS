/*  Copyright (C) 2008-2018 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
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

namespace Alphaleonis.Win32.Filesystem
{
   internal static partial class NativeMethods
   {
      /// <summary>Indicates the properties of a storage device or adapter to retrieve as the input buffer passed to the IOCTL_STORAGE_QUERY_PROPERTY control code.</summary>
      [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
      public struct STORAGE_PROPERTY_QUERY
      {
         /// <summary>Indicates whether the caller is requesting a device descriptor, an adapter descriptor, a write cache property, a device unique ID (DUID),
         /// or the device identifiers provided in the device's SCSI vital product data (VPD) page. For a list of the property IDs that can be assigned to this member, see STORAGE_PROPERTY_ID.
         /// </summary>
         [MarshalAs(UnmanagedType.U4)] public STORAGE_PROPERTY_ID PropertyId;

         /// <summary>Contains flags indicating the type of query to be performed as enumerated by the STORAGE_QUERY_TYPE enumeration.
         /// PropertyStandardQuery = 0: Instructs the port driver to report a device descriptor, an adapter descriptor or a unique hardware device ID(DUID).
         /// PropertyExistsQuery   = 1: Instructs the port driver to report whether the descriptor is supported.
         /// </summary>
         [MarshalAs(UnmanagedType.U4)] public STORAGE_QUERY_TYPE QueryType;

         /// <summary>Contains an array of bytes that can be used to retrieve additional parameters for specific queries.</summary>
         [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
         public byte[] AdditionalParameters;
      }
   }
}
