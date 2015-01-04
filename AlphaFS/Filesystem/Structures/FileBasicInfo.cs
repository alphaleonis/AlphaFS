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

using System.IO;
using System.Runtime.InteropServices;
//using FILETIME = System.Runtime.InteropServices.ComTypes.FILETIME;

namespace Alphaleonis.Win32.Filesystem
{
   internal static partial class NativeMethods
   {
      /// <summary>
      ///   FILE_BASIC_INFO structure - Contains the basic information for a file.
      ///   <para>Used for file handles.</para>
      /// </summary>
      /// <remarks>
      ///   <para>Specifying -1 for <see cref="LastAccessTime"/>, <see cref="ChangeTime"/>, or <see cref="LastWriteTime"/></para>
      ///   <para>indicates that operations on the current handle should not affect the given field.</para>
      ///   <para>(I.e, specifying -1 for <see cref="LastWriteTime"/> will leave the <see cref="LastWriteTime"/> unaffected by writes performed
      ///   on the current handle.)</para>
      /// </remarks>
      [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
      internal struct FileBasicInfo
      {
         /// <summary>The time the file was created in <see cref="NativeMethods.FileTime"/> format,
         /// <para>which is a 64-bit value representing the number of 100-nanosecond intervals since January 1, 1601 (UTC).</para>
         /// </summary>
         public FileTime CreationTime;

         /// <summary>The time the file was last accessed in <see cref="NativeMethods.FileTime"/> format.</summary>
         public FileTime LastAccessTime;

         /// <summary>The time the file was last written to in <see cref="NativeMethods.FileTime"/> format.</summary>
         public FileTime LastWriteTime;

         /// <summary>The time the file was changed in <see cref="NativeMethods.FileTime"/> format.</summary>
         public FileTime ChangeTime;

         /// <summary>The file attributes.</summary>
         /// <remarks>If this is set to 0 in a <see cref="NativeMethods.FileBasicInfo"/> structure passed to SetFileInformationByHandle then none of the attributes are changed.</remarks>
         public FileAttributes FileAttributes;
      }
   }
}