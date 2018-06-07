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

namespace Alphaleonis.Win32.Device
{
   internal static partial class NativeMethods
   {
      /// <summary>Describes the geometry of disk devices and media.
      /// <para>Minimum supported client: Windows XP [desktop apps only]</para>
      /// <para>Minimum supported server: Windows Server 2003 [desktop apps only]</para>
      /// </summary>
      [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
      internal struct DISK_GEOMETRY
      {
         /// <summary>The number of cylinders.</summary>
         [MarshalAs(UnmanagedType.I8)] public readonly long Cylinders;

         /// <summary>The type of media.</summary>
         [MarshalAs(UnmanagedType.U4)] public readonly STORAGE_MEDIA_TYPE MediaType;

         /// <summary>The number of tracks per cylinder.</summary>
         [MarshalAs(UnmanagedType.U4)] public readonly uint TracksPerCylinder;

         /// <summary>The number of sectors per track.</summary>
         [MarshalAs(UnmanagedType.U4)] public readonly uint SectorsPerTrack;

         /// <summary>The number of bytes per sector.</summary>
         [MarshalAs(UnmanagedType.U4)] public readonly uint BytesPerSector;
      }
   }
}
