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
      [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
      internal struct MountPointReparseBuffer
      {
         /// <summary>Offset, in bytes, of the substitute name string in the PathBuffer array.</summary>
         [MarshalAs(UnmanagedType.U2)] public ushort SubstituteNameOffset;

         /// <summary>Length, in bytes, of the substitute name string. If this string is null-terminated, SubstituteNameLength does not include space for the null character.</summary>
         [MarshalAs(UnmanagedType.U2)] public ushort SubstituteNameLength;

         /// <summary>Offset, in bytes, of the print name string in the PathBuffer array.</summary>
         [MarshalAs(UnmanagedType.U2)] public ushort PrintNameOffset;

         /// <summary>Length, in bytes, of the print name string. If this string is null-terminated, PrintNameLength does not include space for the null character. </summary>
         [MarshalAs(UnmanagedType.U2)] public ushort PrintNameLength;

         /// <summary>A buffer containing the unicode-encoded path string. The path string contains the substitute name string and print name string.</summary>
         [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
         public readonly byte[] data;
      }
   }
}
